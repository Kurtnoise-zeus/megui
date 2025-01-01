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
using System.Collections.Generic;
using System.IO;
using System.Text;

using MeGUI.core.details;
using MeGUI.core.util;

namespace MeGUI
{
	/// <summary>
	/// VideoUtil is used to perform various video related tasks, namely autocropping, 
	/// auto resizing
	/// </summary>
	public static class VideoUtil
    {

        #region finding source information

        /// <summary>
        /// gets the dvd decrypter generated chapter file
        /// </summary>
        /// <param name="fileName">name of the first vob to be loaded</param>
        /// <returns>full name of the chapter file or an empty string if no file was found</returns>
        public static string getChapterFile(string fileName)
        {
            string vts;
			string path = Path.GetDirectoryName(fileName);
			string name = Path.GetFileNameWithoutExtension(fileName);
            if (name.Length > 6)
                vts = name.Substring(0, 6);
            else
                vts = name;
			string chapterFile = "";
            string[] files = Directory.GetFiles(path, vts + "*Chapter Information*");
			foreach (string file in files)
			{
				if (file.ToLowerInvariant().EndsWith(".txt") || file.ToLowerInvariant().EndsWith(".qpf"))
                {
					chapterFile = file;
					break;
				}                   
			}
			return chapterFile;
		}

        /// <summary>
        /// checks if the input file has chapters
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns>true if the file has chapters</returns>
        public static bool HasChapters(MediaInfoFile iFile)
        {
            if (iFile == null)
                return false;
            
            if (iFile.HasChapters)
                return true;

            if (Path.GetExtension(iFile.FileName.ToLowerInvariant()) != ".vob" &&
                Path.GetExtension(iFile.FileName.ToLowerInvariant()) != ".ifo")
                return false;

            // detect ifo file
            string videoIFO = String.Empty;
            if (Path.GetExtension(iFile.FileName.ToLowerInvariant()) == ".vob")
            {
                // find the main IFO
                if (Path.GetFileName(iFile.FileName).ToUpperInvariant().Substring(0, 4) == "VTS_")
                    videoIFO = iFile.FileName.Substring(0, iFile.FileName.LastIndexOf("_")) + "_0.IFO";
                else
                    videoIFO = Path.ChangeExtension(iFile.FileName, ".IFO");
            }

            if (!File.Exists(videoIFO))
                return false;

            int iPGCNumber = 1;
            if (iFile.VideoInfo.PGCNumber > 0)
                iPGCNumber = iFile.VideoInfo.PGCNumber;

            IfoExtractor ex = new IfoExtractor();
            ChapterInfo pgc = ex.GetChapterInfo(videoIFO, iPGCNumber);
            if (pgc != null && pgc.HasChapters)
                return true;
            return false;
        }

 		#endregion

		#region dgindex postprocessing
		/// <summary>
		/// gets all demuxed audio files from a given dgindex project
		/// starts with the first file and returns the desired number of files
		/// </summary>
        /// <param name="audioTrackIDs">list of audio TrackIDs</param>
		/// <param name="projectName">the name of the dgindex project</param>
		/// <returns>an array of string of filenames</returns>
        public static Dictionary<int, string> getAllDemuxedAudio(List<AudioTrackInfo> audioTracks, List<AudioTrackInfo> audioTracksDemux, out List<string> arrDeleteFiles, string projectName, LogItem log)
        {
		    Dictionary<int, string> audioFiles = new Dictionary<int, string>();
            arrDeleteFiles = new List<string>();
            string strTrackName;
            string[] files;

            if (String.IsNullOrEmpty(projectName) 
                || ((audioTracks == null || audioTracks.Count == 0) && (audioTracksDemux == null || audioTracksDemux.Count == 0)))
                return audioFiles;

            if (audioTracks != null && audioTracks.Count > 0)
            {
                if (audioTracks[0].ContainerType.ToLowerInvariant().Equals("matroska") ||
                    (Path.GetExtension(projectName).ToLowerInvariant().Equals(".dgi") && audioTracks[0].ContainerType == "MPEG-4"))
                    strTrackName = " [";
                else if (audioTracks[0].ContainerType == "MPEG-TS" || audioTracks[0].ContainerType == "BDAV")
                    strTrackName = " PID ";
                else
                    strTrackName = " T";

                for (int counter = 0; counter < audioTracks.Count; counter++)
                {
                    // if the expected demux file already exists, it will be directly used = no need to find it
                    // e.g. when eac3to demuxes a PCM track from a MKV file
                    if (File.Exists(Path.Combine(Path.GetDirectoryName(projectName), audioTracks[counter].DemuxFileName)))
                    {
                        string file = Path.Combine(Path.GetDirectoryName(projectName), audioTracks[counter].DemuxFileName);
                        if (!audioFiles.ContainsValue(file) && !audioFiles.ContainsKey(audioTracks[counter].TrackID))
                            audioFiles.Add(audioTracks[counter].TrackID, file);
                        continue;
                    }

                    bool bFound = false;
                    string trackFile = strTrackName + audioTracks[counter].TrackIDx + "*";
                    if (Path.GetExtension(projectName).ToLowerInvariant().Equals(".ffindex") || Path.GetExtension(projectName).ToLowerInvariant().Equals(".lwi"))
                        trackFile = Path.GetFileNameWithoutExtension(projectName) + "_track_" + (audioTracks[counter].TrackIndex + 1) + "_*.avs";
                    else
                        trackFile = Path.GetFileNameWithoutExtension(projectName) + trackFile;

                    files = Directory.GetFiles(Path.GetDirectoryName(projectName), trackFile);
                    foreach (string file in files)
                    {
                        if (file.EndsWith(".ac3") ||
                             file.EndsWith(".mp3") ||
                             file.EndsWith(".mp2") ||
                             file.EndsWith(".mp1") ||
                             file.EndsWith(".mpa") ||
                             file.EndsWith(".dts") ||
                             file.EndsWith(".wav") ||
                             file.EndsWith(".ogg") ||
                             file.EndsWith(".flac") ||
                             file.EndsWith(".ra") ||
                             file.EndsWith(".avs") ||
                             file.EndsWith(".aac")) // It is the right track
                        {
                            bFound = true;
                            if (!audioFiles.ContainsValue(file))
                                audioFiles.Add(audioTracks[counter].TrackID, file);
                            break;
                        }
                    }
                    if (!bFound && log != null)
                        log.LogEvent("File not found: " + Path.Combine(Path.GetDirectoryName(projectName), trackFile), ImageType.Error);
                }

                // Find files which can be deleted
                strTrackName = Path.GetFileNameWithoutExtension(projectName) + strTrackName;

                files = Directory.GetFiles(Path.GetDirectoryName(projectName), strTrackName + "*");
                foreach (string file in files)
                {
                    if (file.EndsWith(".ac3") ||
                         file.EndsWith(".mp3") ||
                         file.EndsWith(".mp2") ||
                         file.EndsWith(".mp1") ||
                         file.EndsWith(".mpa") ||
                         file.EndsWith(".dts") ||
                         file.EndsWith(".wav") ||
                         file.EndsWith(".avs") ||
                         file.EndsWith(".aac")) // It is the right track
                    {
                        if (!audioFiles.ContainsValue(file))
                            arrDeleteFiles.Add(file);
                    }
                }
            }
            if (audioTracksDemux == null)
                return audioFiles;
            
            foreach (AudioTrackInfo oTrack in audioTracksDemux)
            {
                bool bFound = false;
                string trackFile = Path.GetDirectoryName(projectName) + "\\" + oTrack.DemuxFileName;
                if (File.Exists(trackFile))
                {
                    bFound = true;
                    if (!audioFiles.ContainsValue(trackFile))
                        audioFiles.Add(oTrack.TrackID, trackFile);
                    continue;
                }
                if (!bFound && log != null)
                    log.LogEvent("File not found: " + trackFile, ImageType.Error);
            }

            return audioFiles;
		}

