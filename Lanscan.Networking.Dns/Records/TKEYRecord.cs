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

    public sealed class TKEYRecord : Record
    {
        private readonly string m_algorithm;
        private readonly uint m_inception;
        private readonly uint m_expiration;
        private readonly ushort m_mode;
        private readonly ushort m_error;
        private readonly ushort m_keySize;
        private readonly byte[] m_keyData;
        private readonly ushort m_otherSize;
        private readonly byte[] m_otherData;

        public TKEYRecord(RR rr, RecordReader recordReader)
            : base(rr)
        {
            if (recordReader == null)
            {
                throw new ArgumentNullException("recordReader");
            }

            m_algorithm = recordReader.ReadDomainName();
            m_inception = recordReader.ReadUInt32();
            m_expiration = recordReader.ReadUInt32();
            m_mode = recordReader.ReadUInt16();
            m_error = recordReader.ReadUInt16();
            m_keySize = recordReader.ReadUInt16();
            m_keyData = recordReader.ReadBytes(m_keySize);
            m_otherSize = recordReader.ReadUInt16();
            m_otherData = recordReader.ReadBytes(m_otherSize);
        }

        public string ALGORITHM
        {
            get { return m_algorithm; }
        }

        public uint INCEPTION
        {
            get { return m_inception; }
        }

        public uint EXPIRATION
        {
            get { return m_expiration; }
        }

        public ushort MODE
        {
            get { return m_mode; }
        }

        public ushort ERROR
        {
            get { return m_error; }
        }

        public ushort KEYSIZE
        {
            get { return m_keySize; }
        }

        public IEnumerable<byte> KEYDATA
        {
            get { return m_keyData; }
        }

        public ushort OTHERSIZE
        {
            get { return m_otherSize; }
        }

        public IEnumerable<byte> OTHERDATA
        {
            get { return m_otherData; }
        }

        public override string ToString()
        {
            var result = String.Format(
                CultureInfo.InvariantCulture,
                "{0} {1} {2} {3} {4}",
                m_algorithm,
                m_inception,
                m_expiration,
                m_mode,
                m_error);
            return result;
        }
    }
}
