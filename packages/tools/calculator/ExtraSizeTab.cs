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
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using MeGUI.core.details;
using MeGUI.core.util;

using MediaInfoWrapper;

namespace MeGUI.packages.tools.calculator
{
    /// <summary>
    /// Windows 11 look of the rows of the audio and extra files list of the calculator: captions above the fields,
    /// rounded text boxes and button, the name follows the width. The controls only move: names and events are unchanged.
    /// </summary>
    internal static class SizeTabLayout
    {
        public static void Apply(UserControl tab, LinkLabel remove, System.Windows.Forms.Label title, TextBox name, Button selectBtn,
            System.Windows.Forms.Label sizeCaption, TextBox size, System.Windows.Forms.Label[] captions, Control[] controls, int[] widths)
        {
            float k = MainForm.Instance != null ? MainForm.Instance.Settings.DPIRescale(1000) / 1000f : 1f;
            Func<int, int> S = delegate (int v) { return (int)Math.Round(v * k); };
            tab.AutoScaleMode = AutoScaleMode.None;
            tab.Font = new System.Drawing.Font("Segoe UI", 10F);
            tab.Size = new System.Drawing.Size(S(610), S(64));

            int right = tab.Width - S(8);
            MeGUI.core.gui.RoundedBox sizeBox = new MeGUI.core.gui.RoundedBox(size, S(90), S(32));
            sizeBox.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            sizeBox.Location = new System.Drawing.Point(right - sizeBox.Width, S(26));
            tab.Controls.Add(sizeBox);
            sizeCaption.AutoSize = true;
            sizeCaption.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            sizeCaption.Location = new System.Drawing.Point(sizeBox.Left, S(4));
            right = sizeBox.Left - S(8);

            for (int i = controls.Length - 1; i >= 0; i--)
            {
                Control c = controls[i];
                c.Width = S(widths[i]);
                c.Anchor = AnchorStyles.Top | AnchorStyles.Right;
                c.Location = new System.Drawing.Point(right - c.Width, S(26) + (S(32) - c.Height) / 2);
                captions[i].AutoSize = true;
                captions[i].Anchor = AnchorStyles.Top | AnchorStyles.Right;
                captions[i].Location = new System.Drawing.Point(c.Left, S(4));
                right = c.Left - S(8);
            }

            selectBtn.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            selectBtn.Size = new System.Drawing.Size(S(34), S(32));
            selectBtn.Location = new System.Drawing.Point(right - selectBtn.Width, S(26));
            right = selectBtn.Left - S(8);

            MeGUI.core.gui.RoundedBox nameBox = new MeGUI.core.gui.RoundedBox(name, right - S(8), S(32));
            nameBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            nameBox.Location = new System.Drawing.Point(S(8), S(26));
            tab.Controls.Add(nameBox);

            remove.Location = new System.Drawing.Point(S(4), S(2));
            title.AutoSize = true;
            title.Location = new System.Drawing.Point(S(34), S(5));
        }
    }

    public partial class ExtraSizeTab : UserControl
    {
        public event EventHandler SomethingChanged;

        public ExtraSizeTab()
        {
            InitializeComponent();
            SizeTabLayout.Apply(this, removeLink, audioLabel, name, selectButton, label2, size, new System.Windows.Forms.Label[0], new Control[0], new int[0]);
            //DragDropUtil.RegisterSingleFileDragDrop(this, selectExtraFile, this.filter);
        }

        private void raiseEvent()
        {
            if (SomethingChanged != null)
                SomethingChanged(this, EventArgs.Empty);
        }

        private void selectExtraFile(string file)
        {
            FileSize = FileSize.Of2(file) ?? FileSize.Empty;
            size.Text = FileSize.ToString();
            name.Text = System.IO.Path.GetFileName(file);
            raiseEvent();
        }

        public FileSize FileSize { get; protected set; }

        private readonly string filter = "All Files (*.*)|*.*";

        private void selectButton_Click(object sender, EventArgs e)
        {
            openFileDialog.Filter = this.filter;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                selectExtraFile(openFileDialog.FileName);
            }
        }

        private void removeLink_LinkClicked(object sender, EventArgs e)
        {
            if (this.Parent != null) this.Parent.Controls.Remove(this);
        }

        private void selectButton_Enter(object sender, EventArgs e)
        {
            selectButton.Focus();
        }
    }
}
