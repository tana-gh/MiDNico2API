using System.IO;

namespace MiDNicoAPI.Data.Mapper
{
    internal interface IMapper<TEntity>
         where TEntity : class
    {
        TEntity Map(string content);
        TEntity Map(Stream content);
    }
}
