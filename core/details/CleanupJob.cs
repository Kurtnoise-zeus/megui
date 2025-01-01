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
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;

using MeGUI.core.util;

namespace MeGUI.core.details
{
    public class CleanupJob : Job
    {
        private CleanupJob() { }

        public static JobChain AddAfter(JobChain other, List<string> files, string strInput)
        {
            CleanupJob j = new CleanupJob();
            j.FilesToDelete.AddRange(files);
            j.Input = strInput;
            return new SequentialChain(other, j);
        }

        public override string CodecString
        {
            get { return "cleanup"; }
        }

        public override string EncodingMode
        {
            get { return "cleanup"; }
        }
    }

    public class CleanupJobRunner : ThreadJobProcessor<CleanupJob>
    {
        public static readonly JobProcessorFactory Factory =
                    new JobProcessorFactory(new ProcessorFactory(init), "cleanup");

        public static readonly JobPostProcessor DeleteIntermediateFilesPostProcessor = new JobPostProcessor(
            delegate (MainForm mf, Job j)
            {
                if (mf.Settings.DeleteIntermediateFiles)
                    return FileUtil.DeleteIntermediateFiles(j.FilesToDelete, true, false);
                return null;
            }
            , "DeleteIntermediateFiles");

        private static IJobProcessor init(MainForm mf, Job j)
        {
            if (j is CleanupJob)
                return new CleanupJobRunner();
            return null;
        }

        public CleanupJobRunner() { }

        protected override void RunInThread()
        {
            Su.Status = "Cleanup files...";

            log.LogValue("Delete Intermediate Files option set", MainForm.Instance.Settings.DeleteIntermediateFiles);
            if (MainForm.Instance.Settings.DeleteIntermediateFiles)
            {
                log.Add(FileUtil.DeleteIntermediateFiles(Job.FilesToDelete, true, false));
                Job.FilesToDelete.Clear();
            }
        }
    }
}
