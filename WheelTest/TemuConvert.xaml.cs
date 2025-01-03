﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
            _index.Text = "3";
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var index = Convert.ToInt32(_index.Text);
            var text= textbox.Text;
            var texts=text.Split('\n');
            List<Data> infos = new List<Data>();
            for (int i = 0; i < texts.Length/ index; i++)
            {
                infos.Add(new Data
                {
                    Name = texts[i * index],
                    Type = texts[i * index + 1],
                    Required = texts[i * index + 2],
                    Synopsis = texts[i * index + index-1],
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
                StringBuilder stringBuilder1 = new StringBuilder();
                foreach (var item in infos)
                {
                    string type = "string";
                    switch (item.Type.Trim())
                    {
                        case "Number":
                        case "integer(int64)":
                            type = "long";
                            break;
                        case "integer(int32)":
                            type = "int";
                            break;
                        case "boolean":
                        case "Boolean":
                            type = "bool";
                            break;
                        case "String":
                        case "string":
                            type = "string";
                            break;
                        case "object":
                            type = "object";
                            break;
                        case "array":
                            type = "List<>";
                            break;
                        case "Object":
                            type = CapitalizeFirstLetter(item.Name);
                            break;
                        case "Object[]":
                            type = CapitalizeFirstLetter(item.Name)+"[]";
                            break;
                        case "string[]":
                        case "String[]":
                            type =  "string[]";
                            break;
                        case "Number[]":
                            type = "long[]";
                            break;
                        default:
                            type = item.Type.Trim();
                            break;
                    }
                    //stringBuilder.AppendFormat("       #region {0} Property", CapitalizeFirstLetter(item.Name));
                    //stringBuilder.AppendLine();
                    //stringBuilder.AppendFormat(" private {1} _{0};", item.Name, type);
                    //stringBuilder.AppendLine();
                    stringBuilder.Append("/// <summary>");
                    stringBuilder.AppendLine();
                    stringBuilder.AppendFormat("/// {0}", item.Synopsis.Trim());
                    stringBuilder.AppendLine();
                    stringBuilder.Append("/// </summary>");
                    stringBuilder.AppendLine();
                    if (_CapitalizeFirstLetter.IsChecked == true)
                        stringBuilder.AppendFormat("public {1} {0} ", CapitalizeFirstLetter(item.Name.Trim()), type.Trim());
                    else
                        stringBuilder.AppendFormat("public {1} {0} ", item.Name.Trim(), type.Trim());
                    stringBuilder.Append("   { get; set; }");
                    stringBuilder.AppendLine();
                    //stringBuilder.Append("{");
                    //stringBuilder.AppendLine();
                    //stringBuilder.Append(" get");
                    //stringBuilder.AppendLine();
                    //stringBuilder.Append("{");
                    //stringBuilder.AppendLine();
                    //stringBuilder.AppendFormat("return _{0};", item.Name);
                    //stringBuilder.AppendLine();
                    //stringBuilder.Append(" }");
                    //stringBuilder.AppendLine();
                    //stringBuilder.Append("set");
                    //stringBuilder.AppendLine();
                    //stringBuilder.Append("{");
                    //stringBuilder.AppendLine();
                    //stringBuilder.AppendFormat("if (_{0} != value)", item.Name);
                    //stringBuilder.AppendLine();
                    //stringBuilder.Append("{");
                    //stringBuilder.AppendLine();
                    //stringBuilder.AppendFormat(" _{0} = value;", item.Name);
                    //stringBuilder.AppendLine();
                    //stringBuilder.AppendFormat("OnPropertyChanged(nameof({0}));", CapitalizeFirstLetter(item.Name));
                    //stringBuilder.AppendLine();
                    //stringBuilder.Append("}");
                    //stringBuilder.AppendLine();
                    //stringBuilder.Append("}");
                    //stringBuilder.AppendLine();
                    //stringBuilder.Append("}");
                    //stringBuilder.AppendLine();
                    //stringBuilder.AppendFormat("#endregion {0} Property", CapitalizeFirstLetter(item.Name));
                    //stringBuilder.AppendLine();
                    //stringBuilder.AppendLine();
                    //stringBuilder.AppendLine();
                    stringBuilder1.AppendLine("apiUtils.AddApiParameter(\""+ item.Name.Trim() + "\", request."+ CapitalizeFirstLetter(item.Name.Trim()) + ");");
                }
                textbox1.Text = stringBuilder.ToString()/*+"\r\n"+ stringBuilder1.ToString()*/;
            }
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
    }
    public class Data 
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Required { get; set; }
        public string Synopsis { get; set; }
    }
}
