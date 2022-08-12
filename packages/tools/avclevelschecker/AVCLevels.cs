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
using System.Windows.Forms;

namespace MeGUI
{
    /// <summary>
	/// Summary description for AVCLevels.
    /// akupenguin http://forum.doom9.org/showthread.php?p=730001#post730001
    /// These are the properties listed in the levels tables in the standard, and how they should limit x264 settings:
    /// MaxMBPS >= width*height*fps. (w&h measured in macroblocks, i.e. pixels/16 round up in each dimension)
    /// MaxFS >= width*height
    /// sqrt(MaxFS*8) >= width
    /// sqrt(MaxFS*8) >= height
    /// MaxDPB >= (bytes in a frame) * min(16, ref)
    /// MaxBR >= vbv_maxrate. It isn't strictly required since we don't write the VCL HRD parameters, but this satisfies the intent.
    /// MaxCPB >= vbv_bufsize. Likewise.
    /// MaxVmvR >= max_mv_range. (Not exposed in the cli, I'll add it if people care.)
    /// MaxMvsPer2Mb, MinLumaBiPredSize, direct_8x8_inference_flag : are not enforced by x264. The only way to ensure compliance is to disable p4x4 at level>=3.1, or at level>=3 w/ B-frames.
    /// MinCR : is not enforced by x264. Won't ever be an issue unless you use lossless.
    /// SliceRate : I don't know what this limits.
	/// </summary>
	public class AVCLevels
    {
        public enum Levels
        {
            [EnumTitle("Level 1")]
            L_10,
            [EnumTitle("Level 1b")]
            L_1B,
            [EnumTitle("Level 1.1")]
            L_11,
            [EnumTitle("Level 1.2")]
            L_12,
            [EnumTitle("Level 1.3")]
            L_13,
            [EnumTitle("Level 2")]
            L_20,
            [EnumTitle("Level 2.1")]
            L_21,
            [EnumTitle("Level 2.2")]
            L_22,
            [EnumTitle("Level 3")]
            L_30,
            [EnumTitle("Level 3.1")]
            L_31,
            [EnumTitle("Level 3.2")]
            L_32,
            [EnumTitle("Level 4")]
            L_40,
            [EnumTitle("Level 4.1")]
            L_41,
            [EnumTitle("Level 4.2")]
            L_42,
            [EnumTitle("Level 5")]
            L_50,
            [EnumTitle("Level 5.1")]
            L_51,
            [EnumTitle("Level 5.2")]
            L_52,
            [EnumTitle("Level 6")]
            L_60,
            [EnumTitle("Level 6.1")]
            L_61,
            [EnumTitle("Level 6.2")]
            L_62,
            [EnumTitle("Unrestricted/Autoguess")]
            L_UNRESTRICTED
        };

        public static readonly Levels[] SupportedLevels = new Levels[] { Levels.L_10, Levels.L_1B, Levels.L_11, 
            Levels.L_12, Levels.L_13, Levels.L_20, Levels.L_21, Levels.L_22, Levels.L_30, Levels.L_31, Levels.L_32, 
            Levels.L_40, Levels.L_41, Levels.L_42, Levels.L_50, Levels.L_51, Levels.L_52,
            Levels.L_60, Levels.L_61, Levels.L_62, Levels.L_UNRESTRICTED };

        /// <summary>
        /// gets the level number as text
        /// </summary>
        /// <param name="avcLevel">the level</param>
        /// <returns>the level number</returns>
        public static string GetLevelText(Levels avcLevel)
        {
            switch (avcLevel)
            {
                case Levels.L_10: return "1.0";
                case Levels.L_1B: return "1b";
                case Levels.L_11: return "1.1";
                case Levels.L_12: return "1.2";
                case Levels.L_13: return "1.3";
                case Levels.L_20: return "2.0";
                case Levels.L_21: return "2.1";
                case Levels.L_22: return "2.2";
                case Levels.L_30: return "3.0";
                case Levels.L_31: return "3.1";
                case Levels.L_32: return "3.2";
                case Levels.L_40: return "4.0";
                case Levels.L_41: return "4.1";
                case Levels.L_42: return "4.2";
                case Levels.L_50: return "5.0";
                case Levels.L_51: return "5.1";
                case Levels.L_52: return "5.2";
                case Levels.L_60: return "6.0";
                case Levels.L_61: return "6.1";
                case Levels.L_62: return "6.2";
                default: return "5.2";
            }
        }

