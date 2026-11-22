// ****************************************************************************
// 
// Copyright (C) 2005-2026 Doom9 & al
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
using System.Drawing;
using System.Runtime.InteropServices;

using MeGUI.core.util;
using MeGUI.core.plugins.interfaces;

namespace MeGUI
{
    // ── Factory ───────────────────────────────────────────────────────────────

    public sealed class AviSynthScriptEnvironment : IDisposable
    {
        public AviSynthScriptEnvironment() { }
        public void Dispose() { }

        public static AviSynthClip OpenScriptFile(string filePath)
            => AviSynthClip.Create(isFile: true, scriptOrPath: filePath, requireRGB24: false);

        public static AviSynthClip OpenScriptFile(string filePath, bool requireRGB24)
            => AviSynthClip.Create(isFile: true, scriptOrPath: filePath, requireRGB24: requireRGB24);

        public static AviSynthClip ParseScript(string script)
            => AviSynthClip.Create(isFile: false, scriptOrPath: script, requireRGB24: false);

        public static AviSynthClip ParseScript(string script, bool requireRGB24)
            => AviSynthClip.Create(isFile: false, scriptOrPath: script, requireRGB24: requireRGB24);

        // runInThread parameter kept for API compatibility — ignored in new implementation
        public static AviSynthClip ParseScript(string script, bool requireRGB24, bool runInThread)
            => AviSynthClip.Create(isFile: false, scriptOrPath: script, requireRGB24: requireRGB24);
    }

    // ── AviSynthClip — backed by AvsLibrary / AvsSession ─────────────────────

    public sealed class AviSynthClip : IDisposable
    {
        private readonly AvsLibrary            _lib;
        private readonly AvsSession            _session;
        private readonly IntPtr                _clip;
        private readonly AvsProbe.StreamInfo   _info;
        private readonly AvsProbe.StreamInfo   _originalInfo;
        private readonly AviSynthColorspace    _originalColorspace;
        private          bool                  _disposed;
        private readonly string                _convertError;

        private static readonly object _locker = new object();

        // value of AvsProbe.StreamInfo.ColorSpace for a packed BGR24 clip, the only format ReadFrame can copy
        private const string RGB24ColorSpace = "BGR24";

        [DllImport("kernel32.dll", EntryPoint = "RtlMoveMemory")]
        private static extern void CopyMemory(IntPtr dest, IntPtr src, int count);

        // ── Factory ───────────────────────────────────────────────────────────

        internal static AviSynthClip Create(bool isFile, string scriptOrPath, bool requireRGB24)
        {
            AvsLibrary  lib     = null;
            AvsSession  session = null;
            IntPtr      clip    = IntPtr.Zero;

            lock (_locker)
            {
                try
                {
                    lib     = AvsLibrary.Load();
                    session = new AvsSession(lib);

                    string evalScript = isFile
                        ? string.Format("Import(\"{0}\")", scriptOrPath.Replace("\\", "\\\\"))
                        : scriptOrPath;

                    clip = session.EvalScript(evalScript);

                    // Get original colorspace (raw pixel_type from VideoInfo)
                    AviSynthColorspace origColorspace = GetColorspaceFromProbe(lib, clip);

                    AvsProbe.StreamInfo info = AvsProbe.Probe(lib, session, clip);

                    // Keep the original probe result before any conversion
                    AvsProbe.StreamInfo originalInfo = info;

                    string convertError = null;
                    if (requireRGB24 && info.Width > 0 && info.ColorSpace != RGB24ColorSpace)
                    {
                        // Apply ConvertToRGB24 for frame reading; retain the pre-conversion colorspace
                        IntPtr rgb24Clip = TryConvertToRGB24(lib, session, clip, evalScript, info.BitDepth, out convertError);
                        if (rgb24Clip != IntPtr.Zero)
                        {
                            lib.ReleaseClip(clip);
                            clip = rgb24Clip;
                            info = AvsProbe.Probe(lib, session, clip);
                        }
                    }

                    return new AviSynthClip(lib, session, clip, info, originalInfo, origColorspace, convertError);
                }
                catch (AviSynthException)
                {
                    if (clip    != IntPtr.Zero) lib?.ReleaseClip(clip);
                    session?.Dispose();
                    lib?.Dispose();
                    throw;
                }
                catch (Exception ex)
                {
                    if (clip    != IntPtr.Zero) lib?.ReleaseClip(clip);
                    session?.Dispose();
                    lib?.Dispose();
                    throw new AviSynthException(ex.Message, ex);
                }
            }
        }