		#endregion

		#region automated job generation
		/// <summary>
		/// ensures that video and audio don't have the same filenames which would lead to severe problems
		/// </summary>
		/// <param name="videoOutput">name of the encoded video file</param>
		/// <param name="muxedOutput">name of the final output</param>
		/// <param name="aStreams">all encodable audio streams</param>
		/// <returns>the info to be added to the log</returns>
		public static LogItem eliminatedDuplicateFilenames(ref string videoOutput, ref string muxedOutput, AudioJob[] aStreams)
		{
            LogItem log = new LogItem("Eliminating duplicate filenames", ImageType.Information, true);
            if (!String.IsNullOrEmpty(videoOutput))
                videoOutput = Path.GetFullPath(videoOutput);
            muxedOutput = Path.GetFullPath(muxedOutput);

            log.LogValue("Video output file", videoOutput);
            if (File.Exists(videoOutput))
            {
                int counter = 0;
                string directoryname = Path.GetDirectoryName(videoOutput);
                string filename = Path.GetFileNameWithoutExtension(videoOutput);
                string extension = Path.GetExtension(videoOutput);

                while (File.Exists(videoOutput))
                {
                    videoOutput = Path.Combine(directoryname,
                        filename + "_" + counter + extension);
                    counter++;
                }

                log.LogValue("File already exists. New video output filename", videoOutput);
            }

            log.LogValue("Muxed output file", muxedOutput);
            if (File.Exists(muxedOutput) || muxedOutput == videoOutput)
            {
                int counter = 0;
                string directoryname = Path.GetDirectoryName(muxedOutput);
                string filename = Path.GetFileNameWithoutExtension(muxedOutput);
                string extension = Path.GetExtension(muxedOutput);

                while (File.Exists(muxedOutput) || muxedOutput == videoOutput)
                {
                    muxedOutput = Path.Combine(directoryname,
                        filename + "_" + counter + extension);
                    counter++;
                }

                log.LogValue("File already exists. New muxed output filename", muxedOutput);
            }

            if (aStreams == null)
                log.LogValue("No Audio Streams found", aStreams);
            else
            {
                for (int i = 0; i < aStreams.Length; i++)
                {
                    string name = Path.GetFullPath(aStreams[i].Output);
                    log.LogValue("Encodable audio stream " + i, name);
                    if (name.Equals(videoOutput) || name.Equals(muxedOutput)) // audio will be overwritten -> no good
                    {
                        name = Path.Combine(Path.GetDirectoryName(name), Path.GetFileNameWithoutExtension(name) + i.ToString() + Path.GetExtension(name));
                        aStreams[i].Output = name;
                        log.LogValue("Stream has the same name as video stream. New audio stream output", name);
                    }
                }
            }
             return log;

		}
        #endregion

