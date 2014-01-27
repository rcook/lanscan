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

    public sealed class ScanNetworkBatch
    {
        private readonly ScanNetworkResult[] m_results;

        public ScanNetworkBatch(ScanNetworkResult[] results)
        {
            if (results == null)
            {
                throw new ArgumentNullException("results");
            }

            m_results = results;
        }

        public IReadOnlyCollection<ScanNetworkResult> Results
        {
            get { return m_results; }
        }
    }
}
