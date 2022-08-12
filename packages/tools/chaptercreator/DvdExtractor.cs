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

using System.Collections.Generic;
using System.IO;
using System.Diagnostics;

using MeGUI.core.util;

namespace MeGUI
{
    public class DvdExtractor : ChapterExtractor
    {
        public override string[] Extensions
        {
            get { return new string[] { }; }
        }

        public override List<ChapterInfo> GetStreams(string location)
        {
            List<ChapterInfo> streams = new List<ChapterInfo>();

            string videoIFO = FileUtil.GetDVDPath(location);
            if (string.IsNullOrEmpty(videoIFO))
                return streams;
            
            IfoExtractor ex = new IfoExtractor();
            ex.StreamDetected += (sender, args) => OnStreamDetected(args.ProgramChain);
            ex.ChaptersLoaded += (sender, args) => OnChaptersLoaded(args.ProgramChain);

            if (File.Exists(videoIFO) && Path.GetFileName(videoIFO).ToUpperInvariant().Equals("VIDEO_TS.IFO"))
            {
                byte[] bytRead = new byte[4];
                long VMG_PTT_STPT_Position = IFOparser.ToFilePosition(IFOparser.GetFileBlock(videoIFO, 0xC4, 4));
                int titlePlayMaps = IFOparser.ToInt16(IFOparser.GetFileBlock(videoIFO, VMG_PTT_STPT_Position, 2));

                // get PGC count from all ifo files
                int pgcCount = 0;
                foreach (string file in Directory.GetFiles(Path.GetDirectoryName(videoIFO), "VTS_*_0.IFO"))
                    pgcCount += IFOparser.GetPGCCount(file);

                if (pgcCount > titlePlayMaps)
                {
                    // process all the ifo files as there are more PGCs than in the VIDEO_TS.IFO
                    foreach (string file in Directory.GetFiles(Path.GetDirectoryName(videoIFO), "VTS_*_0.IFO"))
                        streams.AddRange(ex.GetStreams(file));
                }
                else
                {
                    for (int currentTitle = 1; currentTitle <= titlePlayMaps; ++currentTitle)
                    {
                        long titleInfoStart = 8 + ((currentTitle - 1) * 12);
                        int titleSetNumber = IFOparser.GetFileBlock(videoIFO, (VMG_PTT_STPT_Position + titleInfoStart) + 6L, 1)[0];
                        int titleSetTitleNumber = IFOparser.GetFileBlock(videoIFO, (VMG_PTT_STPT_Position + titleInfoStart) + 7L, 1)[0];
                        string vtsIFO = Path.Combine(Path.GetDirectoryName(videoIFO), string.Format("VTS_{0:D2}_0.IFO", titleSetNumber));
                        if (!File.Exists(vtsIFO))
                        {
                            Trace.WriteLine(string.Format("VTS IFO file missing: {0}", Path.GetFileName(vtsIFO)));
                            continue;
                        }
                        int iAngleCount = IFOparser.GetAngleCount(vtsIFO, titleSetTitleNumber);
                        for (int i = 0; i <= iAngleCount; i++)
                        {
                            if (i == 0 && iAngleCount > 0)
                                continue;
                            ChapterInfo oChapterInfo = ex.GetChapterInfo(vtsIFO, titleSetTitleNumber);
                            oChapterInfo.AngleNumber = i;
                            streams.Add(oChapterInfo);
                        }
                    }
                }
            }
            else if (File.Exists(videoIFO))
            {
                // read only the selected ifo file
                streams.AddRange(ex.GetStreams(videoIFO));
            }
            else
            {
                // read all the ifo files
                foreach (string file in Directory.GetFiles(Path.GetDirectoryName(videoIFO), "VTS_*_0.IFO"))
                    streams.AddRange(ex.GetStreams(file));
            }

            OnExtractionComplete();
            return streams;
        }
    }
}