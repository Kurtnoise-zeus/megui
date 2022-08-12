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

namespace MeGUI.packages.audio.qaac
{
    public partial class qaacConfigurationPanel : MeGUI.core.details.audio.AudioConfigurationPanel, Editable<QaacSettings>
    {

        public qaacConfigurationPanel():base()
        {
            InitializeComponent();
            cbMode.Items.AddRange(EnumProxy.CreateArray(QaacSettings.SupportedModes));
            cbProfile.Items.AddRange(EnumProxy.CreateArray(QaacSettings.SupportedProfiles));
        }

        #region Editable<QaacSettings> Members

        #region properties
        /// <summary>
        /// gets / sets the settings that are being shown in this configuration dialog
        /// </summary>
        protected override AudioCodecSettings CodecSettings
        {
            get
            {
                QaacSettings qas = new QaacSettings();
                switch ((QaacMode)(cbMode.SelectedItem as EnumProxy).RealValue)
                {
                    case QaacMode.ABR: qas.BitrateMode = BitrateManagementMode.ABR; break;
                    case QaacMode.CBR: qas.BitrateMode = BitrateManagementMode.CBR; break;
                    default: qas.BitrateMode = BitrateManagementMode.VBR; break;
                }
                qas.NoDelay = chNoDelay.Checked;
                qas.Mode = (QaacMode)(cbMode.SelectedItem as EnumProxy).RealValue;
                qas.Profile = (QaacProfile)(cbProfile.SelectedItem as EnumProxy).RealValue;
                qas.Quality = Int16.Parse(cbQuality.SelectedItem.ToString());
                qas.Bitrate = (int)trackBar.Value;
                return qas;
            }
            set
            {
                QaacSettings qas = value as QaacSettings;
                cbMode.SelectedItem = EnumProxy.Create(qas.Mode);
                cbProfile.SelectedItem = EnumProxy.Create(qas.Profile);

                // qas.Mode == QaacMode.TVBR
                cbQuality.SelectedItem = qas.Quality.ToString();
                if (cbQuality.SelectedItem == null)
                {
                    // change to a proper value
                    foreach (string item in cbQuality.Items)
                    {
                        if (qas.Quality >= Int16.Parse(item))
                            cbQuality.SelectedItem = item;
                    }

                    // reset to default if required
                    if (cbQuality.SelectedItem == null)
                        cbQuality.SelectedItem = 91;
                }

                // qas.Mode != QaacMode.TVBR)
                trackBar.Value = Math.Max(Math.Min(qas.Bitrate, trackBar.Maximum), trackBar.Minimum);

                chNoDelay.Checked = qas.NoDelay;
            }
        }
        #endregion

        QaacSettings Editable<QaacSettings>.Settings
        {
            get
            {
                return (QaacSettings)Settings;
            }
            set
            {
                Settings = value;
            }
        }

        #endregion

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            switch ((QaacMode)(cbMode.SelectedItem as EnumProxy).RealValue)
            {
                case QaacMode.TVBR:
                    trackBar.Visible = false;
                    cbQuality.Visible = label4.Visible = true;
                    encoderGroupBox.Text = String.Format(" QAAC Options - (Q={0}) ", cbQuality.SelectedItem);
                    break;
                case QaacMode.CVBR:
                    trackBar.Visible = true;
                    cbQuality.Visible = label4.Visible = false;
                    encoderGroupBox.Text = String.Format(" QAAC Options - Constrained Variable Bitrate @ {0} kbit/s ", trackBar.Value);
                    break;
                case QaacMode.ABR:
                    trackBar.Visible = true;
                    cbQuality.Visible = label4.Visible = false;
                    encoderGroupBox.Text = String.Format(" QAAC Options - Average Bitrate @ {0} kbit/s ", trackBar.Value);
                    break;
                case QaacMode.CBR:
                    trackBar.Visible = true;
                    cbQuality.Visible = label4.Visible = false;
                    encoderGroupBox.Text = String.Format(" QAAC Options - Constant Bitrate  @ {0} kbit/s ", trackBar.Value);
                    break;
            }  
            if (cbProfile.SelectedItem != null)
            {
                if (((QaacProfile)(cbProfile.SelectedItem as EnumProxy).RealValue) == QaacProfile.ALAC)
                    encoderGroupBox.Text = String.Format(" QAAC Options ");
                chNoDelay.Visible = (((QaacProfile)(cbProfile.SelectedItem as EnumProxy).RealValue) == QaacProfile.LC);
            }
        }

        private void cbMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            trackBar1_ValueChanged(sender, e);
        }

        private void cbProfile_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cbProfile.SelectedIndex)
            {
                case 2: trackBar.Enabled = false; cbMode.Enabled = false; break;
                default: trackBar.Enabled = true; cbMode.Enabled = true; break;
            }

            if (cbProfile.SelectedIndex == 1)
            {
                QaacMode qMode = (QaacMode)(cbMode.SelectedItem as EnumProxy).RealValue;
                cbMode.Items.Remove(EnumProxy.Create(QaacMode.TVBR));
                if (qMode == QaacMode.TVBR)
                    cbMode.SelectedItem = EnumProxy.Create(QaacMode.CVBR);
            }
            else if (cbMode.Items.Count == 3)
                cbMode.Items.Insert(0, EnumProxy.Create(QaacMode.TVBR));

            cbMode_SelectedIndexChanged(sender, e);
        }
    }
}