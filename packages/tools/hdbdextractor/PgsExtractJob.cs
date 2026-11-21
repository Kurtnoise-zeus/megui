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
using System.Collections.Generic;

using MeGUI.BdSub;

namespace MeGUI
{
    /// <summary>
    /// Extraction of Blu-ray PGS subtitle tracks (.sup) from a playlist (.mpls) or from M2TS files.
    /// Replaces the subtitle tracks that were extracted by eac3to.
    /// </summary>
    public class PgsExtractJob : Job
    {
        private List<string> source;
        private List<PgsTrackRequest> tracks;

        public PgsExtractJob()
            : base()
        {
            source = new List<string>();
            tracks = new List<PgsTrackRequest>();
        }

        /// <param name="input">playlist (.mpls) or M2TS file(s)</param>
        /// <param name="output">output folder</param>
        /// <param name="tracks">PGS tracks to extract</param>
        public PgsExtractJob(List<string> input, string output, List<PgsTrackRequest> tracks)
        {
            Input = input[0];
            source = input;
            Output = output;
            this.tracks = tracks;
        }

        /// <summary>playlist (.mpls) or one or several M2TS files</summary>
        public List<string> Source
        {
            get { return source; }
            set { source = value; }
        }

        /// <summary>PGS tracks to extract (index = rank of the PGS stream in the playlist / PMT)</summary>
        public List<PgsTrackRequest> Tracks
        {
            get { return tracks; }
            set { tracks = value; }
        }

        public override string CodecString
        {
            get { return "pgs"; }
        }

        public override string EncodingMode
        {
            get { return "ext"; }
        }
    }
}
