using Iop.Api;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Security.Policy;
using System.Text;
using System.Threading;
using WheelTest.Style;

namespace Smt.SDK.Test
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ShopeeHttpGet();
            //postproduct();
            //SmtTemplateFreight.Create();
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
            var textData = System.IO.File.ReadAllText(@"C:\Users\Administrator\Desktop\Shopee采集文本.txt");
            Dictionary<string, string> headerDic = new Dictionary<string, string>();
            foreach (var item in textData.SplitExt("\r\n"))
            {
                if (!string.IsNullOrWhiteSpace(item))
                    headerDic.Add(item.SplitExt(":")[0], item.Replace(item.SplitExt(":")[0]+":", ""));
            }
            var a = Helper.HttpGet("https://shopee.com.br/api/v4/pdp/get_pc?shop_id=392144922&item_id=12338656474", headerDic, isHttps: true);
        }
    }
}
