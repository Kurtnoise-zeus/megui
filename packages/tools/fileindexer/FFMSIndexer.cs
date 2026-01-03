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

using MeGUI.core.util;

namespace MeGUI
{
    public class FFMSIndexer : CommandlineJobProcessor<FFMSIndexJob>
    {
        public static readonly JobProcessorFactory Factory =
                    new JobProcessorFactory(new ProcessorFactory(init), "FFMSIndexer");

        private static IJobProcessor init(MainForm mf, Job j)
        {
            if (j is FFMSIndexJob) return new FFMSIndexer(mf.Settings.FFMS.Path);
            return null;
        }

        public FFMSIndexer(string executableName)
        {
            UpdateCacher.CheckPackage("ffms");
            Executable = executableName;
        }

        public override void ProcessLine(string line, StreamType stream, ImageType oType)
        {
            if (Regex.IsMatch(line, "^Indexing, please wait... [0-9]{1,3}%", RegexOptions.Compiled))
            {
                Su.PercentageCurrent = Int32.Parse(line.Substring(25).Split('%')[0]);
                Su.Status = "Creating FFMS index...";
                return;
            }

            if (Regex.IsMatch(line, "^Writing index...", RegexOptions.Compiled))
                Su.Status = "Writing FFMS index...";
            base.ProcessLine(line, stream, oType);
        }

        protected override string Commandline
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                if (Job.DemuxMode > 0)
                {
                    if (Job.AudioTracks.Count == 1)
                        sb.Append("-t 1 "); // force to 1 when we have only one audio stream, otherwise we get audio decoding error msg
                    else
                        sb.Append("-t -1 ");
                }
                sb.Append("-f \"" + Job.Input + "\"");
                if (!String.IsNullOrEmpty(Job.Output))
                    sb.Append(" \"" + Job.Output + "\"");
                return sb.ToString();
            }
        }

        protected override void checkJobIO()
        {
            try
            {
                if (!String.IsNullOrEmpty(Job.Output))
                {
                    FileUtil.ensureDirectoryExists(Path.GetDirectoryName(Job.Output));
                    if (File.Exists(Job.Output))
                        File.Delete(Job.Output);
                }
            }
            finally
            {
                base.checkJobIO();
            }
            Su.Status = "Creating FFMS index...";
        }

        protected override void doExitConfig()
        {
            if (Job.DemuxMode > 0 && !Su.HasError && !Su.WasAborted && Job.AudioTracks.Count > 0)
            {
                int iTracksFound = 0;
                int iCurrentAudioTrack = -1;
                for (int iCurrentTrack = 0; iCurrentTrack <= 29; iCurrentTrack++) // hard limit to max. 30 tracks
                {
                    StringBuilder strAVSScript = new StringBuilder();
                    strAVSScript.Append(VideoUtil.getFFMSAudioInputLine(Job.Input, Job.Output, iCurrentTrack, false));

                    // is this an audio track?
                    string strErrorText;
                    if (AudioUtil.AVSScriptHasAudio(strAVSScript.ToString(), out strErrorText) == false)
                        continue;
                    iCurrentAudioTrack++;

                    foreach (AudioTrackInfo oAudioTrack in Job.AudioTracks)
                    {
                        if (oAudioTrack.TrackIndex != iCurrentAudioTrack)
                            continue;

                        // write avs file
                        string strAudioAVSFile;
                        strAudioAVSFile = Path.GetFileNameWithoutExtension(Job.Output) + "_track_" + (oAudioTrack.TrackIndex + 1) + "_" + oAudioTrack.Language.ToLower(System.Globalization.CultureInfo.InvariantCulture) + ".avs";
                        strAudioAVSFile = Path.Combine(Path.GetDirectoryName(Job.Output), Path.GetFileName(strAudioAVSFile));
                        try
                        {
                            strAVSScript.AppendLine(@"# detected channels: " + oAudioTrack.NbChannels);
                            strAVSScript.Append(@"# detected channels layout: " + oAudioTrack.ChannelPositions);
                            StreamWriter oAVSWriter = new StreamWriter(strAudioAVSFile, false, Encoding.Default);
                            oAVSWriter.Write(strAVSScript);
                            oAVSWriter.Close();
                        }
                        catch (Exception ex)
                        {
                            log.LogValue("Error creating audio AVS file", ex);
                        }
                        break;
                    }
                    if (++iTracksFound == Job.AudioTracks.Count)
                        break;
                }
            }
            base.doExitConfig();
        }
    }
}
