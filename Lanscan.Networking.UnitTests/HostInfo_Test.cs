//////////////////////////////////////////////////////////////////////
//
// Lanscan
// Copyright (C) 2013, Richard Cook. All rights reserved.
//
//////////////////////////////////////////////////////////////////////

namespace Lanscan.Networking.UnitTests
{
    using System;
    using Lanscan.TestFramework;
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
    using Windows.Networking;

    public static class On_HostInfo
    {
        [TestClass]
        public sealed class When_I_create_a_valid_HostInfo : BddTestBase
        {
            private Guid m_networkAdapterId = Guid.NewGuid();
            private HostInfo m_result;

            [TestMethod]
            public void Network_adapter_ID_should_match_expected()
            {
                m_result.NetworkAdapterId.ShouldEqual(m_networkAdapterId);
            }

            [TestMethod]
            public void Host_name_should_match_expected()
            {
                m_result.HostName.IsEqual(new HostName("my-host-name"));
            }

            [TestMethod]
            public void IP_address_should_match_expected()
            {
                m_result.Address.ToString().ShouldEqual("4.3.2.1");
            }

            protected override void BecauseOf()
            {
                m_result = new HostInfo(
                    m_networkAdapterId,
                    new HostName("my-host-name"),
                    IPAddress.Parse("4.3.2.1"));
            }
        }
    }
}
