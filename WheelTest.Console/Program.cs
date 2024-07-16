using System.IO;
using System.Text;
using WheelTest.Style;
using System.Runtime.Serialization.Json;
using System.Collections.Generic;
using System.Collections;
using System;
using Newtonsoft.Json.Linq;

namespace WheelTest.Console
{
    internal class Program
    {
        static void Main(string[] args)
        {
            {
                //var text = "[{\"Code\":\"USD\",\"Name\":\"美元\",\"AuxiliaryUnits\":2},{\"Code\":\"CNY\",\"Name\":\"人民币\",\"AuxiliaryUnits\":2},{\"Code\":\"JPY\",\"Name\":\"日元\",\"AuxiliaryUnits\":0},{\"Code\":\"CAD\",\"Name\":\"加拿大元\",\"AuxiliaryUnits\":2},{\"Code\":\"GBP\",\"Name\":\"英镑\",\"AuxiliaryUnits\":2},{\"Code\":\"AUD\",\"Name\":\"澳大利亚元\",\"AuxiliaryUnits\":2},{\"Code\":\"NZD\",\"Name\":\"新西兰元\",\"AuxiliaryUnits\":2},{\"Code\":\"EUR\",\"Name\":\"欧元\",\"AuxiliaryUnits\":2},{\"Code\":\"MXN\",\"Name\":\"墨西哥比索\",\"AuxiliaryUnits\":2},{\"Code\":\"PLN\",\"Name\":\"波兰兹罗提\",\"AuxiliaryUnits\":2},{\"Code\":\"SEK\",\"Name\":\"瑞典克朗\",\"AuxiliaryUnits\":2},{\"Code\":\"CHF\",\"Name\":\"瑞士法郎\",\"AuxiliaryUnits\":2},{\"Code\":\"KRW\",\"Name\":\"韩元\",\"AuxiliaryUnits\":2},{\"Code\":\"SAR\",\"Name\":\"沙特里亚尔\",\"AuxiliaryUnits\":2},{\"Code\":\"SGD\",\"Name\":\"新加坡元\",\"AuxiliaryUnits\":2},{\"Code\":\"AED\",\"Name\":\"阿联酋迪拉姆\",\"AuxiliaryUnits\":2},{\"Code\":\"KWD\",\"Name\":\"科威特第纳尔\",\"AuxiliaryUnits\":3},{\"Code\":\"NOK\",\"Name\":\"挪威克朗\",\"AuxiliaryUnits\":2},{\"Code\":\"CLP\",\"Name\":\"智利比索\",\"AuxiliaryUnits\":0},{\"Code\":\"MYR\",\"Name\":\"马来西亚林吉特\",\"AuxiliaryUnits\":2},{\"Code\":\"PHP\",\"Name\":\"菲律宾比索\",\"AuxiliaryUnits\":2},{\"Code\":\"TWD\",\"Name\":\"新台湾元\",\"AuxiliaryUnits\":2},{\"Code\":\"THB\",\"Name\":\"泰铢\",\"AuxiliaryUnits\":2},{\"Code\":\"QAR\",\"Name\":\"卡塔尔里亚尔\",\"AuxiliaryUnits\":2},{\"Code\":\"JOD\",\"Name\":\"约旦第纳尔\",\"AuxiliaryUnits\":3},{\"Code\":\"BRL\",\"Name\":\"巴西雷亚尔\",\"AuxiliaryUnits\":2},{\"Code\":\"OMR\",\"Name\":\"阿曼里亚尔\",\"AuxiliaryUnits\":3},{\"Code\":\"BHD\",\"Name\":\"巴林第纳尔\",\"AuxiliaryUnits\":3},{\"Code\":\"ILS\",\"Name\":\"以色列新锡克尔\",\"AuxiliaryUnits\":2},{\"Code\":\"ZAR\",\"Name\":\"南非兰特\",\"AuxiliaryUnits\":2},{\"Code\":\"CZK\",\"Name\":\"捷克克朗\",\"AuxiliaryUnits\":2,\"IsDecimalPoint\":false},{\"Code\":\"HUF\",\"Name\":\"匈牙利福林\",\"AuxiliaryUnits\":2,\"IsDecimalPoint\":false},{\"Code\":\"DKK\",\"Name\":\"丹麦克朗\",\"AuxiliaryUnits\":2},{\"Code\":\"RON\",\"Name\":\"罗马尼亚列伊\",\"AuxiliaryUnits\":2},{\"Code\":\"BGN\",\"Name\":\"保加利亚列瓦\",\"AuxiliaryUnits\":2},{\"Code\":\"HKD\",\"Name\":\"港元\",\"AuxiliaryUnits\":2},{\"Code\":\"COP\",\"Name\":\"哥伦比亚比索\",\"AuxiliaryUnits\":2},{\"Code\":\"GEL\",\"Name\":\"格鲁吉亚拉里\",\"AuxiliaryUnits\":2}]";
                //var text1 = "USD（$）--CNY（¥）--JPY（￥）--CAD（CA$）--GBP（£）--AUD（AU$）--NZD（NZ$）--EUR（€）--MXN（MX$）--PLN（zł）--SEK（kr）--CHF（CHF）--KRW（₩）--SAR（SAR）--SGD（S$）--AED（AED）--KWD（KWD）--NOK（kr）--CLP（CLP$）--MYR（RM）--PHP（₱）--TWD（NT$）--THB（฿）--QAR（QAR）--JOD（JOD）--BRL（R$）--OMR（OMR）--BHD（BHD）--ILS（₪）--ZAR（R）--CZK（Kč）--HUF（Ft）--DKK（kr.）--RON（Lei）--BGN（лв.）--HKD（HK$）--COP（$）--GEL（₾）";
                //var texts = text.SplitExt(",");
                //var texts1 = text1.SplitExt("--");
                //JArray pairs= JArray.Parse(text);
                //ArrayList datas= new ArrayList();    
                //for (int i = 0; i < pairs.Count; i++)
                //{
                //    datas.Add(new 
                //    {
                //        Code = pairs[i].Value<string>("Code"),
                //        Name = pairs[i].Value<string>("Name"),
                //        AuxiliaryUnits = pairs[i].Value<int>("AuxiliaryUnits"),
                //        IsDecimalPoint = pairs[i].Value<bool?>("IsDecimalPoint"),
                //        DisplayName= texts1[i]
                //    });
                //}
            }
            {
                //var text = "WOMENS_BOTTOMS(\"女式下装\"), WOMENS_SWIMWEAR(\"女式泳装\"), WOMENS_OUTERWEAR(\"女士外套\"), WOMENS_JUMPSUITS(\"女士连体裤\"), WOMENS_BODAYSUITS(\"女士紧身衣\"), WOMENS_SKIRTS(\"女士短裙\"), WOMENS_TOPS(\"女上装\"), DRESSES(\"连衣裙\"), PLUS_SIZE_WOMENS_BOTTOMS(\"大码-女式下装\"), PLUS_SIZE_WOMENS_SWIMWEAR(\"大码-女式泳装\"), PLUS_SIZE_WOMENS_OUTERWAER(\"大码-女士外套\"), PLUS_SIZE_WOMENS_JUMPSUITS(\"大码-女士连体裤\"), PLUS_SIZE_WOMENS_BODAYSUITS(\"大码-女士紧身衣\"), PLUS_SIZE_WOMENS_SKIRTS(\"大码-女士短裙\"), PLUS_SIZE_WOMENS_TOPS(\"大码-女上装\"), PLUS_SIZE_DRESSES(\"大码-连衣裙\")";
                //var texts = text.SplitExt(",");
                //Dictionary<string, string> datas = new Dictionary<string, string>();
                //foreach (var t in texts)
                //{
                //    var ts = t.Replace("(\"", ",").Replace("\")", "").Split(',');
                //    {
                //        datas.Add(ts[0], ts[1]);
                //    }
                //}
                //string da = string.Empty;
                //int index = 0;
                //foreach (var t in datas)
                //{
                //    da = da + string.Format("[Display(Name = \"{0}\")]", t.Value)+"\r\n";
                //    da = da + string.Format("{0} = {1},\r\n", t.Key, index);
                //    index++;
                //}
            }

            //using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            //{
            //    System.Windows.Media.Imaging.BitmapImage bitmap = new System.Windows.Media.Imaging.BitmapImage();
            //    bitmap.StreamSource = ms;
            //    ms.Close();
            //   var a= Serialize(new SerializeTest() { PreviewImage = bitmap });
            //}
            //Microsoft.Win32.RegistryKey rk = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Internet Settings", true);

            ////设置代理可用
            //rk.SetValue("ProxyEnable", 1);
            ////设置代理IP和端口
            //rk.SetValue("ProxyServer", "127.0.0.1:8888");
            //rk.Close();
            //using (WebClient webClient = new WebClient())
            //{
            //    var webDatas = webClient.DownloadData("https://www.mzfort.com/items/CA29327-CA29328/3.jpg");
            //}

            {
                var text1 = "{\"display_name\":\"JADLOG_P_BR\",\"logistics_company\":\"JADLOG_P_BR_STD\",\"max_process_day\":15,\"min_process_day\":2,\"recommend_order\":0,\"service_name\":\"JADLOG_P_BR_STD\",\"tracking_no_regex\":\"[0-9]{14}$\"}";
                JObject pairs = JObject.Parse(text1);
                string ttt = string.Format("INSERT INTO [MemberAliLogisticsWays] ( [MemberId], [CategoryName], [LogisticsCompany], [ServiceName], [DisplayName], [MoreDisplayName], [TrackingNoRegex], [RecommendOrder], [MinProcessDay], [MaxProcessDay], [TrackingWebSite], [Remark], [UpdateTime]) VALUES(null,'','{0}','{0}','{1}','{0}','{2}',{3},{4},{5},NULL,NULL,GETDATE());",
                    pairs.Value<string>("logistics_company"), "JADLOG巴西境内包裹",pairs.Value<string>("tracking_no_regex"), pairs.Value<string>("recommend_order"), pairs.Value<string>("min_process_day"), pairs.Value<string>("max_process_day"));
            }


        }
        public static string Serialize<T>(T value)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
            using (MemoryStream ms = new MemoryStream())
            {
                serializer.WriteObject(ms, value);
                var result = Encoding.UTF8.GetString(ms.ToArray());
                return result;
            }
        }
    }
}
