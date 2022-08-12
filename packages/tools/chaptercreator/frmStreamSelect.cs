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
using System.Linq;
using System.Windows.Forms;

namespace MeGUI
{
    public partial class frmStreamSelect : Form
    {
        ChapterExtractor extractor;
        List<ChapterInfo> arrChapterInfo;

        public frmStreamSelect(string strFileOrFolderInput) : this(strFileOrFolderInput, SelectionMode.One)
        {
        }

        public frmStreamSelect(string strFileOrFolderInput, SelectionMode selectionMode)
        {
            InitializeComponent();

            listBox1.Height = MainForm.Instance.Settings.DPIRescale(238);
            btnOK.BringToFront();
            btnCancel.BringToFront();

            listBox1.SelectionMode = selectionMode;
            arrChapterInfo = new List<ChapterInfo>();

            extractor = GetExtractor(strFileOrFolderInput);
            if (extractor == null)
                return;

            if (minimumTitleLength.Maximum >= MainForm.Instance.Settings.ChapterCreatorMinimumLength &&
                minimumTitleLength.Minimum <= MainForm.Instance.Settings.ChapterCreatorMinimumLength)
                minimumTitleLength.Value = MainForm.Instance.Settings.ChapterCreatorMinimumLength;
            else
                minimumTitleLength.Value = 900;

            extractor.StreamDetected += (sender, arg) =>
            {
                arrChapterInfo.Add(arg.ProgramChain);
            };
            extractor.ChaptersLoaded += (sender, arg) =>
            {
                for (int i = 0; i < arrChapterInfo.Count; i++)
                {
                    if (arrChapterInfo[i].SourceFilePath == arg.ProgramChain.SourceFilePath)
                    {
                        arrChapterInfo[i] = arg.ProgramChain;
                        break;
                    }
                }
            };
            extractor.ExtractionComplete += (sender, arg) =>
            {
                if (MainForm.Instance.Settings.ChapterCreatorSortString.Equals("chapter"))
                    btnSortChapter.Checked = true;
                else if (MainForm.Instance.Settings.ChapterCreatorSortString.Equals("name"))
                    btnSortName.Checked = true;
                else
                    btnSortDuration.Checked = true;
                rbShowAll.Text = "show all (" + TitleCount + " in total)";
            };

            extractor.GetStreams(strFileOrFolderInput);
        }

        /// <summary>
        /// Gets the ChapterExtractor for the source
        /// </summary>
        /// <param name="strFileOrInputFolder"></param>
        /// <returns>the ChapterExtrator will be returned if found or null if not</returns>
        private ChapterExtractor GetExtractor(string strFileOrInputFolder)
        {
            ChapterExtractor ex = null;

            if (strFileOrInputFolder.ToLowerInvariant().EndsWith(".mpls"))
                ex = new MplsExtractor();
            else if (strFileOrInputFolder.ToLowerInvariant().EndsWith(".txt") || strFileOrInputFolder.ToLowerInvariant().EndsWith(".xml"))
                ex = new TextExtractor();
            else if (!String.IsNullOrEmpty(MeGUI.core.util.FileUtil.GetBlurayPath(strFileOrInputFolder)))
                ex = new BlurayExtractor();
            else if (!String.IsNullOrEmpty(MeGUI.core.util.FileUtil.GetDVDPath(strFileOrInputFolder)))
            {
                ex = new DvdExtractor();
                this.Text = "Select your Title/PGC";
            }
            else if (System.IO.File.Exists(strFileOrInputFolder))
                ex = new MediaInfoExtractor();

            return ex;
        }

        /// <summary>
        /// True if the input is a DVD source
        /// </summary>
        public bool IsDVDSource
        {
            get { return (extractor is DvdExtractor); }
        }

        /// <summary>
        /// True if the input is a DVD or Blu-ray source
        /// </summary>
        public bool IsDVDOrBluraySource
        {
            get { return (extractor is DvdExtractor || extractor is BlurayExtractor || extractor is MplsExtractor); }
        }

