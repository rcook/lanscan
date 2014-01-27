//////////////////////////////////////////////////////////////////////
//
// Lanscan
// Copyright (C) 2013, Richard Cook. All rights reserved.
//
//////////////////////////////////////////////////////////////////////

namespace Lanscan.DataContracts
{
    using System;
    using System.Runtime.Serialization;

    [DataContract(Name = V1Constants.Service, Namespace = V1Constants.EmptyNamespace)]
    public sealed class V1Service
    {
        [DataMember(Name = V1Constants.Name, IsRequired = true)]
        private readonly string m_name;

        [DataMember(Name = V1Constants.Protocol, IsRequired = true)]
        private readonly V1Protocol m_protocol;

        [DataMember(Name = V1Constants.Port, IsRequired = true)]
        private readonly ushort m_port;

        [DataMember(Name = V1Constants.IsEnabled, IsRequired = true)]
        private readonly bool m_isEnabled;

        public V1Service(string name, V1Protocol protocol, ushort port, bool isEnabled)
        {
            if (String.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Invalid name", "name");
            }

            m_name = name;
            m_protocol = protocol;
            m_port = port;
            m_isEnabled = isEnabled;
        }

        public string Name
        {
            get { return m_name; }
        }

        public V1Protocol Protocol
        {
            get { return m_protocol; }
        }

        public ushort Port
        {
            get { return m_port; }
        }

        public bool IsEnabled
        {
            get { return m_isEnabled; }
        }
    }
}
