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
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

using MeGUI.core.util;

namespace MeGUI.core.gui
{
    public partial class AudioEncodingTab : UserControl
    {
        private AudioEncoderProvider audioEncoderProvider = new AudioEncoderProvider();

        /// <summary>
        /// This delegate is called when a job has been successfully configured and the queue button is pressed
        /// </summary>
        public Setter<AudioJob> QueueJob;
        private bool bShowReset = true;

        public string QueueButtonText
        {
            get { return queueAudioButton.Text; }
            set { queueAudioButton.Text = value; }
        }

        public string AudioContainer
        {
            get { return audioContainer.Text; }
            set { audioContainer.Text = value; }
        }

        public bool ShowReset
        {
            get { return bShowReset; }
            set
            {
                bShowReset = value;
                deleteAudioButton.Visible = value;
            }
        }

        public AudioEncoderProvider AudioEncoderProvider
        {
            get { return audioEncoderProvider; }
        }

        private string AudioInput
        {
            get { return audioInput.Filename; }
            set { audioInput.Filename = value; }
        }

        private string AudioOutput
        {
            get { return audioOutput.Filename; }
            set { audioOutput.Filename = value; }
        }

        /// <summary>
        /// returns the audio codec settings for the currently active audio codec
        /// </summary>
        public AudioCodecSettings AudCodecSettings
        {
            get
            {
                return (AudioCodecSettings)audioProfile.SelectedProfile.BaseSettings;
            }
            private set
            {
                audioProfile.SetSettings(value);
            }
        }

        public string verifyAudioSettings()
        {
            int inputDelay = PrettyFormatting.getDelay(AudioInput) ?? 0;
            int? delay = PrettyFormatting.getDelay(AudioOutput);
            if (delay != null && delay.Value != (inputDelay - (int)this.delay.Value))
            {
                if (MessageBox.Show(string.Format(
                    "The output filename suggests the audio delay is {0}ms. " +
                    "However, the input delay is {1}ms and the delay correction is {2}ms, " +
                    "so a delay of {3}ms is more appropriate. Do you want MeGUI to correct " +
                    "the output filename before adding the job?", delay.Value, inputDelay,
                    this.delay.Value, (inputDelay - (int)this.delay.Value)),
                    "Output filename suggests wrong delay", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    AudioOutput = PrettyFormatting.ReplaceDelay(AudioOutput, (inputDelay - (int)this.delay.Value));
            }

            AudioJob stream;

            try
            {
                stream = AudioJob;
            }
            catch (MeGUIException m)
            {
                return m.Message;
            }

            if (stream == null)
                return "Audio input, audio output, and audio settings must all be configured";

            string fileErr = MainForm.verifyOutputFile(this.AudioOutput);
            if (fileErr != null)
            {
                return "Problem with audio output filename:\n" + fileErr;
            }
            AudioType aot = this.audioContainer.SelectedItem as AudioType;
            // test output file extension
            if (!Path.GetExtension(this.AudioOutput).Replace(".", "").Equals(aot.Extension, StringComparison.InvariantCultureIgnoreCase))
            {
                return "Audio output filename does not have the correct extension.\nBased on current settings, it should be "
                    + aot.Extension;
            }
            return null;
        }
        
        
        public AudioEncodingTab()
        {
            InitializeComponent();
            if (MainForm.Instance != null) // Fix to allow VS2008 designer to load Form1
                audioProfile.Manager = MainForm.Instance.Profiles;
        }


        private void audioInput_FileSelected(FileBar sender, FileBarEventArgs args)
        {
            if (string.IsNullOrEmpty(audioInput.Filename))
                return;

            MediaInfoFile iFile = new MediaInfoFile(audioInput.Filename);
            if (!iFile.HasAudio)
            {
                audioInput.Filename = String.Empty;
                audioOutput.Filename = String.Empty;
                MessageBox.Show("This file cannot be processed", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
                openAudioFile(audioInput.Filename);
        }

        private bool bInitialStart = true;
        private void audioContainer_SelectedIndexChanged(object sender, EventArgs e)
        {
            AudioType currentType = (AudioType)audioContainer.SelectedItem;
            audioOutput.Filter = currentType.OutputFilterString;
            AudioOutput = Path.ChangeExtension(AudioOutput, currentType.Extension);
            if (!String.IsNullOrEmpty(audioInput.Filename))
                if (audioInput.Filename.Equals(audioOutput.Filename) || File.Exists(AudioOutput))
                    AudioOutput = Path.Combine(Path.GetDirectoryName(AudioOutput), Path.GetFileNameWithoutExtension(AudioOutput) + "_new." + currentType.Extension);
            if (!bInitialStart)
                MainForm.Instance.Settings.MainAudioFormat = audioContainer.Text;
            else
                bInitialStart = false;
        }

        private void deleteAudioButton_Click(object sender, EventArgs e)
        {
            Reset();
        }

        private void queueAudioButton_Click(object sender, EventArgs e)
        {
            string settingsError = verifyAudioSettings();
            if (settingsError != null)
            {
                MessageBox.Show(settingsError, "Unsupported configuration", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            QueueJob(AudioJob);
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public AudioJob AudioJob
        {
            get
            {
                if (string.IsNullOrEmpty(AudioInput) ||
                    string.IsNullOrEmpty(AudioOutput) ||
                    AudCodecSettings == null)
                    return null;

                return new AudioJob(this.AudioInput, this.AudioOutput, this.cuts.Filename,
                    this.AudCodecSettings, (int)delay.Value, null, null);
            }
            set
            {
                AudioInput = value.Input;
                AudioOutput = value.Output;
                AudCodecSettings = value.Settings;
                cuts.Filename = value.CutFile;
                delay.Value = value.Delay;
            }
        }

        #region helper methods
        public void openAudioFile(string fileName)
        {
            AudioInput = fileName;
            delay.Value = PrettyFormatting.getDelayAndCheck(fileName) ?? 0;

            try
            {
                AudioOutput = FileUtil.AddToFileName(PrettyFormatting.ReplaceDelay(fileName, 0), MainForm.Instance.Settings.AudioExtension);
            }
            catch (Exception e)
            {
               throw new ApplicationException("The value detected as delay in your filename seems to be too high/low for MeGUI." +
                                              "Try to recreate it with the appropriate tools." + e.Message, e);
            }

            string projectPath = FileUtil.GetOutputFolder(fileName);
            AudioOutput = Path.Combine(projectPath, Path.GetFileName(AudioOutput));
            
            audioContainer_SelectedIndexChanged(null, null);         
        }

        internal Size FileTypeComboBoxSize
        {
            get { return audioContainer.Size; }
            set { audioContainer.Size = value; }
        }

        internal void Reset()
        {
            this.AudioInput = "";
            this.cuts.Filename = "";
            this.AudioOutput = "";
            this.delay.Value = 0;
        }
        #endregion


        private AudioEncoderType lastCodec = null;
        private void audioProfile_SelectedProfileChanged(object sender, EventArgs e)
        {
            if (lastCodec == AudCodecSettings.EncoderType)
                return;

            lastCodec = AudCodecSettings.EncoderType;
            Util.ChangeItemsKeepingSelectedSame(audioContainer, AudioEncoderProvider.GetSupportedOutput(lastCodec));
        }

    }
}