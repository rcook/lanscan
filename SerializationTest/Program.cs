namespace SerializationTest
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Text;
    using System.Xml;

    public interface IServiceRegistryService
    {
        IEnumerable<ServiceDefinition> WellKnownServiceDefinitions { get; }
    }

    public static class SerializationConstants
    {
        public const string EmptyNamespace = "";
        public const string ServiceConfiguration = "ServiceConfiguration";
        public const string WellKnownServiceDefinitions = "PredefinedServiceDefinitions";
        public const string UserServiceDefinitions = "UserDefinedServiceDefinitions";
        public const string ServiceDefinition = "ServiceDefinition";
        public const string Guid = "Guid";
        public const string Name = "Name";
        public const string Description = "Description";
        public const string Protocol = "Protocol";
        public const string Port = "Port";
    }

    public enum Protocol
    {
        Tcp,
        Udp
    }

    [DataContract(Name = SerializationConstants.ServiceDefinition, Namespace = SerializationConstants.EmptyNamespace)]
    public sealed class ServiceDefinition
    {
        [DataMember(Name = SerializationConstants.Guid)]
        private readonly Guid m_guid;

        [DataMember(Name = SerializationConstants.Name)]
        private readonly string m_name;

        [DataMember(Name = SerializationConstants.Description)]
        private readonly string m_description;

        [DataMember(Name = SerializationConstants.Protocol)]
        private readonly Protocol m_protocol;

        [DataMember(Name = SerializationConstants.Port)]
        private readonly uint m_port;

        public ServiceDefinition(Guid guid, string name, string description, Protocol protocol, uint port)
        {
            if (String.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Invalid name", "name");
            }
            if (String.IsNullOrWhiteSpace(description))
            {
                throw new ArgumentException("Invalid description", "description");
            }

            m_guid = guid;
            m_name = name;
            m_description = description;
            m_protocol = protocol;
            m_port = port;
        }

        public Guid Guid
        {
            get { return m_guid; }
        }

        public string Name
        {
            get { return m_name; }
        }

        public string Description
        {
            get { return m_description; }
        }

        public Protocol Protocol
        {
            get { return m_protocol; }
        }

        public uint Port
        {
            get { return m_port; }
        }
    }

    public sealed class ServiceSetting
    {
        private readonly Guid m_serviceDefinitionGuid;
        private readonly bool m_isEnabled;

        public ServiceSetting(Guid serviceDefinitionGuid, bool isEnabled)
        {
            m_serviceDefinitionGuid = serviceDefinitionGuid;
            m_isEnabled = isEnabled;
        }
    }

    [DataContract(Name = SerializationConstants.ServiceConfiguration, Namespace = SerializationConstants.EmptyNamespace)]
    public sealed class ServiceConfiguration
    {
        private static readonly DataContractSerializer Serializer = new DataContractSerializer(typeof(ServiceConfiguration));

        [DataMember(Name = SerializationConstants.WellKnownServiceDefinitions)]
        private readonly ServiceDefinition[] m_wellKnownServiceDefinitions;

        [DataMember(Name = SerializationConstants.UserServiceDefinitions)]
        private readonly List<ServiceDefinition> m_userServiceDefinitions = new List<ServiceDefinition>();

        private ServiceConfiguration(ServiceDefinition[] wellKnownServiceDefinitions)
        {
            if (wellKnownServiceDefinitions == null)
            {
                throw new ArgumentNullException("wellKnownServiceDefinitions");
            }

            m_wellKnownServiceDefinitions = wellKnownServiceDefinitions;
        }

        public IEnumerable<ServiceDefinition> WellKnownServiceDefinitions
        {
            get
            {
                return m_wellKnownServiceDefinitions
                    .OrderBy(x => x.Port)
                    .ThenBy(x => x.Protocol)
                    .ThenBy(x => x.Name)
                    .ThenBy(x => x.Description)
                    .ThenBy(x => x.Guid);
            }
        }

        public IEnumerable<ServiceDefinition> UserServiceDefinitions
        {
            get
            {
                return m_userServiceDefinitions
                    .OrderBy(x => x.Port)
                    .ThenBy(x => x.Protocol)
                    .ThenBy(x => x.Name)
                    .ThenBy(x => x.Description)
                    .ThenBy(x => x.Guid);
            }
        }

        public static ServiceConfiguration CreateDefault(IServiceRegistryService serviceRegistryService)
        {
            if (serviceRegistryService == null)
            {
                throw new ArgumentNullException("serviceRegistryService");
            }

            var wellKnownServiceDefinitions = serviceRegistryService.WellKnownServiceDefinitions.ToArray();
            return new ServiceConfiguration(wellKnownServiceDefinitions);
        }

        public string ToXml()
        {
            var output = new StringBuilder();
            var settings = new XmlWriterSettings
            {
                Indent = true,
                IndentChars = "  "
            };
            using (var writer = XmlWriter.Create(output, settings))
            {
                Serializer.WriteObject(writer, this);
            }

            var result = output.ToString();
            return result;
        }

        public void RegisterUserServiceDefinition(string name, string description, Protocol protocol, uint port)
        {
            if (String.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Invalid name", "name");
            }
            if (String.IsNullOrWhiteSpace(description))
            {
                throw new ArgumentException("Invalid description", "description");
            }

            var guid = Guid.NewGuid();
            var serviceDefinition = new ServiceDefinition(guid, name, description, protocol, port);
            m_userServiceDefinitions.Add(serviceDefinition);
        }
    }

    public static class Program
    {
        public static void Main(string[] args)
        {
            //var config = ServiceConfiguration.CreateDefault(new ServiceRegistryService());
            //config.RegisterUserServiceDefinition("MyProtocol1", "MyProtocol1 description", Protocol.Tcp, 1234);
            //config.RegisterUserServiceDefinition("MyProtocol2", "MyProtocol2 description", Protocol.Tcp, 2345);
            //Console.WriteLine(config.ToXml());
        }

        private sealed class ServiceRegistryService : IServiceRegistryService
        {
            public ServiceRegistryService()
            {
            }

            public IEnumerable<ServiceDefinition> WellKnownServiceDefinitions
            {
                get { return ServiceRegistry.PredefinedServiceDefinitions; }
            }

            private static class ServiceRegistry
            {
                private static readonly Dictionary<Guid, ServiceDefinition> PredefinedServiceDefinitionsInternal = new Dictionary<Guid, ServiceDefinition>();

                static ServiceRegistry()
                {
                    RegisterPredefinedService("8dfc9610-d988-4d39-b306-000000000003", "Skype", "Skype description", Protocol.Udp, 23399);
                    RegisterPredefinedService("8dfc9610-d988-4d39-b306-000000000000", "SSH", "SSH description", Protocol.Tcp, 22);
                    RegisterPredefinedService("8dfc9610-d988-4d39-b306-000000000002", "HTTPS", "HTTPS description", Protocol.Tcp, 443);
                    RegisterPredefinedService("8dfc9610-d988-4d39-b306-000000000001", "HTTP", "HTTP description", Protocol.Tcp, 80);
                }

                public static IEnumerable<ServiceDefinition> PredefinedServiceDefinitions
                {
                    get
                    {
                        return PredefinedServiceDefinitionsInternal
                            .Values
                            .OrderBy(x => x.Port)
                            .ThenBy(x => x.Protocol)
                            .ThenBy(x => x.Name)
                            .ThenBy(x => x.Description)
                            .ThenBy(x => x.Guid);
                    }
                }

                private static void RegisterPredefinedService(string guidString, string name, string description, Protocol protocol, uint port)
                {
                    var guid = Guid.Parse(guidString);
                    PredefinedServiceDefinitionsInternal.Add(guid, new ServiceDefinition(guid, name, description, protocol, port));
                }
            }
        }
    }
}
