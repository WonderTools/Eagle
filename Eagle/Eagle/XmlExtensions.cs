using System.Xml;
using Newtonsoft.Json;

namespace WonderTools.Eagle.NUnit
{
    public static class XmlExtensions
    {
        public static string ToJson(this XmlNode xmlNode)
        {
            var myData = xmlNode.OuterXml;
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(myData);
            return JsonConvert.SerializeXmlNode(doc, Newtonsoft.Json.Formatting.Indented);
        }
    }
}