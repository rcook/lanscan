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

    public sealed class MINFORecord : Record
    {
        private readonly string m_rMailBx;
        private readonly string m_eMailBx;

        public MINFORecord(RR rr, RecordReader recordReader)
            : base(rr)
        {
            if (recordReader == null)
            {
                throw new ArgumentNullException("recordReader");
            }

            m_rMailBx = recordReader.ReadDomainName();
            m_eMailBx = recordReader.ReadDomainName();
        }

        public string RMAILBX
        {
            get { return m_rMailBx; }
        }

        public string EMAILBX
        {
            get { return m_eMailBx; }
        }

        public override string ToString()
        {
            var result = String.Format(
                CultureInfo.InvariantCulture,
                "{0} {1}",
                m_rMailBx,
                m_eMailBx);
            return result;
        }
    }
}
