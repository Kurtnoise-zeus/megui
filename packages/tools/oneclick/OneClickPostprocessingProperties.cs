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
using System.Xml.Serialization;

using MeGUI.core.util;

namespace MeGUI
{
	/// <summary>
	/// Summary description for OneClickPostprocessingProperties.
	/// </summary>
	public class OneClickPostprocessingProperties
	{
		private bool autoDeint, autoCrop, keepInputResolution, prerenderJob, 
            useChapterMarks, eac3toDemux, applyDelayCorrection;
        private int horizontalOutputResolution, _titleNumberToProcess;
        private FileSize? splitSize;
        private ContainerType container;
		private FileSize? outputSize;
		private Dar? ar, forcedar;
        private VideoCodecSettings videoSettings;
        private AviSynthSettings avsSettings;
		private double customAR;
        private string finalOutput, aviSynthScript, deviceType, inputFile, _timeStampFile,
            workingDirectory, _videoFileToMux, ifoFile, _intermediateMKVFile;
        private List<string> filesToDelete;
        private List<OneClickAudioTrack> _audioTracks;
        private List<OneClickStream> _subtitleTrack;
        private FileIndexerWindow.IndexType _indexType;
        private OneClickSettings _oneClickSettings;
        private List<OneClickFilesToProcess> _filesToProcess;
        private ChapterInfo chapterInfo;
        private List<string> _oAttachments;

		public OneClickPostprocessingProperties()
		{
            autoCrop = true;
            keepInputResolution = false;
            ar = null;
            forcedar = null;
            avsSettings = new AviSynthSettings();
			horizontalOutputResolution = 720;
			customAR = 1.0;
			container = MeGUI.ContainerType.MKV;
            outputSize = null;
			splitSize = null;
            prerenderJob = false;
            deviceType = null;
            useChapterMarks = false;
            filesToDelete = new List<string>();
            _audioTracks = new List<OneClickAudioTrack>();
            _subtitleTrack = new List<OneClickStream>();
            _videoFileToMux = null;
            _intermediateMKVFile = null;
            _oneClickSettings = null;
            _filesToProcess = new List<OneClickFilesToProcess>();
            _titleNumberToProcess = 1;
            eac3toDemux = false;
            ifoFile = string.Empty;
            applyDelayCorrection = false;
            chapterInfo = new ChapterInfo();
            _oAttachments = new List<string>();
		}

        public bool AutoDeinterlace
        {
            get { return autoDeint; }
            set { autoDeint = value; }
        }

        /// <summary>
        /// gets / sets the AutoCrop function
        /// </summary>
        public bool AutoCrop
        {
            get { return autoCrop; }
            set { autoCrop = value; }
        }

        /// <summary>
        /// gets / sets Keep Input Resolution
        /// </summary>
        public bool KeepInputResolution
        {
            get { return keepInputResolution; }
            set { keepInputResolution = value; }
        }

        /// <summary>
        /// gets / sets Prerender Job
        /// </summary>
        public bool PrerenderJob
        {
            get { return prerenderJob; }
            set { prerenderJob = value; }
        }

		/// <summary>
		/// gets / sets the horizontal output resolution the output should have
		/// </summary>
		public int HorizontalOutputResolution
		{
			get { return horizontalOutputResolution; }
			set { horizontalOutputResolution = value; }
		}

		/// <summary>
		/// gets / sets the container of the output
		/// </summary>
        [XmlIgnore()]
        public ContainerType Container
		{
			get { return container; }
			set { container = value; }
		}

        public string ContainerTypeString
        {
            get { return Container.ID; }
            set
            {
                foreach (ContainerType t in MainForm.Instance.MuxProvider.GetSupportedContainers())
                {
                    if (t.ID == value) { Container = t; return; }
                }
                Container = null;
            }
        }

        /// <summary>
		/// gets / sets the output size
		/// </summary>
		public FileSize? OutputSize
		{
			get { return outputSize; }
			set { outputSize = value; }
		}

		/// <summary>
		/// gets / sets the split size for the output
		/// </summary>
		public FileSize? Splitting
		{
			get { return splitSize; }
			set { splitSize = value; }
		}

		/// <summary>
		/// gets / sets the aspect ratio of the video input (if known)
		/// </summary>
		public Dar? DAR
		{
			get { return ar; }
			set { ar = value; }
		}

        /// <summary>
        /// gets / sets the forced aspect ratio of the video input
        /// </summary>
        public Dar? ForcedDAR
        {
            get { return forcedar; }
            set { forcedar = value; }
        }

        public AviSynthSettings AvsSettings
        {
            get { return avsSettings; }
            set { avsSettings = value; }
        }

