// ****************************************************************************
//
// Copyright (C) 2005-2026 Doom9 & al
//
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
//
// ****************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace MeGUI.core.gui
{
    /// <summary>
    /// Application wide theming: every window of MeGUI is themed automatically while the dark theme is in effect,
    /// and gets its original (designer) look back when the light theme is selected.
    ///
    /// - nothing is changed in light mode: every property touched in dark mode is remembered first and restored
    /// - windows are discovered with Application.OpenForms (Idle event), controls added later with ControlAdded
    /// - menus / context menus / status bars follow ToolStripManager.Renderer
    /// - the video player keeps its own theme handling (see VideoPlayer.ApplyTheme)
    ///
    /// Not themed by design: standard Windows dialogs (message boxes, file / color dialogs) and progress bars.
    /// </summary>
    public static partial class ThemeManager
    {
        #region state
        /// <summary>what has been changed on a control, so that it can be put back</summary>
        private sealed class Memo
        {
            public readonly List<Action> Undo = new List<Action>();
            public bool Applied;
            public bool HooksSet;
            public OverlayPainter Painter;
            public ListHeaderPainter HeaderPainter;
        }

        private static readonly ConditionalWeakTable<Control, Memo> memos = new ConditionalWeakTable<Control, Memo>();
        private static readonly ConditionalWeakTable<Form, object> knownForms = new ConditionalWeakTable<Form, object>();
        private static SynchronizationContext uiContext;
        private static bool installed;
        #endregion

        #region installation
        /// <summary>
        /// Starts the application wide theming. Call it once, on the UI thread, after the theme mode has been set.
        /// </summary>
        public static void Install()
        {
            if (installed)
                return;
            installed = true;
            uiContext = SynchronizationContext.Current;

            ThemeChanged += delegate
            {
                if (uiContext != null)
                    uiContext.Post(delegate { ApplyToOpenForms(); }, null);
                else
                    ApplyToOpenForms();
            };
            InstallShowHook();
            Application.Idle += delegate
            {
                if (uiContext == null)
                    uiContext = SynchronizationContext.Current; // the UI thread is the one running Idle
                // windows created after the theme has been applied
                if (IsDark)
                    ThemeNewForms();
            };
            ApplyToOpenForms();
        }

        /// <summary>
        /// Applies the theme mode chosen in the settings. Safe to call before or after <see cref="Install"/>.
        /// </summary>
        public static void SetMode(ThemeMode newMode)
        {
            bool wasDark = IsDark;
            Mode = newMode;
            // Mode raises ThemeChanged only if the effective theme changed; the very first call also has to
            // make sure that the windows which exist already are in the right state
            if (installed && wasDark == IsDark)
                ApplyToOpenForms();
        }

        private static List<Form> OpenFormsSnapshot()
        {
            List<Form> forms = new List<Form>();
            foreach (Form f in Application.OpenForms)
                forms.Add(f);
            return forms;
        }

        private static void ApplyToOpenForms()
        {
            UpdateToolStripRenderer();
            foreach (Form f in OpenFormsSnapshot())
                ThemeWindow(f);
        }

        private static void ThemeNewForms()
        {
            foreach (Form f in OpenFormsSnapshot())
            {
                object o;
                if (!knownForms.TryGetValue(f, out o))
                    ThemeWindow(f);
            }
        }

        /// <summary>
        /// Themes one window (and everything it contains) with the current theme.
        /// Windows opened with Application.OpenForms are handled automatically; call it directly only to avoid the
        /// short moment where a new window is shown with the light colours.
        /// </summary>
        public static void ThemeWindow(Form form)
        {
            if (form == null || form.IsDisposed || form is VideoPlayer)
                return;

            object o;
            if (!knownForms.TryGetValue(form, out o))
                knownForms.Add(form, null);

            bool dark = IsDark;
            if (!dark)
            {
                Memo m;
                if (!memos.TryGetValue(form, out m) || !m.Applied)
                    return; // never themed: nothing to undo
            }
            ApplyTree(form, dark);
        }

        private static void UpdateToolStripRenderer()
        {
            try
            {
                if (IsDark)
                    ToolStripManager.Renderer = new ThemedToolStripRenderer(new ThemedColorTable(Palette));
                else
                    ToolStripManager.RenderMode = ToolStripManagerRenderMode.Professional;
            }
            catch (Exception)
            {
                // keep the current renderer
            }
        }
        #endregion

        #region theme a window before it is shown (no flash of light colours)
        private delegate IntPtr HookProc(int code, IntPtr wParam, IntPtr lParam);

        [StructLayout(LayoutKind.Sequential)]
        private struct CWPSTRUCT
        {
            public IntPtr lParam;
            public IntPtr wParam;
            public uint message;
            public IntPtr hwnd;
        }

        private const int WH_CALLWNDPROC = 4;
        private const uint WM_SHOWWINDOW = 0x0018;

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll")]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll")]
        private static extern uint GetCurrentThreadId();

        private static HookProc showHook; // kept in a field: the delegate must not be collected
        private static IntPtr showHookHandle = IntPtr.Zero;

        /// <summary>
        /// A window which is about to be shown for the first time gets the theme right away
        /// (WM_SHOWWINDOW is sent before it becomes visible); Application.Idle would be too late
        /// and the window would flash with the light colours.
        /// </summary>
        private static void InstallShowHook()
        {
            if (showHookHandle != IntPtr.Zero)
                return;
            try
            {
                showHook = ShowHookProc;
                showHookHandle = SetWindowsHookEx(WH_CALLWNDPROC, showHook, IntPtr.Zero, GetCurrentThreadId());
            }
            catch (Exception)
            {
                // the Idle event still themes the new windows, only a bit later
            }
        }

        private static IntPtr ShowHookProc(int code, IntPtr wParam, IntPtr lParam)
        {
            if (code >= 0 && IsDark)
            {
                try
                {
                    CWPSTRUCT msg = (CWPSTRUCT)Marshal.PtrToStructure(lParam, typeof(CWPSTRUCT));
                    if (msg.message == WM_SHOWWINDOW && msg.wParam != IntPtr.Zero)
                    {
                        Form f = Control.FromHandle(msg.hwnd) as Form;
                        object known;
                        if (f != null && !f.IsDisposed && !knownForms.TryGetValue(f, out known))
                            ThemeWindow(f);
                    }
                }
                catch (Exception)
                {
                    // never break the message loop
                }
            }
            return CallNextHookEx(showHookHandle, code, wParam, lParam);
        }
        #endregion

        #region walking the control tree
        private static void ApplyTree(Control c, bool dark)
        {
            if (c == null || c.IsDisposed)
                return;
            // controls which paint themselves with the palette (the video player) are not touched
            if (c is PlayerSeekBar || c is PlayerButton || c is VideoPlayerControl)
                return;

            Memo m = memos.GetOrCreateValue(c);
            if (dark)
            {
                HookControl(c, m);
                ApplyDark(c, m);
            }
            else if (m.Applied)
                Restore(c, m);

            if (c.ContextMenuStrip != null && !c.ContextMenuStrip.IsDisposed)
                ApplyTree(c.ContextMenuStrip, dark);

            ToolStrip hostStrip = c as ToolStrip;
            if (hostStrip != null)
            {
                // progress bars of status strips are controls hosted by the items
                foreach (ToolStripItem item in hostStrip.Items)
                {
                    ToolStripControlHost host = item as ToolStripControlHost;
                    if (host != null && host.Control is ProgressBar)
                        ApplyTree(host.Control, dark);
                }
            }

            // the parts of these controls are managed by the control itself
            if (c is UpDownBase || c is ComboBox || c is ToolStrip)
                return;

            // copy: applying may create handles / change the collection
            Control[] children = new Control[c.Controls.Count];
            c.Controls.CopyTo(children, 0);
            foreach (Control child in children)
                ApplyTree(child, dark);
        }

        private static void HookControl(Control c, Memo m)
        {
            if (m.HooksSet)
                return;
            m.HooksSet = true;
            c.ControlAdded += OnControlAdded;
            c.HandleCreated += OnHandleCreated;
        }

        private static void OnControlAdded(object sender, ControlEventArgs e)
        {
            // controls created after the window has been themed (tabs, per-stream controls, ...)
            if (IsDark)
                ApplyTree(e.Control, true);
        }

        private static void OnHandleCreated(object sender, EventArgs e)
        {
            Control c = sender as Control;
            if (c == null || !IsDark)
                return;
            Memo m;
            if (memos.TryGetValue(c, out m) && m.Applied)
            {
                ApplyNativeTheme(c, true);
                Form f = c as Form;
                if (f != null)
                    ApplyTitleBar(f);
            }
        }
        #endregion

        #region dark mode: what is changed on each kind of control
        private static void ApplyDark(Control c, Memo m)
        {
            ThemePalette p = Palette;
            List<Action> undo = m.Applied ? null : m.Undo; // only the first application records what to restore
            m.Applied = true;

            Form form = c as Form;
            ToolStrip strip = c as ToolStrip;
            TabControl tabs = c as TabControl;
            TabPage page = c as TabPage;
            Button button = c as Button;
            LinkLabel link = c as LinkLabel;

            if (form != null)
            {
                SetColor(c, "BackColor", p.Window, undo);
                SetColor(c, "ForeColor", p.Text, undo);
                ApplyTitleBar(form);
            }
            else if (strip != null)
            {
                // the colours come from the renderer (ToolStripManager.Renderer); make sure this strip uses it
                if (strip.RenderMode != ToolStripRenderMode.ManagerRenderMode)
                {
                    ToolStripRenderMode oldMode = strip.RenderMode;
                    if (undo != null)
                        undo.Add(delegate { strip.RenderMode = oldMode; });
                    strip.RenderMode = ToolStripRenderMode.ManagerRenderMode;
                }
            }
            else if (page != null)
            {
                SetBool(page, "UseVisualStyleBackColor", false, undo);
                SetColor(c, "BackColor", p.Window, undo);
                SetColor(c, "ForeColor", p.Text, undo);
            }
            else if (tabs != null)
            {
                if (tabs.Alignment == TabAlignment.Top && tabs.Appearance == TabAppearance.Normal && m.Painter == null)
                {
                    m.Painter = new TabPainter(tabs);
                    if (undo != null)
                        undo.Add(delegate { DetachPainter(tabs); });
                }
                tabs.Invalidate();
            }
            else if (c is ProgressBar)
            {
                // CustomProgressBar paints itself; the standard one only takes the colours without visual style
                if (!(c is CustomProgressBar))
                {
                    ProgressBar bar = (ProgressBar)c;
                    if (bar.Style == ProgressBarStyle.Blocks)
                    {
                        if (undo != null)
                            undo.Add(delegate { bar.Style = ProgressBarStyle.Blocks; });
                        bar.Style = ProgressBarStyle.Continuous;
                    }
                    SetColor(c, "BackColor", p.Surface, undo);
                    SetColor(c, "ForeColor", ProgressFill, undo);
                }
                c.Invalidate();
            }
            else if (button != null)
            {
                if (!HasCustomColor(c, "BackColor"))
                {
                    SetBool(button, "UseVisualStyleBackColor", false, undo);
                    SetFlatStyle(button, FlatStyle.Flat, undo);
                    button.FlatAppearance.BorderColor = p.Border;
                    button.FlatAppearance.MouseOverBackColor = p.SurfaceHover;
                    button.FlatAppearance.MouseDownBackColor = p.SurfaceHover;
                    SetColor(c, "BackColor", p.Surface, undo);
                    SetColor(c, "ForeColor", p.Text, undo);
                }
            }
            else if (c is TextBoxBase || c is ComboBox || c is ListControl || c is UpDownBase || c is ListView || c is TreeView)
            {
                if (!HasCustomColor(c, "BackColor"))
                    SetColor(c, "BackColor", p.Surface, undo);
                if (!HasCustomColor(c, "ForeColor"))
                    SetColor(c, "ForeColor", p.Text, undo);
                // the sunken 3D border is drawn white: use the flat one
                SetFlatBorder(c, undo);
            }
            else
            {
                // labels, panels, group boxes, check boxes, ...: they inherit the colours of their parent
                // window / page, only an explicit system colour set by the designer has to be replaced
                if (IsSystemColorSet(c, "BackColor"))
                    SetColor(c, "BackColor", p.Window, undo);
                if (IsSystemColorSet(c, "ForeColor"))
                    SetColor(c, "ForeColor", p.Text, undo);

                // the "System" flat style ignores ForeColor (black text on a dark background)
                ButtonBase bb = c as ButtonBase;
                if (bb != null && bb.FlatStyle == FlatStyle.System)
                    SetFlatStyle(bb, FlatStyle.Standard, undo);
                GroupBox gb = c as GroupBox;
                if (gb != null && gb.FlatStyle == FlatStyle.System)
                    SetFlatStyle(gb, FlatStyle.Standard, undo);
            }

            GroupBox group = c as GroupBox;
            if (group != null && m.Painter == null)
            {
                m.Painter = new GroupPainter(group);
                if (undo != null)
                    undo.Add(delegate { DetachPainter(group); });
            }
            ListView listView = c as ListView;
            if (listView != null && m.HeaderPainter == null)
            {
                m.HeaderPainter = new ListHeaderPainter(listView);
                if (undo != null)
                    undo.Add(delegate { DetachHeaderPainter(listView); });
            }
            if (form != null)
                ApplyMenu(form, true);

            if (link != null)
            {
                SetColor(c, "LinkColor", p.Accent, undo);
                SetColor(c, "ActiveLinkColor", p.Accent, undo);
                SetColor(c, "VisitedLinkColor", p.TextDim, undo);
            }

            ApplyNativeTheme(c, true);
        }

        private static void Restore(Control c, Memo m)
        {
            for (int i = m.Undo.Count - 1; i >= 0; i--)
            {
                try
                {
                    m.Undo[i]();
                }
                catch (Exception)
                {
                    // a property that cannot be restored must not stop the others
                }
            }
            m.Undo.Clear();
            m.Applied = false;

            ApplyNativeTheme(c, false);
            Form f = c as Form;
            if (f != null)
            {
                ApplyTitleBar(f);
                ApplyMenu(f, false);
            }
            c.Invalidate();
        }

        private static void DetachPainter(Control c)
        {
            Memo m;
            if (memos.TryGetValue(c, out m) && m.Painter != null)
            {
                m.Painter.Detach();
                m.Painter = null;
            }
            c.Invalidate();
        }

        private static void DetachHeaderPainter(ListView lv)
        {
            Memo m;
            if (memos.TryGetValue(lv, out m) && m.HeaderPainter != null)
            {
                m.HeaderPainter.Detach();
                m.HeaderPainter = null;
            }
            lv.Invalidate();
        }

        private static void SetFlatBorder(Control c, List<Action> undo)
        {
            PropertyDescriptor pd = TypeDescriptor.GetProperties(c)["BorderStyle"];
            if (pd == null || !(pd.GetValue(c) is BorderStyle) || (BorderStyle)pd.GetValue(c) != BorderStyle.Fixed3D)
                return;
            if (undo != null)
                undo.Add(delegate { pd.SetValue(c, BorderStyle.Fixed3D); });
            pd.SetValue(c, BorderStyle.FixedSingle);
        }
        #endregion

        #region property helpers
        /// <summary>
        /// Sets a colour property and records how to put the previous value back
        /// (a property that was not set explicitly goes back to its inherited / default value).
        /// </summary>
        private static void SetColor(Control c, string name, Color value, List<Action> undo)
        {
            PropertyDescriptor pd = TypeDescriptor.GetProperties(c)[name];
            if (pd == null)
                return;
            if (undo != null)
            {
                bool wasSet = pd.ShouldSerializeValue(c);
                object old = pd.GetValue(c);
                undo.Add(delegate
                {
                    if (wasSet)
                        pd.SetValue(c, old);
                    else
                        pd.ResetValue(c);
                });
            }
            pd.SetValue(c, value);
        }

        private static void SetBool(Control c, string name, bool value, List<Action> undo)
        {
            PropertyDescriptor pd = TypeDescriptor.GetProperties(c)[name];
            if (pd == null)
                return;
            if (undo != null)
            {
                object old = pd.GetValue(c);
                undo.Add(delegate { pd.SetValue(c, old); });
            }
            pd.SetValue(c, value);
        }

        private static void SetFlatStyle(ButtonBase b, FlatStyle style, List<Action> undo)
        {
            if (undo != null)
            {
                FlatStyle old = b.FlatStyle;
                undo.Add(delegate { b.FlatStyle = old; });
            }
            b.FlatStyle = style;
        }

        private static void SetFlatStyle(GroupBox g, FlatStyle style, List<Action> undo)
        {
            if (undo != null)
            {
                FlatStyle old = g.FlatStyle;
                undo.Add(delegate { g.FlatStyle = old; });
            }
            g.FlatStyle = style;
        }

        private static bool IsExplicit(Control c, string name)
        {
            PropertyDescriptor pd = TypeDescriptor.GetProperties(c)[name];
            return pd != null && pd.ShouldSerializeValue(c);
        }

        /// <summary>true if the colour was set to a system colour (SystemColors.Control, Window, ...) in the designer</summary>
        private static bool IsSystemColorSet(Control c, string name)
        {
            if (!IsExplicit(c, name))
                return false;
            Color col = (Color)TypeDescriptor.GetProperties(c)[name].GetValue(c);
            return col.IsSystemColor;
        }

        /// <summary>true if the colour was set to a specific colour on purpose (colour swatch buttons, error highlight, ...)</summary>
        private static bool HasCustomColor(Control c, string name)
        {
            if (!IsExplicit(c, name))
                return false;
            Color col = (Color)TypeDescriptor.GetProperties(c)[name].GetValue(c);
            return !col.IsSystemColor;
        }
        #endregion

        #region native dark theme (scroll bars, headers, combo box drop down, spin buttons)
        [DllImport("uxtheme.dll", CharSet = CharSet.Unicode)]
        private static extern int SetWindowTheme(IntPtr hwnd, string subAppName, string subIdList);

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        private static void ApplyNativeTheme(Control c, bool dark)
        {
            if (!c.IsHandleCreated)
                return; // done again from HandleCreated
            string name = null;
            if (c is ProgressBar && !(c is CustomProgressBar))
            {
                // no visual style: the bar then uses the colours of the control
                try
                {
                    SetWindowTheme(c.Handle, dark ? "" : null, dark ? "" : null);
                    if (dark)
                    {
                        SendMessage(c.Handle, 0x2001 /*PBM_SETBKCOLOR*/, IntPtr.Zero, (IntPtr)ColorTranslator.ToWin32(Palette.Surface));
                        SendMessage(c.Handle, 0x0409 /*PBM_SETBARCOLOR*/, IntPtr.Zero, (IntPtr)ColorTranslator.ToWin32(ProgressFill));
                    }
                }
                catch (Exception)
                {
                    // keep the standard bar
                }
                return;
            }
            if (c is ComboBox)
                name = "DarkMode_CFD";
            else if (c is ListView || c is UpDownBase)
                name = "DarkMode_ItemsView";
            else if (c is TextBoxBase || c is ListBox || c is TreeView || c is TabControl)
                name = "DarkMode_Explorer";
            else
            {
                ScrollableControl sc = c as ScrollableControl;
                if (sc != null && sc.AutoScroll)
                    name = "DarkMode_Explorer";
            }
            if (name == null)
                return;
            try
            {
                if (dark)
                    SetWindowTheme(c.Handle, name, null);
                else
                    SetWindowTheme(c.Handle, null, null);
            }
            catch (Exception)
            {
                // uxtheme without the dark themes (older Windows): keep the light scroll bars
            }
        }
        #endregion

        #region painting over the standard drawing (tab headers, group boxes)
        /// <summary>
        /// Base of the painters which draw on top of the standard painting of a control
        /// (no owner draw: sizes and layout do not change).
        /// </summary>
        private abstract class OverlayPainter : NativeWindow
        {
            private const int WM_PAINT = 0x000F;
            protected readonly Control Owner;

            protected OverlayPainter(Control control)
            {
                Owner = control;
                Owner.HandleCreated += OnCreated;
                Owner.HandleDestroyed += OnDestroyed;
                if (Owner.IsHandleCreated)
                    AssignHandle(Owner.Handle);
            }

            public void Detach()
            {
                Owner.HandleCreated -= OnCreated;
                Owner.HandleDestroyed -= OnDestroyed;
                if (Handle != IntPtr.Zero)
                    ReleaseHandle();
            }

            private void OnCreated(object sender, EventArgs e)
            {
                if (Handle == IntPtr.Zero)
                    AssignHandle(Owner.Handle);
            }

            private void OnDestroyed(object sender, EventArgs e)
            {
                if (Handle != IntPtr.Zero)
                    ReleaseHandle();
            }

            protected override void WndProc(ref Message m)
            {
                base.WndProc(ref m);
                if (m.Msg == WM_PAINT && IsDark && !Owner.IsDisposed)
                {
                    try
                    {
                        using (Graphics g = Graphics.FromHwnd(Handle))
                            Paint(g);
                    }
                    catch (Exception)
                    {
                        // never let a painting problem break the window
                    }
                }
            }

            protected abstract void Paint(Graphics g);
        }

        /// <summary>
        /// Paints the tab headers and the frame of a TabControl with the palette.
        /// </summary>
        private sealed class TabPainter : OverlayPainter
        {
            private readonly TabControl tab;

            public TabPainter(TabControl tabControl) : base(tabControl)
            {
                tab = tabControl;
            }

            protected override void Paint(Graphics g)
            {
                int count = tab.TabCount;
                if (count == 0)
                    return;

                ThemePalette p = Palette;
                int headerBottom = 0;
                for (int i = 0; i < count; i++)
                    headerBottom = Math.Max(headerBottom, tab.GetTabRect(i).Bottom);
                int width = tab.ClientSize.Width;
                int height = tab.ClientSize.Height;
                Rectangle selected = tab.SelectedIndex >= 0 ? tab.GetTabRect(tab.SelectedIndex) : Rectangle.Empty;
                Rectangle display = tab.DisplayRectangle;

                using (SolidBrush back = new SolidBrush(p.Window))
                using (Pen border = new Pen(p.Border))
                using (SolidBrush accent = new SolidBrush(p.Accent))
                {
                    // header band (including the free area on the right of the tabs)
                    g.FillRectangle(back, 0, 0, width, headerBottom);

                    // the standard 3D frame around the page (white): erase everything between the control
                    // edge and the page, then draw a thin line on the edge
                    g.SetClip(display, System.Drawing.Drawing2D.CombineMode.Exclude);
                    g.FillRectangle(back, 0, headerBottom, width, height - headerBottom);
                    g.ResetClip();

                    // frame line, open below the selected tab
                    int y = headerBottom;
                    if (selected.IsEmpty)
                        g.DrawLine(border, 0, y, width - 1, y);
                    else
                    {
                        g.DrawLine(border, 0, y, selected.Left, y);
                        g.DrawLine(border, selected.Right - 1, y, width - 1, y);
                    }
                    g.DrawLine(border, 0, y, 0, height - 1);
                    g.DrawLine(border, width - 1, y, width - 1, height - 1);
                    g.DrawLine(border, 0, height - 1, width - 1, height - 1);

                    for (int i = 0; i < count; i++)
                    {
                        Rectangle r = tab.GetTabRect(i);
                        bool isSelected = i == tab.SelectedIndex;
                        if (isSelected)
                        {
                            g.FillRectangle(back, r.Left, r.Top, r.Width, r.Height + 1);
                            g.FillRectangle(accent, r.Left, r.Top, r.Width, 2);
                        }
                        else
                        {
                            g.DrawRectangle(border, r.Left, r.Top, r.Width - 1, r.Height);
                        }
                        TextRenderer.DrawText(g, tab.TabPages[i].Text, tab.Font, r,
                            isSelected ? p.Text : p.TextDim,
                            TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine
                            | TextFormatFlags.EndEllipsis | TextFormatFlags.HidePrefix);
                    }
                }
            }
        }

        /// <summary>
        /// Paints the frame and the title of a GroupBox with the palette (the standard drawing uses a white
        /// frame and a title colour which cannot be changed).
        /// </summary>
        private sealed class GroupPainter : OverlayPainter
        {
            private readonly GroupBox group;

            public GroupPainter(GroupBox groupBox) : base(groupBox)
            {
                group = groupBox;
            }

            protected override void Paint(Graphics g)
            {
                int width = group.ClientSize.Width;
                int height = group.ClientSize.Height;
                if (width < 12 || height < 12)
                    return;

                ThemePalette p = Palette;
                TextFormatFlags flags = TextFormatFlags.NoPadding | TextFormatFlags.SingleLine | TextFormatFlags.Left | TextFormatFlags.NoClipping;
                string text = group.Text;
                Size textSize = string.IsNullOrEmpty(text)
                    ? Size.Empty
                    : TextRenderer.MeasureText(g, text, group.Font, new Size(int.MaxValue, int.MaxValue), flags);
                int titleHeight = Math.Max(textSize.Height, group.Font.Height);
                int top = titleHeight / 2;
                const int band = 3;

                using (SolidBrush back = new SolidBrush(group.BackColor))
                using (Pen border = new Pen(p.Border))
                {
                    // erase the standard frame
                    g.FillRectangle(back, 0, 0, width, titleHeight + 1);
                    g.FillRectangle(back, 0, 0, band, height);
                    g.FillRectangle(back, width - band, 0, band, height);
                    g.FillRectangle(back, 0, height - band, width, band);

                    // frame with a gap for the title
                    int gapStart = 5;
                    int gapEnd = textSize.IsEmpty ? 5 : 8 + textSize.Width + 3;
                    g.DrawLine(border, 0, top, gapStart, top);
                    g.DrawLine(border, gapEnd, top, width - 1, top);
                    g.DrawLine(border, 0, top, 0, height - 1);
                    g.DrawLine(border, width - 1, top, width - 1, height - 1);
                    g.DrawLine(border, 0, height - 1, width - 1, height - 1);
                }

                if (!textSize.IsEmpty)
                    TextRenderer.DrawText(g, text, group.Font, new Point(8, 0), group.Enabled ? p.Text : p.TextDim, flags);
            }
        }
        #endregion

        #region list view column headers
        /// <summary>
        /// Paints the column headers of a ListView (details view) with the palette: the native header ignores
        /// the dark theme. It subclasses the header control of the list and draws over its standard painting.
        /// </summary>
        private sealed class ListHeaderPainter : NativeWindow
        {
            private const int WM_PAINT = 0x000F;
            private const int LVM_GETHEADER = 0x101F;
            private const int HDM_GETITEMRECT = 0x1207;

            [StructLayout(LayoutKind.Sequential)]
            private struct RECT
            {
                public int left, top, right, bottom;
            }

            [DllImport("user32.dll")]
            private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

            [DllImport("user32.dll")]
            private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, ref RECT lParam);

            [DllImport("user32.dll")]
            private static extern bool GetClientRect(IntPtr hWnd, out RECT rect);

            private readonly ListView list;

            public ListHeaderPainter(ListView listView)
            {
                list = listView;
                list.HandleCreated += OnListHandleCreated;
                list.HandleDestroyed += OnListHandleDestroyed;
                Attach();
            }

            public void Detach()
            {
                list.HandleCreated -= OnListHandleCreated;
                list.HandleDestroyed -= OnListHandleDestroyed;
                if (Handle != IntPtr.Zero)
                    ReleaseHandle();
            }

            private void Attach()
            {
                if (Handle != IntPtr.Zero || !list.IsHandleCreated)
                    return;
                IntPtr header = SendMessage(list.Handle, LVM_GETHEADER, IntPtr.Zero, IntPtr.Zero);
                if (header != IntPtr.Zero)
                    AssignHandle(header);
            }

            private void OnListHandleCreated(object sender, EventArgs e)
            {
                Attach();
            }

            private void OnListHandleDestroyed(object sender, EventArgs e)
            {
                if (Handle != IntPtr.Zero)
                    ReleaseHandle();
            }

            protected override void WndProc(ref Message m)
            {
                base.WndProc(ref m);
                if (m.Msg == WM_PAINT && IsDark && !list.IsDisposed)
                {
                    try
                    {
                        using (Graphics g = Graphics.FromHwnd(Handle))
                            Paint(g);
                    }
                    catch (Exception)
                    {
                        // never let a painting problem break the window
                    }
                }
            }

            private void Paint(Graphics g)
            {
                ThemePalette p = Palette;
                RECT client;
                if (!GetClientRect(Handle, out client))
                    return;

                using (SolidBrush back = new SolidBrush(p.Surface))
                using (Pen line = new Pen(p.Border))
                {
                    g.FillRectangle(back, 0, 0, client.right, client.bottom);
                    g.DrawLine(line, 0, client.bottom - 1, client.right, client.bottom - 1);

                    for (int i = 0; i < list.Columns.Count; i++)
                    {
                        RECT r = new RECT();
                        if (SendMessage(Handle, HDM_GETITEMRECT, (IntPtr)i, ref r) == IntPtr.Zero)
                            continue;
                        g.DrawLine(line, r.right - 1, r.top + 3, r.right - 1, r.bottom - 4);

                        TextFormatFlags flags = TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine
                            | TextFormatFlags.EndEllipsis | TextFormatFlags.NoPrefix | TextFormatFlags.NoPadding;
                        HorizontalAlignment align = list.Columns[i].TextAlign;
                        flags |= align == HorizontalAlignment.Center ? TextFormatFlags.HorizontalCenter
                               : align == HorizontalAlignment.Right ? TextFormatFlags.Right : TextFormatFlags.Left;
                        Rectangle text = new Rectangle(r.left + 6, r.top, Math.Max(0, r.right - r.left - 12), r.bottom - r.top);
                        TextRenderer.DrawText(g, list.Columns[i].Text, list.Font, text, p.Text, flags);
                    }
                }
            }
        }
        #endregion

        #region main menu (legacy MainMenu / MenuItem: drawn by Windows, so it is owner drawn)
        [StructLayout(LayoutKind.Sequential)]
        private struct MENUINFO
        {
            public int cbSize;
            public uint fMask;
            public uint dwStyle;
            public uint cyMax;
            public IntPtr hbrBack;
            public uint dwContextHelpID;
            public IntPtr dwMenuData;
        }

        private const uint MIM_BACKGROUND = 0x00000002;
        private const uint MIM_APPLYTOSUBMENUS = 0x80000000;

        [DllImport("user32.dll")]
        private static extern bool SetMenuInfo(IntPtr hmenu, ref MENUINFO lpcmi);

        [DllImport("user32.dll")]
        private static extern bool DrawMenuBar(IntPtr hWnd);

        [DllImport("gdi32.dll")]
        private static extern IntPtr CreateSolidBrush(int colorRef);

        // one brush for the whole application life (the dark colour never changes); a menu keeps using it
        private static IntPtr menuBrush = IntPtr.Zero;

        /// <summary>
        /// Gives the menu bar and its drop-down menus the colours of the palette (dark) or the standard
        /// look (light). The legacy menus have no colour properties: the items are drawn by us and the
        /// background is set with SetMenuInfo.
        /// </summary>
        private static void ApplyMenu(Form form, bool dark)
        {
            MainMenu menu = form.Menu;
            if (menu == null)
                return;
            try
            {
                SetMenuBackground(menu, dark);
                foreach (MenuItem item in menu.MenuItems)
                    ApplyMenuItem(item, dark);
                if (form.IsHandleCreated)
                    DrawMenuBar(form.Handle);
            }
            catch (Exception)
            {
                // keep the standard menu
            }
        }

        private static void SetMenuBackground(Menu menu, bool dark)
        {
            IntPtr brush = IntPtr.Zero;
            if (dark)
            {
                if (menuBrush == IntPtr.Zero)
                    menuBrush = CreateSolidBrush(ColorTranslator.ToWin32(Palette.Window));
                brush = menuBrush;
            }

            MENUINFO mi = new MENUINFO();
            mi.cbSize = Marshal.SizeOf(typeof(MENUINFO));
            mi.fMask = MIM_BACKGROUND | MIM_APPLYTOSUBMENUS;
            mi.hbrBack = brush;
            SetMenuInfo(menu.Handle, ref mi);
        }

        private static void ApplyMenuItem(MenuItem item, bool dark)
        {
            item.DrawItem -= MenuDrawItem;
            item.MeasureItem -= MenuMeasureItem;
            item.Popup -= MenuPopup;
            if (dark)
            {
                item.DrawItem += MenuDrawItem;
                item.MeasureItem += MenuMeasureItem;
                item.Popup += MenuPopup; // items added by the application when the menu opens
            }
            item.OwnerDraw = dark;
            foreach (MenuItem child in item.MenuItems)
                ApplyMenuItem(child, dark);
        }

        private static void MenuPopup(object sender, EventArgs e)
        {
            MenuItem item = sender as MenuItem;
            if (item == null || !IsDark)
                return;
            foreach (MenuItem child in item.MenuItems)
            {
                if (!child.OwnerDraw)
                    ApplyMenuItem(child, true);
            }
        }

        private static string ShortcutText(MenuItem item)
        {
            if (item.Parent is MainMenu || !item.ShowShortcut || item.Shortcut == Shortcut.None)
                return null;
            return TypeDescriptor.GetConverter(typeof(Keys)).ConvertToString((Keys)(int)item.Shortcut);
        }

        private static void MenuMeasureItem(object sender, MeasureItemEventArgs e)
        {
            MenuItem item = (MenuItem)sender;
            Font font = SystemFonts.MenuFont;
            bool top = item.Parent is MainMenu;
            if (item.Text == "-")
            {
                e.ItemWidth = 40;
                e.ItemHeight = 7;
                return;
            }
            Size size = TextRenderer.MeasureText(e.Graphics, item.Text, font, new Size(int.MaxValue, int.MaxValue),
                TextFormatFlags.SingleLine | TextFormatFlags.NoPadding);
            if (top)
            {
                e.ItemWidth = size.Width + 8;
                e.ItemHeight = SystemInformation.MenuHeight;
                return;
            }
            string shortcut = ShortcutText(item);
            int shortcutWidth = 0;
            if (shortcut != null)
                shortcutWidth = 24 + TextRenderer.MeasureText(e.Graphics, shortcut, font, new Size(int.MaxValue, int.MaxValue),
                    TextFormatFlags.SingleLine | TextFormatFlags.NoPadding).Width;
            e.ItemWidth = 28 + size.Width + shortcutWidth + 22;
            e.ItemHeight = Math.Max(font.Height + 8, 22);
        }

        private static void MenuDrawItem(object sender, DrawItemEventArgs e)
        {
            MenuItem item = (MenuItem)sender;
            ThemePalette p = Palette;
            Font font = SystemFonts.MenuFont;
            bool top = item.Parent is MainMenu;
            bool disabled = !item.Enabled;
            bool selected = !disabled && (e.State & (DrawItemState.Selected | DrawItemState.HotLight)) != 0;
            Rectangle r = e.Bounds;

            using (SolidBrush back = new SolidBrush(selected ? p.SurfaceHover : (top ? p.Window : p.Surface)))
                e.Graphics.FillRectangle(back, r);

            if (item.Text == "-")
            {
                using (Pen line = new Pen(p.Border))
                    e.Graphics.DrawLine(line, r.Left + 26, r.Top + r.Height / 2, r.Right - 4, r.Top + r.Height / 2);
                return;
            }

            Color fore = disabled ? p.TextDim : p.Text;
            TextFormatFlags flags = TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine | TextFormatFlags.NoPadding;
            if ((e.State & DrawItemState.NoAccelerator) != 0)
                flags |= TextFormatFlags.HidePrefix;

            if (top)
            {
                TextRenderer.DrawText(e.Graphics, item.Text, font, r, fore, flags | TextFormatFlags.HorizontalCenter);
                return;
            }

            Rectangle textRect = new Rectangle(r.Left + 28, r.Top, r.Width - 28 - 22, r.Height);
            TextRenderer.DrawText(e.Graphics, item.Text, font, textRect, fore, flags | TextFormatFlags.Left);

            string shortcut = ShortcutText(item);
            if (shortcut != null)
                TextRenderer.DrawText(e.Graphics, shortcut, font, new Rectangle(r.Left, r.Top, r.Width - 22, r.Height),
                    p.TextDim, flags | TextFormatFlags.Right);

            if (item.Checked)
            {
                using (Pen tick = new Pen(fore, 2f))
                {
                    System.Drawing.Drawing2D.SmoothingMode old = e.Graphics.SmoothingMode;
                    e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    int cx = r.Left + 12;
                    int cy = r.Top + r.Height / 2;
                    e.Graphics.DrawLines(tick, new Point[] { new Point(cx - 4, cy), new Point(cx - 1, cy + 3), new Point(cx + 5, cy - 4) });
                    e.Graphics.SmoothingMode = old;
                }
            }

            if (item.IsParent)
            {
                int ax = r.Right - 12;
                int ay = r.Top + r.Height / 2;
                using (SolidBrush arrow = new SolidBrush(fore))
                    e.Graphics.FillPolygon(arrow, new Point[] { new Point(ax, ay - 4), new Point(ax + 4, ay), new Point(ax, ay + 4) });
            }
        }
        #endregion
    }
}
