//////////////////////////////////////////////////////////////////////
//
// Lanscan
// Copyright (C) 2013, Richard Cook. All rights reserved.
//
//////////////////////////////////////////////////////////////////////

namespace LanscanApp
{
    using System;

    public static class Constants
    {
        private static readonly Uri PrivacyPolicyUri1 = new Uri("http://lanscan.rcook.org/privacy");

        public const ushort DefaultDnsPort = 53;

        public static Uri PrivacyPolicyUri
        {
            get { return PrivacyPolicyUri1; }
        }
    }
}
