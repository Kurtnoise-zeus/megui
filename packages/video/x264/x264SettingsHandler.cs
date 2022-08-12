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

using MeGUI.core.util;

namespace MeGUI.packages.video.x264
{
    class x264SettingsHandler
    {
        private x264Settings _xs;
        private LogItem _log;
        private x264Device _device;
        
        public x264SettingsHandler(x264Settings xs, LogItem log)
		{
            _xs = xs;
            _log = log;
            _device = _xs.TargetDevice;
        }

        /// <summary>
        /// Checks input video file for device compatibility
        /// </summary>
        /// <param name="hRes">the horizontal resolution</param>
        /// <param name="vRes">the vertical resolution</param>
        /// <param name="fps_n">fps numerator</param>
        /// <param name="fps_d">fps denominator</param>
        /// <returns>whether the source could be opened or not</returns>
        public void CheckInputFile(Dar? d, int hres, int vres, int fps_n, int fps_d)
        {
            if (_log == null)
                return;

            if (_device.BluRay == true)
            {
                if ((hres == 1920 || hres == 1440) && vres == 1080)
                {
                    string fps = fps_n + "/" + fps_d;
                    Double dfps = Double.Parse(fps_n.ToString()) / fps_d;
                    if (fps.Equals("24000/1001") || dfps == 24)
                    {
                        if (_xs.InterlacedMode != x264Settings.x264InterlacedModes.progressive)
                        {
                            _xs.InterlacedMode = x264Settings.x264InterlacedModes.progressive;
                            _log.LogEvent("the selected device does not support interlaced encoding of " + fps_n + "/" + fps_d + " fps. changing interlaced mode to progressive.", ImageType.Warning);
                        }
                        _xs.FakeInterlaced = _xs.PicStruct = false;
                    }
                    else if (dfps == 25 || fps.Equals("30000/1001"))
                    {
                        if (_xs.InterlacedMode == x264Settings.x264InterlacedModes.progressive)
                            _xs.FakeInterlaced = _xs.PicStruct = true;
                        else
                            _xs.FakeInterlaced = _xs.PicStruct = false;
                    }
                    else if (dfps == 30)
                    {
                        _xs.FakeInterlaced = _xs.PicStruct = false;
                        if (_xs.InterlacedMode == x264Settings.x264InterlacedModes.progressive)
                        {
                            _xs.InterlacedMode = x264Settings.x264InterlacedModes.tff;
                            _log.LogEvent("the selected device does not support progressive encoding of " + fps_n + "/" + fps_d + " fps. changing interlaced mode to tff.", ImageType.Warning);
                        }
                    }
                    else
                        _log.LogEvent("the selected device does not support " + fps_n + "/" + fps_d + " fps with a resolution of " + hres + "x" + vres + ". Supported are 24000/1001, 24/1, 25/1, 30000/1001 and 30/1.", ImageType.Warning);
                    
                    _xs.ColorPrim = _xs.Transfer = _xs.ColorMatrix = 1;
                    if (hres == 1440)
                        _xs.SampleAR = 2;
                    else
                        _xs.SampleAR = 1;
                    _xs.X264PullDown = 0;
                }
                else if (hres == 1280 && vres == 720)
                {
                    if (_xs.InterlacedMode != x264Settings.x264InterlacedModes.progressive)
                    {
                        _xs.InterlacedMode = x264Settings.x264InterlacedModes.progressive;
                        _log.LogEvent("the selected device does not support interlaced encoding of " + hres + "x" + vres + ". changing interlaced mode to progressive.", ImageType.Warning);
                    }
                    _xs.FakeInterlaced = _xs.PicStruct = false;
                    _xs.X264PullDown = 0;

                    string fps = fps_n + "/" + fps_d;
                    Double dfps = Double.Parse(fps_n.ToString()) / fps_d;
                    if (dfps == 25 || fps.Equals("30000/1001"))
                    {
                        if (_xs.X264PullDown != 4)
                        {
                            _xs.X264PullDown = 4;
                            _log.LogEvent("changing --pulldown to double as it is required for encoding of " + hres + "x" + vres + " with " + fps_n + "/" + fps_d + " fps for the selected device.", ImageType.Information);
                        }
                    }
                    else if (!fps.Equals("24000/1001") && dfps != 24 && dfps != 50 && !fps.Equals("60000/1001"))
                        _log.LogEvent("the selected device does not support " + fps_n + "/" + fps_d + " fps with a resolution of " + hres + "x" + vres + ". Supported are 24000/1001, 24/1, 25/1, 30000/1001, 50/1 and 60000/1001.", ImageType.Warning);
                    
                    _xs.ColorPrim = _xs.Transfer = _xs.ColorMatrix = 1;
                    _xs.SampleAR = 1;
                }
                else if (hres == 720 && vres == 576)
                {
                    if (_xs.SampleAR != 5 && _xs.SampleAR != 6)
                    {
                        if (!d.HasValue)
                        {
                            _xs.SampleAR = 6;
                            _log.LogEvent("assume --sar 16:11 as only 16:11 or 12:11 are supported with a resolution of " + hres + "x" + vres + ". for the selected device", ImageType.Warning);
                        }
                        else
                        {
                            Sar s = d.Value.ToSar(hres, vres);
                            _log.LogEvent("detected --sar " + s.X + ":" + s.Y, ImageType.Information);
                            Double dDiff16 = 16.0 / 11 - Double.Parse(s.X.ToString()) / s.Y;
                            Double dDiff4 = 12.0 / 11 - Double.Parse(s.X.ToString()) / s.Y;
                            if (dDiff16 <= 0)
                            {
                                _xs.SampleAR = 6;
                                _log.LogEvent("assume --sar 16:11 as only 16:11 or 12:11 are supported with a resolution of " + hres + "x" + vres + " for the selected device.", ImageType.Warning);
                            }
                            else if (dDiff4 >= 0)
                            {
                                _xs.SampleAR = 5;
                                _log.LogEvent("assume --sar 12:11 as only 16:11 or 12:11 are supported with a resolution of " + hres + "x" + vres + " for the selected device.", ImageType.Warning);
                            }
                            else if (Math.Min(dDiff16, -dDiff4) == dDiff16)
                            {
                                _xs.SampleAR = 6;
                                _log.LogEvent("assume --sar 16:11 as only 16:11 or 12:11 are supported with a resolution of " + hres + "x" + vres + " for the selected device.", ImageType.Warning);
                            }
                            else
                            {
                                _xs.SampleAR = 5;
                                _log.LogEvent("assume --sar 12:11 as only 16:11 or 12:11 are supported with a resolution of " + hres + "x" + vres + " for the selected device.", ImageType.Warning);
                            }
                        }
                    }

                    Double dfps = Double.Parse(fps_n.ToString()) / fps_d;
                    if (dfps == 25)
                    {
                        if (_xs.InterlacedMode == x264Settings.x264InterlacedModes.progressive)
                            _xs.FakeInterlaced = _xs.PicStruct = true;
                        else
                            _xs.FakeInterlaced = _xs.PicStruct = false;
                    }
                    else
                        _log.LogEvent("the selected device does not support " + fps_n + "/" + fps_d + " fps with a resolution of " + hres + "x" + vres + ". Supported is 25/1.", ImageType.Warning);

                    _xs.ColorPrim = _xs.Transfer = _xs.ColorMatrix = 3;
                    _xs.X264PullDown = 0;
                }
                else if (hres == 720 && vres == 480)
                {
                    if (_xs.SampleAR != 8 && _xs.SampleAR != 4)
                    {
                        if (!d.HasValue)
                        {
                            _xs.SampleAR = 8;
                            _log.LogEvent("assume --sar 40:33 as only 40:33 or 10:11 are supported with a resolution of " + hres + "x" + vres + " for the selected device.", ImageType.Warning);
                        }
                        else
                        {
                            Sar s = d.Value.ToSar(hres, vres);
                            _log.LogEvent("detected --sar " + s.X + ":" + s.Y, ImageType.Information);
                            Double dDiff16 = 40.0 / 33 - Double.Parse(s.X.ToString()) / s.Y;
                            Double dDiff4 = 10.0 / 11 - Double.Parse(s.X.ToString()) / s.Y;
                            if (dDiff16 <= 0)
                            {
                                _xs.SampleAR = 8;
                                _log.LogEvent("assume --sar 40:33 as only 40:33 or 10:11 are supported with a resolution of " + hres + "x" + vres + " for the selected device.", ImageType.Warning);
                            }
                            else if (dDiff4 >= 0)
                            {
                                _xs.SampleAR = 4;
                                _log.LogEvent("assume --sar 10:11 as only 40:33 or 10:11 are supported with a resolution of " + hres + "x" + vres + " for the selected device.", ImageType.Warning);
                            }
                            else if (Math.Min(dDiff16, -dDiff4) == dDiff16)
                            {
                                _xs.SampleAR = 8;
                                _log.LogEvent("assume --sar 40:33 as only 40:33 or 10:11 are supported with a resolution of " + hres + "x" + vres + " for the selected device.", ImageType.Warning);
                            }
                            else
                            {
                                _xs.SampleAR = 4;
                                _log.LogEvent("assume --sar 10:11 as only 40:33 or 10:11 are supported with a resolution of " + hres + "x" + vres + " for the selected device.", ImageType.Warning);
                            }
                        }
                    }

                    string fps = fps_n + "/" + fps_d;
                    if (fps.Equals("30000/1001"))
                    {
                        _xs.X264PullDown = 0;
                        _xs.FakeInterlaced = _xs.PicStruct = false;
                        if (_xs.InterlacedMode == x264Settings.x264InterlacedModes.progressive)
                        {
                            _xs.InterlacedMode = x264Settings.x264InterlacedModes.tff;
                            _log.LogEvent("the selected device does not support progressive encoding of " + fps_n + "/" + fps_d + " fps. changing interlaced mode to tff.", ImageType.Warning);
                        }
                    }
                    else if (fps.Equals("24000/1001"))
                    {
                        if (_xs.X264PullDown != 2)
                        {
                            _xs.X264PullDown = 2;
                            _log.LogEvent("changing --pulldown to 32 as it is required for encoding of " + hres + "x" + vres + " with " + fps_n + "/" + fps_d + " fps for the selected device.", ImageType.Warning);
                        }
                        if (_xs.InterlacedMode != x264Settings.x264InterlacedModes.progressive)
                        {
                            _xs.InterlacedMode = x264Settings.x264InterlacedModes.progressive;
                            _log.LogEvent("the selected device does not support interlaced encoding of " + hres + "x" + vres + ". changing interlaced mode to progressive.", ImageType.Warning);
                        }
                    }
                    else
                        _log.LogEvent("the selected device does not support " + fps_n + "/" + fps_d + " fps with a resolution of " + hres + "x" + vres + ". Supported are 30000/1001 and 24000/1001.", ImageType.Warning);
                    
                    _xs.ColorPrim = _xs.ColorMatrix = 4;
                    _xs.Transfer = 7;
                }
                else
                {
                    _log.LogEvent("the selected device does not support a resolution of " + hres + "x" + vres + ". Supported are 1920x1080, 1440x1080, 1280x720, 720x576 and 720x480.", ImageType.Warning);
                }
            }
            else
            {
                if (_device.Width > 0 && hres > _device.Width)
                    _log.LogEvent("the selected device does not support a resolution width of " + hres + ". The maximum value is " + _device.Width, ImageType.Warning);
                if (_device.Height > 0 && vres > _device.Height)
                    _log.LogEvent("the selected device does not support a resolution height of " + vres + ". The maximum value is " + _device.Height, ImageType.Warning);
            }
        }

