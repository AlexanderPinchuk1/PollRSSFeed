using System.Collections.Generic;
using System.Web.Services;


namespace WebServiceLoadData
{
    /// <summary>
    /// Сводное описание для WebServiceLoadData
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Чтобы разрешить вызывать веб-службу из скрипта с помощью ASP.NET AJAX, раскомментируйте следующую строку. 
    // [System.Web.Script.Services.ScriptService]
    public class WebServiceLoadData : System.Web.Services.WebService
    {

        [WebMethod]
        public List<string> LoadData(string url)
        {
            DataRetrieval dataRetrieval = new DataRetrieval();
            MessageCreator messageCreator = new MessageCreator();

            return messageCreator.CreateMessage(dataRetrieval.GetMessage(url)); ;
        }
    }
}
