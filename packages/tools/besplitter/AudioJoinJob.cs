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
using System.Diagnostics;
using System.IO;
using System.Text;

namespace MeGUI.packages.tools.besplitter
{
    public class AudioJoinJob : Job
    {
        private TimeSpan cliplength;

        public TimeSpan ClipLength
        {
            get { return cliplength; }
            set { cliplength = value; }
        }


        private string[] inputFiles;

        public string[] InputFiles
        {
            get { return inputFiles; }
            set { inputFiles = value; }
        }

        public string generateJoinCommandline(string tempFilename)
        {
            FilesToDelete.Add(tempFilename);
            return string.Format("-core ( -input \"{0}\" -prefix \"{1}\" -type lst -join )",
                tempFilename, Output);
        }

        public AudioJoinJob() { }

        public AudioJoinJob(string[] inputFiles, string output)
        {
            Debug.Assert(inputFiles.Length > 0);
            this.inputFiles = inputFiles;
            this.Output = output;
            this.Input = inputFiles[0];
        }

        public override string CodecString
        {
            get { return "cut"; }
        }

        public override string EncodingMode
        {
            get { return "join"; }
        }

    }
}
