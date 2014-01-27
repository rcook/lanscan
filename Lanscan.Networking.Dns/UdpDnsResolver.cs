//////////////////////////////////////////////////////////////////////
//
// Lanscan
// Copyright (C) 2013, Richard Cook. All rights reserved.
//
//////////////////////////////////////////////////////////////////////

namespace Lanscan.Networking.Dns
{
    using System;

    public sealed class UdpDnsResolver : DnsResolverBase
    {
        private readonly IDatagramSocketFactory m_datagramSocketFactory;

        public UdpDnsResolver(DnsResolverOptions options, IDatagramSocketFactory datagramSocketFactory, IPEndpoint dnsServerEndpoint, Random random = null)
            : base(options, dnsServerEndpoint, random)
        {
            if (datagramSocketFactory == null)
            {
                throw new ArgumentNullException("datagramSocketFactory");
            }

            m_datagramSocketFactory = datagramSocketFactory;
        }

        protected override Response SendRequest(Request request)
        {
            // RFC1035 max. size of a UDP datagram is 512 bytes
            byte[] responseMessage = new byte[512];

            for (int intAttempts = 0; intAttempts < Options.RetryCount; intAttempts++)
            {
                using (var datagramSocket = m_datagramSocketFactory.CreateDatagramSocket(Options.Timeout))
                {
                    try
                    {
                        var address = IPAddress.Parse(DnsServerEndpoint.Address.ToString());
                        var endpoint = new IPEndpoint(address, (ushort)DnsServerEndpoint.Port);
                        datagramSocket.SendTo(request.Data, endpoint);
                        int intReceived = datagramSocket.Receive(responseMessage);
                        byte[] data = new byte[intReceived];
                        Array.Copy(responseMessage, data, intReceived);
                        Response response = new Response(DnsServerEndpoint, data);
                        CacheResponse(response);
                        return response;
                    }
                    catch (Exception/*SocketException*/)
                    {
                        continue;
                    }
                    finally
                    {
                        IncrementSequenceNumber();
                    }
                }
            }
            Response responseTimeout = new Response();
            responseTimeout.Error = "Timeout Error";
            return responseTimeout;
        }
    }
}
