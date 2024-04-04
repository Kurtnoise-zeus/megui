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
using System.Diagnostics;

using MeGUI.core.util;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

namespace MeGUI
{
	/// <summary>
	/// Class that is used to send an encoding update from the encoder to the GUI
	/// it contains all the elements that will be updated in the GUI at some point
	/// </summary>
	public class StatusUpdate
    {
        private string _processingSpeedOverall;

        private ulong? nbFramesDone, nbFramesTotal;

        #region REAL variables
        // The following groups each allow progress to be calculated (in percent)

        ulong? _frame = null;
        ulong? _framecount = null;

        TimeSpan? _currentTime = null;
        TimeSpan? _totalTime = null;
        private double? _fps = null;
        #endregion

        #region ESTIMATED variables

        // variables for time handling - exact
        private Stopwatch _oTimeElapsed = new Stopwatch();
        private TimeSpan _timeRemaining;

        private decimal? _percentCurrent;
        private decimal _percentEstimated;

        private string _strStatus, _strJobOutput, _strJobName;
        private bool _bHasError, _bIsComplete, _bWasAborted;
        private JobStatus _jobStatus;

        private FileSize? _fileSizeCurrent, _fileSizeEstimated, _fileSizeTotal;

        private TimeSpan? _clipPosition, _clipLength;

        private StatusHistory _statusHistory;

        private string _processingSpeedCurrent;

        #endregion

        public StatusUpdate(string name, string jobOutput)
		{
            _strJobName = name;
            _strJobOutput = jobOutput;
            _bHasError = false;
            _bIsComplete = false;
            _bWasAborted = false;
            _strStatus = "";
            _percentEstimated = 0;
			_clipPosition = null;
            _clipLength = null;
            nbFramesDone = null;
            nbFramesTotal = null;
            _processingSpeedCurrent = null;
            _processingSpeedOverall = null;
            
            _jobStatus = MeGUI.JobStatus.PROCESSING;
            _statusHistory = new StatusHistory();
            ResetTime();
		}

        /// <summary>
        /// What is currently processing?
        /// </summary>
        public string Status
        {
            get { return _strStatus; }
            set { _strStatus = value; }
        }

        /// <summary>
        /// Job Status currently processing
        /// </summary>
        public JobStatus JobStatus
        {
            get { return _jobStatus; }
            set
            {
                _jobStatus = value;
                if (_jobStatus == JobStatus.PAUSED)
                    _oTimeElapsed.Stop();
                else
                    _oTimeElapsed.Start();
            }
        }

		/// <summary>
		/// does the job have any errors?
		/// </summary>
		public bool HasError
        {
			get { return _bHasError; }
			set { _bHasError = value; }
        }

		/// <summary>
		///  has the encoding job completed?
		/// </summary>
		public bool IsComplete
        {
			get { return _bIsComplete; }
			set { _bIsComplete = value; }
        }

		/// <summary>
		/// did we get this statusupdate because the job was aborted?
		/// </summary>
		public bool WasAborted
        {
			get { return _bWasAborted; }
			set { _bWasAborted = value; }
        }

		/// <summary>
		/// name of the job this statusupdate is refering to
		/// </summary>
		public string JobName
        {
			get { return _strJobName; }
			set { _strJobName = value; }
        }

		/// <summary>
		///  position in clip
		/// </summary>
		public TimeSpan? ClipPosition
        {
			get { return _clipPosition; }
            set { _currentTime = value ?? _currentTime; _clipPosition = _currentTime; }
        }

