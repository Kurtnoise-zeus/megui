// ****************************************************************************
// 
// Copyright (C) 2005-2022 Doom9 & al
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
using System.Globalization;
using System.Text;

using MeGUI.core.util;

namespace MeGUI
{
    class XviDEncoder : CommandlineVideoEncoder
    {
        public static readonly JobProcessorFactory Factory =
new JobProcessorFactory(new ProcessorFactory(init), "XviDEncoder");
        private bool bAVSKeyToBeRemoved = true;

        private static IJobProcessor init(MainForm mf, Job j)
        {
            if (j is VideoJob &&
                (j as VideoJob).Settings is xvidSettings)
                return new XviDEncoder(mf.Settings.XviD.Path);
            return null;
        }

        public XviDEncoder(string exePath)
            : base()
        {
            UpdateCacher.CheckPackage("xvid_encraw");
            executable = exePath;
        }

        public override void ProcessLine(string line, StreamType stream, ImageType oType)
        {
            if (bAVSKeyToBeRemoved)
                RemoveAVSKeys();

            if (line.IndexOf(": key") != -1) // we found a position line, parse it
            {
                int frameNumberEnd = line.IndexOf(":");
                if (base.setFrameNumber(line.Substring(0, frameNumberEnd).Trim()))
                    return;
            }

            if (line.ToLowerInvariant().Contains("error")
                || line.ToLowerInvariant().Contains("usage") // we get the usage message if there's an unrecognized parameter
                || line.ToLowerInvariant().Contains("avistreamwrite"))
                oType = ImageType.Error;
            else if (line.ToLowerInvariant().Contains("warning"))
                oType = ImageType.Warning;
            base.ProcessLine(line, stream, oType);
        }

        protected override string Commandline
        {
            get
            {
                SetAVSKeys();
                return genCommandline(job.Input, job.Output, job.DAR, job.Settings as xvidSettings, hres, vres, fps_n, fps_d, job.Zones, base.log);
            }
        }

        /// <summary>
        /// Checks if temporary AVS keys have to be set. 
        /// This is required so that Xvid can find/use the avisynth.dll - especially in the portable avisynth mode
        /// </summary>
        private void SetAVSKeys()
        {
            bool bKeyWritten = false;

            try
            {
                // try to find the class GUID for AVS
                string guid = (string)Microsoft.Win32.Registry.GetValue(@"HKEY_CLASSES_ROOT\AVIFile\Extensions\AVS", null, string.Empty);  
                if (String.IsNullOrEmpty(guid))
                {
                    // GUID not found - set it to the default value
                    guid = "{E6D6B700-124D-11D4-86F3-DB80AFD98778}";
                    bKeyWritten = true;

                    // write the keys if they do not exist
                    Microsoft.Win32.RegistryKey oKey;
                    oKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"Software\Classes\AVIFile");
                    if (oKey == null)
                        Microsoft.Win32.Registry.SetValue(@"HKEY_CURRENT_USER\Software\Classes\AVIFile", "MeGUI", "true");

                    oKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"Software\Classes\AVIFile\Extensions");
                    if (oKey == null)
                        Microsoft.Win32.Registry.SetValue(@"HKEY_CURRENT_USER\Software\Classes\AVIFile\Extensions", "MeGUI", "true");

                    oKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"Software\Classes\AVIFile\Extensions\AVS");
                    if (oKey == null)
                        Microsoft.Win32.Registry.SetValue(@"HKEY_CURRENT_USER\Software\Classes\AVIFile\Extensions\AVS", "MeGUI", "true");

                    // finally the GUID value has to be set
                    Microsoft.Win32.Registry.SetValue(@"HKEY_CURRENT_USER\Software\Classes\AVIFile\Extensions\AVS", null, guid);
                }

                // check which registry part has to be changed
                string strWOWkey = string.Empty;
                if (!MainForm.Instance.Settings.IsMeGUIx64 && MainForm.Instance.Settings.IsOSx64)
                    strWOWkey = @"Wow6432Node\";