        #region new stuff
        public static JobChain GenerateJobSeries(VideoStream video, string muxedOutput, AudioJob[] audioStreams,
            MuxStream[] subtitles, List<string> attachments, string timeStampFile, ChapterInfo chapterInfo, FileSize? desiredSize, FileSize? splitSize, 
            ContainerType container, bool prerender, MuxStream[] muxOnlyAudio, LogItem log, string deviceType, 
            Zone[] zones, string videoFileToMux, OneClickAudioTrack[] audioTracks, bool alwaysMuxOutput)
        {
            if (desiredSize.HasValue && String.IsNullOrEmpty(videoFileToMux))
            {
                if (video.Settings.VideoEncodingType != VideoCodecSettings.VideoEncodingMode.twopassAutomated
                    && video.Settings.VideoEncodingType != VideoCodecSettings.VideoEncodingMode.threepassAutomated) // no automated 2/3 pass
                {
                    if (MainForm.Instance.Settings.NbPasses == 2)
                    {
                        video.Settings.VideoEncodingType = VideoCodecSettings.VideoEncodingMode.twopassAutomated; // automated 2 pass
                        video.Settings.FFV1EncodingType = VideoCodecSettings.FFV1EncodingMode.twopassAutomated;   // automated 2 pass
                    }
                    else if (video.Settings.MaxNumberOfPasses == 3)
                        video.Settings.VideoEncodingType = VideoCodecSettings.VideoEncodingMode.threepassAutomated;
                }
            }

            FixFileNameExtensions(video, audioStreams, container);
            string videoOutput = video.Output;
            log?.Add(eliminatedDuplicateFilenames(ref videoOutput, ref muxedOutput, audioStreams));
            
            JobChain vjobs = null;
            if (!String.IsNullOrEmpty(videoFileToMux))
                video.Output = videoFileToMux;
            else
            {
                video.Output = videoOutput;
                vjobs = JobUtil.prepareVideoJob(video.Input, video.Output, video.Settings, video.DAR, prerender, zones);
                if (vjobs == null) return null;
            }
            
            /* Here, we guess the types of the files based on extension.
             * This is guaranteed to work with MeGUI-encoded files, because
             * the extension will always be recognised. For non-MeGUI files,
             * we can only ever hope.*/
            List<MuxStream> allAudioToMux = new List<MuxStream>();
            List<MuxableType> allInputAudioTypes = new List<MuxableType>();

            if (audioTracks != null)
            {
                // OneClick mode
                foreach (OneClickAudioTrack ocAudioTrack in audioTracks)
                {
                    if (ocAudioTrack.DirectMuxAudio != null)
                    {
                        if (VideoUtil.guessAudioMuxableType(ocAudioTrack.DirectMuxAudio.path, true) != null)
                        {
                            allInputAudioTypes.Add(VideoUtil.guessAudioMuxableType(ocAudioTrack.DirectMuxAudio.path, true));
                            allAudioToMux.Add(ocAudioTrack.DirectMuxAudio);
                        }
                    }
                    if (ocAudioTrack.AudioJob != null && !String.IsNullOrEmpty(ocAudioTrack.AudioJob.Input))
                    {
                        allAudioToMux.Add(ocAudioTrack.AudioJob.ToMuxStream());
                        allInputAudioTypes.Add(ocAudioTrack.AudioJob.ToMuxableType());
                    }
                }
            }
            else
            {
                // AutoEncode mode
                foreach (AudioJob stream in audioStreams)
                {
                    allAudioToMux.Add(stream.ToMuxStream());
                    allInputAudioTypes.Add(stream.ToMuxableType());
                }

                if (muxOnlyAudio != null)
                {
                    foreach (MuxStream muxStream in muxOnlyAudio)
                    {
                        if (VideoUtil.guessAudioMuxableType(muxStream.path, true) != null)
                        {
                            allInputAudioTypes.Add(VideoUtil.guessAudioMuxableType(muxStream.path, true));
                            allAudioToMux.Add(muxStream);
                        }
                    }
                }
            }

            List<MuxableType> allInputSubtitleTypes = new List<MuxableType>();
            if (subtitles != null)
            {
                foreach (MuxStream muxStream in subtitles)
                    if (VideoUtil.guessSubtitleType(muxStream.path) != null)
                        allInputSubtitleTypes.Add(new MuxableType(VideoUtil.guessSubtitleType(muxStream.path), null));
            }

            MuxableType chapterInputType = null;
            if (chapterInfo != null && chapterInfo.HasChapters)
                chapterInputType = new MuxableType(ChapterType.OGG_TXT, null);

            MuxableType deviceOutputType = null;
            if (!String.IsNullOrEmpty(deviceType))
            {
                DeviceType type = VideoUtil.guessDeviceType(deviceType);
                if (type != null)
                    deviceOutputType = new MuxableType(type, null);
            }

            List<string> inputsToDelete = new List<string>();
            if (String.IsNullOrEmpty(videoFileToMux))
                inputsToDelete.Add(video.Output);
            inputsToDelete.AddRange(Array.ConvertAll<AudioJob, string>(audioStreams, delegate(AudioJob a) { return a.Output; }));

            JobChain muxJobs = JobUtil.GenerateMuxJobs(video, null, allAudioToMux.ToArray(), allInputAudioTypes.ToArray(),
                subtitles, allInputSubtitleTypes.ToArray(), attachments, chapterInfo, chapterInputType, container, muxedOutput, timeStampFile,
                splitSize, inputsToDelete, deviceType, deviceOutputType, alwaysMuxOutput);

            if (desiredSize.HasValue && String.IsNullOrEmpty(videoFileToMux))
            {
                BitrateCalculationInfo b = new BitrateCalculationInfo();
                
                List<string> audiofiles = new List<string>();
                foreach (MuxStream s in allAudioToMux)
                    audiofiles.Add(s.path);
                b.AudioFiles = audiofiles;

                b.Container = container;
                b.VideoJobs = new List<TaggedJob>(vjobs.Jobs);
                b.DesiredSize = desiredSize.Value;
                ((VideoJob)vjobs.Jobs[0].Job).BitrateCalculationInfo = b;
            }

            if (!String.IsNullOrEmpty(videoFileToMux))
                return new SequentialChain(new SequentialChain((Job[])audioStreams), new SequentialChain(muxJobs));
            else
                return new SequentialChain(
                    new SequentialChain((Job[])audioStreams),
                    new SequentialChain(vjobs),
                    new SequentialChain(muxJobs));
        }

