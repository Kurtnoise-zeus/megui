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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;

using MeGUI.core.util;

namespace MeGUI.core.gui
{
    public partial class ProfileImporter : MeGUI.core.gui.ProfilePorter
    {
        private static string askForZipFile()
        {
            OpenFileDialog inputChooser = new OpenFileDialog();
            inputChooser.Filter = "Zip archives|*.zip";
            inputChooser.Title = "Select your input file";

            if (inputChooser.ShowDialog() != DialogResult.OK)
                throw new CancelledException();
            
            return inputChooser.FileName;
        }

        private DirectoryInfo tempFolder;
        private DirectoryInfo extraFiles;
        private MainForm mainForm;
        private XmlDocument ContextHelp = new XmlDocument();

        public ProfileImporter(MainForm mf, bool bSilentError)
            :this(mf, askForZipFile(), bSilentError)
        {}

        public ProfileImporter(MainForm mf, string filename, bool bSilentError)
            : this(mf, File.OpenRead(filename), bSilentError)
        {}

        public ProfileImporter(MainForm mf, Stream s, bool bSilentError)
        {
            InitializeComponent();
            
            mainForm = mf;
            mainForm.ImportProfileSuccessful = false;

            tempFolder = FileUtil.CreateTempDirectory();
            FileUtil.ExtractZipFile(s, tempFolder.FullName);

            extraFiles = FileUtil.ensureDirectoryExists(Path.Combine(tempFolder.FullName, "extra"));
            List<Profile> ps = ProfileManager.ReadAllProfiles(tempFolder.FullName, bSilentError);

            fixFileNames(ps, createInitSubTable());

            Profiles = ps.ToArray();
        }

        public bool ErrorDuringInit()
        {
            if (Profiles.Length == 0)
                return true;
            else
                return false;
        }

        public void AutoImport()
        {
            checkAllToolStripMenuItem_Click(null, null);
            import_Click(null, null);
        }

        private Dictionary<string, string> createInitSubTable()
        {
            Dictionary<string, string> d = new Dictionary<string, string>();

            foreach (FileInfo f in extraFiles.GetFiles())
                d[getZippedExtraFileName(f.Name)] = f.FullName;

            return d;
        }

        private List<string> extraFilesList
        {
            get { return new List<FileInfo>(extraFiles.GetFiles()).ConvertAll<string>(delegate(FileInfo f) { return f.FullName; }); }
        }

        private void import_Click(object sender, EventArgs e)
        {
            Util.CatchExceptionsAndTellUser("Error importing file", delegate
            {
                List<Profile> ps = SelectedAndRequiredProfiles;
                fixFileNames(ps,
                    copyExtraFilesToFolder(extraFilesList, Path.Combine(mainForm.MeGUIPath, "extra")));

                mainForm.Profiles.AddAll(ps.ToArray(), mainForm.DialogManager);

                DialogResult = DialogResult.OK;
                mainForm.ImportProfileSuccessful = true;

                this.Close();
            });
        }

        private void checkAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem ts = (ToolStripMenuItem)sender;
            for (int i = 0; i < profileList.Items.Count; i++)
            {
                profileList.SetItemChecked(i, true);
            } 
        }

        private void checkNoneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem ts = (ToolStripMenuItem)sender;
            for (int i = 0; i < profileList.Items.Count; i++)
            {
                profileList.SetItemChecked(i, false);
            } 
        }

        private string SelectHelpText(string node)
        {
            StringBuilder HelpText = new StringBuilder(64);

            try
            {
                string xpath = "/ContextHelp/Form[@name='PresetImporter']/" + node;
                XmlNodeList nl = ContextHelp.SelectNodes(xpath); // Return the details for the specified node

                if (nl.Count == 1) // if it finds the required HelpText, count should be 1
                {
                    HelpText.AppendLine(nl[0]["Text"].InnerText);
                    HelpText.AppendLine();
                }
                else // If count isn't 1, then theres no valid data.
                    HelpText.Append("Error: No data available");
            }
            catch
            {
                HelpText.Append("Error: No data available");
            }

            return (HelpText.ToString());
        }

        private void SetToolTips()
        {
            PresetImporterToolTip.SetToolTip(this.profileList, SelectHelpText("presetList"));
        }

        private void ProfileImporter_Shown(object sender, EventArgs e)
        {
            try
            {
                string p = System.IO.Path.Combine(Application.StartupPath, "Data");
                p = System.IO.Path.Combine(p, "ContextHelp.xml");
                ContextHelp.Load(p);
                SetToolTips();
            }
            catch
            {
                MessageBox.Show("The ContextHelp.xml file could not be found. Please check in the 'Data' directory to see if it exists. Help tooltips will not be available.", "File Not Found", MessageBoxButtons.OK);
            }           
        }
    }

    public class CancelledException : MeGUIException
    {
        public CancelledException() : base("User cancelled") { }
    }
}

