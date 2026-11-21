using System;

namespace MeGUI.BdSub
{
    /// <summary>
    /// Recoloring of the PGS subtitles. PGS subtitles are bitmaps whose pixels are indexes into a palette
    /// (PDS segments): only the palette can be changed, not the shape of the glyphs.
    /// </summary>
    public sealed class PgsColorOptions
    {
        public bool Enabled;

        /// <summary>Color of the text (0xRRGGBB): replaces the brightest palette entries.</summary>
        public int TextRgb = 0xFFFFFF;

        /// <summary>Color of the outline / shadow (0xRRGGBB): replaces the darkest palette entries.</summary>
        public int OutlineRgb = 0x000000;
    }

    internal static class PgsRecolor
    {
        /// <summary>
        /// Returns a recolored copy of a PDS (palette definition) segment.
        /// Each palette entry is replaced by a mix of the outline color and the text color, according to its
        /// original luma (dark = outline, bright = text), so that the anti-aliasing is kept.
        /// The transparency of the entries is not changed.
        /// </summary>
        /// <param name="pds">PDS segment data: palette id, version, then entries of 5 bytes (id, Y, Cr, Cb, alpha)</param>
        /// <param name="bt709">true for HD subtitles (BT.709), false for SD (BT.601)</param>
        public static byte[] Apply(byte[] pds, PgsColorOptions options, bool bt709)
        {
            byte[] result = (byte[])pds.Clone();

            double kr = bt709 ? 0.2126 : 0.299;
            double kb = bt709 ? 0.0722 : 0.114;
            double kg = 1.0 - kr - kb;

            double textR = (options.TextRgb >> 16) & 0xff, textG = (options.TextRgb >> 8) & 0xff, textB = options.TextRgb & 0xff;
            double outR = (options.OutlineRgb >> 16) & 0xff, outG = (options.OutlineRgb >> 8) & 0xff, outB = options.OutlineRgb & 0xff;

            for (int p = 2; p + 5 <= result.Length; p += 5)
            {
                if (result[p + 4] == 0)         // fully transparent entry
                    continue;

                double t = (result[p + 1] - 16) / 219.0;
                t = Math.Max(0.0, Math.Min(1.0, t));

                double r = outR + (textR - outR) * t;
                double g = outG + (textG - outG) * t;
                double b = outB + (textB - outB) * t;

                double y = (kr * r + kg * g + kb * b) / 255.0;
                double cb = (b / 255.0 - y) / (2.0 * (1.0 - kb));
                double cr = (r / 255.0 - y) / (2.0 * (1.0 - kr));

                result[p + 1] = ToByte(16.0 + 219.0 * y);
                result[p + 2] = ToByte(128.0 + 224.0 * cr);
                result[p + 3] = ToByte(128.0 + 224.0 * cb);
            }
            return result;
        }

        private static byte ToByte(double value)
        {
            return (byte)Math.Max(0, Math.Min(255, (int)Math.Round(value)));
        }
    }
}
