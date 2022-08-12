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
	/// Summary description for x265Settings.
	/// </summary>
	[Serializable]
	public class x265Settings: VideoCodecSettings
	{
        public static string ID = "x265";

        public static readonly x265PsyTuningModes[] SupportedPsyTuningModes = new x265PsyTuningModes[] 
        { x265PsyTuningModes.NONE,
          x265PsyTuningModes.PSNR,
          x265PsyTuningModes.SSIM,
          x265PsyTuningModes.ZeroLatency,
          x265PsyTuningModes.FastDecode,
          x265PsyTuningModes.Grain };

        public enum x265PsyTuningModes
        {
            [EnumTitle("None")]
            NONE,
            [EnumTitle("PSNR")]
            PSNR,
            [EnumTitle("SSIM")]
            SSIM,
            [EnumTitle("ZeroLatency")]
            ZeroLatency,
            [EnumTitle("FastDecode")]
            FastDecode,
            [EnumTitle("Grain")]
            Grain
        };

        public enum x265PresetLevelModes : int 
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

        public override void setAdjustedNbThreads(int nbThreads)
        {
            base.setAdjustedNbThreads(0);
        }

        public override void FixFileNames(System.Collections.Generic.Dictionary<string, string> substitutionTable)
        {
            base.FixFileNames(substitutionTable);
        }

        int NewadaptiveBFrames, nbRefFrames, alphaDeblock, betaDeblock, subPelRefinement, maxQuantDelta, tempQuantBlur,
            bframePredictionMode, vbvBufferSize, vbvMaxBitrate, meType, meRange, minGOPSize, macroBlockOptions,
            quantizerMatrixType, x265Trellis, noiseReduction, deadZoneInter, deadZoneIntra, AQMode, profile,
            lookahead, slicesnb, maxSliceSyzeBytes, maxSliceSyzeMBs, bFramePyramid, weightedPPrediction, x265Nalhrd,
            colorMatrix, transfer, colorPrim, sampleAR, _gopCalculation;
		decimal ipFactor, pbFactor, chromaQPOffset, vbvInitialBuffer, bitrateVariance, quantCompression,
			tempComplexityBlur, tempQuanBlurCC, scdSensitivity, bframeBias, quantizerCrf, AQStrength, psyRDO, psyTrellis;
		bool deblock, cabac, p4x4mv, p8x8mv, b8x8mv, i4x4mv, i8x8mv, weightedBPrediction, 
			chromaME, adaptiveDCT, noMixedRefs, noFastPSkip, psnrCalc, noDctDecimate, ssimCalc, useQPFile,
            FullRange, advSet, noMBTree, threadInput, noPsy, scenecut, x265SlowFirstpass, picStruct,
            fakeInterlaced, nonDeterministic, tuneFastDecode, tuneZeroLatency, stitchable;
		string quantizerMatrix, qpfile, openGop, range;
        x265PresetLevelModes preset;
        x265PsyTuningModes psyTuningMode;
        AVCLevels.Levels avcLevel;
		#region constructor
        /// <summary>
		/// default constructor, initializes codec default values
		/// </summary>
		public x265Settings():base(ID, VideoEncoderType.X265)
		{
            preset = x265PresetLevelModes.medium;
            psyTuningMode = x265PsyTuningModes.NONE;
            deadZoneInter = 21;
            deadZoneIntra = 11;
			noFastPSkip = false;
            ssimCalc = false;
            psnrCalc = false;
			VideoEncodingType = VideoEncodingMode.quality;
			BitrateQuantizer = 28;
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
			MaxQuantizer = 69;
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
            base.MaxNumberOfPasses = 3;
            AQMode = 1;
            AQStrength = new decimal(1.0);
            useQPFile = false;
            qpfile = "";
            FullRange = false;
            range = "auto";
            advSet = false;
            lookahead = 40;
            noMBTree = true;
            threadInput = true;
            noPsy = false;
            scenecut = true;
            slicesnb = 0;
            maxSliceSyzeBytes = 0;
            maxSliceSyzeMBs = 0;
            x265Nalhrd = 0;
            sampleAR = 0;
            colorMatrix = 0;
            transfer = 0;
            colorPrim = 0;
            profile = 3; // Autoguess. High if using default options.
            avcLevel = AVCLevels.Levels.L_UNRESTRICTED;
            openGop = "False";
            picStruct = false;
            fakeInterlaced = false;
            nonDeterministic = false;
            _gopCalculation = 1;
            quantizerCrf = 28;
            tuneFastDecode = tuneZeroLatency = false;
            stitchable = false;
		}
		#endregion
		#region properties
        public x265PresetLevelModes x265PresetLevel
        {
            get { return preset; }
            set { preset = value; }
        }

        public x265PsyTuningModes x265PsyTuning
        {
            get { return psyTuningMode; }
            set { psyTuningMode = value; }
        }

        public decimal QuantizerCRF
        {
            get { return quantizerCrf; }
            set { quantizerCrf = value; }
        }

        [XmlIgnore()]
        [MeGUI.core.plugins.interfaces.PropertyEqualityIgnoreAttribute()]
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
		public int x265BFramePyramid
		{
			get { return bFramePyramid; }
			set { bFramePyramid = value; }
		}
        public int x265GOPCalculation
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
        public bool x265AdvancedSettings
        {
            get { return advSet; }
            set { advSet = value; }
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
            get { return x265Nalhrd; }
            set { x265Nalhrd = value; }
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

        public string Level
        {
            get { return "migrated"; }
            set
            {
                if (value.Equals("migrated"))
                    return;

                if (value.Equals("0"))
                    avcLevel = AVCLevels.Levels.L_10;
                if (value.Equals("1"))
                    avcLevel = AVCLevels.Levels.L_11;
                if (value.Equals("2"))
                    avcLevel = AVCLevels.Levels.L_12;
                if (value.Equals("3"))
                    avcLevel = AVCLevels.Levels.L_13;
                if (value.Equals("4"))
                    avcLevel = AVCLevels.Levels.L_20;
                if (value.Equals("5"))
                    avcLevel = AVCLevels.Levels.L_21;
                if (value.Equals("6"))
                    avcLevel = AVCLevels.Levels.L_22;
                if (value.Equals("7"))
                    avcLevel = AVCLevels.Levels.L_30;
                if (value.Equals("8"))
                    avcLevel = AVCLevels.Levels.L_31;
                if (value.Equals("9"))
                    avcLevel = AVCLevels.Levels.L_32;
                if (value.Equals("10"))
                    avcLevel = AVCLevels.Levels.L_40;
                if (value.Equals("11"))
                    avcLevel = AVCLevels.Levels.L_41;
                if (value.Equals("12"))
                    avcLevel = AVCLevels.Levels.L_42;
                if (value.Equals("13"))
                    avcLevel = AVCLevels.Levels.L_50;
                if (value.Equals("14"))
                    avcLevel = AVCLevels.Levels.L_51;
                if (value.Equals("15"))
                    avcLevel = AVCLevels.Levels.L_UNRESTRICTED;
            }
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
        ///  Handles assessment of whether the encoding options vary between two x265Settings instances
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
        public bool IsAltered(x265Settings otherSettings)
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
                this.x265BFramePyramid != otherSettings.x265BFramePyramid ||
                this.x265GOPCalculation != otherSettings.x265GOPCalculation ||
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
                this.x265SlowFirstpass != otherSettings.x265SlowFirstpass ||
                this.V4MV != otherSettings.V4MV ||
                this.VBVBufferSize != otherSettings.VBVBufferSize ||
                this.VBVInitialBuffer != otherSettings.VBVInitialBuffer ||
                this.VBVMaxBitrate != otherSettings.VBVMaxBitrate ||
                this.WeightedBPrediction != otherSettings.WeightedBPrediction ||
                this.WeightedPPrediction != otherSettings.WeightedPPrediction ||
                this.x265Trellis != otherSettings.x265Trellis ||
                this.AQmode != otherSettings.AQmode ||
                this.AQstrength != otherSettings.AQstrength ||
                this.UseQPFile != otherSettings.UseQPFile ||
                this.QPFile != otherSettings.QPFile ||
                this.FullRange != otherSettings.FullRange ||
                this.Range != otherSettings.Range ||
                this.MacroBlockOptions != otherSettings.MacroBlockOptions ||
                this.x265PresetLevel != otherSettings.x265PresetLevel ||
                this.x265PsyTuning != otherSettings.x265PsyTuning ||
                this.x265AdvancedSettings != otherSettings.x265AdvancedSettings ||
                this.Lookahead != otherSettings.Lookahead ||
                this.NoMBTree != otherSettings.NoMBTree ||
                this.ThreadInput != otherSettings.ThreadInput ||
                this.NoPsy != otherSettings.NoPsy ||
                this.Scenecut != otherSettings.Scenecut ||
                this.SlicesNb != otherSettings.SlicesNb ||
                this.Nalhrd != otherSettings.Nalhrd ||
                this.OpenGop != otherSettings.OpenGop ||
                this.SampleAR != otherSettings.SampleAR ||
                this.ColorMatrix != otherSettings.ColorMatrix ||
                this.Transfer != otherSettings.Transfer ||
                this.ColorPrim != otherSettings.ColorPrim ||
                this.PicStruct != otherSettings.PicStruct ||
                this.FakeInterlaced != otherSettings.FakeInterlaced ||
                this.NonDeterministic != otherSettings.NonDeterministic ||
                this.MaxSliceSyzeBytes != otherSettings.MaxSliceSyzeBytes ||
                this.MaxSliceSyzeMBs != otherSettings.MaxSliceSyzeMBs ||
                this.tuneFastDecode != otherSettings.tuneFastDecode ||
                this.tuneZeroLatency != otherSettings.tuneZeroLatency
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
                    x265BFramePyramid = 0;
                    I8x8mv = false;
                    AdaptiveDCT = false;
                    BframeBias = 0;
                    BframePredictionMode = 1; // default
                    QuantizerMatrixType = 0; // no matrix
                    QuantizerMatrix = "";
                    WeightedPPrediction = 0;
                    break;
                case 1:
                    x265BFramePyramid = 2;
                    I8x8mv = false;
                    AdaptiveDCT = false;
                    QuantizerMatrixType = 0; // no matrix
                    QuantizerMatrix = "";
                    WeightedPPrediction = 0;
                    break;
                case 2:
                    x265BFramePyramid = 2;
                    WeightedPPrediction = 1;
                    break;
            }
            if (VideoEncodingType != VideoEncodingMode.twopass1 && VideoEncodingType != VideoEncodingMode.threepass1)
                x265SlowFirstpass = false;
            if (NbBframes == 0)
            {
                NewAdaptiveBFrames = 0;
                WeightedBPrediction = false;
            }
            if (!Cabac) // trellis requires CABAC
                x265Trellis = 0;
            if (!P8x8mv) // p8x8 requires p4x4
                P4x4mv = false;
        }

        public static int GetDefaultNumberOfRefFrames(x265PresetLevelModes oPreset, x265PsyTuningModes oTuningMode, bool blurayCompat)
        {
            return GetDefaultNumberOfRefFrames(oPreset, oTuningMode, blurayCompat, -1, -1);
        }

        public static int GetDefaultNumberOfRefFrames(x265PresetLevelModes oPreset, x265PsyTuningModes oTuningMode, bool blurayCompat, int hRes, int vRes)
        {
            int iDefaultSetting = 1;
            switch (oPreset)
            {
                case x265Settings.x265PresetLevelModes.ultrafast:
                case x265Settings.x265PresetLevelModes.superfast:
                case x265Settings.x265PresetLevelModes.veryfast:    iDefaultSetting = 1; break;
                case x265Settings.x265PresetLevelModes.faster:
                case x265Settings.x265PresetLevelModes.fast:        iDefaultSetting = 2; break;
                case x265Settings.x265PresetLevelModes.medium:      iDefaultSetting = 3; break;
                case x265Settings.x265PresetLevelModes.slow:        iDefaultSetting = 5; break;
                case x265Settings.x265PresetLevelModes.slower:      iDefaultSetting = 8; break;
                case x265Settings.x265PresetLevelModes.veryslow:
                case x265Settings.x265PresetLevelModes.placebo:     iDefaultSetting = 16; break;
            }
            if (iDefaultSetting > 16)
                iDefaultSetting = 16;
            if (blurayCompat)
                iDefaultSetting = Math.Min(6, iDefaultSetting);
            /*if (hRes > 0 && vRes > 0)
            {
                int iMaxRefForLevel = MeGUI.packages.video.x265.x265SettingsHandler.getMaxRefForLevel(avcLevel, hRes, vRes);
                if (iMaxRefForLevel > -1 && iMaxRefForLevel < iDefaultSetting)
                    iDefaultSetting = iMaxRefForLevel;
            }*/
            return iDefaultSetting;
        }

        public static int GetDefaultNumberOfBFrames(x265PresetLevelModes oPresetLevel, x265PsyTuningModes oTuningMode, bool bTuneZeroLatency, bool blurayCompat)
        {
            int iDefaultSetting = 0;

            if (bTuneZeroLatency)
                return iDefaultSetting;

            switch (oPresetLevel)
            {
                case x265Settings.x265PresetLevelModes.ultrafast: iDefaultSetting = 0; break;
                case x265Settings.x265PresetLevelModes.superfast:
                case x265Settings.x265PresetLevelModes.veryfast:
                case x265Settings.x265PresetLevelModes.faster:
                case x265Settings.x265PresetLevelModes.fast:
                case x265Settings.x265PresetLevelModes.medium:
                case x265Settings.x265PresetLevelModes.slow:
                case x265Settings.x265PresetLevelModes.slower: iDefaultSetting = 3; break;
                case x265Settings.x265PresetLevelModes.veryslow: iDefaultSetting = 8; break;
                case x265Settings.x265PresetLevelModes.placebo: iDefaultSetting = 16; break;
            }

            if (iDefaultSetting > 16)
                iDefaultSetting = 16;
            return iDefaultSetting;
        }

        public static int GetDefaultNumberOfWeightp(x265PresetLevelModes oPresetLevel, bool bFastDecode, bool bBlurayCompat)
        {
            if (bFastDecode) // Fast Decode
                return 0;

            int iDefaultSetting = 0;
            switch (oPresetLevel)
            {
                case x265Settings.x265PresetLevelModes.ultrafast:   iDefaultSetting = 0; break;
                case x265Settings.x265PresetLevelModes.superfast:   
                case x265Settings.x265PresetLevelModes.veryfast:
                case x265Settings.x265PresetLevelModes.faster:
                case x265Settings.x265PresetLevelModes.fast:        iDefaultSetting = 1; break;
                case x265Settings.x265PresetLevelModes.medium:
                case x265Settings.x265PresetLevelModes.slow:
                case x265Settings.x265PresetLevelModes.slower: 
                case x265Settings.x265PresetLevelModes.veryslow: 
                case x265Settings.x265PresetLevelModes.placebo:     iDefaultSetting = 2; break;
            }
            if (bBlurayCompat)
                return Math.Min(iDefaultSetting, 1);
            else
                return iDefaultSetting;
        }

        public static int GetDefaultAQMode(x265PresetLevelModes oPresetLevel, x265PsyTuningModes oTuningMode)
        {
            if (oTuningMode == x265PsyTuningModes.SSIM)
                return 2;

            if (oTuningMode == x265PsyTuningModes.PSNR || oPresetLevel == x265Settings.x265PresetLevelModes.ultrafast)
                return 0;

            return 1;
        }

        public static int GetDefaultRCLookahead(x265PresetLevelModes oPresetLevel, bool bTuneZeroLatency)
        {
            int iDefaultSetting = 0;
            if (bTuneZeroLatency)
                return iDefaultSetting;

            switch (oPresetLevel)
            {
                case x265Settings.x265PresetLevelModes.ultrafast:
                case x265Settings.x265PresetLevelModes.superfast:   iDefaultSetting = 0; break;
                case x265Settings.x265PresetLevelModes.veryfast:    iDefaultSetting = 10; break;
                case x265Settings.x265PresetLevelModes.faster:      iDefaultSetting = 20; break;
                case x265Settings.x265PresetLevelModes.fast:        iDefaultSetting = 30; break;
                case x265Settings.x265PresetLevelModes.medium:      iDefaultSetting = 40; break;
                case x265Settings.x265PresetLevelModes.slow:        iDefaultSetting = 50; break;
                case x265Settings.x265PresetLevelModes.slower:
                case x265Settings.x265PresetLevelModes.veryslow:
                case x265Settings.x265PresetLevelModes.placebo:     iDefaultSetting = 60; break;
            }
            return iDefaultSetting;
        }
	}
}
