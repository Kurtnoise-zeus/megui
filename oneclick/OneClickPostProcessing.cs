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
using System.IO;
using System.Threading;

using MeGUI.core.details;
using MeGUI.core.util;

namespace MeGUI
{
    public sealed class OneClickPostProcessing : ThreadJobProcessor<OneClickPostProcessingJob>
    {
        public static readonly JobProcessorFactory Factory = new JobProcessorFactory(new ProcessorFactory(init), "OneClickPostProcessing");

        private static IJobProcessor init(MainForm mf, Job j)
        {
            if (j is OneClickPostProcessingJob)
                return new OneClickPostProcessing();
            return null;
        }

        #region OneClick properties
        Dictionary<int, string> audioFiles;
        private AVCLevels al = new AVCLevels();
        private bool finished = false;
        private bool interlaced = false;
        private DeinterlaceFilter[] filters;
        private SourceDetector _sourceDetector = null;
        #endregion

        public OneClickPostProcessing() { }

        #region OneClickPostProcessor

        protected override void RunInThread()
        {
            JobChain c = null;
            List<string> intermediateFiles = new List<string>();
            bool bError = false;

            try
            {
                log.LogEvent("Processing thread started");
                su.Status = "Preprocessing...   ***PLEASE WAIT***";
                su.ResetTime();

                List<string> arrAudioFilesDelete = new List<string>();
                audioFiles = new Dictionary<int, string>();
                List<AudioTrackInfo> arrAudioTracks = new List<AudioTrackInfo>();
                List<AudioJob> arrAudioJobs = new List<AudioJob>();
                List<MuxStream> arrMuxStreams = new List<MuxStream>();
                FileUtil.ensureDirectoryExists(job.PostprocessingProperties.WorkingDirectory);

                // audio handling
                foreach (OneClickAudioTrack oAudioTrack in job.PostprocessingProperties.AudioTracks)
                {
                    if (IsJobStopped())
                        return;

                    if (oAudioTrack.AudioTrackInfo != null)
                    {
                        if (oAudioTrack.AudioTrackInfo.ExtractMKVTrack)
                        {
                            if (job.PostprocessingProperties.ApplyDelayCorrection && File.Exists(job.PostprocessingProperties.IntermediateMKVFile))
                            {
                                MediaInfoFile oFile = new MediaInfoFile(job.PostprocessingProperties.IntermediateMKVFile, ref log);
                                bool bFound = false;
                                foreach (AudioTrackInfo oAudioInfo in oFile.AudioInfo.Tracks)
                                {
                                    if (oAudioInfo.MMGTrackID == oAudioTrack.AudioTrackInfo.MMGTrackID)
                                        bFound = true;
                                }
                                int mmgTrackID = 0;
                                if (!bFound)
                                    mmgTrackID = oFile.AudioInfo.Tracks[oAudioTrack.AudioTrackInfo.TrackIndex].MMGTrackID;
                                else
                                    mmgTrackID = oAudioTrack.AudioTrackInfo.MMGTrackID;
                                foreach (AudioTrackInfo oAudioInfo in oFile.AudioInfo.Tracks)
                                {
                                    if (oAudioInfo.MMGTrackID == mmgTrackID)
                                    {
                                        if (oAudioTrack.DirectMuxAudio != null)
                                            oAudioTrack.DirectMuxAudio.delay = oAudioInfo.Delay;
                                        if (oAudioTrack.AudioJob != null)
                                            oAudioTrack.AudioJob.Delay = oAudioInfo.Delay;
                                        break;
                                    }
                                }
                            }
                            if (!audioFiles.ContainsKey(oAudioTrack.AudioTrackInfo.TrackID))
                            {
                                audioFiles.Add(oAudioTrack.AudioTrackInfo.TrackID, job.PostprocessingProperties.WorkingDirectory + "\\" + oAudioTrack.AudioTrackInfo.DemuxFileName);
                                arrAudioFilesDelete.Add(job.PostprocessingProperties.WorkingDirectory + "\\" + oAudioTrack.AudioTrackInfo.DemuxFileName);
                            }
                        }
                        else
                            arrAudioTracks.Add(oAudioTrack.AudioTrackInfo);
                    }
                    if (oAudioTrack.AudioJob != null)
                    {
                        if (job.PostprocessingProperties.IndexType == FileIndexerWindow.IndexType.NONE
                            && String.IsNullOrEmpty(oAudioTrack.AudioJob.Input))
                            oAudioTrack.AudioJob.Input = job.Input;
                        arrAudioJobs.Add(oAudioTrack.AudioJob);
                    }
                    if (oAudioTrack.DirectMuxAudio != null)
                        arrMuxStreams.Add(oAudioTrack.DirectMuxAudio);
                }
                if (audioFiles.Count == 0 && !job.PostprocessingProperties.Eac3toDemux
                    && job.PostprocessingProperties.IndexType != FileIndexerWindow.IndexType.NONE
                    && job.PostprocessingProperties.IndexType != FileIndexerWindow.IndexType.AVISOURCE)
                {

                    if ((job.PostprocessingProperties.IndexType == FileIndexerWindow.IndexType.DGI || job.PostprocessingProperties.IndexType == FileIndexerWindow.IndexType.DGM)
                        && File.Exists(Path.ChangeExtension(job.IndexFile, ".log")))
                    {
                        job.PostprocessingProperties.FilesToDelete.Add(Path.ChangeExtension(job.IndexFile, ".log"));
                        audioFiles = AudioUtil.GetAllDemuxedAudioFromDGI(arrAudioTracks, out arrAudioFilesDelete, job.IndexFile, log);
                    }
                    else
                        audioFiles = VideoUtil.getAllDemuxedAudio(arrAudioTracks, new List<AudioTrackInfo>(), out arrAudioFilesDelete, job.IndexFile, log);
                }

                FillInAudioInformation(ref arrAudioJobs, arrMuxStreams);

                if (!String.IsNullOrEmpty(job.PostprocessingProperties.VideoFileToMux))
                    log.LogEvent("Don't encode video: True");
                else
                    log.LogEvent("Desired size: " + job.PostprocessingProperties.OutputSize);
                log.LogEvent("Split size: " + job.PostprocessingProperties.Splitting);

                if (IsJobStopped())
                    return;

                // video file handling
                string avsFile = String.Empty;
                VideoStream myVideo = new VideoStream();
                VideoCodecSettings videoSettings = job.PostprocessingProperties.VideoSettings;
                if (String.IsNullOrEmpty(job.PostprocessingProperties.VideoFileToMux))
                {
                    //Open the video
                    try
                    {
                        avsFile = CreateAVSFile(job.IndexFile, job.Input, job.PostprocessingProperties.DAR,
                        job.PostprocessingProperties.HorizontalOutputResolution, log,
                        job.PostprocessingProperties.AvsSettings, job.PostprocessingProperties.AutoDeinterlace, videoSettings,
                        job.PostprocessingProperties.AutoCrop, job.PostprocessingProperties.KeepInputResolution,
                        job.PostprocessingProperties.UseChaptersMarks);
                    }
                    catch (Exception ex)
                    {
                        log.LogValue("An error occurred creating the AVS file", ex, ImageType.Error);
                    }

                    if (IsJobStopped())
                        return;

                    if (!String.IsNullOrEmpty(avsFile))
                    {
                        // check AVS file 
                        JobUtil.GetInputProperties(avsFile, out ulong frameCount, out double frameRate);

                        myVideo.Input = avsFile;
                        myVideo.Output = Path.Combine(job.PostprocessingProperties.WorkingDirectory, Path.GetFileNameWithoutExtension(job.Input) + "_Video");
                        myVideo.NumberOfFrames = frameCount;
                        myVideo.Framerate = (decimal)frameRate;
                        myVideo.VideoType = new MuxableType((new VideoEncoderProvider().GetSupportedOutput(videoSettings.EncoderType))[0], videoSettings.Codec);
                        myVideo.Settings = videoSettings;
                    }
                    else
                        bError = true;
                }
                else
                {
                    myVideo.DAR = job.PostprocessingProperties.ForcedDAR;
                    myVideo.Output = job.PostprocessingProperties.VideoFileToMux;
                    MediaInfoFile oInfo = new MediaInfoFile(myVideo.Output, ref log);
                    if (Path.GetExtension(job.PostprocessingProperties.VideoFileToMux).Equals(".unknown") && !String.IsNullOrEmpty(oInfo.ContainerFileTypeString))
                    {
                        job.PostprocessingProperties.VideoFileToMux = Path.ChangeExtension(job.PostprocessingProperties.VideoFileToMux, oInfo.ContainerFileTypeString.ToLowerInvariant());
                        File.Move(myVideo.Output, job.PostprocessingProperties.VideoFileToMux);
                        myVideo.Output = job.PostprocessingProperties.VideoFileToMux;
                        job.PostprocessingProperties.FilesToDelete.Add(myVideo.Output);
                    }

                    myVideo.Settings = videoSettings;
                    myVideo.Framerate = (decimal)oInfo.VideoInfo.FPS;
                    myVideo.NumberOfFrames = oInfo.VideoInfo.FrameCount;
                }

                if (IsJobStopped())
                    return;

                intermediateFiles.Add(avsFile);
                intermediateFiles.Add(job.IndexFile);
                intermediateFiles.AddRange(audioFiles.Values);
                foreach (string file in arrAudioFilesDelete)
                    intermediateFiles.Add(file);
                intermediateFiles.Add(Path.ChangeExtension(job.Input, ".log"));
                foreach (string file in job.PostprocessingProperties.FilesToDelete)
                    intermediateFiles.Add(file);

                // subtitle handling
                List<MuxStream> subtitles = new List<MuxStream>();
                if (job.PostprocessingProperties.SubtitleTracks.Count > 0)
                {
                    foreach (OneClickStream oTrack in job.PostprocessingProperties.SubtitleTracks)
                    {
                        if (oTrack.TrackInfo.ExtractMKVTrack)
                        {
                            //demuxed MKV
                            string trackFile = Path.GetDirectoryName(job.IndexFile) + "\\" + oTrack.TrackInfo.DemuxFileName;
                            if (File.Exists(trackFile))
                            {
                                intermediateFiles.Add(trackFile);
                                if (Path.GetExtension(trackFile).ToLowerInvariant().Equals(".idx"))
                                    intermediateFiles.Add(FileUtil.GetPathWithoutExtension(trackFile) + ".sub");

                                subtitles.Add(new MuxStream(trackFile, oTrack.Language, oTrack.Name, oTrack.Delay, oTrack.DefaultStream, oTrack.ForcedStream, null));
                            }
                            else
                                log.LogEvent("Ignoring subtitle as the it cannot be found: " + trackFile, ImageType.Warning);
                        }
                        else
                        {
                            // sometimes the language is detected differently by vsrip and the IFO parser. Therefore search also for other files
                            string strDemuxFile = oTrack.DemuxFilePath;
                            if (!File.Exists(strDemuxFile) && Path.GetFileNameWithoutExtension(strDemuxFile).Contains("_"))
                            {
                                string strDemuxFileName = Path.GetFileNameWithoutExtension(strDemuxFile);
                                strDemuxFileName = strDemuxFileName.Substring(0, strDemuxFileName.LastIndexOf("_")) + "_*" + Path.GetExtension(strDemuxFile);
                                foreach (string strFileName in Directory.GetFiles(Path.GetDirectoryName(strDemuxFile), strDemuxFileName))
                                {
                                    strDemuxFile = Path.Combine(Path.GetDirectoryName(strDemuxFile), strFileName);
                                    intermediateFiles.Add(strDemuxFile);
                                    intermediateFiles.Add(Path.ChangeExtension(strDemuxFile, ".sub"));
                                    log.LogEvent("Subtitle + " + oTrack.DemuxFilePath + " cannot be found. " + strFileName + " will be used instead", ImageType.Information);
                                    break;
                                }
                            }
                            if (File.Exists(strDemuxFile))
                            {
                                string strTrackName = oTrack.Name;

                                // check if a forced stream is available
                                string strForcedFile = Path.Combine(Path.GetDirectoryName(strDemuxFile), Path.GetFileNameWithoutExtension(strDemuxFile) + "_forced.idx");
                                if (File.Exists(strForcedFile))
                                {
                                    subtitles.Add(new MuxStream(strForcedFile, oTrack.Language, SubtitleUtil.ApplyForcedStringToTrackName(true, oTrack.Name), oTrack.Delay, oTrack.DefaultStream, true, null));
                                    intermediateFiles.Add(strForcedFile);
                                    intermediateFiles.Add(Path.ChangeExtension(strForcedFile, ".sub"));
                                }
                                subtitles.Add(new MuxStream(strDemuxFile, oTrack.Language, SubtitleUtil.ApplyForcedStringToTrackName(false, oTrack.Name), oTrack.Delay, oTrack.DefaultStream, (File.Exists(strForcedFile) ? false : oTrack.ForcedStream), null));
                            }
                            else
                                log.LogEvent("Ignoring subtitle as the it cannot be found: " + oTrack.DemuxFilePath, ImageType.Warning);
                        }
                    }
                }

                if (IsJobStopped())
                    return;
                    
                if (!bError)
                    c = VideoUtil.GenerateJobSeries(myVideo, job.PostprocessingProperties.FinalOutput, arrAudioJobs.ToArray(), 
                        subtitles.ToArray(), job.PostprocessingProperties.Attachments, job.PostprocessingProperties.TimeStampFile,
                        job.PostprocessingProperties.ChapterInfo, job.PostprocessingProperties.OutputSize,
                        job.PostprocessingProperties.Splitting, job.PostprocessingProperties.Container,
                        job.PostprocessingProperties.PrerenderJob, arrMuxStreams.ToArray(),
                        log, job.PostprocessingProperties.DeviceOutputType, null, job.PostprocessingProperties.VideoFileToMux, 
                        job.PostprocessingProperties.AudioTracks.ToArray(), true);

                if (c != null && !String.IsNullOrEmpty(job.PostprocessingProperties.TimeStampFile) &&
                    c.Jobs[c.Jobs.Length - 1].Job is MuxJob && (c.Jobs[c.Jobs.Length - 1].Job as MuxJob).MuxType == MuxerType.MP4BOX)
                {
                    // last job is a mp4box job and vfr timecode data has to be applied
                    MP4FpsModJob mp4FpsMod = new MP4FpsModJob(((MuxJob)c.Jobs[c.Jobs.Length - 1].Job).Output, job.PostprocessingProperties.TimeStampFile);
                    c = new SequentialChain(c, new SequentialChain(mp4FpsMod));
                }   
            }
            catch (Exception e)
            {
                log.LogValue("An error occurred", e, ImageType.Error);
                bError = true;
            }

            if (c == null || bError)
            {
                log.Error("Job creation aborted");
                su.HasError = true;
            }

            // add cleanup job also in case of an error
            c = CleanupJob.AddAfter(c, intermediateFiles, job.PostprocessingProperties.FinalOutput);
            MainForm.Instance.Jobs.AddJobsWithDependencies(c, false);

            // batch processing other input files if necessary
            if (job.PostprocessingProperties.FilesToProcess.Count > 0)
            {
                OneClickWindow ocw = new OneClickWindow();
                ocw.setBatchProcessing(job.PostprocessingProperties.FilesToProcess, job.PostprocessingProperties.OneClickSetting);
            }

            su.IsComplete = true;
        }

