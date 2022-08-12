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


namespace MeGUI.packages.tools.hdbdextractor
{
    public class StringValueAttribute : System.Attribute
    {
        public string Value { get; set; }

        public StringValueAttribute(string value)
        {
            Value = value;
        }
    }

    public static class Extensions
    {
        static Dictionary<Enum, StringValueAttribute> stringValues = new Dictionary<Enum, StringValueAttribute>();

        public static string GetStringValue(Enum value)
        {    
            string output = null;
            Type valueType = value.GetType();
            Type stringValueAttributeType = typeof(StringValueAttribute);

            if (stringValues.ContainsKey(value))
                output = stringValues[value].Value;
            else
            {
                FieldInfo fi = valueType.GetField(value.ToString());
                StringValueAttribute[] attrs = fi.GetCustomAttributes(stringValueAttributeType, false) as StringValueAttribute[];

                if (attrs.Length > 0)
                {
                    stringValues.Add(value, attrs[0]);
                    output = attrs[0].Value;
                }
            }

            return output;
        }

        /// <summary>Given a set of words separated by a space return a word which starts with an uppercase.</summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToPascal(string value)
        {
            var s = value;
            s = s.Trim().ToLower(System.Globalization.CultureInfo.InvariantCulture);
           
            if (s.Length == 0)
                return value;

            string[] ar = null;

            if (s.IndexOf('_') > -1)
                ar = s.Split('_');
            else if (s.IndexOf(' ') > -1)
                ar = s.Split(' ');

            s = string.Empty;

            if (ar != null)
            {
                for (int i = 0; i < ar.Length; i++)
                    if (ar[i].Trim().Length > 0)
                        s += ar[i].Substring(0, 1).ToUpper(System.Globalization.CultureInfo.InvariantCulture) + ar[i].Substring(1);

                return s;
            }
            return value;
        }

        /// <summary>Given a set of words separated by a space or an underscore return a lower Camel cased word.</summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToCamel(string value)
        {
            var s = value;
            s = s.Trim().ToLower(System.Globalization.CultureInfo.InvariantCulture);
           
            if (s.Length == 0)
                return value;
            
            string[] ar = null;

            if (s.IndexOf('_') > -1)
                ar = s.Split('_');
            else if (s.IndexOf(' ') > -1)
                ar = s.Split(' ');

            if (ar != null)
            {
                for (int i = 0; i < ar.Length; i++)
                {
                    if (ar[i].Trim().Length > 0)
                    {
                        if (i == 0)
                            s = ar[i];
                        else
                            s += ar[i].Substring(0, 1).ToUpper(System.Globalization.CultureInfo.InvariantCulture) + ar[i].Substring(1);
                    }
                }

                return s;
            }

            return value;
        }

        /// <summary>Given a word or a phrase make the first letter of the phrase and the first letter after a point Upper case and all the rest lower case.</summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string Capitalize(string value)
        {
            var s = value;
            s = s.Trim().ToLower(System.Globalization.CultureInfo.InvariantCulture);
           
            if (s.Length == 0)
                return value;

            string[] ar = s.Split('.');
            s = string.Empty;

            if (ar != null)
            {
                for (int i = 0; i < ar.Length; i++)
                    if (ar[i].Trim().Length > 0)
                        s += ar[i].Trim().Substring(0, 1).ToUpper(System.Globalization.CultureInfo.InvariantCulture) + ar[i].Trim().Substring(1) + ". ";

                return s;
            }

            return value;
        }

        /// <summary>Given a word or a phrase make the first letter of each word uppercase.</summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string CapitalizeAll(string value)
        {
            var s = value;
            s = s.Trim().ToLower(System.Globalization.CultureInfo.InvariantCulture);
           
            if (s.Length == 0)
                return value;

            string[] ar = s.Split(' ');
            s = string.Empty;

            if (ar != null)
            {
                for (int i = 0; i < ar.Length; i++)
                    if (ar[i].Trim().Length > 0)
                        s += ar[i].Trim().Substring(0, 1).ToUpper(System.Globalization.CultureInfo.InvariantCulture) + ar[i].Trim().Substring(1) + " ";

                return s;
            }

            return value;
        }
    }
}
