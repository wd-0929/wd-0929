using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WheelTest.Style;

namespace WheelTest
{
    /// <summary>
    /// 属性转wpf的可通知属性.xaml 的交互逻辑
    /// </summary>
    public partial class 属性转wpf的可通知属性 : Window
    {
        public 属性转wpf的可通知属性()
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
            var text = textbox.Text; StringBuilder stringBuilder = new StringBuilder();
            if (_json.IsChecked == true)
            {
                stringBuilder.AppendLine("Dictionary<string, string> searchParams=new Dictionary<string, string>();");
                JObject jsonDatas= JObject.Parse(text);
                foreach (var item in jsonDatas) 
                {
                    var value = item.Value.Value<string>();
                    if (value == null)
                    {
                        stringBuilder.AppendLine(string.Format("searchParams.Add(\"{0}\", null);", item.Key));
                    }
                    else 
                    {

                        stringBuilder.AppendLine(string.Format("searchParams.Add(\"{0}\", \"{1}\");", item.Key, value));
                    }
                }
            }
            else if (_json1.IsChecked == true)
            {
                byte[] bytes = Convert.FromBase64String(text);
            }
            else
            {
                Regex regex = new Regex("(?<Description>/// <summary>[\\s\\S]+?/// </summary>)[\\s\\S]+?public[\\s]+?(?<type>[\\S]+)[\\s]+(?<name>[\\S]+)");
                if (regex.IsMatch(text))
                {
                    foreach (Match item in regex.Matches(text))
                    {
                        var name = item.Groups["name"].Value;
                        var type = item.Groups["type"].Value;
                        var Description = item.Groups["Description"].Value;
                        stringBuilder.AppendFormat("       #region {0} Property", name);
                        stringBuilder.AppendLine();
                        stringBuilder.AppendFormat(" private {1} _{0};", name, type);
                        stringBuilder.AppendLine();
                        stringBuilder.Append(Description);
                        stringBuilder.AppendLine();
                        stringBuilder.AppendFormat("public {1} {0} ", name, type.Trim());
                        stringBuilder.AppendLine();
                        stringBuilder.Append("{");
                        stringBuilder.AppendLine();
                        stringBuilder.Append(" get");
                        stringBuilder.AppendLine();
                        stringBuilder.Append("{");
                        stringBuilder.AppendLine();
                        stringBuilder.AppendFormat("return _{0};", name);
                        stringBuilder.AppendLine();
                        stringBuilder.Append(" }");
                        stringBuilder.AppendLine();
                        stringBuilder.Append("set");
                        stringBuilder.AppendLine();
                        stringBuilder.Append("{");
                        stringBuilder.AppendLine();
                        stringBuilder.AppendFormat("if (_{0} != value)", name);
                        stringBuilder.AppendLine();
                        stringBuilder.Append("{");
                        stringBuilder.AppendLine();
                        stringBuilder.AppendFormat(" _{0} = value;", name);
                        stringBuilder.AppendLine();
                        stringBuilder.AppendFormat("OnPropertyChanged(nameof({0}));", name);
                        stringBuilder.AppendLine();
                        stringBuilder.Append("}");
                        stringBuilder.AppendLine();
                        stringBuilder.Append("}");
                        stringBuilder.AppendLine();
                        stringBuilder.Append("}");
                        stringBuilder.AppendLine();
                        stringBuilder.AppendFormat("#endregion {0} Property", name);
                        stringBuilder.AppendLine();
                        stringBuilder.AppendLine();
                        stringBuilder.AppendLine();
                    }
                }
                regex = new Regex("public[\\s]+?(?<type>[\\S]+)[\\s]+(?<name>[\\S]+)");
                if (regex.IsMatch(text))
                {
                    foreach (Match item in regex.Matches(text))
                    {
                        var name = item.Groups["name"].Value;
                        stringBuilder.AppendLine(string.Format("{0} = pro.{1},", name, CapitalizeFirstLetter(name)));
                        //var name = CapitalizeFirstLetter(item.Groups["name"].Value);
                        //var type = item.Groups["type"].Value;
                        //stringBuilder.AppendFormat("       #region {0} Property", name);
                        //stringBuilder.AppendLine();
                        //stringBuilder.AppendFormat(" private {1} _{0};", name, type);
                        //stringBuilder.AppendLine();
                        //stringBuilder.AppendFormat("public {1} {0} ", name, type.Trim());
                        //stringBuilder.AppendLine();
                        //stringBuilder.Append("{");
                        //stringBuilder.AppendLine();
                        //stringBuilder.Append(" get");
                        //stringBuilder.AppendLine();
                        //stringBuilder.Append("{");
                        //stringBuilder.AppendLine();
                        //stringBuilder.AppendFormat("return _{0};", name);
                        //stringBuilder.AppendLine();
                        //stringBuilder.Append(" }");
                        //stringBuilder.AppendLine();
                        //stringBuilder.Append("set");
                        //stringBuilder.AppendLine();
                        //stringBuilder.Append("{");
                        //stringBuilder.AppendLine();
                        //stringBuilder.AppendFormat("if (_{0} != value)", name);
                        //stringBuilder.AppendLine();
                        //stringBuilder.Append("{");
                        //stringBuilder.AppendLine();
                        //stringBuilder.AppendFormat(" _{0} = value;", name);
                        //stringBuilder.AppendLine();
                        //stringBuilder.AppendFormat("OnPropertyChanged(nameof({0}));", CapitalizeFirstLetter(name));
                        //stringBuilder.AppendLine();
                        //stringBuilder.Append("}");
                        //stringBuilder.AppendLine();
                        //stringBuilder.Append("}");
                        //stringBuilder.AppendLine();
                        //stringBuilder.Append("}");
                        //stringBuilder.AppendLine();
                        //stringBuilder.AppendFormat("#endregion {0} Property", name);
                        //stringBuilder.AppendLine();
                        //stringBuilder.AppendLine();
                        //stringBuilder.AppendLine();
                    }
                }
            }
            textbox1.Text= stringBuilder.ToString();
        }
    }
}