        private void FillInAudioInformation(ref List<AudioJob> arrAudioJobs, List<MuxStream> arrMuxStreams)
        {
            foreach (MuxStream m in arrMuxStreams)
                m.path = ConvertTrackNumberToFile(m.path, ref m.delay);

            List<AudioJob> tempList = new List<AudioJob>();
            foreach (AudioJob a in arrAudioJobs)
            {
                a.Input = ConvertTrackNumberToFile(a.Input, ref a.Delay);
                if (String.IsNullOrEmpty(a.Output) && !String.IsNullOrEmpty(a.Input))
                    a.Output = FileUtil.AddToFileName(a.Input, "_audio");
                if (!String.IsNullOrEmpty(a.Input))
                    tempList.Add(a);
            }
            arrAudioJobs = tempList;
        }

        /// <summary>
        /// if input is a track number (of the form, "::&lt;number&gt;::")
        /// then it returns the file path of that track number. Otherwise,
        /// it returns the string only
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private string ConvertTrackNumberToFile(string input, ref int delay)
        {
            if (String.IsNullOrEmpty(input))
            {
                log.Warn("Couldn't find audio file. Skipping track.");
                return null;
            }

            if (input.StartsWith("::") && input.EndsWith("::") && input.Length > 4)
            {
                string sub = input.Substring(2, input.Length - 4);
                try
                {
                    int t = int.Parse(sub);
                    string s = audioFiles[t];
                    if (PrettyFormatting.getDelay(s) != null)
                        delay = PrettyFormatting.getDelay(s) ?? 0;
                    return s;
                }
                catch (Exception)
                {
                    log.Warn(string.Format("Couldn't find audio file for track {0}. Skipping track.", input));
                    return null;
                }
            }

            return input;
        }
            