        /// <summary>
        /// Calculates the --vbv-bufsize value
        /// </summary>
        /// <returns>the --vbv-bufsize value</returns>
        public int getVBVBufsize(AVCLevels.Levels avcLevel, bool bIsHighProfile)
        {
            string strCustomValue;
            int iTemp = _xs.VBVBufferSize;
            extractCustomCommand("vbv-bufsize", out strCustomValue);
            if (Int32.TryParse(strCustomValue, out iTemp))
                _xs.VBVBufferSize = iTemp;

            if (_device.VBVBufsize > -1 && (_xs.VBVBufferSize > _device.VBVBufsize || _xs.VBVBufferSize == 0))
            {
                if (_log != null)
                    _log.LogEvent("changing --vbv-bufsize to " + _device.VBVBufsize + " as required for the selected device");
                _xs.VBVBufferSize = _device.VBVBufsize;
            }

            if (_log != null && avcLevel != AVCLevels.Levels.L_UNRESTRICTED)
            {
                AVCLevels al = new AVCLevels();
                iTemp = al.getMaxCBP(avcLevel, bIsHighProfile);
                if (_xs.VBVBufferSize == 0)
                    _log.LogEvent("--vbv-bufsize is not restricted. Maximum value for level " + AVCLevels.GetLevelText(avcLevel) + " is " + iTemp + ". Playback may be affected. Reselect AVC level/profile or target playback device in the x264 preset to set the proper value.", ImageType.Warning);
            }

            return _xs.VBVBufferSize;
        }

