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
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Forms;

using MeGUI.core.details;
using MeGUI.core.util;

namespace MeGUI.core.gui
{
    public enum JobWorkerMode { RequestNewJobs, CloseOnLocalListCompleted }
    public enum JobWorkerStatus { Idle, Running, Stopping, Stopped, Postponed }

    /// <summary>
    /// This class represents a processing 'worker', which processes jobs
    /// one by one. In a single instance of MeGUI, there can be multiple
    /// workers, facilitating parallel job processing.
    /// 
    /// JobControl keeps the job queue, and distributes jobs one by one when
    /// requested by a JobWorker. Ordinarily, this worker job list is empty, 
    /// and the worker just requests jobs from the queue until none are left.
    /// However, it may be useful to specify 'run this job now' or 'run these
    /// jobs now', in which case they are put onto the reserved jobs list,
    /// and they are run before requesting any from the job queue.
    /// 
    /// This can be useful for running small jobs like muxing or d2v indexing
    /// while a video encode is going in the background: since the user is at
    /// the computer *now*, he/she doesn't want to wait until the video encode
    /// is finished, and can instead select 'run this job in a new worker.

    /// 
    /// Dependencies are managed by JobControl. Each job has a list of jobs
    /// it depends on; a job will only be distributed to a worker if all the
    /// dependant jobs have been completed successfully.
    /// 
    /// A Worker can run in two modes: RequestNewJobs and CloseOnLocalListCompleted.
    /// RequestNewJobs means that after the the local list is completed, it
    /// requests another job from the Job Control, continuing until all jobs
    /// are completed. In this mode, the JobWorker never closes by itself.
    /// CloseOnLocalListCompleted means that it will complete the jobs on
    /// its local list and then close, without requesting any more jobs from
    /// the JobControl.
    /// 
    /// A Worker must always be in one of five states: Idle, Running, Stopping, Stopped, Postponed.
    /// Idle means that no jobs are currently being processed. Running means
    /// that a job is being processed, and further jobs will continue to be
    /// processed until either there are no more jobs or the worker is closed.
    /// Stopping means that a job is currently being processed, but after this
    /// job is completed, no further jobs will be started. Stopped means that no new
    /// will be processed automatically. Postponed means that because of another running
    /// job in another worker this worker is stopped temporarily.
    /// 
    /// ProcessingThreads can run in several modes, enumerated 
    /// </summary>
    public class JobWorker : IDisposable
    {
        private IJobProcessor currentProcessor;
        private TaggedJob currentJob; // the job being processed at the moment
        private ProgressWindow pw;
        private MainForm mainForm;
        private LogItem log;

        public event EventHandler WorkerFinishedJobs;

        public JobWorker(MainForm mf)
        {
            mainForm = mf;
            pw = new ProgressWindow();
            pw.Abort += new AbortCallback(Pw_Abort);
            pw.Suspend += new SuspendCallback(Pw_Suspend);
            pw.PriorityChanged += new PriorityChangedCallback(Pw_PriorityChanged);
            pw.CreateControl();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // dispose managed resources
                pw.Dispose();
                currentProcessor.Dispose();
            }
            // free native resources
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #region process window opening and closing
        public void HideProcessWindow()
        {
            if (pw != null)
                MainForm.Instance.Jobs.ShowProgressWindow(pw, false);
        }

        public void ShowProcessWindow()
        {
            if (pw != null)
                MainForm.Instance.Jobs.ShowProgressWindow(pw, true);
        }

        /// <summary>
        /// callback for the progress window
        /// this method is called if the abort button in the progress window is called
        /// it stops the encoder cold
        /// </summary>
        private void Pw_Abort()
        {
            UserRequestedAbort();
        }

        /// <summary>
        /// callback for the progress window
        /// this method is called if the suspend button in the progress window is called
        /// it stops the encoder cold
        /// </summary>
        public void Pw_Suspend()
        {
            if (currentJob.Status != JobStatus.PROCESSING && currentJob.Status != JobStatus.PAUSED)
                return;

            if (currentJob.Status == JobStatus.PROCESSING)
                Pause();
            else
                Resume();
        }

