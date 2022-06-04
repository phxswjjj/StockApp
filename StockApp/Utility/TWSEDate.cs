using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockApp.Utility
{
    class TWSEDate
    {
        public static DateTime Today => DateTime.Now.AddHours(-13).AddMinutes(-30).Date;
    }
}
