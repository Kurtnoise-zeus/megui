// ****************************************************************************
// 
// Copyright (C) 2005-2026 Doom9 & al
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
using System.Collections;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace MeGUI.core.util
{
    public enum ImageType { Error = 0, Warning = 1, Information = 2, NoImage = 3 }

    public class EventArgs<TData> : EventArgs
    {
        public TData Data
        {
             get { return data; }
        }

        private TData data;

        public EventArgs(TData t)
        {
            data = t;
        }
    }

    public class LogItem
    {
        public event EventHandler<EventArgs<LogItem>> SubItemAdded;
        public event EventHandler<EventArgs<ImageType>> TypeChanged;
        public event EventHandler Expanded;
        public event EventHandler Collapsed;

        public void Expand()
        {
            if (Expanded != null)
                Expanded(this, new EventArgs());
        }

        public void Collapse()
        {
            if (Collapsed != null)
                Collapsed(this, new EventArgs());
        }

        public string Text
        {
            get { return text; }
        }

        private int iLevel = 0;
        public int Level
        {
            get { return iLevel;  }
            set { iLevel = value; }
        }

        public ImageType Type
        {
            get { return type; }
        }

        public List<LogItem> SubEvents
        {
            get { return subEvents; }
        }

        public LogItem(string name) : this(name, ImageType.Information, false) { }

        public LogItem(string name, ImageType type) : this(name, type, false) { }

        public LogItem(string name, ImageType type, bool bIncludeTime)
        {
            this.text = bIncludeTime ? string.Format("[{0:G}] {1}", DateTime.Now, name) : name;
            this.type = type;
            if (type == ImageType.Warning)
                MainForm.Instance.setOverlayIcon(System.Drawing.SystemIcons.Warning, false);
            else if (type == ImageType.Error)
                MainForm.Instance.setOverlayIcon(System.Drawing.SystemIcons.Error, false);
        }

        public LogItem Add(LogItem logItem)
        {
            logItem.Level = iLevel + 1;
            logItem.WriteLogEntry();
            subEvents.Add(logItem);
            if (SubItemAdded != null)
                SubItemAdded(this, new EventArgs<LogItem>(logItem));

            logItem.TypeChanged += subItemTypeChanged;
            subItemTypeChanged(logItem, new EventArgs<ImageType>(logItem.Type));

            return logItem;
        }

        public LogItem LogValue(string name, object value)
        {
            return LogValue(name, value, ImageType.Information, true);
        }

        public LogItem LogValue(string name, object value, bool bIncludeTime)
        {
            return LogValue(name, value, ImageType.Information, bIncludeTime);
        }

        public LogItem LogValue(string name, object value, ImageType im)
        {
            return LogValue(name, value, im, true);
        }

        public LogItem LogValue(string name, object value, ImageType im, bool bIncludeTime)
        {
            return Add(AutomatedLogger.LogValue(bIncludeTime ? string.Format("[{0:G}] {1}", DateTime.Now, name) : name , value, im));
        }

        public LogItem LogEvent(string eventName)
        {
            return LogEvent(eventName, ImageType.Information, true);
        }

        public LogItem LogEvent(string eventName, ImageType image)
        {
            return LogEvent(eventName, image, true);
        }

        public LogItem LogEvent(string eventName, bool bIncludeTime)
        {
            return LogEvent(eventName, ImageType.Information, bIncludeTime);
        }

        public LogItem LogEvent(string eventName, ImageType image, bool bIncludeTime)
        {
            return Add(new LogItem(bIncludeTime ? string.Format("[{0:G}] {1}", DateTime.Now, eventName) : eventName, image));
        }

        public LogItem Info(string text)
        {
            return Add(new LogItem(text, ImageType.Information));
        }

        public LogItem Warn(string text)
        {
            return Add(new LogItem(text, ImageType.Warning));
        }

        public LogItem Error(string text)
        {
            return Add(new LogItem(text, ImageType.Error));
        }

        private ImageType type;
        private string text;

        private List<LogItem> subEvents = new List<LogItem>();

        private void subItemTypeChanged(object sender, EventArgs<ImageType> typeInfo)
        {
            if (typeInfo.Data < this.Type)
            {
                this.type = typeInfo.Data;
                if (TypeChanged != null)
                    TypeChanged(this, typeInfo);
            }
        }

        public override string ToString()
        {
            return ToString(0);
        }

        public string ToString(bool includeSubnodes)
        {
            if (includeSubnodes)
                return ToString(0);
            else
                return string.Format("[{0}] {1}", Type, Text);
        }

        private string ToString(int level)
        {
            StringBuilder res = new StringBuilder();

            try
            {
                res.AppendFormat("{0}[{1}] {2}{3}", dashes(level), Type, Text, Environment.NewLine);

                foreach (LogItem i in SubEvents)
                    res.Append(i.ToString(level + 1));
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            return res.ToString();
        }

        private void WriteLogEntry()
        {
            StringBuilder res = new StringBuilder();
            res.AppendFormat("{0}[{1}] {2}{3}", dashes(iLevel), Type, Text, Environment.NewLine);
            FileUtil.WriteToFile(MainForm.Instance.LogFile, res.ToString(), true, false);
        }

        private static string dashes(int number)
        {
            char[] s = new char[number];
            for (int i = 0; i < number; ++i)
                s[i] = '-';

            return new string(s);
        }

    }

    public delegate LogItem TypeHandler(string message, object o, ImageType i);

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public class LogByMembersAttribute : Attribute { }
    
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public class LogAsToStringAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public class CustomLoggerAttribute : Attribute
    {
        public TypeHandler CustomLogger
        {
            get { return customLogger; }
        }

        private TypeHandler customLogger;

        public CustomLoggerAttribute(TypeHandler logger)
        {
            this.customLogger = logger;
        }
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class LogIgnoreAttribute : Attribute { }

    public class AutomatedLogger
    {
        private static List<Type> typesHandled = new List<Type>();
        private static Dictionary<Type, TypeHandler> typeHandlers = new Dictionary<Type, TypeHandler>();

        public static void Register(Type ty, TypeHandler h)
        {
            Debug.Assert(!typeHandlers.ContainsKey(ty));
            typesHandled.Add(ty);
            typeHandlers[ty] = h;
        }

        public static LogItem LogValue(string name, object value, ImageType image)
        {
            if (value == null)
                value = "null"; // to avoid NullReferenceExceptions

            List<Type> candidates = new List<Type>(typesHandled);

            // Remove the types which can't handle value
            candidates.RemoveAll(delegate(Type t)
            {
                return !t.IsInstanceOfType(value);
            });

            // Ensure we used the most derived type
            candidates.RemoveAll(delegate(Type t)
            {
                foreach (Type t2 in candidates)
                {
                    if (t2.IsSubclassOf(t))
                        return true;
                }
                return false;
            });

            List<TypeHandler> handlerCands = candidates.ConvertAll<TypeHandler>(delegate(Type t) { return typeHandlers[t]; });

            Type ty = value.GetType();

            if (ty.IsDefined(typeof(LogByMembersAttribute), true))
                handlerCands.Add(LogByMembers);
            if (ty.IsDefined(typeof(CustomLoggerAttribute), true))
            {
                CustomLoggerAttribute[] c = (CustomLoggerAttribute[])ty.GetCustomAttributes(typeof(CustomLoggerAttribute), true);
                Debug.Assert(c.Length == 1);
                handlerCands.Add(c[0].CustomLogger);
            }
            if (handlerCands.Count == 0 || ty.IsDefined(typeof(LogAsToStringAttribute), true))
                handlerCands.Add(LogAsToString);

            // We are guaranteed to have at least one logger: LogAsToString
            if (handlerCands.Count == 1)
                return handlerCands[0](name, value, image);
            else
            {
                LogItem i = new LogItem(name, image);
                i.Warn(string.Format("More than 1 loggers are defined for type '{0}'.", ty));
                foreach (TypeHandler t in handlerCands)
                    i.Add(t("Value", value, ImageType.NoImage));
                return i;
            }
        }

        public static LogItem LogAsToString(string message, object o, ImageType i)
        {
            string[] lines = o.ToString().Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            if (lines.Length == 1)
                return new LogItem(message + ": " + lines[0], i);
            else
            {
                LogItem[] events = Array.ConvertAll<string, LogItem>(lines,
                    delegate(string s) { return new LogItem(s, ImageType.NoImage); });
                LogItem r = new LogItem(message, i);
                foreach (LogItem e in events)
                    r.Add(e);

                return r;
            }
        }

        public static LogItem LogByMembers(string message, object o, ImageType im)
        {
            LogItem i = new LogItem(message, im);
            Type ty = o.GetType();

            foreach (PropertyInfo p in ty.GetProperties())
            {
                if (!(p.CanRead && p.GetGetMethod().IsPublic) ||
                        p.IsDefined(typeof(LogIgnoreAttribute), true))
                    continue;

                i.LogValue(p.Name, p.GetValue(o, null));
            }

            foreach (FieldInfo f in ty.GetFields())
            {
                if ((!f.IsPublic) || f.IsDefined(typeof(LogIgnoreAttribute), true))
                    continue;

                i.LogValue(f.Name, f.GetValue(o));
            }

            return i;
        }

        static AutomatedLogger()
        {
            Register(typeof(Exception), ExceptionHandler);
            Register(typeof(Array), ArrayHandler);
        }

        private static LogItem ExceptionHandler(string message, object o, ImageType i)
        {
            Exception e = (Exception)o;

            LogItem l = new LogItem(message, ImageType.Error);
            l.LogValue("Exception message", e.Message, ImageType.Error);
            l.LogValue("Stacktrace", e.StackTrace, ImageType.Error);
            l.LogValue("Inner exception", e.InnerException, ImageType.Error);

            foreach (DictionaryEntry info in e.Data)
                l.LogValue(info.Key.ToString(), info.Value, ImageType.Error);

            return l;
        }

        private static LogItem ArrayHandler(string message, object o, ImageType im)
        {
            Array a = (Array)o;
            int len = a.GetLength(0);

            LogItem l = new LogItem(message, im);
            l.LogValue("array.Length", len);

            for (int i = 0; i < len; ++i)
                l.LogValue(string.Format("array[{0}]", i), a.GetValue(i));
            
            return l;
        }
    }
}
