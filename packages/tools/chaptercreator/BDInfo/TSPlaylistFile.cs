//============================================================================
// BDInfo - Blu-ray Video and Audio Analysis Tool
// Copyright © 2010 Cinema Squid
//
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2.1 of the License, or (at your option) any later version.
//
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
//=============================================================================

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace BDInfo
{
    public class TSPlaylistFile
    {
        private FileInfo FileInfo = null;
        public string FileType = null;
        public bool IsInitialized = false;
        public string Name = null;
        public bool HasHiddenTracks = false;
        public bool HasLoops = false;
        public bool IsCustom = false;

        public List<double> Chapters = new List<double>();

        public Dictionary<ushort, TSStream> Streams =
            new Dictionary<ushort, TSStream>();
        public Dictionary<ushort, TSStream> PlaylistStreams =
            new Dictionary<ushort, TSStream>();
        public List<TSStreamClip> StreamClips =
            new List<TSStreamClip>();
        public List<Dictionary<ushort, TSStream>> AngleStreams =
            new List<Dictionary<ushort, TSStream>>();
        public List<Dictionary<double, TSStreamClip>> AngleClips =
            new List<Dictionary<double, TSStreamClip>>();
        public int AngleCount = 0;

        public TSPlaylistFile(
            FileInfo fileInfo)
        {
            FileInfo = fileInfo;
            Name = fileInfo.Name.ToUpperInvariant();
        }

        public override string ToString()
        {
            return Name;
        }

        public double TotalLength
        {
            get
            {
                double length = 0;
                foreach (TSStreamClip clip in StreamClips)
                {
                    if (clip.AngleIndex == 0)
                    {
                        length += clip.Length;
                    }
                }
                return length;
            }
        }

        public void Scan(
            Dictionary<string, TSStreamFile> streamFiles,
            Dictionary<string, TSStreamClipFile> streamClipFiles)
        {
            FileStream fileStream = null;
            BinaryReader fileReader = null;

            try
            {
                Streams.Clear();
                StreamClips.Clear();

                fileStream = File.OpenRead(FileInfo.FullName);
                fileReader = new BinaryReader(fileStream);

                byte[] data = new byte[fileStream.Length];
                int dataLength = fileReader.Read(data, 0, data.Length);

                int pos = 0;

                FileType = ReadString(data, 8, ref pos);
                if (FileType != "MPLS0100" && FileType != "MPLS0200" && FileType != "MPLS0300")
                {
                    System.Windows.Forms.MessageBox.Show(string.Format(
                        "Playlist {0} has an unknown file type {1}.",
                        FileInfo.Name, FileType));
                    return;
                }

                int playlistOffset = ReadInt32(data, ref pos);
                int chaptersOffset = ReadInt32(data, ref pos);
                int extensionsOffset = ReadInt32(data, ref pos);

                pos = playlistOffset;

                int playlistLength = ReadInt32(data, ref pos);
                int playlistReserved = ReadInt16(data, ref pos);
                int itemCount = ReadInt16(data, ref pos);
                int subitemCount = ReadInt16(data, ref pos);

                List<TSStreamClip> chapterClips = new List<TSStreamClip>();
                for (int itemIndex = 0; itemIndex < itemCount; itemIndex++)
                {
                    int itemStart = pos;
                    int itemLength = ReadInt16(data, ref pos);
                    string itemName = ReadString(data, 5, ref pos);
                    string itemType = ReadString(data, 4, ref pos);

                    TSStreamFile streamFile = null;
                    string streamFileName = string.Format(
                        "{0}.M2TS", itemName);
                    if (streamFiles.ContainsKey(streamFileName))
                    {
                        streamFile = streamFiles[streamFileName];
                    }
                    if (streamFile == null)
                    {
                        Debug.WriteLine(string.Format(
                            "Playlist {0} referenced missing file {1}.",
                            FileInfo.Name, streamFileName));
                    }

                    TSStreamClipFile streamClipFile = null;
                    string streamClipFileName = string.Format(
                        "{0}.CLPI", itemName);
                    if (streamClipFiles.ContainsKey(streamClipFileName))
                    {
                        streamClipFile = streamClipFiles[streamClipFileName];
                    }
                    if (streamClipFile == null)
                    {
                        System.Windows.Forms.MessageBox.Show(string.Format(
                            "Playlist {0} referenced missing file {1}.",
                            FileInfo.Name, streamFileName));
                        return;
                    }

                    pos += 1;
                    int multiangle = (data[pos] >> 4) & 0x01;
                    int condition = data[pos] & 0x0F;
                    pos += 2;

                    int inTime = ReadInt32(data, ref pos);
                    if (inTime < 0) inTime &= 0x7FFFFFFF;
                    double timeIn = (double)inTime / 45000;

                    int outTime = ReadInt32(data, ref pos);
                    if (outTime < 0) outTime &= 0x7FFFFFFF;
                    double timeOut = (double)outTime / 45000;

                    TSStreamClip streamClip = new TSStreamClip(streamClipFile);

                    streamClip.Name = streamFileName; //TODO
                    streamClip.TimeIn = timeIn;
                    streamClip.TimeOut = timeOut;
                    streamClip.Length = streamClip.TimeOut - streamClip.TimeIn;
                    streamClip.RelativeTimeIn = TotalLength;
                    streamClip.RelativeTimeOut = streamClip.RelativeTimeIn + streamClip.Length;
                    StreamClips.Add(streamClip);
                    chapterClips.Add(streamClip);

                    pos += 12;
                    if (multiangle > 0)
                    {
                        int angles = data[pos];
                        pos += 2;
                        for (int angle = 0; angle < angles - 1; angle++)
                        {
                            string angleName = ReadString(data, 5, ref pos);
                            string angleType = ReadString(data, 4, ref pos);
                            pos += 1;

                            TSStreamFile angleFile = null;
                            string angleFileName = string.Format(
                                "{0}.M2TS", angleName);
                            if (streamFiles.ContainsKey(angleFileName))
                            {
                                angleFile = streamFiles[angleFileName];
                            }
                            if (angleFile == null)
                            {
                                System.Windows.Forms.MessageBox.Show(string.Format(
                                    "Playlist {0} referenced missing angle file {1}.",
                                    FileInfo.Name, angleFileName));
                                return;
                            }

                            TSStreamClipFile angleClipFile = null;
                            string angleClipFileName = string.Format(
                                "{0}.CLPI", angleName);
                            if (streamClipFiles.ContainsKey(angleClipFileName))
                            {
                                angleClipFile = streamClipFiles[angleClipFileName];
                            }
                            if (angleClipFile == null)
                            {
                                System.Windows.Forms.MessageBox.Show(string.Format(
                                    "Playlist {0} referenced missing angle file {1}.",
                                    FileInfo.Name, angleClipFileName));
                                return;
                            }

                            TSStreamClip angleClip = new TSStreamClip(//angleFile, 
                                angleClipFile);
                            angleClip.AngleIndex = angle + 1;
                            angleClip.TimeIn = streamClip.TimeIn;
                            angleClip.TimeOut = streamClip.TimeOut;
                            angleClip.RelativeTimeIn = streamClip.RelativeTimeIn;
                            angleClip.RelativeTimeOut = streamClip.RelativeTimeOut;
                            angleClip.Length = streamClip.Length;
                            StreamClips.Add(angleClip);
                        }
                        if (angles - 1 > AngleCount) AngleCount = angles - 1;
                    }

                    int streamInfoLength = ReadInt16(data, ref pos);
                    pos += 2;
                    int streamCountVideo = data[pos++];
                    int streamCountAudio = data[pos++];
                    int streamCountPG = data[pos++];
                    int streamCountIG = data[pos++];
                    int streamCountSecondaryAudio = data[pos++];
                    int streamCountSecondaryVideo = data[pos++];
                    int streamCountPIP = data[pos++];
                    pos += 5;

#if DEBUG
                    Debug.WriteLine(string.Format(
                        "{0} : {1} -> V:{2} A:{3} PG:{4} IG:{5} 2A:{6} 2V:{7} PIP:{8}",
                        Name, streamFileName, streamCountVideo, streamCountAudio, streamCountPG, streamCountIG,
                        streamCountSecondaryAudio, streamCountSecondaryVideo, streamCountPIP));
#endif

                    for (int i = 0; i < streamCountVideo; i++)
                    {
                        TSStream stream = CreatePlaylistStream(data, ref pos);
                        if (stream != null) PlaylistStreams[stream.PID] = stream;
                    }
                    for (int i = 0; i < streamCountAudio; i++)
                    {
                        TSStream stream = CreatePlaylistStream(data, ref pos);
                        if (stream != null) PlaylistStreams[stream.PID] = stream;
                    }
                    for (int i = 0; i < streamCountPG; i++)
                    {
                        TSStream stream = CreatePlaylistStream(data, ref pos);
                        if (stream != null) PlaylistStreams[stream.PID] = stream;
                    }
                    for (int i = 0; i < streamCountIG; i++)
                    {
                        TSStream stream = CreatePlaylistStream(data, ref pos);
                        if (stream != null) PlaylistStreams[stream.PID] = stream;
                    }
                    for (int i = 0; i < streamCountSecondaryAudio; i++)
                    {
                        TSStream stream = CreatePlaylistStream(data, ref pos);
                        if (stream != null) PlaylistStreams[stream.PID] = stream;
                        pos += 2;
                    }
                    for (int i = 0; i < streamCountSecondaryVideo; i++)
                    {
                        TSStream stream = CreatePlaylistStream(data, ref pos);
                        if (stream != null) PlaylistStreams[stream.PID] = stream;
                        pos += 6;
                    }
                    /*
                     * TODO
                     * 
                    for (int i = 0; i < streamCountPIP; i++)
                    {
                        TSStream stream = CreatePlaylistStream(data, ref pos);
                        if (stream != null) PlaylistStreams[stream.PID] = stream;
                    }
                    */

                    pos += itemLength - (pos - itemStart) + 2;
                }

                pos = chaptersOffset + 4;

                int chapterCount = ReadInt16(data, ref pos);

                for (int chapterIndex = 0;
                    chapterIndex < chapterCount;
                    chapterIndex++)
                {
                    int chapterType = data[pos + 1];

                    if (chapterType == 1)
                    {
                        int streamFileIndex =
                            ((int)data[pos + 2] << 8) + data[pos + 3];

                        long chapterTime =
                            ((long)data[pos + 4] << 24) +
                            ((long)data[pos + 5] << 16) +
                            ((long)data[pos + 6] << 8) +
                            ((long)data[pos + 7]);

                        TSStreamClip streamClip = chapterClips[streamFileIndex];

                        double chapterSeconds = (double)chapterTime / 45000;

                        double relativeSeconds =
                            chapterSeconds -
                            streamClip.TimeIn +
                            streamClip.RelativeTimeIn;

                        // TODO: Ignore short last chapter?
                        if (TotalLength - relativeSeconds > 1.0)
                        {
                            streamClip.Chapters.Add(chapterSeconds);
                            this.Chapters.Add(relativeSeconds);
                        }
                    }
                    else
                    {
                        // TODO: Handle other chapter types?
                    }
                    pos += 14;
                }
            }
            finally
            {
                if (fileReader != null)
                {
                    fileReader.Close();
                }
                if (fileStream != null)
                {
                    fileStream.Close();
                }
            }
        }

        protected TSStream CreatePlaylistStream(byte[] data, ref int pos)
        {
            TSStream stream = null;

            int start = pos;

            int headerLength = data[pos++];
            int headerPos = pos;
            int headerType = data[pos++];

            int pid = 0;
            int subpathid = 0;
            int subclipid = 0;

            switch (headerType)
            {
                case 1:
                    pid = ReadInt16(data, ref pos);
                    break;
                case 2:
                    subpathid = data[pos++];
                    subclipid = data[pos++];
                    pid = ReadInt16(data, ref pos);
                    break;
                case 3:
                    subpathid = data[pos++];
                    pid = ReadInt16(data, ref pos);
                    break;
                case 4:
                    subpathid = data[pos++];
                    subclipid = data[pos++];
                    pid = ReadInt16(data, ref pos);
                    break;
                default:
                    break;
            }

            pos = headerPos + headerLength;

            int streamLength = data[pos++];
            int streamPos = pos;

            TSStreamType streamType = (TSStreamType)data[pos++];
            switch (streamType)
            {
                case TSStreamType.MVC_VIDEO:
                    // TODO
                    break;

                case TSStreamType.AVC_VIDEO:
                case TSStreamType.MPEG1_VIDEO:
                case TSStreamType.MPEG2_VIDEO:
                case TSStreamType.VC1_VIDEO:

                    TSVideoFormat videoFormat = (TSVideoFormat)
                        (data[pos] >> 4);
                    TSFrameRate frameRate = (TSFrameRate)
                        (data[pos] & 0xF);
                    TSAspectRatio aspectRatio = (TSAspectRatio)
                        (data[pos + 1] >> 4);

                    stream = new TSVideoStream();
                    ((TSVideoStream)stream).VideoFormat = videoFormat;
                    ((TSVideoStream)stream).AspectRatio = aspectRatio;
                    ((TSVideoStream)stream).FrameRate = frameRate;

#if DEBUG
                    Debug.WriteLine(string.Format(
                        "\t{0} {1} {2} {3} {4}",
                        pid,
                        streamType,
                        videoFormat,
                        frameRate,
                        aspectRatio));
#endif

                    break;

                case TSStreamType.AC3_AUDIO:
                case TSStreamType.AC3_PLUS_AUDIO:
                case TSStreamType.AC3_PLUS_SECONDARY_AUDIO:
                case TSStreamType.AC3_TRUE_HD_AUDIO:
                case TSStreamType.DTS_AUDIO:
                case TSStreamType.DTS_HD_AUDIO:
                case TSStreamType.DTS_HD_MASTER_AUDIO:
                case TSStreamType.DTS_HD_SECONDARY_AUDIO:
                case TSStreamType.LPCM_AUDIO:
                case TSStreamType.MPEG1_AUDIO:
                case TSStreamType.MPEG2_AUDIO:

                    int audioFormat = ReadByte(data, ref pos);

                    TSChannelLayout channelLayout = (TSChannelLayout)
                        (audioFormat >> 4);
                    TSSampleRate sampleRate = (TSSampleRate)
                        (audioFormat & 0xF);

                    string audioLanguage = ReadString(data, 3, ref pos);

                    stream = new TSAudioStream();
                    ((TSAudioStream)stream).ChannelLayout = channelLayout;
                    ((TSAudioStream)stream).SampleRate = TSAudioStream.ConvertSampleRate(sampleRate);
                    ((TSAudioStream)stream).LanguageCode = audioLanguage;

#if DEBUG
                    Debug.WriteLine(string.Format(
                        "\t{0} {1} {2} {3} {4}",
                        pid,
                        streamType,
                        audioLanguage,
                        channelLayout,
                        sampleRate));
#endif

                    break;

                case TSStreamType.INTERACTIVE_GRAPHICS:
                case TSStreamType.PRESENTATION_GRAPHICS:

                    string graphicsLanguage = ReadString(data, 3, ref pos);

                    stream = new TSGraphicsStream();
                    ((TSGraphicsStream)stream).LanguageCode = graphicsLanguage;

                    if (data[pos] != 0)
                    {
                    }

#if DEBUG
                    Debug.WriteLine(string.Format(
                        "\t{0} {1} {2}",
                        pid,
                        streamType,
                        graphicsLanguage));
#endif

                    break;

                case TSStreamType.SUBTITLE:

                    int code = ReadByte(data, ref pos); // TODO
                    string textLanguage = ReadString(data, 3, ref pos);

                    stream = new TSTextStream();
                    ((TSTextStream)stream).LanguageCode = textLanguage;

#if DEBUG
                    Debug.WriteLine(string.Format(
                        "\t{0} {1} {2}",
                        pid,
                        streamType,
                        textLanguage));
#endif

                    break;

                default:
                    break;
            }

            pos = streamPos + streamLength;

            if (stream != null)
            {
                stream.PID = (ushort)pid;
                stream.StreamType = streamType;
            }

            return stream;
        }
        
        protected string ReadString(
            byte[] data,
            int count,
            ref int pos)
        {
            string val =
                ASCIIEncoding.ASCII.GetString(data, pos, count);

            pos += count;

            return val;
        }

        protected int ReadInt32(
            byte[] data,
            ref int pos)
        {
            int val =
                ((int)data[pos] << 24) +
                ((int)data[pos + 1] << 16) +
                ((int)data[pos + 2] << 8) +
                ((int)data[pos + 3]);

            pos += 4;

            return val;
        }

        protected int ReadInt16(
            byte[] data,
            ref int pos)
        {
            int val =
                ((int)data[pos] << 8) +
                ((int)data[pos + 1]);

            pos += 2;

            return val;
        }

        protected byte ReadByte(
            byte[] data,
            ref int pos)
        {
            return data[pos++];
        }
    }
}