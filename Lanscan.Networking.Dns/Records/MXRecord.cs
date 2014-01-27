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
    using System.Globalization;

    public sealed class MXRecord : Record, IComparable
    {
        private readonly ushort m_preference;
        private readonly string m_exchange;

        public MXRecord(RR rr, RecordReader recordReader)
            : base(rr)
        {
            if (recordReader == null)
            {
                throw new ArgumentNullException("recordReader");
            }

            m_preference = recordReader.ReadUInt16();
            m_exchange = recordReader.ReadDomainName();
        }

        public ushort PREFERENCE
        {
            get { return m_preference; }
        }

        public string EXCHANGE
        {
            get { return m_exchange; }
        }

        public override string ToString()
        {
            var result = String.Format(
                CultureInfo.InvariantCulture,
                "{0} {1}",
                m_preference,
                m_exchange);
            return result;
        }

        public int CompareTo(object obj)
        {
            if (obj == null)
            {
                return 1;
            }

            var other = obj as MXRecord;
            if (other == null)
            {
                throw new ArgumentException("Not a RecordMX", "obj");
            }

            var temp = m_preference.CompareTo(other.m_preference);
            if (temp != 0)
            {
                return temp;
            }

            var result = String.Compare(m_exchange, other.m_exchange, StringComparison.OrdinalIgnoreCase);
            return result;
        }
    }
}
