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
using System.Collections;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

using MeGUI.core.util;

namespace MeGUI
{
    public partial class UpdateWindow : Form
    {
        #region Variables
        private ListViewColumnSorter lvwColumnSorter;
        private bool bUpdateAllowed;
        private enum PackageStatus
        {
            [EnumTitle("no update available")]
            NoUpdateAvailable,
            [EnumTitle("update available")]
            UpdateAvailable,
            [EnumTitle("update ignored")]
            UpdateIgnored,
            [EnumTitle("reinstalling")]
            Reinstall,
            [EnumTitle("package disabled")]
            Disabled
        };
        public enum ErrorState
        {
            [EnumTitle("File cannot be found on the server")]
            FileNotOnServer,
            [EnumTitle("Update server is not available")]
            ServerNotAvailable,
            [EnumTitle("File cannot be downloaded")]
            CouldNotDownloadFile,
            [EnumTitle("Backup file cannot be removed")]
            CouldNotRemoveBackup,
            [EnumTitle("File cannot be saved")]
            CouldNotSaveNewFile,
            [EnumTitle("Backup file cannot be created")]
            CouldNotCreateBackup,
            [EnumTitle("File cannot be installed")]
            CouldNotInstall,
            [EnumTitle("Update successful")]
            Successful,
            [EnumTitle("File cannot be extracted")]
            CouldNotExtract,
            [EnumTitle("Update XML is invalid")]
            InvalidXML,
            [EnumTitle("The requirements for this file are not met")]
            RequirementNotMet
        }
        #endregion
        #region Classes

        public abstract class iUpgradeable
        {
            /// <summary>
            /// May be overridden in a special init is to be done after the xmlserializer
            /// </summary>
            public virtual void init() { }
            
            internal iUpgradeable()
            {
                availableVersion = new Version();

            } // constructor

            // Overrideable methods
            public bool HasAvailableVersion
            {
                get
                {
                    Version latest = this.availableVersion;
                    if (this.name == "neroaacenc")
                    {
                        if (currentVersion != null && currentVersion.FileVersion != null && currentVersion.FileVersion.Equals(latest.FileVersion))
                            latest.UploadDate = currentVersion.UploadDate;
                    }
                    else if (this.name == "fdkaac")
                    {
                        if (MainForm.Instance.Settings.UseFDKAac && File.Exists(MainForm.Instance.Settings.Fdkaac.Path))
                            latest.UploadDate = currentVersion.UploadDate;
                    }
                    return latest != null && (latest.CompareTo(currentVersion) != 0);
                }
            }

            public bool isAvailable()
            {
                ArrayList arrPath = new ArrayList();
                switch (this.name)
                {
                    case "neroaacenc":
                        if (MainForm.Instance.Settings.UseNeroAacEnc)
                        {
                            if (MainForm.Instance.Settings.NeroAacEnc.UpdateAllowed())
                                arrPath.AddRange(MainForm.Instance.Settings.NeroAacEnc.Files);
                            if (File.Exists(MainForm.Instance.Settings.NeroAacEnc.Path))
                            {
                                System.Diagnostics.FileVersionInfo finfo = System.Diagnostics.FileVersionInfo.GetVersionInfo(MainForm.Instance.Settings.NeroAacEnc.Path);
                                FileInfo fi = new FileInfo(MainForm.Instance.Settings.NeroAacEnc.Path);
                                CurrentVersion.FileVersion = finfo.FileMajorPart + "." + finfo.FileMinorPart + "." + finfo.FileBuildPart + "." + finfo.FilePrivatePart;
                                CurrentVersion.UploadDate = fi.LastWriteTimeUtc;
                            }
                        }
                        break;
                    case "fdkaac":
                        if (MainForm.Instance.Settings.UseFDKAac)
                        {
                            if (MainForm.Instance.Settings.Fdkaac.UpdateAllowed())
                                arrPath.AddRange(MainForm.Instance.Settings.Fdkaac.Files);
                            if (File.Exists(MainForm.Instance.Settings.Fdkaac.Path))
                            {
                                System.Diagnostics.FileVersionInfo finfo = System.Diagnostics.FileVersionInfo.GetVersionInfo(MainForm.Instance.Settings.Fdkaac.Path);
                                FileInfo fi = new FileInfo(MainForm.Instance.Settings.Fdkaac.Path);
                                CurrentVersion.FileVersion = finfo.FileMajorPart + "." + finfo.FileMinorPart + "." + finfo.FileBuildPart + "." + finfo.FilePrivatePart;
                                CurrentVersion.UploadDate = fi.LastWriteTimeUtc;
                            }
                        }
                        break;
                    default:
                        ProgramSettings pSettings = UpdateCacher.GetPackage(this.name);
                        if (pSettings != null && pSettings.UpdateAllowed())
                            arrPath.AddRange(pSettings.Files);
                        break;
                }

                foreach (string strAppPath in arrPath)
                {
                    if (String.IsNullOrEmpty(strAppPath))
                        return false;
                    if (File.Exists(strAppPath) == false)
                        return false;
                    FileInfo fInfo = new FileInfo(strAppPath);
                    if (fInfo.Length == 0)
                        return false;
                }
                return true;
            }

