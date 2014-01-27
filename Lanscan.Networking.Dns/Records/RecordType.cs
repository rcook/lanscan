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
    public enum RecordType : ushort
    {
        A = 1,				// a IPV4 host address
        NS = 2,				// an authoritative name server
        MD = 3,				// a mail destination (Obsolete - use MX)
        MF = 4,				// a mail forwarder (Obsolete - use MX)
        CNAME = 5,			// the canonical name for an alias
        SOA = 6,			// marks the start of a zone of authority
        MB = 7,				// a mailbox domain name (EXPERIMENTAL)
        MG = 8,				// a mail group member (EXPERIMENTAL)
        MR = 9,				// a mail rename domain name (EXPERIMENTAL)
        NULL = 10,			// a null RR (EXPERIMENTAL)
        WKS = 11,			// a well known service description
        PTR = 12,			// a domain name pointer
        HINFO = 13,			// host information
        MINFO = 14,			// mailbox or mail list information
        MX = 15,			// mail exchange
        TXT = 16,			// text strings

        RP = 17,			// The Responsible Person rfc1183
        AFSDB = 18,			// AFS Data Base location
        X25 = 19,			// X.25 address rfc1183
        ISDN = 20,			// ISDN address rfc1183 
        RT = 21,			// The Route Through rfc1183

        NSAP = 22,			// Network service access point address rfc1706
        NSAPPTR = 23,		// Obsolete, rfc1348

        SIG = 24,			// Cryptographic public key signature rfc2931 / rfc2535
        KEY = 25,			// Public key as used in DNSSEC rfc2535

        PX = 26,			// Pointer to X.400/RFC822 mail mapping information rfc2163

        GPOS = 27,			// Geographical position rfc1712 (obsolete)

        AAAA = 28,			// a IPV6 host address, rfc3596

        LOC = 29,			// Location information rfc1876

        NXT = 30,			// Next Domain, Obsolete rfc2065 / rfc2535

        EID = 31,			// *** Endpoint Identifier (Patton)
        NIMLOC = 32,		// *** Nimrod Locator (Patton)

        SRV = 33,			// Location of services rfc2782

        ATMA = 34,			// *** ATM Address (Dobrowski)

        NAPTR = 35,			// The Naming Authority Pointer rfc3403

        KX = 36,			// Key Exchange Delegation Record rfc2230

        CERT = 37,			// *** CERT RFC2538

        A6 = 38,			// IPv6 address rfc3363 (rfc2874 rfc3226)
        DNAME = 39,			// A way to provide aliases for a whole domain, not just a single domain name as with CNAME. rfc2672

        SINK = 40,			// *** SINK Eastlake
        OPT = 41,			// *** OPT RFC2671

        APL = 42,			// *** APL [RFC3123]

        DS = 43,			// Delegation Signer rfc3658

        SSHFP = 44,			// SSH Key Fingerprint rfc4255
        IPSECKEY = 45,		// IPSECKEY rfc4025
        RRSIG = 46,			// RRSIG rfc3755
        NSEC = 47,			// NSEC rfc3755
        DNSKEY = 48,		// DNSKEY 3755
        DHCID = 49,			// DHCID rfc4701

        NSEC3 = 50,			// NSEC3 rfc5155
        NSEC3PARAM = 51,	// NSEC3PARAM rfc5155

        HIP = 55,			// Host Identity Protocol  [RFC-ietf-hip-dns-09.txt]

        SPF = 99,			// SPF rfc4408

        UINFO = 100,		// *** IANA-Reserved
        UID = 101,			// *** IANA-Reserved
        GID = 102,			// *** IANA-Reserved
        UNSPEC = 103,		// *** IANA-Reserved

        TKEY = 249,			// Transaction key rfc2930
        TSIG = 250,			// Transaction signature rfc2845

        TA = 32768,			// DNSSEC Trust Authorities          [Weiler]  13 December 2005
        DLV = 32769			// DNSSEC Lookaside Validation       [RFC4431]
    }
}
