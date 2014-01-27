//////////////////////////////////////////////////////////////////////
//
// Lanscan
// Copyright (C) 2013, Richard Cook. All rights reserved.
//
//////////////////////////////////////////////////////////////////////

namespace LanscanApp.UnitTests
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Lanscan.Networking;
    using Lanscan.TestFramework;
    using Lanscan.Utilities;
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
    using Windows.Networking;

    public static class On_MainPageViewModel
    {
        [TestClass]
        public sealed class MainPageViewModelConstructorTest
        {
            private static readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(5);

            [TestMethod]
            public void TestSample()
            {
                var serviceProvider = CreateServiceProvider();
                var appViewModel = new AppViewModel(serviceProvider);

                MainPageViewModel viewModel = null;
                var action = Execute.OnUIThread(() =>
                {
                    viewModel = new MainPageViewModel(serviceProvider, appViewModel);
                });
                Eventually.ShouldBecomeTrue(() => viewModel != null, DefaultTimeout);

                Eventually.ShouldBecomeTrue(() => viewModel.ConnectionInfo != null, DefaultTimeout);
                viewModel.ConnectionInfoTaskStatus.ShouldEqual(TaskStatus.RanToCompletion);
                viewModel.ConnectionInfo.Address.ToString().ShouldEqual("22.33.44.55");
                viewModel.ConnectionInfo.Network.ToString().ShouldEqual("22.33.44.0/24");

                Eventually.ShouldBecomeTrue(() => viewModel.DhcpInfo != null, DefaultTimeout);
                viewModel.DhcpInfoTaskStatus.ShouldEqual(TaskStatus.RanToCompletion);
                viewModel.DhcpInfo.GatewayAddress.ToString().ShouldEqual("100.101.102.103");
                viewModel.DhcpInfo.DnsServerAddress.ToString().ShouldEqual("200.201.202.203");
                viewModel.DhcpInfo.DomainName.ShouldEqual("my-domain-name");

                Eventually.ShouldBecomeTrue(() => viewModel.ExternalAddress != null, DefaultTimeout);
                viewModel.ExternalAddressTaskStatus.ShouldEqual(TaskStatus.RanToCompletion);
                viewModel.ExternalAddress.ToString().ShouldEqual("50.51.52.53");

                Eventually.ShouldBecomeTrue(() => viewModel.ScanCommand.CanExecute(null), DefaultTimeout);
                Eventually.ShouldBecomeFalse(() => viewModel.CancelScanCommand.CanExecute(null), DefaultTimeout);
                Eventually.ShouldBecomeTrue(() => viewModel.ShowServicesCommand.CanExecute(null), DefaultTimeout);

                viewModel.AvailableServices.ShouldBeEmpty();
                viewModel.ProgressPercent.ShouldEqual(0.0d);
                viewModel.Status.ShouldEqual(String.Empty);

                viewModel.CustomSelectionCommands.Count.ShouldEqual(1);
                viewModel.CustomSelectionCommands.First().Label.ShouldEqual("select whole network");

                viewModel.StartAddressValue.ShouldEqual(IPAddress.Parse("22.33.44.01").Value);
                viewModel.IsStartAddressValid.ShouldBeTrue();
                viewModel.EndAddressValue.ShouldEqual(IPAddress.Parse("22.33.44.254").Value);
                viewModel.IsEndAddressValid.ShouldBeTrue();
            }

            private static MockServiceProvider CreateServiceProvider()
            {
                var serviceProviderOptions = new MockServiceProviderOptions
                {
                    SettingsPropertyStore = new DictionaryPropertyStore(),
                    HostInfo = new HostInfo(Guid.NewGuid(), new HostName("myhost"), IPAddress.Parse("11.22.33.44")),
                    ConnectionInfo = new ConnectionInfo(IPAddress.Parse("22.33.44.55"), new IPNetwork(IPAddress.Parse("22.33.44.00"), 24)),
                    DhcpInfo = new DhcpInfo(IPAddress.Parse("100.101.102.103"), IPAddress.Parse("200.201.202.203"), "my-domain-name"),
                    ExternalAddress = IPAddress.Parse("50.51.52.53")
                };
                var serviceProvider = new MockServiceProvider(serviceProviderOptions);
                return serviceProvider;
            }
        }
    }
}
