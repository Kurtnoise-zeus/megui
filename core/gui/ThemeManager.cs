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
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

using Microsoft.Win32;

namespace MeGUI.core.gui
{
    /// <summary>
    /// User choice for the colour theme.
    /// </summary>
    public enum ThemeMode
    {
        /// <summary>Follow the Windows "app mode" setting (light / dark)</summary>
        System,
        Dark,
        Light
    }

    /// <summary>
    /// The set of colours used by themed windows and controls.
    /// </summary>
    public sealed class ThemePalette
    {
        public Color Window;        // form / panel background
        public Color Surface;       // title area, menus, text boxes
        public Color SurfaceHover;  // hovered / pressed button background
        public Color Text;
        public Color TextDim;
        public Color Track;         // empty part of the seek bar
        public Color Accent;
        public Color OnAccent;      // text / glyph drawn on the accent colour
        public Color Border;
        public Color VideoBack;     // background around the video
    }

    /// <summary>
    /// Central place for the light / dark theme of MeGUI (the palette, the video player helpers and the
    /// menus renderer). The application wide theming of all the windows is in ThemeEngine.cs.
    /// </summary>
    public static partial class ThemeManager
    {
        private static readonly ThemePalette dark = new ThemePalette
        {
            Window = Color.FromArgb(0x20, 0x20, 0x20),
            Surface = Color.FromArgb(0x2B, 0x2B, 0x2B),
            SurfaceHover = Color.FromArgb(0x3C, 0x3C, 0x3C),
            Text = Color.FromArgb(0xF0, 0xF0, 0xF0),
            TextDim = Color.FromArgb(0xB4, 0xB4, 0xB4),
            Track = Color.FromArgb(0x6B, 0x6B, 0x6B),
            Accent = Color.FromArgb(0x4C, 0xC2, 0xFF),
            OnAccent = Color.FromArgb(0x0B, 0x22, 0x30),
            Border = Color.FromArgb(0x4A, 0x4A, 0x4A),
            VideoBack = Color.Black
        };

        private static readonly ThemePalette light = new ThemePalette
        {
            Window = Color.FromArgb(0xF3, 0xF3, 0xF3),
            Surface = Color.FromArgb(0xFF, 0xFF, 0xFF),
            SurfaceHover = Color.FromArgb(0xE0, 0xE0, 0xE0),
            Text = Color.FromArgb(0x1A, 0x1A, 0x1A),
            TextDim = Color.FromArgb(0x55, 0x55, 0x55),
            Track = Color.FromArgb(0xBD, 0xBD, 0xBD),
            Accent = Color.FromArgb(0x00, 0x67, 0xC0),
            OnAccent = Color.White,
            Border = Color.FromArgb(0xC8, 0xC8, 0xC8),
            VideoBack = Color.FromArgb(0xD9, 0xD9, 0xD9)
        };

        private static ThemeMode mode = ThemeMode.System;
        private static bool systemEventsHooked;
        private static bool? systemDark; // cached: IsDark is read very often (every idle, every paint)

        /// <summary>Raised (on the thread that changed the theme) when the effective colours change.</summary>
        public static event EventHandler ThemeChanged;

        /// <summary>The user's choice. Setting it raises ThemeChanged if the effective theme changes.</summary>
        public static ThemeMode Mode
        {
            get { return mode; }
            set
            {
                bool wasDark = IsDark;
                mode = value;
                HookSystemEvents();
                if (wasDark != IsDark)
                    RaiseThemeChanged();
            }
        }

        /// <summary>true if the dark palette is in effect</summary>
        public static bool IsDark
        {
            get
            {
                switch (mode)
                {
                    case ThemeMode.Dark: return true;
                    case ThemeMode.Light: return false;
                    default: return SystemUsesDarkMode();
                }
            }
        }

        public static ThemePalette Palette
        {
            get { return IsDark ? dark : light; }
        }

