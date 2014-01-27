//////////////////////////////////////////////////////////////////////
//
// Lanscan
// Copyright (C) 2013, Richard Cook. All rights reserved.
//
//////////////////////////////////////////////////////////////////////

namespace Lanscan.TestFramework
{
    using System;
    using System.Threading;

    public static class Eventually
    {
        public static void ShouldBecomeTrue(Func<bool> condition, TimeSpan timeout)
        {
            if (!SpinWait.SpinUntil(condition, timeout))
            {
                var value = condition();
                value.ShouldBeTrue();
            }
        }

        public static void ShouldBecomeFalse(Func<bool> condition, TimeSpan timeout)
        {
            ShouldBecomeTrue(() => !condition(), timeout);
        }
    }
}
