// ****************************************************************************
// 
// Copyright (C) 2005-2025 Doom9 & al
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
using System.Runtime.InteropServices;
using System.IO;

using MeGUI;

namespace MediaInfoWrapper
{
    //disables the xml comment warning on compilation
#pragma warning disable 1591
    public enum StreamKind
    {
        General,
        Video,
        Audio,
        Text,
        Other,
        Image,
        Menu,
    }

    public enum InfoKind
    {
        Name,
        Text,
        Measure,
        Options,
        NameText,
        MeasureText,
        Info,
        HowTo
    }

    public enum InfoOptions
    {
        ShowInInform,
        Support,
        ShowInSupported,
        TypeOfValue
    }

    public enum InfoFileOptions
    {
        FileOption_Nothing = 0x00,
        FileOption_NoRecursive = 0x01,
        FileOption_CloseAll = 0x02,
        FileOption_Max = 0x04
    };

    public enum Status
    {
        None = 0x00,
        Accepted = 0x01,
        Filled = 0x02,
        Updated = 0x04,
        Finalized = 0x08,
    }
#pragma warning restore 1591

    /// <summary>
    /// When called with a proper file target, returns a MediaInfo object filled with list of media tracks containing
    /// every information MediaInfo.dll can collect.
    /// Tracks are accessibles as properties.
    /// </summary>
    public class MediaInfo : IDisposable
    {

        [DllImport("MediaInfo.dll")]
        private static extern IntPtr MediaInfo_New();
        [DllImport("MediaInfo.dll")]
        private static extern void MediaInfo_Delete(IntPtr Handle);
        [DllImport("MediaInfo.dll")]
        private static extern IntPtr MediaInfo_Open(IntPtr Handle, [MarshalAs(UnmanagedType.LPWStr)] string FileName);
        [DllImport("MediaInfo.dll")]
        private static extern IntPtr MediaInfoA_Open(IntPtr Handle, IntPtr FileName);
        [DllImport("MediaInfo.dll")]
        private static extern IntPtr MediaInfo_Open_Buffer_Init(IntPtr Handle, Int64 File_Size, Int64 File_Offset);
        [DllImport("MediaInfo.dll")]
        private static extern IntPtr MediaInfoA_Open(IntPtr Handle, Int64 File_Size, Int64 File_Offset);
        [DllImport("MediaInfo.dll")]
        private static extern IntPtr MediaInfo_Open_Buffer_Continue(IntPtr Handle, IntPtr Buffer, IntPtr Buffer_Size);
        [DllImport("MediaInfo.dll")]
        private static extern IntPtr MediaInfoA_Open_Buffer_Continue(IntPtr Handle, Int64 File_Size, byte[] Buffer, IntPtr Buffer_Size);
        [DllImport("MediaInfo.dll")]
        private static extern Int64 MediaInfo_Open_Buffer_Continue_GoTo_Get(IntPtr Handle);
        [DllImport("MediaInfo.dll")]
        private static extern Int64 MediaInfoA_Open_Buffer_Continue_GoTo_Get(IntPtr Handle);
        [DllImport("MediaInfo.dll")]
        private static extern IntPtr MediaInfo_Open_Buffer_Finalize(IntPtr Handle);
        [DllImport("MediaInfo.dll")]
        private static extern IntPtr MediaInfoA_Open_Buffer_Finalize(IntPtr Handle);
        [DllImport("MediaInfo.dll")]
        private static extern void MediaInfo_Close(IntPtr Handle);
        [DllImport("MediaInfo.dll")]
        private static extern IntPtr MediaInfo_Inform(IntPtr Handle, IntPtr Reserved);
        [DllImport("MediaInfo.dll")]
        private static extern IntPtr MediaInfoA_Inform(IntPtr Handle, IntPtr Reserved);
        [DllImport("MediaInfo.dll")]
        private static extern IntPtr MediaInfo_GetI(IntPtr Handle, IntPtr StreamKind, IntPtr StreamNumber, IntPtr Parameter, IntPtr KindOfInfo);
        [DllImport("MediaInfo.dll")]
        private static extern IntPtr MediaInfoA_GetI(IntPtr Handle, IntPtr StreamKind, IntPtr StreamNumber, IntPtr Parameter, IntPtr KindOfInfo);
        [DllImport("MediaInfo.dll")]
        private static extern IntPtr MediaInfo_Get(IntPtr Handle, IntPtr StreamKind, IntPtr StreamNumber, [MarshalAs(UnmanagedType.LPWStr)] string Parameter, IntPtr KindOfInfo, IntPtr KindOfSearch);
        [DllImport("MediaInfo.dll")]
        private static extern IntPtr MediaInfoA_Get(IntPtr Handle, IntPtr StreamKind, IntPtr StreamNumber, IntPtr Parameter, IntPtr KindOfInfo, IntPtr KindOfSearch);
        [DllImport("MediaInfo.dll")]
        private static extern IntPtr MediaInfo_Option(IntPtr Handle, [MarshalAs(UnmanagedType.LPWStr)] string Option, [MarshalAs(UnmanagedType.LPWStr)] string Value);
        [DllImport("MediaInfo.dll")]
        private static extern IntPtr MediaInfoA_Option(IntPtr Handle, IntPtr Option, IntPtr Value);
        [DllImport("MediaInfo.dll")]
        private static extern IntPtr MediaInfo_State_Get(IntPtr Handle);
        [DllImport("MediaInfo.dll")]
        private static extern IntPtr MediaInfo_Count_Get(IntPtr Handle, IntPtr StreamKind, IntPtr StreamNumber);

