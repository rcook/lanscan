//////////////////////////////////////////////////////////////////////
//
// Lanscan
// Copyright (C) 2013, Richard Cook. All rights reserved.
//
//////////////////////////////////////////////////////////////////////

namespace LanscanApp
{
    using Lanscan.Networking;

    public interface INetworkServiceCore
    {
        Protocol Protocol { get; }
        ushort Port { get; }
    }
}
