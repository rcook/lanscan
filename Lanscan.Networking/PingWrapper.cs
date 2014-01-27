//////////////////////////////////////////////////////////////////////
//
// Lanscan
// Copyright (C) 2013, Richard Cook. All rights reserved.
//
//////////////////////////////////////////////////////////////////////

namespace Lanscan.Networking
{
    using System;

    public abstract class PingWrapper : IDisposable
    {
        protected PingWrapper()
        {
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public abstract bool PerformPing(IPEndpoint endpoint);

        protected virtual void Dispose(bool disposing)
        {
        }
    }
}
