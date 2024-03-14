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
using System.Xml.Serialization;

namespace MeGUI
{
    #region video and audio codecs
    /// <summary>
    /// Dummy interface to avoid some runtime type errors. This should be implemented by VideoCodec and AudioCodec
    /// </summary>
    public interface ICodec { }
    
    public class VideoCodec : ICodec, IIDable
    {
        public VideoCodec(string id, string mediaInfoRegex)
        {
            this.id = id;
            this.mediaInfoRegex = mediaInfoRegex;
        }

        private string id;
        public string ID
        {
            get { return id; }
        }

        private string mediaInfoRegex;
        public string MediaInfoRegex
        {
            get { return mediaInfoRegex; }
            set { mediaInfoRegex = value; }
        }

        public static readonly VideoCodec ASP     = new VideoCodec("ASP", "mpeg-4 visual");
        public static readonly VideoCodec AVC     = new VideoCodec("AVC", "avc");
        public static readonly VideoCodec HEVC    = new VideoCodec("HEVC", "hevc");
        public static readonly VideoCodec HFYU    = new VideoCodec("HFYU", "ffvh");
        public static readonly VideoCodec MPEG1   = new VideoCodec("MPEG1", "(mpeg-1 video)|(mpeg video version 1)");
        public static readonly VideoCodec MPEG2   = new VideoCodec("MPEG2", "(mpeg-2 video)|(mpeg video version 2)");
        public static readonly VideoCodec VC1     = new VideoCodec("VC1", "vc-1");
        public static readonly VideoCodec VP8     = new VideoCodec("VP8", "VP8");
        public static readonly VideoCodec VP9     = new VideoCodec("VP9", "VP9");
        public static readonly VideoCodec UNKNOWN = new VideoCodec("UNKNOWN", ".*");
    }

    public class AudioCodec : ICodec, IIDable
    {
        // needed for serilaization only
        public AudioCodec() : this("", "") { }

        public AudioCodec(string id, string mediaInfoRegex)
        {
            this.id = id;
            this.mediaInfoRegex = mediaInfoRegex;
        }

        private string id;
        public string ID
        {
            get { return id; }
            set { id = value; }
        }

        private string mediaInfoRegex;
        public string MediaInfoRegex
        {
            get { return mediaInfoRegex; }
            set { mediaInfoRegex = value; }
        }
 
        public static readonly AudioCodec AAC     = new AudioCodec("AAC", "^AAC$");
        public static readonly AudioCodec AC3     = new AudioCodec("AC-3", "^AC-3$");
        public static readonly AudioCodec AVS     = new AudioCodec("AVS", "AVS$");
        public static readonly AudioCodec DTS     = new AudioCodec("DTS", "^DTS$");
        public static readonly AudioCodec EAC3    = new AudioCodec("EAC3", "^E-AC-3$");
        public static readonly AudioCodec FLAC    = new AudioCodec("FLAC", "^FLAC$");
        public static readonly AudioCodec MP2     = new AudioCodec("MP2", "^MP2$");
        public static readonly AudioCodec MP3     = new AudioCodec("MP3", "^MP3$");
        public static readonly AudioCodec OPUS    = new AudioCodec("OPUS", "^Opus$");
        public static readonly AudioCodec PCM     = new AudioCodec("PCM", "^PCM$");
        public static readonly AudioCodec THD     = new AudioCodec("THD", "^(?=.*TrueHD)(?!.*AC-3).*");
        public static readonly AudioCodec THDAC3  = new AudioCodec("THDAC3", "^(?=.*TrueHD)(?=.*AC-3).*");
        public static readonly AudioCodec VORBIS  = new AudioCodec("VORBIS", "^Vorbis$");
        public static readonly AudioCodec UNKNOWN = new AudioCodec("UNKNOWN", ".*");
    }

    public class SubtitleCodec : ICodec, IIDable
    {
        // needed for serilaization only
        public SubtitleCodec() : this("", "") { }

        public SubtitleCodec(string id, string mediaInfoRegex)
        {
            this.id = id;
            this.mediaInfoRegex = mediaInfoRegex;
        }

        private string id;
        public string ID
        {
            get { return id; }
        }

