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

    public static class On_ConnectionInfo
    {
        [TestClass]
        public abstract class ConstructorTestBase : BddTestBase
        {
            private ConnectionInfo m_result;

            protected abstract IPAddress Address { get; }

            protected abstract IPNetwork Network { get; }

            protected abstract IPAddress ExpectedAddress { get; }

            protected abstract string ExpectedNetworkString { get; }

            [TestMethod]
            public void Address_should_match_expected()
            {
                m_result.Address.ShouldEqual(ExpectedAddress);
            }

            [TestMethod]
            public void Network_string_should_match_expected()
            {
                m_result.Network.ToString().ShouldEqual(ExpectedNetworkString);
            }

            protected override void BecauseOf()
            {
                m_result = new ConnectionInfo(Address, Network);
            }
        }

        [TestClass]
        public sealed class When_I_pass_valid_address_and_network : ConstructorTestBase
        {
            protected override IPAddress Address
            {
                get { return IPAddress.Parse("1.2.3.4"); }
            }

            protected override IPNetwork Network
            {
                get { return new IPNetwork(IPAddress.Parse("11.22.33.44"), 5); }
            }

            protected override IPAddress ExpectedAddress
            {
                get { return IPAddress.Parse("1.2.3.4"); }
            }

            protected override string ExpectedNetworkString
            {
                get { return "8.0.0.0/5"; }
            }
        }
    }
}
