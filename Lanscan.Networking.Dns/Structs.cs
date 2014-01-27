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
    /*
     * 3.2.3. QTYPE values
     *
     * QTYPE fields appear in the question part of a query.  QTYPES are a
     * superset of TYPEs, hence all TYPEs are valid QTYPEs.  In addition, the
     * following QTYPEs are defined:
     *
     *		QTYPE		value			meaning
     */
    public enum QType : ushort
    {
        A = RecordType.A,			// a IPV4 host address
        NS = RecordType.NS,		// an authoritative name server
        MD = RecordType.MD,		// a mail destination (Obsolete - use MX)
        MF = RecordType.MF,		// a mail forwarder (Obsolete - use MX)
        CNAME = RecordType.CNAME,	// the canonical name for an alias
        SOA = RecordType.SOA,		// marks the start of a zone of authority
        MB = RecordType.MB,		// a mailbox domain name (EXPERIMENTAL)
        MG = RecordType.MG,		// a mail group member (EXPERIMENTAL)
        MR = RecordType.MR,		// a mail rename domain name (EXPERIMENTAL)
        NULL = RecordType.NULL,	// a null RR (EXPERIMENTAL)
        WKS = RecordType.WKS,		// a well known service description
        PTR = RecordType.PTR,		// a domain name pointer
        HINFO = RecordType.HINFO,	// host information
        MINFO = RecordType.MINFO,	// mailbox or mail list information
        MX = RecordType.MX,		// mail exchange
        TXT = RecordType.TXT,		// text strings

        RP = RecordType.RP,		// The Responsible Person rfc1183
        AFSDB = RecordType.AFSDB,	// AFS Data Base location
        X25 = RecordType.X25,		// X.25 address rfc1183
        ISDN = RecordType.ISDN,	// ISDN address rfc1183
        RT = RecordType.RT,		// The Route Through rfc1183

        NSAP = RecordType.NSAP,	// Network service access point address rfc1706
        NSAP_PTR = RecordType.NSAPPTR, // Obsolete, rfc1348

        SIG = RecordType.SIG,		// Cryptographic public key signature rfc2931 / rfc2535
        KEY = RecordType.KEY,		// Public key as used in DNSSEC rfc2535

        PX = RecordType.PX,		// Pointer to X.400/RFC822 mail mapping information rfc2163

        GPOS = RecordType.GPOS,	// Geographical position rfc1712 (obsolete)

        AAAA = RecordType.AAAA,	// a IPV6 host address

        LOC = RecordType.LOC,		// Location information rfc1876

        NXT = RecordType.NXT,		// Obsolete rfc2065 / rfc2535

        EID = RecordType.EID,		// *** Endpoint Identifier (Patton)
        NIMLOC = RecordType.NIMLOC,// *** Nimrod Locator (Patton)

        SRV = RecordType.SRV,		// Location of services rfc2782

        ATMA = RecordType.ATMA,	// *** ATM Address (Dobrowski)

        NAPTR = RecordType.NAPTR,	// The Naming Authority Pointer rfc3403

        KX = RecordType.KX,		// Key Exchange Delegation Record rfc2230

        CERT = RecordType.CERT,	// *** CERT RFC2538

        A6 = RecordType.A6,		// IPv6 address rfc3363
        DNAME = RecordType.DNAME,	// A way to provide aliases for a whole domain, not just a single domain name as with CNAME. rfc2672

        SINK = RecordType.SINK,	// *** SINK Eastlake
        OPT = RecordType.OPT,		// *** OPT RFC2671

        APL = RecordType.APL,		// *** APL [RFC3123]

        DS = RecordType.DS,		// Delegation Signer rfc3658

        SSHFP = RecordType.SSHFP,	// *** SSH Key Fingerprint RFC-ietf-secsh-dns
        IPSECKEY = RecordType.IPSECKEY, // rfc4025
        RRSIG = RecordType.RRSIG,	// *** RRSIG RFC-ietf-dnsext-dnssec-2535
        NSEC = RecordType.NSEC,	// *** NSEC RFC-ietf-dnsext-dnssec-2535
        DNSKEY = RecordType.DNSKEY,// *** DNSKEY RFC-ietf-dnsext-dnssec-2535
        DHCID = RecordType.DHCID,	// rfc4701

        NSEC3 = RecordType.NSEC3,	// RFC5155
        NSEC3PARAM = RecordType.NSEC3PARAM, // RFC5155

        HIP = RecordType.HIP,		// RFC-ietf-hip-dns-09.txt

        SPF = RecordType.SPF,		// RFC4408
        UINFO = RecordType.UINFO,	// *** IANA-Reserved
        UID = RecordType.UID,		// *** IANA-Reserved
        GID = RecordType.GID,		// *** IANA-Reserved
        UNSPEC = RecordType.UNSPEC,// *** IANA-Reserved

        TKEY = RecordType.TKEY,	// Transaction key rfc2930
        TSIG = RecordType.TSIG,	// Transaction signature rfc2845

        IXFR = 251,			// incremental transfer                  [RFC1995]
        AXFR = 252,			// transfer of an entire zone            [RFC1035]
        MAILB = 253,		// mailbox-related RRs (MB, MG or MR)    [RFC1035]
        MAILA = 254,		// mail agent RRs (Obsolete - see MX)    [RFC1035]
        ANY = 255,			// A request for all records             [RFC1035]

        TA = RecordType.TA,		// DNSSEC Trust Authorities    [Weiler]  13 December 2005
        DLV = RecordType.DLV		// DNSSEC Lookaside Validation [RFC4431]
    }
    /*
     * 3.2.4. CLASS values
     *
     * CLASS fields appear in resource records.  The following CLASS mnemonics
     *and values are defined:
     *
     *		CLASS		value			meaning
     */
    public enum Class : ushort
    {
        IN = 1,				// the Internet
        CS = 2,				// the CSNET class (Obsolete - used only for examples in some obsolete RFCs)
        CH = 3,				// the CHAOS class
        HS = 4				// Hesiod [Dyer 87]
    }
    /*
     * 3.2.5. QCLASS values
     *
     * QCLASS fields appear in the question section of a query.  QCLASS values
     * are a superset of CLASS values; every CLASS is a valid QCLASS.  In
     * addition to CLASS values, the following QCLASSes are defined:
     *
     *		QCLASS		value			meaning
     */
    public enum QClass : ushort
    {
        IN = Class.IN,		// the Internet
        CS = Class.CS,		// the CSNET class (Obsolete - used only for examples in some obsolete RFCs)
        CH = Class.CH,		// the CHAOS class
        HS = Class.HS,		// Hesiod [Dyer 87]

        ANY = 255			// any class
    }

    /*
RCODE           Response code - this 4 bit field is set as part of
                responses.  The values have the following
                interpretation:
     */
    public enum RCode
    {
        NoError = 0,		// No Error                           [RFC1035]
        FormErr = 1,		// Format Error                       [RFC1035]
        ServFail = 2,		// Server Failure                     [RFC1035]
        NXDomain = 3,		// Non-Existent Domain                [RFC1035]
        NotImp = 4,			// Not Implemented                    [RFC1035]
        Refused = 5,		// Query Refused                      [RFC1035]
        YXDomain = 6,		// Name Exists when it should not     [RFC2136]
        YXRRSet = 7,		// RR Set Exists when it should not   [RFC2136]
        NXRRSet = 8,		// RR Set that should exist does not  [RFC2136]
        NotAuth = 9,		// Server Not Authoritative for zone  [RFC2136]
        NotZone = 10,		// Name not contained in zone         [RFC2136]

        RESERVED11 = 11,	// Reserved
        RESERVED12 = 12,	// Reserved
        RESERVED13 = 13,	// Reserved
        RESERVED14 = 14,	// Reserved
        RESERVED15 = 15,	// Reserved

        BADVERSSIG = 16,	// Bad OPT Version                    [RFC2671]
        // TSIG Signature Failure             [RFC2845]
        BADKEY = 17,		// Key not recognized                 [RFC2845]
        BADTIME = 18,		// Signature out of time window       [RFC2845]
        BADMODE = 19,		// Bad TKEY Mode                      [RFC2930]
        BADNAME = 20,		// Duplicate key name                 [RFC2930]
        BADALG = 21,		// Algorithm not supported            [RFC2930]
        BADTRUNC = 22		// Bad Truncation                     [RFC4635]
        /*
            23-3840              available for assignment
                0x0016-0x0F00
            3841-4095            Private Use
                0x0F01-0x0FFF
            4096-65535           available for assignment
                0x1000-0xFFFF
        */

    }

    /*
OPCODE          A four bit field that specifies kind of query in this
                message.  This value is set by the originator of a query
                and copied into the response.  The values are:

                0               a standard query (QUERY)

                1               an inverse query (IQUERY)

                2               a server status request (STATUS)

                3-15            reserved for future use
     */
    public enum OPCode
    {
        Query = 0,				// a standard query (QUERY)
        IQUERY = 1,				// OpCode Retired (previously IQUERY - No further [RFC3425]
        // assignment of this code available)
        Status = 2,				// a server status request (STATUS) RFC1035
        RESERVED3 = 3,			// IANA

        Notify = 4,				// RFC1996
        Update = 5,				// RFC2136

        RESERVED6 = 6,
        RESERVED7 = 7,
        RESERVED8 = 8,
        RESERVED9 = 9,
        RESERVED10 = 10,
        RESERVED11 = 11,
        RESERVED12 = 12,
        RESERVED13 = 13,
        RESERVED14 = 14,
        RESERVED15 = 15,
    }
}
