using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    /// TemuConvert.xaml 的交互逻辑
    /// </summary>
    public partial class TemuConvert : Window
    {
        public TemuConvert()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var text= textbox.Text;
            var texts=text.SplitExt("\r\n");
            List<Data> infos = new List<Data>();
            for (int i = 0; i < texts.Length/4; i++)
            {
                infos.Add(new Data
                {
                    Name = texts[i * 4],
                    Type = texts[i * 4 + 1],
                    Required = texts[i * 4 + 2],
                    Synopsis = texts[i * 4 + 3],
                });
            }
            //{
            //    StringBuilder stringBuilder = new StringBuilder();
            //    foreach (var item in infos)
            //    {
            //        stringBuilder.AppendFormat("{0}=orderDetail.{1},", CapitalizeFirstLetter(item.Name),item.Name); stringBuilder.AppendLine();
            //    }
            //    textbox1.Text = stringBuilder.ToString();
            //}
            {
                StringBuilder stringBuilder = new StringBuilder();
                foreach (var item in infos)
                {
                    string type = "string";
                    switch (item.Type)
                    {
                        case "integer(int64)":
                            type = "long";
                            break;
                        case "integer(int32)":
                            type = "int";
                            break;
                        case "boolean":
                            type = "bool";
                            break;
                        case "string":
                            type = "string";
                            break;
                        case "object":
                            type = "object";
                            break;
                        case "array":
                            type = "List<>";
                            break;
                        default:
                            throw new Exception("生成错误");
                    }
                    stringBuilder.AppendFormat("       #region {0} Property", CapitalizeFirstLetter(item.Name));
                    stringBuilder.AppendLine();
                    stringBuilder.AppendFormat(" private {1} _{0};", item.Name, type);
                    stringBuilder.AppendLine();
                    stringBuilder.Append("/// <summary>");
                    stringBuilder.AppendLine();
                    stringBuilder.AppendFormat("/// {0}", item.Synopsis);
                    stringBuilder.AppendLine();
                    stringBuilder.Append("/// </summary>");
                    stringBuilder.AppendLine();
                    stringBuilder.AppendFormat("public {1} {0}", CapitalizeFirstLetter(item.Name), type);
                    stringBuilder.AppendLine();
                    stringBuilder.Append("{");
                    stringBuilder.AppendLine();
                    stringBuilder.Append(" get");
                    stringBuilder.AppendLine();
                    stringBuilder.Append("{");
                    stringBuilder.AppendLine();
                    stringBuilder.AppendFormat("return _{0};", item.Name);
                    stringBuilder.AppendLine();
                    stringBuilder.Append(" }");
                    stringBuilder.AppendLine();
                    stringBuilder.Append("set");
                    stringBuilder.AppendLine();
                    stringBuilder.Append("{");
                    stringBuilder.AppendLine();
                    stringBuilder.AppendFormat("if (_{0} != value)", item.Name);
                    stringBuilder.AppendLine();
                    stringBuilder.Append("{");
                    stringBuilder.AppendLine();
                    stringBuilder.AppendFormat(" _{0} = value;", item.Name);
                    stringBuilder.AppendLine();
                    stringBuilder.AppendFormat("OnPropertyChanged(nameof({0}));", CapitalizeFirstLetter(item.Name));
                    stringBuilder.AppendLine();
                    stringBuilder.Append("}");
                    stringBuilder.AppendLine();
                    stringBuilder.Append("}");
                    stringBuilder.AppendLine();
                    stringBuilder.Append("}");
                    stringBuilder.AppendLine();
                    stringBuilder.AppendFormat("#endregion {0} Property", CapitalizeFirstLetter(item.Name));
                    stringBuilder.AppendLine();
                    stringBuilder.AppendLine();
                    stringBuilder.AppendLine();
                }
                textbox1.Text = stringBuilder.ToString();
            }
        }
        public string CapitalizeFirstLetter(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input; // 如果输入字符串为空或null，直接返回
            }

            return input[0].ToString().ToUpper() + input.Substring(1);
        }
    }
    public class Data 
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Required { get; set; }
        public string Synopsis { get; set; }
    }
}
