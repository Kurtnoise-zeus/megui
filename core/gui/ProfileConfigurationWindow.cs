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
using System.Diagnostics;
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
    }
}