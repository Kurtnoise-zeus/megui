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
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

using MeGUI.core.util;

namespace MeGUI
{
    public class D2VIndexer : CommandlineJobProcessor<D2VIndexJob>
    {
        private static readonly Regex DGPercent =
            new Regex(@"\[(?<num>[0-9]*)%\]",
            RegexOptions.Compiled);

        private static readonly TimeSpan TwoSeconds = new TimeSpan(0, 0, 2);

        public static readonly JobProcessorFactory Factory =
            new JobProcessorFactory(new ProcessorFactory(init), "D2VIndexer");

        private static IJobProcessor init(MainForm mf, Job j)
        {
            if (j is D2VIndexJob) return new D2VIndexer(mf.Settings.DGIndex.Path);
            return null;
        }

        public D2VIndexer(string executableName)
        {
            UpdateCacher.CheckPackage("dgindex");
            Executable = executableName;
        }

        public override void ProcessLine(string line, StreamType stream, ImageType oType)
        {
            if (Regex.IsMatch(line, "^[0-9]{1,3}$", RegexOptions.Compiled))
                Su.PercentageCurrent = Int32.Parse(line);
            else
                base.ProcessLine(line, stream, oType);
        }

        protected override string Commandline
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                int idx = 0;
                string projName = Path.Combine(Path.GetDirectoryName(Job.Output), Path.GetFileNameWithoutExtension(Job.Output));
                if (MainForm.Instance.Settings.AutoLoadDG)
                    sb.Append("-SD=< -AIF=<" + Job.Input);
                else
                    sb.Append("-SD=< -IF=<" + Job.Input);
                if (Job.DemuxVideo)
                    sb.Append("< -OFD=<" + projName + "< -FO=0 -exit -hide");
                else 
                    sb.Append("< -OF=<" + projName + "< -FO=0 -exit -hide");
                if (Job.DemuxMode == 2)
                    sb.Append(" -OM=2"); // demux everything
                else if (Job.DemuxMode == 1)
                {
                    sb.Append(" -OM=1 -TN="); // demux only tracks checked
                    foreach (AudioTrackInfo ati in Job.AudioTracks)
                    {
                        if (idx > 0)
                             sb.Append("," + ati.DgIndexID);
                        else sb.Append(ati.DgIndexID);
                        ++idx;
                    }
                }
                else
                    sb.Append(" -OM=0"); // no audio demux
                return sb.ToString();
            }
        }

        protected override void checkJobIO()
        {
            try
            {
                if (!String.IsNullOrEmpty(Job.Output))
                    FileUtil.ensureDirectoryExists(Path.GetDirectoryName(Job.Output));
            }
            finally
            {
                base.checkJobIO();
            }
            Su.Status = "Creating DGV...";
        }
        
        protected override void doExitConfig()
        {
            if (MainForm.Instance.Settings.AutoForceFilm && !Su.HasError && !Su.WasAborted )
            {
                LogItem l = log.LogEvent("Running auto force film");
                double filmPercent;
                try
                {
                    filmPercent = d2vFile.GetFilmPercent(Job.Output);
                }
                catch (Exception error)
                {
                    l.LogValue("Exception opening file to apply force film", error, ImageType.Error);
                    Su.HasError = true;
                    return;
                }
                if (!Su.HasError)
                {
                    l.LogValue("Film percentage", filmPercent);
                    if (MainForm.Instance.Settings.ForceFilmThreshold <= (decimal)filmPercent)
                    {
                        bool success = applyForceFilm(Job.Output);
                        if (success)
                            l.LogEvent("Applied force film");
                        else
                        {
                            l.Error("Applying force film failed");
                            Su.HasError = true;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// opens a DGIndex project file and applies force film to it
        /// </summary>
        /// <param name="fileName">the dgindex project where force film is to be applied</param>
        private bool applyForceFilm(string fileName)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                using (StreamReader sr = new StreamReader(fileName))
                {
                    string line = null;
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (line.IndexOf("Field_Operation") != -1) // this is the line we have to replace
                            sb.Append("Field_Operation=1" + Environment.NewLine);
                        else if (line.IndexOf("Frame_Rate") != -1)
                        {
                            if (line.IndexOf("/") != -1) // If it has a slash, it means the framerate is signalled as a fraction, like below
                                sb.Append("Frame_Rate=23976 (24000/1001)" + Environment.NewLine);
                            else // If it doesn't, then it doesn't
                                sb.Append("Frame_Rate=23976" + Environment.NewLine);
                        }
                        else
                        {
                            sb.Append(line);
                            sb.Append(Environment.NewLine);
                        }
                    }
                }
                using (StreamWriter sw = new StreamWriter(fileName))
                {
                    sw.Write(sb.ToString());
                }
                return true;
            }
            catch (Exception e)
            {
                log.LogValue("Exception in applyForceFilm", e, ImageType.Error);
                return false;
            }
        }

        protected override void RunStatusCycle()
        {
            while (isRunning())
            {
                try
                {
                    if (Su.TimeElapsed<TwoSeconds) // give it some time to start up, otherwise MainWindowHandle remains null
                        return;
                    string text = Proc.MainWindowTitle;

                    Match m = DGPercent.Match(text);
                    if (m.Success)
                    {
                        Su.PercentageCurrent = int.Parse(m.Groups["num"].Value);
                    }
                }
                catch (Exception) { }
                    Thread.Sleep(1000);
            }
        }

    }
}
