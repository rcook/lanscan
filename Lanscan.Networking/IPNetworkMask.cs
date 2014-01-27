//////////////////////////////////////////////////////////////////////
//
// Lanscan
// Copyright (C) 2013, Richard Cook. All rights reserved.
//
//////////////////////////////////////////////////////////////////////

namespace Lanscan.Networking
{
    public sealed class IPNetworkMask
    {
        private readonly uint m_value;

        // TODO: Consider disallowing invalid network masks in the future!
        public IPNetworkMask(uint value)
        {
            m_value = value;
        }

        public uint Value
        {
            get { return m_value; }
        }

        public override string ToString()
        {
            var result = IPAddressHelper.GetAddressString(m_value);
            return result;
        }
    }
}
