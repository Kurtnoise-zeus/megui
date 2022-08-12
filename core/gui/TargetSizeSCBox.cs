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
using System.Text;
using System.Windows.Forms;

using MeGUI.core.util;

namespace MeGUI.core.gui
{
    public class TargetSizeSCBox : StandardAndCustomComboBox
    {
        public static readonly Named<FileSize>[] PredefinedFilesizes = new Named<FileSize>[] {
            new Named<FileSize>("CD  (700MB)", new FileSize(Unit.MB, 700)),
            new Named<FileSize>("DVD or BD-5  (4480MB)", new FileSize(Unit.MB, 4480)),
            new Named<FileSize>("DVD-DL or BD-9 (8145MB)", new FileSize(Unit.MB, 8145)),
            new Named<FileSize>("BD  (23450MB)", new FileSize(Unit.MB, 23450)),
            new Named<FileSize>("BD-DL  (46900MB)", new FileSize(Unit.MB, 46900)) };

        protected override void Dispose(bool disposing)
        {
            if (MainForm.Instance != null && base.bSaveEveryItem) // form designer fix
                MainForm.Instance.Settings.CustomFileSizes = CustomSizes;
            base.Dispose(disposing);
        }

        private string nullString;
        /// <summary>
        /// String to display which represents "null" filesize. If NullString is set to null, then
        /// there is no option not to select a filesize.
        /// </summary>
        public string NullString
        {
            get { return nullString; }
            set { nullString = value; fillStandard(); }
        }

        private FileSize minSize = FileSize.MinNonZero;

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public FileSize MinimumFileSize
        {
            get { return minSize; }
            set { minSize = value; }
        }

        private FileSize? maxSize = null;
        
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public FileSize? MaximumFileSize
        {
            get { return maxSize; }
            set { maxSize = value; }
        }

        public TargetSizeSCBox() : base("Remove custom sizes...", "Select custom size...")
        {
            base.Getter = new Getter<object>(getter);
            base.bSaveEveryItem = true;
            SaveCustomValues = false;
            if (MainForm.Instance != null) // form designer fix
                CustomSizes = MainForm.Instance.Settings.CustomFileSizes;
        }

        private void fillStandard()
        {
            List<object> objects = new List<object>();
            if (!string.IsNullOrEmpty(NullString))
                objects.Add(NullString);
            objects.AddRange(TargetSizeSCBox.PredefinedFilesizes);
            base.StandardItems = objects.ToArray();
        }

        FileSizeDialog ofd = new FileSizeDialog();

        private object getter()
        {
            ofd.Value = Value ?? new FileSize(Unit.MB, 700);
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                if (ofd.Value >= minSize &&
                   maxSize == null || ofd.Value <= maxSize)
                    return ofd.Value;
                else
                    MessageBox.Show(genRestrictions(), "Invalid filesize", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            return null;
        }

        public FileSize[] CustomSizes
        {
            get
            {
                return Util.CastAll<FileSize>(CustomItems);
            }
            set
            {
                if (value == null || value.Length == 0)
                    return;
                CustomItems = Util.CastAll<FileSize, object>(value);
            }
        }


        private string genRestrictions()
        {
            if (maxSize.HasValue)
                return string.Format("Filesize must be between {0} and {1}.", minSize, maxSize);
            else
                return string.Format("Filesize must be at least {0}.", minSize);
        }

        /// <summary>
        /// Gets / sets the target, or null if the user doesn't care about filesize
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public FileSize? Value
        {
            get
            {
                object o = base.SelectedObject;
                if (o.Equals(NullString))
                    return null;
                if (o is Named<FileSize>)
                    return ((Named<FileSize>)o).Data;
                else
                    return (FileSize)o;
            }

            set
            {
                if (value.HasValue)
                    base.SelectedObject = value.Value;
                else
                    base.SelectedObject = NullString;
            }
        }

        /// <summary>
        /// Gets / sets the target, or null if the user doesn't care about filesize
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public FileSize CertainValue
        {
            get { return Value.Value; }
            set { Value = value; }
        }

        public bool SaveCustomValues
        {
            get { return base.bSaveEveryItem; }
            set 
            {
                if (base.bSaveEveryItem != value)
                {
                    base.bSaveEveryItem = value;
                    base.SetTargetSizeSCBoxType("Remove custom sizes...", "Select custom size...");
                    fillStandard();
                    if (MainForm.Instance != null) // form designer fix
                        CustomSizes = MainForm.Instance.Settings.CustomFileSizes;
                }
            }
        }
    }
}
