// ****************************************************************************
// 
// Copyright (C) 2005-2025 Doom9 & al
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
using System.Text;

namespace MeGUI
{
    public class hfyuSettings : VideoCodecSettings
    {
        public static string ID = "Huffyuv";

        public hfyuSettings() : base(ID, VideoEncoderType.HFYU)
        {
            base.BitrateQuantizer = 0;
            base.VideoEncodingType = VideoEncodingMode.CQ;
        }

        public override bool UsesSAR
        {
            get { return false; }
        }
    }
}