            public ListViewItem CreateListViewItem()
            {
                ListViewItem myitem = new ListViewItem();
                ListViewItem.ListViewSubItem name = new ListViewItem.ListViewSubItem();
                ListViewItem.ListViewSubItem existingVersion = new ListViewItem.ListViewSubItem();
                ListViewItem.ListViewSubItem latestVersion = new ListViewItem.ListViewSubItem();
                ListViewItem.ListViewSubItem serverArchitecture = new ListViewItem.ListViewSubItem();
                ListViewItem.ListViewSubItem existingDate = new ListViewItem.ListViewSubItem();
                ListViewItem.ListViewSubItem latestDate = new ListViewItem.ListViewSubItem();
                ListViewItem.ListViewSubItem lastUsed = new ListViewItem.ListViewSubItem();
                ListViewItem.ListViewSubItem status = new ListViewItem.ListViewSubItem();

                myitem.Name = this.Name;

                name.Name = "Name";
                existingVersion.Name = "Existing Version";
                latestVersion.Name = "Latest Version";
                serverArchitecture.Name = "Build";
                existingDate.Name = "Existing Date";
                latestDate.Name = "Latest Date";
                lastUsed.Name = "Last Used";
                status.Name = "Status";

                name.Text = this.DisplayName;

                Version v = this.availableVersion;
                if (v != null)
                {
                    latestVersion.Text = v.FileVersion;
                    latestDate.Text = v.UploadDate.ToShortDateString();
                }
                else
                {
                    latestVersion.Text = "unknown";
                    latestDate.Text = "unknown";
                }

                if (this.CurrentVersion != null && !String.IsNullOrEmpty(this.CurrentVersion.FileVersion))
                {
                    existingVersion.Text = this.CurrentVersion.FileVersion;
                    if (this.CurrentVersion.UploadDate.Year > 1)
                        existingDate.Text = this.CurrentVersion.UploadDate.ToShortDateString();
                    else
                        existingDate.Text = "N/A";
                }
                else
                {
                    existingVersion.Text = "N/A";
                    existingDate.Text = "N/A";
                }

                serverArchitecture.Text = "N/A";
                if (this.AvailableVersion.Architecture.Equals("64"))
                    serverArchitecture.Text = "x64";
                else if (this.AvailableVersion.Architecture.Equals("32"))
                    serverArchitecture.Text = "x86";
                else if (this.AvailableVersion.Architecture.Equals("any"))
                    serverArchitecture.Text = "any";

                ProgramSettings pSettings = UpdateCacher.GetPackage(this.name);
                if (pSettings != null && !pSettings.UpdateAllowed())
                {
                    status.Text = EnumProxy.Create(PackageStatus.Disabled).ToString();
                }
                else if (!this.isAvailable())
                {
                    status.Text = EnumProxy.Create(PackageStatus.Reinstall).ToString();
                    this.DownloadChecked = this.AllowUpdate = true;
                }
                else if (!HasAvailableVersion)
                {
                    if (this.DownloadChecked)
                        status.Text = EnumProxy.Create(PackageStatus.Reinstall).ToString();
                    else
                        status.Text = EnumProxy.Create(PackageStatus.NoUpdateAvailable).ToString();
                }
                else
                {
                    if (this.AllowUpdate)
                        status.Text = EnumProxy.Create(PackageStatus.UpdateAvailable).ToString();
                    else
                        status.Text = EnumProxy.Create(PackageStatus.UpdateIgnored).ToString();
                }

                if (this.AllowUpdate)
                {
                    if (this.DownloadChecked)
                        myitem.Checked = true;
                    else
                        myitem.Checked = false;
                }

                if (pSettings != null)
                {
                    if (pSettings.LastUsed.Year > 1)
                        lastUsed.Text = pSettings.LastUsed.ToShortDateString();
                    else
                        lastUsed.Text = "N/A";
                }
                else
                    lastUsed.Text = "---";

                myitem.SubItems.Add(name);
                myitem.SubItems.Add(existingVersion);
                myitem.SubItems.Add(latestVersion);
                myitem.SubItems.Add(serverArchitecture);
                myitem.SubItems.Add(existingDate);
                myitem.SubItems.Add(latestDate);
                myitem.SubItems.Add(lastUsed);
                myitem.SubItems.Add(status);
                return myitem;
            }
            
            private bool downloadChecked;
            public bool DownloadChecked
            {
                get { return downloadChecked; }
                set { downloadChecked = value; }
            }

            private int iRestartCount = 0;
            [XmlIgnore]
            public int RestartCount
            {
                get { return iRestartCount; }
                set { iRestartCount = value; }
            }

            private string savePath;
            [XmlIgnore]
            public string SavePath
            {
                get { return savePath; }
                set { savePath = value; }
            }

            private string saveFolder;
            [XmlIgnore]
            public string SaveFolder
            {
                get { return saveFolder; }
                set { saveFolder = value; }
            }
	
