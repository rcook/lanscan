//////////////////////////////////////////////////////////////////////
//
// Lanscan
// Copyright (C) 2013, Richard Cook. All rights reserved.
//
//////////////////////////////////////////////////////////////////////

namespace LanscanApp
{
    using System;
    using System.Globalization;
    using System.Threading.Tasks;
    using Lanscan.Networking;
    using Lanscan.Utilities;
    using IServiceProvider = Lanscan.Utilities.IServiceProvider;

    public sealed class AppServiceProvider : IServiceProvider, ISettingsService, IHostInfoService, IConnectionInfoService, IDhcpInfoService, IExternalAddressService
    {
        private readonly SettingsPropertyStore m_settingsPropertyStore = new SettingsPropertyStore(V1SettingsConstants.ContainerName);

        public AppServiceProvider()
        {
        }

        public async Task InitializeAsync()
        {
            await m_settingsPropertyStore.InitializeAsync();
        }

        public T GetService<T>() where T : class
        {
            var service = this as T;
            if (service == null)
            {
                throw new NotSupportedException(String.Format(CultureInfo.InvariantCulture, "Unsupported service type {0}", typeof(T).FullName));
            }

            return service;
        }

        public IPropertyStore SettingsPropertyStore
        {
            get { return m_settingsPropertyStore; }
        }

        public HostInfo GetCurrentHostInfo()
        {
            var result = HostInfo.GetCurrentHostInfo();
            return result;
        }

        public ConnectionInfo GetConnectionInfo(HostInfo hostInfo)
        {
            if (hostInfo == null)
            {
                throw new ArgumentNullException("hostInfo");
            }

            var result = ConnectionInfo.GetConnectionInfo(hostInfo);
            return result;
        }

        public DhcpInfo GetDhcpInfo(Guid networkAdapterId)
        {
            var result = DhcpInfo.GetDhcpInfo(networkAdapterId);
            return result;
        }

        public Task<IPAddress> GetCurrentExternalAddressAsync()
        {
            var result = ExternalAddressHelper.GetCurrentExternalAddressAsync();
            return result;
        }
    }
}
