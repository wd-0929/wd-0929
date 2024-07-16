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
using static WheelTest.ListViewGrouping;

namespace WheelTest
{
    /// <summary>
    /// ListBoxGrouping.xaml 的交互逻辑
    /// </summary>
    public partial class ListBoxGrouping : Window
    {
        public ListBoxGrouping()
        {
            InitializeComponent();
            List<Info> arrays = new List<Info>();
            for (int i = 0; i < 4; i++)
            {
                arrays.Add(new Info { Name = "name" + i, GroupingId = i, Title = "title" + i });
            }
            Infos = arrays.ToArray();
        }

        public Info[] Infos
        {
            get { return (Info[])GetValue(InfosProperty); }
            set { SetValue(InfosProperty, value); }
        }

        public static readonly DependencyProperty InfosProperty =
            DependencyProperty.Register(nameof(Infos), typeof(Info[]), typeof(ListViewGrouping), new PropertyMetadata(null));

    }
}
