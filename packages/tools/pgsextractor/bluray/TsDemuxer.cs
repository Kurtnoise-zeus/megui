using System;
using System.Collections.Generic;
using System.IO;

namespace MeGUI.BdSub
{
    /// <summary>Reassembled PES packet.</summary>
    public sealed class PesPacket
    {
        public int Pid;
        public int StreamId;
        public long Pts = -1;   // 90 kHz, -1 si absent
        public long Dts = -1;
        public byte[] Payload;
    }

    /// <summary>
    /// Minimal MPEG-TS demultiplexer for Blu-ray M2TS files (192-byte packets: TP_extra_header + 188)
    /// or plain TS files (188). It reassembles the PES packets of the requested PIDs and reads PAT/PMT.
    /// </summary>
    public static class TsDemuxer
    {
        public sealed class EsInfo
        {
            public int Pid;
            public int StreamType;
            public string Language = "";
        }

        /// <summary>Detects the packet size (192 or 188) from the sync bytes.</summary>
        public static int DetectPacketSize(byte[] head, int count)
        {
            foreach (int size in new int[] { 192, 188 })
            {
                int off = size == 192 ? 4 : 0;
                int ok = 0, n = 0;
                for (int i = 0; off + i * size < count && n < 8; i++, n++)
                    if (head[off + i * size] == 0x47) ok++;
                if (n >= 3 && ok == n) return size;
            }
            throw new BdFormatException("Unrecognized transport stream (no 0x47 sync byte with 188/192 byte packets)");
        }

