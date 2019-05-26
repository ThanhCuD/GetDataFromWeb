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
using System.Text;
using System.Threading;


namespace GetData
{
    class Program
    {
       
        [STAThread]
        static void Main(string[] args)
        {
            string url = "https://shopee.vn/flash_sale";

            HtmlWeb htmlWeb = new HtmlWeb()
            {
                AutoDetectEncoding = false,
                OverrideEncoding = Encoding.UTF8
            };

            ChromeOptions options = new ChromeOptions();
            options.AddArgument("headless");

            var driverService = ChromeDriverService.CreateDefaultService("C:\\Users\\Admin\\Downloads\\Compressed\\chromedriver_win32_2");
            driverService.HideCommandPromptWindow = true;
             var driver = new ChromeDriver(driverService, options);
            driver.Navigate().GoToUrl(url);
            //
            try
            {
                bool check = driver.PageSource.Contains("flash-sale-item-card__buy-now-button");
                while (!check)
                {
                     check = driver.PageSource.Contains("flash-sale-item-card__buy-now-button");
                }

                HtmlDocument doc = new HtmlDocument();
                var html = driver.PageSource.ToString();
                doc.LoadHtml(html);

                var saleItems = doc.DocumentNode.SelectNodes("//div[@class = 'flash-sale-items']/div").ToList();

                var items = new List<object>();

                Console.OutputEncoding = Encoding.UTF8;
                foreach (var item in saleItems)
                {
                    var itemName = item.SelectSingleNode(".//div[@class = 'flash-sale-item-card__item-name']").InnerText;
                    var price = item.SelectSingleNode(".//span[@class='item-price-number']").InnerText;
                    Console.Write(itemName);
                    Console.WriteLine("price : "+price);
                    items.Add(new { itemName, price});
                }
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                driver.Close();
                throw ex;
            }
            
        }
    }
}
