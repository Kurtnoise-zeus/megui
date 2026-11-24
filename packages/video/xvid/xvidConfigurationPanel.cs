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
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using MeGUI.core.plugins.interfaces;

namespace MeGUI.packages.video.xvid
{
    public partial class xvidConfigurationPanel : MeGUI.core.details.video.VideoConfigurationPanel, Editable<xvidSettings>
    {

        #region start / stop
        public xvidConfigurationPanel()
            : base()
        {
            InitializeComponent();
            BuildModernLayout();
            cqmComboBox1.StandardItems = new string[] { xvidSettings.H263Matrix, xvidSettings.MPEGMatrix };
        }
        #endregion
        #region adjustments
        private MeGUI.core.gui.SettingsCard rcCard, cbrCard;

        /// <summary>
        /// Windows 11 settings look: one page of cards per tab (see ModernDialog). The controls are only moved:
        /// names, events and the settings are unchanged. The labels which the code changes are used as titles of their rows.
        /// </summary>
        private static MeGUI.core.gui.ToggleSwitch Tg(MeGUI.core.gui.ModernDialog d, MeGUI.core.gui.ToggleSwitch t)
        {
            t.AutoSize = false;
            t.Size = new Size(d.S(96), d.S(26));
            return t;
        }

        private void BuildModernLayout()
        {
            MeGUI.core.gui.ModernDialog d = new MeGUI.core.gui.ModernDialog(this);
            MeGUI.core.gui.SettingsPage main = d.CurrentPage;
            d.Section("General");
            MeGUI.core.gui.SettingsCard c1 = d.Card();
            c1.AddRow("Mode", null, d.Wide(xvidEncodingMode, 180));
            xvidTurbo.Text = "Turbo";
            d.ToggleIn(c1, xvidTurbo, null);
            c1.AddLabelRow(xvidBitrateQuantLabel, null, d.Wide(xvidBitrateQuantizer, 120));
            c1.AddRow("Logfile", null, d.Wide(logfile, 260), d.Btn(logfileOpenButton));
            d.Section("Tools");
            MeGUI.core.gui.SettingsCard c2 = d.Card();
            c2.AddRow("VHQ mode", null, d.Wide(xvidVHQ, 180));
            c2.AddRow("Use VHQ for B-frames", null, Tg(d, xvidUseVHQForBframes));
            c2.AddRow("QPel", null, Tg(d, xvidQpel));
            c2.AddRow("GMC", null, Tg(d, xvidGMC));
            c2.AddRow("Chroma motion", null, Tg(d, xvidChromaMotion));
            c2.AddRow("Closed GOP", null, Tg(d, xvidClosedGop));
            c2.AddRow("HVS masking", null, d.Wide(HVSMasking, 180));
            c2.AddRow("Quantization type", null, d.Wide(cqmComboBox1, 300));
            d.Section("Other settings");
            MeGUI.core.gui.SettingsCard c3 = d.Card();
            xvidTrellisQuant.Text = "Trellis quant";
            d.ToggleIn(c3, xvidTrellisQuant, null);
            xvidInterlaced.Text = "Interlaced";
            d.ToggleIn(c3, xvidInterlaced, null);
            c3.AddRow("Packed bitstream", null, Tg(d, xvidPackedBitstream));
            c3.AddRow("Max keyframe interval", null, d.Wide(xvidKeyframeInterval, 90));
            c3.AddRow("Number of B-frames", null, d.Wide(xvidNbBFrames, 90));
            c3.AddRow("Threads", null, d.Wide(nbThreads, 90));
            c3.AddRow("Motion search precision", null, d.Wide(xvidMotionSearchPrecision, 200));
            c3.AddRow("Profile", null, d.Wide(xvidProfile, 200));
            c3.AddRow("VBV buffer", null, d.Wide(xvidVbvBuffer, 90));
            c3.AddRow("VBV max rate", null, d.Wide(xvidVbvMaxRate, 90));
            c3.AddRow("VBV peak rate", null, d.Wide(xvidVbvPeakRate, 90));
            d.Section("Other options");
            MeGUI.core.gui.SettingsCard c4 = d.Card();
            c4.AddRow("Custom commandline options", null, d.Wide(xvidCustomCommandlineOptions, 300));
            MeGUI.core.gui.SettingsPage adv = d.BeginPage();
            d.Section("Quantizers");
            MeGUI.core.gui.SettingsCard c5 = d.Card();
            c5.AddRow("I-frame quantizer (min / max)", null, d.Wide(xvidMinIQuant, 80), d.Wide(xvidMaxIQuant, 80));
            c5.AddRow("P-frame quantizer (min / max)", null, d.Wide(xvidMinPQuant, 80), d.Wide(xvidMaxPQuant, 80));
            c5.AddRow("B-frame quantizer (min / max)", null, d.Wide(xvidMinBQuant, 80), d.Wide(xvidMaxBQuant, 80));
            c5.AddRow("Credits quantizer", null, d.Wide(xvidCreditsQuantizer, 90));
            c5.AddRow("B-frame quantizer ratio", null, d.Wide(xvidBframeQuantRatio, 90));
            c5.AddRow("B-frame quantizer offset", null, d.Wide(xvidBframeQuantOffset, 90));
            d.Section("Rate control");
            rcCard = d.Card();
            rcCard.AddRow("I-frame boost (10x %)", null, d.Wide(xvidIframeBoost, 90));
            rcCard.AddRow("I-frames closer than ... (frames)", null, d.Wide(xvidKeyframeTreshold, 90));
            rcCard.AddRow("... are reduced by (%)", null, d.Wide(xvidKeyframeReduction, 90));
            rcCard.AddRow("Overflow control strength (%)", null, d.Wide(xvidOverflowControlStrength, 90));
            rcCard.AddRow("Max overflow improvement (%)", null, d.Wide(xvidMaxOverflowImprovement, 90));
            rcCard.AddRow("Max overflow degradation (%)", null, d.Wide(xvidMaxOverflowDegradation, 90));
            rcCard.AddRow("High bitrate scenes degradation (%)", null, d.Wide(xvidHighBitrateDegradation, 90));
            rcCard.AddRow("Low bitrate scenes improvement (%)", null, d.Wide(xvidLowBitrateImprovement, 90));
            d.Section("CBR rate control");
            cbrCard = d.Card();
            cbrCard.AddRow("Reaction delay factor", null, d.Wide(xvidRCDelayFactor, 90));
            cbrCard.AddRow("Averaging period", null, d.Wide(xvidRCAveragingPeriod, 90));
            cbrCard.AddRow("RC buffer size", null, d.Wide(xvidRCBufferSize, 90));
            d.Section("Custom command line");
            d.Block(customCommandlineOptions, 600, 34, d.Card());

            // each tab of the control gets its page of cards
            AttachPage(mainTabPage, main);
            AttachPage(advancedTabPage, adv);
            Size = new Size(d.S(700), d.S(600));
            tabControl1.Bounds = new Rectangle(0, 0, Width, Height - d.S(96));
            commandline.Bounds = new Rectangle(0, Height - d.S(90), Width, d.S(90));
            d.AttachTheme();
        }

