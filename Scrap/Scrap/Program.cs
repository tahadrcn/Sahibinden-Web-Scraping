using HtmlAgilityPack;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;

namespace WebScraper
{
    class Program
    {
        static void Main(string[] args)
        {
            // liste tanımlamaları
            List<string> titles = new List<string>();
            List<string> links = new List<string>();
            List<string> prices = new List<string>();

           
            var userAgent = "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/112.0.0.0 Safari/537.36";

            // Proxy sunucusu ve port numarası
            var proxy = new Proxy
            {
                Kind = ProxyKind.Manual,
                IsAutoDetect = false,
                HttpProxy = "http://proxyserver:port",
                SslProxy = "http://proxyserver:port",
            };

            // Proxy sunucusu kullanarak sürücüyü yapılandırın
            var options = new ChromeOptions();
            options.Proxy = proxy;
            options.AddArgument("--user-agent=" + userAgent);



            // ChromeDriver yolunu belirtin
            var driverService = ChromeDriverService.CreateDefaultService();
            driverService.HideCommandPromptWindow = true;
            var driver = new ChromeDriver(driverService, options);

            driver.Navigate().GoToUrl("https://www.sahibinden.com/");

            var ul = driver.FindElement(By.CssSelector("ul.vitrin-list clearfix"));

            // <a> etiketlerini bulun, href ve title attribute'ünü  alın ve listeye ekleyin
            var liElements = ul.FindElements(By.CssSelector("a"));
            foreach (var li in liElements)
            {
                links.Add(li.GetAttribute("href"));
                titles.Add(li.GetAttribute("title"));
            }

            // detay linklerini dolaşın ve fiyatları çekip listeye atın
            foreach(var href in links)
            {
                driver.Navigate().GoToUrl(href);
                var span = driver.FindElement(By.CssSelector("span.sticky-header-attribute price"));
                prices.Add(span.Text);

            }

            // Console ekranına yazdırın
            for (int i = 0; i < titles.Count; i++)
            {
                Console.WriteLine("ilan adı: " + titles[i] + " fiyatı: " + prices[i]);
            }

            // Dosya oluşturun , yolunu belirtin ve dosyaya yazdırın
            StreamWriter sahibinden = new StreamWriter("C:\\sahibinden.txt");
            string DosyaYolu = "C:\\sahibinden.txt";
            if (File.Exists(DosyaYolu))
            {
                for (int i = 0; i < titles.Count; i++)
                {
                    sahibinden.WriteLine("ilan adı: " + titles[i] + " fiyatı: " + prices[i]);
                }
            }
            else
            {
                Console.WriteLine("Folder not found");
            }




            sahibinden.Close();
            driver.Quit();
            Console.ReadKey();





        }

    }
}