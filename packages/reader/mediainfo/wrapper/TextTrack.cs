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
    ///<summary>Contains properties for a TextTrack </summary>
    public class TextTrack
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
        private string _Delay;
        private string _Language;
        private string _LanguageString;
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
                    this._ID="";
                return _ID;
            }
            set
            {
                this._ID=value;
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

        ///<summary> Codec used </summary>
        public string CodecID
        {
            get
            {
                if (String.IsNullOrEmpty(this._CodecID))
                    this._CodecID="";
                return _CodecID;
            }
            set
            {
                this._CodecID=value;
            }
        }

        ///<summary> Codec used (test) </summary>
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

        ///<summary> Codec used (info) </summary>
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

        ///<summary> Delay fixed in the stream (relative) </summary>
        public string Delay
        {
            get
            {
                if (String.IsNullOrEmpty(this._Delay))
                    this._Delay="";
                return _Delay;
            }
            set
            {
                this._Delay=value;
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