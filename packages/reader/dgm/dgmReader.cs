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
using System.Text.RegularExpressions;

using MeGUI.core.util;

namespace MeGUI
{
    public class dgmFileFactory : IMediaFileFactory
    {

        #region IMediaFileFactory Members

        public IMediaFile Open(string file)
        {
            return new dgmFile(file);
        }

        public int HandleLevel(string file)
        {
            if (!file.ToLowerInvariant().EndsWith(".dgi"))
                return -1;

            if (FileIndexerWindow.isDGMFile(file))
                return 20;
            return -1;
        }

        #endregion

        #region IIDable Members

        public string ID
        {
            get { return "dgm"; }
        }

        #endregion
    }

    /// <summary>
    /// Summary description for dgiReader.
    /// </summary>
    public class dgmFile : IMediaFile
    {
        private AvsFile reader;
        private string fileName;
        private VideoInformation info;

        /// <summary>
        /// initializes the dgi reader
        /// </summary>
        /// <param name="fileName">the DGNVIndex project file that this reader will process</param>
        public dgmFile(string fileName)
        {
            this.fileName = fileName;
            UpdateCacher.CheckPackage("dgindexim");

            string strScript = "";
            string strPath = Path.GetDirectoryName(MainForm.Instance.Settings.DGIndexIM.Path);
            strScript = "LoadPlugin(\"" + Path.Combine(strPath, "DGDecodeIM.dll") + "\")\r\nDGSourceIM(\"" + this.fileName + "\", silent=true)";
            reader = AvsFile.ParseScript(strScript, true);

            this.readFileProperties();
        }

        private static readonly Regex r = new Regex("[0-9.]+(?=% FILM)");

        public static double GetFilmPercent(string file)
        {
            double filmPercentage = -1.0;

            if (String.IsNullOrEmpty(file))
                return filmPercentage;

            using (StreamReader sr = new StreamReader(file))
            {
                string line = sr.ReadLine();
                while ((line = sr.ReadLine()) != null)
                    if (r.IsMatch(line))
                        filmPercentage = double.Parse(r.Match(line).Value,
                            System.Globalization.CultureInfo.InvariantCulture);
            }
            return filmPercentage;
        }

        /// <summary>
        /// reads the dgi file, which is essentially a text file
        /// </summary>
        private void readFileProperties()
        {
            info = reader.VideoInfo.Clone();
            using (StreamReader sr = new StreamReader(fileName))
            {
                string line = null;
                int iLineCount = 0;
                while ((line = sr.ReadLine()) != null)
                {
                    if (iLineCount == 3)
                    {
                        string strSourceFile = line.Substring(0, line.LastIndexOf(" "));
                        strSourceFile = Path.Combine(Path.GetDirectoryName(fileName), strSourceFile);
                        if (File.Exists(strSourceFile))
                        {
                            MediaInfoFile oInfo = new MediaInfoFile(strSourceFile);
                            info.DAR = oInfo.VideoInfo.DAR;
                        }
                        break;
                    }
                    iLineCount++;
                }
            }
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