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
    partial class OneClickWindow
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
            System.Windows.Forms.Label label1;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OneClickWindow));
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.outputTab = new System.Windows.Forms.TabControl();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.oneclickProfile = new MeGUI.core.gui.ConfigableProfilesControl();
            this.label3 = new System.Windows.Forms.Label();
            this.output = new MeGUI.FileBar();
            this.outputLabel = new System.Windows.Forms.Label();
            this.videoTab = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.input = new MeGUI.core.gui.FileSCBox();
            this.inputLabel = new System.Windows.Forms.Label();
            this.audioTab = new System.Windows.Forms.TabControl();
            this.audioPage0 = new System.Windows.Forms.TabPage();
            this.oneClickAudioStreamControl1 = new MeGUI.OneClickStreamControl();
            this.tabPage6 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.subtitlesTab = new System.Windows.Forms.TabControl();
            this.subPage0 = new System.Windows.Forms.TabPage();
            this.oneClickSubtitleStreamControl1 = new MeGUI.OneClickStreamControl();
            this.tabPage7 = new System.Windows.Forms.TabPage();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.subtitleMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.subtitleAddTrack = new System.Windows.Forms.ToolStripMenuItem();
            this.subtitleRemoveTrack = new System.Windows.Forms.ToolStripMenuItem();
            this.audioMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.audioAddTrack = new System.Windows.Forms.ToolStripMenuItem();
            this.audioRemoveTrack = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.encoderConfigTab = new System.Windows.Forms.TabPage();
            this.avsBox = new System.Windows.Forms.GroupBox();
            this.keepInputResolution = new System.Windows.Forms.CheckBox();
            this.autoCrop = new System.Windows.Forms.CheckBox();
            this.avsProfile = new MeGUI.core.gui.ConfigableProfilesControl();
            this.autoDeint = new System.Windows.Forms.CheckBox();
            this.outputResolutionLabel = new System.Windows.Forms.Label();
            this.horizontalResolution = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.locationGroupBox = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.inputName = new System.Windows.Forms.TextBox();
            this.deleteChapter = new System.Windows.Forms.Button();
            this.deleteWorking = new System.Windows.Forms.Button();
            this.chapterFile = new MeGUI.FileBar();
            this.workingDirectory = new MeGUI.FileBar();
            this.chapterLabel = new System.Windows.Forms.Label();
            this.workingDirectoryLabel = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.fileSize = new MeGUI.core.gui.TargetSizeSCBox();
            this.filesizeLabel = new System.Windows.Forms.Label();
            this.devicetype = new System.Windows.Forms.ComboBox();
            this.deviceLabel = new System.Windows.Forms.Label();
            this.containerFormatLabel = new System.Windows.Forms.Label();
            this.containerFormat = new System.Windows.Forms.ComboBox();
            this.splitting = new MeGUI.core.gui.TargetSizeSCBox();
            this.videoGroupBox = new System.Windows.Forms.GroupBox();
            this.ARLabel = new System.Windows.Forms.Label();
            this.ar = new MeGUI.core.gui.ARChooser();
            this.chkDontEncodeVideo = new System.Windows.Forms.CheckBox();
            this.usechaptersmarks = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.videoProfile = new MeGUI.core.gui.ConfigableProfilesControl();
            this.addPrerenderJob = new System.Windows.Forms.CheckBox();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.goButton = new System.Windows.Forms.Button();
            this.openOnQueue = new System.Windows.Forms.CheckBox();
            this.cbGUIMode = new System.Windows.Forms.ComboBox();
            this.helpButton1 = new MeGUI.core.gui.HelpButton();
            label1 = new System.Windows.Forms.Label();
            this.tabPage1.SuspendLayout();
            this.outputTab.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.videoTab.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.audioTab.SuspendLayout();
            this.audioPage0.SuspendLayout();
            this.subtitlesTab.SuspendLayout();
            this.subPage0.SuspendLayout();
            this.subtitleMenu.SuspendLayout();
            this.audioMenu.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.encoderConfigTab.SuspendLayout();
            this.avsBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.horizontalResolution)).BeginInit();
            this.locationGroupBox.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.videoGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(6, 47);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(45, 13);
            label1.TabIndex = 39;
            label1.Text = "Splitting";
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.outputTab);
            this.tabPage1.Controls.Add(this.videoTab);
            this.tabPage1.Controls.Add(this.audioTab);
            this.tabPage1.Controls.Add(this.subtitlesTab);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(464, 487);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "General";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // outputTab
            // 
            this.outputTab.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.outputTab.Controls.Add(this.tabPage4);
            this.outputTab.Location = new System.Drawing.Point(6, 379);
            this.outputTab.Name = "outputTab";
            this.outputTab.SelectedIndex = 0;
            this.outputTab.Size = new System.Drawing.Size(452, 95);
            this.outputTab.TabIndex = 22;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.oneclickProfile);
            this.tabPage4.Controls.Add(this.label3);
            this.tabPage4.Controls.Add(this.output);
            this.tabPage4.Controls.Add(this.outputLabel);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(444, 69);
            this.tabPage4.TabIndex = 0;
            this.tabPage4.Text = "Output";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // oneclickProfile
            // 
            this.oneclickProfile.Location = new System.Drawing.Point(99, 40);
            this.oneclickProfile.Name = "oneclickProfile";
            this.oneclickProfile.ProfileSet = "OneClick";
            this.oneclickProfile.Size = new System.Drawing.Size(333, 22);
            this.oneclickProfile.TabIndex = 31;
            this.oneclickProfile.UpdateSelectedProfile = true;
            this.oneclickProfile.SelectedProfileChanged += new System.EventHandler(this.OneClickProfileChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 44);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(81, 13);
            this.label3.TabIndex = 30;
            this.label3.Text = "OneClick profile";
            // 
            // output
            // 
            this.output.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.output.Filename = "";
            this.output.Filter = "MP4 Files|*.mp4";
            this.output.Location = new System.Drawing.Point(99, 10);
            this.output.Name = "output";
            this.output.SaveMode = true;
            this.output.Size = new System.Drawing.Size(333, 23);
            this.output.TabIndex = 29;
            // 
            // outputLabel
            // 
            this.outputLabel.Location = new System.Drawing.Point(3, 14);
            this.outputLabel.Name = "outputLabel";
            this.outputLabel.Size = new System.Drawing.Size(100, 13);
            this.outputLabel.TabIndex = 28;
            this.outputLabel.Text = "Output file";
            // 
            // videoTab
            // 
            this.videoTab.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.videoTab.Controls.Add(this.tabPage3);
            this.videoTab.Location = new System.Drawing.Point(6, 6);
            this.videoTab.Name = "videoTab";
            this.videoTab.SelectedIndex = 0;
            this.videoTab.Size = new System.Drawing.Size(452, 65);
            this.videoTab.TabIndex = 21;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.input);
            this.tabPage3.Controls.Add(this.inputLabel);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(444, 39);
            this.tabPage3.TabIndex = 0;
            this.tabPage3.Text = "Video";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // input
            // 
            this.input.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.input.Filter = "All files (*.*)|*.*";
            this.input.Location = new System.Drawing.Point(63, 6);
            this.input.MaximumSize = new System.Drawing.Size(1000, 29);
            this.input.MinimumSize = new System.Drawing.Size(64, 29);
            this.input.Name = "input";
            this.input.SelectedIndex = -1;
            this.input.SelectedItem = null;
            this.input.Size = new System.Drawing.Size(368, 29);
            this.input.TabIndex = 50;
            this.input.Type = MeGUI.core.gui.FileSCBox.FileSCBoxType.OC_FILE_AND_FOLDER;
            this.input.SelectionChanged += new MeGUI.StringChanged(this.input_SelectionChanged);
            // 
            // inputLabel
            // 
            this.inputLabel.AutoSize = true;
            this.inputLabel.Location = new System.Drawing.Point(8, 14);
            this.inputLabel.Name = "inputLabel";
            this.inputLabel.Size = new System.Drawing.Size(33, 13);
            this.inputLabel.TabIndex = 49;
            this.inputLabel.Text = "Input";
            // 
            // audioTab
            // 
            this.audioTab.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.audioTab.Controls.Add(this.audioPage0);
            this.audioTab.Controls.Add(this.tabPage6);
            this.audioTab.Controls.Add(this.tabPage2);
            this.audioTab.Location = new System.Drawing.Point(6, 77);
            this.audioTab.Name = "audioTab";
            this.audioTab.SelectedIndex = 0;
            this.audioTab.Size = new System.Drawing.Size(452, 175);
            this.audioTab.TabIndex = 20;
            this.audioTab.SelectedIndexChanged += new System.EventHandler(this.audioTab_SelectedIndexChanged);
            this.audioTab.MouseClick += new System.Windows.Forms.MouseEventHandler(this.audioTab_MouseClick);
            this.audioTab.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.audioTab_MouseDoubleClick);
            // 
            // audioPage0
            // 
            this.audioPage0.Controls.Add(this.oneClickAudioStreamControl1);
            this.audioPage0.Location = new System.Drawing.Point(4, 22);
            this.audioPage0.Name = "audioPage0";
            this.audioPage0.Padding = new System.Windows.Forms.Padding(3);
            this.audioPage0.Size = new System.Drawing.Size(444, 149);
            this.audioPage0.TabIndex = 2;
            this.audioPage0.Text = "Audio 1";
            this.audioPage0.UseVisualStyleBackColor = true;
            // 
            // oneClickAudioStreamControl1
            // 
            this.oneClickAudioStreamControl1.CustomStreams = new object[0];
            this.oneClickAudioStreamControl1.Filter = "All files (*.*)|*.*";
            this.oneClickAudioStreamControl1.Location = new System.Drawing.Point(0, 0);
            this.oneClickAudioStreamControl1.Name = "oneClickAudioStreamControl1";
            this.oneClickAudioStreamControl1.SelectedStreamIndex = 0;
            this.oneClickAudioStreamControl1.ShowDefaultStream = false;
            this.oneClickAudioStreamControl1.ShowDelay = true;
            this.oneClickAudioStreamControl1.ShowForceStream = false;
            this.oneClickAudioStreamControl1.Size = new System.Drawing.Size(434, 149);
            this.oneClickAudioStreamControl1.StandardStreams = new object[0];
            this.oneClickAudioStreamControl1.TabIndex = 0;
            this.oneClickAudioStreamControl1.TrackNumber = 1;
            this.oneClickAudioStreamControl1.FileUpdated += new System.EventHandler(this.oneClickAudioStreamControl_FileUpdated);
            // 
            // tabPage6
            // 
            this.tabPage6.Location = new System.Drawing.Point(4, 22);
            this.tabPage6.Name = "tabPage6";
            this.tabPage6.Size = new System.Drawing.Size(444, 149);
            this.tabPage6.TabIndex = 4;
            this.tabPage6.Text = "    -";
            this.tabPage6.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(444, 149);
            this.tabPage2.TabIndex = 3;
            this.tabPage2.Text = "   +";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // subtitlesTab
            // 
            this.subtitlesTab.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.subtitlesTab.Controls.Add(this.subPage0);
            this.subtitlesTab.Controls.Add(this.tabPage7);
            this.subtitlesTab.Controls.Add(this.tabPage5);
            this.subtitlesTab.Location = new System.Drawing.Point(6, 258);
            this.subtitlesTab.Name = "subtitlesTab";
            this.subtitlesTab.SelectedIndex = 0;
            this.subtitlesTab.Size = new System.Drawing.Size(452, 115);
            this.subtitlesTab.TabIndex = 19;
            this.subtitlesTab.SelectedIndexChanged += new System.EventHandler(this.subtitlesTab_SelectedIndexChanged);
            this.subtitlesTab.MouseClick += new System.Windows.Forms.MouseEventHandler(this.subtitlesTab_MouseClick);
            this.subtitlesTab.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.subtitlesTab_MouseDoubleClick);
            // 
            // subPage0
            // 
            this.subPage0.Controls.Add(this.oneClickSubtitleStreamControl1);
            this.subPage0.Location = new System.Drawing.Point(4, 22);
            this.subPage0.Name = "subPage0";
            this.subPage0.Padding = new System.Windows.Forms.Padding(3);
            this.subPage0.Size = new System.Drawing.Size(444, 89);
            this.subPage0.TabIndex = 2;
            this.subPage0.Text = "Subtitle 1";
            this.subPage0.UseVisualStyleBackColor = true;
            // 
            // oneClickSubtitleStreamControl1
            // 
            this.oneClickSubtitleStreamControl1.CustomStreams = new object[0];
            this.oneClickSubtitleStreamControl1.Filter = "All files (*.*)|*.*";
            this.oneClickSubtitleStreamControl1.Location = new System.Drawing.Point(0, 0);
            this.oneClickSubtitleStreamControl1.Name = "oneClickSubtitleStreamControl1";
            this.oneClickSubtitleStreamControl1.SelectedStreamIndex = 0;
            this.oneClickSubtitleStreamControl1.ShowDefaultStream = true;
            this.oneClickSubtitleStreamControl1.ShowDelay = true;
            this.oneClickSubtitleStreamControl1.ShowForceStream = true;
            this.oneClickSubtitleStreamControl1.Size = new System.Drawing.Size(434, 90);
            this.oneClickSubtitleStreamControl1.StandardStreams = new object[0];
            this.oneClickSubtitleStreamControl1.TabIndex = 0;
            this.oneClickSubtitleStreamControl1.TrackNumber = 1;
            this.oneClickSubtitleStreamControl1.FileUpdated += new System.EventHandler(this.oneClickSubtitleStreamControl_FileUpdated);
            // 
            // tabPage7
            // 
            this.tabPage7.Location = new System.Drawing.Point(4, 22);
            this.tabPage7.Name = "tabPage7";
            this.tabPage7.Size = new System.Drawing.Size(444, 89);
            this.tabPage7.TabIndex = 4;
            this.tabPage7.Text = "    -";
            this.tabPage7.UseVisualStyleBackColor = true;
            // 
            // tabPage5
            // 
            this.tabPage5.Location = new System.Drawing.Point(4, 22);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage5.Size = new System.Drawing.Size(444, 89);
            this.tabPage5.TabIndex = 3;
            this.tabPage5.Text = "   +";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // subtitleMenu
            // 
            this.subtitleMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.subtitleAddTrack,
            this.subtitleRemoveTrack});
            this.subtitleMenu.Name = "subtitleMenu";
            this.subtitleMenu.Size = new System.Drawing.Size(149, 48);
            // 
            // subtitleAddTrack
            // 
            this.subtitleAddTrack.Name = "subtitleAddTrack";
            this.subtitleAddTrack.Size = new System.Drawing.Size(148, 22);
            this.subtitleAddTrack.Text = "Add Track";
            this.subtitleAddTrack.Click += new System.EventHandler(this.subtitleAddTrack_Click);
            // 
            // subtitleRemoveTrack
            // 
            this.subtitleRemoveTrack.Name = "subtitleRemoveTrack";
            this.subtitleRemoveTrack.Size = new System.Drawing.Size(148, 22);
            this.subtitleRemoveTrack.Text = "Remove Track";
            this.subtitleRemoveTrack.Click += new System.EventHandler(this.subtitleRemoveTrack_Click);
            // 
            // audioMenu
            // 
            this.audioMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.audioAddTrack,
            this.audioRemoveTrack});
            this.audioMenu.Name = "audioMenu";
            this.audioMenu.Size = new System.Drawing.Size(149, 48);
            // 
            // audioAddTrack
            // 
            this.audioAddTrack.Name = "audioAddTrack";
            this.audioAddTrack.Size = new System.Drawing.Size(148, 22);
            this.audioAddTrack.Text = "Add Track";
            this.audioAddTrack.Click += new System.EventHandler(this.audioAddTrack_Click);
            // 
            // audioRemoveTrack
            // 
            this.audioRemoveTrack.Name = "audioRemoveTrack";
            this.audioRemoveTrack.Size = new System.Drawing.Size(148, 22);
            this.audioRemoveTrack.Text = "Remove Track";
            this.audioRemoveTrack.Click += new System.EventHandler(this.audioRemoveTrack_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.encoderConfigTab);
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(472, 513);
            this.tabControl1.TabIndex = 0;
            // 
            // encoderConfigTab
            // 
            this.encoderConfigTab.Controls.Add(this.avsBox);
            this.encoderConfigTab.Controls.Add(this.locationGroupBox);
            this.encoderConfigTab.Controls.Add(this.groupBox1);
            this.encoderConfigTab.Controls.Add(this.videoGroupBox);
            this.encoderConfigTab.Location = new System.Drawing.Point(4, 22);
            this.encoderConfigTab.Name = "encoderConfigTab";
            this.encoderConfigTab.Padding = new System.Windows.Forms.Padding(3);
            this.encoderConfigTab.Size = new System.Drawing.Size(464, 487);
            this.encoderConfigTab.TabIndex = 2;
            this.encoderConfigTab.Text = "Advanced Config";
            this.encoderConfigTab.UseVisualStyleBackColor = true;
            // 
            // avsBox
            // 
            this.avsBox.Controls.Add(this.keepInputResolution);
            this.avsBox.Controls.Add(this.autoCrop);
            this.avsBox.Controls.Add(this.avsProfile);
            this.avsBox.Controls.Add(this.autoDeint);
            this.avsBox.Controls.Add(this.outputResolutionLabel);
            this.avsBox.Controls.Add(this.horizontalResolution);
            this.avsBox.Controls.Add(this.label2);
            this.avsBox.Location = new System.Drawing.Point(6, 128);
            this.avsBox.Name = "avsBox";
            this.avsBox.Size = new System.Drawing.Size(452, 118);
            this.avsBox.TabIndex = 44;
            this.avsBox.TabStop = false;
            this.avsBox.Text = " AviSynth Settings ";
            // 
            // keepInputResolution
            // 
            this.keepInputResolution.AutoSize = true;
            this.keepInputResolution.Location = new System.Drawing.Point(120, 19);
            this.keepInputResolution.Name = "keepInputResolution";
            this.keepInputResolution.Size = new System.Drawing.Size(241, 17);
            this.keepInputResolution.TabIndex = 25;
            this.keepInputResolution.Text = "Keep Input Resolution (disable crop && resize)";
            this.keepInputResolution.UseVisualStyleBackColor = true;
            this.keepInputResolution.CheckedChanged += new System.EventHandler(this.keepInputResolution_CheckedChanged);
            // 
            // autoCrop
            // 
            this.autoCrop.AutoSize = true;
            this.autoCrop.Checked = true;
            this.autoCrop.CheckState = System.Windows.Forms.CheckState.Checked;
            this.autoCrop.Location = new System.Drawing.Point(190, 43);
            this.autoCrop.Name = "autoCrop";
            this.autoCrop.Size = new System.Drawing.Size(72, 17);
            this.autoCrop.TabIndex = 24;
            this.autoCrop.Text = "AutoCrop";
            this.autoCrop.UseVisualStyleBackColor = true;
            // 
            // avsProfile
            // 
            this.avsProfile.Location = new System.Drawing.Point(120, 69);
            this.avsProfile.Name = "avsProfile";
            this.avsProfile.ProfileSet = "AviSynth";
            this.avsProfile.Size = new System.Drawing.Size(326, 22);
            this.avsProfile.TabIndex = 23;
            this.avsProfile.UpdateSelectedProfile = false;
            // 
            // autoDeint
            // 
            this.autoDeint.AutoSize = true;
            this.autoDeint.Location = new System.Drawing.Point(120, 97);
            this.autoDeint.Name = "autoDeint";
            this.autoDeint.Size = new System.Drawing.Size(139, 17);
            this.autoDeint.TabIndex = 20;
            this.autoDeint.Text = "Automatic Deinterlacing";
            this.autoDeint.UseVisualStyleBackColor = true;
            // 
            // outputResolutionLabel
            // 
            this.outputResolutionLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.outputResolutionLabel.Location = new System.Drawing.Point(6, 36);
            this.outputResolutionLabel.Name = "outputResolutionLabel";
            this.outputResolutionLabel.Size = new System.Drawing.Size(100, 30);
            this.outputResolutionLabel.TabIndex = 3;
            this.outputResolutionLabel.Text = "Output Resolution       (Max. Width)";
            // 
            // horizontalResolution
            // 
            this.horizontalResolution.Increment = new decimal(new int[] {
            16,
            0,
            0,
            0});
            this.horizontalResolution.Location = new System.Drawing.Point(120, 42);
            this.horizontalResolution.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.horizontalResolution.Minimum = new decimal(new int[] {
            16,
            0,
            0,
            0});
            this.horizontalResolution.Name = "horizontalResolution";
            this.horizontalResolution.Size = new System.Drawing.Size(64, 21);
            this.horizontalResolution.TabIndex = 0;
            this.horizontalResolution.Value = new decimal(new int[] {
            640,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 73);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "AviSynth Profile";
            // 
            // locationGroupBox
            // 
            this.locationGroupBox.Controls.Add(this.label5);
            this.locationGroupBox.Controls.Add(this.inputName);
            this.locationGroupBox.Controls.Add(this.deleteChapter);
            this.locationGroupBox.Controls.Add(this.deleteWorking);
            this.locationGroupBox.Controls.Add(this.chapterFile);
            this.locationGroupBox.Controls.Add(this.workingDirectory);
            this.locationGroupBox.Controls.Add(this.chapterLabel);
            this.locationGroupBox.Controls.Add(this.workingDirectoryLabel);
            this.locationGroupBox.Location = new System.Drawing.Point(6, 359);
            this.locationGroupBox.Name = "locationGroupBox";
            this.locationGroupBox.Size = new System.Drawing.Size(452, 116);
            this.locationGroupBox.TabIndex = 43;
            this.locationGroupBox.TabStop = false;
            this.locationGroupBox.Text = " Extra IO ";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(7, 82);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(92, 13);
            this.label5.TabIndex = 53;
            this.label5.Text = "Video Track Name";
            // 
            // inputName
            // 
            this.inputName.Location = new System.Drawing.Point(119, 79);
            this.inputName.Name = "inputName";
            this.inputName.Size = new System.Drawing.Size(325, 21);
            this.inputName.TabIndex = 52;
            // 
            // deleteChapter
            // 
            this.deleteChapter.Location = new System.Drawing.Point(420, 48);
            this.deleteChapter.Name = "deleteChapter";
            this.deleteChapter.Size = new System.Drawing.Size(26, 23);
            this.deleteChapter.TabIndex = 41;
            this.deleteChapter.Text = "X";
            this.deleteChapter.UseVisualStyleBackColor = true;
            this.deleteChapter.Click += new System.EventHandler(this.deleteChapter_Click);
            // 
            // deleteWorking
            // 
            this.deleteWorking.Location = new System.Drawing.Point(420, 17);
            this.deleteWorking.Name = "deleteWorking";
            this.deleteWorking.Size = new System.Drawing.Size(26, 23);
            this.deleteWorking.TabIndex = 40;
            this.deleteWorking.Text = "X";
            this.deleteWorking.UseVisualStyleBackColor = true;
            this.deleteWorking.Click += new System.EventHandler(this.deleteWorking_Click);
            // 
            // chapterFile
            // 
            this.chapterFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chapterFile.Filename = "";
            this.chapterFile.Filter = "Chapter files (*.txt, *.xml)|*.txt;*.xml";
            this.chapterFile.Location = new System.Drawing.Point(120, 48);
            this.chapterFile.Name = "chapterFile";
            this.chapterFile.Size = new System.Drawing.Size(294, 23);
            this.chapterFile.TabIndex = 39;
            // 
            // workingDirectory
            // 
            this.workingDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.workingDirectory.Filename = "";
            this.workingDirectory.FolderMode = true;
            this.workingDirectory.Location = new System.Drawing.Point(120, 17);
            this.workingDirectory.Name = "workingDirectory";
            this.workingDirectory.Size = new System.Drawing.Size(294, 23);
            this.workingDirectory.TabIndex = 38;
            this.workingDirectory.FileSelected += new MeGUI.FileBarEventHandler(this.workingDirectory_FileSelected);
            // 
            // chapterLabel
            // 
            this.chapterLabel.Location = new System.Drawing.Point(6, 53);
            this.chapterLabel.Name = "chapterLabel";
            this.chapterLabel.Size = new System.Drawing.Size(100, 13);
            this.chapterLabel.TabIndex = 36;
            this.chapterLabel.Text = "Chapter File";
            // 
            // workingDirectoryLabel
            // 
            this.workingDirectoryLabel.Location = new System.Drawing.Point(6, 21);
            this.workingDirectoryLabel.Name = "workingDirectoryLabel";
            this.workingDirectoryLabel.Size = new System.Drawing.Size(100, 13);
            this.workingDirectoryLabel.TabIndex = 32;
            this.workingDirectoryLabel.Text = "Working Directory";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.fileSize);
            this.groupBox1.Controls.Add(this.filesizeLabel);
            this.groupBox1.Controls.Add(this.devicetype);
            this.groupBox1.Controls.Add(this.deviceLabel);
            this.groupBox1.Controls.Add(this.containerFormatLabel);
            this.groupBox1.Controls.Add(this.containerFormat);
            this.groupBox1.Controls.Add(label1);
            this.groupBox1.Controls.Add(this.splitting);
            this.groupBox1.Location = new System.Drawing.Point(6, 252);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(452, 100);
            this.groupBox1.TabIndex = 42;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = " Output Settings ";
            // 
            // fileSize
            // 
            this.fileSize.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fileSize.CustomSizes = new MeGUI.core.util.FileSize[0];
            this.fileSize.Location = new System.Drawing.Point(120, 13);
            this.fileSize.MaximumSize = new System.Drawing.Size(1000, 29);
            this.fileSize.MinimumSize = new System.Drawing.Size(64, 29);
            this.fileSize.Name = "fileSize";
            this.fileSize.NullString = "Don\'t Care";
            this.fileSize.SaveCustomValues = false;
            this.fileSize.SelectedIndex = 0;
            this.fileSize.Size = new System.Drawing.Size(326, 29);
            this.fileSize.TabIndex = 46;
            // 
            // filesizeLabel
            // 
            this.filesizeLabel.Location = new System.Drawing.Point(6, 21);
            this.filesizeLabel.Name = "filesizeLabel";
            this.filesizeLabel.Size = new System.Drawing.Size(90, 13);
            this.filesizeLabel.TabIndex = 45;
            this.filesizeLabel.Text = "Filesize";
            // 
            // devicetype
            // 
            this.devicetype.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.devicetype.FormattingEnabled = true;
            this.devicetype.Location = new System.Drawing.Point(349, 69);
            this.devicetype.Name = "devicetype";
            this.devicetype.Size = new System.Drawing.Size(95, 21);
            this.devicetype.TabIndex = 44;
            this.devicetype.SelectedIndexChanged += new System.EventHandler(this.updateChapterSelection);
            // 
            // deviceLabel
            // 
            this.deviceLabel.AutoSize = true;
            this.deviceLabel.Location = new System.Drawing.Point(279, 72);
            this.deviceLabel.Name = "deviceLabel";
            this.deviceLabel.Size = new System.Drawing.Size(66, 13);
            this.deviceLabel.TabIndex = 43;
            this.deviceLabel.Text = "Device Type";
            // 
            // containerFormatLabel
            // 
            this.containerFormatLabel.AutoSize = true;
            this.containerFormatLabel.Location = new System.Drawing.Point(6, 72);
            this.containerFormatLabel.Name = "containerFormatLabel";
            this.containerFormatLabel.Size = new System.Drawing.Size(91, 13);
            this.containerFormatLabel.TabIndex = 42;
            this.containerFormatLabel.Text = "Container Format";
            // 
            // containerFormat
            // 
            this.containerFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.containerFormat.Location = new System.Drawing.Point(120, 69);
            this.containerFormat.Name = "containerFormat";
            this.containerFormat.Size = new System.Drawing.Size(95, 21);
            this.containerFormat.TabIndex = 41;
            this.containerFormat.SelectedIndexChanged += new System.EventHandler(this.containerFormat_SelectedIndexChanged);
            // 
            // splitting
            // 
            this.splitting.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitting.AutoSize = true;
            this.splitting.CustomSizes = new MeGUI.core.util.FileSize[0];
            this.splitting.Location = new System.Drawing.Point(120, 40);
            this.splitting.MaximumSize = new System.Drawing.Size(1000, 29);
            this.splitting.MinimumSize = new System.Drawing.Size(64, 29);
            this.splitting.Name = "splitting";
            this.splitting.NullString = "No splitting";
            this.splitting.SaveCustomValues = false;
            this.splitting.SelectedIndex = 0;
            this.splitting.Size = new System.Drawing.Size(326, 29);
            this.splitting.TabIndex = 40;
            // 
            // videoGroupBox
            // 
            this.videoGroupBox.Controls.Add(this.ARLabel);
            this.videoGroupBox.Controls.Add(this.ar);
            this.videoGroupBox.Controls.Add(this.chkDontEncodeVideo);
            this.videoGroupBox.Controls.Add(this.usechaptersmarks);
            this.videoGroupBox.Controls.Add(this.label4);
            this.videoGroupBox.Controls.Add(this.videoProfile);
            this.videoGroupBox.Controls.Add(this.addPrerenderJob);
            this.videoGroupBox.Location = new System.Drawing.Point(6, 6);
            this.videoGroupBox.Name = "videoGroupBox";
            this.videoGroupBox.Size = new System.Drawing.Size(452, 116);
            this.videoGroupBox.TabIndex = 41;
            this.videoGroupBox.TabStop = false;
            this.videoGroupBox.Text = " Video Settings ";
            // 
            // ARLabel
            // 
            this.ARLabel.AutoSize = true;
            this.ARLabel.Location = new System.Drawing.Point(7, 90);
            this.ARLabel.Name = "ARLabel";
            this.ARLabel.Size = new System.Drawing.Size(57, 13);
            this.ARLabel.TabIndex = 42;
            this.ARLabel.Text = "Input DAR";
            // 
            // ar
            // 
            this.ar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ar.CustomDARs = new MeGUI.core.util.Dar[0];
            this.ar.HasLater = true;
            this.ar.Location = new System.Drawing.Point(120, 83);
            this.ar.MaximumSize = new System.Drawing.Size(1000, 29);
            this.ar.MinimumSize = new System.Drawing.Size(64, 29);
            this.ar.Name = "ar";
            this.ar.SelectedIndex = 0;
            this.ar.Size = new System.Drawing.Size(326, 29);
            this.ar.TabIndex = 41;
            // 
            // chkDontEncodeVideo
            // 
            this.chkDontEncodeVideo.AutoSize = true;
            this.chkDontEncodeVideo.Location = new System.Drawing.Point(120, 16);
            this.chkDontEncodeVideo.Name = "chkDontEncodeVideo";
            this.chkDontEncodeVideo.Size = new System.Drawing.Size(118, 17);
            this.chkDontEncodeVideo.TabIndex = 40;
            this.chkDontEncodeVideo.Text = "Don\'t encode video";
            this.chkDontEncodeVideo.UseVisualStyleBackColor = true;
            this.chkDontEncodeVideo.CheckedChanged += new System.EventHandler(this.chkDontEncodeVideo_CheckedChanged);
            // 
            // usechaptersmarks
            // 
            this.usechaptersmarks.AutoSize = true;
            this.usechaptersmarks.Location = new System.Drawing.Point(120, 67);
            this.usechaptersmarks.Name = "usechaptersmarks";
            this.usechaptersmarks.Size = new System.Drawing.Size(234, 17);
            this.usechaptersmarks.TabIndex = 39;
            this.usechaptersmarks.Text = "Force using Key-Frames for chapters marks";
            this.usechaptersmarks.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 42);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(88, 13);
            this.label4.TabIndex = 38;
            this.label4.Text = "Encoder Settings";
            // 
            // videoProfile
            // 
            this.videoProfile.Location = new System.Drawing.Point(120, 39);
            this.videoProfile.Name = "videoProfile";
            this.videoProfile.ProfileSet = "Video";
            this.videoProfile.Size = new System.Drawing.Size(326, 22);
            this.videoProfile.TabIndex = 17;
            this.videoProfile.UpdateSelectedProfile = false;
            // 
            // addPrerenderJob
            // 
            this.addPrerenderJob.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.addPrerenderJob.AutoSize = true;
            this.addPrerenderJob.Location = new System.Drawing.Point(-507, 47);
            this.addPrerenderJob.Name = "addPrerenderJob";
            this.addPrerenderJob.Size = new System.Drawing.Size(132, 17);
            this.addPrerenderJob.TabIndex = 16;
            this.addPrerenderJob.Text = "Add pre-rendering job";
            this.addPrerenderJob.UseVisualStyleBackColor = true;
            // 
            // openFileDialog
            // 
            this.openFileDialog.Filter = "IFO Files|*.ifo|VOB Files (*.vob)|*.vob|MPEG-1/2 Program Streams (*.mpg)|*.mpg|Tr" +
    "ansport Streams (*.ts)|*.ts";
            // 
            // goButton
            // 
            this.goButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.goButton.Location = new System.Drawing.Point(385, 519);
            this.goButton.Name = "goButton";
            this.goButton.Size = new System.Drawing.Size(75, 23);
            this.goButton.TabIndex = 29;
            this.goButton.Text = "Queue";
            this.goButton.UseVisualStyleBackColor = true;
            this.goButton.Click += new System.EventHandler(this.goButton_Click);
            // 
            // openOnQueue
            // 
            this.openOnQueue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.openOnQueue.AutoSize = true;
            this.openOnQueue.Checked = true;
            this.openOnQueue.CheckState = System.Windows.Forms.CheckState.Checked;
            this.openOnQueue.Location = new System.Drawing.Point(302, 523);
            this.openOnQueue.Name = "openOnQueue";
            this.openOnQueue.Size = new System.Drawing.Size(77, 17);
            this.openOnQueue.TabIndex = 33;
            this.openOnQueue.Text = "close after";
            // 
            // cbGUIMode
            // 
            this.cbGUIMode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cbGUIMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbGUIMode.FormattingEnabled = true;
            this.cbGUIMode.Location = new System.Drawing.Point(104, 521);
            this.cbGUIMode.Name = "cbGUIMode";
            this.cbGUIMode.Size = new System.Drawing.Size(145, 21);
            this.cbGUIMode.TabIndex = 34;
            this.cbGUIMode.SelectedIndexChanged += new System.EventHandler(this.cbGUIMode_SelectedIndexChanged);
            // 
            // helpButton1
            // 
            this.helpButton1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.helpButton1.ArticleName = "Tools/One_Click_Encoder";
            this.helpButton1.AutoSize = true;
            this.helpButton1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.helpButton1.Location = new System.Drawing.Point(12, 519);
            this.helpButton1.Name = "helpButton1";
            this.helpButton1.Size = new System.Drawing.Size(38, 23);
            this.helpButton1.TabIndex = 32;
            // 
            // OneClickWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(472, 550);
            this.Controls.Add(this.helpButton1);
            this.Controls.Add(this.openOnQueue);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.goButton);
            this.Controls.Add(this.cbGUIMode);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "OneClickWindow";
            this.Text = "MeGUI - One Click Encoder";
            this.Shown += new System.EventHandler(this.OneClickWindow_Shown);
            this.tabPage1.ResumeLayout(false);
            this.outputTab.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            this.videoTab.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.audioTab.ResumeLayout(false);
            this.audioPage0.ResumeLayout(false);
            this.subtitlesTab.ResumeLayout(false);
            this.subPage0.ResumeLayout(false);
            this.subtitleMenu.ResumeLayout(false);
            this.audioMenu.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.encoderConfigTab.ResumeLayout(false);
            this.avsBox.ResumeLayout(false);
            this.avsBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.horizontalResolution)).EndInit();
            this.locationGroupBox.ResumeLayout(false);
            this.locationGroupBox.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.videoGroupBox.ResumeLayout(false);
            this.videoGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.TabPage encoderConfigTab;
        private System.Windows.Forms.Button goButton;
        private MeGUI.core.gui.HelpButton helpButton1;
        private System.Windows.Forms.CheckBox openOnQueue;
        private System.Windows.Forms.TabControl subtitlesTab;
        private System.Windows.Forms.ContextMenuStrip subtitleMenu;
        private System.Windows.Forms.ToolStripMenuItem subtitleAddTrack;
        private System.Windows.Forms.ToolStripMenuItem subtitleRemoveTrack;
        private System.Windows.Forms.TabPage subPage0;
        private OneClickStreamControl oneClickSubtitleStreamControl1;
        private System.Windows.Forms.TabControl audioTab;
        private System.Windows.Forms.ContextMenuStrip audioMenu;
        private System.Windows.Forms.ToolStripMenuItem audioAddTrack;
        private System.Windows.Forms.ToolStripMenuItem audioRemoveTrack;
        private System.Windows.Forms.TabPage audioPage0;
        private OneClickStreamControl oneClickAudioStreamControl1;
        private System.Windows.Forms.TabControl outputTab;
        private System.Windows.Forms.TabPage tabPage4;
        private core.gui.ConfigableProfilesControl oneclickProfile;
        private System.Windows.Forms.Label label3;
        private FileBar output;
        private System.Windows.Forms.Label outputLabel;
        private System.Windows.Forms.TabControl videoTab;
        private System.Windows.Forms.TabPage tabPage3;
        private core.gui.FileSCBox input;
        private System.Windows.Forms.Label inputLabel;
        private System.Windows.Forms.GroupBox videoGroupBox;
        private System.Windows.Forms.CheckBox chkDontEncodeVideo;
        private System.Windows.Forms.CheckBox usechaptersmarks;
        private System.Windows.Forms.Label label4;
        private core.gui.ConfigableProfilesControl videoProfile;
        private System.Windows.Forms.CheckBox addPrerenderJob;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox devicetype;
        private System.Windows.Forms.Label deviceLabel;
        private System.Windows.Forms.Label containerFormatLabel;
        private System.Windows.Forms.ComboBox containerFormat;
        private core.gui.TargetSizeSCBox splitting;
        private System.Windows.Forms.GroupBox locationGroupBox;
        private FileBar chapterFile;
        private FileBar workingDirectory;
        private System.Windows.Forms.Label chapterLabel;
        private System.Windows.Forms.Label workingDirectoryLabel;
        private System.Windows.Forms.GroupBox avsBox;
        private System.Windows.Forms.CheckBox keepInputResolution;
        private System.Windows.Forms.CheckBox autoCrop;
        private core.gui.ConfigableProfilesControl avsProfile;
        private System.Windows.Forms.CheckBox autoDeint;
        private System.Windows.Forms.Label outputResolutionLabel;
        private System.Windows.Forms.NumericUpDown horizontalResolution;
        private System.Windows.Forms.Label label2;
        private core.gui.TargetSizeSCBox fileSize;
        private System.Windows.Forms.Label filesizeLabel;
        private System.Windows.Forms.ComboBox cbGUIMode;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.Label ARLabel;
        private core.gui.ARChooser ar;
        private System.Windows.Forms.Button deleteChapter;
        private System.Windows.Forms.Button deleteWorking;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox inputName;
        private System.Windows.Forms.TabPage tabPage6;
        private System.Windows.Forms.TabPage tabPage7;
    }
}