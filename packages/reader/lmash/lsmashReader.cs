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
using System.IO;

using MeGUI.core.util;

namespace MeGUI
{
    public class lsmashFileFactory : IMediaFileFactory
    {

        #region IMediaFileFactory Members

        public IMediaFile Open(string file)
        {
            if (file.Contains("|"))
                return new lsmashFile(file.Split('|')[0], file.Split('|')[1]);
            else if (file.ToLowerInvariant().EndsWith(".lwi"))
                return new lsmashFile(null, file);
            else
                return new lsmashFile(file, null);
        }

        public int HandleLevel(string file)
        {
            if (file.ToLowerInvariant().EndsWith(".lwi") || File.Exists(file + ".lwi"))
                return 13;
            return -1;
        }

        #endregion

        #region IIDable Members

        public string ID
        {
            get { return "lsmash"; }
        }

        #endregion
    }

    /// <summary>
    /// Summary description for lsmashReader.
    /// </summary>
    public class lsmashFile : IMediaFile
    {
        private AvsFile reader;
        private VideoInformation info;

        /// <summary>
        /// initializes the lsmash reader
        /// </summary>
        /// <param name="fileName">the LSMASHIndex source file file that this reader will process</param>
        /// <param name="indexFile">the LSMASHIndex index file that this reader will process</param>
        public lsmashFile(string fileName, string indexFile)
        {
            MediaInfoFile oInfo = null;
            reader = AvsFile.ParseScript(VideoUtil.getLSMASHVideoInputLine(fileName, indexFile, 0, ref oInfo), true);
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