using System.Collections.Generic;
using System.Web.Services;

namespace WebServiceFilter
{
    /// <summary>
    /// Сводное описание для WebServiceFilter
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Чтобы разрешить вызывать веб-службу из скрипта с помощью ASP.NET AJAX, раскомментируйте следующую строку. 
    // [System.Web.Script.Services.ScriptService]
    public class WebServiceFilter : System.Web.Services.WebService
    {

        [WebMethod]
        public List<string> Filter(List<string> articles, List<string> keyWords)
        {
            Filter filter = new Filter(); 
            return filter.FilterByKeyWords(articles,keyWords);
        }
    }
}
