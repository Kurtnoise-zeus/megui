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

namespace MeGUI.core.details.audio
{
    public partial class AudioConfigurationPanel : UserControl
    {
        private EnumProxy[] _avisynthChannelSet;
	    
        #region start / stop

	    public AudioConfigurationPanel()
	    {
            _avisynthChannelSet =
                EnumProxy.CreateArray(
                    this.IsMultichanelSupported
                    ?
                    (
                    this.IsMultichanelRequed
                    ?
                    new object[]{
                                ChannelMode.Upmix,
                                ChannelMode.UpmixUsingSoxEq,
                                ChannelMode.UpmixWithCenterChannelDialog
                    }
                    :
                    new object[]{
                                ChannelMode.KeepOriginal,
                                ChannelMode.Downmix51,
                                ChannelMode.StereoDownmix,
                                ChannelMode.DPLDownmix,
                                ChannelMode.DPLIIDownmix,
                                ChannelMode.ConvertToMono,
                                ChannelMode.Upmix,
                                ChannelMode.UpmixUsingSoxEq,
                                ChannelMode.UpmixWithCenterChannelDialog
                    }

                    )
                                :
                    new object[]{
                                ChannelMode.StereoDownmix,
                                ChannelMode.DPLDownmix,
                                ChannelMode.DPLIIDownmix,
                                ChannelMode.ConvertToMono
                    }
                    );

            InitializeComponent();
            this.primaryDecoding.DataSource = EnumProxy.CreateArray(new object[] {
                AudioDecodingEngine.NicAudio,
                AudioDecodingEngine.FFAudioSource,
                AudioDecodingEngine.DirectShow,
                AudioDecodingEngine.BassAudio,
                AudioDecodingEngine.LWLibavAudioSource });
            
            this.cbDownmixMode.DataSource = _avisynthChannelSet;
            this.cbDownmixMode.BindingContext = new BindingContext();
            
            this.cbSampleRate.DataSource = EnumProxy.CreateArray(new object[] {
                SampleRateMode.KeepOriginal,
                SampleRateMode.ConvertTo08000,
                SampleRateMode.ConvertTo11025,
                SampleRateMode.ConvertTo22050,
                SampleRateMode.ConvertTo32000,
                SampleRateMode.ConvertTo44100,
                SampleRateMode.ConvertTo48000,
                SampleRateMode.ConvertTo96000 });

            this.cbTimeModification.DataSource = EnumProxy.CreateArray(new object[] {
                TimeModificationMode.KeepOriginal,
                TimeModificationMode.SlowDown25To23976,
                TimeModificationMode.SlowDown25To23976WithCorrection,
                TimeModificationMode.SlowDown25To24,
                TimeModificationMode.SlowDown25To24WithCorrection,
                TimeModificationMode.SpeedUp23976To25,
                TimeModificationMode.SpeedUp23976To25WithCorrection,
                TimeModificationMode.SpeedUp24To25,
                TimeModificationMode.SpeedUp24To25WithCorrection });

            this.cbDownmixMode.SelectedIndex = 0;
            this.cbSampleRate.SelectedIndex = 0;
            this.cbTimeModification.SelectedIndex = 0;
        }
	    
		#endregion
		#region dropdowns
			
		#endregion
		#region checkboxes

        protected void showCommandLine()
	    {
	        
	    }

		#endregion
		#region properties


	    protected virtual bool IsMultichanelSupported
	    {
	        get
	        {
                return true;
	        }
	    }

        protected virtual bool IsMultichanelRequed
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Must collect data from UI / Fill UI from Data
        /// </summary>
        [Browsable(false)]
        protected virtual AudioCodecSettings CodecSettings
        {
            get
            {
                throw new NotImplementedException("Must be overridden");
            }
            set
            {
                throw new NotImplementedException("Must be overridden");
            }
        }

	    
		/// <summary>
		/// gets / sets the settings that are being shown in this configuration dialog
		/// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public AudioCodecSettings Settings
		{
			get
			{
                AudioCodecSettings fas = CodecSettings;
                fas.PreferredDecoder = (AudioDecodingEngine)primaryDecoding.SelectedIndex;
                EnumProxy o = cbDownmixMode.SelectedItem as EnumProxy;
			    if(o != null)
				    fas.DownmixMode = (ChannelMode)o.RealValue ;
                o = cbSampleRate.SelectedItem as EnumProxy;
                if (o != null)
                    fas.SampleRate = (SampleRateMode)o.RealValue;
                o = cbTimeModification.SelectedItem as EnumProxy;
                if (o != null)
                    fas.TimeModification = (TimeModificationMode)o.RealValue;
				fas.AutoGain = autoGain.Checked;
                fas.ApplyDRC = applyDRC.Checked;
                fas.Normalize = (int)normalize.Value;
                fas.CustomEncoderOptions = tbAudioCLI.Text.Trim();
				return fas;
			}
			set
			{
				AudioCodecSettings fas = value;
                cbDownmixMode.SelectedItem = EnumProxy.Create(fas.DownmixMode);
                cbSampleRate.SelectedItem = EnumProxy.Create(fas.SampleRate);
                cbTimeModification.SelectedItem = EnumProxy.Create(fas.TimeModification);
                primaryDecoding.SelectedIndex = (int)fas.PreferredDecoder;
				autoGain.Checked = fas.AutoGain;
                applyDRC.Checked = fas.ApplyDRC;
                normalize.Value = fas.Normalize;
                tbAudioCLI.Text = fas.CustomEncoderOptions;
                CodecSettings = fas;
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
		#region commandline
		private void besweetDelay_TextChanged(object sender, System.EventArgs e)
		{
		}
		#endregion

        private void applyDRC_CheckedChanged(object sender, EventArgs e)
        {
            autoGain.Checked = applyDRC.Checked;
            autoGain_CheckedChanged(sender, e);
        }

        private void autoGain_CheckedChanged(object sender, EventArgs e)
        {
            normalize.Enabled = autoGain.Checked;
        }
    }
}