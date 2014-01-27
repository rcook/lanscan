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
    using System.ComponentModel;
    using System.Linq;
    using System.Linq.Expressions;
    using Lanscan.Networking;
    using Lanscan.Utilities;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;

    public sealed partial class AddUserServiceControl : UserControl, INotifyPropertyChanged
    {
        private static readonly Protocol[] AvailableProtocolsInternal = Enum.GetValues(typeof(Protocol))
            .Cast<Protocol>()
            .ToArray();
        private string m_selectedName;
        private string m_selectedPort;

        public static readonly DependencyProperty SelectedProtocolProperty = DependencyProperty.Register(
            "SelectedProtocol",
            typeof(Protocol),
            typeof(AddUserServiceControl),
            new PropertyMetadata(Protocol.Tcp));

        public event PropertyChangedEventHandler PropertyChanged;

        public event EventHandler AddClicked;

        public AddUserServiceControl()
        {
            InitializeComponent();
        }

        public IEnumerable<Protocol> AvailableProtocols
        {
            get { return AvailableProtocolsInternal; }
        }

        public string SelectedName
        {
            get { return m_selectedName; }
            private set
            {
                if (Object.Equals(value, m_selectedName))
                {
                    return;
                }

                m_selectedName = value;
                NotifyPropertyChanged(() => SelectedName);
                NotifyPropertyChanged(() => IsSelectedNameInvalid);
                NotifyPropertyChanged(() => IsComplete);
            }
        }

        public bool IsSelectedNameInvalid
        {
            get
            {
                if (String.IsNullOrEmpty(m_selectedName))
                {
                    return false;
                }

                var result = String.IsNullOrWhiteSpace(m_selectedName);
                return result;
            }
        }

        public Protocol SelectedProtocol
        {
            get { return (Protocol)GetValue(SelectedProtocolProperty); }
            set { SetValue(SelectedProtocolProperty, value); }
        }

        public bool IsSelectedProtocolValid
        {
            get { return true; }
        }

        public string SelectedPort
        {
            get { return m_selectedPort; }
            private set
            {
                if (Object.Equals(value, m_selectedPort))
                {
                    return;
                }

                m_selectedPort = value;
                NotifyPropertyChanged(() => SelectedPort);
                NotifyPropertyChanged(() => IsSelectedPortInvalid);
                NotifyPropertyChanged(() => IsComplete);
            }
        }

        public bool IsSelectedPortInvalid
        {
            get
            {
                if (String.IsNullOrEmpty(m_selectedPort))
                {
                    return false;
                }

                ushort temp;
                var result = !UInt16.TryParse(m_selectedPort, out temp);
                return result;
            }
        }

        public bool IsComplete
        {
            get
            {
                if (String.IsNullOrEmpty(m_selectedName))
                {
                    return false;
                }
                if (String.IsNullOrEmpty(m_selectedPort))
                {
                    return false;
                }

                var result = !IsSelectedNameInvalid && !IsSelectedPortInvalid;
                return result;
            }
        }

        private void NotifyPropertyChanged<T>(Expression<Func<T>> propertySelector)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                // Consider moving caching from NotifyPropertyChangedBase into helper so we can reuse it...
                var propertyName = BindingHelper.GetPropertyName(propertySelector);
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            RaiseAddClicked();
        }

        private void RaiseAddClicked()
        {
            var handler = AddClicked;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        private void NameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = (TextBox)sender;
            SelectedName = textBox.Text;
        }

        private void PortTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = (TextBox)sender;
            SelectedPort = textBox.Text;
        }
    }
}
