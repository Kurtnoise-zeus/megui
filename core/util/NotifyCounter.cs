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
using System.Diagnostics;
using System.Text;

namespace MeGUI.core.util
{
    /// <summary>
    /// Keeps track of resources, including how many tasks are accessing it.
    /// The idea is to allow code to temporarily disable certain functions, and re-enable it later.
    /// Furthermore, this is nestable.
    /// 
    /// See FileBar for a usage of this.
    /// </summary>
    public class NotifyCounter
    {
        private uint count = 0;
        
        /// <summary>
        /// Returns whether this function/resource can be used, ie whether anyone still forbids its use.
        /// </summary>
        public bool Ready
        {
            get { return (count == 0); }
        }

        /// <summary>
        /// Add another forbid of its use.
        /// </summary>
        public void Increment() { count++; }

        /// <summary>
        /// Take of one person forbidding its use.
        /// </summary>
        public void Decrement() { Debug.Assert(count > 0); count--; }
        
        /// <summary>
        /// To be used in a using() { } statement, to wrap a Increment/Decrement pair:
        /// using(IDisposable a = myAccessor.Wrap())
        /// {
        ///     // This code hase myAccessor disabled.
        /// } // but the curly brace ensures you don't forget to re-enable it
        /// </summary>
        /// <returns></returns>
        public IDisposable Wrap()
        {
            return new AutoCloser(this);
        }

        internal class AutoCloser : IDisposable
        {
            private NotifyCounter counter;

            internal AutoCloser(NotifyCounter counter)
            {
                this.counter = counter;
                counter.Increment();
            }

            public void Dispose()
            {
                counter.Decrement();
            }
        }
    }
}