		/// <summary>
		/// gets / sets the video codec settings used for video encoding
		/// </summary>
		public VideoCodecSettings VideoSettings
		{
			get { return videoSettings; }
			set { videoSettings = value; }
		}

		/// <summary>
		/// gets / sets a custom aspect ratio for the input
		/// (requires AR set to AspectRatio.Custom to be taken into account)
		/// </summary>
		public double CustomAR
		{
			get { return customAR; }
			set { customAR = value; }
		}

		/// <summary>
		/// gets / sets the chapter file for the output
		/// </summary>
		public ChapterInfo ChapterInfo
		{
			get { return chapterInfo; }
			set { chapterInfo = value; }
		}

		/// <summary>
		/// gets / sets the path and name of the final output file
		/// </summary>
		public string FinalOutput
		{
			get { return finalOutput; }
			set { finalOutput = value; }
		}

        /// <summary>
        /// gets / sets the path and name of the video input file
        /// </summary>
        public string VideoInput
        {
            get { return inputFile; }
            set { inputFile = value; }
        }

        /// <summary>
        /// gets / sets the path and name of the video input file
        /// </summary>
        public string IFOInput
        {
            get { return ifoFile; }
            set { ifoFile = value; }
        }

        /// <summary>
        /// gets / sets the working directory
        /// </summary>
        public string WorkingDirectory
        {
            get { return workingDirectory; }
            set { workingDirectory = value; }
        }

		/// <summary>
		/// gets / sets the path and name of the AviSynth script created from the dgindex project
		/// this is filled in during postprocessing
		/// </summary>
		public string AviSynthScript
		{
			get { return aviSynthScript; }
			set { aviSynthScript = value; }
		}

        /// <summary>
        /// gets / sets the device output type
        /// </summary>
        public string DeviceOutputType
        {
            get { return deviceType; }
            set { deviceType = value; }
        }

        public bool UseChaptersMarks
        {
            get { return useChapterMarks; }
            set { useChapterMarks = value; }
        }

        /// <summary>
        /// gets / sets if the input is demuxed with eac3to
        /// </summary>
        public bool Eac3toDemux
        {
            get { return eac3toDemux; }
            set { eac3toDemux = value; }
        }

        /// <summary>
        /// gets / sets if a delay correction must be applied
        /// </summary>
        public bool ApplyDelayCorrection
        {
            get { return applyDelayCorrection; }
            set { applyDelayCorrection = value; }
        }

        public List<string> FilesToDelete
        {
            get { return filesToDelete; }
            set { filesToDelete = value; }
        }

        public List<OneClickAudioTrack> AudioTracks
        {
            get { return _audioTracks; }
            set { _audioTracks = value; }
        }

        public List<OneClickStream> SubtitleTracks
        {
            get { return _subtitleTrack; }
            set { _subtitleTrack = value; }
        }

        /// <summary>
        /// gets / sets the video file for mux only
        /// </summary>
        public string VideoFileToMux
        {
            get { return _videoFileToMux; }
            set { _videoFileToMux = value; }
        }

        /// <summary>
        /// gets / sets the intermediate mkv file
        /// </summary>
        public string IntermediateMKVFile
        {
            get { return _intermediateMKVFile; }
            set { _intermediateMKVFile = value; }
        }

        /// <summary>
        /// gets / sets the file index type
        /// </summary>
        public FileIndexerWindow.IndexType IndexType
        {
            get { return _indexType; }
            set { _indexType = value; }
        }

        /// <summary>
        /// gets / sets files which need to be processed
        /// </summary>
        public List<OneClickFilesToProcess> FilesToProcess
        {
            get { return _filesToProcess; }
            set { _filesToProcess = value; }
        }

        /// <summary>
        /// gets / sets attachments which need to be processed
        /// </summary>
        public List<string> Attachments
        {
            get { return _oAttachments; }
            set { _oAttachments = value; }
        }

        /// <summary>
        /// gets / sets timestamp file which need to be processed
        /// </summary>
        public string TimeStampFile
        {
            get { return _timeStampFile; }
            set { _timeStampFile = value; }
        }

        /// <summary>
        /// gets / sets settings for the FilesToProcess
        /// </summary>
        public OneClickSettings OneClickSetting
        {
            get { return _oneClickSettings; }
            set { _oneClickSettings = value; }
        }

        /// <summary>
        /// gets / sets settings for the TitleNumberToProcess
        /// </summary>
        public int TitleNumberToProcess
        {
            get { return _titleNumberToProcess; }
            set { _titleNumberToProcess = value; }
        }
	}
}