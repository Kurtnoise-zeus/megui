//============================================================================
// BDInfo - Blu-ray Video and Audio Analysis Tool
// Copyright © 2010 Cinema Squid
//
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2.1 of the License, or (at your option) any later version.
//
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
//=============================================================================

using System;
using System.Collections.Generic;
using System.Text;

namespace BDInfo
{
    public class TSStreamClip
    {
        public int AngleIndex = 0;
        public string Name;
        public double TimeIn;
        public double TimeOut;
        public double RelativeTimeIn;
        public double RelativeTimeOut;
        public double Length;

        public ulong FileSize = 0;
        public ulong InterleavedFileSize = 0;
        public ulong PayloadBytes = 0;
        public ulong PacketCount = 0;
        public double PacketSeconds = 0;

        public List<double> Chapters = new List<double>();
        public TSStreamClipFile StreamClipFile = null;

        public TSStreamClip(TSStreamClipFile streamClipFile)
        {
            Name = streamClipFile.Name;
            StreamClipFile = streamClipFile;
        }
    }
}