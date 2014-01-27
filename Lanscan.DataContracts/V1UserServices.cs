//////////////////////////////////////////////////////////////////////
//
// Lanscan
// Copyright (C) 2013, Richard Cook. All rights reserved.
//
//////////////////////////////////////////////////////////////////////

namespace Lanscan.DataContracts
{
    using System;
    using System.Runtime.Serialization;

    [DataContract(Name = V1Constants.UserServices, Namespace = V1Constants.EmptyNamespace)]
    public sealed class V1UserServices
    {
        private static readonly DataContractSerializer Serializer = new DataContractSerializer(typeof(V1UserServices));

        [DataMember(Name = V1Constants.Services, IsRequired = true)]
        private readonly V1ServiceCollection m_services = new V1ServiceCollection();

        public V1UserServices()
        {
        }

        public V1ServiceCollection Services
        {
            get { return m_services; }
        }

        public static bool TryCreateFromXml(string xml, out V1UserServices output)
        {
            output = default(V1UserServices);

            if (xml == null)
            {
                throw new ArgumentNullException("xml");
            }

            V1UserServices temp;
            if (!SerializationHelper.TryCreateFromXml<V1UserServices>(Serializer, xml, out temp))
            {
                return false;
            }

            output = temp;
            return true;
        }

        public string ToXml()
        {
            var result = SerializationHelper.ToXml(Serializer, this);
            return result;
        }
    }
}
