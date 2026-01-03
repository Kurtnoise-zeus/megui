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
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Threading;

using MeGUI.core.util;

namespace MeGUI
{
    public enum StreamType : Int32 { None = 0, Stderr = 1, Stdout = 2 }

    public abstract class CommandlineJobProcessor<TJob> : IJobProcessor
        where TJob : Job
    {
        #region variables
        private TJob job;
        private Process proc = new Process(); // the encoder process
        private string executable; // path and filename of the commandline encoder to be used
        private ManualResetEvent mre = new ManualResetEvent(true); // lock used to pause encoding
        private ManualResetEvent stdoutDone = new ManualResetEvent(false);
        private ManualResetEvent stderrDone = new ManualResetEvent(false);
        private StatusUpdate su;
#pragma warning disable CA1051 // Do not declare visible instance fields
        protected LogItem log;
#pragma warning restore CA1051 // Do not declare visible instance fields
        private LogItem stdoutLog;
        private LogItem stderrLog;
        private Thread readFromStdErrThread;
        private Thread readFromStdOutThread;
        private bool bFirstPass = true;
        private bool bSecondPassNeeded = false;
        private bool bWaitForExit = false;
        private bool bCommandLine = true;
        private List<int> arrSuccessCodes = new List<int>() { 0 };
        private int iMinimumChildProcessCount = 0;

        protected TJob Job { get => job; set => job = value; }
        protected Process Proc { get => proc; set => proc = value; }
        protected string Executable { get => executable; set => executable = value; }
        protected StatusUpdate Su { get => su; set => su = value; }
        protected LogItem StdoutLog { get => stdoutLog; set => stdoutLog = value; }
        protected bool BFirstPass { get => bFirstPass; set => bFirstPass = value; }
        protected bool BSecondPassNeeded { get => bSecondPassNeeded; set => bSecondPassNeeded = value; }
        protected bool BCommandLine { get => bCommandLine; set => bCommandLine = value; }
        protected List<int> ArrSuccessCodes { get => arrSuccessCodes; set => arrSuccessCodes = value; }
        protected int IMinimumChildProcessCount { get => iMinimumChildProcessCount; set => iMinimumChildProcessCount = value; }
        #endregion

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // dispose managed resources
                proc.Dispose();
                mre.Dispose();
                stdoutDone.Dispose();
                stderrDone.Dispose();
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
            Util.ensureExists(Job.Input);
        }

        protected virtual void doExitConfig()
        {
            if (Su.HasError || Su.WasAborted)
                return;
            
            if (String.IsNullOrEmpty(Job.Output))
                return;
            
            if (File.Exists(Job.Output))
            {
                using (MediaInfoFile oInfo = new MediaInfoFile(Job.Output, ref log))
                {
                                    }
                            }
            else if (Path.GetExtension(Job.Output).ToLowerInvariant().Equals(".avi"))
            {
                string strFile = Path.GetFileNameWithoutExtension(Job.Output) + " (1).avi";
                strFile = Path.Combine(Path.GetDirectoryName(Job.Output), strFile);
                if (File.Exists(strFile))
                {
                    using (MediaInfoFile oInfo = new MediaInfoFile(strFile, ref log))
                    {
                    }
                }
            }
        }

        // returns true if the exit code yields a meaningful answer
        protected virtual bool checkExitCode
        {
            get { return true; }
        }

        /// <summary>
        /// true if the process main window should be hidden
        /// </summary>
        protected virtual bool hideProcess
        {
            get { return false; }
        }

        /// <summary>
        /// true if a second run is needed
        /// </summary>
        /// <returns></returns>
        protected virtual bool secondRunNeeded()
        {
            if (BFirstPass && BSecondPassNeeded)
                return true;
            return false;
        }

        protected virtual void getErrorLine()
        {
            return;
        }

        protected abstract string Commandline
        {
            get;
        }

