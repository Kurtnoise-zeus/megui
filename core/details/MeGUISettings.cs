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
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;

using MeGUI.core.util;
using MeGUI.core.gui;

namespace MeGUI
{
	/// <summary>
	/// Summary description for MeGUISettings.
	/// </summary>
	[LogByMembers]
    public class MeGUISettings
    {
        #region variables
        public enum OCGUIMode
        {
            [EnumTitle("Show Basic Settings")]
            Basic,
            [EnumTitle("Show Default Settings")]
            Default,
            [EnumTitle("Show Advanced Settings")]
            Advanced
        };
        public enum StandbySettings
        {
            [EnumTitle("Do not prevent standby")]
            SystemDefault,
            [EnumTitle("Prevent system standby")]
            DisableSystemStandby,
            [EnumTitle("Prevent monitor standby")]
            DisableMonitorStandby
        };
        private string[][] autoUpdateServerLists;
        private DateTime lastUpdateCheck;
        private string strMainAudioFormat, strMainFileFormat, meguiupdatecache, neroAacEncPath, version,
                       defaultLanguage1, defaultLanguage2, afterEncodingCommand, videoExtension, audioExtension,
                       strEac3toLastFolderPath, strEac3toLastFilePath, strEac3toLastDestinationPath, tempDirMP4,
                       fdkAacPath, httpproxyaddress, httpproxyport, httpproxyuid, httpproxypwd, defaultOutputDir, exhalePath,
                       appendToForcedStreams, lastUsedOneClickFolder, lastUpdateServer, chapterCreatorSortString, muxInputPath, muxOutputPath;
        private bool autoForceFilm, autoOpenScript, bUseQAAC, bUseDGIndexNV, bUseDGIndexIM, bInput8Bit,
                     overwriteStats, keep2of3passOutput, autoUpdate, deleteIntermediateFiles, workerAutoStart,
                     deleteAbortedOutput, openProgressWindow, bEac3toAutoSelectStreams, bUseFDKAac, bVobSubberKeepAll,
                     alwaysOnTop, addTimePosition, alwaysbackupfiles, bUseITU, bEac3toLastUsedFileMode, bMeGUIx64,
                     bAutoLoadDG, bAlwayUsePortableAviSynth, bVobSubberSingleFileExport, workerAutoStartOnStartup,
                     bEnsureCorrectPlaybackSpeed, bExternalMuxerX264, bUseNeroAacEnc, bOSx64, bEnableDirectShowSource,
                     bVobSubberExtractForced, bVobSubberShowAll, bUsex64Tools, bShowDebugInformation, bEac3toDefaultToHD,
                     bEac3toEnableEncoder, bEac3toEnableDecoder, bEac3toEnableCustomOptions, bFirstUpdateCheck,
                     bChapterCreatorCounter, bX264AdvancedSettings, bEac3toAddPrefix, workerRemoveJob;
        private decimal forceFilmThreshold, acceptableFPSError;
        private int nbPasses, autoUpdateServerSubList, minComplexity, updateFormSplitter,
                    maxComplexity, jobColumnWidth, inputColumnWidth, outputColumnWidth, codecColumnWidth,
                    modeColumnWidth, statusColumnWidth, startColumnWidth, endColumnWidth, fpsColumnWidth,
                    updateFormUpdateColumnWidth, updateFormNameColumnWidth, updateFormLocalVersionColumnWidth,
                    updateFormServerVersionColumnWidth, updateFormLocalDateColumnWidth, updateFormServerDateColumnWidth,
                    updateFormLastUsedColumnWidth, updateFormStatusColumnWidth, updateFormServerArchitectureColumnWidth, 
                    ffmsThreads, chapterCreatorMinimumLength, updateCheckInterval, disablePackageInterval, iWorkerMaximumCount;
        private double dpiScaleFactor, dLastDPIScaleFactor;
        private SourceDetectorSettings sdSettings;
        private AutoEncodeDefaultsSettings aedSettings;
        private DialogSettings dialogSettings;
        private Point mainFormLocation, updateFormLocation;
        private Size mainFormSize, updateFormSize;
        private FileSize[] customFileSizes;
        private FPS[] customFPSs;
        private Dar[] customDARs;
        private OCGUIMode ocGUIMode;
        private StandbySettings standbySetting;
        private AfterEncoding afterEncoding;
        private ProxyMode httpProxyMode;
        private List<WorkerSettings> arrWorkerSettings;
        private List<WorkerPriority> arrWorkerPriority;
        private ProgramSettings avimuxgui, avisynth, avisynthplugins, besplit, dgindexim, dgindex, dgindexnv,
                                eac3to, fdkaac, ffmpeg, ffms, flac, haali, lame, lsmash, mediainfo,
                                megui_core, megui_help, megui_libs, megui_updater, mkvmerge, mp4box, neroaacenc, exhale, 
                                oggenc, opus, pgcdemux, qaac, redist, tsmuxer, vsrip, x264, x265, xvid, bestsource, svtav1psy;
        Dictionary<string, string> oRedistVersions;
        #endregion
        public MeGUISettings()
		{
            // get the DPI scale factor
            using (Graphics graphics = Graphics.FromHwnd(IntPtr.Zero))
            {
                float dpiX = graphics.DpiX;
                float dpiY = graphics.DpiY;
                dpiScaleFactor = dpiX / 96.0;
            }

            // OS / build detection
#if x64
            bMeGUIx64 = true;
            bOSx64 = true;
#else
            bMeGUIx64 = false;
            bOSx64 = OSInfo.IsWow64();
#endif
            bUsex64Tools = true;

            autoUpdateServerLists = new string[][] { new string[] { "Development", "http://megui.org/auto/fork/" }, new string[] { "Custom" }};
            lastUpdateCheck = DateTime.Now.AddDays(-77).ToUniversalTime();
            lastUpdateServer = "http://megui.org/auto/fork/";
            disablePackageInterval = 14;
            updateCheckInterval = 240;
            acceptableFPSError = 0.01M;
            autoUpdateServerSubList = 0;
            autoUpdate = true;
            dialogSettings = new DialogSettings();
            sdSettings = new SourceDetectorSettings();
            AedSettings = new AutoEncodeDefaultsSettings();
            meguiupdatecache = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "update_cache");
			autoForceFilm = true;
            bAutoLoadDG = true;
			forceFilmThreshold = new decimal(95);
			defaultLanguage1 = "English";
			defaultLanguage2 = "English";
            afterEncoding = AfterEncoding.DoNothing;
			autoOpenScript = true;
			overwriteStats = true;
			keep2of3passOutput = false;
			nbPasses = 2;
			deleteIntermediateFiles = true;
			deleteAbortedOutput = true;
            bEac3toAutoSelectStreams = true;
            strEac3toLastFolderPath = strEac3toLastFilePath = strEac3toLastDestinationPath = "";
            bEac3toLastUsedFileMode = false;
            bEac3toDefaultToHD = bEac3toEnableEncoder = bEac3toEnableDecoder = bEac3toEnableCustomOptions = false;
            bEac3toAddPrefix = true;
            openProgressWindow = true;
            videoExtension = "";
            audioExtension = "";
            alwaysOnTop = false;
            httpProxyMode = ProxyMode.None;
            httpproxyaddress = "";
            httpproxyport = "";
            httpproxyuid = "";
            httpproxypwd = "";
            defaultOutputDir = "";
            tempDirMP4 = "";
            addTimePosition = true;
            alwaysbackupfiles = false;
            strMainFileFormat = "";
            strMainAudioFormat = "";
            minComplexity = 72;
            maxComplexity = 78;
            mainFormLocation = new Point(0, 0);
            mainFormSize = new Size(713, 478);
            updateFormLocation = new Point(0, 0);
            updateFormSize = new Size(780, 313);
            updateFormSplitter = 180;
            updateFormUpdateColumnWidth = 47;
            updateFormNameColumnWidth = 105;
            updateFormLocalVersionColumnWidth = 117; 
            updateFormServerVersionColumnWidth = 117;
            updateFormServerArchitectureColumnWidth = 50;
            updateFormLocalDateColumnWidth = 70;
            updateFormServerDateColumnWidth = 70;
            updateFormLastUsedColumnWidth = 70;
            updateFormStatusColumnWidth = 111;
            jobColumnWidth = 40;
            inputColumnWidth = 105;
            outputColumnWidth = 105;
            codecColumnWidth = 79;
            modeColumnWidth = 79;
            statusColumnWidth = 65;
            startColumnWidth = 58;
            endColumnWidth = 58;
            fpsColumnWidth = 95;
            bEnsureCorrectPlaybackSpeed = false;
            bAlwayUsePortableAviSynth = true;
            ffmsThreads = 1;
            appendToForcedStreams = "";
            ocGUIMode = OCGUIMode.Default;
            bUseITU = true;
            lastUsedOneClickFolder = "";
            bUseNeroAacEnc = bUseFDKAac = bUseQAAC = bUseDGIndexNV = bUseDGIndexIM = false;
            chapterCreatorMinimumLength = 900;
            bExternalMuxerX264 = true;
            bVobSubberSingleFileExport = false;
            bVobSubberKeepAll = false;
            bVobSubberExtractForced = false;
            bVobSubberShowAll = false;
            chapterCreatorSortString = "duration";
            bShowDebugInformation = false;
            bEnableDirectShowSource = false;
            bFirstUpdateCheck = true;
            dLastDPIScaleFactor = 0;
            oRedistVersions = new Dictionary<string, string>();
            bChapterCreatorCounter = true;
            bX264AdvancedSettings = false;
            workerAutoStart = true;
            workerAutoStartOnStartup = false;
            workerRemoveJob = true;
            bInput8Bit = true;
            ResetWorkerSettings();
            ResetWorkerPriority();
            version = "";
            standbySetting = StandbySettings.DisableSystemStandby;
            muxInputPath = null;
            muxOutputPath = null;
        }

