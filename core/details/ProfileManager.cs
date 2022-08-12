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
using System.IO;
using System.Reflection;
using System.Xml.Serialization;
using System.Text;
using System.Windows.Forms;

using MeGUI.core.gui;
using MeGUI.core.plugins.interfaces;
using MeGUI.core.util;
using MeGUI.packages.tools.oneclick;
using MeGUI.packages.video.x264;
using MeGUI.packages.video.x265;
using MeGUI.packages.video.xvid;

namespace MeGUI
{
    public interface Editable<TSettings>
    {
        TSettings Settings
        {
            get;
            set;
        }
    }

    public class ProfileManager
    {
        public static readonly string ScratchPadName = "*scratchpad*";
        private string profileFolder;
        private List<ProfileType> profileTypes = new List<ProfileType>();
        private List<ProfileGroup> profileGroups = new List<ProfileGroup>();

        #region init
        public ProfileManager(string profileFolder)
        {
            if (Directory.Exists(Path.Combine(profileFolder, "allprofiles")))
                this.profileFolder = Path.Combine(profileFolder, "allprofiles");
            else
                this.profileFolder = profileFolder;
            profileGroups.Add(new ProfileGroup(typeof(VideoCodecSettings), "Video"));
            SafeRegister<x264Settings, x264ConfigurationPanel>("Video");
            SafeRegister<x265Settings, x265ConfigurationPanel>("Video");
            SafeRegister<xvidSettings, xvidConfigurationPanel>("Video");
            profileGroups.Add(new ProfileGroup(typeof(AudioCodecSettings), "Audio"));
            SafeRegister<AC3Settings, MeGUI.packages.audio.ffac3.AC3ConfigurationPanel>("Audio");
            SafeRegister<MP2Settings, MeGUI.packages.audio.ffmp2.MP2ConfigurationPanel>("Audio");
            SafeRegister<MP3Settings, MeGUI.packages.audio.lame.lameConfigurationPanel>("Audio");
            SafeRegister<NeroAACSettings, MeGUI.packages.audio.naac.neroConfigurationPanel>("Audio");
            SafeRegister<OggVorbisSettings, MeGUI.packages.audio.vorbis.OggVorbisConfigurationPanel>("Audio");
            SafeRegister<FlacSettings, MeGUI.packages.audio.flac.FlacConfigurationPanel>("Audio");
            SafeRegister<QaacSettings, MeGUI.packages.audio.qaac.qaacConfigurationPanel>("Audio");
            SafeRegister<OpusSettings, MeGUI.packages.audio.opus.OpusConfigurationPanel>("Audio");
            SafeRegister<FDKAACSettings, MeGUI.packages.audio.fdkaac.FDKAACConfigurationPanel>("Audio");
            SafeRegister<FFAACSettings, MeGUI.packages.audio.ffaac.FFAACConfigurationPanel>("Audio");

            SafeRegister<OneClickSettings, OneClickConfigPanel>();
            SafeRegister<AviSynthSettings, AviSynthProfileConfigPanel>();
        }
        #endregion

        #region registration 
        public bool SafeRegister<TSettings, TPanel>(params string[] groups)
            where TSettings : GenericSettings, new()
            where TPanel : Control, Editable<TSettings>, new()
        {
            string name = new TSettings().SettingsID;
            if (byName(name) != null) return false;
            if (bySettingsType(typeof(TSettings)) != null) return false;

            ProfileType p = new SpecificProfileType<TSettings, TPanel>(name);
            profileTypes.Add(p);

            foreach (string g in groups)
                groupByName(g).Register(p, name, typeof(TSettings));
            return true;
        }

        public bool Register(Type TSettings, Type TPanel, string name)
        {
            MethodInfo m = typeof(ProfileManager).GetMethod("SafeRegister");
            MethodInfo m2 = m.MakeGenericMethod(TSettings, TPanel);
            return (bool)m2.Invoke(this, new object[] { name });
        }
        #endregion

