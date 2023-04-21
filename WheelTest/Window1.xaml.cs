using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.CodeDom.Compiler;
using System.Reflection;
using WheelTest.CSDN.DailyExercise;
using Imazen.WebP.Extern;
using System.Runtime.InteropServices;
using System.Windows.Navigation;
using System.ComponentModel;
using System.Net;
using System.Windows.Forms;
using Iop.Api.Util;
using System.Security.Policy;

namespace WheelTest
{
    /// <summary>
    /// Window1.xaml 的交互逻辑
    /// </summary>
    public partial class Window1 : Window
    {
        string szTmp = "https://item.taobao.com/item.htm?spm=a211oj.24780012.3690372280.ditem2&id=698892741964&utparam=%7B%22ald_res%22%3A%2228320006%22%2C%22ald_solution%22%3A%22SmartHorseRace%22%2C%22ald_biz%22%3A1234%2C%22ump_item_price%22%3A%22639%22%2C%22traceId%22%3A%222147b31e16820640056328654e11ea%22%2C%22ald_st%22%3A%221682064005769%22%2C%22item_price%22%3A%22639%22%2C%22ald_price_field%22%3A%22itemActPrice%22%2C%22ump_invoke%22%3A%222%22%2C%22ump_sku_id%22%3A%225113921126542%22%2C%22ump_price_stage%22%3A%220%22%7D";
        public Window1()
        {
            var a = WinInetHelper.GetCookieString("https://item.taobao.com/");
            InitializeComponent();
            {
                //new SMTApiTest().Mian();
            }
            {
                //new DailyExercise().DailyExercise2023020181();
            }
            {
                //GetJsMethd(new object[] { "{\"dataType\":\"moduleData\",\"argString\":\"{\\\\\"memberId\\\\\":\\\\\"packpal\\\\\",\\\\\"appName\\\\\":\\\\\"pcmodules\\\\\",\\\\\"resourceName\\\\\":\\\\\"wpOfferColumn\\\\\",\\\\\"type\\\\\":\\\\\"view\\\\\",\\\\\"version\\\\\":\\\\\"1.0.0\\\\\",\\\\\"appdata\\\\\":{\\\\\"sortType\\\\\":\\\\\"wangpu_score\\\\\",\\\\\"sellerRecommendFilter\\\\\":false,\\\\\"mixFilter\\\\\":false,\\\\\"tradenumFilter\\\\\":false,\\\\\"quantityBegin\\\\\":null,\\\\\"pageNum\\\\\":1,\\\\\"count\\\\\":30}}\"}", "b319aafe27d217cda2a547d803956e39" });
            }
            Geta(szTmp);
         
        }
        public void Geta(string url) 
        {
            string szTmp = url;
            Uri uri = new Uri(szTmp);
            webBrowser.Navigate(uri);
        }