        /// <summary>
        /// Calculates the --vbv-maxrate value
        /// </summary>
        /// <returns>the --vbv-maxrate value</returns>
        public int getVBVMaxrate(AVCLevels.Levels avcLevel, bool bIsHighProfile)
        {
            string strCustomValue;
            int iTemp = _xs.VBVMaxBitrate;
            extractCustomCommand("vbv-maxrate", out strCustomValue);
            if (Int32.TryParse(strCustomValue, out iTemp))
                _xs.VBVMaxBitrate = iTemp;

            if (_device.VBVMaxrate > -1 && (_xs.VBVMaxBitrate > _device.VBVMaxrate || _xs.VBVMaxBitrate == 0))
            {
                if (_log != null)
                    _log.LogEvent("changing --vbv-maxrate to " + _device.VBVMaxrate + " as required for the selected device");
                _xs.VBVMaxBitrate = _device.VBVMaxrate;
            }

            if (_log != null && avcLevel != AVCLevels.Levels.L_UNRESTRICTED)
            {
                AVCLevels al = new AVCLevels();
                iTemp = al.getMaxBR(avcLevel, bIsHighProfile);
                if (_xs.VBVMaxBitrate == 0)
                    _log.LogEvent("--vbv-maxrate is not restricted. Maximum value for level " + AVCLevels.GetLevelText(avcLevel) + " is " + iTemp + ". Playback may be affected. Reselect AVC level/profile or target playback device in the x264 preset to set the proper value.", ImageType.Warning);
            }

            return _xs.VBVMaxBitrate;
        }

