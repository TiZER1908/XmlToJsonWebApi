using Microsoft.EntityFrameworkCore;
using XmlToJsonWebApi.Data.Model;

namespace XmlToJsonWebApi.Data
{
    public class XmlToJsonDBContext : DbContext
    {
        public XmlToJsonDBContext(DbContextOptions<XmlToJsonDBContext> options)
            : base(options) { }
        public DbSet<Dictionary> Dictionaries { get; set; }
    }
}
