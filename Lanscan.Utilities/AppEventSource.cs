//////////////////////////////////////////////////////////////////////
//
// Lanscan
// Copyright (C) 2013, Richard Cook. All rights reserved.
//
//////////////////////////////////////////////////////////////////////

namespace Lanscan.Utilities
{
    using System.Diagnostics.Tracing;

    public sealed class AppEventSource : EventSource
    {
        private const int DebugEventId = 1;

        private static readonly AppEventSource Instance1 = new AppEventSource();

        private AppEventSource()
        {
        }

        public static AppEventSource Instance
        {
            get { return Instance1; }
        }

        [Event(DebugEventId, Level = EventLevel.Verbose)]
        public void Debug(string message)
        {
            WriteEvent(DebugEventId, message);
        }
    }
}
