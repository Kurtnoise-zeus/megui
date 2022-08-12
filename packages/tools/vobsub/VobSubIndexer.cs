// ****************************************************************************
// 
// Copyright (C) 2005-2022 Doom9 & al
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
            executable = MainForm.Instance.Settings.VobSubRipper.Path;
            bCommandLine = false;

            // success exit code is 2
            this.arrSuccessCodes = new System.Collections.Generic.List<int>() { 2 };
        }

        #region IJobProcessor Members
        protected override bool secondRunNeeded()
        {
            // independent of the iteration write the subtitles first
            writeSubtitles();

            if (job.ExtractForcedTracks && base.bFirstPass && !su.HasError && !su.WasAborted)
            {
                base.bSecondPassNeeded = true;
                generateConfigFile();
                su.Status = "Demuxing forced subtitles...";
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
            job.FilesToDelete.Add(configFile);
            job.FilesToDelete.Add(Path.ChangeExtension(job.Input, ".chunks"));

            // delete CC streams
            string strForcedFile = Path.Combine(Path.GetDirectoryName(job.Output), Path.GetFileNameWithoutExtension(job.Output) + "_forced.idx");

            job.FilesToDelete.Add(Path.ChangeExtension(job.Output, ".cc.raw"));
            job.FilesToDelete.Add(Path.ChangeExtension(job.Output, ".cc.srt"));
            job.FilesToDelete.Add(Path.ChangeExtension(job.Output, ".cc.utf16be.srt"));
            job.FilesToDelete.Add(Path.ChangeExtension(job.Output, ".cc.utf16le.srt"));
            job.FilesToDelete.Add(Path.ChangeExtension(job.Output, ".cc.utf8.srt"));
            
            job.FilesToDelete.Add(Path.ChangeExtension(strForcedFile, ".cc.raw"));
            job.FilesToDelete.Add(Path.ChangeExtension(strForcedFile, ".cc.srt"));
            job.FilesToDelete.Add(Path.ChangeExtension(strForcedFile, ".cc.utf16be.srt"));
            job.FilesToDelete.Add(Path.ChangeExtension(strForcedFile, ".cc.utf16le.srt"));
            job.FilesToDelete.Add(Path.ChangeExtension(strForcedFile, ".cc.utf8.srt"));

            if (!job.SingleFileExport)
            {
                job.FilesToDelete.Add(job.Output);
                job.FilesToDelete.Add(Path.ChangeExtension(job.Output, ".sub"));
                job.FilesToDelete.Add(strForcedFile);
                job.FilesToDelete.Add(Path.ChangeExtension(strForcedFile, ".sub"));
            }

            su.Status = "Demuxing subtitles...";
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
            script.AppendLine(job.Input);
            script.AppendLine(FileUtil.GetPathWithoutExtension(job.Output) + (base.bSecondPassNeeded ? "_forced" : String.Empty));
            script.AppendLine(job.PGC.ToString());
            if (job.Angle > 1)
                script.AppendLine(job.Angle.ToString());
            else
                script.AppendLine("1");
            if (!job.IndexAllTracks)
            {
                foreach (int id in job.TrackIDs)
                    script.Append(id + " ");
                script.AppendLine();
            }
            else
                script.AppendLine("ALL"); //process everything and strip down later
            if (base.bSecondPassNeeded)
                script.AppendLine("FORCEDONLY");
            script.AppendLine("RESETTIME");
            script.AppendLine("CLOSEIGNOREERRORS");
            script.AppendLine("CLOSE");

            // write the script to a temp file
            configFile = Path.ChangeExtension(job.Output, ".vobsub");
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
            string strInputFile = job.Output;
            if (!base.bFirstPass)
            {
                strInputFile = Path.Combine(Path.GetDirectoryName(strInputFile), Path.GetFileNameWithoutExtension(strInputFile) + "_forced.idx");
                string strSUBFile = Path.ChangeExtension(strInputFile, ".sub");
                FileInfo f = null;
                if (File.Exists(strSUBFile))
                    f = new FileInfo(strSUBFile);
                if (f == null || f.Length == 0)
                {
                    log.LogEvent("no forced subtitles found");
                    job.FilesToDelete.Add(strInputFile);
                    job.FilesToDelete.Add(strSUBFile);
                    return;
                }
            }

            if (job.SingleFileExport || !File.Exists(strInputFile))
                return;

            // multiple output files have to be generated based on the single input file
            su.Status = "Generating files...";

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
                            if (!job.IndexAllTracks)
                            {
                                foreach (int id in job.TrackIDs)
                                {
                                    if (index == id)
                                        bFound = true;
                                }
                            }

                            // export if found or if all tracks should be demuxed
                            if ((bFound && bFirstPass) || (bFound && !bFirstPass && bContentFound) || (job.IndexAllTracks && bContentFound))
                            {
                                // create output file
                                string outputFile = Path.Combine(Path.GetDirectoryName(job.Output), 
                                    Path.GetFileNameWithoutExtension(job.Output)) + "_" + index + "_" + strLanguage + (!base.bFirstPass ? "_forced" : string.Empty) + ".idx";
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