using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace MeGUI
{
    /// <summary>
    /// Header sent in front of the audio data on the standard input of the encoder
    /// </summary>
    public enum GCLInputHeader
    {
        [EnumTitle("WAV")]
        WAV,
        [EnumTitle("W64 (Wave64)")]
        W64,
        [EnumTitle("RF64")]
        RF64,
        [EnumTitle("None (raw PCM)")]
        NONE
    }

    /// <summary>
    /// Settings of the generic command line audio encoder ("gcl").
    /// The audio is sent to the standard input of the configured encoder. The only encoder specific
    /// settings are the encoder executable, the command line, the output file extension and the
    /// header of the audio sent to the encoder (WAV by default).
    /// </summary>
    public class GCLSettings : AudioCodecSettings
    {
        public static readonly string ID = "gcl";
        public static readonly string DefaultCommandLine = "-hide_banner -i - -y \"{output}\"";
        public static readonly string DefaultExtension = "mka";

        /// <summary>
        /// Placeholders which can be used in the command line (case insensitive)
        /// </summary>
        public static readonly GCLInputHeader[] SupportedInputHeaders = new GCLInputHeader[] { GCLInputHeader.WAV, GCLInputHeader.W64, GCLInputHeader.RF64, GCLInputHeader.NONE };

        public static readonly string[] SupportedPlaceholders = new string[] { "output", "samplerate", "bits", "channels", "samples", "size" };

        private string encoderPath;
        private string commandLine;
        private string outputExtension;
        private GCLInputHeader inputHeader;

        public GCLSettings() : base(ID, AudioCodec.UNKNOWN, AudioEncoderType.GCL, 128)
        {
            base.DownmixMode = ChannelMode.KeepOriginal;
            encoderPath = string.Empty;
            commandLine = DefaultCommandLine;
            outputExtension = DefaultExtension;
            inputHeader = GCLInputHeader.WAV;
        }

        /// <summary>
        /// full path of the encoder executable (a relative path is resolved against the MeGUI folder)
        /// </summary>
        public string EncoderPath
        {
            get { return encoderPath; }
            set { encoderPath = (value ?? string.Empty).Trim(); }
        }

        /// <summary>
        /// command line arguments passed to the encoder, see BuildCommandLine for the placeholders
        /// </summary>
        public string CommandLine
        {
            get { return commandLine; }
            set { commandLine = (value ?? string.Empty).Trim(); }
        }

        /// <summary>
        /// extension of the output file, without leading dot
        /// </summary>
        public string OutputExtension
        {
            get { return outputExtension; }
            set { outputExtension = NormalizeExtension(value); }
        }

        /// <summary>
        /// header sent to the standard input of the encoder: WAV (default), W64 and RF64 for files larger than 4 GB,
        /// or none for raw PCM (use {samplerate}, {bits} and {channels} in the command line to describe it)
        /// </summary>
        public GCLInputHeader InputHeader
        {
            get { return inputHeader; }
            set { inputHeader = value; }
        }

        /// <summary>
        /// returns the output type matching the configured extension. This is a method (not a property)
        /// on purpose: properties are compared when two profiles are checked for equality.
        /// </summary>
        public AudioType GetOutputType()
        {
            return new AudioType(ID, "Generic command line audio files", outputExtension, null, AudioCodec.UNKNOWN);
        }

        /// <summary>
        /// removes the leading dots, spaces and characters which are not allowed in a file name
        /// </summary>
        public static string NormalizeExtension(string extension)
        {
            if (extension == null)
                return DefaultExtension;

            StringBuilder sb = new StringBuilder();
            char[] invalid = Path.GetInvalidFileNameChars();
            const string forbidden = "\\/:*?\"<>|"; // same characters on every platform
            foreach (char c in extension.Trim().TrimStart('.'))
            {
                if (!char.IsWhiteSpace(c) && Array.IndexOf(invalid, c) < 0 && forbidden.IndexOf(c) < 0)
                    sb.Append(c);
            }
            return sb.Length == 0 ? DefaultExtension : sb.ToString();
        }

        /// <summary>
        /// Converts the command line of the profile into the format used by AviSynthAudioEncoder, which is
        /// handed over to string.Format: {0}=output file, {1}=sample rate, {2}=bits per sample, {3}=channels,
        /// {4}=sample count and {5}=size in bytes.
        /// Placeholders (case insensitive): {output} (always quoted, quotes around it are optional),
        /// {samplerate}, {bits}, {channels}, {samples} and {size}. Without {output} the quoted output
        /// file is appended at the end. All other braces are kept as is.
        /// </summary>
        public static string BuildCommandLine(string commandLine)
        {
            string strCommandLine = commandLine ?? string.Empty;

            // escape all braces, the placeholders are restored afterwards
            strCommandLine = strCommandLine.Replace("{", "{{").Replace("}", "}}");

            bool bOutput = Regex.IsMatch(strCommandLine, @"\{\{output\}\}", RegexOptions.IgnoreCase);
            strCommandLine = Regex.Replace(strCommandLine, "\"?\\{\\{output\\}\\}\"?", "\"{0}\"", RegexOptions.IgnoreCase);
            strCommandLine = Regex.Replace(strCommandLine, @"\{\{samplerate\}\}", "{1}", RegexOptions.IgnoreCase);
            strCommandLine = Regex.Replace(strCommandLine, @"\{\{bits\}\}", "{2}", RegexOptions.IgnoreCase);
            strCommandLine = Regex.Replace(strCommandLine, @"\{\{channels\}\}", "{3}", RegexOptions.IgnoreCase);
            strCommandLine = Regex.Replace(strCommandLine, @"\{\{samples\}\}", "{4}", RegexOptions.IgnoreCase);
            strCommandLine = Regex.Replace(strCommandLine, @"\{\{size\}\}", "{5}", RegexOptions.IgnoreCase);

            if (!bOutput)
                strCommandLine = (strCommandLine + " \"{0}\"").Trim();

            return strCommandLine;
        }
    }
}
