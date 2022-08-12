// ****************************************************************************
// 
// Copyright (C) 2005-2022 Doom9 & al
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
using System.IO;
using System.Globalization;
using System.Text;

using MeGUI.core.util;

namespace MeGUI
{
    class x265Encoder : CommandlineVideoEncoder
    {
        public static readonly JobProcessorFactory Factory = new JobProcessorFactory(new ProcessorFactory(init), "x265Encoder");

        private static IJobProcessor init(MainForm mf, Job j)
        {
            if (j is VideoJob && (j as VideoJob).Settings is x265Settings)
            {
                UpdateCacher.CheckPackage("ffmpeg");
                UpdateCacher.CheckPackage("x265");
                return new x265Encoder("cmd.exe");
            }
            return null;
        }

        public x265Encoder(string encoderPath) : base()
        {
            executable = encoderPath;
            iMinimumChildProcessCount = 1;
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

        public static string genCommandline(string input, string output, Dar? d, int hres, int vres, int fps_n, int fps_d, ref ulong numberOfFrames, x265Settings _xs, Zone[] zones, LogItem log)
        {
            int qp;
            StringBuilder sb = new StringBuilder();
            CultureInfo ci = new CultureInfo("en-us");
            x265Settings xs = (x265Settings)_xs.Clone();
            MeGUI.packages.video.x265.x265SettingsHandler oSettingsHandler = new packages.video.x265.x265SettingsHandler(xs, log);

            // log
            if (log != null)
            {
                if (!String.IsNullOrEmpty(xs.CustomEncoderOptions))
                    log.LogEvent("custom command line: " + xs.CustomEncoderOptions);

                sb.Append("/c \"\"" + MainForm.Instance.Settings.FFmpeg.Path + "\" -loglevel level+error -i \"" + input + "\" -strict -1 -f yuv4mpegpipe - | ");
                if (!MainForm.Instance.Settings.Usex64Tools)
                    sb.Append("\"" + Path.Combine(Path.GetDirectoryName(MainForm.Instance.Settings.X265.Path), @"x86\x265.exe") + "\" ");
                else
                    sb.Append("\"" + Path.Combine(Path.GetDirectoryName(MainForm.Instance.Settings.X265.Path), @"x64\x265.exe") + "\" ");
            }

            #region main tab
            ///<summary>
            /// x265 Main Tab Settings
            ///</summary>

            // x265 Presets
            if (!xs.CustomEncoderOptions.Contains("--preset "))
            {
                switch (xs.x265PresetLevel)
                {
                    case x265Settings.x265PresetLevelModes.ultrafast: sb.Append("--preset ultrafast "); break;
                    case x265Settings.x265PresetLevelModes.superfast: sb.Append("--preset superfast "); break;
                    case x265Settings.x265PresetLevelModes.veryfast: sb.Append("--preset veryfast "); break;
                    case x265Settings.x265PresetLevelModes.faster: sb.Append("--preset faster "); break;
                    case x265Settings.x265PresetLevelModes.fast: sb.Append("--preset fast "); break;
                    //case x265Settings.x265PresetLevelModes.medium: sb.Append("--preset medium "); break; // default value
                    case x265Settings.x265PresetLevelModes.slow: sb.Append("--preset slow "); break;
                    case x265Settings.x265PresetLevelModes.slower: sb.Append("--preset slower "); break;
                    case x265Settings.x265PresetLevelModes.veryslow: sb.Append("--preset veryslow "); break;
                    case x265Settings.x265PresetLevelModes.placebo: sb.Append("--preset placebo "); break;
                }
            }

            // x265 Tunings
            if (!xs.CustomEncoderOptions.Contains("--tune "))
            {
                switch (xs.x265PsyTuning)
                {
                    case x265Settings.x265PsyTuningModes.PSNR: sb.Append("--tune psnr "); break;
                    case x265Settings.x265PsyTuningModes.SSIM: sb.Append("--tune ssim "); break;
                    case x265Settings.x265PsyTuningModes.FastDecode: sb.Append("--tune fastdecode "); break;
                    case x265Settings.x265PsyTuningModes.ZeroLatency: sb.Append("--tune zerolatency "); break;
                    case x265Settings.x265PsyTuningModes.Grain: sb.Append("--tune grain "); break;
                    default: break;
                }
            }

            // Encoding Modes
            switch (xs.VideoEncodingType)
            {
                case VideoCodecSettings.VideoEncodingMode.CBR:
                    if (!xs.CustomEncoderOptions.Contains("--bitrate ")) 
                        sb.Append("--bitrate " + xs.BitrateQuantizer + " ");
                    break;
                case VideoCodecSettings.VideoEncodingMode.CQ:
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
                        if (xs.QuantizerCRF != 28)
                            sb.Append("--crf " + xs.QuantizerCRF.ToString(ci) + " ");
                    break;
            }

            // Threads
            if (!xs.CustomEncoderOptions.Contains("--frame-threads "))
                if (xs.NbThreads > 0)
                    sb.Append("--frame-threads " + xs.NbThreads + " ");
            #endregion

            string CustomSarValue;
            xs.SampleAR = oSettingsHandler.getSar(d, hres, vres, out CustomSarValue, String.Empty);

            // get number of frames to encode
            oSettingsHandler.getFrames(ref numberOfFrames);

            xs.CustomEncoderOptions = oSettingsHandler.getCustomCommandLine();
            if (!String.IsNullOrEmpty(xs.CustomEncoderOptions)) // add custom encoder options
                sb.Append(xs.CustomEncoderOptions + " ");

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

            if (log != null)
            {
                // input/output
                if (xs.VideoEncodingType == VideoCodecSettings.VideoEncodingMode.twopass1
                    || xs.VideoEncodingType == VideoCodecSettings.VideoEncodingMode.threepass1)
                    sb.Append("--output NUL ");
                else if (!String.IsNullOrEmpty(output))
                    sb.Append("--output " + "\"" + output + "\" ");
                sb.Append("--frames " + numberOfFrames + " --y4m -\"");
            }

            return sb.ToString();
        }

        protected override string Commandline
        {
            get 
            {
                string strCommandLine = genCommandline(job.Input, job.Output, job.DAR, hres, vres, fps_n, fps_d, ref numberOfFrames, job.Settings as x265Settings, job.Zones, base.log);
                su.NbFramesTotal = numberOfFrames;
                return strCommandLine;
            }
        }

        protected override void doExitConfig()
        {
            base.doExitConfig();
        }
    }
}