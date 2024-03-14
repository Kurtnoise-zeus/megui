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

namespace MeGUI.packages.tools.hdbdextractor
{
    partial class HdBdStreamExtractor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HdBdStreamExtractor));
            this.StatusStrip = new System.Windows.Forms.StatusStrip();
            this.ToolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.ToolStripProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.FolderInputTextBox = new System.Windows.Forms.TextBox();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.HelpButton2 = new System.Windows.Forms.Button();
            this.QueueButton = new System.Windows.Forms.Button();
            this.CancelButton2 = new System.Windows.Forms.Button();
            this.InputGroupBox = new System.Windows.Forms.GroupBox();
            this.FileSelection = new System.Windows.Forms.RadioButton();
            this.FolderSelection = new System.Windows.Forms.RadioButton();
            this.FolderInputSourceButton = new System.Windows.Forms.Button();
            this.Eac3toLinkLabel = new System.Windows.Forms.LinkLabel();
            this.FeatureGroupBox = new System.Windows.Forms.GroupBox();
            this.FeatureDataGridView = new MeGUI.packages.tools.hdbdextractor.CustomDataGridView();
            this.FeatureNumberDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FeatureNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FeatureDescriptionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FeatureFileDataGridViewComboBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.FeatureDurationDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FeatureBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.StreamGroupBox = new System.Windows.Forms.GroupBox();
            this.StreamDataGridView = new MeGUI.packages.tools.hdbdextractor.CustomDataGridView();
            this.StreamNumberTextBox = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StreamExtractCheckBox = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.StreamTypeTextBox = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StreamDescriptionTextBox = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StreamExtractAsComboBox = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.StreamAddOptionsTextBox = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.languageDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StreamsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.OutputGroupBox = new System.Windows.Forms.GroupBox();
            this.FolderOutputSourceButton = new System.Windows.Forms.Button();
            this.FolderOutputTextBox = new System.Windows.Forms.TextBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.extractTypesBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.closeOnQueue = new System.Windows.Forms.CheckBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripMenuItem();
            this.addPrefixBasedOnInputToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.StatusStrip.SuspendLayout();
            this.InputGroupBox.SuspendLayout();
            this.FeatureGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FeatureDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FeatureBindingSource)).BeginInit();
            this.StreamGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.StreamDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.StreamsBindingSource)).BeginInit();
            this.OutputGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.extractTypesBindingSource)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // StatusStrip
            // 
            this.StatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripStatusLabel,
            this.ToolStripProgressBar});
            this.StatusStrip.Location = new System.Drawing.Point(0, 433);
            this.StatusStrip.Name = "StatusStrip";
            this.StatusStrip.ShowItemToolTips = true;
            this.StatusStrip.Size = new System.Drawing.Size(759, 22);
            this.StatusStrip.TabIndex = 11;
            // 
            // ToolStripStatusLabel
            // 
            this.ToolStripStatusLabel.AutoSize = false;
            this.ToolStripStatusLabel.Name = "ToolStripStatusLabel";
            this.ToolStripStatusLabel.Size = new System.Drawing.Size(358, 17);
            this.ToolStripStatusLabel.Text = "Ready";
            this.ToolStripStatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ToolStripStatusLabel.ToolTipText = "Status";
            // 
            // ToolStripProgressBar
            // 
            this.ToolStripProgressBar.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.ToolStripProgressBar.Name = "ToolStripProgressBar";
            this.ToolStripProgressBar.Size = new System.Drawing.Size(200, 16);
            this.ToolStripProgressBar.ToolTipText = "Progress";
            // 
            // FolderInputTextBox
            // 
            this.FolderInputTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.FolderInputTextBox.Enabled = false;
            this.FolderInputTextBox.Location = new System.Drawing.Point(6, 48);
            this.FolderInputTextBox.Name = "FolderInputTextBox";
            this.FolderInputTextBox.Size = new System.Drawing.Size(321, 21);
            this.FolderInputTextBox.TabIndex = 0;
            // 
            // HelpButton2
            // 
            this.HelpButton2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.HelpButton2.AutoSize = true;
            this.HelpButton2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.HelpButton2.Location = new System.Drawing.Point(12, 402);
            this.HelpButton2.Name = "HelpButton2";
            this.HelpButton2.Size = new System.Drawing.Size(38, 23);
            this.HelpButton2.TabIndex = 8;
            this.HelpButton2.Text = "Help";
            this.HelpButton2.UseVisualStyleBackColor = true;
            this.HelpButton2.Click += new System.EventHandler(this.HelpButton2_Click);
            // 
            // QueueButton
            // 
            this.QueueButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.QueueButton.Location = new System.Drawing.Point(593, 402);
            this.QueueButton.Name = "QueueButton";
            this.QueueButton.Size = new System.Drawing.Size(75, 23);
            this.QueueButton.TabIndex = 4;
            this.QueueButton.Text = "Queue";
            this.QueueButton.UseVisualStyleBackColor = true;
            this.QueueButton.Click += new System.EventHandler(this.QueueButton_Click);
            // 
            // CancelButton2
            // 
            this.CancelButton2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CancelButton2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelButton2.Location = new System.Drawing.Point(674, 402);
            this.CancelButton2.Name = "CancelButton2";
            this.CancelButton2.Size = new System.Drawing.Size(75, 23);
            this.CancelButton2.TabIndex = 10;
            this.CancelButton2.Text = "Cancel";
            this.CancelButton2.UseVisualStyleBackColor = true;
            this.CancelButton2.Click += new System.EventHandler(this.CancelButton2_Click);
            // 
            // InputGroupBox
            // 
            this.InputGroupBox.Controls.Add(this.FileSelection);
            this.InputGroupBox.Controls.Add(this.FolderSelection);
            this.InputGroupBox.Controls.Add(this.FolderInputSourceButton);
            this.InputGroupBox.Controls.Add(this.FolderInputTextBox);
            this.InputGroupBox.Location = new System.Drawing.Point(10, 24);
            this.InputGroupBox.Name = "InputGroupBox";
            this.InputGroupBox.Size = new System.Drawing.Size(365, 74);
            this.InputGroupBox.TabIndex = 0;
            this.InputGroupBox.TabStop = false;
            this.InputGroupBox.Text = " Input ";
            // 
            // FileSelection
            // 
            this.FileSelection.AutoSize = true;
            this.FileSelection.Location = new System.Drawing.Point(164, 20);
            this.FileSelection.Name = "FileSelection";
            this.FileSelection.Size = new System.Drawing.Size(116, 17);
            this.FileSelection.TabIndex = 14;
            this.FileSelection.TabStop = true;
            this.FileSelection.Text = "Select File as Input";
            this.FileSelection.UseVisualStyleBackColor = true;
            // 
            // FolderSelection
            // 
            this.FolderSelection.AutoSize = true;
            this.FolderSelection.Checked = true;
            this.FolderSelection.Location = new System.Drawing.Point(6, 20);
            this.FolderSelection.Name = "FolderSelection";
            this.FolderSelection.Size = new System.Drawing.Size(130, 17);
            this.FolderSelection.TabIndex = 13;
            this.FolderSelection.TabStop = true;
            this.FolderSelection.Text = "Select Folder as Input";
            this.FolderSelection.UseVisualStyleBackColor = true;
            // 
            // FolderInputSourceButton
            // 
            this.FolderInputSourceButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.FolderInputSourceButton.Location = new System.Drawing.Point(330, 47);
            this.FolderInputSourceButton.Name = "FolderInputSourceButton";
            this.FolderInputSourceButton.Size = new System.Drawing.Size(29, 23);
            this.FolderInputSourceButton.TabIndex = 0;
            this.FolderInputSourceButton.Text = "...";
            this.FolderInputSourceButton.UseVisualStyleBackColor = true;
            this.FolderInputSourceButton.Click += new System.EventHandler(this.FolderInputSourceButton_Click);
            // 
            // Eac3toLinkLabel
            // 
            this.Eac3toLinkLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Eac3toLinkLabel.AutoSize = true;
            this.Eac3toLinkLabel.Location = new System.Drawing.Point(57, 407);
            this.Eac3toLinkLabel.Name = "Eac3toLinkLabel";
            this.Eac3toLinkLabel.Size = new System.Drawing.Size(40, 13);
            this.Eac3toLinkLabel.TabIndex = 13;
            this.Eac3toLinkLabel.TabStop = true;
            this.Eac3toLinkLabel.Text = "eac3to";
            this.Eac3toLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.Eac3toLinkLabel_LinkClicked);
            // 
            // FeatureGroupBox
            // 
            this.FeatureGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.FeatureGroupBox.Controls.Add(this.FeatureDataGridView);
            this.FeatureGroupBox.Location = new System.Drawing.Point(10, 104);
            this.FeatureGroupBox.Name = "FeatureGroupBox";
            this.FeatureGroupBox.Size = new System.Drawing.Size(737, 110);
            this.FeatureGroupBox.TabIndex = 2;
            this.FeatureGroupBox.TabStop = false;
            this.FeatureGroupBox.Text = " Feature(s) ";
            // 
            // FeatureDataGridView
            // 
            this.FeatureDataGridView.AllowUserToAddRows = false;
            this.FeatureDataGridView.AllowUserToDeleteRows = false;
            this.FeatureDataGridView.AllowUserToOrderColumns = true;
            this.FeatureDataGridView.AllowUserToResizeRows = false;
            this.FeatureDataGridView.AutoGenerateColumns = false;
            this.FeatureDataGridView.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.FeatureDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.FeatureDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.FeatureNumberDataGridViewTextBoxColumn1,
            this.FeatureNameDataGridViewTextBoxColumn,
            this.FeatureDescriptionDataGridViewTextBoxColumn,
            this.FeatureFileDataGridViewComboBoxColumn,
            this.FeatureDurationDataGridViewTextBoxColumn});
            this.FeatureDataGridView.DataSource = this.FeatureBindingSource;
            this.FeatureDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FeatureDataGridView.Location = new System.Drawing.Point(3, 17);
            this.FeatureDataGridView.MultiSelect = false;
            this.FeatureDataGridView.Name = "FeatureDataGridView";
            this.FeatureDataGridView.RowHeadersVisible = false;
            this.FeatureDataGridView.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.FeatureDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.FeatureDataGridView.ShowEditingIcon = false;
            this.FeatureDataGridView.Size = new System.Drawing.Size(731, 90);
            this.FeatureDataGridView.TabIndex = 13;
            this.FeatureDataGridView.DataSourceChanged += new System.EventHandler(this.FeatureDataGridView_DataSourceChanged);
            this.FeatureDataGridView.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.FeatureDataGridView_DataBindingComplete);
            // 
            // FeatureNumberDataGridViewTextBoxColumn1
            // 
            this.FeatureNumberDataGridViewTextBoxColumn1.DataPropertyName = "Number";
            this.FeatureNumberDataGridViewTextBoxColumn1.HeaderText = "#";
            this.FeatureNumberDataGridViewTextBoxColumn1.MinimumWidth = 2;
            this.FeatureNumberDataGridViewTextBoxColumn1.Name = "FeatureNumberDataGridViewTextBoxColumn1";
            this.FeatureNumberDataGridViewTextBoxColumn1.ReadOnly = true;
            this.FeatureNumberDataGridViewTextBoxColumn1.ToolTipText = "Feature number";
            this.FeatureNumberDataGridViewTextBoxColumn1.Width = 20;
            // 
            // FeatureNameDataGridViewTextBoxColumn
            // 
            this.FeatureNameDataGridViewTextBoxColumn.DataPropertyName = "Name";
            this.FeatureNameDataGridViewTextBoxColumn.HeaderText = "Name";
            this.FeatureNameDataGridViewTextBoxColumn.MinimumWidth = 2;
            this.FeatureNameDataGridViewTextBoxColumn.Name = "FeatureNameDataGridViewTextBoxColumn";
            this.FeatureNameDataGridViewTextBoxColumn.ReadOnly = true;
            this.FeatureNameDataGridViewTextBoxColumn.ToolTipText = "Feature name";
            this.FeatureNameDataGridViewTextBoxColumn.Width = 125;
            // 
            // FeatureDescriptionDataGridViewTextBoxColumn
            // 
            this.FeatureDescriptionDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.FeatureDescriptionDataGridViewTextBoxColumn.DataPropertyName = "Description";
            this.FeatureDescriptionDataGridViewTextBoxColumn.HeaderText = "Description";
            this.FeatureDescriptionDataGridViewTextBoxColumn.MinimumWidth = 2;
            this.FeatureDescriptionDataGridViewTextBoxColumn.Name = "FeatureDescriptionDataGridViewTextBoxColumn";
            this.FeatureDescriptionDataGridViewTextBoxColumn.ReadOnly = true;
            this.FeatureDescriptionDataGridViewTextBoxColumn.ToolTipText = "Feature description";
            // 
            // FeatureFileDataGridViewComboBoxColumn
            // 
            this.FeatureFileDataGridViewComboBoxColumn.HeaderText = "File(s)";
            this.FeatureFileDataGridViewComboBoxColumn.MinimumWidth = 2;
            this.FeatureFileDataGridViewComboBoxColumn.Name = "FeatureFileDataGridViewComboBoxColumn";
            this.FeatureFileDataGridViewComboBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.FeatureFileDataGridViewComboBoxColumn.ToolTipText = "Feature File(s)";
            this.FeatureFileDataGridViewComboBoxColumn.Width = 90;
            // 
            // FeatureDurationDataGridViewTextBoxColumn
            // 
            this.FeatureDurationDataGridViewTextBoxColumn.DataPropertyName = "Duration";
            this.FeatureDurationDataGridViewTextBoxColumn.HeaderText = "Duration";
            this.FeatureDurationDataGridViewTextBoxColumn.MinimumWidth = 2;
            this.FeatureDurationDataGridViewTextBoxColumn.Name = "FeatureDurationDataGridViewTextBoxColumn";
            this.FeatureDurationDataGridViewTextBoxColumn.ReadOnly = true;
            this.FeatureDurationDataGridViewTextBoxColumn.ToolTipText = "Feature duration";
            this.FeatureDurationDataGridViewTextBoxColumn.Width = 67;
            // 
            // FeatureBindingSource
            // 
            this.FeatureBindingSource.AllowNew = false;
            this.FeatureBindingSource.DataSource = typeof(eac3to.Feature);
            // 
            // StreamGroupBox
            // 
            this.StreamGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.StreamGroupBox.Controls.Add(this.StreamDataGridView);
            this.StreamGroupBox.Location = new System.Drawing.Point(10, 220);
            this.StreamGroupBox.Name = "StreamGroupBox";
            this.StreamGroupBox.Size = new System.Drawing.Size(737, 176);
            this.StreamGroupBox.TabIndex = 3;
            this.StreamGroupBox.TabStop = false;
            this.StreamGroupBox.Text = " Stream(s) ";
            // 
            // StreamDataGridView
            // 
            this.StreamDataGridView.AllowUserToAddRows = false;
            this.StreamDataGridView.AllowUserToDeleteRows = false;
            this.StreamDataGridView.AllowUserToOrderColumns = true;
            this.StreamDataGridView.AllowUserToResizeRows = false;
            this.StreamDataGridView.AutoGenerateColumns = false;
            this.StreamDataGridView.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.StreamDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.StreamDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.StreamNumberTextBox,
            this.StreamExtractCheckBox,
            this.StreamTypeTextBox,
            this.StreamDescriptionTextBox,
            this.StreamExtractAsComboBox,
            this.StreamAddOptionsTextBox,
            this.languageDataGridViewTextBoxColumn,
            this.nameDataGridViewTextBoxColumn});
            this.StreamDataGridView.DataSource = this.StreamsBindingSource;
            this.StreamDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.StreamDataGridView.Location = new System.Drawing.Point(3, 17);
            this.StreamDataGridView.MultiSelect = false;
            this.StreamDataGridView.Name = "StreamDataGridView";
            this.StreamDataGridView.RowHeadersVisible = false;
            this.StreamDataGridView.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.StreamDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.StreamDataGridView.ShowEditingIcon = false;
            this.StreamDataGridView.Size = new System.Drawing.Size(731, 156);
            this.StreamDataGridView.TabIndex = 7;
            this.StreamDataGridView.DataSourceChanged += new System.EventHandler(this.StreamDataGridView_DataSourceChanged);
            // 
            // StreamNumberTextBox
            // 
            this.StreamNumberTextBox.DataPropertyName = "Number";
            this.StreamNumberTextBox.HeaderText = "#";
            this.StreamNumberTextBox.MinimumWidth = 2;
            this.StreamNumberTextBox.Name = "StreamNumberTextBox";
            this.StreamNumberTextBox.ReadOnly = true;
            this.StreamNumberTextBox.ToolTipText = "Stream number";
            this.StreamNumberTextBox.Width = 20;
            // 
            // StreamExtractCheckBox
            // 
            this.StreamExtractCheckBox.FalseValue = "0";
            this.StreamExtractCheckBox.HeaderText = "Extract?";
            this.StreamExtractCheckBox.IndeterminateValue = "-1";
            this.StreamExtractCheckBox.MinimumWidth = 2;
            this.StreamExtractCheckBox.Name = "StreamExtractCheckBox";
            this.StreamExtractCheckBox.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.StreamExtractCheckBox.ToolTipText = "Extract stream?";
            this.StreamExtractCheckBox.TrueValue = "1";
            this.StreamExtractCheckBox.Width = 49;
            // 
            // StreamTypeTextBox
            // 
            this.StreamTypeTextBox.DataPropertyName = "Type";
            this.StreamTypeTextBox.HeaderText = "Type";
            this.StreamTypeTextBox.MinimumWidth = 2;
            this.StreamTypeTextBox.Name = "StreamTypeTextBox";
            this.StreamTypeTextBox.ReadOnly = true;
            this.StreamTypeTextBox.ToolTipText = "Stream type";
            this.StreamTypeTextBox.Width = 47;
            // 
            // StreamDescriptionTextBox
            // 
            this.StreamDescriptionTextBox.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.StreamDescriptionTextBox.DataPropertyName = "Description";
            this.StreamDescriptionTextBox.HeaderText = "Description";
            this.StreamDescriptionTextBox.MinimumWidth = 2;
            this.StreamDescriptionTextBox.Name = "StreamDescriptionTextBox";
            this.StreamDescriptionTextBox.ReadOnly = true;
            this.StreamDescriptionTextBox.ToolTipText = "Stream description";
            // 
            // StreamExtractAsComboBox
            // 
            this.StreamExtractAsComboBox.HeaderText = "Extract As";
            this.StreamExtractAsComboBox.MinimumWidth = 2;
            this.StreamExtractAsComboBox.Name = "StreamExtractAsComboBox";
            this.StreamExtractAsComboBox.ToolTipText = "Stream extract type";
            this.StreamExtractAsComboBox.Width = 80;
            // 
            // StreamAddOptionsTextBox
            // 
            this.StreamAddOptionsTextBox.HeaderText = "+ Options";
            this.StreamAddOptionsTextBox.MinimumWidth = 2;
            this.StreamAddOptionsTextBox.Name = "StreamAddOptionsTextBox";
            this.StreamAddOptionsTextBox.ToolTipText = "Stream extract additional options";
            this.StreamAddOptionsTextBox.Width = 176;
            // 
            // languageDataGridViewTextBoxColumn
            // 
            this.languageDataGridViewTextBoxColumn.DataPropertyName = "Language";
            this.languageDataGridViewTextBoxColumn.HeaderText = "Language";
            this.languageDataGridViewTextBoxColumn.MinimumWidth = 2;
            this.languageDataGridViewTextBoxColumn.Name = "languageDataGridViewTextBoxColumn";
            this.languageDataGridViewTextBoxColumn.ToolTipText = "Stream language";
            this.languageDataGridViewTextBoxColumn.Width = 80;
            // 
            // nameDataGridViewTextBoxColumn
            // 
            this.nameDataGridViewTextBoxColumn.DataPropertyName = "Name";
            this.nameDataGridViewTextBoxColumn.HeaderText = "Name";
            this.nameDataGridViewTextBoxColumn.MinimumWidth = 2;
            this.nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
            this.nameDataGridViewTextBoxColumn.ToolTipText = "Stream name";
            // 
            // StreamsBindingSource
            // 
            this.StreamsBindingSource.DataSource = typeof(eac3to.Stream);
            // 
            // OutputGroupBox
            // 
            this.OutputGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.OutputGroupBox.Controls.Add(this.FolderOutputSourceButton);
            this.OutputGroupBox.Controls.Add(this.FolderOutputTextBox);
            this.OutputGroupBox.Location = new System.Drawing.Point(381, 24);
            this.OutputGroupBox.Name = "OutputGroupBox";
            this.OutputGroupBox.Size = new System.Drawing.Size(366, 74);
            this.OutputGroupBox.TabIndex = 1;
            this.OutputGroupBox.TabStop = false;
            this.OutputGroupBox.Text = " Output ";
            // 
            // FolderOutputSourceButton
            // 
            this.FolderOutputSourceButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.FolderOutputSourceButton.Location = new System.Drawing.Point(331, 47);
            this.FolderOutputSourceButton.Name = "FolderOutputSourceButton";
            this.FolderOutputSourceButton.Size = new System.Drawing.Size(29, 23);
            this.FolderOutputSourceButton.TabIndex = 2;
            this.FolderOutputSourceButton.Text = "...";
            this.FolderOutputSourceButton.UseVisualStyleBackColor = true;
            this.FolderOutputSourceButton.Click += new System.EventHandler(this.FolderOutputSourceButton_Click);
            // 
            // FolderOutputTextBox
            // 
            this.FolderOutputTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.FolderOutputTextBox.Location = new System.Drawing.Point(6, 48);
            this.FolderOutputTextBox.Name = "FolderOutputTextBox";
            this.FolderOutputTextBox.Size = new System.Drawing.Size(322, 21);
            this.FolderOutputTextBox.TabIndex = 1;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.Filter = resources.GetString("openFileDialog1.Filter");
            this.openFileDialog1.FilterIndex = 5;
            this.openFileDialog1.Multiselect = true;
            // 
            // extractTypesBindingSource
            // 
            this.extractTypesBindingSource.DataMember = "ExtractTypes";
            this.extractTypesBindingSource.DataSource = this.StreamsBindingSource;
            // 
            // closeOnQueue
            // 
            this.closeOnQueue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.closeOnQueue.Checked = true;
            this.closeOnQueue.CheckState = System.Windows.Forms.CheckState.Checked;
            this.closeOnQueue.Location = new System.Drawing.Point(514, 402);
            this.closeOnQueue.Name = "closeOnQueue";
            this.closeOnQueue.Size = new System.Drawing.Size(74, 24);
            this.closeOnQueue.TabIndex = 19;
            this.closeOnQueue.Text = "and close";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.menuStrip1.Size = new System.Drawing.Size(759, 24);
            this.menuStrip1.TabIndex = 20;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem2,
            this.toolStripMenuItem3,
            this.toolStripMenuItem4,
            this.toolStripMenuItem5,
            this.toolStripMenuItem6,
            this.addPrefixBasedOnInputToolStripMenuItem});
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(61, 20);
            this.toolStripMenuItem1.Text = "&Settings";
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Checked = true;
            this.toolStripMenuItem2.CheckOnClick = true;
            this.toolStripMenuItem2.CheckState = System.Windows.Forms.CheckState.Checked;
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(311, 22);
            this.toolStripMenuItem2.Text = "Preselect tracks based on default language(s)";
            this.toolStripMenuItem2.Click += new System.EventHandler(this.toolStripMenuItem2_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.CheckOnClick = true;
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(311, 22);
            this.toolStripMenuItem3.Text = "Extract As: default to HD option";
            this.toolStripMenuItem3.Click += new System.EventHandler(this.toolStripMenuItem3_Click);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.CheckOnClick = true;
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(311, 22);
            this.toolStripMenuItem4.Text = "Extract As: show encoding option(s)";
            this.toolStripMenuItem4.Click += new System.EventHandler(this.toolStripMenuItem4_Click);
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.CheckOnClick = true;
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(311, 22);
            this.toolStripMenuItem5.Text = "Extract As: show all demux options";
            this.toolStripMenuItem5.Click += new System.EventHandler(this.toolStripMenuItem5_Click);
            // 
            // toolStripMenuItem6
            // 
            this.toolStripMenuItem6.CheckOnClick = true;
            this.toolStripMenuItem6.Name = "toolStripMenuItem6";
            this.toolStripMenuItem6.Size = new System.Drawing.Size(311, 22);
            this.toolStripMenuItem6.Text = "Enable Custom Options";
            this.toolStripMenuItem6.Click += new System.EventHandler(this.toolStripMenuItem6_Click);
            // 
            // addPrefixBasedOnInputToolStripMenuItem
            // 
            this.addPrefixBasedOnInputToolStripMenuItem.Checked = true;
            this.addPrefixBasedOnInputToolStripMenuItem.CheckOnClick = true;
            this.addPrefixBasedOnInputToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.addPrefixBasedOnInputToolStripMenuItem.Name = "addPrefixBasedOnInputToolStripMenuItem";
            this.addPrefixBasedOnInputToolStripMenuItem.Size = new System.Drawing.Size(311, 22);
            this.addPrefixBasedOnInputToolStripMenuItem.Text = "Add prefix to output file(s) based on input";
            this.addPrefixBasedOnInputToolStripMenuItem.Click += new System.EventHandler(this.addPrefixBasedOnInputToolStripMenuItem_Click);
            // 
            // HdBdStreamExtractor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(759, 455);
            this.Controls.Add(this.closeOnQueue);
            this.Controls.Add(this.OutputGroupBox);
            this.Controls.Add(this.StreamGroupBox);
            this.Controls.Add(this.FeatureGroupBox);
            this.Controls.Add(this.Eac3toLinkLabel);
            this.Controls.Add(this.InputGroupBox);
            this.Controls.Add(this.CancelButton2);
            this.Controls.Add(this.QueueButton);
            this.Controls.Add(this.HelpButton2);
            this.Controls.Add(this.StatusStrip);
            this.Controls.Add(this.menuStrip1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(775, 494);
            this.Name = "HdBdStreamExtractor";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "MeGUI - HD-DVD/Blu-ray Streams Extractor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.HdBrStreamExtractor_FormClosing);
            this.StatusStrip.ResumeLayout(false);
            this.StatusStrip.PerformLayout();
            this.InputGroupBox.ResumeLayout(false);
            this.InputGroupBox.PerformLayout();
            this.FeatureGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.FeatureDataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FeatureBindingSource)).EndInit();
            this.StreamGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.StreamDataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.StreamsBindingSource)).EndInit();
            this.OutputGroupBox.ResumeLayout(false);
            this.OutputGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.extractTypesBindingSource)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.GroupBox InputGroupBox;
        private System.Windows.Forms.TextBox FolderInputTextBox;
        private System.Windows.Forms.Button FolderInputSourceButton;
        private System.Windows.Forms.BindingSource FeatureBindingSource;
        private System.Windows.Forms.GroupBox FeatureGroupBox;
        private System.Windows.Forms.BindingSource StreamsBindingSource;
        private System.Windows.Forms.GroupBox StreamGroupBox;
        private MeGUI.packages.tools.hdbdextractor.CustomDataGridView StreamDataGridView;
        private System.Windows.Forms.Button HelpButton2;
        private System.Windows.Forms.LinkLabel Eac3toLinkLabel;
        private System.Windows.Forms.Button QueueButton;
        private System.Windows.Forms.Button CancelButton2;
        private System.Windows.Forms.StatusStrip StatusStrip;
        private System.Windows.Forms.ToolStripStatusLabel ToolStripStatusLabel;
        private System.Windows.Forms.ToolStripProgressBar ToolStripProgressBar;
        private MeGUI.packages.tools.hdbdextractor.CustomDataGridView FeatureDataGridView;
        private System.Windows.Forms.GroupBox OutputGroupBox;
        private System.Windows.Forms.Button FolderOutputSourceButton;
        private System.Windows.Forms.TextBox FolderOutputTextBox;
        private System.Windows.Forms.RadioButton FolderSelection;
        private System.Windows.Forms.RadioButton FileSelection;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.BindingSource extractTypesBindingSource;
        private System.Windows.Forms.CheckBox closeOnQueue;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem5;
        private System.Windows.Forms.DataGridViewTextBoxColumn FeatureNumberDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn FeatureNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn FeatureDescriptionDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn FeatureFileDataGridViewComboBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn FeatureDurationDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn StreamNumberTextBox;
        private System.Windows.Forms.DataGridViewCheckBoxColumn StreamExtractCheckBox;
        private System.Windows.Forms.DataGridViewTextBoxColumn StreamTypeTextBox;
        private System.Windows.Forms.DataGridViewTextBoxColumn StreamDescriptionTextBox;
        private System.Windows.Forms.DataGridViewComboBoxColumn StreamExtractAsComboBox;
        private System.Windows.Forms.DataGridViewTextBoxColumn StreamAddOptionsTextBox;
        private System.Windows.Forms.DataGridViewTextBoxColumn languageDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem6;
        private System.Windows.Forms.ToolStripMenuItem addPrefixBasedOnInputToolStripMenuItem;
    }
}