        private static void HookSystemEvents()
        {
            if (systemEventsHooked)
                return;
            systemEventsHooked = true;
            try
            {
                SystemEvents.UserPreferenceChanged += delegate (object s, UserPreferenceChangedEventArgs e)
                {
                    if (e.Category == UserPreferenceCategory.General || e.Category == UserPreferenceCategory.Color || e.Category == UserPreferenceCategory.VisualStyle)
                    {
                        bool wasDark = IsDark;
                        systemDark = null;
                        if (mode == ThemeMode.System && wasDark != IsDark)
                            RaiseThemeChanged();
                    }
                };
            }
            catch (Exception)
            {
                // no system events available: the theme simply will not follow live changes
            }
        }

        private static void RaiseThemeChanged()
        {
            ThemeChanged?.Invoke(null, EventArgs.Empty);
        }

        private static bool SystemUsesDarkMode()
        {
            if (systemDark == null)
                systemDark = ReadSystemDarkMode();
            return systemDark.Value;
        }

        private static bool ReadSystemDarkMode()
        {
            try
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize"))
                {
                    object v = key?.GetValue("AppsUseLightTheme");
                    if (v is int)
                        return (int)v == 0;
                }
            }
            catch (Exception)
            {
                // registry not readable: fall back to light
            }
            return false;
        }

        #region applying a theme to controls
        /// <summary>
        /// Applies the current palette to a control and all its children.
        /// Controls that paint themselves (PlayerSeekBar, PlayerButton, VideoPlayerControl) read the palette
        /// directly and are only invalidated.
        /// </summary>
        public static void Apply(Control root)
        {
            if (root == null)
                return;
            ThemePalette p = Palette;
            root.SuspendLayout();
            ApplyRecursive(root, p);
            root.ResumeLayout(false);
            Form f = root as Form;
            if (f != null)
                ApplyTitleBar(f);
            root.Invalidate(true);
        }

        private static void ApplyRecursive(Control c, ThemePalette p)
        {
            if (c is PlayerSeekBar || c is PlayerButton || c is VideoPlayerControl || c is PictureBox)
            {
                c.Invalidate();
                return;
            }

            if (c is Form || c is Panel || c is UserControl || c is GroupBox || c is Label || c is CheckBox || c is RadioButton)
            {
                c.BackColor = p.Window;
                c.ForeColor = p.Text;
            }
            else if (c is Button)
            {
                Button b = (Button)c;
                b.UseVisualStyleBackColor = false;
                b.FlatStyle = FlatStyle.Flat;
                b.FlatAppearance.BorderColor = p.Border;
                b.FlatAppearance.MouseOverBackColor = p.SurfaceHover;
                b.FlatAppearance.MouseDownBackColor = p.SurfaceHover;
                b.BackColor = p.Surface;
                b.ForeColor = p.Text;
            }
            else if (c is TextBoxBase || c is ComboBox || c is ListControl || c is NumericUpDown || c is ListView || c is TreeView)
            {
                c.BackColor = p.Surface;
                c.ForeColor = p.Text;
            }

            foreach (Control child in c.Controls)
                ApplyRecursive(child, p);
        }

        private const int DWMWA_USE_IMMERSIVE_DARK_MODE_OLD = 19;
        private const int DWMWA_USE_IMMERSIVE_DARK_MODE = 20;