        #region private util
        /// <summary>
        /// eliminates non allowed characters and replaced them with an underscore
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private static string fixName(string name)
        {
            name = name.Replace("\"", "_hc_");
            name = name.Replace("*", "_st_");
            name = name.Replace("/", "_sl_");
            name = name.Replace(":", "_dp_");
            name = name.Replace("<", "_lt_");
            name = name.Replace(">", "_rt_");
            name = name.Replace("?", "_qm_");
            name = name.Replace("\\", "_bs_");
            name = name.Replace("|", "_vl_");
            return name;
        }

        private ProfileGroup groupByName(string name)
        {
            return Util.ByID(profileGroups, name);
        }
        
        private ProfileType byName(string name)
        {
            return Util.ByID(profileTypes, name);
        }

        private ProfileType bySettingsType(Type t)
        {
            return profileTypes.Find(delegate(ProfileType p) { return p.SettingsType == t; });
        }

        private Type profileType(string id)
        {
            return typeof(GenericProfile<>).MakeGenericType(byName(id).SettingsType);
        }
        #endregion

        #region loading and saving
        private static string profilePath(string path, Profile prof)
        {
            return Path.Combine(Path.Combine(path, prof.BaseSettings.SettingsID), fixName(prof.FQName) + ".xml");
        }

        private static string GetDefaultProfilPath()
        {
            return Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "allprofiles");
        }

        public bool SaveProfiles()
        {
            bool bOK = WriteProfiles(null, AllProfiles);
            if (!saveSelectedProfiles())
                bOK = false;
            return bOK;
        }

        /// <summary>
        /// saves all the profiles
        /// this is called when the program exists and ensures that all
        /// currently defined profiles are saved, overwriting currently existing ones
        /// </summary>
        public static bool WriteProfiles(string savePath, IEnumerable<Profile> profiles)
        {
            if (string.IsNullOrEmpty(savePath))
                savePath = GetDefaultProfilPath();

            // remove old backup files if available
            if (!deleteFiles(savePath, "*.backup"))
                return false;

            // backup profiles
            try
            {
                DirectoryInfo fi = new DirectoryInfo(savePath);
                FileInfo[] files = fi.GetFiles("*.xml", SearchOption.AllDirectories);
                foreach (FileInfo f in files)
                {
                    f.CopyTo(Path.Combine(f.Directory.FullName, Path.ChangeExtension(f.Name, ".backup")));
                }
            }
            catch (Exception ex)
            {
                LogItem _oLog = MainForm.Instance.Log.Info("Error");
                _oLog.LogValue("Backup profile files could not be created", ex, ImageType.Error);

                // remove backup files
                deleteFiles(savePath, "*.backup");
                return false;
            }

            // remove profile files
            if (!deleteFiles(savePath, "*.xml"))
            {
                // restore backup
                try
                {
                    DirectoryInfo fi = new DirectoryInfo(savePath);
                    FileInfo[] files = fi.GetFiles("*.backup", SearchOption.AllDirectories);
                    foreach (FileInfo f in files)
                    {
                        f.CopyTo(Path.Combine(f.Directory.FullName, Path.ChangeExtension(f.Name, ".xml")));
                    }
                }
                catch (Exception e)
                {
                    LogItem _oLog = MainForm.Instance.Log.Info("Error");
                    _oLog.LogValue("Profile files could not be restored", e, ImageType.Error);
                }
                return false;
            }

            bool bSuccess = true;
            try
            {
                foreach (Profile p in profiles)
                {
                    if (!Util.XmlSerialize(p, profilePath(savePath, p)))
                    {
                        string backupFile = Path.ChangeExtension(profilePath(savePath, p), ".backup");
                        if (File.Exists(backupFile))
                            File.Copy(backupFile, profilePath(savePath, p), true);
                        bSuccess = false;
                    }
                }

                deleteFiles(savePath, "*.backup");
            }
            catch (Exception ex)
            {
                LogItem _oLog = MainForm.Instance.Log.Info("Error");
                _oLog.LogValue("Profile files could not be created", ex, ImageType.Error);
                bSuccess = false;
            }

            return bSuccess;
        }

        private static bool deleteFiles(string path, string files)
        {
            try
            {
                if (!Directory.Exists(path))
                    return true;

                Array.ForEach(Directory.GetFiles(path, files, SearchOption.AllDirectories), delegate (string filePath) { File.Delete(filePath); });
                return true;
            }
            catch (Exception ex)
            {
                LogItem _oLog = MainForm.Instance.Log.Info("Error");
                _oLog.LogValue("Files could not be deleted", ex, ImageType.Error);
                return false;
            }
        }

