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
using System.IO;
using System.Text;
using System.Windows.Forms;

using MeGUI.core.util;

namespace MeGUI
{
    /// <summary>
	/// Summary description for idxReader.
	/// </summary>
    public class idxReader
    {
        private string fileName;
        
        /// <summary>
		/// initializes the idx reader
		/// </summary>
		/// <param name="fileName">the idx file that this reader will process</param>
        public idxReader(string fileName)
		{
			this.fileName = fileName;
        }

		/// <summary>
		/// reads the idx file, which is essentially a text file
		/// the first few lines contain the video properties in plain text and the 
		/// last line contains index, language and timestamp from subtitles
		/// this method reads indexes and languages and store it internally, then 
		/// closes the idx file again
		/// </summary>
        public static void readFileProperties(string infoFile, out List<SubtitleInfo> subtitles)
        {
            subtitles = new List<SubtitleInfo>();
            string lng = "";
            int idx = 0;
            SubtitleInfo si;
            bool bNewSubIDFound = false;
            string line;

            try
            {
                using (StreamReader sr = new StreamReader(infoFile))
                {
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (line.StartsWith("id")) // Language & Index values found
                        {
                            lng = line.Substring(4, 2);
                            idx = Convert.ToInt32(line.Substring(15, 1));
                            bNewSubIDFound = true;
                        }

                        if (line.StartsWith("timestamp: ") && bNewSubIDFound) // to ensure to have a sub, not just an idx/lng
                        {
                            si = new SubtitleInfo(lng, idx);
                            subtitles.Add(si);
                            bNewSubIDFound = false;
                        }                          
                    }
                }
            }
            catch (Exception i)
            {
                MessageBox.Show("The following error ocurred when parsing the idx file " + infoFile + "\r\n" + i.Message, "Error parsing idx file", MessageBoxButtons.OK);
            }
        }

        /// <summary>
        /// reads the idx file to retrieve the default Language Index Value
        /// </summary>
        public static int defaultLangIdx(string idxFile)
        {
            int idx = 0;

            try
            {
                using (StreamReader sr = new StreamReader(idxFile))
                {
                    string line = sr.ReadLine();
                    while (line != null)
                    {
                        line = sr.ReadLine();
                        if (line != null)
                        {
                            if (line.StartsWith("langidx:"))
                                idx = Convert.ToInt32(line.Substring(9));
                        }
                    }
                }
            }
            catch (Exception i)
            {
                MessageBox.Show("The following error ocurred when parsing the idx file " + idxFile + "\r\n" + i.Message, "Error parsing idx file", MessageBoxButtons.OK);
            }

            return idx;
        }

    }
}
