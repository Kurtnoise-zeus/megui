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

namespace MeGUI.core.gui
{
    /// <summary>
    /// Provides dark/light theme support for WinForms controls.
    /// </summary>
    public static class ThemeManager
    {
        #region Win32 interop for dark title bar
        [DllImport("dwmapi.dll", PreserveSig = true)]
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);

        private const int DWMWA_USE_IMMERSIVE_DARK_MODE_BEFORE_20H1 = 19;
        private const int DWMWA_USE_IMMERSIVE_DARK_MODE = 20;
        #endregion

        #region Theme colors
        // Dark theme palette
        private static readonly Color DarkFormBack = Color.FromArgb(255, 32, 32, 32);
        private static readonly Color DarkControlBack = Color.FromArgb(255, 45, 45, 48);
        private static readonly Color DarkFieldBack = Color.FromArgb(255, 51, 51, 55);
        private static readonly Color DarkForeColor = Color.FromArgb(255, 241, 241, 241);
        private static readonly Color DarkDisabledFore = Color.FromArgb(255, 160, 160, 160);
        private static readonly Color DarkBorderColor = Color.FromArgb(255, 67, 67, 70);
        private static readonly Color DarkHighlight = Color.FromArgb(255, 0, 122, 204);
        private static readonly Color DarkAltRowColor = Color.FromArgb(255, 42, 42, 46);
        private static readonly Color DarkGroupBoxFore = Color.FromArgb(255, 200, 200, 200);

        // Light theme palette (default WinForms system colors)
        private static readonly Color LightFormBack = SystemColors.Control;
        private static readonly Color LightControlBack = SystemColors.Control;
        private static readonly Color LightFieldBack = SystemColors.Window;
        private static readonly Color LightForeColor = SystemColors.ControlText;
        private static readonly Color LightBorderColor = SystemColors.ActiveBorder;
        private static readonly Color LightHighlight = SystemColors.Highlight;
        private static readonly Color LightAltRowColor = Color.FromArgb(255, 225, 235, 255);
        private static readonly Color LightGroupBoxFore = SystemColors.ControlText;
        #endregion

        /// <summary>
        /// Gets whether the current effective theme is dark.
        /// </summary>
        public static bool IsDarkTheme
        {
            get
            {
                if (MainForm.Instance == null)
                    return false;

                AppTheme theme = MainForm.Instance.Settings.Theme;
                if (theme == AppTheme.FollowSystem)
                    return OSInfo.IsSystemDarkTheme;
                return theme == AppTheme.Dark;
            }
        }

        /// <summary>Current form/container background color.</summary>
        public static Color FormBackColor => IsDarkTheme ? DarkFormBack : LightFormBack;

        /// <summary>Current control background color (for GroupBoxes, Panels, etc.).</summary>
        public static Color ControlBackColor => IsDarkTheme ? DarkControlBack : LightControlBack;

        /// <summary>Current input field background color (TextBox, ComboBox, ListBox, etc.).</summary>
        public static Color FieldBackColor => IsDarkTheme ? DarkFieldBack : LightFieldBack;

        /// <summary>Current foreground (text) color.</summary>
        public static Color ForeColor => IsDarkTheme ? DarkForeColor : LightForeColor;

        /// <summary>Current border color.</summary>
        public static Color BorderColor => IsDarkTheme ? DarkBorderColor : LightBorderColor;

        /// <summary>Current highlight/accent color.</summary>
        public static Color HighlightColor => IsDarkTheme ? DarkHighlight : LightHighlight;

        /// <summary>Alternating row color for list views.</summary>
        public static Color AltRowColor => IsDarkTheme ? DarkAltRowColor : LightAltRowColor;

        /// <summary>Current GroupBox text color.</summary>
        public static Color GroupBoxForeColor => IsDarkTheme ? DarkGroupBoxFore : LightGroupBoxFore;

        /// <summary>
        /// Applies the current theme to the specified form and all child controls.
        /// Also sets the title bar to dark/light mode on Windows 10+.
        /// </summary>
        public static void ApplyTheme(Form form)
        {
            if (form == null)
                return;

            ApplyToControl(form);
            SetDarkTitleBar(form, IsDarkTheme);
        }

