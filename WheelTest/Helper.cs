using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO.Compression;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Diagnostics;
using System.Windows.Shapes;

namespace WheelTest
{
    public static class Helper
    {
        /// <summary>
        /// 执行程序命令
        /// </summary>
        /// <param name="arguments">命令</param>
        /// <exception cref="Exception"></exception>
        public static void ExecCommand(string programPath, string arguments)
        {
            Process proc = new Process();
            proc.StartInfo.FileName = programPath;
            //proc.StartInfo.WorkingDirectory = VideosPath;
            proc.StartInfo.Arguments = arguments;
            proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            proc.StartInfo.CreateNoWindow = true;
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.RedirectStandardError = true;
            List<string> errors = new List<string>();
            errors.Add(arguments);
            proc.ErrorDataReceived += new DataReceivedEventHandler((sender, e) =>
            {
                errors.Add(e.Data);
            });
            proc.Start();
            proc.BeginErrorReadLine();

            proc.WaitForExit();

            if (proc.ExitCode != 0)
            {
                throw new Exception("调用失败，详情请查看日志");
            }
#if DEBUG
            else
            {
            }
#endif
        }
        /// <summary>
        /// js的使用
        /// </summary>
        /// <param name="js"></param>
        /// <param name="para"></param>
        /// <returns></returns>
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
        public static void AddHeader(HttpWebRequest myHttpWebRequest, Dictionary<string, string> Headers)
        {
            if (Headers != null)
            {
                foreach (var item in Headers)
                {
                    if (!string.IsNullOrWhiteSpace(item.Key))
                    {
                        switch (item.Key.ToUpper())
                        {
                            case "ACCEPT":
                                myHttpWebRequest.Accept = item.Value;
                                break;
                            case "COBBECTION":
                                myHttpWebRequest.Connection = item.Value;
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
                            case "REFERE":
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
            }
        }

        public static void CreateSecurityProtocol()
        {
            var version = Environment.Version;
            if (version.Revision >= 42000)
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)(3072 | 768 | 192 | 48);
            else
            {
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)(192 | 48);
            }
        }
        public static string GetCookie(string url, Dictionary<string, string> Headers = null, string header = "Set-Cookie")
        {
            HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            AddHeader(myHttpWebRequest, Headers);
            myHttpWebRequest.CookieContainer = new CookieContainer();
            myHttpWebRequest.CookieContainer.SetCookies(new Uri(url), "");
            HttpWebResponse myresponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
            return myresponse.Headers[header];
        }
        private static string GenerateIP()
        {
            return new Random(new A().GetHashCode()).Next(0, 254) + "." + new Random(new A().GetHashCode()).Next(0, 254) + "." + new Random(new A().GetHashCode()).Next(0, 254) + "." + new Random(new A().GetHashCode()).Next(0, 254);
        }
        public static string GetStrMd5_32X(string ConvertString) //32位小写
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            string t2 = BitConverter.ToString(md5.ComputeHash(UTF8Encoding.Default.GetBytes(ConvertString)));
            t2 = t2.Replace("-", "");
            return t2.ToLower();

        }
        private class A { }

        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true;
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
    }
}
    
