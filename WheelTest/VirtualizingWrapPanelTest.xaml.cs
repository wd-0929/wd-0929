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
    /// VirtualizingWrapPanelTest.xaml 的交互逻辑
    /// </summary>
    public partial class VirtualizingWrapPanelTest : Window
    {
        public VirtualizingWrapPanelTest()
        {
            InitializeComponent();
            List<string> list = new List<string>();
            for (int i = 0; i < 1000000; i++)
            {
                list.Add("item"+i);
            }
            _values.ItemsSource= list;
        }
    }
}
