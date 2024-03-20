// ****************************************************************************
// 
// Copyright (C) 2005-2024 Doom9 & al
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
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Windows.Forms;

using MeGUI.core.util;

namespace MeGUI
{

    /// <summary>
	/// Summary description for AviSynthWindow.
	/// </summary>
	public partial class AviSynthWindow : Form
	{
		#region variable declaration
        private string originalScript;
        private bool originalInlineAvs;
        private bool isPreviewMode = false;
        private bool eventsOn = true;
		private VideoPlayer player;
        private IMediaFile file;
        private IVideoReader reader;
		public event OpenScriptCallback OpenScript;
        private Dar? suggestedDar;
        private MainForm mainForm;
        private PossibleSources sourceType;
        private SourceDetector detector;
        private string indexFile;
        private int scriptRefresh = 1; // >= 1 enabled; < 1 disabled
        private bool bAllowUpsizing;
        private LogItem _oLog;
		#endregion

		#region construction/deconstruction
        public AviSynthWindow(MainForm mainForm)
        {
            scriptRefresh--;
            eventsOn = false;
            this.mainForm = mainForm;
			InitializeComponent();

            lblAspectError.Size = new System.Drawing.Size(MainForm.Instance.Settings.DPIRescale(75), MainForm.Instance.Settings.DPIRescale(21));

            this.controlsToDisable = new List<Control>
            {
                reopenOriginal,
                filtersGroupbox,
                deinterlacingGroupBox,
                mpegOptGroupBox,
                aviOptGroupBox,
                resNCropGroupbox,
                previewAvsButton,
                saveButton,
                arChooser,
                inputDARLabel,
                signalAR,
                avisynthScript,
                openDLLButton,
                dgOptions
            };
            EnableControls(false);

            this.resizeFilterType.Items.Clear();
            this.resizeFilterType.DataSource = ScriptServer.ListOfResizeFilterType;
            this.resizeFilterType.BindingContext = new BindingContext();
            this.noiseFilterType.Items.Clear();
            this.noiseFilterType.DataSource = ScriptServer.ListOfDenoiseFilterType;
            this.noiseFilterType.BindingContext = new BindingContext();
            this.deintFieldOrder.Items.Clear();
            this.deintFieldOrder.DataSource = ScriptServer.ListOfFieldOrders;
            this.deintFieldOrder.BindingContext = new BindingContext();
            this.deintSourceType.Items.Clear();
            this.deintSourceType.DataSource = ScriptServer.ListOfSourceTypes;
            this.deintSourceType.BindingContext = new BindingContext();
            this.cbNvDeInt.Items.Clear();
            this.cbNvDeInt.DataSource = ScriptServer.ListOfNvDeIntType;
            this.cbNvDeInt.BindingContext = new BindingContext();

            sourceType = PossibleSources.directShow;
            deintFieldOrder.SelectedIndex = -1;
            deintSourceType.SelectedIndex = -1;
            cbNvDeInt.SelectedIndex = 0;
            cbCharset.SelectedIndex = 0;
            modValueBox.SelectedIndex = 0;
            bAllowUpsizing = false;

            this.originalScript = String.Empty;
            this.isPreviewMode = false;

			player = null;
			this.crop.Checked = false;
			this.cropLeft.Value = 0;
			this.cropTop.Value = 0;
			this.cropRight.Value = 0;
			this.cropBottom.Value = 0;

            deinterlaceType.DataSource = new DeinterlaceFilter[] { new DeinterlaceFilter("Do nothing (source not detected)", "#blank deinterlace line") };

            avsProfile.Manager = MainForm.Instance.Profiles;

            // set filters

            string filter = string.Empty;
            if (MainForm.Instance.Settings.IsDGIIndexerAvailable() || MainForm.Instance.Settings.IsDGMIndexerAvailable())
            {
                filter += "All AviSynth script files|*.avs";
                filter += "|All index files|*.d2v;*.dgi;*.ffindex;*.lwi";
                filter += "|All indexable files|*.264;*.avc;*.265;*.hevc;*.avi;*.flv;*.h264;*.265;*.hevc;*.ifo;*.m1v;*.m2t;*.m2ts;*.m2v;*.mkv;*.mp4;*.mpeg;*.mpg;*.mpls;*.mpv;*.mts;*.ogm;*.pva;*.tp;*.trp;*.ts;*.vc1;*.vob;*.vro;*.wmv";
                filter += "|All suported files|*.264;*.265;*.hevc;*.avc;*.avi;*.avs;*.d2v;*.dgi;*.ffindex;*.flv;*.h264;*.ifo;*.lwi;*.m1v;*.m2t*;*.m2ts;*.m2v;*.mkv;*.mp4;*.mpeg;*.mpg;*.mpls;*.mpv;*.mts;*.ogm;*.pva;*.tp;*.trp;*.ts;*.vc1;*.vdr;*.vob;*.vro;*.wmv";
            }
            else
            {
                filter += "All AviSynth script files|*.avs";
                filter += "|All index files|*.d2v;*.ffindex;*.lwi";
                filter += "|All indexable files|*.264;*.avc;*.avi;*.flv;*.h264;*.ifo;*.m1v;*.m2t;*.m2ts;*.m2v;*.mkv;*.mp4;*.mpeg;*.mpg;*.mpls;*.mpv;*.mts;*.ogm;*.pva;*.tp;*.trp;*.ts;*.vob;*.vro;*.wmv";
                filter += "|All suported files|*.264;*.avc;*.avi;*.avs;*.d2v;*.ffindex;*.flv;*.h264;*.ifo;*.lwi;*.m1v;*.m2t*;*.m2ts;*.m2v;*.mkv;*.mp4;*.mpeg;*.mpg;*.mpls;*.mpv;*.mts;*.ogm;*.pva;*.tp;*.trp;*.ts;*.vdr;*.vob;*.vro;*.wmv";
            }
            filter += "|All files|*.*";
            input.Filter = filter;
            input.FilterIndex = 4;

            eventsOn = true;
            UpdateEverything(true, true, resize.Checked);
		}

        void ProfileChanged(object sender, EventArgs e)
        {
            this.Settings = GetProfileSettings();
        }

		/// <summary>
		/// constructor that first initializes everything using the default constructor
		/// then opens a preview window with the video given as parameter
		/// </summary>
		/// <param name="videoInput">the DGIndex script to be loaded</param>
		public AviSynthWindow(MainForm mainForm, string videoInput) : this(mainForm)
		{
            if (String.IsNullOrEmpty(videoInput))
            return;
            
            scriptRefresh--;
            OpenVideoSource(videoInput, null);
            UpdateEverything(true, true, resize.Checked);
		}

        public AviSynthWindow(MainForm mainForm, string videoInput, string indexFile)
            : this(mainForm)
        {
            if (String.IsNullOrEmpty(videoInput))
                return;
            
            scriptRefresh--;
            OpenVideoSource(videoInput, indexFile);
            UpdateEverything(true, true, resize.Checked);
        }

		protected override void OnClosing(CancelEventArgs e)
		{
            player?.Close();
            detector?.Stop();
            detector = null;
			base.OnClosing (e);
		}
		#endregion

