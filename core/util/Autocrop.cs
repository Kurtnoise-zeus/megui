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
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;

namespace MeGUI.core.util
{
    public class Autocrop
    {
        public static bool autocrop(out CropValues cropValues, IVideoReader reader, bool signalAR, mod16Method cropMethod, modValue mValue)
        {
            cropValues = Autocrop.autocrop(reader);

            if (signalAR)
            {
                if (cropMethod == mod16Method.overcrop)
                    ScriptServer.overcrop(ref cropValues, mValue);
                else if (cropMethod == mod16Method.mod4Horizontal)
                    ScriptServer.cropMod4Horizontal(ref cropValues);
                else if (cropMethod == mod16Method.undercrop)
                    ScriptServer.undercrop(ref cropValues, mValue);
            }

            if (cropValues.left < 0)
                return false;
            else
                return true;
        }

        public static CropValues autocrop(IVideoReader reader)
        {
            // start at 10% of the video, then advance by 6,66% and analyze 11 frames in total
            int pos = reader.FrameCount / 10;
            int step = reader.FrameCount / 15;
            CropValues[] cropValues = new CropValues[11];
            for (int i = 0; i < 11; i++)
            {
                using (Bitmap b = reader.ReadFrameBitmap(pos))
                    cropValues[i] = getAutoCropValues(b);
                pos += step;
            }
            bool error = false;
            CropValues final = getFinalAutocropValues(cropValues);
            if (!error)
            {
                return final;
            }
            else
            {
                final.left = -1;
                final.right = -1;
                final.top = -1;
                final.bottom = -1;
                return final;
            }
        }

        /// <summary>
        /// iterates through a set of CropValues and tries to find a majority of matching crop values. 
        /// if a match is found, the crop values are returned
        /// if not, the minimum found value is returned for the value in question
        /// </summary>
        /// <param name="values">the CropValues array to be analyzed</param>
        /// <returns>the final CropValues</returns>
        private static CropValues getFinalAutocropValues(CropValues[] values)
        {
            CropValues retval = values[0].Clone();
            Dictionary<int, int> topValues = new Dictionary<int, int>();
            Dictionary<int, int> leftValues = new Dictionary<int, int>();
            Dictionary<int, int> rightValues = new Dictionary<int, int>();
            Dictionary<int, int> bottomValues = new Dictionary<int, int>();
            
            // group crop values
            for (int i = 0; i < values.Length; i++)
            {
                if (leftValues.ContainsKey(values[i].left))
                    leftValues[values[i].left]++;
                else
                    leftValues.Add(values[i].left, 1);
                if (topValues.ContainsKey(values[i].top))
                    topValues[values[i].top]++;
                else
                    topValues.Add(values[i].top, 1);
                if (rightValues.ContainsKey(values[i].right))
                    rightValues[values[i].right]++;
                else
                    rightValues.Add(values[i].right, 1);
                if (bottomValues.ContainsKey(values[i].bottom))
                    bottomValues[values[i].bottom]++;
                else
                    bottomValues.Add(values[i].bottom, 1);

                // set min values found
                if (values[i].left < retval.left)
                    retval.left = values[i].left;
                if (values[i].top < retval.top)
                    retval.top = values[i].top;
                if (values[i].right < retval.right)
                    retval.right = values[i].right;
                if (values[i].bottom < retval.bottom)
                    retval.bottom = values[i].bottom;
            }

            // get "best match" values
            foreach (KeyValuePair<int, int> kvp in leftValues)
            {
                if (kvp.Value > values.Length / 2) // we have more than 50% matching values, use value found
                {
                    retval.left = kvp.Key;
                    break;
                }
            }
            foreach (KeyValuePair<int, int> kvp in topValues)
            {
                if (kvp.Value > values.Length / 2) // we have more than 50% matching values, use value found
                {
                    retval.top = kvp.Key;
                    break;
                }
            }
            foreach (KeyValuePair<int, int> kvp in rightValues)
            {
                if (kvp.Value > values.Length / 2) // we have more than 50% matching values, use value found
                {
                    retval.right = kvp.Key;
                    break;
                }
            }
            foreach (KeyValuePair<int, int> kvp in bottomValues)
            {
                if (kvp.Value > values.Length / 2) // we have more than 50% matching values, use value found
                {
                    retval.bottom = kvp.Key;
                    break;
                }
            }

            return retval;
        }

