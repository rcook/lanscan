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
    using System.Globalization;
    using System.Threading;
    using System.Threading.Tasks;
    using Lanscan.Networking;
    using Lanscan.Networking.Dns;
    using Windows.Networking.Sockets;
    using Windows.Storage.Streams;

    public sealed class DatagramSocketFactory : IDatagramSocketFactory
    {
        public DatagramSocketFactory()
        {
        }

        public IDatagramSocket CreateDatagramSocket(int timeout)
        {
            if (timeout < 1)
            {
                throw new ArgumentOutOfRangeException("timeout");
            }

            var result = new SocketImpl(timeout);
            return result;
        }

        private sealed class SocketImpl : IDatagramSocket
        {
            private readonly int m_timeout;
            private DatagramSocket m_socket;
            private AutoResetEvent m_messageReceivedEvent;
            private List<Message> m_messages = new List<Message>();

            public SocketImpl(int timeout)
            {
                if (timeout < 1)
                {
                    throw new ArgumentOutOfRangeException("timeout");
                }

                m_timeout = timeout;
                m_socket = new DatagramSocket();
                m_messageReceivedEvent = new AutoResetEvent(false);
                m_socket.MessageReceived += OnMessageReceived;
            }

            public void SendTo(byte[] data, IPEndpoint endpoint)
            {
                Task.Factory.StartNew(async () =>
                {
                    //try
                    //{
                    await m_socket.ConnectAsync(endpoint.Address.HostName, endpoint.Port.ToString(CultureInfo.InvariantCulture));
                    //}
                    //catch (Exception ex)
                    //{
                    //    Logger.Trace("ConnectAsync failed: {0}", ex);
                    //    throw;
                    //}

                    var writer = new DataWriter(m_socket.OutputStream);
                    writer.WriteBytes(data);
                    await writer.StoreAsync();
                });
            }

            public int Receive(byte[] buffer)
            {
                if (!m_messageReceivedEvent.WaitOne(Timeout.Infinite/*m_timeout*/))
                {
                    return 0;
                }

                var bufferIndex = 0;
                var bufferRemaining = buffer.Length;
                lock (m_messages)
                {
                    foreach (var message in m_messages)
                    {
                        if (message.Data.Length > bufferRemaining)
                        {
                            throw new InvalidOperationException("Buffer not big enough");
                        }
                        Array.Copy(message.Data, 0, buffer, bufferIndex, message.Data.Length);
                        bufferIndex += message.Data.Length;
                        bufferRemaining -= message.Data.Length;
                    }
                }
                return bufferIndex;
            }

            public void Dispose()
            {
                if (m_messageReceivedEvent != null)
                {
                    m_messageReceivedEvent.Dispose();
                    m_messageReceivedEvent = null;
                }
                if (m_socket != null)
                {
                    m_socket.Dispose();
                    m_socket = null;
                }
            }

            private void OnMessageReceived(DatagramSocket sender, DatagramSocketMessageReceivedEventArgs args)
            {
                //Logger.Trace("OnMessageReceived: endpoint = {0}, port = {1}", args.RemoteAddress, args.RemotePort);

                var reader = args.GetDataReader();
                var count = reader.UnconsumedBufferLength;
                var data = new byte[count];
                reader.ReadBytes(data);
                lock (m_messages)
                {
                    m_messages.Add(new Message { Data = data });
                }
                m_messageReceivedEvent.Set();
            }

            private sealed class Message
            {
                public byte[] Data;
            }
        }
    }
}
