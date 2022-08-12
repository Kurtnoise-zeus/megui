#if NONE
using System;
using System.Collections.Generic;
using System.Text;
using MeGUI.core.gui;
using System.Diagnostics;
using MeGUI.core.util;
using System.Windows.Forms;
using System.IO;
using System.Threading;

namespace MeGUI.core.details
{

    public class NameTakenException : MeGUIException {
        private string name;
        public string Name
        {
            get { return name; }
        }
        public NameTakenException(string name)
            : base("Worker name '" + name + "' is already in use.")
        {
            this.name = name;
        }
    }
    /// <summary>
    /// This class represents a processing 'worker', which processes jobs
    /// one by one. In a single instance of MeGUI, there can be multiple
    /// workers, facilitating parallel job processing.
    /// 
    /// JobControl keeps the job queue, and distributes jobs one by one when
    /// requested by a JobWorker. Each JobWorker also maintains a list of jobs
    /// reserved for that particular worker. Ordinarily, this list is empty, 
    /// and the worker just requests jobs from the queue until none are left.
    /// However, it may be useful to specify 'run this job now' or 'run these
    /// jobs now', in which case they are put onto the reserved jobs list,
    /// and they are run before requesting any from the job queue.
    /// 
    /// This can be useful for running small jobs like muxing or d2v indexing
    /// while a video encode is going in the background: since the user is at
    /// the computer *now*, he/she doesn't want to wait until the video encode
    /// is finished, and can instead select 'run this job in a new worker.'
    /// This will also be how indexing and autodeint jobs are run from the
    /// AviSynth script creator. In fact, it will be possible to process jobs
    /// without them ever being on the main job queue: they can be put directly
    /// onto a worker's local list and run from that.
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
    /// A Worker must always be in one of three states: Idle, Running, Stopping.
    /// Idle means that no jobs are currently being processed. Running means
    /// that a job is being processed, and further jobs will continue to be
    /// processed until either there are no more jobs or the worker is closed.
    /// Stopping means that a job is currently being processed, but after this
    /// job is completed, no further jobs will be started.
    /// 
    /// 
    /// 
    /// ProcessingThreads can run in several modes, enumerated 
    /// </summary>
    public class JobWorker
    {
        private IJobProcessor currentProcessor;
        private Job currentJob; // The job being processed at the moment
        private ProgressWindow pw;
        private JobWorkerWindow display;
        private MainForm mainForm;
        private int progress;

        #region process window opening and closing
        public void HideProcessWindow()
        {
            if (pw != null)
                pw.Hide();
        }

        public void ShowProcessWindow()
        {
            if (pw != null)
                pw.Show();
        }

        public bool ProcessWindowAccessible
        {
            get { return (pw != null); }
        }
        /// <summary>
        /// callback for the Progress Window
        /// This is called when the progress window has been closed and ensures that
        /// no futher attempt is made to send a statusupdate to the progress window
        /// </summary>
        private void pw_WindowClosed(bool hideOnly)
        {
            mainForm.ProcessStatusChecked = false;
            if (!hideOnly)
                pw = null;
        }
        /// <summary>
        /// callback for the progress window
        /// this method is called if the abort button in the progress window is called
        /// it stops the encoder cold
        /// </summary>
        private void pw_Abort()
        {
            UserRequestedAbort();
        }

        /// <summary>
        /// catches the ChangePriority event from the progresswindow and forward it to the encoder class
        /// </summary>
        /// <param name="priority"></param>
        private void pw_PriorityChanged(ProcessPriority priority)
        {
            try
            {
                currentProcessor.changePriority(priority);
            }
            catch (JobRunException e)
            {
                mainForm.addToLog("Error when attempting to change priority: " + e.Message);
            }
        }
        #endregion

        #region public interface
        public int Progress
        {
            get { return progress; }
        }

