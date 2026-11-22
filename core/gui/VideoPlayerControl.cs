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
using System.Threading;
using System.Windows.Forms;
using Timer = System.Threading.Timer;

namespace MeGUI.core.gui
{
    /// <summary>
    /// How the player shows the source (input file) next to the filtered script output.
    /// </summary>
    public enum CompareMode
    {
        Off,
        /// <summary>One picture, the source on the left of a draggable divider and the filtered output on the right</summary>
        Split,
        /// <summary>Source and filtered output next to each other</summary>
        SideBySide
    }

    public partial class VideoPlayerControl : UserControl
    {
        public VideoPlayerControl()
        {
            InitializeComponent();
            playTimer = new Timer(PlayNextFrame);

            videoPreview.MouseDown += videoPreview_MouseDown;
            videoPreview.MouseMove += videoPreview_MouseMove;
            videoPreview.MouseUp += videoPreview_MouseUp;
        }

        //ensures that UnloadVideo only returns if the reader is not used by other threads any more
        private readonly ReaderWriterLock readerWriterLock = new ReaderWriterLock();

        #region current frame position handling
        public event EventHandler PositionChanged;
        public void OnPositionChanged()
        {
            PositionChanged?.Invoke(this, new EventArgs());
        }

        private object positionLock = new object();
        
        private bool OffsetPosition(int offset, bool update)
        {
            bool success;
            //ensures that the correct offset is always added even
            //if multiple threads are calling this methods (e.g. playback)
            lock (positionLock)
            {
                //Position property ensures that position does not get out of bounds
                success = SetPositionInternal(position + offset);
            }

            InvokeOnPositionChanged();

            if(update) 
                UpdateVideo();

            return success;
        }

        public bool OffsetPosition(int offset)
        {
            return OffsetPosition(offset, true);
        }

        private bool SetPositionInternal(int value)
        {
            int max = FrameCount - 1;

            //Prevent setting the position out of range
            if (value < 0)
                value = 0;
            else if (value > max)
                value = max;

            //position unchanged
            if (position == value) 
                return false;

            position = value;

            return true;
        }

        public void InvokeOnPositionChanged()
        {            
            //HACK: Invoke does not work before handle is created
            if (initalized)
                Invoke(new SimpleDelegate(OnPositionChanged));
            else
                OnPositionChanged();
        }
        #endregion

        #region Rendering
        public void UpdateVideo()
        {
            //the player may already be disposed (window closing): nothing to render any more
            if (IsDisposed)
                return;
            try
            {
                renderEvent.Set();
            }
            catch (ObjectDisposedException)
            {
            }
        }

        /// <summary>
        /// Resizes the video frame
        /// http://www.peterprovost.org/archive/2003/05/29/516.aspx
        /// </summary>
        /// <param name="b"></param>
        /// <param name="nWidth"></param>
        /// <param name="nHeight"></param>
        /// <returns>A resized bitmap (needs disposal)</returns>
        private Bitmap ResizeBitmap(Bitmap b, int nWidth, int nHeight)
        {
            float factorX = nWidth / (float)b.Width;
            float factorY = nHeight / (float)b.Height;

            //calculate source and destination rectangles with applied cropping values
            RectangleF src = new RectangleF(cropMargin.Left, cropMargin.Top, b.Width - cropMargin.Horizontal, b.Height - cropMargin.Vertical);
            RectangleF dst = new RectangleF(cropMargin.Left * factorX, cropMargin.Top * factorY, (b.Width - cropMargin.Horizontal) * factorX, (b.Height - cropMargin.Vertical) * factorY);

            Bitmap result = new Bitmap(nWidth, nHeight);
            using (Graphics g = Graphics.FromImage(result))
            {
                //apply cropping
                using (Region reg = new Region())
                {
                    reg.MakeInfinite();
                    reg.Exclude(dst);
                    g.FillRegion(Brushes.White, reg);
                }

                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Bilinear;
                //g.DrawImage(b, 0, 0, nWidth, nHeight);
                g.DrawImage(b, dst, src, GraphicsUnit.Pixel);

                if (DisplayActualFramerate)
                {
                    g.DrawString(ActualFramerate.ToString("0.00 fps"), Font, Brushes.Green, 0, 0);
                }
            }
            return result;
        }
        #endregion

