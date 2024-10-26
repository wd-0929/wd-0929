using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
    /// ErpClassGenerate.xaml 的交互逻辑
    /// </summary>
    public partial class ErpClassGenerate : Window
    {
        public ErpClassGenerate()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var text = this.textbox.Text;
            var path = System.IO.Path.Combine(AppUtility.ExecPath, "ClassDatas");
            try
            {
                Directory.Delete(path, true);
            }
            catch (Exception)
            {

            }

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            foreach (var item in text.SplitExt("\r\n"))
            {
                var text1 = item.Trim();
                var text2 = text1.Replace("Aliexpress", "");
                var Command = Resource1.Command.Replace("&", text2);
                var Handler = Resource1.Handler.Replace("&", text2).Replace("@", text1);
                var compath = System.IO.Path.Combine(path, text2 + "Command.cs");
                System.IO.File.WriteAllText(compath, Command);
                System.IO.File.WriteAllText(System.IO.Path.Combine(path, text2 + "Handler.cs"), Handler);
            }

            string commandLineArgs = $"/select, \"{path}\"";
            // 启动 Windows Explorer 并选中文件
            Process.Start("explorer.exe", commandLineArgs);
        }
    }
}
