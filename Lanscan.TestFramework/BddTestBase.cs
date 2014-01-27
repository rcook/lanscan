//////////////////////////////////////////////////////////////////////
//
// Lanscan
// Copyright (C) 2013, Richard Cook. All rights reserved.
//
//////////////////////////////////////////////////////////////////////

namespace Lanscan.TestFramework
{
    using System;
#if NETFX_CORE
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
#else
    using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

    [TestClass]
    public abstract class BddTestBase
    {
        [TestInitialize]
        public void TestInitialize()
        {
            Context();
            BecauseOf();
        }

        protected static Exception Catch(Action action)
        {
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            var result = BddTestHelper.Catch(action);
            return result;
        }

        protected virtual void Context()
        {
        }

        protected virtual void BecauseOf()
        {
        }
    }
}
