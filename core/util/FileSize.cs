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
using System.ComponentModel;
using System.IO;

namespace MeGUI.core.util
{
    public enum Unit : ushort { B = 0, KB, MB, GB }

    /// <summary>
    /// Aims to be the universal representation of filesize in MeGUI.
    /// Should avoid problems of MB/KB/B, and gives nice formatting as required
    /// </summary>
    [TypeConverter(typeof(FileSizeConverter))]
    public struct FileSize
    {
        public static readonly FileSize Empty = new FileSize(0);
        public static readonly FileSize MinNonZero = new FileSize(1);


        public static FileSize Of(string filename)
        {
            FileInfo fi = new FileInfo(filename);
            return new FileSize(fi.Length);
        }

        public static FileSize? Of2(string filename)
        {
            if (string.IsNullOrEmpty(filename) || !File.Exists(filename))
                return null;
            
            FileInfo fi = new FileInfo(filename);
            if (fi.Length <= 0)
                return null;
            
            return new FileSize(fi.Length);
        }            
        public static readonly string[] UnitSuffixes = new string[] { "bytes", "KB", "MB", "GB" };
        private ulong sizeInBytes;

        #region helper functions
        /// <summary>
        /// Returns the stored size, divided by divisor and rounded appropriately
        /// </summary>
        /// <param name="divisor"></param>
        /// <returns></returns>
        private ulong divideAndRound(ulong divisor)
        {
            ulong divided = sizeInBytes / divisor;
            if (sizeInBytes % divisor > (divisor / 2))
                divided++;
            return divided;
        }

        /// <summary>
        /// Sets sizeInBytes to (value * divisor) rounded appropriately
        /// </summary>
        /// <param name="divisor"></param>
        /// <param name="value"></param>
        private void setExact(ulong divisor, decimal value)
        {
            decimal result = value * ((decimal)divisor);
            sizeInBytes = (ulong)Math.Round(result);
        }
        #endregion
        
        #region generic Unit accessors
        private ulong divisor(Unit u)
        {
            ulong divisor = 1;
            for (ushort i = 0; i < (ushort)u; i++)
                divisor *= 1024;
            return divisor;
        }

        /// <summary>
        /// Returns the stored size in the requested units (rounded)
        /// </summary>
        /// <param name="shift"></param>
        /// <returns></returns>
        public ulong InUnits(Unit u)
        {
            return divideAndRound(divisor(u));
        }

        /// <summary>
        /// Returns the stored size in the requested units (exact)
        /// </summary>
        /// <param name="u"></param>
        /// <returns></returns>
        public decimal InUnitsExact(Unit u)
        {
            return ((decimal)sizeInBytes) / ((decimal)divisor(u));
        }

        public void SetWithUnits(Unit u, decimal value)
        {
            setExact(divisor(u), value);
        }
        #endregion 
        
        #region pretty formatting
        public Unit BestUnit
        {
            get
            {
                Unit u = Unit.B;
                while (u < Unit.GB)
                {
                    if (InUnitsExact(u) < 1000M)
                        break;
                    u++;
                }
                return u;
            }
        }
        public override string ToString()
        {
            Unit u = BestUnit;
            decimal d = InUnitsExact(u);
            return (Math.Round(d, 2)).ToString("0.00") + " " + UnitSuffixes[(ushort)u];
        }
        public static FileSize Parse(string s)
        {
            string[] parts = s.Split(' ');
            return new FileSize((Unit)Array.IndexOf(UnitSuffixes, parts[1]), decimal.Parse(parts[0]));
            
        }
        #endregion
        
        #region byte, KB, MB, GB accessors (exact and rounded)
        /// <summary>
        /// The size in bytes
        /// </summary>
        public ulong Bytes
        {
            get { return sizeInBytes; }
            set { sizeInBytes = value; }
        }

