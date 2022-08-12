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

namespace MeGUI
{
	public delegate void WindowClosedCallback(bool hideOnly); // delegate for WindowClosed event
	public delegate void AbortCallback(); // delegate for Abort event
    public delegate void SuspendCallback(); // delegate for Suspend event
    public delegate void UpdateStatusCallback(StatusUpdate su); // delegate for UpdateStatus event
	public delegate void PriorityChangedCallback(WorkerPriorityType priority); // delegate for PriorityChanged event

    /// <summary>
    /// ProgressWindow is a window that is being shown during encoding and shows the current encoding status
    /// it is being fed by UpdateStatus events fired from the main GUI class (Form1)
    /// </summary>
    public partial class ProgressWindow : Form
    {
        private bool bIsSuspended;

        #region start / stop & show / hide
        /// <summary>
		/// default constructor, initializes the GUI components
		/// </summary>
		public ProgressWindow()
        {
            InitializeComponent();
            isUserClosing = true;
            bIsSuspended = false;

            if (OSInfo.IsWindows7OrNewer)
                taskbarProgress = (ITaskbarList3)new ProgressTaskbar();
		}

		/// <summary>
		/// handles the onclosing event
		/// ensures that if the user closed the window, it will only be hidden
		/// whereas if the system closed it, it is allowed to close
		/// </summary>
		/// <param name="e"></param>
        protected override void OnClosing(CancelEventArgs e)
        {
            this.Hide();

            // possible to abort job
            abortButton.Enabled = false;

            // Current position
            positionInClip.Text = "--- / ---";

            // Current frame
            currentVideoFrame.Text = "--- / ---";

            // Data
            videoData.Text = "--- / ---";

            // Processing speed
            fps.Text = "---";

            TimeSpan oTimeSpan = new TimeSpan(0);

            // Time elapsed 
            // Now we use TotalHours to avoid 24h+ resets...
            timeElapsed.Text = string.Format("{0:00}:{1:00}:{2:00}", (int)oTimeSpan.Hours, oTimeSpan.Minutes, oTimeSpan.Seconds);

            // Estimated time
            // go back to the old function ;-)
            totalTime.Text = getTimeString(oTimeSpan, 0M);

            this.Text = "Status: " + 0M.ToString("0.00") + " %";
            statusLabel.Text = string.Empty;

            jobNameLabel.Text = string.Empty;

            progress.Value = 0;

            btnSuspend.Text = "Suspend";
            bIsSuspended = false;

            if (this.IsUserAbort)
                e.Cancel = true;
            else
                base.OnClosing(e);
        }

        public void SetVisible(bool bShow)
        {
            this.Visible = bShow;
        }

        #endregion

        #region statusupdate processing
        /// <summary>
        /// catches the StatusUpdate event fired from Form1 and updates the GUI accordingly
        /// </summary>
        /// <param name="su"></param>
        public void UpdateStatus(StatusUpdate su)
        {
            try
            {
                // possible to abort job
                abortButton.Enabled = (su.JobStatus == JobStatus.PROCESSING || su.JobStatus == JobStatus.PAUSED);

                // possible to suspend/resume job
                btnSuspend.Enabled = (su.JobStatus == JobStatus.PROCESSING || su.JobStatus == JobStatus.PAUSED);
                if (su.JobStatus == JobStatus.PROCESSING)
                {
                    bIsSuspended = false;
                    btnSuspend.Text = "Suspend";
                }
                else if (su.JobStatus == JobStatus.PAUSED)
                {
                    bIsSuspended = true;
                    btnSuspend.Text = "Resume";
                }

                // Current position
                positionInClip.Text = (Util.ToString(su.ClipPosition) ?? "---") +
                    " / " + (Util.ToString(su.ClipLength) ?? "---");

                // Current frame
                currentVideoFrame.Text = (Util.ToString(su.NbFramesDone, true) ?? "---") +
                    " / " + (Util.ToString(su.NbFramesTotal, true) ?? "---");

                // Data
                videoData.Text = (su.CurrentFileSize.HasValue ? su.CurrentFileSize.Value.ToString() : "---") +
                    " / " + (su.ProjectedFileSize.HasValue ? su.ProjectedFileSize.Value.ToString() : "---");

                // Processing speed
                fps.Text = su.ProcessingSpeed ?? "---";

                // Time elapsed 
                // Now we use TotalHours to avoid 24h+ resets...
                if (su.TimeElapsed.TotalHours > 24)
                    timeElapsed.Text = string.Format("{0:00}:{1:00}:{2:00}:{3:00}", (int)su.TimeElapsed.TotalDays, su.TimeElapsed.Hours, su.TimeElapsed.Minutes, su.TimeElapsed.Seconds);
                else
                    timeElapsed.Text = string.Format("{0:00}:{1:00}:{2:00}", su.TimeElapsed.Hours, su.TimeElapsed.Minutes, su.TimeElapsed.Seconds);

                // Estimated time
                // go back to the old function ;-)
                totalTime.Text = getTimeString(su.TimeElapsed, su.PercentageDoneExact ?? 0M);

                this.Text = "Status: " + (su.PercentageDoneExact ?? 0M).ToString("0.00") + " %";
                statusLabel.Text = su.Status ?? "";

                jobNameLabel.Text = "[" + su.JobName + "]";

                progress.Value = su.PercentageDone;

                if (OSInfo.IsWindows7OrNewer)
                    taskbarProgress.SetProgressValue(this.Handle, Convert.ToUInt64(su.PercentageDone), 100);
            }
            catch (Exception) { }
        }
        #endregion

