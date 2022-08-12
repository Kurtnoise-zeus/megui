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
using System.Diagnostics;
using System.Text;

namespace MeGUI.core.util
{
    public class Resolution
    {
        /// <summary>
        /// gets the desired resolution based on input settings
        /// </summary>
        /// <param name="sourceWidth">the complete width of the input file without cropping</param>
        /// <param name="sourceHeight">the complete height of the input file without cropping</param>
        /// <param name="inputDAR">the input DAR value</param>
        /// <param name="cropValues">the crop values</param>
        /// <param name="cropEnabled">true if crop values must be used</param>
        /// <param name="mod">the mod value used for the final resize values</param>
        /// <param name="resizeEnabled">true if resize can be used</param>
        /// <param name="upsizingEnabled">true if upsizing can be used</param>
        /// <param name="signalAR">whether or not ar signalling is to be used for the output 
        /// (depending on this parameter, resizing changes to match the source AR)</param>
        /// <param name="suggestHeight">true if height should be calculated</param>
        /// <param name="acceptableAspectErrorPercent">acceptable aspect error if signalAR is true</param>
        /// <param name="xTargetDevice">x264 Target Device - may limit the output resolution</param>
        /// <param name="fps">the frames per second of the source</param>
        /// <param name="outputWidth">the calculated output width</param>
        /// <param name="outputHeight">the calculated output height</param>
        /// <param name="paddingValues">the padding values</param>
        /// <param name="outputDar">the output DAR value</param>
        /// <param name="_log">the log item</param>
        public static void GetResolution(int sourceWidth, int sourceHeight, Dar inputDar, 
            ref CropValues cropValues, bool cropEnabled, int mod, ref bool resizeEnabled, bool upsizingAllowed, 
            bool signalAR, bool suggestHeight, decimal acceptableAspectErrorPercent,
            x264Device xTargetDevice, Double fps, ref int outputWidth, ref int outputHeight, 
            out CropValues paddingValues, out Dar? outputDar, LogItem _log)
        {
            paddingValues = new CropValues();

            getResolution(sourceWidth, sourceHeight, inputDar,
                cropValues, cropEnabled, mod, resizeEnabled, upsizingAllowed,
                signalAR, suggestHeight, acceptableAspectErrorPercent,
                ref outputWidth, ref outputHeight,
                out outputDar, _log);

            bool settingsChanged;
            if (isResolutionDeviceCompliant(xTargetDevice, outputWidth, outputHeight, out settingsChanged, ref resizeEnabled, ref cropEnabled, _log) == true)
            {
                if (!cropEnabled)
                    cropValues = new CropValues();
                return;
            }

            if (settingsChanged)
            {
                getResolution(sourceWidth, sourceHeight, inputDar,
                    cropValues, cropEnabled, mod, resizeEnabled, upsizingAllowed,
                    signalAR, suggestHeight, acceptableAspectErrorPercent,
                    ref outputWidth, ref outputHeight,
                    out outputDar, _log);

                // check if the resolution is now compliant
                if (isResolutionDeviceCompliant(xTargetDevice, outputWidth, outputHeight, out settingsChanged, ref resizeEnabled, ref cropEnabled, null) == true)
                {
                    if (!cropEnabled)
                        cropValues = new CropValues();
                    return;
                }
            }
            if (!cropEnabled)
                cropValues = new CropValues();

            // adjust horizontal resolution if width or height are too large
            int outputHeightIncludingPadding = 0;
            int outputWidthIncludingPadding = 0;
            if (xTargetDevice.BluRay)
            {
                if (outputWidth >= 1920)
                {
                    outputWidth = 1920;
                    outputHeightIncludingPadding = 1080;
                }
                else if (outputWidth >= 1280)
                {
                    outputWidth = 1280;
                    outputHeightIncludingPadding = 720;
                }
                else
                {
                    outputWidth = 720;
                    if (fps == 25)
                        outputHeightIncludingPadding = 576;
                    else
                        outputHeightIncludingPadding = 480;
                }
                outputWidthIncludingPadding = outputWidth;
                if (_log != null)
                    _log.LogEvent("Force resolution of " + outputWidth + "x" + outputHeightIncludingPadding + " as required for " + xTargetDevice.Name);
            }
            else if (xTargetDevice.Width > 0 && outputWidth > xTargetDevice.Width)
            {
                outputWidth = xTargetDevice.Width;
                _log.LogEvent("Set resolution width to " + outputWidth + " as required for " + xTargetDevice.Name);
            }

            // adjust cropped vertical resolution
            getResolution(sourceWidth, sourceHeight, inputDar,
                    cropValues, cropEnabled, mod, resizeEnabled, upsizingAllowed,
                    signalAR, suggestHeight, acceptableAspectErrorPercent,
                    ref outputWidth, ref outputHeight,
                    out outputDar, _log);
            while ((xTargetDevice.Height > 0 && outputHeight > xTargetDevice.Height) || (xTargetDevice.BluRay && outputHeight > outputHeightIncludingPadding))
            {
                outputWidth -= mod;
                getResolution(sourceWidth, sourceHeight, inputDar,
                    cropValues, cropEnabled, mod, resizeEnabled, upsizingAllowed,
                    signalAR, suggestHeight, acceptableAspectErrorPercent,
                    ref outputWidth, ref outputHeight,
                    out outputDar, _log);
            }

            paddingValues.left = Convert.ToInt32(Math.Floor((outputWidthIncludingPadding - outputWidth) / 2.0));
            paddingValues.right = Convert.ToInt32(Math.Ceiling((outputWidthIncludingPadding - outputWidth) / 2.0));
            paddingValues.bottom = Convert.ToInt32(Math.Floor((outputHeightIncludingPadding - outputHeight) / 2.0));
            paddingValues.top = Convert.ToInt32(Math.Ceiling((outputHeightIncludingPadding - outputHeight) / 2.0));

            outputWidth = outputWidthIncludingPadding;
            outputHeight = outputHeightIncludingPadding;

            if (!cropEnabled)
                cropValues = new CropValues();
        }

