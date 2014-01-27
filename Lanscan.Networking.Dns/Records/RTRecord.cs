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

    public sealed class RTRecord : Record
    {
        private readonly ushort m_preference;
        private readonly string m_intermediateHost;

        public RTRecord(RR rr, RecordReader recordReader)
            : base(rr)
        {
            if (recordReader == null)
            {
                throw new ArgumentNullException("recordReader");
            }

            m_preference = recordReader.ReadUInt16();
            m_intermediateHost = recordReader.ReadDomainName();
        }

        public ushort PREFERENCE
        {
            get { return m_preference; }
        }

        public string INTERMEDIATEHOST
        {
            get { return m_intermediateHost; }
        }

        public override string ToString()
        {
            var result = String.Format(
                CultureInfo.InvariantCulture,
                "{0} {1}",
                m_preference,
                m_intermediateHost);
            return result;
        }
    }
}
