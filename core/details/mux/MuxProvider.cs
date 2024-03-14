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
using System.Collections.Generic;

namespace MeGUI
{
    public enum MuxerType { MP4BOX, MKVMERGE, AVIMUXGUI, TSMUXER, FFMPEG };
    public class MuxableType
    {
        public OutputType outputType;
        public ICodec codec;
        public MuxableType(OutputType type, ICodec codec)
        {
            this.outputType = type;
            this.codec = codec;
        }
    }
    public class MuxProvider
    {
        MainForm mainForm;
        MuxPathComparer comparer;
        VideoEncoderProvider vProvider = new VideoEncoderProvider();
        AudioEncoderProvider aProvider = new AudioEncoderProvider();
        public MuxProvider(MainForm mainForm)
        {
            this.mainForm = mainForm;
            comparer = new MuxPathComparer();
        }

        public IMuxing GetMuxer(MuxerType type)
        {
            foreach (IMuxing muxing in mainForm.PackageSystem.MuxerProviders.Values)
                if (muxing.MuxerType == type)
                    return muxing;
            return null;
        }

        public IJobProcessor GetMuxer(MuxerType type, MeGUISettings settings)
        {
            IMuxing muxer = GetMuxer(type);
            if (muxer == null)
                return null;
            return muxer.GetMuxer(settings);
        }


        /// <summary>
        /// Finds the best mux path and the output types the encoders should produce
        /// </summary>
        /// <param name="videoCodec"></param>
        /// <param name="audioCodecs"></param>
        /// <param name="containerType"></param>
        /// <param name="dictatedTypes"></param>
        /// <returns></returns>
        public MuxPath GetMuxPath(VideoEncoderType videoCodec, AudioEncoderType[] audioCodecs, ContainerType containerType, 
            params MuxableType[] dictatedTypes)
        {
            List<IEncoderType> inputCodecs = new List<IEncoderType>();
            if (videoCodec != null) 
                inputCodecs.Add(videoCodec);
            foreach (AudioEncoderType ac in audioCodecs)
                inputCodecs.Add(ac);
            
            List<MuxableType> decidedTypeList = new List<MuxableType>();
            decidedTypeList.AddRange(dictatedTypes);

            return findBestMuxPathAndConfig(inputCodecs, decidedTypeList, containerType);
        }

        /// <summary>
        /// Finds the best mux path
        /// </summary>
        /// <param name="containerType"></param>
        /// <param name="allTypes"></param>
        /// <returns></returns>
        public MuxPath GetMuxPath(ContainerType containerType, bool alwaysMux, params MuxableType[] allTypes)
        {
            List<MuxableType> inputTypes = new List<MuxableType>();
            inputTypes.AddRange(allTypes);
            MuxPath shortestPath = getShortestMuxPath(new MuxPath(inputTypes, containerType, alwaysMux), inputTypes, containerType);
            return shortestPath;
        }

        public bool CanBeMuxed(ContainerType containerType, params MuxableType[] allTypes)
        {
            MuxPath muxPath = GetMuxPath(containerType, false, allTypes);
            if (muxPath != null)
                return true;
            else
                return false;
        }

        public bool CanBeMuxed(VideoEncoderType codec, AudioEncoderType[] audioCodecs, ContainerType containerType,
            params MuxableType[] decidedTypes)
        {
            MuxPath muxPath = GetMuxPath(codec, audioCodecs, containerType, decidedTypes);
            if (muxPath != null)
                return true;
            else
                return false;
        }
                
        public List<ContainerType> GetSupportedContainers()
        {
            List<ContainerType> supportedContainers = new List<ContainerType>();
            foreach (IMuxing muxerInterface in mainForm.PackageSystem.MuxerProviders.Values)
            {
                foreach (ContainerType type in muxerInterface.GetSupportedContainerOutputTypes())
                {
                    if (!supportedContainers.Contains(type))
                        supportedContainers.Add(type);
                }
            }
            return supportedContainers;
        }

        public List<DeviceType> GetSupportedDevices(ContainerType oType)
        {
            List<DeviceType> supportedDevices = new List<DeviceType>();
            foreach (IMuxing muxerInterface in mainForm.PackageSystem.MuxerProviders.Values)
            {
                foreach (DeviceType type in muxerInterface.GetSupportedDeviceTypes())
                {
                    if (type.ContainerType == oType)
                        if (!supportedDevices.Contains(type))
                            supportedDevices.Add(type);
                }
            }
            return supportedDevices;
        }

        /// <summary>
        /// gets all the containers that can be supported given the video and a list of audio types
        /// this is used to limit the container dropdown in the autoencode window
        /// </summary>
        /// <param name="videoType">the desired video type</param>
        /// <param name="audioTypes">the desired audio types</param>
        /// <returns>a list of containers that can be supported</returns>
        public List<ContainerType> GetSupportedContainers(params MuxableType[] inputTypes)
        {
            // remove duplicate types
            List<MuxableType> allTypes = new List<MuxableType>();
            foreach (MuxableType oType in inputTypes)
            {
                bool bFound = false;
                foreach (MuxableType oAllType in allTypes)
                {
                    if (oType.outputType.ID.Equals(oAllType.outputType.ID))
                    {
                        bFound = true;
                        break;
                    }
                }
                if (!bFound)
                    allTypes.Add(oType);
            }

            List<ContainerType> supportedContainers = new List<ContainerType>();
            foreach (ContainerType cot in GetSupportedContainers())
            {
                if (CanBeMuxed(cot, allTypes.ToArray()))
                {
                    if (!supportedContainers.Contains(cot))
                        supportedContainers.Add(cot);
                }
            }
            return supportedContainers;
        }

        public List<ContainerType> GetSupportedContainers(VideoEncoderType videoCodec, AudioEncoderType[] audioCodecs,
            params MuxableType[] dictatedOutputTypes)
        {
            // remove duplicate codecs
            List<AudioEncoderType> allCodecs = new List<AudioEncoderType>();
            foreach (AudioEncoderType oType in audioCodecs)
                if (!allCodecs.Contains(oType))
                    allCodecs.Add(oType);

            // remove duplicate types
            List<MuxableType> allTypes = new List<MuxableType>();
            foreach (MuxableType oType in dictatedOutputTypes)
            {
                bool bFound = false;
                foreach (MuxableType oAllType in allTypes)
                {
                    if (oType.outputType != null && oAllType.outputType != null && oType.outputType.ID.Equals(oAllType.outputType.ID))
                    {
                        bFound = true;
                        break;
                    }
                }
                if (!bFound)
                    allTypes.Add(oType);
            }

            List<ContainerType> supportedContainers = new List<ContainerType>();
            List<ContainerType> allKnownContainers = GetSupportedContainers();
            foreach (ContainerType cot in allKnownContainers)
            {
                if (CanBeMuxed(videoCodec, allCodecs.ToArray(), cot, allTypes.ToArray()))
                {
                    if (!supportedContainers.Contains(cot))
                        supportedContainers.Add(cot);
                }
            }
            return supportedContainers;
        }
        
