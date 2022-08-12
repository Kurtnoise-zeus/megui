// ****************************************************************************
// 
// Copyright (C) 2009  Jarrett Vance
// 
// code from http://jvance.com/pages/ChapterGrabber.xhtml
// 
// ****************************************************************************

using System;
using System.Collections.Generic;
using System.IO;

using MeGUI.core.util;

namespace MeGUI
{
    public class IfoExtractor : ChapterExtractor
    {
        public override string[] Extensions
        {
            get { return new string[] { "ifo" }; }
        }

        public override bool SupportsMultipleStreams
        {
            get { return false; }
        }

        public override List<ChapterInfo> GetStreams(string ifoFile)
        {
            List<ChapterInfo> oList = new List<ChapterInfo>();

            int pgcCount = IFOparser.GetPGCCount(ifoFile);
            for (int i = 1; i <= pgcCount; i++)
            { 
                int iAngleCount = IFOparser.GetAngleCount(ifoFile, i);
                for (int a = 0; a <= iAngleCount; a++)
                {
                    if (a == 0 && iAngleCount > 0)
                        continue;
                    ChapterInfo oChapterInfo = GetChapterInfo(ifoFile, i);
                    oChapterInfo.AngleNumber = a;
                    oList.Add(oChapterInfo);
                } 
            }
            return oList;
        }

        public ChapterInfo GetChapterInfo(string strIFOFile, int iPGC)
        {
            TimeSpan duration;
            double fps;

            int iTitle = 0;
            string strFileName = Path.GetFileNameWithoutExtension(strIFOFile);
            if (FileUtil.RegExMatch(strFileName, @"\AVTS_\d{2}_\d{1}\z", true))
                iTitle = int.Parse(strFileName.Split('_')[1]);

            ChapterInfo pgc = new ChapterInfo();
            pgc.SourceType = "DVD";
            pgc.SourceFilePath = strIFOFile;
            pgc.TitleNumber = iTitle;
            pgc.PGCNumber = iPGC;
            pgc.Title = Path.GetFileNameWithoutExtension(strIFOFile);
            if (iTitle > 0)
                pgc.Title = "VTS_" + iTitle.ToString("D2");
            pgc.Chapters = GetChapters(strIFOFile, iPGC, out duration, out fps);
            pgc.Duration = duration;
            pgc.FramesPerSecond = fps;

            OnStreamDetected(pgc);
            OnExtractionComplete();

            return pgc;
        }

        List<Chapter> GetChapters(string ifoFile, int programChain, out TimeSpan duration, out double fps)
        {
            List<Chapter> chapters = new List<Chapter>();
            duration = TimeSpan.Zero;
            fps = 0;
      
            long pcgITPosition = IFOparser.GetPCGIP_Position(ifoFile);
            int programChainPrograms = -1;
            TimeSpan programTime = TimeSpan.Zero;
            if (programChain >= 0)
            {
                double FPS;
                uint chainOffset = IFOparser.GetPGCOffset(ifoFile, pcgITPosition, programChain);
                programTime = IFOparser.ReadTimeSpan(ifoFile, pcgITPosition, chainOffset, out FPS) ?? TimeSpan.Zero;
                programChainPrograms = IFOparser.GetNumberOfPrograms(ifoFile, pcgITPosition, chainOffset);
            }
            else
            {
                int programChains = IFOparser.GetPGCCount(ifoFile, pcgITPosition);
                for (int curChain = 1; curChain <= programChains; curChain++)
                {
                    double FPS;
                    uint chainOffset = IFOparser.GetPGCOffset(ifoFile, pcgITPosition, curChain);
                    TimeSpan? time = IFOparser.ReadTimeSpan(ifoFile, pcgITPosition, chainOffset, out FPS);
                    if (time == null)
                        break;

                    if (time.Value > programTime)
                    {
                        programChain = curChain;
                        programChainPrograms = IFOparser.GetNumberOfPrograms(ifoFile, pcgITPosition, chainOffset);
                        programTime = time.Value;
                    }
                }
            }
            if (programChain < 0)
                return null;

            chapters.Add(new Chapter() { Name = "Chapter 01" });

            uint longestChainOffset = IFOparser.GetPGCOffset(ifoFile, pcgITPosition, programChain);
            int programMapOffset = IFOparser.ToInt16(IFOparser.GetFileBlock(ifoFile, (pcgITPosition + longestChainOffset) + 230, 2));
            int cellTableOffset = IFOparser.ToInt16(IFOparser.GetFileBlock(ifoFile, (pcgITPosition + longestChainOffset) + 0xE8, 2));
            for (int currentProgram = 0; currentProgram < programChainPrograms; ++currentProgram)
            {
                int entryCell = IFOparser.GetFileBlock(ifoFile, ((pcgITPosition + longestChainOffset) + programMapOffset) + currentProgram, 1)[0];
                int exitCell = entryCell;
                if (currentProgram < (programChainPrograms - 1))
                    exitCell = IFOparser.GetFileBlock(ifoFile, ((pcgITPosition + longestChainOffset) + programMapOffset) + (currentProgram + 1), 1)[0] - 1;

                TimeSpan totalTime = TimeSpan.Zero;
                for (int currentCell = entryCell; currentCell <= exitCell; currentCell++)
                {
                    int cellStart = cellTableOffset + ((currentCell - 1) * 0x18);
                    byte[] bytes = IFOparser.GetFileBlock(ifoFile, (pcgITPosition + longestChainOffset) + cellStart, 4);
                    int cellType = bytes[0] >> 6;
                    if (cellType == 0x00 || cellType == 0x01)
                    {
                        bytes = IFOparser.GetFileBlock(ifoFile, ((pcgITPosition + longestChainOffset) + cellStart) + 4, 4);
                        TimeSpan time = IFOparser.ReadTimeSpan(bytes, out fps) ?? TimeSpan.Zero;
                        totalTime += time;
                    }
                }
        
                //add a constant amount of time for each chapter?
                //totalTime += TimeSpan.FromMilliseconds(fps != 0 ? (double)1000 / fps / 8D : 0);

                duration += totalTime;
                if (currentProgram + 1 < programChainPrograms)
                    chapters.Add(new Chapter() { Name = string.Format("Chapter {0:D2}", currentProgram + 2), Time = duration });        
            }
            return chapters;
        }
    }
}