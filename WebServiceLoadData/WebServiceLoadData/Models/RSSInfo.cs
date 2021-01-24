using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebServiceLoadData.Models
{
    public class RSSInfo
    {
        public Channel Channel { get; set; } = new Channel();
        public List<Item> Items { get; set; } = new List<Item>();

        public RSSInfo()
        {
        }

        public RSSInfo(Channel channel, List<Item> items)
        {
            Channel = channel;
            Items = items;
        }
    }
}