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
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

using MeGUI.core.details;
using MeGUI.core.util;

namespace MeGUI
{
    class tsMuxeR : CommandlineMuxer
    {
        public static readonly JobProcessorFactory Factory = 
            new JobProcessorFactory(new ProcessorFactory(init), "TSMuxer");

        private static IJobProcessor init(MainForm mf, Job j)
        {
            if (j is MuxJob && (j as MuxJob).MuxType == MuxerType.TSMUXER)
                return new tsMuxeR(mf.Settings.TSMuxer.Path);
            return null;
        }

        private int numberOfAudioTracks, numberOfSubtitleTracks;
        private string metaFile = null;

        public tsMuxeR(string executablePath)
        {
            UpdateCacher.CheckPackage("tsmuxer");
            this.executable = executablePath;
        }
        #region setup/start overrides
        protected override void checkJobIO()
        {
            su.Status = "Muxing...";
            this.numberOfAudioTracks = job.Settings.AudioStreams.Count;
            this.numberOfSubtitleTracks = job.Settings.SubtitleStreams.Count;
            generateMetaFile();
            Util.ensureExists(metaFile);
            base.checkJobIO();
        }

        #endregion


        protected override void setProjectedFileSize()
        {
        }

        protected override string Commandline
        {
            get
            {
                return " \"" + metaFile + "\"" + " \"" + job.Output + "\"";
            }
        }

        public override void ProcessLine(string line, StreamType stream, ImageType oType)
        {
            if (Regex.IsMatch(line, @"^[0-9]{1,3}\.[0-9]{1}%", RegexOptions.Compiled))
            {
                su.PercentageDoneExact = getPercentage(line);
                return;
            }

            if (stream == StreamType.Stderr || line.ToLowerInvariant().Contains("error"))
                oType = ImageType.Error;
            else if (line.ToLowerInvariant().Contains("warning"))
                oType = ImageType.Warning;
            base.ProcessLine(line, stream, oType);
        }
        
        /// <summary>
        /// gets the completion percentage of an tsmuxer line
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        private decimal? getPercentage(string line)
        {
            try
            {
                string[] strPercentage = line.Split('%')[0].Split('.');
                return Convert.ToDecimal(strPercentage[0]) + Convert.ToDecimal(strPercentage[1]) / 10;
            }
            catch (Exception e)
            {
                log.LogValue("Exception in getPercentage(" + line + ")", e, MeGUI.core.util.ImageType.Warning);
                return null;
            }
        }

        /// <summary>
        /// determines if a read line is empty
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        private bool isEmptyLine(string line)
        {
            char[] characters = line.ToCharArray();
            bool isEmpty = true;
            foreach (char c in characters)
            {
                if (c != 32)
                {
                    isEmpty = false;
                    break;
                }
            }
            return isEmpty;
        }