            protected Version currentVersion;
            public virtual Version CurrentVersion
            {
                get
                {
                    if (currentVersion == null)
                        currentVersion = new Version();
                    return currentVersion;
                }
                set { currentVersion = value; }
            }

            private Version availableVersion;
            public Version AvailableVersion
            {
                get { return availableVersion; }
                set { availableVersion = value; }
            }

            private bool allowUpdate;
            public bool AllowUpdate
            {
                get { return allowUpdate; }
                set
                {
                    if (!value)
                        downloadChecked = false;
                    allowUpdate = value;
                }
            }

            private string name;
            public string Name
            {
                get { return this.name; }
                set { this.name = value; }
            }

            private string displayName;
            public string DisplayName
            {
                get { return this.displayName; }
                set { this.displayName = value; }
            }

            private int requiredBuild;
            public int RequiredBuild
            {
                get { return requiredBuild; }
                set { requiredBuild = value; }
            }

            private string requiredNET;
            public string RequiredNET
            {
                get { return requiredNET; }
                set { requiredNET = value; }
            }

            private bool needsRestartedCopying = false;
            public bool NeedsRestartedCopying
            {
                get { return needsRestartedCopying; }
                set { needsRestartedCopying = value; }
            }

            public virtual ErrorState Install(iUpgradeable file)
            {
                return MainForm.Instance.UpdateHandler.ExtractFile(file);
            }
        }
        public class iUpgradeableCollection : CollectionBase
        {
            public iUpgradeableCollection() { }
            public iUpgradeableCollection(int capacity)
            {
                this.InnerList.Capacity = capacity;
            }

            public iUpgradeable this[int index]
            {
                get { return (iUpgradeable)this.List[index]; }
                set { this.List[index] = value; }
            }
            public void Add(iUpgradeable item)
            {
                if (FindByName(item.Name) != null)
                    throw new Exception("Can't have multiple upgradeable items with the same name");
                this.InnerList.Add(item);
            }
            public void Remove(iUpgradeable item)
            {
                this.InnerList.Remove(item);
            }
            public iUpgradeable FindByName(string name)
            {
                foreach (iUpgradeable file in this.InnerList)
                {
                    if (file.Name.Equals(name))
                        return file;
                }
                return null;
            }
            public int CountCheckedFiles()
            {
                int count=0;
                foreach (iUpgradeable file in this.InnerList)
                {
                    if (file.DownloadChecked)
                        count++;
                }
                return count;
            }
        }
        public class ProfilesFile : iUpgradeable
        {
            public ProfilesFile()
            {
            }

            public ProfilesFile(string name, MainForm mainForm)
            {
                this.Name = name;
                this.AllowUpdate = true;
                this.mainForm = mainForm;
            }
            
            private MainForm mainForm;

            public MainForm MainForm
            {
                set { mainForm = value; }
            }

            public override ErrorState Install(iUpgradeable file)
            {
                try
                {
                    string filename = Path.Combine(MainForm.Instance.Settings.MeGUIUpdateCache, Path.GetFileName(file.AvailableVersion.Url));
                    mainForm.importProfiles(filename, true);
                    if (mainForm.ImportProfileSuccessful == true)
                        return ErrorState.Successful;
                    else
                        return ErrorState.CouldNotInstall;
                }
                catch
                {
                    return ErrorState.CouldNotInstall;
                }
            }
        }

        public class AviSynthFile : iUpgradeable
        {
            public override void init()
            {
                base.init();
            }

            private AviSynthFile()
            {
            }

            public AviSynthFile(string name)
            {
                this.Name = name;
                this.AllowUpdate = true;
            }
        }

        public class MeGUIFile : iUpgradeable
        {
            private MeGUIFile()
            {
            }

            public MeGUIFile(string name)
            {
                this.Name = name;
                this.AllowUpdate = true;
                this.SaveFolder = Application.StartupPath;
            }

            public override Version CurrentVersion
            {
                get
                {
                    if (Name == "core")
                        base.CurrentVersion.FileVersion = new System.Version(Application.ProductVersion).Build.ToString();
                    return base.CurrentVersion;
                }
                set
                {
                    base.CurrentVersion = value;
                }
            }

            public override void init()
            {
                base.init();
                this.SaveFolder = Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath);
            }
        }

        public class ProgramFile : iUpgradeable
        {
            private ProgramFile()
            {
            }

            public override void init()
            {
                ProgramSettings pSettings = UpdateCacher.GetPackage(this.Name);
                if (pSettings == null || String.IsNullOrEmpty(pSettings.Path))
                    throw new FileNotRegisteredYetException(Name);
                SavePath = pSettings.Path;
            }
            
            public ProgramFile(string name) // Constructor
            {
                this.Name = name;
                this.AllowUpdate = true;
                init();
            }
        }

        public class Version : IComparable<Version>
        {
            public Version() { }

            private string architecture = string.Empty;
            public string Architecture
            {
                get { return architecture; }
                set { architecture = value; }
            }

            private string fileVersion = string.Empty;
            public string FileVersion
            {
                get { return fileVersion; }
                set { fileVersion = value; }
            }

