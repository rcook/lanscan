//////////////////////////////////////////////////////////////////////
//
// Lanscan
// Copyright (C) 2013, Richard Cook. All rights reserved.
//
//////////////////////////////////////////////////////////////////////

namespace Lanscan.Networking.Dns
{
    using System;
    using System.Globalization;
    using System.Linq;

    public static class DnsHelper
    {
        public static string GetArpaUrl(IPAddress address)
        {
            if (address == null)
            {
                throw new ArgumentNullException("address");
            }

            var result = String.Format(
                CultureInfo.InvariantCulture,
                "{0}.in-addr.arpa.",
                String.Join(".", address.GetAddressBytes().Reverse()));
            return result;
        }
    }
}
