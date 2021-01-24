using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using System.Xml;
using WebServiceLoadData.Attributes;
using WebServiceLoadData.Models;

namespace WebServiceLoadData
{
    public class DataRetrieval
    {
        private readonly List<Item> Items;
        public Channel Channel;

        public DataRetrieval()
        {
            Channel = new Channel();
            Items = new List<Item>();
        }

        public RSSInfo GetMessage(string url)
        {
            XmlDocument xmlDocument = GetXmlInfoElseReturnNull(url);

            if (xmlDocument == null)
                return new RSSInfo();

            XmlNode root = xmlDocument.DocumentElement;
            XmlNodeList xmlNodeList = root.ChildNodes;
            foreach (XmlNode channel in xmlNodeList)
            {
                foreach (XmlNode xmlNode in channel)
                {
                    SetInfoAboutNodes(xmlNode, Channel);
                    if (XmlNodeIsItem(xmlNode))
                    {
                        AddItemInfo(xmlNode);
                    }
                }
            }

            return new RSSInfo(Channel, Items);
        }

        private XmlDocument GetXmlInfoElseReturnNull(string url)
        {
            XmlDocument xmlDocument = null;
            try
            {
                XmlTextReader xmlTextReader = new XmlTextReader(GetStreamOfURL(url));
                xmlDocument = new XmlDocument();
                xmlDocument.Load(xmlTextReader);
            }
            catch
            {
            }

            return xmlDocument;
        }

        public Stream GetStreamOfURL(string url)
        {
            WebRequest webRequest = WebRequest.Create(url);
            webRequest.Proxy.Credentials = CredentialCache.DefaultCredentials;

            return webRequest.GetResponse().GetResponseStream();
        }

        private void SetInfoAboutNodes(XmlNode xmlNode, object obj)
        {
            Type objType = obj.GetType();
            PropertyInfo[] objProperties = objType.GetProperties();

            foreach (PropertyInfo propertyInfo in objProperties)
            {
                XmlNodeAttribute attribute = (XmlNodeAttribute)propertyInfo.GetCustomAttribute(typeof(XmlNodeAttribute));

                if ((attribute != null) && (attribute.XmlNode == xmlNode.Name))
                    propertyInfo.SetValue(obj, xmlNode.InnerText);
            }
        }

        private void AddItemInfo(XmlNode xmlNode)
        {
            XmlNodeList itemsList = xmlNode.ChildNodes;
            Items.Add(new Item());

            foreach (XmlNode xmlNodeInItem in itemsList)
            {
                SetInfoAboutNodes(xmlNodeInItem, Items[Items.Count-1]);
            }
        }

        public bool XmlNodeIsItem(XmlNode xmlNode)
        {
            if (xmlNode.Name == "item")
                return true;
            else
                return false;
        }
    }
}