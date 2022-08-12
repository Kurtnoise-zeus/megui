// ****************************************************************************
// 
// Copyright (C) 2009  Jarrett Vance
// 
// code from http://jvance.com/pages/ChapterGrabber.xhtml
// 
// ****************************************************************************

using System.Collections.Generic;
using System.Linq;
using System.IO;

using MeGUI.core.util;

namespace MeGUI
{
    public class BlurayExtractor : ChapterExtractor
    {
        public override string[] Extensions
        {
            get { return new string[] { }; }
        }

        public override List<ChapterInfo> GetStreams(string location)
        {
            List<ChapterInfo> mpls = new List<ChapterInfo>();

            string path = FileUtil.GetBlurayPath(location);
            if (!Directory.Exists(path))
                return mpls;

            ChapterExtractor ex = new BDInfoExtractor();
            ex.StreamDetected += (sender, args) => OnStreamDetected(args.ProgramChain);
            ex.ChaptersLoaded += (sender, args) => OnChaptersLoaded(args.ProgramChain);

            foreach (string file in Directory.GetFiles(path, "*.mpls"))
            {
                ChapterInfo pl = ex.GetStreams(file)[0];
                if (pl == null)
                    continue;

                pl.SourceFilePath = file;
                pl.SourceType = "Blu-Ray";
                mpls.Add(pl);
            }

            mpls = mpls.OrderByDescending(p => p.Duration).ToList();
            OnExtractionComplete();
            return mpls;
        }
    }
}