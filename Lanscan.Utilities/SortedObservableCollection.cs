//////////////////////////////////////////////////////////////////////
//
// Lanscan
// Copyright (C) 2013, Richard Cook. All rights reserved.
//
//////////////////////////////////////////////////////////////////////

namespace Lanscan.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    public sealed class SortedObservableCollection<T> : ObservableCollection<T>
    {
        private readonly IComparer<T> m_comparer;

        public SortedObservableCollection(IComparer<T> comparer)
        {
            if (comparer == null)
            {
                throw new ArgumentNullException("comparer");
            }

            m_comparer = comparer;
        }

        protected override void InsertItem(int index, T item)
        {
            for (var i = 0; i < Count; ++i)
            {
                switch (Compare(this[i], item))
                {
                    case 0:
                        throw new ArgumentException("Duplicate item", "item");
                    case 1:
                        base.InsertItem(i, item);
                        return;
                    case -1:
                        break;
                }
            }
            base.InsertItem(Count, item);
        }

        private int Compare(T x, T y)
        {
            var result = Math.Sign(m_comparer.Compare(x, y));
            return result;
        }
    }
}
