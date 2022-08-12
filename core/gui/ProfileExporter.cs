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
using System.Xml;

using MeGUI.core.util;

namespace MeGUI.core.gui
{
    public partial class ProfileExporter : MeGUI.core.gui.ProfilePorter
    {
        private XmlDocument ContextHelp = new XmlDocument();

        public ProfileExporter()
        {
            InitializeComponent();
            Profiles = MainForm.Instance.Profiles.AllProfiles;
        }

        private List<string> GetRequiredFiles(List<Profile> ps)
        {
            List<string> files = new List<string>();

            foreach (Profile p in ps)
                files.AddRange(p.BaseSettings.RequiredFiles);

            return Util.Unique(files);
        }

        private void Export_Click(object sender, EventArgs e)
        {
            this.Enabled = false;
            if (ExportProfiles())
            {
                MessageBox.Show("File export completed successfully", "Export Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
            }
            this.Enabled = true;
        }

        private bool ExportProfiles()
        {
            string tempFolderName = string.Empty;
            try
            {
                SaveFileDialog outputFilesChooser = new SaveFileDialog
                {
                    Title = "Choose your output file",
                    Filter = "Zip archives|*.zip"
                };
                if (outputFilesChooser.ShowDialog() != DialogResult.OK)
                    return false;

                DirectoryInfo tempFolder = FileUtil.CreateTempDirectory();
                tempFolderName = tempFolder.FullName;

                List<Profile> profs = SelectedAndRequiredProfiles;
                Dictionary<string, string> subTable =
                    copyExtraFilesToFolder(GetRequiredFiles(profs),
                    Path.Combine(tempFolder.FullName, "extra"));

                subTable = TurnValuesToZippedStyleName(subTable);

                fixFileNames(profs, subTable);

                if (!ProfileManager.WriteProfiles(tempFolder.FullName, profs))
                {
                    MessageBox.Show("An error occurred when saving the file", "Export Result", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                if (!FileUtil.CreateZipFile(tempFolder.FullName, outputFilesChooser.FileName))
                {
                    MessageBox.Show("An error occurred when saving the file", "Export Result", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            catch
            {
                return false;
            }
            FileUtil.DeleteDirectoryIfExists(tempFolderName, true);
            return true;
        }

        private Dictionary<string, string> TurnValuesToZippedStyleName(Dictionary<string, string> subTable)
        {
            Dictionary<string, string> newTable = new Dictionary<string, string>();
            foreach (string key in subTable.Keys)
                newTable[key] = getZippedExtraFileName(subTable[key]);
            return newTable;
        }

        private string SelectHelpText(string node)
        {
            StringBuilder HelpText = new StringBuilder(64);

            try
            {
                string xpath = "/ContextHelp/Form[@name='PresetExporter']/" + node;
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
            PresetExporterToolTip.SetToolTip(this.profileList, SelectHelpText("presetList"));
        }

        private void ProfileExporter_Shown(object sender, EventArgs e)
        {
            try
            {
                string p = System.IO.Path.Combine(Application.StartupPath, "Data");
                p = System.IO.Path.Combine(p, "ContextHelp.xml");
                ContextHelp.Load(p);
                SetToolTips();
            }
            catch { }
        }

        private void CheckAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem ts = (ToolStripMenuItem)sender;
            for (int i = 0; i < profileList.Items.Count; i++)
            {
                profileList.SetItemChecked(i, true);
            } 
        }

        private void CheckNoneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem ts = (ToolStripMenuItem)sender;
            for (int i = 0; i < profileList.Items.Count; i++)
            {
                profileList.SetItemChecked(i, false);
            } 
        }

        private void ProfileList_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            button2.Enabled = (profileList.CheckedItems.Count > 1 || e.NewValue == CheckState.Checked);
        }
    }
}