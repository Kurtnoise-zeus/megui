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

namespace MeGUI
{
    partial class baseMuxWindow
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
            this.muxButton = new System.Windows.Forms.Button();
            this.videoGroupbox = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.videoInputLabel = new System.Windows.Forms.Label();
            this.videoName = new System.Windows.Forms.TextBox();
            this.fps = new MeGUI.core.gui.FPSChooser();
            this.videoNameLabel = new System.Windows.Forms.Label();
            this.vInput = new MeGUI.FileBar();
            this.MuxFPSLabel = new System.Windows.Forms.Label();
            this.removeVideoTrack = new System.Windows.Forms.Button();
            this.chaptersGroupbox = new System.Windows.Forms.GroupBox();
            this.deleteChapterFile = new System.Windows.Forms.Button();
            this.chaptersInputLabel = new System.Windows.Forms.Label();
            this.chapters = new MeGUI.FileBar();
            this.outputGroupbox = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.lbContainer = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.muxedOutputLabel = new System.Windows.Forms.Label();
            this.splitting = new MeGUI.core.gui.TargetSizeSCBox();
            this.splittingLabel = new System.Windows.Forms.Label();
            this.output = new MeGUI.FileBar();
            this.cbType = new System.Windows.Forms.ComboBox();
            this.cbContainer = new System.Windows.Forms.ComboBox();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.audioPanel = new System.Windows.Forms.Panel();
            this.audioMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.audioAddTrack = new System.Windows.Forms.ToolStripMenuItem();
            this.audioRemoveTrack = new System.Windows.Forms.ToolStripMenuItem();
            this.audio = new System.Windows.Forms.TabControl();
            this.audioPage1 = new System.Windows.Forms.TabPage();
            this.muxStreamControl2 = new MeGUI.core.details.mux.MuxStreamControl();
            this.subtitleMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.subtitleAddTrack = new System.Windows.Forms.ToolStripMenuItem();
            this.subtitleRemoveTrack = new System.Windows.Forms.ToolStripMenuItem();
            this.subtitlePanel = new System.Windows.Forms.Panel();
            this.subtitles = new System.Windows.Forms.TabControl();
            this.subPage1 = new System.Windows.Forms.TabPage();
            this.muxStreamControl1 = new MeGUI.core.details.mux.MuxStreamControl();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.helpButton1 = new MeGUI.core.gui.HelpButton();
            this.chkCloseOnQueue = new System.Windows.Forms.CheckBox();
            this.videoGroupbox.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.chaptersGroupbox.SuspendLayout();
            this.outputGroupbox.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.audioPanel.SuspendLayout();
            this.audioMenu.SuspendLayout();
            this.audio.SuspendLayout();
            this.audioPage1.SuspendLayout();
            this.subtitleMenu.SuspendLayout();
            this.subtitlePanel.SuspendLayout();
            this.subtitles.SuspendLayout();
            this.subPage1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // muxButton
            // 
            this.muxButton.Location = new System.Drawing.Point(376, 565);
            this.muxButton.Margin = new System.Windows.Forms.Padding(12, 9, 12, 9);
            this.muxButton.Name = "muxButton";
            this.muxButton.Size = new System.Drawing.Size(56, 23);
            this.muxButton.TabIndex = 5;
            this.muxButton.Text = "&Queue";
            // 
            // videoGroupbox
            // 
            this.videoGroupbox.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.videoGroupbox, 4);
            this.videoGroupbox.Controls.Add(this.tableLayoutPanel2);
            this.videoGroupbox.Location = new System.Drawing.Point(3, 3);
            this.videoGroupbox.Name = "videoGroupbox";
            this.videoGroupbox.Size = new System.Drawing.Size(438, 90);
            this.videoGroupbox.TabIndex = 0;
            this.videoGroupbox.TabStop = false;
            this.videoGroupbox.Text = "Video";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.AutoSize = true;
            this.tableLayoutPanel2.ColumnCount = 5;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 48.99329F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 51.00671F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 44F));
            this.tableLayoutPanel2.Controls.Add(this.videoInputLabel, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.videoName, 3, 1);
            this.tableLayoutPanel2.Controls.Add(this.fps, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.videoNameLabel, 2, 1);
            this.tableLayoutPanel2.Controls.Add(this.vInput, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.MuxFPSLabel, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.removeVideoTrack, 4, 1);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 17);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(432, 70);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // videoInputLabel
            // 
            this.videoInputLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.videoInputLabel.AutoSize = true;
            this.videoInputLabel.Location = new System.Drawing.Point(3, 11);
            this.videoInputLabel.Name = "videoInputLabel";
            this.videoInputLabel.Size = new System.Drawing.Size(62, 13);
            this.videoInputLabel.TabIndex = 0;
            this.videoInputLabel.Text = "Video Input";
            // 
            // videoName
            // 
            this.videoName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.videoName.Location = new System.Drawing.Point(245, 42);
            this.videoName.MaxLength = 100;
            this.videoName.Name = "videoName";
            this.videoName.Size = new System.Drawing.Size(139, 21);
            this.videoName.TabIndex = 5;
            // 
            // fps
            // 
            this.fps.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.fps.Location = new System.Drawing.Point(71, 38);
            this.fps.MaximumSize = new System.Drawing.Size(1000, 29);
            this.fps.MinimumSize = new System.Drawing.Size(64, 29);
            this.fps.Name = "fps";
            this.fps.NullString = "Not set";
            this.fps.SelectedIndex = 0;
            this.fps.Size = new System.Drawing.Size(134, 29);
            this.fps.TabIndex = 3;
            this.fps.SelectionChanged += new MeGUI.StringChanged(this.fps_SelectionChanged);
            // 
            // videoNameLabel
            // 
            this.videoNameLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.videoNameLabel.AutoSize = true;
            this.videoNameLabel.Location = new System.Drawing.Point(208, 46);
            this.videoNameLabel.Margin = new System.Windows.Forms.Padding(0);
            this.videoNameLabel.Name = "videoNameLabel";
            this.videoNameLabel.Size = new System.Drawing.Size(34, 13);
            this.videoNameLabel.TabIndex = 4;
            this.videoNameLabel.Text = "Name";
            // 
            // vInput
            // 
            this.vInput.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel2.SetColumnSpan(this.vInput, 4);
            this.vInput.Filename = "";
            this.vInput.Location = new System.Drawing.Point(71, 6);
            this.vInput.Margin = new System.Windows.Forms.Padding(3, 3, 6, 3);
            this.vInput.Name = "vInput";
            this.vInput.Size = new System.Drawing.Size(355, 23);
            this.vInput.TabIndex = 1;
            this.vInput.FileSelected += new MeGUI.FileBarEventHandler(this.vInput_FileSelected);
            // 
            // MuxFPSLabel
            // 
            this.MuxFPSLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.MuxFPSLabel.AutoSize = true;
            this.MuxFPSLabel.Location = new System.Drawing.Point(3, 46);
            this.MuxFPSLabel.Name = "MuxFPSLabel";
            this.MuxFPSLabel.Size = new System.Drawing.Size(25, 13);
            this.MuxFPSLabel.TabIndex = 2;
            this.MuxFPSLabel.Text = "FPS";
            // 
            // removeVideoTrack
            // 
            this.removeVideoTrack.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.removeVideoTrack.Location = new System.Drawing.Point(397, 41);
            this.removeVideoTrack.Name = "removeVideoTrack";
            this.removeVideoTrack.Size = new System.Drawing.Size(24, 23);
            this.removeVideoTrack.TabIndex = 39;
            this.removeVideoTrack.Text = "X";
            this.removeVideoTrack.Click += new System.EventHandler(this.removeVideoTrack_Click);
            // 
            // chaptersGroupbox
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.chaptersGroupbox, 4);
            this.chaptersGroupbox.Controls.Add(this.deleteChapterFile);
            this.chaptersGroupbox.Controls.Add(this.chaptersInputLabel);
            this.chaptersGroupbox.Controls.Add(this.chapters);
            this.chaptersGroupbox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chaptersGroupbox.Location = new System.Drawing.Point(3, 361);
            this.chaptersGroupbox.Name = "chaptersGroupbox";
            this.chaptersGroupbox.Size = new System.Drawing.Size(438, 48);
            this.chaptersGroupbox.TabIndex = 3;
            this.chaptersGroupbox.TabStop = false;
            this.chaptersGroupbox.Text = "Chapter";
            // 
            // deleteChapterFile
            // 
            this.deleteChapterFile.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.deleteChapterFile.Location = new System.Drawing.Point(410, 17);
            this.deleteChapterFile.Name = "deleteChapterFile";
            this.deleteChapterFile.Size = new System.Drawing.Size(24, 23);
            this.deleteChapterFile.TabIndex = 40;
            this.deleteChapterFile.Text = "X";
            this.deleteChapterFile.Click += new System.EventHandler(this.deleteChapterFile_Click);
            // 
            // chaptersInputLabel
            // 
            this.chaptersInputLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.chaptersInputLabel.AutoSize = true;
            this.chaptersInputLabel.Location = new System.Drawing.Point(6, 22);
            this.chaptersInputLabel.Name = "chaptersInputLabel";
            this.chaptersInputLabel.Size = new System.Drawing.Size(70, 13);
            this.chaptersInputLabel.TabIndex = 4;
            this.chaptersInputLabel.Text = "Chapters File";
            // 
            // chapters
            // 
            this.chapters.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.chapters.Filename = "";
            this.chapters.Filter = "";
            this.chapters.Location = new System.Drawing.Point(88, 17);
            this.chapters.Name = "chapters";
            this.chapters.Size = new System.Drawing.Size(316, 23);
            this.chapters.TabIndex = 3;
            this.chapters.FileSelected += new MeGUI.FileBarEventHandler(this.chapters_FileSelected);
            // 
            // outputGroupbox
            // 
            this.outputGroupbox.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.outputGroupbox, 4);
            this.outputGroupbox.Controls.Add(this.tableLayoutPanel4);
            this.outputGroupbox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.outputGroupbox.Location = new System.Drawing.Point(3, 415);
            this.outputGroupbox.Name = "outputGroupbox";
            this.outputGroupbox.Size = new System.Drawing.Size(438, 138);
            this.outputGroupbox.TabIndex = 4;
            this.outputGroupbox.TabStop = false;
            this.outputGroupbox.Text = "Output";
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.AutoSize = true;
            this.tableLayoutPanel4.ColumnCount = 2;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Controls.Add(this.lbContainer, 0, 4);
            this.tableLayoutPanel4.Controls.Add(this.label1, 0, 2);
            this.tableLayoutPanel4.Controls.Add(this.muxedOutputLabel, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.splitting, 1, 1);
            this.tableLayoutPanel4.Controls.Add(this.splittingLabel, 0, 1);
            this.tableLayoutPanel4.Controls.Add(this.output, 1, 0);
            this.tableLayoutPanel4.Controls.Add(this.cbType, 1, 2);
            this.tableLayoutPanel4.Controls.Add(this.cbContainer, 1, 4);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(3, 17);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 5;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.Size = new System.Drawing.Size(432, 118);
            this.tableLayoutPanel4.TabIndex = 0;
            // 
            // lbContainer
            // 
            this.lbContainer.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lbContainer.AutoSize = true;
            this.lbContainer.Location = new System.Drawing.Point(3, 98);
            this.lbContainer.Name = "lbContainer";
            this.lbContainer.Size = new System.Drawing.Size(54, 13);
            this.lbContainer.TabIndex = 11;
            this.lbContainer.Text = "Container";
            this.lbContainer.Visible = false;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 71);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Device Type";
            // 
            // muxedOutputLabel
            // 
            this.muxedOutputLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.muxedOutputLabel.AutoSize = true;
            this.muxedOutputLabel.Location = new System.Drawing.Point(3, 8);
            this.muxedOutputLabel.Name = "muxedOutputLabel";
            this.muxedOutputLabel.Size = new System.Drawing.Size(76, 13);
            this.muxedOutputLabel.TabIndex = 0;
            this.muxedOutputLabel.Text = "Muxed Output";
            // 
            // splitting
            // 
            this.splitting.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.splitting.CustomSizes = new MeGUI.core.util.FileSize[0];
            this.splitting.Location = new System.Drawing.Point(85, 32);
            this.splitting.MaximumSize = new System.Drawing.Size(1000, 29);
            this.splitting.MinimumSize = new System.Drawing.Size(64, 29);
            this.splitting.Name = "splitting";
            this.splitting.NullString = "No splitting";
            this.splitting.SaveCustomValues = false;
            this.splitting.SelectedIndex = 0;
            this.splitting.Size = new System.Drawing.Size(344, 29);
            this.splitting.TabIndex = 3;
            // 
            // splittingLabel
            // 
            this.splittingLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.splittingLabel.AutoSize = true;
            this.splittingLabel.Location = new System.Drawing.Point(3, 40);
            this.splittingLabel.Name = "splittingLabel";
            this.splittingLabel.Size = new System.Drawing.Size(45, 13);
            this.splittingLabel.TabIndex = 2;
            this.splittingLabel.Text = "Splitting";
            // 
            // output
            // 
            this.output.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.output.Filename = "";
            this.output.Location = new System.Drawing.Point(85, 3);
            this.output.Name = "output";
            this.output.ReadOnly = false;
            this.output.SaveMode = true;
            this.output.Size = new System.Drawing.Size(344, 23);
            this.output.TabIndex = 1;
            this.output.Click += new System.EventHandler(this.output_Click);
            // 
            // cbType
            // 
            this.cbType.Dock = System.Windows.Forms.DockStyle.Left;
            this.cbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbType.FormattingEnabled = true;
            this.cbType.Location = new System.Drawing.Point(85, 67);
            this.cbType.Name = "cbType";
            this.cbType.Size = new System.Drawing.Size(121, 21);
            this.cbType.TabIndex = 7;
            this.cbType.SelectedIndexChanged += new System.EventHandler(this.cbType_SelectedIndexChanged);
            // 
            // cbContainer
            // 
            this.cbContainer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbContainer.FormattingEnabled = true;
            this.cbContainer.Location = new System.Drawing.Point(85, 94);
            this.cbContainer.Name = "cbContainer";
            this.cbContainer.Size = new System.Drawing.Size(121, 21);
            this.cbContainer.TabIndex = 10;
            this.cbContainer.Visible = false;
            this.cbContainer.SelectedIndexChanged += new System.EventHandler(this.cbContainer_SelectedIndexChanged);
            // 
            // audioPanel
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.audioPanel, 4);
            this.audioPanel.ContextMenuStrip = this.audioMenu;
            this.audioPanel.Controls.Add(this.audio);
            this.audioPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.audioPanel.Location = new System.Drawing.Point(3, 99);
            this.audioPanel.Name = "audioPanel";
            this.audioPanel.Size = new System.Drawing.Size(438, 125);
            this.audioPanel.TabIndex = 1;
            // 
            // audioMenu
            // 
            this.audioMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.audioAddTrack,
            this.audioRemoveTrack});
            this.audioMenu.Name = "audioMenu";
            this.audioMenu.Size = new System.Drawing.Size(147, 48);
            this.audioMenu.Opening += new System.ComponentModel.CancelEventHandler(this.audioMenu_Opening);
            // 
            // audioAddTrack
            // 
            this.audioAddTrack.Name = "audioAddTrack";
            this.audioAddTrack.Size = new System.Drawing.Size(146, 22);
            this.audioAddTrack.Text = "Add track";
            this.audioAddTrack.Click += new System.EventHandler(this.audioAddTrack_Click);
            // 
            // audioRemoveTrack
            // 
            this.audioRemoveTrack.Name = "audioRemoveTrack";
            this.audioRemoveTrack.Size = new System.Drawing.Size(146, 22);
            this.audioRemoveTrack.Text = "Remove track";
            this.audioRemoveTrack.Click += new System.EventHandler(this.audioRemoveTrack_Click);
            // 
            // audio
            // 
            this.audio.Controls.Add(this.audioPage1);
            this.audio.Dock = System.Windows.Forms.DockStyle.Fill;
            this.audio.Location = new System.Drawing.Point(0, 0);
            this.audio.Name = "audio";
            this.audio.SelectedIndex = 0;
            this.audio.Size = new System.Drawing.Size(438, 125);
            this.audio.TabIndex = 0;
            // 
            // audioPage1
            // 
            this.audioPage1.Controls.Add(this.muxStreamControl2);
            this.audioPage1.Location = new System.Drawing.Point(4, 22);
            this.audioPage1.Name = "audioPage1";
            this.audioPage1.Padding = new System.Windows.Forms.Padding(3);
            this.audioPage1.Size = new System.Drawing.Size(430, 99);
            this.audioPage1.TabIndex = 0;
            this.audioPage1.Text = "Audio 1";
            this.audioPage1.UseVisualStyleBackColor = true;
            // 
            // muxStreamControl2
            // 
            this.muxStreamControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.muxStreamControl2.Filter = null;
            this.muxStreamControl2.Location = new System.Drawing.Point(3, 3);
            this.muxStreamControl2.Name = "muxStreamControl2";
            this.muxStreamControl2.ShowDefaultSubtitleStream = false;
            this.muxStreamControl2.ShowDelay = true;
            this.muxStreamControl2.ShowForceSubtitleStream = false;
            this.muxStreamControl2.Size = new System.Drawing.Size(424, 93);
            this.muxStreamControl2.TabIndex = 0;
            this.muxStreamControl2.FileUpdated += new System.EventHandler(this.muxStreamControl2_FileUpdated);
            // 
            // subtitleMenu
            // 
            this.subtitleMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.subtitleAddTrack,
            this.subtitleRemoveTrack});
            this.subtitleMenu.Name = "subtitleMenu";
            this.subtitleMenu.Size = new System.Drawing.Size(147, 48);
            this.subtitleMenu.Opening += new System.ComponentModel.CancelEventHandler(this.subtitleMenu_Opening);
            // 
            // subtitleAddTrack
            // 
            this.subtitleAddTrack.Name = "subtitleAddTrack";
            this.subtitleAddTrack.Size = new System.Drawing.Size(146, 22);
            this.subtitleAddTrack.Text = "Add track";
            this.subtitleAddTrack.Click += new System.EventHandler(this.subtitleAddTrack_Click);
            // 
            // subtitleRemoveTrack
            // 
            this.subtitleRemoveTrack.Name = "subtitleRemoveTrack";
            this.subtitleRemoveTrack.Size = new System.Drawing.Size(146, 22);
            this.subtitleRemoveTrack.Text = "Remove track";
            this.subtitleRemoveTrack.Click += new System.EventHandler(this.subtitleRemoveTrack_Click);
            // 
            // subtitlePanel
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.subtitlePanel, 4);
            this.subtitlePanel.ContextMenuStrip = this.subtitleMenu;
            this.subtitlePanel.Controls.Add(this.subtitles);
            this.subtitlePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.subtitlePanel.Location = new System.Drawing.Point(3, 230);
            this.subtitlePanel.Name = "subtitlePanel";
            this.subtitlePanel.Size = new System.Drawing.Size(438, 125);
            this.subtitlePanel.TabIndex = 2;
            // 
            // subtitles
            // 
            this.subtitles.Controls.Add(this.subPage1);
            this.subtitles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.subtitles.Location = new System.Drawing.Point(0, 0);
            this.subtitles.Name = "subtitles";
            this.subtitles.SelectedIndex = 0;
            this.subtitles.Size = new System.Drawing.Size(438, 125);
            this.subtitles.TabIndex = 0;
            // 
            // subPage1
            // 
            this.subPage1.Controls.Add(this.muxStreamControl1);
            this.subPage1.Location = new System.Drawing.Point(4, 22);
            this.subPage1.Name = "subPage1";
            this.subPage1.Padding = new System.Windows.Forms.Padding(3);
            this.subPage1.Size = new System.Drawing.Size(430, 99);
            this.subPage1.TabIndex = 0;
            this.subPage1.Text = "Subtitle 1";
            this.subPage1.UseVisualStyleBackColor = true;
            // 
            // muxStreamControl1
            // 
            this.muxStreamControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.muxStreamControl1.Filter = null;
            this.muxStreamControl1.Location = new System.Drawing.Point(3, 3);
            this.muxStreamControl1.Name = "muxStreamControl1";
            this.muxStreamControl1.ShowDefaultSubtitleStream = false;
            this.muxStreamControl1.ShowDelay = false;
            this.muxStreamControl1.ShowForceSubtitleStream = false;
            this.muxStreamControl1.Size = new System.Drawing.Size(424, 93);
            this.muxStreamControl1.TabIndex = 0;
            this.muxStreamControl1.FileUpdated += new System.EventHandler(this.muxStreamControl1_FileUpdated);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.helpButton1, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.audioPanel, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.subtitlePanel, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.chaptersGroupbox, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.outputGroupbox, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.videoGroupbox, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.chkCloseOnQueue, 2, 5);
            this.tableLayoutPanel1.Controls.Add(this.muxButton, 3, 5);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 6;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(444, 597);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // helpButton1
            // 
            this.helpButton1.ArticleName = "Manual mux window";
            this.helpButton1.AutoSize = true;
            this.helpButton1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.helpButton1.Location = new System.Drawing.Point(12, 565);
            this.helpButton1.Margin = new System.Windows.Forms.Padding(12, 9, 12, 9);
            this.helpButton1.Name = "helpButton1";
            this.helpButton1.Size = new System.Drawing.Size(38, 23);
            this.helpButton1.TabIndex = 8;
            // 
            // chkCloseOnQueue
            // 
            this.chkCloseOnQueue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.chkCloseOnQueue.AutoSize = true;
            this.chkCloseOnQueue.Checked = true;
            this.chkCloseOnQueue.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkCloseOnQueue.Location = new System.Drawing.Point(281, 565);
            this.chkCloseOnQueue.Margin = new System.Windows.Forms.Padding(12, 9, 12, 9);
            this.chkCloseOnQueue.Name = "chkCloseOnQueue";
            this.chkCloseOnQueue.Size = new System.Drawing.Size(71, 23);
            this.chkCloseOnQueue.TabIndex = 9;
            this.chkCloseOnQueue.Text = "and close";
            this.chkCloseOnQueue.UseVisualStyleBackColor = true;
            // 
            // baseMuxWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(444, 597);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "baseMuxWindow";
            this.Text = "Mux";
            this.videoGroupbox.ResumeLayout(false);
            this.videoGroupbox.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.chaptersGroupbox.ResumeLayout(false);
            this.chaptersGroupbox.PerformLayout();
            this.outputGroupbox.ResumeLayout(false);
            this.outputGroupbox.PerformLayout();
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.audioPanel.ResumeLayout(false);
            this.audioMenu.ResumeLayout(false);
            this.audio.ResumeLayout(false);
            this.audioPage1.ResumeLayout(false);
            this.subtitleMenu.ResumeLayout(false);
            this.subtitlePanel.ResumeLayout(false);
            this.subtitles.ResumeLayout(false);
            this.subPage1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion
        protected FileBar output;
        protected FileBar vInput;
        protected MeGUI.core.gui.FPSChooser fps;
        protected MeGUI.core.gui.TargetSizeSCBox splitting;
        protected System.Windows.Forms.TextBox videoName;
        protected System.Windows.Forms.TabControl subtitles;
        protected System.Windows.Forms.GroupBox videoGroupbox;
        protected System.Windows.Forms.Button muxButton;
        protected System.Windows.Forms.Label label1;
        protected System.Windows.Forms.Label lbContainer;
        protected System.Windows.Forms.CheckBox chkCloseOnQueue;
        protected System.Windows.Forms.GroupBox chaptersGroupbox;
        protected System.Windows.Forms.ComboBox cbType;
        protected System.Windows.Forms.ComboBox cbContainer;
        protected System.Windows.Forms.TabControl audio;
        private MeGUI.core.details.mux.MuxStreamControl muxStreamControl1;
        private MeGUI.core.details.mux.MuxStreamControl muxStreamControl2;
        private MeGUI.core.gui.HelpButton helpButton1;
        private System.Windows.Forms.Panel audioPanel;
        private System.Windows.Forms.TabPage audioPage1;
        private System.Windows.Forms.ContextMenuStrip audioMenu;
        private System.Windows.Forms.ContextMenuStrip subtitleMenu;
        private System.Windows.Forms.ToolStripMenuItem audioAddTrack;
        private System.Windows.Forms.ToolStripMenuItem audioRemoveTrack;
        private System.Windows.Forms.ToolStripMenuItem subtitleAddTrack;
        private System.Windows.Forms.ToolStripMenuItem subtitleRemoveTrack;
        private System.Windows.Forms.Panel subtitlePanel;
        private System.Windows.Forms.TabPage subPage1;
        private System.Windows.Forms.Label splittingLabel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.Button removeVideoTrack;
        private System.Windows.Forms.Label videoInputLabel;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.Label MuxFPSLabel;
        private System.Windows.Forms.Label muxedOutputLabel;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.GroupBox outputGroupbox;
        private System.Windows.Forms.Label videoNameLabel;
        private System.Windows.Forms.Button deleteChapterFile;
        private System.Windows.Forms.Label chaptersInputLabel;
        protected FileBar chapters;
    }
}