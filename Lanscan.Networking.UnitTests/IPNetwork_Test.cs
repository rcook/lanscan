//////////////////////////////////////////////////////////////////////
//
// Lanscan
// Copyright (C) 2013, Richard Cook. All rights reserved.
//
//////////////////////////////////////////////////////////////////////

namespace Lanscan.Networking.UnitTests
{
    using System;
    using System.Linq;
    using Lanscan.TestFramework;
#if NETFX_CORE
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
#else
    using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

    public static class On_IPNetwork
    {
        [TestClass]
        public sealed class When_I_pass_a_null_network_address : BddTestBase
        {
            private Exception m_exception;

            [TestMethod]
            public void Exception_should_be_of_expected_type()
            {
                m_exception.ShouldBeOfType<ArgumentNullException>();
            }

            protected override void BecauseOf()
            {
                m_exception = Catch(() => { new IPNetwork(null, 33); });
            }
        }

        [TestClass]
        public sealed class When_I_pass_a_valid_unnormalized_network_address : BddTestBase
        {
            private IPNetwork m_result;

            [TestMethod]
            public void String_should_match_expected()
            {
                m_result.ToString().ShouldEqual("17.34.51.0/24");
            }

            [TestMethod]
            public void Network_address_value_should_be_expected_value()
            {
                m_result.NetworkAddress.Value.ShouldEqual(0x11223300);
            }

            [TestMethod]
            public void Prefix_length_should_be_expected_value()
            {
                m_result.PrefixLength.ShouldEqual(24);
            }

            protected override void BecauseOf()
            {
                m_result = new IPNetwork(new IPAddress(0x11223344), 24);
            }
        }

        [TestClass]
        public sealed class When_I_pass_a_valid_normalized_network_address : BddTestBase
        {
            private IPNetwork m_result;

            [TestMethod]
            public void String_should_match_expected()
            {
                m_result.ToString().ShouldEqual("17.34.51.0/24");
            }

            [TestMethod]
            public void Network_address_value_should_be_expected_value()
            {
                m_result.NetworkAddress.Value.ShouldEqual(0x11223300);
            }

            [TestMethod]
            public void Prefix_length_should_be_expected_value()
            {
                m_result.PrefixLength.ShouldEqual(24);
            }

            protected override void BecauseOf()
            {
                m_result = new IPNetwork(new IPAddress(0x11223300), 24);
            }
        }

        [TestClass]
        public sealed class When_I_pass_an_invalid_prefix_length : BddTestBase
        {
            private Exception m_exception;

            [TestMethod]
            public void Exception_should_be_of_expected_type()
            {
                m_exception.ShouldBeOfType<ArgumentOutOfRangeException>();
            }

            protected override void BecauseOf()
            {
                m_exception = Catch(() => { new IPNetwork(0x11223344, 33); });
            }
        }

        [TestClass]
        public sealed class When_I_pass_a_prefix_length_of_32 : BddTestBase
        {
            private IPNetwork m_result;

            [TestMethod]
            public void String_should_match_expected()
            {
                m_result.ToString().ShouldEqual("17.34.51.68/32");
            }

            [TestMethod]
            public void Prefix_length_should_match_expected_value()
            {
                m_result.PrefixLength.ShouldEqual(32);
            }

            [TestMethod]
            public void Network_mask_should_match_expected_value()
            {
                m_result.NetworkMask.Value.ShouldEqual(0xffffffff);
            }

            [TestMethod]
            public void Network_address_value_should_match_expected_value()
            {
                m_result.NetworkAddress.Value.ShouldEqual(0x11223344);
            }

            [TestMethod]
            public void First_usable_address_should_be_null()
            {
                m_result.FirstUsableAddress.ShouldBeNull();
            }

            [TestMethod]
            public void Last_usable_address_should_be_null()
            {
                m_result.LastUsableAddress.ShouldBeNull();
            }

            [TestMethod]
            public void Broadcast_address_value_should_be_null()
            {
                m_result.BroadcastAddress.ShouldBeNull();
            }

            [TestMethod]
            public void Usable_addresses_should_be_empty()
            {
                m_result.UsableAddresses.Count.ShouldEqual(0);
            }

            [TestMethod]
            public void Usable_addresses_should_match_expected()
            {
                m_result.UsableAddresses.ToArray().ShouldEqual(new IPAddress[0]);
            }

            protected override void BecauseOf()
            {
                m_result = new IPNetwork(0x11223344, 32);
            }
        }

