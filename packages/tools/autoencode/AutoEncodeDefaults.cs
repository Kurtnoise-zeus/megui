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

using MeGUI.core.util;

namespace MeGUI
{
    public partial class AutoEncodeDefaults : Form
    {
        public AutoEncodeDefaults()
        {
            InitializeComponent();
        }

        public AutoEncodeDefaultsSettings Settings
        {
            get
            {
                AutoEncodeDefaultsSettings defaults = new AutoEncodeDefaultsSettings();
                defaults.AddAdditionalContent = addSubsNChapters.Checked;
                defaults.SplitSize = splitSize.Value;
                defaults.FileSizeMode = FileSizeRadio.Checked;
                defaults.FileSize = fileSize.Value;
                defaults.BitrateMode = averageBitrateRadio.Checked;
                if (defaults.BitrateMode)
                {
                    try
                    {
                        int bitrate = Int32.Parse(projectedBitrateKBits.Text);
                        defaults.Bitrate = bitrate;
                    }
                    catch (Exception)
                    {

                    }
                }
                defaults.NoTargetSizeMode = noTargetRadio.Checked;
                defaults.Container = container.SelectedItem.ToString();
                defaults.DeviceOutputType = device.SelectedItem.ToString();
                return defaults;
            }
            set
            {
                AutoEncodeDefaultsSettings defaults = value;
                addSubsNChapters.Checked = defaults.AddAdditionalContent;
                splitSize.Value = defaults.SplitSize;
                FileSizeRadio.Checked = defaults.FileSizeMode;
                fileSize.Value = defaults.FileSize;
                averageBitrateRadio.Checked = defaults.BitrateMode;
                projectedBitrateKBits.Text = defaults.Bitrate.ToString();
                noTargetRadio.Checked = defaults.NoTargetSizeMode;
                container.Items.AddRange(MainForm.Instance.MuxProvider.GetSupportedContainers( ).ToArray( ));
                foreach (object o in container.Items) // I know this is ugly, but using the ContainerType doesn't work unless we're switching to manual serialization
                {
                    if (o.ToString().Equals(defaults.Container))
                    {
                        container.SelectedItem = o;
                        break;
                    }
                }
                this.device.Items.Clear();
                this.device.Items.Add("Standard");
                this.device.Items.AddRange(MainForm.Instance.MuxProvider.GetSupportedDevices((ContainerType)container.SelectedItem).ToArray());
                this.device.SelectedIndex = 0;
                foreach (object o in device.Items) // I know this is ugly, but using the DeviceOutputType doesn't work unless we're switching to manual serialization
                {
                    if (o.ToString().Equals(defaults.DeviceOutputType))
                    {
                        device.SelectedItem = o;
                        break;
                    }
                }
            }
        }

        private void FileSizeRadio_CheckedChanged(object sender, EventArgs e)
        {
            if (averageBitrateRadio.Checked)
            {
                fileSize.Enabled = false;
                this.projectedBitrateKBits.Enabled = true;
            }
            else if (noTargetRadio.Checked)
            {
                fileSize.Enabled = false;
                this.projectedBitrateKBits.Enabled = false;
            }
            else
            {
                fileSize.Enabled = true;
                this.projectedBitrateKBits.Enabled = false;
            }
        }
        private void textField_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && (int)Keys.Back != (int)e.KeyChar)
                e.Handled = true;
        }

        private void container_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.container.Text == "MKV")
                this.device.Enabled = false;
            else
                this.device.Enabled = true;

            this.device.Items.Clear();
            this.device.Items.Add("Standard");
            this.device.Items.AddRange(MainForm.Instance.MuxProvider.GetSupportedDevices((ContainerType)container.SelectedItem).ToArray());
            if (container.SelectedItem.ToString().Equals(MainForm.Instance.Settings.AedSettings.Container))
            {
                foreach (object o in device.Items) // I know this is ugly, but using the DeviceOutputType doesn't work unless we're switching to manual serialization
                {
                    if (o.ToString().Equals(MainForm.Instance.Settings.AedSettings.DeviceOutputType))
                    {
                        device.SelectedItem = o;
                        break;
                    }
                }
            }
            else
                this.device.SelectedIndex = 0;
        }
    }
}