using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace social.Utilities
{
    public static class Money
    {
        public static string Convert(int price)
        {
            return price.ToString("#,##0");
        }
    }
}
