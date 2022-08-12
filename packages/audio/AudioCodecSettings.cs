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

using MeGUI.core.plugins.interfaces;

namespace MeGUI
{
	/// <summary>
	/// Summary description for AudioCodecSettings.
	/// </summary>
	/// 
	public enum BitrateManagementMode {CBR, VBR, ABR};
	
    public enum ChannelMode
	{
	    [EnumTitle("Keep Original Channels")]
	    KeepOriginal, 
	    [EnumTitle("Convert to Mono")]
	    ConvertToMono,
        [EnumTitle("Downmix multichannel to 5.1")]
        Downmix51,
        [EnumTitle("Downmix multichannel to Stereo")]
	    StereoDownmix,
	    [EnumTitle("Downmix multichannel to Dolby Pro Logic")]
	    DPLDownmix,
        [EnumTitle("Downmix multichannel to Dolby Pro Logic II")]
        DPLIIDownmix,
        [EnumTitle("Upmix 2 to 5.1 via SuperEQ (slow)")]
	    Upmix,
        [EnumTitle("Upmix 2 to 5.1 via Sox equalizer adjustments")]
        UpmixUsingSoxEq,
        [EnumTitle("Upmix 2 to 5.1 with center channel dialog")]
        UpmixWithCenterChannelDialog
	};

    public enum AudioDecodingEngine
    {
        NicAudio,
        FFAudioSource,
        DirectShow,
        BassAudio,
        LWLibavAudioSource
    };

    public enum SampleRateMode
    {
        [EnumTitle("Keep Original Sample Rate")]
        KeepOriginal,
        [EnumTitle("Change to   8000 Hz")]
        ConvertTo08000,
        [EnumTitle("Change to 11025 Hz")]
        ConvertTo11025,
        [EnumTitle("Change to 22050 Hz")]
        ConvertTo22050,
        [EnumTitle("Change to 32000 Hz")]
        ConvertTo32000,
        [EnumTitle("Change to 44100 Hz")]
        ConvertTo44100,
        [EnumTitle("Change to 48000 Hz")]
        ConvertTo48000,
        [EnumTitle("Change to 96000 Hz")]
        ConvertTo96000
    };

    public enum TimeModificationMode
    {
        [EnumTitle("Keep Original")]
        KeepOriginal,
        [EnumTitle("Speed-up (23.976 to 25)")]
        SpeedUp23976To25,
        [EnumTitle("Slow-down (25 to 23.976)")]
        SlowDown25To23976,
        [EnumTitle("Speed-up (24 to 25)")]
        SpeedUp24To25,
        [EnumTitle("Slow-down (25 to 24)")]
        SlowDown25To24,
        [EnumTitle("Speed-up (23.976 to 25) with pitch correction")]
        SpeedUp23976To25WithCorrection,
        [EnumTitle("Slow-down (25 to 23.976) with pitch correction")]
        SlowDown25To23976WithCorrection,
        [EnumTitle("Speed-up (24 to 25) with pitch correction")]
        SpeedUp24To25WithCorrection,
        [EnumTitle("Slow-down (25 to 24) with pitch correction")]
        SlowDown25To24WithCorrection
    };
     
    [
        XmlInclude(typeof(MP2Settings)),
        XmlInclude(typeof(AC3Settings)), 
        XmlInclude(typeof(NeroAACSettings)), 
        XmlInclude(typeof(MP3Settings)), 
        XmlInclude(typeof(OggVorbisSettings)),
        XmlInclude(typeof(FlacSettings)),
        XmlInclude(typeof(QaacSettings)),
        XmlInclude(typeof(OpusSettings)),
        XmlInclude(typeof(FDKAACSettings)),
        XmlInclude(typeof(FFAACSettings))
    ]
	public abstract class AudioCodecSettings : MeGUI.core.plugins.interfaces.GenericSettings
	{
        protected int[] supportedBitrates;
        private readonly string id;
        public string SettingsID { get { return id; } }

        public virtual void FixFileNames(Dictionary<string, string> _) {}
		private ChannelMode downmixMode;
		private BitrateManagementMode bitrateMode;
        private SampleRateMode sampleRate;
        private TimeModificationMode timeModification;
		private int bitrate;
        private int normalize;
        private bool autoGain;
        private bool applyDRC;
        private AudioCodec audioCodec;
        private AudioEncoderType audioEncoderType;
        private AudioDecodingEngine preferredDecoder;
        private string customEncoderOptions;

        [XmlIgnore()]
        public AudioCodec Codec
        {
            get { return audioCodec; }
            set { audioCodec = value; }
        }
        
        [XmlIgnore]
        public AudioEncoderType EncoderType
        {
            get { return audioEncoderType; }
            set { audioEncoderType = value; }
        }

		public AudioCodecSettings(string id, AudioCodec codec, AudioEncoderType encoder, int bitrate)
            : this(id, codec, encoder, bitrate, BitrateManagementMode.CBR)
        {

        }

