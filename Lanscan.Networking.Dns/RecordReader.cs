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

namespace Lanscan.Networking.Dns
{
    using System;
    using System.Text;
    using Lanscan.Networking.Dns.Records;

    public sealed class RecordReader
    {
        private byte[] m_Data;
        private int m_Position;

        public RecordReader(byte[] data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }

            m_Data = data;
            m_Position = 0;
        }

        public int Position
        {
            get
            {
                return m_Position;
            }
            set
            {
                m_Position = value;
            }
        }

        public int Length
        {
            get
            {
                if (m_Data == null)
                {
                    return 0;
                }
                else
                {
                    return m_Data.Length;
                }
            }
        }

        public RecordReader(byte[] data, int Position)
        {
            m_Data = data;
            m_Position = Position;
        }


        public byte ReadByte()
        {
            if (m_Position >= m_Data.Length)
                return 0;
            else
                return m_Data[m_Position++];
        }

        public char ReadChar()
        {
            return (char)ReadByte();
        }

        public UInt16 ReadUInt16()
        {
            return (UInt16)(ReadByte() << 8 | ReadByte());
        }

        public UInt16 ReadUInt16(int offset)
        {
            m_Position += offset;
            return ReadUInt16();
        }

        public UInt32 ReadUInt32()
        {
            return (UInt32)(ReadUInt16() << 16 | ReadUInt16());
        }

        public string ReadDomainName()
        {
            StringBuilder name = new StringBuilder();
            int length = 0;

            // get  the length of the first label
            while ((length = ReadByte()) != 0)
            {
                // top 2 bits set denotes domain name compression and to reference elsewhere
                if ((length & 0xc0) == 0xc0)
                {
                    // work out the existing domain name, copy this pointer
                    RecordReader newRecordReader = new RecordReader(m_Data, (length & 0x3f) << 8 | ReadByte());

                    name.Append(newRecordReader.ReadDomainName());
                    return name.ToString();
                }

                // if not using compression, copy a char at a time to the domain name
                while (length > 0)
                {
                    name.Append(ReadChar());
                    length--;
                }
                name.Append('.');
            }
            if (name.Length == 0)
                return ".";
            else
                return name.ToString();
        }

        public string ReadString()
        {
            short length = this.ReadByte();
            StringBuilder str = new StringBuilder();
            for (int intI = 0; intI < length; intI++)
                str.Append(ReadChar());
            return str.ToString();
        }

        // changed 28 augustus 2008
        public byte[] ReadBytes(int intLength)
        {
            byte[] list = new byte[intLength];
            for (int intI = 0; intI < intLength; intI++)
                list[intI] = ReadByte();
            return list;
        }