        private bool saveSelectedProfiles()
        {
            string filename = Path.Combine(GetDefaultProfilPath(), "SelectedProfiles.xml");
            XmlSerializer ser = null;
            bool bOK = true;
            try
            {
                using (Stream s = File.Open(filename, System.IO.FileMode.Create, System.IO.FileAccess.Write))
                {
                    // Hacky workaround because serialization of dictionaries isn't possible
                    string[] groupKeys = profileGroups.ConvertAll<string>(delegate (ProfileGroup g) { return g.ID; }).ToArray();
                    string[] groupValues = profileGroups.ConvertAll<string>(delegate (ProfileGroup g) { return g.SelectedChild; }).ToArray();
                    string[] keys = profileTypes.ConvertAll<string>(delegate (ProfileType p) { return p.ID; }).ToArray();
                    string[] values = profileTypes.ConvertAll<string>(delegate (ProfileType p) { return p.SelectedProfile.Name; }).ToArray();
                    string[][] data = new string[][] { keys, values, groupKeys, groupValues };
                    ser = new XmlSerializer(data.GetType());
                    ser.Serialize(s, data);
                }
            }
            catch (Exception e)
            {
                LogItem _oLog = MainForm.Instance.Log.Info("Error");
                _oLog.LogValue("List of selected profiles could not be saved", e, ImageType.Error);
                bOK = false;
            }
            return bOK;
        }