        /// <summary>
        /// handles the encoder process existing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void proc_Exited(object sender, EventArgs e)
        {
            mre.Set();  // Make sure nothing is waiting for pause to stop
            if (BCommandLine)
            {
                stdoutDone.WaitOne(); // wait for stdout to finish processing
                stderrDone.WaitOne(); // wait for stderr to finish processing
            }

            getErrorLine();

            // check the exitcode
            if (checkExitCode && !ArrSuccessCodes.Contains(Proc.ExitCode))
            {
                string strError = WindowUtil.GetErrorText(Proc.ExitCode);
                if (!Su.WasAborted)
                    {
                        Su.HasError = true;
                        log.LogEvent("Process exits with error: " + strError, ImageType.Error);
                    }
                else
                    log.LogEvent("Process exits with error: " + strError);
            }

            if (secondRunNeeded())
            {
                BFirstPass = false;
                Su.HasError = false;
                start();
            }
            else
            {
                Su.Status = "Finalizing...";
                Su.IsComplete = true;
                doExitConfig();
                StatusUpdate(Su);
            }

            bWaitForExit = false;
        }

        #region IVideoEncoder overridden Members

        public void setup(Job job2, StatusUpdate su, LogItem log)
        {
            Debug.Assert(job2 is TJob, "Job is the wrong type");

            this.log = log;
            TJob job = (TJob)job2;
            this.Job = job;
            
            if (!Executable.ToLowerInvariant().Equals("cmd.exe"))
            {
                // This enables relative paths, etc
                if (!File.Exists(Executable))
                    Executable = Path.Combine(System.Windows.Forms.Application.StartupPath, Executable);
                    Util.ensureExists(Executable);
            }
            
            this.Su = su;
            checkJobIO();
        }

        public void start()
        {
            Proc = new Process();
            ProcessStartInfo pstart = new ProcessStartInfo();
            pstart.FileName = Executable;
            pstart.Arguments = Commandline;
            if (BCommandLine)
            {
                pstart.RedirectStandardOutput = true;
                pstart.RedirectStandardError = true;
                pstart.UseShellExecute = false;
            }
            pstart.WindowStyle = ProcessWindowStyle.Minimized;
            pstart.CreateNoWindow = true;
            Proc.StartInfo = pstart;
            Proc.EnableRaisingEvents = true;
            Proc.Exited += new EventHandler(proc_Exited);
            bWaitForExit = false;
            log.LogValue("Job command line", '"' + pstart.FileName + "\" " + pstart.Arguments);

            try
            {
                Proc.Start();
                log.LogEvent("Process started");
                StdoutLog = log.Info(string.Format("[{0:G}] {1}", DateTime.Now, "Standard output stream"));
                stderrLog = log.Info(string.Format("[{0:G}] {1}", DateTime.Now, "Standard error stream"));
                if (BCommandLine)
                {
                    readFromStdErrThread = new Thread(new ThreadStart(readStdErr));
                    readFromStdOutThread = new Thread(new ThreadStart(readStdOut));
                    readFromStdOutThread.Start();
                    readFromStdErrThread.Start();
                }
                new System.Windows.Forms.MethodInvoker(this.RunStatusCycle).BeginInvoke(null, null);

                if (hideProcess)
                {
                    // try to hide the main window
                    while (!Proc.HasExited && !WindowUtil.GetIsWindowVisible(Proc.MainWindowHandle))
                        MeGUI.core.util.Util.Wait(100);
                    if (!Proc.HasExited)
                        WindowUtil.HideWindow(Proc.MainWindowHandle);
                }
                
                WorkerPriority.GetJobPriority(Job, out WorkerPriorityType oPriority, out bool lowIOPriority);
                this.changePriority(oPriority);
            }
            catch (Exception e)
            {
                throw new JobRunException(e);
            }
        }

