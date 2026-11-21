using System;
using System.Collections.Generic;

namespace MeGUI.BdSub
{
    public enum BdLogLevel { Info, Warning, Error }

    /// <summary>PG stream (bitmap subtitles) declared in the STN_table of a PlayItem or in the PMT of an M2TS.</summary>
    public sealed class PgStreamInfo
    {
        /// <summary>Rank (0-based) among the PGS streams of the list.</summary>
        public int Index;
        public int Pid;
        /// <summary>ISO 639-2 language code (e.g. "eng"), empty if unknown.</summary>
        public string Language = "";
        public bool SubPath;

        public override string ToString()
        {
            return string.Format("PGS #{0}: PID 0x{1:X4}, {2}", Index + 1, Pid, string.IsNullOrEmpty(Language) ? "und" : Language);
        }
    }

    /// <summary>PlayItem of an MPLS playlist (times in 45 kHz ticks, like in the file).</summary>
    public sealed class MplsPlayItem
    {
        public string ClipName = "";
        public long InTime45;
        public long OutTime45;
        public int ConnectionCondition;
        public int AngleCount = 1;
        public List<PgStreamInfo> PgStreams = new List<PgStreamInfo>();

        public long In90 { get { return InTime45 * 2; } }
        public long Out90 { get { return OutTime45 * 2; } }
    }

    /// <summary>Raw PGS segment (without the "PG" header of the .sup format).</summary>
    public sealed class PgsSegment
    {
        public byte Type;
        public long Pts;
        public long Dts;
        public byte[] Data;
    }

    /// <summary>Extraction request of a PGS track.</summary>
    public sealed class PgsTrackRequest
    {
        /// <summary>Rank (0-based) of the PGS stream in the list of the disc (order of the STN_table / of the PMT).</summary>
        public int Index;
        public string Output = "";
        public string Language = "";
    }

    public sealed class PgsTrackResult
    {
        public int Index;
        public int Pid;
        public string Output = "";
        public int DisplaySets;
        public int SkippedSets;
    }

    public sealed class PgsExtractionResult
    {
        public bool Success;
        public List<PgsTrackResult> Tracks = new List<PgsTrackResult>();
    }

    public sealed class BdFormatException : Exception
    {
        public BdFormatException(string message) : base(message) { }
    }
}