        [TestClass]
        public abstract class ConstructorTestBase : BddTestBase
        {
            private IPNetwork m_result;

            [TestMethod]
            public void String_should_match_expected()
            {
                m_result.ToString().ShouldEqual(ExpectedString);
            }

            [TestMethod]
            public void Prefix_length_should_match_expected_value()
            {
                m_result.PrefixLength.ShouldEqual(ExpectedPrefixLength);
            }

            [TestMethod]
            public void Network_mask_should_match_expected_value()
            {
                m_result.NetworkMask.Value.ShouldEqual(ExpectedNetworkMask);
            }

            [TestMethod]
            public void Network_address_value_should_match_expected_value()
            {
                m_result.NetworkAddress.Value.ShouldEqual(ExpectedNetworkAddressValue);
            }

            [TestMethod]
            public void First_usable_address_value_should_match_expected_value()
            {
                m_result.FirstUsableAddress.Value.ShouldEqual(ExpectedFirstUsableAddressValue);
            }

            [TestMethod]
            public void Last_usable_address_value_should_match_expected_value()
            {
                m_result.LastUsableAddress.Value.ShouldEqual(ExpectedLastUsableAddressValue);
            }

            [TestMethod]
            public void Broadcast_address_value_should_match_expected_value()
            {
                m_result.BroadcastAddress.Value.ShouldEqual(ExpectedBroadcastAddressValue);
            }

            [TestMethod]
            public void Usable_address_count_should_match_expected_value()
            {
                m_result.UsableAddresses.Count.ShouldEqual(ExpectedUsableAddressesCount);
            }

            protected IPNetwork Result
            {
                get { return m_result; }
            }

            protected abstract uint Value { get; }

            protected abstract byte PrefixLength { get; }

            protected abstract string ExpectedString { get; }

            protected abstract byte ExpectedPrefixLength { get; }

            protected abstract uint ExpectedNetworkMask { get; }

            protected abstract uint ExpectedNetworkAddressValue { get; }

            protected abstract uint ExpectedFirstUsableAddressValue { get; }

            protected abstract uint ExpectedLastUsableAddressValue { get; }

            protected abstract uint ExpectedBroadcastAddressValue { get; }

            protected abstract int ExpectedUsableAddressesCount { get; }

            protected override void BecauseOf()
            {
                m_result = new IPNetwork(Value, PrefixLength);
            }
        }

        [TestClass]
        public sealed class When_I_pass_a_prefix_length_of_29_unnormalized_value : ConstructorTestBase
        {
            [TestMethod]
            public void Usable_addresses_should_match_expected()
            {
                Result.UsableAddresses.Select(x => x.Value).ShouldEqual(new[]
                {
                    0x11223341U,
                    0x11223342U,
                    0x11223343U,
                    0x11223344U,
                    0x11223345U,
                    0x11223346U
                });
            }

            protected override uint Value
            {
                get { return 0x11223344; }
            }

            protected override byte PrefixLength
            {
                get { return 29; }
            }

            protected override string ExpectedString
            {
                get { return "17.34.51.64/29"; }
            }

            protected override byte ExpectedPrefixLength
            {
                get { return 29; }
            }

            protected override uint ExpectedNetworkMask
            {
                get { return 0xfffffff8; }
            }

            protected override uint ExpectedNetworkAddressValue
            {
                get { return 0x11223340; }
            }

            protected override uint ExpectedFirstUsableAddressValue
            {
                get { return 0x11223341; }
            }

            protected override uint ExpectedLastUsableAddressValue
            {
                get { return 0x11223346; }
            }

            protected override uint ExpectedBroadcastAddressValue
            {
                get { return 0x11223347; }
            }

            protected override int ExpectedUsableAddressesCount
            {
                get { return 6; }
            }
        }

        [TestClass]
        public sealed class When_I_pass_a_prefix_length_of_24_normalized_value : ConstructorTestBase
        {
            protected override uint Value
            {
                get { return 0x11223300; }
            }

            protected override byte PrefixLength
            {
                get { return 24; }
            }

            protected override string ExpectedString
            {
                get { return "17.34.51.0/24"; }
            }

            protected override byte ExpectedPrefixLength
            {
                get { return 24; }
            }

            protected override uint ExpectedNetworkMask
            {
                get { return 0xffffff00; }
            }

            protected override uint ExpectedNetworkAddressValue
            {
                get { return 0x11223300; }
            }

            protected override uint ExpectedFirstUsableAddressValue
            {
                get { return 0x11223301; }
            }

            protected override uint ExpectedLastUsableAddressValue
            {
                get { return 0x112233fe; }
            }