        /// <summary>
        /// Calculates --sar value
        /// </summary>
        /// <returns>the --sar value</returns>
        public int getSar(Dar? d, int hRes, int vRes, out string CustomSarValue, string CustomSarValueInput)
        {
            string strCustomValue;
            CustomSarValue = String.Empty;
            if (String.IsNullOrEmpty(CustomSarValueInput) && extractCustomCommand("sar", out strCustomValue))
            {
                switch (strCustomValue.ToLowerInvariant())
                {
                    case "1:1": _xs.SampleAR = 1; break;
                    case "4:3": _xs.SampleAR = 2; break;
                    case "8:9": _xs.SampleAR = 3; break;
                    case "10:11": _xs.SampleAR = 4; break;
                    case "12:11": _xs.SampleAR = 5; break;
                    case "16:11": _xs.SampleAR = 6; break;
                    case "16:15": _xs.SampleAR = 7; break;
                    case "32:27": _xs.SampleAR = 8; break;
                    case "40:33": _xs.SampleAR = 9; break;
                    case "64:45": _xs.SampleAR = 10; break;
                    default:
                        CustomSarValue = strCustomValue;
                        _xs.SampleAR = 0; break;
                }
            }

            if (d.HasValue && _xs.SampleAR == 0 && 
                String.IsNullOrEmpty(CustomSarValue) && String.IsNullOrEmpty(CustomSarValueInput))
            {
                Sar s = d.Value.ToSar(hRes, vRes);
                switch (s.X + ":" + s.Y)
                {
                    case "1:1": _xs.SampleAR = 1; break;
                    case "4:3": _xs.SampleAR = 2; break;
                    case "8:9": _xs.SampleAR = 3; break;
                    case "10:11": _xs.SampleAR = 4; break;
                    case "12:11": _xs.SampleAR = 5; break;
                    case "16:11": _xs.SampleAR = 6; break;
                    case "16:15": _xs.SampleAR = 7; break;
                    case "32:27": _xs.SampleAR = 8; break;
                    case "40:33": _xs.SampleAR = 9; break;
                    case "64:45": _xs.SampleAR = 10; break;
                    default: CustomSarValue = s.X + ":" + s.Y; break;
                }
            }

            return _xs.SampleAR;
        }

        /// <summary>
        /// Calculates --colorprim value
        /// </summary>
        /// <returns>the --colorprim value</returns>
        public int getColorprim()
        {
            string strCustomValue;
            if (extractCustomCommand("colorprim", out strCustomValue))
            {
                switch (strCustomValue.ToLowerInvariant())
                {
                    case "bt709": _xs.ColorPrim = 1; break;
                    case "bt470m": _xs.ColorPrim = 2; break;
                    case "bt470bg": _xs.ColorPrim = 3; break;
                    case "smpte170m": _xs.ColorPrim = 4; break;
                    case "smpte240m": _xs.ColorPrim = 5; break;
                    case "film": _xs.ColorPrim = 6; break;
                }
            }
            return _xs.ColorPrim;
        }

        /// <summary>
        /// Calculates --pulldown value
        /// </summary>
        /// <returns>the --pulldown value</returns>
        public int getPulldown()
        {
            string strCustomValue;
            if (extractCustomCommand("pulldown", out strCustomValue))
            {
                switch (strCustomValue.ToLowerInvariant())
                {
                    case "none": _xs.X264PullDown = 0; break;
                    case "22": _xs.X264PullDown = 1; break;
                    case "32": _xs.X264PullDown = 2; break;
                    case "64": _xs.X264PullDown = 3; break;
                    case "double": _xs.X264PullDown = 4; break;
                    case "triple": _xs.X264PullDown = 5; break;
                    case "euro": _xs.X264PullDown = 6; break;
                }
            }
            return _xs.X264PullDown;
        }

        /// <summary>
        /// Calculates --transfer value
        /// </summary>
        /// <returns>the --transfer value</returns>
        public int getTransfer()
        {
            string strCustomValue;
            if (extractCustomCommand("transfer", out strCustomValue))
            {
                switch (strCustomValue.ToLowerInvariant())
                {
                    case "bt709": _xs.Transfer = 1; break;
                    case "bt470m": _xs.Transfer = 2; break;
                    case "bt470bg": _xs.Transfer = 3; break;
                    case "linear": _xs.Transfer = 4; break;
                    case "log100": _xs.Transfer = 5; break;
                    case "log316": _xs.Transfer = 6; break;
                    case "smpte170m": _xs.Transfer = 7; break;
                    case "smpte240m": _xs.Transfer = 8; break;
                }
            }
            return _xs.Transfer;
        }

        /// <summary>
        /// Calculates --colormatrix value
        /// </summary>
        /// <returns>the --colormatrix value</returns>
        public int getColorMatrix()
        {
            string strCustomValue;
            if (extractCustomCommand("colormatrix", out strCustomValue))
            {
                switch (strCustomValue.ToLowerInvariant())
                {
                    case "bt709": _xs.ColorMatrix = 1; break;
                    case "fcc": _xs.ColorMatrix = 2; break;
                    case "bt470bg": _xs.ColorMatrix = 3; break;
                    case "smpte170m": _xs.ColorMatrix = 4; break;
                    case "smpte240m": _xs.ColorMatrix = 5; break;
                    case "log316": _xs.ColorMatrix = 6; break;
                    case "gbr": _xs.ColorMatrix = 7; break;
                    case "ycgco": _xs.ColorMatrix = 8; break;
                }
            }
            return _xs.ColorMatrix;
        }

