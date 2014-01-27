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

    public sealed class NetworkServiceRegistryEntry
    {
        private readonly Guid m_guid;
        private readonly string m_name;
        private readonly Protocol m_protocol;
        private readonly string m_description;
        private readonly ushort m_port;

        public NetworkServiceRegistryEntry(Guid guid, string name, Protocol protocol, string description, ushort port)
        {
            if (String.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Invalid name", "name");
            }
            if (description != null && description.Length < 1)
            {
                throw new ArgumentException("Invalid description", "description");
            }

            m_guid = guid;
            m_name = name;
            m_protocol = protocol;
            m_description = description;
            m_port = port;
        }

        public Guid Guid
        {
            get { return m_guid; }
        }

        public string Name
        {
            get { return m_name; }
        }

        public Protocol Protocol
        {
            get { return m_protocol; }
        }

        public string Description
        {
            get { return m_description; }
        }

        public ushort Port
        {
            get { return m_port; }
        }
    }
}
