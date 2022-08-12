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
using System.Windows.Forms;

namespace MeGUI.packages.audio.naac
{
    public partial class neroConfigurationPanel : MeGUI.core.details.audio.AudioConfigurationPanel, Editable<NeroAACSettings>
    {
        public TrackBar vQuality;
        public RadioButton rbtnVBR;
        public TrackBar vBitrate;
        public RadioButton rbtnCBR;
        private ComboBox comboBox1;
        private Label label1;
        public RadioButton rbtnABR;
		#region variables

        #endregion
        #region start / stop
        public neroConfigurationPanel():base()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
            comboBox1.Items.AddRange(EnumProxy.CreateArray(NeroAACSettings.SupportedProfiles));
            rbtnABR_CheckedChanged(null, null);
            vBitrate_ValueChanged(null, null);
		}


		#endregion
		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.vQuality = new System.Windows.Forms.TrackBar();
            this.rbtnVBR = new System.Windows.Forms.RadioButton();
            this.vBitrate = new System.Windows.Forms.TrackBar();
            this.rbtnCBR = new System.Windows.Forms.RadioButton();
            this.rbtnABR = new System.Windows.Forms.RadioButton();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.encoderGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.vQuality)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.vBitrate)).BeginInit();
            this.SuspendLayout();
            // 
            // encoderGroupBox
            // 
            this.encoderGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
            this.encoderGroupBox.Controls.Add(this.label1);
            this.encoderGroupBox.Controls.Add(this.comboBox1);
            this.encoderGroupBox.Controls.Add(this.vQuality);
            this.encoderGroupBox.Controls.Add(this.rbtnVBR);
            this.encoderGroupBox.Controls.Add(this.vBitrate);
            this.encoderGroupBox.Controls.Add(this.rbtnCBR);
            this.encoderGroupBox.Controls.Add(this.rbtnABR);
            this.encoderGroupBox.Size = new System.Drawing.Size(402, 214);
            this.encoderGroupBox.TabIndex = 1;
            this.encoderGroupBox.Text = " NeroDigital AAC Options ";
            // 
            // besweetOptionsGroupbox
            // 
            this.besweetOptionsGroupbox.TabIndex = 0;
            // 
            // vQuality
            // 
            this.vQuality.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.vQuality.Location = new System.Drawing.Point(3, 133);
            this.vQuality.Maximum = 100;
            this.vQuality.Name = "vQuality";
            this.vQuality.Size = new System.Drawing.Size(393, 45);
            this.vQuality.TabIndex = 4;
            this.vQuality.TickFrequency = 5;
            this.vQuality.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.vQuality.ValueChanged += new System.EventHandler(this.vBitrate_ValueChanged);
            // 
            // rbtnVBR
            // 
            this.rbtnVBR.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rbtnVBR.Location = new System.Drawing.Point(13, 115);
            this.rbtnVBR.Name = "rbtnVBR";
            this.rbtnVBR.Size = new System.Drawing.Size(275, 24);
            this.rbtnVBR.TabIndex = 3;
            this.rbtnVBR.Text = "Variable Bitrate";
            this.rbtnVBR.CheckedChanged += new System.EventHandler(this.rbtnABR_CheckedChanged);
            // 
            // vBitrate
            // 
            this.vBitrate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.vBitrate.Location = new System.Drawing.Point(3, 64);
            this.vBitrate.Maximum = 640;
            this.vBitrate.Minimum = 16;
            this.vBitrate.Name = "vBitrate";
            this.vBitrate.Size = new System.Drawing.Size(393, 45);
            this.vBitrate.TabIndex = 2;
            this.vBitrate.TickFrequency = 8;
            this.vBitrate.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.vBitrate.Value = 16;
            this.vBitrate.ValueChanged += new System.EventHandler(this.vBitrate_ValueChanged);
            // 
            // rbtnCBR
            // 
            this.rbtnCBR.Location = new System.Drawing.Point(13, 42);
            this.rbtnCBR.Name = "rbtnCBR";
            this.rbtnCBR.Size = new System.Drawing.Size(320, 24);
            this.rbtnCBR.TabIndex = 1;
            this.rbtnCBR.Text = "Constant Bitrate";
            this.rbtnCBR.CheckedChanged += new System.EventHandler(this.rbtnABR_CheckedChanged);
            // 
            // rbtnABR
            // 
            this.rbtnABR.Location = new System.Drawing.Point(13, 24);
            this.rbtnABR.Name = "rbtnABR";
            this.rbtnABR.Size = new System.Drawing.Size(302, 17);
            this.rbtnABR.TabIndex = 0;
            this.rbtnABR.Text = "Adaptive Bitrate";
            this.rbtnABR.CheckedChanged += new System.EventHandler(this.rbtnABR_CheckedChanged);
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(106, 179);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(266, 21);
            this.comboBox1.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 182);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "AAC Profile";
            // 
            // neroConfigurationPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Name = "neroConfigurationPanel";
            this.Size = new System.Drawing.Size(410, 428);
            this.encoderGroupBox.ResumeLayout(false);
            this.encoderGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.vQuality)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.vBitrate)).EndInit();
            this.ResumeLayout(false);

		}
		#endregion
		#region properties
		/// <summary>
		/// gets / sets the settings that are being shown in this configuration dialog. These will be added to by the base class
		/// </summary>
		protected override AudioCodecSettings CodecSettings
		{
			get
			{
				NeroAACSettings nas = new NeroAACSettings();
                if (rbtnABR.Checked) nas.BitrateMode = BitrateManagementMode.ABR;
                if (rbtnCBR.Checked) nas.BitrateMode = BitrateManagementMode.CBR;
                if (rbtnVBR.Checked) nas.BitrateMode = BitrateManagementMode.VBR;
                nas.Bitrate = vBitrate.Value;
                nas.Quality= (Decimal)vQuality.Value/vQuality.Maximum;
                nas.Profile = (AacProfile)(comboBox1.SelectedItem as EnumProxy).RealValue;
				return nas;
			}
			set
			{
                NeroAACSettings nas = (NeroAACSettings)value;
                vBitrate.Value = Math.Max(Math.Min(nas.Bitrate, vBitrate.Maximum), vBitrate.Minimum);
                vQuality.Value = (int)(nas.Quality * (Decimal)vQuality.Maximum);
                rbtnABR.Checked = nas.BitrateMode == BitrateManagementMode.ABR;
                rbtnCBR.Checked = nas.BitrateMode == BitrateManagementMode.CBR;
                rbtnVBR.Checked = nas.BitrateMode == BitrateManagementMode.VBR;
                comboBox1.SelectedItem = EnumProxy.Create(nas.Profile);
			}
		}
		#endregion

        private void rbtnABR_CheckedChanged(object sender, EventArgs e)
        {
            vBitrate.Enabled = !(vQuality.Enabled = rbtnVBR.Checked);
            vBitrate_ValueChanged(sender, e);
        }

        private void vBitrate_ValueChanged(object sender, EventArgs e)
        {
            if (rbtnVBR.Checked)
            {
                label1.Visible = comboBox1.Visible = false;
                rbtnABR.Text = "Adaptive Bitrate";
                rbtnCBR.Text = "Constant Bitrate";
                Decimal q = ((Decimal)vQuality.Value) / vQuality.Maximum;
                rbtnVBR.Text = String.Format("Variable Bitrate, Q > 0.31 for AAC LC (Q={0}) ", q);
            }
            else if (rbtnABR.Checked)
            {
                label1.Visible = comboBox1.Visible = true;
                rbtnABR.Text = String.Format("Adaptive Bitrate @ {0} kbit/s", vBitrate.Value);
                rbtnCBR.Text = "Constant Bitrate";
                rbtnVBR.Text = "Variable Bitrate";
            }
            else
            {
                label1.Visible = comboBox1.Visible = true;
                rbtnABR.Text = "Adaptive Bitrate";
                rbtnCBR.Text = String.Format("Constant Bitrate @ {0} kbit/s", vBitrate.Value);
                rbtnVBR.Text = "Variable Bitrate";
            }
        }

        #region Editable<NeroAACSettings> Members

        NeroAACSettings Editable<NeroAACSettings>.Settings
        {
            get
            {
                return (NeroAACSettings)Settings;
            }
            set
            {
                Settings = value;
            }
        }

        #endregion
    }
}