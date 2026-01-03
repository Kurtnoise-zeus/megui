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
using System.Xml.Serialization;

namespace MeGUI
{
	/// <summary>
	/// Summary description for ffv1Settings.
	/// </summary>
	[Serializable]
	public class ffv1Settings: VideoCodecSettings
	{
        public static string ID = "FFV1";

        public override void setAdjustedNbThreads(int nbThreads)
        {
            base.setAdjustedNbThreads(0);
        }

        public override void FixFileNames(System.Collections.Generic.Dictionary<string, string> substitutionTable)
        {
            base.FixFileNames(substitutionTable);
        }

        int coder, context, gopsize, slicesnb, nbThreads;
		decimal quantizerCrf;
		bool errorCorrection, ffv110Bits;

		#region constructor
        /// <summary>
		/// default constructor, initializes codec default values
		/// </summary>
		public ffv1Settings():base(ID, VideoEncoderType.FFV1)
		{
            coder = 1;
            context = 1;
            gopsize = 1;
            nbThreads = 1;
            errorCorrection = true;
            slicesnb = 1;
            quantizerCrf = 28;
            ffv110Bits = false;
            FFV1EncodingType = FFV1EncodingMode.none;
            base.MaxNumberOfPasses = 2;
        }
        #endregion
        #region properties

        [XmlIgnore()]
        [MeGUI.core.plugins.interfaces.PropertyEqualityIgnoreAttribute()]
        public int Coder
        {
            get { return coder; }
            set { coder = value; }
        }
        public int Context
		{
			get { return context; }
			set { context = value; }
		}
		public int GOPSize
		{
			get { return gopsize; }
			set { gopsize = value; }
		}
        public int NBThreads
        {
            get { return nbThreads; }
            set { nbThreads = value; }
        }
        public int Slices
        {
            get { return slicesnb; }
            set { slicesnb = value; }
        }
        public bool ErrorCorrection
        {
            get { return errorCorrection; }
            set { errorCorrection = value; }
        }
        public bool FFV110Bits
        {
            get { return ffv110Bits; }
            set { ffv110Bits = value; }
        }
        #endregion
        public override bool UsesSAR
        {
            get { return true; }
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
        public bool IsAltered(ffv1Settings otherSettings)
        {
            if (
                this.FFV1EncodingType != otherSettings.FFV1EncodingType
                )
                return true;
            else
                return false;
        }

	}
}
