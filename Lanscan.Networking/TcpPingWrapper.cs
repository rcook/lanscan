//////////////////////////////////////////////////////////////////////
//
// Lanscan
// Copyright (C) 2013, Richard Cook. All rights reserved.
//
//////////////////////////////////////////////////////////////////////

namespace Lanscan.Networking
{
    using System;
    using Windows.Networking.Sockets;

    public sealed class TcpPingWrapper : PingWrapper
    {
        private const int ConnectAsyncTimeout = 50;
        private StreamSocket m_socket = new StreamSocket();

        public TcpPingWrapper()
        {
        }

        public override bool PerformPing(IPEndpoint endpoint)
        {
            if (endpoint == null)
            {
                throw new ArgumentNullException("endpoint");
            }

            var result = m_socket.ConnectAsync(endpoint.Address.HostName, endpoint.ServiceName).AsTask().Wait(ConnectAsyncTimeout);
            return result;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (m_socket != null)
                {
                    m_socket.Dispose();
                    m_socket = null;
                }
            }
            base.Dispose(disposing);
        }
    }
}