        private string mediaInfoRegex;
        public string MediaInfoRegex
        {
            get { return mediaInfoRegex; }
            set { mediaInfoRegex = value; }
        }
        
        public static readonly SubtitleCodec ASS        = new SubtitleCodec("ASS", "ass");
        public static readonly SubtitleCodec PGS        = new SubtitleCodec("PGS", "pgs");
        public static readonly SubtitleCodec SRT        = new SubtitleCodec("SRT", "(text)|(utf-8)|(subrip)");
        public static readonly SubtitleCodec SSA        = new SubtitleCodec("SSA", "ssa");
        public static readonly SubtitleCodec TTXT       = new SubtitleCodec("TTXT", "ttxt");
        public static readonly SubtitleCodec VOBSUB     = new SubtitleCodec("VOBSUB", "(vobsub)|(rle)");
        public static readonly SubtitleCodec UNKNOWN    = new SubtitleCodec("UNKNOWN", ".*");
    }
    #endregion
    #region video and audio encoder types
    /// <summary>
    /// Dummy interface so runtime typing problems don't arise, and we can avoid ugly (object) casts
    /// </summary>
    public interface IEncoderType
    {
        ICodec Codec
        {
            get;
        }
    }

    public class VideoEncoderType : IEncoderType, IIDable
    {
        private string id;
        private VideoCodec codec;
        public VideoCodec VCodec
        {
            get { return codec; }
        }
        public ICodec Codec
        {
            get { return VCodec; }
        }

        public string ID
        {
            get { return id; }
        }
        public VideoEncoderType(string id, VideoCodec codec)
        {
            this.id = id;
            this.codec = codec;
        }

        public override string ToString()
        {
            return ID;
        }
        public static readonly VideoEncoderType HFYU = new VideoEncoderType("Huffyuv", VideoCodec.HFYU);
        public static readonly VideoEncoderType X264 = new VideoEncoderType("x264", VideoCodec.AVC);
        public static readonly VideoEncoderType X265 = new VideoEncoderType("x265", VideoCodec.HEVC);
        public static readonly VideoEncoderType XVID = new VideoEncoderType("Xvid", VideoCodec.ASP);
    }
    public class AudioEncoderType : IEncoderType, IIDable
    {
        private string id;
        private AudioCodec codec;
        public ICodec Codec
        {
            get { return ACodec; }
        }
        public AudioCodec ACodec
        {
            get { return codec; }
        }
        public string ID
        {
            get { return id; }
        }
        public AudioEncoderType(string id, AudioCodec codec)
        {
            this.id = id;
            this.codec = codec;
        }
        public static readonly AudioEncoderType LAME   = new AudioEncoderType("LAME",       AudioCodec.MP3);
        public static readonly AudioEncoderType NAAC   = new AudioEncoderType("NAAC",       AudioCodec.AAC);
        public static readonly AudioEncoderType VORBIS = new AudioEncoderType("VORBIS",     AudioCodec.VORBIS);
        public static readonly AudioEncoderType FFAC3  = new AudioEncoderType("FFAC3",      AudioCodec.AC3);
        public static readonly AudioEncoderType FFMP2  = new AudioEncoderType("FFMP2",      AudioCodec.MP2);
        public static readonly AudioEncoderType FLAC   = new AudioEncoderType("FLAC",       AudioCodec.FLAC);
        public static readonly AudioEncoderType QAAC   = new AudioEncoderType("QAAC",       AudioCodec.AAC);
        public static readonly AudioEncoderType OPUS   = new AudioEncoderType("OPUS",       AudioCodec.OPUS);
        public static readonly AudioEncoderType FDKAAC = new AudioEncoderType("FDK-AAC",    AudioCodec.AAC); 
        public static readonly AudioEncoderType FFAAC  = new AudioEncoderType("FFAAC",      AudioCodec.AAC);

    }
    #endregion
    
