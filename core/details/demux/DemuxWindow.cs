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
using System.IO;
using System.Windows.Forms;

using MeGUI.core.util;

namespace MeGUI
{
    public partial class DemuxWindow : Form
    {
        private MediaInfoFile mediaInfo;

        public DemuxWindow()
        {
            InitializeComponent();
            BuildModernLayout();
        }

        private void btnBrowseInput_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Supported Files (*.mkv;*.webm;*.mp4;*.mov;*.m4v;*.ts;*.m2ts)|*.mkv;*.webm;*.mp4;*.mov;*.m4v;*.ts;*.m2ts|All Files (*.*)|*.*";
                ofd.Title = "Select input file to demux";

                if (!string.IsNullOrEmpty(txtInputFile.Text))
                {
                    try
                    {
                        ofd.InitialDirectory = Path.GetDirectoryName(txtInputFile.Text);
                    }
                    catch { }
                }

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    LoadInputFile(ofd.FileName);
                }
            }
        }

        private void LoadInputFile(string filePath)
        {
            txtInputFile.Text = filePath;
            clbTracks.Items.Clear();

            if (mediaInfo != null)
            {
                mediaInfo.Dispose();
                mediaInfo = null;
            }

            if (!File.Exists(filePath))
            {
                MessageBox.Show("The selected file does not exist.", "File not found",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string ext = Path.GetExtension(filePath).ToLowerInvariant();
            if (ext != ".mkv" && ext != ".webm" &&
                ext != ".mp4" && ext != ".mov" && ext != ".m4v" &&
                ext != ".ts" && ext != ".m2ts")
            {
                MessageBox.Show("Unsupported file format. Supported formats: MKV, WebM, MP4, MOV, M4V, TS, M2TS.",
                    "Unsupported format", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtInputFile.Text = string.Empty;
                return;
            }

            try
            {
                Cursor = Cursors.WaitCursor;
                mediaInfo = new MediaInfoFile(filePath);

                if (!mediaInfo.MediaInfoOK)
                {
                    MessageBox.Show("Could not read media information from the selected file.",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtInputFile.Text = string.Empty;
                    return;
                }

                PopulateTracks();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error reading file: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtInputFile.Text = string.Empty;
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void PopulateTracks()
        {
            clbTracks.Items.Clear();

            if (mediaInfo.HasVideo && mediaInfo.VideoInfo.Track != null)
            {
                clbTracks.Items.Add(mediaInfo.VideoInfo.Track, true);
            }

            foreach (AudioTrackInfo audioTrack in mediaInfo.AudioInfo.Tracks)
            {
                clbTracks.Items.Add(audioTrack, true);
            }

            foreach (SubtitleTrackInfo subtitleTrack in mediaInfo.SubtitleInfo.Tracks)
            {
                clbTracks.Items.Add(subtitleTrack, true);
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            txtInputFile.Text = string.Empty;
            clbTracks.Items.Clear();

            if (mediaInfo != null)
            {
                mediaInfo.Dispose();
                mediaInfo = null;
            }
        }

        private void btnQueue_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtInputFile.Text))
            {
                MessageBox.Show("You must select an input file.", "Missing input",
                    MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            if (clbTracks.CheckedItems.Count == 0)
            {
                MessageBox.Show("You must select at least one track to extract.", "No tracks selected",
                    MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            string inputFile = txtInputFile.Text;
            string outputPath = Path.GetDirectoryName(inputFile);
            string ext = Path.GetExtension(inputFile).ToLowerInvariant();

            List<TrackInfo> selectedTracks = new List<TrackInfo>();
            foreach (object item in clbTracks.CheckedItems)
            {
                if (item is TrackInfo trackInfo)
                {
                    trackInfo.SourceFileName = inputFile;
                    selectedTracks.Add(trackInfo);
                }
            }

            Job job = CreateDemuxJob(inputFile, outputPath, ext, selectedTracks);
            if (job != null)
            {
                MainForm.Instance.Jobs.AddJobsToQueue(job);
                MessageBox.Show("Demux job has been added to the queue.", "Job Queued",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                if (chkCloseOnQueue.Checked)
                    this.Close();
            }
        }

        private Job CreateDemuxJob(string inputFile, string outputPath, string ext, List<TrackInfo> tracks)
        {
            if (ext == ".mkv" || ext == ".webm")
            {
                return new MkvExtractJob(inputFile, outputPath, tracks);
            }
            else if (ext == ".mp4" || ext == ".mov" || ext == ".m4v")
            {
                return new MP4BoxDemuxJob(inputFile, outputPath, tracks);
            }
            else if (ext == ".ts" || ext == ".m2ts")
            {
                return new TsMuxeRDemuxJob(inputFile, outputPath, tracks);
            }

            MessageBox.Show("No suitable demuxer found for this file type.", "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            return null;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (mediaInfo != null)
            {
                mediaInfo.Dispose();
                mediaInfo = null;
            }
            base.OnFormClosing(e);
        }

        /// <summary>
        /// Windows 11 settings look: the controls are moved into rounded cards (see ModernDialog)
        /// </summary>
        private void BuildModernLayout()
        {
            MeGUI.core.gui.ModernDialog d = new MeGUI.core.gui.ModernDialog(this);
            d.Row("Input file", null, d.Wide(txtInputFile, 300), d.Btn(btnBrowseInput));
            MeGUI.core.gui.SettingsCard tracks = d.Card();
            tracks.AddRow("Tracks", null);
            d.Block(clbTracks, 560, 200, tracks);
            d.Toggle(chkCloseOnQueue, null);
            d.Finish(null, 640, btnQueue, btnRemove);
        }
    }
}
