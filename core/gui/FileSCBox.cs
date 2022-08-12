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
using System.Windows.Forms;

using MeGUI.core.util;

namespace MeGUI.core.gui
{
    public class FileSCBox : StandardAndCustomComboBox
    {
        public enum FileSCBoxType
        {
            Default, OC_FILE_AND_FOLDER, OC_FILE
        };

        public FileSCBox() : base("Clear user-selected files...", "Select file...")
        {
            base.Getter = new Getter<object>(getter);
            base.bSaveEveryItem = true;
        }

        OpenFileDialog ofd = new OpenFileDialog();
        FolderBrowserDialog fbd = new FolderBrowserDialog();
        FileSCBoxType oType = FileSCBoxType.Default;

        private object getter()
        {
            if (ofd.ShowDialog() == DialogResult.OK)
                return ofd.FileName;
            return null;
        }

        private object getterFolder()
        {
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                MainForm.Instance.Settings.LastUsedOneClickFolder = fbd.SelectedPath;
                return fbd.SelectedPath;
            }
            return null;
        }

        public string Filter
        {
            get { return ofd.Filter; }
            set { ofd.Filter = value; }
        }

        public FileSCBoxType Type
        {
            get { return oType; }
            set 
            { 
                oType = value;
                if (oType == FileSCBoxType.OC_FILE || oType == FileSCBoxType.OC_FILE_AND_FOLDER)
                {
                    base.SetFileSCBoxType("Select file...", "Select folder...", oType);
                    if (oType == FileSCBoxType.OC_FILE_AND_FOLDER)
                    {
                        base.GetterFolder = new Getter<object>(getterFolder);
                        if (MainForm.Instance != null && System.IO.Directory.Exists(MainForm.Instance.Settings.LastUsedOneClickFolder))
                            fbd.SelectedPath = MainForm.Instance.Settings.LastUsedOneClickFolder;
                    }
                }
            }
        }
    }
}
