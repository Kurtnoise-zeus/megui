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
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;

using MeGUI.core.util;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace MeGUI
{
    public delegate void FileBarEventHandler(FileBar sender, FileBarEventArgs args);
    
    public partial class FileBar : UserControl
    {
        public event FileBarEventHandler FileSelected;
        NotifyCounter raiseEvent = new NotifyCounter();
        private string oldName;

        public FileBar()
        {
            InitializeComponent();
            DragDropUtil.RegisterSingleFileDragDrop(filename, SetFilename, delegate() { return Filter; });

            if (MainForm.Instance == null)
                return;

            // DPI rescale
            if (MainForm.Instance != null)
                openButton.Height = filename.Height + MainForm.Instance.Settings.DPIRescale(3);
        }

        #region properties

        private bool saveMode = false;
        [Category("FileBar"),
            Description("When this property is true, the dialogue to save files/folders is used instead of the one to open these."),
            DefaultValue(false)]
        public bool SaveMode
        {
            get { return saveMode; }
            set 
            { 
                saveMode = value; 
                filename.AllowDrop = !value; 
            }
        }

        [Category("FileBar"),
            Description("When this property is true, the TextBox is set to read only."),
            DefaultValue(true)]
        public bool ReadOnly
        {
            get { return filename.ReadOnly; }
            set { filename.ReadOnly = value; }
        }

        private string title = null;
        [Category("FileBar"),
            Description("The title of the open/save dialogue."),
            DefaultValue(null)]
        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        [Category("FileBar"),
            Description("The selected file or folder path."),
            DefaultValue(null)]
        public string Filename
        {
            get { return filename.Text; }
            set
            {
                using (IDisposable wrapper = raiseEvent.Wrap())
                {
                    filename.Text = value;
                    oldName = value;
                }
            }
        }

        private bool folderMode = false;
        [Category("FileBar"),
           Description("When this property is true, a folder instead of a file has to be choosen."),
           DefaultValue(false)]
        public bool FolderMode
        {
            get { return folderMode; }
            set { folderMode = value; }
        }

        private string filter = null;
        [Category("FileBar"),
           Description("The filter list e.g. All files (*.*)|*.*"),
           DefaultValue(null)]
        public string Filter
        {
            get { return filter; }
            set { filter = value; }
        }

        private int filterIndex = 0;
        [Category("FileBar"),
           Description("The by default selected filter index position."),
           DefaultValue(0)]
        public int FilterIndex
        {
            get { return filterIndex; }
            set { filterIndex = value; }
        }

        private string folderPath = null;
        /// <summary>
        /// Sets the intial folder 
        /// </summary>
        /// <returns></returns>
        public void SetInitialFolder(string strPath)
        {
            folderPath = strPath;
        }

        #endregion

        private void openButton_Click(object sender, EventArgs e)
        {
            if (folderMode)
            {
                using (FolderBrowserDialog dialog = new FolderBrowserDialog())
                {
                    if (!String.IsNullOrEmpty(folderPath))
                        dialog.SelectedPath = folderPath;
                    if (dialog.ShowDialog() == DialogResult.OK)
                        SetFilename(dialog.SelectedPath);
                }
            }
            else
            {
                using (FileDialog dialog = saveMode ? (FileDialog)new SaveFileDialog() : (FileDialog)new OpenFileDialog())
                {
                    if (!String.IsNullOrEmpty(folderPath))
                        dialog.InitialDirectory = folderPath;
                        dialog.Filter = filter;
                        dialog.FilterIndex = filterIndex;
                        dialog.Title = title;
                    if (!string.IsNullOrEmpty(Filename))
                    {
                        try
                        { 
                            dialog.InitialDirectory = Path.GetDirectoryName(Filename);
                            dialog.FileName = Path.GetFileName(Filename);
                        }
                        catch { }
                    }
                    if (dialog.ShowDialog() == DialogResult.OK)
                        SetFilename(dialog.FileName);
                }
            }
        }

        public void PerformClick()
        {
            openButton.PerformClick();
        }

        private void SetFilename(string filename)
        {
            oldName = this.filename.Text;
            using (IDisposable a = raiseEvent.Wrap())
            {
                this.filename.Text = filename;
            }
            TriggerEvent();
        }

        private void TriggerEvent()
        {
            if (raiseEvent.Ready && FileSelected != null)
                FileSelected(this, new FileBarEventArgs(oldName, filename.Text));
            oldName = filename.Text;
        }

        private void filename_TextChanged(object sender, EventArgs e)
        {
            SetFilename(filename.Text);
            TriggerEvent();
        }
    }

    public class FileBarEventArgs : EventArgs
    {
        public readonly string OldFileName;
        public readonly string NewFileName;
        public FileBarEventArgs(string oldName, string newName)
            : base()
        {
            OldFileName = oldName;
            NewFileName = newName;
        }
    }
}