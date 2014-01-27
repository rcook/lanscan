//////////////////////////////////////////////////////////////////////
//
// Lanscan
// Copyright (C) 2013, Richard Cook. All rights reserved.
//
//////////////////////////////////////////////////////////////////////

namespace Lanscan.DataContracts.UnitTests
{
    using System;
    using System.Collections.Generic;

    public sealed class V1ServiceComparer : IComparer<V1Service>
    {
        public V1ServiceComparer()
        {
        }

        public int Compare(V1Service x, V1Service y)
        {
            if (Object.ReferenceEquals(x, y))
            {
                return 0;
            }
            if (Object.ReferenceEquals(null, x))
            {
                return -1;
            }
            if (Object.ReferenceEquals(null, y))
            {
                return 1;
            }

            var result = x.Port.CompareTo(y.Port);
            if (result != 0)
            {
                return result;
            }

            result = x.Protocol.CompareTo(y.Protocol);
            if (result != 0)
            {
                return result;
            }

            result = x.Name.CompareTo(y.Name);
            if (result != 0)
            {
                return result;
            }

            result = x.IsEnabled.CompareTo(y.IsEnabled);
            return result;
        }
    }
}
