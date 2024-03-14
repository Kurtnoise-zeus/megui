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
using System.Text;

using MeGUI.core.details;
using MeGUI.core.util;

namespace MeGUI
{
    class FFmpegMuxer : CommandlineMuxer
    {
        public static readonly JobProcessorFactory Factory = new JobProcessorFactory(new ProcessorFactory(init), "FFmpegMuxer");
        
        private static IJobProcessor init(MainForm mf, Job j)
        {
            if (j is MuxJob && (j as MuxJob).MuxType == MuxerType.FFMPEG)
                return new FFmpegMuxer(mf.Settings.FFmpeg.Path);
            return null;
        }
        
        public FFmpegMuxer(string executablePath)
        {
            UpdateCacher.CheckPackage("ffmpeg");
            this.executable = executablePath;
        }

        #region line processing
        public override void ProcessLine(string line, StreamType stream, ImageType oType)
        {
            if (line.StartsWith("Pos:") || line.StartsWith("frame=")) // status update
                return;

            if (line.ToLowerInvariant().Contains("error") &&
               !line.ToLowerInvariant().StartsWith("input #0, m4v, from '") &&
               !line.ToLowerInvariant().StartsWith("output #0, avi, to '") &&
               !line.ToLowerInvariant().StartsWith("output #0, matroska, to '") &&
               !line.ToLowerInvariant().EndsWith("input/output error"))
                oType = ImageType.Error;
            else if (line.ToLowerInvariant().Contains("warning") &&
                !line.ToLowerInvariant().StartsWith("input #0, m4v, from '") &&
                !line.ToLowerInvariant().StartsWith("output #0, avi, to '") &&
                !line.ToLowerInvariant().StartsWith("output #0, matroska, to '"))
                oType = ImageType.Warning;
            base.ProcessLine(line, stream, oType);
        }
        #endregion

        protected override void checkJobIO()
        {
            su.Status = "Muxing ...";
            base.checkJobIO();
        }

        protected override string Commandline
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                MuxSettings settings = job.Settings;
                
                string inputFile = settings.VideoInput;
                if (string.IsNullOrEmpty(settings.VideoInput))
                    inputFile = settings.MuxedInput;

                MediaInfoFile oVideoInfo = new MediaInfoFile(inputFile, ref log);

                // get source FPS
                System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo("en-us");
                string fpsString = oVideoInfo.VideoInfo.FPS.ToString(ci);
                if (settings.Framerate.HasValue)
                    fpsString = settings.Framerate.Value.ToString(ci);

                string aspect = null;
                if (settings.DAR.HasValue)
                    aspect = " -aspect " + settings.DAR.Value.X + ":" + settings.DAR.Value.Y;

                sb.Append("-y -i \"" + inputFile + "\" -vcodec copy -vtag XVID -r " + fpsString + aspect + " \"" + job.Output + "\" ");

                return sb.ToString();
            }
        }
    }
}