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
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

using MeGUI.core.details;
using MeGUI.core.gui;
using MeGUI.core.plugins.interfaces;
using MeGUI.core.util;

namespace MeGUI
{
    public delegate void UpdateGUIStatusCallback(StatusUpdate su); // catches the UpdateGUI events fired from the encoder

    /// <summary>
    /// MainForm is the main GUI of the program
    /// it contains all the elements required to start encoding and contains the application intelligence as well.
    /// </summary>
    public partial class MainForm : Form
    {
        // This instance is to be used by the serializers that can't be passed a MainForm as a parameter
        public static MainForm Instance;

        #region variable declaration
        private List<string> filesToDeleteOnClosing = new List<string>();
        private List<Form> allForms = new List<Form>();
        private List<Form> formsToReopen = new List<Form>();
        private ITaskbarList3 taskbarItem;
        private Icon taskbarIcon;
        private string strLogFile;
        private UpdateHandler _updateHandler;
        private List<ProgramSettings> _programsettings;
        private bool restart = false;
        private DialogManager dialogManager;
        private string path; // path the program was started from
        private MediaFileFactory mediaFileFactory;
        private PackageSystem packageSystem = new PackageSystem();
        private MuxProvider muxProvider;
        private MeGUISettings settings = new MeGUISettings();
        private ProfileManager profileManager;
        private CodecManager codecs;
        public List<ProgramSettings> ProgramSettings { get { return _programsettings; } set { _programsettings = value; } }
        public bool IsHiddenMode { get { return trayIcon.Visible; } }
        public bool IsOverlayIconActive { get { return taskbarIcon != null; } }
        public string LogFile { get { return strLogFile; } }
        public UpdateHandler UpdateHandler { get { return _updateHandler; } set { _updateHandler = value; } }
        public MuxProvider MuxProvider { get { return muxProvider; } }

        private LogItem _oneClickLog;
        private LogItem _autoEncodeLog;
        private LogItem _aVSScriptCreatorLog;
        private LogItem _fileIndexerLog;
        private LogItem _eac3toLog;
        private LogItem _avisynthWrapperLog;
        private LogItem _mediaInfoWrapperLog;
        private LogItem _oTempLog = new LogItem("Log", ImageType.NoImage);
        public LogItem OneClickLog { get { return _oneClickLog; } set { _oneClickLog = value; } }
        public LogItem AutoEncodeLog { get { return _autoEncodeLog; } set { _autoEncodeLog = value; } }
        public LogItem AVSScriptCreatorLog { get { return _aVSScriptCreatorLog; } set { _aVSScriptCreatorLog = value; } }
        public LogItem FileIndexerLog { get { return _fileIndexerLog; } set { _fileIndexerLog = value; } }
        public LogItem Eac3toLog { get { return _eac3toLog; } set { _eac3toLog = value; } }
        public LogItem AviSynthWrapperLog { get { return _avisynthWrapperLog; } set { _avisynthWrapperLog = value; } }
        public LogItem MediaInfoWrapperLog { get { return _mediaInfoWrapperLog; } set { _mediaInfoWrapperLog = value; } }

        #endregion

        public void DeleteOnClosing(string file)
        {
            filesToDeleteOnClosing.Add(file);
        }

        /// <summary>
        /// launches the megui wiki in the default browser
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuGuide_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://en.wikibooks.org/wiki/MeGUI");
        }

        public MainForm()
        {
            // Log File Handling
            string strMeGUILogPath = Path.GetDirectoryName(Application.ExecutablePath) + @"\logs";
            FileUtil.ensureDirectoryExists(strMeGUILogPath);
            strLogFile = strMeGUILogPath + @"\logfile-" + DateTime.Now.ToString("yy'-'MM'-'dd'_'HH'-'mm'-'ss") + ".log";
            FileUtil.WriteToFile(strLogFile, "Preliminary log file only. During closing of MeGUI the well formed log file will be written.\r\n\r\n", false);
            Instance = this;
            constructMeGUIInfo();
            InitializeComponent();
            logTree1.SetLog(_oTempLog);
            System.Reflection.Assembly myAssembly = this.GetType().Assembly;
            string name = this.GetType().Namespace + ".";
#if CSC
			name = "";
#endif
            string[] resources = myAssembly.GetManifestResourceNames();
            this.trayIcon.Icon = new Icon(myAssembly.GetManifestResourceStream(name + "App.ico"));
            this.Icon = trayIcon.Icon;
            this.TitleText = Application.ProductName + " " + new System.Version(Application.ProductVersion).Build;
            if (MainForm.Instance.Settings.IsMeGUIx64)
                this.TitleText += " x64";
            getVersionInformation();
            if (!MainForm.Instance.Settings.AutoUpdate)
                this.TitleText += " UPDATE SERVER DISABLED";
            else if (MainForm.Instance.Settings.AutoUpdateServerSubList == 1)
                this.TitleText += " DEVELOPMENT UPDATE SERVER";
            setGUIInfo();
            Jobs.ShowAfterEncodingStatus(Settings);
            this.videoEncodingComponent1.FileType = MainForm.Instance.Settings.MainFileFormat;

            this.ClientSize = settings.MainFormSize;
            this.Location = settings.MainFormLocation;

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

            GetChangeLog();
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            this.ClientSize = settings.MainFormSize;

            if (OSInfo.IsWindows7OrNewer)
            {
                taskbarItem = (ITaskbarList3)new ProgressTaskbar();
                setOverlayIcon(taskbarIcon, true);
            }

            if (settings.WorkerAutoStartOnStartup)
                jobControl1.StartAll(false);

            if (MainForm.Instance.Settings.UpdateMode != UpdateMode.Disabled)
                _updateHandler.BeginUpdateCheck();
        }

