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
  partial class frmStreamSelect
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmStreamSelect));
            this.btnOK = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.rbShowOnly = new System.Windows.Forms.RadioButton();
            this.rbShowAll = new System.Windows.Forms.RadioButton();
            this.minimumTitleLength = new System.Windows.Forms.NumericUpDown();
            this.btnSortChapter = new System.Windows.Forms.RadioButton();
            this.btnSortName = new System.Windows.Forms.RadioButton();
            this.btnSortDuration = new System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.minimumTitleLength)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnOK.Location = new System.Drawing.Point(130, 353);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(76, 22);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // listBox1
            // 
            this.listBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(13, 93);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(429, 238);
            this.listBox1.TabIndex = 2;
            this.listBox1.DoubleClick += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(230, 353);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(76, 22);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.panel1);
            this.groupBox1.Controls.Add(this.btnSortChapter);
            this.groupBox1.Controls.Add(this.btnSortName);
            this.groupBox1.Controls.Add(this.btnSortDuration);
            this.groupBox1.Location = new System.Drawing.Point(13, 8);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(429, 81);
            this.groupBox1.TabIndex = 14;
            this.groupBox1.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.rbShowOnly);
            this.panel1.Controls.Add(this.rbShowAll);
            this.panel1.Controls.Add(this.minimumTitleLength);
            this.panel1.Location = new System.Drawing.Point(6, 43);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(411, 34);
            this.panel1.TabIndex = 20;
            // 
            // rbShowOnly
            // 
            this.rbShowOnly.AutoSize = true;
            this.rbShowOnly.Checked = true;
            this.rbShowOnly.Location = new System.Drawing.Point(149, 5);
            this.rbShowOnly.Name = "rbShowOnly";
            this.rbShowOnly.Size = new System.Drawing.Size(201, 17);
            this.rbShowOnly.TabIndex = 20;
            this.rbShowOnly.TabStop = true;
            this.rbShowOnly.Text = "show only if longer than (in seconds)";
            this.rbShowOnly.UseVisualStyleBackColor = true;
            this.rbShowOnly.CheckedChanged += new System.EventHandler(this.btnSort_CheckedChanged);
            // 
            // rbShowAll
            // 
            this.rbShowAll.AutoSize = true;
            this.rbShowAll.Location = new System.Drawing.Point(0, 5);
            this.rbShowAll.Name = "rbShowAll";
            this.rbShowAll.Size = new System.Drawing.Size(92, 17);
            this.rbShowAll.TabIndex = 19;
            this.rbShowAll.TabStop = true;
            this.rbShowAll.Text = "show all (xxx)";
            this.rbShowAll.UseVisualStyleBackColor = true;
            this.rbShowAll.CheckedChanged += new System.EventHandler(this.btnSort_CheckedChanged);
            // 
            // minimumTitleLength
            // 
            this.minimumTitleLength.Location = new System.Drawing.Point(357, 5);
            this.minimumTitleLength.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.minimumTitleLength.Name = "minimumTitleLength";
            this.minimumTitleLength.Size = new System.Drawing.Size(54, 21);
            this.minimumTitleLength.TabIndex = 21;
            this.minimumTitleLength.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.minimumTitleLength.Value = new decimal(new int[] {
            900,
            0,
            0,
            0});
            this.minimumTitleLength.ValueChanged += new System.EventHandler(this.minimumTitleLength_ValueChanged);
            this.minimumTitleLength.KeyUp += new System.Windows.Forms.KeyEventHandler(this.minimumTitleLength_KeyUp);
            // 
            // btnSortChapter
            // 
            this.btnSortChapter.AutoSize = true;
            this.btnSortChapter.Location = new System.Drawing.Point(288, 20);
            this.btnSortChapter.Name = "btnSortChapter";
            this.btnSortChapter.Size = new System.Drawing.Size(129, 17);
            this.btnSortChapter.TabIndex = 18;
            this.btnSortChapter.Text = "sort by chapter count";
            this.btnSortChapter.UseVisualStyleBackColor = true;
            this.btnSortChapter.CheckedChanged += new System.EventHandler(this.btnSort_CheckedChanged);
            // 
            // btnSortName
            // 
            this.btnSortName.AutoSize = true;
            this.btnSortName.Location = new System.Drawing.Point(155, 20);
            this.btnSortName.Name = "btnSortName";
            this.btnSortName.Size = new System.Drawing.Size(88, 17);
            this.btnSortName.TabIndex = 17;
            this.btnSortName.Text = "sort by name";
            this.btnSortName.UseVisualStyleBackColor = true;
            this.btnSortName.CheckedChanged += new System.EventHandler(this.btnSort_CheckedChanged);
            // 
            // btnSortDuration
            // 
            this.btnSortDuration.AutoSize = true;
            this.btnSortDuration.Location = new System.Drawing.Point(6, 20);
            this.btnSortDuration.Name = "btnSortDuration";
            this.btnSortDuration.Size = new System.Drawing.Size(102, 17);
            this.btnSortDuration.TabIndex = 16;
            this.btnSortDuration.Text = "sort by duration";
            this.btnSortDuration.UseVisualStyleBackColor = true;
            this.btnSortDuration.CheckedChanged += new System.EventHandler(this.btnSort_CheckedChanged);
            // 
            // frmStreamSelect
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(454, 387);
            this.ControlBox = false;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.btnOK);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(470, 426);
            this.Name = "frmStreamSelect";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Select your playlist";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmStreamSelect_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.minimumTitleLength)).EndInit();
            this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Button btnOK;
    private System.Windows.Forms.ListBox listBox1;
    private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton btnSortChapter;
        private System.Windows.Forms.RadioButton btnSortName;
        private System.Windows.Forms.RadioButton btnSortDuration;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton rbShowOnly;
        private System.Windows.Forms.RadioButton rbShowAll;
        private System.Windows.Forms.NumericUpDown minimumTitleLength;
    }
}