                // try to find the avisynth.dll entry
                string strAViSynthDLL = (string)Microsoft.Win32.Registry.GetValue(@"HKEY_CLASSES_ROOT\" + strWOWkey + @"CLSID\" + guid + @"\InProcServer32", null, string.Empty);
                if (String.IsNullOrEmpty(strAViSynthDLL) || !strAViSynthDLL.ToLowerInvariant().Equals("avisynth.dll"))
                {
                    // avisynth.dll value could not be found
                    bKeyWritten = true;

                    // write the keys if they do not exist
                    Microsoft.Win32.RegistryKey oKey;
                    oKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"Software\Classes\" + strWOWkey + @"CLSID\" + guid);
                    if (oKey == null)
                        Microsoft.Win32.Registry.SetValue(@"HKEY_CURRENT_USER\Software\Classes\" + strWOWkey + @"CLSID\" + guid, "MeGUI", "true");

                    oKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"Software\Classes\" + strWOWkey + @"CLSID\" + guid + @"\InProcServer32");
                    if (oKey == null)
                        Microsoft.Win32.Registry.SetValue(@"HKEY_CURRENT_USER\Software\Classes\" + strWOWkey + @"CLSID\" + guid + @"\InProcServer32", "MeGUI", "true");

                    // finally the avisynth value has to be set
                    Microsoft.Win32.Registry.SetValue(@"HKEY_CURRENT_USER\Software\Classes\" + strWOWkey + @"CLSID\" + guid + @"\InProcServer32", null, "AviSynth.dll");
                }
            }
            catch (Exception ex)
            {
                log.LogEvent("Temporary Xvid registry entries could not be added: " + ex.Message, ImageType.Error);
            }

            if (bKeyWritten)
                log.LogEvent("Temporary Xvid registry entries have been added", ImageType.Information);
        }

        /// <summary>
        /// If set by MeGUI the temporary AVS keys have to be removed again
        /// </summary>
        private void RemoveAVSKeys()
        {
            bool bKeyRemoved = false;

            try
            {
                string guid = (string)Microsoft.Win32.Registry.GetValue(@"HKEY_CLASSES_ROOT\AVIFile\Extensions\AVS", null, string.Empty);
                if (String.IsNullOrEmpty(guid))
                {
                    // GUID not found - set it
                    guid = "{E6D6B700-124D-11D4-86F3-DB80AFD98778}";
                }

                string strMeGUIWritten = (string)Microsoft.Win32.Registry.GetValue(@"HKEY_CURRENT_USER\Software\Classes\AVIFile\Extensions\AVS", "MeGUI", string.Empty);
                if (!String.IsNullOrEmpty(strMeGUIWritten))
                {
                    Microsoft.Win32.Registry.CurrentUser.DeleteSubKeyTree(@"Software\Classes\AVIFile\Extensions\AVS", false);
                    bKeyRemoved = true;
                }

                strMeGUIWritten = (string)Microsoft.Win32.Registry.GetValue(@"HKEY_CURRENT_USER\Software\Classes\AVIFile\Extensions", "MeGUI", string.Empty);
                if (!String.IsNullOrEmpty(strMeGUIWritten))
                {
                    Microsoft.Win32.Registry.CurrentUser.DeleteSubKeyTree(@"Software\Classes\AVIFile\Extensions", false);
                    bKeyRemoved = true;
                }

                strMeGUIWritten = (string)Microsoft.Win32.Registry.GetValue(@"HKEY_CURRENT_USER\Software\Classes\AVIFile", "MeGUI", string.Empty);
                if (!String.IsNullOrEmpty(strMeGUIWritten))
                {
                    Microsoft.Win32.Registry.CurrentUser.DeleteSubKeyTree(@"Software\Classes\AVIFile", false);
                    bKeyRemoved = true;
                }

                // check which registry part has been changed
                string strWOWkey = string.Empty;
                if (!MainForm.Instance.Settings.IsMeGUIx64 && MainForm.Instance.Settings.IsOSx64)
                    strWOWkey = @"Wow6432Node\";

                strMeGUIWritten = (string)Microsoft.Win32.Registry.GetValue(@"HKEY_CURRENT_USER\Software\Classes\" + strWOWkey + @"CLSID\" + guid + @"\InProcServer32", "MeGUI", string.Empty);
                if (!String.IsNullOrEmpty(strMeGUIWritten))
                {
                    Microsoft.Win32.Registry.CurrentUser.DeleteSubKeyTree(@"Software\Classes\" + strWOWkey + @"CLSID\" + guid + @"\InProcServer32", false);
                    bKeyRemoved = true;
                }

                strMeGUIWritten = (string)Microsoft.Win32.Registry.GetValue(@"HKEY_CURRENT_USER\Software\Classes\" + strWOWkey + @"CLSID\" + guid, "MeGUI", string.Empty);
                if (!String.IsNullOrEmpty(strMeGUIWritten))
                {
                    Microsoft.Win32.Registry.CurrentUser.DeleteSubKeyTree(@"Software\Classes\" + strWOWkey + @"CLSID\" + guid, false);
                    bKeyRemoved = true;
                }
            }
            catch (Exception ex)
            {
                log.LogEvent("Temporary Xvid registry changes could not be removed: " + ex.Message, ImageType.Error);
            }

            bAVSKeyToBeRemoved = false;
            if (bKeyRemoved)
                log.LogEvent("Temporary Xvid registry entries have been removed", ImageType.Information);
        }

        public static string genCommandline(string input, string output, Dar? d, xvidSettings xs, int hres, int vres, int fps_n, int fps_d, Zone[] zones, LogItem log)
        {
            StringBuilder sb = new StringBuilder();
            CultureInfo ci = new CultureInfo("en-us");

            // log
            if (log != null)
            {
                if (!String.IsNullOrEmpty(xs.CustomEncoderOptions))
                    log.LogEvent("custom command line: " + xs.CustomEncoderOptions);
            }

            #region input options
            if (log != null)
                sb.Append("-i \"" + input + "\" ");
            #endregion

            #region output options
            if (log != null)
                if (xs.VideoEncodingType != VideoCodecSettings.VideoEncodingMode.twopass1) // not 2 pass vbr first pass, add output filename and output type
                    sb.Append("-o \"" + output + "\" ");
            #endregion

            #region rate control options
            switch (xs.VideoEncodingType)
            {
                case VideoCodecSettings.VideoEncodingMode.twopass1: // 2 pass first pass
                    sb.Append("-pass1 " + "\"" + xs.Logfile + "\" "); // add logfile
                    break;
                case VideoCodecSettings.VideoEncodingMode.twopass2: // 2 pass second pass
                case VideoCodecSettings.VideoEncodingMode.twopassAutomated: // automated twopass
                    sb.Append("-pass2 " + "\"" + xs.Logfile + "\" "); // add logfile
                    break;
            }
            if (!xs.CustomEncoderOptions.Contains("-bitrate ") && !xs.CustomEncoderOptions.Contains("-cq "))
            {
                if (xs.VideoEncodingType == VideoCodecSettings.VideoEncodingMode.CQ)
                    sb.Append("-cq " + xs.Quantizer.ToString(ci) + " "); // add quantizer
                else if (xs.BitrateQuantizer != 700)
                    sb.Append("-bitrate " + xs.BitrateQuantizer + " "); // add bitrate
            }
            if (!xs.CustomEncoderOptions.Contains("-max_key_interval ") && xs.KeyframeInterval != 300)
                sb.Append("-max_key_interval " + xs.KeyframeInterval + " ");
            if (zones != null && zones.Length > 0 && xs.CreditsQuantizer >= new decimal(1))
            {
                foreach (Zone zone in zones)
                {
                    if (zone.mode == ZONEMODE.Quantizer)
                        sb.Append("-zq " + zone.startFrame + " " + zone.modifier + " ");
                    if (zone.mode == ZONEMODE.Weight)
                    {
                        sb.Append("-zw " + zone.startFrame + " ");
                        double mod = (double)zone.modifier / 100.0;
                        sb.Append(mod.ToString(ci) + " ");
                    }
                }
            }
            #endregion

            #region single pass options
            if (xs.VideoEncodingType == VideoCodecSettings.VideoEncodingMode.CQ || xs.VideoEncodingType == VideoCodecSettings.VideoEncodingMode.CBR)
            {
                if (!xs.CustomEncoderOptions.Contains("-reaction ") && xs.ReactionDelayFactor != 16)
                    sb.Append("-reaction " + xs.ReactionDelayFactor + " ");
                if (!xs.CustomEncoderOptions.Contains("-averaging ") && xs.AveragingPeriod != 100)
                    sb.Append("-averaging " + xs.AveragingPeriod + " ");
                if (!xs.CustomEncoderOptions.Contains("-smoother ") && xs.RateControlBuffer != 100)
                    sb.Append("-smoother " + xs.RateControlBuffer + " ");
            }
            #endregion

            #region second pass options
            if (xs.VideoEncodingType != VideoCodecSettings.VideoEncodingMode.CQ && xs.VideoEncodingType != VideoCodecSettings.VideoEncodingMode.CBR)
            {
                if (!xs.CustomEncoderOptions.Contains("-kboost ") && xs.KeyFrameBoost != 10)
                    sb.Append("-kboost " + xs.KeyFrameBoost + " ");
                if (!xs.CustomEncoderOptions.Contains("-kthresh ") && xs.KeyframeThreshold != 1)
                    sb.Append("-kthresh " + xs.KeyframeThreshold + " ");
                if (!xs.CustomEncoderOptions.Contains("-kreduction ") && xs.KeyframeReduction != 20)
                    sb.Append("-kreduction " + xs.KeyframeReduction + " ");
                if (!xs.CustomEncoderOptions.Contains("-ostrength ") && xs.OverflowControlStrength != 5)
                    sb.Append("-ostrength " + xs.OverflowControlStrength + " ");
                if (!xs.CustomEncoderOptions.Contains("-oimprove ") && xs.MaxOverflowImprovement != 5)
                    sb.Append("-oimprove " + xs.MaxOverflowImprovement + " ");
                if (!xs.CustomEncoderOptions.Contains("-odegrade ") && xs.MaxOverflowDegradation != 5)
                    sb.Append("-odegrade " + xs.MaxOverflowDegradation + " ");
                if (!xs.CustomEncoderOptions.Contains("-chigh ") && xs.HighBitrateDegradation != 0)
                    sb.Append("-chigh " + xs.HighBitrateDegradation + " ");
                if (!xs.CustomEncoderOptions.Contains("-clow ") && xs.LowBitrateImprovement != 0)
                    sb.Append("-clow " + xs.LowBitrateImprovement + " ");
                // -overhead missing
                if (xs.XvidProfile != 0)
                {
                    int ivbvmax = 0, ivbvsize = 0, ivbvpeak = 0;
                    switch (xs.XvidProfile)
                    {
                        case 1:
                            ivbvmax = 4854000;
                            ivbvsize = 3145728;
                            ivbvpeak = 2359296;
                            break;
                        case 2:
                            ivbvmax = 9708400;
                            ivbvsize = 6291456;
                            ivbvpeak = 4718592;
                            break;
                        case 3:
                            ivbvmax = 20000000;
                            ivbvsize = 16000000;
                            ivbvpeak = 12000000;
                            break;
                        case 4:
                            ivbvmax = 200000;
                            ivbvsize = 262144;
                            ivbvpeak = 196608;
                            break;
                        case 5:
                            ivbvmax = 600000;
                            ivbvsize = 655360;
                            ivbvpeak = 491520;
                            break;
                        case 6:
                            ivbvmax = xs.VbvMaxRate;
                            ivbvsize = xs.VbvBuffer;
                            ivbvpeak = xs.VbvPeakRate;
                            break;
                    }
                    if (!xs.CustomEncoderOptions.Contains("-vbvsize ") && ivbvsize != 0)
                        sb.Append("-vbvsize " + ivbvsize + " ");
                    if (!xs.CustomEncoderOptions.Contains("-vbvmax ") && ivbvmax != 0)
                        sb.Append("-vbvmax " + ivbvmax + " ");
                    if (!xs.CustomEncoderOptions.Contains("-vbvpeak ") && ivbvpeak != 0)
                        sb.Append("-vbvpeak " + ivbvpeak + " ");
                }
            }
            #endregion

            #region bframes options
            if (!xs.CustomEncoderOptions.Contains("-max_bframes ") && xs.NbBframes != 2)
                sb.Append("-max_bframes " + xs.NbBframes + " ");
            if (xs.NbBframes > 0)
            {
                if (!xs.CustomEncoderOptions.Contains("-bquant_ratio ") && xs.BQuantRatio != 150)
                    sb.Append("-bquant_ratio " + xs.BQuantRatio + " ");
                if (!xs.CustomEncoderOptions.Contains("-bquant_offset ") && xs.BQuantOffset != 100)
                    sb.Append("-bquant_offset " + xs.BQuantOffset + " ");
            }
            #endregion

            #region other options
            // -noasm missing
            if (!xs.CustomEncoderOptions.Contains("-turbo") && xs.Turbo)
                sb.Append("-turbo ");
            if (!xs.CustomEncoderOptions.Contains("-quality ") && xs.MotionSearchPrecision != 6)
                sb.Append("-quality " + xs.MotionSearchPrecision + " ");
            if (!xs.CustomEncoderOptions.Contains("-vhqmode ") && xs.VHQMode != 1)
                sb.Append("-vhqmode " + xs.VHQMode + " ");
            if (xs.NbBframes > 0 && !xs.CustomEncoderOptions.Contains("-bvhq ") && xs.VHQForBframes)
                sb.Append("-bvhq ");
            // -metric missing
            if (!xs.CustomEncoderOptions.Contains("-qpel ") && xs.QPel)
                sb.Append("-qpel ");
            if (!xs.CustomEncoderOptions.Contains("-gmc ") && xs.GMC)
                sb.Append("-gmc ");
            if (!xs.CustomEncoderOptions.Contains("-qtype ") && xs.QuantizerMatrix == xvidSettings.MPEGMatrix)
                sb.Append("-qtype 1 ");
            else if (!xs.CustomEncoderOptions.Contains("-qmatrix ") && xs.QuantizerMatrix != xvidSettings.H263Matrix && !string.IsNullOrEmpty(xs.QuantizerMatrix))
                sb.Append("-qmatrix \"" + xs.QuantizerMatrix + "\" ");
            if (!xs.CustomEncoderOptions.Contains("-interlaced ") && xs.Interlaced)
            {
                sb.Append("-interlaced ");
                if (xs.BottomFieldFirst)
                    sb.Append("1 ");
                else
                    sb.Append("2 ");
            }
            if (!xs.CustomEncoderOptions.Contains("-nopacked") && !xs.PackedBitstream)
                sb.Append("-nopacked ");
            if (!xs.CustomEncoderOptions.Contains("-noclosed_gop") && !xs.ClosedGOP)
                sb.Append("-noclosed_gop ");
            if (!xs.CustomEncoderOptions.Contains("-masking ") && xs.HVSMasking != 0)
                sb.Append("-masking " + xs.HVSMasking + " ");
            // -stats missing
            // -ssim missing
            // -ssim_file missing
            // -debug missing
            // -vop_debug missing
            if (!xs.CustomEncoderOptions.Contains("-nochromame") && !xs.ChromaMotion)
                sb.Append("-nochromame ");
            if (!xs.CustomEncoderOptions.Contains("-notrellis") && !xs.Trellis)
                sb.Append("-notrellis ");
            if (!xs.CustomEncoderOptions.Contains("-imin ") && xs.MinQuantizer != 2)
                sb.Append("-imin " + xs.MinQuantizer + " ");
            if (!xs.CustomEncoderOptions.Contains("-imax ") && xs.MaxQuantizer != 31)
                sb.Append("-imax " + xs.MaxQuantizer + " ");
            if (xs.NbBframes > 0)
            {
                if (!xs.CustomEncoderOptions.Contains("-bmin ") && xs.MinBQuant != 2)
                    sb.Append("-bmin " + xs.MinBQuant + " ");
                if (!xs.CustomEncoderOptions.Contains("-bmax ") && xs.MaxBQuant != 31)
                    sb.Append("-bmax " + xs.MaxBQuant + " ");
            }
            if (!xs.CustomEncoderOptions.Contains("-pmin ") && xs.MinPQuant != 2)
                sb.Append("-pmin " + xs.MinPQuant + " ");
            if (!xs.CustomEncoderOptions.Contains("-pmax ") && xs.MaxPQuant != 31)
                sb.Append("-pmax " + xs.MaxPQuant + " ");
            if (!xs.CustomEncoderOptions.Contains("-drop ") && xs.FrameDropRatio != 0)
                sb.Append("-drop " + xs.FrameDropRatio + " ");
            // -start missing
            if (!xs.CustomEncoderOptions.Contains("-threads ") && xs.NbThreads > 0)
                sb.Append("-threads " + xs.NbThreads + " ");
            // -slices missing
            // -progress missing
            if (!xs.CustomEncoderOptions.Contains("-par ") && d.HasValue) // custom PAR mode
            {
                Sar s = d.Value.ToSar(hres, vres);
                if (s.X == 1 && s.Y == 1)
                    sb.Append("-par 1 ");
                else if (s.X == 12 && s.Y == 11)
                    sb.Append("-par 2 ");
                else if (s.X == 10 && s.Y == 11)
                    sb.Append("-par 3 ");
                else if (s.X == 16 && s.Y == 11)
                    sb.Append("-par 4 ");
                else if (s.X == 40 && s.Y == 33)
                    sb.Append("-par 5 ");
                else
                    sb.Append("-par " + s.X + ":" + s.Y + " ");
            }
            #endregion

            if (!String.IsNullOrEmpty(xs.CustomEncoderOptions.Trim())) // add custom encoder options
                sb.Append(xs.CustomEncoderOptions.Trim());

            return sb.ToString();
        }
    }
}