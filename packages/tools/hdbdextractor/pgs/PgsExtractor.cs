using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace MeGUI.BdSub
{
    /// <summary>
    /// Extraction des sous-titres PGS d'un Blu-ray vers des fichiers .sup (format de BDSup2Sub / mkvmerge) :
    /// lecture de la playlist MPLS (ou directement des M2TS), démultiplexage des PID PGS, regroupement en
    /// display sets, recalage des temps sur le début de la playlist.
    /// Les flux chiffrés AACS ne sont pas supportés.
    /// </summary>
    public sealed class PgsExtractor
    {
        private const int EndSegment = 0x80;
        private const int PcsSegment = 0x16;

        private readonly Action<BdLogLevel, string> _log;
        public Action<double> Progress { get; set; }

        public PgsExtractor(Action<BdLogLevel, string> log)
        {
            _log = log ?? delegate { };
        }

        private void Emit(BdLogLevel l, string m) { _log(l, m); }

        // ------------------------------------------------------------------ pistes

        /// <summary>Liste les flux PGS d'une playlist (.mpls, ordre de la STN_table du premier PlayItem) ou d'un M2TS (ordre de la PMT).</summary>
        public static List<PgStreamInfo> ListTracks(string path)
        {
            if (path.EndsWith(".mpls", StringComparison.OrdinalIgnoreCase))
                return MplsPlaylist.Load(path).Items[0].PgStreams;

            List<PgStreamInfo> list = new List<PgStreamInfo>();
            foreach (TsDemuxer.EsInfo es in TsDemuxer.ReadPmt(path))
            {
                if (es.StreamType != 0x90) continue;
                PgStreamInfo s = new PgStreamInfo();
                s.Index = list.Count;
                s.Pid = es.Pid;
                s.Language = es.Language;
                list.Add(s);
            }
            return list;
        }

        // ------------------------------------------------------------------ extraction

        private sealed class Clip
        {
            public string Path;
            public long In90;
            public long Out90;          // long.MaxValue : jusqu'à la fin du fichier
            public long Offset90;       // temps du début du clip dans la playlist
            public Dictionary<int, int> PidByTrack = new Dictionary<int, int>();   // index de requête -> PID
        }

        private sealed class Track
        {
            public PgsTrackRequest Request;
            public PgsTrackResult Result = new PgsTrackResult();
            public FileStream Stream;
            public bool NeedEpoch = true;
        }

        private sealed class DisplaySet
        {
            public long Pts;
            public int State;
            public List<PgsSegment> Segments = new List<PgsSegment>();
        }

        /// <summary>Extrait depuis une playlist .mpls (les M2TS sont cherchés dans ../STREAM).</summary>
        public PgsExtractionResult ExtractPlaylist(string mplsPath, IList<PgsTrackRequest> requests, CancellationToken ct)
        {
            MplsPlaylist pl = MplsPlaylist.Load(mplsPath);
            string streamDir = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.IO.Path.GetFullPath(mplsPath)) ?? ".", "..", "STREAM");

            List<Clip> clips = new List<Clip>();
            long offset = 0;
            foreach (MplsPlayItem it in pl.Items)
            {
                Clip c = new Clip();
                c.Path = System.IO.Path.GetFullPath(System.IO.Path.Combine(streamDir, it.ClipName + ".m2ts"));
                c.In90 = it.In90;
                c.Out90 = it.Out90;
                c.Offset90 = offset;
                offset += Math.Max(0, it.Out90 - it.In90);
                if (it.AngleCount > 1) Emit(BdLogLevel.Warning, "Multi-angle play item " + it.ClipName + ": only the first angle is extracted");

                foreach (PgsTrackRequest r in requests)
                {
                    if (r.Index < 0 || r.Index >= it.PgStreams.Count)
                        throw new BdFormatException(string.Format("Playlist {0} has only {1} PGS stream(s), cannot extract #{2}", System.IO.Path.GetFileName(mplsPath), it.PgStreams.Count, r.Index + 1));
                    c.PidByTrack[requests.IndexOf(r)] = it.PgStreams[r.Index].Pid;
                }
                clips.Add(c);
            }

            Emit(BdLogLevel.Info, string.Format("Playlist {0}: {1} play item(s), {2} PGS stream(s)",
                System.IO.Path.GetFileName(mplsPath), pl.Items.Count, pl.Items[0].PgStreams.Count));
            foreach (PgStreamInfo s in pl.Items[0].PgStreams) Emit(BdLogLevel.Info, "  " + s);

            return Run(clips, requests, ct);
        }

        /// <summary>Extrait directement de fichiers M2TS (mis bout à bout, le temps est recalé sur le premier PTS vidéo).</summary>
        public PgsExtractionResult ExtractFiles(IList<string> m2tsFiles, IList<PgsTrackRequest> requests, CancellationToken ct)
        {
            List<Clip> clips = new List<Clip>();
            long offset = 0;
            foreach (string f in m2tsFiles)
            {
                List<PgStreamInfo> tracks = ListTracks(f);
                Clip c = new Clip();
                c.Path = f;
                long first, last;
                VideoPtsRange(f, out first, out last);
                c.In90 = first;
                c.Out90 = long.MaxValue;
                c.Offset90 = offset;
                offset += Math.Max(0, last - first);

                foreach (PgsTrackRequest r in requests)
                {
                    if (r.Index < 0 || r.Index >= tracks.Count)
                        throw new BdFormatException(string.Format("{0} has only {1} PGS stream(s), cannot extract #{2}", System.IO.Path.GetFileName(f), tracks.Count, r.Index + 1));
                    c.PidByTrack[requests.IndexOf(r)] = tracks[r.Index].Pid;
                }
                clips.Add(c);
                Emit(BdLogLevel.Info, string.Format("{0}: {1} PGS stream(s), video starts at {2}", System.IO.Path.GetFileName(f), tracks.Count, TimeSpan.FromSeconds(first / 90000.0)));
            }
            return Run(clips, requests, ct);
        }

        private static void VideoPtsRange(string path, out long first, out long last)
        {
            long min = long.MaxValue, max = long.MinValue;
            long len = new FileInfo(path).Length;

            // PID vidéo d'après la PMT (MPEG-2, H.264, VC-1, HEVC)
            HashSet<int> videoPids = new HashSet<int>();
            foreach (TsDemuxer.EsInfo es in TsDemuxer.ReadPmt(path))
                if (es.StreamType == 0x02 || es.StreamType == 0x1b || es.StreamType == 0xea || es.StreamType == 0x24) videoPids.Add(es.Pid);
            Func<int, bool> isVideo = pid => videoPids.Contains(pid);

            Action<PesPacket> onPes = delegate (PesPacket p)
            {
                if (p.StreamId >= 0xe0 && p.StreamId <= 0xef && p.Pts >= 0)
                {
                    if (p.Pts < min) min = p.Pts;
                    if (p.Pts > max) max = p.Pts;
                }
            };

            if (len <= (16L << 20))
            {
                TsDemuxer.Run(path, 0, len, isVideo, onPes, null, null);
            }
            else
            {
                TsDemuxer.Run(path, 0, 8L << 20, isVideo, onPes, null, null);
                TsDemuxer.Run(path, len - (8L << 20), len, isVideo, onPes, null, null);
            }

            first = min == long.MaxValue ? 0 : min;
            last = max == long.MinValue ? first : max;
            if (last < first) last = first;
        }

        private PgsExtractionResult Run(List<Clip> clips, IList<PgsTrackRequest> requests, CancellationToken ct)
        {
            PgsExtractionResult result = new PgsExtractionResult();
            List<Track> tracks = new List<Track>();
            try
            {
                for (int i = 0; i < requests.Count; i++)
                {
                    Track t = new Track();
                    t.Request = requests[i];
                    t.Result.Index = requests[i].Index;
                    t.Result.Output = requests[i].Output;
                    string dir = System.IO.Path.GetDirectoryName(System.IO.Path.GetFullPath(requests[i].Output));
                    if (!string.IsNullOrEmpty(dir)) Directory.CreateDirectory(dir);
                    t.Stream = new FileStream(requests[i].Output, FileMode.Create, FileAccess.Write, FileShare.Read, 1 << 16);
                    tracks.Add(t);
                }

                long totalBytes = clips.Sum(c => new FileInfo(c.Path).Length);
                long doneBytes = 0;

                foreach (Clip clip in clips)
                {
                    if (ct.IsCancellationRequested) break;
                    if (!File.Exists(clip.Path)) throw new FileNotFoundException("M2TS not found: " + clip.Path);
                    long clipLen = new FileInfo(clip.Path).Length;
                    Emit(BdLogLevel.Info, string.Format("Reading {0} (offset {1})", System.IO.Path.GetFileName(clip.Path), TimeSpan.FromSeconds(clip.Offset90 / 90000.0)));

                    // PID -> pistes qui le demandent ; un assembleur de segments par PID
                    Dictionary<int, List<Track>> byPid = new Dictionary<int, List<Track>>();
                    foreach (KeyValuePair<int, int> kv in clip.PidByTrack)
                    {
                        List<Track> l;
                        if (!byPid.TryGetValue(kv.Value, out l)) { l = new List<Track>(); byPid[kv.Value] = l; }
                        l.Add(tracks[kv.Key]);
                    }
                    Dictionary<int, SegmentParser> parsers = byPid.Keys.ToDictionary(pid => pid, pid => new SegmentParser());
                    Dictionary<int, DisplaySet> current = new Dictionary<int, DisplaySet>();
                    Dictionary<int, long> lastPts = new Dictionary<int, long>();
                    bool allPast = false;

                    Action<int, PgsSegment> onSegment = delegate (int pid, PgsSegment seg)
                    {
                        lastPts[pid] = seg.Pts;
                        DisplaySet ds;
                        current.TryGetValue(pid, out ds);

                        if (seg.Type == PcsSegment)
                        {
                            ds = new DisplaySet();
                            ds.Pts = seg.Pts;
                            ds.State = seg.Data.Length > 7 ? seg.Data[7] : 0;
                            current[pid] = ds;
                        }
                        if (ds == null) return;     // segments orphelins avant le premier PCS
                        ds.Segments.Add(seg);

                        if (seg.Type == EndSegment)
                        {
                            current.Remove(pid);
                            foreach (Track t in byPid[pid]) Commit(t, clip, ds);
                            if (clip.Out90 != long.MaxValue && byPid.Keys.All(k => lastPts.ContainsKey(k) && lastPts[k] > clip.Out90 + 5 * 90000))
                                allPast = true;
                        }
                    };

                    TsDemuxer.Run(clip.Path, 0, 0,
                        pid => byPid.ContainsKey(pid),
                        pes => { if (pes.Payload.Length > 0) parsers[pes.Pid].Feed(pes, seg => onSegment(pes.Pid, seg)); },
                        pos => { if (Progress != null) Progress(Math.Min(1.0, (doneBytes + pos) / (double)Math.Max(1, totalBytes))); },
                        () => ct.IsCancellationRequested || allPast);

                    doneBytes += clipLen;
                }

                if (Progress != null) Progress(1);
                result.Success = !ct.IsCancellationRequested;
            }
            finally
            {
                foreach (Track t in tracks)
                {
                    if (t.Stream != null) t.Stream.Dispose();
                    result.Tracks.Add(t.Result);
                }
            }

            foreach (PgsTrackResult r in result.Tracks)
            {
                Emit(r.DisplaySets > 0 ? BdLogLevel.Info : BdLogLevel.Warning,
                    string.Format("PGS #{0} -> {1}: {2} display set(s){3}", r.Index + 1, System.IO.Path.GetFileName(r.Output), r.DisplaySets,
                        r.SkippedSets > 0 ? ", " + r.SkippedSets + " skipped (no epoch start)" : ""));
                if (r.DisplaySets == 0) result.Success = false;
            }
            return result;
        }

        /// <summary>Écrit un display set dans la piste s'il appartient à la plage du PlayItem.</summary>
        private static void Commit(Track t, Clip clip, DisplaySet ds)
        {
            if (ds.Pts < clip.In90 || ds.Pts >= clip.Out90) return;

            // un display set « normal » n'est décodable qu'après un début d'époque
            if (t.NeedEpoch && ds.State == 0) { t.Result.SkippedSets++; return; }
            t.NeedEpoch = false;

            long shift = clip.Offset90 - clip.In90;
            byte[] hdr = new byte[13];
            foreach (PgsSegment s in ds.Segments)
            {
                long pts = s.Pts + shift;
                long dts = s.Dts >= 0 ? s.Dts + shift : 0;
                if (pts < 0) pts = 0;
                if (dts < 0) dts = 0;
                hdr[0] = (byte)'P'; hdr[1] = (byte)'G';
                PutU32(hdr, 2, pts);
                PutU32(hdr, 6, dts);
                hdr[10] = s.Type;
                hdr[11] = (byte)(s.Data.Length >> 8);
                hdr[12] = (byte)(s.Data.Length & 0xff);
                t.Stream.Write(hdr, 0, 13);
                t.Stream.Write(s.Data, 0, s.Data.Length);
            }
            t.Result.DisplaySets++;
        }

        private static void PutU32(byte[] b, int o, long v)
        {
            uint u = unchecked((uint)(v & 0xffffffffL));
            b[o] = (byte)((u >> 24) & 0xff); b[o + 1] = (byte)((u >> 16) & 0xff); b[o + 2] = (byte)((u >> 8) & 0xff); b[o + 3] = (byte)(u & 0xff);
        }

        /// <summary>Découpe le flux de PES d'un PID en segments PGS (un segment peut chevaucher deux PES).</summary>
        private sealed class SegmentParser
        {
            private byte[] _carry = new byte[0];
            private long _carryPts, _carryDts;

            public void Feed(PesPacket pes, Action<PgsSegment> sink)
            {
                byte[] data;
                int carryLen = _carry.Length;
                if (carryLen == 0) data = pes.Payload;
                else
                {
                    data = new byte[carryLen + pes.Payload.Length];
                    Buffer.BlockCopy(_carry, 0, data, 0, carryLen);
                    Buffer.BlockCopy(pes.Payload, 0, data, carryLen, pes.Payload.Length);
                }

                int pos = 0;
                while (pos + 3 <= data.Length)
                {
                    int type = data[pos];
                    int size = (data[pos + 1] << 8) | data[pos + 2];
                    if (pos + 3 + size > data.Length) break;

                    PgsSegment seg = new PgsSegment();
                    seg.Type = (byte)type;
                    bool fromCarry = pos < carryLen;
                    seg.Pts = fromCarry ? _carryPts : pes.Pts;
                    seg.Dts = fromCarry ? _carryDts : pes.Dts;
                    seg.Data = new byte[size];
                    Buffer.BlockCopy(data, pos + 3, seg.Data, 0, size);
                    pos += 3 + size;
                    if (seg.Pts >= 0) sink(seg);
                }

                int rest = data.Length - pos;
                if (rest > 0 && rest < (1 << 17))
                {
                    _carry = new byte[rest];
                    Buffer.BlockCopy(data, pos, _carry, 0, rest);
                    bool restFromCarry = pos < carryLen;
                    _carryPts = restFromCarry ? _carryPts : pes.Pts;
                    _carryDts = restFromCarry ? _carryDts : pes.Dts;
                }
                else _carry = new byte[0];
            }
        }
    }
}
