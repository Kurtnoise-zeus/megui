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
using System.ComponentModel;
using System.Windows.Forms;

using MeGUI.core.util;

namespace MeGUI.core.gui
{
    public partial class SimpleProfilesControl : UserControl
    {
        public SimpleProfilesControl()
        {
            InitializeComponent();
        }

        public void SetSettings(AudioCodecSettings value)
        {
            manager.SetSettings(value);
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Profile SelectedProfile
        {
            get { return ((Named<Profile>)comboBox1.SelectedItem).Data; }
        }

        public void SelectProfile(Profile prof)
        {
            SelectProfile(prof.FQName);
        }

        public void SelectProfile(string fqname)
        {
            foreach (Named<Profile> n in comboBox1.Items)
            {
                if (n.Data.FQName == fqname)
                {
                    comboBox1.SelectedItem = n;
                    return;
                }
            }
            RefreshProfiles(null);
            foreach (Named<Profile> n in comboBox1.Items)
            {
                if (n.Data.FQName == fqname)
                {
                    comboBox1.SelectedItem = n;
                    return;
                }
            }
            if (comboBox1.Items.Count > 0)
            {
                comboBox1.SelectedIndex = 0;
                MessageBox.Show("The profile \"" + fqname + "\" could not be selected.\r\nSelecting profile \"" + comboBox1.SelectedItem.ToString() + "\" instead.", "Profile couldn't be selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        public SimpleProfilesControl GetInstance
        {
            get { return this; }
        }

        public void SetProfileNameOrWarn(string fqname)
        {
            if (string.IsNullOrEmpty(fqname))
                return;
            try
            {
                SelectProfile(fqname);
            }
            catch (ProfileCouldntBeSelectedException)
            {
                MessageBox.Show("The profile, " + fqname + ", could not be selected.", "Profile couldn't be selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private ProfileManager manager = new ProfileManager("");

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ProfileManager Manager
        {
            get { return manager; }
            set {
                if (!string.IsNullOrEmpty(ProfileSet))
                {
                    manager.RemoveProfilesChangedListener(ProfileSet, ProfilesChanged);
                    value.AddProfilesChangedListener(ProfileSet, ProfilesChanged);
                }
                manager = value; 
                RefreshProfiles(null); 
            }
        }

        private void ProfilesChanged(object _, EventArgs args)
        {
            if (args is SelectedProfileEventArgs)
            {
                if (this == ((SelectedProfileEventArgs)args).Instance)
                    RefreshProfiles(((SelectedProfileEventArgs)args).SelectedProfile);
            }
            else
                RefreshProfiles(null);
        }

        private string profileSet;
        /// <summary>
        /// The string describing the profile set to request from the profile manager
        /// </summary>
        public string ProfileSet
        {
            get { return profileSet; }
            set {
                if (!string.IsNullOrEmpty(profileSet))
                    Manager.RemoveProfilesChangedListener(profileSet, ProfilesChanged);
                if (!string.IsNullOrEmpty(value))
                    Manager.AddProfilesChangedListener(value, ProfilesChanged);
                
                profileSet = value; 
            }
        }

        protected void RefreshProfiles(string SelectedProfile)
        {
            comboBox1.Items.Clear();
            foreach (Named<Profile> oProfile in Manager.Profiles(ProfileSet))
            {
                if ((oProfile.Data.BaseSettings is NeroAACSettings) && !MainForm.Instance.Settings.UseNeroAacEnc)
                    continue;

                if ((oProfile.Data.BaseSettings is FDKAACSettings) && !MainForm.Instance.Settings.UseFDKAac)
                    continue;

                if ((oProfile.Data.BaseSettings is QaacSettings) && !MainForm.Instance.Settings.UseQAAC)
                    continue;

                comboBox1.Items.Add(oProfile);
            }
            if (String.IsNullOrEmpty(SelectedProfile))
                SelectProfile(Manager.GetSelectedProfile(ProfileSet));
            else
                SelectProfile(SelectedProfile);
        }

        protected void raiseProfileChangedEvent()
        {
            if (SelectedProfileChanged != null)
                SelectedProfileChanged(this, EventArgs.Empty);
        }

        private bool bUpdateSelectedProfile = true;
        /// <summary>
        /// The string describing the profile set to request from the profile manager
        /// </summary>
        public bool UpdateSelectedProfile
        {
            get { return bUpdateSelectedProfile; }
            set { bUpdateSelectedProfile = value; }
        }

        public event EventHandler SelectedProfileChanged;
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (bUpdateSelectedProfile)
                Manager.SetSelectedProfile(SelectedProfile);
            raiseProfileChangedEvent();
        }
    }

    public class ProfileCouldntBeSelectedException : MeGUIException
    {
        private string name;
        public string ProfileName
        {
            get { return name; }
        }

        public ProfileCouldntBeSelectedException(string name)
            : base("The profile '" + name + "' couldn't be selected.")
        {
            this.name = name;
        }
    }

    public class SelectedProfileEventArgs : EventArgs
    {
        private readonly string selectedProfile;
        private readonly SimpleProfilesControl instance;

        public SelectedProfileEventArgs(string selectedProfile, SimpleProfilesControl instance)
        {
            this.selectedProfile = selectedProfile;
            this.instance = instance;
        }

        public string SelectedProfile
        {
            get { return this.selectedProfile; }
        }

        public SimpleProfilesControl Instance
        {
            get { return this.instance; }
        }
    }
}