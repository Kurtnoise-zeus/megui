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
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

using MeGUI.core.details;
using MeGUI.core.util;
using MeGUI.packages.tools.calculator;
using MeGUI.Properties;

namespace MeGUI
{ 
	/// <summary>
	/// Summary description for Calculator.
	/// </summary>
	partial class Calculator : System.Windows.Forms.Form
	{
        private bool updatingContainers = false;
        private MainForm mainForm;
        private MuxProvider muxProvider;
        private CodecManager codecs = new CodecManager();
        private bool isUpdating = false;
        private bool calculating = false;
        
		public Calculator(MainForm mainForm) : this()
		{
            this.mainForm = mainForm;
            this.muxProvider = mainForm.MuxProvider;
            this.videoCodec.Items.AddRange(CodecManager.VideoEncoderTypes.ValuesArray);
            videoCodec.SelectedItem = CodecManager.VideoEncoderTypes["x264"];
            this.containerFormat.Items.AddRange(muxProvider.GetSupportedContainers().ToArray());
            containerFormat.SelectedItem = ContainerType.MKV;

            // set complexity from settings
            this.complexity.Minimum = 0;
            this.complexity.Maximum = 100;
            this.complexity.Value = (mainForm.Settings.MinComplexity + mainForm.Settings.MaxComplexity) / 2;
            this.complexity.Minimum = mainForm.Settings.MinComplexity;
            this.complexity.Maximum = mainForm.Settings.MaxComplexity;

            // wire the input boxes to auto-select text on focus
            AddAutoSelectHandler(this);

            // set focus of calculate by
            this.averageBitrateRadio.Click += (s, args) => projectedBitrate.Focus();
            this.bppRadio.Click += (s, args) => bpp.Focus();
            this.qEstRadio.Click += (s, args) => qest.Focus();
            this.fileSizeRadio.Click += (s, args) => targetSize.Focus();

            // smart focus for audio and extras (try to fix render bug w/ scrollbar)
            this.audioExtraFlow.ControlRemoved += (s, args) =>
            {
                int idx = (int)args.Control.Tag;
                if (idx > 0) audioExtraFlow.Controls[idx - 1].Focus();
                TagIndexes();
            };
            this.audioExtraFlow.ControlAdded += (s, args) =>
            {
                args.Control.Focus();
                TagIndexes();
            };
		}

		/// <summary>
		/// sets video, audio and codec defaults
		/// </summary>
		/// <param name="nbFrames">number of frames of the video source</param>
		/// <param name="framerate">framerate of the video source</param>
		/// <param name="codec">codec selected</param>
		/// <param name="container">container</param>
		/// <param name="audio1Bitrate">bitrate of the first audio track</param>
		/// <param name="audio2Bitrate">bitrate of the second audio track</param>
        public void SetDefaults(ulong nbFrames, double framerate, int hRes, int vRes, VideoCodecSettings vSettings, List<AudioJob> audioStreams)
		{
            fpsChooser.Value = (decimal)framerate;

			if (nbFrames > 0) this.nbFrames.Value = nbFrames;

            if (hRes > 0) this.width.Value = hRes;
            if (vRes > 0) this.height.Value = vRes;

            if (vSettings != null)
            {
                bframes.Checked = vSettings.NbBframes > 0;

                if (videoCodec.Items.Contains(vSettings.EncoderType))
                    videoCodec.SelectedItem = vSettings.EncoderType;
            }

            foreach (AudioJob job in audioStreams)
            {
                var a = AddAudio();
                a.SetAudioJob(job);
            }

            // make sure there is at least one audio displayed
            if (GetAudioStreams().Count() == 0) AddAudio();
		}


        public VideoEncoderType SelectedVCodec
        {
            get { return (VideoEncoderType)videoCodec.SelectedItem; }
        }

        public int VideoBitrate
        {
            get { return (int)projectedBitrate.Value; }
        }

        #region Event Handlers

        private void audio_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            UpdateContainers();
            Calculate();
        }

