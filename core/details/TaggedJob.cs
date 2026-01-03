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
using System.Xml.Serialization;

namespace MeGUI.core.details
{
    public class TaggedJob
    {
        private Job job;
        private JobStatus status;
        private DateTime start, end; // time the job was started / ended
        private string name;
        private string owningWorker;
        private List<string> requiredJobNames;
        private List<string> enabledJobNames;
        private string encodingSpeed = "";

        [XmlIgnore]
        private StatusUpdate su;

        [XmlIgnore]
        private List<TaggedJob> enabledJobs = new List<TaggedJob>();

        [XmlIgnore]
        private List<TaggedJob> requiredJobs = new List<TaggedJob>();

        public TaggedJob() { }

        internal TaggedJob(Job j) : this()
        {
            job = j;
        }

        public string EncodingSpeed { get => encodingSpeed; set => encodingSpeed = value; }
        
        [XmlIgnore]
        public StatusUpdate Su { get => su; set => su = value; }

        [XmlIgnore]
        public List<TaggedJob> EnabledJobs { get => enabledJobs; set => enabledJobs = value; }

        [XmlIgnore]
        public List<TaggedJob> RequiredJobs { get => requiredJobs; set => requiredJobs = value; }

        public Job Job
        {
            get { return job; }
            set { job = value; }
        }

        [XmlIgnore]
        public string OwningWorker
        {
            get { return owningWorker; }
            set { owningWorker = value; }
        }

        public void AddDependency(TaggedJob other)
        {
            if (other == null)
                return;
            
            // we can't have each job depending on the other
            System.Diagnostics.Debug.Assert(!other.RequiredJobs.Contains(this));
            RequiredJobs.Add(other);
            other.EnabledJobs.Add(this);
        }

        /// <summary>
        /// List of jobs which need to be completed before this can be processed
        /// </summary>
        public List<string> RequiredJobNames
        {
            get { return requiredJobNames; }
            set { requiredJobNames = value; }
        }

        /// <summary>
        /// List of jobs which completing this job enables
        /// </summary>
        public List<string> EnabledJobNames
        {
            get { return enabledJobNames; }
            set { enabledJobNames = value; }
        }

        /// <summary>
        /// the name of this job
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// status of the job
        /// </summary>
        public JobStatus Status
        {
            get { return status; }
            set
            {
                status = value;
                                if (Su != null)
                    Su.JobStatus = status;
            }
        }

        /// <summary>
        /// time the job was started
        /// </summary>
        public DateTime Start
        {
            get { return start; }
            set { start = value; }
        }

        /// <summary>
        ///  time the job was completed
        /// </summary>
        public DateTime End
        {
            get { return end; }
            set { end = value; }
        }

        /// <summary>
        /// gets a humanly readable status tring
        /// </summary>
        public string StatusString
        {
            get
            {
                switch (status)
                {
                    case JobStatus.WAITING:
                        return "waiting";
                    case JobStatus.PROCESSING:
                        return "processing";
                    case JobStatus.PAUSED:
                        return "paused";
                    case JobStatus.POSTPONED:
                        return "postponed";
                    case JobStatus.ERROR:
                        return "error";
                    case JobStatus.ABORTING:
                        return "aborting";
                    case JobStatus.ABORTED:
                        return "aborted";
                    case JobStatus.DONE:
                        return "done";
                    case JobStatus.SKIP:
                        return "skip";
                    default:
                        return "";
                }
            }
        }

        /// <summary>
        /// filename without path of the source for this job
        /// </summary>
        public string InputFileName
        {
            get
            {
                return Path.GetFileName(this.Job.Input);
            }
        }

        /// <summary>
        /// input file name & path of the job
        /// </summary>
        public string InputFile
        {
            get
            {
                return this.Job.Input;
            }
        }

        /// <summary>
        ///  filename without path of the destination of this job
        /// </summary>
        public string OutputFileName
        {
            get
            {
                return Path.GetFileName(this.Job.Output);
            }
        }

        /// <summary>
        /// output file name & path the job
        /// </summary>
        public string OutputFile
        {
            get
            {
                return this.Job.Output;
            }
        }
    }
}