        private static AviSynthColorspace GetColorspaceFromProbe(AvsLibrary lib, IntPtr clip)
        {
            IntPtr viPtr = lib.GetVideoInfo(clip);
            if (viPtr == IntPtr.Zero) return AviSynthColorspace.Unknown;
            int pixelType = Marshal.ReadInt32(viPtr, 20);
            return (AviSynthColorspace)pixelType;
        }
        private static IntPtr TryConvertToRGB24(AvsLibrary lib, AvsSession session, IntPtr clip,
                                                string evalScript, int bitDepth, out string error)
        {
            error = null;

            // Re-evaluate the original script with the conversion appended.
            // We cannot rely on AviSynth's implicit 'last' variable because it
            // does not persist across separate avs_invoke("Eval", ...) calls.
            // Any expensive source indexing (e.g. LWI) is cached from the first
            // evaluation and will not be repeated.
            //
            // High bit depth clips (10/12/14/16 bit, float) are not always accepted by ConvertToRGB24():
            // fall back to an explicit 8 bit conversion first (#71).
            string[] conversions = bitDepth > 8
                ? new[] { "ConvertToRGB24()", "ConvertBits(8)\nConvertToRGB24()" }
                : new[] { "ConvertToRGB24()" };

            foreach (string conversion in conversions)
            {
                try
                {
                    IntPtr rgb24Clip = session.EvalScript(evalScript + "\n" + conversion);
                    if (rgb24Clip != IntPtr.Zero)
                    {
                        error = null;
                        return rgb24Clip;
                    }
                }
                catch (Exception ex)
                {
                    error = error == null
                        ? ex.Message
                        : error + " | " + conversion.Replace("\n", "; ") + ": " + ex.Message;
                }
            }

            return IntPtr.Zero;
        }

        // ── Constructor ───────────────────────────────────────────────────────

        private AviSynthClip(AvsLibrary lib, AvsSession session, IntPtr clip,
                             AvsProbe.StreamInfo info, AvsProbe.StreamInfo originalInfo,
                             AviSynthColorspace originalColorspace, string convertError)
        {
            _lib                = lib;
            _session            = session;
            _clip               = clip;
            _info               = info;
            _originalInfo       = originalInfo;
            _originalColorspace = originalColorspace;
            _convertError       = convertError;
        }

        // ── Video properties ──────────────────────────────────────────────────

        public bool HasVideo  => _info.Width > 0 && _info.Height > 0;
        public int  VideoWidth  => _info.Width;
        public int  VideoHeight => _info.Height;
        public int  raten       => (int)_info.FpsNum;
        public int  rated       => (int)_info.FpsDen;
        public int  aspectn     => 0;
        public int  aspectd     => 1;
        public int  interlaced_frame  => _info.IsFieldBased ? 1 : 0;
        public int  top_field_first   => _info.IsTff ? 1 : 0;
        public int  num_frames        => _info.NumFrames;

        /// <summary>Probe result of the clip as the script produces it (before the RGB24 conversion used for the preview)</summary>
        public AvsProbe.StreamInfo OriginalStreamInfo => _originalInfo;

        /// <summary>true if the preview clip was converted to RGB24 and therefore differs from the script output</summary>
        public bool ConvertedToRGB24 => !ReferenceEquals(_info, _originalInfo);

        public AviSynthColorspace PixelType         => _originalColorspace;
        public AviSynthColorspace OriginalColorspace => _originalColorspace;

        // ── Audio properties ──────────────────────────────────────────────────