        private static void AttachPage(TabPage tab, MeGUI.core.gui.SettingsPage page)
        {
            tab.Controls.Clear();
            tab.Padding = new Padding(0);
            page.Dock = DockStyle.Fill;
            tab.Controls.Add(page);
        }

        private void doCheckBoxAdjustments()
        {
            if ((int)xvidNbBFrames.Value > 0)
            {
                xvidUseVHQForBframes.Enabled = true;
                xvidMinBQuant.Enabled = true;
                xvidMaxBQuant.Enabled = true;
                xvidBframeQuantRatio.Enabled = true;
                xvidBframeQuantOffset.Enabled = true;
                xvidBframeThreshold.Enabled = true;
                xvidFrameDropRatio.Enabled = true;
            }
            else // no b-frames
            {
                xvidUseVHQForBframes.Enabled = false;
                xvidUseVHQForBframes.Checked = false;
                xvidMinBQuant.Enabled = false;
                xvidMaxBQuant.Enabled = false;
                xvidBframeQuantRatio.Enabled = false;
                xvidBframeQuantOffset.Enabled = false;
                xvidBframeThreshold.Enabled = false;
                xvidFrameDropRatio.Enabled = false;
            }
        }
        /// <summary>
        /// Returns whether the given mode is a bitrate or quality-based mode
        /// </summary>
        /// <param name="mode">selected encoding mode</param>
        /// <returns>true if the mode is a bitrate mode, false otherwise</returns>
        private bool isBitrateMode(VideoCodecSettings.VideoEncodingMode mode)
        {
            return !(mode == VideoCodecSettings.VideoEncodingMode.CQ ||
                mode == VideoCodecSettings.VideoEncodingMode.quality);
        }
        private void doDropDownAdjustments()
        {
            logfileOpenButton.Enabled = false;
            if (isBitrateMode((VideoCodecSettings.VideoEncodingMode)xvidEncodingMode.SelectedIndex))
            {
                xvidBitrateQuantLabel.Text = "Bitrate";
                xvidBitrateQuantizer.Maximum = 10000;
                xvidBitrateQuantizer.Minimum = 1;
                xvidBitrateQuantizer.Increment = 1;
                xvidBitrateQuantizer.DecimalPlaces = 0;
            }
            else
            {
                xvidBitrateQuantLabel.Text = "Quantizer";
                xvidBitrateQuantizer.Maximum = 31;
                xvidBitrateQuantizer.Minimum = 1;
                xvidBitrateQuantizer.Increment = 0.1M;
                xvidBitrateQuantizer.DecimalPlaces = 1;
            }
            switch (this.xvidEncodingMode.SelectedIndex)
            {
                case 0: // cbr
                    xvidProfile.SelectedIndex = 0;
                    xvidProfile.Enabled = false;
                    xvidTurbo.Checked = false;
                    xvidTurbo.Enabled = false;
                    rcCard.Enabled = false;
                    cbrCard.Enabled = true;
                    break;
                case 1: // CQ
                    xvidProfile.SelectedIndex = 0;
                    xvidProfile.Enabled = false;
                    xvidTurbo.Checked = false;
                    xvidTurbo.Enabled = false;
                    rcCard.Enabled = false;
                    cbrCard.Enabled = true;
                    break;
                case 2: // 2pass first pass
                    xvidProfile.Enabled = true;
                    xvidTurbo.Enabled = true;
                    rcCard.Enabled = true;
                    cbrCard.Enabled = false;
                    logfileOpenButton.Enabled = true;
                    break;
                case 3: // 2 pass 2nd pass
                    xvidProfile.Enabled = true;
                    xvidTurbo.Enabled = true;
                    rcCard.Enabled = true;
                    cbrCard.Enabled = false;
                    logfileOpenButton.Enabled = true;
                    break;
                case 4: // automated 2pass
                    xvidProfile.Enabled = true;
                    xvidTurbo.Enabled = true;
                    rcCard.Enabled = true;
                    cbrCard.Enabled = false;
                    logfileOpenButton.Enabled = true;
                    break;
            }
            switch (this.xvidProfile.SelectedIndex)
            {
                case 0: // Unrestricted
                    this.xvidNbBFrames.Maximum = 4;
                    this.xvidPackedBitstream.Enabled = true;
                    this.cqmComboBox1.Enabled = true;
                    this.xvidQpel.Enabled = true;
                    this.xvidGMC.Enabled = true;
                    this.xvidOverflowControlStrength.Minimum = 0;
                    this.xvidMaxOverflowDegradation.Minimum = 0;
                    this.xvidMaxOverflowImprovement.Minimum = 0;
                    this.xvidInterlaced.Enabled = true;
                    this.xvidVbvBuffer.Enabled = false;
                    this.xvidVbvMaxRate.Enabled = false;
                    this.xvidVbvPeakRate.Enabled = false;
                    this.xvidVbvBuffer.Text = "0";
                    this.xvidVbvMaxRate.Text = "0";
                    this.xvidVbvPeakRate.Text = "0";
                    break;
                case 1: // Home Theater
                    this.xvidNbBFrames.Maximum = 1;
                    this.xvidPackedBitstream.Checked = true;
                    this.cqmComboBox1.SelectedIndex = 0;
                    this.cqmComboBox1.Enabled = false;
                    this.xvidQpel.Enabled = false;
                    this.xvidQpel.Checked = false;
                    this.xvidGMC.Enabled = false;
                    this.xvidGMC.Checked = false;
                    this.xvidOverflowControlStrength.Minimum = 10;
                    this.xvidMaxOverflowDegradation.Minimum = 10;
                    this.xvidMaxOverflowImprovement.Minimum = 10;
                    this.xvidInterlaced.Enabled = true;
                    this.xvidVbvBuffer.Enabled = false;
                    this.xvidVbvMaxRate.Enabled = false;
                    this.xvidVbvPeakRate.Enabled = false;
                    this.xvidVbvBuffer.Text = "0";
                    this.xvidVbvMaxRate.Text = "0";
                    this.xvidVbvPeakRate.Text = "0";
                    break;
                case 2: // Hi-Def 720p
                    this.xvidNbBFrames.Maximum = 2;
                    this.xvidPackedBitstream.Checked = false;
                    this.cqmComboBox1.SelectedIndex = 0;
                    this.cqmComboBox1.Enabled = false;
                    this.xvidQpel.Enabled = false;
                    this.xvidQpel.Checked = false;
                    this.xvidGMC.Enabled = false;
                    this.xvidGMC.Checked = false;
                    this.xvidOverflowControlStrength.Minimum = 10;
                    this.xvidMaxOverflowDegradation.Minimum = 10;
                    this.xvidMaxOverflowImprovement.Minimum = 10;
                    this.xvidInterlaced.Enabled = true;
                    this.xvidVbvBuffer.Enabled = false;
                    this.xvidVbvMaxRate.Enabled = false;
                    this.xvidVbvPeakRate.Enabled = false;
                    this.xvidVbvBuffer.Text = "0";
                    this.xvidVbvMaxRate.Text = "0";
                    this.xvidVbvPeakRate.Text = "0";
                    break;
                case 3: // Hi-Def 1080p
                    this.xvidNbBFrames.Maximum = 2;
                    this.xvidPackedBitstream.Checked = false;
                    this.cqmComboBox1.SelectedIndex = 0;
                    this.cqmComboBox1.Enabled = false;
                    this.xvidQpel.Enabled = false;
                    this.xvidQpel.Checked = false;
                    this.xvidGMC.Enabled = false;
                    this.xvidGMC.Checked = false;
                    this.xvidOverflowControlStrength.Minimum = 10;
                    this.xvidMaxOverflowDegradation.Minimum = 10;
                    this.xvidMaxOverflowImprovement.Minimum = 10;
                    this.xvidInterlaced.Enabled = true;
                    this.xvidVbvBuffer.Enabled = false;
                    this.xvidVbvMaxRate.Enabled = false;
                    this.xvidVbvPeakRate.Enabled = false;
                    this.xvidVbvBuffer.Text = "0";
                    this.xvidVbvMaxRate.Text = "0";
                    this.xvidVbvPeakRate.Text = "0";
                    break;
                case 4: // Handheld
                    this.xvidNbBFrames.Maximum = 0;
                    this.cqmComboBox1.SelectedIndex = 0;
                    this.cqmComboBox1.Enabled = false;
                    this.xvidQpel.Enabled = false;
                    this.xvidQpel.Checked = false;
                    this.xvidGMC.Enabled = false;
                    this.xvidGMC.Checked = false;
                    this.xvidOverflowControlStrength.Minimum = 10;
                    this.xvidMaxOverflowDegradation.Minimum = 10;
                    this.xvidMaxOverflowImprovement.Minimum = 10;
                    this.xvidInterlaced.Checked = false;
                    this.xvidInterlaced.Enabled = false;
                    this.xvidVbvBuffer.Enabled = false;
                    this.xvidVbvMaxRate.Enabled = false;
                    this.xvidVbvPeakRate.Enabled = false;
                    this.xvidVbvBuffer.Text = "0";
                    this.xvidVbvMaxRate.Text = "0";
                    this.xvidVbvPeakRate.Text = "0";
                    break;
                case 5: // Portable
                    this.xvidNbBFrames.Maximum = 0;
                    this.cqmComboBox1.SelectedIndex = 0;
                    this.cqmComboBox1.Enabled = false;
                    this.xvidQpel.Enabled = false;
                    this.xvidQpel.Checked = false;
                    this.xvidGMC.Enabled = false;
                    this.xvidGMC.Checked = false;
                    this.xvidOverflowControlStrength.Minimum = 10;
                    this.xvidMaxOverflowDegradation.Minimum = 10;
                    this.xvidMaxOverflowImprovement.Minimum = 10;
                    this.xvidInterlaced.Checked = false;
                    this.xvidInterlaced.Enabled = false;
                    this.xvidVbvBuffer.Enabled = false;
                    this.xvidVbvMaxRate.Enabled = false;
                    this.xvidVbvPeakRate.Enabled = false;
                    this.xvidVbvBuffer.Text = "0";
                    this.xvidVbvMaxRate.Text = "0";
                    this.xvidVbvPeakRate.Text = "0";
                    break;
                case 6: // Custom
                    this.xvidOverflowControlStrength.Minimum = 10;
                    this.xvidMaxOverflowDegradation.Minimum = 10;
                    this.xvidMaxOverflowImprovement.Minimum = 10;
                    this.xvidVbvBuffer.Enabled = true;
                    this.xvidVbvMaxRate.Enabled = true;
                    this.xvidVbvPeakRate.Enabled = true;
                    break;
            }
            // We check whether the bitrate/quality text needs to be changed
            if (isBitrateMode((VideoCodecSettings.VideoEncodingMode)lastEncodingMode) != isBitrateMode((VideoCodecSettings.VideoEncodingMode)xvidEncodingMode.SelectedIndex))
            {
                if (isBitrateMode((VideoCodecSettings.VideoEncodingMode)xvidEncodingMode.SelectedIndex))
                {
                    this.xvidBitrateQuantizer.Value = 700;
                }
                else
                {
                    this.xvidBitrateQuantizer.Value = 8;
                }
            }

            lastEncodingMode = (VideoCodecSettings.VideoEncodingMode)this.xvidEncodingMode.SelectedIndex;

            if (xvidVHQ.SelectedIndex == 0)
            {
                xvidUseVHQForBframes.Checked = false;
                xvidUseVHQForBframes.Enabled = false;
            }
            else
                xvidUseVHQForBframes.Enabled = true;
        }

