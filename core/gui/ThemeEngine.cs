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
            public TabPainter Painter;
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
                ApplyTitleBar(f);
            c.Invalidate();
        }

        private static void DetachPainter(TabControl tabs)
        {
            Memo m;
            if (memos.TryGetValue(tabs, out m) && m.Painter != null)
            {
                m.Painter.Detach();
                m.Painter = null;
            }
            tabs.Invalidate();
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

        private static void ApplyNativeTheme(Control c, bool dark)
        {
            if (!c.IsHandleCreated)
                return; // done again from HandleCreated
            string name = null;
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

        #region tab headers
        /// <summary>
        /// Paints the tab headers of a TabControl with the palette. It draws on top of the standard painting
        /// (no owner draw, so the size of the tabs does not change).
        /// </summary>
        private sealed class TabPainter : NativeWindow
        {
            private const int WM_PAINT = 0x000F;
            private readonly TabControl tab;

            public TabPainter(TabControl tabControl)
            {
                tab = tabControl;
                tab.HandleCreated += OnCreated;
                tab.HandleDestroyed += OnDestroyed;
                if (tab.IsHandleCreated)
                    AssignHandle(tab.Handle);
            }

            public void Detach()
            {
                tab.HandleCreated -= OnCreated;
                tab.HandleDestroyed -= OnDestroyed;
                if (Handle != IntPtr.Zero)
                    ReleaseHandle();
            }

            private void OnCreated(object sender, EventArgs e)
            {
                if (Handle == IntPtr.Zero)
                    AssignHandle(tab.Handle);
            }

            private void OnDestroyed(object sender, EventArgs e)
            {
                if (Handle != IntPtr.Zero)
                    ReleaseHandle();
            }

            protected override void WndProc(ref Message m)
            {
                base.WndProc(ref m);
                if (m.Msg == WM_PAINT && IsDark)
                {
                    try
                    {
                        PaintHeaders();
                    }
                    catch (Exception)
                    {
                        // never let a painting problem break the window
                    }
                }
            }

            private void PaintHeaders()
            {
                int count = tab.TabCount;
                if (count == 0 || tab.IsDisposed)
                    return;

                ThemePalette p = Palette;
                int headerBottom = 0;
                for (int i = 0; i < count; i++)
                    headerBottom = Math.Max(headerBottom, tab.GetTabRect(i).Bottom);
                int width = tab.ClientSize.Width;
                int height = tab.ClientSize.Height;
                Rectangle selected = tab.SelectedIndex >= 0 ? tab.GetTabRect(tab.SelectedIndex) : Rectangle.Empty;

                using (Graphics g = Graphics.FromHwnd(Handle))
                using (SolidBrush back = new SolidBrush(p.Window))
                using (Pen border = new Pen(p.Border))
                using (SolidBrush accent = new SolidBrush(p.Accent))
                {
                    // header band (including the free area on the right of the tabs)
                    g.FillRectangle(back, 0, 0, width, headerBottom);

                    // border of the page, open below the selected tab
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
        #endregion
    }
}
