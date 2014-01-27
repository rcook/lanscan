namespace SerializationTest.UnitTests
{
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    public static class On_ServiceConfiguration
    {
        //[TestClass]
        //[Ignore]
        public sealed class ServiceConfiguration_Test
        {
            //[TestMethod]
            public void TestSimple()
            {
                var serviceRegistryService = new MockServiceRegistryService();
                var serviceConfiguration = ServiceConfiguration.CreateDefault(serviceRegistryService);
                var wellKnownServiceDefinitions = serviceConfiguration.WellKnownServiceDefinitions.ToArray();
                TestHelper.AssertSequencesAreEqual(new[] { 22u, 80u, 443u, 23399u }, wellKnownServiceDefinitions.Select(x => x.Port));
                TestHelper.AssertSequencesAreEqual(new[] { "SSH", "HTTP", "HTTPS", "Skype" }, wellKnownServiceDefinitions.Select(x => x.Name));
                var userServiceDefinitions = serviceConfiguration.UserServiceDefinitions.ToArray();
                TestHelper.AssertSequencesAreEqual(new ServiceDefinition[0], userServiceDefinitions);

                serviceConfiguration.RegisterUserServiceDefinition("MyProtocol1", "MyProtocol1Description", Protocol.Tcp, 2345u);
                serviceConfiguration.RegisterUserServiceDefinition("MyProtocol2", "MyProtocol2Description", Protocol.Udp, 1234u);
                wellKnownServiceDefinitions = serviceConfiguration.WellKnownServiceDefinitions.ToArray();
                TestHelper.AssertSequencesAreEqual(new[] { 22u, 80u, 443u, 23399u }, wellKnownServiceDefinitions.Select(x => x.Port));
                TestHelper.AssertSequencesAreEqual(new[] { "SSH", "HTTP", "HTTPS", "Skype" }, wellKnownServiceDefinitions.Select(x => x.Name));
                userServiceDefinitions = serviceConfiguration.UserServiceDefinitions.ToArray();
                TestHelper.AssertSequencesAreEqual(new[] { 1234u, 2345u }, userServiceDefinitions.Select(x => x.Port));
                TestHelper.AssertSequencesAreEqual(new[] { "MyProtocol2", "MyProtocol1" }, userServiceDefinitions.Select(x => x.Name));
            }
        }
    }
}
