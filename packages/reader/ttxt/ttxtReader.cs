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
    /// Summary description for ttxtReader.
    /// </summary>
    public class ttxtReader
    {
        private string fileName;

        /// <summary>
        /// initializes the ttxt reader
        /// </summary>
        /// <param name="fileName">the ttxt file that this reader will process</param>
        public ttxtReader(string fileName)
        {
            this.fileName = fileName;
        }

        /// <summary>
        /// reads the ttxt file, which is essentially a text file
        /// </summary>
        public bool readFileProperties(string infoFile)
        {
            bool ttxtFileFound = false;
            string line;

            try
            {
                using (StreamReader sr = new StreamReader(infoFile))
                {
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (line.StartsWith("<!-- GPAC 3GPP Text Stream -->"))
                            ttxtFileFound = true;
                    }
                }
            }
            catch (Exception i)
            {
                MessageBox.Show("The following error ocurred when parsing the ttxt file " + infoFile + "\r\n" + i.Message, "Error parsing ttxt file", MessageBoxButtons.OK);
            }

            return ttxtFileFound;
        }
    }
}
