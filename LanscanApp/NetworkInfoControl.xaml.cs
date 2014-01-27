//////////////////////////////////////////////////////////////////////
//
// Lanscan
// Copyright (C) 2013, Richard Cook. All rights reserved.
//
//////////////////////////////////////////////////////////////////////

namespace LanscanApp
{
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using Lanscan.Networking;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;

    public sealed partial class NetworkInfoControl : UserControl
    {
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly DependencyProperty ConnectionInfoProperty = DependencyProperty.Register(
            "ConnectionInfo",
            typeof(ConnectionInfo),
            typeof(NetworkInfoControl),
            null);

        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly DependencyProperty ConnectionInfoTaskStatusProperty = DependencyProperty.Register(
            "ConnectionInfoTaskStatus",
            typeof(TaskStatus),
            typeof(NetworkInfoControl),
            null);

        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly DependencyProperty DhcpInfoProperty = DependencyProperty.Register(
            "DhcpInfo",
            typeof(DhcpInfo),
            typeof(NetworkInfoControl),
            null);

        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly DependencyProperty DhcpInfoTaskStatusProperty = DependencyProperty.Register(
            "DhcpInfoTaskStatus",
            typeof(TaskStatus),
            typeof(NetworkInfoControl),
            null);

        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly DependencyProperty ExternalAddressProperty = DependencyProperty.Register(
            "ExternalAddress",
            typeof(IPAddress),
            typeof(NetworkInfoControl),
            null);

        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly DependencyProperty ExternalAddressTaskStatusProperty = DependencyProperty.Register(
            "ExternalAddressTaskStatus",
            typeof(TaskStatus),
            typeof(NetworkInfoControl),
            null);

        public NetworkInfoControl()
        {
            InitializeComponent();
        }

        public ConnectionInfo ConnectionInfo
        {
            get { return (ConnectionInfo)GetValue(ConnectionInfoProperty); }
            set { SetValue(ConnectionInfoProperty, value); }
        }

        public TaskStatus ConnectionInfoTaskStatus
        {
            get { return (TaskStatus)GetValue(ConnectionInfoTaskStatusProperty); }
            set { SetValue(ConnectionInfoTaskStatusProperty, value); }
        }

        public DhcpInfo DhcpInfo
        {
            get { return (DhcpInfo)GetValue(DhcpInfoProperty); }
            set { SetValue(DhcpInfoProperty, value); }
        }

        public TaskStatus DhcpInfoTaskStatus
        {
            get { return (TaskStatus)GetValue(DhcpInfoTaskStatusProperty); }
            set { SetValue(DhcpInfoTaskStatusProperty, value); }
        }

        public IPAddress ExternalAddress
        {
            get { return (IPAddress)GetValue(ExternalAddressProperty); }
            set { SetValue(ExternalAddressProperty, value); }
        }

        public TaskStatus ExternalAddressTaskStatus
        {
            get { return (TaskStatus)GetValue(ExternalAddressTaskStatusProperty); }
            set { SetValue(ExternalAddressTaskStatusProperty, value); }
        }
    }
}