        public void stop()
        {
            try
            {
                if (Proc == null || Proc.HasExited)
                    return;

                bWaitForExit = true;
                mre.Set(); // if it's paused, then unpause
                Su.WasAborted = true;
                if (Proc.StartInfo.FileName.ToLowerInvariant().Equals("cmd.exe"))
                {
                    foreach (Process oProc in OSInfo.GetChildProcesses(Proc))
                    {
                        if (!oProc.ProcessName.ToLowerInvariant().Equals("conhost"))
                            try { OSInfo.KillProcess(oProc); } catch { }
                    }
                }
                Proc.Kill();
                while (bWaitForExit) // wait until the process has terminated without locking the GUI
                    MeGUI.core.util.Util.Wait(100);
                Proc.WaitForExit();
                return;
            }
            catch (Exception e)
            {
                throw new JobRunException(e);
            }
        }

        public bool pause()
        {
            bool bResult = OSInfo.SuspendProcess(Proc);
            if (!mre.Reset() || !bResult)
                return false;
            return true;
        }

        public bool resume()
        {
            bool bResult = OSInfo.ResumeProcess(Proc);
            if (!mre.Set() || !bResult)
                return false;
            return true;
        }

        public bool isRunning()
        {
            return(Proc != null && !Proc.HasExited);
        }

        public void changePriority(WorkerPriorityType priority)
        {
            if (!isRunning())
                return;

            try
            {
                WorkerPriority.GetJobPriority(Job, out WorkerPriorityType oPriority, out bool lowIOPriority);
                OSInfo.SetProcessPriority(Proc, priority, lowIOPriority, IMinimumChildProcessCount);
                return;
            }
            catch (Exception e) // process could not be running anymore
            {
                throw new JobRunException(e);
            }
        }
        #endregion

        #region reading process output
        protected virtual void readStream(StreamReader sr, ManualResetEvent rEvent, StreamType str)
        {
            // Use a using statement to ensure the StreamReader is disposed after use
            using (sr)
            {
                string line;
                if (Proc != null && sr != null && rEvent != null)
                {
                    try
                    {
                        while ((line = sr.ReadLine()) != null)
                        {
                            mre.WaitOne();
                            ProcessLine(line, str, ImageType.Information);
                        }
                    }
                    catch (Exception e)
                    {
                        ProcessLine("Exception in readStream. Line cannot be processed. " + e.Message, str, ImageType.Error);
                    }
                    rEvent.Set();
                }
            }
        }

        protected void readStdOut()
        {
            StreamReader sr;
            try
            {
                sr = Proc.StandardOutput;
            }
            catch (Exception e)
            {
                log.LogValue("Exception getting IO reader for stdout", e, ImageType.Error);
                stdoutDone.Set();
                return;
            }
            readStream(sr, stdoutDone, StreamType.Stdout);
        }

        protected void readStdErr()
        {
            StreamReader sr;
            try
            {
                sr = Proc.StandardError;
            }
            catch (Exception e)
            {
                log.LogValue("Exception getting IO reader for stderr", e, ImageType.Error);
                stderrDone.Set();
                return;
            }
            readStream(sr, stderrDone, StreamType.Stderr);
        }

        public virtual void ProcessLine(string line, StreamType stream, ImageType oType)
        {
            if(String.IsNullOrEmpty(line) || String.IsNullOrEmpty(line.Trim()))
                return;

            byte[] bytes = System.Text.Encoding.GetEncoding(0).GetBytes(line);
            line = System.Text.Encoding.UTF8.GetString(bytes);

            if (stream == StreamType.Stdout)
                StdoutLog.LogEvent(line, oType);
            if (stream == StreamType.Stderr)
                stderrLog.LogEvent(line, oType);

            if (oType == ImageType.Error)
                Su.HasError = true;
        }

        #endregion
        #region status updates
        public event JobProcessingStatusUpdateCallback StatusUpdate;
        protected virtual void RunStatusCycle()
        { }

        protected virtual void doStatusCycleOverrides()
        { }
        #endregion
    }
}