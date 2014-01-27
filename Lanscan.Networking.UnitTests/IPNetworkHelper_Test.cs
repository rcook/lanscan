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

    public static class On_IPNetworkHelper
    {
        [TestClass]
        public abstract class IsValidNetworkPrefixLengthTestBase : BddTestBase
        {
            private bool m_result;

            [TestMethod]
            public void Result_should_match_expected()
            {
                m_result.ShouldEqual(ExpectedResult);
            }

            protected abstract byte Value { get; }

            protected abstract bool ExpectedResult { get; }

            protected override void BecauseOf()
            {
                m_result = IPNetworkHelper.IsValidNetworkPrefixLength(Value);
            }
        }

        [TestClass]
        public sealed class When_I_pass_zero_to_IsValidNetworkPrefixLength : IsValidNetworkPrefixLengthTestBase
        {
            protected override byte Value
            {
                get { return 0; }
            }

            protected override bool ExpectedResult
            {
                get { return true; }
            }
        }

        [TestClass]
        public sealed class When_I_pass_one_to_IsValidNetworkPrefixLength : IsValidNetworkPrefixLengthTestBase
        {
            protected override byte Value
            {
                get { return 1; }
            }

            protected override bool ExpectedResult
            {
                get { return true; }
            }
        }

        [TestClass]
        public sealed class When_I_pass_five_to_IsValidNetworkPrefixLength : IsValidNetworkPrefixLengthTestBase
        {
            protected override byte Value
            {
                get { return 5; }
            }

            protected override bool ExpectedResult
            {
                get { return true; }
            }
        }

        [TestClass]
        public sealed class When_I_pass_thirty_two_to_IsValidNetworkPrefixLength : IsValidNetworkPrefixLengthTestBase
        {
            protected override byte Value
            {
                get { return 32; }
            }

            protected override bool ExpectedResult
            {
                get { return true; }
            }
        }

        [TestClass]
        public sealed class When_I_pass_thirty_three_to_IsValidNetworkPrefixLength : IsValidNetworkPrefixLengthTestBase
        {
            protected override byte Value
            {
                get { return 33; }
            }

            protected override bool ExpectedResult
            {
                get { return false; }
            }
        }

        [TestClass]
        public sealed class When_I_pass_invalid_prefix_length_to_GetNetworkMaskValue : BddTestBase
        {
            private Exception m_exception;

            [TestMethod]
            public void Exception_should_be_of_expected_type()
            {
                m_exception.ShouldBeOfType<ArgumentOutOfRangeException>();
            }

            protected override void BecauseOf()
            {
                m_exception = Catch(() => { IPNetworkHelper.GetNetworkMaskValue(33); });
            }
        }

        [TestClass]
        public abstract class GetNetworkMaskValueTestBase : BddTestBase
        {
            private uint m_result;

            [TestMethod]
            public void Result_should_match_expected()
            {
                m_result.ShouldEqual(ExpectedResult);
            }

            protected abstract byte PrefixLength { get; }

            protected abstract uint ExpectedResult { get; }

            protected override void BecauseOf()
            {
                m_result = IPNetworkHelper.GetNetworkMaskValue(PrefixLength);
            }
        }

        [TestClass]
        public sealed class When_I_pass_prefix_length_of_0 : GetNetworkMaskValueTestBase
        {
            protected override byte PrefixLength
            {
                get { return 0; }
            }

            protected override uint ExpectedResult
            {
                get { return 0x00000000; }
            }
        }

        [TestClass]
        public sealed class When_I_pass_prefix_length_of_5 : GetNetworkMaskValueTestBase
        {
            protected override byte PrefixLength
            {
                get { return 5; }
            }

            protected override uint ExpectedResult
            {
                get { return 0xf8000000; }
            }
        }

        [TestClass]
        public sealed class When_I_pass_prefix_length_of_22 : GetNetworkMaskValueTestBase
        {
            protected override byte PrefixLength
            {
                get { return 22; }
            }

            protected override uint ExpectedResult
            {
                get { return 0xfffffc00; }
            }
        }

        [TestClass]
        public sealed class When_I_pass_prefix_length_of_24 : GetNetworkMaskValueTestBase
        {
            protected override byte PrefixLength
            {
                get { return 24; }
            }

            protected override uint ExpectedResult
            {
                get { return 0xffffff00; }
            }
        }

        [TestClass]
        public sealed class When_I_pass_prefix_length_of_32 : GetNetworkMaskValueTestBase
        {
            protected override byte PrefixLength
            {
                get { return 32; }
            }

            protected override uint ExpectedResult
            {
                get { return 0xffffffff; }
            }
        }
    }
}
