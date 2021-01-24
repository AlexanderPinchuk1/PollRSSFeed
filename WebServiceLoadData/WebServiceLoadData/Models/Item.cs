using WebServiceLoadData.Attributes;

namespace WebServiceLoadData.Models
{
    public class Item
    {
        [XmlNode("title")]
        public string Title { get; set; } = "";
        [XmlNode("link")]
        public string Link { get; set; } = "";
        [XmlNode("description")]
        public string Description { get; set; } = "";
        [XmlNode("pubDate")]
        public string PubDate { get; set; } = "";
    }
}