        /// <summary>
        /// check if resolution is device compatible
        /// </summary>
        /// <param name="xTargetDevice">x264 Target Device - may limit the output resolution</param>
        /// <param name="outputWidth">the calculated output width</param>
        /// <param name="outputHeight">the calculated output height</param>
        /// <param name="settingsChanged">true if resize or crop has been changed</param>
        /// <param name="resizeEnabled">if resize is enabled</param>
        /// <param name="cropEnabled">if crop is enabled</param>
        /// <param name="_log">log item</param>
        /// <returns>true if the settings are device compatible</returns>
        private static bool isResolutionDeviceCompliant(x264Device xTargetDevice, int outputWidth, int outputHeight, 
            out bool settingsChanged, ref bool resizeEnabled, ref bool cropEnabled, LogItem _log)
        {
            settingsChanged = false;
            if (xTargetDevice == null)
                return true;
            if (!xTargetDevice.BluRay && xTargetDevice.Height <= 0 && xTargetDevice.Width <= 0)
                return true;

            bool bAdjustResolution = false;
            if (xTargetDevice.Width > 0 && xTargetDevice.Width < outputWidth)
            {
                // width must be lowered to be target conform
                bAdjustResolution = true;
                if (!resizeEnabled)
                {
                    resizeEnabled = settingsChanged = true;
                    if (_log != null)
                        _log.LogEvent("Enabling \"Resize\" as " + xTargetDevice.Name + " does not support a resolution width of "
                            + outputWidth + ". The maximum value is " + xTargetDevice.Width + ".");
                }
            }
            else if (xTargetDevice.Height > 0 && xTargetDevice.Height < outputHeight)
            {
                // height must be lowered to be target conform
                bAdjustResolution = true;
                if (!resizeEnabled)
                {
                    resizeEnabled = settingsChanged = true;
                    if (_log != null)
                        _log.LogEvent("Enabling \"Resize\" as " + xTargetDevice.Name + " does not support a resolution height of "
                            + outputHeight + ". The maximum value is " + xTargetDevice.Height + ".");
                }
            }
            else if (xTargetDevice.BluRay)
            {
                string strResolution = outputWidth + "x" + outputHeight;
                if (!strResolution.Equals("1920x1080") &&
                    !strResolution.Equals("1440x1080") &&
                    !strResolution.Equals("1280x720") &&
                    !strResolution.Equals("720x576") &&
                    !strResolution.Equals("720x480"))
                {
                    bAdjustResolution = settingsChanged = true;
                    if (!resizeEnabled)
                    {
                        resizeEnabled = true;
                        if (_log != null)
                            _log.LogEvent("Enabling \"Resize\" as " + xTargetDevice.Name + " does not support a resolution of "
                                + outputWidth + "x" + outputHeight
                                + ". Supported are 1920x1080, 1440x1080, 1280x720, 720x576 and 720x480.");
                    }
                }
            }

            if (bAdjustResolution && !cropEnabled)
            {
                if (_log != null)
                    _log.LogEvent("Enabling \"Crop\"");
                cropEnabled = settingsChanged = true;
            }

            if (bAdjustResolution)
                return false;
            return true;
        }

