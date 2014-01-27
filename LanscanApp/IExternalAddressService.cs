//////////////////////////////////////////////////////////////////////
//
// Lanscan
// Copyright (C) 2013, Richard Cook. All rights reserved.
//
//////////////////////////////////////////////////////////////////////

namespace LanscanApp
{
    using System.Threading.Tasks;
    using Lanscan.Networking;

    public interface IExternalAddressService
    {
        Task<IPAddress> GetCurrentExternalAddressAsync();
    }
}
