// ****************************************************************************
// 
// Copyright (C) 2005-2024 Doom9 & al
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
using System.ComponentModel;
using System.Xml.Serialization;

using MeGUI.core.details;
using MeGUI.packages.tools.calculator;

namespace MeGUI.core.util
{
    public class BitrateCalculationInfo
    {
        public string[] _VideoJobNames
        {
            get
            {
                return JobControl.ToStringList(VideoJobs).ToArray();
            }
            set 
            {
                _videoJobNames = value;
            }
        }
        private string[] _videoJobNames;

        private List<TaggedJob> videoJobs;
        [XmlIgnore]
        public List<TaggedJob> VideoJobs
        {
            get
            {
                if (videoJobs == null && _videoJobNames != null)
                    videoJobs = MainForm.Instance.Jobs.ToJobList(_videoJobNames);
                return videoJobs;
            }
            set
            {
                videoJobs = value;
            }
        }

        private FileSize desiredSize;
        private List<string> audioFiles;


        [XmlIgnore]
        private ContainerType container;

        public string _ContainerName
        {
            get { return Container.ID; }
            set { Container = ContainerType.ByName(value); }
        }

        public ContainerType Container { get => container; set => container = value; }
        public List<string> AudioFiles { get => audioFiles; set => audioFiles = value; }
        public FileSize DesiredSize { get => desiredSize; set => desiredSize = value; }
    }
}