//////////////////////////////////////////////////////////////////////
//
// Lanscan
// Copyright (C) 2013, Richard Cook. All rights reserved.
//
//////////////////////////////////////////////////////////////////////

namespace LanscanApp.UnitTests
{
    using Lanscan.Networking;
    using Lanscan.Utilities;

    public sealed class MockServiceProviderOptions
    {
        public IPropertyStore SettingsPropertyStore { get; set; }
        public HostInfo HostInfo { get; set; }
        public ConnectionInfo ConnectionInfo { get; set; }
        public DhcpInfo DhcpInfo { get; set; }
        public IPAddress ExternalAddress { get; set; }
    }
}
