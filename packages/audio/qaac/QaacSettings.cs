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
    public enum QaacProfile
    {
        [EnumTitle("LC-AAC")]
        LC,
        [EnumTitle("HE-AAC")]
        HE,
        [EnumTitle("ALAC")]
        ALAC
    }

    public enum QaacMode
    {
        [EnumTitle("True VBR")]
        TVBR,
        [EnumTitle("Constrained VBR")]
        CVBR,
        [EnumTitle("ABR")]
        ABR,
        [EnumTitle("CBR")]
        CBR
    }

    public class QaacSettings : AudioCodecSettings
	{
        public static readonly string ID = "QAAC";

        public static readonly QaacProfile[] SupportedProfiles = new QaacProfile[] { QaacProfile.LC, QaacProfile.HE, QaacProfile.ALAC };
        public static readonly QaacMode[]    SupportedModes    = new QaacMode[] { QaacMode.TVBR, QaacMode.CVBR, QaacMode.ABR, QaacMode.CBR };

		public QaacSettings() : base(ID, AudioCodec.AAC, AudioEncoderType.QAAC, 0, BitrateManagementMode.VBR)
		{
            Quality = 91;
            Mode = QaacMode.TVBR;
            Profile = QaacProfile.LC;
            noDelay = true;
		}

        private QaacProfile profile;
        public QaacProfile Profile
        {
            get { return profile; }
            set { profile = value; }
        }

        private QaacMode mode;
        public QaacMode Mode
        {
            get { return mode; }
            set { mode = value; }
        }

        private Int16 quality;
        public Int16 Quality
        {
            get { return quality; }
            set { quality = value; }
        }

        private bool noDelay;
        public bool NoDelay
        {
            get { return noDelay; }
            set { noDelay = value; }
        }
	}
}