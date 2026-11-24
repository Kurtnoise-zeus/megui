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
using System.Drawing;
using System.Windows.Forms;

namespace MeGUI.packages.tools.oneclick
{
    public partial class OneClickConfigPanel : UserControl, Editable<OneClickSettings>
    {
        private MainForm mainForm;
        List<OneClickAudioControl> audioConfigurations;

        #region profiles
        #region AVS profiles
        private void initAvsHandler()
        {
            // Init AVS handlers
            avsProfile.Manager = mainForm.Profiles;
        }
        #endregion
        #region Video profiles
        private void initVideoHandler()
        {
            videoProfile.Manager = mainForm.Profiles;
        }
        #endregion
        #endregion
        
        public OneClickConfigPanel() 
        {
            InitializeComponent();
            BuildModernLayout();
            mainForm = MainForm.Instance;
            // We do this because the designer will attempt to put such a long string in the resources otherwise
            containerFormatLabel.Text = "Since the possible output filetypes are not known until the input is configured, the output type cannot be configured in a profile. Instead, here is a list of known file-types. You choose which you are happy with, and MeGUI will attempt to encode to one of those on the list.";

            foreach (ContainerType t in mainForm.MuxProvider.GetSupportedContainers())
                containerTypeList.Items.Add(t.ToString());
   
            initAvsHandler();
            initVideoHandler();

            audioConfigurations = new List<OneClickAudioControl>();
            audioConfigurations.Add(oneClickAudioControl1);
            oneClickAudioControl1.SetDefault();
        }

        /// <summary>
        /// Windows 11 settings look: one page of cards per tab (see ModernDialog). The controls are only moved:
        /// names, events and the settings are unchanged.
        /// </summary>
        private void BuildModernLayout()
        {
            MeGUI.core.gui.ModernDialog d = new MeGUI.core.gui.ModernDialog(this);
            const int wide = 620;

            // video tab
            MeGUI.core.gui.SettingsPage video = d.CurrentPage;
            d.Section("Video setup");
            MeGUI.core.gui.SettingsCard videoCard = d.Card();
            d.ToggleIn(videoCard, chkDontEncodeVideo, null);
            usechaptersmarks.Text = "Force key frames for chapters marks";
            d.ToggleIn(videoCard, usechaptersmarks, null);
            videoCard.AddRow("Encoder", null, d.Wide(videoProfile, 380));
            videoCard.AddRow("Force input DAR", null, d.Wide(ar, 380));
            d.Section("AviSynth setup");
            MeGUI.core.gui.SettingsCard avsCard = d.Card();
            keepInputResolution.Text = "Keep input resolution (disable crop and resize)";
            d.ToggleIn(avsCard, keepInputResolution, null);
            d.ToggleIn(avsCard, autoCrop, null);
            avsCard.AddRow("Output resolution (max. width)", null, d.Wide(horizontalResolution, 120));
            avsCard.AddRow("AviSynth profile", null, d.Wide(avsProfile, 380));
            d.ToggleIn(avsCard, preprocessVideo, null);
            d.ToggleIn(avsCard, autoDeint, null);

            // audio tab
            MeGUI.core.gui.SettingsPage audio = d.BeginPage();
            d.Section("Audio setup (audio settings for specific languages)");
            oneClickAudioControl1.Dock = DockStyle.Fill;
            d.Block(audioTab, wide, 210, d.Card());

            // output tab
            MeGUI.core.gui.SettingsPage output = d.BeginPage();
            d.Section("File");
            MeGUI.core.gui.SettingsCard fileCard = d.Card();
            fileCard.AddRow("Filesize", null, d.Wide(fileSize, 340));
            fileCard.AddRow("Splitting", null, d.Wide(splitSize, 340));
            d.Section("Container");
            d.Message(containerFormatLabel, wide, 70);
            d.Block(containerTypeList, 320, 150, d.Card());

            // language tab
            MeGUI.core.gui.SettingsPage language = d.BeginPage();
            d.Section("Default audio language (tracks with this language will be preselected)");
            d.Block(Picker(d, lbDefaultAudio, lbNonDefaultAudio, btnAudioUp, btnRemoveAudio, btnAddAudio, btnAudioDown), wide, 120, d.Card());
            d.Section("Default subtitle language (tracks with this language will be preselected)");
            d.Block(Picker(d, lbDefaultSubtitle, lbNonDefaultSubtitle, btnSubtitleUp, btnRemoveSubtitle, btnAddSubtitle, btnSubtitleDown), wide, 120, d.Card());
            d.Section("Fallback");
            d.Row("Languages to select if the selection does not match", null, d.Wide(cbLanguageSelect, 140));

            // other tab
            MeGUI.core.gui.SettingsPage other = d.BeginPage();
            d.Section("Default directories");
            MeGUI.core.gui.SettingsCard dirs = d.Card();
            dirs.AddRow("Output", null, d.Bar(outputDirectory, 340), d.Btn(deleteOutput));
            dirs.AddRow("Working", null, d.Bar(workingDirectory, 340), d.Btn(deleteWorking));
            d.Section("Project name");
            MeGUI.core.gui.SettingsCard names = d.Card();
            names.AddRow("Prefix", null, d.Wide(txtPrefixName, 200));
            names.AddRow("Suffix", null, d.Wide(txtSuffixName, 200));
            names.AddRow("Replace", null, d.Wide(txtWorkingNameDelete, 200));
            names.AddRow("With", null, d.Wide(txtWorkingNameReplaceWith, 200));
            d.Section("Indexer / opener priority");
            d.Block(Picker(d, lbIndexerPriority, null, btnIndexerUp, null, null, btnIndexerDown), wide, 120, d.Card());
            d.Section("Intermediate files");
            MeGUI.core.gui.SettingsCard mkvCard = d.Card();
            chkDisableIntermediateMKV.Text = "Disable intermediate MKV file";
            d.ToggleIn(mkvCard, chkDisableIntermediateMKV, null);

            // each tab of the control gets its page of cards
            AttachPage(tabPage1, video);
            AttachPage(tabPage3, audio);
            AttachPage(tabPage2, output);
            AttachPage(tabPage4, language);
            AttachPage(tabPage5, other);
            tabControl1.Dock = DockStyle.Fill;
            Size = new Size(d.S(700), d.S(540));
            d.AttachTheme();
        }

