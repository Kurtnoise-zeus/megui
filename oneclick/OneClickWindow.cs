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
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

using MeGUI.core.details;
using MeGUI.core.util;
using MeGUI.packages.tools.oneclick;

namespace MeGUI
{
    public partial class OneClickWindow : Form
    {
        #region variable declaration
        LogItem _oLog;
        List<OneClickStreamControl> audioTracks;
        List<OneClickStreamControl> subtitleTracks;
        OneClickSettings _oSettings;
        private bool bLock;

        /// <summary>
        /// whether the current processing should be done without user interaction
        /// </summary>
        private bool bAutomatedProcessing;

        /// <summary>
        /// whether the current processing is a part of batch processing
        /// </summary>
        private bool bBatchProcessing;

        /// <summary>
        /// whether we ignore the restrictions on container output type set by the profile
        /// </summary>
        private bool ignoreRestrictions = false;

        /// <summary>
        /// the restrictions from above: the only containers we may use
        /// </summary>
        private ContainerType[] acceptableContainerTypes;

        private MuxProvider muxProvider;
        private MediaInfoFile _videoInputInfo;
        #endregion

        #region profiles
        #region OneClick profiles
        private void initOneClickHandler()
        {
            oneclickProfile.Manager = MainForm.Instance.Profiles;
        }

        private void initTabs()
        {
            audioTracks = new List<OneClickStreamControl>();
            audioTracks.Add(oneClickAudioStreamControl1);
            oneClickAudioStreamControl1.StandardStreams = new object[] { "None" };
            oneClickAudioStreamControl1.SelectedStreamIndex = 0;
            oneClickAudioStreamControl1.Filter = VideoUtil.GenerateCombinedFilter(ContainerManager.AudioTypes.ValuesArray);

            subtitleTracks = new List<OneClickStreamControl>();
            subtitleTracks.Add(oneClickSubtitleStreamControl1);
            oneClickSubtitleStreamControl1.StandardStreams = new object[] { "None" };
            oneClickSubtitleStreamControl1.SelectedStreamIndex = 0;
            oneClickSubtitleStreamControl1.Filter = VideoUtil.GenerateCombinedFilter(ContainerManager.SubtitleTypes.ValuesArray);
        }

        void OneClickProfileChanged(object sender, EventArgs e)
        {
            SetOneClickProfile((OneClickSettings)oneclickProfile.SelectedProfile.BaseSettings);
        }
        #endregion
        #region Video profiles
        private VideoCodecSettings VideoSettings
        {
            get { return (VideoCodecSettings)videoProfile.SelectedProfile.BaseSettings; }
        }
        #endregion
        #region Audio profiles
        private void initAudioHandler()
        {
            oneClickAudioStreamControl1.initProfileHandler();
            oneClickSubtitleStreamControl1.initProfileHandler();
        }
        #endregion
        #endregion
        
        #region init
        public OneClickWindow()
        {
            this._oLog = MainForm.Instance.OneClickLog;
            if (_oLog == null)
            {
                _oLog = MainForm.Instance.Log.Info("OneClick");
                MainForm.Instance.OneClickLog = _oLog;
            }
            this.muxProvider = MainForm.Instance.MuxProvider;
            acceptableContainerTypes = muxProvider.GetSupportedContainers().ToArray();
            InitializeComponent();

            
            //add all container types
            if (containerFormat.Items.Count == 0)
            {
                containerFormat.Items.AddRange(muxProvider.GetSupportedContainers().ToArray());
                this.containerFormat.SelectedIndex = 0;
            }

            beingCalled++;
            videoProfile.Manager = MainForm.Instance.Profiles;
            initTabs();
            initAudioHandler();
            avsProfile.Manager = MainForm.Instance.Profiles;
            initOneClickHandler();
            beingCalled--;
            updatePossibleContainers();

            //add device type
            if (devicetype.Items.Count == 0)
            {
                devicetype.Items.Add("Standard");
                devicetype.Items.AddRange(muxProvider.GetSupportedDevices((ContainerType)this.containerFormat.SelectedItem).ToArray());
            }
            if (containerFormat.SelectedItem.ToString().Equals(MainForm.Instance.Settings.AedSettings.Container))
            {
                foreach (object o in devicetype.Items) // I know this is ugly, but using the DeviceOutputType doesn't work unless we're switching to manual serialization
                {
                    if (o.ToString().Equals(MainForm.Instance.Settings.AedSettings.DeviceOutputType))
                    {
                        devicetype.SelectedItem = o;
                        break;
                    }
                }
            }
            else
                this.devicetype.SelectedIndex = 0;

            bLock = true;
            cbGUIMode.DataSource = EnumProxy.CreateArray(new object[] { MeGUISettings.OCGUIMode.Basic, MeGUISettings.OCGUIMode.Default, MeGUISettings.OCGUIMode.Advanced });
            bLock = false;

            string filter = "All DGIndex supported files|*.vob;*.mpg;*.mpeg;*.m1v;*.m2v;*.mpv;*.tp;*.ts;*.trp;*.m2t;*.m2ts;*.pva;*.vro";
            filter += "|All FFMS Indexer supported files|*.mkv;*.avi;*.mp4;*.flv;*.wmv;*.ogm;*.vob;*.mpg;*.m2ts;*.ts";
            filter += "|All LSMASH Indexer supported files|*.mkv;*.avi;*.mp4;*.flv;*.wmv;*.ogm;*.vob;*.mpg;*.m2ts;*.ts";
            if (MainForm.Instance.Settings.IsDGIIndexerAvailable() || MainForm.Instance.Settings.IsDGMIndexerAvailable())
            {
                if (MainForm.Instance.Settings.IsDGIIndexerAvailable())
                    filter += "|All DGIndexNV supported files|*.264;*.h264;*.265;*.hevc;*.avc;*.m2v;*.mpv;*.vc1;*.mkv;*.vob;*.mp4;*.mpg;*.mpeg;*.m2t;*.m2ts;*.mts;*.tp;*.ts;*.trp";
                if (MainForm.Instance.Settings.IsDGMIndexerAvailable())
                    filter += "|All DGIndexIM supported files|*.264;*.h264;*.265;*.hevc;*.avc;*.m2v;*.mpv;*.vc1;*.mkv;*.vob;*.mp4;*.mpg;*.mpeg;*.m2t;*.m2ts;*.mts;*.tp;*.ts;*.trp";
                filter = "All supported files|*.avs;*.ifo;*.mkv;*.avi;*.mp4;*.flv;*.wmv;*.ogm;*.264;*.h264;*.avc;*.265;*.hevc;*.m2t*;*.m2ts;*.mts;*.tp;*.ts;*.trp;*.vob;*.mpg;*.mpeg;*.m1v;*.m2v;*.mpv;*.pva;*.vro;*.vc1;*.mpls|" + filter + "|All files|*.*";
                input.Filter = filter;
            }
            else
            {
                filter = "All supported files|*.avs;*.ifo;*.mkv;*.avi;*.mp4;*.flv;*.wmv;*.ogm;*.264;*.h264;*.avc;*.m2t*;*.m2ts;*.mts;*.tp;*.ts;*.trp;*.vob;*.mpg;*.mpeg;*.m1v;*.m2v;*.mpv;*.pva;*.vro;*.mpls|" + filter + "|All files|*.*";
                input.Filter = filter;
            }

            DragDropUtil.RegisterMultiFileDragDrop(input, setInput, delegate() { return input.Filter + "|All folders|*."; });
            DragDropUtil.RegisterSingleFileDragDrop(output, setOutput);
            DragDropUtil.RegisterSingleFileDragDrop(chapterFile, null, delegate() { return chapterFile.Filter; });
            DragDropUtil.RegisterSingleFileDragDrop(workingDirectory, setWorkingDirectory);
        }
        #endregion

        #region event handlers
        private void cbGUIMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            EnumProxy o = cbGUIMode.SelectedItem as EnumProxy;
            if (o == null)
                return;

            if (bLock)
                cbGUIMode.SelectedItem = EnumProxy.Create(MainForm.Instance.Settings.OneClickGUIMode);
            else
                MainForm.Instance.Settings.OneClickGUIMode = (MeGUISettings.OCGUIMode)o.RealValue;

            if (MainForm.Instance.Settings.OneClickGUIMode == MeGUISettings.OCGUIMode.Advanced)
            {
                audioTab.Height = MainForm.Instance.Settings.DPIRescale(175);
                subtitlesTab.Location = new Point(subtitlesTab.Location.X, MainForm.Instance.Settings.DPIRescale(258));
                subtitlesTab.Visible = true;
                outputTab.Location = new Point(outputTab.Location.X, MainForm.Instance.Settings.DPIRescale(379));
                this.Height = MainForm.Instance.Settings.DPIRescale(583);
                if (!tabControl1.TabPages.Contains(encoderConfigTab))
                    tabControl1.TabPages.Add(encoderConfigTab);
            }
            else if (MainForm.Instance.Settings.OneClickGUIMode == MeGUISettings.OCGUIMode.Basic)
            {
                audioTab.Height = MainForm.Instance.Settings.DPIRescale(90);
                subtitlesTab.Location = new Point(subtitlesTab.Location.X, MainForm.Instance.Settings.DPIRescale(198));
                subtitlesTab.Visible = false;
                outputTab.Location = new Point(outputTab.Location.X, MainForm.Instance.Settings.DPIRescale(173));
                this.Height = MainForm.Instance.Settings.DPIRescale(377);
                if (tabControl1.TabPages.Contains(encoderConfigTab))
                    tabControl1.TabPages.Remove(encoderConfigTab);
            }
            else
            {
                audioTab.Height = MainForm.Instance.Settings.DPIRescale(115);
                subtitlesTab.Location = new Point(subtitlesTab.Location.X, MainForm.Instance.Settings.DPIRescale(198));
                subtitlesTab.Visible = true;
                outputTab.Location = new Point(outputTab.Location.X, MainForm.Instance.Settings.DPIRescale(319));
                this.Height = MainForm.Instance.Settings.DPIRescale(523);
                if (tabControl1.TabPages.Contains(encoderConfigTab))
                    tabControl1.TabPages.Remove(encoderConfigTab);
            }
        }

