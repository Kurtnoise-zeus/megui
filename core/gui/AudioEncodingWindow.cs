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

using System.Windows.Forms;

using MeGUI.core.details;

namespace MeGUI.core.gui
{
    public partial class AudioEncodingWindow : Form
    {
        public static readonly IDable<ReconfigureJob> Configurer = new IDable<ReconfigureJob>(
            "audio_reconfigure", delegate(Job j)
        {
            if (!(j is AudioJob))
                return null;

            AudioEncodingWindow w = new AudioEncodingWindow();
            w.audioEncodingTab1.AudioJob = (AudioJob)j;

            if (w.ShowDialog() == DialogResult.OK)
                j = w.audioEncodingTab1.AudioJob;

            return j;
        });

        public AudioEncodingWindow()
        {
            InitializeComponent();
            audioEncodingTab1.QueueJob = delegate(AudioJob j)
            {
                this.DialogResult = DialogResult.OK;
            };
        }
    }
}