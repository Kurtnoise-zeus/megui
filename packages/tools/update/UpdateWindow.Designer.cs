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
    partial class UpdateWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UpdateWindow));
            this.statusToolStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.enableToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setIgnoreValue = new System.Windows.Forms.ToolStripMenuItem();
            this.reinstallToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.listViewDetails = new System.Windows.Forms.ListView();
            this.colUpdate = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colExistingVersion = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colLatestVersion = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colArchitecture = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colExistingDate = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colLatestDate = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colLastUsed = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colStatus = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.txtBoxLog = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.chkShowAllFiles = new System.Windows.Forms.CheckBox();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.helpButton1 = new MeGUI.core.gui.HelpButton();
            this.progressBar = new MeGUI.core.gui.CustomProgressBar();
            this.statusToolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusToolStrip
            // 
            this.statusToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.enableToolStripMenuItem,
            this.setIgnoreValue,
            this.reinstallToolStripMenuItem});
            this.statusToolStrip.Name = "statusToolStrip";
            this.statusToolStrip.ShowCheckMargin = true;
            this.statusToolStrip.ShowImageMargin = false;
            this.statusToolStrip.Size = new System.Drawing.Size(156, 70);
            // 
            // enableToolStripMenuItem
            // 
            this.enableToolStripMenuItem.CheckOnClick = true;
            this.enableToolStripMenuItem.Name = "enableToolStripMenuItem";
            this.enableToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
            this.enableToolStripMenuItem.Text = "Enable";
            this.enableToolStripMenuItem.Click += new System.EventHandler(this.enableToolStripMenuItem_Click);
            // 
            // setIgnoreValue
            // 
            this.setIgnoreValue.CheckOnClick = true;
            this.setIgnoreValue.Name = "setIgnoreValue";
            this.setIgnoreValue.Size = new System.Drawing.Size(155, 22);
            this.setIgnoreValue.Text = "Ignore updates";
            this.setIgnoreValue.Click += new System.EventHandler(this.setIgnoreValue_Click);
            // 
            // reinstallToolStripMenuItem
            // 
            this.reinstallToolStripMenuItem.CheckOnClick = true;
            this.reinstallToolStripMenuItem.Name = "reinstallToolStripMenuItem";
            this.reinstallToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
            this.reinstallToolStripMenuItem.Text = "Force (re)install";
            this.reinstallToolStripMenuItem.Click += new System.EventHandler(this.reinstallToolStripMenuItem_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.panel1);
            this.splitContainer1.Size = new System.Drawing.Size(721, 375);
            this.splitContainer1.SplitterDistance = 305;
            this.splitContainer1.SplitterWidth = 1;
            this.splitContainer1.TabIndex = 11;
            this.splitContainer1.SizeChanged += new System.EventHandler(this.splitContainer1_SizeChanged);
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.listViewDetails);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.txtBoxLog);
            this.splitContainer2.Size = new System.Drawing.Size(721, 305);
            this.splitContainer2.SplitterDistance = 199;
            this.splitContainer2.TabIndex = 0;
            this.splitContainer2.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitContainer2_SplitterMoved);
            // 
            // listViewDetails
            // 
            this.listViewDetails.CheckBoxes = true;
            this.listViewDetails.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colUpdate,
            this.colName,
            this.colExistingVersion,
            this.colLatestVersion,
            this.colArchitecture,
            this.colExistingDate,
            this.colLatestDate,
            this.colLastUsed,
            this.colStatus});
            this.listViewDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewDetails.FullRowSelect = true;
            this.listViewDetails.Location = new System.Drawing.Point(0, 0);
            this.listViewDetails.Name = "listViewDetails";
            this.listViewDetails.Size = new System.Drawing.Size(721, 199);
            this.listViewDetails.TabIndex = 6;
            this.listViewDetails.UseCompatibleStateImageBehavior = false;
            this.listViewDetails.View = System.Windows.Forms.View.Details;
            this.listViewDetails.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.listViewDetails_ColumnClick);
            this.listViewDetails.ColumnWidthChanged += new System.Windows.Forms.ColumnWidthChangedEventHandler(this.listViewDetails_ColumnWidthChanged);
            this.listViewDetails.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.listViewDetails_ItemCheck);
            this.listViewDetails.MouseClick += new System.Windows.Forms.MouseEventHandler(this.listViewDetails_MouseClick);
            // 
            // colUpdate
            // 
            this.colUpdate.Text = "Update";
            this.colUpdate.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.colUpdate.Width = 47;
            // 
            // colName
            // 
            this.colName.Text = "Name";
            this.colName.Width = 105;
            // 
            // colExistingVersion
            // 
            this.colExistingVersion.Text = "Local Version";
            this.colExistingVersion.Width = 117;
            // 
            // colLatestVersion
            // 
            this.colLatestVersion.Text = "Server Version";
            this.colLatestVersion.Width = 117;
            // 
            // colArchitecture
            // 
            this.colArchitecture.Text = "Build";
            this.colArchitecture.Width = 50;
            // 
            // colExistingDate
            // 
            this.colExistingDate.Text = "Local Date";
            this.colExistingDate.Width = 70;
            // 
            // colLatestDate
            // 
            this.colLatestDate.Text = "Server Date";
            this.colLatestDate.Width = 70;
            // 
            // colLastUsed
            // 
            this.colLastUsed.Text = "Last Used";
            this.colLastUsed.Width = 59;
            // 
            // colStatus
            // 
            this.colStatus.Text = "Status";
            this.colStatus.Width = 131;
            // 
            // txtBoxLog
            // 
            this.txtBoxLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtBoxLog.Location = new System.Drawing.Point(0, 0);
            this.txtBoxLog.Multiline = true;
            this.txtBoxLog.Name = "txtBoxLog";
            this.txtBoxLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtBoxLog.Size = new System.Drawing.Size(721, 102);
            this.txtBoxLog.TabIndex = 9;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.chkShowAllFiles);
            this.panel1.Controls.Add(this.helpButton1);
            this.panel1.Controls.Add(this.progressBar);
            this.panel1.Controls.Add(this.btnUpdate);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(721, 65);
            this.panel1.TabIndex = 13;
            // 
            // chkShowAllFiles
            // 
            this.chkShowAllFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.chkShowAllFiles.AutoSize = true;
            this.chkShowAllFiles.Location = new System.Drawing.Point(515, 34);
            this.chkShowAllFiles.Name = "chkShowAllFiles";
            this.chkShowAllFiles.Size = new System.Drawing.Size(113, 17);
            this.chkShowAllFiles.TabIndex = 9;
            this.chkShowAllFiles.Text = "Show all packages";
            this.chkShowAllFiles.UseVisualStyleBackColor = true;
            this.chkShowAllFiles.CheckedChanged += new System.EventHandler(this.chkShowAllFiles_CheckedChanged);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUpdate.Enabled = false;
            this.btnUpdate.Location = new System.Drawing.Point(634, 30);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(75, 23);
            this.btnUpdate.TabIndex = 3;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // helpButton1
            // 
            this.helpButton1.ArticleName = "Presentation/AutoUpdate";
            this.helpButton1.AutoSize = true;
            this.helpButton1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.helpButton1.Location = new System.Drawing.Point(12, 30);
            this.helpButton1.Name = "helpButton1";
            this.helpButton1.Size = new System.Drawing.Size(38, 23);
            this.helpButton1.TabIndex = 8;
            // 
            // progressBar
            // 
            this.progressBar.CustomText = null;
            this.progressBar.DisplayStyle = MeGUI.core.gui.ProgressBarDisplayText.CustomText;
            this.progressBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.progressBar.Location = new System.Drawing.Point(0, 0);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(721, 24);
            this.progressBar.TabIndex = 7;
            // 
            // UpdateWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(721, 375);
            this.Controls.Add(this.splitContainer1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(400, 200);
            this.Name = "UpdateWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "MeGUI - Updater";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.UpdateWindow_FormClosing);
            this.Load += new System.EventHandler(this.UpdateWindow_Load);
            this.Shown += new System.EventHandler(this.UpdateWindow_Shown);
            this.Move += new System.EventHandler(this.UpdateWindow_Move);
            this.Resize += new System.EventHandler(this.UpdateWindow_Resize);
            this.statusToolStrip.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView listViewDetails;
        private System.Windows.Forms.ColumnHeader colUpdate;
        private System.Windows.Forms.ColumnHeader colName;
        private System.Windows.Forms.ColumnHeader colExistingVersion;
        private System.Windows.Forms.ColumnHeader colLatestVersion;
        private System.Windows.Forms.ColumnHeader colStatus;
        private System.Windows.Forms.TextBox txtBoxLog;
        private System.Windows.Forms.ContextMenuStrip statusToolStrip;
        private System.Windows.Forms.ToolStripMenuItem setIgnoreValue;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.ToolStripMenuItem reinstallToolStripMenuItem;
        private System.Windows.Forms.ColumnHeader colLastUsed;
        private System.Windows.Forms.ColumnHeader colExistingDate;
        private System.Windows.Forms.ColumnHeader colLatestDate;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckBox chkShowAllFiles;
        private core.gui.HelpButton helpButton1;
        private MeGUI.core.gui.CustomProgressBar progressBar;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.ToolStripMenuItem enableToolStripMenuItem;
        private System.Windows.Forms.ColumnHeader colArchitecture;
    }
}