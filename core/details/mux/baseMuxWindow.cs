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
using System.IO;
using System.Windows.Forms;

using MeGUI.core.details;
using MeGUI.core.details.mux;
using MeGUI.core.util;

namespace MeGUI
{
    /// <summary>
    /// Summary description for Mux.
    /// </summary>
    public partial class baseMuxWindow : Form
    {
        #region variables
        protected List<MuxStreamControl> audioTracks;
        protected List<MuxStreamControl> subtitleTracks;
        protected Dar? dar;
        protected string audioFilter, videoInputFilter, subtitleFilter, chaptersFilter, outputFilter;
        private MuxProvider muxProvider;
        protected IMuxing muxer;
        #endregion

        #region start/stop
        public baseMuxWindow()
        {
            InitializeComponent();

            audioTracks = new List<MuxStreamControl>
            {
                muxStreamControl2
            };
            subtitleTracks = new List<MuxStreamControl>
            {
                muxStreamControl1
            };

            splitting.MinimumFileSize = new FileSize(Unit.MB, 1);
            subtitleTracks[0].input.FileSelected += new MeGUI.FileBarEventHandler(this.Subtitle_FileSelected);
            audioTracks[0].input.FileSelected += new MeGUI.FileBarEventHandler(this.Audio_FileSelected);

            vInput.SetInitialFolder(MainForm.Instance.Settings.MuxInputPath);
            muxStreamControl1.input.SetInitialFolder(MainForm.Instance.Settings.MuxInputPath);
            muxStreamControl2.input.SetInitialFolder(MainForm.Instance.Settings.MuxInputPath);
            chapters.SetInitialFolder(MainForm.Instance.Settings.MuxInputPath);
            output.SetInitialFolder(MainForm.Instance.Settings.MuxOutputPath);
        }

        public baseMuxWindow(IMuxing muxer) : this()
        {
            this.muxer = muxer;

            muxProvider = MainForm.Instance.MuxProvider;
            cbType.Items.Add("Standard");
            cbType.Items.AddRange(muxProvider.GetSupportedDevices((ContainerType)cbContainer.SelectedItem).ToArray());
            cbType.SelectedIndex = 0;
        }
        #endregion
        #region config
        /// <summary>
        /// sets the configuration of the GUI
        /// used when a job is loaded (jobs have everything already filled out)
        /// </summary>
        /// <param name="videoInput">the video input file</param>
        /// <param name="videoName">the name of the video track</param>
        /// <param name="framerate">framerate of the input</param>
        /// <param name="audioStreams">the audio streams</param>
        /// <param name="subtitleStreams">the subtitle streams</param>
        /// <param name="chapterInfo">the chapterinfo information</param>
        /// <param name="output">name of the output file</param>
        /// <param name="splitSize">split size of the output</param>
        /// <param name="dar">the DAR of the file</param>
        /// <param name="deviceType">the device type (e.g. for AVI, M2TS, MP4)</param>
        public void SetConfig(string videoInput, string videoName, decimal? framerate, MuxStream[] audioStreams, MuxStream[] subtitleStreams, ChapterInfo chapterInfo, string output, FileSize? splitSize, Dar? dar, string deviceType)
        {
            this.dar = dar;
            vInput.Filename = videoInput;
            fps.Value = framerate;
            this.videoName.Text = videoName;

            int index = 0;
            if (audioStreams != null)
            {
                foreach (MuxStream stream in audioStreams)
                {
                    if (audioTracks.Count == index)
                        AudioAddTrack();
                    audioTracks[index].Stream = stream;
                    index++;
                }
            }

            index = 0;
            if (subtitleStreams != null)
            {
                foreach (MuxStream stream in subtitleStreams)
                {
                    if (subtitleTracks.Count == index)
                        SubtitleAddTrack();
                    subtitleTracks[index].Stream = stream;
                    index++;
                }
            }
            
            if (chapterInfo != null)
                chapters.Filename = chapterInfo.SourceFilePath;
            this.output.Filename = output;
            this.splitting.Value = splitSize;
            this.muxButton.Text = "Update";
            this.chkCloseOnQueue.Visible = false;
            this.cbType.Text = deviceType;
            CheckIO();
        }

