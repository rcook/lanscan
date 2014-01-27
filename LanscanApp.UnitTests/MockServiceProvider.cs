//////////////////////////////////////////////////////////////////////
//
// Lanscan
// Copyright (C) 2013, Richard Cook. All rights reserved.
//
//////////////////////////////////////////////////////////////////////

namespace LanscanApp.UnitTests
{
    using System;
    using System.Threading.Tasks;
    using Lanscan.Networking;
    using Lanscan.TestFramework;
    using Lanscan.Utilities;
    using IServiceProvider = Lanscan.Utilities.IServiceProvider;

    public sealed class MockServiceProvider : IServiceProvider, ISettingsService, IHostInfoService, IConnectionInfoService, IDhcpInfoService, IExternalAddressService
    {
        private readonly IPropertyStore m_settingsPropertyStore;
        private readonly HostInfo m_hostInfo;
        private readonly ConnectionInfo m_connectionInfo;
        private readonly DhcpInfo m_dhcpInfo;
        private readonly IPAddress m_externalAddress;

        public MockServiceProvider(MockServiceProviderOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException("options");
            }

            m_settingsPropertyStore = options.SettingsPropertyStore;
            m_hostInfo = options.HostInfo;
            m_connectionInfo = options.ConnectionInfo;
            m_dhcpInfo = options.DhcpInfo;
            m_externalAddress = options.ExternalAddress;
        }

        public T GetService<T>() where T : class
        {
            var service = this as T;
            service.ShouldNotBeNull();
            return service;
        }

        public IPropertyStore SettingsPropertyStore
        {
            get { return m_settingsPropertyStore; }
        }

        public HostInfo GetCurrentHostInfo()
        {
            return m_hostInfo;
        }

        public ConnectionInfo GetConnectionInfo(HostInfo hostInfo)
        {
            hostInfo.ShouldNotBeNull();
            hostInfo.ShouldEqual(m_hostInfo);
            return m_connectionInfo;
        }

        public DhcpInfo GetDhcpInfo(Guid networkAdapterId)
        {
            networkAdapterId.ShouldEqual(m_hostInfo.NetworkAdapterId);
            return m_dhcpInfo;
        }

        // http://neverindoubtnet.blogspot.com/2012/03/synchronous-tasks-with-task.html
        public Task<IPAddress> GetCurrentExternalAddressAsync()
        {
            var taskCompletionSource = new TaskCompletionSource<IPAddress>();
            taskCompletionSource.SetResult(m_externalAddress);
            var result = taskCompletionSource.Task;
            return result;
        }
    }
}
