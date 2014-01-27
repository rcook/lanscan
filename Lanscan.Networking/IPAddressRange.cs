//////////////////////////////////////////////////////////////////////
//
// Lanscan
// Copyright (C) 2013, Richard Cook. All rights reserved.
//
//////////////////////////////////////////////////////////////////////

namespace Lanscan.Networking
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public sealed class IPAddressRange : IReadOnlyCollection<IPAddress>
    {
        private static readonly IPAddressRange s_empty = new IPAddressRange();
        private readonly bool m_isEmpty;
        private readonly uint m_firstValue;
        private readonly uint m_lastValue;
        private readonly int m_count;

        public IPAddressRange(IPAddress firstAddress, IPAddress lastAddress)
            : this(GetAddressValue(firstAddress), GetAddressValue(lastAddress))
        {
        }

        public IPAddressRange(uint firstValue, uint lastValue)
        {
            if (lastValue < firstValue)
            {
                throw new ArgumentOutOfRangeException("lastValue");
            }

            m_firstValue = firstValue;
            m_lastValue = lastValue;

            // Cast to long to prevent integer overflow.
            m_count = m_isEmpty ? 0 : checked((int)((long)m_lastValue - (long)m_firstValue + 1));
        }

        private IPAddressRange()
        {
            m_isEmpty = true;
        }

        public static IPAddressRange Empty
        {
            get { return s_empty; }
        }

        public uint FirstValue
        {
            get
            {
                if (m_isEmpty)
                {
                    throw new InvalidOperationException("IP address range is empty");
                }

                return m_firstValue;
            }
        }

        public uint LastValue
        {
            get
            {
                if (m_isEmpty)
                {
                    throw new InvalidOperationException("IP address range is empty");
                }

                return m_lastValue;
            }
        }

        public int Count
        {
            get { return m_count; }
        }

        // TODO: Consider ways to memoize this!
        public IEnumerator<IPAddress> GetEnumerator()
        {
            if (!m_isEmpty)
            {
                for (var i = m_firstValue; i <= m_lastValue; ++i)
                {
                    yield return new IPAddress(i);
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            var result = GetEnumerator();
            return result;
        }

        public IEnumerable<IPAddressRange> Chunk(int chunkSize)
        {
            if (chunkSize < 1)
            {
                throw new ArgumentOutOfRangeException("chunkSize");
            }

            if (!m_isEmpty)
            {
                for (var value = m_firstValue; value <= m_lastValue; value += (uint)chunkSize)
                {
                    var lowerLimit = value;
                    var upperLimit = (uint)Math.Min(lowerLimit + chunkSize, m_lastValue + 1);
                    var chunk = new IPAddressRange(lowerLimit, upperLimit - 1);
                    yield return chunk;
                }
            }
        }

        private static uint GetAddressValue(IPAddress address)
        {
            if (address == null)
            {
                throw new ArgumentNullException("address");
            }

            var result = address.Value;
            return result;
        }
    }
}
