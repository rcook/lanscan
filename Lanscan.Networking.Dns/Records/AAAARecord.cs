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

    public sealed class AAAARecord : Record
    {
        private readonly IPAddress m_address;

        public AAAARecord(RR rr, RecordReader recordReader)
            : base(rr)
        {
            if (recordReader == null)
            {
                throw new ArgumentNullException("recordReader");
            }

            // TODO: Use proper byte array constructor.
            // TODO: Update IPAddress to support IPv6!
            m_address = IPAddress.Parse(
                String.Format(CultureInfo.InvariantCulture, "{0:x}:{1:x}:{2:x}:{3:x}:{4:x}:{5:x}:{6:x}:{7:x}",
                recordReader.ReadUInt16(),
                recordReader.ReadUInt16(),
                recordReader.ReadUInt16(),
                recordReader.ReadUInt16(),
                recordReader.ReadUInt16(),
                recordReader.ReadUInt16(),
                recordReader.ReadUInt16(),
                recordReader.ReadUInt16()));
        }

        public IPAddress Address
        {
            get { return m_address; }
        }

        public override string ToString()
        {
            var result = m_address.ToString();
            return result;
        }
    }
}
