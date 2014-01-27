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

    public static class On_V1DisabledServices
    {
        [TestClass]
        public sealed class When_I_pass_null_to_TryCreateFromXml : BddTestBase
        {
            private Exception m_exception;
            private V1DisabledServices m_output;

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
                m_exception = Catch(() => V1DisabledServices.TryCreateFromXml(null, out m_output));
            }
        }

        [TestClass]
        public sealed class When_I_pass_empty_to_TryCreateFromXml : BddTestBase
        {
            private bool m_result;
            private V1DisabledServices m_output;

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
                m_result = V1DisabledServices.TryCreateFromXml(String.Empty, out m_output);
            }
        }

        [TestClass]
        public abstract class TryCreateFromXmlTestBase : BddTestBase
        {
            private bool m_result;
            private V1DisabledServices m_output;

            [TestMethod]
            public void Result_should_be_true()
            {
                m_result.ShouldBeTrue();
            }

            [TestMethod]
            public void GUIDs_should_match_expected()
            {
                m_output.Guids.ShouldEqual(GetExpectedGuids());
            }

            protected abstract string Xml { get; }

            protected abstract Guid[] GetExpectedGuids();

            protected override void BecauseOf()
            {
                m_result = V1DisabledServices.TryCreateFromXml(Xml, out m_output);
            }
        }

        [TestClass]
        public sealed class When_I_pass_valid_XML : TryCreateFromXmlTestBase
        {
            protected override string Xml
            {
                get
                {
                    return @"<DisabledServices>
  <Guids>
     <Guid>c4c826a9-6b89-449f-a06f-e5153e5e587d</Guid>
     <Guid>c4c826a9-6b89-449f-a06f-e5153e5e587e</Guid>
     <Guid>c4c826a9-6b89-449f-a06f-e5153e5e587f</Guid>
  </Guids>
</DisabledServices>";
                }
            }

            protected override Guid[] GetExpectedGuids()
            {
                return new[]
                {
                    Guid.Parse("c4c826a9-6b89-449f-a06f-e5153e5e587d"),
                    Guid.Parse("c4c826a9-6b89-449f-a06f-e5153e5e587e"),
                    Guid.Parse("c4c826a9-6b89-449f-a06f-e5153e5e587f")
                };
            }
        }

        [TestClass]
        public sealed class When_I_pass_valid_XML_with_no_GUIDs : TryCreateFromXmlTestBase
        {
            protected override string Xml
            {
                get
                {
                    return @"<DisabledServices>
  <Guids>
  </Guids>
</DisabledServices>";
                }
            }

            protected override Guid[] GetExpectedGuids()
            {
                return new Guid[0];
            }
        }

        [TestClass]
        public abstract class InvalidTryCreateFromXmlTestBase : BddTestBase
        {
            private bool m_result;
            private V1DisabledServices m_output;

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
                m_result = V1DisabledServices.TryCreateFromXml(Xml, out m_output);
            }
        }

        [TestClass]
        public sealed class When_I_pass_valid_XML_with_no_GUIDs_element : InvalidTryCreateFromXmlTestBase
        {
            protected override string Xml
            {
                get
                {
                    return @"<DisabledServices>
</DisabledServices>";
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
            private const string Xml = @"<DisabledServices>
  <Guids>
     <Guid>c4c826a9-6b89-449f-a06f-e5153e5e587d</Guid>
     <Guid>c4c826a9-6b89-449f-a06f-e5153e5e587e</Guid>
     <Guid>c4c826a9-6b89-449f-a06f-e5153e5e587f</Guid>
  </Guids>
</DisabledServices>";
            private static readonly V1ServiceComparer Comparer = new V1ServiceComparer();
            private V1DisabledServices m_output;

            [TestMethod]
            public void Output_should_not_be_null()
            {
                m_output.ShouldNotBeNull();
            }

            [TestMethod]
            public void Services_should_match_expected()
            {
                V1DisabledServices temp;
                V1DisabledServices.TryCreateFromXml(Xml, out temp);
                m_output.Guids.ShouldEqual(temp.Guids);
            }

            protected override void BecauseOf()
            {
                V1DisabledServices source;
                V1DisabledServices.TryCreateFromXml(Xml, out source);
                var temp = source.ToXml();
                V1DisabledServices.TryCreateFromXml(temp, out m_output);
            }
        }
    }
}
