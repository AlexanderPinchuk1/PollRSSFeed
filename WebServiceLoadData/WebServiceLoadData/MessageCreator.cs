using NLog;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using WebServiceLoadData.Models;

namespace WebServiceLoadData
{
    public interface ITextCleaner
    {
        string GetInfoAboutChannelWithoutHtml(Channel channel);
        string GetTextWithoutHtml(string articleDescription);
    }

    public class TextCleaner: ITextCleaner
    {
        public string GetInfoAboutChannelWithoutHtml(Channel channel)
        {
            string result = "Channel title: " + channel.Title + "\n" +
                            "Channel description: " + GetTextWithoutHtml(channel.Description) + "\n" +
                            "Channel language: " + channel.Language + "\n" +
                            "Channel link: " + channel.Link + "\n" +
                            "Copyright: " + channel.Copyright + "\n\n";
            return result;
        }

        public string GetTextWithoutHtml(string articleDescription)
        {
            string result = Regex.Replace(articleDescription, "<(/?[^>]+)>", string.Empty);
            result = Regex.Replace(result, "\\n", string.Empty);

            return result;
        }
    }

    public class MessageCreator
    {
        readonly ITextCleaner TextCleaner;

        public MessageCreator(ITextCleaner textCleaner)
        {
            TextCleaner = textCleaner;
        }

        public MessageCreator()
        {
            TextCleaner = new TextCleaner();
        }

        public List<string> CreateMessage(RSSInfo rssInfo)
        {
            List<string> result = new List<string>
            {
                TextCleaner.GetInfoAboutChannelWithoutHtml(rssInfo.Channel)
            };

            string article = "";

            foreach (Item item in rssInfo.Items)
            {
                article += "Title: " + item.Title + "\n";
                article += "Time: " + item.PubDate + "\n";
                article += "Description: " + "\n" + TextCleaner.GetTextWithoutHtml(item.Description);
                if (item.Link != "")
                    article += "\nLink: " + item.Link;

                article += "\n\n";

                result.Add(article);
                article = "";
            }

            return result;
        }

        public List<string> GetInfoAboutNewsArticles(RSSInfo rssInfo)
        {
            List<string> result = new List<string>();
            string article = "";

            foreach (Item item in rssInfo.Items)
            {
                article += "Title: " + item.Title + "\n";
                article += "Time: " + item.PubDate + "\n";
                article += "Description: " + "\n" + TextCleaner.GetTextWithoutHtml(item.Description);
                if (item.Link != "")
                    article += "\nLink: " + item.Link;

                article += "\n\n";

                result.Add(article);
                article = "";
            }

            return result;
        }

        public List<string> CreateMessage(RSSInfo rssInfo, Logger logger)
        {
            List<string> result = new List<string>();

            result.Add(TextCleaner.GetInfoAboutChannelWithoutHtml(rssInfo.Channel));

            logger.Info("Channel information generated.");

            result.AddRange(GetInfoAboutNewsArticles(rssInfo));
            
            logger.Info("News articles generated.");

            return result;
        }


    }
}