    class CodecManager
    {
        public static GenericRegisterer<VideoCodec> VideoCodecs = new GenericRegisterer<VideoCodec>();
        public static GenericRegisterer<AudioCodec> AudioCodecs = new GenericRegisterer<AudioCodec>();
        public static GenericRegisterer<SubtitleCodec> SubtitleCodecs = new GenericRegisterer<SubtitleCodec>();
        public static GenericRegisterer<VideoEncoderType> VideoEncoderTypes = new GenericRegisterer<VideoEncoderType>();
        public static GenericRegisterer<AudioEncoderType> AudioEncoderTypes = new GenericRegisterer<AudioEncoderType>();
        
        static CodecManager()
        {
            if (!(
                VideoCodecs.Register(VideoCodec.ASP)    &&
                VideoCodecs.Register(VideoCodec.AVC)    &&
                VideoCodecs.Register(VideoCodec.HEVC)   &&
                VideoCodecs.Register(VideoCodec.HFYU)   &&
                VideoCodecs.Register(VideoCodec.MPEG1)  &&
                VideoCodecs.Register(VideoCodec.MPEG2)  &&
                VideoCodecs.Register(VideoCodec.VC1)    &&
                VideoCodecs.Register(VideoCodec.VP8)    &&
                VideoCodecs.Register(VideoCodec.VP9)    &&
                VideoCodecs.Register(VideoCodec.UNKNOWN) // must be the last line to serve as catch-all
                ))
                throw new Exception("Failed to register a standard video codec");
            if (!(
                AudioCodecs.Register(AudioCodec.AAC)    &&
                AudioCodecs.Register(AudioCodec.AC3)    &&
                AudioCodecs.Register(AudioCodec.AVS)    &&
                AudioCodecs.Register(AudioCodec.DTS)    &&
                AudioCodecs.Register(AudioCodec.EAC3)   &&
                AudioCodecs.Register(AudioCodec.FLAC)   &&
                AudioCodecs.Register(AudioCodec.MP2)    &&
                AudioCodecs.Register(AudioCodec.MP3)    &&
                AudioCodecs.Register(AudioCodec.OPUS)   &&
                AudioCodecs.Register(AudioCodec.PCM)    &&
                AudioCodecs.Register(AudioCodec.THD)    &&
                AudioCodecs.Register(AudioCodec.THDAC3) &&
                AudioCodecs.Register(AudioCodec.VORBIS) &&
                AudioCodecs.Register(AudioCodec.UNKNOWN) // must be the last line to serve as catch-all
                ))
                throw new Exception("Failed to register a standard audio codec");
            if (!(
                SubtitleCodecs.Register(SubtitleCodec.ASS) &&
                SubtitleCodecs.Register(SubtitleCodec.PGS) &&
                SubtitleCodecs.Register(SubtitleCodec.SRT) &&
                SubtitleCodecs.Register(SubtitleCodec.SSA) &&
                SubtitleCodecs.Register(SubtitleCodec.TTXT) &&
                SubtitleCodecs.Register(SubtitleCodec.VOBSUB) &&
                SubtitleCodecs.Register(SubtitleCodec.UNKNOWN) // must be the last line to serve as catch-all
                ))
                throw new Exception("Failed to register a standard video codec");
            if (!(
                VideoEncoderTypes.Register(VideoEncoderType.HFYU) &&
                VideoEncoderTypes.Register(VideoEncoderType.X264) &&
                VideoEncoderTypes.Register(VideoEncoderType.X265) &&
                VideoEncoderTypes.Register(VideoEncoderType.XVID)))
                throw new Exception("Failed to register a standard video encoder type");
            if (!(
                AudioEncoderTypes.Register(AudioEncoderType.FFAC3)  &&
                AudioEncoderTypes.Register(AudioEncoderType.FFMP2)  &&
                AudioEncoderTypes.Register(AudioEncoderType.LAME)   &&
                AudioEncoderTypes.Register(AudioEncoderType.NAAC)   &&
                AudioEncoderTypes.Register(AudioEncoderType.VORBIS) &&
                AudioEncoderTypes.Register(AudioEncoderType.FLAC)   &&
                AudioEncoderTypes.Register(AudioEncoderType.QAAC)   &&
                AudioEncoderTypes.Register(AudioEncoderType.FDKAAC) &&
                AudioEncoderTypes.Register(AudioEncoderType.FFAAC) &&
                AudioEncoderTypes.Register(AudioEncoderType.OPUS)))
                throw new Exception("Failed to register a standard audio encoder type");
                
        }
    }
    #region Video/Audio/Subtitle Types
    public class VideoType : OutputType
    {
        private VideoCodec[] supportedCodecs;