        /// <summary>
        /// creates the AVS Script file
        /// if the file can be properly opened, auto-cropping is performed, then depending on the AR settings
        /// the proper resolution for automatic resizing, taking into account the derived cropping values
        /// is calculated, and finally the avisynth script is written and its name returned
        /// </summary>
        /// <param name="path">dgindex script</param>
        /// <param name="aspectRatio">aspect ratio selection to be used</param>
        /// <param name="customDAR">custom display aspect ratio for this source</param>
        /// <param name="desiredOutputWidth">desired horizontal resolution of the output</param>
        /// <param name="settings">the codec settings (used only for x264)</param>
        /// <param name="sarX">pixel aspect ratio X</param>
        /// <param name="sarY">pixel aspect ratio Y</param>
        /// <param name="height">the final height of the video</param>
        /// <param name="autoCrop">whether or not autoCrop is used for the input</param>
        /// <returns>the name of the AviSynth script created, empty if there was an error</returns>
        private string CreateAVSFile(string indexFile, string inputFile, Dar? AR, int desiredOutputWidth,
            LogItem _log, AviSynthSettings avsSettings, bool autoDeint, VideoCodecSettings settings,
            bool autoCrop, bool keepInputResolution, bool useChaptersMarks)
        {
            Dar? dar = null;
            Dar customDAR;
            IMediaFile iMediaFile = null;
            IVideoReader reader;
            PossibleSources oPossibleSource;
            x264Device xTargetDevice = null;
            CropValues cropValues = new CropValues();

            int outputWidthIncludingPadding = 0;
            int outputHeightIncludingPadding = 0;
            int outputWidthCropped = 0;
            int outputHeightCropped = 0;

            // encode anamorph either when it is selected in the avs profile or the input resolution should not be touched
            bool signalAR = (avsSettings.Mod16Method != mod16Method.none) || keepInputResolution;

            // make sure the proper anamorphic encode is selected if the input resolution should not be touched
            if (keepInputResolution && avsSettings.Mod16Method != mod16Method.nonMod16)
                avsSettings.Mod16Method = mod16Method.nonMod16;

            // open index file to retrieve information
            if (job.PostprocessingProperties.IndexType == FileIndexerWindow.IndexType.DGI)
            {
                iMediaFile = new dgiFile(indexFile);
                oPossibleSource = PossibleSources.dgi;
            }
            else if (job.PostprocessingProperties.IndexType == FileIndexerWindow.IndexType.D2V)
            {
                iMediaFile = new d2vFile(indexFile);
                oPossibleSource = PossibleSources.d2v;
            }
            else if (job.PostprocessingProperties.IndexType == FileIndexerWindow.IndexType.DGM)
            {
                iMediaFile = new dgmFile(indexFile);
                oPossibleSource = PossibleSources.dgm;
            }
            else if (job.PostprocessingProperties.IndexType == FileIndexerWindow.IndexType.FFMS)
            {
                iMediaFile = new ffmsFile(inputFile, indexFile);
                oPossibleSource = PossibleSources.ffindex;
            }
            else if (job.PostprocessingProperties.IndexType == FileIndexerWindow.IndexType.LSMASH)
            {
                iMediaFile = new lsmashFile(inputFile, indexFile);
                oPossibleSource = PossibleSources.lsmash;
            }
            else if (job.PostprocessingProperties.IndexType == FileIndexerWindow.IndexType.AVISOURCE)
            {
                string tempAvs = "AVISource(\"" + inputFile + "\", audio=false)" + VideoUtil.getAssumeFPS(0, inputFile);
                iMediaFile = AvsFile.ParseScript(tempAvs, true);
                oPossibleSource = PossibleSources.avisource;
            }
            else
            {
                iMediaFile = AvsFile.OpenScriptFile(inputFile, true);
                oPossibleSource = PossibleSources.avs;
            }
            reader = iMediaFile.GetVideoReader();
            
            // abort if the index file is invalid
            if (reader.FrameCount < 1)
            {
                _log.Error("There are " + reader.FrameCount + " frames in the index file. Aborting...");
                return "";
            }

            if (AR == null)
            {
                // AR needs to be detected automatically now
                _log.LogValue("Auto-detect aspect ratio", AR == null);
                customDAR = iMediaFile.VideoInfo.DAR;
                if (customDAR.AR <= 0)
                {
                    customDAR = Dar.ITU16x9PAL;
                    _log.Warn(string.Format("No aspect ratio found, defaulting to {0}.", customDAR));
                }
            }
            else
                customDAR = AR.Value;
            _log.LogValue("Aspect ratio", customDAR);

            // check x264 settings (target device, chapter file)
            if (settings != null && settings is x264Settings)
            {
                x264Settings xs = (x264Settings)settings;
                xTargetDevice = xs.TargetDevice;
                _log.LogValue("Target device", xTargetDevice.Name);
            }

            // get mod value for resizing
            int mod = Resolution.GetModValue(avsSettings.ModValue, avsSettings.Mod16Method, signalAR);

            // crop input as it may be required (autoCrop && !keepInputResolution or Blu-Ray)
            if (Autocrop.autocrop(out cropValues, reader, signalAR, avsSettings.Mod16Method, avsSettings.ModValue) == false)
            {
                _log.Error("Autocrop failed. Aborting...");
                return "";
            }

            int inputWidth = (int)iMediaFile.VideoInfo.Width;
            int inputHeight = (int)iMediaFile.VideoInfo.Height;
            int inputFPS_D = (int)iMediaFile.VideoInfo.FPS_D;
            int inputFPS_N = (int)iMediaFile.VideoInfo.FPS_N;
            int inputFrameCount = (int)iMediaFile.VideoInfo.FrameCount;

            // force destruction of AVS script
            iMediaFile.Dispose();

            Dar? suggestedDar = null;
            if (desiredOutputWidth == 0)
                desiredOutputWidth = outputWidthIncludingPadding = inputWidth;
            else if (!avsSettings.Upsize && desiredOutputWidth > inputWidth)
                outputWidthIncludingPadding = inputWidth;
            else
                outputWidthIncludingPadding = desiredOutputWidth;
            CropValues paddingValues;

            bool resizeEnabled;
            int outputWidthWithoutUpsizing = outputWidthIncludingPadding;
            if (avsSettings.Upsize)
            {
                resizeEnabled = !keepInputResolution;
                CropValues cropValuesTemp = cropValues.Clone();
                int outputHeightIncludingPaddingTemp = 0;
                Resolution.GetResolution(inputWidth, inputHeight, customDAR,
                    ref cropValuesTemp, autoCrop && !keepInputResolution, mod, ref resizeEnabled, false, signalAR, true,
                    avsSettings.AcceptableAspectError, xTargetDevice, Convert.ToDouble(inputFPS_N) / inputFPS_D,
                    ref outputWidthWithoutUpsizing, ref outputHeightIncludingPaddingTemp, out paddingValues, out suggestedDar, _log);
            }

            resizeEnabled = !keepInputResolution;
            Resolution.GetResolution(inputWidth, inputHeight, customDAR,
                ref cropValues, autoCrop && !keepInputResolution, mod, ref resizeEnabled, avsSettings.Upsize, signalAR, true,
                avsSettings.AcceptableAspectError, xTargetDevice, Convert.ToDouble(inputFPS_N) / inputFPS_D, 
                ref outputWidthIncludingPadding, ref outputHeightIncludingPadding, out paddingValues, out suggestedDar, _log);
            keepInputResolution = !resizeEnabled;

            if (signalAR && suggestedDar.HasValue)
                dar = suggestedDar;

            // log calculated output resolution
            outputWidthCropped = outputWidthIncludingPadding - paddingValues.left - paddingValues.right;
            outputHeightCropped = outputHeightIncludingPadding - paddingValues.bottom - paddingValues.top;
            _log.LogValue("Input resolution", inputWidth + "x" + inputHeight);
            _log.LogValue("Desired maximum width", desiredOutputWidth);
            if (!avsSettings.Upsize && outputWidthIncludingPadding < desiredOutputWidth)
                _log.LogEvent("Desired maximum width not reached. Enable upsizing in the AviSynth profile if you want to force it.");
            if (avsSettings.Upsize && outputWidthIncludingPadding > outputWidthWithoutUpsizing)
                _log.LogValue("Desired maximum width reached with upsizing. Target width without upsizing", outputWidthWithoutUpsizing);
            if (cropValues.isCropped())
            {
                _log.LogValue("Autocrop values", cropValues);
                _log.LogValue("Cropped output resolution", outputWidthCropped + "x" + outputHeightCropped);
            }
            else
                _log.LogValue("Output resolution", outputWidthCropped + "x" + outputHeightCropped);
            if (paddingValues.isCropped())
                _log.LogValue("Padded output resolution", outputWidthIncludingPadding + "x" + outputHeightIncludingPadding);
            
            // generate the avs script based on the template
            string inputLine = "#input";
            string deinterlaceLines = "#deinterlace";
            string denoiseLines = "#denoise";
            string cropLine = "#crop";
            string resizeLine = "#resize";

            inputLine = ScriptServer.GetInputLine(
                inputFile, indexFile, false, oPossibleSource, false, false, false, 0, 
                avsSettings.DSS2, NvDeinterlacerType.nvDeInterlacerNone, 0, 0, null);

            if (IsJobStopped())
                return "";

            _log.LogValue("Automatic deinterlacing", autoDeint);
            if (autoDeint)
            {
                su.Status = "Automatic deinterlacing...   ***PLEASE WAIT***";
                string d2vPath = indexFile;
                _sourceDetector = new SourceDetector(inputLine, d2vPath, avsSettings.PreferAnimeDeinterlace, inputFrameCount,
                    Thread.CurrentThread.Priority,
                    MainForm.Instance.Settings.SourceDetectorSettings,
                    new UpdateSourceDetectionStatus(AnalyseUpdate),
                    new FinishedAnalysis(FinishedAnalysis));
                finished = false;
                _sourceDetector.Analyse();
                WaitTillAnalyseFinished();
                _sourceDetector = null;
                if (filters != null)
                {
                    deinterlaceLines = filters[0].Script;
                    if (interlaced)
                        _log.LogValue("Deinterlacing used", deinterlaceLines, ImageType.Warning);
                    else
                        _log.LogValue("Deinterlacing used", deinterlaceLines);
                }
            }

            if (IsJobStopped())
                return "";

            su.Status = "Finalizing preprocessing...   ***PLEASE WAIT***";

            // get final input filter line
            inputLine = ScriptServer.GetInputLine(
                inputFile, indexFile, interlaced, oPossibleSource, avsSettings.ColourCorrect, avsSettings.MPEG2Deblock,
                false, 0, avsSettings.DSS2, NvDeinterlacerType.nvDeInterlacerNone, 0, 0, null);

            // get crop & resize lines
            if (!keepInputResolution)
            {
                if (autoCrop)
                    cropLine = ScriptServer.GetCropLine(cropValues);
                resizeLine = ScriptServer.GetResizeLine(!signalAR || avsSettings.Mod16Method == mod16Method.resize || outputWidthIncludingPadding > 0 || inputWidth != outputWidthCropped,
                                                        outputWidthCropped, outputHeightCropped, outputWidthIncludingPadding, outputHeightIncludingPadding, (ResizeFilterType)avsSettings.ResizeMethod,
                                                        autoCrop, cropValues, inputWidth, inputHeight);
            }

            // get denoise line
            denoiseLines = ScriptServer.GetDenoiseLines(avsSettings.Denoise, (DenoiseFilterType)avsSettings.DenoiseMethod);

            string newScript = ScriptServer.CreateScriptFromTemplate(avsSettings.Template, inputLine, cropLine, resizeLine, denoiseLines, deinterlaceLines);

            if (dar.HasValue)
                newScript = string.Format("global MeGUI_darx = {0}\r\nglobal MeGUI_dary = {1}\r\n{2}", dar.Value.X, dar.Value.Y, newScript);
            else
            {
                if (xTargetDevice != null && xTargetDevice.BluRay)
                {
                    string strResolution = outputWidthIncludingPadding + "x" + outputHeightIncludingPadding;
                    x264Settings _xs = (x264Settings)settings;

                    if (strResolution.Equals("720x480"))
                    {
                        _xs.SampleAR = 4;
                        _log.LogEvent("Set --sar to 10:11 as only 40:33 or 10:11 are supported with a resolution of " +
                            strResolution + " as required for " + xTargetDevice.Name + ".");
                    }
                    else if (strResolution.Equals("720x576"))
                    {
                        _xs.SampleAR = 5;
                        _log.LogEvent("Set --sar to 12:11 as only 16:11 or 12:11 are supported with a resolution of "
                            + strResolution + " as required for " + xTargetDevice.Name + ".");
                    }
                    else if (strResolution.Equals("1280x720") || strResolution.Equals("1920x1080"))
                    {
                        _xs.SampleAR = 1;
                        _log.LogEvent("Set --sar to 1:1 as only 1:1 is supported with a resolution of "
                            + strResolution + " as required for " + xTargetDevice.Name + ".");
                    }
                    else if (strResolution.Equals("1440x1080"))
                    {
                        _xs.SampleAR = 2;
                        _log.LogEvent("Set --sar to 4:3 as only 4:3 is supported with a resolution of "
                            + strResolution + " as required for " + xTargetDevice.Name + ".");
                    }
                }
            }

            _log.LogValue("Generated AviSynth script", newScript);
            string strOutputAVSFile;
            if (String.IsNullOrEmpty(indexFile))
                strOutputAVSFile = Path.ChangeExtension(Path.Combine(job.PostprocessingProperties.WorkingDirectory, Path.GetFileName(inputFile)), ".avs");
            else
                strOutputAVSFile = Path.ChangeExtension(indexFile, ".avs");

            try
            {
                StreamWriter sw = new StreamWriter(strOutputAVSFile, false, System.Text.Encoding.Default);
                sw.Write(newScript);
                sw.Close();
            }
            catch (Exception i)
            {
                _log.LogValue("Error saving AviSynth script", i, ImageType.Error);
                return "";
            }

            JobUtil.GetAllInputProperties(strOutputAVSFile, out ulong numberOfFrames, out double fps, out int fps_n, out int fps_d, out int hres, out int vres, out Dar d, out AviSynthColorspace colorspace);
            _log.LogEvent("resolution: " + hres + "x" + vres);
            _log.LogEvent("frame rate: " + fps_n + "/" + fps_d);
            _log.LogEvent("frames: " + numberOfFrames);
            TimeSpan oTime = TimeSpan.FromSeconds((double)numberOfFrames / fps);
            _log.LogEvent("length: " + string.Format("{0:00}:{1:00}:{2:00}.{3:000}",
                (int)(oTime.TotalHours), oTime.Minutes, oTime.Seconds, oTime.Milliseconds));
            _log.LogValue("aspect ratio", d);
            _log.LogValue("color space", colorspace.ToString());

            if (IsJobStopped())
                return "";

            // create qpf file if necessary and possible 
            if (job.PostprocessingProperties.ChapterInfo.HasChapters && useChaptersMarks && settings != null && settings is x264Settings)
            {
                fps = (double)fps_n / fps_d;
                string strChapterFile = Path.ChangeExtension(strOutputAVSFile, ".qpf");
                job.PostprocessingProperties.ChapterInfo.ChangeFps(fps);
                if (job.PostprocessingProperties.ChapterInfo.SaveQpfile(strChapterFile))
                {
                    job.PostprocessingProperties.FilesToDelete.Add(strChapterFile);
                    _log.LogValue("qpf file created", strChapterFile);
                    x264Settings xs = (x264Settings)settings;
                    xs.UseQPFile = true;
                    xs.QPFile = strChapterFile;
                }
            }

            // check if a timestamp file has to be used
            if (!String.IsNullOrEmpty(job.PostprocessingProperties.TimeStampFile) && settings != null && settings is x264Settings)
            {
                x264Settings xs = (x264Settings)settings;
                xs.TCFile = job.PostprocessingProperties.TimeStampFile;
            }

            return strOutputAVSFile;
        }


