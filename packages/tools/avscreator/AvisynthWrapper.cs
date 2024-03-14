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
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;

namespace MeGUI
{
    [SerializableAttribute]
    public class AviSynthException:ApplicationException
	{
		public AviSynthException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		public AviSynthException(string message) : base(message)
		{
		}

		public AviSynthException(): base()
		{
		}

		public AviSynthException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}

	public enum AudioSampleType:int
	{
        Unknown=0,
		INT8  = 1,
		INT16 = 2, 
		INT24 = 4,    // Int24 is a very stupid thing to code, but it's supported by some hardware.
		INT32 = 8,
		FLOAT = 16
	};

	public sealed class AviSynthScriptEnvironment:IDisposable
	{
		public AviSynthScriptEnvironment()
		{
        }

        public AviSynthClip OpenScriptFile(string filePath)
        {
            return new AviSynthClip("Import", filePath, false, true);
        }

        public AviSynthClip OpenScriptFile(string filePath, bool bRequireRGB24)
        {
            return new AviSynthClip("Import", filePath, bRequireRGB24, true);
        }

        public AviSynthClip ParseScript(string script)
        {
            return new AviSynthClip("Eval", script, false, true);
        }

        public AviSynthClip ParseScript(string script, bool bRequireRGB24)
        {
            return new AviSynthClip("Eval", script, bRequireRGB24, true);
        }

        public AviSynthClip ParseScript(string script, bool bRequireRGB24, bool runInThread)
        {
            return new AviSynthClip("Eval", script, bRequireRGB24, runInThread);
        }

        public void Dispose()
		{

		}
	}

	/// <summary>
	/// Summary description for AviSynthClip.
	/// </summary>
	public class AviSynthClip: IDisposable
	{
		#region PInvoke related stuff
		[StructLayout(LayoutKind.Sequential)]
		struct AVSDLLVideoInfo
		{
			public int width;
			public int height;
			public int raten;
			public int rated;
			public int aspectn;
			public int aspectd;
			public int interlaced_frame;
			public int top_field_first;
			public int num_frames;
			public AviSynthColorspace pixel_type;

			// Audio
			public int audio_samples_per_second;
			public AudioSampleType sample_type;
			public int nchannels;
			public int num_audio_frames;
			public long num_audio_samples;
		}

        [DllImport("AvisynthWrapper", ExactSpelling = true, SetLastError = false, CharSet = CharSet.Ansi)]
        private static extern int dimzon_avs_init_2(ref IntPtr avs, string func, string arg, ref AVSDLLVideoInfo vi, ref AviSynthColorspace originalColorspace, ref AudioSampleType originalSampleType, string cs);
        [DllImport("AvisynthWrapper", ExactSpelling = true, SetLastError = false, CharSet = CharSet.Ansi)]
        private static extern int dimzon_avs_init_3(ref IntPtr avs, string func, string arg, ref AVSDLLVideoInfo vi, ref AviSynthColorspace originalColorspace, ref AudioSampleType originalSampleType, string cs);
        [DllImport("AvisynthWrapper", ExactSpelling = true, SetLastError = false, CharSet = CharSet.Ansi)]
        private static extern int dimzon_avs_destroy(ref IntPtr avs);
        [DllImport("AvisynthWrapper", ExactSpelling = true, SetLastError = false, CharSet = CharSet.Ansi)]
        private static extern int dimzon_avs_getlasterror(IntPtr avs, [MarshalAs(UnmanagedType.LPStr)] StringBuilder sb, int len);
        [DllImport("AvisynthWrapper", ExactSpelling = true, SetLastError = false, CharSet = CharSet.Ansi)]
        private static extern int dimzon_avs_getaframe(IntPtr avs, IntPtr buf, long sampleNo, long sampleCount);
        [DllImport("AvisynthWrapper", ExactSpelling = true, SetLastError = false, CharSet = CharSet.Ansi)]
        private static extern int dimzon_avs_getvframe(IntPtr avs, IntPtr buf, int stride, int frm);
        [DllImport("AvisynthWrapper", ExactSpelling = true, SetLastError = false, CharSet = CharSet.Ansi)]
        private static extern int dimzon_avs_getintvariable(IntPtr avs, string name, ref int val);
        [DllImport("AvisynthWrapper", ExactSpelling = true, SetLastError = false, CharSet = CharSet.Ansi)]
        private static extern int dimzon_avs_getinterfaceversion(ref int val);
        [DllImport("AvisynthWrapper", ExactSpelling = true, SetLastError = false, CharSet = CharSet.Ansi)]
        private static extern int dimzon_avs_getstrfunction(IntPtr avs, string func, [MarshalAs(UnmanagedType.LPStr)] StringBuilder sb, int len);
        [DllImport("AvisynthWrapper", ExactSpelling = true, SetLastError = false, CharSet = CharSet.Ansi)]
        private static extern int dimzon_avs_functionexists(IntPtr avs, string func, ref bool val);
        [DllImport("kernel32", SetLastError = true)]
        private static extern bool FreeLibrary(IntPtr hModule);
        [DllImport("kernel32", SetLastError = true)]
        private static extern bool LoadLibraryA(string hModule);
        [DllImport("kernel32", SetLastError = true)]
        private static extern bool GetModuleHandleExA(int dwFlags, string ModuleName, IntPtr phModule);
        [DllImport("Kernel32")]
        private extern static Boolean CloseHandle(IntPtr handle);

