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
    /// ClassConvert.xaml 的交互逻辑
    /// </summary>
    public partial class ClassConvert : Window
    {
        public ClassConvert()
        {
            InitializeComponent();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(_TextBox.Text))
            {
                List<string> list = new List<string>();
                if (_TextBox.Text.Contains('.'))
                    foreach (var item in _TextBox.Text.Split('.'))
                    {
                        list.Add(item.FirstLetterToUpperWithRegex());
                    }
                if (_TextBox.Text.Contains('_'))
                    foreach (var item in _TextBox.Text.Split('_'))
                    {
                        list.Add(item.FirstLetterToUpperWithRegex());
                    }
                TextBox1.Text = list.ToJoin("");
                TextBox2.Text = TextBox1.Text + "Request";
                TextBox3.Text = TextBox1.Text + "Response";
                TextBox4.Text = "   public Task<"+ TextBox3.Text + "> "+ TextBox1.Text + "("+ TextBox2.Text + " request, string access_token = null)\r\n        {\r\n            string methodName = \"" + _TextBox.Text + "\";\r\n            return RequestAsync<"+ TextBox3.Text + ">(methodName, access_token, request);\r\n        }";
                TextBox3.Text = TextBox3.Text + ":BaseResponse<"+ TextBox3.Text + ".Result>";
            }
        }
    }
}
