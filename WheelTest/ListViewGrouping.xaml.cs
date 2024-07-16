using JSON.GTA.ControlLibrary;
using System;
using System.Collections.Generic;
using System.Windows;

namespace WheelTest
{
    /// <summary>
    /// ListViewGrouping.xaml 的交互逻辑
    /// </summary>
    public partial class ListViewGrouping : Window
    {
        public ListViewGrouping()
        {
            InitializeComponent();
            List<Info> arrays = dada();
            foreach (var item in arrays)
            {
                item.datas = dada().ToArray();
            }
            Infos = arrays.ToArray();
        }
        public List<Info> dada()
        {

            List<Info> arrays = new List<Info>();
            for (int i = 0; i < 4; i++)
            {

                arrays.Add(new Info { Name = "name" + i, GroupingId = i, Title = "title" + i });
            }
            return arrays;
        }

        public Info[] Infos
        {
            get { return (Info[])GetValue(InfosProperty); }
            set { SetValue(InfosProperty, value); }
        }

        public static readonly DependencyProperty InfosProperty =
            DependencyProperty.Register(nameof(Infos), typeof(Info[]), typeof(ListViewGrouping), new PropertyMetadata(null));

        public class Info
        {
            public string Name { get; set; }
            public string Title { get; set; }
            public int GroupingId { get; set; }
            public Info[] datas { get; set; }
        }

        private void SelectAll_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
