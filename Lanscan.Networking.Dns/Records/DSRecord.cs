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
    using System.Linq;

    public sealed class DSRecord : Record
    {
        private readonly ushort m_keyTag;
        private readonly byte m_algorithm;
        private readonly byte m_digestType;
        private readonly byte[] m_digest;

        public DSRecord(RR rr, RecordReader recordReader)
            : base(rr)
        {
            if (recordReader == null)
            {
                throw new ArgumentNullException("recordReader");
            }

            var count = recordReader.ReadUInt16(-2);
            m_keyTag = recordReader.ReadUInt16();
            m_algorithm = recordReader.ReadByte();
            m_digestType = recordReader.ReadByte();
            count -= 4;
            m_digest = recordReader.ReadBytes(count);
        }

        public ushort KEYTAG
        {
            get { return m_keyTag; }
        }

        public byte ALGORITHM
        {
            get { return m_algorithm; }
        }

        public byte DIGESTTYPE
        {
            get { return m_digestType; }
        }

        public IEnumerable<byte> DIGEST
        {
            get { return m_digest; }
        }

        public override string ToString()
        {
            var result = String.Format(
                CultureInfo.InvariantCulture,
                "{0} {1} {2} {3}",
                m_keyTag,
                m_algorithm,
                m_digestType,
                String.Concat(m_digest.Select(x => x.ToString("X2"))));
            return result;
        }
    }
}
