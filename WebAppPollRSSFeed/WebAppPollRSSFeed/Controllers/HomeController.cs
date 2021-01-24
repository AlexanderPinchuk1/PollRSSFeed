using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Office.CustomUI;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel;
using ServiceFilter;
using ServiceLoadData;
using ServiceMail;
using WebAppPollRSSFeed.Helpers;
using WebAppPollRSSFeed.Interfaces;
using WebAppPollRSSFeed.Models;

namespace WebAppPollRSSFeed.Controllers
{
    public class HomeController : Controller
    {
        public readonly List<string> Urls = UrlListSingleton.GetInstance();
        public readonly List<string> KeyWords = KeyWordsListSingleton.GetInstance();
        public readonly List<string> Recipients = RecipientsListSingleton.GetInstance();
        public ArticlesSingleton Articles = ArticlesSingleton.GetInstance();

        public HomeController()
        {
        }

        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Urls = Urls;
            ViewBag.KeyWords = KeyWords;
            ViewBag.Recipients = Recipients;
            ViewBag.ArticlesText = Articles.Text;

            return View("Index");
        }

        public IActionResult DeleteUrl(int num)
        {
            Urls.RemoveAt(num);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult CreateUrl(string url)
        {
            Urls.Add(url);
            return RedirectToAction("Index");
        }

        public IActionResult DeleteKeyWord(int num)
        {
            KeyWords.RemoveAt(num);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult CreateKeyWord(string keyWord)
        {
            if ((keyWord != null) && (keyWord.Trim() != ""))
                KeyWords.Add(keyWord);
            return RedirectToAction("Index");
        }

        public IActionResult DeleteRecipient(int num)
        {
            Recipients.RemoveAt(num);
            return RedirectToAction("Index");
        }

        public IActionResult Action()
        {
            Articles.Text = "";
            foreach (string url in Urls)
            {
                Thread thread = new Thread(new ParameterizedThreadStart(RecieveFromURlAsync));
                thread.Start(url);
            }

            Thread.Sleep(1000);

            return RedirectToAction("Index");
        }

        private async void RecieveFromURlAsync(object url)
        {
            WebServiceLoadDataSoapClient webServiceLoadData = new WebServiceLoadDataSoapClient(WebServiceLoadDataSoapClient.EndpointConfiguration.WebServiceLoadDataSoap);
            LoadDataResponse loadDataResponse = await webServiceLoadData.LoadDataAsync((string)url);
            List<string> data = loadDataResponse.Body.LoadDataResult;

            if (KeyWords.Count != 0)
            {
                WebServiceFilterSoapClient webServiceFilter = new WebServiceFilterSoapClient(WebServiceFilterSoapClient.EndpointConfiguration.WebServiceFilterSoap);

                var filterData = new ServiceFilter.ArrayOfString();
                var  keyWordsData = new ServiceFilter.ArrayOfString();
                filterData.AddRange(data);
                filterData.RemoveAt(0);
                keyWordsData.AddRange(KeyWords);

                FilterResponse filterResponse = await webServiceFilter.FilterAsync(filterData, keyWordsData);
                List<string> recivedFilterData = filterResponse.Body.FilterResult;

                recivedFilterData.Insert(0, data[0]);
                data.Clear();
                data.AddRange(recivedFilterData);
            }

            if (data.Count == 1)
                return;

            string message = "";
            foreach (string artile in data)
            {
                message += artile;
            }

            lock (Articles)
            {
                Articles.Text += data[0];
            }

            SendToRecipients(message);
        }


        private void SendToRecipients(string message)
        {
            foreach (string recipient in Recipients)
            {
                Thread thread = new Thread(new ParameterizedThreadStart(SendToRecipientAsync));
                thread.Start(new Letter(message, recipient));
            }
        }

        private async void SendToRecipientAsync(object letter)
        {
            Letter let = (Letter)letter;
            WebServiceMailSoapClient webServiceMail = new WebServiceMailSoapClient(WebServiceMailSoapClient.EndpointConfiguration.WebServiceMailSoap);
            await webServiceMail.SendMessageAsync(let.Message, let.Recipient);
        }

        [HttpPost]
        public IActionResult CreateRecipient(string recipient)
        {
            if ((recipient != null) && (recipient.Trim() != ""))
                Recipients.Add(recipient);
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
