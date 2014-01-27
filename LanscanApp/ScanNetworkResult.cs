//////////////////////////////////////////////////////////////////////
//
// Lanscan
// Copyright (C) 2013, Richard Cook. All rights reserved.
//
//////////////////////////////////////////////////////////////////////

namespace LanscanApp
{
    using System;
    using Lanscan.Networking;

    public sealed class ScanNetworkResult
    {
        private readonly Protocol m_protocol;
        private readonly IPEndpoint m_endpoint;
        private readonly string m_hostName;
        private readonly bool m_isAvailable;

        public ScanNetworkResult(Protocol protocol, IPEndpoint endpoint, string hostName, bool isAvailable)
        {
            if (endpoint == null)
            {
                throw new ArgumentNullException("endpoint");
            }
            if (hostName != null && String.IsNullOrWhiteSpace(hostName))
            {
                throw new ArgumentException("Invalid host name", "hostName");
            }

            m_protocol = protocol;
            m_endpoint = endpoint;
            m_hostName = hostName;
            m_isAvailable = isAvailable;
        }

        public Protocol Protocol
        {
            get { return m_protocol; }
        }

        public IPEndpoint Endpoint
        {
            get { return m_endpoint; }
        }

        public string HostName
        {
            get { return m_hostName; }
        }

        public bool IsAvailable
        {
            get { return m_isAvailable; }
        }
    }
}