        private PauseState pauseStatus;
        public PauseState PauseStatus
        {
            get { return pauseStatus; }
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

        public void SetStopping()
        {
            Debug.Assert(status == JobWorkerStatus.Running);
            status = JobWorkerStatus.Stopping;
        }

        public void SetRunning()
        {
            Debug.Assert(status == JobWorkerStatus.Stopping);
            status = JobWorkerStatus.Running;
        }

        private string name;

        public string Name
        {
            get { return name; }
            set
            {
                mainForm.Jobs.RenameWorker(name, value); // throws NameTakenException if it fails
                name = value;
            }
        }

        public bool IsEncoding
        {
            get { return status == JobWorkerStatus.Running || status == JobWorkerStatus.Stopping; }
        }
        #endregion

        private Dictionary<string, Job> localJobs = new Dictionary<string, Job>();

        public JobWorker()
        {
            display = new JobWorkerWindow(this);
        }

        #region job run util

        #region delete intermediate files
        public static readonly JobPostProcessor DeleteIntermediateFilesPostProcessor = new JobPostProcessor(
            new Processor(deleteIntermediateFiles), "DeleteIntermediateFiles");

        /// <summary>
        /// Attempts to delete all files listed in job.FilesToDelete if settings.DeleteIntermediateFiles is checked
        /// </summary>
        /// <param name="job">the job which should just have been completed</param>
        private static void deleteIntermediateFiles(MainForm mainForm, Job job)
        {
            if (mainForm.Settings.DeleteIntermediateFiles)
            {
                mainForm.addToLog("Job completed successfully and deletion of intermediate files is activated\r\n");
                foreach (string file in job.FilesToDelete)
                {
                    mainForm.addToLog("Found intermediate output file '" + ((string)file)
                        + "', deleting...");
                    try
                    {
                        File.Delete(file);
                        mainForm.addToLog("Deletion succeeded.\r\n");
                    }
                    catch (IOException)
                    {
                        mainForm.addToLog("Deletion failed.\r\n");
                    }
                }
            }
        }
        #endregion


        /// <summary>
        /// Postprocesses the given job according to the JobPostProcessors in the mainForm's PackageSystem
        /// </summary>
        /// <param name="job"></param>
        private void postprocessJob(Job job)
        {
            mainForm.addToLog("Starting postprocessing of job...\r\n");
            foreach (JobPostProcessor pp in mainForm.PackageSystem.JobPostProcessors.Values)
            {
                pp.PostProcessor(mainForm, job);
            }
            mainForm.addToLog("Postprocessing finished!\r\n");
        }

        /// <summary>
        /// Preprocesses the given job according to the JobPreProcessors in the mainForm's PackageSystem
        /// </summary>
        /// <param name="job"></param>
        private void preprocessJob(Job job)
        {
            mainForm.addToLog("Starting preprocessing of job...\r\n");
            foreach (JobPreProcessor pp in mainForm.PackageSystem.JobPreProcessors.Values)
            {
                pp.PreProcessor(mainForm, job);
            }
            mainForm.addToLog("Preprocessing finished!\r\n");
        }

        private IJobProcessor getProcessor(Job job)
        {
            mainForm.addToLog("Looking for job processor for job...\r\n");
            foreach (JobProcessorFactory f in mainForm.PackageSystem.JobProcessors.Values)
            {
                IJobProcessor p = f.Factory(mainForm, job);
                if (p != null)
                {
                    mainForm.addToLog("Processor found!\r\n");
                    return p;
                }
            }
            mainForm.addToLog("No processor found!\r\n");
            return null;
        }
        #endregion

        internal void ShutDown(JobsOnQueue rest)
        {
            throw new Exception("The method or operation is not implemented.");
        }
        internal void UserRequestShutDown()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        internal void GUIDeleteJob(Job j)
        {
            mainForm.Jobs.DeleteJob(j);
//            mainForm.Jobs
        }

        #region job starting / stopping
        #region abort
        /// <summary>
        /// aborts the currently active job
        /// </summary>
        public void Abort()
        {
            Debug.Assert(IsEncoding);
            if (currentProcessor == null) return;
            try
            {
                currentProcessor.stop();
            }
            catch (JobRunException er)
            {
                mainForm.addToLog("Error when trying to stop processing: " + er.Message + "\r\n");
            }
            markJobAborted();
            status = JobWorkerStatus.Idle;
            refreshAll();
        }

        private void refreshAll()
        {
            if (display != null)
                display.RefreshInfo();
        }
        #endregion

        #region starting jobs
        public void StartEncoding(/*bool showMessageBoxes*/)
        {
            JobStartInfo retval = startNextJobInQueue();
            /*            if (showMessageBoxes)
                        {*/
            if (retval == JobStartInfo.COULDNT_START)
                MessageBox.Show("Couldn't start processing. Please consult the log for more details", "Processing failed", MessageBoxButtons.OK);
            else if (retval == JobStartInfo.NO_JOBS_WAITING)
                MessageBox.Show("No jobs are waiting. Nothing to do", "No jobs waiting", MessageBoxButtons.OK);
            //            }
        }
        
        /// <summary>
        /// Copies completion info into the job: end time, FPS, status.
        /// </summary>
        /// <param name="job">Job to fill with info</param>
        /// <param name="su">StatusUpdate with info</param>
        private void copyInfoIntoJob(Job job, StatusUpdate su)
        {
            Debug.Assert(su.IsComplete);

            job.End = DateTime.Now;
            job.EncodingSpeed = su.ProcessingSpeed;

            JobStatus s;
            if (su.WasAborted)
                s = JobStatus.ABORTED;
            else if (su.HasError)
                s = JobStatus.ERROR;
            else
                s = JobStatus.DONE;
            job.Status = s;
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
            {
                // so we don't lock up the GUI, we start a new thread
                new Thread(new ThreadStart(delegate
                {
                    Job job = mainForm.Jobs.ByName(su.JobName);

                    copyInfoIntoJob(job, su);
                    progress = 0;
                    ensureProgressWindowClosed();
                    currentProcessor = null;
                    currentJob = null;

                    // Logging
                    addToLog("Processing ended at " + DateTime.Now.ToLongTimeString() + "\r\n");
                    addToLog("----------------------" +
                        "\r\n\r\nLog for job " + su.JobName + "\r\n\r\n" + su.Log +
                        "\r\n----------------------\r\n");

                    // Postprocessing
                    bool jobCompletedSuccessfully = (job.Status == JobStatus.DONE);
                    if (jobCompletedSuccessfully)
                        postprocessJob(job);

                    if (jobCompletedSuccessfully && mainForm.Settings.DeleteCompletedJobs)
                        mainForm.Jobs.removeCompletedJob(job);

                    addToLog("End of log for " + job.Name + "\r\n" +
                        "-------------------------------------------------------\r\n\r\n");


                    if (job.Status == JobStatus.ABORTED)
                    {
                        addToLog("The current job was aborted. Stopping queue mode\r\n");
                        status = JobWorkerStatus.Idle;
                    }
                    else if (status == JobWorkerStatus.Stopping)
                    {
                        addToLog("Told to stop. Stopping queue mode.\r\n");
                        status = JobWorkerStatus.Idle;
                    }
                    else
                        startNextJobInQueue();

                    refreshAll();
                })).Start();
            }
            else // job is not complete yet
            {
                try
                {
                    if (pw.IsHandleCreated) // the window is there, send the update to the window
                    {
                        pw.Invoke(new UpdateStatusCallback(pw.UpdateStatus), new object[] { su });
                    }
                }
                catch (Exception e)
                {
                    mainForm.addToLog("Exception when trying to update status while a job is running. Text: " + e.Message + " stacktrace: " + e.StackTrace);
                }

                progress = su.PercentageDone;
                /*
                string percentage = (su.PercentageDoneExact ?? 0M).ToString("##.##");
                if (percentage.IndexOf(".") != -1 && percentage.Substring(percentage.IndexOf(".")).Length == 1)
                    percentage += "0";
                mainForm.TitleText = "MeGUI " + su.JobName + " " + percentage + "% ";
                if (mainForm.Settings.AfterEncoding == AfterEncoding.Shutdown)
                    mainForm.TitleText += "- SHUTDOWN after encode";
                this.jobProgress.Value = su.PercentageDone;*/
            }
        }

        private enum ExceptionType { UserSkip, Error };
        private class JobStartException : MeGUIException
        {
            public ExceptionType type;
            public JobStartException(string reason, ExceptionType type) : base(reason) { this.type = type; }
        }
        /// <summary>
        /// starts the job provided as parameters
        /// </summary>
        /// <param name="job">the Job object containing all the parameters</param>
        /// <returns>success / failure indicator</returns>
        private bool startEncoding(Job job)
        {
            Debug.Assert(status == JobWorkerStatus.Idle);

            try
            {
                //Check to see if output file already exists before encoding.
                if (File.Exists(job.Output) && !mainForm.DialogManager.overwriteJobOutput(job.Output))
                    throw new JobStartException("File exists and the user doesn't want to overwrite", ExceptionType.UserSkip);

                // Get IJobProcessor
                currentProcessor = getProcessor(job);
                if (currentProcessor == null)
                    throw new JobStartException("No processor could be found", ExceptionType.Error);

                addToLog("\r\n\r\n------------------------------------------------------\r\n\r\n");
                addToLog("Starting job " + job.Name + " at " + DateTime.Now.ToLongTimeString() + "\r\n");

                // Preprocess
                preprocessJob(job);

                // Setup
                try
                {
                    currentProcessor.setup(job);
                }
                catch (JobRunException e)
                {
                    throw new JobStartException("Calling setup of processor failed with error '" + e.Message + "'", ExceptionType.Error);
                }

                // Do JobControl setup
                addToLog("encoder commandline:\r\n" + job.Commandline + "\r\n");
                currentProcessor.StatusUpdate += new JobProcessingStatusUpdateCallback(UpdateGUIStatus);

                // Progress window
                pw = new ProgressWindow(job.JobType);
                pw.WindowClosed += new WindowClosedCallback(pw_WindowClosed);
                pw.Abort += new AbortCallback(pw_Abort);
                pw.setPriority(job.Priority);
                pw.PriorityChanged += new PriorityChangedCallback(pw_PriorityChanged);
                if (mainForm.Settings.OpenProgressWindow && mainForm.Visible)
                    pw.Show();

                job.Status = JobStatus.PROCESSING;
                job.Start = DateTime.Now;
                status = JobWorkerStatus.Running;
                pauseStatus = PauseState.Encoding;
                currentJob = job;

                // Start
                try
                {
                    currentProcessor.start();
                }
                catch (JobRunException e)
                {
                    throw new JobStartException("starting encoder failed with error '" + e.Message +"'", ExceptionType.Error);
                }

                addToLog("successfully started encoding\r\n");

                refreshAll();
                return true;
            }
            catch (JobStartException e)
            {
                addToLog("Job not started. Reason: " + e.Message + "\r\n");
                if (e.type == ExceptionType.Error)
                    job.Status = JobStatus.ERROR;
                else // ExceptionType.UserSkip
                    job.Status = JobStatus.SKIP;
                currentProcessor = null;
                currentJob = null;
                status = JobWorkerStatus.Idle;
                pauseStatus = PauseState.NotEncoding;
                refreshAll();
                return false;
            }

        }

        private void addToLog(string p)
        {
            mainForm.addToLog(p);
        }

        private Job getNextJob()
        {
            foreach (Job j in display.Jobs)
                if (j.Status == JobStatus.WAITING && mainForm.Jobs.areDependenciesMet(j))
                    return j;
            if (mode == JobWorkerMode.RequestNewJobs)
                return mainForm.Jobs.getJobToProcess();
            else
                return null;
        }

        private JobStartInfo startNextJobInQueue()
        {
            Job job = getNextJob();
            
            if (job == null) return JobStartInfo.NO_JOBS_WAITING;

            while (job != null)
            {
                if (startEncoding(job)) // successful
                    return JobStartInfo.JOB_STARTED;
                job = getNextJob();
            }
            return JobStartInfo.COULDNT_START;
        }
        #endregion
        #endregion

        /// <summary>
        /// marks job currently marked as processing as aborted
        /// </summary>
        private void markJobAborted()
        {
            Job job = currentJob;
            job.Status = JobStatus.ABORTED;
            job.End = DateTime.Now;
            if (mainForm.Settings.DeleteAbortedOutput)
            {
                mainForm.addToLog("Job aborted, deleting output file...");
                try
                {
                    File.Delete(job.Output);
                    mainForm.addToLog("Deletion successful.\r\n");
                }
                catch (Exception)
                {
                    mainForm.addToLog("Deletion failed.\r\n");
                }
            }
        }

        #region pause / resume
        public void Pause()
        {
            Debug.Assert(pauseStatus == PauseState.Encoding);
            try
            {
                currentProcessor.pause();
                pauseStatus = PauseState.Paused;
                refreshAll();
            }
            catch (JobRunException ex)
            {
                addToLog("Error when trying to pause encoding: " + ex.Message + Environment.NewLine);
            }
        }

        public void Resume()
        {
            Debug.Assert(pauseStatus == PauseState.Paused);
            try
            {
                currentProcessor.resume();
                pauseStatus = PauseState.Encoding;
            }
            catch (JobRunException ex)
            {
                addToLog("Error when trying to resume encoding: " + ex.Message + Environment.NewLine);
            }
        }
        #endregion

        /// <summary>
        /// Makes sure that the progress window is closed
        /// </summary>
        private void ensureProgressWindowClosed()
        {
            if (pw != null)
            {
                pw.IsUserAbort = false; // ensures that the window will be closed
                pw.Close();
                pw = null;
            }
        }


        internal void RemoveJobFromQueue(Job job)
        {
            localJobs.Remove(job.Name);
            display.
        }

        internal void UserRequestedAbort()
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }

    public enum PauseState { NotEncoding, Encoding, Paused }
    public enum JobWorkerMode { RequestNewJobs, CloseOnLocalListCompleted }
    public enum JobWorkerStatus { Idle, Running, Stopping}
    public enum JobsOnQueue { Delete, ReturnToMainQueue }
}
#endif