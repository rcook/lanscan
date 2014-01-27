//////////////////////////////////////////////////////////////////////
//
// Lanscan
// Copyright (C) 2013, Richard Cook. All rights reserved.
//
//////////////////////////////////////////////////////////////////////

namespace Lanscan.UI
{
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
    using WinRtBehaviors;

    // Based on:
    // http://blog.functionalfun.net/2009/02/how-to-databind-to-selecteditems.html
    // http://www.codeproject.com/Articles/412417/Managing-Multiple-selection-in-View-Model-NET-Metr
    public sealed class MultiselectBehaviour : Behavior<ListViewBase>
    {
        private bool m_selectionChangedInProgress;

        public static readonly DependencyProperty SelectedItemsProperty = DependencyProperty.Register(
            "SelectedItems",
            typeof(ObservableCollection<object>),
            typeof(MultiselectBehaviour),
            new PropertyMetadata(new ObservableCollection<object>(), OnSelectedItemsPropertyChanged));

        public MultiselectBehaviour()
        {
        }

        public ObservableCollection<object> SelectedItems
        {
            get { return (ObservableCollection<object>)GetValue(SelectedItemsProperty); }
            set { SetValue(SelectedItemsProperty, value); }
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.SelectionChanged += OnSelectionChanged;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.SelectionChanged -= OnSelectionChanged;
        }

        private static void OnSelectedItemsPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            var oldValueCollection = args.OldValue as ObservableCollection<object>;
            if (oldValueCollection != null)
            {
                oldValueCollection.CollectionChanged -= OnSelectedItemsChanged;
            }

            var newValueCollection = args.NewValue as ObservableCollection<object>;
            if (newValueCollection != null)
            {
                newValueCollection.CollectionChanged += OnSelectedItemsChanged;
            }
        }

        private static void OnSelectedItemsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            var self = sender as MultiselectBehaviour;
            if (self != null)
            {
                self.OnSelectedItemsChanged(e);
            }
        }

        private void OnSelectedItemsChanged(NotifyCollectionChangedEventArgs e)
        {
            var selectedItems = AssociatedObject.SelectedItems;

            if (e.OldItems != null)
            {
                foreach (var item in e.OldItems)
                {
                    if (selectedItems.Contains(item))
                    {
                        selectedItems.Remove(item);
                    }
                }
            }

            if (e.NewItems != null)
            {
                foreach (var item in e.NewItems)
                {
                    if (!selectedItems.Contains(item))
                    {
                        selectedItems.Add(item);
                    }
                }
            }
        }

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (m_selectionChangedInProgress)
            {
                return;
            }

            try
            {
                m_selectionChangedInProgress = true;

                foreach (var item in e.RemovedItems)
                {
                    if (SelectedItems.Contains(item))
                    {
                        SelectedItems.Remove(item);
                    }
                }

                foreach (var item in e.AddedItems)
                {
                    if (!SelectedItems.Contains(item))
                    {
                        SelectedItems.Add(item);
                    }
                }
            }
            finally
            {
                m_selectionChangedInProgress = false;
            }
        }
    }
}
