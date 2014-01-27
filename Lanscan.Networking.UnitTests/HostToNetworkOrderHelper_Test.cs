//////////////////////////////////////////////////////////////////////
//
// Lanscan
// Copyright (C) 2013, Richard Cook. All rights reserved.
//
//////////////////////////////////////////////////////////////////////

namespace Lanscan.Networking.UnitTests
{
    using Lanscan.Networking;
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

    public static class On_HostToNetworkOrderHelper
    {
        [TestClass]
        public sealed class ConvertTest
        {
            [TestMethod]
            public void Test_short()
            {
                Assert.AreEqual(unchecked((short)0x3412), HostToNetworkOrderHelper.Convert(unchecked((short)0x1234)));
            }

            [TestMethod]
            public void Test_int()
            {
                Assert.AreEqual(unchecked((int)0x78563412), HostToNetworkOrderHelper.Convert(unchecked((int)0x12345678)));
            }

            [TestMethod]
            public void Test_long()
            {
                Assert.AreEqual(unchecked((long)0x8866442278563412), HostToNetworkOrderHelper.Convert(unchecked((long)0x1234567822446688)));
            }
        }
    }
}
