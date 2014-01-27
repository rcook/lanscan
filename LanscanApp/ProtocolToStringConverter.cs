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
    using Windows.UI.Xaml.Data;

    // TODO: This class is broken is so many ways:
    // 1. It does not check source or target types.
    // 2. It does not respect the passed-in language.
    public sealed class ProtocolToStringConverter : IValueConverter
    {
        public ProtocolToStringConverter()
        {
        }

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var protocol = (Protocol)value;
            switch (protocol)
            {
                case Protocol.Tcp: return Strings.ProtocolToStringConverter_Tcp;
                case Protocol.Udp: return Strings.ProtocolToStringConverter_Udp;
                default: return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new System.NotImplementedException();
        }
    }
}
