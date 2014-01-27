//////////////////////////////////////////////////////////////////////
//
// Lanscan
// Copyright (C) 2013, Richard Cook. All rights reserved.
//
//////////////////////////////////////////////////////////////////////

namespace Lanscan.Networking
{
    using System;

    public sealed class IPAddressHelper
    {
        public static byte[] GetAddressBytes(uint value)
        {
            var result = new byte[]
            {
                unchecked((byte)(value >> 24 & 0xFF)),
                unchecked((byte)(value >> 16 & 0xFF)),
                unchecked((byte)(value >> 8 & 0xFF)),
                unchecked((byte)(value & 0xFF)),
            };
            return result;
        }

        public static uint GetAddressValue(byte[] addressBytes)
        {
            if (addressBytes == null)
            {
                throw new ArgumentNullException("addressBytes");
            }
            if (!IsValidAddressBytes(addressBytes))
            {
                throw new ArgumentException("Invalid address bytes", "addressBytes");
            }

            var result = (uint)addressBytes[0] << 24 | (uint)addressBytes[1] << 16 | (uint)addressBytes[2] << 8 | (uint)addressBytes[3];
            return result;
        }

        public static string GetAddressString(uint value)
        {
            var addressBytes = GetAddressBytes(value);
            var result = GetAddressString(addressBytes);
            return result;
        }

        public static string GetAddressString(byte[] addressBytes)
        {
            if (addressBytes == null)
            {
                throw new ArgumentNullException("addressBytes");
            }
            if (!IsValidAddressBytes(addressBytes))
            {
                throw new ArgumentException("Invalid address bytes", "addressBytes");
            }

            var result = String.Join(".", addressBytes);
            return result;
        }

        private static bool IsValidAddressBytes(byte[] data)
        {
            var result = data != null && data.Length == 4;
            return result;
        }
    }
}
