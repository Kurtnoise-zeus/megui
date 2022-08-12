// ****************************************************************************
// 
// Copyright (C) 2005-2022 Doom9 & al
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
using MeGUI.core.plugins.interfaces;
using MeGUI.core.util;
using MeGUI.packages.tools.calculator;

namespace MeGUI
{
	/// <summary>
	/// Summary description for AutoEncode.
	/// </summary>
	public partial class AutoEncodeWindow : System.Windows.Forms.Form
	{
		#region variables
        private List<AudioJob> audioStreams;
        private bool prerender;
        private VideoInfo vInfo;
		private bool isBitrateMode = true;
        #endregion

        #region start / stop
        public AutoEncodeWindow()
		{
            InitializeComponent();
        }
        public AutoEncodeWindow(VideoStream videoStream, List<AudioJob> audioStreams, bool prerender, VideoInfo vInfo) : this()
        {
            this.vInfo = vInfo;
            this.videoStream = videoStream;
            this.audioStreams = audioStreams;
            this.prerender = prerender;
            muxProvider = MainForm.Instance.MuxProvider;
            container.Items.AddRange(muxProvider.GetSupportedContainers().ToArray());
            splitting.MinimumFileSize = new FileSize(Unit.MB, 1);

            if (MainForm.Instance.AutoEncodeLog == null)
                MainForm.Instance.AutoEncodeLog = MainForm.Instance.Log.Info("AutoEncode");
        }
        /// <summary>
        /// does the final initialization of the dialog
        /// gets all audio types from the audio streams, then asks the muxprovider for a list of containers it can mux the video and audio streams into
        /// if there is no muxer that can deliver any container for the video / audio combination, we can abort right away
        /// </summary>
        /// <returns>true if the given video/audio combination can be muxed to at least a single container, false if not</returns>
        public bool init()
        {
            List<AudioEncoderType> aTypes = new List<AudioEncoderType>();
            AudioEncoderType[] audioTypes;
            foreach (AudioJob stream in this.audioStreams)
            {
                if (stream.Settings != null && !String.IsNullOrEmpty(stream.Input) && !String.IsNullOrEmpty(stream.Output))
                    aTypes.Add(stream.Settings.EncoderType);
            }
            audioTypes = aTypes.ToArray();
            List<ContainerType> supportedOutputTypes = this.muxProvider.GetSupportedContainers(
                this.videoStream.Settings.EncoderType, audioTypes);
            
            if (supportedOutputTypes.Count <= 0)
                return false;

            this.container.Items.Clear();
            this.container.Items.AddRange(supportedOutputTypes.ToArray());
            this.container.SelectedIndex = 0;

            List<DeviceType> supportedOutputDeviceTypes = this.muxProvider.GetSupportedDevices((ContainerType)container.SelectedItem);
            this.device.Items.AddRange(supportedOutputDeviceTypes.ToArray());
            this.device.SelectedIndex = 0;

            if (this.container.Text == "MKV")
                this.device.Enabled = false;
            else
                this.device.Enabled = true;
                
            string muxedName = FileUtil.AddToFileName(vInfo.VideoOutput, "-muxed");

            this.muxedOutput.Filename = Path.ChangeExtension(muxedName, (this.container.SelectedItem as ContainerType).Extension);

            splitting.Value = MainForm.Instance.Settings.AedSettings.SplitSize;
            if (MainForm.Instance.Settings.AedSettings.FileSizeMode)
            {
                FileSizeRadio.Checked = true;
                targetSize.Value = MainForm.Instance.Settings.AedSettings.FileSize;
            }
            else if (MainForm.Instance.Settings.AedSettings.BitrateMode)
            {
                averageBitrateRadio.Checked = true;
                projectedBitrateKBits.Text = MainForm.Instance.Settings.AedSettings.Bitrate.ToString();
            }
            else
                noTargetRadio.Checked = true;
            if (MainForm.Instance.Settings.AedSettings.AddAdditionalContent)
                addSubsNChapters.Checked = true;
            foreach (object o in container.Items) // I know this is ugly, but using the ContainerType doesn't work unless we're switching to manual serialization
            {
                if (o.ToString().Equals(MainForm.Instance.Settings.AedSettings.Container))
                {
                    container.SelectedItem = o;
                    break;
                }
            }
            foreach (object o in device.Items) // I know this is ugly, but using the DeviceOutputType doesn't work unless we're switching to manual serialization
            {
                if (o.ToString().Equals(MainForm.Instance.Settings.AedSettings.DeviceOutputType))
                {
                    device.SelectedItem = o;
                    break;
                }
            }
            return true;
        }
        #endregion
        #region dropdowns
        /// <summary>
        /// adjusts the output extension when the container is being changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void container_SelectedIndexChanged(object sender, EventArgs e)
        {
            ContainerType cot = this.container.SelectedItem as ContainerType;
            this.muxedOutput.Filter = cot.OutputFilterString;
            if (!String.IsNullOrEmpty (muxedOutput.Filename))
            {
                if (this.container.Text == "MKV")
                    this.device.Enabled = false;
                else 
                    this.device.Enabled = true;

                List<DeviceType> supportedOutputDeviceTypes = this.muxProvider.GetSupportedDevices(cot);
                this.device.Items.Clear();
                this.device.Items.Add("Standard");
                this.device.Items.AddRange(supportedOutputDeviceTypes.ToArray());

                if (container.SelectedItem.ToString().Equals(MainForm.Instance.Settings.AedSettings.Container))
                {
                    foreach (object o in device.Items) // I know this is ugly, but using the DeviceOutputType doesn't work unless we're switching to manual serialization
                    {
                        if (o.ToString().Equals(MainForm.Instance.Settings.AedSettings.DeviceOutputType))
                        {
                            device.SelectedItem = o;
                            break;
                        }
                    }
                }
                else
                    this.device.SelectedIndex = 0;

                this.muxedOutput.Filename = Path.ChangeExtension(muxedOutput.Filename, (this.container.SelectedItem as ContainerType).Extension);
            }
        }
        #endregion
		#region additional events
		/// <summary>
		/// handles the selection of the output format
		/// in case of avi, if an encodeable audio stream is already present,
		/// the selection of additional streams needs to be completely disabled
		/// if not, it an be left enabled bt the text has to indicate the fact
		/// that you can only add an audio track and nothing else
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void outputFormat_CheckedChanged(object sender, System.EventArgs e)
		{
            ContainerType cot = this.container.SelectedItem as ContainerType;
            this.muxedOutput.Filename = Path.ChangeExtension(this.muxedOutput.Filename, cot.Extension);
        }
		/// <summary>
		/// separates encodable from muxable audio streams
		/// in addition to returning the two types separately an array of SubStreams is returned
		/// which is plugged into the muxer.. it contains the names of all the audio files
		/// that have to be muxed
		/// </summary>
		/// <param name="encodable">encodeable audio streams</param>
		/// <param name="muxable">muxable Audio Streams with the path filled out and a blank language</param>
        private void separateEncodableAndMuxableAudioStreams(out AudioJob[] encodable, out MuxStream[] muxable, out AudioEncoderType[] muxTypes)
		{
			encodable = AudioUtil.getConfiguredAudioJobs(audioStreams.ToArray()); // discards improperly configured ones
			// the rest of the job is all encodeable
			muxable = new MuxStream[encodable.Length];
            muxTypes = new AudioEncoderType[encodable.Length];
			int j = 0;
            foreach (AudioJob stream in encodable)
			{
                muxable[j] = stream.ToMuxStream();
                muxTypes[j] = stream.Settings.EncoderType;
				j++;
			}
		}
		#endregion
		#region helper methods
		/// <summary>
		/// sets the projected video bitrate field in the GUI
		/// </summary>
        private void setVideoBitrate()
        {
            try
            {
                CalcData data = GetCalcData();
                data.TotalSize = new FileSize(targetSize.Value.Value.Bytes);
                data.CalcByTotalSize();

                this.videoSize.Text = data.VideoSize.ToString();
                this.projectedBitrateKBits.Text = ((int)data.VideoBitrate).ToString();
            }
            catch (Exception)
            {
                this.projectedBitrateKBits.Text = "";
                videoSize.Text = "";
            }
        }

