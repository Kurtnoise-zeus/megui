// ****************************************************************************
// 
// Copyright (C) 2005-2024 Doom9 & al
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
using System.Windows.Forms;
using System.Xml.Serialization;
using System.Linq;

using MeGUI.core.util;
using MeGUI.core.gui;

namespace MeGUI.core.details
{
    public enum JobStartInfo { JOB_STARTED, NO_JOBS_WAITING, COULDNT_START }

    public partial class JobControl : UserControl
    {
        private Dictionary<string, TaggedJob> allJobs = new Dictionary<string, TaggedJob>(); //storage for all the jobs and profiles known to the system
        private Dictionary<string, JobWorker> workers = new Dictionary<string, JobWorker>();
        private MainForm mainForm;
        private WorkerSummary summary;
        private AfterEncoding currentAfterEncoding;
        static readonly object resourceLock = new object();

        public JobControl()
        {
            InitializeComponent();
            mainForm = MainForm.Instance;
            AddClearButton();
            AddSendToTemporaryWorkerMenuItem();
            globalJobQueue.RequestJobDeleted = new RequestJobDeleted(this.DeleteJob);
            summary = new WorkerSummary();
        }

        public JobQueue GlobalJobQueue
        {
            get { return globalJobQueue; }
            set { globalJobQueue = value; }
        }

        #region public interface: process windows, start/stop/abort

        /// <summary>
        /// shows all process windows
        /// </summary>
        public void ShowAllProcessWindows()
        {
            foreach (JobWorker w in workers.Values)
                if (w.IsProgressWindowAvailable)
                    w.ShowProcessWindow();
        }


        /// <summary>
        /// hides all process windows
        /// </summary>
        public void HideAllProcessWindows()
        {
            foreach (JobWorker w in workers.Values)
                w.HideProcessWindow();
        }

        /// <summary>
        /// Changes the visibility of the ProgressWindow
        /// Invoke needed to ensure that it is threadsafe
        /// Therefore it will run in the JobControl thread
        /// </summary>
        /// <param name="oProgress">the ProgressWindow to change </param>
        /// <param name="bShow">true if to be shown, false if not</param>
        public void ShowProgressWindow(ProgressWindow oProgress, bool bShow)
        {
            this.Invoke((System.Action)delegate { oProgress.Visible = bShow; });
        }

        /// <summary>
        /// aborts all running workers
        /// </summary>
        public void AbortAll()
        {
            foreach (JobWorker worker in workers.Values)
                if (worker.IsRunning)
                    worker.Abort();
            RefreshStatus();
        }

        /// <summary>
        /// starts all workers
        /// </summary>
        /// <param name="restartStopping">true if also stopping workers should be started</param>
        public void StartAll(bool restartStopping)
        {
            foreach (JobWorker w in workers.Values)
            {
                if (!w.IsRunning)
                    w.StartEncoding(false);
                else if (restartStopping && w.Status == JobWorkerStatus.Stopping)
                    w.SetRunning();
            }
            RefreshStatus();
        }

        /// <summary>
        /// sets all workers to stop after the current job
        /// </summary>
        public void StopAll()
        {
            foreach (JobWorker w in workers.Values)
                if (w.IsRunning) 
                    w.SetStopping();
            RefreshStatus();
        }
        #endregion

        /// <summary>
        /// adjusts the worker count either after a job has finished or if the maximum worker number has been changed
        /// </summary>
        /// <param name="startIdleWorkers">if true idle workers will be started</param>
        public void AdjustWorkerCount(bool startIdleWorkers)
        {
            lock (mainForm.Jobs.ResourceLock)
            {
                // remove worker(s) if needed
                if (PermanentWorkerCount > MainForm.Instance.Settings.WorkerMaximumCount)
                {

                    foreach (string key in workers.Keys.Reverse())
                    {
                        if (!workers.TryGetValue(key, out JobWorker worker))
                            continue;

                        if (!worker.IsRunning)
                        {
                            worker.ShutDown();
                            if (workers.Values.Count <= MainForm.Instance.Settings.WorkerMaximumCount)
                                break;
                        }
                    }
                }

                // add worker(s) if needed
                while (PermanentWorkerCount < MainForm.Instance.Settings.WorkerMaximumCount)
                    NewWorker("Worker");
            }

            // check if any jobs can be started
            if (startIdleWorkers)
                StartIdleWorkers();
        }

        private int PermanentWorkerCount
        {
            get
            {
                int workerCount = 0;
                foreach (JobWorker w in workers.Values)
                {
                    if (!w.IsTemporaryWorker)
                        workerCount++;
                }
                return workerCount;
            }
        }

