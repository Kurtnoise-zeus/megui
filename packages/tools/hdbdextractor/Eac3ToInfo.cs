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
using System.ComponentModel;
using System.Diagnostics;
using System.Text.RegularExpressions;

using MeGUI.core.util;
using eac3to;

namespace MeGUI.packages.tools.hdbdextractor
{
    struct eac3toArgs
    {
        public string eac3toPath { get; set; }
        public string workingFolder { get; set; }
        public string featureNumber { get; set; }
        public string args { get; set; }
        public ResultState resultState { get; set; }
    }

    public enum ResultState
    {
        [StringValue("Feature Retrieval Completed")]
        FeatureCompleted,
        [StringValue("Stream Retrieval Completed")]
        StreamCompleted,
        [StringValue("Stream Extraction Completed")]
        ExtractCompleted
    }

    public delegate void OnFetchInformationCompletedHandler(Eac3toInfo sender, RunWorkerCompletedEventArgs args);
    public delegate void OnProgressChangedHandler(Eac3toInfo sender, ProgressChangedEventArgs args);

    public class Eac3toInfo
    {
        private BackgroundWorker backgroundWorker;
        private List<Feature> features;
        private OperatingMode oMode;
        private eac3toArgs args;
        private List<string> input;
        private LogItem _log;
        private MediaInfoFile iFile;
        private bool bFetchAll;
        private int iFeatureToFetch;
        private Stream lastStream;

        private enum OperatingMode
        {
            FileBased,
            FolderBased
        }

        public event OnFetchInformationCompletedHandler FetchInformationCompleted;
        public event OnProgressChangedHandler ProgressChanged;

        public Eac3toInfo(List<string> input, MediaInfoFile iFile, LogItem oLog)
        {
            // create log instance
            if (oLog == null)
            {
                _log = MainForm.Instance.Eac3toLog;
                if (_log == null)
                {
                    _log = MainForm.Instance.Log.Info("HD Streams Extractor");
                    MainForm.Instance.Eac3toLog = _log;
                }
            }
            else
                _log = oLog.Add(new LogItem("eac3toInfo"));

            foreach (string strPath in input)
                _log.LogEvent("Input: " + strPath);
            if (System.IO.Directory.Exists(input[0]))
                oMode = OperatingMode.FolderBased;
            else
                oMode = OperatingMode.FileBased;
            this.input = input;
            this.iFile = iFile;
            lastStream = null;
        }

        public bool IsBusy()
        {
            return backgroundWorker.IsBusy;
        }

        public List<Feature> Features
        {
            get { return features; }
        }

