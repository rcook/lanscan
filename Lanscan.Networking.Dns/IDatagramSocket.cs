//////////////////////////////////////////////////////////////////////
//
// Lanscan
// Copyright (C) 2013, Richard Cook. All rights reserved.
//
//////////////////////////////////////////////////////////////////////

namespace Lanscan.Networking.Dns
{
    using System;

    public interface IDatagramSocket : IDisposable
    {
        void SendTo(byte[] data, IPEndpoint endpoint);
        int Receive(byte[] buffer);
    }
}
