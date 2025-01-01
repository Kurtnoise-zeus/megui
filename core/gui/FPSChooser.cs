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
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Forms;

using MeGUI.core.util;

namespace MeGUI.core.gui
{
    public class FPSChooser : StandardAndCustomComboBox
    {
        private static readonly object[] Framerates = new object[] { 
            new FPS(23.976M), 
            new FPS(24.0M), 
            new FPS(25.0M), 
            new FPS(29.97M), 
            new FPS(30.0M), 
            new FPS(50.0M), 
            new FPS(59.94M), 
            new FPS(60.0M) };

        public FPSChooser()
            : base("Clear user-selected framerates...", "Select framerate...")
        {
            base.Getter = delegate
            {
                decimal result;
                if (NumberChooser.ShowDialog(
                    "Enter your framerate:", "Custom framerate", 0,
                    1M, 100000M, Value ?? 25M, Value ?? 1000M, out result) == DialogResult.OK)
                    return new FPS(result);
                else
                    return null;
            };
            StandardItems = Framerates;
            if (MainForm.Instance != null) // form designer fix
                CustomFPSs = MainForm.Instance.Settings.CustomFPSs;
        }

        protected override void Dispose(bool disposing)
        {
            if (MainForm.Instance != null) // form designer fix
                MainForm.Instance.Settings.CustomFPSs = CustomFPSs;
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
        
        private void fillStandard()
        {
            List<object> objects = new List<object>();
            if (!string.IsNullOrEmpty(NullString))
                objects.Add(NullString);
            objects.AddRange(Framerates);
            base.StandardItems = objects.ToArray();

        }

        private FPS[] CustomFPSs
        {
            get 
            { 
                return Util.CastAll<FPS>(CustomItems); 
            }
            set 
            {
                if (value == null || value.Length == 0)
                    return;
                CustomItems = Util.CastAll<FPS, object>(value); 
            }
        }


        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public decimal? Value
        {
            get
            {
                if (SelectedObject.Equals(NullString))
                    return null;
                return ((FPS)SelectedObject).fps;
            }
            set
            {
                if (value == null)
                {
                    SelectedObject = NullString;
                    return;
                }
                if (value == 0) return;
                SelectedObject = new FPS(value.Value);
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public decimal CertainValue
        {
            get
            {
                return Value.Value;
            }
            set
            {
                Value = value;
            }
        }
    }

    [TypeConverter(typeof(FPSConverter))]
    public struct FPS
    {
        public FPS(decimal v)
        {
            fps = v;
        }

        public decimal fps;

        public override string ToString()
        {
            return fps.ToString();
        }

        public static FPS Parse(string s)
        {
            return new FPS(decimal.Parse(s));
        }

        public override bool Equals(object obj)
        {
            if (!(obj is FPS)) return false;
            decimal other = ((FPS)obj).fps;
            return (Math.Abs(fps - other) < MainForm.Instance.Settings.AcceptableFPSError);
        }

        public override int GetHashCode()
        {
            return 0;
        }
    }

    class FPSConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return (sourceType == typeof(string));
        }

        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (value is string)
                return FPS.Parse((string)value);

            throw new Exception();
        }

        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
                return value.ToString();

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
