//============================================================================
// BDInfo - Blu-ray Video and Audio Analysis Tool
// Copyright © 2010 Cinema Squid
//
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2.1 of the License, or (at your option) any later version.
//
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
//=============================================================================

using System;
using System.Collections.Generic;
using System.IO;
using BDInfo;

namespace MeGUI
{
    public class BDInfoExtractor : ChapterExtractor
    {
        public override string[] Extensions
        {
            get { return new string[] { "mpls" }; }
        }

        public override bool SupportsMultipleStreams
        {
            get { return false; }
        }

        private DirectoryInfo GetDirectoryBDMV(string path)
        {
            DirectoryInfo dir = new DirectoryInfo(path);

            while (dir != null)
            {
                if (dir.Name == "BDMV")
                {
                    return dir;
                }
                dir = dir.Parent;
            }

            return GetDirectory("BDMV", new DirectoryInfo(path), 0);
        }

        private DirectoryInfo GetDirectory(string name, DirectoryInfo dir, int searchDepth)
        {
            if (dir == null)
                return null;

            DirectoryInfo[] children = dir.GetDirectories();
            foreach (DirectoryInfo child in children)
            {
                if (child.Name == name)
                {
                    return child;
                }
            }
            if (searchDepth > 0)
            {
                foreach (DirectoryInfo child in children)
                {
                    GetDirectory(name, child, searchDepth - 1);
                }
            }

            return null;
        }

        public override List<ChapterInfo> GetStreams(string location)
        {
            ChapterInfo pgc = new ChapterInfo();
            pgc.SourceFilePath = location;
            pgc.Title = Path.GetFileNameWithoutExtension(location);
            pgc.SourceType = "Blu-Ray";

            DirectoryInfo DirectoryBDMV = GetDirectoryBDMV(location);
            if (DirectoryBDMV == null)
            {
                // Unable to locate BD structure
                OnExtractionComplete();
                return new List<ChapterInfo>();
            }

            DirectoryInfo DirectoryRoot =
                DirectoryBDMV.Parent;
            DirectoryInfo DirectoryBDJO =
                GetDirectory("BDJO", DirectoryBDMV, 0);
            DirectoryInfo DirectoryCLIPINF =
                GetDirectory("CLIPINF", DirectoryBDMV, 0);
            DirectoryInfo DirectoryPLAYLIST =
                GetDirectory("PLAYLIST", DirectoryBDMV, 0);
            DirectoryInfo DirectorySNP =
                GetDirectory("SNP", DirectoryRoot, 0);
            DirectoryInfo DirectorySTREAM =
                GetDirectory("STREAM", DirectoryBDMV, 0);
            DirectoryInfo DirectorySSIF =
                GetDirectory("SSIF", DirectorySTREAM, 0);

            Dictionary<string, TSStreamClipFile> StreamClipFiles = new Dictionary<string, TSStreamClipFile>();
            Dictionary<string, TSStreamFile> StreamFiles = new Dictionary<string, TSStreamFile>();
            if (DirectorySTREAM != null)
            {
                FileInfo[] files = DirectorySTREAM.GetFiles("*.m2ts");
                if (files.Length == 0)
                {
                    files = DirectoryPLAYLIST.GetFiles("*.M2TS");
                }
                foreach (FileInfo file in files)
                {
                    StreamFiles.Add(file.Name.ToUpper(), new TSStreamFile(file));
                }
            }

            if (DirectoryCLIPINF != null)
            {
                FileInfo[] files = DirectoryCLIPINF.GetFiles("*.clpi");
                if (files.Length == 0)
                {
                    files = DirectoryPLAYLIST.GetFiles("*.CLPI");
                }
                foreach (FileInfo file in files)
                {
                    StreamClipFiles.Add(file.Name.ToUpper(), new TSStreamClipFile(file));
                }
            }

            FileInfo fileInfo = new FileInfo(location);
            TSPlaylistFile mpls = new TSPlaylistFile(fileInfo);
            mpls.Scan(StreamFiles, StreamClipFiles);

            int count = 1;
            foreach (double d in mpls.Chapters)
            {
                pgc.Chapters.Add(new Chapter()
                {
                    Name = "Chapter " + count.ToString("D2"),
                    Time = new TimeSpan((long)(d * (double)TimeSpan.TicksPerSecond))
                });
                count++;
            }

            pgc.Duration = new TimeSpan((long)(mpls.TotalLength * (double)TimeSpan.TicksPerSecond));

            foreach (TSStreamClip clip in mpls.StreamClips)
            {
                clip.StreamClipFile.Scan();
                foreach (TSStream stream in clip.StreamClipFile.Streams.Values)
                {
                    if (stream.IsVideoStream)
                    {
                        pgc.FramesPerSecond = VideoUtil.ConvertFPSFractionToDouble(((TSVideoStream)stream).FrameRateEnumerator, ((TSVideoStream)stream).FrameRateDenominator);
                        break;
                    }
                }
                if (pgc.FramesPerSecond != 0) 
                    break;
            }

            OnStreamDetected(pgc);
            OnChaptersLoaded(pgc);
            OnExtractionComplete();

            return new List<ChapterInfo>() { pgc };
        }
    }
}