        /// <summary>
        /// gets the desired resolution based on input settings
        /// </summary>
        /// <param name="sourceWidth">the complete width of the input file without cropping</param>
        /// <param name="sourceHeight">the complete height of the input file without cropping</param>
        /// <param name="inputDAR">the input DAR value</param>
        /// <param name="cropping">the crop values</param>
        /// <param name="mod">the mod value used for the final resize values</param>
        /// <param name="resizeEnabled">true if resize can be used</param>
        /// <param name="upsizingEnabled">true if upsizing can be used</param>
        /// <param name="signalAR">whether or not ar signalling is to be used for the output 
        /// (depending on this parameter, resizing changes to match the source AR)</param>
        /// <param name="suggestHeight">true if height should be calculated</param>
        /// <param name="acceptableAspectErrorPercent">acceptable aspect error if signalAR is true</param>
        /// <param name="outputWidth">the calculated output width</param>
        /// <param name="outputHeight">the calculated output height</param>
        /// <param name="outputDar">the output DAR value</param>
        /// <param name="_log">the log item</param>
        private static void getResolution(int sourceWidth, int sourceHeight, Dar inputDar,
            CropValues cropValues, bool cropEnabled, int mod, bool resizeEnabled, 
            bool upsizingAllowed, bool signalAR, bool suggestHeight, 
            decimal acceptableAspectErrorPercent, ref int outputWidth, 
            ref int outputHeight, out Dar? outputDar, LogItem _log)
        {
            outputDar = null;

            CropValues cropping = new CropValues();
            if (cropEnabled)
                cropping = cropValues.Clone();

            // remove upsizing if not allowed
            if (!upsizingAllowed && sourceWidth - cropping.left - cropping.right < outputWidth)
            {
                outputWidth = sourceWidth - cropping.left - cropping.right;
                if (_log != null)
                    _log.LogEvent("Lowering output width resolution to " + outputWidth + " to avoid upsizing");
            }

            // correct hres if not mod compliant
            if (outputWidth % mod != 0)
            {
                int diff = outputWidth % mod;
                if (outputWidth - diff > 0)
                    outputWidth -= diff;
                else
                    outputWidth = mod;
            }

            if (suggestHeight)
            {
                int scriptVerticalResolution = Resolution.SuggestVerticalResolution(sourceHeight, sourceWidth, inputDar, cropping,
                    outputWidth, signalAR, out outputDar, mod, acceptableAspectErrorPercent);

                int iMaximum = 9999;
                if (!upsizingAllowed)
                    iMaximum = sourceHeight - cropping.top - cropping.bottom;

                // Reduce horizontal resolution until a fit is found
                while (scriptVerticalResolution > iMaximum && outputWidth > mod)
                {
                    outputWidth -= mod;
                    scriptVerticalResolution = Resolution.SuggestVerticalResolution(sourceHeight, sourceWidth, inputDar, cropping,
                        outputWidth, signalAR, out outputDar, mod, acceptableAspectErrorPercent);
                }
                outputHeight = scriptVerticalResolution;
            }
            else if (!resizeEnabled)
                outputHeight = sourceHeight - cropping.top - cropping.bottom;
        }

        /// <summary>
        /// calculates the mod value
        /// </summary>
        /// <param name="modMethod">the selected mod method</param>
        /// <param name="modMethod">the selected mod value</param>
        /// <param name="signalAR">whether or not we're going to signal the aspect ratio</param>
        /// <returns>the mod value</returns>
        public static int GetModValue(modValue modValue, mod16Method modMethod, bool signalAR)
        {
            int mod = 16;
            if (!signalAR || modMethod != mod16Method.nonMod16)
            {
                switch (modValue)
                {
                    case modValue.mod8: mod = 8; break;
                    case modValue.mod4: mod = 4; break;
                    case modValue.mod2: mod = 2; break;
                }
            }
            else
                mod = 1;

            return mod;
        }