        #region internal logic and calculation routines for level verification
        private double pictureBufferSize(x264Settings settings, double bytesInUncompressedFrame)
        {
            double decodedPictureBufferSizeTestValue = 0;
            if (settings != null)
                decodedPictureBufferSizeTestValue = bytesInUncompressedFrame * Math.Min(16, settings.NbRefFrames);
            return decodedPictureBufferSizeTestValue;
        }

        private int macroblocks(int res)
        {
            int blocks;
            if (res % 16 == 0)
                blocks = res / 16;
            else
            {
                int remainder;
                blocks = Math.DivRem(res, 16, out remainder);
                blocks++;
            }
            return blocks;
        }

        private double maxFS(int hRes, int vRes)
        {
            int horizontalBlocks, verticalBlocks;
            if (hRes % 16 == 0)
                horizontalBlocks = hRes / 16;
            else
            {
                int remainder;
                horizontalBlocks = Math.DivRem(hRes, 16, out remainder);
                horizontalBlocks++;
            }
            if (vRes % 16 == 0)
                verticalBlocks = vRes / 16;
            else
            {
                int remainder;
                verticalBlocks = Math.DivRem(vRes, 16, out remainder);
                verticalBlocks++;
            }
            return (double)horizontalBlocks * (double)verticalBlocks;
        }

        private int maxBPS(int hres, int vres, double framerate)
        {
            return (int)(maxFS(hres, vres) * framerate);
        }
        #endregion
        #region public calculation utilities
        public double bytesPerFrame(int hres, int vres)
        {
            return hres * vres * 1.5;
        }
        #endregion
        #region constructor
        public AVCLevels()
		{
			//
			// TODO: Add constructor logic here
			//
        }
        #endregion
        #region public look-up routines
		/// <summary>
		/// gets the MaxCBP value corresponding to a given AVC Level
		/// </summary>
		/// <param name="level">the level</param>
		/// <returns>the MaxCBP in kbit/s</returns>
		public int getMaxCBP(Levels avcLevel, bool isHighProfile)
		{
            int maxCBP = 0;
            switch (avcLevel)
            {
                case Levels.L_10: maxCBP = 175;    break;
                case Levels.L_1B: maxCBP = 350;    break;
                case Levels.L_11: maxCBP = 500;    break;
                case Levels.L_12: maxCBP = 1000;   break;
                case Levels.L_13: maxCBP = 2000;   break;
                case Levels.L_20: maxCBP = 2000;   break;
                case Levels.L_21: maxCBP = 4000;   break;
                case Levels.L_22: maxCBP = 4000;   break;
                case Levels.L_30: maxCBP = 10000;  break;
                case Levels.L_31: maxCBP = 14000;  break;
                case Levels.L_32: maxCBP = 20000;  break;
                case Levels.L_40: maxCBP = 25000;  break;
                case Levels.L_41: maxCBP = 62500;  break;
                case Levels.L_42: maxCBP = 62500;  break;
                case Levels.L_50: maxCBP = 135000; break;
                case Levels.L_51: maxCBP = 240000; break;
                case Levels.L_52: maxCBP = 240000; break;
                case Levels.L_60: maxCBP = 240000; break;
                case Levels.L_61: maxCBP = 480000; break;
                case Levels.L_62: maxCBP = 800000; break;
            }

            if (isHighProfile) // all bitrates and CBPs are multiplied by 1.25 in high profile
                maxCBP = maxCBP * 125 / 100;

            return maxCBP;
        }

