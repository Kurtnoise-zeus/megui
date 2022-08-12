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
using MeGUI.packages.tools.hdbdextractor;

namespace eac3to
{
    /// <summary>An enumeration of Subtitle Stream types</summary>
    public enum SubtitleStreamType
    {
        [StringValue("UNKNOWN")]
        UNKNOWN,
        [StringValue("ASS")]
        ASS,
        [StringValue("SSA")]
        SSA,
        [StringValue("SRT")]
        SRT,
        [StringValue("IDX")]
        IDX,
        [StringValue("SUP")]
        SUP
    }

    /// <summary>A Stream of StreamType Subtitle</summary>
    public class SubtitleStream : Stream
    {
        public SubtitleStreamType SubtitleType { get; set; }
        public override string Language { get; set; }
        public bool IsSDH { get; set; }

        public override object[] ExtractTypes
        {
            get
            {
                switch (SubtitleType)
                {
                    case SubtitleStreamType.ASS:
                        return new object[] { "ASS" };
                    case SubtitleStreamType.SSA:
                        return new object[] { "SSA" };
                    case SubtitleStreamType.SRT:
                        return new object[] { "SRT" };
                    case SubtitleStreamType.SUP:
                        return new object[] { "SUP" };
                    default:
                        return new object[] { "UNKNOWN" };
                }
            }
        }

        public SubtitleStream(string s, LogItem _log) : base(StreamType.Subtitle, s, _log)
        {
            if (string.IsNullOrEmpty(s))
                throw new ArgumentNullException("s", "The string 's' cannot be null or empty.");
        }

        new public static Stream Parse(string s, LogItem _log)
        {
            //5: Subtitle, English, "SDH"
            //6: Subtitle, French
            //7: Subtitle, Spanish

            if (string.IsNullOrEmpty(s))
                throw new ArgumentNullException("s", "The string 's' cannot be null or empty.");

            string type = s.Substring(s.IndexOf(":") + 12, 3).Trim();
            SubtitleStream subtitleStream = new SubtitleStream(s, _log);
            switch (type.ToUpperInvariant())
            {
                case "ASS":
                    subtitleStream.SubtitleType = SubtitleStreamType.ASS; break;
                case "SSA":
                    subtitleStream.SubtitleType = SubtitleStreamType.SSA; break;
                case "SRT":
                    subtitleStream.SubtitleType = SubtitleStreamType.SRT; break;
                case "PGS":
                    subtitleStream.SubtitleType = SubtitleStreamType.SUP; break;
                case "VOB":
                    subtitleStream.SubtitleType = SubtitleStreamType.UNKNOWN; break;
                default:
                    _log.Warn("\"" + type + "\" is not known. " + s);
                    subtitleStream.SubtitleType = SubtitleStreamType.UNKNOWN;
                    break;
            }

            subtitleStream.IsSDH = s.Contains("\"SDH\"") ? true : false;
            return subtitleStream;
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}