        private void loadSelectedProfiles()
        {
            string file = Path.Combine(GetDefaultProfilPath(), "SelectedProfiles.xml");
            if (!File.Exists(file)) 
                return;

            deleteDeprecatedEntries(file);

            using (Stream s = File.OpenRead(file))
            {
                try
                {
                    // Hacky workaround because serialization of dictionaries isn't possible
                    XmlSerializer ser = new XmlSerializer(typeof(string[][]));
                    string[][] data = (string[][])ser.Deserialize(s);
                    string[] keys = data[0];
                    string[] values = data[1];
                    string[] groupKeys = data[2];
                    string[] groupValues = data[3];
                    System.Diagnostics.Debug.Assert(keys.Length == values.Length);
                    Debug.Assert(groupKeys.Length == groupValues.Length);
                    for (int i = 0; i < keys.Length; i++)
                    {
                        byName(keys[i]).SelectedProfile = byName(keys[i]).ByName(values[i]);
                    }

                    for (int i = 0; i < groupKeys.Length; ++i)
                    {
                        groupByName(groupKeys[i]).SelectedChild = groupValues[i];
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("List of selected profiles could not be loaded.", "Error loading profile", MessageBoxButtons.OK);
                }
            }
        }

        private void deleteDeprecatedEntries(String strFile)
        {
            String line;
            StringBuilder sbNewFile = new StringBuilder();
            int counter = 0, iBlock = 0;
            List<int> arrDeprecated = new List<int>();
            StreamReader file = new StreamReader(strFile);
            bool bFound = false;
            
            while ((line = file.ReadLine()) != null)
            {
                if (line.Equals("  <ArrayOfString>"))
                {
                    iBlock++;
                    counter = 0;
                }
                counter++;
                if (iBlock == 1 && (line.Equals("    <string>Aften AC-3</string>") || line.Equals("    <string>Snow</string>") || line.Equals("    <string>Aud-X MP3</string>") || line.Equals("    <string>Winamp AAC</string>") || line.Equals("    <string>FAAC</string>")))
                {
                    bFound = true;
                    arrDeprecated.Add(counter);
                }
                else if (iBlock == 4 && (line.Equals("    <string>Aften AC-3</string>") || line.Equals("    <string>Snow</string>") || line.Equals("    <string>Aud-X MP3</string>") || line.Equals("    <string>Winamp AAC</string>") || line.Equals("    <string>FAAC</string>")))
                {
                    bFound = true;
                    if (line.Contains("Snow"))
                        sbNewFile.AppendLine("    <string>x264</string>");
                    else
                        sbNewFile.AppendLine("    <string>FFmpeg AC-3</string>");
                }
                else if (iBlock == 2)
                {
                    bool bDelete = false;
                    foreach (int i in arrDeprecated)
                    {
                        if (i == counter)
                            bDelete = true;
                    }
                    if (!bDelete)
                        sbNewFile.AppendLine(line);
                }
                else
                    sbNewFile.AppendLine(line);
            }
            file.Close();

            if (bFound)
            {
                StreamWriter newfile = new StreamWriter(strFile);
                newfile.Write(sbNewFile);
                newfile.Close();
            }
        }

        public void LoadProfiles()
        {
            foreach (ProfileType t in profileTypes)
                t.Profiles = readAllProfiles(t, false);

            loadSelectedProfiles();
        }

        public static List<Profile> ReadAllProfiles(string path, bool bSilentError)
        {
            ProfileManager p = new ProfileManager(path);
            List<Profile> ps = new List<Profile>();

            foreach (ProfileType t in p.profileTypes)
                ps.AddRange(p.readAllProfiles(t, bSilentError));

            return ps;
        }

        private List<Profile> readAllProfiles(ProfileType type, bool bSilentError)
        {
            string profilePath = Path.Combine(this.profileFolder, type.ID);
            DirectoryInfo di = FileUtil.ensureDirectoryExists(profilePath);
            FileInfo[] files = di.GetFiles("*.xml");

            return Util.RemoveDuds(new List<FileInfo>(files).ConvertAll<Profile>(
                delegate(FileInfo fi)
                {
                    return (Profile)Util.XmlDeserialize(fi.FullName, type.GenericProfileType, bSilentError);
                }));
        }

        private void setAllProfiles(List<Profile> ps)
        {
            ps = Util.RemoveDuds(ps);
            foreach (ProfileType type in profileTypes)
            {
                type.Profiles = ps.FindAll(
                    delegate(Profile p) { return p.GetType().Equals(type.GenericProfileType); });
            }
        }
        #endregion

        #region selected profiles
        public Profile GetSelectedProfile(string profileType)
        {
            ProfileType t = byName(profileType);
            if (t != null)
                return t.SelectedProfile;

            ProfileGroup g = groupByName(profileType);
            Debug.Assert(g != null);

            return GetSelectedProfile(g.SelectedChild);
        }

        public void SetSelectedProfile(Profile prof)
        {
            bySettingsType(GetSettingsType(prof)).SelectedProfile = prof;
        }
        #endregion

        public Profile GetProfile(string type, string name)
        {
            return byName(type).ByName(name);
        }

        #region profileset operations
        private List<ProfileType> byProfileSet(string profileSet)
        {
            List<ProfileType> res = new List<ProfileType>();
            ProfileType p = byName(profileSet);
            if (p != null)
            {
                res.Add(p);
                return res;
            }
            
            ProfileGroup g = groupByName(profileSet);
            if (g == null)
                throw new Exception();

            foreach (string s in g.ChildIDs)
                res.AddRange(byProfileSet(s));

            return res;
        }

        public IEnumerable<Named<Profile>> Profiles(string type) 
        {
            List<ProfileType> ps = byProfileSet(type);
            
            if (ps.Count == 1)
                return Util.ConvertAll<Profile, Named<Profile>>(ps[0].Profiles, delegate (Profile pr) { return new Named<Profile>(pr.Name, pr); });

            List<Named<Profile>> profiles = new List<Named<Profile>>();
            foreach (ProfileType p in ps)
                profiles.AddRange(Util.ConvertAll<Profile, Named<Profile>>(p.Profiles, delegate (Profile prof) { return new Named<Profile>(prof.FQName, prof); }));

            return profiles;
        }

        public void AddProfilesChangedListener(string profileSet, EventHandler listener)
        {
            foreach (ProfileType p in byProfileSet(profileSet))
                p.ProfilesChanged += listener;
        }

        public void RemoveProfilesChangedListener(string profileSet, EventHandler listener)
        {
            foreach (ProfileType p in byProfileSet(profileSet))
                p.ProfilesChanged -= listener;
        }
        #endregion

        #region Profile Listing
        private Profile byFormattedName(string formattedName)
        {
            string type = formattedName.Substring(0, formattedName.IndexOf(':'));
            System.Diagnostics.Debug.Assert(formattedName.StartsWith(type + ": "));
            string profileName = formattedName.Substring(type.Length + 2);
            return GetProfile(type, profileName);
        }

        public Profile[] AllProfiles
        {
            get
            {
                List<Profile> profileList = new List<Profile>();
                
                foreach (ProfileType ty in profileTypes)
                    profileList.AddRange(ty.Profiles);
                
                return profileList.ToArray();
            }
        }

        public void AddAll(Profile[] profiles, DialogManager asker)
        {
            foreach (Profile prof in profiles)
            {
                bySettingsType(GetSettingsType(prof)).Add(prof, asker);
            }
        }
        #endregion

        #region misc operations
        private static Type GetSettingsType(Profile p)
        {
            return p.GetType().GetGenericArguments()[0];
        }

        public void Configure(Profile SelectedProfile, bool UpdateSelectedProfile, SimpleProfilesControl instance)
        {
            bySettingsType(GetSettingsType(SelectedProfile)).ConfigureProfiles(SelectedProfile, UpdateSelectedProfile, instance);
        }

        public void SetSettings(GenericSettings s)
        {
            bySettingsType(s.GetType()).SetSettings(s);
        }

        public GenericSettings GetCurrentSettings(string p)
        {
            return GetSelectedProfile(p).BaseSettings;
        }
        #endregion
    }

