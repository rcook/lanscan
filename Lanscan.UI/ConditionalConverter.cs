//////////////////////////////////////////////////////////////////////
//
// Lanscan
// Copyright (C) 2013, Richard Cook. All rights reserved.
//
//////////////////////////////////////////////////////////////////////

namespace Lanscan.UI
{
    using System;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Data;

    public sealed class ConditionalConverter : IValueConverter
    {
        public ConditionalConverter()
        {
        }

        public object TrueValue { get; set; }

        public object FalseValue { get; set; }

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == DependencyProperty.UnsetValue)
            {
                return FalseValue;
            }

            if (!(value is bool))
            {
                throw new ArgumentException("Invalid value", "value");
            }

            var result = (bool)value ? TrueValue : FalseValue;
            return result;
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, string language)
        {
            var result = Object.Equals(TrueValue, value);
            return result;
        }
    }
}
