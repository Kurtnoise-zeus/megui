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
using System.IO;

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
        private PgsColorOptions colors;

        public PgsExtractJob()
            : base()
        {
            source = new List<string>();
            tracks = new List<PgsTrackRequest>();
            colors = new PgsColorOptions();
        }

        /// <param name="input">playlist (.mpls) or M2TS file(s)</param>
        /// <param name="output">output folder (the .sup files are named by the tracks)</param>
        /// <param name="tracks">PGS tracks to extract</param>
        public PgsExtractJob(List<string> input, string output, List<PgsTrackRequest> tracks)
            : this(input, output, tracks, null)
        {
        }

        /// <param name="colors">optional recoloring of the subtitles (null = unchanged)</param>
        public PgsExtractJob(List<string> input, string output, List<PgsTrackRequest> tracks, PgsColorOptions colors)
        {
            this.colors = colors ?? new PgsColorOptions();
            Input = input[0];
            source = input;
            // Output is only used for the display in the queue and in the log ("... -> sup"):
            // the real output files are the ones of the tracks
            Output = Path.Combine(output, "sup");
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

        /// <summary>Recoloring of the subtitles (the palette is changed, the bitmaps are not)</summary>
        public PgsColorOptions Colors
        {
            get { return colors; }
            set { colors = value; }
        }

        public override string CodecString
        {
            get { return "pgs"; }
        }

        public override string EncodingMode
        {
            get { return "pgs extract"; }
        }
    }
}
