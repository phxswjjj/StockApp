using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace StockApp.Web
{
    internal class DecompressedContent : StreamContent
    {
        public DecompressedContent(HttpContent content, Stream stream) : base(stream)
        {
            // copy the headers from the original content
            foreach (KeyValuePair<string, IEnumerable<string>> header in content.Headers)
            {
                Headers.TryAddWithoutValidation(header.Key, header.Value);
            }
        }
    }
}
