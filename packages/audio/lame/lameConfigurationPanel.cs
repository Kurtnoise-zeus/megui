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

namespace MeGUI.packages.audio.lame
{
    public partial class lameConfigurationPanel : MeGUI.core.details.audio.AudioConfigurationPanel, Editable<MP3Settings>
    {
        public lameConfigurationPanel():base()
        {
            InitializeComponent();
            cbrBitrate.DataSource = MP3Settings.SupportedBitrates;
            cbrBitrate.BindingContext = new BindingContext();
            cbrBitrate.SelectedItem = 128;
        }
		#region properties
        protected override bool IsMultichanelSupported
        {
            get
            {
                return false;
            }
        }
	    /// <summary>
	    /// gets / sets the settings that are being shown in this configuration dialog
	    /// </summary>
	    protected override AudioCodecSettings CodecSettings
	    {
	        get
	        {
                MP3Settings ms = new MP3Settings();
                if (vbrMode.Checked)
                    ms.BitrateMode = BitrateManagementMode.VBR;
                else if (abrMode.Checked)
                    ms.BitrateMode = BitrateManagementMode.ABR;
                else
                    ms.BitrateMode = BitrateManagementMode.CBR;
                ms.Bitrate = (int)cbrBitrate.SelectedItem;
                ms.AbrBitrate = (int)abrValue.Value;
                ms.Quality = (int)vbrValue.Value;
                return ms;
	        }
	        set
	        {
                MP3Settings ms = value as MP3Settings;
                if (ms.BitrateMode == BitrateManagementMode.ABR)
                    abrMode.Checked = true;
                else if (ms.BitrateMode == BitrateManagementMode.VBR)
                    vbrMode.Checked = true;
                else
                    cbrMode.Checked = true;
                cbrBitrate.SelectedItem = ms.Bitrate;
                if (ms.AbrBitrate < 8 || ms.AbrBitrate > 320)
                    abrValue.Value = 128;
                else
                    abrValue.Value = ms.AbrBitrate;
                vbrValue.Value = ms.Quality;	            
	        }
	    }
		#endregion
		#region buttons
		/// <summary>
		/// handles entires into textfiels, blocks entry of non digit characters
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void textField_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (! char.IsDigit(e.KeyChar) && (int)Keys.Back != (int)e.KeyChar)
				e.Handled = true;
		}
		#endregion

        #region Editable<MP3Settings> Members

        MP3Settings Editable<MP3Settings>.Settings
        {
            get
            {
                return (MP3Settings)Settings;
            }
            set
            {
                Settings = value;
            }
        }

        #endregion

        private void cbrMode_CheckedChanged(object sender, EventArgs e)
        {
            cbrBitrate.Enabled = true;
            vbrValue.Enabled = abrValue.Enabled = false;
        }

        private void abrMode_CheckedChanged(object sender, EventArgs e)
        {
            abrValue.Enabled = true;
            vbrValue.Enabled = cbrBitrate.Enabled = false;
        }

        private void vbrMode_CheckedChanged(object sender, EventArgs e)
        {
            vbrValue.Enabled = true;
            abrValue.Enabled = cbrBitrate.Enabled = false;
        }
    }
}