        #endregion

        private IntPtr _avs;
        private AVSDLLVideoInfo _vi;
        private AviSynthColorspace _colorSpace;
        private AudioSampleType _sampleType;
        private static object _locker = new object();
        private static object _lockerAccessCounter = new object();
        private static object _lockerDLL = new object();
        private static int _countDLL = 0;
        private int _countAccess = 0;
        private int _random;


#if dimzon

        #region syncronization staff

        private class EnvRef
        {
            public IntPtr env;
            public long refCount;
            public object lockObj;
            public int threadID;

            public EnvRef(IntPtr e, int threadID)
            {
                this.env = e;
                refCount = 1;
                lockObj = new object();
                this.threadID = threadID;
            }
        }
        private static Hashtable _threadHash = new Hashtable();

        private EnvRef createNewEnvRef(int threadId)
        {
            //TODO:
            return new EnvRef(new IntPtr(0), threadId);
        }

        private void destroyEnvRef(EnvRef r)
        {
            //TODO:
        }

        private EnvRef addRef()
        {
            int threadId = System.Threading.Thread.CurrentThread.ManagedThreadId;
            lock (_threadHash.SyncRoot)
            {
                EnvRef r;
                if(_threadHash.ContainsKey(threadId))
                {
                    r = (EnvRef)_threadHash[threadId];
                    lock(r.lockObj)
                    {
                        if (0 == r.refCount)
                        {
                            r = createNewEnvRef(threadId);
                            _threadHash.Remove(threadId);
                            _threadHash.Add(threadId, r);
                        }
                        else
                        {
                            ++r.refCount;
                        }
                    }
                }
                else
                {
                    r = createNewEnvRef(threadId);
                    _threadHash.Add(threadId, r);
                }
                return r;
            }
        }

        private void Release()
        {
            if (_avsEnv == null)
                return;
            lock (_avsEnv.lockObj)
            {
                --_avsEnv.refCount;
                if (0 == _avsEnv.refCount)
                {
                    destroyEnvRef(_avsEnv);
                }
            }
            _avsEnv = null;
        }

        private EnvRef _avsEnv;


        #endregion
#endif

        private string getLastError()
        {
            const int errlen = 1024;
            StringBuilder sb = new StringBuilder(errlen);
            try
            {
                if (_avs != IntPtr.Zero)
                {
                    AccessCounter(true);
                    sb.Length = dimzon_avs_getlasterror(_avs, sb, errlen);
                    AccessCounter(false);
                }
            }
            catch (Exception ex)
            {
                sb.AppendLine(ex.Message);
            }
            return sb.ToString();
        }

		#region Clip Properties

        public bool HasVideo
        {
            get
            {
                return VideoWidth > 0 && VideoHeight > 0;
            }
        }

		public int VideoWidth
		{
			get
			{
				return _vi.width;
			}
		}

		public int VideoHeight
		{
			get
			{
				return _vi.height;
			}
		}

		public int raten
		{
			get
			{
				return _vi.raten;
			}
		}

		public int rated
		{
			get
			{
				return _vi.rated;
			}
		}

		public int aspectn
		{
			get
			{
				return _vi.aspectn;
			}
		}

