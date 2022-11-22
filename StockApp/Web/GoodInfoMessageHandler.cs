using AngleSharp.Io;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace StockApp.Web
{
    internal class GoodInfoMessageHandler : DelegatingHandler
    {
        static Uri LastReferUri = new Uri("https://goodinfo.tw/tw/index.asp");
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var headerBuilder = request as IRequestHeaderBuilder;
            headerBuilder?.SetHeaders(request.Headers);

            Console.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] Request: {request.RequestUri}");
            if (request.Headers.Referrer == null)
                request.Headers.Referrer = LastReferUri;
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] Refer: {request.Headers.Referrer}");

            LastReferUri = request.RequestUri;

            var tcs = new TaskCompletionSource<HttpResponseMessage>();

            var task = base.SendAsync(request, cancellationToken);
            task.ContinueWith(t =>
            {
                var resp = t.Result;
                if (resp.Content.Headers.ContentEncoding.FirstOrDefault() == "gzip")
                {
                    var stream = new GZipStream(resp.Content.ReadAsStreamAsync().Result,
                        CompressionMode.Decompress);
                    resp.Content = new DecompressedContent(resp.Content, stream);
                }

                tcs.SetResult(resp);
            });
            return tcs.Task;
        }
    }
}
