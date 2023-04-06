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

namespace WheelTest
{
    /// <summary>
    /// searchTextBox.xaml 的交互逻辑
    /// </summary>
    public partial class searchTextBox : Window
    {
        public searchTextBox()
        {
            InitializeComponent();
            _itemc.ItemsSource = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