        public VideoCodec[] SupportedCodecs
        {
            get { return supportedCodecs; }
        }

        public VideoType(string name, string filterName, string extension, ContainerType containerType, VideoCodec supportedCodec)
            : this(name, filterName, extension, containerType, new VideoCodec[] { supportedCodec }) { }

        public VideoType(string name, string filterName, string extension, ContainerType containerType, VideoCodec[] supportedCodecs)
            : base(name, filterName, extension, containerType) {
                this.supportedCodecs = supportedCodecs;
        }
        public static readonly VideoType AVI      = new VideoType("AVI", "AVI Files", "avi", ContainerType.AVI, new VideoCodec[] { VideoCodec.ASP, VideoCodec.AVC, VideoCodec.HFYU });
        public static readonly VideoType M2TS     = new VideoType("M2TS", "M2TS Files", "m2ts", ContainerType.M2TS, new VideoCodec[] { VideoCodec.AVC, VideoCodec.HEVC, VideoCodec.MPEG2, VideoCodec.VC1 });
        public static readonly VideoType MKV      = new VideoType("MKV", "Matroska Files", "mkv", ContainerType.MKV, new VideoCodec[] { VideoCodec.ASP, VideoCodec.AVC, VideoCodec.HFYU, VideoCodec.MPEG1, VideoCodec.MPEG2, VideoCodec.VC1 });
        public static readonly VideoType MP4      = new VideoType("MP4", "MP4 Files", "mp4", ContainerType.MP4, new VideoCodec[] { VideoCodec.ASP, VideoCodec.AVC, VideoCodec.HEVC, VideoCodec.MPEG1, VideoCodec.MPEG2 });
        public static readonly VideoType MPEG1    = new VideoType("MPEG1", "MPEG-1 Files", "m1v", null, VideoCodec.MPEG1);
        public static readonly VideoType MPEG2    = new VideoType("MPEG2", "MPEG-2 Files", "m2v", null, VideoCodec.MPEG2);
        public static readonly VideoType RAWASP   = new VideoType("RAWASP", "RAW MPEG-4 ASP Files", "m4v", null, VideoCodec.ASP);
        public static readonly VideoType RAWAVC   = new VideoType("RAWAVC", "RAW MPEG-4 AVC Files", "264", null, VideoCodec.AVC);
        public static readonly VideoType RAWAVC2  = new VideoType("RAWAVC", "RAW MPEG-4 AVC Files", "h264", null, VideoCodec.AVC);
        public static readonly VideoType RAWHEVC2 = new VideoType("RAWHEVC", "RAW MPEG-H HEVC Files", "h265", null, VideoCodec.HEVC);
        public static readonly VideoType RAWHEVC  = new VideoType("RAWHEVC", "RAW MPEG-H HEVC Files", "hevc", null, VideoCodec.HEVC);
        public static readonly VideoType VC1      = new VideoType("VC1", "VC-1 Files", "vc1", null, VideoCodec.VC1);
        public static readonly VideoType VP8      = new VideoType("VP8", "VP8 Files", "ivf", null, VideoCodec.VP8);
        public static readonly VideoType VP9      = new VideoType("VP9", "VP9 Files", "ivf", null, VideoCodec.VP9);
    }

    public class AudioType : OutputType
    {
        // needed only for serialization
        public AudioType() : base("", "", "", null) { }

        public AudioType(string name, string filterName, string extension, ContainerType containerType, AudioCodec supportedCodec)
            : this(name, filterName, extension, containerType, new AudioCodec[] { supportedCodec }) { }

        public AudioType(string name, string filterName, string extension, ContainerType containerType, AudioCodec[] supportedCodecs)
            : base(name, filterName, extension, containerType) {
                this.supportedCodecs = supportedCodecs;
        }

