using WebServiceLoadData.Attributes;

namespace WebServiceLoadData.Models
{
    public class Channel
    {
        [XmlNode("title")]
        public string Title { get; set; } = "";
        [XmlNode("description")]
        public string Description { get; set; } = "";
        [XmlNode("link")]
        public string Link { get; set; } = "";
        [XmlNode("copyright")]
        public string Copyright { get; set; } = "";
        [XmlNode("language")]
        public string Language { get; set; } = "";
    }
}