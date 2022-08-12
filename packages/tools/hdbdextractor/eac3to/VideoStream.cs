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
using MeGUI.core.util;

namespace eac3to
{
    /// <summary>A Stream of StreamType Video</summary>
    public class VideoStream : Stream
    {
        public VideoStreamType VideoType { get; set; }
        public override string Language { get; set; }

        public override object[] ExtractTypes
        {
            get
            {
                switch (VideoType)
                {
                    case VideoStreamType.AVC:
                        return new object[] { "MKV", "H264" };
                    case VideoStreamType.MVC:
                        return new object[] { "H264" };
                    case VideoStreamType.HEVC:
                        return new object[] { "H265" };
                    case VideoStreamType.VC1:
                        return new object[] { "MKV", "VC1" };
                    case VideoStreamType.MPEG:
                        return new object[] { "MKV", "M2V" };
                    case VideoStreamType.THEORA:
                        return new object[] { "MKV", "OGG" };
                    case VideoStreamType.DIRAC:
                        return new object[] { "MKV", "DRC" };
                    default:
                        return new object[] { "UNKNOWN" };
                }
            }
        }

        public VideoStream(string s, LogItem _log) : base(StreamType.Video, s, _log)
        {
            if (string.IsNullOrEmpty(s))
                throw new ArgumentNullException("s", "The string 's' cannot be null or empty.");
        }

        new public static Stream Parse(string s, LogItem _log)
        {
            //3: VC-1, 1080p24 /1.001 (16:9) with pulldown flags

            if (string.IsNullOrEmpty(s))
                throw new ArgumentNullException("s", "The string 's' cannot be null or empty.");
 
            string type = s.Substring(s.IndexOf(":") + 1, s.IndexOf(',') - s.IndexOf(":") - 1).Trim().Split(' ')[0];
            VideoStream videoStream = new VideoStream(s, _log);
            switch (type.ToUpperInvariant())
            {
                case "H264/AVC":
                    videoStream.VideoType = VideoStreamType.AVC;
                    break;
                case "H264/MVC":
                    videoStream.VideoType = VideoStreamType.MVC;
                    break;
                case "H265/HEVC":
                    videoStream.VideoType = VideoStreamType.HEVC;
                    break;
                case "VC-1":
                    videoStream.VideoType = VideoStreamType.VC1;
                    break;
                case "MPEG":
                case "MPEG2":
                    videoStream.VideoType = VideoStreamType.MPEG;
                    break;
                case "THEORA":
                    videoStream.VideoType = VideoStreamType.THEORA;
                    break;
                case "DIRAC":
                    videoStream.VideoType = VideoStreamType.DIRAC;
                    break;
                default:
                    _log.Warn("\"" + type + "\" is not known. " + s);
                    videoStream.VideoType = VideoStreamType.UNKNOWN;
                    break;
            }
            return videoStream;
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}