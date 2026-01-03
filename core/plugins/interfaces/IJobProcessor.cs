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
using System.Text;

using MeGUI.core.util;

namespace MeGUI
{
    public delegate void JobProcessingStatusUpdateCallback(StatusUpdate su);
    public delegate LogItem Processor(MainForm info, Job job);
    
    /// <summary>
    /// Returns an IJobProcessor if this job can be processed, returns null otherwise.
    /// </summary>
    /// <param name="info"></param>
    /// <param name="job"></param>
    /// <returns></returns>
    public delegate IJobProcessor ProcessorFactory(MainForm info, Job job);
    public sealed class JobPreProcessor : IIDable
    {
        private string id;
        public string ID
        {
            get { return id; }
        }
        private Processor processor;
        public Processor PreProcessor
        {
            get { return processor; }
        }
        public JobPreProcessor(Processor p, string id)
        {
            this.id = id;
            this.processor = p;
        }
    }
    public sealed class JobPostProcessor : IIDable
    {
        private string id;
        public string ID
        {
            get { return id; }
        }
        private Processor processor;
        public Processor PostProcessor
        {
            get { return processor; }
        }
        public JobPostProcessor(Processor p, string id)
        {
            this.id = id;
            this.processor = p;
        }
    }
    public sealed class JobProcessorFactory : IIDable
    {
        private string id;
        public string ID
        {
            get { return id; }
        }
        private ProcessorFactory factory;
        public ProcessorFactory Factory
        {
            get { return factory; }
        }
        public JobProcessorFactory(ProcessorFactory factory, string id)
        {
            this.factory = factory;
            this.id = id;
        }
    }
	
    public interface IJobProcessor : IDisposable
    {
        /// <summary>
        /// sets up encoding
        /// </summary
        /// <param name="job">the job to be processed</param>
        /// <param name="error">output for any errors that might ocurr during this method</param>
        /// <returns>true if the setup has succeeded, false if it has not</returns>
        void setup(Job job, StatusUpdate su, LogItem log);
        /// <summary>
        /// starts the encoding process
        /// </summary>
        /// <param name="error">output for any errors that might ocurr during this method</param>
        /// <returns>true if encoding has been successfully started, false if not</returns>
        void start();
        /// <summary>
        /// stops the encoding process
        /// </summary>
        /// <param name="error">output for any errors that might ocurr during this method</param>
        /// <returns>true if encoding has been successfully stopped, false if not</returns>
        void stop();
        /// <summary>
        /// pauses the encoding process
        /// </summary>
        /// <param name="error">output for any errors that might ocurr during this method</param>
        /// <returns>true if encoding has been successfully paused, false if not</returns>
        bool pause();
        /// <summary>
        /// resumes the encoding process
        /// </summary>
        /// <param name="error">output for any errors that might ocurr during this method</param>
        /// <returns>true if encoding has been successfully started, false if not</returns>
        bool resume();
        /// <summary>
        /// changes the priority of the encoding process/thread
        /// </summary>
        /// <param name="priority">the priority to change to</param>
        /// <param name="error">output for any errors that might ocurr during this method</param>
        /// <returns>true if the priority has been changed, false if not</returns>
        void changePriority(WorkerPriorityType priority);
        event JobProcessingStatusUpdateCallback StatusUpdate;
    }
}