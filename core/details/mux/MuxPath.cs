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

namespace MeGUI
{
    /// <summary>
    /// Provides a thin wrapper for a list of MuxPathLegs and adds some extra MuxPath-specific functionality
    /// </summary>
    public class MuxPath
    {
        private List<MuxPathLeg> path;
        private List<MuxableType> initialInputTypes;
        private ContainerType targetType;
        private bool alwaysMux;

        public ContainerType TargetType
        {
            get { return targetType; }
            set { targetType = value; }
        }

        public List<MuxableType> InitialInputTypes
        {
            get { return initialInputTypes; }
            set { initialInputTypes = value; }
        }

        public int Length
        {
            get { return path.Count; }
        }

        public void Add(MuxPathLeg leg)
        {
            path.Add(leg);
        }

        public MuxPath(ContainerType targetType)
        {
            path = new List<MuxPathLeg>();
            initialInputTypes = new List<MuxableType>();
            this.targetType = targetType;
            alwaysMux = false;
        }

        public MuxPath(IEnumerable<MuxableType> initialInputTypes, ContainerType targetType)
            : this(targetType)
        {
            this.initialInputTypes.AddRange(initialInputTypes);
        }

        public MuxPath(IEnumerable<MuxableType> initialInputTypes, ContainerType targetType, bool alwaysMux)
            : this(initialInputTypes, targetType)
        {
            this.alwaysMux = alwaysMux;
        }

        public MuxPath Clone()
        {
            MuxPath nMuxPath = new MuxPath(initialInputTypes, targetType);
            nMuxPath.path.AddRange(path);
            return nMuxPath;
        }

        public IEnumerator<MuxPathLeg> GetEnumerator()
        {
            return path.GetEnumerator();
        }

        public MuxPathLeg this[int index]
        {
            get { return path[index]; }
        }

        public bool IsCompleted()
        {
            if (path.Count == 0)
            {
                if (initialInputTypes.Count == 1 && MainForm.Instance.Settings.UseExternalMuxerX264 && this.targetType == ContainerType.MKV)
                    return false;

                return (initialInputTypes.Count == 0 || 
                    (initialInputTypes.Count == 1 && !alwaysMux && initialInputTypes[0].outputType != null 
                    && initialInputTypes[0].outputType.ContainerType == this.targetType));
            }
            else
            {
                return (path[path.Count - 1].unhandledInputTypes.Count == 0 &&
                    path[path.Count - 1].muxerInterface.GetSupportedContainerOutputTypes().Contains(targetType));
            }
        }
    }
}