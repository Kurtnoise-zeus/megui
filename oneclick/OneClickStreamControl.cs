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
using System.ComponentModel;
using System.Windows.Forms;

using MeGUI.core.gui;
using MeGUI.core.util;

namespace MeGUI
{
    public partial class OneClickStreamControl : UserControl
    {
        public event EventHandler SomethingChanged;

        public OneClickStreamControl()
        {
            InitializeComponent();
            subtitleLanguage.Items.AddRange(new List<string>(LanguageSelectionContainer.Languages.Keys).ToArray());
            cbEncodingMode.Items.AddRange(EnumProxy.CreateArray(OneClickSettings.SupportedModes));
            initProfileHandler();
        }

        public void enableDragDrop()
        {
            // only enable drag&drop when the main form is visible
            DragDropUtil.RegisterSingleFileDragDrop(input, setFileName, delegate() { return Filter; });
        }

        public void initProfileHandler()
        {
            if (MainForm.Instance != null)
                encoderProfile.Manager = MainForm.Instance.Profiles;
        }

        public void SelectProfileNameOrWarn(string fqname)
        {
            encoderProfile.SetProfileNameOrWarn(fqname);
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public AudioEncodingMode EncodingMode
        {
            get { return (AudioEncodingMode)cbEncodingMode.SelectedIndex; }
            set { cbEncodingMode.SelectedIndex = (int)value; }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public AudioCodecSettings EncoderProfileSettings
        {
            get { return (AudioCodecSettings)encoderProfile.SelectedProfile.BaseSettings; }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string EncoderProfile
        {
            get { return encoderProfile.SelectedProfile.FQName; }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public OneClickStream Stream
        {
            get
            {
                if (string.IsNullOrEmpty(input.SelectedText))
                    return null;

                return new OneClickStream(input.SelectedText, TrackType.Unknown, null, null, 0, subtitleLanguage.Text, subName.Text, (int)delay.Value, chkDefaultStream.Checked, chkForceStream.Checked, (AudioCodecSettings)encoderProfile.SelectedProfile.BaseSettings, (AudioEncodingMode)cbEncodingMode.SelectedIndex);
            }

            set
            {
                if (value == null)
                {
                    removeTrack_Click(null, null);
                    return;
                }

                NiceComboBoxItem oItem = new NiceComboBoxItem(value.DemuxFilePath, value);
                input.SelectedItem = oItem;
                if (!string.IsNullOrEmpty(value.Language))
                {
                    if (!String.IsNullOrEmpty(LanguageSelectionContainer.LookupISOCode(value.Language)))
                        subtitleLanguage.Text = LanguageSelectionContainer.LookupISOCode(value.Language);
                    else
                        subtitleLanguage.Text = value.Language;
                }
                else
                    subtitleLanguage.Text = MainForm.Instance.Settings.DefaultLanguage1;
                subName.Text = value.Name;
                delay.Value = value.Delay;
                chkDefaultStream.Checked = value.DefaultStream;
                chkForceStream.Checked = value.ForcedStream;
            }
        }

        private bool showDelay;
        public bool ShowDelay
        {
            set
            {
                showDelay = value;
                delayLabel.Visible = value;
                delay.Visible = value;
                if (!value) 
                    delay.Value = 0;
            }
            get
            {
                return showDelay;
            }
        }

        private bool showDefaultStream;
        public bool ShowDefaultStream
        {
            set
            {
                showDefaultStream = value;
                chkDefaultStream.Visible = value;
                if (!value) 
                    chkDefaultStream.Checked = false;
            }
            get
            {
                return showDefaultStream;
            }
        }

        private bool showForceStream;
        public bool ShowForceStream
        {
            set
            {
                showForceStream = value;
                chkForceStream.Visible = value;
                if (!value)
                    chkForceStream.Checked = false;
            }
            get
            {
                return showForceStream;
            }
        }

        private int trackNumber = 0;
        public int TrackNumber
        {
            get { return trackNumber; }
            set { trackNumber = value; }
        }

        public string Filter
        {
            get { return input.Filter; }
            set { input.Filter = value; }
        }

        public object[] StandardStreams
        {
            get { return input.StandardItems; }
            set { input.StandardItems = value; }
        }

        public object[] CustomStreams
        {
            get { return input.CustomItems; }
            set { input.CustomItems = value; }
        }

        /// <summary>
        /// Index of the selected item, or -1 if the selected item isn't on the list
        /// or if it is in a submenu.
        /// </summary>
        public int SelectedStreamIndex
        {
            get { return input.SelectedIndex; }
            set { input.SelectedIndex = value; }
        }

        /// <summary>
        /// Selected Item
        /// </summary>
        public SCItem SelectedItem
        {
            get { return input.SelectedSCItem; }
        }

        /// <summary>
        /// Selected Stream
        /// </summary>
        public OneClickStream SelectedStream
        {
            get 
            {
                if (input.SelectedObject != null && !String.IsNullOrEmpty(input.SelectedText) && !input.SelectedText.Equals("None") && input.SelectedSCItem.IsStandard)
                {
                    OneClickStream oStream = (OneClickStream)input.SelectedObject;
                    oStream.Language = subtitleLanguage.Text;
                    oStream.Name = subName.Text;
                    oStream.Delay = (int)delay.Value;
                    oStream.DefaultStream = ((OneClickStream)input.SelectedObject).DefaultStream;
                    oStream.ForcedStream = ((OneClickStream)input.SelectedObject).ForcedStream;
                    oStream.EncoderSettings = (AudioCodecSettings)encoderProfile.SelectedProfile.BaseSettings;
                    oStream.EncodingMode = (AudioEncodingMode)cbEncodingMode.SelectedIndex;
                    oStream.TrackInfo.OneClickTrackNumber = trackNumber;
                    return oStream;
                }
                else
                    return new OneClickStream(input.SelectedText, TrackType.Unknown, null, null, 0, subtitleLanguage.Text, subName.Text, (int)delay.Value, chkDefaultStream.Checked, chkForceStream.Checked, (AudioCodecSettings)encoderProfile.SelectedProfile.BaseSettings, (AudioEncodingMode)cbEncodingMode.SelectedIndex);
            }
        }

        /// <summary>
        /// Selected File
        /// </summary>
        public string SelectedFile
        {
            get { return input.SelectedText; }
        }

        public void SetLanguage(string lang)
        {
            subtitleLanguage.SelectedItem = lang;
        }

        private void removeTrack_Click(object sender, EventArgs e)
        {
            input.Text = "";
            subtitleLanguage.SelectedIndex = -1;
            subName.Text = "";
            delay.Value = 0;
            raiseEvent();
        }

        private void raiseEvent()
        {
            if (FileUpdated != null)
                FileUpdated(this, new EventArgs());

            if (SomethingChanged != null)
                SomethingChanged(this, null);
        }

        private void chkForceStream_CheckedChanged(object sender, EventArgs e)
        {
            if (input.SelectedObject is OneClickStream)
                ((OneClickStream)input.SelectedObject).ForcedStream = chkForceStream.Checked;
            subName.Text = SubtitleUtil.ApplyForcedStringToTrackName(chkForceStream.Checked, subName.Text);
        }

        private void chkDefaultStream_CheckedChanged(object sender, EventArgs e)
        {
            if (input.SelectedObject is OneClickStream)
                ((OneClickStream)input.SelectedObject).DefaultStream = chkDefaultStream.Checked;
        }

        private void setFileName(string strFileName)
        {
            input.AddCustomItem(strFileName);
            input.SelectedObject = strFileName;
        }

        public event EventHandler FileUpdated;
        private void input_SelectionChanged(object sender, string val)
        {
            // get language
            bool bFound = false;
            string strLanguage = LanguageSelectionContainer.GetLanguageFromFileName(System.IO.Path.GetFileNameWithoutExtension(input.SelectedText));
            if (!String.IsNullOrEmpty(strLanguage))
            {
                SetLanguage(strLanguage);
                bFound = true;
            }
            else if (input.SelectedText.ToLowerInvariant().EndsWith(".idx"))
            {
                List<SubtitleInfo> subTracks;
                idxReader.readFileProperties(input.SelectedText, out subTracks);
                if (subTracks.Count > 0)
                {
                    SetLanguage(LanguageSelectionContainer.LookupISOCode(subTracks[0].Name));
                    bFound = true;
                }
            }
            if (!bFound && this.SelectedItem != null && this.SelectedStreamIndex > 0)
                SetLanguage(MainForm.Instance.Settings.DefaultLanguage1);

            // get delay & track name
            delay.Value = 0;
            if (this.SelectedItem != null && this.SelectedStreamIndex > 0 && this.SelectedItem.Tag is OneClickStream)
            {
                delay.Value = ((OneClickStream)this.SelectedItem.Tag).Delay;
                subName.Text = ((OneClickStream)this.SelectedItem.Tag).Name;
            }
            if (PrettyFormatting.getDelayAndCheck(input.SelectedText) != null)
                delay.Value = PrettyFormatting.getDelayAndCheck(input.SelectedText) ?? 0;                

            if (showDefaultStream)
                chkDefaultStream.Checked = this.SelectedStream.DefaultStream;

            if (showForceStream)
                chkForceStream.Checked = this.SelectedStream.ForcedStream;

            raiseEvent();
        }

        private void encoderProfile_SelectedProfileChanged(object sender, EventArgs e)
        {
            raiseEvent();
        }

        private void cbEncodingMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            encoderProfile.Enabled = !cbEncodingMode.SelectedText.Equals("never");
            raiseEvent();
        }
    }
}
