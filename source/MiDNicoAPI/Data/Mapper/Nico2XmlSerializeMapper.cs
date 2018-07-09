using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace MiDNicoAPI.Data.Mapper
{
    public class Nico2XmlSerializeMapper<T> : IMapper<T>
        where T : class
    {
        private readonly XmlSerializer _serializer;
        public Nico2XmlSerializeMapper()
        {
            _serializer = new XmlSerializer(typeof(T));
        }

        public T Map (
            string xml
        )
        {
            var bytes = Encoding.UTF8.GetBytes(xml);
            using (var stream = new MemoryStream(bytes))
            {
                try
                {
                    var serializer = new XmlSerializer(typeof(T));
                    var entity = serializer.Deserialize(stream) as T;
                    return entity;
                }
                catch { return null; }
            }
        }

        public T Map (
            Stream content
        )
        {
            try
            {
                var serializer = new XmlSerializer(typeof(T));
                var entity = serializer.Deserialize(content) as T;
                return entity;
            }
            catch { return null; }
        }
    }
}
