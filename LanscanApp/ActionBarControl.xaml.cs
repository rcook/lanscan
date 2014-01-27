//////////////////////////////////////////////////////////////////////
//
// Lanscan
// Copyright (C) 2013, Richard Cook. All rights reserved.
//
//////////////////////////////////////////////////////////////////////

namespace LanscanApp
{
    using System;
    using System.Collections.ObjectModel;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using Lanscan.UI;
    using Windows.UI.Popups;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;

    public sealed partial class ActionBarControl : UserControl
    {
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly DependencyProperty IsScanInProgressProperty = DependencyProperty.Register(
            "IsScanInProgress",
            typeof(bool),
            typeof(ActionBarControl),
            new PropertyMetadata(false));

        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly DependencyProperty CustomSelectionCommandsProperty = DependencyProperty.Register(
            "CustomSelectionCommands",
            typeof(ObservableCollection<UICommand>),
            typeof(ActionBarControl),
            null);

        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly DependencyProperty IsValidAddressRangeProperty = DependencyProperty.Register(
            "IsValidAddressRange",
            typeof(bool),
            typeof(ActionBarControl),
            new PropertyMetadata(false));

        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly DependencyProperty StartAddressValueProperty = DependencyProperty.Register(
            "StartAddressValue",
            typeof(uint),
            typeof(ActionBarControl),
            new PropertyMetadata(0u));

        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly DependencyProperty IsStartAddressValidProperty = DependencyProperty.Register(
            "IsStartAddressValid",
            typeof(bool),
            typeof(ActionBarControl),
            new PropertyMetadata(false, OnAddressValidPropertyChanged));

        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly DependencyProperty EndAddressValueProperty = DependencyProperty.Register(
            "EndAddressValue",
            typeof(uint),
            typeof(ActionBarControl),
            new PropertyMetadata(0u));

        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly DependencyProperty IsEndAddressValidProperty = DependencyProperty.Register(
            "IsEndAddressValid",
            typeof(bool),
            typeof(ActionBarControl),
            new PropertyMetadata(false, OnAddressValidPropertyChanged));

        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly DependencyProperty ScanCommandProperty = DependencyProperty.Register(
            "ScanCommand",
            typeof(ICommand),
            typeof(ActionBarControl),
            null);

        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly DependencyProperty CancelScanCommandProperty = DependencyProperty.Register(
            "CancelScanCommand",
            typeof(ICommand),
            typeof(ActionBarControl),
            null);

        public ActionBarControl()
        {
            InitializeComponent();
        }

        public bool IsScanInProgress
        {
            get { return (bool)GetValue(IsScanInProgressProperty); }
            set { SetValue(IsScanInProgressProperty, value); }
        }

        public ObservableCollection<UICommand> CustomSelectionCommands
        {
            get { return (ObservableCollection<UICommand>)GetValue(CustomSelectionCommandsProperty); }
            set { SetValue(CustomSelectionCommandsProperty, value); }
        }

        public bool IsValidAddressRange
        {
            get { return (bool)GetValue(IsValidAddressRangeProperty); }
            set { SetValue(IsValidAddressRangeProperty, value); }
        }

        public uint StartAddressValue
        {
            get { return (uint)GetValue(StartAddressValueProperty); }
            set { SetValue(StartAddressValueProperty, value); }
        }

        public bool IsStartAddressValid
        {
            get { return (bool)GetValue(IsStartAddressValidProperty); }
            set { SetValue(IsStartAddressValidProperty, value); }
        }

        public uint EndAddressValue
        {
            get { return (uint)GetValue(EndAddressValueProperty); }
            set { SetValue(EndAddressValueProperty, value); }
        }

        public bool IsEndAddressValid
        {
            get { return (bool)GetValue(IsEndAddressValidProperty); }
            set { SetValue(IsEndAddressValidProperty, value); }
        }

        public ICommand ScanCommand
        {
            get { return (ICommand)GetValue(ScanCommandProperty); }
            set { SetValue(ScanCommandProperty, value); }
        }

        public ICommand CancelScanCommand
        {
            get { return (ICommand)GetValue(CancelScanCommandProperty); }
            set { SetValue(CancelScanCommandProperty, value); }
        }

        private static void OnAddressValidPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var self = (ActionBarControl)sender;
            self.OnAddressValidPropertyChanged((bool)e.OldValue, (bool)e.NewValue);
        }

        private void OnAddressValidPropertyChanged(bool oldValue, bool newValue)
        {
            IsValidAddressRange = IsStartAddressValid && IsEndAddressValid;
        }

        private async void SelectionButton_Click(object sender, RoutedEventArgs e)
        {
            var selectionButton = (Button)sender;
            var menu = new PopupMenu();
            if (CustomSelectionCommands != null)
            {
                foreach (var command in CustomSelectionCommands)
                {
                    menu.Commands.Add(command);
                }
            }
            await menu.ShowForSelectionAsync(selectionButton.GetRect());
        }

        //private async void SynchronizationContextUnhandledExceptionButton_Click(object sender, RoutedEventArgs e)
        //{
        //    throw new InvalidOperationException("SynchronizationContextUnhandledExceptionButton_Click");
        //}

        //private void OtherUnhandledExceptionButton_Click(object sender, RoutedEventArgs e)
        //{
        //    var t = Test();
        //}

        //private async Task Test()
        //{
        //    throw new InvalidOperationException("OtherUnhandledExceptionButton_Click");
        //}

        //private void AppUnhandledExceptionButton_Click(object sender, RoutedEventArgs e)
        //{
        //    throw new InvalidOperationException("AppUnhandledExceptionButton_Click");
        //}
    }
}
