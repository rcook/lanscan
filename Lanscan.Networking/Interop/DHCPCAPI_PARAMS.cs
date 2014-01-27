//////////////////////////////////////////////////////////////////////
//
// Lanscan
// Copyright (C) 2013, Richard Cook. All rights reserved.
//
//////////////////////////////////////////////////////////////////////

namespace Lanscan.Networking.Interop
{
    using System;
    using System.Runtime.InteropServices;

    public struct DHCPCAPI_PARAMS
    {
        public uint Flags;
        public OPTION OptionId;
        [MarshalAs(UnmanagedType.Bool)]
        public bool IsVendor;
        public IntPtr Data;
        public uint nBytesData;
    }
}
