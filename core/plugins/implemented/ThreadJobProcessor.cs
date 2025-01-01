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
        private TJob job;
        private StatusUpdate su;
        private Thread processThread;
        private ManualResetEvent mre = new System.Threading.ManualResetEvent(true); // lock used to pause processing
        private bool continueWorking = true;

#pragma warning disable CA1051 // Do not declare visible instance fields
        protected LogItem log;
#pragma warning restore CA1051 // Do not declare visible instance fields
        
        protected TJob Job { get => job; set => job = value; }
        protected StatusUpdate Su { get => su; set => su = value; }
        protected ManualResetEvent Mre { get => mre; set => mre = value; }
        #endregion

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // dispose managed resources
                mre.Dispose();
            }
            // free native resources
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void checkJobIO()
        {
            // only check if the input file exist if it is not a cleanup job
            if (!(this is MeGUI.core.details.CleanupJobRunner))
                Util.ensureExists(Job.Input);
        }

        protected virtual void doExitConfig()
        {
            if (Su.HasError || Su.WasAborted)
                return;

            if (!String.IsNullOrEmpty(Job.Output) && File.Exists(Job.Output))
            {
                using (MediaInfoFile oInfo = new MediaInfoFile(Job.Output, ref log))
                {
                }
            }
        }

        protected virtual void RunInThread() { }

        protected void ThreadFinished()
        {
            doExitConfig();
            Su.IsComplete = true;
            if (StatusUpdate != null)
                StatusUpdate(Su);
        }

        #region IVideoEncoder overridden Members

        public void setup(Job job2, StatusUpdate su, LogItem log)
        {
            Debug.Assert(job2 is TJob, "Job is the wrong type");

            this.log = log;
            TJob job = (TJob)job2;
            this.Job = job;

            this.Su = su;
            checkJobIO();
        }

        public void start()
        {
            try
            {
                WorkerPriority.GetJobPriority(Job, out WorkerPriorityType oPriority, out bool lowIOPriority);
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
                Su.WasAborted = true;
                continueWorking = false;
                Mre.Set(); // if it's paused, then unpause
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
            return Mre.Reset();
        }

        public virtual bool resume()
        {
            return Mre.Set();
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
            Mre.WaitOne();
            return !continueWorking;
        }

        #endregion
 
        #region status updates
        public event JobProcessingStatusUpdateCallback StatusUpdate;
        protected void RunStatusCycle()
        {
            while (IsRunning())
                MeGUI.core.util.Util.Wait(1000);
            ThreadFinished();
        }
        #endregion
    }
}