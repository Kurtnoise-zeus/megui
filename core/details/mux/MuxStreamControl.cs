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

using MeGUI.core.util;

namespace MeGUI.core.details.mux
{
    public partial class MuxStreamControl : UserControl
    {
        public MuxStreamControl()
        {
            InitializeComponent();
            subtitleLanguage.Items.AddRange(new List<string>(LanguageSelectionContainer.Languages.Keys).ToArray());
        }

        private TrackInfo _trackInfo;
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public MuxStream Stream
        {
            get
            {
                if (string.IsNullOrEmpty(input.Filename))
                    return null;

                int iDelay = 0;
                if (showDelay)
                    iDelay = (int)audioDelay.Value;
                bool bDefault = false;
                if (showDefaultSubtitleStream)
                    bDefault = chkDefaultStream.Checked;
                bool bForce = false;
                if (showForceSubtitleStream)
                    bForce = chkForceStream.Checked;

                return new MuxStream(input.Filename, subtitleLanguage.Text, subName.Text, iDelay, bDefault, bForce, _trackInfo);
            }

            set
            {
                if (value == null)
                {
                    removeSubtitleTrack_Click(null, null);
                    return;
                }

                input.Filename = value.path;
                if (!string.IsNullOrEmpty(value.language))
                {
                    if (!String.IsNullOrEmpty(LanguageSelectionContainer.LookupISOCode(value.language)))
                        subtitleLanguage.Text = LanguageSelectionContainer.LookupISOCode(value.language);
                    else
                        subtitleLanguage.Text = value.language;
                }
                subName.Text = value.name;
                audioDelay.Value = value.delay;
                chkDefaultStream.Checked = value.bDefaultTrack;
                chkForceStream.Checked = value.bForceTrack;
                _trackInfo = value.MuxOnlyInfo;
            }
        }

        private bool showDelay;
        public bool ShowDelay
        {
            set
            {
                showDelay = value;
                delayLabel.Visible = value;
                audioDelay.Visible = value;
            }
            get
            {
                return showDelay;
            }
        }

        private bool showDefaultSubtitleStream;
        public bool ShowDefaultSubtitleStream
        {
            set
            {
                showDefaultSubtitleStream = value;
                chkDefaultStream.Visible = value;
            }
            get
            {
                return showDefaultSubtitleStream;
            }
        }

        private bool showForceSubtitleStream;
        public bool ShowForceSubtitleStream
        {
            set
            {
                showForceSubtitleStream = value;
                chkForceStream.Visible = value;
            }
            get
            {
                return showForceSubtitleStream;
            }
        }

        public string Filter
        {
            get { return input.Filter; }
            set { input.Filter = value; }
        }

        public void SetLanguage(string lang)
        {
            subtitleLanguage.SelectedItem = lang;
        }

        public void SetAutoEncodeMode()
        {
            audioDelay.Enabled = input.Enabled = false;
        }

        private void removeSubtitleTrack_Click(object sender, EventArgs e)
        {
            input.Text = "";
            subtitleLanguage.SelectedIndex = -1;
            subName.Text = "";
            audioDelay.Value = 0;
            raiseEvent();
        }

        private void raiseEvent()
        {
            if (FileUpdated != null)
                FileUpdated(this, new EventArgs());
        }

        public event EventHandler FileUpdated;

        private void input_FileSelected(FileBar sender, FileBarEventArgs args)
        {
            audioDelay.Value = PrettyFormatting.getDelayAndCheck(input.Filename) ?? 0;

            string strLanguage = LanguageSelectionContainer.GetLanguageFromFileName(System.IO.Path.GetFileNameWithoutExtension(input.Filename));
            if (!String.IsNullOrEmpty(strLanguage))
            {
                SetLanguage(strLanguage);
            }
            else if (input.Filename.ToLowerInvariant().EndsWith(".idx"))
            {
                List<SubtitleInfo> subTracks;
                idxReader.readFileProperties(input.Filename, out subTracks);
                if (subTracks.Count > 0)
                    SetLanguage(LanguageSelectionContainer.LookupISOCode(subTracks[0].Name));
            }

            raiseEvent();
        }

        private void chkForceStream_CheckedChanged(object sender, EventArgs e)
        {
            subName.Text = SubtitleUtil.ApplyForcedStringToTrackName(chkForceStream.Checked, subName.Text);
        }
    }
}