		public int aspectd
		{
			get
			{
				return _vi.aspectd;
			}
		}

		public int interlaced_frame
		{
			get
			{
				return _vi.interlaced_frame;
			}
		}

		public int top_field_first
		{
			get
			{
				return _vi.top_field_first;
			}
		}

		public int num_frames
		{
			get
			{
				return _vi.num_frames;
			}
		}

		// Audio
        public bool HasAudio
        {
            get
            {
                return _vi.num_audio_samples > 0;
            }
        }

		public int AudioSampleRate
		{
			get
			{
				return _vi.audio_samples_per_second;
			}
		}

		public long SamplesCount
		{
			get
			{
				return _vi.num_audio_samples;
			}
		}

		public AudioSampleType SampleType
		{
			get
			{
				return _vi.sample_type;
			}
		}

		public short ChannelsCount
		{
			get
			{
				return (short)_vi.nchannels;
			}
		}

        public AviSynthColorspace PixelType
        {
            get
            {
                return _vi.pixel_type;
            }
        }

        public AviSynthColorspace OriginalColorspace
        {
            get
            {
                return _colorSpace;
            }
        }

        public AudioSampleType OriginalSampleType
        {
            get
            {
                return _sampleType;
            }
        }

        #endregion

        public string GetStrFunction(string strFunction)
        {
            const int errlen = 1024;
            StringBuilder sb = new StringBuilder(errlen);
            if (_avs != IntPtr.Zero)
            {
                AccessCounter(true);
                sb.Length = dimzon_avs_getstrfunction(_avs, strFunction, sb, errlen);
                AccessCounter(false);
            }
            return sb.ToString();
        }

        public int GetIntVariable(string variableName, int defaultValue)
        {
            int v = 0;
            int res = 0;
            if (_avs != IntPtr.Zero)
            {
                AccessCounter(true);
                res = dimzon_avs_getintvariable(this._avs, variableName, ref v);
                AccessCounter(false);
            }
            if (res < 0)
                throw new AviSynthException(getLastError());
            return (0 == res) ? v : defaultValue;
        }

		public void ReadAudio(IntPtr addr, long offset, int count)
		{
            if (_avs != IntPtr.Zero)
            {
                AccessCounter(true);
                if (0 != dimzon_avs_getaframe(_avs, addr, offset, count))
                    throw new AviSynthException(getLastError());
                AccessCounter(false);
            }
        }

        public void ReadFrame(IntPtr addr, int stride, int frame)
        {
            if (_avs != IntPtr.Zero)
            {
                AccessCounter(true);
                if (0 != dimzon_avs_getvframe(_avs, addr, stride, frame))
                    throw new AviSynthException(getLastError());
                AccessCounter(false);
            }
        }

        /// <summary>
        /// Gets the AviSynth interface version of the AviSynthWrapper.dll
        /// </summary>
        /// <returns></returns>
        public static int GetAvisynthWrapperInterfaceVersion()
        {
            int iVersion = 0;
            try
            {
                int iResult = dimzon_avs_getinterfaceversion(ref iVersion);
            }
            catch (Exception) { }
            return iVersion;
        }