        #region buttons
        private void input_FileSelected(FileBar sender, FileBarEventArgs args)
        {
            scriptRefresh--;
            OpenVideoSource(input.Filename, null);
            UpdateEverything(true, true, resize.Checked);
		}

		private void openDLLButton_Click(object sender, System.EventArgs e)
		{
            this.openFilterDialog.InitialDirectory = MainForm.Instance.Settings.AvisynthPluginsPath;
			if (this.openFilterDialog.ShowDialog() == DialogResult.OK)
			{
				dllPath.Text = openFilterDialog.FileName;
                string temp = avisynthScript.Text;
                StringBuilder script = new StringBuilder();
				script.Append("LoadPlugin(\"" + openFilterDialog.FileName + "\")\r\n");
				script.Append(temp);
				avisynthScript.Text = script.ToString();
			}
		}

		private void previewButton_Click(object sender, System.EventArgs e)
		{
            // If the player is null, create a new one.
            // Otherwise use the existing player to load the latest preview.
            if (player == null || player.IsDisposed) 
                player = new VideoPlayer();

			if (player.LoadVideo(avisynthScript.Text, PREVIEWTYPE.REGULAR, false, true, player.CurrentFrame, true))
			{
				player.disableIntroAndCredits();
                reader = player.Reader;
                isPreviewMode = true;
                SendCropValues();
                if (this.Visible)
                    player.Show();
                player.SetScreenSize();
                this.TopMost = player.TopMost = true;
                if (!mainForm.Settings.AlwaysOnTop)
                    this.TopMost = player.TopMost = false;
			}
		}
		
        private void saveButton_Click(object sender, System.EventArgs e)
		{
            string fileName = videoOutput.Filename;
            WriteScript(fileName);
			if (onSaveLoadScript.Checked)
			{
                player?.Close();
                this.Close();
                OpenScript(fileName, null);
            }
		}
		#endregion

		#region script generation
		private string GenerateScript()
		{			
			string inputLine = "#input";
			string deinterlaceLines = "#deinterlace";
			string denoiseLines = "#denoise";
			string cropLine = "#crop";
			string resizeLine = "#resize";

            // input
            if (!String.IsNullOrEmpty(this.input.Filename) && File.Exists(this.input.Filename))
                inputLine = GetInputLine();

            // deinterlace
            if (deinterlace.Checked && deinterlaceType.SelectedItem is DeinterlaceFilter)
                deinterlaceLines = ((DeinterlaceFilter)deinterlaceType.SelectedItem).Script;

            // crop
            if ((!dgOptions.Enabled || !nvResize.Checked) && crop.Checked)
                cropLine = ScriptServer.GetCropLine(Cropping);
            
            // resize
            if ((!dgOptions.Enabled || !nvResize.Checked) && horizontalResolution.Value > 0 && verticalResolution.Value > 0)
            {
                int iWidth = (int)horizontalResolution.Maximum;
                int iHeight = (int)verticalResolution.Maximum;
                if (file != null)
                {
                    iWidth = (int)file.VideoInfo.Width;
                    iHeight = (int)file.VideoInfo.Height;
                }
                resizeLine = ScriptServer.GetResizeLine(resize.Checked, (int)horizontalResolution.Value, (int)verticalResolution.Value, 0, 0, (ResizeFilterType)(resizeFilterType.SelectedItem as EnumProxy).RealValue,
                                                        crop.Checked, Cropping, iWidth, iHeight);
            }

            // denoise
            denoiseLines = ScriptServer.GetDenoiseLines(noiseFilter.Checked, (DenoiseFilterType)(noiseFilterType.SelectedItem as EnumProxy).RealValue);

            // final script
            string newScript = ScriptServer.CreateScriptFromTemplate(GetProfileSettings().Template, inputLine, cropLine, resizeLine, denoiseLines, deinterlaceLines);

            if (this.signalAR.Checked && suggestedDar.HasValue)
                newScript = string.Format("# Set DAR in encoder to {0} : {1}. The following line is for automatic signalling\r\nglobal MeGUI_darx = {0}\r\nglobal MeGUI_dary = {1}\r\n",
                    suggestedDar.Value.X, suggestedDar.Value.Y) + newScript;

            if (!string.IsNullOrEmpty(this.SubtitlesPath.Text))
            {
                newScript += "\r\nLoadPlugin(\"" + Path.Combine(MainForm.Instance.Settings.AvisynthPluginsPath, "VSFilter.dll") + "\")";
                if (cbCharset.Enabled)
                {
                    string charset = CharsetValue();
                    newScript += "\r\nTextSub(\"" + SubtitlesPath.Text + "\"" + ", " + charset + ")\r\n";
                }
                else
                    newScript += "\r\nVobSub(\"" + SubtitlesPath.Text + "\")\r\n";
            }
            return newScript;
		}

        private string _tempInputLine;
        private string _tempInputFileName, _tempInputIndexFile;
        private bool _tempDeinterlacer, _tempColourCorrect, _tempMpeg2Deblocking, _tempFlipVertical, _tempDSS2, _tempNvDeint, _tempResize, _tempNvResize, _tempCrop;
        private PossibleSources _tempInputSourceType;
        private NvDeinterlacerType _tempNvDeintType;
        private CropValues _tempCropValues;
        private double _tempFPS;
        private decimal _tempNvHorizontalResolution, _tempNvVerticalResolution;
        private string GetInputLine()
        {
            if (_tempInputFileName != this.input.Filename || _tempInputIndexFile != this.indexFile || _tempDeinterlacer != deinterlace.Checked || _tempResize != resize.Checked ||
                _tempInputSourceType != sourceType || _tempColourCorrect != colourCorrect.Checked || _tempMpeg2Deblocking != mpeg2Deblocking.Checked ||
                _tempFlipVertical != flipVertical.Checked || _tempFPS != (double)fpsBox.Value || _tempDSS2 != dss2.Checked || _tempNvDeint != nvDeInt.Checked ||
                _tempNvDeintType != (NvDeinterlacerType)(cbNvDeInt.SelectedItem as EnumProxy).RealValue || _tempNvResize != nvResize.Checked ||  _tempCrop != crop.Checked ||
                (nvResize.Checked && resize.Checked && (_tempNvHorizontalResolution != horizontalResolution.Value || _tempNvVerticalResolution != verticalResolution.Value)) ||
                (nvResize.Checked && crop.Checked && _tempCropValues != Cropping))
            {
                _tempInputFileName = this.input.Filename;
                _tempInputIndexFile = this.indexFile;
                _tempDeinterlacer = deinterlace.Checked;
                _tempInputSourceType = sourceType;
                _tempColourCorrect = colourCorrect.Checked;
                _tempMpeg2Deblocking = mpeg2Deblocking.Checked;
                _tempFlipVertical = flipVertical.Checked;
                _tempFPS = (double)fpsBox.Value;
                _tempDSS2 = dss2.Checked;
                _tempNvDeint = nvDeInt.Checked;
                _tempNvDeintType = (NvDeinterlacerType)(cbNvDeInt.SelectedItem as EnumProxy).RealValue;
                _tempResize = resize.Checked;
                _tempNvResize = nvResize.Checked;
                _tempNvHorizontalResolution = horizontalResolution.Value;
                _tempNvVerticalResolution = verticalResolution.Value;
                _tempCrop = crop.Checked;
                _tempCropValues = Cropping;

                _tempInputLine = ScriptServer.GetInputLine(
                                                    _tempInputFileName,
                                                    _tempInputIndexFile,
                                                    _tempDeinterlacer,
                                                    _tempInputSourceType,
                                                    _tempColourCorrect,
                                                    _tempMpeg2Deblocking,
                                                    _tempFlipVertical,
                                                    _tempFPS,
                                                    _tempDSS2,
                                                    nvDeInt.Checked ? _tempNvDeintType : NvDeinterlacerType.nvDeInterlacerNone,
                                                    (_tempNvResize && _tempResize) ? (int)_tempNvHorizontalResolution : 0,
                                                    (_tempNvResize && _tempResize) ? (int)_tempNvVerticalResolution: 0,
                                                    (_tempNvResize && _tempCrop) ? _tempCropValues : null);
            }
            
            return _tempInputLine;
        }

