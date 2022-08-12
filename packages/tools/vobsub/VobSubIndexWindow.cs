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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;

using MeGUI.core.plugins.interfaces;
using MeGUI.core.util;
using MeGUI.core.details;

namespace MeGUI
{
    public partial class VobSubIndexWindow : Form
    {
        #region variables
        private bool dialogMode = false;
        private bool configured = false;
        private int iPGC = 1;
        private int iAngle = 0;
        #endregion

        #region start / stop

        public static readonly IDable<ReconfigureJob> Configurer = new IDable<ReconfigureJob>("vobsubber_reconfigure", new ReconfigureJob(ReconfigureJob));

        private static Job ReconfigureJob(Job j)
        {
            if (!(j is SubtitleIndexJob))
                return null;

            SubtitleIndexJob m = (SubtitleIndexJob)j;
            VobSubIndexWindow w = new VobSubIndexWindow(MainForm.Instance);

            w.Job = m;
            if (w.ShowDialog() == DialogResult.OK)
                return w.Job;
            else
                return m;
        }

        public VobSubIndexWindow(MainForm mainForm)
        {
            InitializeComponent();
            this.chkSingleFileExport.Checked = MainForm.Instance.Settings.VobSubberSingleFileExport;
            this.chkKeepAllStreams.Checked = MainForm.Instance.Settings.VobSubberKeepAll;
            this.chkShowAllStreams.Checked = MainForm.Instance.Settings.VobSubberShowAll;
            this.chkExtractForced.Checked = MainForm.Instance.Settings.VobSubberExtractForced;
        }

        private void VobSubIndexWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            MainForm.Instance.Settings.VobSubberSingleFileExport = this.chkSingleFileExport.Checked;
            MainForm.Instance.Settings.VobSubberKeepAll = this.chkKeepAllStreams.Checked;
            MainForm.Instance.Settings.VobSubberShowAll = this.chkShowAllStreams.Checked;
            MainForm.Instance.Settings.VobSubberExtractForced = this.chkExtractForced.Checked;
        }
        #endregion

