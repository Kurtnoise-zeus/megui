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
using System.Xml;

using MeGUI.core.details.video;
using MeGUI.core.gui;
using MeGUI.core.plugins.interfaces;

namespace MeGUI.packages.video.x265
{
    public partial class x265ConfigurationPanel : MeGUI.core.details.video.VideoConfigurationPanel, Editable<x265Settings>
    {
        #region variables
        private XmlDocument ContextHelp = new XmlDocument();
        #endregion
        #region start / stop
        public x265ConfigurationPanel()
            : base()
        {
            InitializeComponent();
            BuildModernLayout();
            cqmComboBox1.StandardItems = new string[] { "Flat (none)", "JVT" };
            cqmComboBox1.SelectedIndex = 0;
            x265Tunes.Items.AddRange(EnumProxy.CreateArray(x265Settings.SupportedPsyTuningModes));
            x265Tunes.SelectedItem = EnumProxy.Create(x265Settings.x265PsyTuningModes.NONE);
        }
        #endregion
        #region dropdowns
        private void doTuningsAdjustments()
        {
            if (this.x265NumberOfRefFrames.Value != x265Settings.GetDefaultNumberOfRefFrames((x265Settings.x265PresetLevelModes)tbx265Presets.Value, getPsyTuning(), chkBlurayCompat.Checked))
                this.x265NumberOfRefFrames.Value = x265Settings.GetDefaultNumberOfRefFrames((x265Settings.x265PresetLevelModes)tbx265Presets.Value, getPsyTuning(), chkBlurayCompat.Checked);
            if (this.cbAQMode.SelectedIndex != x265Settings.GetDefaultAQMode((x265Settings.x265PresetLevelModes)tbx265Presets.Value, getPsyTuning()))
                this.cbAQMode.SelectedIndex = x265Settings.GetDefaultAQMode((x265Settings.x265PresetLevelModes)tbx265Presets.Value, getPsyTuning());
            
            switch (getPsyTuning())
            {
                case x265Settings.x265PsyTuningModes.NONE:
                    {
                        if (this.x265AlphaDeblock.Value != 0)
                            this.x265AlphaDeblock.Value = 0;
                        if (this.x265BetaDeblock.Value != 0)
                            this.x265BetaDeblock.Value = 0;
                        if (this.PsyTrellis.Value != 0.0M)
                            this.PsyTrellis.Value = 0.0M;
                        if (this.PsyRD.Value != 1.0M)
                            this.PsyRD.Value = 1.0M;
                        if (this.numAQStrength.Value != 1.0M)
                            this.numAQStrength.Value = 1.0M;
                        if (this.x265IPFrameFactor.Value != 1.4M)
                            this.x265IPFrameFactor.Value = 1.4M;
                        if (this.x265PBFrameFactor.Value != 1.3M)
                            this.x265PBFrameFactor.Value = 1.3M;
                        if (this.deadzoneInter.Value != 21)
                            this.deadzoneInter.Value = 21;
                        if (this.deadzoneIntra.Value != 11)
                            this.deadzoneIntra.Value = 11;
                        if (this.x265QuantizerCompression.Value != 0.6M)
                            this.x265QuantizerCompression.Value = 0.6M;
                        if (noDCTDecimateOption.Checked)
                            noDCTDecimateOption.Checked = false;
                        if (nopsy.Checked)
                            nopsy.Checked = false;
                    }
                    break;

                case x265Settings.x265PsyTuningModes.PSNR:
                    {
                        if (this.x265AlphaDeblock.Value != 0)
                            this.x265AlphaDeblock.Value = 0;
                        if (this.x265BetaDeblock.Value != 0)
                            this.x265BetaDeblock.Value = 0;
                        if (this.PsyTrellis.Value != 0.0M)
                            this.PsyTrellis.Value = 0.0M;
                        if (this.PsyRD.Value != 1.0M)
                            this.PsyRD.Value = 1.0M;
                        if (this.numAQStrength.Value != 1.0M)
                            this.numAQStrength.Value = 1.0M;
                        if (this.x265IPFrameFactor.Value != 1.4M)
                            this.x265IPFrameFactor.Value = 1.4M;
                        if (this.x265PBFrameFactor.Value != 1.3M)
                            this.x265PBFrameFactor.Value = 1.3M;
                        if (this.deadzoneInter.Value != 21)
                            this.deadzoneInter.Value = 21;
                        if (this.deadzoneIntra.Value != 11)
                            this.deadzoneIntra.Value = 11;
                        if (this.x265QuantizerCompression.Value != 0.6M)
                            this.x265QuantizerCompression.Value = 0.6M;
                        if (noDCTDecimateOption.Checked)
                            noDCTDecimateOption.Checked = false;
                        if (!nopsy.Checked)
                            nopsy.Checked = true;
                    }
                    break;
                case x265Settings.x265PsyTuningModes.SSIM:
                    {
                        if (this.x265AlphaDeblock.Value != 0)
                            this.x265AlphaDeblock.Value = 0;
                        if (this.x265BetaDeblock.Value != 0)
                            this.x265BetaDeblock.Value = 0;
                        if (this.PsyTrellis.Value != 0.0M)
                            this.PsyTrellis.Value = 0.0M;
                        if (this.PsyRD.Value != 1.0M)
                            this.PsyRD.Value = 1.0M;
                        if (this.numAQStrength.Value != 1.0M)
                            this.numAQStrength.Value = 1.0M;
                        if (this.x265IPFrameFactor.Value != 1.4M)
                            this.x265IPFrameFactor.Value = 1.4M;
                        if (this.x265PBFrameFactor.Value != 1.3M)
                            this.x265PBFrameFactor.Value = 1.3M;
                        if (this.deadzoneInter.Value != 21)
                            this.deadzoneInter.Value = 21;
                        if (this.deadzoneIntra.Value != 11)
                            this.deadzoneIntra.Value = 11;
                        if (this.x265QuantizerCompression.Value != 0.6M)
                            this.x265QuantizerCompression.Value = 0.6M;
                        if (noDCTDecimateOption.Checked)
                            noDCTDecimateOption.Checked = false;
                        if (!nopsy.Checked)
                            nopsy.Checked = true;
                    }
                    break;
            }

        }