        /// <summary>
        /// Calculates the AVC profile number
        /// </summary>
        /// <returns>the AVC profile value (0 baseline, 1 main, 2 high)</returns>
        public int getProfile()
        {
            string strCustomValue;
            extractCustomCommand("profile", out strCustomValue);
            switch (strCustomValue.ToLowerInvariant())
            {
                case "baseline": _xs.Profile = 0; break;
                case "main": _xs.Profile = 1; break;
                case "high": _xs.Profile = 2; break;
            }

            if (_log == null)
                return _xs.Profile;

            if (_device.Profile > -1 &&_xs.Profile > _device.Profile)
            {
                if (_device.Profile == 0)
                    _log.LogEvent("changing --profile to baseline as required for the selected device");
                else if (_device.Profile == 1)
                    _log.LogEvent("changing --profile to main as required for the selected device");
                else
                    _log.LogEvent("changing --profile to high as required for the selected device");
                _xs.Profile = _device.Profile;
            }

            return _xs.Profile;
        }

        /// <summary>
        /// Calculates --nal-hrd value
        /// </summary>
        /// <returns>the --nal-hrd value</returns>
        public int getNalHrd()
        {
            string strCustomValue;
            extractCustomCommand("nal-hrd", out strCustomValue);
            switch (strCustomValue.ToLowerInvariant())
            {
                case "none": _xs.Nalhrd = 0; break;
                case "vbr": _xs.Nalhrd = 1; break;
                case "cbr": _xs.Nalhrd = 2; break;
            }

            if (_log == null)
                return _xs.Nalhrd;

            if (_device.BluRay && _xs.Nalhrd < 1)
            {
                _log.LogEvent("changing --nal-hrd to vbr as required for the selected device");
                _xs.Nalhrd = 1;
            }

            return _xs.Nalhrd;
        }

        /// <summary>
        /// Calculates the AVC level number
        /// </summary>
        /// <returns>the AVC level value</returns>
        public AVCLevels.Levels getLevel()
        {
            string strCustomValue;
            extractCustomCommand("level", out strCustomValue);
            switch (strCustomValue.ToLowerInvariant().Replace(".", "").Trim())
            {
                case "1": 
                case "10": _xs.AVCLevel = AVCLevels.Levels.L_10; break;
                case "11": _xs.AVCLevel = AVCLevels.Levels.L_11; break;
                case "12": _xs.AVCLevel = AVCLevels.Levels.L_12; break;
                case "13": _xs.AVCLevel = AVCLevels.Levels.L_13; break;
                case "1b": _xs.AVCLevel = AVCLevels.Levels.L_1B; break;
                case "2":
                case "20": _xs.AVCLevel = AVCLevels.Levels.L_20; break;
                case "21": _xs.AVCLevel = AVCLevels.Levels.L_21; break;
                case "22": _xs.AVCLevel = AVCLevels.Levels.L_22; break;
                case "3":
                case "30": _xs.AVCLevel = AVCLevels.Levels.L_30; break;
                case "31": _xs.AVCLevel = AVCLevels.Levels.L_31; break;
                case "32": _xs.AVCLevel = AVCLevels.Levels.L_32; break;
                case "4":
                case "40": _xs.AVCLevel = AVCLevels.Levels.L_40; break;
                case "41": _xs.AVCLevel = AVCLevels.Levels.L_41; break;
                case "42": _xs.AVCLevel = AVCLevels.Levels.L_42; break;
                case "5":
                case "50": _xs.AVCLevel = AVCLevels.Levels.L_50; break;
                case "51": _xs.AVCLevel = AVCLevels.Levels.L_51; break;
                case "52": _xs.AVCLevel = AVCLevels.Levels.L_52; break;
                case "6":
                case "60": _xs.AVCLevel = AVCLevels.Levels.L_60; break;
                case "61": _xs.AVCLevel = AVCLevels.Levels.L_61; break;
                case "62": _xs.AVCLevel = AVCLevels.Levels.L_62; break;
            }

            if (_log == null)
                return _xs.AVCLevel;

            if (_xs.AVCLevel.CompareTo(_device.AVCLevel) > 0)
            {
                _log.LogEvent("changing --level to " + _device.AVCLevel.ToString());
                _xs.Profile = _device.Profile;
            }

            return _xs.AVCLevel;
        }

