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
using System.IO;
using System.Text;

using MeGUI.core.util;

namespace MeGUI
{
    public class VobSubIndexer : CommandlineJobProcessor<SubtitleIndexJob>
    {
        public static readonly JobProcessorFactory Factory = new JobProcessorFactory(new ProcessorFactory(init), "VobSubIndexer");

        private string configFile = null;

        private static IJobProcessor init(MainForm mf, Job j)
        {
            if (j is SubtitleIndexJob)
                return new VobSubIndexer();

            return null;
        }

        public VobSubIndexer()
        {
            UpdateCacher.CheckPackage("vsrip");
            Executable = MainForm.Instance.Settings.VobSubRipper.Path;
            BCommandLine = false;

            // success exit code is 2
            this.ArrSuccessCodes = new System.Collections.Generic.List<int>() { 2 };
        }

        #region IJobProcessor Members
        protected override bool secondRunNeeded()
        {
            // independent of the iteration write the subtitles first
            writeSubtitles();

            if (Job.ExtractForcedTracks && base.BFirstPass && !Su.HasError && !Su.WasAborted)
            {
                base.BSecondPassNeeded = true;
                generateConfigFile();
                Su.Status = "Demuxing forced subtitles...";
            }

            return base.secondRunNeeded();
        }

        protected override string Commandline
        {
            get
            {
                return "\"" + configFile + "\"";
            }
        }

        protected override void checkJobIO()
        {
            base.checkJobIO();
            generateConfigFile();
            Util.ensureExists(configFile);
            Job.FilesToDelete.Add(configFile);
            Job.FilesToDelete.Add(Path.ChangeExtension(Job.Input, ".chunks"));

            // delete CC streams
            string strForcedFile = Path.Combine(Path.GetDirectoryName(Job.Output), Path.GetFileNameWithoutExtension(Job.Output) + "_forced.idx");

            Job.FilesToDelete.Add(Path.ChangeExtension(Job.Output, ".cc.raw"));
            Job.FilesToDelete.Add(Path.ChangeExtension(Job.Output, ".cc.srt"));
            Job.FilesToDelete.Add(Path.ChangeExtension(Job.Output, ".cc.utf16be.srt"));
            Job.FilesToDelete.Add(Path.ChangeExtension(Job.Output, ".cc.utf16le.srt"));
            Job.FilesToDelete.Add(Path.ChangeExtension(Job.Output, ".cc.utf8.srt"));
            
            Job.FilesToDelete.Add(Path.ChangeExtension(strForcedFile, ".cc.raw"));
            Job.FilesToDelete.Add(Path.ChangeExtension(strForcedFile, ".cc.srt"));
            Job.FilesToDelete.Add(Path.ChangeExtension(strForcedFile, ".cc.utf16be.srt"));
            Job.FilesToDelete.Add(Path.ChangeExtension(strForcedFile, ".cc.utf16le.srt"));
            Job.FilesToDelete.Add(Path.ChangeExtension(strForcedFile, ".cc.utf8.srt"));

            if (!Job.SingleFileExport)
            {
                Job.FilesToDelete.Add(Job.Output);
                Job.FilesToDelete.Add(Path.ChangeExtension(Job.Output, ".sub"));
                Job.FilesToDelete.Add(strForcedFile);
                Job.FilesToDelete.Add(Path.ChangeExtension(strForcedFile, ".sub"));
            }

            Su.Status = "Demuxing subtitles...";
        }

        protected override bool hideProcess
        {
            get { return true; }
        }

        #endregion

        /// <summary>
        /// writes the config file for vsrip
        /// </summary>
        private void generateConfigFile()
        {
            // create the configuration script
            StringBuilder script = new StringBuilder();
            script.AppendLine(Job.Input);
            script.AppendLine(FileUtil.GetPathWithoutExtension(Job.Output) + (base.BSecondPassNeeded ? "_forced" : String.Empty));
            script.AppendLine(Job.PGC.ToString());
            if (Job.Angle > 1)
                script.AppendLine(Job.Angle.ToString());
            else
                script.AppendLine("1");
            if (!Job.IndexAllTracks)
            {
                foreach (int id in Job.TrackIDs)
                    script.Append(id + " ");
                script.AppendLine();
            }
            else
                script.AppendLine("ALL"); //process everything and strip down later
            if (base.BSecondPassNeeded)
                script.AppendLine("FORCEDONLY");
            script.AppendLine("RESETTIME");
            script.AppendLine("CLOSEIGNOREERRORS");
            script.AppendLine("CLOSE");

            // write the script to a temp file
            configFile = Path.ChangeExtension(Job.Output, ".vobsub");
            FileUtil.ensureDirectoryExists(Path.GetDirectoryName(configFile));
            using (StreamWriter output = new StreamWriter(configFile, false, Encoding.Default))
                output.Write(script.ToString());

            log.LogValue("VobSub configuration file", script);
        }

