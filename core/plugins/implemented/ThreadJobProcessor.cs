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
using System.Diagnostics;
using System.IO;
using System.Threading;

using MeGUI.core.util;

namespace MeGUI
{
    public abstract class ThreadJobProcessor<TJob> : IJobProcessor
        where TJob : Job
    {
        #region variables
        protected TJob job;
        protected StatusUpdate su;
        protected LogItem log;
        protected Thread processThread;
        protected ManualResetEvent mre = new System.Threading.ManualResetEvent(true); // lock used to pause processing
        protected bool continueWorking = true;
        protected string jobOutputFile = string.Empty;
        #endregion

        protected virtual void checkJobIO()
        {
            // only check if the input file exist if it is not a cleanup job
            if (!(this is MeGUI.core.details.CleanupJobRunner))
                Util.ensureExists(job.Input);
            jobOutputFile = job.Output;
        }

        protected virtual void doExitConfig()
        {
            if (su.HasError || su.WasAborted)
                return;

            if (!String.IsNullOrEmpty(job.Output) && File.Exists(job.Output))
            {
                MediaInfoFile oInfo = new MediaInfoFile(job.Output, ref log);
            }
        }

        protected virtual void RunInThread() { }

        protected void ThreadFinished()
        {
            doExitConfig();
            su.IsComplete = true;
            StatusUpdate(su);
        }

        #region IVideoEncoder overridden Members

        public void setup(Job job2, StatusUpdate su, LogItem log)
        {
            Debug.Assert(job2 is TJob, "Job is the wrong type");

            this.log = log;
            TJob job = (TJob)job2;
            this.job = job;

            this.su = su;
            checkJobIO();
        }

        public void start()
        {
            try
            {
                WorkerPriority.GetJobPriority(job, out WorkerPriorityType oPriority, out bool lowIOPriority);
                processThread = new Thread(new ThreadStart(RunInThread));
                processThread.Priority = OSInfo.GetThreadPriority(oPriority);
                processThread.Start();
                new System.Windows.Forms.MethodInvoker(this.RunStatusCycle).BeginInvoke(null, null);
            }
            catch (Exception e)
            {
                throw new JobRunException(e);
            }
        }

        public virtual void stop()
        {
            try
            {
                su.WasAborted = true;
                continueWorking = false;
                mre.Set(); // if it's paused, then unpause
                while (IsRunning())
                    MeGUI.core.util.Util.Wait(1000);
                return;
            }
            catch (Exception e)
            {
                throw new JobRunException(e);
            }
        }

        public virtual bool pause()
        {
            return mre.Reset();
        }

        public virtual bool resume()
        {
            return mre.Set();
        }

        public bool IsRunning()
        {
            return (this.processThread != null && processThread.IsAlive);
        }

        public virtual void changePriority(WorkerPriorityType priority)
        {
            if (!IsRunning())
                return;

            try
            {
                processThread.Priority = OSInfo.GetThreadPriority(priority);
            }
            catch (Exception e) // process could not be running anymore
            {
                throw new JobRunException(e);
            }
        }

        public bool IsJobStopped()
        {
            // check for stop or suspend of the thread
            mre.WaitOne();
            return !continueWorking;
        }

        #endregion
 
        #region status updates
        public event JobProcessingStatusUpdateCallback StatusUpdate;
        protected void RunStatusCycle()
        {
            while (IsRunning())
            {
                su.CurrentFileSize = FileSize.Of2(jobOutputFile);
                su.FillValues();
                if (StatusUpdate != null && IsRunning())
                    StatusUpdate(su);
                MeGUI.core.util.Util.Wait(1000);
            }
            ThreadFinished();
        }
        #endregion
    }
}