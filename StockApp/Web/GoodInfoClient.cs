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
        static readonly Uri BaseUri = new Uri("https://goodinfo.tw");
        static DateTime NextFireTime = DateTime.MinValue;
        //每次請求安全的間隔時間(ms)
        const int WaitMS = 30_000;

        static Lazy<HttpClientHandler> Handler = new Lazy<HttpClientHandler>(() =>
        {
            var handler = new HttpClientHandler();
            handler.CookieContainer.SetCookies(BaseUri, Properties.Settings.Default.GoodInfoLogin);
            handler.AllowAutoRedirect = false;
            return handler;
        });

        public GoodInfoClient() : base(Handler.Value)
        {
        }

        public override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var tcs = new TaskCompletionSource<HttpResponseMessage>();

            lock (LockObject)
            {
                var waitMsDiff = NextFireTime - DateTime.Now;
                if (waitMsDiff.TotalMilliseconds > 0)
                {
                    Console.WriteLine($"Wait {waitMsDiff.TotalMilliseconds:N0} ms..");
                    Thread.Sleep(waitMsDiff);
                }

                Console.WriteLine($"request: {request.RequestUri}");
                base.SendAsync(request, cancellationToken)
                    .ContinueWith(t =>
                    {
                        var resp = t.Result;
                        if (resp.StatusCode == HttpStatusCode.Redirect)
                        {
                            var newRequest = new HttpRequestMessage(HttpMethod.Get, resp.Headers.Location);

                            foreach (var header in request.Headers)
                            {
                                newRequest.Headers.TryAddWithoutValidation(header.Key, header.Value);
                            }
                            newRequest.Headers.TryAddWithoutValidation("Referer", request.RequestUri.AbsoluteUri);
                            foreach (var property in request.Properties)
                            {
                                newRequest.Properties.Add(property);
                            }
                            Console.WriteLine($"redirect request: {request.RequestUri}");
                            base.SendAsync(newRequest, cancellationToken)
                                .ContinueWith(t2 => tcs.SetResult(t2.Result));
                        }
                        else
                            tcs.SetResult(resp);
                    });
                var wait = new Random().Next(WaitMS / 2, WaitMS * 2);
                NextFireTime = DateTime.Now.AddMilliseconds(wait);
            }
            return tcs.Task;
        }
    }
}