        private static bool isBadPixel(int pixel)
        {
            int comp = 12632256;
            int res = pixel & comp;
            return (res != 0);
        }

        /// <summary>
        /// iterates through the lines and columns of the bitmap and compares the pixel values with the value of the upper left corner pixel
        /// if a pixel that doesn't have the same color value is found, it is assumed that this is the first line that does not need to be cropped away
        /// </summary>
        /// <param name="b">the bitmap to be analyzed</param>
        /// <returns>struct containing the number of lines to be cropped away from the left, top, right and bottom</returns>
        private static unsafe CropValues getAutoCropValues(Bitmap b)
        {
            // When locking the pixels into memory, they are currently being converted from 24bpp to 32bpp. This incurs a small (5%) speed penalty,
            // but means that pixel management is easier, because each pixel is a 4-byte int.
            BitmapData image = b.LockBits(new Rectangle(0, 0, b.Width, b.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            int* pointer = (int*)image.Scan0.ToPointer();
            int* lineBegin, pixel;
            int stride = image.Stride / 4;
            CropValues retval = new CropValues();
            bool lineFound = false;
            int badPixelThreshold = 50;
            int widthBadPixelThreshold = b.Width / badPixelThreshold;
            int heightBadPixelThreshold = b.Height / badPixelThreshold;
            int nbBadPixels = 0;

            lineBegin = pointer;
            for (int i = 0; i < b.Width; i++)
            {
                pixel = lineBegin;
                for (int j = 0; j < b.Height; j++)
                {
                    if (isBadPixel(*pixel))
                        nbBadPixels++;
                    if (nbBadPixels > heightBadPixelThreshold)
                    {
                        retval.left = i;
                        if (retval.left < 0)
                            retval.left = 0;
                        if (retval.left % 2 != 0)
                            retval.left++;
                        lineFound = true;
                        break;
                    }
                    pixel += stride;
                }
                nbBadPixels = 0;
                if (lineFound)
                    break;
                lineBegin += 1; // 4-byte Argb
            }
            nbBadPixels = 0;
            lineFound = false;
            lineBegin = pointer;
            for (int i = 0; i < b.Height; i++)
            {
                pixel = lineBegin;
                for (int j = 0; j < b.Width; j++)
                {
                    if (isBadPixel(*pixel))
                        nbBadPixels++;
                    if (nbBadPixels > widthBadPixelThreshold)
                    {
                        retval.top = i;
                        if (retval.top < 0)
                            retval.top = 0;
                        if (retval.top % 2 != 0)
                            retval.top++;
                        lineFound = true;
                        break;
                    }
                    pixel += 1; // 4-byte Argb
                }
                nbBadPixels = 0;
                if (lineFound)
                    break;
                lineBegin += stride;
            }
            nbBadPixels = 0;
            lineFound = false;
            lineBegin = pointer + b.Width - 1;
            for (int i = b.Width - 1; i >= 0; i--)
            {
                pixel = lineBegin;
                for (int j = 0; j < b.Height; j++)
                {
                    if (isBadPixel(*pixel))
                        nbBadPixels++;
                    if (nbBadPixels > heightBadPixelThreshold)
                    {
                        retval.right = b.Width - i - 1;
                        if (retval.right < 0)
                            retval.right = 0;
                        if (retval.right % 2 != 0)
                            retval.right++;
                        lineFound = true;
                        break;
                    }
                    pixel += stride;
                }
                nbBadPixels = 0;
                if (lineFound)
                    break;
                lineBegin -= 1; // Backwards across 4-byte Argb
            }
            nbBadPixels = 0;
            lineFound = false;
            lineBegin = pointer + stride * (b.Height - 1);
            for (int i = b.Height - 1; i >= 0; i--)
            {
                pixel = lineBegin;
                for (int j = 0; j < b.Width; j++)
                {
                    if (isBadPixel(*pixel))
                        nbBadPixels++;
                    if (nbBadPixels > widthBadPixelThreshold)
                    {
                        retval.bottom = b.Height - i - 1;
                        if (retval.bottom < 0)
                            retval.bottom = 0;
                        if (retval.bottom % 2 != 0)
                            retval.bottom++;
                        lineFound = true;
                        break;
                    }
                    pixel += 1;// 4-byte Argb
                }
                nbBadPixels = 0;
                if (lineFound)
                    break;
                lineBegin -= stride;
            }
            b.UnlockBits(image);
            return retval;
        }
    }
}