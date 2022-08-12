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

using MeGUI.core.util;

namespace MeGUI
{
	/// <summary>
	/// Summary description for VideoJob.
	/// </summary>
	public class VideoJob : Job
	{
		private VideoCodecSettings settings;
        public BitrateCalculationInfo BitrateCalculationInfo;

		public VideoJob():base()
		{
		}

        private Zone[] zones = new Zone[] { };
        /// <summary>
        /// gets / sets the zones
        /// </summary>
        public Zone[] Zones
        {
            get { return zones; }
            set { zones = value; }
        }

        public VideoJob(string input, string output, VideoCodecSettings settings, Dar? dar, Zone[] zones)
            : base(input, output)
        {
            Settings = settings;
            DAR = dar;
            Zones = zones;
        }

        private Dar? dar;
        public Dar? DAR
        {
            get { return dar; }
            set { dar = value; }
        }

		/// <summary>
		/// the codec settings for this job
		/// </summary>
		public VideoCodecSettings Settings
		{
			get {return settings;}
			set {settings = value;}
		}
		/// <summary>
		/// codec used as presentable string
		/// </summary>
		public override string CodecString
		{
			get
			{
                return settings.SettingsID;
			}
		}
		/// <summary>
		/// returns the encoding mode as a human readable string
		/// (this string is placed in the appropriate column in the queue)
		/// </summary>
		public override string EncodingMode
		{
			get
			{
                return "video";
			}
		}
    }
}