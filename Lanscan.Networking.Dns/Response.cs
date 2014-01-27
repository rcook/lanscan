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
    using System.Collections.Generic;
    using System.Linq;

    public sealed class Response
    {
        /// <summary>
        /// List of Question records
        /// </summary>
        public List<Question> Questions;
        /// <summary>
        /// List of AnswerRR records
        /// </summary>
        public List<AnswerRR> Answers;
        /// <summary>
        /// List of AuthorityRR records
        /// </summary>
        public List<AuthorityRR> Authorities;
        /// <summary>
        /// List of AdditionalRR records
        /// </summary>
        public List<AdditionalRR> Additionals;

        public Header header;

        /// <summary>
        /// Error message, empty when no error
        /// </summary>
        public string Error;

        /// <summary>
        /// The Size of the message
        /// </summary>
        public int MessageSize;

        /// <summary>
        /// TimeStamp when cached
        /// </summary>
        public DateTime TimeStamp;

        /// <summary>
        /// Server which delivered this response
        /// </summary>
        public IPEndpoint Server;

        public Response()
        {
            Questions = new List<Question>();
            Answers = new List<AnswerRR>();
            Authorities = new List<AuthorityRR>();
            Additionals = new List<AdditionalRR>();

            Server = new IPEndpoint(new IPAddress(0), 0);
            Error = String.Empty;
            MessageSize = 0;
            TimeStamp = DateTime.Now;
            header = new Header();
        }

        public Response(IPEndpoint endpoint, byte[] data)
        {
            Error = String.Empty;
            Server = endpoint;
            TimeStamp = DateTime.Now;
            MessageSize = data.Length;
            RecordReader rr = new RecordReader(data);

            Questions = new List<Question>();
            Answers = new List<AnswerRR>();
            Authorities = new List<AuthorityRR>();
            Additionals = new List<AdditionalRR>();

            header = new Header(rr);

            //if (header.RCODE != RCode.NoError)
            //	Error = header.RCODE.ToString();

            for (int intI = 0; intI < header.QDCOUNT; intI++)
            {
                Questions.Add(new Question(rr));
            }

            for (int intI = 0; intI < header.ANCOUNT; intI++)
            {
                Answers.Add(new AnswerRR(rr));
            }

            for (int intI = 0; intI < header.NSCOUNT; intI++)
            {
                Authorities.Add(new AuthorityRR(rr));
            }
            for (int intI = 0; intI < header.ARCOUNT; intI++)
            {
                Additionals.Add(new AdditionalRR(rr));
            }
        }

        public IEnumerable<RR> ResourceRecords
        {
            get { return Answers.Cast<RR>().Concat(Authorities.Cast<RR>()).Concat(Additionals.Cast<RR>()); }
        }
    }
}