        private IntPtr Handle;
        private bool MustUseAnsi;
        private bool bSuccess;

        #region Handle DLL
        [DllImport("kernel32", SetLastError = true)]
        private static extern bool FreeLibrary(IntPtr hModule);
        [DllImport("kernel32", SetLastError = true)]
        private static extern bool LoadLibraryA(string hModule);

        private static object _locker = new object();
        private int _random;
        private static object _lockerDLL = new object();
        private static int _countDLL = 0;
        #endregion

        public bool OpenSuccess { get => bSuccess; }

        //MediaInfo class
        public MediaInfo()
        {
            Random rnd = new Random();
            _random = rnd.Next(1, 1000000);
            bSuccess = false;

            try
            {
                Handle = MediaInfo_New();
            }
            catch
            {
                Handle = (IntPtr)0;
            }
            if (Environment.OSVersion.ToString().IndexOf("Windows") == -1)
                MustUseAnsi = true;
            else
                MustUseAnsi = false;
        }

        /// <summary>
        /// When called with a proper file target, returns a MediaInfo object filled with list of media tracks containing
        /// information MediaInfo.dll can collect. Tracks are accessible as properties.
        /// </summary>
        /// <param name="path"></param>
        public MediaInfo(string path) : this ()
        {
            if (Handle == (IntPtr)0)
                return;

            _FileName = path;
            if (MainForm.Instance.Settings.ShowDebugInformation)
                HandleMediaInfoWrapperDLL(false);

            if (!File.Exists(path))
            {
                Close();
                return;
            }
            
            Open(path);
            getStreamCount();
            getAllInfos();
            Close();
            bSuccess = true;
        }

        private void HandleMediaInfoWrapperDLL(bool bUnload)
        {
            lock (_lockerDLL)
            {
                if (MainForm.Instance.MediaInfoWrapperLog == null)
                    MainForm.Instance.MediaInfoWrapperLog = MainForm.Instance.Log.Info("MediaInfoWrapper");

                bool bDebug = false;
#if DEBUG
                bDebug = true;
#endif
                MeGUI.core.util.LogItem _oLog = new MeGUI.core.util.LogItem("X");
                if (bUnload)
                {
                    _countDLL--;
                    if (_countDLL > 0)
                    {
                        _oLog = new MeGUI.core.util.LogItem("sessions open: " + _countDLL + ", id: " + _random + ", close");
                    }
                    else
                    {
                        bool bResult = false;
                        string strFile = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath), "mediainfo.dll");
                        foreach (System.Diagnostics.ProcessModule mod in System.Diagnostics.Process.GetCurrentProcess().Modules)
                        {
                            if (mod.FileName.ToLowerInvariant().Equals(strFile.ToLowerInvariant()))
                                bResult = FreeLibrary(mod.BaseAddress);
                        }
                        _oLog = new MeGUI.core.util.LogItem("sessions open: " + _countDLL + ", id: " + _random + ", close: " + bResult);
                    }
                }
                else
                {
                    if (_countDLL == 0)
                        LoadLibraryA("mediainfo.dll");
                    _countDLL++;
                    _oLog = new MeGUI.core.util.LogItem("sessions open: " + _countDLL + ", id: " + _random);
                }
                _oLog.LogValue("File", _FileName);
                if (bDebug)
                    _oLog.LogValue("StackTrace", Environment.StackTrace);
                MainForm.Instance.MediaInfoWrapperLog.Add(_oLog);
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // dispose managed state (managed objects).
                }

