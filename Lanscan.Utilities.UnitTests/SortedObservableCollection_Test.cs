//////////////////////////////////////////////////////////////////////
//
// Lanscan
// Copyright (C) 2013, Richard Cook. All rights reserved.
//
//////////////////////////////////////////////////////////////////////

namespace Lanscan.Utilities.UnitTests
{
    using System;
    using System.Collections.Generic;
    using Lanscan.TestFramework;
    using Lanscan.Utilities;
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

    public static class On_SortedObservableCollection
    {
        [TestClass]
        public sealed class TestCase
        {
            [TestMethod]
            public void TestAdd()
            {
                var collection = new SortedObservableCollection<int>(Comparer<int>.Default);
                collection.ShouldEqual(new int[0]);

                collection.Add(5);
                collection.ShouldEqual(new[] { 5 });

                collection.Add(4);
                collection.ShouldEqual(new[] { 4, 5 });

                collection.Add(3);
                collection.ShouldEqual(new[] { 3, 4, 5 });

                collection.Add(10);
                collection.ShouldEqual(new[] { 3, 4, 5, 10 });

                var exception = BddTestHelper.Catch(() => { collection.Add(10); });
                exception.ShouldNotBeNull();
                exception.ShouldBeOfType<ArgumentException>();
                collection.ShouldEqual(new[] { 3, 4, 5, 10 });
            }
        }
    }
}
