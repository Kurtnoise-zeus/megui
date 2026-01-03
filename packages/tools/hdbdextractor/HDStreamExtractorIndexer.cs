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
using System.Text;
using System.Text.RegularExpressions;

using MeGUI.core.util;

namespace MeGUI
{
    public class HDStreamExtractorIndexer: CommandlineJobProcessor<HDStreamsExJob>
    {
        public static readonly JobProcessorFactory Factory =
            new JobProcessorFactory(new ProcessorFactory(init), "HDStreamExtIndexer");

        private static IJobProcessor init(MainForm mf, Job j)
        {
            if (j is HDStreamsExJob) 
                return new HDStreamExtractorIndexer(mf.Settings.Eac3to.Path);
            return null;
        }

        public HDStreamExtractorIndexer(string executablePath)
        {
            UpdateCacher.CheckPackage("eac3to");
            this.Executable = executablePath;
        }

        /// <summary>
        /// gets the completion percentage
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        private decimal? getPercentage(string line)
        {
            try
            {
                if (Regex.IsMatch(line, "^process: [0-9]{1,3}%$", RegexOptions.Compiled))
                {
                    int pct = Int32.Parse(Regex.Match(line, "[0-9]{1,3}").Value);
                    return pct;
                }
                else if (Regex.IsMatch(line, "^analyze: [0-9]{1,3}%$", RegexOptions.Compiled))
                {
                    int pct = Int32.Parse(Regex.Match(line, "[0-9]{1,3}").Value);
                    return pct;
                }
                else 
                    return null;
            }
            catch (Exception e)
            {
                log.LogValue("Exception in getPercentage(" + line + ") ", e, ImageType.Warning);
                return null;
            }
        }

        protected override void checkJobIO()
        {
            foreach (string strSource in Job.Source)
                Util.ensureExists(strSource);
        }

        /// <summary>
        /// search for the log file and add error messages to the stdoutLog
        /// required as the error lines in stderr or stdout are not readable
        /// </summary>
        protected override void getErrorLine()
        {
            try
            {
                string[] entry = Regex.Split(Job.Args, "[0-9]{1,3}:\\\"");
                if (entry.Length < 2 || entry[1].Length < 3)
                    return;

                string strLogFile = entry[1].Split('\"')[0];
                strLogFile = FileUtil.AddToFileName(System.IO.Path.ChangeExtension(strLogFile, "txt"), " - Log");
                if (!System.IO.File.Exists(strLogFile))
                    return;

                using (System.IO.StreamReader file = new System.IO.StreamReader(strLogFile))
                {
                    string line;
                    while ((line = file.ReadLine()) != null)
                    {
                        if (line.ToLowerInvariant().Contains("<error>"))
                            StdoutLog.LogEvent(line, ImageType.Error);
                        else if (line.ToLowerInvariant().Contains("<warning>")
                            && !FileUtil.RegExMatch(line, @"\[\w\d{2}\] audio overlaps for ", true)
                            && !FileUtil.RegExMatch(line, @"\[\w\d{2}\] the video framerate is correct, but rather unusual", true))
                            StdoutLog.LogEvent(line, ImageType.Warning);
                        if (line.Contains("Getting \"Haali Matroska Muxer\" instance failed"))
                        {
                            // haali media splitter is missing ==> try to (re)install it
                            if (!Su.WasAborted && FileUtil.InstallHaali(ref log))
                                base.BSecondPassNeeded = true;
                        }
                    }
                }
            }
            catch (Exception)
            {
                StdoutLog.LogEvent("Error parsing: " + Job.Args, ImageType.Error);
            }
        }

