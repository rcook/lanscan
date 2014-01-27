//////////////////////////////////////////////////////////////////////
//
// Lanscan
// Copyright (C) 2013, Richard Cook. All rights reserved.
//
//////////////////////////////////////////////////////////////////////

namespace Lanscan.Networking.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Lanscan.TestFramework;
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

    public static class On_IPAddressRange
    {
        [TestClass]
        public sealed class When_I_get_empty_instance : BddTestBase
        {
            private IPAddressRange m_result;

            [TestMethod]
            public void Count_should_be_expected_value()
            {
                m_result.Count.ShouldEqual(0);
            }

            [TestMethod]
            public void First_value_should_throw_expected_exception()
            {
                Catch(() => { ForceEvaluation(m_result.FirstValue); }).ShouldBeOfType<InvalidOperationException>();
            }

            [TestMethod]
            public void Last_value_should_throw_expected_exception()
            {
                Catch(() => { ForceEvaluation(m_result.LastValue); }).ShouldBeOfType<InvalidOperationException>();
            }

            protected override void BecauseOf()
            {
                m_result = IPAddressRange.Empty;
            }

            private static void ForceEvaluation(uint value)
            {
            }
        }

        [TestClass]
        public sealed class When_I_pass_last_value_less_than_first_value_to_constructor : BddTestBase
        {
            private Exception m_exception;

            [TestMethod]
            public void Exception_should_be_of_expected_type()
            {
                m_exception.ShouldBeOfType<ArgumentOutOfRangeException>();
            }

            protected override void BecauseOf()
            {
                m_exception = Catch(() => { new IPAddressRange(1, 0); });
            }
        }

        [TestClass]
        public sealed class When_I_pass_last_address_less_than_first_address_to_constructor : BddTestBase
        {
            private Exception m_exception;

            [TestMethod]
            public void Exception_should_be_of_expected_type()
            {
                m_exception.ShouldBeOfType<ArgumentOutOfRangeException>();
            }

            protected override void BecauseOf()
            {
                m_exception = Catch(() => { new IPAddressRange(new IPAddress(1), new IPAddress(0)); });
            }
        }

        [TestClass]
        public abstract class ConstructorTestBase : BddTestBase
        {
            private IPAddressRange m_result;

            [TestMethod]
            public void Count_should_be_expected_value()
            {
                m_result.Count.ShouldEqual(ExpectedCount);
            }

            [TestMethod]
            public void First_value_should_be_expected_value()
            {
                m_result.FirstValue.ShouldEqual(ExpectedFirstValue);
            }

            [TestMethod]
            public void Last_value_should_be_expected_value()
            {
                m_result.LastValue.ShouldEqual(ExpectedLastValue);
            }

            protected abstract uint FirstValue { get; }

            protected abstract uint LastValue { get; }

            protected abstract int ExpectedCount { get; }

            protected abstract uint ExpectedFirstValue { get; }

            protected abstract uint ExpectedLastValue { get; }

            protected override void BecauseOf()
            {
                m_result = new IPAddressRange(FirstValue, LastValue);
            }
        }

        [TestClass]
        public sealed class When_I_create_valid_IPAddressRange_1 : ConstructorTestBase
        {
            protected override uint FirstValue
            {
                get { return 5; }
            }

            protected override uint LastValue
            {
                get { return 5; }
            }

            protected override int ExpectedCount
            {
                get { return 1; }
            }

            protected override uint ExpectedFirstValue
            {
                get { return 5; }
            }

            protected override uint ExpectedLastValue
            {
                get { return 5; }
            }
        }

        [TestClass]
        public sealed class When_I_create_valid_IPAddressRange_2 : ConstructorTestBase
        {
            protected override uint FirstValue
            {
                get { return 5; }
            }

            protected override uint LastValue
            {
                get { return 6; }
            }

            protected override int ExpectedCount
            {
                get { return 2; }
            }

            protected override uint ExpectedFirstValue
            {
                get { return 5; }
            }

            protected override uint ExpectedLastValue
            {
                get { return 6; }
            }
        }

        [TestClass]
        public sealed class When_I_create_valid_IPAddressRange_3 : ConstructorTestBase
        {
            protected override uint FirstValue
            {
                get { return 5; }
            }

            protected override uint LastValue
            {
                get { return 50; }
            }

            protected override int ExpectedCount
            {
                get { return 46; }
            }

            protected override uint ExpectedFirstValue
            {
                get { return 5; }
            }

            protected override uint ExpectedLastValue
            {
                get { return 50; }
            }
        }

        [TestClass]
        public sealed class When_I_create_valid_IPAddressRange_4 : ConstructorTestBase
        {
            protected override uint FirstValue
            {
                get { return 2637895681; }
            }

            protected override uint LastValue
            {
                get { return 2637896702; }
            }

            protected override int ExpectedCount
            {
                get { return 1022; }
            }

            protected override uint ExpectedFirstValue
            {
                get { return 2637895681; }
            }

            protected override uint ExpectedLastValue
            {
                get { return 2637896702; }
            }
        }

        [TestClass]
        public sealed class When_I_iterate_over_a_valid_address_range : BddTestBase
        {
            private IPAddress[] m_result;

            [TestMethod]
            public void Addresses_should_match_expected()
            {
                m_result.Select(x => x.Value).ShouldEqual(new uint[]
                {
                    0x1000,
                    0x1001,
                    0x1002,
                    0x1003,
                    0x1004,
                    0x1005
                });
            }

            protected override void BecauseOf()
            {
                m_result = new IPAddressRange(0x1000, 0x1005).ToArray();
            }
        }

        [TestClass]
        public abstract class ChunkTestBase : BddTestBase
        {
            private uint[][] m_result;

            protected abstract IPAddressRange Addresses { get; }

            protected abstract int ChunkSize { get; }

            protected abstract uint[][] GetExpectedResult();

            [TestMethod]
            public void Address_range_should_chunk_as_expected()
            {
                var expectedResult = GetExpectedResult();
                AssertChunksEqual(expectedResult, m_result);
            }

            protected override void BecauseOf()
            {
                var addressValueChunks = new List<uint[]>();
                foreach (var addressChunk in Addresses.Chunk(ChunkSize))
                {
                    var addressValues = new List<uint>();
                    foreach (var address in addressChunk)
                    {
                        addressValues.Add(address.Value);
                    }
                    addressValueChunks.Add(addressValues.ToArray());
                }
                m_result = addressValueChunks.ToArray();
            }

            private static void AssertChunksEqual(uint[][] expected, uint[][] actual)
            {
                Assert.AreEqual(expected.Length, actual.Length);

                for (var i = 0; i < expected.Length; ++i)
                {
                    Assert.AreEqual(expected[i].Length, actual[i].Length);
                    for (var j = 0; j < expected[i].Length; ++j)
                    {
                        Assert.AreEqual(expected[i][j], actual[i][j]);
                    }
                }
            }
        }

        [TestClass]
        public sealed class When_I_chunk_address_range_into_one_chunk : ChunkTestBase
        {
            protected override IPAddressRange Addresses
            {
                get { return new IPAddressRange(0, 5); }
            }

            protected override int ChunkSize
            {
                get { return 6; }
            }

            protected override uint[][] GetExpectedResult()
            {
                return new uint[][]
                {
                    new uint[] { 0, 1, 2, 3, 4, 5 }
                };
            }
        }

        [TestClass]
        public sealed class When_I_chunk_address_range_into_two_chunks : ChunkTestBase
        {
            protected override IPAddressRange Addresses
            {
                get { return new IPAddressRange(1, 11); }
            }

            protected override int ChunkSize
            {
                get { return 6; }
            }

            protected override uint[][] GetExpectedResult()
            {
                return new uint[][]
                {
                    new uint[] { 1, 2, 3, 4, 5, 6},
                    new uint[] { 7, 8, 9, 10, 11 }
                };
            }
        }

        [TestClass]
        public sealed class When_I_chunk_empty_address_range : ChunkTestBase
        {
            protected override IPAddressRange Addresses
            {
                get { return IPAddressRange.Empty; }
            }

            protected override int ChunkSize
            {
                get { return 6; }
            }

            protected override uint[][] GetExpectedResult()
            {
                return new uint[0][];
            }
        }
    }
}
