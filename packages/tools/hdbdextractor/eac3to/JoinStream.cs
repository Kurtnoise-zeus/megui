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
    /// <summary>A Stream of StreamType Join</summary>
    public class JoinStream : Stream
    {
        public override object[] ExtractTypes
        {
            get
            {
                switch (base.Name.ToString())
                {
                    case "Joined EVO":
                        return new object[] { "EVO" };
                    case "Joined VOB":
                        return new object[] { "VOB" };
                    default:
                        return new object[] { "" };
                }
            }
        }

        public JoinStream(string s, LogItem _log) : base(StreamType.Join, s, _log)
        {
            if (string.IsNullOrEmpty(s))
                throw new ArgumentNullException("s", "The string 's' cannot be null or empty.");
        }

        new public static Stream Parse(string s, LogItem _log)
        {
            //1: Joined EVO file

            if (string.IsNullOrEmpty(s))
                throw new ArgumentNullException("s", "The string 's' cannot be null or empty.");

            return new JoinStream(s, _log);
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}