		public AudioCodecSettings(string id, AudioCodec codec, AudioEncoderType encoder, int bitrate, BitrateManagementMode mode)
		{
            this.id = id;
			audioCodec = codec;
			audioEncoderType = encoder;
			downmixMode = ChannelMode.KeepOriginal;
			bitrateMode = mode;
			this.bitrate = bitrate;
			autoGain = false;
            applyDRC = false;
            normalize = 100;
            preferredDecoder = AudioDecodingEngine.LWLibavAudioSource;
            timeModification = TimeModificationMode.KeepOriginal;
            sampleRate = SampleRateMode.KeepOriginal;
            customEncoderOptions = string.Empty;
		}

        [XmlIgnore()]
        public AudioDecodingEngine PreferredDecoder
        {
            get { return preferredDecoder; }
            set { preferredDecoder = value; }
        }

        // for profile import/export in case the enum changes
        public string PreferredDecoderString
        {
            get { return preferredDecoder.ToString(); }
            set 
            {
                if (value.Equals("FFAudioSource"))
                    preferredDecoder = AudioDecodingEngine.FFAudioSource;
                else if (value.Equals("DirectShow"))
                    preferredDecoder = AudioDecodingEngine.DirectShow;
                else if (value.Equals("BassAudio"))
                    preferredDecoder = AudioDecodingEngine.BassAudio;
                else if (value.Equals("NicAudio"))
                    preferredDecoder = AudioDecodingEngine.NicAudio;
                else if (value.Equals("LWLibavAudioSource"))
                    preferredDecoder = AudioDecodingEngine.LWLibavAudioSource;
            }
        }

		public ChannelMode DownmixMode
		{
			get { return downmixMode; }
			set { downmixMode = value; }
		}

		public virtual BitrateManagementMode BitrateMode
		{
			get { return bitrateMode; }
			set { bitrateMode = value; }
		}

		public virtual int Bitrate
		{
			get { return NormalizeVar(bitrate, supportedBitrates); }
			set { bitrate = value; }
		}

		public bool AutoGain
		{
			get { return autoGain; }
			set { autoGain = value; }
		}

        public string SampleRateType
        {
            get { return "deprecated"; }
            set
            {
                if (value.Equals("deprecated"))
                    return;

                sampleRate = SampleRateMode.KeepOriginal;
                timeModification = TimeModificationMode.KeepOriginal;

                if (value.Equals("1"))
                    sampleRate = SampleRateMode.ConvertTo08000;
                else if (value.Equals("2"))
                    sampleRate = SampleRateMode.ConvertTo11025;
                else if (value.Equals("3"))
                    sampleRate = SampleRateMode.ConvertTo22050;
                else if (value.Equals("4"))
                    sampleRate = SampleRateMode.ConvertTo32000;
                else if (value.Equals("5"))
                    sampleRate = SampleRateMode.ConvertTo44100;
                else if (value.Equals("6"))
                    sampleRate = SampleRateMode.ConvertTo48000;
                else if (value.Equals("7"))
                    timeModification = TimeModificationMode.SpeedUp23976To25;
                else if (value.Equals("8"))
                    timeModification = TimeModificationMode.SlowDown25To23976;
                else if (value.Equals("9"))
                    timeModification = TimeModificationMode.SpeedUp24To25;
                else if (value.Equals("10"))
                    timeModification = TimeModificationMode.SlowDown25To24;
            }
        }

        public SampleRateMode SampleRate
        {
            get { return sampleRate; }
            set { sampleRate = value; }
        }

        public TimeModificationMode TimeModification
        {
            get { return timeModification; }
            set { timeModification = value; }
        }

        public bool ApplyDRC
        {
            get { return applyDRC; }
            set { applyDRC = value; }
        }

        public int Normalize
        {
            get { return normalize; }
            set { normalize = value; }
        }

        object ICloneable.Clone()
        {
            return Clone();
        }

        GenericSettings GenericSettings.Clone()
        {
            return Clone();
        }

        public AudioCodecSettings Clone()
        {
            // This method is sutable for all known descendants!
            return this.MemberwiseClone() as AudioCodecSettings;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as GenericSettings);
        }

        public bool Equals(GenericSettings other)
        {
            // This works for all known descendants
            return other == null ? false : PropertyEqualityTester.AreEqual(this, other);
        }

        internal virtual int NormalizeVar(int bitrate, int[] supportedBitrates)
        {
            if (supportedBitrates == null)
                return bitrate;

            int closest = bitrate;
            int d = int.MaxValue;

            foreach (int i in supportedBitrates)
            {
                int d1 = Math.Abs(i - bitrate);
                if (d1 <= d)
                {
                    closest = i;
                    d = d1;
                }
            }
            return closest;
        }

        public override int GetHashCode()
        {
            // DO NOT CALL base.GetHashCode();
            return 0;
        }

        public string[] RequiredFiles
        {
            get { return new string[0]; }
        }

        public string[] RequiredProfiles
        {
            get { return new string[0]; }
        }

        /// <summary>
        /// gets / set custom commandline options for the encoder
        /// </summary>
        public string CustomEncoderOptions
        {
            get { return customEncoderOptions; }
            set { customEncoderOptions = value; }
        }
    }
}