                if (Handle == (IntPtr)0)
                    return;
                MediaInfo_Delete(Handle);
                disposedValue = true;

                if (MainForm.Instance.Settings.ShowDebugInformation)
                    HandleMediaInfoWrapperDLL(true);
            }
        }

         ~MediaInfo()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion

        public int Open(String FileName)
        {
            if (Handle == (IntPtr)0)
                return 0;
            if (MustUseAnsi)
            {
                IntPtr FileName_Ptr = Marshal.StringToHGlobalAnsi(FileName);
                int ToReturn = (int)MediaInfoA_Open(Handle, FileName_Ptr);
                Marshal.FreeHGlobal(FileName_Ptr);
                return ToReturn;
            }
            else
                return (int)MediaInfo_Open(Handle, FileName);
        }

        public int Open_Buffer_Init(Int64 File_Size, Int64 File_Offset)
        {
            if (Handle == (IntPtr)0)
                return 0;
            return (int)MediaInfo_Open_Buffer_Init(Handle, File_Size, File_Offset);
        }

        public int Open_Buffer_Continue(IntPtr Buffer, IntPtr Buffer_Size)
        {
            if (Handle == (IntPtr)0)
                return 0;
            return (int)MediaInfo_Open_Buffer_Continue(Handle, Buffer, Buffer_Size);
        }

        public Int64 Open_Buffer_Continue_GoTo_Get()
        {
            if (Handle == (IntPtr)0)
                return 0;
            return (Int64)MediaInfo_Open_Buffer_Continue_GoTo_Get(Handle);
        }

        public int Open_Buffer_Finalize()
        {
            if (Handle == (IntPtr)0)
                return 0;
            return (int)MediaInfo_Open_Buffer_Finalize(Handle);
        }

        public void Close()
        {
            if (Handle == (IntPtr)0)
                return;
            MediaInfo_Close(Handle);
        }

        public String Inform()
        {
            if (Handle == (IntPtr)0)
                return "Unable to load MediaInfo library";
            if (MustUseAnsi)
                return Marshal.PtrToStringAnsi(MediaInfoA_Inform(Handle, (IntPtr)0));
            else
                return Marshal.PtrToStringUni(MediaInfo_Inform(Handle, (IntPtr)0));
        }

        public String Get(StreamKind StreamKind, int StreamNumber, String Parameter, InfoKind KindOfInfo, InfoKind KindOfSearch)
        {
            if (Handle == (IntPtr)0)
                return "Unable to load MediaInfo library";
            if (MustUseAnsi)
            {
                IntPtr Parameter_Ptr = Marshal.StringToHGlobalAnsi(Parameter);
                String ToReturn = Marshal.PtrToStringAnsi(MediaInfoA_Get(Handle, (IntPtr)StreamKind, (IntPtr)StreamNumber, Parameter_Ptr, (IntPtr)KindOfInfo, (IntPtr)KindOfSearch));
                Marshal.FreeHGlobal(Parameter_Ptr);
                return ToReturn;
            }
            else
                return Marshal.PtrToStringUni(MediaInfo_Get(Handle, (IntPtr)StreamKind, (IntPtr)StreamNumber, Parameter, (IntPtr)KindOfInfo, (IntPtr)KindOfSearch));
        }

        public String Get(StreamKind StreamKind, int StreamNumber, int Parameter, InfoKind KindOfInfo)
        {
            if (Handle == (IntPtr)0)
                return "Unable to load MediaInfo library";
            if (MustUseAnsi)
                return Marshal.PtrToStringAnsi(MediaInfoA_GetI(Handle, (IntPtr)StreamKind, (IntPtr)StreamNumber, (IntPtr)Parameter, (IntPtr)KindOfInfo));
            else
                return Marshal.PtrToStringUni(MediaInfo_GetI(Handle, (IntPtr)StreamKind, (IntPtr)StreamNumber, (IntPtr)Parameter, (IntPtr)KindOfInfo));
        }

        public String Option(String Option, String Value)
        {
            if (Handle == (IntPtr)0)
                return "Unable to load MediaInfo library";
            if (MustUseAnsi)
            {
                IntPtr Option_Ptr = Marshal.StringToHGlobalAnsi(Option);
                IntPtr Value_Ptr = Marshal.StringToHGlobalAnsi(Value);
                String ToReturn = Marshal.PtrToStringAnsi(MediaInfoA_Option(Handle, Option_Ptr, Value_Ptr));
                Marshal.FreeHGlobal(Option_Ptr);
                Marshal.FreeHGlobal(Value_Ptr);
                return ToReturn;
            }
            else
                return Marshal.PtrToStringUni(MediaInfo_Option(Handle, Option, Value));
        }

        public int State_Get()
        {
            if (Handle == (IntPtr)0)
                return 0;
            return (int)MediaInfo_State_Get(Handle);
        }

        public int Count_Get(StreamKind StreamKind, int StreamNumber)
        {
            if (Handle == (IntPtr)0)
                return 0;
            return (int)MediaInfo_Count_Get(Handle, (IntPtr)StreamKind, (IntPtr)StreamNumber);
        }

        public int Count_Get(StreamKind StreamKind)
        {
            return Count_Get(StreamKind, -1);
        }

        public String Get(StreamKind StreamKind, int StreamNumber, String Parameter, InfoKind KindOfInfo)
        {
            return Get(StreamKind, StreamNumber, Parameter, KindOfInfo, InfoKind.Name);
        }

        public String Get(StreamKind StreamKind, int StreamNumber, String Parameter)
        {
            return Get(StreamKind, StreamNumber, Parameter, InfoKind.Text, InfoKind.Name);
        }

        public String Get(StreamKind StreamKind, int StreamNumber, int Parameter)
        {
            return Get(StreamKind, StreamNumber, Parameter, InfoKind.Text);
        }

        public String Option(String Option_)
        {
            return Option(Option_, "");
        }


        private List<VideoTrack> _Video;
        private List<GeneralTrack> _General;
        private List<AudioTrack> _Audio;
        private List<TextTrack> _Text;
        private List<ChaptersTrack> _Chapters;
        private Int32 _VideoCount;
        private Int32 _GeneralCount;
        private Int32 _AudioCount;
        private Int32 _TextCount;
        private Int32 _ChaptersCount;
        private string _FileName;


        private string GetSpecificMediaInfo(StreamKind KindOfStream, int trackindex, string NameOfParameter)
        {
            return Get(KindOfStream, trackindex, NameOfParameter);
        }

        private void getStreamCount()
        {
            _AudioCount = Count_Get(StreamKind.Audio);
            _VideoCount = Count_Get(StreamKind.Video);
            _GeneralCount = Count_Get(StreamKind.General);
            _TextCount = Count_Get(StreamKind.Text);
            _ChaptersCount = Count_Get(StreamKind.Menu);
        }

        private void getAllInfos()
        {
            getVideoInfo();
            getAudioInfo();
            getChaptersInfo();
            getTextInfo();
            getGeneralInfo();
        }

        ///<summary> List of all the General streams available in the file, type GeneralTrack[trackindex] to access a specific track</summary>
        public List<GeneralTrack> General
        {
            get
            {
                if (this._General == null)
                {
                   getGeneralInfo();
                }
                return this._General;
            }
        }

        private void getGeneralInfo()
        {
            if (this._General == null)
            {
                this._General = new List<GeneralTrack>();
                int num1 = Count_Get(StreamKind.General);
                if (num1 > 0)
                {
                    int num3 = num1 - 1;
                    for (int num2 = 0; num2 <= num3; num2++)
                    {
                        GeneralTrack _tracktemp_ = new GeneralTrack();              
                        _tracktemp_.FileSize= GetSpecificMediaInfo(StreamKind.General,num2,"FileSize");
                        _tracktemp_.Format= GetSpecificMediaInfo(StreamKind.General,num2,"Format");
                        _tracktemp_.FormatString= GetSpecificMediaInfo(StreamKind.General,num2,"Format/String");
                        _tracktemp_.PlayTimeString3= GetSpecificMediaInfo(StreamKind.General,num2,"PlayTime/String3");
                        _tracktemp_.Attachments = GetSpecificMediaInfo(StreamKind.General, num2, "Attachments");
                        this._General.Add(_tracktemp_);
                    }
                }  
            }
        }

        ///<summary> List of all the Video streams available in the file, type VideoTrack[trackindex] to access a specific track</summary>
        public List<VideoTrack> Video
        {
            get
            {
                if (this._Video == null)
                {
                   getVideoInfo();
                }
                return this._Video;
            }
        }

        private void getVideoInfo()
        {
            if (this._Video == null)
            {
                this._Video = new List<VideoTrack>();
                int num1 = Count_Get(StreamKind.Video);
                if (num1 > 0)
                {
                    int num3 = num1 - 1;
                    for (int num2 = 0; num2 <= num3; num2++)
                    {
                        VideoTrack _tracktemp_ = new VideoTrack();                            
                        _tracktemp_.StreamOrder = GetSpecificMediaInfo(StreamKind.Video, num2, "StreamOrder");
                        _tracktemp_.ID = GetSpecificMediaInfo(StreamKind.Video,num2,"ID");
                        _tracktemp_.CodecID = GetSpecificMediaInfo(StreamKind.Video, num2, "CodecID");
                        _tracktemp_.CodecIDString = GetSpecificMediaInfo(StreamKind.Video, num2, "CodecID/String");
                        _tracktemp_.CodecIDInfo = GetSpecificMediaInfo(StreamKind.Video, num2, "CodecID/Info");
                        _tracktemp_.Format = GetSpecificMediaInfo(StreamKind.Video, num2, "Format");
                        _tracktemp_.FormatInfo = GetSpecificMediaInfo(StreamKind.Video, num2, "Format/Info");
                        _tracktemp_.FormatString = GetSpecificMediaInfo(StreamKind.Video, num2, "Format/String");
                        _tracktemp_.FormatCommercial = GetSpecificMediaInfo(StreamKind.Video, num2, "Format_Commercial_IfAny");
                        _tracktemp_.FormatVersion = GetSpecificMediaInfo(StreamKind.Video, num2, "Format_Version");
                        _tracktemp_.Width = GetSpecificMediaInfo(StreamKind.Video,num2,"Width");
                        _tracktemp_.Height = GetSpecificMediaInfo(StreamKind.Video,num2,"Height");
                        _tracktemp_.AspectRatio = GetSpecificMediaInfo(StreamKind.Video,num2,"AspectRatio");
                        _tracktemp_.AspectRatioString = GetSpecificMediaInfo(StreamKind.Video, num2, "AspectRatio/String");
                        _tracktemp_.PixelAspectRatio = GetSpecificMediaInfo(StreamKind.Video, num2, "PixelAspectRatio");
                        _tracktemp_.FrameRate = GetSpecificMediaInfo(StreamKind.Video,num2,"FrameRate");
                        _tracktemp_.FrameRateNum = GetSpecificMediaInfo(StreamKind.Video, num2, "FrameRate_Num");
                        _tracktemp_.FrameRateDen = GetSpecificMediaInfo(StreamKind.Video, num2, "FrameRate_Den");
                        _tracktemp_.FrameRateOriginal = GetSpecificMediaInfo(StreamKind.Video, num2, "FrameRate_Original");
                        _tracktemp_.FrameRateMode = GetSpecificMediaInfo(StreamKind.Video, num2, "FrameRate_Mode");
                        _tracktemp_.FrameRateModeString = GetSpecificMediaInfo(StreamKind.Video, num2, "FrameRate_Mode/String");
                        _tracktemp_.FrameCount = GetSpecificMediaInfo(StreamKind.Video,num2,"FrameCount");
                        _tracktemp_.BitDepth = GetSpecificMediaInfo(StreamKind.Video,num2,"BitDepth");
                        _tracktemp_.Delay = GetSpecificMediaInfo(StreamKind.Video,num2,"Delay");
                        _tracktemp_.DurationString3 = GetSpecificMediaInfo(StreamKind.Video,num2,"Duration/String3");
                        _tracktemp_.Language = GetSpecificMediaInfo(StreamKind.Video,num2,"Language");
                        _tracktemp_.LanguageString = GetSpecificMediaInfo(StreamKind.Video,num2,"Language/String");
                        _tracktemp_.ScanTypeString = GetSpecificMediaInfo(StreamKind.Video, num2, "ScanType/String");
                        _tracktemp_.Default = GetSpecificMediaInfo(StreamKind.Video, num2, "Default");
                        _tracktemp_.Forced = GetSpecificMediaInfo(StreamKind.Video, num2, "Forced");
                        _tracktemp_.Title = GetSpecificMediaInfo(StreamKind.Video, num2, "Title");
                        _tracktemp_.Source = GetSpecificMediaInfo(StreamKind.Video, num2, "Source");
                        this._Video.Add(_tracktemp_);
                    }
                }
            }
        }

        ///<summary> List of all the Audio streams available in the file, type AudioTrack[trackindex] to access a specific track</summary>
        public List<AudioTrack> Audio
        {
            get
            {
                if (this._Audio == null)
                {
                   getAudioInfo();
                }
                return this._Audio;
            }
        }

        private void getAudioInfo()
        {
            if (this._Audio == null)
            {
                this._Audio = new List<AudioTrack>();
                int num1 = Count_Get(StreamKind.Audio);
                if (num1 > 0)
                {
                    int num3 = num1 - 1;
                    for (int num2 = 0; num2 <= num3; num2++)
                    {
                        AudioTrack _tracktemp_ = new AudioTrack();
                        _tracktemp_.StreamOrder = GetSpecificMediaInfo(StreamKind.Audio, num2, "StreamOrder");
                        _tracktemp_.ID= GetSpecificMediaInfo(StreamKind.Audio,num2,"ID");
                        _tracktemp_.CodecID = GetSpecificMediaInfo(StreamKind.Audio, num2, "CodecID");
                        _tracktemp_.CodecIDString = GetSpecificMediaInfo(StreamKind.Audio, num2, "CodecID/String");
                        _tracktemp_.CodecIDInfo = GetSpecificMediaInfo(StreamKind.Audio, num2, "CodecID/Info");
                        _tracktemp_.Format = GetSpecificMediaInfo(StreamKind.Audio, num2, "Format");
                        _tracktemp_.FormatInfo = GetSpecificMediaInfo(StreamKind.Audio, num2, "Format/Info");
                        _tracktemp_.FormatString = GetSpecificMediaInfo(StreamKind.Audio, num2, "Format/String");
                        _tracktemp_.FormatCommercial = GetSpecificMediaInfo(StreamKind.Audio, num2, "Format_Commercial_IfAny");
                        _tracktemp_.FormatVersion = GetSpecificMediaInfo(StreamKind.Audio, num2, "Format_Version");
                        _tracktemp_.FormatProfile = GetSpecificMediaInfo(StreamKind.Audio, num2, "Format_Profile");
                        _tracktemp_.FormatSettingsSBR = GetSpecificMediaInfo(StreamKind.Audio, num2, "Format_Settings_SBR");
                        _tracktemp_.FormatSettingsPS = GetSpecificMediaInfo(StreamKind.Audio, num2, "Format_Settings_PS");
                        _tracktemp_.MuxingMode = GetSpecificMediaInfo(StreamKind.Audio, num2, "MuxingMode");
                        _tracktemp_.BitRateMode = GetSpecificMediaInfo(StreamKind.Audio, num2, "BitRate_Mode");
                        _tracktemp_.Channels = GetSpecificMediaInfo(StreamKind.Audio, num2, "Channel(s)");
                        _tracktemp_.ChannelsString = GetSpecificMediaInfo(StreamKind.Audio, num2, "Channel(s)/String");
                        _tracktemp_.ChannelPositionsString2 = GetSpecificMediaInfo(StreamKind.Audio, num2, "ChannelLayout");
                        _tracktemp_.SamplingRate = GetSpecificMediaInfo(StreamKind.Audio, num2, "SamplingRate");
                        _tracktemp_.SamplingRateString = GetSpecificMediaInfo(StreamKind.Audio, num2, "SamplingRate/String");
                        _tracktemp_.Delay = GetSpecificMediaInfo(StreamKind.Audio, num2, "Delay");
                        _tracktemp_.Title= GetSpecificMediaInfo(StreamKind.Audio,num2,"Title");
                        _tracktemp_.Language= GetSpecificMediaInfo(StreamKind.Audio,num2,"Language");
                        _tracktemp_.LanguageString= GetSpecificMediaInfo(StreamKind.Audio,num2,"Language/String");
                        _tracktemp_.Default = GetSpecificMediaInfo(StreamKind.Audio, num2, "Default");
                        _tracktemp_.Forced = GetSpecificMediaInfo(StreamKind.Audio, num2, "Forced");
                        _tracktemp_.Source = GetSpecificMediaInfo(StreamKind.Audio, num2, "Source");
                        this._Audio.Add(_tracktemp_);
                    }
                }
            }
        }

        ///<summary> List of all the Text streams available in the file, type TextTrack[trackindex] to access a specific track</summary>
        public List<TextTrack> Text
        {
            get
            {
                if (this._Text == null)
                {
                   getTextInfo();
                }
                return this._Text;
            }
        }

        private void getTextInfo()
        {
            if (this._Text == null)
            {
                this._Text = new List<TextTrack>();
                int num1 = Count_Get(StreamKind.Text);
                if (num1 > 0)
                {
                    int num3 = num1 - 1;
                    for (int num2 = 0; num2 <= num3; num2++)
                    {
                        TextTrack _tracktemp_ = new TextTrack();                              
                        _tracktemp_.StreamOrder = GetSpecificMediaInfo(StreamKind.Text, num2, "StreamOrder");
                        _tracktemp_.ID= GetSpecificMediaInfo(StreamKind.Text,num2,"ID");
                        _tracktemp_.Title= GetSpecificMediaInfo(StreamKind.Text,num2,"Title");
                        _tracktemp_.CodecID= GetSpecificMediaInfo(StreamKind.Text,num2,"CodecID");
                        _tracktemp_.CodecIDString = GetSpecificMediaInfo(StreamKind.Text, num2, "CodecID/String");
                        _tracktemp_.CodecIDInfo = GetSpecificMediaInfo(StreamKind.Text, num2, "CodecID/Info");
                        _tracktemp_.Format = GetSpecificMediaInfo(StreamKind.Text, num2, "Format");
                        _tracktemp_.FormatInfo = GetSpecificMediaInfo(StreamKind.Text, num2, "Format/Info");
                        _tracktemp_.FormatString = GetSpecificMediaInfo(StreamKind.Text, num2, "Format/String");
                        _tracktemp_.FormatCommercial = GetSpecificMediaInfo(StreamKind.Text, num2, "Format_Commercial_IfAny");
                        _tracktemp_.FormatVersion = GetSpecificMediaInfo(StreamKind.Text, num2, "Format_Version");
                        _tracktemp_.Delay= GetSpecificMediaInfo(StreamKind.Text,num2,"Delay");
                        _tracktemp_.Language= GetSpecificMediaInfo(StreamKind.Text,num2,"Language");
                        _tracktemp_.LanguageString= GetSpecificMediaInfo(StreamKind.Text,num2,"Language/String");
                        _tracktemp_.Default = GetSpecificMediaInfo(StreamKind.Text, num2, "Default");
                        _tracktemp_.Forced = GetSpecificMediaInfo(StreamKind.Text, num2, "Forced");
                        _tracktemp_.Source = GetSpecificMediaInfo(StreamKind.Text, num2, "Source");
                        this._Text.Add(_tracktemp_);
                    }
                }
            }
        }

        ///<summary> List of all the Chapters streams available in the file, type ChaptersTrack[trackindex] to access a specific track</summary>
        public List<ChaptersTrack> Chapters
        {
            get
            {
                if (this._Chapters == null)
                {
                   getChaptersInfo();
                }
                return this._Chapters;
            }
        }

        private void getChaptersInfo()
        {
            if (this._Chapters != null)
                return;

            this._Chapters = new List<ChaptersTrack>();
            int num1 = Count_Get(StreamKind.Menu);
            if (num1 <= 0)
                return;

            int num3 = num1 - 1;
            for (int num2 = 0; num2 <= num3; num2++)
            {
                int iStart = 0;
                int iEnd = 0;
                Int32.TryParse(GetSpecificMediaInfo(StreamKind.Menu, num2, "Chapters_Pos_Begin"), out iStart);
                Int32.TryParse(GetSpecificMediaInfo(StreamKind.Menu, num2, "Chapters_Pos_End"), out iEnd);

                if (iStart == iEnd)
                    continue;

                ChaptersTrack _tracktemp_ = new ChaptersTrack();
                for (int i = iStart; i < iEnd; i++)
                    _tracktemp_.Chapters.Add(Get(StreamKind.Menu, num2, i, InfoKind.Name), Get(StreamKind.Menu, num2, i, InfoKind.Text));

                this._Chapters.Add(_tracktemp_);
            }
        }
    }
}