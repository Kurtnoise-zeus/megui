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
using System.IO;
using System.Globalization;
using System.Text;

using MeGUI.core.util;

namespace MeGUI
{
    class ffv1Encoder : CommandlineVideoEncoder
    {
        public static readonly JobProcessorFactory Factory = new JobProcessorFactory(new ProcessorFactory(init), "ffv1Encoder");

        private static IJobProcessor init(MainForm mf, Job j)
        {
            if (j is VideoJob && (j as VideoJob).Settings is ffv1Settings)
            {
                UpdateCacher.CheckPackage("ffmpeg");
                return new ffv1Encoder(mf.Settings.FFmpeg.Path);
            }
            return null;
        }

        public ffv1Encoder(string encoderPath) : base()
        {
            Executable = encoderPath;
            IMinimumChildProcessCount = 1;
        }

        public override void ProcessLine(string line, StreamType stream, ImageType oType)
        {
            if (line.StartsWith("Pos:")) // status update
            {
                int frameNumberStart = line.IndexOf("s", 4) + 1;
                int frameNumberEnd = line.IndexOf("f");
                if (base.setFrameNumber(line.Substring(frameNumberStart, frameNumberEnd - frameNumberStart).Trim()))
                    return;
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

        public static string genCommandline(string input, string output, Dar? d, int hres, int vres, ref ulong numberOfFrames, ffv1Settings _xs, LogItem log)
        {
            StringBuilder sb = new StringBuilder();
            CultureInfo ci = new CultureInfo("en-us");
            ffv1Settings xs = (ffv1Settings)_xs.Clone();
            MeGUI.packages.video.ffv1.ffv1SettingsHandler oSettingsHandler = new packages.video.ffv1.ffv1SettingsHandler(xs, log);

            // log
            if (log != null)
            {
                if (!String.IsNullOrEmpty(xs.CustomEncoderOptions))
                    log.LogEvent("custom command line: " + xs.CustomEncoderOptions);
            }

            #region main tab
            ///<summary>
            /// FFV1 Main Tab Settings
            ///</summary>
            ///
            // Input
            sb.Append("-hide_banner -i "  + "\"" + input + "\" ");

            // Only FFV1.3 supported
            sb.Append("-vcodec ffv1 -level 3 "); 

            // Contex
            if (xs.Context == 1)
                sb.Append("-context 1 ");
            else
                sb.Append("-context 0 ");

            // Coder
            if (xs.Coder > -1)
                sb.Append("-coder " + xs.Coder.ToString()  + " ");

            // Error Correction
            if (xs.ErrorCorrection)
                sb.Append("-slicecrc 1 ");

            // Slices
            switch (xs.Slices)
            {
                case 0: sb.Append("-slices 4 "); break;
                case 1: sb.Append("-slices 6 "); break;
                case 2: sb.Append("-slices 9 "); break;
                case 3: sb.Append("-slices 12 "); break;
                case 4: sb.Append("-slices 16 "); break;
                case 5: sb.Append("-slices 24 "); break;
                case 6: sb.Append("-slices 30 "); break;
                default: break;
            }

            // GOP Size
            if (xs.GOPSize >=1)
                sb.Append("-g " + xs.GOPSize.ToString() + " ");

            // Encoding Modes
            switch (xs.FFV1EncodingType)
            {
                case VideoCodecSettings.FFV1EncodingMode.none: // No MultiPass
                    break;
                case VideoCodecSettings.FFV1EncodingMode.twopass1: // 2 pass first pass
                    sb.Append("-pass 1 -passlogfile " + "\"" + Path.ChangeExtension(xs.Logfile, "") + "\" ");
                    break;
                case VideoCodecSettings.FFV1EncodingMode.twopass2: // 2 pass second pass
                case VideoCodecSettings.FFV1EncodingMode.twopassAutomated: // automated twopass
                    sb.Append("-pass 2 -passlogfile " + "\"" + Path.ChangeExtension(xs.Logfile, "") + "\" ");
                    break;
                default: break;
            }

            // Threads
            if (!xs.CustomEncoderOptions.Contains("-threads "))
                if (xs.NbThreads > 0)
                    sb.Append("-threads " + xs.NbThreads + " ");

            // bit-depth
            if (xs.FFV110Bits)
                sb.Append("-pix_fmt yuv420p10le ");
            #endregion

            // get number of frames to encode
            oSettingsHandler.getFrames(ref numberOfFrames);

            xs.CustomEncoderOptions = oSettingsHandler.getCustomCommandLine();
            if (!String.IsNullOrEmpty(xs.CustomEncoderOptions)) // add custom encoder options
                sb.Append(xs.CustomEncoderOptions + " ");

            if (log != null)
            {
                // input/output
                if (xs.FFV1EncodingType == VideoCodecSettings.FFV1EncodingMode.twopass1)
                    sb.Append("-f null NUL ");
                else if (!String.IsNullOrEmpty(output))
                    sb.Append("-f matroska " + "\"" + output + "\" "); // Force MKV output
            }

            return sb.ToString();
        }

        protected override string Commandline
        {
            get 
            {
                string strCommandLine = genCommandline(Job.Input, Job.Output, Job.DAR, Hres, Vres, ref NumberOfFrames, Job.Settings as ffv1Settings, base.log);
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