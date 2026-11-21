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
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

using MeGUI.BdSub;
using MeGUI.core.plugins.interfaces;

namespace MeGUI.packages.tools.pgsextractor
{
    /// <summary>
    /// Dedicated tool which extracts the PGS subtitles (.sup) of a Blu-ray (folder, playlist or M2TS files).
    /// It does not use eac3to: the playlist and the transport streams are read by the managed demuxer.
    /// </summary>
    public class PgsExtractorWindow : Form
    {
        /// <summary>An entry of the playlist combo box</summary>
        private sealed class PlaylistEntry
        {
            public string Path;
            public TimeSpan Duration;
            public int Items;
            public bool IsM2tsList;
            public string Text;

            public override string ToString()
            {
                return Text;
            }
        }

        private TextBox txtInput;
        private TextBox txtOutput;
        private ComboBox cmbPlaylist;
        private ListView lvTracks;
        private CheckBox chkClose;
        private Button btnFolder;
        private Button btnFiles;
        private Button btnOutput;
        private Button btnQueue;
        private CheckBox chkColors;
        private Button btnTextColor;
        private Button btnOutlineColor;
        private Button btnClose;
        private PictureBox picBefore;
        private PictureBox picAfter;
        private CheckBox chkLightBackground;
        private Label lblPreviewStatus;

        // first subtitle image of the selected track (palette indexes), rendered with the original / the new colors
        private PgsPreviewImage previewImage;
        private int previewRequest;
        private System.Threading.CancellationTokenSource previewCts;

        private List<string> m2tsFiles = new List<string>();
        private bool updating;

        public PgsExtractorWindow()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            this.AutoScaleDimensions = new SizeF(96F, 96F);
            this.AutoScaleMode = AutoScaleMode.Dpi;
            this.ClientSize = new Size(640, 632);
            this.MinimumSize = new Size(560, 590);
            this.FormClosed += new FormClosedEventHandler(PgsExtractorWindow_FormClosed);
            this.AllowDrop = true;
            this.DragEnter += new DragEventHandler(OnDragEnter);
            this.DragDrop += new DragEventHandler(OnDragDrop);
            this.Text = "PGS Subtitle Extractor (Blu-ray)";
            this.StartPosition = FormStartPosition.CenterParent;

            Label lblInput = new Label();
            lblInput.Text = "Blu-ray folder, playlist (.mpls) or M2TS file(s) (drag && drop is supported):";
            lblInput.AutoSize = true;
            lblInput.Location = new Point(12, 12);

            txtInput = new TextBox();
            txtInput.ReadOnly = true;
            txtInput.Location = new Point(12, 32);
            txtInput.Size = new Size(392, 23);
            txtInput.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtInput.AllowDrop = true;
            txtInput.DragEnter += new DragEventHandler(OnDragEnter);
            txtInput.DragDrop += new DragEventHandler(OnDragDrop);

            btnFolder = new Button();
            btnFolder.Text = "Folder...";
            btnFolder.Location = new Point(410, 31);
            btnFolder.Size = new Size(100, 25);
            btnFolder.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnFolder.Click += new EventHandler(btnFolder_Click);

            btnFiles = new Button();
            btnFiles.Text = "File(s)...";
            btnFiles.Location = new Point(516, 31);
            btnFiles.Size = new Size(112, 25);
            btnFiles.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnFiles.Click += new EventHandler(btnFiles_Click);

            Label lblPlaylist = new Label();
            lblPlaylist.Text = "Playlist:";
            lblPlaylist.AutoSize = true;
            lblPlaylist.Location = new Point(12, 66);

            cmbPlaylist = new ComboBox();
            cmbPlaylist.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbPlaylist.Location = new Point(12, 86);
            cmbPlaylist.Size = new Size(616, 23);
            cmbPlaylist.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            cmbPlaylist.SelectedIndexChanged += new EventHandler(cmbPlaylist_SelectedIndexChanged);

            Label lblTracks = new Label();
            lblTracks.Text = "PGS subtitle tracks to extract:";
            lblTracks.AutoSize = true;
            lblTracks.Location = new Point(12, 120);

