// ****************************************************************************
// 
// Copyright (C) 2005-2025 Doom9 & al
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
using System.Windows.Forms;
using System.Drawing;

namespace MeGUI.packages.tools.hdbdextractor
{
    /// <summary>A custom DataGridView with VerticalScrollBar always shown</summary>
    public class CustomDataGridView : DataGridView
    {
        public CustomDataGridView() : base()
        {
            VerticalScrollBar.Visible = true;
            VerticalScrollBar.VisibleChanged += new EventHandler(VerticalScrollBar_VisibleChanged);
        }

        void VerticalScrollBar_VisibleChanged(object sender, EventArgs e)
        {
            if (!VerticalScrollBar.Visible)
            {
                VerticalScrollBar.Location = new Point(ClientRectangle.Width - VerticalScrollBar.Width, 1);
                VerticalScrollBar.Size = new Size(VerticalScrollBar.Width, ClientRectangle.Height - 1);// - HorizontalScrollBar.Height);
                VerticalScrollBar.Show();
            }

            VerticalScrollBar.Visible = true;
        }
    }
}
