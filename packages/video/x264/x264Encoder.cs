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
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Text;

using MeGUI.core.util;

namespace MeGUI
{
    class x264Encoder : CommandlineVideoEncoder
    {
        public static readonly JobProcessorFactory Factory = new JobProcessorFactory(new ProcessorFactory(init), "x264Encoder");

        private static IJobProcessor init(MainForm mf, Job j)
        {
            if (j is VideoJob && (j as VideoJob).Settings is x264Settings)
            {
                UpdateCacher.CheckPackage("x264");

                string encoderPath = mf.Settings.X264.Path;
                if (UseWrapper())
                {
                    UpdateCacher.CheckPackage("ffmpeg");
                    encoderPath = "cmd.exe";
                }
                    
                return new x264Encoder(encoderPath);
            }
            return null;
        }

        private static bool UseWrapper()
        {
            return (!MainForm.Instance.Settings.IsMeGUIx64 && MainForm.Instance.Settings.Usex64Tools);
        }

        public x264Encoder(string encoderPath) : base()
        {
            Executable = encoderPath;
            if (UseWrapper())
                IMinimumChildProcessCount = 1;
        }

        public override void ProcessLine(string line, StreamType stream, ImageType oType)
        {
            if (line.StartsWith("[")) // status update
            {
                int frameNumberStart = line.IndexOf("]", 4) + 2;
                int frameNumberEnd = line.IndexOf("/");
                if (frameNumberStart > 0 && frameNumberEnd > 0 && frameNumberEnd > frameNumberStart)
                    if (base.setFrameNumber(line.Substring(frameNumberStart, frameNumberEnd - frameNumberStart).Trim()))
                        return;
            }

            if (FileUtil.RegExMatch(line, @"^encoded \d+ frames", true))
                base.setFrameNumber(line.Substring(8, line.IndexOf(" frames") - 8));

            if (line.ToLowerInvariant().Contains("[error]:")
                || line.ToLowerInvariant().Contains("error:"))
                oType = ImageType.Error;
            else if (line.ToLowerInvariant().Contains("[warning]:")
                || line.ToLowerInvariant().Contains("warning:"))
                oType = ImageType.Warning;
            base.ProcessLine(line, stream, oType);
        }

