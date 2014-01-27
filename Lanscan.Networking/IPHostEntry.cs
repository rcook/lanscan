//////////////////////////////////////////////////////////////////////
//
// Lanscan
// Copyright (C) 2013, Richard Cook. All rights reserved.
//
//////////////////////////////////////////////////////////////////////

namespace Lanscan.Networking
{
    using System;
    using System.Collections.Generic;

    public sealed class IPHostEntry
    {
        private readonly string m_hostName;
        private readonly IPAddress[] m_addresses;
        private readonly string[] m_aliases;

        public IPHostEntry(string hostName, IPAddress[] addresses, string[] aliases)
        {
            if (string.IsNullOrWhiteSpace(hostName))
            {
                throw new ArgumentException("Invalid host name", "hostName");
            }
            if (addresses == null)
            {
                throw new ArgumentNullException("addresses");
            }
            if (aliases == null)
            {
                throw new ArgumentNullException("aliases");
            }

            m_hostName = hostName;
            m_addresses = addresses;
            m_aliases = aliases;
        }

        public string HostName
        {
            get { return m_hostName; }
        }

        public IEnumerable<IPAddress> AddressList
        {
            get { return m_addresses; }
        }

        public IEnumerable<string> Aliases
        {
            get { return m_aliases; }
        }
    }
}
