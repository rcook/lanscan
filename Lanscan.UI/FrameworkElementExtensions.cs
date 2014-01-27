//////////////////////////////////////////////////////////////////////
//
// Lanscan
// Copyright (C) 2013, Richard Cook. All rights reserved.
//
//////////////////////////////////////////////////////////////////////

namespace Lanscan.UI
{
    using System;
    using Windows.Foundation;
    using Windows.UI.Xaml;

    public static class FrameworkElementExtensions
    {
        public static Rect GetRect(this FrameworkElement element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            var transform = element.TransformToVisual(null);
            var point = transform.TransformPoint(new Point());
            var size = new Size(element.ActualWidth, element.ActualHeight);
            var result = new Rect(point, size);
            return result;
        }
    }
}
