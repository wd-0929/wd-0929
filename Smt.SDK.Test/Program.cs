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
            Dictionary<string, string> headerDic = new Dictionary<string, string>();
            headerDic.Add("Connection", "keep-alive");
            headerDic.Add("sec-ch-ua", "\"Chromium\";v=\"21\", \" Not;A Brand\";v=\"99\"");
            headerDic.Add("340228d9", "7T/h(>T??k\"@m@J:r6G:(^Zf4");
            headerDic.Add("Content-Type", "Content-Type");
            headerDic.Add("x-sz-sdk-version", "2.9.2-2&1.4.1");
            headerDic.Add("X-Shopee-Language", "pt-BR");
            headerDic.Add("X-Requested-With", "XMLHttpRequest");
            headerDic.Add("af-ac-enc-dat", "AAcyLjkuMi0yAAABidkjdacAABg8BCAAAAAAAAAAAj+vsCN1JmrO07Nj/3HXJNy9QGsYG8eCFMPb9y8MfhH57RBoogtRLmZkn7tL4fSAwB4RmvQ+Hbck0cMxc/iRNDmeU9q5aZZ+cqgHHZ3ryA3DphTWM1Gm/Vbzly1WD/bKLi+0GeZoLZalZfOBlXGNFeOLjR4JO8lDr5/zSRFuMDoZFxkNy5/422Z3KMXceu6eZ3UDS+ow0JNmbNmHwanPijG2Gl2OHtUqPF7pUXvv77N7iH4hyaXwkE2GoUtTmOOFr0fVNlT3fT+aD0RSs87TJszd7g5ZZucL5Tdmv1JtHBB6MUiooP++tGS9C/ya8h53KhwcDI7+N/b85J63HZXluidukfEpCDywXUbe1Xl1b1r4RKIhIHIO35NjPtsw7SwZxGawI2JYV6jnBubHVQ6qw1821PbpRpr2GcZbn7jeUN7oqoE89pf/NhMnw8f21UXD2mkb2JsAQ8Y9lVW9cL3Wx1rWu2p1JlK5T9YILoMnn3ik/mYiMR6VS53WR9UIiElv83d1mcmSNBl6IsJlRothKqB/kRXQVU9X4WU+nFdzyP1H3V3+V+wjmnzdUe0FKGFbU+DA0VA1uLy4zrkcVI5bzP6p1YKF9zB5KcdXI70Zg9ca89Hw02ysZzL1BZNqULZdDXbnRSjSZYMdnzJBiQMN8q/PJbAegqylczPMQi+Lw8nwmy4KmbtYe3rBxwUWvb8W7lL7z/JsZcXYzWOH7iGbel66TR1t+SybO0rEbLiR7JWjOIfiramn+52uA4nO3Y/HP6xbbFNaAW4ujyVdPNagLxZAQ/SJiwNIwJqikhcFaxcNjyV9E1Z7eY6xaWZMOva63BPHwc4UQBVmdGHCx9Lder4hJ/EnQegKIqDnFRws7lm0EkRRE7CGs4Jtau4mAXppU1u/JEs/pRC6+4UsjNjOiAFZwWml4SvQdsTuXY+c7pk2BnaprCspyVuoOk8InxI61dvMNt1sDLK/1WQeQv7NJSTWsrD4g4lf7ge+83GIJSD2Q8L9ENl+rRyD1TyuM4TS/w3iVqvR/bfgrbXVfimEODo26MGEjQW+NDUUZ7PYP7hsv/KvArJB4VR7cY6kdKWdzGsxe/Vz2ERGr+T0UMYLd9qENYmfMXP3Onk/h7bvn9NZyCvU3Xsg7vT4C213mojge2eTUXD6puWUDCNQnWs1gQSFkyS0HGCex/QGXEhm4zpKyDnbzc/441wNRX51v/uI5+Fs6pKX/zYTJ8PH9tVFw9ppG9ibl/82EyfDx/bVRcPaaRvYmyeoHKo3Fh/DG6F936U9hCbEpAl7FqG+TFDvI4S1M5wQe6t7zu7+8yXm19k9+z1nc6l7RVXkXczQN7uEd0D1mFzvCsz7QWXycCh0lhMBvXbA0RySdhyhKzToeXrsCr/7ViAHPuRW3s1F6aFm4aSBboc=");
            headerDic.Add("X-CSRFToken", "LhlTTfnQk6GPMi1dzxiYTkvI8uZmylu3");
            headerDic.Add("sec-ch-ua-platform", "\"Windows\"");
            headerDic.Add("sec-ch-ua-mobile", "?0");
            headerDic.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/95.0.4638.69 Safari/537.36");
            headerDic.Add("x-sap-ri", "483cd3645cf4d6caff1d543d7bdd8c9755c20517fe7f03d6");
            headerDic.Add("Accept", "application/json");
            headerDic.Add("edca1d7", "Y%IR3r3%i#l)ladXNYd0\\9O#n6\\_pr_!mL29eSd0\\j?_H#ZQJ>ooP.qT;OYLh)RuU#1LYX.iTX[(C)kufD>*K83?./lAlDAL7;HmPki7i5K\\PqC4NsTHJIY%*nH\\W)uIS)5Ea0!YqoJ7@$YhR=<iDCOnO_+,'&=e&SH9QPA4]Z7lO,W]4^QBZA)]A_W&,_nVQfeRtu^NPE\\<@<4Tu4H:l>u\"k+.[A;V3m3Q:Xraei'U)8o&f\\#Pi5;&<em\"l^Zc3$eE+)BmL9)'g-O:nZc#YHbtpJF+lm,S?X`B<>\",G+j5I3eJtaB<Z`f7jCG@E*5i\"b07N%c>BAF(m?dF;O;*L@L#\"nOPHu,*QR/8Z[2;V");
            headerDic.Add("X-API-SOURCE", "pc");
            headerDic.Add("35fd218a", "@JUR3KNE4R\"]I4JT]hQAi\\;I!");
            headerDic.Add("af-ac-enc-sz-token", "6hJcwn++9K81CVim6TzD5g==|loyVdKroo73D5yGeYRE+wi8q9Yn1lxCRkIzetrhYcicbkmFmC28Xhz/IUoKod4p3rGEdC4pN8Sc=|GvHFgD4X5JneOaaz|08|3");
            headerDic.Add("Sec-Fetch-Site", "same-origin");
            headerDic.Add("Sec-Fetch-Mode", "cors");
            headerDic.Add("Sec-Fetch-Dest", "empty");
            headerDic.Add("Accept-Encoding", "gzip, deflate, br");
            headerDic.Add("Accept-Language", "Accept-Language");
            headerDic.Add("Cookie", "REC_T_ID=48c5a755-f47c-11ed-9024-42d3cd50aece; SPC_R_T_ID=jKzlKuf2RqZL0xj1g0jGvHNQEeUlmNQbE4qTlsLw9YLyRLTG0GtmtYK8cve0+q/dnK1DqzXVfEabWEilRyvyrSJ1thHNE2u4wUS7UG3oT6mTXstbhjJZBHJT321Zcw6pxvVaZv1iPI0VP0fV+kXNRuQ005OSQrFXN5ripf/sskI=; SPC_R_T_IV=NDlQVDlCWWdlVERIVWxuWA==; SPC_T_ID=jKzlKuf2RqZL0xj1g0jGvHNQEeUlmNQbE4qTlsLw9YLyRLTG0GtmtYK8cve0+q/dnK1DqzXVfEabWEilRyvyrSJ1thHNE2u4wUS7UG3oT6mTXstbhjJZBHJT321Zcw6pxvVaZv1iPI0VP0fV+kXNRuQ005OSQrFXN5ripf/sskI=; SPC_T_IV=NDlQVDlCWWdlVERIVWxuWA==; SPC_F=wGYdPd5HexY24Yiibd9Daia1lsa4uWId; _gcl_au=1.1.1546365956.1684305016; _ga=GA1.1.126574235.1684305026; _ga_0GS478VKB8=GS1.1.1686208957.3.0.1686208966.51.0.0; SPC_SI=/mrQZAAAAAA4UWZxSmRPNIh4JAAAAAAAbGVIa29EVUI=; _QPWSDCXHZQA=ffcb2c0b-d0df-4843-accf-7ac4f703c2af; csrftoken=LhlTTfnQk6GPMi1dzxiYTkvI8uZmylu3; shopee_webUnique_ccd=6hJcwn%2B%2B9K81CVim6TzD5g%3D%3D%7CloyVdKroo73D5yGeYRE%2Bwi8q9Yn1lxCRkIzetrhYcicbkmFmC28Xhz%2FIUoKod4p3rGEdC4pN8Sc%3D%7CGvHFgD4X5JneOaaz%7C08%7C3; ds=5b163e3c672daa2e193afade513016ba");
            var a = Helper.HttpGet("https://shopee.com.br/api/v4/pdp/get_pc?item_id=12338656474&shop_id=392144922", headerDic, isHttps: true);
        }
    }
}
