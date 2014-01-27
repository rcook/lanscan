//////////////////////////////////////////////////////////////////////
//
// Lanscan
// Copyright (C) 2013, Richard Cook. All rights reserved.
//
//////////////////////////////////////////////////////////////////////

namespace Lanscan.Utilities
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq.Expressions;

    public abstract class ServiceProviderViewModel : ViewModel
    {
        private readonly IServiceProvider m_serviceProvider;
        private readonly IPropertyStore m_dictionaryPropertyStore = new DictionaryPropertyStore();
        private readonly IPropertyStore m_settingsPropertyStore;

        protected ServiceProviderViewModel(IServiceProvider serviceProvider)
        {
            if (serviceProvider == null)
            {
                throw new ArgumentNullException("serviceProvider");
            }

            m_serviceProvider = serviceProvider;
            var settingsService = m_serviceProvider.GetService<ISettingsService>();
            m_settingsPropertyStore = settingsService.SettingsPropertyStore;
        }

        protected IServiceProvider ServiceProvider
        {
            get { return m_serviceProvider; }
        }

        [SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed")]
        protected IPropertyDescriptor<T> CreateDictionaryProperty<T>(Expression<Func<T>> propertySelector, Func<T> defaultValueGetter, Action changedAction = null)
        {
            var propertyName = BindingHelper.GetPropertyName(propertySelector);
            var result = new DictionaryPropertyDescriptor<T>(m_dictionaryPropertyStore, propertyName, defaultValueGetter, changedAction);
            return result;
        }

        [SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed")]
        protected IPropertyDescriptor<T> CreateSettingsProperty<T>(string key, Expression<Func<T>> propertySelector, Func<T> defaultValueGetter, Action changedAction = null)
        {
            var propertyName = BindingHelper.GetPropertyName(propertySelector);
            var result = new SettingsPropertyDescriptor<T>(key, m_settingsPropertyStore, propertyName, defaultValueGetter, changedAction);
            return result;
        }

        protected T GetPropertyValue<T>(IPropertyDescriptor<T> propertyDescriptor)
        {
            if (propertyDescriptor == null)
            {
                throw new ArgumentNullException("propertyDescriptor");
            }

            T propertyValue;
            if (!propertyDescriptor.TryGetValue(out propertyValue))
            {
                propertyValue = propertyDescriptor.GetDefaultValue();
                UpdateAndFireNotifications(propertyDescriptor, propertyValue);
            }
            return propertyValue;
        }

        protected void SetPropertyValue<T>(IPropertyDescriptor<T> propertyDescriptor, T propertyValue)
        {
            if (propertyDescriptor == null)
            {
                throw new ArgumentNullException("propertyDescriptor");
            }

            T oldPropertyValue;
            if (!propertyDescriptor.TryGetValue(out oldPropertyValue) || !Object.Equals(oldPropertyValue, propertyValue))
            {
                UpdateAndFireNotifications(propertyDescriptor, propertyValue);
            }
        }

        private void UpdateAndFireNotifications<T>(IPropertyDescriptor<T> propertyDescriptor, T propertyValue)
        {
            propertyDescriptor.SetValue(propertyValue);
            NotifyPropertyChanged(propertyDescriptor.PropertyName);
            var handler = propertyDescriptor.ChangedAction;
            if (handler != null)
            {
                handler();
            }
        }
    }
}