        /// <summary>
        /// Applies the current theme to the specified control and all its children.
        /// </summary>
        public static void ApplyToControl(Control control)
        {
            if (control == null)
                return;

            bool dark = IsDarkTheme;
            ApplyThemeRecursive(control, dark);
        }

        private static void ApplyThemeRecursive(Control control, bool dark)
        {
            // Apply colors based on control type
            if (control is Form form)
            {
                form.BackColor = dark ? DarkFormBack : LightFormBack;
                form.ForeColor = dark ? DarkForeColor : LightForeColor;
            }
            else if (control is TabControl tabCtrl)
            {
                // TabControl: keep default rendering for tab headers
                tabCtrl.BackColor = dark ? DarkControlBack : LightControlBack;
                tabCtrl.ForeColor = dark ? DarkForeColor : LightForeColor;
            }
            else if (control is TabPage tabPage)
            {
                tabPage.BackColor = dark ? DarkControlBack : LightControlBack;
                tabPage.ForeColor = dark ? DarkForeColor : LightForeColor;
                tabPage.UseVisualStyleBackColor = false;
            }
            else if (control is MenuStrip || control is ToolStrip)
            {
                control.BackColor = dark ? DarkControlBack : LightControlBack;
                control.ForeColor = dark ? DarkForeColor : LightForeColor;
                if (control is MenuStrip menuStrip)
                {
                    menuStrip.Renderer = dark
                        ? (ToolStripRenderer)new DarkToolStripRenderer()
                        : new ToolStripProfessionalRenderer();
                }
            }
            else if (control is GroupBox groupBox)
            {
                groupBox.BackColor = dark ? DarkControlBack : LightControlBack;
                groupBox.ForeColor = dark ? DarkGroupBoxFore : LightGroupBoxFore;
            }
            else if (control is TextBox textBox)
            {
                textBox.BackColor = dark ? DarkFieldBack : LightFieldBack;
                textBox.ForeColor = dark ? DarkForeColor : LightForeColor;
                textBox.BorderStyle = dark ? BorderStyle.FixedSingle : BorderStyle.Fixed3D;
            }
            else if (control is RichTextBox rtb)
            {
                rtb.BackColor = dark ? DarkFieldBack : LightFieldBack;
                rtb.ForeColor = dark ? DarkForeColor : LightForeColor;
            }
            else if (control is ListBox listBox)
            {
                listBox.BackColor = dark ? DarkFieldBack : LightFieldBack;
                listBox.ForeColor = dark ? DarkForeColor : LightForeColor;
            }
            else if (control is CheckedListBox clb)
            {
                clb.BackColor = dark ? DarkFieldBack : LightFieldBack;
                clb.ForeColor = dark ? DarkForeColor : LightForeColor;
            }
            else if (control is ListView listView)
            {
                listView.BackColor = dark ? DarkFieldBack : LightFieldBack;
                listView.ForeColor = dark ? DarkForeColor : LightForeColor;
            }
            else if (control is TreeView treeView)
            {
                treeView.BackColor = dark ? DarkFieldBack : LightFieldBack;
                treeView.ForeColor = dark ? DarkForeColor : LightForeColor;
            }
            else if (control is DataGridView dgv)
            {
                dgv.BackgroundColor = dark ? DarkFieldBack : SystemColors.Window;
                dgv.ForeColor = dark ? DarkForeColor : LightForeColor;
                dgv.GridColor = dark ? DarkBorderColor : SystemColors.ControlDark;
                dgv.DefaultCellStyle.BackColor = dark ? DarkFieldBack : SystemColors.Window;
                dgv.DefaultCellStyle.ForeColor = dark ? DarkForeColor : SystemColors.ControlText;
                dgv.ColumnHeadersDefaultCellStyle.BackColor = dark ? DarkControlBack : SystemColors.Control;
                dgv.ColumnHeadersDefaultCellStyle.ForeColor = dark ? DarkForeColor : SystemColors.ControlText;
                dgv.RowHeadersDefaultCellStyle.BackColor = dark ? DarkControlBack : SystemColors.Control;
                dgv.RowHeadersDefaultCellStyle.ForeColor = dark ? DarkForeColor : SystemColors.ControlText;
                dgv.EnableHeadersVisualStyles = !dark;
            }
            else if (control is ComboBox comboBox)
            {
                comboBox.BackColor = dark ? DarkFieldBack : LightFieldBack;
                comboBox.ForeColor = dark ? DarkForeColor : LightForeColor;
                comboBox.FlatStyle = dark ? FlatStyle.Flat : FlatStyle.Standard;
            }
            else if (control is NumericUpDown nud)
            {
                nud.BackColor = dark ? DarkFieldBack : LightFieldBack;
                nud.ForeColor = dark ? DarkForeColor : LightForeColor;
            }
            else if (control is Button btn)
            {
                btn.BackColor = dark ? DarkControlBack : LightControlBack;
                btn.ForeColor = dark ? DarkForeColor : LightForeColor;
                btn.FlatStyle = dark ? FlatStyle.Flat : FlatStyle.Standard;
                if (dark)
                {
                    btn.FlatAppearance.BorderColor = DarkBorderColor;
                    btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(255, 62, 62, 66);
                    btn.FlatAppearance.MouseDownBackColor = DarkHighlight;
                }
                btn.UseVisualStyleBackColor = !dark;
            }
            else if (control is CheckBox checkBox)
            {
                checkBox.BackColor = Color.Transparent;
                checkBox.ForeColor = dark ? DarkForeColor : LightForeColor;
            }
            else if (control is RadioButton radio)
            {
                radio.BackColor = Color.Transparent;
                radio.ForeColor = dark ? DarkForeColor : LightForeColor;
            }
            else if (control is Label label)
            {
                // Keep Transparent labels transparent
                if (label.BackColor != Color.Transparent)
                    label.BackColor = dark ? DarkControlBack : LightControlBack;
                label.ForeColor = dark ? DarkForeColor : LightForeColor;
            }
            else if (control is Panel || control is SplitContainer || control is FlowLayoutPanel
                     || control is TableLayoutPanel || control is SplitterPanel)
            {
                control.BackColor = dark ? DarkControlBack : LightControlBack;
                control.ForeColor = dark ? DarkForeColor : LightForeColor;
            }
            else if (control is ProgressBar)
            {
                // ProgressBar handles its own painting via CustomProgressBar
                control.BackColor = dark ? DarkControlBack : LightControlBack;
            }
            else if (control is StatusStrip statusStrip)
            {
                statusStrip.BackColor = dark ? DarkControlBack : LightControlBack;
                statusStrip.ForeColor = dark ? DarkForeColor : LightForeColor;
            }
            else if (control is TrackBar tb)
            {
                tb.BackColor = dark ? DarkControlBack : LightControlBack;
            }
            else
            {
                // Generic fallback
                if (!(control.BackColor == Color.Transparent))
                {
                    control.BackColor = dark ? DarkControlBack : LightControlBack;
                }
                control.ForeColor = dark ? DarkForeColor : LightForeColor;
            }

            // Recurse into children (includes SplitContainer panels)
            if (control is SplitContainer splitContainer)
            {
                ApplyThemeRecursive(splitContainer.Panel1, dark);
                ApplyThemeRecursive(splitContainer.Panel2, dark);
            }

            foreach (Control child in control.Controls)
            {
                ApplyThemeRecursive(child, dark);
            }
        }

