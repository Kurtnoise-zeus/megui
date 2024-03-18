// ****************************************************************************
// 
// Copyright (C) 2005-2024 Doom9 & al
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
    public class LSMASHIndexJob : IndexJob
    {
        public LSMASHIndexJob() : base()
        {
        }

        public LSMASHIndexJob(string input, string indexFile, int demuxType, List<AudioTrackInfo> audioTracks, bool loadSources) : base()
        {
            Input = input;
            LoadSources = loadSources;
            Output = input + ".lwi";
            OutputFile = indexFile;

            if (audioTracks == null || audioTracks.Count == 0)
            {
                AudioTracks = new List<AudioTrackInfo>();
                DemuxMode = 0;
            }
            else
            {
                DemuxMode = demuxType;
                AudioTracks = audioTracks;
            }

            DemuxVideo = false;
        }

        private string outputFile;
        public string OutputFile
        {
            get { return outputFile; }
            set { outputFile = value; }
        }
    
        public override string CodecString
        {
            get { return "lsmashindex"; }
        }

        public override string EncodingMode
        {
            get { return "idx"; }
        }
    }
}