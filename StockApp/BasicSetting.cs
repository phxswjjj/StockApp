using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockApp
{
    class BasicSetting
    {
        static BasicSetting _Instance;
        public static BasicSetting Instance => System.Threading.LazyInitializer.EnsureInitialized(ref _Instance, () =>
        {
            var ins = new BasicSetting();
            ins.Load();
            return ins;
        });
        public decimal PriceLimit { get; private set; }

        public void Load()
        {
            var price = Properties.Settings.Default.PriceLimit;
            this.PriceLimit = price;
        }
    }
}
