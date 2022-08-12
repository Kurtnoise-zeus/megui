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
using System.Text;

using MeGUI.core.util;

namespace MeGUI
{
    [LogByMembers]
    public class DialogSettings
    {
        private bool ovewriteJobOutputResponse = true;
        public bool OverwriteJobOutputResponse
        {
            get { return ovewriteJobOutputResponse; }
            set { ovewriteJobOutputResponse = value; }
        }

        private bool askAboutOverwriteJobOutput = true;
        public bool AskAboutOverwriteJobOutput
        {
            get { return askAboutOverwriteJobOutput; }
            set { askAboutOverwriteJobOutput = value; }
        }

        private bool dupResponse;
        public bool DuplicateResponse
        {
            get { return dupResponse; }
            set { dupResponse = value; }
        }

        private bool askAboutDuplicates;
        public bool AskAboutDuplicates
        {
            get { return askAboutDuplicates; }
            set { askAboutDuplicates = value; }
        }

        private bool askAboutError;
        public bool AskAboutError
        {
            get { return askAboutError; }
            set { askAboutError = value; }
        }

        private bool continueDespiteError;
        public bool ContinueDespiteError
        {
            get { return continueDespiteError; }
            set { continueDespiteError = value; }
        }

        private bool askAboutColorConersion;
        public bool AskAboutColorConversion
        {
            get { return askAboutColorConersion; }
            set { askAboutColorConersion = value; }
        }

        private bool askAboutUpdates;
        public bool AskAboutUpdates
        {
            get { return askAboutUpdates; }
            set { askAboutUpdates = value; }
        }

        private bool addConvertTo;
        public bool AddConvertTo
        {
            get { return addConvertTo; }
            set { addConvertTo = value; }
        }

        private bool askAboutVOBs;
        public bool AskAboutVOBs
        {
            get { return askAboutVOBs; }
            set { askAboutVOBs = value; }
        }

        private bool useOneClick;
        public bool UseOneClick
        {
            get { return useOneClick; }
            set { useOneClick = value; }
        }

        private bool askAboutIntermediateDelete;
        public bool AskAboutIntermediateDelete
        {
            get { return askAboutIntermediateDelete; }
            set { askAboutIntermediateDelete = value; }
        }

        private bool intermediateDelete;
        public bool IntermediateDelete
        {
            get { return intermediateDelete; }
            set { intermediateDelete = value; }
        }

        public DialogSettings()
        {
            askAboutVOBs = true;
            useOneClick = true;
            askAboutError = true;
            askAboutColorConersion = true;
            addConvertTo = true;
            continueDespiteError = true;
            askAboutDuplicates = true;
            dupResponse = true;
            askAboutIntermediateDelete = true;
            intermediateDelete = true;
            askAboutUpdates = true;
        }
    }
}