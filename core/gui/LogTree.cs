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
using System.IO;
using System.Windows.Forms;

using MeGUI.core.util;

namespace MeGUI.core.gui
{
    public partial class LogTree : UserControl
    {
        private LogItem _oLog;

        public LogTree()
        {
            InitializeComponent();

            _oLog = new LogItem("Log", ImageType.NoImage);

            ImageList i = new ImageList();
            i.Images.Add(System.Drawing.SystemIcons.Error);
            i.Images.Add(System.Drawing.SystemIcons.Warning);
            i.Images.Add(System.Drawing.SystemIcons.Information);
            treeView.ImageList = i;
        }

        public LogItem Log
        {
            get { return _oLog; }
        }

        public void SetLog(LogItem log)
        {
            _oLog = log;
            foreach (LogItem oItem in _oLog.SubEvents)
                Util.ThreadSafeRun(treeView, delegate { treeView.Nodes.Add(Register(oItem)); });

            _oLog.SubItemAdded += delegate (object sender, EventArgs<LogItem> args)
            {
                Util.ThreadSafeRun(treeView, delegate { treeView.Nodes.Add(Register(args.Data)); });
            };
        }

        private TreeNode Register(LogItem log)
        {
            List<TreeNode> subNodes = log.SubEvents.ConvertAll<TreeNode>(delegate(LogItem e)
            {
                return Register(e);
            });

            TreeNode node = new TreeNode(log.Text, (int)log.Type, (int)log.Type, subNodes.ToArray())
            {
                Tag = log
            };
            log.SubItemAdded += delegate(object sender, EventArgs<LogItem> args)
            {
                Util.ThreadSafeRun(treeView, delegate { node.Nodes.Add(Register(args.Data)); });
            };

            log.TypeChanged += delegate(object sender, EventArgs<ImageType> args)
            {
                Util.ThreadSafeRun(treeView, delegate { node.SelectedImageIndex = node.ImageIndex = (int)args.Data; });
            };
            log.Expanded += delegate(object sender, EventArgs e)
            {
                Util.ThreadSafeRun(treeView, delegate { node.Expand(); });
            };
            log.Collapsed += delegate(object sender, EventArgs e)
            {
                Util.ThreadSafeRun(treeView, delegate { node.Collapse(); });
            };

            return node;
        }

        private void OfIndividualNodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Show(SelectedLogItem, false);
        }

        private void OfBranchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Show(SelectedLogItem, true);
        }

        private void EditLog_Click(object sender, EventArgs e)
        {
            Show(_oLog, true);
        }

        private LogItem SelectedLogItem
        {
            get
            {
                if (treeView.SelectedNode == null)
                    return null;

                return (treeView.SelectedNode.Tag as LogItem);
            }
        }

        private void Show(LogItem l, bool subnodes)
        {
            if (l == null)
                return;

            TextViewer t = new TextViewer()
            {
                Contents = l.ToString(subnodes),
                Wrap = false
            };
            t.ShowDialog();
        }

        private void SaveLog_Click(object sender, EventArgs e)
        {
            Save(_oLog);
        }

        private void SaveBranch_Click(object sender, EventArgs e)
        {
            LogItem i = SelectedLogItem;
            if (i == null)
            {
                MessageBox.Show("No log branch selected", "Can't save file", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Save(i);
        }

        private void Save(LogItem i)
        {
            if (saveDialog.ShowDialog() != DialogResult.OK)
                return;

            try
            {
                File.WriteAllText(saveDialog.FileName, i.ToString());
                MessageBox.Show("File saved successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.None);
            }
            catch (Exception ie)
            {
                MessageBox.Show("Error saving file: " + ie.Message, "Error saving file", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

       private void ExpandOrCollapseAll(LogItem i, bool expand)
        {
            if (expand)
                i.Expand();
            else
                i.Collapse();

            foreach (LogItem i2 in i.SubEvents)
                ExpandOrCollapseAll(i2, expand);
        }

        private void ExpandAll(LogItem i) { ExpandOrCollapseAll(i, true); }
        private void CollapseAll(LogItem i) { ExpandOrCollapseAll(i, false); }

        private void ExpandLog_Click(object sender, EventArgs e)
        {
            ExpandAll(_oLog);
        }

        private void ExpandBranch_Click(object sender, EventArgs e)
        {
            ExpandAll(SelectedLogItem);
        }

        private void CollapseLog_Click(object sender, EventArgs e)
        {
            CollapseAll(_oLog);
        }

        private void CollapseBranch_Click(object sender, EventArgs e)
        {
            CollapseAll(SelectedLogItem);
        }

        private void LogTree_Load(object sender, EventArgs e)
        {
            if (OSInfo.IsWindowsVistaOrNewer)
                OSInfo.SetWindowTheme(treeView.Handle, "explorer", null);
        }

        private void ResetOverlayIcon_Click(object sender, EventArgs e)
        {
           MainForm.Instance.setOverlayIcon(null, false);
        }

        private void ContextMenu_Opening(object sender, CancelEventArgs e)
        {
            if (MainForm.Instance.IsOverlayIconActive)
                resetOverlayIcon.Visible = true;
            else
                resetOverlayIcon.Visible = false;
        }
    }
}