//////////////////////////////////////////////////////////////////////
//
// Lanscan
// Copyright (C) 2013, Richard Cook. All rights reserved.
//
//////////////////////////////////////////////////////////////////////

namespace Lanscan.Networking
{
    using System.Collections.Generic;

    public sealed class DefaultIPEndpointComparer : IComparer<IPEndpoint>
    {
        private static readonly DefaultIPEndpointComparer Instance1 = new DefaultIPEndpointComparer();

        private DefaultIPEndpointComparer()
        {
        }

        public static DefaultIPEndpointComparer Instance
        {
            get { return Instance1; }
        }

        public int Compare(IPEndpoint x, IPEndpoint y)
        {
            if (x == null)
            {
                return y == null ? 0 : -1;
            }
            if (y == null)
            {
                return 1;
            }

            var addressResult = x.Address.Value.CompareTo(y.Address.Value);
            if (addressResult != 0)
            {
                return addressResult;
            }

            var result = x.Port.CompareTo(y.Port);
            return result;
        }
    }
}