        private AviSynthSettings GetProfileSettings()
        {
            return (AviSynthSettings)avsProfile.SelectedProfile.BaseSettings;
        }

		private void ShowScript(bool bForce)
		{
            if (bForce)
                scriptRefresh++;
            if (scriptRefresh < 1)
                return;

            string oldScript = avisynthScript.Text;
            avisynthScript.Text = this.GenerateScript();
            if (!oldScript.Equals(avisynthScript.Text))
                chAutoPreview_CheckedChanged(null, null);
		}
		#endregion

		#region helper methods
        /// <summary>
        /// Opens a video source using the correct method based on the extension of the file name
        /// </summary>
        /// <param name="videoInput"></param>
        private void OpenVideoSource(string videoInput, string indexFileTemp)
        {
            string ext, projectPath, fileNameNoPath;

            indexFile = indexFileTemp;
            if (String.IsNullOrEmpty(indexFile))
            {
                ext = Path.GetExtension(videoInput).ToLowerInvariant();
                projectPath = FileUtil.GetOutputFolder(videoInput);
                fileNameNoPath = Path.GetFileName(videoInput);
            }
            else
            {
                ext = Path.GetExtension(indexFile).ToLowerInvariant();
                projectPath = FileUtil.GetOutputFolder(indexFile);
                fileNameNoPath = Path.GetFileName(indexFile);
            }           
            videoOutput.Filename = Path.Combine(projectPath, Path.ChangeExtension(fileNameNoPath, ".avs"));

            switch (ext)
            {
                case ".avs":
                    sourceType = PossibleSources.avs;
                    videoOutput.Filename = Path.Combine(projectPath, Path.GetFileNameWithoutExtension(fileNameNoPath) + "_new.avs"); // to avoid overwritten
                    OpenAVSScript(videoInput);
                    break;           
                case ".d2v":
                    sourceType = PossibleSources.d2v;
                    OpenVideo(videoInput);
                    break;
                case ".dgi":
                    if (FileIndexerWindow.isDGMFile(videoInput))
                        sourceType = PossibleSources.dgm;
                    else
                        sourceType = PossibleSources.dgi;
                    OpenVideo(videoInput); 
                    break;
                case ".ffindex":
                    sourceType = PossibleSources.ffindex;
                    if (videoInput.ToLowerInvariant().EndsWith(".ffindex"))
                        OpenVideo(videoInput.Substring(0, videoInput.Length - 8));
                    else
                        OpenVideo(videoInput);
                    break;
                case ".lwi":
                    sourceType = PossibleSources.lsmash;
                    OpenVideo(videoInput);
                    break;
                case ".vdr":
                    sourceType = PossibleSources.vdr;
                    OpenVDubFrameServer(videoInput);
                    break;
                default:
                    if (File.Exists(videoInput + ".dgi") || File.Exists(Path.ChangeExtension(videoInput, "dgi")))
                    {
                        if (FileIndexerWindow.isDGMFile(videoInput))
                            sourceType = PossibleSources.dgm;
                        else
                            sourceType = PossibleSources.dgi;
                        if (File.Exists(videoInput + ".dgi"))
                            OpenVideo(videoInput + ".dgi");
                        else
                            OpenVideo(Path.ChangeExtension(videoInput, "dgi"));
                    }
                    else if (File.Exists(videoInput + ".d2v") || File.Exists(Path.ChangeExtension(videoInput, "d2v")))
                    {
                        sourceType = PossibleSources.d2v;
                        if (File.Exists(videoInput + ".d2v"))
                            OpenVideo(videoInput);
                        else
                            OpenVideo(Path.ChangeExtension(videoInput, "d2v"));
                    }
                    else if (File.Exists(videoInput + ".lwi"))
                    {
                        sourceType = PossibleSources.lsmash;
                        OpenVideo(videoInput);
                    }
                    else if (File.Exists(videoInput + ".ffindex"))
                    {
                        sourceType = PossibleSources.ffindex;
                        OpenVideo(videoInput);
                    }
                    else
                    {
                        int iResult = mainForm.DialogManager.AVSCreatorOpen(videoInput);
                        switch (iResult)
                        {
                            case 0:
                                OneClickWindow ocmt = new OneClickWindow();
                                ocmt.Show();
                                ocmt.setInput(videoInput);
                                this.Close();
                                break;
                            case 1:
                                FileIndexerWindow fileIndexer = new FileIndexerWindow(mainForm);
                                fileIndexer.setConfig(videoInput, null, 2, true, true, true, false);
                                fileIndexer.Show();
                                this.Close();
                                break;
                            case 2:
                                sourceType = PossibleSources.avisource;
                                OpenDirectShow(videoInput);
                                break;
                            case 3:
                                sourceType = PossibleSources.directShow;
                                OpenDirectShow(videoInput);
                                break;
                            default:
                                MessageBox.Show("Unable to render the file.\r\nYou probably don't have the correct filters installed.", "DirectShow Error", MessageBoxButtons.OK);
                                return;
                        }
                    }
                    break;
            }
            SetSourceInterface();
        }
		
        /// <summary>
		/// writes the AviSynth script currently shown in the GUI to the given path
		/// </summary>
		/// <param name="path">path and name of the AviSynth script to be written</param>
		private void WriteScript(string path)
		{
			try
			{
				using (StreamWriter sw = new StreamWriter(path, false))
                {
				    sw.Write(avisynthScript.Text);
				    sw.Close();
                }
			}
			catch (Exception i)
			{
				MessageBox.Show("An error occurred when trying to save the AviSynth script:\r\n" + i.Message);
			}
		}
        
