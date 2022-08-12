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

namespace MeGUI.packages.tools.oneclick
{
    partial class OneClickConfigPanel
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.Label label3;
            this.otherGroupBox = new System.Windows.Forms.GroupBox();
            this.keepInputResolution = new System.Windows.Forms.CheckBox();
            this.autoCrop = new System.Windows.Forms.CheckBox();
            this.avsProfile = new MeGUI.core.gui.ConfigableProfilesControl();
            this.preprocessVideo = new System.Windows.Forms.CheckBox();
            this.autoDeint = new System.Windows.Forms.CheckBox();
            this.horizontalResolution = new System.Windows.Forms.NumericUpDown();
            this.outputResolutionLabel = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.ARLabel = new System.Windows.Forms.Label();
            this.ar = new MeGUI.core.gui.ARChooser();
            this.chkDontEncodeVideo = new System.Windows.Forms.CheckBox();
            this.usechaptersmarks = new System.Windows.Forms.CheckBox();
            this.videoProfile = new MeGUI.core.gui.ConfigableProfilesControl();
            this.videoCodecLabel = new System.Windows.Forms.Label();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.extraGroupbox = new System.Windows.Forms.GroupBox();
            this.audioTab = new System.Windows.Forms.TabControl();
            this.audioPage0 = new System.Windows.Forms.TabPage();
            this.oneClickAudioControl1 = new MeGUI.OneClickAudioControl();
            this.audioPageAdd = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.splitSize = new MeGUI.core.gui.TargetSizeSCBox();
            this.fileSize = new MeGUI.core.gui.TargetSizeSCBox();
            this.label2 = new System.Windows.Forms.Label();
            this.filesizeLabel = new System.Windows.Forms.Label();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.containerTypeList = new System.Windows.Forms.CheckedListBox();
            this.containerFormatLabel = new System.Windows.Forms.Label();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.cbLanguageSelect = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnSubtitleDown = new System.Windows.Forms.Button();
            this.btnSubtitleUp = new System.Windows.Forms.Button();
            this.btnRemoveSubtitle = new System.Windows.Forms.Button();
            this.btnAddSubtitle = new System.Windows.Forms.Button();
            this.lbNonDefaultSubtitle = new System.Windows.Forms.ListBox();
            this.lbDefaultSubtitle = new System.Windows.Forms.ListBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnAudioDown = new System.Windows.Forms.Button();
            this.btnAudioUp = new System.Windows.Forms.Button();
            this.btnRemoveAudio = new System.Windows.Forms.Button();
            this.btnAddAudio = new System.Windows.Forms.Button();
            this.lbNonDefaultAudio = new System.Windows.Forms.ListBox();
            this.lbDefaultAudio = new System.Windows.Forms.ListBox();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.chkDisableIntermediateMKV = new System.Windows.Forms.CheckBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.deleteWorking = new System.Windows.Forms.Button();
            this.deleteOutput = new System.Windows.Forms.Button();
            this.outputDirectory = new MeGUI.FileBar();
            this.label8 = new System.Windows.Forms.Label();
            this.workingDirectory = new MeGUI.FileBar();
            this.workingDirectoryLabel = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtSuffixName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtPrefixName = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtWorkingNameReplaceWith = new System.Windows.Forms.TextBox();
            this.txtWorkingNameDelete = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnIndexerDown = new System.Windows.Forms.Button();
            this.btnIndexerUp = new System.Windows.Forms.Button();
            this.lbIndexerPriority = new System.Windows.Forms.ListBox();
            this.audioMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.audioAddTrack = new System.Windows.Forms.ToolStripMenuItem();
            this.audioRemoveTrack = new System.Windows.Forms.ToolStripMenuItem();
            label3 = new System.Windows.Forms.Label();
            this.otherGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.horizontalResolution)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.extraGroupbox.SuspendLayout();
            this.audioTab.SuspendLayout();
            this.audioPage0.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox8.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.audioMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(6, 82);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(80, 13);
            label3.TabIndex = 40;
            label3.Text = "AviSynth profile";
            // 
            // otherGroupBox
            // 
            this.otherGroupBox.Controls.Add(this.keepInputResolution);
            this.otherGroupBox.Controls.Add(this.autoCrop);
            this.otherGroupBox.Controls.Add(label3);
            this.otherGroupBox.Controls.Add(this.avsProfile);
            this.otherGroupBox.Controls.Add(this.preprocessVideo);
            this.otherGroupBox.Controls.Add(this.autoDeint);
            this.otherGroupBox.Controls.Add(this.horizontalResolution);
            this.otherGroupBox.Controls.Add(this.outputResolutionLabel);
            this.otherGroupBox.Location = new System.Drawing.Point(6, 123);
            this.otherGroupBox.Name = "otherGroupBox";
            this.otherGroupBox.Size = new System.Drawing.Size(416, 128);
            this.otherGroupBox.TabIndex = 38;
            this.otherGroupBox.TabStop = false;
            this.otherGroupBox.Text = "AviSynth setup ";
            // 
            // keepInputResolution
            // 
            this.keepInputResolution.AutoSize = true;
            this.keepInputResolution.Location = new System.Drawing.Point(109, 19);
            this.keepInputResolution.Name = "keepInputResolution";
            this.keepInputResolution.Size = new System.Drawing.Size(236, 17);
            this.keepInputResolution.TabIndex = 42;
            this.keepInputResolution.Text = "Keep Input Resolution (disable crop && resize)";
            this.keepInputResolution.UseVisualStyleBackColor = true;
            this.keepInputResolution.CheckedChanged += new System.EventHandler(this.keepInputResolution_CheckedChanged);
            // 
            // autoCrop
            // 
            this.autoCrop.AutoSize = true;
            this.autoCrop.Checked = true;
            this.autoCrop.CheckState = System.Windows.Forms.CheckState.Checked;
            this.autoCrop.Location = new System.Drawing.Point(179, 43);
            this.autoCrop.Name = "autoCrop";
            this.autoCrop.Size = new System.Drawing.Size(70, 17);
            this.autoCrop.TabIndex = 41;
            this.autoCrop.Text = "AutoCrop";
            this.autoCrop.UseVisualStyleBackColor = true;
            // 
            // avsProfile
            // 
            this.avsProfile.Location = new System.Drawing.Point(109, 78);
            this.avsProfile.Name = "avsProfile";
            this.avsProfile.ProfileSet = "AviSynth";
            this.avsProfile.Size = new System.Drawing.Size(298, 22);
            this.avsProfile.TabIndex = 39;
            this.avsProfile.UpdateSelectedProfile = false;
            // 
            // preprocessVideo
            // 
            this.preprocessVideo.AutoSize = true;
            this.preprocessVideo.Location = new System.Drawing.Point(109, 106);
            this.preprocessVideo.Name = "preprocessVideo";
            this.preprocessVideo.Size = new System.Drawing.Size(101, 17);
            this.preprocessVideo.TabIndex = 37;
            this.preprocessVideo.Text = "Prerender video";
            this.preprocessVideo.UseVisualStyleBackColor = true;
            // 
            // autoDeint
            // 
            this.autoDeint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.autoDeint.AutoSize = true;
            this.autoDeint.Location = new System.Drawing.Point(272, 106);
            this.autoDeint.Name = "autoDeint";
            this.autoDeint.Size = new System.Drawing.Size(138, 17);
            this.autoDeint.TabIndex = 35;
            this.autoDeint.Text = "Automatic Deinterlacing";
            this.autoDeint.UseVisualStyleBackColor = true;
            // 
            // horizontalResolution
            // 
            this.horizontalResolution.Increment = new decimal(new int[] {
            16,
            0,
            0,
            0});
            this.horizontalResolution.Location = new System.Drawing.Point(109, 42);
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
            this.horizontalResolution.Size = new System.Drawing.Size(64, 20);
            this.horizontalResolution.TabIndex = 27;
            this.horizontalResolution.Value = new decimal(new int[] {
            640,
            0,
            0,
            0});
            // 
            // outputResolutionLabel
            // 
            this.outputResolutionLabel.Location = new System.Drawing.Point(6, 39);
            this.outputResolutionLabel.Name = "outputResolutionLabel";
            this.outputResolutionLabel.Size = new System.Drawing.Size(100, 26);
            this.outputResolutionLabel.TabIndex = 30;
            this.outputResolutionLabel.Text = "Output Resolution        (Max. Width)";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Controls.Add(this.tabPage5);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(433, 280);
            this.tabControl1.TabIndex = 39;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.otherGroupBox);
            this.tabPage1.Controls.Add(this.groupBox6);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(425, 254);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Video";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.ARLabel);
            this.groupBox6.Controls.Add(this.ar);
            this.groupBox6.Controls.Add(this.chkDontEncodeVideo);
            this.groupBox6.Controls.Add(this.usechaptersmarks);
            this.groupBox6.Controls.Add(this.videoProfile);
            this.groupBox6.Controls.Add(this.videoCodecLabel);
            this.groupBox6.Location = new System.Drawing.Point(6, 6);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(416, 111);
            this.groupBox6.TabIndex = 47;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = " Video Setup ";
            // 
            // ARLabel
            // 
            this.ARLabel.AutoSize = true;
            this.ARLabel.Location = new System.Drawing.Point(6, 80);
            this.ARLabel.Name = "ARLabel";
            this.ARLabel.Size = new System.Drawing.Size(87, 13);
            this.ARLabel.TabIndex = 45;
            this.ARLabel.Text = "Force Input DAR";
            // 
            // ar
            // 
            this.ar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ar.CustomDARs = new MeGUI.core.util.Dar[0];
            this.ar.HasLater = true;
            this.ar.Location = new System.Drawing.Point(99, 72);
            this.ar.MaximumSize = new System.Drawing.Size(1000, 29);
            this.ar.MinimumSize = new System.Drawing.Size(64, 29);
            this.ar.Name = "ar";
            this.ar.SelectedIndex = 0;
            this.ar.Size = new System.Drawing.Size(308, 29);
            this.ar.TabIndex = 44;
            // 
            // chkDontEncodeVideo
            // 
            this.chkDontEncodeVideo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkDontEncodeVideo.AutoSize = true;
            this.chkDontEncodeVideo.Location = new System.Drawing.Point(81, 22);
            this.chkDontEncodeVideo.Name = "chkDontEncodeVideo";
            this.chkDontEncodeVideo.Size = new System.Drawing.Size(119, 17);
            this.chkDontEncodeVideo.TabIndex = 42;
            this.chkDontEncodeVideo.Text = "Don\'t encode video";
            this.chkDontEncodeVideo.CheckedChanged += new System.EventHandler(this.chkDontEncodeVideo_CheckedChanged);
            // 
            // usechaptersmarks
            // 
            this.usechaptersmarks.AutoSize = true;
            this.usechaptersmarks.Location = new System.Drawing.Point(210, 22);
            this.usechaptersmarks.Name = "usechaptersmarks";
            this.usechaptersmarks.Size = new System.Drawing.Size(197, 17);
            this.usechaptersmarks.TabIndex = 41;
            this.usechaptersmarks.Text = "Force key frames for chapters marks";
            this.usechaptersmarks.UseVisualStyleBackColor = true;
            // 
            // videoProfile
            // 
            this.videoProfile.Location = new System.Drawing.Point(81, 47);
            this.videoProfile.Name = "videoProfile";
            this.videoProfile.ProfileSet = "Video";
            this.videoProfile.Size = new System.Drawing.Size(326, 22);
            this.videoProfile.TabIndex = 40;
            this.videoProfile.UpdateSelectedProfile = false;
            // 
            // videoCodecLabel
            // 
            this.videoCodecLabel.Location = new System.Drawing.Point(6, 51);
            this.videoCodecLabel.Name = "videoCodecLabel";
            this.videoCodecLabel.Size = new System.Drawing.Size(90, 13);
            this.videoCodecLabel.TabIndex = 18;
            this.videoCodecLabel.Text = "Encoder";
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.extraGroupbox);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(425, 254);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Audio";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // extraGroupbox
            // 
            this.extraGroupbox.Controls.Add(this.audioTab);
            this.extraGroupbox.Location = new System.Drawing.Point(3, 6);
            this.extraGroupbox.Name = "extraGroupbox";
            this.extraGroupbox.Size = new System.Drawing.Size(419, 242);
            this.extraGroupbox.TabIndex = 40;
            this.extraGroupbox.TabStop = false;
            this.extraGroupbox.Text = " Audio Setup (audio settings for specific languages) ";
            // 
            // audioTab
            // 
            this.audioTab.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.audioTab.Controls.Add(this.audioPage0);
            this.audioTab.Controls.Add(this.audioPageAdd);
            this.audioTab.Location = new System.Drawing.Point(6, 30);
            this.audioTab.Name = "audioTab";
            this.audioTab.SelectedIndex = 0;
            this.audioTab.Size = new System.Drawing.Size(407, 150);
            this.audioTab.TabIndex = 44;
            this.audioTab.SelectedIndexChanged += new System.EventHandler(this.audioTab_SelectedIndexChanged);
            this.audioTab.VisibleChanged += new System.EventHandler(this.audioTab_VisibleChanged);
            this.audioTab.KeyUp += new System.Windows.Forms.KeyEventHandler(this.audioTab_KeyUp);
            this.audioTab.MouseClick += new System.Windows.Forms.MouseEventHandler(this.audioTab_MouseClick);
            this.audioTab.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.audioTab_MouseDoubleClick);
            // 
            // audioPage0
            // 
            this.audioPage0.Controls.Add(this.oneClickAudioControl1);
            this.audioPage0.Location = new System.Drawing.Point(4, 22);
            this.audioPage0.Name = "audioPage0";
            this.audioPage0.Size = new System.Drawing.Size(399, 124);
            this.audioPage0.TabIndex = 2;
            this.audioPage0.Text = "Default";
            this.audioPage0.UseVisualStyleBackColor = true;
            // 
            // oneClickAudioControl1
            // 
            this.oneClickAudioControl1.Location = new System.Drawing.Point(6, 9);
            this.oneClickAudioControl1.Name = "oneClickAudioControl1";
            this.oneClickAudioControl1.Size = new System.Drawing.Size(386, 114);
            this.oneClickAudioControl1.TabIndex = 0;
            // 
            // audioPageAdd
            // 
            this.audioPageAdd.Location = new System.Drawing.Point(4, 22);
            this.audioPageAdd.Name = "audioPageAdd";
            this.audioPageAdd.Size = new System.Drawing.Size(399, 124);
            this.audioPageAdd.TabIndex = 3;
            this.audioPageAdd.Text = "   +";
            this.audioPageAdd.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.groupBox8);
            this.tabPage2.Controls.Add(this.groupBox7);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(425, 254);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Output";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.splitSize);
            this.groupBox8.Controls.Add(this.fileSize);
            this.groupBox8.Controls.Add(this.label2);
            this.groupBox8.Controls.Add(this.filesizeLabel);
            this.groupBox8.Location = new System.Drawing.Point(3, 148);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(416, 100);
            this.groupBox8.TabIndex = 19;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = " File ";
            // 
            // splitSize
            // 
            this.splitSize.CustomSizes = new MeGUI.core.util.FileSize[0];
            this.splitSize.Location = new System.Drawing.Point(69, 49);
            this.splitSize.MaximumSize = new System.Drawing.Size(1000, 29);
            this.splitSize.MinimumSize = new System.Drawing.Size(64, 29);
            this.splitSize.Name = "splitSize";
            this.splitSize.NullString = "Dont split";
            this.splitSize.SaveCustomValues = false;
            this.splitSize.SelectedIndex = 0;
            this.splitSize.Size = new System.Drawing.Size(340, 29);
            this.splitSize.TabIndex = 41;
            // 
            // fileSize
            // 
            this.fileSize.CustomSizes = new MeGUI.core.util.FileSize[0];
            this.fileSize.Location = new System.Drawing.Point(69, 23);
            this.fileSize.MaximumSize = new System.Drawing.Size(1000, 29);
            this.fileSize.MinimumSize = new System.Drawing.Size(64, 29);
            this.fileSize.Name = "fileSize";
            this.fileSize.NullString = "Don\'t care";
            this.fileSize.SaveCustomValues = false;
            this.fileSize.SelectedIndex = 0;
            this.fileSize.Size = new System.Drawing.Size(340, 29);
            this.fileSize.TabIndex = 42;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(8, 57);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(90, 13);
            this.label2.TabIndex = 39;
            this.label2.Text = "Splitting";
            // 
            // filesizeLabel
            // 
            this.filesizeLabel.Location = new System.Drawing.Point(8, 31);
            this.filesizeLabel.Name = "filesizeLabel";
            this.filesizeLabel.Size = new System.Drawing.Size(90, 13);
            this.filesizeLabel.TabIndex = 40;
            this.filesizeLabel.Text = "Filesize";
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.containerTypeList);
            this.groupBox7.Controls.Add(this.containerFormatLabel);
            this.groupBox7.Location = new System.Drawing.Point(3, 7);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(419, 135);
            this.groupBox7.TabIndex = 18;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = " Container ";
            // 
            // containerTypeList
            // 
            this.containerTypeList.CheckOnClick = true;
            this.containerTypeList.Location = new System.Drawing.Point(6, 16);
            this.containerTypeList.Name = "containerTypeList";
            this.containerTypeList.Size = new System.Drawing.Size(171, 109);
            this.containerTypeList.TabIndex = 21;
            // 
            // containerFormatLabel
            // 
            this.containerFormatLabel.Location = new System.Drawing.Point(183, 16);
            this.containerFormatLabel.Name = "containerFormatLabel";
            this.containerFormatLabel.Size = new System.Drawing.Size(223, 109);
            this.containerFormatLabel.TabIndex = 18;
            this.containerFormatLabel.Text = "Text change later for resource behavior reasons";
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.cbLanguageSelect);
            this.tabPage4.Controls.Add(this.label7);
            this.tabPage4.Controls.Add(this.groupBox2);
            this.tabPage4.Controls.Add(this.groupBox1);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(425, 254);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Language";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // cbLanguageSelect
            // 
            this.cbLanguageSelect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbLanguageSelect.FormattingEnabled = true;
            this.cbLanguageSelect.Items.AddRange(new object[] {
            "all",
            "none"});
            this.cbLanguageSelect.Location = new System.Drawing.Point(268, 207);
            this.cbLanguageSelect.Name = "cbLanguageSelect";
            this.cbLanguageSelect.Size = new System.Drawing.Size(130, 21);
            this.cbLanguageSelect.TabIndex = 47;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(16, 210);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(238, 13);
            this.label7.TabIndex = 46;
            this.label7.Text = "Languages to select if selection does not match: ";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnSubtitleDown);
            this.groupBox2.Controls.Add(this.btnSubtitleUp);
            this.groupBox2.Controls.Add(this.btnRemoveSubtitle);
            this.groupBox2.Controls.Add(this.btnAddSubtitle);
            this.groupBox2.Controls.Add(this.lbNonDefaultSubtitle);
            this.groupBox2.Controls.Add(this.lbDefaultSubtitle);
            this.groupBox2.Location = new System.Drawing.Point(6, 101);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(413, 93);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = " Default Subtitle Language (tracks with this language will be preselected) ";
            // 
            // btnSubtitleDown
            // 
            this.btnSubtitleDown.Location = new System.Drawing.Point(151, 68);
            this.btnSubtitleDown.Name = "btnSubtitleDown";
            this.btnSubtitleDown.Size = new System.Drawing.Size(29, 20);
            this.btnSubtitleDown.TabIndex = 6;
            this.btnSubtitleDown.Text = "-";
            this.btnSubtitleDown.UseVisualStyleBackColor = true;
            this.btnSubtitleDown.Click += new System.EventHandler(this.btnSubtitleDown_Click);
            // 
            // btnSubtitleUp
            // 
            this.btnSubtitleUp.Location = new System.Drawing.Point(151, 18);
            this.btnSubtitleUp.Name = "btnSubtitleUp";
            this.btnSubtitleUp.Size = new System.Drawing.Size(29, 20);
            this.btnSubtitleUp.TabIndex = 5;
            this.btnSubtitleUp.Text = "+";
            this.btnSubtitleUp.UseVisualStyleBackColor = true;
            this.btnSubtitleUp.Click += new System.EventHandler(this.btnSubtitleUp_Click);
            // 
            // btnRemoveSubtitle
            // 
            this.btnRemoveSubtitle.Location = new System.Drawing.Point(151, 43);
            this.btnRemoveSubtitle.Name = "btnRemoveSubtitle";
            this.btnRemoveSubtitle.Size = new System.Drawing.Size(29, 20);
            this.btnRemoveSubtitle.TabIndex = 4;
            this.btnRemoveSubtitle.Text = ">>";
            this.btnRemoveSubtitle.UseVisualStyleBackColor = true;
            this.btnRemoveSubtitle.Click += new System.EventHandler(this.btnRemoveSubtitle_Click);
            // 
            // btnAddSubtitle
            // 
            this.btnAddSubtitle.Location = new System.Drawing.Point(227, 43);
            this.btnAddSubtitle.Name = "btnAddSubtitle";
            this.btnAddSubtitle.Size = new System.Drawing.Size(29, 20);
            this.btnAddSubtitle.TabIndex = 3;
            this.btnAddSubtitle.Text = "<<";
            this.btnAddSubtitle.UseVisualStyleBackColor = true;
            this.btnAddSubtitle.Click += new System.EventHandler(this.btnAddSubtitle_Click);
            // 
            // lbNonDefaultSubtitle
            // 
            this.lbNonDefaultSubtitle.FormattingEnabled = true;
            this.lbNonDefaultSubtitle.Location = new System.Drawing.Point(262, 18);
            this.lbNonDefaultSubtitle.Name = "lbNonDefaultSubtitle";
            this.lbNonDefaultSubtitle.Size = new System.Drawing.Size(130, 69);
            this.lbNonDefaultSubtitle.Sorted = true;
            this.lbNonDefaultSubtitle.TabIndex = 2;
            this.lbNonDefaultSubtitle.SelectedIndexChanged += new System.EventHandler(this.lbNonDefaultSubtitle_SelectedIndexChanged);
            // 
            // lbDefaultSubtitle
            // 
            this.lbDefaultSubtitle.FormattingEnabled = true;
            this.lbDefaultSubtitle.Location = new System.Drawing.Point(15, 18);
            this.lbDefaultSubtitle.Name = "lbDefaultSubtitle";
            this.lbDefaultSubtitle.Size = new System.Drawing.Size(130, 69);
            this.lbDefaultSubtitle.TabIndex = 1;
            this.lbDefaultSubtitle.SelectedIndexChanged += new System.EventHandler(this.lbDefaultSubtitle_SelectedIndexChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnAudioDown);
            this.groupBox1.Controls.Add(this.btnAudioUp);
            this.groupBox1.Controls.Add(this.btnRemoveAudio);
            this.groupBox1.Controls.Add(this.btnAddAudio);
            this.groupBox1.Controls.Add(this.lbNonDefaultAudio);
            this.groupBox1.Controls.Add(this.lbDefaultAudio);
            this.groupBox1.Location = new System.Drawing.Point(6, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(413, 93);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = " Default Audio Language (tracks with this language will be preselected) ";
            // 
            // btnAudioDown
            // 
            this.btnAudioDown.Location = new System.Drawing.Point(151, 68);
            this.btnAudioDown.Name = "btnAudioDown";
            this.btnAudioDown.Size = new System.Drawing.Size(29, 20);
            this.btnAudioDown.TabIndex = 6;
            this.btnAudioDown.Text = "-";
            this.btnAudioDown.UseVisualStyleBackColor = true;
            this.btnAudioDown.Click += new System.EventHandler(this.btnAudioDown_Click);
            // 
            // btnAudioUp
            // 
            this.btnAudioUp.Location = new System.Drawing.Point(151, 18);
            this.btnAudioUp.Name = "btnAudioUp";
            this.btnAudioUp.Size = new System.Drawing.Size(29, 20);
            this.btnAudioUp.TabIndex = 5;
            this.btnAudioUp.Text = "+";
            this.btnAudioUp.UseVisualStyleBackColor = true;
            this.btnAudioUp.Click += new System.EventHandler(this.btnAudioUp_Click);
            // 
            // btnRemoveAudio
            // 
            this.btnRemoveAudio.Location = new System.Drawing.Point(151, 43);
            this.btnRemoveAudio.Name = "btnRemoveAudio";
            this.btnRemoveAudio.Size = new System.Drawing.Size(29, 20);
            this.btnRemoveAudio.TabIndex = 4;
            this.btnRemoveAudio.Text = ">>";
            this.btnRemoveAudio.UseVisualStyleBackColor = true;
            this.btnRemoveAudio.Click += new System.EventHandler(this.btnRemoveAudio_Click);
            // 
            // btnAddAudio
            // 
            this.btnAddAudio.Location = new System.Drawing.Point(227, 43);
            this.btnAddAudio.Name = "btnAddAudio";
            this.btnAddAudio.Size = new System.Drawing.Size(29, 20);
            this.btnAddAudio.TabIndex = 3;
            this.btnAddAudio.Text = "<<";
            this.btnAddAudio.UseVisualStyleBackColor = true;
            this.btnAddAudio.Click += new System.EventHandler(this.btnAddAudio_Click);
            // 
            // lbNonDefaultAudio
            // 
            this.lbNonDefaultAudio.FormattingEnabled = true;
            this.lbNonDefaultAudio.Location = new System.Drawing.Point(262, 18);
            this.lbNonDefaultAudio.Name = "lbNonDefaultAudio";
            this.lbNonDefaultAudio.Size = new System.Drawing.Size(130, 69);
            this.lbNonDefaultAudio.Sorted = true;
            this.lbNonDefaultAudio.TabIndex = 2;
            this.lbNonDefaultAudio.SelectedIndexChanged += new System.EventHandler(this.lbNonDefaultAudio_SelectedIndexChanged);
            // 
            // lbDefaultAudio
            // 
            this.lbDefaultAudio.FormattingEnabled = true;
            this.lbDefaultAudio.Location = new System.Drawing.Point(15, 19);
            this.lbDefaultAudio.Name = "lbDefaultAudio";
            this.lbDefaultAudio.Size = new System.Drawing.Size(130, 69);
            this.lbDefaultAudio.TabIndex = 1;
            this.lbDefaultAudio.SelectedIndexChanged += new System.EventHandler(this.lbDefaultAudio_SelectedIndexChanged);
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.chkDisableIntermediateMKV);
            this.tabPage5.Controls.Add(this.groupBox5);
            this.tabPage5.Controls.Add(this.groupBox4);
            this.tabPage5.Controls.Add(this.groupBox3);
            this.tabPage5.Location = new System.Drawing.Point(4, 22);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage5.Size = new System.Drawing.Size(425, 254);
            this.tabPage5.TabIndex = 4;
            this.tabPage5.Text = "Other";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // chkDisableIntermediateMKV
            // 
            this.chkDisableIntermediateMKV.AutoSize = true;
            this.chkDisableIntermediateMKV.Location = new System.Drawing.Point(15, 221);
            this.chkDisableIntermediateMKV.Name = "chkDisableIntermediateMKV";
            this.chkDisableIntermediateMKV.Size = new System.Drawing.Size(161, 17);
            this.chkDisableIntermediateMKV.TabIndex = 45;
            this.chkDisableIntermediateMKV.Text = "disable intermediate MKV file";
            this.chkDisableIntermediateMKV.UseVisualStyleBackColor = true;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.deleteWorking);
            this.groupBox5.Controls.Add(this.deleteOutput);
            this.groupBox5.Controls.Add(this.outputDirectory);
            this.groupBox5.Controls.Add(this.label8);
            this.groupBox5.Controls.Add(this.workingDirectory);
            this.groupBox5.Controls.Add(this.workingDirectoryLabel);
            this.groupBox5.Location = new System.Drawing.Point(6, 6);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(413, 84);
            this.groupBox5.TabIndex = 44;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = " Default Directories ";
            // 
            // deleteWorking
            // 
            this.deleteWorking.Location = new System.Drawing.Point(370, 52);
            this.deleteWorking.Name = "deleteWorking";
            this.deleteWorking.Size = new System.Drawing.Size(28, 23);
            this.deleteWorking.TabIndex = 46;
            this.deleteWorking.Text = "X";
            this.deleteWorking.UseVisualStyleBackColor = true;
            this.deleteWorking.Click += new System.EventHandler(this.deleteWorking_Click);
            // 
            // deleteOutput
            // 
            this.deleteOutput.Location = new System.Drawing.Point(370, 20);
            this.deleteOutput.Name = "deleteOutput";
            this.deleteOutput.Size = new System.Drawing.Size(28, 23);
            this.deleteOutput.TabIndex = 45;
            this.deleteOutput.Text = "X";
            this.deleteOutput.UseVisualStyleBackColor = true;
            this.deleteOutput.Click += new System.EventHandler(this.deleteOutput_Click);
            // 
            // outputDirectory
            // 
            this.outputDirectory.Filename = "";
            this.outputDirectory.FolderMode = true;
            this.outputDirectory.Location = new System.Drawing.Point(64, 20);
            this.outputDirectory.Name = "outputDirectory";
            this.outputDirectory.Size = new System.Drawing.Size(300, 23);
            this.outputDirectory.TabIndex = 43;
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(6, 25);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(108, 13);
            this.label8.TabIndex = 44;
            this.label8.Text = "Output";
            // 
            // workingDirectory
            // 
            this.workingDirectory.Filename = "";
            this.workingDirectory.FolderMode = true;
            this.workingDirectory.Location = new System.Drawing.Point(64, 52);
            this.workingDirectory.Name = "workingDirectory";
            this.workingDirectory.Size = new System.Drawing.Size(300, 23);
            this.workingDirectory.TabIndex = 42;
            // 
            // workingDirectoryLabel
            // 
            this.workingDirectoryLabel.Location = new System.Drawing.Point(6, 56);
            this.workingDirectoryLabel.Name = "workingDirectoryLabel";
            this.workingDirectoryLabel.Size = new System.Drawing.Size(108, 13);
            this.workingDirectoryLabel.TabIndex = 41;
            this.workingDirectoryLabel.Text = "Working";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label4);
            this.groupBox4.Controls.Add(this.txtSuffixName);
            this.groupBox4.Controls.Add(this.label1);
            this.groupBox4.Controls.Add(this.txtPrefixName);
            this.groupBox4.Controls.Add(this.label6);
            this.groupBox4.Controls.Add(this.label5);
            this.groupBox4.Controls.Add(this.txtWorkingNameReplaceWith);
            this.groupBox4.Controls.Add(this.txtWorkingNameDelete);
            this.groupBox4.Location = new System.Drawing.Point(212, 96);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(207, 142);
            this.groupBox4.TabIndex = 43;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = " Project Name ";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 57);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(33, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Suffix";
            // 
            // txtSuffixName
            // 
            this.txtSuffixName.Location = new System.Drawing.Point(60, 54);
            this.txtSuffixName.Name = "txtSuffixName";
            this.txtSuffixName.Size = new System.Drawing.Size(121, 20);
            this.txtSuffixName.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(33, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Prefix";
            // 
            // txtPrefixName
            // 
            this.txtPrefixName.Location = new System.Drawing.Point(60, 28);
            this.txtPrefixName.Name = "txtPrefixName";
            this.txtPrefixName.Size = new System.Drawing.Size(121, 20);
            this.txtPrefixName.TabIndex = 4;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(7, 109);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(29, 13);
            this.label6.TabIndex = 3;
            this.label6.Text = "With";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(7, 83);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(47, 13);
            this.label5.TabIndex = 2;
            this.label5.Text = "Replace";
            // 
            // txtWorkingNameReplaceWith
            // 
            this.txtWorkingNameReplaceWith.Location = new System.Drawing.Point(60, 106);
            this.txtWorkingNameReplaceWith.Name = "txtWorkingNameReplaceWith";
            this.txtWorkingNameReplaceWith.Size = new System.Drawing.Size(121, 20);
            this.txtWorkingNameReplaceWith.TabIndex = 1;
            // 
            // txtWorkingNameDelete
            // 
            this.txtWorkingNameDelete.Location = new System.Drawing.Point(60, 80);
            this.txtWorkingNameDelete.Name = "txtWorkingNameDelete";
            this.txtWorkingNameDelete.Size = new System.Drawing.Size(121, 20);
            this.txtWorkingNameDelete.TabIndex = 0;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btnIndexerDown);
            this.groupBox3.Controls.Add(this.btnIndexerUp);
            this.groupBox3.Controls.Add(this.lbIndexerPriority);
            this.groupBox3.Location = new System.Drawing.Point(6, 96);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(200, 111);
            this.groupBox3.TabIndex = 42;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = " Indexer / Opener Priority ";
            // 
            // btnIndexerDown
            // 
            this.btnIndexerDown.Enabled = false;
            this.btnIndexerDown.Location = new System.Drawing.Point(145, 64);
            this.btnIndexerDown.Name = "btnIndexerDown";
            this.btnIndexerDown.Size = new System.Drawing.Size(29, 23);
            this.btnIndexerDown.TabIndex = 44;
            this.btnIndexerDown.Text = "-";
            this.btnIndexerDown.UseVisualStyleBackColor = true;
            this.btnIndexerDown.Click += new System.EventHandler(this.btnIndexerDown_Click);
            // 
            // btnIndexerUp
            // 
            this.btnIndexerUp.Enabled = false;
            this.btnIndexerUp.Location = new System.Drawing.Point(145, 33);
            this.btnIndexerUp.Name = "btnIndexerUp";
            this.btnIndexerUp.Size = new System.Drawing.Size(29, 23);
            this.btnIndexerUp.TabIndex = 43;
            this.btnIndexerUp.Text = "+";
            this.btnIndexerUp.UseVisualStyleBackColor = true;
            this.btnIndexerUp.Click += new System.EventHandler(this.btnIndexerUp_Click);
            // 
            // lbIndexerPriority
            // 
            this.lbIndexerPriority.FormattingEnabled = true;
            this.lbIndexerPriority.Location = new System.Drawing.Point(33, 18);
            this.lbIndexerPriority.Name = "lbIndexerPriority";
            this.lbIndexerPriority.Size = new System.Drawing.Size(106, 82);
            this.lbIndexerPriority.TabIndex = 42;
            this.lbIndexerPriority.MouseClick += new System.Windows.Forms.MouseEventHandler(this.lbIndexerPriority_MouseClick);
            // 
            // audioMenu
            // 
            this.audioMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.audioAddTrack,
            this.audioRemoveTrack});
            this.audioMenu.Name = "audioMenu";
            this.audioMenu.Size = new System.Drawing.Size(149, 48);
            this.audioMenu.Opening += new System.ComponentModel.CancelEventHandler(this.audioMenu_Opening);
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
            // OneClickConfigPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.tabControl1);
            this.Name = "OneClickConfigPanel";
            this.Size = new System.Drawing.Size(433, 280);
            this.otherGroupBox.ResumeLayout(false);
            this.otherGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.horizontalResolution)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.extraGroupbox.ResumeLayout(false);
            this.audioTab.ResumeLayout(false);
            this.audioPage0.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.groupBox8.ResumeLayout(false);
            this.groupBox7.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.tabPage5.ResumeLayout(false);
            this.tabPage5.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.audioMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox otherGroupBox;
        private System.Windows.Forms.CheckBox autoDeint;
        private System.Windows.Forms.NumericUpDown horizontalResolution;
        private System.Windows.Forms.Label outputResolutionLabel;
        private System.Windows.Forms.CheckBox preprocessVideo;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private MeGUI.core.gui.ConfigableProfilesControl avsProfile;
        private System.Windows.Forms.CheckBox autoCrop;
        private System.Windows.Forms.CheckBox keepInputResolution;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.GroupBox extraGroupbox;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListBox lbDefaultAudio;
        private System.Windows.Forms.ListBox lbNonDefaultAudio;
        private System.Windows.Forms.Button btnAudioDown;
        private System.Windows.Forms.Button btnAudioUp;
        private System.Windows.Forms.Button btnRemoveAudio;
        private System.Windows.Forms.Button btnAddAudio;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnSubtitleDown;
        private System.Windows.Forms.Button btnSubtitleUp;
        private System.Windows.Forms.Button btnRemoveSubtitle;
        private System.Windows.Forms.Button btnAddSubtitle;
        private System.Windows.Forms.ListBox lbNonDefaultSubtitle;
        private System.Windows.Forms.ListBox lbDefaultSubtitle;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ListBox lbIndexerPriority;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtWorkingNameReplaceWith;
        private System.Windows.Forms.TextBox txtWorkingNameDelete;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cbLanguageSelect;
        private System.Windows.Forms.GroupBox groupBox5;
        private FileBar outputDirectory;
        private System.Windows.Forms.Label label8;
        private FileBar workingDirectory;
        private System.Windows.Forms.Label workingDirectoryLabel;
        private System.Windows.Forms.Button btnIndexerDown;
        private System.Windows.Forms.Button btnIndexerUp;
        private System.Windows.Forms.TabControl audioTab;
        private System.Windows.Forms.TabPage audioPage0;
        private OneClickAudioControl oneClickAudioControl1;
        private System.Windows.Forms.ContextMenuStrip audioMenu;
        private System.Windows.Forms.ToolStripMenuItem audioAddTrack;
        private System.Windows.Forms.ToolStripMenuItem audioRemoveTrack;
        private System.Windows.Forms.TabPage audioPageAdd;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.CheckBox chkDontEncodeVideo;
        private System.Windows.Forms.CheckBox usechaptersmarks;
        private core.gui.ConfigableProfilesControl videoProfile;
        private System.Windows.Forms.Label videoCodecLabel;
        private System.Windows.Forms.GroupBox groupBox8;
        private core.gui.TargetSizeSCBox splitSize;
        private core.gui.TargetSizeSCBox fileSize;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label filesizeLabel;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.CheckedListBox containerTypeList;
        private System.Windows.Forms.Label containerFormatLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtPrefixName;
        private System.Windows.Forms.Button deleteWorking;
        private System.Windows.Forms.Button deleteOutput;
        private System.Windows.Forms.CheckBox chkDisableIntermediateMKV;
        private System.Windows.Forms.Label ARLabel;
        private core.gui.ARChooser ar;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtSuffixName;
    }
}
