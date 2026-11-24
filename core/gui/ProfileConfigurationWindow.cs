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
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

using MeGUI.core.plugins.interfaces;
using MeGUI.core.util;

namespace MeGUI.core.gui
{
    public partial class ProfileConfigurationWindow<TSettings, TPanel> : Form
        where TSettings : GenericSettings, new()
        where TPanel : Control, Editable<TSettings>
    {
        private GenericProfile<TSettings> scratchPadProfile
        {
            get
            {
                return byName(ProfileManager.ScratchPadName);
            }
        }

        private TPanel s;
        private bool bSaveSettings = false;
        private bool bSettingsChanged = false;  // will be true if something has been changed
        private GenericProfile<TSettings> oldSelectedPreset;
        private bool bIgnoreChanges = false;

        private TSettings Settings
        {
            get { return s.Settings; }
            set { s.Settings = value; }
        }

        /// <summary>
        /// gets the name of the currently selected profile
        /// </summary>
        public string CurrentProfileName
        {
            get
            {
                return SelectedProfile.Name;
            }
        }

        public ProfileConfigurationWindow(TPanel t, string title)
        {
            InitializeComponent();
            this.Text = title + " configuration dialog";
            this.s = t;
            System.Drawing.Size size = Size;
            size.Height += t.Height - panel1.Height;
            size.Width += Math.Max(t.Width - panel1.Width, 0);
            Size = size;
            t.Dock = DockStyle.Fill;
            panel1.Controls.Add(t);
            this.Icon = MainForm.Instance.Icon;
            this.TopMost = MainForm.Instance.Settings.AlwaysOnTop;
            BuildModernLayout();
        }

        private void loadDefaultsButton_Click(object sender, EventArgs e)
        {
            s.Settings = new TSettings();
            bSettingsChanged = true;
        }

        private void updateButton_Click(object sender, EventArgs e)
        {
            GenericProfile<TSettings> prof = SelectedProfile;
            if (s.Settings.Equals(prof.BaseSettings))
                return;

            prof.Settings = s.Settings;
            bSettingsChanged = true;
        }

        private void newVideoProfileButton_Click(object sender, EventArgs e)
        {
            string profileName = InputBox.Show("Please give the preset a name", "Please give the preset a name", "");
            if (profileName == null)
                return;

            profileName = profileName.Trim();
            if (profileName.Length == 0)
                return;

            GenericProfile<TSettings> prof = new GenericProfile<TSettings>(profileName, s.Settings);
            if (byName(profileName) != null)
                MessageBox.Show("Sorry, presets must have unique names", "Duplicate preset name", MessageBoxButtons.OK);
            else
            {
                videoProfile.Items.Add(prof);
                videoProfile.SelectedItem = prof;
                bSettingsChanged = true;
            }
        }

        public GenericProfile<TSettings> SelectedProfile
        {
            get { return (GenericProfile<TSettings>)videoProfile.SelectedItem; }
            set
            {
                // We can't just set videoProfile.SelectedItem = value, because the profiles in videoProfile are cloned
                foreach (GenericProfile<TSettings> p in videoProfile.Items)
                {
                    if (p.Name == value.Name)
                    {
                        videoProfile.SelectedItem = p;
                        return;
                    }
                }
            }
        }

        public Tuple<IEnumerable<GenericProfile<TSettings>>, GenericProfile<TSettings>> Profiles
        {
            get
            {
                return new Tuple<IEnumerable<GenericProfile<TSettings>>, GenericProfile<TSettings>>(
                    Util.CastAll<GenericProfile<TSettings>>(videoProfile.Items),
                    SelectedProfile);
            }

            set
            {
                videoProfile.Items.Clear();
                foreach (GenericProfile<TSettings> p in value.Item1)
                    videoProfile.Items.Add(p.clone());

                SelectedProfile = value.Item2;
            }
        }

        private GenericProfile<TSettings> byName(string profileName)
        {
            foreach (GenericProfile<TSettings> p in Profiles.Item1)
                if (p.Name == profileName)
                    return p;

            return null;
        }

        private void videoProfile_SelectedIndexChanged(object sender, EventArgs e)
        {
            GenericProfile<TSettings> prof = oldSelectedPreset;
            if (!bIgnoreChanges && prof != null && !s.Settings.Equals(prof.BaseSettings))
            {
                switch (MessageBox.Show("The formerly selected preset has been modified.\nDo you want to save the changes?", "Preset update", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                {
                    case DialogResult.Yes:
                        prof.Settings = s.Settings;
                        bSettingsChanged = true;
                        break;

                    case DialogResult.No:
                        break;
                }
            }

            this.Settings = SelectedProfile.Settings;
            this.oldSelectedPreset = SelectedProfile;

            deleteVideoProfileButton.Enabled = (SelectedProfile.Name != ProfileManager.ScratchPadName ? true : false);
        }

        private void deleteVideoProfileButton_Click(object sender, EventArgs e)
        {
            GenericProfile<TSettings> prof = (GenericProfile<TSettings>)this.videoProfile.SelectedItem;
            Debug.Assert(prof != null);

            if (prof.Name == ProfileManager.ScratchPadName)
                return;

            int iIndex = this.videoProfile.SelectedIndex;
            videoProfile.Items.Remove(prof);
            videoProfile.SelectedIndex = iIndex > 0 ? (iIndex - 1) : 0;
            bSettingsChanged = true;
        }

        private void putSettingsInScratchpad()
        {
            TSettings s = Settings;
            GenericProfile<TSettings> p = scratchPadProfile;

            if (p == null)
            {
                p = new GenericProfile<TSettings>(ProfileManager.ScratchPadName, s);
                videoProfile.Items.Add(p);
            }

            p.Settings = s;
            videoProfile.SelectedItem = p;
        }

        private void ProfileConfigurationWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            bSaveSettings = false;
            if (this.DialogResult == DialogResult.Cancel)
            {
                // cancel selected, do not save
                return;
            }

            Profile prof = SelectedProfile;
            if (Settings.Equals(prof.BaseSettings))
            {
                if (bSettingsChanged)
                    bSaveSettings = true;
                return;
            }

            if (prof.Name == ProfileManager.ScratchPadName)
                prof.BaseSettings = Settings;
            else
            {
                switch (MessageBox.Show("The selected preset has been modified. Update the selected preset?\nPressing \"No\" will save your changes to the scratchpad.",
                    "Preset update", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
                {
                    case DialogResult.Yes:
                        prof.BaseSettings = Settings;
                        break;

                    case DialogResult.No:
                        bIgnoreChanges = true;
                        putSettingsInScratchpad();
                        bIgnoreChanges = false;
                        break;

                    case DialogResult.Cancel:
                        e.Cancel = true;
                        return;
                }
            }
            bSaveSettings = true;
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        public bool SavePresets()
        {
            return bSaveSettings;
        }

        /// <summary>
        /// Windows 11 settings look: the presets are a card on top of the window, the OK / Cancel buttons are in
        /// a footer, the configuration panel keeps the rest of the window. The window is scaled here once
        /// (the configuration panel keeps its own layout), then its automatic scaling is switched off.
        /// </summary>
        private void BuildModernLayout()
        {
            PerformAutoScale();
            AutoScaleMode = AutoScaleMode.None;
            float scale = MainForm.Instance.Settings.DPIRescale(1000) / 1000f;
            Font ui = new Font("Segoe UI", 10F);
            Size panelSize = panel1.Size;
            SuspendLayout();

            // presets
            SettingsSurface top = new SettingsSurface(SettingsSurfaceKind.Window);
            top.Dock = DockStyle.Top;
            top.Font = ui;
            SettingsCard card = new SettingsCard(scale);
            card.Font = ui;
            card.Location = new Point(Scaled(24, scale), Scaled(8, scale));
            card.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            Button[] presetButtons = new Button[] { newVideoProfileButton, deleteVideoProfileButton, updateButton, loadDefaultsButton };
            foreach (Button b in presetButtons)
            {
                b.AutoSize = false;
                b.Font = ui;
                b.Height = Scaled(32, scale);
                b.Width = TextRenderer.MeasureText(b.Text, ui).Width + Scaled(28, scale);
            }
            videoProfile.Font = ui;
            videoProfile.Width = Scaled(200, scale);
            card.AddRow("Preset", null, videoProfile, newVideoProfileButton, deleteVideoProfileButton, updateButton, loadDefaultsButton);
            top.Controls.Add(card);
            top.Height = card.Height + Scaled(16, scale);

            // footer
            SettingsSurface footer = new SettingsSurface(SettingsSurfaceKind.Footer);
            footer.Dock = DockStyle.Bottom;
            footer.Height = Scaled(64, scale);
            footer.Font = ui;
            Button[] footerButtons = new Button[] { okButton, cancelButton };
            foreach (Button b in footerButtons)
            {
                b.Anchor = AnchorStyles.Top | AnchorStyles.Left;
                b.AutoSize = false;
                b.Font = ui;
                b.Size = new Size(Math.Max(Scaled(96, scale), TextRenderer.MeasureText(b.Text, ui).Width + Scaled(36, scale)), Scaled(34, scale));
                footer.Controls.Add(b);
            }
            MethodInvoker layoutFooter = delegate
            {
                int right = footer.Width - Scaled(24, scale);
                for (int i = footerButtons.Length - 1; i >= 0; i--)
                {
                    footerButtons[i].Location = new Point(right - footerButtons[i].Width, (footer.Height - footerButtons[i].Height) / 2);
                    right = footerButtons[i].Left - Scaled(10, scale);
                }
            };
            footer.Resize += delegate { layoutFooter(); };
            top.Resize += delegate { card.Width = Math.Max(Scaled(100, scale), top.Width - Scaled(48, scale)); };

            // the docked control added first is laid out last (it gets the remaining space)
            Controls.Clear();
            panel1.Dock = DockStyle.Fill;
            Controls.Add(panel1);
            Controls.Add(top);
            Controls.Add(footer);

            Rectangle area = Screen.PrimaryScreen.WorkingArea;
            FormBorderStyle = FormBorderStyle.Sizable;
            SizeGripStyle = SizeGripStyle.Show;
            ClientSize = new Size(Math.Min(Math.Max(panelSize.Width, Scaled(780, scale)), area.Width),
                Math.Min(top.Height + panelSize.Height + footer.Height, area.Height - Scaled(60, scale)));
            MinimumSize = Size;
            top.Width = ClientSize.Width;
            card.Width = Math.Max(Scaled(100, scale), top.Width - Scaled(48, scale));
            layoutFooter();

            MethodInvoker theme = delegate
            {
                foreach (Button b in presetButtons)
                    ModernDialog.StyleButton(b, false);
                ModernDialog.StyleButton(okButton, true);
                ModernDialog.StyleButton(cancelButton, false);
                Invalidate(true);
            };
            theme();
            EventHandler themeChanged = delegate
            {
                if (IsHandleCreated && !IsDisposed)
                    BeginInvoke(theme);
            };
            ThemeManager.ThemeChanged += themeChanged;
            Disposed += delegate { ThemeManager.ThemeChanged -= themeChanged; };
            // the theme engine colours the window when it is shown: restyle after it
            Shown += delegate { BeginInvoke(theme); };
            ResumeLayout(true);
        }

        private static int Scaled(int value, float scale)
        {
            return (int)Math.Round(value * scale);
        }
    }
}