        #region properties

        [XmlIgnore]
        public bool IsMeGUIx64
        {
            get { return bMeGUIx64; }
        }

        [XmlIgnore]
        public bool IsOSx64
        {
            get { return bOSx64; }
        }

        public bool Usex64Tools
        {
            get
            {
                if (!IsOSx64)
                    bUsex64Tools = false;
                else if (IsMeGUIx64)
                    bUsex64Tools = true;
                return bUsex64Tools;
            }
            set { bUsex64Tools = value; }
        }

        public bool ShowDebugInformation
        {
            get { return bShowDebugInformation; }
            set { bShowDebugInformation = value; }
        }

        [XmlIgnore]
        public double DPIScaleFactor
        {
            get { return dpiScaleFactor; }
        }

        public double LastDPIScaleFactor
        {
            get { return dpiScaleFactor; }
            set { dLastDPIScaleFactor = value; }
        }

        public Point MainFormLocation
        {
            get { return mainFormLocation; }
            set { mainFormLocation = value; }
        }

        public Size MainFormSize
        {
            get { return mainFormSize; }
            set { mainFormSize = value; }
        }

        public Point UpdateFormLocation
        {
            get { return updateFormLocation; }
            set { updateFormLocation = value; }
        }

        public Size UpdateFormSize
        {
            get { return updateFormSize; }
            set { updateFormSize = value; }
        }

        public int UpdateFormSplitter
        {
            get { return updateFormSplitter; }
            set { updateFormSplitter = value; }
        }

        public int UpdateFormUpdateColumnWidth
        {
            get { return updateFormUpdateColumnWidth; }
            set { updateFormUpdateColumnWidth = value; }
        }

        public int UpdateFormNameColumnWidth
        {
            get { return updateFormNameColumnWidth; }
            set { updateFormNameColumnWidth = value; }
        }

        public int UpdateFormLocalVersionColumnWidth
        {
            get { return updateFormLocalVersionColumnWidth; }
            set { updateFormLocalVersionColumnWidth = value; }
        }

        public int UpdateFormServerVersionColumnWidth
        {
            get { return updateFormServerVersionColumnWidth; }
            set { updateFormServerVersionColumnWidth = value; }
        }

        public int UpdateFormServerArchitectureColumnWidth
        {
            get { return updateFormServerArchitectureColumnWidth; }
            set { updateFormServerArchitectureColumnWidth = value; }
        }

        public int UpdateFormLocalDateColumnWidth
        {
            get { return updateFormLocalDateColumnWidth; }
            set { updateFormLocalDateColumnWidth = value; }
        }

        public int UpdateFormServerDateColumnWidth
        {
            get { return updateFormServerDateColumnWidth; }
            set { updateFormServerDateColumnWidth = value; }
        }

        public int UpdateFormLastUsedColumnWidth
        {
            get { return updateFormLastUsedColumnWidth; }
            set { updateFormLastUsedColumnWidth = value; }
        }

        public int UpdateFormStatusColumnWidth
        {
            get { return updateFormStatusColumnWidth; }
            set { updateFormStatusColumnWidth = value; }
        }

        public int JobColumnWidth
        {
            get { return jobColumnWidth; }
            set { jobColumnWidth = value; }
        }

        public int InputColumnWidth
        {
            get { return inputColumnWidth; }
            set { inputColumnWidth = value; }
        }

        public int OutputColumnWidth
        {
            get { return outputColumnWidth; }
            set { outputColumnWidth = value; }
        }

        public int CodecColumnWidth
        {
            get { return codecColumnWidth; }
            set { codecColumnWidth = value; }
        }

        public int ModeColumnWidth
        {
            get { return modeColumnWidth; }
            set { modeColumnWidth = value; }
        }

        public int StatusColumnWidth
        {
            get { return statusColumnWidth; }
            set { statusColumnWidth = value; }
        }

        public int StartColumnWidth
        {
            get { return startColumnWidth; }
            set { startColumnWidth = value; }
        }

        public int EndColumnWidth
        {
            get { return endColumnWidth; }
            set { endColumnWidth = value; }
        }

        public int FPSColumnWidth
        {
            get { return fpsColumnWidth; }
            set { fpsColumnWidth = value; }
        }

        public FileSize[] CustomFileSizes
        {
            get { return customFileSizes; }
            set { customFileSizes = value; }
        }

        public FPS[] CustomFPSs
        {
            get { return customFPSs; }
            set { customFPSs = value; }
        }

        public Dar[] CustomDARs
        {
            get { return customDARs; }
            set { customDARs = value; }
        }

        public string Version
        {
            get { return version; }
            set { version = value; }
        }

