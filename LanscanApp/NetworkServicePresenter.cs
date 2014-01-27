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

    public abstract class NetworkServicePresenter
    {
        private readonly string m_name;
        private readonly Protocol m_protocol;
        private readonly ushort m_port;
        private bool m_isEnabled;

        protected NetworkServicePresenter(string name, Protocol protocol, ushort port, bool isEnabled)
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

        public event EventHandler<IsEnabledChangedEventArgs> IsEnabledChanged;

        public string Name
        {
            get { return m_name; }
        }

        public Protocol Protocol
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
            set
            {
                if (value != m_isEnabled)
                {
                    m_isEnabled = value;
                    RaiseIsEnabledChanged(m_isEnabled);
                }
            }
        }

        private void RaiseIsEnabledChanged(bool isEnabled)
        {
            var handler = IsEnabledChanged;
            if (handler != null)
            {
                handler(this, new IsEnabledChangedEventArgs(isEnabled));
            }
        }
    }
}
