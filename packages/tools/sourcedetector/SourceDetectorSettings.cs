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
using System.Threading;

using MeGUI.core.util;

namespace MeGUI
{
    [LogByMembers]
    public class SourceDetectorSettings
    {
        private double combedFrameMinimum;
        private int minimumUsefulSections;
        private int hybridThreshold;
        private double decimationThreshold;
        private int analysePercent;
        private int minAnalyseSections;
        private int hybridFOPercent;
        private double portionThreshold;
        private bool portionsAllowed;
        private int maxPortions;
        private ThreadPriority priority;
                
        public SourceDetectorSettings()
        {
            combedFrameMinimum = 5.0;
            minimumUsefulSections = 20;
            hybridThreshold = 5;
            decimationThreshold = 2.0;
            portionThreshold = 5.0;
            analysePercent = 0;
            minAnalyseSections = 150;
            hybridFOPercent = 10;
            portionsAllowed = false;
            maxPortions = 5;
            priority = ThreadPriority.BelowNormal;
        }

        public double CombedFrameMinimum
        {
            get { return combedFrameMinimum; }
            set { combedFrameMinimum = value; }
        }

        public int MinimumUsefulSections
        {
            get { return minimumUsefulSections; }
            set { minimumUsefulSections = value; }
        }
        
        public double DecimationThreshold
        {
            get { return decimationThreshold; }
            set { decimationThreshold = value; }
        }

        public int MaxPortions
        {
            get { return maxPortions; }
            set { maxPortions = value; }
        }

        public ThreadPriority Priority
        {
            get { return priority; }
            set { priority = value; }
        }

        public bool PortionsAllowed
	    {
		    get { return portionsAllowed;}
		    set { portionsAllowed = value;}
	    }

        public double PortionThreshold
        {
            get { return portionThreshold; }
            set { portionThreshold = value; }
        }

        public int HybridFOPercent
        {
            get { return hybridFOPercent; }
            set { hybridFOPercent = value; }
        }

        public int MinimumAnalyseSections
        {
            get { return minAnalyseSections; }
            set { minAnalyseSections = value; }
        }

        public int AnalysePercent
        {
            get { return analysePercent; }
            set { analysePercent = value; }
        }
	
        public int HybridPercent
        {
            get { return hybridThreshold; }
            set { hybridThreshold = value; }
        }
    }
}