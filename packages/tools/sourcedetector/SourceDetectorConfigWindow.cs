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
using System.Threading;
using System.Windows.Forms;

namespace MeGUI
{
    public partial class SourceDetectorConfigWindow : Form
    {
        public SourceDetectorConfigWindow()
        {
            InitializeComponent();
        }

        private void portionsAllowed_CheckedChanged(object sender, EventArgs e)
        {
            portionThreshold.Enabled = portionsAllowed.Checked;
            maximumPortions.Enabled = portionsAllowed.Checked;
        }

        public SourceDetectorSettings Settings
        {
            get
            {
                SourceDetectorSettings settings = new SourceDetectorSettings();
                settings.AnalysePercent = (int)analysisPercent.Value;
                settings.HybridFOPercent = (int)hybridFOThreshold.Value;
                settings.HybridPercent = (int)hybridThreshold.Value;
                settings.MinimumAnalyseSections = (int)minAnalyseSections.Value;
                settings.PortionsAllowed = portionsAllowed.Checked;
                if (settings.PortionsAllowed)
                {
                    settings.PortionThreshold = (double)portionThreshold.Value;
                    settings.MaxPortions = (int)maximumPortions.Value;
                }
                settings.Priority = (ThreadPriority)priority.SelectedIndex;
                return settings;
            }
            set
            {
                analysisPercent.Value = value.AnalysePercent;
                hybridFOThreshold.Value = value.HybridFOPercent;
                hybridThreshold.Value = value.HybridPercent;
                minAnalyseSections.Value = value.MinimumAnalyseSections;
                portionsAllowed.Checked = value.PortionsAllowed;
                portionThreshold.Value = (decimal)value.PortionThreshold;
                maximumPortions.Value = value.MaxPortions;
                priority.SelectedIndex = (int)value.Priority;
            }
        }
    }
}