        /// <summary>
        /// Detects if the AviSynth version can be used
        /// </summary>
        /// <returns>0 if everything is fine, 3 if the version is outdated or a different value for other errors</param>
        public static int CheckAvisynthInstallation(out string strVersion, out bool bIsAVS26, out bool bIsAVSPlus, out bool bIsMT, out string strAviSynthDLL, ref core.util.LogItem oLog)
        {
            strVersion = "";
            bIsAVS26 = false;
            bIsAVSPlus = false;
            bIsMT = false;
            strAviSynthDLL = string.Empty;

            IntPtr _avs = new IntPtr(0);
            AVSDLLVideoInfo _vi = new AVSDLLVideoInfo();
            AviSynthColorspace _colorSpace = AviSynthColorspace.Unknown;
            AudioSampleType _sampleType = AudioSampleType.Unknown;

            int iStartResult = -1;
            try
            {
                iStartResult = dimzon_avs_init_2(ref _avs, "Eval", "Version()", ref _vi, ref _colorSpace, ref _sampleType, AviSynthColorspace.RGB24.ToString());
            
                foreach (System.Diagnostics.ProcessModule module in System.Diagnostics.Process.GetCurrentProcess().Modules)
                {
                    if (module.FileName.ToLowerInvariant().EndsWith("avisynth.dll"))
                        strAviSynthDLL = module.FileName.ToLowerInvariant();
                }

                if (iStartResult == 0)
                {
                    int iWrapperVersion = GetAvisynthWrapperInterfaceVersion();
                    try
                    {
                        const int errlen = 1024;
                        StringBuilder sb = new StringBuilder(errlen);
                        sb.Length = dimzon_avs_getstrfunction(_avs, "VersionString", sb, errlen);
                        strVersion = sb.ToString();

                        bool bResult = false;
                        int iResult = dimzon_avs_functionexists(_avs, "AutoloadPlugins", ref bResult);
                        bIsAVSPlus = false;
                        if (iResult == 0)
                            bIsAVSPlus = bResult;

                        if (iWrapperVersion < 5)
                        {
                            bResult = false;
                            iResult = dimzon_avs_functionexists(_avs, "ConvertToYV16", ref bResult);
                            bIsAVS26 = false;
                            if (iResult == 0)
                                bIsAVS26 = bResult;
                        }
                        else
                            bIsAVS26 = true;

                        string strMTFunction = "Prefetch";
                        if (!bIsAVSPlus)
                            strMTFunction = "SetMTMode";
                        bResult = false;
                        iResult = dimzon_avs_functionexists(_avs, strMTFunction, ref bResult);
                        bIsMT = false;
                        if (iResult == 0)
                            bIsMT = bResult;
                    }
                    catch (Exception ex)
                    {
                        oLog.LogValue("Error", ex.Message, core.util.ImageType.Error, false);
                    }
                }

                int iCloseResult = dimzon_avs_destroy(ref _avs);
                if (_avs != IntPtr.Zero)
                    CloseHandle(_avs);
                _avs = IntPtr.Zero;
            }
            catch (Exception ex)
            {
                oLog.LogValue("Error", ex.Message, core.util.ImageType.Error, false);
            }

            return iStartResult;
        }

        public AviSynthClip(string func, string arg, bool bRequireRGB24, bool bRunInThread)
		{
			_vi = new AVSDLLVideoInfo();
            _avs = IntPtr.Zero;
            _colorSpace = AviSynthColorspace.Unknown;
            _sampleType = AudioSampleType.Unknown;
            bool bOpenSuccess = false;
            string strErrorMessage = string.Empty;

            lock (_locker)
            {
                Random rnd = new Random();
                _random = rnd.Next(1, 1000000);

                if (MainForm.Instance.Settings.ShowDebugInformation)
                    HandleAviSynthWrapperDLL(false, arg);

                System.Windows.Forms.Application.UseWaitCursor = true;
                if (bRunInThread)
                {
                    Thread t = new Thread(new ThreadStart(delegate
                    {
                        bOpenSuccess = OpenAVSScript(func, arg, bRequireRGB24, out strErrorMessage);
                    }));
                    t.Start();
                    while (t.ThreadState == ThreadState.Running)
                        MeGUI.core.util.Util.Wait(1000);
                }
                else
                {
                    bOpenSuccess = OpenAVSScript(func, arg, bRequireRGB24, out strErrorMessage);
                }
                System.Windows.Forms.Application.UseWaitCursor = false;
            }

            if (bOpenSuccess == false)
            {
                string err = string.Empty;
                if (_avs != IntPtr.Zero)
                    err = getLastError();
                else
                    err = strErrorMessage;
                Dispose(false);
                throw new AviSynthException(err);
            }
		}

		~AviSynthClip()
		{
			Dispose(false);
		}

		public void Dispose()
		{
			Dispose(true);
		}

        protected virtual void Dispose(bool disposing)
        {
            if (_avs == IntPtr.Zero)
                return;

            // wait till the avs object is not used anymore
            while (_countAccess > 0)
                MeGUI.core.util.Util.Wait(100);
            dimzon_avs_destroy(ref _avs);
            if (_avs != IntPtr.Zero)
                CloseHandle(_avs);
            _avs = IntPtr.Zero;
            if (disposing)
                GC.SuppressFinalize(this);
            if (MainForm.Instance.Settings.ShowDebugInformation)
                HandleAviSynthWrapperDLL(true, String.Empty);
        }

