namespace SerializationTest.UnitTests
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    public static class TestHelper
    {
        public static void AssertSequencesAreEqual<T>(IEnumerable<T> expected, IEnumerable<T> actual)
        {
            var expectedArray = expected.ToArray();
            var actualArray = actual.ToArray();

            Assert.AreEqual(expectedArray.Length, actualArray.Length);
            for (var i = 0; i < expectedArray.Length; ++i)
            {
                Assert.AreEqual(expectedArray[i], actualArray[i]);
            }
        }
    }
}