        private AudioCodec[] supportedCodecs;
        public AudioCodec[] SupportedCodecs
        {
            get { return supportedCodecs; }
            set { supportedCodecs = value; }
        }

        public static readonly AudioType MP4AAC = new AudioType("MP4-AAC", "MP4 AAC Files", "mp4", ContainerType.MP4, AudioCodec.AAC);
        public static readonly AudioType M4A    = new AudioType("M4A", "MP4 Audio Files", "m4a", ContainerType.MP4, AudioCodec.AAC);
        public static readonly AudioType RAWAAC = new AudioType("Raw-AAC", "RAW AAC Files", "aac", null, AudioCodec.AAC);
        public static readonly AudioType MP3    = new AudioType("MP3", "MP3 Files", "mp3", null, AudioCodec.MP3);
        public static readonly AudioType VORBIS = new AudioType("Ogg", "Ogg Vorbis Files", "ogg", null, AudioCodec.VORBIS);
        public static readonly AudioType AC3    = new AudioType("AC3", "AC3 Files", "ac3", null, AudioCodec.AC3);
        public static readonly AudioType MP2    = new AudioType("MP2", "MP2 Files", "mp2", null, AudioCodec.MP2);
        public static readonly AudioType DTS    = new AudioType("DTS", "DTS Files", "dts", null, AudioCodec.DTS);
        public static readonly AudioType WAV    = new AudioType("WAV", "WAV Files", "wav", null, AudioCodec.PCM);
        public static readonly AudioType W64    = new AudioType("W64", "W64 Files", "w64", null, AudioCodec.PCM);
        public static readonly AudioType PCM    = new AudioType("PCM", "PCM Files", "pcm", null, AudioCodec.PCM);
        public static readonly AudioType EAC3   = new AudioType("EAC3", "EAC3 Files", "eac3", null, AudioCodec.EAC3);
        public static readonly AudioType THD    = new AudioType("THD", "TrueHD Files", "thd", null, AudioCodec.THD);
        public static readonly AudioType THDAC3 = new AudioType("THDAC3", "TrueHD+AC3 Files", "thd+ac3", null, AudioCodec.THDAC3);
        public static readonly AudioType FLAC   = new AudioType("FLAC", "FLAC Audio Lossless Files", "flac", null, AudioCodec.FLAC);
        public static readonly AudioType AVS    = new AudioType("AVS", "AviSynth Script Files", "avs", null, AudioCodec.PCM);
        public static readonly AudioType OPUS   = new AudioType("OPUS", "Opus Audio Files", "opus", null, AudioCodec.OPUS);
    }

    public class SubtitleType : OutputType
    {
        public SubtitleType(string name, string filterName, string extension, ContainerType containerType)
            : base(name, filterName, extension, containerType) { }
        public static readonly SubtitleType SSA    = new SubtitleType("SubStationAlpha", "SubStation Alpha Subtitle Files", "ssa", null);
        public static readonly SubtitleType ASS    = new SubtitleType("Advanced SubStationAlpha", "Advanced SubStation Alpha Subtitle Files", "ass", null);
        public static readonly SubtitleType SUBRIP = new SubtitleType("Subrip", "Subrip Subtitle Files", "srt", null);
        public static readonly SubtitleType BDSUP  = new SubtitleType("BDSup", "Blu-ray Sup Subtitle Files", "sup", null); 
        public static readonly SubtitleType VOBSUB = new SubtitleType("Vobsub", "Vobsub Subtitle Files", "idx", null);
        public static readonly SubtitleType TTXT   = new SubtitleType("TTXT", "Time Text Subtitles Files", "ttxt", null);
    }

    public class ChapterType : OutputType
    {
        public ChapterType(string name, string filterName, string extension, ContainerType containerType)
            : base(name, filterName, extension, containerType) { }
        public static readonly ChapterType OGG_TXT = new ChapterType("Ogg Chapter", "Ogg Chapter Files", "txt", null);
        public static readonly ChapterType MKV_XML = new ChapterType("Matroska Chapter", "Matroska Chapter Files", "xml", null);
    }

