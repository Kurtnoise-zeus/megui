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
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

using MeGUI.core.util;

namespace MeGUI.core.gui
{
    public partial class WorkerSummary : Form
    {
        Dictionary<string, IndividualWorkerSummary> displays = new Dictionary<string, IndividualWorkerSummary>();
        private Timer _GUIUpdateTimer = new Timer();

        public WorkerSummary()
        {
            InitializeComponent();
            panel1.Controls.Clear(); // they're just there for the designer
            panel1.Dock = DockStyle.Top;
            int width = panel1.Width;
            panel1.Dock = DockStyle.None;
            panel1.Height = 0;
            panel1.Width = width;
            panel1.Location = new Point(0, 0);
            panel1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

            _GUIUpdateTimer.Tick += new EventHandler(TimerEventProcessor);
            _GUIUpdateTimer.Interval = 5000;
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
                _GUIUpdateTimer.Dispose();
            }
            base.Dispose(disposing);
        }

        private void TimerEventProcessor(Object myObject, EventArgs myEventArgs)
        {
    RefreshInfo();
}

        public void RefreshInfo()
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(RefreshInfo));
                return;
            }
            if (!Visible) 
                return;
            foreach (IndividualWorkerSummary i in displays.Values)
                i.RefreshInfo();
        }

        public void Add(JobWorker w)
        {
            IndividualWorkerSummary i = new IndividualWorkerSummary();
            i.Worker = w;
            i.Dock = DockStyle.Bottom;
            Util.ThreadSafeRun(panel1, delegate { panel1.Controls.Add(i); });
            displays[w.Name] = i;
            RefreshInfo();
        }

        public void Remove(string name)
        {
            Util.ThreadSafeRun(panel1, delegate { panel1.Controls.Remove(displays[name]); });
            displays.Remove(name);
            RefreshInfo();
        }

        private void WorkerSummary_VisibleChanged(object sender, EventArgs e)
        {
            if (Visible)
            { 
                RefreshInfo();
                _GUIUpdateTimer.Start();
            }
            else
                _GUIUpdateTimer.Stop();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (e != null)
                e.Cancel = true;
            Hide();
        }
    }
}