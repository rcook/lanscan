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

    public sealed class NetworkServicePresenter<T> : NetworkServicePresenter
    {
        private readonly T m_service;

        public NetworkServicePresenter(string name, Protocol protocol, ushort port, bool isEnabled, T service)
            : base(name, protocol, port, isEnabled)
        {
            if (service == null)
            {
                throw new ArgumentNullException("service");
            }

            m_service = service;
        }

        public T Service
        {
            get { return m_service; }
        }
    }
}
