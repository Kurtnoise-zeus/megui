using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeGUI
{

    public enum FFAACProfile
    {
        [EnumTitle("MPEG-4 LC-AAC")]
        M4LC,
        [EnumTitle("MPEG-2 PNS-AAC")]
        M4PNS,
        [EnumTitle("MPEG-4 LTP-AAC")]
        M4LTP,
        [EnumTitle("MPEG-4 AAC MAIN")]
        M4MAIN,
    }

    public enum FFAACMode
    {
        [EnumTitle("CBR")]
        CBR,
        [EnumTitle("VBR")]
        VBR
    }

    public class FFAACSettings : AudioCodecSettings
    {
        public static readonly string ID = "FFmpeg AAC";

        public static readonly object[] SupportedBitrates = new object[] { 32, 64, 128, 160, 192, 224, 256, 320, 528 };
        public static readonly FFAACProfile[] SupportedProfiles = new FFAACProfile[] { FFAACProfile.M4LC, FFAACProfile.M4PNS, FFAACProfile.M4LTP, FFAACProfile.M4MAIN};
        public static readonly FFAACMode[] SupportedModes = new FFAACMode[] { FFAACMode.CBR, FFAACMode.VBR };


        public FFAACSettings() : base(ID, AudioCodec.AAC, AudioEncoderType.FFAAC, 128)
        {
            base.DownmixMode = ChannelMode.KeepOriginal;
            base.supportedBitrates = Array.ConvertAll<object, int>(SupportedBitrates, delegate (object o) { return (int)o; });
            mode = FFAACMode.CBR;
            profile = FFAACProfile.M4LC;
            quality = 60;
        }

        private FFAACProfile profile;
        public FFAACProfile Profile
        {
            get { return profile; }
            set { profile = value; }
        }

        private FFAACMode mode;
        public FFAACMode Mode
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