        public string MuxInputPath
        {
            get { return muxInputPath; }
            set { muxInputPath = value; }
        }

        public string MuxOutputPath
        {
            get { return muxOutputPath; }
            set { muxOutputPath = value; }
        }

        public string Eac3toLastFolderPath
        {
            get { return strEac3toLastFolderPath; }
            set { strEac3toLastFolderPath = value; }
        }

        public string Eac3toLastFilePath
        {
            get { return strEac3toLastFilePath; }
            set { strEac3toLastFilePath = value; }
        }

        public string Eac3toLastDestinationPath
        {
            get { return strEac3toLastDestinationPath; }
            set { strEac3toLastDestinationPath = value; }
        }

        public bool Eac3toLastUsedFileMode
        {
            get { return bEac3toLastUsedFileMode; }
            set { bEac3toLastUsedFileMode = value; }
        }

        public bool Eac3toDefaultToHD
        {
            get { return bEac3toDefaultToHD; }
            set { bEac3toDefaultToHD = value; }
        }

        public bool Eac3toEnableEncoder
        {
            get { return bEac3toEnableEncoder; }
            set { bEac3toEnableEncoder = value; }
        }

        public bool Eac3toEnableDecoder
        {
            get { return bEac3toEnableDecoder; }
            set { bEac3toEnableDecoder = value; }
        }

        /// <summary>
        /// true if HD Streams Extractor should automatically select tracks
        /// </summary>
        public bool Eac3toAutoSelectStreams
        {
            get { return bEac3toAutoSelectStreams; }
            set { bEac3toAutoSelectStreams = value; }
        }

        public bool Eac3toAddPrefix
        {
            get { return bEac3toAddPrefix; }
            set { bEac3toAddPrefix = value; }
        }

        public bool Eac3toEnableCustomOptions
        {
            get { return bEac3toEnableCustomOptions; }
            set { bEac3toEnableCustomOptions = value; }
        }

        public bool ChapterCreatorCounter
        {
            get { return bChapterCreatorCounter; }
            set { bChapterCreatorCounter = value; }
        }

        public bool X264AdvancedSettings
        {
            get { return bX264AdvancedSettings; }
            set { bX264AdvancedSettings = value; }
        }

        public bool EnableDirectShowSource
        {
            get { return bEnableDirectShowSource; }
            set { bEnableDirectShowSource = value; }
        }

        /// <summary>
        /// True if no update check has been started yet
        /// </summary>
        public bool FirstUpdateCheck
        {
            get { return bFirstUpdateCheck; }
            set { bFirstUpdateCheck = value; }
        }

        /// <summary>
        /// Gets / sets whether the one-click advanced settings will be shown
        /// </summary>
        public OCGUIMode OneClickGUIMode
        {
            get { return ocGUIMode; }
            set { ocGUIMode = value; }
        }

        /// <summary>
        /// Gets / sets the standby settings
        /// </summary>
        public StandbySettings StandbySetting
        {
            get { return standbySetting; }
            set { standbySetting = value; }
        }

        /// <summary>
        /// Gets / sets whether the playback speed in video preview should match the fps
        /// </summary>
        public bool EnsureCorrectPlaybackSpeed
        {
            get { return bEnsureCorrectPlaybackSpeed; }
            set { bEnsureCorrectPlaybackSpeed = value; }
        }

        /// <summary>
        /// Maximum error that the bitrate calculator should accept when rounding the framerate
        /// </summary>
        public decimal AcceptableFPSError
        {
            get { return acceptableFPSError; }
            set { acceptableFPSError = value; }
        }

        /// <summary>
        /// Which sublist to look in for the update servers
        /// </summary>
        public int AutoUpdateServerSubList
        {
            get
            {
#if DEBUG
                return 1; // always development update server for MeGUI debug builds
#else
                return autoUpdateServerSubList;
#endif
            }
            set { autoUpdateServerSubList = value; }
        }

        /// <summary>
        /// Last update check
        /// </summary>
        public DateTime LastUpdateCheck
        {
            get { return lastUpdateCheck; }
            set { lastUpdateCheck = value; }
        }

        /// <summary>
        /// Last update server
        /// </summary>
        public string LastUpdateServer
        {
            get { return lastUpdateServer; }
            set { lastUpdateServer = value; }
        }

        /// <summary>
        /// Disable package after X days
        /// </summary>
        public int DisablePackageInterval
        {
            get { return disablePackageInterval; }
            set { disablePackageInterval = value; }
        }

        /// <summary>
        /// Check update server max every X hours
        /// </summary>
        public int UpdateCheckInterval
        {
            get 
            {
#if DEBUG
                return 0;
#else
                return updateCheckInterval; 
#endif
            }
            set { updateCheckInterval = value; }
        }

        /// <summary>
        /// List of servers to use for autoupdate
        /// </summary>
        public string[][] AutoUpdateServerLists
        {
            get { return autoUpdateServerLists; }
            set { autoUpdateServerLists = value; }
        }

        /// <summary>
        /// What to do after all encodes are finished
        /// </summary>
        public AfterEncoding AfterEncoding
        {
            get { return afterEncoding; }
            set { afterEncoding = value; }
        }

        /// <summary>
        /// Command to run after encoding is finished (only if AfterEncoding is RunCommand)
        /// </summary>
        public string AfterEncodingCommand
        {
            get { return afterEncodingCommand; }
            set { afterEncodingCommand = value; }
        }

        ///<summary>
        /// gets / sets whether megui puts the Video Preview Form "Alwyas on Top" or not
        /// </summary>
        public bool AlwaysOnTop
        {
            get { return alwaysOnTop; }
            set { alwaysOnTop = value; }
        }

        ///<summary>
        /// gets / sets whether megui add the Time Position or not to the Video Player
        /// </summary>
        public bool AddTimePosition
        {
            get { return addTimePosition; }
            set { addTimePosition = value; }
        }

        /// <summary>
        /// bool to decide whether to use external muxer for the x264 encoder
        /// </summary>
        public bool UseExternalMuxerX264
        {
            get { return bExternalMuxerX264; }
            set { bExternalMuxerX264 = value; }
        }

        /// <summary>
        /// gets / sets the default output directory
        /// </summary>
        public string DefaultOutputDir
        {
            get { return defaultOutputDir; }
            set { defaultOutputDir = value; }
        }

        /// <summary>
        /// gets / sets the temp directory for MP4 Muxer
        /// </summary>
        public string TempDirMP4
        {
            get 
            {
                if (String.IsNullOrEmpty(tempDirMP4) || Path.GetPathRoot(tempDirMP4).Equals(tempDirMP4, StringComparison.CurrentCultureIgnoreCase))
                    return String.Empty;
                return tempDirMP4;
            }
            set { tempDirMP4 = value; }
        }

        ///<summary>
        /// gets / sets whether megui backup files from updater or not
        /// </summary>
        public bool AlwaysBackUpFiles
        {
            get { return alwaysbackupfiles; }
            set { alwaysbackupfiles = value; }
        }

        /// <summary>
        /// folder containing the avisynth plugins
        /// </summary>
        public string AvisynthPluginsPath
        {
            get 
            {
                UpdateCacher.CheckPackage("avisynth_plugin");
                return Path.GetDirectoryName(avisynthplugins.Path); 
            }
        }

