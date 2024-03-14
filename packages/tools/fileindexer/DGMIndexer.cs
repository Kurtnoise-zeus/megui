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
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

using MeGUI.core.util;

namespace MeGUI
{
    public class DGMIndexer : CommandlineJobProcessor<DGMIndexJob>
    {
        public static readonly JobProcessorFactory Factory = new JobProcessorFactory(new ProcessorFactory(init), "DGMIndexer");

        private static IJobProcessor init(MainForm mf, Job j)
        {
            if (j is DGMIndexJob)
                return new DGMIndexer(mf.Settings.DGIndexIM.Path);
            return null;
        }

        public DGMIndexer(string executableName)
        {
            UpdateCacher.CheckPackage("dgindexim");
            executable = executableName;
        }

        public override void ProcessLine(string line, StreamType stream, ImageType oType)
        {
            if (Regex.IsMatch(line, "^[0-9]{1,3}$", RegexOptions.Compiled))
            {
                su.PercentageDoneExact = Int32.Parse(line);
                return;
            }

            if (line.Contains("Project"))
                su.Status = "Creating DGI...";
            else
                su.Status = "Creating " + line;
            base.su.ResetTime();
            base.ProcessLine(line, stream, oType);
        }

        /// <summary>
        /// If necessary the dgindexim.ini will be created or changed
        /// </summary>
        private void CheckINI()
        {
            try
            {
                string strFileName = "DGIndexIM.ini";
                bool bChanged = false;
                bool bResponseOnAudioMismatchFound = false;
                bool bEnable_Info_Log = false;
                string strINIFile = Path.Combine(Path.GetDirectoryName(MainForm.Instance.Settings.DGIndexIM.Path), strFileName);
                StringBuilder sb = new StringBuilder();
                if (File.Exists(strINIFile))
                {
                    // Read the file
                    string line;
                    using (StreamReader file = new StreamReader(strINIFile, Encoding.Default))
                    {
                        while ((line = file.ReadLine()) != null)
                        {
                            if (line.StartsWith("ResponseOnAudioMismatch", StringComparison.InvariantCultureIgnoreCase))
                            {
                                bResponseOnAudioMismatchFound = true;
                                if (!line.Equals("ResponseOnAudioMismatch=1", StringComparison.InvariantCultureIgnoreCase))
                                {
                                    sb.AppendLine("ResponseOnAudioMismatch=1");
                                    log.LogEvent("ResponseOnAudioMismatch=1 written to " + strFileName, ImageType.Information);
                                    bChanged = true;
                                }
                                continue;
                            }

                            if (line.StartsWith("Enable_Info_Log", StringComparison.InvariantCultureIgnoreCase))
                            {
                                bEnable_Info_Log = true;
                                if (!line.Equals("Enable_Info_Log=1", StringComparison.InvariantCultureIgnoreCase))
                                {
                                    sb.AppendLine("Enable_Info_Log=1");
                                    log.LogEvent("Enable_Info_Log=1 written to " + strFileName, ImageType.Information);
                                    bChanged = true;
                                }
                                continue;
                            }

                            if (String.IsNullOrEmpty(line))
                                continue;

                            sb.AppendLine(line);
                        }
                    }
                }

                if (!File.Exists(strINIFile) || !bResponseOnAudioMismatchFound || !bEnable_Info_Log)
                    ResetINI(strINIFile);
                else if (bChanged)
                    File.WriteAllText(strINIFile, sb.ToString(), Encoding.Default);
            }
            catch (Exception) { }
        }

        private void ResetINI(string strINIFile)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Version=");
            sb.AppendLine("Window_Position=0,0");
            sb.AppendLine("Info_Window_Position=0,0");
            sb.AppendLine("Process_Priority=2");
            sb.AppendLine("Playback_Speed=3");
            sb.AppendLine("AVS_Template_Folder=");
            sb.AppendLine("AVS_Template_File=template.avs");
            sb.AppendLine("AVS_Enable_Template=1");
            sb.AppendLine("AVS_Overwrite=0");
            sb.AppendLine("Full_Path_In_Files=1");
            sb.AppendLine("MRUList[0]=");
            sb.AppendLine("MRUList[1]=");
            sb.AppendLine("MRUList[2]=");
            sb.AppendLine("MRUList[3]=");
            sb.AppendLine("Enable_Info_Log=1");
            sb.AppendLine("Loop_Playback=0");
            sb.AppendLine("AVC_Extension=264");
            sb.AppendLine("MPG_Extension=m2v");
            sb.AppendLine("VC1_Extension=vc1");
            sb.AppendLine("Deinterlace=0");
            sb.AppendLine("UsePF=0");
            sb.AppendLine("AlwaysCrop=1");
            sb.AppendLine("UseD3D=0");
            sb.AppendLine("Snapped=0");
            sb.AppendLine("ResponseOnAudioMismatch=1");
            sb.AppendLine("Enable_Audio_Demux=1");
            sb.AppendLine("CUDA_Device=255");
            sb.AppendLine("Decode_Modes=0,1,0");
            sb.AppendLine("Full_Info=1");
            sb.AppendLine("Bare_Demux=0");