		private void textField_TextChanged(object sender, System.EventArgs e)
		{
            lock(this)
            {
                if(sender is NumericUpDown && !this.isUpdating)
                {
                    this.isUpdating = true;
                    NumericUpDown tb = (NumericUpDown)sender;
                    decimal value = tb.Value;
                    if (tb == totalSeconds)
                    {
                        int hours = (int)value / 3600;
                        value -= hours * 3600;
                        int minutes = (int)value / 60;
                        value -= minutes * 60;
                        if (hours <= this.hours.Maximum)
                        {
                            this.hours.Value = hours;
                            this.minutes.Value = minutes;
                            this.seconds.Value = value;
                        }
                        else // We set to max available time and set frames accordingly
                        {
                            this.hours.Value = this.hours.Maximum;
                            this.minutes.Value = this.minutes.Maximum - 1; //59 mins
                            this.seconds.Value = this.seconds.Maximum - 1; //59 seconds
                        }
                        UpdateTotalSeconds();
                        SetAudioLength();
                        UpdateTotalFrames();

                    }
                    else if (tb == nbFrames)
                    {
                        int secs = (int)(value / fpsChooser.Value);
                        totalSeconds.Text = secs.ToString();
                        int hours = secs / 3600;
                        secs -= hours * 3600;
                        int minutes = secs / 60;
                        secs -= minutes * 60;
                        if (hours < this.hours.Maximum)
                        {
                            this.hours.Value = hours;
                            this.minutes.Value = minutes;
                            this.seconds.Value = secs;
                        }
                        else //Set to max available time and set frames accordingly
                        {
                            this.hours.Value = this.hours.Maximum;
                            this.minutes.Value = this.minutes.Maximum - 1; //59 minutes
                            this.seconds.Value = this.seconds.Maximum - 1; //59 seconds
                            UpdateTotalFrames();
                        }
                        UpdateTotalSeconds();
                        SetAudioLength();
                    }
                    else if (tb == projectedBitrate)
                    {
                        if (averageBitrateRadio.Checked) // only do something here if we're in size calculation mode
                            Calculate();
                    }
                    tb.Select(tb.Text.Length, 0);
                    if((averageBitrateRadio.Checked
                        && tb != projectedBitrate)
                        || !averageBitrateRadio.Checked)
                        Calculate();

                    this.isUpdating = false;
                }
            }
		}

        private void time_ValueChanged(object sender, System.EventArgs e)
        {
            lock (this)
            {
                if (isUpdating)
                    return;

                this.isUpdating = true;
                NumericUpDown ud = (NumericUpDown)sender;

                if (this.hours.Value.Equals(this.hours.Maximum))
                {
                    if (this.minutes.Value == 60)
                    {
                        this.minutes.Value = 59;
                        UpdateTotalSeconds();
                        UpdateTotalFrames();
                        isUpdating = false;
                        return; // we can't increase the time
                    }
                    else if (this.seconds.Value == 60 && this.minutes.Value == 59)
                    {
                        this.seconds.Value = 59;
                        UpdateTotalSeconds();
                        UpdateTotalFrames();
                        isUpdating = false;
                        return; // we can't increase the time
                    }
                }
                if (ud.Value == 60) // time to wrap
                {
                    ud.Value = 0;
                    if (ud == seconds)
                    {
                        if (minutes.Value == 59)
                        {
                            minutes.Value = 0;
                            if (!this.hours.Value.Equals(this.hours.Maximum))
                                hours.Value += 1;
                        }
                        else
                            minutes.Value += 1;
                    }
                    else if (ud == minutes)
                    {
                        minutes.Value = 0;
                        if (this.hours.Value < this.hours.Maximum)
                            hours.Value += 1;
                    }
                }
                UpdateTotalSeconds();
                SetAudioLength();
                UpdateTotalFrames();
                Calculate();

                this.isUpdating = false;
            }
        }

