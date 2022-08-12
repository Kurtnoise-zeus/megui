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
using System.Collections.Generic;
using System.Text;

using MeGUI.packages.tools.hdbdextractor;

namespace eac3to
{
    /// <summary>A Feature</summary>
    public class Feature
    {
        public int Number { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public TimeSpan Duration { get; set; }
        public List<File> Files { get; set; }
        public List<Stream> Streams { get; set; }

        public Feature()
        {
            Number = 0;
            Name = string.Empty;
            Description = string.Empty;
            Duration = new TimeSpan(0, 0, 0);
            Files = new List<File>();
            Streams = new List<Stream>();
        }

        /// <summary>Converts a string representation of a Feature into a Feature equivalent.</summary>
        /// <param name="s">A string containing a line of characters from the StdOut of eac3to</param>
        /// <returns></returns>
        public static Feature Parse(string s)
        {
            /*
            // Blu-Ray
            1) 00001.mpls, 00011.m2ts, 1:53:06
               - h264/AVC, 1080p24 /1.001 (16:9)
               - TrueHD, English, multi-channel, 48khz
               - TrueHD, French, multi-channel, 48khz
               - AC3, Spanish, multi-channel, 48khz
               - AC3, Thai, multi-channel, 48khz
               - AC3, Portuguese, multi-channel, 48khz
               - AC3, English, stereo, 48khz

            2) 00216.mpls, 0:50:19
               [99+100+101+102+103+104+105+106+114].m2ts
               - h264/AVC, 1080i60 /1.001 (16:9)
               - AC3, English, stereo, 48khz
            // HD-DVD
            1) Alternate_feature_01.EVO+Alternate_feature_02.EVO, 1:43:48
               "director"
               - VC-1, 1080p (16:9)
               - TrueHD, English, 5.1, 48khz

            2) Theatrical_feature_01.EVO+Theatrical_feature_02.EVO, 1:40:26
               "feature"
               - VC-1, 1080p (16:9)
               - TrueHD, English, 5.1, 48khz
               - E-AC3, French, 5.1, 48khz
               - E-AC3, Spanish, 5.1, 48khz

            3) default.EVO, 0:00:12
               "pickFeature"
               - VC-1, 1080p (16:9)             

            */

            if (string.IsNullOrEmpty(s))
                throw new ArgumentNullException("s", "The string 's' cannot be null or empty.");

            Feature feature = new Feature();

            feature.Number = int.Parse(s.Substring(0, s.IndexOf(")")));
            feature.Name = s.Substring(s.IndexOf(")") + 1, s.IndexOf(",") - s.IndexOf(")") - 1).Trim();
            feature.Description = s.Substring(s.IndexOf(")") + 1).Trim();
            feature.Duration = TimeSpan.Parse(s.Substring(s.LastIndexOf(',') + 1).Trim());

            if (s.ToUpper(System.Globalization.CultureInfo.InvariantCulture).Contains(".M2TS,")) // Blu-ray
                foreach (string file in s.Substring(s.IndexOf(",") + 1, s.LastIndexOf(',') - s.IndexOf(",") - 1).Split("+".ToCharArray()))
                    feature.Files.Add(new File(file.Trim(), feature.Files.Count + 1));
            else if (s.ToUpper(System.Globalization.CultureInfo.InvariantCulture).Contains(".EVO,")) // HD-DVD
                foreach (string file in s.Substring(s.IndexOf(")") + 1, s.IndexOf(',') - s.IndexOf(")") - 1).Split("+".ToCharArray()))
                    feature.Files.Add(new File(file.Trim(), feature.Files.Count + 1));

            return feature;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("Number:\t{0}{3}Name:\t{1}{3}Duration:\t{2}{3}File(s):{3}", Number, Name, Duration, Environment.NewLine);

            foreach(File file in Files)
                sb.AppendFormat("\t{0}{1}", file.ToString(), Environment.NewLine);

            sb.AppendFormat("Stream(s): {0}", Environment.NewLine);

            if(Streams.Count == 0)
                sb.Append("\tRetrieve streams to view their details.");
            else
                foreach (Stream stream in Streams)
                    sb.AppendFormat("\t{0}{1}", stream.ToString(), Environment.NewLine);

            return sb.ToString();
        }
    }
}