        /// <summary>
        /// catches the ChangePriority event from the progresswindow and forward it to the encoder class
        /// </summary>
        /// <param name="priority"></param>
        private void Pw_PriorityChanged(WorkerPriorityType priority)
        {
            try
            {
                currentProcessor.changePriority(priority);
            }
            catch (JobRunException e)
            {
                log.LogValue("Error attempting to change priority", e, ImageType.Error);
            }
        }
        #endregion

        #region public interface
        public decimal Progress
        {
            get
            {
                if (currentJob == null || currentJob.Su == null)
                    return 0;
                return currentJob.Su.PercentageEstimated;
            }
        }

        public string StatusString
        {
            get
            {
                if (status == JobWorkerStatus.Idle)
                    return "idle";
                if (status == JobWorkerStatus.Stopped)
                    return "stopped";
                if (status == JobWorkerStatus.Postponed)
                    return "postponed (another worker is processing an audio job)";
                string _status = "running"; 
                if (currentJob != null)
                    _status += string.Format(new System.Globalization.CultureInfo("en-US"), " {0} ({1:P2})", currentJob.Name, (currentJob.Su != null ? currentJob.Su.PercentageEstimated / 100M : 0));
                if (mode == JobWorkerMode.CloseOnLocalListCompleted)
                    _status += " (delete worker after current job)";
                else if (status == JobWorkerStatus.Stopping)
                    _status += " (stop worker after current job)";
                if (currentJob != null && currentJob.Status == JobStatus.PAUSED)
                    _status += " (paused)";
                return _status;
            }
        }

        public JobStatus JobStatus
        {
            get
            {
                if (currentJob != null)
                    return currentJob.Status;
                else
                    return JobStatus.WAITING;
            }
        }

        private JobWorkerMode mode;
        public JobWorkerMode Mode
        {
            get { return mode; }
            set { mode = value; }
        }

        private JobWorkerStatus status;
        public JobWorkerStatus Status
        {
            get { return status; }
        }

        private bool bIsTemporaryWorker;
        public bool IsTemporaryWorker
        {
            get { return bIsTemporaryWorker; }
            set { bIsTemporaryWorker = value; }
        }

        public void SetStopping()
        {
            if (status == JobWorkerStatus.Running)
                status = JobWorkerStatus.Stopping;
        }

