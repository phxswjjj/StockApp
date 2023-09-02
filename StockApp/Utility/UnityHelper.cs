using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace StockApp.Utility
{
    internal class UnityHelper
    {
        private static IUnityContainer Instance { get; set; }

        internal static void Initialize(IUnityContainer container) => Instance = container;

        public static IUnityContainer Create() => Instance.CreateChildContainer();
    }
}
