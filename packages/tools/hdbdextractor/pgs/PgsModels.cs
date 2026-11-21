using System;
using System.Collections.Generic;

namespace MeGUI.BdSub
{
    public enum BdLogLevel { Info, Warning, Error }

    /// <summary>Flux PG (sous-titres bitmap) déclaré dans la STN_table d'un PlayItem ou dans la PMT d'un M2TS.</summary>
    public sealed class PgStreamInfo
    {
        /// <summary>Rang (0-based) parmi les flux PGS de la liste.</summary>
        public int Index;
        public int Pid;
        /// <summary>Code langue ISO 639-2 (ex. "eng"), vide si inconnu.</summary>
        public string Language = "";
        public bool SubPath;

        public override string ToString()
        {
            return string.Format("PGS #{0}: PID 0x{1:X4}, {2}", Index + 1, Pid, string.IsNullOrEmpty(Language) ? "und" : Language);
        }
    }

    /// <summary>PlayItem d'une playlist MPLS (temps en ticks de 45 kHz, comme dans le fichier).</summary>
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

    /// <summary>Segment PGS brut (sans l'en-tête "PG" du format .sup).</summary>
    public sealed class PgsSegment
    {
        public byte Type;
        public long Pts;
        public long Dts;
        public byte[] Data;
    }

    /// <summary>Demande d'extraction d'une piste PGS.</summary>
    public sealed class PgsTrackRequest
    {
        /// <summary>Rang (0-based) du flux PGS dans la liste du disque (ordre de la STN_table / de la PMT).</summary>
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
