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


using MeGUI.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace MeGUI.packages.audio.exhale
{
    public partial class ExhaleConfigurationPanel : MeGUI.core.details.audio.AudioConfigurationPanel, Editable<ExhaleSettings>
    {
        public ExhaleConfigurationPanel()
            : base()
        {
            InitializeComponent();
            cbProfile.Items.AddRange(EnumProxy.CreateArray(ExhaleSettings.SupportedProfiles));
        }

        private int quality;
        /// <summary>
        /// gets / sets the settings that are being shown in this configuration dialog
        /// </summary>
        protected override AudioCodecSettings CodecSettings
        {
            get
            {
                ExhaleSettings nas = new ExhaleSettings();
                //switch ((ExhaleMode)(cbMode.SelectedItem as EnumProxy).RealValue)
                //{
                //    case ExhaleMode.VBR: nas.BitrateMode = BitrateManagementMode.VBR; break;
                //    case ExhaleMode.CBR: nas.BitrateMode = BitrateManagementMode.CBR; break;
                //    default: nas.BitrateMode = BitrateManagementMode.CBR; break;
                //}
                //nas.Mode = (ExhaleMode)(cbMode.SelectedItem as EnumProxy).RealValue;
                nas.Profile = (ExhaleProfile)(cbProfile.SelectedItem as EnumProxy).RealValue;
                //if (nas.Mode == ExhaleMode.CBR)
                //    nas.Bitrate = trackBar.Value;
                //else
                //    nas.Quality = trackBar.Value;
                nas.Quality = trackBar.Value;
                return nas;
            }
            set
            {
                ExhaleSettings nas = value as ExhaleSettings;
                quality = nas.Quality;
                cbProfile.SelectedItem = EnumProxy.Create(nas.Profile);
                if (nas.Profile == ExhaleProfile.xHEAAC)
                    trackBar.Value = Math.Max(Math.Min(nas.Quality, trackBar.Maximum), trackBar.Minimum);
                else
                    trackBar.Value = Math.Max(Math.Min(nas.Quality, 6), trackBar.Minimum);
            }
        }

        ExhaleSettings Editable<ExhaleSettings>.Settings
        {
            get
            {
                return (ExhaleSettings)Settings;
            }
            set
            {
                Settings = value;
            }
        }

        private void trackBar_ValueChanged(object sender, EventArgs e)
        {
            switch ((ExhaleProfile)(cbProfile.SelectedItem as EnumProxy).RealValue)
            {
                case ExhaleProfile.xHEAAC:
                    encoderGroupBox.Text = String.Format(" Exhale - xHE-AAC - Preset #{0} ", trackBar.Value);
                    quality = trackBar.Value;
                    break;
                case ExhaleProfile.xHEAACeSBR:
                    switch (trackBar.Value)
                    {
                        case 0:
                            encoderGroupBox.Text = String.Format(" Exhale - xHE-AAC + eSBR - Preset a ");
                            break;
                        case 1:
                            encoderGroupBox.Text = String.Format(" Exhale - xHE-AAC + eSBR - Preset b ");
                            break;
                        case 2:
                            encoderGroupBox.Text = String.Format(" Exhale - xHE-AAC + eSBR - Preset c ");
                            break;
                        case 3:
                            encoderGroupBox.Text = String.Format(" Exhale - xHE-AAC + eSBR - Preset d ");
                            break;
                        case 4:
                            encoderGroupBox.Text = String.Format(" Exhale - xHE-AAC + eSBR - Preset e ");
                            break;
                        case 5:
                            encoderGroupBox.Text = String.Format(" Exhale - xHE-AAC + eSBR - Preset f ");
                            break;
                        case 6:
                            encoderGroupBox.Text = String.Format(" Exhale - xHE-AAC + eSBR - Preset g ");
                            break;
                    }
                    quality = trackBar.Value;
                    break;
            }
        }

        private void cbProfile_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch ((ExhaleProfile)(cbProfile.SelectedItem as EnumProxy).RealValue)
            {
                case ExhaleProfile.xHEAAC:
                    trackBar.Maximum = 9;
                    trackBar.Minimum = 0;
                    quality = trackBar.Value;
                    break;
                case ExhaleProfile.xHEAACeSBR:
                    trackBar.Maximum = 6;
                    trackBar.Minimum = 0;
                    quality = trackBar.Value;
                    break;
            }
            trackBar_ValueChanged(sender, e);
        }
    }
}
