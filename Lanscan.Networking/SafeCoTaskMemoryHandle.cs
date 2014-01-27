//////////////////////////////////////////////////////////////////////
//
// Lanscan
// Copyright (C) 2013, Richard Cook. All rights reserved.
//
//////////////////////////////////////////////////////////////////////

namespace Lanscan.Networking
{
    using System;
    using System.Runtime.InteropServices;

    public sealed class SafeCoTaskMemoryHandle : SafeHandle
    {
        public SafeCoTaskMemoryHandle(int count)
            : base(IntPtr.Zero, true)
        {
            if (count < 0)
            {
                throw new ArgumentOutOfRangeException("count");
            }

            SetHandle(Marshal.AllocCoTaskMem(count));
        }

        public override bool IsInvalid
        {
            get { return handle == IntPtr.Zero; }
        }

        protected override bool ReleaseHandle()
        {
            if (handle != IntPtr.Zero)
            {
                Marshal.FreeCoTaskMem(handle);
                handle = IntPtr.Zero;
                return true;
            }
            return false;
        }
    }
}