        private static void AttachPage(TabPage tab, MeGUI.core.gui.SettingsPage page)
        {
            tab.Controls.Clear();
            tab.Padding = new Padding(0);
            page.Dock = DockStyle.Fill;
            tab.Controls.Add(page);
        }

        /// <summary>a list, the buttons to move / remove / add an item and (optionally) a second list, in one container</summary>
        private static Control Picker(MeGUI.core.gui.ModernDialog d, ListBox left, ListBox right, Button up, Button remove, Button add, Button down)
        {
            Panel p = new Panel();
            p.Size = new Size(d.S(520), d.S(112));
            left.Dock = DockStyle.None;
            left.Bounds = new Rectangle(0, 0, d.S(180), d.S(112));
            p.Controls.Add(left);
            int x = d.S(190);
            foreach (Button b in new Button[] { up, remove, add, down })
            {
                if (b == null)
                    continue;
                b.Dock = DockStyle.None;
                b.AutoSize = false;
                b.Size = new Size(d.S(44), d.S(28));
                p.Controls.Add(b);
            }
            up.Location = new Point(x, 0);
            if (remove != null)
                remove.Location = new Point(x, d.S(42));
            down.Location = new Point(x, d.S(84));
            if (add != null)
            {
                add.Location = new Point(x + d.S(52), d.S(42));
            }
            if (right != null)
            {
                right.Dock = DockStyle.None;
                right.Bounds = new Rectangle(x + d.S(104), 0, d.S(180), d.S(112));
                p.Controls.Add(right);
            }
            return p;
        }

        #region Gettable<OneClickSettings> Members