        /// <summary>
        /// Reads [startByte, endByte) of the file and calls <paramref name="onPes"/> for each complete PES of the requested PIDs
        /// (wantPid == null: all the PIDs, except PAT/PMT).
        /// </summary>
        public static void Run(string path, long startByte, long endByte, Func<int, bool> wantPid,
                               Action<PesPacket> onPes, Action<long> onProgress, Func<bool> cancelled)
        {
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite, 1 << 20, FileOptions.SequentialScan))
            {
                byte[] head = new byte[192 * 8];
                int got = fs.Read(head, 0, head.Length);
                int psize = DetectPacketSize(head, got);
                int off = psize == 192 ? 4 : 0;

                long len = fs.Length;
                if (endByte <= 0 || endByte > len) endByte = len;
                startByte = startByte / psize * psize;
                fs.Seek(startByte, SeekOrigin.Begin);

                Dictionary<int, PesBuilder> builders = new Dictionary<int, PesBuilder>();
                byte[] buf = new byte[psize * 4096];
                long pos = startByte;
                int lastReport = 0;

                while (pos < endByte)
                {
                    if (cancelled != null && cancelled()) return;
                    int want = (int)Math.Min(buf.Length, endByte - pos);
                    want -= want % psize;
                    if (want <= 0) break;
                    int n = ReadFully(fs, buf, want);
                    n -= n % psize;
                    if (n <= 0) break;

                    for (int o = 0; o < n; o += psize)
                    {
                        int b = o + off;
                        if (buf[b] != 0x47)
                            throw new BdFormatException(string.Format("Transport stream sync lost at offset {0}", pos + o));

                        int pid = ((buf[b + 1] & 0x1f) << 8) | buf[b + 2];
                        if (pid == 0x1fff) continue;
                        if (wantPid != null && !wantPid(pid)) continue;
                        if (wantPid == null && pid == 0) continue;
                        if ((buf[b + 1] & 0x80) != 0) continue;                 // transport_error_indicator

                        bool pusi = (buf[b + 1] & 0x40) != 0;
                        int afc = (buf[b + 3] >> 4) & 3;
                        if ((afc & 1) == 0) continue;                            // no payload
                        int p = b + 4;
                        if ((afc & 2) != 0) p += 1 + buf[b + 4];
                        int end = b + 188;
                        if (p >= end) continue;

                        PesBuilder pb;
                        if (!builders.TryGetValue(pid, out pb)) { pb = new PesBuilder(pid); builders[pid] = pb; }

                        int scrambling = (buf[b + 3] >> 6) & 3;
                        if (pusi)
                        {
                            PesPacket done = pb.Finish();
                            if (done != null) onPes(done);
                            pb.Begin(buf, p, end - p, scrambling != 0);
                        }
                        else
                        {
                            pb.Append(buf, p, end - p);
                        }
                    }

                    pos += n;
                    if (onProgress != null && (int)((pos - startByte) >> 24) != lastReport)
                    {
                        lastReport = (int)((pos - startByte) >> 24);
                        onProgress(pos);
                    }
                    if (n < want) break;
                }

                foreach (PesBuilder pb in builders.Values)
                {
                    PesPacket done = pb.Finish();
                    if (done != null) onPes(done);
                }
                if (onProgress != null) onProgress(pos);
            }
        }

        private static int ReadFully(Stream s, byte[] buf, int count)
        {
            int got = 0;
            while (got < count)
            {
                int n = s.Read(buf, got, count - got);
                if (n <= 0) break;
                got += n;
            }
            return got;
        }

        private sealed class PesBuilder
        {
            private readonly int _pid;
            private MemoryStream _ms;
            private bool _scrambled;

            public PesBuilder(int pid) { _pid = pid; }

            public void Begin(byte[] b, int p, int n, bool scrambled)
            {
                _ms = new MemoryStream(n * 4);
                _scrambled = scrambled;
                _ms.Write(b, p, n);
            }

            public void Append(byte[] b, int p, int n)
            {
                if (_ms != null) _ms.Write(b, p, n);
            }

            public PesPacket Finish()
            {
                if (_ms == null) return null;
                byte[] d = _ms.ToArray();
                _ms = null;

                if (d.Length < 9 || d[0] != 0 || d[1] != 0 || d[2] != 1)
                {
                    if (_scrambled || d.Length >= 9)
                        throw new BdFormatException(string.Format("Invalid PES header on PID 0x{0:X4}: the stream is probably AACS-encrypted (decrypt the disc first)", _pid));
                    return null;
                }

                PesPacket pes = new PesPacket();
                pes.Pid = _pid;
                pes.StreamId = d[3];
                int flags = d[7] >> 6;
                int hlen = d[8];
                int start = 9 + hlen;
                if (start > d.Length) return null;
                if ((flags & 2) != 0 && d.Length >= 14) pes.Pts = ReadTs(d, 9);
                if (flags == 3 && d.Length >= 19) pes.Dts = ReadTs(d, 14);

                int pesLen = (d[4] << 8) | d[5];
                int total = pesLen == 0 ? d.Length : Math.Min(d.Length, 6 + pesLen);
                int plen = Math.Max(0, total - start);
                pes.Payload = new byte[plen];
                Buffer.BlockCopy(d, start, pes.Payload, 0, plen);
                return pes;
            }

            private static long ReadTs(byte[] d, int p)
            {
                return ((long)(d[p] & 0x0e) << 29)
                     | ((long)d[p + 1] << 22)
                     | ((long)(d[p + 2] & 0xfe) << 14)
                     | ((long)d[p + 3] << 7)
                     | ((long)d[p + 4] >> 1);
            }
        }

        /// <summary>Reads the PAT then the PMT (start of the file) and returns the declared elementary streams.</summary>
        public static List<EsInfo> ReadPmt(string path)
        {
            List<EsInfo> result = new List<EsInfo>();
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                byte[] head = new byte[192 * 8];
                int got = fs.Read(head, 0, head.Length);
                int psize = DetectPacketSize(head, got);
                int off = psize == 192 ? 4 : 0;

                List<int> pmtPids = new List<int>();
                byte[] pkt = new byte[psize];
                fs.Seek(0, SeekOrigin.Begin);
                long limit = Math.Min(fs.Length, 4L << 20);

                while (fs.Position + psize <= limit && fs.Read(pkt, 0, psize) == psize)
                {
                    int b = off;
                    if (pkt[b] != 0x47) break;
                    int pid = ((pkt[b + 1] & 0x1f) << 8) | pkt[b + 2];
                    bool pusi = (pkt[b + 1] & 0x40) != 0;
                    int afc = (pkt[b + 3] >> 4) & 3;
                    if (!pusi || (afc & 1) == 0) continue;
                    int p = b + 4;
                    if ((afc & 2) != 0) p += 1 + pkt[b + 4];
                    if (p >= b + 188) continue;
                    p += 1 + pkt[p];                         // pointer_field
                    if (p + 12 > b + 188) continue;

                    int tableId = pkt[p];
                    int secLen = ((pkt[p + 1] & 0x0f) << 8) | pkt[p + 2];
                    int secEnd = Math.Min(b + 188, p + 3 + secLen) - 4;     // without CRC

                    if (pid == 0 && tableId == 0)
                    {
                        for (int q = p + 8; q + 4 <= secEnd; q += 4)
                        {
                            int prog = (pkt[q] << 8) | pkt[q + 1];
                            int ppid = ((pkt[q + 2] & 0x1f) << 8) | pkt[q + 3];
                            if (prog != 0 && !pmtPids.Contains(ppid)) pmtPids.Add(ppid);
                        }
                    }
                    else if (tableId == 2 && pmtPids.Contains(pid))
                    {
                        int progInfoLen = ((pkt[p + 10] & 0x0f) << 8) | pkt[p + 11];
                        int q = p + 12 + progInfoLen;
                        while (q + 5 <= secEnd)
                        {
                            EsInfo es = new EsInfo();
                            es.StreamType = pkt[q];
                            es.Pid = ((pkt[q + 1] & 0x1f) << 8) | pkt[q + 2];
                            int esInfoLen = ((pkt[q + 3] & 0x0f) << 8) | pkt[q + 4];
                            int dq = q + 5, dend = Math.Min(secEnd, dq + esInfoLen);
                            while (dq + 2 <= dend)
                            {
                                int tag = pkt[dq], dlen = pkt[dq + 1];
                                if (tag == 0x0a && dlen >= 3 && dq + 5 <= dend)
                                    es.Language = System.Text.Encoding.ASCII.GetString(pkt, dq + 2, 3);
                                dq += 2 + dlen;
                            }
                            result.Add(es);
                            q += 5 + esInfoLen;
                        }
                        return result;
                    }
                }
            }
            return result;
        }
    }
}
