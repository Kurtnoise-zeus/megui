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
using System.Xml.Serialization;

namespace MeGUI
{
	/// <summary>
	/// Summary description for x264Settings.
	/// </summary>
	[Serializable]
	public class x264Settings: VideoCodecSettings
	{
        public static string ID = "x264";

        public static readonly x264PsyTuningModes[] SupportedPsyTuningModes = new x264PsyTuningModes[] 
        { x264PsyTuningModes.NONE, x264PsyTuningModes.ANIMATION, 
          x264PsyTuningModes.FILM, x264PsyTuningModes.GRAIN, x264PsyTuningModes.STILLIMAGE, 
          x264PsyTuningModes.PSNR, x264PsyTuningModes.SSIM 
        };

        public enum x264PsyTuningModes
        {
            [EnumTitle("None")]
            NONE,
            [EnumTitle("Animation")]
            ANIMATION,
            [EnumTitle("Film")]
            FILM,
            [EnumTitle("Grain")]
            GRAIN,
            [EnumTitle("Still Image")]
            STILLIMAGE,
            [EnumTitle("PSNR")]
            PSNR,
            [EnumTitle("SSIM")]
            SSIM
        };

        public enum x264PresetLevelModes : int 
        { 
            ultrafast = 0,
            superfast = 1,
            veryfast = 2,
            faster = 3,
            fast = 4,
            medium = 5,
            slow = 6,
            slower = 7,
            veryslow = 8,
            placebo = 9
        }

        public enum x264InterlacedModes : int
        {
            progressive = 0,
            tff = 1,
            bff = 2
        }

        public override void setAdjustedNbThreads(int nbThreads)
        {
            base.setAdjustedNbThreads(0);
        }

