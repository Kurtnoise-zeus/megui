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

using MeGUI;

namespace MeGUI.core.plugins.implemented
{
    public class JobProcessorManager<TJob> : GenericRegisterer<JobProcessorFactory>
        where TJob : Job
    {
        public IJobProcessor CreateProcessor(MainForm info, Job job)
        {
            Dictionary<JobProcessorFactory, IJobProcessor> processors = new Dictionary<JobProcessorFactory,IJobProcessor>();
            foreach (JobProcessorFactory factory in this.Values)
            {
                IJobProcessor processor = factory.Factory(info, job);
                if (processor != null)
                    processors.Add(factory, processor);
            }
            return null;
        }
    }
}
