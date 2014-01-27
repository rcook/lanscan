//////////////////////////////////////////////////////////////////////
//
// Lanscan
// Copyright (C) 2013, Richard Cook. All rights reserved.
//
//////////////////////////////////////////////////////////////////////

namespace LanscanApp
{
    using System;
    using Lanscan.Networking;

    public interface IDhcpInfoService
    {
        DhcpInfo GetDhcpInfo(Guid networkAdapterId);
    }
}