        private void containerFormat_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.containerFormat.Text == "MKV")
                this.devicetype.Enabled = false;
            else 
                this.devicetype.Enabled = true;
            
            //add device types
            devicetype.Items.Clear();
            devicetype.Items.Add("Standard");
            devicetype.Items.AddRange(muxProvider.GetSupportedDevices((ContainerType)this.containerFormat.SelectedItem).ToArray());
            if (containerFormat.SelectedItem.ToString().Equals(MainForm.Instance.Settings.AedSettings.Container))
            {
                foreach (object o in devicetype.Items) // I know this is ugly, but using the DeviceOutputType doesn't work unless we're switching to manual serialization
                {
                    if (o.ToString().Equals(MainForm.Instance.Settings.AedSettings.DeviceOutputType))
                    {
                        devicetype.SelectedItem = o;
                        break;
                    }
                }
            }
            else
                this.devicetype.SelectedIndex = 0;

            updateChapterSelection(null, null);
            output.Filter = ((ContainerType)containerFormat.SelectedItem).OutputFilterString;
            updateFilename(true, false, false);
        }

        private void updateChapterSelection(object sender, EventArgs e)
        {
            if (this.containerFormat.Text == "AVI" 
                || (this.containerFormat.Text == "M2TS" && (devicetype.SelectedItem == null || devicetype.SelectedItem.ToString().Equals("Standard"))))
                chapterFile.Enabled = false;
            else
                chapterFile.Enabled = true;
        }

        private void updateFilename(bool bUpdateExtension, bool bUpdateFilePath, bool bUpdateFileName)
        {
            string strInputFile = input.SelectedText;
            if (String.IsNullOrEmpty(strInputFile))
                return;

            string strFileName = Path.GetFileName(input.SelectedText);
            if (bUpdateFileName || String.IsNullOrEmpty(output.Filename))
            {
                int iPGCNumber = 0;
                if (_videoInputInfo != null)
                    iPGCNumber = _videoInputInfo.VideoInfo.PGCNumber;

                string filePath = FileUtil.GetOutputFolder(strInputFile);
                string filePrefix = FileUtil.GetOutputFilePrefix(strInputFile);
                string fileName = Path.GetFileNameWithoutExtension(strInputFile);
                string strTempName = strInputFile;

                if (iPGCNumber > 0)
                {
                    // DVD structure found, create the output file name
                    if (FileUtil.RegExMatch(fileName, @"_\d{1,2}\z", false))
                        fileName = fileName.Substring(0, fileName.LastIndexOf('_') + 1);
                    else
                        fileName = fileName + "_";
                    fileName = filePrefix + fileName + iPGCNumber;
                    if (_videoInputInfo.VideoInfo.AngleNumber > 0)
                        fileName += "_" + _videoInputInfo.VideoInfo.AngleNumber;
                    strTempName = Path.Combine(filePath, fileName + Path.GetExtension(strInputFile));
                }
                else
                    strTempName = Path.Combine(filePath, filePrefix + fileName + Path.GetExtension(strInputFile));

                strFileName = PrettyFormatting.ExtractWorkingName(strTempName, _oSettings.LeadingName, _oSettings.SuffixName, _oSettings.WorkingNameReplace, _oSettings.WorkingNameReplaceWith);
                if (!String.IsNullOrEmpty(output.Filename))
                    output.Filename = Path.Combine(Path.GetDirectoryName(output.Filename), strFileName + "." + ((ContainerType)containerFormat.SelectedItem).Extension);
            }
            
            if (bUpdateFilePath || String.IsNullOrEmpty(output.Filename))
            {
                if (!String.IsNullOrEmpty(_oSettings.DefaultOutputDirectory) && Directory.Exists(_oSettings.DefaultOutputDirectory))
                {
                    output.Filename = Path.Combine(_oSettings.DefaultOutputDirectory, strFileName + "." +
                        ((ContainerType)containerFormat.SelectedItem).Extension);
                }
                else
                {
                    string outputPath = FileUtil.GetOutputFolder(strInputFile);
                    output.Filename = Path.Combine(outputPath, strFileName + "." +
                        ((ContainerType)containerFormat.SelectedItem).Extension);
                }
            }
            else if (bUpdateExtension)
                output.Filename = Path.ChangeExtension(output.Filename, ((ContainerType)containerFormat.SelectedItem).Extension); 
        }

        private void input_SelectionChanged(object sender, string val)
        {
            if (!String.IsNullOrEmpty(input.SelectedText))
            {
                if (!bAutomatedProcessing && !bLock)
                    openInput(input.SelectedText);
            }   
        }

        private void setOutput(string strFileName)
        {
            output.Filename = strFileName; 
        }

        private void setWorkingDirectory(string strFolder)
        {
            workingDirectory.Filename = strFolder;
            updateFilename(false, true, false);
        }

        private void workingDirectory_FileSelected(FileBar sender, FileBarEventArgs args)
        {
            if (File.Exists(workingDirectory.Filename))
                workingDirectory.Filename = Path.GetDirectoryName(workingDirectory.Filename);
            updateFilename(false, true, false);
        }

        private void setControlState(bool bDisableControls)
        {
            if (bDisableControls)
            {
                this.Cursor = Cursors.WaitCursor;
                goButton.Enabled = tabControl1.Enabled = false;
            }
            else
            {
                this.Cursor = Cursors.Default;
                goButton.Enabled = tabControl1.Enabled = true;
            }
        }

        public void setInput(string strFileorFolderName)
        {
            input.AddCustomItem(strFileorFolderName);
            input.SelectedObject = strFileorFolderName;
        }

        public void setInput(string[] strFileorFolderName)
        {
            List<OneClickFilesToProcess> arrFilesToProcess = new List<OneClickFilesToProcess>();
            List<string> arrFoldersToProcess = new List<string>();

            foreach (string strFile in strFileorFolderName)
            {
                if (File.Exists(strFile))
                    arrFilesToProcess.Add(new OneClickFilesToProcess(strFile, 1, 0));
                else if (Directory.Exists(strFile))
                    arrFoldersToProcess.Add(strFile);
            }

            foreach (string strFolder in arrFoldersToProcess)
            {
                if (!String.IsNullOrEmpty(FileUtil.GetBlurayPath(strFolder))
                    || !String.IsNullOrEmpty(FileUtil.GetDVDPath(strFolder)))
                {
                    // DVD or Blu-ray structure found
                    setControlState(true);
                    OneClickProcessing oProcessorFolder = new OneClickProcessing(this, strFolder, _oSettings, _oLog);
                    return;
                }

                foreach (string strFile in Directory.GetFiles(strFolder))
                {
                    arrFilesToProcess.Add(new OneClickFilesToProcess(strFile, 1, 0));
                }
            }

            if (arrFilesToProcess.Count == 0)
            {
                MessageBox.Show("These files or folders cannot be used in OneClick mode.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            setControlState(true);
            OneClickProcessing oProcessor = new OneClickProcessing(this, arrFilesToProcess, _oSettings, _oLog); 
        }
        
        private void openInput(string fileName)
        {
            if (!Directory.Exists(fileName) && !File.Exists(fileName))
            {
                MessageBox.Show("Input " + fileName + " does not exists", "Input not found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            setControlState(true);
            OneClickProcessing oProcessor = new OneClickProcessing(this, fileName, _oSettings, _oLog);
        }

        public void setOpenFailure(bool bSilent)
        {
            setControlState(false);
            if (!bSilent)
                MessageBox.Show("This file or folder cannot be used in OneClick mode.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public void setBatchProcessing(List<OneClickFilesToProcess> arrFilesToProcess, OneClickSettings oSettings)
        {
            bBatchProcessing = bAutomatedProcessing = true;
            SetOneClickProfile(oSettings);
            OneClickProcessing oProcessor = new OneClickProcessing(this, arrFilesToProcess, oSettings, _oLog);
            return;
        }

        public void setInputData(MediaInfoFile iFile, List<OneClickFilesToProcess> arrFilesToProcess)
        {
            if (iFile == null)
                return;

            beingCalled++;
            if (!bAutomatedProcessing && arrFilesToProcess.Count > 0)
            {
                string question = "Do you want to process all " + (arrFilesToProcess.Count + 1) + " files/tracks in the selection?\r\nThey all will be processed with the current settings\r\nin the OneClick profile \"" + oneclickProfile.SelectedProfile.Name + "\".\r\nOther settings will be ignored.";
                DialogResult dr = MessageBox.Show(question, "Automated folder processing", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == System.Windows.Forms.DialogResult.Yes)
                {
                    bAutomatedProcessing = true;
                    SetOneClickProfile((OneClickSettings)oneclickProfile.SelectedProfile.BaseSettings);
                    _oSettings.LeadingName = MeGUI.core.gui.InputBox.Show("If desired please enter a leading name", "Please enter a leading name", _oSettings.LeadingName);
                }
            }
            bLock = true;
            if (input.SelectedSCItem == null || !iFile.FileName.Equals((string)input.SelectedObject))
            {
                input.StandardItems = new object[] { iFile.FileName };
                input.SelectedIndex = 0; 
            }
            bLock = false;

            _videoInputInfo = iFile;
            inputName.Text = iFile.VideoInfo.Track.Name;

            List<OneClickStream> arrAudioTrackInfo = new List<OneClickStream>();
            foreach (AudioTrackInfo oInfo in iFile.AudioInfo.Tracks)
                arrAudioTrackInfo.Add(new OneClickStream(oInfo));
            AudioResetTrack(arrAudioTrackInfo, _oSettings);

            List<OneClickStream> arrSubtitleTrackInfo = new List<OneClickStream>();
            foreach (SubtitleTrackInfo oInfo in iFile.SubtitleInfo.Tracks)
                arrSubtitleTrackInfo.Add(new OneClickStream(oInfo));
            SubtitleResetTrack(arrSubtitleTrackInfo, _oSettings);

            beingCalled--;
            updatePossibleContainers();
            
            // Detect Chapters
            if (!VideoUtil.HasChapters(iFile))
            {
                string chapterFile = VideoUtil.getChapterFile(iFile.FileName);
                if (File.Exists(chapterFile))
                    this.chapterFile.Filename = chapterFile;
                if (!File.Exists(this.chapterFile.Filename))
                    this.chapterFile.Filename = String.Empty;
            }
            else
                this.chapterFile.Filename = "<internal chapters>";

            if (String.IsNullOrEmpty(workingDirectory.Filename))
            {
                string strPath = Path.GetDirectoryName(iFile.FileName);
                if (Directory.Exists(strPath) && FileUtil.IsDirWriteable(strPath))
                    workingDirectory.Filename = strPath;
            }

            updateFilename(false, true, true);

            if (_oSettings.DAR.HasValue)
                this.ar.Value = _oSettings.DAR;

            if (VideoSettings.EncoderType.ID == "x264" && this.chapterFile.Filename != null)
                this.usechaptersmarks.Enabled = true;

            if (bAutomatedProcessing)
                createOneClickJob(arrFilesToProcess);

            setControlState(false);
        }

        private int beingCalled = 0;
        private void updatePossibleContainers()
        {
            // Since everything calls everything else, this is just a safeguard to make sure we don't infinitely recurse
            if (beingCalled > 0)
                return;
            beingCalled = 1;

            List<AudioEncoderType> audioCodecs = new List<AudioEncoderType>();
            List<MuxableType> dictatedOutputTypes = new List<MuxableType>();

            if (audioTracks != null)
            {
                for (int i = 0; i < audioTracks.Count; ++i)
                {
                    if (audioTracks[i].SelectedStreamIndex <= 0) // "None"
                        continue;

                    if (audioTracks[i].SelectedStream.EncoderSettings != null 
                        && (audioTracks[i].SelectedStream.EncodingMode == AudioEncodingMode.IfCodecDoesNotMatch
                        || audioTracks[i].SelectedStream.EncodingMode == AudioEncodingMode.Always))
                        audioCodecs.Add(audioTracks[i].SelectedStream.EncoderSettings.EncoderType);
                    else if (audioTracks[i].SelectedStream.EncodingMode == AudioEncodingMode.Never 
                        || audioTracks[i].SelectedStream.EncodingMode == AudioEncodingMode.NeverOnlyCore)
                    {
                        string typeString;
                        if (audioTracks[i].SelectedItem.IsStandard)
                            typeString = audioTracks[i].SelectedStream.TrackInfo.DemuxFileName;
                        else
                            typeString = audioTracks[i].SelectedFile;

                        if (VideoUtil.guessAudioType(typeString) != null)
                            dictatedOutputTypes.Add(VideoUtil.guessAudioMuxableType(typeString, false));
                    }
                }
            }

            if (subtitleTracks != null)
            {
                for (int i = 0; i < subtitleTracks.Count; ++i)
                {
                    if (subtitleTracks[i].SelectedStreamIndex <= 0) // "None"
                        continue;

                    string typeString;
                    if (subtitleTracks[i].SelectedItem.IsStandard)
                        typeString = subtitleTracks[i].SelectedStream.TrackInfo.DemuxFileName;
                    else
                        typeString = subtitleTracks[i].SelectedFile;

                    SubtitleType subtitleType = VideoUtil.guessSubtitleType(typeString);
                    if (subtitleType != null)
                        dictatedOutputTypes.Add((new MuxableType(subtitleType, null)));
                }
            }

            List<ContainerType> tempSupportedOutputTypes = new List<ContainerType>();
            tempSupportedOutputTypes = this.muxProvider.GetSupportedContainers(VideoSettings.EncoderType, audioCodecs.ToArray(), dictatedOutputTypes.ToArray());

            List<ContainerType> supportedOutputTypes = new List<ContainerType>();
            foreach (ContainerType c in acceptableContainerTypes)
                if (tempSupportedOutputTypes.Contains(c))
                    supportedOutputTypes.Add(c);

            ignoreRestrictions = false;
            if (supportedOutputTypes.Count == 0)
            {
                if (tempSupportedOutputTypes.Count > 0 && !ignoreRestrictions)
                {
                    if (!bAutomatedProcessing)
                    {
                        string message = string.Format(
                        "No container type could be found that matches the list of acceptable types " +
                        "in your chosen one click profile. {0}" +
                        "Your restrictions are now being ignored.", Environment.NewLine);
                        MessageBox.Show(message, "Filetype restrictions too restrictive", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    ignoreRestrictions = true;
                }
                if (ignoreRestrictions)
                    supportedOutputTypes = tempSupportedOutputTypes;
                if (tempSupportedOutputTypes.Count == 0)
                {
                    if (bAutomatedProcessing)
                        ignoreRestrictions = true;
                    else
                        MessageBox.Show("No container type could be found for your current settings.\nPlease modify the codecs you use.", "No container found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            if (supportedOutputTypes.Count > 0)
            {
                ContainerType cType = (ContainerType)this.containerFormat.SelectedItem;
                this.containerFormat.SelectedIndexChanged -= new System.EventHandler(this.containerFormat_SelectedIndexChanged);
                this.containerFormat.Items.Clear();
                this.containerFormat.Items.AddRange(supportedOutputTypes.ToArray());
                bool bFound = false;
                foreach (ContainerType selType in this.containerFormat.Items)
                {
                    if (selType == cType)
                    {
                        this.containerFormat.SelectedItem = cType;
                        bFound = true;
                        break;
                    }
                }
                this.containerFormat.SelectedIndexChanged += new System.EventHandler(this.containerFormat_SelectedIndexChanged);
                if (!bFound)
                    this.containerFormat.SelectedIndex = 0;
            }
            beingCalled = 0;
        }

        private void SetOneClickProfile(OneClickSettings settings)
        {
            _oSettings = settings.Clone();

            if (_videoInputInfo != null)
            {
                List<OneClickStream> arrAudioTrackInfo = new List<OneClickStream>();
                foreach (AudioTrackInfo oInfo in _videoInputInfo.AudioInfo.Tracks)
                    arrAudioTrackInfo.Add(new OneClickStream(oInfo));
                AudioResetTrack(arrAudioTrackInfo, settings);

                List<OneClickStream> arrSubtitleTrackInfo = new List<OneClickStream>();
                foreach (SubtitleTrackInfo oInfo in _videoInputInfo.SubtitleInfo.Tracks)
                    arrSubtitleTrackInfo.Add(new OneClickStream(oInfo));
                SubtitleResetTrack(arrSubtitleTrackInfo, settings);
            }
            else
                ResetAudioSettings(settings);

            videoProfile.SetProfileNameOrWarn(settings.VideoProfileName);
            avsProfile.SetProfileNameOrWarn(settings.AvsProfileName);

            List<ContainerType> temp = new List<ContainerType>();
            List<ContainerType> allContainerTypes = muxProvider.GetSupportedContainers();
            foreach (string s in settings.ContainerCandidates)
            {
                ContainerType ct = allContainerTypes.Find(new Predicate<ContainerType>(delegate(ContainerType t) { return t.ToString() == s; }));
                if (ct != null)
                    temp.Add(ct);
            }
            acceptableContainerTypes = temp.ToArray();

            // bools
            chkDontEncodeVideo.Checked = settings.DontEncodeVideo;
            autoDeint.Checked = settings.AutomaticDeinterlacing;
            autoCrop.Checked = settings.AutoCrop;
            keepInputResolution.Checked = settings.KeepInputResolution;
            addPrerenderJob.Checked = settings.PrerenderVideo;
            usechaptersmarks.Checked = settings.UseChaptersMarks;

            splitting.Value = settings.SplitSize;
            fileSize.Value = settings.Filesize;
            horizontalResolution.Value = settings.OutputResolution;
            if (Directory.Exists(settings.DefaultWorkingDirectory) && FileUtil.IsDirWriteable(settings.DefaultWorkingDirectory))
                workingDirectory.Filename = settings.DefaultWorkingDirectory;
            else
                workingDirectory.Filename = String.Empty;

            // device type
            devicetype.Text = settings.DeviceOutputType;

            // clean up after those settings were set
            updatePossibleContainers();

            updateFilename(true, true, true);

            if (settings.DAR != null)
                ar.Value = settings.DAR.Value;
            else
                ar.Value = null;
        }

        private void goButton_Click(object sender, EventArgs e)
        {
            setControlState(true);
            createOneClickJob(null);
        }

        private void createOneClickJob(List<OneClickFilesToProcess> arrFilesToProcess)
        {
            // checks if there is a suitable container
            if (ignoreRestrictions && bAutomatedProcessing)
            {
                _oLog.LogEvent(_videoInputInfo.FileName + ": No container type could be found that matches the list of acceptable types in your chosen one click profile. Skipping...");
                if (arrFilesToProcess != null)
                    setBatchProcessing(arrFilesToProcess, _oSettings);
                return;
            }

            // set random working directory
            string strWorkingDirectory = string.Empty;
            if (Directory.Exists(workingDirectory.Filename) && FileUtil.IsDirWriteable(workingDirectory.Filename))
                strWorkingDirectory = workingDirectory.Filename;
            else if (File.Exists(output.Filename))
                strWorkingDirectory = Path.GetDirectoryName(output.Filename);
            else if (_videoInputInfo != null && File.Exists(_videoInputInfo.FileName))
                strWorkingDirectory = Path.GetDirectoryName(_videoInputInfo.FileName);
            if (!String.IsNullOrEmpty(strWorkingDirectory))
            {
                do
                    strWorkingDirectory = Path.Combine(strWorkingDirectory, Path.GetRandomFileName());
                while (Directory.Exists(strWorkingDirectory));
            }

            if (!verifyInputSettings(_videoInputInfo, strWorkingDirectory))
            {
                setControlState(false);
                return;
            }

            ContainerType inputContainer = _videoInputInfo.ContainerFileType;
            JobChain prepareJobs = null;

            // set initial oneclick job settings
            OneClickPostprocessingProperties dpp = new OneClickPostprocessingProperties();
            if (ar.Value.HasValue)
                dpp.DAR = ar.Value;
            else
                dpp.DAR = _videoInputInfo.VideoInfo.DAR;
            dpp.ForcedDAR = ar.Value;
            dpp.AvsSettings = (AviSynthSettings)avsProfile.SelectedProfile.BaseSettings.Clone();
            dpp.Container = (ContainerType)containerFormat.SelectedItem;
            dpp.FinalOutput = output.Filename;
            dpp.DeviceOutputType = devicetype.Text;
            dpp.VideoSettings = VideoSettings.Clone();
            dpp.VideoSettings.VideoName = inputName.Text;
            dpp.Splitting = splitting.Value;
            dpp.VideoInput = _videoInputInfo.FileName;
            dpp.IndexType = _videoInputInfo.IndexerToUse;
            dpp.TitleNumberToProcess = _videoInputInfo.VideoInfo.PGCNumber;
            if (arrFilesToProcess != null)
            {
                dpp.FilesToProcess = arrFilesToProcess;
                dpp.OneClickSetting = _oSettings.Clone();
            }
            dpp.WorkingDirectory = strWorkingDirectory;
            dpp.FilesToDelete.Add(dpp.WorkingDirectory);
            dpp.AutoCrop = autoCrop.Checked;

            bool muxVideo = chkDontEncodeVideo.Checked;
            // check if muxing of video input is supported
            if (muxVideo && _videoInputInfo != null)
            {
                MuxerProvider muxer = new MKVMergeMuxerProvider();
                if (dpp.Container == ContainerType.AVI)
                    muxer = new AVIMuxGUIMuxerProvider();
                else if (dpp.Container == ContainerType.M2TS)
                    muxer = new TSMuxerProvider();
                else if (dpp.Container == ContainerType.MP4)
                    muxer = new MP4BoxMuxerProvider();

                VideoCodec vCodec = _videoInputInfo.VideoInfo.Codec;
                if (!muxer.SupportsVideoCodec(vCodec))
                {
                    muxVideo = false;
                    _oLog.LogEvent("The video file cannot be muxed as the codec " + vCodec.ID + " is not supported for the container " + dpp.Container.ID, ImageType.Warning);
                }
            }

            // prepare input file
            if (Path.GetExtension(_videoInputInfo.FileName.ToUpperInvariant()) == ".VOB")
            {
                string videoIFO;
                // PGC numbers are not present in VOB, so we check the main IFO
                if (Path.GetFileName(_videoInputInfo.FileName).ToUpperInvariant().Substring(0, 4) == "VTS_")
                    videoIFO = _videoInputInfo.FileName.Substring(0, _videoInputInfo.FileName.LastIndexOf("_")) + "_0.IFO";
                else
                    videoIFO = Path.ChangeExtension(_videoInputInfo.FileName, ".IFO");

                if (File.Exists(videoIFO))
                {
                    dpp.IFOInput = videoIFO;
                    if (_videoInputInfo.VideoInfo.PGCCount > 1 
                        || IFOparser.GetAngleCount(videoIFO, _videoInputInfo.VideoInfo.PGCNumber) > 0)
                    {
                        // pgcdemux must be used as either multiple PGCs or a multi-angle disc are found
                        prepareJobs = new SequentialChain(new PgcDemuxJob(videoIFO, Path.Combine(dpp.WorkingDirectory, "VTS_01_1.VOB"), _videoInputInfo.VideoInfo.PGCNumber, _videoInputInfo.VideoInfo.AngleNumber));
                        for (int i = 1; i < 10; i++)
                            dpp.FilesToDelete.Add(Path.Combine(dpp.WorkingDirectory, "VTS_01_" + i + ".VOB"));
                        dpp.VideoInput = Path.Combine(dpp.WorkingDirectory, "VTS_01_1.VOB");
                        dpp.ApplyDelayCorrection = true;
                    }
                }
            }

            bool bRemuxInputToMKV = false;
            dpp.IntermediateMKVFile = String.Empty;
            if (_videoInputInfo.isEac3toDemuxable())
            {
                // create eac3to demux job if needed - for blu-ray playlists
                dpp.Eac3toDemux = true;
                StringBuilder sb = new StringBuilder();

                dpp.VideoInput = Path.Combine(dpp.WorkingDirectory, Path.GetFileNameWithoutExtension(_videoInputInfo.FileName) + ".mkv");
                sb.Append(string.Format("{0}:\"{1}\" ", _videoInputInfo.VideoInfo.Track.TrackID, dpp.VideoInput));
                inputContainer = ContainerType.MKV;
                dpp.FilesToDelete.Add(dpp.VideoInput);

                foreach (OneClickStreamControl oStreamControl in audioTracks)
                {
                    if (!oStreamControl.SelectedItem.IsStandard)
                        continue;

                    if (oStreamControl.SelectedStreamIndex <= 0) // not NONE
                        continue;

                    bool bCoreOnly = false;
                    AudioCodec audioCodec = ((AudioTrackInfo)oStreamControl.SelectedStream.TrackInfo).AudioCodec;
                    if (oStreamControl.SelectedStream.EncodingMode != AudioEncodingMode.Never 
                        && ((AudioTrackInfo)oStreamControl.SelectedStream.TrackInfo).HasCore)
                    {
                        // audio file can be demuxed && should be touched (core needed)
                        if (audioCodec == AudioCodec.THDAC3)
                        {
                            // if FLAC is used as encoder it cannot deal with THDAC3 so either THD or AC3 has to be used
                            if (oStreamControl.SelectedStream.EncoderSettings is FlacSettings
                                && oStreamControl.SelectedStream.EncoderSettings.DownmixMode == ChannelMode.KeepOriginal
                                && oStreamControl.SelectedStream.EncodingMode != AudioEncodingMode.NeverOnlyCore)
                            {
                                oStreamControl.SelectedStream.TrackInfo.Codec = "TrueHD";
                                ((AudioTrackInfo)oStreamControl.SelectedStream.TrackInfo).AudioType = AudioType.THD;
                                ((AudioTrackInfo)oStreamControl.SelectedStream.TrackInfo).AudioCodec = AudioCodec.THD;
                            }
                            else
                            {
                                oStreamControl.SelectedStream.TrackInfo.Codec = "AC-3";
                                ((AudioTrackInfo)oStreamControl.SelectedStream.TrackInfo).AudioType = AudioType.AC3;
                                ((AudioTrackInfo)oStreamControl.SelectedStream.TrackInfo).AudioCodec = AudioCodec.AC3;
                                bCoreOnly = true;
                            }
                            ((AudioTrackInfo)oStreamControl.SelectedStream.TrackInfo).HasCore = false;
                        }
                        else if (audioCodec == AudioCodec.DTS)
                        {
                            // do not etraxt only the core if FLAC is used as encoder
                            if (!(oStreamControl.SelectedStream.EncoderSettings is FlacSettings)
                                || oStreamControl.SelectedStream.EncodingMode == AudioEncodingMode.NeverOnlyCore
                                || oStreamControl.SelectedStream.EncoderSettings.DownmixMode != ChannelMode.KeepOriginal)
                            {
                                oStreamControl.SelectedStream.TrackInfo.Codec = "DTS";
                                ((AudioTrackInfo)oStreamControl.SelectedStream.TrackInfo).AudioType = AudioType.DTS;
                                ((AudioTrackInfo)oStreamControl.SelectedStream.TrackInfo).AudioCodec = AudioCodec.DTS;
                                ((AudioTrackInfo)oStreamControl.SelectedStream.TrackInfo).HasCore = false;
                                bCoreOnly = true;
                            }
                        }
                    }

                    // core must be extracted (workaround for an eac3to issue)
                    // http://bugs.madshi.net/view.php?id=450
                    if (audioCodec == AudioCodec.EAC3)
                        bCoreOnly = true;

                    OneClickStream oStream = oStreamControl.SelectedStream.Clone();
                    sb.Append(string.Format("{0}:\"{1}\" ", oStream.TrackInfo.TrackID,
                        Path.Combine(dpp.WorkingDirectory, oStream.TrackInfo.DemuxFileName)));
                    if (bCoreOnly)
                        sb.Append("-core ");

                    dpp.FilesToDelete.Add(Path.Combine(dpp.WorkingDirectory, oStream.TrackInfo.DemuxFileName));
                }

                foreach (OneClickStreamControl oStreamControl in subtitleTracks)
                {
                    if (!oStreamControl.SelectedItem.IsStandard)
                        continue;

                    if (oStreamControl.SelectedStreamIndex <= 0) // not NONE
                        continue;

                    string strDemuxFilePath = Path.Combine(dpp.WorkingDirectory, oStreamControl.SelectedStream.TrackInfo.DemuxFileName);
                    sb.Append(string.Format("{0}:\"{1}\" ", oStreamControl.SelectedStream.TrackInfo.MMGTrackID, strDemuxFilePath));
                    oStreamControl.SelectedStream.DemuxFilePath = strDemuxFilePath;
                    dpp.FilesToDelete.Add(strDemuxFilePath);
                    dpp.SubtitleTracks.Add(oStreamControl.SelectedStream.Clone());
                }
                
                if (sb.Length != 0)
                    prepareJobs = new SequentialChain(prepareJobs, new HDStreamsExJob(new List<string>() { _videoInputInfo.FileName }, dpp.WorkingDirectory, null, sb.ToString(), 2));
            }
            else if (inputContainer != ContainerType.MKV)
            {
                // mux input file into MKV if possible and necessary
                if (muxVideo)
                    bRemuxInputToMKV = true;

                // remux needed to support timecodes
                if (_videoInputInfo.VideoInfo.VariableFrameRateMode)
                    bRemuxInputToMKV = true;

                if (!bRemuxInputToMKV && 
                    (dpp.IndexType == FileIndexerWindow.IndexType.FFMS ||
                    dpp.IndexType == FileIndexerWindow.IndexType.AVISOURCE ||
                    dpp.IndexType == FileIndexerWindow.IndexType.LSMASH))
                {
                    foreach (OneClickStreamControl oStreamControl in audioTracks)
                    {
                        if (!oStreamControl.SelectedItem.IsStandard)
                            continue;

                        if (oStreamControl.SelectedStreamIndex <= 0) // not NONE
                            continue;

                        bRemuxInputToMKV = true;
                        break;
                    }
                }

                if (!bRemuxInputToMKV && Path.GetExtension(_videoInputInfo.FileName.ToUpperInvariant()) != ".VOB")
                {
                    foreach (OneClickStreamControl oStreamControl in subtitleTracks)
                    {
                        if (!oStreamControl.SelectedItem.IsStandard)
                            continue;

                        if (oStreamControl.SelectedStreamIndex <= 0) // not NONE
                            continue;

                        bRemuxInputToMKV = true;
                        break;
                    }
                }

                if (bRemuxInputToMKV)
                {
                    if (!_oSettings.DisableIntermediateMKV && _videoInputInfo.MuxableToMKV())
                    {
                        // create job
                        MuxJob mJob = new MuxJob();
                        mJob.MuxType = MuxerType.MKVMERGE;
                        mJob.Input = dpp.VideoInput;
                        mJob.Output = Path.Combine(dpp.WorkingDirectory, Path.GetFileNameWithoutExtension(dpp.VideoInput) + ".mkv"); ;
                        mJob.Settings.MuxAll = true;
                        mJob.Settings.MuxedInput = mJob.Input;
                        mJob.Settings.MuxedOutput = mJob.Output;
                        dpp.FilesToDelete.Add(mJob.Output);

                        // change input file properties
                        if (dpp.IndexType == FileIndexerWindow.IndexType.FFMS ||
                            dpp.IndexType == FileIndexerWindow.IndexType.LSMASH ||
                            dpp.IndexType == FileIndexerWindow.IndexType.AVISOURCE)
                        {
                            inputContainer = ContainerType.MKV;
                            dpp.IntermediateMKVFile = dpp.VideoInput = mJob.Output;
                        }
                        else
                            dpp.IntermediateMKVFile = mJob.Output;

                        // add job to queue
                        prepareJobs = new SequentialChain(prepareJobs, mJob);
                    }
                    else
                    {
                        if (_oSettings.DisableIntermediateMKV)
                            _oLog.LogEvent("Input file will not be demuxed as intermediate MKV is disabled. Therefore some OneClick features will be disabled.");
                        else
                            _oLog.LogEvent("Input file cannot not be muxed into an intermediate MKV as this is not supported. Therefore some OneClick features will be disabled.");
                    }
                }
            }

            // chapter handling
            if (!String.IsNullOrEmpty(chapterFile.Filename))
            {
                if (dpp.Container == ContainerType.AVI || (dpp.Container == ContainerType.M2TS && dpp.DeviceOutputType.Equals("Standard")))
                {
                    _oLog.LogEvent("Chapter handling disabled because of the selected target container");
                    dpp.ChapterInfo = new ChapterInfo();
                }
                else if (!chapterFile.Filename.StartsWith("<"))
                {
                    // external file selected
                    if (!dpp.ChapterInfo.LoadFile(chapterFile.Filename))
                        _oLog.LogEvent("Chapter file cannot be imported: " + chapterFile.Filename, ImageType.Warning);
                    else if (dpp.ChapterInfo.FramesPerSecond == 0)
                    {
                        dpp.ChapterInfo.FramesPerSecond = _videoInputInfo.VideoInfo.FPS;
                    }
                }
                else
                {
                    // internal chapter processing
                    if (File.Exists(dpp.IFOInput))
                    {
                        // IFO/DVD chapters
                        ChapterInfo pgc;
                        IfoExtractor ex = new IfoExtractor();
                        pgc = ex.GetChapterInfo(dpp.IFOInput, dpp.TitleNumberToProcess);
                        if (pgc != null)
                            dpp.ChapterInfo = pgc;
                    }
                    else
                        dpp.ChapterInfo = _videoInputInfo.ChapterInfo;
                }
            }

            // set video mux handling
            if (muxVideo)
            {
                if (!String.IsNullOrEmpty(dpp.IntermediateMKVFile))
                    dpp.VideoFileToMux = dpp.IntermediateMKVFile;
                else if (inputContainer != ContainerType.MKV)
                    _oLog.LogEvent("\"Don't encode video\" has been disabled as it is not supported");
                else
                    dpp.VideoFileToMux = dpp.VideoInput;
            }

            // set oneclick job settings
            if (String.IsNullOrEmpty(dpp.VideoFileToMux))
            {
                dpp.AutoDeinterlace = autoDeint.Checked;
                dpp.KeepInputResolution = keepInputResolution.Checked;
                dpp.OutputSize = fileSize.Value;
                dpp.PrerenderJob = addPrerenderJob.Checked;
                dpp.UseChaptersMarks = usechaptersmarks.Checked;
            }
            else
            {
                dpp.AutoDeinterlace = dpp.PrerenderJob = dpp.UseChaptersMarks = false;
                dpp.KeepInputResolution = false;
                dpp.OutputSize = null;
            }

            if (keepInputResolution.Checked || !String.IsNullOrEmpty(dpp.VideoFileToMux))
                dpp.HorizontalOutputResolution = 0;
            else
                dpp.HorizontalOutputResolution = (int)horizontalResolution.Value;

            // MKV tracks which need to be extracted
            List<TrackInfo> oExtractMKVTrack = new List<TrackInfo>();

            // audio handling
            JobChain audioJobs = null;
            List<AudioTrackInfo> arrAudioTrackInfo = new List<AudioTrackInfo>();
            int iTrueHDOffset = 0;
            foreach (OneClickStreamControl oStreamControl in audioTracks)
            {
                if (oStreamControl.SelectedStreamIndex <= 0) // "None"
                    continue;

                string aInput = null;
                string strLanguage = null;
                string strName = null;
                bool bExtractMKVTrack = false;
                AudioTrackInfo oAudioTrackInfo = null;
                OneClickStream oStream = oStreamControl.SelectedStream.Clone();

                int delay = oStream.Delay;
                if (oStreamControl.SelectedItem.IsStandard)
                {
                    oAudioTrackInfo = (AudioTrackInfo)oStream.TrackInfo;
                    if (dpp.IndexType == FileIndexerWindow.IndexType.AVISOURCE && inputContainer != ContainerType.MKV)
                    {
                        _oLog.LogEvent("Internal audio track " + oAudioTrackInfo.TrackID + " will be skipped as AVISOURCE is going to be used", ImageType.Warning);
                        continue;
                    }

                    if (bRemuxInputToMKV)
                    {
                        // if a remux to MKV will happen, the THDAC3 track will be splitted:
                        // https://github.com/mbunkus/mkvtoolnix/wiki/TrueHD-and-AC-3
                        if (oAudioTrackInfo.AudioCodec == AudioCodec.THDAC3)
                            iTrueHDOffset++;
                        oStream.TrackInfo.MMGTrackID += iTrueHDOffset;
                    }

                    arrAudioTrackInfo.Add(oAudioTrackInfo);
                    if (dpp.IndexType != FileIndexerWindow.IndexType.NONE && !dpp.Eac3toDemux)
                        aInput = "::" + oAudioTrackInfo.TrackID + "::";
                    else
                        aInput = Path.Combine(dpp.WorkingDirectory, oAudioTrackInfo.DemuxFileName);
                    strName = oAudioTrackInfo.Name;
                    strLanguage = oAudioTrackInfo.Language;
                    if (inputContainer == ContainerType.MKV && !dpp.Eac3toDemux) // only if container MKV and no demux with eac3to
                    {
                        if (oAudioTrackInfo.AudioCodec != AudioCodec.PCM) // some PCM tracks cannot be extracted by mkvextract
                        {
                            // if TrueHD has to be extracted from a TrueHD + AC3 track within a MKV, iTrueHDOffset - 1 has to be used for the track ID
                            if (oAudioTrackInfo.AudioCodec == AudioCodec.THDAC3)
                            {
                                if (oStream.EncodingMode == AudioEncodingMode.Never
                                || (oStream.EncoderSettings is FlacSettings
                                    && oStream.EncoderSettings.DownmixMode == ChannelMode.KeepOriginal
                                    && oStream.EncodingMode != AudioEncodingMode.NeverOnlyCore))
                                {
                                    oAudioTrackInfo.MMGTrackID = oStream.TrackInfo.MMGTrackID - 1;
                                    oAudioTrackInfo.Codec = "TrueHD";
                                    oAudioTrackInfo.AudioCodec = AudioCodec.THD;
                                    oAudioTrackInfo.AudioType = AudioType.THD;
                                }
                                else
                                {
                                    oAudioTrackInfo.Codec = "AC-3";
                                    oAudioTrackInfo.AudioCodec = AudioCodec.AC3;
                                    oAudioTrackInfo.AudioType = AudioType.AC3; 
                                }
                                oAudioTrackInfo.HasCore = false;
                            }

                            oExtractMKVTrack.Add(oStream.TrackInfo.Clone());
                            bExtractMKVTrack = true;
                        }
                    }
                }
                else
                {
                    strName = oStream.Name;
                    strLanguage = oStream.Language;
                    aInput = oStreamControl.SelectedFile;
                    MediaInfoFile oInfo = new MediaInfoFile(aInput, ref _oLog);
                    if (oInfo.AudioInfo.Tracks.Count > 0)
                    {
                        oAudioTrackInfo = oInfo.AudioInfo.Tracks[0];
                        oStream.TrackInfo = oAudioTrackInfo;
                        oStream.Language = strLanguage;
                        oStream.Name = strName;
                    }
                }

                bool bIsDontEncodeAudioPossible = isDontEncodeAudioPossible(_videoInputInfo, oStreamControl.SelectedItem.IsStandard, inputContainer);
                if (bIsDontEncodeAudioPossible &&
                    (oStream.EncodingMode == AudioEncodingMode.Never ||
                    (oStream.EncodingMode == AudioEncodingMode.NeverOnlyCore && dpp.Eac3toDemux) ||
                    (oStream.EncodingMode == AudioEncodingMode.IfCodecDoesNotMatch &&
                    oStream.EncoderSettings.EncoderType.ACodec.ID.Equals(oAudioTrackInfo.AudioCodec.ID))))
                {
                    if (oStreamControl.SelectedItem.IsStandard && inputContainer == ContainerType.MKV 
                        && oAudioTrackInfo.AudioCodec == AudioCodec.PCM && !dpp.Eac3toDemux)
                    {
                        int trackID = oStream.TrackInfo.TrackID;
                        if (_videoInputInfo.ContainerFileType != ContainerType.MKV)
                            trackID++;
                        aInput = Path.Combine(strWorkingDirectory, oStream.TrackInfo.DemuxFileName);
                        HDStreamsExJob oJob = new HDStreamsExJob(new List<string>() { dpp.VideoInput }, aInput, null, trackID + ":\"" + aInput + "\"", 2);
                        audioJobs = new SequentialChain(audioJobs, new SequentialChain(oJob));
                        dpp.FilesToDelete.Add(FileUtil.AddToFileName(Path.ChangeExtension(aInput, "txt"), " - Log"));
                        dpp.FilesToDelete.Add(aInput);
                    }
                    dpp.AudioTracks.Add(new OneClickAudioTrack(null, new MuxStream(aInput, strLanguage, strName, delay, false, false, null), oStreamControl.SelectedItem.IsStandard ? oAudioTrackInfo : null, bExtractMKVTrack));
                }
                else
                {
                    if (!bIsDontEncodeAudioPossible &&
                        (oStream.EncodingMode == AudioEncodingMode.Never ||
                        (oStream.EncodingMode == AudioEncodingMode.NeverOnlyCore && dpp.Eac3toDemux) ||
                        (oStream.EncodingMode == AudioEncodingMode.IfCodecDoesNotMatch &&
                         oStream.EncoderSettings.EncoderType.ACodec.ID.Equals(oAudioTrackInfo.AudioCodec.ID))))
                        _oLog.LogEvent("Audio " + oStream + " cannot be processed with encoding mode \"" + oStream.EncodingMode + "\" as it must be encoded", ImageType.Warning);

                    // audio track will be encoded
                    string strFileName = string.Empty;
                    if ((!oStreamControl.SelectedItem.IsStandard || !dpp.Eac3toDemux) 
                        && (inputContainer == ContainerType.MKV || (dpp.IndexType != FileIndexerWindow.IndexType.D2V 
                            && dpp.IndexType != FileIndexerWindow.IndexType.DGI && dpp.IndexType != FileIndexerWindow.IndexType.DGM)))
                    {
                        // only process if
                        // - no DG* indexer is used or DG* indexer is used for a MKV file
                        // - no default/included stream or not demuxed with eac3to

                        if (oAudioTrackInfo.HasCore && oStream.EncodingMode != AudioEncodingMode.Never)
                        {
                            if (oAudioTrackInfo.AudioCodec == AudioCodec.THDAC3)
                            {
                                // if FLAC is used as encoder it cannot deal with THDAC3 so either THD or AC3 has to be used
                                if (oStream.EncoderSettings is FlacSettings
                                    && oStream.EncoderSettings.DownmixMode == ChannelMode.KeepOriginal
                                    && oStream.EncodingMode != AudioEncodingMode.NeverOnlyCore)
                                {
                                    oAudioTrackInfo.Codec = "TrueHD";
                                    oAudioTrackInfo.AudioCodec = AudioCodec.THD;
                                    oAudioTrackInfo.AudioType = AudioType.THD;
                                }
                                else
                                {
                                    oAudioTrackInfo.Codec = "AC-3";
                                    oAudioTrackInfo.AudioCodec = AudioCodec.AC3;
                                    oAudioTrackInfo.AudioType = AudioType.AC3;
                                }
                                oAudioTrackInfo.HasCore = false;

                                if (oStreamControl.SelectedItem.IsStandard)
                                {
                                    strFileName = Path.Combine(strWorkingDirectory, oStream.TrackInfo.DemuxFileName);
                                    aInput = FileUtil.AddToFileName(strFileName, "_core");
                                }
                                else
                                {
                                    strFileName = oStreamControl.SelectedFile;
                                    aInput = FileUtil.AddToFileName(Path.ChangeExtension(strFileName, oAudioTrackInfo.AudioType.Extension), "_core");
                                    aInput = Path.Combine(strWorkingDirectory, Path.GetFileName(aInput));
                                }

                                HDStreamsExJob oJob = new HDStreamsExJob(new List<string>() { strFileName }, aInput, null, "\"" + aInput + "\"", 2);
                                audioJobs = new SequentialChain(audioJobs, new SequentialChain(oJob));
                                dpp.FilesToDelete.Add(FileUtil.AddToFileName(Path.ChangeExtension(aInput, "txt"), " - Log"));
                                dpp.FilesToDelete.Add(aInput);
                            }
                            else if (oAudioTrackInfo.AudioCodec == AudioCodec.DTS)
                            {
                                // do not etraxt only the core if FLAC is used as encoder
                                if (!(oStreamControl.SelectedStream.EncoderSettings is FlacSettings)
                                    || oStreamControl.SelectedStream.EncodingMode == AudioEncodingMode.NeverOnlyCore
                                    || oStreamControl.SelectedStream.EncoderSettings.DownmixMode != ChannelMode.KeepOriginal)
                                {
                                    if (oStreamControl.SelectedItem.IsStandard)
                                    {
                                        strFileName = Path.Combine(strWorkingDirectory, oStream.TrackInfo.DemuxFileName);
                                        aInput = FileUtil.AddToFileName(Path.ChangeExtension(strFileName, "dts"), "_core");
                                    }
                                    else
                                    {
                                        strFileName = oStreamControl.SelectedFile;
                                        aInput = FileUtil.AddToFileName(Path.ChangeExtension(strFileName, "dts"), "_core");
                                        aInput = Path.Combine(strWorkingDirectory, Path.GetFileName(aInput));
                                    }
                                    oAudioTrackInfo.HasCore = false;

                                    HDStreamsExJob oJob = new HDStreamsExJob(new List<string>() { strFileName }, aInput, null, "\"" + aInput + "\" -core", 2);
                                    audioJobs = new SequentialChain(audioJobs, new SequentialChain(oJob));
                                    dpp.FilesToDelete.Add(FileUtil.AddToFileName(Path.ChangeExtension(aInput, "txt"), " - Log"));
                                    dpp.FilesToDelete.Add(aInput);
                                }
                            }
                        }

                        if (oAudioTrackInfo.AudioCodec == AudioCodec.EAC3)
                        {
                            if (oStreamControl.SelectedItem.IsStandard)
                            {
                                strFileName = Path.Combine(strWorkingDirectory, oStream.TrackInfo.DemuxFileName);
                                aInput = FileUtil.AddToFileName(strFileName, "_core");
                            }
                            else
                            {
                                strFileName = oStreamControl.SelectedFile;
                                aInput = FileUtil.AddToFileName(strFileName, "_core");
                                aInput = Path.Combine(strWorkingDirectory, Path.GetFileName(aInput));
                            }

                            if (oAudioTrackInfo.HasCore)
                            {
                                oAudioTrackInfo.Codec = "E-AC-3";
                                oAudioTrackInfo.AudioCodec = AudioCodec.AC3;
                                oAudioTrackInfo.AudioType = AudioType.AC3;
                            }
                            oAudioTrackInfo.HasCore = false;

                            HDStreamsExJob oJob = new HDStreamsExJob(new List<string>() { strFileName }, aInput, null, "\"" + aInput + "\" -core", 2);
                            audioJobs = new SequentialChain(audioJobs, new SequentialChain(oJob));
                            dpp.FilesToDelete.Add(FileUtil.AddToFileName(Path.ChangeExtension(aInput, "txt"), " - Log"));
                            dpp.FilesToDelete.Add(aInput);
                        }
                        else if (oAudioTrackInfo.AudioCodec == AudioCodec.PCM && inputContainer == ContainerType.MKV && oStreamControl.SelectedItem.IsStandard)
                        {
                            int trackID = oStream.TrackInfo.TrackID;
                            if (_videoInputInfo.ContainerFileType != ContainerType.MKV)
                                trackID++;
                            aInput = Path.Combine(strWorkingDirectory, oStream.TrackInfo.DemuxFileName);
                            HDStreamsExJob oJob = new HDStreamsExJob(new List<string>() { dpp.VideoInput }, aInput, null, trackID + ":\"" + aInput + "\"", 2);
                            audioJobs = new SequentialChain(audioJobs, new SequentialChain(oJob));
                            dpp.FilesToDelete.Add(FileUtil.AddToFileName(Path.ChangeExtension(aInput, "txt"), " - Log"));
                            dpp.FilesToDelete.Add(aInput);
                        }
                    }

                    if (dpp.Eac3toDemux && oAudioTrackInfo.AudioCodec == AudioCodec.EAC3 && oAudioTrackInfo.HasCore)
                    {
                        // E-AC-3 has been extracted by eac3to so the AC-3 core has been extracted
                        // change the codec but not the codec string so that the file extension is still correct
                        oAudioTrackInfo.Codec = "E-AC-3";
                        oAudioTrackInfo.AudioCodec = AudioCodec.AC3;
                        oAudioTrackInfo.AudioType = AudioType.AC3;
                        oAudioTrackInfo.HasCore = false;
                    }

                    if (oStream.EncodingMode == AudioEncodingMode.Never || oStream.EncodingMode == AudioEncodingMode.NeverOnlyCore
                        || (oStream.EncodingMode == AudioEncodingMode.IfCodecDoesNotMatch && oStream.EncoderSettings.EncoderType.ACodec.ID.Equals(oAudioTrackInfo.AudioCodec.ID)))
                        dpp.AudioTracks.Add(new OneClickAudioTrack(null, new MuxStream(aInput, strLanguage, strName, delay, false, false, null), oStreamControl.SelectedItem.IsStandard ? oAudioTrackInfo : null, bExtractMKVTrack));
                    else
                        dpp.AudioTracks.Add(new OneClickAudioTrack(new AudioJob(aInput, null, null, oStream.EncoderSettings, delay, strLanguage, strName), null, oStreamControl.SelectedItem.IsStandard ? oAudioTrackInfo : null, bExtractMKVTrack));
                }
            }


            // subtitle handling
            List<int> arrDVDSub = new List<int>();
            string strInput = String.Empty;
            foreach (OneClickStreamControl oStreamControl in subtitleTracks)
            {
                if (oStreamControl.SelectedStreamIndex <= 0) // not NONE
                    continue;

                OneClickStream oStream = oStreamControl.SelectedStream.Clone();

                if (oStreamControl.SelectedItem.IsStandard)
                {
                    if (bRemuxInputToMKV)
                        oStream.TrackInfo.MMGTrackID += iTrueHDOffset;

                    string strExtension = Path.GetExtension(oStream.TrackInfo.SourceFileName.ToLowerInvariant());
                    if (strExtension.Equals(".ifo") || strExtension.Equals(".vob"))
                    {
                        strInput = oStream.TrackInfo.SourceFileName;
                        if (strExtension.Equals(".vob"))
                        {
                            if (Path.GetFileName(strInput).ToUpperInvariant().Substring(0, 4) == "VTS_")
                                strInput = strInput.Substring(0, strInput.LastIndexOf("_")) + "_0.IFO";
                            else
                                strInput = Path.ChangeExtension(strInput, ".IFO");
                        }
                        arrDVDSub.Add(oStream.TrackInfo.MMGTrackID);
                        string outputFile = Path.Combine(dpp.WorkingDirectory, Path.GetFileNameWithoutExtension(strInput)) + "_" + oStream.TrackInfo.MMGTrackID + "_" + oStream.Language + ".idx";
                        oStream.DemuxFilePath = outputFile;
                        dpp.FilesToDelete.Add(outputFile);
                        dpp.FilesToDelete.Add(Path.ChangeExtension(outputFile, ".sub"));
                        dpp.SubtitleTracks.Add(oStream);
                    }
                    else if (inputContainer == ContainerType.MKV && !dpp.Eac3toDemux) // only if container MKV and no demux with eac3to
                    {
                        oStream.TrackInfo.ExtractMKVTrack = true;
                        oExtractMKVTrack.Add(oStream.TrackInfo.Clone());
                        dpp.SubtitleTracks.Add(oStream);
                    }
                }
                else
                    dpp.SubtitleTracks.Add(oStream);
            }
            if (arrDVDSub.Count > 0)
            {
                string outputFile = Path.Combine(dpp.WorkingDirectory, Path.GetFileNameWithoutExtension(strInput)) + ".idx";
                SubtitleIndexJob oJob = new SubtitleIndexJob(strInput, outputFile, false, arrDVDSub, _videoInputInfo.VideoInfo.PGCNumber, _videoInputInfo.VideoInfo.AngleNumber, false, true);
                prepareJobs = new SequentialChain(new SequentialChain(prepareJobs), new SequentialChain(oJob));
            }

            if (muxVideo && dpp.Container != ContainerType.MKV && inputContainer == ContainerType.MKV 
                && (!String.IsNullOrEmpty(dpp.IntermediateMKVFile) || !String.IsNullOrEmpty(dpp.VideoFileToMux)))
            {
                VideoTrackInfo vInfo = _videoInputInfo.VideoInfo.Track;
                if (_videoInputInfo.VideoInfo.Codec != VideoCodec.UNKNOWN)
                    vInfo.Codec = _videoInputInfo.VideoInfo.Codec.ID;
                if (!String.IsNullOrEmpty(dpp.IntermediateMKVFile))
                {
                    vInfo.SourceFileName = dpp.IntermediateMKVFile;
                    dpp.VideoFileToMux = Path.Combine(Path.GetDirectoryName(dpp.IntermediateMKVFile), vInfo.DemuxFileName);
                }
                else
                {
                    vInfo.SourceFileName = dpp.VideoFileToMux;
                    dpp.VideoFileToMux = Path.Combine(Path.GetDirectoryName(dpp.VideoFileToMux), vInfo.DemuxFileName);
                }
                if (!dpp.WorkingDirectory.Equals(Path.GetDirectoryName(dpp.VideoFileToMux)))
                    dpp.VideoFileToMux = Path.Combine(dpp.WorkingDirectory, Path.GetFileName(dpp.VideoFileToMux));

                dpp.FilesToDelete.Add(dpp.VideoFileToMux);
                oExtractMKVTrack.Add(_videoInputInfo.VideoInfo.Track);
            }

            // check if mkv attachments have to be extracted
            if (_videoInputInfo.Attachments.Count > 0 && _videoInputInfo.ContainerFileType == ContainerType.MKV && dpp.Container == ContainerType.MKV)
            {
                foreach (string strFileName in _videoInputInfo.Attachments)
                {
                    string strFile = Path.Combine(dpp.WorkingDirectory, strFileName);
                    dpp.Attachments.Add(strFile);
                    dpp.FilesToDelete.Add(strFile);
                }
            }

            // check if vfr mode is used and timecodes can be extracted
            if (_videoInputInfo.VideoInfo.VariableFrameRateMode && (inputContainer == ContainerType.MKV || !String.IsNullOrEmpty(dpp.IntermediateMKVFile)))
            {
                dpp.TimeStampFile = Path.Combine(dpp.WorkingDirectory, 
                    (string.IsNullOrEmpty(dpp.IntermediateMKVFile) ? Path.GetFileName(dpp.VideoInput) : Path.GetFileName(dpp.IntermediateMKVFile)) + ".timestamps.txt");
                dpp.FilesToDelete.Add(dpp.TimeStampFile);
            }

            // create MKV extract job if required
            // either because a track has to be extracted or that attachments MKV --> MKV are to be extracted & included
            if (oExtractMKVTrack.Count > 0 || dpp.Attachments.Count > 0 || !String.IsNullOrEmpty(dpp.TimeStampFile))
            {
                MkvExtractJob extractJob = new MkvExtractJob(String.IsNullOrEmpty(dpp.IntermediateMKVFile) ? dpp.VideoInput : dpp.IntermediateMKVFile, dpp.WorkingDirectory, oExtractMKVTrack);
                extractJob.Attachments = dpp.Attachments;
                extractJob.TimeStampFile = dpp.TimeStampFile;
                prepareJobs = new SequentialChain(prepareJobs, new SequentialChain(extractJob));
                if (dpp.ApplyDelayCorrection)
                    _oLog.LogEvent("Audio delay will be detected later as an intermediate MKV file is beeing used");
            }
            else
                dpp.ApplyDelayCorrection = false;


            // add audio job to chain if required
            prepareJobs = new SequentialChain(prepareJobs, new SequentialChain(audioJobs));

            JobChain finalJobChain = null;
            if (dpp.IndexType == FileIndexerWindow.IndexType.D2V)
            {   
                string indexFile = string.Empty;
                IndexJob job = null;
                indexFile = Path.Combine(dpp.WorkingDirectory, Path.GetFileNameWithoutExtension(output.Filename) + ".d2v");
                job = new D2VIndexJob(dpp.VideoInput, indexFile, 2, arrAudioTrackInfo, false, false);
                OneClickPostProcessingJob ocJob = new OneClickPostProcessingJob(dpp.VideoInput, indexFile, dpp);
                finalJobChain = new SequentialChain(prepareJobs, new SequentialChain(job), new SequentialChain(ocJob));
            }
            else if (dpp.IndexType == FileIndexerWindow.IndexType.DGI ||
                dpp.IndexType == FileIndexerWindow.IndexType.DGM ||
                dpp.IndexType == FileIndexerWindow.IndexType.FFMS ||
                dpp.IndexType == FileIndexerWindow.IndexType.LSMASH)
            {
                string indexFile = string.Empty;
                if (dpp.IndexType == FileIndexerWindow.IndexType.DGI || dpp.IndexType == FileIndexerWindow.IndexType.DGM)
                    indexFile = Path.Combine(dpp.WorkingDirectory, Path.GetFileNameWithoutExtension(output.Filename) + ".dgi");
                else if (dpp.IndexType == FileIndexerWindow.IndexType.LSMASH)
                    indexFile = Path.Combine(dpp.WorkingDirectory, Path.GetFileName(dpp.VideoInput) + ".lwi");
                else
                    indexFile = Path.Combine(dpp.WorkingDirectory, Path.GetFileName(dpp.VideoInput) + ".ffindex");
                OneClickPostProcessingJob ocJob = new OneClickPostProcessingJob(dpp.VideoInput, indexFile, dpp);

                IndexJob job = null;
                if (inputContainer == ContainerType.MKV)
                {
                    if (dpp.IndexType == FileIndexerWindow.IndexType.DGI)
                        job = new DGIIndexJob(dpp.VideoInput, indexFile, 0, new List<AudioTrackInfo>(), false, false, true);
                    else if (dpp.IndexType == FileIndexerWindow.IndexType.DGM)
                        job = new DGMIndexJob(dpp.VideoInput, indexFile, 0, new List<AudioTrackInfo>(), false, false, true);
                    else if (dpp.IndexType == FileIndexerWindow.IndexType.LSMASH)
                        job = new LSMASHIndexJob(dpp.VideoInput, indexFile, 0, new List<AudioTrackInfo>(), false);
                    else
                        job = new FFMSIndexJob(dpp.VideoInput, indexFile, 0, new List<AudioTrackInfo>(), false);
                    if (!String.IsNullOrEmpty(dpp.VideoFileToMux))
                    {
                        finalJobChain = new SequentialChain(prepareJobs, new SequentialChain(ocJob));
                        dpp.IndexType = FileIndexerWindow.IndexType.NONE;
                    }
                    else
                        finalJobChain = new SequentialChain(prepareJobs, new SequentialChain(job), new SequentialChain(ocJob));
                }
                else
                {
                    if (dpp.IndexType == FileIndexerWindow.IndexType.DGI)
                        job = new DGIIndexJob(dpp.VideoInput, indexFile, 2, arrAudioTrackInfo, false, false, true);
                    else if (dpp.IndexType == FileIndexerWindow.IndexType.DGM)
                        job = new DGMIndexJob(dpp.VideoInput, indexFile, 2, arrAudioTrackInfo, false, false, true);
                    else if (dpp.IndexType == FileIndexerWindow.IndexType.LSMASH)
                        job = new LSMASHIndexJob(dpp.VideoInput, indexFile, 2, arrAudioTrackInfo, false);
                    else
                        job = new FFMSIndexJob(dpp.VideoInput, indexFile, 2, arrAudioTrackInfo, false);
                    finalJobChain = new SequentialChain(prepareJobs, new SequentialChain(job), new SequentialChain(ocJob));
                }
            }
            else
            {
                // no indexer
                if (inputContainer == ContainerType.MKV && dpp.IndexType == FileIndexerWindow.IndexType.AVISOURCE && String.IsNullOrEmpty(dpp.VideoFileToMux))
                    dpp.VideoInput = _videoInputInfo.FileName;
                OneClickPostProcessingJob ocJob = new OneClickPostProcessingJob(dpp.VideoInput, null, dpp);
                finalJobChain = new SequentialChain(prepareJobs, new SequentialChain(ocJob));
            }


            // write all to be processed tracks into the log
            _oLog.LogEvent("Video: " + _videoInputInfo.FileName);
            foreach (OneClickAudioTrack oTrack in dpp.AudioTracks)
            {
                if (oTrack.AudioTrackInfo != null)
                    _oLog.LogEvent("Audio: " + oTrack.AudioTrackInfo.SourceFileName + " (" + oTrack.AudioTrackInfo.ToString() + ")");
                else if (oTrack.AudioJob != null)
                    _oLog.LogEvent("Audio: " + oTrack.AudioJob.Input);
            }
            foreach (OneClickStream oTrack in dpp.SubtitleTracks)
            {
                if (oTrack.TrackInfo != null)
                    _oLog.LogEvent("Subtitle: " + oTrack.TrackInfo.SourceFileName + " (" + oTrack.TrackInfo.ToString() + ")");
            }

            // add jobs to queue
            MainForm.Instance.Jobs.AddJobsWithDependencies(finalJobChain, !bBatchProcessing);

            if (!this.openOnQueue.Checked && this.Visible)
            {
                setControlState(false);
                tabControl1.SelectedTab = tabControl1.TabPages[0];
            }
            else
                this.Close();
        }

        private bool verifyInputSettings(MediaInfoFile oVideoInputInfo, string strWorkingDirectory)
        {
            if (oVideoInputInfo == null || !File.Exists(oVideoInputInfo.FileName))
            {
                if (bAutomatedProcessing)
                    _oLog.LogEvent("invalid input file: " + oVideoInputInfo);
                else
                    MessageBox.Show("Please select a valid input file!", "Incomplete configuration", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }

            if (String.IsNullOrEmpty(output.Filename))
            {
                if (bAutomatedProcessing)
                    _oLog.LogEvent("invalid output file: " + output.Filename);
                else
                    MessageBox.Show("Please select a valid output file!", "Incomplete configuration", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }

            if (!FileUtil.IsDirWriteable(Path.GetDirectoryName(output.Filename)))
            {
                if (bAutomatedProcessing)
                    _oLog.LogEvent("cannot write to the folder: " + Path.GetDirectoryName(output.Filename));
                else
                    MessageBox.Show("MeGUI cannot write to the folder: " + Path.GetDirectoryName(output.Filename) + " \n" +
                                 "Please select a writeable output path to save your project!", "Incomplete configuration", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }

            if (String.IsNullOrEmpty(strWorkingDirectory))
            {
                if (bAutomatedProcessing)
                    _oLog.LogEvent("invalid working directory: " + strWorkingDirectory);
                else
                    MessageBox.Show("Please select a valid working directory!", "Incomplete configuration", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }

            if (!FileUtil.IsDirWriteable(strWorkingDirectory))
            {
                if (bAutomatedProcessing)
                    _oLog.LogEvent("cannot write to the folder: " + strWorkingDirectory);
                else
                    MessageBox.Show("MeGUI cannot write to the folder: " + strWorkingDirectory + " \n" +
                                 "Please select a writeable working path to save your project!", "Incomplete configuration", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }

            if (verifyStreamSettings() != null || VideoSettings == null)
            {
                if (bAutomatedProcessing)
                    _oLog.LogEvent("cannot process this job");
                else
                    MessageBox.Show("MeGUI cannot process this job", "Wrong configuration", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }

            ChapterInfo oChapterInfo = new ChapterInfo();
            if (!String.IsNullOrEmpty(chapterFile.Filename) && !chapterFile.Filename.StartsWith("<") && !oChapterInfo.LoadFile(chapterFile.Filename))
            {
                if (bAutomatedProcessing)
                    _oLog.LogEvent("cannot process chapter file");
                else
                    MessageBox.Show("The chapter file cannot be read or is unsupported", "Wrong configuration", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }

            for (int i = 0; i < audioTracks.Count - 1; i++)
            {
                if (audioTracks[i].SelectedStreamIndex <= 0) // not NONE
                    continue;

                for (int j = i + 1; j < audioTracks.Count; j++)
                {
                    if (audioTracks[j].SelectedStreamIndex <= 0) // not NONE
                        continue;

                    // compare the two controls
                    if (audioTracks[i].SelectedStream.DemuxFilePath.Equals(audioTracks[j].SelectedStream.DemuxFilePath) &&
                        audioTracks[i].SelectedStream.Language.Equals(audioTracks[j].SelectedStream.Language) &&
                        audioTracks[i].SelectedStream.Name.Equals(audioTracks[j].SelectedStream.Name) &&
                        audioTracks[i].SelectedStream.DefaultStream == audioTracks[j].SelectedStream.DefaultStream &&
                        audioTracks[i].SelectedStream.Delay == audioTracks[j].SelectedStream.Delay &&
                        audioTracks[i].SelectedStream.EncoderSettings.Equals(audioTracks[j].SelectedStream.EncoderSettings) &&
                        audioTracks[i].SelectedStream.EncodingMode == audioTracks[j].SelectedStream.EncodingMode &&
                        audioTracks[i].SelectedStream.ForcedStream == audioTracks[j].SelectedStream.ForcedStream)
                    {
                        DialogResult dr = MessageBox.Show("The audio tracks " + (i + 1) + " and " + (j + 1) + " are identical. Are you sure you want to proceed?", "Duplicate audio tracks found", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (dr == DialogResult.No)
                            return false;
                    }  
                }
            }

            for (int i = 0; i < subtitleTracks.Count - 1; i++)
            {
                if (subtitleTracks[i].SelectedStreamIndex <= 0) // not NONE
                    continue;

                for (int j = i + 1; j < subtitleTracks.Count; j++)
                {
                    if (subtitleTracks[j].SelectedStreamIndex <= 0) // not NONE
                        continue;

                    // compare the two controls
                    if (subtitleTracks[i].SelectedStream.DemuxFilePath.Equals(subtitleTracks[j].SelectedStream.DemuxFilePath) &&
                        subtitleTracks[i].SelectedStream.Language.Equals(subtitleTracks[j].SelectedStream.Language) &&
                        subtitleTracks[i].SelectedStream.Name.Equals(subtitleTracks[j].SelectedStream.Name) &&
                        subtitleTracks[i].SelectedStream.DefaultStream == subtitleTracks[j].SelectedStream.DefaultStream &&
                        subtitleTracks[i].SelectedStream.Delay == subtitleTracks[j].SelectedStream.Delay &&
                        subtitleTracks[i].SelectedStream.EncoderSettings.Equals(subtitleTracks[j].SelectedStream.EncoderSettings) &&
                        subtitleTracks[i].SelectedStream.EncodingMode == subtitleTracks[j].SelectedStream.EncodingMode &&
                        subtitleTracks[i].SelectedStream.ForcedStream == subtitleTracks[j].SelectedStream.ForcedStream)
                    {
                        DialogResult dr = MessageBox.Show("The subtitle tracks " + (i + 1) + " and " + (j + 1) + " are identical. Are you sure you want to proceed?", "Duplicate subtitle tracks found", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (dr == DialogResult.No)
                            return false;
                    }
                }
            }



            return true;
        }

        #endregion

        #region profile management

        private string verifyStreamSettings()
        {
            for (int i = 0; i < audioTracks.Count; ++i)
            {
                if (audioTracks[i].SelectedItem.IsStandard || audioTracks[i].SelectedStreamIndex <= 0)
                    continue;

                string r = MainForm.verifyInputFile(audioTracks[i].SelectedFile);
                if (r != null) 
                    return r;
            }
            for (int i = 0; i < subtitleTracks.Count; ++i)
            {
                if (subtitleTracks[i].SelectedItem.IsStandard || subtitleTracks[i].SelectedStreamIndex <= 0)
                    continue;

                string r = MainForm.verifyInputFile(subtitleTracks[i].SelectedFile);
                if (r != null) 
                    return r;
            }
            return null;
        }
        #endregion
        
        private void audio1_SomethingChanged(object sender, EventArgs e)
        {
            updatePossibleContainers();
        }

        void ProfileChanged(object sender, EventArgs e)
        {
            if (videoProfile.SelectedProfile.FQName.StartsWith("x264") && !chkDontEncodeVideo.Checked)
                usechaptersmarks.Enabled = true;
            else
                usechaptersmarks.Enabled = false;
            updatePossibleContainers();
        }

        private void keepInputResolution_CheckedChanged(object sender, EventArgs e)
        {
            if (keepInputResolution.Checked || chkDontEncodeVideo.Checked)
                horizontalResolution.Enabled = autoCrop.Enabled = false;
            else
                horizontalResolution.Enabled = autoCrop.Enabled = true;
        }

        private void chkDontEncodeVideo_CheckedChanged(object sender, EventArgs e)
        {
            if (chkDontEncodeVideo.Checked)
            {
                horizontalResolution.Enabled = autoCrop.Enabled = videoProfile.Enabled = false;
                usechaptersmarks.Enabled = keepInputResolution.Enabled = addPrerenderJob.Enabled = false;
                autoDeint.Enabled = fileSize.Enabled = avsProfile.Enabled = false;
            }
            else
            {
                videoProfile.Enabled = keepInputResolution.Enabled = addPrerenderJob.Enabled = true;
                autoDeint.Enabled = fileSize.Enabled = avsProfile.Enabled = true;
                if (videoProfile.SelectedProfile.FQName.StartsWith("x264"))
                    usechaptersmarks.Enabled = true;
                else
                    usechaptersmarks.Enabled = false;
                keepInputResolution_CheckedChanged(null, null);
            }
            updatePossibleContainers();
        }

        #region subtitle track handling
        private void subtitleAddTrack_Click(object sender, EventArgs e)
        {
            SubtitleAddTrack(true);
        }

        private void subtitleRemoveTrack_Click(object sender, EventArgs e)
        {
            SubtitleRemoveTrack(iSelectedSubtitleTabPage);
        }

        private void SubtitleAddTrack(bool bChangeFocus)
        {
            beingCalled++;

            TabPage p = new TabPage("Subtitle " + (iSelectedSubtitleTabPage + 1));
            p.UseVisualStyleBackColor = subtitlesTab.TabPages[0].UseVisualStyleBackColor;
            p.Padding = subtitlesTab.TabPages[0].Padding;

            OneClickStreamControl a = new OneClickStreamControl();
            a.TrackNumber = subtitleTracks.Count + 1;
            a.Dock = subtitleTracks[0].Dock;
            a.Padding = subtitleTracks[0].Padding;
            a.ShowDelay = subtitleTracks[0].ShowDelay;
            a.ShowDefaultStream = subtitleTracks[0].ShowDefaultStream;
            a.ShowForceStream = subtitleTracks[0].ShowForceStream;
            a.chkDefaultStream.CheckedChanged += new System.EventHandler(this.chkDefaultStream_CheckedChanged);
            a.SomethingChanged += new EventHandler(audio1_SomethingChanged);
            a.Filter = subtitleTracks[0].Filter;
            a.FileUpdated += oneClickSubtitleStreamControl_FileUpdated;

            // clone the streams
            object[] oStreams = new object[subtitleTracks[0].StandardStreams.Length];
            int i = 0;
            foreach (object oStream in subtitleTracks[0].StandardStreams)
            {
                if (oStream is OneClickStream)
                    oStreams[i] = ((OneClickStream)oStream).Clone();
                else
                    oStreams[i] = oStream;
                i++;
            }
            a.StandardStreams = oStreams;

            a.CustomStreams = subtitleTracks[0].CustomStreams;
            a.SelectedStreamIndex = 0;
            a.initProfileHandler();  
            if (this.Visible)
                a.enableDragDrop();

            if (iSelectedSubtitleTabPage > subtitlesTab.TabPages.Count - 3 ||
                iSelectedSubtitleTabPage > subtitleTracks.Count - 1)
                iSelectedSubtitleTabPage = 0;

            IntPtr h = subtitlesTab.Handle;  // fix for TabPages.Insert not working if handle has not been created yet (issue in batch mode)
            subtitlesTab.TabPages.Insert(iSelectedSubtitleTabPage + 1, p);
            subtitleTracks.Insert(iSelectedSubtitleTabPage + 1, a);
            p.Controls.Add(a);

            for (int j = 0; j < subtitlesTab.TabCount - 2; j++)
                subtitlesTab.TabPages[j].Text = "Subtitle " + (j + 1);

            if (bChangeFocus)
                subtitlesTab.SelectedTab = p;

            beingCalled--;
            updatePossibleContainers();
        }

        private void SubtitleRemoveTrack(int iTabPageIndex)
        {
            if (iTabPageIndex >= subtitlesTab.TabCount - 2)
                return;

            beingCalled++;

            if (iTabPageIndex == 0 && subtitlesTab.TabCount == 3)
                SubtitleAddTrack(true);

            subtitlesTab.TabPages.RemoveAt(iTabPageIndex);
            subtitleTracks.RemoveAt(iTabPageIndex);

            for (int i = 0; i < subtitlesTab.TabCount - 2; i++)
                subtitlesTab.TabPages[i].Text = "Subtitle " + (i + 1);

            if (iTabPageIndex < subtitlesTab.TabCount - 2)
                subtitlesTab.SelectedIndex = iTabPageIndex;
            else
                subtitlesTab.SelectedIndex = subtitlesTab.TabCount - 3;
            iSelectedSubtitleTabPage = subtitlesTab.SelectedIndex;

            beingCalled--;
            updatePossibleContainers();
        }


        private int iSelectedSubtitleTabPage = 0;
        private void subtitlesTab_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right)
                return;

            Point p = e.Location;
            for (int i = 0; i < subtitlesTab.TabCount - 2; i++)
            {
                Rectangle rect = subtitlesTab.GetTabRect(i);
                rect.Offset(2, 2);
                rect.Width -= 4;
                rect.Height -= 4;
                if (rect.Contains(p))
                {
                    iSelectedSubtitleTabPage = i;
                    subtitleMenu.Show(subtitlesTab, e.Location);
                    break;
                }
            }
        }

        private void oneClickSubtitleStreamControl_FileUpdated(object sender, EventArgs e)
        {
            if (bLock)
                return;

            int i = subtitleTracks.IndexOf((OneClickStreamControl)sender);

            if (i < 0)
                return;

            OneClickStreamControl track = subtitleTracks[i];
            foreach (OneClickStreamControl oControl in subtitleTracks)
            {
                if (oControl == track)
                    continue;

                if (oControl.CustomStreams.Length != track.CustomStreams.Length)
                {
                    int iIndex = -1;
                    if (!track.SelectedItem.IsStandard)
                        iIndex = oControl.SelectedStreamIndex;
                    bLock = true;
                    oControl.CustomStreams = track.CustomStreams;
                    bLock = false;
                    if (iIndex >= 0 && oControl.SelectedStreamIndex != iIndex)
                        oControl.SelectedStreamIndex = iIndex;
                }
            }

            updatePossibleContainers();
        }

        private void SubtitleResetTrack(List<OneClickStream> arrSubtitleTrackInfo, OneClickSettings settings)
        {
            // generate track names
            List<object> trackNames = new List<object>();
            trackNames.Add("None");
            foreach (object o in arrSubtitleTrackInfo)
                trackNames.Add(o);
            subtitleTracks[0].StandardStreams = trackNames.ToArray();
            subtitleTracks[0].CustomStreams = new object[0];
            subtitleTracks[0].SelectedStreamIndex = 0;

            // delete all tracks beside the first one and the last two
            try
            {
                while (subtitlesTab.TabCount > 3)
                    subtitlesTab.TabPages.RemoveAt(1);
            }
            catch (Exception) {}
            subtitleTracks.RemoveRange(1, subtitleTracks.Count - 1);

            iSelectedSubtitleTabPage = 0;

            foreach (string strLanguage in settings.DefaultSubtitleLanguage)
                if (strLanguage.Equals("[none]"))
                    return;

            int iCounter = 0;
            foreach (string strLanguage in settings.DefaultSubtitleLanguage)
            {
                for (int i = 0; i < arrSubtitleTrackInfo.Count; i++)
                {
                    if (arrSubtitleTrackInfo[i].Language.ToLowerInvariant().Equals(strLanguage.ToLowerInvariant()))
                    {
                        if (iCounter > 0)
                            SubtitleAddTrack(false);

                        iSelectedSubtitleTabPage = iCounter;
                        subtitleTracks[iCounter++].SelectedStreamIndex = i + 1;
                    }
                }
            }

            if (iCounter == 0 && arrSubtitleTrackInfo.Count > 0 && !settings.UseNoLanguagesAsFallback)
            {
                for (int i = 0; i < arrSubtitleTrackInfo.Count; i++)
                {
                    if (iCounter > 0)
                        SubtitleAddTrack(false);

                    iSelectedSubtitleTabPage = iCounter;
                    subtitleTracks[iCounter++].SelectedStreamIndex = i + 1;
                }
            }

            iSelectedSubtitleTabPage = 0;
        }

        private void subtitlesTab_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            SubtitleAddTrack(true);
        }

        private void chkDefaultStream_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked == false)
                return;

            foreach (OneClickStreamControl oTrack in subtitleTracks)
            {
                if (sender != oTrack.chkDefaultStream && oTrack.chkDefaultStream.Checked == true)
                    oTrack.chkDefaultStream.Checked = false;
            }
        }
        #endregion

        #region audio Track Handling
        private void audioAddTrack_Click(object sender, EventArgs e)
        {
            AudioAddTrack(true);
        }

        private void audioRemoveTrack_Click(object sender, EventArgs e)
        {
            AudioRemoveTrack(iSelectedAudioTabPage);
        }

        private void AudioAddTrack(bool bChangeFocus)
        {
            beingCalled++;

            TabPage p = new TabPage("Audio " + (iSelectedAudioTabPage + 1));
            p.UseVisualStyleBackColor = audioTab.TabPages[0].UseVisualStyleBackColor;
            p.Padding = audioTab.TabPages[0].Padding;

            OneClickStreamControl a = new OneClickStreamControl();
            a.TrackNumber = audioTracks.Count + 1;
            a.Dock = audioTracks[0].Dock;
            a.Padding = audioTracks[0].Padding;
            a.ShowDelay = audioTracks[0].ShowDelay;
            a.ShowDefaultStream = audioTracks[0].ShowDefaultStream;
            a.ShowForceStream = audioTracks[0].ShowForceStream;
            a.Filter = audioTracks[0].Filter;
            a.FileUpdated += oneClickAudioStreamControl_FileUpdated;

            // clone the streams
            object[] oStreams = new object[audioTracks[0].StandardStreams.Length];
            int i = 0;
            foreach (object oStream in audioTracks[0].StandardStreams)
            {
                if (oStream is OneClickStream)
                    oStreams[i] = ((OneClickStream)oStream).Clone();
                else
                    oStreams[i] = oStream;
                i++;
            }
            a.StandardStreams = oStreams;

            a.CustomStreams = audioTracks[0].CustomStreams;
            a.SelectedStreamIndex = 0;
            a.SomethingChanged += new EventHandler(audio1_SomethingChanged);
            a.EncodingMode = audioTracks[0].EncodingMode;
            a.initProfileHandler();
            a.SelectProfileNameOrWarn(audioTracks[0].EncoderProfile);
            if (this.Visible)
                a.enableDragDrop();

            if (iSelectedAudioTabPage > audioTab.TabPages.Count - 3 ||
                iSelectedAudioTabPage > audioTracks.Count - 1)
                iSelectedAudioTabPage = 0;

            if (!audioTab.IsDisposed)
            {
                IntPtr h = audioTab.Handle;  // fix for TabPages.Insert not working if handle has not been created yet (issue in batch mode)
            }
            audioTab.TabPages.Insert(iSelectedAudioTabPage + 1, p);
            audioTracks.Insert(iSelectedAudioTabPage + 1, a);
            p.Controls.Add(a);

            for (int j = 0; j < audioTab.TabCount - 2; j++)
                audioTab.TabPages[j].Text = "Audio " + (j + 1);

            if (bChangeFocus)
                audioTab.SelectedTab = p;

            beingCalled--;
            updatePossibleContainers();
        }

        private void AudioRemoveTrack(int iTabPageIndex)
        {
            if (iTabPageIndex >= audioTab.TabCount - 2)
                return;

            beingCalled++;

            if (iTabPageIndex == 0 && audioTab.TabCount == 3)
                AudioAddTrack(true);

            audioTab.TabPages.RemoveAt(iTabPageIndex);
            audioTracks.RemoveAt(iTabPageIndex);

            for (int i = 0; i < audioTab.TabCount - 2; i++)
                audioTab.TabPages[i].Text = "Audio " + (i + 1);

            if (iTabPageIndex < audioTab.TabCount - 2)
                audioTab.SelectedIndex = iTabPageIndex;
            else
                audioTab.SelectedIndex = audioTab.TabCount - 3;
            iSelectedAudioTabPage = audioTab.SelectedIndex;

            beingCalled--;
            updatePossibleContainers();
        }

        private int iSelectedAudioTabPage = 0;
        private void audioTab_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right)
                return;

            Point p = e.Location;
            for (int i = 0; i < audioTab.TabCount - 2; i++)
            {
                Rectangle rect = audioTab.GetTabRect(i);
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

        private void oneClickAudioStreamControl_FileUpdated(object sender, EventArgs e)
        {
            if (bLock)
                return;
            
            int i = audioTracks.IndexOf((OneClickStreamControl)sender);

            if (i < 0)
                return;

            OneClickStreamControl track = audioTracks[i];
            if (!track.SelectedItem.IsStandard)
                track.SelectedStream.Delay = PrettyFormatting.getDelayAndCheck(track.SelectedStream.DemuxFilePath) ?? 0;

            foreach (OneClickStreamControl oControl in audioTracks)
            {
                if (oControl == track)
                    continue;

                if (oControl.CustomStreams.Length != track.CustomStreams.Length)
                {
                    int iIndex = -1;
                    if (!track.SelectedItem.IsStandard)
                        iIndex = oControl.SelectedStreamIndex;
                    bLock = true;
                    oControl.CustomStreams = track.CustomStreams;
                    bLock = false;
                    if (iIndex >= 0 && oControl.SelectedStreamIndex != iIndex)
                        oControl.SelectedStreamIndex = iIndex;
                }
            }

            updatePossibleContainers();
        }

        private void AudioResetTrack(List<OneClickStream> arrAudioTrackInfo, OneClickSettings settings)
        {
            // generate track names
            List<object> trackNames = new List<object>();
            trackNames.Add("None");
            foreach (OneClickStream o in arrAudioTrackInfo)
                trackNames.Add(o);
            audioTracks[0].StandardStreams = trackNames.ToArray();
            audioTracks[0].CustomStreams = new object[0];
            audioTracks[0].SelectedStreamIndex = 0;

            // delete all tracks beside the first one and the last two
            try
            {
                while (audioTab.TabCount > 3)
                    audioTab.TabPages.RemoveAt(1);
            }
            catch (Exception) {}
            audioTracks.RemoveRange(1, audioTracks.Count - 1);

            iSelectedAudioTabPage = 0;

            foreach (string strLanguage in settings.DefaultAudioLanguage)
                if (strLanguage.Equals("[none]"))
                    return;

            int iCounter = 0;
            foreach (string strLanguage in settings.DefaultAudioLanguage)
            {
                for (int i = 0; i < arrAudioTrackInfo.Count; i++)
                {
                    if (arrAudioTrackInfo[i].Language.ToLowerInvariant().Equals(strLanguage.ToLowerInvariant()))
                    {
                        // should only the first audio track for this language be processed?
                        bool bUseFirstTrackOnly = true;
                        if (settings.AudioSettings.Count > 0)
                            bUseFirstTrackOnly = settings.AudioSettings[0].UseFirstTrackOnly;
                        foreach (OneClickAudioSettings oAudioSettings in settings.AudioSettings)
                        {
                            if (arrAudioTrackInfo[i].Language.ToLowerInvariant().Equals(oAudioSettings.Language.ToLowerInvariant()))
                            {
                                bUseFirstTrackOnly = oAudioSettings.UseFirstTrackOnly;
                                break;
                            }
                        }

                        bool bAddTrack = true;
                        if (bUseFirstTrackOnly)
                        {
                            foreach (OneClickStreamControl oAudioControl in audioTracks)
                            {
                                if (oAudioControl.SelectedStreamIndex > 0 &&
                                    oAudioControl.SelectedStream.Language.ToLowerInvariant().Equals(arrAudioTrackInfo[i].Language.ToLowerInvariant()))
                                {
                                    bAddTrack = false;
                                    break;
                                }
                            }
                        }

                        if (!bAddTrack)
                            break;

                        if (iCounter > 0)
                            AudioAddTrack(false);

                        iSelectedAudioTabPage = iCounter;
                        audioTracks[iCounter++].SelectedStreamIndex = i + 1;
                    }
                }
            }

            if (iCounter == 0 && arrAudioTrackInfo.Count > 0 && !settings.UseNoLanguagesAsFallback)
            {
                for (int i = 0; i < arrAudioTrackInfo.Count; i++)
                {
                    // should only the first audio track for this language be processed?
                    bool bUseFirstTrackOnly = true;
                    if (settings.AudioSettings.Count > 0)
                        bUseFirstTrackOnly = settings.AudioSettings[0].UseFirstTrackOnly;
                    foreach (OneClickAudioSettings oAudioSettings in settings.AudioSettings)
                    {
                        if (arrAudioTrackInfo[i].Language.ToLowerInvariant().Equals(oAudioSettings.Language.ToLowerInvariant()))
                        {
                            bUseFirstTrackOnly = oAudioSettings.UseFirstTrackOnly;
                            break;
                        }
                    }

                    bool bAddTrack = true;
                    if (bUseFirstTrackOnly)
                    {
                        foreach (OneClickStreamControl oAudioControl in audioTracks)
                        {
                            if (oAudioControl.SelectedStreamIndex > 0 &&
                                oAudioControl.SelectedStream.Language.ToLowerInvariant().Equals(arrAudioTrackInfo[i].Language.ToLowerInvariant()))
                            {
                                bAddTrack = false;
                                break;
                            }
                        }
                    }

                    if (!bAddTrack)
                        break;

                    if (iCounter > 0)
                        AudioAddTrack(false);

                    iSelectedAudioTabPage = iCounter;
                    audioTracks[iCounter++].SelectedStreamIndex = i + 1;
                }
            }

            iSelectedAudioTabPage = 0;
            ResetAudioSettings(settings);
        }

        private void ResetAudioSettings(OneClickSettings settings)
        {
            foreach (OneClickStreamControl a in audioTracks)
            {
                bool bFound = false;
                for (int i = 1; i < settings.AudioSettings.Count; i++)
                {
                    if (a.SelectedStream.Language.Equals(settings.AudioSettings[i].Language))
                    {
                        a.SelectProfileNameOrWarn(settings.AudioSettings[i].Profile);
                        a.EncodingMode = settings.AudioSettings[i].AudioEncodingMode;
                        bFound = true;
                        break;
                    }
                }
                if (!bFound)
                {
                    if (settings.AudioSettings.Count > 0)
                    {
                        a.SelectProfileNameOrWarn(settings.AudioSettings[0].Profile);
                        a.EncodingMode = settings.AudioSettings[0].AudioEncodingMode;
                    }
                    else
                        a.EncodingMode = AudioEncodingMode.IfCodecDoesNotMatch;
                }
            }
        }

        private void audioTab_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            AudioAddTrack(true);
        }
        #endregion

        private void OneClickWindow_Shown(object sender, EventArgs e)
        {
            oneClickAudioStreamControl1.enableDragDrop();
            oneClickSubtitleStreamControl1.enableDragDrop();
            if (!String.IsNullOrEmpty(input.SelectedText))
                OneClickProfileChanged(null, null);
        }

        private void audioTab_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (audioTab.SelectedTab.Text.Equals("   +"))
                AudioAddTrack(true);
            else if (audioTab.SelectedTab.Text.Equals("    -"))
                AudioRemoveTrack(iSelectedAudioTabPage);
            else
                iSelectedAudioTabPage = audioTab.SelectedIndex;
        }

        private void subtitlesTab_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (subtitlesTab.SelectedTab.Text.Equals("   +"))
                SubtitleAddTrack(true);
            else if (subtitlesTab.SelectedTab.Text.Equals("    -"))
                SubtitleRemoveTrack(iSelectedSubtitleTabPage);
            else
                iSelectedSubtitleTabPage = subtitlesTab.SelectedIndex;
        }

        /// <summary>
        /// Checks if the audio can be not encoded = remuxed only
        /// </summary>
        /// <param name="iFile">MediaInfoFile of the selected video input</param>
        /// <param name="bIsStandardTrack">true if standard = inlcuded track, false if external/dedicated file</param>
        /// <param name="inputContainer">the input container type</param>
        /// <returns>true if do not encode is possible</returns>
        private bool isDontEncodeAudioPossible(MediaInfoFile iFile, bool bIsStandardTrack, ContainerType inputContainer)
        {
            // external files can be remuxed
            if (!bIsStandardTrack)
                return true;

            // if no media information is available ==> recode
            if (iFile == null)
                return false;

            // only mkv content can be extracted and only the DGIndexXX tools extract audio tracks
            if (inputContainer == ContainerType.MKV ||
                iFile.IndexerToUse == FileIndexerWindow.IndexType.D2V ||
                iFile.IndexerToUse == FileIndexerWindow.IndexType.DGM ||
                iFile.IndexerToUse == FileIndexerWindow.IndexType.DGI)
                return true;

            return false;
        }

        private void deleteChapter_Click(object sender, EventArgs e)
        {
            chapterFile.Filename = null;
        }

        private void deleteWorking_Click(object sender, EventArgs e)
        {
            workingDirectory.Filename = null;
        }
    }

    [Serializable]
    public class OneClickFilesToProcess
    {
        public string FilePath;
        public int PGCNumber;
        public int AngleNumber;

        public OneClickFilesToProcess() : this(string.Empty, 1, 0)
        {

        }

        public OneClickFilesToProcess(string strPath, int iPGCNumber, int iAngleNumber)
        {
            FilePath = strPath;
            PGCNumber = iPGCNumber;
            AngleNumber = iAngleNumber;
        }
    }

    public class OneClickTool : MeGUI.core.plugins.interfaces.ITool
    {

        #region ITool Members

        public string Name
        {
            get { return "One Click Encoder"; }
        }

        public void Run(MainForm info)
        {
            OneClickWindow ocmt = new OneClickWindow();
            ocmt.Show();
        }

        public Shortcut[] Shortcuts
        {
            get { return new Shortcut[] { Shortcut.CtrlF1 }; }
        }

        #endregion

        #region IIDable Members

        public string ID
        {
            get { return "one_click"; }
        }

        #endregion

    }
}