        public void SetRunning()
        {
            if (status == JobWorkerStatus.Stopping)
                status = JobWorkerStatus.Running;
        }

        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public bool IsRunning
        {
            get { return status == JobWorkerStatus.Running || status == JobWorkerStatus.Stopping; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="oSettings">the WorkerSettings to check for</param>
        /// <returns>true if a blocked job is running, false if not</returns>
        public bool IsRunningBlockedJob(WorkerSettings oSettings)
        {
            if (oSettings == null || (status != JobWorkerStatus.Running && status != JobWorkerStatus.Stopping) || currentJob == null)
                return false;

            return oSettings.IsBlockedJob(currentJob.Job);
        }
        #endregion

        #region job run util

        /// <summary>
        /// Postprocesses the given job according to the JobPostProcessors in the mainForm's PackageSystem
        /// </summary>
        /// <param name="job"></param>
        private void PostprocessJob(Job job)
        {
            LogItem i = log.LogEvent("Postprocessing");
            foreach (JobPostProcessor pp in mainForm.PackageSystem.JobPostProcessors.Values)
            {
                LogItem plog = pp.PostProcessor(mainForm, job);
                if (plog != null)
                {
                    i.Add(plog);
                }
            }
        }

        /// <summary>
        /// Preprocesses the given job according to the JobPreProcessors in the mainForm's PackageSystem
        /// </summary>
        /// <param name="job"></param>
        private void PreprocessJob(Job job)
        {
            LogItem i = log.LogEvent("Preprocessing");
            foreach (JobPreProcessor pp in mainForm.PackageSystem.JobPreProcessors.Values)
            {
                LogItem plog = pp.PreProcessor(mainForm, job);
                if (plog != null)
                {
                    i.Add(plog);
                }
            }
        }

        private IJobProcessor GetProcessor(Job job)
        {
            foreach (JobProcessorFactory f in mainForm.PackageSystem.JobProcessors.Values)
            {
                IJobProcessor p = f.Factory(mainForm, job);
                if (p != null)
                {
                    return p;
                }
            }
            log.Error("No processor found");
            return null;
        }
        #endregion

        #region shut down
        internal void ShutDown()
        {
            if (IsRunning)
                Abort();

            foreach (TaggedJob j in mainForm.Jobs.GlobalJobQueue.JobList)
                if (j.OwningWorker != null && j.OwningWorker.Equals(Name))
                    JobControl.UnassignJob(j);

            mainForm.Jobs.ShutDown(this);
        }
        #endregion

        #region gui updates
        private void RefreshAll()
        {
            mainForm.Jobs.RefreshStatus();
        }
        #endregion

        #region job starting / stopping
        #region abort
        /// <summary>
        /// aborts the currently active job
        /// </summary>
        public void Abort()
        {
            Debug.Assert(IsRunning);

            if (currentProcessor != null)
            {
                if (currentJob.Status == JobStatus.ABORTING)
                    return;
                try
                {
                    if (currentJob.Status == JobStatus.PAUSED)
                        currentProcessor.resume();
                    currentJob.Status = JobStatus.ABORTING;
                    RefreshAll();
                    currentProcessor.stop();
                }
                catch (JobRunException er)
                {
                    mainForm.Log.LogValue("Error attempting to stop processing", er, ImageType.Error);
                }
                MarkJobAborted();
            }
            status = JobWorkerStatus.Stopped;
            RefreshAll();
        }

        #endregion

        #region starting jobs
        public void StartEncoding(bool showMessageBoxes)
        {
            status = JobWorkerStatus.Idle;
            JobStartInfo retval = JobStartInfo.COULDNT_START;
            retval = StartNextJobInQueue();
            if (showMessageBoxes)
            {
                if (retval == JobStartInfo.COULDNT_START)
                    MessageBox.Show("Couldn't start processing. Please consult the log for more details", "Processing failed", MessageBoxButtons.OK);
                else if (retval == JobStartInfo.NO_JOBS_WAITING)
                    MessageBox.Show("No jobs are waiting or can be processed at the moment.", "No jobs waiting", MessageBoxButtons.OK);
            }

            // check if a temporary worker has to be closed
            if (bIsTemporaryWorker && retval != JobStartInfo.JOB_STARTED)
                ShutDown();
        }

        /// <summary>
        /// Copies completion info into the job: end time, FPS, status.
        /// </summary>
        /// <param name="job">Job to fill with info</param>
        /// <param name="su">StatusUpdate with info</param>
        private static void copyInfoIntoJob(TaggedJob job, StatusUpdate su)
        {
            Debug.Assert(su.IsComplete);

            job.End = DateTime.Now;
            job.EncodingSpeed = su.OverallSpeed;

            if (su.WasAborted)
                job.Status = JobStatus.ABORTED;
            else if (su.HasError)
                job.Status = JobStatus.ERROR;
        }

        /// <summary>
        /// updates the actual GUI with the status information received as parameter
        /// If the StatusUpdate indicates that the job has ended, the Progress window is closed
        /// and the logging messages from the StatusUpdate object are added to the log tab
        /// if the job mentioned in the statusupdate has a next job name defined, the job is looked
        /// up and processing of that job starts - this applies even in queue encoding mode
        /// the linked jobs will always be encoded first, regardless of their position in the queue
        /// If we're in queue encoding mode, the next nob in the queue is also started
        /// </summary>
        /// <param name="su">StatusUpdate object containing the current encoder stats</param>
        private void UpdateGUIStatus(StatusUpdate su)
        {
            if (su.IsComplete)
                JobFinished(su);
        }

        private void JobFinished(StatusUpdate su)
        {
            // so we don't lock up the GUI, we start a new thread
            Thread t = new Thread(new ThreadStart(delegate
            {
                TaggedJob job = mainForm.Jobs.ByName(su.JobName);
                JobStartInfo JobInfo = JobStartInfo.JOB_STARTED;

                copyInfoIntoJob(job, su);
                HideProcessWindow();

                // Postprocessing
                bool jobFailed = (job.Status != JobStatus.PROCESSING);
                if (!jobFailed)
                {
                    PostprocessJob(job.Job);
                    job.Status = JobStatus.DONE;
                }

                currentProcessor = null;
                currentJob = null;

                // Logging
                if (job.Status == JobStatus.ABORTED)
                    log.LogEvent("Job aborted");
                else
                    log.LogEvent("Job completed");
                log.Collapse();

                if (!jobFailed && mainForm.Settings.WorkerRemoveJob)
                    mainForm.Jobs.RemoveCompletedJob(job);
                else
                    JobControl.SaveJob(job, mainForm.MeGUIPath);

                if (job.Status == JobStatus.ABORTED)
                {
                    status = JobWorkerStatus.Stopped;
                    JobInfo = JobStartInfo.COULDNT_START;
                    if (bIsTemporaryWorker)
                        ShutDown();
                }
                else if (status == JobWorkerStatus.Stopping)
                {
                    log.LogEvent("Queue mode stopped");
                    status = JobWorkerStatus.Stopped;
                    JobInfo = JobStartInfo.COULDNT_START;
                }
                else if (mainForm.Jobs.WorkersCount <= MainForm.Instance.Settings.WorkerMaximumCount || bIsTemporaryWorker)
                {
                    JobInfo = StartNextJobInQueue();
                    switch (JobInfo)
                    {
                        case JobStartInfo.COULDNT_START:
                            if (status != JobWorkerStatus.Postponed)
                                status = JobWorkerStatus.Idle;
                            break;

                        case JobStartInfo.NO_JOBS_WAITING:
                            if (status != JobWorkerStatus.Postponed)
                                status = JobWorkerStatus.Idle;
                            if (mode == JobWorkerMode.CloseOnLocalListCompleted)
                                ShutDown();
                            WorkerFinishedJobs(this, EventArgs.Empty);
                            break;
                    }
                }
                else
                    status = JobWorkerStatus.Idle;

                mainForm.Jobs.AdjustWorkerCount(true);

                if (!mainForm.Jobs.IsAnyJobRunning)
                    MeGUI.core.util.WindowUtil.AllowSystemPowerdown();

                RefreshAll();
            }));
            t.IsBackground = true;
            t.Start();
        }

        /// <summary>
        /// starts the job provided as parameters
        /// </summary>
        /// <param name="job">the Job object containing all the parameters</param>
        /// <returns>success / failure indicator</returns>
        private bool StartEncoding(TaggedJob job)
        {
            try
            {
                log = mainForm.Log.Info(string.Format(new System.Globalization.CultureInfo("en-US"), "Log for {0} ({1}, {2} -> {3})", job.Name, job.Job.EncodingMode, job.InputFileName, job.OutputFileName));
                log.LogEvent("Started handling job");
                log.Expand();

                // check to see if output file already exists before encoding
                if (File.Exists(job.Job.Output))
                {
                    if ((!Path.GetExtension(job.Job.Output).Equals(".lwi") && !Path.GetExtension(job.Job.Output).Equals(".ffindex") &&
                        !Path.GetExtension(job.Job.Output).Equals(".d2v") &&
                        !Path.GetExtension(job.Job.Output).Equals(".dgi")) && !mainForm.DialogManager.overwriteJobOutput(job.Job.Output))
                        throw new JobStartException("File exists and the user doesn't want to overwrite", ExceptionType.UserSkip);
                    
                    FileUtil.DeleteFile(job.Job.Output, log);
                }
                
                // get IJobProcessor
                currentProcessor = GetProcessor(job.Job);
                if (currentProcessor == null)
                    throw new JobStartException("No processor could be found", ExceptionType.Error);

                // Preprocess
                PreprocessJob(job.Job);

                // Setup
                try
                {
                    job.Su = new StatusUpdate(job.Name, job.Job.Output);
                    currentProcessor.setup(job.Job, job.Su, log);
                    pw.SetStatusUpdate(job.Su);
                }
                catch (JobRunException e)
                {
                    throw new JobStartException("Calling setup of processor failed with error '" + e.Message + "'", ExceptionType.Error);
                }

                if (currentProcessor == null)
                {
                    throw new JobStartException("starting job failed", ExceptionType.Error);
                }

                // Do JobControl setup
                currentProcessor.StatusUpdate += new JobProcessingStatusUpdateCallback(UpdateGUIStatus);

                // Progress window
                WorkerPriority.GetJobPriority(job.Job, out WorkerPriorityType oPriority, out bool lowIOPriority);
                pw.setPriority(oPriority);
                if (mainForm.Settings.OpenProgressWindow && mainForm.Visible)
                    this.ShowProcessWindow();

                job.Start = DateTime.Now;
                currentJob = job;

                // Start
                try
                {
                    currentProcessor.start();
                }
                catch (JobRunException e)
                {
                    throw new JobStartException("starting job failed with error '" + e.Message + "'", ExceptionType.Error);
                }

                RefreshAll();
                MeGUI.core.util.WindowUtil.PreventSystemPowerdown();
                return true;
            }
            catch (JobStartException e)
            {
                this.HideProcessWindow();
                log.LogValue("Error starting job", e);
                if (e.EType == ExceptionType.Error)
                    job.Status = JobStatus.ERROR;
                else // ExceptionType.UserSkip
                    job.Status = JobStatus.SKIP;
                currentProcessor = null;
                currentJob = null;
                RefreshAll();
                return false;
            }

        }

        /// <summary>
        /// Returns the first job on the queue whose dependencies have been met, whose status
        /// is set to 'waiting', and which isn't owned by any JobWorkers
        /// </summary>
        /// <returns></returns>
        private TaggedJob GetNextAvailableJob()
        {
            TaggedJob oJob = null;
            lock (JobControl.ResourceLock)
            {
                foreach (TaggedJob job in mainForm.Jobs.GlobalJobQueue.JobList)
                {
                    if (mode == JobWorkerMode.CloseOnLocalListCompleted &&                  // if this worker is about to be closed after local queue processing
                        (job.OwningWorker == null || !job.OwningWorker.Equals(this.Name)))  // ignore jobs which are not assigned to this worker
                        continue;

                    if (job.Status == JobStatus.WAITING &&                                  // only process waiting jobs
                        (job.OwningWorker == null || job.OwningWorker.Equals(this.Name)) && // only process unassigned jobs or jobs which are assigned to the current worker
                        JobControl.AreDependenciesMet(job) &&                            // check if all job dependencies are fullfilled
                        (mainForm.Jobs.CanNewJobBeStarted(job.Job) || bIsTemporaryWorker))  // check if a blocking encoding is already in process - always process if temporary worker
                    {
                        job.Status = JobStatus.PROCESSING;                                  // mark job as in processing so that no other worker can fetch it
                        oJob = job;
                        break;
                    }
                }
            }
            return oJob;
        }

        /// <summary>
        /// starts the next job in the queue which is available
        /// </summary>
        /// <returns></returns>
        private JobStartInfo StartNextJobInQueue()
        {
            TaggedJob job = GetNextAvailableJob();
            if (job == null)
            {
                status = JobWorkerStatus.Idle;
                RefreshAll();
                return JobStartInfo.NO_JOBS_WAITING;
            }

            status = JobWorkerStatus.Running;
            RefreshAll();

            if (StartEncoding(job))
            {
                // successfully started
                return JobStartInfo.JOB_STARTED; 
            }
            else
            {
                status = JobWorkerStatus.Idle;
                RefreshAll();
                return JobStartInfo.COULDNT_START;
            }
        }
        #endregion
        #endregion

        /// <summary>
        /// marks job currently marked as processing as aborted
        /// </summary>
        private void MarkJobAborted()
        {
            if (currentJob == null)
                return;

            TaggedJob job = currentJob;
            job.Status = JobStatus.ABORTED;
            job.End = DateTime.Now;

            LogItem i = new LogItem(string.Format("[{0:G}] {1}", DateTime.Now, "Deleting aborted output"));
            i.LogValue("Delete aborted output set", mainForm.Settings.DeleteAbortedOutput);
            if (mainForm.Settings.DeleteAbortedOutput && File.Exists(job.Job.Output))
            {
                // delete outout file and temporary files
                if (File.Exists(job.Job.Output))
                {
                    FileUtil.DeleteFile(job.Job.Output, i);
                    if (!File.Exists(job.Job.Output))
                        i.LogValue("File deleted", job.Job.Output);
                }
                foreach (string strFile in job.Job.FilesToDelete)
                {
                    if (!File.Exists(strFile))
                        continue;

                    FileUtil.DeleteFile(strFile, i);
                    if (!File.Exists(strFile))
                        i.LogValue("File deleted", strFile);
                }
            }
            log.Add(i);
        }

        #region pause / resume
        public void Pause()
        {
            Debug.Assert(currentJob.Status == JobStatus.PROCESSING);
            try
            {
                if (currentProcessor.pause())
                {
                    currentJob.Status = JobStatus.PAUSED;
                    RefreshAll();

                    if (!mainForm.Jobs.IsAnyJobRunning)
                        MeGUI.core.util.WindowUtil.AllowSystemPowerdown();
                }
            }
            catch (JobRunException ex)
            {
                mainForm.Log.LogValue("Error trying to pause encoding", ex, ImageType.Warning);
            }
        }

        public void Resume()
        {
            Debug.Assert(currentJob.Status == JobStatus.PAUSED);
            try
            {
                if (currentProcessor.resume())
                {
                    currentJob.Status = JobStatus.PROCESSING;
                    RefreshAll();

                    MeGUI.core.util.WindowUtil.PreventSystemPowerdown();
                }
            }
            catch (JobRunException ex)
            {
                mainForm.Log.LogValue("Error trying to resume encoding", ex, ImageType.Warning);
            }
        }
        #endregion

        public void UserRequestedAbort()
        {
            if (currentJob == null)
            {
                Abort();
                return;
            }

            if (currentJob.Status == JobStatus.ABORTED || currentJob.Status == JobStatus.ABORTING)
            {
                MessageBox.Show("Job already aborting. Please wait.", "Abort in progress", MessageBoxButtons.OK);
                return;
            }

            DialogResult r = MessageBox.Show("Do you really want to abort?", "Really abort?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (r == DialogResult.Yes)
                Abort();
        }

        /// <summary>
        /// return true if the worker is running
        /// </summary>
        public bool IsProgressWindowAvailable
        { 
            get { return IsRunning; } 
        }


        /// <summary>
        /// return true if the progress window is visible
        /// </summary>
        public bool IsProgressWindowVisible
        {
            get { return (pw != null && pw.Visible); }
        }
    }

    public enum ExceptionType { UserSkip, Error };
    public class JobStartException : MeGUIException
    {
        private ExceptionType type;

        public JobStartException() : base("MeGUIException") { this.EType = ExceptionType.Error; }
        public JobStartException(string reason) : base(reason) { this.EType = ExceptionType.Error; }
        public JobStartException(string reason, Exception innerException) : base(reason, innerException) { this.EType = ExceptionType.Error; }
        public JobStartException(string reason, ExceptionType type) : base(reason) { this.EType = type; }

        public ExceptionType EType { get => type; set => type = value; }
    }
}