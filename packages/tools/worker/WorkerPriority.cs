// ****************************************************************************
// 
// Copyright (C) 2005-2017 Doom9 & al
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
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace MeGUI
{
    public enum WorkerPriorityType : int { IDLE = 0, BELOW_NORMAL, NORMAL, ABOVE_NORMAL };

    public class WorkerPriority
    {
        private JobType _jobType;
        private WorkerPriorityType _priority;
        private bool _lowIOPriority;

        public WorkerPriority() : this(JobType.Audio, WorkerPriorityType.IDLE, false)
        {

        }

        public WorkerPriority(JobType _jobType, WorkerPriorityType _priority, bool _lowIOPriority)
        {
            this._jobType = _jobType;
            this._priority = _priority;
            this._lowIOPriority = _lowIOPriority;
        }

        public JobType JobType
        {
            get { return _jobType; }
            set { _jobType = value; }
        }

        public WorkerPriorityType Priority
        {
            get { return _priority; }
            set { _priority = value; }
        }

        public bool LowIOPriority
        {
            get { return _lowIOPriority; }
            set { _lowIOPriority = value; }
        }

        public static void GetJobPriority(Job oJob, out WorkerPriorityType oPriority, out bool lowIOPriority)
        {
            JobType oType = WorkerSettings.GetJobType(oJob);
            foreach (WorkerPriority oPrioritySettings in MainForm.Instance.Settings.WorkerPriority)
            {
                if (oPrioritySettings.JobType.Equals(oType))
                {
                    oPriority = oPrioritySettings.Priority;
                    lowIOPriority = oPrioritySettings.LowIOPriority;
                    return;
                }
            }
            oPriority = WorkerPriorityType.IDLE;
            lowIOPriority = true;
        }
    }
}