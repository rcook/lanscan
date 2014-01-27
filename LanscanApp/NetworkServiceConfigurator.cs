//////////////////////////////////////////////////////////////////////
//
// Lanscan
// Copyright (C) 2013, Richard Cook. All rights reserved.
//
//////////////////////////////////////////////////////////////////////

namespace LanscanApp
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Lanscan.DataContracts;
    using Windows.Storage;

    public sealed class NetworkServiceConfigurator
    {
        private const string V1DisabledServicesFileName = "DisabledServices_v1.xml";
        private const string V1UserServicesFileName = "UserServices_v1.xml";

        public static async Task<PredefinedNetworkService[]> LoadPredefinedNetworkServicesAsync()
        {
            var disabledServices = await ReadDisabledServicesAsync();
            var services = NetworkServiceRegistry.Entries.Select(x =>
            {
                var isEnabled = disabledServices == null || !disabledServices.Guids.Contains(x.Guid);
                var service = new PredefinedNetworkService(x, isEnabled);
                return service;
            });
            var result = services.ToArray();
            return result;
        }

        public static async Task<UserNetworkService[]> LoadUserNetworkServicesAsync()
        {
            var userServices = await ReadUserServicesAsync();
            if (userServices == null)
            {
                return new UserNetworkService[0];
            }

            var services = userServices.Services.Select(x => new UserNetworkService(x, x.IsEnabled));
            var result = services.ToArray();
            return result;
        }

        public static async Task WriteDisabledServicesAsync(V1DisabledServices input)
        {
            if (input == null)
            {
                throw new ArgumentNullException("input");
            }

            var file = await ApplicationData.Current.LocalFolder.CreateFileAsync(V1DisabledServicesFileName, CreationCollisionOption.OpenIfExists);
            await FileIO.WriteTextAsync(file, input.ToXml());
        }

        public static async Task WriteUserServicesAsync(V1UserServices input)
        {
            if (input == null)
            {
                throw new ArgumentNullException("input");
            }

            var file = await ApplicationData.Current.LocalFolder.CreateFileAsync(V1UserServicesFileName, CreationCollisionOption.OpenIfExists);
            await FileIO.WriteTextAsync(file, input.ToXml());
        }

        private static async Task<V1DisabledServices> ReadDisabledServicesAsync()
        {
            var file = await ApplicationData.Current.LocalFolder.CreateFileAsync(V1DisabledServicesFileName, CreationCollisionOption.OpenIfExists);
            var xml = await FileIO.ReadTextAsync(file);
            V1DisabledServices output;
            V1DisabledServices.TryCreateFromXml(xml, out output);
            return output;
        }

        private static async Task<V1UserServices> ReadUserServicesAsync()
        {
            var file = await ApplicationData.Current.LocalFolder.CreateFileAsync(V1UserServicesFileName, CreationCollisionOption.OpenIfExists);
            var xml = await FileIO.ReadTextAsync(file);
            V1UserServices output;
            V1UserServices.TryCreateFromXml(xml, out output);
            return output;
        }
    }
}
