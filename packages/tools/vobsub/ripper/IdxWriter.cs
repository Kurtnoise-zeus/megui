using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MeGUI.VobSubRip
{
    /// <summary>Position d'un sous-titre dans le .sub (équivalent de <c>CVobSubFile::SubPos</c>).</summary>
    internal sealed class SubPos
    {
        public long FilePos;
        public long Start;       // ms
        public byte VobId;
        public byte CellId;
        public long CellTimestamp;
        public bool Valid;
    }

    internal sealed class SubLang
    {
        public ushort Id;
        public string Name = "";
        public string Alt = "";
        public List<SubPos> Positions = new List<SubPos>();
    }

    /// <summary>Écriture des fichiers .idx et .sub (équivalent de <c>CVobSubFile::WriteIdx / WriteSub</c>, format idx v7).</summary>
    internal static class IdxWriter
    {
        public const int IdxVersion = 7;

        public static void WriteIdx(string path, int width, int height, int[] palette, int langIndex, IList<SubLang> langs, int delayMs = 0)
        {
            StringBuilder sb = new StringBuilder();
            Action<string> L = s => sb.Append(s).Append("\r\n");

            L("# VobSub index file, v" + IdxVersion + " (do not modify this line!)");
            L("# ");
            L("# To repair desyncronization, you can insert gaps this way:");
            L("# (it usually happens after vob id changes)");
            L("# ");
            L("#\t delay: [sign]hh:mm:ss:ms");
            L("# ");
            L("# Where:");
            L("#\t [sign]: +, - (optional)");
            L("#\t hh: hours (0 <= hh)");
            L("#\t mm/ss: minutes/seconds (0 <= mm/ss <= 59)");
            L("#\t ms: milliseconds (0 <= ms <= 999)");
            L("# ");
            L("#\t Note: You can't position a sub before the previous with a negative value.");
            L("# ");
            L("# You can also modify timestamps or delete a few subs you don't like.");
            L("# Just make sure they stay in increasing order.");
            L("");
            L("");

            L("# Settings");
            L("");
            L("# Original frame size");
            L("size: " + width + "x" + height);
            L("");
            L("# Origin, relative to the upper-left corner, can be overloaded by aligment");
            L("org: 0, 0");
            L("");
            L("# Image scaling (hor,ver), origin is at the upper-left corner or at the alignment coord (x, y)");
            L("scale: 100%, 100%");
            L("");
            L("# Alpha blending");
            L("alpha: 100%");
            L("");
            L("# Smoothing for very blocky images (use OLD for no filtering)");
            L("smooth: OFF");
            L("");
            L("# In millisecs");
            L("fadein/out: 50, 50");
            L("");
            L("# Force subtitle placement relative to (org.x, org.y)");
            L("align: OFF at LEFT TOP");
            L("");
            L("# For correcting non-progressive desync. (in millisecs or hh:mm:ss:ms)");
            L("# Note: Not effective in VSFilter, use \"delay: ... \" instead.");
            L("time offset: 0");
            L("");
            L("# ON: displays only forced subtitles, OFF: shows everything");
            L("forced subs: OFF");
            L("");
            L("# The original palette of the DVD");
            L("palette: " + string.Join(", ", palette.Select(c => (c & 0xffffff).ToString("x6"))));
            L("");
            L("# Custom colors (transp idxs and the four colors)");
            L("custom colors: OFF, tridx: 0000, colors: 000000, 000000, 000000, 000000");
            L("");
            L("# Language index in use");
            L("langidx: " + langIndex);
            L("");

            if (delayMs != 0)
            {
                int a = Math.Abs(delayMs);
                L(string.Format("delay: {0}{1:00}:{2:00}:{3:00}:{4:000}", delayMs < 0 ? "-" : "", a / 1000 / 60 / 60 % 60, a / 1000 / 60 % 60, a / 1000 % 60, a % 1000));
                L("");
            }

            for (int i = 0; i < langs.Count; i++)
            {
                SubLang sl = langs[i];
                List<SubPos> sp = sl.Positions;
                if (sp.Count == 0 && sl.Id == 0) continue;

                L("# " + sl.Name);
                ushort id = sl.Id != 0 ? sl.Id : (ushort)0x2D2D;
                L("id: " + (char)(id >> 8) + (char)(id & 0xff) + ", index: " + i);
                L("# Uncomment next line to activate alternative name in VSFilter / Windows Media Player 6.x");
                L((sl.Name == sl.Alt ? "# " : "") + "alt: " + sl.Alt);

                int vob = -1, cell = -1;
                foreach (SubPos s in sp)
                {
                    if (!s.Valid) continue;
                    if (s.VobId != vob || s.CellId != cell)
                    {
                        L("# Vob/Cell ID: " + s.VobId + ", " + s.CellId + " (PTS: " + s.CellTimestamp + ")");
                        vob = s.VobId;
                        cell = s.CellId;
                    }

                    long t = s.Start;
                    long a = Math.Abs(t);
                    L(string.Format("timestamp: {0}{1:00}:{2:00}:{3:00}:{4:000}, filepos: {5:x9}", t < 0 ? "-" : "", a / 1000 / 60 / 60 % 60, a / 1000 / 60 % 60, a / 1000 % 60, a % 1000, s.FilePos));
                }
                L("");
            }

            File.WriteAllText(path, sb.ToString(), new ASCIIEncoding());
        }

        /// <summary>Écrit le .sub : copie des secteurs tant qu'ils commencent par un pack header MPEG-PS (00 00 01 BA).</summary>
        public static void WriteSub(string path, MemoryStream sub)
        {
            using (FileStream f = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.Read))
            {
                sub.Position = 0;
                byte[] buf = new byte[2048];
                int len;
                while ((len = sub.Read(buf, 0, buf.Length)) > 0 && buf[0] == 0 && buf[1] == 0 && buf[2] == 1 && buf[3] == 0xba)
                    f.Write(buf, 0, len);
            }
        }
    }
}
