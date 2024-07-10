using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeGUI
{

    public enum ExhaleProfile
    {
        [EnumTitle("Automatic")]
        xHEAAC,
        [EnumTitle("xHE-AAC+eSBR")]
        xHEAACeSBR,
    }

    public class ExhaleSettings : AudioCodecSettings
    {
        public static readonly string ID = "Exhale";

        public static readonly object[] SupportedBitrates = new object[] { 12, 16, 20, 24, 28, 32, 36, 40, 44, 48 };
        public static readonly ExhaleProfile[] SupportedProfiles = new ExhaleProfile[] { ExhaleProfile.xHEAAC, ExhaleProfile.xHEAACeSBR};


        public ExhaleSettings() : base(ID, AudioCodec.AAC, AudioEncoderType.EXHALE, 48)
        {
            base.DownmixMode = ChannelMode.KeepOriginal;
            base.supportedBitrates = Array.ConvertAll<object, int>(SupportedBitrates, delegate (object o) { return (int)o; });
            profile = ExhaleProfile.xHEAAC;
            quality = 5;
        }

        private ExhaleProfile profile;
        public ExhaleProfile Profile
        {
            get { return profile; }
            set { profile = value; }
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

