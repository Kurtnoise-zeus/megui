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
using System.Diagnostics;

using MeGUI.core.util;

namespace MeGUI
{
	/// <summary>
	/// Class that is used to send an encoding update from the encoder to the GUI
	/// it contains all the elements that will be updated in the GUI at some point
	/// </summary>
	public class StatusUpdate
	{
		private bool hasError, isComplete, wasAborted;
        private string jobName, status;
        private TimeSpan? clipPosition, clipLength, estimatedTime;
        private ulong? nbFramesDone, nbFramesTotal;
        private FileSize? filesize, projectedFileSize;
        private string processingspeed;
		private decimal percentage;
        private JobStatus jobStatus;
        private static readonly TimeSpan FiveSeconds = new TimeSpan(0, 0, 5);
        private const int UpdatesPerEstimate = 10;
        private TimeSpan[] previousUpdates = new TimeSpan[UpdatesPerEstimate];
        private decimal[] previousUpdatesProgress = new decimal[UpdatesPerEstimate];
        private int updateIndex = 0;
        private Stopwatch oTimePaused = new Stopwatch();
        private DateTime oTimeStart;
        private TimeSpan oTimeElapsed;

        internal StatusUpdate(string name)
		{
            jobName = name;
            estimatedTime = null;
			hasError = false;
			isComplete = false;
			wasAborted = false;
			clipPosition = null;
            clipLength = null;
            nbFramesDone = null;
            nbFramesTotal = null;
			projectedFileSize = null;
            processingspeed = null;
			filesize = null;
            jobStatus = MeGUI.JobStatus.PROCESSING;
            ResetTime();
            for (int i = 0; i < UpdatesPerEstimate; ++i)
            {
                previousUpdates[i] = TimeSpan.Zero;
                previousUpdatesProgress[i] = 0M;
            }
		}

        /// <summary>
        /// What is currently processing?
        /// </summary>
        public string Status
        {
            get { return status; }
            set { status = value; }
        }

        /// <summary>
        /// Job Status currently processing
        /// </summary>
        public JobStatus JobStatus
        {
            get { return jobStatus; }
            set
            {
                jobStatus = value;
                if (jobStatus == JobStatus.PAUSED)
                    oTimePaused.Start();
                else
                    oTimePaused.Stop();
            }
        }

		/// <summary>
		/// does the job have any errors?
		/// </summary>
		public bool HasError
		{
			get { return hasError; }
			set { hasError = value; }
		}

		/// <summary>
		///  has the encoding job completed?
		/// </summary>
		public bool IsComplete
		{
			get { return isComplete; }
			set { isComplete = value; }
		}

		/// <summary>
		/// did we get this statusupdate because the job was aborted?
		/// </summary>
		public bool WasAborted
		{
			get { return wasAborted; }
			set { wasAborted = value; }
		}

		/// <summary>
		/// name of the job this statusupdate is refering to
		/// </summary>
		public string JobName
		{
			get { return jobName; }
			set { jobName = value; }
		}

		/// <summary>
		///  position in clip
		/// </summary>
		public TimeSpan? ClipPosition
		{
			get { return clipPosition; }
            set { _currentTime = value ?? _currentTime; clipPosition = _currentTime; }
		}

        /// <summary>
        /// Length of clip
        /// </summary>
        public TimeSpan? ClipLength
        {
            get { return clipLength; }
            set { _totalTime = value ?? _totalTime; clipLength = _totalTime;  }
        }

		/// <summary>
		/// number of frames that have been encoded so far
		/// </summary>
		public ulong? NbFramesDone
		{
			get { return nbFramesDone; }
            set { _frame = value ?? _frame; nbFramesDone = _frame; }
		}

		/// <summary>
		/// number of frames of the source
		/// </summary>
		public ulong? NbFramesTotal
		{
			get { return nbFramesTotal; }
            set { _framecount = value ?? _framecount; nbFramesTotal = _framecount; }
		}

        /// <summary>
        /// Some estimate of the encoding speed (eg FPS, or ratio to realtime)
        /// </summary>
        public string ProcessingSpeed
        {
            get { return processingspeed; }
        }

		/// <summary>
		/// projected output size
		/// </summary>
		public FileSize? ProjectedFileSize
		{
			get { return projectedFileSize; }
            set { _totalSize = value ?? _totalSize; projectedFileSize = _totalSize; }
		}

        public int PercentageDone
        {
            get { return (int)PercentageDoneExact; }
        }

		/// <summary>
		/// gets / sets the exact percentage of the encoding progress
		/// </summary>
        public decimal? PercentageDoneExact
        {
            get { return percentage; }
            set { _percent = value ?? _percent; percentage = _percent ?? 0M; }
        }

		/// <summary>
		/// size of the encoded file at this point
		/// </summary>
		public FileSize? CurrentFileSize
		{
			get { return filesize; }
            set { _currentSize = value ?? _currentSize; filesize = _currentSize; }
		}

