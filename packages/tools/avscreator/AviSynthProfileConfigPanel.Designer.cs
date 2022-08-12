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
    partial class AviSynthProfileConfigPanel
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.templatePage = new System.Windows.Forms.TabPage();
            this.dllBar = new MeGUI.FileBar();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.insertInput = new System.Windows.Forms.Button();
            this.insertDeinterlace = new System.Windows.Forms.Button();
            this.insertDenoise = new System.Windows.Forms.Button();
            this.insertResize = new System.Windows.Forms.Button();
            this.insertCrop = new System.Windows.Forms.Button();
            this.avisynthScript = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.extraSetupPage = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.acceptableAspectError = new System.Windows.Forms.NumericUpDown();
            this.acceptableAspectErrorLabel = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.modValueBox = new System.Windows.Forms.ComboBox();
            this.upsize = new System.Windows.Forms.CheckBox();
            this.signalAR = new System.Windows.Forms.CheckBox();
            this.mod16Box = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cbPreferAnimeDeinterlacing = new System.Windows.Forms.CheckBox();
            this.dss2 = new System.Windows.Forms.CheckBox();
            this.mpegOptGroupBox = new System.Windows.Forms.GroupBox();
            this.colourCorrect = new System.Windows.Forms.CheckBox();
            this.mpeg2Deblocking = new System.Windows.Forms.CheckBox();
            this.filtersGroupbox = new System.Windows.Forms.GroupBox();
            this.noiseFilterType = new System.Windows.Forms.ComboBox();
            this.resize = new System.Windows.Forms.CheckBox();
            this.noiseFilter = new System.Windows.Forms.CheckBox();
            this.resizeFilterType = new System.Windows.Forms.ComboBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.chkNvResize = new System.Windows.Forms.CheckBox();
            this.tabControl1.SuspendLayout();
            this.templatePage.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.extraSetupPage.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.acceptableAspectError)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.mpegOptGroupBox.SuspendLayout();
            this.filtersGroupbox.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.templatePage);
            this.tabControl1.Controls.Add(this.extraSetupPage);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(420, 405);
            this.tabControl1.TabIndex = 1;
            // 
            // templatePage
            // 
            this.templatePage.Controls.Add(this.dllBar);
            this.templatePage.Controls.Add(this.flowLayoutPanel1);
            this.templatePage.Controls.Add(this.avisynthScript);
            this.templatePage.Controls.Add(this.label1);
            this.templatePage.Location = new System.Drawing.Point(4, 22);
            this.templatePage.Name = "templatePage";
            this.templatePage.Size = new System.Drawing.Size(412, 379);
            this.templatePage.TabIndex = 0;
            this.templatePage.Text = "Template";
            // 
            // dllBar
            // 
            this.dllBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dllBar.Filename = "";
            this.dllBar.Filter = "DLL Files (*.dll)|*.dll";
            this.dllBar.Location = new System.Drawing.Point(63, 350);
            this.dllBar.Name = "dllBar";
            this.dllBar.Size = new System.Drawing.Size(328, 23);
            this.dllBar.TabIndex = 46;
            this.dllBar.Title = "Select AviSynth Plugin DLL to open";
            this.dllBar.FileSelected += new MeGUI.FileBarEventHandler(this.dllBar_FileSelected);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanel1.Controls.Add(this.insertInput);
            this.flowLayoutPanel1.Controls.Add(this.insertDeinterlace);
            this.flowLayoutPanel1.Controls.Add(this.insertDenoise);
            this.flowLayoutPanel1.Controls.Add(this.insertResize);
            this.flowLayoutPanel1.Controls.Add(this.insertCrop);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(412, 29);
            this.flowLayoutPanel1.TabIndex = 45;
            // 
            // insertInput
            // 
            this.insertInput.AutoSize = true;
            this.insertInput.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.insertInput.Location = new System.Drawing.Point(3, 3);
            this.insertInput.Name = "insertInput";
            this.insertInput.Size = new System.Drawing.Size(62, 23);
            this.insertInput.TabIndex = 54;
            this.insertInput.Tag = "<input>";
            this.insertInput.Text = "Add input";
            this.insertInput.Click += new System.EventHandler(this.insert_Click);
            // 
            // insertDeinterlace
            // 
            this.insertDeinterlace.AutoSize = true;
            this.insertDeinterlace.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.insertDeinterlace.Location = new System.Drawing.Point(71, 3);
            this.insertDeinterlace.Name = "insertDeinterlace";
            this.insertDeinterlace.Size = new System.Drawing.Size(91, 23);
            this.insertDeinterlace.TabIndex = 55;
            this.insertDeinterlace.Tag = "<deinterlace>";
            this.insertDeinterlace.Text = "Add deinterlace";
            this.insertDeinterlace.Click += new System.EventHandler(this.insert_Click);
            // 
            // insertDenoise
            // 
            this.insertDenoise.AutoSize = true;
            this.insertDenoise.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.insertDenoise.Location = new System.Drawing.Point(168, 3);
            this.insertDenoise.Name = "insertDenoise";
            this.insertDenoise.Size = new System.Drawing.Size(76, 23);
            this.insertDenoise.TabIndex = 51;
            this.insertDenoise.Tag = "<denoise>";
            this.insertDenoise.Text = "Add denoise";
            this.insertDenoise.Click += new System.EventHandler(this.insert_Click);
            // 
            // insertResize
            // 
            this.insertResize.AutoSize = true;
            this.insertResize.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.insertResize.Location = new System.Drawing.Point(250, 3);
            this.insertResize.Name = "insertResize";
            this.insertResize.Size = new System.Drawing.Size(66, 23);
            this.insertResize.TabIndex = 52;
            this.insertResize.Tag = "<resize>";
            this.insertResize.Text = "Add resize";
            this.insertResize.Click += new System.EventHandler(this.insert_Click);
            // 
            // insertCrop
            // 
            this.insertCrop.AutoSize = true;
            this.insertCrop.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.insertCrop.Location = new System.Drawing.Point(322, 3);
            this.insertCrop.Name = "insertCrop";
            this.insertCrop.Size = new System.Drawing.Size(60, 23);
            this.insertCrop.TabIndex = 53;
            this.insertCrop.Tag = "<crop>";
            this.insertCrop.Text = "Add crop";
            this.insertCrop.Click += new System.EventHandler(this.insert_Click);
            // 
            // avisynthScript
            // 
            this.avisynthScript.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.avisynthScript.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.avisynthScript.Location = new System.Drawing.Point(3, 32);
            this.avisynthScript.Multiline = true;
            this.avisynthScript.Name = "avisynthScript";
            this.avisynthScript.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.avisynthScript.Size = new System.Drawing.Size(388, 312);
            this.avisynthScript.TabIndex = 46;
            this.avisynthScript.WordWrap = false;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 356);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 13);
            this.label1.TabIndex = 42;
            this.label1.Text = "Load DLL";
            // 
            // extraSetupPage
            // 
            this.extraSetupPage.Controls.Add(this.groupBox3);
            this.extraSetupPage.Controls.Add(this.groupBox2);
            this.extraSetupPage.Controls.Add(this.groupBox1);
            this.extraSetupPage.Controls.Add(this.mpegOptGroupBox);
            this.extraSetupPage.Controls.Add(this.filtersGroupbox);
            this.extraSetupPage.Location = new System.Drawing.Point(4, 22);
            this.extraSetupPage.Name = "extraSetupPage";
            this.extraSetupPage.Size = new System.Drawing.Size(412, 379);
            this.extraSetupPage.TabIndex = 1;
            this.extraSetupPage.Text = "Extra Setup";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.acceptableAspectError);
            this.groupBox2.Controls.Add(this.acceptableAspectErrorLabel);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.modValueBox);
            this.groupBox2.Controls.Add(this.upsize);
            this.groupBox2.Controls.Add(this.signalAR);
            this.groupBox2.Controls.Add(this.mod16Box);
            this.groupBox2.Location = new System.Drawing.Point(3, 183);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(406, 119);
            this.groupBox2.TabIndex = 24;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = " Resize Options ";
            // 
            // acceptableAspectError
            // 
            this.acceptableAspectError.DecimalPlaces = 1;
            this.acceptableAspectError.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.acceptableAspectError.Location = new System.Drawing.Point(219, 44);
            this.acceptableAspectError.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.acceptableAspectError.Name = "acceptableAspectError";
            this.acceptableAspectError.Size = new System.Drawing.Size(54, 20);
            this.acceptableAspectError.TabIndex = 26;
            this.acceptableAspectError.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // acceptableAspectErrorLabel
            // 
            this.acceptableAspectErrorLabel.AutoSize = true;
            this.acceptableAspectErrorLabel.Location = new System.Drawing.Point(23, 46);
            this.acceptableAspectErrorLabel.Name = "acceptableAspectErrorLabel";
            this.acceptableAspectErrorLabel.Size = new System.Drawing.Size(181, 13);
            this.acceptableAspectErrorLabel.TabIndex = 25;
            this.acceptableAspectErrorLabel.Text = "Acceptable Anamorphic Aspect Error";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(21, 90);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(143, 13);
            this.label2.TabIndex = 24;
            this.label2.Text = "MOD value used for resizing:";
            // 
            // modValueBox
            // 
            this.modValueBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.modValueBox.FormattingEnabled = true;
            this.modValueBox.Items.AddRange(new object[] {
            "mod16",
            "mod8",
            "mod4",
            "mod2"});
            this.modValueBox.Location = new System.Drawing.Point(219, 87);
            this.modValueBox.Name = "modValueBox";
            this.modValueBox.Size = new System.Drawing.Size(75, 21);
            this.modValueBox.TabIndex = 23;
            // 
            // upsize
            // 
            this.upsize.AutoSize = true;
            this.upsize.Location = new System.Drawing.Point(6, 67);
            this.upsize.Name = "upsize";
            this.upsize.Size = new System.Drawing.Size(105, 17);
            this.upsize.TabIndex = 22;
            this.upsize.Text = "Upsizing allowed";
            this.upsize.UseVisualStyleBackColor = true;
            // 
            // signalAR
            // 
            this.signalAR.AutoSize = true;
            this.signalAR.Location = new System.Drawing.Point(6, 19);
            this.signalAR.Name = "signalAR";
            this.signalAR.Size = new System.Drawing.Size(186, 17);
            this.signalAR.TabIndex = 20;
            this.signalAR.Text = "Clever (TM) anamorphic encoding";
            this.signalAR.CheckedChanged += new System.EventHandler(this.signalAR_CheckedChanged);
            // 
            // mod16Box
            // 
            this.mod16Box.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.mod16Box.Enabled = false;
            this.mod16Box.FormattingEnabled = true;
            this.mod16Box.Items.AddRange(new object[] {
            "Resize to selected mod value",
            "Overcrop to achieve selected mod",
            "Encode non-mod16",
            "Crop mod4 horizontally",
            "Undercrop to achieve selected mod"});
            this.mod16Box.Location = new System.Drawing.Point(219, 17);
            this.mod16Box.Name = "mod16Box";
            this.mod16Box.Size = new System.Drawing.Size(181, 21);
            this.mod16Box.TabIndex = 21;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cbPreferAnimeDeinterlacing);
            this.groupBox1.Controls.Add(this.dss2);
            this.groupBox1.Location = new System.Drawing.Point(3, 98);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(213, 79);
            this.groupBox1.TabIndex = 23;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = " Misc ";
            // 
            // cbPreferAnimeDeinterlacing
            // 
            this.cbPreferAnimeDeinterlacing.AutoSize = true;
            this.cbPreferAnimeDeinterlacing.Location = new System.Drawing.Point(6, 45);
            this.cbPreferAnimeDeinterlacing.Name = "cbPreferAnimeDeinterlacing";
            this.cbPreferAnimeDeinterlacing.Size = new System.Drawing.Size(132, 17);
            this.cbPreferAnimeDeinterlacing.TabIndex = 23;
            this.cbPreferAnimeDeinterlacing.Text = "Treat source as Anime";
            this.cbPreferAnimeDeinterlacing.UseVisualStyleBackColor = true;
            // 
            // dss2
            // 
            this.dss2.AutoSize = true;
            this.dss2.Location = new System.Drawing.Point(6, 21);
            this.dss2.Name = "dss2";
            this.dss2.Size = new System.Drawing.Size(201, 17);
            this.dss2.TabIndex = 22;
            this.dss2.Text = "Prefer DSS2 over DirectShowSource";
            this.dss2.UseVisualStyleBackColor = true;
            // 
            // mpegOptGroupBox
            // 
            this.mpegOptGroupBox.Controls.Add(this.colourCorrect);
            this.mpegOptGroupBox.Controls.Add(this.mpeg2Deblocking);
            this.mpegOptGroupBox.Location = new System.Drawing.Point(222, 98);
            this.mpegOptGroupBox.Name = "mpegOptGroupBox";
            this.mpegOptGroupBox.Size = new System.Drawing.Size(187, 79);
            this.mpegOptGroupBox.TabIndex = 12;
            this.mpegOptGroupBox.TabStop = false;
            this.mpegOptGroupBox.Text = " Mpeg Options ";
            // 
            // colourCorrect
            // 
            this.colourCorrect.Location = new System.Drawing.Point(6, 44);
            this.colourCorrect.Name = "colourCorrect";
            this.colourCorrect.Size = new System.Drawing.Size(124, 18);
            this.colourCorrect.TabIndex = 9;
            this.colourCorrect.Text = "Colour Correction";
            // 
            // mpeg2Deblocking
            // 
            this.mpeg2Deblocking.Location = new System.Drawing.Point(6, 20);
            this.mpeg2Deblocking.Name = "mpeg2Deblocking";
            this.mpeg2Deblocking.Size = new System.Drawing.Size(124, 18);
            this.mpeg2Deblocking.TabIndex = 8;
            this.mpeg2Deblocking.Text = "Mpeg2 Deblocking";
            // 
            // filtersGroupbox
            // 
            this.filtersGroupbox.Controls.Add(this.noiseFilterType);
            this.filtersGroupbox.Controls.Add(this.resize);
            this.filtersGroupbox.Controls.Add(this.noiseFilter);
            this.filtersGroupbox.Controls.Add(this.resizeFilterType);
            this.filtersGroupbox.Location = new System.Drawing.Point(3, 8);
            this.filtersGroupbox.Name = "filtersGroupbox";
            this.filtersGroupbox.Size = new System.Drawing.Size(406, 84);
            this.filtersGroupbox.TabIndex = 1;
            this.filtersGroupbox.TabStop = false;
            this.filtersGroupbox.Text = " Filters ";
            // 
            // noiseFilterType
            // 
            this.noiseFilterType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.noiseFilterType.Enabled = false;
            this.noiseFilterType.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.noiseFilterType.ItemHeight = 13;
            this.noiseFilterType.Location = new System.Drawing.Point(96, 51);
            this.noiseFilterType.Name = "noiseFilterType";
            this.noiseFilterType.Size = new System.Drawing.Size(121, 21);
            this.noiseFilterType.TabIndex = 5;
            // 
            // resize
            // 
            this.resize.AutoSize = true;
            this.resize.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.resize.Location = new System.Drawing.Point(6, 24);
            this.resize.Name = "resize";
            this.resize.Size = new System.Drawing.Size(84, 17);
            this.resize.TabIndex = 3;
            this.resize.Text = "Resize Filter";
            this.resize.CheckedChanged += new System.EventHandler(this.resize_CheckedChanged);
            // 
            // noiseFilter
            // 
            this.noiseFilter.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.noiseFilter.Location = new System.Drawing.Point(6, 49);
            this.noiseFilter.Name = "noiseFilter";
            this.noiseFilter.Size = new System.Drawing.Size(104, 24);
            this.noiseFilter.TabIndex = 3;
            this.noiseFilter.Text = "Noise Filter";
            this.noiseFilter.CheckedChanged += new System.EventHandler(this.noiseFilter_CheckedChanged);
            // 
            // resizeFilterType
            // 
            this.resizeFilterType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.resizeFilterType.Enabled = false;
            this.resizeFilterType.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.resizeFilterType.ItemHeight = 13;
            this.resizeFilterType.Location = new System.Drawing.Point(96, 22);
            this.resizeFilterType.Name = "resizeFilterType";
            this.resizeFilterType.Size = new System.Drawing.Size(121, 21);
            this.resizeFilterType.TabIndex = 1;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.chkNvResize);
            this.groupBox3.Location = new System.Drawing.Point(3, 309);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(406, 67);
            this.groupBox3.TabIndex = 25;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = " DGI Source ";
            // 
            // chkNvResize
            // 
            this.chkNvResize.AutoSize = true;
            this.chkNvResize.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkNvResize.Location = new System.Drawing.Point(6, 19);
            this.chkNvResize.Name = "chkNvResize";
            this.chkNvResize.Size = new System.Drawing.Size(196, 17);
            this.chkNvResize.TabIndex = 4;
            this.chkNvResize.Text = "Use Nvidia Crop && Resize if possible";
            // 
            // AviSynthProfileConfigPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.tabControl1);
            this.Name = "AviSynthProfileConfigPanel";
            this.Size = new System.Drawing.Size(420, 405);
            this.tabControl1.ResumeLayout(false);
            this.templatePage.ResumeLayout(false);
            this.templatePage.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.extraSetupPage.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.acceptableAspectError)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.mpegOptGroupBox.ResumeLayout(false);
            this.filtersGroupbox.ResumeLayout(false);
            this.filtersGroupbox.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage templatePage;
        private System.Windows.Forms.Button insertCrop;
        private System.Windows.Forms.Button insertInput;
        private System.Windows.Forms.Button insertDeinterlace;
        private System.Windows.Forms.Button insertDenoise;
        private System.Windows.Forms.Button insertResize;
        private System.Windows.Forms.TextBox avisynthScript;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabPage extraSetupPage;
        private System.Windows.Forms.ComboBox mod16Box;
        private System.Windows.Forms.CheckBox signalAR;
        private System.Windows.Forms.GroupBox mpegOptGroupBox;
        private System.Windows.Forms.CheckBox colourCorrect;
        private System.Windows.Forms.CheckBox mpeg2Deblocking;
        private System.Windows.Forms.GroupBox filtersGroupbox;
        private System.Windows.Forms.ComboBox noiseFilterType;
        private System.Windows.Forms.CheckBox resize;
        private System.Windows.Forms.CheckBox noiseFilter;
        private System.Windows.Forms.ComboBox resizeFilterType;
        private FileBar dllBar;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.CheckBox dss2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox upsize;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox modValueBox;
        private System.Windows.Forms.Label acceptableAspectErrorLabel;
        private System.Windows.Forms.NumericUpDown acceptableAspectError;
        private System.Windows.Forms.CheckBox cbPreferAnimeDeinterlacing;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox chkNvResize;
    }
}
