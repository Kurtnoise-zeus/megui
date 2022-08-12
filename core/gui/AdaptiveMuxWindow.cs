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
using System.IO;
using System.Windows.Forms;

using MeGUI.core.details;
using MeGUI.core.details.mux;
using MeGUI.core.util;

namespace MeGUI
{
    public partial class AdaptiveMuxWindow : baseMuxWindow
    {
        private MuxProvider muxProvider;
        private bool minimizedMode = false;
        private VideoEncoderType knownVideoType;
        private AudioEncoderType[] knownAudioTypes;
        private ContainerType lastSelectedContainerType = null;

        public AdaptiveMuxWindow(): base(MainForm.Instance, null)
        {
            InitializeComponent();
            muxProvider = mainForm.MuxProvider;

            audioTracks[0].Filter = VideoUtil.GenerateCombinedFilter(ContainerManager.AudioTypes.ValuesArray);
            subtitleTracks[0].Filter = VideoUtil.GenerateCombinedFilter(ContainerManager.SubtitleTypes.ValuesArray);
            vInput.Filter = VideoUtil.GenerateCombinedFilter(ContainerManager.VideoTypes.ValuesArray);
            chapters.Filter = VideoUtil.GenerateCombinedFilter(ContainerManager.ChapterTypes.ValuesArray);
            cbContainer.Visible = true;
            lbContainer.Visible = true;

            subtitleTracks[0].chkDefaultStream.CheckedChanged += new System.EventHandler(base.chkDefaultStream_CheckedChanged);
            base.muxButton.Click += new System.EventHandler(this.muxButton_Click);
        }

        protected override void fileUpdated()
        {
            updatePossibleContainers();
        }

        protected override void upDeviceTypes()
        {
            updateDeviceTypes();
        }

        private void getTypes(out AudioEncoderType[] aCodec, out MuxableType[] audioTypes, out MuxableType[] subtitleTypes)
        {
            List<MuxableType> audioTypesList = new List<MuxableType>();
            List<MuxableType> subTypesList = new List<MuxableType>();
            List<AudioEncoderType> audioCodecList = new List<AudioEncoderType>();
            
            int counter = 0;
            foreach (MuxStreamControl c in audioTracks)
            {
                if (minimizedMode && knownAudioTypes.Length > counter)
                {
                    audioCodecList.Add(knownAudioTypes[counter]);
                }
                else if (c.Stream != null)
                {
                    MuxableType audioType = VideoUtil.guessAudioMuxableType(c.Stream.path, true);
                    if (audioType != null)
                    {
                        audioTypesList.Add(audioType);
                    }
                }
                counter++;
            }
            foreach (MuxStreamControl c in subtitleTracks)
            {
                if (c.Stream == null) continue;
                SubtitleType subtitleType = VideoUtil.guessSubtitleType(c.Stream.path);
                if (subtitleType != null)
                {
                    subTypesList.Add(new MuxableType(subtitleType, null));
                }
            }
            audioTypes = audioTypesList.ToArray();
            subtitleTypes = subTypesList.ToArray();
            aCodec = audioCodecList.ToArray();
        }


        protected override void checkIO()
        {
            base.checkIO();
            if (!(cbContainer.SelectedItem is ContainerType))
                muxButton.DialogResult = DialogResult.None;
        }

        private void updateDeviceTypes()
        {
            if (cbContainer.SelectedItem is ContainerType && lastSelectedContainerType == cbContainer.SelectedItem as ContainerType)
                return;
            
            if (cbContainer.SelectedItem is ContainerType)
            {
                lastSelectedContainerType = cbContainer.SelectedItem as ContainerType;
                output.Filter = (cbContainer.SelectedItem as ContainerType).OutputFilterString;
            }
            else
                output.Filter = "";

            output.Filename = Path.ChangeExtension(output.Filename, (this.cbContainer.SelectedItem as ContainerType).Extension);

            foreach (MuxStreamControl oStream in subtitleTracks)
            {
                if ((this.cbContainer.SelectedItem as ContainerType).Extension.Equals("mkv"))
                    oStream.ShowDefaultSubtitleStream = oStream.ShowForceSubtitleStream = oStream.ShowDelay = true;
                else
                    oStream.ShowDefaultSubtitleStream = oStream.ShowForceSubtitleStream = oStream.ShowDelay = false;
            }

            if (this.cbContainer.Text == "MKV")
            {
                this.cbType.Enabled = false;
                subtitleTracks[0].chkDefaultStream.Checked = true;
            }
            else
                this.cbType.Enabled = true;

            if (this.cbContainer.Text == "AVI")
                fps.Enabled = false;
            else
                fps.Enabled = true;

            List<DeviceType> supportedOutputDeviceTypes = this.muxProvider.GetSupportedDevices((ContainerType)cbContainer.SelectedItem);
            this.cbType.Items.Clear();
            this.cbType.Items.Add("Standard");
            this.cbType.Items.AddRange(supportedOutputDeviceTypes.ToArray());

            if (cbContainer.SelectedItem.ToString().Equals(mainForm.Settings.AedSettings.Container))
            {
                foreach (object o in cbType.Items) // I know this is ugly, but using the DeviceOutputType doesn't work unless we're switching to manual serialization
                {
                    if (o.ToString().Equals(mainForm.Settings.AedSettings.DeviceOutputType))
                    {
                        cbType.SelectedItem = o;
                        break;
                    }
                }
            }
            else
                this.cbType.SelectedIndex = 0;
        }

