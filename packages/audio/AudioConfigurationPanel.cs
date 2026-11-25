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

namespace MeGUI.core.details.audio
{
    public partial class AudioConfigurationPanel : UserControl
    {
        private EnumProxy[] _avisynthChannelSet;
        private MeGUI.core.gui.ModernDialog modern;
        private Label encoderSection;
        private MeGUI.core.gui.SettingsCard encoderCard;
	    
        /// <summary>
        /// Removes the "Extra" tab (custom command line) for the encoders which do not use it
        /// </summary>
        protected void HideExtraTab()
        {
            tabControl1.TabPages.Remove(Page2);
        }

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
                AudioDecodingEngine.LWLibavAudioSource,
                AudioDecodingEngine.BestAudioSource });
            
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
                TimeModificationMode.SlowDown24To23976,
                TimeModificationMode.SlowDown24To23976WithCorrection,
                TimeModificationMode.SlowDown25To23976,
                TimeModificationMode.SlowDown25To23976WithCorrection,
                TimeModificationMode.SlowDown25To24,
                TimeModificationMode.SlowDown25To24WithCorrection,
                TimeModificationMode.SpeedUp23976To24,
                TimeModificationMode.SpeedUp23976To24WithCorrection,
                TimeModificationMode.SpeedUp23976To25,
                TimeModificationMode.SpeedUp23976To25WithCorrection,
                TimeModificationMode.SpeedUp24To25,
                TimeModificationMode.SpeedUp24To25WithCorrection });

            this.cbDownmixMode.SelectedIndex = 0;
            this.cbSampleRate.SelectedIndex = 0;
            this.cbTimeModification.SelectedIndex = 0;
            BuildModernLayout();
        }

        /// <summary>
        /// Windows 11 settings look: the general options and the custom command line are cards (see ModernDialog).
        /// The options of the encoder are moved into the first card by ModernizeEncoderOptions, which the panels
        /// of the encoders call at the end of their InitializeComponent.
        /// </summary>
        private void BuildModernLayout()
        {
            modern = new MeGUI.core.gui.ModernDialog(this);
            MeGUI.core.gui.ModernDialog d = modern;

            MeGUI.core.gui.SettingsPage general = d.CurrentPage;
            // the audio processing options are in an expander, at the top of the page
            MeGUI.core.gui.SettingsCard processing = d.Card();
            processing.Margin = new Padding(0, d.S(12), 0, d.S(3));
            processing.AddExpanderHeader("Audio processing", null, true);
            processing.AddRow(MeGUI.core.gui.ModernDialog.Cap(label1.Text), null, d.Wide(primaryDecoding, 240));
            processing.AddRow(MeGUI.core.gui.ModernDialog.Cap(BesweetChannelsLabel.Text), null, d.Wide(cbDownmixMode, 240));
            processing.AddRow(MeGUI.core.gui.ModernDialog.Cap(lbSampleRate.Text), null, d.Wide(cbSampleRate, 240));
            processing.AddRow(MeGUI.core.gui.ModernDialog.Cap(lbTimeModification.Text), null, d.Wide(cbTimeModification, 300));
            autoGain.AutoSize = false;
            autoGain.Size = new Size(d.S(96), d.S(26));
            // the numeric box is framed (a bare NumericUpDown was drawn empty in the row)
            MeGUI.core.gui.RoundedBox normalizeBox = new MeGUI.core.gui.RoundedBox(normalize, d.S(100), d.S(32));
            processing.AddToggle(autoGain, null, normalizeBox);
            d.ToggleIn(processing, applyDRC, null);

            encoderSection = d.Section("Encoder");
            encoderCard = d.Card();

            MeGUI.core.gui.SettingsPage extra = d.BeginPage();
            d.Section("Custom command line");
            d.Block(tbAudioCLI, 560, 34, d.Card());

            Controls.Remove(encoderGroupBox);
            AttachPage(Page1, general);
            AttachPage(Page2, extra);
            tabControl1.Dock = DockStyle.Fill;
            d.AttachTheme();
        }

        private static void AttachPage(TabPage tab, MeGUI.core.gui.SettingsPage page)
        {
            tab.Controls.Clear();
            tab.Padding = new Padding(0);
            page.Dock = DockStyle.Fill;
            tab.Controls.Add(page);
        }

        /// <summary>
        /// Moves the options of the encoder (the group box filled by the panel of the encoder) into the first
        /// card. To call after the InitializeComponent of the panel of the encoder.
        /// </summary>
        protected void ModernizeEncoderOptions()
        {
            if (modern == null)
                return;
            // the designer of the panel of the encoder sets a small size and the automatic scaling again
            AutoScaleMode = AutoScaleMode.None;
            modern.FillCard(encoderCard, encoderGroupBox);
            encoderSection.Text = MeGUI.core.gui.ModernDialog.Cap(encoderGroupBox.Text);
            encoderGroupBox.TextChanged += delegate { encoderSection.Text = MeGUI.core.gui.ModernDialog.Cap(encoderGroupBox.Text); };
            Size = new Size(modern.S(640), modern.S(600));
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