		/// <summary>
		/// handles the change in between bitrate and size calculation mode
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void calculationMode_CheckedChanged(object sender, System.EventArgs e)
		{
			RadioButton rb = (RadioButton)sender;
			if (rb.Checked)
			{
                if (rb == averageBitrateRadio)
                {
                    targetSize.Enabled = false;
                    projectedBitrate.ReadOnly = false;
                    bpp.ReadOnly = true;
                    qest.ReadOnly = true;
                }
                else if (rb == bppRadio)
                {
                    targetSize.Enabled = false;
                    projectedBitrate.ReadOnly = true;
                    bpp.ReadOnly = false;
                    qest.ReadOnly = true;
                }
                else if (rb == qEstRadio)
                {
                    targetSize.Enabled = false;
                    projectedBitrate.ReadOnly = true;
                    bpp.ReadOnly = true;
                    qest.ReadOnly = false;
                }
                else
                {
                    targetSize.Enabled = true;
                    projectedBitrate.ReadOnly = true;
                    bpp.ReadOnly = true;
                    qest.ReadOnly = true;
                }
			}
		}

		/// <summary>
		/// handles codec selection change
		/// the choice is not limited for any codec
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void codec_CheckedChanged(object sender, System.EventArgs e)
		{
            UpdateContainers();
            Calculate();
		}
                
		private void bframes_CheckedChanged(object sender, System.EventArgs e)
		{
			Calculate();
		}

        private void targetSize_SelectionChanged(object sender, string val)
        {
            Calculate();
        }

        private void fpsChooser_SelectionChanged(object sender, string val)
        {
            double framerate = (double)fpsChooser.Value;
            int length = (int)totalSeconds.Value;
            int numberOfFrames = (int)(length * framerate);
            nbFrames.Value = numberOfFrames;
            this.Calculate();
        }

