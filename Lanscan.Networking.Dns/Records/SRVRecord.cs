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

    public sealed class SRVRecord : Record
    {
        private readonly ushort m_priority;
        private readonly ushort m_weight;
        private readonly ushort m_port;
        private readonly string m_target;

        public SRVRecord(RR rr, RecordReader recordReader)
            : base(rr)
        {
            if (recordReader == null)
            {
                throw new ArgumentNullException("recordReader");
            }

            m_priority = recordReader.ReadUInt16();
            m_weight = recordReader.ReadUInt16();
            m_port = recordReader.ReadUInt16();
            m_target = recordReader.ReadDomainName();
        }

        public ushort PRIORITY
        {
            get { return m_priority; }
        }

        public ushort WEIGHT
        {
            get { return m_weight; }
        }

        public ushort PORT
        {
            get { return m_port; }
        }

        public string TARGET
        {
            get { return m_target; }
        }

        public override string ToString()
        {
            var result = String.Format(
                CultureInfo.InvariantCulture,
                "{0} {1} {2} {3}",
                m_priority,
                m_weight,
                m_port,
                m_target);
            return result;
        }
    }
}
