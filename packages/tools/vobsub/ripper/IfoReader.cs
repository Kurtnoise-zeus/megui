using System;
using System.IO;
using System.Text;

namespace MeGUI.VobSubRip
{
    /// <summary>
    /// Parser of a Video Title Set IFO file. Equivalent of <c>CVobSubFileRipper::LoadIfo</c>.
    /// The offsets are the ones of the DVD-Video format (see also the disassembly of VSRip.exe, 0x40B2xx-0x40C354):
    /// 0x200 video attributes, 0x254 subtitles (count, then 6 bytes per stream), 0xCC VTS_PGCIT pointer.
    /// </summary>
    public static class IfoReader
    {
        public static IfoInfo Load(string path, Action<LogLevel, string> log = null)
        {
            if (log == null) log = delegate { };

            FileInfo fi = new FileInfo(path);
            if (!fi.Exists || fi.Length == 0)
            {
                log(LogLevel.Error, "Invalid ifo");
                throw new IfoFormatException("Invalid ifo");
            }

            byte[] d;
            try { d = File.ReadAllBytes(path); }
            catch (Exception e) when (e is IOException || e is UnauthorizedAccessException)
            {
                log(LogLevel.Error, "Cannot open ifo");
                throw new IfoFormatException("Cannot open ifo", e);
            }

            log(LogLevel.Info, "Opening ifo OK");

            if (d.Length < 12 || Encoding.ASCII.GetString(d, 0, 12) != "DVDVIDEO-VTS")
            {
                log(LogLevel.Error, "Not a Video Title Set IFO file!");
                throw new IfoFormatException("Not a Video Title Set IFO file!");
            }

            try
            {
                IfoInfo info = Parse(d);
                info.Path = path;
                log(LogLevel.Info, "Parsing ifo OK");
                return info;
            }
            catch (Exception e) when (e is ArgumentOutOfRangeException || e is IndexOutOfRangeException || e is InvalidDataException || e is OverflowException)
            {
                log(LogLevel.Error, "Invalid ifo");
                throw new IfoFormatException("Invalid ifo", e);
            }
        }

        // --- big-endian reading with bounds checking ---
        private static int U8(byte[] d, long p) { return d[checked((int)p)]; }
        private static int U16(byte[] d, long p) { return (U8(d, p) << 8) | U8(d, p + 1); }
        private static uint U32(byte[] d, long p) { return ((uint)U16(d, p) << 16) | (uint)U16(d, p + 2); }
        private static int Clamp(double v) { return (int)Math.Max(0, Math.Min(255, v)); }