        private void GetChangeLog()
        {
            try
            {
                txtChangeLog.Font = new Font(FontFamily.GenericMonospace, txtChangeLog.Font.Size);

                // check if changelog is available
                string changeLog = Path.Combine(Application.StartupPath, "changelog.txt");
                if (!File.Exists(changeLog))
                {
                    txtChangeLog.Text = "Changelog cannot be found: " + changeLog;
                    return;
                }

                // copy the complete file content into the text box
                txtChangeLog.Text = File.ReadAllText(changeLog);

                // if the last used version is equal to the currently used version, do not take any special actions
                if (MainForm.Instance.Settings.Version.Equals(new System.Version(Application.ProductVersion).Build.ToString()))
                    return;

                if (!String.IsNullOrEmpty(MainForm.Instance.Settings.Version))
                {
                    if ((new System.Version(Application.ProductVersion).Build) > Int32.Parse(MainForm.Instance.Settings.Version))
                    {
                        // the last used version is known and the version is lower than the used version
                        // therefore try to highlight only the new changes

                        int startIndex = 0;
                        string version = "";
                        bool bDark = true;

                        foreach (string line in txtChangeLog.Lines)
                        {
                            if (FileUtil.RegExMatch(line, @"^\d{4,} ", true))
                                version = line.Substring(0, line.IndexOf(" "));
                            else if (FileUtil.RegExMatch(line, @"--> \d{4,}$", true))
                                version = line.Substring(line.IndexOf("--> ") + 4);

                            if (bDark && !String.IsNullOrEmpty(version) && Int32.Parse(version) <= Int32.Parse(MainForm.Instance.Settings.Version))
                                bDark = false;

                            txtChangeLog.Select(startIndex, line.Length);
                            if (bDark)
                                txtChangeLog.SelectionColor = Color.Black;
                            else
                                txtChangeLog.SelectionColor = Color.LightGray;

                            startIndex += line.Length + 1;
                        }
                    }
                }

                // select the changelog tab
                tabControl1.SelectedIndex = 3;
                MainForm.Instance.Settings.Version = new System.Version(Application.ProductVersion).Build.ToString();
            }
            catch (Exception ex)
            {
                txtChangeLog.Text = "Changelog cannot be displayed: " + ex.Message;
            }
        }

        #region GUI properties
        public JobControl Jobs
        {
            get { return jobControl1; }
        }

        public bool ProcessStatusChecked
        {
            get { return progressMenu.Checked; }
            set { progressMenu.Checked = value; }
        }

        public VideoEncodingComponent Video
        {
            get { return videoEncodingComponent1; }
        }

        public AudioEncodingComponent Audio
        {
            get { return audioEncodingComponent1; }
        }
        #endregion

