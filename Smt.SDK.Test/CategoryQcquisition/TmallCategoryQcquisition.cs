using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WheelTest.Style;

namespace Smt.SDK.Test.CategoryQcquisition
{
    internal class TmallCategoryQcquisition : CategoryQcquisitionBase
    {
        public TmallCategoryQcquisition()
        {
            CategoryDatas = new Dictionary<long, string>();
            webClient = new WebClient();
            webClient.Headers.Add(HttpRequestHeader.Cookie, "cna=+DnJHEl78iUCAXF2wKs4jn7y; lgc=%5Cu61F5%5Cu61C2%5Cu7684%5Cu5C11%5Cu5E741135; tracknick=%5Cu61F5%5Cu61C2%5Cu7684%5Cu5C11%5Cu5E741135; sg=58a; mt=ci=1_1; cookie2=1b3fb8f52f8d4f7ab89602aaf32b8612; t=c26ce8116f1f8cd1dcaeb7ee699784bc; _tb_token_=7eeeb4edb3333; _samesite_flag_=true; sgcookie=E100gGWoB7oY5WQMRQVym246WflKtpFPlclDm9PrngqZnX3qPs3%2FypTKyh0eY%2FT5GB5WIa3lBJQtIZQ%2BOP4w4qOWZHz4xRJtdZfutP9spzhyViY%3D; unb=2689081988; uc1=pas=0&cookie15=VT5L2FSpMGV7TQ%3D%3D&cookie16=V32FPkk%2FxXMk5UvIbNtImtMfJQ%3D%3D&cookie21=URm48syIYBrb0wDboXk1&cookie14=Uoe9b6GhrblprQ%3D%3D&existShop=false; uc3=id2=UU6p81V%2BDcMPcA%3D%3D&lg2=U%2BGCWk%2F75gdr5Q%3D%3D&nk2=gKBTTjZZNLcOCX%2FX0AA%3D&vt3=F8dCsGZKBhA8qrx3BIU%3D; csg=ec4e072f; cancelledSubSites=empty; cookie17=UU6p81V%2BDcMPcA%3D%3D; dnk=%5Cu61F5%5Cu61C2%5Cu7684%5Cu5C11%5Cu5E741135; skt=48ce6f5018313e8c; existShop=MTY5Mjg2NjkyNA%3D%3D; uc4=nk4=0%40gut5c5bfxVqVyZ7BdjT3CmP7R0g82Fll8g%3D%3D&id4=0%40U2xkaXiV03OD1dn%2BhZOkd%2BN8kCFw; _cc_=U%2BGCWk%2F7og%3D%3D; _l_g_=Ug%3D%3D; _nk_=%5Cu61F5%5Cu61C2%5Cu7684%5Cu5C11%5Cu5E741135; cookie1=B0b5kj%2BdthNKW%2FFJOjTbZixuFC8TYdJv5qQ5%2BPNmxgQ%3D; _m_h5_tk=247c9f5c3a76eb34e378e6856268c1f6_1692876287346; _m_h5_tk_enc=553548a8da3a62c13799c7f89e52b5ae; isg=BNracBaVajIZceZ5ESyYb57yK4D8C17lQ5lcJ-RT1G0mV3uRzJtm9Z5hJyNLh9Z9");
            GetTmallCategoryQcquisition();
            System.IO.File.WriteAllText(System.IO.Path.Combine(AppContext.BaseDirectory, "Cookie", "TmallCategory.txt"), CategoryDatas.ToJsonData());
        }
        public string GetCookie()
        {
            var psss = System.IO.Path.Combine(AppContext.BaseDirectory, "Cookie", "myseller.taobao.com", "cookie.txt");
            if (System.IO.File.Exists(psss))
                return System.IO.File.ReadAllText(psss);
            return null;
        }
        public void GetTmallCategoryQcquisition(long catId = 0)
        {
            var url = "https://baike.taobao.com/json/allStdCategoryJsonAjaxNew.do?_input_charset=utf-8&appSource=tmall&type=0";
            if (catId > 0)
            {
                url = url + string.Format("&catId={0}", catId);
            }
            var datas = Encoding.UTF8.GetString(webClient.DownloadData(url));
            var data = JObject.Parse(datas);
            foreach (var dataSources in data["data"].Value<JArray>("dataSource"))
            {
                if (dataSources.Value<JArray>("children").IsEmptyAndGreaterThanZero())
                {
                    foreach (var childrenData in dataSources.Value<JArray>("children"))
                    {
                        if (childrenData.Value<bool>("leaf"))
                        {
                            CategoryDatas.Add(childrenData.Value<long>("id"), childrenData.Value<string>("name"));
                        }
                        else
                        {
                            CategoryDatas.Add(childrenData.Value<long>("id"), childrenData.Value<string>("name"));
                            GetTmallCategoryQcquisition(childrenData.Value<long>("id"));
                        }
                    }
                }
                else
                {
                    if (dataSources.Value<bool>("leaf"))
                    {
                        CategoryDatas.Add(dataSources.Value<long>("id"), dataSources.Value<string>("name"));
                    }
                    else
                    {
                        CategoryDatas.Add(dataSources.Value<long>("id"), dataSources.Value<string>("name"));
                        GetTmallCategoryQcquisition(dataSources.Value<long>("id"));
                    }
                }
            }
        }
    }
}