        /// <summary>
        /// folder containing local copies of update files
        /// </summary>
        public string MeGUIUpdateCache
        {
            get { return meguiupdatecache; }
        }

		/// <summary>
		/// should force film automatically be applies if the film percentage crosses the forceFilmTreshold?
		/// </summary>
		public bool AutoForceFilm
		{
			get {return autoForceFilm;}
			set {autoForceFilm = value;}
		}

        /// <summary>
        /// should the file autoloaded incrementally if VOB
        /// </summary>
        public bool AutoLoadDG
        {
            get { return bAutoLoadDG; }
            set { bAutoLoadDG = value; }
        }

		/// <summary>
		/// gets / sets whether megui automatically opens the preview window upon loading an avisynth script
		/// </summary>
		public bool AutoOpenScript
		{
			get {return autoOpenScript;}
			set {autoOpenScript = value;}
		}

		/// <summary>
		/// gets / sets whether the progress window should be opened for each job
		/// </summary>
		public bool OpenProgressWindow
		{
			get {return openProgressWindow;}
			set {openProgressWindow = value;}
		}

		/// <summary>
		/// the threshold to apply force film. If the film percentage is higher than this threshold,
		/// force film will be applied
		/// </summary>
        public decimal ForceFilmThreshold
		{
			get {return forceFilmThreshold;}
			set {forceFilmThreshold = value;}
		}

		/// <summary>
		/// <summary>
		/// first default language
		/// </summary>
		public string DefaultLanguage1
		{
			get {return defaultLanguage1;}
			set {defaultLanguage1 = value;}
		}

		/// <summary>
		/// second default language
		/// </summary>
		public string DefaultLanguage2
		{
			get {return defaultLanguage2;}
			set {defaultLanguage2 = value;}
		}

		/// <summary>
		/// sets / gets if the stats file is updated in the 3rd pass of 3 pass encoding
		/// </summary>
		public bool OverwriteStats
		{
			get {return overwriteStats;}
			set {overwriteStats = value;}
		}

		/// <summary>
		///  gets / sets if the output is video output of the 2nd pass is overwritten in the 3rd pass of 3 pass encoding
		/// </summary>
		public bool Keep2of3passOutput
		{
			get {return keep2of3passOutput;}
			set {keep2of3passOutput = value;}
		}

		/// <summary>
		/// sets the number of passes to be done in automated encoding mode
		/// </summary>
		public int NbPasses
		{
			get {return nbPasses;}
			set {nbPasses = value;}
		}

		/// <summary>
		/// sets / gets if intermediate files are to be deleted after encoding
		/// </summary>
		public bool DeleteIntermediateFiles
		{
			get {return deleteIntermediateFiles;}
			set {deleteIntermediateFiles = value;}
		}

		/// <summary>
		/// gets / sets if the output of an aborted job is to be deleted
		/// </summary>
		public bool DeleteAbortedOutput
		{
			get {return deleteAbortedOutput;}
			set {deleteAbortedOutput = value;}
		}

        public string VideoExtension
        {
            get {return videoExtension;}
            set {videoExtension = value;}
        }

        public string AudioExtension
        {
            get { return audioExtension; }
            set { audioExtension = value; }
        }

        /// <summary>
        /// gets / sets the settings for the update mode
        /// </summary>
        public UpdateMode UpdateMode
        {
            get
            {
                if (autoUpdate)
                    return MeGUI.UpdateMode.Manual;
                else
                    return MeGUI.UpdateMode.Disabled;
            }
        }

        public bool AutoUpdate
        {
            get { return autoUpdate; }
            set { autoUpdate = value; }
        }

        public DialogSettings DialogSettings
        {
            get { return dialogSettings; }
            set { dialogSettings = value; }
        }

        public SourceDetectorSettings SourceDetectorSettings
        {
            get { return sdSettings; }
            set { sdSettings = value; }
        }

        /// <summary>
        /// gets / sets the default settings for the autoencode window
        /// </summary>
        public AutoEncodeDefaultsSettings AedSettings
        {
            get { return aedSettings; }
            set { aedSettings = value; }
        }

        /// <summary>
        /// gets / sets the default settings for the Proxy
        /// </summary>
        public ProxyMode HttpProxyMode
        {
            get { return httpProxyMode; }
            set { httpProxyMode = value; }
        }

        /// <summary>
        /// gets / sets the default settings for the Proxy Adress
        /// </summary>
        public string HttpProxyAddress
        { 
            get { return httpproxyaddress;}
            set {httpproxyaddress = value;}
        }

        /// <summary>
        /// gets / sets the default settings for the Proxy Port
        /// </summary>
        public string HttpProxyPort
        { 
            get { return httpproxyport;}
            set {httpproxyport = value;}
        }

        /// <summary>
        /// gets / sets the default settings for the Proxy Uid
        /// </summary>
        public string HttpProxyUid
        { 
            get { return httpproxyuid;}
            set {httpproxyuid = value;}
        }

        /// <summary>
        /// gets / sets the default settings for the Proxy Password
        /// </summary>
        public string HttpProxyPwd
        {
            get { return httpproxypwd; }
            set { httpproxypwd = value; }
        }

        /// <summary>
        /// gets / sets the text to append to forced streams
        /// </summary>
        public string AppendToForcedStreams
        {
            get { return appendToForcedStreams; }
            set { appendToForcedStreams = value; }
        }

        public string MainAudioFormat
        {
            get { return strMainAudioFormat; }
            set { strMainAudioFormat = value; }
        }

        public string MainFileFormat
        {
            get { return strMainFileFormat; }
            set { strMainFileFormat = value; }
        }

        public string LastUsedOneClickFolder
        {
            get { return lastUsedOneClickFolder; }
            set { lastUsedOneClickFolder = value; }
        }

        public int MinComplexity
        {
            get { return minComplexity; }
            set { minComplexity = value; }
        }

        public int MaxComplexity
        {
            get { return maxComplexity; }
            set { maxComplexity = value; }
        }

        public int FFMSThreads
        {
            get { return ffmsThreads; }
            set { ffmsThreads = value; }
        }

        public bool UseITUValues
        {
            get { return bUseITU; }
            set { bUseITU = value; }
        }

        public int ChapterCreatorMinimumLength
        {
            get { return chapterCreatorMinimumLength; }
            set { chapterCreatorMinimumLength = value; }
        }

        public string ChapterCreatorSortString
        {
            get { return chapterCreatorSortString; }
            set { chapterCreatorSortString = value; }
        }

        public bool VobSubberExtractForced
        {
            get { return bVobSubberExtractForced; }
            set { bVobSubberExtractForced = value; }
        }

        public bool VobSubberSingleFileExport
        {
            get { return bVobSubberSingleFileExport; }
            set { bVobSubberSingleFileExport = value; }
        }

        public bool VobSubberKeepAll
        {
            get { return bVobSubberKeepAll; }
            set { bVobSubberKeepAll = value; }
        }

        public bool VobSubberShowAll
        {
            get { return bVobSubberShowAll; }
            set { bVobSubberShowAll = value; }
        }

        [XmlIgnore()]
        [MeGUI.core.plugins.interfaces.PropertyEqualityIgnoreAttribute()]
        public List<WorkerSettings> WorkerSettings
        {
            get { return arrWorkerSettings; }
            set { arrWorkerSettings = value; }
        }

