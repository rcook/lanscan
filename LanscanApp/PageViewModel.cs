//////////////////////////////////////////////////////////////////////
//
// Lanscan
// Copyright (C) 2013, Richard Cook. All rights reserved.
//
//////////////////////////////////////////////////////////////////////

namespace LanscanApp
{
    using System;
    using Lanscan.Utilities;
    using IServiceProvider = Lanscan.Utilities.IServiceProvider;

    public abstract class PageViewModel : ServiceProviderViewModel
    {
        private readonly AppViewModel m_appViewModel;

        protected PageViewModel(IServiceProvider serviceProvider, AppViewModel appViewModel)
            : base(serviceProvider)
        {
            if (appViewModel == null)
            {
                throw new ArgumentNullException("appViewModel");
            }

            m_appViewModel = appViewModel;
        }

        protected AppViewModel AppViewModel
        {
            get { return m_appViewModel; }
        }
    }
}