        #region private, implementation
        /// <summary>
        /// Finds the best mux path if some of the inputs have not yet been
        /// produced (they are yet to be encoded). When this is the case,
        /// there is more flexibility, as some encoders can produce outputs
        /// in multiple formats. This function suggests the output formats
        /// they should produce as well as the mux path.
        /// </summary>
        /// <param name="undecidedInputs">List of encoder types for the inputs which have not yet been encoded</param>
        /// <param name="decidedInputs">List of file types for the inputs which are already encoded</param>
        /// <param name="containerType">Target container type</param>
        /// <returns></returns>
        private MuxPath findBestMuxPathAndConfig(List<IEncoderType> undecidedInputs, List<MuxableType> decidedInputs, ContainerType containerType)
        {
            if (undecidedInputs.Count == 0)
            {
                return getShortestMuxPath(new MuxPath(decidedInputs, containerType), decidedInputs, containerType);
            }
            else
            {
                List<MuxPath> allPaths = new List<MuxPath>();
                IEncoderType undecidedInput = undecidedInputs[0];
                undecidedInputs.RemoveAt(0);

                if (undecidedInput is VideoEncoderType)
                {
                    VideoType[] allTypes = vProvider.GetSupportedOutput((VideoEncoderType)undecidedInput);
                    foreach (VideoType v in allTypes)
                    {
                        MuxableType input = new MuxableType(v, undecidedInput.Codec);
                        decidedInputs.Add(input);
                        MuxPath path = findBestMuxPathAndConfig(undecidedInputs, decidedInputs, containerType);
                        if (path != null)
                        {
                            allPaths.Add(path);
                        }
                        decidedInputs.Remove(input);
                    }
                }
                if (undecidedInput is AudioEncoderType)
                {
                    AudioType[] allTypes = aProvider.GetSupportedOutput((AudioEncoderType)undecidedInput);
                    foreach (AudioType a in allTypes)
                    {
                        MuxableType input = new MuxableType(a, undecidedInput.Codec);
                        decidedInputs.Add(input);
                        MuxPath path = findBestMuxPathAndConfig(undecidedInputs, decidedInputs, containerType);
                        if (path != null)
                        {
                            allPaths.Add(path);
                        }
                        decidedInputs.Remove(input);
                    }
                }
                undecidedInputs.Insert(0, undecidedInput);
                return comparer.GetBestMuxPath(allPaths);
            }
        }
        
        /// <summary>
        /// Given a mux path in which all the inputs have already been handled,
        /// this finds the shortest mux path to achieve the desired container type.
        /// </summary>
        /// <param name="currentMuxPath"></param>
        /// <param name="remainingMuxers">List of muxers which haven't yet been used
        /// in this final stage. There's no point having a muxer twice in this final
        /// stage, as the output of the second time could substitute the output of 
        /// the first time.</param>
        /// <param name="desiredContainerType"></param>
        /// <returns></returns>
        private MuxPath getShortestMuxPath(MuxPath currentMuxPath,
            List<IMuxing> remainingMuxers, ContainerType desiredContainerType)
        {
            if (currentMuxPath.IsCompleted())
                return currentMuxPath;

            List<MuxPath> allMuxPaths = new List<MuxPath>();

            List<IMuxing> newRemainingMuxers = new List<IMuxing>();
            newRemainingMuxers.AddRange(remainingMuxers);

            foreach (IMuxing muxer in remainingMuxers)
            {
                bool supportsInput = currentMuxPath[currentMuxPath.Length - 1].muxerInterface.GetContainersInCommon(muxer).Count > 0;
                if (!supportsInput) continue;
             
                MuxPath newMuxPath = currentMuxPath.Clone();

                MuxPathLeg currentMPL = new MuxPathLeg();
                currentMPL.muxerInterface = muxer;
                currentMPL.handledInputTypes = new List<MuxableType>();
                currentMPL.unhandledInputTypes = new List<MuxableType>();
                newMuxPath.Add(currentMPL);

                newRemainingMuxers.Remove(muxer);
                MuxPath shortestPath = getShortestMuxPath(newMuxPath, newRemainingMuxers, desiredContainerType);
                if (shortestPath != null)
                {
                    allMuxPaths.Add(shortestPath);
                }
                newRemainingMuxers.Add(muxer);
            }
            return comparer.GetBestMuxPath(allMuxPaths);
        }

        /// <summary>
        /// Step in the recursive stage which chooses, of all the MuxableTypes which
        /// *could* be handled, whether they should be. That means, it generates a
        /// mux path which involves muxing in each of the 2^n combinations of inputs
        /// at this stage. 
        /// 
        /// I'm not sure if this step is actually necessary. The only possible
        /// use I can think of is if you have a specific muxpath rule which says
        /// that only one file can be muxed in at a time, or only some specific
        /// combination of files can be muxed in at a time.
        ///           -- berrinam
        /// </summary>
        /// <param name="currentMuxPath"></param>
        /// <param name="muxer"></param>
        /// <param name="decidedHandledTypes"></param>
        /// <param name="undecidedPossibleHandledTypes"></param>
        /// <param name="unhandledInputTypes"></param>
        /// <param name="desiredContainerType"></param>
        /// <returns></returns>
        private MuxPath getShortestMuxPath(MuxPath currentMuxPath, IMuxing muxer, List<MuxableType> decidedHandledTypes,
            List<MuxableType> undecidedPossibleHandledTypes, List<MuxableType> unhandledInputTypes, ContainerType desiredContainerType)
        {
            if (undecidedPossibleHandledTypes.Count == 0)
            {
                MuxPathLeg mpl = new MuxPathLeg();
                mpl.muxerInterface = muxer;
                mpl.handledInputTypes = new List<MuxableType>(decidedHandledTypes);
                mpl.unhandledInputTypes = new List<MuxableType>(unhandledInputTypes);
                MuxPath newMuxPath = currentMuxPath.Clone();
                newMuxPath.Add(mpl);
                if (decidedHandledTypes.Count == 0)
                    return null;
                return getShortestMuxPath(newMuxPath, unhandledInputTypes, desiredContainerType);
            }
            else
            {
                List<MuxPath> allMuxPaths = new List<MuxPath>();
                MuxableType type = undecidedPossibleHandledTypes[0];
                undecidedPossibleHandledTypes.RemoveAt(0);

                decidedHandledTypes.Add(type);
                MuxPath shortestMuxPath = getShortestMuxPath(currentMuxPath, muxer, decidedHandledTypes, undecidedPossibleHandledTypes, unhandledInputTypes, desiredContainerType);
                if (shortestMuxPath != null)
                    allMuxPaths.Add(shortestMuxPath);
                decidedHandledTypes.Remove(type);

                unhandledInputTypes.Add(type);
                shortestMuxPath = getShortestMuxPath(currentMuxPath, muxer, decidedHandledTypes, undecidedPossibleHandledTypes, unhandledInputTypes, desiredContainerType);
                if (shortestMuxPath != null)
                    allMuxPaths.Add(shortestMuxPath);
                unhandledInputTypes.Remove(type);

                undecidedPossibleHandledTypes.Add(type);

                return (comparer.GetBestMuxPath(allMuxPaths));
            }
        }