        public ChapterInfo SelectedSingleChapterInfo
        {
            get { return listBox1.SelectedItems.Count > 0 ? listBox1.SelectedItem as ChapterInfo : null; }
        }

        public List<ChapterInfo> SelectedMultipleChapterInfo
        {
            get { return listBox1.SelectedItems.Count > 0 ? new List<ChapterInfo>(listBox1.SelectedItems.Cast<ChapterInfo>()) : null; }
        }

        /// <summary>
        /// Gets the number of playlists / titles
        /// </summary>
        public int TitleCount
        {
            get { return arrChapterInfo.Count; }
        }

        /// <summary>
        /// Gets the number of playlists / titles which have at least the minimum length
        /// </summary>
        public int TitleCountWithRequiredLength
        {
            get { return listBox1.Items.Count; }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItems.Count >= 1)
            {
                DialogResult = DialogResult.OK;
                return;
            }

            if (listBox1.Items.Count > 0)
                MessageBox.Show("Please select a title", "Selection missing", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else if (MessageBox.Show("Please change the \"show only ...\" value or select \"show all\" " + 
                "so that you can select a title.\n\nDo you want to switch to \"show all\" now?", 
                "Selection missing", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                rbShowAll.Checked = true;
        }

        /// <summary>
        /// prepare & update the item list based on the settings
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSort_CheckedChanged(object sender, EventArgs e)
        {
            if (arrChapterInfo.Count == 0)
                return;

            if (sender != null)
            {
                if (sender is RadioButton && !((RadioButton)sender).Checked)
                    return;

                minimumTitleLength.Enabled = !rbShowAll.Checked;
            }
            
            List<ChapterInfo> oSelectedList = new List<ChapterInfo>(listBox1.SelectedItems.Cast<ChapterInfo>());

            List<ChapterInfo> list = new List<ChapterInfo>();
            foreach (ChapterInfo oChapter in arrChapterInfo)
                if (rbShowAll.Checked || oChapter.Duration.TotalSeconds >= (int)minimumTitleLength.Value)
                    list.Add(oChapter);

            if (sender == minimumTitleLength && list.Count == listBox1.Items.Count)
                return;

            listBox1.Items.Clear();
            if (list.Count == 0)
                return;

            if (btnSortName.Checked)
                list = list.OrderBy(p => (p.Title + p.PGCNumber + p.AngleNumber)).ToList();
            else if (btnSortDuration.Checked)
                list = list.OrderByDescending(p => p.Duration).ToList();
            else
                list = list.OrderByDescending(p => p.Chapters.Count).ToList();
            
            listBox1.Items.AddRange(list.ToArray());

            if (oSelectedList.Count > 0)
            {
                for (int i = 0; i < listBox1.Items.Count; i++)
                {
                    foreach (ChapterInfo oSelectedItem in oSelectedList)
                    {
                        if ((ChapterInfo)listBox1.Items[i] == oSelectedItem)
                        {
                            listBox1.SetSelected(i, true);
                            break;
                        }
                    }
                }
            }
            if (listBox1.SelectedItems.Count == 0)
                listBox1.SelectedIndex = 0;
        }

        private void minimumTitleLength_ValueChanged(object sender, EventArgs e)
        { 
            btnSort_CheckedChanged(sender, null);
        }

        private void minimumTitleLength_KeyUp(object sender, KeyEventArgs e)
        {
            btnSort_CheckedChanged(sender, null);
        }

        private void frmStreamSelect_FormClosing(object sender, FormClosingEventArgs e)
        {
            MainForm.Instance.Settings.ChapterCreatorMinimumLength = (int)minimumTitleLength.Value;

            if (btnSortChapter.Checked)
                MainForm.Instance.Settings.ChapterCreatorSortString = "chapter";
            else if (btnSortName.Checked)
                MainForm.Instance.Settings.ChapterCreatorSortString = "name";
            else
                MainForm.Instance.Settings.ChapterCreatorSortString = "duration";
        }
    }
}