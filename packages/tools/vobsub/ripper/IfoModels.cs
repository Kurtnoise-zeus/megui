// Portage C# de la logique de VSRip / CVobSubFileRipper (MPC-HC, (C) 2003-2006 Gabest, (C) 2006-2017 voir Authors.txt de MPC-HC)
// Licence GPL v3 ou ultérieure, comme le code d'origine.

using System;
using System.Collections.Generic;
using System.Linq;

namespace MeGUI.VobSubRip
{
    /// <summary>Une cellule d'un PGC (équivalent de <c>vc_t</c> dans MPC-HC).</summary>
    public sealed class VobCell
    {
        public byte Vob;
        public byte Cell;
        /// <summary>Durée de la cellule en ms (corrigée par le facteur 30/29.97 pour le NTSC).</summary>
        public long Time;
        /// <summary>Décalage de temps (ms) à appliquer à partir de la dernière discontinuité.</summary>
        public long TimeOffset;
        /// <summary>Durée cumulée (ms) avant cette cellule.</summary>
        public long TimeTotal;
        /// <summary>Premier secteur (LBA, relatif au début de VTS_xx_1.VOB).</summary>
        public uint StartSector;
        /// <summary>Dernier secteur (LBA).</summary>
        public uint EndSector;
        /// <summary>0 = cellule commune à tous les angles, 1..9 = numéro d'angle.</summary>
        public int AngleIndex;
        public bool Discontinuity;

        public int Key { get { return (Vob << 16) | Cell; } }

        internal VobCell Clone() { return (VobCell)MemberwiseClone(); }
        public override string ToString()
        {
            return string.Format("V{0:00} C{1:00}  {2:hh\\:mm\\:ss\\.fff}  LBA {3}-{4}", Vob, Cell, TimeSpan.FromMilliseconds(Time), StartSector, EndSector);
        }
    }

    /// <summary>Un Program Chain (PGC) du VTS.</summary>
    public sealed class Pgc
    {
        public int NumPrograms;
        public int NumCells;
        /// <summary>Nombre d'angles détectés (0 ou 1 = pas de multi-angle).</summary>
        public int NumAngles;
        /// <summary>Angles[0] = toutes les cellules ; Angles[1..9] = cellules de chaque angle.</summary>
        public List<VobCell>[] Angles { get; } = Enumerable.Range(0, 10).Select(_ => new List<VobCell>()).ToArray();
        /// <summary>Palette d'origine du DVD, 16 couleurs au format 0xRRGGBB.</summary>
        public int[] Palette { get; } = new int[16];
        /// <summary>Codes langue (2 caractères ISO 639-1 packés sur 16 bits) indexés par numéro de flux de sous-titres (0..31).</summary>
        public ushort[] StreamLanguageIds { get; } = new ushort[32];
        /// <summary>Durée totale (ms) de l'angle 0.</summary>
        public long Duration { get { return Angles[0].Sum(c => c.Time); } }
    }

    /// <summary>Résultat de l'analyse d'un fichier VTS_xx_0.IFO.</summary>
    public sealed class IfoInfo
    {
        public string Path { get; internal set; } = "";
        public int Width { get; internal set; }
        public int Height { get; internal set; }
        /// <summary>true = PAL (25 i/s), false = NTSC.</summary>
        public bool IsPal { get; internal set; }
        /// <summary>Ratio d'affichage brut (0 = 4:3, 3 = 16:9).</summary>
        public int AspectRatio { get; internal set; }
        /// <summary>Nombre de flux de sous-titres déclarés dans le VTS.</summary>
        public int SubtitleStreamCount { get; internal set; }
        /// <summary>Codes langue des flux déclarés (0x2D2D = "--" si non défini).</summary>
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
