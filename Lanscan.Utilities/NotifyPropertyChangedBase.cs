//////////////////////////////////////////////////////////////////////
//
// Lanscan
// Copyright (C) 2013, Richard Cook. All rights reserved.
//
//////////////////////////////////////////////////////////////////////

namespace Lanscan.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq.Expressions;
    using System.Runtime.CompilerServices;

    public abstract class NotifyPropertyChangedBase : INotifyPropertyChanged
    {
        private readonly Dictionary<string, PropertyChangedEventArgs> m_propertyChangedEventArgs = new Dictionary<string, PropertyChangedEventArgs>();

        protected NotifyPropertyChangedBase()
        {
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        protected void NotifyPropertyChanged<T>(Expression<Func<T>> propertySelector)
        {
            if (propertySelector == null)
            {
                throw new ArgumentNullException("propertySelector");
            }

            var propertyName = BindingHelper.GetPropertyName(propertySelector);
            NotifyPropertyChanged(propertyName);
        }

        [SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed")]
        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (String.IsNullOrEmpty(propertyName))
            {
                throw new ArgumentException("Invalid property name", "propertyName");
            }

            PropertyChangedEventArgs e;
            if (!m_propertyChangedEventArgs.TryGetValue(propertyName, out e))
            {
                e = new PropertyChangedEventArgs(propertyName);
                m_propertyChangedEventArgs[propertyName] = e;
            }

            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }
    }
}