        private CalcData GetCalcData()
        {
            CalcData data = new CalcData((long)videoStream.NumberOfFrames, videoStream.Framerate, (ContainerType)container.SelectedItem,
                videoStream.Settings.Codec, videoStream.Settings.NbBframes > 0, getAudioStreamsForBitrate());
            return data;
        }

        private AudioBitrateCalculationStream[] getAudioStreamsForBitrate()
        {
            List<AudioBitrateCalculationStream> streams = new List<AudioBitrateCalculationStream>();
            foreach (AudioJob s in audioStreams)
                streams.Add(new AudioBitrateCalculationStream(s.Output));
            return streams.ToArray();
        }

		/// <summary>
		/// sets the size of the output given the desired bitrate
		/// </summary>
		private void setTargetSize()
		{
			try
            {
                CalcData data = GetCalcData();
                data.VideoBitrate = Int32.Parse(this.projectedBitrateKBits.Text);
                data.CalcByVideoSize();
                this.videoSize.Text = data.VideoSize.ToString();
                this.targetSize.Value = new FileSize(Unit.MB, data.TotalSize.MBExact);
            }
            catch (Exception)
            {
                videoSize.Text = "";
                targetSize.Value = null;
			}
		}
		#region audio
        /// <summary>
        /// sets the projected audio size for all audio streams that use CBR mode
        /// </summary>
        private void setAudioSize()
        {
            long[] sizes = new long[this.audioStreams.Count];
            int index = 0;
            foreach (AudioJob stream in this.audioStreams)
            {
                if (!string.IsNullOrEmpty(stream.Output)) // if we don't have the video length or the audio is not fully configured we can give up now
                {
                    long bytesPerSecond = 0;
                    if (stream.BitrateMode == BitrateManagementMode.CBR)
                    {
                        bytesPerSecond = stream.Settings.Bitrate * 1000 / 8;
                    }
                    double lengthInSeconds = (double)this.videoStream.NumberOfFrames / (double)this.videoStream.Framerate;
                    long sizeInBytes = (long)(lengthInSeconds * bytesPerSecond);
                    this.audioStreams[index].SizeBytes = sizeInBytes;
                    index++;
                }
            }
        }
		#endregion
		#endregion
		#region button events
		/// <summary>
		/// handles the go button for automated encoding
		/// checks if we're in automated 2 pass video mode
		/// then the video and audio configuration is checked, and if it checks out
		/// the audio job, video jobs and muxing job are generated, audio and video job are linked
		/// and encoding is started
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void queueButton_Click(object sender, System.EventArgs e)
		{
            if (String.IsNullOrEmpty(this.muxedOutput.Filename))
                return;

            FileSize? desiredSize = targetSize.Value;
            FileSize? splitSize = splitting.Value;
            LogItem log = new LogItem(this.muxedOutput.Filename);
            MainForm.Instance.AutoEncodeLog.Add(log);

            if (FileSizeRadio.Checked)
                log.LogValue("Desired Size", desiredSize);
            else if (averageBitrateRadio.Checked)
                log.LogValue("Projected Bitrate", string.Format("{0}kbps", projectedBitrateKBits.Text));
            else
                log.LogEvent("No Target Size (use profile settings)");

            log.LogValue("Split Size", splitSize);

            MuxStream[] audio;
            AudioJob[] aStreams;
            AudioEncoderType[] muxTypes;
			separateEncodableAndMuxableAudioStreams(out aStreams, out audio, out muxTypes);
			MuxStream[] subtitles = new MuxStream[0];
            ChapterInfo chapters = new ChapterInfo();
			string videoInput = vInfo.VideoInput;
            string videoOutput = vInfo.VideoOutput;
            string muxedOutput = this.muxedOutput.Filename;
            ContainerType cot = this.container.SelectedItem as ContainerType;

            // determine audio language 
            foreach (MuxStream stream in audio)
            {
                string strLanguage = LanguageSelectionContainer.GetLanguageFromFileName(Path.GetFileNameWithoutExtension(stream.path));
                if (!String.IsNullOrEmpty(strLanguage))
                    stream.language = strLanguage;
            }

            if (addSubsNChapters.Checked)
			{
                AdaptiveMuxWindow amw = new AdaptiveMuxWindow();
                amw.setMinimizedMode(videoOutput, "", videoStream.Settings.EncoderType, JobUtil.getFramerate(videoInput), audio,
                    muxTypes, muxedOutput, splitSize, cot);
                if (amw.ShowDialog() == DialogResult.OK)
                    amw.getAdditionalStreams(out audio, out subtitles, out chapters, out muxedOutput, out cot);
                else // user aborted, abort the whole process
                    return;
            }
            removeStreamsToBeEncoded(ref audio, aStreams);
            MainForm.Instance.Jobs.AddJobsWithDependencies(VideoUtil.GenerateJobSeries(videoStream, muxedOutput, aStreams, subtitles, new List<string>(), String.Empty, chapters,
                desiredSize, splitSize, cot, prerender, audio, log, device.Text, vInfo.Zones, null, null, false), true);
            this.Close();
		}

