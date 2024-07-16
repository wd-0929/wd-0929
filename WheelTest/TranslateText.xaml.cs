using System;
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
                Dictionary<string,string> dic=new Dictionary<string, string> ();
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
    }
}