        public override void FixFileNames(System.Collections.Generic.Dictionary<string, string> substitutionTable)
        {
            base.FixFileNames(substitutionTable);
            if (QuantizerMatrixType == 2) // CQM
            {
                if (substitutionTable.ContainsKey(QuantizerMatrix))
                    QuantizerMatrix = substitutionTable[QuantizerMatrix];
            }
        }
        public override string[] RequiredFiles
        {
            get
            {
                List<string> list = new List<string>(base.RequiredFiles);
                if (QuantizerMatrixType == 2) // Custom profile
                    list.Add(QuantizerMatrix);
                return list.ToArray();
            }
        }
        int NewadaptiveBFrames, nbRefFrames, alphaDeblock, betaDeblock, subPelRefinement, maxQuantDelta, tempQuantBlur,
            bframePredictionMode, vbvBufferSize, vbvMaxBitrate, meType, meRange, minGOPSize, macroBlockOptions,
            quantizerMatrixType, x264Trellis, noiseReduction, deadZoneInter, deadZoneIntra, AQMode, profile,
            lookahead, slicesnb, maxSliceSyzeBytes, maxSliceSyzeMBs, bFramePyramid, weightedPPrediction, x264Nalhrd,
            colorMatrix, transfer, colorPrim, x264PullDown, sampleAR, _gopCalculation;
		decimal ipFactor, pbFactor, chromaQPOffset, vbvInitialBuffer, bitrateVariance, quantCompression,
			tempComplexityBlur, tempQuanBlurCC, scdSensitivity, bframeBias, quantizerCrf, AQStrength, psyRDO, psyTrellis;
		bool deblock, cabac, p4x4mv, p8x8mv, b8x8mv, i4x4mv, i8x8mv, weightedBPrediction, blurayCompat,
			chromaME, adaptiveDCT, noMixedRefs, noFastPSkip, psnrCalc, noDctDecimate, ssimCalc, useQPFile,
            FullRange, noMBTree, threadInput, noPsy, scenecut, x264Aud, x264SlowFirstpass, picStruct,
            fakeInterlaced, nonDeterministic, tuneFastDecode, tuneZeroLatency, stitchable, x26410Bits;
		string quantizerMatrix, qpfile, openGop, range, tcfile;
        x264PresetLevelModes preset;
        x264InterlacedModes interlacedMode;
        x264Device targetDevice;
        x264PsyTuningModes psyTuningMode;
        AVCLevels.Levels avcLevel;
        List<x264Device> x264DeviceList;
		#region constructor
        /// <summary>
		/// default constructor, initializes codec default values
		/// </summary>
		public x264Settings():base(ID, VideoEncoderType.X264)
		{
            x264DeviceList = x264Device.CreateDeviceList();
            preset = x264PresetLevelModes.medium;
            psyTuningMode = x264PsyTuningModes.NONE;
            deadZoneInter = 21;
            deadZoneIntra = 11;
			noFastPSkip = false;
            ssimCalc = false;
            psnrCalc = false;
			VideoEncodingType = VideoEncodingMode.quality;
			BitrateQuantizer = 23;
			KeyframeInterval = 250;
			nbRefFrames = 3;
			noMixedRefs = false;
			NbBframes = 3;
			deblock = true;
			alphaDeblock = 0;
			betaDeblock = 0;
			cabac = true;
			weightedBPrediction = true;
            weightedPPrediction = 2;
			NewadaptiveBFrames = 1;
			bFramePyramid = 2;
			subPelRefinement = 7;
			psyRDO = new decimal(1.0);
            psyTrellis = new decimal(0.0);
            macroBlockOptions = 3;
            chromaME = true;
			p8x8mv = true;
			b8x8mv = true;
			p4x4mv = false;
			i4x4mv = true;
			i8x8mv = true;
			MinQuantizer = 0;
			MaxQuantizer = 81;
			maxQuantDelta = 4;
			CreditsQuantizer = new decimal(40);
			ipFactor = new decimal(1.4);
			pbFactor = new decimal(1.3);
			chromaQPOffset = new decimal(0.0);
			vbvBufferSize = 0;
			vbvMaxBitrate = 0;
			vbvInitialBuffer = new decimal(0.9);
			bitrateVariance = 1;
			quantCompression = new decimal(0.6);
			tempComplexityBlur = 20;
			tempQuanBlurCC = new decimal(0.5);
			bframePredictionMode = 1;
			scdSensitivity = new decimal(40);
			bframeBias = new decimal(0);
			meType = 1;
			meRange = 16;
			NbThreads = 0;
			minGOPSize = 25;
			adaptiveDCT = true;
			quantizerMatrix = "";
			quantizerMatrixType = 0; // none
			x264Trellis = 1;
            base.MaxNumberOfPasses = 3;
            AQMode = 1;
            AQStrength = new decimal(1.0);
            useQPFile = false;
            qpfile = "";
            FullRange = false;
            range = "auto";
            lookahead = 40;
            noMBTree = true;
            threadInput = true;
            noPsy = false;
            scenecut = true;
            slicesnb = 0;
            maxSliceSyzeBytes = 0;
            maxSliceSyzeMBs = 0;
            x264Nalhrd = 0;
            x264PullDown = 0;
            sampleAR = 0;
            colorMatrix = 0;
            transfer = 0;
            colorPrim = 0;
            x264Aud = false;
            profile = 3; // Autoguess. High if using default options.
            avcLevel = AVCLevels.Levels.L_UNRESTRICTED;
            x264SlowFirstpass = false;
            openGop = "False";
            picStruct = false;
            fakeInterlaced = false;
            nonDeterministic = false;
            interlacedMode = x264InterlacedModes.progressive;
            targetDevice = x264DeviceList[0];
            blurayCompat = false;
            _gopCalculation = 1;
            quantizerCrf = 23;
            tuneFastDecode = tuneZeroLatency = false;
            stitchable = false;
            x26410Bits = false;
            tcfile = String.Empty;
		}
		#endregion
		#region properties
        public x264PresetLevelModes x264PresetLevel
        {
            get { return preset; }
            set { preset = value; }
        }
        public x264PsyTuningModes x264PsyTuning
        {
            get { return psyTuningMode; }
            set { psyTuningMode = value; }
        }
        public decimal QuantizerCRF
        {
            get { return quantizerCrf; }
            set { quantizerCrf = value; }
        }
        public x264InterlacedModes InterlacedMode
        {
            get { return interlacedMode; }
            set { interlacedMode = value; }
        }
        [XmlIgnore()]
        [MeGUI.core.plugins.interfaces.PropertyEqualityIgnoreAttribute()]
        public x264Device TargetDevice
        {
            get { return targetDevice; }
            set { targetDevice = value; }
        }
        // for profile import/export in case the enum changes
        public string TargetDeviceXML
        {
            get { return targetDevice.ID.ToString(); }
            set
            {
                if (value.Equals("1")) // device profile 1 has been replaced with 7
                    value = "7";
                else if (value.Equals("6")) // device profile 6 has been replaced with 8
                    value = "8";
                else if (value.Equals("10") || value.Equals("11")) // device profiles 10/11 have been replaced with 9
                    value = "9";

                // only support one device at the moment
                targetDevice = x264DeviceList[0];
                foreach (x264Device oDevice in x264DeviceList)
                {
                    if (oDevice.ID.ToString().Equals(value.Split(',')[0], StringComparison.CurrentCultureIgnoreCase))
                    {
                        targetDevice = oDevice;
                        break;
                    }
                }
            }
        }
        [XmlIgnore()]
        public bool BlurayCompat
        {
            get { return blurayCompat; }
            set { blurayCompat = value; }
        }
        /// <summary>
        /// Only for XML export/import. For all other purposes use BlurayCompat()
        /// </summary>
        public string BlurayCompatXML
        {
            get { return blurayCompat.ToString(); }
            set
            {
                if (value.Equals("True", StringComparison.CurrentCultureIgnoreCase))
                    blurayCompat = true;
                else
                    blurayCompat = false;
            }
        }
        public bool NoDCTDecimate
        {
            get { return noDctDecimate; }
            set { noDctDecimate = value; }
        }
        public bool PSNRCalculation
        {
            get { return psnrCalc; }
            set { psnrCalc = value; }
        }
		public bool NoFastPSkip
		{
			get { return noFastPSkip; }
			set { noFastPSkip = value; }
		}
		public int NoiseReduction
        {
            get { return noiseReduction; }
            set { noiseReduction = value; }
        }
        public bool NoMixedRefs
		{
			get { return noMixedRefs; }
			set { noMixedRefs = value; }
		}
		public int X264Trellis
		{
			get { return x264Trellis; }
			set { x264Trellis = value; }
		}
		public int NbRefFrames
		{
			get { return nbRefFrames; }
			set { nbRefFrames = value; }
		}
		public int AlphaDeblock
		{
			get { return alphaDeblock; }
			set { alphaDeblock = value; }
		}
		public int BetaDeblock
		{
			get { return betaDeblock; }
			set { betaDeblock = value; }
		}
		public int SubPelRefinement
		{
			get { return subPelRefinement; }
			set { subPelRefinement = value; }
		}
		public int MaxQuantDelta
		{
			get { return maxQuantDelta; }
			set { maxQuantDelta = value; }
		}
		public int TempQuantBlur
		{
			get { return tempQuantBlur; }
			set { tempQuantBlur = value; }
		}
		public int BframePredictionMode
		{
			get { return bframePredictionMode; }
			set { bframePredictionMode = value; }
		}
		public int VBVBufferSize
		{
			get { return vbvBufferSize; }
			set { vbvBufferSize = value; }
		}
		public int VBVMaxBitrate
		{
			get { return vbvMaxBitrate; }
			set { vbvMaxBitrate = value; }
		}
		public int METype
		{
			get { return meType; }
			set { meType = value; }
		}
		public int MERange
		{
			get { return meRange; }
			set { meRange = value; }
		}
		public int MinGOPSize
		{
			get { return minGOPSize; }
			set { minGOPSize = value; }
		}
		public decimal IPFactor
		{
			get { return ipFactor; }
			set { ipFactor = value; }
		}
		public decimal PBFactor
		{
			get { return pbFactor; }
			set { pbFactor = value; }
		}
		public decimal ChromaQPOffset
		{
			get { return chromaQPOffset; }
			set { chromaQPOffset = value; }
		}
		public decimal VBVInitialBuffer
		{
			get { return vbvInitialBuffer; }
			set { vbvInitialBuffer = value; }
		}
		public decimal BitrateVariance
		{
			get { return bitrateVariance; }
			set { bitrateVariance = value; }
		}
		public decimal QuantCompression
		{
			get { return quantCompression; }
			set { quantCompression = value; }
		}
		public decimal TempComplexityBlur
		{
			get { return tempComplexityBlur; }
			set { tempComplexityBlur = value; }
		}
		public decimal TempQuanBlurCC
		{
			get { return tempQuanBlurCC; }
			set { tempQuanBlurCC = value; }
		}
		public decimal SCDSensitivity
		{
			get { return scdSensitivity; }
			set { scdSensitivity = value; }
		}
		public decimal BframeBias
		{
			get { return bframeBias; }
			set { bframeBias = value; }
		}
        public decimal PsyRDO
        {
            get { return psyRDO; }
            set { psyRDO = value; }
        }
        public decimal PsyTrellis
        {
            get { return psyTrellis; }
            set { psyTrellis = value; }
        }
		public bool Deblock
		{
			get { return deblock; }
			set { deblock = value; }
		}
		public bool Cabac
		{
			get { return cabac; }
			set { cabac = value; }
		}
        public bool UseQPFile
        {
            get { return useQPFile; }
            set { useQPFile = value; }
        }
        public bool WeightedBPrediction
		{
			get { return weightedBPrediction; }
			set { weightedBPrediction = value; }
		}
        public int WeightedPPrediction
        {
            get { return weightedPPrediction; }
            set { weightedPPrediction = value; }
        }
		public int NewAdaptiveBFrames
		{
			get { return NewadaptiveBFrames; }
			set { NewadaptiveBFrames = value; }
		}
		public int x264BFramePyramid
		{
			get { return bFramePyramid; }
			set { bFramePyramid = value; }
		}
        public int x264GOPCalculation
        {
            get { return _gopCalculation; }
            set { _gopCalculation = value; }
        }
        public bool ChromaME
		{
			get { return chromaME; }
			set { chromaME = value; }
		}
        public int MacroBlockOptions
        {
            get { return macroBlockOptions; }
            set { macroBlockOptions = value; }
        }
        public bool P8x8mv
		{
			get { return p8x8mv; }
			set { p8x8mv = value; }
		}
		public bool B8x8mv
		{
			get { return b8x8mv; }
			set { b8x8mv = value; }
		}
		public bool I4x4mv
		{
			get { return i4x4mv; }
			set { i4x4mv = value; }
		}
		public bool I8x8mv
		{
			get { return i8x8mv; }
			set { i8x8mv = value; }
		}
		public bool P4x4mv
		{
			get { return p4x4mv; }
			set { p4x4mv = value; }
		}
		public bool AdaptiveDCT
		{
			get { return adaptiveDCT; }
			set { adaptiveDCT = value; }
		}
        public bool SSIMCalculation
        {
            get { return ssimCalc; }
            set { ssimCalc = value; }
        }
        public bool StitchAble
        {
            get { return stitchable; }
            set { stitchable = value; }
        }
		public string QuantizerMatrix
		{
			get { return quantizerMatrix; }
			set { quantizerMatrix = value; }
		}
		public int QuantizerMatrixType
		{
			get { return quantizerMatrixType; }
			set { quantizerMatrixType = value; }
		}
        public int DeadZoneInter
        {
            get { return deadZoneInter; }
            set { deadZoneInter = value; }
        }
        public int DeadZoneIntra
        {
            get { return deadZoneIntra; }
            set { deadZoneIntra = value; }
        }
        public bool X26410Bits
        {
            get { return x26410Bits; }
            set { x26410Bits = value; }
        }
        /// <summary>
        /// Only for XML export/import. For all other purposes use OpenGopValue()
        /// </summary>
        public string OpenGop
        {
            get { return openGop; }
            set 
            { 
                if (value.Equals("True", StringComparison.CurrentCultureIgnoreCase) || value.Equals("1"))
                    openGop = "True";
                else if (value.Equals("2"))
                {
                    openGop = "True";
                    blurayCompat = true;
                }
                else
                    openGop = "False";
            }
        }
        [XmlIgnore()]
        public bool OpenGopValue
        {
            get 
            {
                if (openGop.Equals("True", StringComparison.CurrentCultureIgnoreCase))
                    return true;
                else
                    return false;
            }
            set { openGop = value.ToString(); }
        }
        public int X264PullDown
        {
            get { return x264PullDown; }
            set { x264PullDown = value; }
        }
        public int SampleAR
        {
            get { return sampleAR; }
            set { sampleAR = value; }
        }
        public int ColorMatrix
        {
            get { return colorMatrix; }
            set { colorMatrix = value; }
        }
        public int ColorPrim
        {
            get { return colorPrim; }
            set { colorPrim = value; }
        }
        public int Transfer
        {
            get { return transfer; }
            set { transfer = value; }
        }
        public int AQmode
        {
            get { return AQMode; }
            set { AQMode = value; }
        }
        public decimal AQstrength
        {
            get { return AQStrength; }
            set { AQStrength = value; }
        }
        public string QPFile
        {
            get { return qpfile; }
            set { qpfile = value; }
        }
        public string TCFile
        {
            get { return tcfile; }
            set { tcfile = value; }
        }
        public string Range
        {
            get 
            {
                if (!range.Equals("pc") && !range.Equals("tv"))
                    return "auto";
                else
                    return range; 
            }
            set { range = value; }
        }
        public int Lookahead
        {
            get { return lookahead; }
            set { lookahead = value; }
        }
        public bool NoMBTree
        {
            get { return noMBTree; }
            set { noMBTree = value; }
        }
        public bool ThreadInput
        {
            get { return threadInput; }
            set { threadInput = value; }
        }
        public bool NoPsy
        {
            get { return noPsy; }
            set { noPsy = value; }
        }
        public bool Scenecut
        {
            get { return scenecut; }
            set { scenecut = value; }
        }
        public int Nalhrd
        {
            get { return x264Nalhrd; }
            set { x264Nalhrd = value; }
        }
        public bool X264Aud
        {
            get { return x264Aud; }
            set { x264Aud = value; }
        }
        public bool X264SlowFirstpass
        {
            get { return x264SlowFirstpass; }
            set { x264SlowFirstpass = value; }
        }
        public bool PicStruct
        {
            get { return picStruct; }
            set { picStruct = value; }
        }
        public bool FakeInterlaced
        {
            get { return fakeInterlaced; }
            set { fakeInterlaced = value; }
        }
        public bool NonDeterministic
        {
            get { return nonDeterministic; }
            set { nonDeterministic = value; }
        }
        public int SlicesNb
        {
            get { return slicesnb; }
            set { slicesnb = value; }
        }
        public int MaxSliceSyzeBytes
        {
            get { return maxSliceSyzeBytes; }
            set { maxSliceSyzeBytes = value; }
        }
        public int MaxSliceSyzeMBs
        {
            get { return maxSliceSyzeMBs; }
            set { maxSliceSyzeMBs = value; }
        }
        public int Profile
        {
            get { return profile; }
            set { profile = value; }
        }
        public AVCLevels.Levels AVCLevel
        {
            get { return avcLevel; }
            set { avcLevel = value; }
        }
        public bool TuneFastDecode
        {
            get { return tuneFastDecode; }
            set { tuneFastDecode = value; }
        }
        public bool TuneZeroLatency
        {
            get { return tuneZeroLatency; }
            set { tuneZeroLatency = value; }
        }
        #endregion
        public override bool UsesSAR
        {
            get { return true; }
        }
        /// <summary>
        ///  Handles assessment of whether the encoding options vary between two x264Settings instances
        /// The following are excluded from the comparison:
        /// BitrateQuantizer
        /// CreditsQuantizer
        /// Logfile
        /// NbThreads
        /// SARX
        /// SARY
        /// Zones
        /// </summary>
        /// <param name="otherSettings"></param>
        /// <returns>true if the settings differ</returns>
        public bool IsAltered(x264Settings otherSettings)
        {
            if (
                this.NewAdaptiveBFrames != otherSettings.NewAdaptiveBFrames ||
                this.AdaptiveDCT != otherSettings.AdaptiveDCT ||
                this.AlphaDeblock != otherSettings.AlphaDeblock ||
                this.NoFastPSkip != otherSettings.NoFastPSkip ||
                this.B8x8mv != otherSettings.B8x8mv ||
                this.BetaDeblock != otherSettings.BetaDeblock ||
                this.BframeBias != otherSettings.BframeBias ||
                this.BframePredictionMode != otherSettings.BframePredictionMode ||
                this.x264BFramePyramid != otherSettings.x264BFramePyramid ||
                this.x264GOPCalculation != otherSettings.x264GOPCalculation ||
                this.BitrateVariance != otherSettings.BitrateVariance ||
                this.PsyRDO != otherSettings.PsyRDO ||
                this.PsyTrellis != otherSettings.PsyTrellis ||
                this.Cabac != otherSettings.Cabac ||
                this.ChromaME != otherSettings.ChromaME ||
                this.ChromaQPOffset != otherSettings.ChromaQPOffset ||
                this.CustomEncoderOptions != otherSettings.CustomEncoderOptions ||
                this.Deblock != otherSettings.Deblock ||
                this.VideoEncodingType != otherSettings.VideoEncodingType ||
                this.I4x4mv != otherSettings.I4x4mv ||
                this.I8x8mv != otherSettings.I8x8mv ||
                this.IPFactor != otherSettings.IPFactor ||
                this.KeyframeInterval != otherSettings.KeyframeInterval ||
                this.AVCLevel != otherSettings.AVCLevel ||
                this.MaxQuantDelta != otherSettings.MaxQuantDelta ||
                this.MaxQuantizer != otherSettings.MaxQuantizer ||
                this.MERange != otherSettings.MERange ||
                this.METype != otherSettings.METype ||
                this.MinGOPSize != otherSettings.MinGOPSize ||
                this.MinQuantizer != otherSettings.MinQuantizer ||
                this.NoMixedRefs != otherSettings.NoMixedRefs ||
                this.NbBframes != otherSettings.NbBframes ||
                this.NbRefFrames != otherSettings.NbRefFrames ||
                this.noiseReduction != otherSettings.noiseReduction ||
                this.P4x4mv != otherSettings.P4x4mv ||
                this.P8x8mv != otherSettings.P8x8mv ||
                this.PBFactor != otherSettings.PBFactor ||
                this.Profile != otherSettings.Profile ||
                this.QPel != otherSettings.QPel ||
                this.QuantCompression != otherSettings.QuantCompression ||
                this.QuantizerMatrix != otherSettings.QuantizerMatrix ||
                this.QuantizerMatrixType != otherSettings.QuantizerMatrixType ||
                this.SCDSensitivity != otherSettings.SCDSensitivity ||
                this.SubPelRefinement != otherSettings.SubPelRefinement ||
                this.TempComplexityBlur != otherSettings.TempComplexityBlur ||
                this.TempQuanBlurCC != otherSettings.TempQuanBlurCC ||
                this.TempQuantBlur != otherSettings.TempQuantBlur ||
                this.Trellis != otherSettings.Trellis ||
                this.x264SlowFirstpass != otherSettings.x264SlowFirstpass ||
                this.V4MV != otherSettings.V4MV ||
                this.VBVBufferSize != otherSettings.VBVBufferSize ||
                this.VBVInitialBuffer != otherSettings.VBVInitialBuffer ||
                this.VBVMaxBitrate != otherSettings.VBVMaxBitrate ||
                this.WeightedBPrediction != otherSettings.WeightedBPrediction ||
                this.WeightedPPrediction != otherSettings.WeightedPPrediction ||
                this.X264Trellis != otherSettings.X264Trellis ||
                this.AQmode != otherSettings.AQmode ||
                this.AQstrength != otherSettings.AQstrength ||
                this.UseQPFile != otherSettings.UseQPFile ||
                this.QPFile != otherSettings.QPFile ||
                this.FullRange != otherSettings.FullRange ||
                this.Range != otherSettings.Range ||
                this.MacroBlockOptions != otherSettings.MacroBlockOptions ||
                this.x264PresetLevel != otherSettings.x264PresetLevel ||
                this.x264PsyTuning != otherSettings.x264PsyTuning ||
                this.Lookahead != otherSettings.Lookahead ||
                this.NoMBTree != otherSettings.NoMBTree ||
                this.ThreadInput != otherSettings.ThreadInput ||
                this.NoPsy != otherSettings.NoPsy ||
                this.Scenecut != otherSettings.Scenecut ||
                this.SlicesNb != otherSettings.SlicesNb ||
                this.Nalhrd != otherSettings.Nalhrd ||
                this.X264Aud != otherSettings.X264Aud ||
                this.OpenGop != otherSettings.OpenGop ||
                this.X264PullDown != otherSettings.X264PullDown ||
                this.SampleAR != otherSettings.SampleAR ||
                this.ColorMatrix != otherSettings.ColorMatrix ||
                this.Transfer != otherSettings.Transfer ||
                this.ColorPrim != otherSettings.ColorPrim ||
                this.PicStruct != otherSettings.PicStruct ||
                this.FakeInterlaced != otherSettings.FakeInterlaced ||
                this.NonDeterministic != otherSettings.NonDeterministic ||
                this.MaxSliceSyzeBytes != otherSettings.MaxSliceSyzeBytes ||
                this.InterlacedMode != otherSettings.InterlacedMode ||
                this.TargetDevice.ID != otherSettings.TargetDevice.ID ||
                this.BlurayCompat != otherSettings.BlurayCompat ||
                this.MaxSliceSyzeMBs != otherSettings.MaxSliceSyzeMBs ||
                this.tuneFastDecode != otherSettings.tuneFastDecode ||
                this.tuneZeroLatency != otherSettings.tuneZeroLatency ||
                this.X26410Bits != otherSettings.X26410Bits
                )
                return true;
            else
                return false;
        }

