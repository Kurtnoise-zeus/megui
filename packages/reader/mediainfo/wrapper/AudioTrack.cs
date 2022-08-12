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

namespace MediaInfoWrapper
{
    ///<summary>Contains properties for a AudioTrack </summary>
    public class AudioTrack
    {
        private string _StreamOrder;
        private string _ID;
        private string _Format;
        private string _FormatInfo;
        private string _FormatString;
        private string _FormatCommercial;
        private string _FormatVersion;
        private string _FormatProfile;
        private string _FormatSettingsSBR;
        private string _FormatSettingsPS;
        private string _MuxingMode;
        private string _CodecID;
        private string _CodecIDString;
        private string _CodecIDInfo;
        private string _BitRateMode;
        private string _Channels;
        private string _ChannelsString;
        private string _ChannelPositionsString2;
        private string _SamplingRate;
        private string _SamplingRateString;
        private string _Delay;
        private string _Title;
        private string _Language;
        private string _LanguageString;
        private string _Default;
        private string _Forced;
        private string _Source;

        ///<summary>Stream order in the file, whatever is the kind of stream (base=0)</summary>
        public string StreamOrder
        {
            get
            {
                if (String.IsNullOrEmpty(this._StreamOrder))
                    this._StreamOrder = "";
                return _StreamOrder;
            }
            set
            {
                this._StreamOrder = value;
            }
        }

        ///<summary> A ID for the stream </summary>
        public string ID
        {
            get
            {
                if (String.IsNullOrEmpty(this._ID))
                    this._ID="";
                return _ID;
            }
            set
            {
                this._ID=value;
            }
        }

        ///<summary> the Format used</summary>
        public string Format
        {
            get
            {
                if (String.IsNullOrEmpty(this._Format))
                    this._Format = "";
                return _Format;
            }
            set
            {
                this._Format = value;
            }
        }

        ///<summary> Info about the Format used</summary>
        public string FormatInfo
        {
            get
            {
                if (String.IsNullOrEmpty(this._FormatInfo))
                    this._FormatInfo = "";
                return _FormatInfo;
            }
            set
            {
                this._FormatInfo = value;
            }
        }

        ///<summary> Info about the Format used</summary>
        public string FormatString
        {
            get
            {
                if (String.IsNullOrEmpty(this._FormatString))
                    this._FormatString = "";
                return _FormatString;
            }
            set
            {
                this._FormatString = value;
            }
        }

        ///<summary> Info about the Format used</summary>
        public string FormatCommercial
        {
            get
            {
                if (String.IsNullOrEmpty(this._FormatCommercial))
                    this._FormatCommercial = "";
                return _FormatCommercial;
            }
            set
            {
                this._FormatCommercial = value;
            }
        }

        ///<summary> the Version of the Format used</summary>
        public string FormatVersion
        {
            get
            {
                if (String.IsNullOrEmpty(this._FormatVersion))
                    this._FormatVersion = "";
                return _FormatVersion;
            }
            set
            {
                this._FormatVersion = value;
            }
        }

        ///<summary> the Profile of the Format used</summary>
        public string FormatProfile
        {
            get
            {
                if (String.IsNullOrEmpty(this._FormatProfile))
                    this._FormatProfile = "";
                return _FormatProfile;
            }
            set
            {
                this._FormatProfile = value;
            }
        }

        ///<summary> the SBR flag</summary>
        public string FormatSettingsSBR
        {
            get
            {
                if (String.IsNullOrEmpty(this._FormatSettingsSBR))
                    this._FormatSettingsSBR = "";
                return _FormatSettingsSBR;
            }
            set
            {
                this._FormatSettingsSBR = value;
            }
        }

        ///<summary> the PS flag</summary>
        public string FormatSettingsPS
        {
            get
            {
                if (String.IsNullOrEmpty(this._FormatSettingsPS))
                    this._FormatSettingsPS = "";
                return _FormatSettingsPS;
            }
            set
            {
                this._FormatSettingsPS = value;
            }
        }

        ///<summary> how the stream has been muxed </summary>
        public string MuxingMode
        {
            get
            {
                if (String.IsNullOrEmpty(this._MuxingMode))
                    this._MuxingMode = "";
                return _MuxingMode;
            }
            set
            {
                this._MuxingMode = value;
            }
        }

