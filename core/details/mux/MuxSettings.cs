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

using MeGUI.core.details;
using MeGUI.core.util;

namespace MeGUI
{
	/// <summary>
	/// Summary description for MuxSettings.
	/// </summary>
	public class MuxSettings
	{
		private List<MuxStream> audioStreams, subtitleStreams;
		private decimal? framerate;
		private string videoName, timeStampFile;
        private string muxedInput, videoInput, muxedOutput, deviceType;
        private bool muxAll;
        private Dar? dar;
        private ChapterInfo chapterInfo;
        private FileSize? splitSize;
        private List<string> attachments;

		public MuxSettings()
		{
			audioStreams = new List<MuxStream>();
			subtitleStreams = new List<MuxStream>();
            chapterInfo = new ChapterInfo();
            attachments = new List<string>();
            framerate = null;
            muxedInput = "";
            videoName = "";
            videoInput = "";
            muxedOutput = "";
            deviceType = "";
			splitSize = null;
            muxAll = false;
            timeStampFile = String.Empty;
		}

        public string MuxedInput
        {
            get { return muxedInput; }
            set { muxedInput = value; }
        }

        public string MuxedOutput
        {
            get { return muxedOutput; }
            set { muxedOutput = value; }
        }

        public string VideoInput
        {
            get { return videoInput; }
            set { videoInput = value; }
        }

		/// <summary>
		/// Array of all the audio streams to be muxed
		/// </summary>
		public List<MuxStream> AudioStreams
		{
			get {return audioStreams;}
			set {audioStreams = value;}
		}

		/// <summary>
		/// Array of subtitle tracks to be muxed
		/// </summary>
		public List<MuxStream> SubtitleStreams
		{
			get {return subtitleStreams;}
			set {subtitleStreams = value;}
		}

		/// <summary>
		/// framerate of the video
		/// </summary>
		public decimal? Framerate
		{
			get {return framerate;}
			set {framerate = value;}
		}

		/// <summary>
		/// the file containing the chapter information
		/// </summary>
		public ChapterInfo ChapterInfo
		{
			get {return chapterInfo;}
			set {chapterInfo = value;}
		}

        /// <summary>
        /// Array of all the attachments to be muxed
        /// </summary>
        public List<string> Attachments
        {
            get { return attachments; }
            set { attachments = value; }
        }

        /// <summary>
        /// file size at which the output file is to be split
        /// </summary>
        public FileSize? SplitSize
		{
			get {return splitSize;}
			set {splitSize = value;}
		}

        public Dar? DAR
        {
            get { return dar; }
            set { dar = value; }
        }

        public string DeviceType
        {
            get { return deviceType; }
            set { deviceType = value; }
        }
		
        /// <summary>
        /// gets / sets the name of the video track
        /// </summary>
        public string VideoName
        {
           get { return videoName; }
           set { videoName = value; }
        }

        /// <summary>
        /// gets / sets if the complete input file must be muxed
        /// </summary>
        public bool MuxAll
        {
            get { return muxAll; }
            set { muxAll = value; }
        }

        /// <summary>
        /// gets / sets the timestamp file
        /// </summary>
        public string TimeStampFile
        {
            get { return timeStampFile; }
            set { timeStampFile = value; }
        }
    }
}