//////////////////////////////////////////////////////////////////////
//
// Lanscan
// Copyright (C) 2013, Richard Cook. All rights reserved.
//
//////////////////////////////////////////////////////////////////////

namespace Lanscan.DataContracts
{
    using System;
    using System.IO;
    using System.Runtime.Serialization;
    using System.Text;
    using System.Xml;

    public static class SerializationHelper
    {
        public static string ToXml(DataContractSerializer serializer, object graph)
        {
            if (serializer == null)
            {
                throw new ArgumentNullException("serializer");
            }
            if (graph == null)
            {
                throw new ArgumentNullException("graph");
            }

            var output = new StringBuilder();
            var settings = new XmlWriterSettings
            {
                Indent = true,
                IndentChars = "  "
            };
            using (var xmlWriter = XmlWriter.Create(output, settings))
            {
                serializer.WriteObject(xmlWriter, graph);
            }

            var result = output.ToString();
            return result;
        }

        public static bool TryCreateFromXml<T>(DataContractSerializer serializer, string xml, out T output)
        {
            output = default(T);

            if (serializer == null)
            {
                throw new ArgumentNullException("serializer");
            }
            if (xml == null)
            {
                throw new ArgumentNullException("xml");
            }

            T temp;
            using (var reader = new StringReader(xml))
            using (var xmlReader = XmlReader.Create(reader))
            {
                if (!TryReadObject(serializer, xmlReader, out temp))
                {
                    return false;
                }
            }

            output = temp;
            return true;
        }

        private static bool TryReadObject<T>(DataContractSerializer serializer, XmlReader xmlReader, out T output)
        {
            output = default(T);

            object temp;
            try
            {
                temp = serializer.ReadObject(xmlReader);
            }
            catch (SerializationException)
            {
                return false;
            }

            output = (T)temp;
            return true;
        }
    }
}
