using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppPollRSSFeed.Models
{
    public class RecipientsListSingleton
    {
        private static List<string> Recipients = new List<string>();

        public static List<string> GetInstance()
        {
            return Recipients;
        }
    }
}
