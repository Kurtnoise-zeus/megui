// ****************************************************************************
// 
// Copyright (C) 2005-2022 Doom9 & al
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

using System.Collections.Generic;
using System.Windows.Forms;

namespace MeGUI
{
    partial class AviSynthWindow
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
            System.Windows.Forms.Label label2;
            System.Windows.Forms.Label label3;
            System.Windows.Forms.Label label4;
            System.Windows.Forms.Label label5;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AviSynthWindow));
            this.resNCropGroupbox = new System.Windows.Forms.GroupBox();
            this.lblAspectError = new System.Windows.Forms.Label();
            this.lblAR = new System.Windows.Forms.Label();
            this.modValueBox = new System.Windows.Forms.ComboBox();
            this.chAutoPreview = new System.Windows.Forms.CheckBox();
            this.signalAR = new System.Windows.Forms.CheckBox();
            this.mod16Box = new System.Windows.Forms.ComboBox();
            this.resize = new System.Windows.Forms.CheckBox();
            this.suggestResolution = new System.Windows.Forms.CheckBox();
            this.cropLeft = new System.Windows.Forms.NumericUpDown();
            this.cropRight = new System.Windows.Forms.NumericUpDown();
            this.cropBottom = new System.Windows.Forms.NumericUpDown();
            this.cropTop = new System.Windows.Forms.NumericUpDown();
            this.autoCropButton = new System.Windows.Forms.Button();
            this.crop = new System.Windows.Forms.CheckBox();
            this.verticalResolution = new System.Windows.Forms.NumericUpDown();
            this.horizontalResolution = new System.Windows.Forms.NumericUpDown();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.optionsTab = new System.Windows.Forms.TabPage();
            this.gbOutput = new System.Windows.Forms.GroupBox();
            this.videoOutput = new MeGUI.FileBar();
            this.label7 = new System.Windows.Forms.Label();
            this.videoGroupBox = new System.Windows.Forms.GroupBox();
            this.input = new MeGUI.FileBar();
            this.avsProfile = new MeGUI.core.gui.ConfigableProfilesControl();
            this.arChooser = new MeGUI.core.gui.ARChooser();
            this.reopenOriginal = new System.Windows.Forms.Button();
            this.tvTypeLabel = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.inputDARLabel = new System.Windows.Forms.Label();
            this.videoInputLabel = new System.Windows.Forms.Label();
            this.filterTab = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.deleteSubtitle = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.SubtitlesPath = new System.Windows.Forms.TextBox();
            this.cbCharset = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.openSubtitlesButton = new System.Windows.Forms.Button();
            this.tabSources = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.mpegOptGroupBox = new System.Windows.Forms.GroupBox();
            this.colourCorrect = new System.Windows.Forms.CheckBox();
            this.mpeg2Deblocking = new System.Windows.Forms.CheckBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.aviOptGroupBox = new System.Windows.Forms.GroupBox();
            this.dss2 = new System.Windows.Forms.CheckBox();
            this.fpsBox = new System.Windows.Forms.NumericUpDown();
            this.fpsLabel = new System.Windows.Forms.Label();
            this.flipVertical = new System.Windows.Forms.CheckBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.dgOptions = new System.Windows.Forms.GroupBox();
            this.nvResize = new System.Windows.Forms.CheckBox();
            this.cbNvDeInt = new System.Windows.Forms.ComboBox();
            this.nvDeInt = new System.Windows.Forms.CheckBox();
            this.deinterlacingGroupBox = new System.Windows.Forms.GroupBox();
            this.deintM = new System.Windows.Forms.NumericUpDown();
            this.deintFieldOrder = new System.Windows.Forms.ComboBox();
            this.deintSourceType = new System.Windows.Forms.ComboBox();
            this.deintIsAnime = new System.Windows.Forms.CheckBox();
            this.analyseButton = new System.Windows.Forms.Button();
            this.deinterlace = new System.Windows.Forms.CheckBox();
            this.deinterlaceType = new System.Windows.Forms.ComboBox();
            this.filtersGroupbox = new System.Windows.Forms.GroupBox();
            this.noiseFilterType = new System.Windows.Forms.ComboBox();
            this.noiseFilter = new System.Windows.Forms.CheckBox();
            this.resizeFilterType = new System.Windows.Forms.ComboBox();
            this.resizeFilterLabel = new System.Windows.Forms.Label();
            this.editTab = new System.Windows.Forms.TabPage();
            this.openDLLButton = new System.Windows.Forms.Button();
            this.dllPath = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.avisynthScript = new System.Windows.Forms.TextBox();
            this.saveAvisynthScriptDialog = new System.Windows.Forms.SaveFileDialog();
            this.openFilterDialog = new System.Windows.Forms.OpenFileDialog();
            this.openSubsDialog = new System.Windows.Forms.OpenFileDialog();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.deintProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.deintStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.onSaveLoadScript = new System.Windows.Forms.CheckBox();
            this.saveButton = new System.Windows.Forms.Button();
            this.previewAvsButton = new System.Windows.Forms.Button();
            this.helpButton1 = new MeGUI.core.gui.HelpButton();
            label2 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            label4 = new System.Windows.Forms.Label();
            label5 = new System.Windows.Forms.Label();
            this.resNCropGroupbox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cropLeft)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cropRight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cropBottom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cropTop)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.verticalResolution)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.horizontalResolution)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.optionsTab.SuspendLayout();
            this.gbOutput.SuspendLayout();
            this.videoGroupBox.SuspendLayout();
            this.filterTab.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabSources.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.mpegOptGroupBox.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.aviOptGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fpsBox)).BeginInit();
            this.tabPage3.SuspendLayout();
            this.dgOptions.SuspendLayout();
            this.deinterlacingGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.deintM)).BeginInit();
            this.filtersGroupbox.SuspendLayout();
            this.editTab.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(9, 22);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(258, 13);
            label2.TabIndex = 11;
            label2.Text = "Source Info (Click on \'Analyse...\' for autodetection):";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(9, 50);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(69, 13);
            label3.TabIndex = 13;
            label3.Text = "Source type:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(9, 79);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(62, 13);
            label4.TabIndex = 14;
            label4.Text = "Field order:";
            // 
            // label5
            // 
            label5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            label5.AutoSize = true;
            label5.Location = new System.Drawing.Point(342, 50);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(19, 13);
            label5.TabIndex = 17;
            label5.Text = "M:";
            // 
            // resNCropGroupbox
            // 
            this.resNCropGroupbox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.resNCropGroupbox.Controls.Add(this.lblAspectError);
            this.resNCropGroupbox.Controls.Add(this.lblAR);
            this.resNCropGroupbox.Controls.Add(this.modValueBox);
            this.resNCropGroupbox.Controls.Add(this.chAutoPreview);
            this.resNCropGroupbox.Controls.Add(this.signalAR);
            this.resNCropGroupbox.Controls.Add(this.mod16Box);
            this.resNCropGroupbox.Controls.Add(this.resize);
            this.resNCropGroupbox.Controls.Add(this.suggestResolution);
            this.resNCropGroupbox.Controls.Add(this.cropLeft);
            this.resNCropGroupbox.Controls.Add(this.cropRight);
            this.resNCropGroupbox.Controls.Add(this.cropBottom);
            this.resNCropGroupbox.Controls.Add(this.cropTop);
            this.resNCropGroupbox.Controls.Add(this.autoCropButton);
            this.resNCropGroupbox.Controls.Add(this.crop);
            this.resNCropGroupbox.Controls.Add(this.verticalResolution);
            this.resNCropGroupbox.Controls.Add(this.horizontalResolution);
            this.resNCropGroupbox.Enabled = false;
            this.resNCropGroupbox.Location = new System.Drawing.Point(3, 157);
            this.resNCropGroupbox.Name = "resNCropGroupbox";
            this.resNCropGroupbox.Size = new System.Drawing.Size(450, 197);
            this.resNCropGroupbox.TabIndex = 0;
            this.resNCropGroupbox.TabStop = false;
            this.resNCropGroupbox.Text = "Crop && Resize";
            // 
            // lblAspectError
            // 
            this.lblAspectError.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblAspectError.Location = new System.Drawing.Point(340, 160);
            this.lblAspectError.Name = "lblAspectError";
            this.lblAspectError.Size = new System.Drawing.Size(75, 21);
            this.lblAspectError.TabIndex = 14;
            this.lblAspectError.Text = "0.00000%";
            this.lblAspectError.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblAR
            // 
            this.lblAR.Location = new System.Drawing.Point(236, 164);
            this.lblAR.Name = "lblAR";
            this.lblAR.Size = new System.Drawing.Size(95, 13);
            this.lblAR.TabIndex = 13;
            this.lblAR.Text = "Aspect Ratio Error";
            // 
            // modValueBox
            // 
            this.modValueBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.modValueBox.FormattingEnabled = true;
            this.modValueBox.Items.AddRange(new object[] {
            "mod16",
            "mod8",
            "mod4",
            "mod2"});
            this.modValueBox.Location = new System.Drawing.Point(340, 119);
            this.modValueBox.Name = "modValueBox";
            this.modValueBox.Size = new System.Drawing.Size(75, 21);
            this.modValueBox.TabIndex = 11;
            this.modValueBox.SelectedIndexChanged += new System.EventHandler(this.updateEverything);
            // 
            // chAutoPreview
            // 
            this.chAutoPreview.AutoSize = true;
            this.chAutoPreview.Location = new System.Drawing.Point(11, 161);
            this.chAutoPreview.Name = "chAutoPreview";
            this.chAutoPreview.Size = new System.Drawing.Size(119, 17);
            this.chAutoPreview.TabIndex = 10;
            this.chAutoPreview.Text = "Apply auto Preview";
            this.chAutoPreview.UseVisualStyleBackColor = true;
            this.chAutoPreview.CheckedChanged += new System.EventHandler(this.chAutoPreview_CheckedChanged);
            // 
            // signalAR
            // 
            this.signalAR.AutoSize = true;
            this.signalAR.Location = new System.Drawing.Point(11, 31);
            this.signalAR.Name = "signalAR";
            this.signalAR.Size = new System.Drawing.Size(190, 17);
            this.signalAR.TabIndex = 11;
            this.signalAR.Text = "Clever (TM) anamorphic encoding:";
            this.signalAR.CheckedChanged += new System.EventHandler(this.updateEverything);
            // 
            // mod16Box
            // 
            this.mod16Box.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.mod16Box.Enabled = false;
            this.mod16Box.FormattingEnabled = true;
            this.mod16Box.Items.AddRange(new object[] {
            "Resize to selected mod",
            "Overcrop to achieve selected mod",
            "Encode non-mod16",
            "Crop mod4 horizontally",
            "Undercrop to achieve selected mod"});
            this.mod16Box.Location = new System.Drawing.Point(222, 29);
            this.mod16Box.Name = "mod16Box";
            this.mod16Box.Size = new System.Drawing.Size(222, 21);
            this.mod16Box.TabIndex = 19;
            this.mod16Box.SelectedIndexChanged += new System.EventHandler(this.updateEverything);
            // 
            // resize
            // 
            this.resize.AutoSize = true;
            this.resize.Checked = true;
            this.resize.CheckState = System.Windows.Forms.CheckState.Checked;
            this.resize.Location = new System.Drawing.Point(11, 121);
            this.resize.Name = "resize";
            this.resize.Size = new System.Drawing.Size(57, 17);
            this.resize.TabIndex = 9;
            this.resize.Text = "Resize";
            this.resize.UseVisualStyleBackColor = true;
            this.resize.CheckedChanged += new System.EventHandler(this.resize_CheckedChanged);
            // 
            // suggestResolution
            // 
            this.suggestResolution.AutoSize = true;
            this.suggestResolution.Checked = true;
            this.suggestResolution.CheckState = System.Windows.Forms.CheckState.Checked;
            this.suggestResolution.Location = new System.Drawing.Point(222, 120);
            this.suggestResolution.Name = "suggestResolution";
            this.suggestResolution.Size = new System.Drawing.Size(118, 17);
            this.suggestResolution.TabIndex = 8;
            this.suggestResolution.Text = "Suggest Resolution";
            this.suggestResolution.CheckedChanged += new System.EventHandler(this.updateEverything);
            // 
            // cropLeft
            // 
            this.cropLeft.Enabled = false;
            this.cropLeft.Increment = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.cropLeft.Location = new System.Drawing.Point(114, 73);
            this.cropLeft.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.cropLeft.Name = "cropLeft";
            this.cropLeft.Size = new System.Drawing.Size(48, 21);
            this.cropLeft.TabIndex = 7;
            this.cropLeft.ValueChanged += new System.EventHandler(this.updateEverything);
            // 
            // cropRight
            // 
            this.cropRight.Enabled = false;
            this.cropRight.Increment = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.cropRight.Location = new System.Drawing.Point(222, 73);
            this.cropRight.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.cropRight.Name = "cropRight";
            this.cropRight.Size = new System.Drawing.Size(48, 21);
            this.cropRight.TabIndex = 6;
            this.cropRight.ValueChanged += new System.EventHandler(this.updateEverything);
            // 
            // cropBottom
            // 
            this.cropBottom.Enabled = false;
            this.cropBottom.Increment = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.cropBottom.Location = new System.Drawing.Point(168, 87);
            this.cropBottom.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.cropBottom.Name = "cropBottom";
            this.cropBottom.Size = new System.Drawing.Size(48, 21);
            this.cropBottom.TabIndex = 5;
            this.cropBottom.ValueChanged += new System.EventHandler(this.updateEverything);
            // 
            // cropTop
            // 
            this.cropTop.Enabled = false;
            this.cropTop.Increment = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.cropTop.Location = new System.Drawing.Point(168, 60);
            this.cropTop.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.cropTop.Name = "cropTop";
            this.cropTop.Size = new System.Drawing.Size(48, 21);
            this.cropTop.TabIndex = 4;
            this.cropTop.ValueChanged += new System.EventHandler(this.updateEverything);
            // 
            // autoCropButton
            // 
            this.autoCropButton.Location = new System.Drawing.Point(340, 73);
            this.autoCropButton.Name = "autoCropButton";
            this.autoCropButton.Size = new System.Drawing.Size(75, 23);
            this.autoCropButton.TabIndex = 3;
            this.autoCropButton.Text = "Auto Crop";
            this.autoCropButton.Click += new System.EventHandler(this.autoCropButton_Click);
            // 
            // crop
            // 
            this.crop.Location = new System.Drawing.Point(11, 64);
            this.crop.Name = "crop";
            this.crop.Size = new System.Drawing.Size(97, 42);
            this.crop.TabIndex = 2;
            this.crop.Text = "Crop";
            this.crop.CheckedChanged += new System.EventHandler(this.updateEverything);
            // 
            // verticalResolution
            // 
            this.verticalResolution.Enabled = false;
            this.verticalResolution.Increment = new decimal(new int[] {
            16,
            0,
            0,
            0});
            this.verticalResolution.Location = new System.Drawing.Point(168, 117);
            this.verticalResolution.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.verticalResolution.Name = "verticalResolution";
            this.verticalResolution.Size = new System.Drawing.Size(48, 21);
            this.verticalResolution.TabIndex = 1;
            this.verticalResolution.ValueChanged += new System.EventHandler(this.updateEverything);
            // 
            // horizontalResolution
            // 
            this.horizontalResolution.Increment = new decimal(new int[] {
            16,
            0,
            0,
            0});
            this.horizontalResolution.Location = new System.Drawing.Point(114, 117);
            this.horizontalResolution.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.horizontalResolution.Name = "horizontalResolution";
            this.horizontalResolution.Size = new System.Drawing.Size(48, 21);
            this.horizontalResolution.TabIndex = 0;
            this.horizontalResolution.ValueChanged += new System.EventHandler(this.updateEverything);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.optionsTab);
            this.tabControl1.Controls.Add(this.filterTab);
            this.tabControl1.Controls.Add(this.editTab);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(463, 459);
            this.tabControl1.TabIndex = 5;
            // 
            // optionsTab
            // 
            this.optionsTab.Controls.Add(this.gbOutput);
            this.optionsTab.Controls.Add(this.videoGroupBox);
            this.optionsTab.Controls.Add(this.resNCropGroupbox);
            this.optionsTab.Location = new System.Drawing.Point(4, 22);
            this.optionsTab.Name = "optionsTab";
            this.optionsTab.Size = new System.Drawing.Size(455, 433);
            this.optionsTab.TabIndex = 0;
            this.optionsTab.Text = "I/O";
            this.optionsTab.UseVisualStyleBackColor = true;
            // 
            // gbOutput
            // 
            this.gbOutput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbOutput.Controls.Add(this.videoOutput);
            this.gbOutput.Controls.Add(this.label7);
            this.gbOutput.Location = new System.Drawing.Point(3, 360);
            this.gbOutput.Name = "gbOutput";
            this.gbOutput.Size = new System.Drawing.Size(450, 55);
            this.gbOutput.TabIndex = 13;
            this.gbOutput.TabStop = false;
            this.gbOutput.Text = "Output";
            // 
            // videoOutput
            // 
            this.videoOutput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.videoOutput.Filename = "";
            this.videoOutput.Filter = "AVI Synth Scripts|*.avs";
            this.videoOutput.Location = new System.Drawing.Point(96, 18);
            this.videoOutput.Name = "videoOutput";
            this.videoOutput.SaveMode = true;
            this.videoOutput.Size = new System.Drawing.Size(348, 23);
            this.videoOutput.TabIndex = 7;
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(8, 24);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(80, 13);
            this.label7.TabIndex = 6;
            this.label7.Text = "Video Output";
            // 
            // videoGroupBox
            // 
            this.videoGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.videoGroupBox.Controls.Add(this.input);
            this.videoGroupBox.Controls.Add(this.avsProfile);
            this.videoGroupBox.Controls.Add(this.arChooser);
            this.videoGroupBox.Controls.Add(this.reopenOriginal);
            this.videoGroupBox.Controls.Add(this.tvTypeLabel);
            this.videoGroupBox.Controls.Add(this.label6);
            this.videoGroupBox.Controls.Add(this.inputDARLabel);
            this.videoGroupBox.Controls.Add(this.videoInputLabel);
            this.videoGroupBox.Location = new System.Drawing.Point(3, 8);
            this.videoGroupBox.Name = "videoGroupBox";
            this.videoGroupBox.Size = new System.Drawing.Size(450, 143);
            this.videoGroupBox.TabIndex = 5;
            this.videoGroupBox.TabStop = false;
            this.videoGroupBox.Text = "Input";
            // 
            // input
            // 
            this.input.AllowDrop = true;
            this.input.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.input.Filename = "";
            this.input.Filter = "All files|*.*";
            this.input.FilterIndex = 1;
            this.input.Location = new System.Drawing.Point(96, 16);
            this.input.Name = "input";
            this.input.Size = new System.Drawing.Size(348, 23);
            this.input.TabIndex = 1;
            this.input.Title = "Select a source file";
            this.input.FileSelected += new MeGUI.FileBarEventHandler(this.input_FileSelected);
            // 
            // avsProfile
            // 
            this.avsProfile.Location = new System.Drawing.Point(96, 111);
            this.avsProfile.Name = "avsProfile";
            this.avsProfile.ProfileSet = "AviSynth";
            this.avsProfile.Size = new System.Drawing.Size(348, 22);
            this.avsProfile.TabIndex = 22;
            this.avsProfile.SelectedProfileChanged += new System.EventHandler(this.ProfileChanged);
            // 
            // arChooser
            // 
            this.arChooser.CustomDARs = new MeGUI.core.util.Dar[0];
            this.arChooser.HasLater = false;
            this.arChooser.Location = new System.Drawing.Point(96, 76);
            this.arChooser.MaximumSize = new System.Drawing.Size(1000, 29);
            this.arChooser.MinimumSize = new System.Drawing.Size(64, 29);
            this.arChooser.Name = "arChooser";
            this.arChooser.SelectedIndex = 0;
            this.arChooser.Size = new System.Drawing.Size(214, 29);
            this.arChooser.TabIndex = 21;
            this.arChooser.SelectionChanged += new MeGUI.StringChanged(this.inputDARChanged);
            // 
            // reopenOriginal
            // 
            this.reopenOriginal.AutoSize = true;
            this.reopenOriginal.Location = new System.Drawing.Point(96, 47);
            this.reopenOriginal.Name = "reopenOriginal";
            this.reopenOriginal.Size = new System.Drawing.Size(183, 23);
            this.reopenOriginal.TabIndex = 20;
            this.reopenOriginal.Text = "Re-open original video player";
            this.reopenOriginal.UseVisualStyleBackColor = true;
            this.reopenOriginal.Click += new System.EventHandler(this.reopenOriginal_Click);
            // 
            // tvTypeLabel
            // 
            this.tvTypeLabel.Location = new System.Drawing.Point(316, 82);
            this.tvTypeLabel.Name = "tvTypeLabel";
            this.tvTypeLabel.Size = new System.Drawing.Size(48, 23);
            this.tvTypeLabel.TabIndex = 10;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(8, 115);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(83, 13);
            this.label6.TabIndex = 7;
            this.label6.Text = "AviSynth profile";
            // 
            // inputDARLabel
            // 
            this.inputDARLabel.Location = new System.Drawing.Point(8, 83);
            this.inputDARLabel.Name = "inputDARLabel";
            this.inputDARLabel.Size = new System.Drawing.Size(72, 13);
            this.inputDARLabel.TabIndex = 7;
            this.inputDARLabel.Text = "Input DAR";
            // 
            // videoInputLabel
            // 
            this.videoInputLabel.Location = new System.Drawing.Point(8, 21);
            this.videoInputLabel.Name = "videoInputLabel";
            this.videoInputLabel.Size = new System.Drawing.Size(80, 13);
            this.videoInputLabel.TabIndex = 0;
            this.videoInputLabel.Text = "Video Input";
            // 
            // filterTab
            // 
            this.filterTab.Controls.Add(this.groupBox1);
            this.filterTab.Controls.Add(this.tabSources);
            this.filterTab.Controls.Add(this.deinterlacingGroupBox);
            this.filterTab.Controls.Add(this.filtersGroupbox);
            this.filterTab.Location = new System.Drawing.Point(4, 22);
            this.filterTab.Name = "filterTab";
            this.filterTab.Size = new System.Drawing.Size(455, 433);
            this.filterTab.TabIndex = 2;
            this.filterTab.Text = "Filters";
            this.filterTab.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.deleteSubtitle);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.SubtitlesPath);
            this.groupBox1.Controls.Add(this.cbCharset);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.openSubtitlesButton);
            this.groupBox1.Location = new System.Drawing.Point(3, 362);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(449, 68);
            this.groupBox1.TabIndex = 15;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = " Subtitles ";
            // 
            // deleteSubtitle
            // 
            this.deleteSubtitle.Location = new System.Drawing.Point(418, 15);
            this.deleteSubtitle.Name = "deleteSubtitle";
            this.deleteSubtitle.Size = new System.Drawing.Size(27, 22);
            this.deleteSubtitle.TabIndex = 15;
            this.deleteSubtitle.Text = "X";
            this.deleteSubtitle.UseVisualStyleBackColor = true;
            this.deleteSubtitle.Click += new System.EventHandler(this.deleteSubtitle_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(9, 20);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(27, 13);
            this.label9.TabIndex = 14;
            this.label9.Text = "File:";
            // 
            // SubtitlesPath
            // 
            this.SubtitlesPath.BackColor = System.Drawing.SystemColors.Control;
            this.SubtitlesPath.Location = new System.Drawing.Point(97, 15);
            this.SubtitlesPath.Name = "SubtitlesPath";
            this.SubtitlesPath.ReadOnly = true;
            this.SubtitlesPath.Size = new System.Drawing.Size(282, 21);
            this.SubtitlesPath.TabIndex = 9;
            // 
            // cbCharset
            // 
            this.cbCharset.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCharset.Enabled = false;
            this.cbCharset.FormattingEnabled = true;
            this.cbCharset.Items.AddRange(new object[] {
            "Default",
            "ANSI",
            "Symbol",
            "Shiftjis",
            "Hangeul",
            "Hangul",
            "GB2312",
            "Chinese Big 5",
            "OEM",
            "Johab",
            "Hebrew",
            "Arabic",
            "Greek",
            "Turkish",
            "Vietnamese",
            "Thai",
            "East Europe",
            "Russian",
            "Mac",
            "Baltic"});
            this.cbCharset.Location = new System.Drawing.Point(97, 42);
            this.cbCharset.Name = "cbCharset";
            this.cbCharset.Size = new System.Drawing.Size(239, 21);
            this.cbCharset.TabIndex = 12;
            this.cbCharset.SelectedIndexChanged += new System.EventHandler(this.refreshScript);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(9, 45);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(49, 13);
            this.label8.TabIndex = 13;
            this.label8.Text = "Charset:";
            // 
            // openSubtitlesButton
            // 
            this.openSubtitlesButton.Location = new System.Drawing.Point(385, 15);
            this.openSubtitlesButton.Name = "openSubtitlesButton";
            this.openSubtitlesButton.Size = new System.Drawing.Size(27, 22);
            this.openSubtitlesButton.TabIndex = 10;
            this.openSubtitlesButton.Text = "...";
            this.openSubtitlesButton.UseVisualStyleBackColor = true;
            this.openSubtitlesButton.Click += new System.EventHandler(this.openSubtitlesButton_Click);
            // 
            // tabSources
            // 
            this.tabSources.Controls.Add(this.tabPage1);
            this.tabSources.Controls.Add(this.tabPage2);
            this.tabSources.Controls.Add(this.tabPage3);
            this.tabSources.Location = new System.Drawing.Point(3, 3);
            this.tabSources.Name = "tabSources";
            this.tabSources.SelectedIndex = 0;
            this.tabSources.Size = new System.Drawing.Size(449, 116);
            this.tabSources.TabIndex = 14;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.mpegOptGroupBox);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(441, 90);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "MPEG2 Source";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // mpegOptGroupBox
            // 
            this.mpegOptGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.mpegOptGroupBox.Controls.Add(this.colourCorrect);
            this.mpegOptGroupBox.Controls.Add(this.mpeg2Deblocking);
            this.mpegOptGroupBox.Enabled = false;
            this.mpegOptGroupBox.Location = new System.Drawing.Point(6, 3);
            this.mpegOptGroupBox.Name = "mpegOptGroupBox";
            this.mpegOptGroupBox.Size = new System.Drawing.Size(426, 80);
            this.mpegOptGroupBox.TabIndex = 22;
            this.mpegOptGroupBox.TabStop = false;
            // 
            // colourCorrect
            // 
            this.colourCorrect.Location = new System.Drawing.Point(10, 43);
            this.colourCorrect.Name = "colourCorrect";
            this.colourCorrect.Size = new System.Drawing.Size(111, 17);
            this.colourCorrect.TabIndex = 9;
            this.colourCorrect.Text = "Colour Correction";
            this.colourCorrect.CheckedChanged += new System.EventHandler(this.refreshScript);
            // 
            // mpeg2Deblocking
            // 
            this.mpeg2Deblocking.Location = new System.Drawing.Point(10, 20);
            this.mpeg2Deblocking.Name = "mpeg2Deblocking";
            this.mpeg2Deblocking.Size = new System.Drawing.Size(124, 17);
            this.mpeg2Deblocking.TabIndex = 8;
            this.mpeg2Deblocking.Text = "Mpeg2 Deblocking";
            this.mpeg2Deblocking.CheckedChanged += new System.EventHandler(this.refreshScript);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.aviOptGroupBox);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(441, 90);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "AVI Source / DSSource";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // aviOptGroupBox
            // 
            this.aviOptGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.aviOptGroupBox.Controls.Add(this.dss2);
            this.aviOptGroupBox.Controls.Add(this.fpsBox);
            this.aviOptGroupBox.Controls.Add(this.fpsLabel);
            this.aviOptGroupBox.Controls.Add(this.flipVertical);
            this.aviOptGroupBox.Enabled = false;
            this.aviOptGroupBox.Location = new System.Drawing.Point(6, 3);
            this.aviOptGroupBox.Name = "aviOptGroupBox";
            this.aviOptGroupBox.Size = new System.Drawing.Size(426, 80);
            this.aviOptGroupBox.TabIndex = 23;
            this.aviOptGroupBox.TabStop = false;
            // 
            // dss2
            // 
            this.dss2.AutoSize = true;
            this.dss2.Location = new System.Drawing.Point(120, 20);
            this.dss2.Name = "dss2";
            this.dss2.Size = new System.Drawing.Size(185, 17);
            this.dss2.TabIndex = 4;
            this.dss2.Text = "Prefer DSSource2 over DSSource";
            this.dss2.UseVisualStyleBackColor = true;
            this.dss2.CheckedChanged += new System.EventHandler(this.refreshScript);
            // 
            // fpsBox
            // 
            this.fpsBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fpsBox.DecimalPlaces = 3;
            this.fpsBox.Location = new System.Drawing.Point(40, 43);
            this.fpsBox.Maximum = new decimal(new int[] {
            999999,
            0,
            0,
            0});
            this.fpsBox.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            this.fpsBox.Name = "fpsBox";
            this.fpsBox.Size = new System.Drawing.Size(130, 21);
            this.fpsBox.TabIndex = 3;
            this.fpsBox.Value = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            this.fpsBox.ValueChanged += new System.EventHandler(this.refreshScript);
            // 
            // fpsLabel
            // 
            this.fpsLabel.Location = new System.Drawing.Point(9, 45);
            this.fpsLabel.Name = "fpsLabel";
            this.fpsLabel.Size = new System.Drawing.Size(25, 13);
            this.fpsLabel.TabIndex = 2;
            this.fpsLabel.Text = "FPS";
            // 
            // flipVertical
            // 
            this.flipVertical.Location = new System.Drawing.Point(10, 20);
            this.flipVertical.Name = "flipVertical";
            this.flipVertical.Size = new System.Drawing.Size(90, 17);
            this.flipVertical.TabIndex = 0;
            this.flipVertical.Text = "Vertical Flip";
            this.flipVertical.CheckedChanged += new System.EventHandler(this.refreshScript);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.dgOptions);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(441, 90);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "DGI Source";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // dgOptions
            // 
            this.dgOptions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgOptions.Controls.Add(this.nvResize);
            this.dgOptions.Controls.Add(this.cbNvDeInt);
            this.dgOptions.Controls.Add(this.nvDeInt);
            this.dgOptions.Location = new System.Drawing.Point(6, 3);
            this.dgOptions.Name = "dgOptions";
            this.dgOptions.Size = new System.Drawing.Size(426, 80);
            this.dgOptions.TabIndex = 14;
            this.dgOptions.TabStop = false;
            // 
            // nvResize
            // 
            this.nvResize.AutoSize = true;
            this.nvResize.Location = new System.Drawing.Point(10, 52);
            this.nvResize.Name = "nvResize";
            this.nvResize.Size = new System.Drawing.Size(125, 17);
            this.nvResize.TabIndex = 2;
            this.nvResize.Text = "Nvidia Crop && Resize";
            this.nvResize.UseVisualStyleBackColor = true;
            this.nvResize.CheckedChanged += new System.EventHandler(this.refreshScript);
            this.nvResize.Click += new System.EventHandler(this.nvDeInt_Click);
            // 
            // cbNvDeInt
            // 
            this.cbNvDeInt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbNvDeInt.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbNvDeInt.Enabled = false;
            this.cbNvDeInt.FormattingEnabled = true;
            this.cbNvDeInt.Location = new System.Drawing.Point(155, 18);
            this.cbNvDeInt.Name = "cbNvDeInt";
            this.cbNvDeInt.Size = new System.Drawing.Size(265, 21);
            this.cbNvDeInt.TabIndex = 1;
            this.cbNvDeInt.SelectedIndexChanged += new System.EventHandler(this.refreshScript);
            // 
            // nvDeInt
            // 
            this.nvDeInt.AutoSize = true;
            this.nvDeInt.Enabled = false;
            this.nvDeInt.Location = new System.Drawing.Point(10, 20);
            this.nvDeInt.Name = "nvDeInt";
            this.nvDeInt.Size = new System.Drawing.Size(116, 17);
            this.nvDeInt.TabIndex = 0;
            this.nvDeInt.Text = "Nvidia Deinterlacer";
            this.nvDeInt.UseVisualStyleBackColor = true;
            this.nvDeInt.CheckedChanged += new System.EventHandler(this.nvDeInt_CheckedChanged);
            this.nvDeInt.Click += new System.EventHandler(this.nvDeInt_Click);
            // 
            // deinterlacingGroupBox
            // 
            this.deinterlacingGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.deinterlacingGroupBox.Controls.Add(label5);
            this.deinterlacingGroupBox.Controls.Add(this.deintM);
            this.deinterlacingGroupBox.Controls.Add(this.deintFieldOrder);
            this.deinterlacingGroupBox.Controls.Add(label4);
            this.deinterlacingGroupBox.Controls.Add(label3);
            this.deinterlacingGroupBox.Controls.Add(this.deintSourceType);
            this.deinterlacingGroupBox.Controls.Add(label2);
            this.deinterlacingGroupBox.Controls.Add(this.deintIsAnime);
            this.deinterlacingGroupBox.Controls.Add(this.analyseButton);
            this.deinterlacingGroupBox.Controls.Add(this.deinterlace);
            this.deinterlacingGroupBox.Controls.Add(this.deinterlaceType);
            this.deinterlacingGroupBox.Enabled = false;
            this.deinterlacingGroupBox.Location = new System.Drawing.Point(3, 121);
            this.deinterlacingGroupBox.Name = "deinterlacingGroupBox";
            this.deinterlacingGroupBox.Size = new System.Drawing.Size(449, 153);
            this.deinterlacingGroupBox.TabIndex = 12;
            this.deinterlacingGroupBox.TabStop = false;
            this.deinterlacingGroupBox.Text = " Deinterlacing ";
            // 
            // deintM
            // 
            this.deintM.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.deintM.Location = new System.Drawing.Point(367, 47);
            this.deintM.Maximum = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.deintM.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.deintM.Name = "deintM";
            this.deintM.Size = new System.Drawing.Size(76, 21);
            this.deintM.TabIndex = 16;
            this.deintM.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.deintM.ValueChanged += new System.EventHandler(this.deintSourceType_SelectedIndexChanged);
            // 
            // deintFieldOrder
            // 
            this.deintFieldOrder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.deintFieldOrder.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.deintFieldOrder.FormattingEnabled = true;
            this.deintFieldOrder.Location = new System.Drawing.Point(97, 76);
            this.deintFieldOrder.Name = "deintFieldOrder";
            this.deintFieldOrder.Size = new System.Drawing.Size(239, 21);
            this.deintFieldOrder.TabIndex = 15;
            this.deintFieldOrder.SelectedIndexChanged += new System.EventHandler(this.deintSourceType_SelectedIndexChanged);
            // 
            // deintSourceType
            // 
            this.deintSourceType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.deintSourceType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.deintSourceType.FormattingEnabled = true;
            this.deintSourceType.Location = new System.Drawing.Point(97, 47);
            this.deintSourceType.Name = "deintSourceType";
            this.deintSourceType.Size = new System.Drawing.Size(239, 21);
            this.deintSourceType.TabIndex = 12;
            this.deintSourceType.SelectedIndexChanged += new System.EventHandler(this.deintSourceType_SelectedIndexChanged);
            // 
            // deintIsAnime
            // 
            this.deintIsAnime.AutoSize = true;
            this.deintIsAnime.Location = new System.Drawing.Point(9, 133);
            this.deintIsAnime.Name = "deintIsAnime";
            this.deintIsAnime.Size = new System.Drawing.Size(297, 17);
            this.deintIsAnime.TabIndex = 10;
            this.deintIsAnime.Text = "Source is Anime (not automatically detected by Analysis)";
            this.deintIsAnime.UseVisualStyleBackColor = true;
            this.deintIsAnime.CheckedChanged += new System.EventHandler(this.deintSourceType_SelectedIndexChanged);
            // 
            // analyseButton
            // 
            this.analyseButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.analyseButton.Location = new System.Drawing.Point(345, 17);
            this.analyseButton.Name = "analyseButton";
            this.analyseButton.Size = new System.Drawing.Size(98, 23);
            this.analyseButton.TabIndex = 8;
            this.analyseButton.Text = "Analyse";
            this.analyseButton.UseVisualStyleBackColor = true;
            this.analyseButton.Click += new System.EventHandler(this.analyseButton_Click);
            // 
            // deinterlace
            // 
            this.deinterlace.Location = new System.Drawing.Point(9, 103);
            this.deinterlace.Name = "deinterlace";
            this.deinterlace.Size = new System.Drawing.Size(82, 24);
            this.deinterlace.TabIndex = 2;
            this.deinterlace.Text = "Deinterlace";
            this.deinterlace.CheckedChanged += new System.EventHandler(this.deinterlace_CheckedChanged);
            // 
            // deinterlaceType
            // 
            this.deinterlaceType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.deinterlaceType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.deinterlaceType.Enabled = false;
            this.deinterlaceType.Items.AddRange(new object[] {
            "Leak Kernel Deinterlace",
            "Field Deinterlace",
            "Field Deinterlace (no blend)",
            "Telecide for PAL"});
            this.deinterlaceType.Location = new System.Drawing.Point(97, 105);
            this.deinterlaceType.Name = "deinterlaceType";
            this.deinterlaceType.Size = new System.Drawing.Size(239, 21);
            this.deinterlaceType.TabIndex = 4;
            this.deinterlaceType.SelectedIndexChanged += new System.EventHandler(this.refreshScript);
            // 
            // filtersGroupbox
            // 
            this.filtersGroupbox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filtersGroupbox.Controls.Add(this.noiseFilterType);
            this.filtersGroupbox.Controls.Add(this.noiseFilter);
            this.filtersGroupbox.Controls.Add(this.resizeFilterType);
            this.filtersGroupbox.Controls.Add(this.resizeFilterLabel);
            this.filtersGroupbox.Enabled = false;
            this.filtersGroupbox.Location = new System.Drawing.Point(3, 280);
            this.filtersGroupbox.Name = "filtersGroupbox";
            this.filtersGroupbox.Size = new System.Drawing.Size(449, 76);
            this.filtersGroupbox.TabIndex = 9;
            this.filtersGroupbox.TabStop = false;
            this.filtersGroupbox.Text = " Filters ";
            // 
            // noiseFilterType
            // 
            this.noiseFilterType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.noiseFilterType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.noiseFilterType.Enabled = false;
            this.noiseFilterType.Location = new System.Drawing.Point(97, 44);
            this.noiseFilterType.Name = "noiseFilterType";
            this.noiseFilterType.Size = new System.Drawing.Size(239, 21);
            this.noiseFilterType.TabIndex = 5;
            this.noiseFilterType.SelectedIndexChanged += new System.EventHandler(this.refreshScript);
            // 
            // noiseFilter
            // 
            this.noiseFilter.Location = new System.Drawing.Point(9, 44);
            this.noiseFilter.Name = "noiseFilter";
            this.noiseFilter.Size = new System.Drawing.Size(104, 24);
            this.noiseFilter.TabIndex = 3;
            this.noiseFilter.Text = "Noise Filter";
            this.noiseFilter.CheckedChanged += new System.EventHandler(this.noiseFilter_CheckedChanged);
            // 
            // resizeFilterType
            // 
            this.resizeFilterType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.resizeFilterType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.resizeFilterType.Location = new System.Drawing.Point(97, 17);
            this.resizeFilterType.Name = "resizeFilterType";
            this.resizeFilterType.Size = new System.Drawing.Size(239, 21);
            this.resizeFilterType.TabIndex = 1;
            this.resizeFilterType.SelectedIndexChanged += new System.EventHandler(this.refreshScript);
            // 
            // resizeFilterLabel
            // 
            this.resizeFilterLabel.Location = new System.Drawing.Point(9, 17);
            this.resizeFilterLabel.Name = "resizeFilterLabel";
            this.resizeFilterLabel.Size = new System.Drawing.Size(100, 23);
            this.resizeFilterLabel.TabIndex = 0;
            this.resizeFilterLabel.Text = "Resize Filter";
            this.resizeFilterLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // editTab
            // 
            this.editTab.Controls.Add(this.openDLLButton);
            this.editTab.Controls.Add(this.dllPath);
            this.editTab.Controls.Add(this.label1);
            this.editTab.Controls.Add(this.avisynthScript);
            this.editTab.Location = new System.Drawing.Point(4, 22);
            this.editTab.Name = "editTab";
            this.editTab.Size = new System.Drawing.Size(455, 433);
            this.editTab.TabIndex = 1;
            this.editTab.Text = "Script";
            this.editTab.UseVisualStyleBackColor = true;
            // 
            // openDLLButton
            // 
            this.openDLLButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.openDLLButton.Location = new System.Drawing.Point(420, 399);
            this.openDLLButton.Name = "openDLLButton";
            this.openDLLButton.Size = new System.Drawing.Size(27, 21);
            this.openDLLButton.TabIndex = 3;
            this.openDLLButton.Text = "...";
            this.openDLLButton.Click += new System.EventHandler(this.openDLLButton_Click);
            // 
            // dllPath
            // 
            this.dllPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dllPath.Location = new System.Drawing.Point(65, 399);
            this.dllPath.Name = "dllPath";
            this.dllPath.ReadOnly = true;
            this.dllPath.Size = new System.Drawing.Size(338, 21);
            this.dllPath.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.Location = new System.Drawing.Point(9, 402);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Load DLL";
            // 
            // avisynthScript
            // 
            this.avisynthScript.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.avisynthScript.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.avisynthScript.Location = new System.Drawing.Point(8, 15);
            this.avisynthScript.Multiline = true;
            this.avisynthScript.Name = "avisynthScript";
            this.avisynthScript.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.avisynthScript.Size = new System.Drawing.Size(439, 356);
            this.avisynthScript.TabIndex = 0;
            // 
            // saveAvisynthScriptDialog
            // 
            this.saveAvisynthScriptDialog.DefaultExt = "avs";
            this.saveAvisynthScriptDialog.Filter = "AviSynth Script Files|*.avs";
            this.saveAvisynthScriptDialog.Title = "Select a name for the AviSynth script";
            // 
            // openFilterDialog
            // 
            this.openFilterDialog.Filter = "AviSynth Filters|*.dll";
            this.openFilterDialog.Title = "Select an AviSynth Filter";
            // 
            // openSubsDialog
            // 
            this.openSubsDialog.Filter = "Subs Files|*.srt;*.ass;*.ssa;*.idx";
            this.openSubsDialog.Title = "Select a subtitle file";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deintProgressBar,
            this.deintStatusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 511);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(463, 22);
            this.statusStrip1.TabIndex = 6;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // deintProgressBar
            // 
            this.deintProgressBar.Name = "deintProgressBar";
            this.deintProgressBar.Size = new System.Drawing.Size(100, 16);
            // 
            // deintStatusLabel
            // 
            this.deintStatusLabel.Name = "deintStatusLabel";
            this.deintStatusLabel.Size = new System.Drawing.Size(0, 17);
            // 
            // onSaveLoadScript
            // 
            this.onSaveLoadScript.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.onSaveLoadScript.AutoSize = true;
            this.onSaveLoadScript.Checked = true;
            this.onSaveLoadScript.CheckState = System.Windows.Forms.CheckState.Checked;
            this.onSaveLoadScript.Location = new System.Drawing.Point(84, 478);
            this.onSaveLoadScript.Name = "onSaveLoadScript";
            this.onSaveLoadScript.Size = new System.Drawing.Size(210, 17);
            this.onSaveLoadScript.TabIndex = 18;
            this.onSaveLoadScript.Text = "On Save close and load to be encoded";
            // 
            // saveButton
            // 
            this.saveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.saveButton.AutoSize = true;
            this.saveButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.saveButton.Location = new System.Drawing.Point(413, 475);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(41, 23);
            this.saveButton.TabIndex = 20;
            this.saveButton.Text = "Save";
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // previewAvsButton
            // 
            this.previewAvsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.previewAvsButton.AutoSize = true;
            this.previewAvsButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.previewAvsButton.Location = new System.Drawing.Point(300, 475);
            this.previewAvsButton.Name = "previewAvsButton";
            this.previewAvsButton.Size = new System.Drawing.Size(107, 23);
            this.previewAvsButton.TabIndex = 19;
            this.previewAvsButton.Text = "Preview AVS Script";
            this.previewAvsButton.Click += new System.EventHandler(this.previewButton_Click);
            // 
            // helpButton1
            // 
            this.helpButton1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.helpButton1.ArticleName = "Tools/Avisynth_Script_Creator";
            this.helpButton1.AutoSize = true;
            this.helpButton1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.helpButton1.Location = new System.Drawing.Point(15, 472);
            this.helpButton1.Name = "helpButton1";
            this.helpButton1.Size = new System.Drawing.Size(38, 23);
            this.helpButton1.TabIndex = 17;
            // 
            // AviSynthWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(463, 533);
            this.Controls.Add(this.onSaveLoadScript);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.previewAvsButton);
            this.Controls.Add(this.helpButton1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.tabControl1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "AviSynthWindow";
            this.Text = "MeGUI - AviSynth script creator";
            this.Shown += new System.EventHandler(this.AviSynthWindow_Shown);
            this.resNCropGroupbox.ResumeLayout(false);
            this.resNCropGroupbox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cropLeft)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cropRight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cropBottom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cropTop)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.verticalResolution)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.horizontalResolution)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.optionsTab.ResumeLayout(false);
            this.gbOutput.ResumeLayout(false);
            this.videoGroupBox.ResumeLayout(false);
            this.videoGroupBox.PerformLayout();
            this.filterTab.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabSources.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.mpegOptGroupBox.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.aviOptGroupBox.ResumeLayout(false);
            this.aviOptGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fpsBox)).EndInit();
            this.tabPage3.ResumeLayout(false);
            this.dgOptions.ResumeLayout(false);
            this.dgOptions.PerformLayout();
            this.deinterlacingGroupBox.ResumeLayout(false);
            this.deinterlacingGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.deintM)).EndInit();
            this.filtersGroupbox.ResumeLayout(false);
            this.editTab.ResumeLayout(false);
            this.editTab.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private GroupBox resNCropGroupbox;
        private CheckBox crop;
        private Button autoCropButton;
        private TabControl tabControl1;
        private TabPage optionsTab;
        private TabPage editTab;
        private GroupBox videoGroupBox;
        private Label videoInputLabel;
        private Label inputDARLabel;
        private Label tvTypeLabel;
        private SaveFileDialog saveAvisynthScriptDialog;
        private TextBox avisynthScript;
        private NumericUpDown horizontalResolution;
        private NumericUpDown verticalResolution;
        private NumericUpDown cropLeft;
        private NumericUpDown cropRight;
        private NumericUpDown cropBottom;
        private NumericUpDown cropTop;
        private OpenFileDialog openFilterDialog;
        private OpenFileDialog openSubsDialog;
        private CheckBox suggestResolution;
        private CheckBox signalAR;

        private List<Control> controlsToDisable;
        private TabPage filterTab;
        private GroupBox deinterlacingGroupBox;
        private CheckBox deintIsAnime;
        private Button analyseButton;
        private CheckBox deinterlace;
        private ComboBox deinterlaceType;
        private GroupBox filtersGroupbox;
        private ComboBox noiseFilterType;
        private CheckBox noiseFilter;
        private ComboBox resizeFilterType;
        private Label resizeFilterLabel;
        private CheckBox resize;
        private ComboBox mod16Box;
        private Label label1;
        private Button openDLLButton;
        private TextBox dllPath;
        private ComboBox deintFieldOrder;
        private ComboBox deintSourceType;
        private NumericUpDown deintM;
        private StatusStrip statusStrip1;
        private ToolStripProgressBar deintProgressBar;
        private ToolStripStatusLabel deintStatusLabel;
        private Button reopenOriginal;
        private MeGUI.core.gui.ARChooser arChooser;
        private MeGUI.core.gui.ConfigableProfilesControl avsProfile;
        private Label label6;
        private CheckBox chAutoPreview;
        private GroupBox gbOutput;
        private Label label7;
        private FileBar videoOutput;
        private FileBar input;
        private CheckBox onSaveLoadScript;
        private Button saveButton;
        private Button previewAvsButton;
        private MeGUI.core.gui.HelpButton helpButton1;
        private TabControl tabSources;
        private TabPage tabPage1;
        private GroupBox mpegOptGroupBox;
        private CheckBox colourCorrect;
        private CheckBox mpeg2Deblocking;
        private TabPage tabPage2;
        private GroupBox aviOptGroupBox;
        private NumericUpDown fpsBox;
        private Label fpsLabel;
        private CheckBox flipVertical;
        private TabPage tabPage3;
        private GroupBox dgOptions;
        private ComboBox cbNvDeInt;
        private CheckBox nvDeInt;
        private CheckBox dss2;
        private CheckBox nvResize;
        private ComboBox modValueBox;
        private Label lblAR;
        private Label lblAspectError;
        private GroupBox groupBox1;
        private TextBox SubtitlesPath;
        private ComboBox cbCharset;
        private Label label8;
        private Button openSubtitlesButton;
        private Button deleteSubtitle;
        private Label label9;
    }
}