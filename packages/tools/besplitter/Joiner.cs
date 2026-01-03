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
using System.Collections.Generic;
using System.IO;
using System.Text;

using MeGUI.core.util;

namespace MeGUI.packages.tools.besplitter
{
    class Joiner : CommandlineJobProcessor<AudioJoinJob>
    {
        public static readonly JobProcessorFactory Factory =
new JobProcessorFactory(new ProcessorFactory(init), "BeSplit_Joiner");

        private static IJobProcessor init(MainForm mf, Job j)
        {
            if (j is AudioJoinJob) return new Joiner(mf.Settings.BeSplit.Path);
            return null;
        }

        string tmpfile;

        public Joiner(string exe)
        {
            UpdateCacher.CheckPackage("besplit");
            Executable = exe;
        }

        protected override bool checkExitCode
        {
            get { return false; }
        }

        protected override string Commandline
        {
            get
            {
                return Job.generateJoinCommandline(tmpfile);
            }
        }

        protected override void checkJobIO()
        {
            base.checkJobIO();
            FileSize totalSize = FileSize.Empty;
            try
            {
                // now create the temporary list
                tmpfile = Path.Combine(Path.GetDirectoryName(Job.Output), Path.GetRandomFileName());
                using (StreamWriter w = new StreamWriter(File.OpenWrite(tmpfile), Encoding.Default))
                {
                    foreach (string file in Job.InputFiles)
                    {
                        Util.ensureExists(file);
                        totalSize += FileSize.Of(file);
                        w.WriteLine(file);
                    }
                }
            }
            catch (Exception e)
            {
                throw new JobRunException("Error generating temporary *.lst file: " + e.Message, e);
            }
            Su.FileSizeTotal = totalSize;
            Su.ClipLength = Job.ClipLength;
        }

        public override void ProcessLine(string line, StreamType stream, ImageType oType)
        {
            if (line.IndexOf("writing to file") != -1)
                return;

            if (line.IndexOf("Usage") != -1)
                oType = ImageType.Error;
            base.ProcessLine(line, stream, oType);
        }
    }
}
