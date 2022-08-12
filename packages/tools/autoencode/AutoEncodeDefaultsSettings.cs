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

using MeGUI.core.gui;
using MeGUI.core.util;

namespace MeGUI
{
    [LogByMembers]
    public class AutoEncodeDefaultsSettings
    {
        private bool fileSizeMode, bitrateMode, noTargetSizeMode, addAdditionalContent;
        private string container, deviceType;
        private FileSize? splitSize = null, fileSize = TargetSizeSCBox.PredefinedFilesizes[2].Data;
        private int bitrate;

        public AutoEncodeDefaultsSettings()
        {
            fileSizeMode = true;
            bitrateMode = false;
            noTargetSizeMode = false;
            addAdditionalContent = false;
            container = "MP4";
            deviceType = "Standard";
            bitrate = 700;
        }
        #region properties
        /// <summary>
        /// gets / sets if additional content should be added
        /// </summary>
        public bool AddAdditionalContent
        {
            get { return addAdditionalContent; }
            set { addAdditionalContent = value; }
        }
        /// <summary>
        /// gets / sets if the final size should be defined by the bitrate/encoding mode set in the video settings
        /// </summary>
        public bool NoTargetSizeMode
        {
            get { return noTargetSizeMode; }
            set { noTargetSizeMode = value; }
        }
        /// <summary>
        /// gets / sets if a fixed bitrate should be used for video encoding
        /// </summary>
        public bool BitrateMode
        {
            get { return bitrateMode; }
            set { bitrateMode = value; }
        }
        /// <summary>
        /// gets / sets if the output should be defined by a filesize
        /// </summary>
        public bool FileSizeMode
        {
            get { return fileSizeMode; }
            set { fileSizeMode = value; }
        }
        /// <summary>
        ///  gets / sets the default container
        /// </summary>
        public string Container
        {
            get { return container; }
            set { container = value; }
        }
        /// <summary>
        ///  gets / sets the device output type
        /// </summary>
        public string DeviceOutputType
        {
            get { return deviceType; }
            set { deviceType = value; }
        }
        /// <summary>
        /// gets / sets the output video bitrate
        /// </summary>
        public int Bitrate
        {
            get { return bitrate; }
            set { bitrate = value; }
        }
        /// <summary>
        /// gets / sets the output filesize
        /// </summary>
        public FileSize? FileSize
        {
            get { return fileSize; }
            set { fileSize = value; }
        }
        /// <summary>
        /// gets / sets the default split size
        /// </summary>
        public FileSize? SplitSize
        {
            get { return splitSize; }
            set { splitSize = value; }
        }
        #endregion
    }
}
