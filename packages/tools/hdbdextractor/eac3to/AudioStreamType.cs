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

using MeGUI.packages.tools.hdbdextractor;

namespace eac3to
{
    /// <summary>An enumeration of Audio Stream types</summary>
    [Flags]
    public enum AudioStreamType
    {
        [StringValue("UNKNOWN")]
        UNKNOWN,
        [StringValue("AC3")]
        AC3,
        [StringValue("E-AC3")]
        EAC3,
        [StringValue("DTS")]
        DTS,
        [StringValue("TrueHD")]
        TrueHD,
        [StringValue("RAW")]
        RAW,
        [StringValue("PCM")]
        PCM,
        [StringValue("WAV")]
        WAV,
        [StringValue("Multi-Channel WAV")]
        WAVS,
        [StringValue("MP2")]
        MP2,
        [StringValue("MP3")]
        MP3,
        [StringValue("AAC")]
        AAC,
        [StringValue("FLAC")]
        FLAC,
        [StringValue("TTA1")]
        TTA,
        [StringValue("WAVPACK4")]
        WAVPACK,
        [StringValue("VORBIS")]
        VORBIS
    }
}