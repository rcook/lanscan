//////////////////////////////////////////////////////////////////////
//
// Lanscan
// Copyright (C) 2013, Richard Cook. All rights reserved.
//
//////////////////////////////////////////////////////////////////////

namespace LanscanApp
{
    public interface INetworkService : INetworkServiceCore
    {
        string Name { get; }
        bool IsEnabled { get; }
    }
}
