//////////////////////////////////////////////////////////////////////
//
// Lanscan
// Copyright (C) 2013, Richard Cook. All rights reserved.
//
//////////////////////////////////////////////////////////////////////

namespace Lanscan.Networking.Dns
{
    public interface IStreamSocketFactory
    {
        IStreamSocket CreateStreamSocket(int timeout);
    }
}
