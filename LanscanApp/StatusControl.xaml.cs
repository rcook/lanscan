//////////////////////////////////////////////////////////////////////
//
// Lanscan
// Copyright (C) 2013, Richard Cook. All rights reserved.
//
//////////////////////////////////////////////////////////////////////

namespace LanscanApp
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;

    public sealed partial class StatusControl : UserControl
    {
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly DependencyProperty StatusProperty = DependencyProperty.Register(
            "Status",
            typeof(string),
            typeof(StatusControl),
            new PropertyMetadata(String.Empty));

        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly DependencyProperty ProgressPercentProperty = DependencyProperty.Register(
            "ProgressPercent",
            typeof(double),
            typeof(StatusControl),
            new PropertyMetadata(0.0d));

        public StatusControl()
        {
            InitializeComponent();
        }

        public string Status
        {
            get { return (string)GetValue(StatusProperty); }
            set { SetValue(StatusProperty, value); }
        }

        public double ProgressPercent
        {
            get { return (double)GetValue(ProgressPercentProperty); }
            set { SetValue(ProgressPercentProperty, value); }
        }
    }
}
