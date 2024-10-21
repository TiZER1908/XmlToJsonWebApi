using XmlToJsonWebApi.Data;
using XmlToJsonWebApi.Share;
using XmlToJsonWebApi.Data.Model;


namespace XmlToJsonWebApi.Repositories
{
    public interface IDictionaryRepository : IRepository<Dictionary>, IDisposable
    {
    }
    public class DictionaryRepository : GenericRepository<Dictionary>, IDictionaryRepository
    {
        public DictionaryRepository(XmlToJsonDBContext contextFactory) 
            : base(contextFactory) { }
    }
}