        private static void FixFileNameExtensions(VideoStream video, AudioJob[] audioStreams, ContainerType container)
        {
            AudioEncoderType[] audioCodecs = new AudioEncoderType[audioStreams.Length];
            for (int i = 0; i < audioStreams.Length; i++)
            {
                audioCodecs[i] = audioStreams[i].Settings.EncoderType;
            }
            MuxPath path;
            if (video.Settings == null)
                path = MainForm.Instance.MuxProvider.GetMuxPath(VideoEncoderType.X264, audioCodecs, container);
            else
                path = MainForm.Instance.MuxProvider.GetMuxPath(video.Settings.EncoderType, audioCodecs, container);
            if (path == null)
                return;
            List<AudioType> audioTypes = new List<AudioType>();
            foreach (MuxableType type in path.InitialInputTypes)
            {
                if (type.outputType is VideoType)
                {
                    if (video.Settings.EncoderType == VideoEncoderType.XVID && (type.outputType.ContainerType == ContainerType.AVI || type.outputType.ContainerType == ContainerType.MKV))
                        video.Output = Path.ChangeExtension(video.Output, ".m4v");
                    else if (video.Settings.EncoderType == VideoEncoderType.X264 && type.outputType.ContainerType == ContainerType.MP4)
                        video.Output = Path.ChangeExtension(video.Output, ".264");
                    else if (video.Settings.EncoderType == VideoEncoderType.X265 && (type.outputType.ContainerType == ContainerType.MKV || type.outputType.ContainerType == ContainerType.MP4))
                        video.Output = Path.ChangeExtension(video.Output, ".hevc");
                    else if (video.Settings.EncoderType == VideoEncoderType.SVTAV1PSY && (type.outputType.ContainerType == ContainerType.MKV || type.outputType.ContainerType == ContainerType.MP4))
                        video.Output = Path.ChangeExtension(video.Output, ".ivf");
                    else 
                        video.Output = Path.ChangeExtension(video.Output, type.outputType.Extension);
                    video.VideoType = type;
                }
                if (type.outputType is AudioType type1)
                {
                    audioTypes.Add(type1);
                }
            }
            AudioEncoderProvider aProvider = new AudioEncoderProvider();
            for (int i = 0; i < audioStreams.Length; i++)
            {
                AudioType[] types = aProvider.GetSupportedOutput(audioStreams[i].Settings.EncoderType);
                foreach (AudioType type in types)
                {
                    if (audioTypes.Contains(type))
                    {
                        string newFileName = Path.ChangeExtension(audioStreams[i].Output, type.Extension);
                        if (!audioStreams[i].Input.Equals(newFileName))
                            audioStreams[i].Output = newFileName;
                        break;
                    }
                }
            }
        }

        public static SubtitleType guessSubtitleType(string p)
        {
            if (String.IsNullOrEmpty(p))
                return null;

            foreach (SubtitleType type in ContainerManager.SubtitleTypes.Values)
            {
                if (Path.GetExtension(p.ToLowerInvariant()) == "." + type.Extension)
                    return type;
            }
            return null;
        }

        public static VideoType guessVideoType(string p)
        {
            if (String.IsNullOrEmpty(p))
                return null;
            
            foreach (VideoType type in ContainerManager.VideoTypes.Values)
            {
                if (Path.GetExtension(p.ToLowerInvariant()) == "." + type.Extension)
                    return type;
            }
            return null;
        }
 
        public static AudioType guessAudioType(string p)
        {
            if (String.IsNullOrEmpty(p))
                return null;
            
            foreach (AudioType type in ContainerManager.AudioTypes.Values)
            {
                if (Path.GetExtension(p.ToLowerInvariant()) == "." + type.Extension)
                    return type;
            }

            return null;
        }

        public static ChapterType guessChapterType(string p)
        {
            if (String.IsNullOrEmpty(p))
                return null;
            
            foreach (ChapterType type in ContainerManager.ChapterTypes.Values)
            {
                if (Path.GetExtension(p.ToLowerInvariant()) == "." + type.Extension)
                    return type;
            }

            return null;
        }

        public static DeviceType guessDeviceType(string p)
        {
            foreach (DeviceType type in ContainerManager.DeviceTypes.Values)
            {
                if (p == type.Extension)
                    return type;
            }
            return null;
        }

