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
using System.Xml.Serialization;

namespace MeGUI
{
    public enum TrackType
    {
        Audio,
        Subtitle,
        Unknown,
        Video
    }

    [XmlInclude(typeof(SubtitleTrackInfo)), XmlInclude(typeof(AudioTrackInfo)), XmlInclude(typeof(VideoTrackInfo))]
    public class TrackInfo : ICloneable
    {
        private string _codec, _containerType, _language, _name, _sourceFileName;
        private int _trackID, _mmgTrackID, _delay, _trackIndex, _oneClickTrackNumber;
        private bool _bDefault, _bForced, _bMKVTrack;
        private TrackType _trackType;

        public TrackInfo() : this(null, null) { }

        public TrackInfo(string language, string name)
        {
            this._language = language;
            this._name = name;
            this._trackType = TrackType.Unknown;
            this._trackID = 0;
            this._mmgTrackID = 0;
            this._delay = 0;
            this._trackIndex = 0;
            this._codec = _containerType = String.Empty;
            this._bMKVTrack = false;
            this._oneClickTrackNumber = 0;
        }

        /// <summary>
        /// generates a copy of this object
        /// </summary>
        /// <returns>the codec specific settings of this object</returns>
        object ICloneable.Clone()
        {
            return Clone();
        }

        public TrackInfo Clone()
        {
            TrackInfo oTrackInfo = (TrackInfo)this.MemberwiseClone();
            return oTrackInfo;
        }

        /// <summary>
        /// The Track Type
        /// </summary>
        public TrackType TrackType
        {
            get { return _trackType; }
            set { _trackType = value; }
        }

        /// <summary>
        /// The full language string
        /// </summary>
        public string Language
        {
            get { return _language; }
            set { _language = value; }
        }

        /// <summary>
        /// The full source file name incuding path
        /// </summary>
        public string SourceFileName
        {
            get { return _sourceFileName; }
            set { _sourceFileName = value; }
        }

        /// <summary>
        /// The track name
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// The track ID
        /// </summary>
        public int TrackID
        {
            get { return _trackID; }
            set { _trackID = value; }
        }

        /// <summary>
        /// The MMG track ID
        /// </summary>
        public int MMGTrackID
        {
            get { return _mmgTrackID; }
            set { _mmgTrackID = value; }
        }

        /// <summary>
        /// The Container Type
        /// </summary>
        public string ContainerType
        {
            get { return _containerType; }
            set { _containerType = value; }
        }

        /// <summary>
        /// The track index
        /// </summary>
        public int TrackIndex
        {
            get { return _trackIndex; }
            set { _trackIndex = value; }
        }

        /// <summary>
        /// The OneClick track number
        /// </summary>
        public int OneClickTrackNumber
        {
            get { return _oneClickTrackNumber; }
            set { _oneClickTrackNumber = value; }
        }

        /// <summary>
        /// The Codec String
        /// </summary>
        public string Codec
        {
            get { return _codec; }
            set { _codec = value; }
        }

        /// <summary>
        /// The delay of the track
        /// </summary>
        public int Delay
        {
            get { return _delay; }
            set { _delay = value; }
        }

        /// <summary>
        /// Default Track
        /// </summary>
        public bool DefaultTrack
        {
            get { return _bDefault; }
            set { _bDefault = value; }
        }

        /// <summary>
        /// Forced Track
        /// </summary>
        public bool ForcedTrack
        {
            get { return _bForced; }
            set { _bForced = value; }
        }

        public bool IsMKVContainer()
        {
            if (String.IsNullOrEmpty(_containerType))
                return false;
            else
                return _containerType.Trim().ToUpperInvariant().Equals("MATROSKA");
        }

        public bool ExtractMKVTrack
        {
            get { return _bMKVTrack; }
            set { _bMKVTrack = value; }
        }

        [XmlIgnore()]
        public String DemuxFileName
        {
            get
            {
                if (String.IsNullOrEmpty(_sourceFileName))
                    return null;

                string strExtension = String.Empty;
                string strCodec = String.Empty;
                string strFileName = String.Empty;

                if (!String.IsNullOrEmpty(_codec))
                    strCodec = _codec.ToUpperInvariant();

                if (IsMKVContainer())
                {
                    string[] arrCodec = new string[] { };
                    arrCodec = _codec.Split('/');
                    if (!String.IsNullOrEmpty(arrCodec[0]) && arrCodec[0].Substring(1, 1).Equals("_"))
                        arrCodec[0] = arrCodec[0].Substring(2);
                    strCodec = arrCodec[0].ToUpperInvariant();
                }

                if (strCodec.Contains("TRUEHD"))
                {
                    if (strCodec.Contains("AC-3"))
                        strCodec = "TRUEHD+AC3";
                    else
                        strCodec = "TRUEHD";
                }

                switch (strCodec)
                {
                    // audio
                    case "AC-3": strExtension = "ac3"; break;
                    case "E-AC-3": strExtension = "eac3"; break;
                    case "TRUEHD": strExtension = "thd"; break;
                    case "TRUEHD+AC3": strExtension = "thd+ac3"; break;
                    case "DTS": strExtension = "dts"; break;
                    case "MP3": strExtension = "mp3"; break;
                    case "MP2": strExtension = "mp2"; break;
                    case "PCM": strExtension = "wav"; break;
                    case "MS/ACM": strExtension = "wav"; break;
                    case "VORBIS": strExtension = "ogg"; break;
                    case "FLAC": strExtension = "flac"; break;
                    case "REAL": strExtension = "ra"; break;
                    case "AAC": strExtension = "aac"; break;
                    case "AVS": strExtension = "avs"; break;

                    // subtitle
                    case "ASS": strExtension = "ass"; break;
                    case "PGS": strExtension = "sup"; break;
                    case "SRT": strExtension = "srt"; break;
                    case "SSA": strExtension = "ssa"; break;
                    case "TTXT": strExtension = "ttxt"; break;
                    case "VOBSUB": strExtension = "idx"; break;

                    // video
                    case "ASP": strExtension = "avi"; break;
                    case "AVC": strExtension = "264"; break;
                    case "HEVC": strExtension = "265"; break;
                    case "HFYU": strExtension = "avi"; break;
                    case "MPEG1": strExtension = "m1v"; break;
                    case "MPEG2": strExtension = "m2v"; break;
                    case "VC1": strExtension = "vc1"; break;
                    default: strExtension = strCodec + ".unknown"; break;
                }

                if (!strExtension.Equals("avs", StringComparison.InvariantCultureIgnoreCase))
                {
                    strFileName = System.IO.Path.GetFileNameWithoutExtension(_sourceFileName) + " - ";
                    if (_oneClickTrackNumber > 0)
                        strFileName += "[" + _oneClickTrackNumber + "]";
                    strFileName += "[" + _trackIndex + "]";
                    if (!String.IsNullOrEmpty(_language))
                        strFileName += " " + _language;
                    if (_delay != 0)
                        strFileName += " Delay " + _delay + "ms";
                    strFileName += "." + strExtension;
                }
                else
                    strFileName = System.IO.Path.GetFileName(_sourceFileName);
                return strFileName;
            }
        }
    }
}