        /// <summary>
        /// The size in kilobytes (rounded)
        /// </summary>
        public ulong KB
        {
            get { return divideAndRound(1024); }
            set { checked { sizeInBytes = value * 1024; } }
        }

        /// <summary>
        /// The size in kilobytes (exact)
        /// </summary>
        public decimal KBExact
        {
            get { return ((decimal)sizeInBytes) / 1024M; }
            set { setExact(1024, value); }
        }

        /// <summary>
        /// The size in megabytes (rounded)
        /// </summary>
        public ulong MB
        {
            get { return divideAndRound(1024*1024); }
            set { checked { sizeInBytes = value * 1024 * 1024; } }
        }

        /// <summary>
        /// The size in megabytes (exact)
        /// </summary>
        public decimal MBExact
        {
            get { return ((decimal)sizeInBytes) / (1024M * 1024M); }
            set { setExact(1024 * 1024, value); }
        }

        /// <summary>
        /// Returns the size in gigabytes (rounded)
        /// </summary>
        public ulong GB
        {
            get { return divideAndRound(1024 * 1024 * 1024); }
            set { checked { sizeInBytes = value * 1024 * 1024 * 1024; } }
        }

        /// <summary>
        /// The size in gigabytes (exact)
        /// </summary>
        public decimal GBExact
        {
            get { return ((decimal)sizeInBytes) / (1024M * 1024M * 1024M); }
            set { setExact(1024 * 1024 * 1024, value); }
        }
        #endregion

        #region operators: + - * / < > <= >= == !=
        public static FileSize operator +(FileSize a, FileSize b)
        {
            FileSize result;
            checked
            {
                result.sizeInBytes = a.sizeInBytes + b.sizeInBytes;
            }
            return result;
        }
        public static FileSize operator -(FileSize a, FileSize b)
        {
            FileSize result;
            checked
            {
                result.sizeInBytes = a.sizeInBytes - b.sizeInBytes;
            }
            return result;
        }
        public static decimal operator /(FileSize a, FileSize b)
        {
            return ((decimal)a.sizeInBytes) / ((decimal)b.sizeInBytes);
        }

        public static FileSize operator /(FileSize a, decimal b)
        {
            return (a * (1.0M / b));
        }

        public static FileSize operator *(decimal a, FileSize b)
        {
            FileSize result;
            checked
            {
                result.sizeInBytes = (ulong)Math.Round(a * (decimal)b.sizeInBytes);
            }
            return result;
        }
        public static FileSize operator *(FileSize a, decimal b)
        {
            return b * a;
        }

        public static bool operator <(FileSize a, FileSize b)
        {
            return (a.sizeInBytes < b.sizeInBytes);
        }
        
        public static bool operator >(FileSize a, FileSize b)
        {
            return b < a;
        }

        public static bool operator ==(FileSize a, FileSize b)
        {
            return (a.sizeInBytes == b.sizeInBytes);
        }

        public static bool operator !=(FileSize a, FileSize b)
        {
            return !(a == b);
        }

        public static bool operator <=(FileSize a, FileSize b)
        {
            return (a.sizeInBytes <= b.sizeInBytes);
        }

        public static bool operator >=(FileSize a, FileSize b)
        {
            return (b <= a);
        }
        #endregion

        public override int GetHashCode()
        {
            return sizeInBytes.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return (obj is FileSize && ((FileSize)obj).sizeInBytes == sizeInBytes);
        }

        public FileSize(ulong numBytes)
        {
            sizeInBytes = numBytes;
        }

        public FileSize(long numBytes)
        {
            if (numBytes > 0)
                sizeInBytes = (ulong)numBytes;
            else
                sizeInBytes = 0;
        }

        public FileSize(Unit u, decimal value)
        {
            sizeInBytes = 0; // Dummy
            SetWithUnits(u, value);
        }
    }

    class FileSizeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
                return true;
            return base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (value is string)
                return FileSize.Parse((string)value);
            return base.ConvertFrom(context, culture, value);
        }
    }
}