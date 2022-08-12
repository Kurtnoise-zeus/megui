// ****************************************************************************
// 
// Copyright (C) 2005-2018 Doom9 & al
// 
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
// 
// ****************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;

using MeGUI;
using MeGUI.core.util;

namespace eac3to
{
    /// <summary>A Stream of StreamType Audio</summary>
    public class AudioStream : Stream
    {
        public AudioStreamType AudioType { get; set; }
        public override string Language { get; set; }
        public string TypeCore;
        public bool HasDialNorm;
        public bool ParsingFailed;

        public override object[] ExtractTypes
        {
            get
            {
                bool bDefaultToHD = MainForm.Instance.Settings.Eac3toDefaultToHD;
                bool bEnableEncoder = MainForm.Instance.Settings.Eac3toEnableEncoder;
                bool bEnableDecoder = MainForm.Instance.Settings.Eac3toEnableDecoder;

                List<string> arrType = new List<string>();

                switch (AudioType)
                {
                    case AudioStreamType.AAC:
                        arrType.Add("AAC"); break;
                    case AudioStreamType.AC3:
                        arrType.Add("AC3"); break;
                    case AudioStreamType.PCM:
                    case AudioStreamType.RAW:
                    case AudioStreamType.WAV:
                    case AudioStreamType.WAVS:
                        arrType.Add("W64");
                        if (bEnableDecoder)
                            arrType.AddRange(new string[] { "WAV", "WAVS", "RAW", "RF64" });
                        break;
                    case AudioStreamType.DTS:
                        if (!String.IsNullOrEmpty(TypeCore) || ParsingFailed)
                        {
                            if (bDefaultToHD && !ParsingFailed)
                                arrType.AddRange(new string[] { "DTS", "DTS_CORE" });
                            else
                                arrType.AddRange(new string[] { "DTS_CORE", "DTS" });
                        }
                        else
                            arrType.Add("DTS");
                        break;
                    case AudioStreamType.EAC3:
                        arrType.AddRange(new string[] { "EAC3_CORE", "EAC3" }); break;
                    case AudioStreamType.FLAC:
                        arrType.Add("FLAC"); break;
                    case AudioStreamType.MP2:
                        arrType.Add("MP2"); break;
                    case AudioStreamType.MP3:
                        arrType.Add("MP3"); break;
                    case AudioStreamType.TrueHD:
                        if (!String.IsNullOrEmpty(TypeCore) || ParsingFailed)
                        {
                            if (bDefaultToHD && !ParsingFailed)
                                arrType.AddRange(new string[] { "THD", "THD+AC3", "AC3" });
                            else
                                arrType.AddRange(new string[] { "AC3", "THD", "THD+AC3" });
                        }
                        else
                        {
                            arrType.Add("THD");
                            if (bEnableEncoder)
                                arrType.Add("THD+AC3");
                        }
                        break;
                    case AudioStreamType.TTA:
                        arrType.Add("TTA"); break;
                    case AudioStreamType.VORBIS:
                        arrType.Add("OGG"); break;
                    case AudioStreamType.WAVPACK:
                        arrType.Add("WV"); break;
                    default:
                        return new object[] { "UNKNOWN" };
                }

                if (bEnableEncoder)
                {
                    if (!arrType.Contains("AC3"))
                        arrType.Add("AC3");
                    if (!arrType.Contains("FLAC"))
                        arrType.Add("FLAC");
                    if (!arrType.Contains("AAC") 
                        && System.IO.File.Exists(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(MainForm.Instance.Settings.Eac3to.Path), "neroaacenc.exe")))
                        arrType.Add("AAC");
                }

                if (bEnableDecoder && !arrType.Contains("W64"))
                    arrType.AddRange(new string[] { "W64", "WAV", "WAVS", "RAW", "RF64" });

                return arrType.Cast<object>().ToArray();
            }
        }

        public AudioStream(string s, LogItem _log) : base(StreamType.Audio, s, _log)
        {
            if (string.IsNullOrEmpty(s))
                throw new ArgumentNullException("s", "The string 's' cannot be null or empty.");
            TypeCore = string.Empty;
            ParsingFailed = false;
            HasDialNorm = System.Text.RegularExpressions.Regex.IsMatch(s, ", dialnorm: ", System.Text.RegularExpressions.RegexOptions.Compiled);
        }

        new public static Stream Parse(string s, LogItem _log)
        {
            //2: AC3, English, 2.0 channels, 192kbps, 48khz, dialnorm: -27dB, -8ms
            //4: TrueHD, English, 5.1 channels, 48khz, dialnorm: -27dB

            if (string.IsNullOrEmpty(s))
                throw new ArgumentNullException("s", "The string 's' cannot be null or empty.");

            AudioStream audioStream = new AudioStream(s, _log);

            string type = s.Substring(s.IndexOf(":") + 1, s.IndexOf(',') - s.IndexOf(":") - 1).Trim();
            switch (type.ToUpperInvariant())
            {
                case "AC3":
                case "AC3 EX":
                case "AC3 SURROUND":
                    audioStream.AudioType = AudioStreamType.AC3;
                    break;
                case "DTS":
                case "DTS-ES":
                case "DTS EXPRESS":
                case "DTS MASTER AUDIO":
                case "DTS HI-RES":
                    audioStream.AudioType = AudioStreamType.DTS;
                    break;
                case "EAC3":
                case "E-AC3":
                case "E-AC3 EX":
                case "E-AC3 SURROUND":
                    audioStream.AudioType = AudioStreamType.EAC3;
                    break;
                case "TRUEHD":
                case "TRUEHD (ATMOS)":
                case "TRUEHD/AC3":
                case "TRUEHD/AC3 (ATMOS)":
                    audioStream.AudioType = AudioStreamType.TrueHD;
                    break;
                case "PCM":
                    audioStream.AudioType = AudioStreamType.PCM;
                    break;
                case "WAV":
                    audioStream.AudioType = AudioStreamType.WAV;
                    break;
                case "WAVS":
                    audioStream.AudioType = AudioStreamType.WAVS;
                    break;
                case "MP2":
                    audioStream.AudioType = AudioStreamType.MP2;
                    break;
                case "MP3":
                    audioStream.AudioType = AudioStreamType.MP3;
                    break;
                case "AAC":
                    audioStream.AudioType = AudioStreamType.AAC;
                    break;
                case "FLAC":
                    audioStream.AudioType = AudioStreamType.FLAC;
                    break;
                case "TTA1":
                    audioStream.AudioType = AudioStreamType.TTA;
                    break;
                case "WAVPACK4":
                    audioStream.AudioType = AudioStreamType.WAVPACK;
                    break;
                case "VORBIS":
                    audioStream.AudioType = AudioStreamType.VORBIS;
                    break;
                case "RAW/PCM":
                case "RAW":
                    audioStream.AudioType = AudioStreamType.RAW;
                    break;
                default:
                    _log.Warn("\"" + type + "\" is not known. " + s);
                    audioStream.AudioType = AudioStreamType.UNKNOWN;
                    break;
            }

            if (s.ToLowerInvariant().EndsWith(", unknown parameters"))
                audioStream.ParsingFailed = true;

            return audioStream;
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}