        public void doTriStateAdjustment()
        {
            switch (Profile)
            {
                case 0:
                    Cabac = false;
                    NbBframes = 0;
                    NewAdaptiveBFrames = 0;
                    x264BFramePyramid = 0;
                    I8x8mv = false;
                    AdaptiveDCT = false;
                    BframeBias = 0;
                    BframePredictionMode = 1; // default
                    QuantizerMatrixType = 0; // no matrix
                    QuantizerMatrix = "";
                    WeightedPPrediction = 0;
                    break;
                case 1:
                    x264BFramePyramid = 2;
                    I8x8mv = false;
                    AdaptiveDCT = false;
                    QuantizerMatrixType = 0; // no matrix
                    QuantizerMatrix = "";
                    WeightedPPrediction = 0;
                    break;
                case 2:
                    x264BFramePyramid = 2;
                    WeightedPPrediction = 1;
                    break;
            }
            if (VideoEncodingType != VideoEncodingMode.twopass1 && VideoEncodingType != VideoEncodingMode.threepass1)
                x264SlowFirstpass = false;
            if (NbBframes == 0)
            {
                NewAdaptiveBFrames = 0;
                WeightedBPrediction = false;
            }
            if (!Cabac) // trellis requires CABAC
                X264Trellis = 0;
            if (!P8x8mv) // p8x8 requires p4x4
                P4x4mv = false;
        }