        /// <summary>
        /// calculates the ideal mod vertical resolution that matches the desired horizontal resolution
        /// </summary>
        /// <param name="readerHeight">height of the source</param>
        /// <param name="readerWidth">width of the source</param>
        /// <param name="customDAR">custom display aspect ratio to be taken into account for resizing</param>
        /// <param name="cropping">the crop values for the source</param>
        /// <param name="horizontalResolution">the desired horizontal resolution of the output</param>
        /// <param name="signalAR">whether or not we're going to signal the aspect ratio (influences the resizing)</param>
        /// <param name="sarX">horizontal pixel aspect ratio (used when signalAR = true)</param>
        /// <param name="sarY">vertical pixel aspect ratio (used when signalAR = true)</param>
        /// <param name="mod">the MOD value</param>
        /// <returns>the suggested horizontal resolution</returns>
        public static int SuggestVerticalResolution(double readerHeight, double readerWidth, Dar inputDAR, CropValues cropping, int horizontalResolution,
            bool signalAR, out Dar? dar, int mod, decimal acceptableAspectErrorPercent)
        {
            int scriptVerticalResolution;

            decimal fractionOfWidth = ((decimal)readerWidth - (decimal)cropping.left - (decimal)cropping.right) / (decimal)readerWidth;
            decimal sourceHorizontalResolution = (decimal)readerHeight * inputDAR.AR * fractionOfWidth;
            decimal sourceVerticalResolution = (decimal)readerHeight - (decimal)cropping.top - (decimal)cropping.bottom;
            decimal resizedVerticalResolution = (decimal)horizontalResolution / (sourceHorizontalResolution / sourceVerticalResolution);

            if (signalAR)
            {
                decimal inputWidthOnHeight = ((decimal)readerWidth - (decimal)cropping.left - (decimal)cropping.right) /
                                          ((decimal)readerHeight - (decimal)cropping.top - (decimal)cropping.bottom);
                decimal realAspectRatio = getAspectRatio(inputDAR.AR, sourceHorizontalResolution / sourceVerticalResolution, acceptableAspectErrorPercent); // the aspect ratio of the video
                resizedVerticalResolution = (decimal)horizontalResolution / inputWidthOnHeight; // Scale vertical resolution appropriately
                scriptVerticalResolution = ((int)Math.Round(resizedVerticalResolution / (decimal)mod) * mod);

                if (inputDAR.AR != realAspectRatio)
                {
                    int parX = 0;
                    int parY = 0;
                    int iLimit = 101;
                    decimal distance = 999999;
                    if (acceptableAspectErrorPercent == 0)
                        iLimit = 100001;
                    for (int i = 1; i < iLimit; i++)
                    {
                        // We create a fraction with integers, and then convert back to a decimal, and see how big the rounding error is
                        decimal fractionApproximation = (decimal)Math.Round(realAspectRatio * ((decimal)i)) / (decimal)i;
                        decimal approximationDifference = Math.Abs(realAspectRatio - fractionApproximation);
                        if (approximationDifference < distance)
                        {
                            distance = approximationDifference;
                            parY = i;
                            parX = (int)Math.Round(realAspectRatio * ((decimal)parY));
                        }
                    }
                    Debug.Assert(parX > 0 && parY > 0);
                    dar = new Dar((ulong)parX, (ulong)parY);
                }
                else
                    dar = inputDAR;
            }
            else
            {
                scriptVerticalResolution = ((int)Math.Round(resizedVerticalResolution / (decimal)mod)) * mod;
                dar = null;
            }

            return scriptVerticalResolution;
        }

        /// <summary>
        /// finds the aspect ratio closest to the one giving as parameter (which is an approximation using the selected DAR for the source and the cropping values)
        /// </summary>
        /// <param name="calculatedAR">the aspect ratio to be approximated</param>
        /// <returns>the aspect ratio that most closely matches the input</returns>
        private static decimal getAspectRatio(decimal inputAR, decimal calculatedAR, decimal acceptableAspectErrorPercent)
        {
            decimal aspectError = inputAR / calculatedAR;
            if (Math.Abs(aspectError - 1.0M) * 100.0M < acceptableAspectErrorPercent)
                return inputAR;
            else
                return calculatedAR;
        }

