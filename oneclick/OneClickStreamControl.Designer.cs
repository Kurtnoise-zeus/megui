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
    partial class OneClickStreamControl
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
            this.subName = new System.Windows.Forms.TextBox();
            this.SubNamelabel = new System.Windows.Forms.Label();
            this.subtitleLanguage = new System.Windows.Forms.ComboBox();
            this.subtitleLanguageLabel = new System.Windows.Forms.Label();
            this.subtitleInputLabel = new System.Windows.Forms.Label();
            this.delayLabel = new System.Windows.Forms.Label();
            this.delay = new System.Windows.Forms.NumericUpDown();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label2 = new System.Windows.Forms.Label();
            this.cbEncodingMode = new System.Windows.Forms.ComboBox();
            this.chkDefaultStream = new System.Windows.Forms.CheckBox();
            this.chkForceStream = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.encoderProfile = new MeGUI.core.gui.ConfigableProfilesControl();
            this.input = new MeGUI.core.gui.FileSCBox();
            ((System.ComponentModel.ISupportInitialize)(this.delay)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // subName
            // 
            this.subName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.subName, 3);
            this.subName.Location = new System.Drawing.Point(232, 35);
            this.subName.MaxLength = 100;
            this.subName.Name = "subName";
            this.subName.Size = new System.Drawing.Size(199, 20);
            this.subName.TabIndex = 40;
            // 
            // SubNamelabel
            // 
            this.SubNamelabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.SubNamelabel.AutoSize = true;
            this.SubNamelabel.Location = new System.Drawing.Point(191, 38);
            this.SubNamelabel.Name = "SubNamelabel";
            this.SubNamelabel.Size = new System.Drawing.Size(35, 13);
            this.SubNamelabel.TabIndex = 39;
            this.SubNamelabel.Text = "Name";
            // 
            // subtitleLanguage
            // 
            this.subtitleLanguage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.subtitleLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.subtitleLanguage.Location = new System.Drawing.Point(64, 34);
            this.subtitleLanguage.Name = "subtitleLanguage";
            this.subtitleLanguage.Size = new System.Drawing.Size(121, 21);
            this.subtitleLanguage.Sorted = true;
            this.subtitleLanguage.TabIndex = 37;
            // 
            // subtitleLanguageLabel
            // 
            this.subtitleLanguageLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.subtitleLanguageLabel.AutoSize = true;
            this.subtitleLanguageLabel.Location = new System.Drawing.Point(3, 38);
            this.subtitleLanguageLabel.Name = "subtitleLanguageLabel";
            this.subtitleLanguageLabel.Size = new System.Drawing.Size(55, 13);
            this.subtitleLanguageLabel.TabIndex = 36;
            this.subtitleLanguageLabel.Text = "Language";
            // 
            // subtitleInputLabel
            // 
            this.subtitleInputLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.subtitleInputLabel.AutoSize = true;
            this.subtitleInputLabel.Location = new System.Drawing.Point(3, 8);
            this.subtitleInputLabel.Name = "subtitleInputLabel";
            this.subtitleInputLabel.Size = new System.Drawing.Size(55, 13);
            this.subtitleInputLabel.TabIndex = 33;
            this.subtitleInputLabel.Text = "Input";
            // 
            // delayLabel
            // 
            this.delayLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.delayLabel.AutoSize = true;
            this.delayLabel.Location = new System.Drawing.Point(3, 68);
            this.delayLabel.Name = "delayLabel";
            this.delayLabel.Size = new System.Drawing.Size(55, 13);
            this.delayLabel.TabIndex = 43;
            this.delayLabel.Text = "Delay";
            // 
            // delay
            // 
            this.delay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.delay.Location = new System.Drawing.Point(64, 65);
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
            this.delay.Size = new System.Drawing.Size(121, 20);
            this.delay.TabIndex = 42;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.ColumnCount = 6;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.cbEncodingMode, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.encoderProfile, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.delayLabel, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.subName, 3, 1);
            this.tableLayoutPanel1.Controls.Add(this.delay, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.SubNamelabel, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.subtitleLanguage, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.subtitleLanguageLabel, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.chkDefaultStream, 3, 2);
            this.tableLayoutPanel1.Controls.Add(this.chkForceStream, 4, 2);
            this.tableLayoutPanel1.Controls.Add(this.input, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.subtitleInputLabel, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 3);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(434, 150);
            this.tableLayoutPanel1.TabIndex = 44;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 128);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 13);
            this.label2.TabIndex = 51;
            this.label2.Text = "Encode";
            // 
            // cbEncodingMode
            // 
            this.cbEncodingMode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.cbEncodingMode, 3);
            this.cbEncodingMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbEncodingMode.FormattingEnabled = true;
            this.cbEncodingMode.Location = new System.Drawing.Point(64, 124);
            this.cbEncodingMode.Name = "cbEncodingMode";
            this.cbEncodingMode.Size = new System.Drawing.Size(253, 21);
            this.cbEncodingMode.TabIndex = 49;
            this.cbEncodingMode.SelectedIndexChanged += new System.EventHandler(this.cbEncodingMode_SelectedIndexChanged);
            // 
            // chkDefaultStream
            // 
            this.chkDefaultStream.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.chkDefaultStream.AutoSize = true;
            this.chkDefaultStream.Location = new System.Drawing.Point(232, 66);
            this.chkDefaultStream.Name = "chkDefaultStream";
            this.chkDefaultStream.Size = new System.Drawing.Size(85, 17);
            this.chkDefaultStream.TabIndex = 44;
            this.chkDefaultStream.Text = "default track";
            this.chkDefaultStream.UseVisualStyleBackColor = true;
            this.chkDefaultStream.Visible = false;
            this.chkDefaultStream.CheckedChanged += new System.EventHandler(this.chkDefaultStream_CheckedChanged);
            // 
            // chkForceStream
            // 
            this.chkForceStream.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.chkForceStream.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.chkForceStream, 2);
            this.chkForceStream.Location = new System.Drawing.Point(323, 66);
            this.chkForceStream.Name = "chkForceStream";
            this.chkForceStream.Size = new System.Drawing.Size(108, 17);
            this.chkForceStream.TabIndex = 45;
            this.chkForceStream.Text = "forced track";
            this.chkForceStream.UseVisualStyleBackColor = true;
            this.chkForceStream.Visible = false;
            this.chkForceStream.CheckedChanged += new System.EventHandler(this.chkForceStream_CheckedChanged);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 98);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 50;
            this.label1.Text = "Encoder";
            // 
            // encoderProfile
            // 
            this.encoderProfile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.encoderProfile, 5);
            this.encoderProfile.Location = new System.Drawing.Point(64, 94);
            this.encoderProfile.Name = "encoderProfile";
            this.encoderProfile.ProfileSet = "Audio";
            this.encoderProfile.Size = new System.Drawing.Size(367, 22);
            this.encoderProfile.TabIndex = 48;
            this.encoderProfile.UpdateSelectedProfile = false;
            // 
            // input
            // 
            this.input.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.input, 5);
            this.input.Filter = "All files (*.*)|*.*";
            this.input.Location = new System.Drawing.Point(64, 3);
            this.input.MaximumSize = new System.Drawing.Size(1000, 29);
            this.input.MinimumSize = new System.Drawing.Size(64, 29);
            this.input.Name = "input";
            this.input.SelectedIndex = -1;
            this.input.Size = new System.Drawing.Size(367, 29);
            this.input.TabIndex = 47;
            this.input.Type = MeGUI.core.gui.FileSCBox.FileSCBoxType.OC_FILE;
            this.input.SelectionChanged += new MeGUI.StringChanged(this.input_SelectionChanged);
            // 
            // OneClickStreamControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "OneClickStreamControl";
            this.Size = new System.Drawing.Size(434, 150);
            ((System.ComponentModel.ISupportInitialize)(this.delay)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        protected System.Windows.Forms.TextBox subName;
        protected System.Windows.Forms.Label SubNamelabel;
        protected System.Windows.Forms.ComboBox subtitleLanguage;
        protected System.Windows.Forms.Label subtitleLanguageLabel;
        protected System.Windows.Forms.Label subtitleInputLabel;
        protected System.Windows.Forms.Label delayLabel;
        protected System.Windows.Forms.NumericUpDown delay;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        public System.Windows.Forms.CheckBox chkDefaultStream;
        public System.Windows.Forms.CheckBox chkForceStream;
        private core.gui.FileSCBox input;
        private core.gui.ConfigableProfilesControl encoderProfile;
        private System.Windows.Forms.ComboBox cbEncodingMode;
        protected System.Windows.Forms.Label label2;
        protected System.Windows.Forms.Label label1;
    }
}