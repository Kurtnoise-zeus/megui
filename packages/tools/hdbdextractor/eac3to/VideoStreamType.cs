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

using MeGUI.packages.tools.hdbdextractor;

namespace eac3to
{
    /// <summary>An enumeration of Video Stream types</summary>
    public enum VideoStreamType
    {
        [StringValue("UNKNOWN")]
        UNKNOWN,
        [StringValue("AVC")]
        AVC,
        [StringValue("MVC")]
        MVC,
        [StringValue("HEVC")]
        HEVC,
        [StringValue("VC-1")]
        VC1,
        [StringValue("MPEG")]
        MPEG,
        [StringValue("DIRAC")]
        DIRAC,
        [StringValue("THEORA")]
        THEORA
    }
}