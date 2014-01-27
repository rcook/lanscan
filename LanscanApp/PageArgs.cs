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

    public abstract class PageArgs
    {
        private readonly IServiceProvider m_serviceProvider;
        private readonly AppViewModel m_appViewModel;

        protected PageArgs(IServiceProvider serviceProvider, AppViewModel appViewModel)
        {
            if (serviceProvider == null)
            {
                throw new ArgumentNullException("serviceProvider");
            }
            if (appViewModel == null)
            {
                throw new ArgumentNullException("appViewModel");
            }

            m_serviceProvider = serviceProvider;
            m_appViewModel = appViewModel;
        }

        public IServiceProvider ServiceProvider
        {
            get { return m_serviceProvider; }
        }

        public AppViewModel AppViewModel
        {
            get { return m_appViewModel; }
        }
    }
}
