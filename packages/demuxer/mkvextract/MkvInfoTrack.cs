// ****************************************************************************
// 
// Copyright (C) 2005-2012 Doom9 & al
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
using System.IO;
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

    public class MkvInfoTrack
    {
        private TrackType _type;
        private int _iTrackID;
        private String _strCodecID, _strLanguage, _strInputFile, _strName, _strAudioChannels;
        private bool _bDefault, _bForced;

        public MkvInfoTrack() : this(null) { }

        public MkvInfoTrack(String strInputFile)
        {
            this._strInputFile = strInputFile;
            this._type = TrackType.Unknown;
            this._iTrackID = -1;
            this._strCodecID = String.Empty;
            this._strLanguage = String.Empty;
            this._bDefault = true;
            this._bForced = false;
            this._strName = String.Empty;
            this._strAudioChannels = String.Empty;
        }

        public TrackType Type
        {
            get { return _type; }
            set 
            { 
                _type = value;
                if (_type != TrackType.Video && _type != TrackType.Unknown && String.IsNullOrEmpty(_strLanguage))
                    _strLanguage = "eng";
            }
        }

        public int TrackID
        {
            get { return _iTrackID; }
            set { _iTrackID = value; }
        }

        public String CodecID
        {
            get { return _strCodecID; }
            set { _strCodecID = value; }
        }

        public String AudioChannels
        {
            get { return _strAudioChannels; }
            set { _strAudioChannels = value; }
        }

        public String Language
        {
            get { return _strLanguage; }
            set { _strLanguage = value; }
        }

        public String Name
        {
            get { return _strName; }
            set { _strName = value; }
        }

        public String InputFile        
        {
            get { return _strInputFile; }
            set { _strInputFile = value; }
        }

        public bool DefaultTrack
        {
            get { return _bDefault; }
            set { _bDefault = value; }
        }

        public bool ForcedTrack
        {
            get { return _bForced; }
            set { _bForced = value; }
        }

        [XmlIgnore()]
        public String FileName
        {
            get 
            {
                if (_type != TrackType.Audio && _type != TrackType.Subtitle)
                    return null;

                string strExtension = String.Empty;
                string[] strCodec = _strCodecID.Split('/');
                switch (strCodec[0])
                {
                    case "A_AC3": strExtension = "ac3"; break;
                    case "A_TRUEHD": strExtension = "thd"; break;
                    case "A_MPEG":
                        if (strCodec.Length > 0 && strCodec[1].Equals("L3"))
                            strExtension = "mp3";
                        else
                            strExtension = "mp2";
                        break;
                    case "A_DTS": strExtension = "dts"; break;
                    case "A_PCM": strExtension = "wav"; break;
                    case "A_VORBIS": strExtension = "ogg"; break;
                    case "A_FLAC": strExtension = "flac"; break;
                    case "A_REAL": strExtension = "ra"; break;
                    case "A_AAC": strExtension = "aac"; break;
                    case "A_MS/ACM": strExtension = "wav"; break;
                    case "S_TEXT":
                        if (strCodec.Length > 0 && strCodec[1].Equals("SSA"))
                            strExtension = "ssa";
                        else if (strCodec.Length > 0 && strCodec[1].Equals("ASS"))
                            strExtension = "ass";
                        else if (strCodec.Length > 0 && strCodec[1].Equals("USF"))
                            strExtension = "usf";
                        else
                            strExtension = "srt";
                        break;
                    case "S_VOBSUB": strExtension = "idx"; break;
                    case "S_HDMV": strExtension = "sup"; break;
                    default: strExtension = strCodec[0] + ".unknown"; break;
                }

                if (String.IsNullOrEmpty(strExtension))
                    return null;

                return Path.GetFileNameWithoutExtension(_strInputFile) + " - [" + _iTrackID + "] " + LanguageSelectionContainer.lookupISOCode(_strLanguage) + "." + strExtension;
            }
        }

        [XmlIgnore()]
        public AudioTrackInfo AudioTrackInfo
        {
            get
            {
                if (_type != TrackType.Audio)
                    return null;

                string strCodecID = _strCodecID.Split('/')[0].Replace("AC3", "AC-3");
                if (strCodecID.Length > 2 && strCodecID.Substring(1, 1).Equals("_"))
                    strCodecID = strCodecID.Substring(2);

                AudioTrackInfo oAudioTrack = new AudioTrackInfo(LanguageSelectionContainer.lookupISOCode(_strLanguage), _strAudioChannels, strCodecID, _iTrackID);
                oAudioTrack.ContainerType = "MATROSKA";
                oAudioTrack.Name = _strName;

                return oAudioTrack;
            }
        }

        [XmlIgnore()]
        public OneClickStream SubtitleTrackInfo
        {
            get
            {
                if (_type != TrackType.Subtitle)
                    return null;

                string strCodecID = _strCodecID.Split('/')[0].Replace("AC3", "AC-3");
                if (strCodecID.Length > 2 && strCodecID.Substring(1,1).Equals("_"))
                    strCodecID = strCodecID.Substring(2);

                OneClickStream oSubtitleTrack = new OneClickStream(_strInputFile, TrackType.Subtitle, strCodecID, "MATROSKA", _iTrackID.ToString(), LanguageSelectionContainer.lookupISOCode(_strLanguage), _strName, 0, _bDefault, _bForced, null, AudioEncodingMode.IfCodecDoesNotMatch, this);

                return oSubtitleTrack;
            }
        }
    }
}
