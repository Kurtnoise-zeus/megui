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
using System.IO;
using System.Windows.Forms;

using MeGUI.core.details;
using MeGUI.core.gui;
using MeGUI.core.util;

namespace MeGUI
{
    public partial class VideoEncodingComponent : UserControl
    {
        #region video info
        private VideoInfo info; 
        private VideoPlayer player; // window that shows a preview of the video
        private VideoEncoderProvider videoEncoderProvider = new VideoEncoderProvider();

        private void InitVideoInfo()
        {
            info = new VideoInfo();
            info.VideoInputChanged += new StringChanged(delegate(object _, string val)
            {
                videoInput.Filename = val;
            });
            info.VideoOutputChanged += new StringChanged(delegate(object _, string val)
            {
                videoOutput.Filename = val;
            });
        }

        public VideoInfo Info
        {
            get
            {
                return info;
            }
        }

        public string FileType
        {
            get
            {
                return fileType.Text; ;
            }
            set
            {
                fileType.Text = value;
            }
        }
        #endregion
        #region generic handlers: filetype, profiles and codec. Also, encoder provider

        public VideoCodecSettings CurrentSettings
        {
            get
            {
                return (VideoCodecSettings)videoProfile.SelectedProfile.BaseSettings;
            }
        }
	
        public VideoEncodingComponent()
        {
            InitVideoInfo();
            InitializeComponent();
            if (MainForm.Instance != null)  // Fix to allow VS2008 designer to load Form1
                videoProfile.Manager = MainForm.Instance.Profiles;
        }
        #endregion

        #region extra properties
        public string VideoInput
        {
            get { return info.VideoInput; }
            set { info.VideoInput = value; }
        }

        public string VideoOutput
        {
            get { return info.VideoOutput; }
            set { info.VideoOutput = value; }
        }

        public VideoType CurrentVideoOutputType
        {
            get { return this.fileType.SelectedItem as VideoType; }
        }
        public bool PrerenderJob
        {
            get { return addPrerenderJob.Checked; }
            set { addPrerenderJob.Checked = value; }
        }
        #endregion

        #region event handlers
        private void videoInput_FileSelected(FileBar sender, FileBarEventArgs args)
        {
            if (!string.IsNullOrEmpty(videoInput.Filename) && !MainForm.Instance.openFile(videoInput.Filename))
            {
                videoInput.Filename = String.Empty;
                videoOutput.Filename = String.Empty;
            }
        }

        private void videoOutput_FileSelected(FileBar sender, FileBarEventArgs args)
        {
            info.VideoOutput = videoOutput.Filename;
        }

        /// <summary>
        /// opens the AviSynth preview for a given AviSynth script
        /// gets the properties of the video, registers the callbacks, updates the video bitrate (we know the lenght of the video now) and updates the commandline
        /// with the scriptname
        /// </summary>
        /// <param name="fileName">the AviSynth script to be opened</param>
        private void OpenAvisynthScript(string fileName)
        {
            if (this.player != null) // make sure only one preview window is open at all times
                player.Close();
            player = new VideoPlayer();
            info.DAR = null; // to be sure to initialize DAR values
            bool videoLoaded = player.LoadVideo(fileName, PREVIEWTYPE.CREDITS, true);
            if (videoLoaded)
            {
                info.DAR = info.DAR ?? player.File.VideoInfo.DAR;
                player.DAR = info.DAR; 
                player.IntroCreditsFrameSet += new IntroCreditsFrameSetCallback(player_IntroCreditsFrameSet);
                player.Closed += new EventHandler(player_Closed);
                player.Show();
                player.SetScreenSize();
                if (MainForm.Instance.Settings.AlwaysOnTop)
                    player.TopMost = true;
            }
        }