            File.WriteAllText(strINIFile, sb.ToString(), Encoding.Default);

            log.LogEvent("Reset " + Path.GetFileName(strINIFile), ImageType.Information);
        }

        protected override void checkJobIO()
        {
            try
            {
                if (!String.IsNullOrEmpty(job.Output))
                    FileUtil.ensureDirectoryExists(Path.GetDirectoryName(job.Output));
                CheckINI();
            }
            finally
            {
                base.checkJobIO();
            }
            su.Status = "Creating DGI...";
        }

        protected override string Commandline
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("-i \"" + job.Input + "\"");
                if (MainForm.Instance.Settings.AutoLoadDG && Path.GetExtension(job.Input).ToLowerInvariant().Equals(".vob"))
                {
                    string strFile = Path.GetFileNameWithoutExtension(job.Input);
                    int iNumber = 0;
                    if (int.TryParse(strFile.Substring(strFile.Length - 1), out iNumber))
                    {
                        while (++iNumber < 10)
                        {
                            string strNewFile = "";
                            strNewFile = Path.Combine(Path.GetDirectoryName(job.Input), strFile.Substring(0, strFile.Length - 1) + iNumber.ToString() + ".vob");
                            if (File.Exists(strNewFile))
                                sb.Append(",\"" + strNewFile + "\"");
                            else
                                break;
                        }
                    }
                }
                if (job.DemuxVideo)
                    sb.Append(" -od \"" + job.Output + "\" -h");
                else
                    sb.Append(" -o \"" + job.Output + "\" -h");
                if (job.DemuxMode > 0)
                    sb.Append(" -a"); // demux everything
                if (Path.GetExtension(job.Input).ToLowerInvariant().Equals(".mpls"))
                    sb.Append(" -ang 0");
                return sb.ToString();
            }
        }

        public static bool DGIHasFullPath(string dgiFile)
        {
            using (StreamReader sr = new StreamReader(dgiFile, Encoding.Default))
            {
                string line = null;
                int iLineCount = 0;
                while ((line = sr.ReadLine()) != null)
                {
                    iLineCount++;
                    if (iLineCount == 4)
                    {
                        string strSourceFile = line.Substring(0, line.LastIndexOf(" "));
                        return File.Exists(strSourceFile);
                    }
                }
            }
            return false;
        }

        private void AddFullPathToDGI()
        {
            int iLineCount = 0;
            string line = null;
            bool bPathExtended = false;
            using (StreamReader reader = new StreamReader(job.Output, Encoding.Default))
            {
                using (StreamWriter writer = new StreamWriter(job.Output + ".temp", false, Encoding.Default))
                {
                    while ((line = reader.ReadLine()) != null)
                    {
                        iLineCount++;
                        if (!bPathExtended && iLineCount >= 4)
                        {
                            if (String.IsNullOrEmpty(line))
                            {
                                bPathExtended = true;
                                writer.WriteLine(line);
                            }
                            else
                            {
                                string strSize = line.Substring(line.LastIndexOf(" "));
                                string strSourceFile = line.Substring(0, line.LastIndexOf(" "));
                                string strFullPath = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(job.Input), strSourceFile));
                                writer.WriteLine(strFullPath + strSize);
                            }
                        }
                        else
                        {
                            writer.WriteLine(line);
                        }
                    }
                }
            }
            FileUtil.DeleteFile(job.Output, log);
            File.Move(job.Output + ".temp", job.Output);
            log.LogEvent("Corrected missing Full_Path_In_Files=1", ImageType.Information);
        }

        protected override void doExitConfig()
        {
            if (!File.Exists(job.Output))
            {
                su.HasError = true;
            }
            else
            {
                if (!DGIHasFullPath(job.Output))
                    AddFullPathToDGI();
            }

            base.doExitConfig();
        }
    }
}