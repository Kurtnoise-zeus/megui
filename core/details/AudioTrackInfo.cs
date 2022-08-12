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

using System;

namespace MeGUI
{
    public class AudioTrackInfo : TrackInfo
    {
        private string nbChannels, samplingRate, channelPositions, codecProfile;
        private int aacFlag;
        private bool bHasCore;
        private AudioCodec _codec;
        private AudioType _type;
        private BitrateManagementMode _bitrateMode;

        public AudioTrackInfo() : this(null, null, 0)
        {
        }

        public AudioTrackInfo(string language, string codec, int trackID)
        {
            base.TrackType = TrackType.Audio;
            base.Language = language;
            base.Codec = codec;
            base.TrackID = trackID;
            this.aacFlag = -1;
            this.nbChannels = "unknown";
            this.samplingRate = "unknown";
            this.channelPositions = "unknown";
            this.bHasCore = false;
            this.codecProfile = string.Empty;
            this._bitrateMode = BitrateManagementMode.CBR;
            this._type = null;
            this._codec = AudioCodec.UNKNOWN;
        }

        public string TrackIDx
        {
            get { return ContainerType == "MPEG-TS" ? TrackID.ToString("x3") : TrackID.ToString("x"); }
            set { TrackID = Int32.Parse(value, System.Globalization.NumberStyles.HexNumber); }
        }

        public string DgIndexID
        {
            get { return ContainerType == "MPEG-TS" ? TrackIndex.ToString() : TrackIDx; }
        }

        public string NbChannels
        {
            get { return nbChannels; }
            set { nbChannels = value; }
        }

        public string ChannelPositions
        {
            get { return channelPositions; }
            set { channelPositions = value; }
        }

        public string SamplingRate
        {
            get { return samplingRate; }
            set { samplingRate = value; }
        }

        public int AACFlag
        {
            get { return aacFlag; }
            set { aacFlag = value; }
        }

        /// <summary>
        /// Returns true if the audio track has a core track
        /// </summary>
        public bool HasCore
        {
            get { return bHasCore; }
            set { bHasCore = value; }
        }

        /// <summary>
        /// The Codec String
        /// </summary>
        public string CodecProfile
        {
            get { return codecProfile; }
            set { codecProfile = value; }
        }

        /// <summary>
        /// AudioCodec of the track
        /// </summary>
        public AudioCodec AudioCodec
        {
            get { return _codec; }
            set { _codec = value; }
        }

        /// <summary>
        /// AudioType of the track
        /// </summary>
        public AudioType AudioType
        {
            get { return _type; }
            set { _type = value; }
        }

        /// <summary>
        /// BitrateMode of the track
        /// </summary>
        public BitrateManagementMode BitrateMode
        {
            get { return _bitrateMode; }
            set { _bitrateMode = value; }
        }

        public override string ToString()
        {
            string fullString = "[" + TrackIDx + "] - " + this.Codec;
            if (!string.IsNullOrEmpty(nbChannels))
                fullString += " - " + this.nbChannels;
            if (!string.IsNullOrEmpty(samplingRate))
                fullString += " / " + samplingRate;
            if (!string.IsNullOrEmpty(Language))
                fullString += " / " + Language;
            if (!string.IsNullOrEmpty(Name))
                fullString += " / " + Name;
            return fullString.Trim();
        }
    }
}