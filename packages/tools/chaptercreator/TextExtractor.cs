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

namespace MeGUI
{
    public class TextExtractor : ChapterExtractor
    {
        public override bool SupportsMultipleStreams
        {
            get { return false; }
        }

        public override string[] Extensions
        {
            get { return new string[] { "txt" }; }
        }

        public override List<ChapterInfo> GetStreams(string location)
        {
            ChapterInfo oChapterInfo = new ChapterInfo();
            if (!oChapterInfo.LoadFile(location))
            {
                OnExtractionComplete();
                return new List<ChapterInfo>();
            }

            List<ChapterInfo> pgcs = new List<ChapterInfo>();
            pgcs.Add(oChapterInfo);

            OnStreamDetected(pgcs[0]);
            OnChaptersLoaded(pgcs[0]);
            OnExtractionComplete();
            return pgcs;
        }
    }
}