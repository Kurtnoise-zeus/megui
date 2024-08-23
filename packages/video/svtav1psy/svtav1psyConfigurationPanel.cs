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
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;

using MeGUI.core.details.video;
using MeGUI.core.gui;
using MeGUI.core.plugins.interfaces;

namespace MeGUI.packages.video.svtav1psy
{
    public partial class svtav1psyConfigurationPanel : MeGUI.core.details.video.VideoConfigurationPanel, Editable<svtav1psySettings>
    {
        private XmlDocument ContextHelp = new XmlDocument();
        public svtav1psyConfigurationPanel()
            : base()
        {
            InitializeComponent();
            svtEncodingMode.SelectedIndex = 0;
            gbPresets.Text = "Preset #" + tbsvtPresets.Value.ToString();
            svtTunes.Items.AddRange(EnumProxy.CreateArray(svtav1psySettings.SupportedPsyTuningModes));
            svtTunes.SelectedItem = EnumProxy.Create(svtav1psySettings.svtAv1PsyTuningModes.NONE);
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

        private void doTuningsAdjustments()
        {
            //if (this.x265NumberOfRefFrames.Value != x265Settings.GetDefaultNumberOfRefFrames((x265Settings.x265PresetLevelModes)tbx265Presets.Value, getPsyTuning(), chkBlurayCompat.Checked))
            //    this.x265NumberOfRefFrames.Value = x265Settings.GetDefaultNumberOfRefFrames((x265Settings.x265PresetLevelModes)tbx265Presets.Value, getPsyTuning(), chkBlurayCompat.Checked);
            //if (this.cbAQMode.SelectedIndex != x265Settings.GetDefaultAQMode((x265Settings.x265PresetLevelModes)tbx265Presets.Value, getPsyTuning()))
            //    this.cbAQMode.SelectedIndex = x265Settings.GetDefaultAQMode((x265Settings.x265PresetLevelModes)tbx265Presets.Value, getPsyTuning());

            switch (getPsyTuning())
            {
                case svtav1psySettings.svtAv1PsyTuningModes.NONE:
                    {

                    }
                    break;

                case svtav1psySettings.svtAv1PsyTuningModes.VQ:
                    {

                    }
                    break;

                case svtav1psySettings.svtAv1PsyTuningModes.PSNR:
                    {

                    }
                    break;

                case svtav1psySettings.svtAv1PsyTuningModes.SSIM:
                    {

                    }
                    break;

                case svtav1psySettings.svtAv1PsyTuningModes.SUBJECTIVESSIM:
                    {

                    }
                    break;
            }

        }

        private void doEncodingModeAdjustments()
        {
            if (isBitrateMode((VideoCodecSettings.VideoEncodingMode)svtEncodingMode.SelectedIndex))
            {
                this.svtBitrateQuantizerLabel.Text = "Bitrate";
                svtBitrateQuantizer.Maximum = 100000;
                svtBitrateQuantizer.Minimum = 1;
                svtBitrateQuantizer.DecimalPlaces = 0;
                svtBitrateQuantizer.Increment = 10;
            }
            else
            {
               if (svtEncodingMode.SelectedIndex == (int)VideoCodecSettings.VideoEncodingMode.CQ)
                {
                    this.svtBitrateQuantizerLabel.Text = "Quantizer";
                }
                if (svtEncodingMode.SelectedIndex == (int)VideoCodecSettings.VideoEncodingMode.quality)
                {
                    this.svtBitrateQuantizerLabel.Text = "Quality";
                }

                if (svtEncodingMode.SelectedIndex == (int)VideoCodecSettings.VideoEncodingMode.quality) // crf
                {
                    svtBitrateQuantizer.Maximum = 63;
                    svtBitrateQuantizer.Minimum = 1.0M;
                    svtBitrateQuantizer.DecimalPlaces = 1;
                    svtBitrateQuantizer.Increment = 0.1M;
                }
                else // qp
                {
                    svtBitrateQuantizer.Maximum = 63;
                    svtBitrateQuantizer.Minimum = 1;
                    svtBitrateQuantizer.Value = (int)svtBitrateQuantizer.Value; // makes sure it is an integer, in case we just swapped from crf                    
                    svtBitrateQuantizer.DecimalPlaces = 0;
                    svtBitrateQuantizer.Increment = 1;
                }

            }

            // We check whether the bitrate/quality text needs to be changed
            if (isBitrateMode((VideoCodecSettings.VideoEncodingMode)lastEncodingMode) != isBitrateMode((VideoCodecSettings.VideoEncodingMode)svtEncodingMode.SelectedIndex))
            {
                if (isBitrateMode((VideoCodecSettings.VideoEncodingMode)svtEncodingMode.SelectedIndex))
                    this.svtBitrateQuantizer.Value = 7000;
                else
                    this.svtBitrateQuantizer.Value = 35;
            }

            lastEncodingMode = (VideoCodecSettings.VideoEncodingMode)svtEncodingMode.SelectedIndex;
        }

        protected override string getCommandline()
        {
            ulong x = 1;
            return svtav1psyEncoder.genCommandline(null, null, null, -1, -1, ref x, Settings as svtav1psySettings, null);
        }
        /// <summary>
        /// Does all the necessary adjustments after a GUI change has been made.
        /// </summary>
        protected override void doCodecSpecificAdjustments()
        {
            doEncodingModeAdjustments();
        }
        /// <summary>
        /// The method by which codecs can add things to the Load event
        /// </summary>
        protected override void doCodecSpecificLoadAdjustments()
        {
            if (svtEncodingMode.SelectedIndex == -1)
                this.svtEncodingMode.SelectedIndex = 0;
            if (svtTunes.SelectedIndex == -1)
                svtTunes.SelectedIndex = 0; // Default
            lastEncodingMode = (VideoCodecSettings.VideoEncodingMode)this.svtEncodingMode.SelectedIndex;
            try
            {
                string p = System.IO.Path.Combine(Application.StartupPath, "Data");
                p = System.IO.Path.Combine(p, "ContextHelp.xml");
                ContextHelp.Load(p);
                SetToolTips();
            }
            catch
            {
                MessageBox.Show("The ContextHelp.xml file could not be found. Please check in the 'Data' directory to see if it exists. Help tooltips will not be available.", "File Not Found", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            tbsvtPresets_Scroll(null, null); // to update the trackbar label
        }


        /// <summary>
        /// Returns whether settings is ffv1Settings
        /// </summary>
        /// <param name="settings">The settings to check</param>
        /// <returns>Whether the settings are valid</returns>
        protected override bool isValidSettings(VideoCodecSettings settings)
        {
            return settings is svtav1psySettings;
        }

        /// <summary>
        /// Returns a new instance of ffv1Settings.
        /// </summary>
        /// <returns>A new instance of ffv1Settings</returns>
        protected override VideoCodecSettings defaultSettings()
        {
            return new svtav1psySettings();
        }

        /// <summary>
        /// gets / sets the settings currently displayed on the GUI
        /// </summary>
        public svtav1psySettings Settings
        {
            get
            {
                svtav1psySettings xs = new svtav1psySettings();
                xs.Preset = tbsvtPresets.Value;
                xs.VideoEncodingType = (VideoCodecSettings.VideoEncodingMode)svtEncodingMode.SelectedIndex;
                xs.BitrateQuantizer = (int)svtBitrateQuantizer.Value;
                xs.QuantizerCRF = svtBitrateQuantizer.Value;
                xs.svtAv1PsyTuning = getPsyTuning();
                return xs;
            }
            set
            {
                // Warning! The ordering of components matters because of the dependency code!
                if (value == null)
                    return;

                svtav1psySettings xs = value;
                updating = true;
                tbsvtPresets.Value = xs.Preset;
                svtTunes.SelectedItem = EnumProxy.Create(xs.svtAv1PsyTuning);
                svtEncodingMode.SelectedIndex = (int)xs.VideoEncodingType;
                xs.QuantizerCRF = svtBitrateQuantizer.Value;
                svtBitrateQuantizer.Value = (isBitrateMode(xs.VideoEncodingType) || xs.QuantizerCRF == 1) ? xs.BitrateQuantizer : xs.QuantizerCRF;
                doEncodingModeAdjustments();
                updating = false;
                genericUpdate();
            }
        }
        private void updateEvent(object sender, EventArgs e)
        {
            doEncodingModeAdjustments();
            genericUpdate();
        }
        private void textField_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && (int)Keys.Back != (int)e.KeyChar)
                e.Handled = true;
        }

        private string SelectHelpText(string node)
        {
            StringBuilder HelpText = new StringBuilder(64);

            try
            {
                string xpath = "/ContextHelp/Codec[@name='SVTAV1PSY']/" + node;
                XmlNodeList nl = ContextHelp.SelectNodes(xpath); // Return the details for the specified node

                if (nl.Count == 1) // if it finds the required HelpText, count should be 1
                {
                    HelpText.Append(nl[0].Attributes["name"].Value);
                    HelpText.AppendLine();
                    HelpText.AppendLine(nl[0]["Text"].InnerText);
                    HelpText.AppendLine();
                    HelpText.AppendLine("Default : " + nl[0]["Default"].InnerText);
                    HelpText.AppendLine("Recommended : " + nl[0]["Recommended"].InnerText);
                }
                else // If count isn't 1, then theres no valid data.
                    HelpText.Append("Error: No data available");
            }
            catch
            {
                HelpText.Append("Error: No data available");
            }

            return (HelpText.ToString());
        }
        private void SetToolTips()
        {
            /**********/
            /* Main   */
            /**********/
         /*   tooltipHelp.SetToolTip(x265Tunes, SelectHelpText("tunes"));
            //tooltipHelp.SetToolTip(cbTarget, SelectHelpText("targetmode"));
            tooltipHelp.SetToolTip(tbx265Presets, SelectHelpText("presets"));
            tooltipHelp.SetToolTip(x265BitrateQuantizer, SelectHelpText("bitrate"));
            tooltipHelp.SetToolTip(x265EncodingMode, SelectHelpText("encodingmode"));
         */
            /**************/
            /* Frame-Type */
            /**************/
         /*  tooltipHelp.SetToolTip(x265KeyframeInterval, SelectHelpText("keyint"));
            tooltipHelp.SetToolTip(NoiseReduction, SelectHelpText("nr"));
            tooltipHelp.SetToolTip(noFastPSkip, SelectHelpText("no-fast-pskip"));
            tooltipHelp.SetToolTip(macroblockOptions, SelectHelpText("partitions"));
            tooltipHelp.SetToolTip(x265ChromaMe, SelectHelpText("no-chroma-me"));
            tooltipHelp.SetToolTip(x265WeightedBPrediction, SelectHelpText("weightb"));
            tooltipHelp.SetToolTip(x265WeightedPPrediction, SelectHelpText("weightp"));
            tooltipHelp.SetToolTip(x265SubpelRefinement, SelectHelpText("subme"));
            tooltipHelp.SetToolTip(cabac, SelectHelpText("no-cabac"));
            tooltipHelp.SetToolTip(x265DeblockActive, SelectHelpText("nf"));
            tooltipHelp.SetToolTip(x265NewAdaptiveBframes, SelectHelpText("b-adapt"));
            tooltipHelp.SetToolTip(x265NumberOfRefFrames, SelectHelpText("ref"));
            tooltipHelp.SetToolTip(x265NumberOfBFrames, SelectHelpText("bframes"));
            tooltipHelp.SetToolTip(x265AlphaDeblock, SelectHelpText("filter"));
            tooltipHelp.SetToolTip(x265BetaDeblock, SelectHelpText("filter"));
            tooltipHelp.SetToolTip(x265CreditsQuantizer, SelectHelpText("creditsquant"));
            tooltipHelp.SetToolTip(x265IPFrameFactor, SelectHelpText("ipratio"));
            tooltipHelp.SetToolTip(x265PBFrameFactor, SelectHelpText("pbratio"));
            tooltipHelp.SetToolTip(x265ChromaQPOffset, SelectHelpText("chroma-qp-offset"));
            tooltipHelp.SetToolTip(cbBPyramid, SelectHelpText("b-pyramid"));
            tooltipHelp.SetToolTip(cbInterlaceMode, SelectHelpText("interlaced"));
            tooltipHelp.SetToolTip(x265PullDown, SelectHelpText("pulldown"));
            tooltipHelp.SetToolTip(scenecut, SelectHelpText("noscenecut"));
            tooltipHelp.SetToolTip(chkOpenGop, SelectHelpText("opengop"));
            tooltipHelp.SetToolTip(slicesnb, SelectHelpText("slicesnb"));
            tooltipHelp.SetToolTip(maxSliceSizeBytes, SelectHelpText("maxSliceSizeBytes"));
            tooltipHelp.SetToolTip(maxSliceSizeMB, SelectHelpText("maxSliceSizeMB"));
            tooltipHelp.SetToolTip(x265MinGOPSize, SelectHelpText("min-keyint"));
            tooltipHelp.SetToolTip(x265SCDSensitivity, SelectHelpText("scenecut"));
            tooltipHelp.SetToolTip(x265BframeBias, SelectHelpText("b-bias"));
            tooltipHelp.SetToolTip(x265BframePredictionMode, SelectHelpText("direct"));
            tooltipHelp.SetToolTip(cbGOPCalculation, SelectHelpText("gopcalculation"));
         */
            /*************************/
            /* Rate Control Tooltips */
            /*************************/
         /*   tooltipHelp.SetToolTip(x265MinimimQuantizer, SelectHelpText("qpmin"));
            tooltipHelp.SetToolTip(x265MaximumQuantizer, SelectHelpText("qpmax"));
            tooltipHelp.SetToolTip(x265MaxQuantDelta, SelectHelpText("qpstep"));
            tooltipHelp.SetToolTip(x265VBVBufferSize, SelectHelpText("vbv-bufsize"));
            tooltipHelp.SetToolTip(x265VBVMaxRate, SelectHelpText("vbv-maxrate"));
            tooltipHelp.SetToolTip(x265VBVInitialBuffer, SelectHelpText("vbv-init"));
            tooltipHelp.SetToolTip(x265RateTol, SelectHelpText("ratetol"));
            tooltipHelp.SetToolTip(x265QuantizerCompression, SelectHelpText("qcomp"));
            tooltipHelp.SetToolTip(x265TempFrameComplexityBlur, SelectHelpText("cplxblur"));
            tooltipHelp.SetToolTip(x265TempQuantBlur, SelectHelpText("qblur"));
            tooltipHelp.SetToolTip(mbtree, SelectHelpText("mbtree"));
            tooltipHelp.SetToolTip(lookahead, SelectHelpText("lookahead"));
            tooltipHelp.SetToolTip(deadzoneInter, SelectHelpText("deadzoneInter"));
            tooltipHelp.SetToolTip(deadzoneIntra, SelectHelpText("deadzoneIntra"));
            tooltipHelp.SetToolTip(cqmComboBox1, SelectHelpText("cqm"));
         */
            /*************************/
            /* Analysis Tooltips */
            /*************************/
         /*   tooltipHelp.SetToolTip(cbAQMode, SelectHelpText("aqmode"));
            tooltipHelp.SetToolTip(numAQStrength, SelectHelpText("aqstrength"));
            tooltipHelp.SetToolTip(macroblockOptions, SelectHelpText("analyse"));
            tooltipHelp.SetToolTip(adaptiveDCT, SelectHelpText("i8x8dct"));
            tooltipHelp.SetToolTip(x265B8x8mv, SelectHelpText("b8x8mv"));
            tooltipHelp.SetToolTip(x265P8x8mv, SelectHelpText("p8x8mv"));
            tooltipHelp.SetToolTip(x265P4x4mv, SelectHelpText("p4x4mv"));
            tooltipHelp.SetToolTip(x265I4x4mv, SelectHelpText("i4x4mv"));
            tooltipHelp.SetToolTip(x265I8x8mv, SelectHelpText("i8x8mv"));
            tooltipHelp.SetToolTip(x265aud, SelectHelpText("aud"));
            tooltipHelp.SetToolTip(x265hrd, SelectHelpText("nalhrd"));
            tooltipHelp.SetToolTip(noDCTDecimateOption, SelectHelpText("noDCTDecimateOption"));
            tooltipHelp.SetToolTip(nopsy, SelectHelpText("nopsy"));
            tooltipHelp.SetToolTip(fakeInterlaced, SelectHelpText("fakeInterlaced"));
            tooltipHelp.SetToolTip(chkBlurayCompat, SelectHelpText("blurayCompat"));
            tooltipHelp.SetToolTip(x265MixedReferences, SelectHelpText("mixed-refs"));
            tooltipHelp.SetToolTip(PsyRD, SelectHelpText("psyrd"));
            tooltipHelp.SetToolTip(PsyTrellis, SelectHelpText("psytrellis"));
            tooltipHelp.SetToolTip(trellis, SelectHelpText("trellis"));
            tooltipHelp.SetToolTip(x265METype, SelectHelpText("me"));
            tooltipHelp.SetToolTip(x265MERange, SelectHelpText("merange"));
         */
            /**************************/
            /* Misc Tooltips */
            /**************************/
         /*   tooltipHelp.SetToolTip(x265NbThreads, SelectHelpText("threads"));
            tooltipHelp.SetToolTip(customCommandlineOptions, SelectHelpText("customcommandline"));
            */
        }

        private void dSettings_Click(object sender, EventArgs e)
        {
            // Main Tab
            this.svtEncodingMode.SelectedIndex = (int)VideoCodecSettings.VideoEncodingMode.quality;
            this.svtBitrateQuantizer.Value = 35;
            this.svtTunes.SelectedIndex = 0;
            this.tbsvtPresets.Value = 10;

            // to update presets label
            tbsvtPresets_Scroll(null, null);
        }

            private void chMultiPass_CheckedChanged(object sender, EventArgs e)
        {
           updateEvent(sender, e);
        }

        private void chErrorCorrection_CheckedChanged(object sender, EventArgs e)
        {
           updateEvent(sender, e);
        }

        private void chContext_CheckedChanged(object sender, EventArgs e)
        {
            updateEvent(sender, e);
        }

        private void ch10BitsEncoder_CheckedChanged(object sender, EventArgs e)
        {
            updateEvent(sender, e);
        }

        private void ffv1Coder_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateEvent(sender, e);
        }

