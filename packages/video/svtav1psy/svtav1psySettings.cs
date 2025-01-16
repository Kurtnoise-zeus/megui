// ****************************************************************************
// 
// Copyright (C) 2005-2025 Doom9 & al
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
using static MeGUI.svtav1psySettings;

namespace MeGUI
{
	/// <summary>
	/// Summary description for svtav1psySettings.
	/// </summary>
	[Serializable]
	public class svtav1psySettings : VideoCodecSettings
	{
        public static string ID = "svt-av1-psy";

        public static readonly svtAv1PsyTuningModes[] SupportedPsyTuningModes = new svtAv1PsyTuningModes[]
        { svtAv1PsyTuningModes.NONE,
          svtAv1PsyTuningModes.VQ,
          svtAv1PsyTuningModes.PSNR,
          svtAv1PsyTuningModes.SSIM,
          svtAv1PsyTuningModes.SUBJECTIVESSIM,
          svtAv1PsyTuningModes.STILLPICTURE
        };

        public enum svtAv1PsyTuningModes
        {
            [EnumTitle("None")]
            NONE,
            [EnumTitle("VQ")]
            VQ,
            [EnumTitle("PSNR")]
            PSNR,
            [EnumTitle("SSIM")]
            SSIM,
            [EnumTitle("Subjective SSIM")]
            SUBJECTIVESSIM,
            [EnumTitle("Still Picture")]
            STILLPICTURE
        };

        public override void setAdjustedNbThreads(int nbThreads)
        {
            base.setAdjustedNbThreads(0);
        }

        public override void FixFileNames(System.Collections.Generic.Dictionary<string, string> substitutionTable)
        {
            base.FixFileNames(substitutionTable);
        }

        bool svt10Bits;
        int preset;
		decimal quantizerCrf;
        svtAv1PsyTuningModes psyTuningMode;


        #region constructor
        /// <summary>
        /// default constructor, initializes codec default values
        /// </summary>
        public svtav1psySettings():base(ID, VideoEncoderType.SVTAV1PSY)
		{
            psyTuningMode = svtAv1PsyTuningModes.NONE;
            quantizerCrf = 35;
            VideoEncodingType = VideoEncodingMode.quality;
            base.MaxNumberOfPasses = 2;
            preset = 10;
            svt10Bits = false;
        }
        #endregion
        #region properties
        public svtAv1PsyTuningModes svtAv1PsyTuning
        {
            get { return psyTuningMode; }
            set { psyTuningMode = value; }
        }

        public decimal QuantizerCRF
        {
            get { return quantizerCrf; }
            set { quantizerCrf = value; }
        }
        public int Preset
        {
            get { return preset; }
            set { preset = value; }
        }

        #endregion
        public override bool UsesSAR
        {
            get { return true; }
        }

        public bool SVT10Bits
        {
            get { return svt10Bits; }
            set { svt10Bits = value; }
        }
        /// <summary>
        ///  Handles assessment of whether the encoding options vary between two ffv1Settings instances
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
        public bool IsAltered(svtav1psySettings otherSettings)
        {
            if (
                this.FFV1EncodingType != otherSettings.FFV1EncodingType ||
                this.svtAv1PsyTuning != otherSettings.svtAv1PsyTuning
                )
                return true;
            else
                return false;
        }

	}
}
