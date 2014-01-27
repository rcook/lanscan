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

    public sealed class NAPTRRecord : Record
    {
        private readonly ushort m_order;
        private readonly ushort m_preference;
        private readonly string m_flags;
        private readonly string m_services;
        private readonly string m_regexp;
        private readonly string m_replacement;

        public NAPTRRecord(RR rr, RecordReader recordReader)
            : base(rr)
        {
            if (recordReader == null)
            {
                throw new ArgumentNullException("recordReader");
            }

            m_order = recordReader.ReadUInt16();
            m_preference = recordReader.ReadUInt16();
            m_flags = recordReader.ReadString();
            m_services = recordReader.ReadString();
            m_regexp = recordReader.ReadString();
            m_replacement = recordReader.ReadDomainName();
        }

        public ushort ORDER
        {
            get { return m_order; }
        }

        public ushort PREFERENCE
        {
            get { return m_preference; }
        }

        public string FLAGS
        {
            get { return m_flags; }
        }

        public string SERVICES
        {
            get { return m_services; }
        }

        public string REGEXP
        {
            get { return m_regexp; }
        }

        public string REPLACEMENT
        {
            get { return m_replacement; }
        }

        public override string ToString()
        {
            var result = String.Format(
                CultureInfo.InvariantCulture,
                "{0} {1} \"{2}\" \"{3}\" \"{4}\" {5}",
                m_order,
                m_preference,
                m_flags,
                m_services,
                m_regexp,
                m_replacement);
            return result;
        }
    }
}
