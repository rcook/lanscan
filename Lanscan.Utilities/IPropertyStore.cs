//////////////////////////////////////////////////////////////////////
//
// Lanscan
// Copyright (C) 2013, Richard Cook. All rights reserved.
//
//////////////////////////////////////////////////////////////////////

namespace Lanscan.Utilities
{
    public interface IPropertyStore
    {
        bool TryGetValue<T>(string propertyName, out T propertyValue);
        void SetValue<T>(string propertyName, T propertyValue);
    }
}
