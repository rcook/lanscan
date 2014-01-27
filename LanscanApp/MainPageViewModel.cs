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
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using Lanscan.Networking;
    using Lanscan.Networking.Dns;
    using Lanscan.Utilities;
    using Windows.UI.Popups;
    using IServiceProvider = Lanscan.Utilities.IServiceProvider;

    public sealed class MainPageViewModel : PageViewModel
    {
        private const int TruncatedAddressSelectionCount = 255;

        private readonly IPropertyDescriptor<ConnectionInfo> m_connectionInfoProperty;
        private readonly IPropertyDescriptor<TaskStatus> m_connectionInfoTaskStatusProperty;
        private readonly IPropertyDescriptor<DhcpInfo> m_dhcpInfoProperty;
        private readonly IPropertyDescriptor<TaskStatus> m_dhcpInfoTaskStatusProperty;
        private readonly IPropertyDescriptor<IPAddress> m_externalAddressProperty;
        private readonly IPropertyDescriptor<TaskStatus> m_externalAddressTaskStatusProperty;
        private readonly IPropertyDescriptor<string> m_statusProperty;
        private readonly IPropertyDescriptor<double> m_progressPercentProperty;
        private readonly IPropertyDescriptor<bool> m_isScanInProgressProperty;
        private readonly IPropertyDescriptor<ObservableCollection<UICommand>> m_customSelectionCommandsProperty;
        private readonly IPropertyDescriptor<uint> m_startAddressValueProperty;
        private readonly IPropertyDescriptor<bool> m_isStartAddressValidProperty;
        private readonly IPropertyDescriptor<uint> m_endAddressValueProperty;
        private readonly IPropertyDescriptor<bool> m_isEndAddressValidProperty;
        private readonly DelegateCommand m_scanCommand;
        private readonly DelegateCommand m_cancelScanCommand;
        private readonly DelegateCommand m_showServicesCommand;
        private readonly DelegateCommand<IPEndpoint> m_launchUriCommand;
        private readonly SortedObservableCollection<ScanNetworkResult> m_availableServices = new SortedObservableCollection<ScanNetworkResult>(ScanNetworkResultComparer.Instance);
        private bool m_isScanCommandEnabled;
        private bool m_isCancelScanCommandEnabled;
        private UdpDnsResolver m_dnsResolver;
        private CancellationTokenSource m_tokenSource;

        [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling")]
        public MainPageViewModel(IServiceProvider serviceProvider, AppViewModel appViewModel)
            : base(serviceProvider, appViewModel)
        {
            m_connectionInfoProperty = CreateDictionaryProperty(() => ConnectionInfo, () => null);
            m_connectionInfoTaskStatusProperty = CreateDictionaryProperty(() => ConnectionInfoTaskStatus, () => TaskStatus.Created);
            m_dhcpInfoProperty = CreateDictionaryProperty(() => DhcpInfo, () => null);
            m_dhcpInfoTaskStatusProperty = CreateDictionaryProperty(() => DhcpInfoTaskStatus, () => TaskStatus.Created);
            m_externalAddressProperty = CreateDictionaryProperty(() => ExternalAddress, () => null);
            m_externalAddressTaskStatusProperty = CreateDictionaryProperty(() => ExternalAddressTaskStatus, () => TaskStatus.Created);
            m_statusProperty = CreateDictionaryProperty(() => Status, () => String.Empty);
            m_progressPercentProperty = CreateDictionaryProperty(() => ProgressPercent, () => 0.0d);
            m_isScanInProgressProperty = CreateDictionaryProperty(() => IsScanInProgress, () => false);
            m_customSelectionCommandsProperty = CreateDictionaryProperty(() => CustomSelectionCommands, () => new ObservableCollection<UICommand>());
            m_startAddressValueProperty = CreateSettingsProperty(V1SettingsConstants.StartAddressValueKey, () => StartAddressValue, () => 0u, () => { UpdateActionBarControls(); });
            m_isStartAddressValidProperty = CreateDictionaryProperty(() => IsStartAddressValid, () => false);
            m_endAddressValueProperty = CreateSettingsProperty(V1SettingsConstants.EndAddressValueKey, () => EndAddressValue, () => 0u, () => { UpdateActionBarControls(); });
            m_isEndAddressValidProperty = CreateDictionaryProperty(() => IsEndAddressValid, () => false);

            var hostInfoService = ServiceProvider.GetService<IHostInfoService>();
            var hostInfo = hostInfoService.GetCurrentHostInfo();
            if (hostInfo == null)
            {
                Status = Strings.Status_GetHostInfoFailed;
            }
            else
            {
                var connectionInfoTask = RunGetConnectionInfoAsync(hostInfo);
                var dhcpInfoTask = RunGetDhcpInfoAsync(hostInfo);
                var externalAddressTask = RunGetExternalAddressAsync();

                Task.Factory.ContinueWhenAll(new[] { connectionInfoTask, dhcpInfoTask }, t =>
                {
                    if (ConnectionInfo != null)
                    {
                        if (DhcpInfo != null && DhcpInfo.DnsServerAddress != null)
                        {
                            m_dnsResolver = new UdpDnsResolver(
                                new DnsResolverOptions(),
                                new DatagramSocketFactory(),
                                new IPEndpoint(DhcpInfo.DnsServerAddress, Constants.DefaultDnsPort));
                        }

                        SetAddressValues(ConnectionInfo.Network);

                        IsScanInProgress = false;
                        m_isScanCommandEnabled = true;
                        m_scanCommand.Refresh();
                        m_isCancelScanCommandEnabled = false;
                        m_cancelScanCommand.Refresh();
                    }
                },
                CancellationToken.None,
                TaskContinuationOptions.None,
                TaskScheduler.FromCurrentSynchronizationContext());
            }

            m_scanCommand = new DelegateCommand(async () =>
            {
                var scanStopwatch = new Stopwatch();
                scanStopwatch.Start();

                m_tokenSource = new CancellationTokenSource();

                AvailableServices.Clear();

                IsScanInProgress = true;
                m_isScanCommandEnabled = false;
                m_scanCommand.Refresh();
                m_isCancelScanCommandEnabled = true;
                m_cancelScanCommand.Refresh();

                Status = Strings.Status_ScanStarted;

                var selectedAddresses = new IPAddressRange(StartAddressValue, EndAddressValue);
                var networkServices = await GetNetworkServicesAsync();

                var index = 0;
                var progress = (IProgress<ScanNetworkBatch>)new Progress<ScanNetworkBatch>(batch =>
                {
                    foreach (var result in batch.Results)
                    {
                        if (result.IsAvailable)
                        {
                            AvailableServices.Add(result);
                        }
                    }
                    index += batch.Results.Count;
                    ProgressPercent = 100.0d * index / (selectedAddresses.Count * networkServices.Length);
                });

                var task = Task.Factory.StartNew(
                    arg => { Scanner.ScanNetwork(m_tokenSource.Token, m_dnsResolver, selectedAddresses, networkServices, progress); },
                    m_tokenSource.Token,
                    TaskCreationOptions.None);
                var t1 = task.ContinueWith(t =>
                {
                    IsScanInProgress = false;
                    m_isScanCommandEnabled = true;
                    m_scanCommand.Refresh();
                    m_isCancelScanCommandEnabled = false;
                    m_cancelScanCommand.Refresh();

                    var isCancelled = m_tokenSource.Token.IsCancellationRequested;
                    if (isCancelled)
                    {
                        Status = Strings.Status_ScanCancelled;
                        ProgressPercent = 0.0d;
                    }
                    else
                    {
                        scanStopwatch.Stop();
                        var timeSpan = scanStopwatch.Elapsed;
                        Status = Strings.Format_Status_ScanCompleted(timeSpan);
                        ProgressPercent = 100.0d;
                    }

                    m_tokenSource.Dispose();
                },
                    CancellationToken.None,
                    TaskContinuationOptions.None,
                    TaskScheduler.FromCurrentSynchronizationContext());
            },
            () => m_isScanCommandEnabled);

            m_cancelScanCommand = new DelegateCommand(() =>
            {
                m_isCancelScanCommandEnabled = false;
                m_cancelScanCommand.Refresh();

                m_tokenSource.Cancel();
            },
            () => m_isCancelScanCommandEnabled);

            m_showServicesCommand = new DelegateCommand(() =>
            {
                AppViewModel.NavigateToServicesPage();
            });

            m_launchUriCommand = new DelegateCommand<IPEndpoint>(endpoint =>
            {
                AppViewModel.NavigateToBrowserPage(endpoint);
            });
        }

        public ConnectionInfo ConnectionInfo
        {
            get { return GetPropertyValue(m_connectionInfoProperty); }
            set { SetPropertyValue(m_connectionInfoProperty, value); }
        }

        public TaskStatus ConnectionInfoTaskStatus
        {
            get { return GetPropertyValue(m_connectionInfoTaskStatusProperty); }
            set { SetPropertyValue(m_connectionInfoTaskStatusProperty, value); }
        }

        public DhcpInfo DhcpInfo
        {
            get { return GetPropertyValue(m_dhcpInfoProperty); }
            set { SetPropertyValue(m_dhcpInfoProperty, value); }
        }

        public TaskStatus DhcpInfoTaskStatus
        {
            get { return GetPropertyValue(m_dhcpInfoTaskStatusProperty); }
            set { SetPropertyValue(m_dhcpInfoTaskStatusProperty, value); }
        }

        public IPAddress ExternalAddress
        {
            get { return GetPropertyValue(m_externalAddressProperty); }
            set { SetPropertyValue(m_externalAddressProperty, value); }
        }

        public TaskStatus ExternalAddressTaskStatus
        {
            get { return GetPropertyValue(m_externalAddressTaskStatusProperty); }
            set { SetPropertyValue(m_externalAddressTaskStatusProperty, value); }
        }

        public string Status
        {
            get { return GetPropertyValue(m_statusProperty); }
            set { SetPropertyValue(m_statusProperty, value); }
        }

        public double ProgressPercent
        {
            get { return GetPropertyValue(m_progressPercentProperty); }
            set { SetPropertyValue(m_progressPercentProperty, value); }
        }

        public bool IsScanInProgress
        {
            get { return GetPropertyValue(m_isScanInProgressProperty); }
            set { SetPropertyValue(m_isScanInProgressProperty, value); }
        }

        public ObservableCollection<UICommand> CustomSelectionCommands
        {
            get { return GetPropertyValue(m_customSelectionCommandsProperty); }
            set { SetPropertyValue(m_customSelectionCommandsProperty, value); }
        }

        public uint StartAddressValue
        {
            get { return GetPropertyValue(m_startAddressValueProperty); }
            set { SetPropertyValue(m_startAddressValueProperty, value); }
        }

        public bool IsStartAddressValid
        {
            get { return GetPropertyValue(m_isStartAddressValidProperty); }
            set { SetPropertyValue(m_isStartAddressValidProperty, value); }
        }

        public uint EndAddressValue
        {
            get { return GetPropertyValue(m_endAddressValueProperty); }
            set { SetPropertyValue(m_endAddressValueProperty, value); }
        }

        public bool IsEndAddressValid
        {
            get { return GetPropertyValue(m_isEndAddressValidProperty); }
            set { SetPropertyValue(m_isEndAddressValidProperty, value); }
        }

        public ICommand ScanCommand
        {
            get { return m_scanCommand; }
        }

        public ICommand CancelScanCommand
        {
            get { return m_cancelScanCommand; }
        }

        public ICommand ShowServicesCommand
        {
            get { return m_showServicesCommand; }
        }

        public ICommand LaunchUriCommand
        {
            get { return m_launchUriCommand; }
        }

        public SortedObservableCollection<ScanNetworkResult> AvailableServices
        {
            get { return m_availableServices; }
        }

        private static bool IsValidStartAddress(IPNetwork network, uint startAddressValue, uint endAddressValue)
        {
            var result = startAddressValue >= network.FirstUsableAddress.Value && startAddressValue <= endAddressValue;
            return result;
        }

        private static bool IsValidEndAddress(IPNetwork network, uint startAddressValue, uint endAddressValue)
        {
            var result = endAddressValue >= startAddressValue && endAddressValue <= network.LastUsableAddress.Value;
            return result;
        }

        private static async Task<INetworkServiceCore[]> GetNetworkServicesAsync()
        {
            var predefinedNetworkServices = await NetworkServiceConfigurator.LoadPredefinedNetworkServicesAsync();
            var userNetworkServices = await NetworkServiceConfigurator.LoadUserNetworkServicesAsync();
            var allNetworkServices = predefinedNetworkServices.Cast<INetworkService>().Concat(userNetworkServices.Cast<INetworkService>());
            var tcpNetworkServices = allNetworkServices
                .Where(x => x.IsEnabled && x.Protocol == Protocol.Tcp)
                .Distinct(NetworkServiceCoreComparer.Instance)
                .OrderBy(x => x.Port);
            var udpNetworkServices = allNetworkServices
                .Where(x => x.IsEnabled && x.Protocol == Protocol.Udp)
                .Distinct(NetworkServiceCoreComparer.Instance)
                .OrderBy(x => x.Port);
            return tcpNetworkServices.Concat(udpNetworkServices).ToArray();
        }

        private Task RunGetConnectionInfoAsync(HostInfo hostInfo)
        {
            var task = Task<ConnectionInfo>.Factory.StartNew(() =>
            {
                var connectionInfoService = ServiceProvider.GetService<IConnectionInfoService>();
                var result = connectionInfoService.GetConnectionInfo(hostInfo);
                if (result == null)
                {
                    throw new Exception("GetConnectionInfo failed");
                }

                return result;
            });
            ConnectionInfoTaskStatus = TaskStatus.Running;
            var completedTask = WrapTask(
                task,
                TaskScheduler.FromCurrentSynchronizationContext(),
                Strings.Status_GetConnectionInfoFailed,
                t =>
                {
                    ConnectionInfo = t.Result;
                    ConnectionInfoTaskStatus = TaskStatus.RanToCompletion;
                },
                t => { ConnectionInfoTaskStatus = TaskStatus.Faulted; });
            return completedTask;
        }

        private Task RunGetDhcpInfoAsync(HostInfo hostInfo)
        {
            var task = Task<DhcpInfo>.Factory.StartNew(() =>
            {
                var dhcpInfoService = ServiceProvider.GetService<IDhcpInfoService>();
                var result = dhcpInfoService.GetDhcpInfo(hostInfo.NetworkAdapterId);
                if (result == null)
                {
                    throw new Exception("GetDhcpInfo failed");
                }

                return result;
            });
            DhcpInfoTaskStatus = TaskStatus.Running;
            var completedTask = WrapTask(
                task,
                TaskScheduler.FromCurrentSynchronizationContext(),
                Strings.Status_GetDhcpInfoFailed,
                t =>
                {
#if SCREENSHOT
                    DhcpInfo = new DhcpInfo(t.Result.GatewayAddress, t.Result.DnsServerAddress, "my-domain-name");
#else
                    DhcpInfo = t.Result;
#endif
                    DhcpInfoTaskStatus = TaskStatus.RanToCompletion;
                },
                t => { DhcpInfoTaskStatus = TaskStatus.Faulted; });
            return completedTask;
        }

        private Task RunGetExternalAddressAsync()
        {
            var externalAddressService = ServiceProvider.GetService<IExternalAddressService>();
            var task = externalAddressService.GetCurrentExternalAddressAsync().ContinueWith(t =>
            {
                if (t.Result == null)
                {
                    throw new Exception("GetCurrentExternalAddressAsync failed");
                }
                return t.Result;
            });
            ExternalAddressTaskStatus = TaskStatus.Running;
            var completedTask = WrapTask(
                task,
                TaskScheduler.FromCurrentSynchronizationContext(),
                Strings.Status_GetExternalAddressFailed,
                t =>
                {
#if SCREENSHOT
                    ExternalAddress = IPAddress.Parse("55.55.55.55");
#else
                    ExternalAddress = t.Result;
#endif
                    ExternalAddressTaskStatus = TaskStatus.RanToCompletion;
                },
                t => { ExternalAddressTaskStatus = TaskStatus.Faulted; });
            return completedTask;
        }

        private Task WrapTask<TResult>(Task<TResult> task, TaskScheduler uiTaskScheduler, string errorMessage, Action<Task<TResult>> successContinuationAction, Action<Task<TResult>> faultContinuationAction)
        {
            var completedTask = task.ContinueWith(
                successContinuationAction,
                CancellationToken.None,
                TaskContinuationOptions.NotOnFaulted,
                uiTaskScheduler);
            task.ContinueWith(t =>
            {
                faultContinuationAction(t);
                Status = errorMessage;
                AppEventSource.Instance.Debug(t.Exception.ToString());
            },
            CancellationToken.None,
            TaskContinuationOptions.OnlyOnFaulted,
            uiTaskScheduler);
            return completedTask;
        }

        private void UpdateActionBarControls()
        {
            if (ConnectionInfo != null)
            {
                IsStartAddressValid = IsValidStartAddress(ConnectionInfo.Network, StartAddressValue, EndAddressValue);
                IsEndAddressValid = IsValidEndAddress(ConnectionInfo.Network, StartAddressValue, EndAddressValue);
                var isValidAddressRange = IsStartAddressValid && IsEndAddressValid;
                m_isScanCommandEnabled = isValidAddressRange;
                m_scanCommand.Refresh();
            }
        }

        private void SetAddressValues(IPNetwork network)
        {
            var isStartAddressValid = IsValidStartAddress(network, StartAddressValue, EndAddressValue);
            var isEndAddressValid = IsValidEndAddress(network, StartAddressValue, EndAddressValue);
            if (!isStartAddressValid || !isEndAddressValid)
            {
                StartAddressValue = network.FirstUsableAddress.Value;
                EndAddressValue = network.LastUsableAddress.Value;
            }
            UpdateActionBarControls();

            if (network.UsableAddresses.Count > TruncatedAddressSelectionCount)
            {
                CustomSelectionCommands.Add(new UICommand(Strings.Format_ActionBarControl_SelectFirstNUsableAddressesFormat(TruncatedAddressSelectionCount), SetTruncatedAddressSelection));
            }

            CustomSelectionCommands.Add(new UICommand(Strings.ActionBarControl_SelectWholeNetwork, SetWholeAddressSelection));
        }

        private void SetTruncatedAddressSelection(IUICommand command)
        {
            var firstUsableAddressValue = ConnectionInfo.Network.FirstUsableAddress.Value;
            var lastUsableAddressValue = ConnectionInfo.Network.LastUsableAddress.Value;

            StartAddressValue = firstUsableAddressValue;
            EndAddressValue = Math.Min(lastUsableAddressValue, firstUsableAddressValue + TruncatedAddressSelectionCount - 1);
        }

        private void SetWholeAddressSelection(IUICommand command)
        {
            StartAddressValue = ConnectionInfo.Network.FirstUsableAddress.Value;
            EndAddressValue = ConnectionInfo.Network.LastUsableAddress.Value;
        }
    }
}
