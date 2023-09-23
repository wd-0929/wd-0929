using Iop.Api;
using Newtonsoft.Json;
using NPOI.SS.Formula.Functions;
using Smt.SDK.Test.CategoryQcquisition;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Channels;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Security.Policy;
using System.Text;
using System.Threading;
using WheelTest.Style;
using WheelTest.Translate;

namespace Smt.SDK.Test
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //new TaobaoCategoryQcquisition();
            // new GZipFilesBuilder().OnBuild();
            //; ShopeeHttpGet();
            //postproduct();
            //SmtTemplateFreight.Create();
            //var a = Deserialize<AccountInfo>(new  { AccountId=12, AccountName="22222" }.ToJsonData());
        }
        [DataContract]
        public class AccountInfo
        {
            [DataMember]
            public int AccountId { get; set; }
            [Newtonsoft.Json.JsonIgnore]
            public string AccountName { get; set; }

            [DataMember]
            public string AccountPassword { get; set; }

            [DataMember]
            public string Phone { get; set; }

            [DataMember]
            public bool IsVerifyPhone { get; set; }


            [DataMember]
            public string WeChatOpenId { get; set; }

        }
        public static T Deserialize<T>(string jsonString)
        {
            return JsonConvert.DeserializeObject<T>(jsonString);
            try
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
                using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString)))
                {
                    return (T)serializer.ReadObject(ms);
                }
            }
            catch
            {
                throw new Exception("数据格式不正确");
            }
        }
        //public static void postproduct() 
        //{
        //    var Channel = "AE_GLOBAL";
        //    var ChannelSellerId = "2673675313";
        //    var accessToken = "5000050000923AawUSaCw132a369fPETCoks3rRgYbi2ckycvtgAfjuiKfKyIW1SPPt";
        //    var url = "https://api-sg.aliexpress.com/sync";
        //    string appkey = "24578631";
        //    string appSecret = "9de53317305962e960b1a8de288b734c";
        //    IIopClient client = new IopClient(url, appkey, appSecret);
        //    IopRequest request = new IopRequest();
        //    request.SetApiName("aliexpress.ascp.scitem.update");
        //    request.AddApiParameter("scitem_update_request", "{\"origin_box_package\":\"true\",\"biz_type\":\"288000\",\"length\":\"1\",\"specification\":\"test\u89C4\u683C\",\"weight\":\"1\",\"customs_unit_price\":\"1.2\",\"title\":\"\u6D4B\u8BD5-title\",\"is_hygroscopic\":\"true\",\"is_danger\":\"true\",\"GWeight\":\"1\",\"dangerous_type\":\"air_embargo\",\"feature\":\"{\\\"key\\\":\\\"value\\\"}\",\"width\":\"1\",\"include_battery\":\"1\",\"NWeight\":\"1\",\"sc_item_id\":\"2123423\",\"height\":\"1\"}");
        //    IopResponse response = client.Execute(request, accessToken,GopProtocolEnum.TOP);
        //    Console.WriteLine(response.IsError());
        //    Console.WriteLine(response.Body);
        //}
        public static void ShopeeHttpGet()
        {
            //var textData = System.IO.File.ReadAllText(@"C:\Users\Administrator\Desktop\Shopee采集文本.txt");
            Dictionary<string, string> headerDic = new Dictionary<string, string>();
            //foreach (var item in textData.SplitExt("\r\n"))
            //{
            //    if (!string.IsNullOrWhiteSpace(item))
            //        headerDic.Add(item.SplitExt(":")[0], item.Replace(item.SplitExt(":")[0]+":", ""));
            //}
            using (WebClient webClient=new WebClient())
            {
                webClient.DownloadFile("https://files.alicdn.com/tpsservice/c37222cc12fc4352b347c02c8771a341.xlsx?spm=5261.25812464.0.0.1c0b3648u3mgmw&file=c37222cc12fc4352b347c02c8771a341.xlsx", "C:\\Users\\Administrator\\Desktop\\ef1ae29e-f0fb-49cd-998d-c40c293aa612.xlsx");

            }
        }
    }
}
