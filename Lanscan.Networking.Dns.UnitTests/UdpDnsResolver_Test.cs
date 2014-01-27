//////////////////////////////////////////////////////////////////////
//
// Lanscan
// Copyright (C) 2013, Richard Cook. All rights reserved.
//
//////////////////////////////////////////////////////////////////////

namespace Lanscan.Networking.Dns.UnitTests
{
    using System;
    using System.Linq;
    using Lanscan.Networking;
    using Lanscan.Networking.Dns;
    using Lanscan.Networking.Dns.Records;
    using Lanscan.TestFramework;
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

    public static class On_UdpDnsResolver
    {
        [TestClass]
        public sealed class QueryTest
        {
            private static readonly byte[] ExpectedSendData = new byte[]
            {
                138, 26, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 3, 49, 54, 55, 2, 49, 49, 3, 49, 54, 56, 3, 49, 57, 50, 7, 105, 110, 45, 97, 100, 100, 114, 4, 97, 114, 112, 97, 0, 0, 12, 0, 1
            };
            private static readonly byte[] ReceivedData = new byte[]
            {
                138, 26, 133, 128, 0, 1, 0, 1, 0, 0, 0, 0, 3, 49, 54, 55, 2, 49, 49, 3, 49, 54, 56, 3, 49, 57, 50, 7, 105, 110, 45, 97, 100, 100, 114, 4, 97, 114, 112, 97, 0, 0, 12, 0, 1, 192, 12, 0, 12, 0, 1, 0, 0, 0, 0, 0, 26, 5, 97, 108, 112, 104, 97, 14, 114, 105, 99, 104, 97, 114, 100, 97, 110, 100, 107, 97, 116, 121, 3, 99, 111, 109, 0
            };

            [TestMethod]
            public void Test_IQUERY_UDP()
            {
                var sendToInvoked = false;
                var receiveInvoked = false;
                var disposeInvoked = false;

                var datagramSocketFactory = new MockDatagramSocketFactory(
                    (data, endpoint) =>
                    {
                        Assert.IsNotNull(data);
                        data.ShouldEqual(ExpectedSendData);
                        Assert.AreEqual("192.168.11.1", endpoint.Address.ToString());
                        Assert.AreEqual((ushort)53, endpoint.Port);

                        Assert.IsFalse(sendToInvoked);
                        Assert.IsFalse(receiveInvoked);
                        Assert.IsFalse(disposeInvoked);
                        sendToInvoked = true;
                    },
                    buffer =>
                    {
                        Assert.IsNotNull(buffer);
                        Assert.AreEqual(512, buffer.Length);

                        Assert.IsTrue(sendToInvoked);
                        Assert.IsFalse(receiveInvoked);
                        Assert.IsFalse(disposeInvoked);

                        Array.Copy(ReceivedData, buffer, ReceivedData.Length);
                        var result = ReceivedData.Length;
                        receiveInvoked = true;
                        return result;
                    },
                    () =>
                    {
                        Assert.IsTrue(sendToInvoked);
                        Assert.IsTrue(receiveInvoked);
                        Assert.IsFalse(disposeInvoked);
                        disposeInvoked = true;
                    });

                var options = new DnsResolverOptions
                {
                    Timeout = 1000,
                    RetryCount = 3,
                    Recursion = true,
                    UseCache = true
                };
                var resolver = new UdpDnsResolver(
                    options,
                    datagramSocketFactory,
                    new IPEndpoint(IPAddress.Parse("192.168.11.1"), 53), new Random(0));

                var response = resolver.Query("167.11.168.192.in-addr.arpa", QType.PTR, QClass.IN);
                Assert.AreEqual(0, response.Error.Length);

                Assert.AreEqual(1, response.header.ANCOUNT);
                Assert.AreEqual(1, response.Answers.Count);

                var answerRR = response.Answers.First();

                Assert.AreEqual("167.11.168.192.in-addr.arpa.", answerRR.NAME);
                Assert.AreEqual(0u, answerRR.TTL);
                Assert.AreEqual(Class.IN, answerRR.Class);
                Assert.AreEqual(Lanscan.Networking.Dns.RecordType.PTR, answerRR.Type);
                Assert.IsInstanceOfType(answerRR.RECORD, typeof(PTRRecord));
                var recordPtr = (PTRRecord)answerRR.RECORD;
                Assert.AreEqual("alpha.example.com.", recordPtr.PTRDNAME);

                Assert.IsTrue(sendToInvoked);
                Assert.IsTrue(receiveInvoked);
                Assert.IsTrue(disposeInvoked);
            }

            private delegate void SendToInvokedHandler(byte[] data, IPEndpoint endpoint);
            private delegate int ReceiveInvokedHandler(byte[] buffer);
            private delegate void DisposeInvokedHandler();

            private sealed class MockDatagramSocketFactory : IDatagramSocketFactory
            {
                private readonly SendToInvokedHandler m_sendToInvokedHandler;
                private readonly ReceiveInvokedHandler m_receiveInvokedHandler;
                private readonly DisposeInvokedHandler m_disposeInvokedHandler;

                public MockDatagramSocketFactory(SendToInvokedHandler sendToInvokedHandler, ReceiveInvokedHandler receiveInvokedHandler, DisposeInvokedHandler disposeInvokedHandler)
                {
                    if (sendToInvokedHandler == null)
                    {
                        throw new ArgumentNullException("sendToInvokedHandler");
                    }
                    if (receiveInvokedHandler == null)
                    {
                        throw new ArgumentNullException("receiveInvokedHandler");
                    }
                    if (disposeInvokedHandler == null)
                    {
                        throw new ArgumentNullException("disposeInvokedHandler");
                    }

                    m_sendToInvokedHandler = sendToInvokedHandler;
                    m_receiveInvokedHandler = receiveInvokedHandler;
                    m_disposeInvokedHandler = disposeInvokedHandler;
                }

                public IDatagramSocket CreateDatagramSocket(int timeout)
                {
                    var result = new SocketImpl(this);
                    return result;
                }

                private sealed class SocketImpl : IDatagramSocket
                {
                    private readonly MockDatagramSocketFactory m_socketFactory;

                    public SocketImpl(MockDatagramSocketFactory socketFactory)
                    {
                        if (socketFactory == null)
                        {
                            throw new ArgumentNullException("socketFactory");
                        }

                        m_socketFactory = socketFactory;
                    }

                    public void SendTo(byte[] data, IPEndpoint endpoint)
                    {
                        m_socketFactory.m_sendToInvokedHandler(data, endpoint);
                    }

                    public int Receive(byte[] buffer)
                    {
                        var result = m_socketFactory.m_receiveInvokedHandler(buffer);
                        return result;
                    }

                    public void Dispose()
                    {
                        m_socketFactory.m_disposeInvokedHandler();
                    }
                }
            }
        }
    }
}