        public OneClickSettings Settings
        {
            get
            {
                OneClickSettings val = new OneClickSettings();
                val.AutomaticDeinterlacing = autoDeint.Checked;
                val.AvsProfileName = avsProfile.SelectedProfile.FQName;
                val.ContainerCandidates = ContainerCandidates;
                val.DontEncodeVideo = chkDontEncodeVideo.Checked;
                val.DisableIntermediateMKV = chkDisableIntermediateMKV.Checked;
                val.Filesize = fileSize.Value;
                val.OutputResolution = (long)horizontalResolution.Value;
                val.PrerenderVideo = preprocessVideo.Checked;
                val.SplitSize = splitSize.Value;
                val.AutoCrop = autoCrop.Checked;
                val.KeepInputResolution = keepInputResolution.Checked;
                val.VideoProfileName = videoProfile.SelectedProfile.FQName;
                val.UseChaptersMarks = usechaptersmarks.Checked;
                val.DefaultWorkingDirectory = workingDirectory.Filename;
                val.DefaultOutputDirectory = outputDirectory.Filename;

                List<OneClickAudioSettings> arrSettings = new List<OneClickAudioSettings>();
                for (int i = 0; i < audioConfigurations.Count; i++)
                {
                    bool bFound = false;
                    for (int j = 0; j < i; j++)
                    {
                        if (audioConfigurations[i].Language.Equals(audioConfigurations[j].Language))
                        {
                            bFound = true;
                            break;
                        }
                    }
                    if (!bFound)
                        arrSettings.Add(new OneClickAudioSettings(audioConfigurations[i].Language, audioConfigurations[i].EncoderProfile.FQName,
                            audioConfigurations[i].EncodingMode, audioConfigurations[i].UseFirstTrackOnly));
                }
                val.AudioSettings = arrSettings;

                List<string> arrDefaultAudio = new List<string>();
                foreach (string s in lbDefaultAudio.Items)
                    arrDefaultAudio.Add(s);
                val.DefaultAudioLanguage = arrDefaultAudio;

                List<string> arrDefaultSubtitle = new List<string>();
                foreach (string s in lbDefaultSubtitle.Items)
                    arrDefaultSubtitle.Add(s);
                val.DefaultSubtitleLanguage = arrDefaultSubtitle;

                List<string> arrIndexerPriority = new List<string>();
                foreach (string s in lbIndexerPriority.Items)
                    arrIndexerPriority.Add(s);

                if (cbLanguageSelect.SelectedItem.Equals("none"))
                    val.UseNoLanguagesAsFallback = true;
                else
                    val.UseNoLanguagesAsFallback = false;

                val.IndexerPriority = arrIndexerPriority;
                val.LeadingName = txtPrefixName.Text;
                val.SuffixName = txtSuffixName.Text;
                val.WorkingNameReplace = txtWorkingNameDelete.Text;
                val.WorkingNameReplaceWith = txtWorkingNameReplaceWith.Text;
                val.DAR = ar.Value;
                return val;
            }
            set
            {
                autoDeint.Checked = value.AutomaticDeinterlacing;
                avsProfile.SetProfileNameOrWarn(value.AvsProfileName);
                ContainerCandidates = value.ContainerCandidates;
                chkDontEncodeVideo.Checked = value.DontEncodeVideo;
                fileSize.Value = value.Filesize;
                horizontalResolution.Value = value.OutputResolution;
                preprocessVideo.Checked = value.PrerenderVideo;
                chkDisableIntermediateMKV.Checked = value.DisableIntermediateMKV;
                splitSize.Value = value.SplitSize;
                autoCrop.Checked = value.AutoCrop;
                keepInputResolution.Checked = value.KeepInputResolution;
                videoProfile.SetProfileNameOrWarn(value.VideoProfileName);
                usechaptersmarks.Checked = value.UseChaptersMarks;
                workingDirectory.Filename = value.DefaultWorkingDirectory;
                outputDirectory.Filename = value.DefaultOutputDirectory;
                txtWorkingNameDelete.Text = value.WorkingNameReplace;
                txtWorkingNameReplaceWith.Text = value.WorkingNameReplaceWith;
                txtPrefixName.Text = value.LeadingName;
                txtSuffixName.Text = value.SuffixName;

                int i = 0;
                AudioResetTrack();
                foreach (OneClickAudioSettings o in value.AudioSettings)
                {
                    if (i++ == 0)
                    {
                        audioConfigurations[0].SetProfileNameOrWarn(o.Profile);
                        audioConfigurations[0].EncodingMode = o.AudioEncodingMode;
                        audioConfigurations[0].UseFirstTrackOnly = o.UseFirstTrackOnly;
                    }
                    else
                        AudioAddTrack(o);
                }

                List<string> arrNonDefaultAudio = new List<string>(LanguageSelectionContainer.Languages.Keys);
                arrNonDefaultAudio.Add("[none]");
                foreach (string strLanguage in value.DefaultAudioLanguage)
                    arrNonDefaultAudio.Remove(strLanguage);
                
                List<string> arrNonDefaultSubtitle = new List<string>(LanguageSelectionContainer.Languages.Keys);
                arrNonDefaultSubtitle.Add("[none]");
                foreach (string strLanguage in value.DefaultSubtitleLanguage)
                    arrNonDefaultSubtitle.Remove(strLanguage);

                lbDefaultAudio.Items.Clear();
                lbDefaultAudio.Items.AddRange(value.DefaultAudioLanguage.ToArray());
                lbDefaultAudio_SelectedIndexChanged(null, null);
                lbNonDefaultAudio.Items.Clear();
                lbNonDefaultAudio.Items.AddRange(arrNonDefaultAudio.ToArray());
                lbNonDefaultAudio_SelectedIndexChanged(null, null);

                lbDefaultSubtitle.Items.Clear();
                lbDefaultSubtitle.Items.AddRange(value.DefaultSubtitleLanguage.ToArray());
                lbDefaultSubtitle_SelectedIndexChanged(null, null);
                lbNonDefaultSubtitle.Items.Clear();
                lbNonDefaultSubtitle.Items.AddRange(arrNonDefaultSubtitle.ToArray());
                lbNonDefaultSubtitle_SelectedIndexChanged(null, null);

                lbIndexerPriority.Items.Clear();
                lbIndexerPriority.Items.AddRange(value.IndexerPriority.ToArray());

                if (!value.UseNoLanguagesAsFallback)
                    cbLanguageSelect.SelectedItem = "all";
                else
                    cbLanguageSelect.SelectedItem = "none";

                ar.Value = value.DAR;
            }
        }

