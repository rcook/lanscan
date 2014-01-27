//////////////////////////////////////////////////////////////////////
//
// Lanscan
// Copyright (C) 2013, Richard Cook. All rights reserved.
//
//////////////////////////////////////////////////////////////////////

namespace Lanscan.Networking
{
    using System;
    using System.Threading.Tasks;
    using Windows.Networking.Sockets;
    using Windows.Storage.Streams;

    public sealed class UdpPingWrapper : PingWrapper
    {
        private const int PingAsyncTimeout = 100;
        private static readonly byte[] CanaryData = new byte[] { 1, 2, 3, 4, 5 };
        private TaskCompletionSource<bool> m_taskCompletionSource = new TaskCompletionSource<bool>();
        private DatagramSocket m_socket = new DatagramSocket();

        public UdpPingWrapper()
        {
        }

        public override bool PerformPing(IPEndpoint endpoint)
        {
            if (endpoint == null)
            {
                throw new ArgumentNullException("endpoint");
            }

            var task = PingAsync(endpoint);
            if (!task.Wait(PingAsyncTimeout))
            {
                return false;
            }

            var result = task.Result;
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

        private static bool IsRemoteEndpoint(DatagramSocketMessageReceivedEventArgs args, IPEndpoint endpoint)
        {
            try
            {
                var result = args.RemoteAddress.IsEqual(endpoint.Address.HostName) && args.RemotePort.Equals(endpoint.ServiceName);
                return result;
            }
            catch
            {
                return false;
            }
        }

        private async Task<bool> PingAsync(IPEndpoint endpoint)
        {
            m_socket.MessageReceived += (sender, e) =>
            {
                var isRemoteEndpoint = IsRemoteEndpoint(e, endpoint);
                m_taskCompletionSource.TrySetResult(isRemoteEndpoint);
            };

            await m_socket.ConnectAsync(endpoint.Address.HostName, endpoint.ServiceName);
            var writer = new DataWriter(m_socket.OutputStream);
            writer.WriteBytes(CanaryData);
            await writer.StoreAsync();

            var result = await m_taskCompletionSource.Task;
            return result;
        }
    }
}
