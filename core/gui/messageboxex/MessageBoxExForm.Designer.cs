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

namespace MeGUI.core.gui.MessageBoxExLib
{
    partial class MessageBoxExForm
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
            this.panelIcon = new System.Windows.Forms.Panel();
            this.chbSaveResponse = new System.Windows.Forms.CheckBox();
            this.imageListIcons = new System.Windows.Forms.ImageList(this.components);
            this.buttonToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.rtbMessage = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // panelIcon
            // 
            this.panelIcon.BackColor = System.Drawing.Color.Transparent;
            this.panelIcon.Location = new System.Drawing.Point(8, 8);
            this.panelIcon.Name = "panelIcon";
            this.panelIcon.Size = new System.Drawing.Size(32, 32);
            this.panelIcon.TabIndex = 3;
            this.panelIcon.Visible = false;
            // 
            // chbSaveResponse
            // 
            this.chbSaveResponse.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.chbSaveResponse.Location = new System.Drawing.Point(56, 56);
            this.chbSaveResponse.Name = "chbSaveResponse";
            this.chbSaveResponse.Size = new System.Drawing.Size(104, 16);
            this.chbSaveResponse.TabIndex = 0;
            // 
            // imageListIcons
            // 
            this.imageListIcons.ColorDepth = System.Windows.Forms.ColorDepth.Depth16Bit;
            this.imageListIcons.ImageSize = new System.Drawing.Size(32, 32);
            this.imageListIcons.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // rtbMessage
            // 
            this.rtbMessage.BackColor = System.Drawing.SystemColors.Control;
            this.rtbMessage.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbMessage.Location = new System.Drawing.Point(200, 8);
            this.rtbMessage.Name = "rtbMessage";
            this.rtbMessage.ReadOnly = true;
            this.rtbMessage.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.rtbMessage.Size = new System.Drawing.Size(100, 48);
            this.rtbMessage.TabIndex = 4;
            this.rtbMessage.Text = "";
            this.rtbMessage.Visible = false;
            // 
            // MessageBoxExForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(322, 224);
            this.Controls.Add(this.rtbMessage);
            this.Controls.Add(this.chbSaveResponse);
            this.Controls.Add(this.panelIcon);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MessageBoxExForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.ResumeLayout(false);

        }
        #endregion

        private System.Windows.Forms.CheckBox chbSaveResponse;
        private System.Windows.Forms.ImageList imageListIcons;
        private System.Windows.Forms.ToolTip buttonToolTip;
    }
}