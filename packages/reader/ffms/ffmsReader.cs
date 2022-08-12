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
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Xml.Serialization;
using System.Windows.Forms;

using MeGUI.core.util;

namespace MeGUI
{
    public class ffmsFileFactory : IMediaFileFactory
    {

        #region IMediaFileFactory Members

        public IMediaFile Open(string file)
        {
            if (file.Contains("|"))
                return new ffmsFile(file.Split('|')[0], file.Split('|')[1]);
            else if (file.ToLowerInvariant().EndsWith(".ffindex"))
                return new ffmsFile(null, file);
            else
                return new ffmsFile(file, null);
        }

        public int HandleLevel(string file)
        {
            if (file.ToLowerInvariant().EndsWith(".ffindex") || File.Exists(file + ".ffindex"))
                return 12;
            return -1;
        }

        #endregion

        #region IIDable Members

        public string ID
        {
            get { return "ffms"; }
        }

        #endregion
    }

    /// <summary>
    /// Summary description for ffmsReader.
    /// </summary>
    public class ffmsFile : IMediaFile
    {
        private AvsFile reader;
        private string fileName;
        private VideoInformation info;

        /// <summary>
        /// initializes the ffms reader
        /// </summary>
        /// <param name="fileName">the FFMSIndex source file file that this reader will process</param>
        /// <param name="indexFile">the FFMSIndex index file that this reader will process</param>
        public ffmsFile(string fileName, string indexFile)
        {
            if (!String.IsNullOrEmpty(indexFile) && String.IsNullOrEmpty(fileName))
            {
                this.fileName = indexFile.Substring(0, indexFile.Length - 8);
                indexFile = null;
            }
            else
                this.fileName = fileName;

            double fps = 0;
            MediaInfoFile oInfo = null;
            if (File.Exists(this.fileName))
            {
                oInfo = new MediaInfoFile(this.fileName);
                if (oInfo.VideoInfo.HasVideo && oInfo.VideoInfo.FPS > 0)
                    fps = oInfo.VideoInfo.FPS;
            }

            reader = AvsFile.ParseScript(VideoUtil.getFFMSVideoInputLine(this.fileName, indexFile, fps), true);
            info = reader.VideoInfo.Clone();
            if (oInfo != null)
                info.DAR = oInfo.VideoInfo.DAR;
        }

        #region properties
        public VideoInformation VideoInfo
        {
            get { return info; }
        }
        #endregion

        #region IMediaFile Members

        public bool CanReadVideo
        {
            get { return reader.CanReadVideo; }
        }

        public bool CanReadAudio
        {
            get { return false; }
        }

        public IVideoReader GetVideoReader()
        {
            return reader.GetVideoReader();
        }

        public IAudioReader GetAudioReader(int track)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            if (reader != null)
                reader.Dispose();
        }

        #endregion
    }
}