		/// <summary>
		/// gets the maxBR rate in bits for a given level
		/// </summary>
		/// <param name="level">the level</param>
		/// <returns>the MaxBR in bits</returns>
		public int getMaxBR(Levels avcLevel, bool isHighProfile)
		{
            int maxBR = 0;
            switch (avcLevel)
            {
                case Levels.L_10: maxBR = 64;     break;
                case Levels.L_1B: maxBR = 128;    break;
                case Levels.L_11: maxBR = 192;    break;
                case Levels.L_12: maxBR = 384;    break;
                case Levels.L_13: maxBR = 768;    break;
                case Levels.L_20: maxBR = 2000;   break;
                case Levels.L_21: maxBR = 4000;   break;
                case Levels.L_22: maxBR = 4000;   break;
                case Levels.L_30: maxBR = 10000;  break;
                case Levels.L_31: maxBR = 14000;  break;
                case Levels.L_32: maxBR = 20000;  break;
                case Levels.L_40: maxBR = 20000;  break;
                case Levels.L_41: maxBR = 50000;  break;
                case Levels.L_42: maxBR = 50000;  break;
                case Levels.L_50: maxBR = 135000; break;
                case Levels.L_51: maxBR = 240000; break;
                case Levels.L_52: maxBR = 240000; break;
                case Levels.L_60: maxBR = 240000; break;
                case Levels.L_61: maxBR = 480000; break;
                case Levels.L_62: maxBR = 800000; break;
            }

            if (isHighProfile) // all bitrates and cbps are multiplied by 1.25 in high profile
                maxBR = maxBR * 125 / 100;

            return maxBR;
		}

		/// <summary>
		/// gets the Maximum macroblock rate given a level
		/// </summary>
		/// <param name="level">the level</param>
		/// <returns>the macroblock rate</returns>
		public int getMaxMBPS(Levels avcLevel)
		{
			switch (avcLevel)
			{
				case Levels.L_10: return 1485;
                case Levels.L_1B: return 1485;
				case Levels.L_11: return 3000;
                case Levels.L_12: return 6000;
                case Levels.L_13: return 11880;
                case Levels.L_20: return 11880;
                case Levels.L_21: return 19800;
                case Levels.L_22: return 20250;
                case Levels.L_30: return 40500;
                case Levels.L_31: return 108000;
                case Levels.L_32: return 216000;
                case Levels.L_40: return 245760;
                case Levels.L_41: return 245760;
                case Levels.L_42: return 491520;
                case Levels.L_50: return 589824;
                case Levels.L_51: return 983040;
                case Levels.L_52: return 2073600;
                case Levels.L_60: return 4177920;
                case Levels.L_61: return 8355840;
                case Levels.L_62: return 16711680;
                default: return 2073600; // level 5.2
			}
		}

		/// <summary>
		/// gets the maximum framesize given a level
		/// </summary>
		/// <param name="level">the level</param>
		/// <returns>the maximum framesize in number of macroblocks
		/// (1MB = 16x16)</returns>
		public int getMaxFS(Levels avcLevel)
		{
			switch (avcLevel)
			{
				case Levels.L_10: return 99;
                case Levels.L_1B: return 99;
				case Levels.L_11: return 396;
				case Levels.L_12: return 396;
                case Levels.L_13: return 396;
                case Levels.L_20: return 396;
                case Levels.L_21: return 792;
                case Levels.L_22: return 1620;
                case Levels.L_30: return 1620;
                case Levels.L_31: return 3600;
                case Levels.L_32: return 5120;
                case Levels.L_40: return 8192;
                case Levels.L_41: return 8192;
                case Levels.L_42: return 8192;
                case Levels.L_50: return 22080;
                case Levels.L_51: return 36864;
                case Levels.L_52: return 36864;
                case Levels.L_60: return 139264;
                case Levels.L_61: return 139264;
                case Levels.L_62: return 139264;
                default: return 36864; // level 5.2
			}
		}