            lvTracks = new ListView();
            lvTracks.View = View.Details;
            lvTracks.CheckBoxes = true;
            lvTracks.FullRowSelect = true;
            lvTracks.GridLines = true;
            lvTracks.HideSelection = false;
            lvTracks.Location = new Point(12, 140);
            lvTracks.Size = new Size(616, 150);
            lvTracks.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lvTracks.AllowDrop = true;
            lvTracks.MultiSelect = false;
            lvTracks.SelectedIndexChanged += new EventHandler(lvTracks_SelectedIndexChanged);
            lvTracks.DragEnter += new DragEventHandler(OnDragEnter);
            lvTracks.DragDrop += new DragEventHandler(OnDragDrop);
            lvTracks.Columns.Add("#", 40);
            lvTracks.Columns.Add("PID", 70);
            lvTracks.Columns.Add("Language", 80);
            lvTracks.Columns.Add("Output file", 400);

            Label lblOutput = new Label();
            lblOutput.Text = "Output folder:";
            lblOutput.AutoSize = true;
            GroupBox grpColors = new GroupBox();
            grpColors.Text = "Subtitle colors";
            grpColors.Location = new Point(12, 296);
            grpColors.Size = new Size(616, 200);
            grpColors.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            chkColors = new CheckBox();
            chkColors.Text = "Change the colors";
            chkColors.AutoSize = true;
            chkColors.Location = new Point(12, 22);
            chkColors.CheckedChanged += new EventHandler(chkColors_CheckedChanged);

            Label lblText = new Label();
            lblText.Text = "Text:";
            lblText.AutoSize = true;
            lblText.Location = new Point(190, 25);

            btnTextColor = CreateColorButton(Color.White, new Point(232, 19));

            Label lblOutline = new Label();
            lblOutline.Text = "Outline:";
            lblOutline.AutoSize = true;
            lblOutline.Location = new Point(332, 25);

            btnOutlineColor = CreateColorButton(Color.Black, new Point(386, 19));

            Label lblColorsNote = new Label();
            lblColorsNote.Text = "PGS subtitles are bitmaps: only their palette (text / outline colors) can be changed.";
            lblColorsNote.AutoSize = true;
            lblColorsNote.ForeColor = SystemColors.GrayText;
            lblColorsNote.Location = new Point(12, 46);

            grpColors.Controls.Add(chkColors);
            grpColors.Controls.Add(lblText);
            grpColors.Controls.Add(btnTextColor);
            grpColors.Controls.Add(lblOutline);
            grpColors.Controls.Add(btnOutlineColor);
            grpColors.Controls.Add(lblColorsNote);

            Label lblBefore = new Label();
            lblBefore.Text = "Before (first subtitle of the selected track):";
            lblBefore.AutoSize = true;
            lblBefore.Location = new Point(12, 68);

            Label lblAfter = new Label();
            lblAfter.Text = "After:";
            lblAfter.AutoSize = true;
            lblAfter.Location = new Point(308, 68);

            picBefore = CreatePreviewBox(new Point(12, 86));
            picAfter = CreatePreviewBox(new Point(308, 86));

            lblPreviewStatus = new Label();
            lblPreviewStatus.AutoSize = true;
            lblPreviewStatus.ForeColor = SystemColors.GrayText;
            lblPreviewStatus.Location = new Point(12, 180);

            chkLightBackground = new CheckBox();
            chkLightBackground.Text = "Light background";
            chkLightBackground.AutoSize = true;
            chkLightBackground.Location = new Point(490, 177);
            chkLightBackground.CheckedChanged += new EventHandler(chkLightBackground_CheckedChanged);
            UpdatePreviewBackground();

            grpColors.Controls.Add(lblBefore);
            grpColors.Controls.Add(lblAfter);
            grpColors.Controls.Add(picBefore);
            grpColors.Controls.Add(picAfter);
            grpColors.Controls.Add(lblPreviewStatus);
            grpColors.Controls.Add(chkLightBackground);
            UpdateColorControls();

            lblOutput.Location = new Point(12, 506);
            lblOutput.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;

            txtOutput = new TextBox();
            txtOutput.Location = new Point(12, 526);
            txtOutput.Size = new Size(498, 23);
            txtOutput.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            btnOutput = new Button();
            btnOutput.Text = "Browse...";
            btnOutput.Location = new Point(516, 525);
            btnOutput.Size = new Size(112, 25);
            btnOutput.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnOutput.Click += new EventHandler(btnOutput_Click);

            chkClose = new CheckBox();
            chkClose.Text = "Close this window after queuing the job";
            chkClose.AutoSize = true;
            chkClose.Checked = true;
            chkClose.Location = new Point(12, 566);
            chkClose.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;

