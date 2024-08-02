using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WheelTest
{
    public static class Utility
    {
        //FocusManager.IsFocusScope="False"-点击调
        public static string CompletionUri(this string uriString)
        {
            if (!string.IsNullOrWhiteSpace(uriString) && uriString.Length > 3 && uriString.Substring(0, 2) == "//")
                return "http:" + uriString;
            else
                return uriString;
        }
    }
}