        /// <summary>
        /// Enables or disables the dark title bar on Windows 10 version 1809+ via DWM.
        /// </summary>
        private static void SetDarkTitleBar(Form form, bool dark)
        {
            if (!form.IsHandleCreated)
                return;

            try
            {
                int useDark = dark ? 1 : 0;
                // Try the newer attribute first (20H1+), then fall back to pre-20H1
                if (DwmSetWindowAttribute(form.Handle, DWMWA_USE_IMMERSIVE_DARK_MODE, ref useDark, sizeof(int)) != 0)
                    DwmSetWindowAttribute(form.Handle, DWMWA_USE_IMMERSIVE_DARK_MODE_BEFORE_20H1, ref useDark, sizeof(int));
            }
            catch
            {
                // DWM call may fail on older Windows versions; silently ignore
            }
        }

        /// <summary>
        /// Applies the dark title bar setting to a form. Call from HandleCreated or Load events.
        /// </summary>
        public static void SetupDarkTitleBar(Form form)
        {
            SetDarkTitleBar(form, IsDarkTheme);
        }

        /// <summary>
        /// Gets a theme-aware text brush for custom-painted controls.
        /// The caller must NOT dispose this brush (it returns a system Brush).
        /// </summary>
        public static Brush TextBrush => IsDarkTheme ? Brushes.White : Brushes.Black;
    }

    #region Dark ToolStrip/MenuStrip renderer
    /// <summary>
    /// Custom renderer for dark-themed MenuStrip and ToolStrip controls.
    /// </summary>
    internal class DarkToolStripRenderer : ToolStripProfessionalRenderer
    {
        private static readonly Color DarkMenuBack = Color.FromArgb(255, 45, 45, 48);
        private static readonly Color DarkMenuBorder = Color.FromArgb(255, 67, 67, 70);
        private static readonly Color DarkMenuHighlight = Color.FromArgb(255, 62, 62, 66);
        private static readonly Color DarkMenuFore = Color.FromArgb(255, 241, 241, 241);

        public DarkToolStripRenderer() : base(new DarkColorTable()) { }

        protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
        {
            Rectangle rc = new Rectangle(Point.Empty, e.Item.Size);
            Color color = e.Item.Selected ? DarkMenuHighlight : DarkMenuBack;
            using (SolidBrush brush = new SolidBrush(color))
                e.Graphics.FillRectangle(brush, rc);
        }

        protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
        {
            e.TextColor = DarkMenuFore;
            base.OnRenderItemText(e);
        }

        protected override void OnRenderSeparator(ToolStripSeparatorRenderEventArgs e)
        {
            int y = e.Item.Height / 2;
            using (Pen pen = new Pen(DarkMenuBorder))
                e.Graphics.DrawLine(pen, 0, y, e.Item.Width, y);
        }

        protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
        {
            using (SolidBrush brush = new SolidBrush(DarkMenuBack))
                e.Graphics.FillRectangle(brush, e.AffectedBounds);
        }

        protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
        {
            using (Pen pen = new Pen(DarkMenuBorder))
                e.Graphics.DrawRectangle(pen, 0, 0, e.AffectedBounds.Width - 1, e.AffectedBounds.Height - 1);
        }

        protected override void OnRenderImageMargin(ToolStripRenderEventArgs e)
        {
            // Don't draw the image margin background in dark mode
        }
    }

    internal class DarkColorTable : ProfessionalColorTable
    {
        private static readonly Color DarkBack = Color.FromArgb(255, 45, 45, 48);
        private static readonly Color DarkBorder = Color.FromArgb(255, 67, 67, 70);
        private static readonly Color DarkHighlight = Color.FromArgb(255, 62, 62, 66);

        public override Color MenuItemSelected => DarkHighlight;
        public override Color MenuItemBorder => DarkBorder;
        public override Color MenuBorder => DarkBorder;
        public override Color MenuItemSelectedGradientBegin => DarkHighlight;
        public override Color MenuItemSelectedGradientEnd => DarkHighlight;
        public override Color MenuItemPressedGradientBegin => DarkHighlight;
        public override Color MenuItemPressedGradientEnd => DarkHighlight;
        public override Color MenuStripGradientBegin => DarkBack;
        public override Color MenuStripGradientEnd => DarkBack;
        public override Color ToolStripDropDownBackground => DarkBack;
        public override Color ImageMarginGradientBegin => DarkBack;
        public override Color ImageMarginGradientMiddle => DarkBack;
        public override Color ImageMarginGradientEnd => DarkBack;
        public override Color SeparatorDark => DarkBorder;
        public override Color SeparatorLight => DarkBorder;
    }
    #endregion
}
