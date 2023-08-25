using Newtonsoft.Json.Linq;
using Smt.SDK.Test.CategoryQcquisition;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Documents;
using WebLoginBLL;
using WheelTest.Style;

namespace Smt.SDK.Test
{
    public class TaobaoCategoryQcquisition : CategoryQcquisitionBase
    {

        public TaobaoCategoryQcquisition()
        {
            var MainLoginSetting = new MainLoginSetting
            {
                AccountName = "13142149841",
                AccountPassword = "wangzhe13.",
                LoginId = "fm-login-id",
                LoginPassword = "fm-login-password",
                Address = "https://myseller.taobao.com/home.htm/QnworkbenchHome/",
                LoginHost = "login.taobao.com",
                OriginalString = "myseller.taobao.com"
            };
            var webLoginInfoBLL = new WebLoginInfoBLL(MainLoginSetting);
            WebLoginInfoBLL.Init();
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
            GetTaobaoCategoryQcquisition();
            System.IO.File.WriteAllText(System.IO.Path.Combine(AppContext.BaseDirectory, "Cookie", "TaobaoCategory.txt"), CategoryDatas.ToJsonData());
        }
    
        public void GetTaobaoCategoryQcquisition(long catId = 0)
        {
            var url = "https://item.upload.taobao.com/router/asyncOpt.htm?optType=categorySelectChildren&fromSmart=true";
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
                            GetTaobaoCategoryQcquisition(childrenData.Value<long>("id"));
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
                        GetTaobaoCategoryQcquisition(dataSources.Value<long>("id"));
                    }
                }
            }
        }
    }
}