        private void initBackgroundWorker()
        {
            backgroundWorker = new BackgroundWorker();
            backgroundWorker.WorkerSupportsCancellation = true;
            backgroundWorker.WorkerReportsProgress = true;
            backgroundWorker.DoWork += new DoWorkEventHandler(backgroundWorker_DoWork);
            backgroundWorker.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker_ProgressChanged);
            backgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker_RunWorkerCompleted);
        }

        public void FetchAllInformation()
        {
            iFeatureToFetch = 0;
            bFetchAll = true;
            FetchFeatureInformation();
            while (bFetchAll)
                MeGUI.core.util.Util.Wait(100);
        }

        public void FetchFeatureInformation()
        {
            UpdateCacher.CheckPackage("eac3to");
            initBackgroundWorker();
            args = new eac3toArgs();
            args.eac3toPath = MainForm.Instance.Settings.Eac3to.Path;
            args.resultState = ResultState.FeatureCompleted;
            args.args = string.Empty;
            args.workingFolder = System.IO.Path.GetDirectoryName(MainForm.Instance.Settings.Eac3to.Path);
            features = new List<Feature>();
            backgroundWorker.ReportProgress(0, "Retrieving features...");
            backgroundWorker.RunWorkerAsync(args);
        }

        public void FetchStreamInformation(int iFeatureNumber)
        {
            UpdateCacher.CheckPackage("eac3to");
            initBackgroundWorker();
            args = new eac3toArgs();
            args.eac3toPath = MainForm.Instance.Settings.Eac3to.Path;
            args.resultState = ResultState.StreamCompleted;
            args.args = iFeatureNumber.ToString();
            args.featureNumber = iFeatureNumber.ToString();
            args.workingFolder = System.IO.Path.GetDirectoryName(MainForm.Instance.Settings.Eac3to.Path);
            backgroundWorker.ReportProgress(0, "Retrieving streams...");
            backgroundWorker.RunWorkerAsync(args);
        }

        #region backgroundWorker
        void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            eac3toArgs args = (eac3toArgs)e.Argument;

            using (Process compiler = new Process())
            {
                string strSource = string.Format("\"{0}\"", input[0]);
                for (int i = 1; i < input.Count; i++)
                    strSource += string.Format("+\"{0}\"", input[i]);

                compiler.StartInfo.FileName = args.eac3toPath;
                switch (args.resultState)
                {
                    case ResultState.FeatureCompleted:
                        compiler.StartInfo.Arguments = string.Format("{0}", strSource);
                        break;
                    case ResultState.StreamCompleted:
                        if (args.args == string.Empty)
                            compiler.StartInfo.Arguments = string.Format("{0}", strSource);
                        else
                            compiler.StartInfo.Arguments = string.Format("{0} {1}) {2}", strSource, args.args, "-progressnumbers");
                        break;
                    case ResultState.ExtractCompleted:
                        if (oMode == OperatingMode.FileBased)
                            compiler.StartInfo.Arguments = string.Format("{0} {1}", strSource, args.args + " -progressnumbers");
                        else
                            compiler.StartInfo.Arguments = string.Format("{0} {1}) {2}", strSource, args.featureNumber, args.args + "-progressnumbers");
                        break;
                }

                if (_log != null)
                    _log.LogEvent(string.Format("Arguments: {0}", compiler.StartInfo.Arguments));

                compiler.StartInfo.WorkingDirectory = args.workingFolder;
                compiler.StartInfo.CreateNoWindow = true;
                compiler.StartInfo.UseShellExecute = false;
                compiler.StartInfo.RedirectStandardOutput = true;
                compiler.StartInfo.RedirectStandardError = true;
                compiler.StartInfo.ErrorDialog = false;
                compiler.EnableRaisingEvents = true;

                compiler.EnableRaisingEvents = true;
                compiler.Exited += new EventHandler(backgroundWorker_Exited);
                compiler.ErrorDataReceived += new DataReceivedEventHandler(backgroundWorker_ErrorDataReceived);
                compiler.OutputDataReceived += new DataReceivedEventHandler(backgroundWorker_OutputDataReceived);

                try
                {
                    compiler.Start();
                    compiler.BeginErrorReadLine();
                    compiler.BeginOutputReadLine();

                    while (!compiler.HasExited)
                        if (backgroundWorker.CancellationPending)
                            compiler.Kill();
                    while (!compiler.HasExited) // wait until the process has terminated without locking the GUI
                        MeGUI.core.util.Util.Wait(100);
                    compiler.WaitForExit();
                }
                catch (Exception ex)
                {
                    if (_log != null)
                        _log.LogValue("Error running job", ex);
                }
                finally
                {
                    compiler.ErrorDataReceived -= new DataReceivedEventHandler(backgroundWorker_ErrorDataReceived);
                    compiler.OutputDataReceived -= new DataReceivedEventHandler(backgroundWorker_OutputDataReceived);
                }
            }

            e.Result = args.resultState;
        }

        void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (this.ProgressChanged != null)
                this.ProgressChanged.Invoke(this, e);
        }

        void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled && _log != null)
                    _log.Error("Work was cancelled");

            if (e.Error != null && _log != null) 
                    _log.LogValue("Error running job", e);

            if (e.Result != null)
            {
                if (_log != null)
                    _log.LogEvent(Extensions.GetStringValue(((ResultState)e.Result)));

                if (bFetchAll)
                {
                    if (features.Count > 0 && iFeatureToFetch < features.Count)
                    {
                        FetchStreamInformation(++iFeatureToFetch);
                        return;
                    }
                    else
                        bFetchAll = false;
                }
            }

            if (this.FetchInformationCompleted != null)
                this.FetchInformationCompleted.Invoke(this, e);
        }

        void backgroundWorker_Exited(object sender, EventArgs e)
        {
            //ResetCursor(Cursors.Default);
        }

        void backgroundWorker_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            string data;

            if (!String.IsNullOrEmpty(e.Data))
            {
                data = e.Data.TrimStart('\b').Trim();

                if (!string.IsNullOrEmpty(data) && _log != null)
                    _log.Error("Error: " + e.Data);
            }
        }

        void backgroundWorker_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (string.IsNullOrEmpty(e.Data))
                return;

            string data = e.Data.TrimStart('\b').Trim();

            if (string.IsNullOrEmpty(data))
                return;

            bool bStreamFound = false;

            // Analyzing
            // analyze: 100%
            if (Regex.IsMatch(data, "^analyze: [0-9]{1,3}%$", RegexOptions.Compiled))
            {
                if (backgroundWorker.IsBusy)
                    backgroundWorker.ReportProgress(int.Parse(Regex.Match(data, "[0-9]{1,3}").Value),
                        string.Format("Analyzing ({0}%)", int.Parse(Regex.Match(data, "[0-9]{1,3}").Value)));
                return;
            }

            // Feature line
            // 2) 00216.mpls, 0:50:19
            if (Regex.IsMatch(data, @"^[0-99]+\).+$", RegexOptions.Compiled))
            {
                if (_log != null)
                    _log.LogEvent(data);

                try
                {
                    features.Add(eac3to.Feature.Parse(data));
                }
                catch (Exception ex)
                {
                    if (_log != null)
                        _log.LogValue("Error receiving output data", ex);
                }
                return;
            }

            // Feature name
            // "Feature Name"
            else if (Regex.IsMatch(data, "^\".+\"$", RegexOptions.Compiled))
            {
                if (_log != null)
                    _log.LogEvent(data);

                if (oMode == OperatingMode.FileBased)
                    features[0].Streams[features[0].Streams.Count - 1].Name = Extensions.CapitalizeAll(data.Trim("\" .".ToCharArray()));
                else
                    features[features.Count - 1].Name = Extensions.CapitalizeAll(data.Trim("\" .".ToCharArray()));
                return;
            }

            // Stream line on feature listing
            // - h264/AVC, 1080p24 /1.001 (16:9)
            else if (Regex.IsMatch(data, "^-.+$", RegexOptions.Compiled))
            {
                if (_log != null)
                    _log.LogEvent(data);
                return;
            }

            // Playlist file listing
            // [99+100+101+102+103+104+105+106+114].m2ts (blueray playlist *.mpls)
            else if (Regex.IsMatch(data, @"^\[.+\].m2ts$", RegexOptions.Compiled))
            {
                if (_log != null)
                    _log.LogEvent(data);

                foreach (string file in Regex.Match(data, @"\[.+\]").Value.Trim("[]".ToCharArray()).Split("+".ToCharArray()))
                    features[features.Count - 1].Files.Add(new File(file.PadLeft(5,'0') + ".m2ts", features[features.Count - 1].Files.Count + 1));
                return;
            }

            // Stream listing feature header
            // M2TS, 1 video track, 6 audio tracks, 9 subtitle tracks, 1:53:06
            // EVO, 2 video tracks, 4 audio tracks, 8 subtitle tracks, 2:20:02
            else if (Regex.IsMatch(data, "^M2TS, .+$", RegexOptions.Compiled) ||
                        Regex.IsMatch(data, "^EVO, .+$", RegexOptions.Compiled) ||
                        Regex.IsMatch(data, "^TS, .+$", RegexOptions.Compiled) ||
                        Regex.IsMatch(data, "^VOB, .+$", RegexOptions.Compiled) ||
                        Regex.IsMatch(data, "^MKV, .+$", RegexOptions.Compiled) ||
                        Regex.IsMatch(data, "^MKA, .+$", RegexOptions.Compiled)
                        )
            {
                if (_log != null)
                    _log.LogEvent(data);
                return;
            }

            // Stream line
            // 8: AC3, English, 2.0 channels, 192kbps, 48khz, dialnorm: -27dB
            else if (Regex.IsMatch(data, "^[0-99]+:.+$", RegexOptions.Compiled))
            {
                if (_log != null)
                    _log.LogEvent(data);

                if (oMode == OperatingMode.FileBased)
                {
                    try
                    {
                        if (features.Count == 0)
                        {
                            Feature dummyFeature = new Feature();
                            for (int i = 0; i < input.Count; i++)
                            {
                                if (System.IO.File.Exists(input[i]) && iFile == null)
                                    iFile = new MediaInfoFile(input[i], ref _log);
                                if (iFile != null)
                                {
                                    dummyFeature.Duration += TimeSpan.FromSeconds(Math.Ceiling(iFile.VideoInfo.FrameCount / iFile.VideoInfo.FPS));
                                    iFile = null;
                                }
                                dummyFeature.Files.Add(new File(System.IO.Path.GetFileName(input[i]), i + 1));
                            }
                            dummyFeature.Name = System.IO.Path.GetFileName(input[0]);
                            dummyFeature.Description = dummyFeature.Name + ", " + dummyFeature.Duration.ToString();
                            features.Add(dummyFeature);
                        }
                        lastStream = eac3to.Stream.Parse(data, _log);
                        features[0].Streams.Add(lastStream);
                        bStreamFound = true;
                    }
                    catch (Exception ex)
                    {
                        if (_log != null)
                            _log.LogValue("Error receiving output data", ex);
                    }
                }
                else
                {
                    try
                    {
                        lastStream = Stream.Parse(data, _log);
                        features[Int32.Parse(args.featureNumber) - 1].Streams.Add(lastStream);
                        bStreamFound = true;
                    }
                    catch (Exception ex)
                    {
                        if (_log != null)
                            _log.LogValue("Error receiving output data", ex);
                    }
                }
                return;
            }

            // Information line
            // [a03] Creating file "audio.ac3"...
            else if (Regex.IsMatch(data, @"^\[.+\] .+\.{3}$", RegexOptions.Compiled))
            {
                if (_log != null)
                    _log.LogEvent(data);
                return;
            }

            else if (Regex.IsMatch(data, @"^\v .*...", RegexOptions.Compiled))
            {
                if (_log != null)
                    _log.LogEvent(data);
                return;
            }

            // Core/Embedded track information
            // (core: AC3, 5.1 channels, 512kbps, 48kHz)
            else if (Regex.IsMatch(data, @"(core: .*)", RegexOptions.Compiled))
            {
                if (_log != null)
                    _log.LogEvent(data);

                if (lastStream != null && lastStream is AudioStream)
                    ((AudioStream)lastStream).TypeCore = data.Substring(7, data.IndexOf(',') - 7);

                return;
            }

            // Core/Embedded track information
            // (embedded: AC3, 5.1 channels, 640kbps, 48kHz)
            else if (Regex.IsMatch(data, @"(embedded: .*)", RegexOptions.Compiled))
            {
                if (_log != null)
                    _log.LogEvent(data);

                if (lastStream != null && lastStream is AudioStream)
                    ((AudioStream)lastStream).TypeCore = data.Substring(11, data.IndexOf(',') - 11);

                return;
            }

            // Creating file
            // Creating file "C:\1_1_chapter.txt"...
            else if (Regex.IsMatch(data, "^Creating file \".+\"\\.{3}$", RegexOptions.Compiled))
            {
                if (_log != null)
                    _log.LogEvent(data);
                return;
            }

            // Processing
            // process: 100%
            else if (Regex.IsMatch(data, "^process: [0-9]{1,3}%$", RegexOptions.Compiled))
            {
                if (backgroundWorker.IsBusy)
                    backgroundWorker.ReportProgress(int.Parse(Regex.Match(data, "[0-9]{1,3}").Value),
                        string.Format("Processing ({0}%)", int.Parse(Regex.Match(data, "[0-9]{1,3}").Value)));
                return;
            }

            // Progress
            // progress: 100%
            else if (Regex.IsMatch(data, "^progress: [0-9]{1,3}%$", RegexOptions.Compiled))
            {
                if (backgroundWorker.IsBusy)
                    backgroundWorker.ReportProgress(int.Parse(Regex.Match(data, "[0-9]{1,3}").Value),
                        string.Format("Progress ({0}%)", int.Parse(Regex.Match(data, "[0-9]{1,3}").Value)));
                return;
            }

            // Done
            // Done.
            else if (data.Equals("Done."))
            {
                if (_log != null)
                    _log.LogEvent(data);
                return;
            }

            // unusual video framerate
            // v02 The video framerate is correct, but rather unusual.
            else if (data.Contains("The video framerate is correct, but rather unusual"))
            {
                if (_log != null)
                    _log.LogEvent(data);
                return;
            }

            #region Errors
            // Source file not found
            // Source file "x:\" not found.
            else if (Regex.IsMatch(data, "^Source file \".*\" not found.$", RegexOptions.Compiled))
            {
                if (_log != null)
                    _log.Error(data);
                return;
            }

            // Format of Source file not detected
            // The format of the source file could not be detected.
            else if (data.Equals("The format of the source file could not be detected."))
            {
                if (_log != null)
                    _log.Error(data);
                return;
            }

            // Audio conversion not supported
            // This audio conversion is not supported.
            else if (data.Equals("This audio conversion is not supported."))
            {
                if (_log != null)
                    _log.Error(data);
                return;
            }
            #endregion

            // Unknown line
            else
            {
                if (_log != null)
                    _log.Warn(string.Format("Unknown line: \"{0}\"", data));
            }

            if (!bStreamFound)
                lastStream = null;
        }
        #endregion
    }
}