        #endregion

        private string[] ContainerCandidates
        {
            get
            {
                string[] val = new string[containerTypeList.CheckedItems.Count];
                containerTypeList.CheckedItems.CopyTo(val, 0);
                return val;
            }
            set
            {
                for (int i = 0; i < containerTypeList.Items.Count; i++)
                    containerTypeList.SetItemChecked(i, false);

                foreach (string val in value)
                {
                    int index = containerTypeList.Items.IndexOf(val);
                    if (index > -1)
                        containerTypeList.SetItemChecked(index, true);
                }
            }
        }

        private void keepInputResolution_CheckedChanged(object sender, EventArgs e)
        {
            if (keepInputResolution.Checked || chkDontEncodeVideo.Checked)
                horizontalResolution.Enabled = autoCrop.Enabled = false;
            else
                horizontalResolution.Enabled = autoCrop.Enabled = true;
        }

        private void videoProfile_SelectedProfileChanged(object sender, EventArgs e)
        {
            if (videoProfile.SelectedProfile.FQName.StartsWith("x264") && !chkDontEncodeVideo.Checked)
                usechaptersmarks.Enabled = true;
            else
                usechaptersmarks.Enabled = false;
        }

        private void chkDontEncodeVideo_CheckedChanged(object sender, EventArgs e)
        {
            if (chkDontEncodeVideo.Checked)
            {
                horizontalResolution.Enabled = autoCrop.Enabled = videoProfile.Enabled = false;
                usechaptersmarks.Enabled = keepInputResolution.Enabled = preprocessVideo.Enabled = false;
                autoDeint.Enabled = fileSize.Enabled = avsProfile.Enabled = false;
            }
            else
            {
                videoProfile.Enabled = keepInputResolution.Enabled = preprocessVideo.Enabled = true;
                autoDeint.Enabled = fileSize.Enabled = avsProfile.Enabled = true;
                if (videoProfile.SelectedProfile.FQName.StartsWith("x264"))
                    usechaptersmarks.Enabled = true;
                else
                    usechaptersmarks.Enabled = false;
                keepInputResolution_CheckedChanged(null, null);
            }
        }