        /// <summary>
        /// Calculates the --key-int value
        /// </summary>
        /// <param name="fps_n">fps numerator</param>
        /// <param name="fps_d">fps denominator</param>
        /// <returns>the --key-int value</returns>
        public int getKeyInt(int fps_n, int fps_d)
        {
            string strCustomValue;
            extractCustomCommand("keyint", out strCustomValue);
            if (strCustomValue.ToLowerInvariant().Equals("infinite"))
                _xs.KeyframeInterval = 0;
            else
            {
                int iTemp = _xs.KeyframeInterval;
                if (Int32.TryParse(strCustomValue, out iTemp))
                    _xs.KeyframeInterval = iTemp;
            }
            
            if (_log == null)
                return _xs.KeyframeInterval;

            if (_xs.x264GOPCalculation == 1) // calculate min-keyint based on 25fps
                _xs.KeyframeInterval = (int)Math.Round((double)_xs.KeyframeInterval / 25.0 * ((double)fps_n / fps_d), 0);

            int fps = (int)Math.Round((decimal)fps_n / fps_d, 0);
            if (_device.MaxGOP > -1 && _xs.KeyframeInterval > fps * _device.MaxGOP)
            {
                _log.LogEvent("changing --keyint to " + (fps * _device.MaxGOP) + " as required for the selected device");
                _xs.KeyframeInterval = fps * _device.MaxGOP;
            }

            return _xs.KeyframeInterval;
        }

        /// <summary>
        /// Calculates the --min-keyint value
        /// </summary>
        /// <param name="fps_n">fps numerator</param>
        /// <param name="fps_d">fps denominator</param>
        /// <returns>the --min-keyint value</returns>
        public int getMinKeyint(int fps_n, int fps_d)
        {
            string strCustomValue;
            extractCustomCommand("min-keyint", out strCustomValue);
            int iTemp = _xs.KeyframeInterval;
            if (Int32.TryParse(strCustomValue, out iTemp))
                _xs.MinGOPSize = iTemp;

            if (_log == null)
                return _xs.MinGOPSize;

            double fps = (double)fps_n / fps_d;
            if (_xs.x264GOPCalculation == 1) // calculate min-keyint based on 25fps
                _xs.MinGOPSize = (int)((double)_xs.MinGOPSize / 25.0 * fps);

            int iMaxValue = _xs.KeyframeInterval / 2 + 1;
            if (_device.MaxGOP > -1 && _xs.MinGOPSize > iMaxValue)
            {
                int iDefault = _xs.KeyframeInterval / 10;
                _log.LogEvent("changing --min-keyint to " + iDefault + " as required for the selected device");
                _xs.MinGOPSize = iDefault;
            }

            return _xs.MinGOPSize;
        }

        /// <summary>
        /// Calculates the --weightp value
        /// </summary>
        /// <returns>the --weightp value</returns>
        public int getWeightp()
        {
            string strCustomValue;
            extractCustomCommand("weightp", out strCustomValue);
            int iTemp = _xs.WeightedPPrediction;
            if (Int32.TryParse(strCustomValue, out iTemp))
                _xs.WeightedPPrediction = iTemp;

            if (_log == null)
                return _xs.WeightedPPrediction;

            if (_device.BluRay && _xs.WeightedPPrediction > 1)
            {
                _log.LogEvent("changing --weightp to 1 as required for the selected device");
                _xs.WeightedPPrediction = 1;
            }

            return _xs.WeightedPPrediction;
        }

        /// <summary>
        /// Calculates the --bluray-compat value
        /// </summary>
        /// <returns>true for --bluray-compat</returns>
        public bool getBlurayCompat()
        {
            string strCustomValue;
            if (extractCustomCommand("bluray-compat", out strCustomValue))
                _xs.BlurayCompat = true;

            if (_log == null)
                return _xs.BlurayCompat;

            if (_device.BluRay && _xs.BlurayCompat == false)
            {
                _log.LogEvent("enabling --bluray-compat as required for the selected device");
                _xs.BlurayCompat = true;
            }

            return _xs.BlurayCompat;
        }

        /// <summary>
        /// Calculates the --pic-struct value
        /// </summary>
        /// <returns>true for --pic-struct</returns>
        public bool getPicStruct()
        {
            string strCustomValue;
            if (extractCustomCommand("pic-struct", out strCustomValue))
                _xs.PicStruct = true;
            return _xs.PicStruct;
        }

        /// <summary>
        /// Calculates the --fake-interlaced value
        /// </summary>
        /// <returns>true for --fake-interlaced</returns>
        public bool getFakeInterlaced()
        {
            string strCustomValue;
            if (extractCustomCommand("fake-interlaced", out strCustomValue))
                _xs.FakeInterlaced = true;
            return _xs.FakeInterlaced;
        }

        /// <summary>
        /// Calculates the interlaced mode
        /// </summary>
        /// <returns>interlaced mode</returns>
        public x264Settings.x264InterlacedModes getInterlacedMode()
        {
            string strCustomValue;
            if (extractCustomCommand("tff", out strCustomValue))
                _xs.InterlacedMode = x264Settings.x264InterlacedModes.tff;
            if (extractCustomCommand("bff", out strCustomValue))
                _xs.InterlacedMode = x264Settings.x264InterlacedModes.bff;
            return _xs.InterlacedMode;
        }

