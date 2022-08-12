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
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace MeGUI
{
    public enum AviSynthColorspace : int
    {
        Unknown = 0,

        // 8 bit
        RGB24       = +1342177281,  // ffmpeg: fail     x264: warning
        YUY2        = +1610612740,  // ffmpeg: fail     x264: warning
        YV12        = -1610612728,  
        YV16        = -1610611960,  //                  x264: warning
        YV24        = -1610611957,  //                  x264: warning
        YV411       = -1610611959,  //                  x264: warning
        Y8          = -536870912,   // ffmpeg: fail     x264: warning
        RGBP        = -1879048191,  // ffmpeg: fail     x264: fail
        RGBP8       = RGBP,         // ffmpeg: fail     x264: fail
        RGB32       = +1342177282,  // ffmpeg: fail     x264: warning
        RGBAP       = -1879048190,  // ffmpeg: fail     x264: fail
        RGBAP8      = RGBAP,        // ffmpeg: fail     x264: fail
        I420        = -1610612720,

        // 10 bit
        RGBP10      = -1878720511,  // ffmpeg: fail     x264: fail
        YUV444P10   = -1610284277,  //                  x264: fail
        YUV422P10   = -1610284280,  //                  x264: fail
        YUV420P10   = -1610285048,  //                  x264: fail
        Y10         = -536543232,   // ffmpeg: fail     x264: fail
        RGBAP10     = -1878720510,  // ffmpeg: fail     x264: fail
        YUVA444P10  = -2012937461,  // ffmpeg: fail     x264: fail
        YUVA422P10  = -2012937464,  // ffmpeg: fail     x264: fail
        YUVA420P10  = -2012938232,  // ffmpeg: fail     x264: fail

        // 12 bit
        RGBP12      = -1878654975,  // ffmpeg: fail     x264: fail
        YUV444P12   = -1610218741,  //                  x264: fail
        YUV422P12   = -1610218744,  //                  x264: fail
        YUV420P12   = -1610219512,  //                  x264: fail
        Y12         = -536477696,   // ffmpeg: fail     x264: fail
        RGBAP12     = -1878654974,  // ffmpeg: fail     x264: fail
        YUVA444P12  = -2012871925,  // ffmpeg: fail     x264: fail
        YUVA422P12  = -2012871928,  // ffmpeg: fail     x264: fail
        YUVA420P12  = -2012872696,  // ffmpeg: fail     x264: fail

        // 14 bit
        RGBP14      = -1878589439,  // ffmpeg: fail     x264: fail
        YUV444P14   = -1610153205,  //                  x264: fail
        YUV422P14   = -1610153208,  //                  x264: fail
        YUV420P14   = -1610153976,  //                  x264: fail
        Y14         = -536412160,   // ffmpeg: fail     x264: fail
        RGBAP14     = -1878589438,  // ffmpeg: fail     x264: fail
        YUVA444P14  = -2012806389,  // ffmpeg: fail     x264: fail
        YUVA422P14  = -2012806392,  // ffmpeg: fail     x264: fail
        YUVA420P14  = -2012807160,  // ffmpeg: fail     x264: fail

        // 16 bit
        RGB48       = 1342242817,   // ffmpeg: fail     x264: warning
        RGBP16      = -1878982655,  // ffmpeg: fail     x264: fail
        YUV444P16   = -1610546421,  //                  x264: warning
        YUV422P16   = -1610546424,  //                  x264: warning
        YUV420P16   = -1610547192,
        Y16         = -536805376,   // ffmpeg: fail     x264: warning
        RGB64       = 1342242818,   // ffmpeg: fail     x264: warning
        RGBAP16     = -1878982654,  // ffmpeg: fail     x264: fail
        YUVA444P16  = -2013199605,  // ffmpeg: fail     x264: fail
        YUVA422P16  = -2013199608,  // ffmpeg: fail     x264: fail
        YUVA420P16  = -2013200376,  // ffmpeg: fail     x264: fail

        // 32 bit
        RGBPS       = -1878917119,  // ffmpeg: fail     x264: fail
        RGBAPS      = -1878917118   // ffmpeg: fail     x264: fail
    }

    class AviSynthColorspaceHelper
    {
        public static Dictionary<AviSynthColorspace, int> GetColorSpaceDictionary()
        {
            Dictionary<AviSynthColorspace, int> arrColorspace = new Dictionary<AviSynthColorspace, int>();

            arrColorspace.Add(AviSynthColorspace.RGB24,         8);
            arrColorspace.Add(AviSynthColorspace.YUY2,          8);
            arrColorspace.Add(AviSynthColorspace.YV12,          8);
            arrColorspace.Add(AviSynthColorspace.YV16,          8);
            arrColorspace.Add(AviSynthColorspace.YV24,          8);
            arrColorspace.Add(AviSynthColorspace.YV411,         8);
            arrColorspace.Add(AviSynthColorspace.Y8,            8);
            arrColorspace.Add(AviSynthColorspace.RGBP,          8);
            //arrColorspace.Add(AviSynthColorspace.RGBP8,       8);
            arrColorspace.Add(AviSynthColorspace.RGB32,         8);
            arrColorspace.Add(AviSynthColorspace.RGBAP,         8);
            //arrColorspace.Add(AviSynthColorspace.RGBAP8,      8);

            arrColorspace.Add(AviSynthColorspace.RGBP10,        10);
            arrColorspace.Add(AviSynthColorspace.RGBAP10,       10);
            arrColorspace.Add(AviSynthColorspace.Y10,           10);
            arrColorspace.Add(AviSynthColorspace.YUV444P10,     10);
            arrColorspace.Add(AviSynthColorspace.YUV422P10,     10);
            arrColorspace.Add(AviSynthColorspace.YUV420P10,     10);
            arrColorspace.Add(AviSynthColorspace.YUVA444P10,    10);
            arrColorspace.Add(AviSynthColorspace.YUVA422P10,    10);
            arrColorspace.Add(AviSynthColorspace.YUVA420P10,    10);

            arrColorspace.Add(AviSynthColorspace.RGBP12,        12);
            arrColorspace.Add(AviSynthColorspace.RGBAP12,       12);
            arrColorspace.Add(AviSynthColorspace.Y12,           12);
            arrColorspace.Add(AviSynthColorspace.YUV444P12,     12);
            arrColorspace.Add(AviSynthColorspace.YUV422P12,     12);
            arrColorspace.Add(AviSynthColorspace.YUV420P12,     12);
            arrColorspace.Add(AviSynthColorspace.YUVA444P12,    12);
            arrColorspace.Add(AviSynthColorspace.YUVA422P12,    12);
            arrColorspace.Add(AviSynthColorspace.YUVA420P12,    12);

            arrColorspace.Add(AviSynthColorspace.RGBP14,        14);
            arrColorspace.Add(AviSynthColorspace.RGBAP14,       14);
            arrColorspace.Add(AviSynthColorspace.Y14,           14);
            arrColorspace.Add(AviSynthColorspace.YUV444P14,     14);
            arrColorspace.Add(AviSynthColorspace.YUV422P14,     14);
            arrColorspace.Add(AviSynthColorspace.YUV420P14,     14);
            arrColorspace.Add(AviSynthColorspace.YUVA444P14,    14);
            arrColorspace.Add(AviSynthColorspace.YUVA422P14,    14);
            arrColorspace.Add(AviSynthColorspace.YUVA420P14,    14);

            arrColorspace.Add(AviSynthColorspace.RGB48,         16);
            arrColorspace.Add(AviSynthColorspace.RGBP16,        16);
            arrColorspace.Add(AviSynthColorspace.RGB64,         16);
            arrColorspace.Add(AviSynthColorspace.RGBAP16,       16);
            arrColorspace.Add(AviSynthColorspace.Y16,           16);
            arrColorspace.Add(AviSynthColorspace.YUV444P16,     16);
            arrColorspace.Add(AviSynthColorspace.YUV422P16,     16);
            arrColorspace.Add(AviSynthColorspace.YUV420P16,     16);
            arrColorspace.Add(AviSynthColorspace.YUVA444P16,    16);
            arrColorspace.Add(AviSynthColorspace.YUVA422P16,    16);
            arrColorspace.Add(AviSynthColorspace.YUVA420P16,    16);

            arrColorspace.Add(AviSynthColorspace.RGBPS,         32);
            arrColorspace.Add(AviSynthColorspace.RGBAPS,        32);

            return arrColorspace;
        }

        /// <summary>
        /// Gets the target color space
        /// </summary>
        /// <param name="strEncoder">the encoder which will be used</param>
        /// <param name="colorspace">the current color space</param>
        /// <returns>Target Color Space</returns>
        public static AviSynthColorspace GetConvertedColorspace(string strEncoder, AviSynthColorspace colorspace)
        {
            ArrayList arrCodecsAllowed = new ArrayList();

            // add always available color spaces
            arrCodecsAllowed.Add(AviSynthColorspace.I420);
            arrCodecsAllowed.Add(AviSynthColorspace.YV12);

            // if avs+ is used add the available color spaces for ffmpeg+x264
            if (!strEncoder.Equals("xvid") && MainForm.Instance.Settings.AviSynthPlus)
                arrCodecsAllowed.Add(AviSynthColorspace.YUV420P16);

            if (strEncoder.Equals("ffmpeg"))
            {
                // ffmpeg is used
                arrCodecsAllowed.Add(AviSynthColorspace.YV16);
                arrCodecsAllowed.Add(AviSynthColorspace.YV24);
                arrCodecsAllowed.Add(AviSynthColorspace.YV411);

                // > 8bit is only possible if AVS+ is used
                if (MainForm.Instance.Settings.AviSynthPlus)
                {
                    arrCodecsAllowed.Add(AviSynthColorspace.YUV444P10);
                    arrCodecsAllowed.Add(AviSynthColorspace.YUV422P10);
                    arrCodecsAllowed.Add(AviSynthColorspace.YUV420P10);
                    arrCodecsAllowed.Add(AviSynthColorspace.YUV444P12);
                    arrCodecsAllowed.Add(AviSynthColorspace.YUV422P12);
                    arrCodecsAllowed.Add(AviSynthColorspace.YUV420P12);
                    arrCodecsAllowed.Add(AviSynthColorspace.YUV444P14);
                    arrCodecsAllowed.Add(AviSynthColorspace.YUV422P14);
                    arrCodecsAllowed.Add(AviSynthColorspace.YUV420P14);
                    arrCodecsAllowed.Add(AviSynthColorspace.YUV444P16);
                    arrCodecsAllowed.Add(AviSynthColorspace.YUV422P16);
                    arrCodecsAllowed.Add(AviSynthColorspace.YUV420P16);
                }
            }

            // check if the current color space does not have to be changed
            if (arrCodecsAllowed.Contains(colorspace))
                return colorspace;

            // if xvid is the encoder or AVS+ is not used, YV12 has to be used
            if (strEncoder.Equals("xvid") || !MainForm.Instance.Settings.AviSynthPlus)
                return AviSynthColorspace.YV12;

            // try to get the bit depth
            Dictionary<AviSynthColorspace, int> arrColorspace = GetColorSpaceDictionary();
            if (!arrColorspace.TryGetValue(colorspace, out int iBit))
                return colorspace;

            if (strEncoder.Equals("ffmpeg"))
            {
                switch (iBit)
                {
                    case 8:  colorspace = AviSynthColorspace.YV12; break;
                    case 10: colorspace = AviSynthColorspace.YUV420P10; break;
                    case 12: colorspace = AviSynthColorspace.YUV420P12; break;
                    case 14: colorspace = AviSynthColorspace.YUV420P14; break;
                    case 16: colorspace = AviSynthColorspace.YUV420P16; break;
                    case 32: colorspace = AviSynthColorspace.YUV420P16; break;
                    default: colorspace = AviSynthColorspace.YV12; break;
                }
            }
            else
            {
                switch (iBit)
                {
                    case 8: colorspace = AviSynthColorspace.YV12; break;
                    default: colorspace = AviSynthColorspace.YUV420P16; break;
                }
            }

            return colorspace;
        }

        /// <summary>
        /// Gets the AviSynth ConvertTo() function
        /// </summary>
        /// <param name="colorspace"></param>
        /// <returns>ConvertTo() function</returns>
        public static string GetConvertTo(string colorspace_new, string colorspace_old)
        {
            string strConvertTo = colorspace_new;
            if (strConvertTo.ToLower().Contains("p"))
                strConvertTo = strConvertTo.Substring(0, strConvertTo.ToLower().IndexOf("p"));
            if (colorspace_old.ToLower().Contains("a"))
                strConvertTo = "RemoveAlphaPlane().ConvertTo" + strConvertTo + "()";
            else
                strConvertTo = "ConvertTo" + strConvertTo + "()";
            return strConvertTo;
        }

        public static bool AppendConvertTo(string file, AviSynthColorspace colorspace_new, AviSynthColorspace colorspace_old)
        {
            try
            {
                StreamWriter avsOut = new StreamWriter(file, true);
                if (MainForm.Instance.Settings.AviSynthPlus)
                {
                    System.Collections.Generic.Dictionary<AviSynthColorspace, int> arrColorspace = AviSynthColorspaceHelper.GetColorSpaceDictionary();
                    if (!arrColorspace.TryGetValue(colorspace_new, out int iBit))
                        avsOut.Write("\r\nConvertBits(8)");
                    else
                        avsOut.Write("\r\nConvertBits(" + iBit + ")");
                }
                avsOut.Write("\r\n" + AviSynthColorspaceHelper.GetConvertTo(colorspace_new.ToString(), colorspace_old.ToString()));
                avsOut.Close();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public static bool IsConvertedToColorspace(string file, string colorspace)
        {
            try
            {
                String strLastLine = "", line = "";
                using (StreamReader reader = new StreamReader(file))
                {
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (!String.IsNullOrEmpty(line))
                            strLastLine = line;
                    }
                }
                if (strLastLine.ToLowerInvariant().Equals(AviSynthColorspaceHelper.GetConvertTo(colorspace, string.Empty).ToLowerInvariant()))
                    return true;
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }
    }
}