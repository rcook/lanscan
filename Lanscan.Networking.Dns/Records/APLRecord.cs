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

namespace Lanscan.Networking.Dns.Records
{
    using System;
    using System.Collections.Generic;

    public sealed class APLRecord : Record
    {
        private readonly byte[] m_rdata;

        public APLRecord(RR rr, RecordReader recordReader)
            : base(rr)
        {
            if (recordReader == null)
            {
                throw new ArgumentNullException("recordReader");
            }

            var count = recordReader.ReadUInt16(-2);
            m_rdata = recordReader.ReadBytes(count);
        }

        public IEnumerable<byte> RDATA
        {
            get { return m_rdata; }
        }

        public override string ToString()
        {
            return Constants.NotUsed;
        }
    }
}
