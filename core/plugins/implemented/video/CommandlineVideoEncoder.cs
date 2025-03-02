// ****************************************************************************
// 
// Copyright (C) 2005-2025 Doom9 & al
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
using System.Collections;
using System.IO;
using System.Text;

using MeGUI.core.util;

namespace MeGUI
{
    public delegate void EncoderOutputCallback(string line, int type);

    public abstract class CommandlineVideoEncoder : CommandlineJobProcessor<VideoJob>
    {
        #region variables
#pragma warning disable CA1051 // Do not declare visible instance fields
        protected ulong NumberOfFrames;
        protected int Vres = 0;
        protected int Fps_d = 0;
        protected int Hres = 0;
        protected int Fps_n = 0;
#pragma warning restore CA1051 // Do not declare visible instance fields
        #endregion

        public CommandlineVideoEncoder() : base()
        {
        }

        #region helper methods
        protected override void checkJobIO()
        {
            base.checkJobIO();
            Su.Status = "Encoding video...";
            getInputProperties(Job);
        }

        private string GetAVSFileContent()
        {
            if (!File.Exists(Job.Input) || !Path.GetExtension(Job.Input).ToLowerInvariant().Equals(".avs"))
                return string.Empty;

            string strAVSFile = String.Empty;
            try
            {
                StreamReader sr = new StreamReader(Job.Input, Encoding.Default);
                strAVSFile = sr.ReadToEnd();
                sr.Close();
            }
            catch (Exception) { }

            return strAVSFile;
        }

        /// <summary>
        /// tries to open the video source and gets the number of frames from it, or 
        /// exits with an error
        /// </summary>
        /// <param name="videoSource">the AviSynth script</param>
        /// <param name="error">return parameter for all errors</param>
        /// <returns>true if the file could be opened, false if not</returns>
        protected void getInputProperties(VideoJob job)
        {
            log.LogValue("AviSynth input script", GetAVSFileContent());
 
            if (job == null)
            {
                log.LogEvent("job not found", ImageType.Error);
                    return;
            }
            
            double fps;
            Dar d = Dar.A1x1;
            AviSynthColorspace colorspace_original;
            JobUtil.GetAllInputProperties(job.Input, out NumberOfFrames, out fps, out Fps_n, out Fps_d, out Hres, out Vres, out d, out colorspace_original);

            Dar? dar = job.DAR;
            Su.NbFramesTotal = NumberOfFrames;
            Su.FPS = fps;
            Su.ClipLength = TimeSpan.FromSeconds((double)NumberOfFrames / fps);

            if (!job.DAR.HasValue)
                job.DAR = d;

            // log
            if (log == null)
                return;

            log.LogEvent("resolution: " + Hres + "x" + Vres);
            log.LogEvent("frame rate: " + Fps_n + "/" + Fps_d);
            log.LogEvent("frames: " + NumberOfFrames);
            log.LogEvent("length: " + string.Format(new System.Globalization.CultureInfo("en-US"), "{0:00}:{1:00}:{2:00}.{3:000}",
            (int)(Su.ClipLength.Value.TotalHours), Su.ClipLength.Value.Minutes, Su.ClipLength.Value.Seconds, Su.ClipLength.Value.Milliseconds));
            if (dar.HasValue && d.AR == dar.Value.AR)
            {
                log.LogValue("aspect ratio", d);
            }
            else
            {
                log.LogValue("aspect ratio (avs)", d);
                if (dar.HasValue)
                    log.LogValue("aspect ratio (job)", dar.Value);
            }
                
            if (Int32.TryParse(colorspace_original.ToString(), out int result))
                log.LogValue("color space", colorspace_original.ToString(), ImageType.Warning);
            else
                log.LogValue("color space", colorspace_original.ToString());

            string strEncoder = "ffmpeg";
            if (this is XviDEncoder)
                strEncoder = "xvid";
            else if (this is x264Encoder && (MainForm.Instance.Settings.IsMeGUIx64 || !MainForm.Instance.Settings.Usex64Tools))
                strEncoder = "x264";
            else if (this is svtav1psyEncoder)
                strEncoder = "svtav1psy";

            AviSynthColorspace colorspace_target = AviSynthColorspaceHelper.GetConvertedColorspace(strEncoder, colorspace_original);
            int iBit = AviSynthColorspaceHelper.GetInputBitsDepth(colorspace_original);
            log.LogValue("Bits Depth", iBit.ToString());

            if (colorspace_original != colorspace_target
                && !AviSynthColorspaceHelper.IsConvertedToColorspace(job.Input, colorspace_target.ToString()))
            {
                if (MainForm.Instance.DialogManager.AddConvertTo(colorspace_original.ToString(), colorspace_target.ToString()))
                {
                    AviSynthColorspaceHelper.AppendConvertTo(job.Input, colorspace_target, colorspace_original);
                    log.LogValue("AviSynth input script (appended)", GetAVSFileContent());

                    // Check everything again, to see if it is all fixed now
                    AviSynthColorspace colorspace_converted;
                    JobUtil.GetAllInputProperties(job.Input, out NumberOfFrames, out fps, out Fps_n, out Fps_d, out Hres, out Vres, out d, out colorspace_converted);
                    if (colorspace_original != colorspace_converted)
                        log.LogValue("color space converted", colorspace_converted.ToString());
                    else
                        log.LogEvent("color space not supported, conversion failed", ImageType.Error);
                }
                else
                    log.LogEvent("color space not supported", ImageType.Error);
            }
        }

