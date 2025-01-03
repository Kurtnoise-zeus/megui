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
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace MeGUI
{
    public enum ResizeFilterType
    {

        [EnumTitle("Bilinear (Soft)", "BilinearResize({0},{1})")]
        Bilinear=0,
        [EnumTitle("Bicubic (Sharp)", "BicubicResize({0},{1},0,0.75)")]
        BicubicSharp,
        [EnumTitle("Bicubic (Neutral)", "BicubicResize({0},{1},0,0.5)")]
        BicubicNeutral,
        [EnumTitle("Bicubic (Soft)", "BicubicResize({0},{1},0.333,0.333)")]
        BicubicSoft,
        [EnumTitle("Blackman", "BlackmanResize({0},{1})")]
        Blackman,
        [EnumTitle("Gauss (Neutral)", "GaussResize({0},{1})")]
        Gauss,
        [EnumTitle("Lanczos (Sharp)", "LanczosResize({0},{1})")]
        Lanczos,
        [EnumTitle("Lanczos4 (Sharp)", "Lanczos4Resize({0},{1})")]
        Lanczos4,
        [EnumTitle("Point (Sharp)", "PointResize({0},{1})")]
        Point,
        [EnumTitle("Sinc (Neutral)", "SincResize({0},{1})")]
        Sinc,
        [EnumTitle("Sinc (8 taps)", "SincResize({0},{1},8)")]
        Sinc8,
        [EnumTitle("Spline16 (Neutral)", "Spline16Resize({0},{1})")]
        Spline16,
        [EnumTitle("Spline36 (Neutral)", "Spline36Resize({0},{1})")]
        Spline36,
        [EnumTitle("Spline64 (Sharp)", "Spline64Resize({0},{1})")]
        Spline64
    }

    public enum DenoiseFilterType
    {
        [EnumTitle("Minimal Noise", "Undot()")]
        MinimalNoise = 0,
        [EnumTitle("Little Noise", "mergechroma(blur(1.3))")]
        LittleNoise,
        [EnumTitle("Medium Noise", "FluxSmoothST(7,7)")]
        MediumNoise
#if x86
        ,[EnumTitle("Heavy Noise", "Convolution3D(\"movielq\")")]
        HeavyNoise
#endif
    }

    public enum NvDeinterlacerType
    {
        [EnumTitle("No Deinterlacing", ", deinterlace=0")]
        nvDeInterlacerNone = 0,
        [EnumTitle("Single Rate Deinterlacing", ", deinterlace=1")]
        nvDeInterlacerSingle,
        [EnumTitle("Bobbing", ", deinterlace=2")]
        nvDeInterlacerDouble
    }

    public enum HwdDevice
    {
        [EnumTitle("None", "")]
        hwdDeviceNone = 0,
        [EnumTitle("d3d11va", ", hwdevice=\"" + "d3d11va" + "\"")]
        hwdDeviceD3d11,
        [EnumTitle("Cuda", ", hwdevice=\"" + "cuda" + "\"" )]
        hwdDeviceCuda,
        [EnumTitle("Vulkan", ", hwdevice=\"" + "vulkan" + "\"")]
        hwdDeviceVulkan
    }

    public enum UserSourceType
    {
        [EnumTitle("Progressive", SourceType.PROGRESSIVE)]
        Progressive,
        [EnumTitle("Interlaced", SourceType.INTERLACED)]
        Interlaced,
        [EnumTitle("Film", SourceType.FILM)]
        Film,
        [EnumTitle("M-in-5 decimation required", SourceType.DECIMATING)]
        Decimating,
        [EnumTitle("Hybrid film/interlaced. Mostly film", SourceType.HYBRID_FILM_INTERLACED)]
        HybridFilmInterlaced,
        [EnumTitle("Hybrid film/interlaced. Mostly interlaced", SourceType.HYBRID_FILM_INTERLACED)]
        HybridInterlacedFilm,
        [EnumTitle("Partially interlaced", SourceType.HYBRID_PROGRESSIVE_INTERLACED)]
        HybridProgressiveInterlaced,
        [EnumTitle("Partially film", SourceType.HYBRID_PROGRESSIVE_FILM)]
        HybridProgressiveFilm
    }

    public enum UserFieldOrder
    {
        [EnumTitle("Top Field First", FieldOrder.TFF)]
        TFF,
        [EnumTitle("Bottom Field First", FieldOrder.BFF)]
        BFF,
        [EnumTitle("Varying field order", FieldOrder.VARIABLE)]
        Varying
    }

    public class ScriptServer
    {
        public static readonly IList ListOfResizeFilterType = EnumProxy.CreateArray(typeof(ResizeFilterType));

        public static readonly IList ListOfDenoiseFilterType = EnumProxy.CreateArray(typeof(DenoiseFilterType));

        public static readonly IList ListOfSourceTypes = EnumProxy.CreateArray(typeof(UserSourceType));

        public static readonly IList ListOfFieldOrders = EnumProxy.CreateArray(typeof(UserFieldOrder));

        public static readonly IList ListOfNvDeIntType = EnumProxy.CreateArray(typeof(NvDeinterlacerType));

        public static readonly IList ListOfHwDeviceType = EnumProxy.CreateArray(typeof(HwdDevice));

        public static MainForm mainForm;
        

        public static string CreateScriptFromTemplate(string template, string inputLine, string cropLine, string resizeLine, string denoiseLines, string deinterlaceLines)
        {
            string newScript = template;
            newScript = newScript.Replace("<crop>", cropLine);
            newScript = newScript.Replace("<resize>", resizeLine);
            newScript = newScript.Replace("<denoise>", denoiseLines);
            newScript = newScript.Replace("<deinterlace>", deinterlaceLines);
            newScript = newScript.Replace("<input>", inputLine);
            return newScript;
        }

        public static string GetInputLine(string input, string indexFile, bool interlaced, PossibleSources sourceType,
            bool colormatrix, bool mpeg2deblock, bool flipVertical, double fps, bool dss2,
            NvDeinterlacerType nvDeintType, int nvHorizontalResolution, int nvVerticalResolution, CropValues nvCropValues, HwdDevice hwdDevice, bool timecodesv2)
        {
            string inputLine = "#input";
            string strDLLPath = "";

            switch (sourceType)
            {
                case PossibleSources.avs:
                    inputLine = "Import(\"" + input + "\")";
                    break;
                case PossibleSources.d2v:
                    UpdateCacher.CheckPackage("dgindex");
                    if (String.IsNullOrEmpty(indexFile))
                        indexFile = input;
                    strDLLPath = Path.Combine(Path.GetDirectoryName(MainForm.Instance.Settings.DGIndex.Path), "DGDecode.dll");
                    inputLine = "LoadPlugin(\"" + strDLLPath + "\")\r\nDGDecode_mpeg2source(\"" + indexFile + "\"";
                    if (mpeg2deblock)
                        inputLine += ", cpu=4";
                    if (colormatrix)
                        inputLine += ", info=3";
                    inputLine += ")";
                    if (colormatrix)
                        inputLine += string.Format("\r\nLoadPlugin(\"" + Path.Combine(MainForm.Instance.Settings.AvisynthPluginsPath, "ColorMatrix.dll") + "\")\r\nColorMatrix(hints=true{0}, threads=0)", interlaced ? ", interlaced=true" : "");
                    break;
                case PossibleSources.dgm:
                    UpdateCacher.CheckPackage("dgindexim");
                    if (String.IsNullOrEmpty(indexFile))
                        indexFile = input;
                    strDLLPath = Path.Combine(Path.GetDirectoryName(MainForm.Instance.Settings.DGIndexIM.Path), "dgdecodeim.dll");
                    inputLine = "LoadPlugin(\"" + strDLLPath + "\")\r\nDGSourceIM(\"" + indexFile + "\", silent=true)";
                    if (MainForm.Instance.Settings.AviSynthPlus && MainForm.Instance.Settings.Input8Bit)
                        inputLine += "\r\nConvertBits(8)";
                    break;
                case PossibleSources.dgi:
                    UpdateCacher.CheckPackage("dgindexnv");
                    if (String.IsNullOrEmpty(indexFile))
                        indexFile = input;
                    strDLLPath = Path.Combine(Path.GetDirectoryName(MainForm.Instance.Settings.DGIndexNV.Path), "DGDecodeNV.dll");
                    inputLine = "LoadPlugin(\"" + strDLLPath + "\")\r\nDGSource(\"" + indexFile + "\"";
                    if (MainForm.Instance.Settings.AutoForceFilm &&
                        MainForm.Instance.Settings.ForceFilmThreshold <= (decimal)dgiFile.GetFilmPercent(indexFile))
                        inputLine += ",fieldop=1"; // fieldop=0 is the default value
                    if (nvDeintType != NvDeinterlacerType.nvDeInterlacerNone)
                        inputLine += ScriptServer.GetNvDeInterlacerLine(true, nvDeintType);
                    if (nvCropValues != null && nvCropValues.isCropped())
                    {
                        GetMod4Cropping(ref nvCropValues);
                        inputLine += ", ct=" + nvCropValues.top + ", cb=" + nvCropValues.bottom + ", cl=" + nvCropValues.left + ", cr=" + nvCropValues.right;
                    }
                    if (nvHorizontalResolution > 0 && nvVerticalResolution > 0)
                        inputLine += ", rw=" + nvHorizontalResolution + ", rh=" + nvVerticalResolution;
                    inputLine += ")";
                    if (MainForm.Instance.Settings.AviSynthPlus && MainForm.Instance.Settings.Input8Bit)
                        inputLine += "\r\nConvertBits(8)";
                    break;
                case PossibleSources.ffindex:
                    inputLine = VideoUtil.getFFMSVideoInputLine(input, indexFile, fps);
                    break;
                case PossibleSources.lsmash:
                    MediaInfoFile oInfo = null;
                    inputLine = VideoUtil.GetLSMASHVideoInputLine(input, indexFile, fps, ref oInfo);
                    if (oInfo != null)
                        oInfo.Dispose();
                    break;
                case PossibleSources.vdr:
                case PossibleSources.avisource:
                    inputLine = "AVISource(\"" + input + "\", audio=false)" + VideoUtil.getAssumeFPS(fps, input);
                    if (MainForm.Instance.Settings.AviSynthPlus && MainForm.Instance.Settings.Input8Bit)
                        inputLine += "\r\nConvertBits(8)";
                    break;
                case PossibleSources.directShow:
                    if (dss2)
                    {
                        string path = MeGUI.core.util.FileUtil.GetHaaliInstalledPath();
                        if (!File.Exists(Path.Combine(path, "avss.dll")))
                        {
                            UpdateCacher.CheckPackage("haali");
                            path = Path.GetDirectoryName(MainForm.Instance.Settings.Haali.Path);
                        }
                        inputLine = "LoadPlugin(\"" + path + "\\avss.dll" + "\")\r\ndss2(\"" + input + "\"" + ((fps > 0) ? ", fps=" + fps.ToString("F3", new CultureInfo("en-us")) : string.Empty) + ")" + VideoUtil.getAssumeFPS(fps, input);
                    }
                    else
                    {
                        inputLine = String.Empty;
                        if (MainForm.Instance.Settings.PortableAviSynth)
                            inputLine = "LoadPlugin(\"" + Path.Combine(Path.GetDirectoryName(MainForm.Instance.Settings.AviSynth.Path), @"plugins\directshowsource.dll") + "\")\r\n";
                        inputLine += "DirectShowSource(\"" + input + "\"" + ((fps > 0) ? ", fps=" + fps.ToString("F3", new CultureInfo("en-us")) : string.Empty) + ", audio=false, convertfps=true)" + VideoUtil.getAssumeFPS(fps, input);
                    }
                    if (MainForm.Instance.Settings.AviSynthPlus && MainForm.Instance.Settings.Input8Bit)
                        inputLine += "\r\nConvertBits(8)";
                    if (flipVertical)
                        inputLine = inputLine + "\r\nFlipVertical()";
                    break;
                case PossibleSources.bestsource:
                    UpdateCacher.CheckPackage("bestsource");

                    inputLine = String.Empty;
                    bool fpsdetails = VideoUtil.GetFPSDetails(fps, input, out int fpsnum, out int fpsden);

                    //if (!String.IsNullOrEmpty(Path.GetDirectoryName(MainForm.Instance.Settings.BestSource.Path)))
                    inputLine = "LoadPlugin(\"" + Path.Combine(Path.GetDirectoryName(MainForm.Instance.Settings.BestSource.Path), "BestSource.dll") + "\")\r\n";

                    inputLine += "BSVideoSource(\"" + input + "\"" + ((fpsdetails == true) ? ", fpsnum=" + fpsnum.ToString() + ", fpsden=" + fpsden.ToString() : String.Empty);

                    if (hwdDevice != (HwdDevice.hwdDeviceNone))
                        inputLine += ScriptServer.GetGetHwdDevice(true, hwdDevice);

                    if (timecodesv2)
                        inputLine += ", timecodes=\"" + Path.Combine(Path.GetDirectoryName(MainForm.Instance.Settings.BestSource.Path), "timecodes_v2.txt");

                    if (hwdDevice != (HwdDevice.hwdDeviceNone) || timecodesv2)
                        inputLine += "\")";
                    else
                        inputLine += ")";

                    if (MainForm.Instance.Settings.AviSynthPlus && MainForm.Instance.Settings.Input8Bit)
                        inputLine += "\r\nConvertBits(8)";
                    
                    if (flipVertical)
                        inputLine = inputLine + "\r\nFlipVertical()";
                    break;

            }
            return inputLine;
        }

        public static string GetCropLine(CropValues cropValues)
        {
            string cropLine = "#crop";
            if (cropValues.isCropped())
            {
                cropLine = string.Format("crop({0}, {1}, {2}, {3})", cropValues.left, cropValues.top, -cropValues.right, -cropValues.bottom);
            }
            return cropLine;
        }

        private static void GetMod4Cropping(ref CropValues cropValues)
        {
            if (!cropValues.isCropped())
                return;

            cropValues.left = cropValues.left + cropValues.left % 2;
            cropValues.top = cropValues.top + cropValues.top % 2;
            cropValues.right = cropValues.right + cropValues.right % 2;
            cropValues.bottom = cropValues.bottom + cropValues.bottom % 2;
        }

        public static string GetResizeLine(bool resize, int hres, int vres, int hresWithBorder, int vresWithBorder, ResizeFilterType type, bool crop, CropValues cropValues, int originalHRes, int originalVRes)
        {
            int iInputHresAfterCrop = originalHRes;
            int iInputVresAfterCrop = originalVRes;
            if (crop)
            {
                iInputHresAfterCrop = iInputHresAfterCrop - cropValues.left - cropValues.right;
                iInputVresAfterCrop = iInputVresAfterCrop - cropValues.top - cropValues.bottom;
            }

            // only resize if necessary
            if (!resize || (hres == iInputHresAfterCrop && vres == iInputVresAfterCrop))
            {
                if (hresWithBorder > iInputHresAfterCrop || vresWithBorder > iInputVresAfterCrop)
                    return getAddBorders(hres, vres, hresWithBorder, vresWithBorder);
                else
                    return "#resize";
            }
                
            EnumProxy p = EnumProxy.Create(type);
            if (p.Tag != null)
                if (hresWithBorder > hres || vresWithBorder > vres)
                    return string.Format(p.Tag + "." + getAddBorders(hres, vres, hresWithBorder, vresWithBorder) + " # {2}", hres, vres, p);
                else
                    return string.Format(p.Tag + " # {2}", hres, vres, p);
            else
                return "#resize - " + p;
        }

        private static string getAddBorders(int hres, int vres, int hresWithBorder, int vresWithBorder)
        {
            CropValues borderValues = new CropValues();
            borderValues.left = (int)Math.Floor((hresWithBorder - hres) / 2.0);
            borderValues.top = (int)Math.Floor((vresWithBorder - vres) / 2.0);
            borderValues.right = (int)Math.Ceiling((hresWithBorder - hres) / 2.0);
            borderValues.bottom = (int)Math.Ceiling((vresWithBorder - vres) / 2.0);

            // border values must be even for AviSynth 2.6
            if (borderValues.left % 2 != 0 && borderValues.right % 2 != 0)
            {
                borderValues.left -= 1;
                borderValues.right += 1;
            }
            if (borderValues.top % 2 != 0 && borderValues.bottom % 2 != 0)
            {
                borderValues.top -= 1;
                borderValues.bottom += 1;
            }

            return string.Format("AddBorders({0},{1},{2},{3})", borderValues.left, borderValues.top, borderValues.right, borderValues.bottom);
        }

        public static string GetDenoiseLines(bool denoise, DenoiseFilterType type)
        {
            string denoiseLines = "#denoise";
            string strPath = "";
            if (denoise)
            {
                EnumProxy p = EnumProxy.Create(type);
                if (p.Tag != null)
                {
                    if (p.Tag.ToString().ToLowerInvariant().Contains("undot"))
                        strPath = "LoadPlugin(\"" + Path.Combine(MainForm.Instance.Settings.AvisynthPluginsPath, "UnDot.dll") + "\")\r\n";
                    else if (p.Tag.ToString().ToLowerInvariant().Contains("fluxsmoothst"))
                        strPath = "LoadPlugin(\"" + Path.Combine(MainForm.Instance.Settings.AvisynthPluginsPath, "FluxSmooth.dll") + "\")\r\n";
                    else if (p.Tag.ToString().ToLowerInvariant().Contains("convolution3d"))
                        strPath = "LoadPlugin(\"" + Path.Combine(MainForm.Instance.Settings.AvisynthPluginsPath, "Convolution3DYV12.dll") + "\")\r\n";
                    denoiseLines = string.Format(strPath + p.Tag + " # " + p);
                }
                else
                    denoiseLines = "#denoise - " + p;
            }
            return denoiseLines;
        }

        public static string GetNvDeInterlacerLine(bool deint, NvDeinterlacerType type)
        {
            string nvDeInterlacerLine = "";
            if (deint)
            {
                EnumProxy p = EnumProxy.Create(type);
                if (p.Tag != null)
                    nvDeInterlacerLine = p.Tag.ToString();
            }
            return nvDeInterlacerLine;
        }

        public static string GetGetHwdDevice(bool hwd, HwdDevice type)
        {
            string hwdevice = "";
            if (hwd)
            {
                EnumProxy p = EnumProxy.Create(type);
                if (p.Tag != null)
                    hwdevice = p.Tag.ToString();
            }
            return hwdevice;
        }


        public static List<DeinterlaceFilter> GetDeinterlacers(SourceInfo info)
        {
            List<DeinterlaceFilter> filters = new List<DeinterlaceFilter>();
            if (info.sourceType == SourceType.PROGRESSIVE)
            {
                filters.Add(new DeinterlaceFilter(
                    "Do nothing",
                    "#Not doing anything because the source is progressive"));
            }
            else if (info.sourceType == SourceType.DECIMATING)
            {
                ScriptServer.AddTDecimate(info.decimateM, filters);
            }
            else if (info.sourceType == SourceType.INTERLACED)
            {
                ScriptServer.AddYadif(info.fieldOrder, filters, false);
                ScriptServer.AddYadif(info.fieldOrder, filters, true);
                ScriptServer.AddTDeint(info.fieldOrder, filters, true, false, false);
                ScriptServer.AddTDeint(info.fieldOrder, filters, true, false, true);
                ScriptServer.AddTDeint(info.fieldOrder, filters, true, true, false);
                ScriptServer.AddTDeint(info.fieldOrder, filters, true, true, true);
                if (info.fieldOrder != FieldOrder.VARIABLE)
                    ScriptServer.AddLeakDeint(info.fieldOrder, filters);
                ScriptServer.AddTMC(info.fieldOrder, filters);
                ScriptServer.AddFieldDeint(info.fieldOrder, filters, true, true);
                ScriptServer.AddFieldDeint(info.fieldOrder, filters, true, false);
            }
            else if (info.sourceType == SourceType.FILM)
            {
                ScriptServer.AddTIVTC("", info.isAnime, false, true, false, info.fieldOrder, filters);
                ScriptServer.AddIVTC(info.fieldOrder, false, true, filters);
            }
            else if (info.sourceType == SourceType.HYBRID_FILM_INTERLACED ||
                info.sourceType == SourceType.HYBRID_PROGRESSIVE_FILM)
            {
                ScriptServer.AddTIVTC("", info.isAnime, true, info.majorityFilm, false,
                    info.fieldOrder, filters);
                ScriptServer.AddTIVTC("", info.isAnime, true, info.majorityFilm, true,
                    info.fieldOrder, filters);
                ScriptServer.AddIVTC(info.fieldOrder, true, info.majorityFilm, filters);
            }
            else if (info.sourceType == SourceType.HYBRID_PROGRESSIVE_INTERLACED)
            {
                ScriptServer.AddYadif(info.fieldOrder, filters, false);
                ScriptServer.AddYadif(info.fieldOrder, filters, true);
                ScriptServer.AddTDeint(info.fieldOrder, filters, false, false, false);
                ScriptServer.AddTDeint(info.fieldOrder, filters, true, false, true);
                ScriptServer.AddTDeint(info.fieldOrder, filters, true, true, false);
                ScriptServer.AddTDeint(info.fieldOrder, filters, false, true, true);
                ScriptServer.AddFieldDeint(info.fieldOrder, filters, false, true);
                ScriptServer.AddFieldDeint(info.fieldOrder, filters, false, false);
                if (info.fieldOrder != FieldOrder.VARIABLE)
                    ScriptServer.AddLeakDeint(info.fieldOrder, filters);
                ScriptServer.AddTMC(info.fieldOrder, filters);
            }
            return filters;
        }

#region deinterlacing snippets
        public static int Order(FieldOrder order)
        {
            int i_order = -1;
            if (order == FieldOrder.BFF)
                i_order = 0;
            if (order == FieldOrder.TFF)
                i_order = 1;
            return i_order;
        }

        public static void AddYadif(FieldOrder order, List<DeinterlaceFilter> filters, bool bobber)
        {
            string path = Path.Combine(MainForm.Instance.Settings.AvisynthPluginsPath, "yadifmod2.dll");
            filters.Add(new DeinterlaceFilter(
                bobber ? "Yadif (with Bob)" : "Yadif",
                string.Format("LoadPlugin(\"{0}\"){1}Yadifmod2({2}order={3})", 
                    path, Environment.NewLine,
                    bobber ? "mode=1, " : "", Order(order))));
        }

        public static void AddLeakDeint(FieldOrder order, List<DeinterlaceFilter> filters)
        {
            filters.Add(new DeinterlaceFilter(
                "LeakKernelDeint",
                string.Format("LoadPlugin(\"{0}\"){1}LeakKernelDeint(order={2},sharp=true)", Path.Combine(MainForm.Instance.Settings.AvisynthPluginsPath, "LeakKernelDeint.dll"), Environment.NewLine, Order(order))));
        }

        public static void AddTDeint(FieldOrder order, List<DeinterlaceFilter> filters, bool processAll, bool eedi2, bool bob)
        {
            StringBuilder script = new StringBuilder();
            script.Append("LoadPlugin(\"" + Path.Combine(MainForm.Instance.Settings.AvisynthPluginsPath, "TDeint.dll") + "\")\r\n");
            if (eedi2)
            {
                script.Append("LoadPlugin(\"" + Path.Combine(MainForm.Instance.Settings.AvisynthPluginsPath, "EEDI2.dll") + "\")\r\n");
                script.Append("edeintted = last.");
                if (order == FieldOrder.TFF)
                    script.Append("AssumeTFF().");
                else if (order == FieldOrder.BFF)
                    script.Append("AssumeBFF().");
                script.Append("SeparateFields().");
                if (!bob)
                    script.Append("SelectEven().");
                script.Append("EEDI2(field=-1)\r\n");
            }
            script.Append("TDeint(");

            if (bob)
                script.Append("mode=1,");
            if (order != FieldOrder.VARIABLE)
                script.Append("order=" + Order(order) + ",");
            if (!processAll) // For hybrid clips
                script.Append("full=false,");
            if (eedi2)
                script.Append("edeint=edeintted,");

            script = new StringBuilder(script.ToString().TrimEnd(new char[] { ',' }));
            script.Append(")");
            filters.Add(new DeinterlaceFilter(
                bob ? (eedi2 ? "TDeint (with EDI + Bob)" : "TDeint (with Bob)") : (eedi2 ? "TDeint (with EDI)" : "TDeint"),
                script.ToString()));
        }

        public static void AddFieldDeint(FieldOrder order, List<DeinterlaceFilter> filters, bool processAll, bool blend)
        {
            string name = "FieldDeinterlace";
            if (!blend)
                name = "FieldDeinterlace (no blend)";

            StringBuilder script = new StringBuilder();
            script.Append("LoadPlugin(\"" + Path.Combine(MainForm.Instance.Settings.AvisynthPluginsPath, "Decomb.dll") + "\")\r\n");
            if (order == FieldOrder.TFF)
                script.Append("AssumeTFF().");
            else if (order == FieldOrder.BFF)
                script.Append("AssumeBFF().");

            script.Append("FieldDeinterlace(");
            
            if (!blend)
                script.Append("blend=false");

            if (!processAll)
            {
                if (!blend)
                    script.Append(",");
                script.Append("full=false");
            }
            script.Append(")");
            filters.Add(new DeinterlaceFilter(
                name,
                script.ToString()));
        }

        public static void AddTMC(FieldOrder order, List<DeinterlaceFilter> filters)
        {
            string strPluginPath = Path.Combine(MainForm.Instance.Settings.AvisynthPluginsPath, "TomsMoComp.dll");
            if (!File.Exists(strPluginPath))
                return;

            filters.Add(new DeinterlaceFilter(
                "TomsMoComp",
                string.Format("LoadPlugin(\"{0}\"){1}TomsMoComp({2},5,1)", strPluginPath, Environment.NewLine, Order(order))));
        }

        public static void Portionize(List<DeinterlaceFilter> filters, string trimLine)
        {
            for (int i = 0; i < filters.Count; i++)
            {
                string script = filters[i].Script;
                StringBuilder newScript = new StringBuilder();
                newScript.AppendLine("original = last");
                newScript.Append("deintted = original.");
                newScript.AppendLine(script);
                newScript.Append(trimLine);
                filters[i].Script = newScript.ToString();
            }
        }

#endregion

#region IVTC snippets
        public static void AddTIVTC(string d2vFile, bool anime, bool hybrid, bool mostlyFilm, bool advancedDeinterlacing,
            FieldOrder fieldOrder, List<DeinterlaceFilter> filters)
        {
            StringBuilder script = new StringBuilder();
            script.Append("LoadPlugin(\"" + Path.Combine(MainForm.Instance.Settings.AvisynthPluginsPath, "TIVTC.dll") + "\")\r\n");
            if (advancedDeinterlacing)
            {
                script.Append("LoadPlugin(\"" + Path.Combine(MainForm.Instance.Settings.AvisynthPluginsPath, "EEDI2.dll") + "\")\r\n");
                script.Append("LoadPlugin(\"" + Path.Combine(MainForm.Instance.Settings.AvisynthPluginsPath, "TDeint.dll") + "\")\r\n");
                script.Append("edeintted = ");
                if (fieldOrder == FieldOrder.TFF)
                    script.Append("AssumeTFF().");
                else if (fieldOrder == FieldOrder.BFF)
                    script.Append("AssumeBFF().");
                script.AppendFormat("SeparateFields().SelectEven().EEDI2(field=-1)\r\n");
                script.Append("tdeintted = TDeint(edeint=edeintted");
                if (fieldOrder != FieldOrder.VARIABLE)
                    script.Append(",order=" + Order(fieldOrder));
                script.Append(")\r\n");
            }

            script.Append("tfm(");
            if (d2vFile.Length <= 0)
                script.AppendFormat("order={0}", Order(fieldOrder));
            if (advancedDeinterlacing)
            {
                if (d2vFile.Length <= 0)
                    script.Append(",");
                script.Append("clip2=tdeintted");
            }
            script.Append(")");

            script.Append(".tdecimate(");
            if (anime)
                script.Append("mode=1");
            if (hybrid)
            {
                if (anime)
                    script.Append(",");
                if (mostlyFilm)
                    script.Append("hybrid=1");
                else
                    script.Append("hybrid=3");
                
            }
            script.Append(")");
            filters.Add(new DeinterlaceFilter(
                advancedDeinterlacing ? "TIVTC + TDeint(EDI) -- slow" : "TIVTC",
                script.ToString()));
        }

        public static void AddIVTC(FieldOrder order, bool hybrid, bool mostlyFilm,
            List<DeinterlaceFilter> filters)
        {
            StringBuilder script = new StringBuilder();
            script.Append("LoadPlugin(\"" + Path.Combine(MainForm.Instance.Settings.AvisynthPluginsPath, "Decomb.dll") + "\")\r\n");
            if (order == FieldOrder.TFF)
                script.Append("AssumeTFF().");
            else if (order == FieldOrder.BFF)
                script.Append("AssumeBFF().");

            script.Append("Telecide(guide=1).Decimate(");

            if (hybrid)
            {
                if (mostlyFilm)
                    script.Append("mode=3,");
                else
                    script.Append("mode=1,");

                script.Append("threshold=2.0");
            }

            script.Append(")");
            filters.Add(new DeinterlaceFilter(
                "Decomb IVTC",
                script.ToString()));
        }


#endregion

#region decimate snippet
        public static void AddTDecimate(int decimateM, List<DeinterlaceFilter> filters)
        {
            filters.Add(new DeinterlaceFilter(
                "Tritical Decimate",
                string.Format("LoadPlugin(\"{0}\"){1}TDecimate(cycleR={2})", Path.Combine(MainForm.Instance.Settings.AvisynthPluginsPath, "TIVTC.dll"), Environment.NewLine, decimateM)));
        }
#endregion

#region analysis scripting
        private const string DetectionScript =
@"# original script
{0}

# trimming
{1}

# load plugin
{2}

# color format change
{4}

# detection script
global unused_ = blankclip(pixel_type=""yv12"", length=10).TFM()
file=""{3}""
global sep=""-""
function IsMoving() {{
  global b = (diff < 1.0) ? false : true
}}
c = last
global clip = last
c = WriteFile(c, file, ""a"", ""sep"", ""b"")
c = FrameEvaluate(c, ""global a = IsCombedTIVTC(clip, cthresh=9)"")
c = FrameEvaluate(c, ""IsMoving"")
c = FrameEvaluate(c,""global diff = 0.50*YDifferenceFromPrevious(clip) + 0.25*UDifferenceFromPrevious(clip) + 0.25*VDifferenceFromPrevious(clip)"")

# crop
crop(c,0,0,16,16)

# select range
{5}";

        private const string FieldOrderScript =
@"# original script
{0}

# trimming
{1}

# color format change
{3}

# detection script
file=""{2}""
global sep=""-""
d = last
global abff = d.assumebff().separatefields()
global atff = d.assumetff().separatefields()
c = d.loop(2)
c = WriteFile(c, file, ""diffa"", ""sep"", ""diffb"")
c = FrameEvaluate(c,""global diffa = 0.50*YDifferenceFromPrevious(abff) + 0.25*UDifferenceFromPrevious(abff) + 0.25*VDifferenceFromPrevious(abff)"")
c = FrameEvaluate(c,""global diffb = 0.50*YDifferenceFromPrevious(atff) + 0.25*UDifferenceFromPrevious(atff) + 0.25*VDifferenceFromPrevious(atff)"")

# crop
crop(c,0,0,16,16)

# select range
{4}";

        public static string GetDetectionScript(int scriptType, string originalScript, string trimLine, string logFileName, int selectEvery, int selectLength)
        {
            string strPlugin = "LoadPlugin(\"" + Path.Combine(MainForm.Instance.Settings.AvisynthPluginsPath, "TIVTC.dll") + "\")";
            string strConvertColorFormat = "ConvertToYV12()";
            if (MainForm.Instance.Settings.AviSynthPlus)
                strConvertColorFormat = "ConvertBits(8)\r\n" + strConvertColorFormat;
            string strSelectRange = (selectEvery > selectLength) ? "SelectRangeEvery(" + selectEvery + "," + selectLength + ",0)" : "#process all";
            
            if (scriptType == 0) // detection
                return string.Format(DetectionScript, originalScript, trimLine, strPlugin, logFileName, strConvertColorFormat, strSelectRange);
            else if (scriptType == 1) // field order
                return string.Format(FieldOrderScript, originalScript, trimLine, logFileName, strConvertColorFormat, strSelectRange);
            else
                return null;
        }
#endregion

        public static void undercrop(ref CropValues crop, modValue mValue)
        {
            int mod = MeGUI.core.util.Resolution.GetModValue(mValue, mod16Method.undercrop, true);

            if (crop.left % 2 != 0 && crop.top % 2 != 0 && crop.bottom % 2 != 0 && crop.right % 2 != 0)
                throw new Exception("Cropping by odd numbers not supported in undercropping to mod" + mod);

            while ((crop.left + crop.right) % mod > 0)
            {
                if (crop.left > crop.right)
                {
                    if (crop.left > 1)
                        crop.left -= 2;
                    else
                        crop.left = 0;
                }
                else
                {
                    if (crop.right > 1)
                        crop.right -= 2;
                    else
                        crop.right = 0;
                }
            }
            while ((crop.top + crop.bottom) % mod > 0)
            {
                if (crop.top > crop.bottom)
                {
                    if (crop.top > 1)
                        crop.top -= 2;
                    else
                        crop.top = 0;
                }
                else
                {
                    if (crop.bottom > 1)
                        crop.bottom -= 2;
                    else
                        crop.bottom = 0;
                }
            }
        }

        public static void overcrop(ref CropValues crop, modValue mValue)
        {
            int mod = MeGUI.core.util.Resolution.GetModValue(mValue, mod16Method.overcrop, true);

            if (crop.left % 2 != 0 && crop.top % 2 != 0 && crop.bottom % 2 != 0 && crop.right % 2 != 0)
                throw new Exception("Cropping by odd numbers not supported in overcropping to mod" + mod);

            bool doLeftNext = true;
            while ((crop.left + crop.right) % mod != 0)
            {
                if (doLeftNext)
                    crop.left += 2;
                else
                    crop.right += 2;
                doLeftNext = !doLeftNext;
            }

            bool doTopNext = true;
            while ((crop.top + crop.bottom) % mod != 0)
            {
                if (doTopNext)
                    crop.top += 2;
                else
                    crop.bottom += 2;
                doTopNext = !doTopNext;
            }
        }

        public static void cropMod4Horizontal(ref CropValues crop)
        {
            if (crop.left % 2 != 0 && crop.top % 2 != 0 && crop.bottom % 2 != 0 && crop.right % 2 != 0)
                throw new Exception("Cropping by odd numbers not supported in mod4 horizontal cropping");
            while ((crop.left + crop.right) % 4 > 0)
            {
                if (crop.left > crop.right)
                {
                    if (crop.left > 1)
                    {
                        crop.left -= 2;
                    }
                    else
                    {
                        crop.left = 0;
                    }
                }
                else
                {
                    if (crop.right > 1)
                    {
                        crop.right -= 2;
                    }
                    else
                    {
                        crop.right = 0;
                    }
                }
            }
        }
    }
}