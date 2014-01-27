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

    public sealed class AFSDBRecord : Record
    {
        private readonly ushort m_subtype;
        private readonly string m_hostName;

        public AFSDBRecord(RR rr, RecordReader recordReader)
            : base(rr)
        {
            if (recordReader == null)
            {
                throw new ArgumentNullException("recordReader");
            }

            m_subtype = recordReader.ReadUInt16();
            m_hostName = recordReader.ReadDomainName();
        }

        public ushort SUBTYPE
        {
            get { return m_subtype; }
        }

        public string HOSTNAME
        {
            get { return m_hostName; }
        }

        public override string ToString()
        {
            var result = String.Format(
                CultureInfo.InvariantCulture,
                "{0} {1}",
                m_subtype,
                m_hostName);
            return result;
        }
    }
}
