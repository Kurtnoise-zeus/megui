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
    partial class OneClickAudioControl
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
            this.language = new System.Windows.Forms.ComboBox();
            this.languageLabel = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label2 = new System.Windows.Forms.Label();
            this.cbEncodingMode = new System.Windows.Forms.ComboBox();
            this.encoderProfile = new MeGUI.core.gui.ConfigableProfilesControl();
            this.label1 = new System.Windows.Forms.Label();
            this.cbFirstTrackOnly = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // language
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.language, 2);
            this.language.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.language.Location = new System.Drawing.Point(64, 3);
            this.language.Name = "language";
            this.language.Size = new System.Drawing.Size(312, 21);
            this.language.Sorted = true;
            this.language.TabIndex = 37;
            this.language.SelectedIndexChanged += new System.EventHandler(this.language_SelectedIndexChanged);
            // 
            // languageLabel
            // 
            this.languageLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.languageLabel.AutoSize = true;
            this.languageLabel.Location = new System.Drawing.Point(3, 8);
            this.languageLabel.Name = "languageLabel";
            this.languageLabel.Size = new System.Drawing.Size(55, 13);
            this.languageLabel.TabIndex = 36;
            this.languageLabel.Text = "Language";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.cbEncodingMode, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.encoderProfile, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.language, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.languageLabel, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.cbFirstTrackOnly, 1, 3);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(380, 120);
            this.tableLayoutPanel1.TabIndex = 44;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 68);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 13);
            this.label2.TabIndex = 51;
            this.label2.Text = "Encode";
            // 
            // cbEncodingMode
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.cbEncodingMode, 2);
            this.cbEncodingMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbEncodingMode.FormattingEnabled = true;
            this.cbEncodingMode.Location = new System.Drawing.Point(64, 63);
            this.cbEncodingMode.Name = "cbEncodingMode";
            this.cbEncodingMode.Size = new System.Drawing.Size(312, 21);
            this.cbEncodingMode.TabIndex = 49;
            this.cbEncodingMode.SelectedIndexChanged += new System.EventHandler(this.cbEncodingMode_SelectedIndexChanged);
            // 
            // encoderProfile
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.encoderProfile, 2);
            this.encoderProfile.Location = new System.Drawing.Point(64, 33);
            this.encoderProfile.Name = "encoderProfile";
            this.encoderProfile.ProfileSet = "Audio";
            this.encoderProfile.Size = new System.Drawing.Size(313, 22);
            this.encoderProfile.TabIndex = 48;
            this.encoderProfile.UpdateSelectedProfile = false;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 50;
            this.label1.Text = "Encoder";
            // 
            // cbFirstTrackOnly
            // 
            this.cbFirstTrackOnly.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.cbFirstTrackOnly, 2);
            this.cbFirstTrackOnly.Location = new System.Drawing.Point(64, 93);
            this.cbFirstTrackOnly.Name = "cbFirstTrackOnly";
            this.cbFirstTrackOnly.Size = new System.Drawing.Size(129, 17);
            this.cbFirstTrackOnly.TabIndex = 52;
            this.cbFirstTrackOnly.Text = "use only the first track";
            this.cbFirstTrackOnly.UseVisualStyleBackColor = true;
            // 
            // OneClickAudioControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "OneClickAudioControl";
            this.Size = new System.Drawing.Size(380, 120);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        protected System.Windows.Forms.ComboBox language;
        protected System.Windows.Forms.Label languageLabel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private core.gui.ConfigableProfilesControl encoderProfile;
        private System.Windows.Forms.ComboBox cbEncodingMode;
        protected System.Windows.Forms.Label label2;
        protected System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox cbFirstTrackOnly;
    }
}
