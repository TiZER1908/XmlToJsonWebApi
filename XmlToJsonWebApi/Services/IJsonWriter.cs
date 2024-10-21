namespace XmlToJsonWebApi.Services
{
    public interface IJsonWriter
    {
        bool WriteToJson(List<DictionaryBaseType> dictionary, string outputPath);
    }
}
