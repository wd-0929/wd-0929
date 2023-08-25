using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using CollectLoginBLL;
using WheelTest.Style;

namespace Smt.SDK.Test.CategoryQcquisition
{
    internal class TmallCategoryQcquisition : CategoryQcquisitionBase
    {
        public TmallCategoryQcquisition()
        {
            CategoryDatas = new Dictionary<long, string>();
            var MainLoginSetting = new MainLoginSetting
            {
                Address = "https://myseller.taobao.com/home.htm/QnworkbenchHome/",
                LoginHost = "login.taobao.com",
                OriginalString = "myseller.taobao.com"
            };
            var webLoginInfoBLL = new CollectLoginInfoBLL(MainLoginSetting);
            CollectLoginInfoBLL.Init(null);
        rety:
            var cookie = webLoginInfoBLL.GetCookie();
            if (string.IsNullOrWhiteSpace(cookie))
            {
                webLoginInfoBLL.ExecCommand();
                goto rety;
            }
            CategoryDatas = new Dictionary<long, string>();
            List<string> list = new List<string>();
            var a = JArray.Parse(cookie);
            foreach (var item in a)
            {
                list.Add($@"{item.Value<string>("Name")}={item.Value<string>("Value")}");
            }
            webClient = new WebClient();
            webClient.Headers.Add(HttpRequestHeader.Cookie, list.ToJoin(";"));
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
