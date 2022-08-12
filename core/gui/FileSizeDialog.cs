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
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using MeGUI.core.util;

namespace MeGUI.core.gui
{
    public partial class FileSizeDialog : Form
    {
        public FileSizeDialog()
        {
            InitializeComponent();
            CurrentUnit = Unit.MB;
        }

        private FileSize maxVal = new FileSize(ulong.MaxValue);

        /// <summary>
        /// Gets / sets maximum allowed filesize
        /// </summary>
        public FileSize Maximum
        {
            get { return maxVal; }
            set { maxVal = value; }
        }

        private FileSize minVal = FileSize.Empty;

        /// <summary>
        /// Gets / sets minimum allowed displayable filesize
        /// </summary>
        public FileSize Minimum
        {
            get { return minVal; }
            set { minVal = value; }
        }

        /// <summary>
        /// Gets / sets the value displayed by the component
        /// </summary>
        public FileSize Value
        {
            get
            {
                return readValue(CurrentUnit);
            }
            set
            {
                Unit u = value.BestUnit;
                CurrentUnit = u;
                adjustDP();
                number.Value = value.InUnitsExact(u);
            }
        }

        /// <summary>
        /// Reads the value off the GUI, using the given Unit
        /// </summary>
        /// <param name="u"></param>
        /// <returns></returns>
        private FileSize readValue(Unit u)
        {
            return new FileSize(u, number.Value);
        }

        /// <summary>
        /// Reads / writes the current unit from the GUI
        /// </summary>
        private Unit CurrentUnit
        {
            get
            {
                if (units.SelectedIndex < 0) return Unit.B;
                return (Unit)units.SelectedIndex;
            }
            set { units.SelectedIndex = (ushort)value; }
        }

        /// <summary>
        /// Adjusts the number of decimal places depending on the 
        /// </summary>
        private void adjustDP()
        {
            if (CurrentUnit == Unit.B)
            {
                number.DecimalPlaces = 0;
                number.Increment = 1;
            }
            else
            {
                number.DecimalPlaces = 1;
                number.Increment = 0.1M;
            }
            number.Maximum = maxVal.InUnitsExact(CurrentUnit);
            number.Minimum = minVal.InUnitsExact(CurrentUnit);
        }

        private void FileSizeDialog_Shown(object sender, EventArgs e)
        {
            number.Select(0, number.Value.ToString().Length);
            number.Focus();
        }

        private void number_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
                okButton.PerformClick();
        }
    }
}