        /// <summary>
        /// Length of clip
        /// </summary>
        public TimeSpan? ClipLength
        {
            get { return _clipLength; }
            set { _totalTime = value ?? _totalTime; _clipLength = _totalTime; }
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
        /// FPS of the source
        /// </summary>
        public double? FPS
        {
            get { return _fps; }
            set { _fps = value ?? _fps; }
        }

        /// <summary>
        /// Some estimate of the overall encoding speed (eg FPS, or ratio to realtime)
        /// </summary>
        public string OverallSpeed
        {
            get { return _processingSpeedOverall; }
        }

        /// <summary>
        /// Some estimate of the current encoding speed (eg FPS, or ratio to realtime)
        /// </summary>
        public string CurrentSpeed
        {
            get { return _processingSpeedCurrent; }
        }

        /// <summary>
        /// projected output size
        /// </summary>
        public FileSize? FileSizeTotal
       {
            get
            {
                if (!_fileSizeTotal.HasValue && _fileSizeEstimated.HasValue)
                    return _fileSizeEstimated;
                return _fileSizeTotal;
            }
            set { _fileSizeTotal = value; }
        }

        /// <summary>
        /// size of the encoded file at this point
        /// </summary>
        public FileSize? FileSizeCurrent
        {
            get { return _fileSizeCurrent; }
            set { _fileSizeCurrent = value; }
        }

		/// <summary>
		/// gets / sets the exact percentage of the job progress
		/// </summary>
        public decimal? PercentageCurrent
        {
            get { return _percentCurrent; }
            set
                {
                    decimal ? _percentOld = _percentCurrent;
                    _percentCurrent = value;
                    _percentEstimated = value ?? 0M;
                if (_percentCurrent.HasValue)
                {
                    if (_statusHistory.InitialResetCount == 0)
                    {
                        _statusHistory.InitialResetCount = 1;
                        _oTimeElapsed.Restart();
                    }
                    else if (_percentOld.HasValue && _percentOld > _percentCurrent)
                    {
                        ResetTime();
                        _statusHistory.InitialResetCount = 0;
                    }
                    else if (_statusHistory.InitialResetCount == 1)
                        _statusHistory.InitialResetCount = 2;
                    _statusHistory.SetValue(value ?? 0M, _oTimeElapsed.Elapsed);
                }
        }
    }

        /// <summary>
        /// gets / sets the estimated percentage of the job progress
        /// </summary>
        public decimal PercentageEstimated
        {
            get { return _percentEstimated; }
        }

        /// <summary>
        /// time elapsed between start of encoding and the point where this status update is being sent
        /// </summary>
        public TimeSpan TimeElapsed
        {
        get { return _oTimeElapsed.Elapsed; }
        }

        /// <summary>
        /// time elapsed between start of encoding and the point where this status update is being sent
        /// </summary>
        public TimeSpan TimeRemaining
        {
            get { return _timeRemaining; }
        }

        public void ResetTime()
        {
            _oTimeElapsed.Reset();
            _statusHistory = new StatusHistory();
            _oTimeElapsed.Start();
        }

        public void FillValues()
        {
            try
            {
                // First we attempt to find the percent done
                decimal? fraction = null;

                // Percent
                if (_percentCurrent.HasValue)
                    fraction = _percentCurrent / 100M;
                // Frame counts
                else if (_frame.HasValue && _framecount.HasValue && _framecount != 0)
                    fraction = ((decimal)_frame.Value / (decimal)_framecount.Value);
                // File sizes
                else if (_fileSizeCurrent.HasValue && _fileSizeTotal.HasValue && _fileSizeTotal != FileSize.Empty)
                    fraction = (_fileSizeCurrent.Value / _fileSizeTotal.Value);
                // Clip positions
                else if (_currentTime.HasValue && _totalTime.HasValue && _totalTime != TimeSpan.Zero)
                    fraction = ((decimal)_currentTime.Value.Ticks / (decimal)_totalTime.Value.Ticks);

                /// Frame counts
                if (_frame.HasValue)
                    NbFramesDone = _frame.Value;
                if (_framecount.HasValue)
                    NbFramesTotal = _framecount.Value;
                if (_framecount.HasValue && !_frame.HasValue && fraction.HasValue)
                    NbFramesDone = (fraction.Value > 0 ? (ulong)(fraction.Value * _framecount.Value) : NbFramesTotal); // to avoid false positive mismatch at the end of encoding that may happen - https://forum.doom9.org/showthread.php?t=185380
                if (!_framecount.HasValue && _frame.HasValue && fraction.HasValue)
                    NbFramesTotal = (fraction.Value > 0 ? (ulong)(_frame.Value / fraction.Value) : NbFramesDone); // to avoid false positive mismatch at the end of encoding that may happen - https://forum.doom9.org/showthread.php?t=185380

                // File size
                _fileSizeCurrent = FileSize.Of2(_strJobOutput);
                if (_fileSizeCurrent.HasValue && !_fileSizeTotal.HasValue && fraction.HasValue)
                    _fileSizeEstimated = _fileSizeCurrent / fraction.Value;
                
                // Clip position
                if (_totalTime.HasValue)
                    _clipLength = _totalTime;
                if (_currentTime.HasValue)
                    _clipPosition = _currentTime;
                else if (_totalTime.HasValue && fraction.HasValue)
                    _clipPosition = new TimeSpan((long)((decimal)_totalTime.Value.Ticks * fraction.Value));
                
                // Overall speed
                if (_frame.HasValue && _oTimeElapsed.Elapsed.TotalSeconds > 0)
                    _processingSpeedOverall =
                    Util.ToString((decimal)_frame.Value / (decimal)_oTimeElapsed.Elapsed.TotalSeconds, false) + " FPS";
                else if (_currentTime.HasValue && _oTimeElapsed.ElapsedTicks > 0)
                    _processingSpeedOverall =
                    Util.ToString((decimal)_currentTime.Value.Ticks / (decimal)_oTimeElapsed.ElapsedTicks, false) + "x";
                else if (fraction.HasValue && _totalTime.HasValue && _oTimeElapsed.ElapsedTicks > 0)
                    _processingSpeedOverall =
                    Util.ToString((decimal)_totalTime.Value.Ticks * fraction.Value / (decimal)_oTimeElapsed.ElapsedTicks, false) + "x";
                else if (_statusHistory.InitialResetCount > 1)
                    _processingSpeedOverall = Util.ToString(_statusHistory.GetSpeedOverall(), false) + " PPM";
                
                
                _percentEstimated = _statusHistory.GetEstimatedProgress(_oTimeElapsed.Elapsed, out decimal dProgressPerTick);
                if (_percentEstimated > 0)
                {
                    // estimate remaining time
                    _timeRemaining = new TimeSpan((long)(((decimal)100 - _percentEstimated) * ((decimal)1 / dProgressPerTick)));
                    
                    if (_frame.HasValue && _totalTime.HasValue)
                        _processingSpeedCurrent = Util.ToString(_totalTime.Value.Ticks / (100 / dProgressPerTick) * (decimal)_fps.Value, false) + " FPS";
                    else if (_totalTime.HasValue)
                        _processingSpeedCurrent = Util.ToString(_totalTime.Value.Ticks / (100 / dProgressPerTick), false) + "x";
                    else
                        _processingSpeedCurrent = Util.ToString(dProgressPerTick * 600000000, false) + " PPM";
                }
                else
                    _processingSpeedCurrent = _processingSpeedOverall;
            }
            catch (Exception)
            {
            }
        }
    }

