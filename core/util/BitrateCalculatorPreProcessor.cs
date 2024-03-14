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
using System.Xml.Serialization;

using MeGUI.core.details;
using MeGUI.packages.tools.calculator;

namespace MeGUI.core.util
{
    public class BitrateCalculatorPreProcessor
    {
        #region bitrate calculation preprocessing
        public static JobPreProcessor CalculationProcessor = new JobPreProcessor(calculateBitrate, "CalculationProcessor");
        /// <summary>
        /// postprocesses an audio job followed by a video job
        /// this constellation happens in automated or one click encoding where we have an audio job linked
        /// to a video job
        /// first, any audio jobs previous to the audio job in question will be located
        /// then we get the size of all audio tracks
        /// from the desired final output size stored in the first video job, we calculate the video bitrate
        /// we have to use to obtain the final file size, taking container overhead into account
        /// the calculated bitrate is then applied to all video jobs
        /// </summary>
        /// <param name="firstAudio">the audio job that is linked to a video job</param>
        /// <param name="firstpass">the video job to which the audio job is linked</param>
        public static LogItem calculateBitrate(MainForm mainForm, Job ajob)
        {
            if (!(ajob is VideoJob)) return null;
            VideoJob job = (VideoJob)ajob;
            if (job.BitrateCalculationInfo == null) return null;

            BitrateCalculationInfo b = job.BitrateCalculationInfo;
            LogItem log = new LogItem("Bitrate calculation for video");
            
            List<AudioBitrateCalculationStream> audioStreams = new List<AudioBitrateCalculationStream>();
            foreach (string s in b.AudioFiles)
                audioStreams.Add(new AudioBitrateCalculationStream(s));

            double framerate;
            ulong framecount;
            JobUtil.GetInputProperties(job.Input, out framecount, out framerate);

            CalcData data = new CalcData((long)framecount, (decimal)framerate, b.Container, job.Settings.Codec, 
                job.Settings.NbBframes > 0, audioStreams.ToArray());
            data.TotalSize = b.DesiredSize;

            try
            {
                data.CalcByTotalSize();
            }
            catch (Exception e)
            {
                log.LogValue("Calculation failed", e, ImageType.Error);
                return log;
            }

            log.LogValue("Desired size after subtracting audio", Math.Floor(data.VideoSize.KBExact) + " KBs");
            log.LogValue("Calculated desired bitrate", (int)data.VideoBitrate + " kbit/s");

            foreach (TaggedJob t in b.VideoJobs)
                if (t.Job is VideoJob)
                    ((VideoJob)t.Job).Settings.BitrateQuantizer = (int)data.VideoBitrate;

            return log;
        }
        #endregion
    }

    public class BitrateCalculationInfo
    {
        public string[] _VideoJobNames
        {
            get
            {
                return MainForm.Instance.Jobs.ToStringList(VideoJobs).ToArray();
            }
            set {
                _videoJobNames = value;
            }
        }
        private string[] _videoJobNames;

        private List<TaggedJob> videoJobs;
        [XmlIgnore]
        public List<TaggedJob> VideoJobs
        {
            get
            {
                if (videoJobs == null && _videoJobNames != null)
                    videoJobs = MainForm.Instance.Jobs.ToJobList(_videoJobNames);
                return videoJobs;
            }
            set
            {
                videoJobs = value;
            }
        }

        public FileSize DesiredSize;

        public List<string> AudioFiles;


        [XmlIgnore]
        public ContainerType Container;

        public string _ContainerName
        {
            get { return Container.ID; }
            set { Container = ContainerType.ByName(value); }
        }
    }
}