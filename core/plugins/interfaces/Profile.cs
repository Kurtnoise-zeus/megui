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

using System.Diagnostics;
using System.Xml.Serialization;

using MeGUI.core.plugins.interfaces;

namespace MeGUI
{
	/// <summary>
	/// Superclass of an actual video or audio profile
	/// defines some basic properties
	/// </summary>
	public abstract class Profile : IIDable
	{
		private string name; // name of the profile
		/// <summary>
		///  default constructor, initializes the private variables
		/// </summary>
		public Profile():this("default")
		{
		}

		public Profile(string name)
		{
			this.name = name;
		}

		/// <summary>
		/// Local name of the profile. Within a given settings type, this
        /// is guaranteed to be unique, but not within the entire profile
        /// collection
		/// </summary>
        public string Name
        {
            get
            {
                Debug.Assert(!string.IsNullOrEmpty(name));
                return name;
            }
            set
            {
                name = value;
                Debug.Assert(!string.IsNullOrEmpty(name));
            }
        }

        /// <summary>
        /// Fully qualified name in format 'type: name'. For a given profile
        /// collection, this is guaranteed to be unique.
        /// </summary>
        public string FQName
        {
            get { return BaseSettings.SettingsID + ": " + Name; }
        }

        public override string ToString()
        {
            return Name;
        }

        [XmlIgnore]
        public abstract GenericSettings BaseSettings
        {
            get;
            set;
        }

        public abstract Profile baseClone();

        public string ID { get { return FQName; } }
	}

    public class GenericProfile<TSettings> : Profile
        where TSettings : GenericSettings
    {
		private TSettings settings;

		public GenericProfile():base()
		{
		}
		public GenericProfile(string name, TSettings settings):base(name)
		{
			this.settings = settings;
		}
		
        [XmlIgnore]
        public override GenericSettings BaseSettings
		{
			get {return settings;}
            set { System.Diagnostics.Debug.Assert(value is TSettings); settings = (TSettings)value; }
		}

        public TSettings Settings
        {
            get { return settings; }
            set { settings = value; }
        }
        public GenericProfile<TSettings> clone()
        {
            return new GenericProfile<TSettings>(Name, (TSettings)settings.Clone());
        }
        public override Profile baseClone()
        {
            return clone();
        }
    }
}