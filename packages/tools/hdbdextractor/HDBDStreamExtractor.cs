// ****************************************************************************
// 
// Copyright (C) 2005-2026 Doom9 & al
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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Forms;

using MeGUI.core.plugins.interfaces;
using eac3to;

namespace MeGUI.packages.tools.hdbdextractor
{
    public partial class HdBdStreamExtractor : Form
    {
        private int inputType = 1;
        private string dummyInput = "";
        private Eac3toInfo _oEac3toInfo;
        private List<string> input = new List<string>();

        public HdBdStreamExtractor()
        {
            InitializeComponent();

            FeatureDataGridView.RowTemplate.Height = MainForm.Instance.Settings.DPIRescale(22);
            FeatureDataGridView.ColumnHeadersHeight = MainForm.Instance.Settings.DPIRescale(23);
            FeatureDataGridView.Columns[0].Width = MainForm.Instance.Settings.DPIRescale(20);
            FeatureDataGridView.Columns[1].Width = MainForm.Instance.Settings.DPIRescale(125);
            //FeatureDataGridView.Columns[2].Width = MainForm.Instance.Settings.DPIRescale(426);
            FeatureDataGridView.Columns[3].Width = MainForm.Instance.Settings.DPIRescale(100);
            FeatureDataGridView.Columns[4].Width = MainForm.Instance.Settings.DPIRescale(80);

            StreamDataGridView.RowTemplate.Height = MainForm.Instance.Settings.DPIRescale(22);
            StreamDataGridView.ColumnHeadersHeight = MainForm.Instance.Settings.DPIRescale(23);
            StreamDataGridView.Columns[0].Width = MainForm.Instance.Settings.DPIRescale(20);
            StreamDataGridView.Columns[1].Width = MainForm.Instance.Settings.DPIRescale(49);
            StreamDataGridView.Columns[2].Width = MainForm.Instance.Settings.DPIRescale(47);
            //StreamDataGridView.Columns[3].Width = MainForm.Instance.Settings.DPIRescale(260);
            StreamDataGridView.Columns[4].Width = MainForm.Instance.Settings.DPIRescale(90);
            StreamDataGridView.Columns[5].Width = MainForm.Instance.Settings.DPIRescale(90);
            StreamDataGridView.Columns[6].Width = MainForm.Instance.Settings.DPIRescale(90);
            StreamDataGridView.Columns[7].Width = MainForm.Instance.Settings.DPIRescale(80);
            
            FolderOutputSourceButton.Height = FolderOutputTextBox.Height + 2;
            FolderInputSourceButton.Height = FolderInputTextBox.Height + 2;
            
            if (MainForm.Instance.Settings.Eac3toLastUsedFileMode)
                FileSelection.Select();

            StreamDataGridView.Columns["StreamAddOptionsTextBox"].Visible = MainForm.Instance.Settings.Eac3toEnableCustomOptions;
            toolStripMenuItem2.Checked = MainForm.Instance.Settings.Eac3toAutoSelectStreams;
            toolStripMenuItem3.Checked = MainForm.Instance.Settings.Eac3toDefaultToHD;
            toolStripMenuItem4.Checked = MainForm.Instance.Settings.Eac3toEnableEncoder;
            toolStripMenuItem5.Checked = MainForm.Instance.Settings.Eac3toEnableDecoder;
            toolStripMenuItem6.Checked = MainForm.Instance.Settings.Eac3toEnableCustomOptions;
            addPrefixBasedOnInputToolStripMenuItem.Checked = MainForm.Instance.Settings.Eac3toAddPrefix;
        }

        #region backgroundWorker

        private void SetProgress(object sender, ProgressChangedEventArgs e)
        {
            SetToolStripProgressBarValue(e.ProgressPercentage);
            if (e.UserState != null)
                SetToolStripLabelText(e.UserState.ToString());
        }

