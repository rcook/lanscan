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
    using System.Text;

    public class Request
    {
        public Header header;

        private List<Question> questions;

        public Request()
        {
            header = new Header();
            header.OPCODE = OPCode.Query;
            header.QDCOUNT = 0;

            questions = new List<Question>();
        }

        public void AddQuestion(Question question)
        {
            questions.Add(question);
        }

        public byte[] Data
        {
            get
            {
                List<byte> data = new List<byte>();
                header.QDCOUNT = (ushort)questions.Count;
                data.AddRange(header.Data);
                foreach (Question q in questions)
                    data.AddRange(q.Data);
                return data.ToArray();
            }
        }
    }
}
