//////////////////////////////////////////////////////////////////////
//
// Lanscan
// Copyright (C) 2013, Richard Cook. All rights reserved.
//
//////////////////////////////////////////////////////////////////////

namespace LanscanApp
{
    using System;
    using Lanscan.DataContracts;
    using Lanscan.Networking;

    public sealed class UserNetworkService : INetworkService
    {
        private readonly V1Service m_service;
        private readonly bool m_isEnabled;

        public UserNetworkService(V1Service service, bool isEnabled)
        {
            if (service == null)
            {
                throw new ArgumentNullException("service");
            }

            m_service = service;
            m_isEnabled = isEnabled;
        }

        public V1Service Service
        {
            get { return m_service; }
        }

        public string Name
        {
            get { return m_service.Name; }
        }

        public Protocol Protocol
        {
            get { return m_service.Protocol.Convert(); }
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
