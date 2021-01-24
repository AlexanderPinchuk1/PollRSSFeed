using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppPollRSSFeed.Models
{
    public class Letter
    {
        public string Message { get; set; }
        public string Recipient { get; set; }

        public Letter(string message, string recipient)
        {
            Message = message;
            Recipient = recipient;
        }
    }
}