        public static string genCommandline(string input, string output, Dar? d, int hres, int vres, int fps_n, int fps_d, ref ulong numberOfFrames, x264Settings _xs, Zone[] zones, LogItem log)
        {
            int qp;
            bool display = false;
            StringBuilder sb = new StringBuilder();
            CultureInfo ci = new CultureInfo("en-us");
            x264Settings xs = (x264Settings)_xs.Clone();
            MeGUI.packages.video.x264.x264SettingsHandler oSettingsHandler = new packages.video.x264.x264SettingsHandler(xs, log);

            // log
            if (log != null)
            {
                if (xs.TargetDevice.ID > 0)
                    log.LogEvent("target device selected: " + xs.TargetDevice.ToString());
                if (!String.IsNullOrEmpty(xs.CustomEncoderOptions))
                    log.LogEvent("custom command line: " + xs.CustomEncoderOptions);

                if (UseWrapper())
                {
                    // add executable
                    sb.Append("/c \"\"" + MainForm.Instance.Settings.FFmpeg.Path  + "\" -loglevel level+error -i \"" + input + "\" -strict -1 -f yuv4mpegpipe - | \"" + MainForm.Instance.Settings.X264.Path + "\" ");
                }
            }

            oSettingsHandler.getFPS(ref fps_n, ref fps_d);

            #region main tab
            ///<summary>
            /// x264 Main Tab Settings
            ///</summary>

            // AVC Profiles
            if (!xs.X26410Bits) // disable those profiles - not suitable for 10-bit encodings
            {
                xs.Profile = oSettingsHandler.getProfile();
                switch (xs.Profile)
                {
                    case 0: sb.Append("--profile baseline "); break;
                    case 1: sb.Append("--profile main "); break;
                    case 2: break; // --profile high is the default value
                }
            }

            // AVC Levels
            xs.AVCLevel = oSettingsHandler.getLevel();
            if (xs.AVCLevel != AVCLevels.Levels.L_UNRESTRICTED) // unrestricted
                sb.Append("--level " + AVCLevels.GetLevelText(xs.AVCLevel) + " ");

            // bit-depth
            if (xs.X26410Bits)
                sb.Append("--output-depth 10 ");

            // --bluray-compat
            xs.BlurayCompat = oSettingsHandler.getBlurayCompat();
            if (xs.BlurayCompat)
                sb.Append("--bluray-compat ");

            // x264 Presets
            if (!xs.CustomEncoderOptions.Contains("--preset "))
            {
                switch (xs.x264PresetLevel)
                {
                    case x264Settings.x264PresetLevelModes.ultrafast: sb.Append("--preset ultrafast "); break;
                    case x264Settings.x264PresetLevelModes.superfast: sb.Append("--preset superfast "); break;
                    case x264Settings.x264PresetLevelModes.veryfast: sb.Append("--preset veryfast "); break;
                    case x264Settings.x264PresetLevelModes.faster: sb.Append("--preset faster "); break;
                    case x264Settings.x264PresetLevelModes.fast: sb.Append("--preset fast "); break;
                    //case x264Settings.x264PresetLevelModes.medium: sb.Append("--preset medium "); break; // default value
                    case x264Settings.x264PresetLevelModes.slow: sb.Append("--preset slow "); break;
                    case x264Settings.x264PresetLevelModes.slower: sb.Append("--preset slower "); break;
                    case x264Settings.x264PresetLevelModes.veryslow: sb.Append("--preset veryslow "); break;
                    case x264Settings.x264PresetLevelModes.placebo: sb.Append("--preset placebo "); break;
                }
            }

            // x264 Tunings
            if (!xs.CustomEncoderOptions.Contains("--tune "))
            {
                switch (xs.x264PsyTuning)
                {
                    case x264Settings.x264PsyTuningModes.FILM: sb.Append("--tune film"); break;
                    case x264Settings.x264PsyTuningModes.ANIMATION: sb.Append("--tune animation"); break;
                    case x264Settings.x264PsyTuningModes.GRAIN: sb.Append("--tune grain"); break;
                    case x264Settings.x264PsyTuningModes.PSNR: sb.Append("--tune psnr"); break;
                    case x264Settings.x264PsyTuningModes.SSIM: sb.Append("--tune ssim"); break;
                    case x264Settings.x264PsyTuningModes.STILLIMAGE: sb.Append("--tune stillimage"); break;
                    default: break;
                }
                if (xs.TuneFastDecode || xs.TuneZeroLatency)
                {
                    string tune = String.Empty;
                    if (xs.TuneFastDecode)
                        tune = ",fastdecode";
                    if (xs.TuneZeroLatency)
                        tune += ",zerolatency";
                    if (!sb.ToString().Contains("--tune "))
                        sb.Append("--tune " + tune.Substring(1));
                    else
                        sb.Append(tune);
                }
                if (sb.ToString().Contains("--tune "))
                    sb.Append(" ");
            }

            // Encoding Modes
            switch (xs.VideoEncodingType)
            {
                case VideoCodecSettings.VideoEncodingMode.CBR:
                    if (!xs.CustomEncoderOptions.Contains("--bitrate ")) 
                        sb.Append("--bitrate " + xs.BitrateQuantizer + " ");
                    break;
                case VideoCodecSettings.VideoEncodingMode.CQ: // CQ
                    if (!xs.CustomEncoderOptions.Contains("--qp "))
                    {
                        qp = (int)xs.QuantizerCRF;
                        sb.Append("--qp " + qp.ToString(ci) + " ");
                    }
                    break;
                case VideoCodecSettings.VideoEncodingMode.twopass1: // 2 pass first pass
                    sb.Append("--pass 1 --bitrate " + xs.BitrateQuantizer + " --stats " + "\"" + xs.Logfile + "\" ");
                    break;
                case VideoCodecSettings.VideoEncodingMode.twopass2: // 2 pass second pass
                case VideoCodecSettings.VideoEncodingMode.twopassAutomated: // automated twopass
                    sb.Append("--pass 2 --bitrate " + xs.BitrateQuantizer + " --stats " + "\"" + xs.Logfile + "\" ");
                    break;
                case VideoCodecSettings.VideoEncodingMode.threepass1: // 3 pass first pass
                    sb.Append("--pass 1 --bitrate " + xs.BitrateQuantizer + " --stats " + "\"" + xs.Logfile + "\" ");
                    break;
                case VideoCodecSettings.VideoEncodingMode.threepass2: // 3 pass 2nd pass
                    sb.Append("--pass 3 --bitrate " + xs.BitrateQuantizer + " --stats " + "\"" + xs.Logfile + "\" ");
                    break;
                case VideoCodecSettings.VideoEncodingMode.threepass3: // 3 pass 3rd pass
                case VideoCodecSettings.VideoEncodingMode.threepassAutomated: // automated threepass, show third pass options
                    sb.Append("--pass 3 --bitrate " + xs.BitrateQuantizer + " --stats " + "\"" + xs.Logfile + "\" ");
                    break;
                case VideoCodecSettings.VideoEncodingMode.quality: // constant quality
                    if (!xs.CustomEncoderOptions.Contains("--crf "))
                        if (xs.QuantizerCRF != 23)
                            sb.Append("--crf " + xs.QuantizerCRF.ToString(ci) + " ");
                    break;
            } 

            // Slow 1st Pass
            if (!xs.CustomEncoderOptions.Contains("--slow-firstpass"))
                if ((xs.X264SlowFirstpass) && xs.x264PresetLevel < x264Settings.x264PresetLevelModes.placebo &&
                   ((xs.VideoEncodingType == VideoCodecSettings.VideoEncodingMode.twopass1) || // 2 pass first pass
                    (xs.VideoEncodingType == VideoCodecSettings.VideoEncodingMode.twopassAutomated) || // automated twopass
                    (xs.VideoEncodingType == VideoCodecSettings.VideoEncodingMode.threepass1) || // 3 pass first pass
                    (xs.VideoEncodingType == VideoCodecSettings.VideoEncodingMode.threepassAutomated)))  // automated threepass
                    sb.Append("--slow-firstpass ");

            // Threads
            if (!xs.CustomEncoderOptions.Contains("--thread-input"))
                if (xs.ThreadInput && xs.NbThreads == 1)
                    sb.Append("--thread-input ");
            if (!xs.CustomEncoderOptions.Contains("--threads "))
                if (xs.NbThreads > 0)
                    sb.Append("--threads " + xs.NbThreads + " ");
            #endregion

            #region frame-type tab
            ///<summary>
            /// x264 Frame-Type Tab Settings
            ///</summary>

            // H.264 Features
            if (xs.Deblock && !xs.CustomEncoderOptions.Contains("--no-deblock"))
            {
                if (!xs.CustomEncoderOptions.Contains("--deblock "))
                {
                    display = false;
                    switch (xs.x264PsyTuning)
                    {
                        case x264Settings.x264PsyTuningModes.FILM:          if (xs.AlphaDeblock != -1 || xs.BetaDeblock != -1) display = true; break;
                        case x264Settings.x264PsyTuningModes.ANIMATION:     if (xs.AlphaDeblock != 1 || xs.BetaDeblock != 1) display = true; break;
                        case x264Settings.x264PsyTuningModes.GRAIN:         if (xs.AlphaDeblock != -2 || xs.BetaDeblock != -2) display = true; break;
                        case x264Settings.x264PsyTuningModes.STILLIMAGE:    if (xs.AlphaDeblock != 3 || xs.BetaDeblock != -3) display = true; break;
                        default:                                            if (xs.AlphaDeblock != 0 || xs.BetaDeblock != 0) display = true; break;
                    }
                    if (display)
                        sb.Append("--deblock " + xs.AlphaDeblock + ":" + xs.BetaDeblock + " ");
                }
            }
            else
            {
                if (!xs.CustomEncoderOptions.Contains("--no-deblock"))
                    if (xs.x264PresetLevel != x264Settings.x264PresetLevelModes.ultrafast && !xs.TuneFastDecode)
                        sb.Append("--no-deblock ");
            }

            if (xs.Profile > 0 && !xs.CustomEncoderOptions.Contains("--no-cabac"))
            {
                if (!xs.Cabac)
                {
                    if (xs.x264PresetLevel != x264Settings.x264PresetLevelModes.ultrafast && !xs.TuneFastDecode)
                        sb.Append("--no-cabac ");
                }
            }

            // GOP Size
            int iBackupKeyframeInterval = xs.KeyframeInterval;
            int iBackupMinGOPSize = xs.MinGOPSize;

            xs.KeyframeInterval = oSettingsHandler.getKeyInt(fps_n, fps_d);
            if (xs.KeyframeInterval != 250) // gop size of 250 is default
            {
                if (xs.KeyframeInterval == 0)
                    sb.Append("--keyint infinite ");
                else
                    sb.Append("--keyint " + xs.KeyframeInterval + " ");
            }

            if (!xs.BlurayCompat)
            {
                xs.MinGOPSize = oSettingsHandler.getMinKeyint(fps_n, fps_d);
                if (xs.MinGOPSize > (xs.KeyframeInterval / 2 + 1))
                {
                    xs.MinGOPSize = xs.KeyframeInterval / 2 + 1;
                    if (log != null)
                        log.LogEvent("--min-keyint bigger as --keyint/2+1. Lowering --min-keyint to max value: " + xs.MinGOPSize, ImageType.Warning);
                }
                int iDefault = xs.KeyframeInterval / 10;
                if (log != null)
                    iDefault = Math.Min(xs.KeyframeInterval / 10, fps_n / fps_d);
                if (xs.MinGOPSize != iDefault) // (MIN(--keyint / 10,--fps)) is default
                    sb.Append("--min-keyint " + xs.MinGOPSize + " ");
            }

            xs.KeyframeInterval = iBackupKeyframeInterval;
            xs.MinGOPSize = iBackupMinGOPSize;

            if (!xs.CustomEncoderOptions.Contains("--open-gop") && (xs.OpenGopValue || xs.TargetDevice.BluRay))
                sb.Append("--open-gop ");

            // B-Frames
            xs.NbBframes = oSettingsHandler.getBFrames();
            if (xs.Profile > 0 && xs.NbBframes != x264Settings.GetDefaultNumberOfBFrames(xs.x264PresetLevel, xs.x264PsyTuning, xs.TuneZeroLatency, xs.Profile, null, xs.BlurayCompat))
                sb.Append("--bframes " + xs.NbBframes + " ");

            if (xs.NbBframes > 0)
            {
                if (!xs.CustomEncoderOptions.Contains("-b-adapt"))
                {
                    display = false;
                    if (xs.x264PresetLevel > x264Settings.x264PresetLevelModes.slow)
                    {
                        if (xs.NewAdaptiveBFrames != 2)
                            display = true;
                    }
                    else if (xs.x264PresetLevel > x264Settings.x264PresetLevelModes.ultrafast)
                    {
                        if (xs.NewAdaptiveBFrames != 1)
                            display = true;
                    }
                    else
                    {
                        if (xs.NewAdaptiveBFrames != 0)
                            display = true;
                    }
                    if (display)
                        sb.Append("--b-adapt " + xs.NewAdaptiveBFrames + " ");
                }

                xs.x264BFramePyramid = oSettingsHandler.getBPyramid();
                if (xs.NbBframes > 1 && ((xs.x264BFramePyramid != 2 && !xs.BlurayCompat) || (xs.x264BFramePyramid != 1 && xs.BlurayCompat)))
                {
                    switch (xs.x264BFramePyramid) // pyramid needs a minimum of 2 b frames
                    {
                        case 1: sb.Append("--b-pyramid strict "); break;
                        case 0: sb.Append("--b-pyramid none "); break;
                    }
                }

                if (!xs.CustomEncoderOptions.Contains("--no-weightb"))
                    if (!xs.WeightedBPrediction && !xs.TuneFastDecode && xs.x264PresetLevel != x264Settings.x264PresetLevelModes.ultrafast)
                        sb.Append("--no-weightb ");                    
            }

            // B-Frames bias
            if (!xs.CustomEncoderOptions.Contains("--b-bias "))
                if (xs.BframeBias != 0.0M)
                    sb.Append("--b-bias " + xs.BframeBias.ToString(ci) + " ");

            // Other
            if (xs.Scenecut)
            {
                if (!xs.CustomEncoderOptions.Contains("--scenecut "))
                    if ((xs.SCDSensitivity != 40M && xs.x264PresetLevel != x264Settings.x264PresetLevelModes.ultrafast) ||
                        (xs.SCDSensitivity != 0M && xs.x264PresetLevel == x264Settings.x264PresetLevelModes.ultrafast))
                        sb.Append("--scenecut " + xs.SCDSensitivity.ToString(ci) + " ");
            }
            else
            {
                if (!xs.CustomEncoderOptions.Contains("--no-scenecut"))
                    if (xs.x264PresetLevel != x264Settings.x264PresetLevelModes.ultrafast)
                        sb.Append("--no-scenecut ");
            }

            // reference frames
            int iRefFrames = oSettingsHandler.getRefFrames(hres, vres);
            if (iRefFrames != x264Settings.GetDefaultNumberOfRefFrames(xs.x264PresetLevel, xs.x264PsyTuning, null, xs.AVCLevel, xs.BlurayCompat, hres, vres))
                sb.Append("--ref " + iRefFrames + " ");

            // WeightedPPrediction
            xs.WeightedPPrediction = oSettingsHandler.getWeightp();
            if (xs.WeightedPPrediction != x264Settings.GetDefaultNumberOfWeightp(xs.x264PresetLevel, xs.TuneFastDecode, xs.Profile, xs.BlurayCompat))
                sb.Append("--weightp " + xs.WeightedPPrediction + " ");

            // Slicing
            xs.SlicesNb = oSettingsHandler.getSlices();
            if (xs.SlicesNb != 0)
                sb.Append("--slices " + xs.SlicesNb + " ");
            if (!xs.CustomEncoderOptions.Contains("--slice-max-size "))
                if (xs.MaxSliceSyzeBytes != 0)
                    sb.Append("--slice-max-size " + xs.MaxSliceSyzeBytes + " ");
            if (!xs.CustomEncoderOptions.Contains("--slice-max-mbs "))
                if (xs.MaxSliceSyzeMBs != 0)
                    sb.Append("--slice-max-mbs " + xs.MaxSliceSyzeMBs + " ");
            #endregion

            #region rc tab
            ///<summary>
            /// x264 Rate Control Tab Settings
            /// </summary>


            if (!xs.CustomEncoderOptions.Contains("--qpmin "))
                if (xs.MinQuantizer != 0)
                    sb.Append("--qpmin " + xs.MinQuantizer + " ");
            if (!xs.CustomEncoderOptions.Contains("--qpmax "))
                if ((xs.X26410Bits && xs.MaxQuantizer != 81) || (!xs.X26410Bits && xs.MaxQuantizer < 69))
                    sb.Append("--qpmax " + xs.MaxQuantizer + " ");
            if (!xs.CustomEncoderOptions.Contains("--qpstep "))
                if (xs.MaxQuantDelta != 4)
                    sb.Append("--qpstep " + xs.MaxQuantDelta + " ");

            if (xs.IPFactor != 1.4M)
            {
                if (!xs.CustomEncoderOptions.Contains("--ipratio "))
                {
                    if (xs.x264PsyTuning != x264Settings.x264PsyTuningModes.GRAIN || xs.IPFactor != 1.1M)
                        sb.Append("--ipratio " + xs.IPFactor.ToString(ci) + " ");
                }
            }

            if (xs.PBFactor != 1.3M) 
            {
                if (!xs.CustomEncoderOptions.Contains("--pbratio "))
                {
                    if (xs.x264PsyTuning != x264Settings.x264PsyTuningModes.GRAIN || xs.PBFactor != 1.1M)
                        sb.Append("--pbratio " + xs.PBFactor.ToString(ci) + " ");
                }
            }

            if (!xs.CustomEncoderOptions.Contains("--chroma-qp-offset "))
                if (xs.ChromaQPOffset != 0.0M)
                    sb.Append("--chroma-qp-offset " + xs.ChromaQPOffset.ToString(ci) + " ");

            if (xs.VideoEncodingType != VideoCodecSettings.VideoEncodingMode.CQ) // doesn't apply to CQ mode
            {
                xs.VBVBufferSize = oSettingsHandler.getVBVBufsize(xs.AVCLevel, xs.Profile == 2);
                if (xs.VBVBufferSize > 0)
                    sb.Append("--vbv-bufsize " + xs.VBVBufferSize + " ");
                xs.VBVMaxBitrate = oSettingsHandler.getVBVMaxrate(xs.AVCLevel, xs.Profile == 2);
                if (xs.VBVMaxBitrate > 0)
                    sb.Append("--vbv-maxrate " + xs.VBVMaxBitrate + " ");
                if (!xs.CustomEncoderOptions.Contains("--vbv-init "))
                    if (xs.VBVInitialBuffer != 0.9M)
                        sb.Append("--vbv-init " + xs.VBVInitialBuffer.ToString(ci) + " ");
                if (!xs.CustomEncoderOptions.Contains("--ratetol "))
                    if (xs.BitrateVariance != 1.0M)
                        sb.Append("--ratetol " + xs.BitrateVariance.ToString(ci) + " ");

                if (!xs.CustomEncoderOptions.Contains("--qcomp "))
                {
                    display = true;
                    if ((xs.x264PsyTuning == x264Settings.x264PsyTuningModes.GRAIN && xs.QuantCompression == 0.8M) 
                        || (xs.x264PsyTuning != x264Settings.x264PsyTuningModes.GRAIN && xs.QuantCompression == 0.6M))
                        display = false;
                    if (display)
                        sb.Append("--qcomp " + xs.QuantCompression.ToString(ci) + " ");
                }

                if (xs.VideoEncodingType != VideoCodecSettings.VideoEncodingMode.CBR 
                    && xs.VideoEncodingType != VideoCodecSettings.VideoEncodingMode.CQ
                    && xs.VideoEncodingType != VideoCodecSettings.VideoEncodingMode.quality) // applies only to twopass
                {
                    if (!xs.CustomEncoderOptions.Contains("--cplxblur "))
                        if (xs.TempComplexityBlur != 20)
                            sb.Append("--cplxblur " + xs.TempComplexityBlur.ToString(ci) + " ");
                    if (!xs.CustomEncoderOptions.Contains("--qblur "))
                        if (xs.TempQuanBlurCC != 0.5M)
                            sb.Append("--qblur " + xs.TempQuanBlurCC.ToString(ci) + " ");
                }
            }

            // Dead Zones
            if (!xs.CustomEncoderOptions.Contains("--deadzone-inter "))
            {
                display = true;
                if ((xs.x264PsyTuning != x264Settings.x264PsyTuningModes.GRAIN && xs.DeadZoneInter == 21 && xs.DeadZoneIntra == 11) 
                    || (xs.x264PsyTuning == x264Settings.x264PsyTuningModes.GRAIN && xs.DeadZoneInter == 6 && xs.DeadZoneIntra == 6))
                    display = false;
                if (display)
                    sb.Append("--deadzone-inter " + xs.DeadZoneInter + " ");
            }

            if (!xs.CustomEncoderOptions.Contains("--deadzone-intra "))
            {
                display = true;
                if ((xs.x264PsyTuning != x264Settings.x264PsyTuningModes.GRAIN && xs.DeadZoneIntra == 11) 
                    || (xs.x264PsyTuning == x264Settings.x264PsyTuningModes.GRAIN && xs.DeadZoneIntra == 6))
                    display = false;
                if (display)
                    sb.Append("--deadzone-intra " + xs.DeadZoneIntra + " ");
            }

            // Disable Macroblok Tree
            if (!xs.CustomEncoderOptions.Contains("--no-mbtree"))
                if (!xs.NoMBTree && xs.x264PresetLevel > x264Settings.x264PresetLevelModes.veryfast && !xs.TuneZeroLatency)
                    sb.Append("--no-mbtree ");

            // RC Lookahead
            if (!xs.CustomEncoderOptions.Contains("--rc-lookahead "))
                if (xs.Lookahead != x264Settings.GetDefaultRCLookahead(xs.x264PresetLevel, xs.TuneZeroLatency))
                    sb.Append("--rc-lookahead " + xs.Lookahead + " ");

            // AQ-Mode
            if (xs.VideoEncodingType != VideoCodecSettings.VideoEncodingMode.CQ)
            {
                if (!xs.CustomEncoderOptions.Contains("--aq-mode "))
                    if (xs.AQmode != x264Settings.GetDefaultAQMode(xs.x264PresetLevel, xs.x264PsyTuning))
                        sb.Append("--aq-mode " + xs.AQmode.ToString() + " ");

                if (!xs.CustomEncoderOptions.Contains("--aq-strength "))
                {
                    if (xs.AQmode > 0)
                    {
                        display = false;
                        switch (xs.x264PsyTuning)
                        {
                            case x264Settings.x264PsyTuningModes.ANIMATION:     if (xs.AQstrength != 0.6M) display = true; break;
                            case x264Settings.x264PsyTuningModes.GRAIN:         if (xs.AQstrength != 0.5M) display = true; break;
                            case x264Settings.x264PsyTuningModes.STILLIMAGE:    if (xs.AQstrength != 1.2M) display = true; break;
                            default:                                            if (xs.AQstrength != 1.0M) display = true; break;
                        }
                        if (display)
                            sb.Append("--aq-strength " + xs.AQstrength.ToString(ci) + " ");
                    }
                }
            }

            // custom matrices 
            if (xs.Profile > 1 && xs.QuantizerMatrixType > 0)
            {
                switch (xs.QuantizerMatrixType)
                {
                    case 1: if (!xs.CustomEncoderOptions.Contains("--cqm ")) sb.Append("--cqm \"jvt\" "); break;
                    case 2: if (!xs.CustomEncoderOptions.Contains("--cqmfile"))
                            {
                                if (log != null && !System.IO.File.Exists(xs.QuantizerMatrix))
                                    log.LogEvent(xs.QuantizerMatrix + " not found. --cqmfile will be skipped.", ImageType.Warning);
                                else
                                    sb.Append("--cqmfile \"" + xs.QuantizerMatrix + "\" ");
                            } break;
                }
            }

            #endregion

            #region analysis tab
            ///<summary>
            /// x264 Analysis Tab Settings
            /// </summary>

            // Disable Chroma Motion Estimation
            if (!xs.CustomEncoderOptions.Contains("--no-chroma-me"))    
                if (!xs.ChromaME)
                    sb.Append("--no-chroma-me ");

            // Motion Estimation Range
            if (!xs.CustomEncoderOptions.Contains("--merange "))   
            {
                if ((xs.x264PresetLevel <= x264Settings.x264PresetLevelModes.slower && xs.MERange != 16) ||
                    (xs.x264PresetLevel >= x264Settings.x264PresetLevelModes.veryslow && xs.MERange != 24))
                    sb.Append("--merange " + xs.MERange + " ");
            }

            // ME Type
            if (!xs.CustomEncoderOptions.Contains("--me "))
            {
                display = false;
                switch (xs.x264PresetLevel)
                {
                    case x264Settings.x264PresetLevelModes.ultrafast:
                    case x264Settings.x264PresetLevelModes.superfast:   if (xs.METype != 0) display = true; break;
                    case x264Settings.x264PresetLevelModes.veryfast: 
                    case x264Settings.x264PresetLevelModes.faster:
                    case x264Settings.x264PresetLevelModes.fast:
                    case x264Settings.x264PresetLevelModes.medium:
                    case x264Settings.x264PresetLevelModes.slow:        if (xs.METype != 1) display = true; break;
                    case x264Settings.x264PresetLevelModes.slower:
                    case x264Settings.x264PresetLevelModes.veryslow:    if (xs.METype != 2) display = true; break;
                    case x264Settings.x264PresetLevelModes.placebo:     if (xs.METype != 4) display = true; break;
                }

                if (display)
                {
                    switch (xs.METype)
                    {
                        case 0: sb.Append("--me dia "); break;
                        case 1: sb.Append("--me hex "); break;
                        case 2: sb.Append("--me umh "); break;
                        case 3: sb.Append("--me esa "); break;
                        case 4: sb.Append("--me tesa "); break;
                    }
                }

            }

            if (!xs.CustomEncoderOptions.Contains("--direct "))
            {
                display = false;
                if (xs.x264PresetLevel > x264Settings.x264PresetLevelModes.medium)
                {
                    if (xs.BframePredictionMode != 3)
                        display = true;
                }
                else if (xs.BframePredictionMode != 1)
                    display = true;
                
                if (display)
                {
                    switch (xs.BframePredictionMode)
                    {
                        case 0: sb.Append("--direct none "); break;
                        case 1: sb.Append("--direct spatial "); break;
                        case 2: sb.Append("--direct temporal "); break;
                        case 3: sb.Append("--direct auto "); break;
                    }
                }
            }

            if (!xs.CustomEncoderOptions.Contains("--nr "))
                if (xs.NoiseReduction > 0)
                    sb.Append("--nr " + xs.NoiseReduction + " ");
      

            // subpel refinement
            if (!xs.CustomEncoderOptions.Contains("--subme "))
            {
                display = false;
                switch (xs.x264PresetLevel)
                {
                    case x264Settings.x264PresetLevelModes.ultrafast:   if (xs.SubPelRefinement != 0) display = true; break;
                    case x264Settings.x264PresetLevelModes.superfast:   if (xs.SubPelRefinement != 1) display = true; break;
                    case x264Settings.x264PresetLevelModes.veryfast:    if (xs.SubPelRefinement != 2) display = true; break;
                    case x264Settings.x264PresetLevelModes.faster:      if (xs.SubPelRefinement != 4) display = true; break;
                    case x264Settings.x264PresetLevelModes.fast:        if (xs.SubPelRefinement != 6) display = true; break;
                    case x264Settings.x264PresetLevelModes.medium:      if (xs.SubPelRefinement != 7) display = true; break;
                    case x264Settings.x264PresetLevelModes.slow:        if (xs.SubPelRefinement != 8) display = true; break;
                    case x264Settings.x264PresetLevelModes.slower:      if (xs.SubPelRefinement != 9) display = true; break;
                    case x264Settings.x264PresetLevelModes.veryslow:    if (xs.SubPelRefinement != 10) display = true; break;
                    case x264Settings.x264PresetLevelModes.placebo:     if (xs.SubPelRefinement != 11) display = true; break;
                }
                if (display)
                    sb.Append("--subme " + (xs.SubPelRefinement) + " ");
            }

            // macroblock types
            if (!xs.CustomEncoderOptions.Contains("--partitions "))
            {
                bool bExpectedP8x8mv = true;
                bool bExpectedB8x8mv = true;
                bool bExpectedI4x4mv = true;
                bool bExpectedI8x8mv = true;
                bool bExpectedP4x4mv = true;

                switch (xs.x264PresetLevel) 
                {
                    case x264Settings.x264PresetLevelModes.ultrafast:   bExpectedP8x8mv = false; bExpectedB8x8mv = false; bExpectedI4x4mv = false; 
                                                                        bExpectedI8x8mv = false; bExpectedP4x4mv = false; break;
                    case x264Settings.x264PresetLevelModes.superfast:   bExpectedP8x8mv = false; bExpectedB8x8mv = false; bExpectedP4x4mv = false; break;
                    case x264Settings.x264PresetLevelModes.veryfast:
                    case x264Settings.x264PresetLevelModes.faster:
                    case x264Settings.x264PresetLevelModes.fast:
                    case x264Settings.x264PresetLevelModes.medium:
                    case x264Settings.x264PresetLevelModes.slow:        bExpectedP4x4mv = false; break;
                }

                if (xs.Profile < 2)
                    bExpectedI8x8mv = false;

                if (bExpectedP8x8mv != xs.P8x8mv || bExpectedB8x8mv != xs.B8x8mv 
                    || bExpectedI4x4mv != xs.I4x4mv || bExpectedI8x8mv != xs.I8x8mv 
                    || bExpectedP4x4mv != xs.P4x4mv)
                {
                    if (xs.P8x8mv || xs.B8x8mv || xs.I4x4mv || xs.I8x8mv || xs.P4x4mv)
                    {
                        sb.Append("--partitions ");
                        if (xs.I4x4mv && xs.I8x8mv && xs.P4x4mv && xs.P8x8mv && xs.B8x8mv)
                            sb.Append("all ");
                        else
                        {
                            if (xs.P8x8mv) // default is checked
                                sb.Append("p8x8,");
                            if (xs.P4x4mv) // default is unchecked
                                sb.Append("p4x4,");
                            if (xs.B8x8mv) // default is checked
                                sb.Append("b8x8,");
                            if (xs.I8x8mv) // default is checked
                                sb.Append("i8x8");
                            if (xs.I4x4mv) // default is checked
                                sb.Append("i4x4,");
                            if (sb.ToString().EndsWith(","))
                                sb.Remove(sb.Length - 1, 1);
                        }

                        if (!sb.ToString().EndsWith(" "))
                            sb.Append(" ");
                    }
                    else
                        sb.Append("--partitions none ");
                }
            }

            if (xs.Profile > 1  && !xs.CustomEncoderOptions.Contains("--no-8x8dct"))
                if (!xs.AdaptiveDCT)
                    if (xs.x264PresetLevel > x264Settings.x264PresetLevelModes.ultrafast)
                        sb.Append("--no-8x8dct ");

            // Trellis
            if (!xs.CustomEncoderOptions.Contains("--trellis "))
            {
                display = false;
                switch (xs.x264PresetLevel)
                {
                    case x264Settings.x264PresetLevelModes.ultrafast: 
                    case x264Settings.x264PresetLevelModes.superfast:
                    case x264Settings.x264PresetLevelModes.veryfast:    if (xs.X264Trellis != 0) display = true; break;
                    case x264Settings.x264PresetLevelModes.faster: 
                    case x264Settings.x264PresetLevelModes.fast: 
                    case x264Settings.x264PresetLevelModes.medium:      if (xs.X264Trellis != 1) display = true; break;
                    case x264Settings.x264PresetLevelModes.slow:
                    case x264Settings.x264PresetLevelModes.slower: 
                    case x264Settings.x264PresetLevelModes.veryslow:
                    case x264Settings.x264PresetLevelModes.placebo:     if (xs.X264Trellis != 2) display = true; break;
                }
                if (display)
                    sb.Append("--trellis " + xs.X264Trellis + " ");
            }

            if (!xs.CustomEncoderOptions.Contains("--no-psy"))
                if (xs.NoPsy && (xs.x264PsyTuning != x264Settings.x264PsyTuningModes.PSNR && xs.x264PsyTuning != x264Settings.x264PsyTuningModes.SSIM))
                    sb.Append("--no-psy ");

            if (!xs.CustomEncoderOptions.Contains("--psy-rd "))
            {
                if (xs.SubPelRefinement > 5)
                {
                    display = false;
                    if (!xs.CustomEncoderOptions.Contains("--no-psy") && !xs.NoPsy)
                    {
                        switch (xs.x264PsyTuning)
                        {
                            case x264Settings.x264PsyTuningModes.FILM: if ((xs.PsyRDO != 1.0M) || (xs.PsyTrellis != 0.15M)) display = true; break;
                            case x264Settings.x264PsyTuningModes.ANIMATION: if ((xs.PsyRDO != 0.4M) || (xs.PsyTrellis != 0.0M)) display = true; break;
                            case x264Settings.x264PsyTuningModes.GRAIN: if ((xs.PsyRDO != 1.0M) || (xs.PsyTrellis != 0.25M)) display = true; break;
                            case x264Settings.x264PsyTuningModes.STILLIMAGE: if ((xs.PsyRDO != 2.0M) || (xs.PsyTrellis != 0.7M)) display = true; break;
                            default: if ((xs.PsyRDO != 1.0M) || (xs.PsyTrellis != 0.0M)) display = true; break;
                        }
                    }

                    if (display)
                        sb.Append("--psy-rd " + xs.PsyRDO.ToString(ci) + ":" + xs.PsyTrellis.ToString(ci) + " ");
                }
            }

            if (!xs.CustomEncoderOptions.Contains("--no-mixed-refs"))
                if (xs.NoMixedRefs)
                    if (xs.x264PresetLevel >= x264Settings.x264PresetLevelModes.fast)
                        sb.Append("--no-mixed-refs ");

            if (!xs.CustomEncoderOptions.Contains("--no-dct-decimate"))
                if (xs.NoDCTDecimate)
                    if (xs.x264PsyTuning != x264Settings.x264PsyTuningModes.GRAIN)
                        sb.Append("--no-dct-decimate ");

            if (!xs.CustomEncoderOptions.Contains("--no-fast-pskip"))
                if (xs.NoFastPSkip)
                    if (xs.x264PresetLevel != x264Settings.x264PresetLevelModes.placebo)
                        sb.Append("--no-fast-pskip ");

            xs.X264Aud = oSettingsHandler.getAud();
            if (xs.X264Aud && !xs.BlurayCompat)
                sb.Append("--aud ");

            xs.Nalhrd = oSettingsHandler.getNalHrd();
            switch (xs.Nalhrd)
            {
                case 1: if (!xs.BlurayCompat) sb.Append("--nal-hrd vbr "); break;
                case 2: sb.Append("--nal-hrd cbr "); break;
            }

            if (!xs.CustomEncoderOptions.Contains("--non-deterministic"))
                if (xs.NonDeterministic)
                    sb.Append("--non-deterministic ");            
            #endregion

            #region misc tab
            ///<summary>
            /// x264 Misc Tab Settings
            /// </summary>

            // QPFile
            if (!xs.CustomEncoderOptions.Contains("--qpfile "))
            {
                if (xs.UseQPFile)
                {
                    if (xs.VideoEncodingType == VideoCodecSettings.VideoEncodingMode.CBR
                        || xs.VideoEncodingType == VideoCodecSettings.VideoEncodingMode.CQ
                        || xs.VideoEncodingType == VideoCodecSettings.VideoEncodingMode.twopass1
                        || xs.VideoEncodingType == VideoCodecSettings.VideoEncodingMode.threepass1
                        || xs.VideoEncodingType == VideoCodecSettings.VideoEncodingMode.quality)
                    {
                        if (log != null && !System.IO.File.Exists(xs.QPFile))
                            log.LogEvent(xs.QPFile + " not found. --qpfile will be skipped.", ImageType.Warning);
                        else
                            sb.Append("--qpfile " + "\"" + xs.QPFile + "\" ");
                    }
                }
            }

            if (!xs.CustomEncoderOptions.Contains("--psnr"))
                if (xs.PSNRCalculation)
                    sb.Append("--psnr ");

            if (!xs.CustomEncoderOptions.Contains("--ssim"))
                if (xs.SSIMCalculation)
                    sb.Append("--ssim ");

            if (!xs.CustomEncoderOptions.Contains("--range "))    
                if (!xs.Range.Equals("auto"))
                    sb.Append("--range " + xs.Range + " ");

            if (!xs.CustomEncoderOptions.Contains("--stitchable"))
                if (xs.StitchAble)
                    sb.Append("--stitchable ");

            #endregion

            #region zones
            if (zones != null && zones.Length > 0)
            {
                StringBuilder sbZones = new StringBuilder();
                foreach (Zone zone in zones)
                {
                    if (zone.startFrame >= zone.endFrame)
                    {
                        if (log != null)
                            log.LogEvent("invalid zone ignored: start=" + zone.startFrame + " end=" + zone.endFrame, ImageType.Warning);
                        continue;
                    }

                    sbZones.Append(zone.startFrame + "," + zone.endFrame + ",");
                    if (zone.mode == ZONEMODE.Quantizer && zone.modifier >= 0 && zone.modifier <= 51)
                    {
                        sbZones.Append("q=");
                        sbZones.Append(zone.modifier.ToString(ci) + "/");
                    }
                    else
                    {
                        sbZones.Append("b=");
                        double mod = (double)zone.modifier / 100.0;
                        sbZones.Append(mod.ToString(ci) + "/");
                    }
                }
                if (sbZones.Length > 0)
                {
                    sbZones.Remove(sbZones.Length - 1, 1);
                    sb.Append("--zones ");
                    sb.Append(sbZones);
                    sb.Append(" ");
                }
            }
            #endregion

            #region input / ouput / custom

            // Call twice as they may be changed during CheckInputFile()
            string CustomSarValue;
            xs.SampleAR = oSettingsHandler.getSar(d, hres, vres, out CustomSarValue, String.Empty);
            xs.ColorPrim = oSettingsHandler.getColorprim();
            xs.Transfer = oSettingsHandler.getTransfer();
            xs.ColorMatrix = oSettingsHandler.getColorMatrix();
            xs.InterlacedMode = oSettingsHandler.getInterlacedMode();
            xs.FakeInterlaced = oSettingsHandler.getFakeInterlaced();
            xs.PicStruct = oSettingsHandler.getPicStruct();
            xs.X264PullDown = oSettingsHandler.getPulldown();

            oSettingsHandler.CheckInputFile(d, hres, vres, fps_n, fps_d);

            xs.InterlacedMode = oSettingsHandler.getInterlacedMode();
            switch (xs.InterlacedMode)
            {
                case x264Settings.x264InterlacedModes.bff: sb.Append("--bff "); break;
                case x264Settings.x264InterlacedModes.tff: sb.Append("--tff "); break;
            }

            xs.FakeInterlaced = oSettingsHandler.getFakeInterlaced();
            if (xs.FakeInterlaced && xs.InterlacedMode == x264Settings.x264InterlacedModes.progressive)
                sb.Append("--fake-interlaced ");

            xs.PicStruct = oSettingsHandler.getPicStruct();
            if (xs.PicStruct && xs.InterlacedMode == x264Settings.x264InterlacedModes.progressive && xs.X264PullDown == 0)
                sb.Append("--pic-struct ");

            xs.ColorPrim = oSettingsHandler.getColorprim();
            switch (xs.ColorPrim)
            {
                case 0: break;
                case 1: sb.Append("--colorprim bt709 "); break;
                case 2: sb.Append("--colorprim bt470m "); break;
                case 3: sb.Append("--colorprim bt470bg "); break;
                case 4: sb.Append("--colorprim smpte170m "); break;
                case 5: sb.Append("--colorprim smpte240m "); break;
                case 6: sb.Append("--colorprim film "); break;
            }

            xs.Transfer = oSettingsHandler.getTransfer();
            switch (xs.Transfer)
            {
                case 0: break;
                case 1: sb.Append("--transfer bt709 "); break;
                case 2: sb.Append("--transfer bt470m "); break;
                case 3: sb.Append("--transfer bt470bg "); break;
                case 4: sb.Append("--transfer linear "); break;
                case 5: sb.Append("--transfer log100 "); break;
                case 6: sb.Append("--transfer log316 "); break;
                case 7: sb.Append("--transfer smpte170m "); break;
                case 8: sb.Append("--transfer smpte240m "); break;
            }

            xs.ColorMatrix = oSettingsHandler.getColorMatrix();
            switch (xs.ColorMatrix)
            {
                case 0: break;
                case 1: sb.Append("--colormatrix bt709 "); break;
                case 2: sb.Append("--colormatrix fcc "); break;
                case 3: sb.Append("--colormatrix bt470bg "); break;
                case 4: sb.Append("--colormatrix smpte170m "); break;
                case 5: sb.Append("--colormatrix smpte240m "); break;
                case 6: sb.Append("--colormatrix GBR "); break;
                case 7: sb.Append("--colormatrix YCgCo "); break;
            }

            xs.X264PullDown = oSettingsHandler.getPulldown();
            switch (xs.X264PullDown)
            {
                case 0: break;
                case 1: sb.Append("--pulldown 22 "); break;
                case 2: sb.Append("--pulldown 32 "); break;
                case 3: sb.Append("--pulldown 64 "); break;
                case 4: sb.Append("--pulldown double "); break;
                case 5: sb.Append("--pulldown triple "); break;
                case 6: sb.Append("--pulldown euro "); break;
            }

            // get number of frames to encode
            oSettingsHandler.getFrames(ref numberOfFrames);

            xs.CustomEncoderOptions = oSettingsHandler.getCustomCommandLine();
            if (!String.IsNullOrEmpty(xs.CustomEncoderOptions)) // add custom encoder options
                sb.Append(xs.CustomEncoderOptions.Trim() + " ");

            string strTemp;
            xs.SampleAR = oSettingsHandler.getSar(d, hres, vres, out strTemp, CustomSarValue);
            switch (xs.SampleAR)
            {
                case 0:
                    {
                        if (!String.IsNullOrEmpty(CustomSarValue))
                            sb.Append("--sar " + CustomSarValue + " ");
                        break;
                    }
                case 1: sb.Append("--sar 1:1 "); break;
                case 2: sb.Append("--sar 4:3 "); break;
                case 3: sb.Append("--sar 8:9 "); break;
                case 4: sb.Append("--sar 10:11 "); break;
                case 5: sb.Append("--sar 12:11 "); break;
                case 6: sb.Append("--sar 16:11 "); break;
                case 7: sb.Append("--sar 16:15 "); break;
                case 8: sb.Append("--sar 32:27 "); break;
                case 9: sb.Append("--sar 40:33 "); break;
                case 10: sb.Append("--sar 64:45 "); break;
            }

            // input/output section
            if (log != null)
            {
                if (!xs.CustomEncoderOptions.Contains("--tcfile-in "))
                    if (!String.IsNullOrEmpty(xs.TCFile) && File.Exists(xs.TCFile))
                        sb.Append("--tcfile-in \"" + xs.TCFile + "\" ");

                sb.Append("--frames " + numberOfFrames + " ");
                if (xs.VideoEncodingType == VideoCodecSettings.VideoEncodingMode.twopass1
                    || xs.VideoEncodingType == VideoCodecSettings.VideoEncodingMode.threepass1)
                    sb.Append("--output NUL ");
                else if (!String.IsNullOrEmpty(output))
                    sb.Append("--output " + "\"" + output + "\" ");
                if (!UseWrapper())
                    sb.Append("\"" + input + "\" ");
                else
                    sb.Append("--stdin y4m -\"");
            }
            #endregion

            return sb.ToString();
        }

