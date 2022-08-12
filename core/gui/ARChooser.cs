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
using System.Text;
using System.Windows.Forms;

using MeGUI.core.util;

namespace MeGUI.core.gui
{
    public class ARChooser : StandardAndCustomComboBox
    {

        protected override void Dispose(bool disposing)
        {
            if (MainForm.Instance != null)  // Form designer fix
                MainForm.Instance.Settings.CustomDARs = CustomDARs;
            base.Dispose(disposing);
        }

        private static readonly string Later = "Auto-detect later";

        public static readonly Named<Dar>[] ARs = new Named<Dar>[] {
                new Named<Dar>("ITU 16:9 PAL (1.823361)", Dar.ITU16x9PAL),
                new Named<Dar>("ITU 4:3 PAL (1.367521)", Dar.ITU4x3PAL),
                new Named<Dar>("ITU 16:9 NTSC (1.823169)", Dar.ITU16x9NTSC),
                new Named<Dar>("ITU 4:3 NTSC (1.367377)", Dar.ITU4x3NTSC),
                new Named<Dar>("4:3 (1.333333)", Dar.STATIC4x3),
                new Named<Dar>("16:9 (1.777778)", Dar.STATIC16x9),
                new Named<Dar>("1:1", Dar.A1x1) };

        public ARChooser()
            : base("Clear user-selected ARs...", "Select AR...")
        {
            base.Getter = delegate
            {
                Dar result;
                if (AspectRatioChooser.ShowDialog(Value ?? Dar.ITU16x9PAL, out result) == DialogResult.OK)
                    return result;
                else
                    return null;
            };

            HasLater = true;

            if (MainForm.Instance != null)  // Form designer fix
                CustomDARs = MainForm.Instance.Settings.CustomDARs;
        }

        bool hasLater;
        public bool HasLater
        {
            get { return hasLater; }
            set
            {
                hasLater = value;
                if (hasLater)
                {
                    List<object> o = new List<object>();
                    o.Add(Later);
                    o.AddRange(ARs);
                    base.StandardItems = o.ToArray();
                }
                else
                {
                    base.StandardItems = ARs;
                }
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Dar? Value
        {
            get
            {
                if (SelectedObject.Equals(Later))
                    return null;
                if (SelectedObject is Named<Dar>)
                    return ((Named<Dar>)SelectedObject).Data;
                else
                    return ((Dar)SelectedObject);
            }
            set
            {
                if (value == null)
                    SelectedObject = Later;
                else
                    SelectedObject = value;
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Dar RealValue
        {
            get
            {
                if (HasLater) throw new Exception("For OneClicker, use Value");
                return Value.Value;
            }
            set { Value = value; }
        }

        public Dar[] CustomDARs
        {
            get
            {
                return Util.CastAll<Dar>(CustomItems);
            }
            set
            {
                if (value == null || value.Length == 0)
                    return;
                base.CustomItems = Util.CastAll<Dar, object>(value);
            }
        }
    }
    
}
