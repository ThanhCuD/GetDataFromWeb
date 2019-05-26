using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;


namespace GetData
{
    class Program
    {

        [STAThread]
        static void Main(string[] args)
        {
            #region const
            const string url = "https://shopee.vn/flash_sale";
            const string linkToDriverServer = "D:\\Code\\GetdataFromWeb\\GetData\\GetDataFromWeb\\GetData\\GetData\\bin\\Debug";
            const string tagNeedLoad = "flash-sale-item-card__buy-now-button";
            const string xPathListSelect = "//div[@class = 'flash-sale-items']/div";
            const string xPathItemName = ".//div[@class = 'flash-sale-item-card__item-name']";
            const string xPathItemPrice = ".//span[@class='item-price-number']";
            #endregion

            var items = new List<object>();

            HtmlWeb htmlWeb = new HtmlWeb()
            {
                AutoDetectEncoding = false,
                OverrideEncoding = Encoding.UTF8
            };

            // use headless mode
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("headless");

            var driverService = ChromeDriverService.CreateDefaultService(linkToDriverServer);
            //hide your brower
            driverService.HideCommandPromptWindow = true;
            var driver = new ChromeDriver(driverService, options);
            driver.Navigate().GoToUrl(url);

            try
            {

                bool check = driver.PageSource.Contains(tagNeedLoad);
                // Wait until tagNeedLoad loaded
                while (!check)
                {
                    check = driver.PageSource.Contains(tagNeedLoad);
                }

                HtmlDocument doc = new HtmlDocument();
                var html = driver.PageSource.ToString();
                doc.LoadHtml(html);

                var saleItems = doc.DocumentNode.SelectNodes(xPathListSelect).ToList();

                Console.OutputEncoding = Encoding.UTF8;
                foreach (var item in saleItems)
                {
                    var itemName = item.SelectSingleNode(xPathItemName).InnerText;
                    var price = item.SelectSingleNode(xPathItemPrice).InnerText;
                    items.Add(new { itemName, price });
                }
                driver.Close();
            }
            catch (Exception ex)
            {
                driver.Close();
                throw ex;
            }

        }
    }
}
