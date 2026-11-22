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
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace MeGUI.core.gui
{
    public enum PlayerGlyph
    {
        None,
        Play,
        Pause,
        PreviousFrame,
        NextFrame,
        Back25,
        Forward25,
        Compare,
        More
    }

    /// <summary>
    /// A flat, themed button whose icon is drawn with GDI+ (no icon font needed, works on every Windows version).
    /// The Play glyph is drawn on a round accent background; other buttons are flat with a hover highlight.
    /// </summary>
    public class PlayerButton : Control
    {
        private PlayerGlyph glyph;
        private bool primary;
        private bool toggled;
        private bool hover, pressed;

        public PlayerButton()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.Selectable | ControlStyles.StandardClick, true);
            TabStop = true;
            Size = new Size(44, 44);
            ThemeManager.ThemeChanged += OnThemeChanged;
        }

        [DefaultValue(PlayerGlyph.None)]
        public PlayerGlyph Glyph
        {
            get { return glyph; }
            set { glyph = value; Invalidate(); }
        }

        /// <summary>Round accent-coloured button (the play / pause button)</summary>
        [DefaultValue(false)]
        public bool Primary
        {
            get { return primary; }
            set { primary = value; Invalidate(); }
        }

        /// <summary>Shows the button as switched on (accent outline), e.g. the Compare button</summary>
        [DefaultValue(false)]
        public bool Toggled
        {
            get { return toggled; }
            set { toggled = value; Invalidate(); }
        }

        public override string Text
        {
            get { return base.Text; }
            set { base.Text = value; Invalidate(); }
        }

        private void OnThemeChanged(object sender, EventArgs e)
        {
            if (IsHandleCreated && !IsDisposed)
                BeginInvoke((MethodInvoker)Invalidate);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                ThemeManager.ThemeChanged -= OnThemeChanged;
            base.Dispose(disposing);
        }

        protected override void OnEnabledChanged(EventArgs e) { base.OnEnabledChanged(e); Invalidate(); }
        protected override void OnMouseEnter(EventArgs e) { base.OnMouseEnter(e); hover = true; Invalidate(); }
        protected override void OnMouseLeave(EventArgs e) { base.OnMouseLeave(e); hover = false; pressed = false; Invalidate(); }
        protected override void OnGotFocus(EventArgs e) { base.OnGotFocus(e); Invalidate(); }
        protected override void OnLostFocus(EventArgs e) { base.OnLostFocus(e); Invalidate(); }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == MouseButtons.Left)
            {
                pressed = true;
                Focus();
                Invalidate();
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            pressed = false;
            Invalidate();
            // the Click event itself is raised by Control (ControlStyles.StandardClick)
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (e.KeyCode == Keys.Space || e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                OnClick(EventArgs.Empty);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            ThemePalette p = ThemeManager.Palette;
            Graphics g = e.Graphics;
            g.Clear(Parent != null ? Parent.BackColor : p.Window);
            g.SmoothingMode = SmoothingMode.AntiAlias;

            Color fg = Enabled ? p.Text : p.Track;
            Rectangle r = new Rectangle(0, 0, Width - 1, Height - 1);

            if (primary)
            {
                int d = Math.Min(Width, Height) - 2;
                Rectangle circle = new Rectangle((Width - d) / 2, (Height - d) / 2, d, d);
                Color back = Enabled ? p.Accent : p.Track;
                if (hover && Enabled)
                    back = ControlPaint.Light(back, 0.2f);
                using (Brush b = new SolidBrush(back))
                    g.FillEllipse(b, circle);
                fg = p.OnAccent;
            }
            else
            {
                Color back = pressed ? p.SurfaceHover : (hover && Enabled ? p.SurfaceHover : Color.Empty);
                if (toggled)
                    back = Color.FromArgb(60, p.Accent);
                if (back != Color.Empty)
                    using (GraphicsPath path = RoundRect(r, 8))
                    using (Brush b = new SolidBrush(back))
                        g.FillPath(b, path);
                if (toggled)
                    using (GraphicsPath path = RoundRect(r, 8))
                    using (Pen pen = new Pen(p.Accent))
                        g.DrawPath(pen, path);
            }

            // icon (left part) and optional text
            int iconSize = Math.Max(10, Math.Min(Height - 2, primary ? Height / 2 : (int)(Height * 0.45f)));
            bool hasText = !string.IsNullOrEmpty(Text);
            SizeF textSize = SizeF.Empty;
            if (hasText)
                textSize = g.MeasureString(Text, Font);
            float gap = (glyph != PlayerGlyph.None && hasText) ? 8 : 0;
            float contentW = (glyph != PlayerGlyph.None ? iconSize : 0) + gap + (hasText ? textSize.Width : 0);
            float x = (Width - contentW) / 2f;
            float cy = Height / 2f;

            if (glyph != PlayerGlyph.None)
            {
                DrawGlyph(g, glyph, new RectangleF(x, cy - iconSize / 2f, iconSize, iconSize), fg);
                x += iconSize + gap;
            }
            if (hasText)
            {
                g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
                using (Brush b = new SolidBrush(fg))
                    g.DrawString(Text, Font, b, x, cy - textSize.Height / 2f);
            }

            if (Focused && ShowFocusCues && !primary)
            {
                using (Pen focus = new Pen(p.Accent) { DashStyle = DashStyle.Dot })
                    g.DrawRectangle(focus, 2, 2, Width - 5, Height - 5);
            }
        }

        private static GraphicsPath RoundRect(Rectangle r, int radius)
        {
            int d = radius * 2;
            GraphicsPath path = new GraphicsPath();
            path.AddArc(r.X, r.Y, d, d, 180, 90);
            path.AddArc(r.Right - d, r.Y, d, d, 270, 90);
            path.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
            path.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }

        private static void DrawGlyph(Graphics g, PlayerGlyph kind, RectangleF r, Color color)
        {
            float w = r.Width, h = r.Height, x = r.X, y = r.Y;
            using (Brush brush = new SolidBrush(color))
            using (Pen pen = new Pen(color, Math.Max(1.5f, w / 11f)))
            {
                pen.StartCap = LineCap.Round;
                pen.EndCap = LineCap.Round;
                pen.LineJoin = LineJoin.Round;
                switch (kind)
                {
                    case PlayerGlyph.Play:
                        g.FillPolygon(brush, new[] { new PointF(x + w * 0.2f, y), new PointF(x + w * 0.2f, y + h), new PointF(x + w, y + h / 2) });
                        break;
                    case PlayerGlyph.Pause:
                        g.FillRectangle(brush, x + w * 0.12f, y, w * 0.28f, h);
                        g.FillRectangle(brush, x + w * 0.60f, y, w * 0.28f, h);
                        break;
                    case PlayerGlyph.PreviousFrame:
                        g.DrawLine(pen, x + w * 0.1f, y, x + w * 0.1f, y + h);
                        g.FillPolygon(brush, new[] { new PointF(x + w, y), new PointF(x + w, y + h), new PointF(x + w * 0.3f, y + h / 2) });
                        break;
                    case PlayerGlyph.NextFrame:
                        g.DrawLine(pen, x + w * 0.9f, y, x + w * 0.9f, y + h);
                        g.FillPolygon(brush, new[] { new PointF(x, y), new PointF(x, y + h), new PointF(x + w * 0.7f, y + h / 2) });
                        break;
                    case PlayerGlyph.Back25:
                        g.FillPolygon(brush, new[] { new PointF(x + w * 0.55f, y + h * 0.1f), new PointF(x + w * 0.55f, y + h * 0.9f), new PointF(x, y + h / 2) });
                        g.FillPolygon(brush, new[] { new PointF(x + w, y + h * 0.1f), new PointF(x + w, y + h * 0.9f), new PointF(x + w * 0.45f, y + h / 2) });
                        break;
                    case PlayerGlyph.Forward25:
                        g.FillPolygon(brush, new[] { new PointF(x, y + h * 0.1f), new PointF(x, y + h * 0.9f), new PointF(x + w * 0.55f, y + h / 2) });
                        g.FillPolygon(brush, new[] { new PointF(x + w * 0.45f, y + h * 0.1f), new PointF(x + w * 0.45f, y + h * 0.9f), new PointF(x + w, y + h / 2) });
                        break;
                    case PlayerGlyph.Compare:
                        g.DrawRectangle(pen, x, y + h * 0.1f, w, h * 0.8f);
                        g.DrawLine(pen, x + w / 2, y + h * 0.1f, x + w / 2, y + h * 0.9f);
                        break;
                    case PlayerGlyph.More:
                        float dot = Math.Max(2.5f, w / 6f);
                        for (int i = 0; i < 3; i++)
                            g.FillEllipse(brush, x + w * (0.15f + 0.35f * i) - dot / 2f, y + h / 2f - dot / 2f, dot, dot);
                        break;
                }
            }
        }
    }
}
