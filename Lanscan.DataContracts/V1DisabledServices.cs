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

    [DataContract(Name = V1Constants.DisabledServices, Namespace = V1Constants.EmptyNamespace)]
    public sealed class V1DisabledServices
    {
        private static readonly DataContractSerializer Serializer = new DataContractSerializer(typeof(V1DisabledServices));

        [DataMember(Name = V1Constants.Guids, IsRequired = true)]
        private readonly V1GuidCollection m_guids = new V1GuidCollection();

        public V1DisabledServices()
        {
        }

        public V1GuidCollection Guids
        {
            get { return m_guids; }
        }

        public static bool TryCreateFromXml(string xml, out V1DisabledServices output)
        {
            output = default(V1DisabledServices);

            if (xml == null)
            {
                throw new ArgumentNullException("xml");
            }

            V1DisabledServices temp;
            if (!SerializationHelper.TryCreateFromXml<V1DisabledServices>(Serializer, xml, out temp))
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
