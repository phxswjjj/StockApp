using LiteDB;
using StockApp.Group;
using StockApp.Utility;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace StockApp.Data
{
    internal class LocalDb
    {
        private static Lazy<string> LazyFilePath = new Lazy<string>(() => ConfigurationManager.AppSettings["LocalDbPath"]);
        public static string FilePath => LazyFilePath.Value;

        public static bool Initialize()
        {
            var container = UnityHelper.Create();
            using (ILiteDatabase db = Create())
            {
                container.RegisterInstance(db);

                InitializeGroup(container);

            }
            return true;
        }

        private static void InitializeGroup(IUnityContainer container)
        {
            var custGroupRepo = container.Resolve<CustomGroupRepository>();

            if (!custGroupRepo.Initialize<CustomGroup>())
                throw new Exception($"Init {nameof(CustomGroup)} Fail");

            if (!custGroupRepo.Initialize<FavoriteGroup>())
                throw new Exception($"Init {nameof(FavoriteGroup)} Fail");

            if (!custGroupRepo.Initialize<HateGroup>())
                throw new Exception($"Init {nameof(HateGroup)} Fail");

            if (!custGroupRepo.Initialize<ETFGroup>())
                throw new Exception($"Init {nameof(ETFGroup)} Fail");

            if (!custGroupRepo.Initialize<TraceGroup>())
                throw new Exception($"Init {nameof(TraceGroup)} Fail");
        }

        public static LiteDatabase Create(string path = null)
        {
            if (string.IsNullOrWhiteSpace(path))
                path = FilePath;

            return new LiteDatabase(path);
        }

        public class ImportHistory
        {
            public ImportHistory(string name)
            {
                Name = name;
            }

            public string Name { get; private set; }
            public DateTime Timestamp { get; private set; } = DateTime.Now;
        }
    }
}
