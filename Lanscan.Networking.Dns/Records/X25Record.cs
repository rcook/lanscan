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

    public sealed class X25Record : Record
    {
        private readonly string m_psdnAddress;

        public X25Record(RR rr, RecordReader recordReader)
            : base(rr)
        {
            if (recordReader == null)
            {
                throw new ArgumentNullException("recordReader");
            }

            m_psdnAddress = recordReader.ReadString();
        }

        public string PSDNADDRESS
        {
            get { return m_psdnAddress; }
        }

        public override string ToString()
        {
            var result = String.Format(CultureInfo.InvariantCulture, "{0}", PSDNADDRESS);
            return result;
        }
    }
}
