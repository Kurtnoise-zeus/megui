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
using System.Reflection;

namespace MeGUI.core.plugins.interfaces
{
    public interface GenericSettings : IEquatable<GenericSettings>, ICloneable
    {
        /************************************************************************************
         *                   Classes implementing GenericSettings must                      *
         *                    ensure that object.Equals(object other)                       *
         *                     is overridden and is correct for the                         *
         *                                 given class.                                     *
         ************************************************************************************/

        /// <summary>
        /// Deep-clones the settings
        /// </summary>
        /// <returns></returns>
        new GenericSettings Clone();

        /// <summary>
        /// Returns the meta type of a profile. This is used as a lookup in the ProfileManager class
        /// to group like profile types. There should be one meta-type per settings type.
        /// </summary>
        /// <returns></returns>
        string SettingsID { get; }
        
        /// <summary>
        /// Substitutes any filenames stored in this profile (eg quantizer matrices) according to
        /// the substitution table
        /// </summary>
        /// <param name="substitutionTable"></param>
        void FixFileNames(Dictionary<string, string> substitutionTable);

        /// <summary>
        /// Lists all the files that these codec settings depend upon
        /// </summary>
        string[] RequiredFiles { get; }

        /// <summary>
        /// Lists all the profiles that these codec settings depend upon
        /// </summary>
        string[] RequiredProfiles { get; }

    }


    [AttributeUsage(AttributeTargets.Property)]
    public class PropertyEqualityIgnoreAttribute : Attribute
    {
        public PropertyEqualityIgnoreAttribute() {}
    }


    public class PropertyEqualityTester
    {
        /// <summary>
        /// Returns whether all of the properties (excluding those with the PropertyEqualityIgnoreAttribute)
        /// of the two objects are equal
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool AreEqual(object a, object b)
        {
            // If they are the same object or are both null
            if (a == b)
                return true;

            // If only one is null
            if (a == null || b == null || (a.GetType() != b.GetType()))
                return false;

            Type t = a.GetType();
            foreach (PropertyInfo info in t.GetProperties())
            {
                if (info.IsDefined(typeof(PropertyEqualityIgnoreAttribute), true))
                    continue;

                object aVal = null, bVal = null;
                try 
                { 
                    aVal = info.GetValue(a, null); 
                }
                catch { }
                try 
                { 
                    bVal = info.GetValue(b, null); 
                }
                catch { }
                if (!ArrayEqual(aVal, bVal)) 
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Returns whether these two objects are equal. Returns object.Equals except for arrays,
        /// where it recursively does an elementwise comparison
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        private static bool ArrayEqual(object a, object b)
        {
            if (a == b)
                return true;

            if (a == null || b == null || (a.GetType() != b.GetType()))
                return false;

            if (!a.GetType().IsArray)
                return a.Equals(b);

            object[] arrayA = (object[])a;
            object[] arrayB = (object[])b;

            if (arrayA.Length != arrayB.Length)
                return false;

            for (int i = 0; i < arrayA.Length; i++)
            {
                if (!ArrayEqual(arrayA[i], arrayB[i]))
                    return false;
            }
            return true;
        }
    }
}