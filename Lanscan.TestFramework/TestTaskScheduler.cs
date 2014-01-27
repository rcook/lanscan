//////////////////////////////////////////////////////////////////////
//
// Lanscan
// Copyright (C) 2013, Richard Cook. All rights reserved.
//
//////////////////////////////////////////////////////////////////////

namespace Lanscan.TestFramework
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    // http://stackoverflow.com/questions/7852960/how-to-write-unit-tests-with-tpl-and-taskscheduler
    public sealed class TestTaskScheduler : TaskScheduler
    {
        private readonly Queue<Task> m_tasks = new Queue<Task>();

        public TestTaskScheduler()
        {
        }

        protected override IEnumerable<Task> GetScheduledTasks()
        {
            return m_tasks;
        }

        protected override void QueueTask(Task task)
        {
            if (task == null)
            {
                throw new ArgumentNullException("task");
            }

            m_tasks.Enqueue(task);
        }

        protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
        {
            if (task == null)
            {
                throw new ArgumentNullException("task");
            }

            task.RunSynchronously();
            return true;
        }

        public void RunAll()
        {
            while (m_tasks.Count > 0)
            {
                m_tasks.Dequeue().RunSynchronously();
            }
        }
    }
}
