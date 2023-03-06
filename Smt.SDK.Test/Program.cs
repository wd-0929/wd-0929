using Iop.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Security.Policy;
using System.Text;
using System.Threading;

namespace Smt.SDK.Test
{
    internal class Program
    {
        static void Main(string[] args)
        {
            SmtTemplateFreight.Create();
        }
        public void postproduct() 
        {
            var Channel = "AE_GLOBAL";
            var ChannelSellerId = "2673675313";
            var accessToken = "5000050000923AawUSaCw132a369fPETCoks3rRgYbi2ckycvtgAfjuiKfKyIW1SPPt";
            var url = "https://api-sg.aliexpress.com/sync";
            string appkey = "24578631";
            string appSecret = "9de53317305962e960b1a8de288b734c";
            IIopClient client = new IopClient(url, appkey, appSecret);  
            IopRequest request = new IopRequest();
            request.SetApiName("aliexpress.postproduct.redefining.findproductinfolistquery");
            request.AddApiParameter("aeop_a_e_product_list_query", "{\"product_status_type\":\"onSelling\"}");
            IopResponse response = client.Execute(request, accessToken, GopProtocolEnum.TOP);
            Console.WriteLine(response.IsError());
            Console.WriteLine(response.Body);
        }
    }
}
