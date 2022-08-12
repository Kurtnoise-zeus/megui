// ****************************************************************************
// 
// Copyright (C) 2005-2018 Doom9 & al
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
using System.Windows.Forms;

namespace MeGUI.core.gui
{
    public enum ProgressBarDisplayText
    {
        Percentage,
        CustomText
    }

    class CustomProgressBar : ProgressBar
    {
        [System.Runtime.InteropServices.DllImportAttribute("uxtheme.dll")]
        private static extern int SetWindowTheme(IntPtr hWnd, string appname, string idlist);

        //Property to set to decide whether to print a % or Text
        public ProgressBarDisplayText DisplayStyle { get; set; }

        //Property to hold the custom text
        public String CustomText { get; set; }

        public CustomProgressBar()
        {
            // Modify the ControlStyles flags
            //http://msdn.microsoft.com/en-us/library/system.windows.forms.controlstyles.aspx
            if (Application.RenderWithVisualStyles)
                SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (!Application.RenderWithVisualStyles)
            {
                base.OnPaint(e);
                return;
            }

            Rectangle rect = ClientRectangle;
            Graphics g = e.Graphics;

            ProgressBarRenderer.DrawHorizontalBar(g, rect);
            //rect.Inflate(-3, -3);
            if (Value > 0)
            {
                // As we doing this ourselves we need to draw the chunks on the progress bar
                Rectangle clip = new Rectangle(rect.X, rect.Y, (int)Math.Round(((float)Value / Maximum) * rect.Width), rect.Height);
                ProgressBarRenderer.DrawHorizontalChunks(g, clip);
            }

            // Set the Display text (Either a % amount or our custom text
            string text = DisplayStyle == ProgressBarDisplayText.Percentage ? Value.ToString() + '%' : CustomText;

            using (Font f = new Font("Tahoma", 8.25f))
            {
                SizeF len = g.MeasureString(text, f);
                // Calculate the location of the text (the middle of progress bar)
                Point location = new Point(Convert.ToInt32((rect.Width / 2) - (len.Width / 2)), Convert.ToInt32((rect.Height / 2) - (len.Height / 2)));
                // Draw the custom text
                g.DrawString(text, f, Brushes.Black, location);
            }
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            if (Application.RenderWithVisualStyles && OSInfo.IsWindowsXPOrNewer)
            {
                // >= Windows XP
                SetWindowTheme(this.Handle, "", "");
            }
            
            base.OnHandleCreated(e);
        }
    }
}