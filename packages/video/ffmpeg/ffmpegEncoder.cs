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

using MeGUI.core.util;

namespace MeGUI
{
    class ffmpegEncoder : CommandlineVideoEncoder
    {
        public static readonly JobProcessorFactory Factory =
new JobProcessorFactory(new ProcessorFactory(init), "FFmpegEncoder");

        private static IJobProcessor init(MainForm mf, Job j)
        {
            if (j is VideoJob && ((j as VideoJob).Settings is hfyuSettings))
                return new ffmpegEncoder(mf.Settings.FFmpeg.Path);
            return null;
        }

        public ffmpegEncoder(string encoderPath)
            : base()
        {
            UpdateCacher.CheckPackage("ffmpeg");
            executable = encoderPath;
        }

        #region commandline generation
        protected override string Commandline
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("-y -i \"" + job.Input + "\" -c:v ffvhuff -threads 0 -sn -an -context 1 -vstrict -2 -pred 2 \"" + job.Output + "\" ");
                return sb.ToString();
            }
        }
        #endregion

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
               !line.ToLowerInvariant().StartsWith("output #0, avi, to '") &&
               !line.ToLowerInvariant().EndsWith("input/output error"))
                oType = ImageType.Error;
            else if (line.ToLowerInvariant().Contains("warning") &&
                !line.ToLowerInvariant().StartsWith("input #0, avisynth, from '") &&
                !line.ToLowerInvariant().StartsWith("output #0, avi, to '"))
                oType = ImageType.Warning;
            base.ProcessLine(line, stream, oType);
        }
    }
}