        private void VideoInput_DoubleClick(object sender, System.EventArgs e)
        {
            if (!string.IsNullOrEmpty(VideoInput))
            {
                this.OpenAvisynthScript(VideoInput);
                if (info.CreditsStartFrame > -1)
                    this.player.CreditsStart = info.CreditsStartFrame;
                if (info.IntroEndFrame > -1)
                    this.player.IntroEnd = info.IntroEndFrame;
            }
            else
                MessageBox.Show("Load an avisynth script first...", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private void queueVideoButton_Click(object sender, System.EventArgs e)
        {
            fileType_SelectedIndexChanged(sender, e); // to select always correct output file extension
            string settingsError = verifyVideoSettings();  // basic input, logfile and output file settings are okay
            if (settingsError != null)
            {
                MessageBox.Show(settingsError, "Unsupported configuration", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            VideoCodecSettings vSettings = this.CurrentSettings.Clone();

            string videoOutput = info.VideoOutput;

            // special handling as the encoders cannot output all desired container types
            if (!fileType.Text.StartsWith("RAW") 
                && (!vSettings.SettingsID.Equals("x264") || !fileType.Text.Equals("MKV") || MainForm.Instance.Settings.UseExternalMuxerX264))
            {
                if (vSettings.SettingsID.Equals("x264"))
                    videoOutput = Path.ChangeExtension(videoOutput, "264");
                else if (vSettings.SettingsID.Equals("x265"))
                    videoOutput = Path.ChangeExtension(videoOutput, "hevc");
                else if (vSettings.SettingsID.Equals("XviD"))
                    videoOutput = Path.ChangeExtension(videoOutput, "m4v");
                else if (vSettings.SettingsID.Equals("SVT-AV1-PSY"))
                    videoOutput = Path.ChangeExtension(videoOutput, "ivf");
            }

            JobUtil.GetInputProperties(info.VideoInput, out ulong frameCount, out _);

            JobChain prepareJobs = JobUtil.AddVideoJobs(info.VideoInput, videoOutput, this.CurrentSettings.Clone(),
                info.IntroEndFrame, info.CreditsStartFrame, info.DAR, PrerenderJob, info.Zones, (int)frameCount);

            if (!fileType.Text.StartsWith("RAW") && 
                vSettings.VideoEncodingType != MeGUI.VideoCodecSettings.VideoEncodingMode.twopass1 && vSettings.VideoEncodingType != MeGUI.VideoCodecSettings.VideoEncodingMode.threepass1
                && (!vSettings.SettingsID.Equals("x264") || !fileType.Text.Equals("MKV") || MainForm.Instance.Settings.UseExternalMuxerX264)
                && (!vSettings.SettingsID.Equals("FFV1")))
            {
                // create mux job
                MuxJob mJob = new MuxJob
                {
                    Input = videoOutput
                };

                if (vSettings.SettingsID.Equals("XviD"))
                {
                    mJob.MuxType = MuxerType.FFMPEG;
                    mJob.Output = Path.ChangeExtension(videoOutput, "avi");
                    if (fileType.Text.Equals("MKV"))
                        mJob.Output = Path.ChangeExtension(videoOutput, "mkv");
                }
                else if (fileType.Text.Equals("MKV"))
                {
                    mJob.MuxType = MuxerType.MKVMERGE;
                    mJob.Output = Path.ChangeExtension(videoOutput, "mkv");
                }
                else
                {
                    mJob.MuxType = MuxerType.MP4BOX;
                    mJob.Output = Path.ChangeExtension(videoOutput, "mp4");
                }

                mJob.Settings.MuxAll = true;
                mJob.Settings.MuxedInput = mJob.Input;
                mJob.Settings.MuxedOutput = mJob.Output;
                mJob.FilesToDelete.Add(videoOutput);

                // add job to queue
                prepareJobs = new SequentialChain(prepareJobs, new SequentialChain(mJob));
            }
            MainForm.Instance.Jobs.AddJobsWithDependencies(prepareJobs, true);
        }

        private bool bInitialStart = true;
        private void fileType_SelectedIndexChanged(object sender, EventArgs e)
        {
            videoOutput.Filter = CurrentVideoOutputType.OutputFilterString;
            this.VideoOutput = Path.ChangeExtension(this.VideoOutput, CurrentVideoOutputType.Extension);
            if (!bInitialStart)
                MainForm.Instance.Settings.MainFileFormat = fileType.Text;
            else
                bInitialStart = false;
        }
        /// <summary>
        /// enables / disables output fields depending on the codec configuration
        /// </summary>
        private void UpdateIOConfig()
        {
            VideoCodecSettings.VideoEncodingMode encodingMode = CurrentSettings.VideoEncodingType;
            if (encodingMode == VideoCodecSettings.VideoEncodingMode.twopass1 
                || encodingMode == VideoCodecSettings.VideoEncodingMode.threepass1) // first pass
            {
                videoOutput.Enabled = false;
            }
            else if (!videoOutput.Enabled)
            {
                videoOutput.Enabled = true;
            }
        }
        #endregion
        #region verification
        /// <summary>
        /// verifies the input, output and logfile configuration
        /// based on the codec and encoding mode certain fields must be filled out
        /// </summary>
        /// <returns>null if no error; otherwise string error message</returns>
        public string verifyVideoSettings()
        {
            // test for valid input filename
            if (String.IsNullOrEmpty(this.VideoInput))
                return "Please specify a video input file";

            string fileErr = MainForm.verifyInputFile(this.VideoInput);
            if (fileErr != null)
            {
                return "Problem with video input filename:\n" + fileErr;
            }

            // test for valid output filename (not needed if just doing 1st pass)
            if (!IsFirstPass())
            {
                fileErr = MainForm.verifyOutputFile(this.VideoOutput);
                if (fileErr != null)
                {
                    return "Problem with video output filename:\n" + fileErr;
                }

                VideoType vot = CurrentVideoOutputType;
                // test output file extension
                if (!Path.GetExtension(this.VideoOutput).Replace(".", "").Equals(vot.Extension, StringComparison.InvariantCultureIgnoreCase))
                {
                    return "Video output filename does not have the correct extension.\nBased on current settings, it should be "
                        + vot.Extension;
                }
            }
            return null;
        }
        #endregion
        #region helpers
        public MuxableType CurrentMuxableVideoType
        {
            get { return new MuxableType(CurrentVideoOutputType, CurrentSettings.Codec); }
        }

        public void openVideoFile(string fileName, MediaInfoFile oNewInfo)
        {
            if (oNewInfo == null)
            {
                using (MediaInfoFile oInfo = new MediaInfoFile(fileName))
                    info.DAR = oInfo.VideoInfo.DAR;
            }
            else
                info.DAR = oNewInfo.VideoInfo.DAR;

            info.CreditsStartFrame = -1;
            info.IntroEndFrame = -1;
            info.VideoInput = fileName;
            info.Zones = null;

            if (MainForm.Instance.Settings.AutoOpenScript)
                OpenAvisynthScript(fileName);

            string filePath = FileUtil.GetOutputFolder(fileName);
            string fileNameNoExtension = Path.GetFileNameWithoutExtension(fileName);
            this.VideoOutput = Path.Combine(filePath, fileNameNoExtension) + MainForm.Instance.Settings.VideoExtension + ".extension";
            this.VideoOutput = Path.ChangeExtension(this.VideoOutput, this.CurrentVideoOutputType.Extension);
            UpdateIOConfig();
        }

        private bool IsFirstPass()
        {
            VideoCodecSettings settings = CurrentSettings;
            if (settings.VideoEncodingType == VideoCodecSettings.VideoEncodingMode.twopass1
                || settings.VideoEncodingType == VideoCodecSettings.VideoEncodingMode.threepass1)
                return true;
            else
                return false;
        }

        #endregion
        #region player info

        internal void ClosePlayer()
        {
            if (this.player != null)
            {
                player.Close();
                player = null;
            }
        }

        internal void hidePlayer()
        {
            player?.Hide();
        }

        internal void showPlayer()
        {
            player?.Show();
        }
        /// <summary>
        /// callback for the video player window closing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void player_Closed(object sender, EventArgs e)
        {
            this.player = null;
        }
        /// <summary>
        /// sets the intro end / credits start frame
        /// </summary>
        /// <param name="frameNumber">the frame number</param>
        /// <param name="isCredits">true if the credits start frame is to be set, false if the intro end is to be set</param>
        private void player_IntroCreditsFrameSet(int frameNumber, bool isCredits)
        {
            if (isCredits)
            {
                if (ValidateCredits(frameNumber))
                {
                    player.CreditsStart = frameNumber;
                    info.CreditsStartFrame = frameNumber;
                }
                else
                    player.CreditsStart = -1;
            }
            else
            {
                if (ValidateIntro(frameNumber))
                {
                    info.IntroEndFrame = frameNumber;
                    player.IntroEnd = frameNumber;
                }
                else
                    player.IntroEnd = -1;
            }
        }
        /// <summary>
        /// iterates through all zones and makes sure we get no intersection by applying the current credits settings
        /// </summary>
        /// <param name="creditsStartFrame">the credits start frame</param>
        /// <returns>returns true if there is no intersetion between zones and credits and false if there is an intersection</returns>
        private bool ValidateCredits(int creditsStartFrame)
        {
            foreach (Zone z in Info.Zones)
            {
                if (creditsStartFrame <= z.endFrame) // credits start before end of this zone -> intersection
                {
                    MessageBox.Show("The start of the end credits intersects with an already configured zone\ngoing from frame " + z.startFrame + " to frame " + z.endFrame +
                        "\nPlease select another credits start frame or reconfigure the zone in the codec configuration.", "Zone intersection detected",
                        MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// iteratees through all zones and makes sure we get no intersection by applying the current intro settings
        /// </summary>
        /// <param name="introEndFrame">the frame where the intro ends</param>
        /// <returns>true if the intro zone does not interesect with a zone, false otherwise</returns>
        private bool ValidateIntro(int introEndFrame)
        {
            foreach (Zone z in Info.Zones)
            {
                if (introEndFrame >= z.startFrame)
                {
                    MessageBox.Show("The end of the intro intersects with an already configured zone\ngoing from frame " + z.startFrame + " to frame " + z.endFrame +
                        "\nPlease select another credits start frame or reconfigure the zone in the codec configuration.", "Zone intersection detected",
                        MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return false;
                }
            }
            return true;
        }
        #endregion
        #region misc
        public VideoEncoderProvider VideoEncoderProvider
        {
            get { return videoEncoderProvider; }
        }

        internal void Reset()
        {
            this.VideoInput = "";
            this.VideoOutput = "";
            info.CreditsStartFrame = 0;
            info.Zones = null;
        }
        #endregion

        private void addAnalysisPass_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(VideoInput))
            {
                MessageBox.Show("Error: Could not add job to queue. Make sure that all the details are entered correctly", "Couldn't create job", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            AviSynthJob job = new AviSynthJob(VideoInput);
            MainForm.Instance.Jobs.AddJobsToQueue(job);
        }

        VideoEncoderType lastCodec = null;
        private void videoProfile_SelectedProfileChanged(object sender, EventArgs e)
        {
            UpdateIOConfig();

            if (CurrentSettings.EncoderType == lastCodec)
                return;

            lastCodec = CurrentSettings.EncoderType;

            VideoType[] vArray = videoEncoderProvider.GetSupportedOutput(lastCodec);
            if (lastCodec == VideoEncoderType.XVID)
            {
                Array.Resize(ref vArray, vArray.Length + 2);
                vArray[vArray.Length - 2] = VideoType.AVI;
                vArray[vArray.Length - 1] = VideoType.MKV;
            }
            else if (lastCodec == VideoEncoderType.X264)
            {
                Array.Resize(ref vArray, vArray.Length + 1);
                vArray[vArray.Length - 1] = VideoType.MP4;
            }
            else if (lastCodec == VideoEncoderType.X265)
            {
                Array.Resize(ref vArray, vArray.Length + 2);
                vArray[vArray.Length - 2] = VideoType.MKV;
                vArray[vArray.Length - 1] = VideoType.MP4;
            }

            Util.ChangeItemsKeepingSelectedSame(fileType, vArray);
        }

        private void editZonesButton_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(videoInput.Filename))
            {
                MessageBox.Show("Load an avisynth script first...", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            ClosePlayer();
            using (ZonesWindow zw = new ZonesWindow(VideoInput))
            {
                zw.Zones = Info.Zones;
                if (zw.ShowDialog() == DialogResult.OK)
                    Info.Zones = zw.Zones;
            }
        }

        private void videopreview_Click(object sender, EventArgs e)
        {
            VideoInput_DoubleClick(null, null);
        }
    }
}