        /// <summary>
        /// Finds the shortest mux path for a given set of input MuxableTypes (ie the
        /// encoder output types have all already been chosen here).
        /// </summary>
        /// Initial stage: if currentMuxPath is empty, it creates a first leg.
        /// Recursive step: It tries out adding all possible muxers to the current mux path,
        ///                 and calls itself with this extended mux path. It returns the shortest path.
        /// Final step: This will stop recursing if there are no muxers that can help, or if a muxer 
        ///                 is found in one step that finalizes the path. This is guaranteed to finish:
        ///                 if no progress is made, then it will not recurse. Otherwise, there is a finite
        ///                 amount of progress (progress is the number of streams muxed), so it will eventually
        ///                 stop progressing.
        /// <param name="currentMuxPath">Current mux path to be worked on</param>
        /// <param name="unhandledDesiredInputTypes">What remains to be muxed</param>
        /// <param name="desiredContainerType">Container type we are aiming at</param>
        /// <returns></returns>
        private MuxPath getShortestMuxPath(MuxPath currentMuxPath, 
            List<MuxableType> unhandledDesiredInputTypes, ContainerType desiredContainerType)
        {
            if (currentMuxPath.IsCompleted())
                return currentMuxPath;
        
            List<MuxableType> handledInputTypes;
            List<MuxableType> unhandledInputTypes;
            List<MuxPath> allMuxPaths = new List<MuxPath>();

            foreach (IMuxing muxer in mainForm.PackageSystem.MuxerProviders.Values)
            {
                bool bFound = false;
                foreach (ContainerType ctype in muxer.GetSupportedContainerOutputTypes())
                {
                    if (desiredContainerType.Equals(ctype))
                    {
                        bFound = true;
                        break;
                    }
                }

                // proceed only if muxer is a possible muxer for the selected output container
                if (!bFound)
                    continue;

                ProcessingLevel level;
                if (currentMuxPath.Length > 0)
                {
                    level = muxer.CanBeProcessed(
                        currentMuxPath[currentMuxPath.Length - 1].muxerInterface.GetSupportedContainerOutputTypes().ToArray(),
                        unhandledDesiredInputTypes.ToArray(), out handledInputTypes, out unhandledInputTypes);
                }
                else
                {
                    level = muxer.CanBeProcessed(unhandledDesiredInputTypes.ToArray(), out handledInputTypes, out unhandledInputTypes);
                }
                if (level != ProcessingLevel.NONE)
                {
                    MuxPath newMuxPath = currentMuxPath.Clone();

                    MuxPathLeg currentMPL = new MuxPathLeg();
                    currentMPL.muxerInterface = muxer;
                    currentMPL.handledInputTypes = handledInputTypes;
                    currentMPL.unhandledInputTypes = unhandledInputTypes;
                    newMuxPath.Add(currentMPL);

                    if (unhandledInputTypes.Count == 0)
                    {
                        // All the streams have been muxed into some file. Now let's 
                        // just make sure that we can convert this file to the format we want
                        // (or leave it alone if it already is in that format).
                        List<IMuxing> allMuxers = new List<IMuxing>();
                        allMuxers.AddRange(mainForm.PackageSystem.MuxerProviders.Values);
                        MuxPath shortestPath = getShortestMuxPath(newMuxPath, allMuxers, desiredContainerType);
                        if (shortestPath != null)
                            allMuxPaths.Add(shortestPath);
                    }

                    MuxPath aShortestPath = getShortestMuxPath(currentMuxPath, muxer, new List<MuxableType>(), handledInputTypes, unhandledInputTypes, desiredContainerType);
                    if (aShortestPath != null)
                        allMuxPaths.Add(aShortestPath);
                }
            }
            return comparer.GetBestMuxPath(allMuxPaths);
        }
        #endregion
    }
    public struct MuxPathLeg
    {
        public IMuxing muxerInterface;
        public List<MuxableType> handledInputTypes;
        public List<MuxableType> unhandledInputTypes; // those remain for the next leg
    }
    #region muxer providers
    public class MP4BoxMuxerProvider : MuxerProvider
    {
        
        public MP4BoxMuxerProvider() : base("MP4Box")
        {
            supportedVideoTypes.Add(VideoType.AVI);
            supportedVideoTypes.Add(VideoType.MP4);
            supportedVideoTypes.Add(VideoType.MPEG1);
            supportedVideoTypes.Add(VideoType.MPEG2);
            supportedVideoTypes.Add(VideoType.RAWASP);
            supportedVideoTypes.Add(VideoType.RAWAVC);
            supportedVideoTypes.Add(VideoType.RAWAVC2);
            supportedVideoTypes.Add(VideoType.RAWHEVC);
            supportedVideoTypes.Add(VideoType.RAWHEVC2);
            supportedVideoTypes.Add(VideoType.MKV);
            supportedVideoTypes.Add(VideoType.VP8);
            supportedVideoTypes.Add(VideoType.VP9);
            supportedVideoTypes.Add(VideoType.VC1);

            supportedAudioTypes.Add(AudioType.AC3);
            supportedAudioTypes.Add(AudioType.RAWAAC);
            supportedAudioTypes.Add(AudioType.MP4AAC);
            supportedAudioTypes.Add(AudioType.M4A);
            supportedAudioTypes.Add(AudioType.MP2);
            supportedAudioTypes.Add(AudioType.MP3);
            supportedAudioTypes.Add(AudioType.OPUS);

            supportedVideoCodecs.Add(VideoCodec.ASP);
            supportedVideoCodecs.Add(VideoCodec.AVC);
            supportedVideoCodecs.Add(VideoCodec.HEVC);
            supportedVideoCodecs.Add(VideoCodec.MPEG1);
            supportedVideoCodecs.Add(VideoCodec.MPEG2);
            supportedVideoCodecs.Add(VideoCodec.VC1);
            supportedVideoCodecs.Add(VideoCodec.VP8);
            supportedVideoCodecs.Add(VideoCodec.VP9);
            supportsAnyInputtableVideoCodec = false;

            supportedAudioCodecs.Add(AudioCodec.AAC);
            supportedAudioCodecs.Add(AudioCodec.AC3);
            supportedAudioCodecs.Add(AudioCodec.MP2);
            supportedAudioCodecs.Add(AudioCodec.MP3);
            supportedAudioCodecs.Add(AudioCodec.OPUS);
            supportsAnyInputtableAudioCodec = false;

            supportedSubtitleTypes.Add(SubtitleType.SUBRIP);
            supportedSubtitleTypes.Add(SubtitleType.VOBSUB);
            supportedSubtitleTypes.Add(SubtitleType.TTXT);

            supportedChapterTypes.Add(ChapterType.OGG_TXT);

            supportedContainerOutputTypes.Add(ContainerType.MP4);

            supportedContainerInputTypes.Add(ContainerType.AVI);
            supportedContainerInputTypes.Add(ContainerType.MP4);
            supportedContainerInputTypes.Add(ContainerType.MKV);

            supportedDeviceTypes.Add(DeviceType.APPLETV);
            supportedDeviceTypes.Add(DeviceType.IPAD);
            supportedDeviceTypes.Add(DeviceType.IPHONE);
            supportedDeviceTypes.Add(DeviceType.IPOD);
            supportedDeviceTypes.Add(DeviceType.ISMA);
            supportedDeviceTypes.Add(DeviceType.PSP);   
            
            base.type = MuxerType.MP4BOX;
            maxFilesOfType = new int[] { 1, -1, -1, 1, 1};
            name = "MP4 Muxer";
            shortcut = System.Windows.Forms.Shortcut.Ctrl5;
        }

