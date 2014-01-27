//////////////////////////////////////////////////////////////////////
//
// Lanscan
// Copyright (C) 2013, Richard Cook. All rights reserved.
//
//////////////////////////////////////////////////////////////////////

namespace Lanscan.Utilities
{
    using System;

    public sealed class DictionaryPropertyDescriptor<T> : PropertyDescriptor<T>
    {
        public DictionaryPropertyDescriptor(IPropertyStore propertyStore, string propertyName, Func<T> defaultValueGetter, Action changedAction = null)
            : base(propertyStore, propertyName, defaultValueGetter, changedAction)
        {
        }
    }
}