		/// <summary>
		/// time elapsed between start of encoding and the point where this status update is being sent
		/// </summary>
		public TimeSpan TimeElapsed
		{
			get { return oTimeElapsed; }
		}

        public void ResetTime()
        {
            oTimePaused.Reset();
            oTimeStart = DateTime.Now;
        }

        #region REAL variables
        TimeSpan? _timeEstimate = null;
        public FileSize? _audioSize = null;

        // The following groups each allow progress to be calculated (in percent)
        decimal? _percent = null;

        ulong? _frame = null;
        ulong? _framecount = null;

        FileSize? _currentSize = null;
        FileSize? _totalSize = null;

        TimeSpan? _currentTime = null;
        TimeSpan? _totalTime = null;
        #endregion

        public void FillValues()
        {
            try
            {
                oTimeElapsed = (DateTime.Now - oTimeStart) - oTimePaused.Elapsed;

                // First we attempt to find the percent done
                decimal? fraction = null;

                // Percent
                if (_percent.HasValue)
                    fraction = _percent / 100M;
                // Time estimates
                else if (_timeEstimate.HasValue && _timeEstimate != TimeSpan.Zero)
                    fraction = ((decimal)oTimeElapsed.Ticks / (decimal)_timeEstimate.Value.Ticks);
                // Frame counts
                else if (_frame.HasValue && _framecount.HasValue && _framecount != 0)
                    fraction = ((decimal)_frame.Value / (decimal)_framecount.Value);
                // File sizes
                else if (_currentSize.HasValue && _totalSize.HasValue && _totalSize != FileSize.Empty)
                    fraction = (_currentSize.Value / _totalSize.Value);
                // Clip positions
                else if (_currentTime.HasValue && _totalTime.HasValue && _totalTime != TimeSpan.Zero)
                    fraction = ((decimal)_currentTime.Value.Ticks / (decimal)_totalTime.Value.Ticks);

                if (fraction.HasValue)
                    percentage = fraction.Value * 100M;

                /// Frame counts
                if (_frame.HasValue)
                    nbFramesDone = _frame.Value;
                if (_framecount.HasValue)
                    nbFramesTotal = _framecount.Value;
                if (_framecount.HasValue && !_frame.HasValue && fraction.HasValue)
                    nbFramesDone = (ulong)(fraction.Value * _framecount.Value);
                if (!_framecount.HasValue && _frame.HasValue && fraction.HasValue)
                    nbFramesTotal = (ulong)(_frame.Value / fraction.Value);

                /// Sizes
                if (_currentSize.HasValue)
                    filesize = _currentSize;
                if (_totalSize.HasValue)
                    projectedFileSize = _totalSize;
                if (_currentSize.HasValue && !_totalSize.HasValue && fraction.HasValue)
                    projectedFileSize = _currentSize / fraction.Value;
                // We don't estimate current size
                // because it would suggest to the user that
                // we are actually measuring it

                // We don't estimate the current time or total time
                // in the clip, because it would suggest we are measuring it.
                if (_currentTime.HasValue)
                    clipPosition = _currentTime;
                if (_totalTime.HasValue)
                    clipLength = _totalTime;
                // However, if we know the total time and the percent, it is
                // ok to estimate the current position
                if (_totalTime.HasValue && !_currentTime.HasValue && fraction.HasValue)
                    clipPosition = new TimeSpan((long)((decimal)_totalTime.Value.Ticks * fraction.Value));

                // FPS
                if (_frame.HasValue && oTimeElapsed.TotalSeconds > 0)
                    processingspeed =
                        Util.ToString((decimal)_frame.Value / (decimal)oTimeElapsed.TotalSeconds, false) + " FPS";
                // Other processing speeds
                else if (_currentTime.HasValue && oTimeElapsed.Ticks > 0)
                    processingspeed =
                        Util.ToString((decimal)_currentTime.Value.Ticks / (decimal)oTimeElapsed.Ticks, false) + "x realtime";
                else if (fraction.HasValue && _totalTime.HasValue && oTimeElapsed.Ticks > 0)
                    processingspeed =
                        Util.ToString((decimal)_totalTime.Value.Ticks * fraction.Value / (decimal)oTimeElapsed.Ticks, false) + "x realtime";

                // Processing time
                if (fraction.HasValue)
                {
                    TimeSpan time = oTimeElapsed - previousUpdates[updateIndex];
                    decimal progress = fraction.Value - previousUpdatesProgress[updateIndex];
                    if (progress > 0 && time > FiveSeconds)
                        estimatedTime = new TimeSpan((long)((decimal)time.Ticks * (1M - fraction) / progress));
                    else
                        estimatedTime = new TimeSpan((long)((decimal)oTimeElapsed.Ticks * ((1 / fraction.Value) - 1)));

                    previousUpdates[updateIndex] = oTimeElapsed;
                    previousUpdatesProgress[updateIndex] = fraction.Value;
                    updateIndex = (updateIndex+1)% UpdatesPerEstimate;
                }
            }
            catch (Exception)
            {
            }
        }
	}
}
