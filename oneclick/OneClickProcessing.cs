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
using System.Windows.Forms;

using MeGUI.core.util;

namespace MeGUI.packages.tools.oneclick
{
    public class OneClickProcessing
    {
        private OneClickWindow oOneClickWindow;
        private string strInput;
        private LogItem _log;
        private OneClickSettings _oSettings;
        private bool _bAbort;

        public OneClickProcessing(OneClickWindow oWindow, String strFileOrFolderName, OneClickSettings oSettings, LogItem oLog)
        {
            this.oOneClickWindow = oWindow;
            this.strInput = strFileOrFolderName;
            this._log = oLog;
            this._oSettings = oSettings;
            this._bAbort = false;

            if (!getInputDVDorBlurayBased(oSettings))
                if (this._bAbort || !getInputFolderBased(oSettings))
                    if (this._bAbort || !getInputFileBased(oSettings))
                        this.oOneClickWindow.setOpenFailure(this._bAbort);   
        }

        // batch processing
        public OneClickProcessing(OneClickWindow oWindow, List<OneClickFilesToProcess> arrFilesToProcess, OneClickSettings oSettings, LogItem oLog)
        {
            this.oOneClickWindow = oWindow;
            this._log = oLog;
            this._bAbort = false;

            List<OneClickFilesToProcess> arrFilesToProcessNew = new List<OneClickFilesToProcess>();
            MediaInfoFile iFile = null;

            foreach (OneClickFilesToProcess oFileToProcess in arrFilesToProcess)
            {
                if (iFile == null)
                {
                    MediaInfoFile iFileTemp = new MediaInfoFile(oFileToProcess.FilePath, ref _log, oFileToProcess.PGCNumber, oFileToProcess.AngleNumber);
                    if (iFileTemp.recommendIndexer(oSettings.IndexerPriority))
                        iFile = iFileTemp;
                    else if (iFileTemp.ContainerFileTypeString.Equals("AVS"))
                    {
                        iFile = iFileTemp;
                        iFile.IndexerToUse = FileIndexerWindow.IndexType.NONE;
                    }
                    else
                        _log.LogEvent(oFileToProcess.FilePath + " cannot be processed as no indexer can be used. skipping...");
                }
                else
                    arrFilesToProcessNew.Add(oFileToProcess);
            }
            if (iFile != null)
                oOneClickWindow.setInputData(iFile, arrFilesToProcessNew);
            else
                oOneClickWindow.setInputData(null, new List<OneClickFilesToProcess>()); // not demuxable
        }

        /// <summary>
        /// checks if the files/folders can be processed as DVD/Blu-ray
        /// </summary>
        /// <returns>true if the files/folder can be processed as DVD or Blu-ray, false otherwise</returns>
        private bool getInputDVDorBlurayBased(OneClickSettings oSettings)
        {
            if (FileUtil.RegExMatch(this.strInput, @"\\playlist\\\d{5}\.mpls\z", true))
            {
                // mpls file selected - if Blu-ray structure exists, the playlist will be directly openend
                string checkFolder = Path.Combine(Path.GetDirectoryName(Path.GetDirectoryName(this.strInput)), "STREAM");
                if (Directory.Exists(checkFolder) && Directory.GetFiles(checkFolder, "*.m2ts").Length > 0)
                    return false;
            }

            using (frmStreamSelect frm = new frmStreamSelect(this.strInput, SelectionMode.MultiExtended))
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
                {
                    // abort as the user clicked not on OK
                    this._bAbort = true;
                    return false;
                }

                // check how many playlists have been selected
                List<ChapterInfo> oChapterList = frm.SelectedMultipleChapterInfo;
                if (oChapterList.Count == 0)
                    return false;
                
                List<OneClickFilesToProcess> arrFilesToProcess = new List<OneClickFilesToProcess>();
                MediaInfoFile iFile = null;

                foreach (ChapterInfo oChapterInfo in oChapterList)
                {
                    string strSourceFile = string.Empty;
                    if (frm.IsDVDSource)
                        strSourceFile = Path.Combine(Path.GetDirectoryName(oChapterInfo.SourceFilePath), oChapterInfo.Title + "_1.VOB");
                    else
                        strSourceFile = oChapterInfo.SourceFilePath;

                    if (!File.Exists(strSourceFile))
                    {
                        _log.LogEvent(strSourceFile + " cannot be found. skipping...");
                        continue;
                    }

                    if (iFile == null)
                    {
                        MediaInfoFile iFileTemp = new MediaInfoFile(strSourceFile, ref _log, frm.IsDVDSource ? oChapterInfo.PGCNumber : 1,
                            frm.IsDVDSource ? oChapterInfo.AngleNumber : 0);
                        if (iFileTemp.recommendIndexer(oSettings.IndexerPriority))
                            iFile = iFileTemp;
                        else
                            _log.LogEvent(strSourceFile + " cannot be processed as no indexer can be used. skipping...");
                    }
                    else
                        arrFilesToProcess.Add(new OneClickFilesToProcess(strSourceFile, frm.IsDVDSource ? oChapterInfo.PGCNumber : 1,
                            frm.IsDVDSource ? oChapterInfo.AngleNumber : 0));
                }

                if (iFile != null)
                {
                    oOneClickWindow.setInputData(iFile, arrFilesToProcess);
                    return true;
                }
                else
                    return false;
            }
        }

        /// <summary>
        /// checks if the input folder can be processed
        /// </summary>
        /// <returns>true if the folder can be processed, false otherwise</returns>
        private bool getInputFolderBased(OneClickSettings oSettings)
        {
            List<OneClickFilesToProcess> arrFilesToProcess = new List<OneClickFilesToProcess>();
            MediaInfoFile iFile = null;

            if (!Directory.Exists(this.strInput))
                return false;

            foreach (string strFileName in Directory.GetFiles(this.strInput))
            {
                if (iFile == null)
                {
                    MediaInfoFile iFileTemp = new MediaInfoFile(strFileName, ref _log);
                    if (iFileTemp.recommendIndexer(oSettings.IndexerPriority))
                        iFile = iFileTemp;
                    else if (iFileTemp.ContainerFileTypeString.Equals("AVS"))
                    {
                        iFile = iFileTemp;
                        iFile.IndexerToUse = FileIndexerWindow.IndexType.NONE;
                    }
                    else
                        _log.LogEvent(strFileName + " cannot be processed as no indexer can be used. skipping...");
                }
                else
                    arrFilesToProcess.Add(new OneClickFilesToProcess(strFileName, 1, 0));
            }
            if (iFile != null)
            {
                oOneClickWindow.setInputData(iFile, arrFilesToProcess);
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// checks if the input file can be processed
        /// </summary>
        /// <returns>true if the file can be processed, false otherwise</returns>
        private bool getInputFileBased(OneClickSettings oSettings)
        {
            if (!File.Exists(this.strInput))
                return false;

            MediaInfoFile iFile = new MediaInfoFile(this.strInput, ref this._log);
            if (iFile.recommendIndexer(oSettings.IndexerPriority))
                return getInputIndexerBased(iFile, oSettings);
            else if (iFile.ContainerFileTypeString.Equals("AVS"))
            {
                iFile.IndexerToUse = FileIndexerWindow.IndexType.NONE;
                return getInputIndexerBased(iFile, oSettings);
            }
            return false;
        }

        private bool getInputIndexerBased(MediaInfoFile iFile, OneClickSettings oSettings)
        {
            oOneClickWindow.setInputData(iFile, new List<OneClickFilesToProcess>());
            return true;
        }
    }
}