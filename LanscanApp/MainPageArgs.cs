//////////////////////////////////////////////////////////////////////
//
// Lanscan
// Copyright (C) 2013, Richard Cook. All rights reserved.
//
//////////////////////////////////////////////////////////////////////

namespace LanscanApp
{
    using System;
    using IServiceProvider = Lanscan.Utilities.IServiceProvider;

    public sealed class MainPageArgs : PageArgs
    {
        private readonly string m_arguments;

        public MainPageArgs(IServiceProvider serviceProvider, AppViewModel appViewModel, string arguments)
            : base(serviceProvider, appViewModel)
        {
            if (arguments == null)
            {
                throw new ArgumentNullException("arguments");
            }

            m_arguments = arguments;
        }

        public string Arguments
        {
            get { return m_arguments; }
        }
    }
}
