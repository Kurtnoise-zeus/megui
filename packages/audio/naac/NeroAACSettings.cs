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

namespace MeGUI
{
	/// <summary>
	/// this class contains all the settings for the Nero AAC encoder
	/// </summary>
    public enum AacProfile
    {
        [EnumTitle("Automatic")]
        Auto,
        [EnumTitle("HE-AAC+PS")]
        PS,
        [EnumTitle("HE-AAC")]
        HE,
        [EnumTitle("AAC-LC")]
        LC,
        [EnumTitle("High")]
        HIGH
    }

    public class NeroAACSettings : AudioCodecSettings
	{
        public static readonly string ID = "Nero AAC";

        public static readonly AacProfile[] SupportedProfiles = new AacProfile[] { AacProfile.Auto, AacProfile.PS, AacProfile.HE, AacProfile.LC };

		public NeroAACSettings() 
            : base(ID, AudioCodec.AAC, AudioEncoderType.NAAC, 0, BitrateManagementMode.VBR)
		{
            Quality = 0.5M;
            Profile = AacProfile.Auto;
		}

        private AacProfile profile;
        public AacProfile Profile
        {
            get { return profile; }
            set { profile = value; }
        }

        private decimal quality;
        public Decimal Quality
        {
            get { return quality; }
            set { quality = value; }
        }
	}
}