        /// <summary>
        /// Reallocates the audio array so that it only has the files to be muxed and not the files to be encoded, then muxed
        /// </summary>
        /// <param name="audio">All files to be muxed (including the ones which will be encoded first)</param>
        /// <param name="aStreams">All files being encoded (these will be removed from the audio array)</param>
        private void removeStreamsToBeEncoded(ref MuxStream[] audio, AudioJob[] aStreams)
        {
            List<MuxStream> newAudio = new List<MuxStream>();
            foreach (MuxStream stream in audio)
            {
                bool matchFound = false;
                foreach (AudioJob a in aStreams)
                {
                    if (stream.path == a.Output)
                    {
                        matchFound = true;              // found a file which needs to be encoded
                        a.Language = stream.language;   // set language
                        a.Name = stream.name;           // set track name
                        break;
                    }
                }
                if (!matchFound) // not found any files which will be encoded first to produce this file
                {
                    newAudio.Add(stream);
                }
            }
            audio = newAudio.ToArray();
        }
		#endregion
		#region event helper methods
		private void textField_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (! char.IsDigit(e.KeyChar) && (int)Keys.Back != (int)e.KeyChar)
				e.Handled = true;
		}

		private void containerOverhead_ValueChanged(object sender, System.EventArgs e)
		{
			if (isBitrateMode)
				this.setVideoBitrate();
			else
				this.setTargetSize();
		}
		private void projectedBitrate_TextChanged(object sender, System.EventArgs e)
		{
			if (!this.isBitrateMode)
				this.setTargetSize();
		}