        public void FinishedAnalysis(SourceInfo info, ExitType exit, string errorMessage)
        {
            if (exit != ExitType.OK)
            {
                if (exit == ExitType.ERROR)
                {
                    LogItem oSourceLog = log.LogEvent("Source detection");
                    if (!string.IsNullOrEmpty(errorMessage))
                        oSourceLog.LogValue("Source detection failed", errorMessage, ImageType.Warning);
                    else
                        oSourceLog.LogValue("Source detection failed", "An error occurred in source detection. Doing no processing", ImageType.Warning);
                    filters = new DeinterlaceFilter[] { new DeinterlaceFilter("Error", "#An error occurred in source detection. Doing no processing") };
                }
                interlaced = false;
            }
            else
            {
                LogItem oSourceLog = log.LogValue("Source detection", info.analysisResult);
                if (info.sourceType == SourceType.NOT_ENOUGH_SECTIONS || info.sourceType == SourceType.UNKNOWN)
                {
                    oSourceLog.LogEvent("Source detection failed: Could not find enough useful sections to determine source type for " + job.Input, ImageType.Warning);
                    filters = new DeinterlaceFilter[] { new DeinterlaceFilter("Error", "#Not enough useful sections for source detection. Doing no processing") };
                }
                else
                    this.filters = ScriptServer.GetDeinterlacers(info).ToArray();
                interlaced = (info.sourceType != SourceType.PROGRESSIVE);
            }
            finished = true;
        }

