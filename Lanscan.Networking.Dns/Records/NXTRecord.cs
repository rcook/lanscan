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
    using System.Text;

    public sealed class NXTRecord : Record
    {
        private readonly string m_nextDomainName;
        private readonly byte[] m_bitmap;

        public NXTRecord(RR rr, RecordReader recordReader)
            : base(rr)
        {
            if (recordReader == null)
            {
                throw new ArgumentNullException("recordReader");
            }

            var length = recordReader.ReadUInt16(-2);
            m_nextDomainName = recordReader.ReadDomainName();
            length -= (ushort)recordReader.Position;
            m_bitmap = recordReader.ReadBytes(length);
        }

        public string NEXTDOMAINNAME
        {
            get { return m_nextDomainName; }
        }

        public IEnumerable<byte> BITMAP
        {
            get { return m_bitmap; }
        }

        public override string ToString()
        {
            var output = new StringBuilder();
            for (var i = 1; i < (m_bitmap.Length * 8); ++i)
            {
                if (IsSet(i))
                {
                    output.Append(" " + (RecordType)i);
                }
            }

            var result = String.Format(
                CultureInfo.InvariantCulture,
                "{0}{1}",
                m_nextDomainName,
                output.ToString());
            return result;
        }

        private bool IsSet(int index)
        {
            var intByte = (int)(index / 8);
            var intOffset = (index % 8);
            var b = m_bitmap[intByte];
            var intTest = 1 << intOffset;

            if ((b & intTest) == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
