// ****************************************************************************
// 
// Copyright (C) 2005-2009  Doom9 & al
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
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using MeGUI.core.details.video;
using MeGUI.core.util;

namespace MeGUI.packages.tools.oneclick
{
    public partial class AudioConfigControl : UserControl
    {
        MainForm mainForm = MainForm.Instance;

        #region Audio profiles
        private void initAudioHandler()
        {
            audioProfile.Manager = mainForm.Profiles;
        }

        void audioCodec_SelectedIndexChanged(object sender, EventArgs e)
        {
            raiseEvent();
        }
        #endregion

        private void ProfileChanged(object o, EventArgs e)
        {
            raiseEvent();
        }

        public event EventHandler SomethingChanged;

        private void dontEncodeAudio_CheckedChanged(object sender, EventArgs e)
        {
            audioProfile.Enabled = !cbAudioEncoding.SelectedText.Equals("never");
            raiseEvent();
        }

        private void raiseEvent()
        {
            if (SomethingChanged != null) SomethingChanged(this, null);
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public AudioCodecSettings Settings
        {
            get { return (AudioCodecSettings)audioProfile.SelectedProfile.BaseSettings; }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool DelayEnabled
        {
            set { delay.Enabled = value; }
            get { return delay.Enabled; }
        }
        
        public void openAudioFile(string p)
        {
            delay.Value = PrettyFormatting.getDelayAndCheck(p) ?? 0;
        }

        public AudioConfigControl()
        {
            InitializeComponent();
        }

        internal void initHandler()
        {
            initAudioHandler();
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public AudioEncodingMode AudioEncodingMode
        {
            get
            {
                return (AudioEncodingMode)cbAudioEncoding.SelectedIndex;
            }
            set
            {
                cbAudioEncoding.SelectedIndex = (int)value;
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public void DisableDontEncode(bool bDisable)
        {
            if (bDisable)
            {
                cbAudioEncoding.SelectedItem = AudioEncodingMode.Always;
                cbAudioEncoding.Enabled = false;
            }
            else
                cbAudioEncoding.Enabled = true;
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsDontEncodePossible()
        {
            return cbAudioEncoding.Enabled;
        }

        public void SelectProfileNameOrWarn(string fqname)
        {
            audioProfile.SetProfileNameOrWarn(fqname);
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int Delay
        {
            get
            {
                return (int)delay.Value;
            }
            set
            {
                delay.Value = value;
            }
        }
    }
}
