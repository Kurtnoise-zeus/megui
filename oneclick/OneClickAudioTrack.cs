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

using MeGUI.core.details;
using MeGUI.core.util;

namespace MeGUI
{
	/// <summary>
	/// Summary description for OneClickAudioTrack.
	/// </summary>
	public class OneClickAudioTrack
	{
        private AudioJob _audioJob;
        private MuxStream _directMuxAudio;
        private AudioTrackInfo _audioTrackInfo;

		public OneClickAudioTrack() : this(null, null, null, false) { }

        public OneClickAudioTrack(AudioJob oAudioJob, MuxStream oMuxStream, AudioTrackInfo oAudioTrackInfo, bool bMKVTrack)
        {
            _audioJob = oAudioJob;
            _directMuxAudio = oMuxStream;
            _audioTrackInfo = oAudioTrackInfo;
            if (_audioTrackInfo != null)
                _audioTrackInfo.ExtractMKVTrack = bMKVTrack;
        }

		/// <summary>
		/// gets / sets the container of the output
		/// </summary>
        public AudioJob AudioJob
		{
            get { return _audioJob; }
            set { _audioJob = value; }
		}

        public MuxStream DirectMuxAudio
        {
            get { return _directMuxAudio; }
            set { _directMuxAudio = value; }
        }

        public AudioTrackInfo AudioTrackInfo
        {
            get { return _audioTrackInfo; }
            set { _audioTrackInfo = value; }
        }
	}
}