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

namespace MeGUI
{
    public class MediaFileFactory
    {
        private MainForm mainForm;
        public MediaFileFactory(MainForm mainForm)
        {
            this.mainForm = mainForm;
        }

        public IMediaFile Open(string file)
        {
            int bestHandleLevel = -1;
            IMediaFile bestMediaFile = null;
            foreach (IMediaFileFactory factory in mainForm.PackageSystem.MediaFileTypes.Values)
            {
                int handleLevel = factory.HandleLevel(file);
                if (handleLevel < 0)
                    continue;
                try
                {
                    if (handleLevel > bestHandleLevel)
                    {
                        IMediaFile mFile = factory.Open(file);
                        if (mFile != null)
                        {
                            bestHandleLevel = handleLevel;
                            if (bestMediaFile != null)
                                bestMediaFile.Dispose();
                            bestMediaFile = mFile;
                        }
                    }
                }
                catch (Exception ex) { string test = ex.Message; }
            }
            return bestMediaFile;
        }
    }
}