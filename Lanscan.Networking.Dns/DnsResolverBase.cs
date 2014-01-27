//////////////////////////////////////////////////////////////////////
//
// Lanscan
// Copyright (C) 2013, Richard Cook. All rights reserved.
//
//////////////////////////////////////////////////////////////////////

//////////////////////////////////////////////////////////////////////
//
// Based on Alphons van der Heijden's code presented at
// http://www.codeproject.com/Articles/23673/DNS-NET-Resolver-C
//
// Licensed under Code Project Open License (CPOL) 1.02
// http://www.codeproject.com/info/cpol10.aspx
//
//////////////////////////////////////////////////////////////////////

namespace Lanscan.Networking.Dns
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using Lanscan.Networking.Dns.Records;

    public abstract class DnsResolverBase
    {
        private readonly DnsResolverOptions m_options;

        // TODO: Rationalize these two caches!
        private readonly ConcurrentDictionary<uint, string> m_hostNames = new ConcurrentDictionary<uint, string>();
        private readonly Dictionary<string, Response> m_responses = new Dictionary<string, Response>();

        private readonly IPEndpoint m_dnsServerEndpoint;
        private ushort m_sequenceNumber;

        protected DnsResolverBase(DnsResolverOptions options, IPEndpoint dnsServerEndpoint, Random random = null)
        {
            if (options == null)
            {
                throw new ArgumentNullException("options");
            }
            if (dnsServerEndpoint == null)
            {
                throw new ArgumentNullException("dnsServerEndpoint");
            }

            m_options = options;
            m_dnsServerEndpoint = dnsServerEndpoint;
            m_sequenceNumber = (ushort)(random ?? new Random()).Next();
        }

        protected DnsResolverOptions Options
        {
            get { return m_options; }
        }

        protected IPEndpoint DnsServerEndpoint
        {
            get { return m_dnsServerEndpoint; }
        }

        public string GetHostName(IPAddress address)
        {
            if (address == null)
            {
                throw new ArgumentNullException("address");
            }

            var result = m_hostNames.GetOrAdd(address.Value, x =>
            {
                var arpaUrl = DnsHelper.GetArpaUrl(address);
                var response = Query(arpaUrl, QType.PTR, QClass.IN);
                if (response.Error.Length > 0 || response.header.ANCOUNT != 1)
                {
                    return null;
                }

                var ptrAnswer = response.Answers.FirstOrDefault();
                if (ptrAnswer == null)
                {
                    return null;
                }

                var ptrRecord = (PTRRecord)ptrAnswer.RECORD;
                var hostName = ptrRecord.PTRDNAME.Trim(new[] { '.' });
                return hostName;
            });
            return result;
        }

        public Response Query(string hostName, QType type, QClass @class)
        {
            if (String.IsNullOrWhiteSpace(hostName))
            {
                throw new ArgumentException("Invalid host name", "hostName");
            }

            var question = new Question(hostName, type, @class);
            var response = FindResponse(question);
            if (response != null)
            {
                return response;
            }

            var request = new Request();
            request.AddQuestion(question);
            response = GetResponse(request);
            return response;
        }

        public void ClearResponses()
        {
            m_responses.Clear();
        }

        protected void IncrementSequenceNumber()
        {
            ++m_sequenceNumber;
        }

        protected void CacheResponse(Response response)
        {
            if (!Options.UseCache)
            {
                return;
            }

            if (response.Questions.Count == 0 || response.header.RCODE != RCode.NoError)
            {
                return;
            }

            var question = response.Questions[0];
            var key = GetQuestionKey(question);
            lock (m_responses)
            {
                m_responses[key] = response;
            }
        }

        protected abstract Response SendRequest(Request request);

        private static string GetQuestionKey(Question question)
        {
            var result = String.Format(
                CultureInfo.InvariantCulture,
                "{0}-{1}-{2}",
                question.QClass,
                question.QType,
                question.QName);
            return result;
        }

        private Response FindResponse(Question question)
        {
            if (!Options.UseCache)
            {
                return null;
            }

            Response response;
            var key = GetQuestionKey(question);
            lock (m_responses)
            {
                var result = m_responses.TryGetValue(key, out response);
                if (!result)
                {
                    return null;
                }
            }

            var timeAlive = (int)((DateTime.Now.Ticks - response.TimeStamp.Ticks) / TimeSpan.TicksPerSecond);
            foreach (var rr in response.ResourceRecords)
            {
                rr.TimeLived = timeAlive;
                if (rr.TTL == 0)
                {
                    return null;
                }
            }
            return response;
        }

        private Response GetResponse(Request request)
        {
            request.header.ID = m_sequenceNumber;
            request.header.RD = Options.Recursion;
            var result = SendRequest(request);
            return result;
        }

        private IPHostEntry MakeEntry(string hostName)
        {
            var response = Query(hostName, QType.A, QClass.IN);
            var addresses = new List<IPAddress>();
            var aliases = new List<string>();
            foreach (var answerRR in response.Answers)
            {
                if (answerRR.Type == RecordType.A)
                {
                    // answerRR.RECORD.ToString() == (answerRR.RECORD as RecordA).Address
                    addresses.Add(IPAddress.Parse((answerRR.RECORD.ToString())));
                    hostName = answerRR.NAME;
                }
                else
                {
                    if (answerRR.Type == RecordType.CNAME)
                    {
                        aliases.Add(answerRR.NAME);
                    }
                }
            }

            var result = new IPHostEntry(hostName, addresses.ToArray(), aliases.ToArray());
            return result;
        }

        public static string GetArpaFromEnum(string s)
        {
            var output = new StringBuilder();
            var number = Regex.Replace(s, "[^0-9]", "");
            output.Append("e164.arpa.");
            foreach (var c in number)
            {
                output.Insert(0, String.Format(CultureInfo.InvariantCulture, "{0}.", c));
            }
            var result = output.ToString();
            return result;
        }
    }
}