        private void updatePossibleContainers()
        {
            MuxableType videoType;
            if (minimizedMode)
                videoType = null;
            else
            {
                videoType = VideoUtil.guessVideoMuxableType(vInput.Filename, true);
                if (videoType != null && (videoType.codec == null || videoType.outputType == null))
                {
                    MessageBox.Show("Unable to determine type of input video. Mux-path finding cannot continue. Your video could well be corrupt.", "Determining type failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            MuxableType[] audioTypes;
            MuxableType[] subTypes;
            AudioEncoderType[] audioCodecs;
            getTypes(out audioCodecs, out audioTypes, out subTypes);

            List<MuxableType> allTypes = new List<MuxableType>();
            if (videoType != null)
                allTypes.Add(videoType);
            allTypes.AddRange(audioTypes);
            allTypes.AddRange(subTypes);

            List<ContainerType> supportedOutputTypes;
            if (minimizedMode)
                supportedOutputTypes = this.muxProvider.GetSupportedContainers(knownVideoType, audioCodecs, allTypes.ToArray());
            else
                supportedOutputTypes = this.muxProvider.GetSupportedContainers(allTypes.ToArray());

            if (supportedOutputTypes.Count > 0)
            {
                this.cbContainer.Items.Clear();
                this.cbContainer.Items.AddRange(supportedOutputTypes.ToArray());
                if (lastSelectedContainerType != null && cbContainer.Items.Contains(lastSelectedContainerType))
                    cbContainer.SelectedItem = lastSelectedContainerType;
                else
                    this.cbContainer.SelectedIndex = 0;
            }
            else
            {
                this.cbContainer.Items.Clear();
                MessageBox.Show("No muxer can be found that supports this input configuration", "Muxing impossible", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public JobChain Jobs
        {
            get
            {
                if (minimizedMode)
                    throw new Exception("Jobs property not accessible in minimized mode");
                
                VideoStream myVideo = new VideoStream();
                myVideo.Input = "";
                myVideo.Output = vInput.Filename;
                myVideo.NumberOfFrames = 1000; // Just a guess, since we have no way of actually knowing
                myVideo.VideoType = VideoUtil.guessVideoMuxableType(vInput.Filename, true);
                myVideo.Settings = new x264Settings();
                myVideo.Settings.NbBframes = 0;
                myVideo.Settings.VideoName = this.videoName.Text;

                MuxableType[] audioTypes;
                MuxableType[] subtitleTypes;
                AudioEncoderType[] audioCodecs;
                MuxStream[] audioStreams, subtitleStreams;
                getTypes(out audioCodecs, out audioTypes, out subtitleTypes);
                ChapterInfo chapterInfo;
                getAdditionalStreams(out audioStreams, out subtitleStreams, out chapterInfo);

                FileSize? splitSize = splitting.Value;

                MuxableType chapterInputType = new MuxableType(ChapterType.OGG_TXT, null);

                MuxableType deviceOutputType = null;
                if (!String.IsNullOrEmpty(this.cbType.Text))
                {
                    DeviceType type = VideoUtil.guessDeviceType(this.cbType.Text);
                    if (type != null)
                        deviceOutputType = new MuxableType(type, null);
                }

                return JobUtil.GenerateMuxJobs(myVideo, fps.Value, audioStreams, audioTypes, subtitleStreams,
                    subtitleTypes, new List<string>(), chapterInfo, chapterInputType, (cbContainer.SelectedItem as ContainerType), 
                    output.Filename, String.Empty, splitSize, new List<string>(), this.cbType.Text, deviceOutputType, false);
            }
        }
        /// <summary>
        /// sets the GUI to a minimal mode allowing to configure audio track languages, configure subtitles, and chapters
        /// the rest of the options are deactivated
        /// </summary>
        /// <param name="videoInput">the video input</param>
        /// <param name="framerate">the framerate of the video input</param>
        /// <param name="audioStreams">the audio streams whose languages have to be assigned</param>
        /// <param name="output">the output file</param>
        /// <param name="splitSize">the output split size</param>
        public void setMinimizedMode(string videoInput, string videoName, VideoEncoderType videoType, double framerate, MuxStream[] audioStreams, AudioEncoderType[] audioTypes, string output,
            FileSize? splitSize, ContainerType cft)
        {
            base.setConfig(videoInput, videoName, (decimal)framerate, audioStreams, new MuxStream[0], new ChapterInfo(), output, splitSize, null, null);

            minimizedMode = true;
            knownVideoType = videoType;
            knownAudioTypes = audioTypes;

            // disable controls where required
            videoGroupbox.Enabled = false;

            for (int i = 0; i < audioStreams.Length; ++i)
                audioTracks[i].SetAutoEncodeMode();

            this.output.Filename = output;
            this.splitting.Value = splitSize;
            this.muxButton.Text = "Go";
            updatePossibleContainers();
            if (this.cbContainer.Items.Contains(cft))
                cbContainer.SelectedItem = cft;
            checkIO();
        }

        public void getAdditionalStreams(out MuxStream[] audio, out MuxStream[] subtitles, out ChapterInfo chapters, out string output, out ContainerType cot)
        {
            cot = (cbContainer.SelectedItem as ContainerType);
            output = this.output.Filename;
            base.getAdditionalStreams(out audio, out subtitles, out chapters);
        }

        protected virtual void muxButton_Click(object sender, System.EventArgs e)
        {
            if (minimizedMode)
                return;

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
                    JobChain oJobs = this.Jobs;
                    if (oJobs.Jobs.Length == 0)
                    {
                        MessageBox.Show("No mux job created as input and output are the same", "Nothing to mux", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                    mainForm.Jobs.AddJobsWithDependencies(oJobs, true);
                    if (chkCloseOnQueue.Checked)
                        this.Close();
                    else
                        output.Filename = String.Empty;
                }
            }
        }
    }
}