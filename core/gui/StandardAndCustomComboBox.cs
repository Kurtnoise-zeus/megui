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
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using MeGUI.core.util;

namespace MeGUI.core.gui
{
    /// <summary>
    /// Represents a drop-down ComboBox which has both standard and custom values.
    /// Examples are CQMs (for x264, the standard two are "flat" and "jvt", and
    /// custom ones are user-selected files) and file-sizes (standard is "1 CD", "2 CD",
    /// custom is "100MB", etc).
    /// 
    /// There are three separated sections, with the Standard at the top, the custom 
    /// in the middle, and one or two buttons at the bottom to choose a new custom 
    /// option, and to clear all custom options.
    /// 
    /// Known derived classes are FileSCBox, which opens an OpenFileDialog as the
    /// new custom option, and FileSizeSCBox, which lets the user choose a non-custom
    /// filesize.
    /// </summary>
    public partial class StandardAndCustomComboBox : MeGUI.core.gui.NiceComboBox
    {
        private NiceComboBoxNormalItem clearItem;
        private int numStandardItems, numCustomItems, numOpener;
        protected Getter<object> Getter;
        protected Getter<object> GetterFolder;
        protected bool bSaveEveryItem;

        public StandardAndCustomComboBox() : base() { }

        public StandardAndCustomComboBox(string clearText, string chooseNewText)
            : base()
        {
            InitializeComponent();

            numOpener = 1;

            clearItem = new NiceComboBoxNormalItem(
                clearText,
                delegate(NiceComboBoxNormalItem i, EventArgs e)
                {
                    clearCustomItems();
                });

            Items.Add(new NiceComboBoxNormalItem(
                chooseNewText, null,
                delegate(NiceComboBoxNormalItem i, EventArgs e)
                {
                    object o = Getter();
                    if (o != null)
                    {
                        AddCustomItem(o);
                        SelectedObject = o;
                    }
                }));
        }

        public void SetFileSCBoxType(string chooseNewText, string chooseNewFolder, MeGUI.core.gui.FileSCBox.FileSCBoxType oType)
        {
            if (oType == FileSCBox.FileSCBoxType.OC_FILE_AND_FOLDER || oType == FileSCBox.FileSCBoxType.OC_FILE)
            {
                Items.Clear();
                clearItem = null;

                numOpener = 1;

                Items.Add(new NiceComboBoxNormalItem(
                    chooseNewText, null,
                    delegate(NiceComboBoxNormalItem i, EventArgs e)
                    {
                        object o = Getter();
                        if (o != null)
                        {
                            AddCustomItem(o);
                            SelectedObject = o;
                        }
                    }));
            }
            if (oType == FileSCBox.FileSCBoxType.OC_FILE_AND_FOLDER)
            {
                numOpener = 2;

                Items.Add(new NiceComboBoxNormalItem(
                    chooseNewFolder, null,
                    delegate(NiceComboBoxNormalItem i, EventArgs e)
                    {
                        object o = GetterFolder();
                        if (o != null)
                        {
                            AddCustomItem(o);
                            SelectedObject = o;
                        }
                    }));
            }
            numStandardItems = numCustomItems = 0;
        }

        public void SetTargetSizeSCBoxType(string clearText, string chooseCustomSize)
        {
            Items.Clear();
            numOpener = 1;
            numStandardItems = numCustomItems = 0;

            Items.Add(new NiceComboBoxNormalItem(
                chooseCustomSize, null,
                delegate(NiceComboBoxNormalItem i, EventArgs e)
                {
                    object o = Getter();
                    if (o != null)
                    {
                        if (bSaveEveryItem)
                        {
                            AddCustomItem(o);
                            SelectedObject = o;
                        }
                        else
                        {
                            oTemporaryItem.Tag = o;
                            SelectedItem = null;
                        }
                    }
                }));

            clearItem = new NiceComboBoxNormalItem(
                    clearText,
                    delegate(NiceComboBoxNormalItem i, EventArgs e)
                    {
                        clearCustomItems();
                    });

            if (!bSaveEveryItem)
                clearItem = null;
        }

