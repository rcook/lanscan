//////////////////////////////////////////////////////////////////////
//
// Lanscan
// Copyright (C) 2013, Richard Cook. All rights reserved.
//
//////////////////////////////////////////////////////////////////////

namespace Lanscan.UI
{
    using System.Collections.Generic;
    using System.Linq;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;

    public sealed partial class OctetControl : UserControl
    {
        private static readonly OctetControlValue[] s_values = Enumerable.Range(0, 256).Select(x => new OctetControlValue { Value = (byte)x }).ToArray();

        public static readonly DependencyProperty SelectedIndexProperty = DependencyProperty.Register(
            "SelectedIndex",
            typeof(int),
            typeof(OctetControl),
            new PropertyMetadata(-1, OnSelectedIndexPropertyChanged));

        public OctetControl()
        {
            InitializeComponent();
        }

        public event SelectionChangedEventHandler SelectionChanged;

        public int SelectedIndex
        {
            get { return (int)GetValue(SelectedIndexProperty); }
            set { SetValue(SelectedIndexProperty, value); }
        }

        public IEnumerable<OctetControlValue> Values
        {
            get { return s_values; }
        }

        private static void OnSelectedIndexPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var self = (OctetControl)sender;
            self.OnSelectedIndexPropertyChanged((int)e.OldValue, (int)e.NewValue);
        }

        private void OnSelectedIndexPropertyChanged(int oldValue, int newValue)
        {
            ComboBox.SelectedIndex = newValue;
        }

        private void RaiseSelectionChangedEvent(SelectionChangedEventArgs e)
        {
            var handler = SelectionChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RaiseSelectionChangedEvent(e);
        }
    }
}