        /// <summary>
        /// gets the additionally configured stream configuration from this window
        /// this method is used when the muxwindow is created from the AutoEncodeWindow in order to configure audio languages
        /// add subtitles and chapters
        /// </summary>
        /// <param name="aStreams">the configured audio streams(language assignments)</param>
        /// <param name="sStreams">the newly added subtitle streams</param>
        /// <param name="chapterInfo">the ChapterInfo</param>
        public void GetAdditionalStreams(out MuxStream[] aStreams, out MuxStream[] sStreams, out ChapterInfo chapterInfo)
        {
            aStreams = GetStreams(audioTracks);
            sStreams = GetStreams(subtitleTracks);
            chapterInfo = new ChapterInfo();
            if (chapters.Filename.StartsWith("<"))
            {
                using (MediaInfoFile oInfo = new MediaInfoFile(vInput.Filename))
                    chapterInfo = oInfo.ChapterInfo;
            }
            else if (File.Exists(chapters.Filename))
            {
                chapterInfo.LoadFile(chapters.Filename);
                if (chapterInfo.FramesPerSecond == 0)
                {
                    if (!fps.Value.HasValue)
                    {
                        using (MediaInfoFile oInfo = new MediaInfoFile(vInput.Filename))
                            chapterInfo.FramesPerSecond = oInfo.VideoInfo.FPS;
                    }
                    else
                        chapterInfo.FramesPerSecond = (double)fps.Value.Value;
                }
            }
        }

        private static MuxStream[] GetStreams(List<MuxStreamControl> controls)
        {
            List<MuxStream> streams = new List<MuxStream>();
            foreach (MuxStreamControl t in controls)
            {
                if (t.Stream != null)
                    streams.Add(t.Stream);
            }
            return streams.ToArray();
        }
        #endregion
        #region helper method
        protected virtual void Subtitle_FileSelected(object sender, System.EventArgs e)
        {
            if (sender != null)
                MainForm.Instance.Settings.MuxInputPath = Path.GetDirectoryName(((FileBar)sender).Filename);
            
            foreach (MuxStreamControl oTrack in subtitleTracks)
            {
                if (String.IsNullOrEmpty(oTrack.input.Filename))
                    return;
            }
            SubtitleAddTrack();
        }

        protected virtual void Audio_FileSelected(object sender, System.EventArgs e)
        {
            if (sender != null)
                MainForm.Instance.Settings.MuxInputPath = Path.GetDirectoryName(((FileBar)sender).Filename);
            
            foreach (MuxStreamControl oTrack in audioTracks)
            {
                if (String.IsNullOrEmpty(oTrack.input.Filename))
                    return;
            }
            AudioAddTrack();
        }
        protected virtual void CheckIO()
        {
            if (string.IsNullOrEmpty(vInput.Filename))
            {
                muxButton.DialogResult = DialogResult.None;
                return;
            }
            else if (string.IsNullOrEmpty(output.Filename))
            {
                muxButton.DialogResult = DialogResult.None;
                return;
            }
            else if (MainForm.verifyOutputFile(output.Filename) != null)
            {
                muxButton.DialogResult = DialogResult.None;
                return;
            }
            else if (fps.Value == null && IsFPSRequired())
            {
                muxButton.DialogResult = DialogResult.None;
                return;
            }
            else
                muxButton.DialogResult = DialogResult.OK;
        }

        protected void chkDefaultStream_CheckedChanged(object sender, EventArgs e)
        {
            if (sender == null || ((CheckBox)sender).Checked == false)
                return;

            foreach (MuxStreamControl oTrack in subtitleTracks)
            {
                if (sender != oTrack.chkDefaultStream && oTrack.chkDefaultStream.Checked == true)
                    oTrack.chkDefaultStream.Checked = false;
            }
        }