        /// <summary>
        /// created a new worker
        /// </summary>
        /// <param name="name">prefix of the worker name</param>
        /// <returns>the new worker</returns>
        private JobWorker NewWorker(string prefix)
        {
            int num = 0;
            string name;
            do
            {
                name = prefix + " " + ++num;
            } while (workers.ContainsKey(name));

            JobWorker w = new JobWorker(mainForm);
            w.Name = name;
            w.WorkerFinishedJobs += new EventHandler(WorkerFinishedJobs);
            workers.Add(w.Name, w);
            summary.Add(w);
            return w;
        }

        /// <summary>
        /// lock object
        /// </summary>
        public object ResourceLock
        {
            get { return resourceLock; }
        }

        /// <summary>
        /// adds the "run in new temporary worker" menu item
        /// </summary>
        private void AddSendToTemporaryWorkerMenuItem()
        {
            globalJobQueue.AddMenuItem("Run in new temporary worker", null, new MultiJobHandler(
                delegate (List<TaggedJob> jobs)
                {
                    JobWorker w = NewWorker("Temporary worker");

                    foreach (TaggedJob j in jobs)
                    {
                        if (j.Status != JobStatus.WAITING)
                            continue;
                        AssignJob(j, w.Name);
                    }
                    RefreshStatus();
                    w.Mode = JobWorkerMode.CloseOnLocalListCompleted;
                    w.IsTemporaryWorker = true;
                    w.StartEncoding(true);
                }));
        }

        /// <summary>
        /// Unassigns a Job
        /// </summary>
        /// <param name="j"></param>
        public void UnassignJob(TaggedJob j)
        {
            j.OwningWorker = null;
        }
        
        /// <summary>
        /// Assigns a Job
        /// </summary>
        /// <param name="j">the job to assign</param>
        /// <param name="WorkerName">the worker to assign the job to</param>
        public void AssignJob(TaggedJob j, string WorkerName)
        {
            j.OwningWorker = WorkerName;
        }

        #region properties
        /// <summary>
        /// true if any worker is currently running a job
        /// </summary>
        public bool IsAnyWorkerRunning
        {
            get
            {
                foreach (JobWorker w in workers.Values)
                    if (w.IsRunning) 
                        return true;
                return false;
            }
        }

        /// <summary>
        /// true if any job is currently in state processing
        /// </summary>
        public bool IsAnyJobRunning
        {
            get
            {
                foreach (JobWorker w in workers.Values)
                    if (w.JobStatus == JobStatus.PROCESSING)
                        return true;
                return false;
            }
        }

        /// <summary>
        /// count of all workers
        /// </summary>
        public int WorkersCount
        {
            get
            {
                return workers.Values.Count;
            }
        }

