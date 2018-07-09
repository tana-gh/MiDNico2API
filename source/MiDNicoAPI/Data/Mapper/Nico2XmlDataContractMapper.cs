using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MiDNicoAPI.Data.Mapper
{
    public class Nico2XmlDataContractMapper<T> : IMapper<T>
        where T : class
    {
        private readonly DataContractSerializer _serializer;
        public Nico2XmlDataContractMapper()
        {
            _serializer = new DataContractSerializer(typeof(T));
        }

        public T Map (
            string xml
        )
        {
            XmlReader xmlReader = XmlReader.Create(new StringReader(xml));
            T obj = (T)_serializer.ReadObject(xmlReader);
            return obj;
        }

        public T Map (
            Stream content
        )
        {
            XmlReader xmlReader = XmlReader.Create(content);
            T obj = (T)_serializer.ReadObject(xmlReader);
            return obj;
        }
    }
}
