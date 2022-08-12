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
using System.ComponentModel;
using System.Windows.Forms;

using MeGUI.core.util;

namespace MeGUI.core.gui
{
    public partial class IndividualWorkerSummary : UserControl
    {
        private JobWorker w;

        public IndividualWorkerSummary()
        {
            InitializeComponent();
        }

        public JobWorker Worker
        {
            set { w = value; }
        }

        public void RefreshInfo()
        {
            workerNameAndJob.Text = string.Format("{0}: {1}", w.Name, w.StatusString);
            progressBar1.Value = (int)w.Progress;
        }

        private void StartEncodingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            w.StartEncoding(true);
            RefreshInfo();
        }

        private void AbortToolStripMenuItem_Click(object sender, EventArgs e)
        {
            w.UserRequestedAbort();
        }

        private void StopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (stopToolStripMenuItem.Checked)
                w.SetRunning();
            else
                w.SetStopping();
        }

        private void ShowProgressWindowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (showProgressWindowToolStripMenuItem.Checked)
                w.HideProcessWindow();
            else
                w.ShowProcessWindow();
        }

        private void PauseResumeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            w.Pw_Suspend();
        }

        private void ContextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            startEncodingToolStripMenuItem.Enabled = !w.IsRunning;

            pauseResumeToolStripMenuItem.Enabled = w.IsRunning;
            pauseResumeToolStripMenuItem.Checked = w.JobStatus == JobStatus.PAUSED;

            abortToolStripMenuItem.Enabled = w.IsRunning;
            
            stopToolStripMenuItem.Enabled = w.IsRunning;
            stopToolStripMenuItem.Checked = w.Status == JobWorkerStatus.Stopping;

            showProgressWindowToolStripMenuItem.Enabled = w.IsProgressWindowAvailable;
            showProgressWindowToolStripMenuItem.Checked = w.IsProgressWindowVisible;
        }
    }
}