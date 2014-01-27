//////////////////////////////////////////////////////////////////////
//
// Lanscan
// Copyright (C) 2013, Richard Cook. All rights reserved.
//
//////////////////////////////////////////////////////////////////////

namespace Lanscan.Networking
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Windows.Networking;

    public sealed class IPAddress
    {
        private readonly byte[] m_addressBytes;
        private readonly uint m_value;
        private readonly string m_stringValue;
        private readonly HostName m_hostName;

        [SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "bytes")]
        public IPAddress(byte[] addressBytes)
        {
            if (addressBytes == null)
            {
                throw new ArgumentNullException("addressBytes");
            }
            if (addressBytes.Length != 4)
            {
                throw new ArgumentOutOfRangeException("addressBytes");
            }

            m_addressBytes = addressBytes;
            m_value = IPAddressHelper.GetAddressValue(m_addressBytes);
            m_stringValue = IPAddressHelper.GetAddressString(m_addressBytes);
            m_hostName = new HostName(m_stringValue);
        }

        public IPAddress(uint value)
        {
            m_addressBytes = IPAddressHelper.GetAddressBytes(value);
            m_value = value;
            m_stringValue = IPAddressHelper.GetAddressString(m_addressBytes);
            m_hostName = new HostName(m_stringValue);
        }

        public uint Value
        {
            get { return m_value; }
        }

        public HostName HostName
        {
            get { return m_hostName; }
        }

        public static IPAddress Parse(string text)
        {
            IPAddress result;
            if (!TryParse(text, out result))
            {
                throw new FormatException("Invalid format");
            }

            return result;
        }

        public static bool TryParse(string text, out IPAddress address)
        {
            address = null;

            if (String.IsNullOrWhiteSpace(text))
            {
                return false;
            }

            var segments = text.Split('.');

            var addressBytes = new byte[4];
            if (segments.Length == 3)
            {
                if (!ExtractThreeSegments(addressBytes, segments))
                {
                    return false;
                }
            }
            else if (segments.Length == 4)
            {
                if (!ExtractFourSegments(addressBytes, segments))
                {
                    return false;
                }
            }
            else
            {
                return false;
            }

            address = new IPAddress(addressBytes);
            return true;
        }

        [SuppressMessage("Microsoft.StyleCop.CSharp.Maintainability", "SA1121:UseBuiltInTypeAlias")]
        public override string ToString()
        {
            return m_stringValue;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var other = (IPAddress)obj;
            var result = Enumerable.SequenceEqual(m_addressBytes, other.m_addressBytes);
            return result;
        }

        public override int GetHashCode()
        {
            var result = m_addressBytes.GetHashCode();
            return result;
        }

        public byte[] GetAddressBytes()
        {
            var result = (byte[])m_addressBytes.Clone();
            return result;
        }

        [SuppressMessage("Microsoft.StyleCop.CSharp.Maintainability", "SA1121:UseBuiltInTypeAlias")]
        private static bool ExtractThreeSegments(byte[] addressBytes, string[] segments)
        {
            var index = 0;
            for (var i = 0; i < segments.Length; ++i, ++index)
            {
                if (!Byte.TryParse(segments[i], out addressBytes[index]))
                {
                    return false;
                }
                if (index == 1)
                {
                    addressBytes[++index] = 0;
                }
            }
            return true;
        }

        private static bool ExtractFourSegments(byte[] addressBytes, string[] segments)
        {
            for (var i = 0; i < segments.Length; ++i)
            {
                if (!Byte.TryParse(segments[i], out addressBytes[i]))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
