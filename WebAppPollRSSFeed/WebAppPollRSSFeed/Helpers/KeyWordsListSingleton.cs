using System.Collections.Generic;

namespace WebAppPollRSSFeed.Models
{
    public class KeyWordsListSingleton
    {
        private static List<string> KeyWords = new List<string>();

        public static List<string> GetInstance()
        {
            return KeyWords;
        }
    }
}
