//////////////////////////////////////////////////////////////////////
//
// Lanscan
// Copyright (C) 2013, Richard Cook. All rights reserved.
//
//////////////////////////////////////////////////////////////////////

namespace Lanscan.Utilities
{
    using System;

    public sealed class SettingsPropertyDescriptor<T> : PropertyDescriptor<T>
    {
        private readonly string m_key;

        public SettingsPropertyDescriptor(string key, IPropertyStore propertyStore, string propertyName, Func<T> defaultValueGetter, Action changedAction = null)
            : base(propertyStore, propertyName, defaultValueGetter, changedAction)
        {
            if (String.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException("Invalid key", "key");
            }

            m_key = key;
        }
    }
}