        private static IfoInfo Parse(byte[] d)
        {
            IfoInfo info = new IfoInfo();

            // ---- subtitle languages (VTS_SPST_ATRT) ----
            ushort[] ids = new ushort[32];
            int len = U16(d, 0x254);
            info.SubtitleStreamCount = len;
            for (int i = 0; i < len && i < 32; i++)
            {
                long p = 0x256 + i * 6;           // 2 bytes of attributes, 2 bytes of language code, 2 bytes of extension
                ids[i] = (ushort)U16(d, p + 2);
                if (ids[i] == 0) ids[i] = 0x2D2D; // '--'
            }
            info.SubtitleLanguageIds = (ushort[])ids.Clone();

            // ---- video attributes ----
            // MPC-HC reads the 2 raw bytes into a bit field (little-endian): byte0 = d[0x200], byte1 = d[0x201].
            int b0 = U8(d, 0x200), b1 = U8(d, 0x201);
            int system = (b0 >> 4) & 3;           // 0 = NTSC, 1 = PAL
            int aspect = (b0 >> 2) & 3;
            int sourceRes = (b1 >> 2) & 3;
            int[,,] res =
            {
                { { 720, 480 }, { 720, 576 } },
                { { 704, 480 }, { 704, 576 } },
                { { 352, 480 }, { 352, 576 } },
                { { 352, 240 }, { 352, 288 } },
            };
            info.Width = res[sourceRes, system & 1, 0];
            info.Height = res[sourceRes, system & 1, 1];
            info.IsPal = (system & 1) == 1;
            info.AspectRatio = aspect;
            double rate = system == 0 ? 30.0 / 29.97 : 1.0;

            // ---- PGC ----
            long pgcpos = (long)U32(d, 0xC0 + 0x0C) * 0x800;
            int nPgc = U16(d, pgcpos);

            for (int i = 0; i < nPgc; i++)
            {
                Pgc pgc = new Pgc();
                long offset = pgcpos + U32(d, pgcpos + 8 + i * 8L + 4);

                pgc.NumPrograms = U8(d, offset + 2);
                int nCells = pgc.NumCells = U8(d, offset + 3);

                // stream number (4:3 / wide) -> language mapping, from SPST_CTL (offset 0x1C, 32 x 4 bytes)
                Array.Copy(ids, pgc.StreamLanguageIds, 32);
                bool anyId = false;
                for (int j = 0; j < 32; j++)
                {
                    if (U8(d, offset + 0x1C + j * 4 + 1) != 0 || U8(d, offset + 0x1C + j * 4 + 2) != 0) { anyId = true; break; }
                }
                if (anyId)
                {
                    for (int j = 0; j < 32; j++)
                    {
                        if ((U8(d, offset + 0x1C + j * 4) & 0x80) == 0) break;
                        int id1 = U8(d, offset + 0x1C + j * 4 + 1) & 0x1F;
                        int id2 = U8(d, offset + 0x1C + j * 4 + 2) & 0x1F;
                        pgc.StreamLanguageIds[id1] = ids[j];
                        pgc.StreamLanguageIds[id2] = ids[j];
                    }
                }

                // YUV palette (offset 0xA4, 16 x [0,Y,Cr,Cb]) -> RGB
                for (int j = 0; j < 16; j++)
                {
                    long p = offset + 0xA4 + j * 4;
                    int y = U8(d, p + 1), u = U8(d, p + 2), v = U8(d, p + 3);
                    // MPC-HC: y = (y - 16) * 255 / 219 stored in a BYTE; the value is clamped here instead of overflowing.
                    double yy = Clamp((y - 16) * 255 / 219);
                    int r = Clamp(yy + 1.4022 * (u - 128));
                    int g = Clamp(yy - 0.3456 * (u - 128) - 0.7145 * (v - 128));
                    int b = Clamp(yy + 1.7710 * (v - 128));
                    pgc.Palette[j] = (r << 16) | (g << 8) | b;
                }

                int celladdroff = U16(d, offset + 0xE8);
                int vobcelloff = U16(d, offset + 0xEA);

                var all = pgc.Angles[0];
                for (int j = 0; j < nCells; j++)
                {
                    long p = offset + vobcelloff + j * 4L;
                    all.Add(new VobCell
                    {
                        Vob = (byte)U16(d, p),          // MPC-HC keeps the low byte of the (BE16) identifier
                        Cell = (byte)U16(d, p + 2),
                    });
                }

                long tOffset = 0, tTotal = 0;
                int iAngle = 0;
                pgc.NumAngles = 0;

                for (int j = 0; j < nCells; j++)
                {
                    long p = offset + celladdroff + j * 24L;
                    VobCell c = all[j];
                    int cat = U8(d, p);
                    switch (cat >> 6)
                    {
                        case 0: iAngle = 0; break;   // normal
                        case 1: iAngle = 1; break;   // first angle block
                        case 2: iAngle++; break;     // intermediate angle block
                        case 3: iAngle++; break;     // last angle block
                    }
                    c.AngleIndex = iAngle;
                    pgc.NumAngles = Math.Max(pgc.NumAngles, iAngle);

                    uint t = U32(d, p + 4);          // playback time in BCD + 2 bits of fps
                    c.StartSector = U32(d, p + 8);
                    c.EndSector = U32(d, p + 20);

                    double fps = ((t >> 6) & 3) == 1 ? 25 : 30;
                    int hh = (int)(((t >> 28) & 0xf) * 10 + ((t >> 24) & 0xf));
                    int mm = (int)(((t >> 20) & 0xf) * 10 + ((t >> 16) & 0xf));
                    int ss = (int)(((t >> 12) & 0xf) * 10 + ((t >> 8) & 0xf));
                    int ms = (int)(1000.0 * (((t >> 4) & 0x3) * 10 + ((t >> 0) & 0xf)) / fps);
                    c.Time = (uint)((((hh * 60 + mm) * 60 + ss) * 1000L + ms) * rate);

                    c.Discontinuity = (cat & 0x02) != 0;
                    if (c.Discontinuity) tOffset = tTotal;
                    c.TimeTotal = tTotal;
                    c.TimeOffset = tOffset;
                    tTotal += c.Time;
                }

                for (iAngle = 1; iAngle <= 9; iAngle++)
                {
                    tOffset = tTotal = 0;
                    var list = pgc.Angles[iAngle];
                    foreach (VobCell src in all)
                    {
                        if (src.AngleIndex != 0 && src.AngleIndex != iAngle) continue;
                        VobCell c = src.Clone();
                        if (c.Discontinuity) tOffset = tTotal;
                        c.TimeTotal = tTotal;
                        c.TimeOffset = tOffset;
                        tTotal += c.Time;
                        list.Add(c);
                    }
                }

                info.Pgcs.Add(pgc);
            }

            return info;
        }
    }
}
