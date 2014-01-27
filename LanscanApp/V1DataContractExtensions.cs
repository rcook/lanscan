//////////////////////////////////////////////////////////////////////
//
// Lanscan
// Copyright (C) 2013, Richard Cook. All rights reserved.
//
//////////////////////////////////////////////////////////////////////

namespace LanscanApp
{
    using System;
    using Lanscan.DataContracts;
    using Lanscan.Networking;

    public static class V1DataContractExtensions
    {
        public static Protocol Convert(this V1Protocol value)
        {
            switch (value)
            {
                case V1Protocol.Tcp: return Protocol.Tcp;
                case V1Protocol.Udp: return Protocol.Udp;
                default: throw new ArgumentException("Invalid protocol", "protocol");
            }
        }

        public static V1Protocol Convert(this Protocol value)
        {
            switch (value)
            {
                case Protocol.Tcp: return V1Protocol.Tcp;
                case Protocol.Udp: return V1Protocol.Udp;
                default: throw new ArgumentException("Invalid protocol", "protocol");
            }
        }
    }
}