            private string osbuild = string.Empty;
            public string OSBuild
            {
                get { return osbuild; }
                set { osbuild = value; }
            }

            private string url = string.Empty;
            public string Url
            {
                get { return url; }
                set { url = value; }
            }

            private DateTime uploadDate = DateTime.MinValue;
            public DateTime UploadDate
            {
                get { return uploadDate; }
                set { uploadDate = value; }
            }

            private string web = string.Empty;
            public string Web
            {
                get { return web; }
                set { web = value; }
            }

            /// <summary>
            /// Helper method to check if a update package is different
            /// </summary>
            /// <param name="version1">The first version to compare</param>
            /// <param name="version2">The second version to compare</param>
            /// <returns>0 if the versions are the same, different from 0 if they are not the same</returns>
            private int CompareUploadDate(Version version1, Version version2)
            {
                if (version1 == null && version2 == null)
                    return 0;
                else if (version1 == null)
                    return -1;
                else if (version2 == null)
                    return 1;
                else if (!version1.OSBuild.Equals(version2.OSBuild))
                    return 1;
                else if (version1.uploadDate != new DateTime() && version2.uploadDate != new DateTime())
                {
                    if (version1.uploadDate > version2.uploadDate)
                        return 1;
                    else if (version1.uploadDate < version2.uploadDate)
                        return -1;
                    else
                        return 0;
                }

                return 1;
            }

            #region IComparable<Version> Members

            public int CompareTo(Version other)
            {
                return CompareUploadDate(this, other);
            }

            #endregion
        }
        #endregion
        #region Delegates and delegate methods
        private delegate void BeginParseUpgradeXml(XmlNode node, XmlNode groupNode, string path);
        private delegate void SetLogText();
        private delegate void SetListView(ListViewItem item);
        private delegate void ClearItems(ListView listview);
        private delegate void UpdateProgressBar(int minValue, int maxValue, int currentValue, string text);
        public void SetProgressBar(int minValue, int maxValue, int currentValue, string text)
        {
            if (this.progressBar.InvokeRequired)
            {
                try
                {
                    UpdateProgressBar d = new UpdateProgressBar(SetProgressBar);
                    progressBar.Invoke(d, minValue, maxValue, currentValue, text);
                }
                catch (Exception) { }
            }
            else
            {
                this.progressBar.CustomText = text;
                this.progressBar.Minimum = (int)minValue;
                this.progressBar.Maximum = (int)maxValue;
                this.progressBar.Value = (int)currentValue;
            }
        }

        public void RefreshText()
        {
            SetLogText d = new SetLogText(UpdateLogText);
            if (this.txtBoxLog.InvokeRequired)
                this.Invoke(d);
            else
                d();
        }

        private void UpdateLogText()
        {
            this.txtBoxLog.Text = MainForm.Instance.UpdateHandler.UpdateLOG.ToString();
            this.txtBoxLog.SelectionStart = txtBoxLog.Text.Length;
            this.txtBoxLog.ScrollToCaret();
        }

