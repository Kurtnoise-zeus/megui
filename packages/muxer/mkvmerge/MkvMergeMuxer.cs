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
using System.Text;

using MeGUI.core.details;
using MeGUI.core.util;

namespace MeGUI
{
    class MkvMergeMuxer : CommandlineMuxer
    {
        public static readonly JobProcessorFactory Factory = 
            new JobProcessorFactory(new ProcessorFactory(init), "MkvMergeMuxer");
        
        private static IJobProcessor init(MainForm mf, Job j)
        {
            if (j is MuxJob && (j as MuxJob).MuxType == MuxerType.MKVMERGE)
                return new MkvMergeMuxer(mf.Settings.MkvMerge.Path);
            return null;
        }
        
        public MkvMergeMuxer(string executablePath)
        {
            UpdateCacher.CheckPackage("mkvmerge");
            this.executable = executablePath;

            // Exit code 0 = everything was OK
            // Exit code 1 = there were non-fatal warnings
            // Exit code 2 = there was a fatal error
            this.arrSuccessCodes.Add(1);
        }

        #region line processing
        /// <summary>
        /// gets the framenumber from an mkvmerge status update line
        /// </summary>
        /// <param name="line">mkvmerge commandline output</param>
        /// <returns>the framenumber included in the line</returns>
        public decimal? getPercentage(string line)
        {
            try
            {
                int percentageStart = 10;
                int percentageEnd = line.IndexOf("%");
                string frameNumber = line.Substring(percentageStart, percentageEnd - percentageStart).Trim();
                return Int32.Parse(frameNumber);
            }
            catch (Exception e)
            {
                log.LogValue("Exception in getPercentage(" + line + ")", e, ImageType.Warning);
                return null;
            }
        }
        #endregion

        protected override void checkJobIO()
        {
            su.Status = "Muxing MKV...";
            base.checkJobIO();
        }

        public override void ProcessLine(string line, StreamType stream, ImageType oType)
        {
            if (line.StartsWith("Progress: ")) //status update
            {
                su.PercentageDoneExact = getPercentage(line);
                return;
            }
            
            if (line.StartsWith("Error: "))
                oType = ImageType.Error;
            else if (line.StartsWith("Warning: "))
                oType = ImageType.Warning;
            base.ProcessLine(line, stream, oType);
        }

        protected override void setProjectedFileSize()
        {
            if (!job.Settings.MuxAll)
                base.setProjectedFileSize();
        }

        protected override string Commandline
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                MuxSettings settings = job.Settings;
                int trackID;
                
                sb.Append("--output \"" + settings.MuxedOutput + "\"");

                if (settings.MuxAll)
                {
                    string strInput = string.Empty;
                    if (!string.IsNullOrEmpty(settings.VideoInput))
                        strInput = settings.VideoInput;
                    else if (!string.IsNullOrEmpty(settings.MuxedInput))
                        strInput = settings.MuxedInput;
                    MediaInfoFile oVideoInfo = new MediaInfoFile(strInput, ref log);
                    if (oVideoInfo.ContainerFileType == ContainerType.MP4 || oVideoInfo.ContainerFileType == ContainerType.MKV)
                        trackID = oVideoInfo.VideoInfo.Track.MMGTrackID;
                    else
                        trackID = 0;

                    sb.Append(" \"" + strInput + "\" --ui-language en");
                    return sb.ToString();
                }

                if(!string.IsNullOrEmpty(settings.VideoInput) || !string.IsNullOrEmpty(settings.MuxedInput))
                {
                    string inputFile = settings.VideoInput;
                    if (string.IsNullOrEmpty(settings.VideoInput))
                        inputFile = settings.MuxedInput;

                    MediaInfoFile oVideoInfo = new MediaInfoFile(inputFile, ref log);
                    if (oVideoInfo.ContainerFileType == ContainerType.MP4 || oVideoInfo.ContainerFileType == ContainerType.MKV)
                        trackID = oVideoInfo.VideoInfo.Track.MMGTrackID;
                    else
                        trackID = 0;

                    if (settings.DAR.HasValue)
                        sb.Append(" --aspect-ratio " + trackID + ":" + settings.DAR.Value.X + "/" + settings.DAR.Value.Y);
                    else
                        sb.Append(" --engage keep_bitstream_ar_info"); // assuming that SAR info is already in the stream...
                    if (!string.IsNullOrEmpty(settings.VideoName))
                        sb.Append(" --track-name \"" + trackID + ":" + settings.VideoName.Replace("\"", "\\\"") + "\"");
                    if (oVideoInfo.VideoInfo.Codec == VideoCodec.UNKNOWN || oVideoInfo.VideoInfo.Codec != VideoCodec.AVC || oVideoInfo.VideoInfo.ScanType.ToLowerInvariant().Equals("progressive"))
                    {
                        string fpsString = String.Format("{0:##.###}", oVideoInfo.VideoInfo.FPS);
                        if (settings.Framerate.HasValue)
                            fpsString = String.Format("{0:##.###}", settings.Framerate.Value);
                        if (!String.IsNullOrEmpty(fpsString))
                            sb.Append(" --default-duration " + trackID + ":" + PrettyFormatting.ReplaceFPSValue(fpsString) + "fps");
                    }
                    string timeStampFile = inputFile + ".timestamps.txt";
                    if (!String.IsNullOrEmpty(settings.TimeStampFile) || File.Exists(timeStampFile))
                        sb.Append(" --timestamps " + trackID + ":\"" + (File.Exists(timeStampFile) ? timeStampFile : settings.TimeStampFile) + "\"");
                    sb.Append(" --video-tracks " + trackID + " --no-audio --no-subtitles --no-chapters");
                    if (settings.Attachments.Count > 0)
                        sb.Append(" --no-attachments");
                    sb.Append(" \"" + inputFile + "\"");
                }

