using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace MeGUI.BdSub
{
    /// <summary>
    /// First displayed subtitle image of a PGS stream, kept as palette indexes so that it can be rendered
    /// with the original or a modified palette without reading the disc again.
    /// </summary>
    public sealed class PgsPreviewImage
    {
        public int Width;
        public int Height;
        /// <summary>Palette index of each pixel (-1: transparent / nothing drawn).</summary>
        public int[] Indices;
        /// <summary>PDS segment data (palette id, version, 5-byte entries).</summary>
        public byte[] Pds;
        public bool Bt709;
        /// <summary>Presentation time (90 kHz) of the display set in its clip.</summary>
        public long Pts;

        /// <summary>
        /// Renders the image as 0xAARRGGBB pixels (not premultiplied).
        /// The palette goes through exactly the same <see cref="PgsRecolor"/> code as the extraction.
        /// </summary>
        public int[] Render(PgsColorOptions colors)
        {
            byte[] pds = (colors != null && colors.Enabled) ? PgsRecolor.Apply(Pds, colors, Bt709) : Pds;
            int[] palette = new int[256];
            for (int p = 2; p + 5 <= pds.Length; p += 5)
                palette[pds[p]] = YCrCbToArgb(pds[p + 1], pds[p + 2], pds[p + 3], pds[p + 4], Bt709);

            int[] result = new int[Indices.Length];
            for (int i = 0; i < result.Length; i++)
                result[i] = Indices[i] < 0 ? 0 : palette[Indices[i]];
            return result;
        }

        private static int YCrCbToArgb(int yy, int cr, int cb, int alpha, bool bt709)
        {
            double kr = bt709 ? 0.2126 : 0.299;
            double kb = bt709 ? 0.0722 : 0.114;
            double kg = 1.0 - kr - kb;
            double y = (yy - 16) / 219.0;
            double pcb = (cb - 128) / 224.0;
            double pcr = (cr - 128) / 224.0;
            double r = y + 2.0 * (1.0 - kr) * pcr;
            double b = y + 2.0 * (1.0 - kb) * pcb;
            double g = (y - kr * r - kb * b) / kg;
            return (Clamp(alpha) << 24) | (Clamp(r * 255.0) << 16) | (Clamp(g * 255.0) << 8) | Clamp(b * 255.0);
        }

        private static int Clamp(double v)
        {
            return Math.Max(0, Math.Min(255, (int)Math.Round(v)));
        }
    }

    public static class PgsPreview
    {
        private const int PcsSegment = 0x16;
        private const int PdsSegment = 0x14;
        private const int OdsSegment = 0x15;
        private const int EndSegment = 0x80;

        /// <summary>
        /// Looks for the first subtitle image of the PGS stream <paramref name="trackIndex"/> (rank in the list of
        /// <see cref="PgsExtractor.ListTracks"/>) in a playlist (.mpls) or in M2TS file(s).
        /// Returns null if nothing is found in the first <paramref name="maxBytes"/> bytes of each clip.
        /// </summary>
        public static PgsPreviewImage Find(IList<string> sources, int trackIndex, long maxBytes, CancellationToken ct)
        {
            if (sources == null || sources.Count == 0) return null;
            List<KeyValuePair<string, int>> clips = new List<KeyValuePair<string, int>>();   // path, PID

            if (sources.Count == 1 && sources[0].EndsWith(".mpls", StringComparison.OrdinalIgnoreCase))
            {
                MplsPlaylist pl = MplsPlaylist.Load(sources[0]);
                string streamDir = Path.Combine(Path.GetDirectoryName(Path.GetFullPath(sources[0])) ?? ".", "..", "STREAM");
                foreach (MplsPlayItem it in pl.Items)
                {
                    if (trackIndex < 0 || trackIndex >= it.PgStreams.Count) continue;
                    clips.Add(new KeyValuePair<string, int>(Path.GetFullPath(Path.Combine(streamDir, it.ClipName + ".m2ts")), it.PgStreams[trackIndex].Pid));
                    if (clips.Count >= 5) break;
                }
            }
            else
            {
                foreach (string f in sources)
                {
                    List<PgStreamInfo> tracks = PgsExtractor.ListTracks(f);
                    if (trackIndex >= 0 && trackIndex < tracks.Count)
                        clips.Add(new KeyValuePair<string, int>(f, tracks[trackIndex].Pid));
                }
            }

            foreach (KeyValuePair<string, int> clip in clips)
            {
                if (ct.IsCancellationRequested) return null;
                if (!File.Exists(clip.Key)) continue;
                PgsPreviewImage img = Scan(clip.Key, clip.Value, maxBytes, ct);
                if (img != null) return img;
            }
            return null;
        }

        private static PgsPreviewImage Scan(string path, int pid, long maxBytes, CancellationToken ct)
        {
            PgsExtractor.SegmentParser parser = new PgsExtractor.SegmentParser();
            List<PgsSegment> set = null;
            PgsPreviewImage found = null;

            Action<PgsSegment> onSegment = delegate (PgsSegment seg)
            {
                if (found != null) return;
                if (seg.Type == PcsSegment) set = new List<PgsSegment>();
                if (set == null) return;
                set.Add(seg);
                if (seg.Type == EndSegment)
                {
                    found = Build(set);
                    set = null;
                }
            };

            TsDemuxer.Run(path, 0, maxBytes, p => p == pid,
                pes => { if (found == null && pes.Payload.Length > 0) parser.Feed(pes, onSegment); },
                null,
                () => found != null || ct.IsCancellationRequested);
            return found;
        }

        /// <summary>Builds the image of a display set, or null if it only clears the screen / updates a palette.</summary>
        private static PgsPreviewImage Build(List<PgsSegment> set)
        {
            PgsSegment pcs = set[0];
            if (pcs.Data.Length < 11) return null;
            int height = (pcs.Data[2] << 8) | pcs.Data[3];
            int paletteId = pcs.Data[9];
            int count = pcs.Data[10];
            if (count == 0) return null;

            // composition objects: id -> position
            Dictionary<int, int[]> positions = new Dictionary<int, int[]>();
            int p = 11;
            for (int i = 0; i < count && p + 8 <= pcs.Data.Length; i++)
            {
                int id = (pcs.Data[p] << 8) | pcs.Data[p + 1];
                bool cropped = (pcs.Data[p + 3] & 0x80) != 0;
                positions[id] = new int[] { (pcs.Data[p + 4] << 8) | pcs.Data[p + 5], (pcs.Data[p + 6] << 8) | pcs.Data[p + 7] };
                p += 8 + (cropped ? 8 : 0);
            }

            // palette entries (all the PDS of the set with the right id)
            SortedDictionary<int, byte[]> entries = new SortedDictionary<int, byte[]>();
            // object data (ODS can be split into fragments)
            Dictionary<int, MemoryStream> objects = new Dictionary<int, MemoryStream>();
            Dictionary<int, int[]> sizes = new Dictionary<int, int[]>();

            foreach (PgsSegment s in set)
            {
                if (s.Type == PdsSegment && s.Data.Length >= 2 && s.Data[0] == paletteId)
                {
                    for (int q = 2; q + 5 <= s.Data.Length; q += 5)
                        entries[s.Data[q]] = new byte[] { s.Data[q], s.Data[q + 1], s.Data[q + 2], s.Data[q + 3], s.Data[q + 4] };
                }
                else if (s.Type == OdsSegment && s.Data.Length >= 4)
                {
                    int id = (s.Data[0] << 8) | s.Data[1];
                    int flag = s.Data[3];
                    if ((flag & 0x80) != 0)
                    {
                        if (s.Data.Length < 11) continue;
                        objects[id] = new MemoryStream();
                        sizes[id] = new int[] { (s.Data[7] << 8) | s.Data[8], (s.Data[9] << 8) | s.Data[10] };
                        objects[id].Write(s.Data, 11, s.Data.Length - 11);
                    }
                    else if (objects.ContainsKey(id))
                        objects[id].Write(s.Data, 4, s.Data.Length - 4);
                }
            }
            if (objects.Count == 0 || entries.Count == 0) return null;

            // bounding box of the objects
            int minX = int.MaxValue, minY = int.MaxValue, maxX = 0, maxY = 0;
            foreach (KeyValuePair<int, int[]> kv in positions)
            {
                int[] sz;
                if (!sizes.TryGetValue(kv.Key, out sz) || sz[0] <= 0 || sz[1] <= 0) continue;
                minX = Math.Min(minX, kv.Value[0]); minY = Math.Min(minY, kv.Value[1]);
                maxX = Math.Max(maxX, kv.Value[0] + sz[0]); maxY = Math.Max(maxY, kv.Value[1] + sz[1]);
            }
            if (maxX <= minX || maxY <= minY || (maxX - minX) * (long)(maxY - minY) > 4000000) return null;

            PgsPreviewImage img = new PgsPreviewImage();
            img.Width = maxX - minX;
            img.Height = maxY - minY;
            img.Indices = new int[img.Width * img.Height];
            for (int i = 0; i < img.Indices.Length; i++) img.Indices[i] = -1;

            foreach (KeyValuePair<int, int[]> kv in positions)
            {
                int[] sz;
                if (!sizes.TryGetValue(kv.Key, out sz) || !objects.ContainsKey(kv.Key)) continue;
                DecodeRle(objects[kv.Key].ToArray(), sz[0], sz[1], img.Indices, img.Width, kv.Value[0] - minX, kv.Value[1] - minY);
            }

            MemoryStream pds = new MemoryStream();
            pds.WriteByte((byte)paletteId);
            pds.WriteByte(0);
            foreach (byte[] e in entries.Values) pds.Write(e, 0, 5);
            img.Pds = pds.ToArray();
            img.Bt709 = height >= 720;
            img.Pts = pcs.Pts;
            return img;
        }

        /// <summary>Decodes the RLE bitmap of a PGS object into <paramref name="dest"/> (stride <paramref name="stride"/>).</summary>
        internal static void DecodeRle(byte[] rle, int width, int height, int[] dest, int stride, int offX, int offY)
        {
            int x = 0, y = 0, i = 0;
            while (i < rle.Length && y < height)
            {
                int b = rle[i++];
                int len = 1, color = b;
                if (b == 0)
                {
                    if (i >= rle.Length) break;
                    int c = rle[i++];
                    if (c == 0) { x = 0; y++; continue; }           // end of line
                    int type = c & 0xC0;
                    if (type == 0x00) { len = c & 0x3F; color = 0; }
                    else if (type == 0x40)
                    {
                        if (i >= rle.Length) break;
                        len = ((c & 0x3F) << 8) | rle[i++]; color = 0;
                    }
                    else if (type == 0x80)
                    {
                        if (i >= rle.Length) break;
                        len = c & 0x3F; color = rle[i++];
                    }
                    else
                    {
                        if (i + 1 >= rle.Length) break;
                        len = ((c & 0x3F) << 8) | rle[i++]; color = rle[i++];
                    }
                }
                for (int k = 0; k < len && x < width; k++, x++)
                {
                    // color 0 is not necessarily transparent: the alpha of the palette decides
                    int index = (offY + y) * stride + offX + x;
                    if (index >= 0 && index < dest.Length) dest[index] = color;
                }
            }
        }
    }
}
