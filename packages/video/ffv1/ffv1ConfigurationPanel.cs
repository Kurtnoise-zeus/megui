// ****************************************************************************
// 
// Copyright (C) 2005-2025 Doom9 & al
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
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;

using MeGUI.core.details.video;
using MeGUI.core.gui;
using MeGUI.core.plugins.interfaces;

namespace MeGUI.packages.video.ffv1
{
    public partial class ffv1ConfigurationPanel : MeGUI.core.details.video.VideoConfigurationPanel, Editable<ffv1Settings>
    {
        private XmlDocument ContextHelp = new XmlDocument();
        public ffv1ConfigurationPanel()
            : base()
        {
            InitializeComponent();
            ffv1Coder.SelectedIndex = 1;
            ffv1Slices.SelectedIndex = 0;
            ffv1EncodingMode.SelectedIndex = 0;
        }

        /// <summary>
        /// Returns whether the given mode is a bitrate or quality-based mode
        /// </summary>
        /// <param name="mode">selected encoding mode</param>
        /// <returns>true if the mode is a bitrate mode, false otherwise</returns>
        private bool isBitrateMode(VideoCodecSettings.VideoEncodingMode mode)
        {
            return !(mode == VideoCodecSettings.VideoEncodingMode.none);
        }
        private void doDropDownAdjustments()
        {
            lastFFV1EncodingMode = (VideoCodecSettings.FFV1EncodingMode)this.ffv1EncodingMode.SelectedIndex;
        }

        protected override string getCommandline()
        {
            ulong x = 1;
            return ffv1Encoder.genCommandline(null, null, null, -1, -1, ref x, Settings as ffv1Settings, null);
        }
        /// <summary>
        /// Does all the necessary adjustments after a GUI change has been made.
        /// </summary>
        protected override void doCodecSpecificAdjustments()
        {
            doDropDownAdjustments();
        }

        /// <summary>
        /// Returns whether settings is ffv1Settings
        /// </summary>
        /// <param name="settings">The settings to check</param>
        /// <returns>Whether the settings are valid</returns>
        protected override bool isValidSettings(VideoCodecSettings settings)
        {
            return settings is ffv1Settings;
        }

        /// <summary>
        /// Returns a new instance of ffv1Settings.
        /// </summary>
        /// <returns>A new instance of ffv1Settings</returns>
        protected override VideoCodecSettings defaultSettings()
        {
            return new ffv1Settings();
        }

        /// <summary>
        /// gets / sets the settings currently displayed on the GUI
        /// </summary>
        public ffv1Settings Settings
        {
            get
            {
                ffv1Settings xs = new ffv1Settings();
                xs.Coder = ffv1Coder.SelectedIndex;
                xs.Slices = ffv1Slices.SelectedIndex;
                xs.FFV110Bits = ch10BitsEncoder.Checked;
                xs.NbThreads = (int)ffv1NbThreads.Value;
                xs.GOPSize = (int)nmGOPSize.Value;
                xs.ErrorCorrection = chErrorCorrection.Checked;
                xs.Context = chContext.Checked ? 1 :0;
                xs.FFV1EncodingType = (VideoCodecSettings.FFV1EncodingMode)this.ffv1EncodingMode.SelectedIndex;
                return xs;
            }
            set
            {
                // Warning! The ordering of components matters because of the dependency code!
                if (value == null)
                    return;

                ffv1Settings xs = value;
                updating = true;
                lastEncodingMode = xs.VideoEncodingType;
                ffv1Coder.SelectedIndex = xs.Coder;
                ffv1Slices.SelectedIndex = xs.Slices;
                ch10BitsEncoder.Checked = xs.FFV110Bits;
                ffv1NbThreads.Value = xs.NbThreads;
                nmGOPSize.Value = xs.GOPSize;
                chErrorCorrection.Checked = xs.ErrorCorrection;
                chContext.Checked = xs.Context == 1 ? true : false;
                ffv1EncodingMode.SelectedIndex = (int)xs.FFV1EncodingType;
                updating = false;
                genericUpdate();
            }
        }
        private void updateEvent(object sender, EventArgs e)
        {
            genericUpdate();
        }
        private void textField_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && (int)Keys.Back != (int)e.KeyChar)
                e.Handled = true;
        }

        private string SelectHelpText(string node)
        {
            StringBuilder HelpText = new StringBuilder(64);

            try
            {
                string xpath = "/ContextHelp/Codec[@name='FFV1']/" + node;
                XmlNodeList nl = ContextHelp.SelectNodes(xpath); // Return the details for the specified node

                if (nl.Count == 1) // if it finds the required HelpText, count should be 1
                {
                    HelpText.Append(nl[0].Attributes["name"].Value);
                    HelpText.AppendLine();
                    HelpText.AppendLine(nl[0]["Text"].InnerText);
                    HelpText.AppendLine();
                    HelpText.AppendLine("Default : " + nl[0]["Default"].InnerText);
                    HelpText.AppendLine("Recommended : " + nl[0]["Recommended"].InnerText);
                }
                else // If count isn't 1, then theres no valid data.
                    HelpText.Append("Error: No data available");
            }
            catch
            {
                HelpText.Append("Error: No data available");
            }

            return (HelpText.ToString());
        }

        private void chMultiPass_CheckedChanged(object sender, EventArgs e)
        {
           updateEvent(sender, e);
        }

        private void chErrorCorrection_CheckedChanged(object sender, EventArgs e)
        {
           updateEvent(sender, e);
        }

        private void chContext_CheckedChanged(object sender, EventArgs e)
        {
            updateEvent(sender, e);
        }

        private void ch10BitsEncoder_CheckedChanged(object sender, EventArgs e)
        {
            updateEvent(sender, e);
        }

        private void ffv1Coder_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateEvent(sender, e);
        }

        private void ffv1Slices_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateEvent(sender, e);
        }

        private void nmGOPSize_ValueChanged(object sender, EventArgs e)
        {
            updateEvent(sender, e);
        }

        private void ffv1NbThreads_ValueChanged(object sender, EventArgs e)
        {
            updateEvent(sender, e);
        }

        private void ffv1EncodingMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            genericUpdate();
        }
    }
}