        private void btnAddAudio_Click(object sender, EventArgs e)
        {
            List<string> arrAudio = new List<string>();
            foreach (string s in lbNonDefaultAudio.SelectedItems)
            {
                lbDefaultAudio.Items.Add(s);
                arrAudio.Add(s);
            }
            foreach (string s in arrAudio)
                lbNonDefaultAudio.Items.Remove(s);
        }

        private void btnRemoveAudio_Click(object sender, EventArgs e)
        {
            List<string> arrAudio = new List<string>();
            foreach (string s in lbDefaultAudio.SelectedItems)
            {
                lbNonDefaultAudio.Items.Add(s);
                arrAudio.Add(s);
            }
            foreach (string s in arrAudio)
                lbDefaultAudio.Items.Remove(s);
        }

        private void btnAudioUp_Click(object sender, EventArgs e)
        {
            int iPos = lbDefaultAudio.SelectedIndex;
            if (iPos < 1)
                return;

            object o = lbDefaultAudio.SelectedItem;
            lbDefaultAudio.Items.RemoveAt(iPos);
            lbDefaultAudio.Items.Insert(iPos - 1, o);
            lbDefaultAudio.SelectedIndex = iPos - 1;
        }

        private void btnAudioDown_Click(object sender, EventArgs e)
        {
            int iPos = lbDefaultAudio.SelectedIndex;
            if (iPos < 0 || iPos > lbDefaultAudio.Items.Count - 2)
                return;

            object o = lbDefaultAudio.SelectedItem;
            lbDefaultAudio.Items.RemoveAt(iPos);
            lbDefaultAudio.Items.Insert(iPos + 1, o);
            lbDefaultAudio.SelectedIndex = iPos + 1;
        }

