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
            JToken jObject = JToken.Parse(_text.Text);
            string str = Convert(jObject);
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
