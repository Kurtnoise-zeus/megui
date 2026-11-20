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
using System.IO;
using System.Text;
using System.Threading;

using MeGUI.core.util;
using MeGUI.VobSubRip;

namespace MeGUI
{
    /// <summary>
    /// Extracts DVD subtitles (VobSub idx/sub) from a VTS IFO file.
    /// The extraction is done in-process by the managed VobSubRip library (a port of the VSRip / MPC-HC ripper),
    /// vsrip.exe is no longer required. The VOB files must not be CSS scrambled.
    /// </summary>
    public class VobSubIndexer : ThreadJobProcessor<SubtitleIndexJob>
    {
        public static readonly JobProcessorFactory Factory = new JobProcessorFactory(new ProcessorFactory(init), "VobSubIndexer");

        private CancellationTokenSource cts;
        private double progressFrom, progressTo, progressMax;

        private static IJobProcessor init(MainForm mf, Job j)
        {
            if (j is SubtitleIndexJob)
                return new VobSubIndexer();

            return null;
        }

        #region IJobProcessor Members
        protected override void checkJobIO()
        {
            base.checkJobIO();
            FileUtil.ensureDirectoryExists(Path.GetDirectoryName(Job.Output));

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

        protected override void RunInThread()
        {
            cts = new CancellationTokenSource();
            try
            {
                bool bForcedPass = Job.ExtractForcedTracks;

                // first pass: the selected subtitle tracks
                progressFrom = 0;
                progressTo = bForcedPass ? 50 : 100;
                if (!runPass(true))
                {
                    Su.HasError = !Su.WasAborted;
                    return;
                }

                // independent of the iteration write the subtitles first
                writeSubtitles(true);

                // second pass: forced subtitles only
                if (bForcedPass && !Su.HasError && !Su.WasAborted)
                {
                    Su.Status = "Demuxing forced subtitles...";
                    progressFrom = 50;
                    progressTo = 100;
                    if (!runPass(false))
                    {
                        Su.HasError = !Su.WasAborted;
                        return;
                    }
                    writeSubtitles(false);
                }
            }
            catch (Exception ex)
            {
                log.LogValue("Error: ", ex.Message, ImageType.Error);
                Su.HasError = true;
            }
            finally
            {
                cts.Dispose();
            }
        }
        #endregion

        /// <summary>
        /// runs the extraction for the selected tracks (first pass) or for the forced subtitles only (second pass)
        /// </summary>
        /// <returns>true if the extraction succeeded</returns>
        private bool runPass(bool bFirstPass)
        {
            string outputBase = FileUtil.GetPathWithoutExtension(Job.Output) + (bFirstPass ? String.Empty : "_forced");
            progressMax = 0;

            try
            {
                using (VobSubRipper ripper = new VobSubRipper(Job.Input, writeLog))
                {
                    ripper.Progress = p =>
                    {
                        // the forced subtitle search restarts at 0%, never go backwards
                        double pct = progressFrom + (progressTo - progressFrom) * Math.Max(0, Math.Min(1, p));
                        if (pct > progressMax)
                        {
                            progressMax = pct;
                            Su.PercentageCurrent = (decimal)pct;
                        }
                        if (IsJobStopped()) // also blocks while the job is paused
                            cts.Cancel();
                    };

                    RipperOptions options = createOptions(ripper.Info, bFirstPass);
                    log.LogValue("VobSub extraction", String.Format("PGC {0}, angle {1}, {2}, forced only: {3}",
                        Job.PGC, options.Angle, Job.IndexAllTracks ? "all tracks" : "tracks " + String.Join(", ", Job.TrackIDs), !bFirstPass));

                    RipResult result = ripper.Index(options, outputBase, cts.Token);
                    if (!result.Success && !Su.WasAborted)
                        log.LogEvent("Subtitle extraction failed", ImageType.Error);
                    return result.Success;
                }
            }
            catch (IfoFormatException ex)
            {
                log.LogEvent(ex.Message, ImageType.Error);
                return false;
            }
        }

        private RipperOptions createOptions(IfoInfo ifo, bool bFirstPass)
        {
            RipperOptions options = new RipperOptions();
            options.PgcIndex = Job.PGC - 1;
            options.Angle = Job.Angle > 1 ? Job.Angle : 1;
            options.ResetTime = true;
            options.ForcedOnly = !bFirstPass;
            options.NoChunksCache = true; // do not write a .chunks cache file into the source folder

            if (Job.IndexAllTracks)
            {
                // process everything and strip down later
                for (int i = 0; i < 32; i++)
                    options.Streams.Add(i);
            }
            else
            {
                foreach (int id in Job.TrackIDs)
                    options.Streams.Add(id);
            }

            // all the cells of the selected angle
            if (options.PgcIndex >= 0 && options.PgcIndex < ifo.Pgcs.Count && options.Angle <= 9)
            {
                foreach (VobCell cell in ifo.Pgcs[options.PgcIndex].Angles[options.Angle])
                    options.Cells.Add(cell.Key);
            }
            return options;
        }

        private void writeLog(MeGUI.VobSubRip.LogLevel level, string message)
        {
            switch (level)
            {
                case MeGUI.VobSubRip.LogLevel.Error: log.LogEvent(message, ImageType.Error); break;
                case MeGUI.VobSubRip.LogLevel.Warning: log.LogEvent(message, ImageType.Warning); break;
                default: log.LogEvent(message, ImageType.Information); break;
            }
        }

        /// <summary>
        /// extracts the subtitle tracks in multiple files if needed
        /// </summary>
        private void writeSubtitles(bool bFirstPass)
        {
            string strInputFile = Job.Output;
            if (!bFirstPass)
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
                            if ((bFound && bFirstPass) || (bFound && !bFirstPass && bContentFound) || (Job.IndexAllTracks && bContentFound))
                            {
                                // create output file
                                string outputFile = Path.Combine(Path.GetDirectoryName(Job.Output), 
                                    Path.GetFileNameWithoutExtension(Job.Output)) + "_" + index + "_" + strLanguage + (!bFirstPass ? "_forced" : string.Empty) + ".idx";
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