    public class DeviceType : OutputType
    {
        public DeviceType(string name, string filterName, string extension, ContainerType containerType)
            : base(name, filterName, extension, containerType) { }
        public static readonly DeviceType APPLETV = new DeviceType("Apple TV", "Apple TV", "Apple TV", ContainerType.MP4);
        public static readonly DeviceType IPAD    = new DeviceType("iPad", "iPad", "iPad", ContainerType.MP4);
        public static readonly DeviceType IPOD    = new DeviceType("iPod", "iPod", "iPod", ContainerType.MP4);
        public static readonly DeviceType IPHONE  = new DeviceType("iPhone", "iPhone", "iPhone", ContainerType.MP4);
        public static readonly DeviceType ISMA    = new DeviceType("ISMA", "ISMA", "ISMA", ContainerType.MP4);
        public static readonly DeviceType PSP     = new DeviceType("PSP", "PSP", "PSP", ContainerType.MP4);
        public static readonly DeviceType BD      = new DeviceType("Blu-ray", "Blu-ray", "Blu-ray", ContainerType.M2TS);
        public static readonly DeviceType AVCHD   = new DeviceType("AVCHD", "AVCHD", "AVCHD", ContainerType.M2TS);
        public static readonly DeviceType PC      = new DeviceType("PC", "PC", "PC", ContainerType.AVI);
    }

    public class ContainerType : OutputFileType
    {
        // needed for the serialization only
        public ContainerType() : base("", "", "") { }

        public ContainerType(string name, string filterName, string extension, string mediaInfoRegEx)
            : base(name, filterName, extension)
        {
            this.mediaInfoRegex = mediaInfoRegEx;
        }

        private string mediaInfoRegex;
        public string MediaInfoRegex
        {
            get { return mediaInfoRegex; }
            set { mediaInfoRegex = value; }
        }

        public static readonly ContainerType MP4        = new ContainerType("MP4", "MP4 Files", "mp4", "(mpeg-4)|(3gpp)");
        public static readonly ContainerType MKV        = new ContainerType("MKV", "Matroska Files", "mkv", "matroska");
        public static readonly ContainerType AVI        = new ContainerType("AVI", "AVI Files", "avi", "avi");
        public static readonly ContainerType M2TS       = new ContainerType("M2TS", "M2TS Files", "m2ts", "(bdav)|(mpeg-ts)");
        public static readonly ContainerType[] Containers = new ContainerType[] { MP4, MKV, AVI, M2TS };

        public static ContainerType ByName(string id)
        {
            foreach (ContainerType t in Containers)
                if (t.ID == id)
                    return t;
            return null;
        }
    }
    #endregion

    public class ContainerManager
    {
        public static GenericRegisterer<VideoType> VideoTypes = new GenericRegisterer<VideoType>();
        public static GenericRegisterer<AudioType> AudioTypes = new GenericRegisterer<AudioType>();
        public static GenericRegisterer<SubtitleType> SubtitleTypes = new GenericRegisterer<SubtitleType>();
        public static GenericRegisterer<ContainerType> ContainerTypes = new GenericRegisterer<ContainerType>();
        public static GenericRegisterer<ChapterType> ChapterTypes = new GenericRegisterer<ChapterType>();
        public static GenericRegisterer<DeviceType> DeviceTypes = new GenericRegisterer<DeviceType>();

