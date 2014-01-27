//////////////////////////////////////////////////////////////////////
//
// Lanscan
// Copyright (C) 2013, Richard Cook. All rights reserved.
//
//////////////////////////////////////////////////////////////////////

namespace Lanscan.Utilities
{
    using System;
    using System.Linq.Expressions;

    public static class BindingHelper
    {
        public static string GetPropertyName<T>(Expression<Func<T>> propertySelector)
        {
            if (propertySelector == null)
            {
                throw new ArgumentNullException("propertySelector");
            }

            var result = ((MemberExpression)propertySelector.Body).Member.Name;
            if (String.IsNullOrWhiteSpace(result))
            {
                throw new ArgumentException("Invalid property selector", "propertySelector");
            }

            return result;
        }
    }
}
