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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var text = textbox.Text;
            Regex regex = new Regex("(?<Description>/// <summary>[\\s\\S]+?/// </summary>)[\\s\\S]+?public[\\s]+?(?<type>[\\S]+)[\\s]+(?<name>[\\S]+)");
            StringBuilder stringBuilder = new StringBuilder();
            foreach (Match item in regex.Matches(text))
            {
                var name=item.Groups["name"].Value;
                var type=item.Groups["type"].Value;
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
            textbox1.Text= stringBuilder.ToString();
        }
    }
}