        /// <summary>
        /// Calculates the --aud value
        /// </summary>
        /// <returns>true for --aud</returns>
        public bool getAud()
        {
            string strCustomValue;
            if (extractCustomCommand("aud", out strCustomValue))
                _xs.X264Aud = true;

            if (_log == null)
                return _xs.X264Aud;

            if (_device.BluRay && _xs.X264Aud == false)
            {
                _log.LogEvent("enabling --aud as required for the selected device");
                _xs.X264Aud = true;
            }

            return _xs.X264Aud;
        }

        /// <summary>
        /// Calculates the --bframes value
        /// </summary>
        /// <returns>the --bframes value</returns>
        public int getBFrames()
        {
            string strCustomValue;
            extractCustomCommand("bframes", out strCustomValue);
            int iTemp = _xs.NbBframes;
            if (Int32.TryParse(strCustomValue, out iTemp))
                _xs.NbBframes = iTemp;

            if (_log == null)
                return _xs.NbBframes;

            if (_device.BFrames > -1 && _xs.NbBframes > _device.BFrames)
            {
                _log.LogEvent("changing --bframes to " + _device.BFrames + " as required for the selected device");
                _xs.NbBframes = _device.BFrames;
            }

            return _xs.NbBframes;
        }

        /// <summary>
        /// Calculates the --frames value
        /// </summary>
        /// <returns>the --frames value</returns>
        public void getFrames(ref ulong frames)
        {
            if (_log == null)
                return;

            string strCustomValue = "";
            ulong seek = 0;
            if (extractCustomCommand("seek", out strCustomValue))
            {
                ulong iTemp = seek;
                if (ulong.TryParse(strCustomValue, out iTemp))
                {
                    seek = iTemp;
                    if (seek >= frames)
                    {
                        seek = 0;
                        _log.LogEvent("removing --seek as the seek value outreaches the frame number");
                    }
                    else
                    {
                        _xs.CustomEncoderOptions += " --seek " + seek;
                        frames -= seek;  // reduce by the number of seek frames
                    }
                }
            }

            strCustomValue = string.Empty;
            ulong framesCustom = frames;
            if (extractCustomCommand("frames", out strCustomValue))
            {
                ulong iTemp = frames;
                if (ulong.TryParse(strCustomValue, out iTemp))
                {
                    framesCustom = iTemp;
                    if (framesCustom > frames)
                    {
                        _log.LogEvent("reducing --frames as required by the selected source");
                        framesCustom = frames;
                    }
                    else
                        frames = framesCustom;
                }
            }

            return;
        }

        /// <summary>
        /// Calculates the --b-pyramid value
        /// </summary>
        /// <returns>the --b-pyramid value</returns>
        public int getBPyramid()
        {
            string strCustomValue;
            extractCustomCommand("b-pyramid", out strCustomValue);
            switch (strCustomValue.ToLowerInvariant())
            {
                case "none": _xs.x264BFramePyramid = 0; break;
                case "normal": _xs.x264BFramePyramid = 2; break;
                case "strict": _xs.x264BFramePyramid = 1; break;
            }

            if (_log == null)
                return _xs.x264BFramePyramid;

            if (_device.BluRay && _xs.x264BFramePyramid > 1)
            {
                _log.LogEvent("changing --b-pyramid to strict as required for the selected device");
                _xs.x264BFramePyramid = 1;
            }

            if (_device.BPyramid > -1 && _xs.x264BFramePyramid != _device.BPyramid)
            {
                string strMode = "normal";
                if (_device.BPyramid == 0)
                    strMode = "none";
                else if (_device.BPyramid == 1)
                    strMode = "strict";
                _log.LogEvent("changing --b-pyramid to " + strMode + " as required for the selected device");
                _xs.x264BFramePyramid = _device.BPyramid;
            }

            return _xs.x264BFramePyramid;
        }

        /// <summary>
        /// Calculates the --slices value
        /// </summary>
        /// <returns>the --slices value</returns>
        public int getSlices()
        {
            string strCustomValue;
            extractCustomCommand("slices", out strCustomValue);
            int iTemp = _xs.SlicesNb;
            if (Int32.TryParse(strCustomValue, out iTemp))
                _xs.SlicesNb = iTemp;

            if (_log == null)
                return _xs.SlicesNb;

            if (_device.BluRay && _xs.SlicesNb != 4)
            {
                _log.LogEvent("changing --slices to 4 as required for the selected device");
                _xs.SlicesNb = 4;
            }

            return _xs.SlicesNb;
        }

        /// <summary>
        /// Calculates the --ref value
        /// </summary>
        /// <param name="hRes">the horizontal resolution</param>
        /// <param name="vRes">the vertical resolution</param>
        /// <returns>the --ref value</returns>
        public int getRefFrames(int hRes, int vRes)
        {
            string strCustomValue;
            extractCustomCommand("ref", out strCustomValue);
            int iTemp = _xs.NbRefFrames;
            if (Int32.TryParse(strCustomValue, out iTemp))
                _xs.NbRefFrames = iTemp;

            if (_log == null)
                return _xs.NbRefFrames;

            if (_device.ReferenceFrames > -1 && _xs.NbRefFrames > _device.ReferenceFrames)
            {
                _log.LogEvent("changing --ref to " + _device.ReferenceFrames + " as required for the selected device");
                _xs.NbRefFrames = _device.ReferenceFrames;
            }

            int iMaxRefForLevel = getMaxRefForLevel(_xs.AVCLevel, hRes, vRes);
            if (iMaxRefForLevel > -1 && iMaxRefForLevel < _xs.NbRefFrames)
            {
                _log.LogEvent("changing --ref to " + iMaxRefForLevel);
                _xs.NbRefFrames = iMaxRefForLevel;
            }

            return _xs.NbRefFrames;
        }