        public static int GetDefaultNumberOfRefFrames(x264PresetLevelModes oPreset, x264PsyTuningModes oTuningMode, x264Device oDevice, AVCLevels.Levels avcLevel, bool blurayCompat)
        {
            return GetDefaultNumberOfRefFrames(oPreset, oTuningMode, oDevice, avcLevel, blurayCompat, -1, -1);
        }

        public static int GetDefaultNumberOfRefFrames(x264PresetLevelModes oPreset, x264PsyTuningModes oTuningMode, x264Device oDevice, AVCLevels.Levels avcLevel, bool blurayCompat, int hRes, int vRes)
        {
            int iDefaultSetting = 1;
            switch (oPreset)
            {
                case x264Settings.x264PresetLevelModes.ultrafast:
                case x264Settings.x264PresetLevelModes.superfast:
                case x264Settings.x264PresetLevelModes.veryfast:    iDefaultSetting = 1; break;
                case x264Settings.x264PresetLevelModes.faster:
                case x264Settings.x264PresetLevelModes.fast:        iDefaultSetting = 2; break;
                case x264Settings.x264PresetLevelModes.medium:      iDefaultSetting = 3; break;
                case x264Settings.x264PresetLevelModes.slow:        iDefaultSetting = 5; break;
                case x264Settings.x264PresetLevelModes.slower:      iDefaultSetting = 8; break;
                case x264Settings.x264PresetLevelModes.veryslow:
                case x264Settings.x264PresetLevelModes.placebo:     iDefaultSetting = 16; break;
            }
            if (oTuningMode == x264PsyTuningModes.ANIMATION && iDefaultSetting > 1)
                iDefaultSetting *= 2;
            if (iDefaultSetting > 16)
                iDefaultSetting = 16;
            if (blurayCompat)
                iDefaultSetting = Math.Min(6, iDefaultSetting);
            if (oDevice != null && oDevice.ReferenceFrames > -1)
                iDefaultSetting = Math.Min(oDevice.ReferenceFrames, iDefaultSetting);
            if (hRes > 0 && vRes > 0)
            {
                int iMaxRefForLevel = MeGUI.packages.video.x264.x264SettingsHandler.getMaxRefForLevel(avcLevel, hRes, vRes);
                if (iMaxRefForLevel > -1 && iMaxRefForLevel < iDefaultSetting)
                    iDefaultSetting = iMaxRefForLevel;
            }
            return iDefaultSetting;
        }

