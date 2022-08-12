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

namespace eac3to
{
    /// <summary>A File</summary>
    public class File
    {
        public int Index { get; set; }
        public string Name { get { return FullName.Substring(0, FullName.IndexOf('.')); } }
        public string Extension { get { return FullName.Substring(FullName.IndexOf('.') + 1); } }
        public string FullName { get; set; }

        public File() { }

        public File(string fileName, int index)
        {
            if (string.IsNullOrEmpty(fileName))
                throw new ArgumentNullException("fileName", "The string 'fileName' cannot be null or empty.");

            FullName = fileName;
            Index = index;
        }

        public override string ToString()
        {
            return FullName;
        }
    }
}
