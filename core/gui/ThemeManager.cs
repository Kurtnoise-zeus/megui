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
    /// Central place for the light / dark theme of MeGUI.
    /// The video player uses it first; other windows can opt in with <see cref="Apply(Control)"/>.
    /// </summary>
    public static class ThemeManager
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
                    if (mode == ThemeMode.System && (e.Category == UserPreferenceCategory.General || e.Category == UserPreferenceCategory.Color || e.Category == UserPreferenceCategory.VisualStyle))
                        RaiseThemeChanged();
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

        private sealed class ThemedToolStripRenderer : ToolStripProfessionalRenderer
        {
            public ThemedToolStripRenderer(ProfessionalColorTable table) : base(table)
            {
                RoundedEdges = false;
            }
        }

        private sealed class ThemedColorTable : ProfessionalColorTable
        {
            private readonly ThemePalette p;
            public ThemedColorTable(ThemePalette palette) { p = palette; UseSystemColors = false; }
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
