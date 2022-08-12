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

namespace MeGUI.core.details.video
{
    partial class VideoConfigurationPanel
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.mainTabPage = new System.Windows.Forms.TabPage();
            this.commandline = new System.Windows.Forms.TextBox();
            this.tooltipHelp = new System.Windows.Forms.ToolTip(this.components);
            this.tabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.mainTabPage);
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(368, 344);
            this.tabControl1.TabIndex = 39;
            // 
            // mainTabPage
            // 
            this.mainTabPage.Location = new System.Drawing.Point(4, 22);
            this.mainTabPage.Name = "mainTabPage";
            this.mainTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.mainTabPage.Size = new System.Drawing.Size(360, 318);
            this.mainTabPage.TabIndex = 0;
            this.mainTabPage.Text = "Main";
            this.mainTabPage.UseVisualStyleBackColor = true;
            // 
            // commandline
            // 
            this.commandline.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.commandline.Location = new System.Drawing.Point(3, 350);
            this.commandline.Multiline = true;
            this.commandline.Name = "commandline";
            this.commandline.ReadOnly = true;
            this.commandline.Size = new System.Drawing.Size(365, 59);
            this.commandline.TabIndex = 41;
            // 
            // tooltipHelp
            // 
            this.tooltipHelp.AutoPopDelay = 30000;
            this.tooltipHelp.InitialDelay = 500;
            this.tooltipHelp.IsBalloon = true;
            this.tooltipHelp.ReshowDelay = 100;
            this.tooltipHelp.ShowAlways = true;
            // 
            // VideoConfigurationPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.commandline);
            this.Controls.Add(this.tabControl1);
            this.Name = "VideoConfigurationPanel";
            this.Size = new System.Drawing.Size(372, 409);
            this.Load += new System.EventHandler(this.VideoConfigurationPanel_Load);
            this.tabControl1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        protected ToolTip tooltipHelp;
        protected TabControl tabControl1;
        protected TextBox commandline;
        protected TabPage mainTabPage;
    }
}