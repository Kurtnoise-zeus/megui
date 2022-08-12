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

using MeGUI.core.util;

namespace MeGUI
{

    public delegate void StringChanged(object sender, string val);
    public delegate void IntChanged(object sender, int val);
    public class VideoInfo
    {
        private string videoInput;
        public event StringChanged VideoInputChanged;
        public string VideoInput
        {
            get { return videoInput; }
            set { videoInput = value; VideoInputChanged(this, value); }
        }

        private string videoOutput;
        public event StringChanged VideoOutputChanged;
        public string VideoOutput
        {
            get { return videoOutput; }
            set { videoOutput = value; VideoOutputChanged(this, value); }
        }

        private Dar? dar = null;
        public Dar? DAR
        {
            get { return dar; }
            set { dar = value; }
        }
        
        private int introEndFrame;
        public int IntroEndFrame
        {
            get { return introEndFrame; }
            set { introEndFrame = value; }
        }

        private int creditsStartFrame;
        public int CreditsStartFrame
        {
            get { return creditsStartFrame; }
            set { creditsStartFrame = value; }
        }

        private Zone[] zones;
        public Zone[] Zones
        {
            get { return zones; }
            set { zones = value ?? new Zone[0]; }
        }

        public VideoInfo(string videoInput, string videoOutput, int darX, int darY, int creditsStartFrame, int introEndFrame, Zone[] zones)
        {
            this.videoInput = videoInput;
            this.videoOutput = videoOutput;
            this.creditsStartFrame = creditsStartFrame;
            this.introEndFrame = introEndFrame;
            this.zones = zones;
        }

        public VideoInfo()
            : this("", "", -1, -1, -1, -1, null) { }

        internal VideoInfo Clone()
        {
            return (VideoInfo)this.MemberwiseClone();
        }
    }
}