        public override IJobProcessor GetMuxer(MeGUISettings settings)
        {
            return new MP4BoxMuxer(settings.Mp4Box.Path);
        }
    }

    public class MKVMergeMuxerProvider : MuxerProvider
    {
        public MKVMergeMuxerProvider() : base("mkvmerge")
        {
            supportedVideoTypes.Add(VideoType.AVI);
            supportedVideoTypes.Add(VideoType.MKV);
            supportedVideoTypes.Add(VideoType.MP4);
            supportedVideoTypes.Add(VideoType.RAWAVC);
            supportedVideoTypes.Add(VideoType.RAWAVC2);
            supportedVideoTypes.Add(VideoType.MPEG1);
            supportedVideoTypes.Add(VideoType.MPEG2);
            supportedVideoTypes.Add(VideoType.VC1);
            supportedVideoTypes.Add(VideoType.VP8);
            supportedVideoTypes.Add(VideoType.VP9);
            supportedVideoTypes.Add(VideoType.RAWHEVC);
            supportedVideoTypes.Add(VideoType.RAWHEVC2);
            supportedVideoTypes.Add(VideoType.M2TS);

            supportedAudioTypes.Add(AudioType.RAWAAC);
            supportedAudioTypes.Add(AudioType.MP4AAC);
            supportedAudioTypes.Add(AudioType.M4A);
            supportedAudioTypes.Add(AudioType.MP3);
            supportedAudioTypes.Add(AudioType.VORBIS);
            supportedAudioTypes.Add(AudioType.OPUS);
            supportedAudioTypes.Add(AudioType.MP2);
            supportedAudioTypes.Add(AudioType.AC3);
            supportedAudioTypes.Add(AudioType.DTS);
            supportedAudioTypes.Add(AudioType.WAV);
            supportedAudioTypes.Add(AudioType.PCM);
            supportedAudioTypes.Add(AudioType.EAC3);
            supportedAudioTypes.Add(AudioType.FLAC);
            supportedAudioTypes.Add(AudioType.THD);
            supportedAudioTypes.Add(AudioType.THDAC3);

            supportedVideoCodecs.Add(VideoCodec.ASP);
            supportedVideoCodecs.Add(VideoCodec.AVC);
            supportedVideoCodecs.Add(VideoCodec.HEVC);
            supportedVideoCodecs.Add(VideoCodec.HFYU);
            supportedVideoCodecs.Add(VideoCodec.MPEG1);
            supportedVideoCodecs.Add(VideoCodec.MPEG2);
            supportedVideoCodecs.Add(VideoCodec.VC1);
            supportsAnyInputtableVideoCodec = false;

            supportedAudioCodecs.Add(AudioCodec.AAC);
            supportedAudioCodecs.Add(AudioCodec.AC3);
            supportedAudioCodecs.Add(AudioCodec.DTS);
            supportedAudioCodecs.Add(AudioCodec.EAC3);
            supportedAudioCodecs.Add(AudioCodec.FLAC);
            supportedAudioCodecs.Add(AudioCodec.MP2);
            supportedAudioCodecs.Add(AudioCodec.MP3);
            supportedAudioCodecs.Add(AudioCodec.OPUS);
            supportedAudioCodecs.Add(AudioCodec.PCM);
            supportedAudioCodecs.Add(AudioCodec.THD);
            supportedAudioCodecs.Add(AudioCodec.THDAC3);
            supportedAudioCodecs.Add(AudioCodec.VORBIS);
            supportsAnyInputtableAudioCodec = false;

            supportedSubtitleTypes.Add(SubtitleType.SUBRIP);
            supportedSubtitleTypes.Add(SubtitleType.VOBSUB);
            supportedSubtitleTypes.Add(SubtitleType.SSA);
            supportedSubtitleTypes.Add(SubtitleType.ASS);
            supportedSubtitleTypes.Add(SubtitleType.BDSUP);

            supportedChapterTypes.Add(ChapterType.OGG_TXT);
            supportedChapterTypes.Add(ChapterType.MKV_XML);

            supportedContainerOutputTypes.Add(ContainerType.MKV);

            supportedContainerInputTypes.Add(ContainerType.MP4);
            supportedContainerInputTypes.Add(ContainerType.AVI);
            supportedContainerInputTypes.Add(ContainerType.MKV);
            supportedContainerInputTypes.Add(ContainerType.M2TS);

            maxFilesOfType = new int[] { -1, -1, -1, 1, 0};
            base.type = MuxerType.MKVMERGE;
            name = "MKV Muxer";
            shortcut = System.Windows.Forms.Shortcut.Ctrl4;
        }

        public override IJobProcessor GetMuxer(MeGUISettings settings)
        {
            return new MkvMergeMuxer(settings.MkvMerge.Path);
        }
    }

