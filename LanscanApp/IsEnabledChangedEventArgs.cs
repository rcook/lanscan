//////////////////////////////////////////////////////////////////////
//
// Lanscan
// Copyright (C) 2013, Richard Cook. All rights reserved.
//
//////////////////////////////////////////////////////////////////////

namespace LanscanApp
{
    using System;

    public class IsEnabledChangedEventArgs : EventArgs
    {
        private readonly bool m_isEnabled;

        public IsEnabledChangedEventArgs(bool isEnabled)
        {
            m_isEnabled = isEnabled;
        }

        public bool IsEnabled
        {
            get { return m_isEnabled; }
        }
    }
}
