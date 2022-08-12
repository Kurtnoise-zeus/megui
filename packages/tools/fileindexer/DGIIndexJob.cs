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

namespace MeGUI
{
    public class DGIIndexJob : IndexJob
    {
        bool bOneClickProcessing;

        public DGIIndexJob() : base()
        {
        }

        public DGIIndexJob(string input, string output, int demuxType, List<AudioTrackInfo> audioTracks, bool loadSources, bool demuxVideo, bool bOneClick)
        {
            Input = input;
            Output = output;
            DemuxMode = demuxType;
            AudioTracks = audioTracks;
            LoadSources = loadSources;
            DemuxVideo = demuxVideo;
            bOneClickProcessing = bOneClick;
        }

        /// <summary>
        /// gets / sets whether the job runs in a OneClick context (then the log file must be deleted later)
        /// </summary>
        public bool OneClickProcessing
        {
            get { return bOneClickProcessing; }
            set { bOneClickProcessing = value; }
        }

        public override string CodecString
        {
            get { return "dgiindex"; }
        }

        public override string EncodingMode
        {
            get { return "idx"; }
        }
    }
}