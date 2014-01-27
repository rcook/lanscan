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

    public sealed class GPOSRecord : Record
    {
        private readonly string m_longitude;
        private readonly string m_latitude;
        private readonly string m_altitude;

        public GPOSRecord(RR rr, RecordReader recordReader)
            : base(rr)
        {
            if (recordReader == null)
            {
                throw new ArgumentNullException("recordReader");
            }

            m_longitude = recordReader.ReadString();
            m_latitude = recordReader.ReadString();
            m_altitude = recordReader.ReadString();
        }

        public string LONGITUDE
        {
            get { return m_longitude; }
        }

        public string LATITUDE
        {
            get { return m_latitude; }
        }

        public string ALTITUDE
        {
            get { return m_altitude; }
        }

        public override string ToString()
        {
            var result = String.Format(
                CultureInfo.InvariantCulture,
                "{0} {1} {2}",
                m_longitude,
                m_latitude,
                m_altitude);
            return result;
        }
    }
}
