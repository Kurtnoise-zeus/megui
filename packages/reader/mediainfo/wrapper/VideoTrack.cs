// ****************************************************************************
// 
// Copyright (C) 2005-2026 Doom9 & al
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
    ///<summary>Contains properties for a VideoTrack </summary>
    public class VideoTrack
    {
        private string _StreamOrder;
        private string _ID;
        private string _CodecID;
        private string _CodecIDString;
        private string _CodecIDInfo;
        private string _Format;
        private string _FormatInfo;
        private string _FormatString;
        private string _FormatCommercial;
        private string _FormatVersion;
        private string _Width;
        private string _Height;
        private string _AspectRatio;
        private string _AspectRatioString;
        private string _PixelAspectRatio;
        private string _FrameRate;
        private string _FrameRateNum;
        private string _FrameRateDen;
        private string _FrameRateOriginal;
        private string _FrameRateMode;
        private string _FrameRateModeString;
        private string _FrameCount;
        private string _BitDepth;
        private string _Delay;
        private string _DurationString3;
        private string _Language;
        private string _LanguageString;
        private string _ScanTypeString;
        private string _Default;
        private string _Forced;
        private string _Title;
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

        ///<summary> A ID for this stream in this file </summary>
        public string ID
        {
            get
            {
                if (String.IsNullOrEmpty(this._ID))
                    this._ID = "";
                return _ID;
            }
            set
            {
                this._ID = value;
            }
        }

        ///<summary> Name of the track </summary>
        public string Title
        {
            get
            {
                if (String.IsNullOrEmpty(this._Title))
                    this._Title = "";
                return _Title;
            }
            set
            {
                this._Title = value;
            }
        }

        ///<summary> Codec ID used </summary>
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

        ///<summary> Codec ID String used </summary>
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

        ///<summary> Info about codec ID </summary>
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

        ///<summary> Width </summary>
        public string Width
        {
            get
            {
                if (String.IsNullOrEmpty(this._Width))
                    this._Width = "";
                return _Width;
            }
            set
            {
                this._Width = value;
            }
        }

        ///<summary> Height </summary>
        public string Height
        {
            get
            {
                if (String.IsNullOrEmpty(this._Height))
                    this._Height = "";
                return _Height;
            }
            set
            {
                this._Height = value;
            }
        }

        ///<summary> Aspect ratio </summary>
        public string AspectRatio
        {
            get
            {
                if (String.IsNullOrEmpty(this._AspectRatio))
                    this._AspectRatio = "";
                return _AspectRatio;
            }
            set
            {
                this._AspectRatio = value;
            }
        }

        ///<summary> Aspect ratio </summary>
        public string AspectRatioString
        {
            get
            {
                if (String.IsNullOrEmpty(this._AspectRatioString))
                    this._AspectRatioString = "";
                return _AspectRatioString;
            }
            set
            {
                this._AspectRatioString = value;
            }
        }

        ///<summary> Pixel Aspect Ratio </summary>
        public string PixelAspectRatio
        {
            get
            {
                if (String.IsNullOrEmpty(this._PixelAspectRatio))
                    this._PixelAspectRatio = "";
                return _PixelAspectRatio;
            }
            set
            {
                this._PixelAspectRatio = value;
            }
        }

        ///<summary> Frames per second </summary>
        public string FrameRate
        {
            get
            {
                if (String.IsNullOrEmpty(this._FrameRate))
                    this._FrameRate = "";
                return _FrameRate;
            }
            set
            {
                this._FrameRate = value;
            }
        }

        ///<summary> Frames per second </summary>
        public string FrameRateNum
        {
            get
            {
                if (String.IsNullOrEmpty(this._FrameRateNum))
                    this._FrameRateNum = "";
                return _FrameRateNum;
            }
            set
            {
                this._FrameRateNum = value;
            }
        }

        ///<summary> Frames per second </summary>
        public string FrameRateDen
        {
            get
            {
                if (String.IsNullOrEmpty(this._FrameRateDen))
                    this._FrameRateDen = "";
                return _FrameRateDen;
            }
            set
            {
                this._FrameRateDen = value;
            }
        }

        ///<summary> Original (in the raw stream) frames per second </summary>
        public string FrameRateOriginal
        {
            get
            {
                if (String.IsNullOrEmpty(this._FrameRateOriginal))
                    this._FrameRateOriginal = "";
                return _FrameRateOriginal;
            }
            set
            {
                this._FrameRateOriginal = value;
            }
        }

        ///<summary>Frame rate mode (CFR, VFR)</summary>
        public string FrameRateMode
        {
            get
            {
                if (String.IsNullOrEmpty(this._FrameRateMode))
                    this._FrameRateMode = "";
                return _FrameRateMode;
            }
            set
            {
                this._FrameRateMode = value;
            }
        }

        ///<summary>Frame rate mode (Constant, Variable)</summary>
        public string FrameRateModeString
        {
            get
            {
                if (String.IsNullOrEmpty(this._FrameRateModeString))
                    this._FrameRateModeString = "";
                return _FrameRateModeString;
            }
            set
            {
                this._FrameRateModeString = value;
            }
        }

        ///<summary> Frame count </summary>
        public string FrameCount
        {
            get
            {
                if (String.IsNullOrEmpty(this._FrameCount))
                    this._FrameCount = "";
                return _FrameCount;
            }
            set
            {
                this._FrameCount = value;
            }
        }

        ///<summary> 16/24/32 </summary>
        public string BitDepth
        {
            get
            {
                if (String.IsNullOrEmpty(this._BitDepth))
                    this._BitDepth = "";
                return _BitDepth;
            }
            set
            {
                this._BitDepth = value;
            }
        }

        ///<summary> Delay fixed in the stream (relative) IN MS </summary>
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

        ///<summary> Play time in format : HH:MM:SS.MMM </summary>
        public string DurationString3
        {
            get
            {
                if (String.IsNullOrEmpty(this._DurationString3))
                    this._DurationString3 = "";
                return _DurationString3;
            }
            set
            {
                this._DurationString3 = value;
            }
        }

        ///<summary> Language (2-letter ISO 639-1 if exists, else 3-letter ISO 639-2, and with optional ISO 3166-1 country separated by a dash if available, e.g. en, en-us, zh-cn) </summary>
        public string Language
        {
            get
            {
                if (String.IsNullOrEmpty(this._Language))
                    this._Language = "";
                return _Language;
            }
            set
            {
                this._Language = value;
            }
        }

        ///<summary> Language (full) </summary>
        public string LanguageString
        {
            get
            {
                if (String.IsNullOrEmpty(this._LanguageString))
                    this._LanguageString = "";
                return _LanguageString;
            }
            set
            {
                this._LanguageString = value;
            }
        }

        ///<summary> Format used </summary>
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

        ///<summary> Format used + additional features </summary>
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

        ///<summary> Info about Format </summary>
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

        ///<summary>Commercial name used by vendor for theses setings or Format field if there is no difference</summary>
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

        ///<summary>Version of this format</summary>
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

        ///<summary> ScanType Info (string format)</summary>
        public string ScanTypeString
        {
            get
            {
                if (String.IsNullOrEmpty(this._ScanTypeString))
                    this._ScanTypeString = "";
                return _ScanTypeString;
            }
            set
            {
                this._ScanTypeString = value;
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