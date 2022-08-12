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
using System.IO;
using System.Text;
using System.Windows.Forms;

using MeGUI.core.details;
using MeGUI.core.util;

namespace MeGUI
{
    /// <summary>
    /// Summary description for File Indexer.
    /// </summary>
    public partial class FileIndexerWindow : Form
    {
        public enum IndexType
        {
            AVISOURCE, D2V, DGM, DGI, FFMS, LSMASH, NONE
        };

        #region variables
        private LogItem _oLog;
        private IndexType IndexerUsed = IndexType.D2V;
        private string strVideoCodec = "";
        private string strVideoScanType = "";
        private string strContainerFormat = "";
        private List<AudioTrackInfo> audioTracks = new List<AudioTrackInfo>();
        private bool dialogMode = false; // $%£%$^>*"%$%%$#{"!!! Affects the public behaviour!
        private bool configured = false;
        private MediaInfoFile iFile;
        #endregion

        #region start / stop
        public void setConfig(string input, string projectName, int demuxType,
            bool showCloseOnQueue, bool closeOnQueue, bool loadOnComplete, bool updateMode)
        {
            openVideo(input);
            if (!string.IsNullOrEmpty(projectName))
                this.output.Filename = projectName;
            if (demuxType == 0)
                demuxNoAudiotracks.Checked = true;
            else
                demuxAll.Checked = true;
            this.loadOnComplete.Checked = loadOnComplete;
            if (updateMode)
            {
                this.dialogMode = true;
                queueButton.Text = "Update";
            }
            else
                this.dialogMode = false;
            checkIndexIO();
            if (!showCloseOnQueue)
            {
                this.closeOnQueue.Hide();
                this.Controls.Remove(this.closeOnQueue);
            }
            this.closeOnQueue.Checked = closeOnQueue;
        }

        public FileIndexerWindow(MainForm mainForm)
        {
            InitializeComponent();
            CheckDGIIndexer();
        }

        public FileIndexerWindow(MainForm mainForm, string fileName) : this(mainForm)
        {
            CheckDGIIndexer();
            openVideo(fileName);
        }

        public FileIndexerWindow(MainForm mainForm, string fileName, bool autoReturn) : this(mainForm, fileName)
        {
            CheckDGIIndexer();
            openVideo(fileName);
            this.loadOnComplete.Checked = true;
            this.closeOnQueue.Checked = true;
            checkIndexIO();
        }

        private void CheckDGIIndexer()
        {
            string filter = "All DGIndex supported files|*.ifo;*.m1v;*.m2t;*.m2ts;*.m2v;*.mpeg;*.mpg;*.mpv;*.pva;*.tp;*.trp;*.ts;*.vob;*.vro";
            filter += "|All FFMS Indexer supported files|*.avi;*.flv;*.ifo;*.m2ts;*.mkv;*.mp4;*.mpg;*.mpls;*.ogm;*.ts;*.vob;*.wmv";
            filter += "|All LSMASH Indexer supported files|*.avi;*.flv;*.ifo;*.m2ts;*.mkv;*.mp4;*.mpg;*.mpls;*.ogm;*.ts;*.vob;*.wmv";
            if (MainForm.Instance.Settings.IsDGIIndexerAvailable() || MainForm.Instance.Settings.IsDGMIndexerAvailable())
            {
                if (MainForm.Instance.Settings.IsDGIIndexerAvailable())
                    filter += "|All DGIndexNV supported files|*.264;*.avc;*.h264;*.265;*.hevc;*.ifo;*.m2t;*.m2ts;*.m2v;*.mkv;*.mp4;*.mpeg;*.mpg;*.mpls;*.mpv;*.mts;*.tp;*.trp;*.ts;*.vc1;*.vob";
                if (MainForm.Instance.Settings.IsDGMIndexerAvailable())
                    filter += "|All DGIndexIM supported files|*.264;*.avc;*.h264;*.265;*.hevc;*.ifo;*.m2t;*.m2ts;*.m2v;*.mkv;*.mp4;*.mpeg;*.mpg;*.mpls;*.mpv;*.mts;*.tp;*.trp;*.ts;*.vc1;*.vob";
                filter += "|All supported files|*.264;*.265;*.hevc;*.avc;*.avi;*.flv;*.h264;*.265;*.hevc;*.ifo;*.m1v;*.m2t*;*.m2ts;*.m2v;*.mkv;*.mp4;*.mpeg;*.mpg;*.mpls;*.mpv;*.mts;*.ogm;*.pva;*.tp;*.trp;*.ts;*.vc1;*.vob;*.vro;*.wmv";
                filter += "|All files|*.*";
                input.Filter = filter;
                if (MainForm.Instance.Settings.IsDGIIndexerAvailable() && MainForm.Instance.Settings.IsDGMIndexerAvailable())
                    input.FilterIndex = 6;
                else
                    input.FilterIndex = 5;
                btnDGI.Visible = MainForm.Instance.Settings.IsDGIIndexerAvailable();
                btnDGM.Visible = MainForm.Instance.Settings.IsDGMIndexerAvailable();
                btnFFMS.Location = new System.Drawing.Point(MainForm.Instance.Settings.DPIRescale(262), MainForm.Instance.Settings.DPIRescale(20));
                
            }
            else
            {
                filter += "|All supported files|*.264;*.avc;*.avi;*.flv;*.h264;*.265;*.hevc;*.ifo;*.m1v;*.m2t*;*.m2ts;*.m2v;*.mkv;*.mp4;*.mpeg;*.mpg;*.mpls;*.mpv;*.mts;*.ogm;*.pva;*.tp;*.trp;*.ts;*.vob;*.vro;*.wmv";
                filter += "|All files|*.*";
                input.Filter = filter;
                input.FilterIndex = 4;
                btnDGM.Visible = btnDGI.Visible = false;
                btnFFMS.Location = new System.Drawing.Point(MainForm.Instance.Settings.DPIRescale(171), MainForm.Instance.Settings.DPIRescale(20));
            }
        }

