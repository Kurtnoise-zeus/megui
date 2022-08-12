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
using System.Xml.Serialization;

using MeGUI.core.plugins.interfaces;
using MeGUI.core.util;

namespace MeGUI
{
    public enum AudioEncodingMode
    {
        [EnumTitle("always")]
        Always,
        [EnumTitle("if codec does not match")]
        IfCodecDoesNotMatch,
        [EnumTitle("never, but use only the core of HD tracks")]
        NeverOnlyCore,
        [EnumTitle("never")]
        Never
    };

	/// <summary>
	/// Summary description for OneClickDefaults.
	/// </summary>
    public class OneClickSettings : GenericSettings
	{
        public static readonly AudioEncodingMode[] SupportedModes = new AudioEncodingMode[] { AudioEncodingMode.Always, AudioEncodingMode.IfCodecDoesNotMatch, AudioEncodingMode.NeverOnlyCore, AudioEncodingMode.Never };

        private string videoProfileName;
        public string VideoProfileName
        {
            get { return videoProfileName; }
            set { videoProfileName = value; }
        }

        private string avsProfileName;
        public string AvsProfileName
        {
            get { return avsProfileName; }
            set { avsProfileName = value; }
        }

        private bool prerenderVideo;
        public bool PrerenderVideo
        {
            get { return prerenderVideo; }
            set { prerenderVideo = value; }
        }

        private string audioProfileName;
        public string AudioProfileName
        {
            get { return "migrated"; }
            set { audioProfileName = value; }
        }

        // for profile import/export in case the enum changes
        public string AudioEncodingModeString
        {
            get { return "migrated"; }
            set
            {
                if (value.Equals("migrated"))
                    return;

                AudioEncodingMode audioEncodingMode = AudioEncodingMode.Always;
                if (value.Equals("Never"))
                    audioEncodingMode = AudioEncodingMode.Never;
                else if (value.Equals("IfCodecDoesNotMatch"))
                    audioEncodingMode = AudioEncodingMode.IfCodecDoesNotMatch;
                else if (value.Equals("NeverOnlyCore"))
                    audioEncodingMode = AudioEncodingMode.NeverOnlyCore;

                audioSettings = new List<OneClickAudioSettings>();
                audioSettings.Add(new OneClickAudioSettings("[default]", audioProfileName, audioEncodingMode, false));
            }
        }

        private List<OneClickAudioSettings> audioSettings;
        [XmlIgnore()]
        [PropertyEqualityIgnoreAttribute()]
        public List<OneClickAudioSettings> AudioSettings
        {
            get { return audioSettings; }
            set { audioSettings = value; }
        }

        public OneClickAudioSettings[] AudioSettingsString
        {
            get { return audioSettings.ToArray(); }
            set { audioSettings = new List<OneClickAudioSettings>(value); }
        }

        private bool dontEncodeVideo;
        public bool DontEncodeVideo
        {
            get { return dontEncodeVideo; }
            set { dontEncodeVideo = value; }
        }

        private bool automaticDeinterlacing;
        public bool AutomaticDeinterlacing
        {
            get { return automaticDeinterlacing; }
            set { automaticDeinterlacing = value; }
        }

        /// <summary>
        /// gets / sets the aspect ratio of the video input (if known)
        /// </summary>
        private Dar? ar;
        public Dar? DAR
        {
            get { return ar; }
            set { ar = value; }
        }

        private bool autoCrop;
        public bool AutoCrop
        {
            get { return autoCrop; }
            set { autoCrop = value; }
        }

        private bool keepInputResolution;
        public bool KeepInputResolution
        {
            get { return keepInputResolution; }
            set { keepInputResolution = value; }
        }

        private long outputResolution;
        public long OutputResolution
        {
            get { return outputResolution; }
            set { outputResolution = value; }
        }

        private FileSize? filesize;
        public FileSize? Filesize
        {
            get { return filesize; }
            set { filesize = value; }
        }

