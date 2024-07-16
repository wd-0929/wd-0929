using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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
    /// DataGrid.xaml 的交互逻辑
    /// </summary>
    public partial class DataGrid : Window
    {
        public DataGrid()
        {
            InitializeComponent();
            List<DataGridInfo> dataGridInfos = new List<DataGridInfo>();
            for (int i = 0; i < 20; i++)
            {
                dataGridInfos.Add(new DataGridInfo { Name="Name"+i,Value= "Value"+i, Value1 = "Value" + i, Value2 = "Value" + i, Value3 = "Value" + i, });
            }
            dgQuestionTemplate.ItemsSource = dataGridInfos;
        }
    }
    public class DataGridInfo 
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public string Value1 { get; set; }
        public string Value2 { get; set; }
        public string Value3 { get; set; }
    }
}