    public class StatusHistory
    {
        private static readonly TimeSpan _timeBetweenUpdates = new TimeSpan(0, 0, 5);
        private const int _historySize = 10;

        private TimeSpan _lastTime;
        private decimal _lastValue;
        private decimal _progressPerTick;
        private TimeSpan[] _historyTime = new TimeSpan[_historySize];
        private decimal[] _historyValue = new decimal[_historySize];
        private int _nextIndex = 0;
        private int _oldestIndex = 0;
        private int _initialResetCount = 0;

        public int InitialResetCount { get => _initialResetCount; set => _initialResetCount = value; }

        public StatusHistory()
        {
            for (int i = 0; i<_historySize; ++i)
            {
                _historyTime[i] = TimeSpan.Zero;
                _historyValue[i] = 0M;
            }

        _lastValue = -1;

        SetValue(0, new TimeSpan(0));
    }

    public void SetValue(decimal progress, TimeSpan time)
    {
                if (_lastValue == progress)
                        return;
    
    _lastTime = time;
    _lastValue = progress;
    
                if (_historyTime[_nextIndex] != TimeSpan.Zero && (time - _historyTime[(_nextIndex + 9) % _historySize]) < _timeBetweenUpdates)
                        return;
    
    _historyTime[_nextIndex] = time;
    _historyValue[_nextIndex] = progress;
    _nextIndex = (_nextIndex + 1) % _historySize;
    
    _oldestIndex = _nextIndex;
                if (_historyTime[_oldestIndex] == TimeSpan.Zero)
                    {
                        for (int i = 1; i < _historySize; ++i)
                            {
            _oldestIndex = (_nextIndex + i) % _historySize;
                                if (_historyTime[_oldestIndex] != TimeSpan.Zero)
                                        break;
                            }
                    }
    
    decimal dProgressPerTick;
                if (_lastTime != TimeSpan.Zero && _lastTime != _historyTime[_oldestIndex])
                    {
        TimeSpan oTimeElapsed = _lastTime - _historyTime[_oldestIndex];
        decimal dProgressElapsed = _lastValue - _historyValue[_oldestIndex];
        dProgressPerTick = dProgressElapsed / oTimeElapsed.Ticks;
                    }
                else
        dProgressPerTick = 0;
    _progressPerTick = dProgressPerTick;
            }

        public decimal GetEstimatedProgress(TimeSpan _timeElapsedTotal, out decimal dProgressPerTick)
        {
    TimeSpan oTimeElapsed = _timeElapsedTotal - _lastTime;
    decimal dProgressEstimated = 0;
                if (oTimeElapsed.Ticks > 0)
        dProgressEstimated = oTimeElapsed.Ticks * _progressPerTick;
                else
        dProgressEstimated = 0;
    dProgressPerTick = _progressPerTick;
    
    decimal estimatedProgress = _lastValue + dProgressEstimated;
                if (estimatedProgress > 100)
                        return 100;
                return estimatedProgress;
            }

        public decimal GetSpeedOverall()
        {
                return _lastValue / (decimal)_lastTime.TotalMinutes;
            }
    }
}