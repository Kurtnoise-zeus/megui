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

namespace MeGUI.packages.audio.flac
{
    public partial class FlacConfigurationPanel : MeGUI.core.details.audio.AudioConfigurationPanel, Editable<FlacSettings>
    {
       public FlacConfigurationPanel():base()
        {
            InitializeComponent();
        }

        #region properties
        /// <summary>
        /// gets / sets the settings that are being shown in this configuration dialog
        /// </summary>
        protected override AudioCodecSettings CodecSettings
        {
            get
            {
                FlacSettings nas = new FlacSettings();
                nas.CompressionLevel = tbQuality.Value;
                return nas;
            }
            set
            {
                FlacSettings nas = value as FlacSettings;
                tbQuality.Value = nas.CompressionLevel;
            }
        }
        #endregion

        #region Editable<FlacSettings> Members

        FlacSettings Editable<FlacSettings>.Settings
        {
            get
            {
                return (FlacSettings)Settings;
            }
            set
            {
                Settings = value;
            }
        }

        #endregion
    }
}




