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

namespace MeGUI
{
    /// <summary>
    /// SubtitleUtil is used to perform various subtitle tasks
    /// </summary>
    public static class SubtitleUtil
    {
        /// <summary>
        /// Adds or removes the forced track name specified in the global settings
        /// </summary>
        /// <param name="bForced">true if the track is a forced one</param>
        /// <param name="strTrackName">the track name</param>
        /// <returns>the new track name</returns>
        public static string ApplyForcedStringToTrackName(bool bForced, string strTrackName)
        {
            string strForceName = MainForm.Instance.Settings.AppendToForcedStreams;
            if (String.IsNullOrEmpty(strForceName))
                return strTrackName;

            if (strTrackName == null)
                strTrackName = string.Empty;
            
            if (bForced)
            {
                if (strTrackName.ToLowerInvariant().EndsWith(strForceName.ToLowerInvariant()))
                    strTrackName = strTrackName.Substring(0, strTrackName.Length - strForceName.Length).TrimEnd();
                if (!String.IsNullOrEmpty(strTrackName) && !strTrackName.EndsWith(" "))
                    strTrackName += " ";
                strTrackName += strForceName;
            }
            else if (!bForced && strTrackName.ToLowerInvariant().EndsWith(strForceName.ToLowerInvariant()))
            {
                strTrackName = strTrackName.Substring(0, strTrackName.Length - strForceName.Length).TrimEnd();
            }
            return strTrackName;
        }
    }
}