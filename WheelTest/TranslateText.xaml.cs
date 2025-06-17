using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using WheelTest.Style;

namespace WheelTest
{
    /// <summary>
    /// TranslateText.xaml 的交互逻辑
    /// </summary>
    public partial class TranslateText : Window
    {
        public TranslateText()
        {
            InitializeComponent();
        }
        public string CapitalizeFirstLetter(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input; // 如果输入字符串为空或null，直接返回
            }
            if (input.Contains("_"))
            {
                string strs = string.Empty;
                foreach (var item in input.SplitExt("_"))
                {
                    strs += CapitalizeFirstLetter(item);
                }
                return strs.Trim();
            }

            return (input[0].ToString().ToUpper() + input.Substring(1)).Trim();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_text.Text))
            {

            }
            else
            {
                var text = _text.Text;
                Dictionary<string, string> dic = new Dictionary<string, string>();
                var datas = text.SplitExt("\n");
                for (int i = 0; i < datas.Length / 2; i++)
                {
                    try
                    {

                        dic.Add(datas[i * 2], datas[i * 2 + 1]);
                    }
                    catch (Exception ex)
                    {

                    }
                }
                var textCn = dic.ToJsonData();
            }
        }

        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            //JObject jObject = JObject.Parse(_text.Text);
            //string str = Convert2(jObject);
            string str = Convert3(_text.Text);
        }
        /// <summary>
        /// 速卖通物流渠道
        /// </summary>
        /// <param name="jToken"></param>
        /// <returns></returns>
        public string Convert3(string jToken)
        {
            JArray jArray = JArray.Parse(jToken);
            List<string> strings = new List<string>();
            foreach (JObject s in jArray) 
            {
                strings.Add(string.Format("INSERT INTO [MemberAliLogisticsWays] ( [MemberId], [CategoryName], [LogisticsCompany], [ServiceName], [DisplayName], [MoreDisplayName], [TrackingNoRegex], [RecommendOrder], [MinProcessDay], [MaxProcessDay], [TrackingWebSite], [Remark], [UpdateTime]) VALUES(null,'{0}','{1}','{2}','{3}','{4}','{5}',{6},{7},{8},NULL,NULL,GETDATE());", s.Value<string>("categoryName"), s.Value<string>("logistics_company"), s.Value<string>("service_name"), s.Value<string>("display_name"), s.Value<string>("display_name"), s.Value<string>("tracking_no_regex"), s.Value<int>("recommend_order"), s.Value<int>("min_process_day"), s.Value<int>("max_process_day")));
            }
            return strings.ToJoin();
        }
        public string Convert2(JObject jToken)
        {
            List<string> strings = new List<string>();
            foreach (var item in jToken["data"].Value<JArray>("columnList"))
            {
                var key = CapitalizeFirstLetter(item.Value<string>("key"));
                var ss = "{Data = i."+ key + ", Day = i.AnalysisProductTime}";
                strings.Add($" {key} = DataItemInfo.GetDataItemInfo(AtPresentDataAnalysis.{key}, BeforeDataAnalysis.{key}, AtPresentData.Select(i => new DataItemByDay{ss}).ToList()),");
                //strings.Add(CapitalizeFirstLetter(item.Value<string>("key")+ "=AtPresentData.Sum(i=>i." + CapitalizeFirstLetter(item.Value<string>("key")))+"),");
                //string type = item.Value<string>("fieldType");
                //switch (type)
                //{
                //    case "BIGINT":
                //        type = "int";
                //        break;
                //    case "DOUBLE":
                //        type = "double";
                //        break;
                //    case "VARCHAR":
                //        type = "string";
                //        break;
                //    default:
                //        throw new Exception("1");
                //}
                //strings.Add(string.Format("productAnalysisInfo.{0}=mode.Value<{2}>(\"{1}\");", CapitalizeFirstLetter(item.Value<string>("key")), item.Value<string>("key"), type));
            }
            return strings.ToJoin();
        }
        public string Convert1(JObject jToken) 
        {
            List<string> strings = new List<string>();
            foreach (var item in jToken["data"].Value<JArray>("columnList"))
            {
                strings.Add(string.Format(" /// <summary>"));
                var title= item.Value<string>("title");
                if (title!= item.Value<string>("description")) 
                {
                    title= title+" ---- "+item.Value<string>("description");
                }
                strings.Add("///"+title);
                strings.Add(string.Format("/// </summary>"));
                string type = item.Value<string>("fieldType");
                switch (type)
                {
                    case "BIGINT":
                        type = "int";
                        break;
                    case "DOUBLE":
                        type = "double";
                        break;
                    case "VARCHAR":
                        type = "string";
                        break;
                    default:
                        throw new Exception("1");
                }
                strings.Add(string.Format("public {0} {1} ", type, CapitalizeFirstLetter(item.Value<string>("key")))+ "{ get; set; }");
            }
            return strings.ToJoin();
        }
        public string Convert(JToken jToken)
        {
            var a = new { a = new ArrayList() { new { } } };
            string str = "new { \r\n";
            foreach (JProperty item in jToken)
            {
                if (item.Value is Newtonsoft.Json.Linq.JArray)
                {
                    var array = item.Value as Newtonsoft.Json.Linq.JArray;

                    List<string> strings = new List<string>();
                    foreach (JToken item1 in array)
                    {
                        strings.Add(Convert(item1));
                    }
                    str += item.Name + " =  new ArrayList(){ \r\n" + strings.ToJoin(",\r\n") + "\r\n},\r\n";
                }
                else if (item.Value is Newtonsoft.Json.Linq.JObject) 
                {

                    var array = item.Value as Newtonsoft.Json.Linq.JToken;

                    str += item.Name + " =  " + Convert(array) + ",\r\n";
                }
                else
                {
                    switch (item.Value.Type)
                    {
                        case JTokenType.Integer:
                        case JTokenType.Float:
                        case JTokenType.Boolean:
                            str += item.Name + " = " + item.Value.ToString().ToLower() + ",\r\n";
                            break;
                        default:
                            str += item.Name + " = \"" + item.Value + "\",\r\n";
                            break;
                    }
                }
            }
            str = str + "\r\n}";
            return str;
        }

    }
}
