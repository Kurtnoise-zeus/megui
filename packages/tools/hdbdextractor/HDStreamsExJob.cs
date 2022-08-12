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
    public class HDStreamsExJob : Job
    {
        private string featureNb;
        private string args;
        private int inputType;
        private List<string> source;

        public HDStreamsExJob()
            : base()
        {
            featureNb = "";
            args = "";
            inputType = 1;
            source = new List<string>();
        }

        public HDStreamsExJob(List<string> input, string ouput, string featureNb, string args, int inputType)
        {
            Input = input[0];
            source = input;
            Output = ouput;
            FeatureNb = featureNb;
            Args = args;
            InputType = inputType;
        }

        public string FeatureNb
        {
            get { return featureNb; }
            set { featureNb = value; }
        }

        public string Args
        {
            get { return args; }
            set { args = value; }
        }

        public List<string> Source
        {
            get { return source; }
            set { source = value; }
        }

        // 1 = Folder as Input
        // 2 = File(s) as Input
        public int InputType
        {
            get { return inputType; }
            set { inputType = value; }
        }

        public override string CodecString
        {
            get { return "eac3to"; }
        }

        public override string EncodingMode
        {
            get { return "ext"; }
        }
    }
}
