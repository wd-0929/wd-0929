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
            using (WebClient webClient = new WebClient())
            {
                var webDatas = webClient.DownloadData("https://www.mzfort.com/items/CA29327-CA29328/3.jpg");
            }
        }
    }
}