        public static int GetDefaultNumberOfBFrames(x264PresetLevelModes oPresetLevel, x264PsyTuningModes oTuningMode, bool bTuneZeroLatency, int oAVCProfile, x264Device oDevice, bool blurayCompat)
        {
            int iDefaultSetting = 0;
            if (oAVCProfile == 0) // baseline
                return iDefaultSetting;
            if (bTuneZeroLatency)
                return iDefaultSetting;

            switch (oPresetLevel)
            {
                case x264Settings.x264PresetLevelModes.ultrafast: iDefaultSetting = 0; break;
                case x264Settings.x264PresetLevelModes.superfast:
                case x264Settings.x264PresetLevelModes.veryfast:
                case x264Settings.x264PresetLevelModes.faster:
                case x264Settings.x264PresetLevelModes.fast:
                case x264Settings.x264PresetLevelModes.medium:
                case x264Settings.x264PresetLevelModes.slow:
                case x264Settings.x264PresetLevelModes.slower: iDefaultSetting = 3; break;
                case x264Settings.x264PresetLevelModes.veryslow: iDefaultSetting = 8; break;
                case x264Settings.x264PresetLevelModes.placebo: iDefaultSetting = 16; break;
            }
            if (oTuningMode == x264PsyTuningModes.ANIMATION)
                iDefaultSetting += 2;
            if (iDefaultSetting > 16)
                iDefaultSetting = 16;
            if (blurayCompat)
                iDefaultSetting = Math.Min(3, iDefaultSetting);
            if (oDevice != null && oDevice.BFrames > -1)
                return Math.Min(oDevice.BFrames, iDefaultSetting);
            else
                return iDefaultSetting;
        }

