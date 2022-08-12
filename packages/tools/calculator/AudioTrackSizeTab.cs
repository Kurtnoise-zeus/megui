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

using MeGUI.core.details;
using MeGUI.core.util;

namespace MeGUI.packages.tools.calculator
{
    public partial class AudioTrackSizeTab : UserControl
    {
        public event EventHandler SomethingChanged;

        private bool updating = false;

        public AudioTrackSizeTab()
        {
            InitializeComponent();
            this.audio1Type.Items.AddRange(ContainerManager.AudioTypes.ValuesArray);

            DragDropUtil.RegisterSingleFileDragDrop(this, selectAudioFile, this.filter);
        }

        public void SetAudioJob(AudioJob job)
        {
            audio1Bitrate.Value = job.Settings.Bitrate;
            if (job.Type != null && audio1Type.Items.Contains(job.Type))
                audio1Type.SelectedItem = job.Type;
        }

        private long length;
        public long PlayLength
        {
            get { return length; }
            set {
                length = value;
                audio1Bitrate_ValueChanged(null, null);
            }
        }

        private void audio1Bitrate_ValueChanged(object sender, EventArgs e)
        {
            if (length <= 0)
                return;

            if (updating)
                return;
            
            updating = true;

            int bitrate = (int)audio1Bitrate.Value;
            if (bitrate > 0 && audio1Type.SelectedIndex == -1)
                audio1Type.SelectedItem = AudioType.MP3;
            double bytesPerSecond = (double)bitrate * 1000.0 / 8.0;
            FileSize f = new FileSize((ulong)(length * bytesPerSecond));
            //size.CertainValue = f;
            size.Text = f.ToString();
            raiseEvent();
            updating = false;
        }

        private void raiseEvent()
        {
            if (SomethingChanged != null)
                SomethingChanged(this, EventArgs.Empty);
        }

        private void selectAudioFile(string file)
        {
            FileSize f = FileSize.Of2(file) ?? FileSize.Empty;
            //size.CertainValue = f;
            size.Text = f.ToString();
            audio1Bitrate.Value = (length > 0) ? (long)(f.Bytes * 8) / 1000L / length : 0;
            name.Text = System.IO.Path.GetFileName(file);

            AudioType aud2Type = VideoUtil.guessAudioType(file);
            if (audio1Type.Items.Contains(aud2Type))
                audio1Type.SelectedItem = aud2Type;
        }

        private readonly string filter = VideoUtil.GenerateCombinedFilter(ContainerManager.AudioTypes.ValuesArray);

        private void selectButton_Click(object sender, EventArgs e)
        {
            openFileDialog.Filter = this.filter;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                selectAudioFile(openFileDialog.FileName);
            }
        }

        public AudioBitrateCalculationStream Stream
        {
            get
            {
                if (audio1Type.SelectedIndex > -1)
                {
                    AudioBitrateCalculationStream stream = new AudioBitrateCalculationStream();
                    stream.Type = stream.AType = audio1Type.SelectedItem as AudioType;
                    //stream.Size = size.CertainValue;
                    stream.Size = FileSize.Parse(size.Text);
                    return stream;
                }
                return null;
            }
        }

        private void size_SelectionChanged(object sender, string val)
        {
            if (length <= 0)
                return;

            if (updating)
                return;

            updating = true;

            FileSize s = FileSize.Parse(size.Text); //size.CertainValue;
            if (s > FileSize.Empty && audio1Type.SelectedIndex == -1)
                audio1Type.SelectedItem = AudioType.MP3;

            double bytesPerSecond = (double)s.Bytes / (double)length;
            int bitrate = (int)(bytesPerSecond * 8.0 / 1000.0);

            if (bitrate > audio1Bitrate.Maximum)
                audio1Bitrate.Maximum = bitrate;

            audio1Bitrate.Value = bitrate;
            raiseEvent();
            
            updating = false;
        }

        private void audio1Type_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool en = audio1Type.SelectedIndex > -1;
            audio1Bitrate.Enabled = en;
            size.Enabled = en;

            raiseEvent();
        }

        private void removeLink_LinkClicked(object sender, EventArgs e)
        {
            if (this.Parent != null) this.Parent.Controls.Remove(this);
        }

        private void AudioTrackSizeTab_Enter(object sender, EventArgs e)
        {
            selectButton.Focus();
        }

    }
}