        [DllImport("dwmapi.dll")]
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);

        /// <summary>
        /// Makes the window title bar follow the dark / light theme (Windows 10 1809 and later; ignored elsewhere).
        /// </summary>
        public static void ApplyTitleBar(Form form)
        {
            if (form == null || !form.IsHandleCreated)
                return;
            try
            {
                int useDark = IsDark ? 1 : 0;
                if (DwmSetWindowAttribute(form.Handle, DWMWA_USE_IMMERSIVE_DARK_MODE, ref useDark, sizeof(int)) != 0)
                    DwmSetWindowAttribute(form.Handle, DWMWA_USE_IMMERSIVE_DARK_MODE_OLD, ref useDark, sizeof(int));
            }
            catch (Exception)
            {
                // dwmapi missing (very old Windows): keep the default title bar
            }
        }

        /// <summary>
        /// Gives a context menu / tool strip the colours of the current palette.
        /// </summary>
        public static void Apply(ToolStrip strip)
        {
            if (strip == null)
                return;
            ThemePalette p = Palette;
            strip.Renderer = new ThemedToolStripRenderer(new ThemedColorTable(p));
            strip.BackColor = p.Surface;
            strip.ForeColor = p.Text;
            foreach (ToolStripItem item in strip.Items)
                ApplyItem(item, p);
        }

        private static void ApplyItem(ToolStripItem item, ThemePalette p)
        {
            item.BackColor = p.Surface;
            item.ForeColor = p.Text;
            ToolStripDropDownItem dd = item as ToolStripDropDownItem;
            if (dd != null)
            {
                dd.DropDown.BackColor = p.Surface;
                dd.DropDown.Renderer = new ThemedToolStripRenderer(new ThemedColorTable(p));
                foreach (ToolStripItem child in dd.DropDownItems)
                    ApplyItem(child, p);
            }
        }
        #endregion

        #region colours for code that sets colours itself
        /// <summary>fill colour of the progress bars in dark mode (white text stays readable on it)</summary>
        public static Color ProgressFill
        {
            get { return Color.FromArgb(0x00, 0x78, 0xD4); }
        }

        /// <summary>background of an input field (text box, numeric box): system colour in light mode</summary>
        public static Color InputBack
        {
            get { return IsDark ? Palette.Surface : SystemColors.Window; }
        }

        /// <summary>text colour of an input field: system colour in light mode</summary>
        public static Color InputText
        {
            get { return IsDark ? Palette.Text : SystemColors.WindowText; }
        }

        /// <summary>
        /// Background of a list row. In light mode it returns lightColor; in dark mode the normal rows use the
        /// surface colour and the highlighted / alternate ones a slightly tinted one.
        /// </summary>
        public static Color ListRowBack(Color lightColor, bool alternate)
        {
            if (!IsDark)
                return lightColor;
            ThemePalette p = Palette;
            return alternate ? Blend(p.Surface, p.Accent, 0.18) : p.Surface;
        }

        internal static Color Blend(Color a, Color b, double amount)
        {
            return Color.FromArgb(
                (int)Math.Round(a.R + (b.R - a.R) * amount),
                (int)Math.Round(a.G + (b.G - a.G) * amount),
                (int)Math.Round(a.B + (b.B - a.B) * amount));
        }
        #endregion

        private sealed class ThemedToolStripRenderer : ToolStripProfessionalRenderer
        {
            private readonly ThemePalette p;

            public ThemedToolStripRenderer(ThemedColorTable table) : base(table)
            {
                RoundedEdges = false;
                p = table.Palette;
            }

            protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
            {
                e.TextColor = e.Item.Enabled ? p.Text : p.TextDim;
                base.OnRenderItemText(e);
            }

            protected override void OnRenderArrow(ToolStripArrowRenderEventArgs e)
            {
                e.ArrowColor = e.Item != null && e.Item.Enabled ? p.Text : p.TextDim;
                base.OnRenderArrow(e);
            }

            protected override void OnRenderItemCheck(ToolStripItemImageRenderEventArgs e)
            {
                Rectangle r = e.ImageRectangle;
                using (SolidBrush back = new SolidBrush(p.SurfaceHover))
                using (Pen border = new Pen(p.Accent))
                using (Pen tick = new Pen(p.Text, 2f))
                {
                    e.Graphics.FillRectangle(back, r);
                    e.Graphics.DrawRectangle(border, r.X, r.Y, r.Width - 1, r.Height - 1);
                    System.Drawing.Drawing2D.SmoothingMode old = e.Graphics.SmoothingMode;
                    e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    e.Graphics.DrawLines(tick, new Point[]
                    {
                        new Point(r.Left + r.Width / 4, r.Top + r.Height / 2),
                        new Point(r.Left + r.Width * 2 / 5, r.Top + r.Height * 2 / 3),
                        new Point(r.Left + r.Width * 3 / 4, r.Top + r.Height / 3)
                    });
                    e.Graphics.SmoothingMode = old;
                }
            }
        }

        private sealed class ThemedColorTable : ProfessionalColorTable
        {
            private readonly ThemePalette p;
            public ThemedColorTable(ThemePalette palette) { p = palette; UseSystemColors = false; }
            public ThemePalette Palette { get { return p; } }
            public override Color MenuStripGradientBegin { get { return p.Window; } }
            public override Color MenuStripGradientEnd { get { return p.Window; } }
            public override Color ToolStripGradientBegin { get { return p.Window; } }
            public override Color ToolStripGradientMiddle { get { return p.Window; } }
            public override Color ToolStripGradientEnd { get { return p.Window; } }
            public override Color StatusStripGradientBegin { get { return p.Window; } }
            public override Color StatusStripGradientEnd { get { return p.Window; } }
            public override Color ToolStripBorder { get { return p.Border; } }
            public override Color ToolStripContentPanelGradientBegin { get { return p.Window; } }
            public override Color ToolStripContentPanelGradientEnd { get { return p.Window; } }
            public override Color ToolStripPanelGradientBegin { get { return p.Window; } }
            public override Color ToolStripPanelGradientEnd { get { return p.Window; } }
            public override Color ButtonSelectedGradientBegin { get { return p.SurfaceHover; } }
            public override Color ButtonSelectedGradientMiddle { get { return p.SurfaceHover; } }
            public override Color ButtonSelectedGradientEnd { get { return p.SurfaceHover; } }
            public override Color ButtonSelectedHighlight { get { return p.SurfaceHover; } }
            public override Color ButtonSelectedBorder { get { return p.Accent; } }
            public override Color ButtonPressedGradientBegin { get { return p.SurfaceHover; } }
            public override Color ButtonPressedGradientMiddle { get { return p.SurfaceHover; } }
            public override Color ButtonPressedGradientEnd { get { return p.SurfaceHover; } }
            public override Color ButtonPressedHighlight { get { return p.SurfaceHover; } }
            public override Color ButtonPressedBorder { get { return p.Accent; } }
            public override Color ButtonCheckedGradientBegin { get { return p.SurfaceHover; } }
            public override Color ButtonCheckedGradientMiddle { get { return p.SurfaceHover; } }
            public override Color ButtonCheckedGradientEnd { get { return p.SurfaceHover; } }
            public override Color ButtonCheckedHighlight { get { return p.SurfaceHover; } }
            public override Color OverflowButtonGradientBegin { get { return p.Window; } }
            public override Color OverflowButtonGradientMiddle { get { return p.Window; } }
            public override Color OverflowButtonGradientEnd { get { return p.Window; } }
            public override Color GripDark { get { return p.Border; } }
            public override Color GripLight { get { return p.Surface; } }
            public override Color ToolStripDropDownBackground { get { return p.Surface; } }
            public override Color ImageMarginGradientBegin { get { return p.Surface; } }
            public override Color ImageMarginGradientMiddle { get { return p.Surface; } }
            public override Color ImageMarginGradientEnd { get { return p.Surface; } }
            public override Color MenuBorder { get { return p.Border; } }
            public override Color MenuItemBorder { get { return p.Accent; } }
            public override Color MenuItemSelected { get { return p.SurfaceHover; } }
            public override Color MenuItemSelectedGradientBegin { get { return p.SurfaceHover; } }
            public override Color MenuItemSelectedGradientEnd { get { return p.SurfaceHover; } }
            public override Color MenuItemPressedGradientBegin { get { return p.SurfaceHover; } }
            public override Color MenuItemPressedGradientEnd { get { return p.SurfaceHover; } }
            public override Color SeparatorDark { get { return p.Border; } }
            public override Color SeparatorLight { get { return p.Surface; } }
            public override Color CheckBackground { get { return p.SurfaceHover; } }
            public override Color CheckSelectedBackground { get { return p.SurfaceHover; } }
            public override Color CheckPressedBackground { get { return p.SurfaceHover; } }
        }
    }
}
