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

using System.Runtime.InteropServices;
using System.Text;

namespace MeGUI
{
    public class Drives
    {
        public static bool ableToWriteOnThisDrive(string myDrive)
        {
            bool b = false;
            switch (GetDriveType(myDrive))
            {
                case 0: // Unknown
                case 1: // Invalid Path
                case 5: // CDROM
                case 6: // RAM
                        b = false; break;
                case 2: // Removable (floppy, drive) 
                case 3: // Fixed (hard drive)
                case 4: // Remote (Network drive) 
                        b = true; break;
            }
            return b;
        }
        
        // used to send string message to media control interface device (mci)
        // like cd rom
        [DllImport("winmm.dll", EntryPoint = "mciSendStringA")]
        public static extern int mciSendString(string lpstrCommand, string lpstrReturnString, int uReturnLength, int hwndCallback);

        // used to get information about a drive ex: its name, seial number
        // if this function return zero means that one of the information could not be retrieved
        // so if it is a CD ROM drive and we can't obtain its name ---> CD ROM is empty
        [DllImport("kernel32.dll", EntryPoint = "GetVolumeInformationA")]
        public static extern int GetVolumeInformation(string lpRootPathName, StringBuilder lpVolumeNameBuffer, int nVolumeNameSize, int lpVolumeSerialNumber, int lpMaximumComponentLength, int lpFileSystemFlags, string lpFileSystemNameBuffer, int nFileSystemNameSize);

        // get the drive type 
        [DllImport("kernel32.dll", EntryPoint = "GetDriveTypeA")]
        public static extern int GetDriveType(string nDrive);
    }
}