        private void changeIndexer(IndexType dgType)
        {
            switch (dgType)
            {
                case IndexType.DGI:
                    {
                        this.output.Filter = "DGIndexNV project files|*.dgi";
                        this.demuxTracks.Enabled = true;
                        this.gbAudio.Text = " Audio Demux ";
                        this.gbOutput.Enabled = true;
                        this.demuxVideo.Enabled = true;
                        IndexerUsed = IndexType.DGI;
                        btnDGI.Checked = true;
                        break;
                    }
                case IndexType.DGM:
                    {
                        this.output.Filter = "DGIndexIM project files|*.dgi";
                        this.demuxTracks.Enabled = true;
                        this.gbAudio.Text = " Audio Demux ";
                        this.gbOutput.Enabled = true;
                        this.demuxVideo.Enabled = true;
                        IndexerUsed = IndexType.DGM;
                        btnDGM.Checked = true;
                        break;
                    }
                case IndexType.D2V:
                    {
                        this.output.Filter = "DGIndex project files|*.d2v";
                        this.demuxTracks.Enabled = true;
                        //this.gbOutput.Enabled = true;
                        this.gbAudio.Text = " Audio Demux ";
                        this.demuxVideo.Enabled = true;
                        IndexerUsed = IndexType.D2V;
                        btnD2V.Checked = true;
                        break;
                    }
                case IndexType.FFMS:
                    {
                        this.output.Filter = "FFMSIndex project files|*.ffindex";
                        //this.gbOutput.Enabled = false;
                        this.demuxTracks.Enabled = true;
                        this.demuxVideo.Checked = false;
                        this.demuxVideo.Enabled = false;
                        IndexerUsed = IndexType.FFMS;
                        btnFFMS.Checked = true;
                        if (txtContainerInformation.Text.Trim().ToUpperInvariant().Equals("MATROSKA"))
                            this.gbAudio.Text = " Audio Demux ";
                        else
                            this.gbAudio.Text = " Audio Encoding ";
                        break;
                    }
                case IndexType.LSMASH:
                    {
                        this.output.Filter = "LSMASHIndex project files|*.lwi";
                        //this.gbOutput.Enabled = false;
                        this.demuxTracks.Enabled = true;
                        this.demuxVideo.Checked = false;
                        this.demuxVideo.Enabled = false;
                        IndexerUsed = IndexType.LSMASH;
                        btnLSMASH.Checked = true;
                        if (txtContainerInformation.Text.Trim().ToUpperInvariant().Equals("MATROSKA"))
                            this.gbAudio.Text = " Audio Demux ";
                        else
                            this.gbAudio.Text = " Audio Encoding ";
                        break;
                    }
            }
            setOutputFileName();
            recommendSettings();
            if (!demuxTracks.Checked)
                rbtracks_CheckedChanged(null, null);
        }
        #endregion
        #region buttons
        private void output_FileSelected(FileBar sender, FileBarEventArgs args)
        {
            checkIndexIO();
        }

        private void input_FileSelected(FileBar sender, FileBarEventArgs args)
        {
            if (Path.GetExtension(input.Filename.ToUpperInvariant()) == ".VOB")
            {
                // switch to IFO if possible as a VOB file is used
                string videoIFO;
                if (Path.GetFileName(input.Filename).ToUpperInvariant().Substring(0, 4) == "VTS_")
                    videoIFO = input.Filename.Substring(0, input.Filename.LastIndexOf("_")) + "_0.IFO";
                else
                    videoIFO = Path.ChangeExtension(input.Filename, ".IFO");

                if (File.Exists(videoIFO))
                    input.Filename = videoIFO;
            }

            openVideo(input.Filename);
            checkIndexIO();
        }


        private void setControlState(bool bDisableControls)
        {
            if (bDisableControls)
            {
                this.Cursor = Cursors.WaitCursor;
                gbInput.Enabled = queueButton.Enabled = gbOutput.Enabled = gbIndexer.Enabled = gbAudio.Enabled = false;
            }
            else
            {
                this.Cursor = Cursors.Default;
                gbInput.Enabled = queueButton.Enabled = true;
            }
        }

