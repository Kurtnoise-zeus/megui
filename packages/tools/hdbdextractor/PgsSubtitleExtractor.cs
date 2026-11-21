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
using System.Threading;

using MeGUI.BdSub;
using MeGUI.core.util;

namespace MeGUI
{
    /// <summary>
    /// Extracts Blu-ray PGS subtitles (.sup) in-process (managed MPLS / M2TS demuxer), eac3to is not needed.
    /// The M2TS files must not be AACS encrypted.
    /// </summary>
    public class PgsSubtitleExtractor : ThreadJobProcessor<PgsExtractJob>
    {
        public static readonly JobProcessorFactory Factory = new JobProcessorFactory(new ProcessorFactory(init), "PgsSubtitleExtractor");

        private CancellationTokenSource cts;
        private double progressMax;

        private static IJobProcessor init(MainForm mf, Job j)
        {
            if (j is PgsExtractJob)
                return new PgsSubtitleExtractor();

            return null;
        }

        protected override void checkJobIO()
        {
            foreach (string strSource in Job.Source)
                Util.ensureExists(strSource);

            if (!String.IsNullOrEmpty(Job.Output))
                FileUtil.ensureDirectoryExists(Job.Output);
            foreach (PgsTrackRequest request in Job.Tracks)
                FileUtil.ensureDirectoryExists(Path.GetDirectoryName(request.Output));

            Su.Status = "Extracting PGS subtitles...";
        }

        protected override void RunInThread()
        {
            cts = new CancellationTokenSource();
            try
            {
                PgsExtractor extractor = new PgsExtractor(writeLog);
                extractor.Progress = p =>
                {
                    double pct = 100.0 * Math.Max(0, Math.Min(1, p));
                    if (pct > progressMax)
                    {
                        progressMax = pct;
                        Su.PercentageCurrent = (decimal)pct;
                    }
                    if (IsJobStopped()) // also blocks while the job is paused
                        cts.Cancel();
                };

                // the number and the language of the PGS streams of the disc
                string first = Job.Source[0];
                bool isPlaylist = Job.Source.Count == 1 && first.EndsWith(".mpls", StringComparison.OrdinalIgnoreCase);
                List<PgStreamInfo> streams = PgsExtractor.ListTracks(first);
                foreach (PgsTrackRequest request in Job.Tracks)
                {
                    if (request.Index >= 0 && request.Index < streams.Count)
                        log.LogEvent(String.Format("Track \"{0}\" ({1}) -> {2}", Path.GetFileName(request.Output), request.Language, streams[request.Index]));
                }

                PgsExtractionResult result = isPlaylist
                    ? extractor.ExtractPlaylist(first, Job.Tracks, cts.Token)
                    : extractor.ExtractFiles(Job.Source, Job.Tracks, cts.Token);

                if (cts.IsCancellationRequested)
                    Su.WasAborted = true;
                else if (!result.Success)
                    Su.HasError = true;
            }
            catch (Exception ex)
            {
                log.LogValue("Error: ", ex.ToString(), ImageType.Error);
                Su.HasError = true;
            }
            finally
            {
                cts.Dispose();
            }
        }

        private void writeLog(BdLogLevel level, string message)
        {
            switch (level)
            {
                case BdLogLevel.Error: log.LogEvent(message, ImageType.Error); break;
                case BdLogLevel.Warning: log.LogEvent(message, ImageType.Warning); break;
                default: log.LogEvent(message, ImageType.Information); break;
            }
        }
    }
}