        /// <summary>
        /// Set the correct states of the interface elements that are only valid for certain inputs
        /// </summary>
        private void SetSourceInterface()
        {
            switch (this.sourceType)
            {            
                case PossibleSources.d2v:
                    this.mpeg2Deblocking.Enabled = true;
                    this.colourCorrect.Enabled = true;
                    this.fpsBox.Enabled = false;
                    this.flipVertical.Enabled = false;
                    this.flipVertical.Checked = false;
                    this.dgOptions.Enabled = false;
                    this.dss2.Enabled = false;
                    this.tabSources.SelectedTab = tabPage1;
                    break;
                case PossibleSources.vdr:
                case PossibleSources.avs:
                case PossibleSources.avisource:
                    this.mpeg2Deblocking.Checked = false;
                    this.mpeg2Deblocking.Enabled = false;
                    this.colourCorrect.Enabled = false;
                    this.colourCorrect.Checked = false;
                    this.flipVertical.Enabled = false;
                    this.flipVertical.Checked = false;
                    this.dss2.Enabled = false;
                    this.fpsBox.Enabled = false;
                    this.dgOptions.Enabled = false;
                    this.tabSources.SelectedTab = tabPage1;
                    break;
                case PossibleSources.ffindex:
                case PossibleSources.lsmash:
                    this.mpeg2Deblocking.Checked = false;
                    this.mpeg2Deblocking.Enabled = false;
                    this.colourCorrect.Enabled = false;
                    this.colourCorrect.Checked = false;
                    this.dss2.Enabled = false;
                    this.fpsBox.Enabled = false;
                    this.flipVertical.Enabled = true;
                    this.dgOptions.Enabled = false;
                    this.tabSources.SelectedTab = tabPage1;
                    break;
                case PossibleSources.directShow:
                    this.mpeg2Deblocking.Checked = false;
                    this.mpeg2Deblocking.Enabled = false;
                    this.colourCorrect.Enabled = false;
                    this.colourCorrect.Checked = false;
                    this.dss2.Enabled = true;
                    this.fpsBox.Enabled = true;
                    this.flipVertical.Enabled = true;
                    this.dgOptions.Enabled = false;
                    this.tabSources.SelectedTab = tabPage2;
                    break;
                case PossibleSources.dgi:
                    this.mpeg2Deblocking.Checked = false;
                    this.mpeg2Deblocking.Enabled = false;
                    this.colourCorrect.Enabled = false;
                    this.colourCorrect.Checked = false;
                    this.flipVertical.Enabled = false;
                    this.flipVertical.Checked = false;
                    this.dss2.Enabled = false;
                    this.fpsBox.Enabled = false;
                    this.dgOptions.Enabled = true;
                    this.tabSources.SelectedTab = tabPage3;
                    break;
                case PossibleSources.dgm:
                    this.mpeg2Deblocking.Checked = false;
                    this.mpeg2Deblocking.Enabled = false;
                    this.colourCorrect.Enabled = false;
                    this.colourCorrect.Checked = false;
                    this.flipVertical.Enabled = false;
                    this.flipVertical.Checked = false;
                    this.dss2.Enabled = false;
                    this.fpsBox.Enabled = false;
                    this.dgOptions.Enabled = false;
                    this.tabSources.SelectedTab = tabPage3;
                    break;
            }
        }

        /// <summary>
        /// check whether or not it's an NV file compatible (for DGxNV tools)
        /// </summary>
        private void CheckNVCompatibleFile(string input)
        {
            bool flag = false;
            using (StreamReader sr = new StreamReader(input))
            {
                string line = sr.ReadLine();
                switch (this.sourceType)
                {
                    case PossibleSources.dgi:
                        if (line.Contains("DGMPGIndexFileNV")) flag = true;
                        if (line.Contains("DGAVCIndexFileNV")) flag = true;
                        if (line.Contains("DGVC1IndexFileNV")) flag = true;
                        break; 
                }
            }
            if (!flag)
            {
                if (MessageBox.Show("You cannot use this option with the " + Path.GetFileName(input) + " file. It's not compatible...",
                    "Information", MessageBoxButtons.OK, MessageBoxIcon.Information) == DialogResult.OK)
                {
                    this.nvDeInt.Checked = false;
                    this.nvResize.Checked = false;
                }
            }
        }

