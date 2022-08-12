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

namespace MeGUI.packages.video.x265
{
    partial class x265ConfigurationPanel
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
            this.x265CodecGeneralGroupbox = new System.Windows.Forms.GroupBox();
            this.x265BitrateQuantizer = new System.Windows.Forms.NumericUpDown();
            this.x265EncodingMode = new System.Windows.Forms.ComboBox();
            this.x265BitrateQuantizerLabel = new System.Windows.Forms.Label();
            this.FrameTypeTabPage = new System.Windows.Forms.TabPage();
            this.gbSlicing = new System.Windows.Forms.GroupBox();
            this.maxSliceSizeMB = new System.Windows.Forms.NumericUpDown();
            this.label11 = new System.Windows.Forms.Label();
            this.maxSliceSizeBytes = new System.Windows.Forms.NumericUpDown();
            this.label10 = new System.Windows.Forms.Label();
            this.slicesnb = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.gbFTOther = new System.Windows.Forms.GroupBox();
            this.cbInterlaceMode = new System.Windows.Forms.ComboBox();
            this.lblInterlacedMode = new System.Windows.Forms.Label();
            this.x265PullDown = new System.Windows.Forms.ComboBox();
            this.pullDownLabel = new System.Windows.Forms.Label();
            this.lblWeightedP = new System.Windows.Forms.Label();
            this.x265WeightedPPrediction = new System.Windows.Forms.ComboBox();
            this.lbExtraIFframes = new System.Windows.Forms.Label();
            this.scenecut = new System.Windows.Forms.CheckBox();
            this.x265NumberOfRefFrames = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.x265SCDSensitivity = new System.Windows.Forms.NumericUpDown();
            this.x265GeneralBFramesgGroupbox = new System.Windows.Forms.GroupBox();
            this.label12 = new System.Windows.Forms.Label();
            this.cbBPyramid = new System.Windows.Forms.ComboBox();
            this.x265WeightedBPrediction = new System.Windows.Forms.CheckBox();
            this.x265BframeBias = new System.Windows.Forms.NumericUpDown();
            this.x265BframeBiasLabel = new System.Windows.Forms.Label();
            this.x265AdaptiveBframesLabel = new System.Windows.Forms.Label();
            this.x265NewAdaptiveBframes = new System.Windows.Forms.ComboBox();
            this.x265NumberOfBFramesLabel = new System.Windows.Forms.Label();
            this.x265NumberOfBFrames = new System.Windows.Forms.NumericUpDown();
            this.gbH264Features = new System.Windows.Forms.GroupBox();
            this.cabac = new System.Windows.Forms.CheckBox();
            this.x265BetaDeblock = new System.Windows.Forms.NumericUpDown();
            this.x265AlphaDeblock = new System.Windows.Forms.NumericUpDown();
            this.x265DeblockActive = new System.Windows.Forms.CheckBox();
            this.x265BetaDeblockLabel = new System.Windows.Forms.Label();
            this.x265AlphaDeblockLabel = new System.Windows.Forms.Label();
            this.gbGOPSize = new System.Windows.Forms.GroupBox();
            this.label19 = new System.Windows.Forms.Label();
            this.cbGOPCalculation = new System.Windows.Forms.ComboBox();
            this.chkOpenGop = new System.Windows.Forms.CheckBox();
            this.x265KeyframeIntervalLabel = new System.Windows.Forms.Label();
            this.x265KeyframeInterval = new System.Windows.Forms.NumericUpDown();
            this.x265MinGOPSize = new System.Windows.Forms.NumericUpDown();
            this.x265MinGOPSizeLabel = new System.Windows.Forms.Label();
            this.RCTabPage = new System.Windows.Forms.TabPage();
            this.x265RCGroupbox = new System.Windows.Forms.GroupBox();
            this.mbtree = new System.Windows.Forms.CheckBox();
            this.label8 = new System.Windows.Forms.Label();
            this.lookahead = new System.Windows.Forms.NumericUpDown();
            this.x265RateTolLabel = new System.Windows.Forms.Label();
            this.x265VBVInitialBuffer = new System.Windows.Forms.NumericUpDown();
            this.x265VBVInitialBufferLabel = new System.Windows.Forms.Label();
            this.x265VBVMaxRate = new System.Windows.Forms.NumericUpDown();
            this.x265TempQuantBlur = new System.Windows.Forms.NumericUpDown();
            this.x265TempFrameComplexityBlur = new System.Windows.Forms.NumericUpDown();
            this.x265QuantizerCompression = new System.Windows.Forms.NumericUpDown();
            this.x265VBVBufferSize = new System.Windows.Forms.NumericUpDown();
            this.x265TempQuantBlurLabel = new System.Windows.Forms.Label();
            this.x265TempFrameComplexityBlurLabel = new System.Windows.Forms.Label();
            this.x265QuantizerCompressionLabel = new System.Windows.Forms.Label();
            this.x265VBVMaxRateLabel = new System.Windows.Forms.Label();
            this.x265VBVBufferSizeLabel = new System.Windows.Forms.Label();
            this.x265RateTol = new System.Windows.Forms.NumericUpDown();
            this.gbAQ = new System.Windows.Forms.GroupBox();
            this.numAQStrength = new System.Windows.Forms.NumericUpDown();
            this.lbAQStrength = new System.Windows.Forms.Label();
            this.cbAQMode = new System.Windows.Forms.ComboBox();
            this.lbAQMode = new System.Windows.Forms.Label();
            this.quantizerMatrixGroupbox = new System.Windows.Forms.GroupBox();
            this.cqmComboBox1 = new MeGUI.core.gui.FileSCBox();
            this.x265QuantizerGroupBox = new System.Windows.Forms.GroupBox();
            this.deadzoneIntra = new System.Windows.Forms.NumericUpDown();
            this.deadzoneInter = new System.Windows.Forms.NumericUpDown();
            this.lbx265DeadZones = new System.Windows.Forms.Label();
            this.x265PBFrameFactor = new System.Windows.Forms.NumericUpDown();
            this.x265IPFrameFactor = new System.Windows.Forms.NumericUpDown();
            this.lbQuantizersRatio = new System.Windows.Forms.Label();
            this.x265CreditsQuantizer = new System.Windows.Forms.NumericUpDown();
            this.x265CreditsQuantizerLabel = new System.Windows.Forms.Label();
            this.x265ChromaQPOffset = new System.Windows.Forms.NumericUpDown();
            this.x265ChromaQPOffsetLabel = new System.Windows.Forms.Label();
            this.x265MaxQuantDelta = new System.Windows.Forms.NumericUpDown();
            this.x265MaximumQuantizer = new System.Windows.Forms.NumericUpDown();
            this.x265MinimimQuantizer = new System.Windows.Forms.NumericUpDown();
            this.x265MinimimQuantizerLabel = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.customCommandlineOptionsLabel = new System.Windows.Forms.Label();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.helpButton1 = new MeGUI.core.gui.HelpButton();
            this.gbPresets = new System.Windows.Forms.GroupBox();
            this.lbPreset = new System.Windows.Forms.Label();
            this.tbx265Presets = new System.Windows.Forms.TrackBar();
            this.gbTunes = new System.Windows.Forms.GroupBox();
            this.x265Tunes = new System.Windows.Forms.ComboBox();
            this.AnalysisTabPage = new System.Windows.Forms.TabPage();
            this.x265Bluray = new System.Windows.Forms.GroupBox();
            this.chkBlurayCompat = new System.Windows.Forms.CheckBox();
            this.fakeInterlaced = new System.Windows.Forms.CheckBox();
            this.x265hrdLabel = new System.Windows.Forms.Label();
            this.x265hrd = new System.Windows.Forms.ComboBox();
            this.x265aud = new System.Windows.Forms.CheckBox();
            this.x265QuantOptionsGroupbox = new System.Windows.Forms.GroupBox();
            this.label16 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.NoiseReduction = new System.Windows.Forms.NumericUpDown();
            this.NoiseReductionLabel = new System.Windows.Forms.Label();
            this.nopsy = new System.Windows.Forms.CheckBox();
            this.x265MixedReferences = new System.Windows.Forms.CheckBox();
            this.x265BframePredictionMode = new System.Windows.Forms.ComboBox();
            this.x265BframePredictionModeLabel = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.PsyTrellis = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.PsyRD = new System.Windows.Forms.NumericUpDown();
            this.noDCTDecimateOption = new System.Windows.Forms.CheckBox();
            this.noFastPSkip = new System.Windows.Forms.CheckBox();
            this.trellis = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.x265MBGroupbox = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.macroblockOptions = new System.Windows.Forms.ComboBox();
            this.adaptiveDCT = new System.Windows.Forms.CheckBox();
            this.x265I4x4mv = new System.Windows.Forms.CheckBox();
            this.x265I8x8mv = new System.Windows.Forms.CheckBox();
            this.x265P4x4mv = new System.Windows.Forms.CheckBox();
            this.x265B8x8mv = new System.Windows.Forms.CheckBox();
            this.x265P8x8mv = new System.Windows.Forms.CheckBox();
            this.x265OtherOptionsGroupbox = new System.Windows.Forms.GroupBox();
            this.label17 = new System.Windows.Forms.Label();
            this.x265SubpelRefinement = new System.Windows.Forms.ComboBox();
            this.x265SubpelRefinementLabel = new System.Windows.Forms.Label();
            this.x265ChromaMe = new System.Windows.Forms.CheckBox();
            this.x265MERangeLabel = new System.Windows.Forms.Label();
            this.x265METypeLabel = new System.Windows.Forms.Label();
            this.x265METype = new System.Windows.Forms.ComboBox();
            this.x265MERange = new System.Windows.Forms.NumericUpDown();
            this.advancedSettings = new System.Windows.Forms.CheckBox();
            this.PsyTrellisLabel = new System.Windows.Forms.Label();
            this.PsyRDLabel = new System.Windows.Forms.Label();
            this.x265NumberOfRefFramesLabel = new System.Windows.Forms.Label();
            this.trellisLabel = new System.Windows.Forms.Label();
            this.MiscTabPage = new System.Windows.Forms.TabPage();
            this.gbx265CustomCmd = new System.Windows.Forms.GroupBox();
            this.customCommandlineOptions = new System.Windows.Forms.TextBox();
            this.x265ThreadsLabel = new System.Windows.Forms.Label();
            this.x265NbThreads = new System.Windows.Forms.NumericUpDown();
            this.tabControl1.SuspendLayout();
            this.mainTabPage.SuspendLayout();
            this.x265CodecGeneralGroupbox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.x265BitrateQuantizer)).BeginInit();
            this.FrameTypeTabPage.SuspendLayout();
            this.gbSlicing.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.maxSliceSizeMB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxSliceSizeBytes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.slicesnb)).BeginInit();
            this.gbFTOther.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.x265NumberOfRefFrames)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.x265SCDSensitivity)).BeginInit();
            this.x265GeneralBFramesgGroupbox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.x265BframeBias)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.x265NumberOfBFrames)).BeginInit();
            this.gbH264Features.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.x265BetaDeblock)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.x265AlphaDeblock)).BeginInit();
            this.gbGOPSize.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.x265KeyframeInterval)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.x265MinGOPSize)).BeginInit();
            this.RCTabPage.SuspendLayout();
            this.x265RCGroupbox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lookahead)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.x265VBVInitialBuffer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.x265VBVMaxRate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.x265TempQuantBlur)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.x265TempFrameComplexityBlur)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.x265QuantizerCompression)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.x265VBVBufferSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.x265RateTol)).BeginInit();
            this.gbAQ.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numAQStrength)).BeginInit();
            this.quantizerMatrixGroupbox.SuspendLayout();
            this.x265QuantizerGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.deadzoneIntra)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.deadzoneInter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.x265PBFrameFactor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.x265IPFrameFactor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.x265CreditsQuantizer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.x265ChromaQPOffset)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.x265MaxQuantDelta)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.x265MaximumQuantizer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.x265MinimimQuantizer)).BeginInit();
            this.gbPresets.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbx265Presets)).BeginInit();
            this.gbTunes.SuspendLayout();
            this.AnalysisTabPage.SuspendLayout();
            this.x265Bluray.SuspendLayout();
            this.x265QuantOptionsGroupbox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NoiseReduction)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PsyTrellis)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PsyRD)).BeginInit();
            this.x265MBGroupbox.SuspendLayout();
            this.x265OtherOptionsGroupbox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.x265MERange)).BeginInit();
            this.MiscTabPage.SuspendLayout();
            this.gbx265CustomCmd.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.x265NbThreads)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.FrameTypeTabPage);
            this.tabControl1.Controls.Add(this.RCTabPage);
            this.tabControl1.Controls.Add(this.AnalysisTabPage);
            this.tabControl1.Controls.Add(this.MiscTabPage);
            this.tabControl1.Size = new System.Drawing.Size(510, 268);
            this.tabControl1.SizeMode = System.Windows.Forms.TabSizeMode.FillToRight;
            this.tabControl1.TabIndex = 0;
            this.tabControl1.Controls.SetChildIndex(this.MiscTabPage, 0);
            this.tabControl1.Controls.SetChildIndex(this.AnalysisTabPage, 0);
            this.tabControl1.Controls.SetChildIndex(this.RCTabPage, 0);
            this.tabControl1.Controls.SetChildIndex(this.FrameTypeTabPage, 0);
            this.tabControl1.Controls.SetChildIndex(this.mainTabPage, 0);
            // 
            // commandline
            // 
            this.commandline.Location = new System.Drawing.Point(0, 270);
            this.commandline.Size = new System.Drawing.Size(507, 89);
            this.commandline.TabIndex = 1;
            this.commandline.Text = " ";
            // 
            // mainTabPage
            // 
            this.mainTabPage.Controls.Add(this.x265ThreadsLabel);
            this.mainTabPage.Controls.Add(this.x265NbThreads);
            this.mainTabPage.Controls.Add(this.advancedSettings);
            this.mainTabPage.Controls.Add(this.gbTunes);
            this.mainTabPage.Controls.Add(this.gbPresets);
            this.mainTabPage.Controls.Add(this.helpButton1);
            this.mainTabPage.Controls.Add(this.x265CodecGeneralGroupbox);
            this.mainTabPage.Size = new System.Drawing.Size(502, 242);
            // 
            // x265CodecGeneralGroupbox
            // 
            this.x265CodecGeneralGroupbox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.x265CodecGeneralGroupbox.Controls.Add(this.x265BitrateQuantizer);
            this.x265CodecGeneralGroupbox.Controls.Add(this.x265EncodingMode);
            this.x265CodecGeneralGroupbox.Controls.Add(this.x265BitrateQuantizerLabel);
            this.x265CodecGeneralGroupbox.Location = new System.Drawing.Point(6, 17);
            this.x265CodecGeneralGroupbox.Name = "x265CodecGeneralGroupbox";
            this.x265CodecGeneralGroupbox.Size = new System.Drawing.Size(310, 48);
            this.x265CodecGeneralGroupbox.TabIndex = 0;
            this.x265CodecGeneralGroupbox.TabStop = false;
            this.x265CodecGeneralGroupbox.Text = " Encoding Mode ";
            // 
            // x265BitrateQuantizer
            // 
            this.x265BitrateQuantizer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.x265BitrateQuantizer.Location = new System.Drawing.Point(249, 19);
            this.x265BitrateQuantizer.Maximum = new decimal(new int[] {
            500000,
            0,
            0,
            0});
            this.x265BitrateQuantizer.Name = "x265BitrateQuantizer";
            this.x265BitrateQuantizer.Size = new System.Drawing.Size(55, 20);
            this.x265BitrateQuantizer.TabIndex = 5;
            this.x265BitrateQuantizer.ValueChanged += new System.EventHandler(this.updateEvent);
            // 
            // x265EncodingMode
            // 
            this.x265EncodingMode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.x265EncodingMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.x265EncodingMode.Items.AddRange(new object[] {
            "ABR",
            "Const. Quantizer",
            "2pass - 1st pass",
            "2pass - 2nd pass",
            "Automated 2pass",
            "3pass - 1st pass",
            "3pass - 2nd pass",
            "3pass - 3rd pass",
            "Automated 3pass",
            "Const. Quality"});
            this.x265EncodingMode.Location = new System.Drawing.Point(15, 19);
            this.x265EncodingMode.MaxDropDownItems = 2;
            this.x265EncodingMode.Name = "x265EncodingMode";
            this.x265EncodingMode.Size = new System.Drawing.Size(121, 21);
            this.x265EncodingMode.TabIndex = 2;
            this.x265EncodingMode.SelectedIndexChanged += new System.EventHandler(this.updateEvent);
            // 
            // x265BitrateQuantizerLabel
            // 
            this.x265BitrateQuantizerLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.x265BitrateQuantizerLabel.Location = new System.Drawing.Point(182, 17);
            this.x265BitrateQuantizerLabel.Margin = new System.Windows.Forms.Padding(3);
            this.x265BitrateQuantizerLabel.Name = "x265BitrateQuantizerLabel";
            this.x265BitrateQuantizerLabel.Padding = new System.Windows.Forms.Padding(3);
            this.x265BitrateQuantizerLabel.Size = new System.Drawing.Size(69, 23);
            this.x265BitrateQuantizerLabel.TabIndex = 3;
            this.x265BitrateQuantizerLabel.Text = "Bitrate";
            this.x265BitrateQuantizerLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // FrameTypeTabPage
            // 
            this.FrameTypeTabPage.Controls.Add(this.gbSlicing);
            this.FrameTypeTabPage.Controls.Add(this.gbFTOther);
            this.FrameTypeTabPage.Controls.Add(this.x265GeneralBFramesgGroupbox);
            this.FrameTypeTabPage.Controls.Add(this.gbH264Features);
            this.FrameTypeTabPage.Controls.Add(this.gbGOPSize);
            this.FrameTypeTabPage.Location = new System.Drawing.Point(4, 22);
            this.FrameTypeTabPage.Name = "FrameTypeTabPage";
            this.FrameTypeTabPage.Size = new System.Drawing.Size(502, 242);
            this.FrameTypeTabPage.TabIndex = 3;
            this.FrameTypeTabPage.Text = "Frame-Type";
            this.FrameTypeTabPage.UseVisualStyleBackColor = true;
            // 
            // gbSlicing
            // 
            this.gbSlicing.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.gbSlicing.Controls.Add(this.maxSliceSizeMB);
            this.gbSlicing.Controls.Add(this.label11);
            this.gbSlicing.Controls.Add(this.maxSliceSizeBytes);
            this.gbSlicing.Controls.Add(this.label10);
            this.gbSlicing.Controls.Add(this.slicesnb);
            this.gbSlicing.Controls.Add(this.label9);
            this.gbSlicing.Location = new System.Drawing.Point(3, 287);
            this.gbSlicing.Name = "gbSlicing";
            this.gbSlicing.Size = new System.Drawing.Size(250, 113);
            this.gbSlicing.TabIndex = 14;
            this.gbSlicing.TabStop = false;
            this.gbSlicing.Text = " Slicing ";
            // 
            // maxSliceSizeMB
            // 
            this.maxSliceSizeMB.Location = new System.Drawing.Point(159, 79);
            this.maxSliceSizeMB.Name = "maxSliceSizeMB";
            this.maxSliceSizeMB.Size = new System.Drawing.Size(85, 20);
            this.maxSliceSizeMB.TabIndex = 5;
            this.maxSliceSizeMB.ValueChanged += new System.EventHandler(this.updateEvent);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(14, 81);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(87, 13);
            this.label11.TabIndex = 4;
            this.label11.Text = "Max size (in mbs)";
            // 
            // maxSliceSizeBytes
            // 
            this.maxSliceSizeBytes.Location = new System.Drawing.Point(159, 50);
            this.maxSliceSizeBytes.Maximum = new decimal(new int[] {
            250,
            0,
            0,
            0});
            this.maxSliceSizeBytes.Name = "maxSliceSizeBytes";
            this.maxSliceSizeBytes.Size = new System.Drawing.Size(85, 20);
            this.maxSliceSizeBytes.TabIndex = 3;
            this.maxSliceSizeBytes.ValueChanged += new System.EventHandler(this.updateEvent);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(14, 52);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(93, 13);
            this.label10.TabIndex = 2;
            this.label10.Text = "Max size (in bytes)";
            // 
            // slicesnb
            // 
            this.slicesnb.Location = new System.Drawing.Point(159, 21);
            this.slicesnb.Name = "slicesnb";
            this.slicesnb.Size = new System.Drawing.Size(85, 20);
            this.slicesnb.TabIndex = 1;
            this.slicesnb.ValueChanged += new System.EventHandler(this.updateEvent);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(14, 25);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(108, 13);
            this.label9.TabIndex = 0;
            this.label9.Text = "Nb of slices by Frame";
            // 
            // gbFTOther
            // 
            this.gbFTOther.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbFTOther.Controls.Add(this.cbInterlaceMode);
            this.gbFTOther.Controls.Add(this.lblInterlacedMode);
            this.gbFTOther.Controls.Add(this.x265PullDown);
            this.gbFTOther.Controls.Add(this.pullDownLabel);
            this.gbFTOther.Controls.Add(this.lblWeightedP);
            this.gbFTOther.Controls.Add(this.x265WeightedPPrediction);
            this.gbFTOther.Controls.Add(this.lbExtraIFframes);
            this.gbFTOther.Controls.Add(this.scenecut);
            this.gbFTOther.Controls.Add(this.x265NumberOfRefFrames);
            this.gbFTOther.Controls.Add(this.label6);
            this.gbFTOther.Controls.Add(this.x265SCDSensitivity);
            this.gbFTOther.Location = new System.Drawing.Point(259, 193);
            this.gbFTOther.Name = "gbFTOther";
            this.gbFTOther.Size = new System.Drawing.Size(240, 207);
            this.gbFTOther.TabIndex = 13;
            this.gbFTOther.TabStop = false;
            this.gbFTOther.Text = " Other ";
            // 
            // cbInterlaceMode
            // 
            this.cbInterlaceMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbInterlaceMode.FormattingEnabled = true;
            this.cbInterlaceMode.Items.AddRange(new object[] {
            "none",
            "TFF",
            "BFF"});
            this.cbInterlaceMode.Location = new System.Drawing.Point(167, 108);
            this.cbInterlaceMode.Name = "cbInterlaceMode";
            this.cbInterlaceMode.Size = new System.Drawing.Size(65, 21);
            this.cbInterlaceMode.TabIndex = 26;
            this.cbInterlaceMode.SelectedIndexChanged += new System.EventHandler(this.updateEvent);
            // 
            // lblInterlacedMode
            // 
            this.lblInterlacedMode.AutoSize = true;
            this.lblInterlacedMode.Location = new System.Drawing.Point(9, 111);
            this.lblInterlacedMode.Name = "lblInterlacedMode";
            this.lblInterlacedMode.Size = new System.Drawing.Size(84, 13);
            this.lblInterlacedMode.TabIndex = 25;
            this.lblInterlacedMode.Text = "Interlaced Mode";
            this.lblInterlacedMode.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // x265PullDown
            // 
            this.x265PullDown.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.x265PullDown.FormattingEnabled = true;
            this.x265PullDown.Items.AddRange(new object[] {
            "none",
            "22",
            "32",
            "64",
            "double",
            "triple",
            "euro"});
            this.x265PullDown.Location = new System.Drawing.Point(167, 134);
            this.x265PullDown.Name = "x265PullDown";
            this.x265PullDown.Size = new System.Drawing.Size(65, 21);
            this.x265PullDown.TabIndex = 24;
            this.x265PullDown.SelectedIndexChanged += new System.EventHandler(this.updateEvent);
            // 
            // pullDownLabel
            // 
            this.pullDownLabel.AutoSize = true;
            this.pullDownLabel.Location = new System.Drawing.Point(9, 137);
            this.pullDownLabel.Name = "pullDownLabel";
            this.pullDownLabel.Size = new System.Drawing.Size(50, 13);
            this.pullDownLabel.TabIndex = 23;
            this.pullDownLabel.Text = "Pulldown";
            // 
            // lblWeightedP
            // 
            this.lblWeightedP.AutoSize = true;
            this.lblWeightedP.Location = new System.Drawing.Point(9, 84);
            this.lblWeightedP.Name = "lblWeightedP";
            this.lblWeightedP.Size = new System.Drawing.Size(142, 13);
            this.lblWeightedP.TabIndex = 22;
            this.lblWeightedP.Text = "P-frame Weighted Prediction";
            this.lblWeightedP.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // x265WeightedPPrediction
            // 
            this.x265WeightedPPrediction.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.x265WeightedPPrediction.FormattingEnabled = true;
            this.x265WeightedPPrediction.Items.AddRange(new object[] {
            "Disabled",
            "Blind",
            "Smart"});
            this.x265WeightedPPrediction.Location = new System.Drawing.Point(167, 81);
            this.x265WeightedPPrediction.Name = "x265WeightedPPrediction";
            this.x265WeightedPPrediction.Size = new System.Drawing.Size(65, 21);
            this.x265WeightedPPrediction.TabIndex = 21;
            this.x265WeightedPPrediction.SelectedIndexChanged += new System.EventHandler(this.updateEvent);
            // 
            // lbExtraIFframes
            // 
            this.lbExtraIFframes.AutoSize = true;
            this.lbExtraIFframes.Location = new System.Drawing.Point(9, 52);
            this.lbExtraIFframes.Name = "lbExtraIFframes";
            this.lbExtraIFframes.Size = new System.Drawing.Size(126, 13);
            this.lbExtraIFframes.TabIndex = 20;
            this.lbExtraIFframes.Text = "Number of Extra I-Frames";
            this.lbExtraIFframes.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // scenecut
            // 
            this.scenecut.Checked = true;
            this.scenecut.CheckState = System.Windows.Forms.CheckState.Checked;
            this.scenecut.Location = new System.Drawing.Point(12, 161);
            this.scenecut.Name = "scenecut";
            this.scenecut.Size = new System.Drawing.Size(163, 24);
            this.scenecut.TabIndex = 19;
            this.scenecut.Text = "Adaptive I-Frame Decision";
            this.scenecut.UseVisualStyleBackColor = true;
            this.scenecut.CheckedChanged += new System.EventHandler(this.updateEvent);
            // 
            // x265NumberOfRefFrames
            // 
            this.x265NumberOfRefFrames.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.x265NumberOfRefFrames.Location = new System.Drawing.Point(181, 21);
            this.x265NumberOfRefFrames.Maximum = new decimal(new int[] {
            16,
            0,
            0,
            0});
            this.x265NumberOfRefFrames.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.x265NumberOfRefFrames.Name = "x265NumberOfRefFrames";
            this.x265NumberOfRefFrames.Size = new System.Drawing.Size(51, 20);
            this.x265NumberOfRefFrames.TabIndex = 18;
            this.x265NumberOfRefFrames.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.x265NumberOfRefFrames.ValueChanged += new System.EventHandler(this.updateEvent);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(9, 23);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(146, 13);
            this.label6.TabIndex = 17;
            this.label6.Text = "Number of Reference Frames";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // x265SCDSensitivity
            // 
            this.x265SCDSensitivity.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.x265SCDSensitivity.Location = new System.Drawing.Point(181, 50);
            this.x265SCDSensitivity.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.x265SCDSensitivity.Name = "x265SCDSensitivity";
            this.x265SCDSensitivity.Size = new System.Drawing.Size(51, 20);
            this.x265SCDSensitivity.TabIndex = 16;
            this.x265SCDSensitivity.Value = new decimal(new int[] {
            40,
            0,
            0,
            0});
            this.x265SCDSensitivity.ValueChanged += new System.EventHandler(this.updateEvent);
            // 
            // x265GeneralBFramesgGroupbox
            // 
            this.x265GeneralBFramesgGroupbox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.x265GeneralBFramesgGroupbox.Controls.Add(this.label12);
            this.x265GeneralBFramesgGroupbox.Controls.Add(this.cbBPyramid);
            this.x265GeneralBFramesgGroupbox.Controls.Add(this.x265WeightedBPrediction);
            this.x265GeneralBFramesgGroupbox.Controls.Add(this.x265BframeBias);
            this.x265GeneralBFramesgGroupbox.Controls.Add(this.x265BframeBiasLabel);
            this.x265GeneralBFramesgGroupbox.Controls.Add(this.x265AdaptiveBframesLabel);
            this.x265GeneralBFramesgGroupbox.Controls.Add(this.x265NewAdaptiveBframes);
            this.x265GeneralBFramesgGroupbox.Controls.Add(this.x265NumberOfBFramesLabel);
            this.x265GeneralBFramesgGroupbox.Controls.Add(this.x265NumberOfBFrames);
            this.x265GeneralBFramesgGroupbox.Location = new System.Drawing.Point(259, 3);
            this.x265GeneralBFramesgGroupbox.Name = "x265GeneralBFramesgGroupbox";
            this.x265GeneralBFramesgGroupbox.Size = new System.Drawing.Size(240, 184);
            this.x265GeneralBFramesgGroupbox.TabIndex = 7;
            this.x265GeneralBFramesgGroupbox.TabStop = false;
            this.x265GeneralBFramesgGroupbox.Text = " B-Frames ";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(14, 155);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(54, 13);
            this.label12.TabIndex = 20;
            this.label12.Text = "B-Pyramid";
            // 
            // cbBPyramid
            // 
            this.cbBPyramid.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbBPyramid.FormattingEnabled = true;
            this.cbBPyramid.Items.AddRange(new object[] {
            "Disabled",
            "Strict",
            "Normal"});
            this.cbBPyramid.Location = new System.Drawing.Point(149, 152);
            this.cbBPyramid.Name = "cbBPyramid";
            this.cbBPyramid.Size = new System.Drawing.Size(83, 21);
            this.cbBPyramid.TabIndex = 19;
            this.cbBPyramid.SelectedIndexChanged += new System.EventHandler(this.updateEvent);
            // 
            // x265WeightedBPrediction
            // 
            this.x265WeightedBPrediction.Checked = true;
            this.x265WeightedBPrediction.CheckState = System.Windows.Forms.CheckState.Checked;
            this.x265WeightedBPrediction.Location = new System.Drawing.Point(17, 19);
            this.x265WeightedBPrediction.Name = "x265WeightedBPrediction";
            this.x265WeightedBPrediction.Padding = new System.Windows.Forms.Padding(3);
            this.x265WeightedBPrediction.Size = new System.Drawing.Size(194, 23);
            this.x265WeightedBPrediction.TabIndex = 18;
            this.x265WeightedBPrediction.Text = "Weighted Prediction for B-Frames";
            this.x265WeightedBPrediction.CheckedChanged += new System.EventHandler(this.updateEvent);
            // 
            // x265BframeBias
            // 
            this.x265BframeBias.Location = new System.Drawing.Point(149, 80);
            this.x265BframeBias.Minimum = new decimal(new int[] {
            90,
            0,
            0,
            -2147483648});
            this.x265BframeBias.Name = "x265BframeBias";
            this.x265BframeBias.Size = new System.Drawing.Size(85, 20);
            this.x265BframeBias.TabIndex = 16;
            this.x265BframeBias.ValueChanged += new System.EventHandler(this.updateEvent);
            // 
            // x265BframeBiasLabel
            // 
            this.x265BframeBiasLabel.AutoSize = true;
            this.x265BframeBiasLabel.Location = new System.Drawing.Point(14, 82);
            this.x265BframeBiasLabel.Name = "x265BframeBiasLabel";
            this.x265BframeBiasLabel.Size = new System.Drawing.Size(65, 13);
            this.x265BframeBiasLabel.TabIndex = 15;
            this.x265BframeBiasLabel.Text = "B-frame bias";
            this.x265BframeBiasLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // x265AdaptiveBframesLabel
            // 
            this.x265AdaptiveBframesLabel.AutoSize = true;
            this.x265AdaptiveBframesLabel.Location = new System.Drawing.Point(14, 125);
            this.x265AdaptiveBframesLabel.Name = "x265AdaptiveBframesLabel";
            this.x265AdaptiveBframesLabel.Size = new System.Drawing.Size(96, 13);
            this.x265AdaptiveBframesLabel.TabIndex = 12;
            this.x265AdaptiveBframesLabel.Text = "Adaptive B-Frames";
            // 
            // x265NewAdaptiveBframes
            // 
            this.x265NewAdaptiveBframes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.x265NewAdaptiveBframes.Items.AddRange(new object[] {
            "0-Off",
            "1-Fast",
            "2-Optimal"});
            this.x265NewAdaptiveBframes.Location = new System.Drawing.Point(149, 122);
            this.x265NewAdaptiveBframes.Name = "x265NewAdaptiveBframes";
            this.x265NewAdaptiveBframes.Size = new System.Drawing.Size(83, 21);
            this.x265NewAdaptiveBframes.TabIndex = 11;
            this.x265NewAdaptiveBframes.SelectedIndexChanged += new System.EventHandler(this.updateEvent);
            // 
            // x265NumberOfBFramesLabel
            // 
            this.x265NumberOfBFramesLabel.AutoSize = true;
            this.x265NumberOfBFramesLabel.Location = new System.Drawing.Point(14, 56);
            this.x265NumberOfBFramesLabel.Name = "x265NumberOfBFramesLabel";
            this.x265NumberOfBFramesLabel.Size = new System.Drawing.Size(100, 13);
            this.x265NumberOfBFramesLabel.TabIndex = 0;
            this.x265NumberOfBFramesLabel.Text = "Number of B-frames";
            this.x265NumberOfBFramesLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // x265NumberOfBFrames
            // 
            this.x265NumberOfBFrames.Location = new System.Drawing.Point(149, 54);
            this.x265NumberOfBFrames.Maximum = new decimal(new int[] {
            16,
            0,
            0,
            0});
            this.x265NumberOfBFrames.Name = "x265NumberOfBFrames";
            this.x265NumberOfBFrames.Size = new System.Drawing.Size(85, 20);
            this.x265NumberOfBFrames.TabIndex = 1;
            this.x265NumberOfBFrames.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.x265NumberOfBFrames.ValueChanged += new System.EventHandler(this.updateEvent);
            // 
            // gbH264Features
            // 
            this.gbH264Features.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbH264Features.Controls.Add(this.cabac);
            this.gbH264Features.Controls.Add(this.x265BetaDeblock);
            this.gbH264Features.Controls.Add(this.x265AlphaDeblock);
            this.gbH264Features.Controls.Add(this.x265DeblockActive);
            this.gbH264Features.Controls.Add(this.x265BetaDeblockLabel);
            this.gbH264Features.Controls.Add(this.x265AlphaDeblockLabel);
            this.gbH264Features.Location = new System.Drawing.Point(3, 3);
            this.gbH264Features.Name = "gbH264Features";
            this.gbH264Features.Size = new System.Drawing.Size(250, 132);
            this.gbH264Features.TabIndex = 4;
            this.gbH264Features.TabStop = false;
            this.gbH264Features.Text = " H.264 Features ";
            // 
            // cabac
            // 
            this.cabac.Checked = true;
            this.cabac.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cabac.Location = new System.Drawing.Point(17, 93);
            this.cabac.Name = "cabac";
            this.cabac.Padding = new System.Windows.Forms.Padding(3);
            this.cabac.Size = new System.Drawing.Size(162, 23);
            this.cabac.TabIndex = 12;
            this.cabac.Text = "CABAC";
            this.cabac.CheckedChanged += new System.EventHandler(this.updateEvent);
            // 
            // x265BetaDeblock
            // 
            this.x265BetaDeblock.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.x265BetaDeblock.Location = new System.Drawing.Point(193, 66);
            this.x265BetaDeblock.Maximum = new decimal(new int[] {
            6,
            0,
            0,
            0});
            this.x265BetaDeblock.Minimum = new decimal(new int[] {
            6,
            0,
            0,
            -2147483648});
            this.x265BetaDeblock.Name = "x265BetaDeblock";
            this.x265BetaDeblock.Size = new System.Drawing.Size(51, 20);
            this.x265BetaDeblock.TabIndex = 4;
            this.x265BetaDeblock.ValueChanged += new System.EventHandler(this.updateEvent);
            // 
            // x265AlphaDeblock
            // 
            this.x265AlphaDeblock.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.x265AlphaDeblock.Location = new System.Drawing.Point(193, 43);
            this.x265AlphaDeblock.Maximum = new decimal(new int[] {
            6,
            0,
            0,
            0});
            this.x265AlphaDeblock.Minimum = new decimal(new int[] {
            6,
            0,
            0,
            -2147483648});
            this.x265AlphaDeblock.Name = "x265AlphaDeblock";
            this.x265AlphaDeblock.Size = new System.Drawing.Size(51, 20);
            this.x265AlphaDeblock.TabIndex = 2;
            this.x265AlphaDeblock.ValueChanged += new System.EventHandler(this.updateEvent);
            // 
            // x265DeblockActive
            // 
            this.x265DeblockActive.Checked = true;
            this.x265DeblockActive.CheckState = System.Windows.Forms.CheckState.Checked;
            this.x265DeblockActive.Location = new System.Drawing.Point(17, 19);
            this.x265DeblockActive.Name = "x265DeblockActive";
            this.x265DeblockActive.Padding = new System.Windows.Forms.Padding(3);
            this.x265DeblockActive.Size = new System.Drawing.Size(156, 23);
            this.x265DeblockActive.TabIndex = 0;
            this.x265DeblockActive.Text = "Deblocking";
            this.x265DeblockActive.CheckedChanged += new System.EventHandler(this.updateEvent);
            // 
            // x265BetaDeblockLabel
            // 
            this.x265BetaDeblockLabel.AutoSize = true;
            this.x265BetaDeblockLabel.Location = new System.Drawing.Point(14, 68);
            this.x265BetaDeblockLabel.Name = "x265BetaDeblockLabel";
            this.x265BetaDeblockLabel.Size = new System.Drawing.Size(111, 13);
            this.x265BetaDeblockLabel.TabIndex = 3;
            this.x265BetaDeblockLabel.Text = "Deblocking Threshold";
            this.x265BetaDeblockLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // x265AlphaDeblockLabel
            // 
            this.x265AlphaDeblockLabel.AutoSize = true;
            this.x265AlphaDeblockLabel.Location = new System.Drawing.Point(14, 45);
            this.x265AlphaDeblockLabel.Name = "x265AlphaDeblockLabel";
            this.x265AlphaDeblockLabel.Size = new System.Drawing.Size(104, 13);
            this.x265AlphaDeblockLabel.TabIndex = 1;
            this.x265AlphaDeblockLabel.Text = "Deblocking Strength";
            this.x265AlphaDeblockLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // gbGOPSize
            // 
            this.gbGOPSize.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbGOPSize.Controls.Add(this.label19);
            this.gbGOPSize.Controls.Add(this.cbGOPCalculation);
            this.gbGOPSize.Controls.Add(this.chkOpenGop);
            this.gbGOPSize.Controls.Add(this.x265KeyframeIntervalLabel);
            this.gbGOPSize.Controls.Add(this.x265KeyframeInterval);
            this.gbGOPSize.Controls.Add(this.x265MinGOPSize);
            this.gbGOPSize.Controls.Add(this.x265MinGOPSizeLabel);
            this.gbGOPSize.Location = new System.Drawing.Point(2, 141);
            this.gbGOPSize.Name = "gbGOPSize";
            this.gbGOPSize.Size = new System.Drawing.Size(251, 140);
            this.gbGOPSize.TabIndex = 1;
            this.gbGOPSize.TabStop = false;
            this.gbGOPSize.Text = " GOP Size ";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(15, 31);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(84, 13);
            this.label19.TabIndex = 13;
            this.label19.Text = "GOP calculation";
            this.label19.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cbGOPCalculation
            // 
            this.cbGOPCalculation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbGOPCalculation.Items.AddRange(new object[] {
            "Fixed",
            "FPS based"});
            this.cbGOPCalculation.Location = new System.Drawing.Point(129, 28);
            this.cbGOPCalculation.Name = "cbGOPCalculation";
            this.cbGOPCalculation.Size = new System.Drawing.Size(116, 21);
            this.cbGOPCalculation.TabIndex = 12;
            this.cbGOPCalculation.SelectedIndexChanged += new System.EventHandler(this.updateEvent);
            // 
            // chkOpenGop
            // 
            this.chkOpenGop.AutoSize = true;
            this.chkOpenGop.Location = new System.Drawing.Point(18, 105);
            this.chkOpenGop.Name = "chkOpenGop";
            this.chkOpenGop.Size = new System.Drawing.Size(78, 17);
            this.chkOpenGop.TabIndex = 5;
            this.chkOpenGop.Text = "Open GOP";
            this.chkOpenGop.UseVisualStyleBackColor = true;
            this.chkOpenGop.CheckedChanged += new System.EventHandler(this.updateEvent);
            // 
            // x265KeyframeIntervalLabel
            // 
            this.x265KeyframeIntervalLabel.AutoSize = true;
            this.x265KeyframeIntervalLabel.Location = new System.Drawing.Point(15, 59);
            this.x265KeyframeIntervalLabel.Name = "x265KeyframeIntervalLabel";
            this.x265KeyframeIntervalLabel.Size = new System.Drawing.Size(158, 13);
            this.x265KeyframeIntervalLabel.TabIndex = 0;
            this.x265KeyframeIntervalLabel.Text = "Maximum GOP Size (0 = Infinite)";
            this.x265KeyframeIntervalLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // x265KeyframeInterval
            // 
            this.x265KeyframeInterval.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.x265KeyframeInterval.Location = new System.Drawing.Point(197, 55);
            this.x265KeyframeInterval.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.x265KeyframeInterval.Name = "x265KeyframeInterval";
            this.x265KeyframeInterval.Size = new System.Drawing.Size(48, 20);
            this.x265KeyframeInterval.TabIndex = 1;
            this.x265KeyframeInterval.Value = new decimal(new int[] {
            250,
            0,
            0,
            0});
            this.x265KeyframeInterval.ValueChanged += new System.EventHandler(this.updateEvent);
            // 
            // x265MinGOPSize
            // 
            this.x265MinGOPSize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.x265MinGOPSize.Location = new System.Drawing.Point(197, 80);
            this.x265MinGOPSize.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.x265MinGOPSize.Name = "x265MinGOPSize";
            this.x265MinGOPSize.Size = new System.Drawing.Size(48, 20);
            this.x265MinGOPSize.TabIndex = 3;
            this.x265MinGOPSize.Value = new decimal(new int[] {
            25,
            0,
            0,
            0});
            this.x265MinGOPSize.ValueChanged += new System.EventHandler(this.updateEvent);
            // 
            // x265MinGOPSizeLabel
            // 
            this.x265MinGOPSizeLabel.AutoSize = true;
            this.x265MinGOPSizeLabel.Location = new System.Drawing.Point(15, 82);
            this.x265MinGOPSizeLabel.Name = "x265MinGOPSizeLabel";
            this.x265MinGOPSizeLabel.Size = new System.Drawing.Size(97, 13);
            this.x265MinGOPSizeLabel.TabIndex = 2;
            this.x265MinGOPSizeLabel.Text = "Minimum GOP Size";
            this.x265MinGOPSizeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // RCTabPage
            // 
            this.RCTabPage.Controls.Add(this.x265RCGroupbox);
            this.RCTabPage.Controls.Add(this.gbAQ);
            this.RCTabPage.Controls.Add(this.quantizerMatrixGroupbox);
            this.RCTabPage.Controls.Add(this.x265QuantizerGroupBox);
            this.RCTabPage.Location = new System.Drawing.Point(4, 22);
            this.RCTabPage.Name = "RCTabPage";
            this.RCTabPage.Size = new System.Drawing.Size(502, 242);
            this.RCTabPage.TabIndex = 4;
            this.RCTabPage.Text = "Rate Control";
            this.RCTabPage.UseVisualStyleBackColor = true;
            // 
            // x265RCGroupbox
            // 
            this.x265RCGroupbox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.x265RCGroupbox.Controls.Add(this.mbtree);
            this.x265RCGroupbox.Controls.Add(this.label8);
            this.x265RCGroupbox.Controls.Add(this.lookahead);
            this.x265RCGroupbox.Controls.Add(this.x265RateTolLabel);
            this.x265RCGroupbox.Controls.Add(this.x265VBVInitialBuffer);
            this.x265RCGroupbox.Controls.Add(this.x265VBVInitialBufferLabel);
            this.x265RCGroupbox.Controls.Add(this.x265VBVMaxRate);
            this.x265RCGroupbox.Controls.Add(this.x265TempQuantBlur);
            this.x265RCGroupbox.Controls.Add(this.x265TempFrameComplexityBlur);
            this.x265RCGroupbox.Controls.Add(this.x265QuantizerCompression);
            this.x265RCGroupbox.Controls.Add(this.x265VBVBufferSize);
            this.x265RCGroupbox.Controls.Add(this.x265TempQuantBlurLabel);
            this.x265RCGroupbox.Controls.Add(this.x265TempFrameComplexityBlurLabel);
            this.x265RCGroupbox.Controls.Add(this.x265QuantizerCompressionLabel);
            this.x265RCGroupbox.Controls.Add(this.x265VBVMaxRateLabel);
            this.x265RCGroupbox.Controls.Add(this.x265VBVBufferSizeLabel);
            this.x265RCGroupbox.Controls.Add(this.x265RateTol);
            this.x265RCGroupbox.Location = new System.Drawing.Point(3, 192);
            this.x265RCGroupbox.Name = "x265RCGroupbox";
            this.x265RCGroupbox.Size = new System.Drawing.Size(496, 200);
            this.x265RCGroupbox.TabIndex = 22;
            this.x265RCGroupbox.TabStop = false;
            this.x265RCGroupbox.Text = "Rate Control";
            // 
            // mbtree
            // 
            this.mbtree.Checked = true;
            this.mbtree.CheckState = System.Windows.Forms.CheckState.Checked;
            this.mbtree.Location = new System.Drawing.Point(295, 68);
            this.mbtree.Name = "mbtree";
            this.mbtree.Size = new System.Drawing.Size(195, 17);
            this.mbtree.TabIndex = 16;
            this.mbtree.Text = "Use MB-Tree";
            this.mbtree.UseVisualStyleBackColor = true;
            this.mbtree.CheckedChanged += new System.EventHandler(this.updateEvent);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(292, 21);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(142, 13);
            this.label8.TabIndex = 15;
            this.label8.Text = "Nb of Frames for Lookahead";
            // 
            // lookahead
            // 
            this.lookahead.Location = new System.Drawing.Point(443, 19);
            this.lookahead.Maximum = new decimal(new int[] {
            250,
            0,
            0,
            0});
            this.lookahead.Name = "lookahead";
            this.lookahead.Size = new System.Drawing.Size(48, 20);
            this.lookahead.TabIndex = 14;
            this.lookahead.Value = new decimal(new int[] {
            40,
            0,
            0,
            0});
            this.lookahead.ValueChanged += new System.EventHandler(this.updateEvent);
            // 
            // x265RateTolLabel
            // 
            this.x265RateTolLabel.AutoSize = true;
            this.x265RateTolLabel.Location = new System.Drawing.Point(12, 95);
            this.x265RateTolLabel.Name = "x265RateTolLabel";
            this.x265RateTolLabel.Size = new System.Drawing.Size(82, 13);
            this.x265RateTolLabel.TabIndex = 6;
            this.x265RateTolLabel.Text = "Bitrate Variance";
            this.x265RateTolLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // x265VBVInitialBuffer
            // 
            this.x265VBVInitialBuffer.DecimalPlaces = 1;
            this.x265VBVInitialBuffer.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.x265VBVInitialBuffer.Location = new System.Drawing.Point(229, 68);
            this.x265VBVInitialBuffer.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.x265VBVInitialBuffer.Name = "x265VBVInitialBuffer";
            this.x265VBVInitialBuffer.Size = new System.Drawing.Size(48, 20);
            this.x265VBVInitialBuffer.TabIndex = 5;
            this.x265VBVInitialBuffer.Value = new decimal(new int[] {
            9,
            0,
            0,
            65536});
            this.x265VBVInitialBuffer.ValueChanged += new System.EventHandler(this.updateEvent);
            // 
            // x265VBVInitialBufferLabel
            // 
            this.x265VBVInitialBufferLabel.AutoSize = true;
            this.x265VBVInitialBufferLabel.Location = new System.Drawing.Point(12, 70);
            this.x265VBVInitialBufferLabel.Name = "x265VBVInitialBufferLabel";
            this.x265VBVInitialBufferLabel.Size = new System.Drawing.Size(86, 13);
            this.x265VBVInitialBufferLabel.TabIndex = 4;
            this.x265VBVInitialBufferLabel.Text = "VBV Initial Buffer";
            this.x265VBVInitialBufferLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // x265VBVMaxRate
            // 
            this.x265VBVMaxRate.Enabled = false;
            this.x265VBVMaxRate.Location = new System.Drawing.Point(222, 43);
            this.x265VBVMaxRate.Maximum = new decimal(new int[] {
            99999999,
            0,
            0,
            0});
            this.x265VBVMaxRate.Name = "x265VBVMaxRate";
            this.x265VBVMaxRate.Size = new System.Drawing.Size(55, 20);
            this.x265VBVMaxRate.TabIndex = 3;
            this.x265VBVMaxRate.ValueChanged += new System.EventHandler(this.x265VBVMaxRate_ValueChanged);
            // 
            // x265TempQuantBlur
            // 
            this.x265TempQuantBlur.DecimalPlaces = 1;
            this.x265TempQuantBlur.Increment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.x265TempQuantBlur.Location = new System.Drawing.Point(229, 168);
            this.x265TempQuantBlur.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.x265TempQuantBlur.Name = "x265TempQuantBlur";
            this.x265TempQuantBlur.Size = new System.Drawing.Size(48, 20);
            this.x265TempQuantBlur.TabIndex = 13;
            this.x265TempQuantBlur.Value = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.x265TempQuantBlur.ValueChanged += new System.EventHandler(this.updateEvent);
            // 
            // x265TempFrameComplexityBlur
            // 
            this.x265TempFrameComplexityBlur.Location = new System.Drawing.Point(229, 143);
            this.x265TempFrameComplexityBlur.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.x265TempFrameComplexityBlur.Name = "x265TempFrameComplexityBlur";
            this.x265TempFrameComplexityBlur.Size = new System.Drawing.Size(48, 20);
            this.x265TempFrameComplexityBlur.TabIndex = 11;
            this.x265TempFrameComplexityBlur.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.x265TempFrameComplexityBlur.ValueChanged += new System.EventHandler(this.updateEvent);
            // 
            // x265QuantizerCompression
            // 
            this.x265QuantizerCompression.DecimalPlaces = 1;
            this.x265QuantizerCompression.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.x265QuantizerCompression.Location = new System.Drawing.Point(229, 118);
            this.x265QuantizerCompression.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.x265QuantizerCompression.Name = "x265QuantizerCompression";
            this.x265QuantizerCompression.Size = new System.Drawing.Size(48, 20);
            this.x265QuantizerCompression.TabIndex = 9;
            this.x265QuantizerCompression.Value = new decimal(new int[] {
            6,
            0,
            0,
            65536});
            this.x265QuantizerCompression.ValueChanged += new System.EventHandler(this.updateEvent);
            // 
            // x265VBVBufferSize
            // 
            this.x265VBVBufferSize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.x265VBVBufferSize.Location = new System.Drawing.Point(222, 18);
            this.x265VBVBufferSize.Maximum = new decimal(new int[] {
            99999999,
            0,
            0,
            0});
            this.x265VBVBufferSize.Name = "x265VBVBufferSize";
            this.x265VBVBufferSize.Size = new System.Drawing.Size(55, 20);
            this.x265VBVBufferSize.TabIndex = 1;
            this.x265VBVBufferSize.ValueChanged += new System.EventHandler(this.x265VBVBufferSize_ValueChanged);
            // 
            // x265TempQuantBlurLabel
            // 
            this.x265TempQuantBlurLabel.AutoSize = true;
            this.x265TempQuantBlurLabel.Location = new System.Drawing.Point(12, 170);
            this.x265TempQuantBlurLabel.Name = "x265TempQuantBlurLabel";
            this.x265TempQuantBlurLabel.Size = new System.Drawing.Size(143, 13);
            this.x265TempQuantBlurLabel.TabIndex = 12;
            this.x265TempQuantBlurLabel.Text = "Temp. Blur of Quant after CC";
            this.x265TempQuantBlurLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // x265TempFrameComplexityBlurLabel
            // 
            this.x265TempFrameComplexityBlurLabel.AutoSize = true;
            this.x265TempFrameComplexityBlurLabel.Location = new System.Drawing.Point(12, 145);
            this.x265TempFrameComplexityBlurLabel.Name = "x265TempFrameComplexityBlurLabel";
            this.x265TempFrameComplexityBlurLabel.Size = new System.Drawing.Size(174, 13);
            this.x265TempFrameComplexityBlurLabel.TabIndex = 10;
            this.x265TempFrameComplexityBlurLabel.Text = "Temp. Blur of est. Frame complexity";
            this.x265TempFrameComplexityBlurLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // x265QuantizerCompressionLabel
            // 
            this.x265QuantizerCompressionLabel.AutoSize = true;
            this.x265QuantizerCompressionLabel.Location = new System.Drawing.Point(12, 120);
            this.x265QuantizerCompressionLabel.Name = "x265QuantizerCompressionLabel";
            this.x265QuantizerCompressionLabel.Size = new System.Drawing.Size(115, 13);
            this.x265QuantizerCompressionLabel.TabIndex = 8;
            this.x265QuantizerCompressionLabel.Text = "Quantizer Compression";
            this.x265QuantizerCompressionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // x265VBVMaxRateLabel
            // 
            this.x265VBVMaxRateLabel.AutoSize = true;
            this.x265VBVMaxRateLabel.Enabled = false;
            this.x265VBVMaxRateLabel.Location = new System.Drawing.Point(12, 45);
            this.x265VBVMaxRateLabel.Name = "x265VBVMaxRateLabel";
            this.x265VBVMaxRateLabel.Size = new System.Drawing.Size(108, 13);
            this.x265VBVMaxRateLabel.TabIndex = 2;
            this.x265VBVMaxRateLabel.Text = "VBV Maximum Bitrate";
            this.x265VBVMaxRateLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // x265VBVBufferSizeLabel
            // 
            this.x265VBVBufferSizeLabel.AutoSize = true;
            this.x265VBVBufferSizeLabel.Location = new System.Drawing.Point(12, 21);
            this.x265VBVBufferSizeLabel.Name = "x265VBVBufferSizeLabel";
            this.x265VBVBufferSizeLabel.Size = new System.Drawing.Size(82, 13);
            this.x265VBVBufferSizeLabel.TabIndex = 0;
            this.x265VBVBufferSizeLabel.Text = "VBV Buffer Size";
            this.x265VBVBufferSizeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // x265RateTol
            // 
            this.x265RateTol.DecimalPlaces = 1;
            this.x265RateTol.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.x265RateTol.Location = new System.Drawing.Point(229, 93);
            this.x265RateTol.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.x265RateTol.Name = "x265RateTol";
            this.x265RateTol.Size = new System.Drawing.Size(48, 20);
            this.x265RateTol.TabIndex = 7;
            this.x265RateTol.Value = new decimal(new int[] {
            10,
            0,
            0,
            65536});
            this.x265RateTol.ValueChanged += new System.EventHandler(this.updateEvent);
            // 
            // gbAQ
            // 
            this.gbAQ.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.gbAQ.Controls.Add(this.numAQStrength);
            this.gbAQ.Controls.Add(this.lbAQStrength);
            this.gbAQ.Controls.Add(this.cbAQMode);
            this.gbAQ.Controls.Add(this.lbAQMode);
            this.gbAQ.Location = new System.Drawing.Point(298, 3);
            this.gbAQ.Name = "gbAQ";
            this.gbAQ.Size = new System.Drawing.Size(201, 78);
            this.gbAQ.TabIndex = 7;
            this.gbAQ.TabStop = false;
            this.gbAQ.Text = "Adaptive Quantizers";
            // 
            // numAQStrength
            // 
            this.numAQStrength.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.numAQStrength.DecimalPlaces = 1;
            this.numAQStrength.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numAQStrength.Location = new System.Drawing.Point(109, 46);
            this.numAQStrength.Maximum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.numAQStrength.Name = "numAQStrength";
            this.numAQStrength.Size = new System.Drawing.Size(78, 20);
            this.numAQStrength.TabIndex = 3;
            this.numAQStrength.Value = new decimal(new int[] {
            10,
            0,
            0,
            65536});
            this.numAQStrength.ValueChanged += new System.EventHandler(this.updateEvent);
            // 
            // lbAQStrength
            // 
            this.lbAQStrength.AutoSize = true;
            this.lbAQStrength.Location = new System.Drawing.Point(12, 48);
            this.lbAQStrength.Name = "lbAQStrength";
            this.lbAQStrength.Size = new System.Drawing.Size(50, 13);
            this.lbAQStrength.TabIndex = 2;
            this.lbAQStrength.Text = "Strength ";
            this.lbAQStrength.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cbAQMode
            // 
            this.cbAQMode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbAQMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbAQMode.FormattingEnabled = true;
            this.cbAQMode.Items.AddRange(new object[] {
            "Disabled",
            "Variance AQ (complexity mask)",
            "Auto-variance AQ (experimental)"});
            this.cbAQMode.Location = new System.Drawing.Point(52, 19);
            this.cbAQMode.Name = "cbAQMode";
            this.cbAQMode.Size = new System.Drawing.Size(135, 21);
            this.cbAQMode.TabIndex = 1;
            this.cbAQMode.SelectedIndexChanged += new System.EventHandler(this.cbAQMode_SelectedIndexChanged);
            // 
            // lbAQMode
            // 
            this.lbAQMode.AutoSize = true;
            this.lbAQMode.Location = new System.Drawing.Point(12, 22);
            this.lbAQMode.Name = "lbAQMode";
            this.lbAQMode.Size = new System.Drawing.Size(34, 13);
            this.lbAQMode.TabIndex = 0;
            this.lbAQMode.Text = "Mode";
            this.lbAQMode.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // quantizerMatrixGroupbox
            // 
            this.quantizerMatrixGroupbox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.quantizerMatrixGroupbox.Controls.Add(this.cqmComboBox1);
            this.quantizerMatrixGroupbox.Location = new System.Drawing.Point(298, 87);
            this.quantizerMatrixGroupbox.Name = "quantizerMatrixGroupbox";
            this.quantizerMatrixGroupbox.Size = new System.Drawing.Size(201, 59);
            this.quantizerMatrixGroupbox.TabIndex = 2;
            this.quantizerMatrixGroupbox.TabStop = false;
            this.quantizerMatrixGroupbox.Text = "Quantizer Matrices";
            // 
            // cqmComboBox1
            // 
            this.cqmComboBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cqmComboBox1.Filter = "Quantizer matrix files (*.cfg)|*.cfg|All Files (*.*)|*.*";
            this.cqmComboBox1.Location = new System.Drawing.Point(12, 19);
            this.cqmComboBox1.MaximumSize = new System.Drawing.Size(1000, 29);
            this.cqmComboBox1.MinimumSize = new System.Drawing.Size(64, 29);
            this.cqmComboBox1.Name = "cqmComboBox1";
            this.cqmComboBox1.SelectedIndex = 0;
            this.cqmComboBox1.Size = new System.Drawing.Size(175, 29);
            this.cqmComboBox1.TabIndex = 5;
            this.cqmComboBox1.Type = MeGUI.core.gui.FileSCBox.FileSCBoxType.Default;
            this.cqmComboBox1.SelectionChanged += new MeGUI.StringChanged(this.cqmComboBox1_SelectionChanged);
            // 
            // x265QuantizerGroupBox
            // 
            this.x265QuantizerGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.x265QuantizerGroupBox.Controls.Add(this.deadzoneIntra);
            this.x265QuantizerGroupBox.Controls.Add(this.deadzoneInter);
            this.x265QuantizerGroupBox.Controls.Add(this.lbx265DeadZones);
            this.x265QuantizerGroupBox.Controls.Add(this.x265PBFrameFactor);
            this.x265QuantizerGroupBox.Controls.Add(this.x265IPFrameFactor);
            this.x265QuantizerGroupBox.Controls.Add(this.lbQuantizersRatio);
            this.x265QuantizerGroupBox.Controls.Add(this.x265CreditsQuantizer);
            this.x265QuantizerGroupBox.Controls.Add(this.x265CreditsQuantizerLabel);
            this.x265QuantizerGroupBox.Controls.Add(this.x265ChromaQPOffset);
            this.x265QuantizerGroupBox.Controls.Add(this.x265ChromaQPOffsetLabel);
            this.x265QuantizerGroupBox.Controls.Add(this.x265MaxQuantDelta);
            this.x265QuantizerGroupBox.Controls.Add(this.x265MaximumQuantizer);
            this.x265QuantizerGroupBox.Controls.Add(this.x265MinimimQuantizer);
            this.x265QuantizerGroupBox.Controls.Add(this.x265MinimimQuantizerLabel);
            this.x265QuantizerGroupBox.Location = new System.Drawing.Point(3, 3);
            this.x265QuantizerGroupBox.Name = "x265QuantizerGroupBox";
            this.x265QuantizerGroupBox.Size = new System.Drawing.Size(291, 183);
            this.x265QuantizerGroupBox.TabIndex = 0;
            this.x265QuantizerGroupBox.TabStop = false;
            this.x265QuantizerGroupBox.Text = "Quantizers";
            // 
            // deadzoneIntra
            // 
            this.deadzoneIntra.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.deadzoneIntra.Location = new System.Drawing.Point(229, 84);
            this.deadzoneIntra.Name = "deadzoneIntra";
            this.deadzoneIntra.Size = new System.Drawing.Size(48, 20);
            this.deadzoneIntra.TabIndex = 17;
            this.deadzoneIntra.Value = new decimal(new int[] {
            11,
            0,
            0,
            0});
            this.deadzoneIntra.ValueChanged += new System.EventHandler(this.updateEvent);
            // 
            // deadzoneInter
            // 
            this.deadzoneInter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.deadzoneInter.Location = new System.Drawing.Point(175, 84);
            this.deadzoneInter.Name = "deadzoneInter";
            this.deadzoneInter.Size = new System.Drawing.Size(48, 20);
            this.deadzoneInter.TabIndex = 15;
            this.deadzoneInter.Value = new decimal(new int[] {
            21,
            0,
            0,
            0});
            this.deadzoneInter.ValueChanged += new System.EventHandler(this.updateEvent);
            // 
            // lbx265DeadZones
            // 
            this.lbx265DeadZones.AutoSize = true;
            this.lbx265DeadZones.Location = new System.Drawing.Point(12, 86);
            this.lbx265DeadZones.Name = "lbx265DeadZones";
            this.lbx265DeadZones.Size = new System.Drawing.Size(123, 13);
            this.lbx265DeadZones.TabIndex = 21;
            this.lbx265DeadZones.Text = "Deadzones (Inter / Intra)";
            this.lbx265DeadZones.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // x265PBFrameFactor
            // 
            this.x265PBFrameFactor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.x265PBFrameFactor.DecimalPlaces = 1;
            this.x265PBFrameFactor.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.x265PBFrameFactor.Location = new System.Drawing.Point(229, 49);
            this.x265PBFrameFactor.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.x265PBFrameFactor.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.x265PBFrameFactor.Name = "x265PBFrameFactor";
            this.x265PBFrameFactor.Size = new System.Drawing.Size(48, 20);
            this.x265PBFrameFactor.TabIndex = 20;
            this.x265PBFrameFactor.Value = new decimal(new int[] {
            13,
            0,
            0,
            65536});
            this.x265PBFrameFactor.ValueChanged += new System.EventHandler(this.updateEvent);
            // 
            // x265IPFrameFactor
            // 
            this.x265IPFrameFactor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.x265IPFrameFactor.DecimalPlaces = 1;
            this.x265IPFrameFactor.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.x265IPFrameFactor.Location = new System.Drawing.Point(175, 49);
            this.x265IPFrameFactor.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.x265IPFrameFactor.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.x265IPFrameFactor.Name = "x265IPFrameFactor";
            this.x265IPFrameFactor.Size = new System.Drawing.Size(48, 20);
            this.x265IPFrameFactor.TabIndex = 19;
            this.x265IPFrameFactor.Value = new decimal(new int[] {
            14,
            0,
            0,
            65536});
            this.x265IPFrameFactor.ValueChanged += new System.EventHandler(this.updateEvent);
            // 
            // lbQuantizersRatio
            // 
            this.lbQuantizersRatio.AutoSize = true;
            this.lbQuantizersRatio.Location = new System.Drawing.Point(12, 51);
            this.lbQuantizersRatio.Name = "lbQuantizersRatio";
            this.lbQuantizersRatio.Size = new System.Drawing.Size(135, 13);
            this.lbQuantizersRatio.TabIndex = 18;
            this.lbQuantizersRatio.Text = "Quantizers Ratio (I:P / P:B)";
            this.lbQuantizersRatio.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // x265CreditsQuantizer
            // 
            this.x265CreditsQuantizer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.x265CreditsQuantizer.Location = new System.Drawing.Point(229, 149);
            this.x265CreditsQuantizer.Maximum = new decimal(new int[] {
            51,
            0,
            0,
            0});
            this.x265CreditsQuantizer.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.x265CreditsQuantizer.Name = "x265CreditsQuantizer";
            this.x265CreditsQuantizer.Size = new System.Drawing.Size(48, 20);
            this.x265CreditsQuantizer.TabIndex = 7;
            this.x265CreditsQuantizer.Value = new decimal(new int[] {
            40,
            0,
            0,
            0});
            this.x265CreditsQuantizer.ValueChanged += new System.EventHandler(this.updateEvent);
            // 
            // x265CreditsQuantizerLabel
            // 
            this.x265CreditsQuantizerLabel.Location = new System.Drawing.Point(9, 149);
            this.x265CreditsQuantizerLabel.Name = "x265CreditsQuantizerLabel";
            this.x265CreditsQuantizerLabel.Padding = new System.Windows.Forms.Padding(3);
            this.x265CreditsQuantizerLabel.Size = new System.Drawing.Size(122, 17);
            this.x265CreditsQuantizerLabel.TabIndex = 6;
            this.x265CreditsQuantizerLabel.Text = "Credits Quantizer";
            this.x265CreditsQuantizerLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // x265ChromaQPOffset
            // 
            this.x265ChromaQPOffset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.x265ChromaQPOffset.Location = new System.Drawing.Point(229, 119);
            this.x265ChromaQPOffset.Maximum = new decimal(new int[] {
            12,
            0,
            0,
            0});
            this.x265ChromaQPOffset.Minimum = new decimal(new int[] {
            12,
            0,
            0,
            -2147483648});
            this.x265ChromaQPOffset.Name = "x265ChromaQPOffset";
            this.x265ChromaQPOffset.Size = new System.Drawing.Size(48, 20);
            this.x265ChromaQPOffset.TabIndex = 13;
            this.x265ChromaQPOffset.ValueChanged += new System.EventHandler(this.updateEvent);
            // 
            // x265ChromaQPOffsetLabel
            // 
            this.x265ChromaQPOffsetLabel.Location = new System.Drawing.Point(9, 119);
            this.x265ChromaQPOffsetLabel.Name = "x265ChromaQPOffsetLabel";
            this.x265ChromaQPOffsetLabel.Padding = new System.Windows.Forms.Padding(3);
            this.x265ChromaQPOffsetLabel.Size = new System.Drawing.Size(122, 17);
            this.x265ChromaQPOffsetLabel.TabIndex = 12;
            this.x265ChromaQPOffsetLabel.Text = "Chroma QP Offset";
            this.x265ChromaQPOffsetLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // x265MaxQuantDelta
            // 
            this.x265MaxQuantDelta.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.x265MaxQuantDelta.Location = new System.Drawing.Point(229, 17);
            this.x265MaxQuantDelta.Maximum = new decimal(new int[] {
            51,
            0,
            0,
            0});
            this.x265MaxQuantDelta.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.x265MaxQuantDelta.Name = "x265MaxQuantDelta";
            this.x265MaxQuantDelta.Size = new System.Drawing.Size(48, 20);
            this.x265MaxQuantDelta.TabIndex = 5;
            this.x265MaxQuantDelta.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.x265MaxQuantDelta.ValueChanged += new System.EventHandler(this.updateEvent);
            // 
            // x265MaximumQuantizer
            // 
            this.x265MaximumQuantizer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.x265MaximumQuantizer.Location = new System.Drawing.Point(175, 17);
            this.x265MaximumQuantizer.Maximum = new decimal(new int[] {
            69,
            0,
            0,
            0});
            this.x265MaximumQuantizer.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.x265MaximumQuantizer.Name = "x265MaximumQuantizer";
            this.x265MaximumQuantizer.Size = new System.Drawing.Size(48, 20);
            this.x265MaximumQuantizer.TabIndex = 3;
            this.x265MaximumQuantizer.Value = new decimal(new int[] {
            69,
            0,
            0,
            0});
            this.x265MaximumQuantizer.ValueChanged += new System.EventHandler(this.updateEvent);
            // 
            // x265MinimimQuantizer
            // 
            this.x265MinimimQuantizer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.x265MinimimQuantizer.Location = new System.Drawing.Point(121, 17);
            this.x265MinimimQuantizer.Maximum = new decimal(new int[] {
            51,
            0,
            0,
            0});
            this.x265MinimimQuantizer.Name = "x265MinimimQuantizer";
            this.x265MinimimQuantizer.Size = new System.Drawing.Size(48, 20);
            this.x265MinimimQuantizer.TabIndex = 1;
            this.x265MinimimQuantizer.ValueChanged += new System.EventHandler(this.updateEvent);
            // 
            // x265MinimimQuantizerLabel
            // 
            this.x265MinimimQuantizerLabel.Location = new System.Drawing.Point(9, 16);
            this.x265MinimimQuantizerLabel.Name = "x265MinimimQuantizerLabel";
            this.x265MinimimQuantizerLabel.Padding = new System.Windows.Forms.Padding(3);
            this.x265MinimimQuantizerLabel.Size = new System.Drawing.Size(93, 18);
            this.x265MinimimQuantizerLabel.TabIndex = 0;
            this.x265MinimimQuantizerLabel.Text = "Min/Max/Delta";
            this.x265MinimimQuantizerLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(12, 227);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(207, 17);
            this.label3.TabIndex = 16;
            this.label3.Text = "Intra luma quantization deadzone";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(12, 201);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(207, 17);
            this.label2.TabIndex = 14;
            this.label2.Text = "Inter luma quantization deadzone";
            // 
            // customCommandlineOptionsLabel
            // 
            this.customCommandlineOptionsLabel.Location = new System.Drawing.Point(6, 301);
            this.customCommandlineOptionsLabel.Name = "customCommandlineOptionsLabel";
            this.customCommandlineOptionsLabel.Size = new System.Drawing.Size(167, 13);
            this.customCommandlineOptionsLabel.TabIndex = 1;
            this.customCommandlineOptionsLabel.Text = "Custom Commandline Options";
            this.customCommandlineOptionsLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "openFileDialog1";
            // 
            // helpButton1
            // 
            this.helpButton1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.helpButton1.ArticleName = "Configuration/Video_Encoder_Configuration/x265";
            this.helpButton1.AutoSize = true;
            this.helpButton1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.helpButton1.Location = new System.Drawing.Point(455, 199);
            this.helpButton1.Name = "helpButton1";
            this.helpButton1.Size = new System.Drawing.Size(39, 23);
            this.helpButton1.TabIndex = 10;
            // 
            // gbPresets
            // 
            this.gbPresets.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbPresets.BackColor = System.Drawing.Color.Transparent;
            this.gbPresets.Controls.Add(this.lbPreset);
            this.gbPresets.Controls.Add(this.tbx265Presets);
            this.gbPresets.Location = new System.Drawing.Point(6, 71);
            this.gbPresets.Name = "gbPresets";
            this.gbPresets.Size = new System.Drawing.Size(488, 102);
            this.gbPresets.TabIndex = 13;
            this.gbPresets.TabStop = false;
            this.gbPresets.Text = " Preset ";
            // 
            // lbPreset
            // 
            this.lbPreset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lbPreset.AutoSize = true;
            this.lbPreset.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbPreset.Location = new System.Drawing.Point(223, 28);
            this.lbPreset.Name = "lbPreset";
            this.lbPreset.Size = new System.Drawing.Size(44, 13);
            this.lbPreset.TabIndex = 1;
            this.lbPreset.Text = "Medium";
            // 
            // tbx265Presets
            // 
            this.tbx265Presets.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbx265Presets.AutoSize = false;
            this.tbx265Presets.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.tbx265Presets.Location = new System.Drawing.Point(12, 50);
            this.tbx265Presets.Maximum = 9;
            this.tbx265Presets.Name = "tbx265Presets";
            this.tbx265Presets.Size = new System.Drawing.Size(464, 30);
            this.tbx265Presets.TabIndex = 0;
            this.tbx265Presets.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.tbx265Presets.Value = 5;
            this.tbx265Presets.Scroll += new System.EventHandler(this.tbx265Presets_Scroll);
            this.tbx265Presets.ValueChanged += new System.EventHandler(this.tbx265Presets_Scroll);
            // 
            // gbTunes
            // 
            this.gbTunes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.gbTunes.Controls.Add(this.x265Tunes);
            this.gbTunes.Location = new System.Drawing.Point(322, 17);
            this.gbTunes.Name = "gbTunes";
            this.gbTunes.Size = new System.Drawing.Size(172, 48);
            this.gbTunes.TabIndex = 14;
            this.gbTunes.TabStop = false;
            this.gbTunes.Text = " Tuning ";
            // 
            // x265Tunes
            // 
            this.x265Tunes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.x265Tunes.FormattingEnabled = true;
            this.x265Tunes.Location = new System.Drawing.Point(10, 16);
            this.x265Tunes.Name = "x265Tunes";
            this.x265Tunes.Size = new System.Drawing.Size(157, 21);
            this.x265Tunes.TabIndex = 0;
            this.x265Tunes.SelectedIndexChanged += new System.EventHandler(this.x265Tunes_SelectedIndexChanged);
            // 
            // AnalysisTabPage
            // 
            this.AnalysisTabPage.Controls.Add(this.x265Bluray);
            this.AnalysisTabPage.Controls.Add(this.x265QuantOptionsGroupbox);
            this.AnalysisTabPage.Controls.Add(this.x265MBGroupbox);
            this.AnalysisTabPage.Controls.Add(this.x265OtherOptionsGroupbox);
            this.AnalysisTabPage.Location = new System.Drawing.Point(4, 22);
            this.AnalysisTabPage.Name = "AnalysisTabPage";
            this.AnalysisTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.AnalysisTabPage.Size = new System.Drawing.Size(502, 242);
            this.AnalysisTabPage.TabIndex = 5;
            this.AnalysisTabPage.Text = "Analysis";
            this.AnalysisTabPage.UseVisualStyleBackColor = true;
            // 
            // x265Bluray
            // 
            this.x265Bluray.Controls.Add(this.chkBlurayCompat);
            this.x265Bluray.Controls.Add(this.fakeInterlaced);
            this.x265Bluray.Controls.Add(this.x265hrdLabel);
            this.x265Bluray.Controls.Add(this.x265hrd);
            this.x265Bluray.Controls.Add(this.x265aud);
            this.x265Bluray.Location = new System.Drawing.Point(296, 135);
            this.x265Bluray.Name = "x265Bluray";
            this.x265Bluray.Size = new System.Drawing.Size(200, 134);
            this.x265Bluray.TabIndex = 29;
            this.x265Bluray.TabStop = false;
            this.x265Bluray.Text = " Blu-Ray ";
            // 
            // chkBlurayCompat
            // 
            this.chkBlurayCompat.AutoSize = true;
            this.chkBlurayCompat.Location = new System.Drawing.Point(9, 98);
            this.chkBlurayCompat.Name = "chkBlurayCompat";
            this.chkBlurayCompat.Size = new System.Drawing.Size(154, 17);
            this.chkBlurayCompat.TabIndex = 4;
            this.chkBlurayCompat.Text = "Enable Blu-ray compatibility";
            this.chkBlurayCompat.UseVisualStyleBackColor = true;
            this.chkBlurayCompat.CheckedChanged += new System.EventHandler(this.updateEvent);
            // 
            // fakeInterlaced
            // 
            this.fakeInterlaced.AutoSize = true;
            this.fakeInterlaced.Location = new System.Drawing.Point(9, 75);
            this.fakeInterlaced.Name = "fakeInterlaced";
            this.fakeInterlaced.Size = new System.Drawing.Size(100, 17);
            this.fakeInterlaced.TabIndex = 3;
            this.fakeInterlaced.Text = "Fake Interlaced";
            this.fakeInterlaced.UseVisualStyleBackColor = true;
            this.fakeInterlaced.CheckedChanged += new System.EventHandler(this.updateEvent);
            // 
            // x265hrdLabel
            // 
            this.x265hrdLabel.AutoSize = true;
            this.x265hrdLabel.Location = new System.Drawing.Point(6, 29);
            this.x265hrdLabel.Name = "x265hrdLabel";
            this.x265hrdLabel.Size = new System.Drawing.Size(52, 13);
            this.x265hrdLabel.TabIndex = 2;
            this.x265hrdLabel.Text = "HRD Info";
            this.x265hrdLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // x265hrd
            // 
            this.x265hrd.AutoCompleteCustomSource.AddRange(new string[] {
            "None",
            "VBR",
            "CBR"});
            this.x265hrd.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.x265hrd.FormattingEnabled = true;
            this.x265hrd.Items.AddRange(new object[] {
            "None",
            "VBR",
            "CBR"});
            this.x265hrd.Location = new System.Drawing.Point(66, 26);
            this.x265hrd.Name = "x265hrd";
            this.x265hrd.Size = new System.Drawing.Size(111, 21);
            this.x265hrd.TabIndex = 1;
            this.x265hrd.SelectedIndexChanged += new System.EventHandler(this.updateEvent);
            // 
            // x265aud
            // 
            this.x265aud.AutoSize = true;
            this.x265aud.Location = new System.Drawing.Point(9, 52);
            this.x265aud.Name = "x265aud";
            this.x265aud.Size = new System.Drawing.Size(153, 17);
            this.x265aud.TabIndex = 0;
            this.x265aud.Text = "Use Access Unit Delimiters";
            this.x265aud.UseVisualStyleBackColor = true;
            this.x265aud.CheckedChanged += new System.EventHandler(this.updateEvent);
            // 
            // x265QuantOptionsGroupbox
            // 
            this.x265QuantOptionsGroupbox.Controls.Add(this.label16);
            this.x265QuantOptionsGroupbox.Controls.Add(this.label15);
            this.x265QuantOptionsGroupbox.Controls.Add(this.label14);
            this.x265QuantOptionsGroupbox.Controls.Add(this.label13);
            this.x265QuantOptionsGroupbox.Controls.Add(this.NoiseReduction);
            this.x265QuantOptionsGroupbox.Controls.Add(this.NoiseReductionLabel);
            this.x265QuantOptionsGroupbox.Controls.Add(this.nopsy);
            this.x265QuantOptionsGroupbox.Controls.Add(this.x265MixedReferences);
            this.x265QuantOptionsGroupbox.Controls.Add(this.x265BframePredictionMode);
            this.x265QuantOptionsGroupbox.Controls.Add(this.x265BframePredictionModeLabel);
            this.x265QuantOptionsGroupbox.Controls.Add(this.label4);
            this.x265QuantOptionsGroupbox.Controls.Add(this.PsyTrellis);
            this.x265QuantOptionsGroupbox.Controls.Add(this.label5);
            this.x265QuantOptionsGroupbox.Controls.Add(this.PsyRD);
            this.x265QuantOptionsGroupbox.Controls.Add(this.noDCTDecimateOption);
            this.x265QuantOptionsGroupbox.Controls.Add(this.noFastPSkip);
            this.x265QuantOptionsGroupbox.Controls.Add(this.trellis);
            this.x265QuantOptionsGroupbox.Controls.Add(this.label7);
            this.x265QuantOptionsGroupbox.Location = new System.Drawing.Point(6, 135);
            this.x265QuantOptionsGroupbox.Name = "x265QuantOptionsGroupbox";
            this.x265QuantOptionsGroupbox.Size = new System.Drawing.Size(284, 272);
            this.x265QuantOptionsGroupbox.TabIndex = 28;
            this.x265QuantOptionsGroupbox.TabStop = false;
            this.x265QuantOptionsGroupbox.Text = "Extra";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(16, 214);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(160, 13);
            this.label16.TabIndex = 21;
            this.label16.Text = "No Psychovisual Enhancements";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(16, 190);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(78, 13);
            this.label15.TabIndex = 20;
            this.label15.Text = "No Fast P-Skip";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(16, 167);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(97, 13);
            this.label14.TabIndex = 19;
            this.label14.Text = "No Dct Decimation";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(16, 145);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(139, 13);
            this.label13.TabIndex = 18;
            this.label13.Text = "No Mixed Reference frames";
            // 
            // NoiseReduction
            // 
            this.NoiseReduction.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.NoiseReduction.Location = new System.Drawing.Point(224, 239);
            this.NoiseReduction.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.NoiseReduction.Name = "NoiseReduction";
            this.NoiseReduction.Size = new System.Drawing.Size(44, 20);
            this.NoiseReduction.TabIndex = 17;
            this.NoiseReduction.ValueChanged += new System.EventHandler(this.updateEvent);
            // 
            // NoiseReductionLabel
            // 
            this.NoiseReductionLabel.Location = new System.Drawing.Point(16, 241);
            this.NoiseReductionLabel.Name = "NoiseReductionLabel";
            this.NoiseReductionLabel.Size = new System.Drawing.Size(86, 13);
            this.NoiseReductionLabel.TabIndex = 16;
            this.NoiseReductionLabel.Text = "Noise Reduction";
            this.NoiseReductionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // nopsy
            // 
            this.nopsy.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.nopsy.Location = new System.Drawing.Point(224, 209);
            this.nopsy.Name = "nopsy";
            this.nopsy.Size = new System.Drawing.Size(45, 24);
            this.nopsy.TabIndex = 15;
            this.nopsy.UseVisualStyleBackColor = true;
            this.nopsy.CheckedChanged += new System.EventHandler(this.updateEvent);
            // 
            // x265MixedReferences
            // 
            this.x265MixedReferences.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.x265MixedReferences.Location = new System.Drawing.Point(224, 140);
            this.x265MixedReferences.Name = "x265MixedReferences";
            this.x265MixedReferences.Size = new System.Drawing.Size(45, 24);
            this.x265MixedReferences.TabIndex = 14;
            this.x265MixedReferences.CheckedChanged += new System.EventHandler(this.updateEvent);
            // 
            // x265BframePredictionMode
            // 
            this.x265BframePredictionMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.x265BframePredictionMode.Items.AddRange(new object[] {
            "None",
            "Spatial",
            "Temporal",
            "Auto"});
            this.x265BframePredictionMode.Location = new System.Drawing.Point(154, 25);
            this.x265BframePredictionMode.Name = "x265BframePredictionMode";
            this.x265BframePredictionMode.Size = new System.Drawing.Size(115, 21);
            this.x265BframePredictionMode.TabIndex = 13;
            this.x265BframePredictionMode.SelectedIndexChanged += new System.EventHandler(this.updateEvent);
            // 
            // x265BframePredictionModeLabel
            // 
            this.x265BframePredictionModeLabel.Location = new System.Drawing.Point(16, 29);
            this.x265BframePredictionModeLabel.Name = "x265BframePredictionModeLabel";
            this.x265BframePredictionModeLabel.Size = new System.Drawing.Size(102, 13);
            this.x265BframePredictionModeLabel.TabIndex = 12;
            this.x265BframePredictionModeLabel.Text = "MV Prediction mode";
            this.x265BframePredictionModeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(16, 116);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(97, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "Psy-Trellis Strength";
            // 
            // PsyTrellis
            // 
            this.PsyTrellis.DecimalPlaces = 2;
            this.PsyTrellis.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.PsyTrellis.Location = new System.Drawing.Point(224, 114);
            this.PsyTrellis.Name = "PsyTrellis";
            this.PsyTrellis.Size = new System.Drawing.Size(45, 20);
            this.PsyTrellis.TabIndex = 10;
            this.PsyTrellis.ValueChanged += new System.EventHandler(this.updateEvent);
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(16, 90);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(86, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Psy-RD Strength";
            // 
            // PsyRD
            // 
            this.PsyRD.DecimalPlaces = 2;
            this.PsyRD.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.PsyRD.Location = new System.Drawing.Point(224, 88);
            this.PsyRD.Maximum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.PsyRD.Name = "PsyRD";
            this.PsyRD.Size = new System.Drawing.Size(45, 20);
            this.PsyRD.TabIndex = 8;
            this.PsyRD.Value = new decimal(new int[] {
            10,
            0,
            0,
            65536});
            this.PsyRD.ValueChanged += new System.EventHandler(this.updateEvent);
            // 
            // noDCTDecimateOption
            // 
            this.noDCTDecimateOption.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.noDCTDecimateOption.Location = new System.Drawing.Point(224, 163);
            this.noDCTDecimateOption.Name = "noDCTDecimateOption";
            this.noDCTDecimateOption.Size = new System.Drawing.Size(45, 23);
            this.noDCTDecimateOption.TabIndex = 6;
            this.noDCTDecimateOption.CheckedChanged += new System.EventHandler(this.updateEvent);
            // 
            // noFastPSkip
            // 
            this.noFastPSkip.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.noFastPSkip.Location = new System.Drawing.Point(224, 186);
            this.noFastPSkip.Name = "noFastPSkip";
            this.noFastPSkip.Size = new System.Drawing.Size(45, 23);
            this.noFastPSkip.TabIndex = 7;
            this.noFastPSkip.CheckedChanged += new System.EventHandler(this.updateEvent);
            // 
            // trellis
            // 
            this.trellis.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.trellis.Items.AddRange(new object[] {
            "0 - None",
            "1 - Final MB",
            "2 - Always"});
            this.trellis.Location = new System.Drawing.Point(154, 53);
            this.trellis.Name = "trellis";
            this.trellis.Size = new System.Drawing.Size(115, 21);
            this.trellis.TabIndex = 1;
            this.trellis.SelectedIndexChanged += new System.EventHandler(this.updateEvent);
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(16, 56);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(34, 13);
            this.label7.TabIndex = 0;
            this.label7.Text = "Trellis";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // x265MBGroupbox
            // 
            this.x265MBGroupbox.Controls.Add(this.label1);
            this.x265MBGroupbox.Controls.Add(this.macroblockOptions);
            this.x265MBGroupbox.Controls.Add(this.adaptiveDCT);
            this.x265MBGroupbox.Controls.Add(this.x265I4x4mv);
            this.x265MBGroupbox.Controls.Add(this.x265I8x8mv);
            this.x265MBGroupbox.Controls.Add(this.x265P4x4mv);
            this.x265MBGroupbox.Controls.Add(this.x265B8x8mv);
            this.x265MBGroupbox.Controls.Add(this.x265P8x8mv);
            this.x265MBGroupbox.Location = new System.Drawing.Point(296, 6);
            this.x265MBGroupbox.Name = "x265MBGroupbox";
            this.x265MBGroupbox.Size = new System.Drawing.Size(202, 123);
            this.x265MBGroupbox.TabIndex = 27;
            this.x265MBGroupbox.TabStop = false;
            this.x265MBGroupbox.Text = "Macroblocks";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Partitions";
            // 
            // macroblockOptions
            // 
            this.macroblockOptions.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.macroblockOptions.Items.AddRange(new object[] {
            "All",
            "None",
            "Custom",
            "Default"});
            this.macroblockOptions.Location = new System.Drawing.Point(66, 16);
            this.macroblockOptions.Name = "macroblockOptions";
            this.macroblockOptions.Size = new System.Drawing.Size(111, 21);
            this.macroblockOptions.TabIndex = 0;
            this.macroblockOptions.SelectedIndexChanged += new System.EventHandler(this.updateEvent);
            // 
            // adaptiveDCT
            // 
            this.adaptiveDCT.Checked = true;
            this.adaptiveDCT.CheckState = System.Windows.Forms.CheckState.Checked;
            this.adaptiveDCT.Location = new System.Drawing.Point(9, 40);
            this.adaptiveDCT.Name = "adaptiveDCT";
            this.adaptiveDCT.Size = new System.Drawing.Size(104, 24);
            this.adaptiveDCT.TabIndex = 1;
            this.adaptiveDCT.Text = "Adaptive DCT";
            this.adaptiveDCT.CheckedChanged += new System.EventHandler(this.updateEvent);
            // 
            // x265I4x4mv
            // 
            this.x265I4x4mv.Checked = true;
            this.x265I4x4mv.CheckState = System.Windows.Forms.CheckState.Checked;
            this.x265I4x4mv.Location = new System.Drawing.Point(9, 67);
            this.x265I4x4mv.Name = "x265I4x4mv";
            this.x265I4x4mv.Size = new System.Drawing.Size(56, 24);
            this.x265I4x4mv.TabIndex = 2;
            this.x265I4x4mv.Text = "I4x4";
            this.x265I4x4mv.CheckedChanged += new System.EventHandler(this.updateEvent);
            // 
            // x265I8x8mv
            // 
            this.x265I8x8mv.Checked = true;
            this.x265I8x8mv.CheckState = System.Windows.Forms.CheckState.Checked;
            this.x265I8x8mv.Location = new System.Drawing.Point(9, 93);
            this.x265I8x8mv.Name = "x265I8x8mv";
            this.x265I8x8mv.Size = new System.Drawing.Size(56, 24);
            this.x265I8x8mv.TabIndex = 4;
            this.x265I8x8mv.Text = "I8x8";
            this.x265I8x8mv.CheckedChanged += new System.EventHandler(this.updateEvent);
            // 
            // x265P4x4mv
            // 
            this.x265P4x4mv.Location = new System.Drawing.Point(66, 67);
            this.x265P4x4mv.Name = "x265P4x4mv";
            this.x265P4x4mv.Size = new System.Drawing.Size(64, 24);
            this.x265P4x4mv.TabIndex = 3;
            this.x265P4x4mv.Text = "P4x4";
            this.x265P4x4mv.CheckedChanged += new System.EventHandler(this.updateEvent);
            // 
            // x265B8x8mv
            // 
            this.x265B8x8mv.Checked = true;
            this.x265B8x8mv.CheckState = System.Windows.Forms.CheckState.Checked;
            this.x265B8x8mv.Location = new System.Drawing.Point(121, 93);
            this.x265B8x8mv.Name = "x265B8x8mv";
            this.x265B8x8mv.Size = new System.Drawing.Size(56, 24);
            this.x265B8x8mv.TabIndex = 6;
            this.x265B8x8mv.Text = "B8x8";
            this.x265B8x8mv.CheckedChanged += new System.EventHandler(this.updateEvent);
            // 
            // x265P8x8mv
            // 
            this.x265P8x8mv.Checked = true;
            this.x265P8x8mv.CheckState = System.Windows.Forms.CheckState.Checked;
            this.x265P8x8mv.Location = new System.Drawing.Point(66, 93);
            this.x265P8x8mv.Name = "x265P8x8mv";
            this.x265P8x8mv.Size = new System.Drawing.Size(64, 24);
            this.x265P8x8mv.TabIndex = 5;
            this.x265P8x8mv.Text = "P8x8";
            this.x265P8x8mv.CheckedChanged += new System.EventHandler(this.updateEvent);
            // 
            // x265OtherOptionsGroupbox
            // 
            this.x265OtherOptionsGroupbox.Controls.Add(this.label17);
            this.x265OtherOptionsGroupbox.Controls.Add(this.x265SubpelRefinement);
            this.x265OtherOptionsGroupbox.Controls.Add(this.x265SubpelRefinementLabel);
            this.x265OtherOptionsGroupbox.Controls.Add(this.x265ChromaMe);
            this.x265OtherOptionsGroupbox.Controls.Add(this.x265MERangeLabel);
            this.x265OtherOptionsGroupbox.Controls.Add(this.x265METypeLabel);
            this.x265OtherOptionsGroupbox.Controls.Add(this.x265METype);
            this.x265OtherOptionsGroupbox.Controls.Add(this.x265MERange);
            this.x265OtherOptionsGroupbox.Location = new System.Drawing.Point(6, 6);
            this.x265OtherOptionsGroupbox.Name = "x265OtherOptionsGroupbox";
            this.x265OtherOptionsGroupbox.Size = new System.Drawing.Size(284, 123);
            this.x265OtherOptionsGroupbox.TabIndex = 19;
            this.x265OtherOptionsGroupbox.TabStop = false;
            this.x265OtherOptionsGroupbox.Text = "Motion Estimation";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(8, 19);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(68, 13);
            this.label17.TabIndex = 9;
            this.label17.Text = "Chroma M.E.";
            // 
            // x265SubpelRefinement
            // 
            this.x265SubpelRefinement.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.x265SubpelRefinement.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.x265SubpelRefinement.Items.AddRange(new object[] {
            "00 - Fullpel Only (not recommended)",
            "01 - QPel SAD",
            "02 - QPel SATD",
            "03 - HPel on MB then QPel",
            "04 - Always QPel",
            "05 - QPel & Bidir ME",
            "06 - RD on I/P frames",
            "07 - RD on all frames",
            "08 - RD refinement on I/P frames",
            "09 - RD refinement on all frames",
            "10 - QP-RD",
            "11 - Full RD: disable all early terminations"});
            this.x265SubpelRefinement.Location = new System.Drawing.Point(125, 96);
            this.x265SubpelRefinement.Name = "x265SubpelRefinement";
            this.x265SubpelRefinement.Size = new System.Drawing.Size(154, 21);
            this.x265SubpelRefinement.TabIndex = 8;
            this.x265SubpelRefinement.SelectedIndexChanged += new System.EventHandler(this.updateEvent);
            // 
            // x265SubpelRefinementLabel
            // 
            this.x265SubpelRefinementLabel.AutoSize = true;
            this.x265SubpelRefinementLabel.Location = new System.Drawing.Point(8, 98);
            this.x265SubpelRefinementLabel.Name = "x265SubpelRefinementLabel";
            this.x265SubpelRefinementLabel.Size = new System.Drawing.Size(104, 13);
            this.x265SubpelRefinementLabel.TabIndex = 7;
            this.x265SubpelRefinementLabel.Text = "Subpixel Refinement";
            this.x265SubpelRefinementLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // x265ChromaMe
            // 
            this.x265ChromaMe.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.x265ChromaMe.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.x265ChromaMe.Checked = true;
            this.x265ChromaMe.CheckState = System.Windows.Forms.CheckState.Checked;
            this.x265ChromaMe.Location = new System.Drawing.Point(230, 15);
            this.x265ChromaMe.Name = "x265ChromaMe";
            this.x265ChromaMe.Size = new System.Drawing.Size(48, 23);
            this.x265ChromaMe.TabIndex = 0;
            this.x265ChromaMe.CheckedChanged += new System.EventHandler(this.updateEvent);
            // 
            // x265MERangeLabel
            // 
            this.x265MERangeLabel.AutoSize = true;
            this.x265MERangeLabel.Location = new System.Drawing.Point(8, 46);
            this.x265MERangeLabel.Name = "x265MERangeLabel";
            this.x265MERangeLabel.Size = new System.Drawing.Size(64, 13);
            this.x265MERangeLabel.TabIndex = 1;
            this.x265MERangeLabel.Text = "M.E. Range";
            this.x265MERangeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // x265METypeLabel
            // 
            this.x265METypeLabel.AutoSize = true;
            this.x265METypeLabel.Location = new System.Drawing.Point(8, 72);
            this.x265METypeLabel.Name = "x265METypeLabel";
            this.x265METypeLabel.Size = new System.Drawing.Size(75, 13);
            this.x265METypeLabel.TabIndex = 5;
            this.x265METypeLabel.Text = "M.E. Algorithm";
            this.x265METypeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // x265METype
            // 
            this.x265METype.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.x265METype.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.x265METype.Items.AddRange(new object[] {
            "Diamond",
            "Hexagon",
            "Multi hex",
            "Exhaustive",
            "SATD Exhaustive"});
            this.x265METype.Location = new System.Drawing.Point(125, 70);
            this.x265METype.Name = "x265METype";
            this.x265METype.Size = new System.Drawing.Size(154, 21);
            this.x265METype.TabIndex = 6;
            this.x265METype.SelectedIndexChanged += new System.EventHandler(this.updateEvent);
            // 
            // x265MERange
            // 
            this.x265MERange.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.x265MERange.Location = new System.Drawing.Point(231, 44);
            this.x265MERange.Maximum = new decimal(new int[] {
            64,
            0,
            0,
            0});
            this.x265MERange.Minimum = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.x265MERange.Name = "x265MERange";
            this.x265MERange.Size = new System.Drawing.Size(48, 20);
            this.x265MERange.TabIndex = 2;
            this.x265MERange.Value = new decimal(new int[] {
            16,
            0,
            0,
            0});
            this.x265MERange.ValueChanged += new System.EventHandler(this.updateEvent);
            // 
            // advancedSettings
            // 
            this.advancedSettings.AutoSize = true;
            this.advancedSettings.Enabled = false;
            this.advancedSettings.Location = new System.Drawing.Point(278, 188);
            this.advancedSettings.Name = "advancedSettings";
            this.advancedSettings.Size = new System.Drawing.Size(146, 17);
            this.advancedSettings.TabIndex = 16;
            this.advancedSettings.Text = "Show Advanced Settings";
            this.advancedSettings.UseVisualStyleBackColor = true;
            this.advancedSettings.Visible = false;
            this.advancedSettings.CheckedChanged += new System.EventHandler(this.advancedSettings_CheckedChanged);
            // 
            // PsyTrellisLabel
            // 
            this.PsyTrellisLabel.AutoSize = true;
            this.PsyTrellisLabel.Location = new System.Drawing.Point(5, 147);
            this.PsyTrellisLabel.Name = "PsyTrellisLabel";
            this.PsyTrellisLabel.Padding = new System.Windows.Forms.Padding(3);
            this.PsyTrellisLabel.Size = new System.Drawing.Size(103, 19);
            this.PsyTrellisLabel.TabIndex = 11;
            this.PsyTrellisLabel.Text = "Psy-Trellis Strength";
            // 
            // PsyRDLabel
            // 
            this.PsyRDLabel.AutoSize = true;
            this.PsyRDLabel.Location = new System.Drawing.Point(5, 122);
            this.PsyRDLabel.Name = "PsyRDLabel";
            this.PsyRDLabel.Padding = new System.Windows.Forms.Padding(3);
            this.PsyRDLabel.Size = new System.Drawing.Size(92, 19);
            this.PsyRDLabel.TabIndex = 9;
            this.PsyRDLabel.Text = "Psy-RD Strength";
            // 
            // x265NumberOfRefFramesLabel
            // 
            this.x265NumberOfRefFramesLabel.AutoSize = true;
            this.x265NumberOfRefFramesLabel.Location = new System.Drawing.Point(4, 42);
            this.x265NumberOfRefFramesLabel.Name = "x265NumberOfRefFramesLabel";
            this.x265NumberOfRefFramesLabel.Padding = new System.Windows.Forms.Padding(3);
            this.x265NumberOfRefFramesLabel.Size = new System.Drawing.Size(152, 19);
            this.x265NumberOfRefFramesLabel.TabIndex = 2;
            this.x265NumberOfRefFramesLabel.Text = "Number of Reference Frames";
            this.x265NumberOfRefFramesLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // trellisLabel
            // 
            this.trellisLabel.AutoSize = true;
            this.trellisLabel.Location = new System.Drawing.Point(5, 93);
            this.trellisLabel.Name = "trellisLabel";
            this.trellisLabel.Padding = new System.Windows.Forms.Padding(3);
            this.trellisLabel.Size = new System.Drawing.Size(40, 19);
            this.trellisLabel.TabIndex = 0;
            this.trellisLabel.Text = "Trellis";
            this.trellisLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // MiscTabPage
            // 
            this.MiscTabPage.Controls.Add(this.gbx265CustomCmd);
            this.MiscTabPage.Location = new System.Drawing.Point(4, 22);
            this.MiscTabPage.Name = "MiscTabPage";
            this.MiscTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.MiscTabPage.Size = new System.Drawing.Size(502, 242);
            this.MiscTabPage.TabIndex = 6;
            this.MiscTabPage.Text = "Misc";
            this.MiscTabPage.UseVisualStyleBackColor = true;
            // 
            // gbx265CustomCmd
            // 
            this.gbx265CustomCmd.Controls.Add(this.customCommandlineOptions);
            this.gbx265CustomCmd.Location = new System.Drawing.Point(6, 6);
            this.gbx265CustomCmd.Name = "gbx265CustomCmd";
            this.gbx265CustomCmd.Size = new System.Drawing.Size(490, 65);
            this.gbx265CustomCmd.TabIndex = 27;
            this.gbx265CustomCmd.TabStop = false;
            this.gbx265CustomCmd.Text = " Custom Command Line ";
            // 
            // customCommandlineOptions
            // 
            this.customCommandlineOptions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.customCommandlineOptions.Location = new System.Drawing.Point(15, 19);
            this.customCommandlineOptions.Multiline = true;
            this.customCommandlineOptions.Name = "customCommandlineOptions";
            this.customCommandlineOptions.Size = new System.Drawing.Size(464, 34);
            this.customCommandlineOptions.TabIndex = 0;
            this.customCommandlineOptions.TextChanged += new System.EventHandler(this.updateEvent);
            // 
            // x265ThreadsLabel
            // 
            this.x265ThreadsLabel.AutoSize = true;
            this.x265ThreadsLabel.Location = new System.Drawing.Point(18, 201);
            this.x265ThreadsLabel.Name = "x265ThreadsLabel";
            this.x265ThreadsLabel.Size = new System.Drawing.Size(127, 13);
            this.x265ThreadsLabel.TabIndex = 17;
            this.x265ThreadsLabel.Text = "Frame-Threads (0 = Auto)";
            // 
            // x265NbThreads
            // 
            this.x265NbThreads.Location = new System.Drawing.Point(160, 199);
            this.x265NbThreads.Name = "x265NbThreads";
            this.x265NbThreads.Size = new System.Drawing.Size(43, 20);
            this.x265NbThreads.TabIndex = 18;
            this.x265NbThreads.ValueChanged += new System.EventHandler(this.updateEvent);
            // 
            // x265ConfigurationPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Name = "x265ConfigurationPanel";
            this.Size = new System.Drawing.Size(510, 362);
            this.tabControl1.ResumeLayout(false);
            this.mainTabPage.ResumeLayout(false);
            this.mainTabPage.PerformLayout();
            this.x265CodecGeneralGroupbox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.x265BitrateQuantizer)).EndInit();
            this.FrameTypeTabPage.ResumeLayout(false);
            this.gbSlicing.ResumeLayout(false);
            this.gbSlicing.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.maxSliceSizeMB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxSliceSizeBytes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.slicesnb)).EndInit();
            this.gbFTOther.ResumeLayout(false);
            this.gbFTOther.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.x265NumberOfRefFrames)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.x265SCDSensitivity)).EndInit();
            this.x265GeneralBFramesgGroupbox.ResumeLayout(false);
            this.x265GeneralBFramesgGroupbox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.x265BframeBias)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.x265NumberOfBFrames)).EndInit();
            this.gbH264Features.ResumeLayout(false);
            this.gbH264Features.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.x265BetaDeblock)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.x265AlphaDeblock)).EndInit();
            this.gbGOPSize.ResumeLayout(false);
            this.gbGOPSize.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.x265KeyframeInterval)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.x265MinGOPSize)).EndInit();
            this.RCTabPage.ResumeLayout(false);
            this.x265RCGroupbox.ResumeLayout(false);
            this.x265RCGroupbox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lookahead)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.x265VBVInitialBuffer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.x265VBVMaxRate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.x265TempQuantBlur)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.x265TempFrameComplexityBlur)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.x265QuantizerCompression)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.x265VBVBufferSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.x265RateTol)).EndInit();
            this.gbAQ.ResumeLayout(false);
            this.gbAQ.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numAQStrength)).EndInit();
            this.quantizerMatrixGroupbox.ResumeLayout(false);
            this.x265QuantizerGroupBox.ResumeLayout(false);
            this.x265QuantizerGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.deadzoneIntra)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.deadzoneInter)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.x265PBFrameFactor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.x265IPFrameFactor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.x265CreditsQuantizer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.x265ChromaQPOffset)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.x265MaxQuantDelta)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.x265MaximumQuantizer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.x265MinimimQuantizer)).EndInit();
            this.gbPresets.ResumeLayout(false);
            this.gbPresets.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbx265Presets)).EndInit();
            this.gbTunes.ResumeLayout(false);
            this.AnalysisTabPage.ResumeLayout(false);
            this.x265Bluray.ResumeLayout(false);
            this.x265Bluray.PerformLayout();
            this.x265QuantOptionsGroupbox.ResumeLayout(false);
            this.x265QuantOptionsGroupbox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NoiseReduction)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PsyTrellis)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PsyRD)).EndInit();
            this.x265MBGroupbox.ResumeLayout(false);
            this.x265MBGroupbox.PerformLayout();
            this.x265OtherOptionsGroupbox.ResumeLayout(false);
            this.x265OtherOptionsGroupbox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.x265MERange)).EndInit();
            this.MiscTabPage.ResumeLayout(false);
            this.gbx265CustomCmd.ResumeLayout(false);
            this.gbx265CustomCmd.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.x265NbThreads)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox x265CodecGeneralGroupbox;
        private System.Windows.Forms.ComboBox x265EncodingMode;
        private System.Windows.Forms.Label x265BitrateQuantizerLabel;
        private System.Windows.Forms.TabPage FrameTypeTabPage;
        private System.Windows.Forms.TabPage RCTabPage;
        private System.Windows.Forms.GroupBox gbGOPSize;
        private System.Windows.Forms.Label x265KeyframeIntervalLabel;
        private System.Windows.Forms.NumericUpDown x265KeyframeInterval;
        private System.Windows.Forms.NumericUpDown x265MinGOPSize;
        private System.Windows.Forms.Label x265MinGOPSizeLabel;
        private System.Windows.Forms.GroupBox quantizerMatrixGroupbox;
        private System.Windows.Forms.GroupBox x265QuantizerGroupBox;
        private System.Windows.Forms.NumericUpDown x265CreditsQuantizer;
        private System.Windows.Forms.Label x265CreditsQuantizerLabel;
        private System.Windows.Forms.NumericUpDown x265ChromaQPOffset;
        private System.Windows.Forms.Label x265ChromaQPOffsetLabel;
        private System.Windows.Forms.NumericUpDown x265MaxQuantDelta;
        private System.Windows.Forms.NumericUpDown x265MaximumQuantizer;
        private System.Windows.Forms.NumericUpDown x265MinimimQuantizer;
        private System.Windows.Forms.Label x265MinimimQuantizerLabel;
        private System.Windows.Forms.Label customCommandlineOptionsLabel;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown deadzoneIntra;
        private System.Windows.Forms.NumericUpDown deadzoneInter;
        private System.Windows.Forms.NumericUpDown x265BitrateQuantizer;
        private MeGUI.core.gui.FileSCBox cqmComboBox1;
        private System.Windows.Forms.NumericUpDown x265IPFrameFactor;
        private System.Windows.Forms.Label lbQuantizersRatio;
        private System.Windows.Forms.NumericUpDown x265PBFrameFactor;
        private System.Windows.Forms.Label lbx265DeadZones;
        private System.Windows.Forms.GroupBox gbAQ;
        private System.Windows.Forms.NumericUpDown numAQStrength;
        private System.Windows.Forms.Label lbAQStrength;
        private System.Windows.Forms.ComboBox cbAQMode;
        private System.Windows.Forms.Label lbAQMode;
        private MeGUI.core.gui.HelpButton helpButton1;
        private System.Windows.Forms.GroupBox gbPresets;
        private System.Windows.Forms.TrackBar tbx265Presets;
        private System.Windows.Forms.Label lbPreset;
        private System.Windows.Forms.GroupBox gbTunes;
        private System.Windows.Forms.ComboBox x265Tunes;
        private System.Windows.Forms.TabPage AnalysisTabPage;
        private System.Windows.Forms.CheckBox advancedSettings;
        private System.Windows.Forms.GroupBox gbH264Features;
        private System.Windows.Forms.NumericUpDown x265BetaDeblock;
        private System.Windows.Forms.NumericUpDown x265AlphaDeblock;
        private System.Windows.Forms.CheckBox x265DeblockActive;
        private System.Windows.Forms.Label x265BetaDeblockLabel;
        private System.Windows.Forms.Label x265AlphaDeblockLabel;
        private System.Windows.Forms.GroupBox x265GeneralBFramesgGroupbox;
        private System.Windows.Forms.Label x265AdaptiveBframesLabel;
        private System.Windows.Forms.ComboBox x265NewAdaptiveBframes;
        private System.Windows.Forms.Label x265NumberOfBFramesLabel;
        private System.Windows.Forms.NumericUpDown x265NumberOfBFrames;
        private System.Windows.Forms.Label PsyTrellisLabel;
        private System.Windows.Forms.Label PsyRDLabel;
        private System.Windows.Forms.Label x265NumberOfRefFramesLabel;
        private System.Windows.Forms.Label trellisLabel;
        private System.Windows.Forms.CheckBox cabac;
        private System.Windows.Forms.GroupBox gbFTOther;
        private System.Windows.Forms.NumericUpDown x265NumberOfRefFrames;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown x265SCDSensitivity;
        private System.Windows.Forms.NumericUpDown NoiseReduction;
        private System.Windows.Forms.Label NoiseReductionLabel;
        private System.Windows.Forms.CheckBox scenecut;
        private System.Windows.Forms.Label lbExtraIFframes;
        private System.Windows.Forms.CheckBox x265WeightedBPrediction;
        private System.Windows.Forms.NumericUpDown x265BframeBias;
        private System.Windows.Forms.Label x265BframeBiasLabel;
        private System.Windows.Forms.GroupBox x265RCGroupbox;
        private System.Windows.Forms.Label x265RateTolLabel;
        private System.Windows.Forms.NumericUpDown x265VBVInitialBuffer;
        private System.Windows.Forms.Label x265VBVInitialBufferLabel;
        private System.Windows.Forms.NumericUpDown x265VBVMaxRate;
        private System.Windows.Forms.NumericUpDown x265TempQuantBlur;
        private System.Windows.Forms.NumericUpDown x265TempFrameComplexityBlur;
        private System.Windows.Forms.NumericUpDown x265QuantizerCompression;
        private System.Windows.Forms.NumericUpDown x265VBVBufferSize;
        private System.Windows.Forms.Label x265TempQuantBlurLabel;
        private System.Windows.Forms.Label x265TempFrameComplexityBlurLabel;
        private System.Windows.Forms.Label x265QuantizerCompressionLabel;
        private System.Windows.Forms.Label x265VBVMaxRateLabel;
        private System.Windows.Forms.Label x265VBVBufferSizeLabel;
        private System.Windows.Forms.NumericUpDown x265RateTol;
        private System.Windows.Forms.TabPage MiscTabPage;
        private System.Windows.Forms.GroupBox gbx265CustomCmd;
        private System.Windows.Forms.TextBox customCommandlineOptions;
        private System.Windows.Forms.GroupBox x265OtherOptionsGroupbox;
        private System.Windows.Forms.ComboBox x265SubpelRefinement;
        private System.Windows.Forms.Label x265SubpelRefinementLabel;
        private System.Windows.Forms.CheckBox x265ChromaMe;
        private System.Windows.Forms.Label x265MERangeLabel;
        private System.Windows.Forms.Label x265METypeLabel;
        private System.Windows.Forms.ComboBox x265METype;
        private System.Windows.Forms.NumericUpDown x265MERange;
        private System.Windows.Forms.GroupBox x265MBGroupbox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox macroblockOptions;
        private System.Windows.Forms.CheckBox adaptiveDCT;
        private System.Windows.Forms.CheckBox x265I4x4mv;
        private System.Windows.Forms.CheckBox x265I8x8mv;
        private System.Windows.Forms.CheckBox x265P4x4mv;
        private System.Windows.Forms.CheckBox x265B8x8mv;
        private System.Windows.Forms.CheckBox x265P8x8mv;
        private System.Windows.Forms.GroupBox x265QuantOptionsGroupbox;
        private System.Windows.Forms.ComboBox x265BframePredictionMode;
        private System.Windows.Forms.Label x265BframePredictionModeLabel;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown PsyTrellis;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown PsyRD;
        private System.Windows.Forms.CheckBox noDCTDecimateOption;
        private System.Windows.Forms.CheckBox noFastPSkip;
        private System.Windows.Forms.ComboBox trellis;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.NumericUpDown lookahead;
        private System.Windows.Forms.CheckBox mbtree;
        private System.Windows.Forms.CheckBox nopsy;
        private System.Windows.Forms.CheckBox x265MixedReferences;
        private System.Windows.Forms.GroupBox gbSlicing;
        private System.Windows.Forms.NumericUpDown slicesnb;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.NumericUpDown maxSliceSizeBytes;
        private System.Windows.Forms.NumericUpDown maxSliceSizeMB;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.ComboBox cbBPyramid;
        private System.Windows.Forms.ComboBox x265WeightedPPrediction;
        private System.Windows.Forms.Label lblWeightedP;
        private System.Windows.Forms.GroupBox x265Bluray;
        private System.Windows.Forms.CheckBox x265aud;
        //private System.Windows.Forms.ComboBox cbTarget;
        private System.Windows.Forms.Label x265hrdLabel;
        private System.Windows.Forms.ComboBox x265hrd;
        private System.Windows.Forms.CheckBox fakeInterlaced;
        private System.Windows.Forms.Label pullDownLabel;
        private System.Windows.Forms.ComboBox x265PullDown;
        private System.Windows.Forms.ComboBox cbInterlaceMode;
        private System.Windows.Forms.Label lblInterlacedMode;
        private System.Windows.Forms.CheckBox chkBlurayCompat;
        private System.Windows.Forms.CheckBox chkOpenGop;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.ComboBox cbGOPCalculation;
        private System.Windows.Forms.Label x265ThreadsLabel;
        private System.Windows.Forms.NumericUpDown x265NbThreads;
    }
}
