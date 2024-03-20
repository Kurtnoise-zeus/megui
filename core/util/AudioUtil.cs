// ****************************************************************************
// 
// Copyright (C) 2005-2024 Doom9 & al
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

using MeGUI.core.util;

namespace MeGUI
{
    /// <summary>
    /// AudioUtil is used to perform various audio related tasks
    /// </summary>
    public static class AudioUtil
    {
        /// <summary>
        /// returns all audio streams that can be encoded or muxed
        /// </summary>
        /// <returns></returns>
        public static AudioJob[] getConfiguredAudioJobs(AudioJob[] audioStreams)
        {
            List<AudioJob> list = new List<AudioJob>();
            if (audioStreams == null)
                return list.ToArray();
            
            foreach (AudioJob stream in audioStreams)
            {
                if (String.IsNullOrEmpty(stream.Input))
                {
                    // no audio is ok, just skip
                    break;
                }
                list.Add(stream);

            }
            return list.ToArray();
        }

        public static bool AVSScriptHasAudio(String strAVSScript, out string strErrorText)
        {
            try
            {
                strErrorText = String.Empty;
                using (AviSynthClip a = AviSynthScriptEnvironment.ParseScript(strAVSScript))
                if (a.ChannelsCount == 0)
                    return false;
                return true;
            }
            catch (Exception ex)
            {
                strErrorText = ex.Message;
                return false;
            }
        }

        public static bool AVSFileHasAudio(String strAVSScript)
        {
            try
            {
                if (!Path.GetExtension(strAVSScript).ToLowerInvariant().Equals(".avs"))
                    return false;
                using (AviSynthClip a = AviSynthScriptEnvironment.OpenScriptFile(strAVSScript))
                if (!a.HasAudio)
                    return false;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static int AVSFileChannelCount(String strAVSScript)
        {
            try
            {
                if (!Path.GetExtension(strAVSScript).ToLowerInvariant().Equals(".avs"))
                    return 0;
                using (AviSynthClip a = AviSynthScriptEnvironment.OpenScriptFile(strAVSScript))
                    return a.ChannelsCount;
            }
            catch
            {
                return 0;
            }
        }

        public static string getChannelPositionsFromAVSFile(String strAVSFile)
        {
            string strChannelPositions = String.Empty;
            
            try
            {
                string line;
                using (StreamReader file = new StreamReader(strAVSFile))
                {
                    while ((line = file.ReadLine()) != null)
                    {
                        if (line.IndexOf(@"# detected channel positions: ") == 0)
                        {
                            strChannelPositions = line.Substring(30);
                            break;
                        }
                    }
                }
                return strChannelPositions;
            }
            catch
            {
                return strChannelPositions;
            }
        }

        public static string getChannelCountFromAVSFile(String strAVSFile)
        {
            string strChannelCount = String.Empty;

            try
            {
                string line;
                using (StreamReader file = new StreamReader(strAVSFile))
                {
                    while ((line = file.ReadLine()) != null)
                    {
                        if (line.IndexOf(@"# detected channels: ") == 0)
                        {
                            strChannelCount = line.Substring(21);
                            break;
                        }
                    }
                }
                return strChannelCount;
            }
            catch
            {
                return strChannelCount;
            }
        }

        /// <summary>
		/// gets all demuxed audio files from a given project
		/// starts with the first file and returns the desired number of files
        /// </summary>
        /// <param name="audioTracks">list of audio tracks</param>
        /// <param name="arrDeleteFiles">files to be deleted afterwards</param>
        /// <param name="projectName">the project name</param>
        /// <param name="log">log item</param>
        /// <returns></returns>
        public static Dictionary<int, string> GetAllDemuxedAudioFromDGI(List<AudioTrackInfo> audioTracks, out List<string> arrDeleteFiles, string projectName, LogItem log)
        {
            Dictionary<int, string> audioFiles = new Dictionary<int, string>();         // files to be used 
            Dictionary<int, string> audioFilesDemuxed = new Dictionary<int, string>();  // files demuxed by DGI/M
            arrDeleteFiles = new List<string>();                                        // files to be deleted

            // get the demuxed files from the log file
            if (File.Exists(Path.ChangeExtension(projectName, ".log")))
            {
                string line;
                using (StreamReader file = new StreamReader(Path.ChangeExtension(projectName, ".log")))
                {
                    while ((line = file.ReadLine()) != null)
                    {
                        if (!FileUtil.RegExMatch(line, @"^\d+: ", false))
                            continue;

                        string strFile = line.Substring(line.IndexOf(':') + 1).Trim();
                        if (!File.Exists(strFile))
                            continue;

                        arrDeleteFiles.Add(strFile);
                        audioFilesDemuxed.Add(Int32.Parse(line.Substring(0, line.IndexOf(':'))), strFile);
                    }
                }
            }

            if (arrDeleteFiles.Count == 0)
            {
                // fallback to the old way how to get demuxed files as the log file either does not exist or does not contain any files
                audioFiles = VideoUtil.getAllDemuxedAudio(audioTracks, new List<AudioTrackInfo>(), out arrDeleteFiles, projectName, null);
                return audioFiles;
            }

            // no need to assign files if no track should be extracted
            if (audioTracks == null || audioTracks.Count == 0)
                return audioFiles;

            for (int counter = 0; counter < audioTracks.Count; counter++)
            {
                if (audioFilesDemuxed.ContainsKey(audioTracks[counter].TrackID))
                {
                    audioFilesDemuxed.TryGetValue(audioTracks[counter].TrackID, out string strFile);
                    audioFiles.Add(audioTracks[counter].TrackID, strFile);
                    arrDeleteFiles.Remove(strFile);
                }
                else
                    log?.LogEvent("Audio track not found: " + audioTracks[counter].TrackID, ImageType.Error);
            }

            return audioFiles;
        }
    }
}