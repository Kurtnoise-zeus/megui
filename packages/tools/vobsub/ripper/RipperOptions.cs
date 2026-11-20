using System.Collections.Generic;

namespace MeGUI.VobSubRip
{
    /// <summary>Paramètres d'une extraction (équivalent de <c>VSFRipperData</c>).</summary>
    public sealed class RipperOptions
    {
        /// <summary>Index du PGC (0 = premier).</summary>
        public int PgcIndex { get; set; }
        /// <summary>Angle choisi (0 = toutes les cellules, 1..9 = angle).</summary>
        public int Angle { get; set; }
        /// <summary>Cellules retenues, clé = (vob &lt;&lt; 16) | cell. Vide = erreur « No valid vob/cell id ».</summary>
        public HashSet<int> Cells { get; } = new HashSet<int>();
        /// <summary>Numéros de flux de sous-titres (0..31) à extraire.</summary>
        public HashSet<int> Streams { get; } = new HashSet<int>();
        /// <summary>« Reset time at the first selected cell » : les timestamps partent de 0 à la première cellule retenue.</summary>
        public bool ResetTime { get; set; }
        /// <summary>Ne garde dans l'idx que les sous-titres forcés.</summary>
        public bool ForcedOnly { get; set; }
        /// <summary>Extraction du closed caption (line 21) : non portée dans cette version, l'option est ignorée avec un avertissement.</summary>
        public bool ClosedCaption { get; set; }
        /// <summary>
        /// true = lit uniquement les plages LBA des cellules sélectionnées (comportement « mode DVD » de VSRip).
        /// false = parcourt tout le fichier, avec cache .chunks (comportement « mode fichier »).
        /// </summary>
        public bool UseIfoCellRanges { get; set; }
        /// <summary>Continuer quand un secteur n'a pas d'en-tête MPEG-PS valide (VSRip demandait à l'utilisateur).</summary>
        public bool ContinueOnBadSector { get; set; } = true;
        /// <summary>Ne pas lire/écrire le cache .chunks.</summary>
        public bool NoChunksCache { get; set; }

        public static int CellKey(int vob, int cell) { return (vob << 16) | cell; }
    }

    public sealed class RipResult
    {
        public bool Success { get; internal set; }
        public string IdxPath { get; internal set; }
        public string SubPath { get; internal set; }
        /// <summary>Nombre de sous-titres indexés par numéro de flux.</summary>
        public Dictionary<int, int> SubtitleCounts { get; } = new Dictionary<int, int>();
    }
}
