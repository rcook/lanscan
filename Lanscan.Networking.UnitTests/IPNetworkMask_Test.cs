//////////////////////////////////////////////////////////////////////
//
// Lanscan
// Copyright (C) 2013, Richard Cook. All rights reserved.
//
//////////////////////////////////////////////////////////////////////

namespace Lanscan.Networking.UnitTests
{
    using Lanscan.TestFramework;
#if NETFX_CORE
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
#else
    using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

    public static class On_IPNetworkMask
    {
        [TestClass]
        public sealed class TestCase : BddTestBase
        {
            [TestMethod]
            public void TestIt()
            {
                var networkMask = new IPNetworkMask(0x11223344);
                networkMask.ToString().ShouldEqual("17.34.51.68");
            }
        }
    }
}
