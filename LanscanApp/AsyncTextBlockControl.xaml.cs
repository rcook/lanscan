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
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;

    public sealed partial class AsyncTextBlockControl : UserControl
    {
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            "Text",
            typeof(string),
            typeof(AsyncTextBlockControl),
            null);

        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly DependencyProperty TaskStatusProperty = DependencyProperty.Register(
            "TaskStatus",
            typeof(TaskStatus),
            typeof(AsyncTextBlockControl),
            null);

        public AsyncTextBlockControl()
        {
            InitializeComponent();
        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public TaskStatus TaskStatus
        {
            get { return (TaskStatus)GetValue(TaskStatusProperty); }
            set { SetValue(TaskStatusProperty, value); }
        }
    }
}