        #endregion
        #region codec-specific overload functions
        protected override string getCommandline()
        {
            return XviDEncoder.genCommandline("input", "output", null, Settings as xvidSettings, -1, -1, null, null);
        }
        /// <summary>
        /// Does all the necessary adjustments after a GUI change has been made.
        /// </summary>
        protected override void doCodecSpecificAdjustments()
        {
            doDropDownAdjustments();
            doCheckBoxAdjustments();
        }

        /// <summary>
        /// The method by which codecs can add things to the Load event
        /// </summary>
        protected override void doCodecSpecificLoadAdjustments()
        {
            if (xvidEncodingMode.SelectedIndex == -1)
                this.xvidEncodingMode.SelectedIndex = (int)VideoCodecSettings.VideoEncodingMode.CBR;
            if (xvidMotionSearchPrecision.SelectedIndex == -1)
                this.xvidMotionSearchPrecision.SelectedIndex = 6;
            if (xvidVHQ.SelectedIndex == -1)
                this.xvidVHQ.SelectedIndex = 1;
            if (cqmComboBox1.SelectedIndex == -1)
                cqmComboBox1.SelectedIndex = 0;
            if (HVSMasking.SelectedIndex == -1)
                HVSMasking.SelectedIndex = 0;
        }

        /// <summary>
        /// Returns whether settings is xvidSettings
        /// </summary>
        /// <param name="settings">The settings to check</param>
        /// <returns>Whether the settings are valid</returns>
        protected override bool isValidSettings(VideoCodecSettings settings)
        {
            return settings is xvidSettings;
        }

