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

namespace MeGUI.core.gui
{
    public partial class CountdownWindow : Form
    {
        private int countdown = 15;
        private int remain = 15;

        public CountdownWindow()
        {
            InitializeComponent();
        }

        public CountdownWindow(int countdown)
        {
            this.countdown = countdown;
            InitializeComponent();
        }

        private void CountdownWindow_Load(object sender, EventArgs e)
        {
            //bool b = this.TopMost;
            this.Activate();
            this.Focus();
            this.BringToFront();
            remain = countdown;
            SetProgressBar(0, 1, 0);
            SetButtonText(remain.ToString());
            timer.Enabled = true;
        }

        private delegate void UpdateProgressBar(int minValue, int maxValue, int currentValue);
        private void SetProgressBar(int minValue, int maxValue, int currentValue)
        {
            if (this.progressBar.InvokeRequired)
            {
                UpdateProgressBar d = new UpdateProgressBar(SetProgressBar);
                this.Invoke(d, minValue, maxValue, currentValue);
            }
            else
            {
                this.progressBar.Minimum = (int)minValue;
                this.progressBar.Maximum = (int)maxValue;
                this.progressBar.Value = (int)currentValue;
            }
        }

        private delegate void UpdateCancelButton(String text);
        private void SetButtonText(String text)
        {
            if (this.cancelButton.InvokeRequired)
            {
                UpdateCancelButton d = new UpdateCancelButton(SetButtonText);
                this.Invoke(d, text);
            }
            else
            {
                this.cancelButton.Text = string.Format("Cancel ({0})", text);
            }
        }

        /// <summary>
        /// TimerTick. Refresh the ProgressBar and the Button.
        /// </summary>
        void TimerTick(object sender, System.EventArgs e)
        {
            remain--;
            if (remain > 0)
            {
                SetProgressBar(0, countdown, countdown - remain);
                SetButtonText((remain).ToString());
            }
            else
            {
                SetProgressBar(0, 1, 1);
                SetButtonText("0");
                base.DialogResult = DialogResult.OK;
            }
        }
    }
}