//////////////////////////////////////////////////////////////////////
//
// Lanscan
// Copyright (C) 2013, Richard Cook. All rights reserved.
//
//////////////////////////////////////////////////////////////////////

namespace Lanscan.Utilities
{
    using System;
    using Windows.ApplicationModel.Core;
    using Windows.Foundation;
    using Windows.UI.Core;

    // Based on:
    // http://caliburnmicro.codeplex.com/SourceControl/changeset/view/3abbd65b0fe0#src/Caliburn.Micro.Silverlight/INPC.cs
    public static class Execute
    {
        public static IAsyncAction OnUIThread(DispatchedHandler action)
        {
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            var result = CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, action);
            return result;
        }
    }
}
