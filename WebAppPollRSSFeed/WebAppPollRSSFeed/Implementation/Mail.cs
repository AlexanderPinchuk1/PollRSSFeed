using System.Net;
using System.Net.Mail;
using WebAppPollRSSFeed.Interfaces;

namespace WebAppPollRSSFeed.Implementation
{
    public class Mail : IDispatchable
    {
        private const string SENDER_MAIL = "spplabs@gmail.com";
        private const string SENDER_PASSWORD = "pg$pEoo5PX2T";

        public void SendMessage(string text, string recipient)
        { 
            MailAddress from = new MailAddress(SENDER_MAIL);
            MailAddress to = new MailAddress(recipient);
            MailMessage message = new MailMessage(from, to)
            {
                Subject = "Articles from RSS",
                Body = text,
                IsBodyHtml = false,
            };

            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential(SENDER_MAIL, SENDER_PASSWORD),
                EnableSsl = true
            };

            smtp.Send(message);
        }
    }
}
