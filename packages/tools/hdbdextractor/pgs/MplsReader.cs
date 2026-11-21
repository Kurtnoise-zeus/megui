using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MeGUI.BdSub
{
    /// <summary>
    /// Lecture minimale d'une playlist Blu-ray (BDMV/PLAYLIST/xxxxx.mpls) : PlayItems et flux PG de la STN_table.
    /// </summary>
    public sealed class MplsPlaylist
    {
        public string Path = "";
        public List<MplsPlayItem> Items = new List<MplsPlayItem>();

        private static int U8(byte[] d, int p) { Check(d, p, 1); return d[p]; }
        private static int U16(byte[] d, int p) { Check(d, p, 2); return (d[p] << 8) | d[p + 1]; }
        private static long U32(byte[] d, int p) { Check(d, p, 4); return ((long)d[p] << 24) | ((long)d[p + 1] << 16) | ((long)d[p + 2] << 8) | d[p + 3]; }
        private static string Ascii(byte[] d, int p, int n) { Check(d, p, n); return Encoding.ASCII.GetString(d, p, n); }
        private static void Check(byte[] d, int p, int n)
        {
            if (p < 0 || p + n > d.Length) throw new BdFormatException("MPLS truncated or invalid (offset " + p + ")");
        }

        public static MplsPlaylist Load(string path)
        {
            return Parse(File.ReadAllBytes(path), path);
        }

        public static MplsPlaylist Parse(byte[] d, string path)
        {
            if (d.Length < 40 || Ascii(d, 0, 4) != "MPLS") throw new BdFormatException("Not an MPLS file: " + path);

            MplsPlaylist pl = new MplsPlaylist();
            pl.Path = path;

            int p = (int)U32(d, 8);                      // PlayList_start_address
            int nItems = U16(d, p + 6);
            p += 10;                                     // length(4) reserved(2) nPlayItems(2) nSubPaths(2)

            for (int i = 0; i < nItems; i++)
            {
                int itemStart = p;
                int itemLen = U16(d, p);
                int itemEnd = itemStart + 2 + itemLen;
                p += 2;

                MplsPlayItem it = new MplsPlayItem();
                it.ClipName = Ascii(d, p, 5);
                p += 5 + 4;                              // Clip_Information_file_name + Clip_codec_identifier
                int flags = U16(d, p); p += 2;
                bool multiAngle = ((flags >> 4) & 1) != 0;
                it.ConnectionCondition = flags & 0x0f;
                p += 1;                                  // ref_to_STC_id
                it.InTime45 = U32(d, p); p += 4;
                it.OutTime45 = U32(d, p); p += 4;
                p += 8;                                  // UO_mask_table
                p += 1;                                  // random access flag + reserved
                p += 1;                                  // still_mode
                p += 2;                                  // still_time / reserved

                if (multiAngle)
                {
                    int nAngles = U8(d, p); p += 2;      // number_of_angles + flags
                    it.AngleCount = Math.Max(1, nAngles);
                    p += (it.AngleCount - 1) * (5 + 4 + 1);
                }

                ParseStn(d, p, it);
                pl.Items.Add(it);
                p = itemEnd;                             // les PlayItems se suivent
            }

            if (pl.Items.Count == 0) throw new BdFormatException("MPLS without PlayItem: " + path);
            return pl;
        }

        private static void ParseStn(byte[] d, int p, MplsPlayItem it)
        {
            // STN_table : length(2) reserved(2) puis 7 compteurs et 5 octets réservés
            p += 2 + 2;
            int nVideo = U8(d, p);
            int nAudio = U8(d, p + 1);
            int nPg = U8(d, p + 2);
            p += 7 + 5;

            // chaque flux : stream_entry (longueur sur 1 octet) puis stream_attributes (idem)
            for (int i = 0; i < nVideo + nAudio; i++) p = SkipStream(d, p);

            for (int i = 0; i < nPg; i++)
            {
                int entryLen = U8(d, p);
                int entryType = U8(d, p + 1);
                PgStreamInfo s = new PgStreamInfo();
                s.Index = i;
                if (entryType == 1) s.Pid = U16(d, p + 2);
                else if (entryType == 2) { s.Pid = U16(d, p + 4); s.SubPath = true; }
                else if (entryType == 3 || entryType == 4) { s.Pid = U16(d, p + 3); s.SubPath = true; }
                p += 1 + entryLen;

                int attrLen = U8(d, p);
                int coding = U8(d, p + 1);
                if (coding == 0x90 || coding == 0x91) s.Language = Ascii(d, p + 2, 3);
                else if (coding == 0x92) s.Language = Ascii(d, p + 3, 3);   // TextST : char_code puis langue
                p += 1 + attrLen;

                it.PgStreams.Add(s);
            }
        }

        private static int SkipStream(byte[] d, int p)
        {
            p += 1 + U8(d, p);      // stream_entry
            p += 1 + U8(d, p);      // stream_attributes
            return p;
        }
    }
}
