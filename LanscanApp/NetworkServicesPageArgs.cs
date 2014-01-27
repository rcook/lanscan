//////////////////////////////////////////////////////////////////////
//
// Lanscan
// Copyright (C) 2013, Richard Cook. All rights reserved.
//
//////////////////////////////////////////////////////////////////////

namespace LanscanApp
{
    using IServiceProvider = Lanscan.Utilities.IServiceProvider;

    public sealed class NetworkServicesPageArgs : PageArgs
    {
        public NetworkServicesPageArgs(IServiceProvider serviceProvider, AppViewModel appViewModel)
            : base(serviceProvider, appViewModel)
        {
        }
    }
}
