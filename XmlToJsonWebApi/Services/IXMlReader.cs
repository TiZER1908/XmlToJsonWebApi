namespace XmlToJsonWebApi.Services
{
    public interface IXMlReader
    {
        List<DictionaryBaseType> ReadFromXml(string filePath);
    }
}
