//////////////////////////////////////////////////////////////////////
//
// Lanscan
// Copyright (C) 2013, Richard Cook. All rights reserved.
//
//////////////////////////////////////////////////////////////////////

namespace Lanscan.Utilities
{
    using System;
    using System.Collections.Generic;

    public sealed class DictionaryPropertyStore : IPropertyStore
    {
        private readonly Dictionary<string, object> m_propertyValues = new Dictionary<string, object>();

        public DictionaryPropertyStore()
        {
        }

        public bool TryGetValue<T>(string propertyName, out T propertyValue)
        {
            propertyValue = default(T);

            if (String.IsNullOrWhiteSpace(propertyName))
            {
                throw new ArgumentException("Invalid property name", "propertyName");
            }

            object temp;
            if (!m_propertyValues.TryGetValue(propertyName, out temp))
            {
                return false;
            }

            propertyValue = (T)temp;
            return true;
        }

        public void SetValue<T>(string propertyName, T propertyValue)
        {
            if (String.IsNullOrWhiteSpace(propertyName))
            {
                throw new ArgumentException("Invalid property name", "propertyName");
            }

            m_propertyValues[propertyName] = propertyValue;
        }
    }
}
