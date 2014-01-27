//////////////////////////////////////////////////////////////////////
//
// Lanscan
// Copyright (C) 2013, Richard Cook. All rights reserved.
//
//////////////////////////////////////////////////////////////////////

namespace Lanscan.Utilities
{
    using System;

    // Can we use Windows.UI.Xaml.UnhandledExceptionEventArgs instead?
    public class UnhandledExceptionEventArgs : EventArgs
    {
        public bool Handled { get; set; }
        public Exception Exception { get; set; }
    }
}
