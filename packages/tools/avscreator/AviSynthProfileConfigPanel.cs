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

namespace MeGUI.core.gui
{
    public partial class AviSynthProfileConfigPanel : UserControl, Editable<AviSynthSettings>
    {     
        public AviSynthProfileConfigPanel() 
        {
            InitializeComponent();
            this.resizeFilterType.DataSource = ScriptServer.ListOfResizeFilterType;
            this.resizeFilterType.BindingContext = new BindingContext();
            this.noiseFilterType.DataSource = ScriptServer.ListOfDenoiseFilterType;
            this.noiseFilterType.BindingContext = new BindingContext();
        }

        #region Gettable<AviSynthSettings> Members

        public AviSynthSettings Settings
        {
            get
            {
                AviSynthSettings oSettings = new AviSynthSettings();
                oSettings.Template = avisynthScript.Text;
                oSettings.Resize = resize.Checked;
                oSettings.ResizeMethod = (ResizeFilterType)(resizeFilterType.SelectedItem as EnumProxy).RealValue;
                oSettings.NvResize = chkNvResize.Checked;
                oSettings.PreferAnimeDeinterlace = cbPreferAnimeDeinterlacing.Checked;
                oSettings.Denoise = noiseFilter.Checked;
                oSettings.DenoiseMethod = (DenoiseFilterType)(noiseFilterType.SelectedItem as EnumProxy).RealValue;
                oSettings.MPEG2Deblock = mpeg2Deblocking.Checked;
                oSettings.ColourCorrect = colourCorrect.Checked;
                oSettings.Mod16Method = (!signalAR.Checked ? mod16Method.none : (mod16Method)mod16Box.SelectedIndex);
                oSettings.ModValue = (modValue)modValueBox.SelectedIndex;
                oSettings.DSS2 = dss2.Checked;
                oSettings.Upsize = upsize.Checked;
                oSettings.AcceptableAspectError = acceptableAspectError.Value;
                return oSettings;
            }
            set
            {
                avisynthScript.Text = value.Template;
                resize.Checked = value.Resize;
                chkNvResize.Checked = value.NvResize;
                upsize.Checked = value.Upsize;
                resizeFilterType.SelectedItem = EnumProxy.Create(value.ResizeMethod);
                noiseFilterType.SelectedItem = EnumProxy.Create(value.DenoiseMethod);
                noiseFilter.Checked = value.Denoise;
                mpeg2Deblocking.Checked = value.MPEG2Deblock;
                colourCorrect.Checked = value.ColourCorrect;
                signalAR.Checked = (value.Mod16Method != mod16Method.none);
                mod16Box.Enabled = signalAR.Checked;
                mod16Box.SelectedIndex = (int)value.Mod16Method;
                dss2.Checked = value.DSS2;
                modValueBox.SelectedIndex = (int)value.ModValue;
                acceptableAspectError.Value = value.AcceptableAspectError;
                cbPreferAnimeDeinterlacing.Checked = value.PreferAnimeDeinterlace;
            }
        }

        #endregion

        #region event handlers
        private void signalAR_CheckedChanged(object sender, EventArgs e)
        {
            mod16Box.Enabled = signalAR.Checked;
        }
        #endregion

        private void insert_Click(object sender, EventArgs e)
        {
            string text = (sender as Control).Tag as string;
            string avsScript = avisynthScript.Text;
			string avsScriptA = avsScript.Substring(0, avisynthScript.SelectionStart);
			string avsScriptB = avsScript.Substring(avisynthScript.SelectionStart + avisynthScript.SelectionLength);
			avisynthScript.Text = avsScriptA + text + avsScriptB;
        }

        private void dllBar_FileSelected(FileBar sender, FileBarEventArgs args)
        {
            avisynthScript.Text = "LoadPlugin(\"" + args.NewFileName + "\")\r\n" + avisynthScript.Text;
        }

        private void noiseFilter_CheckedChanged(object sender, EventArgs e)
        {
            noiseFilterType.Enabled = noiseFilter.Checked;
        }

        private void resize_CheckedChanged(object sender, EventArgs e)
        {
            resizeFilterType.Enabled = resize.Checked;
        }
    }
}