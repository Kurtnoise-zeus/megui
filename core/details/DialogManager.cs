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
using System.Windows.Forms;

using MeGUI.core.gui.MessageBoxExLib;

namespace MeGUI
{
    public enum DuplicateResponse { OVERWRITE, RENAME, SKIP, ABORT };

    public class DialogManager
    {
        private MainForm mainForm;

        public DialogManager(MainForm mainForm)
        {
            this.mainForm = mainForm;
        }

        /// <summary>
        /// Creates a message box with the given text, title and icon. Also creates a 'don't show me again' checkbox
        /// </summary>
        /// <param name="text">The text to display</param>
        /// <param name="caption">The window title</param>
        /// <param name="icon">The icon to display</param>
        /// <returns>The newly created message box</returns>
        private MessageBoxEx createMessageBox(string text, string caption, MessageBoxIcon icon)
        {
            MessageBoxEx msgBox = new MessageBoxEx();
            msgBox.Caption = caption;
            msgBox.Text = text;
            msgBox.Icon = icon;
            msgBox.AllowSaveResponse = true;
            msgBox.SaveResponseText = "Don't ask me this again";
            return msgBox;
        }
        
        /// <summary>
        /// Shows a message dialog (without a question) with a 'don't ask again' checkbox
        /// </summary>
        /// <param name="text">The text to display</param>
        /// <param name="caption">The window title</param>
        /// <param name="icon">The icon to display</param>
        /// <returns>Whether to show this again</returns>
        private bool showMessage(string text, string caption, MessageBoxIcon icon)
        {
            MessageBoxEx msgBox = createMessageBox(text, caption, icon);
            msgBox.AddButtons(MessageBoxButtons.OK);
            msgBox.Show();
            return !msgBox.SaveResponseChecked;
        }

        /// <summary>
        /// Shows a custom dialog built on the MessageBoxEx system
        /// </summary>
        /// <param name="text">The text to display</param>
        /// <param name="caption">The window title to display</param>
        /// <param name="icon">The icon to display</param>
        /// <param name="askAgain">Returns whether to show this dialog again</param>
        /// <returns>true if the user pressed yes, false otherwise</returns>
        private bool askAbout(string text, string caption, MessageBoxIcon icon, out bool askAgain)
        {
            return askAbout(text, caption, "Yes", "No", icon, out askAgain);
        }

        /// <summary>
        /// Shows a custom dialog built on the MessageBoxEx system
        /// </summary>
        /// <param name="text">The text to display</param>
        /// <param name="caption">The window title to display</param>
        /// <param name="button1Text">The text on the first button</param>
        /// <param name="button2Text">The text on the second button</param>
        /// <param name="icon">The icon to display</param>
        /// <param name="askAgain">Returns whether to ask again</param>
        /// <returns>true if button 1 was pressed, false otherwise</returns>
        private bool askAbout(string text, string caption, string button1Text, string button2Text,
            MessageBoxIcon icon, out bool askAgain)
        {
            MessageBoxEx msgBox = createMessageBox(text, caption, icon);

            msgBox.AddButton(button1Text, "true");
            msgBox.AddButton(button2Text, "false");

            string sResult = msgBox.Show();
            askAgain = !msgBox.SaveResponseChecked;
            return (sResult.Equals("true"));
        }

        /// <summary>
        /// Shows a custom dialog built on the MessageBoxEx system
        /// </summary>
        /// <param name="text">The text to display</param>
        /// <param name="caption">The window title to display</param>
        /// <param name="button1Text">The text on the first button</param>
        /// <param name="button2Text">The text on the second button</param>
        /// <param name="icon">The icon to display</param>
        /// <returns>0, 1 or 2 depending on the button pressed</returns>
        private int askAbout2(string text, string caption, string button1Text, string button2Text, MessageBoxIcon icon)
        {
            MessageBoxEx msgBox = createMessageBox(text, caption, icon);
            msgBox.AddButton(button1Text, "0");
            msgBox.AddButton(button2Text, "1");
            msgBox.AllowSaveResponse = false;
            string sResult = msgBox.Show();
            return Int32.Parse(sResult);
        }