        #region Video Playback
        //asynchronous Timer to update video in fixed interval
        readonly Timer playTimer;

        /// <summary>
        /// Is Invoked by playTimer to render the next frame for the video
        /// It will trigger the playThread to render the next frame.
        /// This solution is more complex than a simple Thread.Sleep, but
        /// has the advantage that the playback will be smoother if the frames
        /// take long to render.
        /// </summary>
        private void PlayNextFrame(object state)
        {
            try
            {
                //playback speed is correct, but frames may be dropped if computer is too slow
                if (EnsureCorrectPlaybackSpeed)
                {
                    if (!OffsetPosition(1, false)) 
                        Stop();
                    else 
                        InvokeOnPositionChanged();

                    UpdateVideo();
                }
                //no frames will be dropped, but the playback speed might be slower than realtime
                else
                {
                    nextFrameEvent.Set();
                }

                //Console.WriteLine("Frame {0} requested", Position);
            }
            catch (Exception e)
            {
                MeGUI.core.util.LogItem _oLog = MainForm.Instance.Log.Info("Error");
                _oLog.LogValue("playNextFrame", e, MeGUI.core.util.ImageType.Error);
            }
        }

        //is set by the timer to indicate that the position must be advanced in addition to the rendering.
        //if the position would be incremented directly in playNextFrame, slow computers would drop frames.
        //with this method the computer will always display every frame
        private readonly AutoResetEvent nextFrameEvent = new AutoResetEvent(false);

        //is set to trigger the rendering of the current frame
        private readonly AutoResetEvent renderEvent = new AutoResetEvent(false);

        private void RenderThreadLoop()
        {
            int framesPlayed = 0;
            DateTime start = DateTime.Now;

            do
            {
                DateTime end = DateTime.Now;
                TimeSpan renderTime = end - start;

                //do not update current framerate more often than 200ms
                if (renderTime.Milliseconds > 200 && framesPlayed > 0)
                {
                    //average results
                    actualFramerate = 0.9d * actualFramerate + 0.1d * (TimeSpan.TicksPerSecond / (double)renderTime.Ticks) * framesPlayed;
                    start = end;
                    framesPlayed = 0;
                }

                if (WaitHandle.WaitAny(new WaitHandle[] { nextFrameEvent, renderEvent }) == 0)
                {
                    if (!OffsetPosition(1, false))
                        Stop();
                    else
                        InvokeOnPositionChanged();
                }

                int pos = position;

                // The control may have no size yet (not laid out) or be minimized:
                // "new Bitmap(0, 0)" would throw, so simply wait for the next render request.
                int targetWidth = videoPreview.Width;
                int targetHeight = videoPreview.Height;
                if (targetWidth <= 0 || targetHeight <= 0)
                    continue;

                Bitmap finalBitmap = null;
                try
                {
                    CompareMode cm = compareMode;
                    if (cm != CompareMode.Off && HasCompareReader)
                    {
                        finalBitmap = RenderCompare(pos, targetWidth, targetHeight, cm);
                        if (finalBitmap == null)
                            continue;
                    }
                    else
                    {
                        using (Bitmap b = GetFrame(pos))
                        {
                            if (b == null)
                                continue;

                            //if (cropMargin != Padding.Empty) // only crop when necessary
                            //    cropImage(b);

                            finalBitmap = ResizeBitmap(b, targetWidth, targetHeight);
                        }
                    }

                    // Swap the image on the GUI thread. The previous bitmap is disposed there too,
                    // after the PictureBox no longer references it.
                    Bitmap newImage = finalBitmap;
                    Invoke((SimpleDelegate)(delegate
                    {
                        Image oldImage = videoPreview.Image;
                        videoPreview.Image = newImage;
                        oldImage?.Dispose();
                    }));
                    finalBitmap = null; // ownership transferred to the PictureBox
                }
                catch (Exception ex) when (ex is ObjectDisposedException || (ex is InvalidOperationException && (IsDisposed || Disposing || !IsHandleCreated)))
                {
                    // The player is being closed while a frame was in flight: nothing to report.
                    finalBitmap?.Dispose();
                    break;
                }
                catch (Exception ex)
                {
                    finalBitmap?.Dispose();
                    LogFrameReadError(ex);
                    ReportFrameReadErrorOnce();
                    // Do NOT break: ending the loop would leave the player permanently without a render thread.
                    // The next render request (seek, resize, ...) will simply try again.
                    continue;
                }

                //needed for fps display
                framesPlayed++;

            } while (!IsDisposed);
        }

