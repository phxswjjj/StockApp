using StockApp.Utility;
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

        static readonly Lazy<HttpClient> GoodInfoClient = new Lazy<HttpClient>(() =>
        {
            var cookieHandler = new HttpClientHandler();
            cookieHandler.CookieContainer.SetCookies(new Uri("https://goodinfo.tw"),
                Properties.Settings.Default.GoodInfoLogin);
            cookieHandler.AllowAutoRedirect = true;

            var client = HttpClientFactory.Create(cookieHandler, new GoodInfoMessageHandler());
            return client;
        });
        static readonly Lazy<FrequenceManager> FreqMgr = new Lazy<FrequenceManager>(() =>
        {
            return new FrequenceManager();
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
            var logger = Utility.LogHelper.Log;
            var freqMgr = FreqMgr.Value;

            lock (GoodInfoLockObject)
            {
                while (!freqMgr.Execute())
                {
                    var waitMsDiff = freqMgr.NextTime.Value - DateTime.Now;
                    if (waitMsDiff.TotalMilliseconds > 0)
                    {
                        logger.Information($"Wait {waitMsDiff.TotalMilliseconds:N0} ms..");
                        Thread.Sleep(waitMsDiff);
                    }
                }
            }
            return GoodInfoClient.Value;
        }
    }
}
