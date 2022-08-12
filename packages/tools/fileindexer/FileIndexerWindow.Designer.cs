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
    partial class FileIndexerWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FileIndexerWindow));
            this.gbInput = new System.Windows.Forms.GroupBox();
            this.input = new MeGUI.FileBar();
            this.inputLabel = new System.Windows.Forms.Label();
            this.queueButton = new System.Windows.Forms.Button();
            this.loadOnComplete = new System.Windows.Forms.CheckBox();
            this.gbAudio = new System.Windows.Forms.GroupBox();
            this.demuxAll = new System.Windows.Forms.RadioButton();
            this.AudioTracks = new System.Windows.Forms.CheckedListBox();
            this.demuxNoAudiotracks = new System.Windows.Forms.RadioButton();
            this.demuxTracks = new System.Windows.Forms.RadioButton();
            this.gbOutput = new System.Windows.Forms.GroupBox();
            this.output = new MeGUI.FileBar();
            this.demuxVideo = new System.Windows.Forms.CheckBox();
            this.outputLabel = new System.Windows.Forms.Label();
            this.closeOnQueue = new System.Windows.Forms.CheckBox();
            this.gbIndexer = new System.Windows.Forms.GroupBox();
            this.btnLSMASH = new System.Windows.Forms.RadioButton();
            this.btnDGM = new System.Windows.Forms.RadioButton();
            this.btnFFMS = new System.Windows.Forms.RadioButton();
            this.btnD2V = new System.Windows.Forms.RadioButton();
            this.btnDGI = new System.Windows.Forms.RadioButton();
            this.gbFileInformation = new System.Windows.Forms.GroupBox();
            this.txtContainerInformation = new System.Windows.Forms.TextBox();
            this.txtScanTypeInformation = new System.Windows.Forms.TextBox();
            this.txtCodecInformation = new System.Windows.Forms.TextBox();
            this.lblScanType = new System.Windows.Forms.Label();
            this.lblCodec = new System.Windows.Forms.Label();
            this.lblContainer = new System.Windows.Forms.Label();
            this.helpButton1 = new MeGUI.core.gui.HelpButton();
            this.gbInput.SuspendLayout();
            this.gbAudio.SuspendLayout();
            this.gbOutput.SuspendLayout();
            this.gbIndexer.SuspendLayout();
            this.gbFileInformation.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbInput
            // 
            this.gbInput.Controls.Add(this.input);
            this.gbInput.Controls.Add(this.inputLabel);
            this.gbInput.Location = new System.Drawing.Point(12, 6);
            this.gbInput.Name = "gbInput";
            this.gbInput.Size = new System.Drawing.Size(449, 50);
            this.gbInput.TabIndex = 0;
            this.gbInput.TabStop = false;
            this.gbInput.Text = " Input ";
            // 
            // input
            // 
            this.input.AllowDrop = true;
            this.input.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.input.Filename = "";
            this.input.Filter = "";
            this.input.Location = new System.Drawing.Point(81, 17);
            this.input.Name = "input";
            this.input.Size = new System.Drawing.Size(354, 23);
            this.input.TabIndex = 4;
            this.input.FileSelected += new MeGUI.FileBarEventHandler(this.input_FileSelected);
            // 
            // inputLabel
            // 
            this.inputLabel.Location = new System.Drawing.Point(9, 22);
            this.inputLabel.Name = "inputLabel";
            this.inputLabel.Size = new System.Drawing.Size(100, 13);
            this.inputLabel.TabIndex = 0;
            this.inputLabel.Text = "Input File";
            // 
            // queueButton
            // 
            this.queueButton.Location = new System.Drawing.Point(385, 394);
            this.queueButton.Name = "queueButton";
            this.queueButton.Size = new System.Drawing.Size(74, 23);
            this.queueButton.TabIndex = 10;
            this.queueButton.Text = "Queue";
            this.queueButton.Click += new System.EventHandler(this.queueButton_Click);
            // 
            // loadOnComplete
            // 
            this.loadOnComplete.Checked = true;
            this.loadOnComplete.CheckState = System.Windows.Forms.CheckState.Checked;
            this.loadOnComplete.Location = new System.Drawing.Point(91, 395);
            this.loadOnComplete.Name = "loadOnComplete";
            this.loadOnComplete.Size = new System.Drawing.Size(144, 24);
            this.loadOnComplete.TabIndex = 11;
            this.loadOnComplete.Text = "On completion load files";
            // 
            // gbAudio
            // 
            this.gbAudio.Controls.Add(this.demuxAll);
            this.gbAudio.Controls.Add(this.AudioTracks);
            this.gbAudio.Controls.Add(this.demuxNoAudiotracks);
            this.gbAudio.Controls.Add(this.demuxTracks);
            this.gbAudio.Enabled = false;
            this.gbAudio.Location = new System.Drawing.Point(12, 187);
            this.gbAudio.Name = "gbAudio";
            this.gbAudio.Size = new System.Drawing.Size(449, 125);
            this.gbAudio.TabIndex = 8;
            this.gbAudio.TabStop = false;
            this.gbAudio.Text = " Audio Demux ";
            // 
            // demuxAll
            // 
            this.demuxAll.AutoSize = true;
            this.demuxAll.Checked = true;
            this.demuxAll.Location = new System.Drawing.Point(345, 20);
            this.demuxAll.Name = "demuxAll";
            this.demuxAll.Size = new System.Drawing.Size(100, 17);
            this.demuxAll.TabIndex = 15;
            this.demuxAll.TabStop = true;
            this.demuxAll.Text = "All Audio Tracks";
            this.demuxAll.UseVisualStyleBackColor = true;
            this.demuxAll.CheckedChanged += new System.EventHandler(this.rbtracks_CheckedChanged);
            // 
            // AudioTracks
            // 
            this.AudioTracks.CheckOnClick = true;
            this.AudioTracks.Enabled = false;
            this.AudioTracks.FormattingEnabled = true;
            this.AudioTracks.Location = new System.Drawing.Point(12, 43);
            this.AudioTracks.Name = "AudioTracks";
            this.AudioTracks.Size = new System.Drawing.Size(419, 68);
            this.AudioTracks.TabIndex = 14;
            // 
            // demuxNoAudiotracks
            // 
            this.demuxNoAudiotracks.Location = new System.Drawing.Point(12, 16);
            this.demuxNoAudiotracks.Name = "demuxNoAudiotracks";
            this.demuxNoAudiotracks.Size = new System.Drawing.Size(127, 24);
            this.demuxNoAudiotracks.TabIndex = 13;
            this.demuxNoAudiotracks.Text = "No Audio";
            this.demuxNoAudiotracks.CheckedChanged += new System.EventHandler(this.rbtracks_CheckedChanged);
            // 
            // demuxTracks
            // 
            this.demuxTracks.Enabled = false;
            this.demuxTracks.Location = new System.Drawing.Point(171, 16);
            this.demuxTracks.Name = "demuxTracks";
            this.demuxTracks.Size = new System.Drawing.Size(120, 24);
            this.demuxTracks.TabIndex = 7;
            this.demuxTracks.Text = "Select Audio Tracks";
            this.demuxTracks.CheckedChanged += new System.EventHandler(this.rbtracks_CheckedChanged);
            // 
            // gbOutput
            // 
            this.gbOutput.Controls.Add(this.output);
            this.gbOutput.Controls.Add(this.demuxVideo);
            this.gbOutput.Controls.Add(this.outputLabel);
            this.gbOutput.Enabled = false;
            this.gbOutput.Location = new System.Drawing.Point(12, 318);
            this.gbOutput.Name = "gbOutput";
            this.gbOutput.Size = new System.Drawing.Size(447, 69);
            this.gbOutput.TabIndex = 12;
            this.gbOutput.TabStop = false;
            this.gbOutput.Text = " Output ";
            // 
            // output
            // 
            this.output.AllowDrop = true;
            this.output.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.output.Filename = "";
            this.output.Filter = "";
            this.output.Location = new System.Drawing.Point(81, 15);
            this.output.Name = "output";
            this.output.SaveMode = true;
            this.output.Size = new System.Drawing.Size(350, 23);
            this.output.TabIndex = 7;
            this.output.FileSelected += new MeGUI.FileBarEventHandler(this.output_FileSelected);
            // 
            // demuxVideo
            // 
            this.demuxVideo.AutoSize = true;
            this.demuxVideo.Location = new System.Drawing.Point(81, 44);
            this.demuxVideo.Name = "demuxVideo";
            this.demuxVideo.Size = new System.Drawing.Size(125, 17);
            this.demuxVideo.TabIndex = 6;
            this.demuxVideo.Text = "Demux Video Stream";
            this.demuxVideo.UseVisualStyleBackColor = true;
            // 
            // outputLabel
            // 
            this.outputLabel.Location = new System.Drawing.Point(11, 21);
            this.outputLabel.Name = "outputLabel";
            this.outputLabel.Size = new System.Drawing.Size(100, 13);
            this.outputLabel.TabIndex = 3;
            this.outputLabel.Text = "Output File";
            // 
            // closeOnQueue
            // 
            this.closeOnQueue.Checked = true;
            this.closeOnQueue.CheckState = System.Windows.Forms.CheckState.Checked;
            this.closeOnQueue.Location = new System.Drawing.Point(310, 394);
            this.closeOnQueue.Name = "closeOnQueue";
            this.closeOnQueue.Size = new System.Drawing.Size(72, 24);
            this.closeOnQueue.TabIndex = 13;
            this.closeOnQueue.Text = "and close";
            // 
            // gbIndexer
            // 
            this.gbIndexer.Controls.Add(this.btnLSMASH);
            this.gbIndexer.Controls.Add(this.btnDGM);
            this.gbIndexer.Controls.Add(this.btnFFMS);
            this.gbIndexer.Controls.Add(this.btnD2V);
            this.gbIndexer.Controls.Add(this.btnDGI);
            this.gbIndexer.Enabled = false;
            this.gbIndexer.Location = new System.Drawing.Point(12, 135);
            this.gbIndexer.Name = "gbIndexer";
            this.gbIndexer.Size = new System.Drawing.Size(451, 46);
            this.gbIndexer.TabIndex = 15;
            this.gbIndexer.TabStop = false;
            this.gbIndexer.Text = " File Indexer ";
            // 
            // btnLSMASH
            // 
            this.btnLSMASH.AutoSize = true;
            this.btnLSMASH.Location = new System.Drawing.Point(345, 20);
            this.btnLSMASH.Name = "btnLSMASH";
            this.btnLSMASH.Size = new System.Drawing.Size(101, 17);
            this.btnLSMASH.TabIndex = 4;
            this.btnLSMASH.TabStop = true;
            this.btnLSMASH.Text = "L-SMASH Works";
            this.btnLSMASH.UseVisualStyleBackColor = true;
            this.btnLSMASH.Click += new System.EventHandler(this.btnLSMASH_Click);
            // 
            // btnDGM
            // 
            this.btnDGM.AutoSize = true;
            this.btnDGM.Location = new System.Drawing.Point(85, 20);
            this.btnDGM.Name = "btnDGM";
            this.btnDGM.Size = new System.Drawing.Size(79, 17);
            this.btnDGM.TabIndex = 3;
            this.btnDGM.TabStop = true;
            this.btnDGM.Text = "DGIndexIM";
            this.btnDGM.UseVisualStyleBackColor = true;
            this.btnDGM.Click += new System.EventHandler(this.btnDGM_Click);
            // 
            // btnFFMS
            // 
            this.btnFFMS.AutoSize = true;
            this.btnFFMS.Location = new System.Drawing.Point(262, 20);
            this.btnFFMS.Name = "btnFFMS";
            this.btnFFMS.Size = new System.Drawing.Size(79, 17);
            this.btnFFMS.TabIndex = 2;
            this.btnFFMS.TabStop = true;
            this.btnFFMS.Text = "FFMSIndex";
            this.btnFFMS.UseVisualStyleBackColor = true;
            this.btnFFMS.Click += new System.EventHandler(this.btnFFMS_Click);
            // 
            // btnD2V
            // 
            this.btnD2V.AutoSize = true;
            this.btnD2V.Location = new System.Drawing.Point(12, 20);
            this.btnD2V.Name = "btnD2V";
            this.btnD2V.Size = new System.Drawing.Size(67, 17);
            this.btnD2V.TabIndex = 1;
            this.btnD2V.TabStop = true;
            this.btnD2V.Text = "DGIndex";
            this.btnD2V.UseVisualStyleBackColor = true;
            this.btnD2V.Click += new System.EventHandler(this.btnD2V_Click);
            // 
            // btnDGI
            // 
            this.btnDGI.AutoSize = true;
            this.btnDGI.Location = new System.Drawing.Point(171, 20);
            this.btnDGI.Name = "btnDGI";
            this.btnDGI.Size = new System.Drawing.Size(80, 17);
            this.btnDGI.TabIndex = 0;
            this.btnDGI.TabStop = true;
            this.btnDGI.Text = "DGIndexNV";
            this.btnDGI.UseVisualStyleBackColor = true;
            this.btnDGI.Click += new System.EventHandler(this.btnDGI_Click);
            // 
            // gbFileInformation
            // 
            this.gbFileInformation.Controls.Add(this.txtContainerInformation);
            this.gbFileInformation.Controls.Add(this.txtScanTypeInformation);
            this.gbFileInformation.Controls.Add(this.txtCodecInformation);
            this.gbFileInformation.Controls.Add(this.lblScanType);
            this.gbFileInformation.Controls.Add(this.lblCodec);
            this.gbFileInformation.Controls.Add(this.lblContainer);
            this.gbFileInformation.Location = new System.Drawing.Point(12, 62);
            this.gbFileInformation.Name = "gbFileInformation";
            this.gbFileInformation.Size = new System.Drawing.Size(449, 67);
            this.gbFileInformation.TabIndex = 16;
            this.gbFileInformation.TabStop = false;
            this.gbFileInformation.Text = " File Information ";
            // 
            // txtContainerInformation
            // 
            this.txtContainerInformation.Enabled = false;
            this.txtContainerInformation.Location = new System.Drawing.Point(298, 34);
            this.txtContainerInformation.Name = "txtContainerInformation";
            this.txtContainerInformation.Size = new System.Drawing.Size(133, 21);
            this.txtContainerInformation.TabIndex = 5;
            // 
            // txtScanTypeInformation
            // 
            this.txtScanTypeInformation.Enabled = false;
            this.txtScanTypeInformation.Location = new System.Drawing.Point(160, 34);
            this.txtScanTypeInformation.Name = "txtScanTypeInformation";
            this.txtScanTypeInformation.Size = new System.Drawing.Size(133, 21);
            this.txtScanTypeInformation.TabIndex = 4;
            // 
            // txtCodecInformation
            // 
            this.txtCodecInformation.Enabled = false;
            this.txtCodecInformation.Location = new System.Drawing.Point(12, 34);
            this.txtCodecInformation.Name = "txtCodecInformation";
            this.txtCodecInformation.Size = new System.Drawing.Size(141, 21);
            this.txtCodecInformation.TabIndex = 3;
            // 
            // lblScanType
            // 
            this.lblScanType.AutoSize = true;
            this.lblScanType.Location = new System.Drawing.Point(157, 18);
            this.lblScanType.Name = "lblScanType";
            this.lblScanType.Size = new System.Drawing.Size(57, 13);
            this.lblScanType.TabIndex = 2;
            this.lblScanType.Text = "Scan Type";
            // 
            // lblCodec
            // 
            this.lblCodec.AutoSize = true;
            this.lblCodec.Location = new System.Drawing.Point(11, 18);
            this.lblCodec.Margin = new System.Windows.Forms.Padding(0);
            this.lblCodec.Name = "lblCodec";
            this.lblCodec.Size = new System.Drawing.Size(37, 13);
            this.lblCodec.TabIndex = 1;
            this.lblCodec.Text = "Codec";
            // 
            // lblContainer
            // 
            this.lblContainer.AutoSize = true;
            this.lblContainer.Location = new System.Drawing.Point(295, 18);
            this.lblContainer.Name = "lblContainer";
            this.lblContainer.Size = new System.Drawing.Size(54, 13);
            this.lblContainer.TabIndex = 0;
            this.lblContainer.Text = "Container";
            // 
            // helpButton1
            // 
            this.helpButton1.ArticleName = "File Indexer window";
            this.helpButton1.AutoSize = true;
            this.helpButton1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.helpButton1.Location = new System.Drawing.Point(13, 394);
            this.helpButton1.Name = "helpButton1";
            this.helpButton1.Size = new System.Drawing.Size(38, 23);
            this.helpButton1.TabIndex = 14;
            // 
            // FileIndexerWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(471, 425);
            this.Controls.Add(this.gbFileInformation);
            this.Controls.Add(this.gbIndexer);
            this.Controls.Add(this.helpButton1);
            this.Controls.Add(this.closeOnQueue);
            this.Controls.Add(this.gbInput);
            this.Controls.Add(this.gbOutput);
            this.Controls.Add(this.loadOnComplete);
            this.Controls.Add(this.queueButton);
            this.Controls.Add(this.gbAudio);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FileIndexerWindow";
            this.Text = "MeGUI - File Indexer";
            this.gbInput.ResumeLayout(false);
            this.gbAudio.ResumeLayout(false);
            this.gbAudio.PerformLayout();
            this.gbOutput.ResumeLayout(false);
            this.gbOutput.PerformLayout();
            this.gbIndexer.ResumeLayout(false);
            this.gbIndexer.PerformLayout();
            this.gbFileInformation.ResumeLayout(false);
            this.gbFileInformation.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

        private System.Windows.Forms.GroupBox gbAudio;
        private System.Windows.Forms.GroupBox gbOutput;
        private System.Windows.Forms.Label inputLabel;
        private System.Windows.Forms.Label outputLabel;
        private System.Windows.Forms.GroupBox gbInput;
        private System.Windows.Forms.RadioButton demuxTracks;
        private System.Windows.Forms.RadioButton demuxNoAudiotracks;
        private System.Windows.Forms.Button queueButton;
        private System.Windows.Forms.CheckBox loadOnComplete;
        private System.Windows.Forms.CheckBox closeOnQueue;
        private MeGUI.core.gui.HelpButton helpButton1;
        private System.Windows.Forms.CheckedListBox AudioTracks;
        private System.Windows.Forms.RadioButton demuxAll;
        private FileBar input;
        private System.Windows.Forms.CheckBox demuxVideo;
        private System.Windows.Forms.GroupBox gbIndexer;
        private System.Windows.Forms.RadioButton btnDGI;
        private System.Windows.Forms.RadioButton btnD2V;
        private System.Windows.Forms.RadioButton btnFFMS;
        private System.Windows.Forms.RadioButton btnDGM;
        private System.Windows.Forms.GroupBox gbFileInformation;
        private System.Windows.Forms.Label lblContainer;
        private System.Windows.Forms.Label lblScanType;
        private System.Windows.Forms.Label lblCodec;
        private System.Windows.Forms.TextBox txtCodecInformation;
        private System.Windows.Forms.TextBox txtContainerInformation;
        private System.Windows.Forms.TextBox txtScanTypeInformation;
        private System.Windows.Forms.RadioButton btnLSMASH;
        private FileBar output;
    }
}