        protected override void doExitConfig()
        {
            if (!Su.HasError && !Su.WasAborted)
                compileFinalStats();

            base.doExitConfig();
        }

        /// <summary>
        /// compiles final bitrate statistics
        /// </summary>
        protected void compileFinalStats()
        {
            try
            {
                if (string.IsNullOrEmpty(Job.Output) || !File.Exists(Job.Output))
                    return;

                FileInfo fi = new FileInfo(Job.Output);
                long size = fi.Length; // size in bytes
                long bitrate = (long)((double)(size * 8.0) / (Su.ClipLength.Value.TotalSeconds * 1000.0));

                LogItem stats = log.Info(string.Format("[{0:G}] {1}", DateTime.Now, "Final statistics"));
                if (Job.Settings.VideoEncodingType == VideoCodecSettings.VideoEncodingMode.CQ) // CQ mode
                    stats.LogValue("Constant Quantizer Mode", "Quantizer " + Job.Settings.BitrateQuantizer + " computed...");
                else if (Job.Settings.VideoEncodingType == VideoCodecSettings.VideoEncodingMode.quality)
                    stats.LogValue("Constant Quality Mode", "Quality " + Job.Settings.BitrateQuantizer + " computed...");
                else
                    stats.LogValue("Video Bitrate Desired", Job.Settings.BitrateQuantizer + " kbit/s");
                stats.LogValue("Video Bitrate Obtained (approximate)", bitrate + " kbit/s");

                if ((this is x264Encoder || this is x265Encoder) && Su.NbFramesDone != Su.NbFramesTotal)
                {
                    stats.LogEvent("Number of encoded frames does not match the source: " + Su.NbFramesDone + "/" + Su.NbFramesTotal, ImageType.Error);
                    Su.HasError = true;
                }
                else
                    stats.LogValue("Frames Source vs Encoded","Number of encoded frames match the source: " + Su.NbFramesDone + "/" + Su.NbFramesTotal);
            }
            catch (Exception e)
            {
                log.LogValue("Exception in compileFinalStats", e, ImageType.Warning);
            }
        }
        #endregion

        protected bool setFrameNumber(string frameString)
        {
            int iCurrentFrameNumber;
            if (int.TryParse(frameString, out iCurrentFrameNumber))
            {
                if (iCurrentFrameNumber < 0)
                {
                    Su.NbFramesDone = 0;
                    Su.PercentageCurrent = 0;
                }
                else
                    {
                        Su.NbFramesDone = (ulong)iCurrentFrameNumber;
                        Su.PercentageCurrent = (decimal)Su.NbFramesDone / Su.NbFramesTotal * 100;
                    }
                return true;
            }
            return false;
        }
    }
}