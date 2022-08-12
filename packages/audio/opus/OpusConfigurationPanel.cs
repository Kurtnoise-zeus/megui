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

using MeGUI.core.plugins.interfaces;

namespace MeGUI.packages.audio.opus
{
    public partial class OpusConfigurationPanel : MeGUI.core.details.audio.AudioConfigurationPanel, Editable<OpusSettings>
    {

        public OpusConfigurationPanel():base()
        {
            InitializeComponent();
            cbMode.Items.AddRange(EnumProxy.CreateArray(OpusSettings.SupportedModes));
            trackBar_ValueChanged(null, null);
        }

        private void InitializeComponent()
        {
            this.trackBar = new System.Windows.Forms.TrackBar();
            this.cbMode = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.encoderGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar)).BeginInit();
            this.SuspendLayout();
            // 
            // encoderGroupBox
            // 
            this.encoderGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
            this.encoderGroupBox.Controls.Add(this.label2);
            this.encoderGroupBox.Controls.Add(this.trackBar);
            this.encoderGroupBox.Controls.Add(this.cbMode);
            this.encoderGroupBox.Size = new System.Drawing.Size(402, 105);
            this.encoderGroupBox.Text = "Opus Options";
            // 
            // trackBar
            // 
            this.trackBar.Location = new System.Drawing.Point(9, 58);
            this.trackBar.Maximum = 512;
            this.trackBar.Minimum = 6;
            this.trackBar.Name = "trackBar";
            this.trackBar.Size = new System.Drawing.Size(387, 45);
            this.trackBar.TabIndex = 4;
            this.trackBar.TickFrequency = 8;
            this.trackBar.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.trackBar.Value = 64;
            this.trackBar.ValueChanged += new System.EventHandler(this.trackBar_ValueChanged);
            // 
            // cbMode
            // 
            this.cbMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbMode.FormattingEnabled = true;
            this.cbMode.Location = new System.Drawing.Point(84, 31);
            this.cbMode.Name = "cbMode";
            this.cbMode.Size = new System.Drawing.Size(121, 21);
            this.cbMode.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(39, 34);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(34, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Mode";
            // 
            // OpusConfigurationPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Name = "OpusConfigurationPanel";
            this.Size = new System.Drawing.Size(410, 322);
            this.encoderGroupBox.ResumeLayout(false);
            this.encoderGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar)).EndInit();
            this.ResumeLayout(false);

        }

        #region Editable<OpusSettings> Members

        #region properties
        /// <summary>
        /// gets / sets the settings that are being shown in this configuration dialog
        /// </summary>
        protected override AudioCodecSettings CodecSettings
        {
            get
            {
                OpusSettings oas = new OpusSettings();
                if (cbMode.SelectedIndex == 0) oas.BitrateMode = BitrateManagementMode.VBR;
                if (cbMode.SelectedIndex == 1) oas.BitrateMode = BitrateManagementMode.VBR;
                if (cbMode.SelectedIndex == 2) oas.BitrateMode = BitrateManagementMode.CBR;
                oas.Mode = (OpusMode)(cbMode.SelectedItem as EnumProxy).RealValue;
                oas.Bitrate = (int)trackBar.Value;
                return oas;
            }
            set
            {
                OpusSettings oas = value as OpusSettings;
                if (cbMode.SelectedIndex == 0) oas.BitrateMode = BitrateManagementMode.VBR;
                if (cbMode.SelectedIndex == 1) oas.BitrateMode = BitrateManagementMode.VBR;
                if (cbMode.SelectedIndex == 2) oas.BitrateMode = BitrateManagementMode.CBR;
                cbMode.SelectedItem = EnumProxy.Create(oas.Mode);
                trackBar.Value = Math.Max(Math.Min(oas.Bitrate, trackBar.Maximum), trackBar.Minimum); 
            }
        }
        #endregion

        OpusSettings Editable<OpusSettings>.Settings
        {
            get
            {
                return (OpusSettings)Settings;
            }
            set
            {
                Settings = value;
            }
        }

        #endregion

        private void trackBar_ValueChanged(object sender, EventArgs e)
        {
            switch (cbMode.SelectedIndex)
            {
                case 0: // VBR
                    encoderGroupBox.Text = String.Format("OPUS Options - Variable Bitrate @ {0} kbit/s", trackBar.Value);
                    break;
                case 1: // CVBR
                    encoderGroupBox.Text = String.Format("OPUS Options - Constrained Variable Bitrate @ {0} kbit/s", trackBar.Value);
                    break;
                case 2: // CBR
                    encoderGroupBox.Text = String.Format("OPUS Options - Hard Constant Bitrate  @ {0} kbit/s", trackBar.Value);
                    break;
            }
        }
    }
}