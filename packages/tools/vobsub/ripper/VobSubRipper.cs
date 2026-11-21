using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace MeGUI.VobSubRip
{
    /// <summary>
    /// Extraction of the subtitles of a VTS to an .idx/.sub pair.
    /// Port of <c>CVobSubFileRipper</c> (MPC-HC / VSRip) for VOB files which are already readable:
    /// CSS decryption and direct reading of a DVD drive are not supported.
    /// </summary>
    public sealed class VobSubRipper : IDisposable
    {
        private readonly string _ifoPath;
        private VobSequence _vob;

        public IfoInfo Info { get; private set; }
        public Action<LogLevel, string> Log { get; set; }
        /// <summary>Progress 0.0 -> 1.0. Called about every 512 KB read.</summary>
        public Action<double> Progress { get; set; }

        public VobSubRipper(string ifoPath, Action<LogLevel, string> log = null)
        {
            _ifoPath = ifoPath;
            Log = log;
            Info = IfoReader.Load(ifoPath, Emit);
        }

        private void Emit(LogLevel l, string m)
        {
            Action<LogLevel, string> log = Log;
            if (log != null) log(l, m);
        }

        private VobSequence OpenVobs()
        {
            if (_vob != null) return _vob;
            Emit(LogLevel.Info, "Searching vobs...");
            _vob = VobSequence.Open(_ifoPath, Emit);
            return _vob;
        }

        public void Dispose()
        {
            if (_vob != null) _vob.Dispose();
            _vob = null;
        }

        private struct Chunk
        {
            public long Start, End;
            public uint Vc;
            public Chunk(long start, long end, uint vc) { Start = start; End = end; Vc = vc; }
        }

        private static uint LE32(byte[] b, int o) { return unchecked((uint)(b[o] | (b[o + 1] << 8) | (b[o + 2] << 16) | (b[o + 3] << 24))); }

        /// <summary>
        /// Indexes the VOB files and writes <c>outputPath</c>.idx / .sub (a possible extension of <paramref name="outputPath"/> is removed).
        /// </summary>
        public RipResult Index(RipperOptions o, string outputPath, CancellationToken ct = default(CancellationToken))
        {
            RipResult result = new RipResult();

            if (o.PgcIndex < 0 || o.PgcIndex >= Info.Pgcs.Count)
            {
                Emit(LogLevel.Error, "Invalid program chain number (" + o.PgcIndex + ")!");
                return result;
            }
            Pgc pgc = Info.Pgcs[o.PgcIndex];

            if (o.Angle < 0 || o.Angle > 9 || pgc.Angles[o.Angle].Count == 0)
            {
                Emit(LogLevel.Error, "Invalid angle number (" + o.Angle + ")!");
                return result;
            }
            List<VobCell> angle = pgc.Angles[o.Angle];

            if (o.Streams.Count == 0 && !o.ClosedCaption)
            {
                Emit(LogLevel.Error, "No valid stream set to be extacted!");
                return result;
            }
            if (o.Cells.Count == 0)
            {
                Emit(LogLevel.Error, "No valid vob/cell id set to be extacted!");
                return result;
            }
            if (o.ClosedCaption)
                Emit(LogLevel.Warning, "Closed caption extraction is not implemented in this port, ignored.");

            VobSequence vob;
            try { vob = OpenVobs(); }
            catch (FileNotFoundException e) { Emit(LogLevel.Error, e.Message); return result; }

            Emit(LogLevel.Info, "Indexing...");

            string title = outputPath;
            string ext = Path.GetExtension(title);
            if (ext.Length > 0 && ext.Length <= 5) title = title.Substring(0, title.Length - ext.Length);

            MemoryStream sub = new MemoryStream(1024 * 1024);
            List<SubLang> langs = Enumerable.Range(0, 32).Select(_ => new SubLang()).ToList();

            long PTS = 0, tOffset = 0, tPrevOffset = 0, tTotal = 0, tStart = 0;
            int vobId = 0, cellId = 0;
            bool bDiscontinuity = false, bDiscontinuityFixApplied = false, bNavpackFound = false;
            long PTSframeoffset = 0, minPTSframeoffset = 0;

            if (o.ResetTime)
            {
                // first cell of the angle which is part of the selection
                int first = o.Cells.First();
                foreach (VobCell c in angle) if (o.Cells.Contains(c.Key)) { first = c.Key; break; }
                foreach (VobCell c in angle)
                {
                    if (c.Key == first) break;
                    tStart += c.Time;
                }
                Emit(LogLevel.Info, string.Format("Counting timestamps from {0}ms (v{1:00}c{2:00})", tStart, first >> 16, first & 0xffff));
            }

            Dictionary<int, long> selvcmap = new Dictionary<int, long>();
            foreach (int k in o.Cells) selvcmap[k] = long.MaxValue;

            List<Chunk> chunks = new List<Chunk>();
            List<Chunk> foundchunks = new List<Chunk>();
            List<Chunk> loadedchunks = new List<Chunk>();

            if (o.UseIfoCellRanges)
            {
                Emit(LogLevel.Info, "Indexing mode: DVD");
                foreach (VobCell c in angle)
                {
                    if (!selvcmap.ContainsKey(c.Key)) continue;
                    chunks.Add(new Chunk(2048L * c.StartSector, 2048L * c.EndSector + 2048, unchecked((uint)c.Key)));
                    Emit(LogLevel.Info, string.Format("Adding: 0x{0:x} - 0x{1:x} (lba) for vob {2} cell {3}", c.StartSector, c.EndSector, c.Vob, c.Cell));
                }
            }
            else if (!o.NoChunksCache && LoadChunks(vob, loadedchunks))
            {
                Emit(LogLevel.Info, "Indexing mode: File");
                foreach (Chunk c in loadedchunks)
                    if (selvcmap.ContainsKey(unchecked((int)c.Vc))) chunks.Add(c);
                Emit(LogLevel.Info, ".chunk file loaded");
            }
            else
            {
                Emit(LogLevel.Info, "Indexing mode: File");
                chunks.Add(new Chunk(0, 2048L * vob.Length, 0));
            }

            long sizedone = 0, sizetotal = chunks.Sum(c => c.End - c.Start);
            byte[] buff = new byte[2048];

            for (int i = 0; !ct.IsCancellationRequested && i < chunks.Count; i++)
            {
                long curpos = chunks[i].Start, endpos = chunks[i].End;
                Chunk curchunk = new Chunk(curpos, curpos, chunks[i].Vc);

                vob.Seek(curpos / 2048);
                for (; !ct.IsCancellationRequested && curpos < endpos; curpos += 2048, sizedone += 2048)
                {
                    if ((curpos & 0x7ffff) == 0 && Progress != null) Progress(sizetotal > 0 ? (double)sizedone / sizetotal : 0);

                    if (!vob.Read(buff))
                    {
                        Emit(LogLevel.Error, "Cannot read, either locked dvd or truncated/missing files!");
                        return result;
                    }

                    curchunk.End = curpos;

                    if ((buff[0x14] & 0x30) != 0)
                    {
                        Emit(LogLevel.Error, "Encrypted (CSS) sector found: decryption is not supported by this library, please unlock the VOBs with a DVD ripper first.");
                        return result;
                    }

                    if (LE32(buff, 0) != 0xba010000)
                    {
                        Emit(LogLevel.Warning, string.Format("Bad sector header at block {0:00000000}!", curpos / 2048));
                        if (!o.ContinueOnBadSector)
                        {
                            Emit(LogLevel.Error, "Terminated!");
                            return result;
                        }
                    }

                    bool hasPTS = false;
                    uint code0e = LE32(buff, 0x0e);

                    if ((code0e == 0xe0010000 || code0e == 0xbd010000) && (buff[0x15] & 0x80) != 0)
                    {
                        PTS = ((long)(buff[0x17] & 0x0e) << 29)
                            | ((long)buff[0x18] << 22)
                            | ((long)(buff[0x19] & 0xfe) << 14)
                            | ((long)buff[0x1a] << 7)
                            | ((long)buff[0x1b] >> 1);
                        hasPTS = true;
                    }

                    if (code0e == 0xbb010000)   // system header = nav pack
                    {
                        bNavpackFound = true;

                        if (vobId == buff[0x420] && cellId == buff[0x422]) continue;

                        vobId = buff[0x420];
                        cellId = buff[0x422];

                        tOffset = tTotal = 0;
                        foreach (VobCell c in angle)
                        {
                            if (c.Vob == vobId && c.Cell == cellId)
                            {
                                tPrevOffset = tOffset;
                                tOffset = c.TimeOffset;
                                tTotal = c.TimeTotal;
                                bDiscontinuity = c.Discontinuity;
                                bDiscontinuityFixApplied = false;
                                break;
                            }
                        }

                        uint vcNew = unchecked((uint)((vobId << 16) | cellId));
                        if (curchunk.Vc != vcNew)
                        {
                            if (curchunk.Vc != 0) foundchunks.Add(curchunk);
                            curchunk = new Chunk(curpos, curpos, vcNew);
                        }

                        string str = string.Format("v{0:00} c{1:00} lba{2:00000000}", vobId, cellId, curpos / 2048);
                        if (selvcmap.TryGetValue(unchecked((int)vcNew), out minPTSframeoffset))
                            str += string.Format(", total={0}ms, off={1}ms, corr={2}ms, discont.:{3}", tTotal, tOffset, -tStart, bDiscontinuity ? 1 : 0);
                        else
                            str += ", skipping";
                        Emit(LogLevel.Info, str);
                    }

                    int vcid = (vobId << 16) | cellId;
                    if (!selvcmap.TryGetValue(vcid, out minPTSframeoffset)) continue;

                    if (hasPTS && bDiscontinuity && !bDiscontinuityFixApplied)
                    {
                        long tDiff = tOffset - tPrevOffset;
                        if (tDiff > 0 && tDiff < (PTS / 90 + 1000))
                        {
                            Emit(LogLevel.Info, "False discontinuity detected, correcting time by " + (-tDiff) + "ms");
                            tStart += tDiff;
                        }
                        bDiscontinuityFixApplied = true;
                    }

                    if (code0e == 0xe0010000)   // video: used as the time reference
                    {
                        if (bDiscontinuity)
                        {
                            if (PTS < minPTSframeoffset) selvcmap[vcid] = PTSframeoffset = PTS;
                            bDiscontinuity = false;
                        }
                    }
                    else if (code0e == 0xbd010000)   // private stream 1
                    {
                        int id = buff[0x17 + buff[0x16]], iLang = id & 0x1f;

                        if ((id & 0xe0) == 0x20 && o.Streams.Contains(iLang))
                        {
                            if (hasPTS)
                            {
                                langs[iLang].Positions.Add(new SubPos
                                {
                                    FilePos = sub.Position,
                                    Start = tOffset + ((PTS - PTSframeoffset) / 90) - tStart,
                                    VobId = (byte)vobId,
                                    CellId = (byte)cellId,
                                    CellTimestamp = tTotal,
                                    Valid = true,
                                });
                            }
                            sub.Write(buff, 0, 2048);
                        }
                    }
                }

                // Intentional difference with MPC-HC: the last chunk is saved too (End is exclusive = end of the range).
                // Without this, the .chunks cache omits the last cell and a second extraction loses its subtitles.
                if (curchunk.Vc != 0 && !ct.IsCancellationRequested)
                {
                    curchunk.End = curpos;
                    foundchunks.Add(curchunk);
                }
            }

            if (ct.IsCancellationRequested)
            {
                Emit(LogLevel.Warning, "Terminated!");
                return result;
            }

            if (sizedone < sizetotal)
            {
                Emit(LogLevel.Error, "Indexing terminated before reaching the end!");
                if (Progress != null) Progress(0);
                return result;
            }

            if (!bNavpackFound)
            {
                Emit(LogLevel.Error, "Could not find any system header start code! (0x000001bb)");
                if (!o.UseIfoCellRanges) Emit(LogLevel.Error, "Make sure the ripper doesn't strip these packets.");
                if (Progress != null) Progress(0);
                return result;
            }

            Emit(LogLevel.Info, "Indexing finished");
            if (Progress != null) Progress(1);

            int nLang = -1;
            for (int i = 0; i < langs.Count; i++)
            {
                SubLang sl = langs[i];
                if (nLang < 0 && sl.Positions.Count > 0) nLang = i;
                sl.Id = pgc.StreamLanguageIds[i];
                sl.Name = sl.Alt = LangTable.FindName(sl.Id);
                sl.Positions = sl.Positions.OrderBy(p => p.Start).ToList();   // stable sort

                if (o.ForcedOnly)
                {
                    Emit(LogLevel.Info, "Searching for forced subs...");
                    if (Progress != null) Progress(0);
                    for (int j = 0; j < sl.Positions.Count; j++)
                    {
                        if (Progress != null) Progress((double)j / sl.Positions.Count);
                        SubPos p = sl.Positions[j];
                        p.Valid = false;
                        int packetSize, dataSize;
                        byte[] pk = GetPacket(sub, p.FilePos, i, out packetSize, out dataSize);
                        bool forced;
                        if (pk != null && SubPacket.GetInfo(pk, packetSize, dataSize, out forced))
                            p.Valid = forced;
                    }
                    if (Progress != null) Progress(1);
                }
            }

            Emit(LogLevel.Info, "Saving files...");

            if (nLang >= 0)
            {
                try
                {
                    string outDir = Path.GetDirectoryName(Path.GetFullPath(title + ".idx"));
                    if (!string.IsNullOrEmpty(outDir)) Directory.CreateDirectory(outDir);
                    IdxWriter.WriteIdx(title + ".idx", Info.Width, Info.Height, pgc.Palette, nLang, langs);
                    IdxWriter.WriteSub(title + ".sub", sub);
                    result.IdxPath = title + ".idx";
                    result.SubPath = title + ".sub";
                }
                catch (Exception e) when (e is IOException || e is UnauthorizedAccessException)
                {
                    Emit(LogLevel.Error, "Could not save output files!");
                    return result;
                }
                Emit(LogLevel.Info, "Subtitles saved");
            }
            else
            {
                Emit(LogLevel.Warning, "No subtitle packet found for the selected streams / cells: no output written.");
            }

            for (int i = 0; i < langs.Count; i++)
                if (langs[i].Positions.Count > 0) result.SubtitleCounts[i] = langs[i].Positions.Count(p => p.Valid);

            if (!o.UseIfoCellRanges && !o.NoChunksCache && loadedchunks.Count == 0)
            {
                if (SaveChunks(vob, foundchunks)) Emit(LogLevel.Info, ".chunk file saved");
            }

            Emit(LogLevel.Info, "Done!");
            result.Success = true;
            return result;
        }

        /// <summary>Rebuilds an SPU packet from the .sub being written (equivalent of <c>CVobSubFile::GetPacket</c>).</summary>
        private static byte[] GetPacket(MemoryStream sub, long filepos, int lang, out int packetSize, out int dataSize)
        {
            packetSize = dataSize = 0;
            byte[] buff = new byte[0x800];

            sub.Position = filepos;
            if (sub.Read(buff, 0, buff.Length) != buff.Length) return null;

            if (LE32(buff, 0) != 0xba010000
                || LE32(buff, 0x0e) != 0xbd010000
                || (buff[0x15] & 0x80) == 0
                || (buff[0x17] & 0xf0) != 0x20
                || (buff[buff[0x16] + 0x17] & 0xe0) != 0x20
                || (buff[buff[0x16] + 0x17] & 0x1f) != lang)
                return null;

            packetSize = (buff[buff[0x16] + 0x18] << 8) + buff[buff[0x16] + 0x19];
            dataSize = (buff[buff[0x16] + 0x1a] << 8) + buff[buff[0x16] + 0x1b];
            if (dataSize + 4 > packetSize) return null;

            byte[] ret = new byte[packetSize];
            int i = 0, sizeLeft = packetSize;
            while (i < packetSize)
            {
                int hsize = 0x18 + buff[0x16];
                int size = Math.Min(sizeLeft, 0x800 - hsize);
                Buffer.BlockCopy(buff, hsize, ret, i, size);
                i += size;
                sizeLeft -= size;

                if (sizeLeft > 0)
                {
                    bool ok = false;
                    while (sub.Read(buff, 0, buff.Length) == buff.Length)
                    {
                        if (buff[buff[0x16] + 0x17] == (lang | 0x20)) { ok = true; break; }
                    }
                    if (!ok) return null;
                }
            }
            return ret;
        }

        // ---- .chunks cache (VSRip compatible format: version=1, IFO checksum, VOB length, list of chunks) ----
        private string ChunksPath { get { return Path.ChangeExtension(_ifoPath, ".chunks"); } }

        private uint IfoChecksum()
        {
            using (FileStream f = File.OpenRead(_ifoPath))
            {
                uint sum = 0;
                byte[] b = new byte[4];
                while (f.Read(b, 0, 4) == 4) sum = unchecked(sum + LE32(b, 0));
                return sum;
            }
        }

        private bool LoadChunks(VobSequence vob, List<Chunk> chunks)
        {
            try
            {
                if (!File.Exists(ChunksPath)) return false;
                using (BinaryReader r = new BinaryReader(File.OpenRead(ChunksPath)))
                {
                    uint version = r.ReadUInt32();
                    if (version != 1) return false;
                    uint chk = r.ReadUInt32();
                    long voblen = r.ReadInt64();
                    uint n = r.ReadUInt32();
                    if (voblen != vob.Length || n > 10000000) return false;
                    List<Chunk> list = new List<Chunk>();
                    for (uint i = 0; i < n; i++) list.Add(new Chunk(r.ReadInt64(), r.ReadInt64(), r.ReadUInt32()));
                    if (chk != IfoChecksum()) return false;
                    chunks.AddRange(list);
                    return true;
                }
            }
            catch (Exception e) when (e is IOException || e is EndOfStreamException || e is UnauthorizedAccessException) { return false; }
        }

        private bool SaveChunks(VobSequence vob, List<Chunk> chunks)
        {
            try
            {
                using (BinaryWriter w = new BinaryWriter(File.Create(ChunksPath)))
                {
                    w.Write(1u);
                    w.Write(IfoChecksum());
                    w.Write(vob.Length);
                    w.Write((uint)chunks.Count);
                    foreach (Chunk c in chunks) { w.Write(c.Start); w.Write(c.End); w.Write(c.Vc); }
                }
                return true;
            }
            catch (Exception e) when (e is IOException || e is UnauthorizedAccessException) { return false; }
        }
    }
}
