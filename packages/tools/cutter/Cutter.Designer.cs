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

namespace MeGUI.packages.tools.cutter
{
    partial class Cutter
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Cutter));
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.addZoneButton = new System.Windows.Forms.Button();
            this.clearZonesButton = new System.Windows.Forms.Button();
            this.updateZoneButton = new System.Windows.Forms.Button();
            this.removeZoneButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.transitionStyle = new System.Windows.Forms.ComboBox();
            this.startFrameLabel = new System.Windows.Forms.Label();
            this.endFrameLabel = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.startFrame = new System.Windows.Forms.NumericUpDown();
            this.endFrame = new System.Windows.Forms.NumericUpDown();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.doAllClose = new System.Windows.Forms.Button();
            this.closeButton = new System.Windows.Forms.Button();
            this.saveCuts = new System.Windows.Forms.Button();
            this.addCutsToScript = new System.Windows.Forms.Button();
            this.helpButton1 = new MeGUI.core.gui.HelpButton();
            this.avsScript = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.sections = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.flowLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.startFrame)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.endFrame)).BeginInit();
            this.flowLayoutPanel2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.Controls.Add(this.addZoneButton);
            this.flowLayoutPanel1.Controls.Add(this.clearZonesButton);
            this.flowLayoutPanel1.Controls.Add(this.updateZoneButton);
            this.flowLayoutPanel1.Controls.Add(this.removeZoneButton);
            this.flowLayoutPanel1.Controls.Add(this.label1);
            this.flowLayoutPanel1.Controls.Add(this.transitionStyle);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 354);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Padding = new System.Windows.Forms.Padding(5);
            this.flowLayoutPanel1.Size = new System.Drawing.Size(476, 39);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // addZoneButton
            // 
            this.addZoneButton.AutoSize = true;
            this.addZoneButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.addZoneButton.Location = new System.Drawing.Point(8, 8);
            this.addZoneButton.Name = "addZoneButton";
            this.addZoneButton.Size = new System.Drawing.Size(36, 23);
            this.addZoneButton.TabIndex = 16;
            this.addZoneButton.Text = "Add";
            this.addZoneButton.Click += new System.EventHandler(this.addZoneButton_Click);
            // 
            // clearZonesButton
            // 
            this.clearZonesButton.AutoSize = true;
            this.clearZonesButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.clearZonesButton.Enabled = false;
            this.clearZonesButton.Location = new System.Drawing.Point(50, 8);
            this.clearZonesButton.Name = "clearZonesButton";
            this.clearZonesButton.Size = new System.Drawing.Size(42, 23);
            this.clearZonesButton.TabIndex = 14;
            this.clearZonesButton.Text = "Clear";
            this.clearZonesButton.Click += new System.EventHandler(this.clearZonesButton_Click);
            // 
            // updateZoneButton
            // 
            this.updateZoneButton.AutoSize = true;
            this.updateZoneButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.updateZoneButton.Enabled = false;
            this.updateZoneButton.Location = new System.Drawing.Point(98, 8);
            this.updateZoneButton.Name = "updateZoneButton";
            this.updateZoneButton.Size = new System.Drawing.Size(52, 23);
            this.updateZoneButton.TabIndex = 17;
            this.updateZoneButton.Text = "Update";
            this.updateZoneButton.Click += new System.EventHandler(this.updateZoneButton_Click);
            // 
            // removeZoneButton
            // 
            this.removeZoneButton.AutoSize = true;
            this.removeZoneButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.removeZoneButton.Enabled = false;
            this.removeZoneButton.Location = new System.Drawing.Point(156, 8);
            this.removeZoneButton.Name = "removeZoneButton";
            this.removeZoneButton.Size = new System.Drawing.Size(56, 23);
            this.removeZoneButton.TabIndex = 15;
            this.removeZoneButton.Text = "Remove";
            this.removeZoneButton.Click += new System.EventHandler(this.removeZoneButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(218, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 29);
            this.label1.TabIndex = 18;
            this.label1.Text = "Transition style";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // transitionStyle
            // 
            this.transitionStyle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.transitionStyle.FormattingEnabled = true;
            this.transitionStyle.Location = new System.Drawing.Point(304, 8);
            this.transitionStyle.Name = "transitionStyle";
            this.transitionStyle.Size = new System.Drawing.Size(141, 21);
            this.transitionStyle.TabIndex = 19;
            this.transitionStyle.SelectedIndexChanged += new System.EventHandler(this.transitionStyle_SelectedIndexChanged);
            // 
            // startFrameLabel
            // 
            this.startFrameLabel.AutoSize = true;
            this.startFrameLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.startFrameLabel.Location = new System.Drawing.Point(8, 5);
            this.startFrameLabel.Name = "startFrameLabel";
            this.startFrameLabel.Size = new System.Drawing.Size(64, 27);
            this.startFrameLabel.TabIndex = 10;
            this.startFrameLabel.Text = "Start Frame";
            this.startFrameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // endFrameLabel
            // 
            this.endFrameLabel.AutoSize = true;
            this.endFrameLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.endFrameLabel.Location = new System.Drawing.Point(8, 32);
            this.endFrameLabel.Name = "endFrameLabel";
            this.endFrameLabel.Size = new System.Drawing.Size(64, 27);
            this.endFrameLabel.TabIndex = 12;
            this.endFrameLabel.Text = "End Frame";
            this.endFrameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.startFrameLabel, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.endFrameLabel, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.startFrame, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.endFrame, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 290);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.Padding = new System.Windows.Forms.Padding(5);
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(476, 64);
            this.tableLayoutPanel1.TabIndex = 14;
            // 
            // startFrame
            // 
            this.startFrame.Dock = System.Windows.Forms.DockStyle.Fill;
            this.startFrame.Location = new System.Drawing.Point(78, 8);
            this.startFrame.Name = "startFrame";
            this.startFrame.Size = new System.Drawing.Size(393, 21);
            this.startFrame.TabIndex = 13;
            // 
            // endFrame
            // 
            this.endFrame.Dock = System.Windows.Forms.DockStyle.Fill;
            this.endFrame.Location = new System.Drawing.Point(78, 35);
            this.endFrame.Name = "endFrame";
            this.endFrame.Size = new System.Drawing.Size(393, 21);
            this.endFrame.TabIndex = 14;
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.AutoSize = true;
            this.flowLayoutPanel2.Controls.Add(this.doAllClose);
            this.flowLayoutPanel2.Controls.Add(this.closeButton);
            this.flowLayoutPanel2.Controls.Add(this.saveCuts);
            this.flowLayoutPanel2.Controls.Add(this.addCutsToScript);
            this.flowLayoutPanel2.Controls.Add(this.helpButton1);
            this.flowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.flowLayoutPanel2.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flowLayoutPanel2.Location = new System.Drawing.Point(0, 393);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Padding = new System.Windows.Forms.Padding(5);
            this.flowLayoutPanel2.Size = new System.Drawing.Size(476, 39);
            this.flowLayoutPanel2.TabIndex = 15;
            // 
            // doAllClose
            // 
            this.doAllClose.AutoSize = true;
            this.doAllClose.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.doAllClose.Location = new System.Drawing.Point(372, 8);
            this.doAllClose.Name = "doAllClose";
            this.doAllClose.Size = new System.Drawing.Size(91, 23);
            this.doAllClose.TabIndex = 4;
            this.doAllClose.Text = "Do all and close";
            this.doAllClose.UseVisualStyleBackColor = true;
            this.doAllClose.Click += new System.EventHandler(this.doAllClose_Click);
            // 
            // closeButton
            // 
            this.closeButton.AutoSize = true;
            this.closeButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.closeButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.closeButton.Location = new System.Drawing.Point(323, 8);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(43, 23);
            this.closeButton.TabIndex = 0;
            this.closeButton.Text = "Close";
            this.closeButton.UseVisualStyleBackColor = true;
            this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
            // 
            // saveCuts
            // 
            this.saveCuts.AutoSize = true;
            this.saveCuts.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.saveCuts.Location = new System.Drawing.Point(236, 8);
            this.saveCuts.Name = "saveCuts";
            this.saveCuts.Size = new System.Drawing.Size(81, 23);
            this.saveCuts.TabIndex = 1;
            this.saveCuts.Text = "Save cuts file";
            this.saveCuts.UseVisualStyleBackColor = true;
            this.saveCuts.Click += new System.EventHandler(this.saveCuts_Click);
            // 
            // addCutsToScript
            // 
            this.addCutsToScript.AutoSize = true;
            this.addCutsToScript.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.addCutsToScript.Location = new System.Drawing.Point(129, 8);
            this.addCutsToScript.Name = "addCutsToScript";
            this.addCutsToScript.Size = new System.Drawing.Size(101, 23);
            this.addCutsToScript.TabIndex = 2;
            this.addCutsToScript.Text = "Add cuts to script";
            this.addCutsToScript.UseVisualStyleBackColor = true;
            this.addCutsToScript.Click += new System.EventHandler(this.addCutsToScript_Click);
            // 
            // helpButton1
            // 
            this.helpButton1.ArticleName = "Avs Cutter";
            this.helpButton1.AutoSize = true;
            this.helpButton1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.helpButton1.Location = new System.Drawing.Point(85, 8);
            this.helpButton1.Name = "helpButton1";
            this.helpButton1.Size = new System.Drawing.Size(38, 23);
            this.helpButton1.TabIndex = 3;
            // 
            // avsScript
            // 
            this.avsScript.Dock = System.Windows.Forms.DockStyle.Top;
            this.avsScript.Location = new System.Drawing.Point(0, 0);
            this.avsScript.Name = "avsScript";
            this.avsScript.Padding = new System.Windows.Forms.Padding(2);
            this.avsScript.Size = new System.Drawing.Size(476, 21);
            this.avsScript.TabIndex = 17;
            this.avsScript.Text = "AviSynth script:   ";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.sections);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 21);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(5);
            this.groupBox1.Size = new System.Drawing.Size(476, 269);
            this.groupBox1.TabIndex = 18;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Included sections";
            // 
            // sections
            // 
            this.sections.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.sections.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sections.FullRowSelect = true;
            this.sections.Location = new System.Drawing.Point(5, 19);
            this.sections.Name = "sections";
            this.sections.Size = new System.Drawing.Size(466, 245);
            this.sections.TabIndex = 17;
            this.sections.UseCompatibleStateImageBehavior = false;
            this.sections.View = System.Windows.Forms.View.Details;
            this.sections.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.sections_ItemSelectionChanged);
            this.sections.SelectedIndexChanged += new System.EventHandler(this.sections_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Start frame";
            this.columnHeader1.Width = 193;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "End frame";
            this.columnHeader2.Width = 262;
            // 
            // Cutter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(476, 432);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.avsScript);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.flowLayoutPanel2);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Cutter";
            this.Text = "MeGUI - AVS Cutter";
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.startFrame)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.endFrame)).EndInit();
            this.flowLayoutPanel2.ResumeLayout(false);
            this.flowLayoutPanel2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button addZoneButton;
        private System.Windows.Forms.Button clearZonesButton;
        private System.Windows.Forms.Button updateZoneButton;
        private System.Windows.Forms.Button removeZoneButton;
        private System.Windows.Forms.Label startFrameLabel;
        private System.Windows.Forms.Label endFrameLabel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox transitionStyle;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.Button saveCuts;
        private System.Windows.Forms.NumericUpDown startFrame;
        private System.Windows.Forms.NumericUpDown endFrame;
        private System.Windows.Forms.Button addCutsToScript;
        private System.Windows.Forms.Label avsScript;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListView sections;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.Button doAllClose;
        private MeGUI.core.gui.HelpButton helpButton1;        
    }
}