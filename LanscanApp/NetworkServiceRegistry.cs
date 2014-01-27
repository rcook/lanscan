//////////////////////////////////////////////////////////////////////
//
// Lanscan
// Copyright (C) 2013, Richard Cook. All rights reserved.
//
//////////////////////////////////////////////////////////////////////

namespace LanscanApp
{
    using System;
    using System.Collections.Generic;
    using Lanscan.Networking;

    public static partial class NetworkServiceRegistry
    {
        private static readonly Dictionary<Guid, NetworkServiceRegistryEntry> EntriesInternal = new Dictionary<Guid, NetworkServiceRegistryEntry>();

        static NetworkServiceRegistry()
        {
            // TCP services.
            RegisterNetworkService("5632170c-c2ed-4122-b604-000000000000", "ssh", Protocol.Tcp, "The Secure Shell (SSH) Protocol", 22);
            RegisterNetworkService("5632170c-c2ed-4122-b604-000000000001", "telnet", Protocol.Tcp, "Telnet", 23);
            RegisterNetworkService("5632170c-c2ed-4122-b604-000000000002", "domain", Protocol.Tcp, "Domain Name Server", 53);
            RegisterNetworkService("5632170c-c2ed-4122-b604-000000000003", "http", Protocol.Tcp, "World Wide Web HTTP", 80);
            RegisterNetworkService("5632170c-c2ed-4122-b604-000000000004", "epmap", Protocol.Tcp, "DCE endpoint resolution", 135);
            RegisterNetworkService("5632170c-c2ed-4122-b604-000000000005", "https", Protocol.Tcp, "http protocol over TLS/SSL", 443);
            RegisterNetworkService("5632170c-c2ed-4122-b604-000000000006", "microsoft-ds", Protocol.Tcp, "Microsoft-DS", 445);
            RegisterNetworkService("5632170c-c2ed-4122-b604-000000000007", "printer", Protocol.Tcp, "spooler", 515);
            RegisterNetworkService("5632170c-c2ed-4122-b604-000000000008", "afpovertcp", Protocol.Tcp, "AFP over TCP", 548);
            RegisterNetworkService("5632170c-c2ed-4122-b604-000000000009", "rtsp", Protocol.Tcp, "Real Time Streaming Protocol (RTSP)", 554);
            RegisterNetworkService("5632170c-c2ed-4122-b604-00000000000a", "ipp", Protocol.Tcp, "IPP (Internet Printing Protocol)", 631);
            RegisterNetworkService("5632170c-c2ed-4122-b604-00000000000b", "ideafarm-door", Protocol.Tcp, "self documenting Telnet Door", 902);
            RegisterNetworkService("5632170c-c2ed-4122-b604-00000000000c", "ms-wbt-server", Protocol.Tcp, "MS WBT Server", 3389);
            RegisterNetworkService("5632170c-c2ed-4122-b604-00000000000d", "commplex-main", Protocol.Tcp, null, 5000);
            RegisterNetworkService("5632170c-c2ed-4122-b604-00000000000e", "commplex-link", Protocol.Tcp, null, 5001);
            RegisterNetworkService("5632170c-c2ed-4122-b604-00000000000f", "postgresql", Protocol.Tcp, "PostgreSQL Database", 5432);
            RegisterNetworkService("5632170c-c2ed-4122-b604-000000000010", "websm", Protocol.Tcp, "WebSM", 9090);

            // UDP services.
            RegisterNetworkService("5632170c-c2ed-4122-b604-000000000011", "echo", Protocol.Udp, "Echo", 7);
            RegisterNetworkService("5632170c-c2ed-4122-b604-000000000012", "netbios-ssn", Protocol.Udp, "NETBIOS Session Service", 139);
            RegisterNetworkService("5632170c-c2ed-4122-b604-000000000013", "skype", Protocol.Udp, "Skype Default Protocol", 23399);
        }

        public static IEnumerable<NetworkServiceRegistryEntry> Entries
        {
            get { return EntriesInternal.Values; }
        }

        private static void RegisterNetworkService(string guidString, string name, Protocol protocol, string description, ushort port)
        {
            var guid = Guid.Parse(guidString);
            var service = new NetworkServiceRegistryEntry(guid, name, protocol, description, port);
            EntriesInternal.Add(guid, service);
        }
    }
}
