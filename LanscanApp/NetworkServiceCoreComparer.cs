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

    public sealed class NetworkServiceCoreComparer : EqualityComparer<INetworkServiceCore>
    {
        private static readonly NetworkServiceCoreComparer InstanceInternal = new NetworkServiceCoreComparer();

        private NetworkServiceCoreComparer()
        {
        }

        public static NetworkServiceCoreComparer Instance
        {
            get { return InstanceInternal; }
        }

        public override bool Equals(INetworkServiceCore x, INetworkServiceCore y)
        {
            if (Object.ReferenceEquals(null, x))
            {
                return Object.ReferenceEquals(null, y);
            }
            if (Object.ReferenceEquals(null, y))
            {
                return false;
            }

            var result = x.Protocol.Equals(y.Protocol) && x.Port.Equals(y.Port);
            return result;
        }

        public override int GetHashCode(INetworkServiceCore obj)
        {
            if (Object.ReferenceEquals(null, obj))
            {
                throw new ArgumentNullException("obj");
            }

            var result = obj.Protocol.GetHashCode() ^ obj.Port.GetHashCode();
            return result;
        }
    }
}