        private FileSize? splitSize;
        public FileSize? SplitSize
        {
            get { return splitSize; }
            set { splitSize = value; }
        }

        private string[] containerCandidates;
        public string[] ContainerCandidates
        {
            get { return containerCandidates; }
            set { containerCandidates = value; }
        }

        private string deviceType;
        public string DeviceOutputType
        {
            get { return deviceType; }
            set { deviceType = value; }
        }

        private string leadingName;
        public string LeadingName
        {
            get { return leadingName; }
            set { leadingName = value; }
        }

        private string suffixName;
        public string SuffixName
        {
            get { return suffixName; }
            set { suffixName = value; }
        }

        private string workingNameReplace;
        public string WorkingNameReplace
        {
            get { return workingNameReplace; }
            set { workingNameReplace = value; }
        }

        private string workingNameReplaceWith;
        public string WorkingNameReplaceWith
        {
            get { return workingNameReplaceWith; }
            set { workingNameReplaceWith = value; }
        }

        private bool useChaptersMarks;
        public bool UseChaptersMarks
        {
            get { return useChaptersMarks; }
            set { useChaptersMarks = value; }
        }

        private bool useNoLanguagesAsFallback;
        public bool UseNoLanguagesAsFallback
        {
            get { return useNoLanguagesAsFallback; }
            set { useNoLanguagesAsFallback = value; }
        }

        private bool disableIntermediateMKV;
        public bool DisableIntermediateMKV
        {
            get { return disableIntermediateMKV; }
            set { disableIntermediateMKV = value; }
        }

        private string defaultWorkingDirectory;
        public string DefaultWorkingDirectory
        {
            get { return defaultWorkingDirectory; }
            set { defaultWorkingDirectory = value; }
        }

        private string defaultOutputDirectory;
        public string DefaultOutputDirectory
        {
            get { return defaultOutputDirectory; }
            set { defaultOutputDirectory = value; }
        }

        private List<string> defaultAudioLanguage;
        [XmlIgnore()]
        [PropertyEqualityIgnoreAttribute()]
        public List<string> DefaultAudioLanguage
        {
            get { return defaultAudioLanguage; }
            set { defaultAudioLanguage = value; }
        }

        public string[] DefaultAudioLanguageString
        {
            get { return defaultAudioLanguage.ToArray(); }
            set { defaultAudioLanguage = new List<string>(value); }
        }

        private List<string> defaultSubtitleLanguage;
        [XmlIgnore()]
        [PropertyEqualityIgnoreAttribute()]
        public List<string> DefaultSubtitleLanguage
        {
            get { return defaultSubtitleLanguage; }
            set { defaultSubtitleLanguage = value; }
        }

        public string[] DefaultSubtitleLanguageString
        {
            get { return defaultSubtitleLanguage.ToArray(); }
            set { defaultSubtitleLanguage = new List<string>(value); }
        }

        private List<string> indexerPriority;
        [XmlIgnore()]
        [PropertyEqualityIgnoreAttribute()]
        public List<string> IndexerPriority
        {
            get { return indexerPriority; }
            set { indexerPriority = value; }
        }

        public string[] IndexerPriorityString
        {
            get { return indexerPriority.ToArray(); }
            set 
            {
                if (value.Length == 4)
                {
                    Array.Resize<string>(ref value, 5);
                    value[4] = FileIndexerWindow.IndexType.AVISOURCE.ToString();
                }
                else if (value.Length == 5)
                {
                    bool bFound = false;
                    Array.Resize<string>(ref value, 6);
                    for (int i = 4; i >= 0; i--)
                    {
                        if (value[i].Equals(FileIndexerWindow.IndexType.FFMS.ToString()))
                        {
                            value[i + 1] = FileIndexerWindow.IndexType.LSMASH.ToString();
                            bFound = true;
                        }
                        if (!bFound)
                            value[i + 1] = value[i];
                    }
                }
                for (int i = 0; i < value.Length; i++)
                {
                    if (value[i].ToUpper().Equals("DGA"))
                    {
                        value[0] = FileIndexerWindow.IndexType.DGI.ToString();
                        value[1] = FileIndexerWindow.IndexType.DGM.ToString();
                        value[2] = FileIndexerWindow.IndexType.D2V.ToString();
                        value[3] = FileIndexerWindow.IndexType.LSMASH.ToString();
                        value[4] = FileIndexerWindow.IndexType.FFMS.ToString();
                        value[5] = FileIndexerWindow.IndexType.AVISOURCE.ToString();
                        break;
                    }
                }
                indexerPriority = new List<string>(value);
            }
        }