        private int frameErrorReported;

        private static void LogFrameReadError(Exception ex)
        {
            MeGUI.core.util.LogItem _oLog = MainForm.Instance.AVSScriptCreatorLog;
            if (_oLog == null)
            {
                _oLog = MainForm.Instance.Log.Info("AVS Script Creator");
                MainForm.Instance.AVSScriptCreatorLog = _oLog;
            }
            _oLog.LogValue("Could not read frame", ex.Message, MeGUI.core.util.ImageType.Warning, true);
        }

        /// <summary>
        /// Shows the "Could not read AVS frame" message once per loaded video, on the GUI thread
        /// (a modal MessageBox on the render thread has no owner window and blocks that thread).
        /// </summary>
        private void ReportFrameReadErrorOnce()
        {
            if (Interlocked.Exchange(ref frameErrorReported, 1) != 0)
                return;

            try
            {
                BeginInvoke((SimpleDelegate)(delegate
                {
                    MessageBox.Show(this.FindForm(), "Could not read AVS frame.", "Video Player Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }));
            }
            catch (InvalidOperationException)
            {
                // handle already gone: window is closing
            }
        }

        private Bitmap GetFrame(int pos)
        {
            readerWriterLock.AcquireReaderLock(Timeout.Infinite);
            try
            {
                IVideoReader reader = VideoReader;
                if (reader == null) 
                    return null;
                return reader.ReadFrameBitmap(pos);
            }
            finally
            {
                readerWriterLock.ReleaseReaderLock();
            }
        }

        /// <summary>
        /// The picture box covers the whole control: it must show the context menu too
        /// </summary>
        public override ContextMenuStrip ContextMenuStrip
        {
            get { return base.ContextMenuStrip; }
            set
            {
                base.ContextMenuStrip = value;
                videoPreview.ContextMenuStrip = value;
            }
        }

        #region Compare (source vs filtered)
        private const int SideBySideGap = 4;

        private CompareMode compareMode = CompareMode.Off;
        private float splitPosition = 0.5f;
        private IVideoReader compareReader;

        //resized frames of the last compare rendering: dragging the divider only recomposes them.
        //They are only used by the render thread (and disposed after that thread has ended).
        private Bitmap cacheFiltered, cacheSource;
        private int cachePos = -1, cachePw, cachePh;
        private Padding cacheCrop;
        private volatile bool cacheDirty = true;

        /// <summary>
        /// Source clip (the input file without any filter) used by the compare modes.
        /// Pass null to remove it. The caller keeps the ownership and must dispose the clip after removing it here.
        /// </summary>
        public void SetCompareReader(IVideoReader reader)
        {
            //the writer lock ensures that the render thread does not use the old reader any more when this returns
            readerWriterLock.AcquireWriterLock(Timeout.Infinite);
            try
            {
                compareReader = reader;
            }
            finally
            {
                readerWriterLock.ReleaseWriterLock();
            }
            cacheDirty = true;
            if (reader == null)
                compareMode = CompareMode.Off;
            UpdateVideo();
        }

        public bool HasCompareReader
        {
            get { return compareReader != null; }
        }

        public CompareMode CompareMode
        {
            get { return compareMode; }
            set
            {
                if (value != CompareMode.Off && !HasCompareReader)
                    value = CompareMode.Off;
                compareMode = value;
                videoPreview.Cursor = value == CompareMode.Split ? Cursors.SizeWE : Cursors.Default;
                cacheDirty = true;
                UpdateVideo();
            }
        }

        /// <summary>Position of the divider in Split mode (0 = all filtered, 1 = all source)</summary>
        public float SplitPosition
        {
            get { return splitPosition; }
            set
            {
                splitPosition = Math.Max(0f, Math.Min(1f, value));
                UpdateVideo();
            }
        }

        private Bitmap GetCompareFrame(int pos)
        {
            readerWriterLock.AcquireReaderLock(Timeout.Infinite);
            try
            {
                IVideoReader reader = compareReader;
                if (reader == null)
                    return null;
                int max = reader.FrameCount - 1;
                if (max < 0)
                    return null;
                //the filters may have changed the frame count: stay inside the source
                return reader.ReadFrameBitmap(Math.Min(pos, max));
            }
            finally
            {
                readerWriterLock.ReleaseReaderLock();
            }
        }

        private Bitmap RenderCompare(int pos, int w, int h, CompareMode mode)
        {
            int pw = w, ph = h;
            if (mode == CompareMode.SideBySide)
            {
                pw = Math.Max(1, (w - SideBySideGap) / 2);
                ph = Math.Max(1, (int)Math.Round(pw * (double)h / w));
            }

            Padding crop = cropMargin;
            bool valid = !cacheDirty && cacheFiltered != null && cacheSource != null
                && cachePos == pos && cachePw == pw && cachePh == ph && cacheCrop == crop;
            if (!valid)
            {
                cacheDirty = false; //reset first: an invalidation during the reading below is not lost
                Bitmap filtered, source;
                using (Bitmap rawFiltered = GetFrame(pos))
                {
                    if (rawFiltered == null)
                        return null;
                    filtered = ResizeBitmap(rawFiltered, pw, ph);
                }
                using (Bitmap rawSource = GetCompareFrame(pos))
                {
                    if (rawSource == null)
                    {
                        //no source frame available: show the filtered picture only
                        Bitmap fallback = new Bitmap(w, h);
                        using (Graphics fg = Graphics.FromImage(fallback))
                            fg.DrawImage(filtered, 0, 0, w, h);
                        filtered.Dispose();
                        return fallback;
                    }
                    source = ResizeBitmap(rawSource, pw, ph);
                }
                cacheFiltered?.Dispose();
                cacheSource?.Dispose();
                cacheFiltered = filtered;
                cacheSource = source;
                cachePos = pos;
                cachePw = pw;
                cachePh = ph;
                cacheCrop = crop;
            }
            return ComposeCompare(cacheSource, cacheFiltered, w, h, mode, splitPosition);
        }

        private static Bitmap ComposeCompare(Bitmap source, Bitmap filtered, int w, int h, CompareMode mode, float split)
        {
            Bitmap result = new Bitmap(w, h);
            using (Graphics g = Graphics.FromImage(result))
            {
                g.Clear(Color.Black);
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;

                if (mode == CompareMode.Split)
                {
                    g.DrawImageUnscaled(source, 0, 0);
                    int x = Math.Max(0, Math.Min(w, (int)Math.Round(w * split)));
                    g.SetClip(new Rectangle(x, 0, w - x, h));
                    g.DrawImageUnscaled(filtered, 0, 0);
                    g.ResetClip();

                    using (Pen line = new Pen(Color.White, 2f))
                        g.DrawLine(line, x, 0, x, h);
                    DrawDividerHandle(g, x, h / 2);
                    DrawChip(g, "Source", 10, 10, false);
                    DrawChip(g, "Filtered", w - 10, 10, true);
                }
                else
                {
                    int total = source.Width + SideBySideGap + filtered.Width;
                    int left = Math.Max(0, (w - total) / 2);
                    int top = Math.Max(0, (h - source.Height) / 2);
                    g.DrawImageUnscaled(source, left, top);
                    g.DrawImageUnscaled(filtered, left + source.Width + SideBySideGap, top);
                    DrawChip(g, "Source", left + 10, top + 10, false);
                    DrawChip(g, "Filtered", left + total - 10, top + 10, true);
                }
            }
            return result;
        }

        private static void DrawDividerHandle(Graphics g, int x, int y)
        {
            const int d = 34;
            g.FillEllipse(Brushes.White, x - d / 2, y - d / 2, d, d);
            using (Brush arrow = new SolidBrush(Color.FromArgb(0x20, 0x20, 0x20)))
            {
                g.FillPolygon(arrow, new[] { new Point(x - 4, y - 6), new Point(x - 4, y + 6), new Point(x - 11, y) });
                g.FillPolygon(arrow, new[] { new Point(x + 4, y - 6), new Point(x + 4, y + 6), new Point(x + 11, y) });
            }
        }

        private static void DrawChip(Graphics g, string text, int x, int y, bool alignRight)
        {
            using (Font font = new Font("Segoe UI", 9f, FontStyle.Bold))
            {
                SizeF size = g.MeasureString(text, font);
                float width = size.Width + 14;
                float height = size.Height + 4;
                float left = alignRight ? x - width : x;
                using (Brush back = new SolidBrush(Color.FromArgb(170, 0, 0, 0)))
                    g.FillRectangle(back, left, y, width, height);
                g.DrawString(text, font, Brushes.White, left + 7, y + 2);
            }
        }

        private bool draggingDivider;

        private void MoveDivider(MouseEventArgs e)
        {
            if (compareMode == CompareMode.Split && videoPreview.Width > 0)
                SplitPosition = e.X / (float)videoPreview.Width;
        }

        private void videoPreview_MouseDown(object sender, MouseEventArgs e)
        {
            if (compareMode == CompareMode.Split && e.Button == MouseButtons.Left)
            {
                draggingDivider = true;
                MoveDivider(e);
            }
        }

        private void videoPreview_MouseMove(object sender, MouseEventArgs e)
        {
            if (draggingDivider)
                MoveDivider(e);
        }

        private void videoPreview_MouseUp(object sender, MouseEventArgs e)
        {
            draggingDivider = false;
        }
        #endregion

        private bool isPlaying;

        /// <summary>
        /// Start the playing of the video
        /// </summary>
        public void Play()
        {
            if (videoReader == null)
                throw new InvalidOperationException("Video must be loaded before playback can be started");

            playTimer.Change(0, (int)(1000d / (Framerate * SpeedUp)));
            isPlaying = true;

            actualFramerate = Framerate * SpeedUp;
        }

        /// <summary>
        /// Stops the playing of the video
        /// </summary>
        public void Stop()
        {
            playTimer.Change(Timeout.Infinite, Timeout.Infinite);
            nextFrameEvent.Reset();
            renderEvent.Reset();
            isPlaying = false;

            actualFramerate = 0;
        }
        #endregion

        #region LoadVideo
        public void LoadVideo(IVideoReader reader)
        {
            LoadVideo(reader, 25, 0);
        }

        public void LoadVideo(IVideoReader reader, double fps)
        {
            LoadVideo(reader, fps, 0);
        }

        public void LoadVideo(IVideoReader reader, double fps, int startPosition)
        {
            UnloadVideo();

            //a new video gets a fresh chance to report a frame read error
            Interlocked.Exchange(ref frameErrorReported, 0);

            //just to be sure... shouldn't be necessary because after UnloadVideo
            //videoReader will be null
            readerWriterLock.AcquireWriterLock(Timeout.Infinite);
            try
            {
                videoReader = reader;
            }
            finally
            {
                readerWriterLock.ReleaseWriterLock();
            }

            cacheDirty = true;
            Framerate = fps;
            Position = startPosition;
        }
        public void UnloadVideo()
        {
            Stop();
            //ensures that no other thread uses the reader at the moment,
            //so that the video file can be safely disposed when the method returns
            readerWriterLock.AcquireWriterLock(Timeout.Infinite);
            try
            {
                videoReader = null;
            }
            finally
            {
                readerWriterLock.ReleaseWriterLock();
            }
            position = 0;
            cacheDirty = true;
        }
        #endregion


        #region Event Handler
        private void VideoPlayerControl_Resize(object sender, EventArgs e)
        {
            UpdateVideo();
        }
        private void VideoPlayerControl_Load(object sender, EventArgs e)
        {
            initalized = true;

            //Thread for video playing
            renderThread = new Thread(RenderThreadLoop)
            {
                Name = "Render Thread"
            };
            renderThread.Start();
        }
        #endregion

        #region Properties
        private int position;
        public int Position
        {
            get
            {
                return position;
            }
            set
            {
                if (SetPositionInternal(value))
                {
                    UpdateVideo();
                    InvokeOnPositionChanged();
                }
            }
        }

        private IVideoReader videoReader;
        public IVideoReader VideoReader
        {
            get
            {
                return videoReader;
            }
        }

        private double framerate = 25;
        public double Framerate
        {
            get
            {
                return framerate;
            }
            set
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException(nameof(value), "FPS cannot be zero or lower");
                framerate = value;

                //Restart video to adjust playback speed for new framerate value
                if (isPlaying)
                    Play();
            }
        }

        public int FrameCount
        {
            get
            {
                readerWriterLock.AcquireReaderLock(Timeout.Infinite);
                try
                {
                    IVideoReader reader = VideoReader;

                    if (reader == null) return 0;

                    return reader.FrameCount;
                }
                finally
                {
                    readerWriterLock.ReleaseReaderLock();
                }
            }
        }

        private Padding cropMargin;
        public Padding CropMargin
        {
            get
            {
                return cropMargin;
            }
            set
            {
                cropMargin = value;
                cacheDirty = true;
                UpdateVideo();
            }
        }

        private bool displayActualFramerate;
        //if set the actual framerate of the video playback will be displayed
        //in the top left corner of the video frame
        public bool DisplayActualFramerate
        {
            get { return displayActualFramerate; }
            set { displayActualFramerate = value; UpdateVideo(); }
        }

        private bool ensureCorrectPlaybackSpeed;
        //if set frames will be dropped to ensure a more correct playing speed
        public bool EnsureCorrectPlaybackSpeed
        {
            get { return ensureCorrectPlaybackSpeed; }
            set { ensureCorrectPlaybackSpeed = value; }
        }

        private double speedUp = 1d;
        //Set this to speed up or slow down the playback
        public double SpeedUp
        {
            get { return speedUp; }
            set
            {
                speedUp = value;

                //Restart video to adjust playback speed for new speed up value
                if (isPlaying) Play();
            }
        }

        #endregion

        private bool initalized;
        private Thread renderThread;

        private double actualFramerate;
        public double ActualFramerate
        {
            get
            {
                return actualFramerate;
            }
        }

        /// <summary> 
        /// Release Ressources.
        /// </summary>
        /// <param name="disposing">True, if managed ressources should be released; otherwise false.</param>
        protected override void Dispose(bool disposing)
        {
            Stop();

            playTimer.Dispose();

            if (renderThread != null)
            {
                renderThread.Abort();
                renderThread.Join();
            }

            videoPreview.Image?.Dispose(); // get rid of bitmap
            cacheFiltered?.Dispose();
            cacheSource?.Dispose();
            nextFrameEvent.Dispose();
            renderEvent.Dispose();

            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}
