using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    public class TextHelper
    {
        public static string ChageStringToPassword(string text)
        {
            return new string('\u2022', text.Length);
        }
        public static string BoolToString(bool showEnable)
        {
            return showEnable ? "1" : "0";
        }
        public static  int BoolToInt(bool showEnable)
        {
            return showEnable ? 1 : 0;
        }
    }
}
