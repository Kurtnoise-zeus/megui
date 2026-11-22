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
            if (disposing)
            {
                ThemeManager.ThemeChanged -= OnThemeChanged;
                DisposeCompare();
            }
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VideoPlayer));
            this.contextMenu1 = new System.Windows.Forms.ContextMenu();
            this.mnuIntroEnd = new System.Windows.Forms.MenuItem();
            this.mnuCreditsStart = new System.Windows.Forms.MenuItem();
            this.mnuZoneStart = new System.Windows.Forms.MenuItem();
            this.mnuZoneEnd = new System.Windows.Forms.MenuItem();
            this.playButton = new MeGUI.core.gui.PlayerButton();
            this.nextFrameButton = new MeGUI.core.gui.PlayerButton();
            this.previousFrameButton = new MeGUI.core.gui.PlayerButton();
            this.ffButton = new MeGUI.core.gui.PlayerButton();
            this.fwdButton = new MeGUI.core.gui.PlayerButton();
            this.compareButton = new MeGUI.core.gui.PlayerButton();
            this.compareModeButton = new MeGUI.core.gui.PlayerButton();
            this.moreButton = new MeGUI.core.gui.PlayerButton();
            this.buttonPanel = new System.Windows.Forms.Panel();
            this.lblCurrentTime = new System.Windows.Forms.Label();
            this.lblTotalTime = new System.Windows.Forms.Label();
            this.lblFrame = new System.Windows.Forms.Label();
            this.positionSlider = new MeGUI.core.gui.PlayerSeekBar();
            this.arChooser = new MeGUI.core.gui.ARChooser();
            this.showPAR = new System.Windows.Forms.CheckBox();
            this.moreMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.goToFrameButton = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.zoneStartButton = new System.Windows.Forms.ToolStripMenuItem();
            this.zoneEndButton = new System.Windows.Forms.ToolStripMenuItem();
            this.setZoneButton = new System.Windows.Forms.ToolStripMenuItem();
            this.introEndButton = new System.Windows.Forms.ToolStripMenuItem();
            this.creditsStartButton = new System.Windows.Forms.ToolStripMenuItem();
            this.chapterButton = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.btnFitScreen = new System.Windows.Forms.ToolStripMenuItem();
            this.originalSizeButton = new System.Windows.Forms.ToolStripMenuItem();
            this.zoomInButton = new System.Windows.Forms.ToolStripMenuItem();
            this.zoomOutButton = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.btnReloadVideo = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.themeMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.themeSystemItem = new System.Windows.Forms.ToolStripMenuItem();
            this.themeDarkItem = new System.Windows.Forms.ToolStripMenuItem();
            this.themeLightItem = new System.Windows.Forms.ToolStripMenuItem();
            this.defaultToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.videoPanel = new System.Windows.Forms.Panel();
            this.videoPreview = new MeGUI.core.gui.VideoPlayerControl();
            this.buttonPanel.SuspendLayout();
            this.moreMenu.SuspendLayout();
            this.videoPanel.SuspendLayout();
            this.SuspendLayout();
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
            this.playButton.Glyph = MeGUI.core.gui.PlayerGlyph.Play;
            this.playButton.Location = new System.Drawing.Point(300, 44);
            this.playButton.Name = "playButton";
            this.playButton.Primary = true;
            this.playButton.Size = new System.Drawing.Size(52, 52);
            this.playButton.TabIndex = 4;
            this.defaultToolTip.SetToolTip(this.playButton, "Play / stop");
            this.playButton.Click += new System.EventHandler(this.playButton_Click);
            // 
            // nextFrameButton
            // 
            this.nextFrameButton.Glyph = MeGUI.core.gui.PlayerGlyph.NextFrame;
            this.nextFrameButton.Location = new System.Drawing.Point(356, 48);
            this.nextFrameButton.Name = "nextFrameButton";
            this.nextFrameButton.Size = new System.Drawing.Size(44, 44);
            this.nextFrameButton.TabIndex = 6;
            this.defaultToolTip.SetToolTip(this.nextFrameButton, "Advance by 1 frame");
            this.nextFrameButton.Click += new System.EventHandler(this.nextFrameButton_Click);
            this.nextFrameButton.KeyDown += new System.Windows.Forms.KeyEventHandler(this.nextFrameButton_KeyDown);
            // 
            // previousFrameButton
            // 
            this.previousFrameButton.Glyph = MeGUI.core.gui.PlayerGlyph.PreviousFrame;
            this.previousFrameButton.Location = new System.Drawing.Point(250, 48);
            this.previousFrameButton.Name = "previousFrameButton";
            this.previousFrameButton.Size = new System.Drawing.Size(44, 44);
            this.previousFrameButton.TabIndex = 2;
            this.defaultToolTip.SetToolTip(this.previousFrameButton, "Go back 1 frame");
            this.previousFrameButton.Click += new System.EventHandler(this.previousFrameButton_Click);
            this.previousFrameButton.KeyDown += new System.Windows.Forms.KeyEventHandler(this.previousFrameButton_KeyDown);
            // 
            // ffButton
            // 
            this.ffButton.Glyph = MeGUI.core.gui.PlayerGlyph.Forward25;
            this.ffButton.Location = new System.Drawing.Point(406, 48);
            this.ffButton.Name = "ffButton";
            this.ffButton.Size = new System.Drawing.Size(44, 44);
            this.ffButton.TabIndex = 5;
            this.defaultToolTip.SetToolTip(this.ffButton, "Advance 25 frames");
            this.ffButton.Click += new System.EventHandler(this.ffButton_Click);
            // 
            // fwdButton
            // 
            this.fwdButton.Glyph = MeGUI.core.gui.PlayerGlyph.Back25;
            this.fwdButton.Location = new System.Drawing.Point(200, 48);
            this.fwdButton.Name = "fwdButton";
            this.fwdButton.Size = new System.Drawing.Size(44, 44);
            this.fwdButton.TabIndex = 3;
            this.defaultToolTip.SetToolTip(this.fwdButton, "Go back 25 frames");
            this.fwdButton.Click += new System.EventHandler(this.fwdButton_Click);
            // 
            // compareButton
            // 
            this.compareButton.Glyph = MeGUI.core.gui.PlayerGlyph.Compare;
            this.compareButton.Location = new System.Drawing.Point(480, 48);
            this.compareButton.Name = "compareButton";
            this.compareButton.Size = new System.Drawing.Size(104, 44);
            this.compareButton.TabIndex = 7;
            this.compareButton.Text = "Compare";
            this.compareButton.Visible = false;
            this.defaultToolTip.SetToolTip(this.compareButton, "Compare the source (input file) with the filtered video");
            this.compareButton.Click += new System.EventHandler(this.compareButton_Click);
            // 
            // compareModeButton
            // 
            this.compareModeButton.Location = new System.Drawing.Point(590, 48);
            this.compareModeButton.Name = "compareModeButton";
            this.compareModeButton.Size = new System.Drawing.Size(104, 44);
            this.compareModeButton.TabIndex = 8;
            this.compareModeButton.Text = "Split";
            this.compareModeButton.Visible = false;
            this.defaultToolTip.SetToolTip(this.compareModeButton, "Switch between split view and side by side");
            this.compareModeButton.Click += new System.EventHandler(this.compareModeButton_Click);
            // 
            // moreButton
            // 
            this.moreButton.Glyph = MeGUI.core.gui.PlayerGlyph.More;
            this.moreButton.Location = new System.Drawing.Point(700, 48);
            this.moreButton.Name = "moreButton";
            this.moreButton.Size = new System.Drawing.Size(44, 44);
            this.moreButton.TabIndex = 9;
            this.defaultToolTip.SetToolTip(this.moreButton, "Zone, chapters, zoom, reload, theme...");
            this.moreButton.Click += new System.EventHandler(this.moreButton_Click);
            // 
            // buttonPanel
            // 
            this.buttonPanel.Controls.Add(this.lblCurrentTime);
            this.buttonPanel.Controls.Add(this.positionSlider);
            this.buttonPanel.Controls.Add(this.lblTotalTime);
            this.buttonPanel.Controls.Add(this.lblFrame);
            this.buttonPanel.Controls.Add(this.arChooser);
            this.buttonPanel.Controls.Add(this.showPAR);
            this.buttonPanel.Controls.Add(this.fwdButton);
            this.buttonPanel.Controls.Add(this.previousFrameButton);
            this.buttonPanel.Controls.Add(this.playButton);
            this.buttonPanel.Controls.Add(this.nextFrameButton);
            this.buttonPanel.Controls.Add(this.ffButton);
            this.buttonPanel.Controls.Add(this.compareButton);
            this.buttonPanel.Controls.Add(this.compareModeButton);
            this.buttonPanel.Controls.Add(this.moreButton);
            this.buttonPanel.Location = new System.Drawing.Point(0, 225);
            this.buttonPanel.Margin = new System.Windows.Forms.Padding(0);
            this.buttonPanel.Name = "buttonPanel";
            this.buttonPanel.Size = new System.Drawing.Size(760, 104);
            this.buttonPanel.TabIndex = 8;
            this.buttonPanel.Resize += new System.EventHandler(this.buttonPanel_Resize);
            // 
            // lblCurrentTime
            // 
            this.lblCurrentTime.Location = new System.Drawing.Point(12, 6);
            this.lblCurrentTime.Name = "lblCurrentTime";
            this.lblCurrentTime.Size = new System.Drawing.Size(84, 28);
            this.lblCurrentTime.TabIndex = 20;
            this.lblCurrentTime.Text = "00:00:00.000";
            this.lblCurrentTime.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // positionSlider
            // 
            this.positionSlider.Location = new System.Drawing.Point(100, 6);
            this.positionSlider.Margin = new System.Windows.Forms.Padding(0);
            this.positionSlider.Name = "positionSlider";
            this.positionSlider.Size = new System.Drawing.Size(560, 28);
            this.positionSlider.TabIndex = 1;
            this.positionSlider.Scroll += new System.EventHandler(this.positionSlider_Scroll);
            // 
            // lblTotalTime
            // 
            this.lblTotalTime.Location = new System.Drawing.Point(664, 6);
            this.lblTotalTime.Name = "lblTotalTime";
            this.lblTotalTime.Size = new System.Drawing.Size(84, 28);
            this.lblTotalTime.TabIndex = 21;
            this.lblTotalTime.Text = "00:00:00.000";
            this.lblTotalTime.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblFrame
            // 
            this.lblFrame.Location = new System.Drawing.Point(12, 60);
            this.lblFrame.Name = "lblFrame";
            this.lblFrame.Size = new System.Drawing.Size(130, 20);
            this.lblFrame.TabIndex = 22;
            this.lblFrame.Text = "Frame 0 / 0";
            this.lblFrame.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // arChooser
            // 
            this.arChooser.CustomDARs = new MeGUI.core.util.Dar[0];
            this.arChooser.HasLater = false;
            this.arChooser.Location = new System.Drawing.Point(12, 100);
            this.arChooser.MaximumSize = new System.Drawing.Size(1000, 29);
            this.arChooser.MinimumSize = new System.Drawing.Size(64, 29);
            this.arChooser.Name = "arChooser";
            this.arChooser.SelectedIndex = 0;
            this.arChooser.Size = new System.Drawing.Size(170, 29);
            this.arChooser.TabIndex = 11;
            this.arChooser.Visible = false;
            this.arChooser.SelectionChanged += new MeGUI.StringChanged(this.arChooser_SelectionChanged);
            // 
            // showPAR
            // 
            this.showPAR.AutoSize = true;
            this.showPAR.Location = new System.Drawing.Point(190, 106);
            this.showPAR.Name = "showPAR";
            this.showPAR.Size = new System.Drawing.Size(88, 17);
            this.showPAR.TabIndex = 12;
            this.showPAR.Text = "Preview DAR";
            this.showPAR.UseVisualStyleBackColor = true;
            this.showPAR.Visible = false;
            this.showPAR.CheckedChanged += new System.EventHandler(this.showPAR_CheckedChanged);
            // 
            // moreMenu
            // 
            this.moreMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.goToFrameButton,
            this.menuSeparator1,
            this.zoneStartButton,
            this.zoneEndButton,
            this.setZoneButton,
            this.introEndButton,
            this.creditsStartButton,
            this.chapterButton,
            this.menuSeparator2,
            this.btnFitScreen,
            this.originalSizeButton,
            this.zoomInButton,
            this.zoomOutButton,
            this.menuSeparator3,
            this.btnReloadVideo,
            this.menuSeparator4,
            this.themeMenuItem});
            this.moreMenu.Name = "moreMenu";
            this.moreMenu.Size = new System.Drawing.Size(200, 330);
            // 
            // goToFrameButton
            // 
            this.goToFrameButton.Name = "goToFrameButton";
            this.goToFrameButton.ShortcutKeyDisplayString = "Ctrl+G";
            this.goToFrameButton.Text = "Go to frame...";
            this.goToFrameButton.Click += new System.EventHandler(this.goToFrameButton_Click);
            // 
            // menuSeparator1
            // 
            this.menuSeparator1.Name = "menuSeparator1";
            // 
            // zoneStartButton
            // 
            this.zoneStartButton.Name = "zoneStartButton";
            this.zoneStartButton.Text = "Zone start";
            this.zoneStartButton.ToolTipText = "Sets the start frame of a new zone";
            this.zoneStartButton.Click += new System.EventHandler(this.zoneStartButton_Click);
            // 
            // zoneEndButton
            // 
            this.zoneEndButton.Name = "zoneEndButton";
            this.zoneEndButton.Text = "Zone end";
            this.zoneEndButton.ToolTipText = "Sets the end frame of a new zone";
            this.zoneEndButton.Click += new System.EventHandler(this.zoneEndButton_Click);
            // 
            // setZoneButton
            // 
            this.setZoneButton.Name = "setZoneButton";
            this.setZoneButton.Text = "Add zone";
            this.setZoneButton.ToolTipText = "Adds the zone to the codec configuration";
            this.setZoneButton.Click += new System.EventHandler(this.setZoneButton_Click);
            // 
            // introEndButton
            // 
            this.introEndButton.Name = "introEndButton";
            this.introEndButton.Text = "Set intro end";
            this.introEndButton.ToolTipText = "Set the frame where the intro ends";
            this.introEndButton.Click += new System.EventHandler(this.introEndButton_Click);
            // 
            // creditsStartButton
            // 
            this.creditsStartButton.Name = "creditsStartButton";
            this.creditsStartButton.Text = "Set credits start";
            this.creditsStartButton.ToolTipText = "Set the frame where the credits start";
            this.creditsStartButton.Click += new System.EventHandler(this.creditsStartButton_Click);
            // 
            // chapterButton
            // 
            this.chapterButton.Name = "chapterButton";
            this.chapterButton.Text = "Set chapter";
            this.chapterButton.ToolTipText = "Sets a chapter at the current frame";
            this.chapterButton.Click += new System.EventHandler(this.chapterButton_Click);
            // 
            // menuSeparator2
            // 
            this.menuSeparator2.Name = "menuSeparator2";
            // 
            // btnFitScreen
            // 
            this.btnFitScreen.Name = "btnFitScreen";
            this.btnFitScreen.Text = "Fit to screen";
            this.btnFitScreen.ToolTipText = "resets the zoom to the maximum screen size";
            this.btnFitScreen.Click += new System.EventHandler(this.btnFitScreen_Click);
            // 
            // originalSizeButton
            // 
            this.originalSizeButton.Name = "originalSizeButton";
            this.originalSizeButton.Text = "Original size (1:1)";
            this.originalSizeButton.ToolTipText = "displays the original video size";
            this.originalSizeButton.Click += new System.EventHandler(this.originalSizeButton_Click);
            // 
            // zoomInButton
            // 
            this.zoomInButton.Name = "zoomInButton";
            this.zoomInButton.Text = "Zoom in";
            this.zoomInButton.Click += new System.EventHandler(this.zoomInButton_Click);
            // 
            // zoomOutButton
            // 
            this.zoomOutButton.Name = "zoomOutButton";
            this.zoomOutButton.Text = "Zoom out";
            this.zoomOutButton.Click += new System.EventHandler(this.zoomOutButton_Click);
            // 
            // menuSeparator3
            // 
            this.menuSeparator3.Name = "menuSeparator3";
            // 
            // btnReloadVideo
            // 
            this.btnReloadVideo.Name = "btnReloadVideo";
            this.btnReloadVideo.Text = "Reload video";
            this.btnReloadVideo.ToolTipText = "Reload the video file";
            this.btnReloadVideo.Click += new System.EventHandler(this.btnReloadVideo_Click);
            // 
            // menuSeparator4
            // 
            this.menuSeparator4.Name = "menuSeparator4";
            // 
            // themeMenuItem
            // 
            this.themeMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.themeSystemItem,
            this.themeDarkItem,
            this.themeLightItem});
            this.themeMenuItem.Name = "themeMenuItem";
            this.themeMenuItem.Text = "Theme";
            // 
            // themeSystemItem
            // 
            this.themeSystemItem.Name = "themeSystemItem";
            this.themeSystemItem.Text = "System (follow Windows)";
            this.themeSystemItem.Click += new System.EventHandler(this.themeItem_Click);
            // 
            // themeDarkItem
            // 
            this.themeDarkItem.Name = "themeDarkItem";
            this.themeDarkItem.Text = "Dark";
            this.themeDarkItem.Click += new System.EventHandler(this.themeItem_Click);
            // 
            // themeLightItem
            // 
            this.themeLightItem.Name = "themeLightItem";
            this.themeLightItem.Text = "Light";
            this.themeLightItem.Click += new System.EventHandler(this.themeItem_Click);
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
            this.videoPreview.Position = 0;
            this.videoPreview.Size = new System.Drawing.Size(274, 164);
            this.videoPreview.SpeedUp = 1D;
            this.videoPreview.TabIndex = 11;
            this.videoPreview.PositionChanged += new System.EventHandler(this.videoPreview_PositionChanged);
            // 
            // VideoPlayer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(760, 329);
            this.Controls.Add(this.videoPanel);
            this.Controls.Add(this.buttonPanel);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "VideoPlayer";
            this.Text = "VideoPlayer";
            this.Shown += new System.EventHandler(this.VideoPlayer_Shown);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.VideoPlayer_KeyDown);
            this.buttonPanel.ResumeLayout(false);
            this.buttonPanel.PerformLayout();
            this.moreMenu.ResumeLayout(false);
            this.videoPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        private MeGUI.core.gui.PlayerButton playButton;
        private MeGUI.core.gui.PlayerButton nextFrameButton;
        private MeGUI.core.gui.PlayerButton previousFrameButton;
        private MeGUI.core.gui.PlayerButton ffButton;
        private MeGUI.core.gui.PlayerButton fwdButton;
        private MeGUI.core.gui.PlayerButton compareButton;
        private MeGUI.core.gui.PlayerButton compareModeButton;
        private MeGUI.core.gui.PlayerButton moreButton;
        private System.Windows.Forms.Panel buttonPanel;
        private System.Windows.Forms.ContextMenu contextMenu1;
        private System.Windows.Forms.MenuItem mnuZoneStart;
        private System.Windows.Forms.MenuItem mnuZoneEnd;
        private System.Windows.Forms.MenuItem mnuIntroEnd;
        private System.Windows.Forms.MenuItem mnuCreditsStart;
        private System.Windows.Forms.ToolTip defaultToolTip;
        private CheckBox showPAR;
        private ARChooser arChooser;
        private Label lblCurrentTime;
        private Label lblTotalTime;
        private Label lblFrame;
        private MeGUI.core.gui.PlayerSeekBar positionSlider;
        private ContextMenuStrip moreMenu;
        private ToolStripMenuItem goToFrameButton;
        private ToolStripSeparator menuSeparator1;
        private ToolStripMenuItem zoneStartButton;
        private ToolStripMenuItem zoneEndButton;
        private ToolStripMenuItem setZoneButton;
        private ToolStripMenuItem introEndButton;
        private ToolStripMenuItem creditsStartButton;
        private ToolStripMenuItem chapterButton;
        private ToolStripSeparator menuSeparator2;
        private ToolStripMenuItem btnFitScreen;
        private ToolStripMenuItem originalSizeButton;
        private ToolStripMenuItem zoomInButton;
        private ToolStripMenuItem zoomOutButton;
        private ToolStripSeparator menuSeparator3;
        private ToolStripMenuItem btnReloadVideo;
        private ToolStripSeparator menuSeparator4;
        private ToolStripMenuItem themeMenuItem;
        private ToolStripMenuItem themeSystemItem;
        private ToolStripMenuItem themeDarkItem;
        private ToolStripMenuItem themeLightItem;
        private Panel videoPanel;
        private VideoPlayerControl videoPreview;

    }
}
