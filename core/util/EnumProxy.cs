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
using System.Text;

namespace MeGUI
{
    [AttributeUsage(AttributeTargets.Field)]
    public class EnumTitleAttribute: Attribute
    {
        private string text;
        private object tag;

        public string Text
        {
            get { return this.text; }
        }
        public object Tag
        {
            get { return this.tag; }
        }
        public override string ToString()
        {
            return Text;
        }
        public EnumTitleAttribute(string text, object tag)
        {
            this.text = text;
            this.tag = tag;
        }

        public EnumTitleAttribute(string text):this(text, null)
        {
        }

        
    }
    
    class EnumProxy
    {
        private object realValue;
        private static readonly Dictionary<object, EnumProxy> cache = new Dictionary<object, EnumProxy>();
        private static readonly object lockObject = new object();
        private EnumTitleAttribute attribute;
        
        public static EnumProxy Create(object v)
        {
            lock (lockObject)
            {
                if (cache.ContainsKey(v))
                    return cache[v];
                else
                {
                    EnumProxy p = new EnumProxy(v);
                    cache.Add(v, p);
                    return p;
                }
            }
        }
        
        private EnumProxy(object v)
        {
            this.realValue = v;
            System.Type t = v.GetType();
            FieldInfo fi = t.GetField(v.ToString());
            object[] attr = fi.GetCustomAttributes(typeof(EnumTitleAttribute), false);
            if (null == attr || attr.Length == 0)
                this.attribute = new EnumTitleAttribute(v.ToString());
            else
                this.attribute = attr[0] as EnumTitleAttribute;
        }

        public override string ToString()
        {
            return this.attribute.Text;
        }
        
        public object RealValue
        {
            get
            {
                return this.realValue;
            }
        }

        public object Tag
        {
            get
            {
                return this.attribute.Tag;
            }
        }
       
        
        public static EnumProxy[] CreateArray(System.Collections.IList list)
        {
            EnumProxy[] arr = new EnumProxy[list.Count];
            for(int i=0; i<arr.Length; i++)
            {
                arr[i] = Create(list[i]);
            }
            return arr;
        }

        public static EnumProxy[] CreateArray(System.Type enumType)
        {
            if (enumType == null)
                throw new ArgumentNullException("enumType");
            if (!enumType.IsEnum)
                throw new ArgumentException("enumType must be Enum", "enumType");
            FieldInfo[] enumItems = enumType.GetFields(BindingFlags.Public | BindingFlags.Static);
            EnumProxy[] arr = new EnumProxy[enumItems.Length];
            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = Create( enumItems[i].GetValue(null) );
            }
            return arr;
        }
        
        public static int IndexOf(System.Collections.IList values, object valueToFind)
        {
            for (int i = 0; i < values.Count; i++)
            {
                if ((int)valueToFind == (int)(values[i] as EnumProxy).RealValue)
                    return i;
            }
            return -1;            
        }     
    }
}
