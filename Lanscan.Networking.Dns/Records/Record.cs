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

    public abstract class Record
    {
        private readonly RR m_rr;

        protected Record(RR resourceRecord)
        {
            if (resourceRecord == null)
            {
                throw new ArgumentNullException("resourceRecord");
            }

            m_rr = resourceRecord;
        }

        public RR RR
        {
            get { return m_rr; }
        }
    }
}
