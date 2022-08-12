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
using System.Text;
using System.Windows.Forms;

namespace MeGUI.core.util
{
    public delegate void SingleFileReceiver(string file);
    public delegate void MultiFileReceiver(string[] files);

    public class DragDropUtil
    {
        public static void RegisterSingleFileDragDrop(Control c, SingleFileReceiver r)
        {
            RegisterSingleFileDragDrop(c, r, "All files|*.*");
        }

        public static void RegisterSingleFileDragDrop(Control c, SingleFileReceiver r, string filter)
        {
            RegisterSingleFileDragDrop(c, r, delegate() { return filter; });
        }

        public static void RegisterSingleFileDragDrop(Control c, SingleFileReceiver r, Getter<string> filter)
        {
            c.AllowDrop = true;
            c.DragEnter += delegate(object sender, DragEventArgs e)
                {
                    e.Effect = DragDropEffects.None;

                    if (e.Data.GetDataPresent(DataFormats.FileDrop))
                    {
                        string[] files = (string[])e.Data.GetData(DataFormats.FileDrop, false);
                        if (files.Length == 1 && FileUtil.MatchesFilter(filter(), files[0]))
                            e.Effect = DragDropEffects.All;
                    }
                };
            c.DragDrop += delegate(object sender, DragEventArgs e)
                {
                    r(((string[])e.Data.GetData(DataFormats.FileDrop, false))[0]);
                };
        }

        public static void RegisterMultiFileDragDrop(Control c, MultiFileReceiver r)
        {
            RegisterMultiFileDragDrop(c, r, "All files|*.*");
        }

        public static void RegisterMultiFileDragDrop(Control c, MultiFileReceiver r, string filter)
        {
            RegisterMultiFileDragDrop(c, r, delegate() { return filter; });
        }

        public static void RegisterMultiFileDragDrop(Control c, MultiFileReceiver r, Getter<string> filter)
        {
            c.AllowDrop = true;
            c.DragEnter += delegate(object sender, DragEventArgs e)
                {
                    e.Effect = DragDropEffects.None;

                    if (e.Data.GetDataPresent(DataFormats.FileDrop))
                    {
                        string[] files = (string[])e.Data.GetData(DataFormats.FileDrop, false);
                        if (files.Length > 0 &&
                            Array.Exists<string>(files, delegate(string s)
                        {
                            return FileUtil.MatchesFilter(filter(), s);
                        }))
                            e.Effect = DragDropEffects.All;
                    }
                };

            c.DragDrop += delegate(object sender, DragEventArgs e)
                {
                    r(Array.FindAll<string>(((string[])e.Data.GetData(DataFormats.FileDrop, false)),
                        delegate(string s)
                        {
                            return FileUtil.MatchesFilter(filter(), s);
                        }));
                };

        }
    }
}
