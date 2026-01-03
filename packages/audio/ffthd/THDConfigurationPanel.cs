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
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MeGUI.packages.audio.ffthd
{
    public partial class THDConfigurationPanel : MeGUI.core.details.audio.AudioConfigurationPanel, Editable<THDSettings>
    {
        public THDConfigurationPanel():base()
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
                THDSettings nas = new THDSettings();
                return nas;
            }
            set
            {
                THDSettings nas = value as THDSettings;
            }
        }
        #endregion

        #region Editable<THDSettings> Members

        THDSettings Editable<THDSettings>.Settings
        {
            get
            {
                return (THDSettings)Settings;
            }
            set
            {
                Settings = value;
            }
        }

        #endregion
    }
}