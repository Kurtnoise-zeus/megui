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

namespace MeGUI.core.details
{
    #region public part
    public abstract partial class JobChain {
        public static implicit operator JobChain(Job j)
        {
            return new JobDependencyChain(j);
        }
    }

    public sealed partial class SequentialChain : JobChain
    {
        public SequentialChain(params JobChain[] chains)
        {
            resolve(chains);
        }

        public SequentialChain(params Job[] jobs) : this(DUtil.convert(jobs)) { }
    }

    public sealed partial class ParallelChain : JobChain
    {
        public ParallelChain(params JobChain[] chains)
        {
            resolve(chains);
        }

        public ParallelChain(params Job[] jobs) : this(DUtil.convert(jobs)) {}
    }
    #endregion 

    #region private part
    class DUtil
    {
        internal static JobChain[] convert(Job[] jobs)
        {
            JobChain[] d = new JobChain[jobs.Length];
            for (int i = 0; i < jobs.Length; ++i)
                d[i] = jobs[i];

            return d;
        }
    }
    internal delegate void MakeDependant(TaggedJob j);

    partial class JobChain
    {
        internal abstract TaggedJob[] Jobs { get; }
        internal abstract void MakeJobDependOnChain(TaggedJob allowedEnd);
        internal abstract void MakeStartDepend(MakeDependant requiredEnd);
    }

    internal sealed class EmptyJobChain : JobChain
    {

        internal override TaggedJob[] Jobs
        {
            get { return new TaggedJob[] { }; }
        }

        internal override void MakeJobDependOnChain(TaggedJob allowedEnd)
        {
            
        }

        internal override void MakeStartDepend(MakeDependant requiredEnd)
        {
            
        }
    }

    internal sealed class JobDependencyChain : JobChain
    {
        internal TaggedJob j;
        internal TaggedJob[] jobs;

        internal JobDependencyChain(Job j)
        {
            this.j = new TaggedJob(j);
            jobs = new TaggedJob[] { this.j };
        }

        internal override TaggedJob[] Jobs
        {
            get
            {
                return jobs;
            }
        }

        internal override void MakeJobDependOnChain(TaggedJob allowedEnd)
        {
            allowedEnd.AddDependency(j);
        }

        internal override void MakeStartDepend(MakeDependant requiredEnd)
        {
            requiredEnd(j);
        }
    }

    partial class ParallelChain {
        private TaggedJob[] jobs;
        private JobChain[] chains;

        private void resolve(JobChain[] chains)
        {
            this.chains = chains;

            List<TaggedJob> jobsConstructor = new List<TaggedJob>();
            foreach (JobChain chain in chains)
                jobsConstructor.AddRange(chain.Jobs);

            jobs = jobsConstructor.ToArray();
        }

        internal override TaggedJob[] Jobs
        {
            get { return jobs; }
        }

        internal override void MakeJobDependOnChain(TaggedJob j)
        {
            foreach (JobChain c in chains)
                c.MakeJobDependOnChain(j);
        }

        internal override void MakeStartDepend(MakeDependant requiredEnd)
        {
            foreach (JobChain c in chains)
                c.MakeStartDepend(requiredEnd);
        }
    }

    partial class SequentialChain 
    {
        private TaggedJob[] jobs;
        private JobChain first;
        private JobChain last;

        private void resolve(JobChain[] chains)
        {
            if (chains.Length == 0)
                chains = new JobChain[] { new EmptyJobChain() };

            List<TaggedJob> jobs = new List<TaggedJob>();
            JobChain last = null;

            for (int i = 0; i < chains.Length; i++ )
            {
                if (chains[i] == null || chains[i].Jobs.Length == 0)
                    continue;
                TaggedJob[] cjobs = chains[i].Jobs;
                if (last != null)
                    chains[i].MakeStartDepend(new MakeDependant(last.MakeJobDependOnChain));
                jobs.AddRange(cjobs);
                last = chains[i];
            }
            this.jobs = jobs.ToArray();
            for (int i = 0; i < chains.Length; i++)
            {
                this.first = chains[i];
                if (chains[i] != null && chains[i].Jobs.Length > 0)
                    break;
            }
            for (int i = chains.Length - 1; i >= 0; i--)
            {
                this.last = chains[i];
                if (chains[i] != null && chains[i].Jobs.Length > 0)
                    break;
            }
        }

        internal override TaggedJob[] Jobs
        {
            get { return jobs; }
        }

        internal override void MakeJobDependOnChain(TaggedJob j)
        {
            last.MakeJobDependOnChain(j);
        }

        internal override void MakeStartDepend(MakeDependant requiredEnd)
        {
            first.MakeStartDepend(requiredEnd);
        }

    }
    
    #endregion
}