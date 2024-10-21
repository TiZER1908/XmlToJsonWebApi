using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XmlToJsonWebApi.Data.Model
{
    public class Dictionary : BaseEntity
    {
        [Column(TypeName = "date")]
        public DateTime BeginDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime EndDate { get; set; }

        public string Code { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string Comments { get; set; } = string.Empty;
    }
}
