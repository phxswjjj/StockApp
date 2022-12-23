using PuppeteerSharp;
using Serilog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockApp.Web
{
    internal class ChromiumBrowser
    {
        public static Lazy<IBrowser> Instance = new Lazy<IBrowser>(() =>
        {
            var logger = Utility.LogHelper.Log;

            var sw = new Stopwatch();
            sw.Start();
            using (var browserFetcher = new BrowserFetcher())
            {
                browserFetcher.DownloadAsync().Wait();
            }
            var browser = Puppeteer.LaunchAsync(new LaunchOptions { Headless = true }).Result;
            sw.Stop();
            logger
                .ForContext("Elapsed", sw.ElapsedMilliseconds)
                .Information("Browser Instance Initialized {Elapsed:#,##0} ms");
            return browser;
        });

        public static IPage NewPage(string url)
        {
            var logger = Utility.LogHelper.Log;

            var browser = Instance.Value;
            var page = browser.NewPageAsync().Result;
            page.SetUserAgentAsync("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/108.0.0.0 Safari/537.36").Wait();

            var sw = new Stopwatch();
            sw.Start();
            page.GoToAsync(url, WaitUntilNavigation.DOMContentLoaded).Wait();
            sw.Stop();
            logger
                .ForContext("RequestUrl", url)
                .ForContext("Elapsed", sw.ElapsedMilliseconds)
                .Information("Load Page {Elapsed:#,##0} ms: {RequestUrl}");

            return page;
        }
    }
}
