using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StockApp.Web
{
    class WebRequest
    {
        static readonly object GoodInfoLockObject = new object();
        static DateTime GoodInfoNextFireTime = DateTime.MinValue;
        //每次請求安全的間隔時間(ms)
        const int GoodInfoWaitMS = 1_000;

        static readonly Lazy<HttpClient> GoodInfoClient = new Lazy<HttpClient>(() =>
        {
            var cookieHandler = new HttpClientHandler();
            cookieHandler.CookieContainer.SetCookies(new Uri("https://goodinfo.tw"),
                Properties.Settings.Default.GoodInfoLogin);
            cookieHandler.AllowAutoRedirect = true;

            var client = HttpClientFactory.Create(cookieHandler, new GoodInfoMessageHandler());
            return client;
        });

        internal static HttpClient Create()
        {
            var client = new HttpClient();

            client.DefaultRequestHeaders.UserAgent.Clear();
            client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/103.0.0.0 Safari/537.36");

            return client;
        }
        internal static HttpClient CreateGoodInfo()
        {
            //limit: 20 times/min
            lock (GoodInfoLockObject)
            {
                var waitMsDiff = GoodInfoNextFireTime - DateTime.Now;
                if (waitMsDiff.TotalMilliseconds > 0)
                {
                    Console.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] Wait {waitMsDiff.TotalMilliseconds:N0} ms..");
                    Thread.Sleep(waitMsDiff);
                }
                var wait = new Random().Next(GoodInfoWaitMS / 2, GoodInfoWaitMS * 2);
                GoodInfoNextFireTime = DateTime.Now.AddMilliseconds(wait);
            }
            return GoodInfoClient.Value;
        }
    }
}
