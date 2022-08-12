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

namespace MeGUI.core.details.audio
{
    partial class AudioConfigurationPanel
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
            this.encoderGroupBox = new System.Windows.Forms.GroupBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.Page1 = new System.Windows.Forms.TabPage();
            this.besweetOptionsGroupbox = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.cbTimeModification = new System.Windows.Forms.ComboBox();
            this.cbSampleRate = new System.Windows.Forms.ComboBox();
            this.primaryDecoding = new System.Windows.Forms.ComboBox();
            this.cbDownmixMode = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.BesweetChannelsLabel = new System.Windows.Forms.Label();
            this.lbSampleRate = new System.Windows.Forms.Label();
            this.autoGain = new System.Windows.Forms.CheckBox();
            this.normalize = new System.Windows.Forms.NumericUpDown();
            this.applyDRC = new System.Windows.Forms.CheckBox();
            this.lbTimeModification = new System.Windows.Forms.Label();
            this.Page2 = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbAudioCLI = new System.Windows.Forms.TextBox();
            this.tabControl1.SuspendLayout();
            this.Page1.SuspendLayout();
            this.besweetOptionsGroupbox.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.normalize)).BeginInit();
            this.Page2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // encoderGroupBox
            // 
            this.encoderGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.encoderGroupBox.Location = new System.Drawing.Point(4, 212);
            this.encoderGroupBox.Name = "encoderGroupBox";
            this.encoderGroupBox.Size = new System.Drawing.Size(402, 106);
            this.encoderGroupBox.TabIndex = 9;
            this.encoderGroupBox.TabStop = false;
            this.encoderGroupBox.Text = "placeholder for encoder options";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.Page1);
            this.tabControl1.Controls.Add(this.Page2);
            this.tabControl1.Location = new System.Drawing.Point(4, 3);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(404, 203);
            this.tabControl1.TabIndex = 10;
            // 
            // Page1
            // 
            this.Page1.Controls.Add(this.besweetOptionsGroupbox);
            this.Page1.Location = new System.Drawing.Point(4, 22);
            this.Page1.Name = "Page1";
            this.Page1.Padding = new System.Windows.Forms.Padding(3);
            this.Page1.Size = new System.Drawing.Size(396, 177);
            this.Page1.TabIndex = 0;
            this.Page1.Text = "General";
            this.Page1.UseVisualStyleBackColor = true;
            // 
            // besweetOptionsGroupbox
            // 
            this.besweetOptionsGroupbox.Controls.Add(this.tableLayoutPanel1);
            this.besweetOptionsGroupbox.Location = new System.Drawing.Point(3, 0);
            this.besweetOptionsGroupbox.Name = "besweetOptionsGroupbox";
            this.besweetOptionsGroupbox.Size = new System.Drawing.Size(387, 174);
            this.besweetOptionsGroupbox.TabIndex = 9;
            this.besweetOptionsGroupbox.TabStop = false;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.cbTimeModification, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.cbSampleRate, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.primaryDecoding, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.cbDownmixMode, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.BesweetChannelsLabel, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.lbSampleRate, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.lbTimeModification, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.normalize, 2, 4);
            this.tableLayoutPanel1.Controls.Add(this.applyDRC, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.autoGain, 1, 4);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 6;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(381, 155);
            this.tableLayoutPanel1.TabIndex = 13;
            // 
            // cbTimeModification
            // 
            this.cbTimeModification.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.cbTimeModification, 2);
            this.cbTimeModification.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbTimeModification.FormattingEnabled = true;
            this.cbTimeModification.Location = new System.Drawing.Point(103, 81);
            this.cbTimeModification.Name = "cbTimeModification";
            this.cbTimeModification.Size = new System.Drawing.Size(275, 21);
            this.cbTimeModification.TabIndex = 17;
            // 
            // cbSampleRate
            // 
            this.cbSampleRate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.cbSampleRate, 2);
            this.cbSampleRate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSampleRate.FormattingEnabled = true;
            this.cbSampleRate.Location = new System.Drawing.Point(103, 55);
            this.cbSampleRate.Name = "cbSampleRate";
            this.cbSampleRate.Size = new System.Drawing.Size(275, 21);
            this.cbSampleRate.TabIndex = 15;
            // 
            // primaryDecoding
            // 
            this.primaryDecoding.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.primaryDecoding, 2);
            this.primaryDecoding.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.primaryDecoding.FormattingEnabled = true;
            this.primaryDecoding.Location = new System.Drawing.Point(103, 3);
            this.primaryDecoding.Name = "primaryDecoding";
            this.primaryDecoding.Size = new System.Drawing.Size(275, 21);
            this.primaryDecoding.TabIndex = 13;
            // 
            // cbDownmixMode
            // 
            this.cbDownmixMode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.cbDownmixMode, 2);
            this.cbDownmixMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDownmixMode.Location = new System.Drawing.Point(103, 29);
            this.cbDownmixMode.Name = "cbDownmixMode";
            this.cbDownmixMode.Size = new System.Drawing.Size(275, 21);
            this.cbDownmixMode.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(94, 26);
            this.label1.TabIndex = 14;
            this.label1.Text = "Preferred Decoder";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // BesweetChannelsLabel
            // 
            this.BesweetChannelsLabel.AutoSize = true;
            this.BesweetChannelsLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BesweetChannelsLabel.Location = new System.Drawing.Point(3, 26);
            this.BesweetChannelsLabel.Name = "BesweetChannelsLabel";
            this.BesweetChannelsLabel.Size = new System.Drawing.Size(94, 26);
            this.BesweetChannelsLabel.TabIndex = 2;
            this.BesweetChannelsLabel.Text = "Output Channels";
            this.BesweetChannelsLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbSampleRate
            // 
            this.lbSampleRate.AutoSize = true;
            this.lbSampleRate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbSampleRate.Location = new System.Drawing.Point(3, 52);
            this.lbSampleRate.Name = "lbSampleRate";
            this.lbSampleRate.Size = new System.Drawing.Size(94, 26);
            this.lbSampleRate.TabIndex = 11;
            this.lbSampleRate.Text = "Sample Rate";
            this.lbSampleRate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // autoGain
            // 
            this.autoGain.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.autoGain.AutoSize = true;
            this.autoGain.Location = new System.Drawing.Point(103, 107);
            this.autoGain.Name = "autoGain";
            this.autoGain.Size = new System.Drawing.Size(117, 20);
            this.autoGain.TabIndex = 6;
            this.autoGain.Text = "Normalize Peaks to";
            // 
            // normalize
            // 
            this.normalize.Location = new System.Drawing.Point(303, 107);
            this.normalize.Name = "normalize";
            this.normalize.Size = new System.Drawing.Size(52, 20);
            this.normalize.TabIndex = 10;
            this.normalize.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // applyDRC
            // 
            this.applyDRC.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.applyDRC.AutoSize = true;
            this.applyDRC.Location = new System.Drawing.Point(103, 136);
            this.applyDRC.Name = "applyDRC";
            this.applyDRC.Size = new System.Drawing.Size(194, 17);
            this.applyDRC.TabIndex = 9;
            this.applyDRC.Text = "Apply Dynamic Range Compression";
            this.applyDRC.UseVisualStyleBackColor = true;
            // 
            // lbTimeModification
            // 
            this.lbTimeModification.AutoSize = true;
            this.lbTimeModification.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbTimeModification.Location = new System.Drawing.Point(3, 78);
            this.lbTimeModification.Name = "lbTimeModification";
            this.lbTimeModification.Size = new System.Drawing.Size(94, 26);
            this.lbTimeModification.TabIndex = 18;
            this.lbTimeModification.Text = "Time Modification";
            this.lbTimeModification.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Page2
            // 
            this.Page2.Controls.Add(this.groupBox1);
            this.Page2.Location = new System.Drawing.Point(4, 22);
            this.Page2.Name = "Page2";
            this.Page2.Padding = new System.Windows.Forms.Padding(3);
            this.Page2.Size = new System.Drawing.Size(396, 177);
            this.Page2.TabIndex = 1;
            this.Page2.Text = "Extra";
            this.Page2.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.tbAudioCLI);
            this.groupBox1.Location = new System.Drawing.Point(3, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(387, 174);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(121, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Custom Command Line :";
            // 
            // tbAudioCLI
            // 
            this.tbAudioCLI.Location = new System.Drawing.Point(6, 36);
            this.tbAudioCLI.Name = "tbAudioCLI";
            this.tbAudioCLI.Size = new System.Drawing.Size(372, 20);
            this.tbAudioCLI.TabIndex = 0;
            // 
            // AudioConfigurationPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.encoderGroupBox);
            this.Name = "AudioConfigurationPanel";
            this.Size = new System.Drawing.Size(411, 321);
            this.tabControl1.ResumeLayout(false);
            this.Page1.ResumeLayout(false);
            this.besweetOptionsGroupbox.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.normalize)).EndInit();
            this.Page2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        protected System.Windows.Forms.GroupBox encoderGroupBox;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage Page1;
        protected System.Windows.Forms.GroupBox besweetOptionsGroupbox;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.ComboBox cbTimeModification;
        private System.Windows.Forms.ComboBox cbSampleRate;
        private System.Windows.Forms.ComboBox primaryDecoding;
        private System.Windows.Forms.ComboBox cbDownmixMode;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label BesweetChannelsLabel;
        private System.Windows.Forms.Label lbSampleRate;
        private System.Windows.Forms.CheckBox autoGain;
        private System.Windows.Forms.NumericUpDown normalize;
        private System.Windows.Forms.CheckBox applyDRC;
        private System.Windows.Forms.Label lbTimeModification;
        private System.Windows.Forms.TabPage Page2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbAudioCLI;

    }
}