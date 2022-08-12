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
using System.Xml.Serialization;

using MeGUI.core.details;
using MeGUI.core.plugins.interfaces;
using MeGUI.core.util;
using MeGUI.packages.tools.besplitter;

namespace MeGUI
{
    public class JobID
    {
        private static readonly Random gen = new Random();

        private string name;
        public string Name
        {
            get { return name; }
        }
        private int uniqueID;
        public int UniqueID
        {
            get { return uniqueID; }
        }

        public JobID(string name)
        {
            for (int i = 0; i < name.Length; i++)
                if (!(Char.IsLetterOrDigit(name[i]) || name[i] == '_'))
                    throw new MeGUIException("The name must be alphanumeric, including underscores.");
            
            this.name = name;
            this.uniqueID = gen.Next();
        }

        public override string ToString()
        {
            return name + " " + uniqueID.ToString();
        }

        public override bool Equals(object obj)
        {
            JobID other = obj as JobID;
            if (other == null) return false;

            return (name == other.name && uniqueID == other.uniqueID);
        }

        public override int GetHashCode()
        {
            return name.GetHashCode() ^ uniqueID.GetHashCode();
        }
    }

    public enum JobStatus: int { WAITING = 0, PROCESSING, POSTPONED, ERROR, ABORTED, DONE, SKIP, ABORTING, PAUSED };
    // status of job, 0: waiting, 1: processing, 2: postponed, 3: error, 4: aborted, 5: done, 6: skip, 7: aborting, 8: paused
	
    
    /// <summary>
	/// This represents an un-identifiable job. It only has information about how to complete
    /// it and no way to say *which specific job* it is in the queue. Furthermore, it doesn't
    /// have any extraneous information about its storage in the queue. That information is held
    /// in the TaggedJob class.
	/// </summary>
	[XmlInclude(typeof(VideoJob)), XmlInclude(typeof(AudioJob)), XmlInclude(typeof(MuxJob)), 
    XmlInclude(typeof(D2VIndexJob)), XmlInclude(typeof(AviSynthJob)), XmlInclude(typeof(MP4FpsModJob)),
    XmlInclude(typeof(SubtitleIndexJob)), XmlInclude(typeof(AudioSplitJob)), XmlInclude(typeof(AudioJoinJob)),
    XmlInclude(typeof(CleanupJob)), XmlInclude(typeof(DGMIndexJob)), XmlInclude(typeof(FFMSIndexJob)),
    XmlInclude(typeof(DGIIndexJob)), XmlInclude(typeof(HDStreamsExJob)), XmlInclude(typeof(PgcDemuxJob)),
    XmlInclude(typeof(MkvExtractJob)), XmlInclude(typeof(OneClickPostProcessingJob)), XmlInclude(typeof(LSMASHIndexJob))]
	public abstract class Job
	{
        #region important details
        public string Input;
        public string Output;
        public List<string> FilesToDelete;
        #endregion

        #region init
        public Job():this(null, null)
		{
        }

        public Job(string input, string output)
        {
            Input = input;
            Output = output;
            if (!string.IsNullOrEmpty(input) && input == output)
                throw new MeGUIException("Input and output files may not be the same");

            FilesToDelete = new List<string>();
        }
        #endregion

        #region queue display details
        /// <summary>
        /// Codec used
        /// </summary>
        public abstract string CodecString
        {
            get;
        }

        /// <summary>
        /// New meaning: Encoding mode means job type: video/audio/mux/index/split/etc
        /// </summary>
        public abstract string EncodingMode
        {
            get;
        }

        #endregion
    }
}
