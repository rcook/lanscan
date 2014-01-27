//////////////////////////////////////////////////////////////////////
//
// Lanscan
// Copyright (C) 2013, Richard Cook. All rights reserved.
//
//////////////////////////////////////////////////////////////////////

namespace Lanscan.Networking.UnitTests
{
    using Lanscan.TestFramework;
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

    public static class On_DhcpInfo
    {
        [TestClass]
        public abstract class ConstructorTestBase : BddTestBase
        {
            private DhcpInfo m_result;

            protected abstract IPAddress GatewayAddress { get; }

            protected abstract IPAddress DnsServerAddress { get; }

            protected abstract string DomainName { get; }

            protected abstract IPAddress ExpectedGatewayAddress { get; }

            protected abstract IPAddress ExpectedDnsServerAddress { get; }

            protected abstract string ExpectedDomainName { get; }

            [TestMethod]
            public void Gateway_address_should_match_expected()
            {
                m_result.GatewayAddress.ShouldEqual(ExpectedGatewayAddress);
            }

            [TestMethod]
            public void DNS_server_address_should_match_expected()
            {
                m_result.DnsServerAddress.ShouldEqual(ExpectedDnsServerAddress);
            }

            [TestMethod]
            public void Domain_name_should_match_expected()
            {
                m_result.DomainName.ShouldEqual(ExpectedDomainName);
            }

            protected override void BecauseOf()
            {
                m_result = new DhcpInfo(GatewayAddress, DnsServerAddress, DomainName);
            }
        }

        [TestClass]
        public sealed class When_I_pass_valid_address_and_network : ConstructorTestBase
        {
            protected override IPAddress GatewayAddress
            {
                get { return IPAddress.Parse("11.22.33.44"); }
            }

            protected override IPAddress DnsServerAddress
            {
                get { return IPAddress.Parse("101.102.103.104"); }
            }

            protected override string DomainName
            {
                get { return "foo.com"; }
            }

            protected override IPAddress ExpectedGatewayAddress
            {
                get { return IPAddress.Parse("11.22.33.44"); }
            }

            protected override IPAddress ExpectedDnsServerAddress
            {
                get { return IPAddress.Parse("101.102.103.104"); }
            }

            protected override string ExpectedDomainName
            {
                get { return "foo.com"; }
            }
        }
    }
}
