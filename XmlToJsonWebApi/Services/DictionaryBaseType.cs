using System.ComponentModel.DataAnnotations;

namespace XmlToJsonWebApi
{
    public class DictionaryBaseType
    {
        [Display(Name = "Код")]
        public string Code { get; set; } = string.Empty;

        [Display(Name = "Начало")]
        public DateTime BeginDate { get; set; }

        [Display(Name = "Окончание")]
        public DateTime EndDate { get; set; }

        [Display(Name = "Наименование")]
        public string Name { get; set; } = string.Empty;

        public DictionaryBaseType()
        {
        }
    }
}
