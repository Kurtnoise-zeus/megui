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
    /// <summary>
    /// Checks a mux path for any outright problems that absolutely cannot be tolerated.
    /// </summary>
    /// <param name="muxPath"></param>
    /// <returns>True if a problem is found</returns>
    public delegate bool CheckMuxPath(MuxPath muxPath);
    /// <summary>
    /// Compare two mux paths to find which one is better by some given metric.
    /// </summary>
    /// <param name="x">first mux path</param>
    /// <param name="y">second mux path</param>
    /// <returns>negative if x is better, positive if y is better, 0 if they are equal</returns>
    public delegate int MuxPathCompare(MuxPath x, MuxPath y);

    /// <summary>
    /// Class to transparently handle specific cases for mux paths. 
    /// By default, the 'best' mux path is chosen according to which is shortest. However, if other
    /// factors come into play, these can be added be creating a new compare method, and registering it.
    /// Registering it with a lower order means it will regarded as more important than ones with higher order.
    /// 
    /// Similarly, this class manages checking for invalid mux paths. No examples spring to mind right now,
    /// but an invalid mux path is something that appears to the mux path generator to be valid, but is in fact
    /// not.
    /// </summary>
    public class MuxPathComparer
    {
        #region variables
        private List<PreferencedComparer> comparisonMethods = new List<PreferencedComparer>();
        private List<CheckMuxPath> checkingMethods = new List<CheckMuxPath>();
        private bool comparisonSorted = false;
        #endregion
        #region start / stop
        public MuxPathComparer()
        {
            registerChecker(CheckForUnsupportedCodecs);
            registerChecker(CheckForVFWMatroska);
            registerComparer(0, CompareByLength);
            registerComparer(100, CompareByAACContainer); // A higher order number means it is less important
        }
        #endregion
        #region registering
        public void registerComparer(int order, MuxPathCompare method)
        {
            PreferencedComparer p = new PreferencedComparer();
            p.method = method;
            p.preference = order;
            lock (this)
            {
                comparisonMethods.Add(p);
                comparisonSorted = false;
            }
        }
        public void registerChecker(CheckMuxPath checker)
        {
            checkingMethods.Add(checker);
        }
        #endregion
        #region IComparer<MuxPath> Members (generic comparing method)
        /// <summary>
        /// Returns the result of the comparison of the two mux paths according to all the registered muxcomparers
        /// </summary>
        /// <param name="x">The first mux path to compare</param>
        /// <param name="y">The second mux path to compare</param>
        /// <returns>Negative if x is better, positive if y is better, 0 if equally good.</returns>
        int Compare(MuxPath x, MuxPath y)
        {
            lock (this)
            {
                if (!comparisonSorted)
                {
                    comparisonMethods.Sort(Compare);
                    comparisonSorted = true;
                }

                foreach (PreferencedComparer comparer in comparisonMethods)
                {
                    int result = comparer.method(x, y);
                    if (result != 0)
                        return result;
                }
                return 0;
            }
        }
        #endregion
        #region generic checking method
        /// <summary>
        /// Checks if this mux path is invalid, according to any of the checkers registered.
        /// </summary>
        /// <param name="path"></param>
        /// <returns>false if valid or no checkers found, true if invalid.</returns>
        bool BadMuxPath(MuxPath path)
        {
            lock (this)
            {
                foreach (CheckMuxPath checker in checkingMethods)
                {
                    if (checker(path))
                        return true;
                }
                return false;
            }
        }

        #endregion
        #region finding the 'best'
        public MuxPath GetBestMuxPath(List<MuxPath> muxPaths)
        {
            muxPaths.RemoveAll(BadMuxPath);
            if (muxPaths.Count == 0)
                return null;
            
            muxPaths.Sort(Compare);
            return muxPaths[0];
        }
        #endregion
        #region comparison methods
        /// <summary>
        /// The most primitive comparison method. Says the shorter ones are better
        /// </summary>
        /// <param name="x">The first mux path</param>
        /// <param name="y">The second mux path</param>
        /// <returns>x.Length - y.Length</returns>
        public static int CompareByLength(MuxPath x, MuxPath y)
        {
            return x.Length - y.Length;
        }
        /// <summary>
        /// Compares x to y by counting the number of raw AAC files that are input. Fewer is better,
        /// because the muxers can't pick up whether SBR is signalled if it is in raw aac format
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static int CompareByAACContainer(MuxPath x, MuxPath y)
        {
            int xInitialRawAAC = 0, yInitialRawAAC = 0;
            foreach (MuxableType initialType in x.InitialInputTypes)
            {
                if (initialType.outputType.Equals(AudioType.RAWAAC))
                    xInitialRawAAC++;
            }
            foreach (MuxableType initialType in y.InitialInputTypes)
            {
                if (initialType.outputType.Equals(AudioType.RAWAAC))
                    yInitialRawAAC++;
            }
            return xInitialRawAAC - yInitialRawAAC;
        }

        public static bool CheckForUnsupportedCodecs(MuxPath x)
        {
            for (int i = 0; i < x.Length; i++)
            {
                foreach (MuxableType type in x[i].handledInputTypes)
                {
                    for (int j = i; j < x.Length; j++)
                    {
                        if (type.codec is VideoCodec)
                        {
                            if (!x[j].muxerInterface.SupportsVideoCodec((VideoCodec)type.codec))
                                return true;
                        }
                        else if (type.codec is AudioCodec)
                        {
                            if (!x[j].muxerInterface.SupportsAudioCodec((AudioCodec)type.codec))
                                return true;
                        }
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Checks whether a mux path for AVC-in-MKV uses VFW-based storage (which is bad).
        /// </summary>
        /// <param name="x"></param>
        /// <returns>true if VFW storage is found</returns>
        public static bool CheckForVFWMatroska(MuxPath x)
        {
            if (x.TargetType != ContainerType.MKV)
                return false;

            foreach (MuxPathLeg mpl in x)
            {
               /* if (mpl.muxerInterface is AVC2AVIMuxerProvider)
                {
                    return true;
                }*/
            }
            return false;
        }
        #endregion
        #region Ordering comparer
        static int Compare(PreferencedComparer a, PreferencedComparer b)
        {
            return a.preference - b.preference;
        }
        #endregion
    }

    struct PreferencedComparer
    {
        public int preference;
        public MuxPathCompare method;
    }
}
