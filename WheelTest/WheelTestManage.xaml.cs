using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WheelTest.Translate;

namespace WheelTest
{
    /// <summary>
    /// WheelTestManage.xaml 的交互逻辑
    /// </summary>
    public partial class WheelTestManage : Window
    {
        public WheelTestManage()
        {
            InitializeComponent();
        }

        private void Translate_Click(object sender, RoutedEventArgs e)
        {
            Translate.Translate.BaiDuTranslate("啊实打实");
        }

        private void AESdecrypt_Click(object sender, RoutedEventArgs e)
        {
            Translate.Translate.AesDecrypt();
        }

        private void HotKeysList_Click(object sender, RoutedEventArgs e)
        {
            Helper.ExecCommand(System.IO.Path.Combine(AppUtility.ExecPath, "HotKeysList.exe"), "/sxml "+ System.IO.Path.Combine(AppUtility.ExecPath, "HotKeysList.Txt"));
        }
    }
}
