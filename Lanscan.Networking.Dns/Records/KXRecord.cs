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

    public sealed class KXRecord : Record, IComparable
    {
        private readonly ushort m_preference;
        private readonly string m_exchanger;

        public KXRecord(RR rr, RecordReader recordReader)
            : base(rr)
        {
            if (recordReader == null)
            {
                throw new ArgumentNullException("recordReader");
            }

            m_preference = recordReader.ReadUInt16();
            m_exchanger = recordReader.ReadDomainName();
        }

        public ushort PREFERENCE
        {
            get { return m_preference; }
        }

        public string EXCHANGER
        {
            get { return m_exchanger; }
        }

        public override string ToString()
        {
            var result = String.Format(
                CultureInfo.InvariantCulture,
                "{0} {1}",
                m_preference,
                m_exchanger);
            return result;
        }

        public int CompareTo(object obj)
        {
            if (obj == null)
            {
                return 1;
            }

            var other = obj as KXRecord;
            if (other == null)
            {
                throw new ArgumentException("Not a RecordKX", "obj");
            }

            var temp = m_preference.CompareTo(other.m_preference);
            if (temp != 0)
            {
                return temp;
            }

            var result = String.Compare(m_exchanger, other.m_exchanger, StringComparison.OrdinalIgnoreCase);
            return result;
        }
    }
}
