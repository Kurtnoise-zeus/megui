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
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Messaging;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Windows.Forms;

using MeGUI.core.util;

namespace MeGUI
{
    public sealed class AviSynthAudioEncoder : IJobProcessor // : AudioEncoder
    {
        public static readonly JobProcessorFactory Factory = new JobProcessorFactory(new ProcessorFactory(init), "AviSynthAudioEncoder");

        public enum HeaderType : int
        {
            NONE = 0,
            WAV = 1,
            W64 = 2
        }
        private static IJobProcessor init(MainForm mf, Job j)
        {
            if (j is AudioJob &&
                (((j as AudioJob).Settings is MP3Settings) ||
                ((j as AudioJob).Settings is MP2Settings) ||
                ((j as AudioJob).Settings is AC3Settings) ||
                ((j as AudioJob).Settings is OggVorbisSettings) ||
                ((j as AudioJob).Settings is NeroAACSettings) ||
                ((j as AudioJob).Settings is FlacSettings) ||
                ((j as AudioJob).Settings is QaacSettings) ||
                ((j as AudioJob).Settings is OpusSettings) ||
                ((j as AudioJob).Settings is FDKAACSettings) ||
                ((j as AudioJob).Settings is FFAACSettings) ||
                ((j as AudioJob).Settings is ExhaleSettings)))
                return new AviSynthAudioEncoder(mf.Settings);
            return null;
        }

        #region fields
        private Process _encoderProcess;
        private string _avisynthAudioScript;
        private string _encoderExecutablePath;
        private string _encoderCommandLine;
        private HeaderType _sendWavHeaderToEncoderStdIn;

        private int _sampleRate;

        private ManualResetEvent _mre = new System.Threading.ManualResetEvent(true); // lock used to pause encoding
        private Thread _encoderThread = null;

        private ManualResetEvent stdoutDone = new ManualResetEvent(false);
        private ManualResetEvent stderrDone = new ManualResetEvent(false);
        private Thread _readFromStdOutThread = null;
        private Thread _readFromStdErrThread = null;
        private LogItem stdoutLog;
        private LogItem stderrLog;
        private LogItem _log;
        private string _encoderStdErr;
        private static readonly System.Text.RegularExpressions.Regex _cleanUpStringRegex = new System.Text.RegularExpressions.Regex(@"\n[^\n]+\r", System.Text.RegularExpressions.RegexOptions.Compiled | System.Text.RegularExpressions.RegexOptions.CultureInvariant);

        private MeGUISettings _settings = null;
        private AudioJob audioJob;
        private StatusUpdate su;

        private List<string> _tempFiles = new List<string>();
        private readonly string _uniqueId = Guid.NewGuid().ToString("N");
        private Stopwatch _oLastUpdate = new Stopwatch();
        #endregion

        public AviSynthAudioEncoder(MeGUISettings settings)
        {
            _settings = settings;
        }

