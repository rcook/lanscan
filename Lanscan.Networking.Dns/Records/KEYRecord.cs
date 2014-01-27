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

    public sealed class KEYRecord : Record
    {
        private readonly ushort m_flags;
        private readonly byte m_protocol;
        private readonly byte m_algorithm;
        private readonly string m_publicKey;

        public KEYRecord(RR rr, RecordReader recordReader)
            : base(rr)
        {
            if (recordReader == null)
            {
                throw new ArgumentNullException("recordReader");
            }

            m_flags = recordReader.ReadUInt16();
            m_protocol = recordReader.ReadByte();
            m_algorithm = recordReader.ReadByte();
            m_publicKey = recordReader.ReadString();
        }

        public ushort FLAGS
        {
            get { return m_flags; }
        }

        public byte PROTOCOL
        {
            get { return m_protocol; }
        }

        public byte ALGORITHM
        {
            get { return m_algorithm; }
        }

        public string PUBLICKEY
        {
            get { return m_publicKey; }
        }

        public override string ToString()
        {
            var result = String.Format(
                CultureInfo.InvariantCulture,
                "{0} {1} {2} \"{3}\"",
                m_flags,
                m_protocol,
                m_algorithm,
                m_publicKey);
            return result;
        }
    }
}
