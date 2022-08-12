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
    partial class ConfigableProfilesControl
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
            this.config = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // config
            // 
            this.config.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.config.AutoSize = true;
            this.config.Location = new System.Drawing.Point(194, 0);
            this.config.Margin = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.config.Name = "config";
            this.config.Size = new System.Drawing.Size(47, 22);
            this.config.TabIndex = 1;
            this.config.Text = "Config";
            this.config.UseVisualStyleBackColor = true;
            this.config.Click += new System.EventHandler(this.config_Click);
            this.tableLayoutPanel1.Controls.Add(this.config);
            this.tableLayoutPanel1.SetCellPosition(this.config, new System.Windows.Forms.TableLayoutPanelCellPosition(1, 0));
            // 
            // ConfigableProfilesControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Margin = new System.Windows.Forms.Padding(3);
            this.Name = "ConfigableProfilesControl";
            this.Size = new System.Drawing.Size(241, 22);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button config;
    }
}