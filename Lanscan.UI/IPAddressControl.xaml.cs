//////////////////////////////////////////////////////////////////////
//
// Lanscan
// Copyright (C) 2013, Richard Cook. All rights reserved.
//
//////////////////////////////////////////////////////////////////////

namespace Lanscan.UI
{
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;

    public sealed partial class IPAddressControl : UserControl
    {
        public static readonly DependencyProperty AddressValueProperty = DependencyProperty.Register(
            "AddressValue",
            typeof(uint),
            typeof(IPAddressControl),
            new PropertyMetadata((uint)0, OnAddressValuePropertyChanged));

        public static readonly DependencyProperty BorderStyleProperty = DependencyProperty.Register(
            "BorderStyle",
            typeof(Style),
            typeof(IPAddressControl),
            new PropertyMetadata(null, OnBorderStylePropertyChanged));

        public IPAddressControl()
        {
            InitializeComponent();
        }

        public uint AddressValue
        {
            get { return (uint)GetValue(AddressValueProperty); }
            set { SetValue(AddressValueProperty, value); }
        }

        public Style BorderStyle
        {
            get { return (Style)GetValue(BorderStyleProperty); }
            set { SetValue(BorderStyleProperty, value); }
        }

        private static void OnAddressValuePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var self = (IPAddressControl)sender;
            self.OnAddressValuePropertyChanged((uint)e.OldValue, (uint)e.NewValue);
        }

        private static void OnBorderStylePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var self = (IPAddressControl)sender;
            self.OnBorderStylePropertyChanged((Style)e.OldValue, (Style)e.NewValue);
        }

        private static uint GetAddressValue(uint addressValue, int octetIndex, byte octetValue)
        {
            var bitShift = ((3 - octetIndex) * 8);

            var mask = unchecked((uint)~(0xFF << bitShift));
            var shiftedOctetValue = unchecked((uint)octetValue << bitShift);

            var result = addressValue & mask | shiftedOctetValue;
            return result;
        }

        private void OnAddressValuePropertyChanged(uint oldValue, uint newValue)
        {
            var firstOctetControlSelectedIndex = (int)newValue >> 24 & 0xFF;
            var secondOctetControlSelectedIndex = (int)newValue >> 16 & 0xFF;
            var thirdOctetControlSelectedIndex = (int)newValue >> 8 & 0xFF;
            var fourthOctetControlSelectedIndex = (int)newValue & 0xFF;

            FirstOctetControl.SelectedIndex = firstOctetControlSelectedIndex;
            SecondOctetControl.SelectedIndex = secondOctetControlSelectedIndex;
            ThirdOctetControl.SelectedIndex = thirdOctetControlSelectedIndex;
            FourthOctetControl.SelectedIndex = fourthOctetControlSelectedIndex;
        }

        private void OnBorderStylePropertyChanged(Style oldValue, Style newValue)
        {
            Border.Style = newValue;
        }

        private void FirstOctetControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var octetValue = ((OctetControlValue)e.AddedItems[0]).Value;
            AddressValue = GetAddressValue(AddressValue, 0, octetValue);
        }

        private void SecondOctetControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var octetValue = ((OctetControlValue)e.AddedItems[0]).Value;
            AddressValue = GetAddressValue(AddressValue, 1, octetValue);
        }

        private void ThirdOctetControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var octetValue = ((OctetControlValue)e.AddedItems[0]).Value;
            AddressValue = GetAddressValue(AddressValue, 2, octetValue);
        }

        private void FourthOctetControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var octetValue = ((OctetControlValue)e.AddedItems[0]).Value;
            AddressValue = GetAddressValue(AddressValue, 3, octetValue);
        }
    }
}
