//////////////////////////////////////////////////////////////////////
//
// Lanscan
// Copyright (C) 2013, Richard Cook. All rights reserved.
//
//////////////////////////////////////////////////////////////////////

namespace Lanscan.Networking
{
    using System;
    using System.Globalization;

    public sealed class IPEndpoint
    {
        private readonly IPAddress m_address;
        private readonly ushort m_port;
        private readonly string m_serviceName;

        public IPEndpoint(IPAddress address, ushort port)
        {
            if (address == null)
            {
                throw new ArgumentNullException("address");
            }

            m_address = address;
            m_port = port;
            m_serviceName = m_port.ToString(CultureInfo.InvariantCulture);
        }

        public IPAddress Address
        {
            get { return m_address; }
        }

        public ushort Port
        {
            get { return m_port; }
        }

        public string ServiceName
        {
            get { return m_serviceName; }
        }

        public override string ToString()
        {
            var result = String.Format(
                CultureInfo.InvariantCulture,
                "{0}:{1}",
                m_address,
                m_port);
            return result;
        }
    }
}
