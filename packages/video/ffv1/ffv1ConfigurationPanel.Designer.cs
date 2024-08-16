// ****************************************************************************
// 
// Copyright (C) 2005-2024 Doom9 & al
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

namespace MeGUI.packages.video.ffv1
{
    partial class ffv1ConfigurationPanel
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
            this.ffv1ThreadsLabel = new System.Windows.Forms.Label();
            this.ffv1NbThreads = new System.Windows.Forms.NumericUpDown();
            this.ch10BitsEncoder = new System.Windows.Forms.CheckBox();
            this.chErrorCorrection = new System.Windows.Forms.CheckBox();
            this.chContext = new System.Windows.Forms.CheckBox();
            this.lbGOPSize = new System.Windows.Forms.Label();
            this.nmGOPSize = new System.Windows.Forms.NumericUpDown();
            this.gbExtra = new System.Windows.Forms.GroupBox();
            this.ffv1CodecGeneralGroupbox = new System.Windows.Forms.GroupBox();
            this.ffv1EncodingMode = new System.Windows.Forms.ComboBox();
            this.ffv1SlicesGroupbox = new System.Windows.Forms.GroupBox();
            this.ffv1Slices = new System.Windows.Forms.ComboBox();
            this.gbCoder = new System.Windows.Forms.GroupBox();
            this.ffv1Coder = new System.Windows.Forms.ComboBox();
            this.tabControl1.SuspendLayout();
            this.mainTabPage.SuspendLayout();
            this.MiscTabPage.SuspendLayout();
            this.gbffv1CustomCmd.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ffv1NbThreads)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmGOPSize)).BeginInit();
            this.gbExtra.SuspendLayout();
            this.ffv1CodecGeneralGroupbox.SuspendLayout();
            this.ffv1SlicesGroupbox.SuspendLayout();
            this.gbCoder.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.MiscTabPage);
            this.tabControl1.Size = new System.Drawing.Size(370, 246);
            this.tabControl1.SizeMode = System.Windows.Forms.TabSizeMode.FillToRight;
            this.tabControl1.TabIndex = 0;
            this.tabControl1.Controls.SetChildIndex(this.MiscTabPage, 0);
            this.tabControl1.Controls.SetChildIndex(this.mainTabPage, 0);
            // 
            // commandline
            // 
            this.commandline.Location = new System.Drawing.Point(0, 248);
            this.commandline.Size = new System.Drawing.Size(367, 89);
            this.commandline.TabIndex = 1;
            this.commandline.Text = " ";
            // 
            // mainTabPage
            // 
            this.mainTabPage.Controls.Add(this.gbExtra);
            this.mainTabPage.Controls.Add(this.helpButton1);
            this.mainTabPage.Size = new System.Drawing.Size(362, 220);
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
            this.helpButton1.ArticleName = "Configuration/Video_Encoder_Configuration/FFV1";
            this.helpButton1.AutoSize = true;
            this.helpButton1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.helpButton1.Location = new System.Drawing.Point(299, 183);
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
            this.MiscTabPage.Size = new System.Drawing.Size(362, 220);
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
            // ffv1ThreadsLabel
            // 
            this.ffv1ThreadsLabel.AutoSize = true;
            this.ffv1ThreadsLabel.Location = new System.Drawing.Point(12, 31);
            this.ffv1ThreadsLabel.Name = "ffv1ThreadsLabel";
            this.ffv1ThreadsLabel.Size = new System.Drawing.Size(95, 13);
            this.ffv1ThreadsLabel.TabIndex = 17;
            this.ffv1ThreadsLabel.Text = "Threads (0 = Auto)";
            // 
            // ffv1NbThreads
            // 
            this.ffv1NbThreads.Location = new System.Drawing.Point(114, 29);
            this.ffv1NbThreads.Name = "ffv1NbThreads";
            this.ffv1NbThreads.Size = new System.Drawing.Size(43, 20);
            this.ffv1NbThreads.TabIndex = 18;
            this.ffv1NbThreads.ValueChanged += new System.EventHandler(this.updateEvent);
            // 
            // ch10BitsEncoder
            // 
            this.ch10BitsEncoder.AutoSize = true;
            this.ch10BitsEncoder.Location = new System.Drawing.Point(15, 140);
            this.ch10BitsEncoder.Name = "ch10BitsEncoder";
            this.ch10BitsEncoder.Size = new System.Drawing.Size(142, 17);
            this.ch10BitsEncoder.TabIndex = 41;
            this.ch10BitsEncoder.Text = "Enable 10-Bits Encoding";
            this.ch10BitsEncoder.UseVisualStyleBackColor = true;
            this.ch10BitsEncoder.CheckedChanged += new System.EventHandler(this.updateEvent);
            // 
            // chErrorCorrection
            // 
            this.chErrorCorrection.AutoSize = true;
            this.chErrorCorrection.Checked = true;
            this.chErrorCorrection.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chErrorCorrection.Location = new System.Drawing.Point(15, 94);
            this.chErrorCorrection.Name = "chErrorCorrection";
            this.chErrorCorrection.Size = new System.Drawing.Size(135, 17);
            this.chErrorCorrection.TabIndex = 43;
            this.chErrorCorrection.Text = "Enable Error Correction";
            this.chErrorCorrection.UseVisualStyleBackColor = true;
            this.chErrorCorrection.CheckedChanged += new System.EventHandler(this.updateEvent);
            // 
            // chContext
            // 
            this.chContext.AutoSize = true;
            this.chContext.Checked = true;
            this.chContext.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chContext.Location = new System.Drawing.Point(15, 117);
            this.chContext.Name = "chContext";
            this.chContext.Size = new System.Drawing.Size(98, 17);
            this.chContext.TabIndex = 44;
            this.chContext.Text = "Enable Context";
            this.chContext.UseVisualStyleBackColor = true;
            this.chContext.CheckedChanged += new System.EventHandler(this.updateEvent);
            // 
            // lbGOPSize
            // 
            this.lbGOPSize.AutoSize = true;
            this.lbGOPSize.Location = new System.Drawing.Point(54, 65);
            this.lbGOPSize.Name = "lbGOPSize";
            this.lbGOPSize.Size = new System.Drawing.Size(53, 13);
            this.lbGOPSize.TabIndex = 45;
            this.lbGOPSize.Text = "GOP Size";
            // 
            // nmGOPSize
            // 
            this.nmGOPSize.Location = new System.Drawing.Point(114, 63);
            this.nmGOPSize.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nmGOPSize.Name = "nmGOPSize";
            this.nmGOPSize.Size = new System.Drawing.Size(43, 20);
            this.nmGOPSize.TabIndex = 46;
            this.nmGOPSize.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nmGOPSize.ValueChanged += new System.EventHandler(this.updateEvent);
            // 
            // gbExtra
            // 
            this.gbExtra.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbExtra.Controls.Add(this.ffv1CodecGeneralGroupbox);
            this.gbExtra.Controls.Add(this.ch10BitsEncoder);
            this.gbExtra.Controls.Add(this.chContext);
            this.gbExtra.Controls.Add(this.lbGOPSize);
            this.gbExtra.Controls.Add(this.chErrorCorrection);
            this.gbExtra.Controls.Add(this.nmGOPSize);
            this.gbExtra.Controls.Add(this.ffv1SlicesGroupbox);
            this.gbExtra.Controls.Add(this.gbCoder);
            this.gbExtra.Controls.Add(this.ffv1ThreadsLabel);
            this.gbExtra.Controls.Add(this.ffv1NbThreads);
            this.gbExtra.Location = new System.Drawing.Point(6, 6);
            this.gbExtra.Name = "gbExtra";
            this.gbExtra.Size = new System.Drawing.Size(350, 171);
            this.gbExtra.TabIndex = 47;
            this.gbExtra.TabStop = false;
            this.gbExtra.Text = "FFV1 Settings";
            // 
            // ffv1CodecGeneralGroupbox
            // 
            this.ffv1CodecGeneralGroupbox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ffv1CodecGeneralGroupbox.Controls.Add(this.ffv1EncodingMode);
            this.ffv1CodecGeneralGroupbox.Location = new System.Drawing.Point(198, 9);
            this.ffv1CodecGeneralGroupbox.Name = "ffv1CodecGeneralGroupbox";
            this.ffv1CodecGeneralGroupbox.Size = new System.Drawing.Size(146, 48);
            this.ffv1CodecGeneralGroupbox.TabIndex = 45;
            this.ffv1CodecGeneralGroupbox.TabStop = false;
            this.ffv1CodecGeneralGroupbox.Text = " Encoding Mode ";
            // 
            // ffv1EncodingMode
            // 
            this.ffv1EncodingMode.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ffv1EncodingMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ffv1EncodingMode.Items.AddRange(new object[] {
            "None",
            "2pass - 1st pass",
            "2pass - 2nd pass",
            "Automated 2pass"});
            this.ffv1EncodingMode.Location = new System.Drawing.Point(14, 19);
            this.ffv1EncodingMode.MaxDropDownItems = 2;
            this.ffv1EncodingMode.Name = "ffv1EncodingMode";
            this.ffv1EncodingMode.Size = new System.Drawing.Size(123, 21);
            this.ffv1EncodingMode.TabIndex = 2;
            this.ffv1EncodingMode.SelectedIndexChanged += new System.EventHandler(this.updateEvent);
            // 
            // ffv1SlicesGroupbox
            // 
            this.ffv1SlicesGroupbox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ffv1SlicesGroupbox.Controls.Add(this.ffv1Slices);
            this.ffv1SlicesGroupbox.Location = new System.Drawing.Point(198, 117);
            this.ffv1SlicesGroupbox.Name = "ffv1SlicesGroupbox";
            this.ffv1SlicesGroupbox.Size = new System.Drawing.Size(146, 48);
            this.ffv1SlicesGroupbox.TabIndex = 44;
            this.ffv1SlicesGroupbox.TabStop = false;
            this.ffv1SlicesGroupbox.Text = "Slices";
            // 
            // ffv1Slices
            // 
            this.ffv1Slices.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ffv1Slices.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ffv1Slices.Items.AddRange(new object[] {
            "4",
            "6",
            "9",
            "12",
            "16",
            "24",
            "30"});
            this.ffv1Slices.Location = new System.Drawing.Point(14, 19);
            this.ffv1Slices.Name = "ffv1Slices";
            this.ffv1Slices.Size = new System.Drawing.Size(123, 21);
            this.ffv1Slices.TabIndex = 0;
            this.ffv1Slices.SelectedIndexChanged += new System.EventHandler(this.updateEvent);
            // 
            // gbCoder
            // 
            this.gbCoder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbCoder.Controls.Add(this.ffv1Coder);
            this.gbCoder.Location = new System.Drawing.Point(198, 63);
            this.gbCoder.Name = "gbCoder";
            this.gbCoder.Size = new System.Drawing.Size(146, 48);
            this.gbCoder.TabIndex = 43;
            this.gbCoder.TabStop = false;
            this.gbCoder.Text = "Coder";
            // 
            // ffv1Coder
            // 
            this.ffv1Coder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ffv1Coder.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ffv1Coder.FormattingEnabled = true;
            this.ffv1Coder.Items.AddRange(new object[] {
            "0",
            "1",
            "2"});
            this.ffv1Coder.Location = new System.Drawing.Point(14, 19);
            this.ffv1Coder.Name = "ffv1Coder";
            this.ffv1Coder.Size = new System.Drawing.Size(123, 21);
            this.ffv1Coder.TabIndex = 0;
            this.ffv1Coder.SelectedIndexChanged += new System.EventHandler(this.updateEvent);
            // 
            // ffv1ConfigurationPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Name = "ffv1ConfigurationPanel";
            this.Size = new System.Drawing.Size(370, 340);
            this.tabControl1.ResumeLayout(false);
            this.mainTabPage.ResumeLayout(false);
            this.mainTabPage.PerformLayout();
            this.MiscTabPage.ResumeLayout(false);
            this.gbffv1CustomCmd.ResumeLayout(false);
            this.gbffv1CustomCmd.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ffv1NbThreads)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmGOPSize)).EndInit();
            this.gbExtra.ResumeLayout(false);
            this.gbExtra.PerformLayout();
            this.ffv1CodecGeneralGroupbox.ResumeLayout(false);
            this.ffv1SlicesGroupbox.ResumeLayout(false);
            this.gbCoder.ResumeLayout(false);
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
        private System.Windows.Forms.Label ffv1ThreadsLabel;
        private System.Windows.Forms.NumericUpDown ffv1NbThreads;
        private System.Windows.Forms.CheckBox ch10BitsEncoder;
        private System.Windows.Forms.CheckBox chErrorCorrection;
        private System.Windows.Forms.CheckBox chContext;
        private System.Windows.Forms.Label lbGOPSize;
        private System.Windows.Forms.NumericUpDown nmGOPSize;
        private System.Windows.Forms.GroupBox gbExtra;
        private System.Windows.Forms.GroupBox ffv1SlicesGroupbox;
        private System.Windows.Forms.ComboBox ffv1Slices;
        private System.Windows.Forms.GroupBox gbCoder;
        private System.Windows.Forms.ComboBox ffv1Coder;
        private System.Windows.Forms.GroupBox ffv1CodecGeneralGroupbox;
        private System.Windows.Forms.ComboBox ffv1EncodingMode;
    }
}
