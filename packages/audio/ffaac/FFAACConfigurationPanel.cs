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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MeGUI.packages.audio.ffaac
{
    public partial class FFAACConfigurationPanel : MeGUI.core.details.audio.AudioConfigurationPanel, Editable<FFAACSettings>
    {
        public FFAACConfigurationPanel()
            : base()
        {
            InitializeComponent();
            cbMode.Items.AddRange(EnumProxy.CreateArray(FFAACSettings.SupportedModes));
            cbProfile.Items.AddRange(EnumProxy.CreateArray(FFAACSettings.SupportedProfiles));
        }

        #region properties
        private int bitrate;
        private int quality;
        /// <summary>
        /// gets / sets the settings that are being shown in this configuration dialog
        /// </summary>
        protected override AudioCodecSettings CodecSettings
        {
            get
            {
                FFAACSettings nas = new FFAACSettings();
                switch ((FFAACMode)(cbMode.SelectedItem as EnumProxy).RealValue)
                {
                    case FFAACMode.VBR: nas.BitrateMode = BitrateManagementMode.VBR; break;
                    case FFAACMode.CBR: nas.BitrateMode = BitrateManagementMode.CBR; break;
                    default: nas.BitrateMode = BitrateManagementMode.CBR; break;
                }
                nas.Mode = (FFAACMode)(cbMode.SelectedItem as EnumProxy).RealValue;
                nas.Profile = (FFAACProfile)(cbProfile.SelectedItem as EnumProxy).RealValue;
                if (nas.Mode == FFAACMode.CBR)
                    nas.Bitrate = trackBar.Value;
                else
                    nas.Quality = trackBar.Value ;
                return nas;
            }
            set
            {
                FFAACSettings nas = value as FFAACSettings;
                bitrate = nas.Bitrate;
                quality = nas.Quality;
                cbMode.SelectedItem = EnumProxy.Create(nas.Mode);
                cbProfile.SelectedItem = EnumProxy.Create(nas.Profile);
                if (nas.Mode == FFAACMode.CBR)
                    trackBar.Value = Math.Max(Math.Min(nas.Bitrate, trackBar.Maximum), trackBar.Minimum);
                else
                    trackBar.Value = Math.Max(Math.Min(nas.Quality, trackBar.Maximum), trackBar.Minimum);
            }
        }
        #endregion

        #region Editable<FFAACSettings> Members

        FFAACSettings Editable<FFAACSettings>.Settings
        {
            get
            {
                return (FFAACSettings)Settings;
            }
            set
            {
                Settings = value;
            }
        }

        #endregion

        private void cbMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch ((FFAACMode)(cbMode.SelectedItem as EnumProxy).RealValue)
            {
                case FFAACMode.VBR:
                    trackBar.Visible = true;
                    label4.Visible = false;
                    trackBar.Minimum = 1;
                    trackBar.Maximum = 100;
                    trackBar.TickFrequency = 5;
                    trackBar.Value = quality;
                    break;
                case FFAACMode.CBR:
                    trackBar.Visible = true;
                    label4.Visible = false;
                    trackBar.Minimum = 32;
                    trackBar.Maximum = 528;
                    trackBar.TickFrequency = 16;
                    trackBar.Value = bitrate;
                    break;
            }
            //trackBar_ValueChanged(sender, e);
        }

        private void trackBar_ValueChanged(object sender, EventArgs e)
        {
            switch ((FFAACMode)(cbMode.SelectedItem as EnumProxy).RealValue)
            {
                case FFAACMode.VBR:
                    encoderGroupBox.Text = String.Format(" FFMpeg AAC Options - Variable Bitrate @ Quality = {0} ", trackBar.Value);
                    quality = trackBar.Value;
                    break;
                case FFAACMode.CBR:
                    encoderGroupBox.Text = String.Format(" FFMpeg AAC Options - Constant Bitrate  @ {0} kbit/s ", trackBar.Value);
                    bitrate = trackBar.Value;
                    break;
            }
        }
    }
}




