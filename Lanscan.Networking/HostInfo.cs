//////////////////////////////////////////////////////////////////////
//
// Lanscan
// Copyright (C) 2013, Richard Cook. All rights reserved.
//
//////////////////////////////////////////////////////////////////////

namespace Lanscan.Networking
{
    using System;
    using Windows.Networking;
    using Windows.Networking.Connectivity;

    public sealed class HostInfo
    {
        private readonly Guid m_networkAdapterId;
        private readonly HostName m_hostName;
        private readonly IPAddress m_address;

        public HostInfo(Guid networkAdapterId, HostName hostName, IPAddress address)
        {
            if (hostName == null)
            {
                throw new ArgumentNullException("hostName");
            }
            if (address == null)
            {
                throw new ArgumentNullException("address");
            }

            m_networkAdapterId = networkAdapterId;
            m_hostName = hostName;
            m_address = address;
        }

        public Guid NetworkAdapterId
        {
            get { return m_networkAdapterId; }
        }

        public HostName HostName
        {
            get { return m_hostName; }
        }

        public IPAddress Address
        {
            get { return m_address; }
        }

        public static HostInfo GetCurrentHostInfo()
        {
            var profile = NetworkInformation.GetInternetConnectionProfile();
            if (profile == null || profile.NetworkAdapter == null)
            {
                return null;
            }

            var networkAdapterId = profile.NetworkAdapter.NetworkAdapterId;

            foreach (var hostName in NetworkInformation.GetHostNames())
            {
                if (!IsCurrentIPv4Address(profile, hostName))
                {
                    continue;
                }

                IPAddress address;
                if (!IPAddress.TryParse(hostName.CanonicalName, out address))
                {
                    continue;
                }

                var result = new HostInfo(networkAdapterId, hostName, address);
                return result;
            }
            return null;
        }

        private static bool IsCurrentIPv4Address(ConnectionProfile profile, HostName hostName)
        {
            var result = hostName.Type == HostNameType.Ipv4
                && IsCurrentHostName(profile, hostName);
            return result;
        }

        private static bool IsCurrentHostName(ConnectionProfile profile, HostName hostName)
        {
            var result = hostName.IPInformation != null
                && hostName.IPInformation.NetworkAdapter != null
                && hostName.IPInformation.NetworkAdapter.NetworkAdapterId == profile.NetworkAdapter.NetworkAdapterId;
            return result;
        }
    }
}
