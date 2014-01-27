//////////////////////////////////////////////////////////////////////
//
// Lanscan
// Copyright (C) 2013, Richard Cook. All rights reserved.
//
//////////////////////////////////////////////////////////////////////

namespace Lanscan.Networking.Dns
{
    using System;

    public sealed class TcpDnsResolver : DnsResolverBase
    {
        private readonly IStreamSocketFactory m_streamSocketFactory;

        public TcpDnsResolver(DnsResolverOptions options, IStreamSocketFactory streamSocketFactory, IPEndpoint dnsServerEndpoint, Random random = null)
            : base(options, dnsServerEndpoint, random)
        {
            if (streamSocketFactory == null)
            {
                throw new ArgumentNullException("streamSocketFactory");
            }

            m_streamSocketFactory = streamSocketFactory;
        }

        protected override Response SendRequest(Request request)
        {
            //System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            //sw.Start();

            byte[] responseMessage = new byte[512];

            for (int intAttempts = 0; intAttempts < Options.RetryCount; intAttempts++)
            {
                using (var streamSocket = m_streamSocketFactory.CreateStreamSocket(Options.Timeout))
                {
                    try
                    {
                        var result = streamSocket.BeginConnect(DnsServerEndpoint.Address, DnsServerEndpoint.Port, null, null);

                        var success = result.AsyncWaitHandle.WaitOne(Options.Timeout);
                        if (!success || !streamSocket.Connected)
                        {
                            continue;
                        }

                        var sendStream = streamSocket.GetSendStream();

                        byte[] data = request.Data;
                        sendStream.WriteByte((byte)((data.Length >> 8) & 0xff));
                        sendStream.WriteByte((byte)(data.Length & 0xff));
                        sendStream.Write(data, 0, data.Length);
                        sendStream.Flush();

                        var receiveStream = streamSocket.GetReceiveStream();

                        Response TransferResponse = new Response();
                        int intSoa = 0;
                        int intMessageSize = 0;

                        //Debug.WriteLine("Sending "+ (request.Length+2) + " bytes in "+ sw.ElapsedMilliseconds+" mS");

                        while (true)
                        {
                            int intLength = receiveStream.ReadByte() << 8 | receiveStream.ReadByte();
                            if (intLength <= 0)
                            {
                                throw new Exception/*SocketException*/(); // next try
                            }

                            intMessageSize += intLength;

                            data = new byte[intLength];
                            receiveStream.Read(data, 0, intLength);
                            Response response = new Response(DnsServerEndpoint, data);

                            //Debug.WriteLine("Received "+ (intLength+2)+" bytes in "+sw.ElapsedMilliseconds +" mS");

                            if (response.header.RCODE != RCode.NoError)
                                return response;

                            if (response.Questions[0].QType != QType.AXFR)
                            {
                                CacheResponse(response);
                                return response;
                            }

                            // Zone transfer!!

                            if (TransferResponse.Questions.Count == 0)
                                TransferResponse.Questions.AddRange(response.Questions);
                            TransferResponse.Answers.AddRange(response.Answers);
                            TransferResponse.Authorities.AddRange(response.Authorities);
                            TransferResponse.Additionals.AddRange(response.Additionals);

                            if (response.Answers[0].Type == RecordType.SOA)
                                intSoa++;

                            if (intSoa == 2)
                            {
                                TransferResponse.header.QDCOUNT = (ushort)TransferResponse.Questions.Count;
                                TransferResponse.header.ANCOUNT = (ushort)TransferResponse.Answers.Count;
                                TransferResponse.header.NSCOUNT = (ushort)TransferResponse.Authorities.Count;
                                TransferResponse.header.ARCOUNT = (ushort)TransferResponse.Additionals.Count;
                                TransferResponse.MessageSize = intMessageSize;
                                return TransferResponse;
                            }
                        }
                    } // try
                    catch (Exception/*SocketException*/)
                    {
                        continue; // next try
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
