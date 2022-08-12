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

namespace MeGUI.packages.audio.vorbis
{
    partial class OggVorbisConfigurationPanel
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
            this.vQuality = new System.Windows.Forms.TrackBar();
            this.label1 = new System.Windows.Forms.Label();
            this.encoderGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.vQuality)).BeginInit();
            this.SuspendLayout();
            // 
            // encoderGroupBox
            // 
            this.encoderGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
            this.encoderGroupBox.Controls.Add(this.vQuality);
            this.encoderGroupBox.Controls.Add(this.label1);
            this.encoderGroupBox.Size = new System.Drawing.Size(402, 84);
            this.encoderGroupBox.TabIndex = 1;
            this.encoderGroupBox.Text = " Ogg Vorbis Options ";
            // 
            // besweetOptionsGroupbox
            // 
            this.besweetOptionsGroupbox.TabIndex = 0;
            // 
            // vQuality
            // 
            this.vQuality.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.vQuality.Location = new System.Drawing.Point(3, 36);
            this.vQuality.Maximum = 1000;
            this.vQuality.Minimum = -200;
            this.vQuality.Name = "vQuality";
            this.vQuality.Size = new System.Drawing.Size(396, 45);
            this.vQuality.TabIndex = 1;
            this.vQuality.TickFrequency = 25;
            this.vQuality.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.vQuality.ValueChanged += new System.EventHandler(this.vQuality_ValueChanged);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "label1";
            // 
            // OggVorbisConfigurationPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Name = "OggVorbisConfigurationPanel";
            this.Size = new System.Drawing.Size(411, 302);
            this.encoderGroupBox.ResumeLayout(false);
            this.encoderGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.vQuality)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.TrackBar vQuality;
        private System.Windows.Forms.Label label1;
    }
}