        protected void Dispose(bool disposing)
        {
            if (disposing)
            {
                // dispose managed resources
                _encoderProcess.Dispose();
                _mre.Dispose();
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
#region methods

        private void WriteTempTextFile(string filePath, string text)
        {
            using (Stream temp = new FileStream(filePath, System.IO.FileMode.Create))
            {
                using (TextWriter avswr = new StreamWriter(temp, System.Text.Encoding.Default))
                {
                    avswr.WriteLine(text);
                }
            }
            _tempFiles.Add(filePath);
        }

        private void DeleteTempFiles()
        {
            foreach (string filePath in _tempFiles)
                SafeDelete(filePath);
        }

        private static void SafeDelete(string filePath)
        {
            try
            {
                File.Delete(filePath);
            }
            catch
            {
                // Do Nothing
            }
        }

        private void CreateTemporallyEqFiles(string tempPath)
        {
            // http://forum.doom9.org/showthread.php?p=778156#post778156
            WriteTempTextFile(tempPath + "front.feq", @"-96
-96
-96
-4
-4
-4
-4
-4
-4
-4
-4
-4
-4
-4
-4
-96
-96
-96
");
            WriteTempTextFile(tempPath + "center.feq", @"-96
-96
-96
-96
-96
-96
3
3
3
3
3
3
3
3
3
3
3
3
");
            WriteTempTextFile(tempPath + "lfe.feq", @"0
0
0
-96
-96
-96
-96
-96
-96
-96
-96
-96
-96
-96
-96
-96
-96
-96
");
            WriteTempTextFile(tempPath + "back.feq", @"-96
-96
-96
-6
-6
-6
-6
-6
-6
-6
-6
-6
-6
-6
-6
-96
-96
-96
");
        }

        private static string GetChannelLayoutFromMask(int mask)
        {
            Dictionary<int, string> ChannelMask = new Dictionary<int, string>
            {
                { 4,      "Mono (FC)" },
                { 12,     "1.1 (FC/LFE)" },
                { 3,      "Stereo (FL/FR)" },
                { 11,     "2.1 (FL/FR/LFE)" },
                { 7,      "3.0 (FL/FR/FC)" },
                { 259,    "3.0 (back) (FL/FR/BC)" },
                { 15,     "3.1 (FL/FR/FC/LFE)" },
                { 263,    "4.0 (FL/FR/FC/BC)" },
                { 51,     "Quad (FL/FR/BL/BR)" },
                { 1539,   "Quad (side) (FL/FR/SL/SR)" },
                { 59,     "Quad.1 (FL/FR/LFE/BL/BR)" },
                { 55,     "5.0 (FL/FR/FC/BL/BR)" },
                { 1543,   "5.0 (side) (FL/FR/FC/BL/BR)" },
                { 271,    "4.1 (FL/FR/FC/LFE/BC)" },
                { 63,     "5.1 (FL/FR/FC/LFE//BL/BR)" },
                { 1551,   "5.1 (side) (FL/FR/FC/LFE/SL/SR)" },
                { 1799,   "6.0 (FL/FR/FC/BC/SL/SR)" },
                { 1731,   "6.0 (front) (FL/FR/FLC/FRC/SL/SR)" },
                { 311,    "Hexagonal (FL/FR/FC/BL/BR/BC)" },
                { 1807,   "6.1 (FL/FR/FC/LFE/BC/SL/SR)" },
                { 319,    "6.1 (back) (FL/FR/FC/LFE/BL/BR/BC)" },
                { 1735,   "6.1 (front) (FL/FR/LFE/FLC/FRC/SL/SR)" },
                { 1599,   "7.1 (FL/FR/FC/LFE/BL/BR/SL/SR)" },
                { 255,    "7.1 (wide) (FL/FR/FC/LFE/BL/BR/FLC/FRC)" },
                { 1743,   "7.1 (wide-side) (FL/FR/FC/LFE/FLC/FRC/SL/SR)" },
                { 1847,   "Octagonal (FL/FR/FC/bl:br:BC/SL/SR)" },
                { 184371, "Cube (FL/FR/BL/BR/TFL/TFR/TBL/TBR)" },
                { 20543,  "7.1 (top) (FL/FR/FC/LFE/BL/BR/TFL/TFR)" },
                { 22031,  "5.1.2 (front) (FL/FR/FC/LFE/SL/SR/TFL/TFR)" },
                { 165391, "5.1.2 (side) (FL/FR/FC/LFE/SL/SR/TBL/TBR)" }
            };



            ChannelMask.TryGetValue(mask, out string strLayout);
            if (String.IsNullOrEmpty(strLayout))
                strLayout = "LAYOUT_UNKNOWN";
            return strLayout;
        }

        private bool bShowError = false;
        private void RaiseEvent()
        {
            if (su.IsComplete || su.WasAborted || su.HasError)
            {
                _mre.Set();  // Make sure nothing is waiting for pause to stop
                if (_encoderProcess != null)
                {
                    stdoutDone.WaitOne(); // wait for stdout to finish processing
                    stderrDone.WaitOne(); // wait for stderr to finish processing
                }

                if (!su.HasError && !su.WasAborted)
                {
                    if (!String.IsNullOrEmpty(audioJob.Output) && File.Exists(audioJob.Output))
                    {
                        MediaInfoFile oInfo = new MediaInfoFile(audioJob.Output, ref _log);
                        oInfo.Dispose();
                    }
                }
                else if (su.HasError && !bShowError && audioJob.Settings is QaacSettings && _encoderStdErr != null && _encoderStdErr.ToLowerInvariant().Contains("coreaudiotoolbox.dll"))
                {
                    bShowError = true;
                    _log.LogEvent("CoreAudioToolbox.dll is missing and must be installed. Please have a look at the install.txt in the qaac folder.", ImageType.Error);
                    string installFile = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"tools\qaac\install.txt");
                    if (File.Exists(installFile))
                    {
                        if (MessageBox.Show("CoreAudioToolbox.dll is missing and must be installed.\r\nOtherwise QAAC cannot be used.\r\n\r\nDo you want to open the installation instructions?", "CoreAudioToolbox.dll missing", MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.Yes)
                            System.Diagnostics.Process.Start(installFile);
                    }
                    else
                        MessageBox.Show("CoreAudioToolbox.dll is missing and must be installed.\r\nOtherwise QAAC cannot be used", "CoreAudioToolbox.dll missing", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                su.IsComplete = true;
            }
            StatusUpdate?.Invoke(su);
        }

        private void SetProgress(decimal n)
        {
            su.PercentageCurrent = n * 100M;
        }

        private void RaiseEvent(string s)
        {
            su.Status = s;
            RaiseEvent();
        }

        private void ReadStdOut()
        {
            StreamReader sr;
            try
            {
                sr = _encoderProcess.StandardOutput;
            }
            catch (Exception e)
            {
                _log.LogValue("Exception getting IO reader for stdout", e, ImageType.Error);
                stdoutDone.Set();
                return;
            }
            ReadStream(sr, stdoutDone, StreamType.Stdout);
        }

        private void ReadStdErr()
        {
            StreamReader sr;
            try
            {
                sr = _encoderProcess.StandardError;
            }
            catch (Exception e)
            {
                _log.LogValue("Exception getting IO reader for stderr", e, ImageType.Error);
                stderrDone.Set();
                return;
            }
            ReadStream(sr, stderrDone, StreamType.Stderr);
        }

        private void ReadStream(StreamReader sr, ManualResetEvent rEvent, StreamType str)
        {
            string line;
            if (_encoderProcess != null)
            {
                try
                {
                    while ((line = sr.ReadLine()) != null)
                    {
                        _mre.WaitOne();
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

        private void ProcessLine(string line, StreamType stream, ImageType oType)
        {
            line = _cleanUpStringRegex.Replace(line.Replace(Environment.NewLine, "\n"), Environment.NewLine);
            if (String.IsNullOrEmpty(line.Trim()))
                return;

            if (audioJob.Settings is QaacSettings)
            {
                if (System.Text.RegularExpressions.Regex.IsMatch(line, @"^[0-9]*:"))
                    return;
                if (line.ToLowerInvariant().StartsWith("error:"))
                    oType = ImageType.Error;
            }
            else if (audioJob.Settings is OggVorbisSettings)
            {
                if (line.ToLowerInvariant().StartsWith("\tencoding ["))
                    return;
            }
            else if (audioJob.Settings is OpusSettings)
            {
                if (System.Text.RegularExpressions.Regex.IsMatch(line.ToLowerInvariant(), @"[|\/-]"))
                    return;
            }
            else if (audioJob.Settings is NeroAACSettings)
            {
                if (System.Text.RegularExpressions.Regex.IsMatch(line.ToLowerInvariant(), @"^processed\s?[0-9]{0,5}\s?seconds..."))
                    return;
            }
            else if (audioJob.Settings is AC3Settings || audioJob.Settings is MP2Settings || audioJob.Settings is FFAACSettings)
            {
                if (System.Text.RegularExpressions.Regex.IsMatch(line.ToLowerInvariant(), @"^size= "))
                    return;
            }
            else if (audioJob.Settings is FDKAACSettings)
            {
                if (System.Text.RegularExpressions.Regex.IsMatch(line, @"^[0-9%]*"))
                    return;
            }

            byte[] bytes = System.Text.Encoding.GetEncoding(0).GetBytes(line);
            line = System.Text.Encoding.UTF8.GetString(bytes);

            if (stream == StreamType.Stderr)
                _encoderStdErr += line + "\n";

            if (stream == StreamType.Stdout)
                stdoutLog.LogEvent(line, oType);
            if (stream == StreamType.Stderr)
                stderrLog.LogEvent(line, oType);

            if (oType == ImageType.Error)
                su.HasError = true;
        }

        private void Encode()
        {
            string fileErr = MainForm.verifyInputFile(this.audioJob.Input);
            if (fileErr != null)
            {
                _log.LogEvent("Problem with audio input file: " + fileErr, ImageType.Error);
                su.HasError = su.IsComplete = true;
                RaiseEvent();
                return;
            }

            Thread t = null;
            try
            {
                RaiseEvent("Opening file....please wait, it may take some time");
                if (CreateAviSynthScript(out string strAVSError))
                {
                    RaiseEvent("Preprocessing...please wait, it may take some time");
                    _log.LogEvent("AviSynth script environment opened");
                    using (AviSynthClip a = AviSynthScriptEnvironment.ParseScript(_avisynthAudioScript))
                    {
                        _log.LogEvent("Script loaded");
                        if (0 == a.ChannelsCount)
                            throw new ApplicationException("Can't find audio stream");
                        
                        LogItem inputLog = _log.LogEvent("Output Decoder", ImageType.Information);
                        inputLog.LogValue("Channels", a.ChannelsCount);
                        inputLog.LogValue("Bits per sample", a.BitsPerSample);
                        inputLog.LogValue("Sample rate", a.AudioSampleRate);
                        inputLog.LogValue("Channel Mask", a.ChannelMask);
                        inputLog.LogValue("Channel Layout", GetChannelLayoutFromMask(a.ChannelMask));
                        
                        const int MAX_SAMPLES_PER_ONCE = 4096;
                        int frameSample = 0;
                        int frameBufferTotalSize = MAX_SAMPLES_PER_ONCE * a.ChannelsCount * a.BytesPerSample;
                        byte[] frameBuffer = new byte[frameBufferTotalSize];
                        CreateEncoderProcess(a);
                        try
                        {
                            using (Stream target = _encoderProcess.StandardInput.BaseStream)
                            {
                                // let's write WAV Header
                                WriteHeader(target, a, _sendWavHeaderToEncoderStdIn, a.ChannelMask);
                                _sampleRate = a.AudioSampleRate;
                                GCHandle h = GCHandle.Alloc(frameBuffer, GCHandleType.Pinned);
                                IntPtr address = h.AddrOfPinnedObject();
                                try
                                {
                                    su.ClipLength = TimeSpan.FromSeconds((double)a.SamplesCount / (double)_sampleRate);
                                    RaiseEvent("Encoding audio...");
                                    _oLastUpdate.Restart();
                                    while (frameSample < a.SamplesCount)
                                    {
                                        su.ClipLength = TimeSpan.FromSeconds((double)a.SamplesCount / (double)_sampleRate);
                                        while (frameSample < a.SamplesCount)
                                        {
                                            _mre.WaitOne();

                                            if (_encoderProcess != null)
                                            {
                                                if (_encoderProcess.HasExited)
                                                {
                                                    string strError = WindowUtil.GetErrorText(_encoderProcess.ExitCode);
                                                    throw new ApplicationException("Abnormal encoder termination. Exit code: " + strError);
                                                }

                                            }
                                            int nHowMany = Math.Min((int)(a.SamplesCount - frameSample), MAX_SAMPLES_PER_ONCE);

                                            a.ReadAudio(address, frameSample, nHowMany);

                                            target.Write(frameBuffer, 0, nHowMany * a.ChannelsCount * a.BytesPerSample);
                                            target.Flush();
                                            frameSample += nHowMany;
                                            if (_oLastUpdate.Elapsed.TotalSeconds > 1)
                                            {
                                                SetProgress((decimal)frameSample / (decimal)a.SamplesCount);
                                                _oLastUpdate.Restart();
                                            }
                                        }
                                    }
                                }
                                finally
                                {
                                    h.Free();
                                }
                                
                                SetProgress(1M);
                                if (_sendWavHeaderToEncoderStdIn != HeaderType.NONE && a.BytesPerSample % 2 == 1)
                                    target.WriteByte(0);
                                }
                                
                                RaiseEvent("Finalizing encoder");
                                while (!_encoderProcess.HasExited) // wait until the process has terminated without locking the GUI
                                    MeGUI.core.util.Util.Wait(100);
                                _encoderProcess.WaitForExit();
                                _readFromStdErrThread.Join();
                                _readFromStdOutThread.Join();
                                if (0 != _encoderProcess.ExitCode)
                                {
                                    string strError = WindowUtil.GetErrorText(_encoderProcess.ExitCode);
                                    throw new ApplicationException("Abnormal encoder termination. Exit code: " + strError);
                                }
                            }
                         finally
                        {
                            if (!_encoderProcess.HasExited)
                            { 
                                _encoderProcess.Kill();
                                while (!_encoderProcess.HasExited) // wait until the process has terminated without locking the GUI
                                    MeGUI.core.util.Util.Wait(100);
                                _encoderProcess.WaitForExit();
                                _readFromStdErrThread.Join();
                                _readFromStdOutThread.Join();
                            }
                            _readFromStdErrThread = null;
                            _readFromStdOutThread = null;
                            
                            if (!String.IsNullOrEmpty(audioJob.Output) && File.Exists(audioJob.Output) && (new System.IO.FileInfo(audioJob.Output).Length) == 0)
                                _log.Error("Output file is empty, nothing was encoded");
                        }
                    }
                }
                else
                {
                    _log.LogEvent(strAVSError, ImageType.Error);
                    su.HasError = true;
                }
            }
            catch (Exception e)
            {
                _oLastUpdate.Stop();
                DeleteOutputFile();
                if (e is ThreadAbortException)
                {
                    _log.LogEvent("Aborting...");
                    su.WasAborted = true;
                    RaiseEvent();
                    return;
                }
                else if (e is AviSynthException)
                {
                    stderrDone.Set();
                    stdoutDone.Set();
                    _log.LogValue("An error occurred", e.Message, ImageType.Error);
                    su.HasError = true;
                }
                else if (e is ApplicationException)
                {
                    _log.LogValue("An error occurred", e.Message, ImageType.Error);
                    su.HasError = true;
                }
                else
                {
                    stderrDone.Set();
                    stdoutDone.Set();
                    _log.LogValue("An error occurred", e, ImageType.Error);
                    su.HasError = true;
                    RaiseEvent();
                    return;
                }
            }
            finally
            {
                _oLastUpdate.Stop();
                DeleteTempFiles();
            }
            su.IsComplete = true;
            RaiseEvent();
        }

        private void DeleteOutputFile()
        {
            SafeDelete(audioJob.Output);
        }

        private void CreateEncoderProcess(AviSynthClip a)
        {
            try
            {
                _encoderProcess = new Process();
                ProcessStartInfo info = new ProcessStartInfo();
                // Command line arguments, to be passed to encoder
                // {0} means output file name
                // {1} means samplerate in Hz
                // {2} means bits per sample
                // {3} means channel count
                // {4} means samplecount
                // {5} means size in bytes
                info.Arguments = string.Format(new System.Globalization.CultureInfo("en-US"), _encoderCommandLine,
                    audioJob.Output, a.AudioSampleRate, a.BitsPerSample, a.ChannelsCount, a.SamplesCount, a.AudioSizeInBytes);
                info.FileName = _encoderExecutablePath;
                _log.LogValue("Job command line", _encoderExecutablePath + " " + info.Arguments);
                info.UseShellExecute = false;
                info.RedirectStandardInput = true;
                info.RedirectStandardOutput = true;
                info.RedirectStandardError = true;
                info.CreateNoWindow = true;
                _encoderProcess.StartInfo = info;
                _encoderProcess.Start();

                // Take priority from AviSynth thread rather than default in settings
                // just in case user has managed to change job setting before getting here.
                if (_encoderThread.Priority == ThreadPriority.BelowNormal)
                    this.changePriority(WorkerPriorityType.BELOW_NORMAL);
                else if (_encoderThread.Priority == ThreadPriority.Normal)
                    this.changePriority(WorkerPriorityType.NORMAL);
                else if (_encoderThread.Priority == ThreadPriority.AboveNormal)
                    this.changePriority(WorkerPriorityType.ABOVE_NORMAL);
                else
                    this.changePriority(WorkerPriorityType.IDLE);

                _log.LogEvent("Process started");
                stdoutLog = _log.Info(string.Format("[{0:G}] {1}", DateTime.Now, "Standard output stream"));
                stderrLog = _log.Info(string.Format("[{0:G}] {1}", DateTime.Now, "Standard error stream"));
                _readFromStdOutThread = new Thread(new ThreadStart(ReadStdOut));
                _readFromStdErrThread = new Thread(new ThreadStart(ReadStdErr));
                _readFromStdOutThread.Start();
                _readFromStdErrThread.Start();
            }
            catch (Exception e)
            {
                throw new ApplicationException("Can't start encoder: " + e.Message, e);
            }
        }

        private static void WriteHeader(Stream target, AviSynthClip a, HeaderType headerType, int iChannelMask)
        {
            // https://github.com/jones1913/BeHappy

            if (headerType == HeaderType.NONE)
                return;

            const uint FAAD_MAGIC_VALUE = 0xFFFFFF00;
            bool Greater4GB = a.AudioSizeInBytes >= (uint.MaxValue - 68);
            bool WExtHeader = iChannelMask >= 0;
            uint HeaderSize = (uint)(WExtHeader ? 60 : 36);
            int[] defmask = { 0, 4, 3, 11, 263, 1543, 1551, 1807, 1599, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            if (Greater4GB || headerType == HeaderType.W64)
            {
                // W64
                HeaderSize = (uint)(WExtHeader ? 128 : 112);
                target.Write(System.Text.Encoding.ASCII.GetBytes("riff"), 0, 4);
                target.Write(BitConverter.GetBytes((uint)0x11CF912E), 0, 4);  // GUID
                target.Write(BitConverter.GetBytes((uint)0xDB28D6A5), 0, 4);
                target.Write(BitConverter.GetBytes((uint)0x0000C104), 0, 4);
                target.Write(BitConverter.GetBytes((a.AudioSizeInBytes + HeaderSize)), 0, 8);
                target.Write(System.Text.Encoding.ASCII.GetBytes("wave"), 0, 4);
                target.Write(BitConverter.GetBytes((uint)0x11D3ACF3), 0, 4);  // GUID
                target.Write(BitConverter.GetBytes((uint)0xC000D18C), 0, 4);
                target.Write(BitConverter.GetBytes((uint)0x8ADB8E4F), 0, 4);
                target.Write(System.Text.Encoding.ASCII.GetBytes("fmt "), 0, 4);
                target.Write(BitConverter.GetBytes((uint)0x11D3ACF3), 0, 4);  // GUID
                target.Write(BitConverter.GetBytes((uint)0xC000D18C), 0, 4);
                target.Write(BitConverter.GetBytes((uint)0x8ADB8E4F), 0, 4);
                target.Write(BitConverter.GetBytes(WExtHeader ? (ulong)64 : (ulong)48), 0, 8);
            }
            else
            {
                //WAV
                target.Write(System.Text.Encoding.ASCII.GetBytes("RIFF"), 0, 4);
                target.Write(BitConverter.GetBytes(Greater4GB ? (FAAD_MAGIC_VALUE + HeaderSize) : (uint)(a.AudioSizeInBytes + HeaderSize)), 0, 4);
                target.Write(System.Text.Encoding.ASCII.GetBytes("WAVE"), 0, 4);
                target.Write(System.Text.Encoding.ASCII.GetBytes("fmt "), 0, 4);
                target.Write(BitConverter.GetBytes(WExtHeader ? (uint)40 : (uint)16), 0, 4);
            }
            // fmt chunk common
            target.Write(BitConverter.GetBytes(WExtHeader ? (uint)0xFFFE : (uint)((a.SampleType == AudioSampleType.FLOAT) ? 3 : 1)), 0, 2);
            target.Write(BitConverter.GetBytes(a.ChannelsCount), 0, 2);
            target.Write(BitConverter.GetBytes(a.AudioSampleRate), 0, 4);
            target.Write(BitConverter.GetBytes(a.BitsPerSample * a.AudioSampleRate * a.ChannelsCount / 8), 0, 4);
            target.Write(BitConverter.GetBytes(a.ChannelsCount * a.BitsPerSample / 8), 0, 2);
            target.Write(BitConverter.GetBytes(a.BitsPerSample), 0, 2);
            // if WAVE_FORMAT_EXTENSIBLE continue fmt chunk
            if (WExtHeader)
            {
                if (iChannelMask == 0)
                    iChannelMask = defmask[a.ChannelsCount];
                target.Write(BitConverter.GetBytes((uint)0x16), 0, 2);
                target.Write(BitConverter.GetBytes(a.BitsPerSample), 0, 2);
                target.Write(BitConverter.GetBytes(iChannelMask), 0, 4);
                target.Write(BitConverter.GetBytes(((a.SampleType == AudioSampleType.FLOAT) ? 3 : 1)), 0, 4);
                target.Write(BitConverter.GetBytes((uint)0x00100000), 0, 4); // GUID
                target.Write(BitConverter.GetBytes((uint)0xAA000080), 0, 4);
                target.Write(BitConverter.GetBytes((uint)0x719B3800), 0, 4);
            }
            // data chunk
            if (Greater4GB || headerType == HeaderType.W64)
            {
                // W64
                if (!WExtHeader)
                {
                    target.Write(BitConverter.GetBytes((uint)0x0000D000), 0, 4); // pad
                    target.Write(BitConverter.GetBytes((uint)0x0000D000), 0, 4);
                }
                target.Write(System.Text.Encoding.ASCII.GetBytes("data"), 0, 4);
                target.Write(BitConverter.GetBytes((uint)0x11D3ACF3), 0, 4);  // GUID
                target.Write(BitConverter.GetBytes((uint)0xC000D18C), 0, 4);
                target.Write(BitConverter.GetBytes((uint)0x8ADB8E4F), 0, 4);
                target.Write(BitConverter.GetBytes(a.AudioSizeInBytes + 24), 0, 8);
            }
            else
            {
                // WAV
                target.Write(System.Text.Encoding.ASCII.GetBytes("data"), 0, 4);
                target.Write(BitConverter.GetBytes(Greater4GB ? FAAD_MAGIC_VALUE : (uint)a.AudioSizeInBytes), 0, 4);
            }
        }

        internal void Start()
        {
            Util.ensureExists(audioJob.Input);
            _encoderThread = new Thread(new ThreadStart(this.Encode));
            WorkerPriority.GetJobPriority(audioJob, out WorkerPriorityType oPriority, out bool lowIOPriority);
            _encoderThread.Priority = OSInfo.GetThreadPriority(oPriority);
            _encoderThread.Start();
        }

        internal void Abort()
        {
            _encoderThread.Abort();
            while (_encoderThread.IsAlive)
                MeGUI.core.util.Util.Wait(100);
            _encoderThread = null;
            if (_encoderProcess == null)
            {
                DeleteTempFiles();
                su.WasAborted = true;
            }
        }

        private bool OpenSourceWithFFAudioSource(out StringBuilder sbOpen)
        {
            bool applyDRC = audioJob.Settings.ApplyDRC ? true : false;
            sbOpen = new StringBuilder();
            sbOpen.Append(VideoUtil.getFFMSAudioInputLine(audioJob.Input, null, -1, applyDRC));

            _log.LogEvent("Trying to open the file with FFAudioSource()", ImageType.Information);
            if (AudioUtil.AVSScriptHasAudio(sbOpen.ToString(), out string strErrorText))
            {
                _log.LogEvent("Successfully opened the file with FFAudioSource()", ImageType.Information);
                audioJob.FilesToDelete.Add(audioJob.Input + ".ffindex");
                return true;
            }
            sbOpen = new StringBuilder();
            FileUtil.DeleteFile(audioJob.Input + ".ffindex", _log);
            if (String.IsNullOrEmpty(strErrorText))
                _log.LogEvent("Failed opening the file with FFAudioSource()", ImageType.Information);
            else
                _log.LogEvent("Failed opening the file with FFAudioSource(). " + strErrorText, ImageType.Information);
            return false;
        }

        private bool OpenSourceWithLSMASHAudioSource(out StringBuilder sbOpen)
        {
            sbOpen = new StringBuilder();
            bool applyDRC = audioJob.Settings.ApplyDRC ? true : false;
            sbOpen.Append(VideoUtil.getLSMASHAudioInputLine(audioJob.Input, null, -1, applyDRC));

            _log.LogEvent("Trying to open the file with LWLibavAudioSource()", ImageType.Information);
            if (AudioUtil.AVSScriptHasAudio(sbOpen.ToString(), out string strErrorText))
            {
                _log.LogEvent("Successfully opened the file with LWLibavAudioSource()", ImageType.Information);
                audioJob.FilesToDelete.Add(audioJob.Input + ".lwi");
                return true;
            }

            sbOpen = new StringBuilder();
            FileUtil.DeleteFile(audioJob.Input + ".lwi", _log);
            if (String.IsNullOrEmpty(strErrorText))
                _log.LogEvent("Failed opening the file with LWLibavAudioSource()", ImageType.Information);
            else
                _log.LogEvent("Failed opening the file with LWLibavAudioSource(). " + strErrorText, ImageType.Information);
            return false;
        }

        private bool OpenSourceWithBestAudioSource(out StringBuilder sbOpen)
        {
            sbOpen = new StringBuilder();
            bool applyDRC = audioJob.Settings.ApplyDRC ? true : false;
            sbOpen.Append(VideoUtil.getBestAudioInputLine(audioJob.Input, null, -1, applyDRC));

            _log.LogEvent("Trying to open the file with BSAudioSource()", ImageType.Information);
            if (AudioUtil.AVSScriptHasAudio(sbOpen.ToString(), out string strErrorText))
            {
                _log.LogEvent("Sucessfully opened the file with BSAudioSource()", ImageType.Information);
                return true;                    
             }

            sbOpen = new StringBuilder();
            if (String.IsNullOrEmpty(strErrorText))
                _log.LogEvent("Failed opening the file with BSAudioSource()", ImageType.Information);
            else
                _log.LogEvent("Failed opening the file with BSAudioSource(). " + strErrorText, ImageType.Information);
            return false;
        }

        private bool OpenSourceWithBassAudio(out StringBuilder sbOpen)
        {
            sbOpen = new StringBuilder();
            string strPluginPath = Path.Combine(MainForm.Instance.Settings.AvisynthPluginsPath, "BassAudio.dll");
            if (File.Exists(strPluginPath))
            {
                sbOpen.AppendFormat("LoadPlugin(\"{0}\"){1}", strPluginPath, Environment.NewLine);
                sbOpen.AppendFormat("BassAudioSource(\"{0}\"){1}", audioJob.Input, Environment.NewLine);
                _log.LogEvent("Trying to open the file with BassAudioSource()", ImageType.Information);
                if (AudioUtil.AVSScriptHasAudio(sbOpen.ToString(), out string strErrorText))
                {
                    _log.LogEvent("Successfully opened the file with BassAudioSource()", ImageType.Information);
                    return true;
                }
                sbOpen = new StringBuilder();
                if (String.IsNullOrEmpty(strErrorText))
                    _log.LogEvent("Failed opening the file with BassAudioSource()", ImageType.Information);
                else
                    _log.LogEvent("Failed opening the file with BassAudioSource(). " + strErrorText, ImageType.Information);
            }
            else
                _log.LogEvent("Failed opening the file with BassAudioSource() as BassAudio.dll is not available", ImageType.Information);
            return false;
        }

        private bool OpenSourceWithDirectShow(out StringBuilder sbOpen, MediaInfoFile oInfo)
        {
            sbOpen = new StringBuilder();

            try
            {
                if (oInfo.HasAudio)
                {
                    if (MainForm.Instance.Settings.PortableAviSynth)
                        sbOpen.AppendFormat("LoadPlugin(\"{0}\"){1}", Path.Combine(Path.GetDirectoryName(MainForm.Instance.Settings.AviSynth.Path), @"plugins\directshowsource.dll"), Environment.NewLine);
                    if (oInfo.HasVideo)
                        sbOpen.AppendFormat("DirectShowSource(\"{0}\", video=false){1}", audioJob.Input, Environment.NewLine);
                    else
                        sbOpen.AppendFormat("DirectShowSource(\"{0}\"){1}", audioJob.Input, Environment.NewLine);
                    sbOpen.AppendFormat("EnsureVBRMP3Sync(){0}", Environment.NewLine);
                }
            }
            catch { }

            string strErrorText = String.Empty;
            _log.LogEvent("Trying to open the file with DirectShowSource()", ImageType.Information);
            if (sbOpen.Length > 0 && AudioUtil.AVSScriptHasAudio(sbOpen.ToString(), out strErrorText))
            {
                _log.LogEvent("Successfully opened the file with DirectShowSource()", ImageType.Information);
                return true;
            }
            sbOpen = new StringBuilder();
            if (String.IsNullOrEmpty(strErrorText))
                _log.LogEvent("Failed opening the file with DirectShowSource()", ImageType.Information);
            else
                _log.LogEvent("Failed opening the file with DirectShowSource(). " + strErrorText, ImageType.Information);
            return false;
        }

        private bool OpenSourceWithImportAVS(out StringBuilder sbOpen)
        {
            sbOpen = new StringBuilder();
            sbOpen.AppendFormat("Import(\"{0}\"){1}", audioJob.Input, Environment.NewLine);
            return true;
        }

        private bool OpenSourceWithNicAudio(out StringBuilder sbOpen, MediaInfoFile oInfo, bool bForce)
        {
            string size = String.Empty;
            FileInfo fi = new FileInfo(audioJob.Input);
            if (fi.Length / Math.Pow(1024, 3) > 2)
                size = ", 1";

            sbOpen = new StringBuilder();
            switch (Path.GetExtension(audioJob.Input).ToLowerInvariant())
            {
                case ".ac3":
                case ".ddp":
                case ".eac3":
                    sbOpen.AppendFormat("LoadPlugin(\"{0}\"){1}", Path.Combine(MainForm.Instance.Settings.AvisynthPluginsPath, "NicAudio.dll"), Environment.NewLine);
                    sbOpen.AppendFormat("NicAc3Source(\"{0}\"", audioJob.Input);
                    if (audioJob.Settings.ApplyDRC)
                        sbOpen.AppendFormat(", DRC=1){0}", Environment.NewLine);
                    else
                        sbOpen.AppendFormat("){0}", Environment.NewLine);
                    break;
                case ".avi":
                    sbOpen.AppendFormat("AVISource(\"{0}\", audio=true){1}", audioJob.Input, Environment.NewLine);
                    sbOpen.AppendFormat("EnsureVBRMP3Sync(){0}", Environment.NewLine);
                    sbOpen.AppendFormat("Trim(0,0){0}", Environment.NewLine); // to match audio length
                    break;
                case ".avs":
                    sbOpen.AppendFormat("Import(\"{0}\"){1}", audioJob.Input, Environment.NewLine);
                    break;
                case ".dtshd":
                case ".dtsma":
                case ".dts":
                    sbOpen.AppendFormat("LoadPlugin(\"{0}\"){1}", Path.Combine(MainForm.Instance.Settings.AvisynthPluginsPath, "NicAudio.dll"), Environment.NewLine);
                    sbOpen.AppendFormat("NicDtsSource(\"{0}\"", audioJob.Input);
                    if (audioJob.Settings.ApplyDRC)
                        sbOpen.AppendFormat(", DRC=1){0}", Environment.NewLine);
                    else
                        sbOpen.AppendFormat("){0}", Environment.NewLine);
                    break;
                case ".mpa":
                case ".mpg":
                case ".mp2":
                case ".mp3":
                    sbOpen.AppendFormat("LoadPlugin(\"{0}\"){1}", Path.Combine(MainForm.Instance.Settings.AvisynthPluginsPath, "NicAudio.dll"), Environment.NewLine);
                    sbOpen.AppendFormat("NicMPG123Source(\"{0}\"){1}", audioJob.Input, Environment.NewLine);
                    audioJob.FilesToDelete.Add(audioJob.Input + ".d2a");
                    break;
                case ".wav":
                    BinaryReader r = new BinaryReader(File.Open(audioJob.Input, FileMode.Open, FileAccess.Read));

                    try
                    {
                        r.ReadBytes(20);
                        UInt16 AudioFormat = r.ReadUInt16();  // read a LE int_16, offset 20 + 2 = 22

                        switch (AudioFormat)
                        {
                            case 0x0001:         // PCM Format Int
                                r.ReadBytes(22);   // 22 + 22 = 44
                                UInt32 DtsHeader = r.ReadUInt32(); // read a LE int_32
                                sbOpen.AppendFormat("LoadPlugin(\"{0}\"){1}", Path.Combine(MainForm.Instance.Settings.AvisynthPluginsPath, "NicAudio.dll"), Environment.NewLine);
                                if (DtsHeader == 0xE8001FFF)
                                {
                                    sbOpen.AppendFormat("NicDtsSource(\"{0}\"", audioJob.Input);
                                    if (audioJob.Settings.ApplyDRC)
                                        sbOpen.AppendFormat(", DRC=1){0}", Environment.NewLine);
                                    else
                                        sbOpen.AppendFormat("){0}", Environment.NewLine);
                                }
                                else
                                    sbOpen.AppendFormat("RaWavSource(\"{0}\"{2}){1}", audioJob.Input, Environment.NewLine, size);
                                break;
                            case 0x0003:         // IEEE Float
                            case 0xFFFE:         // WAVE_FORMAT_EXTENSIBLE header
                                sbOpen.AppendFormat("LoadPlugin(\"{0}\"){1}", Path.Combine(MainForm.Instance.Settings.AvisynthPluginsPath, "NicAudio.dll"), Environment.NewLine);
                                sbOpen.AppendFormat("RaWavSource(\"{0}\"{2}){1}", audioJob.Input, Environment.NewLine, size);
                                break;
                            case 0x0055:         // MPEG Layer 3
                                sbOpen.AppendFormat("LoadPlugin(\"{0}\"){1}", Path.Combine(MainForm.Instance.Settings.AvisynthPluginsPath, "NicAudio.dll"), Environment.NewLine);
                                sbOpen.AppendFormat("NicMPG123Source(\"{0}\"){1}", audioJob.Input, Environment.NewLine);
                                break;
                            case 0x2000:         // AC3
                                sbOpen.AppendFormat("LoadPlugin(\"{0}\"){1}", Path.Combine(MainForm.Instance.Settings.AvisynthPluginsPath, "NicAudio.dll"), Environment.NewLine);
                                sbOpen.AppendFormat("NicAc3Source(\"{0}\"", audioJob.Input);
                                if (audioJob.Settings.ApplyDRC)
                                    sbOpen.AppendFormat(", DRC=1){0}", Environment.NewLine);
                                else
                                    sbOpen.AppendFormat("){0}", Environment.NewLine);
                                break;
                            default:
                                sbOpen.AppendFormat("WavSource(\"{0}\"){1}", audioJob.Input, Environment.NewLine);
                                break;
                        }
                    }
                    catch (EndOfStreamException e)
                    {
                        LogItem _oLog = MainForm.Instance.Log.Info("Error");
                        _oLog.LogValue(e.GetType().Name + ", wavfile can't be read.", e, ImageType.Error);
                    }
                    finally
                    {
                        r.Close();
                    }
                    break;
                case ".w64":
                case ".aif":
                case ".au":
                case ".caf":
                case ".bwf":
                    sbOpen.AppendFormat("LoadPlugin(\"{0}\"){1}", Path.Combine(MainForm.Instance.Settings.AvisynthPluginsPath, "NicAudio.dll"), Environment.NewLine);
                    sbOpen.AppendFormat("RaWavSource(\"{0}\"{2}){1}", audioJob.Input, Environment.NewLine, size);
                    break;
            }

            _log.LogEvent("Trying to open the file with NicAudio", ImageType.Information);

            string strErrorText = String.Empty;
            if (oInfo.AudioInfo.Tracks.Count > 0 && oInfo.AudioInfo.Tracks[0].AudioCodec == AudioCodec.DTS && oInfo.AudioInfo.Tracks[0].CodecProfile.Contains("MA") && !bForce)
            {
                _log.LogEvent("DTS-MA is blocked in the first place when using NicAudio", ImageType.Information);
            }
            else if (sbOpen.Length > 0 && AudioUtil.AVSScriptHasAudio(sbOpen.ToString(), out strErrorText))
            {
                _log.LogEvent("Successfully opened the file with NicAudio", ImageType.Information);
                return true;
            }

            sbOpen = new StringBuilder();
            if (String.IsNullOrEmpty(strErrorText))
                _log.LogEvent("Failed opening the file with NicAudio()", ImageType.Information);
            else
                _log.LogEvent("Failed opening the file with NicAudio(). " + strErrorText, ImageType.Information);
            return false;
        }

        #endregion

        #region IJobProcessor Members


        public void setup(Job job, StatusUpdate su, LogItem log)
        {
            this._log = log;
            this.audioJob = (AudioJob)job;
            this.su = su;
        }

        private bool CreateAviSynthScript(out string strError)
        {
            strError = string.Empty;

            //let's create the avisynth script
            StringBuilder script = new StringBuilder();

            string id = _uniqueId;
            string tmp = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), id);

            string strChannelCount = string.Empty;
            string strChannelPositions = string.Empty;
            int iChannelCount = 0;
            int iAVSChannelCount = 0;
            int iAVSAudioSampleRate = 0;
            int iChannelMask = 0;

            using (MediaInfoFile oInfo = new MediaInfoFile(audioJob.Input, ref _log))
            {

                bool bFound = false;
                if (oInfo.ContainerFileTypeString.Equals("AVS"))
                {
                    bFound = OpenSourceWithImportAVS(out script);
                }
                else if (audioJob.Settings.PreferredDecoder == AudioDecodingEngine.NicAudio)
                {
                    bFound = OpenSourceWithNicAudio(out script, oInfo, false);
                    if (!bFound)
                        bFound = OpenSourceWithLSMASHAudioSource(out script);
                    if (!bFound)
                        bFound = OpenSourceWithBassAudio(out script);
                    if (!bFound)
                        bFound = OpenSourceWithFFAudioSource(out script);
                    if (!bFound)
                        bFound = OpenSourceWithDirectShow(out script, oInfo);
                    if (!bFound)
                        bFound = OpenSourceWithBestAudioSource(out script);
                }
                else if (audioJob.Settings.PreferredDecoder == AudioDecodingEngine.BassAudio)
                {
                    bFound = OpenSourceWithBassAudio(out script);
                    if (!bFound)
                        bFound = OpenSourceWithLSMASHAudioSource(out script);
                    if (!bFound)
                        bFound = OpenSourceWithNicAudio(out script, oInfo, false);
                    if (!bFound)
                        bFound = OpenSourceWithFFAudioSource(out script);
                    if (!bFound)
                        bFound = OpenSourceWithDirectShow(out script, oInfo);
                    if (!bFound)
                        bFound = OpenSourceWithBestAudioSource(out script);
                }
                else if (audioJob.Settings.PreferredDecoder == AudioDecodingEngine.LWLibavAudioSource)
                {
                    bFound = OpenSourceWithLSMASHAudioSource(out script);
                    if (!bFound)
                        bFound = OpenSourceWithNicAudio(out script, oInfo, false);
                    if (!bFound)
                        bFound = OpenSourceWithBassAudio(out script);
                    if (!bFound)
                        bFound = OpenSourceWithFFAudioSource(out script);
                    if (!bFound)
                        bFound = OpenSourceWithDirectShow(out script, oInfo);
                    if (!bFound)
                        bFound = OpenSourceWithBestAudioSource(out script);
                }
                else if (audioJob.Settings.PreferredDecoder == AudioDecodingEngine.FFAudioSource)
                {
                    bFound = OpenSourceWithFFAudioSource(out script);
                    if (!bFound)
                        bFound = OpenSourceWithLSMASHAudioSource(out script);
                    if (!bFound)
                        bFound = OpenSourceWithNicAudio(out script, oInfo, false);
                    if (!bFound)
                        bFound = OpenSourceWithBassAudio(out script);
                    if (!bFound)
                        bFound = OpenSourceWithDirectShow(out script, oInfo);
                    if (!bFound)
                        bFound = OpenSourceWithBestAudioSource(out script);
                }
                else if (audioJob.Settings.PreferredDecoder == AudioDecodingEngine.BestAudioSource)
                {
                     bFound = OpenSourceWithBestAudioSource(out script);
                    if (!bFound)
                        bFound = OpenSourceWithLSMASHAudioSource(out script);
                    if (!bFound)
                        bFound = OpenSourceWithFFAudioSource(out script);
                    if (!bFound)
                        bFound = OpenSourceWithNicAudio(out script, oInfo, false);
                    if (!bFound)
                        bFound = OpenSourceWithBassAudio(out script);
                    if (!bFound)
                        bFound = OpenSourceWithDirectShow(out script, oInfo);
                }
                else
                {
                    bFound = OpenSourceWithDirectShow(out script, oInfo);
                    if (!bFound)
                        bFound = OpenSourceWithLSMASHAudioSource(out script);
                    if (!bFound)
                        bFound = OpenSourceWithNicAudio(out script, oInfo, false);
                    if (!bFound)
                        bFound = OpenSourceWithBassAudio(out script);
                    if (!bFound)
                        bFound = OpenSourceWithFFAudioSource(out script);
                }

                if (!bFound && oInfo.AudioInfo.Tracks.Count > 0 && oInfo.AudioInfo.Tracks[0].AudioCodec == AudioCodec.DTS && oInfo.AudioInfo.Tracks[0].CodecProfile.Contains("MA"))
                    bFound = OpenSourceWithNicAudio(out script, oInfo, true);

                if (!bFound)
                {
                    DeleteTempFiles();
                    strError = "Failed opening the file: " + audioJob.Input;
                    return false;
                }

                if (MainForm.Instance.Settings.PortableAviSynth && MainForm.Instance.Settings.AviSynthPlus)
                {
                    script.Insert(0, String.Format("ClearAutoloadDirs(){0}AddAutoloadDir(\"{1}\"){0}",
                        Environment.NewLine,
                        Path.Combine(Path.GetDirectoryName(MainForm.Instance.Settings.AviSynth.Path), @"plugins")));
                }

                if (audioJob.Delay != 0)
                    script.AppendFormat("DelayAudio({0}.0/1000.0){1}", audioJob.Delay, Environment.NewLine);

                if (!string.IsNullOrEmpty(audioJob.CutFile))
                {
                    try
                    {
                        Cuts cuts = FilmCutter.ReadCutsFromFile(audioJob.CutFile);
                        script.AppendLine(FilmCutter.GetCutsScript(cuts, true));
                    }
                    catch (FileNotFoundException)
                    {
                        DeleteTempFiles();
                        strError = "Required file '" + audioJob.CutFile + "' is missing.";
                        return false;
                    }
                    catch (Exception)
                    {
                        DeleteTempFiles();
                        strError = "Broken cuts file, " + audioJob.CutFile + ", can't continue.";
                        return false;
                    }
                }

                // detect audio information
                // get real detected channel count from AVS 
                using (AvsFile avi = AvsFile.ParseScript(script.ToString()))
                {
                    iAVSChannelCount    = avi.Clip.ChannelsCount;
                    iAVSAudioSampleRate = avi.Clip.AudioSampleRate;
                    iChannelMask        = avi.Clip.ChannelMask;
                }

                // get the channel information from source file
                if (oInfo.ContainerFileTypeString.Equals("AVS"))
                {
                    strChannelCount = AudioUtil.getChannelCountFromAVSFile(audioJob.Input);
                    if (String.IsNullOrEmpty(strChannelCount))
                        strChannelCount = oInfo.AudioInfo.Tracks[0].NbChannels;
                    strChannelPositions = AudioUtil.getChannelPositionsFromAVSFile(audioJob.Input);
                }
                else
                {
                    strChannelCount = oInfo.AudioInfo.Tracks[0].NbChannels;
                    strChannelPositions = oInfo.AudioInfo.Tracks[0].ChannelPositions;
                } 
            }

            int iCount = 0;
            string[] x = strChannelCount.Split(new string[] { " / " }, StringSplitOptions.None);
            for (int i = 0; i < x.Length; i++)
            {
                if (int.TryParse(x[i].Split(' ')[0].Trim(), out iChannelCount))
                {
                    iCount = i;
                    if (iChannelCount == iAVSChannelCount)
                        break;
                }
            }

            if (!String.IsNullOrEmpty(strChannelPositions))
            {
                x = strChannelPositions.Split(new string[] { " / " }, StringSplitOptions.None);
                if (iCount < x.Length)
                    strChannelPositions = x[iCount].Trim();
            }

            script.AppendFormat(@"# Detected Channels from input file : {0}{1}", iChannelCount, Environment.NewLine);

            if (MainForm.Instance.Settings.AviSynthPlus)
            {
                script.AppendFormat(@"# Detected Channel Mask from input file : {0}{1}", iChannelMask, Environment.NewLine);
                script.AppendFormat(@"# Detected Channels Layout from input file : {0}{1}", GetChannelLayoutFromMask(iChannelMask), Environment.NewLine);
            }
            else
                script.AppendFormat(@"# Detected Channels Layout from input file : {0}{1}", strChannelPositions, Environment.NewLine);


            if (iAVSChannelCount != iChannelCount)
                _log.LogEvent("Channel count mismatch! The input file is reporting " + iChannelCount + " channels and the AviSynth script is reporting " + iAVSChannelCount + " channels", ImageType.Warning);

            if (audioJob.Settings.DownmixMode != ChannelMode.KeepOriginal)
                script.Append(@"# Applied Downmix" + Environment.NewLine);

            switch (audioJob.Settings.DownmixMode)
            {
                case ChannelMode.KeepOriginal:
                    break;
                case ChannelMode.ConvertToMono:
                    script.AppendFormat("ConvertToMono(){0}", Environment.NewLine);
                    break;
                case ChannelMode.Downmix51:
                case ChannelMode.DPLDownmix:
                case ChannelMode.DPLIIDownmix:
                case ChannelMode.StereoDownmix:
                    if (iChannelCount == 0)
                    {
                        _log.LogEvent("No audio detected: " + audioJob.Input, ImageType.Error);
                        break;
                    }
                    if (iAVSChannelCount != iChannelCount)
                    {
                        _log.LogEvent("Ignoring Downmix because of the channel count mismatch", ImageType.Warning);
                        break;
                    }
                    if (iChannelCount <= 2 || (iChannelCount <= 6 && audioJob.Settings.DownmixMode == ChannelMode.Downmix51))
                    {
                        _log.LogEvent("Ignoring Downmix as there is only " + iChannelCount + " channel(s)", ImageType.Information);
                        break;
                    }
                    if (String.IsNullOrEmpty(strChannelPositions))
                        _log.LogEvent("No channel positions found. Downmix result may be wrong.", ImageType.Information);

                    if (iChannelCount > 5)
                    {
                        string strPluginPath = Path.Combine(MainForm.Instance.Settings.AvisynthPluginsPath, "AudioLimiter.dll");
                        if (File.Exists(strPluginPath))
                            script.AppendFormat("LoadPlugin(\"{0}\"){1}", strPluginPath, Environment.NewLine);
                    }

                    switch (strChannelPositions)
                    {
                        // http://forum.doom9.org/showthread.php?p=1461787#post1461787
                        case "3/0/0":
                        case "2/0/0.1": 
                            script.Append(@"c3_stereo(ConvertAudioToFloat(last))" + Environment.NewLine);
                            script.Append(@"# 3 Channels to Stereo
function c3_stereo(clip a)
{
    fl = GetChannel(a, 1)
    fr = GetChannel(a, 2)
    fc = GetChannel(a, 3)
    l = MixAudio(fl, fc, 0.5858, 0.4142)
    r = MixAudio(fr, fc, 0.5858, 0.4142)
    return MergeChannels(l, r)
}" + Environment.NewLine);
                            break;
                        case "2/1/0":
                        case "2/0/1":
                            if (audioJob.Settings.DownmixMode == ChannelMode.StereoDownmix)
                            {
                                script.Append(@"c3_stereo(ConvertAudioToFloat(last))" + Environment.NewLine);
                                script.Append(@"# 3 Channels to Stereo
function c3_stereo(clip a)
{
    fl = GetChannel(a, 1)
    fr = GetChannel(a, 2)
    fc = GetChannel(a, 3)
    l = MixAudio(fl, fc, 0.5858, 0.4142)
    r = MixAudio(fr, fc, 0.5858, 0.4142)
    return MergeChannels(l, r)
}" + Environment.NewLine);
                            }
                            else
                            {
                                script.Append(@"c3_dpl(ConvertAudioToFloat(last))" + Environment.NewLine);
                                script.Append(@"# 3 Channels to Dolby ProLogic
function c3_dpl(clip a)
{
    fl = GetChannel(a, 1)
    fr = GetChannel(a, 2)
    bc = GetChannel(a, 3)
    l = MixAudio(fl, bc, 0.5858, 0.4142)
    r = MixAudio(fr, bc, 0.5858, -0.4142)
    return MergeChannels(l, r)
}" + Environment.NewLine);
                            }
                            break;
                        case "2/2/0":
                        case "2/0/2":
                            if (audioJob.Settings.DownmixMode == ChannelMode.StereoDownmix)
                            {
                                script.Append(@"c4_stereo(ConvertAudioToFloat(last))" + Environment.NewLine);
                                script.Append(@"# 4 Channels to Stereo
function c4_stereo(clip a)
{
    fl = GetChannel(a, 1)
    fr = GetChannel(a, 2)
    sl = GetChannel(a, 3)
    sr = GetChannel(a, 4)
    l = MixAudio(fl, sl, 0.5, 0.5)
    r = MixAudio(fr, sr, 0.5, 0.5)
    return MergeChannels(l, r)
}" + Environment.NewLine);
                            }
                            else if (audioJob.Settings.DownmixMode == ChannelMode.DPLDownmix)
                            {
                                script.Append(@"c4_dpl(ConvertAudioToFloat(last))" + Environment.NewLine);
                                script.Append(@"# 4 Channels to Dolby ProLogic
function c4_dpl(clip a)
{
    fl = GetChannel(a, 1)
    fr = GetChannel(a, 2)
    sl = GetChannel(a, 3)
    sr = GetChannel(a, 4)
    bc = MixAudio(sl, sr, 0.2929, 0.2929)
    l = MixAudio(fl, bc, 0.4142, 1.0)
    r = MixAudio(fr, bc, 0.4142, -1.0)
    return MergeChannels(l, r)
}" + Environment.NewLine);
                            }
                            else
                            {
                                script.Append(@"c4_dpl2(ConvertAudioToFloat(last))" + Environment.NewLine);
                                script.Append(@"# 4 Channels to Dolby ProLogic II
function c4_dpl2(clip a)
{
    fl = GetChannel(a, 1)
    fr = GetChannel(a, 2)
    sl = GetChannel(a, 3)
    sr = GetChannel(a, 4)
    ssl = MixAudio(sl, sr, 0.3714, 0.2144)
    ssr = MixAudio(sl, sr, -0.2144, -0.3714)
    l = MixAudio(fl, ssl, 0.4142, 1.0)
    r = MixAudio(fr, ssr, 0.4142, 1.0)
    return MergeChannels(l, r)
}" + Environment.NewLine);
                            }
                            break;
                        case "2/1/0.1":
                        case "2/0/1.1":
                            if (audioJob.Settings.DownmixMode == ChannelMode.StereoDownmix)
                            {
                                script.Append(@"c42_stereo(ConvertAudioToFloat(last))" + Environment.NewLine);
                                script.Append(@"# 4 Channels to Stereo
function c42_stereo(clip a)
{
    fl = GetChannel(a, 1)
    fr = GetChannel(a, 2)
    fc = GetChannel(a, 3)
    lf = GetChannel(a, 4)
    fc_lf = MixAudio(fc, lf, 0.2929, 0.2929)
    l = MixAudio(fl, fc_lf, 0.4142, 1.0)
    r = MixAudio(fr, fc_lf, 0.4142, 1.0)
    return MergeChannels(l, r)
}" + Environment.NewLine);
                            }
                            else
                            {
                                script.Append(@"c42_dpl(ConvertAudioToFloat(last))" + Environment.NewLine);
                                script.Append(@"# 4 Channels L,R,LFE,S  -> Dolby ProLogic
function c42_dpl(clip a)
{
    fl = GetChannel(a, 1)
    fr = GetChannel(a, 2)
    bc = GetChannel(a, 4)
    l = MixAudio(fl, bc, 0.5858, 0.4142)
    r = MixAudio(fr, bc, 0.5858, -0.4142)
    return MergeChannels(l, r)
}" + Environment.NewLine);
                            }  
                            break;
                        case "3/0/0.1":
                            if (audioJob.Settings.DownmixMode == ChannelMode.StereoDownmix)
                            {
                                script.Append(@"c42_stereo(ConvertAudioToFloat(last))" + Environment.NewLine);
                                script.Append(@"# 4 Channels L,R,C,LFE or L,R,S,LFE or L,R,C,S -> Stereo
function c42_stereo(clip a)
{
    fl = GetChannel(a, 1)
    fr = GetChannel(a, 2)
    fc = GetChannel(a, 3)
    lf = GetChannel(a, 4)
    fc_lf = MixAudio(fc, lf, 0.2929, 0.2929)
    l = MixAudio(fl, fc_lf, 0.4142, 1.0)
    r = MixAudio(fr, fc_lf, 0.4142, 1.0)
    return MergeChannels(l, r)
}" + Environment.NewLine);
                            }
                            else
                            {
                                script.Append(@"c3_stereo(ConvertAudioToFloat(last))" + Environment.NewLine);
                                script.Append(@"# 3 Channels L,R,C or L,R,S or L,R,LFE -> Stereo
function c3_stereo(clip a)
{
    fl = GetChannel(a, 1)
    fr = GetChannel(a, 2)
    fc = GetChannel(a, 3)
    l = MixAudio(fl, fc, 0.5858, 0.4142)
    r = MixAudio(fr, fc, 0.5858, 0.4142)
    return MergeChannels(l, r)
}" + Environment.NewLine);
                            }
                            break;

                        case "3/1/0":
                        case "3/0/1":
                            if (audioJob.Settings.DownmixMode == ChannelMode.StereoDownmix)
                            {
                                script.Append(@"c42_stereo(ConvertAudioToFloat(last))" + Environment.NewLine);
                                script.Append(@"# 4 Channels L,R,C,LFE or L,R,S,LFE or L,R,C,S -> Stereo
function c42_stereo(clip a)
{
    fl = GetChannel(a, 1)
    fr = GetChannel(a, 2)
    fc = GetChannel(a, 3)
    lf = GetChannel(a, 4)
    fc_lf = MixAudio(fc, lf, 0.2929, 0.2929)
    l = MixAudio(fl, fc_lf, 0.4142, 1.0)
    r = MixAudio(fr, fc_lf, 0.4142, 1.0)
    return MergeChannels(l, r)
}" + Environment.NewLine);
                            }
                            else
                            {
                                script.Append(@"c43_dpl(ConvertAudioToFloat(last))" + Environment.NewLine);
                                script.Append(@"# 4 Channels L,R,C,S -> Dolby ProLogic
function c43_dpl(clip a)
{
    fl = GetChannel(a, 1)
    fr = GetChannel(a, 2)
    fc = GetChannel(a, 3)
    bc = GetChannel(a, 4)
    fl_fc = MixAudio(fl, fc, 0.4142, 0.2929)
    fr_fc = MixAudio(fr, fc, 0.4142, 0.2929)
    l = MixAudio(fl_fc, bc, 1.0, 0.2929)
    r = MixAudio(fr_fc, bc, 1.0, -0.2929)
    return MergeChannels(l, r)
}" + Environment.NewLine);
                            }
                            break;
                        case "3/2/0":
                        case "3/0/2":
                            if (audioJob.Settings.DownmixMode == ChannelMode.StereoDownmix)
                            {
                                script.Append(@"c5_stereo(ConvertAudioToFloat(last))" + Environment.NewLine);
                                script.Append(@"# 5 Channels L,R,C,SL,SR or L,R,LFE,SL,SR-> Stereo
function c5_stereo(clip a)
{
    fl = GetChannel(a, 1)
    fr = GetChannel(a, 2)
    fc = GetChannel(a, 3)
    sl = GetChannel(a, 4)
    sr = GetChannel(a, 5)
    fl_sl = MixAudio(fl, sl, 0.3694, 0.3694)
    fr_sr = MixAudio(fr, sr, 0.3694, 0.3694)
    l = MixAudio(fl_sl, fc, 1.0, 0.2612)
    r = MixAudio(fr_sr, fc, 1.0, 0.2612)
    return MergeChannels(l, r)
}" + Environment.NewLine);
                            }
                            else if (audioJob.Settings.DownmixMode == ChannelMode.DPLDownmix)
                            {
                                script.Append(@"c5_dpl(ConvertAudioToFloat(last))" + Environment.NewLine);
                                script.Append(@"# 5 Channels L,R,C,SL,SR -> Dolby ProLogic
function c5_dpl(clip a)
{
    fl = GetChannel(a, 1)
    fr = GetChannel(a, 2)
    fc = GetChannel(a, 3)
    sl = GetChannel(a, 4)
    sr = GetChannel(a, 5)
    bc = MixAudio(sl, sr, 0.2265, 0.2265)
    fl_fc = MixAudio(fl, fc, 0.3205, 0.2265)
    fr_fc = MixAudio(fr, fc, 0.3205, 0.2265)
    l = MixAudio(fl_fc, bc, 1.0, 1.0)
    r = MixAudio(fr_fc, bc, 1.0, -1.0)
    return MergeChannels(l, r)
}" + Environment.NewLine);
                            }
                            else
                            {
                                script.Append(@"c5_dpl2(ConvertAudioToFloat(last))" + Environment.NewLine);
                                script.Append(@"# 5 Channels L,R,C,SL,SR -> Dolby ProLogic II
function c5_dpl2(clip a)
{
    fl = GetChannel(a, 1)
    fr = GetChannel(a, 2)
    fc = GetChannel(a, 3)
    sl = GetChannel(a, 4)
    sr = GetChannel(a, 5)
    ssl = MixAudio(sl, sr, 0.2818, 0.1627)
    ssr = MixAudio(sl, sr, -0.1627, -0.2818)
    fl_fc = MixAudio(fl, fc, 0.3254, 0.2301)
    fr_fc = MixAudio(fr, fc, 0.3254, 0.2301)
    l = MixAudio(fl_fc, ssl, 1.0, 1.0)
    r = MixAudio(fr_fc, ssr, 1.0, 1.0)
    return MergeChannels(l, r)
}" + Environment.NewLine);
                            }
                            break;
                        case "2/2/0.1":
                        case "2/0/2.1":
                            if (audioJob.Settings.DownmixMode == ChannelMode.StereoDownmix)
                            {
                                script.Append(@"c5_stereo(ConvertAudioToFloat(last))" + Environment.NewLine);
                                script.Append(@"# 5 Channels L,R,C,SL,SR or L,R,LFE,SL,SR-> Stereo
function c5_stereo(clip a)
{
    fl = GetChannel(a, 1)
    fr = GetChannel(a, 2)
    fc = GetChannel(a, 3)
    sl = GetChannel(a, 4)
    sr = GetChannel(a, 5)
    fl_sl = MixAudio(fl, sl, 0.3694, 0.3694)
    fr_sr = MixAudio(fr, sr, 0.3694, 0.3694)
    l = MixAudio(fl_sl, fc, 1.0, 0.2612)
    r = MixAudio(fr_sr, fc, 1.0, 0.2612)
    return MergeChannels(l, r)
}" + Environment.NewLine);
                            }
                            else if (audioJob.Settings.DownmixMode == ChannelMode.DPLDownmix)
                            {
                                script.Append(@"c52_dpl(ConvertAudioToFloat(last))" + Environment.NewLine);
                                script.Append(@"# 5 Channels L,R,LFE,SL,SR -> Dolby ProLogic
function c52_dpl(clip a)
{
    fl = GetChannel(a, 1)
    fr = GetChannel(a, 2)
    sl = GetChannel(a, 4)
    sr = GetChannel(a, 5)
    bc = MixAudio(sl, sr, 0.2929, 0.2929)
    l = MixAudio(fl, bc, 0.4142, 1.0)
    r = MixAudio(fr, bc, 0.4142, -1.0)
    return MergeChannels(l, r)
}" + Environment.NewLine);
                            }
                            else
                            {
                                script.Append(@"c52_dpl2(ConvertAudioToFloat(last))" + Environment.NewLine);
                                script.Append(@"# 5 Channels L,R,LFE,SL,SR -> Dolby ProLogic II
function c52_dpl2(clip a)
{
    fl = GetChannel(a, 1)
    fr = GetChannel(a, 2)
    sl = GetChannel(a, 4)
    sr = GetChannel(a, 5)
    ssl = MixAudio(sl, sr, 0.3714, 0.2144)
    ssr = MixAudio(sl, sr, -0.2144, -0.3714)
    l = MixAudio(fl, ssl, 0.4142, 1.0)
    r = MixAudio(fr, ssr, 0.4142, 1.0)
    return MergeChannels(l, r)
}" + Environment.NewLine);
                            }
                            break;
                        case "3/1/0.1":
                        case "3/0/1.1":
                            if (audioJob.Settings.DownmixMode == ChannelMode.StereoDownmix)
                            {
                                script.Append(@"c52_stereo(ConvertAudioToFloat(last))" + Environment.NewLine);
                                script.Append(@"# 5 Channels L,R,C,LFE,S -> Stereo
function c52_stereo(clip a)
{
    fl = GetChannel(a, 1)
    fr = GetChannel(a, 2)
    fc = GetChannel(a, 3)
    lf = GetChannel(a, 4)
    bc = GetChannel(a, 5)
    fl_bc = MixAudio(fl, bc, 0.3205, 0.2265)
    fr_bc = MixAudio(fr, bc, 0.3205, 0.2265)
    fc_lf = MixAudio(fc, lf, 0.2265, 0.2265)
    l = MixAudio(fl_bc, fc_lf, 1.0, 1.0)
    r = MixAudio(fr_bc, fc_lf, 1.0, 1.0)
    return MergeChannels(l, r)
}" + Environment.NewLine);
                            }
                            else
                            {
                                script.Append(@"c53_dpl(ConvertAudioToFloat(last))" + Environment.NewLine);
                                script.Append(@"# 5 Channels L,R,C,LFE,S -> Dolby ProLogic
function c53_dpl(clip a)
{
    fl = GetChannel(a, 1)
    fr = GetChannel(a, 2)
    fc = GetChannel(a, 3)
    bc = GetChannel(a, 5)
    fl_fc = MixAudio(fl, fc, 0.4142, 0.2929)
    fr_fc = MixAudio(fr, fc, 0.4142, 0.2929)
    l = MixAudio(fl_fc, bc, 1.0, 0.2929)
    r = MixAudio(fr_fc, bc, 1.0, -0.2929)
    return MergeChannels(l, r)
}" + Environment.NewLine);
                            }
                            break;
                        case "3/2/0.1":
                        case "3/0/2.1":
                            if (audioJob.Settings.DownmixMode == ChannelMode.StereoDownmix)
                            {
                                script.Append(@"c6_stereo(ConvertAudioToFloat(last))" + Environment.NewLine);
                                script.Append(@"# 5.1 Channels L,R,C,LFE,SL,SR -> stereo + LFE
function c6_stereo(clip a)
{
    fl = GetChannel(a, 1)
    fr = GetChannel(a, 2)
    fc = GetChannel(a, 3)
    lf = GetChannel(a, 4)
    sl = GetChannel(a, 5)
    sr = GetChannel(a, 6)
    fl_sl = MixAudio(fl, sl, 0.2929, 0.2929)
    fr_sr = MixAudio(fr, sr, 0.2929, 0.2929)
    fc_lf = MixAudio(fc, lf, 0.2071, 0.2071)
    l = MixAudio(fl_sl, fc_lf, 1.0, 1.0)
    r = MixAudio(fr_sr, fc_lf, 1.0, 1.0)
    return MergeChannels(l, r)
}" + Environment.NewLine);
                            }
                            else if (audioJob.Settings.DownmixMode == ChannelMode.DPLDownmix)
                            {
                                script.Append(@"c6_dpl(ConvertAudioToFloat(last))" + Environment.NewLine);
                                script.Append(@"# 5.1 Channels L,R,C,LFE,SL,SR -> Dolby ProLogic
function c6_dpl(clip a)
{
    fl = GetChannel(a, 1)
    fr = GetChannel(a, 2)
    fc = GetChannel(a, 3)
    sl = GetChannel(a, 5)
    sr = GetChannel(a, 6)
    bc = MixAudio(sl, sr, 0.2265, 0.2265)
    fl_fc = MixAudio(fl, fc, 0.3205, 0.2265)
    fr_fc = MixAudio(fr, fc, 0.3205, 0.2265)
    l = MixAudio(fl_fc, bc, 1.0, 1.0)
    r = MixAudio(fr_fc, bc, 1.0, -1.0)
    return MergeChannels(l, r)
}" + Environment.NewLine);
                            }
                            else
                            {
                                script.Append(@"c6_dpl2(ConvertAudioToFloat(last))" + Environment.NewLine);
                                script.Append(@"# 5.1 Channels L,R,C,LFE,SL,SR -> Dolby ProLogic II
function c6_dpl2(clip a)
{
    fl = GetChannel(a, 1)
    fr = GetChannel(a, 2)
    fc = GetChannel(a, 3)
    sl = GetChannel(a, 5)
    sr = GetChannel(a, 6)
    ssl = MixAudio(sl, sr, 0.2818, 0.1627)
    ssr = MixAudio(sl, sr, -0.1627, -0.2818)
    fl_fc = MixAudio(fl, fc, 0.3254, 0.2301)
    fr_fc = MixAudio(fr, fc, 0.3254, 0.2301)
    l = MixAudio(fl_fc, ssl, 1.0, 1.0)
    r = MixAudio(fr_fc, ssr, 1.0, 1.0)
    return MergeChannels(l, r)
}" + Environment.NewLine);
                            }
                            break;
                        default:
                            if (audioJob.Settings.DownmixMode == ChannelMode.Downmix51)
                            {
                                switch (iChannelCount)
                                {
                                    case 8:
                                        script.Append(@"8<=Audiochannels(last)?c71_c51(ConvertAudioToFloat(last)):last" + Environment.NewLine);
                                        script.Append(@"# 7.1 Channels L,R,C,LFE,BL,BR,SL,SR -> standard 5.1
function c71_c51(clip a)
{
    front = GetChannel(a, 1, 2, 3, 4)
    back  = GetChannel(a, 5, 6)
    side  = GetChannel(a, 7, 8)
    mix   = MixAudio(back, side, 0.5, 0.5).ConvertAudioTo32bit()
    mix   = mix.SoxFilter(""compand 0.1,0.1 -90,-84,-16,-10,-0.1,-3 0.0 -90 0.0"").ConvertAudioToFloat()
    return MergeChannels(front, mix)
}" + Environment.NewLine);
                                        break;
                                    case 7:
                                        script.Append(@"7==Audiochannels(last)?c61_c51(ConvertAudioToFloat(last)):last" + Environment.NewLine);
                                        script.Append(@"# 6.1 Channels L,R,C,LFE,BC,SL,SR -> standard 5.1
function c61_c51(clip a)
{
    front = GetChannel(a, 1, 2, 3, 4)
    bcent = GetChannel(a, 5).Amplify(0.7071)
    back  = MergeChannels(bcent, bcent)
    side  = GetChannel(a, 6, 7)
    mix   = MixAudio(back, side, 0.5, 0.5).ConvertAudioTo32bit()
    mix   = mix.SoxFilter(""compand 0.1,0.1 -90,-84,-16,-10,-0.1,-3 0.0 -90 0.0"").ConvertAudioToFloat()
    return MergeChannels(front, mix)
}" + Environment.NewLine);
                                        break;
                                    default:
                                        script.Append(@"ConvertAudioToFloat(last))" + Environment.NewLine);
                                        break;
                                }
                            }
                            else if (audioJob.Settings.DownmixMode == ChannelMode.StereoDownmix)
                            {
                                switch (iChannelCount)
                                {
                                    case 8:
                                        script.Append(@"8<=Audiochannels(last)?c71_c51(ConvertAudioToFloat(last)):last" + Environment.NewLine);
                                        script.Append(@"# 7.1 Channels L,R,C,LFE,BL,BR,SL,SR -> standard 5.1
function c71_c51(clip a)
{
    front = GetChannel(a, 1, 2, 3, 4)
    back  = GetChannel(a, 5, 6)
    side  = GetChannel(a, 7, 8)
    mix   = MixAudio(back, side, 0.5, 0.5).ConvertAudioTo32bit()
    mix   = mix.SoxFilter(""compand 0.1,0.1 -90,-84,-16,-10,-0.1,-3 0.0 -90 0.0"").ConvertAudioToFloat()
    return MergeChannels(front, mix)
}" + Environment.NewLine);
                                        break;
                                    case 7:
                                        script.Append(@"7==Audiochannels(last)?c61_c51(ConvertAudioToFloat(last)):last" + Environment.NewLine);
                                        script.Append(@"# 6.1 Channels L,R,C,LFE,BC,SL,SR -> standard 5.1
function c61_c51(clip a)
{
    front = GetChannel(a, 1, 2, 3, 4)
    bcent = GetChannel(a, 5).Amplify(0.7071)
    back  = MergeChannels(bcent, bcent)
    side  = GetChannel(a, 6, 7)
    mix   = MixAudio(back, side, 0.5, 0.5).ConvertAudioTo32bit()
    mix   = mix.SoxFilter(""compand 0.1,0.1 -90,-84,-16,-10,-0.1,-3 0.0 -90 0.0"").ConvertAudioToFloat()
    return MergeChannels(front, mix)
}" + Environment.NewLine);
                                        break;
                                    case 6:
                                        script.Append(@"6==Audiochannels(last)?c6_stereo(ConvertAudioToFloat(last)):last" + Environment.NewLine);
                                        script.Append(@"# 5.1 Channels L,R,C,LFE,SL,SR -> stereo + LFE
function c6_stereo(clip a)
{
    fl = GetChannel(a, 1)
    fr = GetChannel(a, 2)
    fc = GetChannel(a, 3)
    lf = GetChannel(a, 4)
    sl = GetChannel(a, 5)
    sr = GetChannel(a, 6)
    fl_sl = MixAudio(fl, sl, 0.2929, 0.2929)
    fr_sr = MixAudio(fr, sr, 0.2929, 0.2929)
    fc_lf = MixAudio(fc, lf, 0.2071, 0.2071)
    l = MixAudio(fl_sl, fc_lf, 1.0, 1.0)
    r = MixAudio(fr_sr, fc_lf, 1.0, 1.0)
    return MergeChannels(l, r)
}" + Environment.NewLine);
                                        break;
                                    case 5:
                                        script.Append(@"5==Audiochannels(last)?c5_stereo(ConvertAudioToFloat(last)):last" + Environment.NewLine);
                                        script.Append(@"# 5 Channels L,R,C,SL,SR or L,R,LFE,SL,SR-> Stereo
function c5_stereo(clip a)
{
    fl = GetChannel(a, 1)
    fr = GetChannel(a, 2)
    fc = GetChannel(a, 3)
    sl = GetChannel(a, 4)
    sr = GetChannel(a, 5)
    fl_sl = MixAudio(fl, sl, 0.3694, 0.3694)
    fr_sr = MixAudio(fr, sr, 0.3694, 0.3694)
    l = MixAudio(fl_sl, fc, 1.0, 0.2612)
    r = MixAudio(fr_sr, fc, 1.0, 0.2612)
    return MergeChannels(l, r)
}" + Environment.NewLine);
                                        break;
                                    case 4:
                                        script.Append(@"4==Audiochannels(last)?c4_stereo(ConvertAudioToFloat(last)):last" + Environment.NewLine);
                                        script.Append(@"# 4 Channels Quadro L,R,SL,SR -> Stereo
function c4_stereo(clip a)
{
    fl = GetChannel(a, 1)
    fr = GetChannel(a, 2)
    sl = GetChannel(a, 3)
    sr = GetChannel(a, 4)
    l = MixAudio(fl, sl, 0.5, 0.5)
    r = MixAudio(fr, sr, 0.5, 0.5)
    return MergeChannels(l, r)
}" + Environment.NewLine);
                                        break;
                                    case 3:
                                        script.Append(@"3==Audiochannels(last)?c3_stereo(ConvertAudioToFloat(last)):last" + Environment.NewLine);
                                        script.Append(@"# 3 Channels L,R,C or L,R,S or L,R,LFE -> Stereo
function c3_stereo(clip a)
{
    fl = GetChannel(a, 1)
    fr = GetChannel(a, 2)
    fc = GetChannel(a, 3)
    l = MixAudio(fl, fc, 0.5858, 0.4142)
    r = MixAudio(fr, fc, 0.5858, 0.4142)
    return MergeChannels(l, r)
}" + Environment.NewLine);
                                        break;
                                    default:
                                        script.Append(@"ConvertAudioToFloat(last))" + Environment.NewLine);
                                        break;
                                }
                            }
                            else if (audioJob.Settings.DownmixMode == ChannelMode.DPLDownmix)
                            {
                                switch (iChannelCount)
                                {
                                    case 8:
                                        script.Append(@"8<=Audiochannels(last)?c71_c51(ConvertAudioToFloat(last)):last" + Environment.NewLine);
                                        script.Append(@"# 7.1 Channels L,R,C,LFE,BL,BR,SL,SR -> standard 5.1
function c71_c51(clip a)
{
    front = GetChannel(a, 1, 2, 3, 4)
    back = GetChannel(a, 5, 6)
    side = GetChannel(a, 6, 7)
    mix = MixAudio(back, side, 0.5, 0.5).SoxFilter(""compand 0.0,0.0 -90,-84,-8,-2,-6,-1,-0,-0.1"")
    return MergeChannels(front, mix)
}" + Environment.NewLine);
                                        break;
                                    case 7:
                                        script.Append(@"7==Audiochannels(last)?c61_c51(ConvertAudioToFloat(last)):last" + Environment.NewLine);
                                        script.Append(@"# 6.1 Channels L,R,C,LFE,BC,SL,SR -> standard 5.1
function c61_c51(clip a)
{
    front = GetChannel(a, 1, 2, 3, 4)
    back = GetChannel(a, 5, 5)
    side = GetChannel(a, 6, 7)
    mix = MixAudio(back, side, 0.25, 0.75).SoxFilter(""""compand 0.0, 0.0 - 90, -84, -8, -2, -6, -1, -0, -0.1"""")
    return MergeChannels(front, mix)
}" + Environment.NewLine);
                                        break;
                                    case 6:
                                        script.Append(@"6==Audiochannels(last)?c6_dpl(ConvertAudioToFloat(last)):last" + Environment.NewLine);
                                        script.Append(@"# 5.1 Channels L,R,C,LFE,SL,SR -> Dolby ProLogic
function c6_dpl(clip a)
    {
        fl = GetChannel(a, 1)
        fr = GetChannel(a, 2)
        fc = GetChannel(a, 3)
        sl = GetChannel(a, 5)
        sr = GetChannel(a, 6)
        bc = MixAudio(sl, sr, 0.2265, 0.2265)
        fl_fc = MixAudio(fl, fc, 0.3205, 0.2265)
        fr_fc = MixAudio(fr, fc, 0.3205, 0.2265)
        l = MixAudio(fl_fc, bc, 1.0, 1.0)
        r = MixAudio(fr_fc, bc, 1.0, -1.0)
        return MergeChannels(l, r)
    }
" + Environment.NewLine);
                                        break;
                                    case 5:
                                        script.Append(@"5==Audiochannels(last)?c5_dpl(ConvertAudioToFloat(last)):last" + Environment.NewLine);
                                        script.Append(@"# 5 Channels L,R,C,SL,SR -> Dolby ProLogic
function c5_dpl(clip a)
{
    fl = GetChannel(a, 1)
    fr = GetChannel(a, 2)
    fc = GetChannel(a, 3)
    sl = GetChannel(a, 4)
    sr = GetChannel(a, 5)
    bc = MixAudio(sl, sr, 0.2265, 0.2265)
    fl_fc = MixAudio(fl, fc, 0.3205, 0.2265)
    fr_fc = MixAudio(fr, fc, 0.3205, 0.2265)
    l = MixAudio(fl_fc, bc, 1.0, 1.0)
    r = MixAudio(fr_fc, bc, 1.0, -1.0)
    return MergeChannels(l, r)
}" + Environment.NewLine);
                                        break;
                                    case 4:
                                        script.Append(@"4==Audiochannels(last)?c4_dpl(ConvertAudioToFloat(last)):last" + Environment.NewLine);
                                        script.Append(@"# 4 Channels Quadro L,R,SL,SR -> Dolby ProLogic
function c4_dpl(clip a)
{
    fl = GetChannel(a, 1)
    fr = GetChannel(a, 2)
    sl = GetChannel(a, 3)
    sr = GetChannel(a, 4)
    bc = MixAudio(sl, sr, 0.2929, 0.2929)
    l = MixAudio(fl, bc, 0.4142, 1.0)
    r = MixAudio(fr, bc, 0.4142, -1.0)
    return MergeChannels(l, r)
}" + Environment.NewLine);
                                        break;
                                    case 3:
                                        script.Append(@"3==Audiochannels(last)?c3_dpl(ConvertAudioToFloat(last)):last" + Environment.NewLine);
                                        script.Append(@"# 3 Channels L,R,S  -> Dolby ProLogic
function c3_dpl(clip a)
{
    fl = GetChannel(a, 1)
    fr = GetChannel(a, 2)
    bc = GetChannel(a, 3)
    l = MixAudio(fl, bc, 0.5858, 0.4142)
    r = MixAudio(fr, bc, 0.5858, -0.4142)
    return MergeChannels(l, r)
}" + Environment.NewLine);
                                        break;
                                    default:
                                        script.Append(@"ConvertAudioToFloat(last))" + Environment.NewLine);
                                        break;
                                }
                            }
                            else
                            {
                                switch (iChannelCount)
                                {
                                    case 8:
                                        script.Append(@"8<=Audiochannels(last)?c71_c51(ConvertAudioToFloat(last)):last" + Environment.NewLine);
                                        script.Append(@"# 7.1 Channels L,R,C,LFE,BL,BR,SL,SR -> standard 5.1
function c71_c51(clip a)
{
    front = GetChannel(a, 1, 2, 3, 4)
    back = GetChannel(a, 5, 6)
    side = GetChannel(a, 7, 8)
    mix = MixAudio(back, side, 0.5, 0.5).ConvertAudioTo32bit()
    mix = mix.SoxFilter(""compand 0.1, 0.1 - 90, -84, -16, -10, -0.1, -3 0.0 - 90 0.0"").ConvertAudioToFloat()
    return MergeChannels(front, mix)
}" + Environment.NewLine);
                                        break;
                                    case 7:
                                        script.Append(@"7<=Audiochannels(last)?c61_c51(ConvertAudioToFloat(last)):last" + Environment.NewLine);
                                        script.Append(@"# 6.1 Channels L,R,C,LFE,BC,SL,SR -> standard 5.1
function c61_c51(clip a)
{
        front = GetChannel(a, 1, 2, 3, 4)
        bcent = GetChannel(a, 5).Amplify(0.7071)
        back  = MergeChannels(bcent, bcent)
        side  = GetChannel(a, 6, 7)
        mix   = MixAudio(back, side, 0.5, 0.5).ConvertAudioTo32bit()
        mix   = mix.SoxFilter(""compand 0.1,0.1 -90,-84,-16,-10,-0.1,-3 0.0 -90 0.0"").ConvertAudioToFloat()
        return MergeChannels(front, mix)
}" + Environment.NewLine);
                                        break;
                                    case 6:
                                        script.Append(@"6==Audiochannels(last)?c6_dpl2(ConvertAudioToFloat(last)):last" + Environment.NewLine);
                                        script.Append(@"# 5.1 Channels L,R,C,LFE,SL,SR -> Dolby ProLogic II
function c6_dpl2(clip a)
{
    fl = GetChannel(a, 1)
    fr = GetChannel(a, 2)
    fc = GetChannel(a, 3)
    sl = GetChannel(a, 5)
    sr = GetChannel(a, 6)
    ssl = MixAudio(sl, sr, 0.2818, 0.1627)
    ssr = MixAudio(sl, sr, -0.1627, -0.2818)
    fl_fc = MixAudio(fl, fc, 0.3254, 0.2301)
    fr_fc = MixAudio(fr, fc, 0.3254, 0.2301)
    l = MixAudio(fl_fc, ssl, 1.0, 1.0)
    r = MixAudio(fr_fc, ssr, 1.0, 1.0)
    return MergeChannels(l, r)
}" + Environment.NewLine);
                                        break;
                                    case 5:
                                        script.Append(@"5==Audiochannels(last)?c5_dpl2(ConvertAudioToFloat(last)):last" + Environment.NewLine);
                                        script.Append(@"# 5 Channels L,R,C,SL,SR -> Dolby ProLogic II
function c5_dpl2(clip a)
{
    fl = GetChannel(a, 1)
    fr = GetChannel(a, 2)
    fc = GetChannel(a, 3)
    sl = GetChannel(a, 4)
    sr = GetChannel(a, 5)
    ssl = MixAudio(sl, sr, 0.2818, 0.1627)
    ssr = MixAudio(sl, sr, -0.1627, -0.2818)
    fl_fc = MixAudio(fl, fc, 0.3254, 0.2301)
    fr_fc = MixAudio(fr, fc, 0.3254, 0.2301)
    l = MixAudio(fl_fc, ssl, 1.0, 1.0)
    r = MixAudio(fr_fc, ssr, 1.0, 1.0)
    return MergeChannels(l, r)
}" + Environment.NewLine);
                                        break;
                                    case 4:
                                        script.Append(@"4==Audiochannels(last)?c4_dpl2(ConvertAudioToFloat(last)):last" + Environment.NewLine);
                                        script.Append(@"# 4 Channels Quadro L,R,SL,SR -> Dolby ProLogic II
function c4_dpl2(clip a)
{
    fl = GetChannel(a, 1)
    fr = GetChannel(a, 2)
    sl = GetChannel(a, 3)
    sr = GetChannel(a, 4)
    ssl = MixAudio(sl, sr, 0.3714, 0.2144)
    ssr = MixAudio(sl, sr, -0.2144, -0.3714)
    l = MixAudio(fl, ssl, 0.4142, 1.0)
    r = MixAudio(fr, ssr, 0.4142, 1.0)
    return MergeChannels(l, r)
}" + Environment.NewLine);
                                        break;
                                    case 3:
                                        script.Append(@"3==Audiochannels(last)?c3_dpl2(ConvertAudioToFloat(last)):last" + Environment.NewLine);
                                        script.Append(@"# 3 Channels L,R,S  -> Dolby ProLogic (we can't make dpl II) # for L,R,C or L,R,LFE use always -> stereo
function c3_dpl2(clip a)
{                      
    flr = GetChannel(a, 1, 2)
    sl  = GetChannel(a, 3)
    sr  = Amplify(sl, -1.0)
    slr = MergeChannels(sl, sr)
    return MixAudio(flr, slr, 0.5858, 0.4142)
}" + Environment.NewLine);
                                        break;

                                    default:
                                        script.Append(@"ConvertAudioToFloat(last))" + Environment.NewLine);
                                        break;
                                }
                            }
                            break;
                    }
                    break;
                case ChannelMode.Upmix:
                case ChannelMode.UpmixUsingSoxEq:
                case ChannelMode.UpmixWithCenterChannelDialog:
                    if (iChannelCount == 0)
                    {
                        _log.LogEvent("No audio detected: " + audioJob.Input, ImageType.Error);
                        break;
                    }
                    if (iAVSChannelCount != iChannelCount)
                    {
                        _log.LogEvent("Ignoring upmix because of the channel count mismatch", ImageType.Warning);
                        break;
                    }
                    if (iChannelCount != 2)
                    {
                        _log.LogEvent("Ignoring upmix as it can only be used for 2 channels", ImageType.Information);
                        break;
                    }                     
                    if (audioJob.Settings.DownmixMode == ChannelMode.Upmix)
                    {
                        CreateTemporallyEqFiles(tmp);
                        script.Append(@"# Applied Upmix" + Environment.NewLine);
                        script.Append("2==Audiochannels(last)?x_upmix" + id + @"(last):last" + Environment.NewLine);
                    }
                    else if (audioJob.Settings.DownmixMode != ChannelMode.Upmix)
                    {
                        script.Append(@"# Applied Upmix using SoxFilter" + Environment.NewLine);
                        string strPluginPath = Path.Combine(MainForm.Instance.Settings.AvisynthPluginsPath, "SoxFilter.dll");
                        if (File.Exists(strPluginPath))
                        {
                            script.AppendFormat("LoadPlugin(\"{0}\"){1}", strPluginPath, Environment.NewLine);
                            if (audioJob.Settings.DownmixMode == ChannelMode.UpmixUsingSoxEq)
                                script.Append("2==Audiochannels(last)?x_upmixR" + id + @"(last):last" + Environment.NewLine);
                            else
                                script.Append("2==Audiochannels(last)?x_upmixC" + id + @"(last):last" + Environment.NewLine);
                        }
                        else
                        {
                            _log.LogEvent("As SoxFilter() is not availabble, use default upmix mode", ImageType.Information);
                            CreateTemporallyEqFiles(tmp);
                            script.Append("2==Audiochannels(last)?x_upmix" + id + @"(last):last" + Environment.NewLine);
                        }
                    }
                    break;
            }

            // SampleRate
            int iTargetAudioSampleRate = 0;
            switch (audioJob.Settings.SampleRate)
            {
                case SampleRateMode.ConvertTo08000:
                    iTargetAudioSampleRate = 8000;  break;
                case SampleRateMode.ConvertTo11025:
                    iTargetAudioSampleRate = 11025; break;
                case SampleRateMode.ConvertTo22050:
                    iTargetAudioSampleRate = 22050; break;
                case SampleRateMode.ConvertTo32000:
                    iTargetAudioSampleRate = 32000; break;
                case SampleRateMode.ConvertTo44100:
                    iTargetAudioSampleRate = 44100; break;
                case SampleRateMode.ConvertTo48000:
                    iTargetAudioSampleRate = 48000; break;
                case SampleRateMode.ConvertTo96000:
                    iTargetAudioSampleRate = 96000; break;
            }

            if (MainForm.Instance.Settings.AviSynthPlus &&
                ((audioJob.Settings.SampleRate != SampleRateMode.KeepOriginal && iTargetAudioSampleRate != iAVSAudioSampleRate)
                    || audioJob.Settings.TimeModification != TimeModificationMode.KeepOriginal))
                script.Append("ConvertAudioToFloat(last)" + Environment.NewLine);

            if (audioJob.Settings.SampleRate != SampleRateMode.KeepOriginal)
            {
                if (iTargetAudioSampleRate == iAVSAudioSampleRate)
                    _log.LogEvent("Ignoring sample rate change as it is the same as in the AVS", ImageType.Information);
                else
                    script.Append("SSRC(" + iTargetAudioSampleRate + ")" + Environment.NewLine);
            }

            // TimeModification
            if (!MainForm.Instance.Settings.AviSynthPlus && (
                audioJob.Settings.TimeModification == TimeModificationMode.SlowDown25To23976WithCorrection ||
                audioJob.Settings.TimeModification == TimeModificationMode.SlowDown25To24WithCorrection ||
                audioJob.Settings.TimeModification == TimeModificationMode.SpeedUp23976To25WithCorrection ||
                audioJob.Settings.TimeModification == TimeModificationMode.SpeedUp24To25WithCorrection))
            {
                string strPluginPath = Path.Combine(MainForm.Instance.Settings.AvisynthPluginsPath, "timestretch.dll");
                if (File.Exists(strPluginPath))
                    script.AppendFormat("LoadPlugin(\"{0}\"){1}", strPluginPath, Environment.NewLine);
            }

            switch (audioJob.Settings.TimeModification)
            {
                case TimeModificationMode.KeepOriginal:
                    break;
                case TimeModificationMode.SpeedUp23976To24:
                    script.Append((MainForm.Instance.Settings.AviSynthPlus ? "TimeStretch" : "TimeStretchPlugin") + "(AssumeSampleRate(Round((AudioRate()*1001.0)/100.0)).SSRC(AudioRate()))" + Environment.NewLine);
                    break;
                case TimeModificationMode.SpeedUp23976To25:
                    script.Append("AssumeSampleRate(Round((AudioRate()*1001.0)/960.0)).SSRC(AudioRate())" + Environment.NewLine);
                    break;
                case TimeModificationMode.SlowDown24To23976:
                    script.Append((MainForm.Instance.Settings.AviSynthPlus ? "TimeStretch" : "TimeStretchPlugin") + "(SSRC(Round((AudioRate() * 1001.0) / 1000.0)).AssumeSampleRate(AudioRate()))" + Environment.NewLine);
                    break;
                case TimeModificationMode.SlowDown25To23976:
                    script.Append("SSRC(Round((AudioRate()*1001.0)/960.0)).AssumeSampleRate(AudioRate())" + Environment.NewLine);
                    break;
                case TimeModificationMode.SpeedUp24To25:
                    script.Append("AssumeSampleRate(Round((AudioRate()*25.0)/24.0)).SSRC(AudioRate())" + Environment.NewLine);
                    break;
                case TimeModificationMode.SlowDown25To24:
                    script.Append("SSRC(Round((AudioRate()*25.0)/24.0)).AssumeSampleRate(AudioRate())" + Environment.NewLine);
                    break;
                case TimeModificationMode.SpeedUp23976To24WithCorrection:
                    script.Append((MainForm.Instance.Settings.AviSynthPlus ? "TimeStretch" : "TimeStretchPlugin") + "(AssumeSampleRate(Round((AudioRate()*1001.0)/100.0)).SSRC(AudioRate()))" + Environment.NewLine + "TimeStretch(tempo=(1001.0/10.0))" + Environment.NewLine);
                    break;
                case TimeModificationMode.SpeedUp23976To25WithCorrection:
                    script.Append((MainForm.Instance.Settings.AviSynthPlus ? "TimeStretch" : "TimeStretchPlugin") + "((tempo=(1001.0/9.6)))" + Environment.NewLine);
                    break;
                case TimeModificationMode.SlowDown25To23976WithCorrection:
                    script.Append((MainForm.Instance.Settings.AviSynthPlus ? "TimeStretch" : "TimeStretchPlugin") + "((tempo=(96.0/1.001)))" + Environment.NewLine);
                    break;
                case TimeModificationMode.SpeedUp24To25WithCorrection:
                    script.Append((MainForm.Instance.Settings.AviSynthPlus ? "TimeStretch" : "TimeStretchPlugin") + "((tempo=(2500.0/24.0)))" + Environment.NewLine);
                    break;
                case TimeModificationMode.SlowDown25To24WithCorrection:
                    script.Append((MainForm.Instance.Settings.AviSynthPlus ? "TimeStretch" : "TimeStretchPlugin") + "((tempo=(2400.0/25.0)))" + Environment.NewLine);
                    break;
                case TimeModificationMode.SlowDown24To23976WithCorrection:
                    script.Append((MainForm.Instance.Settings.AviSynthPlus ? "TimeStretch" : "TimeStretchPlugin") + "(SSRC(Round((AudioRate() * 1001.0) / 1000.0)).AssumeSampleRate(AudioRate()))" + Environment.NewLine + "TimeStretch(tempo = (100.0 / 1.001))" + Environment.NewLine);
                    break;
            }

            // put Normalize() after downmix cases >> http://forum.doom9.org/showthread.php?p=1166117#post1166117
            if (audioJob.Settings.AutoGain)
            {
                if (audioJob.Settings.Normalize != 100)
                    script.AppendFormat("Normalize(" + (audioJob.Settings.Normalize / 100.0).ToString(new CultureInfo("en-us")) + "){0}", Environment.NewLine);
                else
                    script.AppendFormat("Normalize(){0}", Environment.NewLine);
            }

            // let's obtain command line & other stuff
            StringBuilder sb = new StringBuilder();
            if (audioJob.Settings is FlacSettings)
            {
                UpdateCacher.CheckPackage("flac");
                FlacSettings oSettings = audioJob.Settings as FlacSettings;
                _encoderExecutablePath = this._settings.Flac.Path;
                _sendWavHeaderToEncoderStdIn = HeaderType.W64;

                script.Append("AudioBits(last)>24?ConvertAudioTo24bit(last):last " + Environment.NewLine);

                sb.Append(" -" + oSettings.CompressionLevel);
                if (!String.IsNullOrEmpty(oSettings.CustomEncoderOptions))
                    sb.Append(" " + oSettings.CustomEncoderOptions.Trim());
                sb.Append(" - -o \"{0}\"");
            }
            else if (audioJob.Settings is AC3Settings)
            {
                UpdateCacher.CheckPackage("ffmpeg");
                AC3Settings oSettings = audioJob.Settings as AC3Settings;
                _encoderExecutablePath = this._settings.FFmpeg.Path;
                _sendWavHeaderToEncoderStdIn = HeaderType.W64;

                script.Append("32==Audiobits(last)?ConvertAudioTo24bit(last):last" + Environment.NewLine);

                sb.Append(" -hide_banner -i - -y");
                if (!oSettings.CustomEncoderOptions.Contains("-acodec "))
                    sb.Append(" -acodec ac3");
                if (!oSettings.CustomEncoderOptions.Contains("-ab "))
                    sb.Append(" -ab " + oSettings.Bitrate + "k");
                if (!String.IsNullOrEmpty(oSettings.CustomEncoderOptions))
                    sb.Append(" " + oSettings.CustomEncoderOptions.Trim());
                sb.Append(" \"{0}\"");
            }
            else if (audioJob.Settings is MP2Settings)
            {
                UpdateCacher.CheckPackage("ffmpeg");
                MP2Settings oSettings = audioJob.Settings as MP2Settings;
                _encoderExecutablePath = this._settings.FFmpeg.Path;
                _sendWavHeaderToEncoderStdIn = HeaderType.W64;

                script.Append("32==Audiobits(last)?ConvertAudioTo24bit(last):last" + Environment.NewLine);

                sb.Append(" -hide_banner -i - -y");
                if (!oSettings.CustomEncoderOptions.Contains("-acodec "))
                    sb.Append(" -acodec mp2");
                if (!oSettings.CustomEncoderOptions.Contains("-ab "))
                    sb.Append(" -ab " + oSettings.Bitrate + "k");
                if (!String.IsNullOrEmpty(oSettings.CustomEncoderOptions))
                    sb.Append(" " + oSettings.CustomEncoderOptions.Trim());
                sb.Append(" \"{0}\"");
            }
            else if (audioJob.Settings is OggVorbisSettings)
            {
                UpdateCacher.CheckPackage("oggenc2");
                OggVorbisSettings oSettings = audioJob.Settings as OggVorbisSettings;
                _encoderExecutablePath = this._settings.OggEnc.Path;
                _sendWavHeaderToEncoderStdIn = HeaderType.WAV;

                script.Append("32==Audiobits(last)?ConvertAudioTo24bit(last):last" + Environment.NewLine);

                if (!oSettings.CustomEncoderOptions.Contains("--ignorelength"))
                    sb.Append(" --ignorelength");
                if (!oSettings.CustomEncoderOptions.Contains("--quality "))
                    sb.Append(" --quality " + oSettings.Quality.ToString(System.Globalization.CultureInfo.InvariantCulture) + "k");
                if (!String.IsNullOrEmpty(oSettings.CustomEncoderOptions))
                    sb.Append(" " + oSettings.CustomEncoderOptions.Trim());
                sb.Append(" -o \"{0}\" -");
            }
            else if (audioJob.Settings is NeroAACSettings)
            {
                UpdateCacher.CheckPackage("neroaacenc");
                NeroAACSettings oSettings = audioJob.Settings as NeroAACSettings;
                _encoderExecutablePath = this._settings.NeroAacEnc.Path;
                _sendWavHeaderToEncoderStdIn = HeaderType.WAV;

                script.Append("32==Audiobits(last)?ConvertAudioTo24bit(last):last" + Environment.NewLine);

                sb.Append(" -ignorelength");
                if (oSettings.BitrateMode != BitrateManagementMode.VBR && !oSettings.CustomEncoderOptions.Contains("-he") && !oSettings.CustomEncoderOptions.Contains("-hev2") && !oSettings.CustomEncoderOptions.Contains("-lc"))
                {
                    switch (oSettings.Profile)
                    {
                        case AacProfile.HE:
                            sb.Append(" -he");
                            break;
                        case AacProfile.PS:
                            sb.Append(" -hev2");
                            break;
                        case AacProfile.LC:
                            sb.Append(" -lc");
                            break;
                    }
                }

                if (!oSettings.CustomEncoderOptions.Contains("-br ") && !oSettings.CustomEncoderOptions.Contains("-cbr ") && !oSettings.CustomEncoderOptions.Contains("-q "))
                {
                    switch (oSettings.BitrateMode)
                    {
                        case BitrateManagementMode.ABR:
                            sb.AppendFormat(System.Globalization.CultureInfo.InvariantCulture, " -br {0}", oSettings.Bitrate * 1000);
                            break;
                        case BitrateManagementMode.CBR:
                            sb.AppendFormat(System.Globalization.CultureInfo.InvariantCulture, " -cbr {0}", oSettings.Bitrate * 1000);
                            break;
                        case BitrateManagementMode.VBR:
                            sb.AppendFormat(System.Globalization.CultureInfo.InvariantCulture, " -q {0}", oSettings.Quality);
                            break;
                    }
                }

                // Custom Command Line
                if (!String.IsNullOrEmpty(oSettings.CustomEncoderOptions))
                    sb.Append(" " + oSettings.CustomEncoderOptions.Trim());

                sb.Append(" -if - -of \"{0}\"");
            }
            else if (audioJob.Settings is MP3Settings)
            {
                UpdateCacher.CheckPackage("lame");
                MP3Settings oSettings = audioJob.Settings as MP3Settings;
                _encoderExecutablePath = this._settings.Lame.Path;
                _sendWavHeaderToEncoderStdIn = HeaderType.WAV;

                script.Append("32==Audiobits(last)?ConvertAudioTo24bit(last):last" + Environment.NewLine);

                if (!oSettings.CustomEncoderOptions.Contains("-V") && !oSettings.CustomEncoderOptions.Contains("-b ") && !oSettings.CustomEncoderOptions.Contains("--abr "))
                {
                    switch (oSettings.BitrateMode)
                    {
                        case BitrateManagementMode.VBR:
                            sb.Append(" -V" + oSettings.Quality);
                            break;
                        case BitrateManagementMode.CBR:
                            sb.Append(" -b " + oSettings.Bitrate + " --cbr -h");
                            break;
                        case BitrateManagementMode.ABR:
                            sb.Append(" --abr " + oSettings.AbrBitrate + " -h");
                            break;
                    }
                }

                if (!String.IsNullOrEmpty(oSettings.CustomEncoderOptions))
                    sb.Append(" " + oSettings.CustomEncoderOptions.Trim());
                sb.Append(" - \"{0}\"");
            }
            else if (audioJob.Settings is QaacSettings)
            {
                UpdateCacher.CheckPackage("qaac");
                QaacSettings oSettings = audioJob.Settings as QaacSettings;
                _encoderExecutablePath = this._settings.QAAC.Path;
                _sendWavHeaderToEncoderStdIn = HeaderType.WAV;

                script.Append("32==Audiobits(last)?ConvertAudioTo24bit(last):last" + Environment.NewLine);

                if (!oSettings.CustomEncoderOptions.Contains("--ignorelength"))
                    sb.Append(" --ignorelength");
                if (!oSettings.CustomEncoderOptions.Contains("--threading"))
                    sb.Append(" --threading");

                if (!oSettings.CustomEncoderOptions.Contains("-A") &&
                    !oSettings.CustomEncoderOptions.Contains("-V ") &&
                    !oSettings.CustomEncoderOptions.Contains("-v ") &&
                    !oSettings.CustomEncoderOptions.Contains("-a ") &&
                    !oSettings.CustomEncoderOptions.Contains("-c "))
                {
                    if (oSettings.Profile == QaacProfile.ALAC)
                        sb.Append(" -A");
                    else
                    {
                        if (!oSettings.CustomEncoderOptions.Contains("--he"))
                            if (oSettings.Profile == QaacProfile.HE)
                                sb.Append(" --he");

                        switch (oSettings.Mode)
                        {
                            case QaacMode.TVBR: sb.Append(" -V " + oSettings.Quality); break;
                            case QaacMode.CVBR: sb.Append(" -v " + oSettings.Bitrate); break;
                            case QaacMode.ABR: sb.Append(" -a " + oSettings.Bitrate); break;
                            case QaacMode.CBR: sb.Append(" -c " + oSettings.Bitrate); break;
                        }
                    }
                }

                if (!oSettings.CustomEncoderOptions.Contains("--no-delay"))
                    if (oSettings.NoDelay && oSettings.Profile == QaacProfile.LC) // To resolve some A/V sync issues 
                        sb.Append(" --no-delay");

                if (!String.IsNullOrEmpty(oSettings.CustomEncoderOptions))
                    sb.Append(" " + oSettings.CustomEncoderOptions.Trim());
                sb.Append(" - -o \"{0}\"");

            }
            else if (audioJob.Settings is ExhaleSettings)
            {
                UpdateCacher.CheckPackage("exhale");
                ExhaleSettings oSettings = audioJob.Settings as ExhaleSettings;
                _encoderExecutablePath = this._settings.Exhale.Path;
                _sendWavHeaderToEncoderStdIn = HeaderType.WAV;

                script.Append("32==Audiobits(last)?ConvertAudioTo24bit(last):last" + Environment.NewLine);

                switch (oSettings.Profile)
                {
                    case ExhaleProfile.xHEAAC: sb.Append(" " + oSettings.Quality); break;
                    case ExhaleProfile.xHEAACeSBR: 
                        switch (oSettings.Quality)
                        {
                            case 0: sb.Append(" a"); break;
                            case 1: sb.Append(" b"); break;
                            case 2: sb.Append(" c"); break;
                            case 3: sb.Append(" d"); break;
                            case 4: sb.Append(" e"); break;
                            case 5: sb.Append(" f"); break;
                            case 6: sb.Append(" g"); break;
                        }
                     break;
                }

                if (!String.IsNullOrEmpty(oSettings.CustomEncoderOptions))
                    sb.Append(" " + oSettings.CustomEncoderOptions.Trim());
                sb.Append(" \"{0}\"");
            }

            else if (audioJob.Settings is OpusSettings)
            {
                UpdateCacher.CheckPackage("opus");
                OpusSettings oSettings = audioJob.Settings as OpusSettings;
                _encoderExecutablePath = this._settings.Opus.Path;
                _sendWavHeaderToEncoderStdIn = HeaderType.WAV;

                script.Append("32==Audiobits(last)?ConvertAudioTo24bit(last):last" + Environment.NewLine);

                if (!oSettings.CustomEncoderOptions.Contains("--ignorelength"))
                    sb.Append(" --ignorelength");

                if (!oSettings.CustomEncoderOptions.Contains("--cvbr") && !oSettings.CustomEncoderOptions.Contains("--hard-cbr") && !oSettings.CustomEncoderOptions.Contains("--vbr"))
                {
                    switch (oSettings.Mode)
                    {
                        case OpusMode.CVBR: sb.Append(" --cvbr"); break;
                        case OpusMode.HCBR: sb.Append(" --hard-cbr"); break;
                        case OpusMode.VBR: sb.Append(" --vbr"); break;
                    }
                }
                if (!oSettings.CustomEncoderOptions.Contains("--bitrate "))
                    sb.Append(" --bitrate " + oSettings.Bitrate);

                if (!String.IsNullOrEmpty(oSettings.CustomEncoderOptions))
                    sb.Append(" " + oSettings.CustomEncoderOptions.Trim());

                sb.Append(" - \"{0}\"");
            }
            else if (audioJob.Settings is FDKAACSettings)
            {
                UpdateCacher.CheckPackage("fdkaac");
                FDKAACSettings oSettings = audioJob.Settings as FDKAACSettings;
                _encoderExecutablePath = this._settings.Fdkaac.Path;
                _sendWavHeaderToEncoderStdIn = HeaderType.WAV;

                script.Append("32==Audiobits(last)?ConvertAudioTo24bit(last):last" + Environment.NewLine);

                if (!oSettings.CustomEncoderOptions.Contains("--ignorelength"))
                    sb.Append(" --ignorelength");
                if (!oSettings.CustomEncoderOptions.Contains("-m "))
                {
                    switch (oSettings.Mode)
                    {
                        case FdkAACMode.CBR: // default
                            sb.Append(" -m 0");
                            if (!oSettings.CustomEncoderOptions.Contains("-b "))
                                sb.Append(" -b " + oSettings.Bitrate);
                            break;
                        case FdkAACMode.VBR:
                            sb.Append(" -m " + oSettings.Quality);
                            break;
                    }
                }
                if (!oSettings.CustomEncoderOptions.Contains("-p "))
                {
                    switch (oSettings.Profile)
                    {
                        case FdkAACProfile.M4LC: sb.Append(" -p 2"); break; // default
                        case FdkAACProfile.M4HE: sb.Append(" -p 5"); break;
                        case FdkAACProfile.M4LD: sb.Append(" -p 23"); break;
                        case FdkAACProfile.M4HE2: sb.Append(" -p 29"); break;
                        case FdkAACProfile.M4ELD: sb.Append(" -p 39"); break;
                    }
                }

                if (!String.IsNullOrEmpty(oSettings.CustomEncoderOptions))
                    sb.Append(" " + oSettings.CustomEncoderOptions.Trim());

                sb.Append(" - -o \"{0}\"");
            }
            else if (audioJob.Settings is FFAACSettings)
            {
                UpdateCacher.CheckPackage("ffmpeg");
                FFAACSettings oSettings = audioJob.Settings as FFAACSettings;
                _encoderExecutablePath = this._settings.FFmpeg.Path;
                _sendWavHeaderToEncoderStdIn = HeaderType.W64;

                script.Append("32==Audiobits(last)?ConvertAudioTo24bit(last):last" + Environment.NewLine); 

                sb.Append(" -hide_banner -i - -y");
                if (!oSettings.CustomEncoderOptions.Contains("-acodec ") && !oSettings.CustomEncoderOptions.Contains("-codec:a "))
                    sb.Append(" -codec:a aac");

                if (!oSettings.CustomEncoderOptions.Contains("-b:a ") && !oSettings.CustomEncoderOptions.Contains("-q:a "))
                {
                    switch (oSettings.Mode)
                    {
                        case FFAACMode.CBR: // default
                            sb.Append(" -b:a " + oSettings.Bitrate + "k");
                            break;
                        case FFAACMode.VBR:
                            sb.Append(" -q:a " + (Math.Max(oSettings.Quality / 50.0, 0.1)).ToString(CultureInfo.InvariantCulture)); // https://sourceforge.net/p/megui/bugs/957/
                            break;
                    }
                }

                if (!oSettings.CustomEncoderOptions.Contains("-profile:a "))
                {
                    switch (oSettings.Profile)
                    {
                        case FFAACProfile.M4LC: sb.Append(" -profile:a aac_low"); break; // default
                        case FFAACProfile.M4PNS: sb.Append(" -profile:a mpeg2_aac_low"); break;
                    }
                }

                if (!String.IsNullOrEmpty(oSettings.CustomEncoderOptions))
                    sb.Append(" " + oSettings.CustomEncoderOptions.Trim());
                sb.Append(" \"{0}\"");
            }

            _encoderCommandLine = sb.ToString().Trim();

            //Just check encoder existance
            _encoderExecutablePath = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, _encoderExecutablePath);
            if (!File.Exists(_encoderExecutablePath))
            {
                DeleteTempFiles();
                throw new EncoderMissingException(_encoderExecutablePath);
            }

            script.AppendLine(Environment.NewLine);
            script.AppendLine(@"return last");
            script.AppendLine(Environment.NewLine);

            // copy the appropriate functions at the end of the script
            if (iAVSChannelCount > 6)
            {
                string strPluginPath = Path.Combine(MainForm.Instance.Settings.AvisynthPluginsPath, "AudioLimiter.dll");
                if (File.Exists(strPluginPath))
                {
                    script.AppendLine(@"");
                }
                else
                {
                    // plugin not available (x64)
                    script.AppendLine(@"");
                }
            }

            switch (audioJob.Settings.DownmixMode)
            {
                case ChannelMode.KeepOriginal:
                    break;
                case ChannelMode.ConvertToMono:
                    break;
                case ChannelMode.Downmix51:
                    break;
                case ChannelMode.DPLDownmix:
                case ChannelMode.DPLIIDownmix:
                case ChannelMode.StereoDownmix:
                    script.AppendLine(@"");
                    break;
                case ChannelMode.Upmix:
                    script.AppendLine(@"
function x_upmix" + id + @"(clip a) 
 {
    m = ConvertToMono(a)
    a1 = GetLeftChannel(a)
    a2 = GetRightChannel(a)
    fl = SuperEQ(a1,""" + tmp + @"front.feq"")
    fr = SuperEQ(a2,""" + tmp + @"front.feq"")
    c = SuperEQ(m,""" + tmp + @"center.feq"") 
    lfe = SuperEQ(m,""" + tmp + @"lfe.feq"") 
    sl = SuperEQ(a1,""" + tmp + @"back.feq"")
    sr = SuperEQ(a2,""" + tmp + @"back.feq"")
    return MergeChannels(fl,fr,c,lfe,sl,sr)
 }");
                    break;
                case ChannelMode.UpmixUsingSoxEq:
                    script.AppendLine(@"
function x_upmixR" + id + @"(clip Stereo) 
 {
    Front = mixaudio(Stereo.SoxFilter(""filter 0-600""),mixaudio(Stereo.SoxFilter(""filter 600-1200""),Stereo.SoxFilter(""filter 1200-7000""),0.45,0.25),0.50,1)
    Back = mixaudio(Stereo.SoxFilter(""filter 0-600""),mixaudio(Stereo.SoxFilter(""filter 600-1200""),Stereo.SoxFilter(""filter 1200-7000""),0.35,0.15),0.40,1)
    fl = GetLeftChannel(Front)
    fr = GetRightChannel(Front)
    cc = ConvertToMono(stereo).SoxFilter(""filter 625-24000"")
    lfe = ConvertToMono(stereo).SoxFilter(""lowpass 100"",""vol -0.5"")
    sl = GetLeftChannel(Back)
    sr = GetRightChannel(Back)
    sl = DelayAudio(sl,0.02)
    sr = DelayAudio(sr,0.02)
    return MergeChannels(fl,fr,cc,lfe,sl,sr)
 }");
                    break;
                case ChannelMode.UpmixWithCenterChannelDialog:
                    script.AppendLine(@"
function x_upmixC" + id + @"(clip stereo) 
 {
    left = stereo.GetLeftChannel()
    right = stereo.GetRightChannel()
    fl = mixaudio(left.SoxFilter(""filter 0-24000""),right.SoxFilter(""filter 0-24000""),0.6,-0.5)
    fr = mixaudio(right.SoxFilter(""filter 0-24000""),left.SoxFilter(""filter 0-24000""),0.6,-0.5)
    cc = ConvertToMono(stereo).SoxFilter(""filter 625-24000"")
    lfe = ConvertToMono(stereo).SoxFilter(""lowpass 100"",""vol -0.5"")
    sl = mixaudio(left.SoxFilter(""filter 0-24000""),right.SoxFilter(""filter 0-24000""),0.5,-0.4)
    sr = mixaudio(right.SoxFilter(""filter 0-24000""),left.SoxFilter(""filter 0-24000""),0.5,-0.4)
    sl = DelayAudio(sl,0.02)
    sr = DelayAudio(sr,0.02)
     return MergeChannels(fl,fr,cc,lfe,sl,sr)                                                                                                                                              
 }");
                    break;
            }

            _avisynthAudioScript = script.ToString();

            _log.LogValue("AviSynth script", _avisynthAudioScript);
            _log.LogValue("Command line used", _encoderCommandLine);

            return true;
        }

        public void start()
        {
            try
            {
                this.Start();
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
                this.Abort();
            }
            catch (Exception e)
            {
                throw new JobRunException(e);
            }
        }

        public bool pause()
        {
            bool bResult = OSInfo.SuspendProcess(_encoderProcess);
            if (!_mre.Reset() || !bResult)
                return false;
            return true;
        }

        public bool resume()
        {
            bool bResult = OSInfo.ResumeProcess(_encoderProcess);
            if (!_mre.Set() || !bResult)
                return false;
            return true;
        }

        public void changePriority(WorkerPriorityType priority)
        {
            if (this._encoderThread == null || !_encoderThread.IsAlive)
                return;

            try
            {
                _encoderThread.Priority = OSInfo.GetThreadPriority(priority);
                WorkerPriority.GetJobPriority(audioJob, out WorkerPriorityType oPriority, out bool lowIOPriority);
                OSInfo.SetProcessPriority(_encoderProcess, priority, lowIOPriority, 0);
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