        private void openVideo(string fileName)
        {
            setControlState(true);

            this._oLog = MainForm.Instance.FileIndexerLog;
            if (_oLog == null)
            {
                _oLog = MainForm.Instance.Log.Info("FileIndexer");
                MainForm.Instance.FileIndexerLog = _oLog;
            }

            gbFileInformation.Text = " File Information ";
            iFile = null;
            if (GetDVDorBluraySource(fileName, ref iFile))
            {
                if (iFile != null)
                {
                    fileName = iFile.FileName;
                    string strText = (iFile.VideoInfo.PGCNumber > 1 ? " - PGC " + iFile.VideoInfo.PGCNumber.ToString("D2") : string.Empty) + (iFile.VideoInfo.AngleNumber > 0 ? " - Angle " + iFile.VideoInfo.AngleNumber + " " : string.Empty);
                    if (strText.Trim().Length > 0)
                        gbFileInformation.Text += strText.Trim() + " ";
                }
            }
            else
                iFile = new MediaInfoFile(fileName, ref _oLog);

            if (iFile != null && iFile.HasVideo)
            {
                strVideoCodec = iFile.VideoInfo.Track.Codec;
                strVideoScanType = iFile.VideoInfo.ScanType;
                strContainerFormat = iFile.ContainerFileTypeString;
            }
            else
                strVideoCodec = strVideoScanType = strContainerFormat = string.Empty;

            if (String.IsNullOrEmpty(strVideoCodec))
                txtCodecInformation.Text = " unknown";
            else
                txtCodecInformation.Text = " " + strVideoCodec;
            if (String.IsNullOrEmpty(strContainerFormat))
                txtContainerInformation.Text = " unknown";
            else
                txtContainerInformation.Text = " " + strContainerFormat;
            if (String.IsNullOrEmpty(strVideoScanType))
                txtScanTypeInformation.Text = " unknown";
            else
                txtScanTypeInformation.Text = " " + strVideoScanType;

            if (iFile != null && iFile.HasAudio)
                audioTracks = iFile.AudioInfo.Tracks;
            else
                audioTracks = new List<AudioTrackInfo>();

            if (input.Filename != fileName)
                input.Filename = fileName;

            generateAudioList();

            if (iFile != null)
            {
                IndexType newType = IndexType.NONE;
                iFile.recommendIndexer(out newType);
                if (newType == IndexType.D2V || newType == IndexType.DGM ||
                    newType == IndexType.DGI || newType == IndexType.FFMS ||
                    newType == IndexType.LSMASH)
                {
                    btnD2V.Enabled = iFile.isD2VIndexable();
                    btnDGM.Enabled = iFile.isDGMIndexable();
                    btnDGI.Enabled = iFile.isDGIIndexable();
                    btnFFMS.Enabled = iFile.isFFMSIndexable();
                    btnLSMASH.Enabled = iFile.isLSMASHIndexable();

                    gbIndexer.Enabled = gbAudio.Enabled = gbOutput.Enabled = true;
                    changeIndexer(newType);
                    setControlState(false);
                    return;
                }
            }

            // source file not supported
            btnD2V.Enabled = btnDGM.Enabled = btnDGI.Enabled = btnFFMS.Enabled = btnLSMASH.Enabled = false;
            gbIndexer.Enabled = gbAudio.Enabled = gbOutput.Enabled = false;
            btnFFMS.Checked = btnD2V.Checked = btnDGM.Checked = btnDGI.Checked = btnLSMASH.Checked = false;
            output.Filename = "";
            demuxNoAudiotracks.Checked = true;
            setControlState(false);
            MessageBox.Show("No indexer for this file found!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        /// <summary>
        /// Checks if the source file if from a DVD/Blu-ray stucture & asks for title list if needed
        /// </summary>
        /// <param name="fileName">input file name</param>
        /// <param name="iFileTemp">reference to the mediainfofile object</param>
        /// <returns>true if DVD/Blu-ray source is found, false if no DVD/Blu-ray source is available</returns>
        private bool GetDVDorBluraySource(string fileName, ref MediaInfoFile iFileTemp)
        {
            iFileTemp = null;
            using (frmStreamSelect frm = new frmStreamSelect(fileName, SelectionMode.One))
            {
                // check if playlists have been found
                if (frm.TitleCount == 0)
                    return false;

                // only continue if a DVD or Blu-ray structure is found
                if (!frm.IsDVDOrBluraySource)
                    return false;

                // open the selection window
                DialogResult dr = DialogResult.OK;
                dr = frm.ShowDialog();

                if (dr != DialogResult.OK)
                    return false;

                ChapterInfo oChapterInfo = frm.SelectedSingleChapterInfo;
                string strSourceFile = string.Empty;
                if (frm.IsDVDSource)
                    strSourceFile = Path.Combine(Path.GetDirectoryName(oChapterInfo.SourceFilePath), oChapterInfo.Title + "_0.IFO");
                else
                    strSourceFile = oChapterInfo.SourceFilePath;
                if (!File.Exists(strSourceFile))
                {
                    _oLog.LogEvent(strSourceFile + " cannot be found. skipping...");
                    return true;
                }
                iFileTemp = new MediaInfoFile(strSourceFile, ref _oLog, oChapterInfo.PGCNumber, oChapterInfo.AngleNumber);
            }
            return true;
        }

        private void generateAudioList()
        {
            AudioTracks.Items.Clear();
            foreach (AudioTrackInfo atrack in audioTracks)
                AudioTracks.Items.Add(atrack);
        }

        /// <summary>
        /// recommend input settings based upon the input file
        /// </summary>
        private void recommendSettings()
        {
            if (AudioTracks.Items.Count > 0)
            {
                if (IndexerUsed == IndexType.D2V)
                {
                    if (strContainerFormat.Equals("MPEG-PS") || strContainerFormat.Equals("DVD Video"))
                    {
                        demuxTracks.Enabled = true;
                    }
                    else
                    {
                        if (demuxTracks.Checked)
                            demuxAll.Checked = true;
                        demuxTracks.Enabled = false;
                    }
                }
            }
            else
            {
                demuxNoAudiotracks.Checked = true;
                demuxTracks.Enabled = false;
            }
            AudioTracks.Enabled = demuxTracks.Checked;
        }

        /// <summary>
        /// sets the output file name
        /// </summary>
        private void setOutputFileName()
        {
            if (String.IsNullOrEmpty(this.input.Filename))
                return;

            string projectPath = string.Empty;
            if (!String.IsNullOrEmpty(output.Filename))
                projectPath = Path.GetDirectoryName(output.Filename);
            else
                projectPath = FileUtil.GetOutputFolder(this.input.Filename);

            string fileNamePrefix = FileUtil.GetOutputFilePrefix(this.input.Filename);
            string fileNameNoPath = Path.GetFileName(this.input.Filename);

            string fileName = Path.GetFileNameWithoutExtension(this.input.Filename);
            if (Path.GetExtension(this.input.Filename).ToUpperInvariant().Equals(".IFO") 
                && FileUtil.RegExMatch(fileName, @"_\d{1,2}\z", false))
            {
                // DVD structure found &&
                // file ends with e.g. _1 as in VTS_01_1
                fileName = fileName.Substring(0, fileName.LastIndexOf('_') + 1) + iFile.VideoInfo.PGCNumber;
                if (iFile.VideoInfo.AngleNumber > 0)
                    fileName += "_" + iFile.VideoInfo.AngleNumber;

            }
            fileName = fileNamePrefix + fileName + Path.GetExtension(this.input.Filename).ToLowerInvariant();

            switch (IndexerUsed)
            {
                case IndexType.D2V: output.Filename = Path.Combine(projectPath, Path.ChangeExtension(fileName, ".d2v")); break;
                case IndexType.DGM:
                case IndexType.DGI: output.Filename = Path.Combine(projectPath, Path.ChangeExtension(fileName, ".dgi")); break;
                case IndexType.FFMS:
                    if (Path.GetExtension(fileName).Equals(".mpls", StringComparison.InvariantCultureIgnoreCase))
                        output.Filename = Path.Combine(projectPath, Path.ChangeExtension(fileName, ".ffindex"));
                    else
                        output.Filename = Path.Combine(projectPath, fileName + ".ffindex");
                    break;
                case IndexType.LSMASH:
                    if (Path.GetExtension(fileName).Equals(".mpls", StringComparison.InvariantCultureIgnoreCase))
                        output.Filename = Path.Combine(projectPath, Path.ChangeExtension(fileName, ".lwi"));
                    else
                        output.Filename = Path.Combine(projectPath, fileName + ".lwi");
                    break;
            }
        }

        /// <summary>
        /// creates a project
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void queueButton_Click(object sender, System.EventArgs e)
        {
            if (dialogMode)
                return;

            if (!configured)
            {
                MessageBox.Show("You must select the input and output file to continue",
                    "Configuration incomplete", MessageBoxButtons.OK);
                return;
            }

            if (!Drives.ableToWriteOnThisDrive(Path.GetPathRoot(output.Filename)))
            {
                MessageBox.Show("MeGUI cannot write on the disc " + Path.GetPathRoot(output.Filename) + "\n" +
                    "Please, select another output path to save your project...", "Configuration Incomplete", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            JobChain prepareJobs = null;
            string videoInput = input.Filename;

            // create pgcdemux job if needed
            if (Path.GetExtension(input.Filename).ToUpperInvariant().Equals(".IFO"))
            {
                if (iFile.VideoInfo.PGCCount > 1 || iFile.VideoInfo.AngleCount > 0)
                {
                    // pgcdemux must be used as either multiple PGCs or a multi-angle disc are found
                    string tempFile = Path.Combine(Path.GetDirectoryName(output.Filename), Path.GetFileNameWithoutExtension(output.Filename) + "_1.VOB");
                    prepareJobs = new SequentialChain(new PgcDemuxJob(videoInput, tempFile, iFile.VideoInfo.PGCNumber, iFile.VideoInfo.AngleNumber));
                    videoInput = tempFile;

                    string filesToOverwrite = string.Empty;
                    for (int i = 1; i < 10; i++)
                    {
                        string file = Path.Combine(Path.GetDirectoryName(output.Filename), Path.GetFileNameWithoutExtension(output.Filename) + "_" + i + ".VOB");
                        if (File.Exists(file))
                        {
                            filesToOverwrite += file + "\n";
                        }
                    }
                    if (!String.IsNullOrEmpty(filesToOverwrite))
                    {
                        DialogResult dr = MessageBox.Show("The pgc demux files already exist: \n" + filesToOverwrite + "\n" +
                                                            "Do you want to overwrite these files?", "Configuration Incomplete",
                                                            MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (dr == DialogResult.No)
                            return;
                    }
                }
                else
                {
                    // change from _0.IFO to _1.VOB for the indexer
                    videoInput = input.Filename.Substring(0, input.Filename.Length - 5) + "1.VOB";
                    if (!File.Exists(videoInput))
                    {
                        MessageBox.Show("The file following file cannot be found: \n" + videoInput , "Configuration Incomplete", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
            }
            else if (Path.GetExtension(input.Filename).ToUpperInvariant().Equals(".MPLS") && IndexerUsed != IndexType.DGI && IndexerUsed != IndexType.DGM)
            {
                // blu-ray playlist without DGI/DGM used - therefore eac3to must be used first

                string strTempMKVFile = Path.Combine(Path.GetDirectoryName(output.Filename), Path.GetFileNameWithoutExtension(output.Filename) + ".mkv");
                if (File.Exists(strTempMKVFile))
                {
                    DialogResult dr = MessageBox.Show("The demux file already exist: \n" + strTempMKVFile + "\n" +
                                                        "Do you want to overwrite this file?", "Configuration Incomplete",
                                                        MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (dr == DialogResult.No)
                        return;
                }

                StringBuilder sb = new StringBuilder();
                sb.Append(string.Format("{0}:\"{1}\" ", iFile.VideoInfo.Track.TrackID, strTempMKVFile));

                foreach (AudioTrackInfo oStreamControl in AudioTracks.CheckedItems)
                {
                    bool bCoreOnly = false;
                    AudioCodec audioCodec = oStreamControl.AudioCodec;
                    if (oStreamControl.HasCore && !MainForm.Instance.Settings.Eac3toDefaultToHD)
                    {
                        // audio file can be demuxed && should be touched (core needed)
                        if (audioCodec == AudioCodec.THDAC3)
                        {
                            oStreamControl.Codec = "AC-3";
                            oStreamControl.AudioType = AudioType.AC3;
                            oStreamControl.AudioCodec = AudioCodec.AC3;
                            bCoreOnly = true;
                            oStreamControl.HasCore = false;
                        }
                        else if (audioCodec == AudioCodec.DTS)
                        {
                            oStreamControl.Codec = "DTS";
                            oStreamControl.AudioType = AudioType.DTS;
                            oStreamControl.AudioCodec = AudioCodec.DTS;
                            oStreamControl.HasCore = false;
                            bCoreOnly = true;
                        }
                    }

                    // core must be extracted (workaround for an eac3to issue)
                    // http://bugs.madshi.net/view.php?id=450
                    if (audioCodec == AudioCodec.EAC3)
                        bCoreOnly = true;

                    string strSourceFileName = Path.Combine(Path.GetDirectoryName(input.Filename), Path.GetFileNameWithoutExtension(strTempMKVFile));

                    oStreamControl.SourceFileName = strSourceFileName;

                    sb.Append(string.Format("{0}:\"{1}\" ", oStreamControl.TrackID, Path.Combine(Path.GetDirectoryName(output.Filename), oStreamControl.DemuxFileName)));
                    if (bCoreOnly)
                        sb.Append("-core ");
                }

                HDStreamsExJob oJob = new HDStreamsExJob(new List<string>() { input.Filename }, Path.GetDirectoryName(output.Filename), null, sb.ToString(), 2);
                prepareJobs = new SequentialChain(oJob);
                videoInput = strTempMKVFile;
            }

            switch (IndexerUsed)
            {
                case IndexType.D2V:
                    {
                        prepareJobs = new SequentialChain(prepareJobs, new SequentialChain(generateD2VIndexJob(videoInput)));
                        MainForm.Instance.Jobs.AddJobsWithDependencies(prepareJobs, true);
                        if (this.closeOnQueue.Checked)
                            this.Close();
                        break;
                    }
                case IndexType.DGI:
                    {
                        prepareJobs = new SequentialChain(prepareJobs, new SequentialChain(generateDGNVIndexJob(videoInput)));
                        MainForm.Instance.Jobs.AddJobsWithDependencies(prepareJobs, true);
                        if (this.closeOnQueue.Checked)
                            this.Close();
                        break;
                    }
                case IndexType.DGM:
                    {
                        prepareJobs = new SequentialChain(prepareJobs, new SequentialChain(generateDGMIndexJob(videoInput)));
                        MainForm.Instance.Jobs.AddJobsWithDependencies(prepareJobs, true);
                        if (this.closeOnQueue.Checked)
                            this.Close();
                        break;
                    }
                case IndexType.FFMS:
                    {
                        FFMSIndexJob job = generateFFMSIndexJob(videoInput);
                        if (job.DemuxMode > 0 && job.AudioTracks.Count > 0 &&
                            (txtContainerInformation.Text.Trim().ToUpperInvariant().Equals("MATROSKA")
                            || txtContainerInformation.Text.Trim().ToUpperInvariant().Equals("BLU-RAY PLAYLIST")))
                        {
                            job.DemuxMode = 0;
                            job.AudioTracksDemux = job.AudioTracks;
                            job.AudioTracks = new List<AudioTrackInfo>();
                            if (txtContainerInformation.Text.Trim().ToUpperInvariant().Equals("MATROSKA"))
                            {
                                MkvExtractJob extractJob = new MkvExtractJob(videoInput, Path.GetDirectoryName(this.output.Filename), job.AudioTracksDemux);
                                prepareJobs = new SequentialChain(prepareJobs, new SequentialChain(extractJob));
                            }
                        }
                        prepareJobs = new SequentialChain(prepareJobs, new SequentialChain(job));
                        MainForm.Instance.Jobs.AddJobsWithDependencies(prepareJobs, true);
                        if (this.closeOnQueue.Checked)
                            this.Close();
                        break;
                    }
                case IndexType.LSMASH:
                    {
                        LSMASHIndexJob job = generateLSMASHIndexJob(videoInput);
                        if (job.DemuxMode > 0 && job.AudioTracks.Count > 0 &&
                            (txtContainerInformation.Text.Trim().ToUpperInvariant().Equals("MATROSKA")
                            || txtContainerInformation.Text.Trim().ToUpperInvariant().Equals("BLU-RAY PLAYLIST")))
                        {
                            job.DemuxMode = 0;
                            job.AudioTracksDemux = job.AudioTracks;
                            job.AudioTracks = new List<AudioTrackInfo>();
                            if (txtContainerInformation.Text.Trim().ToUpperInvariant().Equals("MATROSKA"))
                            {
                                MkvExtractJob extractJob = new MkvExtractJob(videoInput, Path.GetDirectoryName(this.output.Filename), job.AudioTracksDemux);
                                prepareJobs = new SequentialChain(prepareJobs, new SequentialChain(extractJob));
                            }
                        }
                        prepareJobs = new SequentialChain(prepareJobs, new SequentialChain(job));
                        MainForm.Instance.Jobs.AddJobsWithDependencies(prepareJobs, true);
                        if (this.closeOnQueue.Checked)
                            this.Close();
                        break;
                    }
            }              
        }
        #endregion
        #region helper methods
        private void checkIndexIO()
        {
            configured = (!input.Filename.Equals("") && !output.Filename.Equals(""));
            if (configured && dialogMode)
                queueButton.DialogResult = DialogResult.OK;
            else
                queueButton.DialogResult = DialogResult.None;
        }

        public static bool isDGMFile(string input)
        {
            if (!File.Exists(input))
                return false;

            using (StreamReader sr = new StreamReader(input, System.Text.Encoding.Default))
            {
                string line = sr.ReadLine();
                if (line.ToLower().Contains("dgindexim"))
                    return true;
                else
                    return false;
            }
        }

        private D2VIndexJob generateD2VIndexJob(string videoInput)
        {
            int demuxType = 0;
            if (demuxTracks.Checked)
                demuxType = 1;
            else if (demuxNoAudiotracks.Checked)
                demuxType = 0;
            else
                demuxType = 2;

            List<AudioTrackInfo> audioTracks = new List<AudioTrackInfo>();
            foreach (AudioTrackInfo ati in AudioTracks.CheckedItems)
                audioTracks.Add(ati);

            return new D2VIndexJob(videoInput, this.output.Filename, demuxType, audioTracks, loadOnComplete.Checked, demuxVideo.Checked);
        }

        private DGIIndexJob generateDGNVIndexJob(string videoInput)
        {
            int demuxType = 0;
            if (demuxTracks.Checked)
                demuxType = 1;
            else if (demuxNoAudiotracks.Checked)
                demuxType = 0;
            else
                demuxType = 2;

            List<AudioTrackInfo> audioTracks = new List<AudioTrackInfo>();
            foreach (AudioTrackInfo ati in AudioTracks.CheckedItems)
                audioTracks.Add(ati);

            return new DGIIndexJob(videoInput, this.output.Filename, demuxType, audioTracks, loadOnComplete.Checked, demuxVideo.Checked, false);
        }

        private DGMIndexJob generateDGMIndexJob(string videoInput)
        {
            int demuxType = 0;
            if (demuxTracks.Checked)
                demuxType = 1;
            else if (demuxNoAudiotracks.Checked)
                demuxType = 0;
            else
                demuxType = 2;

            List<AudioTrackInfo> audioTracks = new List<AudioTrackInfo>();
            foreach (AudioTrackInfo ati in AudioTracks.CheckedItems)
                audioTracks.Add(ati);

            return new DGMIndexJob(videoInput, this.output.Filename, demuxType, audioTracks, loadOnComplete.Checked, demuxVideo.Checked, false);
        }

        private FFMSIndexJob generateFFMSIndexJob(string videoInput)
        {
            int demuxType = 0;
            if (demuxTracks.Checked)
                demuxType = 1;
            else if (demuxNoAudiotracks.Checked)
                demuxType = 0;
            else
                demuxType = 2;

            List<AudioTrackInfo> audioTracks = new List<AudioTrackInfo>();
            foreach (AudioTrackInfo ati in AudioTracks.CheckedItems)
                audioTracks.Add(ati);

            return new FFMSIndexJob(videoInput, output.Filename, demuxType, audioTracks, loadOnComplete.Checked);
        }

        private LSMASHIndexJob generateLSMASHIndexJob(string videoInput)
        {
            int demuxType = 0;
            if (demuxTracks.Checked)
                demuxType = 1;
            else if (demuxNoAudiotracks.Checked)
                demuxType = 0;
            else
                demuxType = 2;

            List<AudioTrackInfo> audioTracks = new List<AudioTrackInfo>();
            foreach (AudioTrackInfo ati in AudioTracks.CheckedItems)
                audioTracks.Add(ati);

            return new LSMASHIndexJob(videoInput, output.Filename, demuxType, audioTracks, loadOnComplete.Checked);
        }
        #endregion

        private void rbtracks_CheckedChanged(object sender, EventArgs e)
        {
            // Now defaults to starting with every track selected
            for (int i = 0; i < AudioTracks.Items.Count; i++)
                AudioTracks.SetItemChecked(i, !demuxNoAudiotracks.Checked);
            AudioTracks.Enabled = demuxTracks.Checked;
        }

        private void btnFFMS_Click(object sender, EventArgs e)
        {
            changeIndexer(IndexType.FFMS);
        }

        private void btnDGI_Click(object sender, EventArgs e)
        {
            changeIndexer(IndexType.DGI);
        }

        private void btnDGM_Click(object sender, EventArgs e)
        {
            changeIndexer(IndexType.DGM);
        }

        private void btnD2V_Click(object sender, EventArgs e)
        {
            changeIndexer(IndexType.D2V);
        }

        private void btnLSMASH_Click(object sender, EventArgs e)
        {
            changeIndexer(IndexType.LSMASH);
        }
    }

    public class D2VCreatorTool : MeGUI.core.plugins.interfaces.ITool
    {

        #region ITool Members

        public string Name
        {
            get { return "File Indexer"; }
        }

        public void Run(MainForm info)
        {
            new FileIndexerWindow(info).Show();

        }

        public Shortcut[] Shortcuts
        {
            get { return new Shortcut[] { Shortcut.Ctrl2 }; }
        }

        #endregion

        #region IIDable Members

        public string ID
        {
            get { return "d2v_creator"; }
        }

        #endregion
    }

    public class d2vIndexJobPostProcessor
    {
        public static JobPostProcessor PostProcessor = new JobPostProcessor(postprocess, "D2V_postprocessor");
        private static LogItem postprocess(MainForm mainForm, Job ajob)
        {
            if (!(ajob is D2VIndexJob)) return null;
            D2VIndexJob job = (D2VIndexJob)ajob;

            StringBuilder logBuilder = new StringBuilder();
            List<string> arrFilesToDelete = new List<string>();
            Dictionary<int, string> audioFiles = VideoUtil.getAllDemuxedAudio(job.AudioTracks, new List<AudioTrackInfo>(), out arrFilesToDelete, job.Output, null);
            if (job.LoadSources)
            {
                if (audioFiles.Count > 0)
                {
                    string[] files = new string[audioFiles.Values.Count];
                    audioFiles.Values.CopyTo(files, 0);
                    Util.ThreadSafeRun(mainForm, new MethodInvoker(
                        delegate
                        {
                            mainForm.Audio.openAudioFile(files);
                        }));
                }
                // if the above needed delegation for openAudioFile this needs it for openVideoFile?
                // It seems to fix the problem of ASW dissapearing as soon as it appears on a system (Vista X64)
                Util.ThreadSafeRun(mainForm, new MethodInvoker(
                    delegate
                    {
                        AviSynthWindow asw = new AviSynthWindow(mainForm, job.Output);
                        asw.OpenScript += new OpenScriptCallback(mainForm.Video.openVideoFile);
                        asw.Show();
                    }));
            }

            return null;
        }
    }

    public class dgiIndexJobPostProcessor
    {
        public static JobPostProcessor PostProcessor = new JobPostProcessor(postprocess, "Dgi_postprocessor");
        private static LogItem postprocess(MainForm mainForm, Job ajob)
        {
            if (!(ajob is DGIIndexJob))
                return null;
            DGIIndexJob job = (DGIIndexJob)ajob;

            StringBuilder logBuilder = new StringBuilder();
            Dictionary<int, string> audioFiles = new Dictionary<int, string>();

            if ((job.AudioTracks != null && job.AudioTracks.Count > 0) || job.DemuxMode > 0)
            {
                List<string> arrFilesToDelete = new List<string>();
                audioFiles = AudioUtil.GetAllDemuxedAudioFromDGI(job.AudioTracks, out arrFilesToDelete, job.Output, null);
                job.FilesToDelete.AddRange(arrFilesToDelete);
            }

            if (!job.OneClickProcessing)
                job.FilesToDelete.Add(Path.ChangeExtension(job.Output, ".log"));
            if (!Path.ChangeExtension(job.Output, ".log").Equals(Path.ChangeExtension(job.Input, ".log")))
                job.FilesToDelete.Add(Path.ChangeExtension(job.Input, ".log"));

            if (job.LoadSources)
            {
                if (audioFiles.Count > 0)
                {
                    string[] files = new string[audioFiles.Values.Count];
                    audioFiles.Values.CopyTo(files, 0);
                    Util.ThreadSafeRun(mainForm, new MethodInvoker(
                        delegate
                        {
                            mainForm.Audio.openAudioFile(files);
                        }));
                }
                // if the above needed delegation for openAudioFile this needs it for openVideoFile?
                // It seems to fix the problem of ASW dissapearing as soon as it appears on a system (Vista X64)
                Util.ThreadSafeRun(mainForm, new MethodInvoker(
                    delegate
                    {
                        AviSynthWindow asw = new AviSynthWindow(mainForm, job.Output);
                        asw.OpenScript += new OpenScriptCallback(mainForm.Video.openVideoFile);
                        asw.Show();
                    }));
            }

            return null;
        }
    }

    public class dgmIndexJobPostProcessor
    {
        public static JobPostProcessor PostProcessor = new JobPostProcessor(postprocess, "Dgm_postprocessor");
        private static LogItem postprocess(MainForm mainForm, Job ajob)
        {
            if (!(ajob is DGMIndexJob))
                return null;
            DGMIndexJob job = (DGMIndexJob)ajob;

            StringBuilder logBuilder = new StringBuilder();
            Dictionary<int, string> audioFiles = new Dictionary<int, string>();

            if ((job.AudioTracks != null && job.AudioTracks.Count > 0) || job.DemuxMode > 0)
            {
                List<string> arrFilesToDelete = new List<string>();
                audioFiles = AudioUtil.GetAllDemuxedAudioFromDGI(job.AudioTracks, out arrFilesToDelete, job.Output, null);
                job.FilesToDelete.AddRange(arrFilesToDelete);
            }

            if (!job.OneClickProcessing)
                job.FilesToDelete.Add(Path.ChangeExtension(job.Output, ".log"));
            if (!Path.ChangeExtension(job.Output, ".log").Equals(Path.ChangeExtension(job.Input, ".log")))
                job.FilesToDelete.Add(Path.ChangeExtension(job.Input, ".log"));

            if (job.LoadSources)
            {
                if (audioFiles.Count > 0)
                {
                    string[] files = new string[audioFiles.Values.Count];
                    audioFiles.Values.CopyTo(files, 0);
                    Util.ThreadSafeRun(mainForm, new MethodInvoker(
                        delegate
                        {
                            mainForm.Audio.openAudioFile(files);
                        }));
                }
                // if the above needed delegation for openAudioFile this needs it for openVideoFile?
                // It seems to fix the problem of ASW dissapearing as soon as it appears on a system (Vista X64)
                Util.ThreadSafeRun(mainForm, new MethodInvoker(
                    delegate
                    {
                        AviSynthWindow asw = new AviSynthWindow(mainForm, job.Output);
                        asw.OpenScript += new OpenScriptCallback(mainForm.Video.openVideoFile);
                        asw.Show();
                    }));
            }

            return null;
        }
    }

    public class ffmsIndexJobPostProcessor
    {
        public static JobPostProcessor PostProcessor = new JobPostProcessor(postprocess, "FFMS_postprocessor");
        private static LogItem postprocess(MainForm mainForm, Job ajob)
        {
            if (!(ajob is FFMSIndexJob)) return null;
            FFMSIndexJob job = (FFMSIndexJob)ajob;

            StringBuilder logBuilder = new StringBuilder();
            List<string> arrFilesToDelete = new List<string>();
            Dictionary<int, string> audioFiles = VideoUtil.getAllDemuxedAudio(job.AudioTracks, job.AudioTracksDemux, out arrFilesToDelete, job.Output, null);
            if (job.LoadSources)
            {
                if (audioFiles.Count > 0)
                {
                    string[] files = new string[audioFiles.Values.Count];
                    audioFiles.Values.CopyTo(files, 0);
                    Util.ThreadSafeRun(mainForm, new MethodInvoker(
                        delegate
                        {
                            mainForm.Audio.openAudioFile(files);
                        }));
                }
                // if the above needed delegation for openAudioFile this needs it for openVideoFile?
                // It seems to fix the problem of ASW dissapearing as soon as it appears on a system (Vista X64)
                Util.ThreadSafeRun(mainForm, new MethodInvoker(
                    delegate
                    {
                        AviSynthWindow asw = new AviSynthWindow(mainForm, job.Input, job.Output);
                        asw.OpenScript += new OpenScriptCallback(mainForm.Video.openVideoFile);
                        asw.Show();
                    }));
            }

            return null;
        }
    }

    public class lsmashIndexJobPostProcessor
    {
        public static JobPostProcessor PostProcessor = new JobPostProcessor(postprocess, "LSMASH_postprocessor");
        private static LogItem postprocess(MainForm mainForm, Job ajob)
        {
            if (!(ajob is LSMASHIndexJob)) 
                return null;
            LSMASHIndexJob job = (LSMASHIndexJob)ajob;

            StringBuilder logBuilder = new StringBuilder();
            List<string> arrFilesToDelete = new List<string>();
            Dictionary<int, string> audioFiles = VideoUtil.getAllDemuxedAudio(job.AudioTracks, job.AudioTracksDemux, out arrFilesToDelete, job.Output, null);
            if (job.LoadSources)
            {
                if (audioFiles.Count > 0)
                {
                    string[] files = new string[audioFiles.Values.Count];
                    audioFiles.Values.CopyTo(files, 0);
                    Util.ThreadSafeRun(mainForm, new MethodInvoker(
                        delegate
                        {
                            mainForm.Audio.openAudioFile(files);
                        }));
                }
                // if the above needed delegation for openAudioFile this needs it for openVideoFile?
                // It seems to fix the problem of ASW dissapearing as soon as it appears on a system (Vista X64)
                Util.ThreadSafeRun(mainForm, new MethodInvoker(
                    delegate
                    {
                        AviSynthWindow asw = new AviSynthWindow(mainForm, job.Input, job.Output);
                        asw.OpenScript += new OpenScriptCallback(mainForm.Video.openVideoFile);
                        asw.Show();
                    }));
            }

            return null;
        }
    }

    public delegate void ProjectCreationComplete(); // this event is fired when the dgindex thread finishes
}