    public class AVIMuxGUIMuxerProvider : MuxerProvider
    {
        public AVIMuxGUIMuxerProvider(): base("AVIMuxGUI")
        {
            supportedVideoTypes.Add(VideoType.AVI);
            
            supportedVideoCodecs.Add(VideoCodec.ASP);
            supportedVideoCodecs.Add(VideoCodec.AVC);
            supportedVideoCodecs.Add(VideoCodec.HFYU);
            supportsAnyInputtableVideoCodec = false;

            supportedAudioTypes.Add(AudioType.AC3);
            supportedAudioTypes.Add(AudioType.DTS);
            supportedAudioTypes.Add(AudioType.MP3);
            supportedAudioTypes.Add(AudioType.MP2);
            supportedAudioTypes.Add(AudioType.RAWAAC);

            supportedAudioCodecs.Add(AudioCodec.AAC);
            supportedAudioCodecs.Add(AudioCodec.AC3);
            supportedAudioCodecs.Add(AudioCodec.DTS);
            supportedAudioCodecs.Add(AudioCodec.MP2);
            supportedAudioCodecs.Add(AudioCodec.MP3);
            supportsAnyInputtableAudioCodec = false;

            supportedDeviceTypes.Add(DeviceType.PC);
            
            supportedSubtitleTypes.Add(SubtitleType.SUBRIP);
            
            supportedContainerOutputTypes.Add(ContainerType.AVI);

            supportedContainerInputTypes.Add(ContainerType.AVI);
            supportedContainerInputTypes.Add(ContainerType.MKV);
            
            maxFilesOfType = new int[] { 1, -1, -1, 0, 1};
            base.type = MuxerType.AVIMUXGUI;
            name = "AVI Muxer";
            shortcut = System.Windows.Forms.Shortcut.Ctrl2;
        }

        public override IJobProcessor GetMuxer(MeGUISettings settings)
        {
            return new AMGMuxer(settings.AviMuxGui.Path);
        }
    }
    public class TSMuxerProvider : MuxerProvider
    {
        public TSMuxerProvider() : base("tsMuxeR")
        {
            supportedVideoTypes.Add(VideoType.RAWAVC);
            supportedVideoTypes.Add(VideoType.RAWAVC2);
            supportedVideoTypes.Add(VideoType.RAWHEVC);
            supportedVideoTypes.Add(VideoType.RAWHEVC2);
            supportedVideoTypes.Add(VideoType.VC1);
            supportedVideoTypes.Add(VideoType.MPEG2);
            supportedVideoTypes.Add(VideoType.MP4);
            supportedVideoTypes.Add(VideoType.MKV);
            supportedVideoTypes.Add(VideoType.M2TS);

            supportedAudioTypes.Add(AudioType.AC3);
            supportedAudioTypes.Add(AudioType.DTS);
            supportedAudioTypes.Add(AudioType.EAC3);
            supportedAudioTypes.Add(AudioType.RAWAAC);
            supportedAudioTypes.Add(AudioType.MP4AAC);
            supportedAudioTypes.Add(AudioType.WAV);
            supportedAudioTypes.Add(AudioType.W64);
            supportedAudioTypes.Add(AudioType.PCM);
            supportedAudioTypes.Add(AudioType.THD);
            supportedAudioTypes.Add(AudioType.THDAC3);

            supportedVideoCodecs.Add(VideoCodec.AVC);
            supportedVideoCodecs.Add(VideoCodec.HEVC);
            supportedVideoCodecs.Add(VideoCodec.MPEG2);
            supportedVideoCodecs.Add(VideoCodec.VC1);
            supportsAnyInputtableVideoCodec = false;

            supportedAudioCodecs.Add(AudioCodec.AAC);
            supportedAudioCodecs.Add(AudioCodec.AC3);
            supportedAudioCodecs.Add(AudioCodec.DTS);
            supportedAudioCodecs.Add(AudioCodec.EAC3);
            supportedAudioCodecs.Add(AudioCodec.THDAC3);
            supportedAudioCodecs.Add(AudioCodec.PCM);
            supportsAnyInputtableAudioCodec = false;

            supportedSubtitleTypes.Add(SubtitleType.SUBRIP);
            supportedSubtitleTypes.Add(SubtitleType.BDSUP);

            supportedChapterTypes.Add(ChapterType.OGG_TXT);

            supportedContainerOutputTypes.Add(ContainerType.M2TS);

            supportedContainerInputTypes.Add(ContainerType.MKV);
            supportedContainerInputTypes.Add(ContainerType.MP4);
            supportedContainerInputTypes.Add(ContainerType.M2TS);

            supportedDeviceTypes.Add(DeviceType.AVCHD);
            supportedDeviceTypes.Add(DeviceType.BD);

            base.type = MuxerType.TSMUXER;
            maxFilesOfType = new int[] { 1, -1, -1, 1, 1 };
            name = "M2TS Muxer";
            shortcut = System.Windows.Forms.Shortcut.Ctrl3;
        }

        public override IJobProcessor GetMuxer(MeGUISettings settings)
        {
            return new tsMuxeR(settings.TSMuxer.Path);
        }
    }

    public class FFmpegMuxerProvider : MuxerProvider
    {
        public FFmpegMuxerProvider() : base("FFmpeg")
        {
            supportedVideoTypes.Add(VideoType.RAWASP);
            supportedVideoCodecs.Add(VideoCodec.ASP);
            supportsAnyInputtableVideoCodec = false;
            supportsAnyInputtableAudioCodec = false;

            supportedContainerOutputTypes.Add(ContainerType.AVI);
            supportedContainerOutputTypes.Add(ContainerType.MKV);

            supportedContainerInputTypes.Add(ContainerType.AVI);

            maxFilesOfType = new int[] { 1, 0, 0, 0, 0 };
            base.type = MuxerType.FFMPEG;
            name = "FFmpeg Muxer";
        }
 
        public override IJobProcessor GetMuxer(MeGUISettings settings)
        {
            return new tsMuxeR(settings.FFmpeg.Path);
        }
    }
    #endregion