        /// <summary>
        /// extracts the subtitle tracks in multiple files if needed
        /// </summary>
        private void writeSubtitles()
        {
            string strInputFile = Job.Output;
            if (!base.BFirstPass)
            {
                strInputFile = Path.Combine(Path.GetDirectoryName(strInputFile), Path.GetFileNameWithoutExtension(strInputFile) + "_forced.idx");
                string strSUBFile = Path.ChangeExtension(strInputFile, ".sub");
                FileInfo f = null;
                if (File.Exists(strSUBFile))
                    f = new FileInfo(strSUBFile);
                if (f == null || f.Length == 0)
                {
                    log.LogEvent("no forced subtitles found");
                    Job.FilesToDelete.Add(strInputFile);
                    Job.FilesToDelete.Add(strSUBFile);
                    return;
                }
            }

            if (Job.SingleFileExport || !File.Exists(strInputFile))
                return;

            // multiple output files have to be generated based on the single input file
            Su.Status = "Generating files...";

            string line;
            string strLanguage = string.Empty;
            bool bHeader = true; // same header for all output files
            bool bWait = false;
            bool bContentFound = false;
            StringBuilder sbHeader = new StringBuilder();
            StringBuilder sbIndex = new StringBuilder();
            StringBuilder sbIndexTemp = new StringBuilder();
            int index = 0;

            using (StreamReader file = new StreamReader(strInputFile))
            {
                while ((line = file.ReadLine()) != null)
                {
                    if (bHeader)
                    {
                        if (line.StartsWith("langidx:"))
                        {
                            // first subtitle track detected
                            bHeader = false;
                            bWait = true;
                        }
                        else
                            sbHeader.AppendLine(line);
                    }
                    else if (bWait)
                    {
                        sbIndexTemp.AppendLine(line);
                        if (line.StartsWith("id: "))
                        {
                            // new track detected
                            index = Int32.Parse(line.Substring(line.LastIndexOf(' ')));
                            strLanguage = line.Substring(line.IndexOf(' ') + 1, line.LastIndexOf(',') - line.IndexOf(' ') - 1);
                            strLanguage = LanguageSelectionContainer.LookupISOCode(strLanguage);

                            // create full output text
                            sbIndex.Clear();
                            sbIndex.Append(sbHeader.ToString());
                            sbIndex.AppendLine("langidx: " + index);
                            sbIndex.Append(sbIndexTemp.ToString());
                            bWait = false;
                            bContentFound = false;
                        }
                    }
                    else
                    {
                        if (String.IsNullOrEmpty(line))
                        {
                            bWait = true;

                            // check if the track found in the input file is selected to be demuxed
                            bool bFound = false;
                            if (!Job.IndexAllTracks)
                            {
                                foreach (int id in Job.TrackIDs)
                                {
                                    if (index == id)
                                        bFound = true;
                                }
                            }

                            // export if found or if all tracks should be demuxed
                            if ((bFound && BFirstPass) || (bFound && !BFirstPass && bContentFound) || (Job.IndexAllTracks && bContentFound))
                            {
                                // create output file
                                string outputFile = Path.Combine(Path.GetDirectoryName(Job.Output), 
                                    Path.GetFileNameWithoutExtension(Job.Output)) + "_" + index + "_" + strLanguage + (!base.BFirstPass ? "_forced" : string.Empty) + ".idx";
                                using (StreamWriter output = new StreamWriter(outputFile))
                                    output.WriteLine(sbIndex.ToString());

                                outputFile = Path.ChangeExtension(outputFile, ".sub");
                                File.Copy(Path.ChangeExtension(strInputFile, ".sub"), outputFile, true);

                                log.LogEvent("Subtitle file created: " + Path.GetFileName(outputFile));
                            }

                            sbIndexTemp.Clear();
                            sbIndexTemp.AppendLine(line);
                        }
                        else
                        {
                            if (line.StartsWith("timestamp:"))
                                bContentFound = true;
                            sbIndex.AppendLine(line);
                        }
                    }
                }
            }
        }

    }
}