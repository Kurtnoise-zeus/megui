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
using System.Text;

namespace MeGUI
{
    public class x264Device
    {
        private string strName;
        private int iID, iProfile, iVBVBufsize, iVBVMaxrate, iBframes, iReframes, iMaxWidth, iMaxHeight, iMaxGop, iBPyramid;
        private bool bBluRay;
        private AVCLevels.Levels avcLevel;

        public static List<x264Device> CreateDeviceList()
        {
            List<x264Device> x264DeviceList = new List<x264Device>();
            x264DeviceList.Add(new x264Device(0, "Default", -1, AVCLevels.Levels.L_UNRESTRICTED, -1, -1, -1, -1, -1, -1));
            x264DeviceList.Add(new x264Device(2, "AVCHD", 2, AVCLevels.Levels.L_41, 14000, 14000, 3, 6, 1920, 1080, true, -1, -1));
            x264DeviceList.Add(new x264Device(3, "Blu-ray", 2, AVCLevels.Levels.L_41, 30000, 40000, 3, 6, 1920, 1080, true, 1, -1));
            x264DeviceList.Add(new x264Device(4, "DivX Plus HD", 2, AVCLevels.Levels.L_40, 25000, 20000, 3, -1, 1920, 1080, false, 4, -1));
            x264DeviceList.Add(new x264Device(5, "DXVA", 2, AVCLevels.Levels.L_41, -1, -1, -1, -1, -1, -1));
            x264DeviceList.Add(new x264Device(7, "iPhone 2G", 0, AVCLevels.Levels.L_13, -1, -1, 0, 1, 640, 480));
            x264DeviceList.Add(new x264Device(9, "iPhone 3G/3GS, iPod classic, iPod touch 1/2/3, iPod nano 3/4/5", 0, AVCLevels.Levels.L_30, -1, -1, 0, 1, 640, 480));
            x264DeviceList.Add(new x264Device(8, "iPhone 4, iPad 1/2, iPod touch 4/5", 1, AVCLevels.Levels.L_31, -1, -1, -1, -1, 1280, 720));
            x264DeviceList.Add(new x264Device(15, "iPhone 4S/5, iPad 3/4/mini, WDTV", 2, AVCLevels.Levels.L_41, -1, -1, -1, -1, 1920, 1080));
            x264DeviceList.Add(new x264Device(17, "iPod nano 7", 2, AVCLevels.Levels.L_30, -1, -1, -1, -1, 720, 576));
            x264DeviceList.Add(new x264Device(12, "PS3", 2, AVCLevels.Levels.L_41, 31250, 31250, -1, -1, 1920, 1080));
            x264DeviceList.Add(new x264Device(13, "PSP", 1, AVCLevels.Levels.L_30, 10000, 10000, -1, 3, 480, 272, false, -1, 0));
            x264DeviceList.Add(new x264Device(14, "Xbox 360", 2, AVCLevels.Levels.L_41, 24000, 24000, 3, 3, 1920, 1080));
            return x264DeviceList;
        }

        public x264Device(int iID, string strName, int iProfile, AVCLevels.Levels avcLevel, int iVBVBufsize, int iVBVMaxrate, int iBframes, int iReframes, int iMaxWidth, int iMaxHeight) : 
            this(iID, strName, iProfile, avcLevel, iVBVBufsize, iVBVMaxrate, iBframes, iReframes, iMaxWidth, iMaxHeight, false, -1, -1) { }

        public x264Device(int iID, string strName, int iProfile, AVCLevels.Levels avcLevel, int iVBVBufsize, int iVBVMaxrate, int iBframes, int iReframes, int iMaxWidth, int iMaxHeight, bool bBluRay, int iMaxGop, int iBPyramid)
        {
            this.iID = iID;
            this.strName = strName;
            this.iProfile = iProfile;
            this.avcLevel = avcLevel;
            this.iVBVBufsize = iVBVBufsize;
            this.iVBVMaxrate = iVBVMaxrate;
            this.iBframes = iBframes;
            this.iReframes = iReframes;
            this.iMaxWidth = iMaxWidth;
            this.iMaxHeight = iMaxHeight;
            this.bBluRay = bBluRay;
            this.iBPyramid = iBPyramid;
            this.iMaxGop = iMaxGop;
        }

        public int ID
        {
            get { return iID; }
        }

        public string Name
        {
            get { return strName; }
        }

        public int Profile
        {
            get { return iProfile; }
        }

        public AVCLevels.Levels AVCLevel
        {
            get { return avcLevel; }
        }

        public int VBVBufsize
        {
            get { return iVBVBufsize; }
        }

        public int VBVMaxrate
        {
            get { return iVBVMaxrate; }
        }

        public int BFrames
        {
            get { return iBframes; }
        }

        public int ReferenceFrames
        {
            get { return iReframes; }
        }

        public int Height
        {
            get { return iMaxHeight; }
        }

        public int Width
        {
            get { return iMaxWidth; }
        }

        public int MaxGOP
        {
            get { return iMaxGop; }
            set { iMaxGop = value; }
        }

        public bool BluRay
        {
            get { return bBluRay; }
            set { bBluRay = value; }
        }

        public int BPyramid
        {
            get { return iBPyramid; }
            set { iBPyramid = value;}
        }

        public override string ToString()
        {
            return strName;
        }
    }
}