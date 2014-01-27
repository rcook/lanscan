//////////////////////////////////////////////////////////////////////
//
// Lanscan
// Copyright (C) 2013, Richard Cook. All rights reserved.
//
//////////////////////////////////////////////////////////////////////

namespace Lanscan.Networking
{
    using System;

    public static class IPNetworkHelper
    {
        public static bool IsValidNetworkPrefixLength(byte value)
        {
            var result = value <= 32;
            return result;
        }

        public static uint GetNetworkMaskValue(byte prefixLength)
        {
            if (!IsValidNetworkPrefixLength(prefixLength))
            {
                throw new ArgumentOutOfRangeException("prefixLength");
            }

            var temp1 = 1L << (32 - prefixLength);
            var temp2 = temp1 - 1;
            var temp3 = ~temp2;
            var result = unchecked((uint)temp3);
            return result;
        }
    }
}
