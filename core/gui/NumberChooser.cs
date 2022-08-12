// ****************************************************************************
// 
// Copyright (C) 2005-2022 Doom9 & al
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
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MeGUI.core.gui
{
    public partial class NumberChooser : Form
    {
        private NumberChooser()
        {
            InitializeComponent();
        }



        public static DialogResult ShowDialog(string message, string title, 
            int decimals, decimal min, decimal max,
            decimal defaultFPSNum, decimal defaultFPSDen, out decimal number)
        {
            NumberChooser n = new NumberChooser();
            n.Text = title;
            n.label1.Text = message;
            n.numericUpDown1.DecimalPlaces = decimals;
            n.numericUpDown1.Minimum = min;
            n.numericUpDown1.Maximum = max;
            n.numericUpDown1.Value = defaultFPSNum;
            n.numericUpDown2.Value = defaultFPSDen;

            DialogResult r = n.ShowDialog();
            number = n.numericUpDown1.Value / n.numericUpDown2.Value;
            return r;
        }

        private void NumberChooser_Shown(object sender, EventArgs e)
        {
            numericUpDown1.Select(0, stringLength);
            numericUpDown1.Focus();

        }

        private int stringLength
        {
            get
            {
                return Math.Round(numericUpDown1.Value, 0).ToString().Length + 
                    (numericUpDown1.DecimalPlaces > 0 ? 1 : 0) + 
                    numericUpDown1.DecimalPlaces;
            }
        }

    }
}