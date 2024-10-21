using System.Text.Encodings.Web;
using System.Text.Json;
using XmlToJsonWebApi.Services;
using System.Text.Json.Serialization;


namespace XmlToJsonWebApi
{
    public class JsonWriter : IJsonWriter
    {
        public bool WriteToJson(List<DictionaryBaseType> dictionary, string outputPath)
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                };

                string result = JsonSerializer.Serialize(dictionary, options);

                return true;
            }
            catch 
            { 
                return false; 
            }
            
        }
    }
}