        public void AddStandardItem(object o)
        {
            if (numStandardItems == 0)
                Items.Insert(0, new NiceComboBoxSeparator());

            Items.Insert(numStandardItems, new NiceComboBoxNormalItem(new SCItem(o, true)));
            numStandardItems++;
        }

        public void AddCustomItem(object name)
        {
            if (numCustomItems == 0)
            {
                Items.Insert(Items.Count - numOpener, new NiceComboBoxSeparator());
                if (clearItem != null)
                    Items.Insert(Items.Count - numOpener, clearItem);
            }

            if (clearItem != null)
                Items.Insert(Items.Count - 2 - numOpener, new NiceComboBoxNormalItem(new SCItem(name, false)));
            else
                Items.Insert(Items.Count - 1 - numOpener, new NiceComboBoxNormalItem(new SCItem(name, false)));
            numCustomItems++;
        }

        private void clearStandardItems()
        {
            if (numStandardItems == 0) return;

            Items.RemoveRange(0, numStandardItems + 1);
            numStandardItems = 0;
        }

        private void clearCustomItems()
        {
            int start = getCustomItemStart();

            if (SelectedIndex >= start)
                SelectedIndex = 0;

            Items.RemoveRange(start, numCustomItems);
            if (numCustomItems > 0)
            {
                Items.RemoveAt(start);
                if (clearItem != null)
                    Items.RemoveAt(start);
            }

            numCustomItems = 0;
        }

        private int getCustomItemStart()
        {
            if (numStandardItems > 0)
                return numStandardItems + 1;
            return 0;
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public object[] CustomItems
        {
            get
            {
                object[] res = new object[numCustomItems];
                int start = getCustomItemStart();

                for (int i = 0; i < numCustomItems; ++i)
                {
                    res[i] = ((SCItem)Items[start + i].Tag).Tag;
                }
                return res;
            }
            set
            {
                clearCustomItems();
                if (value == null)
                    return;
                foreach (object s in value)
                    AddCustomItem(s);
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public object[] StandardItems
        {
            get
            {
                object[] res = new object[numStandardItems];

                for (int i = 0; i < numStandardItems; ++i)
                {
                    res[i] = (Items[i].Tag as SCItem).Tag;
                }
                return res;
            }
            set
            {
                clearStandardItems();
                foreach (object o in value)
                    AddStandardItem(o);
                if (SelectedIndex == -1 && value.Length > 0)
                    SelectedIndex = 0;
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public object SelectedObject
        {
            get
            {
                return SelectedSCItem.Tag;
            }
            set
            {
                oTemporaryItem.Tag = null;

                if (value == null)
                    return;

                if (value.ToString() == "")
                    return;

                foreach (NiceComboBoxItem i in Items)
                {
                    if ((i.Tag as SCItem) != null && ((SCItem)i.Tag).Tag.Equals(value))
                    {
                        SelectedItem = i;
                        return;
                    }
                }
                if (bSaveEveryItem)
                {
                    AddCustomItem(value);
                    SelectedObject = value;
                }
                else
                {
                    oTemporaryItem.Tag = value;
                    SelectedItem = null;
                }
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public SCItem SelectedSCItem
        {
            get
            {
                if (oTemporaryItem.Tag != null && SelectedItem == null)
                    return oTemporaryItem;

                if (SelectedItem == null) 
                    SelectedIndex = 0;

                return (SCItem)SelectedItem.Tag;
            }
        }

    }

    public class SCItem
    {
        public object Tag;
        public bool IsStandard;

        public SCItem(object tag, bool isStandard)
        {
            Tag = tag;
            IsStandard = isStandard;
        }

        public override string ToString()
        {
            return Tag.ToString();
        }
    }

    public class Named<T>
    {
        public T Data;
        public string Name;

        public Named(string name, T data)
        {
            Data = data;
            Name = name;
        }

        public override bool Equals(object obj)
        {
            return (((obj as Named<T>) != null) && ((Named<T>)obj).Data.Equals(Data))
                    || (obj is T && ((T)obj).Equals(Data));
        }

        public override int GetHashCode()
        {
            return 0;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}


