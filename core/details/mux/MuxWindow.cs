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

using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

using MeGUI.core.util;
using MeGUI.core.details;

namespace MeGUI
{
    public partial class MuxWindow : baseMuxWindow
    {
        public static readonly IDable<ReconfigureJob> Configurer = new IDable<ReconfigureJob>("mux_reconfigure", new ReconfigureJob(ReconfigureJob));

        private static Job ReconfigureJob(Job j)
        {
            if (!(j is MuxJob))
                return null;

            MuxJob m = (MuxJob)j;
            MuxWindow w = new MuxWindow(MainForm.Instance.MuxProvider.GetMuxer(m.MuxType));

            w.Job = m;
            if (w.ShowDialog() == DialogResult.OK)
                return w.Job;
            else
                return m;
        }

        public MuxWindow(IMuxing muxer) : base(muxer)
        {
            InitializeComponent();
            
            if (muxer == null)
                return;
            
            this.muxer = muxer;
            if (muxer.GetSupportedAudioTypes().Count == 0)
                audio.Enabled = false;
            if (muxer.GetSupportedChapterTypes().Count == 0)
                chaptersGroupbox.Enabled = false;
            if (muxer.GetSupportedSubtitleTypes().Count == 0)
                subtitles.Enabled = false;
            else if (this.muxer.MuxerType == MuxerType.MKVMERGE)
            {
                subtitleTracks[0].ShowDefaultSubtitleStream = true;
                subtitleTracks[0].ShowDelay = true;
                subtitleTracks[0].chkDefaultStream.CheckedChanged += new System.EventHandler(base.chkDefaultStream_CheckedChanged);
                subtitleTracks[0].chkDefaultStream.Checked = true;
                subtitleTracks[0].ShowForceSubtitleStream = true;
            }
            else if (this.muxer.MuxerType == MuxerType.MP4BOX)
            {
                subtitleTracks[0].ShowDefaultSubtitleStream = false;
                subtitleTracks[0].ShowDelay = false;
                subtitleTracks[0].ShowForceSubtitleStream = true;
            }

            if (muxer.GetSupportedChapterTypes().Count == 0)
                chaptersGroupbox.Enabled = false;
            if (muxer.GetSupportedDeviceTypes().Count == 0)
                cbType.Enabled = false;
            muxedInput.Filter = muxer.GetMuxedInputFilter();

            if (this.muxer.MuxerType == MuxerType.AVIMUXGUI)
                fps.Enabled = false;

            audioTracks[0].Filter = muxer.GetAudioInputFilter();
            output.Filter = muxer.GetOutputTypeFilter();
            subtitleTracks[0].Filter = muxer.GetSubtitleInputFilter();
            vInput.Filter = muxer.GetVideoInputFilter();
            chapters.Filter = muxer.GetChapterInputFilter();

            base.muxButton.Click += new System.EventHandler(this.muxButton_Click);

            this.Text = "MeGUI - " + muxer.Name;

            cbType.Items.Clear();
            cbType.Items.Add("Standard");
            cbType.Items.AddRange(muxer.GetSupportedDeviceTypes().ToArray());
            this.cbType.SelectedIndex = 0;
            foreach (object o in cbType.Items) // I know this is ugly, but using the DeviceOutputType doesn't work unless we're switching to manual serialization
            {
                if (o.ToString().Equals(MainForm.Instance.Settings.AedSettings.DeviceOutputType))
                {
                    cbType.SelectedItem = o;
                    break;
                }
            }
        }

