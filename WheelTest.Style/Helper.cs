using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Security;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.CodeDom.Compiler;
using System.Reflection;
using System.IO.Compression;
using System.Threading;
using System.Drawing;
using Imazen.WebP;
using System.Windows.Media;
using System.Windows;

namespace WheelTest.Style
{
    public static class Helper
    {

        public static T ToEmun<T>(this object value, T defaultValue)
       where T : struct
        {
            if (value != null)
            {
                T result;
                if (Enum.TryParse<T>(value.ToString(), true, out result))
                {
                    return result;
                }
            }
            return defaultValue;
        }
        private static string ValiImageUrl(string url)
        {
            if (!string.IsNullOrEmpty(url))
            {
                if (!url.Contains("https:") && !url.Contains("http:"))
                {
                    url = "http:" + url;
                }
                Regex re = new Regex(@"_\d+x\d+.*$");
                if (re.IsMatch(url))
                {
                    url = re.Replace(url, "");
                }
            }
            return url;
        }
        public static string[] SplitExt(this string sourceString, string splitString)
        {
            List<string> arrayList = new List<string>();
            string s = string.Empty;
            while (sourceString.IndexOf(splitString) > -1)
            {
                s = sourceString.Substring(0, sourceString.IndexOf(splitString));
                sourceString = sourceString.Substring(sourceString.IndexOf(splitString) + splitString.Length);
                arrayList.Add(s);
            }
            arrayList.Add(sourceString);
            return arrayList.ToArray();
        }
        /// <summary>
        /// 四舍五入-保留小数
        /// </summary>
        /// <param name="value"></param>
        /// <param name="digit">几位小数 默认为2</param>
        /// <returns></returns>
        public static double ToRound(this double value, int digit = 2)
        {
            return Math.Round(value, digit, MidpointRounding.AwayFromZero);
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string ToJsonData(this object data)
        {
            if (data == null)
            {
                return null;
            }
            return JsonConvert.SerializeObject(data);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static T ToJsonData<T>(this string data)
        {
            if (string.IsNullOrWhiteSpace(data))
            {
                return default(T);
            }
            return JsonConvert.DeserializeObject<T>(data);
        }
        public static ReadOnlyCollection<T> ToReadOnlyCollection<T>(this IList<T> data)
        {
            if (data == null)
            {
                return null;
            }
            return new ReadOnlyCollection<T>(data);
        }
        public static ObservableCollection<T> ToObservableCollection<T>(this IList<T> data)
        {
            if (data == null)
            {
                return null;
            }
            return new ObservableCollection<T>(data);
        }
        public static byte[] GetMD5HashBytes(this string strHash_)
        {
            System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();

            byte[] bytes = Encoding.ASCII.GetBytes(strHash_);
            return md5.ComputeHash(bytes);
        }
        /// <summary>
        /// md5加密
        /// </summary>
        /// <param name="strHash_"></param>
        /// <returns></returns>
        public static string GetMD5Hash(this string strHash_)
        {
            System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();

            byte[] bytes = Encoding.ASCII.GetBytes(strHash_);

            byte[] encoded = md5.ComputeHash(bytes);

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < encoded.Length; ++i)
            {
                sb.Append(encoded[i].ToString("x2"));

            }

            return sb.ToString();
        }
        public static string MD5Encrypt16(this string password)
        {
            var md5 = new MD5CryptoServiceProvider();
            string t2 = BitConverter.ToString(md5.ComputeHash(Encoding.Default.GetBytes(password)), 4, 8);
            t2 = t2.Replace("-", "");
            return t2;
        }
        public static string HttpGet(string url, Dictionary<string, string> headerDic = null, bool isHttps = false, Encoding encoding = null)
        {
            if (encoding == null)
            {
                encoding = Encoding.Default;
            }
            if (isHttps)
            {
                var version = Environment.Version;
                if (version.Revision >= 42000)
                    ServicePointManager.SecurityProtocol = (SecurityProtocolType)(3072 | 768 | 192 | 48);
                else
                {
                    ServicePointManager.SecurityProtocol = (SecurityProtocolType)(192 | 48);
                }
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback((a, b, c, d) => true);
            }
            try
            {
                string result = string.Empty;

                HttpWebRequest wbRequest = (HttpWebRequest)WebRequest.Create(url);

                wbRequest.Method = "GET";
                if (headerDic != null && headerDic.Count > 0)
                {
                    AddHeader(wbRequest, headerDic);
                }
                HttpWebResponse wbResponse = (HttpWebResponse)wbRequest.GetResponse();
                result = GetHttpWebRulest(wbResponse);
                return result;
            }
            catch (WebException ex)
            {

                using (Stream responseStream = ex.Response.GetResponseStream())
                {
                    using (StreamReader sread = new StreamReader(responseStream))
                    {
                        return sread.ReadToEnd();
                    }
                }
                throw ex;
            }
        }
        public static void AddHeader(HttpWebRequest myHttpWebRequest, Dictionary<string, string> Headers)
        {
            if (Headers != null)
            {
                foreach (var item in Headers)
                {
                    try
                    {
                        if (!string.IsNullOrWhiteSpace(item.Key))
                        {
                            switch (item.Key.ToUpper())
                            {
                                case "ACCEPT":
                                    myHttpWebRequest.Accept = item.Value;
                                    break;
                                case "COBBECTION":
                                case "CONNECTION":
                                    myHttpWebRequest.Connection = item.Value;
                                    break;
                                case "COOKIE":
                                    if (!string.IsNullOrWhiteSpace(item.Value))
                                    {
                                        string[] cookstr = item.Value.Split(';');
                                        CookieContainer myCookieContainer = new CookieContainer();
                                        foreach (string str in cookstr)
                                        {
                                            string[] cookieNameValue = str.Split('=');
                                            Cookie ck = new Cookie(cookieNameValue[0].Trim().ToString(), str.Replace(cookieNameValue[0]+"=","").Trim().ToString());
                                            ck.Domain = myHttpWebRequest.Host;
                                            myCookieContainer.Add(ck);
                                        }
                                        myHttpWebRequest.CookieContainer = myCookieContainer;
                                    }
                                    break;
                                case "CONTENT-LENGTH":
                                    try
                                    {
                                        myHttpWebRequest.ContentLength = long.Parse(item.Value);
                                    }
                                    catch (Exception)
                                    {

                                    }
                                    break;
                                case "CONTENT-TYPE":
                                case "CONTENTTYPE":
                                    myHttpWebRequest.ContentType = item.Value;
                                    break;
                                case "EXPECT":
                                    myHttpWebRequest.Expect = item.Value;
                                    break;
                                case "DATE":
                                    try
                                    {
                                        myHttpWebRequest.Date = DateTime.Parse(item.Value);
                                    }
                                    catch (Exception)
                                    {

                                    }
                                    break;
                                case "HOST":
                                    myHttpWebRequest.Host = item.Value;
                                    break;
                                case "IF-MODIFIED-SINCE":
                                    try
                                    {
                                        myHttpWebRequest.IfModifiedSince = DateTime.Parse(item.Value);
                                    }
                                    catch (Exception)
                                    {

                                    }
                                    break;
                                //case "Range":
                                //    myHttpWebRequest.ad = item.Value;
                                //    break;
                                case "REFERER":
                                    myHttpWebRequest.Referer = item.Value;
                                    break;
                                case "TRANSFET-ENCODING":
                                    myHttpWebRequest.TransferEncoding = item.Value;
                                    break;
                                case "USER-AGENT":
                                    myHttpWebRequest.UserAgent = item.Value;
                                    break;
                                default:
                                    myHttpWebRequest.Headers[item.Key] = item.Value;
                                    break;
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
        }

        /// <summary>
        /// 签名
        /// </summary>
        /// <param name="urlPath"></param>
        /// <param name="paramDic"></param>
        /// <returns></returns>
        public static string Sign(string urlPath, Dictionary<string, string> paramDic, string AppSecret)
        {
            byte[] signatureKey = Encoding.UTF8.GetBytes(AppSecret);//此处用自己的签名密钥
            List<string> list = new List<string>();
            foreach (KeyValuePair<string, string> kv in paramDic)
            {
                list.Add(kv.Key + kv.Value);
            }
            list.Sort();
            string tmp = urlPath;
            foreach (string kvstr in list)
            {
                tmp = tmp + kvstr;
            }

            //HMAC-SHA1
            HMACSHA1 hmacsha1 = new HMACSHA1(signatureKey);
            hmacsha1.ComputeHash(Encoding.UTF8.GetBytes(tmp));
            byte[] hash = hmacsha1.Hash;
            //TO HEX
            return BitConverter.ToString(hash).Replace("-", string.Empty).ToUpper();
        }

        /// <summary>
        /// 1970年1月1日起至今的时间转换为毫秒
        /// </summary>
        /// <returns></returns>
        public static string Timestamp()
        {
            return (DateTime.Now - DateTime.Parse("1970-1-1 GMT")).TotalMilliseconds.ToString("F0");
        }
     
        /// <summary>
        /// 把数组所有元素，按照“参数=参数值”的模式用“&”字符拼接成字符串
        /// </summary>
        /// <param name="sArray">需要拼接的数组</param>
        /// <returns>拼接完成以后的字符串</returns>
        public static string CreateLinkString(SortedDictionary<string, string> dicArray)
        {
            StringBuilder prestr = new StringBuilder();
            foreach (KeyValuePair<string, string> temp in dicArray)
            {
                if (temp.Key.ToLower() != "sign" && temp.Key.ToLower() != "sign_type" && temp.Value != "" && temp.Value != null)
                    prestr.Append(temp.Key + "=" + temp.Value + "&");
            }

            //去掉最後一個&字符
            int nLen = prestr.Length;
            prestr.Remove(nLen - 1, 1);

            return prestr.ToString();
        }

        /// <summary>
        /// 把数组所有元素，按照“参数=参数值”的模式用“&”字符拼接成字符串，并对参数值做urlencode
        /// </summary>
        /// <param name="sArray">需要拼接的数组</param>
        /// <param name="code">字符编码</param>
        /// <returns>拼接完成以后的字符串</returns>
        public static string CreateLinkStringUrlencode(SortedDictionary<string, string> dicArray, Encoding code)
        {
            StringBuilder prestr = new StringBuilder();
            foreach (KeyValuePair<string, string> temp in dicArray)
            {
                prestr.Append(temp.Key + "=" + HtmlEncode(temp.Value) + "&");
            }

            //去掉最後一個&字符
            int nLen = prestr.Length;
            prestr.Remove(nLen - 1, 1);

            return prestr.ToString();
        }
        public static string HtmlDecode(string url)
        {
            return HtmlAgilityPack.HtmlEntity.DeEntitize(url);
        }
        public static string HtmlEncode(string url)
        {
            return HtmlAgilityPack.HtmlEntity.Entitize(url);
        }
        public static string MD5(byte[] datas)
        {
            StringBuilder sb = new StringBuilder();
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] t = md5.ComputeHash(datas);
            for (int i = 0; i < t.Length; i++)
            {
                sb.Append(t[i].ToString("x").PadLeft(2, '0'));
            }

            return sb.ToString();
        }
        #region 一个算法-组合
        public static List<List<T>> AllCombinationsOf<T>(List<List<T>> sets)
        {
            var combinations = new List<List<T>>();
            foreach (var value in sets[0])
                combinations.Add(new List<T> { value });
            foreach (var set in sets.Skip(1))
                combinations = AddExtraSet(combinations, set);
            return combinations;
        }

        private static List<List<T>> AddExtraSet<T>(List<List<T>> combinations, List<T> set)
        {
            var newCombinations = from value in set from combination in combinations select new List<T>(combination) { value };
            return newCombinations.ToList();
        }
        public static List<int> Conversion(int a)
        {
            List<int> listints = new List<int>();
            for (int i = 0; i < a; i++)
            {
                listints.Add(i);
            }
            return listints;
        }
        #endregion

        #region 1688和淘宝采集
        public static string GetCookie(string url, Dictionary<string, string> Headers = null, string header = "Set-Cookie")
        {
            HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            AddHeader(myHttpWebRequest, Headers);
            //停止重定向
            myHttpWebRequest.AllowAutoRedirect = false;
            myHttpWebRequest.CookieContainer = new CookieContainer();
            myHttpWebRequest.CookieContainer.SetCookies(new Uri(url), "");
            HttpWebResponse myresponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
            return myresponse.Headers[header];
        }
        public static string GetCookie(string url, string body, Dictionary<string, string> Headers = null)
        {
            HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            myHttpWebRequest.Method = "post";
            AddHeader(myHttpWebRequest, Headers);
            var byteArray = Encoding.UTF8.GetBytes(body);
            using (Stream newStream = myHttpWebRequest.GetRequestStream())
            {
                newStream.Write(byteArray, 0, byteArray.Length);//写入参数
            }
            myHttpWebRequest.CookieContainer = new CookieContainer();
            myHttpWebRequest.CookieContainer.SetCookies(new Uri(url), "");
            HttpWebResponse myresponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
            return myresponse.Headers["Set-Cookie"];
        }
        private static string appKey = "12574478";
        public static string GetTaobao_m_h5_tk(string url, string host, string jsonData, string method = "post")
        {
            string _m_h5_tk = string.Empty;
            string _m_h5_tk1 = string.Empty;
            string _m_h5_tk_enc = string.Empty;
            int index = 0;
            var _1688token = Resource1._1688Token;
        rety:
            var t = GetTimestamp();
            var sign = GetJsMethd(_1688token, new object[] { jsonData, _m_h5_tk, appKey, t });
            var Url = string.Format(url, appKey, t, sign);
            if (method != "post")
            {
                Url = Url + "&data=" + HtmlEncode(jsonData);
            }
            var referer = host;
            if (string.IsNullOrWhiteSpace(_m_h5_tk))
            {
                Dictionary<string, string> header = new Dictionary<string, string>();
                header.Add("ContentType", "application/x-www-form-urlencoded");
                header.Add("Referer", referer);
                header.Add("Accept-Encoding", "gzip, deflate, br");
                header.Add("Accept-Language", "zh-CN,zh;q=0.9");
                var getCookie = GetCookie(Url, "data=" + HtmlEncode(jsonData), header);
                Regex _m_h5_tkRegex = new Regex("_m_h5_tk=([^_]+)_([^;]+);");
                Regex _m_h5_tk_encRegex = new Regex("_m_h5_tk_enc=([^;]+);");
                if (_m_h5_tkRegex.IsMatch(getCookie) && _m_h5_tk_encRegex.IsMatch(getCookie))
                {
                    _m_h5_tk = _m_h5_tkRegex.Match(getCookie).Groups[1].Value;
                    _m_h5_tk1 = _m_h5_tkRegex.Match(getCookie).Groups[1].Value + "_" + _m_h5_tkRegex.Match(getCookie).Groups[2].Value;
                    _m_h5_tk_enc = _m_h5_tk_encRegex.Match(getCookie).Groups[1].Value;
                    goto rety;
                }
            }

            HttpWebRequest request = HttpWebRequest.Create(Url) as HttpWebRequest;
            request.ContentType = "application/x-www-form-urlencoded";
            request.Referer = referer;
            request.Headers["Accept-Encoding"] = "gzip, deflate, br";
            request.Headers["Accept-Language"] = "zh-CN,zh;q=0.9";
            request.Method = method;
            request.Headers.Add(HttpRequestHeader.Cookie, string.Format("_m_h5_tk={0}; _m_h5_tk_enc={1}", _m_h5_tk1, _m_h5_tk_enc));
            if (method == "post")
            {
                var byteArray = Encoding.UTF8.GetBytes("data=" + HtmlEncode(jsonData));
                using (Stream newStream = request.GetRequestStream())
                {
                    newStream.Write(byteArray, 0, byteArray.Length);//写入参数
                }
            }
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                var rulest = GetHttpWebRulest(response);
                if (rulest.Contains("哎哟喂,被挤爆啦,请稍后重试") && index < 3)
                {
                    _m_h5_tk = string.Empty;
                    Thread.Sleep(5000);
                    index++;
                    goto rety;
                }
                return rulest;
            }
        }
        public static string GetHttpWebRulest(HttpWebResponse response)
        {

            var contentEncoding = response.Headers[HttpResponseHeader.ContentEncoding];
            if (contentEncoding != null)
            {
                contentEncoding = contentEncoding.ToLower();
                if (contentEncoding.Contains("gzip"))
                {
                    using (GZipStream stream = new GZipStream(response.GetResponseStream(), CompressionMode.Decompress))
                    {
                        using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                        {
                            return reader.ReadToEnd();
                        }
                    }
                }
                else if (contentEncoding.Contains("deflate"))
                {
                    using (DeflateStream stream = new DeflateStream(response.GetResponseStream(), CompressionMode.Decompress))
                    {
                        using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                        {

                            return reader.ReadToEnd();
                        }

                    }
                }
            }
            using (Stream stream = response.GetResponseStream())
            {
                using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                {
                    return reader.ReadToEnd();
                }
            }
        }
        public static string GetTimestamp()
        {
            return ((long)(DateTime.Now.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds).ToString();
        }
        public static string GetTimestamp13()
        {
            //ToUniversalTime()转换为标准时区的时间,去掉的话直接就用北京时间
            TimeSpan ts = DateTime.Now.ToUniversalTime() - new DateTime(1970, 1, 1);
            //得到精确到毫秒的时间戳（长度13位）
            long time = (long)ts.TotalMilliseconds;
            return time.ToString();
        }
        public static string GetJsMethd(string js, object[] para)
        {
            CompilerParameters parameters = new CompilerParameters();

            parameters.GenerateInMemory = true;

            CodeDomProvider _provider = new Microsoft.JScript.JScriptCodeProvider();

            CompilerResults results = _provider.CompileAssemblyFromSource(parameters, js);

            Assembly assembly = results.CompiledAssembly;

            Type _evaluateType = assembly.GetType("aa.JScript");

            object obj = _evaluateType.InvokeMember("test", BindingFlags.InvokeMethod,
            null, null, para);
            return obj.ToString();
        }
        #endregion

        public static Bitmap WebpConvrtJpg(byte[] b)
        {
            try
            {
                var det = new SimpleDecoder();
                return det.DecodeFromBytes(b, b.Length);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    
        /// <summary>
        /// 是否包含
        /// </summary>
        /// <param name="source">原始集合</param>
        /// <param name="source">原始集合完全包含的数据</param>
        /// <returns></returns>
        public static bool IsAssignment(this IEnumerable<string> source, IEnumerable<string> include)
        {
            bool IsAssignment = true;
            if (!source.IsEmptyAndGreaterThanZero() && include.IsEmptyAndGreaterThanZero())
            {
                IsAssignment = false;
            }
            else
            {
                ///是否包含
                foreach (var item in include)
                {
                    //不包含就返回false
                    if (!source.Contains(item))
                    {
                        IsAssignment = false;
                    }
                }
            }
            return IsAssignment;
        }
        public static bool IsAssignment(this IEnumerable<long> source, IEnumerable<long> include)
        {
            bool IsAssignment = true;
            if (!source.IsEmptyAndGreaterThanZero() && include.IsEmptyAndGreaterThanZero())
            {
                IsAssignment = false;
            }
            else
            {
                ///是否包含
                foreach (var item in include)
                {
                    //不包含就返回false
                    if (!source.Contains(item))
                    {
                        IsAssignment = false;
                    }
                }
            }
            return IsAssignment;
        }
        public static string ToJoin(this IEnumerable<string> data, string join = "\r\n")
        {
            if (!data.IsEmptyAndGreaterThanZero())
            {
                return null;
            }
            return string.Join(join, data);
        }
        /// <summary>
        /// 重复项扩展
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="source">根据某个字段或者属性来去除重复项</param>
        /// <param name="keySelector"></param>
        /// <returns></returns>
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }
        /// <summary>
        /// 找寻子控件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static List<T> GetChildObjects<T>(DependencyObject obj, string name) where T : FrameworkElement
        {
            DependencyObject child = null;
            List<T> childList = new List<T>();
            for (int i = 0; i <= VisualTreeHelper.GetChildrenCount(obj) - 1; i++)
            {
                child = VisualTreeHelper.GetChild(obj, i);
                if (child is T && (((T)child).Name == name || string.IsNullOrEmpty(name)))
                {
                    childList.Add((T)child);
                }
                childList.AddRange(GetChildObjects<T>(child, ""));
            }
            return childList;
        }
        
        public static string GetTranslateSign(string t,string r)
        {
            var baidufanyi = Resource1.baidufanyi;
            var sign = GetJsMethd(baidufanyi, new object[] { t, r });
            return sign;
        }
    }
}
