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
using System.IO;

namespace MeGUI
{
    /// <summary>
    /// Container object for PgcDemux
    /// </summary>
    public class PgcDemuxJob : Job
    {
        private string _strOutputFileName;
        private string _strTemporaryPath;
        private int _pgcNumber;
        private int _angleNumber;

        public PgcDemuxJob() : this(null, null, 1, 0) { }

        public PgcDemuxJob(string strInput, string strOutputFileName, int pgcNumber, int angleNumber)
            : base(strInput, strOutputFileName)
        {
            this._strOutputFileName = strOutputFileName;
            this._pgcNumber = pgcNumber;
            this._angleNumber = angleNumber;
            setTemporaryPath();
        }

        private void setTemporaryPath()
        {
            if (String.IsNullOrEmpty(this._strOutputFileName))
                return;

            this._strTemporaryPath = Path.Combine(Path.GetDirectoryName(_strOutputFileName),
                Path.GetFileNameWithoutExtension(_strOutputFileName).Substring(0, Path.GetFileNameWithoutExtension(_strOutputFileName).Length - 2));

            FilesToDelete.Add(Path.Combine(_strTemporaryPath, "LogFile.txt"));
            FilesToDelete.Add(Path.Combine(_strTemporaryPath, "Celltimes.txt"));
            for (int i = 1; i < 10; i++)
                FilesToDelete.Add(Path.Combine(_strTemporaryPath, "VTS_01_" + i + ".VOB"));
            FilesToDelete.Add(_strTemporaryPath);
        }

        public override string CodecString
        {
            get { return "pgcdemux"; }
        }

        public override string EncodingMode
        {
            get { return "ext"; }
        }

        public string OutputFileName
        {
            get { return _strOutputFileName; }
            set
            {
                _strOutputFileName = value;
                setTemporaryPath();
            }
        }

        public string TemporaryPath
        {
            get { return _strTemporaryPath; }
            set { _strTemporaryPath = value; }
        }

        public int PGCNumber
        {
            get { return _pgcNumber; }
            set { _pgcNumber = value; }
        }

        public int AngleNumber
        {
            get { return _angleNumber; }
            set { _angleNumber = value; }
        }
    }
}