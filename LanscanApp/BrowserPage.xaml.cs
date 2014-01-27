//////////////////////////////////////////////////////////////////////
//
// Lanscan
// Copyright (C) 2013, Richard Cook. All rights reserved.
//
//////////////////////////////////////////////////////////////////////

namespace LanscanApp
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using Lanscan.Networking;
    using LanscanApp.Common;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Navigation;

    // TODO: Create a proper view model for all of this...
    public sealed partial class BrowserPage : LayoutAwarePage
    {
        public static readonly DependencyProperty CurrentUriProperty = DependencyProperty.Register(
            "CurrentUri",
            typeof(Uri),
            typeof(BrowserPage),
            null);

        public BrowserPage()
        {
            InitializeComponent();
            WebView.LoadCompleted += WebView_LoadCompleted;
            WebView.NavigationFailed += WebView_NavigationFailed;
        }

        public Uri CurrentUri
        {
            get { return (Uri)GetValue(CurrentUriProperty); }
            set { SetValue(CurrentUriProperty, value); }
        }

        protected override void LoadState(object navigationParameter, Dictionary<string, object> pageState)
        {
            if (navigationParameter == null)
            {
                throw new ArgumentNullException("navigationParameter");
            }

            var args = (BrowserPageArgs)navigationParameter;
            var targetUri = GetUri(args.Endpoint);
            WebView.Navigate(targetUri);
        }

        protected override void SaveState(Dictionary<string, object> pageState)
        {
        }

        private static Uri GetUri(IPEndpoint endpoint)
        {
            var uriScheme = endpoint.Port == 443 ? "https" : "http";
            var uriString = String.Format(CultureInfo.InvariantCulture, "{0}://{1}:{2}", uriScheme, endpoint.Address, endpoint.Port);
            var result = new Uri(uriString);
            return result;
        }

        // TODO: Move over to ICommand style.
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            WebView.InvokeScript("eval", new[] { "history.back();" });
        }

        private void WebView_LoadCompleted(object sender, NavigationEventArgs e)
        {
            ProgressRing.IsActive = false;
            ProgressRing.Visibility = Visibility.Collapsed;
            WebView.Visibility = Visibility.Visible;
            CurrentUri = e.Uri;
        }

        private void WebView_NavigationFailed(object sender, WebViewNavigationFailedEventArgs e)
        {
            ProgressRing.IsActive = false;
            ProgressRing.Visibility = Visibility.Collapsed;
            WatermarkTextBlock.Visibility = Visibility.Visible;
        }
    }
}
