//////////////////////////////////////////////////////////////////////
//
// Lanscan
// Copyright (C) 2013, Richard Cook. All rights reserved.
//
//////////////////////////////////////////////////////////////////////

namespace LanscanApp
{
    using System.Diagnostics.CodeAnalysis;
    using System.Windows.Input;
    using Lanscan.Utilities;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;

    public sealed partial class NetworkMapControl : UserControl
    {
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly DependencyProperty AvailableServicesProperty = DependencyProperty.Register(
            "AvailableServices",
            typeof(SortedObservableCollection<ScanNetworkResult>),
            typeof(NetworkMapControl),
            null);

        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly DependencyProperty LaunchUriCommandProperty = DependencyProperty.Register(
            "LaunchUriCommand",
            typeof(ICommand),
            typeof(NetworkMapControl),
            null);

        public NetworkMapControl()
        {
            InitializeComponent();
        }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public SortedObservableCollection<ScanNetworkResult> AvailableServices
        {
            get { return (SortedObservableCollection<ScanNetworkResult>)GetValue(AvailableServicesProperty); }
            set { SetValue(AvailableServicesProperty, value); }
        }

        public ICommand LaunchUriCommand
        {
            get { return (ICommand)GetValue(LaunchUriCommandProperty); }
            set { SetValue(LaunchUriCommandProperty, value); }
        }
    }
}
