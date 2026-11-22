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
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

using MeGUI.core.gui;
using MeGUI.core.util;

namespace MeGUI
{
	public delegate void IntroCreditsFrameSetCallback(int frameNumber, bool isCredits);
	public delegate void ZoneSetCallback(int start, int end);
	public delegate void ChapterSetCallback(int frameNumber);
	public delegate void SimpleDelegate();
	public enum PREVIEWTYPE {REGULAR, CREDITS, ZONES, CHAPTERS};
	/// <summary>
	/// The video player is used to preview an AviSynth script or DGIndex project. 
	/// It can also be used to set the credits and intro and is used to generate zones visually
	/// </summary>
	public partial class VideoPlayer : System.Windows.Forms.Form
	{
		#region variable declaration
        public event IntroCreditsFrameSetCallback IntroCreditsFrameSet; // event to update the status in the GUI
		public event ZoneSetCallback ZoneSet;
		public event ChapterSetCallback ChapterSet;

		private IMediaFile file;
        private IVideoReader reader;
        private bool hasAR = false;
        private int creditsStartFrame = -1, introEndFrame = -1;
		private int zoneStart = -1, zoneEnd = -1; // zone start and end frames
        private int formHeightDelta; // height delta of the form versus the size of the VideoPlayerControl (the reference for the gui size)
		private const int defaultSpacing = 8; // default spacing from GUI elements
        private int videoWindowWidth, videoWindowHeight;
        private PREVIEWTYPE viewerType;
        private static bool sizeLock; // recursion lock for resize event handler
        private int zoomWidth;
        private int zoomMaxWidth; // max width so that it can be seen completly on the screen
        private int zoomFactor; // relation between zoomWidth and zoomMaxWidth (0-100%)
        private int zoomFactorStepSize = 15; // during zoom in/out this step size wil be used
        private int buttonPanelMinWidth;
        private bool bOriginalSize;
        private string totalTime;
        private string currentTime;
        private string strFileName; // the video file name
        private bool bInlineAVS;
        private bool playing;
        private int baseButtonPanelHeight; // height of the transport bar without the optional aspect ratio row
        private string compareScript; // the source (input file) used by the compare modes
        private bool compareInline;
        private IMediaFile compareFile;
        private CompareMode lastCompareMode = CompareMode.Split;

		#endregion
		#region constructor
		public VideoPlayer()
		{
			InitializeComponent();
            this.Resize += new EventHandler(FormResized);

            baseButtonPanelHeight = buttonPanel.Size.Height;
            formHeightDelta = (int)((buttonPanel.Size.Height + 4 * defaultSpacing + SystemInformation.CaptionHeight));
            buttonPanelMinWidth = ComputeMinWidth();

            zoomFactor = 100;
            SetZoomButtons();
            sizeLock = false;

            if (MainForm.Instance != null && MainForm.Instance.Settings != null)
                ThemeManager.Mode = MainForm.Instance.Settings.ThemeMode;
            ThemeManager.ThemeChanged += OnThemeChanged;
            ApplyTheme();

            bool showOsd = MainForm.Instance == null || MainForm.Instance.Settings == null || MainForm.Instance.Settings.PlayerShowOsd;
            videoPreview.OsdVisible = showOsd;
            osdItem.Checked = showOsd;
		}

        public bool AllowClose
        {
            set
            {
                this.ControlBox = value;
            }
            get
            {
                return this.ControlBox;
            }
        }

		/// <summary>
		/// loads the video, sets up the proper window size and enables / disables the GUI buttons depending on the
		/// preview type set
		/// </summary>
		/// <param name="path">path of the video file to be loaded</param>
		/// <param name="type">type of window</param>
		/// <returns>true if the video could be opened, false if not</returns>
        public bool LoadVideo(string path, PREVIEWTYPE type, bool hasAR)
        {
            return LoadVideo(path, type, hasAR, false, -1, false);
        }

