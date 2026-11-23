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
using System.IO;
using System.Text;
using System.Xml;
using System.Windows.Forms;
using System.Drawing;
using Gui = MeGUI.core.gui;

namespace MeGUI
{
	/// <summary>
	/// Summary description for SettingsForm.
	/// </summary>
	public partial class SettingsForm : Form
	{
		#region variables
        private MeGUISettings internalSettings = new MeGUISettings();
        private XmlDocument ContextHelp = new XmlDocument();
        private AutoEncodeDefaultsSettings autoEncodeDefaults;
        private SourceDetectorSettings sdSettings;
		#endregion

		#region start / stop
		public SettingsForm()
		{
			InitializeComponent();
            InitThemeSelector();
            List<string> keys = new List<string>(LanguageSelectionContainer.Languages.Keys);
            defaultLanguage2.DataSource = defaultLanguage1.DataSource = keys;
            defaultLanguage2.BindingContext = new BindingContext();
            defaultLanguage1.BindingContext = new BindingContext();
            SetToolTips();
            ffmsThreads.Maximum = System.Environment.ProcessorCount;
            chk64Bit.Visible = !MainForm.Instance.Settings.IsMeGUIx64 && MainForm.Instance.Settings.IsOSx64;

            // worker tab
            workerMaximumCount.Value = MainForm.Instance.Settings.WorkerMaximumCount;
            workerJobsListBox.DataSource = EnumProxy.CreateArray(new object[] { JobType.Audio, JobType.Demuxer, JobType.Indexer, JobType.Muxer, JobType.OneClick, JobType.Video });
            cbJobType.DataSource = EnumProxy.CreateArray(new object[] { JobType.Audio, JobType.Demuxer, JobType.Indexer, JobType.Muxer, JobType.OneClick, JobType.Video });
            cbAutoStartOnStartup.Checked = MainForm.Instance.Settings.WorkerAutoStartOnStartup;
            cbAutoStart.Checked = MainForm.Instance.Settings.WorkerAutoStart;
            cbRemoveJob.Checked = MainForm.Instance.Settings.WorkerRemoveJob;
            UpdateWorkerSettingsListBox();

            // DPI rescale
            if (MainForm.Instance != null)
            {
                workerJobsListBox.Height = MainForm.Instance.Settings.DPIRescale(84);
                workerSettingsListBox.Height = MainForm.Instance.Settings.DPIRescale(160);
                numericUpDown1.Location = new System.Drawing.Point(numericUpDown1.Location.X, workerJobsListBox.Height + workerJobsListBox.Location.Y - MainForm.Instance.Settings.DPIRescale(numericUpDown1.Height));

                cbStandbySettings.DataSource = EnumProxy.CreateArray(new object[] { MeGUISettings.StandbySettings.SystemDefault, MeGUISettings.StandbySettings.DisableSystemStandby, MeGUISettings.StandbySettings.DisableMonitorStandby });
            }

            BuildModernLayout();
            MeGUI.core.gui.ThemeManager.ThemeWindow(this); // no flash of light colors when the dark theme is on
        }

        private ComboBox cbTheme;

        /// <summary>
        /// Creates the colour theme selector (System / Dark / Light); it is shown in the Main page
        /// </summary>
        private void InitThemeSelector()
        {
            cbTheme = new ComboBox();
            cbTheme.DropDownStyle = ComboBoxStyle.DropDownList;
            cbTheme.Items.AddRange(new object[] { "System (follow Windows)", "Dark", "Light" });
            cbTheme.SelectedIndex = 0;
        }

