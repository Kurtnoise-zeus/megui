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
using System.Text;

using MeGUI.core.util;

namespace MeGUI
{
    public enum ProcessingLevel { NONE, SOME, ALL };
    /// <summary>
    /// interface for muxer providers
    /// this interface offers a number of methods used to find out if a muxjob can be processed
    /// and how it can be processed
    /// </summary>
    public interface IMuxing : IIDable
    {
        bool SupportsVideoCodec(VideoCodec codec);
        bool SupportsAudioCodec(AudioCodec codec);
        List<VideoType> GetSupportedVideoTypes();
        List<VideoCodec> GetSupportedVideoCodecs();
        List<AudioCodec> GetSupportedAudioCodecs();
        List<AudioType> GetSupportedAudioTypes();
        List<SubtitleType> GetSupportedSubtitleTypes();
        List<ChapterType> GetSupportedChapterTypes();
        List<DeviceType> GetSupportedDeviceTypes();
        List<ContainerType> GetSupportedContainerOutputTypes();
        List<ContainerType> GetSupportedContainerInputTypes();
        /// <summary>
        /// checks all the given input stream types if they can be handled by this muxer
        /// </summary>
        /// <param name="inputTypes">all input stream types</param>
        /// <param name="handledInputTypes">input stream types this muxer can handle</param>
        /// <param name="unhandledInputTypes">input stream types this muxer cannot handle</param>
        /// <returns>ProcessingLevel indicating how much can be processed</returns>
        ProcessingLevel CanBeProcessed(MuxableType[] inputTypes, out List<MuxableType> handledInputTypes, out List<MuxableType> unhandledInputTypes);
        /// <summary>
        /// checks if the given input stream types and container input type can be handled by this muxer
        /// </summary>
        /// <param name="inputTypes">all input stream types</param>
        /// <param name="handledInputTypes">input stream types this muxer can handle</param>
        /// <param name="unhandledInputTypes">input stream types this muxer cannot handle</param>
        /// <returns>ProcessingLevel indicating how much can be processed</returns>
        ProcessingLevel CanBeProcessed(ContainerType[] inputContainer, MuxableType[] inputTypes, out List<MuxableType> handledInputTypes,
            out List<MuxableType> unhandledInputTypes);
        MuxerType MuxerType { get; }
        IJobProcessor GetMuxer(MeGUISettings settings);
        string GetOutputTypeFilter(ContainerType containerType);
        string GetVideoInputFilter();
        string GetAudioInputFilter();
        string GetSubtitleInputFilter();
        string GetChapterInputFilter();
        string GetOutputTypeFilter();
        string GetMuxedInputFilter();
        string GetDeviceInputFilter();
        string Name { get;}
        System.Windows.Forms.Shortcut Shortcut { get; }
        List<ContainerType> GetContainersInCommon(IMuxing iMuxing);
    }
    public interface IEncoding<TCodec, TType, TEncoderType>
    where TType : OutputType
    {
        List<TEncoderType> GetSupportedEncoderTypes();
        List<TCodec> GetSupportedCodecs();
        List<TType> GetSupportedOutputTypes(TEncoderType encoder);
        IJobProcessor GetEncoder(TEncoderType codec, TType type, MeGUISettings settings);
        string GetOutputTypeFilter(TEncoderType codec, TType type);
        string GetInputTypeFilter();
    }

    public class VideoStream
    {
        string input, output;
        ulong numberOfFrames;
        Dar? dar = null;
        decimal framerate;
        MuxableType videoType;
        VideoCodecSettings settings;
        public VideoStream()
        {
            input = "";
            numberOfFrames = 0;
        }
        public string Output
        {
          get { return output; }
          set { output = value; }
        }
        public string Input
        {
          get { return input; }
          set { input = value; }
        }
        public Dar? DAR
        {
            get { return dar; }
            set { dar = value; }
        }
        public ulong NumberOfFrames
        {
          get { return numberOfFrames; }
          set { numberOfFrames = value; }
        }
        public decimal Framerate
        {
          get { return framerate; }
          set { framerate = value; }
        }
        public MuxableType VideoType
        {
            get { return videoType; }
            set { videoType = value; }
        }
        public VideoCodecSettings Settings
        {
            get { return settings; }
            set { settings = value; }
        }
    }
}