        private void splitSize_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && (int)Keys.Back != (int)e.KeyChar)
                e.Handled = true;
        }
        #endregion
        protected virtual bool IsFPSRequired()
        {
            try
            {
                if (vInput.Filename.Length > 0)
                    return (VideoUtil.guessVideoType(vInput.Filename).ContainerType == null);
                return true;
            }
            catch (NullReferenceException) // This will throw if it can't guess the video type
            {
                return true;
            }
        }

        #region button event handlers
        private void vInput_FileSelected(FileBar sender, FileBarEventArgs args)
        {
            using (MediaInfoFile oInfo = new MediaInfoFile(vInput.Filename))
            {
                fps.Value = (decimal)oInfo.VideoInfo.FPS;
                
                if (string.IsNullOrEmpty(output.Filename))
                    ChooseOutputFilename();
                
                if (String.IsNullOrEmpty(chapters.Filename) && oInfo.HasChapters)
                    chapters.Filename = "<internal chapters>";
            }
            
            string videoFolder = Path.GetDirectoryName(vInput.Filename);
            MainForm.Instance.Settings.MuxInputPath = videoFolder;

            // Set input folder for EXISTING tracks
            foreach (var audioTrack in audioTracks)
                audioTrack.input.SetInitialFolder(videoFolder);

            foreach (var subtitleTrack in subtitleTracks)
                subtitleTrack.input.SetInitialFolder(videoFolder);

            chapters.SetInitialFolder(videoFolder);

            FileUpdated();
            CheckIO();
        }

        private void output_FileSelected(FileBar sender, FileBarEventArgs args)
        {
            MainForm.Instance.Settings.MuxOutputPath = Path.GetDirectoryName(output.Filename);
        }

        protected virtual void ChangeOutputExtension() { }

        private void ChooseOutputFilename()
        {
            string projectPath = MainForm.Instance.Settings.MuxOutputPath;
            if (String.IsNullOrEmpty(projectPath))
                projectPath = FileUtil.GetOutputFolder(vInput.Filename);
            string fileNameNoPath = Path.GetFileName(vInput.Filename);
            output.Filename = FileUtil.AddToFileName(Path.Combine(projectPath, fileNameNoPath), "-muxed");
            ChangeOutputExtension();
        }

        private void chapters_FileSelected(FileBar sender, FileBarEventArgs args)
        {
            // Use the current video folder if available, otherwise setting
            string folder = !string.IsNullOrEmpty(vInput.Filename)
                ? Path.GetDirectoryName(vInput.Filename)
                : Path.GetDirectoryName(chapters.Filename);
            

            if (!File.Exists(chapters.Filename))
            {
                FileUpdated();
                return;
            }

            ChapterInfo oChapter = new ChapterInfo();
            if (!oChapter.LoadFile(chapters.Filename))
                MessageBox.Show("The selected file is not a valid chapter file", "Invalid Chapter File", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            FileUpdated();
        }
        #endregion
        #region language dropdowns
        #endregion
        #region other events
        private void fps_SelectionChanged(object sender, string val)
        {
            CheckIO();
        }
        #endregion
        protected virtual void FileUpdated() { }
        protected virtual void UpDeviceTypes() { }


        #region adding / removing tracks
        private void audioAddTrack_Click(object sender, EventArgs e)
        {
            AudioAddTrack();
        }

        protected void AudioAddTrack()
        {
            TabPage p = new TabPage("Audio " + (audioTracks.Count + 1))
            {
                UseVisualStyleBackColor = audio.TabPages[0].UseVisualStyleBackColor,
                Padding = audio.TabPages[0].Padding
            };
            
            MuxStreamControl a = new MuxStreamControl
            {
                Dock = audioTracks[0].Dock,
                Padding = audioTracks[0].Padding,
                ShowDelay = audioTracks[0].ShowDelay,
                Filter = audioTracks[0].Filter
            };
            a.FileUpdated += muxStreamControl2_FileUpdated;
            a.input.FileSelected += new MeGUI.FileBarEventHandler(this.Audio_FileSelected);

            // Use the current video folder if available, otherwise setting
            string folder = !string.IsNullOrEmpty(vInput.Filename)
                ? Path.GetDirectoryName(vInput.Filename)
                : MainForm.Instance.Settings.MuxInputPath;
            a.input.SetInitialFolder(folder);

            audio.TabPages.Add(p);
            p.Controls.Add(a);
            audioTracks.Add(a);
        }

        private void audioRemoveTrack_Click(object sender, EventArgs e)
        {
            AudioRemoveTrack();
        }

        protected void AudioRemoveTrack()
        {
            audio.TabPages.RemoveAt(audio.TabPages.Count - 1);
            audioTracks.RemoveAt(audioTracks.Count - 1);
        }

        private void subtitleAddTrack_Click(object sender, EventArgs e)
        {
            SubtitleAddTrack();
        }

        protected void SubtitleAddTrack()
        {
            TabPage p = new TabPage("Subtitle " + (subtitleTracks.Count + 1))
            {
                UseVisualStyleBackColor = subtitles.TabPages[0].UseVisualStyleBackColor,
                Padding = subtitles.TabPages[0].Padding
            };
            
            MuxStreamControl a = new MuxStreamControl
            {
                Dock = subtitleTracks[0].Dock,
                Padding = subtitleTracks[0].Padding,
                ShowDelay = subtitleTracks[0].ShowDelay,
                ShowDefaultSubtitleStream = subtitleTracks[0].ShowDefaultSubtitleStream,
                ShowForceSubtitleStream = subtitleTracks[0].ShowForceSubtitleStream
            };
            a.chkDefaultStream.CheckedChanged += new System.EventHandler(this.chkDefaultStream_CheckedChanged);
            a.input.FileSelected += new MeGUI.FileBarEventHandler(this.Subtitle_FileSelected);
            a.Filter = subtitleTracks[0].Filter;
            a.FileUpdated += muxStreamControl1_FileUpdated;

            // Use the current video folder if available, otherwise setting
            string folder = !string.IsNullOrEmpty(vInput.Filename)
                ? Path.GetDirectoryName(vInput.Filename)
                : MainForm.Instance.Settings.MuxInputPath;
            a.input.SetInitialFolder(folder);

            subtitles.TabPages.Add(p);
            p.Controls.Add(a);
            subtitleTracks.Add(a);
        }

        private void subtitleRemoveTrack_Click(object sender, EventArgs e)
        {
            SubtitleRemoveTrack();
        }

        protected void SubtitleRemoveTrack()
        {
            subtitles.TabPages.RemoveAt(subtitles.TabPages.Count - 1);
            subtitleTracks.RemoveAt(subtitleTracks.Count - 1);
        }
        #endregion

        private void audioMenu_Opening(object sender, CancelEventArgs e)
        {
            audioRemoveTrack.Enabled = audioTracks.Count > 1;
        }

        private void subtitleMenu_Opening(object sender, CancelEventArgs e)
        {
            subtitleRemoveTrack.Enabled = subtitleTracks.Count > 1;
        }

        private void muxStreamControl1_FileUpdated(object sender, EventArgs e)
        {
            FileUpdated();
        }

        private void muxStreamControl2_FileUpdated(object sender, EventArgs e)
        {
            FileUpdated();
        }

        private void deleteChapterFile_Click(object sender, EventArgs e)
        {
            chapters.Filename = null;
        }

        private void cbContainer_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpDeviceTypes();
            UpdateChapterBox();
        }

        private void baseMuxWindow_KeyDown(object sender, KeyEventArgs e)
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

        private void output_Click(object sender, EventArgs e)
        {
            CheckIO();
            FileUpdated();
        }

        private void removeVideoTrack_Click(object sender, EventArgs e)
        {
            vInput.Filename = "";
            videoName.Text = "";
            fps.Value = null;
        }

        private void UpdateChapterBox()
        {
            if (muxer != null 
                && (muxer.MuxerType == MuxerType.AVIMUXGUI 
                || (muxer.MuxerType == MuxerType.TSMUXER && cbType.SelectedItem.ToString() == "Standard")))
                chaptersGroupbox.Enabled = false;
            else if (cbContainer.SelectedItem != null 
                && (cbContainer.SelectedItem.ToString().Equals("AVI") 
                || (cbContainer.SelectedItem.ToString().Equals("M2TS") && cbType.SelectedItem.ToString() == "Standard")))
                chaptersGroupbox.Enabled = false;
            else
                chaptersGroupbox.Enabled = true;
        }

        private void cbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateChapterBox();
        }
    }
}