        /// <summary>
        /// Sets any required tooltips
        /// </summary>
        private void SetToolTips()
        {
            try
            {
                string p = System.IO.Path.Combine(Application.StartupPath, "Data");
                p = System.IO.Path.Combine(p, "ContextHelp.xml");             
                ContextHelp.Load(p);
            }
            catch
            {
                MessageBox.Show("The ContextHelp.xml file could not be found. Please check in the 'Data' directory to see if it exists. Help tooltips will not be available.", "File Not Found", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            toolTipHelp.SetToolTip(chx264ExternalMuxer, SelectHelpText("alwaysmuxmkv"));
            toolTipHelp.SetToolTip(ffmsThreads, SelectHelpText("ffmsthreads"));
            toolTipHelp.SetToolTip(cbUseITUValues, SelectHelpText("useituvalues"));
        }

        /// <summary>
        /// Gets the help text
        /// </summary>
        private string SelectHelpText(string node)
        {
            StringBuilder HelpText = new StringBuilder(64);

            try
            {
                string xpath = "/ContextHelp/Form[@name='SettingsForm']/" + node;
                XmlNodeList nl = ContextHelp.SelectNodes(xpath); // Return the details for the specified node

                if (nl.Count == 1) // if it finds the required HelpText, count should be 1
                {
                    HelpText.Append(nl[0].Attributes["name"].Value);
                    HelpText.AppendLine();
                    HelpText.AppendLine(nl[0]["Text"].InnerText);
                }
                else // If count isn't 1, then theres no valid data.
                    HelpText.Append("Error: No data available");
            }
            catch
            {
                HelpText.Append("Error: No data available");
            }

            return (HelpText.ToString());
        }
        #endregion

        #region Windows 11 settings look
        // navigation list + pages of rounded cards, built with the existing controls (names, events and the
        // Settings property are untouched: the controls are only moved from the tab pages into the cards)
        private float uiScale = 1f;
        private Font sectionFont;
        private Gui.SettingsNav nav;
        private Label pageTitle;
        private Gui.SettingsSurface footer;
        private readonly List<Gui.SettingsPage> pages = new List<Gui.SettingsPage>();
        private readonly List<Button> flatButtons = new List<Button>();

        private int S(int value)
        {
            return (int)Math.Round(value * uiScale);
        }

        private void BuildModernLayout()
        {
            uiScale = MainForm.Instance != null ? MainForm.Instance.Settings.DPIRescale(1000) / 1000f : 1f;
            SuspendLayout();

            // all the sizes below are computed with uiScale: no automatic scaling on top of it
            AutoScaleMode = AutoScaleMode.None;
            Font = new Font("Segoe UI", 10F);
            sectionFont = new Font("Segoe UI Semibold", 10.5F);
            FormBorderStyle = FormBorderStyle.Sizable;
            MaximizeBox = true;
            Rectangle area = Screen.PrimaryScreen.WorkingArea;
            ClientSize = new Size(Math.Min(S(1040), area.Width - S(60)), Math.Min(S(780), area.Height - S(120)));
            MinimumSize = new Size(Math.Min(S(760), area.Width), Math.Min(S(520), area.Height));

            // the pages
            BuildMainPage();
            BuildExtendedPage();
            BuildWorkerPage();
            BuildExternalPage();

            // the window: pages (fill), title, navigation (left), footer (bottom). Added in this order: the
            // docked controls added last are laid out first
            Controls.Clear();

            Gui.SettingsSurface content = new Gui.SettingsSurface(Gui.SettingsSurfaceKind.Window);
            content.Dock = DockStyle.Fill;
            foreach (Gui.SettingsPage page in pages)
            {
                page.Visible = false;
                content.Controls.Add(page);
            }
            pageTitle = new Label();
            pageTitle.AutoSize = false;
            pageTitle.Dock = DockStyle.Top;
            pageTitle.Height = S(66);
            pageTitle.Padding = new Padding(S(34), S(6), 0, 0);
            pageTitle.TextAlign = ContentAlignment.MiddleLeft;
            pageTitle.Font = new Font("Segoe UI Semibold", 20F);
            pageTitle.UseMnemonic = false;
            content.Controls.Add(pageTitle);

            nav = new Gui.SettingsNav(uiScale);
            nav.Dock = DockStyle.Left;
            nav.Width = S(260);
            nav.AddItem(tabPage1.Text, "");
            nav.AddItem(tabPage3.Text, "");
            nav.AddItem(tabPage4.Text, "");
            nav.AddItem(tabPage2.Text, "");
            nav.SelectedIndexChanged += delegate { ShowPage(nav.SelectedIndex); };

            footer = new Gui.SettingsSurface(Gui.SettingsSurfaceKind.Footer);
            footer.Dock = DockStyle.Bottom;
            footer.Height = S(64);
            foreach (Control c in new Control[] { helpButton1, saveButton, cancelButton })
            {
                c.Dock = DockStyle.None;
                c.Anchor = AnchorStyles.Top | AnchorStyles.Left;
                footer.Controls.Add(c);
            }
            saveButton.Size = new Size(S(120), S(34));
            cancelButton.Size = new Size(S(120), S(34));
            flatButtons.Add(saveButton);
            flatButtons.Add(cancelButton);
            footer.Resize += delegate { LayoutFooter(); };
            LayoutFooter();

            Controls.Add(content);
            Controls.Add(nav);
            Controls.Add(footer);

            ApplyModernTheme();
            Gui.ThemeManager.ThemeChanged += OnModernThemeChanged;
            Disposed += delegate { Gui.ThemeManager.ThemeChanged -= OnModernThemeChanged; };

            ResumeLayout(true);
            nav.SelectedIndex = 0;
        }

        private void LayoutFooter()
        {
            int top = (footer.Height - saveButton.Height) / 2;
            cancelButton.Location = new Point(footer.Width - S(24) - cancelButton.Width, top);
            saveButton.Location = new Point(cancelButton.Left - S(10) - saveButton.Width, top);
            helpButton1.Location = new Point(S(24), (footer.Height - helpButton1.Height) / 2);
        }

        private void ShowPage(int index)
        {
            if (index < 0 || index >= pages.Count)
                return;
            string[] titles = { tabPage1.Text, tabPage3.Text, tabPage4.Text, tabPage2.Text };
            pageTitle.Text = titles[index];
            for (int i = 0; i < pages.Count; i++)
                pages[i].Visible = (i == index);
        }

        private void OnModernThemeChanged(object sender, EventArgs e)
        {
            // may be raised from another thread (system colour change)
            if (IsHandleCreated && !IsDisposed)
                BeginInvoke(new MethodInvoker(ApplyModernTheme));
        }

        private void ApplyModernTheme()
        {
            foreach (Button b in flatButtons)
                StyleButton(b, b == saveButton);
            Invalidate(true);
        }

        /// <summary>flat button with the colours of the palette (accent colour for the main button)</summary>
        private void StyleButton(Button b, bool primary)
        {
            Gui.ThemePalette p = Gui.ThemeManager.Palette;
            bool dark = Gui.ThemeManager.IsDark;
            b.UseVisualStyleBackColor = false;
            b.FlatStyle = FlatStyle.Flat;
            if (primary)
            {
                b.BackColor = p.Accent;
                b.ForeColor = p.OnAccent;
                b.FlatAppearance.BorderColor = p.Accent;
                b.FlatAppearance.MouseOverBackColor = Gui.ThemeManager.Blend(p.Accent, dark ? Color.White : Color.Black, 0.12);
                b.FlatAppearance.MouseDownBackColor = Gui.ThemeManager.Blend(p.Accent, dark ? Color.Black : Color.White, 0.15);
            }
            else
            {
                b.BackColor = Gui.ThemeManager.Blend(p.Surface, p.Text, dark ? 0.08 : 0.02);
                b.ForeColor = p.Text;
                b.FlatAppearance.BorderColor = Gui.ThemeManager.Blend(p.Surface, p.Text, 0.18);
                b.FlatAppearance.MouseOverBackColor = Gui.ThemeManager.Blend(p.Surface, p.Text, 0.14);
                b.FlatAppearance.MouseDownBackColor = Gui.ThemeManager.Blend(p.Surface, p.Text, 0.10);
            }
        }

        #region helpers to fill the pages
        private Gui.SettingsPage NewPage()
        {
            Gui.SettingsPage page = new Gui.SettingsPage(uiScale);
            pages.Add(page);
            return page;
        }

        private void Section(Gui.SettingsPage page, string title)
        {
            page.AddSection(title, sectionFont);
        }

        private static string Cap(string text)
        {
            text = text.Replace("  ", " ").Trim().TrimEnd(':').Trim();
            return text.Length == 0 ? text : char.ToUpper(text[0]) + text.Substring(1);
        }

        /// <summary>a card with a toggle switch (its Text is the title); other controls can go on the left of the switch</summary>
        private void ToggleRow(Gui.SettingsPage page, Gui.ToggleSwitch toggle, string description, params Control[] leftOfSwitch)
        {
            toggle.AutoSize = false;
            toggle.Size = new Size(S(96), S(26));
            page.AddCard().AddToggle(toggle, description, leftOfSwitch);
        }

        /// <summary>a card with a title and the controls of the setting on the right</summary>
        private void ValueRow(Gui.SettingsPage page, string title, string description, params Control[] controls)
        {
            page.AddCard().AddRow(title, description, controls);
        }

        private Control Wide(Control c, int width)
        {
            c.Width = S(width);
            return c;
        }

        private Button Btn(Button b)
        {
            b.AutoSize = false;
            b.Height = S(32);
            b.Width = b.Text.Length <= 2
                ? S(34)
                : Math.Max(S(96), TextRenderer.MeasureText(b.Text, Font).Width + S(36));
            if (!flatButtons.Contains(b))
                flatButtons.Add(b);
            return b;
        }

        private Control Bar(FileBar bar, int width)
        {
            bar.Size = new Size(S(width), S(30));
            return bar;
        }
        #endregion

        #region pages
        private Gui.SettingsPage BuildMainPage()
        {
            Gui.SettingsPage page = NewPage();

            Section(page, "Appearance");
            ValueRow(page, "Color theme", "Applies to all MeGUI windows", Wide(cbTheme, 240));

            Section(page, "General");
            ToggleRow(page, chkDebugInformation, null);
            ToggleRow(page, cbUseITUValues, null);
            ToggleRow(page, openProgressWindow, null);
            ToggleRow(page, deleteIntermediateFiles, null);
            ToggleRow(page, deleteAbortedOutput, null);
            ToggleRow(page, openScript, null);

            Section(page, "Miscellaneous");
            ValueRow(page, Cap(lblMinimumLength.Text), null, Wide(minimumTitleLength, 90));
            ValueRow(page, Cap(label15.Text), null, Wide(acceptableFPSError, 90));
            ValueRow(page, "Dialogs", null, Btn(resetDialogs));
            ValueRow(page, "Source detector", null, Btn(configSourceDetector));

            Section(page, "Default output");
            ValueRow(page, "Default output directory", null, Bar(defaultOutputDir, 340), Btn(btnClearOutputDirecoty));
            ValueRow(page, "Custom file size values", null, Wide(targetSizeSCBox1, 300));

            Section(page, "Default languages");
            ValueRow(page, "Default language 1", null, Wide(defaultLanguage1, 220));
            ValueRow(page, "Default language 2", null, Wide(defaultLanguage2, 220));

            Section(page, "AviSynth / AviSynth+");
            ToggleRow(page, chkEnableHwd, null, Wide(cbHwdDecoder, 180));
            ToggleRow(page, chkInput8Bit, null);
            ValueRow(page, Cap(lblffmsThreads.Text), null, Wide(ffmsThreads, 90));
            ToggleRow(page, chkDirectShowSource, null);
            return page;
        }

        private Gui.SettingsPage BuildExtendedPage()
        {
            Gui.SettingsPage page = NewPage();

            Section(page, "Auto update");
            ToggleRow(page, useAutoUpdateCheckbox, null);
            ValueRow(page, "Update servers", null, Wide(cbAutoUpdateServerSubList, 200), Btn(configureServersButton));
            ToggleRow(page, backupfiles, null);

            Section(page, "Network");
            Gui.SettingsCard proxy = page.AddCard();
            proxy.AddRow("HTTP proxy for auto update", null, Wide(cbHttpProxyMode, 220));
            proxy.AddSubRow("Server", Wide(txt_httpproxyaddress, 260));
            proxy.AddSubRow("Port", Wide(txt_httpproxyport, 100));
            proxy.AddSubRow("Login", Wide(txt_httpproxyuid, 220));
            proxy.AddSubRow("Password", Wide(txt_httpproxypwd, 220));

            Section(page, "After encoding");
            Gui.SettingsCard after = page.AddCard();
            foreach (RadioButton r in new RadioButton[] { donothing, rbCloseMeGUI, shutdown, runCommand })
                r.AutoSize = true;
            after.AddLeft(donothing);
            after.AddLeft(rbCloseMeGUI);
            after.AddLeft(shutdown);
            after.AddLeft(runCommand, Wide(command, 320));

            Section(page, "Video preview");
            ToggleRow(page, chkEnsureCorrectPlaybackSpeed, null);
            ToggleRow(page, cbAddTimePos, null);
            ToggleRow(page, chAlwaysOnTop, null);

            Section(page, "Automated encoding");
            ValueRow(page, "AutoEncode defaults", null, Btn(configAutoEncodeDefaults));
            ValueRow(page, Cap(label13.Text), null, Wide(nbPasses, 90));
            ToggleRow(page, keep2ndPassLogFile, null);
            ToggleRow(page, keep2ndPassOutput, null);

            Section(page, "Optional output extensions");
            ValueRow(page, Cap(label12.Text), null, Wide(videoExtension, 160));
            ValueRow(page, Cap(label11.Text), null, Wide(audioExtension, 160));
            ValueRow(page, Cap(lblForcedName.Text), null, Wide(txtForcedName, 240));
            return page;
        }

        private Gui.SettingsPage BuildWorkerPage()
        {
            Gui.SettingsPage page = NewPage();

            Section(page, "Global worker settings");
            ValueRow(page, Cap(label6.Text), null, Wide(workerMaximumCount, 90));
            ToggleRow(page, cbAutoStart, null);
            ToggleRow(page, cbAutoStartOnStartup, null);
            ToggleRow(page, cbRemoveJob, null);
            ValueRow(page, "Standby", Cap(label7.Text), Wide(cbStandbySettings, 240));

            Section(page, "Worker priority");
            ValueRow(page, Cap(label3.Text), null, Wide(cbJobType, 200));
            ValueRow(page, Cap(label5.Text), null, Wide(cbProcessPriority, 200));
            ValueRow(page, Cap(label4.Text), null, Wide(cbIOPriority, 200));

            Section(page, "Worker rules");
            workerSettingsListBox.Height = S(130);
            page.AddCard().AddWide("Worker rule sets", "Defines how many jobs of specific types can run in parallel",
                workerSettingsListBox, Btn(btnDeleteSettings), Btn(btnResetSettings));

            workerJobsListBox.Height = S(130);
            label2.AutoSize = false;
            label2.Height = S(40);
            page.AddCard().AddWide("New worker rule set", "Select the job types, then the maximum number of parallel jobs",
                workerJobsListBox, label2, numericUpDown1, Btn(btnAddSettings));
            return page;
        }

        private Gui.SettingsPage BuildExternalPage()
        {
            Gui.SettingsPage page = NewPage();

            Section(page, "AAC encoders");
            ProgramCard(page, useQAAC, lblQaac, qaacLocation);
            ProgramCard(page, useFDKAac, lblFDK, fdkaacLocation);
            ProgramCard(page, useNeroAacEnc, lblNero, neroaacencLocation);

            Section(page, "DGIndex tools");
            ToggleRow(page, useDGIndexIM, null);
            ToggleRow(page, useDGIndexNV, null);
            ToggleRow(page, cbAutoLoadDG, null);
            autoForceFilm.AutoSize = false;
            autoForceFilm.Size = new Size(S(96), S(26));
            Gui.SettingsCard film = page.AddCard();
            film.AddToggle(autoForceFilm, null);
            film.AddSubRow("Percent", Wide(forceFilmPercentage, 90));

            Section(page, "Misc");
            if (!MainForm.Instance.Settings.IsMeGUIx64 && MainForm.Instance.Settings.IsOSx64)
                ToggleRow(page, chk64Bit, null);
            ToggleRow(page, chx264ExternalMuxer, null);
            ValueRow(page, "Temp directory for MP4 muxer", null, Bar(tempDirMP4, 340), Btn(btnClearMP4TempDirectory));
            return page;
        }

        /// <summary>an encoder: enable switch, and its location under it</summary>
        private void ProgramCard(Gui.SettingsPage page, Gui.ToggleSwitch enable, Label locationLabel, FileBar location)
        {
            enable.AutoSize = false;
            enable.Size = new Size(S(96), S(26));
            Gui.SettingsCard card = page.AddCard();
            card.AddToggle(enable, null);
            card.AddSubRow(Cap(locationLabel.Text), Bar(location, 380));
        }
        #endregion
        #endregion

        #region worker tab
        private void UpdateWorkerSettingsListBox()
        {
            workerSettingsListBox.Items.Clear();
            foreach (WorkerSettings oSettings in MainForm.Instance.Settings.WorkerSettings)
            {
                string strText = string.Empty;
                foreach (JobType oType in oSettings.JobTypes)
                {
                    strText += oType.ToString() + " Jobs + ";
                }
                strText = strText.Substring(0, strText.Length - 2) + " <= " + oSettings.MaxCount + " Job(s) in parallel";
                workerSettingsListBox.Items.Add(strText);
            }
        }

        private void BtnAddSettings_Click(object sender, EventArgs e)
        {
            if (workerJobsListBox.CheckedItems.Count == 0)
            {
                MessageBox.Show("Please select at least one job type.", "Add Worker Rule", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            WorkerSettings oSettings = new WorkerSettings();
            for (int i = 0; i < workerJobsListBox.Items.Count; i++)
            {
                if (workerJobsListBox.GetItemCheckState(i) != CheckState.Checked)
                    continue;
                JobType oType = (JobType)((EnumProxy)workerJobsListBox.Items[i]).RealValue;
                oSettings.JobTypes.Add(oType);
            }
            oSettings.MaxCount = (int)numericUpDown1.Value;

            foreach (WorkerSettings oSettingsExisting in MainForm.Instance.Settings.WorkerSettings)
            {
                if (oSettingsExisting.HasSameJobTypeList(oSettings))
                {
                    if (oSettingsExisting.MaxCount != oSettings.MaxCount)
                    {
                        oSettingsExisting.MaxCount = oSettings.MaxCount;
                        UpdateWorkerSettingsListBox();
                    }
                    else
                        MessageBox.Show("This rule set is already in the list.", "Add Worker Rule", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            MainForm.Instance.Settings.WorkerSettings.Add(oSettings);
            UpdateWorkerSettingsListBox();
        }

        private void BtnDeleteSettings_Click(object sender, EventArgs e)
        {
            int i = workerSettingsListBox.SelectedIndex;
            if (i < 0)
            {
                MessageBox.Show("Please select a rule set first.", "Remove Worker Rule", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            MainForm.Instance.Settings.WorkerSettings.RemoveAt(i);
            UpdateWorkerSettingsListBox();
        }

        private void WorkerMaximumCount_ValueChanged(object sender, EventArgs e)
        {
            if (MainForm.Instance.Settings.WorkerMaximumCount == (int)workerMaximumCount.Value)
                return;

            MainForm.Instance.Settings.WorkerMaximumCount = (int)workerMaximumCount.Value;
            MainForm.Instance.Jobs.AdjustWorkerCount(MainForm.Instance.Jobs.IsAnyWorkerRunning);
        }

        private void WorkerSettingsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnDeleteSettings.Enabled = workerSettingsListBox.SelectedIndex >= 0;
        }

        private void BtnResetSettings_Click(object sender, EventArgs e)
        {
            MainForm.Instance.Settings.ResetWorkerSettings();
            MainForm.Instance.Settings.ResetWorkerPriority();
            workerMaximumCount.Value = MainForm.Instance.Settings.WorkerMaximumCount;
            UpdateWorkerSettingsListBox();
            CbJobType_SelectedIndexChanged(null, null);
            MainForm.Instance.Jobs.AdjustWorkerCount(MainForm.Instance.Jobs.IsAnyWorkerRunning);
        }

        bool doNotSave = false;
        private void CbJobType_SelectedIndexChanged(object sender, EventArgs e)
        {
            doNotSave = true;
            JobType oType = (JobType)((EnumProxy)cbJobType.SelectedItem).RealValue;
            foreach (WorkerPriority oPriority in MainForm.Instance.Settings.WorkerPriority)
            {
                if (oType == oPriority.JobType)
                {
                    cbProcessPriority.SelectedIndex = (int)oPriority.Priority;
                    cbIOPriority.SelectedIndex = oPriority.LowIOPriority ? 0 : 1;
                }
            }
            doNotSave = false;
        }

        private void CbProcessPriority_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (doNotSave)
                return;

            JobType oType = (JobType)((EnumProxy)cbJobType.SelectedItem).RealValue;
            foreach (WorkerPriority oPriority in MainForm.Instance.Settings.WorkerPriority)
            {
                if (oType == oPriority.JobType)
                {
                    oPriority.Priority = (WorkerPriorityType)cbProcessPriority.SelectedIndex;
                }
            }
        }

        private void CbIOPriority_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (doNotSave)
                return;

            JobType oType = (JobType)((EnumProxy)cbJobType.SelectedItem).RealValue;
            foreach (WorkerPriority oPriority in MainForm.Instance.Settings.WorkerPriority)
            {
                if (oType == oPriority.JobType)
                {
                    oPriority.LowIOPriority = (cbIOPriority.SelectedIndex == 0);
                }
            }
        }

        private void CbAutoStartOnStartup_CheckedChanged(object sender, EventArgs e)
        {
            MainForm.Instance.Settings.WorkerAutoStartOnStartup = cbAutoStartOnStartup.Checked;
        }

        private void CbAutoStart_CheckedChanged(object sender, EventArgs e)
        {
            MainForm.Instance.Settings.WorkerAutoStart = cbAutoStart.Checked;
        }

        private void CbRemoveJob_CheckedChanged(object sender, EventArgs e)
        {
            MainForm.Instance.Settings.WorkerRemoveJob = cbRemoveJob.Checked;
        }
        #endregion

        #region button handlers
        private void configSourceDetector_Click(object sender, EventArgs e)
        {
            SourceDetectorConfigWindow sdcWindow = new SourceDetectorConfigWindow();
            sdcWindow.Settings = sdSettings;
            if (sdcWindow.ShowDialog() == DialogResult.OK)
                sdSettings = sdcWindow.Settings;
        }
  
        private void resetDialogs_Click(object sender, EventArgs e)
        {
            internalSettings.DialogSettings = new DialogSettings();
            MessageBox.Show(this, "Successfully reset all dialogs", "Success", MessageBoxButtons.OK, MessageBoxIcon.None);
        }

        private void runCommand_CheckedChanged(object sender, EventArgs e)
        {
            command.Enabled = runCommand.Checked;
        }
        /// <summary>
        /// launches the autoencode default settings window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void autoEncodeDefaultsButton_Click(object sender, EventArgs e)
        {
            using (AutoEncodeDefaults aed = new AutoEncodeDefaults())
            {
                aed.Settings = this.autoEncodeDefaults;
                DialogResult dr = aed.ShowDialog();
                if (dr == DialogResult.OK)
                {
                    this.autoEncodeDefaults = aed.Settings;
                }
            }
        }

        /// <summary>
        /// Launches configuration of auto-update servers
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void configureServersButton_Click(object sender, EventArgs e)
        {
            using (MeGUI.core.gui.AutoUpdateServerConfigWindow w = new MeGUI.core.gui.AutoUpdateServerConfigWindow())
            {
                w.ServerList = internalSettings.AutoUpdateServerLists;
                w.ServerListIndex = cbAutoUpdateServerSubList.SelectedIndex;
                if (w.ShowDialog() == DialogResult.OK)
                    internalSettings.AutoUpdateServerLists = w.ServerList;
            }
        }

        private void cbHttpProxyMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            var httpProxyMode = (ProxyMode)this.cbHttpProxyMode.SelectedIndex;

            txt_httpproxyaddress.Enabled = httpProxyMode == ProxyMode.CustomProxy || httpProxyMode == ProxyMode.CustomProxyWithLogin;
            txt_httpproxyport.Enabled = httpProxyMode == ProxyMode.CustomProxy || httpProxyMode == ProxyMode.CustomProxyWithLogin;
            txt_httpproxyuid.Enabled = httpProxyMode == ProxyMode.CustomProxyWithLogin;
            txt_httpproxypwd.Enabled = httpProxyMode == ProxyMode.CustomProxyWithLogin;
        }

        private void clearDefaultOutputDir_Click(object sender, EventArgs e)
        {
            defaultOutputDir.Filename = "";
        }

		#endregion

		#region properties
		public MeGUISettings Settings
		{
			get 
			{
                MeGUISettings settings = internalSettings;
                settings.AcceptableFPSError = acceptableFPSError.Value; 
                
                settings.SourceDetectorSettings = sdSettings;
                settings.NeroAacEncPath = neroaacencLocation.Filename;
                settings.FDKAacPath = fdkaacLocation.Filename;
                settings.QAacPath = qaacLocation.Filename;
                settings.VideoExtension = videoExtension.Text;
                settings.AudioExtension = audioExtension.Text;
				settings.DefaultLanguage1 = defaultLanguage1.Text;
				settings.DefaultLanguage2 = defaultLanguage2.Text;
				settings.AutoForceFilm = autoForceFilm.Checked;
                settings.AutoLoadDG = cbAutoLoadDG.Checked;
                settings.EnsureCorrectPlaybackSpeed = chkEnsureCorrectPlaybackSpeed.Checked;
				settings.ForceFilmThreshold = forceFilmPercentage.Value;
                if (donothing.Checked)
                    settings.AfterEncoding = AfterEncoding.DoNothing;
                else if (shutdown.Checked)
                    settings.AfterEncoding = AfterEncoding.Shutdown;
                else if (rbCloseMeGUI.Checked)
                    settings.AfterEncoding = AfterEncoding.CloseMeGUI;
                else
                {
                    settings.AfterEncoding = AfterEncoding.RunCommand;
                    settings.AfterEncodingCommand = command.Text;
                }
                settings.AutoOpenScript = openScript.Checked;
                settings.ThemeMode = (MeGUI.core.gui.ThemeMode)Math.Max(0, cbTheme.SelectedIndex);
				settings.DeleteIntermediateFiles = deleteIntermediateFiles.Checked;
				settings.DeleteAbortedOutput = deleteAbortedOutput.Checked;
				settings.OpenProgressWindow = openProgressWindow.Checked;
				settings.Keep2of3passOutput = keep2ndPassOutput.Checked;
				settings.OverwriteStats = keep2ndPassLogFile.Checked;
				settings.NbPasses = (int)nbPasses.Value;
                settings.AedSettings = this.autoEncodeDefaults;
                settings.AlwaysOnTop = chAlwaysOnTop.Checked;
                settings.DefaultOutputDir = defaultOutputDir.Filename;
                settings.TempDirMP4 = tempDirMP4.Filename;
                settings.AddTimePosition = cbAddTimePos.Checked;
                settings.FFMSThreads = Decimal.ToInt32(ffmsThreads.Value);
                settings.HwdItems = cbHwdDecoder.SelectedIndex;
                settings.EnableHwdAVSVideoDec = chkEnableHwd.Checked;
                settings.AppendToForcedStreams = txtForcedName.Text;
                settings.UseITUValues = cbUseITUValues.Checked;

                // update server settings
                settings.AlwaysBackUpFiles = backupfiles.Checked;
                settings.AutoUpdate = useAutoUpdateCheckbox.Checked;
                MainForm.Instance.UpdateHandler.UpdateMode = settings.UpdateMode;
                settings.HttpProxyMode = (ProxyMode)this.cbHttpProxyMode.SelectedIndex;
                settings.HttpProxyAddress = txt_httpproxyaddress.Text;
                settings.HttpProxyPort = txt_httpproxyport.Text;
                settings.HttpProxyUid = txt_httpproxyuid.Text;
                settings.HttpProxyPwd = txt_httpproxypwd.Text;
                if (cbAutoUpdateServerSubList.SelectedIndex != internalSettings.AutoUpdateServerSubList)
                    settings.LastUpdateCheck = DateTime.Now.AddDays(-7).ToUniversalTime();
                settings.AutoUpdateServerSubList = cbAutoUpdateServerSubList.SelectedIndex;

                if (useDGIndexNV.Checked != internalSettings.UseDGIndexNV)
                    UpdateCacher.CheckPackage("dgindexnv", useDGIndexNV.Checked, false);
                settings.UseDGIndexNV = useDGIndexNV.Checked;

                if (useDGIndexIM.Checked != internalSettings.UseDGIndexIM)
                    UpdateCacher.CheckPackage("dgindexim", useDGIndexIM.Checked, false);
                settings.UseDGIndexIM = useDGIndexIM.Checked;

                if (useNeroAacEnc.Checked != internalSettings.UseNeroAacEnc)
                    UpdateCacher.CheckPackage("neroaacenc", useNeroAacEnc.Checked, false);
                settings.UseNeroAacEnc = useNeroAacEnc.Checked;

                if (useFDKAac.Checked != internalSettings.UseFDKAac)
                    UpdateCacher.CheckPackage("fdkaac", useFDKAac.Checked, false);
                settings.UseFDKAac = useFDKAac.Checked;

                if (useQAAC.Checked != internalSettings.UseQAAC)
                    UpdateCacher.CheckPackage("qaac", useQAAC.Checked, false);
                settings.UseQAAC = useQAAC.Checked;

                settings.Input8Bit = chkInput8Bit.Checked;
                settings.UseExternalMuxerX264 = chx264ExternalMuxer.Checked;
                settings.ChapterCreatorMinimumLength = (int)minimumTitleLength.Value;
                settings.Usex64Tools = chk64Bit.Checked;
                settings.ShowDebugInformation = chkDebugInformation.Checked;
                settings.EnableDirectShowSource = chkDirectShowSource.Checked;

                EnumProxy o = cbStandbySettings.SelectedItem as EnumProxy;
                settings.StandbySetting = (MeGUISettings.StandbySettings)o.RealValue;

                return settings;
			}
			set
			{
                internalSettings = value;
                MeGUISettings settings = value;
                acceptableFPSError.Value = settings.AcceptableFPSError;
                useAutoUpdateCheckbox.Checked = settings.AutoUpdate;
                neroaacencLocation.Filename = settings.NeroAacEncPath;
                fdkaacLocation.Filename = settings.FDKAacPath;
                qaacLocation.Filename = settings.QAacPath;
                sdSettings = settings.SourceDetectorSettings;
                videoExtension.Text = settings.VideoExtension;
                audioExtension.Text = settings.AudioExtension;
                chkEnsureCorrectPlaybackSpeed.Checked = settings.EnsureCorrectPlaybackSpeed;
				int index = this.defaultLanguage1.Items.IndexOf(settings.DefaultLanguage1);
				if (index != -1)
					defaultLanguage1.SelectedIndex = index;
				index = defaultLanguage2.Items.IndexOf(settings.DefaultLanguage2);
				if (index != -1)
					defaultLanguage2.SelectedIndex = index;
				autoForceFilm.Checked = settings.AutoForceFilm;
                cbAutoLoadDG.Checked = settings.AutoLoadDG;
				forceFilmPercentage.Value = settings.ForceFilmThreshold;
                donothing.Checked = settings.AfterEncoding == AfterEncoding.DoNothing;
                shutdown.Checked = settings.AfterEncoding == AfterEncoding.Shutdown;
                runCommand.Checked = settings.AfterEncoding == AfterEncoding.RunCommand;
                rbCloseMeGUI.Checked = settings.AfterEncoding == AfterEncoding.CloseMeGUI;
                command.Text = settings.AfterEncodingCommand;
                openScript.Checked = settings.AutoOpenScript;
                cbTheme.SelectedIndex = (int)settings.ThemeMode;
				deleteIntermediateFiles.Checked = settings.DeleteIntermediateFiles;
				deleteAbortedOutput.Checked = settings.DeleteAbortedOutput;
				openProgressWindow.Checked = settings.OpenProgressWindow;
				keep2ndPassOutput.Checked = settings.Keep2of3passOutput;
				keep2ndPassLogFile.Checked = settings.OverwriteStats;
				nbPasses.Value = (decimal)settings.NbPasses;
                this.autoEncodeDefaults = settings.AedSettings;
                chAlwaysOnTop.Checked = settings.AlwaysOnTop;
                cbHttpProxyMode.SelectedIndex = (int)settings.HttpProxyMode;
                txt_httpproxyaddress.Text = settings.HttpProxyAddress;
                txt_httpproxyport.Text = settings.HttpProxyPort;
                txt_httpproxyuid.Text = settings.HttpProxyUid;
                txt_httpproxypwd.Text = settings.HttpProxyPwd;
                defaultOutputDir.Filename = settings.DefaultOutputDir;
                tempDirMP4.Filename = settings.TempDirMP4;
                cbAddTimePos.Checked = settings.AddTimePosition;
                backupfiles.Checked = settings.AlwaysBackUpFiles;
                cbAutoUpdateServerSubList.SelectedIndex = settings.AutoUpdateServerSubList;
                txtForcedName.Text = settings.AppendToForcedStreams;
                if (ffmsThreads.Maximum < settings.FFMSThreads)
                    ffmsThreads.Value = ffmsThreads.Maximum;
                else
                    ffmsThreads.Value = settings.FFMSThreads;
                cbHwdDecoder.SelectedIndex = (int)settings.HwdItems;
                chkEnableHwd.Checked = settings.EnableHwdAVSVideoDec;
                if (chkEnableHwd.Checked)
                    cbHwdDecoder.Enabled = true;
                else
                    cbHwdDecoder.Enabled = false;
                cbUseITUValues.Checked = settings.UseITUValues;
                useDGIndexNV.Checked = settings.UseDGIndexNV;
                useDGIndexIM.Checked = settings.UseDGIndexIM;
                useNeroAacEnc.Checked = settings.UseNeroAacEnc;
                useFDKAac.Checked = settings.UseFDKAac;
                useQAAC.Checked = settings.UseQAAC;
                chx264ExternalMuxer.Checked = settings.UseExternalMuxerX264;
                minimumTitleLength.Value = settings.ChapterCreatorMinimumLength;
                chk64Bit.Checked = settings.Usex64Tools;
                chkDebugInformation.Checked = settings.ShowDebugInformation;
                chkDirectShowSource.Checked = settings.EnableDirectShowSource;
                chkInput8Bit.Checked = settings.Input8Bit;

                cbStandbySettings.SelectedItem = EnumProxy.Create(MainForm.Instance.Settings.StandbySetting);
            }
		}
		#endregion

        private void backupfiles_CheckedChanged(object sender, EventArgs e)
        {
            if (backupfiles.Checked && !internalSettings.AlwaysBackUpFiles)
            {
                if (MessageBox.Show("If you enable this feature the MeGUI folder size will not be limited as downloaded files and logs are not deleted anymore.", "Backup requested", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.Cancel)
                {
                    backupfiles.Checked = false;
                }
            }

            if (backupfiles.Checked)
                return;

            string meguiToolsFolder = Application.ExecutablePath.Substring(0, Application.ExecutablePath.LastIndexOf('\\'));
            if (Directory.Exists(meguiToolsFolder))
            {
                try
                {  // remove all backup files found
                    Array.ForEach(Directory.GetFiles(meguiToolsFolder, "*.backup", SearchOption.AllDirectories),
                        delegate(string path) { File.Delete(path); });
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnClearMP4TempDirectory_Click(object sender, EventArgs e)
        {
            tempDirMP4.Filename = "";
        }

        private void btnClearOutputDirecoty_Click(object sender, EventArgs e)
        {
            defaultOutputDir.Filename = "";
        }

        private void tempDirMP4_FileSelected(FileBar sender, FileBarEventArgs args)
        {
            if (System.IO.Path.GetPathRoot(tempDirMP4.Filename).Equals(tempDirMP4.Filename, StringComparison.CurrentCultureIgnoreCase))
            {
                // mp4box has in some builds problems if the tmp directory is in the root of a drive
                MessageBox.Show("A root folder cannot be selected!", "Wrong path", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tempDirMP4.Filename = String.Empty;
            }
        }

        private void useNeroAacEnc_CheckedChanged(object sender, EventArgs e)
        {
            neroaacencLocation.Enabled = lblNero.Enabled = useNeroAacEnc.Checked;
            if (useNeroAacEnc.Checked && !internalSettings.UseNeroAacEnc)
                MessageBox.Show("You have to restart MeGUI in order to get access to NeroAacEnc.\r\nAlso the program files are not available on the MeGUI update server. They have to be downloaded from:\r\nhttp://www.nero.com/eng/downloads-nerodigital-nero-aac-codec.php", "Restart required", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void useFDKAac_CheckedChanged(object sender, EventArgs e)
        {
            fdkaacLocation.Enabled = lblFDK.Enabled = useFDKAac.Checked;
            if (useFDKAac.Checked && !internalSettings.UseFDKAac)
                MessageBox.Show("You have to restart MeGUI in order to get access to FDK-AAC.\r\nAlso the program files are not available on the MeGUI update server. They have to be downloaded/compiled manually because of the license. It will not be available with the online updater.", "Restart required", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void useQAAC_CheckedChanged(object sender, EventArgs e)
        {
            qaacLocation.Enabled = lblQaac.Enabled = useQAAC.Checked;
            if (useQAAC.Checked && !internalSettings.UseQAAC)
                MessageBox.Show("You have to restart MeGUI in order to get access to QAAC.\r\nAlso external dependencies must be installed if not already available. More information can be found here:\r\nhttps://sites.google.com/site/qaacpage/", "Restart required", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void useDGIndexNV_CheckedChanged(object sender, EventArgs e)
        {
            if (!useDGIndexNV.Checked)
                return;
        }

        private void useDGIndexIM_CheckedChanged(object sender, EventArgs e)
        {
            if (!useDGIndexIM.Checked)
                return;

            // check if the license file is available
            if (!File.Exists(Path.Combine(Path.GetDirectoryName(MainForm.Instance.Settings.DGIndexIM.Path), "license.txt")))
            {
                if (File.Exists(Path.Combine(Path.GetDirectoryName(MainForm.Instance.Settings.DGIndexNV.Path), "license.txt")))
                {
                    // license.txt available in the other indexer directory. copy it
                    Directory.CreateDirectory(Path.GetDirectoryName(MainForm.Instance.Settings.DGIndexIM.Path));
                    File.Copy(Path.Combine(Path.GetDirectoryName(MainForm.Instance.Settings.DGIndexNV.Path), "license.txt"), Path.Combine(Path.GetDirectoryName(MainForm.Instance.Settings.DGIndexIM.Path), "license.txt"));
                }
                else
                    MessageBox.Show("DGIndexIM cannot be used for free. Therefore you have to copy/create the file license.txt into your ..\\tools\\dgindexnv directory manually.", "license.txt for DGIndexIM missing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void cbAutoUpdateServerSubList_SelectedIndexChanged(object sender, EventArgs e)
        {
            configureServersButton.Enabled = (cbAutoUpdateServerSubList.SelectedIndex == 2);
        }

        private void chkEnableHwd_CheckedChanged(object sender, EventArgs e)
        {
            cbHwdDecoder.Enabled = chkEnableHwd.Checked;
        }

        private void SettingsForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (Form.ModifierKeys == Keys.None && keyData == Keys.Escape)
            {
                this.Close();
                return true;
            }
            return base.ProcessDialogKey(keyData);
        }
    }
}