        public static MuxableType guessVideoMuxableType(string p, bool useMediaInfo)
        {
            if (string.IsNullOrEmpty(p))
                return null;

            if (useMediaInfo)
            {
                using (MediaInfoFile info = new MediaInfoFile(p))
                {
                    if (info.VideoInfo.HasVideo)
                        return new MuxableType(info.VideoInfo.Type, info.VideoInfo.Codec);
                }
            }

            VideoType vType = guessVideoType(p);
            if (vType != null)
            {
                if (vType.SupportedCodecs.Length == 1)
                    return new MuxableType(vType, vType.SupportedCodecs[0]);
                else
                    return new MuxableType(vType, null);
            }
            return null;
        }

        public static MuxableType guessAudioMuxableType(string p, bool useMediaInfo)
        {
            if (string.IsNullOrEmpty(p))
                return null;

            if (useMediaInfo)
            {
                using (MediaInfoFile info = new MediaInfoFile(p))
                {
                    if (info.AudioInfo.Tracks.Count == 1 && info.AudioInfo.Tracks[0].AudioType != null)
                        return new MuxableType(info.AudioInfo.Tracks[0].AudioType, info.AudioInfo.Tracks[0].AudioCodec);
                }
            }

            AudioType aType = guessAudioType(p);
            if (aType != null)
            {
                if (aType.SupportedCodecs.Length == 1)
                    return new MuxableType(aType, aType.SupportedCodecs[0]);
                else
                    return new MuxableType(aType, null);
            }
            return null;
        }
        #endregion

        public static string GenerateCombinedFilter(OutputFileType[] types)
        {
            StringBuilder initialFilterName = new StringBuilder();
            StringBuilder initialFilter = new StringBuilder();
            StringBuilder allSmallFilters = new StringBuilder();
            initialFilterName.Append("All supported files (");
            foreach (OutputFileType type in types)
            {
                initialFilter.Append(type.OutputFilter);
                initialFilter.Append(';');
                initialFilterName.Append(type.OutputFilter);
                initialFilterName.Append(", ");
                allSmallFilters.Append(type.OutputFilterString);
                allSmallFilters.Append('|');
            }

            string initialFilterTrimmed = initialFilterName.ToString().TrimEnd(' ', ',') + ")|" +
                initialFilter.ToString();

            if (types.Length > 1)
                return initialFilterTrimmed + "|" + allSmallFilters.ToString().TrimEnd('|');
            else
                return allSmallFilters.ToString().TrimEnd('|');
        }

        public static string getFFMSVideoInputLine(string inputFile, string indexFile, double fps)
        {
            UpdateCacher.CheckPackage("ffms");
            GetFPSDetails(fps, inputFile, out int fpsnum, out int fpsden, true, out bool variableFrameRate);
            return GetFFMSBasicInputLine(!IsFFMSDefaultPluginRequired(), inputFile, indexFile, -1, 0, fpsnum, fpsden, true, variableFrameRate);
        }

        public static string getFFMSAudioInputLine(string inputFile, string indexFile, int track, bool applyDRC)
        {
            UpdateCacher.CheckPackage("ffms");
               return GetFFAudioInputLine(inputFile, indexFile, track, true, applyDRC);
        }

        private static bool IsFFMSDefaultPluginRequired()
        {
            StringBuilder script = new StringBuilder();
            script.AppendFormat("LoadPlugin(\"{0}\"){1}", Path.Combine(Path.GetDirectoryName(MainForm.Instance.Settings.FFMS.Path), "ffms2.dll"), Environment.NewLine);
            script.AppendFormat("BlankClip(){0}", Environment.NewLine);
            return AVSScriptHasVideo(script.ToString(), out _);
        }

        private static string GetFFMSBasicInputLine(bool loadCPlugin, string inputFile, string indexFile, int track, int rffmode, int fpsnum, int fpsden, bool video, bool variableFrameRate)
        {
            StringBuilder script = new StringBuilder();
            script.AppendFormat("Load{0}Plugin(\"{1}\"){2}",
                (loadCPlugin ? "C" : String.Empty),
                Path.Combine(Path.GetDirectoryName(MainForm.Instance.Settings.FFMS.Path), "ffms2.dll"),
                Environment.NewLine);

            if (inputFile.ToLowerInvariant().EndsWith(".ffindex"))
                inputFile = inputFile.Substring(0, inputFile.Length - 8);
            if (!String.IsNullOrEmpty(indexFile) && indexFile.ToLowerInvariant().Equals(inputFile.ToLowerInvariant() + ".ffindex"))
                indexFile = null;

            if (video)
            {
                // use FFVideoSource
                script.AppendFormat("FFVideoSource(\"{0}\"{1}{2}{3}{4}{5}{6})",
                    inputFile,
                    (track > -1 ? ", track=" + track : String.Empty),
                    (!String.IsNullOrEmpty(indexFile) ? ", cachefile=\"" + indexFile + "\"" : String.Empty),
                    ((fpsnum > 0 && fpsden > 0 && !variableFrameRate) ? ", fpsnum=" + fpsnum + ", fpsden=" + fpsden : String.Empty),
                    (MainForm.Instance.Settings.FFMSThreads > 0 ? ", threads=" + MainForm.Instance.Settings.FFMSThreads : String.Empty),
                    (rffmode > 0 ? ", rffmode=" + rffmode : String.Empty),
                    (MainForm.Instance.Settings.Input8Bit && MainForm.Instance.Settings.AviSynthPlus ? ", colorspace=\"YUV420P8\"" : String.Empty));
            }
            else
            {
                // use FFAudioSource
                script.AppendFormat("FFAudioSource(\"{0}\"{1}{2}){3}",
                    inputFile,
                    (track > -1 ? ", track=" + track : String.Empty),
                    (!String.IsNullOrEmpty(indexFile) ? ", cachefile=\"" + indexFile + "\"" : String.Empty),
                    Environment.NewLine);
            }
            return script.ToString();
        }
        private static string GetFFAudioInputLine(string inputFile, string indexFile, int track, bool audio, bool drc)

