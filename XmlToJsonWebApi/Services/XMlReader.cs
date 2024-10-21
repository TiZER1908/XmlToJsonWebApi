using System.Text;
using System.Xml.Linq;

namespace XmlToJsonWebApi.Services
{
    public class XMlReader : IXMlReader
    {
        public List<DictionaryBaseType> ReadFromXml(string filePath)
        {
            var listzap = new List<DictionaryBaseType>();

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            XDocument xml = XDocument.Load(filePath);

            XElement? packetxml = xml.Element("packet");

            string format = "dd.mm.yyyy";

            if (packetxml is not null)
            {
                foreach (XElement zapxml in packetxml.Elements("zap"))
                {
                    DictionaryBaseType zap = new DictionaryBaseType();
                    XElement? idump = zapxml.Element("IDUMP");
                    XElement? datebeg = zapxml.Element("DATEBEG");
                    XElement? dateend = zapxml.Element("DATEEND");
                    XElement? umpname = zapxml.Element("UMPNAME");
                    if (idump is not null)
                    {
                        zap.Code = idump.Value;
                    }
                    if (datebeg is not null)
                    {
                        if (datebeg.Value != "")
                        {
                            zap.BeginDate = DateTime.ParseExact(datebeg.Value, format, null);
                        }
                        else
                        {
                            zap.BeginDate = DateTime.MaxValue;
                        }
                    }
                    if (dateend is not null)
                    {
                        if (dateend.Value != "")
                        {
                            zap.EndDate = DateTime.ParseExact(dateend.Value, format, null);
                        }
                        else
                        {
                            zap.EndDate = DateTime.MaxValue;
                        }
                    }
                    if (umpname is not null)
                    {
                        zap.Name = umpname.Value;
                    }
                    listzap.Add(zap);
                }
            }
            return listzap;
        }
    }
}
