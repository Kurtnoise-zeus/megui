// C# port of the VSRip / CVobSubFileRipper logic (MPC-HC, (C) 2003-2006 Gabest, (C) 2006-2017 see the MPC-HC Authors.txt)
// GPL v3 or later license, like the original code.

using System;
using System.Collections.Generic;
using System.Linq;

namespace MeGUI.VobSubRip
{
    /// <summary>A cell of a PGC (equivalent of <c>vc_t</c> in MPC-HC).</summary>
    public sealed class VobCell
    {
        public byte Vob;
        public byte Cell;
        /// <summary>Duration of the cell in ms (corrected by the 30/29.97 factor for NTSC).</summary>
        public long Time;
        /// <summary>Time offset (ms) to apply from the last discontinuity.</summary>
        public long TimeOffset;
        /// <summary>Accumulated duration (ms) before this cell.</summary>
        public long TimeTotal;
        /// <summary>First sector (LBA, relative to the start of VTS_xx_1.VOB).</summary>
        public uint StartSector;
        /// <summary>Last sector (LBA).</summary>
        public uint EndSector;
        /// <summary>0 = cell common to all angles, 1..9 = angle number.</summary>
        public int AngleIndex;
        public bool Discontinuity;

        public int Key { get { return (Vob << 16) | Cell; } }

        internal VobCell Clone() { return (VobCell)MemberwiseClone(); }
        public override string ToString()
        {
            return string.Format("V{0:00} C{1:00}  {2:hh\\:mm\\:ss\\.fff}  LBA {3}-{4}", Vob, Cell, TimeSpan.FromMilliseconds(Time), StartSector, EndSector);
        }
    }

    /// <summary>A Program Chain (PGC) of the VTS.</summary>
    public sealed class Pgc
    {
        public int NumPrograms;
        public int NumCells;
        /// <summary>Number of detected angles (0 or 1 = no multi-angle).</summary>
        public int NumAngles;
        /// <summary>Angles[0] = all the cells; Angles[1..9] = cells of each angle.</summary>
        public List<VobCell>[] Angles { get; } = Enumerable.Range(0, 10).Select(_ => new List<VobCell>()).ToArray();
        /// <summary>Original palette of the DVD, 16 colors in 0xRRGGBB format.</summary>
        public int[] Palette { get; } = new int[16];
        /// <summary>Language codes (2 ISO 639-1 characters packed in 16 bits) indexed by subtitle stream number (0..31).</summary>
        public ushort[] StreamLanguageIds { get; } = new ushort[32];
        /// <summary>Total duration (ms) of angle 0.</summary>
        public long Duration { get { return Angles[0].Sum(c => c.Time); } }
    }

    /// <summary>Result of the analysis of a VTS_xx_0.IFO file.</summary>
    public sealed class IfoInfo
    {
        public string Path { get; internal set; } = "";
        public int Width { get; internal set; }
        public int Height { get; internal set; }
        /// <summary>true = PAL (25 fps), false = NTSC.</summary>
        public bool IsPal { get; internal set; }
        /// <summary>Raw display aspect ratio (0 = 4:3, 3 = 16:9).</summary>
        public int AspectRatio { get; internal set; }
        /// <summary>Number of subtitle streams declared in the VTS.</summary>
        public int SubtitleStreamCount { get; internal set; }
        /// <summary>Language codes of the declared streams (0x2D2D = "--" if undefined).</summary>
        public ushort[] SubtitleLanguageIds { get; internal set; } = new ushort[32];
        public List<Pgc> Pgcs { get; } = new List<Pgc>();

        public static string LanguageCode(ushort id) { return new string(new[] { (char)(id >> 8), (char)(id & 0xff) }); }
        public static string LanguageName(ushort id) { return LangTable.FindName(id); }
    }

    public enum LogLevel { Info, Warning, Error }

    public sealed class IfoFormatException : Exception
    {
        public IfoFormatException(string message, Exception inner = null) : base(message, inner) { }
    }
}
