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
    public partial class VideoPlayerControl : UserControl
    {
        public VideoPlayerControl()
        {
            InitializeComponent();
            playTimer = new Timer(PlayNextFrame);
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
            renderEvent.Set();
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
            Bitmap finalBitmap = null;

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

                try
                {
                    using (Bitmap b = GetFrame(pos))
                    {
                        if (b == null)
                            continue;

                        //if (cropMargin != Padding.Empty) // only crop when necessary            
                        //    cropImage(b);

                        finalBitmap = ResizeBitmap(b, videoPreview.Width, videoPreview.Height);
                    }

                    using (videoPreview.Image) // get rid of previous bitmap
                    {
                        //do the actual rendering on GUI thread
                        Invoke((SimpleDelegate)(delegate { videoPreview.Image = finalBitmap; }));
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Could not read AVS frame.", "Video Player Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    MeGUI.core.util.LogItem _oLog = MainForm.Instance.AVSScriptCreatorLog;
                    if (_oLog == null)
                    {
                        _oLog = MainForm.Instance.Log.Info("AVS Script Creator");
                        MainForm.Instance.AVSScriptCreatorLog = _oLog;
                    }
                    _oLog.LogValue("Could not read frame", ex.Message, MeGUI.core.util.ImageType.Warning, true);

                    break;
                }

                //needed for fps display
                framesPlayed++;

            } while (!IsDisposed);

            finalBitmap?.Dispose();
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
