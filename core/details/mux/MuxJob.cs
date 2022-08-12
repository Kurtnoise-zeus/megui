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
using System.Collections;

namespace MeGUI
{
	/// <summary>
	/// Summary description for MuxJob.
	/// </summary>
	public class MuxJob: Job
	{
		private string codec;
        private int nbOfBframes, bitrate;
        private ulong nbOfFrames;
		private double overhead;
		private MuxSettings settings;
		private MuxerType type;
        private ContainerType containerType;

        [System.Xml.Serialization.XmlIgnore()]
        public ContainerType ContainerType
        {
            get { return containerType; }
            set { containerType = value; }
        }

        public string ContainerTypeString
        {
            get { return ContainerType.ID; }
            set
            {
                foreach (ContainerType t in MainForm.Instance.MuxProvider.GetSupportedContainers())
                {
                    if (t.ID == value) { ContainerType = t; return; }
                }
                ContainerType = null;
            }
        }

        public override string CodecString
        {
            get { return type.ToString().ToLowerInvariant(); }
        }

        public override string EncodingMode
        {
            get { return "mux"; }
        }

		public MuxJob():base()
		{
			codec = "";
			nbOfBframes = 0;
			bitrate = 0;
			overhead = 4.3;
			type = MuxerType.MP4BOX;
            containerType = ContainerType.MP4;
			settings = new MuxSettings();
		}

		/// <summary>
		/// codec used for video, used for informational purposes (put in the log)
		/// </summary>
		public string Codec
		{
			get {return codec;}
			set {codec = value;}
		}

		/// <summary>
		/// number of b-frames in the video input, used for informational purposes
		/// </summary>
		public int NbOfBFrames
		{
			get {return nbOfBframes;}
			set {nbOfBframes = value;}
		}

		/// <summary>
		/// the number of frames the video has
		/// </summary>
		public ulong NbOfFrames
		{
			get {return nbOfFrames;}
			set {nbOfFrames = value;}
		}

		/// <summary>
		/// chosen video bitrate for the output, used for informational purposes
		/// </summary>
		public int Bitrate
		{
			get {return bitrate;}
			set {bitrate = value;}
		}

		/// <summary>
		/// projected mp4 muxing overhead for this job in bytes / frame
		/// </summary>
		public double Overhead
		{
			get {return overhead;}
			set {overhead = value;}
		}

		/// <summary>
		/// the settings for this job
		/// contains additional information like additional streams, framerate, etc.
		/// </summary>
		public MuxSettings Settings
		{
			get {return settings;}
			set {settings = value;}
		}

		/// <summary>
		/// gets / sets the type of mux job this is
		/// </summary>
		public MuxerType MuxType
		{
			get {return type;}
			set {type = value;}
		}
	}
}