                // audio tracks
                foreach (object o in settings.AudioStreams)
                {
                    MuxStream stream = (MuxStream)o;
                    MediaInfoFile oAudioInfo = new MediaInfoFile(stream.path, ref log);

                    if (!oAudioInfo.HasAudio)
                    {
                        log.LogEvent("No audio track found: " + stream.path, ImageType.Warning);
                        continue;
                    }

                    if (oAudioInfo.ContainerFileType == ContainerType.MP4 || oAudioInfo.ContainerFileType == ContainerType.MKV)
                        trackID = oAudioInfo.AudioInfo.GetFirstTrackID();
                    else
                        trackID = 0;

                    if (oAudioInfo.ContainerFileType == ContainerType.MP4 || oAudioInfo.AudioInfo.Tracks[0].AudioCodec == AudioCodec.AAC)
                    {
                        int heaac_flag = -1;
                        if (oAudioInfo.AudioInfo.Tracks.Count > 0)
                            heaac_flag = oAudioInfo.AudioInfo.Tracks[0].AACFlag;
                        if (heaac_flag == 1)
                            sb.Append(" --aac-is-sbr " + trackID + ":1");
                        else if (heaac_flag == 0)
                            sb.Append(" --aac-is-sbr " + trackID + ":0");
                    }

                    if (!string.IsNullOrEmpty(stream.language))
                    {
                        foreach (KeyValuePair<string, string> strLanguage in LanguageSelectionContainer.Languages)
                        {
                            if (stream.language.ToLowerInvariant().Equals(strLanguage.Key.ToLowerInvariant()))
                            {
                                sb.Append(" --language " + trackID + ":" + strLanguage.Value);
                                break;
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(stream.name))
                        sb.Append(" --track-name \"" + trackID + ":" + stream.name.Replace("\"", "\\\"") + "\"");
                    if (stream.delay != 0)
                        sb.AppendFormat(" --sync {0}:{1}", trackID, stream.delay);
                    sb.Append(" --audio-tracks " + trackID + " --no-chapters --no-video --no-subtitles \"" + stream.path + "\"");
                }

                // subtitle tracks
                foreach (object o in settings.SubtitleStreams)
                {
                    MuxStream stream = (MuxStream)o;

                    trackID = 0;
                    if (File.Exists(stream.path))
                    {
                        MediaInfoFile oSubtitleInfo = new MediaInfoFile(stream.path, ref log);
                        if (oSubtitleInfo.ContainerFileType == ContainerType.MP4 || oSubtitleInfo.ContainerFileType == ContainerType.MKV)
                            trackID = oSubtitleInfo.SubtitleInfo.GetFirstTrackID();

                        FileInfo oFileInfo = new FileInfo(stream.path);
                        if (oFileInfo.Length < 20)
                        {
                            log.LogEvent("Ignoring subtitle track as it is too small: " + stream.path, ImageType.Warning);
                            break;
                        }
                    }

                    if (stream.MuxOnlyInfo != null)
                    {
                        trackID = stream.MuxOnlyInfo.MMGTrackID;
                        if (!string.IsNullOrEmpty(stream.MuxOnlyInfo.Language))
                            sb.Append(" --language " + trackID + ":" + stream.MuxOnlyInfo.Language);
                        if (!string.IsNullOrEmpty(stream.MuxOnlyInfo.Name))
                            sb.Append(" --track-name \"" + trackID + ":" + stream.MuxOnlyInfo.Name.Replace("\"", "\\\"") + "\"");
                        if (stream.delay != 0)
                            sb.AppendFormat(" --sync {0}:{1}", trackID, stream.delay);
                        if (stream.MuxOnlyInfo.DefaultTrack)
                            sb.Append(" --default-track " + trackID + ":yes");
                        else
                            sb.Append(" --default-track " + trackID + ":no");
                        if (stream.MuxOnlyInfo.ForcedTrack)
                            sb.Append(" --forced-track " + trackID + ":yes");
                        else
                            sb.Append(" --forced-track " + trackID + ":no");
                        sb.Append(" --subtitle-tracks " + trackID + " --no-video --no-audio --no-track-tags --no-global-tags --no-chapters \"" + stream.MuxOnlyInfo.SourceFileName + "\"");
                    }
                    else if (stream.path.ToLowerInvariant().EndsWith(".idx"))
                    {
                        List<SubtitleInfo> subTracks;
                        idxReader.readFileProperties(stream.path, out subTracks);
                        if (subTracks.Count == 0)
                        {
                            log.LogEvent("No subtitle track found: " + stream.path, ImageType.Warning);
                            continue;
                        }
                        foreach (SubtitleInfo strack in subTracks)
                        {
                            foreach (KeyValuePair<string, string> strLanguage in LanguageSelectionContainer.Languages)
                            {  
                                if (trackID == 0 && !string.IsNullOrEmpty(stream.language) 
                                    && stream.language.ToLowerInvariant().Equals(strLanguage.Key.ToLowerInvariant()))
                                {
                                    sb.Append(" --language " + trackID + ":" + strLanguage.Value);
                                    break;
                                }
                                else if (((trackID == 0 && string.IsNullOrEmpty(stream.language)) || trackID > 0)
                                    && LanguageSelectionContainer.LookupISOCode(strack.Name).ToLowerInvariant().Equals(strLanguage.Key.ToLowerInvariant()))
                                {
                                    sb.Append(" --language " + trackID + ":" + strLanguage.Value);
                                    break;
                                }
                            }
                            if (!string.IsNullOrEmpty(stream.name))
                                sb.Append(" --track-name \"" + trackID + ":" + stream.name.Replace("\"", "\\\"") + "\"");
                            if (stream.delay != 0)
                                sb.AppendFormat(" --sync {0}:{1}", trackID, stream.delay);
                            if (stream.bDefaultTrack && trackID == 0)
                                sb.Append(" --default-track 0:yes");
                            else
                                sb.Append(" --default-track " + trackID + ":no");
                            if (stream.bForceTrack)
                                sb.Append(" --forced-track " + trackID + ":yes");
                            else
                                sb.Append(" --forced-track " + trackID + ":no");
                            ++trackID;
                        }
                        trackID = 0;
                        sb.Append(" --subtitle-tracks ");
                        foreach (SubtitleInfo strack in subTracks)
                        {
                            if (trackID > 0)
                                sb.Append("," + trackID);
                            else 
                                sb.Append("0");
                            ++trackID;
                        }
                        sb.Append(" --no-video --no-audio \"" + stream.path + "\"");
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(stream.language))
                        {
                            foreach (KeyValuePair<string, string> strLanguage in LanguageSelectionContainer.Languages)
                            {
                                if (stream.language.ToLowerInvariant().Equals(strLanguage.Key.ToLowerInvariant()))
                                {
                                    sb.Append(" --language " + trackID + ":" + strLanguage.Value);
                                    break;
                                }
                            }
                        }
                        if (!string.IsNullOrEmpty(stream.name))
                            sb.Append(" --track-name \"" + trackID + ":" + stream.name.Replace("\"", "\\\"") + "\"");
                        if (stream.delay != 0)
                            sb.AppendFormat(" --sync {0}:{1}", trackID, stream.delay);
                        if (stream.bDefaultTrack)
                            sb.Append(" --default-track " + trackID + ":yes");
                        else
                            sb.Append(" --default-track " + trackID + ":no");
                        if (stream.bForceTrack)
                            sb.Append(" --forced-track " + trackID + ":yes");
                        else
                            sb.Append(" --forced-track " + trackID + ":no");
                        sb.Append(" --subtitle-tracks " + trackID + " --no-video --no-audio \"" + stream.path + "\"");
                    }
                }

                // chapters
                if (settings.ChapterInfo.HasChapters) // chapters are defined
                {
                    string strChapterFile = Path.Combine(Path.GetDirectoryName(settings.MuxedOutput), Path.GetFileNameWithoutExtension(settings.MuxedOutput) + "_chptmp.txt");
                    settings.ChapterInfo.SaveText(strChapterFile);
                    job.FilesToDelete.Add(strChapterFile);
                    sb.Append(" --chapters \"" + strChapterFile + "\"");
                }

                // attachments
                if (settings.Attachments.Count > 0)
                {
                    foreach (string strFileName in settings.Attachments)
                        sb.Append(" --attach-file \"" + strFileName + "\"");
                }

                if (settings.SplitSize.HasValue)
                    sb.Append(" --split " + (settings.SplitSize.Value.MB) + "M");

                sb.Append(" --engage no_cue_duration --engage no_cue_relative_position --ui-language en");

                return sb.ToString();
            }
        }
    }
}