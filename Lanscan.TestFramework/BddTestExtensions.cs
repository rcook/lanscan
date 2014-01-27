//////////////////////////////////////////////////////////////////////
//
// Lanscan
// Copyright (C) 2013, Richard Cook. All rights reserved.
//
//////////////////////////////////////////////////////////////////////

namespace Lanscan.TestFramework
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Linq;
#if NETFX_CORE
    using System.Reflection;
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
#else
    using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

    public static class BddTestExtensions
    {
        public static void ShouldBeOfType<T>(this object actual)
        {
            if (actual == null)
            {
                throw new ArgumentNullException("actual");
            }

            actual.GetType().ShouldEqual(typeof(T));
        }

        public static void ShouldBeOfType(this object actual, Type type)
        {
            if (actual == null)
            {
                throw new ArgumentNullException("actual");
            }
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            actual.GetType().ShouldEqual(type);
        }

        public static void ShouldBeSameAs(this object actual, object expected)
        {
            Assert.AreSame(expected, actual);
        }

        public static void ShouldBeEmpty<T>(this IEnumerable<T> collection)
        {
            var items = collection.ToArray();
            Assert.IsTrue(items.Length == 0);
        }

        [SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", MessageId = "Microsoft.VisualStudio.TestPlatform.UnitTestFramework.Assert.IsTrue(System.Boolean,System.String)")]
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public static void ShouldBeThrownBy(this Type type, Action action)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            var caughtExpectedExceptionType = false;
            try
            {
                action();
            }
            catch (Exception ex)
            {
#if NETFX_CORE
                caughtExpectedExceptionType = type.GetTypeInfo().IsAssignableFrom(ex.GetType().GetTypeInfo());
#else
                caughtExpectedExceptionType = type.IsAssignableFrom(ex.GetType());
#endif
            }
            Assert.IsTrue(
                caughtExpectedExceptionType,
                String.Format(CultureInfo.CurrentCulture, "Code block did not throw exception of expected type \"{0}\"", type.FullName));
        }

        public static void ShouldBeNull(this object value)
        {
            Assert.IsNull(value);
        }

        public static void ShouldNotBeNull(this object value)
        {
            Assert.IsNotNull(value);
        }

        public static void ShouldBeTrue(this bool condition)
        {
            Assert.IsTrue(condition);
        }

        public static void ShouldBeFalse(this bool condition)
        {
            Assert.IsFalse(condition);
        }

        public static void ShouldEqual(this int actualValue, int expectedValue)
        {
            Assert.AreEqual(expectedValue, actualValue);
        }

        public static void ShouldNotEqual(this int actualValue, int expectedValue)
        {
            Assert.AreNotEqual(expectedValue, actualValue);
        }

        public static void ShouldEqual(this float actualValue, float expectedValue)
        {
            Assert.AreEqual(expectedValue, actualValue);
        }

        public static void ShouldNotEqual(this float actualValue, float expectedValue)
        {
            Assert.AreNotEqual(expectedValue, actualValue);
        }

        public static void ShouldEqual(this double actualValue, double expectedValue)
        {
            Assert.AreEqual(expectedValue, actualValue);
        }

        public static void ShouldNotEqual(this double actualValue, double expectedValue)
        {
            Assert.AreNotEqual(expectedValue, actualValue);
        }

        public static void ShouldEqual(this string actualValue, string expectedValue)
        {
            Assert.AreEqual(expectedValue, actualValue);
        }

        public static void ShouldNotEqual(this string actualValue, string expectedValue)
        {
            Assert.AreNotEqual(expectedValue, actualValue);
        }

        public static void ShouldEqual(this byte actualValue, byte expectedValue)
        {
            Assert.AreEqual(expectedValue, actualValue);
        }

        public static void ShouldEqual(this uint actualValue, uint expectedValue)
        {
            Assert.AreEqual(expectedValue, actualValue);
        }

        public static void ShouldEqual(this object actualValue, object expectedValue)
        {
            Assert.AreEqual(expectedValue, actualValue);
        }

        public static void ShouldNotEqual(this object actualValue, object expectedValue)
        {
            Assert.AreNotEqual(expectedValue, actualValue);
        }

        [SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", MessageId = "Microsoft.VisualStudio.TestPlatform.UnitTestFramework.Assert.AreEqual<T>(T,T,System.String)")]
        [SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", MessageId = "Microsoft.VisualStudio.TestPlatform.UnitTestFramework.Assert.AreEqual<System.Int32>(T,T,System.String)")]
        public static void ShouldEqual<T>(this IEnumerable<T> actualItemCollection, IEnumerable<T> expectedItemCollection, IComparer<T> comparer = null)
        {
            if (actualItemCollection == null)
            {
                throw new ArgumentNullException("actualItemCollection");
            }
            if (expectedItemCollection == null)
            {
                throw new ArgumentNullException("expectedItemCollection");
            }

            var actualItems = actualItemCollection.ToArray();
            var expectedItems = expectedItemCollection.ToArray();
            Assert.AreEqual(
                expectedItems.Length,
                actualItems.Length,
                String.Format(CultureInfo.CurrentCulture, "Actual collection has {0} elements instead of {1} expected elements", actualItems.Length, expectedItems.Length));
            if (expectedItems.Length == actualItems.Length)
            {
                for (var i = 0; i < expectedItems.Length; ++i)
                {
                    AssertAreEqual(
                        expectedItems[i],
                        actualItems[i],
                        String.Format(CultureInfo.CurrentCulture, "Value of element {0} \"{1}\" is not equal to expected value \"{2}\"", i, actualItems[i], expectedItems[i]),
                        comparer);
                }
            }
        }

        private static void AssertAreEqual<T>(T expected, T actual, string message, IComparer<T> comparer = null)
        {
            var tempComparer = comparer ?? Comparer<T>.Default;
            var result = tempComparer.Compare(expected, actual);
            Assert.AreEqual(0, result, message);
        }
    }
}
