// ****************************************************************************
// 
// Copyright (C) 2005-2026 Doom9 & al
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
            BuildModernLayout();
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

        /// <summary>
        /// Windows 11 settings look: the controls are moved into rounded cards (see MeGUI.core.gui.ModernDialog)
        /// </summary>
        private void BuildModernLayout()
        {
            MeGUI.core.gui.ModernDialog d = new MeGUI.core.gui.ModernDialog(this);
            d.Section("Analysis");
            d.Row(MeGUI.core.gui.ModernDialog.Cap(analysisPercentLabel.Text), null, d.Wide(analysisPercent, 110));
            d.Row(MeGUI.core.gui.ModernDialog.Cap(minAnalyseSectionsLabel.Text), null, d.Wide(minAnalyseSections, 110));
            d.Section("Hybrid detection");
            d.Row(MeGUI.core.gui.ModernDialog.Cap(hybridThresholdLabel.Text), null, d.Wide(hybridThreshold, 110));
            d.Row(MeGUI.core.gui.ModernDialog.Cap(hybridFOThresholdLabel.Text), null, d.Wide(hybridFOThreshold, 110));
            d.Section("Portions");
            d.Toggle(portionsAllowed, null);
            d.Row(MeGUI.core.gui.ModernDialog.Cap(portionThresholdLabel.Text), null, d.Wide(portionThreshold, 110));
            d.Row(MeGUI.core.gui.ModernDialog.Cap(maximumPortionsLabel.Text), null, d.Wide(maximumPortions, 110));
            d.Section("Process");
            d.Row(MeGUI.core.gui.ModernDialog.Cap(priorityLabel.Text), null, d.Wide(priority, 160));
            d.Finish(helpButton1, 620, saveButton, cancelButton);
        }
    }
}