            btnQueue = new Button();
            btnQueue.Text = "Queue";
            btnQueue.Location = new Point(440, 590);
            btnQueue.Size = new Size(90, 28);
            btnQueue.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnQueue.Click += new EventHandler(btnQueue_Click);

            btnClose = new Button();
            btnClose.Text = "Close";
            btnClose.DialogResult = DialogResult.Cancel;
            btnClose.Location = new Point(538, 590);
            btnClose.Size = new Size(90, 28);
            btnClose.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnClose.Click += new EventHandler(btnClose_Click);

            this.CancelButton = btnClose;

            this.Controls.Add(lblInput);
            this.Controls.Add(txtInput);
            this.Controls.Add(btnFolder);
            this.Controls.Add(btnFiles);
            this.Controls.Add(lblPlaylist);
            this.Controls.Add(cmbPlaylist);
            this.Controls.Add(lblTracks);
            this.Controls.Add(lvTracks);
            this.Controls.Add(grpColors);
            this.Controls.Add(lblOutput);
            this.Controls.Add(txtOutput);
            this.Controls.Add(btnOutput);
            this.Controls.Add(chkClose);
            this.Controls.Add(btnQueue);
            this.Controls.Add(btnClose);

            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #region colors
        private Button CreateColorButton(Color color, Point location)
        {
            Button button = new Button();
            button.Location = location;
            button.Size = new Size(78, 25);
            button.FlatStyle = FlatStyle.Standard;
            button.UseVisualStyleBackColor = false;
            SetButtonColor(button, color);
            button.Click += new EventHandler(btnColor_Click);
            return button;
        }

        private static void SetButtonColor(Button button, Color color)
        {
            button.BackColor = color;
            button.Text = String.Format("#{0:X2}{1:X2}{2:X2}", color.R, color.G, color.B);
            // readable text whatever the color
            button.ForeColor = (color.R * 299 + color.G * 587 + color.B * 114) / 1000 > 140 ? Color.Black : Color.White;
        }

