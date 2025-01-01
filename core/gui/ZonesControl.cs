// ****************************************************************************
// 
// Copyright (C) 2005-2025 Doom9 & al
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
using System.Security.Policy;
using System.Windows.Forms;

namespace MeGUI
{
    public partial class ZonesControl : UserControl
    {
        public delegate void UpdateConfigGUI(); // handles the events triggered when a new commandline is needed

        public ZonesControl()
        {
            this.introEndFrame = 0;
            this.creditsStartFrame = 0;
            zones = Array.Empty<Zone>();
            input = "";
            InitializeComponent();
        }

        #region variables
        public event UpdateConfigGUI UpdateGUIEvent;
        private Zone[] zones;
        private string input;
        private VideoPlayer player;
        private int introEndFrame, creditsStartFrame;
        #endregion
        #region handlers
        private void UpdateGUI()
        {
            UpdateGUIEvent?.Invoke();
        }

        private void zoneMode_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (zoneMode.SelectedIndex == 0) // quantizer
            {
                this.modifierLabel.Text = "Quantizer";
                this.zoneModifier.Minimum = 0;
                this.zoneModifier.Maximum = 51;
                this.zoneModifier.Value = 26;
            }
            if (zoneMode.SelectedIndex == 1)
            {
                this.modifierLabel.Text = "Bitrate %";
                this.zoneModifier.Minimum = 1;
                this.zoneModifier.Maximum = 500;
                this.zoneModifier.Value = 100;
            }
        }

        private void removeZoneButton_Click(object sender, System.EventArgs e)
        {
            if (this.zoneListView.SelectedItems.Count > 0)
            {
                foreach (ListViewItem item in zoneListView.SelectedItems)
                {
                    zoneListView.Items.Remove(item);
                }
                Zone[] newZones = new Zone[zoneListView.Items.Count];
                int index = 0;
                foreach (ListViewItem item in zoneListView.Items)
                {
                    Zone z = (Zone)item.Tag;
                    newZones[index] = z;
                    index++;
                }
                zones = newZones;
                ShowZones(zones);
                UpdateGUI();
            }
        }

        private void clearZonesButton_Click(object sender, System.EventArgs e)
        {
            this.zoneListView.Items.Clear();
            this.zones = Array.Empty<Zone>();
            UpdateGUI();
        }

