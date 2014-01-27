//////////////////////////////////////////////////////////////////////
//
// Lanscan
// Copyright (C) 2013, Richard Cook. All rights reserved.
//
//////////////////////////////////////////////////////////////////////

namespace Lanscan.UI
{
    using System;
    using Windows.UI.Xaml.Data;

    public sealed class BooleanToOpacityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return (value is bool && (bool)value) ? 1.0d : 0.0d;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return value is double && (double)value == 1.0d;
        }
    }
}
