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

    public sealed class SOARecord : Record
    {
        private readonly string m_mName;
        private readonly string m_rName;
        private readonly uint m_serial;
        private readonly uint m_refresh;
        private readonly uint m_retry;
        private readonly uint m_expire;
        private readonly uint m_minimum;

        public SOARecord(RR rr, RecordReader recordReader)
            : base(rr)
        {
            if (recordReader == null)
            {
                throw new ArgumentNullException("recordReader");
            }

            m_mName = recordReader.ReadDomainName();
            m_rName = recordReader.ReadDomainName();
            m_serial = recordReader.ReadUInt32();
            m_refresh = recordReader.ReadUInt32();
            m_retry = recordReader.ReadUInt32();
            m_expire = recordReader.ReadUInt32();
            m_minimum = recordReader.ReadUInt32();
        }

        public string MNAME
        {
            get { return m_mName; }
        }

        public string RNAME
        {
            get { return m_rName; }
        }

        public uint SERIAL
        {
            get { return m_serial; }
        }

        public uint REFRESH
        {
            get { return m_refresh; }
        }

        public uint RETRY
        {
            get { return m_retry; }
        }

        public uint EXPIRE
        {
            get { return m_expire; }
        }

        public uint MINIMUM
        {
            get { return m_minimum; }
        }

        public override string ToString()
        {
            var result = String.Format(
                CultureInfo.InvariantCulture,
                "{0} {1} {2} {3} {4} {5} {6}",
                m_mName,
                m_rName,
                m_serial,
                m_refresh,
                m_retry,
                m_expire,
                m_minimum);
            return result;
        }
    }
}