        /// <summary>
        /// Shows a custom dialog built on the MessageBoxEx system
        /// </summary>
        /// <param name="text">The text to display</param>
        /// <param name="caption">The window title to display</param>
        /// <param name="button1Text">The text on the first button</param>
        /// <param name="button2Text">The text on the second button</param>
        /// <param name="button3Text">The text on the third button</param>
        /// <param name="icon">The icon to display</param>
        /// <returns>0, 1 or 2 depending on the button pressed</returns>
        private int askAbout3(string text, string caption, string button1Text, string button2Text,
            string button3Text, MessageBoxIcon icon)
        {
            MessageBoxEx msgBox = createMessageBox(text, caption, icon);

            msgBox.AddButton(button1Text, "0");
            msgBox.AddButton(button2Text, "1");
            msgBox.AddButton(button3Text, "2");

            msgBox.AllowSaveResponse = false;

            string sResult = msgBox.Show();
            return Int32.Parse(sResult);
        }

        public bool overwriteJobOutput(string outputname)
        {
            if (mainForm.Settings.DialogSettings.AskAboutOverwriteJobOutput)
            {
                bool askAgain;
                bool bResult = askAbout("The output file '" + outputname + "' already exists. Would you like to overwrite?",
                    "File Already Exists", MessageBoxIcon.Warning, out askAgain);

                mainForm.Settings.DialogSettings.AskAboutOverwriteJobOutput = askAgain;
                mainForm.Settings.DialogSettings.OverwriteJobOutputResponse = bResult;
                return bResult;
            }
            return mainForm.Settings.DialogSettings.OverwriteJobOutputResponse;
        }

        public bool overwriteProfile(string profname)
        {
            if (mainForm.Settings.DialogSettings.AskAboutDuplicates)
            {
                if (MainForm.Instance.UpdateHandler.UpdateMode != UpdateMode.Automatic)
                {
                    bool askAgain;
                    bool bResult = askAbout("Problem adding profile '"
                        + profname + "':\r\none with the same name already exists. \r\nWhat do you want to do?",
                         "Duplicate profile", "Overwrite profile", "Skip profile", MessageBoxIcon.Exclamation, out askAgain);

                    mainForm.Settings.DialogSettings.AskAboutDuplicates = askAgain;
                    mainForm.Settings.DialogSettings.DuplicateResponse = bResult;
                    return bResult;
                }
                else
                    return false; 
            }
            return mainForm.Settings.DialogSettings.DuplicateResponse;
        }
         
        public bool useOneClick()
        {
            if (mainForm.Settings.DialogSettings.AskAboutVOBs)
            {
                bool askAgain;
                bool bResult = askAbout("Do you want to open this file with\r\n" +
                    "- One Click Encoder (fully automated, easy to use) or\r\n" +
                    "- File Indexer (manual, advanced)?", "Please choose your preferred tool", 
                    "One Click Encoder", "File Indexer", MessageBoxIcon.Question, out askAgain);

                mainForm.Settings.DialogSettings.AskAboutVOBs = askAgain;
                mainForm.Settings.DialogSettings.UseOneClick = bResult;
                return bResult;
            }
            return mainForm.Settings.DialogSettings.UseOneClick;
        }

        /// <summary>
        /// Gets the information how the file can be opened
        /// </summary>
        /// <param name="videoInput"></param>
        /// <returns>
        /// -1 file cannot be opened
        ///  0 OneClick Encoder
        ///  1 File Indexer
        ///  2 AviSource
        ///  3 DirectShowSource
        /// </returns>
        public int AVSCreatorOpen(string videoInput)
        {
            int iResult = -1;
            MediaInfoFile iFile = new MediaInfoFile(videoInput);
            FileIndexerWindow.IndexType oIndexer = FileIndexerWindow.IndexType.NONE;

            // check if OCE or FileIndexer can be used
            if (!iFile.recommendIndexer(out oIndexer))
            {
                // they cannot be used
                if (iFile.isAVISourceIndexable(false))
                    iResult = 2;
                else if (iFile.isDirectShowSourceIndexable())
                    iResult = 3;
                return iResult;
            }

            // OCE or File Indexer can be used - should DirectShowSource/AVISource be tried as well?

            if (MainForm.Instance.Settings.EnableDirectShowSource)
            {
                if (iFile.isAVISourceIndexable(false))
                {
                    iResult = askAbout3("Do you want to open this file with\r\n" +
                        "- One Click Encoder (fully automated, easy to use) or\r\n" +
                        "- File Indexer (manual, advanced) or \r\n" +
                        "- AviSource (manual, expert, may cause problems)?", "Please choose your prefered way to open this file",
                        "One Click Encoder", "File Indexer", "AviSource", MessageBoxIcon.Question);
                    return iResult;
                }
                else if (iFile.isDirectShowSourceIndexable())
                {
                    iResult = askAbout3("Do you want to open this file with\r\n" +
                        "- One Click Encoder (fully automated, easy to use) or\r\n" +
                        "- File Indexer (manual, advanced) or \r\n" +
                        "- DirectShowSource (manual, expert, may cause problems)?", "Please choose your prefered way to open this file",
                        "One Click Encoder", "File Indexer", "DirectShowSource", MessageBoxIcon.Question);
                    if (iResult == 2)
                        iResult = 3;
                    return iResult;
                }
            }

            string strText = string.Empty;
            if (MainForm.Instance.Settings.EnableDirectShowSource)
                strText = "\r\n\r\nDirectShowSource or AVISource is not supported.";
            else
                strText = "\r\n\r\nDirectShowSource or AVISource is not recommended\r\nand has to be enabled in the settings if required.";

            iResult = askAbout2("Do you want to open this file with\r\n" +
                "- One Click Encoder (fully automated, easy to use) or\r\n" +
                "- File Indexer (manual, advanced)?" + strText, "Please choose your preferred tool",
                "One Click Encoder", "File Indexer", MessageBoxIcon.Question);
            return iResult;
        }