        public bool             HasAudio        => _info.NumAudioSamples > 0;
        public int              AudioSampleRate => _info.AudioSampleRate;
        public long             SamplesCount    => _info.NumAudioSamples;
        public AudioSampleType  SampleType      => (AudioSampleType)_info.SampleType;
        public short            ChannelsCount   => (short)_info.AudioChannels;
        public int              ChannelMask     => _info.ChannelMask;
        public AudioSampleType  OriginalSampleType => (AudioSampleType)_info.SampleType;

        public short BytesPerSample
        {
            get
            {
                switch (_info.SampleType)
                {
                    case AvsApi.SAMPLE_INT8:  return 1;
                    case AvsApi.SAMPLE_INT16: return 2;
                    case AvsApi.SAMPLE_INT24: return 3;
                    case AvsApi.SAMPLE_INT32: return 4;
                    case AvsApi.SAMPLE_FLOAT: return 4;
                    default:                  return 2;
                }
            }
        }

        public short BitsPerSample    => (short)(BytesPerSample * 8);
        public int   AvgBytesPerSec   => AudioSampleRate * ChannelsCount * BytesPerSample;
        public long  AudioSizeInBytes => SamplesCount > 0 ? SamplesCount * ChannelsCount * BytesPerSample : 0;

        // ── Methods ───────────────────────────────────────────────────────────

        public int GetIntVariable(string name, int defaultValue)
        {
            // Evaluate the variable name directly via avs_invoke so we can inspect the
            // result type without routing through CheckError().  If the variable is not
            // defined, avs_invoke returns an error-type value and sets an error in the
            // environment; we consume the error via avs_get_error so that the stale
            // error does not interfere with subsequent API calls.
            const int AVS_VALUE_SIZE = 16;
            IntPtr argMem = IntPtr.Zero;
            IntPtr strPtr = IntPtr.Zero;
            try
            {
                argMem = Marshal.AllocHGlobal(AVS_VALUE_SIZE);
                for (int i = 0; i < AVS_VALUE_SIZE; i++) Marshal.WriteByte(argMem, i, 0);
                strPtr = Marshal.StringToHGlobalAnsi(name);
                Marshal.WriteInt16(argMem, 0, (short)'s');
                Marshal.WriteIntPtr(argMem, 8, strPtr);
                var argVal = Marshal.PtrToStructure<AvsApi.AvsValue>(argMem);

                var result = _lib.Invoke(_session.Env, "Eval", argVal, IntPtr.Zero);
                // Consume any environment error without throwing, so callers are not
                // affected when the variable is simply absent.
                _lib.GetError(_session.Env);
                try
                {
                    if (result.type == (short)'i') return result.i;
                    return defaultValue;
                }
                finally
                {
                    _lib.ReleaseValue(result);
                }
            }
            catch { return defaultValue; }
            finally
            {
                if (strPtr != IntPtr.Zero) Marshal.FreeHGlobal(strPtr);
                if (argMem != IntPtr.Zero) Marshal.FreeHGlobal(argMem);
            }
        }

        public void ReadAudio(IntPtr buf, long start, int count)
        {
            if (_disposed) return;
            _lib.GetAudio(_clip, buf, start, count);
        }

        public void ReadFrame(IntPtr dst, int dstStride, int frameNum)
        {
            if (_disposed) return;

            // The copy below assumes packed RGB24 rows. Never reinterpret another pixel format (planar YUV,
            // high bit depth, ...) that way: it produces a tiled, flipped or garbled picture (#71).
            if (dst != IntPtr.Zero && _info.ColorSpace != RGB24ColorSpace)
                throw new AviSynthException(string.Format(
                    "Cannot display frame {0}: the clip is {1} ({2} bit) and could not be converted to RGB24.{3}",
                    frameNum, _info.ColorSpace, _info.BitDepth,
                    _convertError != null ? " Conversion error: " + _convertError : string.Empty));

            IntPtr frame = _lib.GetFrame(_clip, frameNum);
            if (frame == IntPtr.Zero)
                throw new AviSynthException(string.Format("avs_get_frame({0}) returned NULL.", frameNum));
            try
            {
                if (dst != IntPtr.Zero)
                {
                    IntPtr src      = _lib.GetReadPtrP(frame, AvsApi.DEFAULT_PLANE);
                    int    srcPitch = _lib.GetPitchP(frame, AvsApi.DEFAULT_PLANE);
                    int    rowBytes = VideoWidth * 3; // RGB24: 3 bytes per pixel
                    for (int y = 0; y < VideoHeight; y++)
                        CopyMemory(IntPtr.Add(dst, y * dstStride),
                                   IntPtr.Add(src, y * srcPitch),
                                   rowBytes);
                }
            }
            finally
            {
                _lib.ReleaseVideoFrame(frame);
            }
        }

