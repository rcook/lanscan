//////////////////////////////////////////////////////////////////////
//
// Lanscan
// Copyright (C) 2013, Richard Cook. All rights reserved.
//
//////////////////////////////////////////////////////////////////////

namespace Lanscan.Networking
{
    using System;

    public static class Ping
    {
        public static bool PerformPing(IPEndpoint endpoint, Protocol protocol)
        {
            if (endpoint == null)
            {
                throw new ArgumentNullException("endpoint");
            }

            using (var pingWrapper = CreatePingWrapper(protocol))
            {
                var result = pingWrapper.PerformPing(endpoint);
                return result;
            }
        }

        private static PingWrapper CreatePingWrapper(Protocol protocol)
        {
            switch (protocol)
            {
                case Protocol.Tcp: return new TcpPingWrapper();
                case Protocol.Udp: return new UdpPingWrapper();
                default:
                    throw new ArgumentException("Invalid protocol", "protocol");
            }
        }
    }
}
