using AngleSharp.Io;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
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
            var logger = Utility.LogHelper.Log;
            var headerBuilder = request as IRequestHeaderBuilder;
            headerBuilder?.SetHeaders(request.Headers);

            logger
                .ForContext("RequestUri", request.RequestUri)
                .Information($"Request: {{RequestUri}}");
            if (request.Headers.Referrer == null)
                request.Headers.Referrer = LastReferUri;

            LastReferUri = request.RequestUri;

            var tcs = new TaskCompletionSource<HttpResponseMessage>();

            var task = base.SendAsync(request, cancellationToken);
            task.ContinueWith(t =>
            {
                if (t.IsCanceled)
                    return;
                var resp = t.Result;
                if (resp.StatusCode == HttpStatusCode.Redirect)
                {
                    var newRequest = new HttpRequestMessage(System.Net.Http.HttpMethod.Get, resp.Headers.Location);

                    foreach (var header in request.Headers)
                    {
                        newRequest.Headers.TryAddWithoutValidation(header.Key, header.Value);
                    }
                    newRequest.Headers.TryAddWithoutValidation("Referer", request.RequestUri.AbsoluteUri);
                    foreach (var property in request.Properties)
                    {
                        newRequest.Properties.Add(property);
                    }
                    logger.Information($"redirect request: {request.RequestUri}");
                    base.SendAsync(newRequest, cancellationToken)
                        .ContinueWith(t2 => tcs.SetResult(t2.Result));
                }
                else if (resp.Content.Headers.ContentEncoding.FirstOrDefault() == "gzip")
                {
                    var stream = new GZipStream(resp.Content.ReadAsStreamAsync().Result,
                        CompressionMode.Decompress);
                    resp.Content = new DecompressedContent(resp.Content, stream);
                    tcs.SetResult(resp);
                }
                else
                    tcs.SetResult(resp);
            });
            return tcs.Task;
        }
    }
}
