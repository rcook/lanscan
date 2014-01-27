//////////////////////////////////////////////////////////////////////
//
// Lanscan
// Copyright (C) 2013, Richard Cook. All rights reserved.
//
//////////////////////////////////////////////////////////////////////

namespace LanscanApp
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Lanscan.Networking;
    using Lanscan.Utilities;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
    using IServiceProvider = Lanscan.Utilities.IServiceProvider;

    public sealed class AppViewModel : ViewModel
    {
        private readonly IServiceProvider m_serviceProvider;

        public AppViewModel(IServiceProvider serviceProvider)
        {
            if (serviceProvider == null)
            {
                throw new ArgumentNullException("serviceProvider");
            }

            m_serviceProvider = serviceProvider;
        }

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        private Frame Frame
        {
            get { return (Frame)Window.Current.Content; }
        }

        [SuppressMessage("Microsoft.Usage", "CA2201:DoNotRaiseReservedExceptionTypes")]
        public void NavigateToMainPage(string arguments)
        {
            if (arguments == null)
            {
                throw new ArgumentNullException("arguments");
            }

            if (!Frame.Navigate(typeof(MainPage), new MainPageArgs(m_serviceProvider, this, arguments)))
            {
                throw new Exception("Failed to create main page");
            }
        }

        [SuppressMessage("Microsoft.Usage", "CA2201:DoNotRaiseReservedExceptionTypes")]
        public void NavigateToServicesPage()
        {
            if (!Frame.Navigate(typeof(NetworkServicesPage), new NetworkServicesPageArgs(m_serviceProvider, this)))
            {
                throw new Exception("Failed to create services page");
            }
        }

        [SuppressMessage("Microsoft.Usage", "CA2201:DoNotRaiseReservedExceptionTypes")]
        public void NavigateToBrowserPage(IPEndpoint endpoint)
        {
            if (endpoint == null)
            {
                throw new ArgumentNullException("endpoint");
            }

            if (!Frame.Navigate(typeof(BrowserPage), new BrowserPageArgs(m_serviceProvider, this, endpoint)))
            {
                throw new Exception("Failed to create browser page");
            }
        }
    }
}
