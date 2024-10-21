using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace XmlToJsonWebApi.Data.Model
{
    public class BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        [Column(TypeName = "date")]
        public DateTime CreateDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime EditDate { get; set; }

        public int DeletedDictId { get; set; }
        public bool IsDeleted { get; set; }

        [Column(TypeName = "date")]
        public DateTime DeleteDate { get; set; }
        public BaseEntity()
        {
            CreateDate = DateTime.Today;
            EditDate = DateTime.Today;
            DeleteDate = DateTime.MaxValue;
            IsDeleted = false;
        }
    }
}
