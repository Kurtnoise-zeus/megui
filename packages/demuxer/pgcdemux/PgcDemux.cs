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
using System.Text;

using MeGUI.core.util;

namespace MeGUI
{
    class PgcDemux : CommandlineJobProcessor<PgcDemuxJob>
    {
        public static readonly JobProcessorFactory Factory = new JobProcessorFactory(new ProcessorFactory(init), "PgcDemux");

        private static IJobProcessor init(MainForm mf, Job j)
        {
            if (j is PgcDemuxJob) 
                return new PgcDemux(mf.Settings.PgcDemux.Path);
            return null;
        }

        public PgcDemux(string executablePath)
        {
            UpdateCacher.CheckPackage("pgcdemux");
            this.Executable = executablePath;
        }

        protected override void checkJobIO()
        {
            try
            {
                if (!String.IsNullOrEmpty(Job.TemporaryPath))
                    FileUtil.ensureDirectoryExists(Job.TemporaryPath);
                Su.Status = "Preparing VOB...";
            }
            finally
            {
                base.checkJobIO();
            }
        }

        protected override string Commandline
        {
            get
            {
                StringBuilder sb = new StringBuilder();

                // Input File
                sb.Append("-pgc " + Job.PGCNumber);
                                if (Job.AngleNumber > 0)
                    sb.Append(" -ang " + Job.AngleNumber);
                sb.Append(" -noaud -nosub -customvob n,v,a,s,l \"" + Job.Input + "\" \"" + Job.TemporaryPath + "\"");
                return sb.ToString();
            }
        }

        protected override void doExitConfig()
        {
            for (int i = 1; i < 10; i++)
            {
                // check first if the output file already exists and delete it
                string output = Job.OutputFileName.Substring(0, Job.OutputFileName.Length - 5) + i + ".VOB";
                if (File.Exists(output))
                {
                    File.Delete(output);
                    log.LogEvent("Old VOB file deleted: " + output);
                }

                // move the new output file to the desired location if found
                string input = Path.Combine(Job.TemporaryPath, "VTS_01_" + i + ".VOB");
                if (!File.Exists(input))
                    continue;

                File.Move(input, output);
                log.LogEvent("VOB file created: " + output);
            }

            base.doExitConfig();
        }
    }
}
