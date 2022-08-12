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


namespace MeGUI.core.gui
{
    partial class AudioEncodingTab
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
            this.label1 = new System.Windows.Forms.Label();
            this.audioContainer = new System.Windows.Forms.ComboBox();
            this.audioContainerLabel = new System.Windows.Forms.Label();
            this.queueAudioButton = new System.Windows.Forms.Button();
            this.audioInputLabel = new System.Windows.Forms.Label();
            this.audioOutputLabel = new System.Windows.Forms.Label();
            this.deleteAudioButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.delay = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.audioProfile = new MeGUI.core.gui.ConfigableProfilesControl();
            this.audioOutput = new MeGUI.FileBar();
            this.cuts = new MeGUI.FileBar();
            this.audioInput = new MeGUI.FileBar();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.delay)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Location = new System.Drawing.Point(3, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(124, 28);
            this.label1.TabIndex = 17;
            this.label1.Text = "Cuts";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // audioContainer
            // 
            this.audioContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.audioContainer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.audioContainer.FormattingEnabled = true;
            this.audioContainer.Location = new System.Drawing.Point(134, 115);
            this.audioContainer.Margin = new System.Windows.Forms.Padding(4, 3, 3, 3);
            this.audioContainer.Name = "audioContainer";
            this.audioContainer.Size = new System.Drawing.Size(121, 21);
            this.audioContainer.TabIndex = 25;
            this.audioContainer.SelectedIndexChanged += new System.EventHandler(this.audioContainer_SelectedIndexChanged);
            // 
            // audioContainerLabel
            // 
            this.audioContainerLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.audioContainerLabel.Location = new System.Drawing.Point(3, 112);
            this.audioContainerLabel.Name = "audioContainerLabel";
            this.audioContainerLabel.Size = new System.Drawing.Size(124, 28);
            this.audioContainerLabel.TabIndex = 24;
            this.audioContainerLabel.Text = "Extension ";
            this.audioContainerLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // queueAudioButton
            // 
            this.queueAudioButton.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.queueAudioButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.queueAudioButton.Location = new System.Drawing.Point(91, 3);
            this.queueAudioButton.Name = "queueAudioButton";
            this.queueAudioButton.Size = new System.Drawing.Size(60, 23);
            this.queueAudioButton.TabIndex = 27;
            this.queueAudioButton.Text = "Queue";
            this.queueAudioButton.Click += new System.EventHandler(this.queueAudioButton_Click);
            // 
            // audioInputLabel
            // 
            this.audioInputLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.audioInputLabel.Location = new System.Drawing.Point(3, 0);
            this.audioInputLabel.Name = "audioInputLabel";
            this.audioInputLabel.Size = new System.Drawing.Size(124, 28);
            this.audioInputLabel.TabIndex = 15;
            this.audioInputLabel.Text = "Audio Input";
            this.audioInputLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // audioOutputLabel
            // 
            this.audioOutputLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.audioOutputLabel.Location = new System.Drawing.Point(3, 56);
            this.audioOutputLabel.Name = "audioOutputLabel";
            this.audioOutputLabel.Size = new System.Drawing.Size(124, 28);
            this.audioOutputLabel.TabIndex = 19;
            this.audioOutputLabel.Text = "Audio Output";
            this.audioOutputLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // deleteAudioButton
            // 
            this.deleteAudioButton.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.deleteAudioButton.Location = new System.Drawing.Point(37, 3);
            this.deleteAudioButton.Name = "deleteAudioButton";
            this.deleteAudioButton.Size = new System.Drawing.Size(48, 23);
            this.deleteAudioButton.TabIndex = 26;
            this.deleteAudioButton.Text = "X";
            this.deleteAudioButton.Click += new System.EventHandler(this.deleteAudioButton_Click);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.Location = new System.Drawing.Point(268, 112);
            this.label2.Margin = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(34, 28);
            this.label2.TabIndex = 28;
            this.label2.Text = "Delay";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // delay
            // 
            this.delay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.delay.Location = new System.Drawing.Point(305, 116);
            this.delay.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.delay.Minimum = new decimal(new int[] {
            -2147483648,
            0,
            0,
            -2147483648});
            this.delay.Name = "delay";
            this.delay.Size = new System.Drawing.Size(122, 20);
            this.delay.TabIndex = 29;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.Location = new System.Drawing.Point(433, 112);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(20, 28);
            this.label3.TabIndex = 30;
            this.label3.Text = "ms";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.Location = new System.Drawing.Point(3, 84);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(124, 28);
            this.label4.TabIndex = 19;
            this.label4.Text = "Encoder settings";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 5;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 130F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.audioInputLabel, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.audioContainer, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.label3, 4, 4);
            this.tableLayoutPanel1.Controls.Add(this.audioProfile, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.delay, 3, 4);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label2, 2, 4);
            this.tableLayoutPanel1.Controls.Add(this.audioOutputLabel, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.audioContainerLabel, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.audioOutput, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.cuts, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.audioInput, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 3, 5);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 6;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(456, 173);
            this.tableLayoutPanel1.TabIndex = 32;
            // 
            // audioProfile
            // 
            this.audioProfile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.audioProfile, 4);
            this.audioProfile.Location = new System.Drawing.Point(133, 87);
            this.audioProfile.Name = "audioProfile";
            this.audioProfile.ProfileSet = "Audio";
            this.audioProfile.Size = new System.Drawing.Size(320, 22);
            this.audioProfile.TabIndex = 31;
            this.audioProfile.SelectedProfileChanged += new System.EventHandler(this.audioProfile_SelectedProfileChanged);
            // 
            // audioOutput
            // 
            this.audioOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.audioOutput, 4);
            this.audioOutput.Filename = "";
            this.audioOutput.Location = new System.Drawing.Point(133, 59);
            this.audioOutput.Name = "audioOutput";
            this.audioOutput.ReadOnly = false;
            this.audioOutput.SaveMode = true;
            this.audioOutput.Size = new System.Drawing.Size(320, 22);
            this.audioOutput.TabIndex = 20;
            this.audioOutput.Title = "Enter name of output";
            // 
            // cuts
            // 
            this.cuts.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.cuts, 4);
            this.cuts.Filename = "";
            this.cuts.Filter = "MeGUI cutlist files (*.clt)|*.clt";
            this.cuts.Location = new System.Drawing.Point(133, 31);
            this.cuts.Name = "cuts";
            this.cuts.Size = new System.Drawing.Size(320, 22);
            this.cuts.TabIndex = 18;
            this.cuts.Title = "Select a file with cuts";
            // 
            // audioInput
            // 
            this.audioInput.AllowDrop = true;
            this.audioInput.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.audioInput, 4);
            this.audioInput.Filename = "";
            this.audioInput.Filter = "All files (*.*)|*.*";
            this.audioInput.Location = new System.Drawing.Point(133, 3);
            this.audioInput.Name = "audioInput";
            this.audioInput.Size = new System.Drawing.Size(320, 22);
            this.audioInput.TabIndex = 16;
            this.audioInput.Title = "Select your audio input";
            this.audioInput.FileSelected += new MeGUI.FileBarEventHandler(this.audioInput_FileSelected);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.SetColumnSpan(this.flowLayoutPanel1, 2);
            this.flowLayoutPanel1.Controls.Add(this.queueAudioButton);
            this.flowLayoutPanel1.Controls.Add(this.deleteAudioButton);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(302, 140);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(154, 33);
            this.flowLayoutPanel1.TabIndex = 32;
            // 
            // AudioEncodingTab
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "AudioEncodingTab";
            this.Size = new System.Drawing.Size(456, 173);
            ((System.ComponentModel.ISupportInitialize)(this.delay)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private FileBar cuts;
        private System.Windows.Forms.Label label1;
        private FileBar audioOutput;
        private FileBar audioInput;
        private System.Windows.Forms.ComboBox audioContainer;
        private System.Windows.Forms.Label audioContainerLabel;
        private System.Windows.Forms.Button queueAudioButton;
        private System.Windows.Forms.Label audioInputLabel;
        private System.Windows.Forms.Label audioOutputLabel;
        private System.Windows.Forms.Button deleteAudioButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown delay;
        private System.Windows.Forms.Label label3;
        private ConfigableProfilesControl audioProfile;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
    }
}
