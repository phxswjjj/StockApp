using Serilog;
using StockApp.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

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

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

        private static void InitLog()
        {
            var logger = new LoggerConfiguration()
                .WriteTo.File(@"Logs\log-.txt", rollingInterval: RollingInterval.Day, retainedFileCountLimit: 3)
                .WriteTo.Console()
                .CreateLogger();
            LogHelper.Log = logger;
        }
    }
}
