using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace WheelTest.Style
{
    public static class Helper
    {
        /// <summary>
        /// 四舍五入-保留小数
        /// </summary>
        /// <param name="value"></param>
        /// <param name="digit">几位小数 默认为2</param>
        /// <returns></returns>
        public static double ToRound(this double value, int digit = 2)
        {
            return Math.Round(value, digit);
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

        /// <summary>
        /// 分隔符的扩展 
        /// </summary>
        /// <param name="sourceString"></param>
        /// <param name="splitString"></param>
        /// <returns></returns>
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
    }
}