        /// <summary>
        /// Returns a new instance of xvidSettings.
        /// </summary>
        /// <returns>A new instance of xvidSettings</returns>
        protected override VideoCodecSettings defaultSettings()
        {
            return new xvidSettings();
        }

        /// <summary>
        /// gets / sets the settings currently displayed on the GUI
        /// </summary>
        public xvidSettings Settings
        {
            get
            {
                xvidSettings xs = new xvidSettings();
                xs.Turbo = this.xvidTurbo.Checked;
                xs.VideoEncodingType = (VideoCodecSettings.VideoEncodingMode)this.xvidEncodingMode.SelectedIndex;
                xs.Quantizer = xvidBitrateQuantizer.Value;
                xs.BitrateQuantizer = (int)xvidBitrateQuantizer.Value;

                if (!String.IsNullOrEmpty(xvidKeyframeTreshold.Text))
                    xs.KeyframeInterval = Int32.Parse(this.xvidKeyframeInterval.Text);
                xs.NbBframes = (int)xvidNbBFrames.Value;
                xs.PackedBitstream = xvidPackedBitstream.Checked;
                xs.XvidProfile = xvidProfile.SelectedIndex;
                xs.VbvBuffer = Int32.Parse(xvidVbvBuffer.Text);
                xs.VbvMaxRate = Int32.Parse(xvidVbvMaxRate.Text);
                xs.VbvPeakRate = Int32.Parse(xvidVbvPeakRate.Text);
                xs.MotionSearchPrecision = xvidMotionSearchPrecision.SelectedIndex;
                xs.VHQMode = xvidVHQ.SelectedIndex;
                xs.VHQForBframes = xvidUseVHQForBframes.Checked;
                xs.QPel = xvidQpel.Checked;
                xs.GMC = xvidGMC.Checked;
                xs.NbThreads = (int)nbThreads.Value;
                xs.ChromaMotion = xvidChromaMotion.Checked;
                xs.ClosedGOP = xvidClosedGop.Checked;
                xs.Interlaced = xvidInterlaced.Checked;
                xs.MinQuantizer = (int)xvidMinIQuant.Value;
                xs.MaxQuantizer = (int)xvidMaxIQuant.Value;
                xs.MinPQuant = (int)xvidMinPQuant.Value;
                xs.MaxPQuant = (int)xvidMaxPQuant.Value;
                xs.MinBQuant = (int)xvidMinBQuant.Value;
                xs.MaxBQuant = (int)xvidMaxBQuant.Value;
                xs.CreditsQuantizer = (int)xvidCreditsQuantizer.Value;
                xs.Trellis = xvidTrellisQuant.Checked;
                xs.BQuantRatio = (int)xvidBframeQuantRatio.Value;
                xs.BQuantOffset = (int)xvidBframeQuantOffset.Value;
                xs.KeyFrameBoost = (int)xvidIframeBoost.Value;
                if (!String.IsNullOrEmpty(xvidRCDelayFactor.Text))
                    xs.KeyframeThreshold = Int32.Parse(xvidKeyframeTreshold.Text);
                xs.KeyframeReduction = (int)xvidKeyframeReduction.Value;
                xs.OverflowControlStrength = (int)xvidOverflowControlStrength.Value;
                xs.MaxOverflowImprovement = (int)xvidMaxOverflowImprovement.Value;
                xs.MaxOverflowDegradation = (int)xvidMaxOverflowDegradation.Value;
                xs.HighBitrateDegradation = (int)xvidHighBitrateDegradation.Value;
                xs.LowBitrateImprovement = (int)xvidLowBitrateImprovement.Value;
                if (!String.IsNullOrEmpty(xvidRCAveragingPeriod.Text))
                    xs.ReactionDelayFactor = Int32.Parse(xvidRCDelayFactor.Text);
                if (!String.IsNullOrEmpty(xvidRCDelayFactor.Text))
                    xs.AveragingPeriod = Int32.Parse(xvidRCAveragingPeriod.Text);
                if (!String.IsNullOrEmpty(xvidRCBufferSize.Text) && !xvidRCBufferSize.Text.Equals("0"))
                    xs.RateControlBuffer = Int32.Parse(xvidRCBufferSize.Text);
                xs.BframeThreshold = this.xvidBframeThreshold.Value;
                xs.FrameDropRatio = (int)xvidFrameDropRatio.Value;
                xs.QuantizerMatrix = cqmComboBox1.SelectedText;
                xs.CustomEncoderOptions = customCommandlineOptions.Text;
                xs.Logfile = this.logfile.Text;
                xs.HVSMasking = HVSMasking.SelectedIndex;
                return xs;
            }
            set
            {
                if (value == null)
                    return;
                
                                xvidSettings xs = value;
                this.xvidTurbo.Checked = xs.Turbo;
                this.xvidEncodingMode.SelectedIndex = (int)xs.VideoEncodingType;
                lastEncodingMode = xs.VideoEncodingType;
                if (xs.VideoEncodingType == VideoCodecSettings.VideoEncodingMode.CQ)
                    xvidBitrateQuantizer.Value = xs.Quantizer;
                else
                    xvidBitrateQuantizer.Value = xs.BitrateQuantizer;
                this.nbThreads.Value = xs.NbThreads;
                this.xvidKeyframeInterval.Text = xs.KeyframeInterval.ToString(); ;
                xvidNbBFrames.Value = xs.NbBframes;
                xvidPackedBitstream.Checked = xs.PackedBitstream;
                xvidProfile.SelectedIndex = xs.XvidProfile;
                xvidVbvBuffer.Text = xs.VbvBuffer.ToString();
                xvidVbvMaxRate.Text = xs.VbvMaxRate.ToString();
                xvidVbvPeakRate.Text = xs.VbvPeakRate.ToString();
                xvidMotionSearchPrecision.SelectedIndex = xs.MotionSearchPrecision;
                xvidVHQ.SelectedIndex = xs.VHQMode;
                xvidUseVHQForBframes.Checked = xs.VHQForBframes;
                xvidQpel.Checked = xs.QPel;
                xvidGMC.Checked = xs.GMC;
                xvidChromaMotion.Checked = xs.ChromaMotion;
                xvidClosedGop.Checked = xs.ClosedGOP;
                xvidInterlaced.Checked = xs.Interlaced;
                xvidMinIQuant.Value = xs.MinQuantizer;
                xvidMaxIQuant.Value = xs.MaxQuantizer;
                xvidMinPQuant.Value = xs.MinPQuant;
                xvidMaxPQuant.Value = xs.MaxPQuant;
                xvidMinBQuant.Value = xs.MinBQuant;
                xvidMaxBQuant.Value = xs.MaxBQuant;
                xvidCreditsQuantizer.Value = xs.CreditsQuantizer;
                xvidTrellisQuant.Checked = xs.Trellis;
                xvidBframeQuantRatio.Value = xs.BQuantRatio;
                xvidBframeQuantOffset.Value = xs.BQuantOffset;
                xvidIframeBoost.Value = xs.KeyFrameBoost;
                xvidKeyframeTreshold.Text = xs.KeyframeThreshold.ToString();
                xvidKeyframeReduction.Value = xs.KeyframeReduction;
                xvidOverflowControlStrength.Value = xs.OverflowControlStrength;
                xvidMaxOverflowImprovement.Value = xs.MaxOverflowImprovement;
                xvidMaxOverflowDegradation.Value = xs.MaxOverflowDegradation;
                xvidHighBitrateDegradation.Value = xs.HighBitrateDegradation;
                xvidLowBitrateImprovement.Value = xs.LowBitrateImprovement;
                xvidRCDelayFactor.Text = xs.ReactionDelayFactor.ToString();
                xvidRCAveragingPeriod.Text = xs.AveragingPeriod.ToString();
                if (xs.RateControlBuffer > 0)
                    xvidRCBufferSize.Text = xs.RateControlBuffer.ToString();
                else
                    xvidRCBufferSize.Text = "";
                this.xvidBframeThreshold.Value = xs.BframeThreshold;
                xvidFrameDropRatio.Value = (decimal)xs.FrameDropRatio;
                cqmComboBox1.SelectedObject = xs.QuantizerMatrix;
                customCommandlineOptions.Text = xs.CustomEncoderOptions;
                this.logfile.Text = xs.Logfile;
                HVSMasking.SelectedIndex = xs.HVSMasking;
            }
        }
        #endregion
        #region events
        private void updateEvent(object sender, EventArgs e)
        {
            genericUpdate();
        }
        private void textField_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && (int)Keys.Back != (int)e.KeyChar)
                e.Handled = true;
        }
        private void logfileOpenButton_Click(object sender, System.EventArgs e)
        {
            if (this.saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                this.logfile.Text = saveFileDialog.FileName;
                this.showCommandLine();
            }
        }
        #endregion

        private void cqmComboBox1_SelectionChanged(object sender, string val)
        {
            genericUpdate();
        }

        private void xvidProfile_SelectedIndexChanged(object sender, EventArgs e)
        {
            genericUpdate();
        }

        private void xvidVbvBuffer_TextChanged(object sender, EventArgs e)
        {
            genericUpdate();
        }

        private void xvidVbvMaxRate_TextChanged(object sender, EventArgs e)
        {
            genericUpdate();
        }

        private void xvidVbvPeakRate_TextChanged(object sender, EventArgs e)
        {
            genericUpdate();
        }
    }
}