    #region top level providers
    public abstract class MuxerProvider : IMuxing
    {
        protected List<VideoType> supportedVideoTypes;
        protected List<VideoCodec> supportedVideoCodecs;
        protected List<AudioCodec> supportedAudioCodecs;
        protected List<AudioType> supportedAudioTypes;
        protected List<SubtitleType> supportedSubtitleTypes;
        protected List<ChapterType> supportedChapterTypes;
        protected List<ContainerType> supportedContainerOutputTypes;
        protected List<ContainerType> supportedContainerInputTypes;
        protected List<DeviceType> supportedDeviceTypes;
        protected bool supportsAnyInputtableAudioCodec = false;
        protected bool supportsAnyInputtableVideoCodec = false;
        protected string videoInputFilter, audioInputFilter, subtitleInputFilter;
        protected int[] maxFilesOfType;
        protected string name;
        protected System.Windows.Forms.Shortcut shortcut;
        protected string id;
        protected MuxerType type;
        public MuxerProvider(string id)
        {
            supportedVideoTypes = new List<VideoType>();
            supportedAudioTypes = new List<AudioType>();
            supportedSubtitleTypes = new List<SubtitleType>();
            supportedChapterTypes = new List<ChapterType>();
            supportedAudioCodecs = new List<AudioCodec>();
            supportedVideoCodecs = new List<VideoCodec>();
            supportedContainerOutputTypes = new List<ContainerType>();
            supportedContainerInputTypes = new List<ContainerType>();
            supportedDeviceTypes = new List<DeviceType>();
            videoInputFilter = audioInputFilter = subtitleInputFilter = "";
            this.id = id;
        }
        #region IMuxing Members
        public string ID
        {
            get
            {
                return id;
            }
        }
        public bool SupportsVideoCodec(VideoCodec codec)
        {
            if (supportsAnyInputtableVideoCodec)
                return true;
            return (GetSupportedVideoCodecs().Contains(codec));
        }

        public bool SupportsAudioCodec(AudioCodec codec)
        {
            if (supportsAnyInputtableAudioCodec)
                return true;
            return (GetSupportedAudioCodecs().Contains(codec));
        }

        public List<VideoCodec> GetSupportedVideoCodecs()
        {
            return supportedVideoCodecs;
        }

        public List<AudioCodec> GetSupportedAudioCodecs()
        {
            return supportedAudioCodecs;
        }

        public string Name
        {
            get
            {
                return name;
            }
        }

        public MuxerType MuxerType
        {
            get { return type; }
        }

        public System.Windows.Forms.Shortcut Shortcut
        {
            get { return shortcut; }
        }

        public List<VideoType> GetSupportedVideoTypes()
        {
            return this.supportedVideoTypes;
        }

        public List<AudioType> GetSupportedAudioTypes()
        {
            return this.supportedAudioTypes;
        }

        public List<SubtitleType> GetSupportedSubtitleTypes()
        {
            return this.supportedSubtitleTypes;
        }

        public List<ChapterType> GetSupportedChapterTypes()
        {
            return this.supportedChapterTypes;
        }

        public List<ContainerType> GetSupportedContainerOutputTypes()
        {
            return this.supportedContainerOutputTypes;
        }

        public List<DeviceType> GetSupportedDeviceTypes()
        {
            return this.supportedDeviceTypes;
        }

        public List<ContainerType> GetSupportedContainerInputTypes()
        {
            return this.supportedContainerInputTypes;
        }

        public List<ContainerType> GetContainersInCommon(IMuxing iMuxing)
        {
            List<ContainerType> supportedOutputTypes = GetSupportedContainerOutputTypes();
            List<ContainerType> nextSupportedInputTypes = iMuxing.GetSupportedContainerInputTypes();
            List<ContainerType> commonContainers = new List<ContainerType>();
            foreach (ContainerType eligibleType in supportedOutputTypes)
            {
                if (nextSupportedInputTypes.Contains(eligibleType))
                    commonContainers.Add(eligibleType);
            }
            return commonContainers;
        }

        public abstract IJobProcessor GetMuxer(MeGUISettings meguiSettings);

        /// <summary>
        /// Returns the number of the type if it is supported, otherwise -1
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private int getSupportedType(MuxableType type)
        {
            if (type.outputType is VideoType && supportedVideoTypes.Contains((VideoType)type.outputType) &&
                (supportsAnyInputtableVideoCodec || supportedVideoCodecs.Contains((VideoCodec)type.codec)))
                return 0;
            if (type.outputType is AudioType && supportedAudioTypes.Contains((AudioType)type.outputType) &&
                (supportsAnyInputtableAudioCodec || supportedAudioCodecs.Contains((AudioCodec)type.codec)))
                return 1;
            if (type.outputType is SubtitleType && supportedSubtitleTypes.Contains((SubtitleType)type.outputType))
                return 2;
            if (type.outputType is ChapterType && supportedChapterTypes.Contains((ChapterType)type.outputType))
                return 3;
            if (type.outputType is DeviceType && supportedDeviceTypes.Contains((DeviceType)type.outputType))
                return 4;
            return -1;
        }

        public ProcessingLevel CanBeProcessed(MuxableType[] inputTypes, out List<MuxableType> handledInputTypes,
            out List<MuxableType> unhandledInputTypes)
        {
            handledInputTypes = new List<MuxableType>();
            unhandledInputTypes = new List<MuxableType>();
            int[] filesOfType = new int[5];
            foreach (MuxableType inputType in inputTypes)
            {
                int type = getSupportedType(inputType);
                if (type >= 0)
                {
                    if (maxFilesOfType[type] < 0 // We ignore it in this case
                        || filesOfType[type] < maxFilesOfType[type])
                    {
                        handledInputTypes.Add(inputType);
                        filesOfType[type]++;
                    }
                    else
                        unhandledInputTypes.Add(inputType);
                }
                else
                    unhandledInputTypes.Add(inputType);
            }
            ProcessingLevel retval = ProcessingLevel.NONE;
            if (handledInputTypes.Count > 0)
                retval = ProcessingLevel.SOME;
            if (unhandledInputTypes.Count == 0 || inputTypes.Length == 0)
                retval = ProcessingLevel.ALL;
            return retval;
        }


        public ProcessingLevel CanBeProcessed(ContainerType[] inputContainers, MuxableType[] inputTypes, out List<MuxableType> handledInputTypes,
            out List<MuxableType> unhandledInputTypes)
        {
            bool commonContainerFound = false;
            foreach (ContainerType inputContainer in inputContainers)
            {
                if (GetSupportedContainerInputTypes().Contains(inputContainer))
                {
                    commonContainerFound = true;
                    break;
                }
            }
            if (commonContainerFound)
                return CanBeProcessed(inputTypes, out handledInputTypes, out unhandledInputTypes);
            else
            {
                handledInputTypes = new List<MuxableType>();
                unhandledInputTypes = new List<MuxableType>();
                return ProcessingLevel.NONE;
            }
        }

        public string GetOutputTypeFilter(ContainerType containerType)
        {
            foreach (ContainerType type in this.supportedContainerOutputTypes)
            {
                if (type == containerType)
                {
                    return type.OutputFilterString;
                }
            }
            return "";
        }

        public string GetOutputTypeFilter()
        {
            return VideoUtil.GenerateCombinedFilter(supportedContainerOutputTypes.ToArray());
        }

        public string GetVideoInputFilter()
        {
            return VideoUtil.GenerateCombinedFilter(supportedVideoTypes.ToArray());
        }

        public string GetAudioInputFilter()
        {
            return VideoUtil.GenerateCombinedFilter(supportedAudioTypes.ToArray());
        }