        public void AnalyseUpdate(int amountDone, int total)
        {
            try
            {
                decimal n = (decimal)amountDone / (decimal)total;
                if (n * 100M < su.PercentageDoneExact)
                    su.ResetTime();
                su.PercentageDoneExact = n * 100M;
                su.FillValues();
            }
            catch (Exception) { } // If we get any errors, just ignore -- it's only a cosmetic thing.
        }

        private void WaitTillAnalyseFinished()
        {
            while (!finished)
                MeGUI.core.util.Util.Wait(1000);
        }

        #endregion

        #region IJobProcessor Members
        public override void stop()
        {
            if (_sourceDetector != null)
                _sourceDetector.Stop();
            base.stop();
        }

        public override bool pause()
        {
            bool bResult = true;
            if (_sourceDetector != null)
                bResult = _sourceDetector.Pause();
            if (mre.Reset() && bResult)
                return true;
            return false;
        }

        public override bool resume()
        {
            bool bResult = true;
            if (_sourceDetector != null)
                bResult = _sourceDetector.Resume();
            if (mre.Set() && bResult)
                return true;
            return false;
        }

        public override void changePriority(WorkerPriorityType priority)
        {
            base.changePriority(priority);
            if (_sourceDetector != null)
                _sourceDetector.ChangePriority(priority);
        }
        #endregion
    }
}