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
    ///<summary>Contains properties for a GeneralTrack </summary>
    public class GeneralTrack
    {
        private string _FileSize;
        private string _Format;
        private string _FormatString;
        private string _PlayTimeString3;
        private string _Attachments;

        ///<summary> File size in bytes </summary>
        public string FileSize
        {
            get
            {
                if (String.IsNullOrEmpty(this._FileSize))
                    this._FileSize = "";
                return _FileSize;
            }
            set
            {
                this._FileSize = value;
            }
        }

        ///<summary> Format (short name) </summary>
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

        ///<summary> Format (full name) </summary>
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

        ///<summary> Play time in format : HH:MM:SS.MMM </summary>
        public string PlayTimeString3
        {
            get
            {
                if (String.IsNullOrEmpty(this._PlayTimeString3))
                    this._PlayTimeString3 = "";
                return _PlayTimeString3;
            }
            set
            {
                this._PlayTimeString3 = value;
            }
        }

        ///<summary> Attachments separated by " / " </summary>
        public string Attachments
        {
            get
            {
                if (String.IsNullOrEmpty(this._Attachments))
                    this._Attachments = "";
                return _Attachments;
            }
            set
            {
                this._Attachments = value;
            }
        }
    }
}