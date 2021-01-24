using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppPollRSSFeed.Models
{
    public static class UrlListSingleton
    {
        private static List<string> Urls = new List<string>();

        public static List<string> GetInstance()
        {
            return Urls;
        }
    }
}
