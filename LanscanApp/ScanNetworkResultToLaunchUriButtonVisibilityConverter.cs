//////////////////////////////////////////////////////////////////////
//
// Lanscan
// Copyright (C) 2013, Richard Cook. All rights reserved.
//
//////////////////////////////////////////////////////////////////////

namespace LanscanApp
{
    using System;
    using Lanscan.Networking;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Data;

    public sealed class ScanNetworkResultToLaunchUriButtonVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var service = value as ScanNetworkResult;
            if (service != null)
            {
                if (service.Protocol != Protocol.Tcp)
                {
                    return Visibility.Collapsed;
                }
                return service.Endpoint.Port == 80 || service.Endpoint.Port == 443 ? Visibility.Visible : Visibility.Collapsed;
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