        /// <summary>
        /// Returns a multi-line debug string with all stream information for OSD overlay.
        /// Shows original (pre-conversion) probe data alongside the current (post-conversion) data.
        /// </summary>
        public string GetDebugOverlayText()
        {
            var sb = new System.Text.StringBuilder();
            sb.AppendFormat("=== ORIGINAL (pre-conversion) ===\n");
            sb.AppendFormat("Colorspace enum : {0} (0x{1:X8})\n", _originalColorspace, (int)_originalColorspace);
            sb.AppendFormat("ColorSpace probe: {0}\n", _originalInfo.ColorSpace);
            sb.AppendFormat("Resolution      : {0}x{1}\n", _originalInfo.Width, _originalInfo.Height);
            sb.AppendFormat("BitDepth        : {0}\n", _originalInfo.BitDepth);
            sb.AppendFormat("FullRange       : {0}\n", _originalInfo.FullRange);
            sb.AppendFormat("FPS             : {0}/{1}\n", _originalInfo.FpsNum, _originalInfo.FpsDen);
            sb.AppendFormat("Frames          : {0}\n", _originalInfo.NumFrames);
            sb.AppendFormat("ImageType       : 0x{0:X8}\n", _originalInfo.ImageType);
            sb.AppendFormat("FieldBased      : {0}  BFF: {1}  TFF: {2}\n",
                _originalInfo.IsFieldBased, _originalInfo.IsBff, _originalInfo.IsTff);
            sb.AppendFormat("Audio           : {0}Hz  {1}ch  {2} samples\n",
                _originalInfo.AudioSampleRate, _originalInfo.AudioChannels, _originalInfo.NumAudioSamples);
            sb.AppendFormat("SampleType      : 0x{0:X8}\n", _originalInfo.SampleType);
            sb.AppendFormat("ChannelMask     : 0x{0:X8}\n", _originalInfo.ChannelMask);
            sb.AppendFormat("\n=== CURRENT (post-conversion) ===\n");
            sb.AppendFormat("ColorSpace probe: {0}\n", _info.ColorSpace);
            sb.AppendFormat("Resolution      : {0}x{1}\n", _info.Width, _info.Height);
            sb.AppendFormat("BitDepth        : {0}\n", _info.BitDepth);
            sb.AppendFormat("FullRange       : {0}\n", _info.FullRange);
            sb.AppendFormat("RGB24 active    : {0}\n", !ReferenceEquals(_info, _originalInfo));
            if (_convertError != null)
            {
                sb.AppendFormat("\n=== CONVERT ERROR ===\n{0}\n", _convertError);
            }
            return sb.ToString();
        }

        // ── Static installation check ─────────────────────────────────────────

        public static int CheckAvisynthInstallation(
            out string strVersion, out bool bIsAVS26, out bool bIsAVSPlus, out bool bIsMT,
            out string strAviSynthDLL, ref LogItem oLog)
        {
            strVersion    = string.Empty;
            bIsAVS26      = false;
            bIsAVSPlus    = false;
            bIsMT         = false;
            strAviSynthDLL = string.Empty;

            try
            {
                using (AvsLibrary lib = AvsLibrary.Load())
                {
                    strAviSynthDLL = lib.LoadedPath;
                    using (AvsSession session = new AvsSession(lib))
                    {
                        // Successfully created environment with API v9 → AviSynth+
                        bIsAVS26   = true;
                        bIsAVSPlus = true;
                        bIsMT      = true;
                        try { strVersion = session.EvalString("VersionString()"); }
                        catch { /* version string is optional */ }
                        return 0;
                    }
                }
            }
            catch (Exception ex)
            {
                oLog?.LogValue("Error", ex.Message, ImageType.Error, false);
                return 1;
            }
        }

