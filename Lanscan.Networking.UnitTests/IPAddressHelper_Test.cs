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

    public static class On_IPAddressHelper
    {
        [TestClass]
        public sealed class GetAddressBytesTest : BddTestBase
        {
            [TestMethod]
            public void TestIt()
            {
                var addressBytes = IPAddressHelper.GetAddressBytes(0x11223344);
                addressBytes.Length.ShouldEqual(4);
                addressBytes[0].ShouldEqual(0x11);
                addressBytes[1].ShouldEqual(0x22);
                addressBytes[2].ShouldEqual(0x33);
                addressBytes[3].ShouldEqual(0x44);
            }
        }

        [TestClass]
        public sealed class GetAddressValueTest : BddTestBase
        {
            [TestMethod]
            public void TestIt()
            {
                Catch(() => { IPAddressHelper.GetAddressValue(null); }).ShouldBeOfType<ArgumentNullException>();
                Catch(() => { IPAddressHelper.GetAddressValue(new byte[0]); }).ShouldBeOfType<ArgumentException>();
                Catch(() => { IPAddressHelper.GetAddressValue(new byte[] { 0, 1 }); }).ShouldBeOfType<ArgumentException>();

                var addressValue = IPAddressHelper.GetAddressValue(new byte[] { 0x11, 0x22, 0x33, 0x44 });
                addressValue.ShouldEqual(0x11223344);
            }
        }

        [TestClass]
        public sealed class GetAddressStringTest : BddTestBase
        {
            [TestMethod]
            public void TestIt()
            {
                Catch(() => { IPAddressHelper.GetAddressString(null); }).ShouldBeOfType<ArgumentNullException>();
                Catch(() => { IPAddressHelper.GetAddressString(new byte[0]); }).ShouldBeOfType<ArgumentException>();
                Catch(() => { IPAddressHelper.GetAddressString(new byte[] { 0, 1 }); }).ShouldBeOfType<ArgumentException>();

                var addressString1 = IPAddressHelper.GetAddressString(new byte[] { 0x11, 0x22, 0x33, 0x44 });
                addressString1.ShouldEqual("17.34.51.68");

                var addressString2 = IPAddressHelper.GetAddressString(0x11223345);
                addressString2.ShouldEqual("17.34.51.69");
            }
        }
    }
}