        {
            StringBuilder script = new StringBuilder();
            script.AppendFormat("LoadPlugin(\"{0}\"){1}",
                Path.Combine(Path.GetDirectoryName(MainForm.Instance.Settings.FFMS.Path), "ffms2.dll"),
                Environment.NewLine);

            if (inputFile.ToLowerInvariant().EndsWith(".ffindex") && File.Exists(inputFile))
                inputFile = inputFile.Substring(0, inputFile.Length - 4);
            if (!String.IsNullOrEmpty(indexFile) && indexFile.ToLowerInvariant().Equals(inputFile.ToLowerInvariant() + ".ffindex"))
                indexFile = null;

            bool bUseFFAudio = UseFFAudioSource(inputFile, audio);
            if (audio)
            {
                script.AppendFormat("{0}(\"{1}\"{2}{3}{4}){5}",
                    ("FFAudioSource"),
                    inputFile,
                    (track > -1 ? ", track=" + track : String.Empty),
                    (!bUseFFAudio && !String.IsNullOrEmpty(indexFile) ? ", cachefile=\"" + indexFile + "\"" : String.Empty),
                    (!bUseFFAudio && drc ? ", drc_scale=1" : ", drc_scale=0"),
                    Environment.NewLine);
            }
            return script.ToString();
        }

