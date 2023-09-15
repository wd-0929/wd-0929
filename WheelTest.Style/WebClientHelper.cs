using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Security;
using System.Net;
using System.Text;
using System.IO.Compression;
using System.Windows.Documents;

namespace WheelTest.Style
{
    public static class WebClientHelper
    {
        public static string HttpWebClient(string url, Dictionary<string, string> headerDic = null, string body = null, bool isHttps = false)
        {
            if (isHttps)
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Ssl3;
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback((a, b, c, d) => true);
            }
            try
            {
                string result = string.Empty;

                HttpWebRequest wbRequest = (HttpWebRequest)WebRequest.Create(url);
                if (body == null)
                {
                    wbRequest.Method = "GET";
                }
                else
                {
                    wbRequest.Method = "POST";
                    var bodyByte = Encoding.UTF8.GetBytes(body);
                    using (Stream requestStream = wbRequest.GetRequestStream())
                    {
                        requestStream.Write(bodyByte, 0, body.Length);
                        requestStream.Close();
                    }
                }
                if (headerDic != null && headerDic.Count > 0)
                {
                    AddHeader(wbRequest, headerDic);
                }
                HttpWebResponse wbResponse = (HttpWebResponse)wbRequest.GetResponse();
                return wbResponse.GetResponseString();
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
        private static string GetResponseString(this WebResponse response)
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
                                        //string[] cookstr = item.Value.Split(';');
                                        //CookieContainer myCookieContainer = new CookieContainer();
                                        //foreach (string str in cookstr)
                                        //{
                                        //    string[] cookieNameValue = str.Split('=');
                                        //    Cookie ck = new Cookie(cookieNameValue[0].Trim().ToString(), str.Replace(cookieNameValue[0]+"=", ""));
                                        //    ck.Domain = myHttpWebRequest.Host;
                                        //    myCookieContainer.Add(ck);
                                        //}
                                        //myHttpWebRequest.CookieContainer = myCookieContainer;
                                        myHttpWebRequest.Headers.Add("Cookie", item.Value);
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
    }
}
