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
    using System.Diagnostics.CodeAnalysis;

    public static class TestHelper
    {
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "args")]
        public static void ForceEvaluation(params object[] args)
        {
        }

        public static void ForEach<T>(IEnumerable<T> items, Action<T> action)
        {
            if (items == null)
            {
                throw new ArgumentNullException("items");
            }
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            foreach (var item in items)
            {
                action(item);
            }
        }
    }
}
