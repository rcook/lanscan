//////////////////////////////////////////////////////////////////////
//
// Lanscan
// Copyright (C) 2013, Richard Cook. All rights reserved.
//
//////////////////////////////////////////////////////////////////////

namespace Lanscan.DataContracts.UnitTests
{
    using System;
    using Lanscan.TestFramework;
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

    public static class On_V1UserServices
    {
        [TestClass]
        public sealed class When_I_pass_null_to_TryCreateFromXml : BddTestBase
        {
            private Exception m_exception;
            private V1UserServices m_output;

            [TestMethod]
            public void Exception_should_be_of_expected_type()
            {
                m_exception.ShouldBeOfType<ArgumentNullException>();
            }

            [TestMethod]
            public void Output_should_be_null()
            {
                m_output.ShouldBeNull();
            }

            protected override void BecauseOf()
            {
                m_exception = Catch(() => V1UserServices.TryCreateFromXml(null, out m_output));
            }
        }

        [TestClass]
        public sealed class When_I_pass_empty_to_TryCreateFromXml : BddTestBase
        {
            private bool m_result;
            private V1UserServices m_output;

            [TestMethod]
            public void Result_should_be_false()
            {
                m_result.ShouldBeFalse();
            }

            [TestMethod]
            public void Output_should_be_null()
            {
                m_output.ShouldBeNull();
            }

            protected override void BecauseOf()
            {
                m_result = V1UserServices.TryCreateFromXml(String.Empty, out m_output);
            }
        }

        [TestClass]
        public abstract class TryCreateFromXmlTestBase : BddTestBase
        {
            private static readonly V1ServiceComparer Comparer = new V1ServiceComparer();
            private bool m_result;
            private V1UserServices m_output;

            [TestMethod]
            public void Result_should_be_true()
            {
                m_result.ShouldBeTrue();
            }

            [TestMethod]
            public void Services_should_match_expected()
            {
                m_output.Services.ShouldEqual(GetExpectedServices(), Comparer);
            }

            protected abstract string Xml { get; }

            protected abstract V1Service[] GetExpectedServices();

            protected override void BecauseOf()
            {
                m_result = V1UserServices.TryCreateFromXml(Xml, out m_output);
            }
        }

        [TestClass]
        public sealed class When_I_pass_valid_XML : TryCreateFromXmlTestBase
        {
            protected override string Xml
            {
                get
                {
                    return @"<UserServices>
  <Services>
    <Service>
      <IsEnabled>true</IsEnabled>
      <Name>MyProtocol1</Name>
      <Port>1234</Port>
      <Protocol>Tcp</Protocol>
    </Service>
    <Service>
      <IsEnabled>false</IsEnabled>
      <Name>MyProtocol2</Name>
      <Port>2345</Port>
      <Protocol>Udp</Protocol>
    </Service>
  </Services>
</UserServices>";
                }
            }

            protected override V1Service[] GetExpectedServices()
            {
                return new[]
                {
                    new V1Service("MyProtocol1", V1Protocol.Tcp, 1234, true),
                    new V1Service("MyProtocol2", V1Protocol.Udp, 2345, false)
                };
            }
        }

        [TestClass]
        public sealed class When_I_pass_valid_XML_with_no_service : TryCreateFromXmlTestBase
        {
            protected override string Xml
            {
                get
                {
                    return @"<UserServices>
  <Services>
  </Services>
</UserServices>";
                }
            }

            protected override V1Service[] GetExpectedServices()
            {
                return new V1Service[0];
            }
        }

        [TestClass]
        public abstract class InvalidTryCreateFromXmlTestBase : BddTestBase
        {
            private bool m_result;
            private V1UserServices m_output;

            [TestMethod]
            public void Result_should_be_false()
            {
                m_result.ShouldBeFalse();
            }

            [TestMethod]
            public void Output_should_be_null()
            {
                m_output.ShouldBeNull();
            }

            protected abstract string Xml { get; }

            protected override void BecauseOf()
            {
                m_result = V1UserServices.TryCreateFromXml(Xml, out m_output);
            }
        }

        [TestClass]
        public sealed class When_I_pass_valid_XML_with_no_services_element : InvalidTryCreateFromXmlTestBase
        {
            protected override string Xml
            {
                get
                {
                    return @"<UserServices>
</UserServices>";
                }
            }
        }

        [TestClass]
        public sealed class When_I_pass_valid_XML_with_garbage : InvalidTryCreateFromXmlTestBase
        {
            protected override string Xml
            {
                get { return "garbage"; }
            }
        }

        [TestClass]
        public sealed class When_I_serialize_and_deserialize : BddTestBase
        {
            private const string Xml = @"<UserServices>
  <Services>
    <Service>
      <IsEnabled>true</IsEnabled>
      <Name>MyProtocol1</Name>
      <Port>1234</Port>
      <Protocol>Tcp</Protocol>
    </Service>
    <Service>
      <IsEnabled>false</IsEnabled>
      <Name>MyProtocol2</Name>
      <Port>2345</Port>
      <Protocol>Udp</Protocol>
    </Service>
  </Services>
</UserServices>";
            private static readonly V1ServiceComparer Comparer = new V1ServiceComparer();
            private V1UserServices m_output;

            [TestMethod]
            public void Output_should_not_be_null()
            {
                m_output.ShouldNotBeNull();
            }

            [TestMethod]
            public void Services_should_match_expected()
            {
                V1UserServices temp;
                V1UserServices.TryCreateFromXml(Xml, out temp);
                m_output.Services.ShouldEqual(temp.Services, Comparer);
            }

            protected override void BecauseOf()
            {
                V1UserServices source;
                V1UserServices.TryCreateFromXml(Xml, out source);
                var temp = source.ToXml();
                V1UserServices.TryCreateFromXml(temp, out m_output);
            }
        }
    }
}
