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
using System.Windows.Forms;
using MeGUI.core.util;

namespace MeGUI.core.gui
{
    public partial class AspectRatioChooser : Form
    {
        private bool bDisableEvents;

        private AspectRatioChooser()
        {
            InitializeComponent();
        }

        public static DialogResult ShowDialog(Dar defaultDar, out Dar newDar)
        {
            AspectRatioChooser n = new AspectRatioChooser();
            n.SetValues(defaultDar);
            if (defaultDar.Y < 1)
                n.radioButton2.Checked = true;
            else
                n.radioButton1.Checked = true;
            
            DialogResult r = n.ShowDialog();
            if (n.radioButton1.Checked)
                newDar = new Dar(n.numericUpDown1.Value);
            else
                newDar = new Dar((ulong)n.numericUpDown2.Value, (ulong)n.numericUpDown3.Value);
            return r;
        }

        public void SetValues(Dar ar)
        {
            bDisableEvents = true;
            if (ar.AR >= numericUpDown1.Minimum && ar.AR <= numericUpDown1.Maximum)
                numericUpDown1.Value = ar.AR;
            if (ar.X >= numericUpDown2.Minimum && ar.X <= numericUpDown2.Maximum)
                numericUpDown2.Value = ar.X;
            if (ar.Y >= numericUpDown3.Minimum && ar.Y <= numericUpDown3.Maximum)
                numericUpDown3.Value = ar.Y;
            bDisableEvents = false;
        }

        private void radioButton_CheckedChanged(object sender, EventArgs e)
        {
            numericUpDown1.Enabled = radioButton1.Checked;
            numericUpDown2.Enabled = numericUpDown3.Enabled = !radioButton1.Checked;  
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            if (bDisableEvents)
                return;
            bDisableEvents = true;
            Dar ar = new Dar((ulong)numericUpDown2.Value, (ulong)numericUpDown3.Value);
            if (ar.AR >= numericUpDown1.Minimum && ar.AR <= numericUpDown1.Maximum)
                numericUpDown1.Value = ar.AR;
            bDisableEvents = false;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (bDisableEvents)
                return;
            bDisableEvents = true;
            Dar ar = new Dar(numericUpDown1.Value);
            if (ar.X >= numericUpDown2.Minimum && ar.X <= numericUpDown2.Maximum)
                numericUpDown2.Value = ar.X;
            if (ar.Y >= numericUpDown3.Minimum && ar.Y <= numericUpDown3.Maximum)
                numericUpDown3.Value = ar.Y;
            bDisableEvents = false;
        }
    }
}