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

namespace MeGUI
{
    /// <summary>
    /// this class contains all the settings for the xvid codec
    /// </summary>
    public class xvidSettings : VideoCodecSettings
    {
        public static readonly string ID = "XviD";
        public static readonly string H263Matrix = "H.263";
        public static readonly string MPEGMatrix = "MPEG";

        public override void setAdjustedNbThreads(int nbThreads)
        {
            base.setAdjustedNbThreads(0);
        }

        public bool IsCustomMatrix
        {
            get
            {
                return QuantizerMatrix != H263Matrix && QuantizerMatrix != MPEGMatrix && !string.IsNullOrEmpty(QuantizerMatrix);
            }
        }

        public override void FixFileNames(System.Collections.Generic.Dictionary<string, string> substitutionTable)
        {
            base.FixFileNames(substitutionTable);
            if (IsCustomMatrix)
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
                if (IsCustomMatrix)
                    list.Add(QuantizerMatrix);
                return list.ToArray();
            }
        }

        private int motionSearchPrecision, vhqMode, minPQuant, maxPQuant, minBQuant, maxBQuant, bQuantRatio, bQuantOffset,
                    keyFrameBoost, keyframeThreshold, keyframeReduction, overflowControlStrength,
                    maxOverflowImprovement, maxOverflowDegradation, highBitrateDegradation, lowBitrateImprovement, reactionDelayFactor, averagingPeriod,
                    rateControlBuffer, frameDropRatio, xvidProfile, vbvBuffer, vbvMaxRate, vbvPeakRate, hvsMasking;
        private bool packedBitstream, gmc, chromaMotion, closedGOP, vhqForBframes, interlaced, bottomFieldFirst, turbo;
        private decimal bframeThreshold, quantizer;
        private string customQuantizerMatrix;
        public override bool UsesSAR
        {
            get { return true; }
        }
        /// <summary>
        /// default constructor
        /// initializes all the variables at the codec's default (based on the xvid VfW defaults
        /// </summary>
        public xvidSettings()
            : base(ID, VideoEncoderType.XVID)
        {
            VideoEncodingType = VideoEncodingMode.CBR;
            quantizer = 0;
            BitrateQuantizer = 700;
            KeyframeInterval = 300;
            NbBframes = 2;
            motionSearchPrecision = 6;
            vhqMode = 1;
            MinQuantizer = 2;
            MaxQuantizer = 31;
            minPQuant = 2;
            maxPQuant = 31;
            minBQuant = 2;
            maxBQuant = 31;
            CreditsQuantizer = new decimal(20);
            bQuantRatio = 150;
            bQuantOffset = 100;
            keyFrameBoost = 10;
            keyframeThreshold = 1;
            keyframeReduction = 20;
            overflowControlStrength = 5;
            maxOverflowImprovement = 5;
            maxOverflowDegradation = 5;
            highBitrateDegradation = 0;
            lowBitrateImprovement = 0;
            reactionDelayFactor = 16;
            averagingPeriod = 100;
            rateControlBuffer = 100;
            Turbo = true;
            packedBitstream = true;
            QPel = false;
            gmc = false;
            chromaMotion = true;
            closedGOP = true;
            Trellis = true;
            bframeThreshold = new decimal(0);
            interlaced = false;
            frameDropRatio = 0;
            bottomFieldFirst = true;
            customQuantizerMatrix = "";
            xvidProfile = 0;
            vbvBuffer = 0;
            vbvMaxRate = 0;
            vbvPeakRate = 0;
            base.MaxNumberOfPasses = 2;
            hvsMasking = 0;
            NbThreads = 0;
        }
        #region properties
        public decimal Quantizer
        {
            get { return quantizer; }
            set { quantizer = value; }
        }
        public bool Turbo
        {
            get { return turbo; }
            set { turbo = value; }
        }
        public int MotionSearchPrecision
        {
            get { return motionSearchPrecision; }
            set { motionSearchPrecision = value; }
        }
        public int VHQMode
        {
            get { return vhqMode; }
            set { vhqMode = value; }
        }
        public int MinPQuant
        {
            get { return minPQuant; }
            set { minPQuant = value; }
        }
        public int MaxPQuant
        {
            get { return maxPQuant; }
            set { maxPQuant = value; }
        }
        public int MinBQuant
        {
            get { return minBQuant; }
            set { minBQuant = value; }
        }
        public int MaxBQuant
        {
            get { return maxBQuant; }
            set { maxBQuant = value; }
        }
        public int BQuantRatio
        {
            get { return bQuantRatio; }
            set { bQuantRatio = value; }
        }
        public int BQuantOffset
        {
            get { return bQuantOffset; }
            set { bQuantOffset = value; }
        }
        public int KeyFrameBoost
        {
            get { return keyFrameBoost; }
            set { keyFrameBoost = value; }
        }
        public int KeyframeThreshold
        {
            get { return keyframeThreshold; }
            set { keyframeThreshold = value; }
        }
        public int KeyframeReduction
        {
            get { return keyframeReduction; }
            set { keyframeReduction = value; }
        }
        public int OverflowControlStrength
        {
            get { return overflowControlStrength; }
            set { overflowControlStrength = value; }
        }
        public int MaxOverflowImprovement
        {
            get { return maxOverflowImprovement; }
            set { maxOverflowImprovement = value; }
        }
        public int MaxOverflowDegradation
        {
            get { return maxOverflowDegradation; }
            set { maxOverflowDegradation = value; }
        }
        public int HighBitrateDegradation
        {
            get { return highBitrateDegradation; }
            set { highBitrateDegradation = value; }
        }
        public int LowBitrateImprovement
        {
            get { return lowBitrateImprovement; }
            set { lowBitrateImprovement = value; }
        }
        public int ReactionDelayFactor
        {
            get { return reactionDelayFactor; }
            set { reactionDelayFactor = value; }
        }
        public int AveragingPeriod
        {
            get { return averagingPeriod; }
            set { averagingPeriod = value; }
        }
        public int FrameDropRatio
        {
            get { return frameDropRatio; }
            set { frameDropRatio = value; }
        }
        public int RateControlBuffer
        {
            get { return rateControlBuffer; }
            set { rateControlBuffer = value; }
        }
        public int XvidProfile
        {
            get { return xvidProfile; }
            set { xvidProfile = value; }
        }
        /// <summary>
        /// gets / sets the VBV peak bitrate
        /// </summary>
        public int VbvPeakRate
        {
            get { return vbvPeakRate; }
            set { vbvPeakRate = value; }
        }
        /// <summary>
        /// gets / sets the VBV maximum bitrate
        /// </summary>
        public int VbvMaxRate
        {
            get { return vbvMaxRate; }
            set { vbvMaxRate = value; }
        }
        /// <summary>
        /// gets / sets the VBV buffer size
        /// </summary>
        public int VbvBuffer
        {
            get { return vbvBuffer; }
            set { vbvBuffer = value; }
        }
        public bool PackedBitstream
        {
            get { return packedBitstream; }
            set { packedBitstream = value; }
        }
        public bool GMC
        {
            get { return gmc; }
            set { gmc = value; }
        }
        public bool ChromaMotion
        {
            get { return chromaMotion; }
            set { chromaMotion = value; }
        }
        public bool ClosedGOP
        {
            get { return closedGOP; }
            set { closedGOP = value; }
        }
        public bool VHQForBframes
        {
            get { return vhqForBframes; }
            set { vhqForBframes = value; }
        }
        public bool Interlaced
        {
            get { return interlaced; }
            set { interlaced = value; }
        }
        /// <summary>
        /// gets / sets whether the interlacing mode is bff
        /// if false, it's tff
        /// only active if <paramref name="Interlaced"/> is also set
        /// </summary>
        public bool BottomFieldFirst
        {
            get { return bottomFieldFirst; }
            set { bottomFieldFirst = value; }
        }
        public decimal BframeThreshold
        {
            get { return bframeThreshold; }
            set { bframeThreshold = value; }
        }
        /// <summary>
        /// gets / sets the custom quantizer matrix to be used for encoding
        /// </summary>
        public string QuantizerMatrix
        {
            get { return customQuantizerMatrix; }
            set { customQuantizerMatrix = value; }
        }
        public int HVSMasking
        {
            get 
            {
                if (hvsMasking > 2)
                    hvsMasking = 2;
                return hvsMasking; 
            }
            set { hvsMasking = value; }
        }
        #endregion