        /// <summary>
		/// loads the video, sets up the proper window size and enables / disables the GUI buttons depending on the
		/// preview type set
		/// </summary>
		/// <param name="path">path of the video file to be loaded</param>
		/// <param name="type">type of window</param>
        /// <param name="inlineAvs">true if path contain not filename but avsynth script to be parsed</param>
        /// <param name="startFrame">Select a specific frame to start off with or -1 for middle of video</param>
		/// <returns>true if the video could be opened, false if not</returns>
        public bool LoadVideo(string path, PREVIEWTYPE type, bool hasAR, bool inlineAvs, int startFrame, bool originalSize)
		{
            videoPreview.UnloadVideo();
            bInlineAVS = inlineAvs;
            strFileName = path;
            bOriginalSize = originalSize;

            lock (_locker)
            {
                file?.Dispose();
            }

            try
            {
                if (inlineAvs)
                {
                    file = AvsFile.ParseScript(path, true);
                    btnReloadVideo.Enabled = false;
                }
                else
                {
                    file = MainForm.Instance.MediaFileFactory.Open(path);
                    if (file == null)
                        throw new Exception("The video stream cannot be opened");
                    btnReloadVideo.Enabled = true;
                }
                reader = file.GetVideoReader();
            }
            catch (AviSynthException e)
            {
                MessageBox.Show("AviSynth script error:\r\n" + e.Message, "AviSynth error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            catch (ArgumentException e)
            {
                MessageBox.Show("AviSynth script error:\r\n" + e.Message, "AviSynth error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            catch (Exception e)
            {               
                MessageBox.Show("The file " + path + " cannot be opened.\r\n"
                    + "Error message: " + e.Message,
                    "Cannot open video input", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }

			if (reader != null && reader.FrameCount > 0)
			{
                this.positionSlider.Minimum = 0;
				this.positionSlider.Maximum = reader.FrameCount - 1;
                this.viewerType = type;
                this.hasAR = hasAR;
                ApplyLayoutForClip();
                zoomMaxWidth = 0;
                SetMaxZoomWidth();
                DoInitialAdjustment();
                int iStart;
                if (startFrame >= 0)
                    iStart = startFrame;
                else
                    iStart = reader.FrameCount / 2;
                videoPreview.LoadVideo(reader, file.VideoInfo.FPS, iStart);
                UpdateOsdInfo();
                SetTitleText();
				return true;
			}
			return false;
		}

        private readonly object _locker = new object();
        /// <summary>
        /// reloads the video, sets up the proper window size and enables / disables the GUI buttons depending on the
        /// preview type set
        /// </summary>
        /// <returns>true if the video could be opened, false if not</returns>
        public bool ReloadVideo()
        {
            videoPreview.UnloadVideo();

            lock (_locker)
            {
                file?.Dispose();
            }

            try
            {
                if (bInlineAVS)
                {
                    file = AvsFile.ParseScript(strFileName, true);
                }
                else
                {
                    file = MainForm.Instance.MediaFileFactory.Open(strFileName);
                    if (file == null)
                        throw new Exception("The video stream cannot be opened");
                }
                reader = file.GetVideoReader();
            }
            catch (AviSynthException e)
            {
                MessageBox.Show("AviSynth script error:\r\n" + e.Message, "AviSynth error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            catch (ArgumentException e)
            {
                MessageBox.Show("AviSynth script error:\r\n" + e.Message, "AviSynth error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            catch (Exception e)
            {
                MessageBox.Show("The file " + strFileName + " cannot be opened.\r\n"
                    + "Error message: " + e.Message,
                    "Cannot open video input", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }

            if (reader != null && reader.FrameCount > 0)
            {
                this.positionSlider.Minimum = 0;
                this.positionSlider.Maximum = reader.FrameCount - 1;
                SetMaxZoomWidth();
                DoInitialAdjustment();
                int iStart;
                if (positionSlider.Value >= 0 && positionSlider.Value <= reader.FrameCount)
                    iStart = positionSlider.Value;
                else
                    iStart = reader.FrameCount / 2;
                videoPreview.LoadVideo(reader, file.VideoInfo.FPS, iStart);
                UpdateOsdInfo();
                SetTitleText();
                return true;
            }
            return false;
        }

		/// <summary>
		/// disables intro and credits setting
		/// </summary>
		public void disableIntroAndCredits()
		{
            introEndButton.Visible = false;
            creditsStartButton.Visible = false;
		}
		#endregion

		#region Form sizing
        /// <summary>
        /// Reset the video preview to the size of the input stream
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void originalSizeButton_Click(object sender, EventArgs e)
        {
            bOriginalSize = true;
            zoomWidth = (int)file.VideoInfo.Width;
            // never make the window larger than the screen: fall back to the largest size that fits
            if (zoomMaxWidth > 0 && zoomWidth > zoomMaxWidth)
                zoomWidth = zoomMaxWidth;
            zoomFactor = (int)((double)zoomWidth / zoomMaxWidth * 100.0);
            SetZoomButtons();
            resize(zoomWidth, showPAR.Checked);
        }

        /// <summary>
        /// Reset the video preview to fit the input stream
        /// </summary>
        public void SetScreenSize()
        {
            if (bOriginalSize)
                originalSizeButton_Click(null, null);
            else
                btnFitScreen_Click(null, null);
        }

        /// <summary>
        /// Reset the video preview to fit the input stream
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFitScreen_Click(object sender, EventArgs e)
        {
            bOriginalSize = false;
            zoomWidth = zoomMaxWidth;
            zoomFactor = 100;
            SetZoomButtons();
            SetMaxZoomWidth();
            resize(zoomWidth, showPAR.Checked);
        }

        private void SetZoomButtons()
        {
            if (zoomFactor >= 1000)
            {
                zoomInButton.Enabled = false;
                zoomOutButton.Enabled = true;
            }
            else if (zoomFactor - zoomFactorStepSize > zoomFactorStepSize)
            {
                zoomInButton.Enabled = true;

                int iZoomWidth = (int)(zoomMaxWidth * (zoomFactor - zoomFactorStepSize) / 100);
                if (buttonPanel.Location.X + buttonPanelMinWidth < iZoomWidth)
                    zoomOutButton.Enabled = true;
                else
                    zoomOutButton.Enabled = false;
            }
            else
            {
                zoomInButton.Enabled = true;
                zoomOutButton.Enabled = false;
            }

        }

        private void zoomInButton_Click(object sender, EventArgs e)
        {
            if (zoomFactor > 1000)
                return;

            if (zoomFactor < 1000)
            {
                zoomFactor += zoomFactorStepSize;
                if (zoomFactor > 1000)
                    zoomFactor = 1000;
                SetZoomButtons();
                zoomWidth = (int)(zoomMaxWidth * zoomFactor / 100);
                resize(zoomWidth, showPAR.Checked);
            }
            else if (zoomFactor == 100)
            {
                originalSizeButton_Click(null, null);
            }     
        }

        private void zoomOutButton_Click(object sender, EventArgs e)
        {
            if (zoomFactor > 1000)
            {
                btnFitScreen_Click(null, null);
                return;
            }

            if (zoomFactor - zoomFactorStepSize > zoomFactorStepSize)
            {               
                zoomFactor -= zoomFactorStepSize;
                int iZoomWidth = (int)(zoomMaxWidth * zoomFactor / 100);
                if (buttonPanel.Location.X + buttonPanelMinWidth < iZoomWidth)
                { 
                    zoomWidth = iZoomWidth;
                    resize(zoomWidth, showPAR.Checked);
                }
                else
                    zoomFactor += zoomFactorStepSize;
                SetZoomButtons();
            }
        }

        /// <summary>
        /// sets the maximum zoom width so that the video fits the screen including controls
        /// </summary>
        private void SetMaxZoomWidth()
        {
            Size oSizeScreen = Screen.GetWorkingArea(this).Size;
            int iScreenHeight = oSizeScreen.Height - 2 * SystemInformation.FixedFrameBorderSize.Height;
            int iScreenWidth = oSizeScreen.Width - 2 * SystemInformation.FixedFrameBorderSize.Width;

            // does the video fit into the screen?
            if ((int)file.VideoInfo.Height + formHeightDelta > iScreenHeight ||
                (int)file.VideoInfo.Width > iScreenWidth)
            {
                Dar d = new Dar(file.VideoInfo.Width, file.VideoInfo.Height);
                if (showPAR.Checked) d = arChooser.Value ?? d;

                int height;
                if ((int)file.VideoInfo.Width > iScreenWidth)
                {
                    zoomMaxWidth = iScreenWidth;
                    height = (int)Math.Round((decimal)zoomMaxWidth / d.AR);
                    if (height + formHeightDelta > iScreenHeight)
                    {
                        height = iScreenHeight - formHeightDelta;
                        zoomMaxWidth = (int)Math.Round((decimal)height * d.AR);
                    }
                }
                else
                {
                    height = iScreenHeight - formHeightDelta;
                    zoomMaxWidth = (int)Math.Round((decimal)height * d.AR);
                }
                videoWindowWidth = zoomMaxWidth;
                videoWindowHeight = height;
            }
            else
            {
                zoomMaxWidth = (int)file.VideoInfo.Width;
                videoWindowWidth = zoomMaxWidth;
                videoWindowHeight = (int)file.VideoInfo.Height;
            }

            if (zoomFactor != 100)
            {
                zoomWidth = (int)(zoomMaxWidth * zoomFactor / 100);
                Dar d = new Dar(file.VideoInfo.Width, file.VideoInfo.Height);
                if (showPAR.Checked)
                    d = arChooser.Value ?? d;
                int height = (int)Math.Round((decimal)zoomWidth / d.AR);
                videoWindowWidth = zoomWidth;
                videoWindowHeight = (int)height;
            }

            if (zoomMaxWidth < zoomWidth)
                if (!bOriginalSize)
                    btnFitScreen_Click(null, null);
                else
                    originalSizeButton_Click(null, null);
        }

        private void DoInitialAdjustment()
        {
            bool zones = viewerType == PREVIEWTYPE.ZONES;
            bool credits = viewerType == PREVIEWTYPE.CREDITS;
            bool chapters = viewerType == PREVIEWTYPE.CHAPTERS;
            creditsStartButton.Visible = credits;
            introEndButton.Visible = credits;
            chapterButton.Visible = chapters;
            zoneStartButton.Visible = zones;
            zoneEndButton.Visible = zones;
            setZoneButton.Visible = zones;
        }

		/// <summary>
		/// adjusts the size of the GUI to match the source video
		/// </summary>
		private void AdjustSize()
		{
            SuspendLayout();
            sizeLock = true;

            // Get screen dimensions
            Size oSizeScreen = Screen.GetWorkingArea(this).Size;
            int iScreenWidth = oSizeScreen.Width - 2 * SystemInformation.FixedFrameBorderSize.Width;
            int iScreenHeight = oSizeScreen.Height - 2 * SystemInformation.FixedFrameBorderSize.Height;

            // Respect min width for controls
            int videoPanelWidth = Math.Max(videoWindowWidth, buttonPanelMinWidth);
            videoPanelWidth = Math.Min(videoPanelWidth, iScreenWidth);

            // Reserve space for buttonPanel below videoPanel
            int maxVideoPanelHeight = iScreenHeight
                - buttonPanel.Height
                - MainForm.Instance.Settings.DPIRescale(12)
                - 2 * SystemInformation.FixedFrameBorderSize.Height;
            int videoPanelHeight = Math.Min(videoWindowHeight, maxVideoPanelHeight);

            // Set videoPanel on top
            videoPanel.Location = new Point(0, 0);
            videoPanel.Size = new Size(videoPanelWidth, videoPanelHeight);

            // Set videoPreview inside videoPanel, centered or anchored
            videoPreview.Size = new Size(videoWindowWidth, videoWindowHeight);
            // Optional: center videoPreview in videoPanel if smaller than panel
            int previewOffsetX = Math.Max(0, (videoPanelWidth - videoWindowWidth) / 2);
            int previewOffsetY = Math.Max(0, (videoPanelHeight - videoWindowHeight) / 2);
            videoPreview.Location = new Point(previewOffsetX, previewOffsetY);

            // Set buttonPanel just below videoPanel
            buttonPanel.Size = new Size(videoPanelWidth, buttonPanel.Height);
            buttonPanel.Location = new Point(videoPanel.Left, videoPanel.Bottom);

            // Set Form Size to fit both panels
            int totalHeight = videoPanel.Height + buttonPanel.Height + SystemInformation.CaptionHeight
                + 2 * SystemInformation.FixedFrameBorderSize.Height + MainForm.Instance.Settings.DPIRescale(12);
            int totalWidth = videoPanelWidth + 2 * SystemInformation.FixedFrameBorderSize.Width + MainForm.Instance.Settings.DPIRescale(12);
            if (totalWidth > iScreenWidth) totalWidth = iScreenWidth;
            if (totalHeight > iScreenHeight) totalHeight = iScreenHeight;

            this.Size = new Size(totalWidth, totalHeight);

            ResumeLayout(false);
            sizeLock = false;
        }

        private void FormResized(object sender, EventArgs e)
        {
            if (!sizeLock)
            {
                Control formControl = (Control)sender;
                if ((formControl.Width <= this.MaximumSize.Width) &&
                    (formControl.Height <= this.MaximumSize.Height) &&
                    (formControl.Width >= this.MinimumSize.Width) &&
                    (formControl.Height >= this.MinimumSize.Height))
                {
                    // Unusable without events from .NET 2.0 
                    resize(formControl.Width, showPAR.Checked);
                }
            }
        }
        #endregion

        #region destructor
        /// <summary>
        /// performs additional tasks when the window is closed
        /// ensures that if the AviReader/d2vreader is valid, access to the file is properly closed
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClosing(CancelEventArgs e)
		{
            if (!this.AllowClose)
            {
                if (e != null)
                    e.Cancel = true; return;
            }

            this.Visible = false;
			videoPreview.UnloadVideo();
            DisposeCompare();
            file?.Dispose();
			base.OnClosing(e);
		}
		#endregion
		
		#region position changes
		/// <summary>
        /// Sets the Position property on the videoPreview Control via gotoFrame to render the new frame.
        /// The PositionChanged-EventHandler then updates the window's title
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void positionSlider_Scroll(object sender, System.EventArgs e)
		{
            CurrentFrame = positionSlider.Value;
		}

		/// <summary>
		/// sets the text in the title bar in function of the position, credits and zone settings
		/// </summary>
		private void SetTitleText()
		{
            totalTime = Util.converFrameNumberToTimecode(this.positionSlider.Maximum + 1, file.VideoInfo.FPS);
            currentTime = Util.converFrameNumberToTimecode(this.positionSlider.Value, file.VideoInfo.FPS);
            lblCurrentTime.Text = currentTime;
            lblTotalTime.Text = totalTime;
            lblFrame.Text = "Frame " + CurrentFrame + " / " + FrameCount;
            if (this.zoneStart > -1 || this.zoneEnd > -1)
            {
                this.Text = "Pos: " + CurrentFrame + "/" + FrameCount;
                
                this.Text += " Zone start: " + (zoneStart > -1 ? zoneStart.ToString() : "?");
                this.Text += " end: " + (zoneEnd > -1 ? zoneEnd.ToString() : "?");
            }
            else
                this.Text = "Current position: " + CurrentFrame + "/" + FrameCount;
            if (this.introEndFrame > -1)
				this.Text += " Intro end: " + this.introEndFrame;
			if (this.creditsStartFrame > -1)
				this.Text += " Credits start: " + this.creditsStartFrame;
            if (MainForm.Instance.Settings.AddTimePosition)
                this.Text += "   -   " + currentTime + "/" + totalTime;
		}
		#endregion

		#region cropping
        public void crop(CropValues cropping)
        {
            crop(cropping.left, cropping.top, cropping.right, cropping.bottom);
        }

		/// <summary>
		/// sets the cropping values for this player
		/// </summary>
		/// <param name="left">number of pixels to crop from the left</param>
		/// <param name="top">number of pixels to crop from the top</param>
		/// <param name="right">number of pixels to crop from the right</param>
		/// <param name="bottom">number of pixels to crop from the bottom</param>
		public void crop(int left, int top, int right, int bottom)
		{
            if (videoPreview.CropMargin.Left != left)
            {
                videoPanel.HorizontalScroll.Value = videoPanel.HorizontalScroll.Minimum;
                videoPanel.HorizontalScroll.Value = videoPanel.HorizontalScroll.Minimum;
            }
            else if (videoPreview.CropMargin.Right != right)
            {
                videoPanel.HorizontalScroll.Value = videoPanel.HorizontalScroll.Maximum;
                videoPanel.HorizontalScroll.Value = videoPanel.HorizontalScroll.Maximum;
            }
            if (videoPreview.CropMargin.Top != top)
            {
                videoPanel.VerticalScroll.Value = videoPanel.VerticalScroll.Minimum;
                videoPanel.VerticalScroll.Value = videoPanel.VerticalScroll.Minimum;
            }
            else if (videoPreview.CropMargin.Bottom != bottom)
            {
                videoPanel.VerticalScroll.Value = videoPanel.VerticalScroll.Maximum;
                videoPanel.VerticalScroll.Value = videoPanel.VerticalScroll.Maximum;
            }

            videoPreview.CropMargin = new Padding(left, top, right, bottom);
		}
		#endregion

		#region player
		/// <summary>
		/// handles the play button
		/// starts video playback or stops it
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void playButton_Click(object sender, System.EventArgs e)
		{
			if (!playing)
			{
                playing = true;
                this.playButton.Glyph = PlayerGlyph.Pause;
                videoPreview.EnsureCorrectPlaybackSpeed = MainForm.Instance.Settings.EnsureCorrectPlaybackSpeed;
                videoPreview.Play();
			}
			else
			{
                playing = false;
                this.playButton.Glyph = PlayerGlyph.Play;
                videoPreview.Stop();
			}
		}
        
		private void previousFrameButton_Click(object sender, System.EventArgs e)
		{
            videoPreview.OffsetPosition(-1);
		}

		private void nextFrameButton_Click(object sender, System.EventArgs e)
		{
            videoPreview.OffsetPosition(1);
		}

		private void fwdButton_Click(object sender, System.EventArgs e)
		{
            videoPreview.OffsetPosition(-25);
		}

		private void ffButton_Click(object sender, System.EventArgs e)
		{
            videoPreview.OffsetPosition(25);
        }
		#endregion

		#region credits / intro
		/// <summary>
		/// fires an event indicating the credits start position has been set.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void creditsStartButton_Click(object sender, System.EventArgs e)
		{
            IntroCreditsFrameSet?.Invoke(CurrentFrame, true);
        }

		/// <summary>
		/// fires an event indicating that the intro end position has been set
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void introEndButton_Click(object sender, System.EventArgs e)
		{
            IntroCreditsFrameSet?.Invoke(CurrentFrame, false);
        }
		#endregion

		#region zones
		private void zoneEndButton_Click(object sender, System.EventArgs e)
		{
            int pos = CurrentFrame;
			if (creditsStartFrame > -1 && pos >= creditsStartFrame)
			{
				MessageBox.Show("Zone end intersects with credits zone\nPlease adjust zone end or credits zone", "Zone interesection detected", 
					MessageBoxButtons.OK, MessageBoxIcon.Stop);
			}
			else
			{
                ZoneEnd = pos;
			}
		}

		private void zoneStartButton_Click(object sender, System.EventArgs e)
		{
            int pos = CurrentFrame;
			if (pos > this.introEndFrame) // else we have an intersection with the credits which is not allowed
			{
				if (this.creditsStartFrame > -1 && pos >= creditsStartFrame) // zone starts inside credits zone, not allowed
				{
					MessageBox.Show("Zone start intersects with credits zone\nPlease adjust zone start or credits zone", "Zone interesection detected", 
						MessageBoxButtons.OK, MessageBoxIcon.Stop);
				}
				else
				{
                    ZoneStart = pos;
				}
			}
			else
				MessageBox.Show("Zone start intersects with with intro zone\nPlease adjust zone start or intro zone.", "Zone intersection detected",
					MessageBoxButtons.OK, MessageBoxIcon.Stop);
		}

		private void setZoneButton_Click(object sender, System.EventArgs e)
		{
			if (ZoneSet != null)
			{
				if (zoneEnd > zoneStart)
				{
					ZoneSet(this.zoneStart, this.zoneEnd);
                    zoneStart = -1;
                    zoneEnd = -1;
                    positionSlider.ZoneStart = -1;
                    positionSlider.ZoneEnd = -1;
					SetTitleText();
				}
				else
					MessageBox.Show("The end of a zone must be after its start", "Invalid zone configuration", MessageBoxButtons.OK, MessageBoxIcon.Stop);
			}
		}
		private void chapterButton_Click(object sender, System.EventArgs e)
		{
            ChapterSet?.Invoke(CurrentFrame);
        }
		#endregion

		#region context menu
		private void mnuIntroEnd_Click(object sender, System.EventArgs e)
		{
            CurrentFrame = introEndFrame;

		}

		private void mnuCreditsStart_Click(object sender, System.EventArgs e)
		{
			this.positionSlider.Value = this.creditsStartFrame;
			positionSlider_Scroll(null, null);
		}

		private void mnuZoneStart_Click(object sender, System.EventArgs e)
		{
             CurrentFrame = creditsStartFrame;
		}

		private void mnuZoneEnd_Click(object sender, System.EventArgs e)
		{
            CurrentFrame = zoneEnd;
		}

		private void contextMenu1_Popup(object sender, System.EventArgs e)
		{
			if (this.introEndFrame > -1)
				this.mnuIntroEnd.Enabled = true;
			else
				this.mnuIntroEnd.Enabled = false;
			if (this.creditsStartFrame > -1)
				this.mnuCreditsStart.Enabled = true;
			else
				this.mnuCreditsStart.Enabled = false;
			if (this.zoneStart > -1)
				this.mnuZoneStart.Enabled = true;
			else
				this.mnuZoneStart.Enabled = false;
			if (this.zoneEnd > -1)
				this.mnuZoneEnd.Enabled = true;
			else
				this.mnuZoneEnd.Enabled = false;
		}
		#endregion

		#region properties
        public Dar? DAR
        {
            get { return arChooser.Value; }
            set { arChooser.Value = value; }
        }

		/// <summary>
		/// returns the underlying video reader
		/// </summary>
		/// <returns>the VideoReader object used by this window for the preview</returns>
		public IVideoReader Reader
		{
			get {return this.reader;}
		}

        /// <summary>
        /// returns the underlying media file
        /// </summary>
        /// <returns>the IMediaFile used by this window for the preview</returns>
        public IMediaFile File
        {
            get { return this.file; }
        }

		/// <summary>
		/// gets /sets the frame where the credits start
		/// </summary>
		public int CreditsStart
		{
			get {return this.creditsStartFrame;}
			set 
			{
					creditsStartFrame = value;
                    positionSlider.CreditsStart = value;
				SetTitleText();
			}
		}

		/// <summary>
		/// gets / sets the frame where the intro ends
		/// </summary>
		public int IntroEnd
		{
			get {return this.introEndFrame;}
			set 
			{
					introEndFrame = value;
                    positionSlider.IntroEnd = value;
				SetTitleText();
			}
		}

		/// <summary>
		/// gets / sets the frame where the current zone starts
		/// </summary>
		public int ZoneStart
		{
			get {return this.zoneStart;}
			set 
			{
					zoneStart = value;
                    positionSlider.ZoneStart = value;
                //positionSlider_Scroll(null, null);
                SetTitleText();
			}
		}

		/// <summary>
		/// gets / sets the frame where the current zone starts
		/// </summary>
		public int ZoneEnd
		{
			get {return this.zoneEnd;}
			set
			{
					zoneEnd = value;
                    positionSlider.ZoneEnd = value;
				SetTitleText();
			}
		}

		/// <summary>
		/// gets the framerate of the video that is currently loaded
		/// </summary>
		public double Framerate
		{
            get { return file.VideoInfo.FPS; }
		}

		/// <summary>
		/// gets / sets the frame currently visible
		/// </summary>
		public int CurrentFrame
		{
            get { return videoPreview.Position; }
            set { videoPreview.Position = value; }
		}

        public int FrameCount
        {
            get { return videoPreview.FrameCount; }
        }
		#endregion

        private void resize(int targetWidth, bool PAR)
        {
            zoomWidth = targetWidth;
            Dar d = new Dar(file.VideoInfo.Width, file.VideoInfo.Height);
            if (PAR)
                d = arChooser.Value ?? d;

            int height = (int)Math.Round((decimal)targetWidth / d.AR);
            videoWindowWidth = targetWidth;
            videoWindowHeight = height;
            sizeLock = true;
            AdjustSize();
            VideoPlayer_Shown(null, null);
            sizeLock = false;
        }

        private void showPAR_CheckedChanged(object sender, EventArgs e)
        {
            SetMaxZoomWidth();
            resize(videoWindowWidth, showPAR.Checked);
        }

        private void goToFrameButton_Click(object sender, EventArgs e)
        {
            bool bTopMost = this.TopMost;
            this.TopMost = false;
            if (NumberChooser.ShowDialog("Enter a frame number:", "Go to frame",
                0, positionSlider.Minimum, positionSlider.Maximum, positionSlider.Value, 1, out decimal val)
                == DialogResult.OK)
            {
                positionSlider.Value = (int)val;
                positionSlider_Scroll(null, null);
            }
            this.TopMost = bTopMost;
        }

        private void arChooser_SelectionChanged(object sender, string val)
        {
            SetMaxZoomWidth();
            resize(videoWindowWidth, showPAR.Checked);
        }

        private void nextFrameButton_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Right)
                videoPreview.OffsetPosition(1);
        }

        private void previousFrameButton_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Right)
                videoPreview.OffsetPosition(-1);
        }

        private void VideoPlayer_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
            else if (e.KeyCode == Keys.I && !e.Control && !e.Alt)
            {
                osdItem_Click(null, EventArgs.Empty);
                e.Handled = true;
            }
        }

        protected override bool ProcessDialogKey(Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Escape:
                    Close();
                    return true;

                case Keys.Control | Keys.G:
                    goToFrameButton_Click(null, EventArgs.Empty);
                    return true;
            }

            return base.ProcessDialogKey(keyData);
        }

        private void videoPreview_PositionChanged(object sender, EventArgs e)
        {
            if (videoPreview.Position > -1)
            {
                positionSlider.Value = videoPreview.Position;
                SetTitleText();
            }
            else
                this.Text = "Error in AviSynth script";
        }

        private void btnReloadVideo_Click(object sender, EventArgs e)
        {
            if (playing)
            {
                playing = false;
                this.playButton.Glyph = PlayerGlyph.Play;
                videoPreview.Stop();
			}
            ReloadVideo();
            positionSlider.Focus();
        }

        private void VideoPlayer_Shown(object sender, EventArgs e)
        {
            Size oSizeScreen = Screen.GetWorkingArea(this).Size;
            Point oLocation = Screen.GetWorkingArea(this).Location;
            int iScreenHeight = oSizeScreen.Height - 2 * SystemInformation.FixedFrameBorderSize.Height;
            int iScreenWidth = oSizeScreen.Width - 2 * SystemInformation.FixedFrameBorderSize.Width;
            
            if (this.Size.Height >= iScreenHeight)
                this.Location = new Point(this.Location.X, oLocation.Y + 5);
            else if (this.Location.Y <= oLocation.Y)
                this.Location = new Point(this.Location.X, oLocation.Y + 5);
            else if (this.Location.Y + this.Size.Height > iScreenHeight)
                this.Location = new Point(this.Location.X, iScreenHeight - this.Size.Height);

            if (this.Size.Width >= iScreenWidth)
                this.Location = new Point(oLocation.X + 3, this.Location.Y);
            else if (this.Location.X <= oLocation.X)
                this.Location = new Point(oLocation.X + 3, this.Location.Y);
            else if (this.Location.X + this.Size.Width > iScreenWidth)
                this.Location = new Point(iScreenWidth - this.Size.Width, this.Location.Y);
        }

        #region OSD
        private void osdItem_Click(object sender, EventArgs e)
        {
            bool show = !videoPreview.OsdVisible;
            videoPreview.OsdVisible = show;
            osdItem.Checked = show;
            if (MainForm.Instance != null && MainForm.Instance.Settings != null)
                MainForm.Instance.Settings.PlayerShowOsd = show;
        }

        private void UpdateOsdInfo()
        {
            videoPreview.OsdInfo = BuildOsdInfo();
        }

        /// <summary>
        /// The static part of the OSD: what the AviSynth Info() filter shows about the clip
        /// (the frame number and the time are added for each picture by the video control)
        /// </summary>
        private string BuildOsdInfo()
        {
            VideoInformation vi = file.VideoInfo;
            StringBuilder sb = new StringBuilder();
            AvsFile avs = file as AvsFile;
            if (avs != null && avs.Clip != null)
            {
                AviSynthClip clip = avs.Clip;
                AvsProbe.StreamInfo i = clip.OriginalStreamInfo;
                sb.AppendLine("ColorSpace: " + i.ColorSpace + ", Bit Depth: " + i.BitDepth + (i.FullRange ? ", Full range" : ", Limited range"));
                sb.AppendLine("Width: " + i.Width + " pixels, Height: " + i.Height + " pixels");
                sb.AppendLine("Frames per second: " + (i.FpsDen > 0 ? ((double)i.FpsNum / i.FpsDen).ToString("0.0000", CultureInfo.InvariantCulture) : "?")
                    + " (" + i.FpsNum + "/" + i.FpsDen + ")");
                sb.AppendLine("Duration: " + Util.converFrameNumberToTimecode(i.NumFrames, vi.FPS) + " (" + i.NumFrames + " frames)");
                sb.AppendLine("FieldBased (Separated) Video: " + (i.IsFieldBased ? "YES" : "NO"));
                sb.AppendLine("Parity: " + (i.IsTff ? "Top Field First" : i.IsBff ? "Bottom Field First" : "Unknown"));
                sb.AppendLine("Video Pitch: " + i.VideoPitch + " bytes");
                if (i.NumAudioSamples > 0)
                {
                    double seconds = i.AudioSampleRate > 0 ? i.NumAudioSamples / (double)i.AudioSampleRate : 0;
                    sb.AppendLine("Audio: " + i.AudioChannels + " channel(s), " + clip.SampleType + ", " + i.AudioSampleRate + " Hz, "
                        + i.NumAudioSamples + " samples (" + TimeSpan.FromSeconds(seconds).ToString(@"hh\:mm\:ss\.fff") + ")");
                }
                else
                    sb.AppendLine("Audio: none");
                if (clip.ConvertedToRGB24)
                    sb.AppendLine("Preview: converted to RGB24");
            }
            else
            {
                sb.AppendLine("Width: " + vi.Width + " pixels, Height: " + vi.Height + " pixels");
                sb.AppendLine("Frames per second: " + vi.FPS.ToString("0.0000", CultureInfo.InvariantCulture));
                sb.AppendLine("Duration: " + Util.converFrameNumberToTimecode((int)vi.FrameCount, vi.FPS) + " (" + vi.FrameCount + " frames)");
            }
            return sb.ToString().TrimEnd('\r', '\n');
        }
        #endregion

        #region theme
        private void OnThemeChanged(object sender, EventArgs e)
        {
            if (IsDisposed || !IsHandleCreated)
                return;
            if (InvokeRequired)
                BeginInvoke(new SimpleDelegate(ApplyTheme));
            else
                ApplyTheme();
        }

        private void ApplyTheme()
        {
            ThemePalette p = ThemeManager.Palette;
            ThemeManager.Apply(this);
            buttonPanel.BackColor = p.Window;
            videoPanel.BackColor = p.VideoBack;
            lblCurrentTime.ForeColor = p.TextDim;
            lblTotalTime.ForeColor = p.TextDim;
            lblFrame.ForeColor = p.TextDim;
            ThemeManager.Apply(moreMenu);

            ThemeMode mode = ThemeManager.Mode;
            themeSystemItem.Checked = mode == ThemeMode.System;
            themeDarkItem.Checked = mode == ThemeMode.Dark;
            themeLightItem.Checked = mode == ThemeMode.Light;
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            ThemeManager.ApplyTitleBar(this);
        }

        private void themeItem_Click(object sender, EventArgs e)
        {
            ThemeMode mode = ThemeMode.System;
            if (sender == themeDarkItem)
                mode = ThemeMode.Dark;
            else if (sender == themeLightItem)
                mode = ThemeMode.Light;

            if (MainForm.Instance != null && MainForm.Instance.Settings != null)
                MainForm.Instance.Settings.ThemeMode = mode;
            ThemeManager.Mode = mode;
            ApplyTheme(); // also refreshes the check marks when the effective theme did not change
        }
        #endregion

        #region transport bar layout
        /// <summary>
        /// Minimum width of the transport bar for the controls currently visible
        /// </summary>
        private int ComputeMinWidth()
        {
            int pad = Math.Max(6, baseButtonPanelHeight / 9);
            int gap = Math.Max(2, pad / 3);
            int center = fwdButton.Width + previousFrameButton.Width + playButton.Width + nextFrameButton.Width + ffButton.Width + 4 * gap;
            int left = lblFrame.Width;
            int right = 0;
            if (compareButton.Visible)
            {
                right += gap + compareButton.Width;
                if (compareModeButton.Visible)
                    right += gap + compareModeButton.Width;
            }
            return 2 * (pad + Math.Max(left, right)) + center;
        }

        private void LayoutTransport()
        {
            int w = buttonPanel.ClientSize.Width;
            if (w <= 0)
                return;
            int pad = Math.Max(6, baseButtonPanelHeight / 9);
            int gap = Math.Max(2, pad / 3);

            // row 1: current time | seek bar | total time
            int row1Top = pad / 2;
            int timeW = lblCurrentTime.Width;
            lblCurrentTime.Location = new Point(pad, row1Top);
            lblTotalTime.Location = new Point(w - pad - timeW, row1Top);
            positionSlider.Location = new Point(pad + timeW + gap, row1Top);
            positionSlider.Width = Math.Max(10, w - 2 * (pad + timeW + gap));
            lblCurrentTime.Height = lblTotalTime.Height = positionSlider.Height;

            // row 2: frame label | transport buttons (centered) | compare / menu
            int row2Top = row1Top + positionSlider.Height + gap;
            int row2Height = playButton.Height;

            int clusterW = fwdButton.Width + previousFrameButton.Width + playButton.Width + nextFrameButton.Width + ffButton.Width + 4 * gap;
            int x = (w - clusterW) / 2;
            foreach (Control c in new Control[] { fwdButton, previousFrameButton, playButton, nextFrameButton, ffButton })
            {
                c.Location = new Point(x, row2Top + (row2Height - c.Height) / 2);
                x += c.Width + gap;
            }

            lblFrame.Location = new Point(pad, row2Top + (row2Height - lblFrame.Height) / 2);

            int rx = w - pad;
            if (compareModeButton.Visible)
            {
                rx -= compareModeButton.Width;
                compareModeButton.Location = new Point(rx, row2Top + (row2Height - compareModeButton.Height) / 2);
            }
            if (compareButton.Visible)
            {
                rx -= gap + compareButton.Width;
                if (!compareModeButton.Visible)
                    rx += gap;
                compareButton.Location = new Point(rx, row2Top + (row2Height - compareButton.Height) / 2);
            }

            // optional row 3: aspect ratio chooser
            int row3Top = row2Top + row2Height + gap;
            arChooser.Location = new Point(pad, row3Top);
            showPAR.Location = new Point(arChooser.Right + gap, row3Top + (arChooser.Height - showPAR.Height) / 2);
        }

        private void buttonPanel_Resize(object sender, EventArgs e)
        {
            LayoutTransport();
        }

        /// <summary>
        /// Shows or hides the aspect ratio row and recomputes the sizes that depend on it
        /// </summary>
        private void ApplyLayoutForClip()
        {
            arChooser.Visible = hasAR;
            showPAR.Visible = hasAR;

            int pad = Math.Max(6, baseButtonPanelHeight / 9);
            int height = baseButtonPanelHeight;
            if (hasAR)
                height += arChooser.Height + pad;
            buttonPanel.Height = height;
            formHeightDelta = (int)((buttonPanel.Size.Height + 4 * defaultSpacing + SystemInformation.CaptionHeight));
            buttonPanelMinWidth = ComputeMinWidth();
            LayoutTransport();
        }

        private void moreMenu_Opening(object sender, CancelEventArgs e)
        {
            bool available = !string.IsNullOrEmpty(compareScript);
            CompareMode mode = videoPreview.CompareMode;
            compareSplitItem.Enabled = available;
            compareSideItem.Enabled = available;
            compareSplitItem.Checked = mode == CompareMode.Split;
            compareSideItem.Checked = mode == CompareMode.SideBySide;
            string hint = available ? null : "Available once a filtered preview is shown (click Preview in the AVS Script Creator)";
            compareSplitItem.ToolTipText = hint ?? "Source (input file) on the left of a draggable divider, filtered video on the right";
            compareSideItem.ToolTipText = hint ?? "Source (input file) and filtered video next to each other";
        }

        private void compareItem_Click(object sender, EventArgs e)
        {
            CompareMode chosen = sender == compareSideItem ? CompareMode.SideBySide : CompareMode.Split;
            if (videoPreview.CompareMode == chosen)
            {
                videoPreview.CompareMode = CompareMode.Off; // clicking the checked mode switches the comparison off
            }
            else if (OpenCompareSource())
            {
                lastCompareMode = chosen;
                videoPreview.CompareMode = chosen;
            }
            UpdateCompareButtons();
        }
        #endregion

        #region compare source / filtered
        /// <summary>
        /// Sets the clip shown as "Source" by the compare modes: the input file before any filter.
        /// The Compare button is only available while a source is set.
        /// </summary>
        /// <param name="script">path of the input (or index) file, or an AviSynth script if inlineAvs is true; null/empty to disable the compare modes</param>
        /// <param name="inlineAvs">true if script contains an AviSynth script and not a file name</param>
        public void SetCompareSource(string script, bool inlineAvs)
        {
            if (script == compareScript && inlineAvs == compareInline)
                return; // unchanged: keep the opened source and the current compare mode

            DisposeCompare();
            compareScript = script;
            compareInline = inlineAvs;
            UpdateCompareButtons();
        }

        private void UpdateCompareButtons()
        {
            bool available = !string.IsNullOrEmpty(compareScript);
            CompareMode mode = videoPreview.CompareMode;
            compareButton.Visible = available;
            compareButton.Toggled = mode != CompareMode.Off;
            compareModeButton.Visible = available && mode != CompareMode.Off;
            compareModeButton.Text = lastCompareMode == CompareMode.SideBySide ? "Side by side" : "Split";
            buttonPanelMinWidth = ComputeMinWidth();
            LayoutTransport();
        }

        private bool OpenCompareSource()
        {
            if (compareFile != null)
                return true;
            try
            {
                if (compareInline)
                    compareFile = AvsFile.ParseScript(compareScript, true);
                else
                    compareFile = MainForm.Instance.MediaFileFactory.Open(compareScript);
                if (compareFile == null)
                    throw new Exception("The source video cannot be opened");
                IVideoReader sourceReader = compareFile.GetVideoReader();
                if (sourceReader == null || sourceReader.FrameCount <= 0)
                    throw new Exception("The source video cannot be read");
                videoPreview.SetCompareReader(sourceReader);
                return true;
            }
            catch (Exception ex)
            {
                videoPreview.SetCompareReader(null);
                lock (_locker)
                {
                    compareFile?.Dispose();
                    compareFile = null;
                }
                MessageBox.Show("The source cannot be opened for comparison.\r\n" + ex.Message, "Compare",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
        }

        private void DisposeCompare()
        {
            DisposeCompare(true);
        }

        /// <param name="updateUi">false when the window itself is being disposed: its controls must not be touched any more</param>
        private void DisposeCompare(bool updateUi)
        {
            if (!videoPreview.IsDisposed)
            {
                videoPreview.CompareMode = CompareMode.Off;
                videoPreview.SetCompareReader(null); // returns when the render thread no longer uses the reader
            }
            lock (_locker)
            {
                compareFile?.Dispose();
                compareFile = null;
            }
            if (updateUi && !IsDisposed)
                UpdateCompareButtons();
        }

        private void compareButton_Click(object sender, EventArgs e)
        {
            if (videoPreview.CompareMode != CompareMode.Off)
                videoPreview.CompareMode = CompareMode.Off;
            else if (OpenCompareSource())
                videoPreview.CompareMode = lastCompareMode;
            UpdateCompareButtons();
        }

        private void compareModeButton_Click(object sender, EventArgs e)
        {
            lastCompareMode = lastCompareMode == CompareMode.Split ? CompareMode.SideBySide : CompareMode.Split;
            if (videoPreview.CompareMode != CompareMode.Off)
                videoPreview.CompareMode = lastCompareMode;
            UpdateCompareButtons();
        }
        #endregion

	}
}