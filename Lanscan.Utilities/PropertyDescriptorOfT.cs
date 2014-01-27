//////////////////////////////////////////////////////////////////////
//
// Lanscan
// Copyright (C) 2013, Richard Cook. All rights reserved.
//
//////////////////////////////////////////////////////////////////////

namespace Lanscan.Utilities
{
    using System;

    public abstract class PropertyDescriptor<T> : IPropertyDescriptor<T>
    {
        private readonly IPropertyStore m_propertyStore;
        private readonly string m_propertyName;
        private readonly Func<T> m_defaultValueGetter;
        private readonly Action m_changedAction;

        protected PropertyDescriptor(IPropertyStore propertyStore, string propertyName, Func<T> defaultValueGetter, Action changedAction = null)
        {
            if (propertyStore == null)
            {
                throw new ArgumentNullException("propertyStore");
            }
            if (String.IsNullOrWhiteSpace(propertyName))
            {
                throw new ArgumentException("Invalid property name", "propertyName");
            }
            if (defaultValueGetter == null)
            {
                throw new ArgumentNullException("defaultValueGetter");
            }

            m_propertyStore = propertyStore;
            m_propertyName = propertyName;
            m_defaultValueGetter = defaultValueGetter;
            m_changedAction = changedAction;
        }

        public string PropertyName
        {
            get { return m_propertyName; }
        }

        public Action ChangedAction
        {
            get { return m_changedAction; }
        }

        public bool TryGetValue(out T propertyValue)
        {
            return m_propertyStore.TryGetValue(m_propertyName, out propertyValue);
        }

        public void SetValue(T propertyValue)
        {
            m_propertyStore.SetValue(m_propertyName, propertyValue);
        }

        public T GetDefaultValue()
        {
            return m_defaultValueGetter();
        }
    }
}
