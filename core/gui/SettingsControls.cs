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
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace MeGUI.core.gui
{
    /// <summary>
    /// Controls of the Options window, drawn like the Windows 11 settings: a navigation list on the left,
    /// pages made of sections and rounded cards, On / Off switches. They are painted with the palette of
    /// <see cref="ThemeManager"/> (light and dark) and follow the theme changes by themselves.
    /// </summary>
    internal static class SettingsDraw
    {
        public static GraphicsPath RoundRect(RectangleF r, float radius)
        {
            GraphicsPath path = new GraphicsPath();
            float d = radius * 2f;
            if (radius <= 0f || d > r.Width || d > r.Height)
            {
                path.AddRectangle(r);
                return path;
            }
            path.AddArc(r.X, r.Y, d, d, 180, 90);
            path.AddArc(r.Right - d, r.Y, d, d, 270, 90);
            path.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
            path.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }

        /// <summary>colour of the thin lines (card border, separators): a bit of the border colour over the card</summary>
        public static Color Line(Color back)
        {
            return ThemeManager.Blend(back, ThemeManager.Palette.Border, ThemeManager.IsDark ? 0.45 : 0.55);
        }

        /// <summary>the name of the installed icon font (Windows 11 / Windows 10), null if there is none</summary>
        public static string IconFont()
        {
            foreach (string name in new string[] { "Segoe Fluent Icons", "Segoe MDL2 Assets" })
            {
                foreach (FontFamily family in FontFamily.Families)
                {
                    if (string.Equals(family.Name, name, StringComparison.OrdinalIgnoreCase))
                        return name;
                }
            }
            return null;
        }
    }

    /// <summary>
    /// Window / footer background of the Options window.
    /// </summary>
    public enum SettingsSurfaceKind
    {
        Window,
        Footer
    }

    public class SettingsSurface : Panel
    {
        private readonly SettingsSurfaceKind kind;

        public SettingsSurface(SettingsSurfaceKind kind)
        {
            this.kind = kind;
            SetStyle(ControlStyles.ResizeRedraw, true);
            ApplyTheme();
        }

        private void ApplyTheme()
        {
            ThemePalette p = ThemeManager.Palette;
            BackColor = kind == SettingsSurfaceKind.Footer
                ? ThemeManager.Blend(p.Window, Color.Black, ThemeManager.IsDark ? 0.12 : 0.04)
                : p.Window;
            Invalidate();
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            ThemeManager.ThemeChanged += OnTheme;
            ApplyTheme();
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            ThemeManager.ThemeChanged -= OnTheme;
            base.OnHandleDestroyed(e);
        }

        private void OnTheme(object sender, EventArgs e)
        {
            if (IsHandleCreated && !IsDisposed)
                BeginInvoke(new MethodInvoker(ApplyTheme));
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (kind == SettingsSurfaceKind.Footer)
            {
                using (Pen pen = new Pen(ThemeManager.Blend(BackColor, ThemeManager.Palette.Text, 0.10)))
                    e.Graphics.DrawLine(pen, 0, 0, Width, 0);
            }
        }
    }

    /// <summary>
    /// On / Off switch. It is a CheckBox (Checked, CheckedChanged, Enabled work as before); its Text is not
    /// drawn by the switch, the card shows it as the title of the row.
    /// </summary>
    public class ToggleSwitch : CheckBox
    {
        private bool hot;

        public ToggleSwitch()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
            AutoSize = false;
            Cursor = Cursors.Hand;
            Size = new Size(96, 26);
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            ThemeManager.ThemeChanged += OnTheme;
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            ThemeManager.ThemeChanged -= OnTheme;
            base.OnHandleDestroyed(e);
        }

        private void OnTheme(object sender, EventArgs e)
        {
            if (IsHandleCreated && !IsDisposed)
                BeginInvoke(new MethodInvoker(Invalidate));
        }

        protected override void OnCheckedChanged(EventArgs e)
        {
            Invalidate();
            base.OnCheckedChanged(e);
        }

        protected override void OnEnabledChanged(EventArgs e)
        {
            Invalidate();
            base.OnEnabledChanged(e);
        }

        protected override void OnGotFocus(EventArgs e)
        {
            Invalidate();
            base.OnGotFocus(e);
        }

        protected override void OnLostFocus(EventArgs e)
        {
            Invalidate();
            base.OnLostFocus(e);
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            hot = true;
            Invalidate();
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            hot = false;
            Invalidate();
            base.OnMouseLeave(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            ThemePalette p = ThemeManager.Palette;
            Graphics g = e.Graphics;
            Color back = Parent != null ? Parent.BackColor : p.Surface;
            g.Clear(back);

            float s = Height / 26f;
            int trackW = (int)Math.Round(40 * s);
            int trackH = (int)Math.Round(20 * s);
            int trackX = Width - trackW - 1;
            int trackY = (Height - trackH) / 2;
            bool on = Checked;
            int alpha = Enabled ? 255 : 110;

            // "On" / "Off" on the left of the switch
            Rectangle textRect = new Rectangle(0, 0, Math.Max(0, trackX - (int)(10 * s)), Height);
            TextRenderer.DrawText(g, on ? "On" : "Off", Font, textRect, Enabled ? p.Text : p.TextDim,
                TextFormatFlags.Right | TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine | TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix);

            g.SmoothingMode = SmoothingMode.AntiAlias;
            RectangleF track = new RectangleF(trackX + 0.5f, trackY + 0.5f, trackW - 1f, trackH - 1f);
            using (GraphicsPath path = SettingsDraw.RoundRect(track, track.Height / 2f))
            {
                if (on)
                {
                    using (SolidBrush fill = new SolidBrush(Color.FromArgb(alpha, p.Accent)))
                        g.FillPath(fill, path);
                }
                else
                {
                    Color line = hot ? p.Text : p.TextDim;
                    using (Pen pen = new Pen(Color.FromArgb(alpha, line), Math.Max(1f, 1.2f * s)))
                        g.DrawPath(pen, path);
                }
            }

            float d = on ? trackH * 0.6f : trackH * 0.5f;
            float margin = (trackH - d) / 2f;
            float thumbX = on ? trackX + trackW - margin - d - 1f : trackX + margin + 1f;
            RectangleF thumb = new RectangleF(thumbX, trackY + margin, d, d);
            using (SolidBrush brush = new SolidBrush(Color.FromArgb(alpha, on ? p.OnAccent : p.TextDim)))
                g.FillEllipse(brush, thumb);

            if (Focused && ShowFocusCues)
            {
                RectangleF focus = RectangleF.Inflate(track, 3f * s, 3f * s);
                using (GraphicsPath path = SettingsDraw.RoundRect(focus, focus.Height / 2f))
                using (Pen pen = new Pen(p.Text, 1f))
                    g.DrawPath(pen, path);
            }
            g.SmoothingMode = SmoothingMode.Default;
        }
    }

    /// <summary>
    /// A rounded card made of rows: a title (and a description) on the left, the controls of the setting on
    /// the right. The controls are the existing controls of the form, they are only moved into the card.
    /// </summary>
    public class SettingsCard : Panel
    {
        private sealed class Row
        {
            public string Title;
            public Label TitleLabel;
            public string Description;
            public bool Sub;
            public Control Left;
            public readonly List<Control> Right = new List<Control>();
            public Control Wide;
            public readonly List<Control> Aside = new List<Control>();
            public int Top;
            public int Height;
            public int HeaderHeight;
            public int TextRight;
        }

        private readonly List<Row> rows = new List<Row>();
        private readonly float scale;
        private Font descriptionFont;

        public SettingsCard(float scale)
        {
            this.scale = scale;
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
            BackColor = ThemeManager.Palette.Surface;
            Height = 0;
        }

        private int S(int value)
        {
            return (int)Math.Round(value * scale);
        }

        #region rows
        /// <summary>a row with a toggle switch (its Text is the title); other controls can be put on the left of the switch</summary>
        public SettingsCard AddToggle(ToggleSwitch toggle, string description, params Control[] leftOfSwitch)
        {
            List<Control> right = new List<Control>();
            if (leftOfSwitch != null)
                right.AddRange(leftOfSwitch);
            right.Add(toggle);
            return AddRow(toggle.Text, description, right.ToArray());
        }

        /// <summary>a row with a title and controls on the right (listed from left to right)</summary>
        public SettingsCard AddRow(string title, string description, params Control[] right)
        {
            Row row = new Row();
            row.Title = title;
            row.Description = description;
            row.HeaderHeight = string.IsNullOrEmpty(description) ? S(56) : S(64);
            row.Height = row.HeaderHeight;
            if (right != null)
                row.Right.AddRange(right);
            return Add(row);
        }

        /// <summary>
        /// a row whose title is an existing label (not added to the card): its Text, Enabled and Visible
        /// are followed, so the code which changes the label keeps working
        /// </summary>
        public SettingsCard AddLabelRow(Label title, string description, params Control[] right)
        {
            Row row = new Row();
            row.TitleLabel = title;
            row.Description = description;
            row.HeaderHeight = string.IsNullOrEmpty(description) ? S(56) : S(64);
            row.Height = row.HeaderHeight;
            if (right != null)
                row.Right.AddRange(right);
            title.TextChanged += delegate { Invalidate(); };
            title.EnabledChanged += delegate { Invalidate(); };
            title.VisibleChanged += delegate { PerformLayout(); };
            return Add(row);
        }

        /// <summary>a row under the first one, indented (details of the setting above)</summary>
        public SettingsCard AddSubRow(string title, params Control[] right)
        {
            Row row = new Row();
            row.Title = title;
            row.Sub = true;
            row.HeaderHeight = S(52);
            row.Height = row.HeaderHeight;
            if (right != null)
                row.Right.AddRange(right);
            return Add(row);
        }

        /// <summary>a row with a control on the left (radio button, check box), optionally followed by other controls</summary>
        public SettingsCard AddLeft(Control left, params Control[] after)
        {
            Row row = new Row();
            row.Left = left;
            row.HeaderHeight = S(48);
            row.Height = row.HeaderHeight;
            if (after != null)
                row.Right.AddRange(after);
            return Add(row);
        }

        /// <summary>
        /// a row with a big control (list) under the title; aside: controls stacked at the right of it
        /// </summary>
        public SettingsCard AddWide(string title, string description, Control wide, params Control[] aside)
        {
            Row row = new Row();
            row.Title = title;
            row.Description = description;
            row.Wide = wide;
            wide.VisibleChanged += delegate { PerformLayout(); };
            if (aside != null)
                row.Aside.AddRange(aside);
            if (string.IsNullOrEmpty(title))
            {
                row.HeaderHeight = 0;
                row.Height = wide.Height + S(24);
            }
            else
            {
                row.HeaderHeight = string.IsNullOrEmpty(description) ? S(56) : S(64);
                row.Height = row.HeaderHeight - S(8) + wide.Height + S(16);
            }
            return Add(row);
        }

        private SettingsCard Add(Row row)
        {
            rows.Add(row);

            List<Control> controls = new List<Control>();
            if (row.Left != null)
                controls.Add(row.Left);
            controls.AddRange(row.Right);
            if (row.Wide != null)
                controls.Add(row.Wide);
            controls.AddRange(row.Aside);
            foreach (Control c in controls)
            {
                c.Dock = DockStyle.None;
                c.Anchor = AnchorStyles.Top | AnchorStyles.Left;
                Controls.Add(c);
            }

            ComputeTops();
            PerformLayout();
            return this;
        }

        // a row whose title label is hidden by the code takes no room
        private bool Collapsed(Row row)
        {
            if (row.TitleLabel != null)
                return !row.TitleLabel.Visible;
            // a big control without a title which the code hides (a track bar...): no room while it is hidden
            // (when the card itself is hidden, e.g. on another tab, Visible is false for all the controls)
            return row.Wide != null && string.IsNullOrEmpty(row.Title) && Visible && !row.Wide.Visible;
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);
            PerformLayout();
        }

        private void ComputeTops()
        {
            int top = 0;
            foreach (Row r in rows)
            {
                r.Top = top;
                if (!Collapsed(r))
                    top += r.Height;
            }
            if (Height != top)
                Height = top;
        }
        #endregion

        protected override void OnLayout(LayoutEventArgs levent)
        {
            // positions are computed here, the anchors / docks of the controls are not used
            int pad = S(16);
            int gap = S(8);
            ComputeTops();
            foreach (Row row in rows)
            {
                if (row.Left != null)
                {
                    row.Left.Left = S(18);
                    row.Left.Top = row.Top + (row.HeaderHeight - row.Left.Height) / 2;
                    int x = row.Left.Right + S(12);
                    foreach (Control c in row.Right)
                    {
                        c.Left = x;
                        c.Top = row.Top + (row.HeaderHeight - c.Height) / 2;
                        x = c.Right + gap;
                    }
                    row.TextRight = Width - pad;
                    continue;
                }

                int right = Width - pad;
                for (int i = row.Right.Count - 1; i >= 0; i--)
                {
                    Control c = row.Right[i];
                    c.Left = right - c.Width;
                    c.Top = row.Top + (row.HeaderHeight - c.Height) / 2;
                    right = c.Left - gap;
                }
                row.TextRight = right - gap;

                if (row.Wide != null)
                {
                    int wideTop = row.Top + (row.HeaderHeight > 0 ? row.HeaderHeight - S(8) : S(12));
                    int asideWidth = row.Aside.Count > 0 ? S(230) : 0;
                    int wideWidth = Width - 2 * pad - (asideWidth > 0 ? asideWidth + S(16) : 0);
                    row.Wide.SetBounds(pad, wideTop, Math.Max(S(100), wideWidth), row.Wide.Height);
                    int y = wideTop;
                    foreach (Control c in row.Aside)
                    {
                        c.SetBounds(Width - pad - asideWidth, y, asideWidth, c.Height);
                        y += c.Height + gap;
                    }
                }
            }
            Invalidate();
        }

        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);
            Invalidate();
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            ThemeManager.ThemeChanged += OnTheme;
            BackColor = ThemeManager.Palette.Surface;
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            ThemeManager.ThemeChanged -= OnTheme;
            base.OnHandleDestroyed(e);
        }

        private void OnTheme(object sender, EventArgs e)
        {
            if (IsHandleCreated && !IsDisposed)
                BeginInvoke(new MethodInvoker(delegate
                {
                    BackColor = ThemeManager.Palette.Surface;
                    Invalidate(true);
                }));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && descriptionFont != null)
            {
                descriptionFont.Dispose();
                descriptionFont = null;
            }
            base.Dispose(disposing);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            ThemePalette p = ThemeManager.Palette;
            g.Clear(Parent != null ? Parent.BackColor : p.Window);

            Color line = SettingsDraw.Line(BackColor);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            using (GraphicsPath path = SettingsDraw.RoundRect(new RectangleF(0.5f, 0.5f, Width - 1f, Height - 1f), S(4)))
            using (SolidBrush fill = new SolidBrush(BackColor))
            using (Pen border = new Pen(line))
            {
                g.FillPath(fill, path);
                g.DrawPath(border, path);
            }
            g.SmoothingMode = SmoothingMode.Default;

            if (descriptionFont == null || Math.Abs(descriptionFont.Size - Font.Size * 0.85f) > 0.01f)
            {
                if (descriptionFont != null)
                    descriptionFont.Dispose();
                descriptionFont = new Font(Font.FontFamily, Font.Size * 0.85f);
            }

            TextFormatFlags flags = TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis | TextFormatFlags.NoPrefix | TextFormatFlags.SingleLine | TextFormatFlags.NoPadding;
            bool first = true;
            foreach (Row row in rows)
            {
                if (Collapsed(row))
                    continue;
                if (!first)
                {
                    using (Pen sep = new Pen(line))
                        g.DrawLine(sep, 1, row.Top, Width - 2, row.Top);
                }
                first = false;

                string title = row.TitleLabel != null ? row.TitleLabel.Text : row.Title;
                Color titleColor = Enabled && (row.TitleLabel == null || row.TitleLabel.Enabled) ? p.Text : p.TextDim;
                if (string.IsNullOrEmpty(title))
                    continue;
                int x = S(18) + (row.Sub ? S(34) : 0);
                int width = Math.Max(0, (row.TextRight > 0 ? row.TextRight : Width - S(16)) - x);
                if (string.IsNullOrEmpty(row.Description))
                {
                    TextRenderer.DrawText(g, title, Font, new Rectangle(x, row.Top, width, row.HeaderHeight), titleColor, flags);
                }
                else
                {
                    TextRenderer.DrawText(g, title, Font, new Rectangle(x, row.Top + S(10), width, S(22)), titleColor, flags);
                    TextRenderer.DrawText(g, row.Description, descriptionFont, new Rectangle(x, row.Top + S(33), width, S(20)), p.TextDim, flags);
                }
            }
        }
    }

    /// <summary>
    /// A page of the Options window: sections (a small title) of cards, scrolled vertically when needed.
    /// </summary>
    public class SettingsPage : FlowLayoutPanel
    {
        private readonly float scale;

        public SettingsPage(float scale)
        {
            this.scale = scale;
            FlowDirection = FlowDirection.TopDown;
            WrapContents = false;
            AutoScroll = true;
            Dock = DockStyle.Fill;
            Padding = new Padding((int)Math.Round(36 * scale), 0, (int)Math.Round(36 * scale), (int)Math.Round(24 * scale));
            BackColor = ThemeManager.Palette.Window;
        }

        /// <summary>a section title; the cards added after it belong to it</summary>
        public Label AddSection(string title, Font font)
        {
            Label label = new Label();
            label.Text = title;
            label.AutoSize = false;
            label.Height = (int)Math.Round(26 * scale);
            label.Margin = new Padding(2, (int)Math.Round(16 * scale), 0, (int)Math.Round(4 * scale));
            label.TextAlign = ContentAlignment.BottomLeft;
            label.UseMnemonic = false;
            label.Font = font;
            Controls.Add(label);
            return label;
        }

        public SettingsCard AddCard()
        {
            SettingsCard card = new SettingsCard(scale);
            card.Margin = new Padding(0, 0, 0, (int)Math.Round(3 * scale));
            Controls.Add(card);
            return card;
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            ThemeManager.ThemeChanged += OnTheme;
            BackColor = ThemeManager.Palette.Window;
            FitChildren();
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            ThemeManager.ThemeChanged -= OnTheme;
            base.OnHandleDestroyed(e);
        }

        private void OnTheme(object sender, EventArgs e)
        {
            if (IsHandleCreated && !IsDisposed)
                BeginInvoke(new MethodInvoker(delegate
                {
                    BackColor = ThemeManager.Palette.Window;
                    Invalidate(true);
                }));
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            FitChildren();
        }

        /// <summary>the cards and the section titles are as wide as the page</summary>
        private void FitChildren()
        {
            int available = ClientSize.Width - Padding.Horizontal;
            if (available <= 0)
                return;
            foreach (Control c in Controls)
            {
                int width = Math.Max(0, available - c.Margin.Horizontal);
                if (c.Width != width)
                    c.Width = width;
            }
        }

        protected override void OnControlAdded(ControlEventArgs e)
        {
            base.OnControlAdded(e);
            int width = Math.Max(0, ClientSize.Width - Padding.Horizontal - e.Control.Margin.Horizontal);
            if (width > 0)
                e.Control.Width = width;
        }
    }

    /// <summary>
    /// Navigation list on the left of the Options window (one entry per page).
    /// </summary>
    public class SettingsNav : Control
    {
        private sealed class Item
        {
            public string Text;
            public string Glyph;
        }

        private readonly List<Item> items = new List<Item>();
        private readonly float scale;
        private readonly string iconFont;
        private Font glyphFont;
        private int selected = -1;
        private int hot = -1;

        public event EventHandler SelectedIndexChanged;

        public SettingsNav(float scale)
        {
            this.scale = scale;
            iconFont = SettingsDraw.IconFont();
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.Selectable, true);
            TabStop = true;
            Cursor = Cursors.Hand;
            BackColor = ThemeManager.Palette.Window;
        }

        public void AddItem(string text, string glyph)
        {
            Item item = new Item();
            item.Text = text;
            item.Glyph = glyph;
            items.Add(item);
            Invalidate();
        }

        public int SelectedIndex
        {
            get { return selected; }
            set
            {
                if (value < 0 || value >= items.Count || value == selected)
                    return;
                selected = value;
                Invalidate();
                EventHandler handler = SelectedIndexChanged;
                if (handler != null)
                    handler(this, EventArgs.Empty);
            }
        }

        private int S(int value)
        {
            return (int)Math.Round(value * scale);
        }

        private Rectangle ItemRect(int index)
        {
            int height = S(40);
            return new Rectangle(S(10), S(6) + index * (height + S(2)), Math.Max(0, Width - S(20)), height);
        }

        private int ItemAt(Point location)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (ItemRect(i).Contains(location))
                    return i;
            }
            return -1;
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            ThemeManager.ThemeChanged += OnTheme;
            BackColor = ThemeManager.Palette.Window;
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            ThemeManager.ThemeChanged -= OnTheme;
            base.OnHandleDestroyed(e);
        }

        private void OnTheme(object sender, EventArgs e)
        {
            if (IsHandleCreated && !IsDisposed)
                BeginInvoke(new MethodInvoker(delegate
                {
                    BackColor = ThemeManager.Palette.Window;
                    Invalidate();
                }));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && glyphFont != null)
            {
                glyphFont.Dispose();
                glyphFont = null;
            }
            base.Dispose(disposing);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            int index = ItemAt(e.Location);
            if (index != hot)
            {
                hot = index;
                Invalidate();
            }
            base.OnMouseMove(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            hot = -1;
            Invalidate();
            base.OnMouseLeave(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            Focus();
            int index = ItemAt(e.Location);
            if (index >= 0)
                SelectedIndex = index;
            base.OnMouseDown(e);
        }

        protected override bool IsInputKey(Keys keyData)
        {
            if (keyData == Keys.Up || keyData == Keys.Down)
                return true;
            return base.IsInputKey(keyData);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                SelectedIndex = Math.Min(items.Count - 1, selected + 1);
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Up)
            {
                SelectedIndex = Math.Max(0, selected - 1);
                e.Handled = true;
            }
            base.OnKeyDown(e);
        }

        protected override void OnGotFocus(EventArgs e)
        {
            Invalidate();
            base.OnGotFocus(e);
        }

        protected override void OnLostFocus(EventArgs e)
        {
            Invalidate();
            base.OnLostFocus(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            ThemePalette p = ThemeManager.Palette;
            g.Clear(p.Window);

            if (iconFont != null && glyphFont == null)
                glyphFont = new Font(iconFont, 11f);

            TextFormatFlags flags = TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis | TextFormatFlags.NoPrefix | TextFormatFlags.SingleLine | TextFormatFlags.NoPadding;
            for (int i = 0; i < items.Count; i++)
            {
                Rectangle r = ItemRect(i);
                bool isSelected = i == selected;
                if (isSelected || i == hot)
                {
                    Color fill = ThemeManager.Blend(p.Window, p.Text, isSelected ? 0.07 : 0.04);
                    g.SmoothingMode = SmoothingMode.AntiAlias;
                    using (GraphicsPath path = SettingsDraw.RoundRect(new RectangleF(r.X, r.Y, r.Width, r.Height), S(5)))
                    using (SolidBrush brush = new SolidBrush(fill))
                        g.FillPath(brush, path);
                    g.SmoothingMode = SmoothingMode.Default;
                }
                if (isSelected)
                {
                    g.SmoothingMode = SmoothingMode.AntiAlias;
                    using (GraphicsPath pill = SettingsDraw.RoundRect(new RectangleF(r.X, r.Y + (r.Height - S(16)) / 2f, S(3), S(16)), 1.5f * scale))
                    using (SolidBrush brush = new SolidBrush(p.Accent))
                        g.FillPath(brush, pill);
                    g.SmoothingMode = SmoothingMode.Default;
                }

                int textX = r.X + S(16);
                if (glyphFont != null && !string.IsNullOrEmpty(items[i].Glyph))
                {
                    TextRenderer.DrawText(g, items[i].Glyph, glyphFont, new Rectangle(r.X + S(14), r.Y, S(24), r.Height), p.Text,
                        TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine | TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix);
                    textX = r.X + S(48);
                }
                TextRenderer.DrawText(g, items[i].Text, Font, new Rectangle(textX, r.Y, Math.Max(0, r.Right - textX - S(6)), r.Height), p.Text, flags);

                if (isSelected && Focused && ShowFocusCues)
                {
                    using (Pen pen = new Pen(p.Text) { DashStyle = DashStyle.Dot })
                        g.DrawRectangle(pen, r.X + 1, r.Y + 1, r.Width - 3, r.Height - 3);
                }
            }
        }
    }

    /// <summary>
    /// Button with the rounded corners of the Windows 11 buttons. It is painted with BackColor, ForeColor and
    /// FlatAppearance (BorderColor, MouseOverBackColor, MouseDownBackColor), which the Options window sets
    /// from the palette; Click, DialogResult, Enabled work as for any Button.
    /// </summary>
    public class RoundedButton : Button
    {
        private bool hot;
        private bool pressed;

        public RoundedButton()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            hot = true;
            Invalidate();
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            hot = false;
            pressed = false;
            Invalidate();
            base.OnMouseLeave(e);
        }

        protected override void OnMouseDown(MouseEventArgs mevent)
        {
            if (mevent.Button == MouseButtons.Left)
            {
                pressed = true;
                Invalidate();
            }
            base.OnMouseDown(mevent);
        }

        protected override void OnMouseUp(MouseEventArgs mevent)
        {
            pressed = false;
            Invalidate();
            base.OnMouseUp(mevent);
        }

        protected override void OnKeyDown(KeyEventArgs kevent)
        {
            if (kevent.KeyCode == Keys.Space)
            {
                pressed = true;
                Invalidate();
            }
            base.OnKeyDown(kevent);
        }

        protected override void OnKeyUp(KeyEventArgs kevent)
        {
            pressed = false;
            Invalidate();
            base.OnKeyUp(kevent);
        }

        protected override void OnEnabledChanged(EventArgs e)
        {
            Invalidate();
            base.OnEnabledChanged(e);
        }

        protected override void OnGotFocus(EventArgs e)
        {
            Invalidate();
            base.OnGotFocus(e);
        }

        protected override void OnLostFocus(EventArgs e)
        {
            Invalidate();
            base.OnLostFocus(e);
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            Graphics g = pevent.Graphics;
            // the first opaque background behind the button (a transparent parent would clear to black)
            Color outside = ThemeManager.Palette.Window;
            for (Control parent = Parent; parent != null; parent = parent.Parent)
            {
                if (parent.BackColor.A == 255)
                {
                    outside = parent.BackColor;
                    break;
                }
            }
            g.Clear(outside);

            // a button which is not styled (light theme, no ModernDialog): the colours of the palette
            bool unstyled = UseVisualStyleBackColor || BackColor.A < 255;
            ThemePalette pal = ThemeManager.Palette;
            bool darkTheme = ThemeManager.IsDark;
            Color back = unstyled ? ThemeManager.Blend(pal.Surface, pal.Text, darkTheme ? 0.08 : 0.02) : BackColor;
            if (Enabled && pressed)
                back = unstyled ? ThemeManager.Blend(pal.Surface, pal.Text, 0.10) : (FlatAppearance.MouseDownBackColor.IsEmpty ? back : FlatAppearance.MouseDownBackColor);
            else if (Enabled && hot)
                back = unstyled ? ThemeManager.Blend(pal.Surface, pal.Text, 0.14) : (FlatAppearance.MouseOverBackColor.IsEmpty ? back : FlatAppearance.MouseOverBackColor);
            Color border = unstyled ? ThemeManager.Blend(pal.Surface, pal.Text, 0.18) : (FlatAppearance.BorderColor.IsEmpty ? back : FlatAppearance.BorderColor);
            Color text = unstyled ? pal.Text : ForeColor;
            if (!Enabled)
            {
                back = ThemeManager.Blend(outside, back, 0.5);
                border = ThemeManager.Blend(outside, border, 0.5);
                text = ThemeManager.Blend(back, ForeColor, 0.45);
            }

            // 4 px corners at 100 % (the radius follows the height, so the DPI scale)
            float radius = Math.Max(3f, Height / 8f);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            using (GraphicsPath path = SettingsDraw.RoundRect(new RectangleF(0.5f, 0.5f, Width - 1f, Height - 1f), radius))
            using (SolidBrush fill = new SolidBrush(back))
            using (Pen pen = new Pen(border))
            {
                g.FillPath(fill, path);
                g.DrawPath(pen, path);
            }
            g.SmoothingMode = SmoothingMode.Default;

            TextFormatFlags flags = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine | TextFormatFlags.EndEllipsis | TextFormatFlags.NoPadding;
            if (!ShowKeyboardCues)
                flags |= TextFormatFlags.HidePrefix;
            Rectangle textRect = new Rectangle(4, 0, Math.Max(0, Width - 8), Height);
            TextRenderer.DrawText(g, Text, Font, textRect, text, flags);

            if (Focused && ShowFocusCues)
            {
                using (GraphicsPath focus = SettingsDraw.RoundRect(new RectangleF(2.5f, 2.5f, Width - 5f, Height - 5f), Math.Max(2f, radius - 2f)))
                using (Pen pen = new Pen(Color.FromArgb(160, ForeColor)))
                    g.DrawPath(pen, focus);
            }
        }
    }
}
