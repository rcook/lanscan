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

    public sealed class PredefinedNetworkService : INetworkService
    {
        private readonly NetworkServiceRegistryEntry m_service;
        private readonly bool m_isEnabled;

        public PredefinedNetworkService(NetworkServiceRegistryEntry service, bool isEnabled)
        {
            if (service == null)
            {
                throw new ArgumentNullException("service");
            }

            m_service = service;
            m_isEnabled = isEnabled;
        }

        public NetworkServiceRegistryEntry Service
        {
            get { return m_service; }
        }

        public string Name
        {
            get { return m_service.Name; }
        }

        public Protocol Protocol
        {
            get { return m_service.Protocol; }
        }

        public ushort Port
        {
            get { return m_service.Port; }
        }

        public bool IsEnabled
        {
            get { return m_isEnabled; }
        }
    }
}
