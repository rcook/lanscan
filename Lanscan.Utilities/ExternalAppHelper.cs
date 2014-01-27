//////////////////////////////////////////////////////////////////////
//
// Lanscan
// Copyright (C) 2013, Richard Cook. All rights reserved.
//
//////////////////////////////////////////////////////////////////////

namespace Lanscan.Utilities
{
    using System;
    using Windows.System;

    public static class ExternalAppHelper
    {
        public static void LaunchUri(Uri uri)
        {
            if (uri == null)
            {
                throw new ArgumentNullException("uri");
            }

#pragma warning disable 4014
            Launcher.LaunchUriAsync(uri);
#pragma warning restore 4014
        }
    }
}
