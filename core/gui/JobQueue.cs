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
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

using MeGUI.core.details;
using MeGUI.core.util;

namespace MeGUI.core.gui
{
    internal delegate Pair<string, MultiJobHandler>[] MultiJobMenuGenerator();
    internal delegate void JobChangeEvent(object sender, JobQueueEventArgs info);
    internal delegate void RequestJobDeleted(TaggedJob jobs);
    internal delegate void SingleJobHandler(TaggedJob job);
    internal delegate void MultiJobHandler(List<TaggedJob> jobs);
    public enum StartStopMode { Start, Stop };
    public enum Dependencies { DeleteAll, RemoveDependencies }

    public partial class JobQueue : UserControl
    {
        private Dictionary<string, TaggedJob> jobs = new Dictionary<string, TaggedJob>();
        private List<ToolStripItem> singleJobHandlers = new List<ToolStripItem>();
        private List<ToolStripItem> multiJobHandlers = new List<ToolStripItem>();
        private List<Pair<ToolStripMenuItem, MultiJobMenuGenerator>> menuGenerators = new List<Pair<ToolStripMenuItem, MultiJobMenuGenerator>>();
        private StartStopMode startStopMode;

        [Browsable(false)]
        public StartStopMode StartStopMode
        {
            get { return startStopMode; }
            set
            {
                if (InvokeRequired)
                {
                    Invoke(new MethodInvoker(delegate { StartStopMode = value; }));
                    return;
                }

                startStopMode = value;
                switch (value)
                {
                    case StartStopMode.Start:
                        startStopButton.Text = "Start";;
                        break;

                    case StartStopMode.Stop:
                        startStopButton.Text = "Stop";;
                        break;
                }
            }
        }

        #region public interface: jobs
        [Browsable(false)]
        internal IEnumerable<TaggedJob> JobList
        {
            get
            {
                if (InvokeRequired)
                {
                    return (IEnumerable<TaggedJob>)Invoke(new Getter<IEnumerable<TaggedJob>>(delegate { return JobList; }));
                }

                TaggedJob[] jobList = new TaggedJob[jobs.Count];
                foreach (TaggedJob j in jobs.Values)
                {
                    jobList[indexOf(j)] = j;
                }
                return jobList;
            }

            set
            {
                if (InvokeRequired)
                {
                    Invoke(new MethodInvoker(delegate { JobList = value; }));
                    return;
                }

                queueListView.BeginUpdate();
                queueListView.Items.Clear();
                foreach (TaggedJob job in value)
                {
                    queueListView.Items.Add(new ListViewItem(new string[] { job.Name, "", "", "", "", "", "", "", "", "" }));
                    jobs[job.Name] = job;
                }
                queueListView.EndUpdate();
                RefreshQueue();
            }
        }

