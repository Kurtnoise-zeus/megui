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
using System.ComponentModel;
using System.Xml.Serialization;

namespace MeGUI
{
    [Serializable]
    public struct Chapter
    {
        public string Name { get; set; }

        [XmlIgnore]
        public TimeSpan Time { get; set; }

        public Chapter(Chapter oOther)
        {
            Name = oOther.Name;
            Time = oOther.Time;
        }

        // XmlSerializer does not support TimeSpan, so use this property for serialization instead
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public long TimeTicks
        {
            get { return Time.Ticks; }
            set { Time = new TimeSpan(value); }
        }

        //public string Lang { get; set; }
        public override string ToString()
        {
            return Time.ToString() + ": " + Name;
        }

        public void SetTimeBasedOnString(string strTimeCode)
        {
            if (strTimeCode.Length > 16)
                strTimeCode = strTimeCode.Substring(0, 16);

            if (TimeSpan.TryParse(strTimeCode, new System.Globalization.CultureInfo("en-US"), out TimeSpan result))
                Time = result;
        }

        public static Chapter ChangeChapterFPS(Chapter oChapter, double fpsIn, double fpsOut)
        {
            if (fpsIn == fpsOut || fpsIn == 0 || fpsOut == 0)
                return oChapter;

            double frames = oChapter.Time.TotalSeconds * fpsIn;
            return new Chapter() { Name = oChapter.Name, Time = new TimeSpan((long)Math.Round(frames / fpsOut * TimeSpan.TicksPerSecond)) };
        }
    }
}