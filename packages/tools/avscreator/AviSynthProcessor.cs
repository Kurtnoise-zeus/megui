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
using System.Diagnostics;
using System.Text;
using System.Threading;

using MeGUI.core.util;

namespace MeGUI
{
 	public delegate void AviSynthStatusUpdateCallback(StatusUpdate su);

    public class AviSynthProcessor : IJobProcessor
    {
        public static readonly JobProcessorFactory Factory =
            new JobProcessorFactory(new ProcessorFactory(init), "AviSynthProcessor");

        private static IJobProcessor init(MainForm mf, Job j)
        {
            if (j is AviSynthJob) return new AviSynthProcessor();
            return null;
        }

        #region variables
        protected System.Threading.ManualResetEvent mre = new System.Threading.ManualResetEvent(true); // lock used to pause encoding
        private AvsFile file;
        private IVideoReader reader;
        private bool aborted;
        private ulong position;
        private Thread processorThread, statusThread;
        public StatusUpdate su = null;
        private AviSynthJob job;
        #endregion
        #region start / stop
        public AviSynthProcessor()
        {
        }
        #endregion
        #region processing

        private void update()
        {
            while (!aborted && position < su.NbFramesTotal)
            {
                su.NbFramesDone = position;
                su.FillValues();
                StatusUpdate(su);
                MeGUI.core.util.Util.Wait(1000);
            }
            su.IsComplete = true;
            StatusUpdate(su);
        }

        private void process()
        {
            IntPtr zero = new IntPtr(0);
            for (position = 0; position < su.NbFramesTotal && !aborted; position++)
            {
                file.Clip.ReadFrame(zero, 0, (int)position);
                mre.WaitOne();
            }
            if (file != null)
                file.Dispose();
        }

        /// <summary>
        /// sets up encoding
        /// </summary
        /// <param name="job">the job to be processed</param>
        /// <param name="error">output for any errors that might ocurr during this method</param>
        /// <returns>true if the setup has succeeded, false if it has not</returns>
        public void setup(Job job, StatusUpdate su, LogItem _)
        {
            Debug.Assert(job is AviSynthJob, "Job isn't an AviSynthJob");
            this.su = su;
            this.job = (AviSynthJob)job;

            try 
            {
                file = AvsFile.OpenScriptFile(job.Input, true);
                reader = file.GetVideoReader();
            }
            catch (Exception ex)
            {
                throw new JobRunException(ex);
            }
            this.su.NbFramesTotal = (ulong)reader.FrameCount;
            this.su.ClipLength = TimeSpan.FromSeconds((double)this.su.NbFramesTotal / file.VideoInfo.FPS);
            this.su.Status = "Playing through file...";
            position = 0;
            try
            {
                processorThread = new Thread(new ThreadStart(process));
            }
            catch (Exception e)
            {
                throw new JobRunException(e);
            }
            try
            {
                statusThread = new Thread(new ThreadStart(update));
            }
            catch (Exception e)
            {
                throw new JobRunException(e);
            }
        }

        /// <summary>
        /// starts the encoding process
        /// </summary>
        /// <param name="error">output for any errors that might ocurr during this method</param>
        /// <returns>true if encoding has been successfully started, false if not</returns>
        public void start()
        {
            try
            {
                statusThread.Start();
                processorThread.Start();
                su.ResetTime();
            }
            catch (Exception e)
            {
                throw new JobRunException(e);
            }
        }

        /// <summary>
        /// stops the encoding process
        /// </summary>
        /// <param name="error">output for any errors that might ocurr during this method</param>
        /// <returns>true if encoding has been successfully stopped, false if not</returns>
        public void stop()
        {
            aborted = true;
        }

        /// <summary>
        /// pauses the encoding process
        /// </summary>
        /// <param name="error">output for any errors that might ocurr during this method</param>
        /// <returns>true if encoding has been successfully paused, false if not</returns>
        public bool pause()
        {
            return mre.Reset();
        }

        /// <summary>
        /// resumes the encoding process
        /// </summary>
        /// <param name="error">output for any errors that might ocurr during this method</param>
        /// <returns>true if encoding has been successfully started, false if not</returns>
        public bool resume()
        {
            return mre.Set();
        }

        /// <summary>
        /// changes the priority of the encoding process/thread
        /// </summary>
        /// <param name="priority">the priority to change to</param>
        /// <param name="error">output for any errors that might ocurr during this method</param>
        /// <returns>true if the priority has been changed, false if not</returns>
        public void changePriority(WorkerPriorityType priority)
        {
            if (this.processorThread == null || !processorThread.IsAlive)
                return;

            try
            {
                processorThread.Priority = OSInfo.GetThreadPriority(priority);
            }
            catch (Exception e) // process could not be running anymore
            {
                throw new JobRunException(e);
            }
        }

        public event JobProcessingStatusUpdateCallback StatusUpdate;
        #endregion
    }
}