        internal void QueueJob(TaggedJob j)
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(delegate { QueueJob(j); }));
                return;
            }

            queueListView.Items.Add(new ListViewItem(new string[] { j.Name, "", "", "", "", "", "", "", "", "" }));
            jobs[j.Name] = j;
            RefreshQueue();
        }

        internal void RemoveJobFromQueue(TaggedJob job)
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(delegate { RemoveJobFromQueue(job); }));
                return;
            }

            queueListView.Items[indexOf(job)].Remove();
            jobs.Remove(job.Name);
            queueListView.Refresh();
        }
        #endregion

        #region adding GUI elements
        

        private void AddItem(ToolStripMenuItem item, string parent)
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(delegate { AddItem(item, parent); }));
                return;
            }

            if (parent == null)
                queueContextMenu.Items.Add(item);
            else
                foreach (ToolStripMenuItem i in queueContextMenu.Items)
                    if (i.Text == parent)
                    {
                        i.DropDownItems.Add(item);
                        break;
                    }
        }

        internal void AddDynamicSubMenu(string name, string parent, MultiJobMenuGenerator gen)
        {
            ToolStripMenuItem item = new ToolStripMenuItem(name);
            menuGenerators.Add(new Pair<ToolStripMenuItem, MultiJobMenuGenerator>(item, gen));
            AddItem(item, parent);
        }

        public void AddMenuItem(string name, string parent)
        {
            AddItem(new ToolStripMenuItem(name), parent);
        }

        internal void AddMenuItem(string name, string parent, SingleJobHandler handler)
        {
            ToolStripMenuItem item = new ToolStripMenuItem(name);
            item.Click += delegate(object sender, EventArgs e)
            {
                Debug.Assert(queueListView.SelectedItems.Count == 1);
                TaggedJob j = jobs[queueListView.SelectedItems[0].Text];
                handler(j);
            };
            singleJobHandlers.Add(item);
            AddItem(item, parent);
        }

        internal void AddMenuItem(string name, string parent, MultiJobHandler handler)
        {
            ToolStripMenuItem item = new ToolStripMenuItem(name);
            item.Click += delegate(object sender, EventArgs e)
            {
                Debug.Assert(queueListView.SelectedItems.Count > 0);
                List<TaggedJob> list = new List<TaggedJob>();
                foreach (ListViewItem i in queueListView.SelectedItems)
                    list.Add(jobs[i.Text]);
                handler(list);
            };
            AddItem(item, parent);
            multiJobHandlers.Add(item);
        }

        public void AddButton(string name, EventHandler handler)
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(delegate { AddButton(name, handler); }));
                return;
            }

            Button b = new Button();
            b.Text = name;
            b.Click += handler;
            b.AutoSize = true;
            b.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flowLayoutPanel1.Controls.Add(b);
        }

        public void SetStartStopButtonsTogether()
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(delegate { SetStartStopButtonsTogether(); }));
                return;
            }

            flowLayoutPanel1.Controls.Remove(stopButton);
        }


        #endregion

        #region indexOf
        private int indexOf(TaggedJob j)
        {
            Debug.Assert(jobs.ContainsKey(j.Name), "Looking for a job which isn't in the jobs dictionary");
            foreach (ListViewItem i in queueListView.Items)
            {
                if (i.Text == j.Name)
                {
                    int index = i.Index;
                    Debug.Assert(index >= 0);
                    return index;
                }
            }
            Debug.Assert(false, "Couldn't find job in the GUI queue");
            throw new Exception();
        }
        #endregion

        public JobQueue()
        {
            InitializeComponent();
            StartStopMode = StartStopMode.Start;            
            this.LoadComponentSettings();
        }

        #region job deletion
        internal RequestJobDeleted RequestJobDeleted;
        public event EventHandler AbortClicked;
        public event EventHandler StartClicked;
        public event EventHandler StopClicked;

        private List<TaggedJob> RemoveAllDependantJobsFromQueue(TaggedJob job)
        {
            RemoveJobFromQueue(job);
            List<TaggedJob> list = new List<TaggedJob>();
            foreach (TaggedJob j in job.EnabledJobs)
            {
                if (jobs.ContainsKey(j.Name))
                    list.AddRange(RemoveAllDependantJobsFromQueue(j));
                else
                    list.Add(j);
            }
            return list;
        }

        private void DeleteJobButton_Click(object sender, EventArgs e)
        {
            if (queueListView.SelectedItems.Count <= 0) 
                return;

            foreach (ListViewItem item in this.queueListView.SelectedItems)
            {
                ProcessUserRequestToDelete(item.Text);
            }
        }

        private void ProcessUserRequestToDelete(string name)
        {
            if (!jobs.ContainsKey(name)) // Check if it has already been deleted
                return;
            TaggedJob job = jobs[name];
            if (job == null) 
                return;
            RequestJobDeleted(job);
        }
        #endregion

        #region list movement
        enum Direction { Up, Down }
        private void DownButton_Click(object sender, EventArgs e)
        {
            MoveListViewItem(Direction.Down);
        }

        private void UpButton_Click(object sender, EventArgs e)
        {
            MoveListViewItem(Direction.Up);
        }

        /// <summary>
        /// moves the currently selected listviewitem up/down
        /// adapted from code by Less Smith @ KnotDot.Net
        /// </summary>
        /// <param name="lv">reference to ListView</param>
        /// <param name="moveUp">whether the currently selected item should be moved up or down</param>
        private void MoveListViewItem(Direction d)
        {
            // We can trust that the button will be disabled unless this condition is met
            Debug.Assert(IsSelectionMovable(d));

            ListView lv = queueListView;
            ListView.ListViewItemCollection items = lv.Items;

            int[] indices = new int[lv.SelectedIndices.Count];
            lv.SelectedIndices.CopyTo(indices, 0);
            Array.Sort(indices);
            int min = indices[0];
            int max = indices[indices.Length - 1];

            lv.BeginUpdate();
            if (d == Direction.Up)
            {
                items[max].Selected = false;
                items[min - 1].Selected = true;

                for (int i = min; i <= max; i++)
                    SwapContents(items[i], items[i - 1]);
            }
            else if (d == Direction.Down)
            {
                items[min].Selected = false;
                items[max + 1].Selected = true;

                for (int i = max; i >= min; i--)
                    SwapContents(items[i], items[i + 1]);
            }
            lv.EndUpdate();
            lv.Refresh();

        }

        /// <summary>
        /// swaps the contents of the two items
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        private void SwapContents(ListViewItem a, ListViewItem b)
        {
            for (int i = 0; i < a.SubItems.Count; i++)
            {
                string cache = b.SubItems[i].Text;
                b.SubItems[i].Text = a.SubItems[i].Text;
                a.SubItems[i].Text = cache;
            }
        }

        /// <summary>
        /// Tells if the current selection can be moved in direction d.
        /// Checks:
        ///     whether it's at the top / bottom
        ///     if anything is actually selected
        ///     whether the selection is contiguous
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        private bool IsSelectionMovable(Direction d)
        {
            ListView lv = queueListView;
            int[] indices = new int[lv.SelectedIndices.Count];
            lv.SelectedIndices.CopyTo(indices, 0);
            Array.Sort(indices);

            if (indices.Length == 0)
                return false;
            if (d == Direction.Up && indices[0] == 0)
                return false;
            if (d == Direction.Down &&
                indices[indices.Length - 1] == queueListView.Items.Count - 1)
                return false;
            if (!ConsecutiveIndices(indices))
                return false;

            return true;
        }

        /// <summary>
        /// Tells if the current selection can be edited.
        /// Checks:
        ///     if one job actually selected
        ///     whether the selected job is waiting or postponed
        /// </summary>
        /// <returns></returns>
        private bool IsSelectionEditable()
        {
            if (queueListView.SelectedItems.Count != 1)
                return false;

            TaggedJob job = jobs[queueListView.SelectedItems[0].Text];
            if (job.Status != JobStatus.WAITING && job.Status != JobStatus.POSTPONED)
                return false;

            return true;
        }

        /// <summary>
        /// Tells if the given list of indices is consecutive; if so, sets min and 
        /// max to the min and max indices
        /// </summary>
        /// <param name="indices"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        private bool ConsecutiveIndices(int[] indices)
        {
            Debug.Assert(indices.Length > 0);

            int last = indices[0] - 1;
            foreach (int i in indices)
            {
                if (i != last + 1) return false;
                last = i;
            }

            return true;
        }

        /// <summary>
        /// Updates the up/down buttons according to whether the selection CAN be moved up/down
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void QueueListView_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            upButton.Enabled = IsSelectionMovable(Direction.Up);
            downButton.Enabled = IsSelectionMovable(Direction.Down);
            editJobButton.Enabled = IsSelectionEditable();
            MarkDependentJobs();
        }

        #endregion

        #region load/update
        private void editJobButton_Click(object sender, EventArgs e)
        {
            bool bJobCanBeEdited = false;

            if (queueListView.SelectedItems.Count != 1)
                return;

            TaggedJob job = jobs[queueListView.SelectedItems[0].Text];
            if (job.Status != JobStatus.WAITING && job.Status != JobStatus.POSTPONED)
                return;

            foreach (IDable<ReconfigureJob> i in MainForm.Instance.PackageSystem.JobConfigurers.Values)
            {
                Job j = i.Data(job.Job);
                if (j != null)
                {
                    bJobCanBeEdited = true;
                    job.Job = j;
                }
            }

            if (!bJobCanBeEdited)
                MessageBox.Show("This kind of job cannot be edited.", "Cannot edit job", MessageBoxButtons.OK, MessageBoxIcon.Warning);

        }
        #endregion

        #region contextmenu events
        /// <summary>
        /// Returns true if all selected jobs have the requested status
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        private bool AllJobsHaveStatus(JobStatus status)
        {
            if (this.queueListView.SelectedItems.Count <= 0)
            {
                return false;
            }
            foreach (ListViewItem item in this.queueListView.SelectedItems)
            {
                TaggedJob job = jobs[item.Text];
                if (job.Status != status)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Returns true if any selected jobs have the requested status
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        private bool AnyJobsHaveStatus(JobStatus status)
        {
            foreach (ListViewItem item in this.queueListView.SelectedItems)
            {
                TaggedJob job = jobs[item.Text];
                if (job.Status == status)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// handles the doubleclick event for the listview
        /// changes the job status from waiting -> postponed to waiting
        /// from done -> waiting -> postponed -> waiting
        /// from error -> waiting -> postponed -> waiting
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void QueueListView_DoubleClick(object sender, EventArgs e)
        {
            if (this.queueListView.SelectedItems.Count > 0) // otherwise 
            {
                int position = this.queueListView.SelectedItems[0].Index;
                TaggedJob job = jobs[this.queueListView.SelectedItems[0].Text];
                if (job.Status == JobStatus.PROCESSING || job.Status == JobStatus.PAUSED || job.Status == JobStatus.ABORTING) // job is being processed -> do nothing
                    return;
                if (job.Status == JobStatus.WAITING) // waiting -> postponed
                    job.Status = JobStatus.POSTPONED;
                else
                    job.Status = JobStatus.WAITING;
                this.queueListView.Items[position].SubItems[5].Text = job.StatusString;
            }
        }

        /// Sets the job status of the selected jobs to postponed.
        /// No selected jobs should currently be running.
        /// </summary>
        /// <param name="sender">This parameter is ignored</param>
        /// <param name="e">This parameter is ignored</param>
        private void PostponeMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in this.queueListView.SelectedItems)
            {
                int position = item.Index;
                TaggedJob job = jobs[item.Text];

                Debug.Assert(job.Status != JobStatus.PROCESSING && job.Status != JobStatus.PAUSED, "shouldn't be able to postpone an active job");

                job.Status = JobStatus.POSTPONED;
                this.queueListView.Items[position].SubItems[5].Text = job.StatusString;
            }
        }

        /// <summary>
        /// Sets the jobs status of the selected jobs to waiting.
        /// No selected jobs should currently be running.
        /// </summary>
        /// <param name="sender">This parameter is ignored</param>
        /// <param name="e">This parameter is ignored</param>
        private void WaitingMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in this.queueListView.SelectedItems)
            {
                int position = item.Index;
                TaggedJob job = jobs[item.Text];

                Debug.Assert(job.Status != JobStatus.PROCESSING && job.Status != JobStatus.PAUSED, "shouldn't be able to set an active job back to waiting");

                job.Status = JobStatus.WAITING;
                queueListView.Items[position].SubItems[5].Text = job.StatusString;
            }
        }


        private void QueueContextMenu_Opened(object sender, EventArgs e)
        {
            int count = queueListView.SelectedItems.Count;
            foreach (ToolStripItem i in multiJobHandlers)
                i.Enabled = (count > 0);
            foreach (ToolStripItem i in singleJobHandlers)
                i.Enabled = (count == 1);

            // here we generate our submenus as required, giving them the event handlers
            if (count > 0)
            {
                foreach (Pair<ToolStripMenuItem, MultiJobMenuGenerator> p in menuGenerators)
                {
                    p.fst.DropDownItems.Clear();
                    foreach (Pair<string, MultiJobHandler> pair in p.snd())
                    {
                        ToolStripItem i = p.fst.DropDownItems.Add(pair.fst);
                        i.Tag = pair.snd;
                        i.Click += delegate(object sender1, EventArgs e2)
                        {
                            Debug.Assert(queueListView.SelectedItems.Count > 0);
                            List<TaggedJob> list = new List<TaggedJob>();
                            foreach (ListViewItem i2 in queueListView.SelectedItems)
                                list.Add(jobs[i2.Text]);
                            ((MultiJobHandler)((ToolStripItem)sender1).Tag)(list);
                        };
                    }
                }
            }
        
            AbortMenuItem.Enabled = AllJobsHaveStatus(JobStatus.PROCESSING) || AllJobsHaveStatus(JobStatus.PAUSED) || AllJobsHaveStatus(JobStatus.ABORTED);
            AbortMenuItem.Checked = AllJobsHaveStatus(JobStatus.ABORTED);

            EditMenuItem.Enabled = IsSelectionEditable();
            EditMenuItem.Checked = false;

            bool canModifySelectedJobs = !AnyJobsHaveStatus(JobStatus.PROCESSING) && !AnyJobsHaveStatus(JobStatus.PAUSED) && !AnyJobsHaveStatus(JobStatus.ABORTING) && this.queueListView.SelectedItems.Count > 0;
            DeleteMenuItem.Enabled = PostponedMenuItem.Enabled = WaitingMenuItem.Enabled = canModifySelectedJobs;

            DeleteMenuItem.Checked = false;
            PostponedMenuItem.Checked = AllJobsHaveStatus(JobStatus.POSTPONED);
            WaitingMenuItem.Checked = AllJobsHaveStatus(JobStatus.WAITING);

            OpenMenuItem.Enabled = false;
            foreach (ListViewItem item in this.queueListView.SelectedItems)
            {
                TaggedJob job = jobs[item.Text];
                inputFileToolStripMenuItem.Enabled = inputFolderToolStripMenuItem.Enabled = false;
                outputFileToolStripMenuItem.Enabled = outputFolderToolStripMenuItem.Enabled = false;
                if (File.Exists(job.InputFile))
                    inputFileToolStripMenuItem.Enabled = OpenMenuItem.Enabled = true;
                if (Directory.Exists(FileUtil.GetDirectoryName(job.InputFile)))
                    inputFolderToolStripMenuItem.Enabled = OpenMenuItem.Enabled = true;
                if (File.Exists(job.OutputFile))
                    outputFileToolStripMenuItem.Enabled = OpenMenuItem.Enabled = true;
                if (Directory.Exists(FileUtil.GetDirectoryName(job.OutputFile)))
                    outputFolderToolStripMenuItem.Enabled = OpenMenuItem.Enabled = true;

                item.SubItems[5].Text = (jobs[item.Text]).StatusString;
            }
        }
        #endregion

        #region start / stop / abort / pause
        private void AbortButton_Click(object sender, EventArgs e)
        {
            AbortClicked(this, e);
        }

        private void StartStopButton_Click(object sender, EventArgs e)
        {
            switch (startStopMode)
            {
                case StartStopMode.Start:
                    StartClicked(this, e);
                    break;

                case StartStopMode.Stop:
                    StopClicked(this, e);
                    break;
            }
        }
        #endregion

        #region redrawing

        public void RefreshQueue()
        {
            if (!Visible) 
                return;

            if (queueListView.InvokeRequired)
            {
                queueListView.Invoke(new MethodInvoker(delegate { RefreshQueue(); }));
                return;
            }

            queueListView.BeginUpdate();
            foreach (ListViewItem item in queueListView.Items)
            {
                TaggedJob job = jobs[item.Text];
                item.SubItems[1].Text = job.InputFileName;
                item.SubItems[2].Text = job.OutputFileName;
                item.SubItems[3].Text = job.Job.CodecString;
                item.SubItems[4].Text = job.Job.EncodingMode;
                item.SubItems[5].Text = job.StatusString;
                
                if (job.Status == JobStatus.DONE)
                {
                    item.SubItems[7].Text = job.End.ToLongTimeString();
                    item.SubItems[8].Text = job.EncodingSpeed;
                }
                else
                {
                    item.SubItems[7].Text = "";
                    item.SubItems[8].Text = "";
                }
                if (job.Status == JobStatus.DONE || job.Status == JobStatus.PROCESSING || job.Status == JobStatus.PAUSED || job.Status == JobStatus.ABORTING)
                    item.SubItems[6].Text = job.Start.ToLongTimeString();
                else
                    item.SubItems[6].Text = "";
            }
            queueListView.EndUpdate();
            queueListView.Refresh();
        }
        #endregion

        private void StopButton_Click(object sender, EventArgs e)
        {
            StopClicked(this, e);
        }

        internal bool HasJob(TaggedJob job)
        {
            return jobs.ContainsKey(job.Name);
        }

        private void QueueListView_VisibleChanged(object sender, EventArgs e)
        {
            RefreshQueue();
        }

        private void AbortMenuItem_Click(object sender, EventArgs e)
        {
            if (AllJobsHaveStatus(JobStatus.ABORTED) && AbortMenuItem.Checked) // set them back to waiting
            {
                foreach (ListViewItem item in queueListView.SelectedItems)
                {
                    jobs[item.Text].Status = JobStatus.WAITING;
                }
                RefreshQueue();
            }
            else if (!AbortMenuItem.Checked)
                AbortClicked(this, e);
        }

        private void QueueListView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode.Equals(Keys.A))
            {
                foreach (ListViewItem item in this.queueListView.Items)
                    item.Selected = true;
                return;
            }

            switch (e.KeyCode)
            {
                case Keys.Delete: DeleteJobButton_Click(sender, e); break;
                case Keys.Up: if (upButton.Enabled && e.Shift) UpButton_Click(sender, e); break;
                case Keys.Down: if (downButton.Enabled && e.Shift) DownButton_Click(sender, e); break;
                case Keys.Escape:
                case Keys.Enter: StartStopButton_Click(sender, e); break;
            }
        }

        #region IPersistComponentSettings Members

        public void LoadComponentSettings()
        {
            if (MainForm.Instance == null) // Designer fix
                return;

            jobColumHeader.Width = MainForm.Instance.Settings.JobColumnWidth;
            inputColumnHeader.Width = MainForm.Instance.Settings.InputColumnWidth;
            outputColumnHeader.Width = MainForm.Instance.Settings.OutputColumnWidth;
            codecHeader.Width = MainForm.Instance.Settings.CodecColumnWidth;
            modeHeader.Width = MainForm.Instance.Settings.ModeColumnWidth;
            statusColumn.Width = MainForm.Instance.Settings.StatusColumnWidth;
            startColumn.Width = MainForm.Instance.Settings.StartColumnWidth;
            endColumn.Width = MainForm.Instance.Settings.EndColumnWidth;
            fpsColumn.Width = MainForm.Instance.Settings.FPSColumnWidth;
        }

        public void SaveComponentSettings()
        {
            if (MainForm.Instance == null) // Designer fix
                return;

            MainForm.Instance.Settings.JobColumnWidth = jobColumHeader.Width;
            MainForm.Instance.Settings.InputColumnWidth = inputColumnHeader.Width;
            MainForm.Instance.Settings.OutputColumnWidth = outputColumnHeader.Width;
            MainForm.Instance.Settings.CodecColumnWidth = codecHeader.Width;
            MainForm.Instance.Settings.ModeColumnWidth = modeHeader.Width;
            MainForm.Instance.Settings.StatusColumnWidth = statusColumn.Width;
            MainForm.Instance.Settings.StartColumnWidth = startColumn.Width;
            MainForm.Instance.Settings.EndColumnWidth = endColumn.Width;
            MainForm.Instance.Settings.FPSColumnWidth = fpsColumn.Width;
        }

        #endregion

        private void JobQueue_Load(object sender, EventArgs e)
        {

            if (OSInfo.IsWindowsVistaOrNewer)
                OSInfo.SetWindowTheme(queueListView.Handle, "explorer", null);
        }

        private void QueueListView_ColumnWidthChanged(object sender, ColumnWidthChangedEventArgs e)
        {
            this.SaveComponentSettings();
        }

        private void MarkDependentJobs()
        {
            List<string> oList = new List<string>();
            foreach (ListViewItem oItem in queueListView.SelectedItems)
            {
                if (!jobs.ContainsKey(oItem.Text)) // check if it has been removed
                    continue;
                TaggedJob job = jobs[oItem.Text];
                if (job == null)
                    continue;
                GetAllDependantJobs(job, ref oList);
            }

            queueListView.SuspendLayout();
            foreach (ListViewItem oItem in queueListView.Items)
            {
                if (oList.Contains(oItem.Text))
                    oItem.BackColor = Color.FromArgb(255, 225, 235, 255);
                else
                    oItem.BackColor = SystemColors.Window;
            }
            queueListView.ResumeLayout();
        }

        private void GetAllDependantJobs(TaggedJob job, ref List<string> oList)
        {
            if (oList.Contains(job.Name))
                return;

            oList.Add(job.Name);
            foreach (TaggedJob j in job.EnabledJobs)
                GetAllDependantJobs(j, ref oList);

            foreach (TaggedJob j in job.RequiredJobs)
                GetAllDependantJobs(j, ref oList);
        }

        private void OutputFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in this.queueListView.SelectedItems)
            {
                TaggedJob job = jobs[item.Text];
                if (!Directory.Exists(FileUtil.GetDirectoryName(job.OutputFile)))
                    continue;

                try
                {
                    Process prc = new Process();
                    prc.StartInfo.FileName = FileUtil.GetDirectoryName(job.OutputFile);
                    prc.Start();
                }
                catch (Exception) { }
            }
        }

        private void OutputFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in this.queueListView.SelectedItems)
            {
                TaggedJob job = jobs[item.Text];
                if (!File.Exists(job.OutputFile))
                    continue;

                try
                {
                    Process prc = new Process();
                    prc.StartInfo.FileName = job.OutputFile;
                    prc.Start();
                }
                catch (Exception) { }
            }
        }

        private void InputFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in this.queueListView.SelectedItems)
            {
                TaggedJob job = jobs[item.Text];
                if (!Directory.Exists(FileUtil.GetDirectoryName(job.InputFile)))
                    continue;

                try
                {
                    Process prc = new Process();
                    prc.StartInfo.FileName = FileUtil.GetDirectoryName(job.InputFile);
                    prc.Start();
                }
                catch (Exception) { }
            }
        }

        private void InputFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in this.queueListView.SelectedItems)
            {
                TaggedJob job = jobs[item.Text];
                if (!File.Exists(job.InputFile))
                    continue;

                try
                {
                    Process prc = new Process();
                    prc.StartInfo.FileName = job.InputFile;
                    prc.Start();
                }
                catch (Exception) { }
            }
        }
    }

    class JobQueueEventArgs : EventArgs
    {
        private Job job;
        public Job ModifiedJob
        {
            get { return job; }
        }

        public JobQueueEventArgs(Job j)
        {
            this.job = j;
        }
    }

}
