//////////////////////////////////////////////////////////////////////
//
// Lanscan
// Copyright (C) 2013, Richard Cook. All rights reserved.
//
//////////////////////////////////////////////////////////////////////

namespace Lanscan.Utilities
{
    using System;

    public interface IPropertyDescriptor<T>
    {
        string PropertyName { get; }
        Action ChangedAction { get; }
        bool TryGetValue(out T propertyValue);
        void SetValue(T propertyValue);
        T GetDefaultValue();
    }
}