    abstract class ProfileType : IIDable
    {
        public event EventHandler SelectedProfileSet;
        public event EventHandler ProfilesChanged;

        private readonly string profileTypeID;

        public string ID { get { return profileTypeID; } }

        public Tuple<IEnumerable<Profile>, Profile> ProfilesAndSelected
        {
            get { return new Tuple<IEnumerable<Profile>, Profile>(profiles, selectedProfile); }
            set
            {
                profiles = new List<Profile>(value.Item1);
                if (profiles.Count == 0)
                    profiles.Add(genScratchpad());

                selectedProfile = value.Item2 ?? profiles[0];
            }
        }

        public IEnumerable<Profile> Profiles
        {
            get { return ProfilesAndSelected.Item1; }
            set { ProfilesAndSelected = new Tuple<IEnumerable<Profile>,Profile>(value, null); }
        }

        protected void raiseChangedEvent(string selectedProfile, SimpleProfilesControl Instance)
        {
            if (ProfilesChanged != null)
                ProfilesChanged(this, new SelectedProfileEventArgs(selectedProfile, Instance));
        }

        protected List<Profile> profiles = new List<Profile>();

        protected abstract Profile genScratchpad();

        private Profile selectedProfile;
        public Profile SelectedProfile
        {
            get { return selectedProfile; }
            set
            {
                selectedProfile = value ?? selectedProfile;
                if (SelectedProfileSet != null)
                    SelectedProfileSet(this, EventArgs.Empty);
            }
        }

        public void SetSettings(GenericSettings s)
        {
            if (s.GetType() != SettingsType)
                throw new Exception("Wrong type of settings used");

            Profile p = ByName(ProfileManager.ScratchPadName);
            if (p == null)
            {
                p = genScratchpad();
                profiles.Add(p);
            }
            p.BaseSettings = s;
            SelectedProfile = p;
        }

        public abstract void ConfigureProfiles(Profile SelectedProfile, bool UpdateSelectedProfile, SimpleProfilesControl Instance);

        public abstract Type SettingsType { get; }
        
        public Type GenericProfileType
        {
            get { return typeof(GenericProfile<>).MakeGenericType(SettingsType); }
        }

        public ProfileType(string name)
        {
            profileTypeID = name;
        }

        public Profile ByName(string name)
        {
            int i = indexOf(name);
            if (i >= 0)
                return profiles[i];
            else
                return null;
        }

        private int indexOf(string name)
        {
            return profiles.FindIndex(delegate(Profile p) { return p.Name == name; });
        }

        public void Add(Profile prof, DialogManager asker)
        {
            int i = indexOf(prof.Name);
            if (i < 0)
            {
                profiles.Add(prof);
                raiseChangedEvent(null, null);
            }
            else if (asker.overwriteProfile(prof.FQName))
                profiles[i] = prof;
            else { /* skip this profile */ }
        }
    }