        private void btnColor_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            using (ColorDialog dialog = new ColorDialog())
            {
                dialog.FullOpen = true;
                dialog.Color = button.BackColor;
                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    SetButtonColor(button, dialog.Color);
                    ShowPreview();
                }
            }
        }

        private void chkColors_CheckedChanged(object sender, EventArgs e)
        {
            UpdateColorControls();
            ShowPreview();
        }

        private void UpdateColorControls()
        {
            btnTextColor.Enabled = chkColors.Checked;
            btnOutlineColor.Enabled = chkColors.Checked;
        }

        private static int ToRgb(Color color)
        {
            return (color.R << 16) | (color.G << 8) | color.B;
        }
        #endregion

        #region preview
        private PictureBox CreatePreviewBox(Point location)
        {
            PictureBox box = new PictureBox();
            box.Location = location;
            box.Size = new Size(296, 86);
            box.SizeMode = PictureBoxSizeMode.Zoom;
            box.BorderStyle = BorderStyle.FixedSingle;
            return box;
        }

        private void chkLightBackground_CheckedChanged(object sender, EventArgs e)
        {
            UpdatePreviewBackground();
        }

        private void UpdatePreviewBackground()
        {
            Color background = (chkLightBackground != null && chkLightBackground.Checked) ? Color.FromArgb(200, 200, 200) : Color.FromArgb(70, 70, 70);
            picBefore.BackColor = background;
            picAfter.BackColor = background;
        }

        private void lvTracks_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvTracks.SelectedItems.Count > 0)
                LoadPreview();
        }

        private void PgsExtractorWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            previewRequest++;
            if (previewCts != null)
                previewCts.Cancel();
        }

        /// <summary>
        /// Reads (in the background) the first subtitle image of the selected track, then displays it.
        /// </summary>
        private void LoadPreview()
        {
            previewRequest++;
            if (previewCts != null)
                previewCts.Cancel();
            previewImage = null;
            ShowPreview();

            PlaylistEntry entry = cmbPlaylist.SelectedItem as PlaylistEntry;
            PgStreamInfo stream = null;
            if (lvTracks.SelectedItems.Count > 0)
                stream = lvTracks.SelectedItems[0].Tag as PgStreamInfo;
            if (entry == null || stream == null)
            {
                lblPreviewStatus.Text = "";
                return;
            }

            List<string> sources = new List<string>();
            if (entry.IsM2tsList)
                sources.AddRange(m2tsFiles);
            else
                sources.Add(entry.Path);

            int request = previewRequest;
            System.Threading.CancellationTokenSource cts = new System.Threading.CancellationTokenSource();
            previewCts = cts;
            int trackIndex = stream.Index;
            lblPreviewStatus.Text = "Loading the preview...";

            System.Threading.ThreadPool.QueueUserWorkItem(delegate
            {
                PgsPreviewImage image = null;
                string error = null;
                try
                {
                    image = PgsPreview.Find(sources, trackIndex, 128L << 20, cts.Token);
                }
                catch (Exception ex)
                {
                    error = ex.Message;
                }

                if (cts.IsCancellationRequested)
                    return;
                try
                {
                    BeginInvoke(new MethodInvoker(delegate
                    {
                        if (request != previewRequest || IsDisposed)
                            return;
                        previewImage = image;
                        lblPreviewStatus.Text = image != null ? "" : (error != null ? "No preview: " + error : "No preview available (no subtitle found, or encrypted stream).");
                        ShowPreview();
                    }));
                }
                catch (InvalidOperationException)
                {
                    // the window was closed in the meantime
                }
            });
        }

        /// <summary>
        /// Displays the original image and the one with the colors currently selected.
        /// </summary>
        private void ShowPreview()
        {
            Image oldBefore = picBefore.Image;
            Image oldAfter = picAfter.Image;

            if (previewImage == null)
            {
                picBefore.Image = null;
                picAfter.Image = null;
            }
            else
            {
                PgsColorOptions colors = new PgsColorOptions();
                colors.Enabled = chkColors.Checked;
                colors.TextRgb = ToRgb(btnTextColor.BackColor);
                colors.OutlineRgb = ToRgb(btnOutlineColor.BackColor);
                picBefore.Image = ToBitmap(previewImage.Render(null), previewImage.Width, previewImage.Height);
                picAfter.Image = ToBitmap(previewImage.Render(colors), previewImage.Width, previewImage.Height);
            }

            if (oldBefore != null)
                oldBefore.Dispose();
            if (oldAfter != null)
                oldAfter.Dispose();
        }

        private static Bitmap ToBitmap(int[] argb, int width, int height)
        {
            Bitmap bitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            BitmapData data = bitmap.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            try
            {
                Marshal.Copy(argb, 0, data.Scan0, argb.Length);    // stride = 4 * width for 32 bpp
            }
            finally
            {
                bitmap.UnlockBits(data);
            }
            return bitmap;
        }
        #endregion

        #region drag & drop
        private void OnDragEnter(object sender, DragEventArgs e)
        {
            e.Effect = e.Data.GetDataPresent(DataFormats.FileDrop) ? DragDropEffects.Copy : DragDropEffects.None;
        }

        private void OnDragDrop(object sender, DragEventArgs e)
        {
            string[] paths = e.Data.GetData(DataFormats.FileDrop) as string[];
            if (paths == null || paths.Length == 0)
                return;

            // a folder (Blu-ray folder) or one / several files (.mpls, .m2ts)
            foreach (string path in paths)
            {
                if (Directory.Exists(path))
                {
                    LoadFolder(path);
                    return;
                }
            }
            LoadFiles(paths);
        }
        #endregion

        #region input selection
        private void btnFolder_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                dialog.Description = "Select the Blu-ray folder (the folder which contains BDMV)";
                dialog.ShowNewFolderButton = false;
                if (!String.IsNullOrEmpty(txtInput.Text) && Directory.Exists(txtInput.Text))
                    dialog.SelectedPath = txtInput.Text;
                if (dialog.ShowDialog(this) == DialogResult.OK)
                    LoadFolder(dialog.SelectedPath);
            }
        }

        private void btnFiles_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Title = "Select a playlist (.mpls) or the M2TS file(s)";
                dialog.Filter = "Blu-ray files (*.mpls;*.m2ts)|*.mpls;*.m2ts|All files (*.*)|*.*";
                dialog.Multiselect = true;
                if (dialog.ShowDialog(this) == DialogResult.OK && dialog.FileNames.Length > 0)
                    LoadFiles(dialog.FileNames);
            }
        }

        /// <summary>
        /// finds the folder which contains the playlists (BDMV\PLAYLIST) starting from the selected folder
        /// </summary>
        private static string FindPlaylistFolder(string path)
        {
            List<string> candidates = new List<string>();
            candidates.Add(Path.Combine(Path.Combine(path, "BDMV"), "PLAYLIST"));
            candidates.Add(Path.Combine(path, "PLAYLIST"));
            candidates.Add(path);
            DirectoryInfo parent = Directory.GetParent(path.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar));
            if (parent != null)
                candidates.Add(Path.Combine(parent.FullName, "PLAYLIST"));

            foreach (string candidate in candidates)
            {
                if (Directory.Exists(candidate) && Directory.GetFiles(candidate, "*.mpls").Length > 0)
                    return candidate;
            }
            return null;
        }

        private static string FormatDuration(TimeSpan duration)
        {
            return String.Format("{0}:{1:00}:{2:00}", (int)duration.TotalHours, duration.Minutes, duration.Seconds);
        }

        private void LoadFolder(string folder)
        {
            string playlistFolder = FindPlaylistFolder(folder);
            if (playlistFolder == null)
            {
                MessageBox.Show(this, "No Blu-ray playlist (BDMV\\PLAYLIST\\*.mpls) was found in this folder.", "PGS Subtitle Extractor",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            List<PlaylistEntry> entries = new List<PlaylistEntry>();
            foreach (string file in Directory.GetFiles(playlistFolder, "*.mpls"))
            {
                try
                {
                    MplsPlaylist playlist = MplsPlaylist.Load(file);
                    PlaylistEntry entry = new PlaylistEntry();
                    entry.Path = file;
                    entry.Duration = playlist.Duration;
                    entry.Items = playlist.Items.Count;
                    entry.Text = String.Format("{0}   {1}   ({2} play item{3}, {4} PGS)", Path.GetFileName(file), FormatDuration(entry.Duration),
                        entry.Items, entry.Items > 1 ? "s" : "", playlist.Items[0].PgStreams.Count);
                    entries.Add(entry);
                }
                catch (BdFormatException)
                {
                    // not a readable playlist: ignored
                }
                catch (IOException)
                {
                }
            }

            if (entries.Count == 0)
            {
                MessageBox.Show(this, "None of the playlists of this folder could be read.", "PGS Subtitle Extractor",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            // the longest playlist (the main feature in general) first
            entries = entries.OrderByDescending(x => x.Duration).ThenBy(x => x.Path, StringComparer.OrdinalIgnoreCase).ToList();

            m2tsFiles.Clear();
            txtInput.Text = folder;
            FillPlaylists(entries);
        }

        private void LoadFiles(string[] files)
        {
            if (files.Length == 1 && files[0].EndsWith(".mpls", StringComparison.OrdinalIgnoreCase))
            {
                try
                {
                    MplsPlaylist playlist = MplsPlaylist.Load(files[0]);
                    PlaylistEntry entry = new PlaylistEntry();
                    entry.Path = files[0];
                    entry.Duration = playlist.Duration;
                    entry.Items = playlist.Items.Count;
                    entry.Text = String.Format("{0}   {1}   ({2} play item{3}, {4} PGS)", Path.GetFileName(files[0]), FormatDuration(entry.Duration),
                        entry.Items, entry.Items > 1 ? "s" : "", playlist.Items[0].PgStreams.Count);

                    m2tsFiles.Clear();
                    txtInput.Text = files[0];
                    FillPlaylists(new List<PlaylistEntry>() { entry });
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, ex.Message, "PGS Subtitle Extractor", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                return;
            }

            List<string> m2ts = files.Where(x => x.EndsWith(".m2ts", StringComparison.OrdinalIgnoreCase)).ToList();
            if (m2ts.Count == 0)
            {
                MessageBox.Show(this, "Select a playlist (.mpls) or one or several M2TS files.", "PGS Subtitle Extractor",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            // the clips are joined in the order of their name (00001.m2ts, 00002.m2ts, ...)
            m2ts.Sort(StringComparer.OrdinalIgnoreCase);
            m2tsFiles = m2ts;

            PlaylistEntry m2tsEntry = new PlaylistEntry();
            m2tsEntry.Path = m2ts[0];
            m2tsEntry.IsM2tsList = true;
            m2tsEntry.Text = m2ts.Count == 1 ? Path.GetFileName(m2ts[0]) : String.Format("{0} M2TS files ({1} ... {2})", m2ts.Count, Path.GetFileName(m2ts[0]), Path.GetFileName(m2ts[m2ts.Count - 1]));
            txtInput.Text = String.Join("; ", m2ts.Select(x => Path.GetFileName(x)).ToArray());
            FillPlaylists(new List<PlaylistEntry>() { m2tsEntry });
        }

        private void FillPlaylists(List<PlaylistEntry> entries)
        {
            updating = true;
            cmbPlaylist.Items.Clear();
            foreach (PlaylistEntry entry in entries)
                cmbPlaylist.Items.Add(entry);
            updating = false;

            cmbPlaylist.SelectedIndex = 0;
            RefreshTracks();
        }

        private void cmbPlaylist_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!updating)
                RefreshTracks();
        }
        #endregion

        /// <summary>
        /// lists the PGS tracks of the selected playlist / M2TS file(s)
        /// </summary>
        private void RefreshTracks()
        {
            lvTracks.Items.Clear();
            PlaylistEntry entry = cmbPlaylist.SelectedItem as PlaylistEntry;
            if (entry == null)
            {
                LoadPreview();
                return;
            }

            List<PgStreamInfo> streams;
            try
            {
                streams = PgsExtractor.ListTracks(entry.Path);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "PGS Subtitle Extractor", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                LoadPreview();
                return;
            }

            string prefix = Path.GetFileNameWithoutExtension(entry.Path);
            foreach (PgStreamInfo stream in streams)
            {
                string language = String.IsNullOrEmpty(stream.Language) ? "und" : stream.Language;
                string fileName = String.Format("{0}_T{1}_{2}.sup", prefix, stream.Index + 1, language);
                ListViewItem item = new ListViewItem(new string[] { (stream.Index + 1).ToString(), String.Format("0x{0:X4}", stream.Pid), language, fileName });
                item.Checked = true;
                item.Tag = stream;
                lvTracks.Items.Add(item);
            }

            if (lvTracks.Items.Count > 0)
                lvTracks.Items[0].Selected = true;      // displays the preview
            else
                LoadPreview();

            if (streams.Count == 0)
            {
                MessageBox.Show(this, "No PGS subtitle stream was found in " + Path.GetFileName(entry.Path) + ".", "PGS Subtitle Extractor",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnOutput_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                dialog.Description = "Select the output folder of the .sup files";
                if (!String.IsNullOrEmpty(txtOutput.Text) && Directory.Exists(txtOutput.Text))
                    dialog.SelectedPath = txtOutput.Text;
                if (dialog.ShowDialog(this) == DialogResult.OK)
                    txtOutput.Text = dialog.SelectedPath;
            }
        }

        private void btnQueue_Click(object sender, EventArgs e)
        {
            PlaylistEntry entry = cmbPlaylist.SelectedItem as PlaylistEntry;
            if (entry == null || lvTracks.Items.Count == 0)
            {
                MessageBox.Show(this, "Select a Blu-ray folder, a playlist or M2TS files first.", "Queue Job",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (String.IsNullOrEmpty(txtOutput.Text))
            {
                MessageBox.Show(this, "Configure the output folder prior to queueing the job.", "Queue Job",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            List<PgsTrackRequest> requests = new List<PgsTrackRequest>();
            foreach (ListViewItem item in lvTracks.Items)
            {
                if (!item.Checked)
                    continue;

                PgStreamInfo stream = (PgStreamInfo)item.Tag;
                PgsTrackRequest request = new PgsTrackRequest();
                request.Index = stream.Index;
                request.Language = stream.Language;
                request.Output = Path.Combine(txtOutput.Text, item.SubItems[3].Text);
                requests.Add(request);
            }

            if (requests.Count == 0)
            {
                MessageBox.Show(this, "Select at least one track to extract.", "Queue Job",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            List<string> source = entry.IsM2tsList ? new List<string>(m2tsFiles) : new List<string>() { entry.Path };
            PgsColorOptions colors = new PgsColorOptions();
            colors.Enabled = chkColors.Checked;
            colors.TextRgb = ToRgb(btnTextColor.BackColor);
            colors.OutlineRgb = ToRgb(btnOutlineColor.BackColor);

            MainForm.Instance.Jobs.AddJobsToQueue(new PgsExtractJob(source, txtOutput.Text, requests, colors));

            if (chkClose.Checked)
                this.Close();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }

    public class PgsExtractorTool : ITool
    {
        #region ITool Members

        public string Name
        {
            get { return "PGS Subtitle Extractor"; }
        }

        public void Run(MainForm info)
        {
            PgsExtractorWindow form = new PgsExtractorWindow();
            form.Show();
        }

        public Shortcut[] Shortcuts
        {
            get { return new Shortcut[0]; }
        }

        #endregion

        #region IIDable Members

        public string ID
        {
            get { return Name; }
        }

        #endregion
    }
}
