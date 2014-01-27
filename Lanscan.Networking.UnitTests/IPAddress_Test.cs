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

    public static class On_IPAddress
    {
        [TestClass]
        public sealed class When_I_construct_using_address_bytes : BddTestBase
        {
            private IPAddress m_result;

            [TestMethod]
            public void Value_should_match_expected()
            {
                m_result.Value.ShouldEqual(0x0B16212cu);
            }

            [TestMethod]
            public void Host_name_should_match_expected()
            {
                m_result.HostName.IsEqual(new HostName("11.22.33.44")).ShouldBeTrue();
            }

            [TestMethod]
            public void String_should_match_expected()
            {
                m_result.ToString().ShouldEqual("11.22.33.44");
            }

            [TestMethod]
            public void Address_bytes_should_match_expected()
            {
                m_result.GetAddressBytes().ShouldEqual(new byte[] { 11, 22, 33, 44 });
            }

            protected override void BecauseOf()
            {
                m_result = new IPAddress(new byte[] { 11, 22, 33, 44 });
            }
        }

        [TestClass]
        public sealed class When_I_construct_using_value : BddTestBase
        {
            private IPAddress m_result;

            [TestMethod]
            public void Value_should_match_expected()
            {
                m_result.Value.ShouldEqual(0x0B16212cu);
            }

            [TestMethod]
            public void Host_name_should_match_expected()
            {
                m_result.HostName.IsEqual(new HostName("11.22.33.44")).ShouldBeTrue();
            }

            [TestMethod]
            public void String_should_match_expected()
            {
                m_result.ToString().ShouldEqual("11.22.33.44");
            }

            [TestMethod]
            public void Address_bytes_should_match_expected()
            {
                m_result.GetAddressBytes().ShouldEqual(new byte[] { 11, 22, 33, 44 });
            }

            protected override void BecauseOf()
            {
                m_result = new IPAddress(0x0B16212cu);
            }
        }

        [TestClass]
        public abstract class ConstructorExceptionTestBase : BddTestBase
        {
            private Exception m_exception;

            protected abstract Type ExpectedExceptionType { get; }

            [TestMethod]
            public void It_should_throw_expected_exception()
            {
                m_exception.GetType().ShouldEqual(ExpectedExceptionType);
            }

            protected abstract byte[] GetAddressBytes();

            protected override void BecauseOf()
            {
                m_exception = Catch(() => new IPAddress(GetAddressBytes()));
            }
        }

        [TestClass]
        public sealed class When_I_pass_null_to_constructor : ConstructorExceptionTestBase
        {
            protected override Type ExpectedExceptionType
            {
                get { return typeof(ArgumentNullException); }
            }

            protected override byte[] GetAddressBytes()
            {
                return null;
            }
        }

        [TestClass]
        public sealed class When_I_pass_empty_address_bytes_to_constructor : ConstructorExceptionTestBase
        {
            protected override Type ExpectedExceptionType
            {
                get { return typeof(ArgumentOutOfRangeException); }
            }

            protected override byte[] GetAddressBytes()
            {
                return new byte[0];
            }
        }

        [TestClass]
        public sealed class When_I_pass_invalid_address_bytes_to_constructor : ConstructorExceptionTestBase
        {
            protected override Type ExpectedExceptionType
            {
                get { return typeof(ArgumentOutOfRangeException); }
            }

            protected override byte[] GetAddressBytes()
            {
                return new byte[] { 0, 1, 2, 3, 4 };
            }
        }

        [TestClass]
        public sealed class When_I_pass_valid_address_bytes_to_constructor : ConstructorExceptionTestBase
        {
            protected override Type ExpectedExceptionType
            {
                get { return typeof(ArgumentOutOfRangeException); }
            }

            protected override byte[] GetAddressBytes()
            {
                return new byte[] { 0, 1, 2, 3, 4 };
            }
        }

        [TestClass]
        public abstract class TryParseTestBase : BddTestBase
        {
            private bool m_result;
            private IPAddress m_address;

            protected abstract string AddressText { get; }

            protected abstract bool ExpectedResult { get; }

            protected abstract string ExpectedString { get; }

            protected abstract uint ExpectedValue { get; }

            protected abstract HostName ExpectedHostName { get; }

            [TestMethod]
            public void Result_should_match_expected()
            {
                m_result.ShouldEqual(ExpectedResult);
            }

            [TestMethod]
            public void String_should_match_expected()
            {
                m_address.ToString().ShouldEqual(ExpectedString);
            }

            [TestMethod]
            public void Address_bytes_should_match_expected()
            {
                var actualAddressBytes = m_address.GetAddressBytes();
                var expectedAddressBytes = GetExpectedAddressBytes();
                actualAddressBytes.ShouldEqual(expectedAddressBytes);
            }

            [TestMethod]
            public void Value_should_match_expected()
            {
                m_address.Value.ShouldEqual(ExpectedValue);
            }

            [TestMethod]
            public void Host_name_should_match_expected()
            {
                m_address.HostName.IsEqual(ExpectedHostName).ShouldBeTrue();
            }

            protected abstract byte[] GetExpectedAddressBytes();

            protected override void BecauseOf()
            {
                m_result = IPAddress.TryParse(AddressText, out m_address);
            }
        }

        [TestClass]
        public sealed class When_I_try_to_parse_valid_IP_address_1 : TryParseTestBase
        {
            protected override string AddressText
            {
                get { return "127.0.0.1"; }
            }

            protected override bool ExpectedResult
            {
                get { return true; }
            }

            protected override string ExpectedString
            {
                get { return "127.0.0.1"; }
            }

            protected override uint ExpectedValue
            {
                get { return 0x7f000001; }
            }

            protected override HostName ExpectedHostName
            {
                get { return new HostName("127.0.0.1"); }
            }

            protected override byte[] GetExpectedAddressBytes()
            {
                return new byte[] { 127, 0, 0, 1 };
            }
        }

        [TestClass]
        public sealed class When_I_try_to_parse_valid_IP_address_2 : TryParseTestBase
        {
            protected override string AddressText
            {
                get { return "255.254.253"; }
            }

            protected override bool ExpectedResult
            {
                get { return true; }
            }

            protected override string ExpectedString
            {
                get { return "255.254.0.253"; }
            }

            protected override uint ExpectedValue
            {
                get { return 0xfffe00fd; }
            }

            protected override HostName ExpectedHostName
            {
                get { return new HostName("255.254.0.253"); }
            }

            protected override byte[] GetExpectedAddressBytes()
            {
                return new byte[] { 255, 254, 0, 253 };
            }
        }

        [TestClass]
        public sealed class When_I_try_to_parse_valid_IP_address_3 : TryParseTestBase
        {
            protected override string AddressText
            {
                get { return "11.22.00.0"; }
            }

            protected override bool ExpectedResult
            {
                get { return true; }
            }

            protected override string ExpectedString
            {
                get { return "11.22.0.0"; }
            }

            protected override uint ExpectedValue
            {
                get { return 0x0B160000; }
            }

            protected override HostName ExpectedHostName
            {
                get { return new HostName("11.22.0.0"); }
            }

            protected override byte[] GetExpectedAddressBytes()
            {
                return new byte[] { 11, 22, 0, 0 };
            }
        }
    }
}
