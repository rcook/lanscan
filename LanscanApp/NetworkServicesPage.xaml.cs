//////////////////////////////////////////////////////////////////////
//
// Lanscan
// Copyright (C) 2013, Richard Cook. All rights reserved.
//
//////////////////////////////////////////////////////////////////////

namespace LanscanApp
{
    using System;
    using System.Collections.Generic;
    using LanscanApp.Common;

    public sealed partial class NetworkServicesPage : LayoutAwarePage
    {
        public NetworkServicesPage()
        {
            InitializeComponent();
        }

        protected override void LoadState(object navigationParameter, Dictionary<string, object> pageState)
        {
            if (navigationParameter == null)
            {
                throw new ArgumentNullException("navigationParameter");
            }

            var args = (NetworkServicesPageArgs)navigationParameter;
            DataContext = CreateViewModel(args);
        }

        protected override void SaveState(Dictionary<string, object> pageState)
        {
        }

        private static NetworkServicesPageViewModel CreateViewModel(NetworkServicesPageArgs args)
        {
            var serviceProvider = args.ServiceProvider;
            var appViewModel = args.AppViewModel;
            var result = new NetworkServicesPageViewModel(serviceProvider, appViewModel);
            return result;
        }

        /*
                // http://answers.flyppdevportal.com/categories/metro/csharpvb.aspx?ID=59d8fad5-5e5a-49b2-b912-38d780474d7c
                private static void ScrollIntoViewHelper(ScrollViewer scrollViewer, ListView listView)
                {
                    // Get current list view selection (if any).
                    var selectedItem = listView.SelectedItem;
                    if (selectedItem != null)
                    {
                        // Get generated content for item.
                        var selectedListItem = (ListViewItem)listView.ItemContainerGenerator.ContainerFromItem(selectedItem);
                        if (selectedListItem != null)
                        {
                            // Force calculation of metrics within scroll view
                            // (including list view and items).
                            scrollViewer.UpdateLayout();

                            // Get location of item within scroll viewer.
                            var offsetTransform = selectedListItem.TransformToVisual(listView);
                            var offset = offsetTransform.TransformPoint(new Point(0, 0));

                            // Scroll down or up to item.
                            scrollViewer.ScrollToVerticalOffset(offset.Y);
                        }
                    }
                }
        */
    }
}