        public string GetChapterInputFilter()
        {
            return VideoUtil.GenerateCombinedFilter(supportedChapterTypes.ToArray());
        }

        public string GetSubtitleInputFilter()
        {
            return VideoUtil.GenerateCombinedFilter(supportedSubtitleTypes.ToArray());
        }

        public string GetMuxedInputFilter()
        {
            return VideoUtil.GenerateCombinedFilter(GetSupportedContainerInputTypes().ToArray());
        }

        public string GetDeviceInputFilter()
        {
            return VideoUtil.GenerateCombinedFilter(supportedDeviceTypes.ToArray());
        }
        #endregion
    }

    #region generic providers
    public abstract class EncodingProvider<TCodec, TType, TEncoderType> : IEncoding<TCodec, TType, TEncoderType>
        where TType : OutputType
    {
        protected List<TType> supportedTypes;
        protected List<TCodec> supportedCodecs;
        protected List<TEncoderType> supportedEncoderTypes;

        public abstract IJobProcessor CreateEncoder(MeGUISettings settings);

        public EncodingProvider()
        {
            supportedTypes = new List<TType>();
            supportedCodecs = new List<TCodec>();
            supportedEncoderTypes = new List<TEncoderType>();
        }

        #region IVideoEncoding Members

        public List<TEncoderType> GetSupportedEncoderTypes()
        {
            return supportedEncoderTypes;
        }

        public List<TCodec> GetSupportedCodecs()
        {
            return supportedCodecs;
        }

        public List<TType> GetSupportedOutputTypes(TEncoderType codec)
        {
            if (supportedEncoderTypes.Contains(codec))
                return supportedTypes;
            else
                return new List<TType>();
        }

        public IJobProcessor GetEncoder(TEncoderType codec, TType outputType, MeGUISettings settings)
        {
            if (supportedEncoderTypes.Contains(codec))
            {
                foreach (TType vto in GetSupportedOutputTypes(codec))
                {
                    if (vto.GetType() == outputType.GetType())
                        return CreateEncoder(settings);
                }
            }
            return null;
        }

        public string GetOutputTypeFilter(TEncoderType codec, TType outputType)
        {
            if (supportedEncoderTypes.Contains(codec))
            {
                foreach (TType vto in GetSupportedOutputTypes(codec))
                {
                    if (vto.GetType() == outputType.GetType())
                        return vto.OutputFilterString;
                }
                return null;
            }
            return "";
        }

        public virtual string GetInputTypeFilter()
        {
            return "AviSynth script files(*.avs)|*.avs";
        }
        #endregion

    }


    public class AllEncoderProvider<TCodec, TType, TEncoderType>
        where TType : OutputType
    {
        List<IEncoding<TCodec, TType, TEncoderType>> registeredEncoders;
        public AllEncoderProvider()
        {
            registeredEncoders = new List<IEncoding<TCodec, TType, TEncoderType>>();
        }
        /// <summary>
        /// checks all available video encoders to see if one supports the desired video codec with the desired
        /// output type
        /// </summary>
        /// <param name="videoCodec">the desired video codec</param>
        /// <param name="outputType">the desired video output</param>
        /// <returns>true if the codec/output type combo can be fullfilled, false if not</returns>
        public bool IsSupported(TEncoderType codec, TType outputType)
        {
            IJobProcessor encoder = GetEncoder(new MeGUISettings(), codec, outputType);
            if (encoder != null)
                return true;
            else
                return false;
        }
        /// <summary>
        /// gets the input filter string for a given codec type (based on the encoder that is capable of encoding
        /// the desired codec)
        /// </summary>
        /// <param name="codec">the desired codec</param>
        /// <returns>the input filter string for the desired codec</returns>
        public string GetSupportedInput(TCodec codec)
        {
            IEncoding<TCodec, TType, TEncoderType> enc = null;
            foreach (IEncoding<TCodec, TType, TEncoderType> encoder in this.registeredEncoders)
            {
                if (encoder.GetSupportedCodecs().Contains(codec))
                {
                    enc = encoder;
                    break;
                }
            }
            if (enc == null)
            {
                return "";
            }
            else
            {
                return enc.GetInputTypeFilter();
            }
        }
        /// <summary>
        /// gets all input types supported by the first encoder capable of handling the desired codec
        /// </summary>
        /// <param name="codec">the desired codec</param>
        /// <returns>a list of output types that the encoder for this desired codec can deliver directly</returns>
        public TType[] GetSupportedOutput(TEncoderType encoderType)
        {
            IEncoding<TCodec, TType, TEncoderType> enc = null;
            foreach (IEncoding<TCodec, TType, TEncoderType> encoder in this.registeredEncoders)
            {
                if (encoder.GetSupportedEncoderTypes().Contains(encoderType))
                {
                    enc = encoder;
                    break;
                }
            }
            if (enc == null)
            {
                return new TType[0];
            }
            else
            {
                List<TType> supportedTypes = enc.GetSupportedOutputTypes(encoderType);
                return supportedTypes.ToArray();
            }
        }
        /// <summary>
        /// gets an encoder for the given codec and output type
        /// </summary>
        /// <param name="videoCodec">the desired video codec</param>
        /// <param name="outputType">the desired output type</param>
        /// <returns>the encoder found or null if no encoder was found</returns>
        public IJobProcessor GetEncoder(MeGUISettings settings, TEncoderType codec, TType outputType)
        {
            IJobProcessor encoder = null;
            foreach (IEncoding<TCodec, TType, TEncoderType> encodingInterface in this.registeredEncoders)
            {
                if (!encodingInterface.GetSupportedEncoderTypes().Contains(codec))
                    continue;
                if (!encodingInterface.GetSupportedOutputTypes(codec).Contains(outputType))
                    continue;
                encoder = encodingInterface.GetEncoder(codec, outputType, settings);
            }
            return encoder;
        }
        /// <summary>
        /// registers a new encoder to the program
        /// </summary>
        /// <param name="encoder"></param>
        public void RegisterEncoder(IEncoding<TCodec, TType, TEncoderType> encoder)
        {
            this.registeredEncoders.Add(encoder);
        }
    }
    #endregion

    public class AudioEncodingProvider : EncodingProvider<AudioCodec, AudioType, AudioEncoderType>
    {
        public AudioEncodingProvider()
            : base()
        { }
        
        public override string GetInputTypeFilter()
        {
            return "All supported types|*.avs;*.wav;*.pcm;*.mpa;*.mp2;*.mp3;*.ac3;*.dts;*.m4a;*.aac";
        }

