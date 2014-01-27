//////////////////////////////////////////////////////////////////////
//
// Lanscan
// Copyright (C) 2013, Richard Cook. All rights reserved.
//
//////////////////////////////////////////////////////////////////////

namespace Lanscan.Networking
{
    using System;
    using System.Globalization;

    public sealed class IPNetwork
    {
        private readonly byte m_prefixLength;
        private readonly IPNetworkMask m_networkMask;
        private readonly uint m_networkAddressValue;
        private readonly IPAddress m_networkAddress;
        private readonly IPAddress m_firstUsableAddress;
        private readonly IPAddress m_lastUsableAddress;
        private readonly IPAddress m_broadcastAddress;
        private readonly IPAddressRange m_usableAddresses;

        public IPNetwork(IPAddress networkAddress, byte prefixLength)
            : this(GetAddressValue(networkAddress), prefixLength)
        {
        }

        public IPNetwork(uint value, byte prefixLength)
        {
            if (!IPNetworkHelper.IsValidNetworkPrefixLength(prefixLength))
            {
                throw new ArgumentOutOfRangeException("prefixLength");
            }

            m_prefixLength = prefixLength;

            // TODO: Consider moving GetNetworkMaskValue into constructor IPNetworkMask since we now have a separate class for it...
            m_networkMask = new IPNetworkMask(IPNetworkHelper.GetNetworkMaskValue(m_prefixLength));

            m_networkAddressValue = value & m_networkMask.Value;
            m_networkAddress = new IPAddress(m_networkAddressValue);
            if (m_prefixLength < 32)
            {
                m_firstUsableAddress = new IPAddress(m_networkAddressValue + 1);
                var wildcardMask = 0xffffffff - m_networkMask.Value;
                var broadcastAddressValue = m_networkAddressValue | wildcardMask;
                m_lastUsableAddress = new IPAddress(broadcastAddressValue - 1);
                m_broadcastAddress = new IPAddress(broadcastAddressValue);
                m_usableAddresses = new IPAddressRange(m_firstUsableAddress, m_lastUsableAddress);
            }
            else
            {
                m_usableAddresses = IPAddressRange.Empty;
            }
        }

        public byte PrefixLength
        {
            get { return m_prefixLength; }
        }

        public IPNetworkMask NetworkMask
        {
            get { return m_networkMask; }
        }

        public IPAddress NetworkAddress
        {
            get { return m_networkAddress; }
        }

        public IPAddress FirstUsableAddress
        {
            get { return m_firstUsableAddress; }
        }

        public IPAddress LastUsableAddress
        {
            get { return m_lastUsableAddress; }
        }

        public IPAddress BroadcastAddress
        {
            get { return m_broadcastAddress; }
        }

        public IPAddressRange UsableAddresses
        {
            get { return m_usableAddresses; }
        }

        public override string ToString()
        {
            var result = String.Format(
                CultureInfo.InvariantCulture,
                "{0}/{1}",
                m_networkAddress,
                m_prefixLength);
            return result;
        }

        private static uint GetAddressValue(IPAddress networkAddress)
        {
            if (networkAddress == null)
            {
                throw new ArgumentNullException("networkAddress");
            }

            var result = networkAddress.Value;
            return result;
        }
    }
}