    abstract class SpecificProfileType<TSettings> : ProfileType
        where TSettings : GenericSettings, new()
    {
        public SpecificProfileType(string name) : base(name) { }

        public override Type SettingsType
        {
            get { return typeof(TSettings); }
        }

        public GenericProfile<TSettings> SByName(string name)
        {
            return (GenericProfile<TSettings>)ByName(name);
        }

        public Tuple<IEnumerable<GenericProfile<TSettings>>, GenericProfile<TSettings>> SProfiles
        {
            get
            {
                Tuple<IEnumerable<Profile>, Profile> p = ProfilesAndSelected;

                return new Tuple<IEnumerable<GenericProfile<TSettings>>, GenericProfile<TSettings>>(
                    Util.CastAll<Profile, GenericProfile<TSettings>>(p.Item1),
                    (GenericProfile<TSettings>)p.Item2);
            }
            set
            {
                ProfilesAndSelected = new Tuple<IEnumerable<Profile>, Profile>(
                    Util.CastAll<GenericProfile<TSettings>, Profile>(value.Item1),
                    value.Item2);
            }
        }

        protected override Profile genScratchpad()
        {
            return new GenericProfile<TSettings>(ProfileManager.ScratchPadName, new TSettings());
        }
    }

    class SpecificProfileType<TSettings, TPanel> : SpecificProfileType<TSettings>
        where TSettings : GenericSettings, new()
        where TPanel : Control, Editable<TSettings>, new()
    {
        public override void ConfigureProfiles(Profile SelectedProfile, bool UpdateSelectedProfile, SimpleProfilesControl Instance)
        {
            // used to init with mainForm, but that is not required any more. OTOH, a ProfileManager instance might be required
            TPanel t = new TPanel();
            ProfileConfigurationWindow<TSettings, TPanel> w = new ProfileConfigurationWindow<TSettings, TPanel>(t, ID);

            Tuple<IEnumerable<GenericProfile<TSettings>>, GenericProfile<TSettings>> profiles = 
                new Tuple<IEnumerable<GenericProfile<TSettings>>, GenericProfile<TSettings>>(
                Util.CastAll<Profile, GenericProfile<TSettings>>(ProfilesAndSelected.Item1),
                (GenericProfile<TSettings>)SelectedProfile);

            w.Profiles = profiles;
            if (w.ShowDialog() == DialogResult.Cancel)
                return;

            bool bRaiseEvent = false;

            // set if possible the selected profile
            foreach (GenericProfile<TSettings> p in SProfiles.Item1)
            {
                if (p.FQName.Equals(w.SelectedProfile.FQName))
                {
                    // profile exist - set it
                    if (UpdateSelectedProfile)
                        SelectedProfile = w.SelectedProfile;
                    bRaiseEvent = true;
                    break;
                }
            }

            if (w.SavePresets())
            {
                SProfiles = w.Profiles;
                MainForm.Instance.Profiles.SaveProfiles();
                bRaiseEvent = true;
            }

            if (bRaiseEvent)
                raiseChangedEvent(w.SelectedProfile.FQName, Instance);
        }

        public SpecificProfileType(string name)
            : base(name)
        { }
    }

    class ProfileGroup : IIDable
    {
        private readonly string profileGroupID;

        public string ID { get { return profileGroupID; } }

        public List<string> ChildIDs = new List<string>();
        public readonly Type CommonType;
        public string SelectedChild = null;

        public ProfileGroup(Type t, string name)
        {
            Debug.Assert(Array.IndexOf(t.GetInterfaces(), typeof(GenericSettings)) >= 0);
            //Debug.Assert(t.IsSubclassOf(typeof(GenericSettings)));
            CommonType = t;
            profileGroupID = name;
        }

        public void Register(ProfileType t, string childID, Type childType)
        {
            Debug.Assert(childType.IsSubclassOf(CommonType));

            if (!ChildIDs.Contains(childID))
                ChildIDs.Add(childID);

            if (SelectedChild == null)
                SelectedChild = childID;

            t.SelectedProfileSet += delegate(object _, EventArgs __)
            {
                SelectedChild = childID;
            };
        }
    }
}