        #region helper methods

        /// <summary>
        /// calculates the remaining encoding time from the elapsed timespan and the percentage the job is done
        /// </summary>
        /// <param name="span">timespan elapsed since the start of the job</param>
        /// <param name="percentageDone">percentage the job is currently done</param>
        /// <returns>presentable string in hh:mm:ss format</returns>
        private string getTimeString(TimeSpan span, decimal percentageDone)
        {
            if (percentageDone == 0)
                return "---";
            else
            {
                long ratio = (long)((decimal)span.Ticks / percentageDone * 100M);
                TimeSpan t = new TimeSpan(ratio - span.Ticks);
                string retval = "";
                if (t.TotalHours > 24)
                {
                    retval += string.Format("{0:00}:{1:00}:{2:00}:{3:00}", (int)t.TotalDays, t.Hours, t.Minutes, t.Seconds);
                }
                else
                {
                    retval += string.Format("{0:00}:{1:00}:{2:00}", (int)t.Hours, t.Minutes, t.Seconds);
                }
                return retval;
            }
        }

        private bool isSettingPriority = false;
        /// <summary>
        /// sets the priority
        /// </summary>
        /// <param name="priority"></param>
        public void setPriority(WorkerPriorityType priority)
        {
            Util.ThreadSafeRun(this.priority, delegate {
                isSettingPriority = true;
                this.priority.SelectedIndex = (int)priority;
                isSettingPriority = false;
            });
        }
        #endregion

        #region events
        /// <summary>
		///  handles the abort button
		///  fires an Abort() event to the main GUI, which looks up the encoder and makes it stop
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void abortButton_Click(object sender, System.EventArgs e)
        {
            Abort();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSuspend_Click(object sender, EventArgs e)
        {
            Suspend();
            bIsSuspended = !bIsSuspended;
            if (bIsSuspended)
                btnSuspend.Text = "Resume";
            else
                btnSuspend.Text = "Suspend";
        }

        /// <summary>Handles changes in the priority dropdwon</summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void priority_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (PriorityChanged == null || isSettingPriority)
                return;
            PriorityChanged((WorkerPriorityType)priority.SelectedIndex);
        }

        private void ProgressWindow_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
                return;

            // possible to abort job
            abortButton.Enabled = false;

            // possible to suspend/resume job
            btnSuspend.Enabled = false;
            bIsSuspended = false;
            btnSuspend.Text = "Suspend";

            // Current position
            positionInClip.Text = "--- / ---";

            // Current frame
            currentVideoFrame.Text = "--- / ---";

            // Data
            videoData.Text = "--- / ---";

            // Processing speed
            fps.Text = "---";

            TimeSpan oTimeSpan = new TimeSpan(0);

            // Time elapsed 
            timeElapsed.Text = string.Format("{0:00}:{1:00}:{2:00}", (int)oTimeSpan.Hours, oTimeSpan.Minutes, oTimeSpan.Seconds);

            // Estimated time
            totalTime.Text = getTimeString(oTimeSpan, 0M);

            this.Text = "Status: " + 0M.ToString("0.00") + " %";
            statusLabel.Text = string.Empty;

            jobNameLabel.Text = string.Empty;

            progress.Value = 0;
            if (OSInfo.IsWindows7OrNewer)
                taskbarProgress.SetProgressValue(this.Handle, 0, 100);
        }
        #endregion

        #region properties
        /// <summary>
        /// gets / sets whether the user closed this window or if the system is closing it
        /// </summary>
        private bool isUserClosing;
        public bool IsUserAbort
        {
            get { return isUserClosing; }
            set { isUserClosing = value; }
        }
        #endregion
    }
}