        /// <summary>
        /// gets the maximum ref count based upon the given level
        /// </summary>
        /// <param name="level">the AVC level</param>
        /// <param name="hRes">the horizontal resolution</param>
        /// <param name="vRes">the vertical resolution</param>
        /// <returns>the maximum reference count; -1 if no restriction</returns>
        public static int getMaxRefForLevel(AVCLevels.Levels avcLevel, int hRes, int vRes)
        {
            if (hRes <= 0 || vRes <= 0 || avcLevel == AVCLevels.Levels.L_UNRESTRICTED)  // Unrestricted/Autoguess
                return -1;

            AVCLevels oAVC = new AVCLevels();
            int maxDPB = (int)oAVC.getMaxDPB(avcLevel);  // the maximum picture decoded buffer for the given level
            int frameHeightInMbs = (int)System.Math.Ceiling((double)vRes / 16);
            int frameWidthInMbs = (int)System.Math.Ceiling((double)hRes / 16);
            int maxRef = (int)System.Math.Floor((double)maxDPB / (frameHeightInMbs * frameWidthInMbs));
            return System.Math.Min(maxRef, 16);
        }

        /// <summary>
        /// Gets the --fps value
        /// </summary>
        /// <returns>the --fps value</returns>
        public void getFPS(ref int fps_n, ref int fps_d)
        {
            string custom = _xs.CustomEncoderOptions;
            string strCustomValue;
            if (!extractCustomCommand("fps", out strCustomValue))
                return;
            _xs.CustomEncoderOptions = custom;

            int fpsnum, fpsden;
            System.Globalization.CultureInfo culture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
            if (strCustomValue.Contains("/"))
            {
                if (!Int32.TryParse(strCustomValue.Split('/')[0], System.Globalization.NumberStyles.None, culture, out fpsnum))
                    return;

                if (!Int32.TryParse(strCustomValue.Split('/')[1], System.Globalization.NumberStyles.None, culture, out fpsden))
                    return;
            }
            else
            {
                double fps;
                if (!Double.TryParse(strCustomValue, System.Globalization.NumberStyles.AllowDecimalPoint, culture, out fps))
                    return;

                if (!VideoUtil.GetFPSDetails(fps, null, out fpsnum, out fpsden))
                    return;
            }

            if (_log != null)
                _log.LogEvent("custom frame rate: " + fpsnum + "/" + fpsden);

            fps_n = fpsnum;
            fps_d = fpsden;
        }

        /// <summary>
        /// returns the value for the selected command in the custom command line 
        /// and removes the command from the custom command line
        /// </summary>
        /// <param name="strCommandToExtract">the command to extract without --</param>
        /// <param name="strCommandValue">the value of the command</param>
        /// <returns>true if the command can be found</returns>
        private bool extractCustomCommand(string strCommandToExtract, out string strCommandValue)
        {
            string strNewCommandLine = "";
            bool bFound = false;
            strCommandValue = String.Empty;

            // add a leading space for easier command detection
            _xs.CustomEncoderOptions = " " + _xs.CustomEncoderOptions;

            // custom command line splitted bei either " -" or " --"
            foreach (string strCommandTemp in System.Text.RegularExpressions.Regex.Split(_xs.CustomEncoderOptions, @"(?= --[a-zA-Z]{1}| -[a-zA-Z]{1})"))
            {
                if (string.IsNullOrEmpty(strCommandTemp.Trim()))
                    continue;
                string strCommand = strCommandTemp;

                string strLeading = string.Empty;
                if (strCommand.StartsWith(" --"))
                    strLeading = " --";
                else if (strCommand.StartsWith(" -"))
                    strLeading = " -";
                strCommand = strCommand.Substring(strLeading.Length);

                if (strCommand.Trim().ToLowerInvariant().Equals(strCommandToExtract.ToLowerInvariant()) 
                    || strCommand.Trim().ToLowerInvariant().StartsWith(strCommandToExtract.ToLowerInvariant() + " "))
                {
                    strCommandValue = strCommand.Substring(strCommandToExtract.Length).Trim();
                    bFound = true;
                }
                else
                    strNewCommandLine += strLeading + strCommand;
            }

            _xs.CustomEncoderOptions = strNewCommandLine.Trim();
            return bFound;
        }

        /// <summary>
        /// returns the custom command line 
        /// </summary>
        /// <returns>the custom command line</returns>
        public string getCustomCommandLine()
        {
            return _xs.CustomEncoderOptions;
        }
    }
}