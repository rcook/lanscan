//////////////////////////////////////////////////////////////////////
//
// Lanscan
// Copyright (C) 2013, Richard Cook. All rights reserved.
//
//////////////////////////////////////////////////////////////////////

namespace Lanscan.Networking
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    public sealed class ConnectionInfo
    {
        private readonly IPAddress m_address;
        private readonly IPNetwork m_network;

        public ConnectionInfo(IPAddress address, IPNetwork network)
        {
            if (address == null)
            {
                throw new ArgumentNullException("address");
            }
            if (network == null)
            {
                throw new ArgumentNullException("network");
            }

            m_address = address;
            m_network = network;
        }

        public IPAddress Address
        {
            get { return m_address; }
        }

        public IPNetwork Network
        {
            get { return m_network; }
        }

        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        public static ConnectionInfo GetConnectionInfo(HostInfo hostInfo)
        {
            if (hostInfo == null)
            {
                throw new ArgumentNullException("hostInfo");
            }

            var network = new IPNetwork(hostInfo.Address, hostInfo.HostName.IPInformation.PrefixLength.Value);
            var result = new ConnectionInfo(hostInfo.Address, network);
            return result;
        }
    }
}
