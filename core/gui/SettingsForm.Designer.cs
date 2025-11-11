// ****************************************************************************
// 
// Copyright (C) 2005-2025 Doom9 & al
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
    partial class SettingsForm
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
            System.Windows.Forms.GroupBox groupBox1;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsForm));
            this.rbCloseMeGUI = new System.Windows.Forms.RadioButton();
            this.command = new System.Windows.Forms.TextBox();
            this.runCommand = new System.Windows.Forms.RadioButton();
            this.shutdown = new System.Windows.Forms.RadioButton();
            this.donothing = new System.Windows.Forms.RadioButton();
            this.saveButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.otherGroupBox = new System.Windows.Forms.GroupBox();
            this.chkDebugInformation = new System.Windows.Forms.CheckBox();
            this.cbUseITUValues = new System.Windows.Forms.CheckBox();
            this.openProgressWindow = new System.Windows.Forms.CheckBox();
            this.deleteIntermediateFiles = new System.Windows.Forms.CheckBox();
            this.deleteAbortedOutput = new System.Windows.Forms.CheckBox();
            this.openScript = new System.Windows.Forms.CheckBox();
            this.openExecutableDialog = new System.Windows.Forms.OpenFileDialog();
            this.openFolderDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.groupBox12 = new System.Windows.Forms.GroupBox();
            this.lblMinimumLength = new System.Windows.Forms.Label();
            this.minimumTitleLength = new System.Windows.Forms.NumericUpDown();
            this.acceptableFPSError = new System.Windows.Forms.NumericUpDown();
            this.label15 = new System.Windows.Forms.Label();
            this.resetDialogs = new System.Windows.Forms.Button();
            this.configSourceDetector = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.defaultLanguage2 = new System.Windows.Forms.ComboBox();
            this.defaultLanguage1 = new System.Windows.Forms.ComboBox();
            this.gbDefaultOutput = new System.Windows.Forms.GroupBox();
            this.targetSizeSCBox1 = new MeGUI.core.gui.TargetSizeSCBox();
            this.btnClearOutputDirecoty = new System.Windows.Forms.Button();
            this.clearDefaultOutputDir = new System.Windows.Forms.Button();
            this.defaultOutputDir = new MeGUI.FileBar();
            this.groupBox13 = new System.Windows.Forms.GroupBox();
            this.cbHwdDecoder = new System.Windows.Forms.ComboBox();
            this.chkEnableHwd = new System.Windows.Forms.CheckBox();
            this.chkInput8Bit = new System.Windows.Forms.CheckBox();
            this.lblffmsThreads = new System.Windows.Forms.Label();
            this.ffmsThreads = new System.Windows.Forms.NumericUpDown();
            this.cbUseIncludedAviSynth = new System.Windows.Forms.CheckBox();
            this.chkDirectShowSource = new System.Windows.Forms.CheckBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cbHttpProxyMode = new System.Windows.Forms.ComboBox();
            this.txt_httpproxyport = new System.Windows.Forms.TextBox();
            this.txt_httpproxypwd = new System.Windows.Forms.TextBox();
            this.txt_httpproxyuid = new System.Windows.Forms.TextBox();
            this.txt_httpproxyaddress = new System.Windows.Forms.TextBox();
            this.label21 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.gbVideoPreview = new System.Windows.Forms.GroupBox();
            this.chkEnsureCorrectPlaybackSpeed = new System.Windows.Forms.CheckBox();
            this.cbAddTimePos = new System.Windows.Forms.CheckBox();
            this.chAlwaysOnTop = new System.Windows.Forms.CheckBox();
            this.autoUpdateGroupBox = new System.Windows.Forms.GroupBox();
            this.cbAutoUpdateServerSubList = new System.Windows.Forms.ComboBox();
            this.backupfiles = new System.Windows.Forms.CheckBox();
            this.configureServersButton = new System.Windows.Forms.Button();
            this.useAutoUpdateCheckbox = new System.Windows.Forms.CheckBox();
            this.outputExtensions = new System.Windows.Forms.GroupBox();
            this.lblForcedName = new System.Windows.Forms.Label();
            this.txtForcedName = new System.Windows.Forms.TextBox();
            this.videoExtension = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.audioExtension = new System.Windows.Forms.TextBox();
            this.autoModeGroupbox = new System.Windows.Forms.GroupBox();
            this.configAutoEncodeDefaults = new System.Windows.Forms.Button();
            this.keep2ndPassLogFile = new System.Windows.Forms.CheckBox();
            this.keep2ndPassOutput = new System.Windows.Forms.CheckBox();
            this.label13 = new System.Windows.Forms.Label();
            this.nbPasses = new System.Windows.Forms.NumericUpDown();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.groupBox11 = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.cbStandbySettings = new System.Windows.Forms.ComboBox();
            this.cbRemoveJob = new System.Windows.Forms.CheckBox();
            this.cbAutoStart = new System.Windows.Forms.CheckBox();
            this.cbAutoStartOnStartup = new System.Windows.Forms.CheckBox();
            this.workerMaximumCount = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox10 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cbIOPriority = new System.Windows.Forms.ComboBox();
            this.cbProcessPriority = new System.Windows.Forms.ComboBox();
            this.cbJobType = new System.Windows.Forms.ComboBox();
            this.groupBox9 = new System.Windows.Forms.GroupBox();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.btnAddSettings = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.workerJobsListBox = new System.Windows.Forms.CheckedListBox();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.btnDeleteSettings = new System.Windows.Forms.Button();
            this.btnResetSettings = new System.Windows.Forms.Button();
            this.workerSettingsListBox = new System.Windows.Forms.ListBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox14 = new System.Windows.Forms.GroupBox();
            this.lblQaac = new System.Windows.Forms.Label();
            this.qaacLocation = new MeGUI.FileBar();
            this.useQAAC = new System.Windows.Forms.CheckBox();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.useFDKAac = new System.Windows.Forms.CheckBox();
            this.lblFDK = new System.Windows.Forms.Label();
            this.fdkaacLocation = new MeGUI.FileBar();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.chk64Bit = new System.Windows.Forms.CheckBox();
            this.chx264ExternalMuxer = new System.Windows.Forms.CheckBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.useNeroAacEnc = new System.Windows.Forms.CheckBox();
            this.lblNero = new System.Windows.Forms.Label();
            this.neroaacencLocation = new MeGUI.FileBar();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.btnClearMP4TempDirectory = new System.Windows.Forms.Button();
            this.tempDirMP4 = new MeGUI.FileBar();
            this.vobGroupBox = new System.Windows.Forms.GroupBox();
            this.useDGIndexIM = new System.Windows.Forms.CheckBox();
            this.useDGIndexNV = new System.Windows.Forms.CheckBox();
            this.cbAutoLoadDG = new System.Windows.Forms.CheckBox();
            this.forceFilmPercentage = new System.Windows.Forms.NumericUpDown();
            this.autoForceFilm = new System.Windows.Forms.CheckBox();
            this.audioExtLabel = new System.Windows.Forms.Label();
            this.videoExtLabel = new System.Windows.Forms.Label();
            this.autoEncodeDefaultsButton = new System.Windows.Forms.Button();
            this.toolTipHelp = new System.Windows.Forms.ToolTip(this.components);
            this.helpButton1 = new MeGUI.core.gui.HelpButton();
            groupBox1 = new System.Windows.Forms.GroupBox();
            groupBox1.SuspendLayout();
            this.otherGroupBox.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox12.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.minimumTitleLength)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.acceptableFPSError)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.gbDefaultOutput.SuspendLayout();
            this.groupBox13.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ffmsThreads)).BeginInit();
            this.tabPage3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.gbVideoPreview.SuspendLayout();
            this.autoUpdateGroupBox.SuspendLayout();
            this.outputExtensions.SuspendLayout();
            this.autoModeGroupbox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nbPasses)).BeginInit();
            this.tabPage4.SuspendLayout();
            this.groupBox11.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.workerMaximumCount)).BeginInit();
            this.groupBox10.SuspendLayout();
            this.groupBox9.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.groupBox8.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox14.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.vobGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.forceFilmPercentage)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(this.rbCloseMeGUI);
            groupBox1.Controls.Add(this.command);
            groupBox1.Controls.Add(this.runCommand);
            groupBox1.Controls.Add(this.shutdown);
            groupBox1.Controls.Add(this.donothing);
            groupBox1.Location = new System.Drawing.Point(4, 241);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new System.Drawing.Size(263, 95);
            groupBox1.TabIndex = 2;
            groupBox1.TabStop = false;
            groupBox1.Text = " After encoding ";
            // 
            // rbCloseMeGUI
            // 
            this.rbCloseMeGUI.AutoSize = true;
            this.rbCloseMeGUI.Location = new System.Drawing.Point(134, 43);
            this.rbCloseMeGUI.Name = "rbCloseMeGUI";
            this.rbCloseMeGUI.Size = new System.Drawing.Size(84, 17);
            this.rbCloseMeGUI.TabIndex = 4;
            this.rbCloseMeGUI.TabStop = true;
            this.rbCloseMeGUI.Text = "close MeGUI";
            this.rbCloseMeGUI.UseVisualStyleBackColor = true;
            // 
            // command
            // 
            this.command.Enabled = false;
            this.command.Location = new System.Drawing.Point(10, 64);
            this.command.Name = "command";
            this.command.Size = new System.Drawing.Size(247, 21);
            this.command.TabIndex = 3;
            // 
            // runCommand
            // 
            this.runCommand.AutoSize = true;
            this.runCommand.Location = new System.Drawing.Point(11, 43);
            this.runCommand.Name = "runCommand";
            this.runCommand.Size = new System.Drawing.Size(96, 17);
            this.runCommand.TabIndex = 2;
            this.runCommand.Text = "Run command:";
            this.runCommand.UseVisualStyleBackColor = true;
            this.runCommand.CheckedChanged += new System.EventHandler(this.runCommand_CheckedChanged);
            // 
            // shutdown
            // 
            this.shutdown.AutoSize = true;
            this.shutdown.Location = new System.Drawing.Point(134, 20);
            this.shutdown.Name = "shutdown";
            this.shutdown.Size = new System.Drawing.Size(73, 17);
            this.shutdown.TabIndex = 1;
            this.shutdown.Text = "Shutdown";
            this.shutdown.UseVisualStyleBackColor = true;
            // 
            // donothing
            // 
            this.donothing.AutoSize = true;
            this.donothing.Checked = true;
            this.donothing.Location = new System.Drawing.Point(11, 20);
            this.donothing.Name = "donothing";
            this.donothing.Size = new System.Drawing.Size(77, 17);
            this.donothing.TabIndex = 0;
            this.donothing.TabStop = true;
            this.donothing.Text = "Do nothing";
            this.donothing.UseVisualStyleBackColor = true;
            // 
            // saveButton
            // 
            this.saveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.saveButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.saveButton.Location = new System.Drawing.Point(473, 468);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(48, 23);
            this.saveButton.TabIndex = 2;
            this.saveButton.Text = "Save";
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(544, 468);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(48, 23);
            this.cancelButton.TabIndex = 3;
            this.cancelButton.Text = "Cancel";
            // 
            // otherGroupBox
            // 
            this.otherGroupBox.Controls.Add(this.chkDebugInformation);
            this.otherGroupBox.Controls.Add(this.cbUseITUValues);
            this.otherGroupBox.Controls.Add(this.openProgressWindow);
            this.otherGroupBox.Controls.Add(this.deleteIntermediateFiles);
            this.otherGroupBox.Controls.Add(this.deleteAbortedOutput);
            this.otherGroupBox.Controls.Add(this.openScript);
            this.otherGroupBox.Location = new System.Drawing.Point(2, 6);
            this.otherGroupBox.Name = "otherGroupBox";
            this.otherGroupBox.Size = new System.Drawing.Size(262, 167);
            this.otherGroupBox.TabIndex = 1;
            this.otherGroupBox.TabStop = false;
            this.otherGroupBox.Tag = "";
            this.otherGroupBox.Text = " Main Settings ";
            // 
            // chkDebugInformation
            // 
            this.chkDebugInformation.AutoSize = true;
            this.chkDebugInformation.Checked = true;
            this.chkDebugInformation.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkDebugInformation.Location = new System.Drawing.Point(13, 134);
            this.chkDebugInformation.Name = "chkDebugInformation";
            this.chkDebugInformation.Size = new System.Drawing.Size(133, 17);
            this.chkDebugInformation.TabIndex = 25;
            this.chkDebugInformation.Text = "Log debug information";
            // 
            // cbUseITUValues
            // 
            this.cbUseITUValues.Checked = true;
            this.cbUseITUValues.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbUseITUValues.Location = new System.Drawing.Point(13, 66);
            this.cbUseITUValues.Name = "cbUseITUValues";
            this.cbUseITUValues.Size = new System.Drawing.Size(144, 17);
            this.cbUseITUValues.TabIndex = 20;
            this.cbUseITUValues.Text = "Use ITU Aspect Ratio";
            // 
            // openProgressWindow
            // 
            this.openProgressWindow.Checked = true;
            this.openProgressWindow.CheckState = System.Windows.Forms.CheckState.Checked;
            this.openProgressWindow.Location = new System.Drawing.Point(13, 20);
            this.openProgressWindow.Name = "openProgressWindow";
            this.openProgressWindow.Size = new System.Drawing.Size(144, 17);
            this.openProgressWindow.TabIndex = 15;
            this.openProgressWindow.Text = "Show progress window";
            // 
            // deleteIntermediateFiles
            // 
            this.deleteIntermediateFiles.Checked = true;
            this.deleteIntermediateFiles.CheckState = System.Windows.Forms.CheckState.Checked;
            this.deleteIntermediateFiles.Location = new System.Drawing.Point(13, 89);
            this.deleteIntermediateFiles.Name = "deleteIntermediateFiles";
            this.deleteIntermediateFiles.Size = new System.Drawing.Size(152, 17);
            this.deleteIntermediateFiles.TabIndex = 13;
            this.deleteIntermediateFiles.Text = "Delete intermediate files";
            // 
            // deleteAbortedOutput
            // 
            this.deleteAbortedOutput.Checked = true;
            this.deleteAbortedOutput.CheckState = System.Windows.Forms.CheckState.Checked;
            this.deleteAbortedOutput.Location = new System.Drawing.Point(13, 111);
            this.deleteAbortedOutput.Name = "deleteAbortedOutput";
            this.deleteAbortedOutput.Size = new System.Drawing.Size(184, 17);
            this.deleteAbortedOutput.TabIndex = 12;
            this.deleteAbortedOutput.Text = "Delete output of aborted jobs";
            // 
            // openScript
            // 
            this.openScript.Checked = true;
            this.openScript.CheckState = System.Windows.Forms.CheckState.Checked;
            this.openScript.Location = new System.Drawing.Point(13, 43);
            this.openScript.Name = "openScript";
            this.openScript.Size = new System.Drawing.Size(277, 17);
            this.openScript.TabIndex = 10;
            this.openScript.Text = "Automatically open video preview";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(597, 461);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox12);
            this.tabPage1.Controls.Add(this.groupBox3);
            this.tabPage1.Controls.Add(this.gbDefaultOutput);
            this.tabPage1.Controls.Add(this.groupBox13);
            this.tabPage1.Controls.Add(this.otherGroupBox);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(589, 435);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Main";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // groupBox12
            // 
            this.groupBox12.Controls.Add(this.lblMinimumLength);
            this.groupBox12.Controls.Add(this.minimumTitleLength);
            this.groupBox12.Controls.Add(this.acceptableFPSError);
            this.groupBox12.Controls.Add(this.label15);
            this.groupBox12.Controls.Add(this.resetDialogs);
            this.groupBox12.Controls.Add(this.configSourceDetector);
            this.groupBox12.Location = new System.Drawing.Point(3, 277);
            this.groupBox12.Name = "groupBox12";
            this.groupBox12.Size = new System.Drawing.Size(579, 90);
            this.groupBox12.TabIndex = 13;
            this.groupBox12.TabStop = false;
            // 
            // lblMinimumLength
            // 
            this.lblMinimumLength.AutoSize = true;
            this.lblMinimumLength.Location = new System.Drawing.Point(9, 48);
            this.lblMinimumLength.Name = "lblMinimumLength";
            this.lblMinimumLength.Size = new System.Drawing.Size(255, 13);
            this.lblMinimumLength.TabIndex = 34;
            this.lblMinimumLength.Text = "Title Selector:  show only if longer than (in seconds)";
            // 
            // minimumTitleLength
            // 
            this.minimumTitleLength.Location = new System.Drawing.Point(270, 46);
            this.minimumTitleLength.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.minimumTitleLength.Name = "minimumTitleLength";
            this.minimumTitleLength.Size = new System.Drawing.Size(65, 21);
            this.minimumTitleLength.TabIndex = 33;
            this.minimumTitleLength.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.minimumTitleLength.Value = new decimal(new int[] {
            900,
            0,
            0,
            0});
            // 
            // acceptableFPSError
            // 
            this.acceptableFPSError.DecimalPlaces = 3;
            this.acceptableFPSError.Increment = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            this.acceptableFPSError.Location = new System.Drawing.Point(270, 15);
            this.acceptableFPSError.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.acceptableFPSError.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            this.acceptableFPSError.Name = "acceptableFPSError";
            this.acceptableFPSError.Size = new System.Drawing.Size(65, 21);
            this.acceptableFPSError.TabIndex = 30;
            this.acceptableFPSError.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.acceptableFPSError.Value = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.BackColor = System.Drawing.Color.Transparent;
            this.label15.Location = new System.Drawing.Point(9, 17);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(245, 13);
            this.label15.TabIndex = 29;
            this.label15.Text = "Acceptable FPS rounding error (bitrate calculator)";
            // 
            // resetDialogs
            // 
            this.resetDialogs.Location = new System.Drawing.Point(396, 17);
            this.resetDialogs.Name = "resetDialogs";
            this.resetDialogs.Size = new System.Drawing.Size(154, 21);
            this.resetDialogs.TabIndex = 31;
            this.resetDialogs.Text = "Reset All Dialogs";
            this.resetDialogs.UseVisualStyleBackColor = true;
            // 
            // configSourceDetector
            // 
            this.configSourceDetector.Location = new System.Drawing.Point(396, 48);
            this.configSourceDetector.Name = "configSourceDetector";
            this.configSourceDetector.Size = new System.Drawing.Size(154, 21);
            this.configSourceDetector.TabIndex = 32;
            this.configSourceDetector.Text = "Configure Source Detector";
            this.configSourceDetector.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.defaultLanguage2);
            this.groupBox3.Controls.Add(this.defaultLanguage1);
            this.groupBox3.Location = new System.Drawing.Point(3, 179);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(200, 92);
            this.groupBox3.TabIndex = 12;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = " Default Languages ";
            // 
            // defaultLanguage2
            // 
            this.defaultLanguage2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.defaultLanguage2.Location = new System.Drawing.Point(12, 56);
            this.defaultLanguage2.Name = "defaultLanguage2";
            this.defaultLanguage2.Size = new System.Drawing.Size(182, 21);
            this.defaultLanguage2.Sorted = true;
            this.defaultLanguage2.TabIndex = 7;
            // 
            // defaultLanguage1
            // 
            this.defaultLanguage1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.defaultLanguage1.Location = new System.Drawing.Point(13, 29);
            this.defaultLanguage1.Name = "defaultLanguage1";
            this.defaultLanguage1.Size = new System.Drawing.Size(181, 21);
            this.defaultLanguage1.Sorted = true;
            this.defaultLanguage1.TabIndex = 2;
            // 
            // gbDefaultOutput
            // 
            this.gbDefaultOutput.Controls.Add(this.targetSizeSCBox1);
            this.gbDefaultOutput.Controls.Add(this.btnClearOutputDirecoty);
            this.gbDefaultOutput.Controls.Add(this.clearDefaultOutputDir);
            this.gbDefaultOutput.Controls.Add(this.defaultOutputDir);
            this.gbDefaultOutput.Location = new System.Drawing.Point(209, 179);
            this.gbDefaultOutput.Name = "gbDefaultOutput";
            this.gbDefaultOutput.Size = new System.Drawing.Size(373, 92);
            this.gbDefaultOutput.TabIndex = 11;
            this.gbDefaultOutput.TabStop = false;
            this.gbDefaultOutput.Text = " Default Output Directory + Custom File Size Values ";
            // 
            // targetSizeSCBox1
            // 
            this.targetSizeSCBox1.CustomSizes = new MeGUI.core.util.FileSize[0];
            this.targetSizeSCBox1.Location = new System.Drawing.Point(8, 56);
            this.targetSizeSCBox1.Margin = new System.Windows.Forms.Padding(4);
            this.targetSizeSCBox1.MaximumSize = new System.Drawing.Size(1000, 28);
            this.targetSizeSCBox1.MinimumSize = new System.Drawing.Size(64, 28);
            this.targetSizeSCBox1.Name = "targetSizeSCBox1";
            this.targetSizeSCBox1.NullString = "Modify custom file size values";
            this.targetSizeSCBox1.SaveCustomValues = true;
            this.targetSizeSCBox1.SelectedIndex = 0;
            this.targetSizeSCBox1.Size = new System.Drawing.Size(329, 28);
            this.targetSizeSCBox1.TabIndex = 44;
            // 
            // btnClearOutputDirecoty
            // 
            this.btnClearOutputDirecoty.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnClearOutputDirecoty.Location = new System.Drawing.Point(342, 27);
            this.btnClearOutputDirecoty.Name = "btnClearOutputDirecoty";
            this.btnClearOutputDirecoty.Size = new System.Drawing.Size(25, 23);
            this.btnClearOutputDirecoty.TabIndex = 43;
            this.btnClearOutputDirecoty.Text = "x";
            // 
            // clearDefaultOutputDir
            // 
            this.clearDefaultOutputDir.Location = new System.Drawing.Point(430, 29);
            this.clearDefaultOutputDir.Name = "clearDefaultOutputDir";
            this.clearDefaultOutputDir.Size = new System.Drawing.Size(24, 26);
            this.clearDefaultOutputDir.TabIndex = 41;
            this.clearDefaultOutputDir.Text = "x";
            // 
            // defaultOutputDir
            // 
            this.defaultOutputDir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.defaultOutputDir.Filename = "";
            this.defaultOutputDir.FolderMode = true;
            this.defaultOutputDir.Location = new System.Drawing.Point(8, 27);
            this.defaultOutputDir.Margin = new System.Windows.Forms.Padding(4);
            this.defaultOutputDir.Name = "defaultOutputDir";
            this.defaultOutputDir.Size = new System.Drawing.Size(329, 23);
            this.defaultOutputDir.TabIndex = 40;
            // 
            // groupBox13
            // 
            this.groupBox13.Controls.Add(this.cbHwdDecoder);
            this.groupBox13.Controls.Add(this.chkEnableHwd);
            this.groupBox13.Controls.Add(this.chkInput8Bit);
            this.groupBox13.Controls.Add(this.lblffmsThreads);
            this.groupBox13.Controls.Add(this.ffmsThreads);
            this.groupBox13.Controls.Add(this.cbUseIncludedAviSynth);
            this.groupBox13.Controls.Add(this.chkDirectShowSource);
            this.groupBox13.Location = new System.Drawing.Point(270, 7);
            this.groupBox13.Name = "groupBox13";
            this.groupBox13.Size = new System.Drawing.Size(311, 166);
            this.groupBox13.TabIndex = 2;
            this.groupBox13.TabStop = false;
            this.groupBox13.Text = "Avisynth/+ Extra Settings";
            // 
            // cbHwdDecoder
            // 
            this.cbHwdDecoder.FormattingEnabled = true;
            this.cbHwdDecoder.Items.AddRange(new object[] {
            "Default Software",
            "NVIDIA CUVID",
            "Intel Quick Sync",
            "Auto",
            "DXVA2",
            "D3D11",
            "Vulkan"});
            this.cbHwdDecoder.Location = new System.Drawing.Point(199, 88);
            this.cbHwdDecoder.Name = "cbHwdDecoder";
            this.cbHwdDecoder.Size = new System.Drawing.Size(106, 21);
            this.cbHwdDecoder.TabIndex = 57;
            // 
            // chkEnableHwd
            // 
            this.chkEnableHwd.AutoSize = true;
            this.chkEnableHwd.Location = new System.Drawing.Point(15, 90);
            this.chkEnableHwd.Name = "chkEnableHwd";
            this.chkEnableHwd.Size = new System.Drawing.Size(180, 17);
            this.chkEnableHwd.TabIndex = 56;
            this.chkEnableHwd.Text = "Enable Hardware Video Decoder";
            this.chkEnableHwd.UseVisualStyleBackColor = true;
            this.chkEnableHwd.CheckedChanged += new System.EventHandler(this.chkEnableHwd_CheckedChanged);
            // 
            // chkInput8Bit
            // 
            this.chkInput8Bit.AutoSize = true;
            this.chkInput8Bit.Location = new System.Drawing.Point(13, 66);
            this.chkInput8Bit.Name = "chkInput8Bit";
            this.chkInput8Bit.Size = new System.Drawing.Size(152, 17);
            this.chkInput8Bit.TabIndex = 55;
            this.chkInput8Bit.Text = "Restrict input filter to 8 bit";
            this.chkInput8Bit.UseVisualStyleBackColor = true;
            // 
            // lblffmsThreads
            // 
            this.lblffmsThreads.AutoSize = true;
            this.lblffmsThreads.Location = new System.Drawing.Point(15, 135);
            this.lblffmsThreads.Name = "lblffmsThreads";
            this.lblffmsThreads.Size = new System.Drawing.Size(106, 13);
            this.lblffmsThreads.TabIndex = 54;
            this.lblffmsThreads.Text = "FFMS Thread Count:";
            // 
            // ffmsThreads
            // 
            this.ffmsThreads.Location = new System.Drawing.Point(127, 133);
            this.ffmsThreads.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.ffmsThreads.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.ffmsThreads.Name = "ffmsThreads";
            this.ffmsThreads.Size = new System.Drawing.Size(38, 21);
            this.ffmsThreads.TabIndex = 53;
            this.ffmsThreads.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // cbUseIncludedAviSynth
            // 
            this.cbUseIncludedAviSynth.Location = new System.Drawing.Point(13, 20);
            this.cbUseIncludedAviSynth.Name = "cbUseIncludedAviSynth";
            this.cbUseIncludedAviSynth.Size = new System.Drawing.Size(203, 17);
            this.cbUseIncludedAviSynth.TabIndex = 28;
            this.cbUseIncludedAviSynth.Text = "Always use the included AviSynth";
            // 
            // chkDirectShowSource
            // 
            this.chkDirectShowSource.AutoSize = true;
            this.chkDirectShowSource.Location = new System.Drawing.Point(13, 43);
            this.chkDirectShowSource.Name = "chkDirectShowSource";
            this.chkDirectShowSource.Size = new System.Drawing.Size(278, 17);
            this.chkDirectShowSource.TabIndex = 27;
            this.chkDirectShowSource.Text = "Enable DirectShowSource() in the AVS Script Creator";
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.groupBox2);
            this.tabPage3.Controls.Add(this.gbVideoPreview);
            this.tabPage3.Controls.Add(groupBox1);
            this.tabPage3.Controls.Add(this.autoUpdateGroupBox);
            this.tabPage3.Controls.Add(this.outputExtensions);
            this.tabPage3.Controls.Add(this.autoModeGroupbox);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(589, 435);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Extended";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.cbHttpProxyMode);
            this.groupBox2.Controls.Add(this.txt_httpproxyport);
            this.groupBox2.Controls.Add(this.txt_httpproxypwd);
            this.groupBox2.Controls.Add(this.txt_httpproxyuid);
            this.groupBox2.Controls.Add(this.txt_httpproxyaddress);
            this.groupBox2.Controls.Add(this.label21);
            this.groupBox2.Controls.Add(this.label20);
            this.groupBox2.Controls.Add(this.label19);
            this.groupBox2.Controls.Add(this.label18);
            this.groupBox2.Location = new System.Drawing.Point(273, 243);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(308, 189);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = " Auto Update Http Proxy ";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Use:";
            // 
            // cbHttpProxyMode
            // 
            this.cbHttpProxyMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbHttpProxyMode.FormattingEnabled = true;
            this.cbHttpProxyMode.Items.AddRange(new object[] {
            "None",
            "System Proxy",
            "Custom Proxy",
            "Custom Proxy With Login"});
            this.cbHttpProxyMode.Location = new System.Drawing.Point(55, 25);
            this.cbHttpProxyMode.Name = "cbHttpProxyMode";
            this.cbHttpProxyMode.Size = new System.Drawing.Size(179, 21);
            this.cbHttpProxyMode.TabIndex = 9;
            this.cbHttpProxyMode.SelectedIndexChanged += new System.EventHandler(this.cbHttpProxyMode_SelectedIndexChanged);
            // 
            // txt_httpproxyport
            // 
            this.txt_httpproxyport.Enabled = false;
            this.txt_httpproxyport.Location = new System.Drawing.Point(191, 52);
            this.txt_httpproxyport.Name = "txt_httpproxyport";
            this.txt_httpproxyport.Size = new System.Drawing.Size(43, 21);
            this.txt_httpproxyport.TabIndex = 6;
            // 
            // txt_httpproxypwd
            // 
            this.txt_httpproxypwd.Enabled = false;
            this.txt_httpproxypwd.Location = new System.Drawing.Point(55, 106);
            this.txt_httpproxypwd.Name = "txt_httpproxypwd";
            this.txt_httpproxypwd.PasswordChar = '*';
            this.txt_httpproxypwd.Size = new System.Drawing.Size(179, 21);
            this.txt_httpproxypwd.TabIndex = 8;
            // 
            // txt_httpproxyuid
            // 
            this.txt_httpproxyuid.Enabled = false;
            this.txt_httpproxyuid.Location = new System.Drawing.Point(55, 79);
            this.txt_httpproxyuid.Name = "txt_httpproxyuid";
            this.txt_httpproxyuid.Size = new System.Drawing.Size(179, 21);
            this.txt_httpproxyuid.TabIndex = 7;
            // 
            // txt_httpproxyaddress
            // 
            this.txt_httpproxyaddress.Enabled = false;
            this.txt_httpproxyaddress.Location = new System.Drawing.Point(55, 52);
            this.txt_httpproxyaddress.Name = "txt_httpproxyaddress";
            this.txt_httpproxyaddress.Size = new System.Drawing.Size(103, 21);
            this.txt_httpproxyaddress.TabIndex = 5;
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(6, 109);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(31, 13);
            this.label21.TabIndex = 4;
            this.label21.Text = "Pwd:";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(6, 82);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(36, 13);
            this.label20.TabIndex = 3;
            this.label20.Text = "Login:";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(164, 55);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(31, 13);
            this.label19.TabIndex = 2;
            this.label19.Text = "Port:";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(6, 55);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(43, 13);
            this.label18.TabIndex = 1;
            this.label18.Text = "Server:";
            // 
            // gbVideoPreview
            // 
            this.gbVideoPreview.Controls.Add(this.chkEnsureCorrectPlaybackSpeed);
            this.gbVideoPreview.Controls.Add(this.cbAddTimePos);
            this.gbVideoPreview.Controls.Add(this.chAlwaysOnTop);
            this.gbVideoPreview.Location = new System.Drawing.Point(3, 342);
            this.gbVideoPreview.Name = "gbVideoPreview";
            this.gbVideoPreview.Size = new System.Drawing.Size(263, 90);
            this.gbVideoPreview.TabIndex = 4;
            this.gbVideoPreview.TabStop = false;
            this.gbVideoPreview.Text = " Video Preview ";
            // 
            // chkEnsureCorrectPlaybackSpeed
            // 
            this.chkEnsureCorrectPlaybackSpeed.AutoSize = true;
            this.chkEnsureCorrectPlaybackSpeed.Location = new System.Drawing.Point(8, 63);
            this.chkEnsureCorrectPlaybackSpeed.Name = "chkEnsureCorrectPlaybackSpeed";
            this.chkEnsureCorrectPlaybackSpeed.Size = new System.Drawing.Size(173, 17);
            this.chkEnsureCorrectPlaybackSpeed.TabIndex = 2;
            this.chkEnsureCorrectPlaybackSpeed.Text = "Ensure correct playback speed";
            this.chkEnsureCorrectPlaybackSpeed.UseVisualStyleBackColor = true;
            // 
            // cbAddTimePos
            // 
            this.cbAddTimePos.AutoSize = true;
            this.cbAddTimePos.Location = new System.Drawing.Point(8, 40);
            this.cbAddTimePos.Name = "cbAddTimePos";
            this.cbAddTimePos.Size = new System.Drawing.Size(110, 17);
            this.cbAddTimePos.TabIndex = 1;
            this.cbAddTimePos.Text = "Add Time Position";
            this.cbAddTimePos.UseVisualStyleBackColor = true;
            // 
            // chAlwaysOnTop
            // 
            this.chAlwaysOnTop.AutoSize = true;
            this.chAlwaysOnTop.Location = new System.Drawing.Point(8, 17);
            this.chAlwaysOnTop.Name = "chAlwaysOnTop";
            this.chAlwaysOnTop.Size = new System.Drawing.Size(169, 17);
            this.chAlwaysOnTop.TabIndex = 0;
            this.chAlwaysOnTop.Text = "Set the Form \"Always on Top\"";
            this.chAlwaysOnTop.UseVisualStyleBackColor = true;
            // 
            // autoUpdateGroupBox
            // 
            this.autoUpdateGroupBox.Controls.Add(this.cbAutoUpdateServerSubList);
            this.autoUpdateGroupBox.Controls.Add(this.backupfiles);
            this.autoUpdateGroupBox.Controls.Add(this.configureServersButton);
            this.autoUpdateGroupBox.Controls.Add(this.useAutoUpdateCheckbox);
            this.autoUpdateGroupBox.Location = new System.Drawing.Point(273, 104);
            this.autoUpdateGroupBox.Name = "autoUpdateGroupBox";
            this.autoUpdateGroupBox.Size = new System.Drawing.Size(308, 133);
            this.autoUpdateGroupBox.TabIndex = 3;
            this.autoUpdateGroupBox.TabStop = false;
            this.autoUpdateGroupBox.Text = " Auto Update ";
            // 
            // cbAutoUpdateServerSubList
            // 
            this.cbAutoUpdateServerSubList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbAutoUpdateServerSubList.FormattingEnabled = true;
            this.cbAutoUpdateServerSubList.Items.AddRange(new object[] {
            "Use development update server",
            "Use custom update server"});
            this.cbAutoUpdateServerSubList.Location = new System.Drawing.Point(9, 47);
            this.cbAutoUpdateServerSubList.Name = "cbAutoUpdateServerSubList";
            this.cbAutoUpdateServerSubList.Size = new System.Drawing.Size(225, 21);
            this.cbAutoUpdateServerSubList.TabIndex = 5;
            this.cbAutoUpdateServerSubList.SelectedIndexChanged += new System.EventHandler(this.cbAutoUpdateServerSubList_SelectedIndexChanged);
            // 
            // backupfiles
            // 
            this.backupfiles.AutoSize = true;
            this.backupfiles.Location = new System.Drawing.Point(9, 103);
            this.backupfiles.Name = "backupfiles";
            this.backupfiles.Size = new System.Drawing.Size(187, 17);
            this.backupfiles.TabIndex = 4;
            this.backupfiles.Text = "Always backup files when needed";
            this.backupfiles.UseVisualStyleBackColor = true;
            this.backupfiles.CheckedChanged += new System.EventHandler(this.backupfiles_CheckedChanged);
            // 
            // configureServersButton
            // 
            this.configureServersButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.configureServersButton.Enabled = false;
            this.configureServersButton.Location = new System.Drawing.Point(9, 74);
            this.configureServersButton.Name = "configureServersButton";
            this.configureServersButton.Size = new System.Drawing.Size(225, 23);
            this.configureServersButton.TabIndex = 1;
            this.configureServersButton.Text = "Configure custom servers...";
            this.configureServersButton.UseVisualStyleBackColor = true;
            this.configureServersButton.Click += new System.EventHandler(this.configureServersButton_Click);
            // 
            // useAutoUpdateCheckbox
            // 
            this.useAutoUpdateCheckbox.AutoSize = true;
            this.useAutoUpdateCheckbox.Location = new System.Drawing.Point(9, 22);
            this.useAutoUpdateCheckbox.Name = "useAutoUpdateCheckbox";
            this.useAutoUpdateCheckbox.Size = new System.Drawing.Size(105, 17);
            this.useAutoUpdateCheckbox.TabIndex = 0;
            this.useAutoUpdateCheckbox.Text = "Use AutoUpdate";
            this.useAutoUpdateCheckbox.UseVisualStyleBackColor = true;
            // 
            // outputExtensions
            // 
            this.outputExtensions.Controls.Add(this.lblForcedName);
            this.outputExtensions.Controls.Add(this.txtForcedName);
            this.outputExtensions.Controls.Add(this.videoExtension);
            this.outputExtensions.Controls.Add(this.label11);
            this.outputExtensions.Controls.Add(this.label12);
            this.outputExtensions.Controls.Add(this.audioExtension);
            this.outputExtensions.Location = new System.Drawing.Point(3, 104);
            this.outputExtensions.Name = "outputExtensions";
            this.outputExtensions.Size = new System.Drawing.Size(264, 131);
            this.outputExtensions.TabIndex = 1;
            this.outputExtensions.TabStop = false;
            this.outputExtensions.Text = " Optional output extensions ";
            // 
            // lblForcedName
            // 
            this.lblForcedName.AutoSize = true;
            this.lblForcedName.Location = new System.Drawing.Point(9, 84);
            this.lblForcedName.Margin = new System.Windows.Forms.Padding(3);
            this.lblForcedName.Name = "lblForcedName";
            this.lblForcedName.Size = new System.Drawing.Size(164, 13);
            this.lblForcedName.TabIndex = 40;
            this.lblForcedName.Text = "Add text to forced track names: ";
            this.lblForcedName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtForcedName
            // 
            this.txtForcedName.Location = new System.Drawing.Point(11, 98);
            this.txtForcedName.Name = "txtForcedName";
            this.txtForcedName.Size = new System.Drawing.Size(243, 21);
            this.txtForcedName.TabIndex = 39;
            // 
            // videoExtension
            // 
            this.videoExtension.Location = new System.Drawing.Point(11, 20);
            this.videoExtension.Name = "videoExtension";
            this.videoExtension.Size = new System.Drawing.Size(182, 21);
            this.videoExtension.TabIndex = 0;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(199, 50);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(34, 13);
            this.label11.TabIndex = 3;
            this.label11.Text = "Audio";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(199, 23);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(33, 13);
            this.label12.TabIndex = 1;
            this.label12.Text = "Video";
            // 
            // audioExtension
            // 
            this.audioExtension.Location = new System.Drawing.Point(11, 48);
            this.audioExtension.Name = "audioExtension";
            this.audioExtension.Size = new System.Drawing.Size(182, 21);
            this.audioExtension.TabIndex = 2;
            // 
            // autoModeGroupbox
            // 
            this.autoModeGroupbox.Controls.Add(this.configAutoEncodeDefaults);
            this.autoModeGroupbox.Controls.Add(this.keep2ndPassLogFile);
            this.autoModeGroupbox.Controls.Add(this.keep2ndPassOutput);
            this.autoModeGroupbox.Controls.Add(this.label13);
            this.autoModeGroupbox.Controls.Add(this.nbPasses);
            this.autoModeGroupbox.Location = new System.Drawing.Point(4, 3);
            this.autoModeGroupbox.Name = "autoModeGroupbox";
            this.autoModeGroupbox.Size = new System.Drawing.Size(577, 95);
            this.autoModeGroupbox.TabIndex = 0;
            this.autoModeGroupbox.TabStop = false;
            this.autoModeGroupbox.Text = " Automated Encoding ";
            // 
            // configAutoEncodeDefaults
            // 
            this.configAutoEncodeDefaults.AutoSize = true;
            this.configAutoEncodeDefaults.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.configAutoEncodeDefaults.Location = new System.Drawing.Point(14, 47);
            this.configAutoEncodeDefaults.Name = "configAutoEncodeDefaults";
            this.configAutoEncodeDefaults.Size = new System.Drawing.Size(179, 23);
            this.configAutoEncodeDefaults.TabIndex = 5;
            this.configAutoEncodeDefaults.Text = "Configure AutoEncode defaults...";
            this.configAutoEncodeDefaults.UseVisualStyleBackColor = true;
            this.configAutoEncodeDefaults.Click += new System.EventHandler(this.autoEncodeDefaultsButton_Click);
            // 
            // keep2ndPassLogFile
            // 
            this.keep2ndPassLogFile.AutoSize = true;
            this.keep2ndPassLogFile.Checked = true;
            this.keep2ndPassLogFile.CheckState = System.Windows.Forms.CheckState.Checked;
            this.keep2ndPassLogFile.Location = new System.Drawing.Point(278, 21);
            this.keep2ndPassLogFile.Name = "keep2ndPassLogFile";
            this.keep2ndPassLogFile.Size = new System.Drawing.Size(176, 17);
            this.keep2ndPassLogFile.TabIndex = 4;
            this.keep2ndPassLogFile.Text = "Overwrite Stats File in 3rd pass";
            this.keep2ndPassLogFile.UseVisualStyleBackColor = true;
            // 
            // keep2ndPassOutput
            // 
            this.keep2ndPassOutput.AutoSize = true;
            this.keep2ndPassOutput.Checked = true;
            this.keep2ndPassOutput.CheckState = System.Windows.Forms.CheckState.Checked;
            this.keep2ndPassOutput.Location = new System.Drawing.Point(278, 51);
            this.keep2ndPassOutput.Name = "keep2ndPassOutput";
            this.keep2ndPassOutput.Size = new System.Drawing.Size(207, 17);
            this.keep2ndPassOutput.TabIndex = 3;
            this.keep2ndPassOutput.Text = "Keep 2nd pass Output in 3 pass mode";
            this.keep2ndPassOutput.UseVisualStyleBackColor = true;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(11, 22);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(93, 13);
            this.label13.TabIndex = 0;
            this.label13.Text = "Number of passes";
            // 
            // nbPasses
            // 
            this.nbPasses.Location = new System.Drawing.Point(117, 20);
            this.nbPasses.Maximum = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.nbPasses.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.nbPasses.Name = "nbPasses";
            this.nbPasses.Size = new System.Drawing.Size(40, 21);
            this.nbPasses.TabIndex = 1;
            this.nbPasses.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.groupBox11);
            this.tabPage4.Controls.Add(this.groupBox10);
            this.tabPage4.Controls.Add(this.groupBox9);
            this.tabPage4.Controls.Add(this.groupBox8);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(589, 435);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Worker/Job/Queue";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // groupBox11
            // 
            this.groupBox11.Controls.Add(this.label7);
            this.groupBox11.Controls.Add(this.cbStandbySettings);
            this.groupBox11.Controls.Add(this.cbRemoveJob);
            this.groupBox11.Controls.Add(this.cbAutoStart);
            this.groupBox11.Controls.Add(this.cbAutoStartOnStartup);
            this.groupBox11.Controls.Add(this.workerMaximumCount);
            this.groupBox11.Controls.Add(this.label6);
            this.groupBox11.Location = new System.Drawing.Point(252, 310);
            this.groupBox11.Name = "groupBox11";
            this.groupBox11.Size = new System.Drawing.Size(329, 119);
            this.groupBox11.TabIndex = 12;
            this.groupBox11.TabStop = false;
            this.groupBox11.Text = " global worker settings ";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(199, 83);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(107, 13);
            this.label7.TabIndex = 38;
            this.label7.Text = "while a job is running";
            // 
            // cbStandbySettings
            // 
            this.cbStandbySettings.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbStandbySettings.FormattingEnabled = true;
            this.cbStandbySettings.Location = new System.Drawing.Point(9, 80);
            this.cbStandbySettings.Margin = new System.Windows.Forms.Padding(2);
            this.cbStandbySettings.Name = "cbStandbySettings";
            this.cbStandbySettings.Size = new System.Drawing.Size(181, 21);
            this.cbStandbySettings.TabIndex = 37;
            // 
            // cbRemoveJob
            // 
            this.cbRemoveJob.AutoSize = true;
            this.cbRemoveJob.Checked = true;
            this.cbRemoveJob.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbRemoveJob.Location = new System.Drawing.Point(178, 43);
            this.cbRemoveJob.Name = "cbRemoveJob";
            this.cbRemoveJob.Size = new System.Drawing.Size(137, 17);
            this.cbRemoveJob.TabIndex = 22;
            this.cbRemoveJob.Text = "remove completed jobs";
            this.cbRemoveJob.CheckedChanged += new System.EventHandler(this.CbRemoveJob_CheckedChanged);
            // 
            // cbAutoStart
            // 
            this.cbAutoStart.AutoSize = true;
            this.cbAutoStart.Checked = true;
            this.cbAutoStart.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbAutoStart.Location = new System.Drawing.Point(178, 19);
            this.cbAutoStart.Name = "cbAutoStart";
            this.cbAutoStart.Size = new System.Drawing.Size(121, 17);
            this.cbAutoStart.TabIndex = 21;
            this.cbAutoStart.Text = "auto-start new jobs";
            this.cbAutoStart.CheckedChanged += new System.EventHandler(this.CbAutoStart_CheckedChanged);
            // 
            // cbAutoStartOnStartup
            // 
            this.cbAutoStartOnStartup.AutoSize = true;
            this.cbAutoStartOnStartup.Location = new System.Drawing.Point(6, 19);
            this.cbAutoStartOnStartup.Name = "cbAutoStartOnStartup";
            this.cbAutoStartOnStartup.Size = new System.Drawing.Size(170, 17);
            this.cbAutoStartOnStartup.TabIndex = 20;
            this.cbAutoStartOnStartup.Text = "auto-start on application start";
            this.cbAutoStartOnStartup.UseVisualStyleBackColor = true;
            this.cbAutoStartOnStartup.CheckedChanged += new System.EventHandler(this.CbAutoStartOnStartup_CheckedChanged);
            // 
            // workerMaximumCount
            // 
            this.workerMaximumCount.Location = new System.Drawing.Point(123, 41);
            this.workerMaximumCount.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.workerMaximumCount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.workerMaximumCount.Name = "workerMaximumCount";
            this.workerMaximumCount.Size = new System.Drawing.Size(34, 21);
            this.workerMaximumCount.TabIndex = 7;
            this.workerMaximumCount.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.workerMaximumCount.ValueChanged += new System.EventHandler(this.WorkerMaximumCount_ValueChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 44);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(115, 13);
            this.label6.TabIndex = 9;
            this.label6.Text = "maximum parallel jobs:";
            // 
            // groupBox10
            // 
            this.groupBox10.Controls.Add(this.label5);
            this.groupBox10.Controls.Add(this.label4);
            this.groupBox10.Controls.Add(this.label3);
            this.groupBox10.Controls.Add(this.cbIOPriority);
            this.groupBox10.Controls.Add(this.cbProcessPriority);
            this.groupBox10.Controls.Add(this.cbJobType);
            this.groupBox10.Location = new System.Drawing.Point(252, 232);
            this.groupBox10.Name = "groupBox10";
            this.groupBox10.Size = new System.Drawing.Size(328, 72);
            this.groupBox10.TabIndex = 11;
            this.groupBox10.TabStop = false;
            this.groupBox10.Text = " worker priority ";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(109, 20);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(81, 13);
            this.label5.TabIndex = 19;
            this.label5.Text = "Process Priority";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(215, 20);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(60, 13);
            this.label4.TabIndex = 18;
            this.label4.Text = "I/O Priority";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 20);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(51, 13);
            this.label3.TabIndex = 17;
            this.label3.Text = "Job Type";
            // 
            // cbIOPriority
            // 
            this.cbIOPriority.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbIOPriority.FormattingEnabled = true;
            this.cbIOPriority.Items.AddRange(new object[] {
            "low",
            "normal"});
            this.cbIOPriority.Location = new System.Drawing.Point(218, 40);
            this.cbIOPriority.Name = "cbIOPriority";
            this.cbIOPriority.Size = new System.Drawing.Size(100, 21);
            this.cbIOPriority.TabIndex = 16;
            this.cbIOPriority.SelectedIndexChanged += new System.EventHandler(this.CbIOPriority_SelectedIndexChanged);
            // 
            // cbProcessPriority
            // 
            this.cbProcessPriority.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbProcessPriority.FormattingEnabled = true;
            this.cbProcessPriority.Items.AddRange(new object[] {
            "low",
            "below normal",
            "normal",
            "above normal"});
            this.cbProcessPriority.Location = new System.Drawing.Point(112, 40);
            this.cbProcessPriority.Name = "cbProcessPriority";
            this.cbProcessPriority.Size = new System.Drawing.Size(100, 21);
            this.cbProcessPriority.TabIndex = 15;
            this.cbProcessPriority.SelectedIndexChanged += new System.EventHandler(this.CbProcessPriority_SelectedIndexChanged);
            // 
            // cbJobType
            // 
            this.cbJobType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbJobType.FormattingEnabled = true;
            this.cbJobType.Location = new System.Drawing.Point(6, 40);
            this.cbJobType.Name = "cbJobType";
            this.cbJobType.Size = new System.Drawing.Size(100, 21);
            this.cbJobType.TabIndex = 14;
            this.cbJobType.SelectedIndexChanged += new System.EventHandler(this.CbJobType_SelectedIndexChanged);
            // 
            // groupBox9
            // 
            this.groupBox9.Controls.Add(this.numericUpDown1);
            this.groupBox9.Controls.Add(this.btnAddSettings);
            this.groupBox9.Controls.Add(this.label2);
            this.groupBox9.Controls.Add(this.workerJobsListBox);
            this.groupBox9.Location = new System.Drawing.Point(7, 232);
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.Size = new System.Drawing.Size(239, 149);
            this.groupBox9.TabIndex = 10;
            this.groupBox9.TabStop = false;
            this.groupBox9.Text = " worker rule ";
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.numericUpDown1.Location = new System.Drawing.Point(160, 79);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.numericUpDown1.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(34, 21);
            this.numericUpDown1.TabIndex = 4;
            this.numericUpDown1.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // btnAddSettings
            // 
            this.btnAddSettings.Location = new System.Drawing.Point(6, 116);
            this.btnAddSettings.Name = "btnAddSettings";
            this.btnAddSettings.Size = new System.Drawing.Size(227, 23);
            this.btnAddSettings.TabIndex = 2;
            this.btnAddSettings.Text = "add rule set";
            this.btnAddSettings.UseVisualStyleBackColor = true;
            this.btnAddSettings.Click += new System.EventHandler(this.BtnAddSettings_Click);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Location = new System.Drawing.Point(120, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(113, 77);
            this.label2.TabIndex = 5;
            this.label2.Text = "maximum parallel jobs of the selected job(s):";
            // 
            // workerJobsListBox
            // 
            this.workerJobsListBox.FormattingEnabled = true;
            this.workerJobsListBox.Location = new System.Drawing.Point(6, 16);
            this.workerJobsListBox.Name = "workerJobsListBox";
            this.workerJobsListBox.Size = new System.Drawing.Size(108, 84);
            this.workerJobsListBox.TabIndex = 1;
            // 
            // groupBox8
            // 
            this.groupBox8.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox8.Controls.Add(this.btnDeleteSettings);
            this.groupBox8.Controls.Add(this.btnResetSettings);
            this.groupBox8.Controls.Add(this.workerSettingsListBox);
            this.groupBox8.Location = new System.Drawing.Point(7, 3);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(574, 223);
            this.groupBox8.TabIndex = 9;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = " worker rule set - defines how many jobs of specific types can run in parallel";
            // 
            // btnDeleteSettings
            // 
            this.btnDeleteSettings.Location = new System.Drawing.Point(6, 194);
            this.btnDeleteSettings.Name = "btnDeleteSettings";
            this.btnDeleteSettings.Size = new System.Drawing.Size(227, 23);
            this.btnDeleteSettings.TabIndex = 3;
            this.btnDeleteSettings.Text = "remove rule set";
            this.btnDeleteSettings.UseVisualStyleBackColor = true;
            this.btnDeleteSettings.Click += new System.EventHandler(this.BtnDeleteSettings_Click);
            // 
            // btnResetSettings
            // 
            this.btnResetSettings.Location = new System.Drawing.Point(354, 194);
            this.btnResetSettings.Name = "btnResetSettings";
            this.btnResetSettings.Size = new System.Drawing.Size(214, 23);
            this.btnResetSettings.TabIndex = 10;
            this.btnResetSettings.Text = "load default worker settings";
            this.btnResetSettings.UseVisualStyleBackColor = true;
            this.btnResetSettings.Click += new System.EventHandler(this.BtnResetSettings_Click);
            // 
            // workerSettingsListBox
            // 
            this.workerSettingsListBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.workerSettingsListBox.Location = new System.Drawing.Point(6, 19);
            this.workerSettingsListBox.Name = "workerSettingsListBox";
            this.workerSettingsListBox.Size = new System.Drawing.Size(562, 147);
            this.workerSettingsListBox.TabIndex = 1;
            this.workerSettingsListBox.SelectedIndexChanged += new System.EventHandler(this.WorkerSettingsListBox_SelectedIndexChanged);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.groupBox14);
            this.tabPage2.Controls.Add(this.groupBox7);
            this.tabPage2.Controls.Add(this.groupBox6);
            this.tabPage2.Controls.Add(this.groupBox5);
            this.tabPage2.Controls.Add(this.groupBox4);
            this.tabPage2.Controls.Add(this.vobGroupBox);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(589, 435);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "External Programs";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // groupBox14
            // 
            this.groupBox14.Controls.Add(this.lblQaac);
            this.groupBox14.Controls.Add(this.qaacLocation);
            this.groupBox14.Controls.Add(this.useQAAC);
            this.groupBox14.Location = new System.Drawing.Point(4, 123);
            this.groupBox14.Name = "groupBox14";
            this.groupBox14.Size = new System.Drawing.Size(577, 55);
            this.groupBox14.TabIndex = 48;
            this.groupBox14.TabStop = false;
            // 
            // lblQaac
            // 
            this.lblQaac.AutoSize = true;
            this.lblQaac.Enabled = false;
            this.lblQaac.Location = new System.Drawing.Point(7, 20);
            this.lblQaac.Name = "lblQaac";
            this.lblQaac.Size = new System.Drawing.Size(47, 13);
            this.lblQaac.TabIndex = 50;
            this.lblQaac.Text = "Location";
            // 
            // qaacLocation
            // 
            this.qaacLocation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.qaacLocation.Enabled = false;
            this.qaacLocation.Filename = "";
            this.qaacLocation.Filter = "QAAC|qaac.exe";
            this.qaacLocation.Location = new System.Drawing.Point(60, 16);
            this.qaacLocation.Margin = new System.Windows.Forms.Padding(4);
            this.qaacLocation.Name = "qaacLocation";
            this.qaacLocation.Size = new System.Drawing.Size(509, 23);
            this.qaacLocation.TabIndex = 49;
            // 
            // useQAAC
            // 
            this.useQAAC.AutoSize = true;
            this.useQAAC.Location = new System.Drawing.Point(12, 0);
            this.useQAAC.Name = "useQAAC";
            this.useQAAC.Size = new System.Drawing.Size(90, 17);
            this.useQAAC.TabIndex = 48;
            this.useQAAC.Text = "Enable QAAC";
            this.useQAAC.UseVisualStyleBackColor = true;
            this.useQAAC.CheckedChanged += new System.EventHandler(this.useQAAC_CheckedChanged);
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.useFDKAac);
            this.groupBox7.Controls.Add(this.lblFDK);
            this.groupBox7.Controls.Add(this.fdkaacLocation);
            this.groupBox7.Location = new System.Drawing.Point(4, 65);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(577, 51);
            this.groupBox7.TabIndex = 47;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "                                   ";
            // 
            // useFDKAac
            // 
            this.useFDKAac.AutoSize = true;
            this.useFDKAac.Location = new System.Drawing.Point(12, -1);
            this.useFDKAac.Name = "useFDKAac";
            this.useFDKAac.Size = new System.Drawing.Size(105, 17);
            this.useFDKAac.TabIndex = 46;
            this.useFDKAac.Text = "Enable FDK-AAC";
            this.useFDKAac.UseVisualStyleBackColor = true;
            this.useFDKAac.CheckedChanged += new System.EventHandler(this.useFDKAac_CheckedChanged);
            // 
            // lblFDK
            // 
            this.lblFDK.AutoSize = true;
            this.lblFDK.Enabled = false;
            this.lblFDK.Location = new System.Drawing.Point(11, 24);
            this.lblFDK.Name = "lblFDK";
            this.lblFDK.Size = new System.Drawing.Size(47, 13);
            this.lblFDK.TabIndex = 45;
            this.lblFDK.Text = "Location";
            // 
            // fdkaacLocation
            // 
            this.fdkaacLocation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fdkaacLocation.Enabled = false;
            this.fdkaacLocation.Filename = "";
            this.fdkaacLocation.Filter = "FDKAAC|fdkaac.exe";
            this.fdkaacLocation.Location = new System.Drawing.Point(64, 20);
            this.fdkaacLocation.Margin = new System.Windows.Forms.Padding(4);
            this.fdkaacLocation.Name = "fdkaacLocation";
            this.fdkaacLocation.Size = new System.Drawing.Size(509, 23);
            this.fdkaacLocation.TabIndex = 44;
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.chk64Bit);
            this.groupBox6.Controls.Add(this.chx264ExternalMuxer);
            this.groupBox6.Location = new System.Drawing.Point(8, 326);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(577, 103);
            this.groupBox6.TabIndex = 33;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = " Misc ";
            // 
            // chk64Bit
            // 
            this.chk64Bit.AutoSize = true;
            this.chk64Bit.Checked = true;
            this.chk64Bit.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chk64Bit.Location = new System.Drawing.Point(12, 20);
            this.chk64Bit.Name = "chk64Bit";
            this.chk64Bit.Size = new System.Drawing.Size(141, 17);
            this.chk64Bit.TabIndex = 51;
            this.chk64Bit.Text = "Use x64 tools if possible";
            this.chk64Bit.UseVisualStyleBackColor = true;
            // 
            // chx264ExternalMuxer
            // 
            this.chx264ExternalMuxer.AutoSize = true;
            this.chx264ExternalMuxer.Checked = true;
            this.chx264ExternalMuxer.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chx264ExternalMuxer.Location = new System.Drawing.Point(225, 20);
            this.chx264ExternalMuxer.Name = "chx264ExternalMuxer";
            this.chx264ExternalMuxer.Size = new System.Drawing.Size(190, 17);
            this.chx264ExternalMuxer.TabIndex = 49;
            this.chx264ExternalMuxer.Text = "x264: use external muxer for MKV";
            this.chx264ExternalMuxer.UseVisualStyleBackColor = true;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.useNeroAacEnc);
            this.groupBox5.Controls.Add(this.lblNero);
            this.groupBox5.Controls.Add(this.neroaacencLocation);
            this.groupBox5.Location = new System.Drawing.Point(4, 8);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(577, 51);
            this.groupBox5.TabIndex = 32;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "                                        ";
            // 
            // useNeroAacEnc
            // 
            this.useNeroAacEnc.AutoSize = true;
            this.useNeroAacEnc.Location = new System.Drawing.Point(12, -1);
            this.useNeroAacEnc.Name = "useNeroAacEnc";
            this.useNeroAacEnc.Size = new System.Drawing.Size(119, 17);
            this.useNeroAacEnc.TabIndex = 46;
            this.useNeroAacEnc.Text = "Enable NeroAacEnc";
            this.useNeroAacEnc.UseVisualStyleBackColor = true;
            this.useNeroAacEnc.CheckedChanged += new System.EventHandler(this.useNeroAacEnc_CheckedChanged);
            // 
            // lblNero
            // 
            this.lblNero.AutoSize = true;
            this.lblNero.Enabled = false;
            this.lblNero.Location = new System.Drawing.Point(11, 24);
            this.lblNero.Name = "lblNero";
            this.lblNero.Size = new System.Drawing.Size(47, 13);
            this.lblNero.TabIndex = 45;
            this.lblNero.Text = "Location";
            // 
            // neroaacencLocation
            // 
            this.neroaacencLocation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.neroaacencLocation.Enabled = false;
            this.neroaacencLocation.Filename = "";
            this.neroaacencLocation.Filter = "NeroAacEnc|neroaacenc.exe";
            this.neroaacencLocation.Location = new System.Drawing.Point(64, 20);
            this.neroaacencLocation.Margin = new System.Windows.Forms.Padding(4);
            this.neroaacencLocation.Name = "neroaacencLocation";
            this.neroaacencLocation.Size = new System.Drawing.Size(509, 23);
            this.neroaacencLocation.TabIndex = 44;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.btnClearMP4TempDirectory);
            this.groupBox4.Controls.Add(this.tempDirMP4);
            this.groupBox4.Location = new System.Drawing.Point(7, 266);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(578, 54);
            this.groupBox4.TabIndex = 31;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Temp Directory for MP4 Muxer";
            // 
            // btnClearMP4TempDirectory
            // 
            this.btnClearMP4TempDirectory.Location = new System.Drawing.Point(547, 20);
            this.btnClearMP4TempDirectory.Name = "btnClearMP4TempDirectory";
            this.btnClearMP4TempDirectory.Size = new System.Drawing.Size(25, 23);
            this.btnClearMP4TempDirectory.TabIndex = 42;
            this.btnClearMP4TempDirectory.Text = "x";
            this.btnClearMP4TempDirectory.Click += new System.EventHandler(this.btnClearMP4TempDirectory_Click);
            // 
            // tempDirMP4
            // 
            this.tempDirMP4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tempDirMP4.Filename = "";
            this.tempDirMP4.FolderMode = true;
            this.tempDirMP4.Location = new System.Drawing.Point(12, 20);
            this.tempDirMP4.Margin = new System.Windows.Forms.Padding(4);
            this.tempDirMP4.Name = "tempDirMP4";
            this.tempDirMP4.Size = new System.Drawing.Size(529, 23);
            this.tempDirMP4.TabIndex = 41;
            this.tempDirMP4.FileSelected += new MeGUI.FileBarEventHandler(this.tempDirMP4_FileSelected);
            // 
            // vobGroupBox
            // 
            this.vobGroupBox.Controls.Add(this.useDGIndexIM);
            this.vobGroupBox.Controls.Add(this.useDGIndexNV);
            this.vobGroupBox.Controls.Add(this.cbAutoLoadDG);
            this.vobGroupBox.Controls.Add(this.forceFilmPercentage);
            this.vobGroupBox.Controls.Add(this.autoForceFilm);
            this.vobGroupBox.Location = new System.Drawing.Point(8, 184);
            this.vobGroupBox.Name = "vobGroupBox";
            this.vobGroupBox.Size = new System.Drawing.Size(577, 76);
            this.vobGroupBox.TabIndex = 29;
            this.vobGroupBox.TabStop = false;
            this.vobGroupBox.Text = " DGIndex Tools";
            // 
            // useDGIndexIM
            // 
            this.useDGIndexIM.AutoSize = true;
            this.useDGIndexIM.Location = new System.Drawing.Point(12, 47);
            this.useDGIndexIM.Name = "useDGIndexIM";
            this.useDGIndexIM.Size = new System.Drawing.Size(115, 17);
            this.useDGIndexIM.TabIndex = 48;
            this.useDGIndexIM.Text = "Enable DGIndexIM";
            this.useDGIndexIM.UseVisualStyleBackColor = true;
            this.useDGIndexIM.CheckedChanged += new System.EventHandler(this.useDGIndexIM_CheckedChanged);
            // 
            // useDGIndexNV
            // 
            this.useDGIndexNV.AutoSize = true;
            this.useDGIndexNV.Location = new System.Drawing.Point(12, 24);
            this.useDGIndexNV.Name = "useDGIndexNV";
            this.useDGIndexNV.Size = new System.Drawing.Size(116, 17);
            this.useDGIndexNV.TabIndex = 47;
            this.useDGIndexNV.Text = "Enable DGIndexNV";
            this.useDGIndexNV.UseVisualStyleBackColor = true;
            this.useDGIndexNV.CheckedChanged += new System.EventHandler(this.useDGIndexNV_CheckedChanged);
            // 
            // cbAutoLoadDG
            // 
            this.cbAutoLoadDG.AutoSize = true;
            this.cbAutoLoadDG.Location = new System.Drawing.Point(225, 51);
            this.cbAutoLoadDG.Name = "cbAutoLoadDG";
            this.cbAutoLoadDG.Size = new System.Drawing.Size(179, 17);
            this.cbAutoLoadDG.TabIndex = 7;
            this.cbAutoLoadDG.Text = "autoload VOB files incrementally";
            this.cbAutoLoadDG.UseVisualStyleBackColor = true;
            // 
            // forceFilmPercentage
            // 
            this.forceFilmPercentage.Location = new System.Drawing.Point(392, 23);
            this.forceFilmPercentage.Name = "forceFilmPercentage";
            this.forceFilmPercentage.Size = new System.Drawing.Size(40, 21);
            this.forceFilmPercentage.TabIndex = 3;
            this.forceFilmPercentage.Value = new decimal(new int[] {
            95,
            0,
            0,
            0});
            // 
            // autoForceFilm
            // 
            this.autoForceFilm.AutoSize = true;
            this.autoForceFilm.Location = new System.Drawing.Point(225, 24);
            this.autoForceFilm.Name = "autoForceFilm";
            this.autoForceFilm.Size = new System.Drawing.Size(161, 17);
            this.autoForceFilm.TabIndex = 2;
            this.autoForceFilm.Text = "Auto Force Film at (percent)";
            // 
            // audioExtLabel
            // 
            this.audioExtLabel.AutoSize = true;
            this.audioExtLabel.Location = new System.Drawing.Point(137, 51);
            this.audioExtLabel.Name = "audioExtLabel";
            this.audioExtLabel.Size = new System.Drawing.Size(34, 13);
            this.audioExtLabel.TabIndex = 24;
            this.audioExtLabel.Text = "Audio";
            // 
            // videoExtLabel
            // 
            this.videoExtLabel.AutoSize = true;
            this.videoExtLabel.Location = new System.Drawing.Point(137, 24);
            this.videoExtLabel.Name = "videoExtLabel";
            this.videoExtLabel.Size = new System.Drawing.Size(34, 13);
            this.videoExtLabel.TabIndex = 23;
            this.videoExtLabel.Text = "Video";
            // 
            // autoEncodeDefaultsButton
            // 
            this.autoEncodeDefaultsButton.Location = new System.Drawing.Point(11, 51);
            this.autoEncodeDefaultsButton.Name = "autoEncodeDefaultsButton";
            this.autoEncodeDefaultsButton.Size = new System.Drawing.Size(114, 23);
            this.autoEncodeDefaultsButton.TabIndex = 4;
            this.autoEncodeDefaultsButton.Text = "Configure Defaults";
            this.autoEncodeDefaultsButton.UseVisualStyleBackColor = true;
            // 
            // toolTipHelp
            // 
            this.toolTipHelp.AutoPopDelay = 30000;
            this.toolTipHelp.InitialDelay = 500;
            this.toolTipHelp.IsBalloon = true;
            this.toolTipHelp.ReshowDelay = 100;
            this.toolTipHelp.ShowAlways = true;
            // 
            // helpButton1
            // 
            this.helpButton1.ArticleName = "Settings";
            this.helpButton1.AutoSize = true;
            this.helpButton1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.helpButton1.Location = new System.Drawing.Point(7, 468);
            this.helpButton1.Margin = new System.Windows.Forms.Padding(4);
            this.helpButton1.Name = "helpButton1";
            this.helpButton1.Size = new System.Drawing.Size(38, 23);
            this.helpButton1.TabIndex = 1;
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(597, 496);
            this.Controls.Add(this.helpButton1);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.saveButton);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingsForm";
            this.ShowInTaskbar = false;
            this.Text = "Options";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SettingsForm_KeyDown);
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            this.otherGroupBox.ResumeLayout(false);
            this.otherGroupBox.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.groupBox12.ResumeLayout(false);
            this.groupBox12.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.minimumTitleLength)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.acceptableFPSError)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.gbDefaultOutput.ResumeLayout(false);
            this.groupBox13.ResumeLayout(false);
            this.groupBox13.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ffmsThreads)).EndInit();
            this.tabPage3.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.gbVideoPreview.ResumeLayout(false);
            this.gbVideoPreview.PerformLayout();
            this.autoUpdateGroupBox.ResumeLayout(false);
            this.autoUpdateGroupBox.PerformLayout();
            this.outputExtensions.ResumeLayout(false);
            this.outputExtensions.PerformLayout();
            this.autoModeGroupbox.ResumeLayout(false);
            this.autoModeGroupbox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nbPasses)).EndInit();
            this.tabPage4.ResumeLayout(false);
            this.groupBox11.ResumeLayout(false);
            this.groupBox11.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.workerMaximumCount)).EndInit();
            this.groupBox10.ResumeLayout(false);
            this.groupBox10.PerformLayout();
            this.groupBox9.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.groupBox8.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.groupBox14.ResumeLayout(false);
            this.groupBox14.PerformLayout();
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.vobGroupBox.ResumeLayout(false);
            this.vobGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.forceFilmPercentage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.FolderBrowserDialog openFolderDialog;
        private System.Windows.Forms.GroupBox otherGroupBox;
        private System.Windows.Forms.OpenFileDialog openExecutableDialog;
        private System.Windows.Forms.CheckBox openScript;
        private System.Windows.Forms.CheckBox deleteAbortedOutput;
        private System.Windows.Forms.CheckBox deleteIntermediateFiles;
        private System.Windows.Forms.CheckBox openProgressWindow;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.GroupBox autoUpdateGroupBox;
        private System.Windows.Forms.CheckBox useAutoUpdateCheckbox;
        private System.Windows.Forms.GroupBox outputExtensions;
        private System.Windows.Forms.TextBox videoExtension;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox audioExtension;
        private System.Windows.Forms.GroupBox autoModeGroupbox;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.NumericUpDown nbPasses;
        private System.Windows.Forms.Label audioExtLabel;
        private System.Windows.Forms.Label videoExtLabel;
        private System.Windows.Forms.Button autoEncodeDefaultsButton;
        private System.Windows.Forms.TextBox command;
        private System.Windows.Forms.RadioButton runCommand;
        private System.Windows.Forms.RadioButton shutdown;
        private System.Windows.Forms.RadioButton donothing;
        private System.Windows.Forms.Button configureServersButton;
        private MeGUI.core.gui.HelpButton helpButton1;
        private System.Windows.Forms.CheckBox keep2ndPassOutput;
        private System.Windows.Forms.CheckBox keep2ndPassLogFile;
        private System.Windows.Forms.Button configAutoEncodeDefaults;
        private System.Windows.Forms.GroupBox gbVideoPreview;
        private System.Windows.Forms.CheckBox chAlwaysOnTop;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txt_httpproxyuid;
        private System.Windows.Forms.TextBox txt_httpproxyaddress;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.TextBox txt_httpproxypwd;
        private System.Windows.Forms.TextBox txt_httpproxyport;
        private System.Windows.Forms.CheckBox cbAddTimePos;
        private System.Windows.Forms.CheckBox backupfiles;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.GroupBox vobGroupBox;
        private System.Windows.Forms.NumericUpDown forceFilmPercentage;
        private System.Windows.Forms.CheckBox autoForceFilm;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button btnClearMP4TempDirectory;
        private FileBar tempDirMP4;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.CheckBox cbAutoLoadDG;
        private System.Windows.Forms.RadioButton rbCloseMeGUI;
        private System.Windows.Forms.ComboBox cbAutoUpdateServerSubList;
        private System.Windows.Forms.ToolTip toolTipHelp;
        private System.Windows.Forms.CheckBox chkEnsureCorrectPlaybackSpeed;
        private System.Windows.Forms.CheckBox cbUseITUValues;
        private FileBar neroaacencLocation;
        private System.Windows.Forms.CheckBox useNeroAacEnc;
        private System.Windows.Forms.Label lblNero;
        private System.Windows.Forms.CheckBox useDGIndexNV;
        private System.Windows.Forms.CheckBox useQAAC;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbHttpProxyMode;
        private System.Windows.Forms.CheckBox chx264ExternalMuxer;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.CheckBox useFDKAac;
        private System.Windows.Forms.Label lblFDK;
        private FileBar fdkaacLocation;
        private System.Windows.Forms.CheckBox useDGIndexIM;
        private System.Windows.Forms.CheckBox chk64Bit;
        private System.Windows.Forms.CheckBox chkDebugInformation;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.GroupBox groupBox8;
        private System.Windows.Forms.Button btnResetSettings;
        private System.Windows.Forms.ListBox workerSettingsListBox;
        private System.Windows.Forms.Button btnDeleteSettings;
        private System.Windows.Forms.GroupBox groupBox9;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckedListBox workerJobsListBox;
        private System.Windows.Forms.Button btnAddSettings;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.GroupBox groupBox10;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cbIOPriority;
        private System.Windows.Forms.ComboBox cbProcessPriority;
        private System.Windows.Forms.ComboBox cbJobType;
        private System.Windows.Forms.GroupBox groupBox11;
        private System.Windows.Forms.CheckBox cbRemoveJob;
        private System.Windows.Forms.CheckBox cbAutoStart;
        private System.Windows.Forms.CheckBox cbAutoStartOnStartup;
        private System.Windows.Forms.NumericUpDown workerMaximumCount;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox groupBox13;
        private System.Windows.Forms.CheckBox cbUseIncludedAviSynth;
        private System.Windows.Forms.CheckBox chkDirectShowSource;
        private System.Windows.Forms.CheckBox chkInput8Bit;
        private System.Windows.Forms.Label lblffmsThreads;
        private System.Windows.Forms.NumericUpDown ffmsThreads;
        private System.Windows.Forms.CheckBox chkEnableHwd;
        private System.Windows.Forms.ComboBox cbHwdDecoder;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ComboBox defaultLanguage2;
        private System.Windows.Forms.ComboBox defaultLanguage1;
        private System.Windows.Forms.GroupBox gbDefaultOutput;
        private core.gui.TargetSizeSCBox targetSizeSCBox1;
        private System.Windows.Forms.Button btnClearOutputDirecoty;
        private System.Windows.Forms.Button clearDefaultOutputDir;
        private FileBar defaultOutputDir;
        private System.Windows.Forms.GroupBox groupBox12;
        private System.Windows.Forms.Label lblMinimumLength;
        private System.Windows.Forms.NumericUpDown minimumTitleLength;
        private System.Windows.Forms.NumericUpDown acceptableFPSError;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Button resetDialogs;
        private System.Windows.Forms.Button configSourceDetector;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cbStandbySettings;
        private System.Windows.Forms.Label lblForcedName;
        private System.Windows.Forms.TextBox txtForcedName;
        private System.Windows.Forms.GroupBox groupBox14;
        private System.Windows.Forms.Label lblQaac;
        private FileBar qaacLocation;
    }
}