        public bool createJobs(string error)
        {
            if (mainForm.Settings.DialogSettings.AskAboutError)
            {
                bool askAgain;
                bool bResult = askAbout(string.Format("Your AviSynth clip has the following problem:\r\n{0}\r\nContinue anyway?", error),
                    "Problem in AviSynth script", MessageBoxIcon.Warning, out askAgain);

                mainForm.Settings.DialogSettings.AskAboutError = askAgain;
                mainForm.Settings.DialogSettings.ContinueDespiteError = bResult;
                return bResult;
            }
            return mainForm.Settings.DialogSettings.ContinueDespiteError;
        }

        public bool AddConvertTo(string colorspace_original, string colorspace_target)
        {
            if (mainForm.Settings.DialogSettings.AskAboutColorConversion)
            {
                bool askAgain;
                bool bResult = askAbout("The colorspace " + colorspace_original + " of your clip is not in " + colorspace_target + ".\r\n" +
                                        "Do you want to add the necessary ConvertTo function(s) to the end of the script?",
                                        "Incorrect Colorspace", MessageBoxIcon.Warning, out askAgain);

                mainForm.Settings.DialogSettings.AskAboutColorConversion = askAgain;
                mainForm.Settings.DialogSettings.AddConvertTo = bResult;
                return bResult;
            }
            return mainForm.Settings.DialogSettings.AddConvertTo;
        }

        public bool AskAboutUpdates()
        {
            if (mainForm.Settings.DialogSettings.AskAboutUpdates)
            {
                bool askAgain;
                bool bResult = askAbout("There are updated packages available that may be necessary for MeGUI\r\n" + 
                    "to work correctly. Some of them are binary files subject to patents, so\r\n" + 
                    "they could be in violation of your local laws. MeGUI will let you choose\r\n" + 
                    "what files to update but please check your local laws about patents\r\n" + 
                    "before proceeding. By clicking on the 'Yes' button you declare you have\r\n" + 
                    "read and accepted this information.\n\rDo you wish to proceed reviewing the updates?",
                                        "Updates Available", MessageBoxIcon.Question, out askAgain);

                if (!bResult && !askAgain)
                    MainForm.Instance.Settings.AutoUpdate = false;
                else
                    mainForm.Settings.DialogSettings.AskAboutUpdates = askAgain;
                return bResult;
            }
            return true;
        }

        public bool DeleteIntermediateFiles(List<string> arrFiles)
        {
            if (mainForm.Settings.DialogSettings.AskAboutIntermediateDelete)
            {
                string strFiles = string.Empty; ;
                foreach (string file in arrFiles)
                    strFiles += "\r\n" + file;

                bool askAgain;
                bool bResult = askAbout("Do you really want to delete the intermediate files below?\r\nThese files may still be required as the job did not finish successfully.\r\n" + strFiles,
                                        "Confirm deletion of intermediate files", MessageBoxIcon.Warning, out askAgain);

                mainForm.Settings.DialogSettings.AskAboutIntermediateDelete = askAgain;
                mainForm.Settings.DialogSettings.IntermediateDelete = bResult;
                return bResult;
            }
            return mainForm.Settings.DialogSettings.IntermediateDelete;
        }
    }
}