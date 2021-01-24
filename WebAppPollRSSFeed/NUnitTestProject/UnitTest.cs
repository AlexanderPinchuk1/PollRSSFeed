using Microsoft.AspNetCore.Mvc;
using Moq;
using NLog;
using NUnit.Framework;
using System.Collections.Generic;
using WebAppPollRSSFeed.Controllers;
using WebServiceLoadData;
using WebServiceLoadData.Models;
using WebServiceFilter;
using System;

namespace NUnitTestProject
{
    public class Tests
    { 
        private HomeController HomeController;
        private ViewResult Result;

        [SetUp]
        public void Setup()
        {
            HomeController = new HomeController();
            Result = HomeController.Index() as ViewResult;
        }

        [Test]
        public void IndexViewResultNotNull()
        { 
            Assert.IsNotNull(Result);
        }

        [Test]
        public void IndexViewEqualIndexCshtml()
        {
            Assert.AreEqual("Index", Result.ViewName);
        }

        [Test]
        public void IndexDataInViewbag()
        { 
            Assert.IsNotNull(HomeController.ViewBag.Urls);
            Assert.IsNotNull(HomeController.ViewBag.KeyWords);
            Assert.IsNotNull(HomeController.ViewBag.Recipients);
            Assert.IsNotNull(HomeController.ViewBag.ArticlesText);
        }


        [Test]
        public void AddDataIntoControllerLists()
        {
            int numUrls = HomeController.Urls.Count;
            int numRecepints = HomeController.Recipients.Count;
            int numKeyWords = HomeController.KeyWords.Count;

            HomeController.CreateUrl("http://rss.art19.com/the-daily");
            HomeController.CreateRecipient("alexanderpinchuk1@gmail.com");
            HomeController.CreateKeyWord("music");

            Assert.Greater(HomeController.Urls.Count, numUrls);
            Assert.Greater(HomeController.Recipients.Count, numRecepints);
            Assert.Greater(HomeController.KeyWords.Count, numKeyWords);
        }

        [Test]
        public void DeleteDataInControllerLists()
        {
            int numUrls = HomeController.Urls.Count;
            int numRecepints = HomeController.Recipients.Count;
            int numKeyWords = HomeController.KeyWords.Count;

            HomeController.DeleteUrl(0);
            HomeController.DeleteRecipient(0);
            HomeController.DeleteKeyWord(0);

            Assert.Less(HomeController.Urls.Count, numUrls);
            Assert.Less(HomeController.Recipients.Count, numRecepints);
            Assert.Less(HomeController.KeyWords.Count, numKeyWords);
        }

        [Test]
        public void CreateMessageWithMockObj()
        {
            var mock = new Mock<ITextCleaner>();
            RSSInfo rssInfo = new RSSInfo();
            string expectedChannelinfo = "Channel info";
            mock.Setup(a => a.GetInfoAboutChannelWithoutHtml(rssInfo.Channel)).Returns(expectedChannelinfo);
            MessageCreator MessageCreator = new MessageCreator(mock.Object);

            List<string> message = MessageCreator.CreateMessage(rssInfo);

            Assert.AreEqual(message[0], expectedChannelinfo);             
        }

        [Test]
        public void CreateMessageWithLogNotNull()
        {
            Logger logger = LogManager.GetCurrentClassLogger();
            MessageCreator MessageCreator = new MessageCreator();

            List<string> message = MessageCreator.CreateMessage(new RSSInfo(), logger);

            Assert.IsNotNull(message);
        }

        [Test]
        public void CreateMessageWithInvalidDataExpectedExeption()
        {
            MessageCreator MessageCreator = new MessageCreator();
            RSSInfo rssInfo = null;

            Assert.Throws<NullReferenceException>(() => MessageCreator.CreateMessage(rssInfo));
        }

        [Test]
        public void FilterTextByKeyWords()
        {
            Filter filter = new Filter();
            List<string> articles = new List<string> {"one two three", "qw er ty iu", "1 2 3 4 5" };
            List<string> keyWords = new List<string> { "one", "1" };
            List<string> expectedResult = new List<string> { "one two three", "1 2 3 4 5" };

            List<string> result = filter.FilterByKeyWords(articles, keyWords);

            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void ÑlearHtmlTags()
        {
            TextCleaner textCleaner = new TextCleaner();
            string expectedResult = "headingparagraph";
            string html = "<html><html><body><h1>heading</h1><p>paragraph</p></body></html>";

            string result = textCleaner.GetTextWithoutHtml(html);

            Assert.AreEqual(result, expectedResult);
        }
    }
}