        public static bool UseFFAudioSource(string inputFile, bool bAudio)
        {
            StringBuilder script = new StringBuilder();
            script.AppendFormat("LoadPlugin(\"{0}\"){1}", MainForm.Instance.Settings.FFMS.Path, Environment.NewLine);
            script.AppendFormat("{0}(\"{1}\")", (bAudio ? "FFAudioSource" : "FFAudioSource"), inputFile);

            try
            {
                using (AviSynthClip a = AviSynthScriptEnvironment.ParseScript(script.ToString()))
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public static string GetLSMASHVideoInputLine(string inputFile, string indexFile, double fps, ref MediaInfoFile oInfo)
        {
            UpdateCacher.CheckPackage("lsmash");

            int iVideoBits = 8;

            if (!String.IsNullOrEmpty(indexFile) && String.IsNullOrEmpty(inputFile))
            {
                using (StreamReader sr = new StreamReader(indexFile, System.Text.Encoding.Default))
                {
                    string line = null;
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (line.StartsWith("<InputFilePath>"))
                        {
                            string strSourceFile = line.Substring(15, line.LastIndexOf("</InputFilePath>") - 15);
                            if (File.Exists(strSourceFile))
                                inputFile = strSourceFile;
                            break;
                        }
                    }
                }
            }
            if (File.Exists(inputFile) && oInfo == null)
                oInfo = new MediaInfoFile(inputFile);
            if (oInfo != null)
            {
                if (oInfo.VideoInfo.HasVideo)
                {
                    if (fps == 0 && oInfo.VideoInfo.FPS > 0)
                        fps = oInfo.VideoInfo.FPS;
                    iVideoBits = oInfo.VideoInfo.BitDepth;
                }
                if (String.IsNullOrEmpty(indexFile) && !oInfo.FileName.Equals(inputFile))
                {
                    indexFile = inputFile;
                    inputFile = oInfo.FileName;
                }
            }

            GetFPSDetails(fps, inputFile, out int fpsnum, out int fpsden);
            return GetLSMASHBasicInputLine(inputFile, indexFile, -1, 0, fpsnum, fpsden, true, iVideoBits);
        }

        public static string getLSMASHAudioInputLine(string inputFile, string indexFile, int track, bool applyDRC)
        {
            UpdateCacher.CheckPackage("lsmash");
            return GetLSMASHAudioInputLine(inputFile, indexFile, track, true, applyDRC);
        }

        public static string getBestAudioInputLine(string inputFile, string indexFile, int track, bool applyDRC)
        {
            UpdateCacher.CheckPackage("bestsource");
            return GetBestAudioInputLine(inputFile, indexFile, track, true, applyDRC);
        }

        private static string GetLSMASHBasicInputLine(string inputFile, string indexFile, int track, int rffmode, int fpsnum, int fpsden, bool video, int iVideoBit)

        {
            StringBuilder script = new StringBuilder();
            script.AppendFormat("LoadPlugin(\"{0}\"){1}",
                MainForm.Instance.Settings.LSMASH.Path,
                Environment.NewLine);

            if (inputFile.ToLowerInvariant().EndsWith(".lwi") && File.Exists(inputFile))
                inputFile = inputFile.Substring(0, inputFile.Length - 4);
            if (!String.IsNullOrEmpty(indexFile) && indexFile.ToLowerInvariant().Equals(inputFile.ToLowerInvariant() + ".lwi"))
                indexFile = null;

            bool bUseLsmash = UseLSMASHVideoSource(inputFile, video);
            if (video)
            {
                script.AppendFormat("{0}(\"{1}\"{2}{3}",
                    (bUseLsmash ? "LSMASHVideoSource" : "LWLibavVideoSource"),
                    inputFile,
                    (track > -1 ? (bUseLsmash ? ", track=" + track : ", stream_index=" + track) : String.Empty),
                    (!bUseLsmash && !String.IsNullOrEmpty(indexFile) ? ", cachefile=\"" + indexFile + "\"" : String.Empty));

                if (iVideoBit <= 8)
                    script.Append(')');
                else if (!MainForm.Instance.Settings.AviSynthPlus || MainForm.Instance.Settings.Input8Bit)
                    script.Append(", format=\"YUV420P8\")");
                else if (iVideoBit <= 10)
                    script.AppendFormat(", format=\"YUV420P10\")", Environment.NewLine);
                else if (iVideoBit <= 12)
                    script.AppendFormat(", format=\"YUV420P12\")", Environment.NewLine);
                else if (iVideoBit <= 14)
                    script.AppendFormat(", format=\"YUV420P14\")", Environment.NewLine);
                else
                    script.AppendFormat(", format=\"YUV420P16\")", Environment.NewLine);
            }
            else
            {
                script.AppendFormat("{0}(\"{1}\"{2}{3}){4}",
                    (bUseLsmash ? "LSMASHAudioSource" : "LWLibavAudioSource"),
                    inputFile,
                    (track > -1 ? (bUseLsmash ? ", track=" + track : ", stream_index=" + track) : String.Empty),
                    (!bUseLsmash && !String.IsNullOrEmpty(indexFile) ? ", cachefile=\"" + indexFile + "\"" : String.Empty),
                    Environment.NewLine);
            }
            return script.ToString();
        }

        public static bool UseLSMASHVideoSource(string inputFile, bool bVideo)
        {
            string extension = Path.GetExtension(inputFile).ToLowerInvariant();
            if (!extension.Equals(".mp4") && !extension.Equals(".m4v") && !extension.Equals(".mov") && !extension.Equals(".m4a") &&
                !extension.Equals(".3gp") && !extension.Equals(".3g2") && !extension.Equals(".aac") && !extension.Equals(".qt"))
                return false;

            StringBuilder script = new StringBuilder();
            script.AppendFormat("LoadPlugin(\"{0}\"){1}", MainForm.Instance.Settings.LSMASH.Path, Environment.NewLine);
            script.AppendFormat("{0}(\"{1}\")", (bVideo ? "LSMASHVideoSource" : "LSMASHAudioSource"), inputFile);

            try
            {
                using (AviSynthClip a = AviSynthScriptEnvironment.ParseScript(script.ToString()))
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        private static string GetBestAudioInputLine(string inputFile, string indexFile, int track, bool audio, bool drc)
        {
            StringBuilder script = new StringBuilder();
            script.AppendFormat("LoadPlugin(\"{0}\"){1}",
                Path.Combine(Path.GetDirectoryName(MainForm.Instance.Settings.BestSource.Path), "BestSource.dll"),
                Environment.NewLine);

            if (audio)
            {
                script.AppendFormat("{0}(\"{1}\"{2}{3}{4}){5}",
                    ("BSAudioSource"),
                    inputFile,
                    (track > -1 ? ("track=" + track) : String.Empty),
                    (drc ? ", drc_scale=1" : ", drc_scale=0"),
                    (!String.IsNullOrEmpty(indexFile) ? ", cachepath=\"" + indexFile + "\"" : String.Empty),                    
                    Environment.NewLine);
            }
            return script.ToString();
        }


        private static string GetLSMASHAudioInputLine(string inputFile, string indexFile, int track, bool audio, bool drc)

        {
            StringBuilder script = new StringBuilder();
            script.AppendFormat("LoadPlugin(\"{0}\"){1}",
                MainForm.Instance.Settings.LSMASH.Path,
                Environment.NewLine);

            if (inputFile.ToLowerInvariant().EndsWith(".lwi") && File.Exists(inputFile))
                inputFile = inputFile.Substring(0, inputFile.Length - 4);
            if (!String.IsNullOrEmpty(indexFile) && indexFile.ToLowerInvariant().Equals(inputFile.ToLowerInvariant() + ".lwi"))
                indexFile = null;

            if (audio)
            {
                script.AppendFormat("{0}(\"{1}\"{2}{3}{4}){5}",
                    ("LWLibavAudioSource"),
                    inputFile,
                    (track > -1 ? ("stream_index=" + track) : String.Empty),
                    (!String.IsNullOrEmpty(indexFile) ? ", cachefile=\"" + indexFile + "\"" : String.Empty),
                    (drc ? ", drc_scale=1" : ", drc_scale=0"),
                    Environment.NewLine);
            }
            return script.ToString();
        }

        public static bool UseLSMASHAudioSource(string inputFile, bool bAudio)
        {
            StringBuilder script = new StringBuilder();
            script.AppendFormat("LoadPlugin(\"{0}\"){1}", MainForm.Instance.Settings.LSMASH.Path, Environment.NewLine);
            script.AppendFormat("{0}(\"{1}\")", ("LWLibavAudioSource"), inputFile);

            try
            {
                using (AviSynthClip a = AviSynthScriptEnvironment.ParseScript(script.ToString()))
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public static string getAssumeFPS(double fps, string strInput)
        {
            if (!GetFPSDetails(fps, strInput, out int fpsnum, out int fpsden, true, out bool variableFrameRate))
                return String.Empty;

            if (!variableFrameRate)
                return ".AssumeFPS(" + fpsnum + "," + fpsden + ")";
            else
                return String.Empty;
        }

        public static bool GetFPSDetails(double fps, string strInput, out int fpsnum, out int fpsden)
        {
            return GetFPSDetails(fps, strInput, out fpsnum, out fpsden, false, out _);
        }

        public static bool GetFPSDetails(double fps, string strInput, out int fpsnum, out int fpsden, bool detectVFR, out bool variableFrameRate)
        {
            fpsnum = fpsden = 0;
            variableFrameRate = false;

            if (!String.IsNullOrEmpty(strInput) && File.Exists(strInput))
            {
                if (fps <= 0)
                {
                    if (strInput.ToLowerInvariant().EndsWith(".ffindex"))
                        strInput = strInput.Substring(0, strInput.Length - 8);
                    if (Path.GetExtension(strInput).ToLowerInvariant().Equals(".avs"))
                    {
                        fps = GetFPSFromAVSFile(strInput);
                        if (fps <= 0)
                            return false;

                        if (detectVFR)
                        {
                            using (MediaInfoFile oInfo = new MediaInfoFile(strInput))
                            {
                                if (oInfo.VideoInfo.HasVideo)
                                    variableFrameRate = oInfo.VideoInfo.VariableFrameRateMode;
                                else
                                    return false;
                            }
                        }
                    }
                    else
                    {
                        using (MediaInfoFile oInfo = new MediaInfoFile(strInput))
                        {
                            if (oInfo.VideoInfo.HasVideo && oInfo.VideoInfo.FPS > 0)
                            {
                                fps = oInfo.VideoInfo.FPS;
                                variableFrameRate = oInfo.VideoInfo.VariableFrameRateMode;
                            }
                            else
                                return false;
                        }
                    }
                }
                else if (detectVFR)
                {
                    using (MediaInfoFile oInfo = new MediaInfoFile(strInput))
                    {
                        if (oInfo.VideoInfo.HasVideo)
                            variableFrameRate = oInfo.VideoInfo.VariableFrameRateMode;
                        else
                            return false;
                    }
                }
            }

            if (fps <= 0)
                return false;

            double dFPS = Math.Round(fps, 3);
            if (dFPS == 23.976)
            {
                fpsnum = 24000;
                fpsden = 1001;
            }
            else if (dFPS == 29.970)
            {
                fpsnum = 30000;
                fpsden = 1001;
            }
            else if (dFPS == 59.940)
            {
                fpsnum = 60000;
                fpsden = 1001;
            }
            else if (dFPS == 119.880)
            {
                fpsnum = 120000;
                fpsden = 1001;
            }
            else
            {
                try
                {
                    MeGUI.core.util.RatioUtils.approximate((decimal)fps, out ulong x, out ulong y);
                    fpsnum = (int)x;
                    fpsden = (int)y;
                }
                catch
                {
                    return false;
                }
            }
            return true;
        }

        public static double ConvertFPSFractionToDouble(int fpsnum, int fpsden)
        {
            string fps = fpsnum + "/" + fpsden;
            switch (fps)
            {
                case "24000/1001": return 23.976;
                case "30000/1001": return 29.97;
                case "60000/1001": return 59.94;
                case "120000/1001": return 119.88;
            }

            return (double)fpsnum / (double)fpsden;
        }

        public static double GetFPSFromAVSFile(String strAVSScript)
        {
            try
            {
                if (!Path.GetExtension(strAVSScript).ToLowerInvariant().Equals(".avs"))
                    return 0;
                using (AviSynthClip a = AviSynthScriptEnvironment.OpenScriptFile(strAVSScript))
                    if (a.HasVideo)
                        return (double)a.raten / (double)a.rated;
                return 0;
            }
            catch
            {
                return 0;
            }
        }

        public static bool AVSScriptHasVideo(String strAVSScript, out string strErrorText)
        {
            try
            {
                strErrorText = String.Empty;
                using (AviSynthClip a = AviSynthScriptEnvironment.ParseScript(strAVSScript))
                    return a.HasVideo;
            }
            catch (Exception ex)
            {
                strErrorText = ex.Message;
                return false;
            }
        }
    }

	#region helper structs
	/// <summary>
	/// helper structure for cropping
	/// holds the crop values for all 4 edges of a frame
	/// </summary>
	[LogByMembers]
    public sealed class CropValues
	{
		public int left, top, right, bottom;
        public CropValues Clone()
        {
            return (CropValues)this.MemberwiseClone();
        }
        public bool isCropped()
        {
            if (left != 0 || top != 0 || right != 0 || bottom != 0)
                return true;
            else
                return false;
        }
	}

    public class SubtitleInfo
    {
        private string name;
        private int index;
        public SubtitleInfo(string name, int index)
        {
            this.name = name;
            this.index = index;
        }
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public int Index
        {
            get { return index; }
            set { index = value; }
        }
        public override string ToString()
        {
            string fullString = "[" + this.index.ToString("D2") + "] - " + this.name;
            return fullString.Trim();
        }
    }
	#endregion
}