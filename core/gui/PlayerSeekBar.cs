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
using System.Windows.Forms;

namespace MeGUI.core.gui
{
    /// <summary>
    /// A themed, full-width seek bar. It replaces the TrackBar of the old video player and
    /// additionally highlights the current zone and marks the intro end / credits start.
    /// Scroll is raised only for changes made by the user, exactly like TrackBar.Scroll.
    /// </summary>
    public class PlayerSeekBar : Control
    {
        private int minimum;
        private int maximum = 100;
        private int value;
        private bool dragging;
        private bool hot;
        private int zoneStart = -1, zoneEnd = -1, introEnd = -1, creditsStart = -1;

        /// <summary>Raised when the user changes the position (mouse or keyboard).</summary>
        public event EventHandler Scroll;

        public PlayerSeekBar()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.Selectable | ControlStyles.StandardClick, true);
            TabStop = true;
            Height = 28;
            ThemeManager.ThemeChanged += OnThemeChanged;
        }

        [DefaultValue(0)]
        public int Minimum
        {
            get { return minimum; }
            set { minimum = value; if (maximum < minimum) maximum = minimum; Value = this.value; Invalidate(); }
        }

        [DefaultValue(100)]
        public int Maximum
        {
            get { return maximum; }
            set { maximum = value; if (minimum > maximum) minimum = maximum; Value = this.value; Invalidate(); }
        }

        [DefaultValue(0)]
        public int Value
        {
            get { return value; }
            set
            {
                int v = Math.Max(minimum, Math.Min(maximum, value));
                if (v == this.value)
                    return;
                this.value = v;
                Invalidate();
            }
        }

        /// <summary>First frame of the zone being defined, or -1</summary>
        public int ZoneStart { get { return zoneStart; } set { zoneStart = value; Invalidate(); } }
        /// <summary>Last frame of the zone being defined, or -1</summary>
        public int ZoneEnd { get { return zoneEnd; } set { zoneEnd = value; Invalidate(); } }
        /// <summary>Frame where the intro ends, or -1</summary>
        public int IntroEnd { get { return introEnd; } set { introEnd = value; Invalidate(); } }
        /// <summary>Frame where the credits start, or -1</summary>
        public int CreditsStart { get { return creditsStart; } set { creditsStart = value; Invalidate(); } }

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

        #region geometry
        private int ThumbDiameter { get { return Math.Max(12, (int)(Height * 0.55f)); } }
        private int Margin2 { get { return ThumbDiameter / 2 + 1; } }

        private float FrameToX(int frame)
        {
            int range = maximum - minimum;
            int usable = Math.Max(1, Width - 2 * Margin2);
            if (range <= 0)
                return Margin2;
            return Margin2 + usable * (float)(frame - minimum) / range;
        }

        private int XToFrame(int x)
        {
            int range = maximum - minimum;
            int usable = Math.Max(1, Width - 2 * Margin2);
            double ratio = (x - Margin2) / (double)usable;
            if (ratio < 0) ratio = 0;
            if (ratio > 1) ratio = 1;
            return minimum + (int)Math.Round(ratio * range);
        }
        #endregion

        #region painting
        protected override void OnPaint(PaintEventArgs e)
        {
            ThemePalette p = ThemeManager.Palette;
            Graphics g = e.Graphics;
            g.Clear(Parent != null && Parent.BackColor != Color.Transparent ? Parent.BackColor : p.Window);
            g.SmoothingMode = SmoothingMode.AntiAlias;

            int trackH = Math.Max(4, Height / 7);
            float cy = Height / 2f;
            RectangleF track = new RectangleF(Margin2, cy - trackH / 2f, Math.Max(1, Width - 2 * Margin2), trackH);

            using (Brush b = new SolidBrush(p.Track))
                FillRounded(g, b, track);

            // zone highlight
            if (zoneStart > -1 || zoneEnd > -1)
            {
                int zs = zoneStart > -1 ? zoneStart : minimum;
                int ze = zoneEnd > -1 ? zoneEnd : maximum;
                if (ze > zs)
                {
                    float x1 = FrameToX(zs), x2 = FrameToX(ze);
                    using (Brush b = new SolidBrush(Color.FromArgb(110, p.Accent)))
                        g.FillRectangle(b, x1, cy - trackH * 1.5f, Math.Max(2f, x2 - x1), trackH * 3f);
                }
            }

            // played part
            float xv = FrameToX(value);
            using (Brush b = new SolidBrush(p.Accent))
                FillRounded(g, b, new RectangleF(track.X, track.Y, Math.Max(1f, xv - track.X), track.Height));

            // intro end / credits start markers
            DrawMarker(g, p, introEnd, cy, trackH);
            DrawMarker(g, p, creditsStart, cy, trackH);

            // thumb
            int d = ThumbDiameter + ((hot || dragging) ? 2 : 0);
            RectangleF thumb = new RectangleF(xv - d / 2f, cy - d / 2f, d, d);
            using (Brush ring = new SolidBrush(p.Window))
                g.FillEllipse(ring, thumb.X - 2, thumb.Y - 2, thumb.Width + 4, thumb.Height + 4);
            using (Brush b = new SolidBrush(p.Accent))
                g.FillEllipse(b, thumb);

            if (Focused && ShowFocusCues)
            {
                using (Pen focus = new Pen(p.Border) { DashStyle = DashStyle.Dot })
                    g.DrawRectangle(focus, 0, 0, Width - 1, Height - 1);
            }
        }

        private void DrawMarker(Graphics g, ThemePalette p, int frame, float cy, int trackH)
        {
            if (frame < minimum || frame > maximum)
                return;
            float x = FrameToX(frame);
            using (Pen pen = new Pen(p.Text, 2f))
                g.DrawLine(pen, x, cy - trackH * 1.8f, x, cy + trackH * 1.8f);
        }

        private static void FillRounded(Graphics g, Brush brush, RectangleF r)
        {
            if (r.Width < r.Height)
            {
                g.FillEllipse(brush, r);
                return;
            }
            using (GraphicsPath path = new GraphicsPath())
            {
                float d = r.Height;
                path.AddArc(r.X, r.Y, d, d, 90, 180);
                path.AddArc(r.Right - d, r.Y, d, d, 270, 180);
                path.CloseFigure();
                g.FillPath(brush, path);
            }
        }
        #endregion

        #region user input
        private void SetFromMouse(int x)
        {
            int v = XToFrame(x);
            if (v == value)
                return;
            Value = v;
            Scroll?.Invoke(this, EventArgs.Empty);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button != MouseButtons.Left)
                return;
            Focus();
            dragging = true;
            Capture = true;
            SetFromMouse(e.X);
            Invalidate();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (dragging)
                SetFromMouse(e.X);
            else
            {
                bool nowHot = Math.Abs(e.X - FrameToX(value)) <= ThumbDiameter;
                if (nowHot != hot)
                {
                    hot = nowHot;
                    Invalidate();
                }
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (dragging)
            {
                dragging = false;
                Capture = false;
                Invalidate();
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            if (hot)
            {
                hot = false;
                Invalidate();
            }
        }

        protected override bool IsInputKey(Keys keyData)
        {
            switch (keyData & Keys.KeyCode)
            {
                case Keys.Left:
                case Keys.Right:
                case Keys.Home:
                case Keys.End:
                case Keys.PageUp:
                case Keys.PageDown:
                    return true;
            }
            return base.IsInputKey(keyData);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            int target = value;
            switch (e.KeyCode)
            {
                case Keys.Left: target = value - 1; break;
                case Keys.Right: target = value + 1; break;
                case Keys.PageUp: target = value - 25; break;
                case Keys.PageDown: target = value + 25; break;
                case Keys.Home: target = minimum; break;
                case Keys.End: target = maximum; break;
                default: return;
            }
            e.Handled = true;
            int before = value;
            Value = target;
            if (value != before)
                Scroll?.Invoke(this, EventArgs.Empty);
        }

        protected override void OnGotFocus(EventArgs e) { base.OnGotFocus(e); Invalidate(); }
        protected override void OnLostFocus(EventArgs e) { base.OnLostFocus(e); Invalidate(); }
        #endregion
    }
}