        private void addExtraToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddExtra();
        }

        private void addAudioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddAudio();
        }

        private void addAudioLink_Clicked(object sender, EventArgs e)
        {
            contextMenuStrip1.Show(addAudioLink, 0, addAudioLink.Height);
        }

        /// <summary>
        /// Switches between showing h m s and total seconds
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void picTime_Click(object sender, EventArgs e)
        {
            Image i = picTime.Image;
            picTime.Image = picTime.InitialImage;
            picTime.InitialImage = i;

            bool showTotal = hoursLabel.Visible;

            hours.Visible = !showTotal;
            hoursLabel.Visible = !showTotal;
            minutes.Visible = !showTotal;
            minutesLabel.Visible = !showTotal;
            seconds.Visible = !showTotal;
            secondsLabel.Visible = !showTotal;

            totalSeconds.Visible = showTotal;
            totalSecondsLabel.Visible = showTotal;
            timeText.Visible = showTotal;

            this.toolTip1.SetToolTip(this.picTime, showTotal ? "Show hours, minutes, seconds" : "Show total seconds");
        }

        #endregion
        
        protected void SetAudioLength()
        {
            foreach (Control c in audioExtraFlow.Controls)
            {
                if (c is AudioTrackSizeTab)
                {
                    AudioTrackSizeTab a = (AudioTrackSizeTab)c;
                    a.PlayLength = (int)totalSeconds.Value;
                }
            }
        }

        protected void UpdateTotalFrames()
        {
            int secs = (int)totalSeconds.Value;
            double fps = (double)fpsChooser.Value;
            int frameNumber = (int)((double)secs * fps);
            nbFrames.Value = frameNumber;
        }

        protected void UpdateTotalSeconds()
        {
            int secs = (int)this.hours.Value * 3600 + (int)this.minutes.Value * 60 + (int)this.seconds.Value;
            totalSeconds.Value = secs;
            timeText.Text = string.Format("{0}h {1}m {2}s", this.hours.Value, this.minutes.Value, this.seconds.Value);
        }
        
        /// <summary>
        /// Determines which containers can be selected based on video and audio streams
        /// </summary>
        protected void UpdateContainers()
        {
            if (updatingContainers) return;
            
            updatingContainers = true;
            List<MuxableType> muxableTypes = new List<MuxableType>();
            muxableTypes.AddRange(GetAudioTypes());
            ContainerType previousContainer = null;
            try 
            {
                previousContainer = containerFormat.SelectedItem as ContainerType;
            }
            catch (Exception) {}

            containerFormat.Items.Clear();
            containerFormat.Items.AddRange(muxProvider.GetSupportedContainers(SelectedVCodec, new AudioEncoderType[0], muxableTypes.ToArray()).ToArray());
            if (previousContainer != null && containerFormat.Items.Contains(previousContainer))
                containerFormat.SelectedItem = previousContainer;
            updatingContainers = false;
        }

        protected AudioTrackSizeTab AddAudio()
        {
            AudioTrackSizeTab a = new AudioTrackSizeTab();
            a.SomethingChanged += audio_SelectedIndexChanged;
            a.PlayLength = (int)totalSeconds.Value;
            audioExtraFlow.Controls.Add(a);
            UpdateContainers();
            Calculate();
            return a;
        }

        protected ExtraSizeTab AddExtra()
        {
            ExtraSizeTab a = new ExtraSizeTab();
            a.SomethingChanged += audio_SelectedIndexChanged;
            audioExtraFlow.Controls.Add(a);
            UpdateContainers();
            Calculate();
            return a;
        }

        protected void TagIndexes()
        {
            for (int i = 0; i < audioExtraFlow.Controls.Count; i++)
            {
                audioExtraFlow.Controls[i].Tag = i;
            }
        }

        protected void AddAutoSelectHandler(Control control)
        {
            // auto select text when focus numeric field
            foreach (Control c in control.Controls)
            {
                if (c.HasChildren) AddAutoSelectHandler(c);
                if (c is NumericUpDown)
                {
                    ((NumericUpDown)c).Enter += (s, args) => ((NumericUpDown)s).Select(0, ((NumericUpDown)s).Text.Length);
                    ((NumericUpDown)c).Click += (s, args) => ((NumericUpDown)s).Select(0, ((NumericUpDown)s).Text.Length);
                }
            }
        }
        
        /// <summary>
        /// Gets the audio streams by finding them in the form
        /// </summary>
        /// <returns></returns>
        protected IEnumerable<AudioBitrateCalculationStream> GetAudioStreams()
        {
            foreach (Control c in audioExtraFlow.Controls)
            {
                if (c is AudioTrackSizeTab)
                {
                    AudioTrackSizeTab a = (AudioTrackSizeTab)c;
                    if (a.Stream != null) yield return a.Stream;
                }
            }
        }

        /// <summary>
        /// Gets the total extra size by finding the extras and summing their sizes
        /// </summary>
        /// <returns>Total size of extra data</returns>
        protected FileSize GetTotalExtraSize()
        {
            FileSize size = FileSize.Empty;
            foreach (Control c in audioExtraFlow.Controls)
            {
                if (c is ExtraSizeTab) size += ((ExtraSizeTab)c).FileSize;
            }
            return size;
        }

        /// <summary>
        /// Gets the audio types used by the audio streams
        /// </summary>
        /// <returns></returns>
        protected IEnumerable<MuxableType> GetAudioTypes()
        {
            foreach (var s in GetAudioStreams())
                yield return new MuxableType(s.AType, s.AType.SupportedCodecs[0]);
        }

        /// <summary>
        /// Calculates by the selected method
        /// </summary>
        protected void Calculate()
        {
            if (calculating) return;
            calculating = true;
            try
            {

                CalcData data = new CalcData((long)nbFrames.Value, fpsChooser.Value ?? 0);
                data.FrameSize = new Size((int)width.Value, (int)height.Value);
                data.ExtraSize = GetTotalExtraSize();
                data.AudioStreams = GetAudioStreams().ToArray();
                data.ContainerType = containerFormat.SelectedItem as ContainerType;
                data.HasBFrames = bframes.Checked;
                data.VideoCodec = SelectedVCodec.VCodec;
                data.QualityCoeffient = (float)complexity.Value / 100F;

                if (fileSizeRadio.Checked) // get video, bpp, qest
                {
                    data.TotalSize = new FileSize(targetSize.Value.Value.Bytes);
                    data.CalcByTotalSize();
                }
                else if (this.bppRadio.Checked) // get video, quest, total
                {
                    data.BitsPerPixel = (float)bpp.Value;
                    data.CalcByBitsPerPixel();
                }
                else if (this.qEstRadio.Checked) // get video, bpp, total
                {
                    data.QualityEstimate = (float)qest.Value;
                    data.CalcByQualityEstimate();
                }
                else // given video size, get total, bpp, quest
                {
                    data.VideoBitrate = projectedBitrate.Value;
                    data.CalcByVideoSize();
                }

                targetSize.Value = new FileSize(Unit.KB, data.TotalSize.KB);
                projectedBitrate.Value = data.VideoBitrate;
                videoSize.Text = new FileSize(Unit.KB, data.VideoSize.KB).ToString();
                bpp.Value = (decimal)data.BitsPerPixel;
                qest.Value = (decimal)data.QualityEstimate;
                applyButton.Enabled = true;
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());
                applyButton.Enabled = false;
                videoSize.Text = "";
                if (fileSizeRadio.Checked)
                {
                    bpp.Value = 0;
                    qest.Value = 0;
                    projectedBitrate.Value = 0;
                }
                else if (this.bppRadio.Checked)
                {
                    qest.Value = 0;
                    projectedBitrate.Value = 0;
                    targetSize.Value = null;
                }
                else if (this.qEstRadio.Checked)
                {
                    bpp.Value = 0;
                    projectedBitrate.Value = 0;
                    targetSize.Value = null;
                }
                else
                {
                    bpp.Value = 0;
                    qest.Value = 0;
                    targetSize.Value = null;
                }
            }
            calculating = false;
        }
	}

    public class CalculatorTool : MeGUI.core.plugins.interfaces.ITool
    {
        #region ITool Members

        public string Name
        {
            get { return "Bitrate Calculator"; }
        }

        public void Run(MainForm info)
        {
            using (Calculator calc = new Calculator(info))
            {
                ulong nbFrames = 0;
                double framerate = 0.0;
                int hRes = 0, vRes = 0;
                Dar dar = new Dar();

                if (!string.IsNullOrEmpty(info.Video.VideoInput))
                    JobUtil.GetAllInputProperties(out nbFrames, out framerate, out hRes, out vRes, out dar, info.Video.VideoInput);

                calc.SetDefaults(nbFrames, framerate, hRes, vRes, info.Video.CurrentSettings, info.Audio.AudioStreams);

                DialogResult dr = calc.ShowDialog();
                if (dr != DialogResult.OK)
                    return;

                if (info.Video.CurrentSettings.EncoderType != calc.SelectedVCodec)
                    return;

                VideoCodecSettings settings = info.Video.CurrentSettings;

                if (settings.VideoEncodingType == VideoCodecSettings.VideoEncodingMode.CQ
                    || settings.VideoEncodingType == VideoCodecSettings.VideoEncodingMode.quality)
                {
                    dr = MessageBox.Show("Copy calculated bitrate into current video settings and change encoding mode to automated " + info.Settings.NbPasses + "-pass?", "Save calculated bitrate?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dr != DialogResult.Yes)
                        return;
                    if (info.Settings.NbPasses == 3)
                        settings.VideoEncodingType = VideoCodecSettings.VideoEncodingMode.threepassAutomated;  // Automated 3-pass
                    else
                        settings.VideoEncodingType = VideoCodecSettings.VideoEncodingMode.twopassAutomated;  // Automated 2-pass
                }
                else
                {
                    dr = MessageBox.Show("Copy calculated bitrate into current video settings?", "Save calculated bitrate?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dr != DialogResult.Yes)
                        return;
                }
                settings.BitrateQuantizer = calc.VideoBitrate;
            }
        }

        public Shortcut[] Shortcuts
        {
            get { return new Shortcut[] { Shortcut.CtrlB }; }
        }

        #endregion

        #region IIDable Members

        public string ID
        {
            get { return "bitrate_calculator_window"; }
        }

        #endregion
    }
}