		private void calculationMode_CheckedChanged(object sender, System.EventArgs e)
		{
			if (averageBitrateRadio.Checked)
			{
				targetSize.Enabled = false;
                this.targetSize.SelectedIndex = 3;
				this.projectedBitrateKBits.Enabled = true;
				this.isBitrateMode = false;
			}
            else if (noTargetRadio.Checked)
            {
                targetSize.Enabled = false;
                this.targetSize.SelectedIndex = 0;
                this.projectedBitrateKBits.Enabled = false;
                this.isBitrateMode = false;
            } 
            else
			{
				targetSize.Enabled = true;
                this.targetSize.SelectedIndex = 3;
				this.projectedBitrateKBits.Enabled = false;
				this.isBitrateMode = true;
			}
		}
		#endregion

        private void targetSize_SelectionChanged(object sender, string val)
        {
            if (isBitrateMode)
                this.setVideoBitrate();
        }
    }
    public class AutoEncodeTool : ITool
    {
        #region ITool Members

        public string Name
        {
            get { return "AutoEncode"; }
        }

        public void Run(MainForm info)
        {
            // normal video verification
            string error = null;
            if ((error = info.Video.verifyVideoSettings()) != null)
            {
                MessageBox.Show(error, "Unsupported video configuration", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            if ((error = info.Audio.verifyAudioSettings()) != null && !error.Equals("No audio input defined."))            {
                MessageBox.Show(error, "Unsupported audio configuration", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            if (info.Video.CurrentSettings.VideoEncodingType == VideoCodecSettings.VideoEncodingMode.twopass1
                || info.Video.CurrentSettings.VideoEncodingType == VideoCodecSettings.VideoEncodingMode.threepass1)
            {
                MessageBox.Show("First pass encoding is not supported for automated encoding as no output is generated.\nPlease choose another encoding mode", "Improper configuration",
                    MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            // close video player so that the AviSynth script is also closed
            info.ClosePlayer();

            JobUtil.GetInputProperties(info.Video.VideoInput, out ulong frameCount, out double frameRate);

            VideoCodecSettings vSettings = info.Video.CurrentSettings.Clone();
            Zone[] zones = info.Video.Info.Zones; // We can't simply modify the zones in place because that would reveal the final zones config to the user, including the credits/start zones
            bool cont = JobUtil.GetFinalZoneConfiguration(vSettings, info.Video.Info.IntroEndFrame, info.Video.Info.CreditsStartFrame, ref zones, (int)frameCount);
            if (cont)
            {
                VideoStream myVideo = new VideoStream();
                myVideo.Input = info.Video.Info.VideoInput;
                myVideo.Output = info.Video.Info.VideoOutput;
                myVideo.NumberOfFrames = frameCount;
                myVideo.Framerate = (decimal)frameRate;
                myVideo.VideoType = info.Video.CurrentMuxableVideoType;
                myVideo.Settings = vSettings;
                
                VideoInfo vInfo = info.Video.Info.Clone(); // so we don't modify the data on the main form
                vInfo.Zones = zones;

                using (AutoEncodeWindow aew = new AutoEncodeWindow(myVideo, info.Audio.AudioStreams, info.Video.PrerenderJob, vInfo))
                {
                    if (aew.init())
                    {
                        aew.ShowDialog();
                    }
                    else
                        MessageBox.Show("The currently selected combination of video and audio output cannot be muxed", "Unsupported configuration", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }           
        }

        public Shortcut[] Shortcuts
        {
            get { return new Shortcut[] { Shortcut.CtrlF6 }; }
        }

        #endregion

        #region IIDable Members

        public string ID
        {
            get { return "AutoEncode"; }
        }

        #endregion
    }
}