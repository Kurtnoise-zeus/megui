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
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace MeGUI.packages.video.x264
{
    public partial class x264ConfigurationPanel : MeGUI.core.details.video.VideoConfigurationPanel, Editable<x264Settings>
    {
        #region variables
        public static bool levelEnforced; // flag to prevent recursion in EnforceLevels. There's probably a better way to do this.
        private XmlDocument ContextHelp = new XmlDocument();
        private List<x264Device> x264DeviceList = new List<x264Device>();
        private x264Device oTargetDevice;
        private bool updateDevice, updateDeviceBlocked;
        #endregion
        #region start / stop
        public x264ConfigurationPanel()
            : base()
        {
            InitializeComponent();
            cqmComboBox1.StandardItems = new string[] { "Flat (none)", "JVT" };
            cqmComboBox1.SelectedIndex = 0;
            AVCLevels al = new AVCLevels();
            this.avcLevel.Items.AddRange(EnumProxy.CreateArray(AVCLevels.SupportedLevels));
            this.avcLevel.SelectedItem = EnumProxy.Create(AVCLevels.Levels.L_UNRESTRICTED);
            x264DeviceList = x264Device.CreateDeviceList();
            foreach (x264Device oDevice in x264DeviceList)
                this.targetDevice.Items.Add(oDevice.Name);
            oTargetDevice = x264DeviceList[0];
            x264Tunes.Items.AddRange(EnumProxy.CreateArray(x264Settings.SupportedPsyTuningModes));
            x264Tunes.SelectedItem = EnumProxy.Create(x264Settings.x264PsyTuningModes.NONE);
            advancedSettings.Checked = MainForm.Instance.Settings.X264AdvancedSettings;
        }
        #endregion
        #region adjustments
        #region checkboxes
        private void doCheckBoxAdjustments()
        {
            x264AlphaDeblock.Enabled = x264DeblockActive.Checked;
            x264BetaDeblock.Enabled = x264DeblockActive.Checked;
            fakeInterlaced.Enabled = cbInterlaceMode.SelectedIndex == 0;
        }
        #endregion
        #region dropdowns
        private void doDeviceSpecificAdjustments()
        {
            // AVC profile
            if (oTargetDevice.Profile > -1)
            {
                if (avcProfile.SelectedIndex > oTargetDevice.Profile)
                    avcProfile.SelectedIndex = oTargetDevice.Profile;
                else if (updateDevice == true && avcProfile.SelectedIndex < oTargetDevice.Profile)
                    avcProfile.SelectedIndex = oTargetDevice.Profile;
            }
            else if (updateDevice == true)
                avcProfile.SelectedIndex = 2;

            // AVC level
            if (getAVCLevel().CompareTo(oTargetDevice.AVCLevel) > 0)
                avcLevel.SelectedItem = EnumProxy.Create(oTargetDevice.AVCLevel);
            else if (updateDevice == true && getAVCLevel().CompareTo(oTargetDevice.AVCLevel) < 0)
                avcLevel.SelectedItem = EnumProxy.Create(oTargetDevice.AVCLevel);

            // VBVBufsize
            if (oTargetDevice.VBVBufsize > -1)
            {
                if (x264VBVBufferSize.Value == 0 || x264VBVBufferSize.Value > oTargetDevice.VBVBufsize)
                    x264VBVBufferSize.Value = oTargetDevice.VBVBufsize;
                else if (updateDevice == true && x264VBVBufferSize.Value < oTargetDevice.VBVBufsize)
                    x264VBVBufferSize.Value = oTargetDevice.VBVBufsize;
            }
            else if (updateDevice == true)
            {
                if (getAVCLevel() == AVCLevels.Levels.L_UNRESTRICTED)
                    x264VBVBufferSize.Value = 0;
                else
                    x264VBVBufferSize.Value = new AVCLevels().getMaxCBP(getAVCLevel(), avcProfile.SelectedIndex == 2);
            } 

            // VBVMaxrate
            if (oTargetDevice.VBVMaxrate > -1)
            {
                if (x264VBVMaxRate.Value == 0 || x264VBVMaxRate.Value > oTargetDevice.VBVMaxrate)
                    x264VBVMaxRate.Value = oTargetDevice.VBVMaxrate;
                else if (updateDevice == true && x264VBVMaxRate.Value < oTargetDevice.VBVMaxrate)
                    x264VBVMaxRate.Value = oTargetDevice.VBVMaxrate;
            }
            else if (updateDevice == true)
            {
                if (getAVCLevel() == AVCLevels.Levels.L_UNRESTRICTED)
                    x264VBVMaxRate.Value = 0;
                else
                    x264VBVMaxRate.Value = new AVCLevels().getMaxBR(getAVCLevel(), avcProfile.SelectedIndex == 2);
            }

            // --b-pyramid
            if (oTargetDevice.BPyramid > -1)
            {
                if (oTargetDevice.BPyramid != cbBPyramid.SelectedIndex)
                    cbBPyramid.SelectedIndex = oTargetDevice.BPyramid;
            }
            else if (updateDevice == true)
                cbBPyramid.SelectedIndex = 1;

            // Blu-ray
            if (oTargetDevice.BluRay)
            {
                chkBlurayCompat.Checked = true;
                chkOpenGop.Checked = true;
                x264aud.Checked = true;
                if (x264hrd.SelectedIndex == 0)
                    x264hrd.SelectedIndex = 1;
                if (x264WeightedPPrediction.SelectedIndex > 1)
                    x264WeightedPPrediction.SelectedIndex = 1;
                if (cbBPyramid.SelectedIndex > 1)
                    cbBPyramid.SelectedIndex = 1;
                slicesnb.Value = 4;
            }
            else if (updateDevice == true)
            {
                x264hrd.SelectedIndex = 0;
                chkBlurayCompat.Checked = false;
                chkOpenGop.Checked = false;
                x264aud.Checked = false;
                slicesnb.Value = 0;
                if (cbBPyramid.SelectedIndex == 1)
                    cbBPyramid.SelectedIndex = 2;
                if (this.x264WeightedPPrediction.SelectedIndex != x264Settings.GetDefaultNumberOfWeightp((x264Settings.x264PresetLevelModes)tbx264Presets.Value, chkTuneFastDecode.Checked, avcProfile.SelectedIndex, chkBlurayCompat.Checked))
                    this.x264WeightedPPrediction.SelectedIndex = x264Settings.GetDefaultNumberOfWeightp((x264Settings.x264PresetLevelModes)tbx264Presets.Value, chkTuneFastDecode.Checked, avcProfile.SelectedIndex, chkBlurayCompat.Checked);
            }
            else if (chkBlurayCompat.Checked)
            {
                x264aud.Checked = true;
                if (x264hrd.SelectedIndex == 0)
                    x264hrd.SelectedIndex = 1;
                if (x264WeightedPPrediction.SelectedIndex > 1)
                    x264WeightedPPrediction.SelectedIndex = 1;
                if (cbBPyramid.SelectedIndex > 1)
                    cbBPyramid.SelectedIndex = 1;
                if (x264NumberOfBFrames.Value > 3)
                    x264NumberOfBFrames.Value = 3;
                if (x264NumberOfRefFrames.Value > 6)
                    x264NumberOfRefFrames.Value = 6;
            }

            // BFrames
            if (oTargetDevice.BFrames > -1)
            {
                if (x264NumberOfBFrames.Value > oTargetDevice.BFrames)
                    x264NumberOfBFrames.Value = oTargetDevice.BFrames;
            }
            if (updateDevice == true)
            {
                int iDefaultBFrames = x264Settings.GetDefaultNumberOfBFrames((x264Settings.x264PresetLevelModes)tbx264Presets.Value, getPsyTuning(), chkTuneZeroLatency.Checked, avcProfile.SelectedIndex, oTargetDevice, chkBlurayCompat.Checked);
                if (x264NumberOfBFrames.Value != iDefaultBFrames)
                    x264NumberOfBFrames.Value = iDefaultBFrames;
            }

            // Reference Frames
            if (oTargetDevice.ReferenceFrames > -1)
            {
                if (x264NumberOfRefFrames.Value > oTargetDevice.ReferenceFrames)
                    x264NumberOfRefFrames.Value = oTargetDevice.ReferenceFrames;
            }
            if (updateDevice == true)
            {
                int iDefaultRFrames = x264Settings.GetDefaultNumberOfRefFrames((x264Settings.x264PresetLevelModes)tbx264Presets.Value, getPsyTuning(), oTargetDevice, getAVCLevel(), chkBlurayCompat.Checked);
                if (x264NumberOfRefFrames.Value != iDefaultRFrames)
                    x264NumberOfRefFrames.Value = iDefaultRFrames;
            }

        }

        private void doMacroBlockAdjustments()
        {
            bool enableOptions = (macroblockOptions.SelectedIndex == 2); // custom
            x264I4x4mv.Enabled = enableOptions;
            x264P4x4mv.Enabled = enableOptions;
            x264P8x8mv.Enabled = enableOptions;
            x264B8x8mv.Enabled = enableOptions;
            x264I8x8mv.Enabled = enableOptions;
            adaptiveDCT.Enabled = enableOptions;

            if (macroblockOptions.SelectedIndex == 1) // none
            {
                x264I4x4mv.Checked = false;
                x264P4x4mv.Checked = false;
                x264P8x8mv.Checked = false;
                x264B8x8mv.Checked = false;
                x264I8x8mv.Checked = false;
                adaptiveDCT.Checked = false;
                return;
            }

            if (macroblockOptions.SelectedIndex == 0) // all
            {
                x264P8x8mv.Checked = true;
                x264I4x4mv.Checked = true;
                x264P4x4mv.Checked = true;
                x264B8x8mv.Checked = true;

                if (avcProfile.SelectedIndex > 1)
                {
                    adaptiveDCT.Checked = true;
                    x264I8x8mv.Checked = true;
                }
                else
                {
                    adaptiveDCT.Checked = false;
                    x264I8x8mv.Checked = false;
                }
            }
            else if (macroblockOptions.SelectedIndex == 2) // custom
            {
                if (avcProfile.SelectedIndex > 1)
                {
                    if (adaptiveDCT.Checked)
                    {
                        x264I8x8mv.Checked = true;
                    }
                    else
                    {
                        x264I8x8mv.Enabled = false;
                        x264I8x8mv.Checked = false;
                    }
                }
                else
                {
                    adaptiveDCT.Enabled = false;
                    adaptiveDCT.Checked = false;
                    x264I8x8mv.Enabled = false;
                    x264I8x8mv.Checked = false;
                }

                if (!this.x264P8x8mv.Checked) // p4x4 requires p8x8 
                {
                    this.x264P4x4mv.Checked = false;
                    this.x264P4x4mv.Enabled = false;
                }
            }
            else // Default
            {
                x264P8x8mv.Checked = true;
                x264I4x4mv.Checked = true;
                x264P4x4mv.Checked = false;
                x264B8x8mv.Checked = true;

                if (avcProfile.SelectedIndex > 1)
                {
                    adaptiveDCT.Checked = true;
                    x264I8x8mv.Checked = true;
                }
                else
                {
                    adaptiveDCT.Checked = false;
                    x264I8x8mv.Checked = false;
                }
            }
            genericUpdate();
        }

        private void doTrellisAdjustments()
        {
            deadzoneInter.Enabled = (trellis.SelectedIndex == 0);
            deadzoneIntra.Enabled = (trellis.SelectedIndex == 0);
            lbx264DeadZones.Enabled = (trellis.SelectedIndex == 0);
            if (trellis.SelectedIndex != 0)
            {
                deadzoneIntra.Value = 11;
                deadzoneInter.Value = 21;
            }
            if (getPsyTuning() == x264Settings.x264PsyTuningModes.GRAIN && trellis.SelectedIndex != 0)
            {
                deadzoneInter.Value = 6;
                deadzoneIntra.Value = 6;
            }
        }

        private void doSubmeAdjustments()
        {
            if (x264SubpelRefinement.SelectedIndex == 10)
            {
                if (trellis.SelectedIndex != 2)
                    trellis.SelectedIndex = 2;
                if (cbAQMode.SelectedIndex == 0)
                    cbAQMode.SelectedIndex = 1;
            }
        }

        private void doTuningsAdjustments()
        {
            if (this.x264NumberOfRefFrames.Value != x264Settings.GetDefaultNumberOfRefFrames((x264Settings.x264PresetLevelModes)tbx264Presets.Value, getPsyTuning(), oTargetDevice, getAVCLevel(), chkBlurayCompat.Checked))
                this.x264NumberOfRefFrames.Value = x264Settings.GetDefaultNumberOfRefFrames((x264Settings.x264PresetLevelModes)tbx264Presets.Value, getPsyTuning(), oTargetDevice, getAVCLevel(), chkBlurayCompat.Checked);
            if (this.x264NumberOfBFrames.Value != x264Settings.GetDefaultNumberOfBFrames((x264Settings.x264PresetLevelModes)tbx264Presets.Value, getPsyTuning(), chkTuneZeroLatency.Checked, avcProfile.SelectedIndex, oTargetDevice, chkBlurayCompat.Checked))
                this.x264NumberOfBFrames.Value = x264Settings.GetDefaultNumberOfBFrames((x264Settings.x264PresetLevelModes)tbx264Presets.Value, getPsyTuning(), chkTuneZeroLatency.Checked, avcProfile.SelectedIndex, oTargetDevice, chkBlurayCompat.Checked);
            if (this.x264WeightedPPrediction.SelectedIndex != x264Settings.GetDefaultNumberOfWeightp((x264Settings.x264PresetLevelModes)tbx264Presets.Value, chkTuneFastDecode.Checked, avcProfile.SelectedIndex, chkBlurayCompat.Checked))
                this.x264WeightedPPrediction.SelectedIndex = x264Settings.GetDefaultNumberOfWeightp((x264Settings.x264PresetLevelModes)tbx264Presets.Value, chkTuneFastDecode.Checked, avcProfile.SelectedIndex, chkBlurayCompat.Checked);
            if (this.cbAQMode.SelectedIndex != x264Settings.GetDefaultAQMode((x264Settings.x264PresetLevelModes)tbx264Presets.Value, getPsyTuning()))
                this.cbAQMode.SelectedIndex = x264Settings.GetDefaultAQMode((x264Settings.x264PresetLevelModes)tbx264Presets.Value, getPsyTuning());
            if (this.lookahead.Value != x264Settings.GetDefaultRCLookahead((x264Settings.x264PresetLevelModes)tbx264Presets.Value, chkTuneZeroLatency.Checked))
                this.lookahead.Value = x264Settings.GetDefaultRCLookahead((x264Settings.x264PresetLevelModes)tbx264Presets.Value, chkTuneZeroLatency.Checked);
            
            switch (getPsyTuning())
            {
                case x264Settings.x264PsyTuningModes.NONE:
                    {
                        if (this.x264AlphaDeblock.Value != 0)
                            this.x264AlphaDeblock.Value = 0;
                        if (this.x264BetaDeblock.Value != 0)
                            this.x264BetaDeblock.Value = 0;
                        if (this.PsyTrellis.Value != 0.0M)
                            this.PsyTrellis.Value = 0.0M;
                        if (this.PsyRD.Value != 1.0M)
                            this.PsyRD.Value = 1.0M;
                        if (this.numAQStrength.Value != 1.0M)
                            this.numAQStrength.Value = 1.0M;
                        if (this.x264IPFrameFactor.Value != 1.4M)
                            this.x264IPFrameFactor.Value = 1.4M;
                        if (this.x264PBFrameFactor.Value != 1.3M)
                            this.x264PBFrameFactor.Value = 1.3M;
                        if (this.deadzoneInter.Value != 21)
                            this.deadzoneInter.Value = 21;
                        if (this.deadzoneIntra.Value != 11)
                            this.deadzoneIntra.Value = 11;
                        if (this.x264QuantizerCompression.Value != 0.6M)
                            this.x264QuantizerCompression.Value = 0.6M;
                        if (noDCTDecimateOption.Checked)
                            noDCTDecimateOption.Checked = false;
                        if (nopsy.Checked)
                            nopsy.Checked = false;
                    }
                    break;
                case x264Settings.x264PsyTuningModes.FILM:
                    {
                        if (this.x264AlphaDeblock.Value != -1)
                            this.x264AlphaDeblock.Value = -1;
                        if (this.x264BetaDeblock.Value != -1)
                            this.x264BetaDeblock.Value = -1;
                        if (this.PsyTrellis.Value != 0.15M)
                            this.PsyTrellis.Value = 0.15M;
                        if (this.PsyRD.Value != 1.0M)
                            this.PsyRD.Value = 1.0M;
                        if (this.numAQStrength.Value != 1.0M)
                            this.numAQStrength.Value = 1.0M;
                        if (this.x264IPFrameFactor.Value != 1.4M)
                            this.x264IPFrameFactor.Value = 1.4M;
                        if (this.x264PBFrameFactor.Value != 1.3M)
                            this.x264PBFrameFactor.Value = 1.3M;
                        if (this.deadzoneInter.Value != 21)
                            this.deadzoneInter.Value = 21;
                        if (this.deadzoneIntra.Value != 11)
                            this.deadzoneIntra.Value = 11;
                        if (this.x264QuantizerCompression.Value != 0.6M)
                            this.x264QuantizerCompression.Value = 0.6M;
                        if (noDCTDecimateOption.Checked)
                            noDCTDecimateOption.Checked = false;
                        if (nopsy.Checked)
                            nopsy.Checked = false;
                    }
                    break;
                case x264Settings.x264PsyTuningModes.ANIMATION:
                    {
                        if (this.x264AlphaDeblock.Value != 1)
                            this.x264AlphaDeblock.Value = 1;
                        if (this.x264BetaDeblock.Value != 1)
                            this.x264BetaDeblock.Value = 1;
                        if (this.PsyTrellis.Value != 0.0M)
                            this.PsyTrellis.Value = 0.0M;
                        if (this.PsyRD.Value != 0.4M)
                            this.PsyRD.Value = 0.4M;
                        if (this.numAQStrength.Value != 0.6M)
                            this.numAQStrength.Value = 0.6M;
                        if (this.x264IPFrameFactor.Value != 1.4M)
                            this.x264IPFrameFactor.Value = 1.4M;
                        if (this.x264PBFrameFactor.Value != 1.3M)
                            this.x264PBFrameFactor.Value = 1.3M;
                        if (this.deadzoneInter.Value != 21)
                            this.deadzoneInter.Value = 21;
                        if (this.deadzoneIntra.Value != 11)
                            this.deadzoneIntra.Value = 11;
                        if (this.x264QuantizerCompression.Value != 0.6M)
                            this.x264QuantizerCompression.Value = 0.6M;
                        if (noDCTDecimateOption.Checked)
                            noDCTDecimateOption.Checked = false;
                        if (nopsy.Checked)
                            nopsy.Checked = false;
                    }
                    break;
                case x264Settings.x264PsyTuningModes.GRAIN:
                    {
                        if (this.x264AlphaDeblock.Value != -2)
                            this.x264AlphaDeblock.Value = -2;
                        if (this.x264BetaDeblock.Value != -2)
                            this.x264BetaDeblock.Value = -2;
                        if (this.PsyTrellis.Value != 0.25M)
                            this.PsyTrellis.Value = 0.25M;
                        if (this.PsyRD.Value != 1.0M)
                            this.PsyRD.Value = 1.0M;
                        if (this.numAQStrength.Value != 0.5M)
                            this.numAQStrength.Value = 0.5M;
                        if (this.x264IPFrameFactor.Value != 1.1M)
                            this.x264IPFrameFactor.Value = 1.1M;
                        if (this.x264PBFrameFactor.Value != 1.1M)
                            this.x264PBFrameFactor.Value = 1.1M;
                        if (this.deadzoneInter.Value != 6)
                            this.deadzoneInter.Value = 6;
                        if (this.deadzoneIntra.Value != 6)
                            this.deadzoneIntra.Value = 6;
                        if (this.x264QuantizerCompression.Value != 0.8M)
                            this.x264QuantizerCompression.Value = 0.8M;
                        if (!noDCTDecimateOption.Checked)
                            noDCTDecimateOption.Checked = true;
                        if (nopsy.Checked)
                            nopsy.Checked = false;
                    }
                    break;
                case x264Settings.x264PsyTuningModes.PSNR:
                    {
                        if (this.x264AlphaDeblock.Value != 0)
                            this.x264AlphaDeblock.Value = 0;
                        if (this.x264BetaDeblock.Value != 0)
                            this.x264BetaDeblock.Value = 0;
                        if (this.PsyTrellis.Value != 0.0M)
                            this.PsyTrellis.Value = 0.0M;
                        if (this.PsyRD.Value != 1.0M)
                            this.PsyRD.Value = 1.0M;
                        if (this.numAQStrength.Value != 1.0M)
                            this.numAQStrength.Value = 1.0M;
                        if (this.x264IPFrameFactor.Value != 1.4M)
                            this.x264IPFrameFactor.Value = 1.4M;
                        if (this.x264PBFrameFactor.Value != 1.3M)
                            this.x264PBFrameFactor.Value = 1.3M;
                        if (this.deadzoneInter.Value != 21)
                            this.deadzoneInter.Value = 21;
                        if (this.deadzoneIntra.Value != 11)
                            this.deadzoneIntra.Value = 11;
                        if (this.x264QuantizerCompression.Value != 0.6M)
                            this.x264QuantizerCompression.Value = 0.6M;
                        if (noDCTDecimateOption.Checked)
                            noDCTDecimateOption.Checked = false;
                        if (!nopsy.Checked)
                            nopsy.Checked = true;
                    }
                    break;
                case x264Settings.x264PsyTuningModes.SSIM:
                    {
                        if (this.x264AlphaDeblock.Value != 0)
                            this.x264AlphaDeblock.Value = 0;
                        if (this.x264BetaDeblock.Value != 0)
                            this.x264BetaDeblock.Value = 0;
                        if (this.PsyTrellis.Value != 0.0M)
                            this.PsyTrellis.Value = 0.0M;
                        if (this.PsyRD.Value != 1.0M)
                            this.PsyRD.Value = 1.0M;
                        if (this.numAQStrength.Value != 1.0M)
                            this.numAQStrength.Value = 1.0M;
                        if (this.x264IPFrameFactor.Value != 1.4M)
                            this.x264IPFrameFactor.Value = 1.4M;
                        if (this.x264PBFrameFactor.Value != 1.3M)
                            this.x264PBFrameFactor.Value = 1.3M;
                        if (this.deadzoneInter.Value != 21)
                            this.deadzoneInter.Value = 21;
                        if (this.deadzoneIntra.Value != 11)
                            this.deadzoneIntra.Value = 11;
                        if (this.x264QuantizerCompression.Value != 0.6M)
                            this.x264QuantizerCompression.Value = 0.6M;
                        if (noDCTDecimateOption.Checked)
                            noDCTDecimateOption.Checked = false;
                        if (!nopsy.Checked)
                            nopsy.Checked = true;
                    }
                    break;
                case x264Settings.x264PsyTuningModes.STILLIMAGE:
                    {
                        if (this.x264AlphaDeblock.Value != 3)
                            this.x264AlphaDeblock.Value = 3;
                        if (this.x264BetaDeblock.Value != -3)
                            this.x264BetaDeblock.Value = -3;
                        if (this.PsyTrellis.Value != 0.7M)
                            this.PsyTrellis.Value = 0.7M;
                        if (this.PsyRD.Value != 2.0M)
                            this.PsyRD.Value = 2.0M;
                        if (this.numAQStrength.Value != 1.2M)
                            this.numAQStrength.Value = 1.2M;
                        if (this.x264IPFrameFactor.Value != 1.4M)
                            this.x264IPFrameFactor.Value = 1.4M;
                        if (this.x264PBFrameFactor.Value != 1.3M)
                            this.x264PBFrameFactor.Value = 1.3M;
                        if (this.deadzoneInter.Value != 21)
                            this.deadzoneInter.Value = 21;
                        if (this.deadzoneIntra.Value != 11)
                            this.deadzoneIntra.Value = 11;
                        if (this.x264QuantizerCompression.Value != 0.6M)
                            this.x264QuantizerCompression.Value = 0.6M;
                        if (noDCTDecimateOption.Checked)
                            noDCTDecimateOption.Checked = false;
                        if (nopsy.Checked)
                            nopsy.Checked = false;
                    }
                    break;
            }

            x264DeblockActive.Checked = !chkTuneFastDecode.Checked;
            cabac.Checked = !chkTuneFastDecode.Checked;
            x264WeightedBPrediction.Checked = !chkTuneFastDecode.Checked;
            mbtree.Checked = !chkTuneZeroLatency.Checked;
        }

        private void doPresetsAdjustments()
        {
            if (this.x264NumberOfRefFrames.Value != x264Settings.GetDefaultNumberOfRefFrames((x264Settings.x264PresetLevelModes)tbx264Presets.Value, getPsyTuning(), oTargetDevice, getAVCLevel(), chkBlurayCompat.Checked))
                this.x264NumberOfRefFrames.Value = x264Settings.GetDefaultNumberOfRefFrames((x264Settings.x264PresetLevelModes)tbx264Presets.Value, getPsyTuning(), oTargetDevice, getAVCLevel(), chkBlurayCompat.Checked);
            if (this.x264NumberOfBFrames.Value != x264Settings.GetDefaultNumberOfBFrames((x264Settings.x264PresetLevelModes)tbx264Presets.Value, getPsyTuning(), chkTuneZeroLatency.Checked, avcProfile.SelectedIndex, oTargetDevice, chkBlurayCompat.Checked))
                this.x264NumberOfBFrames.Value = x264Settings.GetDefaultNumberOfBFrames((x264Settings.x264PresetLevelModes)tbx264Presets.Value, getPsyTuning(), chkTuneZeroLatency.Checked, avcProfile.SelectedIndex, oTargetDevice, chkBlurayCompat.Checked);
            if (this.x264WeightedPPrediction.SelectedIndex != x264Settings.GetDefaultNumberOfWeightp((x264Settings.x264PresetLevelModes)tbx264Presets.Value, chkTuneFastDecode.Checked, avcProfile.SelectedIndex, chkBlurayCompat.Checked))
                this.x264WeightedPPrediction.SelectedIndex = x264Settings.GetDefaultNumberOfWeightp((x264Settings.x264PresetLevelModes)tbx264Presets.Value, chkTuneFastDecode.Checked, avcProfile.SelectedIndex, chkBlurayCompat.Checked);
            if (this.cbAQMode.SelectedIndex != x264Settings.GetDefaultAQMode((x264Settings.x264PresetLevelModes)tbx264Presets.Value, getPsyTuning()))
                this.cbAQMode.SelectedIndex = x264Settings.GetDefaultAQMode((x264Settings.x264PresetLevelModes)tbx264Presets.Value, getPsyTuning());
            if (this.lookahead.Value != x264Settings.GetDefaultRCLookahead((x264Settings.x264PresetLevelModes)tbx264Presets.Value, chkTuneZeroLatency.Checked))
                this.lookahead.Value = x264Settings.GetDefaultRCLookahead((x264Settings.x264PresetLevelModes)tbx264Presets.Value, chkTuneZeroLatency.Checked);

            switch (tbx264Presets.Value)
            {
                case 0: // Ultra Fast
                    {
                        if (this.x264MERange.Value != 16) 
                            this.x264MERange.Value = 16;
                        if (macroblockOptions.SelectedIndex != 1)
                            macroblockOptions.SelectedIndex = 1;
                        if (x264SubpelRefinement.SelectedIndex != 0)
                            x264SubpelRefinement.SelectedIndex = 0;
                        if (x264METype.SelectedIndex != 0)
                            x264METype.SelectedIndex = 0;
                        if (trellis.SelectedIndex != 0)
                            trellis.SelectedIndex = 0;
                        if (x264BframePredictionMode.SelectedIndex != 1)
                            x264BframePredictionMode.SelectedIndex = 1;
                        if (scenecut.Checked)
                            scenecut.Checked = false;
                        if (x264DeblockActive.Checked)
                            x264DeblockActive.Checked = false;
                        if (cabac.Checked)
                            cabac.Checked = false;
                        if (!chkTuneFastDecode.Checked)
                            cabac.Enabled = false;
                        if (adaptiveDCT.Checked)
                            adaptiveDCT.Checked = false;
                        if (!x264MixedReferences.Checked)
                            x264MixedReferences.Checked = true;
                        if (mbtree.Checked)
                            mbtree.Checked = false;
                        mbtree.Enabled = false;
                        if (noFastPSkip.Checked)
                            noFastPSkip.Checked = false;
                        if (x264NewAdaptiveBframes.SelectedIndex != 0)
                            x264NewAdaptiveBframes.SelectedIndex = 0;
                        if (this.x264WeightedBPrediction.Checked)
                            this.x264WeightedBPrediction.Checked = false;
                    }
                    break;
                case 1: // Super Fast
                    {
                        if (macroblockOptions.SelectedIndex != 2)
                            macroblockOptions.SelectedIndex = 2;
                        if (!x264I4x4mv.Checked)
                            x264I4x4mv.Checked = true;
                        if (!x264I8x8mv.Checked)
                            x264I8x8mv.Checked = true;
                        if (x264B8x8mv.Checked)
                            x264B8x8mv.Checked = false;
                        if (x264P4x4mv.Checked)
                            x264P4x4mv.Checked = false;
                        if (x264P8x8mv.Checked)
                            x264P8x8mv.Checked = false;
                        if (x264METype.SelectedIndex != 0)
                            x264METype.SelectedIndex = 0;
                        if (x264SubpelRefinement.SelectedIndex != 1)
                            x264SubpelRefinement.SelectedIndex = 1;
                        if (!x264MixedReferences.Checked)
                            x264MixedReferences.Checked = true;
                        if (trellis.SelectedIndex != 0)
                            trellis.SelectedIndex = 0;
                        if (mbtree.Checked)
                            mbtree.Checked = false;
                        mbtree.Enabled = false;
                        if (x264BframePredictionMode.SelectedIndex != 1)
                            x264BframePredictionMode.SelectedIndex = 1;
                        if (!cabac.Checked)
                            cabac.Checked = true;
                        if (!chkTuneFastDecode.Checked)
                            cabac.Enabled = true;
                        if (!x264DeblockActive.Checked)
                            x264DeblockActive.Checked = true;
                        if (!scenecut.Checked)
                            scenecut.Checked = true;
                        if (this.x264MERange.Value != 16)
                            this.x264MERange.Value = 16;
                        if (noFastPSkip.Checked)
                            noFastPSkip.Checked = false;
                        if (x264NewAdaptiveBframes.SelectedIndex != 1)
                            x264NewAdaptiveBframes.SelectedIndex = 1;
                        if (!this.x264WeightedBPrediction.Checked)
                            this.x264WeightedBPrediction.Checked = true;
                        if (!adaptiveDCT.Checked)
                            adaptiveDCT.Checked = true;
                    }
                    break;
                case 2: // Very Fast
                    {
                        if (macroblockOptions.SelectedIndex != 3)
                            macroblockOptions.SelectedIndex = 3;
                        if (x264METype.SelectedIndex != 1)
                            x264METype.SelectedIndex = 1;
                        if (x264SubpelRefinement.SelectedIndex != 2)
                            x264SubpelRefinement.SelectedIndex = 2;
                        if (!x264MixedReferences.Checked)
                            x264MixedReferences.Checked = true;
                        if (trellis.SelectedIndex != 0)
                            trellis.SelectedIndex = 0;
                        if (!mbtree.Checked)
                            mbtree.Checked = true;
                        mbtree.Enabled = true;
                        if (x264BframePredictionMode.SelectedIndex != 1)
                            x264BframePredictionMode.SelectedIndex = 1;
                        if (!cabac.Checked)
                            cabac.Checked = true;
                        if (!chkTuneFastDecode.Checked)
                            cabac.Enabled = true;
                        if (!x264DeblockActive.Checked)
                             x264DeblockActive.Checked = true;
                        if (!scenecut.Checked)
                            scenecut.Checked = true;
                        if (this.x264MERange.Value != 16)
                            this.x264MERange.Value = 16;
                        if (noFastPSkip.Checked)
                            noFastPSkip.Checked = false;
                        if (x264NewAdaptiveBframes.SelectedIndex != 1)
                            x264NewAdaptiveBframes.SelectedIndex = 1;
                        if (!this.x264WeightedBPrediction.Checked)
                            this.x264WeightedBPrediction.Checked = true;
                        if (!adaptiveDCT.Checked)
                            adaptiveDCT.Checked = true;
                    }
                    break;
                case 3: // Faster
                    {
                        if (x264SubpelRefinement.SelectedIndex != 4)
                            x264SubpelRefinement.SelectedIndex = 4;
                        if (!mbtree.Checked)
                            mbtree.Checked = true;
                        mbtree.Enabled = true;
                        if (trellis.SelectedIndex != 1)
                            trellis.SelectedIndex = 1;
                        if (!x264MixedReferences.Checked)
                             x264MixedReferences.Checked = true;
                        if (x264METype.SelectedIndex != 1)
                            x264METype.SelectedIndex = 1;
                        if (macroblockOptions.SelectedIndex != 3)
                            macroblockOptions.SelectedIndex = 3;
                        if (x264BframePredictionMode.SelectedIndex != 1)
                            x264BframePredictionMode.SelectedIndex = 1;
                        if (!cabac.Checked)
                            cabac.Checked = true;
                        if (!chkTuneFastDecode.Checked)
                            cabac.Enabled = true;
                        if (!x264DeblockActive.Checked)
                             x264DeblockActive.Checked = true;
                        if (!scenecut.Checked)
                             scenecut.Checked = true;
                        if (this.x264MERange.Value != 16)
                            this.x264MERange.Value = 16;
                        if (noFastPSkip.Checked)
                            noFastPSkip.Checked = false;
                        if (x264NewAdaptiveBframes.SelectedIndex != 1)
                            x264NewAdaptiveBframes.SelectedIndex = 1;
                        if (!this.x264WeightedBPrediction.Checked)
                            this.x264WeightedBPrediction.Checked = true;
                        if (adaptiveDCT.Checked)
                            adaptiveDCT.Checked = false;
                    }
                    break;
                case 4: // Fast
                    {
                        if (x264SubpelRefinement.SelectedIndex != 6)
                            x264SubpelRefinement.SelectedIndex = 6;
                        if (trellis.SelectedIndex != 1)
                            trellis.SelectedIndex = 1;
                        if (x264MixedReferences.Checked)
                             x264MixedReferences.Checked = false;
                        if (!mbtree.Checked)
                             mbtree.Checked = true;
                        mbtree.Enabled = true;
                        if (x264METype.SelectedIndex != 1)
                            x264METype.SelectedIndex = 1;
                        if (macroblockOptions.SelectedIndex != 3)
                            macroblockOptions.SelectedIndex = 3;
                        if (x264BframePredictionMode.SelectedIndex != 1)
                            x264BframePredictionMode.SelectedIndex = 1;
                        if (!cabac.Checked)
                            cabac.Checked = true;
                        if (!chkTuneFastDecode.Checked)
                            cabac.Enabled = true;
                        if (!x264DeblockActive.Checked)
                             x264DeblockActive.Checked = true;
                        if (!scenecut.Checked)
                             scenecut.Checked = true;
                        if (this.x264MERange.Value != 16)
                            this.x264MERange.Value = 16;
                        if (noFastPSkip.Checked)
                            noFastPSkip.Checked = false;
                        if (x264NewAdaptiveBframes.SelectedIndex != 1)
                            x264NewAdaptiveBframes.SelectedIndex = 1;
                        if (!this.x264WeightedBPrediction.Checked)
                            this.x264WeightedBPrediction.Checked = true;
                        if (adaptiveDCT.Checked)
                            adaptiveDCT.Checked = false;
                    }
                    break;
                case 5: // Medium
                    {
                        if (x264METype.SelectedIndex != 1)
                            x264METype.SelectedIndex = 1;
                        if (x264SubpelRefinement.SelectedIndex != 7)
                            x264SubpelRefinement.SelectedIndex = 7;
                        if (x264NewAdaptiveBframes.SelectedIndex != 1)
                            x264NewAdaptiveBframes.SelectedIndex = 1;
                        if (x264BframePredictionMode.SelectedIndex != 1)
                            x264BframePredictionMode.SelectedIndex = 1;
                        if (trellis.SelectedIndex != 1)
                            trellis.SelectedIndex = 1;
                        if (x264MixedReferences.Checked)
                            x264MixedReferences.Checked = false;
                        if (!mbtree.Checked)
                            mbtree.Checked = true;
                        mbtree.Enabled = true;
                        if (macroblockOptions.SelectedIndex != 3)
                            macroblockOptions.SelectedIndex = 3;
                        if (!cabac.Checked)
                            cabac.Checked = true;
                        if (!chkTuneFastDecode.Checked)
                            cabac.Enabled = true;
                        if (!x264DeblockActive.Checked)
                            x264DeblockActive.Checked = true;
                        if (!scenecut.Checked)
                            scenecut.Checked = true;
                        if (this.x264MERange.Value != 16)
                            this.x264MERange.Value = 16;
                        if (noFastPSkip.Checked)
                            noFastPSkip.Checked = false;
                        if (!this.x264WeightedBPrediction.Checked)
                            this.x264WeightedBPrediction.Checked = true;
                        if (adaptiveDCT.Checked)
                            adaptiveDCT.Checked = false;
                    }
                    break;
                case 6: // Slow
                    {
                        if (x264METype.SelectedIndex != 1)
                            x264METype.SelectedIndex = 1;
                        if (x264SubpelRefinement.SelectedIndex != 8)
                            x264SubpelRefinement.SelectedIndex = 8;
                        if (x264NewAdaptiveBframes.SelectedIndex != 1)
                            x264NewAdaptiveBframes.SelectedIndex = 1;
                        if (x264BframePredictionMode.SelectedIndex != 3)
                            x264BframePredictionMode.SelectedIndex = 3;
                        if (trellis.SelectedIndex != 2)
                            trellis.SelectedIndex = 2;
                        if (x264MixedReferences.Checked)
                             x264MixedReferences.Checked = false;
                        if (!mbtree.Checked)
                             mbtree.Checked = true;
                        mbtree.Enabled = true;
                        if (macroblockOptions.SelectedIndex != 3)
                            macroblockOptions.SelectedIndex = 3;
                        if (!cabac.Checked)
                            cabac.Checked = true;
                        if (!chkTuneFastDecode.Checked)
                            cabac.Enabled = true;
                        if (!x264DeblockActive.Checked)
                             x264DeblockActive.Checked = true;
                        if (!scenecut.Checked)
                             scenecut.Checked = true;
                        if (this.x264MERange.Value != 16)
                            this.x264MERange.Value = 16;
                        if (noFastPSkip.Checked)
                            noFastPSkip.Checked = false;
                        if (!this.x264WeightedBPrediction.Checked)
                            this.x264WeightedBPrediction.Checked = true;
                        if (adaptiveDCT.Checked)
                            adaptiveDCT.Checked = false;
                    }
                    break;
                case 7: // Slower
                    {
                        if (x264METype.SelectedIndex != 2)
                            x264METype.SelectedIndex = 2;
                        if (x264SubpelRefinement.SelectedIndex != 9)
                            x264SubpelRefinement.SelectedIndex = 9;
                        if (x264NewAdaptiveBframes.SelectedIndex != 2)
                            x264NewAdaptiveBframes.SelectedIndex = 2;
                        if (trellis.SelectedIndex != 2)
                            trellis.SelectedIndex = 2;
                        if (x264BframePredictionMode.SelectedIndex != 3)
                            x264BframePredictionMode.SelectedIndex = 3;
                        if (macroblockOptions.SelectedIndex != 0)
                            macroblockOptions.SelectedIndex = 0;
                        if (x264MixedReferences.Checked)
                             x264MixedReferences.Checked = false;
                        if (!mbtree.Checked)
                            mbtree.Checked = true;
                        mbtree.Enabled = true;
                        if (!cabac.Checked)
                            cabac.Checked = true;
                        if (!chkTuneFastDecode.Checked)
                            cabac.Enabled = true;
                        if (!x264DeblockActive.Checked)
                             x264DeblockActive.Checked = true;
                        if (!scenecut.Checked)
                             scenecut.Checked = true;
                        if (this.x264MERange.Value != 16)
                            this.x264MERange.Value = 16;
                        if (noFastPSkip.Checked)
                            noFastPSkip.Checked = false;
                        if (!this.x264WeightedBPrediction.Checked)
                            this.x264WeightedBPrediction.Checked = true;
                        if (adaptiveDCT.Checked)
                            adaptiveDCT.Checked = false;
                    }
                    break;
                case 8: // Very Slow
                    {
                        if (x264METype.SelectedIndex != 2)
                            x264METype.SelectedIndex = 2;
                        if (x264SubpelRefinement.SelectedIndex != 10)
                            x264SubpelRefinement.SelectedIndex = 10;
                        if (this.x264MERange.Value != 24)
                            this.x264MERange.Value = 24; 
                        if (x264NewAdaptiveBframes.SelectedIndex != 2)
                            x264NewAdaptiveBframes.SelectedIndex = 2;
                        if (x264BframePredictionMode.SelectedIndex != 3)
                            x264BframePredictionMode.SelectedIndex = 3;
                        if (macroblockOptions.SelectedIndex != 0)
                            macroblockOptions.SelectedIndex = 0;
                        if (trellis.SelectedIndex != 2)
                            trellis.SelectedIndex = 2;
                        if (x264MixedReferences.Checked)
                             x264MixedReferences.Checked = false;
                        if (!mbtree.Checked)
                            mbtree.Checked = true;
                        mbtree.Enabled = true;
                        if (!cabac.Checked)
                            cabac.Checked = true;
                        if (!chkTuneFastDecode.Checked)
                            cabac.Enabled = true;
                        if (!x264DeblockActive.Checked)
                             x264DeblockActive.Checked = true;
                        if (!scenecut.Checked)
                             scenecut.Checked = true;
                        if (noFastPSkip.Checked)
                            noFastPSkip.Checked = false;
                        if (!this.x264WeightedBPrediction.Checked)
                            this.x264WeightedBPrediction.Checked = true;
                        if (adaptiveDCT.Checked)
                            adaptiveDCT.Checked = false;
                    }
                    break;
                case 9: // Placebo
                    {
                        if (x264METype.SelectedIndex != 4)
                            x264METype.SelectedIndex = 4;
                        if (x264SubpelRefinement.SelectedIndex != 11)
                            x264SubpelRefinement.SelectedIndex = 11;
                        if (this.x264MERange.Value != 24)
                            this.x264MERange.Value = 24;
                        if (x264NewAdaptiveBframes.SelectedIndex != 2)
                            x264NewAdaptiveBframes.SelectedIndex = 2;
                        if (x264BframePredictionMode.SelectedIndex != 3)
                            x264BframePredictionMode.SelectedIndex = 3;
                        if (macroblockOptions.SelectedIndex != 0)
                            macroblockOptions.SelectedIndex = 0;
                        if (!noFastPSkip.Checked)
                            noFastPSkip.Checked = true;
                        if (trellis.SelectedIndex != 2)
                            trellis.SelectedIndex = 2;
                        if (x264MixedReferences.Checked)
                             x264MixedReferences.Checked = false;
                        if (!mbtree.Checked)
                            mbtree.Checked = true;
                        mbtree.Enabled = true;
                        if (!cabac.Checked)
                            cabac.Checked = true;
                        if (!chkTuneFastDecode.Checked)
                            cabac.Enabled = true;
                        if (!x264DeblockActive.Checked)
                             x264DeblockActive.Checked = true;
                        if (!scenecut.Checked)
                             scenecut.Checked = true;
                        if (!this.x264WeightedBPrediction.Checked)
                            this.x264WeightedBPrediction.Checked = true;
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
            //x264MinimimQuantizer.Enabled = enabled;
            //x264MaximumQuantizer.Enabled = enabled;
            //x264MaxQuantDelta.Enabled = enabled;
            //x264IPFrameFactor.Enabled = enabled;
            //x264PBFrameFactor.Enabled = enabled;
            //x264ChromaQPOffset.Enabled = enabled;
            x264RCGroupbox.Enabled = enabled;
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
            this.gbAQ.Enabled = true;
            if (isBitrateMode((VideoCodecSettings.VideoEncodingMode)x264EncodingMode.SelectedIndex))
            {
                this.x264BitrateQuantizerLabel.Text = "Bitrate";
                x264TempFrameComplexityBlur.Enabled = true;
                x264TempFrameComplexityBlurLabel.Enabled = true;
                x264TempQuantBlur.Enabled = true;
                x264TempQuantBlurLabel.Enabled = true;

                x264BitrateQuantizer.Maximum = 300000;
                x264BitrateQuantizer.Minimum = 0;
                x264BitrateQuantizer.DecimalPlaces = 0;
                x264BitrateQuantizer.Increment = 10;

                cbTarget.Text = "Targeting file size";
                tooltipHelp.SetToolTip(x264BitrateQuantizer, SelectHelpText("bitrate"));
            }
            else
            {
                x264TempFrameComplexityBlur.Enabled = false;
                x264TempFrameComplexityBlurLabel.Enabled = false;
                x264TempQuantBlur.Enabled = false;
                x264TempQuantBlurLabel.Enabled = false;
                if (x264EncodingMode.SelectedIndex == (int)VideoCodecSettings.VideoEncodingMode.CQ)
                {
                    this.x264BitrateQuantizerLabel.Text = "Quantizer";
                    this.gbAQ.Enabled = false;
                    tooltipHelp.SetToolTip(x264BitrateQuantizer, SelectHelpText("qp"));
                }
                if (x264EncodingMode.SelectedIndex == (int)VideoCodecSettings.VideoEncodingMode.quality)
                {
                    this.x264BitrateQuantizerLabel.Text = "Quality";
                    cbTarget.Text = "Targeting quality";
                    tooltipHelp.SetToolTip(x264BitrateQuantizer, SelectHelpText("crf"));
                }
              
                if (x264EncodingMode.SelectedIndex == (int)VideoCodecSettings.VideoEncodingMode.quality) // crf
                {
                    x264BitrateQuantizer.Maximum = 51;
                    x264BitrateQuantizer.Minimum = 0.0M;
                    x264BitrateQuantizer.DecimalPlaces = 1;
                    x264BitrateQuantizer.Increment = 0.1M;
                }
                else // qp
                {
                    x264BitrateQuantizer.Maximum = 69;
                    x264BitrateQuantizer.Minimum = 0;
                    x264BitrateQuantizer.Value = (int)x264BitrateQuantizer.Value; // makes sure it is an integer, in case we just swapped from crf                    
                    x264BitrateQuantizer.DecimalPlaces = 0;
                    x264BitrateQuantizer.Increment = 1;
                }

            }
            if (x264EncodingMode.SelectedIndex != (int)VideoCodecSettings.VideoEncodingMode.CQ)
                setNonQPOptionsEnabled(true);
            else
                setNonQPOptionsEnabled(false);

            switch (x264EncodingMode.SelectedIndex)
            {
                case (int)VideoCodecSettings.VideoEncodingMode.CBR: //Actually, ABR
                    x264SlowFirstpass.Enabled = false;
                    x264RateTol.Enabled = true;
                    x264RateTolLabel.Enabled = true;
                    logfileOpenButton.Enabled = false;
                    break;

                case (int)VideoCodecSettings.VideoEncodingMode.CQ:
                    x264SlowFirstpass.Enabled = false;
                    x264RateTol.Enabled = false;
                    x264RateTolLabel.Enabled = false;
                    logfileOpenButton.Enabled = false;
                    break;

                case (int)VideoCodecSettings.VideoEncodingMode.twopass1:
                case (int)VideoCodecSettings.VideoEncodingMode.threepass1:
                    x264SlowFirstpass.Enabled = true;
                    x264RateTol.Enabled = true;
                    x264RateTolLabel.Enabled = true;
                    logfileOpenButton.Enabled = true;
                    break;

                case (int)VideoCodecSettings.VideoEncodingMode.twopass2:
                case (int)VideoCodecSettings.VideoEncodingMode.threepass2:
                case (int)VideoCodecSettings.VideoEncodingMode.threepass3:
                    x264SlowFirstpass.Enabled = false;
                    x264RateTol.Enabled = true;
                    x264RateTolLabel.Enabled = true;
                    logfileOpenButton.Enabled = true;
                    break;
                case (int)VideoCodecSettings.VideoEncodingMode.twopassAutomated:
                case (int)VideoCodecSettings.VideoEncodingMode.threepassAutomated:
                    x264SlowFirstpass.Enabled = true;
                    x264RateTol.Enabled = true;
                    x264RateTolLabel.Enabled = true;
                    logfileOpenButton.Enabled = true;
                    break;
                case (int)VideoCodecSettings.VideoEncodingMode.quality:
                    x264SlowFirstpass.Enabled = false;
                    logfileOpenButton.Enabled = false;
                    x264RateTol.Enabled = false;
                    x264RateTolLabel.Enabled = false;
                    break;
            }

            // We check whether the bitrate/quality text needs to be changed
            if (isBitrateMode((VideoCodecSettings.VideoEncodingMode)lastEncodingMode) != isBitrateMode((VideoCodecSettings.VideoEncodingMode)x264EncodingMode.SelectedIndex))
            {
                if (isBitrateMode((VideoCodecSettings.VideoEncodingMode)x264EncodingMode.SelectedIndex))
                    this.x264BitrateQuantizer.Value = 1000;
                else
                    this.x264BitrateQuantizer.Value = 23;
            }

            // No Scenecut
            x264SCDSensitivity.Enabled = scenecut.Checked;

            lastEncodingMode = (VideoCodecSettings.VideoEncodingMode)x264EncodingMode.SelectedIndex;
        }
        #endregion
        #region level -> mb
        /// <summary>
        /// adjust the mb selection dropdown in function of the selected profile and the activated
        /// mb options
        /// </summary>
        public void doMBOptionsAdjustments()
        {
            if (!x264P8x8mv.Checked)
            {
                // x264P4x4mv.Checked = false;
                x264P4x4mv.Enabled = false;
            }
            switch (avcProfile.SelectedIndex)
            {
                case 0: // BP
                case 1: // MP
                    if (x264P8x8mv.Checked && x264B8x8mv.Checked && x264I4x4mv.Checked && x264P4x4mv.Checked)
                        this.macroblockOptions.SelectedIndex = 0;
                    else if (!x264P8x8mv.Checked && !x264B8x8mv.Checked && !x264I4x4mv.Checked && !x264P4x4mv.Checked)
                        this.macroblockOptions.SelectedIndex = 1;
                    else
                        this.macroblockOptions.SelectedIndex = 3;
                    break;
                case 2: // HP
                    if (x264P8x8mv.Checked && x264B8x8mv.Checked && x264I4x4mv.Checked && x264I8x8mv.Checked && x264P4x4mv.Checked && adaptiveDCT.Checked)
                        this.macroblockOptions.SelectedIndex = 0;
                    else if (!x264P8x8mv.Checked && !x264B8x8mv.Checked && !x264I4x4mv.Checked && !x264I8x8mv.Checked && !x264P4x4mv.Checked && !adaptiveDCT.Checked)
                        this.macroblockOptions.SelectedIndex = 1;
                    else
                        this.macroblockOptions.SelectedIndex = 3;
                    break;
            }
        }
        #endregion
        #endregion
        #region codec-specific overload functions
        protected override string getCommandline()
        {
            ulong x = 1;
            return x264Encoder.genCommandline("input", "output", null, -1, -1, 0, 1, ref x, Settings as x264Settings, null, null);
        }

        /// <summary>
        /// Does all the necessary adjustments after a GUI change has been made.
        /// </summary>
        protected override void doCodecSpecificAdjustments()
        {
            doDeviceSpecificAdjustments();
            doEncodingModeAdjustments();
            doCheckBoxAdjustments();
            doTrellisAdjustments();            
            doMacroBlockAdjustments();
            x264DialogTriStateAdjustment();
            doMacroBlockAdjustments();
            doSubmeAdjustments();
        }

        /// <summary>
        /// The method by which codecs can add things to the Load event
        /// </summary>
        protected override void doCodecSpecificLoadAdjustments()
        {
            if (x264EncodingMode.SelectedIndex == -1)
                this.x264EncodingMode.SelectedIndex = 0;
            if (x264SubpelRefinement.SelectedIndex == -1)
                this.x264SubpelRefinement.SelectedIndex = 7;
            if (x264BframePredictionMode.SelectedIndex == -1)
                this.x264BframePredictionMode.SelectedIndex = 1;
            if (x264METype.SelectedIndex == -1)
                this.x264METype.SelectedIndex = 0;
            if (macroblockOptions.SelectedIndex == -1)
                macroblockOptions.SelectedIndex = 3;
            if (cqmComboBox1.SelectedIndex == -1)
                cqmComboBox1.SelectedIndex = 0; // flat matrix
            if (this.avcProfile.SelectedIndex == -1)
                avcProfile.SelectedIndex = 2; // high
            if (cbAQMode.SelectedIndex == -1)
                cbAQMode.SelectedIndex = 1;
            if (x264Tunes.SelectedIndex == -1)
                x264Tunes.SelectedIndex = 0; // Default
            if (cbBPyramid.SelectedIndex == -1)
                cbBPyramid.SelectedIndex = 2;
            lastEncodingMode = (VideoCodecSettings.VideoEncodingMode)this.x264EncodingMode.SelectedIndex;
            
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
            tbx264Presets_Scroll(null, null); // to update the trackbar label
        }

        /// <summary>
        /// Returns whether settings is lavcSettings
        /// </summary>
        /// <param name="settings">The settings to check</param>
        /// <returns>Whether the settings are valid</returns>
        protected override bool isValidSettings(VideoCodecSettings settings)
        {
            return settings is x264Settings;
        }

        /// <summary>
        /// Returns a new instance of lavcSettings.
        /// </summary>
        /// <returns>A new instance of lavcSettings</returns>
        protected override VideoCodecSettings defaultSettings()
        {
            return new x264Settings();
        }

        /// <summary>
        /// gets / sets the settings currently displayed on the GUI
        /// </summary>
        public x264Settings Settings
        {
            get
            {
                x264Settings xs = new x264Settings();
                xs.DeadZoneInter = (int)deadzoneInter.Value;
                xs.DeadZoneIntra = (int)deadzoneIntra.Value;
                xs.InterlacedMode = (x264Settings.x264InterlacedModes)this.cbInterlaceMode.SelectedIndex;
                xs.NoDCTDecimate = this.noDCTDecimateOption.Checked;
                xs.SSIMCalculation = this.ssim.Checked;
                xs.PSNRCalculation = this.psnr.Checked;
                xs.StitchAble = this.stitchable.Checked;
                xs.NoFastPSkip = noFastPSkip.Checked;
                xs.X264SlowFirstpass = this.x264SlowFirstpass.Checked;
                xs.NoMixedRefs = x264MixedReferences.Checked;
                xs.VideoEncodingType = (VideoCodecSettings.VideoEncodingMode)x264EncodingMode.SelectedIndex;
                xs.BitrateQuantizer = (int)x264BitrateQuantizer.Value;
                xs.QuantizerCRF = x264BitrateQuantizer.Value;
                xs.KeyframeInterval = (int)x264KeyframeInterval.Value;
                xs.NbRefFrames = (int)this.x264NumberOfRefFrames.Value;
                xs.NbBframes = (int)this.x264NumberOfBFrames.Value;
                xs.NewAdaptiveBFrames = x264NewAdaptiveBframes.SelectedIndex;
                xs.PsyRDO = this.PsyRD.Value;
                xs.PsyTrellis = this.PsyTrellis.Value;
                xs.Deblock = x264DeblockActive.Checked;
                xs.AlphaDeblock = (int)x264AlphaDeblock.Value;
                xs.BetaDeblock = (int)x264BetaDeblock.Value;
                xs.Cabac = cabac.Checked;
                xs.SubPelRefinement = this.x264SubpelRefinement.SelectedIndex;
                xs.WeightedBPrediction = x264WeightedBPrediction.Checked;
                xs.WeightedPPrediction = x264WeightedPPrediction.SelectedIndex;
                xs.ChromaME = this.x264ChromaMe.Checked;
                xs.X264Trellis = trellis.SelectedIndex;
                xs.MacroBlockOptions = macroblockOptions.SelectedIndex; 
                xs.P8x8mv = x264P8x8mv.Checked;
                xs.B8x8mv = x264B8x8mv.Checked;
                xs.I4x4mv = x264I4x4mv.Checked;
                xs.I8x8mv = x264I8x8mv.Checked;
                xs.P4x4mv = x264P4x4mv.Checked;
                xs.MinQuantizer = (int)x264MinimimQuantizer.Value;
                xs.MaxQuantizer = (int)x264MaximumQuantizer.Value;
                xs.MaxQuantDelta = (int)x264MaxQuantDelta.Value;
                xs.CreditsQuantizer = (int)this.x264CreditsQuantizer.Value;
                xs.ChromaQPOffset = this.x264ChromaQPOffset.Value;
                xs.IPFactor = x264IPFrameFactor.Value;
                xs.PBFactor = x264PBFrameFactor.Value;
                xs.VBVBufferSize = (int)x264VBVBufferSize.Value;
                xs.VBVMaxBitrate = (int)x264VBVMaxRate.Value;
                xs.VBVInitialBuffer = x264VBVInitialBuffer.Value;
                xs.BitrateVariance = x264RateTol.Value;
                xs.QuantCompression = x264QuantizerCompression.Value;
                xs.TempComplexityBlur = (int)x264TempFrameComplexityBlur.Value;
                xs.TempQuanBlurCC = x264TempQuantBlur.Value;
                xs.SCDSensitivity = (int)this.x264SCDSensitivity.Value;
                xs.BframeBias = (int)this.x264BframeBias.Value;
                xs.BframePredictionMode = this.x264BframePredictionMode.SelectedIndex;
                xs.METype = this.x264METype.SelectedIndex;
                xs.MERange = (int)x264MERange.Value;
                xs.NbThreads = (int)x264NbThreads.Value;
                xs.MinGOPSize = (int)x264MinGOPSize.Value;
                xs.Logfile = this.logfile.Text;
                xs.AdaptiveDCT = adaptiveDCT.Checked;
                xs.CustomEncoderOptions = customCommandlineOptions.Text;
                if (cqmComboBox1.SelectedIndex > 1)
                    xs.QuantizerMatrixType = 2;
                else
                    xs.QuantizerMatrixType = cqmComboBox1.SelectedIndex;
                xs.QuantizerMatrix = cqmComboBox1.SelectedText;
                xs.Profile = avcProfile.SelectedIndex;
                xs.AVCLevel = getAVCLevel();
                xs.NoiseReduction = (int)NoiseReduction.Value;
                xs.AQmode = (int)cbAQMode.SelectedIndex;
                xs.AQstrength = numAQStrength.Value;
                xs.OpenGopValue = chkOpenGop.Checked;
                xs.ColorPrim = (int)colorPrim.SelectedIndex;
                xs.Transfer = (int)transfer.SelectedIndex;
                xs.ColorMatrix = (int)colorMatrix.SelectedIndex;
                xs.X264PullDown = (int)x264PullDown.SelectedIndex;
                xs.SampleAR = (int)sampleAR.SelectedIndex;
                xs.PicStruct = picStruct.Checked;
                xs.FakeInterlaced = fakeInterlaced.Checked;
                xs.NonDeterministic = nonDeterministic.Checked;
                xs.UseQPFile = useQPFile.Checked;
                xs.QPFile = this.qpfile.Text;
                xs.Range = this.x264Range.SelectedItem.ToString();
                xs.x264PresetLevel = (x264Settings.x264PresetLevelModes)this.tbx264Presets.Value;
                xs.x264PsyTuning = getPsyTuning();
                xs.TuneFastDecode = chkTuneFastDecode.Checked;
                xs.TuneZeroLatency = chkTuneZeroLatency.Checked;
                xs.NoMBTree = mbtree.Checked;
                xs.Lookahead = (int)lookahead.Value;
                xs.ThreadInput = threadin.Checked;
                xs.NoPsy = nopsy.Checked;
                xs.Scenecut = scenecut.Checked;
                xs.SlicesNb = (int)this.slicesnb.Value;
                xs.MaxSliceSyzeBytes = (int)this.maxSliceSizeBytes.Value;
                xs.MaxSliceSyzeMBs = (int)this.maxSliceSizeMB.Value;
                xs.x264BFramePyramid = this.cbBPyramid.SelectedIndex;
                xs.x264GOPCalculation = this.cbGOPCalculation.SelectedIndex;
                xs.X264Aud = x264aud.Checked;
                xs.Nalhrd = (int)x264hrd.SelectedIndex;
                foreach (x264Device oDevice in x264DeviceList)
                {
                    if (oDevice.Name.Equals(targetDevice.SelectedItem.ToString(), StringComparison.CurrentCultureIgnoreCase))
                    {
                        xs.TargetDevice = oDevice;
                        break;
                    }
                }
                xs.BlurayCompat = chkBlurayCompat.Checked;
                xs.X26410Bits = ch10BitsEncoder.Checked;
                return xs;
            }
            set
            {  // Warning! The ordering of components matters because of the dependency code!
                x264Settings xs = value;
                updating = true;
                tbx264Presets.Value = (int)xs.x264PresetLevel;
                if (xs.Profile > 2)
                    avcProfile.SelectedIndex = 2;
                else
                    avcProfile.SelectedIndex = xs.Profile;
                avcLevel.SelectedItem = EnumProxy.Create(xs.AVCLevel);
                x264Tunes.SelectedItem = EnumProxy.Create(xs.x264PsyTuning);
                chkTuneFastDecode.Checked = xs.TuneFastDecode;
                chkTuneZeroLatency.Checked = xs.TuneZeroLatency;
                deadzoneInter.Value = xs.DeadZoneInter;
                deadzoneIntra.Value = xs.DeadZoneIntra;
                cbInterlaceMode.SelectedIndex = (int)xs.InterlacedMode;
                noDCTDecimateOption.Checked = xs.NoDCTDecimate;
                ssim.Checked = xs.SSIMCalculation;
                stitchable.Checked = xs.StitchAble;
                updateDeviceBlocked = true;
                targetDevice.SelectedItem = xs.TargetDevice.Name;
                updateDeviceBlocked = false;
                chkBlurayCompat.Checked = xs.BlurayCompat;
                x264EncodingMode.SelectedIndex = (int)xs.VideoEncodingType;
                doEncodingModeAdjustments();
                this.x264NumberOfRefFrames.Value = xs.NbRefFrames;
                this.x264NumberOfBFrames.Value = xs.NbBframes;
                noFastPSkip.Checked = xs.NoFastPSkip;
                this.x264SubpelRefinement.SelectedIndex = xs.SubPelRefinement;
                x264SlowFirstpass.Checked = xs.X264SlowFirstpass;
                x264BitrateQuantizer.Value = (isBitrateMode(xs.VideoEncodingType) || xs.QuantizerCRF == 0) ? xs.BitrateQuantizer : xs.QuantizerCRF;
                x264KeyframeInterval.Text = xs.KeyframeInterval.ToString() ;
                x264NewAdaptiveBframes.SelectedIndex = xs.NewAdaptiveBFrames;
                x264DeblockActive.Checked = xs.Deblock;
                x264AlphaDeblock.Value = xs.AlphaDeblock;
                x264BetaDeblock.Value = xs.BetaDeblock;
                cabac.Checked = xs.Cabac;
                x264ChromaMe.Checked = xs.ChromaME;
                PsyRD.Value = xs.PsyRDO;
                trellis.SelectedIndex = xs.X264Trellis;
                PsyTrellis.Value = xs.PsyTrellis;
                macroblockOptions.SelectedIndex = xs.MacroBlockOptions;
                if (macroblockOptions.SelectedIndex != 1)
                {
                    adaptiveDCT.Checked = xs.AdaptiveDCT;
                    x264P8x8mv.Checked = xs.P8x8mv;
                    x264B8x8mv.Checked = xs.B8x8mv;
                    x264I4x4mv.Checked = xs.I4x4mv;
                    x264I8x8mv.Checked = xs.I8x8mv;
                    x264P4x4mv.Checked = xs.P4x4mv;
                }
                x264MinimimQuantizer.Value = xs.MinQuantizer;
                x264MaximumQuantizer.Value = xs.MaxQuantizer;
                x264MaxQuantDelta.Value = xs.MaxQuantDelta;
                this.x264CreditsQuantizer.Value = xs.CreditsQuantizer;
                x264IPFrameFactor.Value = xs.IPFactor;
                x264PBFrameFactor.Value = xs.PBFactor;
                x264ChromaQPOffset.Value = xs.ChromaQPOffset;
                if (xs.VBVBufferSize > 0)
                {
                    this.x264VBVMaxRate.Enabled = this.x264VBVMaxRateLabel.Enabled = true;
                    x264VBVBufferSize.Text = xs.VBVBufferSize.ToString();
                }
                else
                {
                    this.x264VBVMaxRate.Enabled = this.x264VBVMaxRateLabel.Enabled = false;
                    x264VBVBufferSize.Text = "0";
                }
                if (xs.VBVMaxBitrate > 0)
                {
                    this.x264VBVMaxRate.Enabled = this.x264VBVMaxRateLabel.Enabled = true;
                    x264VBVMaxRate.Text = xs.VBVMaxBitrate.ToString();
                }
                else
                {
                    x264VBVBufferSize.Text = "0";
                    x264VBVMaxRate.Text = "0";
                }
                x264VBVInitialBuffer.Value = xs.VBVInitialBuffer;
                x264RateTol.Value = xs.BitrateVariance;
                x264QuantizerCompression.Value = xs.QuantCompression;
                x264TempFrameComplexityBlur.Value = xs.TempComplexityBlur;
                x264TempQuantBlur.Value = xs.TempQuanBlurCC;
                this.x264SCDSensitivity.Value = xs.SCDSensitivity;
                this.x264BframeBias.Value = xs.BframeBias;
                this.x264BframePredictionMode.SelectedIndex = xs.BframePredictionMode;
                this.x264METype.SelectedIndex = xs.METype;
                x264MERange.Value = xs.MERange;
                x264NbThreads.Value = xs.NbThreads;
                x264MinGOPSize.Text = xs.MinGOPSize.ToString();
                customCommandlineOptions.Text = xs.CustomEncoderOptions;
                this.logfile.Text = xs.Logfile;
                cqmComboBox1.SelectedObject = xs.QuantizerMatrix;
                psnr.Checked = xs.PSNRCalculation;
                cbAQMode.SelectedIndex = xs.AQmode;
                chkOpenGop.Checked = xs.OpenGopValue;
                colorPrim.SelectedIndex = xs.ColorPrim;
                transfer.SelectedIndex = xs.Transfer;
                colorMatrix.SelectedIndex = xs.ColorMatrix;
                x264PullDown.SelectedIndex = xs.X264PullDown;
                sampleAR.SelectedIndex = xs.SampleAR;
                picStruct.Checked = xs.PicStruct;
                fakeInterlaced.Checked = xs.FakeInterlaced;
                nonDeterministic.Checked = xs.NonDeterministic;
                x264Range.SelectedItem = xs.Range;
                numAQStrength.Value = xs.AQstrength;
                NoiseReduction.Text = xs.NoiseReduction.ToString();
                lookahead.Value = xs.Lookahead;
                mbtree.Checked = xs.NoMBTree;
                threadin.Checked = xs.ThreadInput;
                nopsy.Checked = xs.NoPsy;
                x264MixedReferences.Checked = xs.NoMixedRefs;
                scenecut.Checked = xs.Scenecut;
                this.slicesnb.Value = xs.SlicesNb;
                this.maxSliceSizeBytes.Value = xs.MaxSliceSyzeBytes;
                this.maxSliceSizeMB.Value = xs.MaxSliceSyzeMBs;
                this.cbBPyramid.SelectedIndex = xs.x264BFramePyramid;
                this.cbGOPCalculation.SelectedIndex = xs.x264GOPCalculation;
                x264WeightedBPrediction.Checked = xs.WeightedBPrediction;
                x264WeightedPPrediction.SelectedIndex = xs.WeightedPPrediction;
                x264aud.Checked = xs.X264Aud;
                x264hrd.SelectedIndex = xs.Nalhrd;
                useQPFile.Checked = xs.UseQPFile;
                this.qpfile.Text = xs.QPFile;
                ch10BitsEncoder.Checked = xs.X26410Bits;
                updating = false;
                genericUpdate();
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
            saveFileDialog.Filter = "x264 2pass stats files (*.stats)|*.stats";
            saveFileDialog.FilterIndex = 1;
            if (this.saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                this.logfile.Text = saveFileDialog.FileName;
                this.showCommandLine();
            }
        }
        #endregion
        #region ContextHelp
        private string SelectHelpText(string node)
        {
            StringBuilder HelpText = new StringBuilder(64);

            try
            {
                string xpath = "/ContextHelp/Codec[@name='x264']/" + node;
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
            tooltipHelp.SetToolTip(avcProfile, SelectHelpText("profile"));
            tooltipHelp.SetToolTip(avcLevel, SelectHelpText("level"));
            tooltipHelp.SetToolTip(x264Tunes, SelectHelpText("tunes"));
            tooltipHelp.SetToolTip(cbTarget, SelectHelpText("targetmode"));
            tooltipHelp.SetToolTip(tbx264Presets, SelectHelpText("presets"));
            tooltipHelp.SetToolTip(x264BitrateQuantizer, SelectHelpText("bitrate"));
            tooltipHelp.SetToolTip(targetDevice, SelectHelpText("device"));
            tooltipHelp.SetToolTip(x264EncodingMode, SelectHelpText("encodingmode"));
            tooltipHelp.SetToolTip(ch10BitsEncoder, SelectHelpText("encoder10bit"));
            
            /**************/
            /* Frame-Type */
            /**************/
            tooltipHelp.SetToolTip(x264KeyframeInterval, SelectHelpText("keyint"));
            tooltipHelp.SetToolTip(NoiseReduction, SelectHelpText("nr"));
            tooltipHelp.SetToolTip(noFastPSkip, SelectHelpText("no-fast-pskip"));
            tooltipHelp.SetToolTip(macroblockOptions, SelectHelpText("partitions"));
            tooltipHelp.SetToolTip(x264ChromaMe, SelectHelpText("no-chroma-me"));
            tooltipHelp.SetToolTip(x264WeightedBPrediction, SelectHelpText("weightb"));
            tooltipHelp.SetToolTip(x264WeightedPPrediction, SelectHelpText("weightp"));
            tooltipHelp.SetToolTip(x264SubpelRefinement, SelectHelpText("subme"));
            tooltipHelp.SetToolTip(cabac, SelectHelpText("no-cabac"));
            tooltipHelp.SetToolTip(x264DeblockActive, SelectHelpText("nf"));
            tooltipHelp.SetToolTip(x264NewAdaptiveBframes, SelectHelpText("b-adapt"));
            tooltipHelp.SetToolTip(x264NumberOfRefFrames, SelectHelpText("ref"));
            tooltipHelp.SetToolTip(x264NumberOfBFrames, SelectHelpText("bframes"));
            tooltipHelp.SetToolTip(x264AlphaDeblock, SelectHelpText("filter"));
            tooltipHelp.SetToolTip(x264BetaDeblock, SelectHelpText("filter"));
            tooltipHelp.SetToolTip(x264CreditsQuantizer, SelectHelpText("creditsquant"));
            tooltipHelp.SetToolTip(x264IPFrameFactor, SelectHelpText("ipratio"));
            tooltipHelp.SetToolTip(x264PBFrameFactor, SelectHelpText("pbratio"));
            tooltipHelp.SetToolTip(x264ChromaQPOffset, SelectHelpText("chroma-qp-offset"));
            tooltipHelp.SetToolTip(cbBPyramid, SelectHelpText("b-pyramid"));
            tooltipHelp.SetToolTip(x264SlowFirstpass, SelectHelpText("slow-firstpass"));
            tooltipHelp.SetToolTip(cbInterlaceMode, SelectHelpText("interlaced"));
            tooltipHelp.SetToolTip(x264PullDown, SelectHelpText("pulldown"));
            tooltipHelp.SetToolTip(scenecut, SelectHelpText("noscenecut"));
            tooltipHelp.SetToolTip(chkOpenGop, SelectHelpText("opengop"));
            tooltipHelp.SetToolTip(slicesnb, SelectHelpText("slicesnb"));
            tooltipHelp.SetToolTip(maxSliceSizeBytes, SelectHelpText("maxSliceSizeBytes"));
            tooltipHelp.SetToolTip(maxSliceSizeMB, SelectHelpText("maxSliceSizeMB"));
            tooltipHelp.SetToolTip(x264MinGOPSize, SelectHelpText("min-keyint"));
            tooltipHelp.SetToolTip(x264SCDSensitivity, SelectHelpText("scenecut"));
            tooltipHelp.SetToolTip(x264BframeBias, SelectHelpText("b-bias"));
            tooltipHelp.SetToolTip(x264BframePredictionMode, SelectHelpText("direct"));
            tooltipHelp.SetToolTip(cbGOPCalculation, SelectHelpText("gopcalculation"));

            /*************************/
            /* Rate Control Tooltips */
            /*************************/
            tooltipHelp.SetToolTip(x264MinimimQuantizer, SelectHelpText("qpmin"));
            tooltipHelp.SetToolTip(x264MaximumQuantizer, SelectHelpText("qpmax"));
            tooltipHelp.SetToolTip(x264MaxQuantDelta, SelectHelpText("qpstep"));
            tooltipHelp.SetToolTip(x264VBVBufferSize, SelectHelpText("vbv-bufsize"));
            tooltipHelp.SetToolTip(x264VBVMaxRate, SelectHelpText("vbv-maxrate"));
            tooltipHelp.SetToolTip(x264VBVInitialBuffer, SelectHelpText("vbv-init"));
            tooltipHelp.SetToolTip(x264RateTol, SelectHelpText("ratetol"));
            tooltipHelp.SetToolTip(x264QuantizerCompression, SelectHelpText("qcomp"));
            tooltipHelp.SetToolTip(x264TempFrameComplexityBlur, SelectHelpText("cplxblur"));
            tooltipHelp.SetToolTip(x264TempQuantBlur, SelectHelpText("qblur"));
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
            tooltipHelp.SetToolTip(x264B8x8mv, SelectHelpText("b8x8mv"));
            tooltipHelp.SetToolTip(x264P8x8mv, SelectHelpText("p8x8mv"));
            tooltipHelp.SetToolTip(x264P4x4mv, SelectHelpText("p4x4mv"));
            tooltipHelp.SetToolTip(x264I4x4mv, SelectHelpText("i4x4mv"));
            tooltipHelp.SetToolTip(x264I8x8mv, SelectHelpText("i8x8mv"));
            tooltipHelp.SetToolTip(x264aud, SelectHelpText("aud"));
            tooltipHelp.SetToolTip(x264hrd, SelectHelpText("nalhrd"));
            tooltipHelp.SetToolTip(noDCTDecimateOption, SelectHelpText("noDCTDecimateOption"));
            tooltipHelp.SetToolTip(nopsy, SelectHelpText("nopsy"));
            tooltipHelp.SetToolTip(fakeInterlaced, SelectHelpText("fakeInterlaced"));
            tooltipHelp.SetToolTip(chkBlurayCompat, SelectHelpText("blurayCompat"));
            tooltipHelp.SetToolTip(x264MixedReferences, SelectHelpText("mixed-refs"));
            tooltipHelp.SetToolTip(PsyRD, SelectHelpText("psyrd"));
            tooltipHelp.SetToolTip(PsyTrellis, SelectHelpText("psytrellis"));
            tooltipHelp.SetToolTip(trellis, SelectHelpText("trellis"));
            tooltipHelp.SetToolTip(x264METype, SelectHelpText("me"));
            tooltipHelp.SetToolTip(x264MERange, SelectHelpText("merange"));

            /**************************/
            /* Misc Tooltips */
            /**************************/
            tooltipHelp.SetToolTip(x264NbThreads, SelectHelpText("threads"));
            tooltipHelp.SetToolTip(psnr, SelectHelpText("psnr"));
            tooltipHelp.SetToolTip(ssim, SelectHelpText("ssim"));
            tooltipHelp.SetToolTip(stitchable, SelectHelpText("stitchable"));
            tooltipHelp.SetToolTip(logfile, SelectHelpText("logfile"));
            tooltipHelp.SetToolTip(customCommandlineOptions, SelectHelpText("customcommandline"));
            tooltipHelp.SetToolTip(qpfile, SelectHelpText("qpf"));
            tooltipHelp.SetToolTip(useQPFile, SelectHelpText("qpf"));
            tooltipHelp.SetToolTip(sampleAR, SelectHelpText("sampleAR"));
            tooltipHelp.SetToolTip(x264Range, SelectHelpText("x264Range"));
            tooltipHelp.SetToolTip(picStruct, SelectHelpText("picStruct"));
            tooltipHelp.SetToolTip(colorPrim, SelectHelpText("colorPrim"));
            tooltipHelp.SetToolTip(colorMatrix, SelectHelpText("colorMatrix"));
            tooltipHelp.SetToolTip(transfer, SelectHelpText("transfer"));
            tooltipHelp.SetToolTip(nonDeterministic, SelectHelpText("nonDeterministic"));
            tooltipHelp.SetToolTip(threadin, SelectHelpText("threadin"));
            tooltipHelp.SetToolTip(chkTuneFastDecode, SelectHelpText("fastdecode"));
            tooltipHelp.SetToolTip(chkTuneZeroLatency, SelectHelpText("zerolatency"));

        }
        #endregion
        #region GUI State adjustment
        private void x264DialogTriStateAdjustment()
        {
            bool turboOptions = this.x264SlowFirstpass.Checked &&
                (this.x264EncodingMode.SelectedIndex == (int)VideoCodecSettings.VideoEncodingMode.threepass1 ||
                 this.x264EncodingMode.SelectedIndex == (int)VideoCodecSettings.VideoEncodingMode.twopass1);

            // First we do the Profile Adjustments
            #region profile adjustments
            switch (avcProfile.SelectedIndex)
            {
                case 0: // baseline, disable cabac, b-frames and i8x8
                    cabac.Checked = false;
                    cabac.Enabled = false;
                    x264NumberOfBFrames.Value = 0;
                    x264NumberOfBFrames.Enabled = false;
                    x264NumberOfBFramesLabel.Enabled = false;
                    cqmComboBox1.SelectedIndex = 0;
                    quantizerMatrixGroupbox.Enabled = false;
                    break;
                case 1: // main profile, disable i8x8
                    if (!x264NumberOfBFrames.Enabled)
                    {
                        x264NumberOfBFrames.Enabled = true;
                        x264NumberOfBFrames.Value = 3;
                        x264NumberOfBFramesLabel.Enabled = true;
                        x264WeightedBPrediction.Checked = true;
                    }
                    cqmComboBox1.SelectedIndex = 0;
                    quantizerMatrixGroupbox.Enabled = false;
                    if (!chkTuneFastDecode.Checked  && !cabac.Enabled)
                    {
                        cabac.Enabled = true;
                        cabac.Checked = true;
                    }
                    break;
                case 2: // high profile, enable everything
                    if (!x264NumberOfBFrames.Enabled)
                    {
                        x264NumberOfBFrames.Enabled = true;
                        x264NumberOfBFrames.Value = 3;
                        x264NumberOfBFramesLabel.Enabled = true;
                        x264WeightedBPrediction.Checked = true;
                    }
                    if (!adaptiveDCT.Enabled)
                    {
                        adaptiveDCT.Enabled = true;
                        adaptiveDCT.Checked = true;
                    }
                    quantizerMatrixGroupbox.Enabled = true;
                    if (!chkTuneFastDecode.Checked && !cabac.Enabled)
                    {
                        cabac.Enabled = true;
                        cabac.Checked = true;
                    }
                    break;
            }
            #endregion

            // Now we do B frames adjustments
            #region b-frames
            if (this.x264NumberOfBFrames.Value == 0)
            {
                this.x264NewAdaptiveBframes.Enabled = false;
                this.x264AdaptiveBframesLabel.Enabled = false;
                this.x264BframePredictionMode.Enabled = false;
                this.x264BframePredictionModeLabel.Enabled = false;
                this.x264WeightedBPrediction.Checked = false;
                this.x264WeightedBPrediction.Enabled = false;
                this.x264BframeBias.Value = 0;
                this.x264BframeBias.Enabled = false;
                this.x264BframeBiasLabel.Enabled = false;
                this.cbBPyramid.Enabled = false;
            }
            else
            {
                this.x264NewAdaptiveBframes.Enabled = true;
                this.x264AdaptiveBframesLabel.Enabled = true;
                this.x264BframePredictionMode.Enabled = true;
                this.x264BframePredictionModeLabel.Enabled = true;
                this.x264WeightedBPrediction.Enabled = true;
                // We can enable these if we don't have turbo options
                this.x264BframeBias.Enabled = true;
                this.x264BframeBiasLabel.Enabled = true;
                if (this.x264NumberOfBFrames.Value >= 2) // pyramid requires at least two b-frames
                    this.cbBPyramid.Enabled = true;
                else
                    this.cbBPyramid.Enabled = false;
            }
            #endregion

            // Now we do some additional checks -- ref frames, cabac
            #region extra checks
            if (!string.IsNullOrEmpty(x264VBVBufferSize.Text))
            {
                this.x264VBVMaxRate.Enabled = true;
                this.x264VBVMaxRateLabel.Enabled = true;
            }
            if (this.x264NumberOfRefFrames.Value > 1) // mixed references require at least two reference frames
            {
                if (!this.x264MixedReferences.Enabled)
                    this.x264MixedReferences.Enabled = true;
            }
            else
                this.x264MixedReferences.Enabled = false;

            if (this.x264SubpelRefinement.SelectedIndex > 4)
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
                switch (getPsyTuning())
                {
                    case x264Settings.x264PsyTuningModes.FILM: this.PsyTrellis.Value = 0.15M; break;
                    case x264Settings.x264PsyTuningModes.GRAIN: this.PsyTrellis.Value = 0.25M; break;
                    default: this.PsyTrellis.Value = 0; break;
                }
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
                System.Diagnostics.Process.Start("http://www.videolan.org/developers/x264.html");
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

        private void useQPFile_CheckedChanged(object sender, EventArgs e)
        {
            qpfile.Enabled = useQPFile.Checked;
            qpfileOpenButton.Enabled = useQPFile.Checked;
        }

        private void qpfileOpenButton_Click(object sender, EventArgs e)
        {
            openFileDialog.Filter = "x264 QP Files (*.qpf, *.txt)|*.qpf;*.txt";
            openFileDialog.FilterIndex = 1;
            openFileDialog.FileName = "";
            if (this.openFileDialog.ShowDialog() == DialogResult.OK)
            {
                this.qpfile.Text = openFileDialog.FileName;
                this.showCommandLine();
            }
        }

        private void tbx264Presets_Scroll(object sender, EventArgs e)
        {
            switch (tbx264Presets.Value)
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

        private void advancedSettings_CheckedChanged(object sender, EventArgs e)
        {
            MainForm.Instance.Settings.X264AdvancedSettings = advancedSettings.Checked;
            if (MainForm.Instance.Settings.X264AdvancedSettings)
            {
                if (!tabControl1.TabPages.Contains(FrameTypeTabPage))
                    tabControl1.TabPages.Add(FrameTypeTabPage);
                if (!tabControl1.TabPages.Contains(RCTabPage))
                    tabControl1.TabPages.Add(RCTabPage);
                if (!tabControl1.TabPages.Contains(AnalysisTabPage))
                    tabControl1.TabPages.Add(AnalysisTabPage);
                if (!tabControl1.TabPages.Contains(MiscTabPage))
                    tabControl1.TabPages.Add(MiscTabPage);
                x264EncodingMode.Visible = true;
                cbTarget.Visible = false;
                avcProfileGroupbox.Enabled = !ch10BitsEncoder.Checked;
                avcLevelGroupbox.Enabled = true;
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
                x264EncodingMode.Visible = false;
                cbTarget.Visible = true;
                avcProfileGroupbox.Enabled = false;
                avcLevelGroupbox.Enabled = false;
            }
            genericUpdate();
        }

        private void dSettings_Click(object sender, EventArgs e)
        {
            // Main Tab
            this.x264EncodingMode.SelectedIndex = (int)VideoCodecSettings.VideoEncodingMode.quality;
            this.x264BitrateQuantizer.Value = 23;
            this.x264Tunes.SelectedIndex = 0;
            this.tbx264Presets.Value = 5;
            this.avcProfile.SelectedIndex = 2;
            this.avcLevel.SelectedItem = EnumProxy.Create(AVCLevels.Levels.L_UNRESTRICTED);
            this.advancedSettings.Checked = true;
            this.targetDevice.SelectedIndex = 0;
            this.ch10BitsEncoder.Enabled = false;
            this.ch10BitsEncoder.Checked = false;

            // Frame-Type Tab
            this.x264DeblockActive.Checked = true;
            this.x264AlphaDeblock.Value = 0;
            this.x264BetaDeblock.Value = 0;
            this.cabac.Checked = true;
            this.x264KeyframeInterval.Text = "250";
            this.x264MinGOPSize.Text = "25";
            this.chkOpenGop.Checked = false;
            this.slicesnb.Value = 0;
            this.maxSliceSizeBytes.Value = 0;
            this.maxSliceSizeMB.Value = 0;
            this.x264WeightedBPrediction.Checked = true;
            this.x264NumberOfBFrames.Value = 3;
            this.x264BframeBias.Value = 0;
            this.x264NewAdaptiveBframes.SelectedIndex = 1;
            this.cbBPyramid.SelectedIndex = 2;
            this.x264NumberOfRefFrames.Value = 3;
            this.x264SCDSensitivity.Value = 40;
            this.x264WeightedPPrediction.SelectedIndex = 2;
            this.cbInterlaceMode.SelectedIndex = (int)x264Settings.x264InterlacedModes.progressive;
            this.x264PullDown.SelectedIndex = 0;
            this.scenecut.Checked = true;
            this.cbGOPCalculation.SelectedIndex = 1;
            
            // Rate Control Tab
            this.x264MinimimQuantizer.Value = 0;
            this.x264MaximumQuantizer.Value = 81;
            this.x264MaxQuantDelta.Value = 4;
            this.x264IPFrameFactor.Value = 1.4M;
            this.x264PBFrameFactor.Value = 1.3M;
            this.deadzoneInter.Value = 0;
            this.deadzoneIntra.Value = 0;
            this.x264ChromaQPOffset.Value = 0;
            this.x264CreditsQuantizer.Value = 40;
            this.x264VBVBufferSize.Text = "0";
            this.x264VBVMaxRate.Text = "0";
            this.x264VBVInitialBuffer.Value = 0.9M;
            this.x264RateTol.Value = 1.0M;
            this.x264QuantizerCompression.Value = 0.6M;
            this.x264TempFrameComplexityBlur.Value = 20;
            this.x264TempQuantBlur.Value = 0.5M;
            this.lookahead.Value = 40;
            this.cbAQMode.SelectedIndex = 1;
            this.numAQStrength.Value = 1.0M;
            this.mbtree.Checked = true;
            this.cqmComboBox1.SelectedIndex = 0; 

            // Analysis Tab
            this.x264ChromaMe.Checked = true;
            this.x264MERange.Value = 16;
            this.x264METype.SelectedIndex = 1;                 
            this.x264SubpelRefinement.SelectedIndex = 7;
            this.x264BframePredictionMode.SelectedIndex = 1;
            this.trellis.SelectedIndex = 1;
            this.PsyRD.Value = 1.0M;
            this.PsyTrellis.Value = 0.0M;
            this.x264MixedReferences.Checked = false;
            this.noDCTDecimateOption.Checked = false;
            this.noFastPSkip.Checked = false;
            this.nopsy.Checked = false;
            this.NoiseReduction.Text = "0";
            this.macroblockOptions.SelectedIndex = 3;
            this.adaptiveDCT.Checked = true;
            this.x264I4x4mv.Checked = true;
            this.x264P4x4mv.Checked = false;
            this.x264I8x8mv.Checked = true;
            this.x264P8x8mv.Checked = true;
            this.x264B8x8mv.Checked = true;
            this.x264hrd.SelectedIndex = 0;
            this.x264aud.Checked = false;
            this.fakeInterlaced.Checked = false;
            this.chkBlurayCompat.Checked = false;

            // Misc Tab
            this.useQPFile.Checked = false;
            this.psnr.Checked = false;
            this.ssim.Checked = false;
            this.sampleAR.SelectedIndex = 0;
            this.x264Range.SelectedIndex = 0;
            this.picStruct.Checked = false;
            this.colorPrim.SelectedIndex = 0;
            this.transfer.SelectedIndex = 0;
            this.colorMatrix.SelectedIndex = 0;
            this.x264SlowFirstpass.Checked = false;
            this.threadin.Checked = true;
            this.x264NbThreads.Value = 0;
            this.nonDeterministic.Checked = false;
            this.chkTuneFastDecode.Checked = false;
            this.chkTuneZeroLatency.Checked = false;
            this.stitchable.Checked = false;

            // to update presets label
            tbx264Presets_Scroll(null, null);
        }

        private void btPresetSettings_Click(object sender, EventArgs e)
        {
            doPresetsAdjustments();
            doTuningsAdjustments();
        }

        private void x264Tunes_SelectedIndexChanged(object sender, EventArgs e)
        {
            doTuningsAdjustments();
            genericUpdate();
        }

        private void avcProfile_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.x264NumberOfBFrames.Value != x264Settings.GetDefaultNumberOfBFrames((x264Settings.x264PresetLevelModes)tbx264Presets.Value, getPsyTuning(), chkTuneZeroLatency.Checked, avcProfile.SelectedIndex, oTargetDevice, chkBlurayCompat.Checked))
                this.x264NumberOfBFrames.Value = x264Settings.GetDefaultNumberOfBFrames((x264Settings.x264PresetLevelModes)tbx264Presets.Value, getPsyTuning(), chkTuneZeroLatency.Checked, avcProfile.SelectedIndex, oTargetDevice, chkBlurayCompat.Checked);
            if (this.x264WeightedPPrediction.SelectedIndex != x264Settings.GetDefaultNumberOfWeightp((x264Settings.x264PresetLevelModes)tbx264Presets.Value, chkTuneFastDecode.Checked, avcProfile.SelectedIndex, chkBlurayCompat.Checked))
                this.x264WeightedPPrediction.SelectedIndex = x264Settings.GetDefaultNumberOfWeightp((x264Settings.x264PresetLevelModes)tbx264Presets.Value, chkTuneFastDecode.Checked, avcProfile.SelectedIndex, chkBlurayCompat.Checked);

            avcLevel_SelectedIndexChanged(null, null);
        }

        private void cbTarget_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (cbTarget.SelectedIndex == 0)
            {
                if (MainForm.Instance.Settings.NbPasses == 3)
                    x264EncodingMode.SelectedIndex = (int)VideoCodecSettings.VideoEncodingMode.threepassAutomated;
                else
                    x264EncodingMode.SelectedIndex = (int)VideoCodecSettings.VideoEncodingMode.twopassAutomated;
            }
            else
            {
                x264EncodingMode.SelectedIndex = (int)VideoCodecSettings.VideoEncodingMode.quality;
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            VisitLink();
        }

        private void targetDevice_SelectedIndexChanged(object sender, EventArgs e)
        {
            oTargetDevice = x264DeviceList[0];
            foreach (x264Device oDevice in x264DeviceList)
            {
                if (oDevice.Name.Equals(targetDevice.SelectedItem.ToString(), StringComparison.CurrentCultureIgnoreCase))
                {
                    oTargetDevice = oDevice;
                    break;
                }
            }

            if (!updateDeviceBlocked)
                updateDevice = true;
            genericUpdate();
            updateDevice = false;
        }

        private x264Settings.x264PsyTuningModes getPsyTuning()
        {
            EnumProxy o = x264Tunes.SelectedItem as EnumProxy;
            return (x264Settings.x264PsyTuningModes)o.RealValue;
        }

        private AVCLevels.Levels getAVCLevel()
        {
            EnumProxy o = avcLevel.SelectedItem as EnumProxy;
            return (AVCLevels.Levels)o.RealValue;
        }

        private void avcLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            AVCLevels.Levels avcLevel = getAVCLevel();
            if (avcLevel == AVCLevels.Levels.L_UNRESTRICTED || avcProfile.SelectedIndex < 0)
            {
                x264VBVBufferSize.Value = 0;
                x264VBVMaxRate.Value = 0;
            }
            else
            {
                AVCLevels al = new AVCLevels();
                x264VBVBufferSize.Value = al.getMaxCBP(avcLevel, avcProfile.SelectedIndex == 2);
                x264VBVMaxRate.Value = al.getMaxBR(avcLevel, avcProfile.SelectedIndex == 2);
            }
            genericUpdate();
        }

        private void x264VBVBufferSize_ValueChanged(object sender, EventArgs e)
        {
            x264VBVBufferSize.ForeColor = System.Drawing.SystemColors.WindowText;
            AVCLevels.Levels avcLevel = getAVCLevel();
            if (avcLevel != AVCLevels.Levels.L_UNRESTRICTED && avcProfile.SelectedIndex >= 0)
            {
                AVCLevels al = new AVCLevels();
                if (x264VBVBufferSize.Value <= 0 || x264VBVBufferSize.Value > al.getMaxCBP(avcLevel, avcProfile.SelectedIndex == 2))
                    x264VBVBufferSize.ForeColor = System.Drawing.Color.Red;
            }
            updateEvent(sender, e);
        }

        private void x264VBVMaxRate_ValueChanged(object sender, EventArgs e)
        {
            x264VBVMaxRate.ForeColor = System.Drawing.SystemColors.WindowText;
            AVCLevels.Levels avcLevel = getAVCLevel();
            if (avcLevel != AVCLevels.Levels.L_UNRESTRICTED && avcProfile.SelectedIndex >= 0)
            {
                AVCLevels al = new AVCLevels();
                if (x264VBVMaxRate.Value <= 0 || x264VBVMaxRate.Value > al.getMaxBR(avcLevel, avcProfile.SelectedIndex == 2))
                    x264VBVMaxRate.ForeColor = System.Drawing.Color.Red;
            }
            updateEvent(sender, e);
        }

        private void nopsy_CheckedChanged(object sender, EventArgs e)
        {
            PsyRD.Visible = PsyTrellis.Visible = lblPsyRD.Visible = lblPsyTrellis.Visible = !nopsy.Checked;
            updateEvent(sender, e);
        }

        private void ch10BitsEncoder_CheckedChanged(object sender, EventArgs e)
        {
            avcProfileGroupbox.Enabled = !ch10BitsEncoder.Checked;
            updateEvent(sender, e);
        }
    }
}