		/// <summary>
		/// gets the maximum picture decoded buffer for the given level 
		/// </summary>
		/// <param name="level">the level</param>
		/// <returns>the size of the decoding buffer in bytes</returns>
		public double getMaxDPB(Levels avcLevel)
		{
			double maxDPB = 184320; // level 5.2
			switch (avcLevel)
			{
				case Levels.L_10: maxDPB = 396; break;
                case Levels.L_1B: maxDPB = 396; break;
				case Levels.L_11: maxDPB = 900; break;
                case Levels.L_12: maxDPB = 2376; break;
                case Levels.L_13: maxDPB = 2376; break;
                case Levels.L_20: maxDPB = 2376; break;
                case Levels.L_21: maxDPB = 4752; break;
                case Levels.L_22: maxDPB = 8100; break;
                case Levels.L_30: maxDPB = 8100; break;
                case Levels.L_31: maxDPB = 18000; break;
                case Levels.L_32: maxDPB = 20480; break;
                case Levels.L_40: maxDPB = 32768; break;
                case Levels.L_41: maxDPB = 32768; break;
                case Levels.L_42: maxDPB = 34816; break;
                case Levels.L_50: maxDPB = 110400; break;
                case Levels.L_51: maxDPB = 184320; break;
                case Levels.L_52: maxDPB = 184320; break;
                case Levels.L_60: maxDPB = 696320; break;
                case Levels.L_61: maxDPB = 696320; break;
                case Levels.L_62: maxDPB = 696320; break;
            }
			return maxDPB;
        }

        #endregion
        #region verify and enforce
        /// <summary>
        /// validates a source against a given AVC level taking into account the source properties and the x264 settings
        /// </summary>
		/// <param name="bytesPerFrame">bytesize of a single frame</param>
		/// <param name="FS">frame area in pixels</param>
		/// <param name="MBPS">macroblocks per second</param>
        /// <param name="settings">the codec config to test</param>
		/// <param name="compliantLevel">the first avc level that can be used to encode this source</param>
		/// <returns>whether or not the current level is okay, if false and compliantLevel is -1, 
		/// the source could not be read</returns>
        public bool validateAVCLevel( int hRes, int vRes, double framerate, x264Settings settings, out AVCLevels.Levels? compliantLevel)
        {
            settings = (x264Settings)settings.Clone(); // otherwise this sets it to the lowest compliant level anyway.
            compliantLevel = Levels.L_UNRESTRICTED;
            if (settings.AVCLevel == Levels.L_UNRESTRICTED)
                return true;

            int FrameSize = (int)maxFS(hRes, vRes);
            int MBPS = maxBPS(hRes, vRes, framerate);
            int hBlocks = macroblocks(hRes);
            int vBlocks = macroblocks(vRes);
            double bufferSize = pictureBufferSize(settings, bytesPerFrame(hRes, vRes));
            int allowableBPS = this.getMaxMBPS(settings.AVCLevel);
            int allowableFS = this.getMaxFS(settings.AVCLevel);
            double dimensionRestriction = Math.Ceiling(Math.Sqrt((double)(allowableFS)*8));
            double allowableDPB = this.getMaxDPB(settings.AVCLevel) * 3 / 8 * 1024;

            if (allowableBPS >= MBPS && allowableFS >= FrameSize && allowableDPB >= bufferSize 
                && dimensionRestriction >= hBlocks && dimensionRestriction >= vBlocks)
                return true;
            else
            {
                while (settings.AVCLevel != Levels.L_UNRESTRICTED && (allowableBPS < MBPS || allowableFS < FrameSize || 
                    allowableDPB < bufferSize || dimensionRestriction < hBlocks || dimensionRestriction < vBlocks))
                {
                    settings.AVCLevel = settings.AVCLevel + 1;
                    allowableBPS = this.getMaxMBPS(settings.AVCLevel);
                    allowableFS = this.getMaxFS(settings.AVCLevel);
                    dimensionRestriction = Math.Ceiling(Math.Sqrt((double)(allowableFS)*8));
                    allowableDPB = this.getMaxDPB(settings.AVCLevel) * 3 / 8 * 1024;
                }
                compliantLevel = settings.AVCLevel;
                return false;
            }
        }

        #endregion
    }
}
