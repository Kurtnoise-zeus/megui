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

using System.Windows.Forms;

namespace MeGUI
{
    public partial class AutoEncodeWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AutoEncodeWindow));
            this.AutomaticEncodingGroup = new System.Windows.Forms.GroupBox();
            this.videoSize = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.projectedBitrateKBits = new System.Windows.Forms.TextBox();
            this.targetSize = new MeGUI.core.gui.TargetSizeSCBox();
            this.noTargetRadio = new System.Windows.Forms.RadioButton();
            this.averageBitrateRadio = new System.Windows.Forms.RadioButton();
            this.FileSizeRadio = new System.Windows.Forms.RadioButton();
            this.AverageBitrateLabel = new System.Windows.Forms.Label();
            this.queueButton = new System.Windows.Forms.Button();
            this.OutputGroupBox = new System.Windows.Forms.GroupBox();
            this.device = new System.Windows.Forms.ComboBox();
            this.DeviceLabel = new System.Windows.Forms.Label();
            this.splitting = new MeGUI.core.gui.TargetSizeSCBox();
            this.container = new System.Windows.Forms.ComboBox();
            this.containerLabel = new System.Windows.Forms.Label();
            this.muxedOutputLabel = new System.Windows.Forms.Label();
            this.muxedOutput = new MeGUI.FileBar();
            this.cancelButton = new System.Windows.Forms.Button();
            this.addSubsNChapters = new System.Windows.Forms.CheckBox();
            this.defaultToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.helpButton1 = new MeGUI.core.gui.HelpButton();
            label1 = new System.Windows.Forms.Label();
            this.AutomaticEncodingGroup.SuspendLayout();
            this.OutputGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(191, 23);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(49, 13);
            label1.TabIndex = 27;
            label1.Text = "Splitting:";
            // 
            // AutomaticEncodingGroup
            // 
            this.AutomaticEncodingGroup.Controls.Add(this.videoSize);
            this.AutomaticEncodingGroup.Controls.Add(this.label2);
            this.AutomaticEncodingGroup.Controls.Add(this.projectedBitrateKBits);
            this.AutomaticEncodingGroup.Controls.Add(this.targetSize);
            this.AutomaticEncodingGroup.Controls.Add(this.noTargetRadio);
            this.AutomaticEncodingGroup.Controls.Add(this.averageBitrateRadio);
            this.AutomaticEncodingGroup.Controls.Add(this.FileSizeRadio);
            this.AutomaticEncodingGroup.Controls.Add(this.AverageBitrateLabel);
            this.AutomaticEncodingGroup.Location = new System.Drawing.Point(10, 116);
            this.AutomaticEncodingGroup.Name = "AutomaticEncodingGroup";
            this.AutomaticEncodingGroup.Size = new System.Drawing.Size(456, 106);
            this.AutomaticEncodingGroup.TabIndex = 17;
            this.AutomaticEncodingGroup.TabStop = false;
            this.AutomaticEncodingGroup.Text = "Size and Bitrate";
            // 
            // videoSize
            // 
            this.videoSize.Location = new System.Drawing.Point(310, 48);
            this.videoSize.Name = "videoSize";
            this.videoSize.ReadOnly = true;
            this.videoSize.Size = new System.Drawing.Size(137, 21);
            this.videoSize.TabIndex = 28;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(246, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 13);
            this.label2.TabIndex = 27;
            this.label2.Text = "Video size:";
            // 
            // projectedBitrateKBits
            // 
            this.projectedBitrateKBits.Enabled = false;
            this.projectedBitrateKBits.Location = new System.Drawing.Point(119, 45);
            this.projectedBitrateKBits.Name = "projectedBitrateKBits";
            this.projectedBitrateKBits.Size = new System.Drawing.Size(85, 21);
            this.projectedBitrateKBits.TabIndex = 9;
            this.projectedBitrateKBits.TextChanged += new System.EventHandler(this.projectedBitrate_TextChanged);
            this.projectedBitrateKBits.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textField_KeyPress);
            // 
            // targetSize
            // 
            this.targetSize.CustomSizes = new MeGUI.core.util.FileSize[0];
            this.targetSize.Location = new System.Drawing.Point(116, 15);
            this.targetSize.MaximumSize = new System.Drawing.Size(1000, 29);
            this.targetSize.MinimumSize = new System.Drawing.Size(64, 29);
            this.targetSize.Name = "targetSize";
            this.targetSize.NullString = "Not calculated";
            this.targetSize.SaveCustomValues = false;
            this.targetSize.SelectedIndex = 0;
            this.targetSize.Size = new System.Drawing.Size(208, 29);
            this.targetSize.TabIndex = 25;
            this.targetSize.SelectionChanged += new MeGUI.StringChanged(this.targetSize_SelectionChanged);
            // 
            // noTargetRadio
            // 
            this.noTargetRadio.Location = new System.Drawing.Point(16, 75);
            this.noTargetRadio.Name = "noTargetRadio";
            this.noTargetRadio.Size = new System.Drawing.Size(218, 18);
            this.noTargetRadio.TabIndex = 22;
            this.noTargetRadio.TabStop = true;
            this.noTargetRadio.Text = "No Target Size (use profile settings)";
            this.defaultToolTip.SetToolTip(this.noTargetRadio, "Checking this allows the use of a previously defined bitrate or a non bitrate mod" +
        "e (CQ, CRF)");
            this.noTargetRadio.UseVisualStyleBackColor = true;
            this.noTargetRadio.CheckedChanged += new System.EventHandler(this.calculationMode_CheckedChanged);
            // 
            // averageBitrateRadio
            // 
            this.averageBitrateRadio.AutoSize = true;
            this.averageBitrateRadio.Location = new System.Drawing.Point(16, 49);
            this.averageBitrateRadio.Name = "averageBitrateRadio";
            this.averageBitrateRadio.Size = new System.Drawing.Size(101, 17);
            this.averageBitrateRadio.TabIndex = 16;
            this.averageBitrateRadio.Text = "Average Bitrate";
            this.averageBitrateRadio.CheckedChanged += new System.EventHandler(this.calculationMode_CheckedChanged);
            // 
            // FileSizeRadio
            // 
            this.FileSizeRadio.Checked = true;
            this.FileSizeRadio.Location = new System.Drawing.Point(16, 20);
            this.FileSizeRadio.Name = "FileSizeRadio";
            this.FileSizeRadio.Size = new System.Drawing.Size(100, 18);
            this.FileSizeRadio.TabIndex = 15;
            this.FileSizeRadio.TabStop = true;
            this.FileSizeRadio.Text = "File Size";
            this.FileSizeRadio.CheckedChanged += new System.EventHandler(this.calculationMode_CheckedChanged);
            // 
            // AverageBitrateLabel
            // 
            this.AverageBitrateLabel.AutoSize = true;
            this.AverageBitrateLabel.Location = new System.Drawing.Point(207, 51);
            this.AverageBitrateLabel.Name = "AverageBitrateLabel";
            this.AverageBitrateLabel.Size = new System.Drawing.Size(33, 13);
            this.AverageBitrateLabel.TabIndex = 10;
            this.AverageBitrateLabel.Text = "kbit/s";
            // 
            // queueButton
            // 
            this.queueButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.queueButton.AutoSize = true;
            this.queueButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.queueButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.queueButton.Location = new System.Drawing.Point(358, 228);
            this.queueButton.Name = "queueButton";
            this.queueButton.Size = new System.Drawing.Size(49, 23);
            this.queueButton.TabIndex = 8;
            this.queueButton.Text = "Queue";
            this.queueButton.Click += new System.EventHandler(this.queueButton_Click);
            // 
            // OutputGroupBox
            // 
            this.OutputGroupBox.Controls.Add(this.device);
            this.OutputGroupBox.Controls.Add(this.DeviceLabel);
            this.OutputGroupBox.Controls.Add(label1);
            this.OutputGroupBox.Controls.Add(this.splitting);
            this.OutputGroupBox.Controls.Add(this.container);
            this.OutputGroupBox.Controls.Add(this.containerLabel);
            this.OutputGroupBox.Controls.Add(this.muxedOutputLabel);
            this.OutputGroupBox.Controls.Add(this.muxedOutput);
            this.OutputGroupBox.Location = new System.Drawing.Point(10, 4);
            this.OutputGroupBox.Name = "OutputGroupBox";
            this.OutputGroupBox.Size = new System.Drawing.Size(458, 106);
            this.OutputGroupBox.TabIndex = 18;
            this.OutputGroupBox.TabStop = false;
            this.OutputGroupBox.Text = "Output Options";
            // 
            // device
            // 
            this.device.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.device.FormattingEnabled = true;
            this.device.Items.AddRange(new object[] {
            "Standard"});
            this.device.Location = new System.Drawing.Point(97, 47);
            this.device.Name = "device";
            this.device.Size = new System.Drawing.Size(85, 21);
            this.device.TabIndex = 38;
            // 
            // DeviceLabel
            // 
            this.DeviceLabel.AutoSize = true;
            this.DeviceLabel.Location = new System.Drawing.Point(6, 51);
            this.DeviceLabel.Name = "DeviceLabel";
            this.DeviceLabel.Size = new System.Drawing.Size(39, 13);
            this.DeviceLabel.TabIndex = 37;
            this.DeviceLabel.Text = "Device";
            // 
            // splitting
            // 
            this.splitting.CustomSizes = new MeGUI.core.util.FileSize[0];
            this.splitting.Location = new System.Drawing.Point(243, 16);
            this.splitting.MaximumSize = new System.Drawing.Size(1000, 29);
            this.splitting.MinimumSize = new System.Drawing.Size(64, 29);
            this.splitting.Name = "splitting";
            this.splitting.NullString = "No splitting";
            this.splitting.SaveCustomValues = false;
            this.splitting.SelectedIndex = 0;
            this.splitting.Size = new System.Drawing.Size(208, 29);
            this.splitting.TabIndex = 26;
            // 
            // container
            // 
            this.container.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.container.FormattingEnabled = true;
            this.container.Location = new System.Drawing.Point(97, 20);
            this.container.Name = "container";
            this.container.Size = new System.Drawing.Size(85, 21);
            this.container.TabIndex = 25;
            this.container.SelectedIndexChanged += new System.EventHandler(this.container_SelectedIndexChanged);
            // 
            // containerLabel
            // 
            this.containerLabel.AutoSize = true;
            this.containerLabel.Location = new System.Drawing.Point(6, 23);
            this.containerLabel.Name = "containerLabel";
            this.containerLabel.Size = new System.Drawing.Size(54, 13);
            this.containerLabel.TabIndex = 24;
            this.containerLabel.Text = "Container";
            // 
            // muxedOutputLabel
            // 
            this.muxedOutputLabel.AutoSize = true;
            this.muxedOutputLabel.Location = new System.Drawing.Point(6, 81);
            this.muxedOutputLabel.Name = "muxedOutputLabel";
            this.muxedOutputLabel.Size = new System.Drawing.Size(82, 13);
            this.muxedOutputLabel.TabIndex = 23;
            this.muxedOutputLabel.Text = "Name of output";
            // 
            // muxedOutput
            // 
            this.muxedOutput.Filename = "";
            this.muxedOutput.Location = new System.Drawing.Point(97, 74);
            this.muxedOutput.Name = "muxedOutput";
            this.muxedOutput.ReadOnly = false;
            this.muxedOutput.SaveMode = true;
            this.muxedOutput.Size = new System.Drawing.Size(352, 23);
            this.muxedOutput.TabIndex = 36;
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cancelButton.AutoSize = true;
            this.cancelButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(413, 228);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(49, 23);
            this.cancelButton.TabIndex = 19;
            this.cancelButton.Text = "Cancel";
            // 
            // addSubsNChapters
            // 
            this.addSubsNChapters.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.addSubsNChapters.Location = new System.Drawing.Point(88, 228);
            this.addSubsNChapters.Name = "addSubsNChapters";
            this.addSubsNChapters.Size = new System.Drawing.Size(256, 24);
            this.addSubsNChapters.TabIndex = 20;
            this.addSubsNChapters.Text = "Add additional content (audio, subs, chapters)";
            this.defaultToolTip.SetToolTip(this.addSubsNChapters, "Checking this option allows you to specify pre-encoded audio and subtitle files t" +
        "o be added to your output, as well as assign audio/subtitle languages and assign" +
        " a chapter file");
            // 
            // helpButton1
            // 
            this.helpButton1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.helpButton1.ArticleName = "Tools/AutoEncode";
            this.helpButton1.AutoSize = true;
            this.helpButton1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.helpButton1.Location = new System.Drawing.Point(10, 228);
            this.helpButton1.Name = "helpButton1";
            this.helpButton1.Size = new System.Drawing.Size(38, 23);
            this.helpButton1.TabIndex = 21;
            // 
            // AutoEncodeWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(471, 258);
            this.Controls.Add(this.helpButton1);
            this.Controls.Add(this.addSubsNChapters);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.OutputGroupBox);
            this.Controls.Add(this.AutomaticEncodingGroup);
            this.Controls.Add(this.queueButton);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AutoEncodeWindow";
            this.Text = "MeGUI - Automatic Encoding";
            this.AutomaticEncodingGroup.ResumeLayout(false);
            this.AutomaticEncodingGroup.PerformLayout();
            this.OutputGroupBox.ResumeLayout(false);
            this.OutputGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private System.Windows.Forms.GroupBox AutomaticEncodingGroup;
        private System.Windows.Forms.RadioButton FileSizeRadio;
        private System.Windows.Forms.Label AverageBitrateLabel;
        private System.Windows.Forms.GroupBox OutputGroupBox;
        private System.Windows.Forms.Button queueButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.TextBox projectedBitrateKBits;
        private System.Windows.Forms.CheckBox addSubsNChapters;
        private System.Windows.Forms.RadioButton averageBitrateRadio;
        private System.Windows.Forms.RadioButton noTargetRadio;
        private System.Windows.Forms.Label muxedOutputLabel;
        private System.Windows.Forms.Label containerLabel;
        private System.Windows.Forms.ComboBox container;
        private System.Windows.Forms.ToolTip defaultToolTip;
        private System.Windows.Forms.TextBox videoSize;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label DeviceLabel;
        private System.Windows.Forms.ComboBox device;
        private MeGUI.core.gui.HelpButton helpButton1;
        private MeGUI.core.gui.TargetSizeSCBox splitting;
        private MeGUI.core.gui.TargetSizeSCBox targetSize;
        private MuxProvider muxProvider;
        private VideoStream videoStream;
        protected FileBar muxedOutput;
    }
}