        /// <summary>
        /// 执行JS
        /// </summary>
        /// <param name="sExpression">参数体</param>
        /// <param name="sCode">JavaScript代码的字符串</param>
        /// <returns></returns>
        private static string GetJsMethd(object[] para)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("package aa{");
            sb.Append(" public class JScript {");
            sb.Append("      public static function test(data,token) {");
            sb.Append("        var i = \"//h5api.m.1688.com/h5/mtop.1688.shop.data.get/1.0/\",");
            sb.Append("     a =  \"12574478\",");
            sb.Append("     s = (new Date).getTime(),");
            sb.Append("     u = function(e) {");
            sb.Append("     function t(e, t) {");
            sb.Append("     return e << t | e >>> 32 - t");
            sb.Append("     }");
            sb.Append("     ");
            sb.Append("     function o(e, t) {");
            sb.Append("     var o, n, r, i, a;");
            sb.Append("     return r = 2147483648 & e,");
            sb.Append("     i = 2147483648 & t,");
            sb.Append("     a = (1073741823 & e) + (1073741823 & t),");
            sb.Append("     (o = 1073741824 & e) & (n = 1073741824 & t) ? 2147483648 ^ a ^ r ^ i : o | n ? 1073741824 & a ? 3221225472 ^ a ^ r ^ i : 1073741824 ^ a ^ r ^ i : a ^ r ^ i");
            sb.Append("     }");
            sb.Append("     ");
            sb.Append("     function n(e, n, r, i, a, s, u) {");
            sb.Append("     return o(t(e = o(e, o(o(function(e, t, o) {");
            sb.Append("     return e & t | ~e & o");
            sb.Append("     }(n, r, i), a), u)), s), n)");
            sb.Append("     }");
            sb.Append("     ");
            sb.Append("     function r(e, n, r, i, a, s, u) {");
            sb.Append("     return o(t(e = o(e, o(o(function(e, t, o) {");
            sb.Append("     return e & o | t & ~o");
            sb.Append("     }(n, r, i), a), u)), s), n)");
            sb.Append("     }");
            sb.Append("     ");
            sb.Append("     function i(e, n, r, i, a, s, u) {");
            sb.Append("     return o(t(e = o(e, o(o(function(e, t, o) {");
            sb.Append("     return e ^ t ^ o");
            sb.Append("     }(n, r, i), a), u)), s), n)");
            sb.Append("     }");
            sb.Append("     ");
            sb.Append("     function a(e, n, r, i, a, s, u) {");
            sb.Append("     return o(t(e = o(e, o(o(function(e, t, o) {");
            sb.Append("     return t ^ (e | ~o)");
            sb.Append("     }(n, r, i), a), u)), s), n)");
            sb.Append("     }");
            sb.Append("     ");
            sb.Append("     function s(e) {");
            sb.Append("     var t, o = \"\",");
            sb.Append("     n = \"\";");
            sb.Append("     for (t = 0; 3 >= t; t++)");
            sb.Append("     o += (n = \"0\" + (e >>> 8 * t & 255)");
            sb.Append("     .toString(16))");
            sb.Append("     .substr(n.length - 2, 2);");
            sb.Append("     return o");
            sb.Append("     }");
            sb.Append("     var u, l, c, d, p, f, h, m, y, g;");
            sb.Append("     for (g = function(e) {");
            sb.Append("     for (var t = e.length, o = t + 8, n = 16 * ((o - o % 64) / 64 + 1), r = Array(n - 1), i = 0, a = 0; t > a;)");
            sb.Append("     i = a % 4 * 8,");
            sb.Append("     r[(a - a % 4) / 4] |= e.charCodeAt(a) << i,");
            sb.Append("     a++;");
            sb.Append("     return i = a % 4 * 8,");
            sb.Append("     r[(a - a % 4) / 4] |= 128 << i,");
            sb.Append("     r[n - 2] = t << 3,");
            sb.Append("     r[n - 1] = t >>> 29,");
            sb.Append("     r");
            sb.Append("     }(e = function(e) {");
            sb.Append("     var t = String.fromCharCode;");
            sb.Append("     e = e.replace(/\\r\\n/g, \"\\n\");");
            sb.Append("     for (var o, n = \"\", r = 0; r < e.length; r++)");
            sb.Append("     128 > (o = e.charCodeAt(r)) ? n += t(o) : o > 127 && 2048 > o ? (n += t(o >> 6 | 192),");
            sb.Append("     n += t(63 & o | 128)) : (n += t(o >> 12 | 224),");
            sb.Append("     n += t(o >> 6 & 63 | 128),");
            sb.Append("     n += t(63 & o | 128));");
            sb.Append("     return n");
            sb.Append("     }(e)),");
            sb.Append("     f = 1732584193,");
            sb.Append("     h = 4023233417,");
            sb.Append("     m = 2562383102,");
            sb.Append("     y = 271733878,");
            sb.Append("     u = 0; u < g.length; u += 16)");
            sb.Append("     l = f,");
            sb.Append("     c = h,");
            sb.Append("     d = m,");
            sb.Append("     p = y,");
            sb.Append("     h = a(h = a(h = a(h = a(h = i(h = i(h = i(h = i(h = r(h = r(h = r(h = r(h = n(h = n(h = n(h = n(h, m = n(m, y = n(y, f = n(f, h, m, y, g[u + 0], 7, 3614090360), h, m, g[u + 1], 12, 3905402710), f, h, g[u + 2], 17, 606105819), y, f, g[u + 3], 22, 3250441966), m = n(m, y = n(y, f = n(f, h, m, y, g[u + 4], 7, 4118548399), h, m, g[u + 5], 12, 1200080426), f, h, g[u + 6], 17, 2821735955), y, f, g[u + 7], 22, 4249261313), m = n(m, y = n(y, f = n(f, h, m, y, g[u + 8], 7, 1770035416), h, m, g[u + 9], 12, 2336552879), f, h, g[u + 10], 17, 4294925233), y, f, g[u + 11], 22, 2304563134), m = n(m, y = n(y, f = n(f, h, m, y, g[u + 12], 7, 1804603682), h, m, g[u + 13], 12, 4254626195), f, h, g[u + 14], 17, 2792965006), y, f, g[u + 15], 22, 1236535329), m = r(m, y = r(y, f = r(f, h, m, y, g[u + 1], 5, 4129170786), h, m, g[u + 6], 9, 3225465664), f, h, g[u + 11], 14, 643717713), y, f, g[u + 0], 20, 3921069994), m = r(m, y = r(y, f = r(f, h, m, y, g[u + 5], 5, 3593408605), h, m, g[u + 10], 9, 38016083), f, h, g[u + 15], 14, 3634488961), y, f, g[u + 4], 20, 3889429448), m = r(m, y = r(y, f = r(f, h, m, y, g[u + 9], 5, 568446438), h, m, g[u + 14], 9, 3275163606), f, h, g[u + 3], 14, 4107603335), y, f, g[u + 8], 20, 1163531501), m = r(m, y = r(y, f = r(f, h, m, y, g[u + 13], 5, 2850285829), h, m, g[u + 2], 9, 4243563512), f, h, g[u + 7], 14, 1735328473), y, f, g[u + 12], 20, 2368359562), m = i(m, y = i(y, f = i(f, h, m, y, g[u + 5], 4, 4294588738), h, m, g[u + 8], 11, 2272392833), f, h, g[u + 11], 16, 1839030562), y, f, g[u + 14], 23, 4259657740), m = i(m, y = i(y, f = i(f, h, m, y, g[u + 1], 4, 2763975236), h, m, g[u + 4], 11, 1272893353), f, h, g[u + 7], 16, 4139469664), y, f, g[u + 10], 23, 3200236656), m = i(m, y = i(y, f = i(f, h, m, y, g[u + 13], 4, 681279174), h, m, g[u + 0], 11, 3936430074), f, h, g[u + 3], 16, 3572445317), y, f, g[u + 6], 23, 76029189), m = i(m, y = i(y, f = i(f, h, m, y, g[u + 9], 4, 3654602809), h, m, g[u + 12], 11, 3873151461), f, h, g[u + 15], 16, 530742520), y, f, g[u + 2], 23, 3299628645), m = a(m, y = a(y, f = a(f, h, m, y, g[u + 0], 6, 4096336452), h, m, g[u + 7], 10, 1126891415), f, h, g[u + 14], 15, 2878612391), y, f, g[u + 5], 21, 4237533241), m = a(m, y = a(y, f = a(f, h, m, y, g[u + 12], 6, 1700485571), h, m, g[u + 3], 10, 2399980690), f, h, g[u + 10], 15, 4293915773), y, f, g[u + 1], 21, 2240044497), m = a(m, y = a(y, f = a(f, h, m, y, g[u + 8], 6, 1873313359), h, m, g[u + 15], 10, 4264355552), f, h, g[u + 6], 15, 2734768916), y, f, g[u + 13], 21, 1309151649), m = a(m, y = a(y, f = a(f, h, m, y, g[u + 4], 6, 4149444226), h, m, g[u + 11], 10, 3174756917), f, h, g[u + 2], 15, 718787259), y, f, g[u + 9], 21, 3951481745),");
            sb.Append("     f = o(f, l),");
            sb.Append("     h = o(h, c),");
            sb.Append("     m = o(m, d),");
            sb.Append("     y = o(y, p);");
            sb.Append("     return (s(f) + s(h) + s(m) + s(y))");
            sb.Append("     .toLowerCase()");
            sb.Append("     }(token + \"&\" + s + \"&\" + a + \"&\" + data);");
            sb.Append("     return u;}");
            sb.Append("     ");
            sb.Append("     ");
            sb.Append(" }");
            sb.Append("}");
            return Helper.GetJsMethd(sb.ToString(), para);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //Geta("https://detail.tmall.com/item.htm?de_count=1&id=711237195274");
            var a= WinInetHelper.GetCookieString("https://item.taobao.com/");
            //CookieContainer myCookieContainer = new CookieContainer();
            //if (webBrowser.Document.Cookie != null) 
            //{
            //    string cookieStr = webBrowser.Document.Cookie;
            //    string[] cookstr = cookieStr.Split(';');
            //    foreach (string str in cookstr)
            //    {
            //        string[] cookieNameValue = str.Split('=');
            //        Cookie ck = new Cookie(cookieNameValue[0].Trim().ToString(), cookieNameValue[1].Trim().ToString());
            //        ck.Domain = "www.google.com";
            //        myCookieContainer.Add(ck);
            //    }
            //}
        }
       

        private void webBrowser_SizeChanged(object sender, EventArgs e)
        {

        }

        private void webBrowser_DocumentCompleted(object sender, System.Windows.Forms.WebBrowserDocumentCompletedEventArgs e)
        {

        }
    }
    public static class WinInetHelper
    {
        [DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern bool InternetGetCookieEx(string pchURL, string pchCookieName, StringBuilder pchCookieData, ref System.UInt32 pcchCookieData, int dwFlags, IntPtr lpReserved);

        [DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern int InternetSetCookieEx(string lpszURL, string lpszCookieName, string lpszCookieData, int dwFlags, IntPtr dwReserved);

        public static string GetCookieString(string url)
        {
            // Determine the size of the cookie
            uint datasize = 54010;
            StringBuilder cookieData = new StringBuilder((int)datasize);

            if (!InternetGetCookieEx(url, null, cookieData, ref datasize, 0x2000, IntPtr.Zero))
            {
                // Allocate stringbuilder large enough to hold the cookie
                cookieData = new StringBuilder((int)datasize);
                if (!InternetGetCookieEx(url, null, cookieData, ref datasize, 0x00002000, IntPtr.Zero))
                    return null;
            }
            return cookieData.ToString();
        }
    }
}
