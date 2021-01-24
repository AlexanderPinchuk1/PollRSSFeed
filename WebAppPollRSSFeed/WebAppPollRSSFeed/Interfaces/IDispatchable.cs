using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppPollRSSFeed.Interfaces
{
    public interface IDispatchable
    {
        public void SendMessage(string text, string recipient);
    }
}
