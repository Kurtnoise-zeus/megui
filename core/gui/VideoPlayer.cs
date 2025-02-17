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
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
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

		#endregion
		#region constructor
		public VideoPlayer()
		{
			InitializeComponent();
            this.Resize += new EventHandler(FormResized);

            formHeightDelta = (int)((buttonPanel.Size.Height + 4 * defaultSpacing));
            buttonPanelMinWidth = (int)((showPAR.Location.X + showPAR.Size.Width));

            zoomFactor = 100;
            SetZoomButtons();
            sizeLock = false;
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
                this.positionSlider.TickFrequency = this.positionSlider.Maximum / 20;
                this.viewerType = type;
                this.hasAR = hasAR;
                zoomMaxWidth = 0;
                SetMaxZoomWidth();
                DoInitialAdjustment();
                int iStart;
                if (startFrame >= 0)
                    iStart = startFrame;
                else
                    iStart = reader.FrameCount / 2;
                videoPreview.LoadVideo(reader, file.VideoInfo.FPS, iStart);
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
                this.positionSlider.TickFrequency = this.positionSlider.Maximum / 20;
                SetMaxZoomWidth();
                DoInitialAdjustment();
                int iStart;
                if (positionSlider.Value >= 0 && positionSlider.Value <= reader.FrameCount)
                    iStart = positionSlider.Value;
                else
                    iStart = reader.FrameCount / 2;
                videoPreview.LoadVideo(reader, file.VideoInfo.FPS, iStart);
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
			buttonPanel.Controls.Remove(introEndButton);
			buttonPanel.Controls.Remove(creditsStartButton);
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
            switch (this.viewerType)
            {
                case PREVIEWTYPE.REGULAR:
                    buttonPanel.Controls.Remove(creditsStartButton);
                    buttonPanel.Controls.Remove(introEndButton);
                    buttonPanel.Controls.Remove(chapterButton);
                    buttonPanel.Controls.Remove(zoneStartButton);
                    buttonPanel.Controls.Remove(zoneEndButton);
                    buttonPanel.Controls.Remove(setZoneButton);
                    break;
                case PREVIEWTYPE.ZONES:
                    buttonPanel.Controls.Remove(creditsStartButton);
                    buttonPanel.Controls.Remove(introEndButton);
                    buttonPanel.Controls.Remove(chapterButton);
                    break;
                case PREVIEWTYPE.CREDITS:
                    buttonPanel.Controls.Remove(zoneStartButton);
                    buttonPanel.Controls.Remove(zoneEndButton);
                    buttonPanel.Controls.Remove(setZoneButton);
                    buttonPanel.Controls.Remove(chapterButton);
                    break;
                case PREVIEWTYPE.CHAPTERS:
                    buttonPanel.Controls.Remove(creditsStartButton);
                    buttonPanel.Controls.Remove(introEndButton);
                    buttonPanel.Controls.Remove(zoneStartButton);
                    buttonPanel.Controls.Remove(zoneEndButton);
                    buttonPanel.Controls.Remove(setZoneButton);
                    break;
            }
        }

		/// <summary>
		/// adjusts the size of the GUI to match the source video
		/// </summary>
		private void AdjustSize()
		{
            if (!hasAR)
            {
                buttonPanel.Controls.Remove(arChooser);
                buttonPanel.Controls.Remove(showPAR);
            }
            SuspendLayout();
           
            // resize main window
            sizeLock = true;
            int iMainHeight = this.videoWindowHeight + formHeightDelta;
            int iMainWidth = this.videoWindowWidth + 2 * SystemInformation.FixedFrameBorderSize.Width + MainForm.Instance.Settings.DPIRescale(12);
            if (bOriginalSize)
            {
                Size oSizeScreen = Screen.GetWorkingArea(this).Size;
                int iScreenHeight = oSizeScreen.Height - 2 * SystemInformation.FixedFrameBorderSize.Height;
                int iScreenWidth = oSizeScreen.Width - 2 * SystemInformation.FixedFrameBorderSize.Width;

                if (iMainHeight >= iScreenHeight)
                    iMainHeight = iScreenHeight;

                if (iMainWidth >= iScreenWidth)
                    iMainWidth = iScreenWidth;
            }
            if (iMainWidth - 2 * SystemInformation.FixedFrameBorderSize.Width - 2 < buttonPanelMinWidth)
                iMainWidth = buttonPanelMinWidth + 2 * SystemInformation.FixedFrameBorderSize.Width;
            this.Size = new Size(iMainWidth, iMainHeight);

            // resize videoPanel
            this.videoPanel.Size = new Size(iMainWidth - 2 * SystemInformation.FixedFrameBorderSize.Width, iMainHeight - formHeightDelta + 2);
            sizeLock = false;

            // resize videoPreview
            this.videoPreview.Size = new Size(this.videoWindowWidth, this.videoWindowHeight);

            // resize buttonPanel
            this.buttonPanel.Size = new Size(iMainWidth - 2 * SystemInformation.FixedFrameBorderSize.Width, buttonPanel.Height);
            this.buttonPanel.Location = new Point(1, videoPanel.Location.Y + videoPanel.Size.Height);
            ResumeLayout();
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
			if (this.playButton.Text.Equals("Play"))
			{
				this.playButton.Text = "Stop";
                videoPreview.EnsureCorrectPlaybackSpeed = MainForm.Instance.Settings.EnsureCorrectPlaybackSpeed;
                videoPreview.Play();
			}
			else
			{
				this.playButton.Text = "Play";
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
            if (playButton.Text.Equals("Stop"))
            {
				this.playButton.Text = "Play";
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
	}
}