        /// <summary>
        /// Checks if the timecode file does have the correct header
        /// </summary>
        private void CheckTCFile()
        {
            x264Settings xs = (x264Settings)Job.Settings;
            if (String.IsNullOrEmpty(xs.TCFile) || !File.Exists(xs.TCFile))
                return;

            if (File.Exists(xs.TCFile + ".x264"))
                FileUtil.DeleteFile(xs.TCFile + ".x264", log);

            Su.Status = "Checking TC File...";
            try
            {
                using (StreamReader sr = new StreamReader(xs.TCFile))
                {
                    bool bFound = false;
                    using (StreamWriter sw = new StreamWriter(xs.TCFile + ".x264"))
                    {
                        string line = sr.ReadLine();
                        if (line.Trim().StartsWith("# timestamp format v2"))
                        {
                            bFound = true;
                            sw.WriteLine("# timecode format v2");
                            sw.WriteLine(sr.ReadToEnd());
                            Job.FilesToDelete.Add(xs.TCFile + ".x264");
                            xs.TCFile = xs.TCFile + ".x264";
                        }
                    }
                    if (!bFound)
                        FileUtil.DeleteFile(xs.TCFile + ".x264", log);
                }
            }
            catch (Exception ex)
            {
                log.LogValue("Error parsing " + xs.TCFile, ex.Message, ImageType.Error, true);
            }
            Su.Status = "Encoding video...";
        }

        protected override string Commandline
        {
            get 
            {
                CheckTCFile();
                string strCommandLine = genCommandline(Job.Input, Job.Output, Job.DAR, Hres, Vres, Fps_n, Fps_d, ref NumberOfFrames, Job.Settings as x264Settings, Job.Zones, base.log);
                Su.NbFramesTotal = NumberOfFrames;
                return strCommandLine;
            }
        }
    }
}