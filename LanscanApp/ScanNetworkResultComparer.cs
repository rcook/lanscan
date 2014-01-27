//////////////////////////////////////////////////////////////////////
//
// Lanscan
// Copyright (C) 2013, Richard Cook. All rights reserved.
//
//////////////////////////////////////////////////////////////////////

namespace LanscanApp
{
    using System;
    using System.Collections.Generic;
    using Lanscan.Networking;

    public sealed class ScanNetworkResultComparer : IComparer<ScanNetworkResult>
    {
        private static readonly ScanNetworkResultComparer InstanceInternal = new ScanNetworkResultComparer();

        private ScanNetworkResultComparer()
        {
        }

        public static ScanNetworkResultComparer Instance
        {
            get { return InstanceInternal; }
        }

        public int Compare(ScanNetworkResult x, ScanNetworkResult y)
        {
            if (Object.ReferenceEquals(null, x))
            {
                return Object.ReferenceEquals(null, y) ? 0 : -1;
            }
            if (Object.ReferenceEquals(null, y))
            {
                return 1;
            }

            var result = DefaultIPEndpointComparer.Instance.Compare(x.Endpoint, y.Endpoint);
            return result;
        }
    }
}