        // ── IDisposable ───────────────────────────────────────────────────────

        ~AviSynthClip() { Dispose(false); }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (_disposed) return;
            _disposed = true;
            if (_clip != IntPtr.Zero) _lib.ReleaseClip(_clip);
            _session?.Dispose();
            _lib?.Dispose();
        }
    }

    // ── AvsFileFactory / AvsFile ──────────────────────────────────────────────

    public class AvsFileFactory : IMediaFileFactory
    {
        #region IMediaFileFactory Members
        public IMediaFile Open(string file)
        {
            return AvsFile.OpenScriptFile(file, true);
        }
        #endregion

        #region IMediaFileFactory Members
        public int HandleLevel(string file)
        {
            if (!string.IsNullOrEmpty(file) && file.ToLowerInvariant().EndsWith(".avs"))
                return 30;
            return -1;
        }
        #endregion

        #region IIDable Members
        public string ID { get { return "AviSynth"; } }
        #endregion
    }

    public sealed class AvsFile : IMediaFile
    {
        private AviSynthClip clip = null;
        private IAudioReader audioReader;
        private IVideoReader videoReader;
        private VideoInformation info;

        #region construction

        public AviSynthClip Clip { get { return this.clip; } }

        public static AvsFile OpenScriptFile(string fileName)
            => new AvsFile(fileName, false, false);

        public static AvsFile ParseScript(string scriptBody)
            => new AvsFile(scriptBody, true, false);

        public static AvsFile OpenScriptFile(string fileName, bool bRequireRGB24)
            => new AvsFile(fileName, false, bRequireRGB24);

        public static AvsFile ParseScript(string scriptBody, bool bRequireRGB24)
            => new AvsFile(scriptBody, true, bRequireRGB24);

        private AvsFile(string script, bool parse, bool bRequireRGB24)
        {
            try
            {
                this.clip = parse
                    ? AviSynthScriptEnvironment.ParseScript(script, bRequireRGB24)
                    : AviSynthScriptEnvironment.OpenScriptFile(script, bRequireRGB24);

                checked
                {
                    if (clip.HasVideo)
                    {
                        ulong width  = (ulong)clip.VideoWidth;
                        ulong height = (ulong)clip.VideoHeight;
                        info = new VideoInformation(
                            clip.HasVideo, width, height,
                            new Dar(clip.GetIntVariable("MeGUI_darx", -1),
                                    clip.GetIntVariable("MeGUI_dary", -1),
                                    width, height),
                            (ulong)clip.num_frames,
                            ((double)clip.raten) / ((double)clip.rated),
                            clip.raten, clip.rated);
                    }
                    else if (clip.HasAudio)
                        info = new VideoInformation(false, 0, 0, Dar.A1x1, (ulong)clip.SamplesCount, (double)clip.AudioSampleRate, 0, 0);
                    else
                        info = new VideoInformation(false, 0, 0, Dar.A1x1, 0, 0, 0, 0);
                }
            }
            catch (Exception)
            {
                Cleanup();
                throw;
            }
        }

        private void Cleanup()
        {
            if (this.clip != null)
            {
                (this.clip as IDisposable).Dispose();
                this.clip = null;
            }
            GC.SuppressFinalize(this);
        }
        #endregion

        #region properties
        public VideoInformation VideoInfo   { get { return info; } }
        public bool             CanReadVideo  { get { return true; } }
        public bool             CanReadAudio  { get { return true; } }
        #endregion

        private static readonly object _locker = new object();

        public IAudioReader GetAudioReader(int track)
        {
            if (track != 0 || !clip.HasAudio)
                throw new Exception(string.Format("Can't read audio track {0}, because it can't be found", track));
            if (audioReader == null)
                lock (_locker)
                {
                    if (audioReader == null)
                        audioReader = new AvsAudioReader(clip);
                }
            return audioReader;
        }

        public IVideoReader GetVideoReader()
        {
            if (!this.VideoInfo.HasVideo)
                throw new Exception("Can't get Video Reader, since there is no video stream!");
            if (videoReader == null)
                lock (_locker)
                {
                    if (videoReader == null)
                        videoReader = new AvsVideoReader(clip, (int)VideoInfo.Width, (int)VideoInfo.Height);
                }
            return videoReader;
        }

        // ── Inner readers ─────────────────────────────────────────────────────

        sealed class AvsVideoReader : IVideoReader
        {
            private AviSynthClip clip;
            private int width, height;

            public AvsVideoReader(AviSynthClip clip, int width, int height)
            {
                this.clip   = clip;
                this.width  = width;
                this.height = height;
            }

            public int FrameCount { get { return this.clip.num_frames; } }

            public Bitmap ReadFrameBitmap(int position)
            {
                Bitmap bmp = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                try
                {
                    Rectangle rect    = new Rectangle(0, 0, bmp.Width, bmp.Height);
                    System.Drawing.Imaging.BitmapData bmpData =
                        bmp.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite, bmp.PixelFormat);
                    try
                    {
                        clip.ReadFrame(bmpData.Scan0, bmpData.Stride, position);
                    }
                    finally
                    {
                        bmp.UnlockBits(bmpData);
                    }
                    bmp.RotateFlip(RotateFlipType.Rotate180FlipX);

                    // ── Debug OSD overlay (opt-in via Settings) ───────────
/*                    if (MainForm.Instance.Settings.DebugVideoOSD)
                    { */
                        try
                        {
                          /*  string osdText = clip.GetDebugOverlayText();
                            using (Graphics g = Graphics.FromImage(bmp))
                            {
                                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
                                using (Font font = new Font("Consolas", 10f, FontStyle.Bold))
                                {
                                    SizeF textSize = g.MeasureString(osdText, font);
                                    using (SolidBrush bgBrush = new SolidBrush(Color.FromArgb(180, 0, 0, 0)))
                                    {
                                        g.FillRectangle(bgBrush, 0, 0, textSize.Width + 12, textSize.Height + 8);
                                    }
                                    g.DrawString(osdText, font, Brushes.Black, 7, 5);
                                    g.DrawString(osdText, font, Brushes.Lime, 6, 4);
                                }
                            }*/
                        } 
                        catch { /* OSD failure must not break preview */ }
                    //}
                    
                    return bmp;
                }
                catch (System.Runtime.InteropServices.SEHException ex)
                {
                    bmp.Dispose();
                    LogFrameError(ex.Message);
                    throw;
                }
                catch (Exception ex)
                {
                    bmp.Dispose();
                    LogFrameError(ex.Message);
                    throw;
                }
            }

            private static void LogFrameError(string message)
            {
                LogItem log = MainForm.Instance.AVSScriptCreatorLog;
                if (log == null)
                {
                    log = MainForm.Instance.Log.Info("AVS Script Creator");
                    MainForm.Instance.AVSScriptCreatorLog = log;
                }
                log.LogValue("Could not read frame", message, ImageType.Warning, true);
            }
        }

        sealed class AvsAudioReader : IAudioReader
        {
            private AviSynthClip clip;

            public AvsAudioReader(AviSynthClip clip) { this.clip = clip; }

            public long SampleCount           { get { return clip.SamplesCount; } }
            public bool SupportsFastReading   { get { return true; } }

            public long ReadAudioSamples(long nStart, int nAmount, IntPtr buf)
            {
                clip.ReadAudio(buf, nStart, nAmount);
                return nAmount;
            }

            public byte[] ReadAudioSamples(long nStart, int nAmount) { return null; }
        }

        #region IDisposable Members
        public void Dispose() { Cleanup(); }
        #endregion
    }
}
