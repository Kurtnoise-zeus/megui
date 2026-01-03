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

using MeGUI.core.util;

namespace MeGUI.packages.video.svtav1
{
    class svtav1SettingsHandler
    {
        private svtav1Settings _xs;
        private LogItem _log;
        
        public svtav1SettingsHandler(svtav1Settings xs, LogItem log)
		{
            _xs = xs;
            _log = log;
        }

        /// <summary>
        /// Calculates the --frames value
        /// </summary>
        /// <returns>the --frames value</returns>
        public void getFrames(ref ulong frames)
        {
            if (_log == null)
                return;

            string strCustomValue = "";
            ulong seek = 0;
            if (extractCustomCommand("seek", out strCustomValue))
            {
                ulong iTemp = seek;
                if (ulong.TryParse(strCustomValue, out iTemp))
                {
                    seek = iTemp;
                    if (seek >= frames)
                    {
                        seek = 0;
                        _log.LogEvent("removing --seek as the seek value outreaches the frame number");
                    }
                    else
                    {
                        _xs.CustomEncoderOptions += " --seek " + seek;
                        frames -= seek;  // reduce by the number of seek frames
                    }
                }
            }

            strCustomValue = string.Empty;
            ulong framesCustom = frames;
            if (extractCustomCommand("frames", out strCustomValue))
            {
                ulong iTemp = frames;
                if (ulong.TryParse(strCustomValue, out iTemp))
                {
                    framesCustom = iTemp;
                    if (framesCustom > frames)
                    {
                        _log.LogEvent("reducing --frames as required by the selected source");
                        framesCustom = frames;
                    }
                    else
                        frames = framesCustom;
                }
            }

            return;
        }

        /// <summary>
        /// returns the value for the selected command in the custom command line 
        /// and removes the command from the custom command line
        /// </summary>
        /// <param name="strCommandToExtract">the command to extract without --</param>
        /// <param name="strCommandValue">the value of the command</param>
        /// <returns>true if the command can be found</returns>
        private bool extractCustomCommand(string strCommandToExtract, out string strCommandValue)
        {
            string strNewCommandLine = "";
            bool bFound = false;
            strCommandValue = String.Empty;

            // add a leading space for easier command detection
            _xs.CustomEncoderOptions = " " + _xs.CustomEncoderOptions;

            // custom command line splitted bei either " -" or " --"
            foreach (string strCommandTemp in System.Text.RegularExpressions.Regex.Split(_xs.CustomEncoderOptions, @"(?= --[a-zA-Z]{1}| -[a-zA-Z]{1})"))
            {
                if (string.IsNullOrEmpty(strCommandTemp.Trim()))
                    continue;
                string strCommand = strCommandTemp;

                string strLeading = string.Empty;
                if (strCommand.StartsWith(" --"))
                    strLeading = " --";
                else if (strCommand.StartsWith(" -"))
                    strLeading = " -";
                strCommand = strCommand.Substring(strLeading.Length);

                if (strCommand.Trim().ToLowerInvariant().StartsWith(strCommandToExtract.ToLowerInvariant()))
                {
                    strCommandValue = strCommand.Substring(strCommandToExtract.Length).Trim();
                    bFound = true;
                }
                else
                    strNewCommandLine += strLeading + strCommand;
            }

            _xs.CustomEncoderOptions = strNewCommandLine.Trim();
            return bFound;
        }

        /// <summary>
        /// returns the custom command line 
        /// </summary>
        /// <returns>the custom command line</returns>
        public string getCustomCommandLine()
        {
            return _xs.CustomEncoderOptions;
        }
    }
}