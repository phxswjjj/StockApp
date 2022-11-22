using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace StockApp.Web
{
    internal interface IRequestHeaderBuilder
    {
        void SetHeaders(HttpRequestHeaders headers);
    }
}