        public static int GetDefaultNumberOfWeightp(x264PresetLevelModes oPresetLevel, bool bFastDecode, int oAVCProfile, bool bBlurayCompat)
        {
            if (oAVCProfile == 0) // baseline
                return 0;
            if (bFastDecode) // Fast Decode
                return 0;

            int iDefaultSetting = 0;
            switch (oPresetLevel)
            {
                case x264Settings.x264PresetLevelModes.ultrafast:   iDefaultSetting = 0; break;
                case x264Settings.x264PresetLevelModes.superfast:   
                case x264Settings.x264PresetLevelModes.veryfast:
                case x264Settings.x264PresetLevelModes.faster:
                case x264Settings.x264PresetLevelModes.fast:        iDefaultSetting = 1; break;
                case x264Settings.x264PresetLevelModes.medium:
                case x264Settings.x264PresetLevelModes.slow:
                case x264Settings.x264PresetLevelModes.slower: 
                case x264Settings.x264PresetLevelModes.veryslow: 
                case x264Settings.x264PresetLevelModes.placebo:     iDefaultSetting = 2; break;
            }
            if (bBlurayCompat)
                return Math.Min(iDefaultSetting, 1);
            else
                return iDefaultSetting;
        }

        public static int GetDefaultAQMode(x264PresetLevelModes oPresetLevel, x264PsyTuningModes oTuningMode)
        {
            if (oTuningMode == x264PsyTuningModes.SSIM)
                return 2;

            if (oTuningMode == x264PsyTuningModes.PSNR || oPresetLevel == x264Settings.x264PresetLevelModes.ultrafast)
                return 0;

            return 1;
        }

        public static int GetDefaultRCLookahead(x264PresetLevelModes oPresetLevel, bool bTuneZeroLatency)
        {
            int iDefaultSetting = 0;
            if (bTuneZeroLatency)
                return iDefaultSetting;

            switch (oPresetLevel)
            {
                case x264Settings.x264PresetLevelModes.ultrafast:
                case x264Settings.x264PresetLevelModes.superfast:   iDefaultSetting = 0; break;
                case x264Settings.x264PresetLevelModes.veryfast:    iDefaultSetting = 10; break;
                case x264Settings.x264PresetLevelModes.faster:      iDefaultSetting = 20; break;
                case x264Settings.x264PresetLevelModes.fast:        iDefaultSetting = 30; break;
                case x264Settings.x264PresetLevelModes.medium:      iDefaultSetting = 40; break;
                case x264Settings.x264PresetLevelModes.slow:        iDefaultSetting = 50; break;
                case x264Settings.x264PresetLevelModes.slower:
                case x264Settings.x264PresetLevelModes.veryslow:
                case x264Settings.x264PresetLevelModes.placebo:     iDefaultSetting = 60; break;
            }
            return iDefaultSetting;
        }
	}
}
