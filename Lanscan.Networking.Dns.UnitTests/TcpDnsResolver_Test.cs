//////////////////////////////////////////////////////////////////////
//
// Lanscan
// Copyright (C) 2013, Richard Cook. All rights reserved.
//
//////////////////////////////////////////////////////////////////////

namespace Lanscan.Networking.Dns.UnitTests
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using Lanscan.Networking;
    using Lanscan.Networking.Dns;
    using Lanscan.Networking.Dns.Records;
    using Lanscan.TestFramework;
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

    public static class On_TcpDnsResolver
    {
        [TestClass]
        public sealed class QueryTest
        {
            private static readonly byte[] ExpectedSendData = new byte[]
            {
                0, 45, 138, 26, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 3, 49, 54, 55, 2, 49, 49, 3, 49, 54, 56, 3, 49, 57, 50, 7, 105, 110, 45, 97, 100, 100, 114, 4, 97, 114, 112, 97, 0, 0, 12, 0, 1
            };
            private static readonly byte[] ReceivedData = new byte[]
            {
                0, 83, 138, 26, 133, 128, 0, 1, 0, 1, 0, 0, 0, 0, 3, 49, 54, 55, 2, 49, 49, 3, 49, 54, 56, 3, 49, 57, 50, 7, 105, 110, 45, 97, 100, 100, 114, 4, 97, 114, 112, 97, 0, 0, 12, 0, 1, 192, 12, 0, 12, 0, 1, 0, 0, 0, 0, 0, 26, 5, 97, 108, 112, 104, 97, 14, 114, 105, 99, 104, 97, 114, 100, 97, 110, 100, 107, 97, 116, 121, 3, 99, 111, 109, 0
            };

            [TestMethod]
            public void Test_IQUERY_TCP()
            {
                var connectedInvoked = false;
                var beginConnectInvoked = false;
                var getSendStreamInvoked = false;
                var getReceiveStreamInvoked = false;
                var disposeInvoked = false;

                var sendStream = new MemoryStream();
                var receiveStream = new MemoryStream(ReceivedData);

                var streamSocketFactory = new MockStreamSocketFactory(
                    () =>
                    {
                        Assert.IsFalse(connectedInvoked);
                        Assert.IsTrue(beginConnectInvoked);
                        Assert.IsFalse(getSendStreamInvoked);
                        Assert.IsFalse(getReceiveStreamInvoked);
                        Assert.IsFalse(disposeInvoked);
                        connectedInvoked = true;
                        return true;
                    },
                    (address, port, requestCallback, state) =>
                    {
                        Assert.AreEqual("192.168.11.1", address.ToString());
                        Assert.AreEqual(53, port);
                        Assert.IsNull(requestCallback);
                        Assert.IsNull(state);

                        Assert.IsFalse(connectedInvoked);
                        Assert.IsFalse(beginConnectInvoked);
                        Assert.IsFalse(getSendStreamInvoked);
                        Assert.IsFalse(getReceiveStreamInvoked);
                        Assert.IsFalse(disposeInvoked);
                        var result = new MockConnectAsyncResult();
                        beginConnectInvoked = true;
                        return result;
                    },
                    () =>
                    {
                        Assert.IsTrue(connectedInvoked);
                        Assert.IsTrue(beginConnectInvoked);
                        Assert.IsFalse(getSendStreamInvoked);
                        Assert.IsFalse(getReceiveStreamInvoked);
                        Assert.IsFalse(disposeInvoked);
                        getSendStreamInvoked = true;
                        return sendStream;
                    },
                    () =>
                    {
                        Assert.IsTrue(connectedInvoked);
                        Assert.IsTrue(beginConnectInvoked);
                        Assert.IsTrue(getSendStreamInvoked);
                        Assert.IsFalse(getReceiveStreamInvoked);
                        Assert.IsFalse(disposeInvoked);
                        getReceiveStreamInvoked = true;
                        return receiveStream;
                    },
                    () =>
                    {
                        Assert.IsTrue(connectedInvoked);
                        Assert.IsTrue(beginConnectInvoked);
                        Assert.IsTrue(getSendStreamInvoked);
                        Assert.IsTrue(getReceiveStreamInvoked);
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
                var resolver = new TcpDnsResolver(
                    options,
                    streamSocketFactory,
                    new IPEndpoint(IPAddress.Parse("192.168.11.1"), 53), new Random(0));

                var response = resolver.Query("167.11.168.192.in-addr.arpa", QType.PTR, QClass.IN);
                Assert.AreEqual(0, response.Error.Length);

                sendStream.ToArray().ShouldEqual(ExpectedSendData);

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

                Assert.IsTrue(connectedInvoked);
                Assert.IsTrue(beginConnectInvoked);
                Assert.IsTrue(getSendStreamInvoked);
                Assert.IsTrue(disposeInvoked);
            }

            private delegate bool ConnectedInvokedHandler();
            private delegate IAsyncResult BeginConnectInvokedHandler(IPAddress address, ushort port, AsyncCallback requestCallback, object state);
            private delegate Stream GetStreamInvokedHandler();
            private delegate void DisposeInvokedHandler();

            private sealed class MockStreamSocketFactory : IStreamSocketFactory
            {
                private readonly ConnectedInvokedHandler m_connectedInvokedHandler;
                private readonly BeginConnectInvokedHandler m_beginConnectInvokedHandler;
                private readonly GetStreamInvokedHandler m_getSendStreamInvokedHandler;
                private readonly GetStreamInvokedHandler m_getReceiveStreamInvokedHandler;
                private readonly DisposeInvokedHandler m_disposeInvokedHandler;

                public MockStreamSocketFactory(ConnectedInvokedHandler connectedInvokedHandler, BeginConnectInvokedHandler beginConnectInvokedHandler, GetStreamInvokedHandler getSendStreamInvokedHandler, GetStreamInvokedHandler getReceiveStreamInvokedHandler, DisposeInvokedHandler disposedInvokedHandler)
                {
                    if (connectedInvokedHandler == null)
                    {
                        throw new ArgumentNullException("connectedInvokedHandler");
                    }
                    if (beginConnectInvokedHandler == null)
                    {
                        throw new ArgumentNullException("beginConnectInvokedHandler");
                    }
                    if (getSendStreamInvokedHandler == null)
                    {
                        throw new ArgumentNullException("getSendStreamInvokedHandler");
                    }
                    if (getReceiveStreamInvokedHandler == null)
                    {
                        throw new ArgumentNullException("getReceiveStreamInvokedHandler");
                    }
                    if (disposedInvokedHandler == null)
                    {
                        throw new ArgumentNullException("disposedInvokedHandler");
                    }

                    m_connectedInvokedHandler = connectedInvokedHandler;
                    m_beginConnectInvokedHandler = beginConnectInvokedHandler;
                    m_getSendStreamInvokedHandler = getSendStreamInvokedHandler;
                    m_getReceiveStreamInvokedHandler = getReceiveStreamInvokedHandler;
                    m_disposeInvokedHandler = disposedInvokedHandler;
                }

                public IStreamSocket CreateStreamSocket(int timeout)
                {
                    var result = new SocketImpl(this);
                    return result;
                }

                private sealed class SocketImpl : IStreamSocket
                {
                    private readonly MockStreamSocketFactory m_socketFactory;

                    public SocketImpl(MockStreamSocketFactory socketFactory)
                    {
                        if (socketFactory == null)
                        {
                            throw new ArgumentNullException("socketFactory");
                        }

                        m_socketFactory = socketFactory;
                    }

                    public bool Connected
                    {
                        get
                        {
                            var result = m_socketFactory.m_connectedInvokedHandler();
                            return result;
                        }
                    }

                    public IAsyncResult BeginConnect(IPAddress address, ushort port, AsyncCallback requestCallback, object state)
                    {
                        var result = m_socketFactory.m_beginConnectInvokedHandler(address, port, requestCallback, state);
                        return result;
                    }

                    public Stream GetSendStream()
                    {
                        var result = m_socketFactory.m_getSendStreamInvokedHandler();
                        return result;
                    }

                    public Stream GetReceiveStream()
                    {
                        var result = m_socketFactory.m_getReceiveStreamInvokedHandler();
                        return result;
                    }

                    public void Dispose()
                    {
                        m_socketFactory.m_disposeInvokedHandler();
                    }
                }
            }

            private sealed class MockConnectAsyncResult : IAsyncResult
            {
                public MockConnectAsyncResult()
                {
                }

                public object AsyncState
                {
                    get
                    {
                        Assert.Fail();
                        throw new NotImplementedException();
                    }
                }

                public WaitHandle AsyncWaitHandle
                {
                    get
                    {
                        var result = new MockWaitHandle();
                        return result;
                    }
                }

                public bool CompletedSynchronously
                {
                    get
                    {
                        Assert.Fail();
                        throw new NotImplementedException();
                    }
                }

                public bool IsCompleted
                {
                    get
                    {
                        Assert.Fail();
                        throw new NotImplementedException();
                    }
                }

                private sealed class MockWaitHandle : WaitHandle
                {
                    public MockWaitHandle()
                    {
                    }

                    public override bool WaitOne(int millisecondsTimeout)
                    {
                        return true;
                    }
                }
            }
        }
    }
}
