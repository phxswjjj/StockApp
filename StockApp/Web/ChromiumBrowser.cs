using PuppeteerSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockApp.Web
{
    internal class ChromiumBrowser
    {
        public static Lazy<IBrowser> Instance = new Lazy<IBrowser>(() =>
        {
            using (var browserFetcher = new BrowserFetcher())
            {
                browserFetcher.DownloadAsync().Wait();
            }
            var browser = Puppeteer.LaunchAsync(new LaunchOptions { Headless = true }).Result;
            return browser;
        });

        public static IPage NewPage(string url)
        {
            var browser = Instance.Value;
            var page = browser.NewPageAsync().Result;
            page.SetUserAgentAsync("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/108.0.0.0 Safari/537.36").Wait();
            page.GoToAsync(url, WaitUntilNavigation.DOMContentLoaded).Wait();
            return page;
        }
    }
}
