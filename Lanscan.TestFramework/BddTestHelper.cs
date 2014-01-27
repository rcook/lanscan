//////////////////////////////////////////////////////////////////////
//
// Lanscan
// Copyright (C) 2013, Richard Cook. All rights reserved.
//
//////////////////////////////////////////////////////////////////////

namespace Lanscan.TestFramework
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    public static class BddTestHelper
    {
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public static Exception Catch(Action action)
        {
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            try
            {
                action();
                return null;
            }
            catch (Exception ex)
            {
                return ex;
            }
        }
    }
}