        private bool OpenAVSScript(string func, string arg, bool bRequireRGB24, out string strErrorMessage)
        {
            bool bOpenSuccess = false;
            strErrorMessage = string.Empty;
            try
            {
                if (MainForm.Instance.Settings.AviSynthPlus)
                {
                    if (0 == dimzon_avs_init_3(ref _avs, func, arg, ref _vi, ref _colorSpace, ref _sampleType,
                        bRequireRGB24 ? AviSynthColorspace.RGB24.ToString() : AviSynthColorspace.Unknown.ToString()))
                        bOpenSuccess = true;
                }
                if (!bOpenSuccess)
                {
                    // fallback to the old function
                    if (0 == dimzon_avs_init_2(ref _avs, func, arg, ref _vi, ref _colorSpace, ref _sampleType, AviSynthColorspace.RGB24.ToString()))
                        bOpenSuccess = true;
                }
            }
            catch (Exception ex)
            {
                strErrorMessage = ex.Message;
            }
            return bOpenSuccess;
        }

        private void HandleAviSynthWrapperDLL(bool bUnload, string script)
        {
            lock (_lockerDLL)
            {
                if (MainForm.Instance.AviSynthWrapperLog == null)
                    MainForm.Instance.AviSynthWrapperLog = MainForm.Instance.Log.Info("AviSynthWrapper");

                bool bDebug = false;
#if DEBUG
                bDebug = true;
#endif
                core.util.LogItem _oLog = new core.util.LogItem("X");
                if (bUnload)
                {
                    _countDLL--;
                    if (_countDLL > 0)
                    {
                        _oLog = new core.util.LogItem("sessions open: " + _countDLL + ", id: " + _random + ", close");
                    }
                    else
                    {
                        bool bResult = false;
                        string strFile = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath), "avisynthwrapper.dll");
                        foreach (System.Diagnostics.ProcessModule mod in System.Diagnostics.Process.GetCurrentProcess().Modules)
                        {
                            if (mod.FileName.ToLowerInvariant().Equals(strFile.ToLowerInvariant()))
                                bResult = FreeLibrary(mod.BaseAddress);
                            else if (mod.FileName.ToLowerInvariant().EndsWith("avisynth.dll"))
                                FreeLibrary(mod.BaseAddress);
                        }
                        _oLog = new core.util.LogItem("sessions open: " + _countDLL + ", id: " + _random + ", close: " + bResult);
                    }
                }
                else
                {
                    if (_countDLL == 0)
                        LoadLibraryA("avisynthwrapper.dll");
                    _countDLL++;
                    _oLog = new core.util.LogItem("sessions open: " + _countDLL + ", id: " + _random);
                }

                string strFileName = String.Empty;
                if (System.IO.File.Exists(script))
                {
                    strFileName = script;
                    try
                    {
                        System.IO.StreamReader sr = new System.IO.StreamReader(strFileName, Encoding.Default);
                        script = sr.ReadToEnd();
                        sr.Close();
                    }
                    catch (Exception) { }
                }
                _oLog.LogValue("Script" + (!String.IsNullOrEmpty(strFileName) ? ": " + strFileName : string.Empty), script);
                if (bDebug)
                    _oLog.LogValue("StackTrace", Environment.StackTrace);
                MainForm.Instance.AviSynthWrapperLog.Add(_oLog);
            }
        }

        private void AccessCounter(bool bAdd)
        {
            lock (_lockerAccessCounter)
            {
                if (bAdd)
                    _countAccess++;
                else
                    _countAccess--;
            }
        }

        public short BitsPerSample
		{
			get
			{
				return (short)(BytesPerSample*8);
			}
		}

		public short BytesPerSample
		{
			get
			{
				switch (SampleType) 
				{
					case AudioSampleType.INT8:
						return 1;
					case AudioSampleType.INT16:
						return 2;
					case AudioSampleType.INT24:
						return 3;
					case AudioSampleType.INT32:
						return 4;
					case AudioSampleType.FLOAT:
						return 4;
					default:
						throw new ArgumentException(SampleType.ToString());
				}
			}
		}

        public int AvgBytesPerSec
        {
            get
            {
                return AudioSampleRate * ChannelsCount * BytesPerSample;
            }
        }

        public long AudioSizeInBytes
        {
            get
            {
                return (SamplesCount > 0 ? SamplesCount : 0) * ChannelsCount * BytesPerSample;
            }
        }
	}
}