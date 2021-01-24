using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebServiceFilter
{
    public class Filter
    {
        public List<string> FilterByKeyWords(List<string> articles, List<string> keyWords)
        {
            List<string> result = new List<string>();
            
            foreach (string article in articles)
            {
                foreach (string key in keyWords)
                {
                    if (article.IndexOf(key) != -1)
                    {
                        result.Add(article);
                        break;
                    }
                }                
            }

            return result;
        }
    }
}