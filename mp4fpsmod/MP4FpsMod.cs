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
using System.IO;
using System.Text;

using MeGUI.core.util;

namespace MeGUI
{
    public class MP4FpsMod : CommandlineJobProcessor<MP4FpsModJob>
    {
        public static readonly JobProcessorFactory Factory = new JobProcessorFactory(new ProcessorFactory(init), "MP4FpsMod");

        private static IJobProcessor init(MainForm mf, Job j)
        {
            if (j is MP4FpsModJob)
                return new MP4FpsMod();
            return null;
        }

        public MP4FpsMod()
        {
            UpdateCacher.CheckPackage("mp4box");
            executable = Path.Combine(Path.GetDirectoryName(MainForm.Instance.Settings.Mp4Box.Path), @"mp4fpsmod\mp4fpsmod.exe");
        }

        #region IJobProcessor Members
        protected override string Commandline
        {
            get
            {
                su.Status = "Applying timecodes...";
                return "-i -t \"" + job.TimeStampFile + "\" -x \"" + job.Input + "\"";
            }
        }
        #endregion
    }
}