        private void AddToListview(ListViewItem item)
        {
            if (this.listViewDetails.InvokeRequired)
            {
                SetListView d = new SetListView(AddToListview);
                this.Invoke(d, item);
            }
            else
                this.listViewDetails.Items.Add(item);
        }
        private void ClearListview(ListView listview)
        {
            if (listview.InvokeRequired)
            {
                ClearItems d = new ClearItems(ClearListview);
                this.Invoke(d, listview);
            }
            else
            {
                listview.Items.Clear();
            }
        }
        #endregion
        #region con/de struction
        private void UpdateWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Visible = false;
            MainForm.Instance.UpdateHandler.SaveSettings();
        }

        /// <summary>
        /// Constructor for Updatewindow.
        /// </summary>
        public UpdateWindow()
        {
            InitializeComponent();
            lvwColumnSorter = new ListViewColumnSorter();
            this.listViewDetails.ListViewItemSorter = lvwColumnSorter;
            lvwColumnSorter.SortColumn = 1;
            lvwColumnSorter.Order = SortOrder.Ascending;
            LoadComponentSettings();
            RefreshGUI();
        }

        private void LoadComponentSettings()
        {
            // Restore Size/Position of the window
            this.ClientSize = MainForm.Instance.Settings.UpdateFormSize;
            this.Location = MainForm.Instance.Settings.UpdateFormLocation;
            this.splitContainer2.SplitterDistance = MainForm.Instance.Settings.UpdateFormSplitter;

            colUpdate.Width = MainForm.Instance.Settings.UpdateFormUpdateColumnWidth;
            colName.Width = MainForm.Instance.Settings.UpdateFormNameColumnWidth;
            colExistingVersion.Width = MainForm.Instance.Settings.UpdateFormLocalVersionColumnWidth;
            colLatestVersion.Width = MainForm.Instance.Settings.UpdateFormServerVersionColumnWidth;
            colArchitecture.Width = MainForm.Instance.Settings.UpdateFormServerArchitectureColumnWidth;
            colExistingDate.Width = MainForm.Instance.Settings.UpdateFormLocalDateColumnWidth;
            colLatestDate.Width = MainForm.Instance.Settings.UpdateFormServerDateColumnWidth;
            colLastUsed.Width = MainForm.Instance.Settings.UpdateFormLastUsedColumnWidth;
            colStatus.Width = MainForm.Instance.Settings.UpdateFormStatusColumnWidth;
        }

        private void SaveComponentSettings()
        {
            MainForm.Instance.Settings.UpdateFormUpdateColumnWidth = colUpdate.Width;
            MainForm.Instance.Settings.UpdateFormNameColumnWidth = colName.Width;
            MainForm.Instance.Settings.UpdateFormLocalVersionColumnWidth = colExistingVersion.Width;
            MainForm.Instance.Settings.UpdateFormServerVersionColumnWidth = colLatestVersion.Width;
            MainForm.Instance.Settings.UpdateFormServerArchitectureColumnWidth = colArchitecture.Width;
            MainForm.Instance.Settings.UpdateFormLocalDateColumnWidth = colExistingDate.Width;
            MainForm.Instance.Settings.UpdateFormServerDateColumnWidth = colLatestDate.Width;
            MainForm.Instance.Settings.UpdateFormLastUsedColumnWidth = colLastUsed.Width;
            MainForm.Instance.Settings.UpdateFormStatusColumnWidth = colStatus.Width;
        }

        private void UpdateWindow_Load(object sender, EventArgs e)
        {
            // Move window in the visible area of the screen if neccessary
            Size oSizeScreen = Screen.GetWorkingArea(this).Size;
            Point oLocation = Screen.GetWorkingArea(this).Location;
            int iScreenHeight = oSizeScreen.Height - 2 * SystemInformation.FixedFrameBorderSize.Height;
            int iScreenWidth = oSizeScreen.Width - 2 * SystemInformation.FixedFrameBorderSize.Width;

            if (this.Size.Height >= iScreenHeight)
                this.Location = new Point(this.Location.X, oLocation.Y);
            else if (this.Location.Y <= oLocation.Y)
                this.Location = new Point(this.Location.X, oLocation.Y);
            else if (this.Location.Y + this.Size.Height > iScreenHeight)
                this.Location = new Point(this.Location.X, iScreenHeight - this.Size.Height);

            if (this.Size.Width >= iScreenWidth)
                this.Location = new Point(oLocation.X, this.Location.Y);
            else if (this.Location.X <= oLocation.X)
                this.Location = new Point(oLocation.X, this.Location.Y);
            else if (this.Location.X + this.Size.Width > iScreenWidth)
                this.Location = new Point(iScreenWidth - this.Size.Width, this.Location.Y);

            if (OSInfo.IsWindowsVistaOrNewer)
                OSInfo.SetWindowTheme(listViewDetails.Handle, "explorer", null);
        }
        #endregion
        #region GUI
        public void RefreshGUI()
        {
            int iUpdatesCount = MainForm.Instance.UpdateHandler.NumUpdatableFiles();
            if (chkShowAllFiles.Checked != (iUpdatesCount == 0))
            {
                if (chkShowAllFiles.InvokeRequired)
                    chkShowAllFiles.Invoke(new MethodInvoker(delegate { chkShowAllFiles.Checked = iUpdatesCount == 0; }));
                else
                    chkShowAllFiles.Checked = iUpdatesCount == 0;
            }
            else
            {
                if (listViewDetails.InvokeRequired)
                    listViewDetails.Invoke(new MethodInvoker(delegate { DisplayItems(chkShowAllFiles.Checked); }));
                else
                    DisplayItems(chkShowAllFiles.Checked);
            }
        }

        public void EnableUpdateButton()
        {
            bUpdateAllowed = true;
            if (btnUpdate.InvokeRequired)
                btnUpdate.Invoke(new MethodInvoker(delegate { btnUpdate.Enabled = true; }));
            else
                btnUpdate.Enabled = true;
        }

        private void DisplayItems(bool bShowAllFiles)
        {
            if (!this.Visible)
                return;

            ClearListview(this.listViewDetails);

            foreach (iUpgradeable file in MainForm.Instance.UpdateHandler.UpdateData)
            {
                // only add a package to the view if it is not completly disabled in the settings
                ProgramSettings pSettings = UpdateCacher.GetPackage(file.Name);
                if (pSettings != null && !pSettings.PackageToBeShown())
                    continue;

                if (!bShowAllFiles)
                {
                    
                    if (file.DownloadChecked)
                        AddToListview(file.CreateListViewItem());
                }
                else
                    AddToListview(file.CreateListViewItem());
            }

            listViewDetails.Sort();

            foreach (ListViewItem item in listViewDetails.Items)
            {
                if (item.Index % 2 != 0)
                    item.BackColor = Color.White;
                else
                    item.BackColor = Color.FromArgb(255, 225, 235, 255);
            }
        }

        private void listViewDetails_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (e.Column == 0)
                return;

            // Determine if clicked column is already the column that is being sorted.
            if (e.Column == lvwColumnSorter.SortColumn)
            {
                // Reverse the current sort direction for this column.
                if (lvwColumnSorter.Order == SortOrder.Ascending)
                {
                    lvwColumnSorter.Order = SortOrder.Descending;
                }
                else
                {
                    lvwColumnSorter.Order = SortOrder.Ascending;
                }
            }
            else
            {
                // Set the column number that is to be sorted; default to ascending.
                lvwColumnSorter.SortColumn = e.Column;
                lvwColumnSorter.Order = SortOrder.Ascending;
            }

            // Perform the sort with these new sort options.
            listViewDetails.Sort();

            foreach (ListViewItem item in listViewDetails.Items)
            {
                if (item.Index % 2 != 0)
                    item.BackColor = Color.White;
                else
                    item.BackColor = Color.FromArgb(255, 225, 235, 255);
            }
        }

        private void listViewDetails_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            ListViewItem itm = this.listViewDetails.Items[e.Index];
            // Do not allow checking if there are no updates or it is set to ignore.
            if (itm.SubItems["Status"].Text.Equals(EnumProxy.Create(PackageStatus.NoUpdateAvailable).ToString())
                || itm.SubItems["Status"].Text.Equals(EnumProxy.Create(PackageStatus.UpdateIgnored).ToString())
                || itm.SubItems["Status"].Text.Equals(EnumProxy.Create(PackageStatus.Disabled).ToString()))
                e.NewValue = CheckState.Unchecked;

            iUpgradeable file = MainForm.Instance.UpdateHandler.UpdateData.FindByName(itm.Name);
            if (e.NewValue == CheckState.Checked)
                file.DownloadChecked = file.AllowUpdate = true;
            else
                file.DownloadChecked = false;

            if (e.NewValue == CheckState.Unchecked
                && itm.SubItems["Status"].Text.Equals(EnumProxy.Create(PackageStatus.Reinstall).ToString()))
            {
                ProgramSettings pSettings = UpdateCacher.GetPackage(itm.Name);
                if (pSettings != null && !pSettings.UpdateAllowed())
                    itm.SubItems["Status"].Text = EnumProxy.Create(PackageStatus.Disabled).ToString();
                else if (!file.isAvailable())
                {
                    e.NewValue = CheckState.Checked;
                    file.DownloadChecked = file.AllowUpdate = true;
                }
                else if (!file.AllowUpdate && file.HasAvailableVersion)
                    itm.SubItems["Status"].Text = EnumProxy.Create(PackageStatus.UpdateIgnored).ToString();
                else if (file.HasAvailableVersion)
                    itm.SubItems["Status"].Text = EnumProxy.Create(PackageStatus.UpdateAvailable).ToString();
                else
                    itm.SubItems["Status"].Text = EnumProxy.Create(PackageStatus.NoUpdateAvailable).ToString();
            }
        }

        private void listViewDetails_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right || listViewDetails.SelectedItems.Count != 1)
                return;

            // get the program settings
            ProgramSettings pSettings = UpdateCacher.GetPackage(listViewDetails.SelectedItems[0].Name);
            iUpgradeable file = MainForm.Instance.UpdateHandler.UpdateData.FindByName(listViewDetails.SelectedItems[0].Name);

            // set the enable package value
            ToolStripMenuItem ts = (ToolStripMenuItem)statusToolStrip.Items[0];
            if (pSettings == null)
            {
                ts.Checked = true;
                ts.Enabled = false;
            }
            else
            {
                ts.Checked = pSettings.UpdateAllowed();
                ts.Enabled = !pSettings.Required;
            }

            // set the ignore update data value
            ts = (ToolStripMenuItem)statusToolStrip.Items[1];
            if (pSettings != null && !pSettings.UpdateAllowed())
            {
                ts.Checked = true;
                ts.Enabled = false;
            }
            else if (!file.isAvailable())
            {
                ts.Checked = false;
                ts.Enabled = false;
            }
            else
            {
                ts.Checked = !file.AllowUpdate;
                ts.Enabled = true;
            }

            // set the force reinstall data value
            ts = (ToolStripMenuItem)statusToolStrip.Items[2];
            if (pSettings != null && !pSettings.UpdateAllowed())
            {
                ts.Checked = false;
                ts.Enabled = false;
            }
            else if (!file.isAvailable())
            {
                ts.Checked = true;
                ts.Enabled = false;
            }
            else
            {
                ts.Checked = file.DownloadChecked;
                ts.Enabled = true;
            }

            if (file.HasAvailableVersion)
                ts.Text = "Install";
            else
                ts.Text = "Force reinstall";

            statusToolStrip.Show(Cursor.Position);
        }

        private void setIgnoreValue_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem ts = (ToolStripMenuItem)sender;
            foreach (ListViewItem item in listViewDetails.SelectedItems)
            {
                ProgramSettings pSettings = UpdateCacher.GetPackage(item.Name);
                iUpgradeable file = MainForm.Instance.UpdateHandler.UpdateData.FindByName(item.Name);
                Version latest = file.AvailableVersion;
                file.AllowUpdate = !(ts.Checked);

                if (pSettings != null && !pSettings.UpdateAllowed())
                {
                    item.SubItems["Status"].Text = EnumProxy.Create(PackageStatus.Disabled).ToString();
                    item.Checked = false;
                }
                else if (!file.isAvailable())
                {
                    item.SubItems["Status"].Text = EnumProxy.Create(PackageStatus.Reinstall).ToString();
                    item.Checked = true;
                    file.AllowUpdate = true;
                }
                else if (latest != null && file.CurrentVersion == null || latest.CompareTo(file.CurrentVersion) != 0)
                {
                    if (!file.AllowUpdate)
                    {
                        item.SubItems["Status"].Text = EnumProxy.Create(PackageStatus.UpdateIgnored).ToString();
                        item.Checked = false;
                    }
                    else
                    {
                        item.SubItems["Status"].Text = EnumProxy.Create(PackageStatus.UpdateAvailable).ToString();
                        item.Checked = true;
                    }
                }
                else
                {
                    item.SubItems["Status"].Text = EnumProxy.Create(PackageStatus.NoUpdateAvailable).ToString();
                    item.Checked = false;
                }
            }
        }

        private void reinstallToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem ts = (ToolStripMenuItem)sender;
            foreach (ListViewItem item in listViewDetails.SelectedItems)
            {
                ProgramSettings pSettings = UpdateCacher.GetPackage(item.Name);
                iUpgradeable file = MainForm.Instance.UpdateHandler.UpdateData.FindByName(item.Name);
                Version latest = file.AvailableVersion;

                if (pSettings != null && !pSettings.UpdateAllowed())
                {
                    item.SubItems["Status"].Text = EnumProxy.Create(PackageStatus.Disabled).ToString();
                    item.Checked = false;
                }
                else if (ts.Checked || (!file.isAvailable()))
                {
                    item.SubItems["Status"].Text = EnumProxy.Create(PackageStatus.Reinstall).ToString();
                    item.Checked = true;
                }
                else if (latest != null && file.CurrentVersion == null || latest.CompareTo(file.CurrentVersion) != 0)
                {
                    if (!file.AllowUpdate)
                    {
                        item.SubItems["Status"].Text = EnumProxy.Create(PackageStatus.UpdateIgnored).ToString();
                        item.Checked = false;
                    }
                    else
                    {
                        item.SubItems["Status"].Text = EnumProxy.Create(PackageStatus.UpdateAvailable).ToString();
                        item.Checked = false;
                    }
                }
                else
                {
                    item.SubItems["Status"].Text = EnumProxy.Create(PackageStatus.NoUpdateAvailable).ToString();
                    item.Checked = false;
                }
            }
        }

        private void enableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem ts = (ToolStripMenuItem)sender;
            foreach (ListViewItem item in listViewDetails.SelectedItems)
            {
                ProgramSettings pSettings = UpdateCacher.GetPackage(item.Name);
                if (pSettings != null)
                    UpdateCacher.CheckPackage(item.Name, ts.Checked, false);
                iUpgradeable file = MainForm.Instance.UpdateHandler.UpdateData.FindByName(item.Name);
                Version latest = file.AvailableVersion;

                if (pSettings != null && !pSettings.UpdateAllowed())
                {
                    item.SubItems["Status"].Text = EnumProxy.Create(PackageStatus.Disabled).ToString();
                    item.Checked = false;
                }
                else if (!file.isAvailable())
                {
                    item.SubItems["Status"].Text = EnumProxy.Create(PackageStatus.Reinstall).ToString();
                    item.Checked = true;
                }
                else if (latest != null && file.CurrentVersion == null || latest.CompareTo(file.CurrentVersion) != 0)
                {
                    item.SubItems["Status"].Text = EnumProxy.Create(PackageStatus.UpdateAvailable).ToString();
                    item.Checked = true;
                }
                else
                {
                    item.SubItems["Status"].Text = EnumProxy.Create(PackageStatus.NoUpdateAvailable).ToString();
                    item.Checked = false;
                }             
            }
        }

        public void StartUpdate()
        {
            btnUpdate_Click(null, null);
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            btnUpdate.Enabled = false;

            Thread updateThread = new Thread(new ThreadStart(MainForm.Instance.UpdateHandler.ProcessUpdate));
            updateThread.IsBackground = true;
            updateThread.Start();
            while (updateThread.IsAlive)
            {
                Application.DoEvents();
                Thread.Sleep(100);
            }

            RefreshGUI();

            btnUpdate.Enabled = bUpdateAllowed;
        }

        #endregion
        #region updating

        private void chkShowAllFiles_CheckedChanged(object sender, EventArgs e)
        {
            DisplayItems(chkShowAllFiles.Checked);
        }

        private void UpdateWindow_Move(object sender, EventArgs e)
        {
            if (this.WindowState != FormWindowState.Minimized && this.Visible == true)
                MainForm.Instance.Settings.UpdateFormLocation = this.Location;
        }

        private void UpdateWindow_Resize(object sender, EventArgs e)
        {
            if (this.WindowState != FormWindowState.Minimized && this.Visible == true)
                MainForm.Instance.Settings.UpdateFormSize = this.ClientSize;
        }

        private void splitContainer2_SplitterMoved(object sender, SplitterEventArgs e)
        {
            if (this.WindowState != FormWindowState.Minimized && this.Visible == true)
                MainForm.Instance.Settings.UpdateFormSplitter = this.splitContainer2.SplitterDistance;
        }

        private void listViewDetails_ColumnWidthChanged(object sender, ColumnWidthChangedEventArgs e)
        {
            SaveComponentSettings();
        }

        private void splitContainer1_SizeChanged(object sender, EventArgs e)
        {
            this.splitContainer1.SplitterDistance = this.splitContainer1.Size.Height - MainForm.Instance.Settings.DPIRescale(65);
        }

        private void UpdateWindow_Shown(object sender, EventArgs e)
        {
            RefreshGUI();
        }

        public void CloseWindow()
        {
            if (this.InvokeRequired)
                this.Invoke(new MethodInvoker(delegate { this.Close(); }));
            else
                this.Close();
        }
    }

    #endregion

    /// <summary>
    /// This class is an implementation of the 'IComparer' interface.
    /// </summary>
    public class ListViewColumnSorter : IComparer
    {
        /// <summary>
        /// Specifies the column to be sorted
        /// </summary>
        private int ColumnToSort;
        /// <summary>
        /// Specifies the order in which to sort (i.e. 'Ascending').
        /// </summary>
        private SortOrder OrderOfSort;
        /// <summary>
        /// Case insensitive comparer object
        /// </summary>
        private CaseInsensitiveComparer ObjectCompare;

        /// <summary>
        /// Class constructor.  Initializes various elements
        /// </summary>
        public ListViewColumnSorter()
        {
            // Initialize the column to '0'
            ColumnToSort = 0;

            // Initialize the sort order to 'none'
            OrderOfSort = SortOrder.None;

            // Initialize the CaseInsensitiveComparer object
            ObjectCompare = new CaseInsensitiveComparer();
        }

        /// <summary>
        /// This method is inherited from the IComparer interface.  It compares the two objects passed using a case insensitive comparison.
        /// </summary>
        /// <param name="x">First object to be compared</param>
        /// <param name="y">Second object to be compared</param>
        /// <returns>The result of the comparison. "0" if equal, negative if 'x' is less than 'y' and positive if 'x' is greater than 'y'</returns>
        public int Compare(object x, object y)
        {
            int compareResult;
            ListViewItem listviewX, listviewY;
            DateTime dateX, dateY;            
            
            // Cast the objects to be compared to ListViewItem objects
            listviewX = (ListViewItem)x;
            listviewY = (ListViewItem)y;

            // Compare the two items
            if (DateTime.TryParse(listviewX.SubItems[ColumnToSort].Text, out dateX) &&
                DateTime.TryParse(listviewY.SubItems[ColumnToSort].Text, out dateY))
                compareResult = ObjectCompare.Compare(dateX, dateY);
            else
                compareResult = ObjectCompare.Compare(listviewX.SubItems[ColumnToSort].Text, listviewY.SubItems[ColumnToSort].Text);

            // Calculate correct return value based on object comparison
            if (OrderOfSort == SortOrder.Ascending)
            {
                // Ascending sort is selected, return normal result of compare operation
                return compareResult;
            }
            else if (OrderOfSort == SortOrder.Descending)
            {
                // Descending sort is selected, return negative result of compare operation
                return (-compareResult);
            }
            else
            {
                // Return '0' to indicate they are equal
                return 0;
            }
        }

        /// <summary>
        /// Gets or sets the number of the column to which to apply the sorting operation (Defaults to '0').
        /// </summary>
        public int SortColumn
        {
            set
            {
                ColumnToSort = value;
            }
            get
            {
                return ColumnToSort;
            }
        }

        /// <summary>
        /// Gets or sets the order of sorting to apply (for example, 'Ascending' or 'Descending').
        /// </summary>
        public SortOrder Order
        {
            set
            {
                OrderOfSort = value;
            }
            get
            {
                return OrderOfSort;
            }
        }
    }

    public class UpdateTool : MeGUI.core.plugins.interfaces.ITool
    {

        #region IOption Members

        public string Name
        {
            get { return "Update"; }
        }

        public void Run(MainForm info)
        {
            MainForm.Instance.UpdateHandler.ShowUpdateWindow(false, false);
        }

        public Shortcut[] Shortcuts
        {
            get { return new Shortcut[] { Shortcut.CtrlU }; }
        }

        #endregion

        #region IIDable Members

        public string ID
        {
            get { return "update_window"; }
        }

        #endregion
    }

    public class FileNotRegisteredYetException : MeGUIException
    {
        private string name;

        public string Name { get { return name; } }
        public FileNotRegisteredYetException(string name) : base("AutoUpdate file '" + name + "' not registered with MeGUI.")
        {
            this.name = name;
        }
    }
}