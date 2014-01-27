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

    public sealed class NetworkServicePresenterComparer : IComparer<NetworkServicePresenter>
    {
        private static readonly NetworkServicePresenterComparer InstanceInternal = new NetworkServicePresenterComparer();

        private NetworkServicePresenterComparer()
        {
        }

        public static NetworkServicePresenterComparer Instance
        {
            get { return InstanceInternal; }
        }

        public int Compare(NetworkServicePresenter x, NetworkServicePresenter y)
        {
            if (Object.ReferenceEquals(null, x))
            {
                return Object.ReferenceEquals(null, y) ? 0 : -1;
            }
            if (Object.ReferenceEquals(null, y))
            {
                return 1;
            }

            if (x.Protocol == Protocol.Tcp)
            {
                if (y.Protocol == Protocol.Udp)
                {
                    return -1;
                }
            }
            else
            {
                if (y.Protocol == Protocol.Tcp)
                {
                    return 1;
                }
            }

            var result = x.Port.CompareTo(y.Port);
            if (result != 0)
            {
                return result;
            }

            result = x.Name.CompareTo(y.Name);
            if (result != 0)
            {
                return result;
            }

            return 0;
        }
    }
}
