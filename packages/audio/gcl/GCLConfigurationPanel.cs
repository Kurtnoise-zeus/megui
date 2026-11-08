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
using System.Windows.Forms;

namespace MeGUI.packages.audio.gcl
{
    public partial class GCLConfigurationPanel : MeGUI.core.details.audio.AudioConfigurationPanel, Editable<GCLSettings>
    {
        public GCLConfigurationPanel()
            : base()
        {
            InitializeComponent();
            cbInputHeader.Items.AddRange(EnumProxy.CreateArray(GCLSettings.SupportedInputHeaders));
        }

        /// <summary>
        /// gets / sets the settings that are being shown in this configuration dialog
        /// </summary>
        protected override AudioCodecSettings CodecSettings
        {
            get
            {
                GCLSettings gcl = new GCLSettings();
                gcl.EncoderPath = tbEncoderPath.Text;
                gcl.CommandLine = tbCommandLine.Text;
                gcl.OutputExtension = tbExtension.Text;
                EnumProxy header = cbInputHeader.SelectedItem as EnumProxy;
                gcl.InputHeader = header != null ? (GCLInputHeader)header.RealValue : GCLInputHeader.WAV;
                return gcl;
            }
            set
            {
                GCLSettings gcl = value as GCLSettings;
                tbEncoderPath.Text = gcl.EncoderPath;
                tbCommandLine.Text = gcl.CommandLine;
                tbExtension.Text = gcl.OutputExtension;
                cbInputHeader.SelectedItem = EnumProxy.Create(gcl.InputHeader);
            }
        }

        GCLSettings Editable<GCLSettings>.Settings
        {
            get
            {
                return (GCLSettings)Settings;
            }
            set
            {
                Settings = value;
            }
        }

        private void btnBrowseEncoder_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Title = "Select the audio encoder";
                dialog.Filter = "Executable files (*.exe;*.bat;*.cmd)|*.exe;*.bat;*.cmd|All files (*.*)|*.*";
                if (!String.IsNullOrEmpty(tbEncoderPath.Text))
                    dialog.FileName = tbEncoderPath.Text;
                if (dialog.ShowDialog() == DialogResult.OK)
                    tbEncoderPath.Text = dialog.FileName;
            }
        }

        private void tbExtension_Leave(object sender, EventArgs e)
        {
            tbExtension.Text = GCLSettings.NormalizeExtension(tbExtension.Text);
        }
    }
}
