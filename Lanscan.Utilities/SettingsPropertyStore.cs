//////////////////////////////////////////////////////////////////////
//
// Lanscan
// Copyright (C) 2013, Richard Cook. All rights reserved.
//
//////////////////////////////////////////////////////////////////////

namespace Lanscan.Utilities
{
    using System;
    using System.Threading.Tasks;
    using Windows.Storage;

    public sealed class SettingsPropertyStore : IPropertyStore
    {
        private readonly Lazy<ApplicationDataContainer> m_container;

        public SettingsPropertyStore(string containerName)
        {
            if (String.IsNullOrWhiteSpace(containerName))
            {
                throw new ArgumentException("Invalid container name", "containerName");
            }

            m_container = new Lazy<ApplicationDataContainer>(() => ApplicationData.Current.LocalSettings.CreateContainer(containerName, ApplicationDataCreateDisposition.Always));
        }

        public async Task InitializeAsync()
        {
            await ApplicationData.Current.SetVersionAsync(1u, x => { });
        }

        public bool TryGetValue<T>(string propertyName, out T propertyValue)
        {
            propertyValue = default(T);

            if (String.IsNullOrWhiteSpace(propertyName))
            {
                throw new ArgumentException("Invalid property name", "propertyName");
            }

            object temp;
            if (!m_container.Value.Values.TryGetValue(propertyName, out temp))
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

            m_container.Value.Values[propertyName] = propertyValue;
        }
    }
}