        static ContainerManager()
        {
            if (!(
                VideoTypes.Register(VideoType.AVI)     &&
                VideoTypes.Register(VideoType.MKV)     &&
                VideoTypes.Register(VideoType.MP4)     &&
                VideoTypes.Register(VideoType.RAWASP)  &&
                VideoTypes.Register(VideoType.RAWAVC)  &&
                VideoTypes.Register(VideoType.RAWHEVC) &&
                VideoTypes.Register(VideoType.MPEG2)   &&
                VideoTypes.Register(VideoType.VC1)     &&
                VideoTypes.Register(VideoType.VP8)     &&
                VideoTypes.Register(VideoType.VP9)     &&
                VideoTypes.Register(VideoType.M2TS)))
                throw new Exception("Failed to register a video type");
            if (!(
                AudioTypes.Register(AudioType.AC3)    &&
                AudioTypes.Register(AudioType.MP3)    &&
                AudioTypes.Register(AudioType.DTS)    &&
                AudioTypes.Register(AudioType.WAV)    &&
                AudioTypes.Register(AudioType.W64)    &&
                AudioTypes.Register(AudioType.PCM)    &&
                AudioTypes.Register(AudioType.MP2)    &&
                AudioTypes.Register(AudioType.MP4AAC) &&
                AudioTypes.Register(AudioType.M4A)    &&
                AudioTypes.Register(AudioType.RAWAAC) &&
                AudioTypes.Register(AudioType.VORBIS) &&
                AudioTypes.Register(AudioType.EAC3)   &&
                AudioTypes.Register(AudioType.FLAC)   &&
                AudioTypes.Register(AudioType.AVS)    &&
                AudioTypes.Register(AudioType.OPUS)   &&
                AudioTypes.Register(AudioType.THD)    &&
                AudioTypes.Register(AudioType.THDAC3)))
                throw new Exception("Failed to register an audio type");
            if (!(
                SubtitleTypes.Register(SubtitleType.ASS)    &&
                SubtitleTypes.Register(SubtitleType.SSA)    &&
                SubtitleTypes.Register(SubtitleType.SUBRIP) &&
                SubtitleTypes.Register(SubtitleType.BDSUP)  &&
                SubtitleTypes.Register(SubtitleType.VOBSUB) &&
                SubtitleTypes.Register(SubtitleType.TTXT)))
                throw new Exception("Failed to register a subtitle type");
            if (!(
                ContainerTypes.Register(ContainerType.AVI) &&
                ContainerTypes.Register(ContainerType.MKV) &&
                ContainerTypes.Register(ContainerType.MP4) &&
                ContainerTypes.Register(ContainerType.M2TS)))
                throw new Exception("Failed to register a container type");
            if (!(
	            ChapterTypes.Register(ChapterType.OGG_TXT) &&
                ChapterTypes.Register(ChapterType.MKV_XML)))
		        throw new Exception("Failed to register a chapter type");
            if (!(
                DeviceTypes.Register(DeviceType.AVCHD) &&
                DeviceTypes.Register(DeviceType.BD) &&
                DeviceTypes.Register(DeviceType.PC) &&
                DeviceTypes.Register(DeviceType.IPOD) &&
                DeviceTypes.Register(DeviceType.PSP) &&
                DeviceTypes.Register(DeviceType.IPHONE) &&
                DeviceTypes.Register(DeviceType.APPLETV) &&
                DeviceTypes.Register(DeviceType.IPAD) &&
                DeviceTypes.Register(DeviceType.ISMA)))
                throw new Exception("Failed to register a device type");
        }
    }

    public class OutputFileType : IIDable
    {
        // needed for serialization only
        public OutputFileType () : this ("", "", "") { }

        public OutputFileType(string name, string filterName, string extension)
        {
            this.name = name;
            this.filterName = filterName;
            this.extension = extension;
        }

        private string name;
        public string ID
        {
            get { return name; }
            set { name = value; }
        }

        private string filterName;
        public string FilterName
        {
            get { return filterName; }
            set { filterName = value; }
        }

        /// <summary>
        /// used to display the output type in dropdowns
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.name;
        }

        [XmlIgnore]
        public string OutputFilter
        {
            get { return "*." + extension; }
        }

        /// <summary>
        /// gets a valid filter string for file dialogs based on the known extension
        /// </summary>
        /// <returns></returns>
        [XmlIgnore]
        public string OutputFilterString
        {
            get {return filterName + " (*." + extension + ")|*." + extension;}
        }

        private string extension;
        /// <summary>
        /// gets the extension for this file type
        /// </summary>
        public string Extension
        {
            get {return this.extension;}
            set { this.extension = value; }
        }
    }

    public class OutputType : OutputFileType
    {
        // needed for serialization only
        public OutputType() : base("", "", "") { }

        public OutputType(string name, string filterName, string extension, ContainerType containerType)
            : base(name, filterName, extension)
        {
            this.containerType = containerType;
        }

        private ContainerType containerType;
        public ContainerType ContainerType
        {
            get { return this.containerType; }
            set { this.containerType = value; }
        }
    }
}