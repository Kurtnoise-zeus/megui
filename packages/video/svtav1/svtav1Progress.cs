using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace MeGUI.packages.video.svtav1
{
    /// <summary>
    /// Progress line printed by SVT-AV1 with --progress 2 (Source/App/app_process_cmd.c), for example
    /// "Encoding:    8/4000 Frames @ 2.48 fps | 303.23 kb/s | Size: 0.24 MB [120.94 MB] | Time: 0:00:03 [-0:26:49]"
    /// - the line is colored with ANSI escape sequences unless the NO_COLOR environment variable is set
    /// - the speed is frames done / elapsed time since the start, printed in fpm (frames per minute) when it
    ///   is below 1 fps and in fps otherwise
    /// - the bitrate is the size written so far divided by the number of frames read so far (not written),
    ///   so it is too low at the beginning
    /// - the size is in MB (10^6 bytes), the value in brackets is the final size extrapolated from it
    /// - the remaining time (in brackets, printed with a minus sign) is elapsed / frames done * frames left:
    ///   it includes the start-up time of the encoder, so it is far too high for the first frames
    /// - when SVT-AV1 does not know the number of frames (no --frames option) the total, the estimated size
    ///   and the remaining time are not printed
    /// </summary>
    internal sealed class svtav1Progress
    {
        // CSI sequences (ESC [ ... final byte), plus the color codes whose ESC character got lost on the way
        private static readonly Regex AnsiEscape = new Regex("\u001b\\[[0-9;?]*[ -/]*[@-~]|\\[\\d{1,3}(?:;\\d{1,3})*m", RegexOptions.Compiled);

        // a number as printed by printf, which gives inf, nan or nan(ind) when a division by zero happens
        private const string Num = @"-?(?:\d+(?:\.\d+)?|inf|nan(?:\(ind\))?)";
        private const string Time = @"\d+:\d{2}:\d{2}";

        private static readonly Regex ProgressLine = new Regex(
            @"^\s*Encoding:\s*(?<frame>\d+)(?:\s*/\s*(?<total>\d+))?\s+Frames" +
            @"(?:\s+@\s+(?<speed>" + Num + @")\s+(?<unit>fpm|fps))?" +
            @"(?:\s+\|\s+(?<kbps>" + Num + @")\s+kb/s)?" +
            @"(?:\s+\|\s+Size:\s+(?<size>" + Num + @")\s+MB(?:\s+\[(?<esize>" + Num + @")\s+MB\])?)?" +
            @"(?:\s+\|\s+Time:\s+(?<elapsed>" + Time + @")(?:\s+\[(?<eta>-?" + Time + @")\])?)?",
            RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);

        /// <summary>number of frames encoded so far</summary>
        public ulong Frame { get; private set; }
        /// <summary>number of frames to encode, 0 when SVT-AV1 does not print it (the caller can fill it in)</summary>
        public ulong FrameTotal { get; set; }
        /// <summary>encoding speed in frames per second (fpm values are converted)</summary>
        public double? Fps { get; private set; }
        public double? BitrateKbps { get; private set; }
        /// <summary>size of the output so far, in MB</summary>
        public double? SizeMB { get; private set; }
        /// <summary>size of the output estimated by SVT-AV1 once the encoding is finished, in MB</summary>
        public double? EstimatedSizeMB { get; private set; }
        public TimeSpan? Elapsed { get; private set; }
        /// <summary>remaining time estimated by SVT-AV1 (printed as a negative value, returned as a positive one)</summary>
        public TimeSpan? Remaining { get; private set; }

        /// <summary>
        /// removes the ANSI escape sequences (colors) from a line
        /// </summary>
        public static string StripAnsi(string line)
        {
            return String.IsNullOrEmpty(line) ? line : AnsiEscape.Replace(line, String.Empty);
        }

        /// <summary>
        /// parses a progress line, which must not contain ANSI escape sequences anymore (see StripAnsi)
        /// </summary>
        public static bool TryParse(string line, out svtav1Progress progress)
        {
            progress = null;
            if (String.IsNullOrEmpty(line))
                return false;

            Match m = ProgressLine.Match(line);
            if (!m.Success)
                return false;

            ulong frame, total = 0;
            if (!ulong.TryParse(m.Groups["frame"].Value, NumberStyles.None, CultureInfo.InvariantCulture, out frame))
                return false;
            if (m.Groups["total"].Success && !ulong.TryParse(m.Groups["total"].Value, NumberStyles.None, CultureInfo.InvariantCulture, out total))
                return false;

            svtav1Progress p = new svtav1Progress();
            p.Frame = frame;
            p.FrameTotal = total;

            double? speed = ParseDouble(m.Groups["speed"]);
            if (speed.HasValue)
                p.Fps = m.Groups["unit"].Value == "fpm" ? speed.Value / 60.0 : speed.Value;
            p.BitrateKbps = ParseDouble(m.Groups["kbps"]);
            p.SizeMB = ParseDouble(m.Groups["size"]);
            p.EstimatedSizeMB = ParseDouble(m.Groups["esize"]);
            p.Elapsed = ParseTime(m.Groups["elapsed"]);
            p.Remaining = ParseTime(m.Groups["eta"]);

            progress = p;
            return true;
        }

        /// <summary>
        /// readable one line summary
        /// </summary>
        public override string ToString()
        {
            return ToSummary(true);
        }

        /// <summary>
        /// readable one line summary
        /// </summary>
        /// <param name="withRemaining">false to leave out the remaining time, which is not reliable at the beginning</param>
        public string ToSummary(bool withRemaining)
        {
            CultureInfo ci = CultureInfo.InvariantCulture;
            string strSummary = "Progress: " + Frame.ToString(ci) + (FrameTotal > 0 ? "/" + FrameTotal.ToString(ci) : String.Empty) + " frames";
            if (FrameTotal > 0)
                strSummary += " (" + ((double)Frame * 100.0 / FrameTotal).ToString("0.0", ci) + "%)";
            if (Fps.HasValue)
                strSummary += " | " + Fps.Value.ToString("0.00", ci) + " fps";
            if (BitrateKbps.HasValue)
                strSummary += " | " + BitrateKbps.Value.ToString("0.0", ci) + " kb/s";
            if (SizeMB.HasValue)
                strSummary += " | size " + SizeMB.Value.ToString("0.0", ci) + " MB" + (EstimatedSizeMB.HasValue ? " (estimated " + EstimatedSizeMB.Value.ToString("0.0", ci) + " MB)" : String.Empty);
            if (Elapsed.HasValue)
                strSummary += " | elapsed " + FormatTime(Elapsed.Value);
            if (withRemaining && Remaining.HasValue)
                strSummary += " | remaining " + FormatTime(Remaining.Value);
            return strSummary;
        }

        private static double? ParseDouble(Group g)
        {
            double d;
            if (g.Success && double.TryParse(g.Value, NumberStyles.Float, CultureInfo.InvariantCulture, out d) && !double.IsNaN(d) && !double.IsInfinity(d))
                return d;
            return null;
        }

        // h:mm:ss, the hours can be greater than 24 and the value can be negative
        private static TimeSpan? ParseTime(Group g)
        {
            if (!g.Success)
                return null;

            string[] parts = g.Value.TrimStart('-').Split(':');
            int h, m, s;
            if (parts.Length == 3
                && int.TryParse(parts[0], NumberStyles.None, CultureInfo.InvariantCulture, out h)
                && int.TryParse(parts[1], NumberStyles.None, CultureInfo.InvariantCulture, out m)
                && int.TryParse(parts[2], NumberStyles.None, CultureInfo.InvariantCulture, out s))
                return new TimeSpan(h, m, s);
            return null;
        }

        private static string FormatTime(TimeSpan t)
        {
            return ((int)t.TotalHours).ToString(CultureInfo.InvariantCulture) + ":" + t.Minutes.ToString("00", CultureInfo.InvariantCulture) + ":" + t.Seconds.ToString("00", CultureInfo.InvariantCulture);
        }
    }
}