        private void addZoneButton_Click(object sender, System.EventArgs e)
        {
            if (string.IsNullOrEmpty(startFrame.Text) || string.IsNullOrEmpty(endFrame.Text))
            {
                MessageBox.Show("You must specify both start and end frame", "Missing input", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            else
            {
                Zone zone = new Zone
                {
                    startFrame = Int32.Parse(startFrame.Text),
                    endFrame = Int32.Parse(endFrame.Text)
                };
                if (zone.endFrame <= zone.startFrame)
                {
                    MessageBox.Show("The end frame must be larger than the start frame", "Invalid zone configuration", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
                if (zoneMode.SelectedIndex == 0)
                    zone.mode = ZONEMODE.Quantizer;
                else
                    zone.mode = ZONEMODE.Weight;
                zone.modifier = zoneModifier.Value;
                Zone[] newZones = new Zone[zones.Length + 1];
                int index = 0;
                if (zones.Length == 0) // no zones defined yet
                    newZones[0] = zone;
                bool iterationAborted = false, zoneInserted = false;
                foreach (Zone z in zones)
                {
                    if (zone.startFrame > z.endFrame) // new zone starts after the current one
                    {
                        newZones[index] = z;
                    }
                    else if (zone.endFrame < z.startFrame) // new zone end before the current one
                    {
                        if (!zoneInserted)
                        {
                            newZones[index] = zone;
                            zoneInserted = true;
                            index++;
                        }
                        newZones[index] = z;
                    }
                    else if (z.endFrame >= zone.startFrame || (z.startFrame <= zone.endFrame && z.endFrame >= zone.endFrame)) // intersection
                    {
                        string errorMessage = "The new zone intersects with an existing zone:\nstart:" + z.startFrame + " end: " + z.endFrame;
                        MessageBox.Show(errorMessage, "Invalid zone configuration", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        iterationAborted = true;
                        break;
                    }
                    index++;
                }
                if (!zoneInserted) // in this case the new zone comes after all the others
                    newZones[index] = zone;
                if (!iterationAborted)
                {
                    zones = newZones;
                    this.ShowZones(zones);
                }
                UpdateGUI();
            }
        }

        /// <summary>
        /// shows the zones given as argument in the GUI
        /// first clears the listview of any items, then add one item after the other
        /// </summary>
        /// <param name="zones">the zones to be displayed</param>
        private void ShowZones(Zone[] zones)
        {
            this.zoneListView.Items.Clear();
            foreach (Zone z in zones)
            {
                ListViewItem item = new ListViewItem(new string[] { z.startFrame.ToString(), z.endFrame.ToString(), z.mode.ToString(), z.modifier.ToString() })
                {
                    Tag = z
                };
                zoneListView.Items.Add(item);
            }
        }

        private void zoneListView_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (zoneListView.SelectedIndices.Count != 0)
            {
                if (zoneListView.SelectedIndices.Count == 1)
                {
                    ListViewItem item = zoneListView.SelectedItems[0];
                    Zone z = (Zone)item.Tag;
                    startFrame.Text = z.startFrame.ToString();
                    endFrame.Text = z.endFrame.ToString();
                    if (z.mode == ZONEMODE.Quantizer)
                        zoneMode.SelectedIndex = 0;
                    else
                        zoneMode.SelectedIndex = 1;
                    zoneModifier.Value = (decimal)z.modifier;
                }
            }
        }
        private void updateZoneButton_Click(object sender, System.EventArgs e)
        {
            if (this.zoneListView.SelectedIndices.Count == 1)
            {
                ListViewItem item = zoneListView.SelectedItems[0];
                Zone zone = (Zone)item.Tag;
                if (string.IsNullOrEmpty(startFrame.Text) || string.IsNullOrEmpty(endFrame.Text))
                {
                    MessageBox.Show("You must specify both start and end frame", "Missing input", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
                else
                {
                    zone.startFrame = Int32.Parse(startFrame.Text);
                    zone.endFrame = Int32.Parse(endFrame.Text);
                    if (zone.endFrame <= zone.startFrame)
                    {
                        MessageBox.Show("The end frame must be larger than the start frame", "Invalid zone configuration", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                    if (zoneMode.SelectedIndex == 0)
                        zone.mode = ZONEMODE.Quantizer;
                    else
                        zone.mode = ZONEMODE.Weight;
                    zone.modifier = zoneModifier.Value;
                    zones[item.Index] = zone;
                    this.ShowZones(zones);
                    UpdateGUI();
                }
            }
        }
        #endregion
        #region preview
        /// <summary>
        /// handler for the ZoneSet event
        /// updates zone start / end and adds the zone
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        private void player_ZoneSet(int start, int end)
        {
            startFrame.Text = start.ToString();
            endFrame.Text = end.ToString();
            addZoneButton_Click(null, null);
        }

        /// <summary>
        /// handler for the WindowClosed event
        /// sets the player back to null so that the next time preview is pressed
        /// we open a new window
        /// </summary>
        private void player_Closed(object sender, EventArgs e)
        {
            player = null;
        }
        private void showVideoButton_Click(object sender, System.EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.input))
            {
                if (player == null)
                {
                    player = new VideoPlayer();
                    bool videoLoaded = player.LoadVideo(input, PREVIEWTYPE.ZONES, false);
                    if (videoLoaded)
                    {
                        player.ZoneSet += new ZoneSetCallback(player_ZoneSet);
                        player.Closed += new EventHandler(player_Closed);
                        if (introEndFrame > 0)
                            player.IntroEnd = this.introEndFrame;
                        if (creditsStartFrame > 0)
                            player.CreditsStart = this.creditsStartFrame;
                        player.Show();
                        player.SetScreenSize();
                    }
                    else
                        return;
                }
                if (zoneListView.SelectedItems.Count == 1) // a zone has been selected, show that zone
                {
                    Zone zone = (Zone)zoneListView.SelectedItems[0].Tag;
                    player.ZoneStart = zone.startFrame;
                    player.ZoneEnd = zone.endFrame;
                }
                else // no zone has been selected.. but if start and / or end frame have been configured show them in the preview
                {
                    if (!string.IsNullOrEmpty(startFrame.Text))
                    {
                        player.ZoneStart = Int32.Parse(startFrame.Text);
                    }
                    if (!string.IsNullOrEmpty(endFrame.Text))
                    {
                        player.ZoneEnd = Int32.Parse(endFrame.Text);
                    }
                }
            }
            else
                MessageBox.Show("Please configure video input first", "No video input found", MessageBoxButtons.OK, MessageBoxIcon.Stop);
        }
        #endregion
        #region properties
        /// <summary>
        /// Gets or sets the ending frame of the intro section
        /// </summary>
        public int IntroEndFrame
        {
            get { return introEndFrame; }
            set { introEndFrame = value; }
        }

        /// <summary>
        /// Gets or sets the starting frame of the credits.
        /// </summary>
        public int CreditsStartFrame
        {
            get { return creditsStartFrame; }
            set { creditsStartFrame = value; }
        }
        
        /// <summary>
        /// Gets or sets the currently configured zones. Setting triggers a display update.
        /// </summary>
        public Zone[] Zones
        {
            get { return zones; }
            set { zones = value; ShowZones(zones); }
        }
        /// <summary>
        /// Sets the input video file, used for previewing
        /// </summary>
        public string Input
        {
            set { input = value; showVideoButton.Enabled = !string.IsNullOrEmpty(value); }
            get { return input; }
        }
        #endregion

        public void ClosePlayer()
        {
            player?.Close();
        }

        private void ZonesControl_Load(object sender, EventArgs e)
        {
            zoneMode.SelectedIndex = 0;
        }
    }
}
