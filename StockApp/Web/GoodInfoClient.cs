using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StockApp.Web
{
    class GoodInfoClient : HttpClient
    {
        static readonly object LockObject = new object();
        static DateTime NextFireTime = DateTime.MinValue;
        //每次請求安全的間隔時間(ms)
        const int WaitMS = 20000;

        static Lazy<HttpClientHandler> Handler = new Lazy<HttpClientHandler>(() =>
        {
            var uri = new Uri("https://goodinfo.tw");
            var handler = new HttpClientHandler();
            handler.CookieContainer.SetCookies(uri, Properties.Settings.Default.GoodInfoLogin);
            return handler;
        });

        public GoodInfoClient() : base(Handler.Value)
        {
        }

        public override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var task = Task.Factory.StartNew(() =>
            {
                lock (LockObject)
                {
                    var waitMsDiff = NextFireTime - DateTime.Now;
                    if (waitMsDiff.TotalMilliseconds > 0)
                        Thread.Sleep(waitMsDiff);

                    var result = base.SendAsync(request, cancellationToken).Result;
                    NextFireTime = DateTime.Now.AddMilliseconds(WaitMS);

                    return result;
                }
            });
            return task;
        }
    }
}