        private void doPresetsAdjustments()
        {
            if (this.x265NumberOfRefFrames.Value != x265Settings.GetDefaultNumberOfRefFrames((x265Settings.x265PresetLevelModes)tbx265Presets.Value, getPsyTuning(), chkBlurayCompat.Checked))
                this.x265NumberOfRefFrames.Value = x265Settings.GetDefaultNumberOfRefFrames((x265Settings.x265PresetLevelModes)tbx265Presets.Value, getPsyTuning(), chkBlurayCompat.Checked);
            if (this.cbAQMode.SelectedIndex != x265Settings.GetDefaultAQMode((x265Settings.x265PresetLevelModes)tbx265Presets.Value, getPsyTuning()))
                this.cbAQMode.SelectedIndex = x265Settings.GetDefaultAQMode((x265Settings.x265PresetLevelModes)tbx265Presets.Value, getPsyTuning());

            switch (tbx265Presets.Value)
            {
                case 0: // Ultra Fast
                    {
                        if (this.x265MERange.Value != 16) 
                            this.x265MERange.Value = 16;
                        if (macroblockOptions.SelectedIndex != 1)
                            macroblockOptions.SelectedIndex = 1;
                        if (x265SubpelRefinement.SelectedIndex != 0)
                            x265SubpelRefinement.SelectedIndex = 0;
                        if (x265METype.SelectedIndex != 0)
                            x265METype.SelectedIndex = 0;
                        if (trellis.SelectedIndex != 0)
                            trellis.SelectedIndex = 0;
                        if (x265BframePredictionMode.SelectedIndex != 1)
                            x265BframePredictionMode.SelectedIndex = 1;
                        if (scenecut.Checked)
                            scenecut.Checked = false;
                        if (x265DeblockActive.Checked)
                            x265DeblockActive.Checked = false;
                        if (cabac.Checked)
                            cabac.Checked = false;
                        if (adaptiveDCT.Checked)
                            adaptiveDCT.Checked = false;
                        if (!x265MixedReferences.Checked)
                            x265MixedReferences.Checked = true;
                        if (mbtree.Checked)
                            mbtree.Checked = false;
                        mbtree.Enabled = false;
                        if (noFastPSkip.Checked)
                            noFastPSkip.Checked = false;
                        if (x265NewAdaptiveBframes.SelectedIndex != 0)
                            x265NewAdaptiveBframes.SelectedIndex = 0;
                        if (this.x265WeightedBPrediction.Checked)
                            this.x265WeightedBPrediction.Checked = false;
                    }
                    break;
                case 1: // Super Fast
                    {
                        if (macroblockOptions.SelectedIndex != 2)
                            macroblockOptions.SelectedIndex = 2;
                        if (!x265I4x4mv.Checked)
                            x265I4x4mv.Checked = true;
                        if (!x265I8x8mv.Checked)
                            x265I8x8mv.Checked = true;
                        if (x265B8x8mv.Checked)
                            x265B8x8mv.Checked = false;
                        if (x265P4x4mv.Checked)
                            x265P4x4mv.Checked = false;
                        if (x265P8x8mv.Checked)
                            x265P8x8mv.Checked = false;
                        if (x265METype.SelectedIndex != 0)
                            x265METype.SelectedIndex = 0;
                        if (x265SubpelRefinement.SelectedIndex != 1)
                            x265SubpelRefinement.SelectedIndex = 1;
                        if (!x265MixedReferences.Checked)
                            x265MixedReferences.Checked = true;
                        if (trellis.SelectedIndex != 0)
                            trellis.SelectedIndex = 0;
                        if (mbtree.Checked)
                            mbtree.Checked = false;
                        mbtree.Enabled = false;
                        if (x265BframePredictionMode.SelectedIndex != 1)
                            x265BframePredictionMode.SelectedIndex = 1;
                        if (!cabac.Checked)
                            cabac.Checked = true;
                        if (!x265DeblockActive.Checked)
                            x265DeblockActive.Checked = true;
                        if (!scenecut.Checked)
                            scenecut.Checked = true;
                        if (this.x265MERange.Value != 16)
                            this.x265MERange.Value = 16;
                        if (noFastPSkip.Checked)
                            noFastPSkip.Checked = false;
                        if (x265NewAdaptiveBframes.SelectedIndex != 1)
                            x265NewAdaptiveBframes.SelectedIndex = 1;
                        if (!this.x265WeightedBPrediction.Checked)
                            this.x265WeightedBPrediction.Checked = true;
                        if (!adaptiveDCT.Checked)
                            adaptiveDCT.Checked = true;
                    }
                    break;
                case 2: // Very Fast
                    {
                        if (macroblockOptions.SelectedIndex != 3)
                            macroblockOptions.SelectedIndex = 3;
                        if (x265METype.SelectedIndex != 1)
                            x265METype.SelectedIndex = 1;
                        if (x265SubpelRefinement.SelectedIndex != 2)
                            x265SubpelRefinement.SelectedIndex = 2;
                        if (!x265MixedReferences.Checked)
                            x265MixedReferences.Checked = true;
                        if (trellis.SelectedIndex != 0)
                            trellis.SelectedIndex = 0;
                        if (!mbtree.Checked)
                            mbtree.Checked = true;
                        mbtree.Enabled = true;
                        if (x265BframePredictionMode.SelectedIndex != 1)
                            x265BframePredictionMode.SelectedIndex = 1;
                        if (!cabac.Checked)
                            cabac.Checked = true;
                        if (!x265DeblockActive.Checked)
                             x265DeblockActive.Checked = true;
                        if (!scenecut.Checked)
                            scenecut.Checked = true;
                        if (this.x265MERange.Value != 16)
                            this.x265MERange.Value = 16;
                        if (noFastPSkip.Checked)
                            noFastPSkip.Checked = false;
                        if (x265NewAdaptiveBframes.SelectedIndex != 1)
                            x265NewAdaptiveBframes.SelectedIndex = 1;
                        if (!this.x265WeightedBPrediction.Checked)
                            this.x265WeightedBPrediction.Checked = true;
                        if (!adaptiveDCT.Checked)
                            adaptiveDCT.Checked = true;
                    }
                    break;
                case 3: // Faster
                    {
                        if (x265SubpelRefinement.SelectedIndex != 4)
                            x265SubpelRefinement.SelectedIndex = 4;
                        if (!mbtree.Checked)
                            mbtree.Checked = true;
                        mbtree.Enabled = true;
                        if (trellis.SelectedIndex != 1)
                            trellis.SelectedIndex = 1;
                        if (!x265MixedReferences.Checked)
                             x265MixedReferences.Checked = true;
                        if (x265METype.SelectedIndex != 1)
                            x265METype.SelectedIndex = 1;
                        if (macroblockOptions.SelectedIndex != 3)
                            macroblockOptions.SelectedIndex = 3;
                        if (x265BframePredictionMode.SelectedIndex != 1)
                            x265BframePredictionMode.SelectedIndex = 1;
                        if (!cabac.Checked)
                            cabac.Checked = true;
                        if (!x265DeblockActive.Checked)
                             x265DeblockActive.Checked = true;
                        if (!scenecut.Checked)
                             scenecut.Checked = true;
                        if (this.x265MERange.Value != 16)
                            this.x265MERange.Value = 16;
                        if (noFastPSkip.Checked)
                            noFastPSkip.Checked = false;
                        if (x265NewAdaptiveBframes.SelectedIndex != 1)
                            x265NewAdaptiveBframes.SelectedIndex = 1;
                        if (!this.x265WeightedBPrediction.Checked)
                            this.x265WeightedBPrediction.Checked = true;
                        if (adaptiveDCT.Checked)
                            adaptiveDCT.Checked = false;
                    }
                    break;
                case 4: // Fast
                    {
                        if (x265SubpelRefinement.SelectedIndex != 6)
                            x265SubpelRefinement.SelectedIndex = 6;
                        if (trellis.SelectedIndex != 1)
                            trellis.SelectedIndex = 1;
                        if (x265MixedReferences.Checked)
                             x265MixedReferences.Checked = false;
                        if (!mbtree.Checked)
                             mbtree.Checked = true;
                        mbtree.Enabled = true;
                        if (x265METype.SelectedIndex != 1)
                            x265METype.SelectedIndex = 1;
                        if (macroblockOptions.SelectedIndex != 3)
                            macroblockOptions.SelectedIndex = 3;
                        if (x265BframePredictionMode.SelectedIndex != 1)
                            x265BframePredictionMode.SelectedIndex = 1;
                        if (!cabac.Checked)
                            cabac.Checked = true;
                        if (!x265DeblockActive.Checked)
                             x265DeblockActive.Checked = true;
                        if (!scenecut.Checked)
                             scenecut.Checked = true;
                        if (this.x265MERange.Value != 16)
                            this.x265MERange.Value = 16;
                        if (noFastPSkip.Checked)
                            noFastPSkip.Checked = false;
                        if (x265NewAdaptiveBframes.SelectedIndex != 1)
                            x265NewAdaptiveBframes.SelectedIndex = 1;
                        if (!this.x265WeightedBPrediction.Checked)
                            this.x265WeightedBPrediction.Checked = true;
                        if (adaptiveDCT.Checked)
                            adaptiveDCT.Checked = false;
                    }
                    break;
                case 5: // Medium
                    {
                        if (x265METype.SelectedIndex != 1)
                            x265METype.SelectedIndex = 1;
                        if (x265SubpelRefinement.SelectedIndex != 7)
                            x265SubpelRefinement.SelectedIndex = 7;
                        if (x265NewAdaptiveBframes.SelectedIndex != 1)
                            x265NewAdaptiveBframes.SelectedIndex = 1;
                        if (x265BframePredictionMode.SelectedIndex != 1)
                            x265BframePredictionMode.SelectedIndex = 1;
                        if (trellis.SelectedIndex != 1)
                            trellis.SelectedIndex = 1;
                        if (x265MixedReferences.Checked)
                            x265MixedReferences.Checked = false;
                        if (!mbtree.Checked)
                            mbtree.Checked = true;
                        mbtree.Enabled = true;
                        if (macroblockOptions.SelectedIndex != 3)
                            macroblockOptions.SelectedIndex = 3;
                        if (!cabac.Checked)
                            cabac.Checked = true;
                        if (!x265DeblockActive.Checked)
                            x265DeblockActive.Checked = true;
                        if (!scenecut.Checked)
                            scenecut.Checked = true;
                        if (this.x265MERange.Value != 16)
                            this.x265MERange.Value = 16;
                        if (noFastPSkip.Checked)
                            noFastPSkip.Checked = false;
                        if (!this.x265WeightedBPrediction.Checked)
                            this.x265WeightedBPrediction.Checked = true;
                        if (adaptiveDCT.Checked)
                            adaptiveDCT.Checked = false;
                    }
                    break;
                case 6: // Slow
                    {
                        if (x265METype.SelectedIndex != 2)
                            x265METype.SelectedIndex = 2;
                        if (x265SubpelRefinement.SelectedIndex != 8)
                            x265SubpelRefinement.SelectedIndex = 8;
                        if (x265NewAdaptiveBframes.SelectedIndex != 2)
                            x265NewAdaptiveBframes.SelectedIndex = 2;
                        if (x265BframePredictionMode.SelectedIndex != 3)
                            x265BframePredictionMode.SelectedIndex = 3;
                        if (trellis.SelectedIndex != 1)
                            trellis.SelectedIndex = 1;
                        if (x265MixedReferences.Checked)
                             x265MixedReferences.Checked = false;
                        if (!mbtree.Checked)
                             mbtree.Checked = true;
                        mbtree.Enabled = true;
                        if (macroblockOptions.SelectedIndex != 3)
                            macroblockOptions.SelectedIndex = 3;
                        if (!cabac.Checked)
                            cabac.Checked = true;
                        if (!x265DeblockActive.Checked)
                             x265DeblockActive.Checked = true;
                        if (!scenecut.Checked)
                             scenecut.Checked = true;
                        if (this.x265MERange.Value != 16)
                            this.x265MERange.Value = 16;
                        if (noFastPSkip.Checked)
                            noFastPSkip.Checked = false;
                        if (!this.x265WeightedBPrediction.Checked)
                            this.x265WeightedBPrediction.Checked = true;
                        if (adaptiveDCT.Checked)
                            adaptiveDCT.Checked = false;
                    }
                    break;
                case 7: // Slower
                    {
                        if (x265METype.SelectedIndex != 2)
                            x265METype.SelectedIndex = 2;
                        if (x265SubpelRefinement.SelectedIndex != 9)
                            x265SubpelRefinement.SelectedIndex = 9;
                        if (x265NewAdaptiveBframes.SelectedIndex != 2)
                            x265NewAdaptiveBframes.SelectedIndex = 2;
                        if (trellis.SelectedIndex != 2)
                            trellis.SelectedIndex = 2;
                        if (x265BframePredictionMode.SelectedIndex != 3)
                            x265BframePredictionMode.SelectedIndex = 3;
                        if (macroblockOptions.SelectedIndex != 0)
                            macroblockOptions.SelectedIndex = 0;
                        if (x265MixedReferences.Checked)
                             x265MixedReferences.Checked = false;
                        if (!mbtree.Checked)
                            mbtree.Checked = true;
                        mbtree.Enabled = true;
                        if (!cabac.Checked)
                            cabac.Checked = true;
                        if (!x265DeblockActive.Checked)
                             x265DeblockActive.Checked = true;
                        if (!scenecut.Checked)
                             scenecut.Checked = true;
                        if (this.x265MERange.Value != 16)
                            this.x265MERange.Value = 16;
                        if (noFastPSkip.Checked)
                            noFastPSkip.Checked = false;
                        if (!this.x265WeightedBPrediction.Checked)
                            this.x265WeightedBPrediction.Checked = true;
                        if (adaptiveDCT.Checked)
                            adaptiveDCT.Checked = false;
                    }
                    break;
                case 8: // Very Slow
                    {
                        if (x265METype.SelectedIndex != 2)
                            x265METype.SelectedIndex = 2;
                        if (x265SubpelRefinement.SelectedIndex != 10)
                            x265SubpelRefinement.SelectedIndex = 10;
                        if (this.x265MERange.Value != 24)
                            this.x265MERange.Value = 24; 
                        if (x265NewAdaptiveBframes.SelectedIndex != 2)
                            x265NewAdaptiveBframes.SelectedIndex = 2;
                        if (x265BframePredictionMode.SelectedIndex != 3)
                            x265BframePredictionMode.SelectedIndex = 3;
                        if (macroblockOptions.SelectedIndex != 0)
                            macroblockOptions.SelectedIndex = 0;
                        if (trellis.SelectedIndex != 2)
                            trellis.SelectedIndex = 2;
                        if (x265MixedReferences.Checked)
                             x265MixedReferences.Checked = false;
                        if (!mbtree.Checked)
                            mbtree.Checked = true;
                        mbtree.Enabled = true;
                        if (!cabac.Checked)
                            cabac.Checked = true;
                        if (!x265DeblockActive.Checked)
                             x265DeblockActive.Checked = true;
                        if (!scenecut.Checked)
                             scenecut.Checked = true;
                        if (noFastPSkip.Checked)
                            noFastPSkip.Checked = false;
                        if (!this.x265WeightedBPrediction.Checked)
                            this.x265WeightedBPrediction.Checked = true;
                        if (adaptiveDCT.Checked)
                            adaptiveDCT.Checked = false;
                    }
                    break;
                case 9: // Placebo
                    {
                        if (x265METype.SelectedIndex != 4)
                            x265METype.SelectedIndex = 4;
                        if (x265SubpelRefinement.SelectedIndex != 11)
                            x265SubpelRefinement.SelectedIndex = 11;
                        if (this.x265MERange.Value != 24)
                            this.x265MERange.Value = 24;
                        if (x265NewAdaptiveBframes.SelectedIndex != 2)
                            x265NewAdaptiveBframes.SelectedIndex = 2;
                        if (x265BframePredictionMode.SelectedIndex != 3)
                            x265BframePredictionMode.SelectedIndex = 3;
                        if (macroblockOptions.SelectedIndex != 0)
                            macroblockOptions.SelectedIndex = 0;
                        if (!noFastPSkip.Checked)
                            noFastPSkip.Checked = true;
                        if (trellis.SelectedIndex != 2)
                            trellis.SelectedIndex = 2;
                        if (x265MixedReferences.Checked)
                             x265MixedReferences.Checked = false;
                        if (!mbtree.Checked)
                            mbtree.Checked = true;
                        mbtree.Enabled = true;
                        if (!cabac.Checked)
                            cabac.Checked = true;
                        if (!x265DeblockActive.Checked)
                             x265DeblockActive.Checked = true;
                        if (!scenecut.Checked)
                             scenecut.Checked = true;
                        if (!this.x265WeightedBPrediction.Checked)
                            this.x265WeightedBPrediction.Checked = true;
                        if (adaptiveDCT.Checked)
                            adaptiveDCT.Checked = false;
                    }
                    break;
                default:
                    break;
            }
        }
        #endregion
        #region dropdowns
        private void setNonQPOptionsEnabled(bool enabled)
        {
            rcCard.Enabled = enabled;
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

        private void doEncodingModeAdjustments()
        {
            if ((VideoCodecSettings.VideoEncodingMode)x265EncodingMode.SelectedIndex != VideoCodecSettings.VideoEncodingMode.CQ
                && (VideoCodecSettings.VideoEncodingMode)x265EncodingMode.SelectedIndex != VideoCodecSettings.VideoEncodingMode.quality)
            {
                this.x265BitrateQuantizerLabel.Text = "Bitrate";
                x265BitrateQuantizer.Maximum = 500000;
                x265BitrateQuantizer.Minimum = 0;
                x265BitrateQuantizer.DecimalPlaces = 0;
                x265BitrateQuantizer.Increment = 10;
                tooltipHelp.SetToolTip(x265BitrateQuantizer, SelectHelpText("bitrate"));
            }
            else
            {
                if ((VideoCodecSettings.VideoEncodingMode)x265EncodingMode.SelectedIndex == VideoCodecSettings.VideoEncodingMode.CQ)
                {
                    this.x265BitrateQuantizerLabel.Text = "Quantizer";
                    tooltipHelp.SetToolTip(x265BitrateQuantizer, SelectHelpText("qp"));
                    x265BitrateQuantizer.Maximum = 51;
                    x265BitrateQuantizer.Minimum = 0;
                    x265BitrateQuantizer.Value = (int)x265BitrateQuantizer.Value; // makes sure it is an integer, in case we just swapped from crf                    
                    x265BitrateQuantizer.DecimalPlaces = 0;
                    x265BitrateQuantizer.Increment = 1;
                }
                if ((VideoCodecSettings.VideoEncodingMode)x265EncodingMode.SelectedIndex == VideoCodecSettings.VideoEncodingMode.quality)
                {
                    this.x265BitrateQuantizerLabel.Text = "Quality";
                    tooltipHelp.SetToolTip(x265BitrateQuantizer, SelectHelpText("crf"));
                    x265BitrateQuantizer.Maximum = 51;
                    x265BitrateQuantizer.Minimum = 0.0M;
                    x265BitrateQuantizer.DecimalPlaces = 1;
                    x265BitrateQuantizer.Increment = 0.1M;
                }
            }

            // We check whether the bitrate/quality text needs to be changed
            if (isBitrateMode(lastEncodingMode) != isBitrateMode((VideoCodecSettings.VideoEncodingMode)x265EncodingMode.SelectedIndex))
            {
                if ((VideoCodecSettings.VideoEncodingMode)x265EncodingMode.SelectedIndex != VideoCodecSettings.VideoEncodingMode.CQ
                    && (VideoCodecSettings.VideoEncodingMode)x265EncodingMode.SelectedIndex != VideoCodecSettings.VideoEncodingMode.quality)
                    this.x265BitrateQuantizer.Value = lastBitrateEncodingValue;
                else
                    this.x265BitrateQuantizer.Value = lastQuantizerEncodingValue;
            }

            lastEncodingMode = (VideoCodecSettings.VideoEncodingMode)x265EncodingMode.SelectedIndex;
            if ((VideoCodecSettings.VideoEncodingMode)x265EncodingMode.SelectedIndex != VideoCodecSettings.VideoEncodingMode.CQ
                && (VideoCodecSettings.VideoEncodingMode)x265EncodingMode.SelectedIndex != VideoCodecSettings.VideoEncodingMode.quality)
                lastBitrateEncodingValue = (int)this.x265BitrateQuantizer.Value;
            else
                lastQuantizerEncodingValue = (int)this.x265BitrateQuantizer.Value;
        }
        #endregion
        #region level -> mb
        /// <summary>
        /// adjust the mb selection dropdown in function of the selected profile and the activated
        /// mb options
        /// </summary>
        public void doMBOptionsAdjustments()
        {
            if (!x265P8x8mv.Checked)
            {
                // x265P4x4mv.Checked = false;
                x265P4x4mv.Enabled = false;
            }

        }
        #endregion

        #region codec-specific overload functions
        protected override string getCommandline()
        {
            ulong x = 1;
            return x265Encoder.genCommandline(null, null, null, -1, -1, ref x, Settings as x265Settings, null);
        }

        /// <summary>
        /// Does all the necessary adjustments after a GUI change has been made.
        /// </summary>
        protected override void doCodecSpecificAdjustments()
        {
            doEncodingModeAdjustments();
            x265DialogTriStateAdjustment();
        }

        /// <summary>
        /// The method by which codecs can add things to the Load event
        /// </summary>
        protected override void doCodecSpecificLoadAdjustments()
        {
            if (x265EncodingMode.SelectedIndex == -1)
                this.x265EncodingMode.SelectedIndex = 0;
            if (x265SubpelRefinement.SelectedIndex == -1)
                this.x265SubpelRefinement.SelectedIndex = 7;
            if (x265BframePredictionMode.SelectedIndex == -1)
                this.x265BframePredictionMode.SelectedIndex = 1;
            if (x265METype.SelectedIndex == -1)
                this.x265METype.SelectedIndex = 0;
            if (macroblockOptions.SelectedIndex == -1)
                macroblockOptions.SelectedIndex = 3;
            if (cqmComboBox1.SelectedIndex == -1)
                cqmComboBox1.SelectedIndex = 0; // flat matrix
            if (cbAQMode.SelectedIndex == -1)
                cbAQMode.SelectedIndex = 1;
            if (x265Tunes.SelectedIndex == -1)
                x265Tunes.SelectedIndex = 0; // Default
            if (cbBPyramid.SelectedIndex == -1)
                cbBPyramid.SelectedIndex = 2;
            if (hevcProfile.SelectedIndex == -1)
                hevcProfile.SelectedIndex = 0;
            lastEncodingMode = (VideoCodecSettings.VideoEncodingMode)this.x265EncodingMode.SelectedIndex;
            if ((VideoCodecSettings.VideoEncodingMode)x265EncodingMode.SelectedIndex != VideoCodecSettings.VideoEncodingMode.CQ
                && (VideoCodecSettings.VideoEncodingMode)x265EncodingMode.SelectedIndex != VideoCodecSettings.VideoEncodingMode.quality)
                lastBitrateEncodingValue = (int)this.x265BitrateQuantizer.Value;
            else
                lastQuantizerEncodingValue = (int)this.x265BitrateQuantizer.Value;
            
            try
            {
                string p = System.IO.Path.Combine (Application.StartupPath, "Data");
                p = System.IO.Path.Combine (p, "ContextHelp.xml");
                ContextHelp.Load(p);
                SetToolTips();
            }
            catch
            {
                MessageBox.Show("The ContextHelp.xml file could not be found. Please check in the 'Data' directory to see if it exists. Help tooltips will not be available.", "File Not Found", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            advancedSettings_CheckedChanged(null, null); // to open/close advanced tabs
            tbx265Presets_Scroll(null, null); // to update the trackbar label
        }

        /// <summary>
        /// Returns whether settings is lavcSettings
        /// </summary>
        /// <param name="settings">The settings to check</param>
        /// <returns>Whether the settings are valid</returns>
        protected override bool isValidSettings(VideoCodecSettings settings)
        {
            return settings is x265Settings;
        }

        /// <summary>
        /// Returns a new instance of lavcSettings.
        /// </summary>
        /// <returns>A new instance of lavcSettings</returns>
        protected override VideoCodecSettings defaultSettings()
        {
            return new x265Settings();
        }

        /// <summary>
        /// gets / sets the settings currently displayed on the GUI
        /// </summary>
        public x265Settings Settings
        {
            get
            {
                x265Settings xs = new x265Settings();
                xs.DeadZoneInter = (int)deadzoneInter.Value;
                xs.DeadZoneIntra = (int)deadzoneIntra.Value;
                xs.NoDCTDecimate = this.noDCTDecimateOption.Checked;
                xs.NoFastPSkip = noFastPSkip.Checked;
                xs.NoMixedRefs = x265MixedReferences.Checked;
                xs.VideoEncodingType = (VideoCodecSettings.VideoEncodingMode)x265EncodingMode.SelectedIndex;
                xs.BitrateQuantizer = (int)x265BitrateQuantizer.Value;
                xs.QuantizerCRF = x265BitrateQuantizer.Value;
                xs.KeyframeInterval = (int)x265KeyframeInterval.Value;
                xs.NbRefFrames = (int)this.x265NumberOfRefFrames.Value;
                xs.NbBframes = (int)this.x265NumberOfBFrames.Value;
                xs.NewAdaptiveBFrames = x265NewAdaptiveBframes.SelectedIndex;
                xs.PsyRDO = this.PsyRD.Value;
                xs.PsyTrellis = this.PsyTrellis.Value;
                xs.Deblock = x265DeblockActive.Checked;
                xs.AlphaDeblock = (int)x265AlphaDeblock.Value;
                xs.BetaDeblock = (int)x265BetaDeblock.Value;
                xs.Cabac = cabac.Checked;
                xs.SubPelRefinement = this.x265SubpelRefinement.SelectedIndex;
                xs.WeightedBPrediction = x265WeightedBPrediction.Checked;
                xs.WeightedPPrediction = x265WeightedPPrediction.SelectedIndex;
                xs.ChromaME = this.x265ChromaMe.Checked;
                xs.MacroBlockOptions = macroblockOptions.SelectedIndex; 
                xs.P8x8mv = x265P8x8mv.Checked;
                xs.B8x8mv = x265B8x8mv.Checked;
                xs.I4x4mv = x265I4x4mv.Checked;
                xs.I8x8mv = x265I8x8mv.Checked;
                xs.P4x4mv = x265P4x4mv.Checked;
                xs.MinQuantizer = (int)x265MinimimQuantizer.Value;
                xs.MaxQuantizer = (int)x265MaximumQuantizer.Value;
                xs.MaxQuantDelta = (int)x265MaxQuantDelta.Value;
                xs.CreditsQuantizer = (int)this.x265CreditsQuantizer.Value;
                xs.ChromaQPOffset = this.x265ChromaQPOffset.Value;
                xs.IPFactor = x265IPFrameFactor.Value;
                xs.PBFactor = x265PBFrameFactor.Value;
                xs.VBVBufferSize = (int)x265VBVBufferSize.Value;
                xs.VBVMaxBitrate = (int)x265VBVMaxRate.Value;
                xs.VBVInitialBuffer = x265VBVInitialBuffer.Value;
                xs.BitrateVariance = x265RateTol.Value;
                xs.QuantCompression = x265QuantizerCompression.Value;
                xs.TempComplexityBlur = (int)x265TempFrameComplexityBlur.Value;
                xs.TempQuanBlurCC = x265TempQuantBlur.Value;
                xs.SCDSensitivity = (int)this.x265SCDSensitivity.Value;
                xs.BframeBias = (int)this.x265BframeBias.Value;
                xs.BframePredictionMode = this.x265BframePredictionMode.SelectedIndex;
                xs.METype = this.x265METype.SelectedIndex;
                xs.MERange = (int)x265MERange.Value;
                xs.NbThreads = (int)x265NbThreads.Value;
                xs.AdaptiveDCT = adaptiveDCT.Checked;
                xs.CustomEncoderOptions = customCommandlineOptions.Text;
                if (cqmComboBox1.SelectedIndex > 1)
                    xs.QuantizerMatrixType = 2;
                else
                    xs.QuantizerMatrixType = cqmComboBox1.SelectedIndex;
                xs.QuantizerMatrix = cqmComboBox1.SelectedText;
                xs.NoiseReduction = (int)NoiseReduction.Value;
                xs.AQmode = (int)cbAQMode.SelectedIndex;
                xs.AQstrength = numAQStrength.Value;
                xs.OpenGopValue = chkOpenGop.Checked;
                xs.FakeInterlaced = fakeInterlaced.Checked;
                xs.x265PresetLevel = (x265Settings.x265PresetLevelModes)this.tbx265Presets.Value;
                xs.x265PsyTuning = getPsyTuning();
                xs.AdvancedSettings = advancedSettings.Checked;
                xs.NoMBTree = mbtree.Checked;
                xs.Lookahead = (int)lookahead.Value;
                xs.NoPsy = nopsy.Checked;
                xs.Scenecut = scenecut.Checked;
                xs.SlicesNb = (int)this.slicesnb.Value;
                xs.MaxSliceSyzeBytes = (int)this.maxSliceSizeBytes.Value;
                xs.MaxSliceSyzeMBs = (int)this.maxSliceSizeMB.Value;
                xs.x265BFramePyramid = this.cbBPyramid.SelectedIndex;
                xs.x265GOPCalculation = this.cbGOPCalculation.SelectedIndex;
                xs.Nalhrd = (int)x265hrd.SelectedIndex;
                xs.X26510Bits = ch10BitsEncoder.Checked;
                xs.Profile = hevcProfile.SelectedIndex;
                return xs;
            }
            set
            {
                // Warning! The ordering of components matters because of the dependency code!
                if (value == null)
                    return;
                
                x265Settings xs = value;
                updating = true;
                tbx265Presets.Value = (int)xs.x265PresetLevel;
                if (xs.Profile > 2)
                    hevcProfile.SelectedIndex = 0;
                else
                    hevcProfile.SelectedIndex = xs.Profile;
                x265Tunes.SelectedItem = EnumProxy.Create(xs.x265PsyTuning);
                deadzoneInter.Value = xs.DeadZoneInter;
                deadzoneIntra.Value = xs.DeadZoneIntra;
                noDCTDecimateOption.Checked = xs.NoDCTDecimate;
                x265EncodingMode.SelectedIndex = (int)xs.VideoEncodingType;
                doEncodingModeAdjustments();
                this.x265NumberOfRefFrames.Value = xs.NbRefFrames;
                this.x265NumberOfBFrames.Value = xs.NbBframes;
                noFastPSkip.Checked = xs.NoFastPSkip;
                this.x265SubpelRefinement.SelectedIndex = xs.SubPelRefinement;
                x265BitrateQuantizer.Value = (isBitrateMode(xs.VideoEncodingType) || xs.QuantizerCRF == 0) ? xs.BitrateQuantizer : xs.QuantizerCRF;
                x265KeyframeInterval.Text = xs.KeyframeInterval.ToString() ;
                x265NewAdaptiveBframes.SelectedIndex = xs.NewAdaptiveBFrames;
                x265DeblockActive.Checked = xs.Deblock;
                x265AlphaDeblock.Value = xs.AlphaDeblock;
                x265BetaDeblock.Value = xs.BetaDeblock;
                cabac.Checked = xs.Cabac;
                x265ChromaMe.Checked = xs.ChromaME;
                PsyRD.Value = xs.PsyRDO;
                PsyTrellis.Value = xs.PsyTrellis;
                macroblockOptions.SelectedIndex = xs.MacroBlockOptions;
                if (macroblockOptions.SelectedIndex != 1)
                {
                    adaptiveDCT.Checked = xs.AdaptiveDCT;
                    x265P8x8mv.Checked = xs.P8x8mv;
                    x265B8x8mv.Checked = xs.B8x8mv;
                    x265I4x4mv.Checked = xs.I4x4mv;
                    x265I8x8mv.Checked = xs.I8x8mv;
                    x265P4x4mv.Checked = xs.P4x4mv;
                }
                x265MinimimQuantizer.Value = xs.MinQuantizer;
                x265MaximumQuantizer.Value = xs.MaxQuantizer;
                x265MaxQuantDelta.Value = xs.MaxQuantDelta;
                this.x265CreditsQuantizer.Value = xs.CreditsQuantizer;
                x265IPFrameFactor.Value = xs.IPFactor;
                x265PBFrameFactor.Value = xs.PBFactor;
                x265ChromaQPOffset.Value = xs.ChromaQPOffset;
                if (xs.VBVBufferSize > 0)
                {
                    this.x265VBVMaxRate.Enabled = this.x265VBVMaxRateLabel.Enabled = true;
                    x265VBVBufferSize.Text = xs.VBVBufferSize.ToString();
                }
                else
                {
                    this.x265VBVMaxRate.Enabled = this.x265VBVMaxRateLabel.Enabled = false;
                    x265VBVBufferSize.Text = "0";
                }
                if (xs.VBVMaxBitrate > 0)
                {
                    this.x265VBVMaxRate.Enabled = this.x265VBVMaxRateLabel.Enabled = true;
                    x265VBVMaxRate.Text = xs.VBVMaxBitrate.ToString();
                }
                else
                {
                    x265VBVBufferSize.Text = "0";
                    x265VBVMaxRate.Text = "0";
                }
                x265VBVInitialBuffer.Value = xs.VBVInitialBuffer;
                x265RateTol.Value = xs.BitrateVariance;
                x265QuantizerCompression.Value = xs.QuantCompression;
                x265TempFrameComplexityBlur.Value = xs.TempComplexityBlur;
                x265TempQuantBlur.Value = xs.TempQuanBlurCC;
                this.x265SCDSensitivity.Value = xs.SCDSensitivity;
                this.x265BframeBias.Value = xs.BframeBias;
                this.x265BframePredictionMode.SelectedIndex = xs.BframePredictionMode;
                this.x265METype.SelectedIndex = xs.METype;
                x265MERange.Value = xs.MERange;
                x265NbThreads.Value = xs.NbThreads;
                x265MinGOPSize.Text = xs.MinGOPSize.ToString();
                customCommandlineOptions.Text = xs.CustomEncoderOptions;
                cqmComboBox1.SelectedObject = xs.QuantizerMatrix;
                cbAQMode.SelectedIndex = xs.AQmode;
                chkOpenGop.Checked = xs.OpenGopValue;
                fakeInterlaced.Checked = xs.FakeInterlaced;
                numAQStrength.Value = xs.AQstrength;
                NoiseReduction.Text = xs.NoiseReduction.ToString();
                advancedSettings.Checked = xs.AdvancedSettings;
                lookahead.Value = xs.Lookahead;
                mbtree.Checked = xs.NoMBTree;
                nopsy.Checked = xs.NoPsy;
                x265MixedReferences.Checked = xs.NoMixedRefs;
                scenecut.Checked = xs.Scenecut;
                this.slicesnb.Value = xs.SlicesNb;
                this.maxSliceSizeBytes.Value = xs.MaxSliceSyzeBytes;
                this.maxSliceSizeMB.Value = xs.MaxSliceSyzeMBs;
                this.cbBPyramid.SelectedIndex = xs.x265BFramePyramid;
                this.cbGOPCalculation.SelectedIndex = xs.x265GOPCalculation;
                x265WeightedBPrediction.Checked = xs.WeightedBPrediction;
                x265WeightedPPrediction.SelectedIndex = xs.WeightedPPrediction;
                x265hrd.SelectedIndex = xs.Nalhrd;
                ch10BitsEncoder.Checked = xs.X26510Bits;
                updating = false;
                genericUpdate();
            }
        }
        #endregion
        #region events
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

        #endregion
        #region ContextHelp
        private string SelectHelpText(string node)
        {
            StringBuilder HelpText = new StringBuilder(64);

            try
            {
                string xpath = "/ContextHelp/Codec[@name='x265']/" + node;
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
            tooltipHelp.SetToolTip(x265Tunes, SelectHelpText("tunes"));
            //tooltipHelp.SetToolTip(cbTarget, SelectHelpText("targetmode"));
            tooltipHelp.SetToolTip(tbx265Presets, SelectHelpText("presets"));
            tooltipHelp.SetToolTip(x265BitrateQuantizer, SelectHelpText("bitrate"));
            tooltipHelp.SetToolTip(x265EncodingMode, SelectHelpText("encodingmode"));
            
            /**************/
            /* Frame-Type */
            /**************/
            tooltipHelp.SetToolTip(x265KeyframeInterval, SelectHelpText("keyint"));
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

            /*************************/
            /* Rate Control Tooltips */
            /*************************/
            tooltipHelp.SetToolTip(x265MinimimQuantizer, SelectHelpText("qpmin"));
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

            /*************************/
            /* Analysis Tooltips */
            /*************************/
            tooltipHelp.SetToolTip(cbAQMode, SelectHelpText("aqmode"));
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

            /**************************/
            /* Misc Tooltips */
            /**************************/
            tooltipHelp.SetToolTip(x265NbThreads, SelectHelpText("threads"));
            tooltipHelp.SetToolTip(customCommandlineOptions, SelectHelpText("customcommandline"));
        }
        #endregion
        #region GUI State adjustment
        private void x265DialogTriStateAdjustment()
        {
            // First we do the Profile Adjustments
            #region profile adjustments
            #endregion

            // Now we do B frames adjustments
            #region b-frames
            if (this.x265NumberOfBFrames.Value == 0)
            {
                this.x265NewAdaptiveBframes.Enabled = false;
                this.x265AdaptiveBframesLabel.Enabled = false;
                this.x265BframePredictionMode.Enabled = false;
                this.x265BframePredictionModeLabel.Enabled = false;
                this.x265WeightedBPrediction.Checked = false;
                this.x265WeightedBPrediction.Enabled = false;
                this.x265BframeBias.Value = 0;
                this.x265BframeBias.Enabled = false;
                this.x265BframeBiasLabel.Enabled = false;
                this.cbBPyramid.Enabled = false;
            }
            else
            {
                this.x265NewAdaptiveBframes.Enabled = true;
                this.x265AdaptiveBframesLabel.Enabled = true;
                this.x265BframePredictionMode.Enabled = true;
                this.x265BframePredictionModeLabel.Enabled = true;
                this.x265WeightedBPrediction.Enabled = true;
                // We can enable these if we don't have turbo options
                this.x265BframeBias.Enabled = true;
                this.x265BframeBiasLabel.Enabled = true;
                if (this.x265NumberOfBFrames.Value >= 2) // pyramid requires at least two b-frames
                    this.cbBPyramid.Enabled = true;
                else
                    this.cbBPyramid.Enabled = false;
            }
            #endregion

            // Now we do some additional checks -- ref frames, cabac
            #region extra checks
            if (!string.IsNullOrEmpty(x265VBVBufferSize.Text))
            {
                this.x265VBVMaxRate.Enabled = true;
                this.x265VBVMaxRateLabel.Enabled = true;
            }
            if (this.x265NumberOfRefFrames.Value > 1) // mixed references require at least two reference frames
            {
                if (!this.x265MixedReferences.Enabled)
                    this.x265MixedReferences.Enabled = true;
            }
            else
                this.x265MixedReferences.Enabled = false;

            if (this.x265SubpelRefinement.SelectedIndex > 4)
            {
                this.PsyRD.Enabled = true;
                this.PsyRDLabel.Enabled = true;
            }
            else
            {
                this.PsyRD.Enabled = false;
                this.PsyRDLabel.Enabled = false;
            }
            if (this.trellis.SelectedIndex > 0)
            {
                this.PsyTrellis.Enabled = true;
                this.PsyTrellisLabel.Enabled = true;
            }
            else
            {
                this.PsyTrellis.Enabled = false;
                this.PsyTrellisLabel.Enabled = false;
            }
            #endregion
        }
        #endregion

        private void cqmComboBox1_SelectionChanged(object sender, string val)
        {
            genericUpdate();
        }

        private void VisitLink()
        {
            try
            {
                //Call the Process.Start method to open the default browser 
                //with a URL:
                System.Diagnostics.Process.Start("http://www.x265.org");
            }
            catch (Exception)
            {
                MessageBox.Show("Unable to open link that was clicked.");
            }
        }

        private void cbAQMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbAQMode.SelectedIndex != 0)
                numAQStrength.Enabled = true;
            else 
                numAQStrength.Enabled = false;
            genericUpdate();
        }

        private void tbx265Presets_Scroll(object sender, EventArgs e)
        {
            switch (tbx265Presets.Value)
            {
                case 0: lbPreset.Text = "Ultra Fast"; break;
                case 1: lbPreset.Text = "Super Fast"; break;
                case 2: lbPreset.Text = "Very Fast"; break;
                case 3: lbPreset.Text = "Faster"; break;
                case 4: lbPreset.Text = "Fast"; break;
                case 5: lbPreset.Text = "Medium"; break;
                case 6: lbPreset.Text = "Slow"; break;
                case 7: lbPreset.Text = "Slower"; break;
                case 8: lbPreset.Text = "Very Slow"; break;
                case 9: lbPreset.Text = "Placebo"; break;
            }
            if (sender != null) // workaround so that the first loaded profile will not be overwritten
                doPresetsAdjustments();
            genericUpdate();
        }

        private MeGUI.core.gui.SettingsCard rcCard, aqCard, cqmCard, hevcProfileCard, hevcLevelCard;

        /// <summary>
        /// Windows 11 settings look: one page of cards per tab (see ModernDialog). The controls are only moved:
        /// names, events and the settings are unchanged. The labels which the code changes (text, enabled, visible)
        /// are used as titles of their rows.
        /// </summary>
        private void BuildModernLayout()
        {
            MeGUI.core.gui.ModernDialog d = new MeGUI.core.gui.ModernDialog(this);
            MeGUI.core.gui.SettingsPage main = d.CurrentPage;
            d.Section("Encoding mode");
            MeGUI.core.gui.SettingsCard modeCard = d.Card();
            modeCard.AddRow("Encoding mode", null, d.Wide(x265EncodingMode, 200));
            modeCard.AddLabelRow(x265BitrateQuantizerLabel, null, d.Wide(x265BitrateQuantizer, 110));
            d.Section("Preset");
            MeGUI.core.gui.SettingsCard presetCard = d.Card();
            presetCard.AddLabelRow(lbPreset, null, d.Wide(tbx265Presets, 320));
            d.Section("Tuning");
            MeGUI.core.gui.SettingsCard tuneCard = d.Card();
            tuneCard.AddRow("Tuning", null, d.Wide(x265Tunes, 220));
            d.Section("HEVC profile and level");
            hevcProfileCard = d.Card();
            hevcProfileCard.AddRow("HEVC profile", null, d.Wide(hevcProfile, 220));
            hevcLevelCard = d.Card();
            hevcLevelCard.AddRow("HEVC level", null, d.Wide(hevcLevel, 220));
            d.Section("Other settings");
            MeGUI.core.gui.SettingsCard otherCard = d.Card();
            ch10BitsEncoder.Text = "Enable 10-bits encoding";
            d.ToggleIn(otherCard, ch10BitsEncoder, null);
            otherCard.AddRow("Frame-threads (0 = auto)", null, d.Wide(x265NbThreads, 90));
            chkTuneFastDecode.Text = "Fast decode";
            d.ToggleIn(otherCard, chkTuneFastDecode, null);
            chkTuneZeroLatency.Text = "Zero latency";
            d.ToggleIn(otherCard, chkTuneZeroLatency, null);
            advancedSettings.Text = "Show advanced settings";
            d.ToggleIn(otherCard, advancedSettings, null);
            otherCard.AddRow("Help", null, helpButton1);
            hevcLevelCard.Enabled = false;
            MeGUI.core.gui.SettingsPage frame = d.BeginPage();
            d.Section("H.264 features");
            MeGUI.core.gui.SettingsCard c1 = d.Card();
            x265DeblockActive.Text = "Deblocking";
            d.ToggleIn(c1, x265DeblockActive, null);
            c1.AddRow("Deblocking strength", null, d.Wide(x265AlphaDeblock, 90));
            c1.AddRow("Deblocking threshold", null, d.Wide(x265BetaDeblock, 90));
            cabac.Text = "CABAC";
            d.ToggleIn(c1, cabac, null);
            d.Section("GOP size");
            MeGUI.core.gui.SettingsCard c2 = d.Card();
            c2.AddRow("GOP calculation", null, d.Wide(cbGOPCalculation, 160));
            c2.AddRow("Maximum GOP size (0 = infinite)", null, d.Wide(x265KeyframeInterval, 90));
            c2.AddRow("Minimum GOP size", null, d.Wide(x265MinGOPSize, 90));
            chkOpenGop.Text = "Open GOP";
            d.ToggleIn(c2, chkOpenGop, null);
            d.Section("B-frames");
            MeGUI.core.gui.SettingsCard c3 = d.Card();
            x265WeightedBPrediction.Text = "Weighted prediction for B-frames";
            d.ToggleIn(c3, x265WeightedBPrediction, null);
            c3.AddLabelRow(x265NumberOfBFramesLabel, null, d.Wide(x265NumberOfBFrames, 90));
            c3.AddLabelRow(x265BframeBiasLabel, null, d.Wide(x265BframeBias, 90));
            c3.AddLabelRow(x265AdaptiveBframesLabel, null, d.Wide(x265NewAdaptiveBframes, 160));
            c3.AddRow("B-pyramid", null, d.Wide(cbBPyramid, 160));
            d.Section("Other");
            MeGUI.core.gui.SettingsCard c4 = d.Card();
            c4.AddRow("Number of reference frames", null, d.Wide(x265NumberOfRefFrames, 90));
            c4.AddRow("Number of extra I-frames", null, d.Wide(x265SCDSensitivity, 90));
            c4.AddRow("P-frame weighted prediction", null, d.Wide(x265WeightedPPrediction, 160));
            c4.AddRow("Interlaced mode", null, d.Wide(cbInterlaceMode, 160));
            c4.AddRow("Pulldown", null, d.Wide(x265PullDown, 160));
            scenecut.Text = "Adaptive I-frame decision";
            d.ToggleIn(c4, scenecut, null);
            d.Section("Slicing");
            MeGUI.core.gui.SettingsCard c5 = d.Card();
            c5.AddRow("Number of slices by frame", null, d.Wide(slicesnb, 90));
            c5.AddRow("Max size (in bytes)", null, d.Wide(maxSliceSizeBytes, 90));
            c5.AddRow("Max size (in mbs)", null, d.Wide(maxSliceSizeMB, 90));
            MeGUI.core.gui.SettingsPage rate = d.BeginPage();
            d.Section("Quantizers");
            MeGUI.core.gui.SettingsCard c6 = d.Card();
            c6.AddRow("Min / max / delta", null, d.Wide(x265MinimimQuantizer, 80), d.Wide(x265MaximumQuantizer, 80), d.Wide(x265MaxQuantDelta, 80));
            c6.AddRow("Quantizers ratio (I:P / P:B)", null, d.Wide(x265IPFrameFactor, 80), d.Wide(x265PBFrameFactor, 80));
            c6.AddLabelRow(lbx265DeadZones, null, d.Wide(deadzoneInter, 80), d.Wide(deadzoneIntra, 80));
            c6.AddRow("Chroma QP offset", null, d.Wide(x265ChromaQPOffset, 90));
            c6.AddRow("Credits quantizer", null, d.Wide(x265CreditsQuantizer, 90));
            d.Section("Adaptive quantizers");
            aqCard = d.Card();
            aqCard.AddRow("Mode", null, d.Wide(cbAQMode, 180));
            aqCard.AddRow("Strength", null, d.Wide(numAQStrength, 90));
            d.Section("Quantizer matrices");
            cqmCard = d.Card();
            cqmCard.AddRow("Quantizer matrix", null, d.Wide(cqmComboBox1, 260));
            d.Section("Rate control");
            rcCard = d.Card();
            rcCard.AddRow("VBV buffer size", null, d.Wide(x265VBVBufferSize, 90));
            rcCard.AddLabelRow(x265VBVMaxRateLabel, null, d.Wide(x265VBVMaxRate, 90));
            rcCard.AddRow("VBV initial buffer", null, d.Wide(x265VBVInitialBuffer, 90));
            rcCard.AddLabelRow(x265RateTolLabel, null, d.Wide(x265RateTol, 90));
            rcCard.AddRow("Quantizer compression", null, d.Wide(x265QuantizerCompression, 90));
            rcCard.AddLabelRow(x265TempFrameComplexityBlurLabel, null, d.Wide(x265TempFrameComplexityBlur, 90));
            rcCard.AddLabelRow(x265TempQuantBlurLabel, null, d.Wide(x265TempQuantBlur, 90));
            rcCard.AddRow("Number of frames for lookahead", null, d.Wide(lookahead, 90));
            mbtree.Text = "Use MB-tree";
            d.ToggleIn(rcCard, mbtree, null);
            MeGUI.core.gui.SettingsPage analysis = d.BeginPage();
            d.Section("Motion estimation");
            MeGUI.core.gui.SettingsCard c7 = d.Card();
            x265ChromaMe.Text = "Chroma M.E.";
            d.ToggleIn(c7, x265ChromaMe, null);
            c7.AddRow("M.E. range", null, d.Wide(x265MERange, 90));
            c7.AddRow("M.E. algorithm", null, d.Wide(x265METype, 180));
            c7.AddRow("Subpixel refinement", null, d.Wide(x265SubpelRefinement, 180));
            d.Section("Macroblocks");
            MeGUI.core.gui.SettingsCard c8 = d.Card();
            c8.AddRow("Partitions", null, d.Wide(macroblockOptions, 160));
            adaptiveDCT.Text = "Adaptive DCT";
            d.ToggleIn(c8, adaptiveDCT, null);
            x265I4x4mv.Text = "I4x4";
            d.ToggleIn(c8, x265I4x4mv, null);
            x265I8x8mv.Text = "I8x8";
            d.ToggleIn(c8, x265I8x8mv, null);
            x265P4x4mv.Text = "P4x4";
            d.ToggleIn(c8, x265P4x4mv, null);
            x265P8x8mv.Text = "P8x8";
            d.ToggleIn(c8, x265P8x8mv, null);
            x265B8x8mv.Text = "B8x8";
            d.ToggleIn(c8, x265B8x8mv, null);
            d.Section("Extra");
            MeGUI.core.gui.SettingsCard c9 = d.Card();
            c9.AddRow("MV prediction mode", null, d.Wide(x265BframePredictionMode, 180));
            c9.AddRow("Trellis", null, d.Wide(trellis, 180));
            c9.AddLabelRow(PsyRDLabel, null, d.Wide(PsyRD, 90));
            c9.AddLabelRow(PsyTrellisLabel, null, d.Wide(PsyTrellis, 90));
            nopsy.Text = "No psychovisual enhancements";
            d.ToggleIn(c9, nopsy, null);
            noDCTDecimateOption.Text = "No DCT decimation";
            d.ToggleIn(c9, noDCTDecimateOption, null);
            noFastPSkip.Text = "No fast P-skip";
            d.ToggleIn(c9, noFastPSkip, null);
            x265MixedReferences.Text = "No mixed reference frames";
            d.ToggleIn(c9, x265MixedReferences, null);
            c9.AddRow("Noise reduction", null, d.Wide(NoiseReduction, 90));
            d.Section("Blu-ray");
            MeGUI.core.gui.SettingsCard c10 = d.Card();
            c10.AddRow("HRD info", null, d.Wide(x265hrd, 160));
            x265aud.Text = "Use access unit delimiters";
            d.ToggleIn(c10, x265aud, null);
            fakeInterlaced.Text = "Fake interlaced";
            d.ToggleIn(c10, fakeInterlaced, null);
            chkBlurayCompat.Text = "Enable Blu-ray compatibility";
            d.ToggleIn(c10, chkBlurayCompat, null);
            MeGUI.core.gui.SettingsPage misc = d.BeginPage();
            d.Section("Custom command line");
            d.Block(customCommandlineOptions, 600, 70, d.Card());

            // each tab of the control gets its page of cards
            AttachPage(mainTabPage, main);
            AttachPage(FrameTypeTabPage, frame);
            AttachPage(RCTabPage, rate);
            AttachPage(AnalysisTabPage, analysis);
            AttachPage(MiscTabPage, misc);
            Size = new Size(d.S(780), d.S(690));
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

        /// <summary>two controls which are shown one at a time share the same place</summary>
        private static Control Overlap(MeGUI.core.gui.ModernDialog d, Control a, Control b, int width)
        {
            Panel p = new Panel();
            p.Size = new Size(d.S(width), d.S(32));
            foreach (Control c in new Control[] { a, b })
            {
                c.Dock = DockStyle.None;
                c.Location = new Point(0, 0);
                c.Width = p.Width;
                p.Controls.Add(c);
            }
            return p;
        }

        private static MeGUI.core.gui.ToggleSwitch Tg(MeGUI.core.gui.ModernDialog d, MeGUI.core.gui.ToggleSwitch t)
        {
            t.AutoSize = false;
            t.Size = new Size(d.S(96), d.S(26));
            return t;
        }

        private void advancedSettings_CheckedChanged(object sender, EventArgs e)
        {

            if (advancedSettings.Checked)
            {
                if (!tabControl1.TabPages.Contains(FrameTypeTabPage))
                    tabControl1.TabPages.Add(FrameTypeTabPage);
                if (!tabControl1.TabPages.Contains(RCTabPage))
                    tabControl1.TabPages.Add(RCTabPage);
                if (!tabControl1.TabPages.Contains(AnalysisTabPage))
                    tabControl1.TabPages.Add(AnalysisTabPage);
                if (!tabControl1.TabPages.Contains(MiscTabPage))
                    tabControl1.TabPages.Add(MiscTabPage);
            }
            else
            {
                if (tabControl1.TabPages.Contains(FrameTypeTabPage))
                    tabControl1.TabPages.Remove(FrameTypeTabPage);
                if (tabControl1.TabPages.Contains(RCTabPage))
                    tabControl1.TabPages.Remove(RCTabPage);
                if (tabControl1.TabPages.Contains(AnalysisTabPage))
                    tabControl1.TabPages.Remove(AnalysisTabPage);
                if (tabControl1.TabPages.Contains(MiscTabPage))
                    tabControl1.TabPages.Remove(MiscTabPage);
                x265EncodingMode.Visible = true;
            }
            genericUpdate();
        }

        private void dSettings_Click(object sender, EventArgs e)
        {
            // Main Tab
            this.x265EncodingMode.SelectedIndex = 0;
            this.x265BitrateQuantizer.Value = 32;
            this.x265Tunes.SelectedIndex = 0;
            this.tbx265Presets.Value = 5;
            this.advancedSettings.Checked = false;
            this.hevcProfile.SelectedIndex = 0;


            // Frame-Type Tab
            this.x265DeblockActive.Checked = true;
            this.x265AlphaDeblock.Value = 0;
            this.x265BetaDeblock.Value = 0;
            this.cabac.Checked = true;
            this.x265KeyframeInterval.Text = "250";
            this.x265MinGOPSize.Text = "25";
            this.chkOpenGop.Checked = false;
            this.slicesnb.Value = 0;
            this.maxSliceSizeBytes.Value = 0;
            this.maxSliceSizeMB.Value = 0;
            this.x265WeightedBPrediction.Checked = true;
            this.x265NumberOfBFrames.Value = 3;
            this.x265BframeBias.Value = 0;
            this.x265NewAdaptiveBframes.SelectedIndex = 1;
            this.cbBPyramid.SelectedIndex = 2;
            this.x265NumberOfRefFrames.Value = 3;
            this.x265SCDSensitivity.Value = 40;
            this.x265WeightedPPrediction.SelectedIndex = 2;
            this.x265PullDown.SelectedIndex = 0;
            this.scenecut.Checked = true;
            this.cbGOPCalculation.SelectedIndex = 1;
            
            // Rate Control Tab
            this.x265MinimimQuantizer.Value = 0;
            this.x265MaximumQuantizer.Value = 69;
            this.x265MaxQuantDelta.Value = 4;
            this.x265IPFrameFactor.Value = 1.4M;
            this.x265PBFrameFactor.Value = 1.3M;
            this.deadzoneInter.Value = 0;
            this.deadzoneIntra.Value = 0;
            this.x265ChromaQPOffset.Value = 0;
            this.x265CreditsQuantizer.Value = 40;
            this.x265VBVBufferSize.Text = "0";
            this.x265VBVMaxRate.Text = "0";
            this.x265VBVInitialBuffer.Value = 0.9M;
            this.x265RateTol.Value = 1.0M;
            this.x265QuantizerCompression.Value = 0.6M;
            this.x265TempFrameComplexityBlur.Value = 20;
            this.x265TempQuantBlur.Value = 0.5M;
            this.lookahead.Value = 40;
            this.cbAQMode.SelectedIndex = 1;
            this.numAQStrength.Value = 1.0M;
            this.mbtree.Checked = true;
            this.cqmComboBox1.SelectedIndex = 0; 

            // Analysis Tab
            this.x265ChromaMe.Checked = true;
            this.x265MERange.Value = 16;
            this.x265METype.SelectedIndex = 1;                 
            this.x265SubpelRefinement.SelectedIndex = 7;
            this.x265BframePredictionMode.SelectedIndex = 1;
            this.trellis.SelectedIndex = 1;
            this.PsyRD.Value = 1.0M;
            this.PsyTrellis.Value = 0.0M;
            this.x265MixedReferences.Checked = false;
            this.noDCTDecimateOption.Checked = false;
            this.noFastPSkip.Checked = false;
            this.nopsy.Checked = false;
            this.NoiseReduction.Text = "0";
            this.macroblockOptions.SelectedIndex = 3;
            this.adaptiveDCT.Checked = true;
            this.x265I4x4mv.Checked = true;
            this.x265P4x4mv.Checked = false;
            this.x265I8x8mv.Checked = true;
            this.x265P8x8mv.Checked = true;
            this.x265B8x8mv.Checked = true;
            this.x265hrd.SelectedIndex = 0;
            this.x265aud.Checked = false;
            this.fakeInterlaced.Checked = false;
            this.chkBlurayCompat.Checked = false;

            // Misc Tab
            this.x265NbThreads.Value = 0;

            // to update presets label
            tbx265Presets_Scroll(null, null);
        }

        private void btPresetSettings_Click(object sender, EventArgs e)
        {
            doPresetsAdjustments();
            doTuningsAdjustments();
        }

        private void x265Tunes_SelectedIndexChanged(object sender, EventArgs e)
        {
            doTuningsAdjustments();
            genericUpdate();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            VisitLink();
        }

        private x265Settings.x265PsyTuningModes getPsyTuning()
        {
            EnumProxy o = x265Tunes.SelectedItem as EnumProxy;
            return (x265Settings.x265PsyTuningModes)o.RealValue;
        }
        private AVCLevels.Levels getAVCLevel()
        {
            EnumProxy o = hevcLevel.SelectedItem as EnumProxy;
            return (AVCLevels.Levels)o.RealValue; 
        }

        private void x265VBVBufferSize_ValueChanged(object sender, EventArgs e)
        {
            x265VBVBufferSize.ForeColor = MeGUI.core.gui.ThemeManager.InputText;

            updateEvent(sender, e);
        }

        private void x265VBVMaxRate_ValueChanged(object sender, EventArgs e)
        {
            x265VBVMaxRate.ForeColor = MeGUI.core.gui.ThemeManager.InputText;

            updateEvent(sender, e);
        }

        private void ch10BitsEncoder_CheckedChanged(object sender, EventArgs e)
        {
            updateEvent(sender, e);
        }

        private void ch10BitsEncoder_CheckedChanged_1(object sender, EventArgs e)
        {
            if (ch10BitsEncoder.Checked)
            {
                hevcProfile.SelectedIndex = 1; // Main 10 Profile
                hevcProfileCard.Enabled = false;
            }
            else
            {
                hevcProfileCard.Enabled = true;
            }
            updateEvent(sender, e);
        }

        private void hevcProfile_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.x265NumberOfBFrames.Value != x265Settings.GetDefaultNumberOfBFrames((x265Settings.x265PresetLevelModes)tbx265Presets.Value, getPsyTuning(), chkTuneZeroLatency.Checked, chkBlurayCompat.Checked))
                this.x265NumberOfBFrames.Value = x265Settings.GetDefaultNumberOfBFrames((x265Settings.x265PresetLevelModes)tbx265Presets.Value, getPsyTuning(), chkTuneZeroLatency.Checked, chkBlurayCompat.Checked);
            if (this.x265WeightedPPrediction.SelectedIndex != x265Settings.GetDefaultNumberOfWeightp((x265Settings.x265PresetLevelModes)tbx265Presets.Value, chkTuneFastDecode.Checked, chkBlurayCompat.Checked))
                this.x265WeightedPPrediction.SelectedIndex = x265Settings.GetDefaultNumberOfWeightp((x265Settings.x265PresetLevelModes)tbx265Presets.Value, chkTuneFastDecode.Checked, chkBlurayCompat.Checked);

            hevcLevel_SelectedIndexChanged(null, null);
        }

        private void hevcLevel_SelectedIndexChanged(object sender, EventArgs e)
        {/*
            AVCLevels.Levels avcLevel = getAVCLevel();
            if (avcLevel == AVCLevels.Levels.L_UNRESTRICTED || hevcProfile.SelectedIndex < 0)
            {
                x265VBVBufferSize.Value = 0;
                x265VBVMaxRate.Value = 0;
            }
            else
            {
                AVCLevels al = new AVCLevels();
                x265VBVBufferSize.Value = al.getMaxCBP(avcLevel, hevcProfile.SelectedIndex == 0);
                x265VBVMaxRate.Value = al.getMaxBR(avcLevel, hevcProfile.SelectedIndex == 0);
            }*/
            genericUpdate();
        }
    }
}