        /// <summary>
        /// checks if the job can be started based on the worker rule set(s)
        /// </summary>
        /// <param name="oJob"></param>
        /// <returns>true if the new job can be started</returns>
        public bool CanNewJobBeStarted(Job oJob)
        {
            foreach (WorkerSettings oSettings in MainForm.Instance.Settings.WorkerSettings)
            {
                int iMax = oSettings.MaxCount;
                if (iMax <= 0)
                    continue;

                // check if the job which should be started is one of the blocked types
                if (!oSettings.IsBlockedJob(oJob))
                    continue;

                foreach (JobWorker w in workers.Values)
                {
                    if (w.IsRunningBlockedJob(oSettings))
                        iMax--;
                }

                if (iMax <= 0)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// what should happen after all jobs are finished
        /// </summary>
        public AfterEncoding CurrentAfterEncoding
        {
            get
            {
                return currentAfterEncoding;
            }
        }
        #endregion

        #region Clear button
        /// <summary>
        /// adds the button to remove all jobs from the queue
        /// </summary>
        private void AddClearButton()
        {
            globalJobQueue.AddButton("Clear", new EventHandler(DeleteAllJobsButton_Click));
        }

        /// <summary>
        /// if fired all jobs will be deleted
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteAllJobsButton_Click(object sender, EventArgs e)
        {
            int incompleteJobs = 0;
            DialogResult dr = DialogResult.No;
            TaggedJob[] jobList = new TaggedJob[allJobs.Count];
            allJobs.Values.CopyTo(jobList, 0);
            foreach (TaggedJob j in jobList)
            {
                if (j.Status != JobStatus.DONE)
                    ++incompleteJobs;
            }
            if (incompleteJobs != 0)
            {
                dr = MessageBox.Show("Delete incomplete jobs as well?\n\nYes for All, No for completed or Cancel to abort:", "Are you sure you want to clear the queue?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (dr == DialogResult.Cancel) 
                    return;
            }
            foreach (TaggedJob j in jobList)
            {
                if (dr == DialogResult.Yes || j.Status == JobStatus.DONE)
                    ReallyDeleteJob(j);
            }
        }
        #endregion
        #region deleting jobs
        /// <summary>
        /// deletes a selected job
        /// </summary>
        /// <param name="job">job to delete</param>
        public void DeleteJob(TaggedJob job)
        {
            if (job.Status == JobStatus.PROCESSING || job.Status == JobStatus.PAUSED || job.Status == JobStatus.ABORTING)
            {
                MessageBox.Show("You cannot delete a job while it is being processed.", "Deleting " + job.Name + " failed", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            if (job.EnabledJobs.Count == 0 && job.RequiredJobs.Count == 0)
            {
                ReallyDeleteJob(job);
                return;
            }

            DialogResult dr = MessageBox.Show("'" + job.Name +
                "' is related to a job series. Do you want to delete all related jobs?\r\n" +
                "Press Yes to delete all related jobs, No to delete this job and " +
                "remove the dependencies, or Cancel to abort",
                "Job dependency detected",
                MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Warning);

            switch (dr)
            {
                case DialogResult.Yes: // Delete all dependent jobs
                    DeleteAllDependantJobs(job);
                    break;

                case DialogResult.No: // Just delete the single job
                    ReallyDeleteJob(job);
                    break;

                case DialogResult.Cancel: // do nothing
                    break;
            }
        }

        /// <summary>
        /// removes this job, and any previous jobs that belong to a series of jobs from the
        /// queue, then update the queue positions
        /// </summary>
        /// <param name="job">the job to be removed</param>
        internal void RemoveCompletedJob(TaggedJob job)
        {
            ReallyDeleteJob(job);
        }

        /// <summary>
        /// force deletion of the selected job without dependencies
        /// </summary>
        /// <param name="job">job to delete</param>
        private void ReallyDeleteJob(TaggedJob job)
        {
            if (job.Status == JobStatus.PROCESSING || job.Status == JobStatus.PAUSED || job.Status == JobStatus.ABORTING) 
                return;

            if (job.Status != JobStatus.DONE && MainForm.Instance.Settings.DeleteIntermediateFiles)
            {
                List<string> filesToDelete = new List<string>();
                if (job.Job.FilesToDelete.Count > 0)
                    filesToDelete.AddRange(job.Job.FilesToDelete);
                if (filesToDelete.Count > 0)
                {
                    LogItem oLog = FileUtil.DeleteIntermediateFiles(filesToDelete, false, true);
                    if (oLog != null)
                    {
                        LogItem log = mainForm.Log.Info(string.Format("Log for {0} ({1}, {2} -> {3})", job.Name, job.Job.EncodingMode, job.InputFileName, job.OutputFileName));
                        log.Add(oLog);
                    }
                }
            }

            lock (mainForm.Jobs.ResourceLock)
            {
                if (globalJobQueue.HasJob(job))
                    globalJobQueue.RemoveJobFromQueue(job);

                foreach (TaggedJob p in job.RequiredJobs)
                    p.EnabledJobs.Remove(job);

                foreach (TaggedJob j in job.EnabledJobs)
                    j.RequiredJobs.Remove(job);

                string fileName = Path.Combine(mainForm.MeGUIPath, "jobs");
                fileName = Path.Combine(fileName, job.Name + ".xml");
                if (File.Exists(fileName))
                    File.Delete(fileName);

                allJobs.Remove(job.Name);
            }
        }

        /// <summary>
        /// delete also all dependent jobs
        /// </summary>
        /// <param name="job">job to delete</param>
        private void DeleteAllDependantJobs(TaggedJob job)
        {
            ReallyDeleteJob(job);

            foreach (TaggedJob j in job.EnabledJobs)
                DeleteAllDependantJobs(j);

            foreach (TaggedJob j in job.RequiredJobs)
                DeleteAllDependantJobs(j);
        }
        #endregion

        /// <summary>
        /// refresh status
        /// </summary>
        public void RefreshStatus()
        {
            globalJobQueue.RefreshQueue();
            summary.RefreshInfo();
        }
        
        /// <summary>
        /// start all idle workers
        /// </summary>
        public void StartIdleWorkers()
        {
            // start idle and postponed workers if required
            foreach (JobWorker w in workers.Values)
                if (!w.IsRunning && ((w.Status == JobWorkerStatus.Idle && MainForm.Instance.Settings.WorkerAutoStart) || w.Status == JobWorkerStatus.Postponed))
                    w.StartEncoding(false);
        }

        #region saving / loading jobs
        public List<string> ToStringList(IEnumerable<TaggedJob> jobList)
        {
            List<string> strings = new List<string>();
            foreach (TaggedJob j in jobList)
                strings.Add(j.Name);
            return strings;
        }

        /// <summary>
        /// saves all the jobs in the queue
        /// </summary>
        public void SaveJobs()
        {
            lock (mainForm.Jobs.ResourceLock)
            {
                foreach (TaggedJob job in allJobs.Values)
                {
                    job.EnabledJobNames = ToStringList(job.EnabledJobs);
                    job.RequiredJobNames = ToStringList(job.RequiredJobs);
                    SaveJob(job, mainForm.MeGUIPath);
                }

                JobListSerializer s = new JobListSerializer();
                s.mainJobList = ToStringList(globalJobQueue.JobList);
                string path = Path.Combine(mainForm.MeGUIPath, "joblists.xml");
                Util.XmlSerialize(s, path);
            }
        }

        public class JobListSerializer
        {
            public List<string> mainJobList = new List<string>();
        }

        /// <summary>
        /// loads all the jobs from the harddisk
        /// upon loading, the jobs are ordered according to their position field
        /// so that the order in which the jobs were previously shown in the GUI is preserved
        /// </summary>
        public void LoadJobs()
        {
            string jobsPath = Path.Combine(mainForm.MeGUIPath, "jobs");
            DirectoryInfo di = FileUtil.ensureDirectoryExists(jobsPath);
            FileInfo[] files = di.GetFiles("*.xml");
            foreach (FileInfo fi in files)
            {
                string fileName = fi.FullName;
                TaggedJob job = LoadJob(fileName);
                if (job != null && job.Name != null)
                {
                    if (allJobs.ContainsKey(job.Name))
                        MessageBox.Show("A job named " + job.Name + " is already in the queue.\nThe job defined in " + fileName + "\nwill be discarded", "Duplicate job name detected",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    else
                        allJobs.Add(job.Name, job);
                }
            }

            foreach (TaggedJob job in allJobs.Values)
            {
                if (job.Status == JobStatus.PROCESSING || job.Status == JobStatus.PAUSED || job.Status == JobStatus.ABORTING)
                    job.Status = JobStatus.ABORTED;

                job.RequiredJobs = ToJobList(job.RequiredJobNames);
                job.EnabledJobs = ToJobList(job.EnabledJobNames);
            }

            string path = Path.Combine(mainForm.MeGUIPath, "joblists.xml");
            JobListSerializer s = Util.XmlDeserializeOrDefault<JobListSerializer>(path);
            globalJobQueue.JobList = ToJobList(s.mainJobList);

            foreach (TaggedJob job in allJobs.Values)
            {
                if (globalJobQueue.HasJob(job))
                    continue;
                globalJobQueue.QueueJob(job);
            }

            AdjustWorkerCount(false);
        }

        public List<TaggedJob> ToJobList(IEnumerable<string> list)
        {
            List<TaggedJob> jobList = new List<TaggedJob>();
            foreach (string name in list)
            {
                try
                {
                    jobList.Add(allJobs[name]);
                }
                catch (KeyNotFoundException)
                {

                }
            }
            return jobList;
        }

        #region individual job saving and loading
        /// <summary>
        /// saves a job to programdirectory\jobs\jobname.xml
        /// using the XML Serializer we get a humanly readable file
        /// </summary>
        /// <param name="job">the Job object to be saved</param>
        /// <param name="path">The path where the program was launched from</param>
        public void SaveJob(TaggedJob job, string path)
        {
            string fileName = Path.Combine(Path.Combine(path, "jobs"), job.Name + ".xml");
            Util.XmlSerialize(job, fileName);
        }

        /// <summary>
        /// loads a job with a given name from programdirectory\jobs\jobname.xml
        /// </summary>
        /// <param name="name">name of the job to be loaded (corresponds to the filename)</param>
        /// <returns>the Job object that was read from the harddisk</returns>
        private TaggedJob LoadJob(string name)
        {
            XmlSerializer ser = null;
            using (Stream s = File.OpenRead(name))
            {
                try
                {
                    ser = new XmlSerializer(typeof(TaggedJob));
                    return (TaggedJob)ser.Deserialize(s);
                }
                catch (Exception e)
                {
                    DialogResult r = MessageBox.Show("Job " + name + " could not be loaded. Delete?", "Error loading Job", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                    if (r == DialogResult.Yes)
                    {
                        try 
                        { 
                            s.Close(); 
                            File.Delete(name); 
                        }
                        catch (Exception) { }
                    }
                    LogItem _oLog = MainForm.Instance.Log.Info("Error");
                    _oLog.LogValue("loadJob: " + name, e, ImageType.Error);
                    return null;
                }
            }
        }
        #endregion

        #endregion
        
        #region adding jobs to queue
        public void AddJobsWithDependencies(JobChain c, bool bStartQueue)
        {
            if (c == null)
                return;

            foreach (TaggedJob j in c.Jobs)
                AddJob(j);
            SaveJobs();
            if (mainForm.Settings.WorkerAutoStart && bStartQueue)
                StartAll(false);
            RefreshStatus();
        }

        /// <summary>
        /// adds a job to the Queue (Hashtable) and the listview for graphical display
        /// </summary>
        /// <param name="job">the Job to be added to the next free spot in the queue</param>
        public void AddJobsToQueue(params Job[] jobs)
        {
            foreach (Job j in jobs)
                AddJob(new TaggedJob(j));
            SaveJobs();
            if (mainForm.Settings.WorkerAutoStart)
                StartAll(false);
            RefreshStatus();
        }

        private void AddJob(TaggedJob job)
        {
            lock (mainForm.Jobs.ResourceLock)
            {
                int jobNr = 1;
                string name = "";
                while (true)
                {
                    name = "job" + jobNr;
                    if (!allJobs.ContainsKey(name))
                    {
                        job.Name = name;
                        break;
                    }
                    jobNr++;
                }
                allJobs[job.Name] = job;
                globalJobQueue.QueueJob(job);
            }
        }
        #endregion

        public void ShowAfterEncodingStatus(MeGUISettings Settings)
        {
            currentAfterEncoding = Settings.AfterEncoding;
            cbAfterEncoding.SelectedIndex = (int) currentAfterEncoding;
            if (String.IsNullOrEmpty(Settings.AfterEncodingCommand))
                cbAfterEncoding.Items[2] = "Run command (command not specified!)";
            else
                cbAfterEncoding.Items[2] = "Run '" + Settings.AfterEncodingCommand + "'";
        }


        /// <summary>
        /// Returns the job under the given name
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public TaggedJob ByName(string p)
        {
            return allJobs[p];
        }

        /// <summary>
        /// Returns whether all the jobs that j depends on have been successfully completed
        /// </summary>
        /// <param name="j"></param>
        /// <returns></returns>
        public bool AreDependenciesMet(TaggedJob j)
        {
            foreach (TaggedJob job in j.RequiredJobs)
                if (job.Status != JobStatus.DONE && job.Status != JobStatus.SKIP)
                    return false;

            return true;
        }

        private void JobQueue_StartClicked(object sender, EventArgs e)
        {
            StartAll(true);
        }

        private void JobQueue_StopClicked(object sender, EventArgs e)
        {
            StopAll();
        }

        private void JobQueue_AbortClicked(object sender, EventArgs e)
        {
            DialogResult r = MessageBox.Show("Do you really want to abort all jobs?", "Really abort?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (r == DialogResult.Yes)
                AbortAll();
        }

        private List<Pair<string, bool>> ListWorkers()
        {
            List<Pair<string, bool>> ans = new List<Pair<string,bool>>();
            
            foreach (JobWorker w in workers.Values)
            {
                ans.Add(new Pair<string,bool>(w.Name, false));
            }

            return ans;
        }

        private void WorkerFinishedJobs(object sender, EventArgs e)
        {
            if (IsAnyWorkerRunning)
                return;

            mainForm.runAfterEncodingCommands();
        }

        public void ShowSummary()
        {
            summary.Show();
            summary.BringToFront();
        }

        public void UpdateProgress(string name)
        {
            summary.RefreshInfo(name);
        }

        public void ShutDown(JobWorker w)
        {
            workers.Remove(w.Name);
            summary.Remove(w.Name);
        }

        public List<Pair<string, bool>> ListProgressWindows()
        {
            List<Pair<string, bool>> ans = new List<Pair<string,bool>>();
            foreach (JobWorker w in workers.Values)
            {
                if (w.IsProgressWindowAvailable)
                    ans.Add(new Pair<string, bool>(w.Name, w.IsProgressWindowVisible));
            }
            return ans;
        }

        public void HideProgressWindow(string p)
        {
            if (workers[p].IsProgressWindowAvailable)
                workers[p].HideProcessWindow();
        }

        public void ShowProgressWindow(string p)
        {
            if (workers[p].IsProgressWindowAvailable)
                workers[p].ShowProcessWindow();
        }

        private void CbAfterEncoding_SelectedIndexChanged(object sender, EventArgs e)
        {
            currentAfterEncoding = (AfterEncoding)cbAfterEncoding.SelectedIndex;
        }
    }
}