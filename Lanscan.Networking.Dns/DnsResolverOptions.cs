//////////////////////////////////////////////////////////////////////
//
// Lanscan
// Copyright (C) 2013, Richard Cook. All rights reserved.
//
//////////////////////////////////////////////////////////////////////

namespace Lanscan.Networking.Dns
{
    using System;

    public sealed class DnsResolverOptions
    {
        private int m_timeout = 1000;
        private int m_retryCount = 3;
        private bool m_recursion = true;
        private bool m_useCache = true;

        public int Timeout
        {
            get { return m_timeout; }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("Timeout");
                }

                m_timeout = value;
            }
        }

        public int RetryCount
        {
            get { return m_retryCount; }
            set
            {
                if (value < 1)
                {
                    throw new ArgumentOutOfRangeException("RetryCount");
                }

                m_retryCount = value;
            }
        }

        public bool Recursion
        {
            get { return m_recursion; }
            set { m_recursion = value; }
        }

        public bool UseCache
        {
            get { return m_useCache; }
            set { m_useCache = value; }
        }
    }
}
