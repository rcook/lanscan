//////////////////////////////////////////////////////////////////////
//
// Lanscan
// Copyright (C) 2013, Richard Cook. All rights reserved.
//
//////////////////////////////////////////////////////////////////////

namespace LanscanApp
{
    using System;
    using Lanscan.Networking;
    using IServiceProvider = Lanscan.Utilities.IServiceProvider;

    public sealed class BrowserPageArgs : PageArgs
    {
        private readonly IPEndpoint m_endpoint;

        public BrowserPageArgs(IServiceProvider serviceProvider, AppViewModel appViewModel, IPEndpoint endpoint)
            : base(serviceProvider, appViewModel)
        {
            if (endpoint == null)
            {
                throw new ArgumentNullException("endpoint");
            }

            m_endpoint = endpoint;
        }

        public IPEndpoint Endpoint
        {
            get { return m_endpoint; }
        }
    }
}