        private void generateMetaFile()
        {
            metaFile = Path.ChangeExtension(job.Output, ".meta");
            MuxSettings settings = job.Settings;
            CultureInfo ci = new CultureInfo("en-us");

            using (StreamWriter sw = new StreamWriter(metaFile, false, Encoding.Default))
            {
                string vcodecID = "";
                string extra = "";
                string trackID = "";
                MediaInfoFile oVideoInfo = null;

                sw.Write("MUXOPT --no-pcr-on-video-pid --new-audio-pes"); // mux options
                if (!string.IsNullOrEmpty(settings.DeviceType) && settings.DeviceType != "Standard")
                {
                    switch (settings.DeviceType)
                    {
                        case "Blu-ray": sw.Write(" --blu-ray"); break;
                        case "AVCHD": sw.Write(" --avchd"); break;
                    }

                    if (settings.ChapterInfo.HasChapters) // chapters are defined
                        sw.Write(" --custom-chapters" + settings.ChapterInfo.GetChapterTimeLine());

                    job.Output = Path.GetDirectoryName(job.Output) + "\\" + Path.GetFileNameWithoutExtension(job.Output); // remove m2ts file extension - use folder name only with this mode
                }
                sw.Write(" --vbr --vbv-len=500"); // mux options
                if (settings.SplitSize.HasValue)
                    sw.Write(" --split-size=" + settings.SplitSize.Value.MB + "MB");

                string fpsString = null;
                string videoFile = null;
                if (!string.IsNullOrEmpty(settings.VideoInput))
                    videoFile = settings.VideoInput;
                else if (!string.IsNullOrEmpty(settings.MuxedInput))
                    videoFile = settings.MuxedInput;
                if (!String.IsNullOrEmpty(videoFile))
                {
                    oVideoInfo = new MediaInfoFile(videoFile, ref log);     
                    if (oVideoInfo.HasVideo)
                    {
                        if (oVideoInfo.VideoInfo.Codec == VideoCodec.AVC)
                        {
                            vcodecID = "V_MPEG4/ISO/AVC";
                            extra = "insertSEI, contSPS";
                        }
                        else if (oVideoInfo.VideoInfo.Codec == VideoCodec.HEVC)
                            vcodecID = "V_MPEGH/ISO/HEVC";
                        else if (oVideoInfo.VideoInfo.Codec == VideoCodec.MPEG2)
                            vcodecID = "V_MPEG-2";
                        else if (oVideoInfo.VideoInfo.Codec == VideoCodec.VC1)
                            vcodecID = "V_MS/VFW/WVC1";

                        if (oVideoInfo.ContainerFileType == ContainerType.MP4)
                            trackID = "track=1";
                        else if (oVideoInfo.ContainerFileType == ContainerType.MKV || oVideoInfo.ContainerFileType == ContainerType.M2TS)
                            trackID = "track=" + oVideoInfo.VideoInfo.Track.TrackID;

                        sw.Write("\n" + vcodecID + ", ");
                        sw.Write("\"" + videoFile + "\"");

                        if (settings.DAR.HasValue)
                            sw.Write(", ar=" + settings.DAR.Value.X + ":" + settings.DAR.Value.Y);

                        fpsString = oVideoInfo.VideoInfo.FPS.ToString(ci);
                        if (settings.Framerate.HasValue)
                            fpsString = settings.Framerate.Value.ToString(ci);
                        sw.Write(", fps=" + fpsString);

                        if (!String.IsNullOrEmpty(extra))
                            sw.Write(", " + extra);

                        if (!String.IsNullOrEmpty(trackID))
                            sw.Write(", " + trackID);
                    }
                    else
                        log.Error("No video track found: " + videoFile);
                }

                foreach (object o in settings.AudioStreams)
                {
                    MuxStream stream = (MuxStream)o;
                    string acodecID = "";

                    MediaInfoFile oInfo = new MediaInfoFile(stream.path, ref log);

                    if (!oInfo.HasAudio)
                    {
                        log.Error("No audio track found: " + stream.path);
                        continue;
                    }

                    if (oInfo.AudioInfo.Tracks[0].AudioCodec == AudioCodec.AC3 || oInfo.AudioInfo.Tracks[0].AudioCodec == AudioCodec.EAC3
                        || oInfo.AudioInfo.Tracks[0].AudioCodec == AudioCodec.THDAC3)
                        acodecID = "A_AC3";
                    else if (oInfo.AudioInfo.Tracks[0].AudioCodec == AudioCodec.AAC)
                        acodecID = "A_AAC";
                    else if (oInfo.AudioInfo.Tracks[0].AudioCodec == AudioCodec.DTS)
                        acodecID = "A_DTS";
                    else if (oInfo.AudioInfo.Tracks[0].AudioCodec == AudioCodec.PCM)
                        acodecID = "A_LPCM";
                    else
                    {
                        log.Error("Audio Codec not supported: " + oInfo.AudioInfo.Tracks[0].Codec);
                        continue;
                    }

                    sw.Write("\n" + acodecID + ", ");
                    sw.Write("\"" + stream.path + "\"");
                    
                    if (stream.delay != 0)
                       sw.Write(", timeshift={0}ms", stream.delay);

                    if (!String.IsNullOrEmpty(stream.language))
                    {
                        foreach (KeyValuePair<string, string> strLanguage in LanguageSelectionContainer.Languages)
                        {
                            if (stream.language.ToLowerInvariant().Equals(strLanguage.Key.ToLowerInvariant()))
                            {
                                sw.Write(", lang=" + strLanguage.Value);
                                break;
                            }
                        }
                    }
                }

                foreach (object o in settings.SubtitleStreams)
                {
                    MuxStream stream = (MuxStream)o;
                    string scodecID = "";

                    if (stream.path.ToLowerInvariant().EndsWith(".srt"))
                        scodecID = "S_TEXT/UTF8";
                    else 
                        scodecID = "S_HDMV/PGS"; // sup files

                    sw.Write("\n" + scodecID + ", ");
                    sw.Write("\"" + stream.path + "\"");

                    if (stream.delay != 0)
                        sw.Write(", timeshift={0}ms", stream.delay);

                    if (stream.path.ToLowerInvariant().EndsWith(".srt") && oVideoInfo != null && !String.IsNullOrEmpty(fpsString))
                        sw.Write(", video-width={0}, video-height={1}, fps={2}", oVideoInfo.VideoInfo.Width, oVideoInfo.VideoInfo.Height, fpsString);

                    if (!String.IsNullOrEmpty(stream.language))
                    {
                        foreach (KeyValuePair<string, string> strLanguage in LanguageSelectionContainer.Languages)
                        {
                            if (stream.language.ToLowerInvariant().Equals(strLanguage.Key.ToLowerInvariant()))
                            {
                                sw.Write(", lang=" + strLanguage.Value);
                                break;
                            }
                        }
                    }
                }                
            }

            job.FilesToDelete.Add(metaFile);
            if (File.Exists(metaFile))
            {
                string strMuxFile = String.Empty;
                try
                {
                    StreamReader sr = new StreamReader(metaFile);
                    strMuxFile = sr.ReadToEnd();
                    sr.Close();
                }
                catch (Exception)
                {

                }
                log.LogValue("mux script", strMuxFile);
            }
        }
    }
}