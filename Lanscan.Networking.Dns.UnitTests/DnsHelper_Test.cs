//////////////////////////////////////////////////////////////////////
//
// Lanscan
// Copyright (C) 2013, Richard Cook. All rights reserved.
//
//////////////////////////////////////////////////////////////////////

namespace Lanscan.Networking.Dns.UnitTests
{
    using Lanscan.Networking;
    using Lanscan.Networking.Dns;
    using Lanscan.TestFramework;
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

    public static class On_DnsHelper
    {
        [TestClass]
        public sealed class When_I_get_ARPA_URL_for_address : BddTestBase
        {
            private string m_result;

            [TestMethod]
            public void Result_should_match_expected()
            {
                m_result.ShouldEqual("44.33.22.11.in-addr.arpa.");
            }

            protected override void BecauseOf()
            {
                m_result = DnsHelper.GetArpaUrl(IPAddress.Parse("11.22.33.44"));
            }
        }
    }
}