        public Record ReadRecord(RR resourceRecord, RecordType type)
        {
            if (resourceRecord == null)
            {
                throw new ArgumentNullException("resourceRecord");
            }

            switch (type)
            {
                case RecordType.A:
                    return new ARecord(resourceRecord, this);
                case RecordType.NS:
                    return new NSRecord(resourceRecord, this);
                case RecordType.MD:
                    return new MDRecord(resourceRecord, this);
                case RecordType.MF:
                    return new MFRecord(resourceRecord, this);
                case RecordType.CNAME:
                    return new CNAMERecord(resourceRecord, this);
                case RecordType.SOA:
                    return new SOARecord(resourceRecord, this);
                case RecordType.MB:
                    return new MBRecord(resourceRecord, this);
                case RecordType.MG:
                    return new MGRecord(resourceRecord, this);
                case RecordType.MR:
                    return new MRRecord(resourceRecord, this);
                case RecordType.NULL:
                    return new NULLRecord(resourceRecord, this);
                case RecordType.WKS:
                    return new WKSRecord(resourceRecord, this);
                case RecordType.PTR:
                    return new PTRRecord(resourceRecord, this);
                case RecordType.HINFO:
                    return new HINFORecord(resourceRecord, this);
                case RecordType.MINFO:
                    return new MINFORecord(resourceRecord, this);
                case RecordType.MX:
                    return new MXRecord(resourceRecord, this);
                case RecordType.TXT:
                    return new TXTRecord(resourceRecord, this);
                case RecordType.RP:
                    return new RPRecord(resourceRecord, this);
                case RecordType.AFSDB:
                    return new AFSDBRecord(resourceRecord, this);
                case RecordType.X25:
                    return new X25Record(resourceRecord, this);
                case RecordType.ISDN:
                    return new ISDNRecord(resourceRecord, this);
                case RecordType.RT:
                    return new RTRecord(resourceRecord, this);
                case RecordType.NSAP:
                    return new NSAPRecord(resourceRecord, this);
                case RecordType.NSAPPTR:
                    return new NSAPPTRRecord(resourceRecord, this);
                case RecordType.SIG:
                    return new SIGRecord(resourceRecord, this);
                case RecordType.KEY:
                    return new KEYRecord(resourceRecord, this);
                case RecordType.PX:
                    return new PXRecord(resourceRecord, this);
                case RecordType.GPOS:
                    return new GPOSRecord(resourceRecord, this);
                case RecordType.AAAA:
                    return new AAAARecord(resourceRecord, this);
                case RecordType.LOC:
                    return new LOCRecord(resourceRecord, this);
                case RecordType.NXT:
                    return new NXTRecord(resourceRecord, this);
                case RecordType.EID:
                    return new EIDRecord(resourceRecord, this);
                case RecordType.NIMLOC:
                    return new NIMLOCRecord(resourceRecord, this);
                case RecordType.SRV:
                    return new SRVRecord(resourceRecord, this);
                case RecordType.ATMA:
                    return new ATMARecord(resourceRecord, this);
                case RecordType.NAPTR:
                    return new NAPTRRecord(resourceRecord, this);
                case RecordType.KX:
                    return new KXRecord(resourceRecord, this);
                case RecordType.CERT:
                    return new CERTRecord(resourceRecord, this);
                case RecordType.A6:
                    return new A6Record(resourceRecord, this);
                case RecordType.DNAME:
                    return new DNAMERecord(resourceRecord, this);
                case RecordType.SINK:
                    return new SINKRecord(resourceRecord, this);
                case RecordType.OPT:
                    return new OPTRecord(resourceRecord, this);
                case RecordType.APL:
                    return new APLRecord(resourceRecord, this);
                case RecordType.DS:
                    return new DSRecord(resourceRecord, this);
                case RecordType.SSHFP:
                    return new SSHFPRecord(resourceRecord, this);
                case RecordType.IPSECKEY:
                    return new IPSECKEYRecord(resourceRecord, this);
                case RecordType.RRSIG:
                    return new RRSIGRecord(resourceRecord, this);
                case RecordType.NSEC:
                    return new NSECRecord(resourceRecord, this);
                case RecordType.DNSKEY:
                    return new DNSKEYRecord(resourceRecord, this);
                case RecordType.DHCID:
                    return new DHCIDRecord(resourceRecord, this);
                case RecordType.NSEC3:
                    return new NSEC3Record(resourceRecord, this);
                case RecordType.NSEC3PARAM:
                    return new NSEC3PARAMRecord(resourceRecord, this);
                case RecordType.HIP:
                    return new HIPRecord(resourceRecord, this);
                case RecordType.SPF:
                    return new SPFRecord(resourceRecord, this);
                case RecordType.UINFO:
                    return new UINFORecord(resourceRecord, this);
                case RecordType.UID:
                    return new UIDRecord(resourceRecord, this);
                case RecordType.GID:
                    return new GIDRecord(resourceRecord, this);
                case RecordType.UNSPEC:
                    return new UNSPECRecord(resourceRecord, this);
                case RecordType.TKEY:
                    return new TKEYRecord(resourceRecord, this);
                case RecordType.TSIG:
                    return new RecordTSIG(resourceRecord, this);
                default:
                    return new UnknownRecord(resourceRecord, this);
            }
        }
    }
}
