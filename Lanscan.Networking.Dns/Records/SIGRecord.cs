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

    public sealed class SIGRecord : Record
    {
        private readonly ushort m_typeCovered;
        private readonly byte m_algorithm;
        private readonly byte m_labels;
        private readonly uint m_originalTtl;
        private readonly uint m_signatureExpiration;
        private readonly uint m_signatureInception;
        private readonly ushort m_keyTag;
        private readonly string m_signersName;
        private readonly string m_signature;

        public SIGRecord(RR rr, RecordReader recordReader)
            : base(rr)
        {
            if (recordReader == null)
            {
                throw new ArgumentNullException("recordReader");
            }

            m_typeCovered = recordReader.ReadUInt16();
            m_algorithm = recordReader.ReadByte();
            m_labels = recordReader.ReadByte();
            m_originalTtl = recordReader.ReadUInt32();
            m_signatureExpiration = recordReader.ReadUInt32();
            m_signatureInception = recordReader.ReadUInt32();
            m_keyTag = recordReader.ReadUInt16();
            m_signersName = recordReader.ReadDomainName();
            m_signature = recordReader.ReadString();
        }

        public ushort TYPECOVERED
        {
            get { return m_typeCovered; }
        }

        public byte ALGORITHM
        {
            get { return m_algorithm; }
        }

        public byte LABELS
        {
            get { return m_labels; }
        }

        public uint ORIGINALTTL
        {
            get { return m_originalTtl; }
        }

        public uint SIGNATUREEXPIRATION
        {
            get { return m_signatureExpiration; }
        }

        public uint SIGNATUREINCEPTION
        {
            get { return m_signatureInception; }
        }

        public ushort KEYTAG
        {
            get { return m_keyTag; }
        }

        public string SIGNERSNAME
        {
            get { return m_signersName; }
        }

        public string SIGNATURE
        {
            get { return m_signature; }
        }

        public override string ToString()
        {
            var result = String.Format(
                CultureInfo.InvariantCulture,
                "{0} {1} {2} {3} {4} {5} {6} {7} \"{8}\"",
                m_typeCovered,
                m_algorithm,
                m_labels,
                m_originalTtl,
                m_signatureExpiration,
                m_signatureInception,
                m_keyTag,
                m_signersName,
                m_signature);
            return result;
        }
    }
}
