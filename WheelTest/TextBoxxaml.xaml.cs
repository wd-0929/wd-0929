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
    /// TextBoxxaml.xaml 的交互逻辑
    /// </summary>
    public partial class TextBoxxaml : Window
    {
        public TextBoxxaml()
        {
            InitializeComponent();
        }
        private void txtOnlyNumbers_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // 只允许数字输入
            if (!IsDigit(e.Text))
            {
                e.Handled = true; // 阻止非数字输入
            }
            else e.Handled = false;
        }

        private void txtOnlyNumbers_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            // 阻止特定按键，例如空格键
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
        }

        private static bool IsDigit(string input)
        {
            Regex regex = new Regex(@"[0-9]");
            // 检查字符串是否只包含数字
            if (regex.IsMatch(input)) 
            {
                return true;
            }
            return false;
        }
    }
}
