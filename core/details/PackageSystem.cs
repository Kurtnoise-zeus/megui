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

using MeGUI.core.plugins.interfaces;

namespace MeGUI.core.details
{

    /// <summary>
    /// Shows a dialog (normally) which allows the user to view the job's settings
    /// and reconfigure them. If a dialog is shown, the reconfigured job must be returned.
    /// 
    /// If null is returned, this is taken to mean that the job configurer is 
    /// not valid for this type of job.
    /// </summary>
    /// <param name="j"></param>
    /// <returns></returns>
    public delegate Job ReconfigureJob(Job j);

    public class PackageSystem 
    {
        GenericRegisterer<ITool> tools = new GenericRegisterer<ITool>();
        GenericRegisterer<IMediaFileFactory> mediaFileTypes = new GenericRegisterer<IMediaFileFactory>();
        GenericRegisterer<IMuxing> muxers = new GenericRegisterer<IMuxing>();
        GenericRegisterer<JobPreProcessor> jobPreProcessors = new GenericRegisterer<JobPreProcessor>();
        GenericRegisterer<JobPostProcessor> jobPostProcessors = new GenericRegisterer<JobPostProcessor>();
        GenericRegisterer<JobProcessorFactory> jobProcessors = new GenericRegisterer<JobProcessorFactory>();
        
        public GenericRegisterer<IDable<ReconfigureJob>> JobConfigurers = new GenericRegisterer<IDable<ReconfigureJob>>();

        public GenericRegisterer<ITool> Tools
        {
            get { return tools; }
        }
        public GenericRegisterer<IMediaFileFactory> MediaFileTypes
        {
            get { return mediaFileTypes; }
        }
        public GenericRegisterer<IMuxing> MuxerProviders
        {
            get { return muxers; }
        }
        public GenericRegisterer<JobPreProcessor> JobPreProcessors
        {
            get { return jobPreProcessors; }
        }
        public GenericRegisterer<JobPostProcessor> JobPostProcessors
        {
            get { return jobPostProcessors; }
        }
        public GenericRegisterer<JobProcessorFactory> JobProcessors
        {
            get { return jobProcessors; }
        }
    }
}