//////////////////////////////////////////////////////////////////////
//
// Lanscan
// Copyright (C) 2013, Richard Cook. All rights reserved.
//
//////////////////////////////////////////////////////////////////////

namespace Lanscan.Networking.Dns
{
    using System;
    using System.IO;

    public interface IStreamSocket : IDisposable
    {
        bool Connected { get; }
        IAsyncResult BeginConnect(IPAddress address, ushort port, AsyncCallback requestCallback, object state);
        Stream GetSendStream();
        Stream GetReceiveStream();
    }
}