        /// <summary>
        ///  Handles assessment of whether the encoding options vary between two xvidSettings instances
        /// The following are excluded from the comparison:
        /// BitrateQuantizer
        /// CreditsQuantizer
        /// Logfile
        /// PAR
        /// PARs
        /// SARX
        /// SARY
        /// Zones
        /// </summary>
        /// <param name="otherSettings"></param>
        /// <returns>true if the settings differ</returns>
        public bool IsAltered(VideoCodecSettings settings)
        {
            if (!(settings is xvidSettings))
                return true;
            xvidSettings otherSettings = (xvidSettings)settings;
            if (
               this.AveragingPeriod != otherSettings.AveragingPeriod ||
               this.BframeThreshold != otherSettings.BframeThreshold ||
               this.BQuantOffset != otherSettings.BQuantOffset ||
               this.BQuantRatio != otherSettings.BQuantRatio ||
               this.ChromaMotion != otherSettings.ChromaMotion ||
               this.ClosedGOP != otherSettings.ClosedGOP ||
               this.CustomEncoderOptions != otherSettings.CustomEncoderOptions ||
               this.VideoEncodingType != otherSettings.VideoEncodingType ||
               this.FrameDropRatio != otherSettings.FrameDropRatio ||
               this.GMC != otherSettings.GMC ||
               this.HighBitrateDegradation != otherSettings.HighBitrateDegradation ||
               this.Interlaced != otherSettings.Interlaced ||
               this.KeyFrameBoost != otherSettings.KeyFrameBoost ||
               this.KeyframeInterval != otherSettings.KeyframeInterval ||
               this.KeyframeReduction != otherSettings.KeyframeReduction ||
               this.KeyframeThreshold != otherSettings.KeyframeThreshold ||
               this.LowBitrateImprovement != otherSettings.LowBitrateImprovement ||
               this.MaxBQuant != otherSettings.MaxBQuant ||
               this.MaxOverflowDegradation != otherSettings.MaxOverflowDegradation ||
               this.MaxOverflowImprovement != otherSettings.MaxOverflowImprovement ||
               this.MaxPQuant != otherSettings.MaxPQuant ||
               this.MaxQuantizer != otherSettings.MaxQuantizer ||
               this.MinBQuant != otherSettings.MinBQuant ||
               this.MinPQuant != otherSettings.MinPQuant ||
               this.MinQuantizer != otherSettings.MinQuantizer ||
               this.MotionSearchPrecision != otherSettings.MotionSearchPrecision ||
               this.NbBframes != otherSettings.NbBframes ||
               this.OverflowControlStrength != otherSettings.OverflowControlStrength ||
               this.PackedBitstream != otherSettings.PackedBitstream ||
               this.QPel != otherSettings.QPel ||
               this.RateControlBuffer != otherSettings.RateControlBuffer ||
               this.ReactionDelayFactor != otherSettings.ReactionDelayFactor ||
               this.Trellis != otherSettings.Trellis ||
               this.Turbo != otherSettings.Turbo ||
               this.V4MV != otherSettings.V4MV ||
               this.VHQForBframes != otherSettings.VHQForBframes ||
               this.XvidProfile != otherSettings.XvidProfile ||
               this.VbvBuffer != otherSettings.VbvBuffer ||
               this.VbvMaxRate != otherSettings.VbvMaxRate ||
               this.VbvPeakRate != otherSettings.VbvPeakRate ||
               this.VHQMode != otherSettings.VHQMode ||
               this.HVSMasking != otherSettings.HVSMasking
               )
                return true;
            else
                return false;
        }
    }
}