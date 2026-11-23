// ****************************************************************************
// 
// Copyright (C) 2005-2026 Doom9 & al
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
using System.Drawing.Drawing2D;
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

            BuildImages();
            ThemeManager.ThemeChanged += OnThemeChanged;
            Disposed += delegate { ThemeManager.ThemeChanged -= OnThemeChanged; };
        }

        private void OnThemeChanged(object sender, EventArgs e)
        {
            // may be raised from another thread (system colour change)
            Util.ThreadSafeRun(treeView, delegate
            {
                BuildImages();
                ApplyTreeTheme();
                // the theme engine restores the native theme of the tree in its own pass: apply ours after it
                if (treeView.IsHandleCreated)
                    treeView.BeginInvoke(new MethodInvoker(ApplyTreeTheme));
            });
        }

        /// <summary>native theme of the tree (scroll bars): explorer style in light mode, dark explorer style in dark mode</summary>
        private void ApplyTreeTheme()
        {
            if (!OSInfo.IsWindowsVistaOrNewer || !treeView.IsHandleCreated)
                return;
            try
            {
                OSInfo.SetWindowTheme(treeView.Handle, ThemeManager.IsDark ? "DarkMode_Explorer" : "explorer", null);
            }
            catch (Exception)
            {
                // dark scroll bars not available: keep the standard ones
            }
        }

        /// <summary>
        /// Builds the Error / Warning / Information icons of the log (same order as <see cref="ImageType"/>).
        /// 32 bit colour, sized for the DPI of the screen. Dark theme: flat icons drawn with the palette
        /// (the system icons are heavy and blurry when they are scaled down on a dark background).
        /// </summary>
        private void BuildImages()
        {
            int size = Math.Max(16, (int)Math.Round(16 * DeviceDpi / 96.0));
            ImageList i = new ImageList();
            i.ColorDepth = ColorDepth.Depth32Bit;
            i.ImageSize = new Size(size, size);
            if (ThemeManager.IsDark)
            {
                i.Images.Add(DrawIcon(ImageType.Error, size));
                i.Images.Add(DrawIcon(ImageType.Warning, size));
                i.Images.Add(DrawIcon(ImageType.Information, size));
            }
            else
            {
                i.Images.Add(SystemIcons.Error);
                i.Images.Add(SystemIcons.Warning);
                i.Images.Add(SystemIcons.Information);
            }
            ImageList old = treeView.ImageList;
            treeView.ImageList = i;
            if (old != null)
                old.Dispose();
        }

        private static Bitmap DrawIcon(ImageType type, int s)
        {
            Bitmap bmp = new Bitmap(s, s, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                float f = s / 16f; // everything below is designed on a 16 x 16 grid
                RectangleF body = new RectangleF(1 * f, 1 * f, 14 * f, 14 * f);

                switch (type)
                {
                    case ImageType.Information:
                        using (SolidBrush fill = new SolidBrush(Color.FromArgb(0x4C, 0xA8, 0xF0)))
                        using (SolidBrush glyph = new SolidBrush(Color.White))
                        {
                            g.FillEllipse(fill, body);
                            g.FillEllipse(glyph, 6.9f * f, 3.4f * f, 2.2f * f, 2.2f * f);          // dot
                            g.FillRectangle(glyph, 7.0f * f, 6.8f * f, 2.0f * f, 5.6f * f);        // stem
                        }
                        break;

                    case ImageType.Warning:
                        using (SolidBrush fill = new SolidBrush(Color.FromArgb(0xFF, 0xB9, 0x00)))
                        using (Pen join = new Pen(Color.FromArgb(0xFF, 0xB9, 0x00), 1.6f * f))
                        using (SolidBrush glyph = new SolidBrush(Color.FromArgb(0x2B, 0x20, 0x00)))
                        {
                            join.LineJoin = LineJoin.Round;
                            PointF[] tri = new PointF[] { new PointF(8f * f, 2.2f * f), new PointF(14.2f * f, 13.2f * f), new PointF(1.8f * f, 13.2f * f) };
                            g.FillPolygon(fill, tri);
                            g.DrawPolygon(join, tri);                                              // rounded corners
                            g.FillRectangle(glyph, 7.1f * f, 6.0f * f, 1.8f * f, 4.0f * f);        // stem
                            g.FillEllipse(glyph, 7.0f * f, 10.6f * f, 2.0f * f, 2.0f * f);         // dot
                        }
                        break;

                    case ImageType.Error:
                        using (SolidBrush fill = new SolidBrush(Color.FromArgb(0xE8, 0x4A, 0x4A)))
                        using (Pen cross = new Pen(Color.White, 1.8f * f))
                        {
                            cross.StartCap = LineCap.Round;
                            cross.EndCap = LineCap.Round;
                            g.FillEllipse(fill, body);
                            g.DrawLine(cross, 5.4f * f, 5.4f * f, 10.6f * f, 10.6f * f);
                            g.DrawLine(cross, 10.6f * f, 5.4f * f, 5.4f * f, 10.6f * f);
                        }
                        break;
                }
            }
            return bmp;
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
            ApplyTreeTheme();
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