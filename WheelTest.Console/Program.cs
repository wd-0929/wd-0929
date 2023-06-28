using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace WheelTest.Console
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Microsoft.Win32.RegistryKey rk = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Internet Settings", true);

            //设置代理可用
            rk.SetValue("ProxyEnable", 1);
            //设置代理IP和端口
            rk.SetValue("ProxyServer", "127.0.0.1:8888");
            rk.Close();
            //using (WebClient webClient = new WebClient())
            //{
            //    var webDatas = webClient.DownloadData("https://www.mzfort.com/items/CA29327-CA29328/3.jpg");
            //}
        }
    }
}
