using System;
using System.Collections;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WheelTest
{
    /// <summary>
    /// MultipleComBoBox.xaml 的交互逻辑
    /// </summary>
    public partial class MultipleComBoBox : UserControl
    {
        public MultipleComBoBox()
        {
            InitializeComponent();
        }
        public IEnumerable Items
        {
            get { return (IEnumerable)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }
        // Using a DependencyProperty as the backing store for IsPreviewMode.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemsProperty =
            DependencyProperty.Register("Items", typeof(IEnumerable), typeof(MultipleComBoBox), new PropertyMetadata(null));
        public IEnumerable SelectedItems
        {
            get { return (IEnumerable)GetValue(SelectedItemsProperty); }
            set { SetValue(SelectedItemsProperty, value); }
        }
        // Using a DependencyProperty as the backing store for IsPreviewMode.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedItemsProperty =
            DependencyProperty.Register("SelectedItems", typeof(IEnumerable), typeof(MultipleComBoBox), new PropertyMetadata(OnSelectedItemsProperty));

        private static void OnSelectedItemsProperty(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox listBox = sender as ListBox;
            SelectedItems = listBox.SelectedItems;
        }
    }
}
