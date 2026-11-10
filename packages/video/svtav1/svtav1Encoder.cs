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

using MeGUI.core.util;
using MeGUI.packages.video.svtav1;
using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace MeGUI
{
    class svtav1Encoder : CommandlineVideoEncoder
    {
        public static readonly JobProcessorFactory Factory = new JobProcessorFactory(new ProcessorFactory(init), "svtav1Encoder");

        private static IJobProcessor init(MainForm mf, Job j)
        {
            if (j is VideoJob && (j as VideoJob).Settings is svtav1Settings)
            {
                UpdateCacher.CheckPackage("ffmpeg");
                UpdateCacher.CheckPackage("svtav1");
                return new svtav1Encoder("cmd.exe");
            }
            return null;
        }

        public svtav1Encoder(string encoderPath) : base()
        {
            Executable = encoderPath;
            IMinimumChildProcessCount = 1;
        }

        // the progress is written to the log every LogStepPercent percent instead of for every frame
        private const int LogStepPercent = 10;
        private int _lastLoggedStep = -1;

        protected override void configureStartInfo(ProcessStartInfo pstart)
        {
            // SVT-AV1 only colors its output when NO_COLOR is not set (https://no-color.org/)
            if (!pstart.UseShellExecute)
                pstart.EnvironmentVariables["NO_COLOR"] = "1";
        }

        public override void ProcessLine(string line, StreamType stream, ImageType oType)
        {
            // builds which ignore NO_COLOR color their output with ANSI escape sequences: remove them before anything else
            line = svtav1Progress.StripAnsi(line);

            if (svtav1Progress.TryParse(line, out svtav1Progress progress)) // status update
            {
                // the total is not printed when SVT-AV1 does not know the number of frames
                if (progress.FrameTotal == 0 && Su.NbFramesTotal.HasValue)
                    progress.FrameTotal = Su.NbFramesTotal.Value;

                // Initialize NbFramesTotal
                if ((!Su.NbFramesTotal.HasValue || Su.NbFramesTotal == 0) && progress.FrameTotal > 0)
                    Su.NbFramesTotal = progress.FrameTotal;

                if (base.setFrameNumber(progress.Frame.ToString(CultureInfo.InvariantCulture)))
                {
                    int step = progress.FrameTotal > 0 ? (int)(progress.Frame * 100 / progress.FrameTotal) / LogStepPercent : 0;
                    if (step > _lastLoggedStep)
                    {
                        _lastLoggedStep = step;
                        // the remaining time of SVT-AV1 includes its start-up time, it is far too high for the first frames
                        base.ProcessLine(progress.ToSummary(step > 0), stream, ImageType.Information);
                    }
                    return;
                }
            }
            else if (line.StartsWith("frame=")) // status update for ffmpeg
            {
                int frameNumberEnd = line.IndexOf("f", 6);
                if (base.setFrameNumber(line.Substring(6, frameNumberEnd - 6).Trim()))
                    return;
            }

            if (line.ToLowerInvariant().Contains("error") &&
               !line.ToLowerInvariant().StartsWith("input #0, avisynth, from '") &&
               !line.ToLowerInvariant().EndsWith("input/output error"))
                oType = ImageType.Error;
            else if (line.ToLowerInvariant().Contains("warning") &&
                !line.ToLowerInvariant().StartsWith("input #0, avisynth, from '"))
                oType = ImageType.Warning;
            base.ProcessLine(line, stream, oType);
        }

        public static string genCommandline(string input, string output, Dar? d, int hres, int vres, ref ulong numberOfFrames, svtav1Settings _xs, LogItem log)
        {
            int qp;
            StringBuilder sb = new StringBuilder();
            CultureInfo ci = new CultureInfo("en-us");
            svtav1Settings xs = (svtav1Settings)_xs.Clone();
            MeGUI.packages.video.svtav1.svtav1SettingsHandler oSettingsHandler = new packages.video.svtav1.svtav1SettingsHandler(xs, log);

            // log
            if (log != null)
            {
                if (!String.IsNullOrEmpty(xs.CustomEncoderOptions))
                    log.LogEvent("custom command line: " + xs.CustomEncoderOptions);

                sb.Append("/c \"\"" + MainForm.Instance.Settings.FFmpeg.Path + "\" -loglevel level+error -hide_banner  -i \"" + input + "\" ");
                
                // bit-depth
                if (xs.SVT10Bits)
                    sb.Append("-pix_fmt yuv420p10le ");
                
                sb.Append("-strict -1 -f yuv4mpegpipe - | " + "\"" + MainForm.Instance.Settings.SvtAv1.Path + "\" ");
            }

            #region main tab
            ///<summary>
            /// SVT-AV1-PSY Main Tab Settings
            ///</summary>
            ///

            // Progress & Input
            sb.Append("--progress 2 -i - ");

            // get number of frames to encode
            oSettingsHandler.getFrames(ref numberOfFrames);
            if (numberOfFrames != null)
                sb.Append("--frames " + numberOfFrames.ToString() + " ");

            // Tunings
            if (!xs.CustomEncoderOptions.Contains("--tune "))
            {
                if (xs.VideoEncodingType == VideoCodecSettings.VideoEncodingMode.twopass1 ||
                    xs.VideoEncodingType == VideoCodecSettings.VideoEncodingMode.twopass2 ||
                    xs.VideoEncodingType == VideoCodecSettings.VideoEncodingMode.twopassAutomated ||
                    xs.VideoEncodingType == VideoCodecSettings.VideoEncodingMode.threepass1 ||
                    xs.VideoEncodingType == VideoCodecSettings.VideoEncodingMode.threepass2 ||
                    xs.VideoEncodingType == VideoCodecSettings.VideoEncodingMode.threepassAutomated)
                    xs.svtav1Tuning = svtav1Settings.svtav1TuningModes.PSNR; // multi pass encodings do not support default tunings - force it to PSNR

                switch (xs.svtav1Tuning)
                {
                    case svtav1Settings.svtav1TuningModes.VQ: sb.Append("--tune 0 "); break;
                    case svtav1Settings.svtav1TuningModes.PSNR: sb.Append("--tune 1 "); break;
                    case svtav1Settings.svtav1TuningModes.SSIM: sb.Append("--tune 2 "); break;
                    case svtav1Settings.svtav1TuningModes.SUBJECTIVESSIM: sb.Append("--tune 3 "); break;
                    case svtav1Settings.svtav1TuningModes.STILLPICTURE: sb.Append("--tune 4 "); break;
                    default: break;
                }
            }

            // Presets
            if (!xs.CustomEncoderOptions.Contains("--preset "))
            {
                switch (xs.Preset)
                {
                    case 0: sb.Append("--preset 0 "); break;
                    case 1: sb.Append("--preset 1 "); break;
                    case 2: sb.Append("--preset 2 "); break;
                    case 3: sb.Append("--preset 3 "); break;
                    case 4: sb.Append("--preset 4 "); break;
                    case 5: sb.Append("--preset 5 "); break;
                    case 6: sb.Append("--preset 6 "); break;
                    case 7: sb.Append("--preset 7 "); break;
                    case 8: sb.Append("--preset 8 "); break;
                    case 9: sb.Append("--preset 9 "); break;
                    case 10: sb.Append("--preset 10 "); break; // default value
                    case 11: sb.Append("--preset 11 "); break;
                    case 12: sb.Append("--preset 12 "); break;
                    case 13: sb.Append("--preset 13 "); break;
                    default: break;
                }
            }

            // Encoding Modes
            switch (xs.VideoEncodingType)
            {
                case VideoCodecSettings.VideoEncodingMode.CBR:
                    if (!xs.CustomEncoderOptions.Contains("--tbr "))
                        sb.Append("--rc 2 --tbr " + xs.BitrateQuantizer + " ");
                    break;
                case VideoCodecSettings.VideoEncodingMode.CQ: // CQ
                    if (!xs.CustomEncoderOptions.Contains("--qp "))
                    {
                        qp = (int)xs.QuantizerCRF;
                        sb.Append("--rc 0 --qp " + qp.ToString(ci) + " ");
                    }
                    break;
                case VideoCodecSettings.VideoEncodingMode.twopass1: // 2 pass first pass
                    sb.Append("--rc 1 --pass 1 --tbr " + xs.BitrateQuantizer + " --stats " + "\"" + xs.Logfile + "\" ");
                    break;
                case VideoCodecSettings.VideoEncodingMode.twopass2: // 2 pass second pass
                case VideoCodecSettings.VideoEncodingMode.twopassAutomated: // automated twopass
                    sb.Append("--rc 1 --pass 2 --tbr " + xs.BitrateQuantizer + " --stats " + "\"" + xs.Logfile + "\" ");
                    break;
                case VideoCodecSettings.VideoEncodingMode.threepass1: // 3 pass first pass
                    sb.Append("--rc 1 --pass 1 --tbr " + xs.BitrateQuantizer + " --stats " + "\"" + xs.Logfile + "\" ");
                    break;
                case VideoCodecSettings.VideoEncodingMode.threepass2: // 3 pass 2nd pass
                    sb.Append("--rc 1 --pass 3 --tbr " + xs.BitrateQuantizer + " --stats " + "\"" + xs.Logfile + "\" ");
                    break;
                case VideoCodecSettings.VideoEncodingMode.threepass3: // 3 pass 3rd pass
                case VideoCodecSettings.VideoEncodingMode.threepassAutomated: // automated threepass, show third pass options
                    sb.Append("--rc 1 --pass 3 --tbr " + xs.BitrateQuantizer + " --stats " + "\"" + xs.Logfile + "\" ");
                    break;
                case VideoCodecSettings.VideoEncodingMode.quality: // constant quality
                    if (!xs.CustomEncoderOptions.Contains("--crf "))
                        if (xs.QuantizerCRF != 35)
                            sb.Append("--rc 0 --crf " + xs.QuantizerCRF.ToString(ci) + " ");
                    break;
            }

            #endregion
            xs.CustomEncoderOptions = oSettingsHandler.getCustomCommandLine();
            if (!String.IsNullOrEmpty(xs.CustomEncoderOptions)) // add custom encoder options
                sb.Append(xs.CustomEncoderOptions + " ");

            if (log != null)
            {
                // input/output
                if (xs.VideoEncodingType == VideoCodecSettings.VideoEncodingMode.twopass1
                    || xs.VideoEncodingType == VideoCodecSettings.VideoEncodingMode.threepass1)
                    sb.Append("");
                else if (!String.IsNullOrEmpty(output))
                    sb.Append("-b " + "\"" + output + "\""); 
            }

            return sb.ToString();
        }

        protected override string Commandline
        {
            get 
            {
                string strCommandLine = genCommandline(Job.Input, Job.Output, Job.DAR, Hres, Vres, ref NumberOfFrames, Job.Settings as svtav1Settings, base.log);
                Su.NbFramesTotal = NumberOfFrames;
                return strCommandLine;
            }
        }

        protected override void doExitConfig()
        {
            base.doExitConfig();
        }
    }
}