        public WorkerSettings[] WorkerSettingsString
        {
            get { return arrWorkerSettings.ToArray(); }
            set { arrWorkerSettings = new List<WorkerSettings>(value); }
        }

        public int WorkerMaximumCount
        {
            get { return iWorkerMaximumCount; }
            set { iWorkerMaximumCount = value; }
        }

        [XmlIgnore()]
        [MeGUI.core.plugins.interfaces.PropertyEqualityIgnoreAttribute()]
        public List<WorkerPriority> WorkerPriority
        {
            get { return arrWorkerPriority; }
            set { arrWorkerPriority = value; }
        }

        public WorkerPriority[] WorkerPriorityString
        {
            get { return arrWorkerPriority.ToArray(); }
            set { arrWorkerPriority = new List<WorkerPriority>(value); }
        }

        public bool Input8Bit
        {
            get { return bInput8Bit; }
            set { bInput8Bit = value; }
        }

        public bool WorkerAutoStart
        {
            get { return workerAutoStart; }
            set { workerAutoStart = value; }
        }

        public bool WorkerAutoStartOnStartup
        {
            get { return workerAutoStartOnStartup; }
            set { workerAutoStartOnStartup = value; }
        }

        public bool WorkerRemoveJob
        {
            get { return workerRemoveJob; }
            set { workerRemoveJob = value; }
        }

        /// <summary>
        /// always use portable avisynth
        /// </summary>
        public bool AlwaysUsePortableAviSynth
        {
            get { return bAlwayUsePortableAviSynth; }
            set { bAlwayUsePortableAviSynth = value; }
        }

        /// <summary>
        /// filename and full path of the neroaacenc executable
        /// </summary>
        public string NeroAacEncPath
        {
            get 
            {
                if (!File.Exists(neroAacEncPath))
                    neroAacEncPath = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"tools\eac3to\neroAacEnc.exe");
                return neroAacEncPath; 
            }
            set
            {
                if (!File.Exists(value))
                    neroAacEncPath = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"tools\eac3to\neroAacEnc.exe");
                else
                    neroAacEncPath = value;
            }
        }

        /// <summary>
        /// filename and full path of the fdkaac executable
        /// </summary>
        public string FDKAacPath
        {
            get
            {
                if (!File.Exists(fdkAacPath))
                    fdkAacPath = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"tools\fdkaac\fdkaac.exe");
                return fdkAacPath;
            }
            set
            {
                if (!File.Exists(value))
                    fdkAacPath = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"tools\fdkaac\fdkaac.exe");
                else
                    fdkAacPath = value;
            }
        }


        /// <summary>
        /// filename and full path of the exhale executable
        /// </summary>
        public string ExhalePath
        {
            get
            {
                if (!File.Exists(exhalePath))
                    exhalePath = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"tools\exhale\exhale.exe");
                return exhalePath;
            }
            set
            {
                if (!File.Exists(value))
                    exhalePath = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"tools\exhale\exhale.exe");
                else
                    exhalePath = value;
            }
        }

        public bool UseDGIndexNV
        {
            get { return bUseDGIndexNV; }
            set { bUseDGIndexNV = value; }
        }

        public bool UseDGIndexIM
        {
            get { return bUseDGIndexIM; }
            set { bUseDGIndexIM = value; }
        }

        public bool UseNeroAacEnc
        {
            get { return bUseNeroAacEnc; }
            set { bUseNeroAacEnc = value; }
        }

        public bool UseFDKAac
        {
            get { return bUseFDKAac; }
            set { bUseFDKAac = value; }
        }

        public bool UseQAAC
        {
            get { return bUseQAAC; }
            set { bUseQAAC = value; }
        }

        [XmlIgnore]
        public Dictionary<string, string> RedistVersions
        {
            get { return oRedistVersions; }
            set { oRedistVersions = value; }
        }

        public ProgramSettings AviMuxGui
        {
            get { return avimuxgui; }
            set { avimuxgui = value; }
        }

        public ProgramSettings AviSynth
        {
            get { return avisynth; }
            set { avisynth = value; }
        }

        public ProgramSettings AviSynthPlugins
        {
            get { return avisynthplugins; }
            set { avisynthplugins = value; }
        }

        public ProgramSettings BeSplit
        {
            get { return besplit; }
            set { besplit = value; }
        }

        public ProgramSettings DGIndexIM
        {
            get { return dgindexim; }
            set { dgindexim = value; }
        }

        public ProgramSettings DGIndex
        {
            get { return dgindex; }
            set { dgindex = value; }
        }

        public ProgramSettings DGIndexNV
        {
            get { return dgindexnv; }
            set { dgindexnv = value; }
        }

        public ProgramSettings Eac3to
        {
            get { return eac3to; }
            set { eac3to = value; }
        }

        public ProgramSettings Fdkaac
        {
            get { return fdkaac; }
            set { fdkaac = value; }
        }

        /// <summary>
        /// program settings of ffmpeg
        /// </summary>
        public ProgramSettings FFmpeg
        {
            get { return ffmpeg; }
            set { ffmpeg = value; }
        }

        public ProgramSettings FFMS
        {
            get { return ffms; }
            set { ffms = value; }
        }

        public ProgramSettings Flac
        {
            get { return flac; }
            set { flac = value; }
        }

        public ProgramSettings Haali
        {
            get { return haali; }
            set { haali = value; }
        }

        public ProgramSettings Lame
        {
            get { return lame; }
            set { lame = value; }
        }

        public ProgramSettings LSMASH
        {
            get { return lsmash; }
            set { lsmash = value; }
        }

        public ProgramSettings BestSource
        {
            get { return bestsource; }
            set { bestsource = value; }
        }

        public ProgramSettings MediaInfo
        {
            get { return mediainfo; }
            set { mediainfo = value; }
        }

        public ProgramSettings MeGUI_Core
        {
            get { return megui_core; }
            set { megui_core = value; }
        }

        public ProgramSettings MeGUI_Libraries
        {
            get { return megui_libs; }
            set { megui_libs = value; }
        }

        public ProgramSettings MeGUI_Updater
        {
            get { return megui_updater; }
            set { megui_updater = value; }
        }

        /// <summary>
        /// program settings of mkvmerge
        /// </summary>
        public ProgramSettings MkvMerge
        {
            get { return mkvmerge; }
            set { mkvmerge = value; }
        }

        public ProgramSettings Mp4Box
        {
            get { return mp4box; }
            set { mp4box = value; }
        }

        public ProgramSettings NeroAacEnc
        {
            get { return neroaacenc; }
            set { neroaacenc = value; }
        }

        public ProgramSettings Exhale
        {
            get { return exhale; }
            set { exhale = value; }
        }

        public ProgramSettings OggEnc
        {
            get { return oggenc; }
            set { oggenc = value; }
        }

        public ProgramSettings Opus
        {
            get { return opus; }
            set { opus = value; }
        }

        public ProgramSettings PgcDemux
        {
            get { return pgcdemux; }
            set { pgcdemux = value; }
        }

        public ProgramSettings QAAC
        {
            get { return qaac; }
            set { qaac = value; }
        }

        public ProgramSettings Redist
        {
            get { return redist; }
            set { redist = value; }
        }

        public ProgramSettings TSMuxer
        {
            get { return tsmuxer; }
            set { tsmuxer = value; }
        }

        public ProgramSettings VobSubRipper
        {
            get { return vsrip; }
            set { vsrip = value; }
        }

        /// <summary>
        /// program settings of x264 8bit
        /// </summary>
        public ProgramSettings X264
        {
            get { return x264; }
            set { x264 = value; }
        }

        public ProgramSettings X265
        {
            get { return x265; }
            set { x265 = value; }
        }

        public ProgramSettings XviD
        {
            get { return xvid; }
            set { xvid = value; }
        }

        public ProgramSettings SvtAv1Psy
        {
            get { return svtav1psy; }
            set { svtav1psy = value; }
        }