        public override IJobProcessor CreateEncoder(MeGUISettings settings)
        {
            return new AviSynthAudioEncoder(settings);
        }
    }
    public class VideoEncoderProvider : AllEncoderProvider<VideoCodec, VideoType, VideoEncoderType>
    {
        public VideoEncoderProvider()
            : base()
        {
            this.RegisterEncoder(new X264EncoderProvider());
            this.RegisterEncoder(new X265EncoderProvider());
            this.RegisterEncoder(new XviDEncoderProvider());
        }
    }
    public class AudioEncoderProvider : AllEncoderProvider<AudioCodec, AudioType, AudioEncoderType>
    {
        public AudioEncoderProvider()
            : base()
        {
            RegisterEncoder(new NeroAACEncodingProvider());
            RegisterEncoder(new LameMP3EncodingProvider());
            RegisterEncoder(new VorbisEncodingProvider());
            RegisterEncoder(new AC3EncodingProvider());
            RegisterEncoder(new MP2EncodingProvider());
            RegisterEncoder(new FlacEncodingProvider());
            RegisterEncoder(new QaacEncodingProvider());
            RegisterEncoder(new OpusEncodingProvider());
            RegisterEncoder(new FdkaacEncodingProvider());
            RegisterEncoder(new FfaacEncodingProvider());
        }
    }
    #endregion
    #region video encoding providers
    public class XviDEncoderProvider : EncodingProvider<VideoCodec, VideoType, VideoEncoderType>
    {
        public XviDEncoderProvider()
        {
            supportedCodecs.Add(VideoCodec.ASP);
            //supportedTypes.Add(VideoType.AVI);
            //supportedTypes.Add(VideoType.MKV);
            supportedTypes.Add(VideoType.RAWASP);
            supportedEncoderTypes.Add(VideoEncoderType.XVID);
        }

        public override IJobProcessor CreateEncoder(MeGUISettings settings)
        {
            return new XviDEncoder(settings.XviD.Path);
        }
    }

    public class X264EncoderProvider : EncodingProvider<VideoCodec, VideoType, VideoEncoderType>
    {
        public X264EncoderProvider()
        {
            supportedCodecs.Add(VideoCodec.AVC);
            //supportedTypes.Add(VideoType.MP4); disabled as official x264 builds do not output to mp4
            supportedTypes.Add(VideoType.MKV);
            supportedTypes.Add(VideoType.RAWAVC);
            supportedEncoderTypes.Add(VideoEncoderType.X264);
        }

        public override IJobProcessor CreateEncoder(MeGUISettings settings)
        {
            return new x264Encoder(settings.X264.Path);
        }
    }

    public class X265EncoderProvider : EncodingProvider<VideoCodec, VideoType, VideoEncoderType>
    {
        public X265EncoderProvider()
        {
            supportedCodecs.Add(VideoCodec.HEVC);
            //supportedTypes.Add(VideoType.MP4);
            //supportedTypes.Add(VideoType.MKV); 
            supportedTypes.Add(VideoType.RAWHEVC);
            supportedEncoderTypes.Add(VideoEncoderType.X265);
        }

        public override IJobProcessor CreateEncoder(MeGUISettings settings)
        {
            return new x265Encoder(settings.X265.Path);
        }
    }
    #endregion
    #region audio encoding providers
    public class NeroAACEncodingProvider : AudioEncodingProvider
    {
        public NeroAACEncodingProvider()
            : base()
        {
            supportedCodecs.Add(AudioCodec.AAC);
            supportedTypes.Add(AudioType.MP4AAC);
            supportedTypes.Add(AudioType.M4A);
            supportedEncoderTypes.Add(AudioEncoderType.NAAC);
        }
    }

    public class LameMP3EncodingProvider : AudioEncodingProvider
    {
        public LameMP3EncodingProvider()
            : base()
        {
            supportedCodecs.Add(AudioCodec.MP3);
            supportedTypes.Add(AudioType.MP3);
            supportedEncoderTypes.Add(AudioEncoderType.LAME);
        }
    }

    public class VorbisEncodingProvider : AudioEncodingProvider
    {
        public VorbisEncodingProvider()
            : base()
        {
            supportedCodecs.Add(AudioCodec.VORBIS);
            supportedTypes.Add(AudioType.VORBIS);
            supportedEncoderTypes.Add(AudioEncoderType.VORBIS);
        }
    }

    public class AC3EncodingProvider : AudioEncodingProvider
    {
        public AC3EncodingProvider()
            : base()
        {
            supportedCodecs.Add(AudioCodec.AC3);
            supportedTypes.Add(AudioType.AC3);
            supportedEncoderTypes.Add(AudioEncoderType.FFAC3);
        }
    }

    public class MP2EncodingProvider : AudioEncodingProvider
    {
        public MP2EncodingProvider()
            : base()
        {
            supportedCodecs.Add(AudioCodec.DTS);
            supportedTypes.Add(AudioType.MP2);
            supportedEncoderTypes.Add(AudioEncoderType.FFMP2);
        }
    }

    public class FlacEncodingProvider : AudioEncodingProvider
    {
        public FlacEncodingProvider()
            : base()
        {
            supportedCodecs.Add(AudioCodec.FLAC);
            supportedTypes.Add(AudioType.FLAC);
            supportedEncoderTypes.Add(AudioEncoderType.FLAC);
        }
    }

    public class QaacEncodingProvider : AudioEncodingProvider
    {
        public QaacEncodingProvider()
            : base()
        {
            supportedCodecs.Add(AudioCodec.AAC);
            supportedTypes.Add(AudioType.M4A);
            supportedTypes.Add(AudioType.MP4AAC);
            supportedEncoderTypes.Add(AudioEncoderType.QAAC);
        }
    }

    public class OpusEncodingProvider : AudioEncodingProvider
    {
        public OpusEncodingProvider()
            : base()
        {
            supportedCodecs.Add(AudioCodec.OPUS);
            supportedTypes.Add(AudioType.OPUS);
            supportedEncoderTypes.Add(AudioEncoderType.OPUS);
        }
    }

    public class FdkaacEncodingProvider : AudioEncodingProvider
    {
        public FdkaacEncodingProvider()
            : base()
        {
            supportedCodecs.Add(AudioCodec.AAC);
            supportedTypes.Add(AudioType.M4A);
            supportedTypes.Add(AudioType.MP4AAC);
            supportedEncoderTypes.Add(AudioEncoderType.FDKAAC);
        }
    }

    public class FfaacEncodingProvider : AudioEncodingProvider
    {
        public FfaacEncodingProvider()
            : base()
        {
            supportedCodecs.Add(AudioCodec.AAC);
            supportedTypes.Add(AudioType.M4A);
            supportedTypes.Add(AudioType.MP4AAC);
            supportedEncoderTypes.Add(AudioEncoderType.FFAAC);
        }
    }
    #endregion
}