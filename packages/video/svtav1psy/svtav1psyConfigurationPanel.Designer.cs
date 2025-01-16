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

namespace MeGUI.packages.video.svtav1psy
{
    partial class svtav1psyConfigurationPanel
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
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.customCommandlineOptionsLabel = new System.Windows.Forms.Label();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.helpButton1 = new MeGUI.core.gui.HelpButton();
            this.MiscTabPage = new System.Windows.Forms.TabPage();
            this.gbffv1CustomCmd = new System.Windows.Forms.GroupBox();
            this.customCommandlineOptions = new System.Windows.Forms.TextBox();
            this.ffv1CodecGeneralGroupbox = new System.Windows.Forms.GroupBox();
            this.svtBitrateQuantizer = new System.Windows.Forms.NumericUpDown();
            this.svtEncodingMode = new System.Windows.Forms.ComboBox();
            this.svtBitrateQuantizerLabel = new System.Windows.Forms.Label();
            this.gbPresets = new System.Windows.Forms.GroupBox();
            this.lbPreset = new System.Windows.Forms.Label();
            this.tbsvtPresets = new System.Windows.Forms.TrackBar();
            this.gbTunes = new System.Windows.Forms.GroupBox();
            this.svtTunes = new System.Windows.Forms.ComboBox();
            this.chSvt10Bits = new System.Windows.Forms.CheckBox();
            this.tabControl1.SuspendLayout();
            this.mainTabPage.SuspendLayout();
            this.MiscTabPage.SuspendLayout();
            this.gbffv1CustomCmd.SuspendLayout();
            this.ffv1CodecGeneralGroupbox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.svtBitrateQuantizer)).BeginInit();
            this.gbPresets.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbsvtPresets)).BeginInit();
            this.gbTunes.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.MiscTabPage);
            this.tabControl1.Size = new System.Drawing.Size(490, 288);
            this.tabControl1.SizeMode = System.Windows.Forms.TabSizeMode.FillToRight;
            this.tabControl1.TabIndex = 0;
            this.tabControl1.Controls.SetChildIndex(this.MiscTabPage, 0);
            this.tabControl1.Controls.SetChildIndex(this.mainTabPage, 0);
            // 
            // commandline
            // 
            this.commandline.Location = new System.Drawing.Point(0, 290);
            this.commandline.Size = new System.Drawing.Size(487, 89);
            this.commandline.TabIndex = 1;
            this.commandline.Text = " ";
            // 
            // mainTabPage
            // 
            this.mainTabPage.Controls.Add(this.chSvt10Bits);
            this.mainTabPage.Controls.Add(this.gbTunes);
            this.mainTabPage.Controls.Add(this.gbPresets);
            this.mainTabPage.Controls.Add(this.ffv1CodecGeneralGroupbox);
            this.mainTabPage.Controls.Add(this.helpButton1);
            this.mainTabPage.Size = new System.Drawing.Size(482, 262);
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
            this.helpButton1.ArticleName = "Configuration/Video_Encoder_Configuration/SVTAV1PSY";
            this.helpButton1.AutoSize = true;
            this.helpButton1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.helpButton1.Location = new System.Drawing.Point(431, 176);
            this.helpButton1.Name = "helpButton1";
            this.helpButton1.Size = new System.Drawing.Size(39, 23);
            this.helpButton1.TabIndex = 10;
            // 
            // MiscTabPage
            // 
            this.MiscTabPage.Controls.Add(this.gbffv1CustomCmd);
            this.MiscTabPage.Location = new System.Drawing.Point(4, 22);
            this.MiscTabPage.Name = "MiscTabPage";
            this.MiscTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.MiscTabPage.Size = new System.Drawing.Size(482, 262);
            this.MiscTabPage.TabIndex = 6;
            this.MiscTabPage.Text = "Misc";
            this.MiscTabPage.UseVisualStyleBackColor = true;
            // 
            // gbffv1CustomCmd
            // 
            this.gbffv1CustomCmd.Controls.Add(this.customCommandlineOptions);
            this.gbffv1CustomCmd.Location = new System.Drawing.Point(6, 6);
            this.gbffv1CustomCmd.Name = "gbffv1CustomCmd";
            this.gbffv1CustomCmd.Size = new System.Drawing.Size(490, 65);
            this.gbffv1CustomCmd.TabIndex = 27;
            this.gbffv1CustomCmd.TabStop = false;
            this.gbffv1CustomCmd.Text = " Custom Command Line ";
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
            // 
            // ffv1CodecGeneralGroupbox
            // 
            this.ffv1CodecGeneralGroupbox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ffv1CodecGeneralGroupbox.Controls.Add(this.svtBitrateQuantizer);
            this.ffv1CodecGeneralGroupbox.Controls.Add(this.svtEncodingMode);
            this.ffv1CodecGeneralGroupbox.Controls.Add(this.svtBitrateQuantizerLabel);
            this.ffv1CodecGeneralGroupbox.Location = new System.Drawing.Point(6, 6);
            this.ffv1CodecGeneralGroupbox.Name = "ffv1CodecGeneralGroupbox";
            this.ffv1CodecGeneralGroupbox.Size = new System.Drawing.Size(315, 48);
            this.ffv1CodecGeneralGroupbox.TabIndex = 46;
            this.ffv1CodecGeneralGroupbox.TabStop = false;
            this.ffv1CodecGeneralGroupbox.Text = " Encoding Mode ";
            // 
            // svtBitrateQuantizer
            // 
            this.svtBitrateQuantizer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.svtBitrateQuantizer.Location = new System.Drawing.Point(254, 20);
            this.svtBitrateQuantizer.Maximum = new decimal(new int[] {
            300000,
            0,
            0,
            0});
            this.svtBitrateQuantizer.Name = "svtBitrateQuantizer";
            this.svtBitrateQuantizer.Size = new System.Drawing.Size(55, 20);
            this.svtBitrateQuantizer.TabIndex = 48;
            this.svtBitrateQuantizer.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.svtBitrateQuantizer.ValueChanged += new System.EventHandler(this.updateEvent);
            // 
            // svtEncodingMode
            // 
            this.svtEncodingMode.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.svtEncodingMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.svtEncodingMode.Items.AddRange(new object[] {
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
            this.svtEncodingMode.Location = new System.Drawing.Point(6, 19);
            this.svtEncodingMode.MaxDropDownItems = 2;
            this.svtEncodingMode.Name = "svtEncodingMode";
            this.svtEncodingMode.Size = new System.Drawing.Size(153, 21);
            this.svtEncodingMode.TabIndex = 2;
            this.svtEncodingMode.SelectedIndexChanged += new System.EventHandler(this.updateEvent);
            // 
            // svtBitrateQuantizerLabel
            // 
            this.svtBitrateQuantizerLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.svtBitrateQuantizerLabel.Location = new System.Drawing.Point(178, 19);
            this.svtBitrateQuantizerLabel.Margin = new System.Windows.Forms.Padding(3);
            this.svtBitrateQuantizerLabel.Name = "svtBitrateQuantizerLabel";
            this.svtBitrateQuantizerLabel.Padding = new System.Windows.Forms.Padding(3);
            this.svtBitrateQuantizerLabel.Size = new System.Drawing.Size(70, 23);
            this.svtBitrateQuantizerLabel.TabIndex = 47;
            this.svtBitrateQuantizerLabel.Text = "Bitrate";
            this.svtBitrateQuantizerLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // gbPresets
            // 
            this.gbPresets.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbPresets.BackColor = System.Drawing.Color.Transparent;
            this.gbPresets.Controls.Add(this.lbPreset);
            this.gbPresets.Controls.Add(this.tbsvtPresets);
            this.gbPresets.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbPresets.Location = new System.Drawing.Point(6, 59);
            this.gbPresets.Name = "gbPresets";
            this.gbPresets.Size = new System.Drawing.Size(470, 101);
            this.gbPresets.TabIndex = 47;
            this.gbPresets.TabStop = false;
            this.gbPresets.Text = " Preset ";
            // 
            // lbPreset
            // 
            this.lbPreset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lbPreset.AutoSize = true;
            this.lbPreset.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbPreset.Location = new System.Drawing.Point(83, 74);
            this.lbPreset.Name = "lbPreset";
            this.lbPreset.Size = new System.Drawing.Size(336, 13);
            this.lbPreset.TabIndex = 1;
            this.lbPreset.Text = "Higher Presets means Faster encodes but with a quality tradeoff";
            // 
            // tbsvtPresets
            // 
            this.tbsvtPresets.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbsvtPresets.AutoSize = false;
            this.tbsvtPresets.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.tbsvtPresets.Location = new System.Drawing.Point(6, 32);
            this.tbsvtPresets.Maximum = 13;
            this.tbsvtPresets.Name = "tbsvtPresets";
            this.tbsvtPresets.Size = new System.Drawing.Size(458, 30);
            this.tbsvtPresets.TabIndex = 0;
            this.tbsvtPresets.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.tbsvtPresets.Value = 10;
            this.tbsvtPresets.Scroll += new System.EventHandler(this.tbsvtPresets_Scroll);
            // 
            // gbTunes
            // 
            this.gbTunes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.gbTunes.Controls.Add(this.svtTunes);
            this.gbTunes.Location = new System.Drawing.Point(327, 6);
            this.gbTunes.Name = "gbTunes";
            this.gbTunes.Size = new System.Drawing.Size(149, 48);
            this.gbTunes.TabIndex = 48;
            this.gbTunes.TabStop = false;
            this.gbTunes.Text = " Tuning ";
            // 
            // svtTunes
            // 
            this.svtTunes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.svtTunes.FormattingEnabled = true;
            this.svtTunes.Location = new System.Drawing.Point(10, 19);
            this.svtTunes.Name = "svtTunes";
            this.svtTunes.Size = new System.Drawing.Size(133, 21);
            this.svtTunes.TabIndex = 0;
            this.svtTunes.SelectedIndexChanged += new System.EventHandler(this.svtTunes_SelectedIndexChanged);
            // 
            // chSvt10Bits
            // 
            this.chSvt10Bits.AutoSize = true;
            this.chSvt10Bits.Location = new System.Drawing.Point(12, 176);
            this.chSvt10Bits.Name = "chSvt10Bits";
            this.chSvt10Bits.Size = new System.Drawing.Size(135, 17);
            this.chSvt10Bits.TabIndex = 49;
            this.chSvt10Bits.Text = "Force 10 bits Encoding";
            this.chSvt10Bits.UseVisualStyleBackColor = true;
            this.chSvt10Bits.CheckedChanged += new System.EventHandler(this.chSvt10Bits_CheckedChanged);
            // 
            // svtav1psyConfigurationPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Name = "svtav1psyConfigurationPanel";
            this.Size = new System.Drawing.Size(490, 382);
            this.tabControl1.ResumeLayout(false);
            this.mainTabPage.ResumeLayout(false);
            this.mainTabPage.PerformLayout();
            this.MiscTabPage.ResumeLayout(false);
            this.gbffv1CustomCmd.ResumeLayout(false);
            this.gbffv1CustomCmd.PerformLayout();
            this.ffv1CodecGeneralGroupbox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.svtBitrateQuantizer)).EndInit();
            this.gbPresets.ResumeLayout(false);
            this.gbPresets.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbsvtPresets)).EndInit();
            this.gbTunes.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label customCommandlineOptionsLabel;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private MeGUI.core.gui.HelpButton helpButton1;
        private System.Windows.Forms.TabPage MiscTabPage;
        private System.Windows.Forms.GroupBox gbffv1CustomCmd;
        private System.Windows.Forms.TextBox customCommandlineOptions;
        private System.Windows.Forms.GroupBox ffv1CodecGeneralGroupbox;
        private System.Windows.Forms.ComboBox svtEncodingMode;
        private System.Windows.Forms.NumericUpDown svtBitrateQuantizer;
        private System.Windows.Forms.Label svtBitrateQuantizerLabel;
        private System.Windows.Forms.GroupBox gbPresets;
        private System.Windows.Forms.Label lbPreset;
        private System.Windows.Forms.TrackBar tbsvtPresets;
        private System.Windows.Forms.GroupBox gbTunes;
        private System.Windows.Forms.ComboBox svtTunes;
        private System.Windows.Forms.CheckBox chSvt10Bits;
    }
}
