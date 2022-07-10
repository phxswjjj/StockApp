using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace StockApp
{
    class WebRequest
    {
        internal static HttpClient Create()
        {
            var client = new HttpClient();

            client.DefaultRequestHeaders.UserAgent.Clear();
            client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/103.0.0.0 Safari/537.36");

            return client;
        }
        internal static HttpClient CreateGoodInfo()
        {
            HttpClient client = new Web.GoodInfoClient();

            client.DefaultRequestHeaders.UserAgent.Clear();
            client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/103.0.0.0 Safari/537.36");

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.ParseAdd("text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
            //client.DefaultRequestHeaders.AcceptEncoding.Clear();
            //client.DefaultRequestHeaders.AcceptEncoding.ParseAdd("gzip, deflate, br");
            client.DefaultRequestHeaders.AcceptLanguage.Clear();
            client.DefaultRequestHeaders.AcceptLanguage.ParseAdd("zh-TW,zh;q=0.9,en-US;q=0.8,en;q=0.7,zh-CN;q=0.6");

            return client;
        }
    }
}
