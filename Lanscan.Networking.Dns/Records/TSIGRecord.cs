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

    public sealed class RecordTSIG : Record
    {
        private readonly string m_algorithmName;
        private readonly long m_timeSigned;
        private readonly ushort m_fudge;
        private readonly ushort m_macSize;
        private readonly byte[] m_mac;
        private readonly ushort m_originalId;
        private readonly ushort m_error;
        private readonly ushort m_otherLen;
        private readonly byte[] m_otherData;

        public RecordTSIG(RR rr, RecordReader recordReader)
            : base(rr)
        {
            if (recordReader == null)
            {
                throw new ArgumentNullException("recordReader");
            }

            m_algorithmName = recordReader.ReadDomainName();
            m_timeSigned = recordReader.ReadUInt32() << 32 | recordReader.ReadUInt32();
            m_fudge = recordReader.ReadUInt16();
            m_macSize = recordReader.ReadUInt16();
            m_mac = recordReader.ReadBytes(m_macSize);
            m_originalId = recordReader.ReadUInt16();
            m_error = recordReader.ReadUInt16();
            m_otherLen = recordReader.ReadUInt16();
            m_otherData = recordReader.ReadBytes(m_otherLen);
        }

        public string ALGORITHMNAME
        {
            get { return m_algorithmName; }
        }

        public long TIMESIGNED
        {
            get { return m_timeSigned; }
        }

        public ushort FUDGE
        {
            get { return m_fudge; }
        }

        public ushort MACSIZE
        {
            get { return m_macSize; }
        }

        public IEnumerable<byte> MAC
        {
            get { return m_mac; }
        }

        public ushort ORIGINALID
        {
            get { return m_originalId; }
        }

        public ushort ERROR
        {
            get { return m_error; }
        }

        public ushort OTHERLEN
        {
            get { return m_otherLen; }
        }

        public IEnumerable<byte> OTHERDATA
        {
            get { return m_otherData; }
        }

        public override string ToString()
        {
            var unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            var dateTime = unixEpoch.AddSeconds(TIMESIGNED);
            var result = String.Format(
                CultureInfo.InvariantCulture,
                "{0} {1} {2} {3} {4}",
                m_algorithmName,
                dateTime.ToString("G"),
                m_fudge,
                m_originalId,
                m_error);
            return result;
        }
    }
}