#endregion

        private bool bPortableAviSynth;
        /// <summary>
        /// portable avisynth in use
        /// </summary>
        [XmlIgnore()]
        public bool PortableAviSynth
        {
            get { return bPortableAviSynth; }
            set { bPortableAviSynth = value; }
        }

        private bool bAviSynthPlus;
        /// <summary>
        /// avisynth+ in use
        /// </summary>
        [XmlIgnore()]
        public bool AviSynthPlus
        {
            get { return bAviSynthPlus; }
            set { bAviSynthPlus = value; }
        }

        #region Methods
        private void VerifySettings()
        {
            bool bReset = false;
            foreach (WorkerSettings oSettings in arrWorkerSettings)
            {
                if (oSettings.MaxCount < 1)
                    bReset = true;
                if (oSettings.JobTypes.Count < 1)
                    bReset = true;
            }
            if (bReset)
                ResetWorkerSettings();

            bReset = false;
            Dictionary<JobType, WorkerPriority> arrDict = new Dictionary<JobType, WorkerPriority>();
            foreach (WorkerPriority oPriority in arrWorkerPriority)
            {
                if (arrDict.ContainsKey(oPriority.JobType))
                {
                    bReset = true;
                    break;
                }
                arrDict.Add(oPriority.JobType, oPriority);
            }
            if (bReset || arrDict.Count != Enum.GetNames(typeof(JobType)).Length)
                ResetWorkerPriority();
        }

        public void ResetWorkerSettings()
        {
            arrWorkerSettings = new List<WorkerSettings>();
            arrWorkerSettings.Add(new WorkerSettings(1, new List<JobType>() { JobType.Audio }));
            arrWorkerSettings.Add(new WorkerSettings(1, new List<JobType>() { JobType.Audio, JobType.Demuxer, JobType.Indexer, JobType.Muxer }));
            arrWorkerSettings.Add(new WorkerSettings(1, new List<JobType>() { JobType.OneClick }));
            arrWorkerSettings.Add(new WorkerSettings(1, new List<JobType>() { JobType.Video }));
            iWorkerMaximumCount = 2;
        }

        public void ResetWorkerPriority()
        {
            arrWorkerPriority = new List<WorkerPriority>();
            arrWorkerPriority.Add(new WorkerPriority(JobType.Audio, WorkerPriorityType.BELOW_NORMAL, false));
            arrWorkerPriority.Add(new WorkerPriority(JobType.Demuxer, WorkerPriorityType.BELOW_NORMAL, false));
            arrWorkerPriority.Add(new WorkerPriority(JobType.Indexer, WorkerPriorityType.BELOW_NORMAL, false));
            arrWorkerPriority.Add(new WorkerPriority(JobType.Muxer, WorkerPriorityType.BELOW_NORMAL, false));
            arrWorkerPriority.Add(new WorkerPriority(JobType.OneClick, WorkerPriorityType.BELOW_NORMAL, false));
            arrWorkerPriority.Add(new WorkerPriority(JobType.Video, WorkerPriorityType.IDLE, true));
        }

        public int DPIRescale(int iOriginalValue)
        {
            return (int)Math.Round(iOriginalValue * dpiScaleFactor, 0);
        }

        public int DPIReverse(int iOriginalValue)
        {
            return (int)Math.Round(iOriginalValue / dLastDPIScaleFactor * dpiScaleFactor, 0);
        }

        public void DPIRescaleAll()
        {
            if (dpiScaleFactor == dLastDPIScaleFactor)
                return;

            if (dLastDPIScaleFactor == 0)
            {
                mainFormLocation = new Point(0, 0);
                mainFormSize = new Size(DPIRescale(713), DPIRescale(478));
                updateFormLocation = new Point(0, 0);
                updateFormSize = new Size(DPIRescale(780), DPIRescale(313));
                updateFormSplitter = DPIRescale(180);
                updateFormUpdateColumnWidth = DPIRescale(47);
                updateFormNameColumnWidth = DPIRescale(105);
                updateFormLocalVersionColumnWidth = DPIRescale(117);
                updateFormServerVersionColumnWidth = DPIRescale(117);
                updateFormServerArchitectureColumnWidth = DPIRescale(50);
                updateFormLocalDateColumnWidth = DPIRescale(70);
                updateFormServerDateColumnWidth = DPIRescale(70);
                updateFormLastUsedColumnWidth = DPIRescale(70);
                updateFormStatusColumnWidth = DPIRescale(111);
                jobColumnWidth = DPIRescale(40);
                inputColumnWidth = DPIRescale(105);
                outputColumnWidth = DPIRescale(105);
                codecColumnWidth = DPIRescale(79);
                modeColumnWidth = DPIRescale(79);
                statusColumnWidth = DPIRescale(65);
                startColumnWidth = DPIRescale(58);
                endColumnWidth = DPIRescale(58);
                fpsColumnWidth = DPIRescale(95);
            }
            else
            {
                mainFormLocation = new Point(0, 0);
                mainFormSize = new Size(DPIReverse(mainFormSize.Width), DPIReverse(mainFormSize.Height));
                updateFormLocation = new Point(0, 0);
                updateFormSize = new Size(DPIReverse(updateFormSize.Width), DPIReverse(updateFormSize.Height));
                updateFormSplitter = DPIReverse(updateFormSplitter);
                updateFormUpdateColumnWidth = DPIReverse(updateFormUpdateColumnWidth);
                updateFormNameColumnWidth = DPIReverse(updateFormNameColumnWidth);
                updateFormLocalVersionColumnWidth = DPIReverse(updateFormLocalVersionColumnWidth);
                updateFormServerVersionColumnWidth = DPIReverse(updateFormServerVersionColumnWidth);
                updateFormServerArchitectureColumnWidth = DPIReverse(updateFormServerArchitectureColumnWidth);
                updateFormLocalDateColumnWidth = DPIReverse(updateFormLocalDateColumnWidth);
                updateFormServerDateColumnWidth = DPIReverse(updateFormServerDateColumnWidth);
                updateFormLastUsedColumnWidth = DPIReverse(updateFormLastUsedColumnWidth);
                updateFormStatusColumnWidth = DPIReverse(updateFormStatusColumnWidth);
                jobColumnWidth = DPIReverse(jobColumnWidth);
                inputColumnWidth = DPIReverse(inputColumnWidth);
                outputColumnWidth = DPIReverse(outputColumnWidth);
                codecColumnWidth = DPIReverse(codecColumnWidth);
                modeColumnWidth = DPIReverse(modeColumnWidth);
                statusColumnWidth = DPIReverse(statusColumnWidth);
                startColumnWidth = DPIReverse(startColumnWidth);
                endColumnWidth = DPIReverse(endColumnWidth);
                endColumnWidth = DPIReverse(endColumnWidth);
            }
        }

        public bool IsDGIIndexerAvailable()
        {
            if (!bUseDGIndexNV)
                return false;

            // DGI is not available in a RDP connection
            if (SystemInformation.TerminalServerSession == true)
                return false;

            return true;
        }

        public bool IsDGMIndexerAvailable()
        {
            if (!bUseDGIndexIM)
                return false;

            // check if the license file is available
            if (!File.Exists(Path.Combine(Path.GetDirectoryName(dgindexim.Path), "license.txt")))
            {
                if (File.Exists(Path.Combine(Path.GetDirectoryName(dgindexnv.Path), "license.txt")))
                {
                    // license.txt available in the other indexer directory. copy it
                    Directory.CreateDirectory(Path.GetDirectoryName(dgindexim.Path));
                    File.Copy(Path.Combine(Path.GetDirectoryName(dgindexnv.Path), "license.txt"), Path.Combine(Path.GetDirectoryName(dgindexim.Path), "license.txt"));
                }
                else
                    return false;
            }

            return true;
        }

        public void InitializeProgramSettings()
        {
            // initialize program settings if required
            if (avimuxgui == null)
                avimuxgui = new ProgramSettings("avimux_gui");
            if (avisynth == null)
                avisynth = new ProgramSettings("avs");
            if (avisynthplugins == null)
                avisynthplugins = new ProgramSettings("avisynth_plugin");
            if (besplit == null)
                besplit = new ProgramSettings("besplit");
            if (dgindexim == null)
                dgindexim = new ProgramSettings("dgindexim");
            if (dgindex == null)
                dgindex = new ProgramSettings("dgindex");
            if (dgindexnv == null)
                dgindexnv = new ProgramSettings("dgindexnv");
            if (eac3to == null)
                eac3to = new ProgramSettings("eac3to");
            if (fdkaac == null)
                fdkaac = new ProgramSettings("fdkaac");
            if (ffmpeg == null)
                ffmpeg = new ProgramSettings("ffmpeg");
            if (ffms == null)
                ffms = new ProgramSettings("ffms");
            if (flac == null)
                flac = new ProgramSettings("flac");
            if (haali == null)
                haali = new ProgramSettings("haali");
            if (lame == null)
                lame = new ProgramSettings("lame");
            if (lsmash == null)
                lsmash = new ProgramSettings("lsmash");
            if (bestsource == null)
                bestsource = new ProgramSettings("bestsource");
            if (mediainfo == null)
                mediainfo = new ProgramSettings("mediainfo");
            if (megui_core == null)
                megui_core = new ProgramSettings("core");
            if (megui_help == null)
                megui_help = new ProgramSettings("data");
            if (megui_libs == null)
                megui_libs = new ProgramSettings("libs");
            if (megui_updater == null)
                megui_updater = new ProgramSettings("updater");
            if (mkvmerge == null)
                mkvmerge = new ProgramSettings("mkvmerge");
            if (mp4box == null)
                mp4box = new ProgramSettings("mp4box");
            if (neroaacenc == null)
                neroaacenc = new ProgramSettings("neroaacenc");
            if (exhale == null)
                exhale = new ProgramSettings("exhale");
            if (oggenc == null)
                oggenc = new ProgramSettings("oggenc2");
            if (opus == null)
                opus = new ProgramSettings("opus");
            if (pgcdemux == null)
                pgcdemux = new ProgramSettings("pgcdemux");
            if (qaac == null)
                qaac = new ProgramSettings("qaac");
            if (redist == null)
                redist = new ProgramSettings("redist");
            if (svtav1psy == null)
                svtav1psy = new ProgramSettings("svtav1psy");
            if (tsmuxer == null)
                tsmuxer = new ProgramSettings("tsmuxer");
            if (vsrip == null)
                vsrip = new ProgramSettings("vsrip");
            if (x264 == null)
                x264 = new ProgramSettings("x264");
            if (x265 == null)
                x265 = new ProgramSettings("x265");
            if (xvid == null)
                xvid = new ProgramSettings("xvid_encraw");

            // set default name, program paths & files
            avimuxgui.UpdateInformation("avimux_gui", "AVI-Mux GUI", Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"tools\avimux_gui\avimux_gui.exe"));
            avisynth.UpdateInformation("avs", "AviSynth portable", Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"tools\avs\AviSynth.dll"));
            avisynth.Files.Add(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"tools\avs\DevIL.dll"));
            avisynth.Files.Add(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"tools\avs\plugins\DirectShowSource.dll"));
            avisynth.Required = true;
            avisynthplugins.UpdateInformation("avisynth_plugin", "AviSynth plugins", Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"tools\avisynth_plugin\colormatrix.dll"));
            if (!bMeGUIx64)
            {
                avisynthplugins.Files.Add(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"tools\avisynth_plugin\AudioLimiter.dll"));
                avisynthplugins.Files.Add(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"tools\avisynth_plugin\bass.dll"));
                avisynthplugins.Files.Add(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"tools\avisynth_plugin\bass_aac.dll"));
                avisynthplugins.Files.Add(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"tools\avisynth_plugin\bass_ape.dll"));
                avisynthplugins.Files.Add(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"tools\avisynth_plugin\bass_cda.dll"));
                avisynthplugins.Files.Add(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"tools\avisynth_plugin\bass_flac.dll"));
                avisynthplugins.Files.Add(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"tools\avisynth_plugin\bass_mpc.dll"));
                avisynthplugins.Files.Add(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"tools\avisynth_plugin\bass_spx.dll"));
                avisynthplugins.Files.Add(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"tools\avisynth_plugin\bass_tta.dll"));
                avisynthplugins.Files.Add(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"tools\avisynth_plugin\bass_wma.dll"));
                avisynthplugins.Files.Add(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"tools\avisynth_plugin\bass_wv.dll"));
                avisynthplugins.Files.Add(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"tools\avisynth_plugin\bassaudio.dll"));
                avisynthplugins.Files.Add(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"tools\avisynth_plugin\convolution3dyv12.dll"));
                avisynthplugins.Files.Add(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"tools\avisynth_plugin\soxfilter.dll"));
                avisynthplugins.Files.Add(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"tools\avisynth_plugin\tomsmocomp.dll"));
            }
            avisynthplugins.Files.Add(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"tools\avisynth_plugin\decomb.dll"));
            avisynthplugins.Files.Add(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"tools\avisynth_plugin\eedi2.dll"));
            avisynthplugins.Files.Add(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"tools\avisynth_plugin\fluxsmooth.dll"));
            avisynthplugins.Files.Add(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"tools\avisynth_plugin\leakkerneldeint.dll"));
            avisynthplugins.Files.Add(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"tools\avisynth_plugin\nicaudio.dll"));
            avisynthplugins.Files.Add(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"tools\avisynth_plugin\tdeint.dll"));
            avisynthplugins.Files.Add(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"tools\avisynth_plugin\TimeStretch.dll"));
            avisynthplugins.Files.Add(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"tools\avisynth_plugin\tivtc.dll"));
            avisynthplugins.Files.Add(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"tools\avisynth_plugin\undot.dll"));
            avisynthplugins.Files.Add(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"tools\avisynth_plugin\vsfilter.dll"));
            avisynthplugins.Files.Add(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"tools\avisynth_plugin\yadifmod2.dll"));
            avisynthplugins.Required = true;
            besplit.UpdateInformation("besplit", "Besplit", Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"tools\besplit\besplit.exe"));
            dgindexim.UpdateInformation("dgindexim", "DGIndexIM", Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"tools\dgindexim\dgindexim.exe"));
            dgindexim.Files.Add(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"tools\dgindexim\DGDecodeIM.dll"));
            if (!bMeGUIx64)
                dgindexim.Files.Add(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"tools\dgindexim\libmfxsw32.dll"));
            else
                dgindexim.Files.Add(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"tools\dgindexim\libmfxsw64.dll"));
            if (!MainForm.Instance.Settings.UseDGIndexIM)
                UpdateCacher.CheckPackage("dgindexim", false, false);
            dgindexim.DoNotDeleteFilesOnUpdate.Add(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"tools\dgindexim\license.txt"));
            dgindex.UpdateInformation("dgindex", "DGIndex", Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"tools\dgindex\dgindex.exe"));
            dgindex.Files.Add(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"tools\dgindex\dgdecode.dll"));
            dgindexnv.UpdateInformation("dgindexnv", "DGIndexNV", Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"tools\dgindexnv\dgindexnv.exe"));
            dgindexnv.Files.Add(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"tools\dgindexnv\dgdecodenv.dll"));
            dgindexnv.DoNotDeleteFilesOnUpdate.Add(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"tools\dgindexnv\license.txt"));
            if (!MainForm.Instance.Settings.UseDGIndexNV)
                UpdateCacher.CheckPackage("dgindexnv", false, false);
            eac3to.UpdateInformation("eac3to", "eac3to", Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"tools\eac3to\eac3to.exe"));
            eac3to.DoNotDeleteFilesOnUpdate.Add(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"tools\eac3to\neroaacenc.exe"));
            fdkaac.UpdateInformation("fdkaac", "FDK-AAC", FDKAacPath);
            exhale.UpdateInformation("exhale", "Exhale", ExhalePath);
            if (!MainForm.Instance.Settings.UseFDKAac)
                UpdateCacher.CheckPackage("fdkaac", false, false);
            ffmpeg.UpdateInformation("ffmpeg", "FFmpeg", Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"tools\ffmpeg\ffmpeg.exe"));
            ffms.UpdateInformation("ffms", "FFMS", Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"tools\ffms\ffmsindex.exe"));
            ffms.Files.Add(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"tools\ffms\ffms2.dll"));
            flac.UpdateInformation("flac", "FLAC", Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"tools\flac\flac.exe"));
            haali.UpdateInformation("haali", "Haali Media Splitter", Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"tools\haali\splitter.ax"));
            haali.Files.Add(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"tools\haali\avss.dll"));
            haali.Files.Add(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"tools\haali\install.cmd"));
            lame.UpdateInformation("lame", "LAME", Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"tools\lame\lame.exe"));
            lsmash.UpdateInformation("lsmash", "L-SMASH Works", Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"tools\lsmash\LSMASHSource.dll"));
            bestsource.UpdateInformation("bestsource", "BestSource Avisynth Plugin", Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"tools\bestsource\BestSource.dll"));
            mediainfo.UpdateInformation("mediainfo", "MediaInfo", Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"MediaInfo.dll"));
            mediainfo.Required = true;
            megui_core.UpdateInformation("core", "MeGUI", Application.ExecutablePath);
            megui_core.Required = true;
            megui_help.UpdateInformation("data", "MeGUI Help", Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"Data\ContextHelp.xml"));
            megui_help.Required = true;
            megui_libs.UpdateInformation("libs", "MeGUI Libraries", Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"7z.dll"));
            megui_libs.Files.Add(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"AvisynthWrapper.dll"));
            megui_libs.Files.Add(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"ICSharpCode.SharpZipLib.dll"));
            megui_libs.Files.Add(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"SevenZipSharp.dll"));
            megui_libs.Required = true;
            megui_updater.UpdateInformation("updater", "MeGUI Updater", Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"update.exe"));
            megui_updater.Required = true;
            mkvmerge.UpdateInformation("mkvmerge", "mkvmerge", Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"tools\mkvmerge\mkvmerge.exe"));
            mkvmerge.Files.Add(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"tools\mkvmerge\mkvextract.exe"));
            mp4box.UpdateInformation("mp4box", "MP4Box", Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"tools\mp4box\mp4box.exe"));
            mp4box.Files.Add(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"tools\mp4box\mp4fpsmod\mp4fpsmod.exe"));
            neroaacenc.UpdateInformation("neroaacenc", "NeroAACEnc", NeroAacEncPath);
            if (!MainForm.Instance.Settings.UseNeroAacEnc)
                UpdateCacher.CheckPackage("neroaacenc", false, false);
            oggenc.UpdateInformation("oggenc2", "OggEnc2", Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"tools\oggenc2\oggenc2.exe"));
            opus.UpdateInformation("opus", "Opus", Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"tools\opus\opusenc.exe"));
            pgcdemux.UpdateInformation("pgcdemux", "PgcDemux", Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"tools\pgcdemux\pgcdemux.exe"));
            qaac.UpdateInformation("qaac", "QAAC", Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"tools\qaac\qaac.exe"));
            qaac.DoNotDeleteFoldersOnUpdate.Add(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"tools\qaac\QTfiles"));
            qaac.DoNotDeleteFoldersOnUpdate.Add(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"tools\qaac\QTfiles64"));
            if (!MainForm.Instance.Settings.UseQAAC)
                UpdateCacher.CheckPackage("qaac", false, false);
            redist.UpdateInformation("redist", "Runtime Files", Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"tools\redist\install.cmd"));
            redist.Files.Add(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"tools\redist\remove.cmd"));
            svtav1psy.UpdateInformation("svtav1psy", "SVT-AV1-PSY", Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"tools\svtav1psy\SvtAv1EncApp.exe"));
            tsmuxer.UpdateInformation("tsmuxer", "tsMuxeR", Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"tools\tsmuxer\tsmuxer.exe"));
            vsrip.UpdateInformation("vsrip", "VobSub Ripper", Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"tools\vsrip\vsrip.exe"));
            x264.UpdateInformation("x264", "x264", Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"tools\x264\x264.exe"));
            x265.UpdateInformation("x265", "x265", Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"tools\x265\avs4x26x.exe"));
            x265.Files.Add(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"tools\x265\x64\x265.exe"));
            if (!bMeGUIx64)
                x265.Files.Add(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"tools\x265\x86\x265.exe"));
            xvid.UpdateInformation("xvid_encraw", "Xvid", Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"tools\xvid_encraw\xvid_encraw.exe"));

            VerifySettings();
       }
#endregion
    }

    public enum AfterEncoding { DoNothing = 0, Shutdown = 1, RunCommand = 2, CloseMeGUI = 3 }
    public enum ProxyMode { None = 0, SystemProxy = 1, CustomProxy = 2, CustomProxyWithLogin = 3 }
    public enum UpdateMode
    {
        [EnumTitle("Automatic update")]
        Automatic = 0,
        [EnumTitle("Automatic update check only")]
        Manual = 1,
        [EnumTitle("Automatic update completly disabled")]
        Disabled = 2
    }
}