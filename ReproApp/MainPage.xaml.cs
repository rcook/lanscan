namespace ReproApp
{
    using System;
    using Callisto.Controls;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Controls.Primitives;

    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void MyUserControl_SelectedValueChanged(object sender, EventArgs e)
        {
            var myUserControl = (MyUserControl)sender;
            SelectedValueTextBlock.Text = myUserControl.SelectedValue;
        }

        private void TestFlyoutButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Flyout f = new Flyout();

            f.Margin = new Thickness(20, 12, 20, 12);

            // SampleInput is a user control with a TextBox in it
            f.Content = new NewServiceControl();

            f.Placement = PlacementMode.Top;
            f.PlacementTarget = sender as UIElement;

            var parentGrid = Top;

            parentGrid.Children.Add(f.HostPopup);

            f.Closed += (b, c) =>
            {
                parentGrid.Children.Remove(f.HostPopup);
            };

            f.IsOpen = true;
        }
    }
}
