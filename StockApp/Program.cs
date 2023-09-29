using Serilog;
using StockApp.Data;
using StockApp.Utility;
using StockApp.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Windows.Forms;
using Unity;

namespace StockApp
{
    static class Program
    {
        /// <summary>
        /// 應用程式的主要進入點。
        /// </summary>
        [STAThread]
        static void Main()
        {
            InitLog();
            InitUnity();

            LocalDb.Initialize();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());

            if (ChromiumBrowser.Instance.IsValueCreated)
                ChromiumBrowser.Instance.Value.Dispose();
        }

        private static void InitLog()
        {
            var logger = new LoggerConfiguration()
                .ReadFrom.AppSettings()
                .CreateLogger();
            LogHelper.Log = logger;
        }

        private static void InitUnity()
        {
            IUnityContainer container = new UnityContainer();
            container.RegisterInstance<ILogger>(LogHelper.Log);
            UnityHelper.Initialize(container);
        }
    }
}
