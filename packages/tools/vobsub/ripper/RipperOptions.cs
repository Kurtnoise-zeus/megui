using System.Collections.Generic;

namespace MeGUI.VobSubRip
{
    /// <summary>Settings of an extraction (equivalent of <c>VSFRipperData</c>).</summary>
    public sealed class RipperOptions
    {
        /// <summary>Index of the PGC (0 = first).</summary>
        public int PgcIndex { get; set; }
        /// <summary>Selected angle (0 = all the cells, 1..9 = angle).</summary>
        public int Angle { get; set; }
        /// <summary>Selected cells, key = (vob &lt;&lt; 16) | cell. Empty = "No valid vob/cell id" error.</summary>
        public HashSet<int> Cells { get; } = new HashSet<int>();
        /// <summary>Numbers of the subtitle streams (0..31) to extract.</summary>
        public HashSet<int> Streams { get; } = new HashSet<int>();
        /// <summary>"Reset time at the first selected cell": the timestamps start from 0 at the first selected cell.</summary>
        public bool ResetTime { get; set; }
        /// <summary>Keeps only the forced subtitles in the idx.</summary>
        public bool ForcedOnly { get; set; }
        /// <summary>Closed caption (line 21) extraction: not ported in this version, the option is ignored with a warning.</summary>
        public bool ClosedCaption { get; set; }
        /// <summary>
        /// true = reads only the LBA ranges of the selected cells ("DVD mode" behavior of VSRip).
        /// false = reads the whole file, with a .chunks cache ("file mode" behavior).
        /// </summary>
        public bool UseIfoCellRanges { get; set; }
        /// <summary>Continue when a sector has no valid MPEG-PS header (VSRip asked the user).</summary>
        public bool ContinueOnBadSector { get; set; } = true;
        /// <summary>Do not read/write the .chunks cache.</summary>
        public bool NoChunksCache { get; set; }

        public static int CellKey(int vob, int cell) { return (vob << 16) | cell; }
    }

    public sealed class RipResult
    {
        public bool Success { get; internal set; }
        public string IdxPath { get; internal set; }
        public string SubPath { get; internal set; }
        /// <summary>Number of indexed subtitles per stream number.</summary>
        public Dictionary<int, int> SubtitleCounts { get; } = new Dictionary<int, int>();
    }
}
