using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using WheelTest.Style;

namespace WheelTest.Translate
{
    public static class Translate
    {
        //public Translate(string translateText)
        //{
        //    BaiDuTranslate(translateText);
        //}
        public static string BaiDuTranslate(string translateText)
        {
            List<string> strings = new List<string>();
            Dictionary<string, string> header = new Dictionary<string, string>();
            header.Add("Connection", " keep-alive");
            header.Add("sec-ch-ua", "Not A Brand\";v=\"99\", \"Chromium\";v=\"94\"");
            header.Add("sec-ch-ua-mobile", "?0");
            header.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/94.0.4606.71 Safari/537.36 Core/1.94.202.400 QQBrowser/11.9.5355.400");
            header.Add("Content-Type", "application/x-www-form-urlencoded; charset=UTF-8");
            header.Add("Accept", "*/*");
            header.Add("X-Requested-With", "XMLHttpRequest");
            header.Add("sec-ch-ua-platform", "\"Windows\"");
            header.Add("Sec-Fetch-Site", "same-origin");
            header.Add("Sec-Fetch-Mode", "cors");
            header.Add("Sec-Fetch-Dest", "empty");
            header.Add("Accept-Encoding", "gzip, deflate, br");
            header.Add("Accept-Language", "zh-CN,zh;q=0.9");
            var getCookie = Helper.GetCookie("https://fanyi.baidu.com/", header);
            Regex getCookieRegex = new Regex("BAIDUID=([^;]+);");
            if (getCookieRegex.IsMatch(getCookie))
            {
                var value = getCookieRegex.Match(getCookie).Groups[1].Value;
                header.Add("COOKIE", string.Format("BAIDUID={0}; BAIDUID_BFESS={0}", value));

                var bodyData = WebClientHelper.HttpWebClient("https://fanyi.baidu.com/", header);
                var gtk = string.Empty;
                var token = string.Empty;
                getCookieRegex = new Regex("window.gtk = \"([^\"]+)");
                if (getCookieRegex.IsMatch(bodyData))
                {
                    gtk = getCookieRegex.Match(bodyData).Groups[1].Value;
                }
                getCookieRegex = new Regex("token: '([^\']+)");
                if (getCookieRegex.IsMatch(bodyData))
                {
                    token = getCookieRegex.Match(bodyData).Groups[1].Value;
                }
                var body = string.Format("from=zh&to=en&query={0}&transtype=realtime&simple_means_flag=3&sign={1}&token={2}&domain=common&ts={3}", HttpUtility.UrlEncode(translateText), Helper.GetTranslateSign(translateText, gtk), token, Helper.GetTimestamp13());
                var bodyData1 = WebClientHelper.HttpWebClient("https://fanyi.baidu.com/v2transapi?from=zh&to=en", header, body);
                JObject job = JObject.Parse(bodyData1);
                var ss = job["trans_result"].Value<JArray>("data");
                foreach (var item in ss)
                {
                    strings.Add(item.Value<string>("dst"));
                }
            }
            return strings.ToJoin();
        }
        public static string AESDecrypt(string text)
        {
            try
            {
                var Key = "ydsecret://query/key/B*RGygVywfNBwpmBaZg*WT7SIOUP2T0C9WHMZN39j^DAdaZhAnxvGcCY6VYFwnHl";
                var IV = "ydsecret://query/iv/C@lZe2YzHtZ2CYgaXKSVfsb7Y4QWHjITPPZ0nQp87fBeJ!Iv6v^6fvi2WN@bYpJ4";
                //16进制数据转换成byte
                byte[] encryptedData = Convert.FromBase64String(text.Replace("_", "/").Replace("-", "+"));  // strToToHexByte(text);
                RijndaelManaged rijndaelCipher = new RijndaelManaged();
                rijndaelCipher.Key =  Key.GetMD5HashBytes();
                rijndaelCipher.IV = IV.GetMD5HashBytes();
                rijndaelCipher.Mode = CipherMode.CBC;
                rijndaelCipher.Padding = PaddingMode.PKCS7;
                ICryptoTransform transform = rijndaelCipher.CreateDecryptor();
                byte[] plainText = transform.TransformFinalBlock(encryptedData, 0, encryptedData.Length);
                string result = Encoding.UTF8.GetString(plainText);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static string YouDaoTranslate(string translateText)
        {
            {
                List<string> strings = new List<string>();
                Dictionary<string, string> header = new Dictionary<string, string>();
                header.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/94.0.4606.71 Safari/537.36 Core/1.94.202.400 QQBrowser/11.9.5355.400");
                header.Add("Content-Type", "application/x-www-form-urlencoded; charset=UTF-8");
                header.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
                header.Add("Accept-Encoding", "gzip, deflate");
                header.Add("Accept-Language", "zh-CN,zh;q=0.9");
                var getCookie = Helper.GetCookie("http://fanyi.youdao.com/", header);
                Regex getCookieRegex = new Regex("OUTFOX_SEARCH_USER_ID=([^;]+);");
                if (getCookieRegex.IsMatch(getCookie))
                {
                    var value = getCookieRegex.Match(getCookie).Groups[1].Value;
                    header.Add("COOKIE", string.Format("OUTFOX_SEARCH_USER_ID={0};", value));
                    header.Add("Referer", "https://fanyi.youdao.com/");
                    var r = "1694766537488";
                    var p = string.Format("client={0}&mysticTime={1}&product={2}&key={3}", "fanyideskweb", r, "webfanyi", "fsdsogkndfokasodnaso").GetMD5Hash();
                    var body = string.Format("i={0}&from={1}&to={2}&dictResult=true&keyid=webfanyi&sign={3}&client=fanyideskweb&product=webfanyi&appVersion=1.0.0&vendor=web&pointParam=client%2CmysticTime%2Cproduct&mysticTime={4}&keyfrom=fanyi.web&mid=1&screen=1&model=1&network=wifi&abtest=0&yduuid=abcdefg", HttpUtility.UrlEncode(translateText), "auto","EN", p, r);
                    var bodyData1 = WebClientHelper.HttpWebClient("https://dict.youdao.com/webtranslate", header, body);
                    var datas = AESDecrypt(bodyData1);
                    JObject job = JObject.Parse(datas);
                   var translateResults= job.Value<JArray>("translateResult");
                    foreach (var translateResult in translateResults)
                    {
                        strings.Add(translateResult.First.Value<string>("tgt")?.Trim());
                    }
                }
                return strings.ToJoin();
            }
        }

        public static void AesDecrypt()
        {
            //密钥和初始化向量

            //待解密字符串
            var encryptedString = "Z21kD9ZK1ke6ugku2ccWuwRmpItPkRr5XcmzOgAKD0GcaHTZL9kyNKkN2aYY6yiOCdq0J2ayJb8EfFpBc1v6SH_rdmXEFoCYzrh-_CYW9u0tO-yvDEF7spmIKPmmGY3_qztwTzTAzs1x6q08zWHrh5kx5F9TMuoOXK_gTX4qxj8-uHS9JGVBAK3wj1Y4Ukjj".Replace("_", "/").Replace("-", "+");
            AESDecrypt(encryptedString);
        }
    }
}
