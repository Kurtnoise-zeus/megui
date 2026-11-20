using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MeGUI.VobSubRip
{
    /// <summary>
    /// Séquence de fichiers VTS_xx_n.VOB lue comme un flux continu de secteurs de 2048 octets.
    /// Équivalent (sans la partie lecteur DVD / CSS) de <c>CVobFile</c> de MPC-HC.
    /// Le secteur logique 0 est le début de VTS_xx_1.VOB : le fichier VTS_xx_0.VOB (menu), s'il existe,
    /// est placé en tête de séquence puis masqué par un décalage, exactement comme dans MPC-HC.
    /// </summary>
    public sealed class VobSequence : IDisposable
    {
        public const int SectorSize = 2048;

        private readonly List<Tuple<string, long>> _files = new List<Tuple<string, long>>();   // (chemin, secteurs)
        private long _size;     // taille totale en secteurs (menu inclus)
        private long _offset;   // secteurs du VTS_xx_0.VOB
        private long _pos;      // position absolue courante (secteurs)
        private int _iFile = -1;
        private FileStream _stream;

        public IList<string> Files { get { return _files.Select(f => f.Item1).ToList(); } }
        /// <summary>Longueur utile en secteurs (hors menu).</summary>
        public long Length { get { return _size - _offset; } }
        public long Position { get { return _pos - _offset; } }

        /// <summary>Recherche VTS_xx_0.VOB ... VTS_xx_99.VOB à partir du chemin de l'IFO.</summary>
        public static VobSequence Open(string ifoPath, Action<LogLevel, string> log = null)
        {
            if (log == null) log = delegate { };
            VobSequence seq = new VobSequence();

            string dir = Path.GetDirectoryName(Path.GetFullPath(ifoPath));
            if (string.IsNullOrEmpty(dir)) dir = ".";
            string stem = Path.GetFileNameWithoutExtension(ifoPath).TrimEnd('.', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9'); // "VTS_01_"
            Dictionary<string, string> existing = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            foreach (string f in Directory.EnumerateFiles(dir)) existing[Path.GetFileName(f)] = f;

            long offset = -1;
            List<Tuple<string, long>> found = new List<Tuple<string, long>>();
            for (int i = 0; i < 100; i++)
            {
                string vob;
                if (!existing.TryGetValue(stem + i + ".vob", out vob))
                {
                    if (i == 0) continue;
                    break;
                }

                long size = new FileInfo(vob).Length;
                if ((size & 0x7ff) != 0) { found.Clear(); break; }   // taille non multiple de 2048 : séquence rejetée
                if (size > 0) found.Add(Tuple.Create(vob, size / SectorSize));
                if (i == 0) offset = size / SectorSize;
            }

            if (found.Count == 0)
            {
                log(LogLevel.Error, "Nothing found! (" + Path.Combine(dir, stem) + "*.vob)");
                throw new FileNotFoundException("Cannot open vob sequence");
            }

            foreach (var f in found) log(LogLevel.Info, "Found " + f.Item1);

            seq._files.AddRange(found);
            seq._size = found.Sum(f => f.Item2);
            seq._offset = found.Count == 1 ? 0 : Math.Max(offset, 0);
            return seq;
        }

        private FileStream OpenFile(int i)
        {
            return new FileStream(_files[i].Item1, FileMode.Open, FileAccess.Read, FileShare.ReadWrite, 1 << 16, FileOptions.SequentialScan);
        }

        /// <summary>Se place sur le secteur logique <paramref name="lba"/> (0 = début de VTS_xx_1.VOB).</summary>
        public bool Seek(long lba)
        {
            long pos = Math.Min(Math.Max(lba + _offset, _offset), _size - 1);

            int i = -1;
            long size = 0;
            do { size += _files[++i].Item2; } while (i < _files.Count - 1 && pos >= size);

            if (i != _iFile)
            {
                if (_stream != null) _stream.Dispose();
                _stream = OpenFile(i);
                _iFile = i;
            }

            _pos = pos;
            _stream.Seek((pos - (size - _files[i].Item2)) * SectorSize, SeekOrigin.Begin);
            return true;
        }

        /// <summary>Lit le secteur suivant dans <paramref name="sector"/> (2048 octets au moins). Retourne false en fin de séquence ou si le fichier est tronqué.</summary>
        public bool Read(byte[] sector)
        {
            if (sector == null || sector.Length < SectorSize) throw new ArgumentException("buffer < 2048 octets", "sector");
            if (_pos >= _size) return false;

            if (_stream == null || _stream.Position >= _stream.Length)
            {
                if (_stream != null) _stream.Dispose();
                _stream = null;
                if (_iFile >= _files.Count - 1) return false;
                _stream = OpenFile(++_iFile);
            }

            int got = 0;
            while (got < SectorSize)
            {
                int n = _stream.Read(sector, got, SectorSize - got);
                if (n <= 0) return false;
                got += n;
            }
            _pos++;
            return true;
        }

        public void Dispose()
        {
            if (_stream != null) _stream.Dispose();
            _stream = null;
        }
    }
}
