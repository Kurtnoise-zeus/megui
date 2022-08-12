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

namespace MeGUI
{
    public class OneClickStream : ICloneable
    {
        private string _inputFilePath;
        private string _demuxFilePath;
        private AudioCodecSettings _encoderSettings;
        private AudioEncodingMode _encodingMode;
        private TrackInfo _trackInfo;

        public OneClickStream(string path, TrackType trackType, string codec, string container, int ID, string language, string name, int delay, bool bDefaultTrack, bool bForceTrack, AudioCodecSettings oSettings, AudioEncodingMode oEncodingMode)
        {
            _trackInfo = new TrackInfo(language, name);
            _trackInfo.TrackType = trackType;
            _trackInfo.Delay = delay;
            _trackInfo.DefaultTrack = bDefaultTrack;
            _trackInfo.ForcedTrack = bForceTrack;
            _trackInfo.ContainerType = container;
            _trackInfo.Codec = codec;
            _trackInfo.TrackID = ID;

            this._inputFilePath = path;

            this._encoderSettings = oSettings;
            if ((int)oEncodingMode == -1)
                this._encodingMode = AudioEncodingMode.IfCodecDoesNotMatch;
            else
                this._encodingMode = oEncodingMode;
        }

        public OneClickStream(AudioTrackInfo oInfo) 
        {
            this._trackInfo = oInfo;
            this._encodingMode = AudioEncodingMode.IfCodecDoesNotMatch;
        }

        public OneClickStream(SubtitleTrackInfo oInfo)
        {
            this._trackInfo = oInfo;
            this._encodingMode = AudioEncodingMode.IfCodecDoesNotMatch;
        }

        public OneClickStream() : this(null, TrackType.Unknown, null, null, 0, null, null, 0, false, false, null, AudioEncodingMode.IfCodecDoesNotMatch) { }

        /// <summary>
        /// generates a copy of this object
        /// </summary>
        /// <returns>the codec specific settings of this object</returns>
        object ICloneable.Clone()
        {
            return Clone();
        }

        public OneClickStream Clone()
        {
            OneClickStream newStream = (OneClickStream)this.MemberwiseClone();
            if (_encoderSettings != null)
                newStream.EncoderSettings = _encoderSettings.Clone();
            if (_trackInfo != null)
                newStream.TrackInfo = _trackInfo.Clone();
            return newStream;
        }

        // Stream Type
        public TrackType Type
        {
            get { return _trackInfo.TrackType; }
        }

        // Stream Language
        public string Language
        {
            get { return _trackInfo.Language; }
            set { _trackInfo.Language = value; }
        }

        // Stream Name
        public string Name
        {
            get { return _trackInfo.Name; }
            set { _trackInfo.Name = value; }
        }

        // Demux File Path
        public string DemuxFilePath
        {
            get
            {
                if (!String.IsNullOrEmpty(_demuxFilePath))
                    return _demuxFilePath;
                if (_trackInfo != null && _trackInfo.DemuxFileName != null)
                    return _trackInfo.DemuxFileName;
                else
                    return _inputFilePath; 
            }
            set { _inputFilePath = _demuxFilePath = value; }
        }

        // Stream Delay
        public int Delay
        {
            get { return _trackInfo.Delay; }
            set { _trackInfo.Delay = value; }
        }

        // Stream Delay
        public bool ForcedStream
        {
            get { return _trackInfo.ForcedTrack; }
            set { _trackInfo.ForcedTrack = value; }
        }

        // Stream Delay
        public bool DefaultStream
        {
            get { return _trackInfo.DefaultTrack; }
            set { _trackInfo.DefaultTrack = value; }
        }

        // Track Info
        public TrackInfo TrackInfo
        {
            get { return _trackInfo; }
            set { _trackInfo = value; }
        }

        // Audio Track Info
        public AudioCodecSettings EncoderSettings
        {
            get { return _encoderSettings; }
            set { _encoderSettings = value; }
        }

        // Audio Track Info
        public AudioEncodingMode EncodingMode
        {
            get { return _encodingMode; }
            set 
            {
                if ((int)value == -1)
                    _encodingMode = AudioEncodingMode.IfCodecDoesNotMatch;
                else
                    _encodingMode = value; 
            }
        }

        public override string ToString()
        {
            string strCodec = _trackInfo.Codec;
            if (_trackInfo.IsMKVContainer())
            {
                string[] arrCodec = new string[] { };
                arrCodec = _trackInfo.Codec.Split('/');
                if (arrCodec[0].Substring(1, 1).Equals("_"))
                    arrCodec[0] = arrCodec[0].Substring(2);
                strCodec = arrCodec[0];
            }

            string fullString = "[";
            if (_trackInfo is AudioTrackInfo)
                fullString += ((AudioTrackInfo)_trackInfo).TrackIDx + "] - " + strCodec;
            else
                fullString += _trackInfo.MMGTrackID + "] - " + strCodec;
            if (_trackInfo is AudioTrackInfo && !string.IsNullOrEmpty(((AudioTrackInfo)_trackInfo).NbChannels))
                fullString += " - " + ((AudioTrackInfo)_trackInfo).NbChannels;
            if (_trackInfo is AudioTrackInfo && !string.IsNullOrEmpty(((AudioTrackInfo)_trackInfo).SamplingRate))
                fullString += " - " + ((AudioTrackInfo)_trackInfo).SamplingRate;
            if (!string.IsNullOrEmpty(_trackInfo.Language))
                fullString += " / " + _trackInfo.Language;
            return fullString.Trim();
        }
    }
}