        private void ffv1Slices_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateEvent(sender, e);
        }

        private void nmGOPSize_ValueChanged(object sender, EventArgs e)
        {
            updateEvent(sender, e);
        }

        private void ffv1NbThreads_ValueChanged(object sender, EventArgs e)
        {
            updateEvent(sender, e);
        }

        private void ffv1EncodingMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            genericUpdate();
        }

        private void tbsvtPresets_Scroll(object sender, EventArgs e)
        {
            gbPresets.Text = "Preset #" + tbsvtPresets.Value.ToString();
            genericUpdate();
        }

        private void svtTunes_SelectedIndexChanged(object sender, EventArgs e)
        {
            doTuningsAdjustments();
            genericUpdate();
        }

        private svtav1psySettings.svtAv1PsyTuningModes getPsyTuning()
        {
            EnumProxy o = svtTunes.SelectedItem as EnumProxy;
            return (svtav1psySettings.svtAv1PsyTuningModes)o.RealValue;
        }

        private void svtEncodingMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            genericUpdate();
        }

        private void svtEncodingMode_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (svtEncodingMode.SelectedIndex == 0)
            {
                if (MainForm.Instance.Settings.NbPasses == 3)
                    svtEncodingMode.SelectedIndex = (int)VideoCodecSettings.VideoEncodingMode.threepassAutomated;
                else
                    svtEncodingMode.SelectedIndex = (int)VideoCodecSettings.VideoEncodingMode.twopassAutomated;
            }
            else
            {
                svtEncodingMode.SelectedIndex = (int)VideoCodecSettings.VideoEncodingMode.quality;
            }
        }

        private void svtBitrateQuantizer_ValueChanged(object sender, EventArgs e)
        {
            genericUpdate();
        }

    }
}