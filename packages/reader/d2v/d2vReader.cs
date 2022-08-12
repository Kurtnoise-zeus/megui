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

namespace MeGUI
{
	public enum FieldOperation: int {NONE = 0, FF, RAW};

    public class d2vFileFactory : IMediaFileFactory
    {
        #region IMediaFileFactory Members
        public IMediaFile Open(string file)
        {
            return new d2vFile(file);
        }

        public int HandleLevel(string file)
        {
            if (file.ToLowerInvariant().EndsWith(".d2v"))
                return 18;
            return -1;
        }
        #endregion

        #region IIDable Members
        public string ID
        {
            get { return "d2v"; }
        }
        #endregion
    }
	
    /// <summary>
	/// Summary description for d2vReader.
	/// </summary>
	public class d2vFile : IMediaFile
	{
        private AvsFile reader;
        private string fileName;
        private int fieldOperation;
        private VideoInformation info;
        private double filmPercentage;

        private static readonly System.Text.RegularExpressions.Regex r =
            new System.Text.RegularExpressions.Regex("(?<=FINISHED +)[0-9.]+(?=% FILM)");

        public static double GetFilmPercent(string file)
        {
            double filmPercentage = -1.0;
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
		/// initializes the d2v reader
		/// </summary>
		/// <param name="fileName">the dvd2avi project file that this reader will process</param>
        public d2vFile(string fileName)
		{
            UpdateCacher.CheckPackage("dgindex");
			this.fileName = fileName;
            string strPath = Path.GetDirectoryName(MainForm.Instance.Settings.DGIndex.Path);
            string strDLL = Path.Combine(strPath, "DGDecode.dll");
            reader = AvsFile.ParseScript("LoadPlugin(\"" + strDLL + "\")\r\nDGDecode_Mpeg2Source(\"" + this.fileName + "\")", true);
            this.readFileProperties();
        }

		/// <summary>
		/// reads the d2v file, which is essentially a text file
        /// the first few lines contain the video properties in plain text and the 
		/// last line contains the film percentage
		/// this method reads all this information and stores it internally, then 
		/// closes the d2v file again
		/// </summary>
		private void readFileProperties()
		{
            info = reader.VideoInfo.Clone();
            using (StreamReader sr = new StreamReader(fileName))
            {
                int iLineCount = 0;
				string line = null;
				while ((line = sr.ReadLine()) != null)
				{
                    iLineCount++;
                    if (iLineCount == 3)
                    {
                        string strSourceFile = line;
                        if (File.Exists(strSourceFile))
                        {
                            MediaInfoFile oInfo = new MediaInfoFile(strSourceFile);
                            info.DAR = oInfo.VideoInfo.DAR;
                        }
                    }
					if (line.IndexOf("Field_Operation") != -1)
					{
						string fieldOp = line.Substring(16, 1);
						this.fieldOperation = Int32.Parse(fieldOp);
					}
					else if (line.IndexOf("FINISHED") != -1 && line.IndexOf("FILM") != -1) // dgindex now reports VIDEO % if it's > 50%
					{
						int end = line.IndexOf("%");
						string percentage = line.Substring(10, end - 10);
						this.filmPercentage = Double.Parse(percentage, System.Globalization.CultureInfo.InvariantCulture);
					}
				}
			}
		}

		#region properties
        public VideoInformation VideoInfo
        {
            get { return info; }
        }

		/// <summary>
		/// returns the percentage of film of this source
		/// </summary>
		public double FilmPercentage
		{
			get {return this.filmPercentage;}
		}

		/// <summary>
		/// returns the field operation performed on this source
		/// </summary>
		public int FieldOperation
		{
			get {return this.fieldOperation;}
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