        ///<summary> the ID of the Codec, found in the container </summary>
        public string CodecID
        {
            get
            {
                if (String.IsNullOrEmpty(this._CodecID))
                    this._CodecID = "";
                return _CodecID;
            }
            set
            {
                this._CodecID = value;
            }
        }

        ///<summary> the ID of the Codec, found in the container </summary>
        public string CodecIDString
        {
            get
            {
                if (String.IsNullOrEmpty(this._CodecIDString))
                    this._CodecIDString = "";
                return _CodecIDString;
            }
            set
            {
                this._CodecIDString = value;
            }
        }

        ///<summary> Info about the CodecID </summary>
        public string CodecIDInfo
        {
            get
            {
                if (String.IsNullOrEmpty(this._CodecIDInfo))
                    this._CodecIDInfo = "";
                return _CodecIDInfo;
            }
            set
            {
                this._CodecIDInfo = value;
            }
        }

        ///<summary> Name of the track </summary>
        public string Title
        {
            get
            {
                if (String.IsNullOrEmpty(this._Title))
                    this._Title="";
                return _Title;
            }
            set
            {
                this._Title=value;
            }
        }

        ///<summary> Bit rate mode (VBR, CBR) </summary>
        public string BitRateMode
        {
            get
            {
                if (String.IsNullOrEmpty(this._BitRateMode))
                    this._BitRateMode="";
                return _BitRateMode;
            }
            set
            {
                this._BitRateMode=value;
            }
        }

        ///<summary> Number of channels </summary>
        public string Channels
        {
            get
            {
                if (String.IsNullOrEmpty(this._Channels))
                    this._Channels="";
                return _Channels;
            }
            set
            {
                this._Channels=value;
            }
        }

        ///<summary> Number of channels </summary>
        public string ChannelsString
        {
            get
            {
                if (String.IsNullOrEmpty(this._ChannelsString))
                    this._ChannelsString="";
                return _ChannelsString;
            }
            set
            {
                this._ChannelsString=value;
            }
        }

        ///<summary> Positions of channels (x/y.z format) </summary>
        public string ChannelPositionsString2
        {
            get
            {
                if (String.IsNullOrEmpty(this._ChannelPositionsString2))
                    this._ChannelPositionsString2 = "";
                return _ChannelPositionsString2;
            }
            set
            {
                this._ChannelPositionsString2 = value;
            }
        }

        ///<summary> Sampling rate </summary>
        public string SamplingRate
        {
            get
            {
                if (String.IsNullOrEmpty(this._SamplingRate))
                    this._SamplingRate="";
                return _SamplingRate;
            }
            set
            {
                this._SamplingRate=value;
            }
        }

        ///<summary> in KHz </summary>
        public string SamplingRateString
        {
            get
            {
                if (String.IsNullOrEmpty(this._SamplingRateString))
                    this._SamplingRateString="";
                return _SamplingRateString;
            }
            set
            {
                this._SamplingRateString=value;
            }
        }

        ///<summary> Delay fixed in the stream (relative) </summary>
        public string Delay
        {
            get
            {
                if (String.IsNullOrEmpty(this._Delay))
                    this._Delay = "";
                return _Delay;
            }
            set
            {
                this._Delay = value;
            }
        }

        ///<summary> Language (2 letters) </summary>
        public string Language
        {
            get
            {
                if (String.IsNullOrEmpty(this._Language))
                    this._Language="";
                return _Language;
            }
            set
            {
                this._Language=value;
            }
        }

        ///<summary> Language (full) </summary>
        public string LanguageString
        {
            get
            {
                if (String.IsNullOrEmpty(this._LanguageString))
                    this._LanguageString="";
                return _LanguageString;
            }
            set
            {
                this._LanguageString=value;
            }
        }

        ///<summary> Default Info </summary>
        public string Default
        {
            get
            {
                if (String.IsNullOrEmpty(this._Default))
                    this._Default = "";
                return _Default;
            }
            set
            {
                this._Default = value;
            }
        }

        ///<summary> Forced Info </summary>
        public string Forced
        {
            get
            {
                if (String.IsNullOrEmpty(this._Forced))
                    this._Forced = "";
                return _Forced;
            }
            set
            {
                this._Forced = value;
            }
        }

        ///<summary>Source</summary>
        public string Source
        {
            get
            {
                if (String.IsNullOrEmpty(this._Source))
                    this._Source = "";
                return _Source;
            }
            set
            {
                this._Source = value;
            }
        }
    }
}