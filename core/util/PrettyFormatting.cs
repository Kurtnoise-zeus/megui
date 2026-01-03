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
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace MeGUI.core.util
{
    public class PrettyFormatting
    {
        public static string ExtractWorkingName(string fileName, string strPrefix, string strSuffix, string strReplace, string strReplaceWith)
        {
            string A = Path.GetFileNameWithoutExtension(fileName); // In case they all fail

            int count = 0;
            while (!string.IsNullOrEmpty(Path.GetDirectoryName(fileName)) && count < 3)
            {
                string temp = Path.GetFileNameWithoutExtension(fileName);
                // Fix to only assume extracted DVD source in fileName starts with video_/vts_/audio_
                if (!temp.StartsWith("vts_") && !temp.StartsWith("video_") && !temp.StartsWith("audio_"))
                {
                    // we could potentially stop at video/audio
                    int idxVideo = temp.IndexOf("video");
                    int idxAudio = temp.IndexOf("audio");
                    if (idxVideo < 3) // Too close to start, ignore
                        idxVideo = temp.Length;
                    if (idxAudio < 3)
                        idxAudio = temp.Length;
                    A = temp.Substring(0, Math.Min(idxVideo, idxAudio)).Trim();
                    break;
                }
                fileName = Path.GetDirectoryName(fileName);
                count++;
            }

            // Format it nicely:
            if (!String.IsNullOrEmpty(strReplace))
                A = A.Replace(strReplace, strReplaceWith);
            return strPrefix + A + strSuffix;
        }

        private static readonly Regex delayRegex = new Regex("(?<match>-?[0-9]+)ms");

        /// <summary>
        /// Gets the delay from the filename, but warns the user if this delay is larger than
        /// 10 seconds.
        /// </summary>
        /// <param name="filename"></param>
        /// <returns>The delay, or null if no valid delay was found</returns>
        public static int? getDelayAndCheck(string filename)
        {
            int? delay = getDelay(filename);
            
            if (delay.HasValue && Math.Abs(delay.Value) > 10000)
            {
                if (MessageBox.Show(string.Format("Your input filename suggests the delay is {0}ms ({1}s), " +
                    "which is surprisingly large. Try checking the tool used to create this file to see " +
                    "if it got the delay wrong.\n\nAre you sure this delay is correct?", delay, (delay / 1000)),
                    "Very large delay", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                    delay = null;
            }
            
            return delay;
        }

        /// <summary>
        /// gets the delay from an audio filename
        /// </summary>
        /// <param name="fileName">file name to be analyzed</param>
        /// <returns>the delay in milliseconds</returns>
        public static int? getDelay(string fileName)
        {
            try
            {
                return int.Parse(delayRegex.Match(fileName).Groups["match"].Value);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// replaces the delay in the audio filename with a new delay
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public static string ReplaceDelay(string fileName, int delay)
        {
            return delayRegex.Replace(fileName, delay + "ms", 1);
        }

        /// <summary>
        /// replace the fps value by something more mkvmerge compliant
        /// </summary>
        /// <param name="fpsIn">the input fps</param>
        /// <returns>the output fps formated</returns>
        public static string ReplaceFPSValue(string fpsIn, int fps_n, int fps_d)
        {
             if (fps_n > 0 && fps_d > 0)
             {
                return fps_n + "/" + fps_d;
             }      
            
            string fpsOut;
            fpsIn = fpsIn.Replace(',', '.');

            switch (fpsIn)
            {
                case "23.976": fpsOut = "24000/1001"; break;
                case "24.000": fpsOut = "24"; break;
                case "25.000": fpsOut = "25"; break;
                case "29.970": fpsOut = "30000/1001"; break;
                case "30.000": fpsOut = "30"; break;
                case "50.000": fpsOut = "50"; break;
                case "59.940": fpsOut = "60000/1001"; break;
                case "60.000": fpsOut = "60"; break;
                default      : fpsOut = fpsIn; break;
            }

            return fpsOut;
        }
    }
}
