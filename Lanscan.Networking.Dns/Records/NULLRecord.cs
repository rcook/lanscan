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
    using System.Globalization;

    public sealed class NULLRecord : Record
    {
        private readonly byte[] m_anything;

        public NULLRecord(RR rr, RecordReader recordReader)
            : base(rr)
        {
            if (recordReader == null)
            {
                throw new ArgumentNullException("recordReader");
            }

            var count = recordReader.ReadUInt16(-1);
            m_anything = recordReader.ReadBytes(count);
        }

        public IEnumerable<byte> ANYTHING
        {
            get { return m_anything; }
        }

        public override string ToString()
        {
            var result = String.Format(
                CultureInfo.InvariantCulture,
                "({0} bytes)",
                m_anything.Length);
            return result;
        }
    }
}
