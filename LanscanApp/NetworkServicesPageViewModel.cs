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
    using System.Collections.Specialized;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using Callisto.Controls;
    using Lanscan.DataContracts;
    using Lanscan.Utilities;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls.Primitives;
    using IServiceProvider = Lanscan.Utilities.IServiceProvider;

    public sealed class NetworkServicesPageViewModel : PageViewModel
    {
        private DelegateCommand<UIElement> m_addUserServiceCommand;
        private DelegateCommand m_removeUserServiceCommand;
        private NetworkServicePresenter<NetworkServiceRegistryEntry>[] m_predefinedServiceSelections;
        private SortedObservableCollection<NetworkServicePresenter<V1Service>> m_userNetworkServices = new SortedObservableCollection<NetworkServicePresenter<V1Service>>(NetworkServicePresenterComparer.Instance);
        private ObservableCollection<object> m_selectedUserServices = new ObservableCollection<object>();

        public NetworkServicesPageViewModel(IServiceProvider serviceProvider, AppViewModel appViewModel)
            : base(serviceProvider, appViewModel)
        {
            m_selectedUserServices.CollectionChanged += OnSelectedUserServicesCollectionChanged;

            m_addUserServiceCommand = new DelegateCommand<UIElement>(e => { OnAddUserServiceCommand(e); });

            m_removeUserServiceCommand = new DelegateCommand(async () =>
            {
                if (m_selectedUserServices.Count > 0)
                {
                    var userServiceSelectionsToRemove = m_selectedUserServices.Cast<NetworkServicePresenter<V1Service>>().ToArray();
                    var userServices = new V1UserServices();
                    foreach (var serviceSelection in m_userNetworkServices)
                    {
                        if (!userServiceSelectionsToRemove.Contains(serviceSelection))
                        {
                            var service = serviceSelection.Service;
                            var isEnabled = serviceSelection.IsEnabled;
                            userServices.Services.Add(new V1Service(service.Name, service.Protocol, service.Port, isEnabled));
                        }
                    }
                    await NetworkServiceConfigurator.WriteUserServicesAsync(userServices);
                    foreach (var serviceSelection in userServiceSelectionsToRemove)
                    {
                        m_userNetworkServices.Remove(serviceSelection);
                    }
                }
            },
            () => m_selectedUserServices.Count > 0);

            var t = LoadServiceConfigurationAsync();
        }

        public ICommand AddUserServiceCommand
        {
            get { return m_addUserServiceCommand; }
        }

        public ICommand RemoveUserServiceCommand
        {
            get { return m_removeUserServiceCommand; }
        }

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public NetworkServicePresenter<NetworkServiceRegistryEntry>[] PredefinedNetworkServices
        {
            get { return m_predefinedServiceSelections; }
            private set // TODO: Consider making this ObservableCollection<IServiceSelection> etc.
            {
                m_predefinedServiceSelections = value;
                NotifyPropertyChanged(() => PredefinedNetworkServices);
            }
        }

        public ObservableCollection<NetworkServicePresenter<V1Service>> UserNetworkServices
        {
            get { return m_userNetworkServices; }
        }

        public ObservableCollection<object> SelectedUserServices
        {
            get { return m_selectedUserServices; }
        }

        private static async Task<NetworkServicePresenter<NetworkServiceRegistryEntry>[]> GetPredefinedNetworkServicesAsync()
        {
            var temp = await NetworkServiceConfigurator.LoadPredefinedNetworkServicesAsync();
            var result = temp
                .Select(x => new NetworkServicePresenter<NetworkServiceRegistryEntry>(x.Name, x.Protocol, x.Port, x.IsEnabled, x.Service))
                .ToArray();
            return result;
        }

        private static async Task<NetworkServicePresenter<V1Service>[]> GetUserNetworkServicesAsync()
        {
            var temp = await NetworkServiceConfigurator.LoadUserNetworkServicesAsync();
            var result = temp
                .Select(x => new NetworkServicePresenter<V1Service>(x.Name, x.Protocol, x.Port, x.IsEnabled, x.Service))
                .ToArray();
            return result;
        }

        private async Task LoadServiceConfigurationAsync()
        {
            var predefinedNetworkServices = await GetPredefinedNetworkServicesAsync();
            foreach (var networkService in predefinedNetworkServices)
            {
                networkService.IsEnabledChanged += OnPredefinedServiceSelectionIsEnabledChanged;
            }
            PredefinedNetworkServices = predefinedNetworkServices;

            var userNetworkServices = await GetUserNetworkServicesAsync();
            m_userNetworkServices.Clear();
            foreach (var networkService in userNetworkServices)
            {
                networkService.IsEnabledChanged += OnUserServiceSelectionIsEnabledChanged;
                m_userNetworkServices.Add(networkService);
            }
        }

        private void AddUserServiceSelection(V1Service service)
        {
            var serviceSelection = new NetworkServicePresenter<V1Service>(service.Name, service.Protocol.Convert(), service.Port, service.IsEnabled, service);
            serviceSelection.IsEnabledChanged += OnUserServiceSelectionIsEnabledChanged;
            try
            {
                // TODO: Need to use CollectionView instead of relying on ordered insert.
                m_userNetworkServices.Add(serviceSelection);
            }
            catch (ArgumentException)
            {
                // TODO: Duplicate item. CollectionView will eliminate requirement for this.
            }
        }

        private void OnAddUserServiceCommand(UIElement element)
        {
            var addUserServiceControl = new AddUserServiceControl();
            var flyout = new Flyout();
            flyout.Content = addUserServiceControl;
            flyout.Placement = PlacementMode.Top;
            flyout.PlacementTarget = element;
            flyout.IsOpen = true;
            flyout.Background = addUserServiceControl.Background;

            addUserServiceControl.AddClicked += async (sender, e) =>
            {
                flyout.IsOpen = false;

                var service = new V1Service(
                    addUserServiceControl.SelectedName,
                    addUserServiceControl.SelectedProtocol.Convert(),
                    UInt16.Parse(addUserServiceControl.SelectedPort),
                    true);
                AddUserServiceSelection(service);

                var userServices = new V1UserServices();
                userServices.Services.AddRange(m_userNetworkServices.Select(x => x.Service));
                await NetworkServiceConfigurator.WriteUserServicesAsync(userServices);
            };
        }

        private async void OnPredefinedServiceSelectionIsEnabledChanged(object sender, IsEnabledChangedEventArgs e)
        {
            var disabledServices = new V1DisabledServices();
            disabledServices.Guids.AddRange(m_predefinedServiceSelections.Where(x => !x.IsEnabled).Select(x => x.Service.Guid));
            await NetworkServiceConfigurator.WriteDisabledServicesAsync(disabledServices);
        }

        private async void OnUserServiceSelectionIsEnabledChanged(object sender, IsEnabledChangedEventArgs e)
        {
            var userServices = new V1UserServices();
            foreach (var serviceSelection in m_userNetworkServices)
            {
                var service = serviceSelection.Service;
                var isEnabled = serviceSelection.IsEnabled;
                userServices.Services.Add(new V1Service(service.Name, service.Protocol, service.Port, isEnabled));
            }
            await NetworkServiceConfigurator.WriteUserServicesAsync(userServices);
        }

        private void OnSelectedUserServicesCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            m_removeUserServiceCommand.Refresh();
        }
    }
}
