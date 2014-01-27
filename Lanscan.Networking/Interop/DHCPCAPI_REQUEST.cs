//////////////////////////////////////////////////////////////////////
//
// Lanscan
// Copyright (C) 2013, Richard Cook. All rights reserved.
//
//////////////////////////////////////////////////////////////////////

namespace Lanscan.Networking.Interop
{
    using System;

    [Flags]
    public enum DHCPCAPI_REQUEST : uint
    {
        PERSISTENT = 1u,
        SYNCHRONOUS = 2u,
        ASYNCHRONOUS = 4u,
        CANCEL = 8u,
        MASK = 15u
    }
}
