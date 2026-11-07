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
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

using ICSharpCode.SharpZipLib.Zip;
using Microsoft.Win32;

namespace MeGUI.core.util
{
    delegate bool FileExists(string filename);

    class FileUtil
    {
        public static DirectoryInfo CreateTempDirectory()
        {
            while (true)
            {
                string file = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), Path.GetRandomFileName());

                if (!File.Exists(file) && !Directory.Exists(file))
                {
                    MainForm.Instance.DeleteOnClosing(file);
                    return Directory.CreateDirectory(file);
                }
            }
        }

        public static bool DeleteFile(string strFile, LogItem oLog)
        {
            if (!File.Exists(strFile))
                return true;

            int iCounter = 0;
            string strError = String.Empty;
            while (iCounter++ < 10)
            {
                try
                {
                    File.Delete(strFile);
                    break;
                }
                catch (IOException ex)
                {
                    strError = ex.Message;
                    MeGUI.core.util.Util.Wait(1000);
                }
                catch (Exception ex)
                {
                    strError = ex.Message;
                    break;
                }
            }
            if (oLog != null && !String.IsNullOrEmpty(strError))
                oLog.Error("Error deleting file " + strFile + ": " + strError);
            return (!File.Exists(strFile));
        }

        public static bool CreateZipFile(string path, string filename)
        {
            if (!DeleteFile(filename, null))
                return false;

            using (ZipOutputStream outputFile = new ZipOutputStream(File.OpenWrite(filename)))
            {
                foreach (string file in FileUtil.AllFiles(path))
                {
                    ZipEntry newEntry = new ZipEntry(file.Substring(path.Length).TrimStart('\\', '/'));
                    outputFile.PutNextEntry(newEntry);
                    FileStream input = File.OpenRead(file);
                    FileUtil.copyData(input, outputFile);
                    input.Close();
                }
            }

            return true;
        }

        public static void ExtractZipFile(Stream s, string extractFolder)
        {
            using (ZipFile inputFile = new ZipFile(s))
            {
                foreach (ZipEntry entry in inputFile)
                {
                    string pathname = Path.Combine(extractFolder, entry.Name);
                    if (entry.IsDirectory)
                    {
                        Directory.CreateDirectory(pathname);
                    }
                    else // entry.isFile
                    {
                        System.Diagnostics.Debug.Assert(entry.IsFile);
                        FileUtil.ensureDirectoryExists(Path.GetDirectoryName(pathname));
                        Stream outputStream = File.OpenWrite(pathname);
                        FileUtil.copyData(inputFile.GetInputStream(entry), outputStream);
                        outputStream.Close();
                        File.SetLastWriteTime(pathname, entry.DateTime);
                    }
                }
            }
        }

        public static void DeleteDirectoryIfExists(string p, bool recursive)
        {
            try
            {
                if (Directory.Exists(p))
                    Directory.Delete(p, recursive);
            }
            catch { }
        }

        public static DirectoryInfo ensureDirectoryExists(string p)
        {
            if (Directory.Exists(p))
                return new DirectoryInfo(p);
            if (string.IsNullOrEmpty(p))
                throw new IOException("Can't create directory");
            ensureDirectoryExists(GetDirectoryName(p));
            MeGUI.core.util.Util.Wait(100);
            return Directory.CreateDirectory(p);
        }

        public static string GetDirectoryName(string file)
        {
            string path = string.Empty;
            try
            {
                path = Path.GetDirectoryName(file);
            }
            catch { }
            return path;
        }

        /// <summary>
        /// Generates a unique filename by adding numbers to the filename.
        /// </summary>
        /// <param name="original"></param>
        /// <param name="fileExists"></param>
        /// <returns></returns>
        public static string getUniqueFilename(string original, FileExists fileExists)
        {
            if (!fileExists(original)) return original;
            string prefix = Path.Combine(Path.GetDirectoryName(original),
                Path.GetFileNameWithoutExtension(original)) + "_";
            string suffix = Path.GetExtension(original);
            for (int i = 0; true; i++)
            {
                string filename = prefix + i + suffix;
                if (!fileExists(filename)) return filename;
            }
        }

        public static List<string> AllFiles(string folder)
        {
            List<string> list = new List<string>();
            AddFiles(folder, list);
            return list;
        }

        private static void AddFiles(string folder, List<string> list)
        {
            list.AddRange(Directory.GetFiles(folder));
            foreach (string subFolder in Directory.GetDirectories(folder))
                AddFiles(subFolder, list);
        }

        private const int BUFFER_SIZE = 2 * 1024 * 1024; // 2 MBs
        public static void copyData(Stream input, Stream output)
        {
            int count = -1;
            byte[] data = new byte[BUFFER_SIZE];
            while ((count = input.Read(data, 0, BUFFER_SIZE)) > 0)
            {
                output.Write(data, 0, count);
            }
        }

        /// <summary>
        /// Returns the full path and filename, but without the extension
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetPathWithoutExtension(string path)
        {
            return Path.Combine(Path.GetDirectoryName(path), Path.GetFileNameWithoutExtension(path));
        }

        /// <summary>
        /// Returns TimeSpan value formatted
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string ToShortString(TimeSpan ts)
        {
            string time;
            time = ts.Hours.ToString("00");
            time = time + ":" + ts.Minutes.ToString("00");
            time = time + ":" + ts.Seconds.ToString("00");
            time = time + "." + ts.Milliseconds.ToString("000");
            return time;
        }

        /// <summary>
        /// Adds extra to the filename, modifying the filename but keeping the extension and folder the same.
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="extra"></param>
        /// <returns></returns>
        public static string AddToFileName(string filename, string extra)
        {
            return Path.Combine(
                Path.GetDirectoryName(filename),
                Path.GetFileNameWithoutExtension(filename) + extra + Path.GetExtension(filename));
        }

        /// <summary>
        /// Returns true if the filename matches the filter specified. The format
        /// of the filter is the same as that of a FileDialog.
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static bool MatchesFilter(string filter, string filename)
        {
            if (string.IsNullOrEmpty(filter))
                return true;

            bool bIsFolder = Directory.Exists(filename);

            filter = filter.ToLowerInvariant();
            filename = Path.GetFileName(filename).ToLowerInvariant();
            string[] filters = filter.Split('|');

            for (int i = 1; i < filters.Length; i += 2)
            {
                string[] iFilters = filters[i].Split(';');
                foreach (string f in iFilters)
                {
                    if (f.IndexOf('*') > -1)
                    {
                        if (!f.StartsWith("*."))
                            throw new Exception("Invalid filter format");

                        if (f == "*.*" && filename.IndexOf('.') > -1)
                            return true;

                        if (f == "*." && bIsFolder)
                            return true;

                        string extension = f.Substring(1);
                        if (filename.EndsWith(extension))
                            return true;
                    }
                    else if (f == filename)
                        return true;
                    else return false;

                }
            }

            return false;
        }

        /// <summary>
        /// Backup File
        /// </summary>
        /// <param name"sourcePath">Path of the Source file</param>
        /// <param name="overwrite"></param>
        public static void BackupFile(string sourcePath, bool overwrite)
        {
            try
            {
                if (File.Exists(sourcePath))
                {
                    String targetPath;
                    if (sourcePath.Contains(System.Windows.Forms.Application.StartupPath))
                        targetPath = sourcePath.Replace(System.Windows.Forms.Application.StartupPath, System.Windows.Forms.Application.StartupPath + @"\backup");
                    else
                        targetPath = System.Windows.Forms.Application.StartupPath + @"\backup\" + (new FileInfo(sourcePath)).Name;
                    if (File.Exists(targetPath))
                        File.Delete(targetPath);

                    FileUtil.ensureDirectoryExists(Path.GetDirectoryName(targetPath));

                    File.Move(sourcePath, targetPath);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while moving file: \n" + sourcePath + "\n" + ex.Message, "Error moving file", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Checks if a directory is writable
        /// </summary>
        /// <param name"strPath">path to check</param>
        public static bool IsDirWriteable(string strPath)
        {
            try
            {
                bool bDirectoryCreated = false;

                // does the root directory exists
                if (!Directory.Exists(strPath))
                {
                    Directory.CreateDirectory(strPath);
                    bDirectoryCreated = true;
                }

                string newFilePath = string.Empty;
                // combine the random file name with the path
                do
                    newFilePath = Path.Combine(strPath, Path.GetRandomFileName());
                while (File.Exists(newFilePath));

                // create & delete the file
                FileStream fs = File.Create(newFilePath);
                fs.Close();
                File.Delete(newFilePath);

                if (bDirectoryCreated)
                    Directory.Delete(strPath);

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the proper output path
        /// </summary>
        /// <param name"strInputFileOrFolder">input file or folder name</param>
        /// <returns>either the default output path or a path based on the input file/folder</returns>
        public static string GetOutputFolder(string strInputFileOrFolder)
        {
            string outputPath = MainForm.Instance.Settings.DefaultOutputDir;

            // checks if the default output dir does exist and is writable
            if (string.IsNullOrEmpty(outputPath) || !Directory.Exists(outputPath) || !IsDirWriteable(outputPath))
            {
                // default output directory does not exist, use the input folder instead
                if (Directory.Exists(strInputFileOrFolder))
                    outputPath = strInputFileOrFolder;
                else
                    outputPath = Path.GetDirectoryName(strInputFileOrFolder);
            }
                
            return outputPath;
        }

        /// <summary>
        /// Gets the file prefix based on the folder structure
        /// </summary>
        /// <param name"strInputFile">input file name</param>
        /// <returns>a file prefix if a DVD/Blu-ray structure is found or an emtpy string</returns>
        public static string GetOutputFilePrefix(string strInputFile)
        { 
            string outputFilePrefix = string.Empty;

            // checks if the input is a folder name (only files are supported)
            if (Directory.Exists(strInputFile))
                return outputFilePrefix;

            // checks if the extension is supported
            string strExtension = Path.GetExtension(strInputFile).ToUpperInvariant();
            if (!strExtension.Equals(".IFO") && !strExtension.Equals(".MPLS") 
                && !strExtension.Equals(".VOB") && !strExtension.Equals(".M2TS"))
                return outputFilePrefix;

            if (strExtension.Equals(".VOB") || strExtension.Equals(".IFO"))
            {
                // check for DVD structure
                string fileName = Path.GetFileNameWithoutExtension(strInputFile);
                if (!RegExMatch(fileName, @"\AVTS_\d{2}_\d{1}\z", true))
                    return outputFilePrefix;

                // checks for corresponding VOB/IFO file
                fileName = fileName.Substring(0, 7);
                if (strExtension.Equals(".VOB"))
                    fileName += "0.IFO";
                else
                    fileName += "1.VOB";

                // checks if corresponding VOB/IFO does exist
                if (!File.Exists(Path.Combine(Path.GetDirectoryName(strInputFile), fileName)))
                    return outputFilePrefix;
            }
            else
            {
                // check for Blu-ray structure
                if (RegExMatch(strInputFile, @"\\playlist\\\d{5}\.mpls\z", true))
                {
                    // mpls structure
                    // checks if corresponding M2TS structure exists
                    string checkFolder = Path.Combine(Path.GetDirectoryName(Path.GetDirectoryName(strInputFile)), "STREAM");
                    if (!Directory.Exists(checkFolder) || Directory.GetFiles(checkFolder, "*.m2ts").Length == 0)
                        return outputFilePrefix;
                }
                else if (RegExMatch(strInputFile, @"\\stream\\\d{5}\.m2ts\z", true))
                {
                    // m2ts structure
                    // checks if corresponding MPLS structure exists
                    string checkFolder = Path.Combine(Path.GetDirectoryName(Path.GetDirectoryName(strInputFile)), "PLAYLIST");
                    if (!Directory.Exists(checkFolder) || Directory.GetFiles(checkFolder, "*.mpls").Length == 0)
                        return outputFilePrefix;
                }
                else
                    return outputFilePrefix;
            }

            // get the folder name only
            string folderTemp = Path.GetDirectoryName(strInputFile);

            // skip the DVD/Blu-ray directories
            while (folderTemp.ToUpperInvariant().EndsWith("VIDEO_TS") ||
                    folderTemp.ToUpperInvariant().EndsWith("PLAYLIST") ||
                    folderTemp.ToUpperInvariant().EndsWith("STREAM") ||
                    folderTemp.ToUpperInvariant().EndsWith("BDMV"))
                folderTemp = Path.GetDirectoryName(folderTemp);

            if (Path.GetPathRoot(folderTemp).Equals(folderTemp))
            {
                // root directory; get the volume label
                DriveInfo di = new DriveInfo(folderTemp);
                outputFilePrefix = di.VolumeLabel;
            }
            else
            {
                // get the folder name
                outputFilePrefix = Path.GetFileName(folderTemp);
            }

            // remove any illegal characters from the prefix string
            if (string.Join(" ", outputFilePrefix.Split(Path.GetInvalidFileNameChars())).Trim().Length == 0)
                return string.Empty;
            else
                return string.Join("_", outputFilePrefix.Split(Path.GetInvalidFileNameChars())) + "_";
        }

        /// <summary>
        /// Gets the file prefix based on the folder structure
        /// </summary>
        /// <param name"text">the text to search in</param>
        /// <param name"pattern">RegEx search pattern</param>
        /// <param name"bIgnoreCase">if the search should be not case sensitive</param>
        /// <returns>true if the pattern does match the text</returns>
        public static bool RegExMatch(string text, string pattern, bool bIgnoreCase)
        {
            // https://msdn.microsoft.com/en-us/library/az24scfc(v=vs.110).aspx
            Regex regex = new Regex(pattern);
            if (bIgnoreCase)
                regex = new Regex(pattern, RegexOptions.IgnoreCase);
            return regex.IsMatch(text);
        }

        /// <summary>
        /// Gets the Blu-ray source path (if possible)
        /// </summary>
        /// <param name"strInputFileOrFolder">the source file or folder name</param>
        /// <returns>empty if the source is not a Blu-ray structure, otherwise a path pointing to the PLAYLIST dir</returns>
        public static string GetBlurayPath(string strInputFileOrFolder)
        {
            string path = strInputFileOrFolder;
            if (!Directory.Exists(path))
            {
                if (File.Exists(path))
                    path = Path.GetDirectoryName(path);
                else
                    return string.Empty;
            }

            while (!path.Equals(Path.GetPathRoot(path)))
            {
                if (Directory.Exists(Path.Combine(Path.Combine(path, "BDMV"), "PLAYLIST")))
                    break;

                path = Path.GetDirectoryName(path);
            }

            path = Path.Combine(Path.Combine(path, "BDMV"), "PLAYLIST");
            if (!Directory.Exists(path) || Directory.GetFiles(path, "*.mpls").Length == 0)
                return string.Empty;

            return path;
        }

        /// <summary>
        /// Gets the DVD source path (if possible)
        /// </summary>
        /// <param name"strInputFileOrFolder">the source file or folder name</param>
        /// <returns>empty if the source is not a DVD structure, otherwise the full path of an IFO file</returns>
        public static string GetDVDPath(string strInputFileOrFolder)
        {
            if (File.Exists(strInputFileOrFolder))
            {
                if (FileUtil.RegExMatch(strInputFileOrFolder, @"\\VTS_\d{2}_\d{1}\.VOB\z", true))
                {
                    // input file is a proper VOB file e.g. VTS_01_1.VOB
                    string temp = Path.Combine(Path.GetDirectoryName(strInputFileOrFolder), Path.GetFileNameWithoutExtension(strInputFileOrFolder).Substring(0, 7) + "0.IFO");
                    if (File.Exists(temp))
                        strInputFileOrFolder = temp;
                }

                if (Path.GetFileName(strInputFileOrFolder).ToUpperInvariant().Equals("VIDEO_TS.IFO") 
                    || FileUtil.RegExMatch(strInputFileOrFolder, @"\\VTS_\d{2}_0\.IFO\z", true))
                {
                    // input file is a proper IFO file e.g. VTS_01_0.IFO or VIDEO_TS.IFO
                    return strInputFileOrFolder;
                }
                else
                    strInputFileOrFolder = Path.GetDirectoryName(strInputFileOrFolder);
            }

            if (Directory.Exists(strInputFileOrFolder)
                && Directory.GetFiles(strInputFileOrFolder, "VTS_*_0.IFO").Length > 0)
            {
                return Path.Combine(strInputFileOrFolder, "VIDEO_TS.IFO");
            }
            else if (Directory.Exists(Path.Combine(strInputFileOrFolder, "VIDEO_TS"))
                && Directory.GetFiles(Path.Combine(strInputFileOrFolder, "VIDEO_TS"), "VTS_*_0.IFO").Length > 0)
            {
                return Path.Combine(Path.Combine(strInputFileOrFolder, "VIDEO_TS"), "VIDEO_TS.IFO");
            }

            // No DVD IFO data found
            return string.Empty;
        }

        /// <summary>
        /// Attempts to delete all files and directories listed 
        /// in job.FilesToDelete if settings.DeleteIntermediateFiles is checked
        /// </summary>
        /// <param name="job">the job which should just have been completed</param>
        public static LogItem DeleteIntermediateFiles(List<string> files, bool bAlwaysAddLog, bool askAboutDelete)
        {
            bool bShowLog = false;
            LogItem i = new LogItem(string.Format("[{0:G}] {1}", DateTime.Now, "Deleting intermediate files"));

            List<string> arrFiles = new List<string>();
            foreach (string file in files)
            {
                if (Directory.Exists(file))
                    continue;
                else if (!File.Exists(file))
                    continue;
                if (!arrFiles.Contains(file))
                    arrFiles.Add(file);
            }

            if (arrFiles.Count > 0)
            {
                bShowLog = true;
                bool delete = true;

                if (askAboutDelete)
                    delete = MainForm.Instance.DialogManager.DeleteIntermediateFiles(arrFiles);
                if (!delete)
                    return null;

                // delete all files first
                foreach (string file in arrFiles)
                {
                    int iCounter = 0;
                    while (File.Exists(file))
                    {
                        try
                        {
                            File.Delete(file);
                            i.LogEvent("Successfully deleted " + file);
                        }
                        catch (Exception ex)
                        {
                            if (++iCounter >= 3)
                            {
                                i.LogValue("Problem deleting " + file, ex.Message, ImageType.Warning);
                                break;
                            }
                            else
                                MeGUI.core.util.Util.Wait(2000);
                        }
                    }
                }
            }

            // delete empty directories
            foreach (string file in files)
            {
                try
                {
                    if (Directory.Exists(file))
                    {
                        bShowLog = true;
                        if (Directory.GetFiles(file, "*.*", SearchOption.AllDirectories).Length == 0)
                        {
                            Directory.Delete(file, true);
                            i.LogEvent("Successfully deleted directory " + file);
                        }
                        else
                            i.LogEvent("Did not delete " + file + " as the directory is not empty.", ImageType.Warning);
                    }
                }
                catch (Exception ex)
                {
                    i.LogValue("Problem deleting directory " + file, ex.Message, ImageType.Warning);
                }
            }
            if (bAlwaysAddLog || bShowLog)
                return i;
            return null;
        }

        /// <summary>
        /// Detects the system installed AviSynth+ version/date and writes it into the log
        /// </summary>
        /// <param name="oLog">the version information will be added to the log if available</param>
        public static void CheckAviSynth(ref LogItem oLog)
        {
            LogItem oLogInstalled = new LogItem("AviSynth");

            // remove the files of the former portable AviSynth build. They must not stay next to
            // MeGUI/the encoders, otherwise the stale copy would be loaded instead of the installed AviSynth+
            RemoveLegacyPortableAviSynthFiles();

            // remove the runtime/redist files copied by previous versions: the Microsoft Visual C++
            // runtime now has to be installed on the system, a stale copy must not shadow it
            RemoveLegacyRuntimeFiles();

            MainForm.Instance.Settings.AviSynthPlus = false;

            int iResult = AviSynthClip.CheckAvisynthInstallation(out string strVersion, out bool bIsAVS26, out bool bIsAVSPlus, out bool bIsMT, out string strAviSynthDLL, ref oLogInstalled);

            bool bFound = false;
            if (!String.IsNullOrEmpty(strAviSynthDLL) && GetFileInformation(strAviSynthDLL, out string fileVersion, out string fileDate, out string fileProductName))
            {
                // file information
                oLogInstalled.LogValue("File Version", !String.IsNullOrEmpty(fileVersion) ? fileVersion : "n/a", false);
                oLogInstalled.LogValue("File Date", !String.IsNullOrEmpty(fileDate) ? fileDate : "n/a", false);
                oLogInstalled.LogValue("File Name", !String.IsNullOrEmpty(fileProductName) ? fileProductName : "n/a", false);
                oLogInstalled.LogValue("File Path", strAviSynthDLL, false);

                if (iResult != 0)
                {
                    if (iResult == 3)
                        oLogInstalled.LogValue("AviSynth Status", "installed AviSynth build is out of date", ImageType.Error, false);
                    else
                        oLogInstalled.LogValue("AviSynth Status", "installed AviSynth build cannot be used: " + iResult, ImageType.Error, false);
                }
                else
                {
                    oLogInstalled.LogValue("AviSynth Version", !String.IsNullOrEmpty(strVersion) ? strVersion : "n/a", false);
                    oLogInstalled.LogValue("AviSynth+", bIsAVSPlus ? "true" : "false", false);
                    oLogInstalled.LogValue("AviSynth MT", bIsMT ? "true" : "false", false);

                    bFound = true;
                    if (bIsAVSPlus)
                        MainForm.Instance.Settings.AviSynthPlus = true;
                }
            }
            else
                oLogInstalled.LogValue("AviSynth Status", "not installed. Please install AviSynth+ (https://github.com/AviSynth/AviSynthPlus/releases)", ImageType.Error, false);

            oLog.Add(oLogInstalled);

            if (!bFound)
                oLog.LogValue("AviSynth", "not found", ImageType.Error, false);
        }

        /// <summary>
        /// Detects the file version/date and writes it into the log
        /// </summary>
        /// <param name="strName">the name in the log</param>
        /// <param name="strFile">the file to check</param>
        /// <param name="oLog">the LogItem where the information should be added</param>
        public static void GetFileInformation(string strName, string strFile, ref LogItem oLog)
        {
            string fileVersion = string.Empty;
            string fileDate = string.Empty;
            string fileProductName = string.Empty;

            if (GetFileInformation(strFile, out fileVersion, out fileDate, out fileProductName))
            {
                if (String.IsNullOrEmpty(fileVersion))
                    oLog.LogValue(strName, " (" + fileDate + ")", false);
                else
                    oLog.LogValue(strName, fileVersion + " (" + fileDate + ")", false);
            }
            else
            {
                if (strName.Contains("Haali"))
                    oLog.LogValue(strName, "not installed", ImageType.Information, false);
                else
                    oLog.LogValue(strName, "not installed", ImageType.Error, false);
            }
        }

        /// <summary>
        /// Gets the file version/date
        /// </summary>
        /// <param name="fileName">the file to check</param>
        /// <param name="fileVersion">the file version</param>
        /// <param name="fileDate">the file date</param>
        /// <param name="fileProductName">the file product name</param>
        /// <returns>true if file can be found, false if file cannot be found</returns>
        private static bool GetFileInformation(string fileName, out string fileVersion, out string fileDate, out string fileProductName)
        {
            fileVersion = fileDate = fileProductName = string.Empty;
            if (!File.Exists(fileName))
                return false;

            FileVersionInfo FileProperties = FileVersionInfo.GetVersionInfo(fileName);
            fileVersion = FileProperties.FileVersion;
            if (!String.IsNullOrEmpty(fileVersion))
                fileVersion = fileVersion.Replace(", ", ".");
            fileDate = File.GetLastWriteTimeUtc(fileName).ToString("dd-MM-yyyy");
            fileProductName = FileProperties.ProductName;
            return true;
        }

        #region redist actions
        /// <summary>
        /// Gets the installed Microsoft Visual C++ v14 Redistributable (2015-2022 and later) from the registry
        /// </summary>
        /// <returns></returns>
        public static LogItem GetRedistInformation()
        {
            LogItem oLog = new LogItem("Redistributables");

            // the x64 runtime is needed on a 64-bit OS, the x86 one otherwise
            string strRequired = Environment.Is64BitOperatingSystem ? "x64" : "x86";
            bool bRequiredFound = false;

            foreach (string architecture in new string[] { "x64", "x86" })
            {
                string strVersion = GetVCRuntimeVersion(architecture);
                if (String.IsNullOrEmpty(strVersion))
                    continue;

                oLog.LogValue("Microsoft Visual C++ v14 Redistributable " + architecture, strVersion, false);
                if (architecture.Equals(strRequired))
                    bRequiredFound = true;
            }

            if (!bRequiredFound)
                oLog.LogValue("Microsoft Visual C++ v14 Redistributable " + strRequired, "not detected. Please install the latest Visual C++ Redistributable (https://learn.microsoft.com/cpp/windows/latest-supported-vc-redist)", ImageType.Warning, false);

            return oLog;
        }

        /// <summary>
        /// Reads the version of the installed Visual C++ v14 runtime from
        /// HKLM\SOFTWARE\Microsoft\VisualStudio\14.0\VC\Runtimes\{architecture}, which is the detection method documented by
        /// Microsoft. The key name stays "14.0" for all v14 releases (2015, 2017, 2019, 2022, 14.50 and later).
        /// </summary>
        /// <param name="architecture">x64 or x86</param>
        /// <returns>the version (e.g. 14.51.36247) or an empty string if the runtime is not installed</returns>
        private static string GetVCRuntimeVersion(string architecture)
        {
            // depending on the architecture/OS the key is either in the native or in the Wow6432Node registry view
            foreach (RegistryView view in new RegistryView[] { RegistryView.Registry64, RegistryView.Registry32 })
            {
                try
                {
                    using (RegistryKey oBase = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, view))
                    using (RegistryKey oKey = oBase.OpenSubKey(@"SOFTWARE\Microsoft\VisualStudio\14.0\VC\Runtimes\" + architecture))
                    {
                        if (oKey == null || Convert.ToInt32(oKey.GetValue("Installed", 0)) != 1)
                            continue;

                        // e.g. "v14.51.36247"
                        string strVersion = Convert.ToString(oKey.GetValue("Version", string.Empty)).Trim().TrimStart('v', 'V');
                        if (String.IsNullOrEmpty(strVersion) && oKey.GetValue("Major") != null)
                            strVersion = string.Format("{0}.{1}.{2}", oKey.GetValue("Major"), oKey.GetValue("Minor"), oKey.GetValue("Bld"));

                        return String.IsNullOrEmpty(strVersion) ? "n/a" : strVersion;
                    }
                }
                catch { }
            }

            return string.Empty;
        }

        /// <summary>
        /// Removes the runtime/redist DLLs which previous MeGUI versions copied next to the tools (they were
        /// normally removed on exit, so they can only be left over after a crash). The list of files is taken
        /// from the obsolete tools\redist package folder if it still exists. The folder itself is not touched.
        /// </summary>
        public static void RemoveLegacyRuntimeFiles()
        {
            string strRedistPath = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"tools\redist");
            if (!Directory.Exists(strRedistPath))
                return;

            List<string> targetDirectories = new List<string>();
            targetDirectories.Add(Path.GetDirectoryName(Application.ExecutablePath));
            targetDirectories.Add(Path.GetDirectoryName(MainForm.Instance.Settings.FFmpeg.Path));
            targetDirectories.Add(Path.GetDirectoryName(MainForm.Instance.Settings.SvtAv1.Path));
            targetDirectories.Add(Path.GetDirectoryName(MainForm.Instance.Settings.X264.Path));
            targetDirectories.Add(Path.GetDirectoryName(MainForm.Instance.Settings.X265.Path));
            targetDirectories.Add(Path.GetDirectoryName(MainForm.Instance.Settings.XviD.Path));
            targetDirectories.Add(Path.GetDirectoryName(MainForm.Instance.Settings.AviSynthPlugins.Path));
            targetDirectories.Add(Path.GetDirectoryName(MainForm.Instance.Settings.LSMASH.Path));
            targetDirectories.Add(Path.GetDirectoryName(MainForm.Instance.Settings.BestSource.Path));

            List<string> sourceFiles = new List<string>();
            foreach (FileInfo f in new DirectoryInfo(strRedistPath).GetFiles("*.dll", SearchOption.AllDirectories))
                sourceFiles.Add(f.Name);

            foreach (string dir in targetDirectories)
            {
                if (!Directory.Exists(dir))
                    continue;

                foreach (string file in sourceFiles)
                    DeleteFile(Path.Combine(dir, file), null);
            }
        }
        #endregion

        /// <summary>
        /// Removes the files of the former portable AviSynth build (AviSynth.dll/DevIL.dll), which older
        /// MeGUI versions copied next to the executable and the encoders. Only the files in the MeGUI/tool
        /// folders are touched, the system installed AviSynth+ is never modified.
        /// </summary>
        public static void RemoveLegacyPortableAviSynthFiles()
        {
            ArrayList targetDirectories = new ArrayList();
            targetDirectories.Add(Path.GetDirectoryName(Application.ExecutablePath));
            targetDirectories.Add(Path.GetDirectoryName(MainForm.Instance.Settings.FFmpeg.Path));
            targetDirectories.Add(Path.GetDirectoryName(MainForm.Instance.Settings.SvtAv1.Path));
            targetDirectories.Add(Path.GetDirectoryName(MainForm.Instance.Settings.X264.Path));
            targetDirectories.Add(Path.GetDirectoryName(MainForm.Instance.Settings.X265.Path));
            targetDirectories.Add(Path.GetDirectoryName(MainForm.Instance.Settings.XviD.Path));

            string[] legacyFiles = { "AviSynth.dll", "DevIL.dll" };

            foreach (string dir in targetDirectories)
            {
                if (!Directory.Exists(dir))
                    continue;

                foreach (string file in legacyFiles)
                {
                    string strFile = Path.Combine(dir, file);
                    if (!File.Exists(strFile))
                        continue;

                    try { File.Delete(strFile); }
                    catch { }
                }
            }
        }

        /// <summary>
        /// Creates UTF-8 formatted text file containing the track name of a file.
        /// Allows mp4box to use int'l chars in track names, as well as control chars 
        /// such as colons, equals signs, quotation marks, &c.
        /// </summary>
        public static string CreateUTF8TracknameFile(string trackName, string fileName, int trackNumber)
        {
            string tracknameFilePath = Path.GetFullPath(fileName) + "_TRACKNAME" + trackNumber.ToString() + ".txt";
            try 
            {
                using (StreamWriter sw = new StreamWriter(tracknameFilePath, false, Encoding.UTF8))
                {
                    sw.Write(trackName);
                    sw.Close();
                }
            }
            catch (Exception e)
            {
                e.Message.ToString();
            }
            return tracknameFilePath;
        }

        private static object _locker = new object();
        public static void WriteToFile(string fileName, string text, bool append, bool bRaiseError)
        {
            try
            {
                lock (_locker)
                {
                    if (append)
                        System.IO.File.AppendAllText(fileName, text);
                    else
                        System.IO.File.WriteAllText(fileName, text);
                }
            }
            catch (Exception e)
            {
                if (bRaiseError)
                    MessageBox.Show(e.Message, "Error writing file", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}