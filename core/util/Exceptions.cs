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
using System.Text;

namespace MeGUI.core.util
{
    // The base class for MeGUI-triggered exceptions
    public class MeGUIException : Exception
    { 
        public MeGUIException(string message) : base(message) { }

        public MeGUIException(Exception inner) : base(inner.Message, inner) { }

        public MeGUIException(string message, Exception inner) : base(message, inner) { }

    }

    public class JobRunException : MeGUIException
    {
        public JobRunException(string message) : base(message) { }

        public JobRunException(Exception inner) : base(inner.Message, inner) { }

        public JobRunException(string message, Exception inner) : base(message, inner) { }
    }

    public class MissingFileException : JobRunException
    {
        public string filename;

        public MissingFileException(string file)
            : base("Required file '" + file + "' is missing.")
        {
            filename = file;
        }
    }

    public class EncoderMissingException : MissingFileException
    {
        public EncoderMissingException(string file) : base(file) { }
    }
}