        public void SetData(object sender, RunWorkerCompletedEventArgs e)
        {
            SetToolStripProgressBarValue(0);
            
            if (e == null || e.Result == null)
            {
                ResetCursor(Cursors.Default);
                    return;
            }
            
            SetToolStripLabelText(Extensions.GetStringValue(((ResultState)e.Result)));

            switch ((ResultState)e.Result)
            {
                case ResultState.FeatureCompleted:
                    FeatureDataGridView.DataSource = _oEac3toInfo.Features;
                    FeatureDataGridView.SelectionChanged += new System.EventHandler(this.FeatureDataGridView_SelectionChanged);
                    StreamDataGridView.ClearSelection();
                    StreamDataGridView.DataSource = null;
                    StreamDataGridView.Rows.Clear();
                    StreamDataGridView.DataSource = typeof(eac3to.Stream);
                    ResetCursor(Cursors.Default);
                    if (_oEac3toInfo.Features.Count == 1)
                        FeatureDataGridView.Rows[0].Selected = true;
                    break;
                case ResultState.StreamCompleted:
                    if (FileSelection.Checked)
                        StreamDataGridView.DataSource = _oEac3toInfo.Features[0].Streams;
                    else
                        StreamDataGridView.DataSource = ((eac3to.Feature)FeatureDataGridView.SelectedRows[0].DataBoundItem).Streams;
                    SelectTracks();
                    ResetCursor(Cursors.Default);
                    break;
                case ResultState.ExtractCompleted:
                    QueueButton.Enabled = true;
                    ResetCursor(Cursors.Default);
                    break;
            }
        }
        #endregion

        #region GUI
        delegate void SetToolStripProgressBarValueCallback(int value);
        private void SetToolStripProgressBarValue(int value)
        {
            lock (this)
            {
                if (this.InvokeRequired)
                    this.BeginInvoke(new SetToolStripProgressBarValueCallback(SetToolStripProgressBarValue), value);
                else if (this.ToolStripProgressBar != null)
                    this.ToolStripProgressBar.Value = value;
            }
        }

        delegate void SetToolStripLabelTextCallback(string message);
        private void SetToolStripLabelText(string message)
        {
            lock (this)
            {
                if (this.InvokeRequired)
                    this.BeginInvoke(new SetToolStripLabelTextCallback(SetToolStripLabelText), message);
                else if (this.ToolStripProgressBar != null)
                    this.ToolStripStatusLabel.Text = message;
            }
        }

        delegate void ResetCursorCallback(System.Windows.Forms.Cursor cursor);
        private void ResetCursor(System.Windows.Forms.Cursor cursor)
        {
            lock (this)
            {
                if (!this.InvokeRequired)
                {
                    this.Cursor = cursor;
                    StreamDataGridView.Cursor = cursor;
                    FeatureDataGridView.Enabled = cursor != Cursors.WaitCursor;
                }
                else
                    this.BeginInvoke(new ResetCursorCallback(ResetCursor), cursor);
            }
        }

        private void FolderInputSourceButton_Click(object sender, EventArgs e)
        {
            string myinput = "";
            string outputFolder = "";
            DialogResult dr;
            input.Clear();

            if (FolderSelection.Checked)
            {
                folderBrowserDialog1.SelectedPath = MainForm.Instance.Settings.Eac3toLastFolderPath;
                folderBrowserDialog1.Description = "Choose an input directory";
                folderBrowserDialog1.ShowNewFolderButton = false;
                dr = folderBrowserDialog1.ShowDialog();
                if (dr != DialogResult.OK)
                    return;
                MainForm.Instance.Settings.Eac3toLastFolderPath = folderBrowserDialog1.SelectedPath;

                inputType = 1;
                if (folderBrowserDialog1.SelectedPath.EndsWith(":\\"))
                    myinput = folderBrowserDialog1.SelectedPath;
                else
                    myinput = folderBrowserDialog1.SelectedPath + System.IO.Path.DirectorySeparatorChar;
                outputFolder = myinput.Substring(0, myinput.LastIndexOf("\\") + 1);
                input.Add(myinput);
            }
            else
            {
                openFileDialog1.InitialDirectory = MainForm.Instance.Settings.Eac3toLastFilePath;
                dr = openFileDialog1.ShowDialog();
                if (dr != DialogResult.OK)
                    return;
                MainForm.Instance.Settings.Eac3toLastFilePath = System.IO.Path.GetDirectoryName(openFileDialog1.FileName);

                inputType = 2;
                int idx = 0;
                foreach (String file in openFileDialog1.FileNames)
                {
                    if (idx == 0)
                    {
                        outputFolder = System.IO.Path.GetDirectoryName(file);
                        myinput = file;
                        input.Add(file);
                    }
                    else // seamless branching
                    {
                        myinput += "+" + file;
                        input.Add(file);
                    }
                    idx++;
                }
            }

            FolderInputTextBox.Text = myinput;
            if (String.IsNullOrEmpty(FolderInputTextBox.Text))
                return;

            if (String.IsNullOrEmpty(FolderOutputTextBox.Text))
            {
                FolderOutputTextBox.Text = MeGUI.core.util.FileUtil.GetOutputFolder(outputFolder);
                if (!MeGUI.core.util.FileUtil.IsDirWriteable(FolderOutputTextBox.Text) && !String.IsNullOrEmpty(MainForm.Instance.Settings.Eac3toLastDestinationPath))
                    FolderOutputTextBox.Text = MainForm.Instance.Settings.Eac3toLastDestinationPath;
            }

            FeatureDataGridView.DataSource = null;
            FeatureDataGridView.Rows.Clear();
            FeatureDataGridView.ClearSelection();
            StreamDataGridView.DataSource = null;
            StreamDataGridView.Rows.Clear();
            StreamDataGridView.ClearSelection();

            _oEac3toInfo = new Eac3toInfo(input, null, null);
            _oEac3toInfo.FetchInformationCompleted += new OnFetchInformationCompletedHandler(SetData);
            _oEac3toInfo.ProgressChanged += new OnProgressChangedHandler(SetProgress);

            ResetCursor(Cursors.WaitCursor);
            _oEac3toInfo.FetchFeatureInformation();
        }