        protected virtual void muxButton_Click(object sender, System.EventArgs e)
        {
            if (muxButton.DialogResult != DialogResult.OK)
            {
                if (string.IsNullOrEmpty(vInput.Filename))
                {
                    MessageBox.Show("You must configure a video input file", "Missing input", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
                else if (string.IsNullOrEmpty(output.Filename))
                {
                    MessageBox.Show("You must configure an output file", "Missing input", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
                else if (MainForm.verifyOutputFile(output.Filename) != null)
                {
                    MessageBox.Show("Invalid output file", "Invalid output", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else if (!fps.Value.HasValue)
                {
                    MessageBox.Show("You must select a framerate", "Missing input", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
            }
            else
            {
                if (this.muxButton.Text.Equals("Update"))
                {
                    this.Close();
                }
                else
                {
                    MuxJob job = this.Job;
                    MainForm.Instance.Jobs.AddJobsToQueue(job);
                    if (chkCloseOnQueue.Checked)
                        this.Close();
                    else
                        output.Filename = String.Empty;
                }
            }
        }

        protected virtual MuxJob GenerateMuxJob()
        {
            MuxJob job = new MuxJob();
            GetAdditionalStreams(out MuxStream[] aStreams, out MuxStream[] sStreams, out ChapterInfo chapters);

            job.Settings.AudioStreams.AddRange(aStreams);
            job.Settings.SubtitleStreams.AddRange(sStreams);
            job.Settings.ChapterInfo = chapters;
            job.Settings.VideoName = this.videoName.Text;
            job.Settings.VideoInput = vInput.Filename;
            job.Settings.MuxedOutput = output.Filename;
            job.Settings.MuxedInput = this.muxedInput.Filename;
            job.Settings.DAR = base.dar;
            job.Settings.DeviceType = this.cbType.Text;
            
            if (string.IsNullOrEmpty(job.Settings.VideoInput))
                job.Input = job.Settings.MuxedInput;
            else
                job.Input = job.Settings.VideoInput;

            job.Output = job.Settings.MuxedOutput;
            job.MuxType = muxer.MuxerType;
            job.ContainerType = GetContainerType(job.Settings.MuxedOutput);
            job.Settings.Framerate = fps.Value;
            
            Debug.Assert(!splitting.Value.HasValue || splitting.Value.Value >= new FileSize(Unit.MB, 1));
            job.Settings.SplitSize = splitting.Value;
            return job;
        }

        public MuxJob Job
        {
            get { return GenerateMuxJob(); }
            set
            {
                if (value != null)
                    SetConfig(value.Settings.VideoInput, value.Settings.VideoName, value.Settings.MuxedInput, value.Settings.Framerate,
                    value.Settings.AudioStreams.ToArray(), value.Settings.SubtitleStreams.ToArray(),
                    value.Settings.ChapterInfo, value.Settings.MuxedOutput, value.Settings.SplitSize,
                    value.Settings.DAR, value.Settings.DeviceType);
            }
        }

        private void SetConfig(string videoInput, string videoName, string muxedInput, decimal? framerate, MuxStream[] audioStreams,
            MuxStream[] subtitleStreams, ChapterInfo chapterInfo, string output, FileSize? splitSize, Dar? dar, string deviceType)
        {
            base.SetConfig(videoInput, videoName, framerate, audioStreams, subtitleStreams, chapterInfo, output, splitSize, dar, deviceType);
            this.muxedInput.Filename = muxedInput;
            this.CheckIO();
        }

        protected override void ChangeOutputExtension()
        {
            foreach (ContainerType t in muxer.GetSupportedContainerOutputTypes())
            {
                if (output.Filename.ToLowerInvariant().EndsWith(t.Extension.ToLowerInvariant()))
                    return;
            }
            output.Filename = Path.ChangeExtension(output.Filename, muxer.GetSupportedContainerOutputTypes()[0].Extension);
        }

        private ContainerType GetContainerType(string outputFilename)
        {
            Debug.Assert(outputFilename != null);
            foreach (ContainerType t in muxer.GetSupportedContainerOutputTypes())
            {
                if (outputFilename.ToLowerInvariant().EndsWith(t.Extension.ToLowerInvariant()))
                    return t;
            }
            Debug.Assert(false);
            return null;
        }

        protected override bool IsFPSRequired()
        {
            if (vInput.Filename.Length > 0)
                return base.IsFPSRequired();
            else if (muxedInput.Filename.Length > 0)
                return false;
            else
                return true;
        }

        private void muxedInput_FileSelected(FileBar sender, FileBarEventArgs args)
        {
            CheckIO();
            FileUpdated();
        }
    }
}