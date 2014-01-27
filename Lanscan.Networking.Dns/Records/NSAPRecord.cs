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
    using System.Text;

    public sealed class NSAPRecord : Record
    {
        private readonly ushort m_length;
        private readonly byte[] m_nsapAddress;

        public NSAPRecord(RR rr, RecordReader recordReader)
            : base(rr)
        {
            if (recordReader == null)
            {
                throw new ArgumentNullException("recordReader");
            }

            m_length = recordReader.ReadUInt16();
            m_nsapAddress = recordReader.ReadBytes(m_length);
        }

        public ushort LENGTH
        {
            get { return m_length; }
        }

        public IEnumerable<byte> NSAPADDRESS
        {
            get { return m_nsapAddress; }
        }

        public override string ToString()
        {
            var output = new StringBuilder();
            output.AppendFormat("{0} ", m_length);
            for (var i = 0; i < m_nsapAddress.Length; i++)
            {
                output.AppendFormat("{0:X00}", m_nsapAddress[i]);
            }

            var result = output.ToString();
            return result;
        }

        public string ToGOSIPV2()
        {
            var result = String.Format(
                CultureInfo.InvariantCulture,
                "{0:X}.{1:X}.{2:X}.{3:X}.{4:X}.{5:X}.{6:X}{7:X}.{8:X}",
                m_nsapAddress[0],							// AFI
                m_nsapAddress[1] << 8 | m_nsapAddress[2],	// IDI
                m_nsapAddress[3],							// DFI
                m_nsapAddress[4] << 16 | m_nsapAddress[5] << 8 | m_nsapAddress[6], // AA
                m_nsapAddress[7] << 8 | m_nsapAddress[8],	// Rsvd
                m_nsapAddress[9] << 8 | m_nsapAddress[10],// RD
                m_nsapAddress[11] << 8 | m_nsapAddress[12],// Area
                m_nsapAddress[13] << 16 | m_nsapAddress[14] << 8 | m_nsapAddress[15], // ID-High
                m_nsapAddress[16] << 16 | m_nsapAddress[17] << 8 | m_nsapAddress[18], // ID-Low
                m_nsapAddress[19]);
            return result;
        }
    }
}