        private void FolderOutputSourceButton_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.SelectedPath = MainForm.Instance.Settings.Eac3toLastDestinationPath;
            folderBrowserDialog1.Description = "Choose an output directory";
            folderBrowserDialog1.ShowNewFolderButton = true;
            DialogResult dr = folderBrowserDialog1.ShowDialog();
            if (dr != DialogResult.OK)
                return;

            FolderOutputTextBox.Text = folderBrowserDialog1.SelectedPath;
            MainForm.Instance.Settings.Eac3toLastDestinationPath = folderBrowserDialog1.SelectedPath;
        }

        private void StreamDataGridView_DataSourceChanged(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in StreamDataGridView.Rows)
            {
                DataGridViewComboBoxCell comboBox = row.Cells["StreamExtractAsComboBox"] as DataGridViewComboBoxCell;
                if (comboBox.IsInEditMode)
                {
                    comboBox.ReadOnly = true;
                    comboBox.ReadOnly = false;
                }
                comboBox.Items.Clear();
                if (!(row.DataBoundItem is Stream s) || s.Type == eac3to.StreamType.Unknown)
                {
                    row.ReadOnly = true;
                    continue;
                }
                comboBox.Items.AddRange(s.ExtractTypes);

                switch (s.Type)
                {
                    case eac3to.StreamType.Join:
                        if (s.Name == "Joined EVO")
                            comboBox.Value = "EVO";
                        else 
                            comboBox.Value = "VOB";
                        break;
                    default:
                        comboBox.Value = comboBox.Items[0]; break;
                }
            }
        }

        private void QueueButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(FolderOutputTextBox.Text))
            {
                MessageBox.Show("Configure output target folder prior to queueing job.", "Queue Job", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                return;
            }
            if (StreamDataGridView.Rows.Count == 0)
            {
                MessageBox.Show("Retrieve streams prior to queueing job.", "Queue Job", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                return;
            }
            if (!IsStreamCheckedForExtract())
            {
                MessageBox.Show("Select stream(s) to extract prior to queueing job.", "Queue Job", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                return;
            }
            if (FolderSelection.Checked && FeatureDataGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("Select feature prior to queueing job.", "Queue Job", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                return;
            }
            if (!Drives.ableToWriteOnThisDrive(System.IO.Path.GetPathRoot(FolderOutputTextBox.Text)))
            {
                MessageBox.Show("MeGUI cannot write on " + System.IO.Path.GetPathRoot(FolderOutputTextBox.Text) +
                                "\nPlease, select another Output path.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                return;
            }

            eac3toArgs args = new eac3toArgs();
            args.eac3toPath = MainForm.Instance.Settings.Eac3to.Path;
            if (FolderSelection.Checked)
                args.featureNumber = ((Feature)FeatureDataGridView.SelectedRows[0].DataBoundItem).Number.ToString();
            args.workingFolder = string.IsNullOrEmpty(FolderOutputTextBox.Text) ? FolderOutputTextBox.Text : System.IO.Path.GetDirectoryName(args.eac3toPath);
            args.resultState = ResultState.ExtractCompleted;

            try
            {
                args.args = GenerateArguments();
            }
            catch (ApplicationException ex)
            {
                MessageBox.Show(ex.Message, "Stream Extract", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                return;
            }

            // Load to MeGUI job queue
            HDStreamsExJob job;
            if (FolderSelection.Checked)
                job = new HDStreamsExJob(new List<string>() { dummyInput }, this.FolderOutputTextBox.Text, args.featureNumber, args.args, inputType);
            else
                job = new HDStreamsExJob(new List<string>(input), this.FolderOutputTextBox.Text, null, args.args, inputType);

            MainForm.Instance.Jobs.AddJobsToQueue(job);
            if (this.closeOnQueue.Checked)
                this.Close();
        }

        private string GenerateArguments()
        {
            string filePrefix = string.Empty;
            if (MainForm.Instance.Settings.Eac3toAddPrefix)
            {
                if (FolderSelection.Checked)
                    filePrefix = MeGUI.core.util.FileUtil.GetOutputFilePrefix(dummyInput);
                else
                    filePrefix = System.IO.Path.GetFileNameWithoutExtension(input[0]) + "_";
            }

            StringBuilder sb = new StringBuilder();
            foreach (DataGridViewRow row in StreamDataGridView.Rows)
            {
                Stream stream = row.DataBoundItem as Stream;
                if (stream.Type == eac3to.StreamType.Unknown)
                    continue;

                DataGridViewCheckBoxCell extractStream = row.Cells["StreamExtractCheckBox"] as DataGridViewCheckBoxCell;
                if (extractStream.Value == null || int.Parse(extractStream.Value.ToString()) != 1)
                    continue;

                string strExtension = row.Cells["StreamExtractAsComboBox"].Value.ToString().ToLowerInvariant();
                if (strExtension.ToUpperInvariant().EndsWith("_CORE"))
                    strExtension = strExtension.Substring(0, strExtension.Length - 5);
                if (row.Cells["StreamExtractAsComboBox"].Value == null)
                    throw new ApplicationException(string.Format("Specify an extraction type for stream:\r\n\n\t{0}: {1}", stream.Number, stream.Description));

                if (FolderSelection.Checked)
                    sb.Append(string.Format("{0}:\"{1}\" {2} ", stream.Number,
                        System.IO.Path.Combine(FolderOutputTextBox.Text, string.Format("{0}F{1}_T{2}_{3} - {4}.{5}", filePrefix, 
                        ((Feature)FeatureDataGridView.SelectedRows[0].DataBoundItem).Number, stream.Number, Extensions.GetStringValue(stream.Type), 
                        row.Cells["languageDataGridViewTextBoxColumn"].Value, strExtension)), row.Cells["StreamAddOptionsTextBox"].Value).Trim());
                else
                    sb.Append(string.Format("{0}:\"{1}\" {2} ", stream.Number,
                        System.IO.Path.Combine(FolderOutputTextBox.Text, string.Format("{0}T{1}_{2} - {3}.{4}", filePrefix, stream.Number,
                        Extensions.GetStringValue(stream.Type), row.Cells["languageDataGridViewTextBoxColumn"].Value, strExtension)),
                        row.Cells["StreamAddOptionsTextBox"].Value).Trim());

                if (stream.Type == eac3to.StreamType.Audio)
                {
                    if (((string)row.Cells["StreamExtractAsComboBox"].Value).EndsWith("_CORE"))
                        sb.Append(" -core");
                    else if (((AudioStream)stream).HasDialNorm)
                        sb.Append(" -keepDialnorm");
                }

                sb.Append(' ');
            }

            return sb.ToString();
        }

        private void CancelButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void HelpButton2_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://en.wikibooks.org/wiki/Eac3to/How_to_Use");
        }

        private void HdBrStreamExtractor_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            MainForm.Instance.Settings.Eac3toLastUsedFileMode = FileSelection.Checked;
        }

        private bool IsStreamCheckedForExtract()
        {
            bool enableQueue = false;

            foreach (DataGridViewRow row in StreamDataGridView.Rows)
                if (row.Cells["StreamExtractCheckBox"].Value != null && int.Parse(row.Cells["StreamExtractCheckBox"].Value.ToString()) == 1)
                    enableQueue = true;

            return enableQueue;
        }

        private void Eac3toLinkLabel_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://forum.doom9.org/showthread.php?t=125966");
        }

        private void FeatureDataGridView_SelectionChanged(object sender, EventArgs e)
        {
            if (_oEac3toInfo.IsBusy()) // disallow selection change
                return;

            // only fire after the Databind has completed on grid and a row is selected
            if (FeatureDataGridView.Rows.Count != _oEac3toInfo.Features.Count || FeatureDataGridView.SelectedRows.Count != 1)
                return;

            Feature feature = FeatureDataGridView.SelectedRows[0].DataBoundItem as Feature;

            // Check for Streams
            if (feature.Streams != null && feature.Streams.Count > 0)
            {
                // use already collected streams
                StreamDataGridView.DataSource = feature.Streams;
                SelectTracks();
                return;
            }

            // reset stream view
            StreamDataGridView.DataSource = null;
            StreamDataGridView.Rows.Clear();
            StreamDataGridView.ClearSelection();

            // create dummy input string for megui job
            if (feature.Description.Contains("EVO"))
            {
                if (feature.Description.Contains("+"))
                {
                    if (FolderInputTextBox.Text.ToUpperInvariant().Contains("HVDVD_TS"))
                        dummyInput = FolderInputTextBox.Text.Substring(0, FolderInputTextBox.Text.IndexOf("HVDVD_TS")) + "HVDVD_TS\\" + feature.Description.Substring(0, feature.Description.IndexOf("+"));
                    else
                        dummyInput = FolderInputTextBox.Text + "HVDVD_TS\\" + feature.Description.Substring(0, feature.Description.IndexOf("+"));
                }
                else
                {
                    if (FolderInputTextBox.Text.ToUpperInvariant().Contains("HVDVD_TS"))
                        dummyInput = FolderInputTextBox.Text.Substring(0, FolderInputTextBox.Text.IndexOf("HVDVD_TS")) + "HVDVD_TS\\" + feature.Description.Substring(0, feature.Description.IndexOf(","));
                    else
                        dummyInput = FolderInputTextBox.Text + "HVDVD_TS\\" + feature.Description.Substring(0, feature.Description.IndexOf(","));
                }
            }
            else if (feature.Description.Contains("(angle"))
            {   
                dummyInput = GetBDMVPath(FolderInputTextBox.Text, feature.Description.Substring(0, feature.Description.IndexOf(" (")));
            }
            else if (MeGUI.core.util.FileUtil.RegExMatch(feature.Description, @"\A\d{5}\.mpls,", true))
            {
                // e.g. "00017.mpls, 00018.m2ts, 1:28:39" found
                string des = feature.Description.Substring(0, feature.Description.IndexOf(","));
                dummyInput = GetBDMVPath(FolderInputTextBox.Text, des);
            }
            else if (feature.Description.Substring(feature.Description.LastIndexOf(".") + 1, 4) == "m2ts")
            {
                string des = feature.Description.Substring(feature.Description.IndexOf(",") + 2, feature.Description.LastIndexOf(",") - feature.Description.IndexOf(",") - 2);

                if (des.Contains("+")) // seamless branching
                    dummyInput = GetBDMVPath(FolderInputTextBox.Text, feature.Description.Substring(0, feature.Description.IndexOf(",")));
                else
                    dummyInput = GetBDMVPath(FolderInputTextBox.Text, des);
            }
            else
                dummyInput = GetBDMVPath(FolderInputTextBox.Text, feature.Description.Substring(0, feature.Description.IndexOf(",")));

            ResetCursor(Cursors.WaitCursor);
            _oEac3toInfo.FetchStreamInformation(((Feature)FeatureDataGridView.SelectedRows[0].DataBoundItem).Number);
        }

        private void FeatureDataGridView_DataBindingComplete(object sender, System.Windows.Forms.DataGridViewBindingCompleteEventArgs e)
        {
            FeatureDataGridView.ClearSelection();
        }

        private void FeatureDataGridView_DataSourceChanged(object sender, EventArgs e)
        {
            if (FeatureDataGridView.Rows.Count == 0)
                return;

            foreach (DataGridViewRow row in FeatureDataGridView.Rows)
            {
                DataGridViewComboBoxCell comboBox = row.Cells["FeatureFileDataGridViewComboBoxColumn"] as DataGridViewComboBoxCell;

                if (!(row.DataBoundItem is Feature feature) || feature.Files == null || feature.Files.Count == 0)
                    continue;

                foreach (File file in feature.Files)
                {
                    comboBox.Items.Add(file.FullName);

                    if (file.Index == 1)
                        comboBox.Value = file.FullName;
                }
            }
        }
        #endregion

        private void SelectTracks()
        {
            if (!MainForm.Instance.Settings.Eac3toAutoSelectStreams)
                return;

            bool bVideoSelected = false;
            foreach (DataGridViewRow row in StreamDataGridView.Rows)
            {
                DataGridViewCheckBoxCell extractStream = row.Cells["StreamExtractCheckBox"] as DataGridViewCheckBoxCell;
                if (row.Cells["StreamTypeTextBox"].Value.ToString().Equals("Chapter"))
                {
                    extractStream.Value = 1;
                }
                else if (!bVideoSelected && row.Cells["StreamTypeTextBox"].Value.ToString().Equals("Video"))
                {
                    extractStream.Value = 1;
                    bVideoSelected = true;
                }
                else if (row.Cells["languageDataGridViewTextBoxColumn"].Value.ToString().ToLowerInvariant().Equals(MainForm.Instance.Settings.DefaultLanguage1.ToLowerInvariant()) ||
                    row.Cells["languageDataGridViewTextBoxColumn"].Value.ToString().ToLowerInvariant().Equals(MainForm.Instance.Settings.DefaultLanguage2.ToLowerInvariant()))
                {
                    extractStream.Value = 1;
                }
                else if (String.IsNullOrEmpty(row.Cells["languageDataGridViewTextBoxColumn"].Value.ToString().Trim()) &&
                    (row.Cells["StreamTypeTextBox"].Value.ToString().Equals("Audio") || row.Cells["StreamTypeTextBox"].Value.ToString().Equals("Subtitle")))
                {
                    extractStream.Value = 1;
                }
            }
        }

        private static string GetBDMVPath(string path, string file)
        {
            string filePath;
            while (System.IO.Directory.Exists(path))
            {
                filePath = System.IO.Path.Combine(System.IO.Path.Combine(path, "BDMV\\STREAM"), file);
                if (System.IO.File.Exists(filePath))
                    return filePath;

                filePath = System.IO.Path.Combine(System.IO.Path.Combine(path, "BDMV\\PLAYLIST"), file);
                if (System.IO.File.Exists(filePath))
                    return filePath;

                if (System.IO.Path.GetFullPath(path).Equals(System.IO.Path.GetPathRoot(path)))
                    return file;
                
                System.IO.DirectoryInfo pathInfo = new System.IO.DirectoryInfo(path);
                path = pathInfo.Parent.FullName;
            }
            return file;
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            MainForm.Instance.Settings.Eac3toAutoSelectStreams = toolStripMenuItem2.Checked;
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            MainForm.Instance.Settings.Eac3toDefaultToHD = toolStripMenuItem3.Checked;
            StreamDataGridView_DataSourceChanged(null, null);
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            MainForm.Instance.Settings.Eac3toEnableEncoder = toolStripMenuItem4.Checked;
            StreamDataGridView_DataSourceChanged(null, null);
        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            MainForm.Instance.Settings.Eac3toEnableDecoder = toolStripMenuItem5.Checked;
            StreamDataGridView_DataSourceChanged(null, null);
        }

        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {
            MainForm.Instance.Settings.Eac3toEnableCustomOptions = toolStripMenuItem6.Checked;
            StreamDataGridView.Columns["StreamAddOptionsTextBox"].Visible = toolStripMenuItem6.Checked;
        }

        private void addPrefixBasedOnInputToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainForm.Instance.Settings.Eac3toAddPrefix = addPrefixBasedOnInputToolStripMenuItem.Checked;
        }

        private void HdBdStreamExtractor_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (Form.ModifierKeys == Keys.None && keyData == Keys.Escape)
            {
                this.Close();
                return true;
            }
            return base.ProcessDialogKey(keyData);
        }
    }

    public class HdBdExtractorTool : ITool
    {

        #region ITool Members

        public string Name
        {
            get { return "HD Streams Extractor"; }
        }

        public void Run(MainForm info)
        {
            HdBdStreamExtractor form = new HdBdStreamExtractor();
            form.Show();
        }

        public Shortcut[] Shortcuts
        {
            get { return new Shortcut[] { Shortcut.CtrlF7 }; }
        }

        #endregion

        #region IIDable Members

        public string ID
        {
            get { return Name; }
        }

        #endregion
    }
}