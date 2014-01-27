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
    using System.Collections.Generic;
    using System.Globalization;

    public sealed class WKSRecord : Record
    {
        private readonly string m_address;
        private readonly int m_protocol;
        private readonly byte[] m_bitmap;

        public WKSRecord(RR rr, RecordReader recordReader)
            : base(rr)
        {
            if (recordReader == null)
            {
                throw new ArgumentNullException("recordReader");
            }

            var length = recordReader.ReadUInt16(-2);
            m_address = String.Format(
                CultureInfo.InvariantCulture,
                "{0}.{1}.{2}.{3}",
                recordReader.ReadByte(),
                recordReader.ReadByte(),
                recordReader.ReadByte(),
                recordReader.ReadByte());
            m_protocol = (int)recordReader.ReadByte();
            length -= 5;
            m_bitmap = recordReader.ReadBytes(length);
        }

        public string ADDRESS
        {
            get { return m_address; }
        }
        
        public int PROTOCOL
        {
            get { return m_protocol; }
        }

        public IEnumerable<byte> BITMAP
        {
            get { return m_bitmap; }
        }

        public override string ToString()
        {
            var result = String.Format(
                CultureInfo.InvariantCulture,
                "{0} {1}",
                m_address,
                m_protocol);
            return result;
        }
    }
}
