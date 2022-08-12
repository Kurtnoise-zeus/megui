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
using System.Drawing;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;

using MeGUI.core.details.video;
using MeGUI.core.gui;
using MeGUI.core.plugins.interfaces;

namespace MeGUI
{
    public partial class AudioEncodingComponent : UserControl
    {
        private List<AudioEncodingTab> tabs = new List<AudioEncodingTab>();

        #region init
        public AudioEncodingComponent()
        {
            InitializeComponent();

            if (MainForm.Instance != null)
            {
                tabs.Add(audioEncodingTab1);

                audioEncodingTab1.QueueJob = delegate(AudioJob a)
                {
                    MainForm.Instance.Jobs.AddJobsToQueue(a);
                };

                audioEncodingTab1.AudioContainer = MainForm.Instance.Settings.MainAudioFormat;
            }    
        }
        #endregion

        public void AddTab()
        {
            AudioEncodingTab a = new AudioEncodingTab();
            tabs.Add(a);
            a.AudioContainer = MainForm.Instance.Settings.MainAudioFormat;
            a.Dock = System.Windows.Forms.DockStyle.Fill;
            a.QueueJob = delegate(AudioJob aJob)
            {
                MainForm.Instance.Jobs.AddJobsToQueue(aJob);
            };

            TabPage p = new TabPage("Track " + tabs.Count);
            tabControl1.TabPages.Add(p);
            p.Controls.Add(a);
            p.Padding = tabControl1.TabPages[0].Padding;
            a.FileTypeComboBoxSize = tabs[0].FileTypeComboBoxSize; // has to go after padding
            p.UseVisualStyleBackColor = tabControl1.TabPages[0].UseVisualStyleBackColor;

        }

        /// <summary>
        /// returns the audio streams registered
        /// </summary>
        public List<AudioJob> AudioStreams
        {
            get
            {
                List<AudioJob> streams = new List<AudioJob>();
                foreach (AudioEncodingTab t in tabs)
                {
                    AudioJob a = t.AudioJob;
                    if (a != null)
                        streams.Add(a);
                }
                return streams;
            }
        }

        /// <summary>
        /// Returns null if all audio configurations are valid or incomplete. Returns
        /// an error message if any audio configuration issues a serious (not just incomplete)
        /// error message
        /// </summary>
        /// <returns></returns>
        internal string verifyAudioSettings()
        {
            foreach (AudioEncodingTab t in tabs)
            {
                AudioJob a = t.AudioJob;
                if (a == null) continue;
                string s = t.verifyAudioSettings();
                if (s != null)
                    return s;
            }
            return null;
        }

        internal void Reset()
        {
            foreach (AudioEncodingTab t in tabs)
                t.Reset();
        }

        internal void openAudioFile(params string[] files)
        {
            if (files.Length == 0)
                return;

            tabs.Clear();
            tabControl1.TabPages.Clear();

            for (int i = 0; i < files.Length; ++i)
            {
                Debug.Assert(i <= tabs.Count);

                if (i == tabs.Count)
                    AddTab();

                tabs[i].openAudioFile(files[i]);
            }
            tabControl1.SelectedIndex = files.Length - 1;
        }

        private void newTrackToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddTab();
        }

        private void removeTrackToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RemoveTab();
        }

        private void RemoveTab()
        {
            tabs.RemoveAt(tabControl1.SelectedIndex);
            tabControl1.TabPages.RemoveAt(tabControl1.SelectedIndex);

            for (int i = 0; i < tabControl1.TabPages.Count; ++i)
                tabControl1.TabPages[i].Text = "Track " + (i + 1);
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            if (tabs.Count == 1)
                removeTrackToolStripMenuItem.Enabled = false;
            else
                removeTrackToolStripMenuItem.Enabled = true;
        }
    }
}