        /// <summary>
        /// calculates the aspect ratio error
        /// </summary>
        /// <param name="inputWidth">height of the source (without cropping!)</param>
        /// <param name="inputHeight">width of the source (without cropping!)</param>
        /// <param name="outputWidth">the desired width of the output</param>
        /// <param name="outputHeight">the desired height of the output</param>
        /// <param name="Cropping">the crop values for the source</param>
        /// <param name="inputDar">custom input display aspect ratio to be taken into account</param>
        /// <param name="signalAR">whether or not we're going to signal the aspect ratio (influences the resizing)</param>
        /// <param name="outputDar">custom output display aspect ratio to be taken into account</param>
        /// <returns>the aspect ratio error in percent</returns>
        public static decimal GetAspectRatioError(int inputWidth, int inputHeight, int outputWidth, int outputHeight, CropValues Cropping, Dar? inputDar, bool signalAR, Dar? outputDar)
        {
            if (inputHeight <= 0 || inputWidth <= 0 || outputHeight <= 0 || outputWidth <= 0)
                return 0;

            // get input dimension with SAR 1:1
            int iHeight = inputHeight - Cropping.top - Cropping.bottom;
            decimal iWidth = inputWidth - Cropping.left - Cropping.right;
            if (inputDar.HasValue)
            {
                Sar s = inputDar.Value.ToSar(inputWidth, inputHeight);
                iWidth = iWidth * s.X / s.Y;
            }

            // get output dimension with SAR 1:1
            int oHeight = outputHeight;
            decimal oWidth = outputWidth;
            if (signalAR && outputDar.HasValue)
            {
                Sar s = outputDar.Value.ToSar(outputWidth, outputHeight);
                oWidth = oWidth * s.X / s.Y;
            }

            return (iHeight * oWidth) / (iWidth * oHeight) - 1;
        }

        /// <summary>
        /// calculates the DAR value based upon the video information 
        /// </summary>
        /// <param name="width">width of the video</param>
        /// <param name="height">height of the video</param>
        /// <param name="dar">display aspect ratio </param>
        /// <param name="par">pixel aspect ratio </param>
        /// <param name="darString">display aspect ratio string - e.g. 16:9 or 4:3</param>
        /// <returns>the DAR value</returns>
        public static Dar GetDAR(int width, int height, string darValue, decimal? par, string darString)
        {
            if (!String.IsNullOrEmpty(darString) && width == 720 && (height == 576 || height == 480))
            {
                Dar newDar = Dar.A1x1;
                if (!MainForm.Instance.Settings.UseITUValues)
                {
                    if (darString.Equals("16:9"))
                        newDar = Dar.STATIC16x9;
                    else if (darString.Equals("4:3"))
                        newDar = Dar.STATIC4x3;
                }
                else if (height == 576)
                {
                    if (darString.Equals("16:9"))
                        newDar = Dar.ITU16x9PAL;
                    else if (darString.Equals("4:3"))
                        newDar = Dar.ITU4x3PAL;
                }
                else
                {
                    if (darString.Equals("16:9"))
                        newDar = Dar.ITU16x9NTSC;
                    else if (darString.Equals("4:3"))
                        newDar = Dar.ITU4x3NTSC;
                }
                if (!newDar.Equals(Dar.A1x1))
                    return newDar;
            }

            if (par == null || par <= 0)
                par = 1;

            decimal? dar = easyParseDecimal(darValue);
            if (dar != null && dar > 0)
            {
                decimal correctDar = (decimal)width * (decimal)par / height;
                if (Math.Abs(Math.Round(correctDar, 3) - Math.Round((decimal)dar, 3)) <= 0.001M)
                    return new Dar((ulong)(Math.Round(width * (decimal)par)), (ulong)height);
                else
                    return new Dar((decimal)dar);
            }

            if (darValue.Contains(":"))
                return new Dar(ulong.Parse(darValue.Split(':')[0]), ulong.Parse(darValue.Split(':')[1]));
            else
                return new Dar((ulong)width, (ulong)height);
        }

        private static decimal? easyParseDecimal(string value)
        {
            try
            {
                System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("en-us");
                return decimal.Parse(value, culture);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}