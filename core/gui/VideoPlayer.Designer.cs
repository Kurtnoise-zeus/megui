// ****************************************************************************
// 
// Copyright (C) 2005-2024 Doom9 & al
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

using System.Windows.Forms;

using MeGUI.core.gui;

namespace MeGUI
{
    partial class VideoPlayer
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.Button goToFrameButton;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VideoPlayer));
            this.contextMenu1 = new System.Windows.Forms.ContextMenu();
            this.mnuIntroEnd = new System.Windows.Forms.MenuItem();
            this.mnuCreditsStart = new System.Windows.Forms.MenuItem();
            this.mnuZoneStart = new System.Windows.Forms.MenuItem();
            this.mnuZoneEnd = new System.Windows.Forms.MenuItem();
            this.playButton = new System.Windows.Forms.Button();
            this.nextFrameButton = new System.Windows.Forms.Button();
            this.previousFrameButton = new System.Windows.Forms.Button();
            this.ffButton = new System.Windows.Forms.Button();
            this.fwdButton = new System.Windows.Forms.Button();
            this.creditsStartButton = new System.Windows.Forms.Button();
            this.buttonPanel = new System.Windows.Forms.Panel();
            this.positionSlider = new System.Windows.Forms.TrackBar();
            this.btnFitScreen = new System.Windows.Forms.Button();
            this.btnReloadVideo = new System.Windows.Forms.Button();
            this.zoomOutButton = new System.Windows.Forms.Button();
            this.zoomInButton = new System.Windows.Forms.Button();
            this.showPAR = new System.Windows.Forms.CheckBox();
            this.originalSizeButton = new System.Windows.Forms.Button();
            this.introEndButton = new System.Windows.Forms.Button();
            this.zoneStartButton = new System.Windows.Forms.Button();
            this.setZoneButton = new System.Windows.Forms.Button();
            this.zoneEndButton = new System.Windows.Forms.Button();
            this.chapterButton = new System.Windows.Forms.Button();
            this.defaultToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.videoPanel = new System.Windows.Forms.Panel();
            this.videoPreview = new MeGUI.core.gui.VideoPlayerControl();
            this.arChooser = new MeGUI.core.gui.ARChooser();
            goToFrameButton = new System.Windows.Forms.Button();
            this.buttonPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.positionSlider)).BeginInit();
            this.videoPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // goToFrameButton
            // 
            goToFrameButton.Location = new System.Drawing.Point(4, 75);
            goToFrameButton.Name = "goToFrameButton";
            goToFrameButton.Size = new System.Drawing.Size(82, 20);
            goToFrameButton.TabIndex = 13;
            goToFrameButton.Text = "Go to frame";
            goToFrameButton.Click += new System.EventHandler(this.goToFrameButton_Click);
            // 
            // contextMenu1
            // 
            this.contextMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuIntroEnd,
            this.mnuCreditsStart,
            this.mnuZoneStart,
            this.mnuZoneEnd});
            this.contextMenu1.Popup += new System.EventHandler(this.contextMenu1_Popup);
            // 
            // mnuIntroEnd
            // 
            this.mnuIntroEnd.Index = 0;
            this.mnuIntroEnd.Shortcut = System.Windows.Forms.Shortcut.CtrlI;
            this.mnuIntroEnd.ShowShortcut = false;
            this.mnuIntroEnd.Text = "Go to End of &Intro";
            this.mnuIntroEnd.Click += new System.EventHandler(this.mnuIntroEnd_Click);
            // 
            // mnuCreditsStart
            // 
            this.mnuCreditsStart.Index = 1;
            this.mnuCreditsStart.Shortcut = System.Windows.Forms.Shortcut.CtrlC;
            this.mnuCreditsStart.ShowShortcut = false;
            this.mnuCreditsStart.Text = "Go to Start of &Credits";
            this.mnuCreditsStart.Click += new System.EventHandler(this.mnuCreditsStart_Click);
            // 
            // mnuZoneStart
            // 
            this.mnuZoneStart.Index = 2;
            this.mnuZoneStart.Shortcut = System.Windows.Forms.Shortcut.CtrlS;
            this.mnuZoneStart.ShowShortcut = false;
            this.mnuZoneStart.Text = "Go to &Start of Zone";
            this.mnuZoneStart.Click += new System.EventHandler(this.mnuZoneStart_Click);
            // 
            // mnuZoneEnd
            // 
            this.mnuZoneEnd.Index = 3;
            this.mnuZoneEnd.Shortcut = System.Windows.Forms.Shortcut.CtrlE;
            this.mnuZoneEnd.ShowShortcut = false;
            this.mnuZoneEnd.Text = "Go to &End of Zone";
            this.mnuZoneEnd.Click += new System.EventHandler(this.mnuZoneEnd_Click);
            // 
            // playButton
            // 
            this.playButton.Location = new System.Drawing.Point(50, 49);
            this.playButton.Name = "playButton";
            this.playButton.Size = new System.Drawing.Size(40, 20);
            this.playButton.TabIndex = 4;
            this.playButton.Text = "Play";
            this.defaultToolTip.SetToolTip(this.playButton, "play");
            this.playButton.Click += new System.EventHandler(this.playButton_Click);
            // 
            // nextFrameButton
            // 
            this.nextFrameButton.Location = new System.Drawing.Point(120, 49);
            this.nextFrameButton.Name = "nextFrameButton";
            this.nextFrameButton.Size = new System.Drawing.Size(16, 20);
            this.nextFrameButton.TabIndex = 6;
            this.nextFrameButton.Text = ">";
            this.defaultToolTip.SetToolTip(this.nextFrameButton, "Advance by 1 frame");
            this.nextFrameButton.Click += new System.EventHandler(this.nextFrameButton_Click);
            this.nextFrameButton.KeyDown += new System.Windows.Forms.KeyEventHandler(this.nextFrameButton_KeyDown);
            // 
            // previousFrameButton
            // 
            this.previousFrameButton.Location = new System.Drawing.Point(4, 49);
            this.previousFrameButton.Name = "previousFrameButton";
            this.previousFrameButton.Size = new System.Drawing.Size(16, 18);
            this.previousFrameButton.TabIndex = 2;
            this.previousFrameButton.Text = "<";
            this.defaultToolTip.SetToolTip(this.previousFrameButton, "Go back 1 frame");
            this.previousFrameButton.Click += new System.EventHandler(this.previousFrameButton_Click);
            this.previousFrameButton.KeyDown += new System.Windows.Forms.KeyEventHandler(this.previousFrameButton_KeyDown);
            // 
            // ffButton
            // 
            this.ffButton.Location = new System.Drawing.Point(90, 49);
            this.ffButton.Name = "ffButton";
            this.ffButton.Size = new System.Drawing.Size(30, 20);
            this.ffButton.TabIndex = 5;
            this.ffButton.Text = ">>";
            this.defaultToolTip.SetToolTip(this.ffButton, "Advance 25 frames");
            this.ffButton.Click += new System.EventHandler(this.ffButton_Click);
            // 
            // fwdButton
            // 
            this.fwdButton.Location = new System.Drawing.Point(20, 49);
            this.fwdButton.Name = "fwdButton";
            this.fwdButton.Size = new System.Drawing.Size(30, 20);
            this.fwdButton.TabIndex = 3;
            this.fwdButton.Text = "<<";
            this.defaultToolTip.SetToolTip(this.fwdButton, "Go back 25 frames");
            this.fwdButton.Click += new System.EventHandler(this.fwdButton_Click);
            // 
            // creditsStartButton
            // 
            this.creditsStartButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.creditsStartButton.Location = new System.Drawing.Point(380, 75);
            this.creditsStartButton.Name = "creditsStartButton";
            this.creditsStartButton.Size = new System.Drawing.Size(44, 20);
            this.creditsStartButton.TabIndex = 20;
            this.creditsStartButton.Text = "Credits";
            this.defaultToolTip.SetToolTip(this.creditsStartButton, "Set the frame where the credits start");
            this.creditsStartButton.Click += new System.EventHandler(this.creditsStartButton_Click);
            // 
            // buttonPanel
            // 
            this.buttonPanel.Controls.Add(this.positionSlider);
            this.buttonPanel.Controls.Add(this.btnFitScreen);
            this.buttonPanel.Controls.Add(this.btnReloadVideo);
            this.buttonPanel.Controls.Add(this.zoomOutButton);
            this.buttonPanel.Controls.Add(this.zoomInButton);
            this.buttonPanel.Controls.Add(this.arChooser);
            this.buttonPanel.Controls.Add(this.showPAR);
            this.buttonPanel.Controls.Add(this.originalSizeButton);
            this.buttonPanel.Controls.Add(this.introEndButton);
            this.buttonPanel.Controls.Add(this.previousFrameButton);
            this.buttonPanel.Controls.Add(this.fwdButton);
            this.buttonPanel.Controls.Add(this.playButton);
            this.buttonPanel.Controls.Add(this.ffButton);
            this.buttonPanel.Controls.Add(this.nextFrameButton);
            this.buttonPanel.Controls.Add(this.zoneStartButton);
            this.buttonPanel.Controls.Add(this.creditsStartButton);
            this.buttonPanel.Controls.Add(goToFrameButton);
            this.buttonPanel.Controls.Add(this.setZoneButton);
            this.buttonPanel.Controls.Add(this.zoneEndButton);
            this.buttonPanel.Controls.Add(this.chapterButton);
            this.buttonPanel.Location = new System.Drawing.Point(1, 225);
            this.buttonPanel.Margin = new System.Windows.Forms.Padding(0);
            this.buttonPanel.Name = "buttonPanel";
            this.buttonPanel.Size = new System.Drawing.Size(500, 118);
            this.buttonPanel.TabIndex = 8;
            // 
            // positionSlider
            // 
            this.positionSlider.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.positionSlider.LargeChange = 1000;
            this.positionSlider.Location = new System.Drawing.Point(0, 0);
            this.positionSlider.Margin = new System.Windows.Forms.Padding(0);
            this.positionSlider.Minimum = -1;
            this.positionSlider.Name = "positionSlider";
            this.positionSlider.Size = new System.Drawing.Size(495, 45);
            this.positionSlider.TabIndex = 1;
            this.positionSlider.TickFrequency = 1500;
            this.positionSlider.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.positionSlider.Value = -1;
            this.positionSlider.Scroll += new System.EventHandler(this.positionSlider_Scroll);
            // 
            // btnFitScreen
            // 
            this.btnFitScreen.Location = new System.Drawing.Point(189, 49);
            this.btnFitScreen.Name = "btnFitScreen";
            this.btnFitScreen.Size = new System.Drawing.Size(26, 20);
            this.btnFitScreen.TabIndex = 9;
            this.btnFitScreen.Text = "Fit";
            this.defaultToolTip.SetToolTip(this.btnFitScreen, "resets the zoom to the maximum screen size");
            this.btnFitScreen.UseVisualStyleBackColor = true;
            this.btnFitScreen.Click += new System.EventHandler(this.btnFitScreen_Click);
            // 
            // btnReloadVideo
            // 
            this.btnReloadVideo.Location = new System.Drawing.Point(90, 75);
            this.btnReloadVideo.Name = "btnReloadVideo";
            this.btnReloadVideo.Size = new System.Drawing.Size(79, 20);
            this.btnReloadVideo.TabIndex = 14;
            this.btnReloadVideo.Text = "Reload Video";
            this.defaultToolTip.SetToolTip(this.btnReloadVideo, "Reload the video file");
            this.btnReloadVideo.Click += new System.EventHandler(this.btnReloadVideo_Click);
            // 
            // zoomOutButton
            // 
            this.zoomOutButton.Location = new System.Drawing.Point(215, 49);
            this.zoomOutButton.Name = "zoomOutButton";
            this.zoomOutButton.Size = new System.Drawing.Size(17, 20);
            this.zoomOutButton.TabIndex = 10;
            this.zoomOutButton.Text = "-";
            this.defaultToolTip.SetToolTip(this.zoomOutButton, "zoom out");
            this.zoomOutButton.UseVisualStyleBackColor = true;
            this.zoomOutButton.Click += new System.EventHandler(this.zoomOutButton_Click);
            // 
            // zoomInButton
            // 
            this.zoomInButton.Location = new System.Drawing.Point(145, 49);
            this.zoomInButton.Name = "zoomInButton";
            this.zoomInButton.Size = new System.Drawing.Size(14, 20);
            this.zoomInButton.TabIndex = 7;
            this.zoomInButton.Text = "+";
            this.defaultToolTip.SetToolTip(this.zoomInButton, "zoom in");
            this.zoomInButton.UseVisualStyleBackColor = true;
            this.zoomInButton.Click += new System.EventHandler(this.zoomInButton_Click);
            // 
            // showPAR
            // 
            this.showPAR.AutoSize = true;
            this.showPAR.Location = new System.Drawing.Point(415, 51);
            this.showPAR.Name = "showPAR";
            this.showPAR.Size = new System.Drawing.Size(88, 17);
            this.showPAR.TabIndex = 12;
            this.showPAR.Text = "Preview DAR";
            this.showPAR.UseVisualStyleBackColor = true;
            this.showPAR.CheckedChanged += new System.EventHandler(this.showPAR_CheckedChanged);
            // 
            // originalSizeButton
            // 
            this.originalSizeButton.Location = new System.Drawing.Point(159, 49);
            this.originalSizeButton.Name = "originalSizeButton";
            this.originalSizeButton.Size = new System.Drawing.Size(30, 20);
            this.originalSizeButton.TabIndex = 8;
            this.originalSizeButton.Text = "1:1";
            this.defaultToolTip.SetToolTip(this.originalSizeButton, "displays the original video size");
            this.originalSizeButton.Click += new System.EventHandler(this.originalSizeButton_Click);
            // 
            // introEndButton
            // 
            this.introEndButton.Location = new System.Drawing.Point(338, 75);
            this.introEndButton.Name = "introEndButton";
            this.introEndButton.Size = new System.Drawing.Size(38, 20);
            this.introEndButton.TabIndex = 18;
            this.introEndButton.Text = "Intro";
            this.defaultToolTip.SetToolTip(this.introEndButton, "Set the frame where the intro ends");
            this.introEndButton.Click += new System.EventHandler(this.introEndButton_Click);
            // 
            // zoneStartButton
            // 
            this.zoneStartButton.Location = new System.Drawing.Point(172, 75);
            this.zoneStartButton.Name = "zoneStartButton";
            this.zoneStartButton.Size = new System.Drawing.Size(64, 20);
            this.zoneStartButton.TabIndex = 15;
            this.zoneStartButton.Text = "Zone Start";
            this.defaultToolTip.SetToolTip(this.zoneStartButton, "Sets the start frame of a new zone");
            this.zoneStartButton.Click += new System.EventHandler(this.zoneStartButton_Click);
            // 
            // setZoneButton
            // 
            this.setZoneButton.Location = new System.Drawing.Point(306, 75);
            this.setZoneButton.Name = "setZoneButton";
            this.setZoneButton.Size = new System.Drawing.Size(30, 20);
            this.setZoneButton.TabIndex = 17;
            this.setZoneButton.Text = "Set";
            this.defaultToolTip.SetToolTip(this.setZoneButton, "Adds the zone to the codec configuration");
            this.setZoneButton.Click += new System.EventHandler(this.setZoneButton_Click);
            // 
            // zoneEndButton
            // 
            this.zoneEndButton.Location = new System.Drawing.Point(239, 75);
            this.zoneEndButton.Name = "zoneEndButton";
            this.zoneEndButton.Size = new System.Drawing.Size(64, 20);
            this.zoneEndButton.TabIndex = 16;
            this.zoneEndButton.Text = "Zone End";
            this.defaultToolTip.SetToolTip(this.zoneEndButton, "Sets the end frame of a new zone");
            this.zoneEndButton.Click += new System.EventHandler(this.zoneEndButton_Click);
            // 
            // chapterButton
            // 
            this.chapterButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.chapterButton.Location = new System.Drawing.Point(338, 75);
            this.chapterButton.Name = "chapterButton";
            this.chapterButton.Size = new System.Drawing.Size(72, 20);
            this.chapterButton.TabIndex = 19;
            this.chapterButton.Text = "Set Chapter";
            this.defaultToolTip.SetToolTip(this.chapterButton, "Sets the end frame of a new zone");
            this.chapterButton.Click += new System.EventHandler(this.chapterButton_Click);
            // 
            // videoPanel
            // 
            this.videoPanel.AutoScroll = true;
            this.videoPanel.Controls.Add(this.videoPreview);
            this.videoPanel.Location = new System.Drawing.Point(0, 0);
            this.videoPanel.Margin = new System.Windows.Forms.Padding(0);
            this.videoPanel.Name = "videoPanel";
            this.videoPanel.Size = new System.Drawing.Size(399, 225);
            this.videoPanel.TabIndex = 11;
            // 
            // videoPreview
            // 
            this.videoPreview.CropMargin = new System.Windows.Forms.Padding(0);
            this.videoPreview.DisplayActualFramerate = false;
            this.videoPreview.EnsureCorrectPlaybackSpeed = false;
            this.videoPreview.Framerate = 25D;
            this.videoPreview.Location = new System.Drawing.Point(1, 1);
            this.videoPreview.Margin = new System.Windows.Forms.Padding(0);
            this.videoPreview.Name = "videoPreview";
            this.videoPreview.Position = -1;
            this.videoPreview.Size = new System.Drawing.Size(274, 164);
            this.videoPreview.SpeedUp = 1D;
            this.videoPreview.TabIndex = 11;
            this.videoPreview.PositionChanged += new System.EventHandler(this.videoPreview_PositionChanged);
            // 
            // arChooser
            // 
            this.arChooser.CustomDARs = new MeGUI.core.util.Dar[0];
            this.arChooser.HasLater = false;
            this.arChooser.Location = new System.Drawing.Point(241, 45);
            this.arChooser.MaximumSize = new System.Drawing.Size(1000, 29);
            this.arChooser.MinimumSize = new System.Drawing.Size(64, 29);
            this.arChooser.Name = "arChooser";
            this.arChooser.SelectedIndex = 0;
            this.arChooser.Size = new System.Drawing.Size(170, 29);
            this.arChooser.TabIndex = 11;
            this.arChooser.SelectionChanged += new MeGUI.StringChanged(this.arChooser_SelectionChanged);
            // 
            // VideoPlayer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(518, 352);
            this.Controls.Add(this.videoPanel);
            this.Controls.Add(this.buttonPanel);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "VideoPlayer";
            this.Text = "VideoPlayer";
            this.Shown += new System.EventHandler(this.VideoPlayer_Shown);
            this.buttonPanel.ResumeLayout(false);
            this.buttonPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.positionSlider)).EndInit();
            this.videoPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        private System.Windows.Forms.Button playButton;
        private System.Windows.Forms.Button nextFrameButton;
        private System.Windows.Forms.Button previousFrameButton;
        private System.Windows.Forms.Button ffButton;
        private System.Windows.Forms.Button fwdButton;
        private System.Windows.Forms.Button creditsStartButton;
        private System.Windows.Forms.Panel buttonPanel;
        private System.Windows.Forms.Button zoneStartButton;
        private System.Windows.Forms.Button zoneEndButton;
        private System.Windows.Forms.Button setZoneButton;
        private System.Windows.Forms.ContextMenu contextMenu1;
        private System.Windows.Forms.MenuItem mnuZoneStart;
        private System.Windows.Forms.MenuItem mnuZoneEnd;
        private System.Windows.Forms.MenuItem mnuIntroEnd;
        private System.Windows.Forms.MenuItem mnuCreditsStart;
        private System.Windows.Forms.Button introEndButton;
        private System.Windows.Forms.ToolTip defaultToolTip;
        private System.Windows.Forms.Button chapterButton;
        private Button originalSizeButton;
        private CheckBox showPAR;
        private ARChooser arChooser;
        private Button zoomInButton;
        private Button zoomOutButton;
        private Button btnReloadVideo;
        private Button btnFitScreen;
        private TrackBar positionSlider;
        private Panel videoPanel;
        private VideoPlayerControl videoPreview;

    }
}
