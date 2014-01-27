//////////////////////////////////////////////////////////////////////
//
// Lanscan
// Copyright (C) 2013, Richard Cook. All rights reserved.
//
//////////////////////////////////////////////////////////////////////

namespace Lanscan.Utilities
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public static class TaskExtensions
    {
        public static async Task TimeoutAfter(this Task task, int millisecondsTimeout, CancellationToken cancellationToken)
        {
            if (task == null)
            {
                throw new ArgumentNullException("task");
            }
            if (millisecondsTimeout < 1)
            {
                throw new ArgumentOutOfRangeException("millisecondsTimeout");
            }

            if (task == await Task.WhenAny(task, Task.Delay(millisecondsTimeout, cancellationToken)))
            {
                await task;
            }
            else
            {
                cancellationToken.ThrowIfCancellationRequested();
                throw new TimeoutException();
            }
        }

        public static async Task TimeoutAfter(this Task task, int millisecondsTimeout)
        {
            if (task == null)
            {
                throw new ArgumentNullException("task");
            }
            if (millisecondsTimeout < 1)
            {
                throw new ArgumentOutOfRangeException("millisecondsTimeout");
            }

            if (task == await Task.WhenAny(task, Task.Delay(millisecondsTimeout)))
            {
                await task;
            }
            else
            {
                throw new TimeoutException();
            }
        }

        public static async Task<T> TimeoutAfter<T>(this Task<T> task, int millisecondsTimeout)
        {
            if (task == null)
            {
                throw new ArgumentNullException("task");
            }
            if (millisecondsTimeout < 1)
            {
                throw new ArgumentOutOfRangeException("millisecondsTimeout");
            }

            if (task == await Task.WhenAny(task, Task.Delay(millisecondsTimeout)))
            {
                var result = await task;
                return result;
            }
            else
            {
                throw new TimeoutException();
            }
        }

        public static async Task<T> TimeoutAfter<T>(this Task<T> task, int millisecondsTimeout, CancellationToken cancellationToken)
        {
            if (task == null)
            {
                throw new ArgumentNullException("task");
            }
            if (millisecondsTimeout < 1)
            {
                throw new ArgumentOutOfRangeException("millisecondsTimeout");
            }

            if (task == await Task.WhenAny(task, Task.Delay(millisecondsTimeout, cancellationToken)))
            {
                var result = await task;
                return result;
            }
            else
            {
                cancellationToken.ThrowIfCancellationRequested();
                throw new TimeoutException();
            }
        }
    }
}
