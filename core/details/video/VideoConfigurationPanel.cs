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
using System.Windows.Forms;

namespace MeGUI.core.details.video
{
    public partial class VideoConfigurationPanel : UserControl
    {
        #region variables
        protected bool updating = false;
        private double bytesPerFrame;
        protected VideoCodecSettings.VideoEncodingMode lastEncodingMode = 0;
        protected int lastBitrateEncodingValue = 1000;
        protected int lastQuantizerEncodingValue = 32;

        private bool loaded;
        #endregion

        #region start / stop
        public VideoConfigurationPanel()
            : this(null, new VideoInfo())
        { }


        public VideoConfigurationPanel(MainForm mainForm, VideoInfo info)
        {
            loaded = false;
            InitializeComponent();
        }

        private void VideoConfigurationPanel_Load(object sender, EventArgs e)
        {
            loaded = true;
            doCodecSpecificLoadAdjustments();
            genericUpdate();
        }

        #endregion
        #region codec specific adjustments

        /// <summary>
        /// Generates the commandline
        /// </summary>
        /// <returns></returns>
        protected virtual string getCommandline() {
            return "";
        }

        /// <summary>
        /// The method by which codecs can do their pre-commandline generation adjustments (eg tri-state adjustment).
        /// </summary>
        protected virtual void doCodecSpecificAdjustments() { }

        /// <summary>
        /// The method by which codecs can add things to the Load event
        /// </summary>
        protected virtual void doCodecSpecificLoadAdjustments() { }
        
        /// <summary>
        /// Returns whether settings is a valid settings object for this instance. Should be implemented by one line:
        /// return (settings is xxxxSettings);
        /// </summary>
        /// <param name="settings">The settings to check</param>
        /// <returns>Whether the settings are valid</returns>
        protected virtual bool isValidSettings(VideoCodecSettings settings)
        {
            throw new Exception("A bug in the program -- ProfilableConfigurationDialog.isValidSettings(GenericSettings) is not overridden");
        }

        /// <summary>
        /// Returns a new instance of the codec settings. This must be specific to the type of the config dialog, so
        /// that it can be set with the Settings.set property.
        /// </summary>
        /// <returns>A new instance of xxxSettings</returns>
        protected virtual VideoCodecSettings defaultSettings()
        {
            throw new Exception("A bug in the program -- ProfilableConfigurationDialog.defaultSettings() is not overridden");
        }
        #endregion
        #region showCommandline
        protected void showCommandLine()
        {
            if (!loaded)
                return;
            if (updating)
                return;
            updating = true;

            doCodecSpecificAdjustments();

            this.commandline.Text = getCommandline();
            updating = false;
        }
        #endregion
        #region GUI events
        protected void genericUpdate()
        {
            showCommandLine();
        }
        #endregion
        #region properties

        public double BytesPerFrame
        {
            get { return bytesPerFrame; }
            set { bytesPerFrame = value; }
        }

        #endregion
    }
}