// ****************************************************************************
// 
// Copyright (C) 2005-2024 Doom9 & al
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
        private bool isUserClosing;
        private bool bIsSuspended;
        private Timer _GUIUpdateTimer = new Timer();
        private StatusUpdate _su;

        #region start / stop & show / hide
        /// <summary>
		/// default constructor, initializes the GUI components
		/// </summary>
		public ProgressWindow()
        {
            InitializeComponent();
            isUserClosing = true;
            bIsSuspended = false;

            _GUIUpdateTimer.Tick += new EventHandler(TimerEventProcessor);
            _GUIUpdateTimer.Interval = 1000;
            
            if (OSInfo.IsWindows7OrNewer)
                taskbarProgress = (ITaskbarList3)new ProgressTaskbar();
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
                _GUIUpdateTimer.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// handles the onclosing event
        /// ensures that if the user closed the window, it will only be hidden
        /// whereas if the system closed it, it is allowed to close
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClosing(CancelEventArgs e)
        {
            if (this.IsUserAbort)
            {
                if (e != null)
                    e.Cancel = true;
                this.Hide();
            }
            else
            {
                base.OnClosing(e);
            }
        }

        public void SetVisible(bool bShow)
        {
            this.Visible = bShow;
        }

        public void SetStatusUpdate(StatusUpdate su)
        {
            _su = su;
        }

        #endregion

        #region statusupdate processing

        private void TimerEventProcessor(Object myObject, EventArgs myEventArgs)
        {
            _su.FillValues();
            UpdateStatus();
        }

        /// <summary>
        /// catches the StatusUpdate event fired from Form1 and updates the GUI accordingly
        /// </summary>
        /// <param name="su"></param>
        public void UpdateStatus()
        {
            try
            {
                // possible to abort job
                abortButton.Enabled = (_su.JobStatus == JobStatus.PROCESSING || _su.JobStatus == JobStatus.PAUSED);

                // possible to suspend/resume job
                btnSuspend.Enabled = (_su.JobStatus == JobStatus.PROCESSING || _su.JobStatus == JobStatus.PAUSED);
                if (_su.JobStatus == JobStatus.PROCESSING)
                {
                    bIsSuspended = false;
                    btnSuspend.Text = "Suspend";
                }
                else if (_su.JobStatus == JobStatus.PAUSED)
                {
                    bIsSuspended = true;
                    btnSuspend.Text = "Resume";
                }

                // Current position
                positionInClip.Text = (Util.ToString(_su.ClipPosition) ?? "---");
                positionInClipTotal.Text = (Util.ToString(_su.ClipLength) ?? "---");

                // Current frame
                currentVideoFrame.Text = (Util.ToString(_su.NbFramesDone, true) ?? "---");
                totalVideoFrame.Text = (Util.ToString(_su.NbFramesTotal, true) ?? "---");

                // Data
                currentFileSize.Text = (_su.FileSizeCurrent.HasValue ? _su.FileSizeCurrent.Value.ToString() : "---");
                if (!_su.FileSizeTotal.HasValue)
                    estimatedFileSize.Text = "---";
                else
                    estimatedFileSize.Text = _su.FileSizeTotal.Value.ToString();

                // Processing speed
                overallFPS.Text = _su.OverallSpeed ?? "---";
                currentFPS.Text = _su.CurrentSpeed ?? "---";

                // Time elapsed 
                // Now we use TotalHours to avoid 24h+ resets...
                if (_su.TimeElapsed.TotalHours > 24)
                    timeElapsed.Text = string.Format(new System.Globalization.CultureInfo("en-US"), "{0:00}:{1:00}:{2:00}:{3:00}", (int)_su.TimeElapsed.TotalDays, _su.TimeElapsed.Hours, _su.TimeElapsed.Minutes, _su.TimeElapsed.Seconds);
                else
                    timeElapsed.Text = string.Format(new System.Globalization.CultureInfo("en-US"), "{0:00}:{1:00}:{2:00}", _su.TimeElapsed.Hours, _su.TimeElapsed.Minutes, _su.TimeElapsed.Seconds);

                // Estimated time
                if (_su.PercentageEstimated > 0)
                {
                    TimeSpan t = _su.TimeRemaining;
                    if (t.TotalSeconds <= 0)
                        totalTime.Text = "---";
                    else if (t.TotalHours > 24)
                        totalTime.Text = string.Format(new System.Globalization.CultureInfo("en-US"), "{0:00}:{1:00}:{2:00}:{3:00}", (int)t.TotalDays, t.Hours, t.Minutes, t.Seconds);
                    else
                        totalTime.Text = string.Format(new System.Globalization.CultureInfo("en-US"), "{0:00}:{1:00}:{2:00}", t.Hours, t.Minutes, t.Seconds);
                }
                else
                    totalTime.Text = "---";

                this.Text = "Status: " + (_su.PercentageEstimated).ToString("0.00", new System.Globalization.CultureInfo("en-US")) + " %";
                statusLabel.Text = _su.Status ?? "";

                jobNameLabel.Text = "[" + _su.JobName + "]";

                progress.Value = (int)_su.PercentageEstimated;

                if (OSInfo.IsWindows7OrNewer)
                    taskbarProgress.SetProgressValue(this.Handle, Convert.ToUInt64((int)_su.PercentageEstimated), 100);
            }   
            catch (Exception) { }
     }
#endregion
#region helper methods

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
            {
                _GUIUpdateTimer.Start();
                return;
            }
            
            // stop the refresh timer
            _GUIUpdateTimer.Stop();

            // possible to abort job
            abortButton.Enabled = false;

            // possible to suspend/resume job
            btnSuspend.Enabled = false;
            bIsSuspended = false;
            btnSuspend.Text = "Suspend";

            // position
            positionInClip.Text = positionInClipTotal.Text = "---";
            
            // frame
            currentVideoFrame.Text = totalVideoFrame.Text = "---";

            // Data
            currentFileSize.Text = estimatedFileSize.Text = "---";

            // Processing speed
            overallFPS.Text = "---";
            currentFPS.Text = "---";

            // Time elapsed 
            timeElapsed.Text = string.Format("{0:00}:{1:00}:{2:00}", 0, 0, 0);

            // Estimated time
            totalTime.Text = "---";
            
            // Status
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
        public bool IsUserAbort
        {
            get { return isUserClosing; }
            set { isUserClosing = value; }
        }
        #endregion
    }
}