        private SourceInfo DeintInfo
        {
            get
            {
                SourceInfo info = new SourceInfo();
                try 
                {
                    if (deintSourceType.SelectedItem != null)
                        info.sourceType = (SourceType)((EnumProxy)deintSourceType.SelectedItem).Tag; 
                }
                catch (NullReferenceException) { info.sourceType = SourceType.UNKNOWN; }
                try 
                {
                    if (deintFieldOrder.SelectedItem != null)
                        info.fieldOrder = (FieldOrder)((EnumProxy)deintFieldOrder.SelectedItem).Tag; 
                }
                catch (NullReferenceException) { info.fieldOrder = FieldOrder.UNKNOWN; }
                info.decimateM = (int)deintM.Value;
                try
                {
                    if (deintSourceType.SelectedItem != null)
                        info.majorityFilm =
                            ((UserSourceType)((EnumProxy)deintSourceType.SelectedItem).RealValue) == UserSourceType.HybridFilmInterlaced ||
                            ((UserSourceType)((EnumProxy)deintSourceType.SelectedItem).RealValue) == UserSourceType.HybridProgressiveFilm;
                }
                catch (NullReferenceException) { }
                info.isAnime = deintIsAnime.Checked;
                return info;
            }
            set
            {
                if (value.sourceType == SourceType.UNKNOWN || value.sourceType == SourceType.NOT_ENOUGH_SECTIONS)
                {
                    MessageBox.Show("Source detection couldn't determine the source type!", "Source detection failed", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                foreach (EnumProxy o in deintSourceType.Items)
                {
                    if ((SourceType)o.Tag == value.sourceType)
                        deintSourceType.SelectedItem = o;
                }
                foreach (EnumProxy o in deintFieldOrder.Items)
                {
                    if ((FieldOrder)o.Tag == value.fieldOrder)
                        deintFieldOrder.SelectedItem = o;
                }
                if (value.fieldOrder == FieldOrder.UNKNOWN)
                    deintFieldOrder.SelectedIndex = -1;
                deintM.Value = value.decimateM;
                if (value.sourceType == SourceType.HYBRID_FILM_INTERLACED)
                {
                    if (value.majorityFilm)
                        deintSourceType.SelectedItem = ScriptServer.ListOfSourceTypes[(int)UserSourceType.HybridFilmInterlaced];
                    else
                        deintSourceType.SelectedItem = ScriptServer.ListOfSourceTypes[(int)UserSourceType.HybridInterlacedFilm];
                }
                this.deinterlaceType.DataSource = ScriptServer.GetDeinterlacers(value);
                this.deinterlaceType.BindingContext = new BindingContext();
            }
        }
        
        /// <summary>
        /// Check whether direct show can render the avi and then open it through an avisynth script.
        /// The avs is being used in order to allow more preview flexibility later.
        /// </summary>
        /// <param name="fileName">Input video file</param>     
        private void OpenDirectShow(string fileName)
        {
            if (!File.Exists(fileName))
            {
                MessageBox.Show(fileName + " could not be found", "File Not Found", MessageBoxButtons.OK);
                return;
            }

            string tempAvs;
            if (sourceType == PossibleSources.avisource)
            {
                tempAvs = "AVISource(\"" + fileName + "\", audio=false)" + VideoUtil.getAssumeFPS(0, fileName);
            }
            else
            {
                string frameRateString = null;
                try
                {
                    using (MediaInfoFile info = new MediaInfoFile(fileName))
                    {
                        if (info.VideoInfo.HasVideo && info.VideoInfo.FPS > 0)
                            frameRateString = info.VideoInfo.FPS.ToString(System.Globalization.CultureInfo.InvariantCulture);
                    }
                }
                catch (Exception)
                { }

                tempAvs = string.Format(
                    "DirectShowSource(\"{0}\", audio=false{1}, convertfps=true){2}{3}",
                    fileName,
                    frameRateString == null ? string.Empty : (", fps=" + frameRateString),
                    VideoUtil.getAssumeFPS(0, fileName),
                    this.flipVertical.Checked ? ".FlipVertical()" : string.Empty
                    );
                if (MainForm.Instance.Settings.PortableAviSynth)
                    tempAvs = "LoadPlugin(\"" + Path.Combine(Path.GetDirectoryName(MainForm.Instance.Settings.AviSynth.Path), @"plugins\directshowsource.dll") + "\")\r\n" + tempAvs;
                if (MainForm.Instance.Settings.AviSynthPlus && MainForm.Instance.Settings.Input8Bit)
                    tempAvs += "\r\nConvertBits(8)";
            }
            file?.Dispose();
            OpenVideo(tempAvs, fileName, true);

        }
        
        /// <summary>
        /// Create a temporary avs to wrap the frameserver file then open it as for any other avs
        /// </summary>
        /// <param name="fileName">Name of the .vdr file</param>
        private void OpenVDubFrameServer(string fileName)
        {
            if (!File.Exists(fileName))
            {
                MessageBox.Show(fileName + " could not be found","File Not Found",MessageBoxButtons.OK);
                return;
            }
            OpenVideo("AviSource(\"" + fileName + ", audio=false\")\r\n", fileName, true);
        }

        /// <summary>
        /// Create a temporary avs to wrap the frameserver file then open it as for any other avs
        /// </summary>
        /// <param name="fileName">Name of the avs script</param>
        private void OpenAVSScript(string fileName)
        {
            if (!File.Exists(fileName))
            {
                MessageBox.Show(fileName + " could not be found", "File Not Found", MessageBoxButtons.OK);
                return;
            }
            OpenVideo("Import(\"" + fileName + "\")\r\n", fileName, true);
        }

        private void EnableControls(bool enable)
        {
            foreach (Control ctrl in this.controlsToDisable)
                ctrl.Enabled = enable;

            if (deintSourceType.SelectedIndex < 1)
            {
                deinterlace.Enabled = false;
                deinterlace.Checked = false;
            }
            else
                deinterlace.Enabled = true;
        }

        private void OpenVideo(string videoInput)
        {
            if (String.IsNullOrEmpty(indexFile))
                OpenVideo(videoInput, videoInput, false);
            else
                OpenVideo(videoInput + "|" + indexFile, videoInput, false);
        }

        private bool ShowOriginal()
        {
            int iCurrentFrame = -1;
            if (player == null || player.IsDisposed)
                player = new VideoPlayer();
            else
                iCurrentFrame = player.CurrentFrame;
            this.isPreviewMode = false;
            if (player.LoadVideo(originalScript, PREVIEWTYPE.REGULAR, false, originalInlineAvs, iCurrentFrame, true))
            {
                reader = player.Reader;
                SendCropValues();
                if (this.Visible)
                    player.Show();
                player.SetScreenSize();
                this.TopMost = player.TopMost = true;
                if (!mainForm.Settings.AlwaysOnTop)
                    this.TopMost = player.TopMost = false;
                return true;
            }
            else
            {
                player.Close();
                player = null;
                return false;
            }
        }

		/// <summary>
		/// opens a given script
		/// </summary>
		/// <param name="videoInput">the script to be opened</param>
		private void OpenVideo(string videoInputScript, string strSourceFileName, bool inlineAvs)
		{
			this.crop.Checked = false;
            this.input.Filename = "";
            this.originalScript = videoInputScript;
            this.originalInlineAvs = inlineAvs;
            player?.Dispose();
            
            bool videoLoaded = false;
            if (MainForm.Instance.Settings.AutoOpenScript)
            {
                videoLoaded = ShowOriginal();
                if (videoLoaded)
                {
                    file = player.File;
                    reader = player.Reader;
                }
            }
            else
                {
                    using (MediaInfoFile x = new MediaInfoFile(strSourceFileName))
                    {
                        videoLoaded = x.MediaInfoOK;
                        if (x.MediaInfoOK)
                        {
                            file = x;
                            reader = null;
                        }
                    }
                }
            
            EnableControls(videoLoaded);
            if (videoLoaded)
            {
                eventsOn = false;
                this.input.Filename = strSourceFileName;
                this.fpsBox.Value = (decimal)file.VideoInfo.FPS;
                if (file.VideoInfo.FPS.Equals(25.0)) // disable ivtc for pal sources
                    this.tvTypeLabel.Text = "PAL";
                else
                    this.tvTypeLabel.Text = "NTSC";

                if (horizontalResolution.Maximum < file.VideoInfo.Width)
                    horizontalResolution.Maximum = file.VideoInfo.Width;
                horizontalResolution.Value = file.VideoInfo.Width;
                if (verticalResolution.Maximum < file.VideoInfo.Height)
                    verticalResolution.Maximum = file.VideoInfo.Height;
                verticalResolution.Value = file.VideoInfo.Height;

                if (File.Exists(strSourceFileName) && MainForm.Instance.Settings.AutoOpenScript)
                {
                    using (MediaInfoFile oInfo = new MediaInfoFile(strSourceFileName))
                        arChooser.Value = oInfo.VideoInfo.DAR;
                }
                else
                    arChooser.Value = file.VideoInfo.DAR;

                cropLeft.Maximum = cropRight.Maximum = file.VideoInfo.Width;
                cropTop.Maximum = cropBottom.Maximum = file.VideoInfo.Height;
                eventsOn = true;
            }
		}

        private void CalcAspectError()
        {
            if (file == null)
            {
                lblAspectError.BackColor = System.Drawing.SystemColors.Window;
                lblAspectError.Text = "0.00000%";
                return;
            }

            // get output dimension
            int outputHeight = (int)verticalResolution.Value;
            int outputWidth = (int)horizontalResolution.Value;
            if (!resize.Checked)
            {
                outputHeight = (int)file.VideoInfo.Height - Cropping.top - Cropping.bottom;
                outputWidth = (int)file.VideoInfo.Width - Cropping.left - Cropping.right;
            }

            decimal aspectError = Resolution.GetAspectRatioError((int)file.VideoInfo.Width, (int)file.VideoInfo.Height, outputWidth, outputHeight, Cropping, arChooser.Value, signalAR.Checked, suggestedDar);
            lblAspectError.Text = String.Format("{0:0.00000%}", aspectError);
            if (!signalAR.Checked || Math.Floor(Math.Abs(aspectError) * 100000000) <= this.GetProfileSettings().AcceptableAspectError * 1000000)
                lblAspectError.ForeColor = System.Drawing.SystemColors.WindowText;
            else
                lblAspectError.ForeColor = System.Drawing.Color.Red;
        }

		#endregion

		#region updown
		private static void ChangeNumericUpDownColor(NumericUpDown oControl, bool bMarkRed)
        {
            if (oControl.Enabled)
            {
                if (bMarkRed)
                    oControl.ForeColor = System.Drawing.Color.Red;
                else
                    oControl.ForeColor = System.Drawing.SystemColors.WindowText;
                oControl.BackColor = System.Drawing.SystemColors.Window;
            }
            else
            {
                if (bMarkRed)
                    oControl.BackColor = System.Drawing.Color.FromArgb(255, 255, 180, 180);
                else
                    oControl.BackColor = System.Drawing.SystemColors.Window;
                oControl.ForeColor = System.Drawing.SystemColors.WindowText;
            }
        }

		private void SendCropValues()
		{
            if (player == null || !player.Visible)
                return;

            if (isPreviewMode)
                player.crop(0, 0, 0, 0);
            else
                player.crop(Cropping);
		}
		#endregion

		#region checkboxes
		private void deinterlace_CheckedChanged(object sender, EventArgs e)
		{
			if (deinterlace.Checked)
			{
				deinterlaceType.Enabled = true;
				if (deinterlaceType.SelectedIndex == -1)
					deinterlaceType.SelectedIndex = 0; // make sure something is selected
			}
			else
				deinterlaceType.Enabled = false;

            if (sender != null && e != null)
			    ShowScript(false);
		}

		private void noiseFilter_CheckedChanged(object sender, EventArgs e)
		{
			if (noiseFilter.Checked)
			{
				this.noiseFilterType.Enabled = true;
			}
			else
				this.noiseFilterType.Enabled = false;

            if (sender != null && e != null)
			    ShowScript(false);
		}

		#endregion

		#region autocrop
		/// <summary>
		/// gets the autocrop values
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void autoCropButton_Click(object sender, EventArgs e)
		{
            if (isPreviewMode || player == null || !player.Visible)
            {
                MessageBox.Show(this, "No AutoCropping without the original video window open",
                    "AutoCropping not possible",MessageBoxButtons.OK,MessageBoxIcon.Error);
                return;
            }

            // don't lock up the GUI, start a new thread
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            System.Threading.Thread t = new System.Threading.Thread(new System.Threading.ThreadStart(delegate
            {
                CropValues final = Autocrop.autocrop(reader);
                Invoke(new MethodInvoker(delegate
                {
                    SetCropValues(final);
                }));
            }));
            t.IsBackground = true;
            t.Start();
		}

        private void SetCropValues(CropValues cropValues)
        {
            this.Cursor = System.Windows.Forms.Cursors.Default;
            bool error = (cropValues.left < 0);
            if (!error)
            {
                eventsOn = false;
                cropLeft.Value = cropValues.left;
                cropTop.Value = cropValues.top;
                cropRight.Value = cropValues.right;
                cropBottom.Value = cropValues.bottom;
                if (!crop.Checked)
                    crop.Checked = true;
                eventsOn = true;
                UpdateEverything(true, false, false);
            }
            else
                MessageBox.Show("I'm afraid I was unable to find more than 5 frames that have matching crop values");
        }

		#endregion

        #region properties

        private AviSynthSettings Settings
		{
			set
			{
                eventsOn = false;
				this.resizeFilterType.SelectedItem =  EnumProxy.Create(value.ResizeMethod);
                this.noiseFilterType.SelectedItem = EnumProxy.Create(value.DenoiseMethod);
				this.mpeg2Deblocking.Checked = value.MPEG2Deblock;
				this.colourCorrect.Checked = value.ColourCorrect;
				this.deintIsAnime.Checked = value.PreferAnimeDeinterlace;
				this.noiseFilter.Checked = value.Denoise;
                this.resize.Checked = value.Resize;
                this.nvResize.Checked = value.NvResize;
                this.mod16Box.SelectedIndex = (int)value.Mod16Method;
                this.signalAR.Checked = (value.Mod16Method != mod16Method.none);
                this.dss2.Checked = value.DSS2;
                this.bAllowUpsizing = value.Upsize;
                if (!bAllowUpsizing && file != null)
                {
                    horizontalResolution.Maximum = file.VideoInfo.Width;
                    verticalResolution.Maximum = file.VideoInfo.Height;
                }
                else
                    horizontalResolution.Maximum = verticalResolution.Maximum = 9999;
                this.modValueBox.SelectedIndex = (int)value.ModValue;
                eventsOn = true;
                UpdateEverything(true, false, value.Resize);
			}
        }
        
        private CropValues Cropping
        {
            get
            {
                CropValues returnValue = new CropValues();
                if (crop.Checked)
                {
                    returnValue.bottom = (int)cropBottom.Value;
                    returnValue.top = (int)cropTop.Value;
                    returnValue.left = (int)cropLeft.Value;
                    returnValue.right = (int)cropRight.Value;
                    if (Mod16Method == mod16Method.overcrop)
                        ScriptServer.overcrop(ref returnValue, (modValue)modValueBox.SelectedIndex);
                    else if (Mod16Method == mod16Method.mod4Horizontal)
                        ScriptServer.cropMod4Horizontal(ref returnValue);
                    else if (Mod16Method == mod16Method.undercrop)
                        ScriptServer.undercrop(ref returnValue, (modValue)modValueBox.SelectedIndex);
                }
                return returnValue;
            }
        }

        private mod16Method Mod16Method
        {
            get
            {
                mod16Method m = (mod16Method)mod16Box.SelectedIndex;
                if (!mod16Box.Enabled)
                    m = mod16Method.none;
                return m;
            }
        }

        #endregion

        #region autodeint
        private void analyseButton_Click(object sender, EventArgs e)
        {
            if (input.Filename.Length == 0)
            {
                MessageBox.Show("Can't run any analysis as there is no selected video to analyse.",
                    "Please select a video input file", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (detector == null && analyseButton.Text.Equals("Analyse")) // We want to start the analysis
            {
                analyseButton.Text = "Abort";

                string source = ScriptServer.GetInputLine(
                    input.Filename, indexFile, false, sourceType, false, false, false,
                    0, false, NvDeinterlacerType.nvDeInterlacerNone, 0, 0, null);

                // get number of frames
                int numFrames = 0;
                try
                {
                    using (AvsFile af = AvsFile.ParseScript(source))
                    {
                        numFrames = (int)af.VideoInfo.FrameCount;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("The input clip for source detection could not be opened.\r\n" + ex.Message, "Analysis Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                detector = new SourceDetector(source,
                    input.Filename, deintIsAnime.Checked, numFrames,
                    mainForm.Settings.SourceDetectorSettings.Priority,
                    mainForm.Settings.SourceDetectorSettings,
                    new UpdateSourceDetectionStatus(AnalyseUpdate),
                    new FinishedAnalysis(FinishedAnalysis));
                    detector.Analyse();
                deintStatusLabel.Text = "Analysing...";
            }
            else // We want to cancel the analysis
            {
                if (detector != null)
                {
                    detector.Stop();
                    detector = null;
                }
                analyseButton.Text = "Analyse";
                this.deintProgressBar.Value = 0;
                deintStatusLabel.Text = "";
            }
        }
                

        public void FinishedAnalysis(SourceInfo info, ExitType exit, string errorMessage)
        {
            if (exit == ExitType.ERROR)
            {
                Invoke(new MethodInvoker(delegate
                {
                    MessageBox.Show(this, errorMessage, "Error in analysis", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    deintStatusLabel.Text = "Analysis failed!";
                    analyseButton.Text = "Analyse";
                    this.deintProgressBar.Value = 0;
                }));
            }
            else if (exit == ExitType.OK)
            {
                try
                {
                    info.isAnime = deintIsAnime.Checked;
                    Invoke(new MethodInvoker(delegate
                    {
                        deintProgressBar.Enabled = false;
                        this.deintProgressBar.Value = this.deintProgressBar.Maximum;
                        this.DeintInfo = info;
                        if (deintSourceType.SelectedIndex < 1)
                        {
                            deinterlace.Enabled = false;
                            deinterlace.Checked = false;
                        }
                        else
                            deinterlace.Enabled = true;
                        if (deinterlaceType.Text == "Do nothing")
                            deinterlace.Checked = false;
                        else
                            deinterlace.Checked = true;
                        deintStatusLabel.Text = "Analysis finished!";
                        analyseButton.Text = "Analyse";
                    }));
                }
                catch (Exception) { } // If we get any errors, it's most likely because the window was closed, so just ignore
            }
            detector = null;

            if (info == null)
                return;

            this._oLog = mainForm.AVSScriptCreatorLog;
            if (_oLog == null)
            {
                _oLog = mainForm.Log.Info("AVS Script Creator");
                mainForm.AVSScriptCreatorLog = _oLog;
            }
            _oLog.LogValue("Source detection: " + Path.GetFileName(input.Filename), info.analysisResult, exit == ExitType.ERROR ? ImageType.Warning : ImageType.Information);
        }

        public void AnalyseUpdate(int amountDone, int total)
        {
            try
            {
                Invoke(new MethodInvoker(delegate
                    {
                        if (amountDone > this.deintProgressBar.Maximum)
                            this.deintProgressBar.Maximum = total;
                        this.deintProgressBar.Value = amountDone;
                        this.deintProgressBar.Maximum = total;
                    }));
            }
            catch (Exception) { } // If we get any errors, just ignore -- it's only a cosmetic thing.
        }
        #endregion

        private void deintSourceType_SelectedIndexChanged(object sender, EventArgs e)
        {
            deintM.Enabled = (deintSourceType.SelectedItem == ScriptServer.ListOfSourceTypes[(int)UserSourceType.Decimating]);
            deintFieldOrder.Enabled = !(deintSourceType.SelectedItem == ScriptServer.ListOfSourceTypes[(int)UserSourceType.Progressive]);
            deinterlaceType.DataSource = ScriptServer.GetDeinterlacers(DeintInfo);
            deinterlaceType.BindingContext = new BindingContext();
            if (deintSourceType.SelectedIndex < 1)
            {
                deinterlace.Enabled = false;
                deinterlace.Checked = false;
            }
            else
                deinterlace.Enabled = true;

            if (sender != null && e != null)
                ShowScript(false);
        }

        private void reopenOriginal_Click(object sender, EventArgs e)
        {
            reopenOriginal.Enabled = false;
            reopenOriginal.Text = "Please wait...";
            if (chAutoPreview.Checked)
                chAutoPreview.Checked = false;
            else
                ShowOriginal();
            reopenOriginal.Enabled = true;
            reopenOriginal.Text = "Re-open original video player";
        }

        private void chAutoPreview_CheckedChanged(object sender, EventArgs e)
        {
            if (chAutoPreview.Checked)
                previewButton_Click(null, null);
            else if (this.isPreviewMode == true)
                ShowOriginal();
        }

        private void nvDeInt_CheckedChanged(object sender, EventArgs e)
        {
            if (nvDeInt.Checked)
                cbNvDeInt.Enabled = true;
            else 
                cbNvDeInt.Enabled = false;
            if (sender != null && e != null)
                ShowScript(false);
        }

        private void nvDeInt_Click(object sender, EventArgs e)
        {
            // just to be sure
            CheckNVCompatibleFile(input.Filename);
        }

        private void openSubtitlesButton_Click(object sender, EventArgs e)
        {
            if (this.openSubsDialog.ShowDialog() != DialogResult.OK)
                return;

            if (this.SubtitlesPath.Text != openSubsDialog.FileName)
            {
                string ext = Path.GetExtension(openSubsDialog.FileName).ToString().ToLowerInvariant();
                this.SubtitlesPath.Text = openSubsDialog.FileName;
                cbCharset.Enabled = (ext != ".idx");
            }
            else 
                MessageBox.Show("The subtitles you have chosen were already added...", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            
            if (sender != null && e != null)
                ShowScript(false);
        }

        private string CharsetValue()
        {
            string c = string.Empty;

            if (!string.IsNullOrEmpty(SubtitlesPath.Text))
            {
                switch (cbCharset.SelectedIndex)
                {
                    case 1: c = "0"; break;
                    case 2: c = "2"; break;
                    case 3: c = "128"; break;
                    case 4:
                    case 5: c = "129"; break;
                    case 6: c = "134"; break;
                    case 7: c = "136"; break;
                    case 8: c = "255"; break;
                    case 9: c = "130"; break;
                    case 10: c = "177"; break;
                    case 11: c = "178"; break;
                    case 12: c = "161"; break;
                    case 13: c = "162"; break;
                    case 14: c = "163"; break;
                    case 15: c = "222"; break;
                    case 16: c = "238"; break;
                    case 17: c = "204"; break;
                    case 18: c = "77"; break;
                    case 19: c = "186"; break;
                    default: c = "1"; break;
                }
            }
            return c;
        }

        private void InputDARChanged(object sender, string val)
        {
            UpdateEverything(sender != null, false, false);
        }

        private void UpdateEverything(object sender, EventArgs e)
        {
            UpdateEverything(sender != null, false, false);
        }

        private void UpdateEverything(bool bShowScript, bool bForceScript, bool bResizeEnabled)
{
            if (!eventsOn)
                return;
            eventsOn = false;

            // update events may be triggered
            SetModType();
            SetCrop();
            SetOutputResolution(bResizeEnabled);

            // no update events triggered
            CalcAspectError();
            CheckControls();
            
            if (bShowScript)
                ShowScript(bForceScript);
            eventsOn = true;
        }

        private void SetModType()
        {
            if (!bAllowUpsizing && file != null)
            {
                horizontalResolution.Maximum = file.VideoInfo.Width;
                verticalResolution.Maximum = file.VideoInfo.Height;
            }
            else
                horizontalResolution.Maximum = verticalResolution.Maximum = 9999;

            if (signalAR.Checked)
            {
                mod16Box.Enabled = true;
                if (mod16Box.SelectedIndex == -1)
                    mod16Box.SelectedIndex = 0;
            }
            else
                mod16Box.Enabled = false;

            if (Mod16Method == mod16Method.overcrop)
                crop.Text = "Crop (will be rounded up to selected mod)";
            else if (Mod16Method == mod16Method.undercrop)
                crop.Text = "Crop (will be rounded down to sel. mod)";
            else
                crop.Text = "Crop";

            if (Mod16Method == mod16Method.resize)
            {
                resize.Enabled = false;
                resize.Checked = true;
                suggestResolution.Enabled = false;
                suggestResolution.Checked = true;
            }
            else if (Mod16Method == mod16Method.none)
            {
                resize.Enabled = true;
                suggestResolution.Enabled = resize.Checked;
                if (!suggestResolution.Enabled)
                    suggestResolution.Checked = true;
            }
            else
            {
                resize.Checked = false;
                resize.Enabled = false;
                suggestResolution.Enabled = false;
                suggestResolution.Checked = true;
            }

            if (resize.Checked || (signalAR.Checked && (Mod16Method == mod16Method.resize || Mod16Method == mod16Method.overcrop || Mod16Method == mod16Method.undercrop)))
                modValueBox.Enabled = true;
            else
                modValueBox.Enabled = false;
        }

        private void SetCrop()
        {
            if (crop.Checked)
            {
                this.cropLeft.Enabled = true;
                this.cropTop.Enabled = true;
                this.cropRight.Enabled = true;
                this.cropBottom.Enabled = true;
                SendCropValues();
            }
            else
            {
                this.cropLeft.Enabled = false;
                this.cropTop.Enabled = false;
                this.cropRight.Enabled = false;
                this.cropBottom.Enabled = false;
                if (player != null && player.Visible)
                    player.crop(0, 0, 0, 0);
            }

            if (file != null)
            {
                int inputHeight = (int)file.VideoInfo.Height - Cropping.top - Cropping.bottom;
                int inputWidth = (int)file.VideoInfo.Width - Cropping.left - Cropping.right;
                if (!resize.Checked)
                {
                    if (verticalResolution.Maximum < inputHeight)
                        verticalResolution.Maximum = inputHeight;
                    verticalResolution.Value = inputHeight;

                    if (horizontalResolution.Maximum < inputWidth)
                        horizontalResolution.Maximum = inputWidth;
                    horizontalResolution.Value = inputWidth;
                }
                if (!bAllowUpsizing)
                {
                    verticalResolution.Maximum = inputHeight;
                    horizontalResolution.Maximum = inputWidth;
                }
                if (crop.Checked)
                {
                    int mod = Resolution.GetModValue((modValue)modValueBox.SelectedIndex, (mod16Method)mod16Box.SelectedIndex, signalAR.Checked);
                    cropLeft.Maximum = (int)file.VideoInfo.Width - Cropping.right - mod;
                    cropRight.Maximum = (int)file.VideoInfo.Width - Cropping.left - mod;
                    cropTop.Maximum = (int)file.VideoInfo.Height - Cropping.bottom - mod;
                    cropBottom.Maximum = (int)file.VideoInfo.Height - Cropping.top - mod;
                }
            }
        }

        private void deleteSubtitle_Click(object sender, EventArgs e)
        {
            SubtitlesPath.Text = null;
            ShowScript(false);
        }

        private void SetOutputResolution(bool bResizeEnabled)
        {
            if (resize.Checked)
            {
                this.horizontalResolution.Enabled = true;
                this.verticalResolution.Enabled = !suggestResolution.Checked;
            }
            else
                this.horizontalResolution.Enabled = this.verticalResolution.Enabled = false;

            suggestedDar = null;
            bool signalAR = this.signalAR.Checked;
            if (file == null || (!resize.Checked && !signalAR))
                return;

            int mod = Resolution.GetModValue((modValue)modValueBox.SelectedIndex, (mod16Method)mod16Box.SelectedIndex, signalAR);
            horizontalResolution.Increment = verticalResolution.Increment = mod;

            int outputWidth = (int)horizontalResolution.Value;
            int outputHeight = (int)verticalResolution.Value;

            // remove upsizing or undersizing if value cannot be changed
            if (!resize.Checked && !((!bAllowUpsizing || bResizeEnabled) && (int)file.VideoInfo.Width - Cropping.left - Cropping.right < outputWidth))
                outputWidth = (int)file.VideoInfo.Width - Cropping.left - Cropping.right;

            CropValues cropValues = Cropping.Clone();

            bool resizeEnabled = resize.Checked;
            Resolution.GetResolution((int)file.VideoInfo.Width, (int)file.VideoInfo.Height, arChooser.RealValue,
                ref cropValues, crop.Checked, mod, ref resizeEnabled, bAllowUpsizing, signalAR, suggestResolution.Checked, 
                this.GetProfileSettings().AcceptableAspectError, null, 0, ref outputWidth, ref outputHeight, out _, out suggestedDar, null);
            
            if (!resize.Checked && !suggestResolution.Checked) // just to make sure
                outputHeight = (int)file.VideoInfo.Height - Cropping.top - Cropping.bottom;

            if (outputWidth != (int)horizontalResolution.Value)
            {
                if (horizontalResolution.Maximum < outputWidth)
                    horizontalResolution.Maximum = outputWidth;
                horizontalResolution.Value = outputWidth;
            }
            if (outputHeight != (int)verticalResolution.Value)
            {
                if (verticalResolution.Maximum < outputHeight)
                    verticalResolution.Maximum = outputHeight;
                verticalResolution.Value = outputHeight;
            }
            
            if (suggestResolution.Checked)
                verticalResolution.Enabled = false;
            else
                verticalResolution.Enabled = resize.Checked;
        }

        private void CheckControls()
        {
            if (resize.Checked && file != null && (int)file.VideoInfo.Height - Cropping.top - Cropping.bottom < verticalResolution.Value)
                ChangeNumericUpDownColor(verticalResolution, true);
            else
                ChangeNumericUpDownColor(verticalResolution, false);

            if (resize.Checked && file != null && (int)file.VideoInfo.Width - Cropping.left - Cropping.right < horizontalResolution.Value)
                ChangeNumericUpDownColor(horizontalResolution, true);
            else
                ChangeNumericUpDownColor(horizontalResolution, false);
        }

        private void RefreshScript(object sender, EventArgs e)
        {
            if (sender != null && e != null)
                ShowScript(false);
        }

        private void AviSynthWindow_Shown(object sender, EventArgs e)
        {
            if (player != null && !player.Visible)
                player.Show();
        }

        private void resize_CheckedChanged(object sender, EventArgs e)
        {
            if (sender != null && e != null && resize.Checked)
                UpdateEverything(sender != null, false, true);
            else
                UpdateEverything(sender != null, false, false);
        }
    }
    public delegate void OpenScriptCallback(string avisynthScript, MediaInfoFile oInfo);
    public enum PossibleSources { d2v, dgm, dgi, vdr, directShow, avs, ffindex, lsmash, avisource };
    public enum mod16Method : int { none = -1, resize = 0, overcrop, nonMod16, mod4Horizontal, undercrop };
    public enum modValue : int { mod16 = 0, mod8, mod4, mod2 };

    public class AviSynthWindowTool : MeGUI.core.plugins.interfaces.ITool
    {

        #region ITool Members

        public string Name
        {
            get { return "AVS Script Creator"; }
        }

        public void Run(MainForm info)
        {
            MainForm.Instance.ClosePlayer();
            AviSynthWindow asw = new AviSynthWindow(MainForm.Instance);
            asw.OpenScript += new OpenScriptCallback(MainForm.Instance.Video.openVideoFile);
            asw.Show();
        }

        public Shortcut[] Shortcuts
        {
            get { return new Shortcut[] { Shortcut.CtrlR }; }
        }

        #endregion

        #region IIDable Members

        public string ID
        {
            get { return "AvsCreator"; }
        }

        #endregion
    }
}