        #region button handlers
        private void queueButton_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(output.Filename))
            {
                MessageBox.Show("Please select a propper output file", "Configuration Incomplete", MessageBoxButtons.OK);
                return;
            }

            if (!String.IsNullOrEmpty(output.Filename) && Drives.ableToWriteOnThisDrive(Path.GetPathRoot(output.Filename)))
            {
                if (configured)
                {
                    if (!dialogMode)
                    {
                        SubtitleIndexJob job = generateJob();
                        MainForm.Instance.Jobs.AddJobsToQueue(job);
                        if (this.closeOnQueue.Checked)
                            this.Close();
                    }
                }
                else
                    MessageBox.Show("You must select an Input and Output file to continue",
                        "Configuration incomplete", MessageBoxButtons.OK);
            }
            else
                MessageBox.Show("MeGUI cannot write on " + Path.GetPathRoot(output.Filename) +
                                ". Please select a propper output file.", "Configuration Incomplete", MessageBoxButtons.OK);
        }
        #endregion

        /// <summary>
        /// Even handler fired when a new input file is selected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void input_FileSelected(FileBar sender, FileBarEventArgs args)
        {
            subtitleGroupbox.Text = " Subtitles ";
            subtitleTracks.Items.Clear();
            output.Filename = string.Empty;

            string strIFOFile = input.Filename;
            // check if the input file can be processed
            if (!GetDVDSource(ref strIFOFile))
            {
                MessageBox.Show("No DVD structure found!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            subtitleGroupbox.Text += "- PGC " + iPGC + (iAngle > 0 ? " - Angle " + iAngle + " " : " "); 

            if (!input.Filename.Equals(strIFOFile))
                input.Filename = strIFOFile;

            SetSubtitles();
            checkIndexIO();
        }

        private void SetSubtitles()
        {
            subtitleTracks.Items.Clear();
            subtitleTracks.Items.AddRange(IFOparser.GetSubtitlesStreamsInfos(input.Filename, iPGC, chkShowAllStreams.Checked));
            PreselectItems();

            // get proper pre- and postfix based on the input file
            string filePath = FileUtil.GetOutputFolder(input.Filename);
            string filePrefix = FileUtil.GetOutputFilePrefix(input.Filename);
            string fileName = Path.GetFileNameWithoutExtension(input.Filename);
            if (FileUtil.RegExMatch(fileName, @"_\d{1,2}\z", false))
                fileName = fileName.Substring(0, fileName.LastIndexOf('_') + 1);
            else
                fileName = fileName + "_";
            output.Filename = Path.Combine(filePath, filePrefix + fileName + iPGC + (iAngle > 0 ? "_" + iAngle : "") + ".idx");
        }

        /// <summary>
        /// Checks if the source file if from a DVD stucture & asks for title list if needed
        /// </summary>
        /// <param name="fileName">input file name</param>
        /// <returns>true if DVD source is found, false if no DVD source is available</returns>
        private bool GetDVDSource(ref string fileName)
        {
            iPGC = 1;
            iAngle = 0;
            using (frmStreamSelect frm = new frmStreamSelect(fileName, SelectionMode.One))
            {
                // check if playlists have been found
                if (frm.TitleCount == 0)
                    return false;

                // only continue if a DVD or Blu-ray structure is found
                if (!frm.IsDVDSource)
                    return false;

                // open the selection window if not exactly one title set with the desired minimum length is found
                DialogResult dr = DialogResult.OK;
                if (frm.TitleCountWithRequiredLength != 1)
                    dr = frm.ShowDialog();

                if (dr != DialogResult.OK)
                    return false;

                ChapterInfo oChapterInfo = frm.SelectedSingleChapterInfo;
                string strSourceFile = Path.Combine(Path.GetDirectoryName(oChapterInfo.SourceFilePath), oChapterInfo.Title + "_0.IFO");
                if (!File.Exists(strSourceFile))
                {
                    // cannot be found. skipping...;
                    return false;
                }
                iPGC = oChapterInfo.PGCNumber;
                iAngle = oChapterInfo.AngleNumber;
                fileName = strSourceFile;
            }
            return true;
        }

        /// <summary>
        /// Selects the subtitles based on the language and options
        /// </summary>
        /// <returns></returns>
        private void PreselectItems()
        {
            // (un)select all based on keepAllTracks
            for (int i = 0; i < subtitleTracks.Items.Count; i++)
            {
                subtitleTracks.SetItemChecked(i, chkKeepAllStreams.Checked);
            }

            // no need to check further if all tracks should be selected
            if (chkKeepAllStreams.Checked)
                return;

            // check if any of the tracks should be selected based on the default MeGUI language(s)
            int x = -1;
            List<int> checkedItems = new List<int>();
            foreach (string item in subtitleTracks.Items)
            {
                x++;
                string[] temp = item.Split(new string[] { " - " }, StringSplitOptions.None);
                if (temp.Length < 2)
                    continue;

                if (temp[1].ToLowerInvariant().Trim().Equals(MainForm.Instance.Settings.DefaultLanguage1.ToLowerInvariant())
                    || temp[1].ToLowerInvariant().Trim().Equals(MainForm.Instance.Settings.DefaultLanguage2.ToLowerInvariant()))
                    checkedItems.Add(x);

            }
            foreach (int idx in checkedItems)
            {
                subtitleTracks.SetItemChecked(idx, true);
            }
        }

        private void checkIndexIO()
        {
            configured = (!input.Filename.Equals("") && !output.Filename.Equals(""));
            if (configured && dialogMode)
                queueButton.DialogResult = DialogResult.OK;
            else
                queueButton.DialogResult = DialogResult.None;
        }

        private SubtitleIndexJob generateJob()
        {
            List<int> trackIDs = new List<int>();
            foreach (string s in subtitleTracks.CheckedItems)
                trackIDs.Add(Int32.Parse(s.Substring(1,2)));
            return new SubtitleIndexJob(input.Filename, output.Filename, chkKeepAllStreams.Checked, trackIDs, iPGC, iAngle, chkSingleFileExport.Checked, chkExtractForced.Checked);
        }

        /// <summary>
        /// gets the index job created from the current configuration
        /// </summary>
        public SubtitleIndexJob Job
        {
            get { return generateJob(); }
            set { setConfig(value.Input, value.Output, value.IndexAllTracks, value.TrackIDs, value.PGC, value.Angle); }
        }

        public void setConfig(string input, string output, bool indexAllTracks, List<int> trackIDs, int pgc, int angle)
        {
            this.dialogMode = true;
            queueButton.Text = "Update";
            this.input.Filename = input;
            iPGC = pgc;
            iAngle = angle;
            SetSubtitles();
            this.output.Filename = output;
            checkIndexIO();
            if (indexAllTracks)
            {
                chkKeepAllStreams.Checked = true;
            }
            else
            {
                chkKeepAllStreams.Checked = false;
                int index = 0;
                List<int> checkedItems = new List<int>();
                foreach (string item in subtitleTracks.Items)
                {
                    if (trackIDs.Contains(int.Parse(item.Substring(1,2))))
                        checkedItems.Add(index);
                    index++;
                }
                foreach (int idx in checkedItems)
                {
                    subtitleTracks.SetItemChecked(idx, true);
                }
            }
        }

        private void output_FileSelected(FileBar sender, FileBarEventArgs args)
        {
            checkIndexIO();
        }

        private void chkShowAllStreams_CheckedChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(input.Filename))
                return;

            SetSubtitles();
            checkIndexIO();
            chkKeepAllStreams_CheckedChanged(null, null);
        }


        private void chkKeepAllStreams_CheckedChanged(object sender, EventArgs e)
        {
            subtitleTracks.Enabled = !chkKeepAllStreams.Checked;
            PreselectItems();
        }
    }

    public class VobSubTool : ITool
    {

        #region ITool Members

        public string Name
        {
            get { return "VobSubber"; }
        }

        /// <summary>
        /// launches the vobsub indexer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Run(MainForm info)
        {
            new VobSubIndexWindow(info).Show();
        }

        public Shortcut[] Shortcuts
        {
            get { return new Shortcut[] { Shortcut.CtrlN }; }
        }

        #endregion

        #region IIDable Members

        public string ID
        {
            get { return "VobSubber"; }
        }

        #endregion
    }
}