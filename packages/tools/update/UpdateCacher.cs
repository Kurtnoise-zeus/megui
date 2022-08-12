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
using System.Collections;
using System.Collections.Generic;
using System.IO;

using ICSharpCode.SharpZipLib.Zip;
using SevenZip;

using MeGUI.core.util;

namespace MeGUI
{
    class UpdateCacher
    {
        public static void RemoveOldFiles()
        {
            UpdateWindow.iUpgradeableCollection upgradeData = MainForm.Instance.UpdateHandler.UpdateData;

            try
            {
                string updateCache = MainForm.Instance.Settings.MeGUIUpdateCache;
                if (String.IsNullOrEmpty(updateCache) || !Directory.Exists(updateCache))
                    return;

                List<string> urls = new List<string>();
                foreach (UpdateWindow.iUpgradeable u in upgradeData)
                {
                    if (!String.IsNullOrEmpty(u.AvailableVersion.Url))
                        urls.Add(u.AvailableVersion.Url.ToLowerInvariant());
                    if (!urls.Contains(u.CurrentVersion.Url) && !String.IsNullOrEmpty(u.CurrentVersion.Url))
                        urls.Add(u.CurrentVersion.Url.ToLowerInvariant());
                }

                DirectoryInfo fi = new DirectoryInfo(updateCache);
                FileInfo[] files = fi.GetFiles("*.zip");
                foreach (FileInfo f in files)
                {
                    if (urls.IndexOf(f.Name.ToLowerInvariant()) >= 0)
                        continue;

                    if (f.Name.StartsWith("_obsolete_"))
                        continue;

                    f.LastWriteTimeUtc = DateTime.UtcNow;
                    File.Delete(Path.Combine(updateCache, "_obsolete_" + f.Name));
                    f.MoveTo(Path.Combine(updateCache, "_obsolete_" + f.Name));
                    MainForm.Instance.UpdateHandler.AddTextToLog("Marked file as obsolete: " + f.Name.Substring(10), ImageType.Information, false);
                }

                files = fi.GetFiles("*.7z");
                foreach (FileInfo f in files)
                {
                    if (urls.IndexOf(f.Name.ToLowerInvariant()) >= 0)
                        continue;

                    if (f.Name.StartsWith("_obsolete_"))
                        continue;

                    f.LastWriteTimeUtc = DateTime.UtcNow;
                    File.Delete(Path.Combine(updateCache, "_obsolete_" + f.Name));
                    f.MoveTo(Path.Combine(updateCache, "_obsolete_" + f.Name));
                    MainForm.Instance.UpdateHandler.AddTextToLog("Marked file as obsolete: " + f.Name.Substring(10), ImageType.Information, false);
                }

                if (!MainForm.Instance.Settings.AlwaysBackUpFiles)
                {
                    files = fi.GetFiles("_obsolete_*.*");
                    foreach (FileInfo f in files)
                    {
                        if (urls.IndexOf(f.Name.ToLowerInvariant()) >= 0)
                            continue;

                        // delete file if it is obsolete for more than 90 days
                        if (DateTime.Now - f.LastWriteTime > new TimeSpan(90, 0, 0, 0, 0))
                        {
                            f.Delete();
                            MainForm.Instance.UpdateHandler.AddTextToLog("Deleted obsolete file: " + f.Name.Substring(10), ImageType.Information, false);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MainForm.Instance.UpdateHandler.AddTextToLog("Old package data could not be cleaned: " + ex.Message, ImageType.Error, false);
            }

            if (MainForm.Instance.Settings.AlwaysBackUpFiles)
                return;

            try
            {
                string strMeGUILogPath = Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + @"\logs";
                if (String.IsNullOrEmpty(strMeGUILogPath) || !Directory.Exists(strMeGUILogPath))
                    return;

                DirectoryInfo fi = new DirectoryInfo(strMeGUILogPath);
                FileInfo[] files = fi.GetFiles("*.log");
                foreach (FileInfo f in files)
                {
                    // delete file if it is obsolete for more than 90 days
                    if (DateTime.Now - f.LastWriteTime > new TimeSpan(90, 0, 0, 0, 0))
                    {
                        f.Delete();
                        MainForm.Instance.UpdateHandler.AddTextToLog("Deleted obsolete file: " + f.Name.Substring(10), ImageType.Information, false);
                    }
                }
            }
            catch (Exception ex)
            {
                MainForm.Instance.UpdateHandler.AddTextToLog("Old log data could not be cleaned: " + ex.Message, ImageType.Error, false);
            }
        }

        public static UpdateWindow.ErrorState ManageBackups(string savePath, string name, bool bCopyFile)
        {
            try
            {
                if (File.Exists(savePath + ".backup"))
                    File.Delete(savePath + ".backup");
            }
            catch
            {
                MainForm.Instance.UpdateHandler.AddTextToLog("Outdated backup version of " + name + " could not be deleted. Check if it is in use.", ImageType.Error, true);
                return UpdateWindow.ErrorState.CouldNotRemoveBackup;
            }

            try
            {
                if (File.Exists(savePath))
                {
                    if (bCopyFile == false)
                        File.Move(savePath, (savePath + ".backup"));
                    else
                        File.Copy(savePath, (savePath + ".backup"));
                }
            }
            catch
            {
                MainForm.Instance.UpdateHandler.AddTextToLog("Old version of " + name + " could not be backed up correctly.", ImageType.Error, true);
                return UpdateWindow.ErrorState.CouldNotCreateBackup;
            }

            return UpdateWindow.ErrorState.Successful;
        }

        public static UpdateWindow.ErrorState PreparePackageFolder(string packageName)
        {
            ProgramSettings oPackage = UpdateCacher.GetPackage(packageName);
            if (oPackage == null)
                return UpdateWindow.ErrorState.Successful;

            string packagePath = Path.GetDirectoryName(oPackage.Path);
            if (MainForm.Instance.Settings.AlwaysBackUpFiles)
            {
                try
                {   
                    // remove all old backup files found
                    Array.ForEach(Directory.GetFiles(packagePath, "*.backup", SearchOption.AllDirectories), delegate (string path) { File.Delete(path); });
                }
                catch (Exception ex)
                {
                    MainForm.Instance.UpdateHandler.AddTextToLog("Outdated backup version of " + oPackage.DisplayName + " could not be deleted. Check if it is in use. " + ex.Message, ImageType.Error, true);
                    return UpdateWindow.ErrorState.CouldNotCreateBackup;
                }
            }

            try
            {
                DirectoryInfo fi = new DirectoryInfo(packagePath);
                FileInfo[] files = fi.GetFiles("*.*", SearchOption.AllDirectories);
                foreach (FileInfo f in files)
                {
                    // only continue when file can be deleted/renamed
                    bool bFound = false;
                    foreach (string strFile in oPackage.DoNotDeleteFilesOnUpdate)
                    {
                        if (f.FullName.ToLowerInvariant().Equals(strFile.ToLowerInvariant()))
                        {
                            bFound = true;
                            break;
                        }
                    }
                    foreach (string strFolder in oPackage.DoNotDeleteFoldersOnUpdate)
                    {
                        if (f.DirectoryName.ToLowerInvariant().StartsWith(strFolder.ToLowerInvariant()))
                        {
                            bFound = true;
                            break;
                        }
                    }
                    if (bFound)
                        continue;

                    if (!MainForm.Instance.Settings.AlwaysBackUpFiles)
                        f.Delete();
                    else
                        f.MoveTo(Path.Combine(f.Directory.FullName, f.Name + ".backup"));
                }
            }
            catch (Exception ex)
            {
                MainForm.Instance.UpdateHandler.AddTextToLog("Old version of " + oPackage.DisplayName + " could not be accessed correctly. Check if it is in use. " + ex.Message, ImageType.Error, true);
                return UpdateWindow.ErrorState.CouldNotCreateBackup;
            }

            return UpdateWindow.ErrorState.Successful;
        }

        public static void DeleteCacheFile(string p)
        {
            string localFilename = Path.Combine(MainForm.Instance.Settings.MeGUIUpdateCache, p);
            try
            {
                if (File.Exists(localFilename))
                    File.Delete(localFilename);
            }
            catch (Exception)
            {
                MainForm.Instance.UpdateHandler.AddTextToLog("Could not delete file " + localFilename, ImageType.Error, true);
            }
        }

        public static ProgramSettings GetPackage(string package)
        {
            foreach (ProgramSettings pSettings in MainForm.Instance.ProgramSettings)
            {
                if (!package.Equals(pSettings.Name))
                    continue;
                return pSettings;
            }

            return null;
        }

        public static bool IsPackage(string package)
        {
            if (UpdateCacher.GetPackage(package) != null)
                return true;

            switch (package)
            {
                case "audio":
                case "TXviD": return true;
                default: return false;
            }
        }

        public static bool CheckPackage(string package)
        {
            return CheckPackage(package, true, true);
        }

        public static bool CheckPackage(string package, bool enablePackage, bool forceUpdate)
        {
            ProgramSettings pSettings = GetPackage(package);
            if (pSettings != null)
                return pSettings.Update(enablePackage, forceUpdate);
            return false;
        }

        public static bool IsComponentMissing()
        {
            ArrayList arrPath = new ArrayList();
            foreach (ProgramSettings pSettings in MainForm.Instance.ProgramSettings)
            {
                if (!pSettings.UpdateAllowed())
                    continue;
                arrPath.AddRange(pSettings.Files);
            }

            bool bComponentMissing = false;
            foreach (string strAppPath in arrPath)
            {
                ImageType image = ImageType.Error;
                if (MainForm.Instance.UpdateHandler.UpdateMode == UpdateMode.Automatic)
                    image = ImageType.Information;

                if (String.IsNullOrEmpty(strAppPath))
                {
                    MainForm.Instance.UpdateHandler.AddTextToLog("No path to check for missing components!", image, false);
                    bComponentMissing = true;
                    continue;
                }
                else if (File.Exists(strAppPath) == false)
                {
                    MainForm.Instance.UpdateHandler.AddTextToLog("Component not found: " + strAppPath, image, false);
                    bComponentMissing = true;
                    continue;
                }
                FileInfo fInfo = new FileInfo(strAppPath);
                if (fInfo.Length == 0)
                {
                    MainForm.Instance.UpdateHandler.AddTextToLog("Component has 0 bytes: " + strAppPath, image, false);
                    bComponentMissing = true;
                }
            }
            return bComponentMissing;
        }

        /// <summary>
        /// Checks the local update cache file
        /// </summary>
        /// <param name="file">full file name</param>
        /// <param name="err">reference to the error result</param>
        /// <returns>true if file is ok, false if not</returns>
        public static bool VerifyLocalCacheFile(string file, ref UpdateWindow.ErrorState err)
        {
            if (err == UpdateWindow.ErrorState.Successful)
                err = UpdateWindow.ErrorState.CouldNotDownloadFile;

            if (!File.Exists(file))
            {
                if (!File.Exists(Path.Combine(Path.GetDirectoryName(file), "_obsolete_" + Path.GetFileName(file))))
                    return false;
                File.Move(Path.Combine(Path.GetDirectoryName(file), "_obsolete_" + Path.GetFileName(file)), file);
            }

            FileInfo finfo = new FileInfo(file);
            if (finfo.Length == 0)
            {
                DeleteCacheFile(file);
                return false;
            }

            if (file.ToLowerInvariant().EndsWith(".7z"))
            {
                // check the 7-zip file
                err = UpdateWindow.ErrorState.CouldNotExtract;
                bool bOK = true;
                try
                {
                    using (SevenZipExtractor oArchive = new SevenZipExtractor(file))
                    {
                        if (oArchive.Check() == false)
                            bOK = false;
                    }
                }
                catch { bOK = false; }
                if (!bOK)
                {
                    MainForm.Instance.UpdateHandler.AddTextToLog("Could not extract " + file + ". Deleting file.", ImageType.Error, true);
                    DeleteCacheFile(file);
                    return false;
                }
            }
            else if (file.ToLowerInvariant().EndsWith(".zip"))
            {
                // check the zip file
                err = UpdateWindow.ErrorState.CouldNotExtract;
                bool bOK = true;
                try
                {
                    using (ZipFile zipFile = new ZipFile(file))
                    {
                        if (zipFile.TestArchive(true) == false)
                            bOK = false;
                    }
                }
                catch { bOK = false; }
                if (!bOK)
                {
                    MainForm.Instance.UpdateHandler.AddTextToLog("Could not extract " + file + ". Deleting file.", ImageType.Error, true);
                    DeleteCacheFile(file);
                    return false;
                }
            }
            else if (file.ToLowerInvariant().EndsWith(".xml"))
            {
                // check the xml file
                err = UpdateWindow.ErrorState.InvalidXML;
                System.Xml.XmlDocument upgradeXml = new System.Xml.XmlDocument();
                try
                {
                    upgradeXml.Load(file);
                }
                catch
                {
                    DeleteCacheFile(file);
                    return false;
                }
            }

            err = UpdateWindow.ErrorState.Successful;
            return true;
        }
    }
}