        private void lbDefaultAudio_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbDefaultAudio.SelectedIndex < 0)
            {
                btnRemoveAudio.Enabled = btnAudioUp.Enabled = btnAudioDown.Enabled = false;
                return;
            }
            btnRemoveAudio.Enabled = true;
            if (lbDefaultAudio.SelectedIndex == 0)
                btnAudioUp.Enabled = false;
            else
                btnAudioUp.Enabled = true;
            if (lbDefaultAudio.SelectedIndex == lbDefaultAudio.Items.Count - 1)
                btnAudioDown.Enabled = false;
            else
                btnAudioDown.Enabled = true;
        }

        private void lbNonDefaultAudio_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbNonDefaultAudio.SelectedIndex < 0)
                btnAddAudio.Enabled = false;
            else
                btnAddAudio.Enabled = true;
        }

        private void btnAddSubtitle_Click(object sender, EventArgs e)
        {
            List<string> arrSubtitle = new List<string>();
            foreach (string s in lbNonDefaultSubtitle.SelectedItems)
            {
                lbDefaultSubtitle.Items.Add(s);
                arrSubtitle.Add(s);
            }
            foreach (string s in arrSubtitle)
                lbNonDefaultSubtitle.Items.Remove(s);
        }

        private void btnRemoveSubtitle_Click(object sender, EventArgs e)
        {
            List<string> arrSubtitle = new List<string>();
            foreach (string s in lbDefaultSubtitle.SelectedItems)
            {
                lbNonDefaultSubtitle.Items.Add(s);
                arrSubtitle.Add(s);
            }
            foreach (string s in arrSubtitle)
                lbDefaultSubtitle.Items.Remove(s);
        }

        private void btnSubtitleUp_Click(object sender, EventArgs e)
        {
            int iPos = lbDefaultSubtitle.SelectedIndex;
            if (iPos < 1)
                return;

            object o = lbDefaultSubtitle.SelectedItem;
            lbDefaultSubtitle.Items.RemoveAt(iPos);
            lbDefaultSubtitle.Items.Insert(iPos - 1, o);
            lbDefaultSubtitle.SelectedIndex = iPos - 1;
        }

        private void btnSubtitleDown_Click(object sender, EventArgs e)
        {
            int iPos = lbDefaultSubtitle.SelectedIndex;
            if (iPos < 0 || iPos > lbDefaultSubtitle.Items.Count - 2)
                return;

            object o = lbDefaultSubtitle.SelectedItem;
            lbDefaultSubtitle.Items.RemoveAt(iPos);
            lbDefaultSubtitle.Items.Insert(iPos + 1, o);
            lbDefaultSubtitle.SelectedIndex = iPos + 1;
        }

        private void lbDefaultSubtitle_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbDefaultSubtitle.SelectedIndex < 0)
            {
                btnRemoveSubtitle.Enabled = btnSubtitleUp.Enabled = btnSubtitleDown.Enabled = false;
                return;
            }
            btnRemoveSubtitle.Enabled = true;
            if (lbDefaultSubtitle.SelectedIndex == 0)
                btnSubtitleUp.Enabled = false;
            else
                btnSubtitleUp.Enabled = true;
            if (lbDefaultSubtitle.SelectedIndex == lbDefaultSubtitle.Items.Count - 1)
                btnSubtitleDown.Enabled = false;
            else
                btnSubtitleDown.Enabled = true;
        }

        private void lbNonDefaultSubtitle_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbNonDefaultSubtitle.SelectedIndex < 0)
                btnAddSubtitle.Enabled = false;
            else
                btnAddSubtitle.Enabled = true;
        }

        private void btnIndexerUp_Click(object sender, EventArgs e)
        {
            int iPos = lbIndexerPriority.SelectedIndex;
            if (iPos < 1)
                return;

            object o = lbIndexerPriority.SelectedItem;
            lbIndexerPriority.Items.RemoveAt(iPos);
            lbIndexerPriority.Items.Insert(iPos - 1, o);
            lbIndexerPriority.SelectedIndex = iPos - 1;
            lbIndexerPriority_MouseClick(null, null);
        }

        private void btnIndexerDown_Click(object sender, EventArgs e)
        {
            int iPos = lbIndexerPriority.SelectedIndex;
            if (iPos < 0 || iPos > lbIndexerPriority.Items.Count - 2)
                return;

            object o = lbIndexerPriority.SelectedItem;
            lbIndexerPriority.Items.RemoveAt(iPos);
            lbIndexerPriority.Items.Insert(iPos + 1, o);
            lbIndexerPriority.SelectedIndex = iPos + 1;
            lbIndexerPriority_MouseClick(null, null);
        }

        private void lbIndexerPriority_MouseClick(object sender, MouseEventArgs e)
        {
            int iPos = lbIndexerPriority.SelectedIndex;
            btnIndexerUp.Enabled = iPos > 0;
            btnIndexerDown.Enabled = iPos >= 0 && iPos <= lbIndexerPriority.Items.Count - 2;
        }

        private int iSelectedAudioTabPage = -1;
        private void audioTab_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right)
                return;

            System.Drawing.Point p = e.Location;
            for (int i = 0; i < audioTab.TabCount; i++)
            {
                System.Drawing.Rectangle rect = audioTab.GetTabRect(i);
                rect.Offset(2, 2);
                rect.Width -= 4;
                rect.Height -= 4;
                if (rect.Contains(p))
                {
                    iSelectedAudioTabPage = i;
                    audioMenu.Show(audioTab, e.Location);
                    break;
                }
            }
        }

        private void audioAddTrack_Click(object sender, EventArgs e)
        {
            AudioAddTrack(null);
        }

        private void audioRemoveTrack_Click(object sender, EventArgs e)
        {
            AudioRemoveTrack(iSelectedAudioTabPage);
        }

        private void AudioAddTrack(OneClickAudioSettings oSettings)
        {
            if (oSettings == null)
                oSettings = new OneClickAudioSettings("English",
                    audioConfigurations[0].EncoderProfile.FQName, audioConfigurations[0].EncodingMode, audioConfigurations[0].UseFirstTrackOnly);

            TabPage p = new TabPage(oSettings.Language);
            p.UseVisualStyleBackColor = audioTab.TabPages[0].UseVisualStyleBackColor;
            p.Padding = audioTab.TabPages[0].Padding;

            OneClickAudioControl a = new OneClickAudioControl();
            a.Dock = audioConfigurations[0].Dock;
            a.Padding = audioConfigurations[0].Padding;
            a.Location = audioConfigurations[0].Location;
            a.EncodingMode = oSettings.AudioEncodingMode;
            a.SetProfileNameOrWarn(oSettings.Profile);
            a.Language = oSettings.Language;
            a.UseFirstTrackOnly = oSettings.UseFirstTrackOnly;
            a.LanguageChanged += new EventHandler(audio1_LanguageChanged);
            audioConfigurations.Add(a);
            
            audioTab.TabPages.Insert(audioTab.TabCount - 1, p);
            p.Controls.Add(a);
            audioTab.SelectedTab = p;
        }

        private void AudioResetTrack()
        {
            // delete all tracks beside the first and last one
            for (int i = audioTab.TabCount - 1; i > 1; i--)
                audioTab.TabPages.RemoveAt(i - 1);
            for (int i = audioConfigurations.Count - 1; i > 0; i--)
                audioConfigurations.RemoveAt(i);
        }

        private void audio1_LanguageChanged(object sender, EventArgs e)
        {
            for (int i = 1; i < audioTab.TabCount - 1; i++)
                audioTab.TabPages[i].Text = audioConfigurations[i].Language;
        }

        private void AudioRemoveTrack(int iTabPageIndex)
        {
            if (iTabPageIndex == 0 || iTabPageIndex == audioTab.TabCount - 1)
                return;

            audioTab.TabPages.RemoveAt(iTabPageIndex);
            audioConfigurations.RemoveAt(iTabPageIndex);
            if (iTabPageIndex < audioTab.TabCount - 1)
                audioTab.SelectedIndex = iTabPageIndex;
            else
                audioTab.SelectedIndex = iTabPageIndex - 1;
        }

        private void audioTab_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (audioTab.SelectedTab.Text.Equals("   +"))
                AudioAddTrack(null);
        }

        private void audioTab_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back)
                AudioRemoveTrack(audioTab.SelectedIndex);
        }

        private void audioTab_VisibleChanged(object sender, EventArgs e)
        {
            if (!audioTab.Visible || audioTab.TabCount == audioConfigurations.Count + 1)
                return;

            List<OneClickAudioSettings> arrSettings = new List<OneClickAudioSettings>();
            foreach (OneClickAudioControl o in audioConfigurations)
                arrSettings.Add(new OneClickAudioSettings(o.Language, o.EncoderProfile.FQName, o.EncodingMode, o.UseFirstTrackOnly));
            AudioResetTrack();
            int i = 0;
            foreach (OneClickAudioSettings o in arrSettings)
            {
                if (i++ == 0)
                {
                    audioConfigurations[0].SetProfileNameOrWarn(o.Profile);
                    audioConfigurations[0].EncodingMode = o.AudioEncodingMode;
                }
                else
                    AudioAddTrack(o);
            }
        }

        private void audioTab_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            AudioAddTrack(null);
        }

        private void audioMenu_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            audioRemoveTrack.Enabled = (iSelectedAudioTabPage != audioConfigurations.Count);
        }

        private void deleteOutput_Click(object sender, EventArgs e)
        {
            outputDirectory.Filename = string.Empty;
        }

        private void deleteWorking_Click(object sender, EventArgs e)
        {
            workingDirectory.Filename = string.Empty;
        }
    }
}