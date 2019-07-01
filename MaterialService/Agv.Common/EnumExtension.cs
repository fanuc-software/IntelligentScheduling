using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agv.Common
{
    public static class EnumExtension
    {
        public static string EnumToString<T>(this T t)
        { 
            return Enum.Parse(typeof(T), t.ToString(), false).ToString();
        }
    }
}
