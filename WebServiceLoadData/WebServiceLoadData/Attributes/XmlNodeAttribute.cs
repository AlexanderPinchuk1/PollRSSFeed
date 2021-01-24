using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebServiceLoadData.Attributes
{
    public class XmlNodeAttribute : System.Attribute
    {
        public string XmlNode { get; set; }

        public XmlNodeAttribute(string xmlNode)
        {
            XmlNode = xmlNode;
        }
    }
}