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
using System.Drawing;
using System.Windows.Forms;

namespace MeGUI.core.gui
{
    public partial class AviSynthProfileConfigPanel : UserControl, Editable<AviSynthSettings>
    {     
        public AviSynthProfileConfigPanel() 
        {
            InitializeComponent();
            BuildModernLayout();
            this.resizeFilterType.DataSource = ScriptServer.ListOfResizeFilterType;
            this.resizeFilterType.BindingContext = new BindingContext();
            this.noiseFilterType.DataSource = ScriptServer.ListOfDenoiseFilterType;
            this.noiseFilterType.BindingContext = new BindingContext();
        }

        /// <summary>
        /// Windows 11 settings look (see ModernDialog): one page of cards per tab. The controls are only moved:
        /// names, events and the settings are unchanged.
        /// </summary>
        private void BuildModernLayout()
        {
            MeGUI.core.gui.ModernDialog d = new MeGUI.core.gui.ModernDialog(this);
            MeGUI.core.gui.SettingsPage tpl = d.CurrentPage;
            d.Section("Script template");
            MeGUI.core.gui.SettingsCard c0 = d.Card();
            flowLayoutPanel1.Dock = DockStyle.None;
            flowLayoutPanel1.AutoSize = false;
            flowLayoutPanel1.WrapContents = true;
            foreach (Control b in flowLayoutPanel1.Controls)
            {
                Button bt = b as Button;
                if (bt != null)
                {
                    d.Btn(bt);
                    bt.Margin = new Padding(0, 0, d.S(8), d.S(4));
                }
            }
            flowLayoutPanel1.Size = new Size(d.S(600), d.S(76));
            c0.AddWide(null, null, flowLayoutPanel1);
            avisynthScript.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            avisynthScript.Dock = DockStyle.None;
            d.Block(avisynthScript, 600, 300, d.Card());
            d.Section("Plugins");
            MeGUI.core.gui.SettingsCard c1 = d.Card();
            dllBar.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            c1.AddRow("Load DLL", null, d.Bar(dllBar, 360));

            MeGUI.core.gui.SettingsPage extra = d.BeginPage();
            d.Section("Filters");
            MeGUI.core.gui.SettingsCard c2 = d.Card();
            c2.AddToggle(Tg(d, resize), null, d.Wide(resizeFilterType, 160));
            c2.AddToggle(Tg(d, noiseFilter), null, d.Wide(noiseFilterType, 160));
            d.Section("Misc");
            MeGUI.core.gui.SettingsCard c3 = d.Card();
            c3.AddToggle(Tg(d, dss2), null);
            c3.AddToggle(Tg(d, cbPreferAnimeDeinterlacing), null);
            d.Section("Mpeg options");
            MeGUI.core.gui.SettingsCard c4 = d.Card();
            c4.AddToggle(Tg(d, mpeg2Deblocking), null);
            c4.AddToggle(Tg(d, colourCorrect), null);
            d.Section("Resize options");
            MeGUI.core.gui.SettingsCard c5 = d.Card();
            c5.AddToggle(Tg(d, signalAR), null, d.Wide(mod16Box, 180));
            c5.AddRow("Acceptable anamorphic aspect error", null, d.Wide(acceptableAspectError, 90));
            c5.AddToggle(Tg(d, upsize), null);
            c5.AddRow("MOD value used for resizing", null, d.Wide(modValueBox, 90));
            d.Section("DGI source");
            MeGUI.core.gui.SettingsCard c6 = d.Card();
            c6.AddToggle(Tg(d, chkNvResize), null);

            AttachPage(templatePage, tpl);
            AttachPage(extraSetupPage, extra);
            Size = new Size(d.S(660), d.S(560));
            d.AttachTheme();
        }

        private static MeGUI.core.gui.ToggleSwitch Tg(MeGUI.core.gui.ModernDialog d, MeGUI.core.gui.ToggleSwitch t)
        {
            t.AutoSize = false;
            t.Size = new Size(d.S(96), d.S(26));
            return t;
        }

        private static void AttachPage(TabPage tab, MeGUI.core.gui.SettingsPage page)
        {
            tab.Controls.Clear();
            tab.Padding = new Padding(0);
            page.Dock = DockStyle.Fill;
            tab.Controls.Add(page);
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