		public OneClickSettings()
		{
			videoProfileName = "";
            avsProfileName = "";
            automaticDeinterlacing = true;
            prerenderVideo = false;
            dontEncodeVideo = false;
            disableIntermediateMKV = false;
            useChaptersMarks = true;
            autoCrop = true;
            keepInputResolution = false;
			outputResolution = 720;
            splitSize = null;
            containerCandidates = new string[] {"MKV"};
            defaultAudioLanguage = new List<string>();
            defaultSubtitleLanguage = new List<string>();
            indexerPriority = new List<string>();
            defaultWorkingDirectory = "";
            workingNameReplace = "";
            workingNameReplaceWith = "";
            leadingName = "";
            audioSettings = new List<OneClickAudioSettings>();
            audioSettings.Add(new OneClickAudioSettings("[default]", "FFmpeg AC-3: *scratchpad*", AudioEncodingMode.IfCodecDoesNotMatch, true));

            if (MainForm.Instance != null)
            {
                if (!String.IsNullOrEmpty(MainForm.Instance.Settings.DefaultLanguage1))
                {
                    DefaultAudioLanguage.Add(MainForm.Instance.Settings.DefaultLanguage1);
                    DefaultSubtitleLanguage.Add(MainForm.Instance.Settings.DefaultLanguage1);
                }
                if (!String.IsNullOrEmpty(MainForm.Instance.Settings.DefaultLanguage2) && !DefaultAudioLanguage.Contains(MainForm.Instance.Settings.DefaultLanguage2))
                    DefaultAudioLanguage.Add(MainForm.Instance.Settings.DefaultLanguage2);
                if (!String.IsNullOrEmpty(MainForm.Instance.Settings.DefaultLanguage2) && !DefaultSubtitleLanguage.Contains(MainForm.Instance.Settings.DefaultLanguage2))
                    DefaultSubtitleLanguage.Add(MainForm.Instance.Settings.DefaultLanguage2);
            }
            IndexerPriority.Add(FileIndexerWindow.IndexType.DGI.ToString());
            IndexerPriority.Add(FileIndexerWindow.IndexType.DGM.ToString());
            IndexerPriority.Add(FileIndexerWindow.IndexType.D2V.ToString());
            IndexerPriority.Add(FileIndexerWindow.IndexType.LSMASH.ToString());
            IndexerPriority.Add(FileIndexerWindow.IndexType.FFMS.ToString());
            IndexerPriority.Add(FileIndexerWindow.IndexType.AVISOURCE.ToString());
            ar = null;
		}

        #region GenericSettings Members

        public string SettingsID { get { return "OneClick"; } }
        public virtual void FixFileNames(Dictionary<string, string> _) { }

        public override bool Equals(object obj)
        {
            return Equals(obj as GenericSettings);
        }

        public bool Equals(GenericSettings other)
        {
            return other == null ? false : PropertyEqualityTester.AreEqual(this, other);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        object ICloneable.Clone()
        {
            return Clone();
        }

        GenericSettings GenericSettings.Clone()
        {
            return Clone();
        }

        public OneClickSettings Clone()
        {
            return this.MemberwiseClone() as OneClickSettings;
        }

        public string[] RequiredFiles
        {
            get { return new string[0]; }
        }

        public string[] RequiredProfiles
        {
            get { return new string[] { VideoProfileName, AudioSettings[0].Profile, AvsProfileName }; }
        }

        #endregion
    }
}