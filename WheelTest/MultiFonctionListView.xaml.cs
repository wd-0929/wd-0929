using JSON.GTA.Controls;
using System;
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

namespace WheelTest
{
    /// <summary>
    /// MultiFonctionListView.xaml 的交互逻辑
    /// </summary>
    public partial class MultiFonctionListView : Window
    {
        public MultiFonctionListView()
        {
            InitializeComponent();
            gridView1.Columns.Add(new MultiFonctionGridViewColumn()
            {
                Header = "测试",
                SortFields = "Values[" + 1 + "].ValueName",
                FilterField = "Values",
                Width = 100,
                FilterType = FilterType.Array,
                //FilterConverter = new JSON.GTA.Ali.Converter.CountryCnNameConverter(),
                DisplayMemberBinding = new Binding("Values[" + 1 + "].ValueName")
                {
                    //Converter = new JSON.GTA.Ali.Converter.CountryCnNameConverter()
                },
            });
        }
    }
}
