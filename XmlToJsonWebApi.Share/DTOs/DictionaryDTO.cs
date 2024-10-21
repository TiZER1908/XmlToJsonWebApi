using System.ComponentModel.DataAnnotations;

namespace XmlToJsonWebApi.Share.DTOs
{
    public class DictionaryDTO : BaseModel
    {
        [DisplayFormat(DataFormatString = "{0:d}")]
        [Display(Name = "Начало")]
        public DateTime BeginDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:d}")]
        [Display(Name = "Окончание")]
        public DateTime EndDate { get; set; }
        [Display(Name = "Код")]
        public string Code { get; set; } = string.Empty;
        [Display(Name = "Наименование")]
        public string Name { get; set; } = string.Empty;
        public string Comments { get; set; } = string.Empty;
        public DictionaryDTO()
        {
            BeginDate = DateTime.MaxValue;
            EndDate = DateTime.MaxValue;
        }
    }
}
