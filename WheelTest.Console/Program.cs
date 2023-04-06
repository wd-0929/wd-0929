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
                var webDatas = webClient.DownloadString("https://www.aliexpress.us/item/3256804854588038.html?pdp_ext_f=%7B%22ship_from%22%3A%22CN%22%2C%22sku_id%22%3A%2212000031427760636%22%7D&scm=1007.28480.318308.0&scm_id=1007.28480.318308.0&scm-url=1007.28480.318308.0&pvid=293b97b1-4951-4e22-aa30-4bbba4340911&utparam=%257B%2522process_id%2522%253A%25221102%2522%252C%2522x_object_type%2522%253A%2522product%2522%252C%2522pvid%2522%253A%2522293b97b1-4951-4e22-aa30-4bbba4340911%2522%252C%2522belongs%2522%253A%255B%257B%2522id%2522%253A%252232094160%2522%252C%2522type%2522%253A%2522dataset%2522%257D%255D%252C%2522pageSize%2522%253A%252210%2522%252C%2522language%2522%253A%2522en%2522%252C%2522scm%2522%253A%25221007.28480.318308.0%2522%252C%2522countryId%2522%253A%2522US%2522%252C%2522scene%2522%253A%2522SD-Waterfall%2522%252C%2522tpp_buckets%2522%253A%252221669%25230%2523265320%252372_21669%25234190%252319162%2523452_18480%25230%2523318308%25230_18480%25233885%252317672%25233%2522%252C%2522x_object_id%2522%253A%25223256804854588038%2522%257D&pdp_npi=2%40dis%21USD%21US%20%2475.06%21US%20%2431.78%21%21%21%21%21%402132a24f16802562488174989ef102%2112000031427760636%21gsd&spm=a2g0o.11810135.waterfall.0&aecmd=true&gatewayAdapt=glo2usa4itemAdapt&_randl_shipto=US  ");
            }
        }
    }
}
