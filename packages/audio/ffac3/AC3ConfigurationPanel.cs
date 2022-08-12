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

namespace MeGUI.packages.audio.ffac3
{
    public partial class AC3ConfigurationPanel : MeGUI.core.details.audio.AudioConfigurationPanel, Editable<AC3Settings>
    {
        public AC3ConfigurationPanel():base()
        {
            InitializeComponent();
            comboBox1.Items.AddRange(AC3Settings.SupportedBitrates);
        }

        #region properties
        /// <summary>
        /// gets / sets the settings that are being shown in this configuration dialog
        /// </summary>
        protected override AudioCodecSettings CodecSettings
        {
            get
            {
                AC3Settings nas = new AC3Settings();
                nas.Bitrate = (int)comboBox1.SelectedItem;
                return nas;
            }
            set
            {
                AC3Settings nas = value as AC3Settings;
                comboBox1.SelectedItem = nas.Bitrate;
            }
        }
        #endregion

        #region Editable<AC3Settings> Members

        AC3Settings Editable<AC3Settings>.Settings
        {
            get
            {
                return (AC3Settings)Settings;
            }
            set
            {
                Settings = value;
            }
        }

        #endregion
    }
}