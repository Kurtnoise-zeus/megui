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
    public enum FdkAACProfile
    {
        [EnumTitle("MPEG-4 LC-AAC")]
        M4LC,
        [EnumTitle("MPEG-4 HE-AAC")]
        M4HE,
        [EnumTitle("MPEG-4 HE-AAC v2")]
        M4HE2,
        [EnumTitle("MPEG-4 AAC LD")]
        M4LD,
        [EnumTitle("MPEG-4 AAC ELD")]
        M4ELD,
        [EnumTitle("MPEG-2 LC-AAC")]
        M2LC,
        [EnumTitle("MPEG-2 HE-AAC")]
        M2HE,
        [EnumTitle("MPEG-2 HE-AAC v2")]
        M2HE2
    }

    public enum FdkAACMode
    {
        [EnumTitle("CBR")]
        CBR,
        [EnumTitle("VBR")]
        VBR
    }

    public class FDKAACSettings : AudioCodecSettings
    {
        public static string ID = "FDK-AAC";

        public static readonly object[] SupportedBitrates = new object[] { 64, 128, 160, 192, 224, 256, 288, 320, 352, 384, 448, 512, 576, 640};
        public static readonly FdkAACProfile[] SupportedProfiles = new FdkAACProfile[] { FdkAACProfile.M4LC, FdkAACProfile.M4HE, FdkAACProfile.M4HE2, FdkAACProfile.M4LD, FdkAACProfile.M4ELD, FdkAACProfile.M2LC, FdkAACProfile.M2HE, FdkAACProfile.M2HE2 };
        public static readonly FdkAACMode[] SupportedModes = new FdkAACMode[] { FdkAACMode.CBR, FdkAACMode.VBR };


        public FDKAACSettings()
            : base(ID, AudioCodec.AAC, AudioEncoderType.FDKAAC, 128, BitrateManagementMode.CBR)
        {
            base.supportedBitrates = Array.ConvertAll<object, int>(SupportedBitrates, delegate(object o) { return (int)o; });
            mode = FdkAACMode.CBR;
            profile = FdkAACProfile.M4LC;
            quality = 3;
        }

        private FdkAACProfile profile;
        public FdkAACProfile Profile
        {
            get { return profile; }
            set { profile = value; }
        }

        private FdkAACMode mode;
        public FdkAACMode Mode
        {
            get { return mode; }
            set { mode = value; }
        }

        private int quality;
        public int Quality
        {
            get { return quality; }
            set { quality = value; }
        }

        public override BitrateManagementMode BitrateMode
        {
            get
            {
                return BitrateManagementMode.CBR;
            }
            set
            {
                // Do Nothing
            }
        }
    }
}
