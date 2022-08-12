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
using System.IO;

namespace MeGUI
{
	/// <summary>
	/// Summary description for D2VIndexJob.
	/// </summary>
	public class D2VIndexJob : IndexJob
	{	
		public D2VIndexJob():base()
		{
		}

        public D2VIndexJob(string input, string output, int demuxType, List<AudioTrackInfo> audioTracks, bool loadSources, bool demuxVideo)
            : base()
        {
            Input = input;
            Output = output;
            DemuxMode = demuxType;
            AudioTracks = audioTracks;
            LoadSources = loadSources;
            DemuxVideo = demuxVideo;
            FilesToDelete.Add(Path.Combine(Path.GetDirectoryName(input), Path.GetFileNameWithoutExtension(input) + ".log"));
            FilesToDelete.Add(output + ".bad");
            FilesToDelete.Add(Path.ChangeExtension(output,".fix.txt"));
        }

        public override string CodecString
        {
            get { return "d2vindex"; }
        }

        public override string EncodingMode
        {
            get { return "idx"; }
        }
    }
}