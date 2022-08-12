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
using System.Xml.Serialization;

using MeGUI.core.plugins.interfaces;

namespace MeGUI
{
	/// <summary>
	/// Contains basic codec settings, basically all the settings that are often used by codecs like bitrate, encoding mode, etc.
	/// </summary>
    [XmlInclude(typeof(x264Settings)), XmlInclude(typeof(xvidSettings)), XmlInclude(typeof(hfyuSettings)), XmlInclude(typeof(x265Settings))]
    public abstract class VideoCodecSettings : MeGUI.core.plugins.interfaces.GenericSettings
	{
        private string id;
        public string SettingsID { get { return id; } }

        public virtual void setAdjustedNbThreads(int nbThreads)
        {
            NbThreads = nbThreads;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as GenericSettings);
        }

        public virtual bool Equals(GenericSettings other)
        {
            return PropertyEqualityTester.AreEqual(this, other);
        }

        public override int GetHashCode()
        {
            // DO NOT CALL BASE.GETHASHCODE!
            return 0;
        }

        /// <summary>
        /// video encoding mode
        /// </summary>
        public enum VideoEncodingMode : int
        { 
            CBR = 0,
            CQ = 1,
            twopass1,
            twopass2,
            twopassAutomated,
            threepass1,
            threepass2,
            threepass3,
            threepassAutomated,
            quality
        };

        private int bitrateQuantizer, keyframeInterval, nbBframes, minQuantizer, maxQuantizer, maxNumberOfPasses, nbThreads;
		private bool v4mv, qpel, trellis;
		private decimal creditsQuantizer;
		private string logfile, customEncoderOptions, videoName;
        private VideoEncoderType vet;
        private VideoEncodingMode videoEncodyingType;

        public abstract bool UsesSAR
        {
            get;
        }

		public VideoCodecSettings(string id, VideoEncoderType vet)
		{
            this.id = id;
            this.vet = vet;
            logfile = ".stats";
            customEncoderOptions = "";
            videoName = "";
            nbThreads = 1;
		}
        public VideoCodec Codec
        {
            get { return EncoderType.VCodec; }
        }
        public VideoEncoderType EncoderType
        {
            get { return vet; }
        }
        public VideoEncodingMode VideoEncodingType
        {
            get { return videoEncodyingType; }
            set { videoEncodyingType = value; }
        }
		public int BitrateQuantizer
		{
			get { return bitrateQuantizer; }
			set { bitrateQuantizer = value; }
		}
		public int KeyframeInterval
		{
			get { return keyframeInterval; }
			set { keyframeInterval = value; }
		}
		public int NbBframes
		{
			get { return nbBframes; }
			set { nbBframes = value; }
		}
		public int MinQuantizer
		{
			get { return minQuantizer; }
			set { minQuantizer = value; }
		}
		public int MaxQuantizer
		{
			get { return maxQuantizer; }
			set { maxQuantizer = value; }
		}
		public bool V4MV
		{
			get { return v4mv; }
			set { v4mv = value; }
		}
		public bool QPel
		{
			get { return qpel; }
			set { qpel = value; }
		}
		public bool Trellis
		{
			get { return trellis; }
			set { trellis = value; }
		}
		public decimal CreditsQuantizer
		{
			get { return creditsQuantizer; }
			set { creditsQuantizer = value; }
		}
		/// <summary>
		/// gets / sets the logfile
		/// </summary>
		public string Logfile
		{
			get { return logfile; }
			set { logfile = value; }
		}
        /// <summary>
        /// gets / sets Video Tracks Name (used with the muxers)
        /// </summary>
        public string VideoName
        {
            get { return videoName; }
            set { videoName = value; }
        }
		/// <summary>
		/// gets / set custom commandline options for the encoder
		/// </summary>
		public string CustomEncoderOptions
		{
			get { return customEncoderOptions; }
			set { customEncoderOptions = value; }
		}
		/// <summary>
        ///  gets / sets the maximum number of passes that can be performed with the current codec
        /// </summary>
        public int MaxNumberOfPasses
        {
            get { return maxNumberOfPasses; }
            set { maxNumberOfPasses = value; }
        }
        /// <summary>
        /// gets / sets the number of encoder threads to be used
        /// </summary>
        public int NbThreads
        {
            get { return nbThreads; }
            set { nbThreads = value; }
        }
		/// <summary>
		/// generates a copy of this object
		/// </summary>
		/// <returns>the codec specific settings of this object</returns>
        object ICloneable.Clone()
        {
            return Clone();
        }

        GenericSettings GenericSettings.Clone()
        {
            return Clone();
        }
        
        public VideoCodecSettings Clone()
        {
            // This method is sutable for all known descendants!
            return this.MemberwiseClone() as VideoCodecSettings;
		}

        #region GenericSettings Members
        public virtual void FixFileNames(System.Collections.Generic.Dictionary<string, string> _) { }

        public virtual string[] RequiredFiles
        {
            get { return new string[0]; }
        }

        public virtual string[] RequiredProfiles
        {
            get { return new string[0]; }
        }

        #endregion
    }
	public enum ZONEMODE: int {Quantizer = 0, Weight};
	public class Zone
	{
		public int startFrame;
		public int endFrame;
		public ZONEMODE mode;
		public decimal modifier;
	}
}