        /// <summary>
        /// handles the GUI closing event
        /// saves all jobs, stops the currently active job and saves all profiles as well
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            if (jobControl1.IsAnyWorkerRunning)
            {
                e.Cancel = true; // abort closing
                MessageBox.Show("Please close running jobs before you close MeGUI.", "Job in progress", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            if (!e.Cancel)
            {
                if (!CloseSilent())
                    e.Cancel = true; // abort closing
            }
            base.OnClosing(e);
        }

        #region reset
        private void resetButton_Click(object sender, System.EventArgs e)
        {
            videoEncodingComponent1.Reset();
            audioEncodingComponent1.Reset();
        }
        #endregion

        #region auto encoding
        private void autoEncodeButton_Click(object sender, System.EventArgs e)
        {
            RunTool("AutoEncode");
        }

        private void RunTool(string p)
        {
            try
            {
                ITool tool = PackageSystem.Tools[p];
                tool.Run(this);
            }
            catch (KeyNotFoundException)
            {
                MessageBox.Show("Required tool, '" + p + "', not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region job management
        #region I/O verification
        /// <summary>
        /// Test whether a filename is suitable for writing to
        /// </summary>
        /// <param name="filename"></param>
        /// <returns>Error message if problem, null if ok</returns>
        public static string verifyOutputFile(string filename)
        {
            try
            {
                filename = Path.GetFullPath(filename);  // this will throw ArgumentException if invalid
                if (File.Exists(filename))
                {
                    FileStream fs = File.OpenWrite(filename);  // this will throw if we'll have problems writing
                    fs.Close();
                }
                else
                {
                    FileStream fs = File.Create(filename);  // this will throw if we'll have problems writing
                    fs.Close();
                    File.Delete(filename);
                }
            }
            catch (Exception e)
            {
                return e.Message;
            }
            return null;
        }

        /// <summary>
        /// Test whether a filename is suitable for reading from
        /// </summary>
        /// <param name="filename"></param>
        /// <returns>Error message if problem, null if ok</returns>
        public static string verifyInputFile(string filename)
        {
            try
            {
                filename = Path.GetFullPath(filename);  // this will throw ArgumentException if invalid
                FileStream fs = File.OpenRead(filename);  // this will throw if we'll have problems reading
                fs.Close();
            }
            catch (Exception e)
            {
                return e.Message;
            }
            return null;
        }
        #endregion
        #endregion

        #region settings
        /// <summary>
        /// saves the global GUI settings to settings.xml
        /// </summary>
        public void saveSettings()
        {
            XmlSerializer ser = null;
            string fileName = this.path + @"\settings.xml";
            using (Stream s = File.Open(fileName, System.IO.FileMode.Create, System.IO.FileAccess.Write))
            {
                try
                {
                    ser = new XmlSerializer(typeof(MeGUISettings));
                    ser.Serialize(s, this.settings);
                }
                catch (Exception ex)
                {
                    LogItem _oLog = MainForm.Instance.Log.Info("Error");
                    _oLog.LogValue("saveSettings", ex, ImageType.Error);
                }
            }
        }
        /// <summary>
        /// loads the global settings
        /// </summary>
        public void loadSettings()
        {
            this._programsettings = new List<ProgramSettings>();
            string fileName = Path.Combine(path, "settings.xml");
            if (File.Exists(fileName))
            {
                XmlSerializer ser = null;
                using (Stream s = File.OpenRead(fileName))
                {
                    ser = new XmlSerializer(typeof(MeGUISettings));
                    try
                    {
                        this.settings = (MeGUISettings)ser.Deserialize(s);
                    }
                    catch
                    {
                        MessageBox.Show("MeGUI settings could not be loaded. Default values will be applied now.", "Error loading MeGUI settings", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            MainForm.Instance.Settings.InitializeProgramSettings();
        }

        #endregion

        #region GUI updates

        #region helper methods
        public string TitleText
        {
            get
            {
                return this.Text;
            }
            set
            {
                this.Text = value;
                trayIcon.Text = value;
            }
        }

        /// <summary>
        /// shuts down the PC if the shutdown option is set
        /// also saves all profiles, jobs and the log as MeGUI is killed
        /// via the shutdown so the appropriate methods in the OnClosing are not called
        /// </summary>
        public void runAfterEncodingCommands()
        {
            if (Jobs.CurrentAfterEncoding == AfterEncoding.DoNothing)
                return;

            if (Jobs.CurrentAfterEncoding == AfterEncoding.Shutdown)
            {
                if (!CloseSilent())
                    return; // abort closing

                using (CountdownWindow countdown = new CountdownWindow(30))
                {
                    if (countdown.ShowDialog() == DialogResult.OK)
                    {
                        bool succ = Shutdown.shutdown();
                        if (!succ)
                            Log.LogEvent("Tried and failed to shut down system");
                        else
                            Log.LogEvent("Shutdown initiated");
                    }
                    else
                        Log.LogEvent("User aborted shutdown");
                }
            }
            else if (Jobs.CurrentAfterEncoding == AfterEncoding.CloseMeGUI)
            {
                if (CloseSilent())
                    Application.Exit();
            }
            else if (Jobs.CurrentAfterEncoding == AfterEncoding.RunCommand && !String.IsNullOrEmpty(settings.AfterEncodingCommand))
            {
                string filename = MeGUIPath + @"\after_encoding.bat";
                try
                {
                    using (StreamWriter s = new StreamWriter(File.OpenWrite(filename)))
                    {
                        s.WriteLine(settings.AfterEncodingCommand);
                    }
                    ProcessStartInfo psi = new ProcessStartInfo(filename);
                    psi.CreateNoWindow = true;
                    psi.UseShellExecute = false;
                    Process p = new Process();
                    p.StartInfo = psi;
                    p.Start();
                }
                catch (Exception ex) { MessageBox.Show("Error when attempting to run after encoding command: " + ex.Message, "Run command failed", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            }
        }

        public LogItem Log
        {
            get
            {
                if (logTree1 == null)
                    return _oTempLog;
                return logTree1.Log;
            }
        }

        /// <summary>
        /// saves the whole content of the log into a logfile
        /// </summary>
        public void saveLog()
        {
            string text = Log.ToString();
            FileUtil.WriteToFile(strLogFile, text, false);
        }

        private void exitMeGUIToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// returns the profile manager to whomever might require it
        /// </summary>
        public ProfileManager Profiles
        {
            get
            {
                return this.profileManager;
            }
        }
        #endregion

        #region menu actions
        private void mnuFileOpen_Click(object sender, EventArgs e)
        {
            MediaInfoFile oInfo;
            openFileDialog.Filter = "All files|*.*";
            openFileDialog.Title = "Select your input file";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
                openFile(openFileDialog.FileName, out oInfo);
        }
        private void mnuViewMinimizeToTray_Click(object sender, EventArgs e)
        {
            formsToReopen.Clear();
            this.Visible = false;
            trayIcon.Visible = true;
        }

        private void mnuFileExit_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }

        private void mnuTool_Click(object sender, System.EventArgs e)
        {
            if ((!(sender is System.Windows.Forms.MenuItem)) || (!((sender as MenuItem).Tag is ITool)))
                return;
            ((ITool)(sender as MenuItem).Tag).Run(this);
        }

        private void mnuMuxer_Click(object sender, System.EventArgs e)
        {
            if ((!(sender is System.Windows.Forms.MenuItem)) || (!((sender as MenuItem).Tag is IMuxing)))
                return;
            MuxWindow mw = new MuxWindow((IMuxing)((sender as MenuItem).Tag), this);
            mw.Show();
        }

        private void mnuView_Popup(object sender, System.EventArgs e)
        {
            List<Pair<string, bool>> workers = Jobs.ListProgressWindows();
            progressMenu.MenuItems.Clear();
            progressMenu.MenuItems.Add(showAllProgressWindows);
            progressMenu.MenuItems.Add(hideAllProgressWindows);
            progressMenu.MenuItems.Add(separator2);

            foreach (Pair<string, bool> p in workers)
            {
                MenuItem i = new MenuItem(p.fst);
                i.Checked = p.snd;
                i.Click += new EventHandler(mnuProgress_Click);
                progressMenu.MenuItems.Add(i);
            }

            if (workers.Count == 0)
            {
                MenuItem i = new MenuItem("(No progress windows to show)");
                i.Enabled = false;
                progressMenu.MenuItems.Add(i);
            }
        }

        void mnuProgress_Click(object sender, EventArgs e)
        {
            MenuItem i = (MenuItem)sender;
            if (i.Checked)
                Jobs.HideProgressWindow(i.Text);
            else
                Jobs.ShowProgressWindow(i.Text);
        }
        private void mnuViewProcessStatus_Click(object sender, System.EventArgs e)
        {
        }

        #endregion

        public MeGUISettings Settings
        {
            get { return settings; }
        }

        public MediaFileFactory MediaFileFactory
        {
            get { return mediaFileFactory; }
        }

        #region tray action
        private void trayIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            // Activate the form.
            this.Show(); this.Activate();

            if (progressMenu.Checked)
                Jobs.ShowAllProcessWindows();
            trayIcon.Visible = false;
        }
        private void openMeGUIToolStripMenuItem_Click(object sender, EventArgs e)
        {
            trayIcon.Visible = false;
            this.Visible = true;
        }

        #endregion

        #region file opening
        private void openOtherVideoFile(string fileName)
        {
            AviSynthWindow asw = new AviSynthWindow(this, fileName);
            asw.OpenScript += new OpenScriptCallback(Video.openVideoFile);
            asw.Show();
        }

        private void openIndexableFile(string fileName)
        {
            if (DialogManager.useOneClick())
            {
                OneClickWindow ocmt = new OneClickWindow();
                ocmt.Show();
                ocmt.setInput(fileName);
            }
            else
            {
                FileIndexerWindow mpegInput = new FileIndexerWindow(this);
                mpegInput.setConfig(fileName, null, 2, true, true, true, false);
                mpegInput.Show();
            }
        }

        /// <summary>
        /// tries to open the selected input file
        /// </summary>
        /// <returns>true if it is a proper video AVS file</returns>
        public bool openFile(string file, out MediaInfoFile iFile)
        {
            iFile = null;

            if (Path.GetExtension(file.ToLowerInvariant()).Equals(".zip"))
            {
                importProfiles(file);
                return false;
            }

            if (Directory.Exists(file))
            {
                OneClickWindow ocmt = new OneClickWindow();
                ocmt.Show();
                ocmt.setInput(file);
                return false;
            }

            iFile = new MediaInfoFile(file);
            if (iFile.HasVideo)
            {
                if (iFile.recommendIndexer(out _))
                {
                    openIndexableFile(file);
                }
                else
                {
                    this.tabControl1.SelectedIndex = 0;
                    if (iFile.HasAudio)
                        audioEncodingComponent1.openAudioFile(file);
                    if (iFile.ContainerFileTypeString.Equals("AVS"))
                    {
                        Video.openVideoFile(file, iFile);
                        return true;
                    }
                    else
                        openOtherVideoFile(file);
                }
            }
            else if (iFile.HasAudio)
            {
                audioEncodingComponent1.openAudioFile(file);
                this.tabControl1.SelectedIndex = 0;
            }
            else if (Path.GetExtension(iFile.FileName).ToLowerInvariant().Equals(".avs"))
            {
                try
                {
                    using (AvsFile avi = AvsFile.OpenScriptFile(iFile.FileName))
                    {
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error parsing avs file", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
                MessageBox.Show("This file cannot be opened", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
        }

        private void importProfiles(string file)
        {
            new ProfileImporter(this, file, false).ShowDialog();
        }
        #endregion

        #region Drag 'n' Drop
        private void MeGUI_DragDrop(object sender, DragEventArgs e)
        {
            MediaInfoFile oInfo;
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            Thread openFileThread = new Thread((ThreadStart)delegate 
            {
                if (this.InvokeRequired)
                    Invoke(new MethodInvoker(delegate { openFile(files[0], out oInfo); }));
                else
                    openFile(files[0], out oInfo); 
            });
            openFileThread.Start();
        }

        private void MeGUI_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.None;
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop, false);
                if (files.Length > 0)
                    e.Effect = DragDropEffects.All;
            }
        }
        #endregion

        #region importing
        public void importProfiles(string file, bool bAuto)
        {
            Util.ThreadSafeRun(this, delegate
            {
                ProfileImporter importer = new ProfileImporter(this, file, true);
                if (importer.ErrorDuringInit())
                    return;

                if (MainForm.Instance.settings.UpdateMode != UpdateMode.Automatic)
                {
                    importer.Show();
                    while (importer.Visible == true)    // wait until the profiles have been imported
                        MeGUI.core.util.Util.Wait(100);
                }
                else
                    importer.AutoImport();
            });
        }

        private bool bImportProfileSuccessful = false;
        public bool ImportProfileSuccessful
        {
            get { return bImportProfileSuccessful; }
            set { bImportProfileSuccessful = value; }
        }

        private void mnuFileImport_Click(object sender, EventArgs e)
        {
            try
            {
                new ProfileImporter(this, false).ShowDialog();
            }
            catch (CancelledException) { }
        }

        private void mnuFileExport_Click(object sender, EventArgs e)
        {
            new ProfileExporter().ShowDialog();
        }
        #endregion

        private void mnuToolsAdaptiveMuxer_Click(object sender, EventArgs e)
        {
            AdaptiveMuxWindow amw = new AdaptiveMuxWindow();
            amw.Show();
        }

        internal bool CloseSilent()
        {
            while (!this.profileManager.SaveProfiles())
            {
                DialogResult dR = MessageBox.Show("The profiles could not be saved.\r\nIf you ignore this problem, profile data may be lost!", "Profile backup failed", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Warning);
                if (dR == DialogResult.Abort)
                    return false;
                else if (dR == DialogResult.Ignore)
                    break;
            }
            this.saveSettings();
            _updateHandler.SaveSettings();
            UpdateCacher.RemoveOldFiles();
            jobControl1.SaveJobs();
            FileUtil.RemoveRuntimeFiles();
            this.saveLog();
            deleteFiles();
            this.runRestarter();
            return true;
        }

        private void deleteFiles()
        {
            foreach (string file in filesToDeleteOnClosing)
            {
                try
                {
                    FileUtil.DeleteDirectoryIfExists(file, true);
                    if (File.Exists(file))
                        File.Delete(file);
                }
                catch { }
            }
        }

        #region MeGUIInfo

        #region start and end
        public void setGUIInfo()
        {
            fillMenus();
            jobControl1.LoadJobs();
        }

        /// <summary>
        /// default constructor
        /// initializes all the GUI components, initializes the internal objects and makes a default selection for all the GUI dropdowns
        /// In addition, all the jobs and profiles are being loaded from the harddisk
        /// </summary>
        public void constructMeGUIInfo()
        {
            this.muxProvider = new MuxProvider(this);
            this.codecs = new CodecManager();
            this.path = System.Windows.Forms.Application.StartupPath;
            this.addPackages();
            this.profileManager = new ProfileManager(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "allprofiles"));
            this.profileManager.LoadProfiles();
            this.mediaFileFactory = new MediaFileFactory(this);
            this.loadSettings();
            MainForm.Instance.settings.DPIRescaleAll();
            this.dialogManager = new DialogManager(this);
        }

        private void fillMenus()
        {
            // Fill the muxing menu
            mnuMuxers.MenuItems.Clear();
            mnuToolsAdaptiveMuxer.Shortcut = Shortcut.Ctrl1;
            mnuMuxers.MenuItems.Add(mnuToolsAdaptiveMuxer);
            int index = 1;
            foreach (IMuxing muxer in PackageSystem.MuxerProviders.Values)
            {
                if (muxer.Shortcut == Shortcut.None)
                    continue;

                MenuItem newMenuItem = new MenuItem();
                newMenuItem.Text = muxer.Name;
                newMenuItem.Tag = muxer;
                newMenuItem.Index = index;
                newMenuItem.Shortcut = muxer.Shortcut;
                index++;
                mnuMuxers.MenuItems.Add(newMenuItem);
                newMenuItem.Click += new System.EventHandler(this.mnuMuxer_Click);
            }

            // Fill the tools menu
            mnuTools.MenuItems.Clear();
            List<MenuItem> toolsItems = new List<MenuItem>();
            List<Shortcut> usedShortcuts = new List<Shortcut>();
            toolsItems.Add(mnutoolsD2VCreator);
            toolsItems.Add(mnuMuxers);
            usedShortcuts.Add(mnuMuxers.Shortcut);

            foreach (ITool tool in PackageSystem.Tools.Values)
            {
                if (tool.Name != "File Indexer")
                {
                    MenuItem newMenuItem = new MenuItem();
                    newMenuItem.Text = tool.Name;
                    newMenuItem.Tag = tool;
                    newMenuItem.Click += new System.EventHandler(this.mnuTool_Click);
                    bool shortcutAttempted = false;
                    foreach (Shortcut s in tool.Shortcuts)
                    {
                        shortcutAttempted = true;
                        Debug.Assert(s != Shortcut.None);
                        if (!usedShortcuts.Contains(s))
                        {
                            usedShortcuts.Add(s);
                            newMenuItem.Shortcut = s;
                            break;
                        }
                    }

                    if (shortcutAttempted && newMenuItem.Shortcut == Shortcut.None)
                        Log.Warn("Shortcut for '" + tool.Name + "' is already used. No shortcut selected.");
                    toolsItems.Add(newMenuItem);
                }
            }

            toolsItems.Sort(new Comparison<MenuItem>(delegate(MenuItem a, MenuItem b) { return (a.Text.CompareTo(b.Text)); }));
            index = 0;
            foreach (MenuItem m in toolsItems)
            {
                m.Index = index;
                index++;
                mnuTools.MenuItems.Add(m);
            }
        }

        private void addPackages()
        {
            PackageSystem.JobProcessors.Register(AviSynthAudioEncoder.Factory);
            PackageSystem.JobProcessors.Register(ffmpegEncoder.Factory);
            PackageSystem.JobProcessors.Register(x264Encoder.Factory);
            PackageSystem.JobProcessors.Register(x265Encoder.Factory);
            PackageSystem.JobProcessors.Register(XviDEncoder.Factory);
            PackageSystem.JobProcessors.Register(MkvMergeMuxer.Factory);
            PackageSystem.JobProcessors.Register(MP4BoxMuxer.Factory);
            PackageSystem.JobProcessors.Register(AMGMuxer.Factory);
            PackageSystem.JobProcessors.Register(tsMuxeR.Factory);
            PackageSystem.JobProcessors.Register(FFmpegMuxer.Factory);
            PackageSystem.JobProcessors.Register(MkvExtract.Factory);
            PackageSystem.JobProcessors.Register(PgcDemux.Factory);
            PackageSystem.JobProcessors.Register(OneClickPostProcessing.Factory);
            PackageSystem.JobProcessors.Register(CleanupJobRunner.Factory);
            PackageSystem.JobProcessors.Register(AviSynthProcessor.Factory);
            PackageSystem.JobProcessors.Register(D2VIndexer.Factory);
            PackageSystem.JobProcessors.Register(DGMIndexer.Factory);
            PackageSystem.JobProcessors.Register(DGIIndexer.Factory);
            PackageSystem.JobProcessors.Register(FFMSIndexer.Factory);
            PackageSystem.JobProcessors.Register(LSMASHIndexer.Factory);
            PackageSystem.JobProcessors.Register(VobSubIndexer.Factory);
            PackageSystem.JobProcessors.Register(MP4FpsMod.Factory);
            PackageSystem.JobProcessors.Register(MeGUI.packages.tools.besplitter.Joiner.Factory);
            PackageSystem.JobProcessors.Register(MeGUI.packages.tools.besplitter.Splitter.Factory);
            PackageSystem.JobProcessors.Register(HDStreamExtractorIndexer.Factory);
            PackageSystem.JobProcessors.Register(CleanupJobRunner.Factory);

            PackageSystem.MuxerProviders.Register(new AVIMuxGUIMuxerProvider());
            PackageSystem.MuxerProviders.Register(new TSMuxerProvider());
            PackageSystem.MuxerProviders.Register(new FFmpegMuxerProvider());
            PackageSystem.MuxerProviders.Register(new MKVMergeMuxerProvider());
            PackageSystem.MuxerProviders.Register(new MP4BoxMuxerProvider());

            PackageSystem.Tools.Register(new MeGUI.packages.tools.cutter.CutterTool());
            PackageSystem.Tools.Register(new AviSynthWindowTool());
            PackageSystem.Tools.Register(new AutoEncodeTool());
            PackageSystem.Tools.Register(new CQMEditorTool());
            PackageSystem.Tools.Register(new CalculatorTool());
            PackageSystem.Tools.Register(new ChapterCreatorTool());
            PackageSystem.Tools.Register(new MeGUI.packages.tools.besplitter.BesplitterTool());
            PackageSystem.Tools.Register(new OneClickTool());
            PackageSystem.Tools.Register(new D2VCreatorTool());
            PackageSystem.Tools.Register(new VobSubTool());
            PackageSystem.Tools.Register(new MeGUI.packages.tools.hdbdextractor.HdBdExtractorTool());
            PackageSystem.Tools.Register(new UpdateTool());

            PackageSystem.MediaFileTypes.Register(new AvsFileFactory());        // HandleLevel 30
            PackageSystem.MediaFileTypes.Register(new dgmFileFactory());        // HandleLevel 20
            PackageSystem.MediaFileTypes.Register(new dgiFileFactory());        // HandleLevel 19
            PackageSystem.MediaFileTypes.Register(new d2vFileFactory());        // HandleLevel 18
            PackageSystem.MediaFileTypes.Register(new lsmashFileFactory());     // HandleLevel 13
            PackageSystem.MediaFileTypes.Register(new ffmsFileFactory());       // HandleLevel 12
            PackageSystem.MediaFileTypes.Register(new MediaInfoFileFactory());  // HandleLevel  5

            PackageSystem.JobPreProcessors.Register(BitrateCalculatorPreProcessor.CalculationProcessor);

            PackageSystem.JobPostProcessors.Register(d2vIndexJobPostProcessor.PostProcessor);
            PackageSystem.JobPostProcessors.Register(dgmIndexJobPostProcessor.PostProcessor);
            PackageSystem.JobPostProcessors.Register(dgiIndexJobPostProcessor.PostProcessor);
            PackageSystem.JobPostProcessors.Register(ffmsIndexJobPostProcessor.PostProcessor);
            PackageSystem.JobPostProcessors.Register(lsmashIndexJobPostProcessor.PostProcessor);
            PackageSystem.JobPostProcessors.Register(CleanupJobRunner.DeleteIntermediateFilesPostProcessor);

            // edit job
            PackageSystem.JobConfigurers.Register(MuxWindow.Configurer);
            PackageSystem.JobConfigurers.Register(AudioEncodingWindow.Configurer);
            //PackageSystem.JobConfigurers.Register(VobSubIndexWindow.Configurer);
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            // prevent another instance of MeGUI from the same location
            int iCount = 0;
            foreach (Process oProc in Process.GetProcessesByName(Application.ProductName))
            {
                try
                {
                    if (Application.ExecutablePath.Equals(oProc.MainModule.FileName))
                        iCount++;
                }
                catch { }
            }
            if (iCount > 1)
            {
                MessageBox.Show("There is already another instance of the application running.", "MeGUI Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // check if the program can write to the program dir
            if (!FileUtil.IsDirWriteable(Path.GetDirectoryName(Application.ExecutablePath)))
            {
                // parse if the program has already been started elevated
                Boolean bRunElevated = false;
                foreach (string strParam in args)
                {
                    if (strParam.Equals("-elevate"))
                    {
                        bRunElevated = true;
                        break;
                    }
                }

                // if needed run as elevated process
                if (!bRunElevated)
                {
                    try
                    {
                        Process p = new Process();
                        p.StartInfo.FileName = Application.ExecutablePath;
                        p.StartInfo.Arguments = "-elevate";
                        p.StartInfo.Verb = "runas";
                        p.Start();
                        return;
                    }
                    catch
                    {
                    }
                }

                MessageBox.Show("MeGUI cannot be started as it cannot write to the application directory.\rPlease grant the required permissions or move the application to an unprotected directory.", "MeGUI Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

#if !DEBUG
            // catch uncatched errors
            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
#endif

            // delete PDB file if outdated
            try
            {
                string strDebugFile = Path.ChangeExtension(Application.ExecutablePath, ".pdb");
                if (File.Exists(strDebugFile))
                {
                    DateTime creationAPP = File.GetLastWriteTime(Application.ExecutablePath);
                    DateTime creationPDB = File.GetLastWriteTime(strDebugFile);
                    double difference = (creationAPP < creationPDB) ? (creationPDB - creationAPP).TotalSeconds : (creationAPP - creationPDB).TotalSeconds;
                    if (difference > 360)
                        File.Delete(strDebugFile);
                }
            } catch { }

            Application.EnableVisualStyles();

            MainForm mainForm = new MainForm();

            // start MeGUI form if not blocked
            bool bStart = true;
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == "--dont-start")
                    bStart = false;
            }
            if (bStart)
                Application.Run(mainForm);
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            HandleUnhandledException((Exception)e.ExceptionObject);
        }

        static void HandleUnhandledException(Exception e)
        {
            if (MainForm.Instance != null)
            {
                LogItem i = MainForm.Instance.Log.Error("Unhandled error");
                i.LogValue("Exception message", e.Message);
                i.LogValue("Stacktrace", e.StackTrace);
                i.LogValue("Inner exception", e.InnerException);
                foreach (System.Collections.DictionaryEntry info in e.Data)
                    i.LogValue(info.Key.ToString(), info.Value);
            }

            MessageBox.Show("MeGUI encountered a fatal error and may not be able to proceed. Reason: " + e.Message
                , "Fatal error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            HandleUnhandledException(e.Exception);
        }

        private void runRestarter()
        {
            if (_updateHandler.PackagesToUpdateAtRestart.Count == 0 && !restart)
                return;

            // check if the old updater is still available and delete if found
            if (File.Exists(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"updatecopier.exe")))
                File.Delete(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"updatecopier.exe"));

            if (File.Exists(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"LinqBridge.dll")))
                File.Delete(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"LinqBridge.dll"));

            if (_updateHandler.PackagesToUpdateAtRestart.Count > 0)
            {
                using (Stream fs = new FileStream(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "update.arg"), FileMode.Create))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(List<UpgradeData>));
                    XmlWriterSettings settings = new XmlWriterSettings();
                    settings.Indent = true;
                    XmlWriter writer = XmlTextWriter.Create(fs, settings);
                    serializer.Serialize(writer, _updateHandler.PackagesToUpdateAtRestart);
                }
            }

            Process proc = new Process();
            ProcessStartInfo pstart = new ProcessStartInfo();
            pstart.FileName = MainForm.Instance.settings.MeGUI_Updater.Path;
            if (restart)
                pstart.Arguments += "--restart ";

            // check if the program can write to the program dir
            if (FileUtil.IsDirWriteable(Path.GetDirectoryName(Application.ExecutablePath)))
            {
                pstart.CreateNoWindow = true;
                pstart.UseShellExecute = false;
            }
            else
            {
                // need admin permissions
                proc.StartInfo.Verb = "runas";
                pstart.UseShellExecute = true;
            }

            proc.StartInfo = pstart;
            try
            {
                if (!proc.Start())
                    MessageBox.Show("Could not run updater", "MeGUI Update Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch
            {
                MessageBox.Show("Could not run updater", "MeGUI Update Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region properties
        public PackageSystem PackageSystem
        {
            get { return packageSystem; }
        }

        public bool Restart
        {
            get { return restart; }
            set { restart = value; }
        }

        public DialogManager DialogManager
        {
            get { return dialogManager; }
        }

        /// <summary>
        /// gets the path from where MeGUI was launched
        /// </summary>
        public string MeGUIPath
        {
            get { return this.path; }
        }
        #endregion

        #endregion

        internal void ClosePlayer()
        {
            videoEncodingComponent1.ClosePlayer();
        }

        internal void hidePlayer()
        {
            videoEncodingComponent1.hidePlayer();
        }

        internal void showPlayer()
        {
            videoEncodingComponent1.showPlayer();
        }

        private void viewSummary_Click(object sender, EventArgs e)
        {
            Jobs.ShowSummary();
        }

        private void showAllProgressWindows_Click(object sender, EventArgs e)
        {
            Jobs.ShowAllProcessWindows();
        }

        private void hideAllProgressWindows_Click(object sender, EventArgs e)
        {
            Jobs.HideAllProcessWindows();
        }

        private void mnuForum_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://forum.doom9.org/forumdisplay.php?f=78");
        }

        private void mnuHome_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://sourceforge.net/projects/megui");
        }

        private void mnuBugTracker_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://sourceforge.net/tracker/?group_id=156112&atid=798476");
        }

        private void mnuFeaturesReq_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://sourceforge.net/tracker/?group_id=156112&atid=798479");
        }

        private void mnuDoc_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://en.wikibooks.org/wiki/MeGUI");
        }

        private void mnuOptionsSettings_Click(object sender, EventArgs e)
        {
            using (SettingsForm sform = new SettingsForm())
            {
                sform.Settings = this.settings;
                if (sform.ShowDialog() == DialogResult.OK)
                {
                    MeGUISettings.StandbySettings oldStandbyValue = this.settings.StandbySetting;
                    
                    this.settings = sform.Settings;
                    this.saveSettings();
                    Jobs.ShowAfterEncodingStatus(settings);

                    // check if standby settings have been changed and if yes, change them now
                    if (oldStandbyValue != settings.StandbySetting)
                    {
                        MeGUI.core.util.WindowUtil.AllowSystemPowerdown();
                        if (MainForm.Instance.Jobs.IsAnyJobRunning)
                            MeGUI.core.util.WindowUtil.PreventSystemPowerdown();
                    }
                }
            }
        }

        private void getVersionInformation()
        {
            bool bDebug = false;
#if DEBUG
            bDebug = true;
#endif
            LogItem i = Log.Info("Versions");
            if (!MainForm.Instance.Settings.IsMeGUIx64)
                i.LogValue("MeGUI", new System.Version(Application.ProductVersion).Build + " x86" + (bDebug ? " (DEBUG)" : string.Empty), false);
            else
                i.LogValue("MeGUI", new System.Version(Application.ProductVersion).Build + " x64" + (bDebug ? " (DEBUG)" : string.Empty), false);
            if (File.Exists(Path.ChangeExtension(Application.ExecutablePath, ".pdb")))
                i.LogValue("MeGUI Debug Data", "available", false);
            string strUpdateCheck = "Disabled";
            if (MainForm.Instance.Settings.AutoUpdate)
            {
                if (MainForm.Instance.Settings.AutoUpdateServerSubList == 0)
                    strUpdateCheck = "stable update server";
                else if (MainForm.Instance.Settings.AutoUpdateServerSubList == 1)
                    strUpdateCheck = "development update server";
                else if (MainForm.Instance.Settings.AutoUpdateServerSubList == 2)
                    strUpdateCheck = "custom update server";
            }
            i.LogValue("Update Check", strUpdateCheck, false);

            LogItem s = new LogItem("System Information");
            s.LogValue("Operating System", OSInfo.GetOSName(), false);
            string version40 = OSInfo.GetDotNetVersion("4.0");
            if (String.IsNullOrEmpty(version40))
                s.LogValue(".NET Framework 4.0", "not installed", ImageType.Warning, false);
            else
                s.LogValue(".NET Framework", string.Format("{0}", version40), false);
            string version = OSInfo.GetDotNetVersion();
            if (!String.IsNullOrEmpty(version) && !version40.Equals(version))
                s.LogValue(".NET Framework", string.Format("{0}", version), false);

            s.Add(FileUtil.GetRedistInformation());

            // get DPI and resolution
            using (Graphics graphics = Graphics.FromHwnd(IntPtr.Zero))
            {
                float dpiX = graphics.DpiX;
                float dpiY = graphics.DpiY;
                s.LogValue("DPI", string.Format("{0}% ({1}/{2})", Math.Round(MainForm.Instance.settings.DPIScaleFactor*100, 0), dpiX, dpiY), false);  
            }
            int iCount = 1;
            foreach (Screen screen in Screen.AllScreens)
            {
                LogItem m = new LogItem("Monitor " + iCount++);
                m.LogValue("Resolution", screen.Bounds.Width + "x" + screen.Bounds.Height, false);
                m.LogValue("Primary Screen", screen.Primary.ToString(), false);
                s.Add(m);
            }

            i.Add(s);

            this.UpdateHandler = new UpdateHandler();

            LogItem v = new LogItem("Component Information");
            string haaliPath = FileUtil.GetHaaliInstalledPath();
            FileUtil.GetFileInformation("Haali Media Splitter", Path.Combine(haaliPath, "splitter.ax"), ref v);
            FileUtil.GetFileInformation("Haali DSS2", Path.Combine(haaliPath, "avss.dll"), ref v);
            FileUtil.GetFileInformation("ICSharpCode.SharpZipLib", Path.GetDirectoryName(Application.ExecutablePath) + @"\ICSharpCode.SharpZipLib.dll", ref v);
            FileUtil.GetFileInformation("MediaInfo", Path.GetDirectoryName(Application.ExecutablePath) + @"\MediaInfo.dll", ref v);
            FileUtil.GetFileInformation("SevenZipSharp", Path.GetDirectoryName(Application.ExecutablePath) + @"\SevenZipSharp.dll", ref v);
            FileUtil.GetFileInformation("7z", Path.GetDirectoryName(Application.ExecutablePath) + @"\7z.dll", ref v);
            i.Add(v);

            LogItem a = new LogItem("AviSynth Information");
            FileUtil.CheckAviSynth(ref a);
            i.Add(a);

            UpdateCacher.CheckPackage("libs");
            UpdateCacher.CheckPackage("mediainfo");
            UpdateCacher.CheckPackage("updater");
        }

        public void setOverlayIcon(Icon oIcon, bool bForce)
        {
            if (!OSInfo.IsWindows7OrNewer)
                return;

            if (oIcon == null)
            {
                // remove the overlay icon
                if (taskbarItem != null)
                    Util.ThreadSafeRun(this, delegate { taskbarItem.SetOverlayIcon(this.Handle, IntPtr.Zero, null); });
                taskbarIcon = null;
                return;
            }

            if (taskbarIcon != null && oIcon.Handle == taskbarIcon.Handle && !bForce)
                return;

            if (oIcon == System.Drawing.SystemIcons.Warning && taskbarIcon == System.Drawing.SystemIcons.Error)
                return;

            if (taskbarItem != null)
                Util.ThreadSafeRun(this, delegate { taskbarItem.SetOverlayIcon(this.Handle, oIcon.Handle, null); });
            taskbarIcon = oIcon;
        }

        private void OneClickEncButton_Click(object sender, EventArgs e)
        {
            RunTool("one_click");
        }

        private void HelpButton_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://en.wikibooks.org/wiki/MeGUI");
        }

        private void menuItem5_Click(object sender, EventArgs e)
        {
            FileIndexerWindow d2vc = new FileIndexerWindow(this);
            d2vc.Show();
        }

        private void MainForm_Move(object sender, EventArgs e)
        {
            if (this.WindowState != FormWindowState.Minimized && this.Visible == true)
                settings.MainFormLocation = this.Location;
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            if (this.WindowState != FormWindowState.Minimized && this.Visible == true)
                settings.MainFormSize = this.ClientSize;
        }
    }
}
        #endregion