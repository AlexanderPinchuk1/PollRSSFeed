using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace WebServiceMail
{
    /// <summary>
    /// Сводное описание для WebServiceMail
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Чтобы разрешить вызывать веб-службу из скрипта с помощью ASP.NET AJAX, раскомментируйте следующую строку. 
    // [System.Web.Script.Services.ScriptService]
    public class WebServiceMail : System.Web.Services.WebService
    {

        [WebMethod]
        public void SendMessage(string text, string recipient)
        {
            Mail mail = new Mail();
            mail.SendMessage(text, recipient);
        }
    }
}