        public override void ProcessLine(string line, StreamType stream, ImageType oType)
        {
            if (String.IsNullOrEmpty(line))
                return;
            
            if (line.StartsWith("process: ")) //status update
            {
                if (!base.BFirstPass)
                    Su.Status = "Fixing audio gaps/overlaps...";
                else
                    Su.Status = "Extracting Tracks...";
                Su.PercentageCurrent = getPercentage(line);
            }
            else if (line.StartsWith("analyze: "))
            {
                Su.Status = "Analyzing...";
            }
            else if (line.ToLowerInvariant().Contains("starting 2nd pass"))
            {
                base.BFirstPass = false;
                Su.Status = "Fixing audio gaps/overlaps...";
                base.ProcessLine(line, stream, oType);
            }
            else if (line.ToLowerInvariant().Contains("without making use of the gap/overlap information") 
                || line.ToLowerInvariant().Contains("2nd"))  // "catch all" for all lines with "2nd"
            {
                // "2nd pass will be necessary"
                // "will be stripped in 2nd pass"
                if (!base.BSecondPassNeeded)
                {
                    log.LogEvent("Job will be executed a second time to make use of the gap/overlap information");
                    base.BSecondPassNeeded = true;
                }
                base.ProcessLine(line, stream, oType);
            }
            else if (line.ToLowerInvariant().Contains("<error>"))
            {
                base.ProcessLine(line, stream, ImageType.Error);
            }
            else if (line.ToLowerInvariant().Contains("<warning>")
                || line.ToLowerInvariant().Contains("the wav file is bigger than 4gb")
                || line.ToLowerInvariant().Contains("some wav readers might not be able to handle this file correctly"))

            {
                base.ProcessLine(line, stream, ImageType.Warning);
            }
            else if (Su.PercentageCurrent > 0 && Su.PercentageCurrent < 100
                && !line.ToLowerInvariant().Contains("creating file ") 
                && !line.ToLowerInvariant().Contains("(seamless branching)...")
                && !line.ToLowerInvariant().Contains("applying aac delay...")
                && !line.ToLowerInvariant().Contains("muxing video to matroska... ")
                && !line.ToLowerInvariant().Contains("remaining delay of ")
                && !FileUtil.RegExMatch(line, @"\w\d{2} extracting \w+ track number \d+", true)
                )
            {
                base.ProcessLine(line, stream, ImageType.Warning);
            }
            else
                base.ProcessLine(line, stream, oType);
        }

        protected override string Commandline
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                if (Job.InputType == 1) // Folder as Input
                {
                    if (Job.Input.IndexOf("BDMV") > 0 && (Job.Input.ToLowerInvariant().EndsWith(".m2ts") || Job.Input.ToLowerInvariant().EndsWith(".mpls")))
                        sb.Append(string.Format("\"{0}\" {1}) {2}", Job.Input.Substring(0, Job.Input.IndexOf("BDMV")), Job.FeatureNb, Job.Args.Trim() + " -progressnumbers"));
                    else if (Job.Input.ToLowerInvariant().EndsWith(".evo"))
                        sb.Append(string.Format("\"{0}\" {1}) {2}", Job.Input.Substring(0, Job.Input.IndexOf("HVDVD_TS")), Job.FeatureNb, Job.Args.Trim() + " -progressnumbers"));
                    else
                        sb.Append(string.Format("\"{0}\" {1}) {2}", Job.Input, Job.FeatureNb,Job.Args.Trim() + " -progressnumbers"));
                }
                else
                {
                    if (Job.Source.Count == 0 && !String.IsNullOrEmpty(Job.Input))
                        Job.Source.Add(Job.Input);

                    string strSource = string.Format("\"{0}\"", Job.Source[0]);
                    for (int i = 1; i < Job.Source.Count; i++)
                        strSource += string.Format("+\"{0}\"", Job.Source[i]);
                    sb.Append(string.Format("{0} {1}", strSource, Job.Args.Trim() + " -progressnumbers"));
                }

                MatchCollection arrOutputFiles = Regex.Matches(Job.Args, "[0-9]:\".+?\"");
                foreach (Match oOutputFile in arrOutputFiles)
                {
                    string strOutputFile = oOutputFile.Groups[0].Value.Substring(3, oOutputFile.Groups[0].Value.Length - 4);
                    if (!Job.FilesToDelete.Contains(strOutputFile + ".gaps"))
                        Job.FilesToDelete.Add(strOutputFile + ".gaps");

                    string strLogFile = System.IO.Path.Combine(Job.Output, System.IO.Path.GetFileNameWithoutExtension(strOutputFile) + " - Log.txt");
                    if (!Job.FilesToDelete.Contains(strLogFile))
                        Job.FilesToDelete.Add(strLogFile);
                }

                return sb.ToString();
            }
        }
    }
}