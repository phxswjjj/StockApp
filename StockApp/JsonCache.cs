using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockApp
{
    class JsonCache
    {
        internal static T Load<T>(string jsonFilePath, TimeSpan timeSpan)
        {
            if (!System.IO.File.Exists(jsonFilePath))
                return default(T);

            var lastModifiedAt = new FileInfo(jsonFilePath).LastWriteTime;
            if (DateTime.Now - lastModifiedAt > timeSpan)
                return default(T);
            var content = System.IO.File.ReadAllText(jsonFilePath);
            var caches = JsonConvert.DeserializeObject<T>(content);
            return caches;
        }
        internal static T Load<T>(string jsonFilePath)
        {
            return Load<T>(jsonFilePath, TimeSpan.MaxValue);
        }

        internal static void Store(string jsonFilePath, object result)
        {
            var content = JsonConvert.SerializeObject(result);
            var dirPath = Path.GetDirectoryName(jsonFilePath);
            if (!string.IsNullOrEmpty(dirPath) && !Directory.Exists(dirPath))
                Directory.CreateDirectory(dirPath);
            System.IO.File.WriteAllText(jsonFilePath, content);
        }
    }
}
