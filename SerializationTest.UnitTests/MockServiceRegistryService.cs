namespace SerializationTest.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public sealed class MockServiceRegistryService : IServiceRegistryService
    {
        private static readonly Dictionary<Guid, ServiceDefinition> WellKnownServiceDefinitionsInternal = new Dictionary<Guid, ServiceDefinition>();

        static MockServiceRegistryService()
        {
            RegisterWellKnownServiceDefinition("8dfc9610-d988-4d39-b306-000000000003", "Skype", "Skype description", Protocol.Udp, 23399);
            RegisterWellKnownServiceDefinition("8dfc9610-d988-4d39-b306-000000000000", "SSH", "SSH description", Protocol.Tcp, 22);
            RegisterWellKnownServiceDefinition("8dfc9610-d988-4d39-b306-000000000002", "HTTPS", "HTTPS description", Protocol.Tcp, 443);
            RegisterWellKnownServiceDefinition("8dfc9610-d988-4d39-b306-000000000001", "HTTP", "HTTP description", Protocol.Tcp, 80);
        }

        public MockServiceRegistryService()
        {
        }

        public IEnumerable<ServiceDefinition> WellKnownServiceDefinitions
        {
            get
            {
                return WellKnownServiceDefinitionsInternal
                    .Values
                    .OrderBy(x => x.Port)
                    .ThenBy(x => x.Protocol)
                    .ThenBy(x => x.Name)
                    .ThenBy(x => x.Description)
                    .ThenBy(x => x.Guid);
            }
        }

        private static void RegisterWellKnownServiceDefinition(string guidString, string name, string description, Protocol protocol, uint port)
        {
            var guid = Guid.Parse(guidString);
            WellKnownServiceDefinitionsInternal.Add(guid, new ServiceDefinition(guid, name, description, protocol, port));
        }
    }
}
