namespace ReproApp
{
    using System;
    using Windows.UI.Xaml.Controls;

    public sealed partial class MyUserControl : UserControl
    {
        public MyUserControl()
        {
            InitializeComponent();
        }

        public event EventHandler SelectedValueChanged;

        public string SelectedValue
        {
            get { return (string)((ComboBoxItem)ComboBox.SelectedValue).Content; }
        }

        private void RaiseSelectedValueChangedEvent()
        {
            var handler = SelectedValueChanged;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RaiseSelectedValueChangedEvent();
        }
    }
}