            protected override uint ExpectedBroadcastAddressValue
            {
                get { return 0x112233ff; }
            }

            protected override int ExpectedUsableAddressesCount
            {
                get { return 254; }
            }
        }

        [TestClass]
        public sealed class When_I_pass_a_prefix_length_of_24_unnormalized_value : ConstructorTestBase
        {
            protected override uint Value
            {
                get { return 0x11223344; }
            }

            protected override byte PrefixLength
            {
                get { return 24; }
            }

            protected override string ExpectedString
            {
                get { return "17.34.51.0/24"; }
            }

            protected override byte ExpectedPrefixLength
            {
                get { return 24; }
            }

            protected override uint ExpectedNetworkMask
            {
                get { return 0xffffff00; }
            }

            protected override uint ExpectedNetworkAddressValue
            {
                get { return 0x11223300; }
            }

            protected override uint ExpectedFirstUsableAddressValue
            {
                get { return 0x11223301; }
            }

            protected override uint ExpectedLastUsableAddressValue
            {
                get { return 0x112233fe; }
            }

            protected override uint ExpectedBroadcastAddressValue
            {
                get { return 0x112233ff; }
            }

            protected override int ExpectedUsableAddressesCount
            {
                get { return 254; }
            }
        }

        [TestClass]
        public sealed class When_I_pass_a_prefix_length_of_22_normalized_value : ConstructorTestBase
        {
            protected override uint Value
            {
                get { return 0x11223000; }
            }

            protected override byte PrefixLength
            {
                get { return 22; }
            }

            protected override string ExpectedString
            {
                get { return "17.34.48.0/22"; }
            }

            protected override byte ExpectedPrefixLength
            {
                get { return 22; }
            }

            protected override uint ExpectedNetworkMask
            {
                get { return 0xfffffc00; }
            }

            protected override uint ExpectedNetworkAddressValue
            {
                get { return 0x11223000; }
            }

            protected override uint ExpectedFirstUsableAddressValue
            {
                get { return 0x11223001; }
            }

            protected override uint ExpectedLastUsableAddressValue
            {
                get { return 0x112233fe; }
            }

            protected override uint ExpectedBroadcastAddressValue
            {
                get { return 0x112233ff; }
            }

            protected override int ExpectedUsableAddressesCount
            {
                get { return 1022; }
            }
        }

        [TestClass]
        public sealed class When_I_pass_a_prefix_length_of_22_unnormalized_value : ConstructorTestBase
        {
            protected override uint Value
            {
                get { return 0x11223344; }
            }

            protected override byte PrefixLength
            {
                get { return 22; }
            }

            protected override string ExpectedString
            {
                get { return "17.34.48.0/22"; }
            }

            protected override byte ExpectedPrefixLength
            {
                get { return 22; }
            }

            protected override uint ExpectedNetworkMask
            {
                get { return 0xfffffc00; }
            }

            protected override uint ExpectedNetworkAddressValue
            {
                get { return 0x11223000; }
            }

            protected override uint ExpectedFirstUsableAddressValue
            {
                get { return 0x11223001; }
            }

            protected override uint ExpectedLastUsableAddressValue
            {
                get { return 0x112233fe; }
            }

            protected override uint ExpectedBroadcastAddressValue
            {
                get { return 0x112233ff; }
            }

            protected override int ExpectedUsableAddressesCount
            {
                get { return 1022; }
            }
        }

        [TestClass]
        public sealed class When_I_pass_a_prefix_length_of_5_unnormalized_value : ConstructorTestBase
        {
            protected override uint Value
            {
                get { return 0x11223344; }
            }

            protected override byte PrefixLength
            {
                get { return 5; }
            }

            protected override string ExpectedString
            {
                get { return "16.0.0.0/5"; }
            }

            protected override byte ExpectedPrefixLength
            {
                get { return 5; }
            }

            protected override uint ExpectedNetworkMask
            {
                get { return 0xf8000000; }
            }

            protected override uint ExpectedNetworkAddressValue
            {
                get { return 0x10000000; }
            }

            protected override uint ExpectedFirstUsableAddressValue
            {
                get { return 0x10000001; }
            }

            protected override uint ExpectedLastUsableAddressValue
            {
                get { return 0x17fffffe; }
            }

            protected override uint ExpectedBroadcastAddressValue
            {
                get { return 0x17ffffff; }
            }

            protected override int ExpectedUsableAddressesCount
            {
                get { return 134217726; }
            }
        }
    }
}
