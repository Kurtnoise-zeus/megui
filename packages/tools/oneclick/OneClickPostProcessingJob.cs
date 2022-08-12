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
using System.Xml.Serialization;

namespace MeGUI
{
    /// <summary>
    /// Container object for OneClickPostProcessing
    /// </summary>
    public class OneClickPostProcessingJob : Job
    {
        private OneClickPostprocessingProperties postprocessingProperties;
        private string strIndexFile;

        public OneClickPostProcessingJob() : this(null, null, null) { }

        public OneClickPostProcessingJob(string input, string strIndexFile, OneClickPostprocessingProperties postprocessingProperties) : base(input, null)
        {
            this.postprocessingProperties = postprocessingProperties;
            this.strIndexFile = strIndexFile;
        }

        public override string CodecString
        {
            get { return "oneclick"; }
        }

        public override string EncodingMode
        {
            get { return "oneclick"; }
        }

        /// <summary>
        /// gets / sets the postprocessing properties
        /// and all the postprocessing that has to be done prior to audio encoding
        /// is defined in this property
        /// </summary>
        public OneClickPostprocessingProperties PostprocessingProperties
        {
            get { return postprocessingProperties; }
            set { postprocessingProperties = value; }
        }

        public string IndexFile
        {
            get { return strIndexFile; }
            set { strIndexFile = value; }
        }
    }
}
